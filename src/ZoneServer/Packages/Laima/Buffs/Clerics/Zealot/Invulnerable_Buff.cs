using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.Invulnerable_Buff)]
	public class Invulnerable_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumAccuracyBonusPercent = 10f;
		private const float MaximumAccuracyBonusPercent = 20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			buff.Target.Properties.Invalidate(PropertyName.HR);

			var skillLevel = Math.Clamp(buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var accuracyBonusPercent = MinimumAccuracyBonusPercent + (skillLevel - MinimumSkillLevel) * (MaximumAccuracyBonusPercent - MinimumAccuracyBonusPercent) / (MaximumSkillLevel - MinimumSkillLevel);
			var currentAccuracy = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.HR));
			var accuracyBonus = currentAccuracy * accuracyBonusPercent / 100f;

			if (accuracyBonus > 0f)
				AddPropertyModifier(buff, buff.Target, PropertyName.HR_BM, accuracyBonus);

			buff.Target.Properties.Invalidate(PropertyName.HR);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			buff.Target.Properties.Invalidate(PropertyName.HR);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}
	}
}
