# 🎨 Stray Swarm: Cube Aesthetic Art Bible

> **The definitive visual identity guide for Stray Swarm.** Every art, animation, and particle decision flows from this document.

---

## 🧊 Core Visual Identity: "Kawaii Cube World"

**In one sentence:** Everything in Stray Swarm is made of soft, rounded cubes—the characters, the obstacles, the collectibles—living in a clean, colorful world of smooth paths and vibrant backgrounds.

**Visual pillars:**
1. **Chunky & Rounded** — Soft edges on everything. No sharp corners. Even "stone" obstacles are rounded rectangles.
2. **Colorful & Readable** — Bold, saturated colors. Each animal type is instantly identifiable by color at a glance on a tiny phone screen.
3. **Alive & Bouncy** — Nothing is static. Everything wobbles, squashes, stretches, and bops. The game feels like a box of sentient jelly cubes.
4. **Consistent Depth** — Every object sits on the path with a soft oval drop shadow beneath it, giving the flat 2D world a grounded 3D feel.

---

## 🐱 Character Design System

### Player Cat (Cube Cat)
| Property | Value |
| :--- | :--- |
| **Shape** | Rounded square (like your reference image) |
| **Size** | Fills ~80% of one grid tile |
| **Face** | Simple dot eyes, tiny nose, whisker lines, small ears on top |
| **Shadow** | Soft dark oval sprite positioned beneath, slightly offset |
| **Animations** | NO traditional sprite animations. All motion via code: Wobble, Squash & Stretch, Tilt |
| **Directional Indicator** | Eyes shift subtly in movement direction. Small arrow or ear tilt. |
| **Skins** | Recolors + face variations (Tabby, Tuxedo, Calico, Knight, Crown, Astronaut, etc.) |

### Collected Animals (Cube Animals in Baskets)

This is the key creative decision that ties the whole aesthetic together:

**On the Map (Before Collection):**
- Each stray animal is a **small colored cube** (~0.4 tile size) sitting on the path.
- Each has a unique face/expression matching its species:
  - 🐶 **Blue Puppy** — Floppy ears on cube, tongue out
  - 🐱 **Pink Kitten** — Pointed ears, blush marks
  - 🐦 **Yellow Pigeon** — Small beak, tiny wing bumps on sides
  - 🐸 **Green Frog** — Big round eyes on top of cube
  - 🐹 **Orange Hamster** — Round ears, puffy cheeks
  - 🐰 **Purple Bunny** — Tall ears on top of cube

**In the Tail (After Collection):**
- When collected, the animal cube hops into a **small wooden basket/crate** that follows the player.
- The basket is a rounded wooden box with the animal peeking out from the top (head + ears visible above the rim).
- The basket has a subtle colored glow/band matching the animal's color for quick identification.
- This creates a beautiful visual: **a conga line of bouncing wooden baskets with cute cube heads poking out!**

**Why Baskets Work Perfectly:**
1. Matches the delivery crate stations at the top of the screen (visual storytelling: collect → carry in basket → deliver to matching crate).
2. Makes the tail readable: baskets are uniform in size, so even 20+ in a line look clean and organized.
3. Gives the animals a "rescued & safe" feeling (they're cozy in their basket!).
4. The basket rim hides the body, so we only need to draw the **top of each animal's head** for the in-tail sprite (much simpler art!).

---

## 🏗️ Delivery Stations (Crate Shelters)

| Property | Value |
| :--- | :--- |
| **Shape** | Larger wooden crate/box with open front facing the path |
| **Color Coding** | Colored banner/flag on top matching accepted animal type |
| **Interior** | Shows collected animal cubes stacking up inside as deliveries happen |
| **Full State** | Crate lid swings shut, sparkle burst, satisfied "ding!" |
| **Size** | ~1.5 tiles wide, placed at the edge of the play area (typically at top of screen) |
| **Shadow** | Larger oval shadow beneath |

---

## 🧱 Obstacle Sprites (Cube Aesthetic Consistent)

| Obstacle | Visual Style | Notes |
| :--- | :--- | :--- |
| **One-Way Arrow** | Rounded yellow square tile with white 3D arrow embossed on top | Rotates to face forced direction |
| **Rock Barrier** | Cluster of 2-3 rounded gray/brown cubes stacked | Slightly darker than path color |
| **Numbered Wall** | Single rounded stone cube with large bold white number (1, 2, 3) | Number shrinks with each hit, crumble particles on break |
| **Bridge** | Elevated wooden plank (2 cubes wide) with visible shadow gap underneath | Player cube visually "hops up" onto bridge level |

---

## 🌍 World Themes (Background + Path Palette)

Each world has a distinct color identity but ALL share the same Cube Aesthetic language.

| World | Background | Path Color | Path Outline | Edge Props |
| :--- | :--- | :--- | :--- | :--- |
| 🏜️ **Desert** | Warm Sand `#EDCB96` with subtle wave texture | Sandy Beige `#F5DEB3` | Soft brown `#D4A574` | Rounded cube cacti, tiny rock cubes, sand puff particles |
| 🌲 **Forest** | Vibrant Green `#5CB85C` with grass blades | Light Tan `#E8DCC8` | Earthy brown `#8B7355` | Cube bushes, flower cubes, mushroom cubes, tiny tree cubes |
| ❄️ **Winter** | Icy Blue `#B3D4E8` with frost crystal texture | Pale Blue-White `#E8EFF5` | Light blue `#9FC5E8` | Snowdrift cubes, icicle rectangles, pine tree triangles |
| 🏙️ **City** | Dark Asphalt `#4A4A4A` with subtle grid texture | Gray Cobblestone `#B0B0B0` | Dark gray `#666666` | Cube lamp posts, bench cubes, fire hydrant cubes |

---

## ✨ Animation System: "Wobble Physics" (No Sprite Sheets!)

> **We are NOT using traditional frame-by-frame sprite animations for gameplay.** All character life comes from code-driven transform manipulation. This is faster to develop, costs zero memory, and looks juicier than static frames!

### Core Animation Scripts

#### 1. `CubeWobble.cs` — The Heartbeat of Every Character
Every cube character (player + all followers) has this component:
- **Idle Bob:** Gentle up-down sine wave (`sin(time * 2) * 0.03f`), each entity has a random phase offset so they don't bob in sync.
- **Move Hop:** On each grid step, play a quick "hop": scale Y to 1.2 → back to 1.0 with overshoot (bounce ease). Scale X does the inverse (squash when Y stretches).
- **Turn Tilt:** When changing direction, briefly rotate Z by ±8° in the turn direction, then spring back to 0°.
- **Land Squash:** When arriving at a node: quick squash (scaleY=0.8, scaleX=1.2) → spring back to 1.0.

#### 2. `BasketBounce.cs` — Tail Basket Physics  
The baskets in the conga line have their own wobble layer:
- **Follow Lag:** Each basket trails 0.05s behind the one ahead, creating a natural wave motion.
- **Sway:** Slight left-right rotation oscillation as they move, like cargo being carried.
- **Collect Pop:** When an animal is first collected, the basket spawns with a punch scale (0 → 1.3 → 1.0) and a burst of sparkle particles.

#### 3. `StationPulse.cs` — Delivery Station Breathing
- **Idle Pulse:** Gentle scale pulse (1.0 → 1.02 → 1.0) on a slow 2-second cycle.
- **Attention Glow:** When the player is carrying matching animals, the station's colored banner subtly brightens/dims to draw attention.
- **Receive Bounce:** On each animal delivered, station does a satisfying "gulp" squash (widen briefly, then snap back).

---

## 🎆 Particle Effects (Cube Themed)

All particles should use **small rounded square shapes** (not circles!) to match the cube aesthetic.

| Effect | Trigger | Visual |
| :--- | :--- | :--- |
| **Collect Sparkles** | Animal picked up | Burst of tiny colored squares + one heart shape, radiating outward |
| **Basket Pop** | Basket appears in tail | Ring of white squares expanding outward + scale punch |
| **Delivery Zip** | Animal flies from basket to crate | Trail of colored squares along the flight arc |
| **Crate Fill Burst** | Animal enters crate | Color-matched confetti burst from crate opening |
| **Crate Complete** | Crate fully filled | Golden star squares shower + lid close animation |
| **Wall Crumble** | Numbered wall breaks at 0 | Gray stone-colored squares scatter with gravity |
| **Win Confetti** | Level complete | Multi-colored square confetti rain from top of screen |
| **Combo Text** | x3/x5/x10 combo | Floating text ("Nice!") with small squares trailing behind |

---

## 🖼️ UI Design (Cube Aesthetic Integrated)

### In-Game HUD
- Timer and score use the **Lilita One** font (already imported) with **white fill + deep purple outline** (matching reference screenshots).
- HUD background strips are **rounded rectangles with soft drop shadows**.
- Pause button: Rounded square icon in top-left corner.

### Win/Lose Panels
- Floating card with **extra-rounded corners** (border-radius feel).
- Stars are **rounded star shapes with golden fill and subtle bevel**.
- Buttons are chunky rounded rectangles with **3D press effect** (shadow shifts on tap).

### Main Menu
- Animated **Cube Cat mascot** in the center, doing its idle wobble bob.
- Multiple cube animal friends around it, each wobbling at different phases.
- "STRAY SWARM" title in thick bubbly white text with purple stroke.
- Big rounded "PLAY" button with wobble-on-hover.

### Level Select (World Map)
- Each world is a **colored rounded rectangle zone**.
- Level nodes are **small cube icons** showing 0-3 mini stars beneath.
- Locked levels show a tiny padlock cube on top.
- Current world has a gentle background color pulse.

---

## 📐 Size & Scale Reference

```
┌─────────────────────────────────┐
│  1 Grid Tile = 1.0 Unity Units  │
│                                 │
│  ┌─────────────┐                │
│  │ Player Cat  │ = 0.8 × 0.8   │
│  │  (Cube)     │                │
│  └─────────────┘                │
│                                 │
│  ┌────────┐                     │
│  │Stray   │ = 0.4 × 0.4        │
│  │Animal  │ (on map)            │
│  └────────┘                     │
│                                 │
│  ┌──────────┐                   │
│  │ Basket   │ = 0.6 × 0.6      │
│  │ (in tail)│ (with head peek)  │
│  └──────────┘                   │
│                                 │
│  ┌───────────────┐              │
│  │ Delivery Crate│ = 1.5 × 1.2 │
│  │  (Station)    │              │
│  └───────────────┘              │
│                                 │
│  Shadow = 70% of object width,  │
│  20% opacity black oval,        │
│  offset Y = -0.05 units         │
└─────────────────────────────────┘
```

---

## 🎯 Visual Consistency Checklist

Before adding ANY new visual element, check these rules:

- [ ] Is it made of rounded rectangles/squares? (No circles, no sharp edges)
- [ ] Does it have a soft drop shadow beneath it?
- [ ] Does it wobble/bounce when interacted with?
- [ ] Is its color from the established palette for its world theme?
- [ ] Does it use square-shaped particles (not circular)?
- [ ] Is it immediately readable at phone-screen distance?
- [ ] Does it match the scale reference above?
