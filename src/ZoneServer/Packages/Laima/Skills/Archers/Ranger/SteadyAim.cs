using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
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
	public class Ranger_SteadyAim : ISelfSkillHandler
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

			var criticalRateMultiplier = 0.1f + 0.02f * skill.Level;

			var byAbility = 1f;
			if (caster.TryGetActiveAbility(AbilityId.Ranger14, out var ability))
				byAbility += ability.Level * 0.005f;
			criticalRateMultiplier *= byAbility;

			caster.StartBuff(BuffId.SteadyAim_Buff, skill.Level, criticalRateMultiplier, TimeSpan.FromMilliseconds(1800000f), caster);
		}
	}
}
