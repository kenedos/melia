using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handle for the Aiming, Aiming.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Aiming_Hawk_Buff)]
	public class Falconer_Aiming_Hawk_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
