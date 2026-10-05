using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

/// <summary>
/// Increases the summon's final damage by 5% per ability level and drains 1% of its maximum HP every second.
/// </summary>
[Package("laima")]
[BuffHandler(BuffId.Summoning_Overwork_Buff)]
public class Summoning_Overwork_BuffOverride : BuffHandler
{
	private const float DamageBonusPerLevel = 0.05f;
	private const float HpDrainRate = 0.01f;
	private const int DrainIntervalMilliseconds = 1000;

	public override void OnActivate(Buff buff, ActivationType activationType)
	{
		buff.SetUpdateTime(DrainIntervalMilliseconds);
	}

	public override void WhileActive(Buff buff)
	{
		if (buff.Target is not Summon summon || summon.IsDead)
			return;

		var maxHp = summon.Properties.GetFloat(PropertyName.MHP);
		if (maxHp <= 0)
			return;

		var hpDrain = Math.Max(1f, maxHp * HpDrainRate);
		summon.TakeDamage(hpDrain, summon);
	}

	public override void OnEnd(Buff buff)
	{
	}

	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Summoning_Overwork_Buff)]
	public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!attacker.TryGetBuff(BuffId.Summoning_Overwork_Buff, out var buff))
			return;

		var abilityLevel = Math.Max(1, (int)buff.NumArg1);
		modifier.DamageMultiplier += DamageBonusPerLevel * abilityLevel;
	}
}
