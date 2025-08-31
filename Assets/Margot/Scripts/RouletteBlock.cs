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
        public bool increaseStat = false;
        public bool isPositive = false;
        public bool isSelected = false;
        public bool isDeathBlock = false;
        public int code = 0;
        public bool notAvailable = false;

        [Header("UI")]
        public Image iconImage;
        public Sprite deathIcon;
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
            if (GetComponentInChildren<TextMeshProUGUI>() != null)
                valueText = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            InitValue();
        }

        void OnEnable()
        {
            if (GameManager.Instance == null) return;
            InitValue();
        }

        void InitValue()
        {
            notAvailable = false;

            // WheelBlockÀº °Ç³Ê¶Ù±â
            if (valueText != null) return;

            if (increaseStat)
            {
                rewardValue = GameManager.Instance.statManager.GetCurrentStat(type) + 1;
            }
            else
            {
                int currentVal = GameManager.Instance.statManager.GetCurrentStat(type);

                if (currentVal - 1 > 0)
                {
                    rewardValue = currentVal - 1;
                }
                else
                {
                    if (type == StatManager.StatType.p_health || type == StatManager.StatType.p_attackDamage)
                    {
                        isDeathBlock = true;
                        sr = deathIcon;
                        thisImage.color = Color.red;
                        rewardValue = 0;
                        if (valueText != null) valueText.text = "";
                    }
                    else if (type == StatManager.StatType.e_health)
                    {
                        notAvailable = true;
                        rewardValue = 0;
                    }
                }
            }

            baseRewardValue = rewardValue;
        }

        void OnDisable()
        {
            notAvailable = false;
            isSelected = false;
            isDeathBlock = false;
            if (!thisButton.gameObject.activeSelf) thisButton.gameObject.SetActive(true);
        }

        void Update()
        {
            if (notAvailable)
            {
                thisButton.interactable = false;
                return;
            }

            if (isDeathBlock)
            {
                SetToDeathCard();
                return;
            }

            EnableButton(isSelected);

            if (valueText != null)
            {
                valueText.text = rewardValue.ToString();
                if (GameManager.Instance.rouletteManager.isWheelSpinning)
                    thisButton.gameObject.SetActive(false);
            }

            thisImage.color = isPositive ? posColour : negColour;
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

        public void SetToDeathCard()
        {            
            thisButton.interactable = true;
            iconImage.sprite = deathIcon;
            thisImage.color = Color.red;
            if (valueText != null) valueText.text = "";
        }
    }
}
