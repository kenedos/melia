---
name: combat-debugging
description: Debug or tune combat math — damage too high/low, formulas, SkillModifier, SDR/AoE ratio, crit/block/dodge, property calculations, Laima skill balancing. Use when investigating why combat numbers are wrong or when rebalancing.
---

# Combat debugging & balancing

Last verified: 2026-06-10
Background: `doc/dev/skill_handlers.md` (flow), `doc/dev/buff_handlers.md`
(hooks), `doc/server/formulas.md` (stat formulas),
`doc/packages/laima/balancing.md` (design intent)

## The damage pipeline

```
Skill handler (Packages/Laima/Skills/...)
  → SkillModifier.Default (handler may pre-set fields)
  → SCR_SkillHit (src/ZoneServer/Skills/SkillUseFunctions.cs → scriptable fn)
    → SCR_CalculateDamage (system/scripts/zone/core/calc_combat.cs):
        dodge check (SCR_GetDodgeChance)
        → SCR_Combat_BeforeCalc hooks          ← buff/skill/ability handlers
        → base atk (SCR_GetRandomAtk: PATK/MATK + modifier.BonusPAtk/MAtk)
          + SkillAtkAdd, skill factor, defense …
        → SCR_Combat_BeforeBonuses hooks
        → race/size/attribute/attack-type multipliers
        → SCR_Combat_AfterBonuses hooks
        → crit & block rolls
        → SCR_Combat_AfterCalc hooks
        → per-monster override SCR_CalculateDamage_Monster_<ClassName> if any
  → handler applies: target.TakeDamage(result.Damage, caster)
```

`SCR_SkillHit`/`SCR_CalculateDamage` compute but do NOT apply damage — the
handler does. AoE target count is limited by `targets.LimitBySDR(caster,
skill)` (Skill Damage Ratio / AoE attack ratio) before any damage is rolled.

## Diagnosis checklist — where a wrong number can come from

1. **Db values**: `packages/laima/db/skills.txt` / `skills_overrides.txt` —
   factor, factorByLevel, atkAdd, hitCount, multiHitCount, splash params.
   Overrides patch upstream: check both tiers (see `doc/dev/data_system.md`).
2. **Handler constants**: explicit splash sizes, hardcoded multipliers,
   modifier fields set in the Laima handler itself.
3. **Buff hooks**: search `[CombatCalcModifier(` for the BuffId/SkillId in
   question — every active buff on attacker AND target can mutate
   `modifier` or `skillHitResult.Damage` at its phase.
4. **Abilities**: same attribute with `AbilityId` identifiers
   (`src/ZoneServer/Packages/Laima/Abilities/`).
5. **Base formulas/properties**: `system/scripts/zone/core/calc_combat.cs`
   and property calc scripts; stat → property mapping in
   `doc/server/formulas.md`.

## SkillModifier field glossary

(`src/ZoneServer/Skills/Combat/SkillModifier.cs`)
`BonusPAtk`/`BonusMAtk` (flat atk), `BonusDamage`, `DamageMultiplier`
(multiplicative, default 1), `SkillFactorBonus`, `FinalDamageMultiplier`
(applied last), `DefenseBonus`/`DefensePenetrationRate`, `HitCount`,
crit knobs (`CritRateMultiplier`, `CritDamageMultiplier`, `BonusCritChance`,
`Min/MaxCritChance`), forced outcomes (`ForcedHit`, `ForcedCritical`,
`ForcedBlock`, `ForcedEvade`, `Unblockable`, `ForcedBackAttack`).

## Tips

- To find every modifier touching one skill:
  grep `SkillId.<X>` and `BuffId.<paired buff>` across
  `src/ZoneServer/Packages/Laima/` and `system/scripts/zone/core/`.
- Monster-specific damage overrides: grep `SCR_CalculateDamage_Monster_`.
- AutoSave/property logs print dirty-property counts; combat logs show hit
  results — run the server and reproduce (see `start-zone-1.bat`).

If this procedure no longer matches the code, fix this skill file as part of
your change.
