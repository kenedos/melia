using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Rage buff, which increases offensive stats at the cost of defensive stats.
	/// </summary>
	[BuffHandler(BuffId.Rage)]
	public class Rage : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			var primaryRate = 0.5f + skillLevel * 0.25f;
			var mspdRate = 0.2f + skillLevel * 0.1f;

			var intPenalty = target.Properties.GetFloat(PropertyName.INT) * primaryRate;
			var defPenalty = target.Properties.GetFloat(PropertyName.DEF) * primaryRate;
			var strBonus = target.Properties.GetFloat(PropertyName.STR) * primaryRate;
			var atkBonus = target.Properties.GetFloat(PropertyName.MINMATK) * primaryRate;
			var mspdBonus = target.Properties.GetFloat(PropertyName.MSPD) * mspdRate;

			AddPropertyModifier(buff, target, PropertyName.INT_BM, -intPenalty);
			AddPropertyModifier(buff, target, PropertyName.DEF_BM, -defPenalty);
			AddPropertyModifier(buff, target, PropertyName.STR_BM, strBonus);
			AddPropertyModifier(buff, target, PropertyName.ATK_BM, atkBonus);
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, mspdBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.INT_BM);
			RemovePropertyModifier(buff, target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, target, PropertyName.STR_BM);
			RemovePropertyModifier(buff, target, PropertyName.ATK_BM);
			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
		}
	}
}
