//--- Melia Script ----------------------------------------------------------
// Trade Ticket Item Scripts
//--- Description -----------------------------------------------------------
// Item scripts that exchange items for other items.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class TradeTicketScripts : GeneralScript
{
	/// <summary>
	/// Exchanges a required number of ticket items for a reward item.
	/// Used by Token of Trial items for cosmetics.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="requiredItemClassName">The item class name required for exchange (strArg)</param>
	/// <param name="requiredCount">Number of required items (numArg1)</param>
	/// <param name="rewardCount">Number of reward items to give (numArg2)</param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_GIVE_TRADE_TICKET(Character character, Item item, string requiredItemClassName, float requiredCount, float rewardCount)
	{
		// The strArg2 (reward item) is stored in the item's Script.StrArg2
		var rewardItemClassName = item.Data.Script?.StrArg2 ?? "";
		var required = (int)requiredCount;
		var reward = (int)rewardCount;

		if (string.IsNullOrEmpty(rewardItemClassName))
			return ItemUseResult.Fail;

		if (required <= 0)
			required = 1;
		if (reward <= 0)
			reward = 1;

		// Check if required item exists
		if (!ZoneServer.Instance.Data.ItemDb.TryFind(requiredItemClassName, out _))
		{
			character.SystemMessage("Invalid trade ticket configuration.");
			return ItemUseResult.Fail;
		}

		// Check if player has enough of the required item
		if (!character.HasItem(requiredItemClassName, required))
		{
			character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, "ItemNotEnough", 3);
			return ItemUseResult.OkayNotConsumed;
		}

		// Remove required items and give reward
		character.RemoveItem(requiredItemClassName, required);
		character.AddItem(rewardItemClassName, reward);

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Exchanges pieces/fragments for a complete item (Goddess Ichor).
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="pieceItemClassName">The piece item class name (strArg, e.g., "piece_goddess_icor")</param>
	/// <param name="requiredCount">Number of pieces required (numArg1)</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_GODDESS_ICOR_MISC(Character character, Item item, string pieceItemClassName, float requiredCount, float numArg2)
	{
		// The strArg2 (reward item) is stored in the item's Script.StrArg2
		var rewardItemClassName = item.Data.Script?.StrArg2 ?? "";
		var required = (int)requiredCount;

		if (string.IsNullOrEmpty(rewardItemClassName))
			return ItemUseResult.Fail;

		if (required <= 0)
			required = 1;

		// When using this item, it consumes the item itself (the fragment)
		// and if player has enough fragments, gives the complete item
		if (!character.HasItem(item.Data.ClassName, required))
		{
			character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, "ItemNotEnough", 3);
			return ItemUseResult.OkayNotConsumed;
		}

		// Remove fragments (current item will be consumed by returning Okay)
		// So we remove (required - 1) additional fragments
		if (required > 1)
			character.RemoveItem(item.Data.ClassName, required - 1);

		// Give the complete item
		character.AddItem(rewardItemClassName, 1);

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Event version of Goddess Ichor misc exchange.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="pieceItemClassName">The piece item class name (strArg)</param>
	/// <param name="requiredCount">Number of pieces required (numArg1)</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_GODDESS_ICOR_MISC_EVENT(Character character, Item item, string pieceItemClassName, float requiredCount, float numArg2)
	{
		// Same logic as SCR_USE_GODDESS_ICOR_MISC
		return SCR_USE_GODDESS_ICOR_MISC(character, item, pieceItemClassName, requiredCount, numArg2);
	}
}
