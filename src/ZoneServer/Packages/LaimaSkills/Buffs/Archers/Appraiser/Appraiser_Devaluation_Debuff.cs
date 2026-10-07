using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Devaluation: Decrease Defense debuff, which lowers
	/// the target's physical and magic defense by 10%.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Devaluation_Debuff)]
	public class Appraiser_Devaluation_DebuffOverride : BuffHandler
	{
		private const float DefenseReduction = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM, -DefenseReduction);
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_RATE_BM, -DefenseReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_RATE_BM);
		}
	}
}
