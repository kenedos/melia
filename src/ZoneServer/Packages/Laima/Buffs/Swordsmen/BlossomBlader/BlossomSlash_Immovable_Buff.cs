using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Swordsmen.BlossomBlader
{
	[Package("laima")]
	[BuffHandler(BuffId.BlossomSlash_Immovable_Buff)]
	public class BlossomSlash_Immovable_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler, IBuffOnHitInfoCreatedHandler
	{
		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo.Target != buff.Target)
				return;

			skillHitInfo.HitInfo.Damage = 0;
			skillHitInfo.HitInfo.Type = HitType.Endure;
			skillHitInfo.HitDelay = TimeSpan.Zero;
			skillHitInfo.AniTime = TimeSpan.Zero;
		}
	}
}
