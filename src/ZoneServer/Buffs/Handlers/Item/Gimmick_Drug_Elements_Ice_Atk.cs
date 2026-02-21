using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for GIMMICK_Drug_Elements_Ice_Atk, which modifies Ice Attack.
	/// </summary>
	[BuffHandler(BuffId.GIMMICK_Drug_Elements_Ice_Atk)]
	public class Gimmick_Drug_Elements_Ice_Atk : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.Ice_Atk_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.Ice_Atk_BM, -bonus);
		}
	}
}
