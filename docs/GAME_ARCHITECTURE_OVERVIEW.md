# Soccer Manager 2021: Game Architecture & Technical Overview

## 1. Executive Summary & Engine Identification
- **Game Title**: Soccer Manager 2021 (SM21)
- **Developer / Publisher**: Soccer Manager Ltd
- **Target Platform**: Windows x86_64 (Steam App ID: `1448530`)
- **Game Engine**: Unity Engine (Unity 2019/2020 series)
- **Runtime Model**: Unity Mono runtime (`MonoBleedingEdge`, CIL/bytecode based)
- **Architecture**: AnyCPU managed assemblies targeting .NET Framework v4.0 / CLR 4.0
- **Primary Game Assembly**: `SM21_Data\Managed\Assembly-CSharp.dll` (SHA-256: `6c5a44ffe0ab675df9a0354363d85c352a16810dd60f968c72dfcbe990af7ee9`)
- **First-Pass Assembly**: `SM21_Data\Managed\Assembly-CSharp-firstpass.dll`
- **Security / Anti-Cheat**: Zero client-side anti-cheat drivers (No EAC, BattlEye, or Denuvo). Memory is unprotected and readable/writable.

---

## 2. Directory Layout & Storage Topology

```
Soccer Manager 2021 (Install: D:\SteamLibrary\steamapps\common\Soccer Manager 2021)
├── MonoBleedingEdge\               -> Mono runtime execution environment & core CLR libraries
├── SM21.exe                        -> Unity bootstrapper executable (x64)
├── UnityPlayer.dll                 -> Native engine player runtime
├── steam_api.dll / steam_api64.dll -> Steamworks integration wrapper
└── SM21_Data\
    ├── boot.config                 -> Engine flags (vr-enabled=0, etc.)
    ├── app.info                    -> Company ("Soccer Manager Ltd"), Product ("SM21")
    ├── Managed\                    -> C# managed assemblies (Assembly-CSharp.dll, etc.)
    ├── StreamingAssets\
    │   ├── SM21_StaticData.db      -> SQLite static database (leagues, cups, stadium piece base data)
    │   └── build_info.txt          -> Build metadata
    └── Resources\                  -> Bundled Unity asset assets
```

### User Save and Configuration Directories
1. **Save Game Directory (`Application.persistentDataPath`)**:
   - Path: `%USERPROFILE%\AppData\LocalLow\Soccer Manager Ltd\SM21\`
   - Save Files per Slot (Slot `N`, e.g., 0, 1, 2...):
     - `SM21_save_N`: Easy Save 2 (ES2) serialized binary object graph (`tag=metadata`, `tag=game`).
     - `SM21_N.db`: SQLite runtime save database (injuries, active bids, free agents, fixture events).
     - `SaveLog.txt`: Transactional save verification log.
   - User Profile File:
     - `sm21_User_Profile`: Global manager profile, XP, manager level, credits, boosts, unlocked traits.
2. **Registry Configuration (`PlayerPrefs`)**:
   - Hive: `HKCU\Software\Soccer Manager Ltd\SM21`
   - Stores graphics, sound, match speed, save slot pointers (`Slot_<SteamID>_<SlotNum>`), tutorial state flags, and spotlight seeds.

---

## 3. Core Static & Runtime Data Models

### 3.1 Static Database (`SM21_StaticData.db`)
Located at `SM21_Data\StreamingAssets\SM21_StaticData.db` (SQLite 3 format):
- `ClubDataRec`: Club identifiers, rivalries, transfer tier (`transferTier`), kit style references.
- `LeagueDataRec`: League hierarchy, associations, division tiering.
- `StadiumBuildingRec`: Stand 1-8 and Roof 1-8 piece building base definitions.
- `StadiumPieceCapacitiesRec`: Seat amount per piece level for corners, goal ends, and main stands.
- `StadiumBuildTimesRec`: Required construction days per piece level.
- `TransferTierRec`: Minimum/maximum skills, squad quotas by position, potential caps, valuation thresholds.
- `TransferWindowRec`: Association calendar window start and end days/months.
- `LeagueScheduleRec` & `CupScheduleRec`: Fixture scheduling calendar matrix.

### 3.2 Key Singleton References for Trainers
| Singleton Identifier | Class Type | Description |
| :--- | :--- | :--- |
| `Game.instance` | `Game` (MonoBehaviour) | Master game engine controller, holds club arrays, season, date, timers, debug flags |
| `Game.instance.CurrentManagedClub` | `ManagedClub` | The club currently being managed by the user (funds, squad, tactics) |
| `Game.instance.CurrentManagedClubId`| `short` | Active user club ID |
| `CustomerDataController.Instance` | `CustomerDataController` | In-game credits, boost amounts, shop unlocks |
| `UserManagerProfileData.UserManagerProfile`| `UserManagerProfile` | Manager XP, level (1-50), trait points, obtained traits |
| `SteamManager.Instance` | `SteamManager` | Steamworks interface wrapper |
| `UI.GetInstance()` | `UI` | Screen navigator, overlay canvas, modal controller |

---

## 4. Trainer Implementation Vectors

1. **Mono Injection (Recommended: BepInEx 5.x x64 or Unity Doorstop)**:
   - Drop `version.dll` or `winhttp.dll` with a BepInEx plugin into the root folder.
   - Direct access to `Game.instance`, `ManagedClub`, `PlayerData`, `PlayerStatusInfo`.
   - Allows activating the game's built-in hidden **Debug Tools UI** directly.
2. **External Memory Trainer (Cheat Engine / C++ Process Memory)**:
   - Read/write via standard Windows memory APIs (`ReadProcessMemory` / `WriteProcessMemory`).
   - Use Mono API exports in `mono-2.0-bdwgc.dll` or standard static field pointer chains:
     - `Game.instance` -> `+Offset` -> `managedClub[0]` -> `+Offset` -> `Balance`.
3. **Save Game & Database Editor**:
   - Direct SQL modifications on `SM21_N.db` (injuries, bids, free agents).
   - De-serialization of `SM21_save_N` using Easy Save 2 reader or custom binary parser.
