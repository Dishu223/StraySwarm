using System.Collections;
using UnityEngine;

namespace StraySwarm.Audio
{
    /// <summary>
    /// Production-grade Audio Manager for Stray Swarm.
    /// Handles BGM crossfading, pitch-shifted SFX, ascending combo scales, and volume control.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Sound Effects (SFX)")]
        public AudioClip CollectSound;
        public AudioClip DeliverSound;
        public AudioClip CrateFullSound;
        public AudioClip WallBreakSound;
        public AudioClip Star1Sound;
        public AudioClip Star2Sound;
        public AudioClip Star3Sound;
        public AudioClip WinSound;
        public AudioClip LoseSound;
        public AudioClip ButtonClickSound;
        public AudioClip TimerWarningSound;

        [Header("Music (BGM)")]
        public AudioClip MenuBGM;
        public AudioClip GameplayBGM;

        [Header("Volume Settings")]
        [Range(0f, 1f)] public float MasterVolume = 1f;
        [Range(0f, 1f)] public float SFXVolume = 1f;
        [Range(0f, 1f)] public float MusicVolume = 0.7f;

        private AudioSource _sfxSource;
        private AudioSource _musicSourceA;
        private AudioSource _musicSourceB;
        private bool _isSourceAActive = true;

        private float _lastCollectTime = 0f;
        private int _currentComboPitchStep = 0;
        private readonly float[] _pentatonicPitchScale = { 1.0f, 1.122f, 1.26f, 1.414f, 1.587f, 1.782f, 2.0f }; // C, D, E, G, A, C

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                _sfxSource = gameObject.AddComponent<AudioSource>();
                _sfxSource.playOnAwake = false;

                _musicSourceA = gameObject.AddComponent<AudioSource>();
                _musicSourceA.loop = true;
                _musicSourceA.playOnAwake = false;

                _musicSourceB = gameObject.AddComponent<AudioSource>();
                _musicSourceB.loop = true;
                _musicSourceB.playOnAwake = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (GameplayBGM != null)
            {
                PlayGameplayMusic();
            }
        }

        public void PlayGameplayMusic()
        {
            if (GameplayBGM != null)
            {
                PlayMusic(GameplayBGM, 0.5f);
            }
        }

        public void PlayMenuMusic()
        {
            if (MenuBGM != null)
            {
                PlayMusic(MenuBGM, 0.5f);
            }
        }

        // --- SFX PLAYBACK ---
        public void PlaySound(AudioClip clip, float pitch = 1f, float volumeMultiplier = 1f)
        {
            if (clip == null || _sfxSource == null) return;

            _sfxSource.pitch = pitch;
            _sfxSource.PlayOneShot(clip, MasterVolume * SFXVolume * volumeMultiplier);
        }

        /// <summary>
        /// Plays collect sound with an escalating musical pitch scale for rapid combos!
        /// </summary>
        public void PlayCollect()
        {
            float timeSinceLast = Time.time - _lastCollectTime;
            _lastCollectTime = Time.time;

            if (timeSinceLast < 1.2f)
            {
                _currentComboPitchStep = Mathf.Min(_currentComboPitchStep + 1, _pentatonicPitchScale.Length - 1);
            }
            else
            {
                _currentComboPitchStep = 0;
            }

            float pitch = _pentatonicPitchScale[_currentComboPitchStep];
            PlaySound(CollectSound, pitch);
        }

        public void PlayDeliver() => PlaySound(DeliverSound, Random.Range(0.95f, 1.05f));
        public void PlayCrateFull() => PlaySound(CrateFullSound, 1f);
        public void PlayWallBreak() => PlaySound(WallBreakSound, Random.Range(0.9f, 1.1f));
        public void PlayStar(int starIndex)
        {
            if (starIndex == 0) PlaySound(Star1Sound ?? CollectSound, 1.0f);
            else if (starIndex == 1) PlaySound(Star2Sound ?? CollectSound, 1.25f);
            else PlaySound(Star3Sound ?? CollectSound, 1.5f);
        }
        public void PlayWin() => PlaySound(WinSound, 1f);
        public void PlayLose() => PlaySound(LoseSound, 1f);
        public void PlayButtonClick() => PlaySound(ButtonClickSound, 1f);
        public void PlayTimerWarning() => PlaySound(TimerWarningSound, 1f, 0.6f);

        // --- BGM CROSSFADE ---
        public void PlayMusic(AudioClip newTrack, float fadeDuration = 1.0f)
        {
            if (newTrack == null) return;

            AudioSource activeSource = _isSourceAActive ? _musicSourceA : _musicSourceB;
            AudioSource fadingSource = _isSourceAActive ? _musicSourceB : _musicSourceA;

            if (activeSource.clip == newTrack && activeSource.isPlaying) return;

            StopAllCoroutines();
            StartCoroutine(CrossfadeMusicRoutine(activeSource, fadingSource, newTrack, fadeDuration));
            _isSourceAActive = !_isSourceAActive;
        }

        private IEnumerator CrossfadeMusicRoutine(AudioSource incoming, AudioSource outgoing, AudioClip newClip, float duration)
        {
            incoming.clip = newClip;
            incoming.volume = 0f;
            incoming.Play();

            float elapsed = 0f;
            float targetVolume = MasterVolume * MusicVolume;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                incoming.volume = Mathf.Lerp(0f, targetVolume, t);
                outgoing.volume = Mathf.Lerp(targetVolume, 0f, t);
                yield return null;
            }

            incoming.volume = targetVolume;
            outgoing.Stop();
            outgoing.volume = 0f;
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = Mathf.Clamp01(volume);
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            AudioSource current = _isSourceAActive ? _musicSourceA : _musicSourceB;
            if (current != null) current.volume = MasterVolume * MusicVolume;
        }
    }
}
