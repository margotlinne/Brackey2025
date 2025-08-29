using TMPro;
using UnityEngine;

namespace Margot
{
    public class GameTimer : MonoBehaviour
    {
        TextMeshProUGUI timerText;

        private float elapsedTime = 0f;
        private bool isPaused = false;

        void Awake()
        {
            timerText = GetComponent<TextMeshProUGUI>();    
        }

        void Start()
        {
            PauseTimer();
        }

        private void Update()
        {
            if (!isPaused)
            {
                elapsedTime += Time.deltaTime;

                int minutes = Mathf.FloorToInt(elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);

                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }

        public void ResetTimer()
        {
            elapsedTime = 0f;
            UpdateText();
        }

        public void PauseTimer()
        {
            isPaused = true;
        }

        public void ResumeTimer()
        {
            isPaused = false;
        }

        private void UpdateText()
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}

