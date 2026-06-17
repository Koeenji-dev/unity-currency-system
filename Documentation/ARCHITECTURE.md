# Architecture

Unity Currency System v1.0 separates four distinct concerns:

- **Currency domain logic** — balance storage, validation and event emission.
- **Collection interaction** — pickup detection and delegation to the wallet.
- **UI presentation** — balance display driven by events.
- **Demo-only behavior** — player movement and coin visual spin, independent from the domain.

---

## Architectural Goals

- Small and reusable system focused on a single responsibility per component.
- Explicit dependencies: all references are serialized and assigned in the Inspector.
- Event-driven communication between domain and presentation layers.
- Isolated wallet logic with no knowledge of UI, audio or pickup behavior.
- Testable domain behavior through Edit Mode tests.
- No global manager and no singleton.
- Proportional architecture: no interfaces or abstractions beyond what the system requires.

---

## System Flow

```text
CurrencyPickup
→ CurrencyCollector
→ CurrencyWallet
→ BalanceChanged
→ CurrencyDisplay
```

Full flow:

1. `CurrencyPickup` detects a trigger and looks for a `CurrencyCollector` using `GetComponentInParent`.
2. `CurrencyCollector` exposes its wallet reference through a read-only property.
3. `CurrencyWallet.TryAdd` validates the amount against domain rules.
4. A successful operation updates the balance and raises `BalanceChanged` with the new total.
5. `CurrencyDisplay` receives the event and updates the HUD text.
6. The pickup disables its collider immediately, optionally plays an audio clip and destroys itself.

If `TryAdd` fails:

- the balance does not change;
- no event is emitted;
- no sound is played;
- the pickup is not consumed.

---

## Component Responsibilities

### CurrencyWallet

Owns the current balance. It is the only component that reads or modifies it.

- `Balance` is read-only externally.
- `TryAdd(int amount)` validates the operation before modifying state.
- Rejects zero and negative values.
- Rejects any amount that would cause integer overflow.
- Raises `BalanceChanged` only after a successful addition, with the updated total as payload.
- Contains no UI logic, no sound logic and no knowledge of pickups.

Public API:

```csharp
public int Balance { get; }

public event Action<int> BalanceChanged;

public bool TryAdd(int amount);
```

---

### CurrencyCollector

Marks the collecting entity and provides access to its wallet.

- Holds an explicit serialized reference to `CurrencyWallet`.
- Exposes `Wallet` as a read-only property.
- Acts as the component that pickups look for on the collecting entity.
- Stores no balance.
- Performs no UI updates.
- Does not search globally for dependencies.

---

### CurrencyPickup

Handles the collection event for a single pickup.

- Holds a configurable positive value (`Min(1)` in the Inspector).
- Detects a compatible collector using `GetComponentInParent<CurrencyCollector>()` on the entering collider.
- Guards against duplicate collection with a private `isCollected` flag.
- Disables its collider immediately after a successful call to `TryAdd`.
- Plays an optional audio clip through `AudioSource.PlayClipAtPoint` if one is assigned.
- Destroys its `GameObject` only after a successful wallet addition.

---

### CurrencyDisplay

Renders the current wallet balance in the HUD.

- Holds explicit serialized references to a `CurrencyWallet` and a `TMP_Text` component.
- Subscribes to `BalanceChanged` in `OnEnable`.
- Unsubscribes from `BalanceChanged` in `OnDisable`.
- Renders the current balance immediately when enabled, without waiting for the next event.
- Never polls through `Update`.
- Never modifies wallet state.

---

### PlayerMovement2D

Demo-only component. Provides keyboard movement for the player in the demo scene.

- Uses the new Unity Input System.
- Receives movement input through a public `OnMove(InputAction.CallbackContext context)` method.
- Applies movement by assigning `Rigidbody2D.linearVelocity` in `FixedUpdate`.
- Has no knowledge of the currency system.

---

### CoinVisualSpin

Demo and presentation-only component. Provides a lightweight visual animation for coin pickups.

- Modifies only the local X scale of a target `Transform`, using `Mathf.Cos` for continuous oscillation.
- Preserves the target's original Y and Z scale at all times.
- Has no knowledge of the currency system, physics colliders, audio or UI.
- Replaces the imported Animator animation from the third-party asset, which did not work correctly in the current Unity version.

---

## Dependency Model

Persistent component dependencies are assigned through explicit serialized references in the Inspector. The only runtime discovery is the local `GetComponentInParent<CurrencyCollector>()` lookup performed during a pickup interaction.

The following patterns are not used anywhere in the project:

```text
GameObject.Find
FindFirstObjectByType
FindAnyObjectByType
singletons
service locators
global managers
```

`GetComponentInParent` is used only in `CurrencyPickup.OnTriggerEnter2D` as local discovery during a specific physics interaction. It is not a global search.

---

## Events

`BalanceChanged` is the only event in the system.

- **Publisher:** `CurrencyWallet`
- **Subscriber:** `CurrencyDisplay`
- **Payload:** `int` — the total accumulated balance after the successful operation
- Invalid operations never emit the event
- The UI layer remains fully decoupled from pickup and collection logic

Conceptual usage:

```csharp
// In CurrencyWallet
BalanceChanged?.Invoke(balance);

// In CurrencyDisplay
wallet.BalanceChanged += Refresh;
```

---

## Assemblies

Two Assembly Definitions are active in v1.0:

```text
KoeenjiDev.CurrencySystem
Tests
```

- `KoeenjiDev.CurrencySystem` defines a dedicated compilation boundary for the core runtime currency scripts.
- `Tests` is an Editor-only assembly that references the runtime assembly and the Unity Test Framework dependencies.
- UI and Demo scripts remain in the predefined default assembly in v1.0.
- No additional assemblies are introduced.

---

## Folder Boundaries

```text
Assets/_Project/
├── Demo/
│   ├── Input/
│   ├── Prefabs/
│   └── Runtime/
├── Systems/
│   └── Currency/
│       ├── Prefabs/
│       ├── Runtime/
│       ├── Tests/
│       └── UI/
└── Scenes/
```

- `Systems/Currency` contains the reusable currency system code, prefabs, tests and UI.
- `Demo` contains the player prefab, input configuration and demo-only scripts that are specific to this scene.
- `Assets/ThirdParty` holds external assets and remains outside `_Project`.
- Generated Unity folders (`Library`, `Temp`, `Logs`) are not part of the architecture.

---

## Validation and Failure Handling

The system handles invalid states defensively without throwing exceptions:

- `TryAdd` returns `false` for zero and negative amounts; the balance is not modified.
- `TryAdd` returns `false` for any amount that would overflow `int`; the balance is not modified.
- Missing serialized references on `CurrencyCollector`, `CurrencyDisplay`, `PlayerMovement2D` and `CoinVisualSpin` produce a descriptive `Debug.LogError` with the component as context.
- `CurrencyPickup` validates that a `Collider2D` exists on the same GameObject and reports a descriptive error when it is missing.
- Missing `AudioClip` in `CurrencyPickup` does not prevent collection; the pickup is consumed normally.
- Trigger contacts with objects that have no `CurrencyCollector` are silently ignored.
- Trigger contacts with a collector whose wallet is null are silently ignored.
- `isCollected` in `CurrencyPickup` blocks any further processing after the first successful collection.

The system does not attempt to recover or locate missing references automatically.

---

## Testing Strategy

Wallet logic is covered by Edit Mode tests:

- 9 tests in `CurrencyWalletTests`.
- A `GameObject` with `CurrencyWallet` is created and destroyed per test using `[SetUp]` and `[TearDown]`.
- Tests verify observable behavior: balance values, return values and event counts.
- Implementation details such as field names or internal structure are not tested directly.

Manual Play Mode validation covers:

- WASD and arrow-key movement;
- pickup collection for values `1`, `5` and `10`;
- HUD update after each valid collection;
- final total balance of `22`;
- duplicate collection prevention;
- console free of errors and avoidable warnings.

---

## Deliberate Constraints

The following are excluded from v1.0:

- currency spending;
- cross-scene persistence;
- ScriptableObject-based currency definitions;
- object pooling;
- multiplayer;
- magnetic pickup behavior;
- global audio management;
- package extraction.

These exclusions keep the project focused on demonstrating the core collection and balance flow cleanly within a single scene.

---

## Extension Points

The following extensions are reasonable future directions. None are currently implemented.

- **Currency spending** through a dedicated wallet operation that validates sufficient balance.
- **Persistence adapter** that serializes and restores `CurrencyWallet` balance between sessions.
- **ScriptableObject currency definitions** for configurable pickup values shared across prefabs.
- **Pickup pooling** to support larger scenes with many simultaneous pickups.
- **Reusable Unity package** extracting `Systems/Currency` as a standalone distributable.
- **Additional UI subscribers** to `BalanceChanged` for animations, sound or achievements.

Any extension should preserve the separation between domain logic, collection interaction and presentation.
