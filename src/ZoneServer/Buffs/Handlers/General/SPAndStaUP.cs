using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for SPAndStaUP, which improves SP and Stamina recovery.
	/// </summary>
	[BuffHandler(BuffId.SPAndStaUP)]
	public class SPAndStaUP : BuffHandler
	{
		private const int RspBonus = 2;
		private const int RspTimeBonus = -2500; // Negative value means faster recovery
		private const int RStaBonus = 1000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			// Apply the recovery bonuses.
			AddPropertyModifier(buff, target, PropertyName.RSP_BM, RspBonus);
			AddPropertyModifier(buff, target, PropertyName.RSPTIME_BM, RspTimeBonus);
			AddPropertyModifier(buff, target, PropertyName.RSta_BM, RStaBonus);

			// The original script calls ResetRSPTime. This likely resets the
			// timer in the component responsible for HP/SP recovery ticks.
			if (target.Components.TryGet<RecoveryComponent>(out var recoveryComponent))
			{
				recoveryComponent.ResetSpRecoveryTime();
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Revert the recovery bonuses.
			RemovePropertyModifier(buff, target, PropertyName.RSP_BM);
			RemovePropertyModifier(buff, target, PropertyName.RSPTIME_BM);
			RemovePropertyModifier(buff, target, PropertyName.RSta_BM);
		}
	}
}
