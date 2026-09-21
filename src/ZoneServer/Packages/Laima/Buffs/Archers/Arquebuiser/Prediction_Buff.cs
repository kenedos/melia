using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Prediction Buff, which increases the target's accuracy (hit rate).
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Prediction_Buff)]
	public class Prediction_Buff : BuffHandler
	{
		private const int AccuracyBonusPerLevel = 6;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var hitRateBonus = this.GetHitRateBonus(buff);
			AddPropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM, hitRateBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM);
		}

		// In the client the calculation multiplies the ReinforceAbility
		// directly but a different approach is needed in this case
		// local value = 6 * skill.Level
		// value = value * SCR_REINFORCEABILITY_TOOLTIP(skill)
		private float GetHitRateBonus(Buff buff)
		{
			var skillLevel = buff.NumArg1;
			var accuracyReinforceRateBonus = buff.NumArg2;
			return accuracyReinforceRateBonus > 0 ? skillLevel * AccuracyBonusPerLevel * accuracyReinforceRateBonus : skillLevel * AccuracyBonusPerLevel;
		}
	}
}
