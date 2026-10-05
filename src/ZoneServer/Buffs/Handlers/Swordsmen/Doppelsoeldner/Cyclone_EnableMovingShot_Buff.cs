using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handle for the Cyclone Enable Moving shot Buff, which lets you
	/// move during Doppelsoldner_Cyclone
	/// </summary>
	[BuffHandler(BuffId.Cyclone_EnableMovingShot_Buff)]
	public class Cyclone_EnableMovingShot_Buff : BuffHandler
	{
		private const float MovingShotBonusPerLevel = 1.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MovingShot_BM, this.GetMovingShotBonus(buff));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MovingShot_BM);
		}

		private float GetMovingShotBonus(Buff buff)
		{
			var skillLevel = Math.Max(1f, buff.NumArg1);
			return Math.Min(15f, skillLevel * MovingShotBonusPerLevel);
		}
	}
}
