# Match Engine, Simulation & In-Game Cheats: Technical Specification

## 1. Built-In Developer Cheats & Debug Switches
A remarkable discovery in the Soccer Manager 2021 codebase is the existence of native debug tools left intact inside `Game` and `DebugTools`.

### 1.1 Native Static Cheat Flags in `Game`
| Flag Identifier | Default | Trainer Application |
| :--- | :---: | :--- |
| `Game.debugForceResults` | `OFF` | **Auto-Win / Match Outcome Switch**: Setting this enum to `Game.DebugForceResults.WIN` forces a 5-0 victory for the user's club in **every single match** simulated via `EngineQuick` or `Fixture.DebugForceResult()`. Options: `OFF`, `WIN`, `DRAW` (5-5), `LOSE` (0-5). |
| `Game.debugFast2DPitchEngine` | `false` | **Instant Match Sim**: Bypasses animation delays and executes the 2D pitch engine simulation at ultra-high speed. |
| `Game.instance.debugUnlimitedTraining`| `false` | **Infinite Training**: Removes the 2-session daily limit on player training drills and eliminates cooldowns. |
| `Game.debugAutoPickTeam` | `false` | Automatically picks the optimal 11-player lineup based on current rating and condition. |

---

## 2. Live Match Simulation Architecture: `Fixture` & `cSoccerEngine`

### 2.1 Score & Goals Manipulation
Active match scores are maintained on the `Fixture` object:
```csharp
public byte[] score = new byte[2]; // Index 0 = Home, Index 1 = Away
public byte homeScore => score[0];
public byte awayScore => score[1];

// Direct mutators:
public void Score(int which, byte newScore);
public void HomeScore(byte homeGoals);
public void AwayScore(byte awayGoals);
```

**Trainer Implementations**:
- **Add Home Goal**: Call `fixture.HomeScore((byte)(fixture.homeScore + 1))` during live match.
- **Add Away Goal**: Call `fixture.AwayScore((byte)(fixture.awayScore + 1))`.
- **Freeze Opposition Goals**: In live 2D/3D match, hook or intercept goal events where `scoringClubId != Game.instance.CurrentManagedClubId`.

### 2.2 Match Events & Disciplinary State
Events occurring in a fixture are logged in `List<GameEvent>` collections:
- `goals`: Player ID, match minute, penalty/own-goal flags.
- `yellow`: Player ID and timestamp.
- `red`: Sent-off player tracking.
- `sub`: Substitutions made (Max 3 in normal rules).
  - **Trainer Cheat: Unlimited Substitutions**: Reset the `sub.Count` or bypass the substitution count validation check in `Matchday.ValidateTactics()`.

---

## 3. Tactical Systems: `cTactics` & `Formation`
The user's active tactical configuration is stored in `Game.instance.CurrentManagedClub.currentTactics`:

### 3.1 Lineup Array (`lineUp`)
An 18-element integer array storing player IDs:
- Elements `0` to `10`: Starting XI players.
- Elements `11` to `17`: Substitute bench (7 players).

### 3.2 Tactical Instructions
Configured inside `currentTactics.inst`:
- `formationid`: Selected formation shape (4-3-3, 4-2-3-1, 3-5-2, etc.).
- `mentality`: Defensive, Balanced, Attacking, Very Attacking.
- `tacklingstyle`: Normal, Hard, Aggressive.
- `passingstyleid`: Short, Direct, Mixed, Long Ball.
- `attackingstyle`: Down both flanks, through middle, mixed.
- `tempoid`: Slow, Normal, Fast.
- `pressingid`: Own half, All over, High press.
- `counterattack` (`bool`): Counter-attacking toggle.
- `menbehindball` (`bool`): Park the bus mode.
- `tightmarking` (`bool`): Tightly mark opposing key players.
- `playoffside` (`bool`): Offside trap activation.
- `useplaymaker` (`bool`): Funnel transition play through playmaker.
- `usetargetman` (`bool`): Direct aerial clearances towards target forward.

---

## 4. Activating the Built-In "DebugTools" UI
Soccer Manager 2021 contains a fully functional developer debug menu located on the main UI canvas:
```csharp
GameObject debugTools = UI.GetInstance().overlayCanvas.transform.Find("DebugTools").gameObject;
debugTools.SetActive(true);
```
Once set to active, the GUI presents:
1. **Time Machine (Skip to Date)**: Skip forward 1 day, 3 days, 7 days, 14 days, 1 month, 3 months, 6 months, 12 months, or jump directly to the final match of the season.
2. **Player Ratings & Potential Inspector**: Full inspection and editing of player potentials, base ratings, and individual skill values.
3. **Instant Facilities Panel**: One-click level manipulation for all club facilities.
4. **Respawn & Youth Intake Tools**: Instant generation of youth intakes and regen players.
5. **Quick Match Runner**: High-speed match simulator with automated result reporting.
