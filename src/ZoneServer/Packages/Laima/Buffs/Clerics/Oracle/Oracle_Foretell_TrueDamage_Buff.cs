using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Foretell TrueDamage buff, which nullifies all
	/// incoming true damage while active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Foretell_TrueDamage_Buff)]
	public class Oracle_Foretell_TrueDamage_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		// TODO: Implement damage nullification for TrueDamage/AbsoluteDamage
		// skill types once those attack type constants are mapped.
	}
}
