using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandler(BuffId.Premium_Fortunecookie_2)]
	public class Premium_Fortunecookie_2 : Premium_Fortunecookie_Base
	{
		private const int MspdBonus = 2;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Properties.Modify(PropertyName.MSPD_BM, MspdBonus);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.Properties.Modify(PropertyName.MSPD_BM, -MspdBonus);
		}
	}
}
