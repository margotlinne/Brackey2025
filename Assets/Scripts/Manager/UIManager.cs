using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Margot
{
    public class UIManager : MonoBehaviour
    {
        public enum CanvasType { roulette }
        [Header("Text")]
        public TextMeshProUGUI waveText;

        [Header("Canvas")]
        public GameObject rouletteCanvas;

        private List<GameObject> canvasLists = new List<GameObject>();

        void Start()
        {
            canvasLists.Add(rouletteCanvas);
        }

        void Update()
        {
            waveText.text = "Wave " + GameManager.Instance.waveManager.currentWave.ToString();
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
                    break;
            }
        }
    }

}
