//--- Melia Script ----------------------------------------------------------
// Delmore Hamlet Quest NPCs
//--- Description -----------------------------------------------------------
// Mage Melchioras and the Revelators from Klaipeda breaking the magic
// circle that powers Delmore Rephaim's Kruvina device.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
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

public class FCastle651QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(70400);
	private readonly static QuestId Mq02 = new QuestId(70401);
	private readonly static QuestId Mq03 = new QuestId(70402);
	private readonly static QuestId Mq04 = new QuestId(70403);
	private readonly static QuestId Mq05 = new QuestId(70404);
	private readonly static QuestId Mq06 = new QuestId(70405);
	private readonly static QuestId Sq01 = new QuestId(70406);
	private readonly static QuestId Sq02 = new QuestId(70407);
	private readonly static QuestId Sq03 = new QuestId(70408);
	private readonly static QuestId Sq04 = new QuestId(70409);

	private const string Mq05TrackId = "CASTLE65_1_MQ05_TRACK";

	public const string SpellsFoundVar = "Gabija.Quests.Castle651Mq02.Found";
	private const string SpellSpotVar = "Gabija.Quests.Castle651Mq02.Spot";
	private const string OrbChargeVar = "Gabija.Quests.Castle651Mq03.Charge";
	private const string OrbPlacedVar = "Gabija.Quests.Castle651Mq05.Orb";
	public const string PagDataVar = "Gabija.Quests.Castle651Sq01.Data";
	private const string PaperVar = "Gabija.Quests.Castle651Sq02.Paper";
	private const string PaperMissVar = "Gabija.Quests.Castle651Sq02.Misses";

	private const int SpellsNeeded = 3;
	private const int SpellRange = 250;
	private const int OrbCount = 3;
	private const int OrbChargePerKill = 25;
	private const int PagDataPerKill = 10;
	private const int PaperMissLimit = 2;

	private static readonly TimeSpan PaperRespawn = TimeSpan.FromSeconds(30);

	private static readonly string[] Pags = { "PagAmpullar", "PagSawyer", "Pagclamper" };

	private static readonly double[,] SpellSpots =
	{
		{ 981.60, -40.92, 90 }, { 324.36, -694.46, 90 }, { -1657.30, -1040.11, 43 }, { -1838.91, 1036.06, 90 }, { 995.59, 1521.70, 90 },
	};

	private static readonly int[] SpellSpotModels = { 152022, 152022, 152068, 152022, 152068 };

	private static readonly double[,] OrbSpots =
	{
		{ 38.77, 513.37, 0 }, { 61.42, 519.42, 45 }, { 66.16, 541.83, 90 },
	};

	private static readonly double[,] Papers =
	{
		{ 1184.25, -1213.08, 208 }, { 179.18, -1319.48, 90 }, { 120.67, -786.70, 197 }, { 451.14, -665.86, 34 },
		{ 723.34, -1352.34, 158 }, { 505.55, -1399.28, 332 }, { -141.32, -1466.56, 90 }, { 993.00, -1136.33, 191 },
		{ 512.05, -1602.90, 90 }, { 220.31, -673.61, 198 }, { 849.11, -1157.18, 141 }, { 1117.28, -1489.28, 27 },
	};

	private static readonly int[] DiaryPages = { ItemId.CASTLE65_1_SQ02_ITEM1, ItemId.CASTLE65_1_SQ02_ITEM2, ItemId.CASTLE65_1_SQ02_ITEM3 };

	protected override void Load()
	{
		// Mage Melchioras
		//-------------------------------------------------------------------------
		AddConditionalNpc(155113, L("Mage Melchioras"), "CASTLE651_MQ_01", "f_castle_65_1", 291.21, -266.92, 355, c => !c.Quests.Has(Mq06), this.Melchioras);

		// Revelator Mihail
		//-------------------------------------------------------------------------
		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE651_MQ_03", "f_castle_65_1", 353.97, -262.91, 13, c => !c.Quests.Has(Mq06) && !IsAtCrossroads(c), this.MihailAtCamp);
		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE651_MQ_05", "f_castle_65_1", 328.53, 196.95, 216, IsAtCrossroads, this.MihailAtCrossroads);

		// Follower Andrea
		//-------------------------------------------------------------------------
		AddNpc(152064, L("Follower Andrea"), "CASTLE651_SQ_01", "f_castle_65_1", 376.43, 15.27, 7, this.Andrea);

		// Follower Nuodas
		//-------------------------------------------------------------------------
		AddNpc(155034, L("Follower Nuodas"), "CASTLE651_SQ_03", "f_castle_65_1", 471.10, -132.29, 63, this.Nuodas);

		// The objects under the protection spell
		//-------------------------------------------------------------------------
		for (var i = 0; i < SpellSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(SpellSpotModels[i], "UnvisibleName", "CASTLE651_MQ_02_" + number, "f_castle_65_1", SpellSpots[i, 0], SpellSpots[i, 1], SpellSpots[i, 2],
				c => IsSpellFound(c, number) && !(number == 3 && c.Quests.Has(Mq04)));
		}

		AddConditionalNpc(152068, "UnvisibleName", "CASTLE651_MQ_04_3", "f_castle_65_1", -1657.30, -1040.11, 43, c => c.Quests.IsActive(Mq04) && !c.Quests.IsCompletable(Mq04), this.ProtectedObject);

		// The Magic Power Supply Device at the Ishinti Crossroads
		//-------------------------------------------------------------------------
		AddConditionalNpc(155105, L("Demonic Power Supply Device"), "CASTLE651_MQ_05_PILLAR", "f_castle_65_1", 46.02, 534.29, 90, c => !IsDeviceDestroyed(c));
		AddConditionalNpc(155116, L("Wreckage of the Demonic Power Supply Device"), "CASTLE651_MQ_05_PILLAR_B", "f_castle_65_1", 46.02, 534.29, 90, IsDeviceDestroyed);

		for (var i = 0; i < OrbSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(40095, L("Orb Spot"), "CASTLE651_MQ_05_" + number, "f_castle_65_1", OrbSpots[i, 0], OrbSpots[i, 1], 90,
				c => c.Quests.IsActive(Mq05) && !c.Quests.IsCompletable(Mq05) && !IsOrbPlaced(c, number),
				async dialog => await this.PlaceOrb(dialog, number));

			AddConditionalNpc(155107, L("Magic Power Bead Lump"), "CASTLE651_MQ_05_" + number + "_O", "f_castle_65_1", OrbSpots[i, 0], OrbSpots[i, 1], OrbSpots[i, 2],
				c => IsOrbPlaced(c, number) && c.Quests.IsActive(Mq05) && !IsDeviceDestroyed(c));
		}

		// Ardel's diary pages at the Old Bazaar Site
		//-------------------------------------------------------------------------
		for (var i = 0; i < Papers.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "CASTLE651_SQ_02" : "CASTLE651_SQ_02_" + number;

			AddConditionalNpc(153057, L("Fluttering Paper"), uniqueName, "f_castle_65_1", Papers[i, 0], Papers[i, 1], Papers[i, 2],
				c => c.Quests.IsActive(Sq02) && !c.Quests.IsCompletable(Sq02),
				async dialog => await this.SearchPaper(dialog, number));
		}
	}

	/// <summary>
	/// Mage Melchioras' dialog at the Revelators' camp.
	/// </summary>
	private async Task Melchioras(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mage Melchioras"));

		if (character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("You're a Revelator as well? Hmm... If that's true..."));
			await dialog.Msg(L("We've gathered here to stop Delmore Rephaim, the lord of Delmore Castle. All because he's trying to create another Kruvina."));
			await dialog.Msg(L("Oh, do you know about the Kruvina? The demon's plan to use the spores of death to invade Orsha..."));
			await dialog.Msg(L("The Kruvina crafted by Delmore Rephaim is the center piece of that plan... Which can also exhibit tremendous strength depending on how it is used."));
			await dialog.Msg(L("Delmore Rephaim was once a revered lord and established alchemist... But he touched upon forbidden alchemy by conspiring with the demons."));
			await dialog.Msg(L("That's how the pinnacle of demon magic and human technology was created. That creation is known as the Kruvina."));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (character.Quests.IsCompletable(Mq02))
		{
			await dialog.Msg(L("Above Amjene Sanctum and... Hmm. I see. Good job."));
			await dialog.Msg(L("To sustain such an enormous magic circle, Delmore Rephaim... He placed numerous Magic Power Supply Devices around the castle as well as at the Kruvina Device."));
			await dialog.Msg(L("Creating a large magic circle by oneself is simply impossible, no matter how talented you are."));
			await dialog.Msg(L("Oh, I see some introductions are in order. The person next to me is Revelator Mihail."));
			await dialog.Msg(L("I'm a Revelator just like you. How about talking to Mihail while I interpret the protection spells?"));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("There were many mages under the command of Delmore Rephaim just like me. We were all in charge of separate specializations so interpreting this will take a while."));
			await dialog.Msg(L("I was in charge of the core of the Kruvina device. I thought that there was something wrong..."));
			await dialog.Msg(L("You've done well. All we have to do is destroy the Magic Power Supply Device since the protective spells are gone."));
			await dialog.CompleteQuest(Mq04);
			return;
		}

		if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
		{
			await dialog.Msg(L("Are you planning on passing through Delmore Castle? If you do, I suggest turning around immediately."));
			await dialog.Msg(L("This is a place that has become a den of demons because of the lord of the castle. We should have discovered the true identity of the Kruvina earlier..."));

			var answer = await dialog.SelectQuestOffer(Mq01, L("Maybe if you were a Revelator... Anyway, I've warned you enough so you should return to where it's safe."),
				Option(L("I'm a Revelator"), "accept"),
				Option(L("I understand; I'm going somewhere safe"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq01);
				character.Quests.CompleteObjective(Mq01, "hearMelchioras");
			}
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			await dialog.Msg(L("I was once a mage under Delmore Rephaim as well. But I ran after seeing him create the Kruvina, which simply made me feel sick."));
			await dialog.Msg(L("I should have stopped its creation... But I was a coward back then. Delmore Rephaim is trying to build that Kruvina once more."));
			await dialog.Msg(L("Such terrible events... must not be allowed to happen again. That's why we've gathered the Revelators from Klaipeda and brought them here."));

			var answer = await dialog.SelectQuestOffer(Mq02, L("I'd say the more the merrier. How about lending a hand in stopping him since you're a Revelator as well?"),
				Option(L("Sure, I'll help"), "accept"),
				Option(L("It'll be difficult for me to take part in that"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(SpellsFoundVar, 0);
				for (var i = 1; i <= SpellSpots.GetLength(0); ++i)
					character.Variables.Perm.Set(SpellSpotVar + i, false);

				character.Quests.Start(Mq02);

				if (character.Inventory.CountItem(ItemId.CASTLE65_1_MQ02_ITEM) == 0)
					character.Inventory.Add(ItemId.CASTLE65_1_MQ02_ITEM, 1, InventoryAddType.PickUp);

				await dialog.Msg(L("It's a relief to have your cooperation. Oh, we were just about to destroy the Magic Power Supply Devices supplying the Kruvina Devices with power."));
				await dialog.Msg(L("The device is protected by a powerful protection spell... They've cast it on ordinary items to obscure the spell."));
				await dialog.Msg(L("I'll mark the places where I predict they may be, so take this compass with you. It's a compass that reacts to magic, so I'm sure it'll react to the spells."));
			}
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			var answer = await dialog.SelectQuestOffer(Mq04, L("I've finished interpreting the protection spell cast on the Magic Power Supply Devices. We'll need your help in order to get the job done in a hurry."),
				Option(L("What can I help you with?"), "accept"),
				Option(L("I'll help you next time"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);

				if (character.Inventory.CountItem(ItemId.CASTLE65_1_MQ04_ITEM) == 0)
					character.Inventory.Add(ItemId.CASTLE65_1_MQ04_ITEM, 1, InventoryAddType.PickUp);

				character.LookAround();

				await dialog.Msg(L("I'll give you this amulet. The protection spell will be disarmed when you put it on the enchanted object."));
				await dialog.Msg(L("You take care of the protection spell above Amjene Sanctum. Mihail and I will take care of the rest."));
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Release the protection spell above the Amjene Sanctum"), 5);
			}
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("I'm sure you've heard of the orbs from Mihail. Please set them around the Magic Power Supply Device at Ishinti Crossroads."));

			var answer = await dialog.SelectQuestOffer(Mq05, L("Please go to Mihail as soon as the orbs are in place. He'll detonate the orbs."),
				Option(L("I'll set down the orbs"), "accept"),
				Option(L("That sounds dangerous"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= OrbCount; ++i)
					character.Variables.Perm.Set(OrbPlacedVar + i, false);

				character.Quests.Start(Mq05);

				var orbs = OrbCount - character.Inventory.CountItem(ItemId.CASTLE65_1_MQ05_ITEM);
				if (orbs > 0)
					character.Inventory.Add(ItemId.CASTLE65_1_MQ05_ITEM, orbs, InventoryAddType.PickUp);

				character.LookAround();

				await dialog.Msg(L("It's strange how well everything seems to be going. I'm sure that Delmore Rephaim is creating the Kruvina somewhere..."));
			}
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			await dialog.Msg(L("It's a success! Both you and Mihail did a great job."));
			await dialog.Msg(L("The Kruvina device will soon stop since all of the Magic Power Supply Devices have been destroyed. The protective shield will disappear as well."));
			await dialog.Msg(L("We cannot simply relax because the device has stopped. We must destroy it so that they can never think of creating the Kruvina ever again."));

			var answer = await dialog.SelectQuestOffer(Mq06, L("Come to Delmore Manor where the device is. I'll be waiting there."),
				Option(L("Let's meet at Delmore Manor"), "accept"),
				Option(L("I can only help so much"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq06);
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("Delmore Rephaim had great ambitions. It would have been much better if that was merely a lust for power..."));
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("There were many mages under the command of Delmore Rephaim just like me. We were all in charge of separate specializations so interpreting this will take a while."));
			await dialog.Msg(L("I was in charge of the core of the Kruvina device. I thought that there was something wrong..."));
			return;
		}

		if (character.Quests.IsCompletable(Mq05))
		{
			await dialog.Msg(L("Please let Mihail know that the orbs are ready. I hope everything goes as planned..."));
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("I've tuned it so that the device will be destroyed with the smallest explosion possible... But run away from the orbs just in case you don't get caught in the explosion."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
		{
			await dialog.Msg(L("Are you planning on passing through Delmore Castle? If you do, I suggest turning around immediately."));
			await dialog.Msg(L("This is a place that has become a den of demons because of the lord of the castle. We have gathered here... because he is once more attempting to do the same horrible deeds."));
		}
		else
		{
			await dialog.Msg(L("Never... Never again shall we allow the same thing to happen. I am ashamed of myself... but if this can be any form of retribution..."));
		}
	}

	/// <summary>
	/// Revelator Mihail's dialog at the Revelators' camp.
	/// </summary>
	private async Task MihailAtCamp(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Mihail"));

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("That was quick. It seems as if Melchioras has finished his preparations as well."));
			await dialog.Msg(L("Why don't you go to Mage Melchioras? I know what's going on, but you might want some explanation seeing as you've just arrived here."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("Greetings. Have you come from Orsha? I thought there weren't any Revelators there... We've all come from Klaipeda."));
			await dialog.Msg(L("I've seen many places destroyed by the roots on Medzio Diena... But the scene here is a bit different."));
			await dialog.Msg(L("It seems as if they've all been ripped off the face of the world at the same time. Not just the residents, but even all the soldiers and vassals..."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("Mage Melchioras seems to be taking a while to disarm the protective spell. How about we get prepared for the next move while he's at it?"),
				Option(L("Okay"), "accept"),
				Option(L("Tell me about the Revelators of Klaipeda"), "explain"),
				Option(L("I'll wait for Melchioras to get ready"), "leave")
			);

			if (answer == "explain")
			{
				await dialog.Msg(L("One day, the goddess appeared in my dreams telling me that I am a Revelator and that I must go to Klaipeda. Saying that only Revelators can save the world..."));
				await dialog.Msg(L("At first, I was at a loss of what to do, and I was here before I knew it. Then, it came to me."));
				await dialog.Msg(L("The fact that... Everything I did was linked to saving the goddesses and the world. Perhaps the goddesses sent so many Revelators for that same reason?"));
				return;
			}

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(OrbChargeVar, 0);
				character.Quests.Start(Mq03);

				var orbs = OrbCount - character.Inventory.CountItem(ItemId.CASTLE65_1_MQ03_ITEM1) - character.Inventory.CountItem(ItemId.CASTLE65_1_MQ03_ITEM2);
				if (orbs > 0)
					character.Inventory.Add(ItemId.CASTLE65_1_MQ03_ITEM1, orbs, InventoryAddType.PickUp);

				await dialog.Msg(L("It seems that we get along well. We've destroyed all the other Magic Power Supply Devices... The only one left is the one that Mage Melchioras is working on."));
				await dialog.Msg(L("We have to destroy it with magic orbs when Mage Melchioras disarms the protection spell... But we don't have many left."));
				await dialog.Msg(L("I'll get the detonator ready, so why don't you fill the empty orbs with magic? The orb will gather magic every time you attack Pags."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("When I first arrived at Klaipeda... There were an incredible amount of Revelators gathered there."));
			await dialog.Msg(L("Those people, what do you think they're doing now?"));
			return;
		}

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("The rest of the protective spells have been taken care of. Now it's my turn."));
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("We won't have to come back here if we destroy the last Magic Power Supply Device. It won't take too long to stop the entire Kruvina Device either."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("I couldn't believe what Melchioras was saying at first. But just look at the castle. I realized the severity of the situation soon enough."));
		else
			await dialog.Msg(L("I don't know too much about magic, but this is still the largest magic circle I've seen. You don't know what I'm talking about do you? I mean that the entire castle is a magic circle."));
	}

	/// <summary>
	/// Revelator Mihail's dialog at the Ishinti Crossroads, where he
	/// detonates the orbs.
	/// </summary>
	private async Task MihailAtCrossroads(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Mihail"));

		if (character.Quests.IsCompletable(Mq05))
		{
			if (character.Etc.Properties.GetFloat(Mq05TrackId) != 1)
			{
				await dialog.Msg(L("The rest of the protective spells have been taken care of. Now it's my turn."));
				character.Quests.StartQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("That's the last of the Magic Power Supply Devices. Let's go back and tell Melchioras."));
			await dialog.CompleteQuest(Mq05);
			character.LookAround();
			return;
		}

		await dialog.Msg(L("We won't have to come back here if we destroy the last Magic Power Supply Device. It won't take too long to stop the entire Kruvina Device either."));
	}

	/// <summary>
	/// Follower Andrea's dialog.
	/// </summary>
	private async Task Andrea(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Follower Andrea"));

		if (character.Quests.IsCompletable(Sq01))
		{
			var told = await character.TimeActions.StartAsync(L("Passing on the information"), L("Cancel"), "TALK", TimeSpan.FromSeconds(1));
			if (told != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("You've been a big help. I'm going to have to tell the other Revelators about your deeds when I'm explaining to the others."));
			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (character.Quests.IsCompletable(Sq02))
		{
			await dialog.Msg(L("I don't know how to thank you. I'll remember to hand this back to you if we manage to get back."));
			await dialog.CompleteQuest(Sq02);
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("This place... is terrible. I've seen many ruins but, this place seems to be on a league of its own."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("Ah, I may not be a Revelator... But I volunteered to help. I'm trying to collect information on demons, but it seems as if I took it too lightly."),
				Option(L("I can help you; what kind of information do you need me to gather?"), "accept"),
				Option(L("I don't think I'm skilled enough for that"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(PagDataVar, 0);
				character.Quests.Start(Sq01);

				await dialog.Msg(L("Oh, would you be okay in doing so? If so, I won't say no."));
				await dialog.Msg(L("I've collected all the other information... but I need some about the Pag Ampler. Just tell me how you've dealt with them after defeating them."));
				await dialog.Msg(L("You could tell us now if you already know enough."));
			}
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("Before I came here, I was asked a favor from a person who said that they had relatives in Delmore Castle. They asked me to give their regards since they'd lost contact for a while."));
			await dialog.Msg(L("But... As you can see... Who knew this would have happened..."));

			var answer = await dialog.SelectQuestOffer(Sq02, L("I was trying to give him the remaining pages of the diary... But I wasn't able to make the time because things kept coming up."),
				Option(L("I'll take a look"), "accept"),
				Option(L("That's unfortunate, but it can't be helped"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(PaperMissVar, 0);
				character.Quests.Start(Sq02);
				character.LookAround();

				await dialog.Msg(L("Thank you so much. He says his name is Ardel and he lived at the Old Bazaar Site."));
			}
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			await dialog.Msg(L("We're trying to lessen the burden when the Revelators are dealing with demons. Please do your best to collect information even if it is a bit tiresome."));
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			await dialog.Msg(L("I've heard snippets of information on the Delmore Castle. But I don't know how it became like this. Melchioras keeps saving his breath..."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("I hoped it would be migration suggestions... But somehow I don't think it is. Where are all the soldiers and vassals, not to mention the residents?"));
		else
			await dialog.Msg(L("We're helping the Revelators from behind the scenes. But we never knew there would be so many demons..."));
	}

	/// <summary>
	/// Follower Nuodas' dialog.
	/// </summary>
	private async Task Nuodas(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Follower Nuodas"));

		if (character.Quests.IsCompletable(Sq03))
		{
			await dialog.Msg(L("I hope that this will comfort them a little. There are still many demons, but we will be able to defeat all of them one day."));
			await dialog.CompleteQuest(Sq03);
			return;
		}

		if (character.Quests.IsCompletable(Sq04))
		{
			await dialog.Msg(L("There are many followers besides me that are helping you. Please help them if you see them and don't just pass by."));
			await dialog.CompleteQuest(Sq04);
			return;
		}

		if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
		{
			await dialog.Msg(L("This was the next most thriving castle after Orsha. But the fact that such a place could become like this in an instant..."));
			await dialog.Msg(L("It's unfortunate what happened to the missing people, but there is nothing we can do. All we can do is to keep getting rid of those accursed demons."));

			var answer = await dialog.SelectQuestOffer(Sq03, L("How about helping out? I think that's one way of comforting the people that used to live here."),
				Option(L("I'll help you"), "accept"),
				Option(L("I'm busy with other things"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq03);

				await dialog.Msg(L("Thank you. Could you deal with the demons nearby please?"));
			}
			return;
		}

		if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
		{
			await dialog.Msg(L("I felt as if the Revelator and the followers pulled their weight. Their archery skill was quite impressive."));
			await dialog.Msg(L("But as a fletcher, I think that they waste too many arrows... They don't think of aiming for the weak spots and just pour arrows into a single demon."));

			var answer = await dialog.SelectQuestOffer(Sq04, L("There are more than a few Charcoal Walkers wondering about with usable arrows. Could you gather those arrows after dealing with those demons?"),
				Option(L("I'll do it while I hunt some monsters"), "accept"),
				Option(L("Give up everything that's already been used"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq04);

				await dialog.Msg(L("The more the world is in chaos, the more thrifty you should be. Please, I ask of you."));
			}
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("I feel terrible. Only a year ago, it was still pretty intact despite Medzio Diena."));
			await dialog.Msg(L("But now... Maybe where we live isn't safe anymore either."));
			return;
		}

		if (character.Quests.IsActive(Sq04))
		{
			await dialog.Msg(L("The more the world is in chaos, the more thrifty you should be. Please, I ask of you."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("What do you have to do to make that many people disappear? It is creepy how all the household belongings and other things are left as they are."));
		else
			await dialog.Msg(L("I've heard many stories of losing hometowns because of monsters or demons after Medzio Diena. But this is different. It seems as if something was ripped out of the castle whole..."));
	}

	/// <summary>
	/// The object above the Amjene Sanctum, released with Melchioras'
	/// amulet.
	/// </summary>
	private async Task ProtectedObject(Dialog dialog)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Mq04) || character.Quests.IsCompletable(Mq04))
			return;

		if (character.Inventory.CountItem(ItemId.CASTLE65_1_MQ04_ITEM) == 0)
		{
			character.ServerMessage(L("You need Melchioras' amulet to release the protection spell."));
			return;
		}

		dialog.Npc.PlayEffect("F_light018_yellow", 1f);
		character.Quests.CompleteObjective(Mq04, "releaseSpell");
		character.ServerMessage(L("The protection spell has been released."));
		character.LookAround();

		await Task.CompletedTask;
	}

	/// <summary>
	/// Places one of the Magic Concentration Orbs around the Magic Power
	/// Supply Device.
	/// </summary>
	private async Task PlaceOrb(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Mq05) || character.Quests.IsCompletable(Mq05) || IsOrbPlaced(character, number))
			return;

		if (character.Inventory.CountItem(ItemId.CASTLE65_1_MQ05_ITEM) == 0)
		{
			character.ServerMessage(L("You have no Magic Concentration Orbs left."));
			return;
		}

		character.Inventory.Remove(ItemId.CASTLE65_1_MQ05_ITEM, 1, InventoryItemRemoveMsg.Given);
		character.Variables.Perm.Set(OrbPlacedVar + number, true);
		character.Quests.CompleteObjective(Mq05, "placeOrb" + number);
		character.LookAround();

		await Task.CompletedTask;
	}

	/// <summary>
	/// Searches one of the papers fluttering around the Old Bazaar Site
	/// for a page of Ardel's diary.
	/// </summary>
	private async Task SearchPaper(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02))
			return;

		var searchedAt = character.Variables.Temp.GetLong(PaperVar + number, 0);
		if (searchedAt != 0 && DateTime.Now - new DateTime(searchedAt) < PaperRespawn)
		{
			character.ServerMessage(L("You have already searched this paper."));
			return;
		}

		character.Variables.Temp.SetLong(PaperVar + number, DateTime.Now.Ticks);

		var page = DiaryPages.FirstOrDefault(itemId => character.Inventory.CountItem(itemId) == 0);
		if (page == 0)
			return;

		var misses = character.Variables.Perm.GetInt(PaperMissVar, 0);
		if (misses < PaperMissLimit && GameRandom.Get().NextDouble() >= 0.35)
		{
			character.Variables.Perm.SetInt(PaperMissVar, misses + 1);
			character.ServerMessage(L("It's just a scrap of paper. Ardel's diary must be somewhere else."));
			return;
		}

		character.Variables.Perm.SetInt(PaperMissVar, 0);
		character.Inventory.Add(page, 1, InventoryAddType.PickUp);

		await Task.CompletedTask;
	}

	/// <summary>
	/// Points Melchioras' compass around, revealing an object under the
	/// protection spell when one is close by.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CASTLE65_1_MQ02_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_castle_65_1" || !character.Quests.IsActive(Mq02) || character.Quests.IsCompletable(Mq02))
		{
			character.ServerMessage(L("The compass does not react."));
			return ItemUseResult.OkayNotConsumed;
		}

		for (var i = 0; i < SpellSpots.GetLength(0); ++i)
		{
			var number = i + 1;
			if (IsSpellFound(character, number))
				continue;

			if (character.Position.Get2DDistance(new Position((float)SpellSpots[i, 0], character.Position.Y, (float)SpellSpots[i, 1])) > SpellRange)
				continue;

			character.PlayEffect("F_light018_yellow", 1f);
			character.Variables.Perm.Set(SpellSpotVar + number, true);

			var found = character.Variables.Perm.GetInt(SpellsFoundVar, 0) + 1;
			character.Variables.Perm.SetInt(SpellsFoundVar, found);
			character.ServerMessage(LF("The compass points at an object under the protection spell: {0}/{1}", Math.Min(found, SpellsNeeded), SpellsNeeded));
			character.LookAround();

			return ItemUseResult.OkayNotConsumed;
		}

		character.ServerMessage(L("The compass needle spins aimlessly. There is no protection spell nearby."));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Reads the first page of Ardel's diary.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CASTLE65_1_SQ02_ITEM1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		Send.ZC_NORMAL.ShowBook(character, "CASTLE65_1_SQ02_PAGE_01");
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Reads the second page of Ardel's diary.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CASTLE65_1_SQ02_ITEM2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		Send.ZC_NORMAL.ShowBook(character, "CASTLE65_1_SQ02_PAGE_02");
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Reads the third page of Ardel's diary.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CASTLE65_1_SQ02_ITEM3(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		Send.ZC_NORMAL.ShowBook(character, "CASTLE65_1_SQ02_PAGE_03");
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Charges the Magic Concentration Orbs and gathers Andrea's data
	/// while the player fights the Pags of Delmore Hamlet.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Attacker is not Character character || args.Target is not Mob mob)
			return;

		if (character.Map.ClassName != "f_castle_65_1" || !Pags.Contains(mob.Data.ClassName))
			return;

		if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03) && character.Inventory.CountItem(ItemId.CASTLE65_1_MQ03_ITEM1) > 0)
		{
			var charge = character.Variables.Perm.GetInt(OrbChargeVar, 0) + OrbChargePerKill;

			if (charge >= 100)
			{
				charge = 0;
				character.Inventory.Remove(ItemId.CASTLE65_1_MQ03_ITEM1, 1, InventoryItemRemoveMsg.Given);
				character.Inventory.Add(ItemId.CASTLE65_1_MQ03_ITEM2, 1, InventoryAddType.PickUp);
			}
			else
			{
				character.ServerMessage(LF("Magic Concentration Orb charged: {0}%", charge));
			}

			character.Variables.Perm.SetInt(OrbChargeVar, charge);
		}

		if (mob.Data.ClassName == "PagAmpullar" && character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
		{
			var data = Math.Min(100, character.Variables.Perm.GetInt(PagDataVar, 0) + PagDataPerKill);
			character.Variables.Perm.SetInt(PagDataVar, data);
		}
	}

	/// <summary>
	/// Returns whether Mihail is waiting at the Ishinti Crossroads to
	/// detonate the orbs.
	/// </summary>
	private static bool IsAtCrossroads(Character character)
		=> character.Quests.IsActive(Mq05);

	/// <summary>
	/// Returns whether the compass has found the given object.
	/// </summary>
	private static bool IsSpellFound(Character character, int number)
		=> character.Variables.Perm.GetBool(SpellSpotVar + number, false) && character.Quests.Has(Mq02);

	/// <summary>
	/// Returns whether the given orb is in place around the device.
	/// </summary>
	private static bool IsOrbPlaced(Character character, int number)
		=> character.Variables.Perm.GetBool(OrbPlacedVar + number, false);

	/// <summary>
	/// Returns whether the Magic Power Supply Device has been blown up.
	/// </summary>
	private static bool IsDeviceDestroyed(Character character)
		=> character.Quests.HasCompleted(Mq05) || (character.Quests.IsActive(Mq05) && character.Etc.Properties.GetFloat(Mq05TrackId) == 1);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 70400: A New Plot
//-----------------------------------------------------------------------------
public class FCastle651Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70400);
		SetName(L("A New Plot"));
		SetDescription(L("After hearing that you're a Revelator, Mage Melchioras seems to want to tell you something. Listen to what he has to say."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_MQ_01", "f_castle_65_1", L("Talk to Mage Melchioras"), L("Mage Melchioras seems to have something to say to you. Talk to Mage Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_MQ_01", "f_castle_65_1", L("Talk to Mage Melchioras"), L("After hearing that you're a Revelator, Mage Melchioras seems to want to tell you something. Listen to what he has to say."));
		SetPhase(QuestStatus.Success, "CASTLE651_MQ_01", "f_castle_65_1", L("Talk to Mage Melchioras"), L("After hearing that you're a Revelator, Mage Melchioras seems to want to tell you something. Listen to what he has to say."));

		AddPrerequisite(new QuestStatusPrerequisite(90015, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(66));

		AddObjective("hearMelchioras", L("Talk to Mage Melchioras"), new ManualObjective());
	}
}

// 70401: Investigating the Protection Cast
//-----------------------------------------------------------------------------
public class FCastle651Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70401);
		SetName(L("Investigating the Protection Cast"));
		SetDescription(L("You have decided to help Melchioras. Visit the spots marked on the map and use the compass to find the objects under the protection spell. The compass is activated only near the cast."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_MQ_01", "f_castle_65_1", L("Talk to Mage Melchioras"), L("Mage Melchioras says he has returned to this place to gather the Revelators and stop another Kruvina from being created in Delmore Castle. Keep listening to Mage Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_MQ_01", "f_castle_65_1", L("Use the compass to search for hidden artifacts"), L("You have decided to help Melchioras. Visit the spots marked on the map and use the compass to find the objects under the protection spell. The compass is activated only near the cast."));
		SetPhase(QuestStatus.Success, "CASTLE651_MQ_01", "f_castle_65_1", L("Report to Mage Melchioras"), L("You have found the objects under the protection spell. Relay their location to Mage Melchioras."));

		AddPrerequisite(new QuestStatusPrerequisite(70400, QuestStatus.Completed));

		AddObjective("findSpells", L("Search for objects casting the protection spell"), new VariableCheckObjective(FCastle651QuestNpcsScript.SpellsFoundVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 950));
		AddReward(new TakeItemReward("CASTLE65_1_MQ02_ITEM", -1));
	}
}

// 70402: All For a Bigger Blow
//-----------------------------------------------------------------------------
public class FCastle651Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70402);
		SetName(L("All For a Bigger Blow"));
		SetDescription(L("According to Mihail, they need more magic concentration orbs to blast the Magic Power Supply Device. Defeat demons to charge the orbs."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_MQ_03", "f_castle_65_1", L("Talk to Revelator Mihail"), L("Melchioras is going to analyze the protection spell; meanwhile, talk to Revelator Mihail."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_MQ_03", "f_castle_65_1", L("Defeat demons and charge the Magic Concentration Orbs"), L("According to Mihail, they need more magic concentration orbs to blast the Magic Power Supply Device. Defeat demons to charge the orbs."));
		SetPhase(QuestStatus.Success, "CASTLE651_MQ_03", "f_castle_65_1", L("Deliver to Revelator Mihail"), L("You have filled all orbs with magic. Bring them to Revelator Mihail."));

		AddPrerequisite(new QuestStatusPrerequisite(70401, QuestStatus.Completed));

		AddObjective("chargeOrbs", L("Defeat demons and charge the Magic Concentration Orbs"), new CollectItemObjective("CASTLE65_1_MQ03_ITEM2", 3));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 710));
		AddReward(new SelectItemReward("SWD02_126", "TSW02_122", "MAC02_123", "SPR02_118", "TSP02_113"));
		AddReward(new TakeItemReward("CASTLE65_1_MQ03_ITEM1", -1));
		AddReward(new TakeItemReward("CASTLE65_1_MQ03_ITEM2", -1));
	}
}

// 70403: Starting in the Area
//-----------------------------------------------------------------------------
public class FCastle651Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70403);
		SetName(L("Starting in the Area"));
		SetDescription(L("Mage Melchioras has asked you to dispel the protection spell placed north of the Amjene Sanctum. Use the amulet on the objects protected by the cast to release it."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_MQ_01", "f_castle_65_1", L("Talk to Mage Melchioras"), L("Mage Melchioras' analysis of the protection spell seems to have come to an end. Talk to Mage Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_MQ_04_3", "f_castle_65_1", L("Release the objects' protection spell"), L("Mage Melchioras has asked you to dispel the protection spell placed north of the Amjene Sanctum. Use the amulet on the objects protected by the cast to release it."));
		SetPhase(QuestStatus.Success, "CASTLE651_MQ_01", "f_castle_65_1", L("Report to Mage Melchioras"), L("You have dispelled the protection spell. Return to Mage Melchioras."));

		AddPrerequisite(new QuestStatusPrerequisite(70402, QuestStatus.Completed));

		AddObjective("releaseSpell", L("Release the protection spell on the object"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 480));
		AddReward(new SelectItemReward("STF02_121", "TSF02_121", "TBW02_124", "BOW02_120"));
		AddReward(new TakeItemReward("CASTLE65_1_MQ04_ITEM", -1));
	}
}

// 70404: Destroy the Magic Power Supply Device
//-----------------------------------------------------------------------------
public class FCastle651Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70404);
		SetName(L("Destroy the Magic Power Supply Device"));
		SetDescription(L("It's time to blow up the Magic Power Supply Device. Go to the Ishinti Crossroads and place the charged orbs around the device."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_MQ_01", "f_castle_65_1", L("Talk to Mage Melchioras"), L("All preparations for blowing up the Magic Power Supply Device are now in place. Talk to Mage Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_MQ_05_1", "f_castle_65_1", L("Place the Magic Concentration Orbs on the Magic Power Supply Device"), L("It's time to blow up the Magic Power Supply Device. Go to the Ishinti Crossroads and place the charged orbs around the device."));
		SetPhase(QuestStatus.Success, "CASTLE651_MQ_05", "f_castle_65_1", L("Report to Revelator Mihail"), L("All the charged orbs are now in place. Talk to Revelator Mihail and blow up the Magic Power Supply Device."));

		SetTrack(QuestStatus.Success, QuestStatus.Success, "CASTLE65_1_MQ05_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(70403, QuestStatus.Completed));

		AddObjective("placeOrb1", L("Place an orb on the left of the device"), new ManualObjective());
		AddObjective("placeOrb2", L("Place an orb in front of the device"), new ManualObjective());
		AddObjective("placeOrb3", L("Place an orb on the right of the device"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 710));
		AddReward(new TakeItemReward("CASTLE65_1_MQ05_ITEM", -1));
	}
}

// 70405: Preparations Complete
//-----------------------------------------------------------------------------
public class FCastle651Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70405);
		SetName(L("Preparations Complete"));
		SetDescription(L("Melchioras wants to meet up at Delmore Manor, where the Kruvina device is located. Join Mage Melchioras at Delmore Manor."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_1", "f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_MQ_01", "f_castle_65_1", L("Talk to Mage Melchioras"), L("You have destroyed the Magic Power Supply Device. Talk to Mage Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_MQ_01_TRIGGER", "f_castle_65_2", L("Go to Delmore Manor"), L("Melchioras wants to meet up at Delmore Manor, where the Kruvina device is located. Join Mage Melchioras at Delmore Manor."));
		SetPhase(QuestStatus.Success, "CASTLE652_MQ_01_TRIGGER", "f_castle_65_2", L("Go to Delmore Manor"), L("Melchioras wants to meet up at Delmore Manor, where the Kruvina device is located. Join Mage Melchioras at Delmore Manor."));

		AddPrerequisite(new QuestStatusPrerequisite(70404, QuestStatus.Completed));

		AddObjective("goToManor", L("Go to Delmore Manor"), new ManualObjective());
	}
}

// 70406: Evidence in Ruins
//-----------------------------------------------------------------------------
public class FCastle651Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70406);
		SetName(L("Evidence in Ruins"));
		SetDescription(L("Follower Andrea is looking to obtain information on Pag Amplers. Defeat Pag Amplers and collect any data you can from them."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_SQ_01", "f_castle_65_1", L("Talk to Follower Andrea"), L("Follower Andrea is preparing to deal with the monsters in the area. Ask her if there is anything you can help with."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_SQ_01", "f_castle_65_1", L("Collect data from Pag Amplers"), L("Follower Andrea is looking to obtain information on Pag Amplers. Defeat Pag Amplers and collect any data you can from them."));
		SetPhase(QuestStatus.Success, "CASTLE651_SQ_01", "f_castle_65_1", L("Talk to Follower Andrea"), L("You have collected enough data. Bring it to Follower Andrea."));

		AddPrerequisite(new LevelPrerequisite(66));

		AddObjective("collectData", L("Collect data by fighting Pag Amplers"), new VariableCheckObjective(FCastle651QuestNpcsScript.PagDataVar, 100, isPermanent: true));

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 950));
		AddReward(new ItemReward("Drug_SP2_Q", 30));
	}
}

// 70407: For Those Who Remember Them
//-----------------------------------------------------------------------------
public class FCastle651Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70407);
		SetName(L("For Those Who Remember Them"));
		SetDescription(L("Follower Andrea was asked to contact a resident of Delmore Castle named Ardel, but all Delmore Castle residents are missing. Try and recover Ardel's diary."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_SQ_01", "f_castle_65_1", L("Talk to Follower Andrea"), L("It looks like Follower Andrea needs your help. Talk to Follower Andrea."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_SQ_02", "f_castle_65_1", L("Collect the diary of Delmore Castle resident Ardel"), L("Follower Andrea was asked to contact a resident of Delmore Castle named Ardel, but all Delmore Castle residents are missing. Try and recover Ardel's diary."));
		SetPhase(QuestStatus.Success, "CASTLE651_SQ_01", "f_castle_65_1", L("Deliver to Follower Andrea"), L("You have collected all the pages of the diary. Bring them to Follower Andrea."));

		AddPrerequisite(new LevelPrerequisite(66));

		AddObjective("findPage1", L("Ardel's Diary, Page 1"), new CollectItemObjective("CASTLE65_1_SQ02_ITEM1", 1));
		AddObjective("findPage2", L("Ardel's Diary, Page 2"), new CollectItemObjective("CASTLE65_1_SQ02_ITEM2", 1));
		AddObjective("findPage3", L("Ardel's Diary, Page 3"), new CollectItemObjective("CASTLE65_1_SQ02_ITEM3", 1));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 480));
		AddReward(new TakeItemReward("CASTLE65_1_SQ02_ITEM1", -1));
		AddReward(new TakeItemReward("CASTLE65_1_SQ02_ITEM2", -1));
		AddReward(new TakeItemReward("CASTLE65_1_SQ02_ITEM3", -1));
	}
}

// 70408: Be Considerate to the Next Person
//-----------------------------------------------------------------------------
public class FCastle651Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70408);
		SetName(L("Be Considerate to the Next Person"));
		SetDescription(L("Follower Nuodas believes that clearing out some demons will probably help the residents of Delmore Castle. Defeat nearby demons."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_SQ_03", "f_castle_65_1", L("Talk to Follower Nuodas"), L("Follower Nuodas is feeling sorry for the missing residents of Delmore Castle. Talk to Follower Nuodas."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_SQ_03", "f_castle_65_1", L("Defeat nearby demons"), L("Follower Nuodas believes that clearing out some demons will probably help the residents of Delmore Castle. Defeat nearby demons."));
		SetPhase(QuestStatus.Success, "CASTLE651_SQ_03", "f_castle_65_1", L("Talk to Follower Nuodas"), L("You have defeated enough demons. Go and tell Follower Nuodas."));

		AddPrerequisite(new LevelPrerequisite(66));

		AddObjective("killDemons", L("Defeat nearby demons"), new KillObjective(20, "PagAmpullar", "PagSawyer", "Pagclamper"));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 710));
	}
}

// 70409: Reutilizing Resources
//-----------------------------------------------------------------------------
public class FCastle651Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70409);
		SetName(L("Reutilizing Resources"));
		SetDescription(L("Follower Nuodas thinks the Revelators waste too many arrows. Defeat Charcoal Walkers and collect any arrows that can still be used."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE651_SQ_03", "f_castle_65_1", L("Talk to Follower Nuodas"), L("It seems Follower Nuodas has a favor to ask of you. Talk to Follower Nuodas."));
		SetPhase(QuestStatus.InProgress, "CASTLE651_SQ_03", "f_castle_65_1", L("Collect arrows from Charcoal Walkers"), L("Follower Nuodas thinks the Revelators waste too many arrows. Defeat Charcoal Walkers and collect any arrows that can still be used."));
		SetPhase(QuestStatus.Success, "CASTLE651_SQ_03", "f_castle_65_1", L("Deliver to Follower Nuodas"), L("As Follower Nuodas said, there were plenty of useful arrows to be collected. Bring them to Follower Nuodas."));

		AddPrerequisite(new LevelPrerequisite(66));

		AddObjective("collectArrows", L("Collect Useful Arrows by defeating Charcoal Walkers"), new CollectItemObjective("CASTLE65_1_SQ04_ITEM", 16));
		AddPityDrop("CASTLE65_1_SQ04_ITEM", 0.75f, 3, 1, "charcoal_walker");

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 710));
		AddReward(new TakeItemReward("CASTLE65_1_SQ04_ITEM", -1));
	}
}
