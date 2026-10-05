using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Miko
{
	/// <summary>
	/// Increases outgoing damage by 4% per Clap level,
	/// up to 40% at level 10.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Kasiwade_Buff)]
	public class Kasiwade_BuffOverride : BuffHandler, IBuffOnAttackHitInfoCreatedHandler
	{
		private const int MaximumSkillLevel = 10;
		private const float DamageBonusPerLevel = 0.025f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnAttackHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			var skillLevel = Math.Min((int)buff.NumArg1, MaximumSkillLevel);

			if (skillLevel <= 0)
				return;

			var damageBonus = skillLevel * DamageBonusPerLevel;

			skillHitInfo.HitInfo.Damage *= 1f + damageBonus;
		}
	}
}
