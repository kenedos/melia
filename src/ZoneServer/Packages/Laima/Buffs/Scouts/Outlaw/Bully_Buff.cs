using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Scouts.OutLaw
{
	/// <summary>
	/// Buff handler for Bully Buff, which increases evasion
	/// adds thread and reduces attack
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Bully_Buff)]
	public class Bully_BuffOverride : BuffHandler
	{
		private const float DrBuffRateBase = 0.20f;
		private const float DrBuffRatePerLevel = 0.03f;
		private const float BaseAttackReduction = 0.2f;
		private const float AttackReductionPerLevel = 0.01f;

		/// <summary>
		/// Starts buff, increasing dodge rate.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var dr = buff.Target.Properties.GetFloat(PropertyName.DR);
			var skillLevel = buff.NumArg1;
			var rate = DrBuffRateBase + DrBuffRatePerLevel * skillLevel;
			var attackReduction = BaseAttackReduction - (AttackReductionPerLevel * skillLevel);
			attackReduction = Math.Max(0, attackReduction);

			var byAbility = 1f;
			if (buff.Target.TryGetActiveAbility(AbilityId.Outlaw11, out var ability))
				byAbility += ability.Level * 0.005f;
			rate *= byAbility;

			AddPropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM, rate);
			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM, -attackReduction);
			AddPropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM, -attackReduction);
		}

		/// <summary>
		/// Ends the buff, resetting dodge rate.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM);
		}
	}
}
