using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Ignite, Fire lit up because of the oil..
	/// </summary>
	[BuffHandler(BuffId.Warning)]
	public class Warning : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var caster = buff.Caster;
			var target = buff.Target;

			// If the original caster is gone, the target damages itself for the tick.
			if (caster == null)
			{
				caster = target;
			}

			// The base damage value is passed in NumArg1.
			var baseDamage = buff.NumArg1;
			if (baseDamage <= 0) return;

			// Calculate the random damage range: between 1/3 and 1/2 of the base value.
			var minDamage = baseDamage / 3.0;
			var maxDamage = baseDamage / 2.0;

			var damage = (float)(minDamage + (RandomProvider.Get().NextDouble() * (maxDamage - minDamage)));

			if (damage <= 0) return;

			// Apply the damage and send the hit info to the client.
			target.TakeSimpleHit(damage, caster, SkillId.None);
		}
	}
}
