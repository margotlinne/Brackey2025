using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Margot
{
    public class SoundPlayer : MonoBehaviour
    {
        public AudioClip[] sounds;
        public SoundArrays[] randSound;
        private AudioSource audioSrc;
        private bool hasAudioSource = false;

        void Awake()
        {
            if (GetComponent<AudioSource>() != null)
            {
                hasAudioSource = true;
                audioSrc = GetComponent<AudioSource>();
            }
            else hasAudioSource = false;
        }


        public void PlaySound(int i, float volume = 1f, bool random = false, bool destroyed = false, float p1 = 0.9f, float p2 = 1.1f)
        {
            AudioClip clip = random ? randSound[i].soundArray[Random.Range(0, randSound[i].soundArray.Length)] : sounds[i];
            if (!hasAudioSource)
            {
                audioSrc = GameManager.Instance.poolManager.TakeFromPool("AudioSource").GetComponent<AudioSource>();
                audioSrc.gameObject.SetActive(true);
            }
            audioSrc.pitch = Random.Range(p1, p2);

            if (destroyed)
                AudioSource.PlayClipAtPoint(clip, transform.position, volume);
            else
                audioSrc.PlayOneShot(clip, volume);
        }
        [System.Serializable]
        public class SoundArrays
        {
            public AudioClip[] soundArray;
        }
    }

}
