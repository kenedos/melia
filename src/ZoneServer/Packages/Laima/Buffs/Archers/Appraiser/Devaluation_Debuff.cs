using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.Appraiser
{
	[Package("laima")]
	[BuffHandler(BuffId.Devaluation_Debuff)]
	public class Devaluation_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var reductionRate = Math.Clamp(buff.NumArg1, 0f, 1f);
			var defenseReduction = buff.Target.Properties.GetFloat(PropertyName.DEF) * reductionRate;
			var magicDefenseReduction = buff.Target.Properties.GetFloat(PropertyName.MDEF) * reductionRate;
			var criticalResistanceReduction = buff.Target.Properties.GetFloat(PropertyName.CRTDR) * reductionRate;

			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -defenseReduction);
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, -magicDefenseReduction);
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -criticalResistanceReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}
}
