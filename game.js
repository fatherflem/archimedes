import * as THREE from "https://unpkg.com/three@0.164.1/build/three.module.js";

const canvas = document.querySelector("#game");
const stats = document.querySelector("#stats");
const overlay = document.querySelector("#overlay");
const message = document.querySelector("#message");
const restartBtn = document.querySelector("#restart");
const modeToggle = document.querySelector("#modeToggle");
const modeLabel = document.querySelector("#modeLabel");
const webcamStatus = document.querySelector("#webcamStatus");
const webcamPanel = document.querySelector("#webcamPanel");
const webcamVideo = document.querySelector("#webcam");
const poseOverlay = document.querySelector("#poseOverlay");
const poseCtx = poseOverlay.getContext("2d");

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
scene.add(new THREE.AmbientLight("#8bbce6", 0.65));

const water = new THREE.Mesh(
  new THREE.PlaneGeometry(600, 600, 100, 100),
  new THREE.MeshStandardMaterial({ color: "#2f68a0", roughness: 0.33, metalness: 0.15 })
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
let nextSpawn = 1.4;
let controlMode = "mouse";

let poseController = null;
let cameraFeed = null;
const armState = {
  aimX: 0,
  fire: false,
  clawArmed: false,
};

const enemyFleet = [];
const sparks = [];

function makeRomanShip() {
  const ship = new THREE.Group();

  const hullColor = new THREE.Color().setHSL(THREE.MathUtils.randFloat(0.02, 0.08), 0.65, THREE.MathUtils.randFloat(0.2, 0.35));
  const stripeColor = new THREE.Color().setHSL(THREE.MathUtils.randFloat(0.02, 0.12), 0.75, THREE.MathUtils.randFloat(0.45, 0.58));

  const hull = new THREE.Mesh(
    new THREE.BoxGeometry(2.5, 1.1, 8.2),
    new THREE.MeshStandardMaterial({ color: hullColor, roughness: 0.74, metalness: 0.1 })
  );
  hull.castShadow = true;
  hull.receiveShadow = true;
  ship.add(hull);

  const bow = new THREE.Mesh(
    new THREE.ConeGeometry(1.25, 2.5, 8),
    new THREE.MeshStandardMaterial({ color: hullColor, roughness: 0.72 })
  );
  bow.rotation.x = Math.PI / 2;
  bow.position.z = 5.2;
  bow.position.y = -0.05;
  ship.add(bow);

  const ram = new THREE.Mesh(
    new THREE.ConeGeometry(0.25, 1.4, 10),
    new THREE.MeshStandardMaterial({ color: "#b1bccf", metalness: 0.9, roughness: 0.2 })
  );
  ram.rotation.x = Math.PI / 2;
  ram.position.set(0, -0.1, 6.3);
  ship.add(ram);

  const trim = new THREE.Mesh(
    new THREE.BoxGeometry(2.55, 0.15, 8.4),
    new THREE.MeshStandardMaterial({ color: stripeColor, roughness: 0.8 })
  );
  trim.position.y = 0.62;
  ship.add(trim);

  for (let i = -1; i <= 1; i += 2) {
    for (let j = -3; j <= 3; j += 1) {
      const oar = new THREE.Mesh(
        new THREE.CylinderGeometry(0.03, 0.03, 1.5, 6),
        new THREE.MeshStandardMaterial({ color: "#a77547", roughness: 0.9 })
      );
      oar.rotation.z = Math.PI / 2;
      oar.rotation.y = i > 0 ? 0.35 : -0.35;
      oar.position.set(i * 1.35, 0.3, j * 1.15);
      ship.add(oar);
    }
  }

  const mast = new THREE.Mesh(
    new THREE.CylinderGeometry(0.12, 0.15, 3.8, 10),
    new THREE.MeshStandardMaterial({ color: "#725235", roughness: 0.9 })
  );
  mast.position.y = 2.1;
  ship.add(mast);

  const sail = new THREE.Mesh(
    new THREE.PlaneGeometry(2.5, 2.3),
    new THREE.MeshStandardMaterial({ color: "#eae2cd", side: THREE.DoubleSide, roughness: 0.95 })
  );
  sail.position.set(0, 2.4, 0);
  ship.add(sail);

  const crest = new THREE.Mesh(
    new THREE.PlaneGeometry(1.1, 0.9),
    new THREE.MeshStandardMaterial({ color: "#a2261f", side: THREE.DoubleSide, roughness: 0.8 })
  );
  crest.position.set(0, 3.4, 0);
  crest.rotation.y = Math.PI / 2;
  ship.add(crest);

  ship.position.set(THREE.MathUtils.randFloatSpread(68), -0.3, THREE.MathUtils.randFloat(66, 130));
  scene.add(ship);

  enemyFleet.push({
    mesh: ship,
    hull,
    sail,
    hp: 130,
    speed: THREE.MathUtils.randFloat(3.8, 6.1),
    burning: 0,
    wakeTick: Math.random(),
  });
}

function spawnSpark(position, tint = "#ffd36a", scale = 1) {
  const sprite = new THREE.Mesh(
    new THREE.SphereGeometry(0.12 * scale, 6, 6),
    new THREE.MeshBasicMaterial({ color: tint, transparent: true })
  );
  sprite.position.copy(position);
  sprite.position.y += 1.4;
  scene.add(sprite);
  sparks.push({ mesh: sprite, life: 0.8 + Math.random() * 0.45, vy: 1 + Math.random() * 1.3 });
}

function removeEnemy(enemy) {
  scene.remove(enemy.mesh);
  const idx = enemyFleet.indexOf(enemy);
  if (idx >= 0) enemyFleet.splice(idx, 1);
}

function endGame(text) {
  gameOver = true;
  overlay.classList.add("visible");
  message.textContent = text;
  laserBeam.visible = false;
}

function restartGame() {
  for (const enemy of [...enemyFleet]) removeEnemy(enemy);
  for (const spark of [...sparks]) scene.remove(spark.mesh);
  sparks.length = 0;

  cityIntegrity = 100;
  score = 0;
  gameOver = false;
  firePressed = false;
  clawCooldown = 0;
  spawnTimer = 0;
  nextSpawn = 1.4;
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
  const intersects = raycaster.intersectObjects(enemyFleet.map((e) => e.mesh), true);
  if (intersects.length === 0) {
    laserBeam.visible = false;
    return;
  }

  const hit = intersects[0];
  const enemy = enemyFleet.find((e) => e.mesh === hit.object || e.mesh.children.includes(hit.object));
  if (!enemy) {
    laserBeam.visible = false;
    return;
  }

  enemy.hp -= 34 * dt;
  enemy.burning = 0.55;
  if (Math.random() < 0.68) spawnSpark(hit.point, "#ffca59", 1.1);

  const origin = mirror.getWorldPosition(new THREE.Vector3());
  const end = hit.point.clone();
  const direction = end.clone().sub(origin);
  const unit = direction.clone().normalize();
  const length = direction.length();
  laserBeam.visible = true;
  laserBeam.position.copy(origin).addScaledVector(unit, length / 2);
  laserBeam.scale.set(1, length, 1);
  laserBeam.quaternion.setFromUnitVectors(seaLine, unit);

  if (enemy.hp <= 0) {
    score += 30;
    for (let i = 0; i < 18; i += 1) spawnSpark(enemy.mesh.position, "#ff8e55", 1.4);
    removeEnemy(enemy);
  }
}

function triggerClaw() {
  if (clawCooldown > 0 || gameOver) return;

  let best = null;
  let bestDistance = 14;
  const clawTip = claw.getWorldPosition(new THREE.Vector3());
  for (const enemy of enemyFleet) {
    const dist = enemy.mesh.position.distanceTo(clawTip);
    if (dist < bestDistance) {
      bestDistance = dist;
      best = enemy;
    }
  }

  clawCooldown = 4.5;
  if (!best) return;

  best.mesh.position.z += 16;
  best.mesh.position.y += 4.2;
  best.hp -= 110;
  for (let i = 0; i < 12; i += 1) spawnSpark(best.mesh.position, "#a8e8ff", 1.15);

  if (best.hp <= 0) {
    score += 22;
    removeEnemy(best);
  }
}

function updateArmControl() {
  if (controlMode !== "arm") return;
  pointer.x += (armState.aimX - pointer.x) * 0.2;
  firePressed = armState.fire;
  if (armState.clawArmed) {
    triggerClaw();
    armState.clawArmed = false;
  }
}

async function toggleControlMode() {
  if (controlMode === "mouse") {
    await enableArmControl();
    return;
  }
  disableArmControl("Mouse mode active.");
}

function disableArmControl(status = "Arm control disabled.") {
  controlMode = "mouse";
  modeLabel.textContent = "Mouse";
  modeToggle.textContent = "Enable Arm Control";
  webcamStatus.textContent = status;
  webcamPanel.style.display = "none";
  firePressed = false;

  if (cameraFeed) {
    cameraFeed.stop();
    cameraFeed = null;
  }

  const stream = webcamVideo.srcObject;
  if (stream) {
    stream.getTracks().forEach((t) => t.stop());
    webcamVideo.srcObject = null;
  }
}

async function enableArmControl() {
  if (!window.Pose || !window.Camera) {
    webcamStatus.textContent = "Pose library unavailable in this browser.";
    return;
  }

  try {
    controlMode = "arm";
    modeLabel.textContent = "Arm / Webcam";
    modeToggle.textContent = "Disable Arm Control";
    webcamStatus.textContent = "Starting webcam tracking...";
    webcamPanel.style.display = "block";

    poseOverlay.width = webcamPanel.clientWidth;
    poseOverlay.height = webcamPanel.clientHeight;

    if (!poseController) {
      poseController = new window.Pose({
        locateFile: (file) => `https://cdn.jsdelivr.net/npm/@mediapipe/pose/${file}`,
      });

      poseController.setOptions({
        modelComplexity: 1,
        smoothLandmarks: true,
        minDetectionConfidence: 0.5,
        minTrackingConfidence: 0.5,
      });

      poseController.onResults((results) => {
        poseCtx.save();
        poseCtx.clearRect(0, 0, poseOverlay.width, poseOverlay.height);
        if (results.poseLandmarks && window.drawConnectors && window.drawLandmarks && window.POSE_CONNECTIONS) {
          window.drawConnectors(poseCtx, results.poseLandmarks, window.POSE_CONNECTIONS, { color: "#53d6ff", lineWidth: 2 });
          window.drawLandmarks(poseCtx, results.poseLandmarks, { color: "#ffe175", lineWidth: 1, radius: 3 });

          const leftShoulder = results.poseLandmarks[11];
          const rightShoulder = results.poseLandmarks[12];
          const leftWrist = results.poseLandmarks[15];
          const rightWrist = results.poseLandmarks[16];

          if (leftWrist && rightWrist && leftShoulder && rightShoulder) {
            const armCenter = (leftWrist.x + rightWrist.x) / 2;
            armState.aimX = THREE.MathUtils.clamp((0.5 - armCenter) * 2.2, -1, 1);

            const averageWristY = (leftWrist.y + rightWrist.y) / 2;
            const averageShoulderY = (leftShoulder.y + rightShoulder.y) / 2;
            armState.fire = averageWristY < averageShoulderY - 0.05;

            const handsWide = Math.abs(leftWrist.x - rightWrist.x) > 0.46;
            if (handsWide && clawCooldown <= 0.1) {
              armState.clawArmed = true;
            }
          }
          webcamStatus.textContent = "Arm tracking active: raise both arms to fire. Spread arms wide to trigger claw.";
        }
        poseCtx.restore();
      });
    }

    cameraFeed = new window.Camera(webcamVideo, {
      onFrame: async () => {
        await poseController.send({ image: webcamVideo });
      },
      width: 640,
      height: 360,
    });

    await cameraFeed.start();
  } catch (error) {
    disableArmControl(`Camera access failed: ${error.message}`);
  }
}

function animate() {
  const dt = Math.min(clock.getDelta(), 0.04);
  const elapsed = clock.elapsedTime;

  if (!gameOver) {
    updateArmControl();

    spawnTimer += dt;
    if (spawnTimer > nextSpawn) {
      spawnTimer = 0;
      nextSpawn = Math.max(0.42, 1.35 - score / 220);
      makeRomanShip();
    }

    const move = (keys.has("KeyD") ? 1 : 0) - (keys.has("KeyA") ? 1 : 0);
    mirrorRig.position.x = THREE.MathUtils.clamp(mirrorRig.position.x + move * dt * 11, -33, 33);
    mirrorRig.position.y = 8 + Math.sin(elapsed * 4) * 0.1;

    clawBase.rotation.y = Math.sin(elapsed * 2.5) * 0.08;
    claw.rotation.z = Math.sin(elapsed * 7) * 0.15;

    clawCooldown -= dt;

    for (const enemy of [...enemyFleet]) {
      enemy.mesh.position.z -= enemy.speed * dt;
      enemy.mesh.position.y = -0.32 + Math.sin(elapsed * 2 + enemy.mesh.position.x) * 0.15;
      enemy.mesh.rotation.y = Math.sin(elapsed + enemy.mesh.position.x) * 0.07;
      enemy.sail.rotation.y = Math.sin(elapsed * 2 + enemy.mesh.position.x) * 0.08;

      enemy.wakeTick += dt;
      if (enemy.wakeTick > 0.08) {
        enemy.wakeTick = 0;
        spawnSpark(enemy.mesh.position.clone().add(new THREE.Vector3(0, -0.8, -4.1)), "#9cd8ff", 0.75);
      }

      if (enemy.burning > 0) {
        enemy.burning -= dt;
        enemy.hull.material.emissive = new THREE.Color("#ff8d48");
        enemy.hull.material.emissiveIntensity = 0.52;
      } else {
        enemy.hull.material.emissiveIntensity = 0;
      }

      if (enemy.mesh.position.z < 1.5) {
        cityIntegrity -= 15;
        for (let i = 0; i < 8; i += 1) spawnSpark(enemy.mesh.position, "#ff6961", 1.2);
        removeEnemy(enemy);
      }
    }

    if (cityIntegrity <= 0) {
      endGame("The Romans breached Syracuse. Archimedes vows a smarter defense!");
    }

    if (score >= 340) {
      endGame("Victory! Syracuse stands—your mirrors and claws shattered the Roman fleet.");
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
  water.material.emissiveIntensity = 0.07 + Math.sin(elapsed * 2.2) * 0.03;

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
  if (controlMode === "arm") return;
  pointer.x = (event.clientX / window.innerWidth) * 2 - 1;
  pointer.y = -(event.clientY / window.innerHeight) * 2 + 1;
});

window.addEventListener("pointerdown", (event) => {
  if (controlMode === "mouse" && event.button === 0) firePressed = true;
});

window.addEventListener("pointerup", (event) => {
  if (event.button === 0) firePressed = false;
});

window.addEventListener("keydown", (event) => {
  keys.add(event.code);
  if (event.code === "Space") triggerClaw();
  if (event.code === "KeyM") toggleControlMode();
});

window.addEventListener("keyup", (event) => {
  keys.delete(event.code);
});

modeToggle.addEventListener("click", toggleControlMode);
restartBtn.addEventListener("click", restartGame);

restartGame();
animate();
