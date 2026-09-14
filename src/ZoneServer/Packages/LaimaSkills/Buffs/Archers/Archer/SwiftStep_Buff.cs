using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Swift Step Buff, which increases the target's evasion.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.SwiftStep_Buff)]
	public class SwiftStep_Buff : BuffHandler
	{

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// TODO: Move this to WhileActive, because target's evasion
			// could change and we need to update move speed accordingly
			var drBonus = this.GetDodgeRateBonus(buff);
			var movingShotBonus = this.GetMovingShotBonus(buff);

			AddPropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM, drBonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.MovingShot_BM, movingShotBonus);

			if (buff.Target is Character character)
				Send.ZC_MOVE_SPEED(character);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MovingShot_BM);

			if (buff.Target is Character character)
				Send.ZC_MOVE_SPEED(character);
		}

		private float GetDodgeRateBonus(Buff buff)
			=> GetCaptionRatio(buff, 1) / 100f;

		private float GetMovingShotBonus(Buff buff)
		{
			var baseValue = 0.2f;
			var skillLevel = buff.NumArg1;
			var evasion = buff.Target.Properties.GetFloat(PropertyName.DR);

			return Math.Max(baseValue, baseValue + (evasion / 100) * (GetCaptionRatio(buff, 2) / 100f));
		}
	}
}
