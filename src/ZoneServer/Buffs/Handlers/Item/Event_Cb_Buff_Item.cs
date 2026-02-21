using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for Event_Cb_Buff_Item, providing stat bonuses.
	/// </summary>
	[BuffHandler(BuffId.Event_Cb_Buff_Item)]
	public class Event_Cb_Buff_Item : BuffHandler
	{
		private const int MhpBonus = 2000;
		private const int MspBonus = 1000;
		private const int MspdBonus = 1;
		private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(1);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			target.PlayEffect("F_sys_expcard_normal", 2.5f);
			target.Properties.Modify(PropertyName.MHP_BM, MhpBonus);
			target.Properties.Modify(PropertyName.MSP_BM, MspBonus);
			target.Properties.Modify(PropertyName.MSPD_BM, MspdBonus);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.RemainingDuration > MaxDuration)
			{
				buff.IncreaseDuration(MaxDuration);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.Properties.Modify(PropertyName.MHP_BM, -MhpBonus);
			target.Properties.Modify(PropertyName.MSP_BM, -MspBonus);
			target.Properties.Modify(PropertyName.MSPD_BM, -MspdBonus);
		}
	}
}
