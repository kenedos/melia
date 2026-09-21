# Developer documentation index

Last verified: 2026-06-10

Agent-facing and developer-facing documentation for working on Melia's code
and the Laima package. Entry point for agents: [/CLAUDE.md](../../CLAUDE.md).
User-facing docs (setup, gameplay, scripting reference) live in `doc/server/`,
`doc/scripting/`, `doc/game/`.

## Subsystem guides

| Doc | Covers |
|---|---|
| [architecture.md](architecture.md) | Server topology (Barracks/Zone/Social/Web), ZoneServer subsystem map, startup and data-load order |
| [laima_package.md](laima_package.md) | Laima package conventions: folder taxonomy, file naming, `[Package("laima")]` override semantics, enum additions |
| [skill_handlers.md](skill_handlers.md) | Skill handler interfaces, attributes, splash areas, damage flow, packet sequences |
| [buff_handlers.md](buff_handlers.md) | BuffHandler lifecycle, CombatCalcModifier phases, buff↔skill pairing |
| [pads_abilities.md](pads_abilities.md) | Pad (AoE trigger zone) handlers and ability handlers |
| [data_system.md](data_system.md) | Three-tier db merge (system/packages/user), txt-JSON format, overrides, feature toggles |
| [scripting_system.md](scripting_system.md) | How content scripts load, script categories, where Laima scripts go |
| [persistence.md](persistence.md) | ZoneDb partial classes, save transactions, AutoSaveService, shutdown saves |
| [network_packets.md](network_packets.md) | Packet handler registration, Send.ZC_* conventions, adding packets |
| [services.md](services.md) | Catalog of ZoneServer services and their lifecycle |
| [known_issues.md](known_issues.md) | Durable landmine list — verified bugs and traps with file pointers |

Laima design intent and balancing philosophy: [../packages/laima/balancing.md](../packages/laima/balancing.md)

## Agent skills (procedures with templates + checklists)

Located in `.claude/skills/<name>/SKILL.md` — plain markdown, usable by any tool.

| Skill | Use when |
|---|---|
| `laima-skill-handler` | Adding or reworking a skill handler |
| `laima-buff-handler` | Adding or changing a buff/debuff handler |
| `laima-db-data` | Adding/overriding game data entries (items, skills, buffs, monsters…) |
| `zone-scripting` | NPCs, dialogs, quests, spawns, warps |
| `sql-migration` | Schema changes and persisted-data changes |
| `combat-debugging` | Investigating damage numbers, formulas, balancing |

## Maintenance table (doc-sync rule)

When changing code in an area below, update the owning doc and skill in the
same change. If a doc or skill no longer matches reality, fixing it is part
of the change — not optional follow-up.

| Code area | Owning doc | Affected skill |
|---|---|---|
| `src/ZoneServer/Skills/**`, `Packages/Laima/Skills/**` | skill_handlers.md | laima-skill-handler |
| `src/ZoneServer/Buffs/**`, `Packages/Laima/Buffs/**` | buff_handlers.md | laima-buff-handler |
| `src/ZoneServer/Pads/**`, `src/ZoneServer/Abilities/**` | pads_abilities.md | — |
| `src/Shared/Data/**`, `system/db/**`, `packages/laima/db/**` | data_system.md | laima-db-data |
| `system/scripts/**`, `packages/laima/scripts/**`, `src/ZoneServer/Scripting/**` | scripting_system.md | zone-scripting |
| `src/ZoneServer/Database/**`, `sql/**`, `AutoSaveService` | persistence.md | sql-migration |
| `src/ZoneServer/Network/**` | network_packets.md | — |
| `src/ZoneServer/Services/**` | services.md | — |
| Combat formulas, `SkillModifier`, `SCR_SkillHit` | skill_handlers.md + combat pipeline in combat-debugging skill | combat-debugging |

## Conventions for these docs

- Every page carries a `Last verified: YYYY-MM-DD` line — re-stamp when you
  verify or update it.
- Docs are pointer-maps: they name authoritative source files and explain
  invariants/gotchas. Code templates live only in skills, and each template
  names a "golden example" file to diff against before trusting it.
