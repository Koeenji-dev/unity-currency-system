# Test Plan

Unity Currency System v1.0 validation is divided into three areas: automated Edit Mode tests, manual Play Mode validation and final acceptance criteria.

---

## Testing Goals

- Verify wallet balance behavior under valid and invalid operations.
- Verify invalid operations do not mutate state or emit events.
- Verify event emission contains the correct accumulated total.
- Verify integer overflow protection.
- Verify the full collection flow end-to-end in Play Mode.
- Verify UI, sound and visual feedback behave correctly.
- Verify the project remains free of errors and avoidable warnings.

---

## Automated Test Environment

- Unity Test Framework `1.6.0`
- Mode: Edit Mode
- Assembly: `Tests`
- Runtime reference: `KoeenjiDev.CurrencySystem`
- Test file:

```text
Assets/_Project/Systems/Currency/Tests/EditMode/CurrencyWalletTests.cs
```

Each test creates a `GameObject` with `CurrencyWallet` in `[SetUp]` and destroys it with `Object.DestroyImmediate` in `[TearDown]`.

---

## Automated Test Cases

### NewWallet_StartsWithZeroBalance

**Purpose:** Verify that a newly created wallet has a balance of zero before any operation.
**Expected result:** `wallet.Balance == 0`.

---

### TryAdd_PositiveAmount_IncreasesBalance

**Purpose:** Verify that a valid positive amount increases the balance and the operation reports success.
**Expected result:** `TryAdd(5)` returns `true`; `wallet.Balance == 5`.

---

### TryAdd_MultipleValidAmounts_AccumulatesBalance

**Purpose:** Verify that successive valid additions accumulate correctly.
**Expected result:** `TryAdd(5)` and `TryAdd(10)` both return `true`; `wallet.Balance == 15`.

---

### TryAdd_Zero_ReturnsFalse

**Purpose:** Verify that zero is rejected as an invalid amount.
**Expected result:** `TryAdd(0)` returns `false`.

---

### TryAdd_NegativeAmount_ReturnsFalse

**Purpose:** Verify that negative values are rejected.
**Expected result:** `TryAdd(-1)` returns `false`.

---

### TryAdd_ValidAmount_RaisesBalanceChanged

**Purpose:** Verify that a valid addition raises `BalanceChanged` exactly once.
**Expected result:** event fires once after `TryAdd(5)`.

---

### TryAdd_InvalidAmount_DoesNotRaiseBalanceChanged

**Purpose:** Verify that invalid additions do not emit the event.
**Expected result:** `BalanceChanged` is not raised after `TryAdd(0)` or `TryAdd(-3)`.

---

### TryAdd_MultipleAmounts_RaisesCurrentBalance

**Purpose:** Verify that each event carries the current accumulated total, not just the added amount.
**Expected result:** first event emits `5`; second event emits `15` after `TryAdd(5)` followed by `TryAdd(10)`.

---

### TryAdd_AmountCausingOverflow_ReturnsFalse

**Purpose:** Verify that an amount that would cause `int` overflow is rejected without modifying state.
**Expected result:** after adding `int.MaxValue`, `TryAdd(1)` returns `false`; balance remains `int.MaxValue`; no event is emitted.

---

## Automated Test Result

```
9 tests
9 passed
0 failed
```

All tests run in Edit Mode. No Play Mode tests are included in v1.0.

---

## Manual Play Mode Validation

Open `Assets/_Project/Scenes/CurrencyDemo.unity` and follow this procedure:

1. Enter Play Mode.
2. Move with **WASD** — verify the player moves in all four directions.
3. Move with **arrow keys** — verify both bindings work.
4. Release all input — verify the player stops.
5. Walk to the scene boundaries — verify the player remains inside.
6. Collect the pickup with value `1` — verify HUD shows `1`, pickup disappears, sound plays.
7. Collect the second pickup with value `1` — verify HUD shows `2`.
8. Collect a pickup with value `5` — verify HUD shows `7`.
9. Collect the second pickup with value `5` — verify HUD shows `12`.
10. Collect the pickup with value `10` — verify HUD shows `22`.
11. Verify each pickup disappears immediately after a valid collection.
12. Verify each pickup sound plays once per collection.
13. Verify rapid collections do not produce errors even if sounds overlap.
14. Verify `CoinVisualSpin` remains active and visible on each uncollected pickup.
15. Verify no pickup can be collected a second time once it has been destroyed.
16. Verify the Console shows `0` errors throughout the session.

---

## Reference Validation

Verify the following assignments in the Inspector before entering Play Mode:

| Component | Field | Expected value |
|---|---|---|
| `PlayerMovement2D` | Body | `Rigidbody2D` on Player |
| `CurrencyCollector` | Wallet | `CurrencyWallet` on Player |
| `PlayerInput` | Actions | `PlayerControls.inputactions` |
| `PlayerInput` | Default Map | `Player` |
| `PlayerInput` Move event | Listener | `PlayerMovement2D.OnMove` |
| `CurrencyDisplay` | Wallet | `CurrencyWallet` on Player |
| `CurrencyDisplay` | Balance Text | `BalanceText` TMP object |
| Pickup prefabs | Collection Sound | optional `AudioClip` |
| `CoinVisualSpin` | Target | `Coin` child Transform |
| Pickup visual `Coin` child | Animator | disabled |

---

## Failure Cases

### Invalid amount

- `TryAdd` returns `false`.
- Balance is not modified.
- `BalanceChanged` is not emitted.

### Integer overflow

- `TryAdd` returns `false`.
- Balance is not modified.
- `BalanceChanged` is not emitted.

### Missing collector

- The colliding object has no `CurrencyCollector`.
- `CurrencyPickup` silently ignores the collision.
- The pickup remains in the scene.

### Collector without wallet

- The collector's `Wallet` property is `null`.
- `CurrencyPickup` silently ignores the collision.
- The pickup remains in the scene.

### Missing audio clip

- Collection succeeds normally.
- No sound is played.
- No exception is thrown.

### Duplicate trigger

- `isCollected` is set to `true` on the first successful collection.
- The collider is disabled immediately.
- Any subsequent trigger contact is ignored.
- The pickup is destroyed after the first valid collection.

### Missing configured reference

- A descriptive `Debug.LogError` is logged with the component as context.
- The system does not attempt to locate the missing reference automatically.

---

## Acceptance Criteria

The project is considered valid for v1.0 when all of the following are true:

- Unity compiles with `0` errors.
- All 9 Edit Mode tests pass.
- The player moves correctly with WASD and arrow keys.
- Movement stops when input is released.
- The player remains inside the scene boundaries.
- All five pickups are collectable and total `22`.
- The HUD reaches `22` after collecting all pickups.
- Sound plays once per valid collection.
- `CoinVisualSpin` is active and visible on every uncollected pickup.
- No `Missing` component references exist in the scene.
- No pickup can be collected more than once.
- The Console contains `0` errors and no avoidable warnings during the validated flow.

---

## Out-of-Scope Testing

The following areas are not validated in v1.0:

- currency spending;
- cross-scene persistence;
- standalone builds;
- multiplayer;
- gamepad or controller input;
- object pooling;
- magnetic pickup behavior;
- performance with large numbers of simultaneous pickups.
