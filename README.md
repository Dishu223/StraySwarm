# 🐾 Stray Swarm

> **A hypercasual puzzle-flow mobile game built with Unity 6 (2D URP)**

Navigate colorful city mazes as a street-smart stray cat. Swipe to turn at intersections, collect lost animals into your conga line, and deliver them to matching rescue vans — all against the clock!

---

## 📋 Project Info

| Detail | Value |
|---|---|
| **Engine** | Unity 6 (2D URP) |
| **Platform** | Mobile (Android & iOS) |
| **Genre** | Hypercasual / Puzzle-Flow |
| **Status** | 🟡 Pre-Production |

---

## 📂 Project Structure

```
Stray Swarm/
├── Docs/                          # All project documentation
│   ├── 00_FINAL_OVERVIEW_PLAN.md  # Master overview — the 30,000-foot view
│   ├── 01_GAME_DESIGN_DOCUMENT.md # Complete GDD — mechanics, rules, progression
│   ├── 02_TECHNICAL_ARCHITECTURE.md # Systems, modules, code architecture
│   ├── 03_ART_BIBLE.md           # Visual style, color palette, sprite specs
│   ├── 04_AUDIO_BIBLE.md         # Music, SFX, audio design principles
│   ├── 05_GAME_JUICE_BIBLE.md    # Animations, particles, camera, haptics
│   ├── 06_LEVEL_DESIGN_GUIDE.md  # How to design levels, difficulty curves
│   ├── 07_ASSET_TRACKER.md       # Checklist of every asset with status
│   ├── 08_PUBLISHING_GUIDE.md    # Play Store & App Store requirements
│   ├── 09_TIPS_AND_TRICKS.md     # Unity tips, performance, workflow secrets
│   ├── 10_IDEAS_BACKLOG.md       # Future features, brainstorming, experiments
│   ├── AI_AGENT_INSTRUCTIONS.md   # How the AI guides you through development
│   ├── CHANGELOG.md              # Version history
│   └── TODO.md                   # Master task tracker
├── UnityProject/                  # Unity 6 project (created in Phase 1)
│   └── Assets/
│       └── _StraySwarm/           # All game-specific assets namespaced here
├── References/                    # Inspiration screenshots, GIFs, competitor analysis
├── Builds/                        # Exported APK/IPA builds
├── .gitignore
└── README.md                     # ← You are here
```

---

## 🚀 Getting Started

### Prerequisites
- Unity 6 (6000.x) with 2D URP template
- DOTween (free) — install via Unity Package Manager or .unitypackage
- Git (for version control)

### Setup
1. Clone this repository
2. Open `UnityProject/` folder in Unity Hub
3. Import DOTween and run setup wizard
4. Open `Assets/_StraySwarm/Scenes/Boot.unity`
5. Hit Play!

---

## 📖 Documentation Index

| Doc | Description |
|---|---|
| [Final Overview Plan](Docs/00_FINAL_OVERVIEW_PLAN.md) | Master plan — architecture, roadmap, tech stack, everything |
| [Game Design Document](Docs/01_GAME_DESIGN_DOCUMENT.md) | The bible. Every mechanic, rule, and design decision |
| [Technical Architecture](Docs/02_TECHNICAL_ARCHITECTURE.md) | Code architecture, systems, modularity strategy |
| [Art Bible](Docs/03_ART_BIBLE.md) | Visual identity, color palette, Blender pipeline |
| [Audio Bible](Docs/04_AUDIO_BIBLE.md) | Sound design, music direction, implementation specs |
| [Game Juice Bible](Docs/05_GAME_JUICE_BIBLE.md) | The secret sauce — animations, particles, feel |
| [Level Design Guide](Docs/06_LEVEL_DESIGN_GUIDE.md) | How to create fun, balanced levels |
| [Asset Tracker](Docs/07_ASSET_TRACKER.md) | Every asset needed, with creation status |
| [Publishing Guide](Docs/08_PUBLISHING_GUIDE.md) | Store requirements, ASO, launch checklist |
| [Tips & Tricks](Docs/09_TIPS_AND_TRICKS.md) | Hard-won wisdom for Unity 6 mobile development |
| [Ideas Backlog](Docs/10_IDEAS_BACKLOG.md) | Feature ideas ranked by impact and effort |
| [AI Agent Instructions](Docs/AI_AGENT_INSTRUCTIONS.md) | How the AI guides the beginner developer |
| [Changelog](Docs/CHANGELOG.md) | What changed and when |
| [TODO](Docs/TODO.md) | Master task list with phase tracking |

---

## 🎮 Core Gameplay Loop

```
Swipe to Navigate → Collect Stray Animals → Deliver to Matching Vans → Earn Stars → Unlock Levels
```

---

## 📝 Development Philosophy

1. **Document First** — If it's not written down, it doesn't exist
2. **Modular Always** — Every system is a self-contained module
3. **Juice Everything** — Every interaction gets visual + audio + haptic feedback
4. **Test on Device** — Desktop play ≠ mobile play. Test on real phones weekly
5. **Ship It** — Perfect is the enemy of done. Ship, then polish

---

*Made with ❤️ and lots of game juice*
