using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Image, Graphic

namespace Margot
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] enemyPrefabs;
        public List<GameObject> spawnedEnemies = new List<GameObject>();

        int totalCount = 0; // number spawned so far in current wave

        [Header("Spawn Area")]
        public BoxCollider2D spawnArea;      // assign in Inspector
        [Tooltip("Total enemies to spawn for the current wave. Can be set externally.")]
        public int targetTotalThisWave = 0;  // when totalCount reaches this, consider the wave complete

        [Header("Spawn Warmup")]
        [Tooltip("Seconds to keep enemy unable to attack after spawn.")]
        public float warmupSeconds = 2f;

        void Start()
        {
            foreach (var obj in enemyPrefabs)
            {
                GameManager.Instance.poolManager.InitiatePool(
                    obj, 100,
                    obj.GetComponent<Enemy>().enemyType.ToString() + "Enemy"
                );
            }
        }

        public void BeginWave(int total)
        {
            targetTotalThisWave = total;
            totalCount = 0;
            spawnedEnemies.Clear();
        }

        /// <summary>
        /// Spawn one enemy by pool key (e.g., "RunEnemy", "ChaseEnemy", "ShootEnemy").
        /// </summary>
        public GameObject SpawnFromPool(string poolKey)
        {
            if (targetTotalThisWave > 0 && totalCount >= targetTotalThisWave)
                return null;

            var enemy = GameManager.Instance.poolManager.TakeFromPool(poolKey);
            if (enemy == null)
            {
                Debug.LogWarning($"[EnemySpawner] Pool empty: {poolKey}");
                return null;
            }

            // Activate first (so OnEnable runs), then set transform.
            enemy.SetActive(true);

            var pos = spawnArea != null ? (Vector3)RandomPointIn(spawnArea) : Vector3.zero;
            enemy.transform.SetPositionAndRotation(pos, Quaternion.identity);

            var e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.UpdateStat();
                e.OnDeath -= RemovedEnemyFromSpawnList;
                e.OnDeath += RemovedEnemyFromSpawnList;

                e.EnableAttack(false);
                StartCoroutine(SpawnWarmupRoutine(enemy, e, warmupSeconds));
            }

            spawnedEnemies.Add(enemy);
            totalCount++;
            //PlaySound(1);
            return enemy;
        }

        public void RemovedEnemyFromSpawnList(GameObject enemy)
        {
            spawnedEnemies.Remove(enemy);

            if (spawnedEnemies.Count == 0 && targetTotalThisWave > 0 && totalCount == targetTotalThisWave)
            {
                GameManager.Instance.waveManager.WaveEnd();
                totalCount = 0;
            }
        }

        private static Vector2 RandomPointIn(BoxCollider2D box)
        {
            Vector2 half = box.size * 0.5f;
            Vector2 local = new Vector2(
                Random.Range(-half.x, half.x),
                Random.Range(-half.y, half.y)
            ) + box.offset;

            return box.transform.TransformPoint(local);
        }

        /// <summary>
        /// During warmup: set Image/SpriteRenderer alpha lower, canAttack=false.
        /// After warmup: restore alpha, canAttack=true.
        /// </summary>
        private IEnumerator SpawnWarmupRoutine(GameObject enemyGO, Enemy enemyComp, float seconds)
        {
           
            // wait
            yield return new WaitForSeconds(seconds);
            //PlaySound(0);

            // Enable attack
            if (enemyComp != null)
            {
                enemyComp.EnableAttack(true);
            }
        }
    }
}
