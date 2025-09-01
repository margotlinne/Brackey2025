using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Margot
{
    public class RouletteManager : SoundPlayer
    {
        public List<RouletteBlock> rouletteBlocks = new List<RouletteBlock>();
        public GameObject spinButtonObj;
        public GameObject helpButtonObj;
        public Transform wheel;
        public Transform deckTransform;
        public RouletteBlock[] blocksInWheel;  // Slots in the wheel
        public int wheelBlockCount = 0;        // Number of blocks currently in the wheel
        public bool isWheelSpinning = false;
        float rotateAmount = 0f;               // Used to calculate Z rotation of each block

        void Start()
        {
            ResetWheel();
        }

        /// <summary>
        /// Reset all wheel slots to inactive and clear counters.
        /// </summary>
        public void ResetWheel()
        {
            foreach (var block in blocksInWheel)
            {
                block.gameObject.SetActive(false);
            }

            wheelBlockCount = 0;
            rotateAmount = 0f;
        }

        public void SetRoulette()
        {
            deckTransform.gameObject.SetActive(true);
            helpButtonObj.SetActive(true);
            spinButtonObj.SetActive(true);
        }

        /// <summary>
        /// Add a new block to the wheel and recalculate fill and rotation for all.
        /// </summary>
        public void SelectBlock(RouletteBlock rouletteBlock)
        {
            if (wheelBlockCount >= blocksInWheel.Length) return;
            wheelBlockCount++;

            float fill = 1f / wheelBlockCount;
            rotateAmount = 0f; // reset rotation accumulator

            for (int i = 0; i < wheelBlockCount; i++)
            {
                var block = blocksInWheel[i];
                block.gameObject.SetActive(true);
                PlaySound(1);

                // Last inserted block gets the sprite
                if (i == wheelBlockCount - 1)
                {
                    block.iconImage.sprite = rouletteBlock.sr;
                    block.baseRewardValue =  rouletteBlock.baseRewardValue;
                    block.rewardValue = rouletteBlock.baseRewardValue;
                    block.type = rouletteBlock.type;
                    block.isDeathBlock = rouletteBlock.isDeathBlock;
                    block.increaseStat = rouletteBlock.increaseStat;
                    rouletteBlock.isSelected = true;
                    block.isPositive = rouletteBlock.isPositive;
                    block.code = rouletteBlock.code;
                }

                // Set fill amount
                var img = block.GetComponent<Image>();
                img.fillAmount = fill;

                // Apply rotation based on accumulated value
                block.transform.rotation = Quaternion.Euler(0, 0, rotateAmount);
                rotateAmount -= 360f * fill;
            }

            RecalculateRewards();

        }

        /// <summary>
        /// Remove a block from the wheel, shift the remaining, 
        /// and recalculate fill and rotation.
        /// </summary>
        public void UnselectBlock(RouletteBlock rouletteBlock)
        {
            int indexToRemove = -1;

            // Find the block to remove
            for (int i = 0; i < wheelBlockCount; i++)
            {
                if (blocksInWheel[i].code == rouletteBlock.code)
                {
                    indexToRemove = i;
                    PlaySound(0);
                    break;
                }
            }

            if (indexToRemove == -1)
            {
                Debug.LogError("[RouletteManager] There is no matching block");
                return;
            }

            // Mark the original block as unselected in master list
            foreach (var block in rouletteBlocks)
            {
                if (block.code == rouletteBlock.code)
                {
                    Debug.Log("[RouletteManager] Return " + rouletteBlock.code + " block");
                    block.isSelected = false;
                    break;
                }
            }

            // Shift blocks left to fill the empty slot
            for (int i = indexToRemove; i < wheelBlockCount - 1; i++)
            {
                blocksInWheel[i].iconImage.sprite = blocksInWheel[i + 1].iconImage.sprite;
                blocksInWheel[i].code = blocksInWheel[i + 1].code;
                blocksInWheel[i].type = blocksInWheel[i + 1].type;
                blocksInWheel[i].increaseStat = blocksInWheel[i + 1].increaseStat;
                blocksInWheel[i].isPositive = blocksInWheel[i + 1].isPositive;
                blocksInWheel[i].isDeathBlock = blocksInWheel[i + 1].isDeathBlock;
                blocksInWheel[i].rewardValue = blocksInWheel[i + 1].rewardValue;
                blocksInWheel[i].baseRewardValue = blocksInWheel[i + 1].baseRewardValue;
                blocksInWheel[i].gameObject.SetActive(blocksInWheel[i + 1].gameObject.activeSelf);
            }

            // Clear last block
            blocksInWheel[wheelBlockCount - 1].iconImage.sprite = null;
            blocksInWheel[wheelBlockCount - 1].gameObject.SetActive(false);

            wheelBlockCount--;

            // Recalculate fill amounts and rotations for remaining blocks
            if (wheelBlockCount > 0)
            {
                float fill = 1f / wheelBlockCount;
                rotateAmount = 0f;

                for (int i = 0; i < wheelBlockCount; i++)
                {
                    var block = blocksInWheel[i];
                    var img = block.GetComponent<Image>();
                    img.fillAmount = fill;

                    block.transform.rotation = Quaternion.Euler(0, 0, rotateAmount);
                    rotateAmount -= 360f * fill;
                }
            }
     

            // Recalculate fill amounts and rotations for remaining blocks
            if (wheelBlockCount > 0)
            {
                float fill = 1f / wheelBlockCount;
                rotateAmount = 0f;

                for (int i = 0; i < wheelBlockCount; i++)
                {
                    var block = blocksInWheel[i];
                    var img = block.GetComponent<Image>();
                    img.fillAmount = fill;

                    block.transform.rotation = Quaternion.Euler(0, 0, rotateAmount);
                    rotateAmount -= 360f * fill;
                }

            
            }

            RecalculateRewards();

        }

        public void SpinRouletteButton()
        {
            PlaySound(3);

            if (wheelBlockCount > 1)
            {
                int positiveCount = 0;

                foreach (var block in blocksInWheel)
                {
                    if (block.gameObject.activeSelf && block.isPositive)
                        positiveCount++;
                }

                if (positiveCount == 0)
                {
                    Debug.Log("[RouletteManager] Spin blocked: all blocks are negative.");
                    return;
                }

                if (positiveCount <= wheelBlockCount / 2)
                {
                    StartCoroutine(SpinRoulette());
                }
                else
                {
                    Debug.Log("[RouletteManager] Spin blocked: too many positive blocks.");
                }
            }
        }
        private void RecalculateRewards()
        {
            int negative = 0;
            int positive = 0;

            foreach (var b in blocksInWheel)
            {
                if (b.gameObject.activeSelf)
                {
                    if (b.isPositive) positive++;
                    else negative++;
                }
            }

            foreach (var b in blocksInWheel)
            {
                if (b.isPositive && b.gameObject.activeSelf)
                {
                    int value;

                    int effectiveNeg = Mathf.Max(0, negative - 1);
                    if (positive > 0)
                        effectiveNeg = Mathf.Max(0, (negative - 1) / positive);

                    if (b.increaseStat)
                        value = b.baseRewardValue + effectiveNeg;
                    else
                        value = Mathf.Max(1, b.baseRewardValue - effectiveNeg);

                    b.rewardValue = value;

                    Debug.Log($"[RecalculateRewards] code={b.code}, base={b.baseRewardValue}, neg={negative}, pos={positive}, reward={value}");
                }
            }
        }







        IEnumerator SpinRoulette()
        {
            PlaySound(2);
            helpButtonObj.SetActive(false);
            spinButtonObj.SetActive(false);
            isWheelSpinning = true;
            deckTransform.gameObject.SetActive(false);

            float spinTime = Random.Range(3f, 6f);
            float elapsed = 0f;
            float spinSpeed = Random.Range(720f, 760f);

            while (elapsed < spinTime)
            {
                wheel.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
                spinSpeed = Mathf.Lerp(720f, 0f, elapsed / spinTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            float wheelZ = wheel.eulerAngles.z;
            if (wheelZ > 180f) wheelZ -= 360f;

            RouletteBlock selectedBlock = null;

            if (wheelBlockCount > 0)
            {
                float[] angles = new float[wheelBlockCount];
                for (int i = 0; i < wheelBlockCount; i++)
                {
                    float localZ = blocksInWheel[i].transform.localEulerAngles.z;
                    if (localZ > 180f) localZ -= 360f;

                    float angle = localZ + wheelZ;
                    if (angle > 180f) angle -= 360f; 
                    angles[i] = angle;

                    Debug.Log(i + " " + angle);
                }

                int chosenIndex = -1;
                float minAbove = 9999f;
                float maxBelow = -9999f;

                for (int i = 0; i < wheelBlockCount; i++)
                {
                    if (angles[i] > -180f && angles[i] < minAbove)
                    {
                        minAbove = angles[i];
                        chosenIndex = i;
                    }
                }

                if (chosenIndex == -1)
                {
                    for (int i = 0; i < wheelBlockCount; i++)
                    {
                        if (angles[i] <= -180f && angles[i] > maxBelow)
                        {
                            maxBelow = angles[i];
                            chosenIndex = i;
                        }
                    }
                }

                if (chosenIndex != -1)
                    selectedBlock = blocksInWheel[chosenIndex];
            }

            if (selectedBlock != null)
            {
                Debug.Log("Selected block: " + selectedBlock.code);
                GameManager.Instance.statManager.UpdateStats(selectedBlock.type, selectedBlock.rewardValue);
                selectedBlock.SelectEffect();
            }

            isWheelSpinning = false;
            yield return new WaitForSeconds(3f);
            if (selectedBlock.isDeathBlock)
            {
                GameManager.Instance.uiManager.OpenCanvas(UIManager.CanvasType.gameover);
            }
            else GameManager.Instance.waveManager.NewWave();
        }


    }

}