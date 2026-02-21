using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandler(BuffId.Premium_Fortunecookie_4)]
	public class Premium_Fortunecookie_4 : Premium_Fortunecookie_Base
	{
		private const int MspdBonus = 4;
		private const int MhpBonus = 1000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Properties.Modify(PropertyName.MSPD_BM, MspdBonus);
			buff.Target.Properties.Modify(PropertyName.MHP_BM, MhpBonus);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.Properties.Modify(PropertyName.MSPD_BM, -MspdBonus);
			buff.Target.Properties.Modify(PropertyName.MHP_BM, -MhpBonus);
		}
	}
}
