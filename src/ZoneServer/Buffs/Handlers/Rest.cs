using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the Rest Buff (aka Sitting), which increases the
	/// target's HP, SP, and Stamina recovery.
	/// </summary>
	[BuffHandler(BuffId.Rest)]
	public class Rest_Buff : BuffHandler
	{
		private const int StaminaRecoveryBonus = 10;
		private const float HpSpRecoveryRate = 0.01f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_SHOW_EMOTICON(buff.Target, "I_emo_startuscharge", buff.Duration);
			buff.Target.Properties.Modify(PropertyName.RSta_BM, StaminaRecoveryBonus);
		}

		public override void OnEnd(Buff buff)
		{
			Send.ZC_SHOW_EMOTICON(buff.Target, "", TimeSpan.Zero);
			buff.Target.Properties.Modify(PropertyName.RSta_BM, -StaminaRecoveryBonus);
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			var maxHp = target.Properties.GetFloat(PropertyName.MHP);
			var maxSp = target.Properties.GetFloat(PropertyName.MSP);

			if (target.Hp < maxHp)
				target.Heal(maxHp * HpSpRecoveryRate, 0);

			if (target is Character character && character.Sp < maxSp)
				character.Heal(0, maxSp * HpSpRecoveryRate);
		}
	}
}
