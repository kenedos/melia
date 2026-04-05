using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for card_MDEF buff, which temporarily increases Magical Defense.
	/// Used by Frogola card (Magical Defense +[★ * 3]% for 20s on HP potion use).
	/// Scans all equipped cards to sum star levels for stacking.
	/// </summary>
	[BuffHandler(BuffId.card_MDEF)]
	public class card_MDEF : BuffHandler
	{
		private const float MultiplierPerStar = 3f / 100f;
		private const string ScriptFunction = "SCR_CARDEFFECT_ADD_BUFF_PC_RATE";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var totalStarLevel = CardBuffHelper.GetTotalStarLevel(character, buff.Id, ScriptFunction);
			var bonus = totalStarLevel * MultiplierPerStar;

			SetPropertyModifier(buff, character, PropertyName.MDEF_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_RATE_BM);
		}
	}
}
