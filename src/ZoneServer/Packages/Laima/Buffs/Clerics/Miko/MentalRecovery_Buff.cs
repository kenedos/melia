using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Miko
{
	/// <summary>
	/// Reduces magic damage received by 2% per Gohei level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.MentalRecovery_Buff)]
	public class MentalRecovery_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float MagicDamageReductionPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo.Skill.Data.ClassType != SkillClassType.Magic)
				return;

			var skillLevel = buff.Vars.GetInt("Gohei.SkillLevel");

			if (skillLevel <= 0)
				return;

			var reduction = skillLevel * MagicDamageReductionPerLevel;
			reduction = Math.Min(reduction, 1f);

			skillHitInfo.HitInfo.Damage *= 1f - reduction;
		}
	}
}
