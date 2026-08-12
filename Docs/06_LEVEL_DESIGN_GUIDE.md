# 🎮 Stray Swarm: Level Design Guide

Welcome to the definitive Level Design Guide for **Stray Swarm**. This document is intended for level designers, producers, and developers responsible for crafting the player experience, balancing difficulty, and maintaining the pacing of the game.

Stray Swarm is built on momentum, pattern recognition, and flow. A great level feels like a smooth dance, while a poorly designed level feels frustrating and clunky. 

---

## 1. 🧠 Level Design Philosophy

At the core of Stray Swarm's level design are two guiding principles:

### Every Level Teaches One Thing
Never overwhelm the player by introducing multiple new mechanics or concepts simultaneously. If a level introduces a new animal color, the grid layout should be relatively simple. If a level introduces complex branching paths, stick to familiar colors and van configurations. 
* **Focus:** What is the primary mechanic or concept this level explores? 
* **Execution:** Strip away distractions that dilute this focus.

### Every Level Has One 'Aha!' Moment
Puzzle-flow games rely on a moment of realization where the player sees the optimal path. 
* Early levels: The 'aha' is understanding how mechanics work (e.g., "Oh, if I collect them in this order, they perfectly match the vans!").
* Later levels: The 'aha' is discovering a hidden routing efficiency (e.g., "If I loop around the station *before* picking up the yellow pigeons, I can deposit the blue puppies first and clear my tail!").

> [!TIP]
> **The Flow State:** Stray Swarm is about achieving a state of flow. Dead stops, confusing layouts, or overly punishing timers break this flow. Design for a rhythm of swiping and collecting.

---

## 2. 📏 Grid Design Principles

The grid is the canvas. How you structure the maze dictates the player's movement options and pacing.

### Grid Sizes
* **Small (5x5):** Used exclusively for tutorials and early-game levels (1-5). Keeps the camera zoomed in and limits choices, focusing the player on core mechanics.
* **Medium (7x7):** The standard grid size for mid-game levels. Provides enough space for interesting loops and moderate tail lengths without overwhelming the screen.
* **Large (9x9):** Reserved for advanced levels. Requires significant planning, camera management (if scrolling/zooming is implemented), and allows for massive, satisfying conga lines.

### Path Complexity
* **Linear:** Single path, no choices. Used for teaching basic controls.
* **Branching:** Simple forks in the road. Tests quick decision-making (e.g., "Left for blue, right for pink").
* **Looping:** Circular paths that allow the player to circle back. Essential for later levels where players might need to wait for a specific van or collect animals in a specific order.
* **Maze-like:** Complex webs of intersections. Used in expert levels to challenge routing and spatial awareness.

### Dead Ends
> [!CAUTION]
> **Use dead ends sparingly!** In a flow-based game, hitting a dead end and having to turn around breaks momentum.

* When used, they should function as **optional risk/reward traps** (e.g., a cluster of bonus points or a rare color, but it costs time to retrieve and turn around).
* Always try to provide an alternate route or a small loop at the end of a long path to maintain forward momentum.

### The 'Golden Path'
Every level **must** have an optimal route that allows a skilled player to collect all animals, deliver them perfectly, and achieve a 3-star rating with time to spare. 
* The Golden Path is the intended solution.
* Sub-optimal paths should still allow level completion (1 or 2 stars), but the Golden Path is what hardcore players will hunt for.

---

## 3. 📈 Difficulty Curve (DETAILED)

Difficulty in Stray Swarm is defined by grid complexity, animal color variety, van queue requirements, and time pressure. We follow a structured curve to ensure a smooth onboarding and a challenging endgame.

### Phased Progression

* **Levels 1-5: Tutorial Phase**
  * **Colors:** 1-2 (Blue Puppy, Pink Kitten)
  * **Grid:** 5x5, mostly linear or simple branching.
  * **Timer:** Very generous (essentially non-existent pressure).
  * **Focus:** Teaches basic swipe controls, the collection mechanic, and delivering to the station.

* **Levels 6-10: Easy Phase**
  * **Colors:** 2-3 (Introduces Yellow Pigeon)
  * **Grid:** 7x7, introducing loops.
  * **Timer:** Generous.
  * **Focus:** Introduces multiple rescue vans. Teaches the concept of matching tail order to the van queue. Introduces the "gap-closing dash" mechanic naturally when a middle animal is deposited.

* **Levels 11-20: Medium Phase**
  * **Colors:** 3-4 (Introduces Green Frog)
  * **Grid:** 7x7 to 9x9.
  * **Timer:** Moderate pressure.
  * **Focus:** Animals are placed to require backtracking or looping. The natural path doesn't always match the van queue. Players must start planning their routes.

* **Levels 21-30: Hard Phase**
  * **Colors:** 4-5 (Introduces Orange Hamster)
  * **Grid:** 9x9, complex mazes.
  * **Timer:** Tight.
  * **Focus:** High density of vans. Long tail management is required. Crossing paths (no-penalty tail crossing) becomes essential to navigate crowded grids.

* **Levels 31+: Expert Phase**
  * **Colors:** 5-6 (Introduces Purple Bunny)
  * **Grid:** 9x9, maximum complexity.
  * **Timer:** Very tight.
  * **Focus:** Tests all skills. Requires near-perfect execution of the Golden Path for 3 stars. Future hazard mechanics will be introduced here.

### Level Parameter Matrix

| Level Range | Grid Size | Max Colors | Van Count | Timer Pressure | Star Thresholds (General Rule) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| 1-3   | 5x5 | 1 | 1 | None | 1⭐ Completion, 3⭐ Fast |
| 4-5   | 5x5 | 2 | 2 | Low | 1⭐ Completion, 3⭐ Fast |
| 6-10  | 7x7 | 3 | 2-3 | Low | 2⭐ Moderate pace, 3⭐ Golden Path |
| 11-15 | 7x7 | 3-4 | 3-4 | Medium | 1⭐ Slow, 2⭐ Good route, 3⭐ Flawless |
| 16-20 | 9x9 | 4 | 4-5 | Medium | Requires combo maintenance for 3⭐ |
| 21-25 | 9x9 | 4-5 | 5-6 | High | Mistakes cost stars immediately |
| 26-30 | 9x9 | 5 | 6-8 | Very High | Brutal optimization needed for 3⭐ |
| 31+   | 9x9 | 6 | 8+ | Extreme | Golden Path is mandatory for 3⭐ |

---

## 4. 🐱 Animal Placement Strategy

Where you place the stray animals dictates the player's pathing and decision-making.

### Clustering for Flow
Cluster same-color animals together when you want the player to build up a large segment of their tail quickly. This feels satisfying and is often used on the Golden Path leading up to a specific van.

### Distractor Placements
Place 'distractor' colors just off the main path to tempt the player into sub-optimal routes. 
* Example: The optimal path needs Blue, but there's a shiny Purple Bunny down a side street. Grabbing it might ruin the tail order for the upcoming vans, forcing a time-consuming loop to fix it.

### Intersection Tension
Place animals immediately *after* an intersection to force quick decisions. If the player turns too late, they miss the animal. This leverages the input buffering system and rewards quick reflexes.

---

## 5. 🚐 Van Queue Design

The van queue is the primary puzzle element of Stray Swarm.

### Natural Ordering (Early Game)
In early levels, set up the van queue to match the order in which the player will naturally encounter the animal clusters on the most obvious path. 
* Path: Encounter Blue -> Encounter Pink
* Queue: Blue Van -> Pink Van

### Planned Ordering (Mid-Late Game)
Force the player to think ahead. 
* Path: The easiest route grabs Pink first, then Blue.
* Queue: Requires Blue first, then Pink.
* Solution: The player must find a slightly more complex route to grab Blue first, or grab Pink, then Blue, then loop around the station to deposit Blue while holding Pink in the tail.

### 'Mismatch Tension'
Create situations where the player's tail is dominated by one color, but the active van demands another. This forces the player to manage a very long tail (increasing collision risks visually, even if the tail doesn't collide with itself) while hunting for the required color to unblock the queue.

---

## 6. 🚉 Station Placement

The central hub of the level.

### Central Placement
The standard approach. A centrally located station is easy to reach from all quadrants of the map and serves as a natural anchor for loops.

### Edge / Corner Placement
Placing the station in a corner drastically changes the level dynamics. It often forces long, one-way trips across the map, requiring the player to collect *everything* before making the long trek back. This is great for late-game tension.

### Multiple Approaches
Always ensure there are at least two distinct paths leading *into* the station. This prevents bottlenecks and allows for varied routing strategies.

---

## 7. 🎢 Pacing

A full campaign of levels needs a pulse. Constant, escalating difficulty leads to player burnout.

### The Rollercoaster Curve
* **Spike:** A very difficult level that tests mastery (e.g., Level 20).
* **Rest:** Followed by a slightly easier, 'breather' level (e.g., Level 21) that might introduce a fun new layout but has a forgiving timer.
* **Build:** Slowly ramp up difficulty again over the next few levels.

---

## 8. ✅ Playtesting Checklist

Before a level is marked as "Shippable", verify the following:

1. [ ] **Golden Path Verification:** Can the level be beaten with 3 stars by a skilled player?
2. [ ] **Minimum Viability:** Can the level be beaten with 1 star by a struggling player using a sub-optimal route?
3. [ ] **Clarity:** Is it immediately obvious what the first 1-2 required moves are upon starting the level?
4. [ ] **Dead Ends:** If there are dead ends, is there enough time to recover from entering them?
5. [ ] **Input Flow:** Are there any awkward sequences of rapid turns that feel physically uncomfortable to swipe on a mobile screen?
6. [ ] **Van Queue Logic:** Is the van queue solvable without requiring completely unintuitive or pixel-perfect timing?
7. [ ] **Tail Length:** At maximum tail length for this level, does the tail obscure critical information (like upcoming turns or the station)?

---

## 9. 💾 Level Data Schema

When configuring a level in the Unity Editor using the `LevelData` ScriptableObject, refer to these fields:

```csharp
[CreateAssetMenu(fileName = "Level_XX", menuName = "StraySwarm/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Basic Info")]
    public string levelName;
    public int levelNumber;
    
    [Header("Grid Setup")]
    public int gridWidth;
    public int gridHeight;
    public TextAsset layoutFile; // CSV or custom format defining walls/paths
    
    [Header("Spawns")]
    public Vector2Int playerSpawnPos;
    public Vector2Int stationPos;
    public List<AnimalSpawnData> animalSpawns;
    
    [Header("Objectives")]
    public List<ColorType> vanQueue; // Order of required vans
    
    [Header("Scoring")]
    public float timeLimitSeconds;
    public int oneStarScore;
    public int twoStarScore;
    public int threeStarScore;
}
```

---

## 10. 🗺️ Example Level Designs

Here are 5 concrete examples of level designs illustrating the principles above.

### Legend for ASCII Art
* `.` = Path (Walkable)
* `#` = Wall / Building (Unwalkable)
* `P` = Player Start
* `S` = Station
* `B`, `K`, `Y`, `F` = Animals (Blue Puppy, Pink Kitten, Yellow Pigeon, Green Frog)

---

### Example 1: "First Steps" (Tutorial - Level 1)
**Grid:** 5x5
**Colors:** 1 (Blue)
**Timer:** 60s (Generous)
**Vans:** [Blue]

```text
# # # # #
# . B P #
# . # # #
# . B S #
# # # # #
```

**Design Rationale:**
The simplest possible setup. The player spawns, swipes left to move, automatically collects a Blue Puppy, swipes down, collects another, and swipes right into the station. It teaches the core loop instantly without any chance of failure.

---

### Example 2: "The Fork" (Early Game - Level 4)
**Grid:** 5x5
**Colors:** 2 (Blue, Pink)
**Timer:** 45s
**Vans:** [Blue, Pink]

```text
# # . # #
# B . K #
# # . # #
# . P . #
# . S . #
```

**Design Rationale:**
Introduces a choice. The van queue requires Blue first. The player must recognize this, swipe UP, turn LEFT for the Blue Puppy, then loop back or turn around for the Pink Kitten before heading to the Station. It teaches that sequence matters.

---

### Example 3: "The Loop-de-Loop" (Mid Game - Level 12)
**Grid:** 7x7
**Colors:** 3 (Blue, Pink, Yellow)
**Timer:** 50s
**Vans:** [Pink, Blue, Yellow]

```text
# # . . . # #
# K . # Y . #
# # . # # . #
. P . S . . .
# # . # # . #
# B . . . . #
# # # # # # #
```

**Design Rationale:**
The station is central, surrounded by a loop. The vans require Pink, then Blue, then Yellow. The optimal path is Up -> Left (Pink) -> Down (Loop around) -> Down (Blue) -> Right -> Up -> Right (Yellow) -> Left to Station. This requires navigating a full circuit and planning the collection order carefully.

---

### Example 4: "The Distractor" (Hard - Level 22)
**Grid:** 9x9
**Colors:** 4 (Blue, Pink, Yellow, Frog)
**Timer:** 40s (Tight)
**Vans:** [Yellow, Yellow, Blue]

```text
# . . . . . . . #
. Y # # # # # Y .
. # . . P . . # .
. # . # S # . # .
. B . # . # . B .
. # . . . . . # .
. F # # # # # K .
# . . . . . . . #
```

**Design Rationale:**
A symmetrical, maze-like layout. The vans strictly require Yellows then Blue. There are Frogs and Kittens in the bottom corners serving purely as distractors or point bonuses. If a player goes for the Frog, they waste precious time and mess up their tail order, making it harder to deposit the required Yellows efficiently.

---

### Example 5: "The Long Haul" (Expert - Level 35)
**Grid:** 9x9
**Colors:** 5
**Timer:** 60s (Very tight for the required distance)
**Vans:** [Blue, Pink, Yellow, Frog, Orange]

```text
S . . # . K . . #
# # . # . # # . #
# B . . . . # . #
# # # # # . # . #
P . . . . . . . O
# # # # # . # # #
# Y . . . . . F #
# # . # # # # # #
# . . . . . . . #
```

**Design Rationale:**
The station is jammed in the top-left corner. The player spawns mid-left and must traverse the entire grid to collect all 5 colors in the correct order, ending up near the bottom right or top right before making a massive, long-haul return trip to the top-left station with a 5-animal conga line trailing behind them. Crossing their own tail will happen naturally here, emphasizing the 'no-penalty tail crossing' mechanic visually.
