using UnityEngine;
using UnityEngine.UI;

namespace Margot
{
    public class HealthUI : MonoBehaviour
    {
        public Image[] healthImages;

        private void Update()
        {
            UpdateHealthUI();
        }


        public void UpdateHealthUI()
        {
            int maxHealthCount = Mathf.RoundToInt(GameManager.Instance.statManager.playerStat.maxHealth);
            int currentHealthCount = Mathf.RoundToInt(GameManager.Instance.statManager.player.GetComponent<Player>().currentHealth);
            int lostHealthCount = maxHealthCount - currentHealthCount;  
            int count = 0;

            foreach (var item in healthImages)
            {
                if (count < maxHealthCount)
                {
                    item.gameObject.SetActive(true);
                    count++;
                }
                else
                {
                    item.gameObject.SetActive(false);
                }

            }


            for (int i = maxHealthCount - 1; i >= 0; i--)
            {
                Image healthImage = healthImages[i];
                {
                    if (i >= currentHealthCount)
                        healthImage.gameObject.SetActive(false);
                    else healthImage.gameObject.SetActive(true);
                }
            }
        }
    }

}
