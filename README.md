# Archimedes: Siege of Syracuse

A browser-based 3D game built with **HTML5 + Three.js** where you defend Syracuse from the Roman fleet during the Punic Wars.

## What’s in this version

- Play as Archimedes atop Syracuse's walls.
- Fire **solar death rays** to burn Roman ships.
- Use the **Claw of Archimedes** to snatch and fling ships.
- Fight upgraded, more detailed Roman warships (ram, mast, sail, oars, crest, wakes).
- Optional **arm-control mode** using your webcam + pose tracking.

## How to run it

> You must run from a local server (not `file://`) because modules and webcam APIs need it.

### Option A (Python)

```bash
python3 -m http.server 4173
```

Open:

- <http://localhost:4173>

### Option B (Node)

```bash
npx serve -l 4173 .
```

Open:

- <http://localhost:4173>

## Controls

- `A` / `D`: Move Archimedes along the wall.
- Mouse: Aim death ray.
- Hold left mouse: Fire death ray.
- `Space`: Trigger giant claw.
- `M` or the HUD button: Toggle webcam arm-control mode.

## Arm-control mode (webcam)

When arm mode is enabled:

- Raise both arms above shoulder level to fire the death ray.
- Spread arms wide to trigger the claw (when off cooldown).
- Browser will ask for camera permission.
- If webcam fails, switch back to mouse mode with `M`.
