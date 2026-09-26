//--- Melia Script ----------------------------------------------------------
// Novaha Assembly Hall Quest NPCs
//--- Description -----------------------------------------------------------
// The rescue of the Croa villagers locked in the Special Reading Room,
// the demons' experiment victims, and the memorial to those who died.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey641QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Bracken633Mq050 = new QuestId(50112);
	private readonly static QuestId Mq010 = new QuestId(50117);
	private readonly static QuestId Mq020 = new QuestId(50118);
	private readonly static QuestId Mq030 = new QuestId(50119);
	private readonly static QuestId Mq040 = new QuestId(50120);
	private readonly static QuestId Mq050 = new QuestId(50121);
	private readonly static QuestId Sq010 = new QuestId(50122);
	private readonly static QuestId Sq020 = new QuestId(50123);
	private readonly static QuestId Sq030 = new QuestId(50124);
	private readonly static QuestId Hq1 = new QuestId(50276);
	private readonly static QuestId Hq2 = new QuestId(50277);

	private const string RelicVar = "Gabija.Quests.Abbay641Sq030.Relic";

	private static readonly double[,] Relics =
	{
		{ -1366.56, -852.24, 62 }, { -1206.70, -1153.53, 90 }, { -802.72, -1072.04, -32 }, { -772.68, -811.09, 28 },
		{ -1074.69, -1016.40, 55 }, { -1174.81, -614.86, 90 }, { -1028.56, -790.52, 0 },
	};

	private static readonly double[,] Stones =
	{
		{ -32.24, 1050.34, 2 }, { 565.10, 682.31, 10 }, { 448.37, 1123.94, 216 }, { 268.69, 511.90, -82 },
		{ -56.92, 777.90, 216 }, { 209.40, 1149.54, 105 },
	};

	private static readonly double[,] Devices =
	{
		{ -220.91, -911.20 }, { -277.01, -905.20 }, { -225, -961.24 }, { -283.68, -958.91 },
	};

	private static readonly double[,] LockedVillagers =
	{
		{ 153111, -548.69, -2071.07, 90 }, { 20063, -463, -2035, 134 }, { 20061, -457.36, -2134.69, 147 },
		{ 20064, -469.13, -2202.97, 158 }, { 153110, -517, -2008, 90 }, { 153109, -618.13, -2056.21, 31 },
	};

	private static readonly double[,] WaitingVillagers =
	{
		{ 153111, -504.50, -2037.18, 90 }, { 20063, -463, -2035, 134 }, { 20061, -452.81, -2078.81, 146 },
		{ 20064, -474.93, -2090.22, 157 }, { 153110, -517, -2008, 134 }, { 153109, -530.74, -2035.32, 31 },
	};

	private static readonly string[] VillagerNames = { "Rona", "Kornas", "Anne", "Zacaras", "Allonas", "Litas" };

	protected override void Load()
	{
		// Traveling Merchant Rose at the Special Reading Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY641_ROZE01", "d_abbey_64_1", -379, -1989, 258, IsRoseAtTheReadingRoom, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("So Goss knows how to open the door? Then we have to find him first."));
				await dialog.CompleteQuest(Mq010);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("There are voices coming from the other side of this door. I think the villagers are locked inside here."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("But the door isn't moving, even though there's no lock in sight. What do we do...?"),
					Option(L("I'll ask the villagers on the other side of the door"), "accept"),
					Option(L("Let's find another solution"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq010);

				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				var answer = await dialog.SelectQuestOffer(Mq020, L("The Ankel Small Corridor... It's to the left. The first thing to do is rescue Goss, like the villagers said."),
					Option(L("We should hurry"), "accept"),
					Option(L("Look for another way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq020);
					character.LookAround();

					await dialog.Msg(L("I'll head there first. Meet me at the Ankel Small Corridor."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq010))
			{
				await dialog.Msg(L("If only we can get this door to open..."));
				return;
			}

			if (character.Quests.HasCompleted(Mq020))
			{
				await dialog.Msg(L("I think Goss knows how to open the door. We should be able to rescue the villagers now."));
				return;
			}

			await dialog.Msg(L("When the guards aren't looking, we need to go and free the villagers as fast as we can."));
		});

		// The warehouse gate of the Special Reading Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(153117, "UnvisibleName", "ABBEY641_GATE", "d_abbey_64_1", -431, -1992, 129, c => !c.Quests.Has(Mq050), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq010, "askVillagers"))
				return;

			var asked = await character.TimeActions.StartAsync(L("Asking the villagers locked in the storage room..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
			if (asked != TimeActionResult.Completed)
				return;

			dialog.SetTitle(L("Croa Village Resident"));
			await dialog.Msg(L("Who are you?! Are you here to save us?"));
			await dialog.Msg(L("The way to open the door? You need to get to Goss, quick!"));
			await dialog.Msg(L("The demons took Goss to the Ankel Small Corridor! He knows how to open the door!"));

			character.Quests.CompleteObjective(Mq010, "askVillagers");
		});

		for (var i = 0; i < LockedVillagers.GetLength(0); ++i)
			AddConditionalNpc((int)LockedVillagers[i, 0], L(VillagerNames[i]), "ABBEY641_TOWN_PEAPLE01_" + (i + 1), "d_abbey_64_1", LockedVillagers[i, 1], LockedVillagers[i, 2], LockedVillagers[i, 3], c => c.Quests.HasCompleted(Bracken633Mq050) && !c.Quests.Has(Mq040));

		for (var i = 0; i < WaitingVillagers.GetLength(0); ++i)
			AddConditionalNpc((int)WaitingVillagers[i, 0], L(VillagerNames[i]), "ABBEY641_TOWN_PEAPLE02_" + (i + 1), "d_abbey_64_1", WaitingVillagers[i, 1], WaitingVillagers[i, 2], WaitingVillagers[i, 3], c => c.Quests.Has(Mq040) && !c.Quests.Has(Mq050));

		// Monk Goss bound in the Ankel Small Corridor
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY641_ROZE02", "d_abbey_64_1", -160.90, -1115.83, 164, c => c.Quests.IsActive(Mq020) && !c.Quests.IsCompletable(Mq020), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (!character.Quests.IsActive(Mq020) || character.Quests.IsCompletable(Mq020))
				return;

			await dialog.Msg(L("The first thing to do is rescue Goss, like the villagers said."));
			character.Quests.ClearQuestTrack(Mq020);
			character.Quests.StartQuestTrack(Mq020);
		});

		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY641_ROZE04", "d_abbey_64_1", -246.23, -1009.49, 192, c => c.Quests.IsCompletable(Mq020), async dialog =>
		{
			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			await dialog.Msg(L("Fortunately it seems like Goss is fine. I hope he knows a way to open the door..."));
		});

		AddConditionalNpc(155046, L("Monk Goss"), "ABBEY641_MONK01", "d_abbey_64_1", -258.77, -929.58, -10, c => c.Quests.HasCompleted(Mq010) && !c.Quests.HasCompleted(Mq020), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Monk Goss"));

			if (character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("Thank you for saving me. Seeing you and Rose makes me feel like the missing goddesses are back among us."));
				await dialog.Msg(L("A way to open the door... I will have to take a look at it myself. Meet me in front of the door."));
				await dialog.CompleteQuest(Mq020);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("Just when I thought we were over... you came and saved us. It seems the goddesses haven't abandoned me yet..."));
		});

		for (var i = 0; i < Devices.GetLength(0); ++i)
			AddConditionalNpc(47106, "UnvisibleName", "ABBEY641_DEVICE_" + (i + 1), "d_abbey_64_1", Devices[i, 0], Devices[i, 1], 90, AreGossRestraintsIntact);

		AddConditionalNpc(151006, "UnvisibleName", "ABBEY641_PURIFIER", "d_abbey_64_1", -258, -972, 7, AreGossRestraintsIntact);

		// Monk Goss at the door
		//-------------------------------------------------------------------------
		AddConditionalNpc(155046, L("Monk Goss"), "ABBEY641_MONK02", "d_abbey_64_1", -410, -1947, -23, c => c.Quests.HasCompleted(Mq020) && !c.Quests.Has(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Monk Goss"));

			if (character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("I see you gathered plenty of essences, I can feel it all the way here. Good... I'm ready now, too."));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("Hm... I couldn't see it from inside the room, but it's clear looking at it from the outside."));

				var answer = await dialog.SelectQuestOffer(Mq030, L("The door has been tampered with."),
					Option(L("Is there a way to open the door?"), "accept"),
					Option(L("Can you tell me about the attack at the Novaha Monastery?"), "explain"),
					Option(L("We need to find another way"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("A few days ago... The monastery was ambushed by demons and, unable to resist, I ended up in their hands."));
					await dialog.Msg(L("After Medzio Diena, maintaining the monastery has been no easy task. Apart from me and few other brothers, there was no one left in the monastery, you see."));
					await dialog.Msg(L("The demons kept every single one of us locked inside the annex. We didn't know the demons' demands or their reasoning; all we could do was pray."));
					await dialog.Msg(L("A few brothers were dragged outside one by one... they never came back. When only three of us were left, the demons came for me and locked me in this Reading Room."));
					await dialog.Msg(L("Then... they brought in people of the Croa Village. Just like before, they started to come for and take them one by one."));
					await dialog.Msg(L("Just as my turn was coming... that's when you saved me."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);

					await dialog.Msg(L("This faint magic circle... It's made to react only to demons. It looks like you would have to prove that you're a demon in order to open the door."));
					await dialog.Msg(L("Some Green Apparition Essences should suffice. Meanwhile I'll have a look at the circle."));
				}
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				await dialog.Msg(L("I think the door should be easy to open. It could take quite some time, however..."));
				await dialog.Msg(L("Not to mention all the demons walking around since you went and collected the essence."));

				var answer = await dialog.SelectQuestOffer(Mq040, L("They'll surely notice what I'm doing. Just protect me while I try and open the door."),
					Option(L("Alright"), "accept"),
					Option(L("Please give me some time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("I feel worthless for failing to protect the people of my village. I can only beg for the goddesses to forgive me..."));
				return;
			}

			await dialog.Msg(L("The demons saw us. We should get out of here with the villagers now."));
		});

		AddConditionalNpc(155046, L("Monk Goss"), "ABBEY641_MONK03", "d_abbey_64_1", -421.99, -1981.23, -31, c => c.Quests.Has(Mq040) && !c.Quests.Has(Mq050), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Monk Goss"));

			if (character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("I got it! This is... phew... a huge relief."));
				await dialog.Msg(L("I noticed Rose was looking for her brother... Unfortunately I haven't seen him. I wonder if he wasn't taken to the Annex, where I was..."));
				await dialog.Msg(L("I wish I could go and help you... I'm sorry. Injured as I am I would be a burden to you, and there's people in need of care here."));
				await dialog.Msg(L("I'll pray to the goddesses that Rose may find her brother. And may you receive their blessing..."));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("Just wait for a while... I'm going to focus."));
				character.Quests.ReplayQuestTrack(Mq040);
				return;
			}

			await dialog.Msg(L("If we can only open the door we can rescue the villagers... But the demons will notice us."));
		});

		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY641_ROZE05", "d_abbey_64_1", -395.47, -1984.68, -55, c => c.Quests.Has(Mq040) && !c.Quests.Has(Mq050), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				await dialog.Msg(L("You're saying my brother could be at the Novaha Annex? Why would they take him alone...?"));
				await dialog.Msg(L("We need to make sure the villagers escape first. My brother will be fine... I'm sure..."));

				var answer = await dialog.SelectQuestOffer(Mq050, L("It looks like the demon barrier is a little loose now. It feels odd but we can't waste our chance. We should move quickly."),
					Option(L("I'll help you"), "accept"),
					Option(L("It's still dangerous because of the demons' barrier"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("I don't know but it looks complicated. Still, I think Goss will probably know a way."));
		});

		// Traveling Merchant Rose at the monastery entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY641_ROZE03", "d_abbey_64_1", 661.53, 909.50, -40, c => c.Quests.Has(Mq050) && !c.Quests.HasCompleted(Mq050), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("Are you okay? I thought the barrier had loosened up... We need to be careful."));
				await dialog.Msg(L("Everyone has escaped from the Reading Room now. We should move to the Annex... and save my brother."));
				await dialog.CompleteQuest(Mq050);

				if (character.Quests.HasCompleted(Mq050))
					character.LookAround();
				return;
			}

			await dialog.Msg(L("It looks like the demon barrier is a little loose now. It feels odd but we can't waste our chance. We should move quickly."));
			character.Quests.ReplayQuestTrack(Mq050);
		});

		// Experiment Victim Tilis
		//-------------------------------------------------------------------------
		AddConditionalNpc(20063, L("Experiment Victim Tilis"), "ABBEY641_PEAPLE01", "d_abbey_64_1", 939.29, -1140.62, 124, c => !c.Quests.HasCompleted(Sq020), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Experiment Victim Tilis"));

			if (character.Quests.IsCompletable(Sq010))
			{
				await dialog.Msg(L("I feel a lot better now. I swear, my vision was starting to get blurry, I really thought I was over."));
				await dialog.CompleteQuest(Sq010);
				return;
			}

			if (character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("Thank you. Really, thank you so much! The others should be fine too... right?"));
				await dialog.Msg(L("I should escape from this place now. May the goddesses bless you!"));
				await dialog.CompleteQuest(Sq020);

				if (character.Quests.HasCompleted(Sq020))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				await dialog.Msg(L("Help... The demons... they experimented on me and left me here abandoned..."));
				await dialog.Msg(L("I can't escape, I'm too exhausted to even move."));

				var answer = await dialog.SelectQuestOffer(Sq010, L("There's nothing I can do but hide and wait for the day I return to the goddess..."),
					Option(L("Do you have a Stamina recovery potion?"), "accept"),
					Option(L("It's best to get out of the monastery and get treatment"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq010);
					character.Quests.CompleteObjective(Sq010, "givePotion");
				}
				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				await dialog.Msg(L("Just thinking about that experiment... It makes me want to leave this place immediately."));

				var answer = await dialog.SelectQuestOffer(Sq020, L("I won't ask you to come all the way with me! All I ask is that you clear out some demons so I can escape, will you do that?"),
					Option(L("I will come back soon"), "accept"),
					Option(L("I can't help with that"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq020);

				return;
			}

			if (character.Quests.IsActive(Sq020))
			{
				await dialog.Msg(L("They put me inside magic circles, they fed me things... It was horrible."));
				return;
			}

			if (character.Quests.HasCompleted(Sq010))
			{
				await dialog.Msg(L("I... I don't want them to come for me anymore... I'm so afraid... Those things..."));
				return;
			}

			await dialog.Msg(L("Help me... I have no strength left in my body..."));
		});

		// Experiment Victim Fils
		//-------------------------------------------------------------------------
		AddConditionalNpc(20064, L("Experiment Victim Fils"), "ABBEY641_PEAPLE02", "d_abbey_64_1", -845.14, 996.21, 90, c => !c.Quests.HasCompleted(Sq030), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Experiment Victim Fils"));

			if (character.Quests.IsCompletable(Sq030))
			{
				await dialog.Msg(L("Thank you for bringing these to me. As soon as I escape this monastery I'm going to make a tomb with these objects..."));
				await dialog.CompleteQuest(Sq030);

				if (character.Quests.HasCompleted(Sq030))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq030) && character.Quests.MeetsPrerequisites(Sq030))
			{
				await dialog.Msg(L("You're... not from the Croa Village, are you? And you don't look like you're a Novaha monk, either."));
				await dialog.Msg(L("I want to ask you something... All my friends, my family... my neighbors... They all returned to the goddesses because of the experiments."));
				await dialog.Msg(L("I do want to avenge them, but most of all I want to mourn those who are gone."));

				var answer = await dialog.SelectQuestOffer(Sq030, L("Would you bring me some of their belongings?"),
					Option(L("I'll bring you those objects if I can find them"), "accept"),
					Option(L("First we need to get water"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Relics.GetLength(0); ++i)
						character.Variables.Perm.Set(RelicVar + i, false);

					character.Quests.Start(Sq030);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq030))
			{
				await dialog.Msg(L("I may have survived, but what is it worth... I have no one now."));
				return;
			}

			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("How can someone call themselves human and do something so... atrocious? Not even demons are this evil..."));
			else
				await dialog.Msg(L("Run away while you can... They're doing horrible experiments here..."));
		});

		// The experiment victims' relics at the Raundona Circle Hall
		//-------------------------------------------------------------------------
		for (var i = 0; i < Relics.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47160, "UnvisibleName", "ABBEY641_BOX01_" + number, "d_abbey_64_1", Relics[i, 0], Relics[i, 1], Relics[i, 2],
				character => character.Quests.IsActive(Sq030) && !character.Quests.IsCompletable(Sq030) && !character.Variables.Perm.GetBool(RelicVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq030) || character.Quests.IsCompletable(Sq030) || character.Variables.Perm.GetBool(RelicVar + number, false))
						return;

					character.Variables.Perm.Set(RelicVar + number, true);
					character.Inventory.Add(ItemId.ABBAY641_SQ030_ITEM01, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The memorial to the victims of Novaha
		//-------------------------------------------------------------------------
		for (var i = 0; i < Stones.GetLength(0); ++i)
		{
			AddConditionalNpc(153177, "UnvisibleName", "ABBEY64_2_HIDDENQ2_STONE" + (i + 1), "d_abbey_64_1", Stones[i, 0], Stones[i, 1], Stones[i, 2],
				character => character.Quests.IsActive(Hq1) && !character.Quests.IsCompletable(Hq1),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Hq1) || character.Quests.IsCompletable(Hq1))
						return;

					character.Inventory.Add(ItemId.ABBAY642_HIDDENQ1_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		AddQuestTrigger("ABBEY64_2_HIDDENQ2_OBJ2", "d_abbey_64_1", 436.31, 851.78, 60, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsCompletable(Hq1))
				return;

			var erected = await character.TimeActions.StartAsync(L("Erecting the tombstone..."), L("Cancel"), "SIT_HAMMERING", TimeSpan.FromSeconds(3));
			if (erected != TimeActionResult.Completed)
				return;

			character.Quests.Complete(Hq1);
			if (!character.Quests.HasCompleted(Hq1))
				return;

			character.ServerMessage(L("The tombstone is up. Now to write a few words of praise to the victims."));
			character.Quests.Start(Hq2);
			character.LookAround();
		});

		AddConditionalNpc(151105, L("Memorial"), "ABBEY64_2_HIDDENQ2_OBJ1", "d_abbey_64_1", 419.10, 855.27, 64, c => c.Quests.HasCompleted(Hq1), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.IsActive(Hq2, "carveMessage"))
			{
				var carved = await character.TimeActions.StartAsync(L("Carving a message for the victims on the tombstone..."), L("Cancel"), "WRITE", TimeSpan.FromSeconds(3));
				if (carved != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Hq2, "carveMessage");
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The memorial is complete.{nl}Return to Rona and deliver the news."), 7);
				return;
			}

			dialog.SetTitle(L("Monument"));
			await dialog.Msg(L("In honor of those sacrificed for the demons' monstrous experiments..."));
		});

		// Invisible walls
		//-------------------------------------------------------------------------
		AddConditionalNpc(MonsterId.HiddenWall_40_50_100, "", "ABBEY641_MQ_HIDDENWAL", "d_abbey_64_1", -251.61, -933.42, 90, c => !IsPastProgress(c, Mq020));
		AddConditionalNpc(MonsterId.HiddenWall_10_170_300, "", "ABBEY641_HIDDENWALL", "d_abbey_64_1", -428.67, -2000.60, 44, c => !IsPastProgress(c, Mq050));
	}

	/// <summary>
	/// Returns whether the quest's cutscene is playing or already behind the character.
	/// </summary>
	private static bool IsPastProgress(Character character, QuestId questId)
		=> character.Quests.HasCompleted(questId) || character.Quests.IsCompletable(questId) || character.Tracks.ActiveTrack?.Data.QuestId == questId.Value;

	/// <summary>
	/// Returns whether Rose waits in front of the Special Reading Room.
	/// </summary>
	private static bool IsRoseAtTheReadingRoom(Character character)
		=> character.Quests.HasCompleted(Bracken633Mq050) && (!character.Quests.Has(Mq020) || character.Quests.HasCompleted(Mq020)) && !character.Quests.Has(Mq040);

	/// <summary>
	/// Returns whether the devices binding Monk Goss are still standing.
	/// </summary>
	private static bool AreGossRestraintsIntact(Character character)
		=> character.Quests.HasCompleted(Mq010) && !character.Quests.IsCompletable(Mq020) && !character.Quests.HasCompleted(Mq020);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50117: The Rescue (1)
//-----------------------------------------------------------------------------
public class Abbay641Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50117);
		SetName(L("The Rescue (1)"));
		SetDescription(L("The residents of the Croa Village seem to be locked inside the Special Reading Room. Ask the residents if they know a solution."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_ROZE01", "d_abbey_64_1", L("Follow Rose into the Novaha Monastery"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_GATE", "d_abbey_64_1", L("Talk to the people of the Croa Village"));
		SetPhase(QuestStatus.Success, "ABBEY641_ROZE01", "d_abbey_64_1", L("Talk to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50112, QuestStatus.Completed));

		AddObjective("askVillagers", L("Talk to the people of the Croa Village"), new ManualObjective());
	}
}

// 50118: The Rescue (2)
//-----------------------------------------------------------------------------
public class Abbay641Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50118);
		SetName(L("The Rescue (2)"));
		SetDescription(L("Traveling Merchant Rose thinks it's best to rescue Monk Goss as the residents suggested. Head to the Ankel Small Corridor and rescue Monk Goss!"));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_ROZE01", "d_abbey_64_1", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_ROZE02", "d_abbey_64_1", L("Rescue Monk Goss at the Ankel Small Corridor"));
		SetPhase(QuestStatus.Success, "ABBEY641_MONK01", "d_abbey_64_1", L("Talk to Monk Goss"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ABBAY_64_1_MQ020_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50117, QuestStatus.Completed));

		AddObjective("destroyDevices", L("Destroy the devices attached to the Monk"), new KillObjective(4, "npc_rokas_6") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 4));
		AddReward(new ItemReward("Drug_SP1_Q", 45));
	}
}

// 50119: The Rescue (3)
//-----------------------------------------------------------------------------
public class Abbay641Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50119);
		SetName(L("The Rescue (3)"));
		SetDescription(L("Monk Goss says the demons have created a magic circle which can only be opened by proving that you are a demon. This should be possible with some Green Apparition Essence."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_MONK02", "d_abbey_64_1", L("Talk to Monk Goss"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_MONK02", "d_abbey_64_1", L("Obtain Green Apparition Essence"));
		SetPhase(QuestStatus.Success, "ABBEY641_MONK02", "d_abbey_64_1", L("Deliver to Monk Goss"));

		AddPrerequisite(new QuestStatusPrerequisite(50118, QuestStatus.Completed));

		AddObjective("collectEssence", L("Collect Green Apparition Essence to prove that you are a demon"), new CollectItemObjective("ABBAY641_MQ3_ITEM01", 10));
		AddPityDrop("ABBAY641_MQ3_ITEM01", 1.0f, 0, 1, "Sec_Spector_Gh");

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new TakeItemReward("ABBAY641_MQ3_ITEM01", -1));
	}
}

// 50120: The Rescue (4)
//-----------------------------------------------------------------------------
public class Abbay641Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50120);
		SetName(L("The Rescue (4)"));
		SetDescription(L("Monk Goss says opening the door is easy, although it does take some time. Protect Monk Goss while he opens the door to the Special Reading Room."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_MONK02", "d_abbey_64_1", L("Talk to Monk Goss"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_MONK03", "d_abbey_64_1", L("Protect Monk Goss until the door to the Special Reading Room is open"));
		SetPhase(QuestStatus.Success, "ABBEY641_MONK03", "d_abbey_64_1", L("Talk to Monk Goss"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ABBAY_64_1_MQ040_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50119, QuestStatus.Completed));

		AddObjective("protectGoss", L("Protect Monk Goss until the door to the Special Reading Room is open"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 4));
	}
}

// 50121: The Rescue (5)
//-----------------------------------------------------------------------------
public class Abbay641Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50121);
		SetName(L("The Rescue (5)"));
		SetDescription(L("Traveling Merchant Rose thinks it's best for the village residents to run away immediately. Help the people escape to safety."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_ROZE05", "d_abbey_64_1", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_ROZE03", "d_abbey_64_1", L("Help the Villagers escape to safety"));
		SetPhase(QuestStatus.Success, "ABBEY641_ROZE03", "d_abbey_64_1", L("Talk to Rose at the Novaha Monastery entrance"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ABBAY_64_1_MQ050_TRACK", "m_boss_c", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50120, QuestStatus.Completed));

		AddObjective("killMummyghast", L("Defeat the attacking Mummyghast"), new KillObjective(1, "boss_Mummyghast_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 5));
		AddReward(new SelectItemReward("SWD02_124", "TSW02_120", "STF02_119", "TSF02_119", "TBW02_122", "BOW02_118", "MAC02_121", "SPR02_116"));
	}
}

// 50122: An Exhausted Body (1)
//-----------------------------------------------------------------------------
public class Abbay641Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50122);
		SetName(L("An Exhausted Body (1)"));
		SetDescription(L("Tilis was a victim of terrible experiments and their body is in poor condition. Give a stamina recovery potion to Experiment Victim Tilis."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_PEAPLE01", "d_abbey_64_1", L("Talk to Experiment Victim Tilis"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_PEAPLE01", "d_abbey_64_1", L("Give a Stamina Recovery Potion"));
		SetPhase(QuestStatus.Success, "ABBEY641_PEAPLE01", "d_abbey_64_1", L("Talk to Experiment Victim Tilis"));

		AddPrerequisite(new LevelPrerequisite(35));

		AddObjective("givePotion", L("Give a Stamina Recovery Potion"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50123: An Exhausted Body (2)
//-----------------------------------------------------------------------------
public class Abbay641Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50123);
		SetName(L("An Exhausted Body (2)"));
		SetDescription(L("Experiment Victim Tilis wants you to help them escape the monastery. Defeat any demons nearby."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_PEAPLE01", "d_abbey_64_1", L("Talk to Experiment Victim Tilis"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_PEAPLE01", "d_abbey_64_1", L("Defeat nearby demons"));
		SetPhase(QuestStatus.Success, "ABBEY641_PEAPLE01", "d_abbey_64_1", L("Report to Experiment Victim Tilis"));

		AddPrerequisite(new QuestStatusPrerequisite(50122, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the demons nearby"), new KillObjective(10, "Sec_Spector_Gh", "velwriggler_mage_red"));

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50124: Keepsake
//-----------------------------------------------------------------------------
public class Abbay641Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50124);
		SetName(L("Keepsake"));
		SetDescription(L("Experiment Victim Fils wants you to find the relics that belonged to the experiment victims who died. Go to the Raundona Circle Hall and collect the people's relics."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY641_PEAPLE02", "d_abbey_64_1", L("Talk to Experiment Victim Fils"));
		SetPhase(QuestStatus.InProgress, "ABBEY641_PEAPLE02", "d_abbey_64_1", L("Collect Experiment Victims' Relic"));
		SetPhase(QuestStatus.Success, "ABBEY641_PEAPLE02", "d_abbey_64_1", L("Deliver to Experiment Victim Fils"));

		AddPrerequisite(new LevelPrerequisite(35));

		AddObjective("collectRelics", L("Collect Experiment Victims' Relic"), new CollectItemObjective("ABBAY641_SQ030_ITEM01", 6));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Drug_SP1_Q", 45));
		AddReward(new TakeItemReward("ABBAY641_SQ030_ITEM01", -1));
	}
}

// 50277: Remembering the Victims (2)
//-----------------------------------------------------------------------------
public class Abbey642Hq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50277);
		SetName(L("Remembering the Victims (2)"));
		SetDescription(L("You have erected the tombstone. Carve a message to honor the victims on it."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_1", "f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY64_2_HIDDENQ2_OBJ1", "d_abbey_64_1", L("Carve a Message to Honor the Victims"));
		SetPhase(QuestStatus.InProgress, "ABBEY64_2_HIDDENQ2_OBJ1", "d_abbey_64_1", L("Carve a Message to Honor the Victims"));
		SetPhase(QuestStatus.Success, "BRACKEN632_TOWN_PEAPLE2", "f_bracken_63_2", L("Talk to Rona"));

		AddPrerequisite(new QuestStatusPrerequisite(50276, QuestStatus.Completed));

		AddObjective("carveMessage", L("Carve a Message to Honor the Victims"), new ManualObjective());
	}
}
