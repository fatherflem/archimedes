# Archimedes: Siege of Syracuse

A browser-based 3D mini-game built with **HTML5 + Three.js** where you defend Syracuse during the Punic Wars.

## Features

- Play as Archimedes atop the city walls.
- Use **solar death rays** (left mouse button) to ignite Roman ships.
- Use the legendary **Claw of Archimedes** (space bar) to grab and fling nearby ships.
- Survive until you reach victory score, or lose if city integrity drops to zero.

## Run locally

Because this project uses ES modules from a CDN, run it from a local web server:

```bash
python3 -m http.server 4173
```

Then open <http://localhost:4173>.

## Controls

- `A` / `D`: Move Archimedes along the wall.
- Mouse: Aim.
- Hold left mouse button: Fire solar ray.
- `Space`: Trigger giant claw (has cooldown).
