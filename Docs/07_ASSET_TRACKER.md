# 🎨 Stray Swarm: Master Asset Tracker & Production Manifest

> **Tracks all art, audio, and visual assets. Updated for Kawaii Cube Aesthetic direction.**

---

## 🐱 1. Character Sprites (AI-Generated Cube PNGs)

> **Format:** Single static PNG per character. 512×512 resolution. Transparent background. Rounded-square shape with cute face details.
> **NO sprite sheets, NO directional variants.** All animation is code-driven (CubeWobble.cs).

| Asset Name | Shape | Face Details | Color | Status |
| :--- | :--- | :--- | :--- | :--- |
| **Player Cat** | Rounded square, ~80% of 1 tile | Dot eyes, whiskers, small ears on top, tiny nose | Warm Orange/Tabby | 🟡 Reference exists |
| **Blue Puppy** | Rounded square, ~40% of 1 tile | Floppy ears on sides, tongue out, happy eyes | Sky Blue `#5CB8FF` | 🔴 Needed |
| **Pink Kitten** | Rounded square, ~40% of 1 tile | Pointed ears, blush marks, closed smile | Bubblegum `#FF7EB3` | 🔴 Needed |
| **Yellow Pigeon** | Rounded square, ~40% of 1 tile | Small beak, tiny wing bumps on sides | Sunbeam `#FFCC02` | 🔴 Needed |
| **Green Frog** | Rounded square, ~40% of 1 tile | Big round eyes on top of cube, wide mouth | Minty `#7ED89E` | 🔴 Needed |
| **Orange Hamster** | Rounded square, ~40% of 1 tile | Round ears, puffy cheeks, tiny paws | Tangerine `#FF9F43` | 🔴 Needed |
| **Purple Bunny** | Rounded square, ~40% of 1 tile | Tall ears on top, buck teeth, round eyes | Lavender `#A29BFE` | 🔴 Needed |

---

## 🧺 2. Basket & Crate Sprites

| Asset Name | Description | Status |
| :--- | :--- | :--- |
| **Tail Basket** | Small rounded wooden basket/crate (~60% of 1 tile). Animal head peeks out from top rim. Colored band around the middle matching animal color. | 🔴 Needed |
| **Delivery Crate Station** | Larger wooden crate (~1.5 tiles wide) with colored banner/flag on top. Open front shows stacking animals inside. | 🔴 Needed |
| **Delivery Crate (Full)** | Same crate but with lid closed. Used when station is fully filled. | 🔴 Needed |

---

## 🏞️ 3. Environment & Path Tiles (Unity Rule Tile Sets)

> Each theme requires a **47-tile Rule Tile set** with smooth **rounded inner corners**.

| Theme | Path Color | Background Color | Edge Props (all rounded-cube style) | Status |
| :--- | :--- | :--- | :--- | :--- |
| **Desert World** | Sandy Beige `#F5DEB3` | Warm Sand `#EDCB96` | Cube cacti, tiny rock cubes, sand puff particles | 🔴 Needed |
| **Forest World** | Light Tan `#E8DCC8` | Vibrant Green `#5CB85C` | Cube bushes, flower cubes, mushroom cubes | 🔴 Needed |
| **Winter World** | Pale Blue `#E8EFF5` | Icy Blue `#B3D4E8` | Snowdrift cubes, icicle rectangles, pine triangles | 🔴 Needed |
| **City World** | Gray Stone `#B0B0B0` | Dark Asphalt `#4A4A4A` | Cube lamp posts, bench cubes, hydrant cubes | 🔴 Needed |

---

## 🧱 4. Obstacle Sprites (Cube Aesthetic)

| Object | Visual Style | Status |
| :--- | :--- | :--- |
| **One-Way Arrow** | Rounded yellow square tile with white 3D arrow embossed on top | 🔴 Needed |
| **Rock Barrier** | Cluster of 2–3 rounded gray/brown cubes stacked | 🔴 Needed |
| **Numbered Wall** | Single rounded stone cube with large bold white number (1, 2, 3) | 🔴 Needed |
| **Bridge / Overpass** | Elevated wooden plank (2 cubes wide) with shadow gap underneath | 🔴 Needed |

---

## 🎭 5. Shadow Sprites

| Asset Name | Description | Status |
| :--- | :--- | :--- |
| **Character Shadow** | Soft dark oval, 70% of character width, 20% opacity black | 🔴 Needed |
| **Basket Shadow** | Slightly smaller oval for tail baskets | 🔴 Needed |
| **Station Shadow** | Larger oval for delivery crate stations | 🔴 Needed |

---

## 🖼️ 6. UI & Graphical Assets

| Asset Name | Format | Description | Status |
| :--- | :--- | :--- | :--- |
| **Glossy Golden Star (Earned)** | PNG | Rounded star with golden fill and subtle bevel | 🔴 Needed |
| **Empty Star (Unearned)** | PNG | Dark gray translucent star | 🔴 Needed |
| **Glass Card Background** | PNG | Rounded rect with soft drop shadow | 🔴 Needed |
| **Primary Action Buttons** | PNG | Chunky rounded rect with 3D press shadow | 🔴 Needed |
| **UI Icons** | PNG | Pause, Settings Gear, Retry, Next Arrow, Mute/Unmute | 🔴 Needed |
| **Colorblind Shape Icons** | PNG | Unique shape overlays per animal color | 🔴 Needed |
| **Cat Skin Thumbnails** | PNG | Preview icons for skin shop (50+ planned) | 🔴 Needed |

---

## 🔊 7. Audio Library

| Sound Name | Type | Description | Status |
| :--- | :--- | :--- | :--- |
| `collect_pop.wav` | SFX | Bright, bubbly pop on animal rescue | 🔴 Needed |
| `basket_spawn.wav` | SFX | Wooden crate thud when basket appears | 🔴 Needed |
| `deliver_whoosh.wav` | SFX | Zip/swoosh as animal flies to crate | 🔴 Needed |
| `crate_full.wav` | SFX | Happy chime/bell when a crate fills up | 🔴 Needed |
| `wall_break.wav` | SFX | Crumble sound for numbered wall breaking | 🔴 Needed |
| `star_1/2/3.wav` | SFX | Ascending chimes for star pop-ups | 🔴 Needed |
| `win_fanfare.wav` | SFX | 2-second victory jingle | 🔴 Needed |
| `lose_sad.wav` | SFX | Descending trombone/sad horn | 🔴 Needed |
| `button_click.wav` | SFX | Soft UI tap | 🔴 Needed |
| `timer_warning.wav` | SFX | Heartbeat ticking for last 10s | 🔴 Needed |
| `combo_x3/x5/x10.wav` | SFX | Escalating combo hit sounds | 🔴 Needed |
| `bgm_menu.ogg` | BGM | Chill, looping menu track (~30s) | 🔴 Needed |
| `bgm_gameplay.ogg` | BGM | Upbeat, looping puzzle track (~60s) | 🔴 Needed |

---

## ✨ 8. Particle Systems (Square-Shaped Particles)

> All particles use **rounded-square shapes** to match the Cube Aesthetic.

| Particle Name | Trigger | Visual | Status |
| :--- | :--- | :--- | :--- |
| **Collect Sparkles** | Animal picked up | Burst of tiny colored squares + heart shape | 🔴 Needed |
| **Basket Pop** | Basket appears in tail | Ring of white squares expanding + scale punch | 🔴 Needed |
| **Delivery Zip Trail** | Animal flies to crate | Trail of colored squares along flight arc | 🔴 Needed |
| **Crate Fill Burst** | Animal enters crate | Color-matched confetti from crate opening | 🔴 Needed |
| **Crate Complete** | Crate fully filled | Golden star squares shower + lid close | 🔴 Needed |
| **Wall Crumble** | Numbered wall breaks | Gray stone squares scatter with gravity | 🔴 Needed |
| **Win Confetti** | Level complete | Multi-colored square confetti rain | ✅ Done (needs square update) |
| **Combo Text Trail** | x3/x5/x10 combo | Floating text with small trailing squares | 🔴 Needed |

---

## 🐱 9. Cat Skins (Shop Collectibles)

> Each skin is a single PNG recolor/redesign of the Player Cat cube. Easy to create = massive collection potential!

| Skin Name | Description | Unlock Method | Status |
| :--- | :--- | :--- | :--- |
| **Default Tabby** | Orange tabby cube cat | Free (starting skin) | 🟡 Reference exists |
| **Tuxedo** | Black & white formal cube cat | 100 coins | 🔴 Needed |
| **Calico** | Tri-color patched cube cat | 100 coins | 🔴 Needed |
| **Snow White** | Pure white cube cat, pink nose | 150 coins | 🔴 Needed |
| **Knight Cat** | Armored cube cat with visor | 300 coins | 🔴 Needed |
| **Astronaut Cat** | Space helmet cube cat | 500 coins | 🔴 Needed |
| **Golden Cat** | Shiny gold metallic cube cat | 1000 coins | 🔴 Needed |
| *...30+ more planned* | | | |
