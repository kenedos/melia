using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Packages.Laima.Buffs.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Reduces damage received by 10% after Flash is used while StartUp is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.StartUp_Blossomblader16_Buff)]
	public class StartUp_Blossomblader16_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float DamageReduction = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			skillHitInfo.HitInfo.Damage *= 1f - DamageReduction;
		}
	}
}
