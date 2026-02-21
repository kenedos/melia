using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Monk
{
	/// <summary>
	/// Handler for Golden_Bell_Shield_Buff. Drains 65 SP per second
	/// and tracks total SP consumed. Provides damage reduction equal
	/// to (30% + 3% * SkillLv), hard capped at 80%. When the caster
	/// uses Energy Blast, adds bonus flat damage equal to
	/// totalSpConsumed * (3 + 0.3 * SkillLv), then removes the buff.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Golden_Bell_Shield_Buff)]
	public class Golden_Bell_Shield_BuffOverride : BuffHandler, IBuffCombatAttackBeforeBonusesHandler, IBuffCombatDefenseAfterCalcHandler
	{
		private const int SpDrainPerTick = 65;
		private const float BaseDamageReduction = 0.30f;
		private const float DamageReductionPerLevel = 0.03f;
		private const float MaxDamageReduction = 0.80f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(1000);
			buff.Vars.SetFloat("TotalSpConsumed", 0f);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
			{
				if (!character.TrySpendSp(SpDrainPerTick))
				{
					character.StopBuff(BuffId.Golden_Bell_Shield_Buff);
					return;
				}

				var total = buff.Vars.GetFloat("TotalSpConsumed");
				buff.Vars.SetFloat("TotalSpConsumed", total + SpDrainPerTick);
			}
		}

		public void OnAttackBeforeBonuses(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
		}

		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var skillLevel = buff.NumArg1;
			var reduction = Math.Min(BaseDamageReduction + DamageReductionPerLevel * skillLevel, MaxDamageReduction);

			skillHitResult.Damage *= 1f - reduction;
		}
	}
}
