using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Rodelero
{
	/// <summary>
	/// Handler for ShieldCharge_Buff.
	/// Increases block while channeling Shield Charge.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ShieldCharge_Buff)]
	public class ShieldCharge_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;
			var blockBonus = 250 + 25 * skillLevel;
			var blockRate = 0.15f + 0.01f * skillLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, blockBonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_RATE_BM, blockRate);

			buff.Target.Properties.Modify(PropertyName.Jumpable, -1);
			buff.Target.Properties.Modify(PropertyName.DashRun, 0);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_RATE_BM);

			buff.Target.Properties.Modify(PropertyName.Jumpable, 1);
			buff.Target.Properties.Modify(PropertyName.DashRun, 1);
		}
	}
}
