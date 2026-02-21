using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.SwellBody_VIBORA_RapidGrowth_Buff)]
	public class SwellBody_VIBORA_RapidGrowth_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
