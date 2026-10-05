using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.Allegro_Buff)]
	public class Allegro_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character)
				return;

			var moveSpeedBonus = Math.Max(buff.NumArg1, 0f);

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_Bonus, moveSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_Bonus);
		}
	}
}
