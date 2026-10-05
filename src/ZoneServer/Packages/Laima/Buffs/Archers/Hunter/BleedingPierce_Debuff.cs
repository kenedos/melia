using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the BleedingPierce Debuff, which deals bleeding damage over time.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Snapshotted damage per tick (calculated on buff application)
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.BleedingPierce_Debuff)]
	public class BleedingPierce_DebuffOverride : DamageOverTimeBuffHandler
	{
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Bleeding;
		}
	}
}
