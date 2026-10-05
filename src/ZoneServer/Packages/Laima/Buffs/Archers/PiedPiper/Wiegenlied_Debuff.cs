using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.Wiegenlied_Debuff)]
	public class Wiegenlied_DebuffOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var effectMultiplier = Math.Clamp(buff.NumArg2, 0.5f, 1f);
			var accuracyReductionRate = this.GetAccuracyReductionRate(skillLevel) * effectMultiplier;
			var accuracyReduction = buff.Target.Properties.GetFloat(PropertyName.HR) * accuracyReductionRate;

			AddPropertyModifier(buff, buff.Target, PropertyName.HR_BM, -accuracyReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
		}

		private float GetAccuracyReductionRate(int skillLevel)
		{
			var reductionPercent = 9f + ((skillLevel - 1) * (16f / 9f));
			return reductionPercent / 100f;
		}
	}
}
