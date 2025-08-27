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
                GameManager.Instance.poolManager.InitiatePool(obj, 100, obj.name);
            }
        }
    }
}

