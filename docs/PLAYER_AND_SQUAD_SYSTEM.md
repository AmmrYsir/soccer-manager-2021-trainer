# Player & Squad Systems: Complete Attribute & Status Specification

## 1. Player Data Model Overview
Player representations are split across two core layers:
1. `PlayerStaticData`: Invariant biographical and baseline properties (Date of Birth, Height, Weight, Natural Foot, Baseline Potential).
2. `PlayerData` (inherits `PlayerStaticData`): Dynamic runtime state (Current Overall, Club Contract, Wages, Expiry Date, Squad Status, Progression, Hidden Traits).
3. `PlayerStatusInfo`: Condition, match readiness, morale, cohesion, and concerns.

All players in the game world are indexed in the master dictionary:
```csharp
Game.instance.g_PlayerData // Dictionary<int, PlayerData>, key = playerID
```

---

## 2. Dynamic Player Properties (`PlayerData`)
| Field / Property | Type | Description | Trainer Impact |
| :--- | :--- | :--- | :--- |
| `Rating` | `byte` | Overall player ability score (0 - 99) | Direct skill editing. Affects match simulation weights. |
| `CacheRating` | `byte` | Cached rating value used for fast comparisons | Must be synchronized when changing `Rating`. |
| `ClubId` | `short` | The club the player is currently registered to | Instant transfer without contract negotiation. |
| `Potential` | `byte` | Maximum possible progression rating ceiling | Set to `99` for god-tier youth progression. |
| `ContractWage` | `int` | Player's weekly salary in club currency | Set to `1` or `0` for free contracts. |
| `ContractExpiryDate`| `DateTime` | Expiration date of the contract | Set to `2040+` for permanent player contracts. |
| `SquadStatus` | `ePlayerSquadStatus` | Squad role (`KeyPlayer`, `FirstTeam`, `Rotation`, `Backup`, `HotProspect`, `NotNeeded`) | Resolves squad status dissatisfaction. |
| `Status` | `ePlayerStatus` | Health/availability (`Fit`, `Injured`, `Suspended`, `OnLoan`, `Retiring`) | Instant recovery from long-term injuries or suspensions. |
| `InDecline` | `bool` | Whether age-related attribute degradation is active | Set to `false` to freeze older players in their prime. |
| `SquadNo` | `byte` | Shirt number | Cosmetic customization. |

---

## 3. Player Condition & Morale (`PlayerStatusInfo`)
For the user's club, active player statuses are stored in:
```csharp
Game.instance.CurrentManagedClub.PlayerStatus // Dictionary<int, PlayerStatusInfo>, key = playerID
```

### 3.1 Status Fields
| Field | Type | Scale / Range | Description / Trainer Use |
| :--- | :--- | :--- | :--- |
| `fitness` | `byte` | `0` to `100` | Current physical condition / stamina. **Freeze at 100** for infinite match energy. |
| `morale` | `byte` | `1` to `5` (`4` = High, `5` = Very High) | Psychological morale. Maximize to 5 for peak performance. |
| `moraleBoost` | `byte` | `0`+ | Active morale modifier. |
| `Cohesion` | `PlayerTeamCohesion` | Complex object | Team chemistry. High cohesion optimizes tactical synergy and passing fluidity. |
| `Concerns` | `PlayerConcernsManagedClub` | Complex object | Tracks grievances (PlayingTime, Wages, ClubAmbition, ContractExpiry). Can be cleared to zero out all unrest. |

---

## 4. Attributes Architecture (`PlayerAttributes` & `Attributes`)
Individual player stats are stored inside `PlayerAttributes` (`attrs` array of `double`, `attrsDisplay` array of `float`).

### 4.1 Outfield Player Attributes Mapping
Indexed by numeric ID (`attrId`):

| ID | Name | Category | Description |
| :---: | :--- | :--- | :--- |
| **0** | Aggression | Mental | Willingness to contest tackles and press aggressively |
| **1** | Composure | Mental | Calmness under pressure from defenders or attackers |
| **2** | Concentration | Mental | Focus throughout 90 minutes; prevents mistakes |
| **4** | Pace | Physical | Sprint speed and acceleration |
| **5** | Strength | Physical | Physical power in shoulder duels and aerial contests |
| **6** | Aerial Ability | Physical | Jumping reach and heading prowess |
| **7** | Flair | Mental | Unpredictability, skill moves, and creative instinct |
| **8** | Leadership | Mental | Captaincy influence, inspires squad morale |
| **10** | Marking | Technical | Man-marking discipline |
| **11** | Tackling | Technical | Ground standing and sliding tackle accuracy |
| **12** | Positioning | Technical | Off-the-ball positioning (defensive shape) |
| **13** | Creativity | Technical | Vision to spot openings and through-balls |
| **14** | Movement | Technical | Attacking runs and finding open space |
| **15** | Finishing | Technical | Accuracy and composure when shooting inside the box |
| **16** | Dribbling | Technical | Ball control at pace, beating defenders |
| **17** | Long Shots | Technical | Power and accuracy from outside the penalty box |
| **18** | Crossing | Technical | Quality of deliveries from wide areas |
| **19** | Control | Technical | First touch and ball reception |
| **20** | Long Passing | Technical | Accuracy of aerial cross-field switches |
| **21** | Passing | Technical | Ground passing accuracy and weight |
| **22** | Corners | Technical | Corner delivery accuracy |
| **23** | Free Kicks | Technical | Direct and indirect set-piece shooting |
| **24** | Penalties | Technical | Penalty spot conversion rate |
| **25** | Stamina | Physical | Rate of matchday fitness depletion |
| **27** | Work Rate | Physical | Defensive and offensive tracking effort |

### 4.2 Goalkeeper Attributes Mapping
| ID | Name | Category | Description |
| :---: | :--- | :--- | :--- |
| **0** | Aggression | Mental | Aggressiveness in closing down attackers |
| **1** | Composure | Mental | Composure when pressured with the ball |
| **2** | Concentration | Mental | Vigilance during quiet match periods |
| **4** | Pace | Physical | Quickness off the goal line |
| **5** | Strength | Physical | Physical presence against opposing strikers |
| **6** | Aerial Ability | Physical | High ball claiming ability in crowded boxes |
| **8** | Leadership | Mental | Organization of the defensive line |
| **10** | Command of Area | Technical | Dominance inside the 6-yard and 18-yard box |
| **11** | Distribution | Technical | Long throws and goal kick accuracy |
| **12** | Handling | Technical | Catching cleanly vs parrying into danger |
| **13** | One On Ones | Technical | Shot-stopping when isolated against an attacker |
| **14** | Shot Stopping | Technical | Reflexes and agility on fast shots |
| **15** | Positioning | Technical | Angles and goal coverage |
| **16** | Saving Penalties | Technical | Penalty prediction and reaction |

---

## 5. Hidden Attributes (`PlayerHiddenAttributes`)
Accessible at `PlayerData.HiddenAttributes`. Scale: `1` (Lowest) to `10` (Highest):
1. **Ambition**: Drives desire for higher wages, bigger clubs, and continental competition. High ambition causes player dissatisfaction if club performance lags.
2. **Loyalty**: Attachment to current club. High loyalty prevents transfer requests and reduces contract renewal demands.
3. **Handling Pressure**: Resilience in high-stakes matches (finals, derbies, relegation deciders). High handling pressure ensures players play to full ability in big games.

---

## 6. Fog of War & Scouting Revealer
In the base game, player attributes for unscouted players are obscured by returning `-1f` via `Attributes.GetAttributesAffectedByFogOfWar()`.
- **Global Revealer Cheat**:
  - Setting `Game.instance.g_PlayersScouted[playerId].attributeEffect = 1.0f` for all players completely eliminates fog of war, rendering every stat in the database visible.
  - Alternatively, hooking `GetAttributesAffectedByFogOfWar()` to always return an empty list reveals 100% of stats globally without needing scouts.
