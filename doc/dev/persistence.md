# Persistence (MySQL save/load)

Last verified: 2026-06-10

How character/account state is persisted: ZoneDb structure, the save
transaction, autosave, and shutdown saves.

## Authoritative source files

- `src/ZoneServer/Database/ZoneDb.cs` — `SavePlayerData` entry point: one
  MySqlTransaction wrapping all `InternalSave*` calls, commit, then
  `ClearDirty()` on character/account/item properties
- `src/ZoneServer/Database/ZoneDbInternal.cs` — the `InternalSave*` methods
  (transaction-aware; each takes `conn, trans`)
- Other partials: `ZoneDb.Account.cs`, `ZoneDb.Character.cs`,
  `ZoneDb.Items.cs`, `ZoneDb.Market.cs`, `ZoneDb.Social.cs`,
  `ZoneDb.Rollback.cs`, `ZoneDb.Utilities.cs`
- `src/Shared/Database/MeliaDb.cs` — shared base (accounts, connection)
- `src/ZoneServer/Services/AutoSaveService.cs` — periodic batched saves
- `src/ZoneServer/Util/ServerShutdownManager.cs` — shutdown flow
- Schema: `sql/main.sql` + `sql/updates/` (see `.claude/skills/sql-migration`)

## Save flow

`ZoneDb.SavePlayerData(character, account)` opens one transaction and calls
the relevant `InternalSave*` methods (items, skills, abilities, buffs, quests,
properties, revealed maps, storages, collections, adventure book…), commits,
sets `character.LastSaved`, then clears property dirty flags so the next save
writes only changed properties (differential property saving).

Pattern for new persisted data: add an `InternalSaveX(…, MySqlConnection conn,
MySqlTransaction trans)` method in `ZoneDbInternal.cs`, call it from
`SavePlayerData`'s transaction, and pair it with a load in the matching
`ZoneDb.*.cs` partial. `BatchInsertCommand` with
`ON DUPLICATE KEY UPDATE` is the preferred upsert pattern (see
`InternalSaveRevealedMaps`).

## AutoSaveService

Characters are partitioned into N slots by `DbId % numberOfSlots`; a timer
saves one slot per interval, spreading DB load. Selection predicate is
`c.IsOnline || c.IsAutoTrading` — deliberately including auto-traders, which
the default `Map.GetCharacters()` overload filters out (see Gotchas).
Auto-traders save with `account = null` and skip the session-key check.
Every full cycle it can trigger `OrphanCleanupService`. `SaveAllNow()` is the
synchronous save-everything used at shutdown.

## Shutdown flow

`ServerShutdownManager` (immediate or countdown) → broadcasts warnings →
`ExecuteShutdown()` → `AutoSave.SaveAllNow()` → process exit.

## Gotchas

- **`Map.GetCharacters()` (no predicate) excludes `DummyConnection`
  characters** (`src/ZoneServer/World/Maps/Map.cs:587`) — i.e. auto-traders.
  Any new save/broadcast path must use a predicate overload like
  AutoSaveService does, or auto-traders are silently skipped.
- **`DummyConnection.Close()` is a no-op**
  (`src/ZoneServer/Network/ZoneConnection.cs`) — you cannot remove an
  auto-trader from the world by closing its connection.
- **The `cascadeDeleteItem` DB trigger** deletes from `items` whenever an
  `inventory` row is deleted (`sql/updates/update_2021-10-13_1.sql`), while
  `ZoneDbInternal.cs` explicitly avoids deleting from `items` because items
  may be traded/moved ("We do NOT delete from the `items` table here",
  ~line 87). Any code that DELETEs inventory rows and expects the item row to
  survive is wrong under this trigger. See `known_issues.md`.
- Buff property modifiers on `*_BM` properties are transient and excluded
  from persistence (`BuffHandler.IsBuffTransientProperty`).

Schema-change procedure: `.claude/skills/sql-migration/SKILL.md`.
