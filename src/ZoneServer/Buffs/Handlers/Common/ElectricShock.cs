using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handle for the Slow, Decreased movement speed.
	/// </summary>
	[BuffHandler(BuffId.ElectricShock)]
	public class ElectricShock : BuffHandler
	{
		private const float MspdRate = 2f;
		private const float DefReductionRate = 0.3f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var mspdModValue = buff.NumArg1 * MspdRate;
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -mspdModValue);

			var currentDef = target.Properties.GetFloat(PropertyName.DEF);
			var defModValue = currentDef * DefReductionRate;
			AddPropertyModifier(buff, target, PropertyName.DEF_BM, -defModValue);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}
	}
}
