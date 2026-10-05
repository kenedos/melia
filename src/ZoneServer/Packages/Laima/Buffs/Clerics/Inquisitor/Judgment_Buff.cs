using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Inquisitor
{
	[Package("laima")]
	[BuffHandler(BuffId.Judgment_Buff)]
	public class Judgment_BuffOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumCriticalRateBonus = 0.10f;
		private const float MaximumCriticalRateBonus = 0.20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var levelProgress = (skillLevel - MinimumSkillLevel) / (float)(MaximumSkillLevel - MinimumSkillLevel);
			var bonusRate = MinimumCriticalRateBonus + (MaximumCriticalRateBonus - MinimumCriticalRateBonus) * levelProgress;
			var currentCriticalRate = buff.Target.Properties.GetFloat(PropertyName.CRTHR);
			var criticalRateBonus = currentCriticalRate * bonusRate;

			buff.NumArg2 = criticalRateBonus;
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM, criticalRateBonus);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
