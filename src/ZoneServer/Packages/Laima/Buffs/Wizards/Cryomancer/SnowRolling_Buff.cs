using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Cryomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.SnowRolling_Buff)]
	public class SnowRolling_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, 65);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
