using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Swordsman.Barbarian
{
	/// <summary>
	/// Handler for Warcry_Debuff, which increases physical attack and reduces physical defense.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Warcry_Debuff)]
	public class Warcry_DebuffOverride : BuffHandler
	{
		private const float PAtkBonusRate = 0.30f; // +30% PATK
		private const float DefPenaltyRate = 0.20f; // -20% DEF

		/// <summary>
		/// Starts buff, increasing PATK and reducing DEF
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			// Calculate bonuses based on target's current stats
			var patkBonus = target.Properties.GetFloat(PropertyName.PATK) * PAtkBonusRate;
			var defPenalty = target.Properties.GetFloat(PropertyName.DEF) * DefPenaltyRate;

			// Increase enemy PATK by 30%
			AddPropertyModifier(buff, target, PropertyName.PATK_BM, patkBonus);

			// Decrease enemy DEF by 20%
			AddPropertyModifier(buff, target, PropertyName.DEF_BM, -defPenalty);
		}

		/// <summary>
		/// Ends the buff, resetting PATK and DEF.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}
	}
}
