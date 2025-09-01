using UnityEngine;

namespace Margot
{
    public class StatManager : MonoBehaviour
    {
        public enum StatType { e_attackDamage, e_attackSpeed, e_moveSpeed, e_health, p_attackDamage, p_attackSpeed, p_moveSpeed, p_health}
        [Header("Enemy")]
        public EnemyStats enemyStat;

        [Header("Player")]
        public PlayerStats playerStat;
        public Player player;

        private void Start()
        {
            InitStats();
        }

        private void InitStats() // initial stats
        {
            // set values in inspector
            //enemyStat.maxHealth = 10f;
            //enemyStat.attackDamage = 3f;
            //enemyStat.moveSpeed = 3f;
            //enemyStat.attackSpeedSPS = 0.5f;
            ////enemyStat.bulletsPerShot = 1;
          

            //playerStat.maxHealth = 30f;
            //playerStat.attackDamage = 5f;
            //playerStat.moveSpeed = 5f;
            //playerStat.attackSpeedSPS = 0.5f;
            //// playerStat.bulletsPerShot = 1;

            player.UpdateStat(true);
            GameManager.Instance.uiManager.UpdateStatUIText();
        }

        public int GetCurrentStat(StatType type)
        {
            int val = 0;

            switch (type)
            {
                case StatType.e_attackDamage:
                    val = Mathf.RoundToInt(enemyStat.attackDamage);
                    break;
                case StatType.e_attackSpeed:
                    val = Mathf.RoundToInt(enemyStat.attackSpeedSPS);
                    break;
                case StatType.e_moveSpeed:
                    val = Mathf.RoundToInt(enemyStat.moveSpeed);
                    break;
                case StatType.e_health:
                    val = Mathf.RoundToInt(enemyStat.maxHealth);
                    break;
                case StatType.p_attackDamage:
                    val = Mathf.RoundToInt(playerStat.attackDamage);
                    break;
                case StatType.p_attackSpeed:
                    val = Mathf.RoundToInt(playerStat.attackSpeedSPS);
                    break;
                case StatType.p_moveSpeed:
                    val = Mathf.RoundToInt(playerStat.moveSpeed);
                    break;
                case StatType.p_health:
                    val = Mathf.RoundToInt(playerStat.maxHealth);
                    break;
            }

            // Debug.Log("[StatManager] Get " + type + "stat: " + val);
            return val;
        }


        public void UpdateStats(StatType type, int value)
        {
            Debug.Log("[StatManager] Update Stat: " + type.ToString() + " type, value to : " + value.ToString());
            switch (type)
            {
                case StatType.e_attackDamage:
                    enemyStat.attackDamage = value;
                    break;
                case StatType.e_attackSpeed:
                    enemyStat.attackSpeedSPS = value;
                    break;
                case StatType.e_moveSpeed:
                    enemyStat.moveSpeed = value;
                    break;
                case StatType.e_health:
                    enemyStat.maxHealth = value;
                    break;
                case StatType.p_attackDamage:
                    playerStat.attackDamage = value;
                    player.UpdateStat(false);
                    break;
                case StatType.p_attackSpeed:
                    playerStat.attackSpeedSPS = value;
                    player.UpdateStat(false);
                    break;
                case StatType.p_moveSpeed:
                    playerStat.moveSpeed = value;
                    player.UpdateStat(false);
                    break;
                case StatType.p_health:
                    playerStat.maxHealth = value;
                    player.UpdateStat(true);
                    break;
            }

        }
    }
}

