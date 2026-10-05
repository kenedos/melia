using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Buff handler for IceBlast, which deals ice damage in regular intervals
	/// only when the target is also frozen (has Cryomancer_Freeze buff).
	/// </summary>
	/// <remarks>
	/// NumArg2: Snapshotted damage per tick (pre-calculated by skill handler)
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.IceBlast_Debuff)]
	public class IceBlast_DebuffOverride : DamageOverTimeBuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			if (buff.Target.IsDead)
				return;

			// Only deal damage if target is also frozen
			if (!buff.Target.TryGetBuff(BuffId.Cryomancer_Freeze, out var cryomancerFreeze))
				return;

			// Apply snapshotted damage
			base.WhileActive(buff);
		}

		protected override SkillId GetSkillId(Buff buff)
		{
			return SkillId.Cryomancer_IceBlast;
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Ice;
		}
	}
}
