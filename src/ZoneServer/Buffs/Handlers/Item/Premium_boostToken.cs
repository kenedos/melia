using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the EXP Tome buffs, which raise the base and class
	/// experience earned by the percentage the tome grants (NumArg1).
	/// </summary>
	[BuffHandler(BuffId.Premium_boostToken, BuffId.Premium_boostToken02, BuffId.Premium_boostToken03, BuffId.Premium_boostToken04, BuffId.Premium_boostToken05, BuffId.Premium_boostToken06)]
	public class Premium_boostToken : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			SetPropertyModifier(buff, buff.Target, PropertyName.BonusExp_BM, buff.NumArg1);
			SetPropertyModifier(buff, buff.Target, PropertyName.BonusJobExp_BM, buff.NumArg1);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BonusExp_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.BonusJobExp_BM);
		}
	}
}
