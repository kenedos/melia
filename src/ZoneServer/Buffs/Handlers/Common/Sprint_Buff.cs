using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handler for the Sprint buff. Increases movement speed.
	/// </summary>
	[BuffHandler(BuffId.Sprint_Buff)]
	public class Sprint_Buff : BuffHandler
	{
		private const float MSPDBase = 10f;
		private const float MSPDPerLevel = 2f;
		private const float MSPDCap = 50f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;
			var mspdBonus = Math.Min(MSPDBase + MSPDPerLevel * skillLevel, MSPDCap);

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, mspdBonus);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
