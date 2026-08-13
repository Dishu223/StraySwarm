using UnityEngine;
using UnityEngine.UI;

namespace StraySwarm.UI
{
    /// <summary>
    /// Controls the Settings modal popup (Audio sliders, Haptics, and Accessibility).
    /// </summary>
    public class SettingsUI : MonoBehaviour
    {
        [Header("Audio Sliders")]
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;

        [Header("Toggles")]
        [SerializeField] private Toggle _hapticsToggle;
        [SerializeField] private Toggle _colorblindToggle;

        private void OnEnable()
        {
            // Sync UI with current AudioManager settings
            if (Audio.AudioManager.Instance != null)
            {
                if (_sfxSlider != null) _sfxSlider.value = Audio.AudioManager.Instance.SFXVolume;
                if (_musicSlider != null) _musicSlider.value = Audio.AudioManager.Instance.MusicVolume;
            }

            // Hook up listeners
            if (_sfxSlider != null) _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            if (_musicSlider != null) _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        private void OnDisable()
        {
            if (_sfxSlider != null) _sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
            if (_musicSlider != null) _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        }

        public void OnSFXVolumeChanged(float value)
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.SetSFXVolume(value);
            }
        }

        public void OnMusicVolumeChanged(float value)
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.SetMusicVolume(value);
            }
        }

        public void OnCloseButtonClicked()
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            gameObject.SetActive(false);
        }
    }
}
