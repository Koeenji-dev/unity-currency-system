# Known Issues and Limitations

This document describes the known limitations, deliberate scope exclusions and workarounds for Unity Currency System v1.0. Items are distinguished from confirmed bugs observed in the validated flow.

---

## Imported Coin Animation

**Type:** Known issue — third-party asset compatibility

The imported `Animator` animation from the third-party coin asset did not work correctly in the current Unity version. The `Animator` component remains disabled on all pickup prefabs.

- The coin sprite asset is still used for pickup presentation.
- `CoinVisualSpin` provides the visual spin animation instead.
- This does not affect collection logic, sound or balance behavior in any way.

---

## Editor-Only Validation

**Type:** Limitation — validation coverage

The project has been validated in the Unity Editor as an interactive demo.

- No public standalone build is currently distributed.
- Build configuration and platform testing are outside v1.0 scope.
- This is a limitation of validation coverage, not a confirmed build failure.

---

## No Persistence

**Type:** Deliberate scope exclusion

Balance and scene state are not persisted between Play Mode sessions.

- Restarting Play Mode resets the balance to `0`.
- All pickups return when the scene reloads.
- No save/load mechanism or cross-scene persistence is included.
- This is expected behavior in v1.0.

---

## No Gamepad Support

**Type:** Deliberate scope exclusion

Only keyboard bindings are configured and validated.

- Supported bindings: WASD and arrow keys.
- No gamepad bindings have been configured or tested.
- Gamepad support is listed as a future improvement.

---

## No Spending Operation

**Type:** Deliberate scope exclusion

The wallet supports balance additions only.

- There is no subtraction, spending or deduction API.
- No shop or economy UI exists.
- This is an intentional scope limit for v1.0.

---

## Audio Approach

**Type:** Limitation — scalability

Pickup sounds are played using `AudioSource.PlayClipAtPoint`.

- This is appropriate for the small scale of the current demo.
- The method creates temporary internal audio objects per call.
- There is no audio pooling or audio manager.
- For scenes with very large numbers of simultaneous pickups this approach may not be optimal, though no performance issues have been measured in the current demo.

---

## UI Scope

**Type:** Deliberate scope exclusion — presentation

The HUD displays only the numeric balance as plain text.

- No animation, number formatting, currency icons or localization system is included.
- The underlying event flow (`BalanceChanged`) is fully implemented and ready to support additional presentation.
- Presentation remains intentionally minimal in v1.0.

---

## Pickup Configuration

**Type:** Limitation — authoring

Pickup values and component references are configured individually per prefab in the Inspector.

- No ScriptableObject-based currency definitions exist.
- When editing pickup prefabs, collider size and visual scale must remain aligned manually.
- Missing references produce descriptive `Debug.LogError` messages but are not recovered automatically.

---

## Test Coverage

**Type:** Limitation — automated coverage

Automated tests cover `CurrencyWallet` domain logic only.

- `CurrencyPickup`, `CurrencyCollector`, audio behavior, `CurrencyDisplay` and `PlayerMovement2D` are validated through manual Play Mode testing.
- No automated Play Mode tests are included in v1.0.
- No performance or stress testing has been performed.

---

## Third-Party Licensing

**Type:** Limitation — redistribution

The project includes third-party visual and audio assets.

- These assets remain subject to their own licenses and terms of use.
- Anyone cloning or redistributing this project should verify those terms independently.
- Any project-wide license does not replace or override the licenses and terms that apply to third-party assets.

---

## Not Known Issues

The following items are confirmed working in the validated flow and are not current issues:

- Unity compiles with `0` errors.
- All 9 Edit Mode tests pass.
- No `Missing` component references remain in the demo scene.
- The final collectible total reaches `22` as expected.
