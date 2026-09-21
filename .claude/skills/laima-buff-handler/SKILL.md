---
name: laima-buff-handler
description: Create or modify a buff/debuff handler in the Laima package, including CombatCalcModifier damage hooks and property modifiers. Use when a skill needs a buff effect, when changing buff behavior, or for files under src/ZoneServer/Packages/Laima/Buffs/.
---

# Laima buff handler

Last verified: 2026-06-10
Background: `doc/dev/buff_handlers.md`
Golden examples — diff against these before trusting the templates:
- combat hook only: `src/ZoneServer/Packages/Laima/Buffs/Swordsmen/Barbarian/Cleave_Debuff.cs`
- lifecycle + stacks + property modifier: `src/ZoneServer/Packages/Laima/Buffs/Swordsmen/Barbarian/Frenzy_Buff.cs`

## File placement

`src/ZoneServer/Packages/Laima/Buffs/<ClassTree>/<Job>/<Name>_Buff.cs` (or
`<Name>_Debuff.cs`). ClassTree plural (Swordsmen…), or Monster/Card.

## Template (combat-hook debuff)

```csharp
namespace Melia.Zone.Buffs.Handlers.Swordsman.Barbarian   // singular tree name!
{
	/// <summary>
	/// Handler for <Name>, which <effect summary>.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.X_Debuff)]
	public class X_DebuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.X_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.X_Debuff, out var buff))
				return;

			// NumArg1 convention: skill level
			modifier.DamageMultiplier += 0.30f + (buff.NumArg1 * 0.02f);
		}
	}
}
```

For stat buffs, override lifecycle methods instead/additionally:
- `OnActivate(buff, activationType)` — apply/update bonus, cap `buff.OverbuffCounter`
- `WhileActive(buff)` — periodic logic (requires `updateTime > 0` in the db entry!)
- `OnEnd(buff)` — `RemovePropertyModifier(buff, buff.Target, PropertyName.X_BM)`
  and clean `buff.Vars`

Property modifiers: `AddPropertyModifier` / `UpdatePropertyModifier` /
`RemovePropertyModifier` with `*_BM` property names — see Frenzy_Buff.cs.

Phases: `BeforeCalc`, `BeforeBonuses`, `AfterBonuses`, `AfterCalc`, `OnDodge`,
`OnBlock`, plus `*_Attack`/`*_Defense`/companion variants — constants in
`src/ZoneServer/Scripting/ScriptableEvents/CombatCalcModifier.cs`. Always
`TryGetBuff` on the correct side (attacker for offensive buffs, target for
defensive) inside the hook.

## Starting the buff (from the skill handler)

```csharp
target.StartBuff(BuffId.X_Debuff, skill.Level /*NumArg1*/, 0 /*NumArg2*/, TimeSpan.FromSeconds(10), caster);
```

## Checklist

- [ ] `BuffId.X` exists in `src/Shared/Game/Const/BuffId.cs`
- [ ] Buff data entry: `packages/laima/db/buffs.txt` (new) or
      `buffs_overrides.txt` (patch upstream) — duration, overBuff (max
      stacks), `updateTime > 0` if `WhileActive` is used, `save` flag if it
      must survive relog
- [ ] Client-visible? Check icon/name via the buff's `className` matching
      client data; custom strings via `packages/laima/db/packetstrings.txt`
- [ ] Namespace singular tree (`Swordsman`), class suffixed `Override`
- [ ] `OnEnd` undoes every property modifier added
- [ ] Build passes: `dotnet build Melia.sln`
- [ ] If you changed the buff system itself, update `doc/dev/buff_handlers.md`

If this procedure no longer matches the code, fix this skill file as part of
your change.
