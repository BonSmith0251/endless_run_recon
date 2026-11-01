# Endless Rails — Unity 2022.3 migration

Project: `scene-reconstruction/EndlessRails-2022` — **Unity 2022.3.62f3 (96770f904ca7)**, Windows x64, Mono, URP 14.0.12.
Baseline: `scene-reconstruction/EndlessRails-Reconstructed` (Unity 6000.6.4f1), left unchanged. Original build: 6000.3.10f1.
Migration workspace: `scene-reconstruction/migration-2022` (tools, evidence, backups of every touched file, removed DLLs/packages).

## Issue fixed: Steam emulator

The "original" `steam_api64.dll` was the gbe_fork (Goldberg) Steam emulator (11 MB, `steam_settings/` beside it), not Valve's DLL.
The 2022 project uses Valve's official `steam_api64.dll` from Steamworks.NET 2025.164.0 (SDK 1.64): 317,080 bytes,
Authenticode-signed by Valve Corp., version 10.51.08.25, SHA-256 `eb17909a…bfb75`. The emulator DLL is kept in
`migration-2022/backups/steam/`. `SteamAPI_Init()` now honestly fails when the Steam client/app ownership is unavailable
instead of being faked; achievements and stats are therefore unavailable in editor tests.

## What changed for 2022

| Area | Change |
| --- | --- |
| Packages | Unity 6 player DLLs of Unity packages replaced with 2022 packages: URP/Core/ShaderGraph 14.0.12, ugui 1.0.0, TextMeshPro 3.0.7, Input System 1.14.0, Localization 1.5.2, Addressables 1.22.3, Cinemachine 3.1.7, Timeline 1.7.7, Splines 2.8.0, Mathematics 1.3.3. Unused Unity 6-only DLLs removed (VisualScripting, Recorder, GraphToolkit, GPU-driven, unified ray tracing, etc.). |
| Script references | 421 files / 95 types remapped from DLL `(guid, MD4 fileID)` to package scripts `(guid, 11500000)`. The fileID formula was verified against a known reference. |
| Plugin metas | 39 PluginImporter metas converted from Unity 6 schema (v3 dictionary) to 2022 (v2 list); Windows-only native DLLs no longer marked for Linux/macOS. |
| Third-party DLLs | Patched with Cecil, originals kept: FMODUnity (`Rigidbody(2D).linearVelocity` → `velocity`, 6 sites), VhsFx/VolFx (URP 17 RenderGraph path removed, legacy `Execute` path retargeted to URP 14; type identities unchanged), InputGlyphs.Display (`TMP_Asset.set_material` → field). All kept DLLs resolve against 2022 except two unused UniTask `Awaitable` helpers (`WaitForEndOfFrame(CancellationToken)`, `AsUniTask(Awaitable)`). |
| Game code | Unity 6-only APIs replaced: three named colors, `PolygonCollider2D.CreateFromSprite` (2022 extension using the sprite's imported physics shape, mesh-outline fallback — approximation), `TMP_Settings.defaultFontAsset` setter, `[field: Range]`, URP 17 Bloom filter (both profiles use Gaussian = URP 14 behaviour). `GenericGameEventSinkData` conditions now serialize through a non-generic base (2022 rejects generic `[SerializeReference]` element types; without this the Gator/Grandma achievement conditions were dropped). |
| URP 17-only types | Compatibility volume components keep data for `ScreenSpaceLensFlare` (not rendered in URP 14; intensity 0 in the shipped profile) and `ProbeVolumesOptions` (unused). TAA shader reference retargeted to URP 14. |
| Shaders | 11 reconstructed sprite shaders include a URP 14 compatibility header for Core2D/sprite instancing. |
| TMP data | 15 font + 1 sprite asset get TMP 3.0.7's `material` field; 424 text components get `m_enableWordWrapping`/`m_enableKerning` from Unity 6's `m_TextWrappingMode`/`m_ActiveFontFeatures` (without it 3.0.7 disables wrapping and kerning). Unity 6 fields are kept. |
| Localization | Unity 6 Addressables bundles/binary catalog cannot load in 2022: rebuilt from the recovered tables with the real Localization editor — 15 locales, 20 table collections, 336 entries, original `Locale`/`Locale-<code>` labels, no preload labels (as original). 20 SharedTableData assets got their original GUIDs back so the game's GUID table references resolve. |
| Editor workarounds | The Unity 6 player-DLL hooks for Localization and UGUI were removed; the Bootstrap play-start fix is kept. Harness outputs/identity redirected to `migration-2022` and test identity `EndlessRails-Reconstructed-2022`. |
| Scene drift | 102 fields on 27 layout-driven UI objects/scaled sprites in `SampleScene` had drifted from the original build in later Unity 6 editor sessions (also present in the current baseline). Restored to source values. |

## Validation (2022.3.62f3)

| Check | Result |
| --- | --- |
| Compile | 0 errors |
| Source scene comparison | 2,076 records / 14,082 fields, 0 mismatches (baseline currently has 102) |
| Structural audit | 1,038 scene objects, 92 prefabs, 52,731 references: 0 missing scene scripts, 0 broken references, 0 logged errors; 1 missing script in the exported URP 17 debug-UI prefab (unused) |
| Achievement conditions | 3 sinks load their stored conditions (Gator, Grandma configs; KILL_COUNT has none in the original) |
| Startup (graphics, D3D11) | Bootstrap → SampleScene, localized menu, renders like the baseline; only Steam unavailability errors |
| Gameplay (final) | From a clean test save: New Run → select → playing; 13 enemies, 13 s after first spawn: behaviour PASS. Runtime verdict FAIL only for Steam unavailable (2) and the Animancer Lite Pro-feature restriction inherited from the original — the same inherited FAIL the Unity 6 baseline reports |
| Player build | Windows x64 development, no variant stripping: Succeeded, 0 errors (`migration-2022/player/EndlessRails-2022.exe`) |
| Player run | Starts, renders the menu/scene, closes with exit code 0; completed-log scan 0 exceptions; no generic SerializeReference warning |

Evidence: `migration-2022/evidence/final/` (audit, smoke, gameplay, build reports and captures) and `final-player-2.png`.

Not verified: time-aligned visual comparison with the original game; full progression; DX12/release builds; Steam services.

## Open items

- With a saved run present, the hidden batch-mode editor harness once found the game already resumed into wave 1
  before it could press New Run (`evidence/final-gameplay-3.log`). The standalone player with the same save stayed on
  the main menu. Not investigated further; the editor "Resume" path is therefore unverified.
- The D: drive ran out of space during one validation pass; no project file was truncated (572 rewritten assets
  checked) and every check above was re-run afterwards.
- UniTask `Awaitable` helpers unresolved in 2022 (unused by the game).
- Font asset table entries reference original font GUIDs that were never addressable in the shipped catalog either (unused).
- "trial version" watermark in the player comes from the editor license state (licensing service timeouts), not the project.

## Offline build (network features removed)

Requested for offline use. Backups of every touched file: `backups/offline/`; removed DLLs: `removed/offline-plugins/`.

| Network feature | Change |
| --- | --- |
| Steam (achievements, stats, Steam Input, store integration) | Removed Steamworks.NET, `steam_api64.dll`, InputGlyphs.Loaders.Steam, UnitySteamInputAdapter(.Utils) and InputGlyphs.Sample. `SteamManager`, `AchievementManager`, `AchievementData`, `StatData` are inert offline versions with the same script identities, so the scene object and the 24 achievement/stat assets keep their data. Achievements and stats are not tracked. |
| Social links (X, Discord, Steam store) | `OpenUrl.Open` no longer opens a browser; the main menu `Socials` row (“Follow Endless Rails”) is inactive. This is the only intentional difference from the original scene (source comparison: 1 mismatch, `Socials.m_IsActive`). |
| FMOD Live Update (profiler TCP port) | Disabled on all 12 FMOD platforms (was enabled for play-in-editor). |
| Unity hardware statistics | `submitAnalytics: 0`. Unity Personal licenses may still submit them; analytics, crash and performance reporting were already disabled. |
| Addressables | Startup catalog update check disabled; all content is local. |
| Hot Reload runtime (local server client) | Unused DLL removed. |
| Development player connection (profiler sockets) | `ReconstructionTools.BuildPlayer` now makes a release build; `BuildPlayerDiagnostic` remains a development build. |

Validation after the change (Unity 2022.3.62f3, project closed in other editors):

| Check | Result |
| --- | --- |
| Structural audit | 1,038 scene objects, 92 prefabs, 0 missing scene scripts, 0 broken references, 0 logged errors |
| Startup | PASS, 0 runtime errors (Steam errors gone) |
| Gameplay | Behaviour PASS (12 enemies, 13 s after first spawn); only the inherited Animancer Lite restriction remains |
| Release player build | Succeeded, 0 errors; no Steam or Hot Reload binaries in the player |
| Player network monitor | 60 one-second samples of the player's own sockets: 0 TCP, 0 UDP (the previous development build held TCP 55000 listen and UDP 59992). Player exited with code 0, completed log 0 exceptions |

Evidence: `evidence/offline/`, `evidence/offline-player-process.json`, `evidence/offline-player.png`.
