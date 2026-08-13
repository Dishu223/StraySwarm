using UnityEngine;
using UnityEngine.SceneManagement;

namespace StraySwarm.Core
{
    /// <summary>
    /// Initial bootloader in 0_Boot scene.
    /// Spawns all persistent core managers and transitions seamlessly to MainMenu.
    /// </summary>
    public class BootManager : MonoBehaviour
    {
        [Header("Persistent Manager Prefabs")]
        [SerializeField] private GameObject _audioManagerPrefab;
        [SerializeField] private GameObject _saveManagerPrefab;
        [SerializeField] private GameObject _objectPoolPrefab;
        [SerializeField] private GameObject _levelManagerPrefab;

        [Header("Scene Navigation")]
        [SerializeField] private string _firstSceneName = "1_MainMenu";
        [SerializeField] private float _splashDelay = 0.5f;

        private void Start()
        {
            InitializeManagers();
            Invoke(nameof(LoadFirstScene), _splashDelay);
        }

        private void InitializeManagers()
        {
            // Instantiate persistent singletons if they don't already exist
            if (Audio.AudioManager.Instance == null && _audioManagerPrefab != null)
            {
                Instantiate(_audioManagerPrefab);
            }

            if (Utils.SaveManager.Instance == null && _saveManagerPrefab != null)
            {
                Instantiate(_saveManagerPrefab);
            }

            if (Utils.ObjectPoolManager.Instance == null && _objectPoolPrefab != null)
            {
                Instantiate(_objectPoolPrefab);
            }

            if (LevelManager.Instance == null && _levelManagerPrefab != null)
            {
                Instantiate(_levelManagerPrefab);
            }

            Debug.Log("[BootManager] All persistent core services initialized.");
        }

        private void LoadFirstScene()
        {
            SceneManager.LoadScene(_firstSceneName);
        }
    }
}
