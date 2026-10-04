# Coin Pusher Arcade Engine

## Overview
This project provides a robust, physics-based framework for a 3D Coin Pusher arcade game. It includes procedural wall systems, dynamic physics manipulation (chaos mode), and a decoupled sound/effects management system.

## Getting Started
1. Clone the project and open in Unity 2022.3 LTS or higher.
2. Ensure TextMeshPro is imported via the Package Manager.
3. Open the `Main` scene in `Assets/Scenes/`.

## Component Attachment & Inspector Assignment Table

| Script Name (.cs) | Target GameObject | Required Components & Inspector Assignments |
| :--- | :--- | :--- |
| **GameManager.cs** | GameManager | Set `initialCoins` to desired starting amount. |
| **Coin.cs** | Coin Prefab | Requires Rigidbody. Set `_initialRandomTorque` (e.g. 2.0) and `_outOfBoundsY` (e.g. -5.0). |
| **PusherMovement.cs** | PusherObj | Rigidbody (Kinematic). Set `_amplitude` (1.2) and `_speed` (1.5). |
| **UIManager.cs** | UI_Manager_Obj | Drag-and-drop references for `scoreText`, `coinText`, `gameOverUI`, `feverOverlay`, and `comboText`. |
| **CoinSpawner.cs** | SpawnerObj | Assign `coinPrefab` and `bumperPrefab` from your Project window. |
| **SideWallsController.cs** | WallController | If using static walls, assign `leftWall` and `rightWall`. Otherwise, leave blank for auto-generation. |
| **ScoreZone.cs** | Floor_Trigger | Requires BoxCollider set to `isTrigger`. |
| **SideLossZone.cs** | Side_Gap_Trigger | Requires BoxCollider set to `isTrigger`. |
| **SlotTriggerZone.cs** | Slot_Pocket_Trigger | Place as a trigger collider on the pusher board. |

## Key Features
- **Procedural Effects:** The `EffectsManager` auto-initializes itself; no manual particle setup required.
- **Chaos Mode:** Toggled via `ChaosModeManager`, affects global gravity and applies random forces to all 'Coin' tagged objects.
- **Dynamic Walls:** `SideWallsController` allows raising/lowering walls to prevent coin loss during bonus events.