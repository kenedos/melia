using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for Manticen card's movement speed buff, triggered on
	/// stamina potion use.
	/// </summary>
	/// <remarks>
	/// NumArg1: Combined star level * coefficient from all equipped
	/// Manticen cards, used directly as the MSPD bonus.
	/// </remarks>
	[BuffHandler(BuffId.card_MSPD)]
	public class card_MSPD : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
