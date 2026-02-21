using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Recovery, Recover HP, SP by 5% per second.
	/// </summary>
	[BuffHandler(BuffId.Rejuvenation)]
	public class Rejuvenation : BuffHandler
	{
		private const float HpSpRecoveryRate = 0.05f;

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
