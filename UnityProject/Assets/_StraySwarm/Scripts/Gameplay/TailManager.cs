using System.Collections.Generic;
using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Manages the entire conga line of collected animals.
    /// </summary>
    public class TailManager : MonoBehaviour
    {
        [Header("Tail Settings")]
        [Tooltip("How far apart each animal stands in the line.")]
        [SerializeField] private float _followerSpacing = 0.6f;
        
        [Tooltip("How smoothly they snap to the path (higher = snappier).")]
        [SerializeField] private float _followSpeed = 25f; 

        [Tooltip("Delay in seconds between each sequential animal delivery.")]
        [SerializeField] private float _deliveryStaggerDelay = 0.08f;
        
        [Header("Dependencies")]
        [SerializeField] private PathHistory _pathHistory;

        private List<FollowerBehavior> _tail = new List<FollowerBehavior>();

        private void Start()
        {
            if (_pathHistory == null) _pathHistory = GetComponent<PathHistory>();
        }

        private void Update()
        {
            if (_tail.Count == 0 || _pathHistory == null) return;

            // Update all followers
            for (int i = 0; i < _tail.Count; i++)
            {
                FollowerBehavior follower = _tail[i];
                
                // Index 0 is the first follower, right behind the player (1 * spacing)
                float targetDistanceBehind = (i + 1) * _followerSpacing;
                
                // Find exactly where on the breadcrumb trail they should be standing
                Vector3 targetPos = _pathHistory.GetPositionAtDistance(targetDistanceBehind);

                // Smoothly move the follower towards its exact spot
                follower.transform.position = Vector3.Lerp(
                    follower.transform.position, 
                    targetPos, 
                    Time.deltaTime * _followSpeed
                );
            }
        }

        public void AddFollower(FollowerBehavior animal)
        {
            if (!_tail.Contains(animal))
            {
                animal.Collect();
                _tail.Add(animal);
                
                // Play particle effect!
                if (JuiceManager.Instance != null)
                {
                    JuiceManager.Instance.PlayCollectParticle(animal.transform.position);
                }

                // Play custom animal species sound if assigned, otherwise play harmonic collect pop!
                if (Audio.AudioManager.Instance != null)
                {
                    if (animal.Data != null && animal.Data.CollectSound != null)
                    {
                        Audio.AudioManager.Instance.PlaySound(animal.Data.CollectSound);
                    }
                    else
                    {
                        Audio.AudioManager.Instance.PlayCollect();
                    }
                }
            }
        }
        
        // This triggers when the player walks over an animal (requires 2D Colliders!)
        private void OnTriggerEnter2D(Collider2D other)
        {
            FollowerBehavior follower = other.GetComponent<FollowerBehavior>();
            if (follower != null && !follower.IsCollected)
            {
                AddFollower(follower);
            }
        }

        private bool _isDelivering = false;

        /// <summary>
        /// Attempts to deliver matching animals from FRONT to BACK in the tail to a delivery crate.
        /// </summary>
        public void DeliverToCrate(DeliveryCrate crate)
        {
            if (_tail.Count == 0 || _isDelivering) return;
            StartCoroutine(DeliverToCrateRoutine(crate));
        }

        private System.Collections.IEnumerator DeliverToCrateRoutine(DeliveryCrate crate)
        {
            _isDelivering = true;

            // 1. Collect all matching animals from FRONT to BACK (first in tail goes first!)
            List<FollowerBehavior> matchingAnimals = new List<FollowerBehavior>();
            for (int i = 0; i < _tail.Count; i++)
            {
                var animal = _tail[i];
                if (animal != null)
                {
                    bool isMatch = (animal.Data != null && animal.Data.Type == crate.TargetAnimalType) || (animal.AnimalColor == crate.RequiredColor);
                    if (isMatch)
                    {
                        matchingAnimals.Add(animal);
                    }
                }
            }

            // 2. Deliver one-by-one in fast, rapid succession!
            foreach (var animal in matchingAnimals)
            {
                if (crate == null || crate.IsFull) break;

                if (_tail.Contains(animal))
                {
                    if (crate.TryAcceptAnimal(animal))
                    {
                        _tail.Remove(animal);
                        yield return new WaitForSeconds(_deliveryStaggerDelay);
                    }
                }
            }

            _isDelivering = false;
        }

        /// <summary>
        /// Attempts to deliver matching animals from FRONT to BACK in the tail to the waiting van.
        /// </summary>
        public void DeliverToVan(VanController van)
        {
            if (_tail.Count == 0 || _isDelivering) return;
            StartCoroutine(DeliverRoutine(van));
        }

        private System.Collections.IEnumerator DeliverRoutine(VanController van)
        {
            _isDelivering = true;

            // 1. Collect all matching animals from FRONT to BACK (first in tail goes first!)
            List<FollowerBehavior> matchingAnimals = new List<FollowerBehavior>();
            for (int i = 0; i < _tail.Count; i++)
            {
                if (_tail[i].AnimalColor == van.RequiredColor)
                {
                    matchingAnimals.Add(_tail[i]);
                }
            }

            // 2. Deliver them one-by-one in fast, rapid succession! (POP! POP! POP!)
            foreach (var animal in matchingAnimals)
            {
                if (van == null || van.IsFull || van.IsDrivingAway) break;

                if (_tail.Contains(animal))
                {
                    if (van.TryAcceptAnimal(animal))
                    {
                        _tail.Remove(animal);
                        yield return new WaitForSeconds(_deliveryStaggerDelay);
                    }
                }
            }

            _isDelivering = false;
        }
    }
}
