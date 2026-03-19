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
	/// Handle for the Restoration, periodically restores HP
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Restoration_Buff)]
	public class Restoration_BuffOverride : BuffHandler
	{
		private const int HealTickMs = 3000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = (ICombatEntity)buff.Caster;

			if (caster == null || !caster.TryGetSkill(SkillId.Paladin_Restoration, out var skill))
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

			if (caster == null || !caster.TryGetSkill(SkillId.Paladin_Restoration, out var skill))
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

			return healAmount;
		}
	}
}
