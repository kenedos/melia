//--- Melia Script ----------------------------------------------------------
// Delmore Manor Quest NPCs
//--- Description -----------------------------------------------------------
// Melchioras searching for the hidden Magic Power Supply Devices while
// Yane's Revelators rush the Kruvina device on their own.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle652QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Hamlet651Mq06 = new QuestId(70405);
	private readonly static QuestId Rp1 = new QuestId(60174);
	private readonly static QuestId Rp2 = new QuestId(60175);
	private readonly static QuestId Mq01 = new QuestId(70420);
	private readonly static QuestId Mq02 = new QuestId(70421);
	private readonly static QuestId Mq03 = new QuestId(70422);
	private readonly static QuestId Mq04 = new QuestId(70423);
	private readonly static QuestId Mq05 = new QuestId(70424);
	private readonly static QuestId Sq01 = new QuestId(70425);
	private readonly static QuestId Sq02 = new QuestId(70426);
	private readonly static QuestId Sq03 = new QuestId(70427);
	private readonly static QuestId Sq04 = new QuestId(70428);

	private const string Mq04TrackId = "CASTLE65_2_MQ04_TRACK";

	public const string DevicesDestroyedVar = "Gabija.Quests.Castle652Rp1.Destroyed";
	private const string DeviceVar = "Gabija.Quests.Castle652Rp1.Device";
	private const string BoxVar = "Gabija.Quests.Castle652Sq01.Box";
	private const string MushroomVar = "Gabija.Quests.Castle652Sq03.Mushroom";

	private const int AreaRange = 300;
	private const int BoardsNeeded = 25;
	private const int MushroomsNeeded = 7;
	private const int DevicesNeeded = 5;

	private static readonly TimeSpan GatherRespawn = TimeSpan.FromSeconds(30);

	private static readonly Position RampartArea = new Position(544.06f, 138.17f, -1647.98f);
	private static readonly Position PlazaArea = new Position(1634.83f, 1.22f, -872.17f);
	private static readonly Position WorkshopArea = new Position(393.52f, 104.29f, 1132.53f);

	private static readonly double[,] Boxes =
	{
		{ 847.55, 1246.02, 187 }, { 254.32, 1182.44, 224 }, { -358.66, -825.32, 90 },
		{ 1099.98, 1210.80, 90 }, { 797.64, 956.11, 90 }, { -100.90, -981.63, 90 },
		{ 451.52, 984.99, 90 }, { -263.21, -1228.92, 286 }, { -196.95, -827.91, 90 }, { 1153.60, 1014.08, 90 },
	};

	private static readonly int[] BoxModels = { 151029, 151029, 151029, 151030, 151030, 151030, 155008, 155008, 155008, 155008 };

	private static readonly double[,] Mushrooms =
	{
		{ -452.19, -1207.05 }, { -469.84, -945.71 }, { -59.10, -1124.79 }, { 1460.79, -881.58 }, { 1685.93, -985.47 }, { 1683.60, -769.50 },
	};

	private static readonly double[,] ManorDevices =
	{
		{ -1021.99, 804.52 }, { -1222.76, 1130.16 }, { -1148.97, 1457.16 }, { -912.31, 1604.08 }, { -780.41, 1633.72 }, { -596.49, 1531.83 },
		{ -434.18, 1171.55 }, { -532.83, 1045.25 }, { -639.61, 859.83 }, { -931.38, 1439.11 }, { -886.54, 1020.15 }, { -814.03, 1247.13 },
	};

	protected override void Load()
	{
		// Mage Melchioras
		//-------------------------------------------------------------------------
		AddConditionalNpc(155113, L("Mage Melchioras"), "CASTLE652_MQ_01", "f_castle_65_2", 1149.07, -281.30, 77, IsAtManorCamp, this.Melchioras);

		// Revelator Mihail
		//-------------------------------------------------------------------------
		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE652_MQ_02", "f_castle_65_2", 1188.85, -350.10, 121, IsAtManorCamp, async dialog =>
		{
			dialog.SetTitle(L("Revelator Mihail"));

			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("It's true that Melchioras made a mistake, but Yane was too rash this time. It would be fine if Yane has a good solution... but this doesn't feel good."));
			else
				await dialog.Msg(L("There are always followers of Yane gathering near her. Yane is determined and reliable... But is also hasty."));
		});

		// Follower Bigs
		//-------------------------------------------------------------------------
		AddConditionalNpc(11282, L("Follower Bigs"), "CASTLE652_SQ_01", "f_castle_65_2", 1424.45, -2.29, 356, c => c.Quests.HasCompleted(Mq05), this.Bigs);

		// Follower Wedge
		//-------------------------------------------------------------------------
		AddConditionalNpc(11283, L("Follower Wedge"), "CASTLE652_SQ_03", "f_castle_65_2", 1243.18, -25.92, 38, c => c.Quests.HasCompleted(Mq05), this.Wedge);

		// Follower Nedluss
		//-------------------------------------------------------------------------
		AddConditionalNpc(58292, L("Follower Nedluss"), "CASTLE652_RP_1_NPC", "f_castle_65_2", -933.94, 396.67, 44, c => c.Quests.HasCompleted(Mq05), this.Nedluss);

		// The Kruvina device at the Palma Central Plaza
		//-------------------------------------------------------------------------
		AddNpc(155104, L("Kruvina Central Device"), "CASTLE652_KRUVINA_CENTER", "f_castle_65_2", -801.40, 148.53, 90);

		AddQuestTrigger("CASTLE652_MQ_05_TRIGGER", "f_castle_65_2", -153.01, 247.43, 200, async args =>
		{
			if (args.Initiator is Character character && character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.StartQuestTrack(Mq05);

			await Task.CompletedTask;
		});

		// The Magic Power Supply Devices hidden around the manor
		//-------------------------------------------------------------------------
		AddConditionalNpc(155105, L("Demonic Power Supply Device"), "CASTLE652_MQ_02_PILLAR", "f_castle_65_2", 509.23, -1839.06, 90, c => c.Quests.IsCompletable(Mq02) || FCastle653QuestNpcsScript.IsManorPillarStanding(c, 1));
		AddConditionalNpc(155105, L("Demonic Power Supply Device"), "CASTLE652_MQ_04_PILLAR", "f_castle_65_2", 433.11, 1283.32, 90, c => IsWorkshopPillarRevealed(c) || FCastle653QuestNpcsScript.IsManorPillarStanding(c, 2));
		AddConditionalNpc(155105, L("Demonic Power Supply Device"), "CASTLE652_MQ_PILLAR_EX", "f_castle_65_2", 1987.75, 1009.20, 90, c => c.Quests.HasCompleted(Mq05) && !FCastle653QuestNpcsScript.IsManorPillarDestroyed(c, 3));

		AddConditionalNpc(155116, L("Wreckage of the Demonic Power Supply Device"), "CASTLE653_SQ_02_PILLAR1", "f_castle_65_2", 509.23, -1839.06, 90, c => FCastle653QuestNpcsScript.IsManorPillarDestroyed(c, 1));
		AddConditionalNpc(155116, L("Wreckage of the Demonic Power Supply Device"), "CASTLE653_SQ_02_PILLAR2", "f_castle_65_2", 433.11, 1283.32, 90, c => FCastle653QuestNpcsScript.IsManorPillarDestroyed(c, 2));
		AddConditionalNpc(155116, L("Wreckage of the Demonic Power Supply Device"), "CASTLE653_SQ_02_PILLAR3", "f_castle_65_2", 1987.75, 1009.20, 90, c => FCastle653QuestNpcsScript.IsManorPillarDestroyed(c, 3));

		// The spots Melchioras marked for the crystal
		//-------------------------------------------------------------------------
		AddConditionalNpc(20041, "UnvisibleName", "CASTLE652_MQ_02_AREA", "f_castle_65_2", RampartArea.X, RampartArea.Z, 90, c => c.Quests.IsActive(Mq02) && !c.Quests.IsCompletable(Mq02));
		AddConditionalNpc(20041, "UnvisibleName", "CASTLE652_MQ_03_AREA", "f_castle_65_2", PlazaArea.X, PlazaArea.Z, 90, c => c.Quests.IsActive(Mq03) && !c.Quests.IsCompletable(Mq03));
		AddConditionalNpc(20041, "UnvisibleName", "CASTLE652_MQ_04_AREA", "f_castle_65_2", WorkshopArea.X, WorkshopArea.Z, 90, c => c.Quests.IsActive(Mq04) && !c.Quests.IsCompletable(Mq04));

		// The manor entrance, where Melchioras waits
		//-------------------------------------------------------------------------
		AddQuestTrigger("CASTLE652_MQ_01_TRIGGER", "f_castle_65_2", 1602.03, -118.47, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Hamlet651Mq06))
			{
				character.Quests.CompleteObjective(Hamlet651Mq06, "goToManor");
				character.Quests.Complete(Hamlet651Mq06);
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				character.Quests.Start(Mq01);
				character.LookAround();
			}

			await Task.CompletedTask;
		});

		// Piles of boxes on the Kalbos Vacant Lot and the Handicraft Workshop Road
		//-------------------------------------------------------------------------
		for (var i = 0; i < Boxes.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "CASTLE652_SQ_01_BOX" : "CASTLE652_SQ_01_BOX_" + number;
			var name = BoxModels[i] == 155008 ? L("Wooden Chest") : L("Pile of Boxes");

			AddConditionalNpc(BoxModels[i], name, uniqueName, "f_castle_65_2", Boxes[i, 0], Boxes[i, 1], Boxes[i, 2],
				c => c.Quests.IsActive(Sq01) && !c.Quests.IsCompletable(Sq01),
				async dialog =>
				{
					var character = dialog.Player;
					if (!Gather(character, BoxVar + number, L("You've already searched this spot.")))
						return;

					var boards = Math.Min(GameRandom.Get().Next(2, 5), BoardsNeeded - character.Inventory.CountItem(ItemId.CASTLE65_2_SQ01_ITEM));
					if (boards > 0)
						character.Inventory.Add(ItemId.CASTLE65_2_SQ01_ITEM, boards, InventoryAddType.PickUp);

					await Task.CompletedTask;
				});
		}

		// Thorn Mushrooms on the Kalbos Vacant Lot and the Buried Central Plaza
		//-------------------------------------------------------------------------
		for (var i = 0; i < Mushrooms.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "CASTLE652_SQ_03_MUSHROOM" : "CASTLE652_SQ_03_MUSHROOM_" + number;

			AddConditionalNpc(155023, L("Thorn Mushroom"), uniqueName, "f_castle_65_2", Mushrooms[i, 0], Mushrooms[i, 1], 49,
				c => c.Quests.IsActive(Sq03) && !c.Quests.IsCompletable(Sq03),
				async dialog =>
				{
					var character = dialog.Player;
					if (character.Inventory.CountItem(ItemId.CASTLE65_2_SQ03_ITEM) >= MushroomsNeeded)
						return;

					if (Gather(character, MushroomVar + number, L("The Thorn Mushroom here has already been picked.")))
						character.Inventory.Add(ItemId.CASTLE65_2_SQ03_ITEM, 1, InventoryAddType.PickUp);

					await Task.CompletedTask;
				});
		}

		// The magic devices in the manor's garden watchtower
		//-------------------------------------------------------------------------
		for (var i = 0; i < ManorDevices.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "CASTLE652_RP_1_OBJ" : "CASTLE652_RP_1_OBJ_" + number;

			AddConditionalNpc(153063, L("Manor Magic Device"), uniqueName, "f_castle_65_2", ManorDevices[i, 0], ManorDevices[i, 1], 0,
				c => c.Quests.IsActive(Rp1) && !c.Quests.IsCompletable(Rp1) && !c.Variables.Perm.GetBool(DeviceVar + number, false),
				async dialog => await this.DestroyManorDevice(dialog, number));
		}
	}

	/// <summary>
	/// Mage Melchioras' dialog at the manor entrance.
	/// </summary>
	private async Task Melchioras(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mage Melchioras"));

		if (character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("Oh... Did you see it all? It's all my fault."));
			await dialog.Msg(L("Delmore Rephaim is not a shabby person... Even Revelator Mihail received the same treatment because of me."));
			await dialog.Msg(L("We can't just give up like this. Somehow we have to find a way to neutralize the Magic Power Supply Device!"));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (character.Quests.IsCompletable(Mq02))
		{
			await dialog.Msg(L("Revelator Mihail has gone to take care of the Magic Power Supply Device. But... There seems to be a few new Supply Devices..."));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("So there was no Magic Power Supply Device, only a horde of monsters? That's odd... I'm sorry."));
			await dialog.Msg(L("I must have been a little confused earlier. Next time I'll try to pay more attention so I don't give you any wrong information."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("What? You say Yane found a new way to do it?"));
			await dialog.Msg(L("How does that even... Can you share any more details?"));
			await dialog.CompleteQuest(Mq04);
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			await dialog.Msg(L("Thinking that it would be the same situation as when I ran was a mistake. I'm sure that there's a hidden Magic Power Supply Device somewhere."));

			var answer = await dialog.SelectQuestOffer(Mq02, L("You... There is something else I wish to ask of you."),
				Option(L("What can I help you with?"), "accept"),
				Option(L("I think the other Revelators will find a way"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq02);
				GiveCrystal(character);
				character.LookAround();

				await dialog.Msg(L("If my calculations are correct, it should be somewhere in the Rampart Reconstruction District... Head over there with this crystal."));
				await dialog.Msg(L("You'll be able to find the Magic Power Supply Device when you put this crystal on the floor. I'll be notified when one is discovered and send Revelator Mihail to take care of it."));
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("Evil energy is continuing to flow in... near the Buried Central Plaza. I didn't sense it a while ago, but why..."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("Take these jewels please. I am a bit suspicious but there is just no other way."),
				Option(L("Leave it to me"), "accept"),
				Option(L("I'm a little tired now"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq03);
				GiveCrystal(character);
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			var answer = await dialog.SelectQuestOffer(Mq04, L("This time I'm sure. Please... Will you go to the Handicraft Workshop once again?"),
				Option(L("I will go"), "accept"),
				Option(L("I can't believe anymore"), "leave")
			);

			if (answer == "accept")
			{
				character.SetEtcProperty(Mq04TrackId, 0);
				character.Quests.Start(Mq04);
				GiveCrystal(character);
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("So everyone gathers around the device and attacks at the same time? Are you sure...? I really don't think that's the way to destroy it!"));
			await dialog.Msg(L("Now I know. I realize what went wrong... Delmore Rephaim was watching us all the entire time!"));
			await dialog.Msg(L("Whatever we do we can't gather by the device! We need to stop Yane, quick!"));

			var answer = await dialog.SelectQuestOffer(Mq05, L("This Kruvina... it's made using human lives!"),
				Option(L("Quick, follow me"), "accept"),
				Option(L("Please wait a while"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq05);

				dialog.SetTitle(L("Revelator Mihail"));
				await dialog.Msg(L("Melchioras, did you say... human lives? Oh no, we need to do something!"));
				await dialog.Msg(L("I'll go first and try to talk to her. You and Melchioras, come with me!"));

				dialog.SetTitle(L("Mage Melchioras"));
				await dialog.Msg(L("If the device is activated, the Revelators will all turn into Kruvina! We need to go to Palma Central Plaza right now and stop Yane!"));

				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("There is now way I am wrong. I've designed it after all... If there is even a faint chance of that, I can only imagine that there is a new magic circle."));
			character.Quests.ReplayQuestTrack(Mq01);
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			GiveCrystal(character);
			await dialog.Msg(L("There will be no second mistake. I'm positive that my calculations are correct."));
			return;
		}

		if (character.Quests.IsActive(Mq03) || character.Quests.IsActive(Mq04))
		{
			GiveCrystal(character);
			await dialog.Msg(L("I know it may sound like an excuse... But it's just too strange. There's no way I am wrong..."));
			await dialog.Msg(L("I think things are going in a bad direction. I... I'm sorry. I'll keep concentrating."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("There is now way I am wrong. I've designed it after all... If there is even a faint chance of that, I can only imagine that there is a new magic circle."));
		else
			await dialog.Msg(L("Making a Magic Power Supply Device Kruvina is impossible. I am sure that there is another Supply Device hidden somewhere forming another magic circle."));
	}

	/// <summary>
	/// Follower Bigs' dialog.
	/// </summary>
	private async Task Bigs(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Follower Bigs"));

		if (character.Quests.IsCompletable(Sq01))
		{
			await dialog.Msg(L("Wow, that's... Thank you so much. I should go and pray to the goddesses for Melchioras to come back safe and sound."));
			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (character.Quests.IsCompletable(Sq02))
		{
			await dialog.Msg(L("Everyone feels so sorry for Melchioras. I don't even know how to apologize to him..."));
			await dialog.Msg(L("You're... close to Melchioras, right? It would be great if you could let him know we're sorry..."));
			await dialog.CompleteQuest(Sq02);
			return;
		}

		if (character.Quests.IsCompletable(Rp2))
		{
			await dialog.Msg(L("I think that this will be enough to find a weakness or two. Thank you!"));
			await dialog.CompleteQuest(Rp2);
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("I also thought at first that Melchioras was leading us into a trap... I really don't know how I'll ever be able to face him now."));
			await dialog.Msg(L("We should set up a base camp to help Melchioras heal. That way he'll be safe when he comes back... Let's hope he does..."));
			await dialog.Msg(L("Everything else is fine, but the barricade is going to be a problem. There aren't enough materials around to build one, and we can't tear down another barricade to make our own..."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("And after the Kruvina device, my condition isn't the best either... If that's okay with you, will you help us set up the barricade?"),
				Option(L("Just tell me what you need"), "accept"),
				Option(L("It's best to gather as much as possible from this area"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq01);
				character.LookAround();

				await dialog.Msg(L("I think I saw some crates on the way to the Handicraft Workshop and on the Kalbos Vacant Lot. Try and find some wooden boards there."));
			}
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("Thanks to Melchioras I'm alive... I feel so sorry to him, but all I can do now is prepare for his return so we can nurse him back to health in safety..."));

			var answer = await dialog.SelectQuestOffer(Sq02, L("There are way too many demons! And my condition isn't really... Will you please help me?"),
				Option(L("I'll defeat some demons around here"), "accept"),
				Option(L("I have no time to help you"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Sq02);
			return;
		}

		if (!character.Quests.Has(Rp2) && character.Quests.MeetsPrerequisites(Rp2))
		{
			await dialog.Msg(L("We'll need to know about the demons well in order to drive them out of this area."));

			var answer = await dialog.SelectQuestOffer(Rp2, L("Defeat the demons and bring back their condensed magic. Melchioras may be able to do some research in order to find a fatal weakness."),
				Option(L("I'll help you"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Rp2);
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			await dialog.Msg(L("It's hard to believe, honestly. We barely managed to destroy the Magic Power Supply Device, but if that was all in vain..."));
			await dialog.Msg(L("But the fact is, this was our fault. How are we ever going to face Melchioras again..."));
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			await dialog.Msg(L("If I knew what kind of place this was I would've thought twice about coming. But Melchioras... he came back here knowing what this was."));
			return;
		}

		if (character.Quests.IsActive(Rp2))
		{
			await dialog.Msg(L("I'm sure that they'll be easier to handle if we know their weaknesses..."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("If only we had the skill, we could go with you and help you... I'm just sorry about my own limitations."));
		else
			await dialog.Msg(L("To think they wanted to kidnap a person and run away... That was no normal human being."));
	}

	/// <summary>
	/// Follower Wedge's dialog.
	/// </summary>
	private async Task Wedge(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Follower Wedge"));

		if (character.Quests.IsCompletable(Sq03))
		{
			await dialog.Msg(L("Everyone was really unfair to Melchioras. I have no excuse either. I should have listened to him..."));
			await dialog.Msg(L("Anyway, thanks for the mushrooms. I hope I get to give this medicine to Melchioras."));
			await dialog.CompleteQuest(Sq03);
			return;
		}

		if (character.Quests.IsCompletable(Sq04))
		{
			await dialog.Msg(L("It's time to roll up our sleeves now. There is something we used to do in my hometown, except we had demons there, not monsters..."));
			await dialog.CompleteQuest(Sq04);
			return;
		}

		if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
		{
			await dialog.Msg(L("I knew this would happen. If Melchioras was really trying to trick us he would've taken us to the device right away."));

			var answer = await dialog.SelectQuestOffer(Sq03, L("I'm going to make some medicine for when Melchioras returns... Would you please give me a hand?"),
				Option(L("Alright, I'll help you"), "accept"),
				Option(L("I don't have time for it"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq03);
				character.LookAround();

				await dialog.Msg(L("There's a type of mushroom called Thorn Mushroom. It helps people with serious injuries regain their health."));
				await dialog.Msg(L("They're the only type of mushroom that grows on the Kalbos Vacant Lot and the Buried Central Plaza... I'm sure they'll be easy to find."));
			}
			return;
		}

		if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
		{
			await dialog.Msg(L("Melchioras is truly an honorable man. He did after all sacrifice himself for the people who vilified him."));
			await dialog.Msg(L("We have many injured, and there's monsters and demons everywhere. If we can't wipe them all out, our best bet is to chase away the monsters as much as we can."));

			var answer = await dialog.SelectQuestOffer(Sq04, L("I'm sorry, but could you collect some Charog sap? I want to make a monster repellant."),
				Option(L("I'll collect it"), "accept"),
				Option(L("I have more urgent issues to tend to"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Sq04);
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("If Melchioras doesn't make it... I can hardly imagine how guilty we'll feel, not just Yane but all of us."));
			return;
		}

		if (character.Quests.IsActive(Sq04))
		{
			await dialog.Msg(L("The people who lived here... they must have all been turned into Kruvina. I wish Melchioras had told us everything from the beginning..."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("When the magic circle was activated it felt as if a hand was pulling on my spirit. Not an experience I'd like to repeat."));
		else
			await dialog.Msg(L("There's no telling how far the demons' evil deeds will go. Just look at how they built that device."));
	}

	/// <summary>
	/// Follower Nedluss' dialog.
	/// </summary>
	private async Task Nedluss(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Follower Nedluss"));

		if (character.Quests.IsCompletable(Rp1))
		{
			await dialog.Msg(L("Well done. I'm sure that there will be a few more to destroy if we comb the area but... I think we can call it a day."));
			await dialog.CompleteQuest(Rp1);
			character.LookAround();
			return;
		}

		if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
		{
			await dialog.Msg(L("I've decided to stay here and help out as repentance to my wrongs against Melchioras."));
			await dialog.Msg(L("I've just found several suspicious spell devices on the Garden Watchtower. However, they're a bit too much for me to deal by myself."));

			var answer = await dialog.SelectQuestOffer(Rp1, L("So I was thinking... could you destroy them for me?"),
				Option(L("I will get rid of it"), "accept"),
				Option(L("That doesn't really matter."), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(DevicesDestroyedVar, 0);
				for (var i = 1; i <= ManorDevices.GetLength(0); ++i)
					character.Variables.Perm.Set(DeviceVar + i, false);

				character.Quests.Start(Rp1);
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Rp1))
		{
			await dialog.Msg(L("I shudder to think of how many of those monsterous devices are still hidden..."));
			return;
		}

		await dialog.Msg(L("I hope Melchioras gets well soon..."));
	}

	/// <summary>
	/// Destroys one of the magic devices hidden in the manor.
	/// </summary>
	private async Task DestroyManorDevice(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Rp1) || character.Quests.IsCompletable(Rp1) || character.Variables.Perm.GetBool(DeviceVar + number, false))
			return;

		dialog.Npc.PlayEffect("F_explosion012", 1f);
		character.Variables.Perm.Set(DeviceVar + number, true);

		var destroyed = character.Variables.Perm.GetInt(DevicesDestroyedVar, 0) + 1;
		character.Variables.Perm.SetInt(DevicesDestroyedVar, destroyed);
		character.ServerMessage(LF("Magic devices destroyed: {0}/{1}", Math.Min(destroyed, DevicesNeeded), DevicesNeeded));
		character.LookAround();

		await Task.CompletedTask;
	}

	/// <summary>
	/// Sets Melchioras' crystal down on the floor to reveal any hidden
	/// Magic Power Supply Device.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CASTLE65_2_MQ02_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_castle_65_2")
		{
			character.ServerMessage(L("The crystal does not react here."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
		{
			if (!IsNear(character, RampartArea))
				return ItemUseResult.OkayNotConsumed;

			character.PlayEffect("F_lineup009_ground", 2f);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("A hidden Demonic Power Supply Device has shown up!"), 3);
			character.Quests.CompleteObjective(Mq02, "searchRampart");
			character.LookAround();
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
		{
			if (!IsNear(character, PlazaArea))
				return ItemUseResult.OkayNotConsumed;

			character.PlayEffect("F_lineup009_ground", 2f);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("There is no device here, only a horde of monsters!"), 3);
			F3Cmlake83QuestNpcsScript.SpawnAmbush(character, character.Position, "charog", "PagNanny", "PagWheeler", "Paggnat");
			character.Quests.CompleteObjective(Mq03, "searchPlaza");
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
		{
			if (!IsNear(character, WorkshopArea))
				return ItemUseResult.OkayNotConsumed;

			character.Quests.CompleteObjective(Mq04, "searchWorkshop");
			character.LookAround();
			character.SetEtcProperty(Mq04TrackId, 0);
			_ = character.Tracks.Start(Mq04TrackId, TimeSpan.FromMilliseconds(500));
			return ItemUseResult.OkayNotConsumed;
		}

		character.ServerMessage(L("The crystal does not react here."));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Returns whether the character stands where Melchioras asked for the
	/// crystal, telling them so if not.
	/// </summary>
	private static bool IsNear(Character character, Position area)
	{
		if (character.Position.Get2DDistance(area) <= AreaRange)
			return true;

		character.ServerMessage(L("The crystal does not react here. Set it down at the place Melchioras marked."));
		return false;
	}

	/// <summary>
	/// Hands Melchioras' crystal over again, unless the character still
	/// carries it.
	/// </summary>
	private static void GiveCrystal(Character character)
	{
		if (character.Inventory.CountItem(ItemId.CASTLE65_2_MQ02_ITEM) == 0)
			character.Inventory.Add(ItemId.CASTLE65_2_MQ02_ITEM, 1, InventoryAddType.PickUp);
	}

	/// <summary>
	/// Marks a gathering spot as used, returning false while it has not
	/// grown back yet.
	/// </summary>
	private static bool Gather(Character character, string spotVar, string usedMessage)
	{
		var gatheredAt = character.Variables.Temp.GetLong(spotVar, 0);
		if (gatheredAt != 0 && DateTime.Now - new DateTime(gatheredAt) < GatherRespawn)
		{
			character.ServerMessage(usedMessage);
			return false;
		}

		character.Variables.Temp.SetLong(spotVar, DateTime.Now.Ticks);
		return true;
	}

	/// <summary>
	/// Returns whether Melchioras and Mihail are waiting at the manor
	/// entrance.
	/// </summary>
	private static bool IsAtManorCamp(Character character)
		=> character.Quests.Has(Hamlet651Mq06) && !character.Quests.Has(Mq05);

	/// <summary>
	/// Returns whether Yane's group has uncovered the device on the
	/// Handicraft Workshop Road.
	/// </summary>
	private static bool IsWorkshopPillarRevealed(Character character)
		=> (character.Quests.IsCompletable(Mq04) || character.Quests.HasCompleted(Mq04)) && !character.Quests.HasCompleted(Mq05);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 70420: Unfortunate Distrust
//-----------------------------------------------------------------------------
public class FCastle652Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70420);
		SetName(L("Unfortunate Distrust"));
		SetDescription(L("Mage Melchioras seems to have split up from the group of Revelators. Talk to Mage Melchioras."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_MQ_01_TRIGGER", "f_castle_65_2", L("Go to Delmore Manor"), L("Melchioras wants to meet up at Delmore Manor, where the Kruvina device is located. Join Mage Melchioras at Delmore Manor."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_MQ_01_TRIGGER", "f_castle_65_2", L("Talk to Mage Melchioras"), L("Mage Melchioras seems to have split up from the group of Revelators. Talk to Mage Melchioras."));
		SetPhase(QuestStatus.Success, "CASTLE652_MQ_01", "f_castle_65_2", L("Talk to Mage Melchioras"), L("Mage Melchioras seems to have split up from the group of Revelators. Talk to Mage Melchioras."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CASTLE65_2_MQ01_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(70405, QuestStatus.Completed));

		AddObjective("watchRevelators", L("Talk to Mage Melchioras"), new ManualObjective());
	}
}

// 70421: Fast Return
//-----------------------------------------------------------------------------
public class FCastle652Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70421);
		SetName(L("Fast Return"));
		SetDescription(L("Mage Melchioras believes there is a Magic Power Supply Device in the Rampart Reconstruction District. Go there and set up the crystal to reveal any hidden devices."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_MQ_01", "f_castle_65_2", L("Talk to Mage Melchioras"), L("Mage Melchioras seems to be looking for a new way to neutralize the Kruvina device. Keep talking to Mage Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_MQ_02_AREA", "f_castle_65_2", L("Search for a Magic Power Supply Device at the Rampart Reconstruction District"), L("Mage Melchioras believes there is a Magic Power Supply Device in the Rampart Reconstruction District. Go there and set up the crystal to reveal any hidden devices."));
		SetPhase(QuestStatus.Success, "CASTLE652_MQ_01", "f_castle_65_2", L("Report to Mage Melchioras"), L("You have found the hidden Magic Power Supply Device. Return to Mage Melchioras."));

		AddPrerequisite(new QuestStatusPrerequisite(70420, QuestStatus.Completed));

		AddObjective("searchRampart", L("Search for the Magic Power Supply Device"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 1020));
	}
}

// 70422: The Investigation Continues
//-----------------------------------------------------------------------------
public class FCastle652Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70422);
		SetName(L("The Investigation Continues"));
		SetDescription(L("Mage Melchioras says he felt a new Magic Power Supply Device. Go to the Buried Central Plaza and set up the crystal to reveal any hidden devices."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_MQ_01", "f_castle_65_2", L("Talk to Mage Melchioras"), L("Mage Melchioras believes there are other Magic Power Supply Devices. Keep talking to Mage Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_MQ_03_AREA", "f_castle_65_2", L("Search for a Magic Power Supply Device at the Buried Central Plaza"), L("Mage Melchioras says he felt a new Magic Power Supply Device. Go to the Buried Central Plaza and set up the crystal to reveal any hidden devices."));
		SetPhase(QuestStatus.Success, "CASTLE652_MQ_01", "f_castle_65_2", L("Report to Mage Melchioras"), L("It seems this time his prediction was wrong. Return to Mage Melchioras and let him know."));

		AddPrerequisite(new QuestStatusPrerequisite(70421, QuestStatus.Completed));

		AddObjective("searchPlaza", L("Search for the Magic Power Supply Device at the Buried Plaza"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 1020));
	}
}

// 70423: Rash Judgement
//-----------------------------------------------------------------------------
public class FCastle652Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70423);
		SetName(L("Rash Judgement"));
		SetDescription(L("This time, Mage Melchioras thinks there is a Magic Power Supply Device at the Handicraft Workshop Road. Go there and set up the crystal to reveal any hidden devices."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_MQ_01", "f_castle_65_2", L("Talk to Mage Melchioras"), L("Mage Melchioras feels sorry for his lack of judgement. Keep talking to him."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_MQ_04_AREA", "f_castle_65_2", L("Search for a Magic Power Supply Device at the Handicraft Workshop Road"), L("This time, Mage Melchioras thinks there is a Magic Power Supply Device at the Handicraft Workshop Road. Go there and set up the crystal to reveal any hidden devices."));
		SetPhase(QuestStatus.Success, "CASTLE652_MQ_01", "f_castle_65_2", L("Talk to Mage Melchioras"), L("It looks like Yane is going to try and destroy the central device purely by force. Go back and tell Melchioras about this immediately."));

		AddPrerequisite(new QuestStatusPrerequisite(70422, QuestStatus.Completed));

		AddObjective("searchWorkshop", L("Search for the Magic Power Supply Device at the Workshop Road"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 1020));
		AddReward(new TakeItemReward("CASTLE65_2_MQ02_ITEM", -1));
	}
}

// 70424: Kruvina and the Revelators
//-----------------------------------------------------------------------------
public class FCastle652Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70424);
		SetName(L("Kruvina and the Revelators"));
		SetDescription(L("Mage Melchioras says the Kruvina runs on human lives and gathering around it would be a fatal mistake. Go to the Palma Central Plaza and stop Yane from carrying out her plans!"));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_2", "f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_MQ_01", "f_castle_65_2", L("Talk to Mage Melchioras"), L("Mage Melchioras looks livid after hearing about what Yane is planning to do. Keep talking to him."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_MQ_05_TRIGGER", "f_castle_65_2", L("Stop Revelator Yane at the Palma Central Plaza"), L("Mage Melchioras says the Kruvina runs on human lives and gathering around it would be a fatal mistake. Go to the Palma Central Plaza and stop Yane from carrying out her plans!"));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_01_1", "f_castle_65_3", L("Follow Yane into the Delmore Outskirts"), L("Delmore Rephaim has kidnapped an injured Melchioras and escaped, the other Revelators chasing after him immediately after. Go to the outskirts of Delmore and join Revelator Yane."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CASTLE65_2_MQ05_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(70423, QuestStatus.Completed));

		AddObjective("stopYane", L("Stop Revelator Yane at the Palma Central Plaza"), new ManualObjective());
	}
}

// 70425: First Steps to Camp Defense
//-----------------------------------------------------------------------------
public class FCastle652Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70425);
		SetName(L("First Steps to Camp Defense"));
		SetDescription(L("Bigs wants to create a barricade to protect Melchioras once he returns. Go to the Kalbos Vacant Lot and Handicraft Workshop Road and look in box piles for wood boards to use."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_SQ_01", "f_castle_65_2", L("Talk to Follower Bigs"), L("It looks like Follower Bigs is in need of your help. Talk to Follower Bigs."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_SQ_01_BOX", "f_castle_65_2", L("Obtain Useful Wooden Boards from Box Piles"), L("Bigs wants to create a barricade to protect Melchioras once he returns. Go to the Kalbos Vacant Lot and Handicraft Workshop Road and look in box piles for wood boards to use."));
		SetPhase(QuestStatus.Success, "CASTLE652_SQ_01", "f_castle_65_2", L("Talk to Follower Bigs"), L("You have collected a handful of useful wooden boards. Bring them to Follower Bigs."));

		AddPrerequisite(new QuestStatusPrerequisite(70424, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(70));

		AddObjective("collectBoards", L("Obtain Useful Wooden Boards from old chests and others"), new CollectItemObjective("CASTLE65_2_SQ01_ITEM", 25));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 680));
		AddReward(new TakeItemReward("CASTLE65_2_SQ01_ITEM", -1));
	}
}

// 70426: Eat or Be Eaten
//-----------------------------------------------------------------------------
public class FCastle652Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70426);
		SetName(L("Eat or Be Eaten"));
		SetDescription(L("Bigs hopes you will clear out the demons in the area for those who are now injured. As instructed, defeat any demons nearby."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_SQ_01", "f_castle_65_2", L("Talk to Follower Bigs"), L("It looks like Follower Bigs needs your help. Listen to what he has to say."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_SQ_01", "f_castle_65_2", L("Defeat nearby demons"), L("Bigs hopes you will clear out the demons in the area for those who are now injured. As instructed, defeat any demons nearby."));
		SetPhase(QuestStatus.Success, "CASTLE652_SQ_01", "f_castle_65_2", L("Talk to Follower Bigs"), L("You have defeated the demons as requested. Return to Follower Bigs."));

		AddPrerequisite(new QuestStatusPrerequisite(70424, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(70));

		AddObjective("killDemons", L("Defeat nearby demons"), new KillObjective(12, "charog", "PagNanny", "PagWheeler", "Paggnat"));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 1020));
	}
}

// 70427: A Sorrowful Heart
//-----------------------------------------------------------------------------
public class FCastle652Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70427);
		SetName(L("A Sorrowful Heart"));
		SetDescription(L("Follower Wedge wants to make some medicine to help Melchioras recover his health later. Go and collect some thorn mushrooms from the Kalbos Empty Lot and Buried Central Plaza."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_SQ_03", "f_castle_65_2", L("Talk to Follower Wedge"), L("Follower Wedge seems to be worried about Mage Melchioras. Talk to him."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_SQ_03_MUSHROOM", "f_castle_65_2", L("Collect Thorn Mushrooms"), L("Follower Wedge wants to make some medicine to help Melchioras recover his health later. Go and collect some thorn mushrooms from the Kalbos Empty Lot and Buried Central Plaza."));
		SetPhase(QuestStatus.Success, "CASTLE652_SQ_03", "f_castle_65_2", L("Deliver to Follower Wedge"), L("You have collected enough mushrooms now. Bring them to Follower Wedge."));

		AddPrerequisite(new QuestStatusPrerequisite(70424, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(70));

		AddObjective("collectMushrooms", L("Collect Thorn Mushrooms"), new CollectItemObjective("CASTLE65_2_SQ03_ITEM", 7));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 680));
		AddReward(new TakeItemReward("CASTLE65_2_SQ03_ITEM", -1));
	}
}

// 70428: Hometown Secret
//-----------------------------------------------------------------------------
public class FCastle652Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70428);
		SetName(L("Hometown Secret"));
		SetDescription(L("Follower Wedge wants to produce a monster repellent. Defeat Charogs and collect their sap to use as material for the repellent."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_SQ_03", "f_castle_65_2", L("Talk to Follower Wedge"), L("More than demons, Follower Wedge is worried about the monsters roaming about. Talk to Follower Wedge."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_SQ_03", "f_castle_65_2", L("Collect Charog Sap"), L("Follower Wedge wants to produce a monster repellent. Defeat Charogs and collect their sap to use as material for the repellent."));
		SetPhase(QuestStatus.Success, "CASTLE652_SQ_03", "f_castle_65_2", L("Deliver to Follower Wedge"), L("You have now collected enough Charog sap. Bring it to Follower Wedge."));

		AddPrerequisite(new QuestStatusPrerequisite(70424, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(70));

		AddObjective("collectSap", L("Collect Charog Sap"), new CollectItemObjective("CASTLE65_2_SQ04_ITEM", 16));
		AddPityDrop("CASTLE65_2_SQ04_ITEM", 0.8f, 3, 1, "charog");

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 1020));
		AddReward(new TakeItemReward("CASTLE65_2_SQ04_ITEM", -1));
	}
}

// 60174: Hidden Magic Devices
//-----------------------------------------------------------------------------
public class FCastle652Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60174);
		SetName(L("Hidden Magic Devices"));
		SetDescription(L("Follower Nedluss has asked you to destroy the magic devices that have been discovered within the Manor."));
		SetType(QuestType.Repeat);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_RP_1_NPC", "f_castle_65_2", L("Talk to Follower Nedluss"), L("Follower Nedluss is waiting for help at Delmore Manor."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_RP_1_OBJ", "f_castle_65_2", L("Destroy the magic devices at the Manor"), L("Follower Nedluss has asked you to destroy the magic devices that have been discovered within the Manor."));
		SetPhase(QuestStatus.Success, "CASTLE652_RP_1_NPC", "f_castle_65_2", L("Report back to Follower Nedluss"), L("You have destroyed enough magic devices. Go back and report to Follower Nedluss."));

		AddPrerequisite(new QuestStatusPrerequisite(70424, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(70));

		AddObjective("destroyDevices", L("Destroy the Manor Magic Devices"), new VariableCheckObjective(FCastle652QuestNpcsScript.DevicesDestroyedVar, 5, isPermanent: true));

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 60175: Efforts Towards Redemption
//-----------------------------------------------------------------------------
public class FCastle652Rp2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60175);
		SetName(L("Efforts Towards Redemption"));
		SetDescription(L("Follower Bigs says that he requires Condensed Magic from demons to help Melchioras. Defeat the demons from the surrounding area to collect condensed magic."));
		SetType(QuestType.Repeat);
		SetLocation("f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE652_SQ_01", "f_castle_65_2", L("Talk to Follower Bigs"), L("Follower Bigs is waiting for a Revelator at the Delmore Manor."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_SQ_01", "f_castle_65_2", L("Collect Condensed Magic"), L("Follower Bigs says that he requires Condensed Magic from demons to help Melchioras. Defeat the demons from the surrounding area to collect condensed magic."));
		SetPhase(QuestStatus.Success, "CASTLE652_SQ_01", "f_castle_65_2", L("Report back to Follower Bigs"), L("You have collected sufficient Condensed Magic. Go back to Follower Bigs."));

		AddPrerequisite(new QuestStatusPrerequisite(70424, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(70));

		AddObjective("collectMagic", L("Collect Concentrated Evil Energy"), new CollectItemObjective("CASTLE652_RP_2_ITEM", 12));
		AddPityDrop("CASTLE652_RP_2_ITEM", 0.08f, 2, 1, "charog", "PagNanny", "PagWheeler", "Paggnat");

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new TakeItemReward("CASTLE652_RP_2_ITEM", -1));
	}
}
