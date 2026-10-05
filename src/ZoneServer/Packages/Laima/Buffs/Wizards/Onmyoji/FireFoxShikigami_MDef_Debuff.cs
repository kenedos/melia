using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Wizards.Onmyoji
{
	[Package("laima")]
	[BuffHandler(BuffId.FireFoxShikigami_MDef_Debuff)]
	public class FireFoxShikigami_MDef_DebuffOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 50;
		private const float ReductionPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			buff.Target.Properties.Invalidate(PropertyName.MDEF);

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var reductionRate = skillLevel * ReductionPerLevel;
			var currentMagicDefense = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MDEF));
			var magicDefenseReduction = currentMagicDefense * reductionRate;

			if (magicDefenseReduction > 0f)
				AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, -magicDefenseReduction);

			buff.Target.Properties.Invalidate(PropertyName.MDEF);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			buff.Target.Properties.Invalidate(PropertyName.MDEF);
		}
	}
}
