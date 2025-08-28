using TMPro;
using UnityEngine;

namespace Margot
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI waveText;

        void Update()
        {
            waveText.text = "Wave " + GameManager.Instance.waveManager.currentWave.ToString();
        }
    }

}
