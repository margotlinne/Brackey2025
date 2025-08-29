using System.Collections;
using UnityEngine;

namespace Margot
{
    [System.Serializable]
    public struct WaveConfig
    {
        public int batchSize;   // enemies per batch
        public int total;       // total enemies in this wave
    }

    public class WaveDirector : MonoBehaviour
    {
        [Header("References")]
        public EnemySpawner spawner;

        [Header("Procedural Wave Params")]
        [Tooltip("Total of wave 1 (base).")]
        public int startTotal = 12;

        [Tooltip("Total growth multiplier per wave (e.g., 1.20 = +20% each wave).")]
        public float totalGrowth = 1.20f;

        [Tooltip("Minimum batch size.")]
        public int minBatch = 3;

        [Tooltip("Maximum batch size.")]
        public int maxBatch = 8;

        [Tooltip("Increase batch size by +1 every N waves (0 = disabled).")]
        public int batchEveryNWaves = 2;

        public System.Action onWaveComplete;

        public bool running { get; private set; }
        int waveIndex;                 // 0-based
        Coroutine co;

        /// <summary>Start a wave (1-based index). Stops any ongoing run.</summary>
        public void Begin(int waveNumber)
        {
            waveIndex = Mathf.Max(0, waveNumber - 1);
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(RunWave(waveIndex));
        }

        IEnumerator RunWave(int idx)
        {
            running = true;

            if (spawner == null || spawner.spawnedEnemies == null)
            {
                Debug.LogError("[WaveDirector] EnemySpawner reference missing.");
                running = false;
                yield break;
            }

            int waveNumber = idx + 1;
            WaveConfig cfg = GetWaveConfig(waveNumber);

            // inform spawner about this wave's total
            spawner.BeginWave(cfg.total);

            int spawned = 0;

            // 1) First batch
            spawned += SpawnBatch(cfg.batchSize, waveNumber);

            while (spawned < cfg.total)
            {
                // 2) Wait until field is clear
                yield return new WaitUntil(() => OnField() == 0);

                // 3) Next batch (clamped to remaining)
                int remain = cfg.total - spawned;
                int next = Mathf.Min(cfg.batchSize, remain);
                spawned += SpawnBatch(next, waveNumber);
            }

            // final wait until field is clear
            yield return new WaitUntil(() => OnField() == 0);

            running = false;
            onWaveComplete?.Invoke();
        }

        /// <summary>Enemies currently on the field.</summary>
        int OnField()
        {
            return spawner != null && spawner.spawnedEnemies != null
                ? spawner.spawnedEnemies.Count
                : 0;
        }

        /// <summary>Spawn `count` enemies using wave-dependent type ratios.</summary>
        int SpawnBatch(int count, int waveNumber)
        {
            int spawnedNow = 0;

            for (int i = 0; i < count; i++)
            {
                string poolKey = PickPoolKeyByWave(waveNumber); // "RunEnemy" / "ChaseEnemy" / "ShootEnemy"
                var enemy = spawner.SpawnFromPool(poolKey);     // spawner handles position/list/callbacks
                if (enemy == null)
                {
                    Debug.LogWarning($"[WaveDirector] Failed to spawn from pool: {poolKey}");
                    continue;
                }
                spawnedNow++;
            }

            return spawnedNow;
        }

        /// <summary>Compute WaveConfig for a given waveNumber (1-based).</summary>
        WaveConfig GetWaveConfig(int waveNumber)
        {
            // total grows exponentially from startTotal
            int total = Mathf.Max(1,
                Mathf.RoundToInt(startTotal * Mathf.Pow(totalGrowth, waveNumber - 1)));

            // batch grows stepwise (+1 every N waves), clamped
            int batch = minBatch;
            if (batchEveryNWaves > 0)
                batch += (waveNumber - 1) / batchEveryNWaves;

            batch = Mathf.Clamp(batch, minBatch, maxBatch);
            batch = Mathf.Min(batch, total); // batch cannot exceed total

            return new WaveConfig { batchSize = batch, total = total };
        }

        // Convenience for other scripts (e.g., UI)
        public int TotalFor(int waveNumber) => GetWaveConfig(waveNumber).total;
        public int BatchFor(int waveNumber) => GetWaveConfig(waveNumber).batchSize;

        /// <summary>Pick pool key by wave-dependent type weights.</summary>
        string PickPoolKeyByWave(int w)
        {
            Vector3 wt = GetTypeWeights(w); // x=Runner, y=Chaser, z=Shooting
            float r = Random.value;
            if (r < wt.x) return "RunEnemy";
            if (r < wt.x + wt.y) return "ChaseEnemy";
            return "ShootEnemy";
        }

        /// <summary>Type ratios (Runner, Chaser, Shooting), normalized.</summary>
        Vector3 GetTypeWeights(int w)
        {
            if (w <= 2) return new Vector3(1f, 0f, 0f);              // waves 1–2: Runner only
            if (w <= 5) return new Vector3(0.7f, 0.3f, 0f);          // waves 3–5: Runner + Chaser
            if (w <= 9) return new Vector3(0.55f, 0.30f, 0.15f);     // waves 6–9: add Shooting

            // waves 10+: tilt Runner→Chaser, keep Shooting steady
            float t = Mathf.Clamp01((w - 10) / 20f);
            float runner = Mathf.Lerp(0.45f, 0.30f, t);
            float chaser = Mathf.Lerp(0.35f, 0.45f, t);
            float shooting = 0.25f;
            float sum = runner + chaser + shooting;
            return new Vector3(runner / sum, chaser / sum, shooting / sum);
        }
    }
}
