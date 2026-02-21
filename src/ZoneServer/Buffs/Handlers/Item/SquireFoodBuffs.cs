using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for squire_food1_buff (Salad), which increases Max HP by a percentage.
	/// </summary>
	[BuffHandler(BuffId.squire_food1_buff)]
	public class squire_food1_buff : BuffHandler
	{
		private const string VarMhpBonus = "Melia.SquireFood.MHPBonus";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			// The bonus is based on MaxHP before any other MHP_BM bonuses are applied.
			var baseMaxHp = target.Properties.GetFloat(PropertyName.MHP) - target.Properties.GetFloat(PropertyName.MHP_BM);
			var bonusRate = 0.075f + skillLevel * 0.025f;
			var mhpBonus = (float)Math.Floor(baseMaxHp * bonusRate);

			buff.Vars.SetFloat(VarMhpBonus, mhpBonus);
			target.Properties.Modify(PropertyName.MHP_BM, mhpBonus);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Vars.TryGetFloat(VarMhpBonus, out var mhpBonus))
			{
				buff.Target.Properties.Modify(PropertyName.MHP_BM, -mhpBonus);
			}
		}
	}

	/// <summary>
	/// Handle for squire_food2_buff (Sandwich), which increases Max SP by a percentage.
	/// </summary>
	[BuffHandler(BuffId.squire_food2_buff)]
	public class squire_food2_buff : BuffHandler
	{
		private const string VarMspBonus = "Melia.SquireFood.MSPBonus";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			// The bonus is based on MaxSP before any other MSP_BM bonuses are applied.
			var baseMaxSp = target.Properties.GetFloat(PropertyName.MSP) - target.Properties.GetFloat(PropertyName.MSP_BM);
			var bonusRate = 0.075f + skillLevel * 0.025f;
			var mspBonus = (float)Math.Floor(baseMaxSp * bonusRate);

			buff.Vars.SetFloat(VarMspBonus, mspBonus);
			target.Properties.Modify(PropertyName.MSP_BM, mspBonus);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Vars.TryGetFloat(VarMspBonus, out var mspBonus))
			{
				buff.Target.Properties.Modify(PropertyName.MSP_BM, -mspBonus);
			}
		}
	}

	/// <summary>
	/// Handle for squire_food3_buff (Soup), which increases HP recovery time.
	/// </summary>
	[BuffHandler(BuffId.squire_food3_buff)]
	public class squire_food3_buff : BuffHandler
	{
		private const float RhpTimeBonusPerLevel = 1000f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1 * RhpTimeBonusPerLevel;
			buff.Target.Properties.Modify(PropertyName.RHPTIME_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1 * RhpTimeBonusPerLevel;
			buff.Target.Properties.Modify(PropertyName.RHPTIME_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for squire_food4_buff (Yogurt), which increases SP recovery time.
	/// </summary>
	[BuffHandler(BuffId.squire_food4_buff)]
	public class squire_food4_buff : BuffHandler
	{
		private const float RspTimeBonusPerLevel = 1000f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1 * RspTimeBonusPerLevel;
			buff.Target.Properties.Modify(PropertyName.RSPTIME_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1 * RspTimeBonusPerLevel;
			buff.Target.Properties.Modify(PropertyName.RSPTIME_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for squire_food5_buff (BBQ), which increases Skill Range.
	/// </summary>
	[BuffHandler(BuffId.squire_food5_buff)]
	public class squire_food5_buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;
			var srBonus = (float)Math.Floor(0.5f + (skillLevel - 5) * 0.5f);
			buff.Target.Properties.Modify(PropertyName.SR_BM, srBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var skillLevel = buff.NumArg1;
			var srBonus = (float)Math.Floor(0.5f + (skillLevel - 5) * 0.5f);
			buff.Target.Properties.Modify(PropertyName.SR_BM, -srBonus);
		}
	}

	/// <summary>
	/// Handle for squire_food6_buff (Champagne), which increases AoE Defense Ratio.
	/// </summary>
	[BuffHandler(BuffId.squire_food6_buff)]
	public class squire_food6_buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;
			var sdrBonus = (float)Math.Floor(0.5f + (skillLevel - 5) * 0.5f);
			buff.Target.Properties.Modify(PropertyName.SDR_BM, sdrBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var skillLevel = buff.NumArg1;
			var sdrBonus = (float)Math.Floor(0.5f + (skillLevel - 5) * 0.5f);
			buff.Target.Properties.Modify(PropertyName.SDR_BM, -sdrBonus);
		}
	}
}
