using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

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
		private const float FireResistance = -20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Call base to snapshot damage
			base.OnActivate(buff, activationType);

			// Apply fire resistance debuff
			if (buff.Target is Character)
				buff.Target.Properties.Modify(PropertyName.ResFire_BM, FireResistance);
			else
				buff.Target.Properties.Modify(PropertyName.Fire_Def, FireResistance);
		}

		public override void OnEnd(Buff buff)
		{
			// Remove fire resistance debuff
			if (buff.Target is Character)
				buff.Target.Properties.Modify(PropertyName.ResFire_BM, -FireResistance);
			else
				buff.Target.Properties.Modify(PropertyName.Fire_Def, -FireResistance);
		}

		protected override SkillId GetSkillId(Buff buff)
		{
			return SkillId.Pyromancer_FireWall;
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Fire;
		}
	}
}
