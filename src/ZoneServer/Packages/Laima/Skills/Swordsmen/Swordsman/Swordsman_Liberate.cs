using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Swordsman
{
	/// <summary>
	/// Handler for the Swordman skill Liberate.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Swordman_Liberate)]
	public class Swordman_LiberateOverride : ISelfSkillHandler
	{
		private const float ThreatPerLevel = 100f;

		/// <summary>
		/// Handles skill, applying the buff to the caster.
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

			// Normal duration
			var duration = TimeSpan.FromSeconds(30 * skill.Level);
			var abilityFlag = 0;

			if (caster.TryGetActiveAbilityLevel(AbilityId.Swordman31, out var a))
			{
				duration = TimeSpan.FromSeconds(12);
				abilityFlag = (int)AbilityId.Swordman31;
			}


			if (caster.TryGetActiveAbilityLevel(AbilityId.Swordman32, out var b))
			{
				duration = TimeSpan.FromSeconds(6);
				abilityFlag = (int)AbilityId.Swordman32;
			}

			target.StartBuff(BuffId.Liberate_Buff, skill.Level, abilityFlag, duration, caster);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, target);
		}
	}
}
