using System;
using System.Linq;
using System.Reflection.Emit;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Scan.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_Scan)]
	public class Ranger_ScanOverride : IGroundSkillHandler
	{
		private const float BaseCritResistReduce = 20f;

		// How much accuracy is converted to crit resist multiplier reduction
		// per skill level.
		// At skill level 10, reduces crit resist by 50% of caster's accuracy.
		private const float CritResistMultiplierPerLevel = 0.05f;

		/// <summary>
		/// Handles skill, applying a debuff to the target
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (target == null)
			{
				caster.ServerMessage(Localization.Get("No target specified."));
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, null);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var duration = TimeSpan.FromSeconds(60);
			var accuracy = caster.Properties.GetFloat(PropertyName.HR);
			var critResistReduce = BaseCritResistReduce + (accuracy * skill.Level * CritResistMultiplierPerLevel);

			var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
			critResistReduce *= 1f + SCR_Get_AbilityReinforceRate(skill);

			target.StartBuff(BuffId.Ranger_Scan_Debuff, skill.Level, critResistReduce, duration, caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, null);
		}
	}
}
