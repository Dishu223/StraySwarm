# 🎨 Stray Swarm: Art Bible

> **"A warm, inviting, and playful world where urban animals find their way home."**

This document serves as the ultimate source of truth for the visual identity of **Stray Swarm**. It details the style, color palettes, character designs, UI aesthetics, environment rules, and technical specifications for all art assets in the game. All artists and developers must adhere to these guidelines to maintain a cohesive and premium visual experience.

---

## 1. 🌟 Art Direction Overview

The visual direction for Stray Swarm is defined by three core pillars: **Warm, Playful, and Cute.** The world should feel like a premium, polished toybox. We want players to feel relaxed and joyful, captivated by the squishy, satisfying interactions of the characters.

- **Warmth:** The world is inviting. Environments are brightly lit with soft, warm tones. The city is clean and idealized, not gritty or realistic.
- **Playfulness:** Everything has life. UI elements bounce, characters wobble, and interactions feel satisfyingly tactile.
- **Cuteness:** Characters are unmistakably charming. They feature exaggerated, rounded proportions with big heads, stubby bodies, and expressive faces.

> [!NOTE]
> The overarching tone is lighthearted puzzle-flow. There is no violence, danger, or failure states that invoke stress. The art must reflect this stress-free environment.

---

## 2. 🎭 Style References

Our visual style draws inspiration from a blend of successful mobile and indie titles, combining their best elements into a unique identity:

| Inspiration | Visual Elements Borrowed | Implementation in Stray Swarm |
| :--- | :--- | :--- |
| **Crossy Road** | Voxel-adjacent chunkiness, clean readable silhouettes. | Characters are chunky, easily identifiable, and have strong, clear outlines for readability on small screens. |
| **Pikuniku** | Wobbly personality, flat shading with bold colors. | Animations emphasize squash, stretch, and a loose, wobbly feel. Colors are unashamedly vibrant. |
| **Monument Valley** | Clean elegance, harmonious palettes, satisfying geometry. | Environments are mathematically precise (grid-based) but softened with pastel colors and smooth lighting. UI is minimalist. |
| **Tamagotchi** | Pet-like attachment, distinct iconic character shapes. | Each animal has a distinct, memorable silhouette that translates well to merchandise and icons. |

---

## 3. 🎨 Color Palette

Color is critical to Stray Swarm's gameplay. The six primary colors dictate the matching mechanics. Our palette relies on vibrant, pastel-leaning midtones for characters, set against softer, less saturated backgrounds to ensure readability.

### 🔴 Primary Colors (Animals & Vans)

These colors are gameplay-critical. They must never be altered or used for decorative background elements to prevent player confusion.

| Character / Element | Color Name | Hex Code | RGB | CMYK | Swatch |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Blue Puppy** | Sky Blue | `#5CB8FF` | `92, 184, 255` | `64, 28, 0, 0` | 🟦 |
| **Pink Kitten** | Bubblegum | `#FF7EB3` | `255, 126, 179` | `0, 51, 30, 0` | 🟪 |
| **Yellow Pigeon** | Sunbeam | `#FFCC02` | `255, 204, 2` | `0, 20, 99, 0` | 🟨 |
| **Green Frog** | Minty Green | `#7ED89E` | `126, 216, 158` | `42, 0, 27, 15`| 🟩 |
| **Orange Hamster** | Tangerine | `#FF9F43` | `255, 159, 67` | `0, 38, 74, 0` | 🟧 |
| **Purple Bunny** | Lavender | `#A29BFE` | `162, 155, 254` | `36, 39, 0, 0` | 🟪 |

### 🌆 Background & Environment Colors

The environment uses low-contrast, warm colors to allow the primary characters to pop.

| Element | Color Name | Hex Code | Usage |
| :--- | :--- | :--- | :--- |
| **Sidewalk/Base** | Warm Cream | `#FFF5E6` | Default walkable path. High brightness, low saturation. |
| **Road/Grid** | Soft Asphalt | `#DCDDE1` | The movement grid. Slightly cooler than the sidewalk. |
| **Buildings/Walls** | Soft Charcoal | `#4A4A5A` | Solid barriers and background verticality. |
| **Foliage Base** | Muted Sage | `#B8E994` | Trees, bushes, planters. Distinct from Green Frog. |

### 📱 UI Palette

UI elements use high-contrast colors to differentiate them from the game world.

| Element | Color Name | Hex Code | Usage |
| :--- | :--- | :--- | :--- |
| **Primary Text/Panels** | Deep Indigo | `#2D3561` | Main text color, modal backgrounds, heavy borders. |
| **CTA / Action Buttons**| Bright Coral | `#FF6B6B` | Play button, purchase buttons, important notifications. |
| **Base / Whitespace** | Pure White | `#FFFFFF` | Text inside colored buttons, clean UI panel interiors. |
| **Success/Stars** | Gold | `#FDCB6E` | 3-star ratings, combo counters, victory screens. |

### 🛑 Color Usage DO's and DON'Ts

- **DO** use the exact hex codes for the primary animals and their corresponding vans.
- **DO** use a subtle vertical gradient (darker at the bottom) on flat color fills to add volume.
- **DON'T** use any of the 6 primary colors for environment props (e.g., don't make a blue car that matches the Blue Puppy).
- **DON'T** use pure black (`#000000`) for shadows; use a multiplied, cool-toned dark color (like Deep Indigo at 30% opacity).

---

## 4. 🐾 Character Design Guidelines

Characters are the heart of Stray Swarm. They must be instantly recognizable, expressive, and fun to watch in motion.

### Proportions & Silhouette

The core rule of Stray Swarm characters is **Oversized and Squishy**.

- **Head to Body Ratio:** The head should make up approximately **40% to 50%** of the character's total mass.
- **Limbs:** Arms and legs are stubby, rounded nubs. No elbows or knees are articulated.
- **Eyes:** Wide-set, large, simple black dots or rounded ovals. Expressive eyebrows (floating lines) are encouraged.

```text
ASCII Proportion Mockup (The "Squish" Template)
    ______
  /        \   <-- Head (40-50% mass)
 |  O    O  |  <-- Wide-set eyes
  \   --   /   <-- Tiny mouth
   --------
  /        \   <-- Stubby Body
 |          |
  \        /
   --    --    <-- Nubby feet
```

### Art Style Rules

1. **Outlines:** All character sprites (Player and Followers) must have a **2-3px dark outline**. This outline is not pure black; it is a very dark, tinted version of the character's primary color (e.g., the Yellow Pigeon has a dark brown outline).
2. **Shading:** Flat shading with a very subtle, baked-in gradient (lighter on top, darker on bottom) to simulate soft top-down lighting. No harsh cell shading.
3. **Detail Level:** Minimal. Avoid fur textures or complex clothing. Rely on bold shapes.

### The Player: The Street-Smart Stray Cat

- **Color:** Warm Charcoal (`#353b48`) with bright white paws and a white muzzle.
- **Features:** Slightly nicked ear (shows character), bright yellow eyes (`#fbc531`), a long swishy tail with a white tip.
- **Personality:** Confident, slightly mischievous, but a caring leader.
- **Accessories:** A bright red collar with a golden bell (adds a tiny splash of accent color).

### The Followers (The Swarm)

| Animal | Distinctive Feature | Shape Language |
| :--- | :--- | :--- |
| **Blue Puppy** | Floppy ears, panting tongue. | Rounded rectangle. |
| **Pink Kitten** | Pointy ears, tiny paws. | Triangle/Diamond base. |
| **Yellow Pigeon** | Puffed chest, bobbing head. | Oval, teardrop. |
| **Green Frog** | Huge eyes on top of head. | Wide, squashed oval. |
| **Orange Hamster** | Chubby cheeks, round body. | Perfect circle. |
| **Purple Bunny** | Long ears dragging slightly. | Tall oval. |

### 🛠️ Optimization: The Shared Body Rig

To optimize performance and asset creation time, all 6 follower animals use a **shared body rig** system.

- **The Base Body:** A generic, squishy blob sprite with nub legs.
- **Color Tinting:** The base body is rendered in grayscale and tinted in-engine using the primary color hex codes.
- **Head Overlay:** Each animal has a unique, transparent PNG for their face, ears, and distinct features (e.g., bunny ears, frog eyes) that is parented to the top of the body rig.
- **Animation:** The body rig handles the walk cycle (wobble and bounce), while the head overlay follows along, inheriting the bounce.

---

## 5. 🏙️ Environment Art

The environment is a grid-based maze, but it shouldn't *feel* like a rigid chessboard. It should feel like a cozy, bustling miniature city.

### Tilemap Specifications

- **Base Tile Size:** `256x256` pixels.
- **Pixels Per Unit (PPU):** `100`. Therefore, one tile = `2.56` Unity units.
- **Perspective:** Top-down orthographic with a slight tilt (approx 15 degrees) to show the front faces of buildings and characters. (Often called 3/4 top-down).
- **Format:** PNG with transparency for overlay tiles.

### Environment Elements

1. **Road Tiles (The Grid):** Soft Asphalt. Must have clear, subtle grid lines (perhaps slightly lighter or darker asphalt) to help players judge distance and turns. Intersections should have faint crosswalk markings.
2. **Sidewalks:** Warm Cream. Elevated slightly (1px dark edge on the bottom) to separate from the road.
3. **Buildings (Obstacles):** Chunky blocks. Roofs are flat with slight overhangs. Front faces have simple, non-distracting windows (lit warmly). Colors: Soft Charcoals, muted bricks, dusty blues.
4. **Foliage (Obstacles):** perfectly round or teardrop-shaped bushes. Muted Sage green.
5. **Props:** Hydrants, mailboxes, streetlamps, trash cans. These add life but must never block the main path or confuse the grid layout.

### World Themes

- **Theme 1: City Day (Base Game):** Bright, sunny, warm shadows. The default aesthetic described above.
- **Future Themes (Planned):**
  - **City Night:** Deep blue ambient light, glowing streetlamps, neon signs reflecting on the roads.
  - **City Park:** Dirt paths, lots of foliage, ponds (obstacles).
  - **Beach Boardwalk:** Sand grids, wooden planks, pastel beach huts.

---

## 6. 🚐 Vehicle Design

The Rescue Vans are the goal states for the Swarm. They must be highly visible and immediately identifiable by color.

- **Design:** Chunky, rounded delivery-style vans. Think vintage VW buses mixed with modern Japanese kei-vans.
- **Color Coding:** The entire main body of the van is painted in the exact hex code of the corresponding animal (e.g., the Blue Puppy van is exactly `#5CB8FF`).
- **Details:** White roof, dark grey chunky tires. A bold white symbol on the side (e.g., a bone for the puppy, a fish for the kitten) to aid colorblind players.
- **The Station:** Vans park at a large "Rescue Station" building at the edge of the map. This building is neutral white/grey to let the colored vans stand out.

---

## 7. 🖱️ UI Art Style

The UI must feel like a natural extension of the game world—bouncy, friendly, and clean.

- **Shape Language:** Extremely rounded corners (pill shapes for buttons, soft rounded rects for panels). **No sharp corners anywhere.**
- **Depth:** UI elements should look like thick, physical buttons. Use a heavy drop shadow directly below the button (e.g., a button with `#FF6B6B` has a 4px shadow of `#C0392B`).
- **Icons:** Thick lines, flat colors. 
- **Typography:** Use a chunky, highly legible sans-serif font (e.g., *Nunito, Fredoka One, or similar rounded font*).
- **Animation:** Buttons must physically press down (translate Y) when clicked, accompanied by a satisfying "pop" sound. Panels should scale up with a slight overshoot (spring animation) when opening.

---

## 8. 📐 Sprite Specifications

Strict adherence to these technical specs is required to ensure crisp visuals and optimized memory usage.

- **Resolution / Scale:** Assets are authored at 2x resolution to support high-DPI (Retina) mobile screens.
- **Pixels Per Unit (PPU):** Globally set to **`100`** across the entire project. This ensures 1 pixel on a sprite maps consistently to world space.
- **Texture Format:**
  - Uncompressed PNGs in the repository.
  - Unity settings: ASTC 6x6 for Android, ASTC 4x4 for iOS.
- **Atlas Packing:** All sprites must be packed into Sprite Atlases via Unity's Sprite Atlas system. Use **Sprite Atlas V2** (the default in Unity 6) for all atlases.
  - Separate atlases by context: `Gameplay_Atlas`, `UI_Atlas`, `Environment_Atlas`.
  - Atlas packing settings: Max texture size 2048x2048 for mobile.
- **Filtering:** Point (no filter) if we lean towards pixel-perfect, or Bilinear if we want a smoother, vector-like look. **For Stray Swarm, we use Bilinear filtering** to support the smooth, soft aesthetic.

---

## 9. 🎬 Animation Style

Animation brings the "Playful" pillar to life. We do not use skeletal animation; we rely on **transform-based animation** (via DOTween) and **frame-by-frame sprite swapping**.

### Core Principles

1. **Squash and Stretch:** This is non-negotiable. When a character lands, they squash wide. When they jump or move fast, they stretch tall.
2. **Wobble/Bouncy Feel:** Movement shouldn't be linear. The Conga Line (tail) should exhibit a sine-wave bounce as they move, creating a rhythmic, mesmerizing flow.
3. **Anticipation & Overshoot:** UI panels don't just appear; they scale from 0 to 1.1, then settle to 1.0. Characters lean back before dashing.

### Timing Guidelines

- **Snappy Actions:** Swiping to turn should feel instantaneous. The animation should resolve in <0.15 seconds.
- **Idle Loops:** Should be slow and gentle. Breathing (scaling Y by 1.02 over 1 second), blinking, occasional ear twitches.

---

## 10. 💡 Lighting Guide (URP 2D)

While Stray Swarm is 2D, we utilize the Universal Render Pipeline (URP) 2D lighting system to add depth, mood, and polish.

- **Global Lighting:** The base 2D Global Light should be set to a warm, soft yellow-orange (e.g., `#FFF2D1`) at intensity `0.8`. This bathes the world in a sunny afternoon glow.
- **Light2D Types:** Utilize Global, Point, Spot, and Freeform lights appropriately.
- **Character Illumination:** Characters are mostly unlit (flat shaded) to retain their colors, but they receive a subtle rim light from the environment.
- **Point Lights:** Used sparingly for emphasis.
  - **Player Glow:** The Player Cat has a very subtle, soft white point light attached to them to subtly highlight their position on the grid.
  - **Goal Highlights:** The Rescue Vans have soft point lights matching their color, emitting a welcoming glow.
- **Shadows:** No harsh cast shadows. We use a simple, baked-in dark oval sprite underneath characters to ground them. However, 2D Shadow Casters can be used for dynamic shadows if desired.
- **Post-Processing:** Unity 6 URP 2D Renderer supports Volume-based post-processing. Use sparingly for effects like Bloom (for glowing elements) and Vignette (for depth).

---

## 11. ♿ Accessibility

Stray Swarm must be playable by everyone. Since color matching is a core mechanic, we must design for colorblindness (Protanopia, Deuteranopia, Tritanopia).

1. **Dual Coding (Color + Symbol):** Never rely on color alone.
   - Every colored animal must have a distinct silhouette/shape.
   - Every colored Rescue Van must feature a highly visible, contrasting white icon on its side representing the animal it accepts.
2. **Contrast Ratios:** Ensure UI text has a minimum contrast ratio of 4.5:1 against its background. (e.g., White text on Deep Indigo easily passes; White text on Sunbeam Yellow fails—use Deep Indigo text there).
3. **Motion Sensitivity:** Provide an option in the settings to disable UI screen shake and reduce the intensity of the "wobble" animations.

---

## 12. 🧊 Blender to Unity Pipeline

For solo developers using Blender to create 2D sprites from 3D models, follow this pipeline to ensure visual consistency:

- **Scene Setup:** Set up a dedicated Blender scene for rendering. Use an orthographic camera to remove perspective distortion and ensure sprites tile perfectly.
- **Lighting Setup:** Use consistent lighting in Blender to achieve a flat-shaded look. Disable shadows if baking them into the sprite is not desired, and rely on emission or flat materials.
- **Export Settings:** Render frames as PNG with transparency (RGBA). Follow strict naming conventions to easily identify angles and animations.
- **Batch Rendering:** Use Blender scripts or tools to batch render spritesheets across multiple camera angles and animation frames.
- **Unity Import:** 
  - Set Texture Type to **Sprite (2D and UI)**.
  - Set the appropriate Pixels Per Unit (PPU) (e.g., 100).
  - Set Filter Mode: Use **Point (no filter)** for crisp pixel art, or **Bilinear** for a smooth, vectorized look.
- **Pivot Configuration:** Carefully configure the Sprite Pivot point in Unity (often Bottom or Custom) to ensure accurate Y-sorting for 2D perspective.

---

## 13. 🛍️ Unity Asset Store Integration

When integrating third-party assets from the Unity Asset Store:

- **Style Evaluation:** Evaluate store assets critically. Ensure their base shape language, proportions, and detail density align with Stray Swarm's "Oversized and Squishy" style.
- **License Considerations:** Verify all asset licenses allow for commercial use. Keep a log of used assets and their licenses.
- **Modifying Assets:** Modify store assets to match our art style. This may involve:
  - Applying color tinting via materials to match our specific palette.
  - Adding an outline overlay (via shader or sprite modification) for visual consistency.
  - Scaling elements to fit the grid and character proportions.
- **Recommended Search Terms:** Use terms like "Toon", "Low Poly Soft", "Chunky", "Pastel", and "Casual" to find compatible models and sprites.

---

## 14. 📁 Asset Naming Conventions

Maintain a clean and searchable project structure.

### Sprite Naming

Format: `[Category]_[Name]_[Variant/State]_[Size]`

- **Characters:** `CHR_CatPlayer_Idle_256`, `CHR_Follower_Dog_Run_128`
- **Environment:** `ENV_Tile_RoadStraight_256`, `ENV_Prop_FireHydrant_128`
- **UI:** `UI_Btn_PrimaryPlay_Standard`, `UI_Icon_Star_Filled`
- **Vehicles:** `VEH_Van_BluePuppy_Side`

### Folder Organization

All art assets reside under `Assets/Art/`:

```text
Assets/
└── Art/
    ├── Characters/
    │   ├── Player/
    │   └── Followers/
    │       ├── BaseRig/
    │       └── Heads/
    ├── Environment/
    │   ├── Tiles/
    │   └── Props/
    ├── UI/
    │   ├── Buttons/
    │   ├── Icons/
    │   └── Panels/
    ├── Vehicles/
    └── Atlases/
```

> [!IMPORTANT]
> Consistency is key. Before committing any new asset, ensure it follows the naming convention, has the correct PPU (100), and is assigned to the appropriate Sprite Atlas.

---
*Document Version: 1.0.0*
*Last Updated: August 12, 2026*
