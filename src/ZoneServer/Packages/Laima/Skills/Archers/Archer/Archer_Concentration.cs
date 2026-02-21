using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Archer
{
	/// <summary>
	/// Handler for the Archer skill Concentration.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Archer_Concentration)]
	public class Archer_ConcentrationOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, applying buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var duration = TimeSpan.FromSeconds(300);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Archer39, out _))
				duration = TimeSpan.FromSeconds(5);

			// Due to the dynamic duration this skill has,
			// we need to always remove the buff before applying it.
			caster.RemoveBuff(BuffId.Concentration_Buff);
			caster.StartBuff(BuffId.Concentration_Buff, skill.Level, 0, duration, caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}
	}
}
