using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for drop_inceaseMoney, which stores values for loot calculation.
	/// </summary>
	[BuffHandler(BuffId.drop_inceaseMoney)]
	public class drop_inceaseMoney : BuffHandler
	{
		private const string VarMoneyCount = "Melia.Drop.MoneyCount";
		private const string VarMoneyRatio = "Melia.Drop.MoneyRatio";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// This buff's purpose is to hold values for the drop system to read.
			// It stores its arguments into its own variables.
			buff.Vars.SetFloat(VarMoneyCount, buff.NumArg1);
			buff.Vars.SetFloat(VarMoneyRatio, buff.NumArg2);
		}
	}
}
