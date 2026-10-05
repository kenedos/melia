using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Musketeer
{
	[Package("laima")]
	[BuffHandler(BuffId.GroovingMuzzle_UseStack_Buff)]
	public class GroovingMuzzle_UseStack_BuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker != buff.Target || skill == null)
				return;

			if (!IsMusketAttack(skill))
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);
			var damageBonus = 0.10f + skillLevel * 0.05f;

			modifier.FinalDamageMultiplier += damageBonus;
		}

		private static bool IsMusketAttack(Skill skill)
		{
			return skill.Id == SkillId.Musket_Attack || skill.Data.Tags.Has(SkillTag.UseMusketSkill);
		}
	}
}
