using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Packages.Laima.Buffs.Wizards.Sage
{
	[Package("laima")]
	[BuffHandler(BuffId.MissileHole_MSPD_Buff)]
	public class Sage_MissileHole_MSPD_BuffOverride : BuffHandler
	{
		private const float MovementSpeedBonus = 15f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MovementSpeedBonus);
			Send.ZC_MOVE_SPEED(buff.Target);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
			Send.ZC_MOVE_SPEED(buff.Target);
		}
	}
}
