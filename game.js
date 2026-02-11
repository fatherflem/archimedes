import * as THREE from "https://unpkg.com/three@0.164.1/build/three.module.js";

const canvas = document.querySelector("#game");
const stats = document.querySelector("#stats");
const overlay = document.querySelector("#overlay");
const message = document.querySelector("#message");
const restartBtn = document.querySelector("#restart");

const renderer = new THREE.WebGLRenderer({ canvas, antialias: true });
renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
renderer.setSize(window.innerWidth, window.innerHeight);
renderer.shadowMap.enabled = true;

const scene = new THREE.Scene();
scene.background = new THREE.Color("#73b8f5");
scene.fog = new THREE.Fog("#8dc9ff", 55, 220);

const camera = new THREE.PerspectiveCamera(62, window.innerWidth / window.innerHeight, 0.1, 500);
camera.position.set(0, 16, 26);

const sun = new THREE.DirectionalLight("#fff5b1", 2.2);
sun.position.set(36, 50, 20);
sun.castShadow = true;
scene.add(sun);
scene.add(new THREE.AmbientLight("#8bbce6", 0.6));

const water = new THREE.Mesh(
  new THREE.PlaneGeometry(600, 600, 100, 100),
  new THREE.MeshStandardMaterial({ color: "#2f68a0", roughness: 0.35, metalness: 0.12 })
);
water.rotation.x = -Math.PI / 2;
water.position.y = -1.3;
scene.add(water);

const cityWall = new THREE.Mesh(
  new THREE.BoxGeometry(80, 10, 6),
  new THREE.MeshStandardMaterial({ color: "#b99d72", roughness: 0.92 })
);
cityWall.position.set(0, 4, -3);
cityWall.castShadow = true;
cityWall.receiveShadow = true;
scene.add(cityWall);

const mirrorRig = new THREE.Group();
mirrorRig.position.set(0, 8, 0);
scene.add(mirrorRig);

const engineer = new THREE.Mesh(
  new THREE.CapsuleGeometry(0.7, 1.4, 6, 10),
  new THREE.MeshStandardMaterial({ color: "#5e3f26", roughness: 0.75 })
);
engineer.position.y = 0.9;
mirrorRig.add(engineer);

const mirror = new THREE.Mesh(
  new THREE.CircleGeometry(1.3, 24),
  new THREE.MeshStandardMaterial({ color: "#f8edc2", emissive: "#d5b24d", emissiveIntensity: 0.45, metalness: 0.8, roughness: 0.25 })
);
mirror.rotation.y = Math.PI;
mirror.position.set(0, 1.3, 0.7);
mirrorRig.add(mirror);

const clawBase = new THREE.Group();
clawBase.position.set(-14, 8, 1);
scene.add(clawBase);
const arm = new THREE.Mesh(
  new THREE.BoxGeometry(0.7, 0.7, 6),
  new THREE.MeshStandardMaterial({ color: "#6c4e34" })
);
arm.position.z = 3;
clawBase.add(arm);

const claw = new THREE.Mesh(
  new THREE.TorusGeometry(1.1, 0.22, 14, 22, Math.PI * 1.7),
  new THREE.MeshStandardMaterial({ color: "#b9c4d6", metalness: 0.75, roughness: 0.2 })
);
claw.rotation.x = Math.PI / 2;
claw.position.z = 6.6;
clawBase.add(claw);

const seaLine = new THREE.Vector3(0, 0, 1);
const pointer = new THREE.Vector2();
const keys = new Set();
const raycaster = new THREE.Raycaster();
const clock = new THREE.Clock();

const laserBeam = new THREE.Mesh(
  new THREE.CylinderGeometry(0.08, 0.18, 1, 16),
  new THREE.MeshBasicMaterial({ color: "#ffdb5e", transparent: true, opacity: 0.88 })
);
laserBeam.visible = false;
scene.add(laserBeam);

let cityIntegrity = 100;
let score = 0;
let gameOver = false;
let firePressed = false;
let clawCooldown = 0;
let spawnTimer = 0;
let nextSpawn = 1.6;

const enemyFleet = [];
const sparks = [];

function makeEnemy() {
  const hull = new THREE.Mesh(
    new THREE.BoxGeometry(2.3, 1.2, 7),
    new THREE.MeshStandardMaterial({ color: "#6a2f1e", roughness: 0.8 })
  );
  hull.castShadow = true;
  hull.receiveShadow = true;

  const sail = new THREE.Mesh(
    new THREE.PlaneGeometry(2.2, 2.4),
    new THREE.MeshStandardMaterial({ color: "#ece8d8", side: THREE.DoubleSide, roughness: 0.9 })
  );
  sail.position.set(0, 2.2, 0);
  hull.add(sail);

  hull.position.set(THREE.MathUtils.randFloatSpread(68), -0.3, THREE.MathUtils.randFloat(66, 130));
  scene.add(hull);

  enemyFleet.push({ mesh: hull, hp: 100, speed: THREE.MathUtils.randFloat(4, 6.6), burning: 0 });
}

function spawnSpark(position, tint = "#ffd36a") {
  const sprite = new THREE.Mesh(
    new THREE.SphereGeometry(0.12, 6, 6),
    new THREE.MeshBasicMaterial({ color: tint, transparent: true })
  );
  sprite.position.copy(position);
  sprite.position.y += 1.4;
  scene.add(sprite);
  sparks.push({ mesh: sprite, life: 0.8 + Math.random() * 0.5, vy: 1 + Math.random() * 1.2 });
}

function removeEnemy(enemy) {
  scene.remove(enemy.mesh);
  const idx = enemyFleet.indexOf(enemy);
  if (idx >= 0) {
    enemyFleet.splice(idx, 1);
  }
}

function endGame(text) {
  gameOver = true;
  overlay.classList.add("visible");
  message.textContent = text;
  laserBeam.visible = false;
}

function restartGame() {
  for (const enemy of [...enemyFleet]) {
    removeEnemy(enemy);
  }
  for (const spark of [...sparks]) {
    scene.remove(spark.mesh);
  }
  sparks.length = 0;

  cityIntegrity = 100;
  score = 0;
  gameOver = false;
  firePressed = false;
  clawCooldown = 0;
  spawnTimer = 0;
  nextSpawn = 1.6;
  mirrorRig.position.x = 0;
  overlay.classList.remove("visible");
}

function updateHUD() {
  stats.textContent = `City Integrity: ${Math.max(0, cityIntegrity).toFixed(0)}% | Score: ${score} | Enemy Ships: ${enemyFleet.length} | Claw Cooldown: ${Math.max(0, clawCooldown).toFixed(1)}s`;
}

function applySolarRay(dt) {
  if (!firePressed || enemyFleet.length === 0 || gameOver) {
    laserBeam.visible = false;
    return;
  }

  raycaster.setFromCamera(pointer, camera);
  const intersects = raycaster.intersectObjects(enemyFleet.map((e) => e.mesh), false);
  if (intersects.length === 0) {
    laserBeam.visible = false;
    return;
  }

  const hit = intersects[0];
  const enemy = enemyFleet.find((e) => e.mesh === hit.object);
  if (!enemy) {
    laserBeam.visible = false;
    return;
  }

  const burn = 28 * dt;
  enemy.hp -= burn;
  enemy.burning = 0.5;

  if (Math.random() < 0.65) {
    spawnSpark(hit.point, "#ffca59");
  }

  const origin = mirror.getWorldPosition(new THREE.Vector3());
  const end = hit.point.clone();
  const direction = end.clone().sub(origin);
  const length = direction.length();
  laserBeam.visible = true;
  laserBeam.position.copy(origin).addScaledVector(direction.normalize(), length / 2);
  laserBeam.scale.set(1, length, 1);
  laserBeam.quaternion.setFromUnitVectors(seaLine, direction.normalize());

  if (enemy.hp <= 0) {
    score += 25;
    for (let i = 0; i < 14; i += 1) {
      spawnSpark(enemy.mesh.position, "#ff8e55");
    }
    removeEnemy(enemy);
  }
}

function triggerClaw() {
  if (clawCooldown > 0 || gameOver) {
    return;
  }

  let best = null;
  let bestDistance = 13;
  for (const enemy of enemyFleet) {
    const dist = enemy.mesh.position.distanceTo(claw.getWorldPosition(new THREE.Vector3()));
    if (dist < bestDistance) {
      bestDistance = dist;
      best = enemy;
    }
  }

  clawCooldown = 5;
  if (!best) {
    return;
  }

  best.mesh.position.z += 14;
  best.mesh.position.y += 4;
  best.hp -= 90;
  for (let i = 0; i < 9; i += 1) {
    spawnSpark(best.mesh.position, "#a8e8ff");
  }

  if (best.hp <= 0) {
    score += 18;
    removeEnemy(best);
  }
}

function animate() {
  const dt = Math.min(clock.getDelta(), 0.04);
  const elapsed = clock.elapsedTime;

  if (!gameOver) {
    spawnTimer += dt;
    if (spawnTimer > nextSpawn) {
      spawnTimer = 0;
      nextSpawn = Math.max(0.45, 1.6 - score / 180);
      makeEnemy();
    }

    const move = (keys.has("KeyD") ? 1 : 0) - (keys.has("KeyA") ? 1 : 0);
    mirrorRig.position.x = THREE.MathUtils.clamp(mirrorRig.position.x + move * dt * 11, -33, 33);
    mirrorRig.position.y = 8 + Math.sin(elapsed * 4) * 0.1;

    clawBase.rotation.y = Math.sin(elapsed * 2.5) * 0.08;
    claw.rotation.z = Math.sin(elapsed * 7) * 0.15;

    clawCooldown -= dt;

    for (const enemy of [...enemyFleet]) {
      enemy.mesh.position.z -= enemy.speed * dt;
      enemy.mesh.position.y = -0.3 + Math.sin(elapsed * 2 + enemy.mesh.position.x) * 0.15;
      enemy.mesh.rotation.y = Math.sin(elapsed + enemy.mesh.position.x) * 0.07;

      if (enemy.burning > 0) {
        enemy.burning -= dt;
        enemy.mesh.material.emissive = new THREE.Color("#ff8d48");
        enemy.mesh.material.emissiveIntensity = 0.48;
      } else {
        enemy.mesh.material.emissiveIntensity = 0;
      }

      if (enemy.mesh.position.z < 1.5) {
        cityIntegrity -= 16;
        for (let i = 0; i < 7; i += 1) {
          spawnSpark(enemy.mesh.position, "#ff6961");
        }
        removeEnemy(enemy);
      }
    }

    if (cityIntegrity <= 0) {
      endGame("The Romans breached Syracuse. Archimedes vows a smarter defense!");
    }

    if (score >= 300) {
      endGame("Victory! Syracuse stands—your mirrors and claws scattered the Roman fleet.");
    }
  }

  applySolarRay(dt);

  for (const spark of [...sparks]) {
    spark.life -= dt;
    spark.mesh.position.y += spark.vy * dt;
    spark.mesh.material.opacity = Math.max(0, spark.life);
    if (spark.life <= 0) {
      scene.remove(spark.mesh);
      sparks.splice(sparks.indexOf(spark), 1);
    }
  }

  water.material.emissive = new THREE.Color("#0c2e55");
  water.material.emissiveIntensity = 0.06 + Math.sin(elapsed * 2.2) * 0.03;

  camera.position.x += (mirrorRig.position.x * 0.35 - camera.position.x) * 3 * dt;
  camera.position.y = 16 + Math.sin(elapsed * 0.8) * 0.3;
  camera.lookAt(mirrorRig.position.x * 0.7, 6, 24);

  updateHUD();
  renderer.render(scene, camera);
  requestAnimationFrame(animate);
}

window.addEventListener("resize", () => {
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  renderer.setSize(window.innerWidth, window.innerHeight);
});

window.addEventListener("pointermove", (event) => {
  pointer.x = (event.clientX / window.innerWidth) * 2 - 1;
  pointer.y = -(event.clientY / window.innerHeight) * 2 + 1;
});

window.addEventListener("pointerdown", (event) => {
  if (event.button === 0) {
    firePressed = true;
  }
});

window.addEventListener("pointerup", (event) => {
  if (event.button === 0) {
    firePressed = false;
  }
});

window.addEventListener("keydown", (event) => {
  keys.add(event.code);
  if (event.code === "Space") {
    triggerClaw();
  }
});

window.addEventListener("keyup", (event) => {
  keys.delete(event.code);
});

restartBtn.addEventListener("click", restartGame);

restartGame();
animate();
