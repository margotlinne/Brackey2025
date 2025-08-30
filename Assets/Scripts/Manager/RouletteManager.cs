using Mono.Cecil.Cil;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

namespace Margot
{
    public class RouletteManager : MonoBehaviour
    {
        public List<RouletteBlock> rouletteBlocks = new List<RouletteBlock>();
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

        public void SetRoulette() { }

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

                // Last inserted block gets the sprite
                if (i == wheelBlockCount - 1)
                {
                    block.iconImage.sprite = rouletteBlock.sr;
                    block.baseRewardValue = block.rewardValue  = rouletteBlock.rewardValue;
                    block.type = rouletteBlock.type;
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

            int positive = 0;
            int negative = 0;

            foreach (var block in blocksInWheel)
            {
                if (block.gameObject.activeSelf)
                {
                    if (block.isPositive) positive++;
                    else negative++;
                }
            }

            foreach (var block in blocksInWheel)
            {
                if (block.isPositive && block.gameObject.activeSelf)
                {
                    block.rewardValue += negative;

                    block.rewardValue = Mathf.Max(block.baseRewardValue, block.rewardValue - (positive - 1));
                }
            }
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
                    break;
                }
            }

            if (indexToRemove == -1)
            {
                Debug.LogError("[RouletteManager] There is no matching block");
                return;
            }

            // Shift blocks left to fill the empty slot
            for (int i = indexToRemove; i < wheelBlockCount - 1; i++)
            {
                blocksInWheel[i].iconImage.sprite = blocksInWheel[i + 1].iconImage.sprite;
                blocksInWheel[i].code = blocksInWheel[i + 1].code;
                blocksInWheel[i].type = blocksInWheel[i + 1].type;
                blocksInWheel[i].isPositive = blocksInWheel[i + 1].isPositive;
                blocksInWheel[i].rewardValue = blocksInWheel[i + 1].rewardValue;
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
            // Mark the original block as unselected in master list
            foreach (var block in rouletteBlocks)
            {
                if (block.code == rouletteBlock.code)
                {
                    Debug.Log("[RouletteManager] Return " + rouletteBlock.code + " block");
                    block.isSelected = false;
                    return;
                }
            }
        }

        public void SpinRouletteButton()
        {
            if (wheelBlockCount > 1)
            {
                int positiveCount = 0;

                foreach (var block in blocksInWheel)
                {
                    if (block.gameObject.activeSelf && block.isPositive)
                        positiveCount++;
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

        IEnumerator SpinRoulette()
        {
            isWheelSpinning = true;
            deckTransform.gameObject.SetActive(false);

            float spinTime = Random.Range(3f, 4.5f);
            float elapsed = 0f;
            float spinSpeed = 720f;
            RouletteBlock selectedBlock = null;

            while (elapsed < spinTime)
            {
                wheel.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
                spinSpeed = Mathf.Lerp(720f, 0f, elapsed / spinTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            float finalAngle = wheel.eulerAngles.z;
            if (finalAngle > 180f) finalAngle -= 360f;
            Debug.Log("Final Angle: " + finalAngle);

            float sliceAngle = 360f / wheelBlockCount;

            for (int i = 0; i < wheelBlockCount; i++)
            {
                float blockStart = blocksInWheel[i].transform.eulerAngles.z;
                if (blockStart > 180f) blockStart -= 360f;

                float blockEnd = blockStart - sliceAngle;

                if (finalAngle < blockStart && finalAngle >= blockEnd)
                {
                    selectedBlock = blocksInWheel[i + 1];
                    break;
                }
            }

            if (selectedBlock != null)
            {
                Debug.Log("Selected block: " + selectedBlock.code);
                GameManager.Instance.statManager.UpdateStats(selectedBlock.type, selectedBlock.rewardValue);
                selectedBlock.SelectEffect();
            }

            isWheelSpinning = false;
            yield return new WaitForSeconds(3f);
            GameManager.Instance.waveManager.NewWave();
        }

    }

}