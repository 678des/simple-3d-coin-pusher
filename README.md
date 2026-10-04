# Coin Pusher Arcade Project

## Overview
A 3D physics-based coin pusher game. The project utilizes procedural generation for obstacles and UI to minimize manual scene setup.

## Setup Instructions
1. Open the project in Unity 2022.3 or higher.
2. Create a base platform using a Cube and assign a 'Coin' tag to your Coin prefab.
3. The `GameManager`, `SlotMachineController`, and `EffectsManager` will handle their own initialization if not present in the scene, but should ideally be placed as persistent objects.

## Component Attachment & Inspector Assignment Table
| Script Name (.cs) | Target GameObject | Required Components & Inspector Assignments |
| :--- | :--- | :--- |
| GameManager.cs | GameManager | Assign UIManager reference, Set 'Initial Coins' (e.g., 50) |
| PusherMovement.cs | Pusher | Rigidbody (Is Kinematic), Configure Amplitude/Speed |
| Coin.cs | Coin Prefab | Rigidbody (Mass 1.0-5.0), Set _outOfBoundsY to -5.0 |
| UIManager.cs | UI_Canvas | Drag TMPro objects: scoreText, coinText, gameOverUI, feverOverlay, comboText |
| SideWallsController.cs | WallManager | Drag LeftWall and RightWall game objects |
| SlotMachineController.cs | SlotSystem | Configure symbols array, set spinDuration (1.8s) |