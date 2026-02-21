using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Evasion, Evasion..
	/// </summary>
	[BuffHandler(BuffId.Avoid)]
	public class Avoid : BuffHandler
	{
		private const int BonusAddDR = 50;
		private const float BonusAddMDEFRate = 0.05f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var addDR = (float)Math.Floor(buff.NumArg1 * BonusAddDR);
			var addMDEFRate = buff.NumArg1 * BonusAddMDEFRate;
			var target = buff.Target;

			target.Properties.Modify(PropertyName.DR_BM, addDR);
			target.Properties.Modify(PropertyName.MDEF_RATE_BM, addMDEFRate);

			if (target is Character character)
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.DR_BM, PropertyName.MDEF_RATE_BM);
		}

		public override void OnEnd(Buff buff)
		{
			var addDR = (float)Math.Floor(buff.NumArg1 * BonusAddDR);
			var addMDEFRate = buff.NumArg1 * BonusAddMDEFRate;
			var target = buff.Target;

			target.Properties.Modify(PropertyName.DR_BM, -addDR);
			target.Properties.Modify(PropertyName.MDEF_RATE_BM, -addMDEFRate);

			if (target is Character character)
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.DR_BM, PropertyName.MDEF_RATE_BM);
		}
	}
}
