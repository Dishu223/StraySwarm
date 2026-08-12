# 🧃 Game Juice Bible: Stray Swarm

> **"A game without juice is like a dry sponge. A game with juice is a water balloon ready to burst."**

This document is the definitive guide to the "feel" of **Stray Swarm**. It contains the exact values, easing curves, particle counts, and interaction timings necessary to make the game feel incredible. 

---

## 1. What is Game Juice? 🍊

"Juice" is the feedback a game provides to the player's actions. It transforms a sterile, mechanical system into a living, tactile experience. As outlined by Jan Willem Nijman in *The Art of Screenshake* and Martin Jonasson & Petri Purho in *Juice It or Lose It*, game juice makes every action feel rewarding, powerful, and significant.

In **Stray Swarm**, our goal is hyper-tactile satisfaction. Every swipe, collect, and drop-off must feel snappy, elastic, and delightful.

---

## 2. The Juice Framework 📐

Every significant interaction in Stray Swarm adheres to the **Feedback Triangle**:

1. **Visual:** A change in the game state visible to the player (Animations, VFX, Color flashes).
2. **Audio:** A sound effect that matches the material and weight of the action.
3. **Haptic:** Physical feedback (Vibration/Rumble) on the device, scaling with the action's importance.

| Action Type | Visual Feedback | Audio Feedback | Haptic Feedback |
| :--- | :--- | :--- | :--- |
| **Micro (Footstep)** | Tiny dust puff | Light tap/pat | None |
| **Standard (Collect)** | Squash & Stretch, Sparkles | Satisfying 'pop' | Light impact |
| **Macro (Combo x10)** | Screen flash, Trail glow | Major chord chime | Heavy impact |
| **Meta (Level Clear)**| Confetti, Camera zoom | Triumphant fanfare| Continuous rumble |

---

## 3. Animation Specifications 🎬

We rely heavily on procedural animation via **DOTween** for maximum elasticity. Ensure DOTween v1.3.030+ is installed for Unity 6 compatibility. Run the DOTween Utility Panel after installation.

Unity 6 supports native async/await via `Awaitable`. For complex animation sequences that aren't timing-critical, consider `await Awaitable.WaitForSecondsAsync()` as a cleaner alternative to coroutines. 

### 🐾 Character Movement
*   **Player Walk:** 
    *   **Body Bob:** Sine wave Y-offset. 
    *   *Parameters:* Amplitude `0.05`, Frequency `8Hz`.
    *   **Legs:** 4-frame cycle sprite animation (12 FPS).
*   **Follower Walk:**
    *   Same body bob parameters as the player.
    *   *Offset:* Phase offset based on `tailIndex`. Formula: `time * frequency + (tailIndex * 0.2)`. This creates a mesmerizing rippling wave effect down the conga line.

### ✨ Collection & Interaction
*   **Collect Animal (The Pop):**
    *   *Phase 1 (Anticipation):* Scale to `1.3x` uniform for `0.05s` (Ease: `OutQuad`).
    *   *Phase 2 (Vanish):* Scale to `0x` for `0.1s` (Ease: `InBack`).
    *   *Phase 3 (Reappear at Tail):* Scale from `0x` to `1.2x` overshoot for `0.15s` (Ease: `OutBack`).
    *   *Phase 4 (Settle):* Scale to `1.0x` for `0.1s` (Ease: `InOutSine`).
    *   *Total Time:* `0.4s`.
*   **Gap Close Dash:**
    *   When middle animals are removed, the tail snaps forward.
    *   *Squash:* Scale X to `1.3`, Scale Y to `0.7` for `0.1s`.
    *   *Move:* Rapid translation to new grid position.
    *   *Settle:* Elastic bounce back to `(1.0, 1.0)` over `0.2s` (Ease: `OutElastic`, Overshoot: `1.5`).

### 🚐 Van Interactions
*   **Van Door Open:**
    *   Z-Axis Rotation to target open angle.
    *   *Duration:* `0.3s`.
    *   *Ease:* `OutBack` (creates a snappy, spring-loaded feel).
*   **Animals Board Van:**
    *   *Movement:* DOTween Jump arc (`DOJump`).
    *   *Height:* `0.5` units.
    *   *Duration:* `0.3s` per animal.
    *   *Stagger:* `0.1s` delay per `tailIndex`.
*   **Van Depart:**
    *   *Anticipation:* Bounce down (`PunchPosition` Y `-0.1` for `0.2s`).
    *   *Departure:* Accelerate off-screen along X-axis for `0.8s` (Ease: `InBack`).

### 🏆 UI & Meta-Game
*   **Button Press:**
    *   Scale to `0.9` instantly on pointer down.
    *   Scale back to `1.0` on pointer up/click.
    *   *Duration:* `0.15s`.
    *   *Ease:* `OutBack`.
*   **Star Reveal (End Screen):**
    *   Each star spins and scales in.
    *   *Scale:* `0` to `1.0`.
    *   *Rotation:* `0` to `360` degrees.
    *   *Duration:* `0.4s` per star.
    *   *Ease:* `OutBack`.
    *   *Stagger:* `0.3s` delay between stars.
*   **Timer Urgency (Under 25% Time):**
    *   Text color shifts to Red.
    *   *Pulse:* Scale loops `1.0` ↔ `1.1`.
    *   *Duration:* `0.25s` per cycle (Ping-Pong loop).
*   **Level Complete Celebration:**
    *   All followers in tail perform a synchronized `DOJump` (Height: `0.3`, Duration: `0.4s`).
*   **Floating Text (+1, Combo!):**
    *   *Movement:* Rise `1.0` unit (Y-axis).
    *   *Alpha:* Fade out from `1.0` to `0.0`.
    *   *Duration:* `0.8s` (Ease: `OutQuart`).

---

## 4. Particle Effects 🎇

Every action has a corresponding VFX burst. Use Unity's Particle System (Shuriken).

| Effect Name | Trigger | Particle Count | Color | Lifetime | Start Size | Speed | Gravity | Other Details |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **Collect Sparkle** | Animal collected | 15-20 | Match animal | 0.5s | 0.1 - 0.3 | 2 - 4 | 0 | Burst emission |
| **Confetti Burst** | Level complete | 30-50 | Multi (Pastels) | 1.5s | 0.2 - 0.4 | 5 - 8 | 1.0 | Drag: 2, 3D Rotation |
| **Dash Trail** | Gap close dash | 5-8 | White/Cyan | 0.2s | 0.1 - 0.2 | 0 (static) | 0 | Stretched billboard, Speed lines |
| **Star Earn** | End screen star | 20 | Gold/Yellow | 1.0s | 0.1 - 0.3 | 3 - 5 | 0.5 | Gravity modifier active |
| **Footstep Dust** | Player move | 3-5 | Light Gray | 0.3s | 0.05 - 0.1 | 0.5 - 1 | 0 | Every 3 steps |
| **Van Exhaust** | Van departs | 5 | Dark Gray | 0.8s | 0.3 - 0.6 | 1 - 2 | -0.1 | Rise up slowly, fade out |
| **Combo Fire** | Combo > 10 | Infinite | Orange/Red | 0.4s | 0.2 - 0.4 | 0 | 0 | Trails behind player |
| **Level Complete** | Win screen | 100+ | Multi-color | 3.0s | 0.2 - 0.5 | 8 - 12 | 1.0 | Fireworks + Rain setup |
| **Timer Warning** | Time < 25% | Screen FX | Red Gradient | N/A | N/A | N/A | N/A | Full-screen vignette pulse |

---

## 5. Camera Effects 🎥

The camera is the player's eye. It must feel active but never nauseating. Use Cinemachine v3 (Unity 6). Note: `CinemachineVirtualCamera` is renamed to `CinemachineCamera`.

### Screen Shake
Use a Cinemachine Impulse setup.

*   **Micro Shake (Collect):** Amplitude `0.1`, Frequency `10`, Duration `0.1s`.
*   **Medium Shake (Van Depart):** Amplitude `0.3`, Frequency `15`, Duration `0.2s`.
*   **Macro Shake (Combo x10 Milestone):** Amplitude `0.5`, Frequency `20`, Duration `0.3s`.
*   *Decay Curve:* Always use exponential decay so shakes resolve smoothly.

### Movement & Zoom
*   **Smooth Follow:** 
    *   Cinemachine Framing Transposer. 
    *   *Damping Factor:* `0.1` (X and Y).
    *   *Look-Ahead:* Slight bias in the direction of player movement.
*   **Zoom Pulse (Combo Milestones):** 
    *   Briefly zoom in `5%` (reduce orthographic size).
    *   *Duration:* `0.15s`.
    *   *Ease:* Quick snap in, smooth ease out.
*   **Final Van Slow-Motion:**
    *   When the last required animal boards the van.
    *   *TimeScale:* Drop to `0.5`.
    *   *Duration:* `0.3s` (Unscaled time).
    *   *Transition:* Tween timeScale back to `1.0` over `0.2s`.

---

## 6. Combo System Deep Dive 🔥

The combo system rewards aggressive, continuous play without stopping.

### Mechanics
*   **Increment:** +1 to Combo Counter for every animal collected.
*   **Timer:** The combo window is `2.0` seconds. If no animal is collected within this time, the combo resets to 0.
*   **Score Multiplier:** Base Score x `(1 + (ComboCount * 0.1))`.

### Escalation Tiers
As the combo climbs, the game's audiovisual intensity scales up:

1.  **Combo x3 (Warming Up):**
    *   *Visual:* Small sparkles emit from the player.
    *   *Audio:* Pitch of collect sound increases by +1 semitone per collect.
2.  **Combo x5 (Heating Up):**
    *   *Visual:* Player gains a glowing trail. Combo counter UI shakes on increment.
    *   *Audio:* Synthesizer chord backing track fades in.
3.  **Combo x10+ (On Fire!):**
    *   *Visual:* Screen flashes white (Alpha 0.2 -> 0.0 over 0.1s). "Combo Fire" particles activate. 
    *   *Audio:* Major chord chime plays. Pitch shift caps out at +1 octave.

---

## 7. Screen Transitions 🕳️

Never use hard cuts. Always transition with style.

*   **Standard Transition:** Cat-paw shaped mask scales up to reveal the new scene.
    *   *Out:* Mask scales from `100x` to `0x` (Ease: `InExpo`), centered on the player character.
    *   *In:* Mask scales from `0x` to `100x` (Ease: `OutExpo`).
    *   *Duration:* `0.6s` each way.
*   **Alternative:** Iris wipe (circle mask) or directional sliding panels matching the UI colors.

---

## 8. UI Juice 📱

UI must feel tactile and responsive.

*   **Panels:** 
    *   Slide in from off-screen edges.
    *   *Overshoot:* Use `OutBack` easing to make panels bounce slightly past their target position before settling.
*   **Number Counters (Score/Coins):**
    *   Never snap numbers instantly.
    *   Use DOTween's `DOCounter` to rapidly scroll through numbers.
    *   Pitch up a subtle 'tick' sound while numbers are rolling.
*   **Hover States:**
    *   Scale button to `1.05x`.
    *   Slight rotation (`+/- 2` degrees).

---

## 9. DOTween Cheat Sheet 📈

Consistency in easing curves defines the game's character. Use this guide:

| Ease Type | Visual Description | When to Use It |
| :--- | :--- | :--- |
| **OutBack** | Overshoots target, then settles back. Springy. | UI panels appearing, button pop-ups, animals settling into place. |
| **InBack** | Pulls back in opposite direction, then shoots forward. | Things leaving the screen (Van departures, UI closing). |
| **InOutQuad** | Smooth acceleration and deceleration. | Standard object movement, camera pans. |
| **OutElastic** | Wobbly, jelly-like bounce. High energy. | Gap close dash settling, error buzzes, high-impact collisions. |
| **InExpo** | Starts incredibly slow, ends incredibly fast. | Mask transitions out. |
| **Linear** | Constant speed. No character. | **ALMOST NEVER.** Use only for pure rotation loops or specific progress bars. |

> [!TIP]
> **Overshoot parameter:** When using `OutBack`, tweak the overshoot parameter. Default is `1.70158`. Increase it to `2.5` for extremely exaggerated cartoon physics.

---

## 10. Performance Considerations ⚡

Juice is great, but frame drops are not. Mobile performance is critical.

*   **Particle Budgets:** Max active particles on screen should not exceed `300`. Use object pooling for all particle systems. Use Unity 6 built-in `UnityEngine.Pool.ObjectPool<T>` for pooling particle effect instances instead of custom pooling.
*   **Animation Pooling:** DOTween tweens are generally cheap, but rapidly creating/destroying thousands of tweens causes garbage collection (GC) spikes. 
    *   Use `SetRecyclable(true)` for frequently used tweens (like the walk cycle bob).
    *   Kill tweens explicitly in `OnDestroy` or `OnDisable` to prevent memory leaks.
*   **Animator vs. DOTween:**
    *   Use **Animator (Mecanim)** *only* for sprite-sheet frame flipping (e.g., the 4-frame walk cycle).
    *   Use **DOTween** for all transform manipulation (Scale, Rotation, Position). It is faster to edit in code and provides vastly superior easing control compared to Animation Curves in the Unity Editor.
*   **Material Instancing:** When flashing sprites white for hit-stop/combo effects, use a shared Material Property Block, do *not* create new material instances per sprite.

---

## 11. URP 2D Post-Processing for Juice 🌈

*   Unity 6 URP 2D Renderer fully supports Volume-based post-processing
*   Use Bloom for glowing particle effects and player glow
*   Use Color Adjustments for timer urgency (desaturation or red shift)
*   Use Vignette for screen edge effects (timer warning)
*   Add via: Camera > enable Post Processing, then add Global Volume with a Volume Profile
*   NOTE: Do NOT use the legacy Post-Processing Stack v2

---
*End of Game Juice Bible. Now go make it feel amazing.*
