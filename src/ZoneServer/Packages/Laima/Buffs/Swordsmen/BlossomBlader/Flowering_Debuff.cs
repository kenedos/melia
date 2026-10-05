using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Flowering.
	/// Reduces the target's Accuracy and Evasion through flat and percentage components.
	/// The effect scales with Flowering level and stacks, but is capped for balance.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Flowering_Debuff)]
	public class Flowering_DebuffOverride : BuffHandler
	{
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private const float FlatReductionPerLevelAndStack = 4f;
		private const float PercentageReductionPerLevelAndStack = 0.004f;
		private const float MaximumFlatReduction = 200f;
		private const float MaximumPercentageReduction = 0.20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Remove the previous values before recalculating an updated stack.
			// This prevents the debuff from using its own already-reduced values.
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, MaximumSkillLevel);
			var stacks = Math.Min(Math.Max(buff.OverbuffCounter, 1), buff.MaxOverbuffCount);
			var enhanceMultiplier = this.GetEnhanceMultiplier(buff);
			var scaling = skillLevel * stacks * enhanceMultiplier;
			var flatReduction = Math.Min(scaling * FlatReductionPerLevelAndStack, MaximumFlatReduction);
			var percentageReduction = Math.Min(scaling * PercentageReductionPerLevelAndStack, MaximumPercentageReduction);

			var currentAccuracy = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.HR));
			var currentEvasion = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.DR));
			var accuracyReduction = Math.Min(currentAccuracy, flatReduction + currentAccuracy * percentageReduction);
			var evasionReduction = Math.Min(currentEvasion, flatReduction + currentEvasion * percentageReduction);

			AddPropertyModifier(buff, buff.Target, PropertyName.HR_BM, -accuracyReduction);
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, -evasionReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
		}

		private float GetEnhanceMultiplier(Buff buff)
		{
			var enhanceLevel = Math.Clamp((int)buff.NumArg2, 0, MaximumEnhanceLevel);
			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
