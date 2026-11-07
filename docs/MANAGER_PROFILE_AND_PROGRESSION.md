# Manager Profile, Traits & Progression: Technical Specification

## 1. Manager Identity & Storage Architecture
Manager meta-progression in Soccer Manager 2021 operates globally across save files and is managed by:
- Static Controller: `UserManagerProfileData`
- Profile Entity: `UserManagerProfileData.UserManagerProfile`
- Save File: Saved directly to disk as `sm21_User_Profile` in `Application.persistentDataPath` using Easy Save 2.

---

## 2. Manager Profile Fields (`UserManagerProfile`)
| Field / Property | Type | Range | Description & Trainer Application |
| :--- | :--- | :--- | :--- |
| `ManagerName` | `string` | Text | Name displayed on trophies and manager history. |
| `ManagerLevel` | `int` | `1` to `50` | Current manager progression tier (Max: 50). Can be set directly to `50`. |
| `ManagerXP` | `int` | `0`+ | Total experience points earned from matches and trophies. |
| `ManagerPoints` | `int` | `0`+ | **Trait Currency**: Points used to unlock and upgrade manager skill tree traits. Infinite points cheat: set to `9999`. |
| `Credits` | `int` | `0`+ | **Premium In-Game Currency**: Used for instant signings, instant facility rushes, training packs. Set to `999999`. |
| `Boosts` | `int` | `0`+ | Pre-match morale and tactical boosters. Set to `999`. |
| `VIP` | `bool` | `true`/`false` | Activates premium VIP status. Set to `true` to unlock all VIP perks. |
| `UsersObtainedTraits`| `List<UsersManagerTraits>` | - | The collection of unlocked manager traits and their upgrade levels. |
| `TrophiesEarned` | `Dictionary<short, TrophyList>` | - | Trophy showcase archive. |

---

## 3. Manager Trait System (`ManagerTraitArea`)
Manager traits provide persistent passive bonuses across all club management mechanics.

### 3.1 All 22 Trait Areas (`ManagerTraitArea`)
| Trait Area Enum | Target Mechanic | Bonus Functionality |
| :--- | :--- | :--- |
| `StadiumFacilitiesCost` | Stadium & Facilities | Reduces construction costs for stand upgrades and facilities. |
| `SponsorshipTicketPrice`| Club Revenue | Multiplier on ticket prices and sponsorship agreements. |
| `PlayerWages` | Finances | Lowers squad wage demands during negotiations. |
| `StartingBalance` | Finances | Increases the starting bank balance when taking over a club. |
| `TransferSigning` | Transfers | Reduces transfer fee demands from AI clubs. |
| `SetPieceRating` | Match Simulation | Enhances team conversion rate from free kicks and corners. |
| `PreMatchBoost` | Matches | Amplifies the effectiveness of pre-match team talks and boosts. |
| `CohesionIncrease` | Squad | Accelerates team chemistry and player synergy accumulation. |
| `IncreaseFitnessAfterMatch`| Squad Health | Boosts post-match stamina recovery rate. |
| `MatchEngineSubRating` | Match Simulation | Grants a rating bonus to substitutes entering the pitch. |
| `TrainingSlot` | Training | Unlocks additional simultaneous training slots. |
| `TrainingDiscount` | Training | Lowers the credit/cash cost of running specialized drills. |
| `TrainingInjury` | Training | Reduces the probability of players suffering training injuries. |
| `TrainingPlayerCap` | Training | Expands the maximum number of players permitted per training drill. |
| `YouthTraining` | Youth Academy | Multiplies attribute gain speed for academy graduates. |
| `Scouting` | Scouting | Increases scouting report speed and reveals fog-of-war stats faster. |
| `Media` | Board / Fanbase | Improves board confidence and fan approval during press interactions. |
| `PlayerStats` | Squad | Passive bonus to general player match ratings. |
| `PlayerMorale` | Squad Health | Reduces morale penalties after losses or lack of playing time. |
| `CauseForConcern` | Squad Harmony | Dampens concern generation when players request new contracts. |
| `MatchTactics` | Match Simulation | Increases tactical familiarity and defensive structure execution. |
| `NewPlayerPosition` | Training | Speeds up retraining players into new secondary positions. |
| `YouthPlayersStats` | Youth Academy | Increases baseline rating and potential of new youth intakes. |

### 3.2 Instant Unlock All Traits Cheat
To unlock all traits at maximum tier programmatically:
```csharp
foreach (var trait in UserManagerProfileData.StaticTraits)
{
    var userTrait = UserManagerProfileData.UserManagerProfile.UsersObtainedTraits
        .FirstOrDefault(t => t.TraitID == trait.TraitID);
    if (userTrait == null)
    {
        userTrait = new UsersManagerTraits(trait);
        UserManagerProfileData.UserManagerProfile.UsersObtainedTraits.Add(userTrait);
    }
    userTrait.CurrentTraitLevel = trait.LevelCount - 1; // Set to max tier
}
UserManagerProfileData.SaveUserManagerProfile();
```
