using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.HamelnNagetier_Buff)]
	public class HamelnNagetier_BuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
				PiedPiperHamelnNagetierHelper.RemoveMice(character);
		}
	}
}
