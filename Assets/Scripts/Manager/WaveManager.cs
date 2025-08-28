using System.Collections;
using UnityEngine;

namespace Margot
{
    public class WaveManager : MonoBehaviour
    {
        public int currentWave;
        public bool isWaveDone = true;

        public EnemySpawner enemySpawner;
        public WaveCountDown waveCountdown;
        public WaveDirector waveDirector; 
        
        Coroutine enemySpawnCoroutine;

        void Start()
        {
            currentWave = 0;
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
            yield return new WaitForSeconds(0.5f);

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
            waveDirector.Begin(currentWave);
        }

        public void WaveEnd()
        {
            Debug.Log("[WaveManager] wave done");
            isWaveDone = true;
            RouletteEvent();
        }
    }
}

