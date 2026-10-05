using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Applies Fear to all targets, a brief stun to non-player targets,
	/// and attack penalties to player characters.
	/// </summary>
	[BuffHandler(BuffId.Fear)]
	public class Fear : BuffHandler
	{
		private const float AttackRatePenalty = -0.3f;
		private const float AttackSpeedPenalty = 250f;
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(1);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			target.AddState(StateType.Fear);
			if (target is not Character)
			{
				target.StartBuff(BuffId.Stun, StunDuration, buff.Caster);
				return;
			}
			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, AttackRatePenalty);
			AddPropertyModifier(buff, target, PropertyName.MATK_RATE_BM, AttackRatePenalty);
			AddPropertyModifier(buff, target, PropertyName.ASPD_BM, AttackSpeedPenalty);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.RemoveState(StateType.Fear);
			if (target is not Character)
				return;
			RemovePropertyModifier(buff, target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.ASPD_BM);
		}
	}
}
