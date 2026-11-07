# Soccer Manager 2021: Trainer Development Blueprint & Reference Implementation

## 1. Architectural Strategy Comparison

Because Soccer Manager 2021 is a 64-bit Unity Mono application with zero anti-cheat, there are three primary ways to implement a trainer:

```
+-----------------------------------------------------------------------------------+
|                            Trainer Architecture Options                           |
+--------------------------+------------------------------+-------------------------+
| Option A: BepInEx Plugin | Option B: C++ / Memory Hooks | Option C: Save Editor   |
+--------------------------+------------------------------+-------------------------+
| • Injected inside Mono   | • External GUI (C# / C++)    | • Offline File Editor   |
| • Full access to classes | • Reads memory pointers      | • Edits .db & ES2 files |
| • Zero offset breakage   | • Requires offset updates    | • Safe, no memory hooks |
| • 100% recommended      | • Good for standalone exe    | • Cannot do live match  |
+--------------------------+------------------------------+-------------------------+
```

---

## 2. Option A: In-Game BepInEx / Unity Doorstop Plugin (Recommended)

Using **BepInEx 5.4.x (x64)** allows dropping a single plugin DLL into `Soccer Manager 2021\BepInEx\plugins\SM21Trainer.dll`. The trainer executes inside the Unity engine loop with full type safety and zero memory pointer guessing.

### 2.1 Complete Ready-to-Compile Trainer Plugin (`SM21TrainerPlugin.cs`)

```csharp
using System;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;

namespace SM21Trainer
{
    [BepInPlugin("com.antigravity.sm21trainer", "SM21 Trainer", "1.0.0")]
    public class SM21TrainerPlugin : BaseUnityPlugin
    {
        private bool showUI = false;
        private Rect windowRect = new Rect(20, 20, 360, 520);

        // Cheat Toggles
        private bool freezeFitness = false;
        private bool forceWinMatch = false;
        private bool unlimitedTraining = false;

        private void Update()
        {
            // Toggle Trainer UI with F1
            if (Input.GetKeyDown(KeyCode.F1))
            {
                showUI = !showUI;
            }

            // Open Native Developer Menu with F2
            if (Input.GetKeyDown(KeyCode.F2))
            {
                ToggleDeveloperTools();
            }

            // Continuous Cheats
            if (Game.instance != null && Game.instance.CurrentManagedClub != null)
            {
                // Freeze Fitness & Max Morale
                if (freezeFitness && Game.instance.CurrentManagedClub.PlayerStatus != null)
                {
                    foreach (var status in Game.instance.CurrentManagedClub.PlayerStatus.Values)
                    {
                        status.fitness = 100;
                        status.morale = 5;
                    }
                }

                // Force 5-0 Auto Win in Match Simulation
                if (forceWinMatch)
                {
                    Game.debugForceResults = Game.DebugForceResults.WIN;
                }
                else if (Game.debugForceResults == Game.DebugForceResults.WIN)
                {
                    Game.debugForceResults = Game.DebugForceResults.OFF;
                }

                // Unlimited Training
                if (unlimitedTraining)
                {
                    Game.instance.debugUnlimitedTraining = true;
                }
            }
        }

        private void OnGUI()
        {
            if (!showUI) return;
            windowRect = GUI.Window(9999, windowRect, DrawTrainerWindow, "Soccer Manager 2021 Trainer [F1]");
        }

        private void DrawTrainerWindow(int windowID)
        {
            GUILayout.BeginVertical();

            GUILayout.Label("=== Club Finances ===");
            if (GUILayout.Button("Add $50,000,000 Cash"))
            {
                if (Game.instance?.CurrentManagedClub != null)
                {
                    Game.instance.CurrentManagedClub.Balance += 50000000.0;
                    Game.instance.CurrentManagedClub.WageBudget += 500000.0;
                }
            }

            if (GUILayout.Button("Prevent Getting Sacked (Board Immunity)"))
            {
                if (Game.instance?.CurrentManagedClub != null)
                {
                    Game.instance.CurrentManagedClub.sacked = false;
                }
            }

            GUILayout.Space(10);
            GUILayout.Label("=== Squad & Fitness ===");
            freezeFitness = GUILayout.Toggle(freezeFitness, "Infinite Player Stamina (100% Fitness)");

            if (GUILayout.Button("Heal All Squad Injuries"))
            {
                HealAllInjuries();
            }

            if (GUILayout.Button("Max All Squad Potentials (99)"))
            {
                MaxSquadPotential();
            }

            GUILayout.Space(10);
            GUILayout.Label("=== Match Day Cheats ===");
            forceWinMatch = GUILayout.Toggle(forceWinMatch, "Always Win Match (Force 5-0)");

            if (GUILayout.Button("Instant Fast Match Sim (2D Pitch)"))
            {
                Game.debugFast2DPitchEngine = true;
            }

            GUILayout.Space(10);
            GUILayout.Label("=== Manager & Meta ===");
            if (GUILayout.Button("Add 10,000 Credits & 1,000 Manager Pts"))
            {
                AddManagerCurrencies();
            }

            if (GUILayout.Button("Unlock All Manager Traits (Max Tier)"))
            {
                UnlockAllTraits();
            }

            unlimitedTraining = GUILayout.Toggle(unlimitedTraining, "Unlimited Training Sessions");

            GUILayout.Space(10);
            if (GUILayout.Button("Toggle Hidden Developer Menu [F2]"))
            {
                ToggleDeveloperTools();
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void ToggleDeveloperTools()
        {
            var overlay = UI.GetInstance()?.overlayCanvas;
            if (overlay != null)
            {
                var tools = overlay.transform.Find("DebugTools")?.gameObject;
                if (tools != null)
                {
                    tools.SetActive(!tools.activeSelf);
                }
            }
        }

        private void HealAllInjuries()
        {
            if (Game.instance?.sqLiteService != null)
            {
                Game.instance.sqLiteService.CreateInjuryTbl();
            }
            if (Game.instance?.CurrentManagedClub?.PlayerStatus != null)
            {
                foreach (int pid in Game.instance.CurrentManagedClub.PlayerStatus.Keys)
                {
                    if (Game.instance.g_PlayerData.ContainsKey(pid))
                    {
                        Game.instance.g_PlayerData[pid].Status = ePlayerStatus.FIT;
                    }
                }
            }
        }

        private void MaxSquadPotential()
        {
            if (Game.instance?.CurrentManagedClub?.PlayerStatus != null)
            {
                foreach (int pid in Game.instance.CurrentManagedClub.PlayerStatus.Keys)
                {
                    if (Game.instance.g_PlayerData.ContainsKey(pid))
                    {
                        Game.instance.g_PlayerData[pid].Potential = 99;
                    }
                }
            }
        }

        private void AddManagerCurrencies()
        {
            CustomerDataController.Instance?.AddCredits(10000);
            if (UserManagerProfileData.UserManagerProfile != null)
            {
                UserManagerProfileData.UserManagerProfile.ManagerPoints += 1000;
                UserManagerProfileData.UserManagerProfile.Boosts += 50;
                UserManagerProfileData.UserManagerProfile.VIP = true;
                UserManagerProfileData.SaveUserManagerProfile();
            }
        }

        private void UnlockAllTraits()
        {
            if (UserManagerProfileData.StaticTraits == null || UserManagerProfileData.UserManagerProfile == null) return;
            foreach (var trait in UserManagerProfileData.StaticTraits)
            {
                var existing = UserManagerProfileData.UserManagerProfile.UsersObtainedTraits
                    .Find(t => t.TraitID == trait.TraitID);
                if (existing == null)
                {
                    existing = new UsersManagerTraits(trait);
                    UserManagerProfileData.UserManagerProfile.UsersObtainedTraits.Add(existing);
                }
                existing.CurrentTraitLevel = Math.Max(0, trait.LevelCount - 1);
            }
            UserManagerProfileData.SaveUserManagerProfile();
        }
    }
}
```

---

## 3. Option B: External Memory Trainer (Cheat Engine / C++ / C# Memory Engine)

For external memory trainers connecting via `OpenProcess` and `ReadProcessMemory`:

### 3.1 Static Pointer Chains (Mono Domain)
In Unity Mono games, the root domain and static fields are traversed through Mono structures:
1. `mono-2.0-bdwgc.dll` exports `mono_domain_get()`, `mono_assembly_loaded()`, and `mono_class_get_field_from_name()`.
2. Static class: `Game` -> field: `instance` (pointer to `Game` GameObject component).
3. From `Game.instance`:
   - `+Offset` -> `managedClub` (`ManagedClub[]` array)
   - `managedClub[0]` (`ManagedClub` instance):
     - `+0x18`: `double Balance` (Main Cash Balance)
     - `+0x20`: `double WageBudget`
     - `+0x28`: `double TransferBudget`
     - `+0x48`: `bool sacked` (1 byte, 0 = Active, 1 = Fired)
     - `+0x60`: `Dictionary<int, PlayerStatusInfo> PlayerStatus`
4. Global Static Variable:
   - `Game.debugForceResults` (4-byte Int32: `0` = Off, `1` = Win, `2` = Lose, `3` = Draw)
   - `Game.debugFast2DPitchEngine` (1-byte Boolean: `0` = Normal, `1` = Fast)

---

## 4. Option C: Offline Save & Database Editor

1. **Direct SQLite Query on Save Game (`SM21_<slot>.db`)**:
   - Location: `%USERPROFILE%\AppData\LocalLow\Soccer Manager Ltd\SM21\SM21_<slot>.db`
   - **Instantly Heal All Injured Players**:
     ```sql
     DELETE FROM PlayerInjuryTbl;
     ```
   - **Remove All Red Card Match Suspensions**:
     ```sql
     DELETE FROM GameDisciplinaryEventTbl WHERE cardType = 'Red';
     ```
2. **Global Profile Modification (`sm21_User_Profile`)**:
   - File contains binary EasySave2 payload for `UserManagerProfile`.
   - Modifying fields:
     - `ManagerPoints`: Trait purchase currency.
     - `Credits`: Premium currency.
     - `ManagerLevel`: Current level (set to 50).
     - `VIP`: Boolean flag (set to 1).
