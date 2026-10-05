//--- Melia Script ----------------------------------------------------------
// Event Items
//--- Description -----------------------------------------------------------
// This script handles the event items, such as the Event_Penalty buff and other event-related functionalities.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class EventItemScript : GeneralScript
{
	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_GODDESS_STATUE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var random = new Random();
		var distance = (float)(random.NextDouble() * 30 + 10);
		var spawnPosition = character.Position.GetRelative(character.Direction, distance);

		var npc = AddNpc(
			owner: character,
			className: "farm47_statue_zemina_small",
			position: spawnPosition,
			direction: -45,
			dialogFuncName: "GODDESS_STATUE_EV",
			tacticsFuncName: "GODDESS_STATUE_EV",
			lifeTime: TimeSpan.FromMinutes(1)
		);

		if (npc != null)
		{
			character.Buffs.Start(BuffId.Event_GoddessStatueExp, 0, 0, TimeSpan.FromMinutes(30), character);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, ScpArgMsg("Goddess_Statue3"), 5);
			return ItemUseResult.Okay;
		}

		character.ServerMessage("Cannot place the statue here.");
		return ItemUseResult.Fail;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_WHITECUBE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("Hat_628297", 1, "WHITE_HAIRACC");
		return ItemUseResult.Okay;
	}
}
