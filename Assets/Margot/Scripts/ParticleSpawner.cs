using System.Collections;
using UnityEngine;

namespace Margot
{
    public class ParticleSpawner : MonoBehaviour
    {
        public GameObject bloodParticlePrefab;
        public GameObject bulletParticlePrefab;

        void Start()
        {
            GameManager.Instance.poolManager.InitiatePool(bloodParticlePrefab, 100, "BloodParticle");
            GameManager.Instance.poolManager.InitiatePool(bulletParticlePrefab, 100, "BulletParticle");
        }

    }

}
