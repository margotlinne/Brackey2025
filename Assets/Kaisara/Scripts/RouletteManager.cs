
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Sound
{
    public class RouletteManager : Sounds
    {
        public List<Margot.RouletteBlock> rouletteBlocks = new List<Margot.RouletteBlock>();
        public Transform wheel;
        public Transform deckTransform;
        public Margot.RouletteBlock[] blocksInWheel;  // Slots in the wheel
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
        }

        /// <summary>
        /// Add a new block to the wheel and recalculate fill and rotation for all.
        /// </summary>
        public void SelectBlock(Margot.RouletteBlock rouletteBlock)
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
                    block.baseRewardValue = block.rewardValue  = rouletteBlock.rewardValue;
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

            float positiveRatio = (wheelBlockCount > 0) ? (float)positive / wheelBlockCount : 0f;

            if (positiveRatio < 0.5f)
            {
                foreach (var block in blocksInWheel)
                {
                    if (block.isPositive && block.gameObject.activeSelf)
                    {
                        if (block.increaseStat)
                        {
                            // 증가형 긍정 블럭 → 부정 블럭 수에 따라 강화
                            block.rewardValue += negative;
                            block.rewardValue = Mathf.Max(block.baseRewardValue, block.rewardValue - (positive - 1));
                        }
                        else
                        {
                            // 감소형 긍정 블럭 → -1 처리, 단 1 이하로는 안내려감
                            block.rewardValue = Mathf.Max(1, block.baseRewardValue - 1);
                        }
                    }
                }
            }
            else
            {
                foreach (var block in blocksInWheel)
                {
                    if (block.isPositive && block.gameObject.activeSelf)
                    {
                        if (block.increaseStat)
                        {
                            block.rewardValue = block.baseRewardValue;
                        }
                        else
                        {
                            block.rewardValue = Mathf.Max(1, block.baseRewardValue - 1);
                        }
                    }
                }
            }


        }

        /// <summary>
        /// Remove a block from the wheel, shift the remaining, 
        /// and recalculate fill and rotation.
        /// </summary>
        public void UnselectBlock(Margot.RouletteBlock rouletteBlock)
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

                // --- [추가 부분] 보정 로직 ---
                int positive = 0;
                int negative = 0;

                foreach (var b in blocksInWheel)
                {
                    if (b.gameObject.activeSelf)
                    {
                        if (b.isPositive) positive++;
                        else negative++;
                    }
                }

                float positiveRatio = (wheelBlockCount > 0) ? (float)positive / wheelBlockCount : 0f;

                if (positiveRatio < 0.5f)
                {
                    foreach (var b in blocksInWheel)
                    {
                        if (b.isPositive && b.gameObject.activeSelf)
                        {
                            b.rewardValue = b.baseRewardValue; // 초기화
                            b.rewardValue += negative;
                            b.rewardValue = Mathf.Max(b.baseRewardValue, b.rewardValue - (positive - 1));
                        }
                    }
                }
                else
                {
                    // 긍정 비율이 50% 이상일 때는 보정 없음, base 값으로 리셋
                    foreach (var b in blocksInWheel)
                    {
                        if (b.isPositive && b.gameObject.activeSelf)
                            b.rewardValue = b.baseRewardValue;
                    }
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





        IEnumerator SpinRoulette()
        {
            isWheelSpinning = true;
            deckTransform.gameObject.SetActive(false);

            float spinTime = Random.Range(3f, 4.5f);
            float elapsed = 0f;
            float spinSpeed = 720f;

            while (elapsed < spinTime)
            {
                wheel.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
                spinSpeed = Mathf.Lerp(720f, 0f, elapsed / spinTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            float wheelZ = wheel.eulerAngles.z;
            if (wheelZ > 180f) wheelZ -= 360f;

            Margot.RouletteBlock selectedBlock = null;

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
            if (selectedBlock.sr != selectedBlock.deathIcon) GameManager.Instance.waveManager.NewWave();
            else GameManager.Instance.uiManager.OpenCanvas(Margot.UIManager.CanvasType.gameover);
        }


    }

}