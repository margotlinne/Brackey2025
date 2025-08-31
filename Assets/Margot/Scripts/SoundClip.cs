using UnityEngine;

namespace Margot
{
    public class SoundClip : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on " + gameObject.name);
            }
        }

        private void Update()
        {
            if (audioSource == null) return;

            // Audio playback finished
            if (!audioSource.isPlaying && audioSource.time == 0f)
            {
                // Return AudioSource object to pool
                GameManager.Instance.poolManager.ReturnToPool("AudioSource", this.gameObject);
            }
        }

        /// <summary>
        /// Assign and play an audio clip
        /// </summary>
        //public void PlayClip(AudioClip clip, float volume = 1f)
        //{
        //    if (audioSource == null) return;

        //    audioSource.clip = clip;
        //    audioSource.volume = volume;
        //    audioSource.Play();
        //}
    }
}
