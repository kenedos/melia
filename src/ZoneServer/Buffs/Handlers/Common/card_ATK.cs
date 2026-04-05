using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for card_ATK buff, which temporarily increases Physical Attack.
	/// Used by cards like Glass Mole (Physical Attack +[★ * 1.5]% for duration).
	/// Scans all equipped cards to sum star levels for stacking.
	/// </summary>
	[BuffHandler(BuffId.card_ATK)]
	public class card_ATK : BuffHandler
	{
		private const float MultiplierPerStar = 1.5f / 100f;
		private const string ScriptFunction = "SCR_CARDEFFECT_ADD_BUFF_PC_RATE";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var totalStarLevel = CardBuffHelper.GetTotalStarLevel(character, buff.Id, ScriptFunction);
			var bonus = totalStarLevel * MultiplierPerStar;

			SetPropertyModifier(buff, character, PropertyName.PATK_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
		}
	}
}
