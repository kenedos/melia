# Buff handlers

Last verified: 2026-06-10

How buffs/debuffs are implemented: lifecycle callbacks, property modifiers,
and combat-calculation hooks.

## Authoritative source files

- `src/ZoneServer/Buffs/Base/BuffHandler.cs` — base class: lifecycle + modifier helpers
- `src/ZoneServer/Buffs/BuffHandlers.cs` — discovery/registration (same package-override
  ordering as skill handlers, see `skill_handlers.md`)
- `src/ZoneServer/Scripting/ScriptableEvents/CombatCalcModifier.cs` —
  `[CombatCalcModifier]` attribute + `CombatCalcPhase` constants
- Golden examples:
  - simple combat hook: `src/ZoneServer/Packages/Laima/Buffs/Swordsmen/Barbarian/Cleave_Debuff.cs`
  - full lifecycle + stacks + property modifier: `src/ZoneServer/Packages/Laima/Buffs/Swordsmen/Barbarian/Frenzy_Buff.cs`

## Lifecycle (override the virtuals you need)

| Callback | When |
|---|---|
| `OnActivate(buff, activationType)` | start or overbuff (stack gain); NOT called past max overbuff count — right choice for most stacking bonuses |
| `OnExtend(buff)` | every duration extension, including past max overbuff — use only if you must react after stacks cap |
| `WhileActive(buff)` | periodic tick; only fires if the buff db entry has `updateTime > 0` |
| `OnEnd(buff)` | expiry or manual stop — undo modifiers, clean `buff.Vars` here |
| `OnStart(buff)` | obsolete, use `OnActivate` |

Useful state on `Buff`: `NumArg1`/`NumArg2` (args passed to `StartBuff` —
convention: NumArg1 = skill level), `OverbuffCounter` (stack count, writable),
`Vars` (typed key-value store for handler state), `Caster`, `Target`,
`IncreaseDuration(...)`, `NotifyUpdate()` (push stack/state change to client).

## Property modifiers

`BuffHandler` static helpers manage reversible stat changes, stored in
`buff.Vars` under `Melia.Modifier.<PropertyName>`:

- `AddPropertyModifier(buff, target, PropertyName.X_BM, value)` — stacks
- `UpdatePropertyModifier(...)` / `SetPropertyModifier(...)` — replace
- `RemovePropertyModifier(...)` — undo all (call in `OnEnd`)

Buff modifiers target `*_BM` properties (e.g. `NormalASPD_BM`); `*_BM`
properties are transient and excluded from db persistence
(`IsBuffTransientProperty`).

## Combat hooks: [CombatCalcModifier]

Mark a method to run during damage calculation when the buff is present:

```csharp
[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Cleave_Debuff)]
public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target,
    Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
```

The attribute registers a scriptable function `SCR_Combat_{Phase}_{Identifier}`;
the identifier can be a BuffId, SkillId, AbilityId, or string. Always
`TryGetBuff` inside the method to fetch stacks/args — the hook fires based on
phase + identifier, and you must check which side (attacker vs target) holds
the buff.

Phases (constants in `CombatCalcPhase`): `BeforeCalc`, `BeforeBonuses` (after
base calc, before race/size bonuses), `AfterBonuses` (before crit/block),
`AfterCalc`, `OnDodge`, `OnBlock` — plus side-specific variants
(`*_Attack`, `*_Defense`) and companion variants (`*_CompanionAttack`,
`*_CompanionDefense`). Side-specific phases are checked only on that side's
entity; the plain phases fire for both.

Modify damage via `modifier.DamageMultiplier += ...` (pre-calc) or
`skillHitResult.Damage *= ...` (post-calc) depending on phase.

## Starting a buff from a skill handler

```csharp
target.StartBuff(BuffId.X_Buff, skill.Level, 0, TimeSpan.FromSeconds(10), caster);
//                              NumArg1      NumArg2  duration              caster
```

Duration/overbuff/updateTime defaults come from the buff's db entry
(`buffs.txt` — see `data_system.md`); an explicit duration here overrides it.

## Gotchas

- Namespace is `Melia.Zone.Buffs.Handlers.<Tree>.<Job>` with **singular** tree
  name (`Swordsman`), folder is plural (`Buffs/Swordsmen/`); class name gets
  an `Override` suffix for Laima overrides.
- `WhileActive` silently never fires if the db entry's `updateTime` is 0.
- Forgetting `RemovePropertyModifier` in `OnEnd` leaks the stat bonus until
  relog (the `_BM` transient exclusion limits but does not eliminate this).
- A buff with no db entry in `buffs.txt`/`buffs_overrides.txt` won't start.

Procedure + checklist: `.claude/skills/laima-buff-handler/SKILL.md`.
