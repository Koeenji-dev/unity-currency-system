# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2026-06-17

### Added

- `CurrencyWallet` with positive-value validation and integer overflow protection.
- `BalanceChanged` event that emits the current accumulated balance after each valid addition.
- `CurrencyCollector` as the explicit pickup receiver entry point, with a serialized wallet reference.
- `CurrencyPickup` with configurable pickup value and duplicate collection protection via immediate collider disable.
- Pickup values `1`, `5` and `10`.
- Optional pickup audio via `AudioSource.PlayClipAtPoint`; collection remains functional without an assigned clip.
- `CurrencyDisplay` with event-driven HUD updates; subscribes in `OnEnable` and unsubscribes in `OnDisable`.
- `PlayerMovement2D` using the new Unity Input System, independent from the currency system.
- Reusable `Player` prefab with serialized `Rigidbody2D` and movement references.
- `CoinVisualSpin` for a lightweight scale-based visual animation of coin pickups.
- Top-down 2D demo scene (`CurrencyDemo`) with five pickups totaling `22` currency units.
- Edit Mode test suite with 9 tests covering all `CurrencyWallet` contract cases.
- Minimal Assembly Definitions: one for runtime isolation (`KoeenjiDev.CurrencySystem`) and one for Edit Mode tests (`Tests`).
- Bilingual public README files (`README.md` and `README.es.md`).

### Changed

- Project assets reorganized under canonical `Assets/_Project/` folder structure.
- Demo player and input assets moved to canonical paths under `Assets/_Project/Demo/`.
- Value-5 pickup prefab renamed from `CurrencyPickup_5` to `CurrencyPickup_05` for consistent naming.
- Pickup collider field renamed from `col` to `pickupCollider` for clarity.
- Wallet tests expanded to explicitly validate successful `TryAdd` return values and intermediate overflow state.

### Fixed

- Removed duplicate and obsolete demo asset paths that were present alongside the canonical structure.
- Replaced the imported coin `Animator` animation, which did not work correctly in the current Unity version, with `CoinVisualSpin`.
- Resolved missing-script reference after the visual animation component was renamed.
- Aligned pickup collider references and visual sprites after scene setup.
- Preserved `PlayerInput` action references after the Input Actions asset was moved to its canonical path.
