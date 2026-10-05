using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[BuffHandler(BuffId.VitalityBanner_Buff)]
	public class Templar_VitalityBanner_BuffOverride : BuffHandler
	{
		private const int HealingIntervalMilliseconds = 2000;
		private const float MaximumHpHealingRatePerLevel = 0.005f;
		private const int MaximumSkillLevel = 10;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(HealingIntervalMilliseconds);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, MaximumSkillLevel);
			var maximumHp = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MHP));
			var healingRate = skillLevel * MaximumHpHealingRatePerLevel;
			var healingAmount = Math.Max(1f, maximumHp * healingRate);

			buff.Target.Heal(healingAmount, 0);
		}
	}
}
