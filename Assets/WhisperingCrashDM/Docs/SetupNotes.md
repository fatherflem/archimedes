# Whispering Crash DM Cockpit — Setup Notes

## Repository / project inspection

This repository was not a complete Unity project when this MVP was added. It contained the existing browser game files (`README.md`, `index.html`, `game.js`, `styles.css`) and no `Assets/`, `Packages/manifest.json`, or `ProjectSettings/ProjectVersion.txt` Unity metadata. No Meta XR SDK, Oculus Integration, OpenXR, XR Interaction Toolkit, MRUK, or spatial anchor package could be detected in the repo.

Because the XR/Meta packages are absent, this MVP intentionally implements the data model, runtime state, generated world-space UI, dice roller, sound cue system, editor-friendly input, and safe MR abstraction first. Passthrough and persistent spatial anchors are documented integration steps rather than hard dependencies.

## How to run in the Unity Editor

1. Open this repository with Unity (recommended: a current Unity 2022 LTS or Unity 6 LTS version supported by your Meta XR SDK version).
2. Open `Assets/WhisperingCrashDM/Scenes/WhisperingCrash_DM_Cockpit.unity`.
3. Enter Play Mode.
4. The `WhisperingCrashAutoBootstrap` runtime initializer creates:
   - a Main Camera if the scene has none,
   - an EventSystem,
   - `DMRoot`,
   - AdventureManager,
   - DiceRoller,
   - SoundCueManager,
   - floating world-space panels.
5. Use the mouse to click UI buttons in Play Mode.
6. Use keyboard shortcuts:
   - `1`: toggle Castle Systems panel,
   - `2`: toggle Current Chamber panel,
   - `3`: toggle Xylos State panel,
   - `4`: toggle Fate Dice panel,
   - `5`: toggle Echoes & Cues panel,
   - `6`: toggle God Machine Forecast panel,
   - `R`: reset panel positions.

## How to build toward Quest 3

Manual Unity setup is required because this repo did not include Unity project settings or XR packages.

1. Install Android Build Support for your Unity version.
2. In Unity, switch platform to Android.
3. Install the official Meta XR SDK package appropriate for your Unity version, or configure OpenXR with Meta Quest support.
4. Set Player settings for Quest:
   - package name / version,
   - minimum Android API level required by your Meta SDK,
   - ARM64 target architecture,
   - XR plug-in provider recommended by your chosen Meta setup.
5. Add or replace the generated camera bootstrap with the project-standard Meta XR camera rig if desired.
6. Enable Quest passthrough using the official Meta XR components for your installed SDK version.
7. If using controller ray input, add the Meta / XR Interaction ray interactor modules to drive Unity UI canvases.

## Implemented in this MVP

- Data-driven scene model with `AdventureSceneData`, `DCEntry`, `MonsterReference`, `SceneChoice`, enums, and `SoundCueData`.
- Runtime sample adventure content for all requested Xylos / Castlemania scenes.
- Adventure state with clamped infection, castle integrity, NPC trust, psychic pressure, elemental resolution flags, final modifiers, and adventure log.
- `AdventureManager` with scene switching, choices, purify/absorb handling, double-count prevention, final battle summary generation, and JSON save/load.
- World-space generated panels:
  - Castle Systems,
  - Current Chamber,
  - Xylos State,
  - Fate Dice,
  - Echoes & Cues,
  - God Machine Forecast.
- Dice roller with normal dice, Xylos Intrusion Check, Psychic Backlash, Castle Integrity Check, Secret DM Roll, and roll history.
- Sound cue manager with placeholder cue data and graceful "No clip assigned" notices.
- Panel placement around the current camera and editor keyboard toggles.
- `WorldLockingService` abstraction so spatial anchor persistence can be added later.
- `PassthroughSetupHelper` that probes for Meta passthrough classes without taking a compile-time dependency on absent SDK APIs.

## Manual Quest / Meta setup still needed

- Install and configure a real Meta XR SDK or OpenXR Quest setup.
- Replace or augment the fallback camera with a Meta-compatible rig if the project standard requires it.
- Configure passthrough using the exact components from the installed Meta XR SDK version.
- Configure controller ray interaction against Unity world-space canvases.
- Optionally connect `WorldLockingService` to Meta spatial anchors or MRUK once those packages are installed.

## Intentional MVP limitations

- No multiplayer or shared anchors.
- No full battle map.
- No player-facing UI.
- No character sheets or D&D Beyond integration.
- No voice changer.
- No persistent spatial anchors yet.
- Sound cue assets are placeholders until real AudioClips are assigned.
