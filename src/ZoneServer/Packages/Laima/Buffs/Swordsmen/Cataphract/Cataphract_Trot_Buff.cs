using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Cataphract
{
	/// <summary>
	/// Handler for the Trot Buff, which increases movement speed while riding a companion.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level, used to calculate the speed bonus.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Trot_Buff)]
	public class Trot_BuffOverride : BuffHandler
	{
		private const float BaseMspdBonus = 5f;
		private const float MspdBonusPerLevel = 1f;

		/// <summary>
		/// Applies movement speed bonus when buff activates.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			var mspdBonus = BaseMspdBonus + skillLevel * MspdBonusPerLevel;
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, mspdBonus);
		}

		/// <summary>
		/// Removes movement speed bonus when buff ends.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
		}
	}
}
