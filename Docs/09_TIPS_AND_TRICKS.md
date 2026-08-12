# 💡 Stray Swarm: Tips & Tricks Treasury

Welcome to the treasure trove of hard-won game development wisdom for **Stray Swarm**. This document serves as a comprehensive guide to best practices, optimization strategies, architectural guidelines, and workflow enhancements specifically tailored for this Unity 6, 2D URP mobile puzzle-flow game.

> [!IMPORTANT]
> This is a living document. As new optimizations are discovered and architectural patterns evolve, update this treasury to reflect the latest project standards.

---

## 🎮 1. Unity 6 Tips

To get the most out of Unity 6, follow these foundational tips for project setup and general coding.

### 🕹️ New Input System
The **New Input System** is REQUIRED in Unity 6. The old `Input.GetKeyDown()`, `Input.GetAxis()`, etc. are DEPRECATED.
Use `InputAction` for mobile swipe detection.

```csharp
public class SwipeController : MonoBehaviour
{
    public InputAction swipeAction;

    private void OnEnable() => swipeAction.Enable();
    private void OnDisable() => swipeAction.Disable();

    private void Update()
    {
        if (swipeAction.WasPerformedThisFrame())
        {
            Vector2 swipeDelta = swipeAction.ReadValue<Vector2>();
            // Process swipe
        }
    }
}
```

### ⏳ Unity 6 Awaitable/async-await
Unity 6 introduces the `UnityEngine.Awaitable` class as an alternative to coroutines.
- `await Awaitable.WaitForSecondsAsync(1f);` (replaces `WaitForSeconds`)
- `await Awaitable.NextFrameAsync();` (replaces `yield return null`)
- `await Awaitable.BackgroundThreadAsync();` (for off-main thread logic)

### 🎥 Cinemachine v3
In Unity 6's Cinemachine v3, `CinemachineVirtualCamera` is renamed to `CinemachineCamera`.

### 📦 Assembly Definitions (asmdef)
Compile times can kill momentum. Use Assembly Definitions to split your codebase into logical modules (e.g., `Core`, `UI`, `Gameplay`, `Audio`).
- **Why?** When you change a script in `Gameplay`, Unity only recompiles the `Gameplay` assembly, not the entire project.

```json
{
    "name": "StraySwarm.Gameplay",
    "rootNamespace": "StraySwarm.Gameplay",
    "references": [
        "StraySwarm.Core"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": false,
    "precompiledReferences": [],
    "autoReferenced": true,
    "defineConstraints": [],
    "versionDefines": [],
    "noEngineReferences": false
}
```

### 🗃️ Addressables for Asset Management
If the project grows (more levels, more skins, seasonal content), transition from direct references to **Addressables**.
- It allows asynchronous loading, reducing memory footprint.
- Decouples asset loading from scenes.

### 🖼️ 2D URP Renderer Settings
Optimize for mobile by stripping unnecessary features:
- Disable Post-processing if not strictly needed.
- Limit the number of pixel lights (1 or 2 max).
- Disable shadow cascades and soft shadows to save rendering time.

### 📚 Sprite Atlas
Always use Sprite Atlases for draw call batching.
- Group sprites by UI panels or gameplay entities.
- Ensure "Tight Packing" is disabled for UI sprites to avoid artifact bleeding.

### 📸 Avoid `Camera.main`
`Camera.main` does a `FindGameObjectsWithTag` under the hood in older Unity versions, though it's optimized in newer ones. Still, caching it explicitly is best practice.
```csharp
public class CameraController : MonoBehaviour 
{
    private Camera _mainCamera;

    private void Awake() 
    {
        _mainCamera = Camera.main; // Cache it once
    }
}
```

### 🏷️ Tag Comparison
Never use `==` for tag comparison. It allocates memory. Use `CompareTag()`.
```csharp
// ❌ BAD: Allocates memory for string comparison
if (collision.gameObject.tag == "Player") { ... }

// ✅ GOOD: Zero allocations
if (collision.gameObject.CompareTag("Player")) { ... }
```

### 🔤 TextMeshPro (TMP)
Always use TextMeshPro instead of legacy UI Text. It uses Signed Distance Fields (SDF) for crisp rendering at any scale and resolution. It is NOW BUILT INTO `com.unity.ugui` in Unity 6, so no separate package import is needed.

### 👁️ Inspector Variables
Prefer `[SerializeField] private` over `public` for inspector variables. It maintains encapsulation while allowing designer tweaking.
```csharp
// ❌ BAD: Breaks encapsulation
public float speed = 5f;

// ✅ GOOD: Exposes to inspector, keeps private access
[SerializeField] private float _speed = 5f;
```

---

## ⚡ 2. Performance Tips for Mobile

Mobile devices have strict thermal and battery constraints. **Target 60 FPS** reliably on mid-range devices.

### 📈 Profiling
Profile early, profile often. Use the Unity Profiler *on a real device*, not just in the Editor. The Editor has significant overhead that masks true device performance.

### 🏊 Object Pooling
**Rule of thumb:** If it's instantiated and destroyed frequently during gameplay, pool it.
- **What to pool:** Stray animals, particle effects, floating text, audio sources.
- **Why?** `Instantiate` and `Destroy` cause CPU spikes and trigger Garbage Collection (GC) pauses.
- **Unity 6 Advantage:** Use Unity's BUILT-IN `UnityEngine.Pool.ObjectPool<T>`.

```csharp
using UnityEngine.Pool;

public class StrayAnimalSpawner : MonoBehaviour
{
    [SerializeField] private StrayAnimal _prefab;
    private ObjectPool<StrayAnimal> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<StrayAnimal>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 20
        );
    }

    public void Spawn()
    {
        StrayAnimal animal = _pool.Get(); // Pull from pool
        // Use the animal...
    }

    public void Despawn(StrayAnimal animal)
    {
        _pool.Release(animal); // Return to pool
    }
}
```

### 🗑️ Zero Allocations in Update
The Garbage Collector (GC) is the enemy of smooth framerates on mobile.
- **No `new` keywords** in `Update()`.
- **No LINQ** queries in hot paths.
- **No string concatenation** in `Update()` (e.g., `scoreText.text = "Score: " + score`). Use string builders or pre-allocate strings.

### 📦 Batching
- **Static Batching:** Enable for all non-moving environment pieces (walls, stations).
- **Dynamic Batching:** Enable for simple moving characters, though SRP batcher might handle this better in URP.

### ✨ Particle Limits
Keep particle systems simple.
- **Budget:** Max 200 particles on screen simultaneously.
- Use simple quads, limit overdraw, and disable collision if not strictly needed.

### 🎨 Texture Compression
Uncompressed textures consume massive amounts of memory.
| Platform | Recommended Compression | Fallback |
| :--- | :--- | :--- |
| Android | ASTC (Adaptive Scalable Texture Compression) | ETC2 |
| iOS | ASTC | PVRTC |

### 🔍 Reduce Overdraw
Overdraw happens when multiple transparent layers are drawn on top of each other.
- Trim transparent space around sprites.
- In particle systems, use opaque materials where possible.
- Check overdraw in the Scene view using the Overdraw rendering mode.

### 🖼️ UI LOD (Level of Detail)
Disable Canvas components or GameObjects that are off-screen or completely covered by opaque panels. Do not let hidden UI elements calculate layout or render.

### 🎵 Audio Compression
- Use **OGG Vorbis** for longer music tracks.
- Use **ADPCM** or **Uncompressed** for short UI sound effects.
- Force to Mono for sound effects to save memory, stereo is rarely needed for UI blips.
- Select "Load in Background" to prevent audio loading hitches.

---

## 🏗️ 3. Architecture Tips

A clean architecture prevents spaghetti code and makes adding features (like a new animal type) trivial.

### 📡 Event Systems
Prefer ScriptableObject (SO) architecture for events to decouple systems.

| Event Type | Use Case | Pros | Cons |
| :--- | :--- | :--- | :--- |
| **ScriptableObject Events** | Global game events (e.g., `OnGameWon`, `OnAnimalCollected`) | Total decoupling, visible in inspector, easily mockable. | Requires creating SO assets. |
| **C# `event` Action** | Localized communication between tightly coupled components. | Fast, type-safe, low overhead. | Can lead to strong coupling if misused. |
| **UnityEvents** | UI button clicks. | Designer-friendly inspector binding. | Slow (reflection-based), hard to trace in code, fragile if methods are renamed. |

> [!TIP]
> Avoid UnityEvents for core gameplay logic. Reserve them strictly for UI `Button.onClick`.

### 🏔️ Singletons
Use Singletons sparingly. They introduce global state and make unit testing difficult.
- **Acceptable Singletons:** `GameManager`, `AudioManager`.
- **Alternatives:** Dependency Injection, Service Locator, or ScriptableObject references.

### 🧩 Composition over Inheritance
Avoid deep class hierarchies (`Entity -> MovingEntity -> Animal -> StrayAnimal`).
Instead, build entities using modular components:
- `HealthComponent`
- `MovementComponent`
- `InputComponent`

### 🏃 Thin MonoBehaviours
Keep MonoBehaviours focused purely on Unity lifecycle events and inspector exposure. Move heavy logic into plain C# (POCO) classes. This makes logic testable outside of Unity.

### 🔌 Interfaces for Testability
Code against interfaces, not implementations.
```csharp
public interface IInputProvider 
{
    Vector2 GetSwipeDirection();
}

// Mobile implementation
public class TouchInput : IInputProvider { ... }

// Editor implementation
public class KeyboardInput : IInputProvider { ... }
```

### 🚦 State Machines
Use finite state machines (FSM) for anything with distinct states to avoid messy `switch` statements and boolean flags.
- **Game State:** `MainMenu`, `Playing`, `Paused`, `GameOver`.
- **Animal State:** `Idle`, `Following`, `Rescued`.

### 🧰 Unity 6 Best Practices
- Prefer using `UnityEngine.Pool.ObjectPool<T>` instead of custom pooling scripts.
- Prefer `Awaitable` over coroutines for simple delays and async operations in Unity 6.

---

## 🎬 4. DOTween Tips

DOTween is standard for code-driven animation. Proper usage ensures smooth performance.

### ⚙️ Initialization for Mobile
Initialize DOTween explicitly with settings optimized for mobile performance and debug logging.
```csharp
// Place this in an early Awake method (e.g., in a bootstrapper)
DOTween.Init(recycleAllByDefault: false, useSafeMode: true, logBehaviour: LogBehaviour.ErrorsOnly);
```

### 💀 Kill Tweens on Destroy
If a GameObject is destroyed while a tween is running on it, DOTween will throw errors or cause memory leaks. Always link tweens!
```csharp
transform.DOScale(1.5f, 0.5f).SetLink(gameObject); 
// SetLink automatically kills the tween if the gameObject is destroyed.
```

### 🧬 Sequences
Use Sequences for complex, multi-step animations (e.g., UI panel pop-in).
```csharp
Sequence seq = DOTween.Sequence();
seq.Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack));
seq.Join(canvasGroup.DOFade(1f, 0.3f));
seq.AppendInterval(0.1f);
seq.Append(titleText.DOColor(Color.yellow, 0.2f));
```

### 💾 Caching References
If you frequently pause, resume, or kill a specific tween, cache its reference.
```csharp
private Tween _bounceTween;

private void StartBounce() 
{
    _bounceTween?.Kill(); // Kill existing before starting new
    _bounceTween = transform.DOPunchScale(Vector3.one * 0.1f, 0.5f).SetLink(gameObject);
}
```

### ♻️ Reusable Tweens (For Pooled Objects)
For objects in a pool, creating and destroying tweens repeatedly is inefficient. Create the tween once, set it to not auto-kill, and restart it when the object is pulled from the pool.
```csharp
private Tween _idleTween;

void Awake()
{
    _idleTween = transform.DOMoveY(0.5f, 1f).SetRelative().SetLoops(-1, LoopType.Yoyo).SetAutoKill(false);
    _idleTween.Pause();
}

public void OnSpawnFromPool()
{
    _idleTween.Restart();
}

public void OnReturnToPool()
{
    _idleTween.Pause();
}
```

### 🎢 Easing Cheat Sheet
Choosing the right easing transforms robotic movement into juicy feedback.

| Easing Type | Best Used For | Vibe |
| :--- | :--- | :--- |
| `OutBack` | UI panels popping in, success notifications. | Bouncy, playful. |
| `InBack` | UI panels closing, object disappearing. | Anticipation before leaving. |
| `InOutSine` | Continuous floating/bobbing (Idle states). | Smooth, organic. |
| `OutExpo` | Swift movement stopping abruptly (dashing). | Snappy, responsive. |
| `Linear` | Progress bars, constant speed movement. | Mechanical, predictable. |

---

## 🌳 5. Git Tips for Unity

Proper source control setup prevents catastrophic project loss and collaboration headaches.

### 📝 Force Text Serialization
Always ensure Unity saves assets as text. Binary merge conflicts are impossible to resolve.
- **Edit > Project Settings > Editor > Asset Serialization > Mode: Force Text**

### 🙈 `.gitignore`
Ensure you have a robust Unity `.gitignore`. The most critical rule: **Never commit the `Library/` folder.** It is massive and auto-generated.

### 🐘 Git LFS (Large File Storage)
Use Git LFS for binary assets to prevent repository bloat.
- `.psd`, `.png`, `.jpg`
- `.wav`, `.ogg`, `.mp3`
- `.fbx`, `.obj` (if adding 3D later)
- `.unity` (Scenes) and `.prefab` (can be LFS, but standard text diffs are better if configured).

### ✍️ Commit Hygiene
- Commit often, in small logical chunks.
- Write clear commit messages. 
  - `Fix: Corrected tail crossing logic bug` 
  - `Feat: Added Blue Puppy prefabs and data`
- Avoid massive "end of day" commits containing dozens of unrelated changes.

### 🌿 Branch Strategy
Adopt a simple Git flow:
- `main`: The stable, always-buildable release branch.
- `dev`: The active integration branch.
- `feature/name`: Short-lived branches for specific tasks (e.g., `feature/swipe-input`).

---

## 🛠️ 6. Workflow Tips

Speed up your iteration time to keep development fun and focused.

### 📱 Device Testing
The Unity Editor lies. Screen sizes, input feel, and performance are vastly different on hardware.
- Build and test on a real device at least **once a week**.
- Use **Unity Remote** app for immediate (though slightly laggy) touch input testing without building.

### 🎛️ Debug Menu
Build an in-game debug canvas (hidden in release builds via `#if` directives) to cheat.
- Skip levels instantly.
- Spawn specific animals or hazards.
- Clear save data.
- Toggle slow-motion (manipulate `Time.timeScale`) to analyze animations.

### ✂️ Editor-Only Code
Use preprocessor directives to keep debug code out of the final APK/IPA.
```csharp
#if UNITY_EDITOR
    if (Input.GetKeyDown(KeyCode.Space))
    {
        DebugCompleteLevel();
    }
#endif
```

### 🎲 ScriptableObject Presets
Create ScriptableObjects for level configurations (grid size, animal density, time limit). This allows designers to duplicate and tweak levels without touching code or scene hierarchies.

### 🖍️ Gizmos
Visualize invisible logic in the Editor to debug AI and grid systems.
```csharp
private void OnDrawGizmos()
{
    if (_gridNodes == null) return;
    
    Gizmos.color = Color.cyan;
    foreach(var node in _gridNodes)
    {
        Gizmos.DrawWireCube(node.Position, Vector3.one * 0.9f);
    }
}
```

---

## 🚫 7. Common Pitfalls to Avoid

Learn from the pain of past developers. Don't make these mistakes.

### 🔍 Don't Use `Find()` at Runtime
`GameObject.Find()`, `FindObjectOfType()`, and `GetComponent()` are incredibly expensive.
- **Rule:** Only use them in `Awake()` or Editor scripts.
- **Alternative:** Cache references, pass via inspector, or use a dependency injection framework.

### ⏱️ Coroutines vs Timers
Avoid Coroutines for tight, gameplay-critical timing loops (like input buffering windows). Coroutines allocate memory when yielding and can be messy to track.
- **Use standard `Update()` timers instead:**
```csharp
private float _bufferTimer = 0f;

void Update()
{
    if (_bufferTimer > 0)
    {
        _bufferTimer -= Time.deltaTime;
        if (_bufferTimer <= 0) ExecuteBufferedInput();
    }
}
```

### ⏸️ Pausing and `Time.timeScale`
When you set `Time.timeScale = 0f` to pause the game:
- `Update()` still runs! `Time.deltaTime` will be 0.
- Use `Time.unscaledDeltaTime` if you need UI animations to run while the game is paused.
- Don't forget to reset `Time.timeScale = 1f` on scene load, or the new scene will start paused.

### 🧱 Hardcoding Magic Numbers
Never write `health -= 10;`. Put `10` in a `[SerializeField] private int _damageAmount = 10;` or a constants file. Magic numbers make balancing impossible.

### 🏎️ Premature Optimization
Don't spend hours optimizing a script that runs once at startup.
- **Mantra:** Make it work -> Make it right -> Make it fast.
- Rely on the Profiler to tell you what needs optimizing.

### 🖼️ Asset Import Settings
A single 4K texture with default import settings can add 20MB to your build size. Always review import settings for every new asset (compression, max size, read/write disabled).

---

## 🎒 8. Useful Free Assets & Tools

Don't reinvent the wheel. Utilize these industry-standard free tools.

- **[DOTween](http://dotween.demigiant.com/) (Free Version):** The absolute best tweening engine for Unity.
- **TextMeshPro (Built-in):** Standard for crisp, scalable text.
- **Cinemachine v3 (built-in):** Excellent for smooth camera following, framing, and screen shake.
- **Unity Input System package (required):** Essential for modern input handling.
- **Unity Profiler, Memory Profiler, Frame Debugger (built-in):** Crucial tools for profiling.
- **[NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes):** Free open-source tool for better inspector styling (buttons, sliders, validation) without writing custom editors.
- **[BFXR](https://www.bfxr.net/):** Incredible web-based generator for 8-bit/retro sound effects (jumps, powerups).
- **[Freesound.org](https://freesound.org/):** CC0 community sound effects. Always check licenses!
- **[Kenney.nl](https://kenney.nl/):** The hero of prototyping. Thousands of free, CC0 game assets (UI, sprites, audio).
- **Google Fonts:** Excellent free fonts for UI. Recommended for Stray Swarm's style: *Baloo 2*, *Nunito*, or *Quicksand*.
- **[Lospec.com](https://lospec.com/):** Fantastic resource for color palette inspiration and restrictions.

---

## 📱 9. Mobile-Specific Tips

Developing for a touch screen in a pocket is different from a PC on a desk.

### 👆 Touch Input First
Design interactions for "fat fingers".
- Ensure UI buttons have large enough raycast padding (minimum 44x44 points).
- Implement input buffering for swipes so players don't miss turns because they swiped 50ms too early.

### 🔲 Notch and Safe Area
Modern phones have notches, dynamic islands, and rounded corners.
- Always anchor vital UI (score, pause button) to the **Safe Area**, not the absolute screen corners.
- Use a `SafeAreaFitter` script attached to a root Canvas panel to automatically adjust padding based on `Screen.safeArea`.

### 🔄 App Pause / Resume
Mobile OS will suspend your game when a call comes in or the user switches apps.
- Implement `OnApplicationPause(bool isPaused)` in a core manager.
- Automatically trigger the in-game Pause menu when `isPaused` is true.
- Mute audio instantly to avoid jarring bursts of sound when returning.

### 💾 Defensive Saving
Mobile games crash, or are forcefully closed by the OS to save memory.
- Save data frequently (e.g., end of every level, not just on app quit).
- Use JSON serialization or a robust save system, avoid `PlayerPrefs` for anything complex as it can corrupt easily.

### 🔒 Permissions
Only ask for permissions you absolutely need (e.g., Network, Storage). Request them in context, explaining *why* you need them, rather than immediately on app launch.

### 🧪 Device Spectrum
Test on a low-end Android device (the "minimum spec" target) to check framerates, and a high-end device (iPad Pro or latest Galaxy/iPhone) to check rendering accuracy and UI scaling at high resolutions.
