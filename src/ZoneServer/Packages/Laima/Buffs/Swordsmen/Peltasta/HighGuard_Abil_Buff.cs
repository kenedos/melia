using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Handler for the High Guard ability buff.
	/// </summary>
	/// <remarks>
	/// One effect of this buff that is currently unimplemented is that,
	/// with it, your "Shield" and "Hard Shield" buffs can't be removed
	/// by enemies. We'll have to see how and where exactly to implement
	/// this. TODO.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.HighGuard_Abil_Buff)]
	public class HighGuard_Abil_BuffOverride : BuffHandler
	{
		private const float MSpdReduction = 8;
		private const float DamageRateReduction = 0.20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -MSpdReduction);
			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, -DamageRateReduction);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
			RemovePropertyModifier(buff, target, PropertyName.PATK_RATE_BM);
		}
	}
}
