# Save System, SQLite Schema & File Formats: Technical Specification

## 1. Storage Architecture Overview
Soccer Manager 2021 employs a dual-tiered persistence system:
1. **Easy Save 2 (ES2) Binary Archives**: Serialized C# object graph for memory snapshots (in-memory game state, objects, complex collections).
2. **SQLite3 Relational Database**: Relational tables for event histories, active bids, injuries, awards, and historical fixture records.
3. **Global User Profile**: Serialized independently from slot saves to maintain manager progression across different careers.

Save Folder: `%USERPROFILE%\AppData\LocalLow\Soccer Manager Ltd\SM21\`

---

## 2. File Specifications

### 2.1 The Slot Binary Save (`SM21_save_<slot>`)
- **Technology**: Moodkie Easy Save 2 (ES2)
- **Format**: `ES2Settings.Format.Binary`
- **Default Encryption**: `defaultEncrypt = false` (Plain unencrypted binary stream).
- **Fallback / Embedded Password**: If encryption is enabled by any sub-routine, the hardcoded AES-128 key is:
  ```
  f2WYP35djvxP2pdR
  ```
- **Tags contained within `SM21_save_<slot>`**:
  1. `tag=metadata` -> De-serializes to `SaveMetadata`:
     - `Balance` (`int`): Stored club cash snapshot.
     - `SaveDate` (`DateTime`): Timestamp of the save.
     - `GameDate` (`DateTime`): Current in-game season calendar date.
     - `ClubID` (`short`): Managed club identifier.
     - `ClubDataID` (`int`): Static club reference.
     - `SaveUID` (`string`): Unique GUID for save integrity and cloud sync.
     - `LeaguePosition` (`int`): Active table placement.
     - `DataPackID` (`int`): Active database pack identifier.
  2. `tag=game` -> De-serializes to the complete `Game` singleton instance:
     - `CurrentDate`, `managedClub[]`, `g_ClubData`, `g_PlayerData`, `g_LeagueData`, `g_youthManager`, `g_trainingManager`, `currentTactics`, etc.

### 2.2 Global User Profile (`sm21_User_Profile`)
- **Technology**: Easy Save 2 Binary.
- **De-serializes to**: `UserManagerProfile`
  - `ManagerName` (`string`)
  - `ManagerLevel` (`int`)
  - `ManagerXP` (`int`)
  - `ManagerPoints` (`int`)
  - `Credits` (`int`)
  - `Boosts` (`int`)
  - `VIP` (`bool`)
  - `UsersObtainedTraits` (`List<UsersManagerTraits>`)

---

## 3. SQLite Runtime Save Database (`SM21_<slot>.db`)
Engine: SQLite 3.54.0 compatible database created via `SQLite4Unity3d`.
This file is generated per save slot and updated during transactions.

### 3.1 Complete Table Inventory
| Table Name | Primary Key | Description | Key Columns |
| :--- | :--- | :--- | :--- |
| `PlayerInjuryTbl` | `Id` | Active injuries affecting players | `playerId`, `injuryType`, `daysRemaining`, `injuryDate` |
| `PlayerInjuryHistoryTbl`| `Id` | Historical injury ledger | `playerId`, `injuryType`, `gamesMissed`, `seasonId` |
| `ContractInfoTbl` | `Id` | Ongoing contract negotiation data | `playerId`, `clubId`, `wagesPerWeek`, `contractExpiry`, `signingFee` |
| `BidTbl` | `Id` | Active transfer market offers | `bidId`, `playerId`, `clubIdFrom`, `clubIdTo`, `amount`, `status` |
| `FreeAgentTbl` | `Id` | Pool of uncontracted free agents | `playerId`, `reputation`, `wageExpectation` |
| `LoanProfileTbl` | `Id` | Active loan contracts | `playerId`, `parentClubId`, `loanClubId`, `returnDate`, `wageContribution` |
| `SquadProfileTbl` | `Id` | Squad roster compositions | `clubId`, `squadSize`, `averageRating`, `wageBill` |
| `TransferHistoryTbl` | `Id` | Historical completed transfers | `playerId`, `fromClubId`, `toClubId`, `fee`, `date` |
| `FixtureTbl` | `Id` | Fixture results and scheduled games | `fixtureId`, `homeClubId`, `awayClubId`, `homeScore`, `awayScore`, `date` |
| `GameGoalEventTbl` | `Id` | Goal scorers per fixture | `fixtureId`, `playerId`, `minute`, `isPenalty`, `isOwnGoal` |
| `GameDisciplinaryEventTbl` | `Id` | Yellow and red cards | `fixtureId`, `playerId`, `minute`, `cardType` |
| `TrainingDrillsLogTbl` | `Id` | Player training execution logs | `playerId`, `drillId`, `attributeGain`, `date` |

### 3.2 Trainer Direct SQL Queries (Save Editing)
- **Heal All Injured Players Instantly**:
  ```sql
  DELETE FROM PlayerInjuryTbl;
  ```
- **Clear Disciplinary Cards / Bans**:
  ```sql
  DELETE FROM GameDisciplinaryEventTbl WHERE cardType = 'Red';
  ```
- **Wipe All Pending Opposition Transfer Bids**:
  ```sql
  DELETE FROM BidTbl;
  ```
