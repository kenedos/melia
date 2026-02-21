using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle aggressor, increases accuracy at cost of evasion
	/// </summary>
	[BuffHandler(BuffId.Mon_Aggressor_Buff)]
	public class Aggressor_Buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var minMATK = buff.Target.Properties.GetFloat(PropertyName.MINMATK);
			var maxMATK = buff.Target.Properties.GetFloat(PropertyName.MAXMATK);
			var matk = ((minMATK + maxMATK) / 2) / 15;

			// Apply the defense modifier
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, -matk);
			AddPropertyModifier(buff, buff.Target, PropertyName.HR_BM, matk);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove the defense modifier
			RemovePropertyModifier(buff, target, PropertyName.DR_BM);
			RemovePropertyModifier(buff, target, PropertyName.HR_BM);
		}
	}
}
