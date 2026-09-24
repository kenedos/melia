//--- Melia Script ----------------------------------------------------------
// Novaha Institute Quest NPCs
//--- Description -----------------------------------------------------------
// Edmundas' pursuit of the wizard's devices, Rose's rescue at the Kilnuma
// Oratorium and the siblings' life after the spores.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
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

public class DAbbey643QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Abbay642Mq040 = new QuestId(50128);
	private readonly static QuestId Mq010 = new QuestId(50134);
	private readonly static QuestId Mq020 = new QuestId(50135);
	private readonly static QuestId Mq030 = new QuestId(50136);
	private readonly static QuestId Mq040 = new QuestId(50137);
	private readonly static QuestId Mq050 = new QuestId(50144);
	private readonly static QuestId Sq010 = new QuestId(50138);
	private readonly static QuestId Sq020 = new QuestId(50139);
	private readonly static QuestId Sq040 = new QuestId(50141);
	private readonly static QuestId Sq060 = new QuestId(50143);
	private readonly static QuestId Hq1 = new QuestId(50261);

	private const string AfterTrackId = "ABBAY_64_3_MQ040_AFTER_TRACK";

	private const string BarrierVar = "Gabija.Quests.Abbay643Mq030.Barrier";
	public const string CrystalCountVar = "Gabija.Quests.Abbay643Mq030.Crystals";
	private const string CrystalVar = "Gabija.Quests.Abbay643Mq030.Crystal";
	private const string SporeOrderVar = "Gabija.Quests.Abbay643Sq060.Order";
	private const string SporeStepVar = "Gabija.Quests.Abbay643Sq060.Step";

	private static readonly double[,] Crystals =
	{
		{ -418.45, -1476.55, -84 }, { -172.21, -1374.63, 26 }, { -379.75, -1215.31, 110 }, { -115.17, -1562.35, 90 }, { -42.89, -1215.77, 90 },
	};

	private static readonly double[,] CrystalCircles =
	{
		{ -42.79, -1215.33 }, { -378.82, -1215.08 }, { -173.95, -1374.43 }, { -115.47, -1563.75 }, { -420.66, -1475.97 },
	};

	private static readonly double[,] SporeDevices =
	{
		{ 49.53, 1345.39, 91 }, { -137.76, 1337.54, 169 }, { -158.24, 1185.25, 215 }, { 71.76, 1191.93, 90 },
	};

	protected override void Load()
	{
		// Edmundas at the first device
		//-------------------------------------------------------------------------
		AddConditionalNpc(153110, L("Edmundas"), "ABBEY643_EDMONDA01", "d_abbey_64_3", 712.04, -141.24, 140, c => c.Quests.HasCompleted(Abbay642Mq040) && !c.Quests.HasCompleted(Mq020), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Edmundas"));

			if (character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("Here it is! The mucus of a Hummingbird."));
				await dialog.CompleteQuest(Mq010);

				if (!character.Quests.HasCompleted(Mq010) || character.Quests.Has(Mq020))
					return;
			}

			if (character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("Success! The device overloaded and stopped working."));
				await dialog.Msg(L("The next device is in the Main Hall Atrium. Let's meet up there."));
				await dialog.CompleteQuest(Mq020);

				if (character.Quests.HasCompleted(Mq020))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("Oh... You're here. I'm beyond grateful that you decided to do this favor for me..."));
				await dialog.Msg(L("Rose will probably have to go through the same experiments I did. We could just go after the wizard right away to try and save Rose but..."));
				await dialog.Msg(L("I don't think she would make it until then. We need to stop the experiment. We need to find the wizard's devices and destroy them."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("That'll buy us time until we can get to Rose."),
					Option(L("What do I have to do?"), "accept"),
					Option(L("Tell him that you need some time to think"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);

					await dialog.Msg(L("I don't know what this device in front of me is... But I saw the wizard put something black in it to turn it on, then suddenly the air was filled with something nauseating."));
					await dialog.Msg(L("I felt something grow inside of my body, it was extremely painful. Then the pain stopped, but the wizard became furious."));
					await dialog.Msg(L("He said something about finding someone who's qualified... If Rose is the one they want, she'll be going through the same pain right now."));
					await dialog.Msg(L("If only we can get whatever item the wizard used on the device... If we put too much of it in the device, I think there's a chance it might overload and shut down."));
					await dialog.Msg(L("I saw the wizard tell the demons to go hunt Brown Hummingbirds for it. Anything is fine, just gather what you can and bring it to me."));
				}
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				var answer = await dialog.SelectQuestOffer(Mq020, L("Let's gather more Hummingbird Mucus and destroy the device. The more the better. I'll pour it in all at once."),
					Option(L("I'll collect some mucus for you"), "accept"),
					Option(L("We should consider there won't be any side effects to overloading the device"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq020);

				return;
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("Those who are qualified will not return to the goddesses, despite the pain. That doesn't mean it's painless, however..."));
				return;
			}

			await dialog.Msg(L("It was a black liquid. Just bring me anything. I'll put it in the device and check."));
		});

		AddNpc(151003, "UnvisibleName", "ABBEY643_MQ02_DEVICE01", "d_abbey_64_3", 726.20, -96.90, 90);

		// Edmundas in the Main Hall Atrium
		//-------------------------------------------------------------------------
		AddConditionalNpc(153110, L("Edmundas"), "ABBEY643_EDMONDA02", "d_abbey_64_3", -294.64, -796.18, -32, c => c.Quests.HasCompleted(Mq020) && !c.Quests.HasCompleted(Mq030), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Edmundas"));

			if (character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Are you done? The wizard did say the host would act like a switch... As long as Rose is safe again, I'll do whatever it takes."));
				await dialog.Msg(L("There are too many monsters here. Take the elevator and let's meet upstairs."));
				await dialog.CompleteQuest(Mq030);

				if (character.Quests.HasCompleted(Mq030))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("That magic crystal... The wizard used it for a mind control ritual in the atrium at first."));
				await dialog.Msg(L("The crystal was glowing and I felt my conscience leaving me. When I opened my eyes... I was lying inside the Kilnuma Oratorium."));
				await dialog.Msg(L("Rose is probably going through the same thing."));

				var answer = await dialog.SelectQuestOffer(Mq030, L("But without the protective barrier device, we can't destroy the crystal."),
					Option(L("I'll destroy the magic crystal"), "accept"),
					Option(L("Rose could be mentally affected; that's not right"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Crystals.GetLength(0); ++i)
						character.Variables.Perm.Set(CrystalVar + i, false);
					character.Variables.Perm.SetInt(CrystalCountVar, 0);
					character.Variables.Perm.Set(BarrierVar, false);

					character.Quests.Start(Mq030);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Something bad could happen to Rose if we destroy the magic crystal. Still... no matter what happens, I don't think Rose wouldn't want to be used to spread out the death spores."));
		});

		// The protective barrier device and the Mind Control Crystals
		//-------------------------------------------------------------------------
		AddQuestTrigger("ABBEY643_MQ03_DEVICE01_DESCIPT", "d_abbey_64_3", -239.88, -1747.37, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq030) || character.Quests.IsCompletable(Mq030) || character.Variables.Perm.GetBool(BarrierVar, false))
				return;

			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The device inside the main building's atrium seems to be the protective shield device Edmundas mentioned."), 5);

			await Task.CompletedTask;
		});

		AddNpc(153137, "UnvisibleName", "ABBEY643_MQ03_DEVICE01", "d_abbey_64_3", -239.88, -1747.37, 90, async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq030) || character.Quests.IsCompletable(Mq030) || character.Variables.Perm.GetBool(BarrierVar, false))
				return;

			character.Variables.Perm.Set(BarrierVar, true);
			character.PlayEffect("F_buff_basic009_blue", 1f);

			await Task.CompletedTask;
		});

		for (var i = 0; i < CrystalCircles.GetLength(0); ++i)
			AddConditionalNpc(47124, "UnvisibleName", "ABBEY643_MQ03_CIRCLE_" + (i + 1), "d_abbey_64_3", CrystalCircles[i, 0], CrystalCircles[i, 1], 90, c => !c.Quests.HasCompleted(Mq030));

		for (var i = 0; i < Crystals.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(153060, L("Mind Control Crystal"), "ABBEY643_MQ03_DEVICE02_" + number, "d_abbey_64_3", Crystals[i, 0], Crystals[i, 1], Crystals[i, 2],
				character => !character.Quests.HasCompleted(Mq030) && !character.Variables.Perm.GetBool(CrystalVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Mq030) || character.Quests.IsCompletable(Mq030) || character.Variables.Perm.GetBool(CrystalVar + number, false))
						return;

					if (!character.Variables.Perm.GetBool(BarrierVar, false))
					{
						character.ServerMessage(L("The device inside the main building's atrium seems to be the protective shield device Edmundas mentioned."));
						return;
					}

					character.Variables.Perm.Set(CrystalVar + number, true);
					var destroyed = character.Variables.Perm.GetInt(CrystalCountVar, 0) + 1;
					character.Variables.Perm.SetInt(CrystalCountVar, destroyed);

					dialog.Npc.PlayEffect("F_explosion014", 1f);
					character.ServerMessage(LF("Mind Control Crystals destroyed: {0}/{1}", Math.Min(destroyed, 5), 5));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Edmundas at the Medie State Apartments
		//-------------------------------------------------------------------------
		AddConditionalNpc(153110, L("Edmundas"), "ABBEY643_EDMONDA03", "d_abbey_64_3", -1513, -473, 90, c => c.Quests.HasCompleted(Mq030) && !c.Quests.Has(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Edmundas"));

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				await dialog.Msg(L("Rose will be here at the Kilnuma Oratorium. I wish I could rescue her myself, but I'm afraid I lack the skills to do it."));

				var answer = await dialog.SelectQuestOffer(Mq040, L("Please let it not be too late... Even if it is, your help will have been life-saving."),
					Option(L("I'll rescue Rose"), "accept"),
					Option(L("Ask her to wait a bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Rose will be here at the Kilnuma Oratorium. I wish I could rescue her myself, but I'm afraid I lack the skills to do it."));
		});

		// Rose and Edmundas at the Kilnuma Oratorium
		//-------------------------------------------------------------------------
		AddConditionalNpc(47123, "UnvisibleName", "ABBEY643_MAGIC_CIRCLE", "d_abbey_64_3", -1459, 175, 90, c => c.Quests.IsActive(Mq040) && !c.Quests.IsCompletable(Mq040));
		AddNpc(47254, "UnvisibleName", "ABBEY643_DESK", "d_abbey_64_3", -1555, 268, 90);

		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY643_ROZE01", "d_abbey_64_3", -1459, 175, 13, c => c.Quests.Has(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq040))
			{
				if (character.Etc.Properties.GetFloat(AfterTrackId) != 1)
				{
					_ = character.Tracks.Start(AfterTrackId, TimeSpan.FromMilliseconds(500));
					return;
				}

				await dialog.Msg(L("You... you saved me again. And my brother, too... Are you okay?"));
				await dialog.Msg(L("I was hoping my brother would have survived and escaped this place... But it's so good to see him again... I guess you can say I was lying to myself."));
				await dialog.Msg(L("I still can't believe I'm alive, that I haven't returned to the goddesses. And I... Yes. I know now. I know what I have to do..."));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			if (character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("This wizard... He's sided with the demons. Also, there are other leaders and followers of this plan."));
				await dialog.Msg(L("Everything was connected to the Divine Tree. Humans can't coexist with the Divine Tree..."));
				await dialog.Msg(L("So they created the giant bracken to spread out the death spores all the way to Orsha. To control that, they needed someone who can survive being infected with the spores."));
				await dialog.Msg(L("That's why they were looking for someone who qualifies for that... And it was me."));
				await dialog.Msg(L("Oh, that red liquid we saw on the giant bracken came from the Pelke Shrine Ruins. And I also saw a strange light concentrating at the Letas Stream."));
				await dialog.Msg(L("The demons and their human followers are doing something monumental. I can't even begin to understand what it is..."));
				await dialog.Msg(L("This is all I can tell you... There's other things here and there, but they're just fragments I can't properly explain to you."));
				await dialog.Msg(L("I haven't been able to rest ever since arriving at the Koru Jungle... I should rest now."));
				await dialog.Msg(L("Yes, it was all thanks to you. Thank you so much... really."));
				await dialog.CompleteQuest(Mq050);

				if (character.Quests.HasCompleted(Mq050))
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Rose saw a strange light gathering at Letas Stream.{nl}Head through Karolis Springs, past Dadan Jungle."), 8);
				return;
			}

			if (character.Quests.IsCompletable(Sq060))
			{
				await dialog.Msg(L("No one will be trying this type of horrible experiment anymore. If they do, my brother and I are here to stop them."));
				await dialog.CompleteQuest(Sq060);
				return;
			}

			if (character.Quests.IsCompletable(Hq1))
			{
				var delivered = await character.TimeActions.StartAsync(L("Delivering Anne's letter..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
				if (delivered != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Anne has a crush on someone! Wow, to think the little girl I knew now likes someone..."));
				await dialog.Msg(L("As a big sister, I should hear her out and comfort her but... At least I now know how she is doing."));
				await dialog.CompleteQuest(Hq1);
				return;
			}

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				await dialog.Msg(L("I... I'm already infected with the spore. But don't worry. I'm qualified so I won't return to the goddesses."));
				await dialog.Msg(L("But you never know when the spore might become active... I don't think I can return to the village."));
				await dialog.Msg(L("It's okay. This life of mine that you saved... I'm going to live it to the absolute fullest."));

				var answer = await dialog.SelectQuestOffer(Mq050, L("By the way, I should tell you something. When the wizard was trying to control my mind, for a moment I was able to look into his memory."),
					Option(L("Tell me more about it"), "accept"),
					Option(L("It's okay, just get some rest"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					character.Quests.CompleteObjective(Mq050, "hearTestimony");
				}
				return;
			}

			if (!character.Quests.Has(Sq060) && character.Quests.MeetsPrerequisites(Sq060))
			{
				var answer = await dialog.SelectQuestOffer(Sq060, L("Can I ask you for one last favor? When I was inside the wizard's memories, I saw the device used for breeding the bracken spores."),
					Option(L("I can help you; tell me more"), "accept"),
					Option(L("I can't help with that"), "leave")
				);

				if (answer == "accept")
				{
					var order = Enumerable.Range(1, SporeDevices.GetLength(0)).OrderBy(_ => GameRandom.Get().Next()).ToArray();
					character.Variables.Perm.SetString(SporeOrderVar, string.Join("", order));
					character.Variables.Perm.SetInt(SporeStepVar, 0);

					character.Quests.Start(Sq060);

					await dialog.Msg(L("As long as the device for breeding the spores is there, it will keep happening. This... This experiment can never happen again... Never."));
					await dialog.Msg(L("If the power sources are removed, the spores will die off. The thing is, if you don't do it in the right order, it will activate again."));
					await dialog.Msg(L("The bracken spore breeding device is inside the Sotras Chapel. Don't worry if you fail... Just keep trying to find the right sequence."));
				}
				return;
			}

			if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
			{
				await dialog.Msg(L("The people of our village must be worried for us, still living here at the Novaha Monastery."));

				var answer = await dialog.SelectQuestOffer(Hq1, L("Especially Anne, who was always with me... She must be so worried..."),
					Option(L("I'll deliver the news."), "accept"),
					Option(L("I have other issues to tend to."), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("I need to write Anne a letter. Please wait."));
					character.ServerMessage(L("Rose is writing a letter."));
					await dialog.Msg(L("Here. Please deliver this letter to Anne."));

					character.Quests.Start(Hq1);
					character.Inventory.Add(ItemId.ABBAY64_3_HIDDENQ1_ITEM1, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (character.Quests.IsActive(Sq060))
			{
				await dialog.Msg(L("I guess the mind control wasn't all too bad. All we need to do now is remove the power sources and the spores will die."));
				return;
			}

			if (character.Quests.IsActive(Hq1))
			{
				await dialog.Msg(L("Anne is very kind and thoughtful, I'm sure she's even more worried about us."));
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				character.Quests.ReplayQuestTrack(Mq040);
				return;
			}

			switch (GameRandom.Get().Next(3))
			{
				case 0: await dialog.Msg(L("It's alright. We found the townfolk and I am reunited with my brother... I am really happy even in this state.")); break;
				case 1: await dialog.Msg(L("I owe the rest of my life to you... and will live the rest of my life to the fullest. My brother and I are qualifiers aren't we? Nothing will happen.")); break;
				default: await dialog.Msg(L("One thing I regret is not being able to return to the village. The merchants have no work there anymore, either. I miss our people already...")); break;
			}
		});

		AddConditionalNpc(153110, L("Edmundas"), "ABBEY643_EDMONDA04", "d_abbey_64_3", -1461, 142, 137, c => c.Quests.Has(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Edmundas"));

			if (character.Quests.IsCompletable(Sq010))
			{
				await dialog.Msg(L("You have been a lifesaver. With the goddesses gone, having you close has given me strength."));
				await dialog.CompleteQuest(Sq010);
				return;
			}

			if (character.Quests.IsCompletable(Sq040))
			{
				await dialog.Msg(L("As long as we're not cured from the spores we can't return to the village. We might just have to live here forever."));
				await dialog.Msg(L("That's okay. I'm just grateful for the chance to be with Rose again."));
				await dialog.Msg(L("Thank you for everything. I don't know if we'll ever meet again but I'm sure both of us will never forget you."));
				await dialog.CompleteQuest(Sq040);
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				await dialog.Msg(L("I may not qualify perfectly, but I'm close. And I think I'm already infected with the spores, too..."));
				await dialog.Msg(L("I'm going to stay with Rose and look after her. But right now I'm worried about the monsters."));

				var answer = await dialog.SelectQuestOffer(Sq010, L("My body still hasn't recovered."),
					Option(L("I'll chase away the monsters at the Medie State Apartments"), "accept"),
					Option(L("I'll do it later"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq010);

				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				await dialog.Msg(L("I can handle the monsters once my body recovers but... I don't think I can take the demons left in the monastery yet."));
				await dialog.Msg(L("But it's not right for me to ask you to clear out all the demons here... It's a tricky situation."));

				var answer = await dialog.SelectQuestOffer(Sq020, L("That's right. There is someone in Orsha called Dejamis who owes me a favor. She is the Wizard Submaster; will you go there and let her know about my situation?"),
					Option(L("I'll go see the Wizard Submaster"), "accept"),
					Option(L("I'll go later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq020);
					character.Quests.CompleteObjective(Sq020, "tellDejamis");

					await dialog.Msg(L("By the way, don't tell anyone but Dejamis about what happened to us. I don't want people to be afraid of us, and I don't want us to be treated like freaks."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq010))
			{
				await dialog.Msg(L("Usually I wouldn't have any trouble clearing out this many monsters... But that's when I'm in better condition. I'm afraid you'll have to..."));
				return;
			}

			if (character.Quests.IsActive(Sq040))
			{
				await dialog.Msg(L("Set it up around here. I feel safe now knowing Dejamis made it for us."));
				return;
			}

			if (character.Quests.Has(Sq020) && !character.Quests.HasCompleted(Sq040))
			{
				await dialog.Msg(L("Did Dejamis intruct you on how to create the Protection Barrier Crystal? Now, we can finally feel safe again!"));
				await dialog.Msg(L("I saw Deadborn Scaps gathered in the Veidra Hall. I know it's a chore but please help me on this!"));
				return;
			}

			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("It is all thanks to you. Being able to meet Rose who is safe... all thanks to you."));
			else
				await dialog.Msg(L("I didn't even dare to hope... that I would be able to meet Rose again. I had merely hoped that she would be spared from being used in the wizard's plans... This all seems like a dream."));
		});

		// The Protection Barrier Crystal
		//-------------------------------------------------------------------------
		AddQuestTrigger("ABBEY643_MAGIC_POINT01", "d_abbey_64_3", -1414.36, 115.37, 60, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Sq040, "placeCrystal") || character.Inventory.CountItem(ItemId.ABBAY643_SQ4_ITEM1) == 0)
				return;

			character.Inventory.RemoveItem(ItemId.ABBAY643_SQ4_ITEM1, 1);
			character.Quests.CompleteObjective(Sq040, "placeCrystal");
			character.LookAround();

			await Task.CompletedTask;
		});

		AddConditionalNpc(103006, L("Protection Barrier Crystal"), "ABBEY643_MAGIC_CRYSTAL", "d_abbey_64_3", -1415.56, 116.93, 90, c => c.Quests.IsCompletable(Sq040) || c.Quests.HasCompleted(Sq040));

		// The Giant Bracken Spore Breeding Device in Sotras Chapel
		//-------------------------------------------------------------------------
		for (var i = 0; i < SporeDevices.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(154025, "UnvisibleName", "ABBEY643_SQ6_DEVICE0" + number, "d_abbey_64_3", SporeDevices[i, 0], SporeDevices[i, 1], SporeDevices[i, 2], async dialog =>
			{
				await this.RemovePowerSource(dialog, number);
			});
		}
	}

	/// <summary>
	/// Removes one of the spore breeding device's power sources, which only
	/// holds when they are removed in the device's own order.
	/// </summary>
	private async Task RemovePowerSource(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Sq060, "removePower"))
			return;

		var order = character.Variables.Perm.GetString(SporeOrderVar, "1234");
		var step = character.Variables.Perm.GetInt(SporeStepVar, 0);

		if (step >= order.Length || order[step] - '0' != number)
		{
			character.Variables.Perm.SetInt(SporeStepVar, 0);
			dialog.Npc.PlayEffect("F_spread_out004_dark", 1f);
			character.ServerMessage(L("The power sources have activated again."));
			return;
		}

		step++;
		character.Variables.Perm.SetInt(SporeStepVar, step);
		dialog.Npc.PlayEffect("F_light018_yellow", 1f);
		character.ServerMessage(LF("Power sources removed: {0}/{1}", step, order.Length));

		if (step >= order.Length)
			character.Quests.CompleteObjective(Sq060, "removePower");

		await Task.CompletedTask;
	}
}

/// <summary>
/// Met once Anne in Knidos Jungle told the character she misses Rose.
/// </summary>
public class Abbey643AnneMissesRosePrerequisite : QuestPrerequisite
{
	public const string VarName = "Gabija.Quests.Abbay643Hq1.AnneMissesRose";

	public override bool Met(Character character)
		=> character.Variables.Perm.GetBool(VarName, false);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50134: Rescue Rose (1)
//-----------------------------------------------------------------------------
public class Abbay643Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50134);
		SetName(L("Rescue Rose (1)"));
		SetDescription(L("Edmundas wants to try and overload the device, but he is not sure what to use for that. For now, try and defeat Brown Hummingbirds and collect any black objects you can obtain, then bring them to Edmundas."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_EDMONDA01", "d_abbey_64_3", L("Follow Edmundas Into the Novaha Institute to rescue Rose"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_EDMONDA01", "d_abbey_64_3", L("Find Black Materials to overload the device"));
		SetPhase(QuestStatus.Success, "ABBEY643_EDMONDA01", "d_abbey_64_3", L("Talk to Edmundas"));

		AddPrerequisite(new QuestStatusPrerequisite(50128, QuestStatus.Completed));

		AddObjective("findMaterials", L("Hand over all the items to Edmundas that can be obtained from the Brown Hummingbird"), new CollectItemObjective("ABBAY643_MQ1_ITEM01", 1));
		AddPityDrop("ABBAY643_MQ1_ITEM01", 0.7f, 3, 1, "humming_bud_purple");

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new TakeItemReward("ABBAY643_MQ1_ITEM01", -1));
	}
}

// 50135: Rescue Rose (2)
//-----------------------------------------------------------------------------
public class Abbay643Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50135);
		SetName(L("Rescue Rose (2)"));
		SetDescription(L("To bring Rose back to safety you need to overload the device and destroy it. Collect as much Brown Hummingbird mucus as you can."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_EDMONDA01", "d_abbey_64_3", L("Talk to Edmundas"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_EDMONDA01", "d_abbey_64_3", L("Collect Brown Hummingbird Mucus"));
		SetPhase(QuestStatus.Success, "ABBEY643_EDMONDA01", "d_abbey_64_3", L("Deliver to Edmundas"));

		AddPrerequisite(new QuestStatusPrerequisite(50134, QuestStatus.Completed));

		AddObjective("collectMucus", L("Defeat Brown Hummingbirds to acquire mucus"), new CollectItemObjective("ABBAY643_MQ1_ITEM01", 6));
		AddPityDrop("ABBAY643_MQ1_ITEM01", 0.7f, 3, 1, "humming_bud_purple");

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new TakeItemReward("ABBAY643_MQ1_ITEM01", -1));
	}
}

// 50136: Rescue Rose (3)
//-----------------------------------------------------------------------------
public class Abbay643Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50136);
		SetName(L("Rescue Rose (3)"));
		SetDescription(L("Rose could be under the influence of the Mind Control Crystals. Obtain the protective barrier from the device inside the Main Hall Atrium and destroy the crystal."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_EDMONDA02", "d_abbey_64_3", L("Talk to Edmundas in the Main Hall Atrium"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_MQ03_DEVICE01", "d_abbey_64_3", L("Destroy the Mind Control Crystals"));
		SetPhase(QuestStatus.Success, "ABBEY643_EDMONDA02", "d_abbey_64_3", L("Talk to Edmundas"));

		AddPrerequisite(new QuestStatusPrerequisite(50135, QuestStatus.Completed));

		AddObjective("destroyCrystals", L("Destroy the Mind Control Crystals"), new VariableCheckObjective(DAbbey643QuestNpcsScript.CrystalCountVar, 5, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50137: Rescue Rose (4)
//-----------------------------------------------------------------------------
public class Abbay643Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50137);
		SetName(L("Rescue Rose (4)"));
		SetDescription(L("The wizard wants to use Rose to spread out the giant bracken spores. Thwart the wizard's plans and rescue Rose at the Kilnuma Oratorium!"));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_EDMONDA03", "d_abbey_64_3", L("Talk to Edmundas nearby the Medie State Apartments"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_EDMONDA04", "d_abbey_64_3", L("Rescue Rose at the Kilnuma Oratorium"));
		SetPhase(QuestStatus.Success, "ABBEY643_ROZE01", "d_abbey_64_3", L("Talk to Traveling Merchant Rose"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ABBAY_64_3_MQ040_TRACK", "m_boss_b", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50136, QuestStatus.Completed));

		AddObjective("killDeathweaver", L("Defeat Deathweaver"), new KillObjective(1, "boss_Deathweaver") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 5));
	}
}

// 50144: The Demons' Goals
//-----------------------------------------------------------------------------
public class Abbay643Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50144);
		SetName(L("The Demons' Goals"));
		SetDescription(L("Traveling Merchant Rose believes she was able to read part of the wizard's mind when she was under mind contol. Listen to her testimony."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_ROZE01", "d_abbey_64_3", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_ROZE01", "d_abbey_64_3", L("Listen to Rose's detailed testimony"));
		SetPhase(QuestStatus.Success, "ABBEY643_ROZE01", "d_abbey_64_3", L("Listen to Rose's detailed testimony"));

		AddPrerequisite(new QuestStatusPrerequisite(50137, QuestStatus.Completed));

		AddObjective("hearTestimony", L("Listen to Rose's detailed testimony"), new ManualObjective());
	}
}

// 50138: Edmundas' Worry (1)
//-----------------------------------------------------------------------------
public class Abbay643Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50138);
		SetName(L("Edmundas' Worry (1)"));
		SetDescription(L("Edmundas is not fully recovered yet, and the monsters are worrying him. Defeat the monsters at Medie State Apartments for Edmundas and Rose."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_EDMONDA04", "d_abbey_64_3", L("Talk to Edmundas"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_EDMONDA04", "d_abbey_64_3", L("Defeat monsters at the Medie State Apartments"));
		SetPhase(QuestStatus.Success, "ABBEY643_EDMONDA04", "d_abbey_64_3", L("Report to Edmundas"));

		AddPrerequisite(new QuestStatusPrerequisite(50144, QuestStatus.Completed));

		AddObjective("killMonsters", L("Defeat monsters at the Medie State Apartments"), new KillObjective(10, "Lapemiter", "Sec_Deadbornscab", "Lapeman", "humming_bud_purple"));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 50139: Edmundas' Worry (2)
//-----------------------------------------------------------------------------
public class Abbay643Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50139);
		SetName(L("Edmundas' Worry (2)"));
		SetDescription(L("According to Edmundas, the Wizard Submaster in Orsha owes him a favor and might be able to help them. Go see the Wizard Submaster in Orsha and explain what happened to Edmundas and Rose."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_3", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_EDMONDA04", "d_abbey_64_3", L("Talk to Edmundas"));
		SetPhase(QuestStatus.InProgress, "JOB_2_WIZARD_MASTER", "c_orsha", L("Relay the situation to Orsha's Wizard Submaster"));
		SetPhase(QuestStatus.Success, "JOB_2_WIZARD_MASTER", "c_orsha", L("Relay the situation to Orsha's Wizard Submaster"));

		AddPrerequisite(new QuestStatusPrerequisite(50138, QuestStatus.Completed));

		AddObjective("tellDejamis", L("Relay the situation to Orsha's Wizard Submaster"), new ManualObjective());
	}
}

// 50143: Giant Bracken Spore
//-----------------------------------------------------------------------------
public class Abbay643Sq060Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50143);
		SetName(L("Giant Bracken Spore"));
		SetDescription(L("Remove the power sources of the giant bracken spore breeding device inside Sotras Chapel. To keep the power sources from reactivating, you will have to remove them in the correct order."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_ROZE01", "d_abbey_64_3", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_SQ6_DEVICE01", "d_abbey_64_3", L("Remove the power sources of the Giant Bracken Spore Breeding Device in Sotras Chapel"));
		SetPhase(QuestStatus.Success, "ABBEY643_ROZE01", "d_abbey_64_3", L("Report to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50144, QuestStatus.Completed));

		AddObjective("removePower", L("Remove the power sources of the Giant Bracken Spore Breeding Device in Sotras Chapel"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50261: Good News
//-----------------------------------------------------------------------------
public class Abbay643Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50261);
		SetName(L("Good News"));
		SetDescription(L("Rose wrote a letter to her village telling people there how she's doing. Take the letter and give it to Anne."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_3", "f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY643_ROZE01", "d_abbey_64_3", L("Talk to Rose"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_TOWN_PEAPLE3", "f_bracken_63_2", L("Deliver Rose's Letter to Anne"));
		SetPhase(QuestStatus.Success, "ABBEY643_ROZE01", "d_abbey_64_3", L("Deliver Anne's Letter to Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50144, QuestStatus.Completed));
		AddPrerequisite(new Abbey643AnneMissesRosePrerequisite());

		AddObjective("deliverLetter", L("Deliver Rose's Letter to Anne"), new CollectItemObjective("ABBAY64_3_HIDDENQ1_ITEM2", 1));

		AddReward(new ItemReward("misc_scrollskulp", 1));
		AddReward(new TakeItemReward("ABBAY64_3_HIDDENQ1_ITEM2", 1));
	}
}
