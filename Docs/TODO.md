# 📋 Stray Swarm — Master TODO Tracker

> **Last Updated:** 2026-08-12
> **Legend:** `[ ]` Todo · `[/]` In Progress · `[x]` Done · `[~]` Deferred

---

## Phase 0: Pre-Production ✨ (Current)

### Documentation & Planning
- [x] Write project README
- [x] Create .gitignore (Unity-specific)
- [x] Write Game Design Document (GDD)
- [x] Write Technical Architecture doc
- [x] Write Art Bible
- [x] Write Audio Bible
- [x] Write Game Juice Bible
- [x] Write Level Design Guide
- [x] Write Asset Tracker
- [x] Write Publishing Guide
- [x] Write Tips & Tricks
- [x] Write Ideas Backlog
- [x] Create Changelog
- [x] Create this TODO tracker

### Project Setup
- [ ] Install Unity 6 (6000.x) via Unity Hub
- [ ] Create new 2D URP project in `UnityProject/`
- [ ] Configure URP 2D Renderer settings
- [ ] Set target resolution (1080x1920 portrait)
- [ ] Set target frame rate (60 FPS)
- [ ] Import DOTween (free) and run setup wizard
- [ ] Import TextMeshPro essentials
- [ ] Set Asset Serialization to Force Text
- [ ] Create folder structure inside `Assets/_StraySwarm/`
- [ ] Create Assembly Definition files for script folders
- [ ] Initialize Git repository and make first commit
- [ ] Configure Git LFS for art/audio assets

---

## 🏃 Phase 1: Core Movement (COMPLETED)
- [x] Create Unity 6 Project (2D URP)
- [x] Set up folder structure
- [x] Install New Input System
- [x] Import DOTween & TMP
- [x] Configure build settings (Mobile Portrait)
- [x] Initialize Git repository
- [x] Create `NodeData` structure
- [x] Create `GridManager` (generates test grid)
- [x] Create `InputHandler` (swipe detection via New Input System)
- [x] Create `PlayerController` (node-to-node movement)
- [x] Implement Input Buffering
- [x] Implement Auto-Cornering
- [x] Add Editor Gizmos for grid visualizationg intersection)
- [ ] Add 180° turn prevention
- [ ] Test on device with Unity Remote

### Player Controller
- [ ] Create `PlayerController.cs` (node-to-node Lerp movement)
- [ ] Implement constant speed movement
- [ ] Implement direction change only at nodes
- [ ] Add auto-start (player moves forward on level begin)
- [ ] Create placeholder player sprite (colored square is fine)

### Camera
- [ ] Set up Cinemachine 2D camera (or manual smooth follow)
- [ ] Configure follow damping and dead zone
- [ ] Add look-ahead in movement direction
- [ ] Clamp to level boundaries

### Milestone Checklist
- [ ] Player moves on grid with swipe controls ✅
- [ ] Input buffer works (swipe early, turn executes at node) ✅
- [ ] 180° turn prevented ✅
- [ ] Camera follows smoothly ✅

---

## Phase 2: Tail / Conga Line System (Week 2)

### Path History
- [ ] Create `PathHistory.cs` (position + rotation queue)
- [ ] Record position at fixed distance intervals (every 0.1 units)
- [ ] Visualize path with debug line (editor only)

### Tail Manager
- [ ] Create `TailManager.cs` (LinkedList of followers)
- [ ] Implement `AddFollower()` — append to end of tail
- [ ] Implement `RemoveFollower(index)` — remove from any position
- [ ] Implement `GetMatchingFollowers(color)` — return list of color matches

### Follower Behavior
- [ ] Create `FollowerBehavior.cs` (follows PathHistory by index)
- [ ] Implement smooth movement along path
- [ ] Implement configurable spacing between followers
- [ ] Add sine-wave body bob (wobble animation)
- [ ] Add phase-offset per tail index (wave effect)

### Stray Animal Prefabs
- [ ] Create `AnimalData.cs` ScriptableObject (color, sprites, sounds)
- [ ] Create 6 `AnimalData` assets (Blue, Pink, Yellow, Green, Orange, Purple)
- [ ] Create generic stray animal prefab with color tinting
- [ ] Place test animals on grid nodes

### Collection Logic
- [ ] Detect player reaching a node with an animal
- [ ] Trigger collection → animal joins tail
- [ ] Raise `OnAnimalCollected` event
- [ ] Placeholder collection VFX (scale pop)

### Self-Crossing / Z-Sorting
- [ ] Create `DynamicDepthSort.cs` (Y-based sortingOrder)
- [ ] Attach to player and all followers
- [ ] Verify visual overlap looks correct when crossing tail

### Milestone Checklist
- [ ] Animals on grid, player collects them ✅
- [ ] Followers form a conga line behind player ✅
- [ ] Self-crossing works visually ✅

---

## Phase 3: Rescue Station & Delivery (Week 3)

### Rescue Station
- [ ] Create `RescueStation.cs` (trigger zone at station location)
- [ ] Create station placeholder sprite
- [ ] Detect player passing through station

### Van System
- [ ] Create `VanController.cs` (state machine: Waiting → Filling → Full → Departing)
- [ ] Van has color property and capacity
- [ ] Van visually shows how many animals are boarded (dots/icons)
- [ ] Create `VanQueue.cs` (manages ordered sequence of vans per level)
- [ ] Next van slides in after current van departs

### Drop-Off Logic
- [ ] When player passes station, scan tail for matching colors
- [ ] Detach matching animals from tail (in order, up to van capacity)
- [ ] Raise `OnAnimalBoarded` event per animal
- [ ] Raise `OnVanFilled` event when van is full
- [ ] Trigger van departure sequence

### Gap Closing
- [ ] When a middle follower is removed, followers behind dash forward
- [ ] Implement speed burst with squash animation
- [ ] Reconnect path indices after removal

### Level Data
- [ ] Create `LevelData.cs` ScriptableObject
- [ ] Define all fields (grid, spawns, van queue, timer, star thresholds)
- [ ] Create first test level asset

### Milestone Checklist
- [ ] Full gameplay loop works: collect → deliver → van departs ✅
- [ ] Gap closing animation works ✅
- [ ] Multiple vans in sequence ✅

---

## Phase 4: Game Loop & UI (Week 4)

### Game Manager
- [ ] Create `GameManager.cs` (level lifecycle, state machine)
- [ ] Implement game states: Loading → Playing → Paused → Won → Lost
- [ ] Level loading from LevelData (spawn grid, animals, vans)
- [ ] Timer countdown
- [ ] Win condition check (all vans filled)
- [ ] Loss condition check (timer expired)
- [ ] Star rating calculation

### Object Pooling
- [ ] Create `ObjectPoolManager.cs` (generic pool)
- [ ] Pool stray animal instances
- [ ] Pool VFX particle instances
- [ ] Pool floating text instances

### Spawning
- [ ] Create `AnimalSpawner.cs` (reads LevelData, spawns from pool)
- [ ] Create `VanSpawner.cs` (reads LevelData, creates van queue)

### UI — HUD
- [ ] Create `HUDController.cs`
- [ ] Timer bar (gradient fill: green → yellow → red)
- [ ] Van queue display (upcoming van colors + capacity dots)
- [ ] Tail count indicator
- [ ] Combo counter (fades when inactive)
- [ ] Pause button

### UI — Results Screen
- [ ] Create `ResultsScreen.cs`
- [ ] Star animation (spin in one by one)
- [ ] Stats display (time, animals saved, best combo)
- [ ] Retry / Next / Home buttons

### UI — Main Menu
- [ ] Create `MainMenuController.cs`
- [ ] Play button, Settings button
- [ ] Animated background (subtle parallax or moving clouds)

### UI — Level Select
- [ ] Create `LevelSelectController.cs`
- [ ] Scrollable level grid
- [ ] Star display per level
- [ ] Lock/unlock states
- [ ] Current level highlight

### UI — Settings
- [ ] Music volume slider
- [ ] SFX volume slider
- [ ] Vibration toggle
- [ ] Credits link

### UI — Pause Screen
- [ ] Resume, Restart, Home buttons
- [ ] Dim/blur background

### Scene Transitions
- [ ] Circle/iris wipe transition effect
- [ ] Smooth scene loading

### Save System
- [ ] Create `SaveManager.cs`
- [ ] Save: level stars, current level unlocked, settings
- [ ] Use JSON file (Application.persistentDataPath)
- [ ] Auto-save on level complete and settings change

### Milestone Checklist
- [ ] Complete playable game from menu to level to results ✅
- [ ] 5 tutorial/intro levels designed and playable ✅
- [ ] Settings save and persist ✅

---

## Phase 5: Art & Animation (Week 5-6)

### Character Art
- [ ] Player Cat — all animation sets
- [ ] Blue Puppy — all animation sets
- [ ] Pink Kitten — all animation sets
- [ ] Yellow Pigeon — all animation sets
- [ ] Green Frog — all animation sets
- [ ] Orange Hamster — all animation sets
- [ ] Purple Bunny — all animation sets
- [ ] Set up Sprite Atlases

### Environment Art
- [ ] Road tileset (all variants)
- [ ] Sidewalk/border tileset
- [ ] Grass/park decorative tiles
- [ ] Building decorative tiles
- [ ] Props (trash can, hydrant, bench, lamp, etc.)
- [ ] Rescue station sprite
- [ ] Build final level art for 5+ levels

### Vehicle Art
- [ ] Van sprites (6 color variants, all states)

### UI Art
- [ ] All button sprites
- [ ] All icon sprites
- [ ] Panel backgrounds
- [ ] Timer bar assets
- [ ] Logo / title art
- [ ] Transition mask

### Animation Setup
- [ ] All character Animator controllers
- [ ] All UI animations (in/out transitions)
- [ ] DOTween sequences for all juice animations

### Milestone Checklist
- [ ] All placeholder art replaced with final art ✅
- [ ] All animations smooth and polished ✅

---

## Phase 6: Juice & Polish (Week 6-7)

### Particle Effects
- [ ] Collect sparkle (color-matched)
- [ ] Confetti burst (van depart)
- [ ] Dash trail (gap closing)
- [ ] Timer warning vignette
- [ ] Star earn sparkle
- [ ] Combo fire trail
- [ ] Footstep dust
- [ ] Van exhaust
- [ ] Level complete celebration

### Audio
- [ ] Menu BGM
- [ ] Gameplay BGM (base + urgency layer)
- [ ] All SFX (15+ sounds)
- [ ] Implement pitch-shifting for consecutive collects
- [ ] Implement dynamic music layer crossfade
- [ ] Audio settings integration (volume, mute)

### Camera Effects
- [ ] Screen shake system (per-event intensity)
- [ ] Zoom pulse on combo milestones
- [ ] Slow-motion on final van
- [ ] Vignette pulse on timer warning

### Haptics
- [ ] Light tap on collect
- [ ] Medium pulse on van filled
- [ ] Double tap on combo milestone
- [ ] Success pattern on level win

### Lighting (URP 2D)
- [ ] Global warm ambient light
- [ ] Street lamp point lights
- [ ] Rescue station spotlight
- [ ] Player subtle glow
- [ ] Combo glow intensity scaling

### Milestone Checklist
- [ ] Every interaction has visual + audio + haptic feedback ✅
- [ ] Game feels satisfying and "juicy" ✅

---

## Phase 7: Content & Tuning (Week 7-8)

### Level Content
- [ ] Design 20+ levels with progressive difficulty
- [ ] Verify all levels are completable with 3 stars
- [ ] Balance timer thresholds per level
- [ ] Balance difficulty curve (no sudden spikes)

### Performance
- [ ] Profile on real Android device (mid-range)
- [ ] Profile on real iOS device (if available)
- [ ] Verify 60 FPS stability
- [ ] Verify memory usage is reasonable
- [ ] Optimize draw calls (sprite atlasing, batching)
- [ ] Verify APK size < 100MB

### Testing
- [ ] Playtest with 5+ people unfamiliar with the game
- [ ] Verify no crashes on level transitions
- [ ] Verify save/load works correctly
- [ ] Verify settings persist across sessions
- [ ] Test edge cases (empty tail at station, timer exact-zero, etc.)

### Polish
- [ ] Final pass on all animations
- [ ] Final pass on all audio levels
- [ ] Final pass on all UI layouts (notch/safe area)
- [ ] Loading screen (if needed)
- [ ] Credits screen

### Milestone Checklist
- [ ] 20+ polished, playtested levels ✅
- [ ] Stable 60 FPS on target devices ✅
- [ ] Zero known crashes ✅

---

## Phase 8: Publishing (Week 9+)

### Store Preparation
- [ ] Create Google Play Developer account
- [ ] Create Apple Developer account (if targeting iOS)
- [ ] Write store listing (title, description, keywords)
- [ ] Create store screenshots (phone + tablet)
- [ ] Create feature graphic (Play Store)
- [ ] Create app icon (512x512 + 1024x1024)
- [ ] Record gameplay video / app preview
- [ ] Write privacy policy
- [ ] Complete content rating questionnaire

### Build & Submit
- [ ] Final production build (AAB for Android)
- [ ] Final production build (IPA for iOS, if applicable)
- [ ] Upload to internal testing track
- [ ] QA on internal track
- [ ] Promote to closed/open testing
- [ ] Fix any issues found in testing
- [ ] Promote to production
- [ ] 🎉 LAUNCH DAY! 🎉

---

## Future Phases (Post-Launch)
- [ ] Implement power-ups (v1.1)
- [ ] Add wildcard rainbow animal (v1.1)
- [ ] Add hazards/obstacles (v1.2)
- [ ] New world theme: Beach (v2.0)
- [ ] Player skin shop (v2.0)
- [ ] Leaderboards (v2.0)
- [ ] Daily challenges (v2.0)
- [ ] Level editor (v3.0)
- [ ] Multiplayer (v3.0+)

---

> *"A game is never finished, only abandoned. But let's abandon it at a really high quality bar."*
