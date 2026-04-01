using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_InfectiousDisease_Debuff.
	/// Deals snapshot damage over time to the infected player and spreads
	/// to nearby players who don't already have the debuff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_InfectiousDisease_Debuff)]
	public class Mythic_InfectiousDisease_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const float SpreadRange = 100f;
		private const int SpreadDurationSec = 8;
		private const float OverbuffDamageRate = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Scale damage by overbuff counter: +3% per stack
			var counter = buff.OverbuffCounter;
			if (counter > 0)
				buff.NumArg2 = (int)(buff.NumArg2 * (1f + counter * OverbuffDamageRate));

			base.OnActivate(buff, activationType);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}

		protected override void OnDamageTick(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead || buff.Target.Map == null)
				return;

			var target = buff.Target;
			var snapshotDamage = buff.NumArg2;

			var nearbyPlayers = target.Map.GetActorsInRange<Character>(target.Position, SpreadRange);
			foreach (var nearby in nearbyPlayers)
			{
				if (nearby.IsDead || nearby == target)
					continue;

				if (!nearby.IsBuffActive(BuffId.Mythic_InfectiousDisease_Debuff))
					nearby.StartBuff(BuffId.Mythic_InfectiousDisease_Debuff, 1, snapshotDamage, TimeSpan.FromSeconds(SpreadDurationSec), buff.Caster);
			}
		}
	}
}
