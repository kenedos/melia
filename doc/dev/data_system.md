# Game data system (db files)

Last verified: 2026-06-10

Game data (items, skills, buffs, monsters, jobs…) lives in text files with
JSON-array bodies, loaded and merged at server startup from three tiers.

## Authoritative source files

- `src/Shared/Server.cs` → `LoadDb(...)` (~line 472) — the merge logic itself
- `src/Shared/Data/Database/*.cs` (70 files) — one class per db file defining
  the schema: field names, types, defaults, mandatory fields. To know what a
  db entry may contain, **read the matching `*Data` class** (e.g. `Skills.cs`
  for `skills.txt`, `Buffs.cs` for `buffs.txt`).
- `packages/laima/db/` — Laima's data tier (29 files as of writing)

## File format

```
// comments and a header listing the value columns
[
{ skillId: 1, className: "Normal_Attack", name: "Basic Attack", ... },
...
]
```

Relaxed JSON: unquoted keys, trailing comments allowed. The header comment in
each file lists the accepted fields; brackets `[field]` mean optional.

## Load order and merge semantics (read carefully)

For each db file, `LoadDb` loads in order: **system → packages → user**.
What "load" means depends on the database type:

- **Indexed databases** (type name contains `Indexed`): package/user entries
  are **merged over** the loaded data — same index (id) replaces, new ids add.
- **Non-indexed databases**: loading a package or user file **clears the db
  first** — the later file *fully replaces* system data, no merging.

Check the db's class in `src/Shared/Data/Database/` to see whether it derives
from an indexed base before assuming merge behavior.

### Client versioning

If `Versions.Client != 0`, per-file versioned paths take precedence:
`system/versions/<version>/db/x.txt` over `system/db/x.txt`, and
`user/<version>/db/x.txt` over `user/db/x.txt`. The Laima branch targets
client 390044.

### `*_overrides.txt` files

Some databases have a companion overrides db (e.g. `skills_overrides.txt`,
`buffs_overrides.txt`) loaded separately. These patch a *subset of fields* on
existing entries (sp, cooldown, factor, splash…) without redefining the whole
entry — the preferred way for Laima to rebalance upstream skills/buffs.

### `features.txt`

Feature toggle tree; Laima gates content via `packages/laima/db/features.txt`.

## Rules for Laima content

- New entries and overrides go in `packages/laima/db/` — never edit
  `system/db/` for Laima behavior.
- `user/db/` is the server operator's tier; don't put project content there.
- Ids must not collide with system-tier ids unless the intent is to override.
- New ids usually need a matching enum value in `src/Shared/Game/Const/`
  (SkillId, BuffId, AbilityId…) and, for client-visible strings,
  an entry in `packages/laima/db/packetstrings.txt`.
- Data is read at startup; restart the server to apply changes.

Procedure + checklist: `.claude/skills/laima-db-data/SKILL.md`.
