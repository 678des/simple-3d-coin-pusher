# Coin Pusher Arcade

## Overview
This project is a 3D Physics-based Coin Pusher simulation. Players drop coins to push others off a ledge into scoring zones, triggering bonuses, slot machine events, and wall barriers.

## Setup Guide

### 1. Hierarchy Setup
- **GameManager**: Create an Empty object named `GameManager`. Add `GameManager.cs` and `UIManager.cs`.
- **CoinSpawner**: Create an Empty object named `CoinSpawner`. Add `CoinSpawner.cs`.
- **Pusher**: Create a Cube, scale it as a pusher bar. Add `Rigidbody` (set `isKinematic=true`) and `PusherMovement.cs`.
- **UI**: Ensure you have a Canvas with `TextMeshPro` elements for Score, Coins, Fever, and Wall Timers.

### 2. Component Attachments & Assignments
- **GameManager**: Attach `GameManager.cs`.
- **UIManager**: Attach `UIManager.cs`. Assign inspector variables:
  - `scoreText`, `coinText`: Reference the TMP objects.
  - `gameOverUI`: Reference your UI game over panel.
  - `feverSlider`: Reference a UI Slider component.
- **SlotMachine**: The system automatically adds an instance via `SlotMachineController` at runtime.
- **Colliders**: Ensure all `ScoreZone` and `SideLossZone` objects have `Collider` components with `isTrigger` enabled.

### 3. Features
- **Procedural Effects**: Particle bursts are generated via `EffectsManager` without needing external prefabs.
- **Dynamic UI**: UI fallbacks generate automatically if references are missing.
- **Slot Machine**: Triggered by coins entering pockets on the pusher. Features '777' jackpots and coin showers.
- **Global Physics**: Modifiers like Gravity and TimeScale are managed by `GlobalPhysicsManager`.