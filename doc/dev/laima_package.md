# Laima package conventions

Last verified: 2026-06-10

The Laima expansion is a **package**: code handlers + data that only activate
when `packages.conf` lists `enabled_packages: laima`. Laima never modifies
upstream Melia files — it overrides them.

## The two halves

1. **Code**: `src/ZoneServer/Packages/Laima/` — handler classes marked
   `[Package("laima")]` (attribute: `src/Shared/Packages/PackageAttribute.cs`).
   At startup, package handlers register *after* base handlers and replace
   them for the same SkillId/BuffId/PadName/AbilityId at equal priority
   (see `skill_handlers.md` → Registration & override).
2. **Data**: `packages/laima/db/`, `packages/laima/scripts/`,
   `packages/laima/conf/`, `packages/laima/localization/` — merged over
   `system/` at load time, below `user/` (see `data_system.md`).

## Folder taxonomy & naming

```
src/ZoneServer/Packages/Laima/
├── Skills/<ClassTree>/<Job>/<Job>_<SkillName>.cs     e.g. Skills/Swordsmen/Barbarian/Barbarian_Cleave.cs
├── Buffs/<ClassTree>/<Job>/<Name>_Buff.cs            e.g. Buffs/Swordsmen/Barbarian/Cleave_Debuff.cs
├── Pads/...
└── Abilities/<JobNN>.cs                              e.g. Abilities/Falconer21.cs
```

- ClassTree folders: `Swordsmen`, `Archers`, `Clerics`, `Scouts`, `Wizards`,
  plus `Monsters`, `Common` (skills) / `Monster`, `Card` (buffs).
- **Namespaces reuse the upstream handler namespace with the singular tree
  name**: `Melia.Zone.Skills.Handlers.Swordsman.Barbarian`,
  `Melia.Zone.Buffs.Handlers.Swordsman.Barbarian`. This is deliberate — the
  override sits in the same logical namespace as the handler it replaces.
- Override classes are suffixed `Override`: `Barbarian_CleaveOverride`,
  `Cleave_DebuffOverride`.
- Each file starts with a doc comment summarizing the rework intent
  ("Per the rework, …") — keep this convention; it's the in-code design record.

## Adding new ids

New skills/buffs/abilities need enum values in `src/Shared/Game/Const/`
(`SkillId.cs`, `BuffId.cs`, `AbilityId.cs`, `CooldownId.cs`, `PropertyName.cs`).
These files are shared with upstream — append Laima additions in the
clearly-marked Laima/custom regions if present, otherwise at the end, and
keep numeric ids consistent with the client's data where applicable.

## Rules

- Never edit files outside `Packages/Laima/` to change Laima behavior —
  if upstream behavior must change, add an override handler or a
  `*_overrides.txt` data entry.
- Exception: genuinely shared infrastructure (new enum values, new hook
  points in the combat system) lives upstream; keep such changes minimal
  and behavior-neutral when the package is disabled.
- New game data: `packages/laima/db/*.txt` (see `data_system.md`).
- Feature-gate larger systems via `packages/laima/db/features.txt` toggles.

Balancing/design philosophy: `doc/packages/laima/balancing.md`.
Procedures: `.claude/skills/laima-skill-handler/`, `.claude/skills/laima-buff-handler/`.
