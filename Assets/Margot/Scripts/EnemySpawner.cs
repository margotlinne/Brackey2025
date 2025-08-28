using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Margot
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] enemyPrefabs;
        public List<GameObject> spawnedEnemies = new List<GameObject>();

        private void Start()
        {
            foreach (var obj in enemyPrefabs)
            {
                GameManager.Instance.poolManager.InitiatePool(obj, 100, obj.GetComponent<Enemy>().enemyType.ToString() + "Enemy");
            }
        }

        public void SpawnEnemies()
        {
            GameObject enemy = GameManager.Instance.poolManager.TakeFromPool("ChaseEnemy");
            enemy.SetActive(true);
            enemy.GetComponent<Enemy>().UpdateStat();
            enemy.transform.position = Vector3.zero;
            spawnedEnemies.Add(enemy);
            enemy.GetComponent<Enemy>().OnDeath += RemovedEnemyFromSpawnList;
        }

        public void RemovedEnemyFromSpawnList(GameObject enemy)
        {
            spawnedEnemies.Remove(enemy);   

            if (spawnedEnemies.Count <= 0)
            {
                spawnedEnemies.Clear();
                GameManager.Instance.waveManager.WaveEnd();
            }
        }
    }
}

