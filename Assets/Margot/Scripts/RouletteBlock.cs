using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Margot
{
    public class RouletteBlock : MonoBehaviour
    {
        public StatManager.StatType type;
        public int rewardValue = 1;
        public int baseRewardValue = 0;
        public bool isPositive = false;
        public bool isSelected = false;
        public int code = 0;

        [Header("UI")]
        public Image iconImage;
        TextMeshProUGUI valueText;
        public Color posColour;
        public Color negColour;
        [HideInInspector] public Sprite sr;
        Image thisImage;
        public Button thisButton;
        

        void Awake()
        {
            thisImage = GetComponent<Image>();
            sr = iconImage.sprite;
            if (GetComponentInChildren<TextMeshProUGUI>() != null) valueText = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            if (isPositive) rewardValue = GameManager.Instance.statManager.GetCurrentStat(type) + 1;
            else if (GameManager.Instance.statManager.GetCurrentStat(type) - 1 > 0)
            {
                rewardValue = GameManager.Instance.statManager.GetCurrentStat(type) - 1;
            }
            baseRewardValue = rewardValue;
        }
        void Update()
        {
            EnableButton(isSelected);

            if (valueText != null)
            {
                valueText.text = rewardValue.ToString();
                if (GameManager.Instance.rouletteManager.isWheelSpinning) thisButton.gameObject.SetActive(false);
            }
            if (isPositive) thisImage.color = posColour;
            else thisImage.color = negColour;
        }

        void EnableButton(bool val)
        {
            thisButton.interactable = !val;
        }

        public void SelectThisBlock()
        {
            GameManager.Instance.rouletteManager.SelectBlock(this);
        }

        public void UnselectThisBlock()
        {
            GameManager.Instance.rouletteManager.UnselectBlock(this);
        }

        public void SelectEffect()
        {
            GetComponent<Animation>().Play();
        }
    }
}

