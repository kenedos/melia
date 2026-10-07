using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid skill Lycanthropy, which turns the Druid into a
	/// wolf, or back into themselves when used as one.
	/// </summary>
	/// <remarks>
	/// Lycanthropy: Human Form turns them into a 10 second hybrid instead,
	/// and [Arts] Lycanthropy: Wolf's Spirit keeps the wolf for 30 minutes.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Druid_Lycanthropy)]
	public class Druid_LycanthropyOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan HumanFormDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan WolfSpiritDuration = TimeSpan.FromMinutes(30);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster.IsBuffActive(BuffId.Lycanthropy_Buff) || caster.IsBuffActive(BuffId.Lycanthropy_Half_Buff))
			{
				caster.StopBuff(BuffId.Lycanthropy_Buff, BuffId.Lycanthropy_Half_Buff);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			if (caster.IsAbilityActive(AbilityId.Druid14) && !caster.IsAbilityActive(AbilityId.Druid27))
			{
				caster.StartBuff(BuffId.Lycanthropy_Half_Buff, skill.Level, 0, HumanFormDuration, caster, skill.Id);
				return;
			}

			var duration = caster.IsAbilityActive(AbilityId.Druid27) ? WolfSpiritDuration : skill.Properties.CaptionTime;
			caster.StartBuff(BuffId.Lycanthropy_Buff, skill.Level, 0, duration, caster, skill.Id);
		}
	}
}
