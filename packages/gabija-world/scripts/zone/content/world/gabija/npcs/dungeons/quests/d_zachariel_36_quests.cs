//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 5F Quest NPCs
//--- Description -----------------------------------------------------------
// The Secret Guardian's false revelation, the last guardian Rexipher walks
// through, and the burial chamber of the Great King.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel36QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8388);
	private readonly static QuestId Mq02 = new QuestId(8389);
	private readonly static QuestId Mq03 = new QuestId(8390);
	private readonly static QuestId Mq04 = new QuestId(8391);
	private readonly static QuestId Mq05 = new QuestId(8392);
	private readonly static QuestId Eq01 = new QuestId(8419);
	private readonly static QuestId Eq02 = new QuestId(8420);
	private readonly static QuestId Eq03 = new QuestId(8421);
	private readonly static QuestId Eq04 = new QuestId(8422);
	private readonly static QuestId Eq05 = new QuestId(8423);
	private readonly static QuestId ToFedimian = new QuestId(50007);

	private const int MagicSourcesNeeded = 8;

	private readonly static double[,] ChargedCubeSpots =
	{
		{ -2495, -3600 }, { -2284, -3354 }, { -2310, -3653 }, { -2470, -3975 },
		{ -2611, -3854 }, { -2694, -3530 }, { -2620, -3374 }, { -2653, -3745 },
	};

	private readonly static int[] JarModels = { 47256, 47257, 47258, 47257, 47256, 47257 };

	private readonly static double[,] JarSpots =
	{
		{ -2767.38, -4617.75 }, { -2646.04, -4287.45 }, { -2319.46, -4666.12 },
		{ -2290.02, -4325.25 }, { -2627.56, -4842.29 }, { -2446.75, -4408.29 },
	};

	protected override void Load()
	{
		// Secret Guardian
		//-------------------------------------------------------------------------
		AddNpc(147467, L("Secret Guardian"), "ZACHARIEL_GUARDIAN", "d_zachariel_36", -2600.24, -5617.06, 56, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Guardian"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("The goddess already foresaw this."));
				await dialog.Msg(L("Nothing can escape from her vision."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("The evil presence will never get the revelation."));
				await dialog.CompleteQuest(Mq02);

				character.ServerMessage(L("Something cries out, far above you."));
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("The false revelation is where it needs to be."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("The evil presence is getting close to the goddess' revelation."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("But it will never get the revelation."),
					Option(L("I'll save the magic source of the Royal Mausoleum"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("It was prepared to protect against beings that humans could not stand against."));
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("Pour the source of the Royal Mausoleum's power into the jars. It will create a false revelation as a decoy."),
					Option(L("It's time to pour the source of the evil power in the jars"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					character.Inventory.Add(ItemId.ZACHA5F_MQ_01_ITEM, MagicSourcesNeeded, InventoryAddType.PickUp);
					await dialog.Msg(L("The evil presence will never get the revelation."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("The evil presence has obtained a false revelation."));

				var answer = await dialog.SelectQuestOffer(Mq03, L("How foolish of it. Now set the soul pot where the mausoleum's will can fill it."),
					Option(L("Yes, I'll make sure to fill the jar with the guardian's will"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.Inventory.Add(ItemId.ZACHA5F_MQ03_POT, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("The evil presence will never get the goddess' revelation."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Eight magic sources. The Medakia of the lower hall carry them."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Pour what you carry into the charged cubes of the lower hall."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Set the soul pot at the place prepared for it, further in."));
				return;
			}

			await dialog.Msg(L("It was prepared to protect against beings that humans could not stand against."));
		});

		// Charged Royal Mausoleum Cubes
		//-------------------------------------------------------------------------
		for (var i = 0; i < ChargedCubeSpots.GetLength(0); i++)
		{
			var uniqueName = "ZACHA5F_MQ_02_CUBE" + (i + 1);
			AddNpc(47261, L("Charged Royal Mausoleum Cube"), uniqueName, "d_zachariel_36", ChargedCubeSpots[i, 0], ChargedCubeSpots[i, 1], 90, this.PourMagicSource);
		}

		// Jars of the lower hall
		//-------------------------------------------------------------------------
		for (var i = 0; i < JarSpots.GetLength(0); i++)
		{
			var uniqueName = "ZACHA36_POT1_NPC" + (i + 1);
			AddNpc(JarModels[i], L("Jar"), uniqueName, "d_zachariel_36", JarSpots[i, 0], JarSpots[i, 1], 90, async dialog =>
			{
				dialog.SetTitle(L("Jar"));
				await dialog.Msg(L("A jar of the Royal Mausoleum, standing where it was set a thousand years ago."));
			});
		}

		// The place prepared for the Soul Pot
		//-------------------------------------------------------------------------
		AddNpc(20025, L("Soul Pot Placement Site"), "ZACHA5F_MQ_03", "d_zachariel_36", -2503, -2600, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Soul Pot Placement Site"));

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				if (character.Inventory.CountItem(ItemId.ZACHA5F_MQ03_POT) < 1)
				{
					await dialog.Msg(L("The setting is empty. You are carrying no soul pot to put in it."));
					return;
				}

				var placed = await character.TimeActions.StartAsync(L("Setting the jar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2.5));

				if (placed != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.ZACHA5F_MQ03_POT, 1);
				character.Quests.CompleteObjective(Mq03, "placePot");

				await dialog.Msg(L("The jar settles into the setting, and the will of the mausoleum begins to fill it."));
				return;
			}

			await dialog.Msg(L("A setting cut into the floor, the right size for a jar."));
		});

		// The Last Guardian
		//-------------------------------------------------------------------------
		AddNpc(147467, L("The Last Guardian"), "ZACHA5F_MQ_04", "d_zachariel_36", -2491, -1092, 4, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("The Last Guardian"));

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("Give me the soul pot. What is in it is the last thing standing between the evil presence and the revelation."),
					Option(L("Hand over the Soul Pot"), "accept"),
					Option(L("Hold on to it"), "leave")
				);

				if (answer == "accept")
				{
					var handed = await character.TimeActions.StartAsync(L("Handing over the soul pot..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2.5));

					if (handed != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq04);
					character.Quests.StartQuestTrack(Mq04);
				}
				return;
			}

			if (!character.Quests.Has(ToFedimian) && character.Quests.MeetsPrerequisites(ToFedimian))
			{
				var answer = await dialog.SelectQuestOffer(ToFedimian, L("The revelation names the Mage Tower next. Fedimian is the only road to it."),
					Option(L("Go to Fedimian"), "accept"),
					Option(L("Not yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(ToFedimian);
					await dialog.Msg(L("Take the road out of Zachariel Crossroads through the Tombstone Path. Fedimian is at the end of it."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Rexipher is still standing. Put him down."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			if (character.Quests.IsActive(ToFedimian))
			{
				await dialog.Msg(L("Fedimian, past the Tombstone Path and the Fedimian Suburbs."));
				return;
			}

			await dialog.Msg(L("I am the last of them. After me there is only the King."));
		});

		// Great King Zachariel's Coffin
		//-------------------------------------------------------------------------
		AddNpc(153024, L("Great King Zachariel's Coffin"), "ZACHA5F_MQ_05", "d_zachariel_36", -3018.78, 493.32, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Great King Zachariel's Coffin"));

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("The coffin of the Great King. Something in it has been waiting a thousand years for whoever opens it."),
					Option(L("Go to the burial chamber of the Royal Mausoleum"), "accept"),
					Option(L("Leave the coffin closed"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.Quests.StartQuestTrack(Mq05);
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The King has not finished speaking."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("The coffin of Great King Zachariel, open and empty of everything but dust."));
		});

		// Royal Mausoleum Guardian, lower hall
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Royal Mausoleum Guardian"), "ZACHA5F_EQ_01", "d_zachariel_36", -2509, -4762, 1, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Guardian"));

			if (!character.Quests.Has(Eq01) && character.Quests.MeetsPrerequisites(Eq01))
			{
				await dialog.Msg(L("Once a Guardian gets corrupted, it can never come back to its original status."));

				var answer = await dialog.SelectQuestOffer(Eq01, L("Eternal slumber will be the only fate for that Guardian."),
					Option(L("I'll defeat the corrupted guardians"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Eq01);
					character.Quests.StartQuestTrack(Eq01);
				}
				return;
			}

			if (character.Quests.IsActive(Eq01))
			{
				await dialog.Msg(L("They are still on their feet."));
				character.Quests.ReplayQuestTrack(Eq01);
				return;
			}

			await dialog.Msg(L("Eternal slumber will be the only fate for a corrupted Guardian."));
		});

		// Corrupted Royal Mausoleum Guardian, middle hall
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Corrupted Royal Mausoleum Guardian"), "ZACHA5F_EQ_02", "d_zachariel_36", -2509, -3858, 3, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Corrupted Royal Mausoleum Guardian"));

			if (!character.Quests.Has(Eq02) && character.Quests.MeetsPrerequisites(Eq02))
			{
				var answer = await dialog.SelectQuestOffer(Eq02, L("Anyone who disturbs the King's rest will meet their end here."),
					Option(L("I'll defeat the corrupted guardians"), "accept"),
					Option(L("Run away"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Eq02);
					character.Quests.StartQuestTrack(Eq02);
				}
				return;
			}

			if (character.Quests.IsActive(Eq02))
			{
				await dialog.Msg(L("They are still on their feet."));
				character.Quests.ReplayQuestTrack(Eq02);
				return;
			}

			await dialog.Msg(L("Anyone who disturbs the King's rest will meet their end here."));
		});

		// Hidden Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(40035, L("Hidden Treasure Chest"), "ZACHA5F_EQ_03", "d_zachariel_36", -1517, -2627, 268, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Hidden Treasure Chest"));

			if (!character.Quests.Has(Eq03) && character.Quests.MeetsPrerequisites(Eq03))
			{
				var answer = await dialog.SelectQuestOffer(Eq03, L("A chest stands in the side hall, and the floor around it will not stay still."),
					Option(L("Find the hidden treasure"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Eq03);
					character.Quests.StartQuestTrack(Eq03);
				}
				return;
			}

			if (character.Quests.IsActive(Eq03))
			{
				await dialog.Msg(L("The Medakia are still between you and the chest."));
				character.Quests.ReplayQuestTrack(Eq03);
				return;
			}

			await dialog.Msg(L("A chest of the Royal Mausoleum, its lid closed on nothing."));
		});

		// Royal Mausoleum Desk
		//-------------------------------------------------------------------------
		AddNpc(47254, L("Royal Mausoleum Desk"), "ZACHA5F_EQ_04", "d_zachariel_36", -3440.15, -2584.22, 88, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Desk"));

			if (!character.Quests.Has(Eq04) && character.Quests.MeetsPrerequisites(Eq04))
			{
				var answer = await dialog.SelectQuestOffer(Eq04, L("The name of the Revelator has been erased and only the demon's name remains. You must protect yourself."),
					Option(L("Read the epitaph"), "accept"),
					Option(L("Leave the desk alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Eq04);
					character.Quests.StartQuestTrack(Eq04);
				}
				return;
			}

			if (character.Quests.IsActive(Eq04))
			{
				await dialog.Msg(L("The Rusrat are still closing in on the desk."));
				character.Quests.ReplayQuestTrack(Eq04);
				return;
			}

			await dialog.Msg(L("A desk of the Royal Mausoleum, with one name scraped off its slate."));
		});

		// Corrupted Royal Mausoleum Guardian, upper hall
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Corrupted Royal Mausoleum Guardian"), "ZACHA5F_EQ_05", "d_zachariel_36", -2511, -2872, 359, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Corrupted Royal Mausoleum Guardian"));

			if (!character.Quests.Has(Eq05) && character.Quests.MeetsPrerequisites(Eq05))
			{
				var answer = await dialog.SelectQuestOffer(Eq05, L("Destruction by the Revelator is a preferable alternative to corruption by the evil energy."),
					Option(L("I'll defeat the corrupted guardians"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Eq05);
					character.Quests.StartQuestTrack(Eq05);
				}
				return;
			}

			if (character.Quests.IsActive(Eq05))
			{
				await dialog.Msg(L("They are still on their feet."));
				character.Quests.ReplayQuestTrack(Eq05);
				return;
			}

			await dialog.Msg(L("Destruction by the Revelator is a preferable alternative to corruption by the evil energy."));
		});

		// The portal back to Zachariel Crossroads
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Portal"), "ZACHARIEL36_ROKAS31", "d_zachariel_36", -2656.43, 490.98, 90, c => c.Quests.HasCompleted(Mq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Portal"));

			await dialog.Msg(L("The portal back to the Royal Mausoleum entrance is open."));
			character.Warp("f_rokas_31", -1271, 274, 715);
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The gate into Fedimian, at the end of the Fedimian Suburbs.
		AddQuestTrigger("WS_REMAINS40_TO_FEDMIAN_ENTER", "f_remains_40", -2393, -1377, 60, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(ToFedimian) && !character.Quests.IsCompletable(ToFedimian))
				character.Quests.CompleteObjective(ToFedimian, "reachFedimian");

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Pours one of the mausoleum's magic sources into a charged cube.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PourMagicSource(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Charged Royal Mausoleum Cube"));

		if (!character.Quests.IsActive(Mq02) || character.Quests.IsCompletable(Mq02))
		{
			await dialog.Msg(L("A cube of the Royal Mausoleum, holding a charge it has nowhere to send."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.ZACHA5F_MQ_01_ITEM) < 1)
		{
			await dialog.Msg(L("You are carrying no magic source to pour into it."));
			return;
		}

		var poured = await character.TimeActions.StartAsync(L("Pouring the magic source..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

		if (poured != TimeActionResult.Completed)
			return;

		character.Inventory.RemoveItem(ItemId.ZACHA5F_MQ_01_ITEM, 1);

		if (character.Inventory.CountItem(ItemId.ZACHA5F_MQ_01_ITEM) < 1)
		{
			character.Quests.CompleteObjective(Mq02, "pourSources");
			await dialog.Msg(L("The last source runs into the cube, and the false revelation is whole."));
			return;
		}

		await dialog.Msg(L("The source runs into the cube and the false revelation takes a little more shape."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8388: The Guardian's Jar (1)
//-----------------------------------------------------------------------------
public class Zacha5fMq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8388);
		SetName(L("The Guardian's Jar (1)"));
		SetDescription(L("The Medakia of the lower hall carry the mausoleum's own magic sources."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHARIEL_GUARDIAN", "d_zachariel_36", L("Talk to the Secret Guardian"), L("The evil presence is getting close to the goddess' revelation. Talk to the Secret Guardian."));
		SetPhase(QuestStatus.InProgress, "ZACHARIEL_GUARDIAN", "d_zachariel_36", L("Collect the Royal Mausoleum's Magic Sources"), L("Defeat Medakia and take the Royal Mausoleum's Magic Sources off them."));
		SetPhase(QuestStatus.Success, "ZACHARIEL_GUARDIAN", "d_zachariel_36", L("Talk to the Secret Guardian"), L("You have collected enough magic sources. Talk to the Secret Guardian."));

		AddPrerequisite(new QuestStatusPrerequisite(20184, QuestStatus.Completed));

		AddPityDrop("ZACHA5F_MQ_01_ITEM", 1.0f, 0, 1, "schlesien_darkmage");

		AddObjective("collectSources", L("Obtain the Royal Mausoleum's Magic Source by defeating Medakia"), new CollectItemObjective("ZACHA5F_MQ_01_ITEM", 8));

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new TakeItemReward("ZACHA5F_MQ_01_ITEM"));
	}
}

// 8389: The Guardian's Jar (2)
//-----------------------------------------------------------------------------
public class Zacha5fMq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8389);
		SetName(L("The Guardian's Jar (2)"));
		SetDescription(L("Poured into the charged cubes, the sources make a false revelation for the demon to take."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHARIEL_GUARDIAN", "d_zachariel_36", L("Talk to the Secret Guardian"), L("Talk to the Secret Guardian."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_MQ_02_CUBE1", "d_zachariel_36", L("Pour the magic sources into the jars"), L("Pour the source of the Royal Mausoleum's power into the jars. It will create a false revelation as a decoy."));
		SetPhase(QuestStatus.Success, "ZACHARIEL_GUARDIAN", "d_zachariel_36", L("Talk to the Secret Guardian"), L("All the sources are poured. Talk to the Secret Guardian."));

		AddPrerequisite(new QuestStatusPrerequisite(8388, QuestStatus.Completed));

		AddObjective("pourSources", L("Pour the magic sources into the jars"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 1));
	}
}

// 8390: The Guardian's Jar (3)
//-----------------------------------------------------------------------------
public class Zacha5fMq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8390);
		SetName(L("The Guardian's Jar (3)"));
		SetDescription(L("The soul pot has to stand where the mausoleum's will can fill it."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHARIEL_GUARDIAN", "d_zachariel_36", L("Talk to the Secret Guardian"), L("The evil presence has obtained a false revelation. Talk to the Secret Guardian."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_MQ_03", "d_zachariel_36", L("Place the Soul Pot"), L("Set the soul pot where the will of the Royal Mausoleum can fill it."));
		SetPhase(QuestStatus.Success, "ZACHARIEL_GUARDIAN", "d_zachariel_36", L("Talk to the Secret Guardian"), L("The soul pot is set. Talk to the Secret Guardian."));

		AddPrerequisite(new QuestStatusPrerequisite(8389, QuestStatus.Completed));

		AddObjective("placePot", L("Place the Soul Pot"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("ZACHA5F_MQ03_POT", 1));
	}
}

// 8391: The Guardian's Jar (4)
//-----------------------------------------------------------------------------
public class Zacha5fMq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8391);
		SetName(L("The Guardian's Jar (4)"));
		SetDescription(L("Rexipher works out what the false revelation was and comes back for the real one."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA5F_MQ_04", "d_zachariel_36", L("Give the pot to the Secret Guardian"), L("Hand the filled soul pot to the last guardian of the Royal Mausoleum."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_MQ_04", "d_zachariel_36", L("Defeat Rexipher"), L("Rexipher has come for the revelation itself. Defeat him."));
		SetPhase(QuestStatus.Success, "ZACHA5F_MQ_04", "d_zachariel_36", L("Defeat Rexipher"), L("Rexipher has come for the revelation itself. Defeat him."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA5F_MQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8390, QuestStatus.Completed));

		AddObjective("killRexipher", L("Defeat Rexipher"), new KillObjective(1, "boss_lecifer") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new TakeItemReward("ZACHA5F_MQ03_POT", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Rexipher is driven off. The burial chamber of the Great King lies open."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8392: The King of the Great Humans
//-----------------------------------------------------------------------------
public class Zacha5fMq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8392);
		SetName(L("The King of the Great Humans"));
		SetDescription(L("Great King Zachariel's will, and what Goddess Laima left with him."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "ZACHA5F_MQ_05", "d_zachariel_36", L("Go to the burial chamber of the Royal Mausoleum"), L("Rexipher is driven off. Go to the burial chamber of the Royal Mausoleum."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_MQ_05", "d_zachariel_36", L("Go to the burial chamber of the Royal Mausoleum"), L("Rexipher is driven off. Go to the burial chamber of the Royal Mausoleum."));
		SetPhase(QuestStatus.Success, "ZACHA5F_MQ_05", "d_zachariel_36", L("Listen to the advice of Zachariel"), L("Listen to what the Great King and the goddess have to say."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA5_SPIRIT_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(8391, QuestStatus.Completed));

		AddObjective("hearTheKing", L("Listen to the advice of Zachariel"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("stonetablet04", 1));
		AddReward(new ItemReward("COLLECT_117", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The cutscene is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("The portal to the Royal Mausoleum entrance is open!"));
		character.LookAround();
	}
}

// 8419: Guardian Stone Statue's Warning
//-----------------------------------------------------------------------------
public class Zacha5fEq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8419);
		SetName(L("Guardian Stone Statue's Warning"));
		SetDescription(L("Seven Venucelos in the lower hall are past coming back."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA5F_EQ_01", "d_zachariel_36", L("Read the epitaph"), L("Once a Guardian gets corrupted, it can never come back to its original status."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_EQ_01", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the Royal Mausoleum Guardians that attack you."));
		SetPhase(QuestStatus.Success, "ZACHA5F_EQ_01", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the Royal Mausoleum Guardians that attack you."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA5F_EQ_01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(83));

		AddObjective("killGuardians", L("Defeat the Royal Mausoleum Guardian"), new KillObjective(7, "dog_of_king") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kills are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The lower hall is clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8420: Guardian Stone Statue's Warning
//-----------------------------------------------------------------------------
public class Zacha5fEq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8420);
		SetName(L("Guardian Stone Statue's Warning"));
		SetDescription(L("Eight Venucelos hold the middle hall against anyone who disturbs the King."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA5F_EQ_02", "d_zachariel_36", L("Read the Royal Mausoleum gravestone"), L("Anyone who disturbs the King's rest will meet their end here."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_EQ_02", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the Royal Mausoleum Guardians that attack you."));
		SetPhase(QuestStatus.Success, "ZACHA5F_EQ_02", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the Royal Mausoleum Guardians that attack you."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA5F_EQ_02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(83));

		AddObjective("killGuardians", L("Defeat the Royal Mausoleum Guardian"), new KillObjective(8, "dog_of_king") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kills are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The middle hall is clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8421: Hidden Treasure Chest
//-----------------------------------------------------------------------------
public class Zacha5fEq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8421);
		SetName(L("Hidden Treasure Chest"));
		SetDescription(L("The floor around the side hall's chest is full of Medakia."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA5F_EQ_03", "d_zachariel_36", L("Find the hidden treasure"), L("A chest stands in the side hall of the Royal Mausoleum."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_EQ_03", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the interfering monsters."));
		SetPhase(QuestStatus.Success, "ZACHA5F_EQ_03", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the interfering monsters."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA5F_EQ_03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(84));

		AddObjective("killMedakia", L("Defeat the interfering monsters"), new KillObjective(7, "schlesien_darkmage") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kills are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The side hall is clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8422: Empty Slate
//-----------------------------------------------------------------------------
public class Zacha5fEq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8422);
		SetName(L("Empty Slate"));
		SetDescription(L("The Revelator's name is scraped off the desk's slate, and the Rusrat know it."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA5F_EQ_04", "d_zachariel_36", L("Read the epitaph"), L("The name of the Revelator has been erased and only the demon's name remains."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_EQ_04", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat Rusrat."));
		SetPhase(QuestStatus.Success, "ZACHA5F_EQ_04", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat Rusrat."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA5F_EQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(84));

		AddObjective("killRusrat", L("Defeat Rusrat"), new KillObjective(6, "schlesien_claw") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kills are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The desk hall is clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8423: Guardian Stone Statue's Warning
//-----------------------------------------------------------------------------
public class Zacha5fEq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8423);
		SetName(L("Guardian Stone Statue's Warning"));
		SetDescription(L("Mauros, Medakia and Rusrat hold the upper hall together."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_36");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA5F_EQ_05", "d_zachariel_36", L("Read the epitaph"), L("Destruction by the Revelator is a preferable alternative to corruption by the evil energy."));
		SetPhase(QuestStatus.InProgress, "ZACHA5F_EQ_05", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the Royal Mausoleum Guardians that attack you."));
		SetPhase(QuestStatus.Success, "ZACHA5F_EQ_05", "d_zachariel_36", L("Defeat the corrupted Royal Mausoleum Guardians"), L("Defeat the Royal Mausoleum Guardians that attack you."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA5F_EQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(84));

		AddObjective("killGuardians", L("Defeat the Royal Mausoleum Guardian"), new KillObjective(8, "schlesien_heavycavarly", "schlesien_darkmage", "schlesien_claw") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kills are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The upper hall is clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 50007: To Mage Tower
//-----------------------------------------------------------------------------
public class ZachaToFedimianQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50007);
		SetName(L("To Mage Tower"));
		SetDescription(L("The Revelation of the Royal Mausoleum points at the Mage Tower. Fedimian is the road to it."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_36", "f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA5F_MQ_04", "d_zachariel_36", L("Go to Fedimian"), L("The revelation names the Mage Tower. Make for Fedimian."));
		SetPhase(QuestStatus.InProgress, "WS_REMAINS40_TO_FEDMIAN", "f_remains_40", L("Go to Fedimian"), L("Take the Tombstone Path out of Zachariel Crossroads and follow it to the Fedimian Suburbs."));
		SetPhase(QuestStatus.Success, "WS_REMAINS40_TO_FEDMIAN", "f_remains_40", L("Go to Fedimian"), L("Take the Tombstone Path out of Zachariel Crossroads and follow it to the Fedimian Suburbs."));

		AddPrerequisite(new QuestStatusPrerequisite(8392, QuestStatus.Completed));

		AddObjective("reachFedimian", L("Go to Fedimian"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The arrival is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("You have reached the gate of Fedimian."));
		character.Quests.Complete(this.QuestId);
	}
}
