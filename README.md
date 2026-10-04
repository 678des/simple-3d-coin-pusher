# Coin Pusher Arcade Engine

## Overview
This project provides a robust physics-based Coin Pusher system. It features procedural element generation, arcade-style slot mechanics, and event-based feedback.

## Component Attachment & Inspector Assignment Table

| Script Name (.cs) | Target GameObject | Required Components & Inspector Assignments |
| :--- | :--- | :--- |
| **GameManager.cs** | GameManager | Assign `UIManager` reference. Set `initialCoins`. |
| **UIManager.cs** | GameManager | Assign `scoreText`, `coinText`, `gameOverUI`, `feverOverlay`, `comboText` (TMP objects). |
| **PusherMovement.cs** | Pusher | Rigidbody (isKinematic: True). Set `amplitude`, `speed`, `moveDirection`. |
| **Coin.cs** | CoinPrefab | Rigidbody. Configure `_initialRandomTorque`, `_outOfBoundsY`. |
| **CoinSpawner.cs** | SpawnerManager | Assign `coinPrefab` and `bumperPrefab` prefabs. |
| **SideWallsController.cs** | WallManager | Assign `leftWall` and `rightWall` transforms. |
| **SlotMachineController.cs** | SlotManager | Needs TMPro setup. Uses default procedural generation if empty. |
| **Bumper.cs** | BumperPrefab | BoxCollider (IsTrigger: True). |
| **ScoreZone.cs** | ScoreTrigger | BoxCollider (IsTrigger: True). |
| **SideLossZone.cs** | LossTrigger | BoxCollider (IsTrigger: True). |

## Setup Guide
1. **Dependencies:** Ensure **TextMeshPro** is imported in your Unity Project.
2. **Resources:** Place your `.wav` or `.mp3` files in `Assets/Resources/` matching: `FlipperImpact`, `FeverMusic`, `BumperHit`, `BumperMiss`, `WallToggle`, `SlotTick`, `SlotWin`, `SlotJackpot`, `Miss`.
3. **Scene:** The `SlotMachineController` and `EffectsManager` will automatically initialize at runtime if not found, but it is recommended to manually place them in the scene for better control.