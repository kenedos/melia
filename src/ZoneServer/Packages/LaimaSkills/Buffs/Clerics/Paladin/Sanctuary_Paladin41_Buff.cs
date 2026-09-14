using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Sanctuary: Liberty(Physical), * Increases Physical Defense{nl}* Immune to debuff.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Sanctuary_Paladin41_Buff)]
	public class Sanctuary_Paladin41_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
