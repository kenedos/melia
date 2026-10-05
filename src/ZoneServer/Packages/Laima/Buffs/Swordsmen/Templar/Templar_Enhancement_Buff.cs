using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[BuffHandler(BuffId.Templar_Enhancement_Buff)]
	public class Templar_Enhancement_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			var criticalRateBonusRate = Math.Max(0f, buff.NumArg2);

			if (criticalRateBonusRate <= 0f)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM);

			var currentCriticalRate = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.CRTHR));
			var criticalRateBonus = currentCriticalRate * criticalRateBonusRate;

			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM, criticalRateBonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM, criticalRateBonusRate);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM);
		}
	}
}
