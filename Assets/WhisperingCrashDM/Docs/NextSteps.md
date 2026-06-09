# Whispering Crash DM Cockpit — Recommended Next Steps

## Panel dragging

Add a lightweight grab/reposition component for each panel. In Editor it can use mouse drag on a title bar; on Quest it should use the installed Meta / XR ray interactor select events. Keep this independent from the panel UI scripts so each panel remains data-driven.

## Saved spatial anchors

Extend `WorldLockingService` with a Meta implementation after installing Meta XR SDK / MRUK. Save each panel transform by stable panel ID, then restore positions after room scanning. Keep the editor fallback active so the cockpit remains testable without a headset.

## Real audio clips

Create `SoundCueData` ScriptableObject assets under `Assets/WhisperingCrashDM/ScriptableObjects/SoundCues/` and assign local `AudioClip` assets for each placeholder cue. Use short non-looping stingers for elemental events and subtle looping ambiences for Castle Groan, Xylos Pulse, and God Machine Drone.

## Optional voice changer integration

If a future live Echo / Xylos voice effect is desired, implement it as an optional local audio filter chain or a manually triggered clip system. Do not make it required for the DM cockpit core loop.

## Optional player-facing mode

A later player-facing mode could show only read-aloud text, atmospheric images, and public state. Keep it completely separate from the DM-only panels so secrets and monster notes cannot leak.

## Optional shared anchors / multiplayer

If multiple headsets are ever supported, use shared spatial anchors only after the DM cockpit is stable. Networking should synchronize public effects only; the DM state panel should remain private by default.

## Adventure authoring

Add an editor utility to convert the runtime sample data into real ScriptableObject assets. That will make chamber text and sound cue assignments easier for non-programmers to edit inside Unity.
