//--- Melia Script ----------------------------------------------------------
// Kalejimas Storage Quest NPCs
//--- Description -----------------------------------------------------------
// A second piece of Zanas' soul, the three lamps of the Storage and the
// King's Red Jewel, and the second demon barrier.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison79QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Prison78Mq9 = new QuestId(30153);
	private readonly static QuestId Mq1 = new QuestId(30154);
	private readonly static QuestId Mq2 = new QuestId(30155);
	private readonly static QuestId Mq3 = new QuestId(30156);
	private readonly static QuestId Mq4 = new QuestId(30157);
	private readonly static QuestId Mq5 = new QuestId(30158);
	private readonly static QuestId Mq6 = new QuestId(30159);
	private readonly static QuestId Mq7 = new QuestId(30160);
	private readonly static QuestId Mq8 = new QuestId(30161);
	private readonly static QuestId Mq9 = new QuestId(30162);
	private readonly static QuestId Mq10 = new QuestId(30163);
	private readonly static QuestId Sq1 = new QuestId(30197);

	private const string ZanasPortrait = "Dlg_port_zanas_prison";
	private const string OilPouchVar = "Gabija.Prison79.OilPouch";
	private const string CircleMaskVar = "Gabija.Prison79.CircleMask";
	private const string SideVar = "Gabija.Prison79.DeviceSide.";
	private const string PileVar = "Gabija.Prison79.Pile";
	private const int CoresNeeded = 10;
	private const int AllCircles = 0b11111;

	// The pile the manual floats out of.
	private const int ManualPile = 5;

	private readonly static double[,] PileSpots =
	{
		{ -1604.68, -1537.46 }, { -2104.27, -1527.29 }, { -2121.36, -2207.64 },
		{ -1989.28, -1812.89 }, { -2132.71, -2089.90 },
	};

	private readonly static int[] PileModels = { 151029, 151030, 155008, 46212, 151029 };
	private readonly static double[] PileFacings = { 90, 90, 90, 180, 90 };

	private readonly static double[,] OilPouchSpots =
	{
		{ -1064.25, -965.27 }, { -828.09, -843.28 }, { -404.10, -976.99 }, { -847.88, -1565.57 },
		{ -882.50, -1876.58 }, { -1020.48, -1167.37 }, { -67.16, -1103.75 },
	};

	// The magic circles around the Yellow Lamp, in ring order.
	private readonly static double[,] CircleSpots =
	{
		{ 1129.70, 163.47 }, { 1260.17, 185.33 }, { 1308.46, 72.11 },
		{ 1217.35, -17.61 }, { 1098.65, 33.11 },
	};

	protected override void Load()
	{
		// Zanas' Soul, at the Central Assembly Area
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_79_NPC_1", "d_prison_79", 322.44, 185.05, 0, this.IsZanasAtTheAssemblyArea, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq1) && character.Quests.IsCompletable(Mq1))
			{
				await dialog.Msg(L("Hmm, seems sufficient."));
				await dialog.Msg(L("I hope, it will work out as planned..."));
				await dialog.CompleteQuest(Mq1);
				return;
			}

			if (character.Quests.IsActive(Mq3) && character.Quests.IsCompletable(Mq3))
			{
				await dialog.Msg(L("Found the manual and some memories have been regained. Now things will get a bit easier."));
				await dialog.Msg(L("Now, let's get those jewels."));
				await dialog.CompleteQuest(Mq3);
				return;
			}

			if (!character.Quests.Has(Mq1) && character.Quests.MeetsPrerequisites(Mq1))
			{
				await dialog.Msg(L("Like I have said before..."));
				await dialog.Msg(L("When my soul split, so did my memories. At my current state, helping you is a bit difficult."));
				await dialog.Msg(L("We need a part of my soul in Storage facility in Warehouse No. 3."));
				await dialog.Msg(L("To find the secret device where my soul resides in, you need two types of spell."));
				await dialog.Msg(L("When King Kadumel was making it, there were two types of magic stones..."));
				await dialog.Msg(L("Unfortunately, we do not have them at the moment."));
				await dialog.Msg(L("One of them can be solved by your power alone, Revelator.."));
				await dialog.Msg(L("As for the other one, we should use the power from the monsters."));

				var answer = await dialog.SelectQuestOffer(Mq1, L("They are two completely different powers, it might just solve the secret device."),
					Option(L("Say that you think it's a good idea"), "accept"),
					Option(L("But that's absurd!"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1);
					await dialog.Msg(L("There is a evil energy core you can get from defeating nearby monsters."));
					await dialog.Msg(L("It emits the power exact opposite of your own."));
				}
				return;
			}

			if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
			{
				await dialog.Msg(L("My other soul is hiding in the secret device in Warehouse No. 3."));
				await dialog.Msg(L("It cannot get out of it on its own, please turn it off and let it out."));

				var answer = await dialog.SelectQuestOffer(Mq2, L("To turn the secret device off, you have to give two different powers into the machine on each side. One from your own. The other from the core of the evil energy."),
					Option(L("I'll go there"), "accept"),
					Option(L("Say that it is nonsense"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2);
					await dialog.Msg(L("It bears repeating..."));
					await dialog.Msg(L("Put the power of your own and the power from the monster into each side of the secret device."));
				}
				return;
			}

			if (!character.Quests.Has(Mq4) && character.Quests.MeetsPrerequisites(Mq4))
			{
				await dialog.Msg(L("The King has four jewels."));
				await dialog.Msg(L("The red one is inside the secret device in Warehouse No. 1."));
				await dialog.Msg(L("In order to open it, you must light three lamps."));

				var answer = await dialog.SelectQuestOffer(Mq4, L("Follow the instructions on the manual to light the lamp."),
					Option(L("Say that you will light the Lamp"), "accept"),
					Option(L("Say that you have no idea what the instructions mean"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq4);
					character.Quests.CompleteObjective(Mq4, "findTheLamp");
					await dialog.Msg(L("Two of the lamps require specific oil for each lamp..."));
					await dialog.Msg(L("The rest can be lit with magic."));
					await dialog.Msg(L("It ain't exactly Homunculus summoning, just avoid getting devoured by demons and you are good to go."));
				}
				return;
			}

			if (!character.Quests.Has(Mq7) && character.Quests.MeetsPrerequisites(Mq7))
			{
				await dialog.Msg(L("You lit two of them already?"));
				await dialog.Msg(L("You truly are the revelator picked by Goddess Laima."));
				await dialog.Msg(L("Now, the last lamp is the trickiest lamp of them all."));
				await dialog.Msg(L("But for you, it won't be that hard."));

				var answer = await dialog.SelectQuestOffer(Mq7, L("The last lamp is located at the Kalejimas Visiting Room."),
					Option(L("Say that you will light the last Lamp"), "accept"),
					Option(L("Say that it is too complicated"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq7);
					await dialog.Msg(L("The last lamp will be lit if you activate all the five magic circles surrounding it."));
					await dialog.Msg(L("One activated magic circle will affect the others.."));
					await dialog.Msg(L("So, you have to think ahead on which one you should activate first."));
				}
				return;
			}

			if (!character.Quests.Has(Mq8) && character.Quests.MeetsPrerequisites(Mq8))
			{
				await dialog.Msg(L("The lamps are all lit. All that is left is King's Red Jewel."));

				var answer = await dialog.SelectQuestOffer(Mq8, L("The secret device in Warehouse No. 1 has the King's Red Jewel."),
					Option(L("Say that you will retrieve the King's Red Jewel"), "accept"),
					Option(L("Ask for a moment before continuing"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq8);
					await dialog.Msg(L("Be safe."));
					await dialog.Msg(L("This place is swarming with those demons."));
				}
				return;
			}

			if (!character.Quests.Has(Mq9) && character.Quests.MeetsPrerequisites(Mq9))
			{
				await dialog.Msg(L("Now, we are one step closer."));
				await dialog.Msg(L("3 more King's gems to go."));
				await dialog.Msg(L("Now, let's disable the demon barrier in the storage."));
				await dialog.Msg(L("I would have to pay a part of my soul as a price but..."));
				await dialog.Msg(L("I had one piece just now so, I can endure."));
				await dialog.Msg(L("Please, destroy the monsters on your way to the demon barrier."));

				var answer = await dialog.SelectQuestOffer(Mq9, L("All our struggles would be in vain if we get captured by the demons."),
					Option(L("I'll defeat the monsters"), "accept"),
					Option(L("Say that you don't think there will be much of a problem"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq9);
					await dialog.Msg(L("Get to the barrier and use Teal Magic Stone to call me and I will disable the demon barrier."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq1))
			{
				await dialog.Msg(L("I think the demons have not noticed our movement."));
				await dialog.Msg(L("This is our chance!"));
				return;
			}

			if (character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("I hope my other soul piece has the memory regarding the revelation..."));
				return;
			}

			if (character.Quests.IsActive(Mq3))
			{
				await dialog.Msg(L("The manual is hidden inside a pile of junk in Warehouse No. 2."));
				await dialog.Msg(L("Use the Teal Magic Stone on the pile of junk, there will be a piece of paper levitating."));
				return;
			}

			if (character.Quests.IsActive(Mq4) || character.Quests.IsActive(Mq5) || character.Quests.IsActive(Mq6))
			{
				await dialog.Msg(L("Two of the lamps require specific oil for each lamp..."));
				await dialog.Msg(L("The rest can be lit with magic."));
				return;
			}

			if (character.Quests.IsActive(Mq7))
			{
				await dialog.Msg(L("All these complex machineries..."));
				await dialog.Msg(L("You cannot deny that King Kadumel was great in that respect."));
				await dialog.Msg(L("Then again, Nebulas has yet to find the revelation thanks to him..."));
				return;
			}

			if (character.Quests.IsActive(Mq8))
			{
				await dialog.Msg(L("I feel guilty not doing anything while you are doing all the hard work."));
				await dialog.Msg(L("I mean, I feel useless because of all those demon barriers."));
				return;
			}

			if (character.Quests.IsActive(Mq9))
			{
				await dialog.Msg(L("Failing my duty is the only thing I truly fear."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("We'll need memories if we're going to get the Revelatin back."));
			await dialog.Msg(L("I should round up my spirits."));
		});

		// The Warehouse No. 3 Secret Device
		//-------------------------------------------------------------------------
		AddConditionalNpc(151109, L("Secret Device"), "PRISON_79_OBJ_1", "d_prison_79", 2173.10, -1847.55, 45, this.IsTheSoulStillHidden, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (!character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("A secret device with a slot on either side, humming with something trapped inside."));
				return;
			}

			var side = await dialog.Select(L("A secret device with a slot on either side."),
				Option(L("Infuse your own power"), "power"),
				Option(L("Infuse the evil magic"), "evil"),
				Option(L("Leave it"), "leave")
			);

			if (side == "leave")
				return;

			var sideVar = SideVar + side;

			if (character.Variables.Temp.GetBool(sideVar, false))
			{
				character.ServerMessage(L("You have already infused this side. Try again on the other side."));
				return;
			}

			if (side == "evil" && character.Inventory.CountItem(ItemId.PRISON_79_MQ_1_ITEM) < CoresNeeded)
			{
				await dialog.Msg(L("You do not have enough Evil Energy Cores to infuse the evil magic."));
				return;
			}

			var infused = await character.TimeActions.StartAsync(side == "power" ? L("Infusing with powers") : L("Infusing with evil magic"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

			if (infused != TimeActionResult.Completed)
				return;

			if (side == "evil")
				character.Inventory.RemoveItem(ItemId.PRISON_79_MQ_1_ITEM, CoresNeeded);

			character.Variables.Temp.SetBool(sideVar, true);

			if (!character.Variables.Temp.GetBool(SideVar + "power", false) || !character.Variables.Temp.GetBool(SideVar + "evil", false))
			{
				character.ServerMessage(side == "power" ? L("You have infused your powers. Infuse evil magic from the other side.") : L("You have infused the evil magic. Infuse your powers from the other side."));
				return;
			}

			character.Variables.Temp.Remove(SideVar + "power");
			character.Variables.Temp.Remove(SideVar + "evil");
			character.Quests.CompleteObjective(Mq2, "freeTheSoul");
			character.LookAround();
			character.ServerMessage(L("Zanas' Soul has appeared. Talk to Zanas' Soul."));
		});

		// Zanas' Soul, freed from the device
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_79_NPC_2", "d_prison_79", 2173.66, -1848.23, 90, this.IsTheSoulFreed, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq2) && character.Quests.IsCompletable(Mq2))
			{
				await dialog.Msg(L("Is it you who got me out?"));
				await dialog.Msg(L("Finally, Revelator came."));
				await dialog.Msg(L("That means my other soul piece in the visiting room succeeded."));
				await dialog.Msg(L("Some of my soul pieces are captured but it's a relief that one of them at least succeeded."));
				await dialog.Msg(L("Revelator is here! Now, the next step can be done."));
				await dialog.CompleteQuest(Mq2);
				return;
			}

			if (!character.Quests.Has(Mq3) && character.Quests.MeetsPrerequisites(Mq3))
			{
				await dialog.Msg(L("You need four gems to open the secret device that contains the revelation."));
				await dialog.Msg(L("One of the gems is stored in this storage."));
				await dialog.Msg(L("I have no memory of where all the gems are nor the location of the revelation.."));
				await dialog.Msg(L("Perhaps, my other soul piece would have that information."));
				await dialog.Msg(L("But fortunately, there is an instruction manual for the secret device."));
				await dialog.Msg(L("If you find it, we don't have to find the memory to activate the secret device."));
				await dialog.Msg(L("The problem is special spell is needed to read the manual."));

				var answer = await dialog.SelectQuestOffer(Mq3, L("For that, we need Teal Magic Stone..."),
					Option(L("Show the Teal Magic Stone"), "accept"),
					Option(L("Argue that this is merely complicating things"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq3);
					await dialog.Msg(L("Say what? That's the Teal Magic Stone, right there."));
					await dialog.Msg(L("You came prepared, didn't you."));
					await dialog.Msg(L("That makes the whole thing, easy-peasy."));
					await dialog.Msg(L("The manual is hidden inside a pile of junk in Warehouse No. 2."));
					await dialog.Msg(L("Use the Teal Magic Stone on the pile of junk, there will be a piece of paper levitating."));
					await dialog.Msg(L("That's the manual."));
					await dialog.Msg(L("If you find the manual, please come straight to the Waiting Room."));
					await dialog.Msg(L("I will find my other soul pieces and regain more memories."));
					character.LookAround();
				}
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("I don't know what I'd do if the demons had found me first while I was hiding."));
			await dialog.Msg(L("Fortunately you're here now, I'm glad."));
		});

		// The piles of junk in Warehouse No. 2
		//-------------------------------------------------------------------------
		for (var i = 0; i < PileSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(PileModels[i], L("Pile of Junk"), "PRISON_79_OBJ_2_" + number, "d_prison_79", PileSpots[i, 0], PileSpots[i, 1], PileFacings[i], dialog => this.ShineOnThePile(dialog, number));
		}

		// The Blue Lamp
		//-------------------------------------------------------------------------
		AddNpc(147305, L("Blue Lamp"), "PRISON_79_OBJ_3", "d_prison_79", -653, -1244, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Blue Lamp"));

			if (character.Quests.IsActive(Mq4) && character.Quests.IsCompletable(Mq4))
			{
				await dialog.Msg(L("The first lamp the manual names. Two of the lamps need their own oil; the rest are lit with magic."));
				await dialog.CompleteQuest(Mq4);

				if (!character.Quests.Has(Mq5) && character.Quests.MeetsPrerequisites(Mq5))
					await this.StartTheBlueLamp(character);

				return;
			}

			if (character.Quests.IsActive(Mq5) && character.Quests.IsCompletable(Mq5))
			{
				var lit = await character.TimeActions.StartAsync(L("Lighting Blue Lamp"), L("Cancel"), "FIRE", TimeSpan.FromSeconds(2));

				if (lit != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The oil catches, and the Blue Lamp burns bright."));
				await dialog.CompleteQuest(Mq5);
				character.ServerMessage(L("You have lit the Blue Lamp. Look for the Red Lamp."));
				return;
			}

			if (!character.Quests.Has(Mq5) && character.Quests.MeetsPrerequisites(Mq5))
			{
				await this.StartTheBlueLamp(character);
				return;
			}

			if (character.Quests.IsActive(Mq5))
			{
				await dialog.Msg(L("The Blue Lamp doesn't have any Oil. Gather Oil for it from the nearby Oil Pouches."));
				return;
			}

			await dialog.Msg(character.Quests.HasCompleted(Mq5) ? L("The Blue Lamp burns steadily.") : L("A blue lamp, dark and dry."));
		});

		// Oil Pouches
		//-------------------------------------------------------------------------
		for (var i = 0; i < OilPouchSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(47160, L("Oil Pouch"), "PRISON_79_OBJ_4_" + number, "d_prison_79", OilPouchSpots[i, 0], OilPouchSpots[i, 1], 90, dialog => this.TakeTheOil(dialog, number));
		}

		// The Red Lamp
		//-------------------------------------------------------------------------
		AddNpc(147305, L("Red Lamp"), "PRISON_79_OBJ_5", "d_prison_79", 773, -1229, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Red Lamp"));

			if (character.Quests.IsActive(Mq6) && character.Quests.IsCompletable(Mq6))
			{
				var lit = await character.TimeActions.StartAsync(L("Lighting Red Lamp"), L("Cancel"), "FIRE", TimeSpan.FromSeconds(2));

				if (lit != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The oil catches, and the Red Lamp burns bright."));
				await dialog.CompleteQuest(Mq6);
				character.ServerMessage(L("You have lit the Red Lamp. Return to Zanas' Spirit."));
				return;
			}

			if (!character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6))
			{
				var looked = await character.TimeActions.StartAsync(L("Examining Red Lamp"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq6);
				character.ServerMessage(L("The Red Lamp doesn't have any Oil. Gather Oil for it by defeating nearby monsters."));
				return;
			}

			if (character.Quests.IsActive(Mq6))
			{
				await dialog.Msg(L("The Red Lamp has no oil. The monsters around here like the smell of it."));
				return;
			}

			await dialog.Msg(character.Quests.HasCompleted(Mq6) ? L("The Red Lamp burns steadily.") : L("A red lamp, dark and dry."));
		});

		// The Yellow Lamp
		//-------------------------------------------------------------------------
		AddNpc(147305, L("Yellow Lamp"), "PRISON_79_OBJ_6", "d_prison_79", 1201, 87, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Yellow Lamp"));

			if (character.Quests.IsActive(Mq7) && character.Quests.IsCompletable(Mq7))
			{
				var lit = await character.TimeActions.StartAsync(L("Lighting Yellow Lamp"), L("Cancel"), "FIRE", TimeSpan.FromSeconds(2));

				if (lit != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("With every circle feeding it, the Yellow Lamp lights."));
				await dialog.CompleteQuest(Mq7);
				character.Variables.Temp.Remove(CircleMaskVar);
				character.ServerMessage(L("You have lit the Yellow Lamp. Return to Zanas' Spirit."));
				return;
			}

			if (character.Quests.IsActive(Mq7))
			{
				if (character.Variables.Temp.Has(CircleMaskVar))
				{
					character.ServerMessage(L("The Magic Circle is already being supplied with magic. Control and activate all of the magic circles."));
					return;
				}

				var examined = await character.TimeActions.StartAsync(L("Examining the Yellow Lamp"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (examined != TimeActionResult.Completed)
					return;

				var mask = System.Random.Shared.Next(1, AllCircles);
				character.Variables.Temp.SetInt(CircleMaskVar, mask);

				character.ServerMessage(L("Magic is being supplied to the surrounding magic circle. Control and activate all of the magic circles."));
				character.ServerMessage(LF("Magic circles active: {0}/5", this.CountCircles(mask)));
				return;
			}

			await dialog.Msg(character.Quests.HasCompleted(Mq7) ? L("The Yellow Lamp burns steadily.") : L("A yellow lamp inside a ring of five magic circles."));
		});

		// The Yellow Lamp's magic circles
		//-------------------------------------------------------------------------
		for (var i = 0; i < CircleSpots.GetLength(0); ++i)
		{
			var index = i;

			AddNpc(153047, L("Magic Circle"), "PRISON_79_OBJ_7_" + (i + 1), "d_prison_79", CircleSpots[i, 0], CircleSpots[i, 1], 90, dialog => this.ActivateTheCircle(dialog, index));
		}

		// The Warehouse No. 1 Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_79_OBJ_8", "d_prison_79", -1764, 1951, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq8) && character.Quests.IsCompletable(Mq8))
			{
				await dialog.Msg(L("With the three lamps lit, the secret device opens. The King's Red Jewel sits inside."));
				await dialog.CompleteQuest(Mq8);
				return;
			}

			if (character.Quests.IsActive(Mq8))
			{
				await dialog.Msg(L("Monsters are gathering around the secret device. Defeat all of them."));
				character.Quests.ClearQuestTrack(Mq8);
				return;
			}

			await dialog.Msg(L("A secret device of King Kadumel's, sealed tight."));
		});

		// The Central Assembly Area's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_79_SQ_OBJ_1", "d_prison_79", -122.35, -3.84, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Sq1) && character.Quests.IsCompletable(Sq1))
			{
				await dialog.Msg(L("Tucked inside the open device are supplies someone stored away and never came back for."));
				await dialog.CompleteQuest(Sq1);
				return;
			}

			if (!character.Quests.Has(Sq1) && character.Quests.MeetsPrerequisites(Sq1))
			{
				var opened = await character.TimeActions.StartAsync(L("Disarming the secret device"), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(2));

				if (opened != TimeActionResult.Completed)
					return;

				character.Quests.Start(Sq1);
				character.Quests.CompleteObjective(Sq1, "openTheDevice");
				character.ServerMessage(L("You have succeeded in opening the Secret Device."));
				return;
			}

			await dialog.Msg(L("The device stands open and empty."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("PRISON_79_OBJ_8_AREA", "d_prison_79", -1764, 1951, 350, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq8) && !character.Quests.IsCompletable(Mq8))
				character.Quests.StartQuestTrack(Mq8);

			await Task.CompletedTask;
		});

		AddQuestTrigger("PRISON_79_MQ_10_TRIGGER", "d_prison_79", 2102, 2315, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq10) && character.Quests.MeetsPrerequisites(Mq10))
			{
				character.Quests.Start(Mq10);
				character.Quests.CompleteObjective(Mq10, "releaseTheBarrier");
			}
			else if (character.Quests.IsActive(Mq10) && character.Quests.IsCompletable(Mq10))
			{
				character.Quests.ReplayQuestTrack(Mq10);
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Looks the Blue Lamp over and starts its quest.
	/// </summary>
	/// <param name="character"></param>
	private async Task StartTheBlueLamp(Character character)
	{
		var looked = await character.TimeActions.StartAsync(L("Examining Blue Lamp"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

		if (looked != TimeActionResult.Completed)
			return;

		for (var i = 1; i <= OilPouchSpots.GetLength(0); ++i)
			character.Variables.Perm.Remove(OilPouchVar + i);

		character.Quests.Start(Mq5);
		character.ServerMessage(L("The Blue Lamp doesn't have any Oil. Gather Oil for it from the nearby Oil Pouches."));
	}

	/// <summary>
	/// Takes the Blue Lamp's oil out of one of the Oil Pouches.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="number"></param>
	private async Task TakeTheOil(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Oil Pouch"));

		if (!character.Quests.IsActive(Mq5) || character.Quests.IsCompletable(Mq5))
		{
			await dialog.Msg(L("A pouch of lamp oil, left where it was needed."));
			return;
		}

		if (character.Variables.Perm.GetBool(OilPouchVar + number, false))
		{
			await dialog.Msg(L("This pouch is empty now."));
			return;
		}

		var taken = await character.TimeActions.StartAsync(L("Retrieving oil"), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (taken != TimeActionResult.Completed)
			return;

		character.Variables.Perm.SetBool(OilPouchVar + number, true);
		character.Inventory.Add(ItemId.PRISON_79_MQ_5_ITEM, 1, InventoryAddType.PickUp);
	}

	/// <summary>
	/// Shines the Teal Magic Stone on one of the piles of junk in
	/// Warehouse No. 2.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="number"></param>
	private async Task ShineOnThePile(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Pile of Junk"));

		if (!character.Quests.IsActive(Mq3) || character.Quests.IsCompletable(Mq3))
		{
			await dialog.Msg(L("Junk, stacked up and forgotten."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.PRISON_78_MQ_3_ITEM) == 0)
		{
			await dialog.Msg(L("Without the Teal Magic Stone, it is only a pile of junk."));
			return;
		}

		if (character.Variables.Temp.GetBool(PileVar + number, false))
		{
			character.ServerMessage(L("You have already examined this area"));
			return;
		}

		var shone = await character.TimeActions.StartAsync(L("Examining through a Greenish Magic Stone"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

		if (shone != TimeActionResult.Completed)
			return;

		character.Variables.Temp.SetBool(PileVar + number, true);

		if (number != ManualPile)
		{
			character.ServerMessage(L("Nothing rises out of this pile."));
			return;
		}

		character.Inventory.Add(ItemId.PRISON_79_MQ_3_ITEM, 1, InventoryAddType.PickUp);
		character.ServerMessage(L("You have found the paper that reacts to the Greenish Magic Stone."));
	}

	/// <summary>
	/// Activates one of the Yellow Lamp's magic circles, flipping it and
	/// the two circles beside it.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="index"></param>
	private async Task ActivateTheCircle(Dialog dialog, int index)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Magic Circle"));

		if (!character.Quests.IsActive(Mq7) || character.Quests.IsCompletable(Mq7))
		{
			await dialog.Msg(L("One of the magic circles around the Yellow Lamp."));
			return;
		}

		if (!character.Variables.Temp.Has(CircleMaskVar))
		{
			await dialog.Msg(L("First, look at the Yellow Lamp and supply the magic circle with magic."));
			return;
		}

		var controlled = await character.TimeActions.StartAsync(L("Controlling Magic Circle"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(1));

		if (controlled != TimeActionResult.Completed)
			return;

		var count = CircleSpots.GetLength(0);
		var mask = character.Variables.Temp.GetInt(CircleMaskVar, 0);

		mask ^= 1 << index;
		mask ^= 1 << ((index + 1) % count);
		mask ^= 1 << ((index + count - 1) % count);

		character.Variables.Temp.SetInt(CircleMaskVar, mask);

		if (mask != AllCircles)
		{
			character.ServerMessage(LF("Magic circles active: {0}/5", this.CountCircles(mask)));
			return;
		}

		character.Quests.CompleteObjective(Mq7, "activateTheCircles");
		character.ServerMessage(L("All of the magic circles have been activated. Light the Yellow Lamp."));
	}

	/// <summary>
	/// Returns the number of active magic circles in the mask.
	/// </summary>
	/// <param name="mask"></param>
	private int CountCircles(int mask)
	{
		var count = 0;
		for (var i = 0; i < CircleSpots.GetLength(0); ++i)
		{
			if ((mask & (1 << i)) != 0)
				count++;
		}
		return count;
	}

	/// <summary>
	/// Returns whether Zanas' Soul waits at the Central Assembly Area.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheAssemblyArea(Character character)
		=> character.Quests.HasCompleted(Prison78Mq9) && !character.Quests.HasCompleted(Mq10);

	/// <summary>
	/// Returns whether the second soul is still sealed in its device.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheSoulStillHidden(Character character)
		=> !character.Quests.HasCompleted(Mq2) && !character.Quests.IsCompletable(Mq2);

	/// <summary>
	/// Returns whether the second soul is out of its device and waiting.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheSoulFreed(Character character)
		=> (character.Quests.HasCompleted(Mq2) || character.Quests.IsCompletable(Mq2)) && !character.Quests.Has(Mq3);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30154: Another Soul of Zanas(1)
//-----------------------------------------------------------------------------
public class Prison79Mq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30154);
		SetName(L("Another Soul of Zanas(1)"));
		SetDescription(L("The device hiding another piece of Zanas takes the power of the monsters."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_NPC_1", "d_prison_79", L("Talk to the Zanas' Soul in Storage"), L("You have disarmed the Demon Barrier in the Kalejimas Visiting Room. Talk to Zanas' Soul in Storage."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_NPC_1", "d_prison_79", L("Collect Evil Energy Cores by defeating monsters"), L("Zanas' Soul says that you require two different types of power. Collect Evil Energy Cores by defeating monsters."));
		SetPhase(QuestStatus.Success, "PRISON_79_NPC_1", "d_prison_79", L("Talk to Zanas' Soul"), L("You have gathered all of the Evil Energy Cores. Return to Zanas' Soul."));

		AddPrerequisite(new QuestStatusPrerequisite(30153, QuestStatus.Completed));

		AddObjective("collectCores", L("Defeat monsters to obtain Evil Energy Cores"), new CollectItemObjective("PRISON_79_MQ_1_ITEM", 10));

		AddPityDrop("PRISON_79_MQ_1_ITEM", 1.0f, 0, 1, "nuo_purple", "Socket_bow_red", "TerraNymph_mage_blue");

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30155: Another Soul of Zanas(2)
//-----------------------------------------------------------------------------
public class Prison79Mq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30155);
		SetName(L("Another Soul of Zanas(2)"));
		SetDescription(L("Two opposite powers shut down the device in Warehouse No. 3."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_NPC_1", "d_prison_79", L("Talk to Zanas' Soul"), L("Ask Zanas' Soul where you should go with the Evil Energy Cores."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_OBJ_1", "d_prison_79", L("Free the hiding Soul of Zanas"), L("You must input different energies into each side of the Secret Device in Warehouse No. 3. Place your energy and the eveil energy on either side of the Secret Device."));
		SetPhase(QuestStatus.Success, "PRISON_79_NPC_2", "d_prison_79", L("Talk to Zanas' Soul"), L("You have taken out Zanas' Soul that had been hiding in the Secret Device. Talk to Zanas' Soul."));

		AddPrerequisite(new QuestStatusPrerequisite(30154, QuestStatus.Completed));

		AddObjective("freeTheSoul", L("Free the hiding Soul of Zanas"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30156: Preparing for the worst
//-----------------------------------------------------------------------------
public class Prison79Mq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30156);
		SetName(L("Preparing for the worst"));
		SetDescription(L("The secret device instructions float out of the junk under the Teal Magic Stone's light."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_NPC_2", "d_prison_79", L("Talk to Zanas' Soul"), L("Ask what Zanas' Soul can remember."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_OBJ_2_1", "d_prison_79", L("Search for Kalejimas Secret Device Instructions"), L("Zanas' Soul remembers where the instructions for the Secret Devices are hidden. Find the Kalejimas Secret Device Instructions by shining the Teal Magic Stone on the boxes in Warehouse No. 2."));
		SetPhase(QuestStatus.Success, "PRISON_79_NPC_1", "d_prison_79", L("Talk to Zanas' Soul"), L("You have found the Kalejimas Secret Device Instructions. Talk to Zanas' Soul at Central Assembly Area."));

		AddPrerequisite(new QuestStatusPrerequisite(30155, QuestStatus.Completed));

		AddObjective("findTheManual", L("Search for the Kalejimas Secret Device Instructions in the box at Warehouse No. 2"), new CollectItemObjective("PRISON_79_MQ_3_ITEM", 1));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30157: Storage Lamp(1)
//-----------------------------------------------------------------------------
public class Prison79Mq4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30157);
		SetName(L("Storage Lamp(1)"));
		SetDescription(L("Three lamps have to be lit before the King's Red Jewel comes out."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_NPC_1", "d_prison_79", L("Talk to Zanas' Soul"), L("You have obtained the instructions for the Kalejimas Secret Device. Ask Zanas' Spirit which Device needs to be activated."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_OBJ_3", "d_prison_79", L("Talk to Zanas' Soul"), L("You have obtained the instructions for the Kalejimas Secret Device. Ask Zanas' Spirit which Device needs to be activated."));
		SetPhase(QuestStatus.Success, "PRISON_79_OBJ_3", "d_prison_79", L("Look for the Blue Lamp"), L("Zanas' Spirit says that you must recover four King's Jewels. Look for the first Blue Lamp in order to disarm the Device in which the King's Jewel is hidden."));

		AddPrerequisite(new QuestStatusPrerequisite(30156, QuestStatus.Completed));

		AddObjective("findTheLamp", L("Look for the Blue Lamp"), new ManualObjective());
	}
}

// 30158: Storage Lamp(2)
//-----------------------------------------------------------------------------
public class Prison79Mq5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30158);
		SetName(L("Storage Lamp(2)"));
		SetDescription(L("The Blue Lamp burns only on the oil kept beside it."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_OBJ_3", "d_prison_79", L("Check the Blue Lamp"), L("You have found the Blue Lamp. See if you can light it."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_OBJ_4_1", "d_prison_79", L("Retrieve Oil from the nearby Oil Pouch for the Blue Lamp"), L("How to activate the Blue Lamp : You need special Oil to light the Blue Lamp. Oil needed to light the Blue Lamp is always nearby."));
		SetPhase(QuestStatus.Success, "PRISON_79_OBJ_3", "d_prison_79", L("Give the Blue Lamp Oil"), L("You have enough Oil for the Blue Lamp. Light the Blue Lamp after Oiling it."));

		AddPrerequisite(new QuestStatusPrerequisite(30157, QuestStatus.Completed));

		AddObjective("collectOil", L("Obtain Oil for the Blue Lamp from the nearby Oil Pouch"), new CollectItemObjective("PRISON_79_MQ_5_ITEM", 7));

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8260));
		AddReward(new TakeItemReward("PRISON_79_MQ_5_ITEM"));
	}
}

// 30159: Storage Lamp(3)
//-----------------------------------------------------------------------------
public class Prison79Mq6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30159);
		SetName(L("Storage Lamp(3)"));
		SetDescription(L("The monsters carry off the Red Lamp's oil for the smell of it."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_OBJ_5", "d_prison_79", L("Check the Red Lamp"), L("You have found the Red Lamp. See if you can light it."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_OBJ_5", "d_prison_79", L("Retrieve Oil for the Red Lamp by defeating monsters"), L("How to activate the Red Lamp : You need special Oil to light the Red Lamp. Monsters like the smell of the Red Lamp's Oil."));
		SetPhase(QuestStatus.Success, "PRISON_79_OBJ_5", "d_prison_79", L("Give the Red Lamp Oil"), L("You have gathered enough Oil for the Red Lamp. Light it after giving it enough Oil."));

		AddPrerequisite(new QuestStatusPrerequisite(30158, QuestStatus.Completed));

		AddObjective("collectOil", L("Defeat nearby monsters to retrieve Oil for the Red Lamp"), new CollectItemObjective("PRISON_79_MQ_6_ITEM", 10));

		AddPityDrop("PRISON_79_MQ_6_ITEM", 1.0f, 0, 1, "nuo_purple", "Socket_bow_red", "TerraNymph_mage_blue");

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8260));
		AddReward(new TakeItemReward("PRISON_79_MQ_6_ITEM"));
	}
}

// 30160: Storage Lamp(4)
//-----------------------------------------------------------------------------
public class Prison79Mq7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30160);
		SetName(L("Storage Lamp(4)"));
		SetDescription(L("The Yellow Lamp lights when all five of its magic circles burn at once."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_NPC_1", "d_prison_79", L("Talk to Zanas' Soul"), L("You have lit both Lamps. Return to Zanas' Spirit and ask about the location of the last Lamp."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_OBJ_6", "d_prison_79", L("Activate all of the Yellow Lamp's Magic Circles"), L("How to activate the Yellow Lamp : You must activate all of the Magic Circle to light the Yellow Lamp. First activate the Yellow Lamp to send magic to the Magic Circles and control them to activate it. Activating a Magic Circle will affect the nearby Circles and cause them to either be turned on or off."));
		SetPhase(QuestStatus.Success, "PRISON_79_OBJ_6", "d_prison_79", L("Light the Yellow Lamp"), L("All Magic Circles have been activated. Light the Yellow Lamp."));

		AddPrerequisite(new QuestStatusPrerequisite(30159, QuestStatus.Completed));

		AddObjective("activateTheCircles", L("Activate all of the Yellow Lamp's Magic Circles"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30161: Storage Lamp(5)
//-----------------------------------------------------------------------------
public class Prison79Mq8Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30161);
		SetName(L("Storage Lamp(5)"));
		SetDescription(L("The monsters gather around the device holding the King's Red Jewel."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_NPC_1", "d_prison_79", L("Talk to Zanas' Soul"), L("All Lamps have been lit. Return to Zanas' Spirit and ask about the location of the King's Jewel."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_OBJ_8", "d_prison_79", L("Move to the Secret Device of Warehouse No. 1"), L("King's Red Jewel is hidden within the Secret Device at Warehouse No. 1. Go to Warehouse No. 1."));
		SetPhase(QuestStatus.Success, "PRISON_79_OBJ_8", "d_prison_79", L("Obtain the King's Red Jewel from the Secret Device"), L("Defeat the monsters near the Secret Device. Obtain the King's Red Jewel from the Secret Device."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON_79_MQ_8_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(30160, QuestStatus.Completed));

		AddObjective("killMonsters", L("Defeat the monsters around the Secret Device"), new KillObjective(8, "nuo_purple", "Socket_bow_red", "TerraNymph_mage_blue") { LayerOnly = true });

		AddReward(new ItemReward("PRISON_79_MQ_8_ITEM", 1));
		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30162: Storage Room Barrier(1)
//-----------------------------------------------------------------------------
public class Prison79Mq9Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30162);
		SetName(L("Storage Room Barrier(1)"));
		SetDescription(L("The way to Warehouse No. 4 has to be cleared for Zanas to follow."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_NPC_1", "d_prison_79", L("Talk to Zanas' Soul"), L("You have obtained the King's Red Jewel. Return to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_MQ_10_TRIGGER", "d_prison_79", L("Defeat the monsters on the way to Warehouse No. 4"), L("Zanas' Spirit says that you must disarm the Demon Barrier at the Storage Room. Defeat the monsters on the way there to allow Zanas' Spirit safe passage."));
		SetPhase(QuestStatus.Success, "PRISON_79_MQ_10_TRIGGER", "d_prison_79", L("Defeat the monsters on the way to Warehouse No. 4"), L("Zanas' Spirit says that you must disarm the Demon Barrier at the Storage Room. Defeat the monsters on the way there to allow Zanas' Spirit safe passage."));

		AddPrerequisite(new QuestStatusPrerequisite(30161, QuestStatus.Completed));

		AddObjective("clearTheWay", L("Defeat the monsters on the way to Warehouse No. 4"), new KillObjective(20, "nuo_purple", "Socket_bow_red", "TerraNymph_mage_blue"));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("The way is clear. Go to the Demon Barrier at Warehouse No. 4."));
	}
}

// 30163: Storage Room Barrier(2)
//-----------------------------------------------------------------------------
public class Prison79Mq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30163);
		SetName(L("Storage Room Barrier(2)"));
		SetDescription(L("Zanas pays again for the Dominance Magic, and the second barrier falls."));
		SetType(QuestType.Main);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_MQ_10_TRIGGER", "d_prison_79", L("Release the Demon Barrier"), L("You have defeated enough monsters on the way. Now go to the the Demon Barrier at Warehouse No. 4."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_MQ_10_TRIGGER", "d_prison_79", L("Release the Demon Barrier"), L("You have defeated enough monsters on the way. Now go to the the Demon Barrier at Warehouse No. 4."));
		SetPhase(QuestStatus.Success, "PRISON_79_MQ_10_TRIGGER", "d_prison_79", L("Release the Demon Barrier"), L("You have defeated enough monsters on the way. Now go to the the Demon Barrier at Warehouse No. 4."));

		SetTrack(QuestStatus.Success, QuestStatus.Completed, "PRISON_79_MQ_10_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(30162, QuestStatus.Completed));

		AddObjective("releaseTheBarrier", L("Release the Demon Barrier"), new ManualObjective());
	}
}

// 30197: Supply Room's Secret Device
//-----------------------------------------------------------------------------
public class Prison79Sq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30197);
		SetName(L("Supply Room's Secret Device"));
		SetDescription(L("Something might still be inside the Central Assembly Area's Secret Device."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_79");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_79_SQ_OBJ_1", "d_prison_79", L("Check the Secret Device at the Central Assembly Area"), L("Something might be inside the Central Assembly Area's Secret Device. Take a close look."));
		SetPhase(QuestStatus.InProgress, "PRISON_79_SQ_OBJ_1", "d_prison_79", L("Check the Secret Device at the Central Assembly Area"), L("Something might be inside the Central Assembly Area's Secret Device. Take a close look."));
		SetPhase(QuestStatus.Success, "PRISON_79_SQ_OBJ_1", "d_prison_79", L("Obtain the object from inside the Secret Device"), L("You have succeeded in opening the Secret Device. Take the what is inside."));

		AddPrerequisite(new LevelPrerequisite(252));

		AddObjective("openTheDevice", L("Check the Secret Device at the Central Assembly Area"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8320));
		AddReward(new ItemReward("Drug_Haste1_event", 5));
	}
}
