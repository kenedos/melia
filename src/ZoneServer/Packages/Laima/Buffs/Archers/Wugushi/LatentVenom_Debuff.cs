using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Latent Venom Debuff, which ticks poison damage every second.
	/// </summary>
	/// <remarks>
	/// NumArg1: None
	/// NumArg2: Snapshotted damage per tick (calculated on buff application)
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.LatentVenom_Debuff)]
	public class LatentVenom_DebuffOverride : DamageOverTimeBuffHandler
	{
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}
	}
}
