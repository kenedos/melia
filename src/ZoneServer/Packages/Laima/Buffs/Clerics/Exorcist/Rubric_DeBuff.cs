using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Exorcist
{
	[Package("laima")]
	[BuffHandler(BuffId.Rubric_DeBuff)]
	public class Rubric_DeBuffOverride : BuffHandler
	{
		private const float MovementSpeedReductionRate = 0.30f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			var movementSpeed = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MSPD));
			var reduction = movementSpeed * MovementSpeedReductionRate;

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -reduction);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
