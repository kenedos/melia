using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Appraiser
{
	/// <summary>
	/// Increases accuracy and block penetration.
	/// NumArg1 contains the percentage bonus as a decimal rate.
	/// NumArg2 contains the skill level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.HighMagnifyingGlass_Buff)]
	public class HighMagnifyingGlass_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonusRate = Math.Max(0f, buff.NumArg1);

			AddPropertyModifier(
				buff,
				buff.Target,
				PropertyName.HR_RATE_BM,
				bonusRate
			);

			AddPropertyModifier(
				buff,
				buff.Target,
				PropertyName.BLK_BREAK_RATE_BM,
				bonusRate
			);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(
				buff,
				buff.Target,
				PropertyName.HR_RATE_BM
			);

			RemovePropertyModifier(
				buff,
				buff.Target,
				PropertyName.BLK_BREAK_RATE_BM
			);
		}
	}
}
