using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Inquisitor
{
	[Package("laima")]
	[BuffHandler(BuffId.BreastRipper_Debuff)]
	public class BreastRipper_DebuffOverride : BuffHandler
	{
		private const int MaximumAbilityLevel = 5;
		private const float DefenseReductionPerLevel = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			buff.Target.Properties.Invalidate(PropertyName.DEF, PropertyName.MDEF);
			var level = Math.Clamp((int)buff.NumArg1, 1, MaximumAbilityLevel);
			var reductionRate = level * DefenseReductionPerLevel;
			var physicalDefense = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.DEF));
			var magicDefense = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MDEF));
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -physicalDefense * reductionRate);
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, -magicDefense * reductionRate);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}
}
