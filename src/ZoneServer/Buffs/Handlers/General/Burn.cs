using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Burn buff, which deals damage over time based on
	/// a base value and the number of stacks.
	/// </summary>
	[BuffHandler(BuffId.Burn)]
	public class Burn : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var caster = buff.Caster;
			var target = buff.Target;

			// The caster is required to attribute the damage.
			if (caster == null)
			{
				// The original script has a fallback, but in our system, if the caster is gone,
				// we'll use the target as the damage source for the tick.
				caster = target;
			}

			// The damage is calculated from NumArg2 and the current stack count (OverbuffCounter).
			var baseDamage = buff.NumArg2;
			var stackCount = buff.OverbuffCounter;

			var damage = (baseDamage / 5f) * stackCount;

			if (damage <= 0) return;

			// Apply the damage to the target.
			target.TakeSimpleHit(damage, caster, SkillId.None);
		}
	}
}
