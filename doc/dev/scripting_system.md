# Content scripting system

Last verified: 2026-06-10

Game content (NPCs, quests, spawns, dungeons, AI, item effects) is written as
C# scripts compiled at server startup — not part of the solution build.

## Authoritative source files

- `src/ZoneServer/Scripting/` — script loading, `GeneralScript`, dialogues,
  `Shortcuts` (the `AddNpc`/`L()`/etc. API), extensions, hooking
- Script list files: `system/scripts/zone/scripts.txt` (and `barracks`)
- Laima scripts: `packages/laima/scripts/zone/` + its own `scripts.txt`
- Example with the full NPC/quest pattern:
  `packages/laima/scripts/zone/content/laima/jobs/cryomancer_quest.cs`

## How scripts load

`scripts.txt` is a script-list file with three directives:

- glob lines (`core/**/*`, `other/character_init.cs`) — scripts to compile
- `require "scripts_content.txt"` — include another list (error if missing)
- `divert "/user/scripts/zone/scripts.txt"` — if that file exists, **load it
  instead of the rest of this one** (operator override hook)

`scripts_packages.txt` is auto-generated at startup — do not edit; it pulls
in `packages/<name>/scripts/zone/scripts.txt` for each enabled package.

Zone script categories (same layout in system and laima tiers):
`const/` (constants), `core/` (combat formulas & core callbacks — e.g.
`calc_combat.cs` defines `SCR_SkillHit`), `items/` (item-use scripts),
`ais/` (monster AI), `skills/`, `minigames/`, `other/`, and `content/`
(NPCs, quests, spawns, maps, dungeons, mobs, shops…). `custom/` is the
operator tier — don't put project content there.

## Script anatomy

Scripts subclass `GeneralScript` and override `Load()`; helpers come from
`static Melia.Zone.Scripting.Shortcuts`:

- `AddNpc(monsterId, L("name"), "map", x, z, direction, DialogFunc)`
- dialog functions: `async Task Dialog(Dialog dialog)` using
  `dialog.SetTitle/SetPortrait/Msg/Select(...)`
- `L("...")` wraps localizable strings
- quests, spawns, keywords, warps each have dedicated helpers — the
  user-facing reference docs cover them:
  - `doc/server/scripting/npc_scripting.md`
  - `doc/server/scripting/quests.md`
  - `doc/server/scripting/spawns.md`
  - `doc/server/scripting/scriptable_functions.md` (all `SCR_*` hooks)
  - `doc/server/scripting/text_codes.md`, `minigame.md`, `track.md`

## Rules for Laima content

- Laima content scripts go in `packages/laima/scripts/zone/content/laima/…`
  and are listed via the laima `scripts.txt`/`scripts_content.txt` globs.
- Map class names for placements: `doc/packages/laima/available_maps.md`.
- Scripts compile at startup — a compile error in any listed script is
  reported at boot; restart to apply changes.

Procedure + checklist: `.claude/skills/zone-scripting/SKILL.md`.
