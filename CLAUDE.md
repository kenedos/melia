# Melia (laima branch) — Tree of Savior MMORPG server emulator, C# / .NET

Open-source ToS server. This branch adds the **Laima expansion** as a package:
hundreds of skill/buff/pad/ability handlers, instance dungeons, parties,
trading — all toggled via `packages.conf` (`enabled_packages: laima`).
Targets client version 390044.

## Build & run

- Build: `dotnet build Melia.sln`
- Tests: `dotnet test` (Test.Shared)
- Run: `start-all.bat` (or start-barracks/zone-1/social/web individually)

## Map of the codebase

- `src/ZoneServer/` — gameplay engine: Skills, Buffs, Pads, Abilities, World, AI, Network, Services
- `src/ZoneServer/Packages/Laima/` — our handler overrides (`[Package("laima")]`), organized `<ClassTree>/<Job>/`
- `src/Shared/` — const enums (SkillId, BuffId), data loaders, ObjectProperties, MeliaDb base
- `src/BarracksServer/`, `src/SocialServer/`, `src/WebServer/` — login/character, party/chat, web API
- `system/db/` + `packages/laima/db/` + `user/db/` — three-tier merged JSON-text game data
- `system/scripts/` + `packages/laima/scripts/` — C# content scripts (NPCs, spawns, AI, dungeons)
- `system/conf/` + `user/conf/` — config with include mechanism (user overrides system)
- `sql/main.sql` + `sql/updates/` — MySQL schema + migrations
- Full architecture: `doc/dev/architecture.md`

## Hard rules

1. **Laima content never edits upstream Melia files.** Override behavior via
   `[Package("laima")]` handlers in `src/ZoneServer/Packages/Laima/` and data
   overrides in `packages/laima/db/`. See `doc/dev/laima_package.md`.
2. **New game data entries go in `packages/laima/db/*.txt`** (or
   `*_overrides.txt` to patch upstream entries) — never in `system/db/`.
3. **Schema changes are new files in `sql/updates/update_YYYY-MM-DD_N.sql`**
   (and mirrored in `main.sql` for fresh installs) — never edit applied updates.
4. Coding style: tabs, Allman braces, naming per `CONTRIBUTING.md`.

## Doc-sync rule

When you change how a subsystem works, update its `doc/dev/` page and any
`.claude/skills/` checklist that references it. The subsystem → doc → skill
mapping lives in the maintenance table in `doc/dev/README.md`.

## Where to look first

`doc/dev/README.md` is the index of all developer docs and agent skills.
Known landmines and unresolved bugs: `doc/dev/known_issues.md`.
User-facing docs (setup, scripting reference, formulas): `doc/`.
