using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Sanctuary: Liberty(Magic), * Increases Magic Defense{nl}* Increase in Movement Speed.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Sanctuary_Paladin42_Buff)]
	public class Sanctuary_Paladin42_BuffOverride : BuffHandler
	{
		public override void OnStart(Buff buff)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
