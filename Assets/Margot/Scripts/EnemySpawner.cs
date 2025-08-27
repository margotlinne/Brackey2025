using UnityEngine;

namespace Margot
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] enemyPrefabs;



        private void Start()
        {
            foreach (var obj in enemyPrefabs)
            {
                GameManager.Instance.poolManager.InitiatePool(obj, 100, obj.GetComponent<Enemy>().enemyType.ToString() + "Enemy");
            }
        }


        public void SpawnEnemies()
        {
            GameObject enemy = GameManager.Instance.poolManager.TakeFromPool("ChaseEnemy");
            enemy.SetActive(true);
            enemy.transform.position = Vector3.zero;
        }
    }
}

