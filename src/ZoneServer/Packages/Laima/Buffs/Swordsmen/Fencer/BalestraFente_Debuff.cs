using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Reduces the target's Critical Resistance after Balestra Fente hits.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level.
	/// NumArg2: Flat Critical Resistance reduction.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.BalestraFente_Debuff)]
	public class BalestraFente_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -buff.NumArg2);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}
}
