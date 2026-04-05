using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for Manticen card's movement speed buff, triggered on
	/// stamina potion use. Scans all equipped Manticen cards to sum
	/// star levels for stacking.
	/// </summary>
	[BuffHandler(BuffId.card_MSPD)]
	public class card_MSPD : BuffHandler
	{
		private const float Coefficient = 0.3f;
		private const string ScriptFunction = "SCR_CARDEFFECT_ADD_BUFF_PC_PLUS_MANTICEN";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var totalStarLevel = CardBuffHelper.GetTotalStarLevel(character, buff.Id, ScriptFunction);
			var bonus = totalStarLevel * Coefficient;

			SetPropertyModifier(buff, character, PropertyName.MSPD_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
