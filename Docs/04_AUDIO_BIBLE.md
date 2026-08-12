# 🎵 Stray Swarm Audio Bible

> [!IMPORTANT]
> The Audio Bible defines the complete sonic identity and technical audio implementation for **Stray Swarm**. Every sound, from the subtle menu clicks to the dynamic pitch-shifting combo systems, is documented here.

## 1. 🎧 Audio Direction

The sonic identity of **Stray Swarm** is **cheerful, warm, playful, and deeply satisfying**.
Think *Candy Crush* meets *Animal Crossing*. The audio should reinforce the positive, stress-free vibe of rescuing cute animals, while providing clear, punchy feedback for gameplay actions.

*   **Vibe:** Cozy, upbeat, rewarding, lighthearted.
*   **Avoid:** Harsh tones, aggressive alerts, realistic/gritty soundscapes, low-frequency rumbles (unless for specific heavy feedback).
*   **Keywords:** Bubbly, chime, pop, whoosh, soft, jazzy, pastel.

---

## 2. 🎼 Music Design

The music in Stray Swarm sets the mood and adapts to the game state.

### 2.1. Compositional Guidelines
*   **Key/Scale:** Primarily Major keys (C Major, F Major, G Major). Heavy use of pentatonic scales for melodies to ensure catchiness and non-dissonance.
*   **Instrumentation:** Marimba, vibraphone, pizzicato strings, soft synthesizers, light acoustic percussion (shakers, woodblocks, bongos).
*   **Mixing:** Keep the mid-frequencies clear so SFX can punch through. Music should sit underneath the sound effects.

### 2.2. Tracks

| Track Name | Usage | Tempo | Vibe |
| :--- | :--- | :--- | :--- |
| **BGM_Menu** | Main Menu, Level Select, Settings | ~110-120 BPM | Chill, lo-fi/jazzy loop, warm and inviting. |
| **BGM_Gameplay_Base** | Active gameplay (Timer > 50%) | ~130 BPM | Upbeat, light percussion, bouncy, focused. |
| **BGM_Gameplay_Urge** | Active gameplay (Timer < 50%) | ~130 BPM (Synced) | Adds faster hi-hats, a more persistent bassline, and subtle tension chords. |

### 2.3. Dynamic Music Layers
Gameplay music uses a layered approach.
*   `BGM_Gameplay_Base` plays continuously from the start of the level.
*   `BGM_Gameplay_Urge` plays simultaneously but is muted (Volume = 0).
*   When the timer drops below 50%, the `AudioManager` crossfades the volume of the Urge layer in over 2 seconds.

```csharp
// Example of crossfading logic in AudioManager
public void TriggerUrgencyMusic(bool isUrgent)
{
    float targetVolume = isUrgent ? maxMusicVolume : 0f;
    urgeAudioSource.DOFade(targetVolume, 2f);
}
```

### 2.4. Jingles
Jingles interrupt or layer over the music for key moments.
*   **MUS_Jingle_Victory:** Short triumphant fanfare (2-3 seconds). Brass, ascending scale, sparkling chime at the end.
*   **MUS_Jingle_Loss:** Deflating/sad trombone or descending synth slide (2 seconds). Humorous rather than punishing.

---

## 3. 💥 SFX Design

Every action needs a reaction. SFX provide the tactile feel of the game.

### 3.1. Core Gameplay SFX

| Event | Sound Description | File Name |
| :--- | :--- | :--- |
| **Swipe/Turn** | Soft, airy whoosh. Non-intrusive. | `sfx_swipe_whoosh` |
| **Collect Animal** | Cheerful bloop/pop. *See Pitch Shifting System below.* | `sfx_collect_base` |
| **Board Van** | Satisfying ding/chime, similar to a cash register but softer. | `sfx_van_board` |
| **Van Full** | Happy horn honk + quick sparkling jingle. | `sfx_van_full` |
| **Van Depart** | Comedic engine rev + tire squeak + horn fading out. | `sfx_van_depart` |
| **Gap Close Dash** | Fast zip/whoosh, slightly higher pitch than swipe. | `sfx_dash_zip` |

### 3.2. UI & Feedback SFX

| Event | Sound Description | File Name |
| :--- | :--- | :--- |
| **Button Press** | Soft, organic click or wooden pop. | `sfx_ui_click` |
| **Menu Transition** | Soft slide or paper shuffle sound. | `sfx_ui_slide` |
| **Star Earned (1-3)**| Bright twinkle. Pitch increases for star 2 and 3. | `sfx_star_earn_1/2/3` |
| **Timer Warning** | Ticking clock that increases in tempo and volume in the last 10 seconds. | `sfx_timer_tick` |
| **Level Win Modal** | Confetti pop + celebratory chime. | `sfx_ui_win_modal` |
| **Level Lose Modal**| Dull thud. | `sfx_ui_lose_modal` |

### 3.3. Combo Milestones

When players collect animals in quick succession or deliver a massive tail, specific milestone SFX play.
*   **Combo x3:** Small ascending chord (e.g., C-E-G). `sfx_combo_3`
*   **Combo x5:** Brighter, longer ascending chord + sparkle. `sfx_combo_5`
*   **Combo x10+:** Euphoric arpeggio + subtle bass drop. `sfx_combo_10`

---

## 4. 📈 Pitch Shifting System

To make collecting animals feel addictive and rewarding, we implement a **Pitch Shifting System** for the `sfx_collect_base` sound. This mimics the satisfying scaling audio found in games like Peggle or Candy Crush.

### 4.1. The Mechanic
Every time an animal is added to the tail, the base collection sound plays. If another animal is collected within a short time window (e.g., 2.0 seconds), the pitch of the sound is shifted upward by one semitone.

### 4.2. Implementation Details
*   **Base Pitch:** `1.0` (Normal playback speed/pitch).
*   **Pitch Increment:** `1.05946` (The multiplier for one semitone step in equal temperament, though in Unity, you can just add ~`0.05` to the `AudioSource.pitch` property for a close approximation, or calculate it exactly).
    *   *Formula:* `Mathf.Pow(1.05946f, comboCount)`
*   **Maximum Cap:** Cap the pitch shift at `3.0` (or around +12 semitones/1 octave) to prevent the sound from becoming screechy or unrecognizable.
*   **Reset Window:** If `2.0` seconds pass without a collection, the combo resets, and the next collection sound returns to a pitch of `1.0`.

### 4.3. Code Reference

```csharp
public class PitchShiftSystem : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource sfxSource;
    public AudioClip collectClip;
    
    [Header("Pitch Tuning")]
    public float basePitch = 1.0f;
    public float pitchStep = 0.05f; // Approx 1 semitone
    public float maxPitch = 2.0f;
    public float resetTime = 2.0f;
    
    private int currentCombo = 0;
    private float lastCollectTime = -10f;

    public void PlayCollectSound()
    {
        if (Time.time - lastCollectTime <= resetTime)
        {
            currentCombo++;
        }
        else
        {
            currentCombo = 0;
        }

        float calculatedPitch = basePitch + (currentCombo * pitchStep);
        sfxSource.pitch = Mathf.Min(calculatedPitch, maxPitch);
        
        // Randomize slightly to avoid machine-gun effect
        sfxSource.pitch += Random.Range(-0.02f, 0.02f); 
        
        sfxSource.PlayOneShot(collectClip);
        lastCollectTime = Time.time;
    }
}
```

> [!TIP]
> Always add a tiny bit of random pitch modulation (`Random.Range(-0.02f, 0.02f)`) even on the base pitch to prevent phasing and listener fatigue when the same sound plays repeatedly.

---

## 5. 🗺️ Spatial Audio

While Stray Swarm is a 2D game, we use 2D Spatial Audio (Stereo Panning) to give the world life and help players locate uncollected animals.

### 5.1. Ambient Animal Calls
Uncollected animals emit subtle, idle calling sounds on a randomized timer (e.g., every 4-8 seconds).
*   **Blue Puppy:** Soft "yip" or panting.
*   **Pink Kitten:** High-pitched "mew".
*   **Yellow Pigeon:** Gentle "coo".
*   **Green Frog:** Short "ribbit".
*   **Orange Hamster:** High squeak.
*   **Purple Bunny:** Sniffle/thump.

### 5.2. Panning Logic
The AudioSource for these idle calls is attached to the animal prefab.
*   `Spatial Blend`: Set to `0` (Fully 2D).
*   `Stereo Pan`: Calculated based on the animal's X position relative to the main camera.
*   If the animal is on the left side of the screen, the sound pans left.

```csharp
// Simple 2D Panning logic
public void UpdatePan(Transform listenerCamera)
{
    // Assuming screen coordinates normalized between -1 (left) and 1 (right)
    Vector3 viewportPos = listenerCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
    float panValue = (viewportPos.x * 2f) - 1f; 
    
    // Clamp to avoid extreme hard panning
    audioSource.panStereo = Mathf.Clamp(panValue, -0.8f, 0.8f);
}
```

---

## 6. 🏗️ Audio Manager Architecture

The Audio system relies on a central `AudioManager` Singleton, heavily integrated with the ScriptableObject event system.

### 6.1. Core Components
*   **AudioManager.cs:** The Singleton that persists across scenes (`DontDestroyOnLoad`). It handles music crossfading, global volume, and exposing play methods. Note: `Awaitable.WaitForSecondsAsync()` can be used for audio fade sequences instead of coroutines.
*   **SFXPool.cs:** Object pooling for AudioSources. Instead of instantiating new AudioSources or interrupting a single one, we pull an available AudioSource from a pool of 10-15 to play overlapping sounds. We use Unity 6's built-in `UnityEngine.Pool.ObjectPool<T>` for the SFX AudioSource pool instead of a custom pool.
*   **AudioEvent.cs (ScriptableObject):** A data container for sounds. It holds the `AudioClip`, volume, base pitch, and pitch variance. 

### 6.2. The SFX Pool
When `AudioManager.PlaySFX(AudioEvent evt)` is called:
1.  The manager requests an available `AudioSource` from the pool.
2.  If all are busy, it steals the oldest playing source (except for looping sounds).
3.  It sets the clip, calculates pitch (applying variance), and calls `Play()`.
4.  A coroutine (or Awaitable) returns the source to the pool once the clip finishes.

---

## 7. 🎛️ Audio Settings

Players must have granular control over their audio experience. These settings are saved via `PlayerPrefs`.

| Setting | Default Value | Range | Description |
| :--- | :--- | :--- | :--- |
| **Master Volume** | 1.0 (100%) | 0.0 - 1.0 | Controls the AudioListener global volume. |
| **Music Volume** | 0.8 (80%) | 0.0 - 1.0 | Controls the AudioMixer group for BGM. |
| **SFX Volume** | 1.0 (100%) | 0.0 - 1.0 | Controls the AudioMixer group for SFX. |
| **Mute Toggle** | False | True/False | Instantly mutes Master Volume. |

> [!WARNING]
> Ensure that setting Volume to 0 actually mutes the AudioMixer parameter by mapping the linear slider (0-1) to logarithmic decibels (-80dB to 0dB).
> `mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);`

---

## 8. ⚙️ Technical Specs

To keep the mobile app size small while maintaining quality, adhere to these import settings in Unity.

### 8.1. File Formats & Import Settings
*   **File Formats:** **OGG Vorbis** is recommended as the primary format for mobile (NOT WAV or MP3). OGG has better compression than MP3 and no licensing issues.
*   **Unity Import Settings:**
    *   **Force To Mono:** True for almost all SFX (panning handles spatialization). False for Music.
    *   **SFX:** OGG Vorbis, Decompress on Load, Quality 70-80%
    *   **Music:** OGG Vorbis, Streaming, Quality 50-60%
    *   **Sample rate:** 44100 Hz for music, 22050 Hz for SFX

---

## 9. 📦 Asset Sources

Where to source or create the audio for Stray Swarm:

*   **Music:** GarageBand or Logic Pro using built-in synths and mallets. Consider hiring a Fiverr composer for bespoke loops if budget permits.
*   **Retro/Chiptune SFX:** [BFXR](https://www.bfxr.net/) - Great for generated bloops, powerups, and synth jumps.
*   **Organic/Foley SFX:** [Freesound.org](https://freesound.org/) (Filter by CC0 license). Great for swishes, paper slides, and base animal noises.
*   **UI Sounds:** [Kenney.nl Audio Packs](https://kenney.nl/assets/audio) - Specifically the UI Audio and Casino Audio packs for satisfying clicks and chimes.

---

## 10. 📳 Haptic Feedback (Vibration)

Audio and Haptics go hand-in-hand to create game feel. On mobile, we use a haptic plugin (like iOS Haptic Engine / Android Vibrator wrappers) to trigger patterns that match the audio.

### 10.1. Haptic Events Matrix

| Event | Haptic Pattern | Audio Pair |
| :--- | :--- | :--- |
| **Swipe/Turn** | Very Light Impact | `sfx_swipe_whoosh` |
| **Collect Animal** | Light Impact | `sfx_collect_base` |
| **Board Van** | Medium Impact | `sfx_van_board` |
| **Van Full** | Heavy Impact + Light Pulse | `sfx_van_full` |
| **Gap Close Dash** | Quick Double Light Tap | `sfx_dash_zip` |
| **Combo Milestone** | Ascending sequence of light taps | `sfx_combo_x` |
| **Level Win** | Long Success Pattern (Ta-da!) | `MUS_Jingle_Victory` |
| **Level Lose** | Long Failure Pattern (Thud-thud) | `MUS_Jingle_Loss` |
| **UI Button Press**| Selection/Light Tick | `sfx_ui_click` |

> [!NOTE]
> Haptics should have a separate toggle in the Settings Menu, defaulting to **ON**. Never tie haptics to the audio volume or mute switch.
