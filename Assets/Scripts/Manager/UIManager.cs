using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Margot
{
    public class UIManager : MonoBehaviour
    {
        public enum CanvasType { roulette, gameover }

        [Header("Text")]
        public TextMeshProUGUI waveText;
        public TextMeshProUGUI e_damageText;
        public TextMeshProUGUI e_dmSpeedText;
        public TextMeshProUGUI e_healthText;
        public TextMeshProUGUI e_mvSpeedText;
        public TextMeshProUGUI p_damageText;
        public TextMeshProUGUI p_dmSpeedText;
        public TextMeshProUGUI p_healthText;
        public TextMeshProUGUI p_mvSpeedText;

        [Header("Canvas")]
        public GameObject rouletteCanvas;
        public GameObject gameOverCanvas;
        public bool isCanvasOn = false;

        [Header("UI")]
        public GameTimer gameTimer;

        private List<GameObject> canvasLists = new List<GameObject>();

        void Start()
        {
            canvasLists.Add(rouletteCanvas);
            canvasLists.Add(gameOverCanvas);
        }

        void Update()
        {
            waveText.text = "Wave " + GameManager.Instance.waveManager.currentWave.ToString();
        }

        public void UpdateStatUIText()
        { 
           e_damageText.text = GameManager.Instance.statManager.enemyStat.attackDamage.ToString(); 
           e_dmSpeedText.text = GameManager.Instance.statManager.enemyStat.attackSpeedSPS.ToString();
           e_healthText.text = GameManager.Instance.statManager.enemyStat.maxHealth.ToString();
           e_mvSpeedText.text = GameManager.Instance.statManager.enemyStat.moveSpeed.ToString();


           p_damageText.text = GameManager.Instance.statManager.playerStat.attackDamage.ToString(); 
           p_dmSpeedText.text = GameManager.Instance.statManager.playerStat.attackSpeedSPS.ToString();
           p_healthText.text = GameManager.Instance.statManager.playerStat.maxHealth.ToString();
           p_mvSpeedText.text = GameManager.Instance.statManager.playerStat.moveSpeed.ToString();
        }

        public void OpenCanvas(CanvasType type)
        {
            switch (type)
            {
                case CanvasType.roulette:
                    foreach(var canvas in canvasLists)
                    {
                        if (canvas == rouletteCanvas)                        
                            canvas.SetActive(true);
                        else
                            canvas.SetActive(false);
                    }
                    isCanvasOn = true;
                    break;
                case CanvasType.gameover:
                    foreach (var canvas in canvasLists)
                    {
                        if (canvas == gameOverCanvas)
                            canvas.SetActive(true);
                        else
                            canvas.SetActive(false);
                    }
                    GameManager.Instance.isGameOver = true;
                    isCanvasOn = true;
                    break;
            }
        }

        public void CloseCanvas(CanvasType type)
        {
            switch (type)
            {
                case CanvasType.roulette:
                    foreach (var canvas in canvasLists)
                    {
                        if (canvas == rouletteCanvas)
                            canvas.SetActive(false);
                    }
                    isCanvasOn = false;
                    break;
                case CanvasType.gameover:
                    foreach (var canvas in canvasLists)
                    {
                        if (canvas == gameOverCanvas)
                            canvas.SetActive(false);
                    }
                    isCanvasOn = false;
                    break;
            }
        }
    }

}
