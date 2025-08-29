using UnityEngine;

namespace Margot
{
    public class StatManager : MonoBehaviour
    {
        [Header("Enemy")]
        public EnemyStats chasingEnemyStat;
        public EnemyStats runningEnemyStat;
        public EnemyStats shootingEnemyStat;

        [Header("Player")]
        public PlayerStats playerStat;
        public Player player;

        private void Start()
        {
            InitStats();
        }

        private void InitStats() // initial stats
        {
            chasingEnemyStat.maxHealth = 10f;
            chasingEnemyStat.attackDamage = 3f;
            chasingEnemyStat.moveSpeed = 3f;
            chasingEnemyStat.attackSpeedSPS = null;
            chasingEnemyStat.bulletsPerShot = null;

            runningEnemyStat.maxHealth = 10f;
            runningEnemyStat.attackDamage = 3f;
            runningEnemyStat.moveSpeed = 3f;
            runningEnemyStat.attackSpeedSPS = null;
            runningEnemyStat.bulletsPerShot = null;

            shootingEnemyStat.maxHealth = 10f;
            shootingEnemyStat.attackDamage = 5f;
            shootingEnemyStat.moveSpeed = 3f;
            shootingEnemyStat.attackSpeedSPS = 0.5f;
            shootingEnemyStat.bulletsPerShot = 1;

            playerStat.maxHealth = 30f;
            playerStat.attackDamage = 5f;
            playerStat.moveSpeed = 10f;
            playerStat.attackSpeedSPS = 0.5f;
            shootingEnemyStat.bulletsPerShot = 1;

            player.UpdateStat();
        }
    }
}

