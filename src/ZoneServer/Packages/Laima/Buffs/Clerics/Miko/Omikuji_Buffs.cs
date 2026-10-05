using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Miko
{
	/// <summary>
	/// Great Blessing: Honor.
	/// Increases physical and magical attack by 0,75% per Omikuji level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Honor_Buff)]
	public class Honor_BuffOverride : BuffHandler
	{
		private const float BonusPerLevel = 0.0075f;
		private const string AppliedBonusVariable = "Omikuji.Honor.AppliedBonus";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var bonus = OmikujiBuffHelper.CalculateRate(buff, BonusPerLevel);

			buff.Target.Properties.Modify(PropertyName.PATK_RATE_BM, bonus);
			buff.Target.Properties.Modify(PropertyName.MATK_RATE_BM, bonus);
			buff.Vars.Set(AppliedBonusVariable, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.Vars.GetFloat(AppliedBonusVariable);

			if (bonus == 0f)
				return;

			buff.Target.Properties.Modify(PropertyName.PATK_RATE_BM, -bonus);
			buff.Target.Properties.Modify(PropertyName.MATK_RATE_BM, -bonus);
		}
	}

	/// <summary>
	/// Great Blessing: Hope.
	/// Increases block penetration, accuracy, and critical rate
	/// by 9,5% per Omikuji level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Wish_Buff)]
	public class Wish_BuffOverride : BuffHandler
	{
		private const float BonusPerLevel = 0.095f;
		private const string AppliedBonusVariable = "Omikuji.Hope.AppliedBonus";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var bonus = OmikujiBuffHelper.CalculateRate(buff, BonusPerLevel);

			buff.Target.Properties.Modify(PropertyName.BLK_BREAK_RATE_BM, bonus);
			buff.Target.Properties.Modify(PropertyName.HR_RATE_BM, bonus);
			buff.Target.Properties.Modify(PropertyName.CRTHR_RATE_BM, bonus);
			buff.Vars.Set(AppliedBonusVariable, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.Vars.GetFloat(AppliedBonusVariable);

			if (bonus == 0f)
				return;

			buff.Target.Properties.Modify(PropertyName.BLK_BREAK_RATE_BM, -bonus);
			buff.Target.Properties.Modify(PropertyName.HR_RATE_BM, -bonus);
			buff.Target.Properties.Modify(PropertyName.CRTHR_RATE_BM, -bonus);
		}
	}

	/// <summary>
	/// Great Blessing: Safety.
	/// Increases block, evasion, and critical resistance
	/// by 9,5% per Omikuji level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Safety_Buff)]
	public class Safety_BuffOverride : BuffHandler
	{
		private const float BonusPerLevel = 0.095f;
		private const string AppliedBonusVariable = "Omikuji.Safety.AppliedBonus";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var bonus = OmikujiBuffHelper.CalculateRate(buff, BonusPerLevel);

			buff.Target.Properties.Modify(PropertyName.BLK_RATE_BM, bonus);
			buff.Target.Properties.Modify(PropertyName.DR_RATE_BM, bonus);
			buff.Target.Properties.Modify(PropertyName.CRTDR_RATE_BM, bonus);
			buff.Vars.Set(AppliedBonusVariable, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.Vars.GetFloat(AppliedBonusVariable);

			if (bonus == 0f)
				return;

			buff.Target.Properties.Modify(PropertyName.BLK_RATE_BM, -bonus);
			buff.Target.Properties.Modify(PropertyName.DR_RATE_BM, -bonus);
			buff.Target.Properties.Modify(PropertyName.CRTDR_RATE_BM, -bonus);
		}
	}

	/// <summary>
	/// Great Blessing: Health.
	/// Reduces physical and magical damage received by 2,5% per level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Healthy_Buff)]
	public class Healthy_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float ReductionPerLevel = 0.025f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			var reduction = OmikujiBuffHelper.CalculateRate(buff, ReductionPerLevel);
			reduction = Math.Min(reduction, 0.90f);

			skillHitInfo.HitInfo.Damage *= 1f - reduction;
		}
	}

	internal static class OmikujiBuffHelper
	{
		private const int MaximumSkillLevel = 10;

		public static float CalculateRate(Buff buff, float ratePerLevel)
		{
			var skillLevel = Math.Min((int)buff.NumArg1, MaximumSkillLevel);
			var enhanceRate = Math.Max(buff.NumArg2, 0f);
			var baseRate = skillLevel * ratePerLevel;

			return baseRate * (1f + enhanceRate);
		}
	}
}
