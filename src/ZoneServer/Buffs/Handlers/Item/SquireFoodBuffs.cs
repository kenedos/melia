using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandlerAttribute(BuffId.squire_food1_buff)]
	public class SquireFoodSaladBuff : BuffHandler
	{
		private const float BaseBonusRate = 0.075f;
		private const float BonusRatePerLevel = 0.025f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var level = Math.Max(1f, buff.NumArg1);
			var baseMaxHp = target.Properties.GetFloat(PropertyName.MHP) - target.Properties.GetFloat(PropertyName.MHP_BM);
			var bonusRate = BaseBonusRate + level * BonusRatePerLevel;
			var bonus = MathF.Floor(baseMaxHp * bonusRate);
			AddPropertyModifier(buff, target, PropertyName.MHP_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MHP_BM);
		}
	}

	[BuffHandlerAttribute(BuffId.squire_food2_buff)]
	public class SquireFoodSandwichBuff : BuffHandler
	{
		private const float BaseBonusRate = 0.075f;
		private const float BonusRatePerLevel = 0.025f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var level = Math.Max(1f, buff.NumArg1);
			var baseMaxSp = target.Properties.GetFloat(PropertyName.MSP) - target.Properties.GetFloat(PropertyName.MSP_BM);
			var bonusRate = BaseBonusRate + level * BonusRatePerLevel;
			var bonus = MathF.Floor(baseMaxSp * bonusRate);
			AddPropertyModifier(buff, target, PropertyName.MSP_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSP_BM);
		}
	}

	[BuffHandlerAttribute(BuffId.squire_food3_buff)]
	public class SquireFoodSoupBuff : BuffHandler
	{
		private const float BonusPerLevel = 1000f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = Math.Max(1f, buff.NumArg1);
			var bonus = level * BonusPerLevel;
			AddPropertyModifier(buff, buff.Target, PropertyName.RHPTIME_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.RHPTIME_BM);
		}
	}

	[BuffHandlerAttribute(BuffId.squire_food4_buff)]
	public class SquireFoodYogurtBuff : BuffHandler
	{
		private const float BonusPerLevel = 1000f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = Math.Max(1f, buff.NumArg1);
			var bonus = level * BonusPerLevel;
			AddPropertyModifier(buff, buff.Target, PropertyName.RSPTIME_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.RSPTIME_BM);
		}
	}

	[BuffHandlerAttribute(BuffId.squire_food5_buff)]
	public class SquireFoodBbqSkewerBuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = Math.Max(1f, buff.NumArg1);
			var bonus = Math.Max(1f, MathF.Floor(0.5f + level * 0.5f));
			AddPropertyModifier(buff, buff.Target, PropertyName.SR_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SR_BM);
		}
	}

	[BuffHandlerAttribute(BuffId.squire_food6_buff)]
	public class SquireFoodChampagneBuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = Math.Max(1f, buff.NumArg1);
			var bonus = Math.Max(1f, MathF.Floor(0.5f + level * 0.5f));
			AddPropertyModifier(buff, buff.Target, PropertyName.SDR_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SDR_BM);
		}
	}
}
