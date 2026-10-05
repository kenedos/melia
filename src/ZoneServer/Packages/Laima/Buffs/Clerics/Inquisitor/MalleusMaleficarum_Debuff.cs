using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Inquisitor
{
	[Package("laima")]
	[BuffHandler(BuffId.MalleusMaleficarum_Debuff)]
	public class MalleusMaleficarum_DebuffOverride : BuffHandler
	{
		private const float StatReductionRate = 0.50f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.INT_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MNA_BM);
			buff.Target.Properties.Invalidate(PropertyName.INT, PropertyName.MNA);
			var intelligence = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.INT));
			var spirit = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MNA));

			AddPropertyModifier(buff, buff.Target, PropertyName.INT_BM, -intelligence * StatReductionRate);
			AddPropertyModifier(buff, buff.Target, PropertyName.MNA_BM, -spirit * StatReductionRate);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.INT_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MNA_BM);
		}
	}
}
