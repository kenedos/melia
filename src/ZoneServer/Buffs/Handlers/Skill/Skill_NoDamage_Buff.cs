using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for Skill_NoDamage_Buff.
	/// Makes the target completely untargetable and immune to damage.
	/// Used by skills like Annihilation to provide invincibility during animation.
	/// </summary>
	/// <remarks>
	/// Uses two layers of protection:
	/// 1. LockType.GetDamaged prevents receiving damage from new attacks
	/// 2. IBuffCombatDefenseAfterCalc sets damage to 0 for any attacks that
	///    slip through (e.g., already-targeted skills, delayed damage)
	/// </remarks>
	[BuffHandler(BuffId.Skill_NoDamage_Buff)]
	public class Skill_NoDamage_Buff : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		/// <summary>
		/// Called when the buff is activated, makes target untargetable.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Lock(LockType.GetDamaged);
		}

		/// <summary>
		/// Called when the buff ends, restores target to normal state.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			buff.Target.Unlock(LockType.GetDamaged);
		}

		/// <summary>
		/// Sets damage to 0 for any incoming attacks while buff is active.
		/// This catches attacks from entities that already had a target reference
		/// before the lock was applied.
		/// </summary>
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			skillHitResult.Damage = 0;
			skillHitResult.Result = HitResultType.Dodge;
		}
	}
}
