using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Trigger chill, Cold Air Attack Damage Increase..
	/// Handle for the ColdDetonation buff, which provides a stacking bonus to Ice damage.
	/// </summary>
	/// <remarks>
	/// NumArg1: The base bonus value per stack.
	/// The total bonus is NumArg1 * OverbuffCounter.
	/// </remarks>
	[BuffHandler(BuffId.ColdDetonation)]
	public class ColdDetonation : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonusPerStack = buff.NumArg1;
			var totalStacks = buff.OverbuffCounter + 1;
			var totalIceBonus = bonusPerStack * totalStacks;

			SetPropertyModifier(buff, buff.Target, PropertyName.Ice_Atk_BM, totalIceBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.Ice_Atk_BM);
		}
	}
}
