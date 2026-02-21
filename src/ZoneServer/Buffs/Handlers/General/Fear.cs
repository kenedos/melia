using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Fear debuff, which reduces attack power and speed,
	/// and briefly stuns non-player characters.
	/// </summary>
	[BuffHandler(BuffId.Fear)]
	public class Fear : BuffHandler
	{
		private const float AttackRatePenalty = -0.3f; // 30% reduction
		private const float AttackSpeedPenalty = 250f;  // Positive value slows attack speed
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(1);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;

			// If the target is not a player character, apply a short Stun.
			if (target is not Character)
			{
				target.StartBuff(BuffId.Stun, StunDuration, caster);
			}

			// Apply the stat penalties using the helper methods.
			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, AttackRatePenalty);
			AddPropertyModifier(buff, target, PropertyName.MATK_RATE_BM, AttackRatePenalty);
			//AddPropertyModifier(buff, target, PropertyName.ATKSPEED_BM, AttackSpeedPenalty);
			AddPropertyModifier(buff, target, PropertyName.ASPD_BM, AttackSpeedPenalty);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Revert the stat penalties using the helper methods.
			RemovePropertyModifier(buff, target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_RATE_BM);
			//RemovePropertyModifier(buff, target, PropertyName.ATKSPEED_BM);
			RemovePropertyModifier(buff, target, PropertyName.ASPD_BM);
		}
	}
}
