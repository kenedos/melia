//--- Melia Script ----------------------------------------------------------
// Crystal Mine 1F Quest NPCs
//--- Description -----------------------------------------------------------
// Vaidotas and the purifiers of the first floor, and the quests that run the
// player around the mine repairing them.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine01QuestNpcsScript : GeneralScript
{
	private readonly static QuestId ToTheMines = new QuestId(8082);
	private readonly static QuestId Alchemist = new QuestId(4461);
	private readonly static QuestId Crystal2 = new QuestId(4463);
	private readonly static QuestId Crystal8 = new QuestId(4469);
	private readonly static QuestId Crystal9 = new QuestId(4470);
	private readonly static QuestId Crystal10 = new QuestId(4471);
	private readonly static QuestId Crystal13 = new QuestId(4474);
	private readonly static QuestId Crystal18 = new QuestId(4479);
	private readonly static QuestId Crystal19 = new QuestId(4480);
	private readonly static QuestId Mine2Alchemist = new QuestId(4467);

	protected override void Load()
	{
		// Vaidotas
		//-------------------------------------------------------------------------
		AddNpc(20110, L("[Alchemist Master]{nl}Vaidotas"), "MINE_1_ALCHEMIST", "d_cmine_01", -1188, -1799, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Vaidotas"));
			dialog.SetPortrait("Dlg_port_ALCHEMIST_1");

			if (character.Quests.IsActive(ToTheMines) && !character.Quests.IsCompletable(ToTheMines))
			{
				await dialog.Msg(L("The Vubbes the blast drew in are still on the road. Deal with them first."));
				return;
			}

			if (character.Quests.IsActive(ToTheMines))
			{
				await dialog.Msg(L("You made it through. I told you the explosives would be enough."));
				await dialog.Msg(L("This is the Crystal Mine. The air down here is what killed every rescue party before us."));
				await dialog.CompleteQuest(ToTheMines);
				return;
			}

			if (character.Quests.IsActive(Alchemist) && character.Quests.IsCompletable(Alchemist))
			{
				await dialog.Msg(L("The air is clearing already. Every purifier on this floor is turning again."));
				await dialog.Msg(L("Let us go down to the second floor. The fumes there will be far worse."));
				await dialog.CompleteQuest(Alchemist);
				character.Quests.Start(Mine2Alchemist);
				return;
			}

			if (!character.Quests.Has(Alchemist) && character.Quests.MeetsPrerequisites(Alchemist))
			{
				await dialog.Msg(L("Do you know the legend of Cunningham?"));
				await dialog.Msg(L("It's a legend about how a great demon was trapped in the Crystal Mine in the past."));

				var answer = await dialog.SelectQuestOffer(Alchemist, L("The toxic fumes have to be cleared before we can go any deeper."),
					Option(L("How do I repair a purifier?"), "accept"),
					Option(L("That seems difficult"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Alchemist);
					character.Inventory.Add(ItemId.CMINE_COMPASS_ITEM, 1);
					await dialog.Msg(L("The purifier can be easily fixed by anyone, but it may be hard to find the parts needed."));
					await dialog.Msg(L("In that case, use the compass that I gave you to search for them."));
				}

				return;
			}

			if (character.Quests.IsActive(Alchemist))
			{
				await dialog.Msg(L("If you can't find the parts needed to repair the purifier, use the compass I gave you."));
				await dialog.Msg(L("It'll help you find them."));
				return;
			}

			await dialog.Msg(L("The Crystal Mine has swallowed better miners than either of us. Keep the purifiers running."));
		});

		// Entrance Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("Entrance Purifier"), "MINE_1_PURIFY_1", "d_cmine_01", -882, -1246, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Entrance Purifier"));

			if (character.Quests.IsActive(Crystal2))
			{
				if (!character.Quests.IsCompletable(Crystal2))
				{
					await dialog.Msg(L("Retrieve Purifier Parts from the Vubbe with a question mark on its head."));
					return;
				}

				var repaired = await character.TimeActions.StartAsync(L("Repairing the purifier..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (repaired != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The repair is complete. The Entrance Purifier is running again."));
				await dialog.CompleteQuest(Crystal2);
				CheckPurifiersRepaired(character);
				return;
			}

			if (!character.Quests.Has(Crystal2) && character.Quests.MeetsPrerequisites(Crystal2))
			{
				var answer = await dialog.SelectQuestOffer(Crystal2, L("The purifier stands silent. A panel hangs open where a part should sit."),
					Option(L("Inspect the purifier"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var inspected = await character.TimeActions.StartAsync(L("Checking the purifier..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

					if (inspected != TimeActionResult.Completed)
						return;

					character.Quests.Start(Crystal2);
				}

				return;
			}

			await dialog.Msg(L("The Entrance Purifier hums steadily."));
		});

		// Entrance Purifier Parts
		//-------------------------------------------------------------------------
		AddNpc(151015, L("Entrance Purifier Parts"), "MINE_1_CRYSTAL_4", "d_cmine_01", -1036, -1461, 90, async dialog =>
		{
			await TakePurifierPart(dialog);
		});

		AddNpc(151015, L("Entrance Purifier Parts"), "MINE_1_CRYSTAL_4_2", "d_cmine_01", -743, -86, 90, async dialog =>
		{
			await TakePurifierPart(dialog);
		});

		// Central Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("Central Purifier"), "MINE_1_PURIFY_5", "d_cmine_01", 33, -44, 5, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Central Purifier"));

			if (character.Quests.IsActive(Crystal9))
			{
				if (!character.Quests.IsCompletable(Crystal9))
				{
					await dialog.Msg(L("The Mine Compass points to District 4. Go there and find the spare part."));
					return;
				}

				var replaced = await character.TimeActions.StartAsync(L("Replacing the part..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (replaced != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The Central Purifier is repaired. It is working properly again."));
				await dialog.CompleteQuest(Crystal9);
				CheckPurifiersRepaired(character);
				return;
			}

			if (!character.Quests.Has(Crystal8) && character.Quests.MeetsPrerequisites(Crystal8))
			{
				var answer = await dialog.SelectQuestOffer(Crystal8, L("The Central Purifier has seized. One of its valves will not turn."),
					Option(L("Open the valve"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var opened = await character.TimeActions.StartAsync(L("Opening the valve..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

					if (opened != TimeActionResult.Completed)
						return;

					character.Quests.Start(Crystal8);
					character.Quests.CompleteObjective(Crystal8, "openValve");
					await dialog.CompleteQuest(Crystal8);
					character.Quests.Start(Crystal9);
				}

				return;
			}

			await dialog.Msg(L("The Central Purifier hums steadily."));
		});

		// Spare Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("Spare Purifier"), "MINE_1_CRYSTAL_9_DEVICE", "d_cmine_01", 1418, -914, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Spare Purifier"));

			if (character.Quests.IsActive(Crystal9) && !character.Quests.IsCompletable(Crystal9))
			{
				await dialog.Msg(L("Defeat the Bearkaras that appeared in front of the Spare Purifier."));
				character.Quests.ClearQuestTrack(Crystal9);
				return;
			}

			await dialog.Msg(L("The spare purifier has been stripped for parts."));
		});

		// Crystal Basket
		//-------------------------------------------------------------------------
		AddNpc(147453, L("Crystal Basket"), "MINE_1_CRYSTAL_10", "d_cmine_01", 748, 104, 355, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Crystal Basket"));

			if (character.Quests.IsActive(Crystal10))
			{
				if (!character.Quests.IsCompletable(Crystal10))
				{
					await dialog.Msg(L("The Cyclops is still loose in the tunnel."));
					character.Quests.ReplayQuestTrack(Crystal10);
					return;
				}

				await dialog.Msg(L("The Cyclops is down. The miners can come back for their crystals."));
				await dialog.CompleteQuest(Crystal10);
				return;
			}

			if (!character.Quests.Has(Crystal10) && character.Quests.MeetsPrerequisites(Crystal10))
			{
				var answer = await dialog.SelectQuestOffer(Crystal10, L("The basket has been tipped over and the crystals scattered. Something heavy did this."),
					Option(L("Look around"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var searched = await character.TimeActions.StartAsync(L("Looking the basket over..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

					if (searched != TimeActionResult.Completed)
						return;

					character.Quests.Start(Crystal10);
				}

				return;
			}

			await dialog.Msg(L("A basket for carrying crystals out of the mine."));
		});

		// Passage Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("Passage Purifier"), "MINE_1_PURIFY_7", "d_cmine_01", -342, 992, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Passage Purifier"));

			if (character.Quests.IsActive(Crystal19))
			{
				if (!character.Quests.IsCompletable(Crystal19))
				{
					await dialog.Msg(L("You have the part. Fit it and start the purifier."));
					return;
				}

				var repairedPassage = await character.TimeActions.StartAsync(L("Repairing the purifier..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (repairedPassage != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The Passage Purifier is repaired. It is running again."));
				await dialog.CompleteQuest(Crystal19);
				CheckPurifiersRepaired(character);
				return;
			}

			if (character.Quests.IsActive(Crystal18) && character.Quests.IsCompletable(Crystal18))
			{
				await dialog.Msg(L("You have the part. Return it to the Passage Purifier and start the repair."));
				await dialog.CompleteQuest(Crystal18);
				character.Quests.Start(Crystal19);
				character.Quests.CompleteObjective(Crystal19, "fitPart");
				return;
			}

			if (!character.Quests.Has(Crystal13) && character.Quests.MeetsPrerequisites(Crystal13))
			{
				var answer = await dialog.SelectQuestOffer(Crystal13, L("The Passage Purifier is cold. A part has been torn out of its housing."),
					Option(L("Open the valve"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var opened = await character.TimeActions.StartAsync(L("Opening the valve..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

					if (opened != TimeActionResult.Completed)
						return;

					character.Quests.Start(Crystal13);
					character.Quests.CompleteObjective(Crystal13, "openValve");
					await dialog.CompleteQuest(Crystal13);
					character.Quests.Start(Crystal18);
				}

				return;
			}

			if (character.Quests.IsActive(Crystal18))
			{
				await dialog.Msg(L("The compass points to District 6. Search District 6 for the part."));
				return;
			}

			await dialog.Msg(L("The Passage Purifier hums steadily."));
		});

		// Mine Lift
		//-------------------------------------------------------------------------
		AddNpc(151008, L("Mine Lift"), "MINE_1_ELEVATOR", "d_cmine_01", -1501, 707, 344, async dialog =>
		{
			await dialog.Msg(L("The lift creaks on its cable, waiting for a load."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("MINE_1_CRYSTAL_9_TRIGGER", "d_cmine_01", 1287, -994, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Crystal9) && !character.Quests.IsCompletable(Crystal9))
				character.Quests.StartQuestTrack(Crystal9);

			await Task.CompletedTask;
		});

		AddQuestTrigger("MINE_1_CRYSTAL_18_TRIGGER", "d_cmine_01", -1126, 428, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Crystal18) && !character.Quests.IsCompletable(Crystal18))
				character.Quests.StartQuestTrack(Crystal18);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Marks the floor's main quest done once all three purifiers run again.
	/// </summary>
	private static void CheckPurifiersRepaired(Character character)
	{
		if (!character.Quests.IsActive(Alchemist))
			return;

		if (!character.Quests.HasCompleted(Crystal2) || !character.Quests.HasCompleted(Crystal9) || !character.Quests.HasCompleted(Crystal19))
			return;

		character.Quests.CompleteObjective(Alchemist, "repairPurifiers");
	}

	/// <summary>
	/// Hands the player the entrance purifier's replacement part.
	/// </summary>
	private static async Task TakePurifierPart(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Entrance Purifier Parts"));

		if (character.Quests.IsActive(Crystal2) && !character.Quests.IsCompletable(Crystal2))
		{
			await dialog.Msg(L("Every part in the pile is corroded through. The Vubbe have been carrying the good ones off."));
			return;
		}

		await dialog.Msg(L("A pile of purifier parts, most of them beyond use."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 4461: Purify the Toxic Fumes in 1F
//-----------------------------------------------------------------------------
public class Mine1AlchemistQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4461);
		SetName(L("Purify the Toxic Fumes in 1F"));
		SetDescription(L("Vaidotas cannot go deeper while the first floor is full of toxic fumes. Get every purifier on 1F running again."));
		SetType(QuestType.Main);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_ALCHEMIST", "d_cmine_01", L("Talk to Vaidotas in the Mine"));
		SetPhase(QuestStatus.InProgress, "MINE_1_ALCHEMIST", "d_cmine_01", L("Repair the Purifiers on 1F"));
		SetPhase(QuestStatus.Success, "MINE_1_ALCHEMIST", "d_cmine_01", L("Repair the Purifiers on 1F"));

		AddPrerequisite(new QuestStatusPrerequisite(8082, QuestStatus.Completed));

		AddObjective("repairPurifiers", L("Repair the Purifiers on 1F"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 3));
	}
}

// 4463: Incomplete Purifier
//-----------------------------------------------------------------------------
public class Mine1Crystal2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4463);
		SetName(L("Incomplete Purifier"));
		SetDescription(L("The entrance purifier is missing a part. Find a replacement and fit it."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_PURIFY_1", "d_cmine_01", L("Inspect the Entrance Purifier on the 1st Floor"));
		SetPhase(QuestStatus.InProgress, "MINE_1_PURIFY_1", "d_cmine_01", L("Retrieve Purifier Parts"));
		SetPhase(QuestStatus.Success, "MINE_1_PURIFY_1", "d_cmine_01", L("Repair the Entrance Purifier on 1F"));

		AddPrerequisite(new LevelPrerequisite(10));

		AddPityDrop("MINE_1_CRYSTAL_2_ITEM", 1.0f, 0, 1, "Goblin_Miners_Q1");

		AddObjective("findPart", L("Retrieve Purifier Parts"), new CollectItemObjective("MINE_1_CRYSTAL_2_ITEM", 1));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 15));
		AddReward(new TakeItemReward("MINE_1_CRYSTAL_2_ITEM"));
	}
}

// 4469: Fix the Central Purifier (1)
//-----------------------------------------------------------------------------
public class Mine1Crystal8Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4469);
		SetName(L("Fix the Central Purifier (1)"));
		SetDescription(L("The central purifier has seized. Open its valve and see what is wrong."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_PURIFY_5", "d_cmine_01", L("Inspect the Central Purifier"));
		SetPhase(QuestStatus.InProgress, "MINE_1_PURIFY_5", "d_cmine_01", L("Use the Mine Compass"));
		SetPhase(QuestStatus.Success, "MINE_1_PURIFY_5", "d_cmine_01", L("Use the Mine Compass"));

		AddPrerequisite(new ItemPrerequisite("CMINE_COMPASS_ITEM", 1));

		AddObjective("openValve", L("Inspect the Central Purifier"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4470: Fix the Central Purifier (2)
//-----------------------------------------------------------------------------
public class Mine1Crystal9Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4470);
		SetName(L("Fix the Central Purifier (2)"));
		SetDescription(L("The compass points to District 4, where a spare purifier still holds the part the central one needs."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_CRYSTAL_9_DEVICE", "d_cmine_01", L("Search District 4 for the Spare Purifier"));
		SetPhase(QuestStatus.InProgress, "MINE_1_CRYSTAL_9_DEVICE", "d_cmine_01", L("Get parts from the Spare Purifier in District 4"));
		SetPhase(QuestStatus.Success, "MINE_1_PURIFY_5", "d_cmine_01", L("Fix the Central Purifier"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_1_CRYSTAL_9_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(4469, QuestStatus.Completed));

		AddPityDrop("MINE_1_CRYSTAL_9_ITEM", 1.0f, 0, 1, "boss_bearkaras");

		AddObjective("takePart", L("Collect Purifier Parts from the Spare Purifier"), new CollectItemObjective("MINE_1_CRYSTAL_9_ITEM", 1));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("R_BRC02_101", 1));
		AddReward(new TakeItemReward("MINE_1_CRYSTAL_9_ITEM"));
	}
}

// 4471: Cyclops' Attack in the Crystal Mine
//-----------------------------------------------------------------------------
public class Mine1Crystal10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4471);
		SetName(L("Cyclops' Attack in the Crystal Mine"));
		SetDescription(L("A Cyclops has broken into the crystal store and scattered the miners' baskets. Put it down."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_CRYSTAL_10", "d_cmine_01", L("Defeat Cyclops in the Crystal Mine"));
		SetPhase(QuestStatus.InProgress, "MINE_1_CRYSTAL_10", "d_cmine_01", L("Defeat Cyclops in the Crystal Mine"));
		SetPhase(QuestStatus.Success, "MINE_1_CRYSTAL_10", "d_cmine_01", L("Defeat Cyclops in the Crystal Mine"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_1_CRYSTAL_10_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(10));

		AddObjective("killCyclops", L("Defeat Cyclops"), new KillObjective(1, "boss_Strongholder") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("TreasureboxKey2", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 15));
		AddReward(new ItemReward("Drug_SP1_Q", 15));
	}
}

// 4474: Activate the Passage Purifier (1)
//-----------------------------------------------------------------------------
public class Mine1Crystal13Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4474);
		SetName(L("Activate the Passage Purifier (1)"));
		SetDescription(L("The passage purifier is cold and a part is missing from its housing."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_PURIFY_7", "d_cmine_01", L("Inspect the Passage Purifier on 1F"));
		SetPhase(QuestStatus.InProgress, "MINE_1_PURIFY_7", "d_cmine_01", L("Use the Mine Compass"));
		SetPhase(QuestStatus.Success, "MINE_1_PURIFY_7", "d_cmine_01", L("Use the Mine Compass"));

		AddPrerequisite(new LevelPrerequisite(10));
		AddPrerequisite(new ItemPrerequisite("CMINE_COMPASS_ITEM", 1));

		AddObjective("openValve", L("Inspect the Passage Purifier on 1F"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4479: Activate the Passage Purifier (2)
//-----------------------------------------------------------------------------
public class Mine1Crystal18Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4479);
		SetName(L("Activate the Passage Purifier (2)"));
		SetDescription(L("The compass points into District 6, where a Specter Monarch is hoarding the missing part."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_PURIFY_7", "d_cmine_01", L("Use the Mine Compass"));
		SetPhase(QuestStatus.InProgress, "MINE_1_CRYSTAL_18_TRIGGER", "d_cmine_01", L("Search District 6 for Purifier Parts"));
		SetPhase(QuestStatus.Success, "MINE_1_PURIFY_7", "d_cmine_01", L("Search District 6 for Purifier Parts"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_1_CRYSTAL_18_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(4474, QuestStatus.Completed));

		AddPityDrop("MINE_1_CRYSTAL_18_ITEM", 1.0f, 0, 1, "boss_Spector_m");

		AddObjective("takePart", L("Retrieve Purifier Parts from Specter Monarch"), new CollectItemObjective("MINE_1_CRYSTAL_18_ITEM", 1));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("misc_brcCrystal", 1));
	}
}

// 4480: Activate the Passage Purifier (3)
//-----------------------------------------------------------------------------
public class Mine1Crystal19Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4480);
		SetName(L("Activate the Passage Purifier (3)"));
		SetDescription(L("Fit the recovered part and start the passage purifier."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_01");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_1_PURIFY_7", "d_cmine_01", L("Repair the Passage Purifier on 1F"));
		SetPhase(QuestStatus.InProgress, "MINE_1_PURIFY_7", "d_cmine_01", L("Repair the Passage Purifier on 1F"));
		SetPhase(QuestStatus.Success, "MINE_1_PURIFY_7", "d_cmine_01", L("Repair the Passage Purifier on 1F"));

		AddPrerequisite(new QuestStatusPrerequisite(4479, QuestStatus.Completed));

		AddObjective("fitPart", L("Repair the Passage Purifier on 1F"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 15));
		AddReward(new TakeItemReward("MINE_1_CRYSTAL_18_ITEM"));
	}
}
