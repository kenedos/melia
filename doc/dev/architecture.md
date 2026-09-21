# Architecture overview

Last verified: 2026-06-10 (thin — deepen sections as needed)

## Server topology

Melia is four cooperating servers plus a shared library, all built from
`Melia.sln` (.NET 8) and started via `start-all.bat` / `start-*.bat`:

| Project | Role |
|---|---|
| `src/BarracksServer/` | login, character creation/selection, server list |
| `src/ZoneServer/` | the game world — one process per zone group (`start-zone-1.bat`, `-2`) |
| `src/SocialServer/` | parties, chat, cross-zone social features |
| `src/WebServer/` | client web API endpoints (`doc/server/web_api.md`) |
| `src/Shared/` | const enums, data loaders, properties, MeliaDb, packages, networking primitives |

Servers share the MySQL database (`sql/main.sql`) and the data tiers
(`system/`, `packages/`, `user/` — see `data_system.md`). Inter-server
coordination runs through the database and inter-server messages
(see `src/Shared/Network/`).

## ZoneServer subsystem map

- `World/` — maps, actors (characters, mobs, NPCs), spawning, quests,
  dungeons, trades
- `Skills/` — skill handlers + combat calc plumbing (`skill_handlers.md`)
- `Buffs/` — buff handlers (`buff_handlers.md`)
- `Pads/`, `Abilities/` — AoE trigger zones, passive abilities (`pads_abilities.md`)
- `Scripting/` — runtime-compiled content scripts (`scripting_system.md`)
- `Network/` — packet handlers + Send senders (`network_packets.md`)
- `Database/` — ZoneDb persistence (`persistence.md`)
- `Services/` — background services (`services.md`)
- `Commands/` — chat/admin commands
- `Packages/Laima/` — the Laima override layer (`laima_package.md`)

## Startup order (high level)

Server start (`ZoneServer.Run`) → load conf (`system/conf` with `user/conf`
includes) → init `PackageManager` from `packages.conf` → load data dbs
(`Server.LoadDb`, three-tier merge) → connect MySQL + apply pending
`sql/updates/` → register handlers via reflection (skills, buffs, pads,
abilities, packets — package handlers override base) → compile content
scripts (script lists) → start services and world heartbeat.

For exact details read `src/ZoneServer/ZoneServer.cs` (`Run`).
