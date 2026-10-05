using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Aukuras buff, periodically restores HP and SP.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Aukuras_Buff)]
	public class Aukuras_BuffOverride : BuffHandler
	{
		private const int HealTickMs = 10000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = (ICombatEntity)buff.Caster;

			if (caster == null || !caster.TryGetSkill(SkillId.Kriwi_Aukuras, out var skill))
			{
				buff.End();
				return;
			}

			var healAmount = this.CalculateHealAmount(caster, buff.Target, skill);
			buff.Target.Heal(healAmount, 0);

			buff.SetUpdateTime(HealTickMs);
		}

		public override void WhileActive(Buff buff)
		{
			var caster = (ICombatEntity)buff.Caster;

			if (caster == null || !caster.TryGetSkill(SkillId.Kriwi_Aukuras, out var skill))
				return;

			var healAmount = this.CalculateHealAmount(caster, buff.Target, skill);
			buff.Target.Heal(healAmount, 0);
		}

		private float CalculateHealAmount(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var healAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
			healAmount *= 1f + SCR_Get_AbilityReinforceRate(skill);

			return healAmount;
		}
	}
}
