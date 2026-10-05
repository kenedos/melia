using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Swordsman
{
	/// <summary>
	/// Handler for the Swordman skill Pain Barrier.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Swordman_PainBarrier)]
	public class Swordman_PainBarrierOverride : ISelfSkillHandler
	{
		/// <summary>
		/// Handles skill, applying the buffs to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var target = caster;

			var mainDuration = TimeSpan.FromSeconds(10 + skill.Level * 4);
			var immunityDuration = TimeSpan.FromSeconds(3);

			target.StartBuff(BuffId.PainBarrier_Buff, skill.Level, 0, mainDuration, caster);
			target.StartBuff(BuffId.PainBarrierImmune_Buff, skill.Level, 0, immunityDuration, caster);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, target);
		}
	}
}
