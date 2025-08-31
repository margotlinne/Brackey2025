using UnityEngine;

namespace Margot
{
    public class MusicPlayer : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip introClip;  
        public AudioClip loopClip;   

        void Start()
        {
            audioSource.clip = introClip;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(PlayLoopAfterIntro());
        }

        private System.Collections.IEnumerator PlayLoopAfterIntro()
        {
            yield return new WaitForSeconds(introClip.length);
            audioSource.clip = loopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

}
