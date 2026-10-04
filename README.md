# 3D Coin Pusher System

## Overview
This project implements a complete 3D Coin Pusher game loop. It utilizes a procedural approach for UI, walls, and particle effects, reducing the need for manual scene setup.

## Component Attachment & Inspector Assignment Table

| Script Name (.cs) | Target GameObject | Required Components & Inspector Assignments |
| :--- | :--- | :--- |
| GameManager.cs | GameManager | Assign reference to UIManager script on same object. |
| UIManager.cs | GameManager | Assign `scoreText`, `coinText`, `gameOverUI`, `feverSlider`, etc. |
| PusherMovement.cs | Pusher | Rigidbody (Kinematic: True, Interpolate: Interpolate). |
| Coin.cs | CoinPrefab | Require Rigidbody; set `_initialRandomTorque` (default 2.0). |
| ScoreZone.cs | ScoreZoneTrigger | Collider (isTrigger: True). |
| SideLossZone.cs | SideLossTrigger | Collider (isTrigger: True). |
| SideWallsController.cs | WallManager | Assign `leftWall` and `rightWall` GameObjects. |
| SlotMachineController.cs | SlotMachineManager | No manual assignments; auto-builds 3D billboard. |
| CoinSpawner.cs | Spawner | Assign `coinPrefab` and `bumperPrefab`. |

## Manual Setup Guide
1. **Setup Core Managers**: Create a persistent GameObject (or keep in scene) named 'Managers' and attach `GameManager`, `UIManager`, `PhysicsManager`, and `SoundManager`.
2. **Pusher Configuration**: The Pusher object requires a BoxCollider and Rigidbody. Attach `PusherMovement` and ensure `isKinematic` is checked to ensure it pushes coins correctly.
3. **UI Elements**: Ensure the scene has a Canvas. The `UIManager` will automatically attempt to generate a 'RichUI_Overlay' if fields are left null.
4. **Audio**: The `SoundManager` is ready for implementation; ensure the path `Resources/BumperImpact` contains an AudioClip for the bumper.