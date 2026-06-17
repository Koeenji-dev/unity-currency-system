# Setup

This guide explains how to open, run and validate Unity Currency System v1.0 from a clean clone.

---

## Requirements

- Unity `6000.3.17f1`
- Universal 2D template / URP 17.3.0
- Unity Input System `1.19.0`
- Unity Test Framework `1.6.0`
- TextMeshPro / uGUI
- Git (to clone the repository)

---

## Clone the Repository

```bash
git clone <repository-url>
cd unity-currency-system
```

Replace `<repository-url>` with the actual repository URL.

---

## Open the Project

1. Open Unity Hub.
2. Click **Add project from disk**.
3. Select the repository root folder.
4. Open the project with Unity `6000.3.17f1`.
5. Wait for package resolution and initial asset import to complete.

`Library`, `Temp`, `Logs` and solution files are regenerated locally on first open and are not included in the repository.

---

## Open the Demo Scene

Open the scene before entering Play Mode:

```text
Assets/_Project/Scenes/CurrencyDemo.unity
```

---

## Run the Demo

1. Press **Play**.
2. Click the **Game** view if keyboard input is not focused.
3. Move using **WASD** or the **arrow keys**.
4. Collect all five pickups.
5. Verify the HUD reaches `22`.
6. Verify the pickup sound plays on each collection.
7. Verify no errors appear in the Console.

---

## Run Automated Tests

1. Open **Window → General → Test Runner**.
2. Select the **EditMode** tab.
3. Click **Run All**.

Expected result:

```
9 tests
9 passed
0 failed
```

The tests cover the approved v1.0 `CurrencyWallet` behavior, including validation, event behavior and integer overflow protection.

---

## Scene References

The following references are already configured in the demo scene. They should not need to be reassigned to run the demo normally.

| Component | Reference |
|---|---|
| `PlayerMovement2D` | `Rigidbody2D` on the Player |
| `CurrencyCollector` | `CurrencyWallet` on the Player |
| `PlayerInput` | `PlayerControls.inputactions` |
| `CurrencyDisplay` | `CurrencyWallet` on the Player |
| `CurrencyDisplay` | `BalanceText` TMP object |
| Pickup prefabs | Optional `AudioClip` (collection sound) |
| `CoinVisualSpin` | `Coin` child Transform |

---

## Input Configuration

The input actions asset is located at:

```text
Assets/_Project/Demo/Input/PlayerControls.inputactions
```

Configuration:

```
Action Map: Player
Action:     Move
Bindings:   WASD, Arrow Keys
```

---

## Common Setup Problems

### No movement

- Verify the **Game** view is focused (click inside it).
- Verify `PlayerInput` is enabled on the Player.
- Verify the `Actions` field points to `PlayerControls`.
- Verify `Default Map` is set to `Player`.
- Verify the `Move` event is connected to `PlayerMovement2D.OnMove`.

### HUD does not update

- Verify `CurrencyDisplay` has a wallet reference assigned.
- Verify the `Balance Text` field points to the correct TMP object.
- Check the Console for errors reported at startup.

### Pickup does not collect

- Verify the pickup has a `CircleCollider2D` with **Is Trigger** enabled.
- Verify the Player has a `CurrencyCollector` component.
- Verify the collector has a `CurrencyWallet` assigned.
- Verify a `Rigidbody2D` is present on the Player for physics interaction.

### Coin visual does not animate

- Verify `CoinVisualSpin` is enabled on the pickup's `Visual` child.
- Verify the `target` field points to the `Coin` child Transform.
- Verify the imported `Animator` component remains disabled; `CoinVisualSpin` replaces it.

### Tests are not visible in Test Runner

- Verify Test Runner is set to **EditMode**.
- Verify `Tests.asmdef` exists at `Assets/_Project/Systems/Currency/Tests/EditMode/`.
- Verify the `Tests` assembly references `KoeenjiDev.CurrencySystem`.
- Verify the Editor platform configuration is enabled in the assembly definition settings.

---

## Project Reset Behavior

- Balance starts at `0` each time Play Mode begins.
- Restarting Play Mode fully resets the balance and restores all pickups.
- No persistence is included in v1.0; all state is session-only.

---

## Build Notes

This project is validated as an Editor demo. No public build is currently included in the repository. Standalone build configuration is outside the scope of v1.0.
