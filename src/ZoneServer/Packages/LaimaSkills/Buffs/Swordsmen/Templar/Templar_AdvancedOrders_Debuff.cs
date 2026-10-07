using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Advanced Orders: Bind debuff, which slows enemies
	/// by the movement speed Advanced Orders gives allies.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.AdvancedOrders_Debuff)]
	public class Templar_AdvancedOrders_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -GetCaptionRatio(buff, 2));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
