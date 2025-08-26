using System.Collections.Generic;
using UnityEngine;


namespace Margot
{
    public class PoolManager : MonoBehaviour
    {
        public Transform pools;
        public Dictionary<string, List<GameObject>> poolLists = new Dictionary<string, List<GameObject>>();


        public void InitiatePool(GameObject prefab, int amount, string name)
        {
            GameObject newPool = new GameObject();
            newPool.name = name + " Pool";
            newPool.transform.parent = pools;

            List<GameObject> poolList = new List<GameObject>();

            for (int i = 0; i < amount; i++)
            {
                GameObject newObj = Instantiate(prefab);
                newObj.SetActive(false);
                newObj.transform.parent = newPool.transform;
                poolList.Add(newObj);
            }
            poolLists[name] = poolList;
        }


        public GameObject TakeFromPool(string name)
        {
            GameObject targetObj = null;

            if (poolLists.TryGetValue(name, out var list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!list[i].activeSelf)
                    {
                        targetObj = list[i];
                        break;
                    }
                    else if (i == list.Count - 1)
                    {
                        Debug.LogWarning($"All items in {name} pool is being used. Will instantiate one now.");
                        GameObject newObj = Instantiate(list[i]);
                        list.Add(newObj);
                        targetObj = newObj;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError($"{name} pool does not exist!");
            }


            return targetObj;
        }

        public void ReturnToPool(string name, GameObject item)
        {
            if (poolLists.TryGetValue(name, out var list))
            {
                item.SetActive(false);
            }
            else
            {
                Debug.LogError($"{name} pool does not exist!");
            }
        }
    }

}
