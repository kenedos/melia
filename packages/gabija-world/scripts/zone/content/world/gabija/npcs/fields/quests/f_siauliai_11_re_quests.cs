//--- Melia Script ----------------------------------------------------------
// Paupys Crossing Quest NPCs
//--- Description -----------------------------------------------------------
// The large-scale search for the priests and the road to the Ashaq
// Underground Prison.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
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

public class FSiauliai11ReQuestNpcsScript : GeneralScript
{
	private readonly static QuestId Siau15reMq06 = new QuestId(60093);
	private readonly static QuestId Mq01 = new QuestId(60099);
	private readonly static QuestId Mq02 = new QuestId(60100);
	private readonly static QuestId Mq03 = new QuestId(60101);
	private readonly static QuestId Mq04 = new QuestId(60102);
	private readonly static QuestId Mq05 = new QuestId(60103);
	private readonly static QuestId Mq06 = new QuestId(60104);
	private readonly static QuestId Sq01 = new QuestId(60105);
	private readonly static QuestId Sq02 = new QuestId(60106);
	private readonly static QuestId Sq03 = new QuestId(60107);
	private readonly static QuestId Sq04 = new QuestId(60108);
	private readonly static QuestId Sq05 = new QuestId(60109);
	private readonly static QuestId Sq07 = new QuestId(60110);
	private readonly static QuestId Sq08 = new QuestId(60111);
	private readonly static QuestId OrshaMq2_03 = new QuestId(60114);
	private readonly static QuestId Prison621Mq01 = new QuestId(60115);
	private readonly static QuestId OrshaMq3_01 = new QuestId(60145);

	public const string TraceCountVar = "Gabija.Quests.Siau11reMq03.Traces";
	private const string TraceVar = "Gabija.Quests.Siau11reMq03.Trace";
	public const string CircleCountVar = "Gabija.Quests.Siau11reSq02.Circles";
	private const string CircleVar = "Gabija.Quests.Siau11reSq02.Circle";
	private const string PackageVar = "Gabija.Quests.Siau11reSq08.Package";

	private const int TracesNeeded = 5;
	private const int CirclesNeeded = 7;

	private static readonly double[,] Traces =
	{
		{ 829.61, 286.25 }, { 1164.41, 986.76 }, { 1059.70, 1289.43 }, { 832.24, 1208.25 }, { 1006.24, 1633.58 },
		{ 1205.67, 1534.96 }, { 794.73, 1414.65 }, { 890.74, 971.18 }, { 705.38, 805.23 }, { 1093.14, 553.19 },
		{ 485.11, 347.22 }, { 692.40, 550.11 }, { 105.08, 777.03 }, { -137.39, 807.91 }, { -372.09, 805.42 },
	};

	private static readonly double[,] Circles =
	{
		{ 2623.82, 388.67 }, { 2642.35, 532.79 }, { 2661.31, 698.50 }, { 2721.05, 793.73 }, { 2807.67, 891.84 },
		{ 2664.54, 920.44 }, { 2525.74, 871.90 }, { 2391.33, 975.19 }, { 2300.06, 1123.16 }, { 2181.92, 1040.43 },
		{ 2278.88, 908.75 }, { 2412.97, 827.84 }, { 2394.86, 679.82 }, { 2266.20, 708.46 }, { 2327.02, 544.92 },
		{ 2271, 424.09 }, { 2404.36, 333.18 }, { 2483.55, 432.15 }, { 2485.08, 589.62 },
	};

	private static readonly double[,] Packages =
	{
		{ -436.15, 334.42 }, { -933.72, -13.78 }, { -1061.26, 220.61 }, { -761.68, 180.57 }, { -1127.23, 1580.91 },
		{ -925.76, 389.25 }, { -27.63, 822.78 }, { -561.90, 798.41 }, { -524.81, -22.30 }, { -274.87, 36.05 },
		{ -1285.28, 1001.27 }, { 48.06, 317.24 }, { -1200.38, 1272.64 }, { -103.09, 195.33 }, { -1452.68, 811.02 },
		{ -954.86, 647.18 }, { -916.65, 871.97 },
	};

	protected override void Load()
	{
		// Chaser Talbasi
		//-------------------------------------------------------------------------
		AddNpc(147399, L("Chaser Talbasi"), "SIAULIAI11RE_TALBASI", "f_siauliai_11_re", 2324.01, -643.35, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Talbasi"));

			if (character.Quests.IsCompletable(Siau15reMq06))
			{
				await dialog.Msg(L("You're the person that the lord's messenger told us about. How is the search in the Woods of the Linked Bridges going?"));
				await dialog.CompleteQuest(Siau15reMq06);
				return;
			}

			if (character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Thank you. It's not all recognizable since they're all ripped apart, but I can sort of make sense of it."));
				await dialog.Msg(L("Demons are hiding in the forest waiting to attack us. A few of us were separated while running away. We... decided to group up."));
				await dialog.Msg(L("Oh my, there should be a location of where they were going to group up... But that part is completely missing."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("You've found the journals of Priest Irma and the bishop? Hm... There are too many parts that I can't get my head around."));
				await dialog.Msg(L("If they were merely being pursued, they could have just escaped to Orsha. Yet they went through the trouble of leaving behind pieces of their journals while disappearing somewhere else."));
				await dialog.Msg(L("It could be... That they were threatened... Or they had a mission to complete even though they were being chased?"));
				await dialog.Msg(L("A mission... I've been looking for traces of Priest Gelija. I cannot make out exactly what is written in the notes left by Gelija either since they are only partially recognizable due to the monsters."));
				await dialog.Msg(L("Priest Pranas said that finding traces of Priest Gelija is very important because of the priest's thorough nature. I also think that we'll be able to narrow down our search if we can find all of the clues left by Gelija."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("I'll look over here. How about you look for clues that the monsters may have?"),
					Option(L("I'll try to find them"), "accept"),
					Option(L("It's going to be hard to find it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("I'm sure that there is some crucial evidence in those notes. It is common for people on the run to leave clues behind so that others will find them."));
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("It seems certain that the priests decided to group up somewhere. Letting Priest Pranas know immediately is fine, but acquiring more definite information would be better."));
				await dialog.Msg(L("I'll keep looking for clues here, you go on to Paslaptis Hideout. It's only recently been found, and there are reports that the campfires are still warm."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("Even a small clue will be a big help in the search."),
					Option(L("Yes, I'll drop by"), "accept"),
					Option(L("Let me get ready first"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("Report to Larena under the watchtower when you've finished your investigation. She's second to none when it comes to finding people, so I'm sure she will manage to find some definitive evidence."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("I was trying to not care too much for Orsha as long as I got paid properly. But seeing the demons made me think about my hometown and how it was razed to the ground..."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("It was the demons that destroyed my hometown, not monsters. Their brutality... Geez, I don't even want to think of it again."));
				await dialog.Msg(L("If the whereabouts of the priests really has to do with demons... I'm sorry, but I don't think that we'll be able to see them ever again."));
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("Impressive. You saved the bishop... Well, I at least get some satisfaction in knowing that I was right."));
				return;
			}

			if (character.Quests.HasCompleted(Mq02))
			{
				await dialog.Msg(L("Tracking has never been this hard before. There are monsters near Orsha that I have never even seen before."));
				return;
			}

			await dialog.Msg(L("There is just too much that I cannot comprehend. I'm just a hired hand trying not to care about Orsha... But it feels like my loss."));
		});

		// Agent Larena
		//-------------------------------------------------------------------------
		AddNpc(151078, L("Agent Larena"), "SIAULIAI11RE_RARENA", "f_siauliai_11_re", 324.91, 789.77, -24, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Agent Larena"));

			if (character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Did Jolly find something? It seems as if the bishop's group was near here after all."));
				await dialog.CompleteQuest(Mq03);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("It must be you. The person the lord sent."));
				await dialog.Msg(L("You said that there are only traces of hurried movements in Paslaptis Hideout, correct? That there was a note saying that they decided to group up somewhere."));
				await dialog.Msg(L("I think that the reason they couldn't return to Orsha at once... Hm... I think it was because they had a mission more than merely being pursued or threatened."));
				await dialog.Msg(L("Maybe that's why Jolly is so excited..."));
				await dialog.Msg(L("Oh, this is Jolly by the way. He's Bishop Urbonas' dog."));
				await dialog.Msg(L("I thought that Jolly would be able to find the priests fast since he was adored by all of them. But it's hard for him to find a scent with all this rain pouring down."));

				var answer = await dialog.SelectQuestOffer(Mq03, L("I have to go back Orsha to report to the lord after meeting with a messenger. Why don't you take Jolly out for a walk?"),
					Option(L("I'll try to find them"), "accept"),
					Option(L("I don't think I can, because of the rain"), "leave")
				);

				if (answer != "accept")
					return;

				var relayed = await character.TimeActions.StartAsync(L("Relaying the investigation..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (relayed != TimeActionResult.Completed)
					return;

				for (var i = 1; i <= Traces.GetLength(0); ++i)
					character.Variables.Perm.Set(TraceVar + i, false);
				character.Variables.Perm.SetInt(TraceCountVar, 0);

				character.Quests.Start(Mq03);
				character.Inventory.Add(ItemId.SIAU11RE_MQ_03_ITEM, 1, InventoryAddType.PickUp);
				character.LookAround();
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Find the priests' traces with Jolly!{nl}If Jolly strays too far, call him back with the whistle"), 8);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("Wait a second. I thought I heard a faint scream from up there..."));
				await dialog.Msg(L("This doesn't feel right. You see, Priest Pranas' group went towards Gatves Highway."));

				var answer = await dialog.SelectQuestOffer(Mq04, L("They're all more than capable of looking after themselves, but still... Would you mind going to Gatves Highway just in case?"),
					Option(L("I will follow it"), "accept"),
					Option(L("It's nothing serious"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Quests.CompleteObjective(Mq04, "meetSendal");
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("I wish we had such a friend to work with. Cute, loving... and best of all, a great tracker."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("I'm not a fighter, so I'll only be more of a burden if I tag along. Go after him quickly, please."));
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("You've begun to become quite a celebrity. Rumors about you that were whispered outside the city can now be heard everywhere."));
				return;
			}

			if (character.Quests.HasCompleted(Mq03))
			{
				await dialog.Msg(L("Please don't tell anyone about the investigations that we are conducting here. There will be an uproar if the refugees coming to Orsha find out."));
				return;
			}

			await dialog.Msg(L("I brought Jolly because I thought he may be able to find clues without trouble... But the rain doesn't seem like it's going to end any time soon."));
		});

		// Jolly
		//-------------------------------------------------------------------------
		AddConditionalNpc(154068, L("Jolly"), "SIAU11RE_MQ_03_DOG", "f_siauliai_11_re", 346.06, 796.86, 21, c => !c.Quests.IsActive(Mq03) || c.Quests.IsCompletable(Mq03));

		// Chaser Sendal
		//-------------------------------------------------------------------------
		AddConditionalNpc(147400, L("Chaser Sendal"), "SIAULIAI11RE_SENDAL", "f_siauliai_11_re", -227.38, 365.47, 9, c => c.Quests.Has(Mq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Sendal"));

			if (character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("We are saved! Are you our backup? About Priest Pranas... He hasn't been able to make it out of Groundsle Hill!"));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("We should have increased the number of guards. I swear that this is the strongest demon I've seen since Medzio Diena!"));

				var answer = await dialog.SelectQuestOffer(Mq05, L("All we could do once we made eye contact was to run. Priest Pranas will be in a lot of danger at this rate!"),
					Option(L("I'll save the day"), "accept"),
					Option(L("I am not ready yet"), "leave")
				);

				if (answer != "accept")
					return;

				var talked = await character.TimeActions.StartAsync(L("Talking..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (talked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq05);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("There was nothing we could do. Please save Priest Pranas before it is too late!"));
				character.Quests.ClearQuestTrack(Mq05);
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("I thought that it was impressive enough that you could take on that demon... The fact that you saved Bishop Urbonas as well is simply outstanding."));
				return;
			}

			if (character.Quests.HasCompleted(Mq05))
			{
				await dialog.Msg(L("I can't believe I left Priest Pranas behind even if I was frightened by the demon... I don't deserve to be called a Chaser."));
				return;
			}

			await dialog.Msg(L("A demon that powerful... I've never seen anything like it. It almost makes me want to quit my job."));
		});

		// Chaser Zegaus
		//-------------------------------------------------------------------------
		AddConditionalNpc(147405, L("Chaser Zegaus"), "SIAULIAI11RE_JEGAUS", "f_siauliai_11_re", -172.45, 348.10, -42, c => c.Quests.Has(Mq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Zegaus"));

			if (character.Quests.IsCompletable(Sq07))
			{
				await dialog.Msg(L("If you hadn't help out, I would have been limping along until they got me. Thank you so much."));
				await dialog.CompleteQuest(Sq07);
				return;
			}

			if (character.Quests.IsCompletable(Sq08))
			{
				await dialog.Msg(L("Thank you so much! They're not in perfect shape, but I'm just glad to have at least some of them back."));
				await dialog.CompleteQuest(Sq08);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq07) && character.Quests.MeetsPrerequisites(Sq07))
			{
				await dialog.Msg(L("Thankfully, Priest Pranas is safe... Now we can afford to worry about ourselves. Having ten lives wouldn't be enough if we stay around here for too long."));
				await dialog.Msg(L("Sendal and I have hurt our legs and there's no telling when monsters or demons will come after us... There's nobody but you to ask for help around here."));

				var answer = await dialog.SelectQuestOffer(Sq07, L("Could you deal with the monsters around here until we gain our strength again?"),
					Option(L("I will protect you"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq07);

				return;
			}

			if (!character.Quests.Has(Sq08) && character.Quests.MeetsPrerequisites(Sq08))
			{
				await dialog.Msg(L("Oh... Whoops. By any chance have you seen some bags or packages around here?"));
				await dialog.Msg(L("I had all of my potions and food in there... With my legs in my current state, you're the only person I can ask for help."));

				var answer = await dialog.SelectQuestOffer(Sq08, L("They shouldn't be far since they weight quite a bit. I know it may be a bit rude, but could you look for our luggage?"),
					Option(L("I will go look for it"), "accept"),
					Option(L("I'm sorry, I have more urgent issues to tend to"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Packages.GetLength(0); ++i)
						character.Variables.Perm.Set(PackageVar + i, false);

					character.Quests.Start(Sq08);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq07))
			{
				await dialog.Msg(L("I should have known there was a reason to pay so much. I would have turned down the offer if I knew it was that dangerous."));
				return;
			}

			if (character.Quests.IsActive(Sq08))
			{
				await dialog.Msg(L("Maybe some monsters have gotten hold of it. This just keeps getting worse. They're quite heavy so they shouldn't be too far away."));
				return;
			}

			if (character.Quests.HasCompleted(Mq05))
			{
				await dialog.Msg(L("I am so glad that Priest Pranas is safe. Rewards aside, I didn't think that surviving an encounter with such a powerful demon would have been possible."));
				return;
			}

			await dialog.Msg(L("I didn't hear anything about there being demons here. I didn't think too much of the job since I was told it was an escort mission..."));
		});

		// Priest Pranas at Groundsle Hill
		//-------------------------------------------------------------------------
		AddConditionalNpc(155044, L("Priest Pranas"), "SIAULIAI11RE_PRANAS", "f_siauliai_11_re", -522.71, 1371.62, 179, IsPranasAtGroundsle, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Pranas"));

			if (character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Whoever you are... I thank you for saving me from the bottom of my heart. Could I perhaps know how you came here, or at least know your name?"));
				await dialog.Msg(L("Oh, you've been sent by the lord. Then you must already know that the bishop is missing..."));
				await dialog.Msg(L("I was trying to deal quickly with a few demons roaming about and investigate the campsite... But then, we suddenly found their leader..."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("I... I can't keep holding on. Please... Search the campsite..."));

				var answer = await dialog.SelectQuestOffer(Mq06, L("I'm sure we'll clear things out..."),
					Option(L("I'll try to find them"), "accept"),
					Option(L("It's better to tend to the injuries first"), "leave")
				);

				if (answer != "accept")
					return;

				var talked = await character.TimeActions.StartAsync(L("Talking..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (talked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq06);
				await dialog.Msg(L("Come to Orsha if you find something. I'll leave the rest up to you..."));
				character.LookAround();
				return;
			}

			await dialog.Msg(L("I... I can't keep holding on. Please... Search the campsite..."));
		});

		// The priests' camp on Groundsle Hill
		//-------------------------------------------------------------------------
		AddNpc(153041, "UnVisibleName", "f_siauliai_11_re", -648.85, 1493.35, 269);
		AddNpc(46011, "UnVisibleName", "f_siauliai_11_re", -600.18, 1494.17, 90);
		AddNpc(147375, "UnVisibleName", "f_siauliai_11_re", -641.78, 1569.67, 19);
		AddNpc(147375, "UnVisibleName", "f_siauliai_11_re", -544.97, 1575.93, -7);

		AddConditionalNpc(46212, L("Leftover Box"), "SIAU11RE_MQ_06_NPC", "f_siauliai_11_re", -574.47, 1513.11, 180, c => c.Quests.IsActive(Mq06) && !c.Quests.IsCompletable(Mq06), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			character.Inventory.Add(ItemId.SIAU11RE_MQ_06_ITEM, 1, InventoryAddType.PickUp);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("You discovered a locked box. Bring it to Priest Pranas in Orsha."), 5);
			character.LookAround();

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAU11RE_MQ_05_NPC", "f_siauliai_11_re", -334.50, 1177, 50, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.StartQuestTrack(Mq05);

			await Task.CompletedTask;
		});

		// Paslaptis Hideout
		//-------------------------------------------------------------------------
		AddNpc(147375, "UnVisibleName", "f_siauliai_11_re", 1591.09, 585.37, 30);
		AddNpc(147375, "UnVisibleName", "f_siauliai_11_re", 1717.20, 509.72, 270);

		AddNpc(147312, L("Burnt Memo"), "SIAU11RE_MQ_02_NPC_01", "f_siauliai_11_re", 1566.05, 469.15, 90, async dialog =>
		{
			await this.CheckHideout(dialog, "checkMemo", L("(The writing is hard to make out since it's smudged from the rain.)"));
		});

		AddNpc(154060, L("Extinguished Bonfire"), "SIAU11RE_MQ_02_NPC_02", "f_siauliai_11_re", 1633.73, 522.15, 90, async dialog =>
		{
			await this.CheckHideout(dialog, "checkBonfire", L("(There are traces of a fire having been hastily put out.)"));
		});

		AddNpc(47160, L("Abandoned Bag"), "SIAU11RE_MQ_02_NPC_03", "f_siauliai_11_re", 1703.21, 331.35, 90, async dialog =>
		{
			await this.CheckHideout(dialog, "checkBag", L("(These are the objects of the priests... It seems like they took only a few important things and left.)"));
		});

		// The priests' traces around the watchtower
		//-------------------------------------------------------------------------
		for (var i = 0; i < Traces.GetLength(0); ++i)
		{
			var number = i + 1;

			AddQuestTrigger("SIAU11RE_MQ_03_TRACE_" + number, "f_siauliai_11_re", Traces[i, 0], Traces[i, 1], 60, async args =>
			{
				if (args.Initiator is not Character character)
					return;

				if (!character.Quests.IsActive(Mq03) || character.Quests.IsCompletable(Mq03) || character.Variables.Perm.GetBool(TraceVar + number, false))
					return;

				character.Variables.Perm.Set(TraceVar + number, true);
				var found = character.Variables.Perm.GetInt(TraceCountVar, 0) + 1;
				character.Variables.Perm.SetInt(TraceCountVar, found);

				character.ServerMessage(LF("Jolly found a trace of the priests: {0}/{1}", Math.Min(found, TracesNeeded), TracesNeeded));

				await Task.CompletedTask;
			});
		}

		// Agent Orwen
		//-------------------------------------------------------------------------
		AddNpc(151079, L("Agent Orwen"), "SIAULIAI11RE_ORWEN", "f_siauliai_11_re", 2366.02, -501.56, -26, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Agent Orwen"));

			if (character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("You've taken care of that? I envy your skills."));
				await dialog.Msg(L("There, now I'll get to get back to my work. Thanks a lot."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("What purpose do the magic circles serve? I don't think there have been any incident reports yet... We'll have to wait and see."));
				await dialog.CompleteQuest(Sq02);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("There are a lot of disgusting monsters around here. No wonder nothing can be achieved in this state."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("If you have some spare time, could you deal with some of the monsters around here?"),
					Option(L("I'll help you"), "accept"),
					Option(L("I don't think it'll be of much use"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq01);

				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				await dialog.Msg(L("I've never heard of demons being spotted anywhere near Orsha before... All of a sudden, demons appear and they're poking around this place."));
				await dialog.Msg(L("They've made a bunch of magic circles near Naudingas Felled Area. I don't know why they've done that but it can't be a good thing."));

				var answer = await dialog.SelectQuestOffer(Sq02, L("It's not my responsibility, but I do think that it is better to be safe than sorry. Could you take care of the magic circles at Naudingas Felled Area?"),
					Option(L("I will get rid of it"), "accept"),
					Option(L("Tell her that there is a more emergent issue"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Circles.GetLength(0); ++i)
						character.Variables.Perm.Set(CircleVar + i, false);
					character.Variables.Perm.SetInt(CircleCountVar, 0);

					character.Quests.Start(Sq02);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("It wasn't going great even before the monsters started interrupting my work. It was such a wonderful day until it began raining..."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("I've been here to survey the land because of the refugee problem. I don't remember seeing any demons back then... What is going on?"));
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("I didn't pay too much attention to the tall tales from outside the walls... But you seem to be the real deal."));
				return;
			}

			await dialog.Msg(L("All I'm going to do after this is take a break and sleep all day while wrapped in my warm blanket."));
		});

		// Ominous Magic Circles
		//-------------------------------------------------------------------------
		for (var i = 0; i < Circles.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(154065, L("Ominous Magic Circle"), "SIAU11RE_SQ_02_NPC_" + number, "f_siauliai_11_re", Circles[i, 0], Circles[i, 1], 90,
				character => character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02) && !character.Variables.Perm.GetBool(CircleVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02) || character.Variables.Perm.GetBool(CircleVar + number, false))
						return;

					character.Variables.Perm.Set(CircleVar + number, true);
					var removed = character.Variables.Perm.GetInt(CircleCountVar, 0) + 1;
					character.Variables.Perm.SetInt(CircleCountVar, removed);

					character.ServerMessage(LF("Ominous magic circles removed: {0}/{1}", Math.Min(removed, CirclesNeeded), CirclesNeeded));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Agent Notres
		//-------------------------------------------------------------------------
		AddNpc(20060, L("Agent Notres"), "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", 1488.02, -557.63, 8, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Agent Notres"));

			if (character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("You're saying that there was an enormous monster at the camp? I haven't seen it myself, but we can't make people stay in places like that."));
				await dialog.Msg(L("Right? I'll take your word for it and put it in my report."));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("What is this disgusting stench? I don't know how they could have ordered me to collect this stuff. They know very well that it would have been impossible..."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("There is a lot of nonsense going on because all reports of the area are labeled as classified. I mean, they're planning on making a refugee camp near Deer Hooves Lot."));
				await dialog.Msg(L("Even if the refugees coming here are supposed to be from a village famed for mercenaries. What are they thinking? Saying that they'll be able to fend for themselves if we give them houses... There are demons everywhere..."));
				await dialog.Msg(L("They want a report on whether the monsters here are weak enough for the mercenaries to handle or not. I used to be a paper pusher, what am I supposed to do?"));

				var answer = await dialog.SelectQuestOffer(Sq03, L("I've heard that you're quite skilled. I can't fake a report, so how about checking Deer Hooves Lot for me?"),
					Option(L("I'll try and check it out"), "accept"),
					Option(L("Go look for a safer place"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq03);

				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				await dialog.Msg(L("This is just wrong. I came here as a surveyor, not to fight monsters like the trackers. That's too much for me."));

				var answer = await dialog.SelectQuestOffer(Sq05, L("The monsters may have become more violent because of the demons, so gather some monster blood. ...Are you kidding me? Could you help me out? You'd literally be saving my life."),
					Option(L("I'll collect it"), "accept"),
					Option(L("Tell her that there is a more emergent issue"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("By the goddesses, thank you so much. Just gather some blood from the monsters at Rohonsa Cliff."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq03) || character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("If they're going to keep the investigations a secret, they should at least write a line and keep to it. How much more unlucky can I get? I haven't held a sword since training camp..."));
				character.Quests.ClearQuestTrack(Sq04);
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("I'm not a coward. I just know what I can and can't do."));
				await dialog.Msg(L("My skills aren't with a sword or spear, they're honed with a pen and stamps..."));
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("I was wondering who it was that saved Bishop Urbonas... It was you! Goodness..."));
				return;
			}

			await dialog.Msg(L("I can't handle things like this, but I'd become a deserter if I run off... I'm going to seriously consider resigning after this is over. I don't know what I'd do to make a living though."));
		});

		// The empty camp at Deer Hooves Lot
		//-------------------------------------------------------------------------
		AddNpc(147375, "UnVisibleName", "f_siauliai_11_re", 1447.41, -1330.49, 20);
		AddNpc(153041, "UnVisibleName", "f_siauliai_11_re", 1383.05, -1305.29, -72);
		AddNpc(153041, "UnVisibleName", "f_siauliai_11_re", 1376.28, -1320.15, 217);
		AddNpc(47161, "UnvisibleName", "f_siauliai_11_re", 1316, -1300, 90);
		AddNpc(46011, "UnVisibleName", "f_siauliai_11_re", 1356.68, -1373.77, 90);

		AddQuestTrigger("SIAU11RE_SQ_04_NPC", "f_siauliai_11_re", 1274.11, -1367.16, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq04) && !character.Quests.IsCompletable(Sq04))
				character.Quests.StartQuestTrack(Sq04);

			await Task.CompletedTask;
		});

		// Lost Packages
		//-------------------------------------------------------------------------
		for (var i = 0; i < Packages.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47160, L("Lost Package"), "SIAU11RE_SQ_08_NPC_" + number, "f_siauliai_11_re", Packages[i, 0], Packages[i, 1], 90,
				character => character.Quests.IsActive(Sq08) && !character.Quests.IsCompletable(Sq08) && !character.Variables.Perm.GetBool(PackageVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq08) || character.Quests.IsCompletable(Sq08) || character.Variables.Perm.GetBool(PackageVar + number, false))
						return;

					character.Variables.Perm.Set(PackageVar + number, true);
					character.Inventory.Add(ItemId.SIAU11RE_SQ_08_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Priest Pranas and the Chasers at Gebene Cliff
		//-------------------------------------------------------------------------
		AddConditionalNpc(155044, L("Priest Pranas"), "SIAULIAI11RE_PRANAS_1", "f_siauliai_11_re", 393.54, 1658.51, 2, IsPartyAtTheCliff, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Pranas"));

			if (character.Quests.IsCompletable(OrshaMq2_03))
			{
				await dialog.Msg(L("You're here. I've gathered the best of the best, but this aura... It is sinister beyond my knowledge."));
				await dialog.CompleteQuest(OrshaMq2_03);
				return;
			}

			if (!character.Quests.Has(Prison621Mq01) && character.Quests.MeetsPrerequisites(Prison621Mq01))
			{
				await dialog.Msg(L("I can sense the ominous aura streaming out from the direction of the Underground Prison. We may not be able to make it back out of there once we enter."));

				var answer = await dialog.SelectQuestOffer(Prison621Mq01, L("But we must save the bishop! Let's enter the Ashaq Underground Prison if you've finished your preparations."),
					Option(L("Let's see inside"), "accept"),
					Option(L("I am not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Prison621Mq01);
					await dialog.Msg(L("I will first leave with the chasers. Please follow us right away."));
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("I can sense the ominous aura streaming out from the direction of the Underground Prison. We may not be able to make it back out of there once we enter."));
		});

		AddConditionalNpc(147403, L("Chaser Torvana"), "SIAU11RE_TORNAVA", "f_siauliai_11_re", 346, 1655.49, 70, IsPartyAtTheCliff);
		AddConditionalNpc(147406, L("Chaser Daramaus"), "SIAU11RE_DARAMAUS", "f_siauliai_11_re", 432.95, 1680.07, 4, IsPartyAtTheCliff);
	}

	/// <summary>
	/// Returns whether Priest Pranas is at Groundsle Hill, rescued but
	/// not yet carried back to Orsha.
	/// </summary>
	private static bool IsPranasAtGroundsle(Character character)
		=> character.Quests.IsCompletable(Mq05) || (character.Quests.HasCompleted(Mq05) && !character.Quests.Has(Mq06));

	/// <summary>
	/// Returns whether Pranas' party waits at Gebene Cliff before entering
	/// the prison.
	/// </summary>
	private static bool IsPartyAtTheCliff(Character character)
		=> character.Quests.Has(OrshaMq2_03) && !character.Quests.Has(Prison621Mq01);

	/// <summary>
	/// Checks one of the traces left at Paslaptis Hideout.
	/// </summary>
	private async Task CheckHideout(Dialog dialog, string objectiveIdent, string observation)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Mq02, objectiveIdent))
			return;

		await dialog.Msg(observation);
		character.Quests.CompleteObjective(Mq02, objectiveIdent);
	}

	/// <summary>
	/// Blows Jolly's whistle, whose bark tells how close the priests'
	/// nearest trace is.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_SIAU11RE_MQ_03_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_siauliai_11_re" || !character.Quests.IsActive(Mq03) || character.Quests.IsCompletable(Mq03))
		{
			character.ServerMessage(L("Jolly doesn't answer the whistle."));
			return ItemUseResult.OkayNotConsumed;
		}

		var closest = double.MaxValue;
		for (var i = 0; i < Traces.GetLength(0); ++i)
		{
			if (character.Variables.Perm.GetBool(TraceVar + (i + 1), false))
				continue;

			var distance = character.Position.Get2DDistance(new Position((float)Traces[i, 0], character.Position.Y, (float)Traces[i, 1]));
			closest = Math.Min(closest, distance);
		}

		if (closest < 250)
			character.ServerMessage(L("Jolly barks excitedly. The scent is very close."));
		else if (closest < 600)
			character.ServerMessage(L("Jolly sniffs the ground. There is a faint scent nearby."));
		else
			character.ServerMessage(L("Jolly can't pick up any scent here."));

		return ItemUseResult.OkayNotConsumed;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60099: Large-Scale Search Operation (1)
//-----------------------------------------------------------------------------
public class Siau11reMq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60099);
		SetName(L("Large-Scale Search Operation (1)"));
		SetDescription(L("Chaser Talbasi believes they can figure out what is going on once all of Priest Gelija's notes are collected. Defeat monsters at the Uninhabited Crossing and try to collect all of Priest Gelija's memos."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_TALBASI", "f_siauliai_11_re", L("Talk with Chaser Talbasi"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_TALBASI", "f_siauliai_11_re", L("Collect the memos of Priest Gelija"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_TALBASI", "f_siauliai_11_re", L("Deliver to Chaser Talbasi"));

		AddPrerequisite(new QuestStatusPrerequisite(60093, QuestStatus.Completed));

		AddObjective("collectMemos", L("Collect Priest Gelija's Memos"), new CollectItemObjective("SIAU11RE_MQ_01_ITEM", 4));
		AddPityDrop("SIAU11RE_MQ_01_ITEM", 0.7f, 3, 1, "Sec_Popolion_Blue", "Sec_Hanaming", "Sec_Onion_Red", "woodin");

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 30));
		AddReward(new TakeItemReward("SIAU11RE_MQ_01_ITEM", -1));
	}
}

// 60100: Large-Scale Search Operation (2)
//-----------------------------------------------------------------------------
public class Siau11reMq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60100);
		SetName(L("Large-Scale Search Operation (2)"));
		SetDescription(L("Chaser Talbasi believes it's best to obtain more concrete clues before finding Priest Pranas. Search the recently discovered Paslaptis Hideout before meeting up with Agent Larena."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_TALBASI", "f_siauliai_11_re", L("Talk with Chaser Talbasi"));
		SetPhase(QuestStatus.InProgress, "SIAU11RE_MQ_02_NPC_02", "f_siauliai_11_re", L("Search the Paslaptis Hideout"));
		SetPhase(QuestStatus.Success, "SIAU11RE_MQ_02_NPC_02", "f_siauliai_11_re", L("Search the Paslaptis Hideout"));

		AddPrerequisite(new QuestStatusPrerequisite(60099, QuestStatus.Completed));

		AddObjective("checkMemo", L("Check the burnt memo"), new ManualObjective());
		AddObjective("checkBonfire", L("Check the bonfire"), new ManualObjective());
		AddObjective("checkBag", L("Check the scattered belongings"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 35));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The client names no turn-in NPC; Larena picks up the search.
		character.Quests.Complete(this.QuestId);
	}
}

// 60101: Large-Scale Search Operation (3)
//-----------------------------------------------------------------------------
public class Siau11reMq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60101);
		SetName(L("Large-Scale Search Operation (3)"));
		SetDescription(L("Agent Larena believes Bishop Urbonas and the priests are on the move with a specific purpose. Take Jolly, a dog raised by the priests, and try to find their traces nearby."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_RARENA", "f_siauliai_11_re", L("Talk to Agent Larena"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_RARENA", "f_siauliai_11_re", L("Find the priests' traces with Jolly"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_RARENA", "f_siauliai_11_re", L("Talk to Agent Larena"));

		AddPrerequisite(new QuestStatusPrerequisite(60100, QuestStatus.Completed));

		AddObjective("findTraces", L("Search for traces of the priests with Jolly"), new VariableCheckObjective(FSiauliai11ReQuestNpcsScript.TraceCountVar, 5, isPermanent: true));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 35));
		AddReward(new ItemReward("Drug_SP1_Q", 15));
		AddReward(new TakeItemReward("SIAU11RE_MQ_03_ITEM", -1));
	}
}

// 60102: Large-Scale Search Operation (4)
//-----------------------------------------------------------------------------
public class Siau11reMq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60102);
		SetName(L("Large-Scale Search Operation (4)"));
		SetDescription(L("Chaser Sendal looks gravely injured. Talk to Chaser Sendal and find out what happened."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_RARENA", "f_siauliai_11_re", L("Talk to Agent Larena"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_SENDAL", "f_siauliai_11_re", L("Talk with Chaser Sendal"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_SENDAL", "f_siauliai_11_re", L("Talk with Chaser Sendal"));

		AddPrerequisite(new QuestStatusPrerequisite(60101, QuestStatus.Completed));

		AddObjective("meetSendal", L("Talk with Chaser Sendal"), new ManualObjective());
	}
}

// 60103: Large-Scale Search Operation (5)
//-----------------------------------------------------------------------------
public class Siau11reMq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60103);
		SetName(L("Large-Scale Search Operation (5)"));
		SetDescription(L("Chaser Sendal and Priest Pranas encountered some demons while investigating Groundsle Hill. Chaser Sendal now wants you to help rescue Priest Pranas from the demons."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_SENDAL", "f_siauliai_11_re", L("Talk with Chaser Sendal"));
		SetPhase(QuestStatus.InProgress, "SIAU11RE_MQ_05_NPC", "f_siauliai_11_re", L("Rescue Priest Pranas from the demons at Groundsle Hill"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_PRANAS", "f_siauliai_11_re", L("Talk with Priest Pranas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU11RE_MQ_05_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60102, QuestStatus.Completed));

		AddObjective("killSpecter", L("Defeat Specter Monarch"), new KillObjective(1, "boss_Spector_m_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 40));
		AddReward(new ItemReward("TreasureboxKey2", 1));
	}
}

// 60104: Large-Scale Search Operation (6)
//-----------------------------------------------------------------------------
public class Siau11reMq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60104);
		SetName(L("Large-Scale Search Operation (6)"));
		SetDescription(L("Priest Pranas has asked you to find traces of Bishop Urbonas and return to Orsha. Search Groundsle Hill for any leads on Bishop Urbonas."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_11_re", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_PRANAS", "f_siauliai_11_re", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "SIAU11RE_MQ_06_NPC", "f_siauliai_11_re", L("Look for the traces of Bishop Urbonas"));
		SetPhase(QuestStatus.Success, "C_ORSHA_PRANAS", "c_orsha", L("Talk to Priest Pranas in Orsha"));

		AddPrerequisite(new QuestStatusPrerequisite(60103, QuestStatus.Completed));

		AddObjective("findBox", L("Look for the traces of Bishop Urbonas"), new CollectItemObjective("SIAU11RE_MQ_06_ITEM", 1));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 35));
		AddReward(new TakeItemReward("SIAU11RE_MQ_06_ITEM", -1));
	}
}

// 60105: Support Activities
//-----------------------------------------------------------------------------
public class Siau11reSq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60105);
		SetName(L("Support Activities"));
		SetDescription(L("Agent Orwen wants you to defeat the monsters disturbing the agents' operations at the Naudingas Felled Area."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_ORWEN", "f_siauliai_11_re", L("Talk with Agent Orwen"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_ORWEN", "f_siauliai_11_re", L("Defeat the monsters nearby"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_ORWEN", "f_siauliai_11_re", L("Report to Agent Orwen"));

		AddPrerequisite(new LevelPrerequisite(8));

		AddObjective("killMonsters", L("Defeat the nearby monsters"), new KillObjective(8, "Sec_Popolion_Blue", "Sec_Hanaming", "Sec_Onion_Red", "woodin"));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 30));
	}
}

// 60106: The Suspicious Location
//-----------------------------------------------------------------------------
public class Siau11reSq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60106);
		SetName(L("The Suspicious Location"));
		SetDescription(L("Agent Orwen says the demons have created a series of unknown magic circles. Remove the magic circles created by the demons at the Naudingas Felled Area."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_ORWEN", "f_siauliai_11_re", L("Talk with Agent Orwen"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_ORWEN", "f_siauliai_11_re", L("Remove the Ominous Magic Circles"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_ORWEN", "f_siauliai_11_re", L("Report to Agent Orwen"));

		AddPrerequisite(new LevelPrerequisite(8));

		AddObjective("removeCircles", L("Remove the Ominous Magic Circles"), new VariableCheckObjective(FSiauliai11ReQuestNpcsScript.CircleCountVar, 7, isPermanent: true));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 30));
	}
}

// 60107: Preliminary Investigation (1)
//-----------------------------------------------------------------------------
public class Siau11reSq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60107);
		SetName(L("Preliminary Investigation (1)"));
		SetDescription(L("Agent Notres thinks it doesn't make sense to move the people migrating to Orsha to the Deer Hooves Lot. First, they want you to check whether it would be possible to defeat the monsters there."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", L("Talk with Agent Notres"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", L("Defeat the monsters at the Deer Hooves Lot"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", L("Defeat the monsters at the Deer Hooves Lot"));

		AddPrerequisite(new LevelPrerequisite(8));

		AddObjective("killMonsters", L("Defeat the monsters at the Deer Hooves Lot"), new KillObjective(9, "Sec_Popolion_Blue", "Sec_Hanaming", "Sec_Onion_Red", "woodin"));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 30));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The client names no turn-in NPC; the camp check follows on its own.
		character.Quests.Complete(this.QuestId);
	}
}

// 60108: Preliminary Investigation (2)
//-----------------------------------------------------------------------------
public class Siau11reSq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60108);
		SetName(L("Preliminary Investigation (2)"));
		SetDescription(L("Agent Notres has asked you to check whether the Deer Hooves Lot can be used to make a temporary settler camp. Go there and see whether it is safe."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "SIAU11RE_SQ_04_NPC", "f_siauliai_11_re", L("Check out at the Deer Hooves Lot"));
		SetPhase(QuestStatus.InProgress, "SIAU11RE_SQ_04_NPC", "f_siauliai_11_re", L("Check out at the Deer Hooves Lot"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", L("Talk with Agent Notres"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU11RE_SQ_04_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60107, QuestStatus.Completed));

		AddObjective("killGolem", L("Defeat Gray Golem"), new KillObjective(1, "boss_golem_Gray_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 40));
		AddReward(new ItemReward("TreasureboxKey2", 1));
	}
}

// 60109: The Corrupted Monster
//-----------------------------------------------------------------------------
public class Siau11reSq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60109);
		SetName(L("The Corrupted Monster"));
		SetDescription(L("Agent Notres was sent to collect blood from monsters in order to find out what is making them so ferocious. Defeat some monsters at Rohonsa Cliff and help collect their contaminated blood."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", L("Talk with Agent Notres"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", L("Collect Corrupted Blood"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_NOTORESU", "f_siauliai_11_re", L("Deliver to Agent Notres"));

		AddPrerequisite(new LevelPrerequisite(8));

		AddObjective("collectBlood", L("Collect Corrupted Blood"), new CollectItemObjective("SIAU11RE_SQ_05_ITEM", 7));
		AddPityDrop("SIAU11RE_SQ_05_ITEM", 0.7f, 3, 1, "Sec_Popolion_Blue", "Sec_Hanaming", "Sec_Onion_Red", "woodin");

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 35));
		AddReward(new TakeItemReward("SIAU11RE_SQ_05_ITEM", -1));
	}
}

// 60110: Precious Life and Money
//-----------------------------------------------------------------------------
public class Siau11reSq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60110);
		SetName(L("Precious Life and Money"));
		SetDescription(L("While Chaser Zegaus is glad Priest Pranas is safe, now they are injured and worried for their own safety. Defeat some monsters for Zegaus."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_JEGAUS", "f_siauliai_11_re", L("Talk with Chaser Zegaus"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_JEGAUS", "f_siauliai_11_re", L("Defeat the nearby monsters"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_JEGAUS", "f_siauliai_11_re", L("Talk with Chaser Zegaus"));

		AddPrerequisite(new QuestStatusPrerequisite(60103, QuestStatus.Completed));

		AddObjective("killMonsters", L("Defeat the nearby monsters"), new KillObjective(13, "Sec_Popolion_Blue", "Sec_Hanaming", "Sec_Onion_Red", "woodin"));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 30));
	}
}

// 60111: While You Were Gone
//-----------------------------------------------------------------------------
public class Siau11reSq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60111);
		SetName(L("While You Were Gone"));
		SetDescription(L("Chaser Zegaus seems to have lost a bag with medicine and bandages. Look around and search for the trackers' belongings."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_JEGAUS", "f_siauliai_11_re", L("Talk with Chaser Zegaus"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_JEGAUS", "f_siauliai_11_re", L("Collect Chaser Zegaus' Belongings"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_JEGAUS", "f_siauliai_11_re", L("Deliver to Chaser Zegaus"));

		AddPrerequisite(new QuestStatusPrerequisite(60103, QuestStatus.Completed));

		AddObjective("collectPackages", L("Collect Chaser Zegaus' Belongings"), new CollectItemObjective("SIAU11RE_SQ_08_ITEM", 8));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 30));
		AddReward(new TakeItemReward("SIAU11RE_SQ_08_ITEM", -1));
	}
}

// 60115: Bishop Urbonas' Whereabouts (1)
//-----------------------------------------------------------------------------
public class Prison621Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60115);
		SetName(L("Bishop Urbonas' Whereabouts (1)"));
		SetDescription(L("Priest Pranas says he will enter Ashaq Underground Prison 1F first with the Chasers. Follow Priest Pranas to Ashaq Underground Prison 1F."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_11_re", "d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI11RE_PRANAS_1", "f_siauliai_11_re", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_MQ_01_NPC", "d_prison_62_1", L("Follow Priest Pranas"));
		SetPhase(QuestStatus.Success, "PRISON621_PRANAS", "d_prison_62_1", L("Talk with Priest Pranas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON621_MQ_01_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60114, QuestStatus.Completed));

		AddObjective("followPranas", L("Follow Priest Pranas"), new ManualObjective());
	}
}
