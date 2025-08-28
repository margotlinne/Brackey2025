using UnityEngine;
using UnityEngine.UI;

public class RouletteWheel : MonoBehaviour
{
    [Header("Slot Buttons (4)")]
    public Button[] slotButtons; // Assign your 4 slot buttons

    [Header("Ability Buttons (9)")]
    public Button[] abilityButtons; // Assign your 9 ability buttons

    private int[] slotToAbility; // tracks which ability is in each slot (-1 if empty)

    private void Start()
    {
        slotToAbility = new int[slotButtons.Length];

        // Hook up ability button clicks
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            int index = i; // capture loop variable
            abilityButtons[i].onClick.AddListener(() => OnAbilityClicked(index));
        }

        // Hook up slot button clicks (reset slot)
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i;
            slotButtons[i].onClick.AddListener(() => OnSlotClicked(index));
        }

        ResetSlots();
    }

    private void OnAbilityClicked(int abilityIndex)
    {
        // Find the first empty slot
        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (slotToAbility[i] == -1) // empty slot
            {
                // Get color from ability button
                Color abilityColor = abilityButtons[abilityIndex].GetComponent<Image>().color;

                // Assign it to the slot
                slotButtons[i].GetComponent<Image>().color = abilityColor;

                // Track which ability is here
                slotToAbility[i] = abilityIndex;

                // Disable the ability button (can’t be picked again)
                abilityButtons[abilityIndex].interactable = false;

                return; // stop after filling one slot
            }
        }

        Debug.Log("No empty slots available!");
    }

    private void OnSlotClicked(int slotIndex)
    {
        if (slotToAbility[slotIndex] != -1) // if a slot is filled
        {
            int abilityIndex = slotToAbility[slotIndex];

            // Re-enable that ability button
            abilityButtons[abilityIndex].interactable = true;

            // Reset slot to empty
            slotButtons[slotIndex].GetComponent<Image>().color = Color.white;
            slotToAbility[slotIndex] = -1;
        }
    }

    public void ResetSlots()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            slotButtons[i].GetComponent<Image>().color = Color.white;
            slotToAbility[i] = -1;
        }

        // Re-enable all ability buttons
        foreach (Button ability in abilityButtons)
        {
            ability.interactable = true;
        }
    }
}
