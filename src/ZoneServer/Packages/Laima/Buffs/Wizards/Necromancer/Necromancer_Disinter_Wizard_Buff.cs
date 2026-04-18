using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handle for the Sacrifice: Skeleton Mage, Immune to Debuff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Disinter_Wizard_Buff)]
	public class Necromancer_Disinter_Wizard_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
