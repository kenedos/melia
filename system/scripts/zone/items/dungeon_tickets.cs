//--- Melia Script ----------------------------------------------------------
// Dungeon Entry Ticket Scripts
//--- Description -----------------------------------------------------------
// Item scripts that add dungeon entry counts and weekly entrance tickets.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class DungeonTicketScripts : GeneralScript
{
	/// <summary>
	/// Adds 1 to the weekly dungeon entered count for a specific dungeon type.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="dungeonTypeId">The dungeon type ID (e.g., 821, 822, 819, 823, 826)</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_IndunWeeklyEnteredCount_NumberArg1(Character character, Item item, string strArg, float dungeonTypeId, float numArg2)
	{
		var dungeonId = (int)dungeonTypeId;
		var propertyName = $"IndunWeeklyEnteredCount_{dungeonId}";

		character.ModifyAccountProperty(propertyName, 1);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, $"IndunEnterTicketUse");

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Adds a weekly entrance ticket for a specific dungeon.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg">Entry type name (e.g., "Weekly_Entrance_Ticket")</param>
	/// <param name="dungeonTypeId">The dungeon type ID (e.g., 812)</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_Weekly_Entrance_Ticket(Character character, Item item, string strArg, float dungeonTypeId, float numArg2)
	{
		var dungeonId = (int)dungeonTypeId;
		var propertyName = $"IndunWeeklyEnteredCount_{dungeonId}";

		character.ModifyAccountProperty(propertyName, 1);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, $"IndunEnterTicketUse");

		return ItemUseResult.Okay;
	}
}
