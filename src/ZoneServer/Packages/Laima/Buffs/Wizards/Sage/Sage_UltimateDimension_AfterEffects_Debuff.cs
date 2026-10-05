using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Packages.Laima.Buffs.Wizards.Sage
{
	/// <summary>
	/// Ultimate Dimension: After Effects.
	/// Stores one snapshotted Ultimate Dimension hit as a DoT instance.
	/// Additional Ultimate Dimension casts add additional damage instances
	/// through DamageOverTimeBuffHandler.
	///
	/// NumArg1: Ultimate Dimension skill level.
	/// NumArg2: Snapshotted damage per tick.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.UltimateDimension_Debuff)]
	public class Sage_UltimateDimensionAfterEffectsDebuffOverride : DamageOverTimeBuffHandler
	{
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Normal;
		}
	}
}
