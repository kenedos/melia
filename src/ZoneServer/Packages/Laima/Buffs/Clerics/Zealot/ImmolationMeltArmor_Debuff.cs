using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.ImmolationMeltArmor_Debuff)]
	public class ImmolationMeltArmor_DebuffOverride : BuffHandler
	{
		private const int MinimumAbilityLevel = 1;
		private const int MaximumAbilityLevel = 5;
		private const float ReductionPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			buff.Target.Properties.Invalidate(PropertyName.DEF, PropertyName.MDEF);

			var abilityLevel = Math.Clamp((int)buff.NumArg1, MinimumAbilityLevel, MaximumAbilityLevel);
			var reductionRate = abilityLevel * ReductionPerLevel;
			var currentDefense = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.DEF));
			var currentMagicDefense = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MDEF));
			var defenseReduction = currentDefense * reductionRate;
			var magicDefenseReduction = currentMagicDefense * reductionRate;

			if (defenseReduction > 0f)
				AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -defenseReduction);

			if (magicDefenseReduction > 0f)
				AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, -magicDefenseReduction);

			buff.Target.Properties.Invalidate(PropertyName.DEF, PropertyName.MDEF);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			buff.Target.Properties.Invalidate(PropertyName.DEF, PropertyName.MDEF);
		}
	}
}
