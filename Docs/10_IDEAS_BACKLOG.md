# 💡 Stray Swarm - Ideas Backlog 💡

## 1. Backlog Overview

Welcome to the **Ideas Backlog** for *Stray Swarm*. This document serves as the central repository for all feature ideas, enhancements, polish items, and wild concepts that fall outside the scope of our initial v1.0 release. 

As a living document, this backlog will grow and evolve as we gather player feedback, conduct playtests, and brainstorm internally. It is designed to be the single source of truth for the project's future roadmap.

### The Purpose of the Backlog
1. **Prevent Scope Creep:** By having a place to put great ideas, we prevent them from derailing the current milestone.
2. **Facilitate Planning:** When a milestone ends, this document provides a pre-vetted list of features to pull into the next sprint.
3. **Record History:** It tracks why certain decisions were made, preventing circular discussions (especially via the Rejected Ideas section).
4. **Foster Creativity:** It gives everyone on the team a voice and a designated place to pitch their wildest concepts.

### How to Use This Document

- **Idea Generation:** Anyone on the team (designers, programmers, artists, QA) can contribute ideas to the **Brainstorm Parking Lot** at any time. No idea is too small or too crazy for the parking lot.
- **Triage & Scoring:** During our bi-weekly sprint planning meetings or major milestone reviews, the core team will pull ideas from the parking lot, discuss them, score them using the Impact/Effort system, and move them into the appropriate term category (Near, Mid, or Long-Term).
- **Prioritization:** When planning a new update, we sort the relevant tables by Priority Score (highest first) to identify the most valuable features to implement. We balance high-score quick wins with necessary foundational work for larger features.
- **Rejection:** If an idea is deemed fundamentally incompatible with the core vision of *Stray Swarm*, it moves to the **Rejected Ideas** section. We must always include a clear, objective rationale for the rejection to maintain transparency.

---

## 2. Impact / Effort Scoring System

To objectively evaluate and prioritize ideas, we use a standardized Impact/Effort scoring model. This helps us avoid prioritizing features based solely on who shouted the loudest or which idea is the most "shiny" at the moment.

### 🌟 Impact (1-5)
Measures the value the feature brings to the player experience, the game's metrics (retention, monetization), or the marketing potential.
* **1 (Minimal):** Hardly noticeable by the average player. Niche appeal. Negligible effect on KPIs.
* **2 (Low):** Minor quality of life (QoL) improvement. Appreciated by hardcore players, but won't move the needle much.
* **3 (Medium):** Solid addition. Noticeable improvement to the core loop, general polish, or UI/UX.
* **4 (High):** Major feature. Significant boost to player engagement, retention, or core fun. High marketing value.
* **5 (Massive):** Game-changing feature. Huge marketing potential. Could define a major version number change (e.g., v2.0).

### 🛠️ Effort (1-5)
Measures the cost in time, resources, technical complexity, and risk to implement the feature.
* **1 (Trivial):** A few hours of work. Simple configuration tweak, minor UI text update, simple bug fix.
* **2 (Low):** 1-2 days of work. Straightforward implementation, minimal risk, utilizes existing systems.
* **3 (Medium):** About a week of work. Requires new assets, moderate code changes, UI updates, and standard QA testing.
* **4 (High):** Several weeks of work. Complex new systems, significant art/audio asset creation, cross-discipline collaboration, extensive QA needed.
* **5 (Massive):** Months of work. Core architecture changes, new backend server infrastructure, very high risk of breaking existing systems.

### 🎯 Priority Score
Calculated as: **`Priority Score = Impact / Effort`**

A higher score indicates a better return on investment (ROI) — high impact for low effort. Features with the highest scores should generally be scheduled first.

| Impact | Effort | Priority Score | Assessment | Action Plan |
| :---: | :---: | :---: | :--- | :--- |
| 5 | 1 | 5.0 | **No Brainer** | Do it immediately. Squeeze it into the current sprint if possible. |
| 4 | 2 | 2.0 | **Quick Win** | High priority. Schedule for the very next update. |
| 3 | 3 | 1.0 | **Solid Addition**| Schedule normally based on roadmap needs. |
| 5 | 5 | 1.0 | **Major Project** | Needs careful strategic planning and dedicated resources. |
| 1 | 4 | 0.25 | **Time Sink** | Avoid entirely or rethink the design to reduce effort. |

---

## 3. Near-Term Ideas (v1.1 - v1.2)

These are features planned for the immediate updates following the v1.0 launch. They focus on quick wins, essential polish, expanding the core content to keep early players engaged, and addressing immediate post-launch feedback.

| Idea | Description | Impact | Effort | Score | Status |
| :--- | :--- | :---: | :---: | :---: | :--- |
| **Power-ups: Speed Boost** | Temporary movement speed increase for the swarm to quickly navigate long straightaways and beat the timer. | 3 | 2 | 1.5 | Planned (v1.1) |
| **Power-ups: Magnet** | Automatically pulls nearby stray animals into the tail without needing to cross exactly over their grid tile. | 4 | 3 | 1.33 | Planned (v1.1) |
| **Power-ups: Time Freeze** | Temporarily pauses the overall level timer and all active van patience timers. | 4 | 2 | 2.0 | Planned (v1.1) |
| **Wildcard Rainbow Animal** | A rare, special stray that continuously cycles colors and can act as any color when delivered to a van. | 5 | 3 | 1.66 | Evaluating |
| **Dynamic Van Patience Timer** | Vans have a visual countdown timer ring; delivering animals quickly yields bonus points, taking too long causes them to leave. | 4 | 4 | 1.0 | Concept |
| **Daily Challenge Level** | A unique, procedurally generated or hand-crafted level that changes every 24 hours with its own specific leaderboard. | 4 | 3 | 1.33 | Evaluating |
| **Haptic Feedback Polish** | Integrate nuanced device vibrations (using iOS/Android native APIs) for turning, collecting animals, and successful deliveries. | 3 | 1 | 3.0 | Planned (v1.1) |
| **Tutorial Improvements** | Add interactive tooltips, ghost-cat demonstrations, and clearer UI highlights for the first 3 onboarding levels. | 5 | 2 | 2.5 | High Priority |
| **More Levels (Up to 50)** | Expand the base game campaign from the initial 30 levels to 50 levels with increasing complexity and new grid shapes. | 4 | 4 | 1.0 | In Progress |
| **Combo System UI Polish** | Make the combo counter more explosive and visually rewarding (particle bursts, slight screen shake, dynamic typography). | 3 | 2 | 1.5 | Planned (v1.1) |
| **Colorblind Mode** | Add distinct symbols or patterns to the animals and vans, not relying solely on color. | 4 | 2 | 2.0 | High Priority |
| **Battery Saver Mode** | Toggle to cap framerate at 30fps and disable post-processing bloom. | 3 | 1 | 3.0 | Planned (v1.1) |

> [!TIP]
> Prioritize **Haptic Feedback Polish**, **Tutorial Improvements**, and **Colorblind Mode** for the first minor patch. These offer massive improvements to game feel, accessibility, and onboarding for very little effort.

---

## 4. Mid-Term Ideas (v2.0)

Features targeted for a major update. These require significant development time and aim to introduce new systems that increase long-term retention, add meta-progression, and introduce monetization potential.

| Idea | Description | Impact | Effort | Score | Status |
| :--- | :--- | :---: | :---: | :---: | :--- |
| **World Themes: Beach** | New visual tileset (sand, boardwalks, water hazards) and new ambient audio. | 4 | 4 | 1.0 | Concept |
| **World Themes: Forest** | New visual tileset (grass, trees, dirt paths) and new ambient audio. | 4 | 4 | 1.0 | Concept |
| **World Themes: Night City** | Cyberpunk/neon aesthetic for the existing city grid. Heavy use of bloom. | 3 | 4 | 0.75 | Concept |
| **Player Skins / Customization** | Allow players to unlock or purchase different looks for the lead cat (e.g., tabby, tuxedo, robotic, lion). | 5 | 3 | 1.66 | High Priority |
| **Tail Trail Effects** | Unlockable particle trails that follow the conga line (e.g., stars, rainbows, musical notes). | 3 | 2 | 1.5 | Planned |
| **Achievements / Badges** | In-game achievement system with unlockable profile badges to show off to friends. | 3 | 3 | 1.0 | Evaluating |
| **Global Leaderboards** | Integrate Google Play Games and Apple Game Center for level high scores and total stars. | 4 | 3 | 1.33 | Planned |
| **Sound Pack Options** | Allow players to choose different audio styles (e.g., 8-bit retro, lo-fi beats, orchestral). | 2 | 4 | 0.5 | Low Priority |
| **Seasonal Events: Halloween** | Spooky level decorations, pumpkin collecting mechanics, limited-time ghost cat skin. | 4 | 4 | 1.0 | Concept |
| **Seasonal Events: Christmas** | Snowy levels, gift collecting, reindeer animal type, Santa hat for the lead cat. | 4 | 4 | 1.0 | Concept |
| **Cloud Save Sync** | Allow players to sync progress across multiple devices using their platform accounts. | 5 | 3 | 1.66 | Planned |
| **Hard Mode Variations** | Replay existing levels with tighter timers and more complex van requirements. | 3 | 2 | 1.5 | Evaluating |

---

## 5. Long-Term Ideas (v3.0+)

Massive, transformative features that could define the future of *Stray Swarm*. These ideas require dedicated R&D, significant architectural planning, and potentially expanding the team size.

| Idea | Description | Impact | Effort | Score | Status |
| :--- | :--- | :---: | :---: | :---: | :--- |
| **Level Editor + Sharing** | In-game tool for players to create, test, and upload their own grid mazes for others to play. Server backend needed. | 5 | 5 | 1.0 | Vision |
| **Multiplayer Race Mode** | Real-time competitive mode where 2-4 players navigate the same grid to collect animals first. Requires robust netcode. | 5 | 5 | 1.0 | Vision |
| **Narrative / Story Mode** | Add comic-book style cutscenes and a continuous storyline about rescuing the city from an evil dogcatcher corporation. | 3 | 5 | 0.6 | Vision |
| **Monetization: Rewarded Ads** | Watch an ad to revive upon failure, double end-of-level rewards, or get a free power-up at the start. | 5 | 2 | 2.5 | To Be Discussed |
| **Monetization: IAP Cosmetics** | Premium shop for exclusive skins, trail effects, and custom van designs. | 5 | 3 | 1.66 | To Be Discussed |
| **Monetization: Battle Pass** | A seasonal progression track (free and premium) to unlock cosmetics and currency over 30 days. | 4 | 4 | 1.0 | To Be Discussed |
| **Social Features (Friend Challenges)** | Send a specific level attempt (ghost data) to a friend to see if they can beat your time/score. | 4 | 4 | 1.0 | Vision |
| **Procedural Level Generation** | An endless "infinite runner" mode where the grid generates endlessly ahead of the player. | 4 | 5 | 0.8 | Vision |
| **AR Mode** | Play the game projected onto a real-world table using Augmented Reality (ARCore/ARKit). | 2 | 5 | 0.4 | Vision |
| **Franchise Spinoff** | "Stray Swarm: Highway Rescue" - a different genre utilizing the same IP and characters. | ? | ? | ? | Dream |

---

## 6. Rejected Ideas

Ideas that have been officially reviewed by the core team and discarded. Documenting these prevents us from rehashing old discussions and keeps the team aligned on the core vision.

* **Lives / Energy System (Candy Crush style):** 
    * *Reason for Rejection:* Too punishing for our target demographic. We want a frictionless, hypercasual experience where players can retry immediately without waiting or paying. It breaks the "flow" state we are aiming for and causes frustration.
* **Pay-to-Win Mechanics (Buy Score Multipliers with real money):**
    * *Reason for Rejection:* Fundamentally against our studio values. It ruins the integrity of the leaderboards and alienates the player base. All monetization should be strictly cosmetic or time-saving (e.g., skip ad), never performance-enhancing.
* **Complex Animal Behaviors (Animals running away from the player):**
    * *Reason for Rejection:* The core challenge should be the puzzle of the grid and the execution of the swipes. Adding unpredictable AI to the collectibles makes the game feel unfair and frustrating rather than challenging. The animals should be static puzzle pieces.
* **Intrusive Interstitial Ads (Forced ads between levels):**
    * *Reason for Rejection:* Interrupting gameplay with unskippable ads between every level causes massive player churn. We will rely on opt-in rewarded ads instead to maintain player goodwill.
* **Virtual Joystick Controls:**
    * *Reason for Rejection:* Swiping is the optimal input method for grid-based, 90-degree turning. A virtual joystick introduces ambiguity and imprecise inputs, leading to accidental wrong turns and player frustration.
* **Blood / Gore upon failure:**
    * *Reason for Rejection:* Clashes entirely with the wholesome, family-friendly, pastel aesthetic. Failure states should be comical (e.g., cat gets dizzy) rather than violent.

---

## 7. Brainstorm Parking Lot

Raw, unprocessed ideas. Dump thoughts here during meetings, playtests, or late-night inspiration strikes. We will categorize, refine, and score them during planning sessions.

*   Dogs as roaming enemies that break the tail if they touch it?
*   Traffic lights at intersections that force you to stop and wait?
*   A "pesticide" or "mud" puddle hazard that removes animals from the tail?
*   Cats can jump over small gaps in the road with a double-tap?
*   Underground subway levels with trains as hazards?
*   Helicopter rescue extraction point instead of vans for the final level?
*   VIP animals that are worth 10x points but walk super slow, dragging the whole line down?
*   Collect coins to build and decorate a cat shelter hub-world (merge-game mechanics)?
*   Boss fights? (How would that even work in a puzzle-flow game?? Maybe a giant dog chasing you?)
*   Collaborative multiplayer (two players controlling different ends of the same conga line)?
*   Integration with real-world animal charities (donate a % of IAP to local shelters)?
*   Physical Merchandise? Plushies of the chunky animals? Enamel pins?
*   Twitch integration (chat votes on next level layout or hazards)?
*   Teleporters on the grid?
*   One-way streets that force specific pathing?
*   Bridges and overpasses to add verticality to the grid?
*   A "rewind" button to undo the last turn?
*   Different breeds of lead cats having unique passive stats (e.g., Siamese turns faster)?
*   Weather effects (rain makes the grid slippery)?
*   A photo mode to take pictures of your massive conga line?
