using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the FireWall Debuff, which reduces fire resistance and deals fire damage over time.
	/// </summary>
	/// <remarks>
	/// NumArg2: Snapshotted damage per tick (calculated on buff application)
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.FireWall_Debuff)]
	public class FireWall_DebuffOverride : DamageOverTimeBuffHandler
	{
		/// <summary>
		/// The amount by which the target's fire resistance is reduced.
		/// </summary>
		/// <remarks>
		/// This value is used as part of a percentage based multiplier.
		/// A reduction of 20 effectively means the target takes 20% more
		/// fire damage.
		/// </remarks>
		private const float FireResistance = -20f;

		/// <summary>
		/// Reduces the target's fire resistance when the buff starts
		/// and snapshots damage for the DoT.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="activationType"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);

			if (buff.Target.Properties.Has(PropertyName.ResFire))
				AddPropertyModifier(buff, buff.Target, PropertyName.ResFire_BM, FireResistance);
			else if (buff.Target.Properties.Has(PropertyName.Fire_Def))
				AddPropertyModifier(buff, buff.Target, PropertyName.Fire_Def_BM, FireResistance);
		}

		/// <summary>
		/// Removes the fire resistance reduction when the buff ends.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.ResFire_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.Fire_Def_BM);
		}

		/// <summary>
		/// Returns the skill ID used for damage calculation.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		protected override SkillId GetSkillId(Buff buff)
		{
			return SkillId.Pyromancer_FireWall;
		}

		/// <summary>
		/// Returns the hit type for the DoT damage.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Fire;
		}
	}
}
