//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 1F - Dungeon Content
//--- Description -----------------------------------------------------------
// Royal Mausoleum dungeon with Relic Memorandum collection points.
// Quests: Fedimian/3004 "Royal Mausoleum Expedition"
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using static Melia.Zone.Scripting.Shortcuts;

public class RoyalMausoleumScript : GeneralScript
{
	protected override void Load()
	{
		// Relic Memorandum Collection Helper
		//-------------------------------------------------------------------------
		void AddRelicMemorandumNpc(int sampleNumber, int x, int y, int direction)
		{
			AddNpc(151052, L("Ancient Scroll Pedestal"), "d_zachariel_32", x, y, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("Fedimian", 3004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An ornate pedestal holds an ancient scroll*{/}"));
					await dialog.Msg(L("The scroll appears to be written by the great scholar Kepeck, documenting a powerful relic."));
					await dialog.Msg(L("You would need official permission from the Grand Archive to remove these documents."));
					return;
				}

				var variableKey = $"Laima.Quests.Fedimian.Quest3004.RelicMemorandum{sampleNumber}";
				var collected = character.Variables.Perm.GetBool(variableKey, false);

				if (collected)
				{
					await dialog.Msg(L("{#666666}*The pedestal is empty*{/}"));
					await dialog.Msg(L("You've already collected the scroll from this location."));
					return;
				}

				await dialog.Msg(L("{#666666}*An ornate pedestal holds an ancient scroll bearing Kepeck's seal*{/}"));
				await dialog.Msg(L("This appears to be one of the Relic Memoranda you're searching for."));

				var result = await character.TimeActions.StartAsync(L("Carefully retrieving scroll..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650657, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650657);
					character.ServerMessage(LF("Relic Memoranda collected: {0}/3", currentCount));

					if (currentCount >= 3)
					{
						character.Quests.CompleteObjective(questId, "collectMemoranda");
						character.ServerMessage(L("{#666666}All Relic Memoranda collected! Return to Grand Archivist Elwen in Fedimian.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Collection cancelled."));
				}
			});
		}

		// Relic Memorandum Collection Points
		//-------------------------------------------------------------------------
		AddRelicMemorandumNpc(1, 188, 743, 45);
		AddRelicMemorandumNpc(2, -537, 18, 90);
		AddRelicMemorandumNpc(3, -1230, -935, 90);
		AddRelicMemorandumNpc(4, 1092, -1158, 180);
		AddRelicMemorandumNpc(5, -1005, 1678, 0);
	}
}
