//--- Melia Script ----------------------------------------------------------
// Seir Rainforest Quest NPCs
//--- Description -----------------------------------------------------------
// Rescuing Goddess Lada from Demon Lord Zaura, tearing down the devices
// that feed her life force into the Kruvina, and the villagers' efforts to
// rebuild her temple.
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

public class FOrchard324QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Rp1 = new QuestId(60185);
	private readonly static QuestId Mq01 = new QuestId(80041);
	private readonly static QuestId Mq02 = new QuestId(80042);
	private readonly static QuestId Mq03 = new QuestId(80043);
	private readonly static QuestId Mq04 = new QuestId(80044);
	private readonly static QuestId Mq05 = new QuestId(80045);
	private readonly static QuestId Mq06 = new QuestId(80046);
	private readonly static QuestId Mq07 = new QuestId(80047);
	private readonly static QuestId Sq01 = new QuestId(80048);
	private readonly static QuestId Sq02 = new QuestId(80049);
	private readonly static QuestId Sq03 = new QuestId(80050);
	private readonly static QuestId Sq04 = new QuestId(80051);

	private const string AfterTrackId = "ORCHARD_324_MQ_01_AFTER";

	private const string WardVar = "Gabija.Quests.Orchard324Mq03.Ward";
	private const string TotemVar = "Gabija.Quests.Orchard324Sq03.Totem";
	private const string SaplingVar = "Gabija.Quests.Orchard324Rp1.Sapling";

	private const int SaplingsNeeded = 10;
	private const int SuppressorRange = 150;
	private const int TotemRange = 150;
	private const int FlowerRange = 250;

	private static readonly TimeSpan GatherRespawn = TimeSpan.FromSeconds(30);

	private static readonly Position SuppressorSpot = new Position(-859.62f, 568.08f, 1008.93f);
	private static readonly Position FlowerSpot = new Position(-1607f, 612f, 898f);

	private static readonly double[,] Wards =
	{
		{ -47.13, 813.74 }, { -91.46, 897.53 }, { 1.95, 897.53 },
	};

	private static readonly double[,] Totems =
	{
		{ 1344.36, 59.78 }, { 883.55, 138.42 }, { 438.58, 141.07 }, { 1021.19, -870.69 },
	};

	private static readonly double[,] Saplings =
	{
		{ -1691.65, -824.68 }, { -1672.33, -931.89 }, { -1550.84, -915.45 }, { -1434.94, -1034.62 }, { -1318.36, -914.57 }, { -1398.96, -847.67 },
		{ -1561.19, -718.21 }, { -1385.85, -570.36 }, { -671.06, -774.91 }, { -519.42, -831.62 }, { -380.89, -714.18 }, { -415.63, -564.55 },
		{ -612.69, -579.25 }, { -744.28, -206.07 }, { -1067.98, -160.38 }, { -1040.94, -416.77 }, { -1119.13, -555.98 },
	};

	protected override void Load()
	{
		// Goddess Lada
		//-------------------------------------------------------------------------
		AddConditionalNpc(156043, L("Goddess Lada"), "ORCHARD324_LADA", "f_orchard_32_4", -46.79, 870.94, 0, c => !c.Quests.Has(Mq07), this.Lada);
		AddConditionalNpc(156043, L("Goddess Lada"), "ORCHARD324_LADA2", "f_orchard_32_4", 437.26, 1013.55, 0, c => c.Quests.Has(Mq07), this.LadaAtTemple);

		AddQuestTrigger("ORCHARD_324_MQ_01_TRIG", "f_orchard_32_4", -46.38, 868.26, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
				character.Quests.Start(Mq01);

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
				character.Quests.StartQuestTrack(Mq01);

			await Task.CompletedTask;
		});

		// The Redemption Wards around the goddess
		//-------------------------------------------------------------------------
		for (var i = 0; i < Wards.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(155024, L("Redemption Ward"), "ORCHARD324_BINDIG" + number, "f_orchard_32_4", Wards[i, 0], Wards[i, 1], 90,
				c => !c.Quests.HasCompleted(Mq03) && !IsWardDestroyed(c, number),
				async dialog =>
				{
					var character = dialog.Player;
					if (!character.Quests.IsActive(Mq03) || character.Quests.IsCompletable(Mq03) || IsWardDestroyed(character, number))
						return;

					dialog.Npc.PlayEffect("F_explosion049_fire", 1f, 1, EffectLocation.Bottom);
					character.Variables.Perm.Set(WardVar + number, true);
					character.Quests.CompleteObjective(Mq03, "destroyWard" + number);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The devices feeding the Kruvina
		//-------------------------------------------------------------------------
		AddConditionalNpc(156039, L("Vitality Absorption Device"), "ORCHARD324_DRAIN", "f_orchard_32_4", -287.42, 1417.67, 90, c => !c.Quests.HasCompleted(Mq04), this.Drain);

		AddConditionalNpc(155105, L("Kruvina Suppressor"), "ORCHARD324_DESPENSOR", "f_orchard_32_4", SuppressorSpot.X, SuppressorSpot.Z, 90, c => !c.Quests.HasCompleted(Mq05) && !c.Quests.IsCompletable(Mq05), async dialog =>
		{
			OverloadSuppressor(dialog.Player);
			await Task.CompletedTask;
		});

		AddConditionalNpc(153118, L("Incomplete Kruvina"), "ORCHARD324_KRUVINA", "f_orchard_32_4", -1704.19, 887.56, 45, c => !c.Quests.HasCompleted(Mq06) && !c.Quests.IsCompletable(Mq06), async dialog =>
		{
			var character = dialog.Player;
			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
				character.Quests.StartQuestTrack(Mq06);

			await Task.CompletedTask;
		});

		AddQuestTrigger("ORCHARD324_KRUVINA_TRIGGER", "f_orchard_32_4", -1704.19, 887.56, 120, async args =>
		{
			if (args.Initiator is Character character && character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
				character.Quests.StartQuestTrack(Mq06);

			await Task.CompletedTask;
		});

		// The villagers who follow the goddess to Seir
		//-------------------------------------------------------------------------
		AddConditionalNpc(147407, L("Village Priest"), "ORCHARD324_PRIEST", "f_orchard_32_4", 332.07, 889.06, 90, c => c.Quests.Has(Mq07), this.Priest);
		AddConditionalNpc(147408, L("Great Priest of the Village"), "ORCHARD324_FUZE", "f_orchard_32_4", 336.01, 956.43, 90, c => c.Quests.Has(Mq07), this.GreatPriest);
		AddConditionalNpc(147418, L("Believer Dreka"), "ORCHARD324_RP_1_NPC", "f_orchard_32_4", -1031.60, -152.09, 22, c => c.Quests.Has(Mq07), this.Dreka);

		// The demon totems at the Inega Vacant Lot and the Odur Backyard
		//-------------------------------------------------------------------------
		for (var i = 0; i < Totems.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47150, L("Demon Totem"), "ORCHARD324_EVIL" + number, "f_orchard_32_4", Totems[i, 0], Totems[i, 1], 90,
				c => !c.Quests.HasCompleted(Sq03) && !IsTotemPurified(c, number),
				async dialog =>
				{
					PurifyTotem(dialog.Player, number);
					await Task.CompletedTask;
				});
		}

		// Ceyral Saplings near the Shulti Workshop
		//-------------------------------------------------------------------------
		for (var i = 0; i < Saplings.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "ORCHARD324_RP_1_OBJ" : "ORCHARD324_RP_1_OBJ_" + number;

			AddConditionalNpc(47200, L("Ceyral Saplings"), uniqueName, "f_orchard_32_4", Saplings[i, 0], Saplings[i, 1], 90,
				c => c.Quests.IsActive(Rp1) && !c.Quests.IsCompletable(Rp1),
				async dialog =>
				{
					var character = dialog.Player;
					if (!character.Quests.IsActive(Rp1) || character.Inventory.CountItem(ItemId.ORCHARD324_RP_1_ITEM) >= SaplingsNeeded)
						return;

					if (!Gather(character, SaplingVar + number, L("The Ceyral Saplings here have already been gathered.")))
						return;

					character.Inventory.Add(ItemId.ORCHARD324_RP_1_ITEM, 1, InventoryAddType.PickUp);
					character.ServerMessage(L("You have found Ceyral saplings."));

					await Task.CompletedTask;
				});
		}
	}

	/// <summary>
	/// Goddess Lada's dialog where the demons held her captive.
	/// </summary>
	private async Task Lada(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Goddess Lada"));
		dialog.SetPortrait("Dlg_port_GoddessLada");

		if (character.Quests.IsCompletable(Mq01))
		{
			if (character.Etc.Properties.GetFloat(AfterTrackId) != 1)
			{
				_ = character.Tracks.Start(AfterTrackId, TimeSpan.FromMilliseconds(500));
				return;
			}

			await dialog.Msg(L("You... You've finally saved me."));
			await dialog.Msg(L("When Laima entrusted this forest to me... She said it would be consumed by a wave of calamity."));
			await dialog.Msg(L("And, she said, after the wave has washed away, I would meet you. The one holding the light of salvation..."));
			await dialog.Msg(L("You are the one, the Revelator who will save our world... I have hope that it will be you now."));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (character.Quests.IsCompletable(Mq02))
		{
			await dialog.Msg(L("The Kruvina may be unfinished... But the fact that it uses my life force means it cannot be destroyed by human powers."));
			await dialog.Msg(L("There is only one way... I need to impart my power on the orb Laima gave you..."));
			await dialog.Msg(L("Alas, most of my power was stolen from me during Medzio Diena, and what was left of it is with the Kruvina device."));
			await dialog.Msg(L("If only that power is released... I can use it to help you."));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("You... destroyed the Redemption Ward. I'm fine. I'm just a little dizzy."));
			await dialog.CompleteQuest(Mq03);
			character.LookAround();
			return;
		}

		if (character.Quests.IsCompletable(Mq05))
		{
			await dialog.Msg(L("You did well. With the production stopped, the Kruvina cannot be completed."));
			await dialog.CompleteQuest(Mq05);
			return;
		}

		if (character.Quests.IsCompletable(Mq06))
		{
			await dialog.Msg(L("The Kruvina... is in the hands of the demons. I'm glad you, savior, were able to return in safety."));
			await dialog.Msg(L("Don't blame yourself. This is not your fault. If you hadn't stopped the demons' plans, they would have completed the Kruvina."));
			await dialog.Msg(L("I believe Laima will be aware of this, too. That feat alone was immensely important for us. No one can blame you..."));
			await dialog.CompleteQuest(Mq06);
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			await dialog.Msg(L("The reason behind the red water flowing in this forest and the contaminated crops... The plan to grow a giant bracken and spread the death spores..."));
			await dialog.Msg(L("All of that was possible because of the Kruvina. But their schemes didn't end there."));
			await dialog.Msg(L("After the experiment yielded good results, the demons wanted to create a Kruvina that was even more powerful. Not the lives of people... Hers. The goddess' life."));
			await dialog.Msg(L("That's why such a large amount of water contaminated by the Kruvina was needed. There cannot be a weapon like the Kruvina in our world."));

			var answer = await dialog.SelectQuestOffer(Mq02, L("Only you can destroy the Kruvina and stop the demons' plans. Demon Lord Zaura is gravely injured; now is your chance!"),
				Option(L("What should I do?"), "accept"),
				Option(L("Let me organize my thoughts"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq02);
				character.Quests.CompleteObjective(Mq02, "listenToLada");
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("I... feel my conscience fading away... Hurry and destroy the three devices... before the Kruvina is completed."));
			await dialog.Msg(L("Fortunately, should I say... those devices were made by a human... Your abilities should be enough to destroy them."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("The Redemption Ward first... Please, before I lose my senses..."),
				Option(L("I'll try and destroy it"), "accept"),
				Option(L("It looks hard"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= Wards.GetLength(0); ++i)
					character.Variables.Perm.Set(WardVar + i, false);

				character.Quests.Start(Mq03);
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			await dialog.Msg(L("Now only the vitality absorption device and the suppressor are left. If you destroy the vitality absorption device, you'll stop the production of the Kruvina."));
			await dialog.Msg(L("The device was made to absorb only the vitality of the goddesses... If it absorbs that of another being it will shut down."));

			var answer = await dialog.SelectQuestOffer(Mq04, L("Demon blood, especially, is opposite to my nature, and it will cause an ever stronger reaction on the device."),
				Option(L("I will come back after destroying it"), "accept"),
				Option(L("It looks hard"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Mq04);
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("You really are Laima's chosen one... I feel like I'm regaining my energy a little now."));
			await dialog.Msg(L("Finally, the Kruvina suppressor. That's the device that collects my vitality and supplies it to the Kruvina."));

			var answer = await dialog.SelectQuestOffer(Mq05, L("That much is operated by the demons' magic power alone. Use Laima's orb to overload the device."),
				Option(L("I will come back after destroying it"), "accept"),
				Option(L("If you do more, it won't do you any good."), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Mq05);
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			await dialog.Msg(L("Even an unfinished Kruvina is too dangerous for this world, however... It needs to be destroyed before the demons make use of it."));

			var answer = await dialog.SelectQuestOffer(Mq06, L("Whatever little power I have recovered, I will entrust it unto you. You have one chance... Please, destroy the Kruvina."),
				Option(L("I will come back after destroying it"), "accept"),
				Option(L("I need some time to prepare"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq06);

				dialog.Npc.PlayAnimation("IDLE");
				await Task.Delay(TimeSpan.FromSeconds(1));
				character.PlayEffect("F_buff_basic025_white_line", 1f);
			}
			return;
		}

		if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
		{
			await dialog.Msg(L("Laima... How despairing it must have been for her, knowing she was powerless to change what would happen."));
			await dialog.Msg(L("I imagine that's why she took it upon herself and drifted away. To find a solution for this calamity... she had no choice but to walk a lonely path."));
			await dialog.Msg(L("Oh...! I just heard Laima's voice. She was calling out for you..."));

			var answer = await dialog.SelectQuestOffer(Mq07, L("I think it's Laima's statue in Zeraha. You should go there."),
				Option(L("I'll go right away"), "accept"),
				Option(L("Let's wait for a while"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq07);
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			character.Quests.ClearQuestTrack(Mq01);
			character.ServerMessage(L("An unknown power seems to be blocking the way."));
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("If only I can escape the Redemption Ward... I can get my energy back. Please hurry..."));
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("The Kruvina is still being produced. You need to hurry..."));
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("Please lend me just a little more of your strength. Only you can do this."));
			return;
		}

		if (character.Quests.IsActive(Mq06))
		{
			character.Quests.ClearQuestTrack(Mq06);
			await dialog.Msg(L("Demon Lord Zaura will never stand for this. May you be blessed in the name of the goddesses."));
			return;
		}

		character.ServerMessage(L("An unknown power seems to be blocking the way."));
	}

	/// <summary>
	/// Goddess Lada's dialog beside the villagers' temple grounds.
	/// </summary>
	private async Task LadaAtTemple(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Goddess Lada"));
		dialog.SetPortrait("Dlg_port_GoddessLada");

		if (character.Quests.IsCompletable(Mq07))
		{
			await dialog.Msg(L("Laima... ......"));
			await dialog.Msg(L("Laima left on her wanderings with nothing but a sad smile for me. She knew it would come to this... She knew, and still she chose such a hard path..."));
			await dialog.Msg(L("But seeing you, I think I understand, if only a little, why she made such a dangerous choice. What it means to be the savior... and what she saw at the end of this calamity."));
			await dialog.Msg(L("You said Laima asked you to find the revelations. The closest revelation from here can be found in Fedimian."));
			await dialog.Msg(L("Even if this sacred duty weighs heavily upon you, you must never give in. If not you... no one can save this world and Laima."));
			await dialog.Msg(L("Lastly, I will pass on to you the power that dwells in Laima's orb. It will be a great help in finding the revelations she hid."));
			await dialog.Msg(L("Please... save Laima from Giltine's hands..."));
			await dialog.CompleteQuest(Mq07);
			return;
		}

		if (character.Quests.IsCompletable(Sq04))
		{
			await dialog.Msg(L("Even if it happened in accordance with Laima's prophecy, I have fallen and left the townspeople helpless and worried.. My gratitude cannot be expressed through mere words..."));
			await dialog.Msg(L("And this flower is filled with great life force... My recovery won't take long with this. Again, thank you..."));
			await dialog.Msg(L("Thank you. May you be blessed by all the living beings in this forest..."));
			await dialog.CompleteQuest(Sq04);
			return;
		}

		if (character.Quests.IsActive(Mq07))
		{
			character.Quests.ClearQuestTrack(Mq07);
			await dialog.Msg(L("Her voice was calling out for you, yearning. Hurry and go to the statue of Goddess Laima in Zeraha."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("Dear Savior... I've heard that you helped the villagers while I was captured by the Demon Lord. You've completed a task that I was supposed to do... Thank you very much."));
		else
			await dialog.Msg(L("Our savior... The future of this world depends on your journey... Now go... Follow Laima's revelations and walk the path of salvation..."));
	}

	/// <summary>
	/// The Vitality Absorption Device on the Irbedi Cliff.
	/// </summary>
	private async Task Drain(Dialog dialog)
	{
		var character = dialog.Player;
		if (!character.Quests.IsCompletable(Mq04))
			return;

		var sprayed = await character.TimeActions.StartAsync(L("Spraying the demon blood"), L("Cancel"), "FEED", TimeSpan.FromSeconds(2));
		if (sprayed != TimeActionResult.Completed)
			return;

		dialog.Npc.PlayEffect("F_explosion049_fire", 1f, 1, EffectLocation.Bottom);

		await dialog.Msg(L("The demon blood seeps into the device. It shudders, and the flow of the goddess' vitality stops."));
		await dialog.CompleteQuest(Mq04);
		character.LookAround();
	}

	/// <summary>
	/// The village priest's dialog at the temple grounds.
	/// </summary>
	private async Task Priest(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Village Priest"));

		if (character.Quests.IsCompletable(Sq01))
		{
			await dialog.Msg(L("Thank you. There are a lot less demons around now."));
			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (character.Quests.IsCompletable(Sq02))
		{
			await dialog.Msg(L("You're back already. Did... you find a master to help us?"));
			await dialog.Msg(L("The Druid Master wrote this scroll herself? That's even better than I expected. Thank you!"));
			await dialog.CompleteQuest(Sq02);
			return;
		}

		if (character.Quests.IsCompletable(Sq03))
		{
			await dialog.Msg(L("Revelator, your help has given us new hope. We couldn't have solved this without you..."));
			await dialog.Msg(L("After the new temple is completed, feel free to come by again, Revelator!"));
			await dialog.CompleteQuest(Sq03);
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("You were brought here by destiny! Thank you, thank you for rescuing Goddess Lada."));
			await dialog.Msg(L("Someday I'm going to rebuild a temple for Goddess Lada and Goddess Laima, when they return."));
			await dialog.Msg(L("But the demons will not give up. And Lada's condition itself is still unstable."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("We would like to chase away the demons left around here, would you help?"),
				Option(L("Alright, I'll help you"), "accept"),
				Option(L("I don't have time for that"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Sq01);
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("Now we need to purify the land contaminated by evil energy. But I don't know how."));
			await dialog.Msg(L("This wasn't even the work of demons, but a human... Widas can't seem to find a solution right away, either."));
			await dialog.Msg(L("Not far from here there's a big city called Fedimian. I wonder if there is a master there who could come up with something."));
			await dialog.Msg(L("Of course I would go there myself but... We're short-handed as it is here, I don't think it would be right for me to leave."));

			var answer = await dialog.SelectQuestOffer(Sq02, L("If you can, I would like you to go to Fedimian and ask the masters there for advice."),
				Option(L("I will come back with some answers"), "accept"),
				Option(L("About Fedimian"), "explain"),
				Option(L("I think that's going to be difficult"), "leave")
			);

			if (answer == "explain")
			{
				await dialog.Msg(L("Fedimian is a really big city. Especially because it's close to the Great Cathedral, it's a common destination for pilgrims."));
				await dialog.Msg(L("Fedimian, too, was afected by Medzio Diena... But even though the business district was destroyed, a surprising number of people survived."));
				await dialog.Msg(L("People there think it was because Fedimian is in the divine area where the the great statue of Goddess Austeja is. Now they're hard at work to rebuild the city."));
				return;
			}

			if (answer == "accept")
			{
				character.Quests.Start(Sq02);

				await dialog.Msg(L("Go down at the Kraijwi Crossings and you should see Fedimian. Alright then, I look forward to hear good news from you!"));
			}
			return;
		}

		if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
		{
			await dialog.Msg(L("I found the source of the contamination. The demons set up another totem."));
			await dialog.Msg(L("This purification scroll should do it but... With the demons guarding it, I can't get close enough to the totem."));

			var answer = await dialog.SelectQuestOffer(Sq03, L("Will you destroy the demon totem and purify it with the scroll?"),
				Option(L("I will be the one to purify"), "accept"),
				Option(L("I can only help so much"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= Totems.GetLength(0); ++i)
					character.Variables.Perm.Set(TotemVar + i, false);

				character.Quests.Start(Sq03);

				if (character.Inventory.CountItem(ItemId.ORCHARD_324_SQ_SCROLL) == 0)
					character.Inventory.Add(ItemId.ORCHARD_324_SQ_SCROLL, 1, InventoryAddType.PickUp);

				character.LookAround();

				await dialog.Msg(L("I'm very glad to hear that. The totems are close to the Inega Vacant Lot and in the Odur Backyard."));
			}
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			await dialog.Msg(L("Goddess Lada is still not feeling well. We'll look after Her now."));
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			await dialog.Msg(L("One of the masters in Fedimian should be able to help us! I hope so..."));
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			if (character.Inventory.CountItem(ItemId.ORCHARD_324_SQ_SCROLL) == 0)
				character.Inventory.Add(ItemId.ORCHARD_324_SQ_SCROLL, 1, InventoryAddType.PickUp);

			await dialog.Msg(L("I can't understand why someone would side with the demons... How powerful is their evil energy, that even ferrets can't handle it?"));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
		{
			await dialog.Msg(L("I know it was Goddess Laima who sent us a Revelator, but... Still, I think everything worked out even better because of you."));
			await dialog.Msg(L("I'm very thankful for everything. For saving me, and for rescuing Goddess Lada too."));
		}
		else
		{
			await dialog.Msg(L("Now that we know Goddess Lada is safe, I want to build a shrine with the villagers."));
			await dialog.Msg(L("I have faith that, with the power if the goddess, our river will become clear again. It will."));
		}
	}

	/// <summary>
	/// The Great Priest of the Village's dialog at the temple grounds.
	/// </summary>
	private async Task GreatPriest(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Great Priest of the Village"));

		if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
		{
			await dialog.Msg(L("Our people feel safe now that everything's come to an end... You really are our savior. Thank you, thank you!"));
			await dialog.Msg(L("Our people are going to join forces and build a temple for the goddesses now."));
			await dialog.Msg(L("By the way, I was looking around the Irbedi Cliff for a spot to build the temple and I found a place with a strong vital force."));
			await dialog.Msg(L("I couldn't do much more because of the demons... But the energy of that land, I'm sure it'll help Goddess Lada recover Hers."));
			await dialog.Msg(L("So I want to ask you. They say this seed grows a flower that takes on whatever properties of the land where it's planted."));

			var answer = await dialog.SelectQuestOffer(Sq04, L("If we plant it on that land, the flower too will have the same life force. Will you bring that flower to Goddess Lada?"),
				Option(L("I will try"), "accept"),
				Option(L("I can't help you"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq04);

				if (character.Inventory.CountItem(ItemId.ORCHARD_324_SQ_04_SEED) == 0)
					character.Inventory.Add(ItemId.ORCHARD_324_SQ_04_SEED, 1, InventoryAddType.PickUp);
			}
			return;
		}

		if (character.Quests.IsActive(Sq04))
		{
			if (character.Inventory.CountItem(ItemId.ORCHARD_324_SQ_04_SEED) == 0 && character.Inventory.CountItem(ItemId.ORCHARD_324_SQ_04_FLOWER) == 0)
				character.Inventory.Add(ItemId.ORCHARD_324_SQ_04_SEED, 1, InventoryAddType.PickUp);

			await dialog.Msg(L("I have never felt a force so powerful. I only hope this flower can heal the wounds in Goddess Lada's body and spirit..."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
		{
			await dialog.Msg(L("I never thought everything Goddess Laima predicted would become true. The disaster happening... even the Revelator coming to save us from it."));
			await dialog.Msg(L("And yet the disaster happened, and now you saved our village and Goddess Lada."));
			await dialog.Msg(L("I should have been the one to keep the faith when our village was down, but I didn't. So all of that work was in vain..."));
			await dialog.Msg(L("Thank you for making me realize that before it was too late..."));
		}
		else
		{
			await dialog.Msg(L("Thanks to you Goddess Lada is safe. She's lost her power now but... I know she'll get it back someday."));
			await dialog.Msg(L("I know you'll do well no matter what path you chose. I've seen the miracles you're capable of with my own eyes."));
			await dialog.Msg(L("Goddess Laima is leading you to be the savior of the world, I can tell. May your every step receive the blessing of the goddesses..."));
		}
	}

	/// <summary>
	/// Believer Dreka's dialog near the Shulti Workshop.
	/// </summary>
	private async Task Dreka(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Believer Dreka"));

		if (character.Quests.IsCompletable(Rp1))
		{
			await dialog.Msg(L("I haven't been able to smell Ceyral for such a long time. Thank you so much for gathering this much!"));
			await dialog.Msg(L("However, I do think that we need a bit more."));
			await dialog.CompleteQuest(Rp1);
			return;
		}

		if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
		{
			await dialog.Msg(L("I wish to present Godess Lada with Ceyral Saplings that are filled with the energy of life. That's why we've come all this way..."));
			await dialog.Msg(L("But there are simply too many monsters in there for me to go anywhere near."));

			var answer = await dialog.SelectQuestOffer(Rp1, L("Could you please go get some Ceyral Saplings for me?"),
				Option(L("I will collect them"), "accept"),
				Option(L("Ignore"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Rp1);
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Rp1))
		{
			await dialog.Msg(L("I've gone through so much to get here.. I should have brought my brother along..."));
			return;
		}

		await dialog.Msg(L("We'll start surveying this region in earnest once Goddess Lada recovers.. Of course that is a long way off..."));
	}

	/// <summary>
	/// Overloads the Kruvina Suppressor with the power of Laima's orb.
	/// </summary>
	private static void OverloadSuppressor(Character character)
	{
		if (!character.Quests.IsActive(Mq05) || character.Quests.IsCompletable(Mq05))
			return;

		if (character.Inventory.CountItem(ItemId.ORCHARD_342_MQ_HOLLY_SPHERE) == 0)
		{
			character.ServerMessage(L("You need Goddess Laima's orb to overload the device."));
			return;
		}

		character.PlayEffect("F_buff_basic025_white_line", 1f);
		character.Quests.CompleteObjective(Mq05, "overloadSuppressor");
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The magic power is overloaded on Kruvina Suppressor"), 5);
		character.LookAround();
	}

	/// <summary>
	/// Destroys one of the demon totems and purifies its land with the
	/// Druid Master's scroll.
	/// </summary>
	private static void PurifyTotem(Character character, int number)
	{
		if (!character.Quests.IsActive(Sq03) || character.Quests.IsCompletable(Sq03) || IsTotemPurified(character, number))
			return;

		if (character.Inventory.CountItem(ItemId.ORCHARD_324_SQ_SCROLL) == 0)
		{
			character.ServerMessage(L("You need the Purification Scroll to cleanse the land."));
			return;
		}

		character.PlayEffect("F_light018_yellow", 1f);
		character.Variables.Perm.Set(TotemVar + number, true);
		character.Quests.CompleteObjective(Sq03, "purify" + number);
		character.ServerMessage(L("The demon totem crumbles and the evil energy around it fades."));
		character.LookAround();
	}

	/// <summary>
	/// Overloads the Kruvina Suppressor with Laima's orb.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_342_MQ_HOLLY_SPHERE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_32_4" || !character.Quests.IsActive(Mq05) || character.Position.Get2DDistance(SuppressorSpot) > SuppressorRange)
		{
			character.ServerMessage(L("A warm light glows inside the orb."));
			return ItemUseResult.OkayNotConsumed;
		}

		OverloadSuppressor(character);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Purifies the land around the closest demon totem.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_324_SQ_SCROLL(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName == "f_orchard_32_4" && character.Quests.IsActive(Sq03))
		{
			for (var i = 0; i < Totems.GetLength(0); ++i)
			{
				var number = i + 1;
				if (IsTotemPurified(character, number))
					continue;

				if (character.Position.Get2DDistance(new Position((float)Totems[i, 0], character.Position.Y, (float)Totems[i, 1])) > TotemRange)
					continue;

				PurifyTotem(character, number);
				return ItemUseResult.OkayNotConsumed;
			}
		}

		character.ServerMessage(L("There is no demon totem nearby to purify."));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Plants the Earth Flower seed on the land with the strong vital force.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_324_SQ_04_SEED(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_32_4" || !character.Quests.IsActive(Sq04) || character.Quests.IsCompletable(Sq04))
		{
			character.ServerMessage(L("This isn't the place to plant the seed."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Position.Get2DDistance(FlowerSpot) > FlowerRange)
		{
			character.ServerMessage(L("The land here doesn't hold the vital force the Great Priest spoke of."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.PlayEffect("F_light018_yellow", 1f);
		character.Inventory.Add(ItemId.ORCHARD_324_SQ_04_FLOWER, 1, InventoryAddType.PickUp);
		character.ServerMessage(L("The Earth Flower has blossomed."));

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Returns whether the character destroyed the given Redemption Ward.
	/// </summary>
	private static bool IsWardDestroyed(Character character, int number)
		=> character.Quests.IsActive(Mq03) && character.Variables.Perm.GetBool(WardVar + number, false);

	/// <summary>
	/// Returns whether the character purified the given demon totem.
	/// </summary>
	private static bool IsTotemPurified(Character character, int number)
		=> character.Quests.IsActive(Sq03) && character.Variables.Perm.GetBool(TotemVar + number, false);

	/// <summary>
	/// Returns false if the character searched the spot too recently.
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
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 80041: Divine Encounter
//-----------------------------------------------------------------------------
public class FOrchard324Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80041);
		SetName(L("Divine Encounter"));
		SetDescription(L("Goddess Laima has asked you to rescue Goddess Lada from the demons and thwart their plans. She is being held captive by Demon Lord Zaura in the Seir Rainforest. Go find her."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2", "f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD_324_MQ_01_TRIG", "f_orchard_32_4", L("Look for Goddess Lada"), L("Goddess Laima has asked you to rescue Goddess Lada from the demons and thwart their plans."));
		SetPhase(QuestStatus.InProgress, "ORCHARD_324_MQ_01_TRIG", "f_orchard_32_4", L("Look for Goddess Lada"), L("Goddess Laima has asked you to rescue Goddess Lada from the demons and thwart their plans. She is being held captive by Demon Lord Zaura in the Seir Rainforest. Go find her."));
		SetPhase(QuestStatus.Success, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("Demon Lord Zaura may have escaped, but the force around Goddess Lada has disappeared. Talk to Goddess Lada."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ORCHARD_324_MQ_01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(80036, QuestStatus.Completed));

		AddObjective("defeatZaura", L("Defeat Demon Lord Zaura"), new KillObjective(1, "boss_Zawra") { LayerOnly = true });
	}
}

// 80042: Kidnapped Goddess
//-----------------------------------------------------------------------------
public class FOrchard324Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80042);
		SetName(L("Kidnapped Goddess"));
		SetDescription(L("Goddess Lada says that, before destroying the Kruvina, you need to destroy the devices attached to Her. Talk to Goddess Lada."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("While battling Demon Lord Zaura, the force around Goddess Lada seems to have disappeared. Talk to Goddess Lada."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("While battling Demon Lord Zaura, the force around Goddess Lada seems to have disappeared. Talk to Goddess Lada."));
		SetPhase(QuestStatus.Success, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("Goddess Lada says that, before destroying the Kruvina, you need to destroy the devices attached to Her. Talk to Goddess Lada."));

		AddPrerequisite(new QuestStatusPrerequisite(80041, QuestStatus.Completed));

		AddObjective("listenToLada", L("Speak with Goddess Lada"), new ManualObjective());
	}
}

// 80043: Release the Goddess
//-----------------------------------------------------------------------------
public class FOrchard324Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80043);
		SetName(L("Release the Goddess"));
		SetDescription(L("Goddess Lada seems to be losing Her conscience. Try and destroy the Redemption Wards closest to Goddess Lada."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("Rescue Goddess Lada and ask Her how to thwart the demons' schemes."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_BINDIG1", "f_orchard_32_4", L("Destroy the Redemption Ward"), L("Goddess Lada seems to be losing Her conscience. Try and destroy the Redemption Wards closest to Goddess Lada."));
		SetPhase(QuestStatus.Success, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("You have destroyed all Redemption Wards. Ask Goddess Lada whether She is all right."));

		AddPrerequisite(new QuestStatusPrerequisite(80042, QuestStatus.Completed));

		AddObjective("destroyWard1", L("Destroy the first Redemption Ward"), new ManualObjective());
		AddObjective("destroyWard2", L("Destroy the second Redemption Ward"), new ManualObjective());
		AddObjective("destroyWard3", L("Destroy the third Redemption Ward"), new ManualObjective());
	}
}

// 80044: Destroy Vitality Absorption Device
//-----------------------------------------------------------------------------
public class FOrchard324Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80044);
		SetName(L("Destroy Vitality Absorption Device"));
		SetDescription(L("Goddess Lada believes that adding demon blood to the vitality absorption device will cause it to reverse and become deactivated."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("You have destroyed the Redemption Wards keeping Goddess Lada captive. Talk to Her."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_LADA", "f_orchard_32_4", L("Collect Demon Blood"), L("Goddess Lada believes that adding demon blood to the vitality absorption device will cause it to reverse and become deactivated."));
		SetPhase(QuestStatus.Success, "ORCHARD324_DRAIN", "f_orchard_32_4", L("Destroy Vitality Absorption Device"), L("You have collected enough demon blood. Apply it to the vitality absorption device."));

		AddPrerequisite(new QuestStatusPrerequisite(80043, QuestStatus.Completed));

		AddObjective("collectBlood", L("Defeat Vikaras Mages and collect Demon Blood"), new CollectItemObjective("ORCHARD_324_MQ_04_ITEM", 10));
		AddPityDrop("ORCHARD_324_MQ_04_ITEM", 1.0f, 0, 1, "Sec_wolf_statue_mage");

		AddReward(new TakeItemReward("ORCHARD_324_MQ_04_ITEM", -1));
	}
}

// 80045: Kruvina Suppressor
//-----------------------------------------------------------------------------
public class FOrchard324Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80045);
		SetName(L("Kruvina Suppressor"));
		SetDescription(L("Goddess Lada told you that when you use the orb of the goddess to Kruvina suppressor, the magical energy would overload and it would stop working."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("You've destroyed the vitality absorbing device. Talk with Lada what to do next."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_DESPENSOR", "f_orchard_32_4", L("Destroy the Kruvina Suppressor"), L("Goddess Lada told you that when you use the orb of the goddess to Kruvina suppressor, the magical energy would overload and it would stop working."));
		SetPhase(QuestStatus.Success, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("Destroyed all three machines by following the instruction from Goddess Lada. Report back to Goddess Lada."));

		AddPrerequisite(new QuestStatusPrerequisite(80044, QuestStatus.Completed));

		AddObjective("overloadSuppressor", L("Destroy the Kruvina Suppressor"), new ManualObjective());
	}
}

// 80046: The Thing That Should Not Let It Be
//-----------------------------------------------------------------------------
public class FOrchard324Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80046);
		SetName(L("The Thing That Should Not Let It Be"));
		SetDescription(L("Goddess Lada says even an incomplete Kruvina can be used for evil and thinks it should be destroyed."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("You have destroyed all the Kruvina devices. Talk to Goddess Lada."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_KRUVINA", "f_orchard_32_4", L("Destroy Incomplete Kruvina"), L("Goddess Lada says even an incomplete Kruvina can be used for evil and thinks it should be destroyed."));
		SetPhase(QuestStatus.Success, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("Demon Lord Zaura may have been defeated, but an intruding beholder has stolen the Kruvina. Return to Goddess Lada."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ORCHARD_324_MQ_06_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(80045, QuestStatus.Completed));

		AddObjective("defeatZaura", L("Defeat Zaura, Empowered by Giltine"), new KillObjective(1, "boss_Zawra_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 4));
		AddReward(new ItemReward("Vis", 2280));
		AddReward(new ItemReward("TreasureboxKey4", 1));
	}
}

// 80047: The Goddess' Hidden Message
//-----------------------------------------------------------------------------
public class FOrchard324Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80047);
		SetName(L("The Goddess' Hidden Message"));
		SetDescription(L("Goddess Lada says She heard the voice of Goddess Laima calling for you. Go to the statue of Goddess Laima in Zeraha."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_4", "f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "ORCHARD324_LADA", "f_orchard_32_4", L("Speak with Goddess Lada"), L("The Watcher took the Incomplete Kruvina away before you could do anything about it. Report this to Goddess Lada."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_CRYSTAL", "f_orchard_34_2", L("Go to the Statue of Goddess Laima in Zeraha"), L("Goddess Lada says She heard the voice of Goddess Laima calling for you. Go to the statue of Goddess Laima in Zeraha."));
		SetPhase(QuestStatus.Success, "ORCHARD324_LADA2", "f_orchard_32_4", L("Speak with Goddess Lada"), L("On approaching the great statue of Goddess Laima, you saw She is being hurt by Giltine. Laima has asked you to find all the revelations. Tell Goddess Lada about this."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ORCHARD_324_MQ_07_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(80046, QuestStatus.Completed));

		AddObjective("visitStatue", L("Go to the Statue of Goddess Laima in Zeraha"), new ManualObjective());

		AddReward(new StatPointReward(3));
		AddReward(new ItemReward("expCard6", 5));
		AddReward(new ItemReward("Vis", 2850));
		AddReward(new TakeItemReward("ORCHARD_342_MQ_HOLLY_SPHERE", -1));
	}
}

// 80048: Temple Rebuilding Preparation (1)
//-----------------------------------------------------------------------------
public class FOrchard324Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80048);
		SetName(L("Temple Rebuilding Preparation (1)"));
		SetDescription(L("The village priest wants to rebuild the fallen temple to prepare for the return of Goddesses Laima and Lada. But first, you need to clear out the demons and purify the contaminated land. The village priest hopes you can help chase the demons away."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_PRIEST", "f_orchard_32_4", L("Speak with the Village Priest"), L("The Village Priest seems to want to ask you for a favor. Speak with the Village Priest."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_PRIEST", "f_orchard_32_4", L("Defeat the demons"), L("The village priest wants to rebuild the fallen temple to prepare for the return of Goddesses Laima and Lada. But first, you need to clear out the demons and purify the contaminated land. The village priest hopes you can help chase the demons away."));
		SetPhase(QuestStatus.Success, "ORCHARD324_PRIEST", "f_orchard_32_4", L("Speak with the Village Priest"), L("You've defeated the remaining demons. Report back to the Village Priest."));

		AddPrerequisite(new QuestStatusPrerequisite(80047, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the remaining demons"), new KillObjective(12, "Sec_wolf_statue_mage"));

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("Vis", 1140));
	}
}

// 80049: Temple Rebuilding Preparation (2)
//-----------------------------------------------------------------------------
public class FOrchard324Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80049);
		SetName(L("Temple Rebuilding Preparation (2)"));
		SetDescription(L("The village priest wants to purify the land contaminated with evil energy, but they aren't sure what to do, as it was contaminated on purpose. Go to Fedimian and ask the Masters there for advice on land purification."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_4", "c_fedimian");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_PRIEST", "f_orchard_32_4", L("Speak with the Village Priest"), L("You've defeated the remaining demons like the village priest asked you to. Speak with the Village Priest."));
		SetPhase(QuestStatus.InProgress, "JOB_DRUID3_1_NPC", "c_fedimian", L("Find a Master in Fedimian Who Might Know About Purification"), L("The village priest wants to purify the land contaminated with evil energy, but they aren't sure what to do, as it was contaminated on purpose. Go to Fedimian and ask the Masters there for advice on land purification."));
		SetPhase(QuestStatus.Success, "ORCHARD324_PRIEST", "f_orchard_32_4", L("Speak with the Village Priest"), L("You were able to obtain the scroll of the purification from the Druid Master who you met at Fedimian. Go back to the village priest."));

		AddPrerequisite(new QuestStatusPrerequisite(80048, QuestStatus.Completed));

		AddObjective("findMaster", L("Look for a way to purify the land at Fedimian"), new CollectItemObjective("ORCHARD_324_SQ_SCROLL", 1));

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("Vis", 1140));
		AddReward(new TakeItemReward("ORCHARD_324_SQ_SCROLL", -1));
	}
}

// 80050: Temple Rebuilding Preparation (3)
//-----------------------------------------------------------------------------
public class FOrchard324Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80050);
		SetName(L("Temple Rebuilding Preparation (3)"));
		SetDescription(L("The village priest thinks the demon totems could be the cause of the contamination and wants you to destroy the ones in the Inega Vacant Lot and Odur Backyard, then use the scroll to purify the land there."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_PRIEST", "f_orchard_32_4", L("Speak with the Village Priest"), L("You have obtained a Purification Scroll from the Druid Master in Fedimian. Talk to the village priest."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_EVIL1", "f_orchard_32_4", L("Purify the evil energy"), L("The village priest thinks the demon totems could be the cause of the contamination and wants you to destroy the ones in the Inega Vacant Lot and Odur Backyard, then use the scroll to purify the land there."));
		SetPhase(QuestStatus.Success, "ORCHARD324_PRIEST", "f_orchard_32_4", L("Speak with the Village Priest"), L("You've destroyed the demon totems and purified the land like the priest asked you to. Go back to the Village Priest."));

		AddPrerequisite(new QuestStatusPrerequisite(80049, QuestStatus.Completed));

		AddObjective("purify1", L("Purify the first demon totem"), new ManualObjective());
		AddObjective("purify2", L("Purify the second demon totem"), new ManualObjective());
		AddObjective("purify3", L("Purify the third demon totem"), new ManualObjective());
		AddObjective("purify4", L("Purify the fourth demon totem"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1730));
		AddReward(new TakeItemReward("ORCHARD_324_SQ_SCROLL", -1));
	}
}

// 80051: The Flower Enriched With the Earth
//-----------------------------------------------------------------------------
public class FOrchard324Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80051);
		SetName(L("The Flower Enriched With the Earth"));
		SetDescription(L("The great priest of the village has asked you to plant the Earth Flower seeds and deliver the flower to Goddess Lada"));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_FUZE", "f_orchard_32_4", L("Talk with the Village Great Priest"), L("It seems that the Village Great Priest has a request for you. Talk with the new officer."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_FUZE", "f_orchard_32_4", L("Grow the Earth Flower"), L("The great priest of the village has asked you to plant the Earth Flower seeds and deliver the flower to Goddess Lada"));
		SetPhase(QuestStatus.Success, "ORCHARD324_LADA2", "f_orchard_32_4", L("Deliver to Goddess Lada"), L("The Earth Flower has blossomed. Bring it to Goddess Lada."));

		AddPrerequisite(new QuestStatusPrerequisite(80047, QuestStatus.Completed));

		AddObjective("growFlower", L("Grow the Earth Flower"), new CollectItemObjective("ORCHARD_324_SQ_04_FLOWER", 1));

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1735));
		AddReward(new TakeItemReward("ORCHARD_324_SQ_04_SEED", -1));
		AddReward(new TakeItemReward("ORCHARD_324_SQ_04_FLOWER", -1));
	}
}

// 60185: Hoping the Goddess Will Recover
//-----------------------------------------------------------------------------
public class FOrchard324Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60185);
		SetName(L("Hoping the Goddess Will Recover"));
		SetDescription(L("Believer Dreka wishes to collect Ceyral Saplings for Goddess Rada to recover. Collect Ceyral Saplings from near Shulti Workshop and return to Dreka."));
		SetType(QuestType.Repeat);
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD324_RP_1_NPC", "f_orchard_32_4", L("Talk to Believer Dreka"), L("Believer Dreka is waiting for help at Seir Rainforest."));
		SetPhase(QuestStatus.InProgress, "ORCHARD324_RP_1_OBJ", "f_orchard_32_4", L("Collect Ceyral Saplings"), L("Believer Dreka wishes to collect Ceyral Saplings for Goddess Rada to recover. Collect Ceyral Saplings from near Shulti Workshop and return to Dreka."));
		SetPhase(QuestStatus.Success, "ORCHARD324_RP_1_NPC", "f_orchard_32_4", L("Return to Believer Dreka"), L("You have collected enough Ceyral Saplings. Hand them to Believer Dreka."));

		AddPrerequisite(new QuestStatusPrerequisite(80047, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(90));

		AddObjective("collectSaplings", L("Collect Ceyral Saplings"), new CollectItemObjective("ORCHARD324_RP_1_ITEM", 10));

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new TakeItemReward("ORCHARD324_RP_1_ITEM", -1));
	}
}
