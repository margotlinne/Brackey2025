using UnityEngine;
using UnityEngine.UI;

public class RouletteWheel : MonoBehaviour
{
    public Button[] slots;       // Assign 4 buttons in Inspector
    public Sprite[] abilityIcons; // 9 ability icons to pick from
    public Image[] slotImages;    // The Image component on each slot button

    private int selectedAbilityIndex = -1; // -1 means no ability selected

    private void Start()
    {
        // Assign click listeners to slots
        for (int i = 0; i < slots.Length; i++)
        {
            int index = i;
            slots[i].onClick.AddListener(() => OnSlotClick(index));
        }
    }

    // Call this from your 9 ability buttons
    public void SelectAbility(int abilityIndex)
    {
        selectedAbilityIndex = abilityIndex;
        Debug.Log("Selected Ability: " + abilityIndex);
    }

    private void OnSlotClick(int slotIndex)
    {
        if (slotImages[slotIndex].sprite != null)
        {
            // Remove ability
            slotImages[slotIndex].sprite = null;
        }
        else
        {
            // Add selected ability
            if (selectedAbilityIndex != -1)
            {
                // Optional: Prevent duplicates
                for (int i = 0; i < slotImages.Length; i++)
                {
                    if (i != slotIndex && slotImages[i].sprite == abilityIcons[selectedAbilityIndex])
                        return;
                }

                slotImages[slotIndex].sprite = abilityIcons[selectedAbilityIndex];
            }
        }
    }
}
