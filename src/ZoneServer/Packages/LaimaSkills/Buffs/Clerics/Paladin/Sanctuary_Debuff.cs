using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Sanctuary: Weaken, Cannot receive Sanctuary: Liberty..
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Sanctuary_Debuff)]
	public class Sanctuary_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
