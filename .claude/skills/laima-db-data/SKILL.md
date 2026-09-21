---
name: laima-db-data
description: Add or override game data entries (items, skills, buffs, monsters, jobs, shops, drops, features) in the three-tier db system. Use when editing *.txt files under system/db, packages/laima/db, or user/db, or when a handler references an id that has no data entry.
---

# Laima db data entries

Last verified: 2026-06-10
Background: `doc/dev/data_system.md`

## Procedure

1. **Find the schema**: open the matching class in
   `src/Shared/Data/Database/` (e.g. `Skills.cs` ↔ `skills.txt`). It defines
   valid fields, types, and which are mandatory. The header comment in the
   txt file lists the column order; `[brackets]` = optional.
2. **Pick the right file** in `packages/laima/db/`:
   - brand-new entry → the main file (`skills.txt`, `buffs.txt`, `items.txt`…)
   - rebalancing an upstream entry → the `*_overrides.txt` companion if one
     exists (`skills_overrides.txt`, `buffs_overrides.txt`) — patch only the
     fields you change
   - feature gating → `features.txt`
3. **Copy a neighboring entry** as the template; keep the relaxed-JSON style
   (unquoted keys, one entry per line, section comments).
4. **Mind the merge semantics**: indexed dbs merge package entries over
   system by id; non-indexed dbs are *fully replaced* by the package file.
   Check the db class for `Indexed` in its base type before adding a partial
   package file for a non-indexed db (you'd wipe the system data).

## Checklist

- [ ] Entry is in `packages/laima/db/`, NOT `system/db/` or `user/db/`
- [ ] Id doesn't collide with a system-tier id (unless overriding deliberately)
- [ ] Matching enum value exists in `src/Shared/Game/Const/` (SkillId, BuffId,
      AbilityId, CooldownId…) if code references the id
- [ ] Client-visible name/icon resolves: `className` matches client data, or
      add `packages/laima/db/packetstrings.txt` entry
- [ ] Skill learnable via UI? → `skilltree.txt` / `abilitytree.txt`
- [ ] Restarted the server to load changes (data is read at startup)
- [ ] If you changed the loader/merge logic, update `doc/dev/data_system.md`

If this procedure no longer matches the code, fix this skill file as part of
your change.
