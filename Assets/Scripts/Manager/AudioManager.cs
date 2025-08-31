using UnityEngine;

namespace Margot
{
    public class AudioManager : MonoBehaviour
    {
        public GameObject audioSourcePrefab;

        void Start()
        {
            GameManager.Instance.poolManager.InitiatePool(audioSourcePrefab, 50, "AudioSource");
        }



    }
}

