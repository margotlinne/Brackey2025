using System.Collections;
using UnityEngine;

namespace Margot
{
    public class ParticleBehaviour : MonoBehaviour
    {
        public enum ParticleType { Bullet, Blood }
        public ParticleType type;

        void OnEnable()
        {
            StartCoroutine(DisableThisParticle());
        }

        IEnumerator DisableThisParticle()
        {
            yield return new WaitForSeconds(1f);

            GameManager.Instance.poolManager.ReturnToPool(type.ToString() + "Particle", this.gameObject);
            yield break;
        }
    }

}
