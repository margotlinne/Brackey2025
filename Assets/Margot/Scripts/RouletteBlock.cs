using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Margot
{
    public class RouletteBlock : SoundPlayer
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
        Sprite orgSprite;
        Image thisImage;
        public Button thisButton;

        void Awake()
        {
            thisImage = GetComponent<Image>();
            sr = iconImage.sprite;
            orgSprite = sr;
            if (GetComponentInChildren<TextMeshProUGUI>() != null && GetComponentInChildren<TextMeshProUGUI>().gameObject.tag == "Value")
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

            if (valueText != null) return;

            if (increaseStat)
            {
                int currentVal = GameManager.Instance.statManager.GetCurrentStat(type);

                if (type == StatManager.StatType.p_health && currentVal >= 50)
                {
                    notAvailable = true;
                    rewardValue = 0;
                }
                else
                {
                    rewardValue = currentVal + 1;
                }
            }
            else
            {
                int currentVal = GameManager.Instance.statManager.GetCurrentStat(type);

                if (currentVal - 1 <= 0)
                {
                    if (type == StatManager.StatType.p_health || type == StatManager.StatType.p_attackDamage || type == StatManager.StatType.p_attackSpeed)
                    {
                        isDeathBlock = true;
                        sr = deathIcon;
                        thisImage.color = Color.red;
                        rewardValue = 0;
                        if (valueText != null) valueText.text = "";
                    }
                    else
                    {
                        notAvailable = true;
                        rewardValue = 0;
                    }
                }
                else
                {
                    rewardValue = currentVal - 1;
                }
            }


            baseRewardValue = rewardValue;
        }

        void OnDisable()
        {
            notAvailable = false;
            isSelected = false;
            isDeathBlock = false;
            sr = orgSprite;
            iconImage.sprite = sr;
            if (!thisButton.gameObject.activeSelf) thisButton.gameObject.SetActive(true);
        }

        void Update()
        {
            if (notAvailable)
            {
                thisButton.interactable = false;
                return;
            }

            EnableButton(isSelected);

            if (valueText != null && GameManager.Instance.rouletteManager.isWheelSpinning)
                thisButton.gameObject.SetActive(false);

            if (isDeathBlock)
            {
                SetToDeathCard();
                return;
            }


            if (valueText != null)
            {
                valueText.text = rewardValue.ToString();
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

            if (isPositive) PlaySound(0);
            else PlaySound(1);
        }

        public void SetToDeathCard()
        {            
            // thisButton.interactable = true;
            iconImage.sprite = deathIcon;
            thisImage.color = Color.red;
            if (valueText != null) valueText.text = "";
        }
    }
}
