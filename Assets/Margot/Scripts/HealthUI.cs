using UnityEngine;
using UnityEngine.UI;

namespace Margot
{
    public class HealthUI : MonoBehaviour
    {
        public Image[] healthImages;
        public Sprite emptyHealthSprite;
        public Sprite fullHealthSprite;


        private void Update()
        {
            UpdateHealthUI();
        }


        public void UpdateHealthUI()
        {
            int maxHealthCount = Mathf.RoundToInt(GameManager.Instance.statManager.playerStat.maxHealth / 10f);
            int currentHealthCount = Mathf.RoundToInt(GameManager.Instance.statManager.player.GetComponent<Player>().currentHealth / 10f);
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
                        healthImage.sprite = emptyHealthSprite;
                    else healthImage.sprite = fullHealthSprite;
                }
            }
        }
    }

}
