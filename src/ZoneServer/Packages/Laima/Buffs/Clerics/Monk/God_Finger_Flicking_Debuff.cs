using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Monk
{
	/// <summary>
	/// Handle for the God_Finger_Flicking_Debuff, which reduces the target's Max SP by 50%.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.God_Finger_Flicking_Debuff)]
	public class God_Finger_Flicking_DebuffOverride : BuffHandler
	{
		private const float MspReductionRate = 0.5f;
		private const string MspReductionVar = "GodFinger.MspReduction";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var maxSp = target.Properties.GetFloat(PropertyName.MSP);
			var spReduction = maxSp * MspReductionRate;

			buff.Vars.SetFloat(MspReductionVar, spReduction);
			target.Properties.Modify(PropertyName.MSP_BM, -spReduction);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Vars.TryGetFloat(MspReductionVar, out var spReduction))
			{
				target.Properties.Modify(PropertyName.MSP_BM, spReduction);
			}
		}
	}
}
