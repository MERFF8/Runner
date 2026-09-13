# Runner
Testing task. Time: 4.5 hours total

A small 3D arcade game developed as a Unity test task.

## Gameplay

The player controls a turret mounted on a moving car.

* Tap to start the car.
* Aim and shoot enemies with the turret.
* Enemies attack the car when it gets close to them.
* Destroy as many enemies as possible while keeping the car alive.
* Reach the end of the level to win.

### Win Condition

Reach the end of the level without losing all car HP.

### Lose Condition

The car loses all HP.

After winning or losing, the corresponding screen is displayed and the level can be restarted.

## Features

* Moving vehicle with a mounted turret
* Manual turret aiming
* Enemy spawning and AI behaviour
* Enemy health and damage system
* Car health system
* Shooting and hit effects
* Enemy death animations
* Camera follow
* Basic game win/lose states
* Particle effects and game feel improvements

## Controls

**Mobile / Touch**

* Tap the screen to start the vehicle.
* Drag/touch to control the turret aiming.

**Editor**

* Mouse input can be used for testing.

## Technical Details

* **Engine:** Unity 6000.0.*
* **Language:** C#
* **Platform:** Android
* **Rendering:** 3D
* **Animation:** Unity Animator / Humanoid animations
* **Version Control:** Git

## Project Structure

```text
Assets/
├── Animations/
├── Art/
├── Audio/
├── Materials/
├── Prefabs/
├── Scenes/
├── Scripts/
│   ├── Player/
│   ├── Enemy/
│   ├── Combat/
│   └── Game/
└── VFX/
```

The project follows a component-based approach. Gameplay responsibilities are separated into small, focused components to keep the code easy to maintain and extend.

## How to Run

1. Clone the repository.
2. Open the project with **Unity 6000.0.***.
3. Open the main scene from:

```text
Assets/Scenes/
```

4. Press **Play**.

## Build

The project is intended for Android.

To create a build:

`File → Build Profiles → Android → Build`

## Notes

The project focuses on the core gameplay mechanic, clean code structure and game feel rather than reproducing the reference game in full.

## Author

**MERFF**
