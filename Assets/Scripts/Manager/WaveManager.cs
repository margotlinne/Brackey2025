using System.Collections;
using UnityEngine;

namespace Margot
{
    public class WaveManager : MonoBehaviour
    {
        public int spawnedEnemyCount = 0;
        public int currentWave;
        public bool isWaveDone = true;

        public EnemySpawner enemySpawner;
        
        Coroutine enemySpawnCoroutine;

        void Start()
        {
            currentWave = 1;
        }


        public void RouletteEvent()
        {
            if (isWaveDone)
            {
                // roulette spinning...

                NewWave();
            }
            isWaveDone = false;

        }

        public void NewWave()
        {
            Debug.Log("[WaveManager] start new wave");
            currentWave++;

            if (enemySpawnCoroutine == null) StartCoroutine(SpawnEnemyThroughoutWave());
        }


        IEnumerator SpawnEnemyThroughoutWave()
        {
            do
            {
                Debug.Log("[WaveManager] spawn coroutine");
                enemySpawner.SpawnEnemies();
                spawnedEnemyCount++;
                yield return null;

            } while (spawnedEnemyCount > 0 && spawnedEnemyCount < 5);

            while (spawnedEnemyCount > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            Debug.Log("[WaveManager] wave done");
            isWaveDone = true;
            RouletteEvent();
            enemySpawnCoroutine = null;
            yield break;
        }

    }
}

