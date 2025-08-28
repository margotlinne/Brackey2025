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
        public WaveCountDown waveCountdown;
        
        Coroutine enemySpawnCoroutine;

        void Start()
        {
            currentWave = 1;
        }


        public void RouletteEvent()
        {
            if (isWaveDone)
            {
                GameManager.Instance.uiManager.OpenCanvas(UIManager.CanvasType.roulette);
            }
            isWaveDone = false;
        }

        /// <summary>
        /// Call this when roulette spinning is done
        /// </summary>
        public void NewWave()
        {
            StartCoroutine(OpenRouletteCanvas());
        }

        IEnumerator OpenRouletteCanvas()
        {
            yield return new WaitForSeconds(1f);

            GameManager.Instance.uiManager.CloseCanvas(UIManager.CanvasType.roulette);
            waveCountdown.StartAnimation();

            yield break;
        }
        

        /// <summary>
        /// It's called when countodwn animation is done
        /// </summary>
        public void StartWave()
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

            enemySpawnCoroutine = null;
            yield break;
        }

        public void WaveEnd()
        {
            Debug.Log("[WaveManager] wave done");
            isWaveDone = true;
            RouletteEvent();
        }
    }
}

