using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the UC_debrave debuff, which reduces the target's
	/// physical and magical attack by 10% per stack.
	/// </summary>
	[BuffHandler(BuffId.UC_debrave)]
	public class UC_debrave : BuffHandler
	{
		private const float AttackReductionPerStack = 0.20f;
		private const float MaxReduction = 0.80f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var reduction = -Math.Min(AttackReductionPerStack * buff.OverbuffCounter, MaxReduction);

			UpdatePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM, reduction);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM, reduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM);
		}
	}
}
