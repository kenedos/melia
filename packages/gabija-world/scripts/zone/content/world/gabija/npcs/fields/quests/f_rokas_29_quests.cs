//--- Melia Script ----------------------------------------------------------
// Rukas Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Historian Rexipher's walk along the four epitaphs of the Great King, and
// the adventurer Varkis who does not survive his own research.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas29QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq1 = new QuestId(20179);
	private readonly static QuestId Mq2 = new QuestId(20180);
	private readonly static QuestId Mq2Bridge = new QuestId(19330);
	private readonly static QuestId Mq3 = new QuestId(20181);
	private readonly static QuestId Mq4 = new QuestId(20182);
	private readonly static QuestId Mq4Bridge = new QuestId(19340);
	private readonly static QuestId Mq5 = new QuestId(20183);
	private readonly static QuestId Mq6 = new QuestId(20188);

	private readonly static QuestId Vacys1 = new QuestId(1058);
	private readonly static QuestId Vacys2 = new QuestId(1059);
	private readonly static QuestId Vacys3 = new QuestId(1060);
	private readonly static QuestId Vacys4 = new QuestId(1061);
	private readonly static QuestId Vacys5 = new QuestId(1062);
	private readonly static QuestId Vacys6 = new QuestId(1063);

	protected override void Load()
	{
		// Historian Rexipher, at the Isvalyta Historic Site
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Historian Rexipher"), "ROKAS29_MQ_REXITHER1", "f_rokas_29", 2332, 666, 90, c => !c.Quests.Has(Mq1), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (!character.Quests.Has(Mq1) && character.Quests.MeetsPrerequisites(Mq1))
			{
				await dialog.Msg(L("The Revelator is here?! Thank the goddess."));
				await dialog.Msg(L("I am Rexipher, a historian. I am studying the Royal Mausoleum."));

				var answer = await dialog.SelectQuestOffer(Mq1, L("Neither of us reaches the mausoleum alone. Will you read the epitaphs of the Great King with me?"),
					Option(L("I will cooperate"), "accept"),
					Option(L("I don't need it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1);
					await dialog.Msg(L("Oh, I didn't know I could count on you."));
					await dialog.Msg(L("I will see you at Isvalyta Historic Site."));
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("It is so good to meet a person like you, I guess this is divine grace."));
		});

		// Historian Rexipher, at Isvalyta Historic Site
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Historian Rexipher"), "ROKAS29_MQ_REXITHER2", "f_rokas_29", 1080.55, -785.35, 23, c => c.Quests.Has(Mq1) && !c.Quests.Has(Mq2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (character.Quests.IsActive(Mq1) && character.Quests.IsCompletable(Mq1))
			{
				await dialog.Msg(L("As I expected, this ordeal can only be passed to the Revelator."));
				await dialog.Msg(L("So that's it... it was prepared by the owner of the Royal Mausoleum after all."));
				await dialog.CompleteQuest(Mq1);
				return;
			}

			if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
			{
				await dialog.Msg(L("Well, it's no big issue."));
				await dialog.Msg(L("Everything has a reason for it."));

				var answer = await dialog.SelectQuestOffer(Mq2, L("The next epitaph stands at Serno Highland. Read it and I will follow you there."),
					Option(L("Find the next epitaph"), "accept"),
					Option(L("I can only help so much"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2);
					await dialog.Msg(L("I wonder what they tried to protect by building the Royal Mausoleum."));
					await dialog.Msg(L("I'd like to see it with my own eyes."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq1))
			{
				await dialog.Msg(L("The epitaph at Isvalyta Historic Site is just below us. Read it, and tell me what it says."));
				return;
			}

			await dialog.Msg(L("It is so good to meet a person like you, I guess this is divine grace."));
		});

		// Historian Rexipher, at Serno Highland
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Historian Rexipher"), "ROKAS29_MQ_REXITHER3", "f_rokas_29", 1764.48, 464.24, 360, c => c.Quests.HasCompleted(Mq2) && !c.Quests.Has(Mq3), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (!character.Quests.Has(Mq3) && character.Quests.MeetsPrerequisites(Mq3))
			{
				await dialog.Msg(L("Are you sure you're not hurt?"));

				var answer = await dialog.SelectQuestOffer(Mq3, L("The next tombstone is on the way to Dykyne Fork."),
					Option(L("Go to where the epitaph is"), "accept"),
					Option(L("I can only help so much"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq3);
					await dialog.Msg(L("The god is quiet."));
					await dialog.Msg(L("I think the tomb will be the same."));
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("The road to Dykyne Fork runs west from here."));
		});

		// Historian Rexipher, on the Dykyne Fork road
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Historian Rexipher"), "ROKAS29_MQ_REXITHER4", "f_rokas_29", -66.24, 574.99, 90, c => c.Quests.HasCompleted(Mq2) && !c.Quests.Has(Mq4), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (character.Quests.IsActive(Mq3) && character.Quests.IsCompletable(Mq3))
			{
				await dialog.Msg(L("It was dangerous."));
				await dialog.Msg(L("But conversely, we couldn't stop because it was dangerous."));
				await dialog.CompleteQuest(Mq3);
				return;
			}

			if (!character.Quests.Has(Mq4) && character.Quests.MeetsPrerequisites(Mq4))
			{
				await dialog.Msg(L("Are you hurt?"));
				await dialog.Msg(L("Oh, that's the second symbol of ordeal of the Great King."));

				var answer = await dialog.SelectQuestOffer(Mq4, L("The third epitaph is set into the rock left of Dykyne Fork."),
					Option(L("Move to where the next epitaph is"), "accept"),
					Option(L("I can only help so much"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq4);
					character.Inventory.RemoveItem(ItemId.ROKAS29_SLATE2, 1);
					await dialog.Msg(L("What do I want to obtain from the Royal Mausoleum?"));
					await dialog.Msg(L("That is a secret."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq3))
			{
				await dialog.Msg(L("The epitaph is further down the road. I dare not touch it alone."));
				return;
			}

			await dialog.Msg(L("The god is quiet. I think the tomb will be the same."));
		});

		// Historian Rexipher, left of Dykyne Fork
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Historian Rexipher"), "ROKAS29_MQ_REXITHER5", "f_rokas_29", -666.39, 389.69, 90, c => c.Quests.Has(Mq4) && !c.Quests.Has(Mq5), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (character.Quests.IsActive(Mq4) && character.Quests.IsCompletable(Mq4))
			{
				await dialog.Msg(L("Great King Zachariel's ordeal is not easy."));
				await dialog.Msg(L("With the skill you have, he should have acknowledged you."));
				await dialog.CompleteQuest(Mq4);
				return;
			}

			if (!character.Quests.Has(Mq5) && character.Quests.MeetsPrerequisites(Mq5))
			{
				var answer = await dialog.SelectQuestOffer(Mq5, L("The last epitaph is at Apatinis Cliff. I hope the Great King Zachariel will understand our sincerity."),
					Option(L("Move to where the last epitaph is located"), "accept"),
					Option(L("I can only help so much"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq5);
					await dialog.Msg(L("Soon, all of the symbols of the ordeals will be collected."));
					await dialog.Msg(L("I look forward to it."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq4))
			{
				await dialog.Msg(L("The epitaph is in the rock face behind me. Read it when you are ready."));
				return;
			}

			await dialog.Msg(L("What do I want to obtain from the Royal Mausoleum? That is a secret."));
		});

		// Historian Rexipher, at Apatinis Cliff
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Historian Rexipher"), "ROKAS29_MQ_REXITHER6", "f_rokas_29", -384.29, -413.12, 90, c => c.Quests.Has(Mq5) && !c.Quests.IsActive(Mq6) && !c.Quests.HasCompleted(Mq6), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (character.Quests.IsActive(Mq5) && character.Quests.IsCompletable(Mq5))
			{
				await dialog.Msg(L("Finally, you have collected all the symbols of the ordeals."));
				await dialog.Msg(L("Give me the symbols, please. I will start the interpretations."));
				await dialog.CompleteQuest(Mq5);
				return;
			}

			if (!character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6))
			{
				await dialog.Msg(L("Oh my, so that was the case..."));

				var answer = await dialog.SelectQuestOffer(Mq6, L("The symbols are covered with something, so it is difficult for me to examine them. Hogma teeth would serve as an abrasive."),
					Option(L("I'll go find it"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq6);
					await dialog.Msg(L("I will be waiting patiently."));
					await dialog.Msg(L("Why would we have to be in a hurry when we already have the symbols?"));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq5))
			{
				await dialog.Msg(L("The last epitaph is right over the cliff edge. Read it and bring me the symbol."));
				return;
			}

			await dialog.Msg(L("Soon, all of the symbols of the ordeals will be collected. I look forward to it."));
		});

		// Where Historian Rexipher was standing
		//-------------------------------------------------------------------------
		// The client's turn-in for 20188 is a hidden trigger on the spot Rexipher
		// vanishes from, so the site itself takes the teeth.
		AddConditionalNpc(20026, L("Rexipher's Campsite"), "ROKAS29_MQ_REXITHERLOST", "f_rokas_29", -368.21, -428.27, 90, c => c.Quests.IsActive(Mq6), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Rexipher's Campsite"));

			if (character.Quests.IsActive(Mq6) && character.Quests.IsCompletable(Mq6))
			{
				await dialog.Msg(L("You came back with the Hogma teeth, but Rexipher is nowhere to be seen."));
				await dialog.Msg(L("His lantern is still burning, and the symbols are gone with him."));
				await dialog.CompleteQuest(Mq6);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("Rexipher's things are here, but Rexipher is not."));
		});

		// Zachariel's Epitaph, Isvalyta Historic Site
		//-------------------------------------------------------------------------
		AddNpc(153053, L("Zachariel's Epitaph"), "ROKAS29_MQ_DEVICE1", "f_rokas_29", 1073.31, -819.92, 27, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zachariel's Epitaph"));

			if (character.Quests.IsActive(Mq1) && !character.Quests.IsCompletable(Mq1))
			{
				var read = await character.TimeActions.StartAsync(L("Examining the epitaph..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.ROKAS29_SLATE1, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Mq1, "readFirstEpitaph");

				await dialog.Msg(L("Four ordeals were prepared so that our enemies cannot reach the message."));
				await dialog.Msg(L("However, if you are unprepared, even you won't be able to overcome those ordeals."));
				return;
			}

			await dialog.Msg(L("Wispy words float off the stone and fade before they can be read."));
		});

		// Zachariel's Epitaph, Serno Highland
		//-------------------------------------------------------------------------
		AddNpc(153053, L("Zachariel's Epitaph"), "ROKAS29_MQ_DEVICE2", "f_rokas_29", 1343, 311, 27, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zachariel's Epitaph"));

			if (character.Quests.IsActive(Mq2) && character.Quests.IsCompletable(Mq2))
			{
				await dialog.Msg(L("Barely got rid of the guardians of the epitaph, but Rexipher did not show up."));
				await dialog.Msg(L("The second symbol of the ordeal lifts off the stone and settles in your hand."));
				await dialog.CompleteQuest(Mq2);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq2Bridge) && character.Quests.IsCompletable(Mq2Bridge))
			{
				await dialog.Msg(L("Got rid of the monsters. Now the epitaph can be read in peace."));
				await dialog.CompleteQuest(Mq2Bridge);
				return;
			}

			if (character.Quests.IsActive(Mq2))
			{
				var read = await character.TimeActions.StartAsync(L("Examining the epitaph..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq2);
				return;
			}

			if (character.Quests.IsActive(Mq2Bridge))
			{
				character.Quests.StartQuestTrack(Mq2Bridge);
				return;
			}

			if (!character.Quests.Has(Mq2Bridge) && !character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2Bridge))
			{
				var answer = await dialog.SelectQuestOffer(Mq2Bridge, L("There are epitaphs about the Royal Mausoleum in Rukas Plateau. This is one of them."),
					Option(L("Check the epitaph"), "accept"),
					Option(L("Leave the stone alone"), "leave")
				);

				if (answer == "accept")
				{
					var read = await character.TimeActions.StartAsync(L("Examining the epitaph..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

					if (read != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq2Bridge);
					character.Quests.StartQuestTrack(Mq2Bridge);
				}
				return;
			}

			await dialog.Msg(L("An epitaph of the Great King, worn down by the highland wind."));
		});

		// Zachariel's Epitaph, on the Dykyne Fork road
		//-------------------------------------------------------------------------
		// The client ships no anchor for the third epitaph; its position is the one
		// its own cutscene spawns it at.
		AddNpc(47106, L("Zachariel's Epitaph"), "ROKAS29_MQ_DEVICE3", "f_rokas_29", -206, 495, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zachariel's Epitaph"));

			if (character.Quests.IsActive(Mq3) && !character.Quests.IsCompletable(Mq3))
			{
				character.Inventory.Add(ItemId.ROKAS29_SLATE3, 1, InventoryAddType.PickUp);
				character.Quests.StartQuestTrack(Mq3);
				return;
			}

			await dialog.Msg(L("A third epitaph, half buried where the road bends towards Dykyne Fork."));
		});

		// Zachariel's Epitaph, left of Dykyne Fork
		//-------------------------------------------------------------------------
		AddNpc(153053, L("Zachariel's Epitaph"), "ROKAS29_MQ_DEVICE4", "f_rokas_29", -680, 360, 27, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zachariel's Epitaph"));

			if (character.Quests.IsActive(Mq4Bridge) && character.Quests.IsCompletable(Mq4Bridge))
			{
				await dialog.Msg(L("Barely got rid of the monsters. Now the epitaph can be read thoroughly."));
				await dialog.CompleteQuest(Mq4Bridge);
				return;
			}

			if (character.Quests.IsActive(Mq4) && !character.Quests.IsCompletable(Mq4))
			{
				var read = await character.TimeActions.StartAsync(L("Examining the epitaph..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.ROKAS29_SLATE4, 1, InventoryAddType.PickUp);
				character.Quests.StartQuestTrack(Mq4);
				return;
			}

			if (character.Quests.IsActive(Mq4Bridge))
			{
				character.Quests.StartQuestTrack(Mq4Bridge);
				return;
			}

			if (!character.Quests.Has(Mq4Bridge) && !character.Quests.Has(Mq4) && character.Quests.MeetsPrerequisites(Mq4Bridge))
			{
				var answer = await dialog.SelectQuestOffer(Mq4Bridge, L("The stone on the left of Dykyne Fork carries the same hand as the others."),
					Option(L("Check the epitaph"), "accept"),
					Option(L("Leave the stone alone"), "leave")
				);

				if (answer == "accept")
				{
					var read = await character.TimeActions.StartAsync(L("Examining the epitaph..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

					if (read != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq4Bridge);
					character.Quests.StartQuestTrack(Mq4Bridge);
				}
				return;
			}

			await dialog.Msg(L("An epitaph of the Great King, set into the rock left of the fork."));
		});

		// Zachariel's Epitaph, Apatinis Cliff
		//-------------------------------------------------------------------------
		AddNpc(153053, L("Zachariel's Epitaph"), "ROKAS29_MQ_DEVICE5", "f_rokas_29", -332, -398, 27, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zachariel's Epitaph"));

			if (character.Quests.IsActive(Mq5) && !character.Quests.IsCompletable(Mq5))
			{
				var read = await character.TimeActions.StartAsync(L("Examining it..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.ROKAS29_SLATE5, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Mq5, "readLastEpitaph");

				character.ServerMessage(L("The fourth symbol of the ordeal is yours. Take it back to Rexipher."));
				return;
			}

			await dialog.Msg(L("The last of the four epitaphs, looking out over Apatinis Cliff."));
		});

		// Adventurer Varkis
		//-------------------------------------------------------------------------
		AddConditionalNpc(152000, L("Adventurer Varkis"), "VACYS_LIVE", "f_rokas_29", 1641.21, 757.55, 360, c => !c.Quests.HasCompleted(Vacys2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Adventurer Varkis"));

			if (character.Quests.IsActive(Vacys2) && character.Quests.IsCompletable(Vacys2))
			{
				await dialog.Msg(L("There are still a lot of things I must do..."));
				await dialog.CompleteQuest(Vacys2);

				character.ServerMessage(L("Varkis did not last the hour. Lay his journal to rest at his own camp."));
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Vacys1) && character.Quests.IsCompletable(Vacys1))
			{
				await dialog.Msg(L("My journal! You found it after all."));
				await dialog.CompleteQuest(Vacys1);

				if (!character.Quests.Has(Vacys2))
					character.Quests.Start(Vacys2);

				return;
			}

			if (!character.Quests.Has(Vacys1) && character.Quests.MeetsPrerequisites(Vacys1))
			{
				await dialog.Msg(L("I'm screwed. Monsters took my bag."));

				var answer = await dialog.SelectQuestOffer(Vacys1, L("The bag contained everything I recorded on my journey... What should I do?"),
					Option(L("I can find it"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Vacys1);
					await dialog.Msg(L("The goddess has surely sent help!"));
					await dialog.Msg(L("Those monsters were last seen south of here."));
				}
				return;
			}

			if (!character.Quests.Has(Vacys2) && character.Quests.MeetsPrerequisites(Vacys2))
			{
				await dialog.Msg(L("My journal... Something is wrong. Stay close."));
				character.Quests.Start(Vacys2);
				return;
			}

			if (character.Quests.IsActive(Vacys1))
			{
				await dialog.Msg(L("The bag is somewhere south of here, below Serno Highland."));
				return;
			}

			if (character.Quests.IsActive(Vacys2))
			{
				await dialog.Msg(L("Lithorex - get it off me!"));
				character.Quests.ReplayQuestTrack(Vacys2);
				return;
			}

			await dialog.Msg(L("Every road on this plateau is worth a page. If only they would let me write it."));
		});

		// Adventurer Varkis' Spirit
		//-------------------------------------------------------------------------
		AddConditionalNpc(152000, L("Adventurer Varkis' Spirit"), "VACYS_SOUL", "f_rokas_29", 168.59, 792.08, 360, c => c.Quests.HasCompleted(Vacys2) && !c.Quests.HasCompleted(Vacys6), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Adventurer Varkis' Spirit"));

			if (character.Quests.IsActive(Vacys3) && character.Quests.IsCompletable(Vacys3))
			{
				await dialog.Msg(L("That is the first of them. Thank you."));
				await dialog.CompleteQuest(Vacys3);

				character.ServerMessage(L("The notes say the next batch is hidden on the way to Neck Cliff of Snake."));
				return;
			}

			if (character.Quests.IsActive(Vacys4) && character.Quests.IsCompletable(Vacys4))
			{
				await dialog.Msg(L("Unknocker was sitting on it. Of course it was."));
				await dialog.CompleteQuest(Vacys4);

				character.ServerMessage(L("The notes say the last batch is hidden at Apatinis Cliff."));
				return;
			}

			if (character.Quests.IsActive(Vacys5) && character.Quests.IsCompletable(Vacys5))
			{
				await dialog.Msg(L("If I had the journal, I could continue my adventure, but I wasn't chosen..."));
				await dialog.Msg(L("But for you things might be different..."));
				await dialog.CompleteQuest(Vacys5);

				character.ServerMessage(L("Varkis' spirit leaves the completed research in your hands."));
				return;
			}

			if (character.Quests.IsActive(Vacys6) && character.Quests.IsCompletable(Vacys6))
			{
				await dialog.Msg(L("The smoke carries well. That is enough of an ending for me."));
				await dialog.CompleteQuest(Vacys6);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Vacys3) && character.Quests.MeetsPrerequisites(Vacys3))
			{
				await dialog.Msg(L("If only I had a little more time..."));

				var answer = await dialog.SelectQuestOffer(Vacys3, L("My documents... I can't leave them..."),
					Option(L("I will find it"), "accept"),
					Option(L("Leave"), "leave")
				);

				if (answer == "accept")
				{
					character.Inventory.RemoveItem(ItemId.VACYS_Note, 1);
					character.Quests.Start(Vacys3);
					await dialog.Msg(L("These things are useless to me now..."));
					await dialog.Msg(L("Okay. Maybe you... My documents are at Dykyne Fork..."));
				}
				return;
			}

			if (!character.Quests.Has(Vacys6) && character.Quests.MeetsPrerequisites(Vacys6))
			{
				var answer = await dialog.SelectQuestOffer(Vacys6, L("The research is finished, and I have no more use for paper. Burn it at my camp, and let me go."),
					Option(L("I'll burn it at the camp"), "accept"),
					Option(L("Cancel"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Vacys6);

				return;
			}

			if (character.Quests.IsActive(Vacys3))
			{
				await dialog.Msg(L("Dykyne Fork. The ground there is soft enough to dig."));
				return;
			}

			if (character.Quests.IsActive(Vacys4))
			{
				await dialog.Msg(L("Neck Cliff of Snake. Watch the rocks above you."));
				return;
			}

			if (character.Quests.IsActive(Vacys5))
			{
				await dialog.Msg(L("Apatinis Cliff. The Hogma walk that line often."));
				return;
			}

			if (character.Quests.IsActive(Vacys6))
			{
				await dialog.Msg(L("The bonfire is still where I left it. Put the papers in."));
				return;
			}

			await dialog.Msg(L("An adventure ends where the notes do."));
		});

		// Adventurer's Bag
		//-------------------------------------------------------------------------
		AddConditionalNpc(47161, L("Adventurer's Bag"), "ROKAS29_BAG", "f_rokas_29", 2265, 220, 90, c => c.Quests.IsActive(Vacys1) && !c.Quests.IsCompletable(Vacys1), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Adventurer's Bag"));

			if (character.Quests.IsActive(Vacys1) && !character.Quests.IsCompletable(Vacys1))
			{
				var searched = await character.TimeActions.StartAsync(L("Checking it..."), L("Cancel"), "#SITGROPESET2", TimeSpan.FromSeconds(3.5));

				if (searched != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.VACYS_Note, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Vacys1, "findJournal");

				character.ServerMessage(L("You found Varkis' journal. Take it back to him."));
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A torn pack, emptied out across the slope."));
		});

		// Varkis' cache at Dykyne Fork
		//-------------------------------------------------------------------------
		AddNpc(155026, L("Disturbed Ground"), "ROKAS29_SLATE1", "f_rokas_29", -1184, 320, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Disturbed Ground"));

			if (character.Quests.IsActive(Vacys3) && !character.Quests.IsCompletable(Vacys3))
			{
				var dug = await character.TimeActions.StartAsync(L("Searching for Varkis' materials..."), L("Cancel"), "SITGROPE_LOOP", TimeSpan.FromSeconds(3));

				if (dug != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.ROKAS29_VACYS_ITEM_1, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Vacys3, "digDykyne");

				character.ServerMessage(L("Varkis' research materials, wrapped against the damp."));
				return;
			}

			await dialog.Msg(L("Ground that has been turned over and patted flat again."));
		});

		// Varkis' cache at Neck Cliff of Snake
		//-------------------------------------------------------------------------
		AddNpc(155026, L("Disturbed Ground"), "ROKAS29_SLATE2", "f_rokas_29", -1746.24, 700.19, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Disturbed Ground"));

			if (character.Quests.IsActive(Vacys4) && !character.Quests.IsCompletable(Vacys4))
			{
				await dialog.Msg(L("The ground has been dug out already, and whatever did it is still close."));
				character.Quests.ReplayQuestTrack(Vacys4);
				return;
			}

			if (!character.Quests.Has(Vacys4) && character.Quests.MeetsPrerequisites(Vacys4))
			{
				var dug = await character.TimeActions.StartAsync(L("Searching for the materials Varkis hid..."), L("Cancel"), "#SITGROPESET2", TimeSpan.FromSeconds(3));

				if (dug != TimeActionResult.Completed)
					return;

				character.Quests.Start(Vacys4);
				character.Quests.StartQuestTrack(Vacys4);
				return;
			}

			await dialog.Msg(L("Ground that has been turned over and patted flat again."));
		});

		// Varkis' cache at Apatinis Cliff
		//-------------------------------------------------------------------------
		AddNpc(155026, L("Disturbed Ground"), "ROKAS29_SLATE3", "f_rokas_29", -792, -567, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Disturbed Ground"));

			if (character.Quests.IsActive(Vacys5) && !character.Quests.IsCompletable(Vacys5))
			{
				await dialog.Msg(L("The Hogma are still on this line. Clear them before you dig again."));
				character.Quests.ReplayQuestTrack(Vacys5);
				return;
			}

			if (!character.Quests.Has(Vacys5) && character.Quests.MeetsPrerequisites(Vacys5))
			{
				var dug = await character.TimeActions.StartAsync(L("Searching for the materials Varkis hid..."), L("Cancel"), "#SITGROPESET2", TimeSpan.FromSeconds(3));

				if (dug != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.ROKAS29_VACYS_ITEM_3, 1, InventoryAddType.PickUp);
				character.Quests.Start(Vacys5);
				character.Quests.StartQuestTrack(Vacys5);
				return;
			}

			await dialog.Msg(L("Ground that has been turned over and patted flat again."));
		});

		// Varkis' bonfire
		//-------------------------------------------------------------------------
		AddNpc(46011, L("Bonfire"), "ROKAS29_FIRE", "f_rokas_29", 163, 769, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bonfire"));

			if (character.Quests.IsActive(Vacys6) && !character.Quests.IsCompletable(Vacys6))
			{
				var burned = await character.TimeActions.StartAsync(L("Burning it..."), L("Cancel"), "FIRE", TimeSpan.FromSeconds(2));

				if (burned != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Vacys6, "burnResearch");
				character.Quests.StartQuestTrack(Vacys6);
				return;
			}

			await dialog.Msg(L("A camp fire that has been kept alive longer than the man who lit it."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The approach to Varkis' post, where Lithorex breaks out of the rock.
		AddQuestTrigger("VACYS_LIVE_ENTER", "f_rokas_29", 1549, 539, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Vacys2) && !character.Quests.IsCompletable(Vacys2))
				character.Quests.StartQuestTrack(Vacys2);

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20179: Historian Rexipher's Research (1)
//-----------------------------------------------------------------------------
public class Rokas29Mq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20179);
		SetName(L("Historian Rexipher's Research (1)"));
		SetDescription(L("Historian Rexipher waits along the path to the Royal Mausoleum, and wants the epitaphs of the Great King read."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_REXITHER1", "f_rokas_29", L("Talk to Historian Rexipher"), L("Historian Rexipher is waiting for you along the path to the Royal Mausoleum. Talk to Rexipher in Rukas Plateau."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_DEVICE1", "f_rokas_29", L("Read the epitaph of Isvalyta Historic Site"), L("Historian Rexipher is asking for your cooperation for the common objective of getting to the Royal Mausoleum. Follow his guide and read the epitaph of Isvalyta Historic Site."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_REXITHER2", "f_rokas_29", L("Talk to Historian Rexipher"), L("You have obtained the symbols of the ordeal of the Great King. Talk to Historian Rexipher."));

		// The 2016 chain hands off from the Revelation of Kvailas Forest.
		AddPrerequisite(new QuestStatusPrerequisite(18010, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(63));

		AddObjective("readFirstEpitaph", L("Read the epitaph of Isvalyta Historic Site"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new ItemReward("Drug_SP2_Q", 20));
		AddReward(new TakeItemReward("ROKAS29_SLATE1", 1));
	}
}

// 20180: Historian Rexipher's Research (2)
//-----------------------------------------------------------------------------
public class Rokas29Mq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20180);
		SetName(L("Historian Rexipher's Research (2)"));
		SetDescription(L("The next epitaph stands at Serno Highland, and it is guarded."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_REXITHER2", "f_rokas_29", L("Talk to Historian Rexipher"), L("The epitaph tells you about the ordeal which relates to the authentication of the Revelator. Talk to Historian Rexipher about the location of the next epitaph."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_DEVICE2", "f_rokas_29", L("Read the epitaph at Serno Highland"), L("Rexipher says the next epitaph is at Serno Highland. Go there and check the next epitaph."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_DEVICE2", "f_rokas_29", L("Read the epitaph at Serno Highland"), L("Barely got rid of the guardians of the epitaph, but Rexipher did not show up. Read the epitaph again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_MQ2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20179, QuestStatus.Completed));

		AddObjective("killGuardians", L("Defeat the monsters near the epitaph"), new KillObjective(5, "Zinute_Big", "zinutekas_Q1") { LayerOnly = true });

		AddReward(new ItemReward("ROKAS29_SLATE2", 1));
		AddReward(new ItemReward("expCard5", 1));
	}
}

// 19330: Epitaph of Serno Highland
//-----------------------------------------------------------------------------
public class Rokas29Mq2BridgeQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19330);
		SetName(L("Epitaph of Serno Highland"));
		SetDescription(L("The epitaph at Serno Highland throws off its guardians the moment it is touched."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_DEVICE2", "f_rokas_29", L("Check the epitaph"), L("There are epitaphs about the Royal Mausoleum in Rukas Plateau. Check the epitaph of Serno Highland, which is one of them."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_DEVICE2", "f_rokas_29", L("Defeat the monsters nearby the epitaph"), L("Monsters appeared as if to protect the epitaph when you touched it! Defeat the monsters first then check the epitaph."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_DEVICE2", "f_rokas_29", L("Check the epitaph again"), L("Got rid of the monsters. Now, check the epitaph again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_MQ2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killGuardians", L("Defeat the monsters near the epitaph"), new KillObjective(5, "Zinute_Big", "zinutekas", "zinutekas_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 20181: Historian Rexipher's Research (3)
//-----------------------------------------------------------------------------
public class Rokas29Mq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20181);
		SetName(L("Historian Rexipher's Research (3)"));
		SetDescription(L("The third epitaph lies on the road to Dykyne Fork, and its guardian nearly takes Rexipher with it."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_REXITHER3", "f_rokas_29", L("Talk to Historian Rexipher"), L("To get near the Royal Mausoleum, you should pass all the ordeals of the Great King Zachariel. To get the next token for the next ordeal, talk with Historian Rexipher."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_DEVICE3", "f_rokas_29", L("Read the third epitaph along the way to Dykyne Fork"), L("The next epitaph is at Dykyne Fork. Find the epitaph near the road."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_REXITHER4", "f_rokas_29", L("Talk to Historian Rexipher"), L("The power of the guardian on the epitaph almost got Historian Rexipher in trouble. Talk to him again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_MQ3_TRACK", 4000, autoStart: false, partyPlay: true);

		// The client's only gate is a server-side script; the chain step it sits
		// on is the substitute.
		AddPrerequisite(new QuestStatusPrerequisite(20180, QuestStatus.Completed));

		AddObjective("killAttackers", L("Defeat the attacking monsters"), new KillObjective(25, "hogma_warrior", "Hogma_combat", "zinutekas") { LayerOnly = true });

		AddReward(new TakeItemReward("ROKAS29_SLATE3"));
	}
}

// 20182: Historian Rexipher's Research (3)
//-----------------------------------------------------------------------------
public class Rokas29Mq4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20182);
		SetName(L("Historian Rexipher's Research (3)"));
		SetDescription(L("The third symbol is cut into the rock left of Dykyne Fork, and the Zinutekas guard it."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_REXITHER4", "f_rokas_29", L("Find Historian Rexipher"), L("It is the third ordeal that was prepared by the Great King Zachariel. Read the epitaph on the left side of Dykyne Fork to obtain the third symbol of the ordeal."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_DEVICE4", "f_rokas_29", L("Read the epitaph left of Dykyne Fork"), L("It is the third ordeal that was prepared by the Great King Zachariel. Read the epitaph on the left side of Dykyne Fork to obtain the third symbol of the ordeal."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_REXITHER5", "f_rokas_29", L("Talk to Historian Rexipher"), L("You have obtained the third symbol of the ordeal. Return to Historian Rexipher and talk to him."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_MQ4_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20180, QuestStatus.Completed));

		AddObjective("killZinutekas", L("Defeat Zinutekas"), new KillObjective(7, "zinutekas_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new TakeItemReward("ROKAS29_SLATE4"));
	}
}

// 19340: Epitaph at Dykyne Fork on the right
//-----------------------------------------------------------------------------
public class Rokas29Mq4BridgeQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19340);
		SetName(L("Epitaph at Dykyne Fork on the right"));
		SetDescription(L("The epitaph left of Dykyne Fork sprouts Zinutekas before it can be read."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_DEVICE4", "f_rokas_29", L("Check the epitaph"), L("There are many epitaphs with the secret of the Royal Mausoleum in Rukas Plateau. Check the Epitaph at the left side of Dykyne Fork."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_DEVICE4", "f_rokas_29", L("Defeat the monsters"), L("Monsters sprouted out before you even got to check the epitaph! Defeat them first then check the epitaph again."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_DEVICE4", "f_rokas_29", L("Check the epitaph again"), L("Barely got rid of the monsters. Now check the epitaph thoroughly."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_MQ4_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killZinutekas", L("Defeat Zinutekas"), new KillObjective(7, "zinutekas", "zinutekas_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new ItemReward("ROKAS29_SLATE4", 1));
	}
}

// 20183: Historian Rexipher's Research (4)
//-----------------------------------------------------------------------------
public class Rokas29Mq5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20183);
		SetName(L("Historian Rexipher's Research (4)"));
		SetDescription(L("One epitaph is left, on the lip of Apatinis Cliff."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_REXITHER5", "f_rokas_29", L("Talk to Historian Rexipher"), L("There is only one epitaph remaining until you pass all the ordeals. Talk to Historian Rexipher about the location of the epitaph."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_DEVICE5", "f_rokas_29", L("Read the last epitaph at Apatinis Cliff"), L("This is the last ordeal prepared by the Great King Zachariel. Read the epitaph at Apatinis Cliff and obtain the fourth symbol."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_REXITHER6", "f_rokas_29", L("Talk to Historian Rexipher"), L("You have obtained the fourth symbol of the ordeal. Return to Historian Rexipher and talk to him again."));

		AddPrerequisite(new QuestStatusPrerequisite(20182, QuestStatus.Completed));

		AddObjective("readLastEpitaph", L("Read the last epitaph at Apatinis Cliff"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new ItemReward("Drug_SP2_Q", 25));
		AddReward(new TakeItemReward("ROKAS29_SLATE5"));
	}
}

// 20188: Historian Rexipher's Research (5)
//-----------------------------------------------------------------------------
public class Rokas29Mq6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20188);
		SetName(L("Historian Rexipher's Research (5)"));
		SetDescription(L("The symbols are covered over, and Rexipher wants Hogma teeth to grind them clean."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_MQ_REXITHER6", "f_rokas_29", L("Talk to Historian Rexipher"), L("Historian Rexipher is thinking deeply about something as he sees the symbols. Talk to him again."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_MQ_REXITHERLOST", "f_rokas_29", L("Get Hogma Teeth"), L("Historian Rexipher says that the symbols are covered with something so he asked you to get the teeth of Hogma that will be used as abrasives. Obtain Hogma's Tooth."));
		SetPhase(QuestStatus.Success, "ROKAS29_MQ_REXITHERLOST", "f_rokas_29", L("Talk to Historian Rexipher"), L("Got Hogma's teeth. Talk to Historian Rexipher."));

		AddPrerequisite(new QuestStatusPrerequisite(20183, QuestStatus.Completed));

		AddPityDrop("ROKAS29_MQ_TOXI", 0.4f, 5, 1, "Hogma_combat", "hogma_warrior");

		AddObjective("collectTeeth", L("Collect Hogma Teeth"), new CollectItemObjective("ROKAS29_MQ_TOXI", 5));

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new SelectItemReward("STF02_120", "TSF02_120", "TBW02_123", "BOW02_119"));
		AddReward(new TakeItemReward("ROKAS29_MQ_TOXI", 5));
	}
}

// 1058: Adventurer's Favor (1)
//-----------------------------------------------------------------------------
public class Rokas29Vacys1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1058);
		SetName(L("Adventurer's Favor (1)"));
		SetDescription(L("Monsters took the adventurer Varkis' bag, and his whole journey is written in it."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VACYS_LIVE", "f_rokas_29", L("Talk to Adventurer Varkis"), L("Explorer Varkis seems like he's in trouble. Talk to him."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_BAG", "f_rokas_29", L("Retrieve the Research Materials from Varkis' lost bag"), L("Adventurer Varkis told you that monsters stole the bag and ran away. Retrieve his diary by finding the bag at way below the Serno Highland."));
		SetPhase(QuestStatus.Success, "VACYS_LIVE", "f_rokas_29", L("Talk to Adventurer Varkis"), L("Found Varkis' Research. Return to Varkis and give it to him."));

		AddPrerequisite(new LevelPrerequisite(63));

		AddObjective("findJournal", L("Retrieve the Research Materials from Varkis' lost bag"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 1059: Adventurer's Favor (2)
//-----------------------------------------------------------------------------
public class Rokas29Vacys2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1059);
		SetName(L("Adventurer's Favor (2)"));
		SetDescription(L("Lithorex comes out of the rock face on top of Varkis."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VACYS_LIVE_ENTER", "f_rokas_29", L("Talk to Adventurer Varkis"), L("Talk to Adventurer Varkis."));
		SetPhase(QuestStatus.InProgress, "VACYS_LIVE_ENTER", "f_rokas_29", L("Rescue Varkis who is under attack"), L("Varkis is being attacked by Lithorex! Defeat Lithorex and rescue him."));
		SetPhase(QuestStatus.Success, "VACYS_LIVE", "f_rokas_29", L("Check on Adventurer Varkis"), L("You barely defeated Lithorex, but Varkis seems to be badly hurt. Check his status."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_VACYS2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1058, QuestStatus.Completed));

		AddObjective("killLithorex", L("Defeat Lithorex"), new KillObjective(1, "boss_Lithorex_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
	}
}

// 1060: The Eternal Adventure (1)
//-----------------------------------------------------------------------------
public class Rokas29Vacys3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1060);
		SetName(L("The Eternal Adventure (1)"));
		SetDescription(L("Varkis' spirit cannot leave his research behind. The first cache is at Dykyne Fork."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VACYS_SOUL", "f_rokas_29", L("Go to Camp of Varkis"), L("Varkis passed away. Return to the camp where Varkis used to reside and meet his spirit."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_SLATE1", "f_rokas_29", L("Collect Varkis' Research Materials at Dykyne Fork"), L("It seems that Varkis still has some regrets concerning the research that he used to do when he was alive. Collect his materials at Dykyne Fork."));
		SetPhase(QuestStatus.Success, "VACYS_SOUL", "f_rokas_29", L("Talk to Adventurer Varkis' Spirit"), L("Collected the first of Varkis' hidden research materials. Take them to his spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(1059, QuestStatus.Completed));

		AddObjective("digDykyne", L("Collect Varkis' Research Materials at Dykyne Fork"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 1061: The Eternal Adventure (2)
//-----------------------------------------------------------------------------
public class Rokas29Vacys4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1061);
		SetName(L("The Eternal Adventure (2)"));
		SetDescription(L("Unknocker was sitting on the second cache at Neck Cliff of Snake."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_SLATE2", "f_rokas_29", L("Collect Varkis' Research Materials at Neck Cliff of Snake"), L("It seems that Varkis still has some regrets concerning the research that he used to do when he was alive. Collect his materials at Neck Cliff of Snake."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_SLATE2", "f_rokas_29", L("Defeat Unknocker and take back the materials"), L("Unknocker appeared out of nowhere! It may be holding the research materials, so put it down."));
		SetPhase(QuestStatus.Success, "VACYS_SOUL", "f_rokas_29", L("Talk to Adventurer Varkis' Spirit"), L("Took the second batch of research materials off Unknocker. Take them to Varkis' spirit."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_VACYS4_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1060, QuestStatus.Completed));

		AddPityDrop("ROKAS29_VACYS_ITEM_2", 1.0f, 0, 1, "boss_Unknocker_Q1");

		AddObjective("takeMaterials", L("Defeat Unknocker and get Varkis' Research Materials"), new CollectItemObjective("ROKAS29_VACYS_ITEM_2", 1));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("misc_gemExpStone01", 1));
	}
}

// 1062: The Eternal Adventure (3)
//-----------------------------------------------------------------------------
public class Rokas29Vacys5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1062);
		SetName(L("The Eternal Adventure (3)"));
		SetDescription(L("The last cache is at Apatinis Cliff, and a Hogma patrol walks that line."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_SLATE3", "f_rokas_29", L("Look for Varkis' Research Materials near Apatinis Cliff"), L("Seems like Varkis is still attached to his research work. Collect his data around the Apatinis Cliff."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_SLATE3", "f_rokas_29", L("Defeat the monsters"), L("Monsters are attacking. You better defeat them quickly."));
		SetPhase(QuestStatus.Success, "VACYS_SOUL", "f_rokas_29", L("Talk to Adventurer Varkis' Spirit"), L("It seems that Varkis doesn't have regrets concerning his research anymore. Talk to him again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_VACYS5_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1061, QuestStatus.Completed));

		AddObjective("killHogma", L("Defeat Hogma"), new KillObjective(5, "Hogma_combat", "Hogma_guard") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new ItemReward("VACYS_note_COM", 1));
		AddReward(new SelectItemReward("SWD02_125", "TSW02_121", "MAC02_122", "SPR02_117", "TSP02_112"));
		AddReward(new TakeItemReward("ROKAS29_VACYS_ITEM_1"));
		AddReward(new TakeItemReward("ROKAS29_VACYS_ITEM_2"));
		AddReward(new TakeItemReward("ROKAS29_VACYS_ITEM_3"));
	}
}

// 1063: The Eternal Adventure (4)
//-----------------------------------------------------------------------------
public class Rokas29Vacys6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1063);
		SetName(L("The Eternal Adventure (4)"));
		SetDescription(L("Burn the finished research at Varkis' own camp and let him go."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS29_FIRE", "f_rokas_29", L("Burn the research diary on the fire place"), L("Varkis is now relieved for his research. Gather the data and burn it to comfort his soul."));
		SetPhase(QuestStatus.InProgress, "ROKAS29_FIRE", "f_rokas_29", L("Burn the research diary on the fire place"), L("Varkis is now relieved for his research. Gather the data and burn it to comfort his soul."));
		SetPhase(QuestStatus.Success, "VACYS_SOUL", "f_rokas_29", L("Talk to Adventurer Varkis' Spirit"), L("You burned all diaries to calm his soul. Talk to the spirit of the adventurer, Varkis."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS29_VACYS6_TRACK", 4000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(1062, QuestStatus.Completed));

		AddObjective("burnResearch", L("Burn the research diary on the fire place"), new ManualObjective());

		AddReward(new TakeItemReward("VACYS_note_COM"));
	}
}
