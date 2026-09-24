# Kingdom Last Stand

`TDadGame` is the Unity project for **Kingdom Last Stand**, a portrait mobile tower-defense, merge, and roguelite game targeting Android and iOS.

## Status

Production development has started. This repository is not yet a store-ready release. A build is considered releasable only after every gate in [`Documentation/RELEASE_GATES.md`](Documentation/RELEASE_GATES.md) passes.

## Engine

- Unity 6 LTS (`6000.0.60f1`)
- Universal Render Pipeline
- Android and iOS portrait targets
- Local-first, versioned progression
- Data-driven gameplay through ScriptableObjects

## Open the project

1. Install Unity Hub and Unity `6000.0.60f1` with Android Build Support.
2. On macOS, also install iOS Build Support and Xcode.
3. Add this repository as a Unity project and open it.
4. Run EditMode tests before making a build.

To generate the current battle preview, choose **Kingdom Last Stand → Create Demo Battle Scene** from the Unity editor menu, then press Play. See [`Documentation/PLAYABLE_DEMO.md`](Documentation/PLAYABLE_DEMO.md).

## Scope

The authoritative product scope is in [`Documentation/MASTER_GAME_SPEC.md`](Documentation/MASTER_GAME_SPEC.md). Development must not call the game complete because it merely compiles or contains placeholder screens.
