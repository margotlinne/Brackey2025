using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using NUnit.Framework.Internal;
using static Margot.StatManager;

namespace Margot
{
    public class BlockHoverImage : SoundPlayer, IPointerEnterHandler, IPointerExitHandler
    {
        public Transform block;  
        private Vector3 originalPos;
        private bool isHovered = false;

        public GameObject informationBox;
        public TextMeshProUGUI informationText;

        void Start()
        {
            informationBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySound(0, 0.2f);
            if (block == null || isHovered) return;

            isHovered = true;
            originalPos = block.localPosition;

            block.localPosition = new Vector3(
                originalPos.x,
                originalPos.y + 50f,
                originalPos.z
            );
                      

            string upOrDown = "";

            if (GetComponent<RouletteBlock>().increaseStat) 
                upOrDown = "Up";
            else upOrDown = "Down";

            switch (GetComponent<RouletteBlock>().type)
            {
                case StatType.e_attackDamage:
                    informationText.text = "Enemy Attack Damage " + upOrDown;
                    break;
                case StatType.e_attackSpeed:
                    informationText.text = "Enemy Attack Speed " + upOrDown;
                    break;
                case StatType.e_moveSpeed:
                    informationText.text = "Enemy Move Speed " + upOrDown;
                    break;
                case StatType.e_health:
                    informationText.text = "Enemy Max Health " + upOrDown;
                    break;
                case StatType.p_attackDamage:
                    informationText.text = "Player Attack Damage " + upOrDown;
                    break;
                case StatType.p_attackSpeed:
                    informationText.text = "Player Attack Speed " + upOrDown;
                    break;
                case StatType.p_moveSpeed:
                    informationText.text = "Player Move Speed " + upOrDown;
                    break;
                case StatType.p_health:
                    informationText.text = "Player Max Health " + upOrDown;
                    break;
            }

            informationBox.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlaySound(1, 0.2f);
            if (block == null || !isHovered) return;

            isHovered = false;
            block.localPosition = originalPos;

            informationBox.SetActive(false);
        }
        
    }
}
