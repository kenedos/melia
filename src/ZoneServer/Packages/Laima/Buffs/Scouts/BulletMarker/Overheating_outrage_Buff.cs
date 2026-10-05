using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Bulletmarker
{
	[Package("laima")]
	[BuffHandler(BuffId.Overheating_outrage_Buff)]
	public class Overheating_outrage_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is not Character caster)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);
			var attackSpeedBonus = (float)Math.Floor(100f + 10f * skillLevel);

			AddPropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM, attackSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM);
		}
	}
}
