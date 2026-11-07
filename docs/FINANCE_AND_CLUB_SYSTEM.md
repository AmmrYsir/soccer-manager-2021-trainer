# Club, Finances & Stadium Systems: Technical Specification

## 1. Core Financial Architecture: `ManagedClub`
In Soccer Manager 2021, all financial operations for the player's team are governed by `ManagedClub`, accessible at `Game.instance.CurrentManagedClub` (or `Game.instance.managedClub[Game.instance.g_currUser]`).

### 1.1 Key Financial Fields
| Field | Type | Description | Trainer Impact |
| :--- | :--- | :--- | :--- |
| `Balance` | `double` | Actual liquid club bank balance | **Primary Money Cheat**: Can be set to any positive double (e.g. `1,000,000,000.0`). The transfer engine directly checks `Balance >= (amount + signingFee)`. |
| `TransferBudget` | `double` | Cached transfer budget representation | Display/UI budget synchronization. |
| `WageBudget` | `double` | Maximum allowed weekly wage limit | Modify to allow massive wages without board complaints. |
| `StartBalance` | `double` | Starting balance at start of season | Tracked for season reporting. |
| `TicketPrice` | `int` | Current ticket price for home matches | Multiplier for matchday gate receipts. |
| `Attendance` | `int` | Expected match attendance | Affects matchday revenue. |
| `TvMoney` | `int` | Weekly TV revenue payout | Regular weekly cash injection. |
| `SponsorMoney` | `int` | Weekly sponsor revenue | Additional revenue stream. |
| `sacked` | `bool` | True if the manager has been fired by the board | **Freeze/Immunity Cheat**: Setting `sacked = false` prevents getting fired regardless of poor results or board confidence. |
| `FacilityScore` | `int` | Club facility evaluation score (0-10) | Impacts youth intake, player development, and club prestige. |

### 1.2 Weekly Balance Sheet (`BalanceSheet`)
Stored as a `List<BalanceSheet>` in `managedClub.BalanceSheet`. Each balance sheet records weekly financial inflows and outflows:
- **Income Fields**:
  - `SeasonTicketReceipts` (`double`)
  - `GateReceipts` (`double`)
  - `TVRevenue` (`double`)
  - `Sponsor` (`double`)
  - `Hospitality` (`double`)
  - `Merchandise` (`double`)
  - `TransfersIn` (`double`)
  - `CashInjection` (`double`)
  - `PrizeMoney` (`double`)
  - `OtherIncome` (`double`)
- **Expenditure Fields**:
  - `PlayerWages` (`double`)
  - `StaffWages` (`double`)
  - `GroundMaintenance` (`double`)
  - `TrainingGround` (`double`)
  - `YouthAcademy` (`double`)
  - `TransfersOut` (`double`)
  - `Administration` (`double`)
  - `Overheads` (`double`)
  - `OtherOutgoings` (`double`)

---

## 2. Facilities & Upgrades: `FacilityManager`
Club facility infrastructure is tracked in `FacilityManager`.

### 2.1 Facility Classification
The dictionary `FacilityLevels` (`Dictionary<int, int>`) maps facility ID to its current upgrade level (typically 0 through 5):
- `0`: Training Ground (accelerates player attribute gains, reduces fatigue drop).
- `1`: Youth Academy (increases youth intake frequency and size).
- `2`: Club Zone / Commercial (boosts merchandise and hospitality revenue).
- `4`: Scouting Network (improves scouting accuracy, lowers fog of war).

### 2.2 Instant Facility Upgrades
Ongoing upgrades are stored in `FacilityUnlocksInProgress` (`List<FacilityUnlock>`):
```csharp
public class FacilityUnlock
{
    public int facilityId;
    public int level;
    public int daysRemaining;
    public DateTime completionDate;
}
```
**Trainer Features**:
1. **Instant Facility Completion**:
   - Iterate through `FacilityUnlocksInProgress`, set `daysRemaining = 0` or set `FacilityLevels[id] = 5`, then clear the list.
   - Set `FacilityManager.FacilityRedraw = true` to refresh UI immediately.
2. **Zero Cost Upgrades**:
   - Bypass balance deduction in `FacilityController.UpgradeFacility()`.

---

## 3. Stadium System: `Stadium`

### 3.1 Structure & Capacity
- **Max Capacity**: `128,000` seats (`Stadium.MAX_STADIUM_CAPACITY`).
- **Piece Architecture**: The stadium consists of 8 modular pieces (`StadiumPiece[] yourStadium`):
  - 4 Corners (Max level: 1).
  - 4 Stands (2 Goal ends, 2 Sidelines; Max level: 8).
  - Roof types (0 to 8).

### 3.2 Instant Stadium Expansion Cheat
Construction state is tracked directly on `Game.instance`:
- `g_StadiumUpgradeInProgress` (`bool`): Set to `false` upon completion.
- `g_StadiumPieceConstructionFinish` (`DateTime`): Completion target date. Set to `Game.instance.CurrentDate` for instant finish.
- `g_AddedStadiumSeats` (`int`): Total capacity bonus added by upgrades.
- Set all `StadiumPiece[i].pieceLevel = 8` and `yourStadium[i].underConstruction = false` to achieve a fully upgraded 128,000-seat stadium instantly.

---

## 4. Sponsorships: `Sponsor`
Managed in `managedClub.CurrentSponsor` and `managedClub.AvailableSponsors`:
- Types: `ANNUAL` or `WEEKLY`.
- `GetPerSeasonAmount()` / `GetPerWeekAmount()`:
  - Can be multiplied or edited to generate massive recurring cash flow.
