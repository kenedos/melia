using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Slow_Debuff)]
	public class Slow_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;

			var speedReduction = (float)Math.Floor(8 + skillLevel * 1.5);

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -speedReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
