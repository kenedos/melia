using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for Immolation: Melt Armor, which lowers physical and
	/// magic defense by 1% per ability level.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.ImmolationMeltArmor_Debuff)]
	public class Zealot_ImmolationMeltArmor_DebuffOverride : BuffHandler
	{
		private const float ReductionPerLevel = 0.01f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var reduction = buff.NumArg1 * ReductionPerLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM, -reduction);
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_RATE_BM, -reduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_RATE_BM);
		}
	}
}
