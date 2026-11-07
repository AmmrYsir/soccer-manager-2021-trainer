# Transfers, Scouting & Youth Academy: Technical Specification

## 1. Transfer Negotiations & Market Mechanics

### 1.1 The Instant Sign Cheat (`TransferPlayer.transferPlayer`)
In `Player.cs` (lines 2010-2028), the game contains an internal instant-signing routine utilized by the premium credit store:
```csharp
TransferPlayer.transferPlayer(
    int playerId, 
    short fromClubId, 
    short toClubId, 
    int transferCost, 
    int contractWages, 
    DateTime contractExpiry, 
    ePlayerSquadStatus expectedSquadStatus
);
```
**Trainer Cheat: "Free Instant Transfer"**:
- Calling this method directly bypasses all AI transfer valuation checks, transfer window deadlines, rival club refusal blocks, and wage negotiations.
- Parameters:
  - `toClubId = Game.GetCurrentUserClubID()`
  - `transferCost = 0`
  - `contractWages = 1000` (or player baseline)
  - `contractExpiry = Game.instance.CurrentDate.AddYears(5)`

### 1.2 Transfer Bid Evaluation (`TransferBid`)
When submitting conventional transfer offers, `TransferBid` processes AI club and player reactions:
- **Affordability Check**:
  ```csharp
  public bool managedClubBalanceSufficient()
  {
      return Math.Round(Game.instance.CurrentManagedClub.Balance) - (double)(amount + contract.signingFee) >= 0.0;
  }
  ```
  The game checks solely that liquid `Balance >= amount + signingFee`.
- **Bid Acceptance Criteria**:
  If the offering club is the managed club, `TransferBid.rejectTransferBid()` will auto-accept if `amount >= Player.GetValue(playerId) * 2f`.
  - **Trainer Patch / Hook**: Intercept `TransferBid.rejectTransferBid` or set `this.processed = eBidStatus.ACCEPTED` on every bid to force 100% acceptance on 1-dollar bids.

---

## 2. Scouting Mechanics: `Scout` & `PlayerShortlist`

### 2.1 Scouting Workflow & Rating Reveal
- Unscouted players have their individual technical, physical, and mental attributes hidden behind `-1f` placeholders via `Attributes.GetAttributesAffectedByFogOfWar`.
- The scouting database is maintained in:
  ```csharp
  Game.instance.g_PlayersScouted // Dictionary<int, ScoutReport>, key = playerID
  ```
- **Scout Report Completion**:
  Each entry has an `attributeEffect` (`float` from `0.0f` to `1.0f`).
  - `1.0f` represents 100% scouting completion (full stats, exact potential, contract demands, personality revealed).
  - **Trainer Cheat: 100% Instant Global Scout**:
    Loop through all player IDs in `Game.instance.g_PlayerData.Keys` and populate `g_PlayersScouted[id]` with `attributeEffect = 1.0f`.

---

## 3. Youth Academy & Regens: `YouthAcademy` & `YouthManager`

### 3.1 Youth Intake Generation Architecture
Intakes are controlled by `Game.instance.g_youthManager`:
- `NextYouthIntakeDate` (`DateTime`): The date the next batch of academy youth players will arrive.
- `CurrentIntakePlayers` (`List<int>`): Candidate youth player IDs.

### 3.2 Rating & Potential Calculations
The quality of generated youth prospects directly depends on the club's Youth Academy facility upgrades (levels 0 through 5):
```csharp
// Rating Generator:
public static byte GenerateYouthRating()
{
    int level = Facilities.GetUpgradeLevel(12);
    // Level 5 produces base ratings of 77 - 82 on 16-year-old recruits!
}

// Potential Generator:
public static byte GenerateYouthPotential()
{
    int level = Facilities.GetUpgradeLevel(13);
    // Level 5 produces potentials between 91 and 99!
}
```

### 3.3 Trainer Cheats:
1. **Instant Youth Intake**:
   - Set `Game.instance.g_youthManager.NextYouthIntakeDate = Game.instance.CurrentDate` and invoke `YouthAcademy.ProcessYouthIntake()`.
2. **Golden Generation Generator**:
   - Hook `GenerateYouthPotential()` to always return `99` (or `(byte)random.Next(95, 100)`).
   - Hook `GenerateYouthRating()` to return `85`.
   - Every generated youth player will become an immediate world-class superstar.
