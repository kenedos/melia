using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for GIMMICK_Drug_Elements_Lightning_Atk, which modifies Lightning Attack.
	/// </summary>
	[BuffHandler(BuffId.GIMMICK_Drug_Elements_Lightning_Atk)]
	public class Gimmick_Drug_Elements_Lightning_Atk : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.Lightning_Atk_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.Lightning_Atk_BM, -bonus);
		}
	}
}
