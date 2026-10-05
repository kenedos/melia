using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.Friedenslied_Buff)]
	public class Friedenslied_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo == null || skillHitInfo.Target != buff.Target)
				return;

			skillHitInfo.HitInfo.Damage = 0;
			skillHitInfo.HitInfo.Type = HitType.Endure;
		}
	}
}
