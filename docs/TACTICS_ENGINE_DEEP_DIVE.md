# Soccer Manager 2021: Tactics Engine Deep Dive & Reverse Engineering Analysis

This document provides a complete technical analysis of how the tactical system operates inside *Soccer Manager 2021* (Steam App ID: `1448530`), reverse-engineered directly from `Assembly-CSharp.dll` (`EngineQuick.cs`, `Team.cs`, `cTactics.cs`, `cInstructions.cs`, `Tactics.cs`, `FormationData.cs`, `Constants.cs`, and `Fixture.cs`).

---

## 1. Simulation Engines: How Tactics Work Under the Hood

The game features two distinct match execution paths:
1. **Quick Engine (`EngineQuick.PlayFixture`)**: Used for all background league/cup simulations and instant match resolution.
2. **Full Pitch Engine (`cSoccerEngine`, `SeController`, `TacticsController`)**: Used when the user watches matches in 2D/3D mode.

---

### 1.1 Quick Engine Mathematical Formulation (`EngineQuick.cs`)

In `EngineQuick`, tactical performance is evaluated through a combination of **aggregate matchday ratings**, **positional scoring/assist weights**, and **tactical instruction modifiers**.

#### A. Total Team Rating & Goal Generation
The starting XI ratings are aggregated:
$$\text{TotalRating}_{\text{home}} = \sum_{k=0}^{10} \text{Rating}_{\text{home}}[k], \quad \text{TotalRating}_{\text{away}} = \sum_{k=0}^{10} \text{Rating}_{\text{away}}[k]$$

The score generation algorithm follows an exact formula:
- **If $\text{TotalRating}_{\text{home}} - \text{TotalRating}_{\text{away}} > 2$** (Home clear favorite):
  $$\text{HomeGoals} = \left\lfloor \frac{(\text{Rand} \pmod{100}) \times 7}{130} \right\rfloor \quad [0 \text{ to } 5 \text{ goals}]$$
  $$\text{AwayGoals} = \left\lfloor \frac{(\text{Rand} \pmod{100}) \times 3}{140} \right\rfloor \quad [0 \text{ to } 2 \text{ goals}]$$
- **If $\text{TotalRating}_{\text{home}} - \text{TotalRating}_{\text{away}} \le 2$** (Close match):
  $$\text{HomeGoals} = \left\lfloor \frac{(\text{Rand} \pmod{100}) \times 7}{130} \right\rfloor \quad [0 \text{ to } 5 \text{ goals}]$$
  $$\text{AwayGoals} = \left\lfloor \frac{(\text{Rand} \pmod{100}) \times 3}{130} \right\rfloor \quad [0 \text{ to } 2 \text{ goals}]$$
- **If $\text{TotalRating}_{\text{away}} > \text{TotalRating}_{\text{home}}$**: Symmetric calculation favoring Away.
- **If $\text{TotalRating}_{\text{home}} == \text{TotalRating}_{\text{away}}$**:
  $$\text{HomeGoals} = \left\lfloor \frac{(\text{Rand} \pmod{100}) \times 5}{130} \right\rfloor, \quad \text{AwayGoals} = \text{HomeGoals}$$

#### B. Goalscorer and Assist Attribution Weights
When a goal is awarded, individual player selection is governed by hardcoded position weights (`EngineQuick.cs:56-120`):

| Role | Goalscorer Weight | Assist Weight | Relative Goal Frequency |
| :--- | :---: | :---: | :--- |
| **Attacker (`ePlayerPosition.ATTACKER`)** | **50** | 35 | **50x** more likely than Defender, **10x** more than Midfielder |
| **Midfielder (`ePlayerPosition.MIDFIELDER`)** | **5** | **50** | Primary creator (Highest assist probability) |
| **Defender (`ePlayerPosition.DEFENDER`)** | **1** | 10 | Rare set-piece/long-ball goal threat |

The probability that player $i$ scores is:
$$P(\text{Scorer} = i) = \frac{\text{Rating}_i \times \text{Weight}_{\text{pos}(i)}}{\sum_{j \in \text{Pitch}} \text{Rating}_j \times \text{Weight}_{\text{pos}(j)}}$$

#### C. Tackling Style Disciplinary Hazard (`EngineQuick.cs:310-348`)
Tackling style settings directly dictate foul and card frequency:
- `tacklingstyle = 1` (Normal): Base yellow card divisor is `9`; direct red card divisor is `330`.
- `tacklingstyle = 2` (Hard): Yellow divisor drops to `7`; red divisor drops to `310`.
- `tacklingstyle = 3` (Aggressive): Yellow divisor drops to `5`; red divisor drops to `290`.
- **Derby Matches**: In derby fixtures, yellow divisor drops to `4` (Normal) or `1` (Aggressive). When divisor is `1`, `Random % 1 == 0` evaluates to **100% foul/card rate**, leading to catastrophic red card spirals and suspensions.

---

### 1.2 Full Pitch Engine Mechanics (`cSoccerEngine.cs` & `Tactics.cs`)

In visual 2D/3D matches, tactics define player behavior across a **$9 \times 7$ spatial grid**:
- **Mentality (1=Very Defensive to 5=Very Attacking)**: Modifies forward run thresholds and shot selection distance. Attacking mentalities increase attacking box entries but stretch defensive recovery distance.
- **Passing Style (1=Short, 2=Mixed, 3=Direct, 4=Long Ball)**: Determines node search radius in the pass-decision tree. Short passing restricts targets to adjacent spatial grid cells; Direct/Long Ball targets the forward lines directly.
- **Tempo (1=Slow, 2=Normal, 3=Fast)**: Controls decision-tick latency and animation transitions. Fast tempo reduces time-on-ball before execution, producing more actions per minute.
- **Width (1=Narrow, 2=Normal, 3=Wide)**: Stretches lateral coordinates of fullbacks (`PLAYER_POS_DL`/`DR`) and wingers (`PLAYER_POS_FL`/`FR` / `PLAYER_POS_AML`/`AMR`).
- **Defensive Line / Backline (1=Deep, 2=Normal, 3=High)**: Offsets baseline defender coordinates towards the midfield line, enabling high-press traps or deep low-block defending.

---

## 2. Dynamic Tactical Adaptation: Does the AI Change Based on Opponent Tactics?

**YES.** The game engine features two distinct, hardcoded tactical response layers:

---

### 2.1 Layer 1: Pre-Match Strategic Formation Counter (`Team.cs:1159-1166`)

Before every fixture, the engine evaluates team quality:
```csharp
int oppTeamRatingDiff = averageRating - averageRating2; // Home vs Away
Team.LoadTeam(..., oppTeamRatingDiff);
```
Inside `Team.LoadTeam`:
```csharp
tm.formationid = PickFormation(clubid);
if (oppTeamRatingDiff > 5)
{
    tm.formationid = GetCounterFormation(tm.formationid, attacking: true);
}
if (oppTeamRatingDiff <= -5)
{
    tm.formationid = GetCounterFormation(tm.formationid, attacking: false);
}
```

#### The Formation Counter Matrix (`Team.GetCounterFormation`)

| Base Formation | AI is Heavy Favorite (`oppTeamRatingDiff > 5`) | AI is Underdog (`oppTeamRatingDiff <= -5`) |
| :--- | :--- | :--- |
| **4-4-2** (0) | Shifts to **4-2-4** (19) (4 attackers) | Shifts to **4-5-1** (2) |
| **4-3-1-2** (15) | Shifts to **4-2-4** (19) | Shifts to **4-5-1** (2) |
| **4-2-2-2** (20) | Shifts to **4-2-4** (19) | Shifts to **4-5-1** (2) |
| **3-5-2** (4) | Shifts to **3-4-1-2** (16) | Shifts to **5-4-1** (6) (Low-block park the bus) |
| **3-4-1-2** (16) | Shifts to **4-3-3** (1) | Shifts to **5-4-1** (6) |
| **3-4-2-1** (23) | Shifts to **4-3-3** (1) | Shifts to **5-4-1** (6) |
| **3-1-3-3** (27) | Shifts to **4-3-3** (1) | Shifts to **5-4-1** (6) |
| **5-4-1** (6) | Shifts to **3-4-2-1** (23) | Shifts to **4-5-1** (2) |
| **4-2-4** (19) | Shifts to **4-3-3** (1) | Shifts to **4-4-2** (0) |
| **All Other Formations** (e.g. 4-3-3, 4-2-3-1) | **4-3-3** (1) | **4-5-1** (2) |

#### Tactical Impact for the Player
1. **When you manage a dominant team**: Opponents with rating deficit $\le -5$ will **almost always drop into a 5-4-1 or 4-5-1 defensive shell** against you.
2. **When you manage an underdog team**: Giant clubs with rating advantage $> 5$ will **almost always deploy 4-2-4 or 4-3-3 ultra-aggressive shapes** against you.

---

### 2.2 Layer 2: In-Match Real-Time Reactive Adjustments (`Team.cs:819-861` & `Constants.cs:1922-1944`)

During live match simulation, the engine executes `Team.ChangeTactics()` at regular intervals based on **Time Segment**, **Score State**, and **Club Match Aim**.

#### Time Segments
- **Segment 0**: 0 – 44 min (First Half)
- **Segment 1**: 45 – 59 min (Early Second Half)
- **Segment 2**: 60 – 79 min (Mid Second Half)
- **Segment 3**: 80 – 90+ min (Late Game Crunch)

#### Gamestate Enums
- `0`: **Winning**
- `1`: **Drawing**
- `2`: **Losing**

#### Tactical Aim Delta Lookup Tables
The engine applies numerical deltas to `mentality`, `tempoid`, and `tacklingstyle`:

```csharp
g_tacticaction.tacklingstyle += delta;
g_tacticaction.mentality += delta;
g_tacticaction.tempoid += delta;
```

##### 1. When AI Aim is `MUST WIN` (`g_team_aims_mustwin`):
| Segment | Winning (0) | Drawing (1) | Losing (2) | Effect |
| :---: | :---: | :---: | :---: | :--- |
| **0** (0-44m) | 0 | +1 | +1 | Pushes tempo and mentality immediately if not winning |
| **1** (45-59m) | 0 | +1 | **+2** | Ramps to high attacking pressure if trailing at half-time |
| **2** (60-79m) | 0 | **+2** | **+2** | All-out attack if drawing or losing after 60 min |
| **3** (80-90m) | 0 | **+2** | **+2** | Maximum aggression/tempo in final 10 minutes |

##### 2. When AI Aim is `WIN` (`g_team_aims_win`):
| Segment | Winning (0) | Drawing (1) | Losing (2) | Effect |
| :---: | :---: | :---: | :---: | :--- |
| **0** (0-44m) | 0 | 0 | 0 | Baseline tactical instructions |
| **1** (45-59m) | 0 | 0 | 0 | Baseline tactical instructions |
| **2** (60-79m) | 0 | 0 | +1 | Slight increase in urgency if losing |
| **3** (80-90m) | -1 | +1 | +1 | Protects lead if winning; pushes for winner if drawing/losing |

##### 3. When AI Aim is `DRAW OK` (`g_team_aims_drawok`):
| Segment | Winning (0) | Drawing (1) | Losing (2) | Effect |
| :---: | :---: | :---: | :---: | :--- |
| **0** (0-44m) | -1 | 0 | 0 | Low risk play |
| **1** (45-59m) | -1 | 0 | 0 | Low risk play |
| **2** (60-79m) | **-2** | -1 | +1 | Ultra-defensive shutdown if leading |
| **3** (80-90m) | **-2** | -1 | +1 | Parks the bus to preserve a draw or win |

#### Red Card & Injury Adaptation
- If a team suffers a red card disadvantage (`numreds > numreds_opp + 1`), the team's objective is decremented (`g_tm_aim[tmix]--`).
- The engine identifies vacated tactical positions, automatically executes tactical swaps (`TacticChange.SWAPPLAYER`), and prioritizes substituting off a forward to maintain defensive integrity.

---

## 3. What Tactic is the Best Overall? (Mathematical Evaluation)

Based on the reverse-engineered engine math, the mathematically optimal overall tactical setup is:

### 3.1 The Optimal Formation: **4-3-3 (Formation ID 1)** or **4-2-3-1 (Formation ID 12)**

#### Why 4-3-3 / 4-2-3-1 Dominates:
1. **Positional Weight Exploitation**:
   - `EngineQuick` weights Attackers at **50** for scoring and **35** for assists.
   - Central Midfielders are weighted at **50** for assists.
   - The 4-3-3 fields **3 Attackers** (LW, ST, RW) and **3 Midfielders** (LCM, CM, RCM). This maximizes both scoring and assist weight pools simultaneously.
2. **AI Hardcounter Immunity**:
   - The AI's formation counter specifically targets 3-back formations (shifts to 5-4-1) and 4-4-2/4-3-1-2/4-2-2-2 (shifts to 4-2-4).
   - Neither **4-3-3** nor **4-2-3-1** has an explicit counter rule in the switch statement, forcing the AI into generic default behaviors.
3. **Flank Coverage**:
   - With 4 defenders (DL, DC, DC, DR), you maintain maximum resistance against opposition flank attacks (`attwingers`) while keeping your defensive line stable.

---

### 3.2 The Optimal Tactical Instructions

| Instruction | Optimal Setting | Technical Reason |
| :--- | :---: | :--- |
| **Mentality** | **Attacking (4)** | Generates high shot volume and box entries without triggering the severe defensive transition penalty of "Very Attacking" (5). |
| **Tempo** | **Fast (3)** | Decreases action-tick delay, increasing total action checks per 90 simulated minutes. |
| **Passing Style** | **Mixed (2)** or **Short (1)** | Direct/Long Ball suffers from high turnover rates in the midfield. Short/Mixed maximizes possession (55%–65% bracket in `EngineQuick.cs:615`). |
| **Attacking Style** | **Both Flanks (1)** or **Mixed (5)** | Exploits winger space and wide-forward assist weighting. |
| **Tackling Style** | **Normal (1)** | **CRITICAL**: Setting tackling to Hard (2) or Aggressive (3) reduces the card divisor from 9 to 7 or 5, nearly doubling yellow/red cards. Over a season, suspensions destroy squad depth. |
| **Pressing** | **All Over (2)** | Disrupts opposition passing trees without overcommitting defenders out of position (which happens on High Press 3). |
| **Counter Attack** | **ON (`true`)** | Activates instant transition logic when regaining possession in the defensive third. |
| **Men Behind Ball**| **OFF (`false`)** | Prevents strikers from dropping into defensive midfield during attacking transitions. |
| **Play Offside Trap**| **OFF (`false`)** | Avoids failed offside checks that grant 1v1 chances to opposing fast strikers. |

---

## 4. Trainer Development Opportunities

For our trainer implementation, these findings expose direct memory/hook targets:

1. **Instant Tactical Advantage**:
   - Overriding `Team.LoadTeam`'s `oppTeamRatingDiff` parameter forces the AI into defensive submission (`4-5-1`) regardless of team strength.
2. **Card Immune Aggressive Tackling**:
   - Freeze `fixture.addYellow` and `fixture.addRed` while running `tacklingstyle = 3` (Aggressive) to gain maximum tackle success without disciplinary penalties.
3. **Perfect Aim State**:
   - Lock `Game.instance.g_tm_aim[playerIndex] = 2` (`MUST WIN`) to maintain maximum attacking pressure throughout all 90 minutes.
