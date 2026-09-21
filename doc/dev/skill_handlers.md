# Skill handlers

Last verified: 2026-06-10

How active skills are implemented and dispatched in ZoneServer.

## Authoritative source files

- `src/ZoneServer/Skills/Handlers/Base/Interfaces.cs` — all handler interface contracts
- `src/ZoneServer/Skills/Handlers/SkillHandlers.cs` — discovery, registration, override/priority logic
- `src/ZoneServer/Skills/SkillUseFunctions.cs` — `SCR_SkillHit` damage entry point
- `src/ZoneServer/Skills/Combat/` — `SkillModifier`, `SkillHitResult`, `SkillHitInfo`
- Golden example (ground skill): `src/ZoneServer/Packages/Laima/Skills/Swordsmen/Barbarian/Barbarian_Cleave.cs`

## Registration & override

Handlers are classes implementing an `ISkillHandler` interface, marked with
`[SkillHandler(SkillId.X)]` (accepts multiple ids and an optional priority).
`SkillHandlers.LoadHandlersFromAssembly` reflects over the assembly at startup:

- Non-package handlers register first, `[Package("...")]`-marked handlers
  second — so a package handler **replaces** the upstream handler for the same
  SkillId at equal priority. Higher priority always wins.
- Registering an override also removes any combat-event hooks
  (`SCR_Combat_*_<SkillId>`, `SCR_Buff_OnStart/OnEnd_<SkillId>`) the replaced
  handler had set up (`RemoveCombatEvents`), so overrides start clean.
- `[Package("laima")]` handlers only register when the package is enabled in
  `packages.conf` (`PackageManager.ShouldRegister`).

## Interface choice

Pick the interface matching the skill's `useType` in the skill db
(see `data_system.md`). All are in `Interfaces.cs`:

| Interface | Signature receives | Typical useType |
|---|---|---|
| `IGroundSkillHandler` | originPos, farPos, target | MeleeGround (most attack skills) |
| `IMeleeGroundSkillHandler` | originPos, farPos, IList targets | MeleeGround with client-supplied target list |
| `ITargetSkillHandler` | single target | Targeted/Self buffs on a target |
| `IForceSkillHandler` / `IForceGroundSkillHandler` | originPos, farPos, target | Force (homing projectile) |
| `ISelfSkillHandler` | originPos, direction | Self |
| `IPassiveSkillHandler` | caster only | Passives |
| `IDynamicCasted` | StartDynamicCast/EndDynamicCast | held/charged casts (additional interface) |
| `ICancelSkillHandler` | caster | cancel notification |

The obsolete `ISkillCombat*Handler` interfaces are superseded by
`[CombatCalcModifier]` (see `buff_handlers.md`); companion variants
(`ISkillCombatCompanion*Handler`) are still current, used for passives that
hook companion attacks.

## Canonical execution flow (attack skill)

From `Barbarian_Cleave.cs` — the standard sequence inside `Handle`:

1. `caster.TrySpendSp(skill)` — bail with `ServerMessage` if false
2. `skill.IncreaseOverheat()`, `caster.SetAttackState(true)`
3. `Send.ZC_SKILL_READY(...)` + `Send.ZC_SKILL_MELEE_GROUND(...)` (packet pair
   varies by useType — copy from a working handler of the same type)
4. `skill.GetSplashParameters(caster, originPos, farPos, length, width, angle)`
   → `skill.GetSplashArea(SplashType.X, splashParam)`
5. `skill.Run(this.Attack(...))` — async attack task; inside it:
   - `await skill.Wait(...)` for hit timing
   - `caster.Map.GetAttackableEnemiesIn(caster, splashArea)`
   - `targets.LimitBySDR(caster, skill)` — enforce AoE Attack Ratio
   - per target: `SCR_SkillHit(caster, target, skill, modifier)` →
     `target.TakeDamage(result.Damage, caster)` → optionally
     `target.StartBuff(...)` → collect `SkillHitInfo`
   - `Send.ZC_SKILL_HIT_INFO(caster, hits)`

Damage numbers come from the skill db entry (`factor`, `factorByLevel`,
`atkAdd`, hit counts — see `data_system.md`) flowing through `SCR_SkillHit`;
handlers adjust via `SkillModifier` (e.g. `modifier.DamageMultiplier`).

## Gotchas

- **Namespace is singular class-tree name** (`Melia.Zone.Skills.Handlers.Swordsman.Barbarian`)
  even though the folder is plural (`Packages/Laima/Skills/Swordsmen/Barbarian/`).
  Laima overrides reuse the upstream namespace; class name gets an `Override` suffix.
- Splash parameters in the handler can differ from the db's splash values —
  the handler's explicit `GetSplashParameters` args win.
- `skill.Run(...)` must wrap the async attack; don't `await` it in `Handle`.

Procedure + checklist for adding/overriding a skill: `.claude/skills/laima-skill-handler/SKILL.md`.
Laima placement/naming conventions: `laima_package.md`.
