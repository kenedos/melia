using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzer Reiter skill Evasive Action.
	/// SkillId: 51005
	/// ClassName: Schwarzereiter_EvasiveAction
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_EvasiveAction)]
	public class SchwarzerReiter_EvasiveActionOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster is not Character character || !character.IsRiding)
			{
				caster.ServerMessage(Localization.Get("You must be mounted on a companion."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			// Base Evasive Action duration.
			var duration = TimeSpan.FromMilliseconds(300000);

			// [Arts] Evasive Action: Duration increases the buff duration.
			if (character.IsAbilityActive(AbilityId.Schwarzereiter33))
			{
				duration += TimeSpan.FromMilliseconds(100000);
			}

			// Apply Evasive Action buff.
			caster.StartBuff(
				BuffId.EvasiveAction_Buff,
				skill.Level,
				0f,
				duration,
				caster,
				skill.Id);

			caster.SetAttackState(false);
		}
	}
}
