using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Handles visual game juice: Particle effects, camera shake, and visual feedback!
    /// </summary>
    public class JuiceManager : MonoBehaviour
    {
        public static JuiceManager Instance { get; private set; }

        [Header("Particle Prefabs")]
        [SerializeField] private ParticleSystem _walkDustPrefab;
        [SerializeField] private ParticleSystem _collectParticlePrefab;
        [SerializeField] private ParticleSystem _deliverParticlePrefab;
        [SerializeField] private ParticleSystem _vanSmokePrefab;
        [SerializeField] private ParticleSystem _winConfettiPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Spawns a subtle white dust puff behind the character when moving.
        /// </summary>
        public void PlayWalkDust(Vector3 position)
        {
            if (_walkDustPrefab != null)
            {
                ParticleSystem ps = Instantiate(_walkDustPrefab, position, Quaternion.identity);
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        /// <summary>
        /// Spawns a sparkle burst when an animal is collected.
        /// </summary>
        public void PlayCollectParticle(Vector3 position)
        {
            if (_collectParticlePrefab != null)
            {
                ParticleSystem ps = Instantiate(_collectParticlePrefab, position, Quaternion.identity);
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        /// <summary>
        /// Spawns cartoon exhaust smoke puffs behind the van.
        /// </summary>
        public void PlayVanSmoke(Vector3 position)
        {
            if (_vanSmokePrefab != null)
            {
                ParticleSystem ps = Instantiate(_vanSmokePrefab, position, Quaternion.identity);
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        /// <summary>
        /// Spawns a ring burst when an animal enters the van.
        /// </summary>
        public void PlayDeliverParticle(Vector3 position)
        {
            if (_deliverParticlePrefab != null)
            {
                ParticleSystem ps = Instantiate(_deliverParticlePrefab, position, Quaternion.identity);
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        /// <summary>
        /// Spawns a festive shower of confetti across the screen on victory!
        /// </summary>
        public void PlayWinConfetti()
        {
            if (_winConfettiPrefab != null)
            {
                // Spawn at the top of the camera view!
                Vector3 camPos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
                camPos.z = 0; 
                camPos.y += 5f; // Start at the top of the screen so it falls down!
                
                ParticleSystem ps = Instantiate(_winConfettiPrefab, camPos, Quaternion.identity);
                
                // Set sorting order high so it draws over game tiles
                ParticleSystemRenderer psRenderer = ps.GetComponent<ParticleSystemRenderer>();
                if (psRenderer != null)
                {
                    psRenderer.sortingOrder = 100;
                }

                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }
    }
}
