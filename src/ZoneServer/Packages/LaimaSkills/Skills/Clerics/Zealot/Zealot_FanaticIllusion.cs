using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Zealot skill Fanatic Illusion, which wraps the
	/// Zealot in lightning that shocks nearby enemies for 15 seconds.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Zealot_FanaticIllusion)]
	public class Zealot_FanaticIllusionOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			caster.StartBuff(BuffId.FanaticIllusion_Buff, skill.Level, 0, Duration, caster, skill.Id);
		}
	}
}
