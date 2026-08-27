using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Steady Aim.
	/// Handler for the Ranger skill Full Throttle
	/// </summary>
	/// <remarks>
	/// This skill repurposes the skill id of an earlier skill
	/// called Steady Aim, though the effect is completely different.
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_SteadyAim)]
	public class Ranger_SteadyAimOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			var criticalRateMultiplier = skill.Properties.GetFloat(PropertyName.CaptionRatio) / 100f;

			caster.StartBuff(BuffId.SteadyAim_Buff, skill.Level, criticalRateMultiplier, skill.Properties.CaptionTime, caster, skill.Id);
		}
	}
}
