# Soccer Manager 2021: Reverse Engineering & Game Trainer Documentation

This workspace contains technical analysis, reverse engineering evidence, and trainer development specifications for **Soccer Manager 2021** (Steam App ID: `1448530`).

---

## 📚 Technical Documentation Index

| Document | Description |
| :--- | :--- |
| **[GAME_ARCHITECTURE_OVERVIEW.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/GAME_ARCHITECTURE_OVERVIEW.md)** | Engine architecture (Unity Mono AnyCPU), folder hierarchy, persistent paths, static references, and runtime topology. |
| **[FINANCE_AND_CLUB_SYSTEM.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/FINANCE_AND_CLUB_SYSTEM.md)** | Club finances (`ManagedClub.Balance`, wage/transfer budgets), weekly balance sheets, stadium expansion (max 128k), and facility upgrade timers. |
| **[PLAYER_AND_SQUAD_SYSTEM.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/PLAYER_AND_SQUAD_SYSTEM.md)** | Player structure (`PlayerData`), condition (`PlayerStatusInfo.fitness`), 25 outfield & 14 goalkeeper attributes, hidden personality traits, and Fog of War revealers. |
| **[MANAGER_PROFILE_AND_PROGRESSION.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/MANAGER_PROFILE_AND_PROGRESSION.md)** | Meta-progression (`UserManagerProfile`), Manager Levels (1-50), Manager Points, Credits, VIP status, and all 22 Manager Trait Areas. |
| **[MATCH_ENGINE_AND_TACTICS.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/MATCH_ENGINE_AND_TACTICS.md)** | Built-in debug cheats (`Game.debugForceResults = WIN`), live match score mutators (`Fixture.score`), formation lineups, and the hidden Developer Menu (`DebugTools`). |
| **[TRANSFERS_SCOUTING_AND_YOUTH.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/TRANSFERS_SCOUTING_AND_YOUTH.md)** | Instant transfer cheat (`TransferPlayer.transferPlayer`), bid evaluation logic, global 100% scouting, and youth academy potential generation (91-99 rating). |
| **[SAVE_SYSTEM_AND_STORAGE_SPEC.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/SAVE_SYSTEM_AND_STORAGE_SPEC.md)** | Dual persistence architecture: Moodkie Easy Save 2 (ES2) binary format & SQLite save database (`SM21_<slot>.db`), schema tables, and AES-128 key. |
| **[TRAINER_DEVELOPMENT_BLUEPRINT.md](file:///c:/Users/ammar/Desktop/Docs/soccer-manager-2021-trainer/TRAINER_DEVELOPMENT_BLUEPRINT.md)** | Complete C# BepInEx trainer implementation source code, Cheat Engine memory offsets, and offline save editor SQL recipes. |

---

## 🔍 Forensic Evidence Records (REA Attributions)

| Artifact | Provider / Tool | Evidence ID / Details |
| :--- | :--- | :--- |
| `Assembly-CSharp.dll` | REA Managed Analysis | `ev_fe43910bd8eb21fb7dad3c72f85a5933993e21b987bd3af877694fd45d835d4f` (PE32 AnyCPU, CLI v4.0.30319, 3,083 Types, 20,555 Fields, 23,491 Methods) |
| `Assembly-CSharp.dll` | REA Managed Members | `ev_d101fc328aa6276b3cd1ab05cb663609e98c47fcf4eb7c8b62a8689bbfae7de8` (Member definitions, CIL tuple hashes, ldstr string anchors) |
| `SM21_StaticData.db` | REA SQLite Inspector | `ev_7762049634148ba74480fc1887d076c7798beac8c17346524c3aca1fcb672410` (14 Tables: ClubDataRec, TransferTierRec, StadiumBuildingRec, etc.) |

---

## 🎮 Trainer Cheat Map Summary

```
┌──────────────────────────────────┬────────────────────────────────────────────────────────┐
│ Feature                          │ Implementation Target                                  │
├──────────────────────────────────┼────────────────────────────────────────────────────────┤
│ Unlimited Club Money             │ Game.instance.CurrentManagedClub.Balance += 50000000.0 │
│ Immunity from Board Sacking      │ Game.instance.CurrentManagedClub.sacked = false        │
│ 100% Squad Fitness (Infinite)    │ Game.instance.CurrentManagedClub.PlayerStatus[id].     │
│                                  │   fitness = 100; morale = 5                            │
│ Instant Win Match (5-0 Auto-Win) │ Game.debugForceResults = DebugForceResults.WIN         │
│ Instant Match Simulation         │ Game.debugFast2DPitchEngine = true                     │
│ Unlimited Daily Training         │ Game.instance.debugUnlimitedTraining = true            │
│ Free Instant Transfer            │ TransferPlayer.transferPlayer(...)                     │
│ Instant Facility Completion      │ Clear FacilityManager.FacilityUnlocksInProgress        │
│ Instant 128,000 Capacity Stadium │ Set all yourStadium[i].pieceLevel = 8                  │
│ Max Manager Level (50) & Points  │ UserManagerProfileData.UserManagerProfile.ManagerLevel │
│ Unlock All Manager Traits        │ Populate UserManagerProfile.UsersObtainedTraits        │
│ Reveal All Player Stats (No FoW) │ Game.instance.g_PlayersScouted[id].attributeEffect=1.0 │
│ Open Hidden In-Game Dev Menu     │ UI.GetInstance().overlayCanvas.Find("DebugTools")      │
│                                  │   .gameObject.SetActive(true)                          │
└──────────────────────────────────┴────────────────────────────────────────────────────────┘
```
