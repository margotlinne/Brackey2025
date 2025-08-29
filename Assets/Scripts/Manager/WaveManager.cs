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
            currentWave = 1;
        }


        public void RouletteEvent()
        {
            if (isWaveDone)
            {
                GameManager.Instance.uiManager.gameTimer.PauseTimer();
                GameManager.Instance.uiManager.OpenCanvas(UIManager.CanvasType.roulette);
                currentWave++;
            }
            isWaveDone = false;
        }

        /// <summary>
        /// Call this when roulette spinning is done
        /// </summary>
        public void NewWave()
        {
            StartCoroutine(CloseRouletteCanvas());
        }

        IEnumerator CloseRouletteCanvas()
        {
            yield return new WaitForSeconds(0.5f);

            GameManager.Instance.uiManager.CloseCanvas(UIManager.CanvasType.roulette);
            GameManager.Instance.uiManager.gameTimer.ResumeTimer();
            // waveCountdown.StartAnimation();
            StartWave(); // no countdown, straightly spawn enemies
            yield break;
        }
        

        /// <summary>
        /// It's called when countodwn animation is done
        /// </summary>
        public void StartWave()
        {
            Debug.Log("[WaveManager] start new wave");
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

