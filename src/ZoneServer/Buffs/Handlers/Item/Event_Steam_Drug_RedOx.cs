using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandler(BuffId.Event_Steam_Drug_RedOx)]
	public class Event_Steam_Drug_RedOx : BuffHandler
	{
		private const int SrBonus = 1;
		private const int MspBonus = 1000;
		private const int StaminaRegen = 99999;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			target.Properties.Modify(PropertyName.SR_BM, SrBonus);
			target.Properties.Modify(PropertyName.MSP_BM, MspBonus);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
			{
				character.ModifyStamina(StaminaRegen);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.Properties.Modify(PropertyName.SR_BM, -SrBonus);
			target.Properties.Modify(PropertyName.MSP_BM, -MspBonus);
		}
	}
}
