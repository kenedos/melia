using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Cleric
{
	/// <summary>
	/// Handler for the Cleric skill Cure.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cleric_PatronSaint)]
	public class Cleric_PatronSaintOverride : IMeleeGroundSkillHandler
	{
		private const int BuffDurationSeconds = 300;
		private const float AbilityBonus = 0.005f;

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var healBonus = 0.15f + skill.Level * 0.03f;

			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Cleric10, out var abilityLevel))
				byAbility += abilityLevel * AbilityBonus;
			healBonus *= byAbility;

			caster.StartBuff(BuffId.PatronSaint_Buff, skill.Level, healBonus, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}
	}
}
