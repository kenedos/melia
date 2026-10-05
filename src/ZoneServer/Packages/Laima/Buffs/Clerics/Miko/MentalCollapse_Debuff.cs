using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Miko
{
	/// <summary>
	/// Increases magic damage received by 2% per Gohei level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.MentalCollapse_Debuff)]
	public class MentalCollapse_DebuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float MagicDamageIncreasePerLevel = 0.02f;

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

			var increase = skillLevel * MagicDamageIncreasePerLevel;

			skillHitInfo.HitInfo.Damage *= 1f + increase;
		}
	}
}
