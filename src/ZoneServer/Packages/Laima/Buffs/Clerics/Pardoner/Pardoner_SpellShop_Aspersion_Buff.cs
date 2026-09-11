using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Grace: Increase Physical Defense buff sold by a
	/// Spell Shop. Raises the target's physical defense by a rate.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.SpellShop_Aspersion_Buff)]
	public class Pardoner_SpellShop_Aspersion_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
			=> AddPropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM, GetCaptionRatio(buff, 2) / 100f);

		public override void OnEnd(Buff buff)
			=> RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM);
	}
}
