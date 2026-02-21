using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Restoration, restores HP
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Restoration_Buff)]
	public class Restoration_BuffOverride : BuffHandler
	{
		private const string RHPModPropName = PropertyName.RHP_BM;
		private const string RSPModPropName = PropertyName.RSP_BM;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = (ICombatEntity)buff.Caster;

			if (caster != null && caster.TryGetSkill(SkillId.Paladin_Restoration, out var skill))
			{
				// Calculate heal amount based on skill factor
				var healAmount = this.CalculateHealAmount(caster, target, skill);
				buff.Target.Heal(healAmount, 0);
			}
			else
			{
				buff.End();
			}
		}

		/// <summary>
		/// Calculates the heal amount for the target based on skill factor.
		/// </summary>
		private float CalculateHealAmount(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var healAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			return healAmount;
		}
	}
}
