---
name: sql-migration
description: Change the database schema or persisted character/account data. Use when adding tables/columns, editing files under sql/, or when modifying ZoneDb save/load code in a way that requires schema changes.
---

# SQL schema migration

Last verified: 2026-06-10
Background: `doc/dev/persistence.md`

## Procedure

1. Create `sql/updates/update_YYYY-MM-DD_N.sql` (N = sequence number for that
   day; a descriptive suffix is also seen, e.g. `update_2026-05-05_visual_job.sql`).
   Never edit an already-applied update file.
2. Mirror the change in `sql/main.sql` so fresh installs match upgraded ones.
3. Pair the schema change with code in the right place:
   - new column/table for character/account state → `InternalSaveX(…, conn,
     trans)` in `src/ZoneServer/Database/ZoneDbInternal.cs`, called from the
     `SavePlayerData` transaction in `ZoneDb.cs`
   - matching load in the appropriate partial (`ZoneDb.Character.cs`,
     `ZoneDb.Items.cs`, `ZoneDb.Account.cs`, …)
   - shared account-level data → `src/Shared/Database/MeliaDb.cs`
4. Prefer `BatchInsertCommand` with `ON DUPLICATE KEY UPDATE` for upserts
   (see `InternalSaveRevealedMaps` in `ZoneDbInternal.cs`).
5. Give new columns sane defaults so existing rows load without backfill.

## Landmines (see doc/dev/known_issues.md)

- The `cascadeDeleteItem` trigger (`sql/updates/update_2021-10-13_1.sql`)
  deletes `items` rows when `inventory` rows are deleted — do not write save
  logic that deletes inventory links expecting the item row to survive.
- Auto-traders (`DummyConnection`) are excluded by the default
  `Map.GetCharacters()` — persistence paths must use a predicate overload.

## Checklist

- [ ] New file in `sql/updates/`, named `update_YYYY-MM-DD_N.sql`
- [ ] `sql/main.sql` updated to match (fresh-install parity)
- [ ] Load + save code paired (column without both is a silent data loss)
- [ ] Defaults handle pre-existing rows
- [ ] Tested: start server with an existing character, change state, save
      (relog or autosave), restart, verify state survived
- [ ] `doc/dev/persistence.md` updated if the save flow changed

If this procedure no longer matches the code, fix this skill file as part of
your change.
