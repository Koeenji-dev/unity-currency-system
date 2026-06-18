# Unity Currency System

A reusable 2D currency collection system built with Unity 6, designed as part of the Koeenji Dev Unity Systems Portfolio.

---

## Overview

This project demonstrates a self-contained currency collection system for a 2D top-down context. It covers:

- collectible currency pickups with values of `1`, `5` and `10`;
- wallet-based balance storage;
- event-driven UI updates;
- duplicate collection protection;
- optional pickup sound;
- lightweight visual spin effect;
- automated Edit Mode tests.

---

## Demo

The project includes a top-down 2D demo scene where the player moves with the keyboard and collects currency pickups scattered around the level.

- Keyboard movement: WASD and arrow keys.
- Five currency pickups are placed in the scene.
- Total available balance: `22`.
- The HUD updates after each valid collection.

> Demo video and screenshots will be added after the final portfolio recording.

---

## Portfolio & Media

| Resource | Description |
|---|---|
| [`Documentation/MEDIA_GUIDE.md`](Documentation/MEDIA_GUIDE.md) | Screenshot and GIF specifications, OBS checklist, file naming convention and final media checklist. |
| [`Documentation/VIDEO_SCRIPT.es.md`](Documentation/VIDEO_SCRIPT.es.md) | Full Spanish-language video script with section structure, key phrases and OBS shot list. |

---

## Features

- `CurrencyWallet` — stores balance, validates positive amounts, protects against integer overflow, emits `BalanceChanged`.
- `CurrencyCollector` — holds the explicit wallet reference; acts as the pickup receiver entry point.
- `CurrencyPickup` — configurable pickup value; destroys itself only after a successful addition; prevents duplicate collection.
- `CurrencyDisplay` — subscribes to `BalanceChanged` in `OnEnable`; unsubscribes in `OnDisable`; displays current balance immediately on enable.
- New Unity Input System via `PlayerControls.inputactions`.
- Reusable Player prefab.
- `CoinVisualSpin` — custom lightweight scale-based visual effect for coin pickups.
- 9 automated Edit Mode tests for wallet logic.

---

## Architecture

```text
CurrencyPickup
→ CurrencyCollector
→ CurrencyWallet
→ BalanceChanged
→ CurrencyDisplay
```

### Component responsibilities

**CurrencyWallet**
Stores the current balance. Accepts valid positive amounts, rejects zero and negative values, and protects against `int` overflow. Exposes `Balance` as read-only and emits `BalanceChanged` with the updated total after each valid addition.

**CurrencyCollector**
Holds a serialized reference to `CurrencyWallet` and exposes it as read-only. Serves as the entry point that pickup objects look for on the collecting entity. Does not store balance or interact with UI.

**CurrencyPickup**
Contains a configurable positive value. Detects a `CurrencyCollector` on the entering collider using `GetComponentInParent`. Calls `TryAdd` on the wallet and destroys itself only if the addition succeeds. Disables its collider immediately upon collection to prevent duplicate triggers.

**CurrencyDisplay**
Receives explicit references to a `CurrencyWallet` and a `TMP_Text` component. Subscribes to `BalanceChanged` when enabled and unsubscribes when disabled. Displays the current balance immediately on enable. Never polls in `Update`.

**PlayerMovement2D**
Reads `Vector2` input from the new Input System and applies movement to a `Rigidbody2D` in `FixedUpdate`. Remains fully independent from the currency system.

**CoinVisualSpin**
Animates a target `Transform` by oscillating its X scale using `Mathf.Cos`, producing a lightweight visual spin effect. Has no knowledge of the currency system, colliders, audio or UI.

Demo code and visual presentation are kept separate from the economic logic in both namespace and folder structure.

---

## Project Structure

```text
Assets/_Project/
├── Audio/
├── Demo/
│   ├── Input/
│   ├── Prefabs/
│   └── Runtime/
├── Scenes/
└── Systems/
    └── Currency/
        ├── Prefabs/
        ├── Runtime/
        ├── Tests/
        └── UI/
```

Third-party visual assets are located under:

```text
Assets/ThirdParty/2DRPK/
```

---

## Requirements

- Unity `6000.3.17f1`
- Universal 2D template / URP 17.3.0
- Unity Input System `1.19.0`
- Unity Test Framework `1.6.0`
- TextMeshPro / uGUI

---

## Controls

```
WASD        Move
Arrow Keys  Move
```

---

## Testing

Edit Mode tests for `CurrencyWallet`:

```
9 tests
9 passed
0 failed
```

Cases covered:

- initial balance is zero;
- positive amount increases balance and returns `true`;
- multiple valid amounts accumulate correctly;
- zero is rejected and returns `false`;
- negative amounts are rejected and return `false`;
- valid additions raise `BalanceChanged` exactly once;
- invalid additions do not raise `BalanceChanged`;
- successive events emit the current accumulated balance;
- additions that would cause `int` overflow are rejected.

---

## Design Decisions

- **No singleton.** All dependencies are injected through explicit serialized references in the Inspector.
- **No global manager.** Each component holds only the references it needs.
- **No ScriptableObjects in v1.0.** The wallet is a plain `MonoBehaviour`.
- **No persistence.** Balance resets on Play Mode restart by design.
- **Event-driven UI.** `CurrencyDisplay` reacts to `BalanceChanged`; it never polls in `Update`.
- **Minimal Assembly Definitions.** Only two are used: one for runtime isolation (`KoeenjiDev.CurrencySystem`) and one for Edit Mode tests (`Tests`).
- **No global searches.** `GetComponentInParent` is used for local discovery; `GameObject.Find` and `FindFirstObjectByType` are not used.
- **New Input System only.** The old Input Manager is not referenced anywhere in the project.

---

## Third-Party Assets

### 2D Animated Coin — 2D RPK

The coin sprite used for pickup presentation comes from the 2D RPK asset.

- Used only for visual presentation of the coin pickup.
- The original `Animator` component remains disabled because the imported animation did not work correctly in the current Unity version.
- Visual animation is handled by `CoinVisualSpin` instead.

### Coin pickup sound

An external audio clip is used as the optional collection sound.

- Played through `AudioSource.PlayClipAtPoint` at the pickup position.
- Collection works correctly if no clip is assigned.

Third-party assets remain subject to their respective licenses and terms.

---

## Current Scope

**Implemented:**

- currency collection;
- balance management;
- event-driven HUD;
- optional pickup sound;
- visual spin feedback;
- automated wallet tests.

**Out of scope for v1.0:**

- spending currency;
- shops;
- save and load;
- cross-scene persistence;
- multiplayer;
- object pooling;
- magnetic pickups;
- global audio management.

---

## Future Improvements

- currency spending;
- cross-scene persistence;
- ScriptableObject-based currency definitions;
- object pooling for larger scenes;
- improved visual effects;
- gamepad support after proper testing;
- extraction as a reusable Unity package.

---

## Spanish Version

A Spanish version of this README is available in `README.es.md`.
