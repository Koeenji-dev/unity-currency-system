# Media Guide — Unity Currency System

Unity Currency System v1.0 · Koeenji Dev

---

## Purpose

This guide describes how to record, capture and name the media assets used to present the Unity Currency System in the portfolio. It covers screenshots, GIFs, OBS recording requirements and the final checklist before publishing any media.

---

## Recording Target

The goal is to produce media that demonstrates a complete, working implementation of a 2D currency collection system built with Unity 6. The audience is a technical reviewer who understands Unity architecture, not necessarily a game player.

The media must convey:

- the separation of responsibilities between pickup, wallet and UI;
- the event-driven HUD update in action;
- the automated test suite passing;
- the project structure under `Assets/_Project/Systems/Currency`;
- the complete demo flow ending with a balance of `22`.

---

## Recommended Screenshots

### `screenshot_demo_overview.png`

Capture the full Unity Editor window with the demo scene open in Play Mode.

- Player and all five pickups visible in the Scene or Game view.
- HUD visible in the Game view.
- Console open with zero errors.

### `screenshot_hud_initial_zero.png`

Capture the HUD displaying `0` at the start of Play Mode before any pickup is collected.

- Game view focused.
- Balance field clearly shows `0`.

### `screenshot_pickup_value_01.png`

Capture the moment after collecting the first pickup with value `1`.

- HUD shows `1`.
- One pickup has disappeared from the scene.

### `screenshot_pickup_value_05.png`

Capture the HUD after collecting a pickup with value `5`.

- HUD shows the accumulated value reflecting the `5` addition.

### `screenshot_pickup_value_10.png`

Capture the HUD after collecting the pickup with value `10`.

- HUD shows the accumulated value reflecting the `10` addition.

### `screenshot_final_balance_22.png`

Capture the final HUD state after all five pickups have been collected.

- HUD clearly shows `22`.
- Scene is empty of pickups.
- Console shows zero errors.

### `screenshot_architecture_flow.png`

Capture a diagram or annotated view illustrating the runtime flow:

```
CurrencyPickup → CurrencyCollector → CurrencyWallet → BalanceChanged → CurrencyDisplay
```

Use a clean background. The image may be composed outside Unity using a drawing tool or screenshot of `Documentation/ARCHITECTURE.md` with a clear view of the flow.

### `screenshot_test_runner_9_passed.png`

Capture the Unity Test Runner window showing:

- `9 tests` listed under `EditMode`.
- All 9 marked as passed (green checkmarks).
- Zero failures, zero skipped.

### `screenshot_project_structure.png`

Capture the Unity Project window with `Assets/_Project/Systems/Currency` expanded, showing:

```
Currency/
├── Prefabs/
├── Runtime/
├── Tests/
└── UI/
```

---

## Recommended GIFs

### `gif_currency_collection_flow.gif`

Record a continuous sequence where the player moves through the scene and collects all five pickups in order.

- Duration: approximately 8–12 seconds.
- Keep the Game view focused so the HUD is visible throughout.
- Move smoothly, do not rush.

### `gif_hud_event_update.gif`

Record a close-up of the HUD reacting immediately to a single pickup collection.

- Focus on the HUD element in the Game view.
- The number change must be clearly visible.
- Duration: 2–4 seconds, loopable.

### `gif_final_total_22.gif`

Record the moment the last pickup is collected and the HUD transitions to `22`.

- The final number `22` should be clearly visible before the GIF ends.
- Duration: 3–5 seconds.

### `gif_coin_visual_spin.gif`

Record the `CoinVisualSpin` effect on one or more uncollected pickups.

- Player is not involved; focus on the coin visual.
- The horizontal oscillating scale effect should be clearly visible.
- Duration: 2–4 seconds, loopable.

---

## OBS Recording Checklist

Before starting any OBS capture session, verify each of the following:

- [ ] Unity is open with `CurrencyDemo.unity` as the active scene.
- [ ] Unity Editor is in Edit Mode (not in Play Mode yet).
- [ ] Console is cleared and shows zero messages.
- [ ] Test Runner has been opened and confirms `9 passed / 0 failed`.
- [ ] Game view is set to a fixed resolution (recommended: `1920×1080` or `1280×720`).
- [ ] Game view aspect ratio is locked.
- [ ] All five pickups are present in the scene hierarchy.
- [ ] Player is at the starting position.
- [ ] Audio volume is set appropriately (coin sound is audible but not distorting).
- [ ] OBS scene captures the Game view or the full Unity window as required.
- [ ] OBS output format is set to `.mp4` or `.mkv`, H.264, at least `30 fps`.
- [ ] Recording area does not show private local file system paths or workspace folders.
- [ ] Recording area does not show any IDE tabs unrelated to the project.
- [ ] Desktop notifications are disabled for the duration of the recording.
- [ ] All background applications that may produce pop-ups are closed or minimized.

---

## Manual Validation Before Recording

Run through this checklist in Unity before starting any recording session:

- [ ] Project compiles with `0` errors in the Console.
- [ ] Test Runner shows `9 passed / 0 failed`.
- [ ] Press Play and verify the HUD starts at `0`.
- [ ] Collect each of the five pickups and verify the HUD updates correctly after each one.
- [ ] Collect all five pickups and verify the final balance is `22`.
- [ ] Verify coin sounds play on collection.
- [ ] Verify `CoinVisualSpin` is active on all pickups before any collection.
- [ ] Verify pickups cannot be collected a second time.
- [ ] Verify the Console shows `0` errors throughout the entire validated flow.
- [ ] Stop Play Mode and verify the scene resets correctly.
- [ ] Reopen Play Mode and confirm the balance starts at `0` again.

---

## What to Avoid Showing

Do not expose the following during any recording or screenshot session:

- Private file paths containing personal directory names or workspace identifiers.
- Internal context documents in `_LocalContext/`.
- Cursor workspace settings or `.cursor/` folder contents.
- Any Editor panel showing draft files, temporary notes or private coordination content.
- Console errors or warnings that are not part of the normal demo flow.
- Engine configuration dialogs or import settings unrelated to the system.
- Other Unity projects open in the same Editor.
- Desktop backgrounds, browser tabs or system notifications that reveal personal information.

---

## Suggested Portfolio Thumbnails

For static portfolio thumbnails, consider one of the following compositions:

**Option A — HUD Final Balance**
Game view with the HUD showing `22` and an empty scene. Clean and immediately readable.

**Option B — Collection in Action**
Game view mid-collection with the player adjacent to a pickup and the HUD showing a mid-sequence value.

**Option C — Editor Overview**
Full Unity Editor window with the demo scene open: hierarchy, scene view, inspector and HUD visible.

**Option D — Architecture Diagram**
A clean composition showing the five-component flow diagram over a dark background, matching the documentation style.

Recommended resolution for thumbnails: `1280×720` minimum, `1920×1080` preferred.

---

## File Naming Convention

All media files must follow this convention:

```
{type}_{subject}_{variant}.{ext}
```

| Token    | Values                                      |
|----------|---------------------------------------------|
| type     | `screenshot`, `gif`, `video`, `thumbnail`   |
| subject  | descriptive kebab-case name                 |
| variant  | optional suffix for multiple takes or sizes |
| ext      | `png`, `gif`, `mp4`, `mkv`                  |

### Examples

```
screenshot_demo_overview.png
screenshot_hud_initial_zero.png
screenshot_pickup_value_01.png
screenshot_pickup_value_05.png
screenshot_pickup_value_10.png
screenshot_final_balance_22.png
screenshot_architecture_flow.png
screenshot_test_runner_9_passed.png
screenshot_project_structure.png

gif_currency_collection_flow.gif
gif_hud_event_update.gif
gif_final_total_22.gif
gif_coin_visual_spin.gif

video_currency_system_demo_v1.mp4
thumbnail_currency_system_1280x720.png
```

Media files are not committed to the repository. Store them in a dedicated local folder outside the project directory.

---

## Final Media Checklist

Before publishing any media to the portfolio:

- [ ] All recommended screenshots have been captured.
- [ ] All recommended GIFs have been recorded.
- [ ] Portfolio video has been recorded and exported.
- [ ] No screenshot or GIF contains console errors.
- [ ] No screenshot or GIF exposes private file paths.
- [ ] HUD final balance is `22` in the final capture.
- [ ] Test Runner screenshot shows `9 passed / 0 failed`.
- [ ] Project structure screenshot shows the canonical `Assets/_Project/Systems/Currency` layout.
- [ ] All files are named according to the naming convention.
- [ ] GIFs loop cleanly without visible cuts.
- [ ] Video audio is audible and does not distort on the coin collection sound.
- [ ] Thumbnail is cropped and composed correctly.
- [ ] Media has been reviewed by the portfolio owner before publishing.
