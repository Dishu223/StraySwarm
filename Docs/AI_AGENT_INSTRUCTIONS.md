# 🤖 AI AGENT MASTER INSTRUCTIONS — Stray Swarm

> **CRITICAL: Any AI agent working on this project MUST read this file in its entirety before writing a single line of code or giving any instructions to the user.**

---

## 1. Project Identity

| Field | Value |
| :--- | :--- |
| **Game Title** | Stray Swarm |
| **Engine** | Unity 6 (6000.x) — 2D URP |
| **Platform** | Android (Google Play) + iOS (App Store) |
| **Genre** | Hypercasual / Puzzle-Flow |
| **Art Pipeline** | Kawaii Cube Aesthetic — AI-generated cube character PNGs + code-driven wobble animations (no sprite sheets) |
| **Target** | 60 FPS on mid-range mobile, Portrait (1080×1920), <80MB APK |

## 2. Game Concept

Swipe to navigate a stray cat through themed mazes (Desert, Forest, Winter, City), collect colorful lost animals into a conga-line tail, and deliver them to matching rescue stations before time runs out. Earn 1–3 stars based on speed. Progress through 50+ levels across 4 themed worlds.

## 2b. Reference Design (TARGET QUALITY)

The user provided reference screenshots from a similar production game. Key design targets:

| Feature | Description |
| :--- | :--- |
| **Multiple simultaneous delivery stations** | 2–4 stations visible per level (at screen top), each accepting a different color. Stations are wooden crate/shelter style, NOT driving vans. |
| **Themed world backgrounds** | Desert (warm sand), Forest (vibrant green), Winter (icy blue), City (gray). Each world has unique edge decorations. |
| **Rounded path corners** | Paths use 47-tile Rule Tile set with smooth inner corners, NOT sharp 90° grid squares. |
| **Small dense items** | Collectible animals are small (~0.4 tile size), densely placed along paths. |
| **One-way arrow tiles** | Yellow circular arrows force the player in a specific direction. |
| **Rock/stone obstacles** | Permanent or breakable barriers blocking path segments. |
| **Numbered barriers** | Walls with a number that decrements each time player passes, breaking at 0. |
| **Bridges/overpasses** | Paths that cross over each other on different layers. |
| **Drop shadows** | Every object has a soft drop shadow on the path. |
| **Thick playful font** | White text with purple stroke/outline, very bubbly (TextMeshPro outline). |
| **Kawaii Cube Characters** | Player cat and all animals are soft rounded-square 'cube' characters. NO traditional sprite sheet animations. All motion via code-driven wobble physics (squash, stretch, hop, tilt). Animals in the tail ride inside small wooden baskets with their cube heads peeking out. |

## 2c. Cube Aesthetic Art Direction (CRITICAL)

The game uses a **"Kawaii Cube World"** visual identity. Key rules:

1. **Characters:** Player cat and all 6 animal types are soft rounded-square 'cube' sprites. Single static PNG per character (no directional sprites). Eyes shift in movement direction via code.
2. **Basket Tail System:** When collected, animals hop into small wooden baskets. The conga-line tail is a chain of bouncing baskets with cube animal heads peeking out from the top rim.
3. **NO Sprite Sheet Animations:** All character life comes from code-driven transform manipulation:
   - `CubeWobble.cs`: Idle sine-wave bob, move hop (squash & stretch), turn tilt, land squash.
   - `BasketBounce.cs`: Follow lag, cargo sway, collect pop.
   - `StationPulse.cs`: Idle breathing, attention glow, receive gulp.
4. **Particles Use Square Shapes:** All particle effects use small rounded-square shapes (not circles) to match the cube aesthetic.
5. **Delivery Stations:** Wooden crate/shelter style (matching the baskets). Collected animals visually stack up inside.
6. **Obstacles:** All obstacles use rounded-rectangle/cube shapes (rounded rock cubes, rounded stone walls, yellow square arrows).
7. **Visual Consistency Rule:** Before adding ANY visual element, verify: Is it rounded? Does it have a drop shadow? Does it wobble/bounce on interaction? Are particles square-shaped?

Full art direction details are in `Docs/03_ART_BIBLE.md`.

---

## 3. User Profile

The user is a game developer building Stray Swarm. They want clear, explicit instructions on what to wire, where to drag assets, and what each Inspector field does before moving forward.

## 3b. Strict Inspector Wiring & Communication Standard (CRITICAL RULE)
1. **Never leave empty Inspector fields unexplained:** Whenever a script is created, modified, or introduced, the AI MUST explicitly list all its Inspector serialized fields, explain what each field is for, and give the user the exact drag-and-drop source in Unity.
2. **Account for every field then and there:** Before moving to any next task or phase, verify that every Inspector field on all active scene objects is completely filled.
3. **Always implement robust auto-fallback dependencies in code:** In `Awake()` / `Start()`, always write fallback `GetComponent<T>()` or `FindAnyObjectByType<T>()` for serialized component dependencies (e.g. `_pathHistory`, `_gameManager`, `_tailManager`, `_vanQueue`, `_attachedCrate`) so the game NEVER crashes if a field is unset in the Inspector.

- **Skill Level:** Beginner Unity developer. Knows basics but needs step-by-step guidance.
- **Teaching Standard:**
  - Always explain "why" before "what".
  - Give numbered step-by-step instructions with exact menu paths.
  - Provide **complete code files** (never partial snippets missing context).
  - Include checkpoint verification after each step.
  - **CRITICAL:** NEVER silently change values, inspector settings, or defaults tuned by the user (e.g. spacing, speed, scale, thresholds). Always explicitly inform the user whenever modifying any values and explain why.
  - **CRITICAL: Git Version Control:** Commit changes to git and push to `origin main` (remote: `https://github.com/Dishu223/StraySwarm`) after every major feature/update completed with the user. Always use descriptive, structured commit messages.
  - Be patient, encouraging, and thorough.

---

## 4. Current Project State (UPDATED 2026-08-13)

### What HAS Been Built (Prototype Complete)

**17 C# Scripts exist in `UnityProject/Assets/_StraySwarm/Scripts/`:**

| Folder | Scripts | Status |
| :--- | :--- | :--- |
| `Audio/` | `AudioManager.cs` | ✅ Singleton, pitch-jitter, PlayCollect/Deliver/Win/Lose convenience methods. All clip slots empty (needs real audio files). |
| `Core/` | `NodeData.cs` | ✅ Grid node data structure. |
| `Core/` | `GridManager.cs` | ✅ Tilemap-based scanning + fallback test grid + debug Gizmos. |
| `Core/` | `InputHandler.cs` | ✅ New Input System swipe detection with input buffering. |
| `Core/` | `PlayerController.cs` | ✅ Node-to-node lerp, auto-cornering at dead-ends, 180° turns, Animator hooks (IsMoving, MoveX, MoveY). Uses `GetClosestNode()` for free placement. |
| `Core/` | `GameManager.cs` | ✅ Timer countdown, Win/Loss state machine, Star calculation (1/2/3 based on time %), fires AudioManager & JuiceManager on events. |
| `Core/` | `LevelManager.cs` | ✅ Scene reload, level playlist, DontDestroyOnLoad. |
| `Data/` | `LevelData.cs` | ✅ ScriptableObject (time limit, star thresholds, van color sequence). |
| `Gameplay/` | `PathHistory.cs` | ✅ Breadcrumb `LinkedList<PathPoint>` recording, distance-based position lookup. |
| `Gameplay/` | `TailManager.cs` | ✅ Conga-line follow system, staggered delivery coroutine (0.15s per animal), `_isDelivering` flag prevents double-delivery. |
| `Gameplay/` | `FollowerBehavior.cs` | ✅ `IsCollected` flag, `AnimalColor` string, `FlyToVan()` coroutine animation. |
| `Gameplay/` | `RescueStation.cs` | ✅ `OnTriggerEnter2D`/`Stay2D`/`Exit2D`, `_isPlayerInside` enforcement, public `AttemptDelivery()`. |
| `Gameplay/` | `VanController.cs` | ✅ Color matching, `IsParked` state, Squash&Stretch `BounceRoutine()`, `DelayedDriveAwayRoutine()` (0.4s delay). |
| `Gameplay/` | `VanQueue.cs` | ✅ Sequential van spawning, `DriveInRoutine()` with `SetParked()` callback, race-condition safe. |
| `Gameplay/` | `JuiceManager.cs` | ✅ Singleton, spawns particle prefabs (collect, deliver, confetti). Sets sortingOrder=100. |
| `Gameplay/` | `CameraShake.cs` | ✅ Lightweight random-offset shake, triggered on delivery. |
| `UI/` | `UIManager.cs` | ✅ Timer HUD (Deep Rust color, red warning <10s), Win/Lose panels, EaseOutBack bouncy panel + sequential star pop coroutine, `OnNextLevelButtonClicked()` / `OnRestartButtonClicked()`. |

### Unity Scene Configuration Done
- Canvas: Screen Space - Camera, Scale With Screen Size (1080×1920), Match=0.5.
- Floor Tilemap: Order in Layer = -10.
- Camera: Orthographic, Size=10.
- Player: Tagged "Player", Kinematic Rigidbody2D, Trigger CircleCollider2D.
- Animator Controller: `CatAnimator` with IsMoving (Bool), MoveX/MoveY (Float), Idle↔Run transitions.
- Color Palette: Background=#FFF7ED, Primary=#F97316, CTA=#2563EB, Text=#9A3412.
- Font: Lilita One imported as TMP Font Asset.
- Win Panel: Floating card (800×1100), 3 star slots, "NEXT LEVEL" button wired.
- Lose Panel: "RETRY" button wired.
- Particle: WinConfettiPrefab saved in Prefabs folder.

### What Has NOT Been Built (Critical Gaps)
1. ❌ ScriptableObject Event Bus (systems coupled via FindAnyObjectByType)
2. ❌ AnimalData ScriptableObject (using raw string for color)
3. ❌ Object Pooling (using Instantiate/Destroy)
4. ❌ Save System (no persistence)
5. ❌ Multi-scene architecture (only 1 scene, no Boot/MainMenu/LevelSelect)
6. ❌ Main Menu, Level Select, Settings, Pause Menu screens
7. ❌ Real art assets (using Unity primitive shapes)
8. ❌ Real audio files (all AudioManager clip slots empty)
9. ❌ 50+ levels across 4 themed worlds (only 1 test Tilemap exists)
10. ❌ Combo System
11. ❌ Cinemachine v3
12. ❌ DOTween (using custom coroutine animations)
13. ❌ URP 2D Lighting & Post-Processing
14. ❌ Haptic feedback
15. ❌ Colorblind accessibility (shape icons)
16. ❌ Currency, Shop, Monetization
17. ❌ Tutorial/Onboarding
18. ❌ Sorting Layers (not configured)
19. ❌ Awaitable async (using Coroutines)
20. ❌ Multi-station architecture (currently uses single VanQueue, need multiple simultaneous stations)
21. ❌ Obstacle system (one-way arrows, rock barriers, numbered walls, bridges)
22. ❌ Themed world tile sets (Rule Tiles with rounded corners for Desert/Forest/Winter/City)
23. ❌ Pre-level screen (showing level info before starting)
24. ❌ World map level select (with world-unlock gates based on total stars)
25. ❌ Cube Wobble animation system (CubeWobble.cs, BasketBounce.cs, StationPulse.cs)
26. ❌ Basket tail visual system (animals in baskets instead of raw followers)

---

## 5. Strict Technical Rules (Unity 6)

> [!CAUTION]
> **NEVER use deprecated APIs. The following are STRICTLY FORBIDDEN:**

| ❌ FORBIDDEN | ✅ USE INSTEAD |
| :--- | :--- |
| `Input.GetKeyDown()` / `Input.GetAxis()` | New Input System `InputAction` |
| `UnityEngine.UI.Text` | `TMPro.TextMeshProUGUI` |
| `CinemachineVirtualCamera` | `CinemachineCamera` (Cinemachine v3) |
| Custom object pool | `UnityEngine.Pool.ObjectPool<T>` |
| `PlayerPrefs` for save data | JSON to `Application.persistentDataPath` |
| `FindObjectOfType<T>()` | `FindAnyObjectByType<T>()` or proper dependency injection |
| Legacy Sprite Packer | Sprite Atlas V2 |
| Old Tilemap brushes | Rule Tiles + Tile Palette |

---

## 6. Architecture Conventions

### Namespace Structure
```
StraySwarm.Core       — GameManager, GridManager, InputHandler, PlayerController, LevelManager
StraySwarm.Gameplay   — TailManager, PathHistory, FollowerBehavior, RescueStation, VanController, VanQueue, JuiceManager, CameraShake, ComboTracker, CubeWobble, BasketBounce, StationPulse
StraySwarm.Data       — LevelData, AnimalData, GameSettings
StraySwarm.Audio      — AudioManager
StraySwarm.UI         — UIManager, MainMenuUI, LevelSelectUI, PauseMenuUI, SettingsUI
StraySwarm.Events     — GameEvent, GameEventListener
StraySwarm.Utils      — ObjectPoolManager, SaveManager
```

### Folder Structure
```
Assets/_StraySwarm/
├── Animations/       — Animator Controllers, Animation Clips
├── Art/
│   ├── Characters/   — Animal & cat sprite sheets
│   ├── Environment/  — Tile sets, props, backgrounds
│   └── UI/           — Buttons, panels, stars, icons
├── Audio/
│   ├── BGM/          — Background music .ogg files
│   └── SFX/          — Sound effect .wav files
├── Data/
│   ├── Animals/      — AnimalData ScriptableObject assets
│   ├── Levels/       — LevelData ScriptableObject assets
│   └── Settings/     — GameSettings ScriptableObject asset
├── Materials/        — URP Sprite-Lit materials
├── Prefabs/
│   ├── Characters/   — Player, Animal prefabs
│   ├── Core/         — Grid, Station prefabs
│   ├── Environment/  — Deco prop prefabs
│   └── UI/           — Panel prefabs
├── Scenes/
│   ├── 0_Boot.unity
│   ├── 1_MainMenu.unity
│   ├── 2_LevelSelect.unity
│   └── 3_Gameplay.unity
├── Scripts/          — (see namespace structure above)
└── Tiles/            — Rule Tile assets, Tile Palettes
```

### Scene Architecture
```
0_Boot (Persistent)
  └── Spawns: AudioManager, LevelManager, SaveManager, CurrencyManager
  └── Loads: 1_MainMenu additively

1_MainMenu
  └── Title, Play button, Settings button, coin display

2_LevelSelect
  └── Scrollable level nodes with star/lock state from SaveManager

3_Gameplay (loaded per level)
  └── Grid, Canvas, GameManager, GridManager, InputHandler, PlayerController,
      TailManager, PathHistory, RescueStation, VanQueue, JuiceManager, UIManager
```

---

## 7. Production Roadmap Phases

The full production plan is documented in:
- **`Docs/00_FINAL_OVERVIEW_PLAN.md`** — Original 8-phase plan
- **Artifact: `project_audit_and_roadmap.md`** — Updated 9-phase production roadmap (Phases A through I)

**Priority order for remaining work:**
1. Phase A: Architecture Refactor (Event Bus, AnimalData, Pooling, SaveSystem, Multi-Station, Obstacle System)
2. Phase B: Art Production (Blender renders, Rule Tile sets for 4 worlds, UI art, drop shadows)
3. Phase C: Audio Production (SFX, BGM)
4. Phase D: Complete UI/UX (Boot, MainMenu, World Map LevelSelect, Pause, Settings, Pre-Level screen)
5. Phase E: Gameplay Completion (Combo, Cinemachine, DOTween, Lighting, Haptics, Tutorial, One-Way Arrows, Numbered Walls, Bridges)
6. Phase F: Content Creation (50+ levels across 4 themed worlds: Desert, Forest, Winter, City)
7. Phase G: Polish & Optimization (Profiling, Sorting, Accessibility, Testing)
8. Phase H: Monetization & Meta (Currency, Shop, World-Unlock Gates, Ads, Analytics)
9. Phase I: Publishing (Store builds, listings, release)

**Detailed roadmap with all sub-tasks is in the artifact: `project_audit_and_roadmap.md`**

---

## 8. First Steps for a New AI Session

If you are starting a new conversation on this project, do the following IN ORDER:

1. **Read this file** (`Docs/AI_AGENT_INSTRUCTIONS.md`) completely.
2. **Read** `Docs/00_FINAL_OVERVIEW_PLAN.md` for the full game vision.
3. **Scan** the `Scripts/` folder to see current codebase state.
4. **Ask the user** what they want to work on next.
5. **Check** which Phase (A–I) the requested work falls under.
6. **Follow** the teaching standard: step-by-step, explain why, full code, verify.

> [!IMPORTANT]
> **NEVER** skip reading this file. **NEVER** assume what has or hasn't been built. **ALWAYS** scan the codebase first. The project state described in this file may be outdated — the codebase is the source of truth.

---

## 9. Quality Bar

The user has explicitly stated:
> *"I hope we make it a breathtakingly beautiful game in both looks and feel!"*
> *"Our graphics will be much better, best in fact, and gameplay super smooth and animations and ui ux top notch."*

**The quality bar is: TOP-TIER MOBILE GAME.** Think Crossy Road, Monument Valley, Alto's Odyssey level of polish. Every interaction must feel satisfying. Every screen must look premium. No placeholder art in the final build.
