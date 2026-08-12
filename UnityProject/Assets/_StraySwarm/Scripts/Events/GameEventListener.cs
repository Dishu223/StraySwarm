using UnityEngine;
using UnityEngine.Events;

namespace StraySwarm.Events
{
    /// <summary>
    /// Attach to any GameObject to listen to a ScriptableObject GameEvent.
    /// Executes UnityEvents automatically when the event is raised.
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("The ScriptableObject event channel to listen to.")]
        [SerializeField] private GameEvent _event;

        [Tooltip("The UnityEvent responses to invoke when the event is raised.")]
        [SerializeField] private UnityEvent _response;

        private void OnEnable()
        {
            if (_event != null)
            {
                _event.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if (_event != null)
            {
                _event.UnregisterListener(this);
            }
        }

        public void OnEventRaised()
        {
            _response?.Invoke();
        }
    }
}
