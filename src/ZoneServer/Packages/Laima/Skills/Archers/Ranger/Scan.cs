using System;
using System.Linq;
using System.Reflection.Emit;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Scan.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_Scan)]
	public class Ranger_ScanOverride : IMeleeGroundSkillHandler
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
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			var target = targets.FirstOrDefault();
			if (target == null)
			{
				caster.ServerMessage(Localization.Get("No target specified."));
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

			var byAbility = 1f;
			if (caster.TryGetActiveAbility(AbilityId.Ranger51, out var ability))
				byAbility += ability.Level * 0.005f;
			critResistReduce *= byAbility;

			target.StartBuff(BuffId.Ranger_Scan_Debuff, skill.Level, critResistReduce, duration, caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, null);
		}
	}
}
