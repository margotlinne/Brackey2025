using System.Collections;
using UnityEngine;

namespace Margot
{
    public class WaveDirector : MonoBehaviour
    {
        [Header("References")]
        public EnemySpawner spawner;    // Uses EnemySpawner's list/removal callback

        [Header("Spawn Rules")]
        public int initialBurst = 5;      // How many to spawn immediately at wave start
        public int flowPerTick = 2;      // How many to enqueue per spawn interval
        public float baseInterval = 3.00f;  // Initial interval between spawn ticks
        public float minInterval = 2.25f;  // Minimum interval as waves progress
        public int lerpToMinAtWave = 20;     // Wave number by which interval reaches minInterval

        [Header("Concurrency (on-field cap)")]
        public int baseConcurrency = 10;     // Max on-field enemies at wave 1
        public int concurrencyPerWave = 2;      // +cap per wave
        public int maxConcurrency = 50;     // Absolute cap

        [Header("Budget (total spawns per wave)")]
        public int baseBudget = 30;         // Total spawns for wave 1
        public float budgetGrowth = 1.20f;      // Growth factor per wave (e.g., ×1.20)

        // Optional: external notification (use only if you want Director to signal completion)
        public System.Action onWaveComplete;

        public bool running { get; private set; }

        int wave;       // Current wave number
        int queued;     // Pending spawns waiting to be flushed (respecting concurrency)
        int budget;     // Remaining spawns for this wave
        float interval; // Spawn interval for this wave
        Coroutine co;

        /// <summary>
        /// Starts a new wave with the given number. Stops any previous run.
        /// </summary>
        public void Begin(int waveNumber)
        {
            wave = waveNumber;
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            running = true;

            // Compute per-wave caps and pacing
            int concurrencyCap = Mathf.Min(baseConcurrency + concurrencyPerWave * (wave - 1), maxConcurrency);

            float t = Mathf.Clamp01((wave - 1) / Mathf.Max(1f, (float)lerpToMinAtWave));
            interval = Mathf.Lerp(baseInterval, minInterval, t);

            budget = Mathf.RoundToInt(baseBudget * Mathf.Pow(budgetGrowth, wave - 1));
            queued = 0;

            // Initial burst at wave start
            Enqueue(initialBurst);

            float timer = 0f;

            while (true)
            {
                timer += Time.deltaTime;

                // Periodic enqueue (e.g., every `interval`, add `flowPerTick` to queue)
                if (budget > 0 && timer >= interval)
                {
                    timer -= interval;
                    Enqueue(flowPerTick);
                }

                // Move from queue → actual spawns (respecting on-field concurrency)
                Flush(concurrencyCap);

                // Exit when the wave is fully spent and the field is clear:
                //   - no budget left, no queued spawns, and no enemies on field
                if (budget <= 0 && queued <= 0 && OnField() == 0)
                    break;

                yield return null;
            }

            running = false;

            // Optional: notify listeners. If EnemySpawner already ends the wave,
            // you may leave this unassigned to avoid duplicate signaling.
            onWaveComplete?.Invoke();
        }

        /// <summary>
        /// Returns current number of enemies on the field (from EnemySpawner).
        /// </summary>
        int OnField()
        {
            return (spawner != null && spawner.spawnedEnemies != null)
                ? spawner.spawnedEnemies.Count
                : 0;
        }

        /// <summary>
        /// Adds up to `count` spawns to the queue, clamped by remaining budget.
        /// </summary>
        void Enqueue(int count)
        {
            int add = Mathf.Min(count, budget);
            if (add <= 0) return;
            queued += add;
            budget -= add;
        }

        /// <summary>
        /// Converts queued spawns into actual enemies, without exceeding on-field cap.
        /// Pulls from object pools directly and registers to EnemySpawner bookkeeping.
        /// </summary>
        void Flush(int concurrencyCap)
        {
            if (spawner == null) return;

            int canSpawn = Mathf.Max(0, concurrencyCap - OnField());
            if (canSpawn <= 0 || queued <= 0) return;

            int toSpawn = Mathf.Min(queued, canSpawn);

            for (int i = 0; i < toSpawn; i++)
            {
                // Choose pool key by wave-specific type weights
                string poolKey = PickPoolKeyByWave(wave); // "RunEnemy"/"ChaseEnemy"/"ShootEnemy"

                // Pull from pool and activate
                GameObject enemy = GameManager.Instance.poolManager.TakeFromPool(poolKey);
                enemy.SetActive(true);

                // Basic initialization
                var e = enemy.GetComponent<Enemy>();
                e.UpdateStat();

                // TODO: Replace with your spawn-point logic later
                enemy.transform.position = Vector3.zero;

                // Register to EnemySpawner for tracking and removal callback
                spawner.spawnedEnemies.Add(enemy);
                e.OnDeath += spawner.RemovedEnemyFromSpawnList;
            }

            queued -= toSpawn;
        }

        /// <summary>
        /// Returns a pool key based on wave-dependent type weights.
        /// Pool names must exactly match:
        ///   "RunningEnemy", "ChasingEnemy", "ShootingEnemy"
        /// </summary>
        string PickPoolKeyByWave(int w)
        {
            Vector3 wt = GetTypeWeights(w); // x=Runner, y=Chaser, z=Shooting
            float r = Random.value;
            if (r < wt.x) return "RunEnemy";
            if (r < wt.x + wt.y) return "ChaseEnemy";
            return "ShootEnemy";
        }

        /// <summary>
        /// Wave-dependent weights for (Runner, Chaser, Shooting).
        /// Sums to ~1.0 after normalization.
        /// </summary>
        Vector3 GetTypeWeights(int w)
        {
            if (w <= 2) return new Vector3(1f, 0f, 0f);           // Waves 1–2: Runner only
            if (w <= 5) return new Vector3(0.7f, 0.3f, 0f);           // Waves 3–5: Add Chaser
            if (w <= 9) return new Vector3(0.55f, 0.30f, 0.15f);        // Waves 6–9: Add Shooting

            // 10+: gradually tilt from Runner→Chaser; keep Shooting steady
            float t = Mathf.Clamp01((w - 10) / 20f);
            float runner = Mathf.Lerp(0.45f, 0.30f, t);
            float chaser = Mathf.Lerp(0.35f, 0.45f, t);
            float shooting = 0.25f;
            float sum = runner + chaser + shooting;
            return new Vector3(runner / sum, chaser / sum, shooting / sum);
        }
    }
}
