using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Hunter
{
	/// <summary>
	/// Handler for the Pet_Heal_After buff, which increases companion
	/// movement speed after healing ends.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Pet_Heal_After)]
	public class Pet_Heal_AfterOverride : BuffHandler
	{
		private const float BaseMspdBonus = 10f;
		private const float MspdBonusPerLevel = 2f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;
			var mspdBonus = BaseMspdBonus + skillLevel * MspdBonusPerLevel;
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, mspdBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
