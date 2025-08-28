using UnityEngine;

namespace Margot
{
    public class WaveCountDown : MonoBehaviour
    {
        Animation anim;

        void Awake()
        {
            anim = GetComponent<Animation>();   
        }

        public void StartAnimation()
        {
            anim.Play();
        }

        public void CountDownDone()
        {
            // reset animation
            anim.Stop();              
            anim["count_down"].time = 0f;        
            anim["count_down"].speed = 1f;        
            GameManager.Instance.waveManager.StartWave();
        }
    }

}
