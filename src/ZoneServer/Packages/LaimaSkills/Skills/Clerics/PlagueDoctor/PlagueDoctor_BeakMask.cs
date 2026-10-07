using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Plague Doctor skill Beak Mask, which puts on the
	/// mask that blocks removable debuffs.
	/// </summary>
	/// <remarks>
	/// The skill's cooldown starts when the mask comes off, and [Arts] Bird
	/// Beak Mask: White Mask puts on the White Mask instead.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PlagueDoctor_BeakMask)]
	public class PlagueDoctor_BeakMaskOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster.IsBuffActive(BuffId.BeakMask_Buff) || caster.IsBuffActive(BuffId.WhiteBeakMask_Buff))
			{
				caster.ServerMessage(Localization.Get("You are already wearing the mask."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.RemoveCooldown(skill.Id);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var maskBuffId = caster.IsAbilityActive(AbilityId.PlagueDoctor23) ? BuffId.WhiteBeakMask_Buff : BuffId.BeakMask_Buff;
			caster.StartBuff(maskBuffId, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
		}
	}
}
