//--- Melia Script ----------------------------------------------------------
// Crystal Mine 2F Quest NPCs
//--- Description -----------------------------------------------------------
// The circulation, auxiliary and main purifiers of the second floor, and the
// pipes and supply devices they depend on.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine02QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Alchemist = new QuestId(4467);
	private readonly static QuestId Crystal2 = new QuestId(4483);
	private readonly static QuestId Crystal3 = new QuestId(4484);
	private readonly static QuestId Crystal4 = new QuestId(4485);
	private readonly static QuestId Crystal5 = new QuestId(4486);
	private readonly static QuestId Crystal7 = new QuestId(4488);
	private readonly static QuestId Crystal10 = new QuestId(4491);
	private readonly static QuestId Crystal11 = new QuestId(4492);
	private readonly static QuestId Crystal14 = new QuestId(4495);
	private readonly static QuestId Crystal20 = new QuestId(4501);
	private readonly static QuestId Crystal21 = new QuestId(4502);

	protected override void Load()
	{
		// Circulation Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("Circulation Purifier"), "MINE_2_PURIFY_1", "d_cmine_02", -1615, 122, 30, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Circulation Purifier"));

			if (character.Quests.IsActive(Crystal4) && character.Quests.IsCompletable(Crystal4))
			{
				var started = await character.TimeActions.StartAsync(L("Starting the purifier..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (started != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("All Purifier Pipes seem to be working properly. The Circulation Purifier starts up."));
				await dialog.CompleteQuest(Crystal4);
				CheckPurifiersRepaired(character);
				return;
			}

			if (character.Quests.IsActive(Crystal2) && character.Quests.IsCompletable(Crystal2))
			{
				var worked = await character.TimeActions.StartAsync(L("Working the purifier..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (worked != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The Purifier in District 3 is working properly now. The Circulation Purifier turns over, then stalls again."));
				await dialog.CompleteQuest(Crystal2);
				character.Quests.Start(Crystal3);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Crystal2) && character.Quests.MeetsPrerequisites(Crystal2))
			{
				var answer = await dialog.SelectQuestOffer(Crystal2, L("The Circulation Purifier on the 2nd Floor is not working. Figure out what the problem is."),
					Option(L("Inspect the purifier"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var inspected = await character.TimeActions.StartAsync(L("Checking the purifier..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (inspected != TimeActionResult.Completed)
						return;

					character.Quests.Start(Crystal2);
					character.LookAround();
				}

				return;
			}

			if (!character.Quests.Has(Crystal3) && character.Quests.MeetsPrerequisites(Crystal3))
			{
				await dialog.Msg(L("The Circulation Purifier still stalls. Check the Purifier Pipe in District 2."));
				character.Quests.Start(Crystal3);
				return;
			}

			if (character.Quests.IsActive(Crystal2))
			{
				await dialog.Msg(L("Examine the purifier pipe in District 3 and fix it."));
				return;
			}

			if (character.Quests.IsActive(Crystal4))
			{
				await dialog.Msg(L("Check if the Purifier Pipe in District 2 is functioning properly."));
				return;
			}

			await dialog.Msg(L("The Circulation Purifier hums steadily."));
		});

		// District 3 Purifier Pipe
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("District 3 Purifier Pipe"), "MINE_2_CRYSTAL_2_PIPE", "d_cmine_02", -1313, -753, 90, c => c.Quests.IsActive(Crystal2) && !c.Quests.IsCompletable(Crystal2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("District 3 Purifier Pipe"));

			if (character.Quests.IsActive(Crystal2) && !character.Quests.IsCompletable(Crystal2))
			{
				var cleared = await character.TimeActions.StartAsync(L("Checking the purifier pipe..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (cleared != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The pipe is choked with crystal dust. You clear it and seat the coupling again."));
				character.Quests.CompleteObjective(Crystal2, "repairPipe");
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A length of purifier pipe, running clear."));
		});

		// District 2 Purifier Pipe
		//-------------------------------------------------------------------------
		AddConditionalNpc(151020, L("District 2 Purifier Pipe"), "MINE_2_CRYSTAL_3_PIPE", "d_cmine_02", -1815, 940, 90, IsDistrict2PipeShown, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("District 2 Purifier Pipe"));

			if (character.Quests.IsActive(Crystal4) && !character.Quests.IsCompletable(Crystal4))
			{
				var straightened = await character.TimeActions.StartAsync(L("Checking the purifier pipe..."), L("Cancel"), "GROPE", TimeSpan.FromSeconds(3));

				if (straightened != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("You work the pipe back into shape. Air moves through it again."));
				character.Quests.CompleteObjective(Crystal4, "checkPipe");
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Crystal4) && character.Quests.MeetsPrerequisites(Crystal4))
			{
				await dialog.Msg(L("The pipe has been crushed. Work it back into shape."));
				character.Quests.Start(Crystal4);
				return;
			}

			if (character.Quests.IsActive(Crystal3))
			{
				await dialog.Msg(L("The Carapace will not let you near the pipe."));
				character.Quests.ClearQuestTrack(Crystal3);
				return;
			}

			await dialog.Msg(L("A length of purifier pipe, running clear."));
		});

		// Auxiliary Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("Auxiliary Purifier"), "MINE_2_PURIFY_3", "d_cmine_02", -573, 495, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Auxiliary Purifier"));

			if (character.Quests.IsActive(Crystal11))
			{
				var startedAux = await character.TimeActions.StartAsync(L("Starting the purifier..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (startedAux != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The Magic Supply Device is working well. The Auxiliary Purifier is running again."));
				character.Quests.CompleteObjective(Crystal11, "activate");
				await dialog.CompleteQuest(Crystal11);
				CheckPurifiersRepaired(character);
				return;
			}

			if (!character.Quests.Has(Crystal5) && character.Quests.MeetsPrerequisites(Crystal5))
			{
				var answer = await dialog.SelectQuestOffer(Crystal5, L("The Auxiliary Purifier on the 2nd Floor is halted. Find out why it is halted."),
					Option(L("Inspect the purifier"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var inspectedAux = await character.TimeActions.StartAsync(L("Checking the purifier..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (inspectedAux != TimeActionResult.Completed)
						return;

					character.Quests.Start(Crystal5);
					character.Quests.CompleteObjective(Crystal5, "inspect");
					await dialog.CompleteQuest(Crystal5);
					await dialog.Msg(L("The Mine Compass points towards the Magic Supply Device in District 4."));
					character.Quests.Start(Crystal7);
				}

				return;
			}

			if (!character.Quests.Has(Crystal7) && character.Quests.MeetsPrerequisites(Crystal7))
			{
				await dialog.Msg(L("The Mine Compass points towards the Magic Supply Device in District 4."));
				character.Quests.Start(Crystal7);
				return;
			}

			if (!character.Quests.Has(Crystal11) && character.Quests.MeetsPrerequisites(Crystal11))
			{
				await dialog.Msg(L("The Magic Supply Device is turning again. The Auxiliary Purifier is ready to start."));
				character.Quests.Start(Crystal11);
				return;
			}

			await dialog.Msg(L("The Auxiliary Purifier hums steadily."));
		});

		// Magic Supply Device
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Magic Supply Device"), "MINE_2_CRYSTAL_7_ENERGY", "d_cmine_02", -789, 1148, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Magic Supply Device"));

			if (character.Quests.IsActive(Crystal10))
			{
				if (!character.Quests.IsCompletable(Crystal10))
				{
					await dialog.Msg(L("Follow the lead of the Mine Compass and find a way to remove the rust."));
					return;
				}

				var oiled = await character.TimeActions.StartAsync(L("Oiling the gears..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(2));

				if (oiled != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The lubricant frees the seized gears. The Magic Supply Device turns over."));
				await dialog.CompleteQuest(Crystal10);
				character.Quests.Start(Crystal11);
				return;
			}

			if (character.Quests.IsActive(Crystal7))
			{
				var looked = await character.TimeActions.StartAsync(L("Looking the device over..."), L("Cancel"), "LOOK_SIT", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The device is seized solid with rust."));
				await dialog.Msg(L("Use the Mine Compass to check what you need to fix the Auxiliary Purifier."));
				character.Quests.CompleteObjective(Crystal7, "inspect");
				await dialog.CompleteQuest(Crystal7);
				await dialog.Msg(L("The compass points deeper into the floor. Something there will shift the rust."));
				character.Quests.Start(Crystal10);
				return;
			}

			if (!character.Quests.Has(Crystal10) && character.Quests.MeetsPrerequisites(Crystal10))
			{
				await dialog.Msg(L("The device is still seized with rust. Something on this floor will shift it."));
				character.Quests.Start(Crystal10);
				return;
			}

			await dialog.Msg(L("The Magic Supply Device turns quietly."));
		});

		// Lubricant
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Lubricant"), "MINE_2_CRYSTAL_10_OIL", "d_cmine_02", 338, 1095, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Lubricant"));

			if (character.Quests.IsActive(Crystal10) && !character.Quests.IsCompletable(Crystal10))
			{
				var taken = await character.TimeActions.StartAsync(L("Taking the lubricant..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

				if (taken != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("A miner's can of lubricant, still half full. This will shift the rust."));
				character.Inventory.Add(ItemId.MINE_2_CRYSTAL_10_ITEM, 1);
				return;
			}

			await dialog.Msg(L("An empty lubricant can."));
		});

		// Main Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("Main Purifier"), "MINE_2_PURIFY_7", "d_cmine_02", 524, 364, 60, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Main Purifier"));

			if (character.Quests.IsActive(Crystal21))
			{
				var repairedMain = await character.TimeActions.StartAsync(L("Repairing the purifier..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (repairedMain != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("You fit the recovered part. The Main Purifier is repaired and the fumes are already thinning."));
				character.Quests.CompleteObjective(Crystal21, "fitPart");
				await dialog.CompleteQuest(Crystal21);
				CheckPurifiersRepaired(character);
				return;
			}

			if (!character.Quests.Has(Crystal14) && character.Quests.MeetsPrerequisites(Crystal14))
			{
				var answer = await dialog.SelectQuestOffer(Crystal14, L("The Main Purifier is not working. Go and inspect it."),
					Option(L("Inspect the purifier"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var inspectedMain = await character.TimeActions.StartAsync(L("Looking the purifier over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (inspectedMain != TimeActionResult.Completed)
						return;

					character.Quests.Start(Crystal14);
					character.Quests.CompleteObjective(Crystal14, "inspect");
					await dialog.CompleteQuest(Crystal14);
					await dialog.Msg(L("The compass is pointing towards District 6."));
					character.Quests.Start(Crystal20);
					character.LookAround();
				}

				return;
			}

			if (!character.Quests.Has(Crystal20) && character.Quests.MeetsPrerequisites(Crystal20))
			{
				await dialog.Msg(L("The compass is pointing towards District 6."));
				character.Quests.Start(Crystal20);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Crystal21) && character.Quests.MeetsPrerequisites(Crystal21))
			{
				await dialog.Msg(L("The recovered part fits the housing. Start the purifier."));
				character.Quests.Start(Crystal21);
				return;
			}

			await dialog.Msg(L("The Main Purifier hums steadily."));
		});

		// Main Purifier Parts
		//-------------------------------------------------------------------------
		AddConditionalNpc(151016, L("Main Purifier Parts"), "MINE_2_CRYSTAL_20_PART", "d_cmine_02", 1559, -468, 90, c => c.Quests.IsActive(Crystal20), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Main Purifier Parts"));

			if (character.Quests.IsActive(Crystal20))
			{
				if (!character.Quests.IsCompletable(Crystal20))
				{
					await dialog.Msg(L("The Stone Whale is still guarding the parts."));
					character.Quests.ClearQuestTrack(Crystal20);
					return;
				}

				var recovered = await character.TimeActions.StartAsync(L("Recovering the part..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

				if (recovered != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("You defeated the Stone Whale and got the parts. Return to the Main Purifier and fix it."));
				await dialog.CompleteQuest(Crystal20);
				character.Quests.Start(Crystal21);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("The parts have been stripped from this housing."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("MINE_2_CRYSTAL_3_TRIGGER", "d_cmine_02", -1555, 895, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Crystal3) && !character.Quests.IsCompletable(Crystal3))
				character.Quests.StartQuestTrack(Crystal3);

			await Task.CompletedTask;
		});

		AddQuestTrigger("MINE_2_CRYSTAL_20_TRIGGER", "d_cmine_02", 1654, -803, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Crystal20) && !character.Quests.IsCompletable(Crystal20))
				character.Quests.StartQuestTrack(Crystal20);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether the District 2 pipe is there for the given character,
	/// from the Carapace's defeat until the pipe is worked back into shape.
	/// </summary>
	private static bool IsDistrict2PipeShown(Character character)
		=> character.Quests.HasCompleted(Crystal3) && !character.Quests.IsCompletable(Crystal4) && !character.Quests.HasCompleted(Crystal4);

	/// <summary>
	/// Ends the floor's main quest once all three purifiers run again.
	/// </summary>
	public static void CheckPurifiersRepaired(Character character)
	{
		if (!character.Quests.IsActive(Alchemist))
			return;

		if (!character.Quests.HasCompleted(Crystal4) || !character.Quests.HasCompleted(Crystal11) || !character.Quests.HasCompleted(Crystal21))
			return;

		character.Quests.CompleteObjective(Alchemist, "repairPurifiers");
		character.Quests.Complete(Alchemist);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("All the purifiers have been repaired!{nl}Go down to the 3rd floor and meet Vaidotas!"), 10);
		character.LookAround();
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 4467: Purify the Toxic Fumes in 2F
//-----------------------------------------------------------------------------
public class Mine2AlchemistQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4467);
		SetName(L("Purify the Toxic Fumes in 2F"));
		SetDescription(L("The second floor's fumes are far worse than the first. Get every purifier on 2F running again."));
		SetType(QuestType.Main);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_PURIFY_1", "d_cmine_02", L("Repair the Purifiers on 2F"));
		SetPhase(QuestStatus.InProgress, "MINE_2_PURIFY_1", "d_cmine_02", L("Repair the Purifiers on 2F"));
		SetPhase(QuestStatus.Success, "MINE_2_PURIFY_1", "d_cmine_02", L("Repair the Purifiers on 2F"));

		AddPrerequisite(new QuestStatusPrerequisite(4461, QuestStatus.Completed));

		AddObjective("repairPurifiers", L("Repair the Purifiers on 2F"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 3));
	}
}

// 4483: Circulation Purifier Issues (1)
//-----------------------------------------------------------------------------
public class Mine2Crystal2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4483);
		SetName(L("Circulation Purifier Issues (1)"));
		SetDescription(L("The circulation purifier cannot draw the fumes in. Its pipe in District 3 is blocked."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_PURIFY_1", "d_cmine_02", L("Inspect the Circulation Purifier"));
		SetPhase(QuestStatus.InProgress, "MINE_2_CRYSTAL_2_PIPE", "d_cmine_02", L("Repair the Purifier Pipe"));
		SetPhase(QuestStatus.Success, "MINE_2_PURIFY_1", "d_cmine_02", L("Inspect the Circulation Purifier"));

		AddPrerequisite(new LevelPrerequisite(12));

		AddObjective("repairPipe", L("Repair the Purifier Pipe"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4484: Circulation Purifier Issues (2)
//-----------------------------------------------------------------------------
public class Mine2Crystal3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4484);
		SetName(L("Circulation Purifier Issues (2)"));
		SetDescription(L("A Carapace has settled on the District 2 pipe and will not let anyone near it."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_CRYSTAL_3_PIPE", "d_cmine_02", L("Inspect the Purifier Pipe in District 2"));
		SetPhase(QuestStatus.InProgress, "MINE_2_CRYSTAL_3_TRIGGER", "d_cmine_02", L("Inspect the Purifier Pipe in District 2"));
		SetPhase(QuestStatus.Success, "MINE_2_CRYSTAL_3_PIPE", "d_cmine_02", L("Inspect the Purifier Pipe in District 2"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_2_CRYSTAL_3_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(4483, QuestStatus.Completed));

		AddObjective("killCarapace", L("Defeat Carapace"), new KillObjective(1, "boss_Carapace") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("misc_brcCrystal", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in; the Carapace's death ends the quest and opens the pipe check.
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(4485));
		character.LookAround();
	}
}

// 4485: Circulation Purifier Issues (3)
//-----------------------------------------------------------------------------
public class Mine2Crystal4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4485);
		SetName(L("Circulation Purifier Issues (3)"));
		SetDescription(L("The District 2 pipe has been crushed. Work it back into shape and start the circulation purifier."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_CRYSTAL_3_PIPE", "d_cmine_02", L("Inspect the Purifier Pipe in District 2"));
		SetPhase(QuestStatus.InProgress, "MINE_2_CRYSTAL_3_PIPE", "d_cmine_02", L("Inspect the Purifier Pipe in District 2"));
		SetPhase(QuestStatus.Success, "MINE_2_PURIFY_1", "d_cmine_02", L("Activate the Circulation Purifier"));

		AddPrerequisite(new QuestStatusPrerequisite(4484, QuestStatus.Completed));

		AddObjective("checkPipe", L("Inspect the Purifier Pipe in District 2"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4486: Inoperable Auxiliary Purifier (1)
//-----------------------------------------------------------------------------
public class Mine2Crystal5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4486);
		SetName(L("Inoperable Auxiliary Purifier (1)"));
		SetDescription(L("The auxiliary purifier is drawing no power at all."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_PURIFY_3", "d_cmine_02", L("Inspect the Auxiliary Purifier on 2F"));
		SetPhase(QuestStatus.InProgress, "MINE_2_PURIFY_3", "d_cmine_02", L("Use the Mine Compass"));
		SetPhase(QuestStatus.Success, "MINE_2_PURIFY_3", "d_cmine_02", L("Use the Mine Compass"));

		AddPrerequisite(new LevelPrerequisite(12));
		AddPrerequisite(new ItemPrerequisite("CMINE_COMPASS_ITEM", 1));

		AddObjective("inspect", L("Inspect the Auxiliary Purifier on 2F"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4488: Inoperable Auxiliary Purifier (2)
//-----------------------------------------------------------------------------
public class Mine2Crystal7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4488);
		SetName(L("Inoperable Auxiliary Purifier (2)"));
		SetDescription(L("The compass points to the magic supply device in District 4."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_CRYSTAL_7_ENERGY", "d_cmine_02", L("Inspect the Magic Supply Device"));
		SetPhase(QuestStatus.InProgress, "MINE_2_CRYSTAL_7_ENERGY", "d_cmine_02", L("Use the Mine Compass"));
		SetPhase(QuestStatus.Success, "MINE_2_CRYSTAL_7_ENERGY", "d_cmine_02", L("Use the Mine Compass"));

		AddPrerequisite(new QuestStatusPrerequisite(4486, QuestStatus.Completed));

		AddObjective("inspect", L("Inspect the Magic Supply Device"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4491: Inoperable Auxiliary Purifier (3)
//-----------------------------------------------------------------------------
public class Mine2Crystal10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4491);
		SetName(L("Inoperable Auxiliary Purifier (3)"));
		SetDescription(L("The magic supply device is seized with rust. Something on this floor will shift it."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_CRYSTAL_7_ENERGY", "d_cmine_02", L("Use the Mine Compass"));
		SetPhase(QuestStatus.InProgress, "MINE_2_CRYSTAL_10_OIL", "d_cmine_02", L("Search for tools to remove rust"));
		SetPhase(QuestStatus.Success, "MINE_2_CRYSTAL_7_ENERGY", "d_cmine_02", L("Restore the Magic Supply Device in District 4"));

		AddPrerequisite(new QuestStatusPrerequisite(4488, QuestStatus.Completed));

		AddObjective("findOil", L("Search for tools to remove rust"), new CollectItemObjective("MINE_2_CRYSTAL_10_ITEM", 1));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("MINE_2_CRYSTAL_10_ITEM"));
	}
}

// 4492: Inoperable Auxiliary Purifier (4)
//-----------------------------------------------------------------------------
public class Mine2Crystal11Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4492);
		SetName(L("Inoperable Auxiliary Purifier (4)"));
		SetDescription(L("The magic supply device is turning again. Start the auxiliary purifier."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_PURIFY_3", "d_cmine_02", L("Activate the Auxiliary Purifier on 2F"));
		SetPhase(QuestStatus.InProgress, "MINE_2_PURIFY_3", "d_cmine_02", L("Activate the Auxiliary Purifier on 2F"));
		SetPhase(QuestStatus.Success, "MINE_2_PURIFY_3", "d_cmine_02", L("Activate the Auxiliary Purifier on 2F"));

		AddPrerequisite(new QuestStatusPrerequisite(4491, QuestStatus.Completed));

		AddObjective("activate", L("Activate the Auxiliary Purifier on 2F"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4495: Destroyer of the Main Purifier (1)
//-----------------------------------------------------------------------------
public class Mine2Crystal14Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4495);
		SetName(L("Destroyer of the Main Purifier (1)"));
		SetDescription(L("Parts have been torn out of the main purifier and carried off."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_PURIFY_7", "d_cmine_02", L("Check the Main Purifier"));
		SetPhase(QuestStatus.InProgress, "MINE_2_PURIFY_7", "d_cmine_02", L("Use the Mine Compass"));
		SetPhase(QuestStatus.Success, "MINE_2_PURIFY_7", "d_cmine_02", L("Use the Mine Compass"));

		AddPrerequisite(new LevelPrerequisite(12));
		AddPrerequisite(new ItemPrerequisite("CMINE_COMPASS_ITEM", 1));

		AddObjective("inspect", L("Check the Main Purifier"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 4501: Destroyer of the Main Purifier (2)
//-----------------------------------------------------------------------------
public class Mine2Crystal20Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4501);
		SetName(L("Destroyer of the Main Purifier (2)"));
		SetDescription(L("A Stone Whale dragged the main purifier's parts into District 6 and is sitting on them."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_CRYSTAL_20_PART", "d_cmine_02", L("Use the Mine Compass"));
		SetPhase(QuestStatus.InProgress, "MINE_2_CRYSTAL_20_TRIGGER", "d_cmine_02", L("Search District 6 for parts"));
		SetPhase(QuestStatus.Success, "MINE_2_CRYSTAL_20_PART", "d_cmine_02", L("Fix the Main Purifier"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "mine_2_5", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(4495, QuestStatus.Completed));

		AddObjective("killWhale", L("Defeat Stone Whale"), new KillObjective(1, "boss_stone_whale") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("MINE_2_CRYSTAL_20_ITEM", 1));
	}
}

// 4502: Destroyer of the Main Purifier (3)
//-----------------------------------------------------------------------------
public class Mine2Crystal21Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4502);
		SetName(L("Destroyer of the Main Purifier (3)"));
		SetDescription(L("Fit the recovered part and start the main purifier."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_02");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_2_PURIFY_7", "d_cmine_02", L("Fix the Main Purifier"));
		SetPhase(QuestStatus.InProgress, "MINE_2_PURIFY_7", "d_cmine_02", L("Fix the Main Purifier"));
		SetPhase(QuestStatus.Success, "MINE_2_PURIFY_7", "d_cmine_02", L("Fix the Main Purifier"));

		AddPrerequisite(new QuestStatusPrerequisite(4501, QuestStatus.Completed));

		AddObjective("fitPart", L("Fix the Main Purifier"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("MINE_2_CRYSTAL_20_ITEM"));
	}
}
