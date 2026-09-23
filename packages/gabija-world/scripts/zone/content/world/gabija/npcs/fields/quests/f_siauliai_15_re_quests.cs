//--- Melia Script ----------------------------------------------------------
// Woods of the Linked Bridges Quest NPCs
//--- Description -----------------------------------------------------------
// The lord's agents and hired Chasers searching the woods for the bishop.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
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

public class FSiauliai15ReQuestNpcsScript : GeneralScript
{
	private readonly static QuestId OrshaMq1_04 = new QuestId(60088);
	private readonly static QuestId Mq01 = new QuestId(60089);
	private readonly static QuestId Mq02 = new QuestId(60090);
	private readonly static QuestId Mq03 = new QuestId(60091);
	private readonly static QuestId Mq05 = new QuestId(60092);
	private readonly static QuestId Mq06 = new QuestId(60093);
	private readonly static QuestId Sq01 = new QuestId(60094);
	private readonly static QuestId Sq02 = new QuestId(60095);
	private readonly static QuestId Sq03 = new QuestId(60096);
	private readonly static QuestId Sq04 = new QuestId(60097);
	private readonly static QuestId Sq05 = new QuestId(60098);
	private readonly static QuestId Hq1 = new QuestId(50270);
	private readonly static QuestId OrshaMq3_01 = new QuestId(60145);

	public const string MorenGaugeVar = "Gabija.Quests.Siau15reMq02.Gauge";
	private const string GreeneryVar = "Gabija.Quests.Siau15reMq03.Greenery";
	public const string CartCountVar = "Gabija.Quests.Siau15reSq02.Burnt";
	private const string CartVar = "Gabija.Quests.Siau15reSq02.Cart";
	public const string StimulantCountVar = "Gabija.Quests.Siau15reSq03.Used";
	public const string KillCountVar = "Gabija.Quests.Siauliai15Hq1.Kills";

	private const int MorenGaugeMax = 100;
	private const int MorenGaugePerKill = 10;
	private const int StimulantUses = 8;
	private const int KillsForReport = 1000;

	private static readonly Position MorenPosition = new Position(359.79f, 878.23f, -132.58f);

	private static readonly double[,] Greenery =
	{
		{ -803.38, -159.40 }, { -816.87, 67.42 }, { -961.63, -72.42 }, { -353.55, -635.11 }, { -272.69, -796.70 },
		{ 90.54, -758.99 }, { 108.25, -583.53 }, { -126.87, -584.42 }, { -187.72, -1265.54 }, { -286.92, -1481.44 },
		{ 111.94, -1667.02 }, { 180.72, -1497.49 }, { 494.25, 182.02 }, { 651.13, -205.40 }, { 235.64, -217.81 },
		{ 428.65, -362.63 }, { 790.59, -436.24 },
	};

	private static readonly double[,] Carts =
	{
		{ 45313, 1918.81, -1444.30, 229 }, { 45312, 1838.49, -1711.99, -31 }, { 45312, 1861.47, -2060.06, -15 },
		{ 45312, 2174.90, -2170.48, -36 }, { 45311, 2279.44, -1912.46, 4 }, { 45311, 2260.12, -1601.97, 183 },
		{ 45311, 2571.77, -1315.73, 222 }, { 45311, 2192.24, -665.26, 178 }, { 45311, 1854.75, -1276.16, -3 },
		{ 45311, 1921.69, -1013.47, -30 },
	};

	private static readonly double[,] NestProps =
	{
		{ 47204, -1135.92, 1290.13, 90 }, { 47204, -1186.55, 1299.60, 90 }, { 47204, -1181.76, 1323.29, 90 }, { 47204, -1132.40, 1350.08, 90 },
		{ 47204, -1101.17, 1328.40, 90 }, { 47204, -1064.64, 1283.66, 90 }, { 47204, -1054.51, 1322.53, 90 }, { 47204, -1107.34, 1354.49, 90 },
		{ 103034, -1150.48, 1313.67, -1 }, { 103034, -1165.31, 1333.46, -5 }, { 103034, -1132.75, 1310.94, 32 },
	};

	protected override void Load()
	{
		// Agent Cherasia
		//-------------------------------------------------------------------------
		AddNpc(20059, L("Agent Cherasia"), "SIAULIAI15RE_CHERASIA", "f_siauliai_15_re", 3095.08, 99.48, 77, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Agent Cherasia"));

			if (character.Quests.IsCompletable(OrshaMq1_04))
			{
				await dialog.Msg(L("I've heard about you from the lord. Glad to meet you. I'm Cherasia."));
				await dialog.Msg(L("I've been searching for clues about the bishop and the priests around here."));
				await dialog.CompleteQuest(OrshaMq1_04);
				return;
			}

			if (character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("It's hard to recognize thanks to all the monster saliva... But it seems as if they were being chased by something."));
				await dialog.Msg(L("You should hurry. Take this and go find Agent Moren at Gyvenimo Crossroads."));
				await dialog.Msg(L("I'll stay here and try to find any more clues."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("I'm sure you've heard of Priest Pranas from the lord. Pranas only recently returned from his asceticism from the north."));
				await dialog.Msg(L("He recently found the traces of Priest Irma, one of the bishop's disciples. There were pieces of what seemed to be Irma's diary ripped apart."));
				await dialog.Msg(L("From the way they've been ripped apart... It looks like the monsters did this. I was left here while Priest Pranas and the rest went to look for more clues."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("I've been trying my best to gather the remaining pages from the diary, but it seems to be a bit too much to do by myself. You'll have to lend a hand if we want to cover more ground while searching."),
					Option(L("I'll try to find them"), "accept"),
					Option(L("I think it'll be too late already"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq01);

				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("It's unnatural for monsters to tear apart something as unattractive as a diary. My guess is... that somebody did this on purpose."));
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("I've heard that Bishop Urbonas made it safely back to Orsha. But we still have something to deal with before we go. I am talking about the demons of course."));
				return;
			}

			if (character.Quests.HasCompleted(Mq01))
			{
				await dialog.Msg(L("That's strange. An abundance of evidence, but not one core piece."));
				return;
			}

			await dialog.Msg(L("There probably won't be a anything more important to Orsha than what we are doing now. This is worrying. No matter how hard I look, there is one piece of information that is missing..."));
		});

		// Agent Moren
		//-------------------------------------------------------------------------
		AddNpc(20060, L("Agent Moren"), "SIAULIAI15RE_MOREN", "f_siauliai_15_re", 359.79, -132.58, 251, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Agent Moren"));

			if (character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Paper burned by something... There's definitely something written on it. Why did they only burn some of it?"));
				await dialog.Msg(L("They've covered the campfire with leaves after dousing it. Not something that you would normally do. It has a great risk of catching fire."));
				await dialog.Msg(L("Maybe they did this to try to secretly notify us of something."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsCompletable(Mq03))
			{
				dialog.PlayAnimation("event_sit_2");
				await dialog.Msg(L("Good. Let's gather the notes and see what's written on them."));
				await dialog.Msg(L("A revelation? The bishop of Orsha... Revelator..?"));
				await dialog.Msg(L("I have no idea what they mean with the 'Revelator'... Do you know anything about this?"));
				await dialog.Msg(L("I see you don't have a clue either. Maybe Pranas knows something about this... Ulysses has gone with Priest Pranas so look for him up north."));
				await dialog.Msg(L("By the way, there are also other hired trackers around here besides Pranas and the other agents. I'd appreciate it if you could help them out as much as you can."));
				await dialog.CompleteQuest(Mq03);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				dialog.PlayAnimation("event_sit_2");
				await dialog.Msg(L("I've received word from the lord. You're the person here to help us right?"));
				await dialog.Msg(L("The journal Cherasia found... The fact that Irma is being chased most likely means that the bishop is also being chased."));
				await dialog.Msg(L("Priest Pranas asked me to check the traces we've found here. Then he went up with the rest of the trackers."));
				await dialog.Msg(L("This is where the bishop and the priests stayed for a while... The monsters are turning any investigation into a nightmare."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("I'd like to ask you to deal with the monsters so that I can get some investigation done."),
					Option(L("Leave it to me"), "accept"),
					Option(L("I don't think I can do that"), "leave")
				);

				if (answer == "accept")
				{
					var journals = character.Inventory.CountItem(ItemId.SIAU15RE_MQ_01_1_ITEM);
					if (journals > 0)
						character.Inventory.Remove(ItemId.SIAU15RE_MQ_01_1_ITEM, journals, InventoryItemRemoveMsg.Given);

					character.Variables.Perm.SetInt(MorenGaugeVar, 0);
					character.Quests.Start(Mq02);
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Protect Agent Moren from the monsters while he investigates the traces!"), 8);
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				dialog.PlayAnimation("event_sit_2");
				await dialog.Msg(L("Cherasia said that Priest Irma was being chased by something? If so, then these half-burnt notes may have been burned to hide something from her pursuers."));
				await dialog.Msg(L("Whatever that may have been, there is a need to check these more thoroughly. Let me give it a try since it is my expertise after all."));

				var answer = await dialog.SelectQuestOffer(Mq03, L("Please look for more clues while I take a look at these. You may find some more burnt notes if you go through all of the smoking leaves around here."),
					Option(L("I'll try to find them"), "accept"),
					Option(L("I will think about it little more"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Greenery.GetLength(0); ++i)
						character.Variables.Perm.Set(GreeneryVar + i, false);

					character.Quests.Start(Mq03);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("I think just a bit more will do... I'm trying my best to get all the evidence I can, so don't worry."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				dialog.PlayAnimation("event_sit_2");
				await dialog.Msg(L("There's no way people on the run could have had the time to make campfires all over the place like this. Do you think it's part of a charade?"));
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("Cleaning up the mess is part of our job as well. Besides, we are barely handling the influx of refugees into Orsha, we won't be able to handle demons at the same time."));
				return;
			}

			if (character.Quests.HasCompleted(Mq03))
			{
				await dialog.Msg(L("Of course the refugee situation in Orsha is important... There will be mass confusion if word of what we are doing now gets out."));
				return;
			}

			await dialog.Msg(L("If only I had a bit more time... I think I can find it. I'm not in the intelligence business for nothing you know."));
		});

		// Chaser Ulysses
		//-------------------------------------------------------------------------
		AddNpc(147406, L("Chaser Ulysses"), "SIAULIAI15RE_YEULIS", "f_siauliai_15_re", -2625.06, -67, -10, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Ulysses"));

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("Priest Pranas? He's gone to Paupys Crossing."));
				await dialog.Msg(L("He left me in charge of the investigation here with a very concerned look. He asked me to look for clues about the whereabouts of the priests near Greate Stone Face Hill."));
				await dialog.Msg(L("There are definitely traces of someone staying there for a while... But there are simply too many monsters for me to handle by myself."));

				var answer = await dialog.SelectQuestOffer(Mq05, L("I've heard that you're quite good. Before you head off to Priest Pranas, could you reconnoiter Greate Stone Face Hill?"),
					Option(L("I will check it"), "accept"),
					Option(L("I'll follow Pranas"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("A Minotaur... Are you telling me that there are demons? By the goddesses, demons this close to Orsha?"));
				await dialog.Msg(L("The bishop's diary? What are you talking about? It seems as if I am not the only person that thinks something dangerous is afoot..."));

				var answer = await dialog.SelectQuestOffer(Mq06, L("Forget about me, you should head to Paupys Crossing as quickly as you can. My friend Talbasi is searching that region with Priest Pranas, so you should look for Talbasi."),
					Option(L("I'll go right away"), "accept"),
					Option(L("I still have other things to do"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					character.Quests.CompleteObjective(Mq06, "meetTalbasi");
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Revelator? I don't know what you're talking about. Do you perhaps mean the people like the Bokors or Oracles?"));
				character.Quests.ClearQuestTrack(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Demons appearing this close to Orsha is no small incident. There have never been any demons even if half of Orsha had been destroyed on Medzio Diena."));
				await dialog.Msg(L("My mother will have a heart attack if she ever finds out that I'm tracking demons..."));
				return;
			}

			if (character.Quests.HasCompleted(OrshaMq3_01))
			{
				await dialog.Msg(L("I don't know if I am willing to keep working as a Chaser after this. I mean, demons..."));
				return;
			}

			if (character.Quests.HasCompleted(Mq06))
			{
				await dialog.Msg(L("I may simply be working for the lord of Orsha for the money... But I am fully aware of how important the current situation is."));
				return;
			}

			await dialog.Msg(L("It seems that... I am not the only one that feels something is wrong."));
		});

		// Agent Pierneef
		//-------------------------------------------------------------------------
		AddNpc(20060, L("Agent Pierneef"), "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", 2356.71, -1051.38, 7, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Agent Pierneef"));

			if (character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("It's less of a hassle to deal with two monsters than three of them, and one monster is easier than two. Thank you so much."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("At least I won't be responsible for any more monsters in my area for a while. Thank you for helping out."));
				await dialog.CompleteQuest(Sq02);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("The monsters are pouring in... There's not enough manpower... They don't want to give us a break do they?"));

				var answer = await dialog.SelectQuestOffer(Sq01, L("Oh, I'm helping behind the scenes to make it easier for the others. I'm dealing with monsters in Zbuka Inner Court, but they just keep coming and tired me out."),
					Option(L("Can I help you with anything?"), "accept"),
					Option(L("Get some rest and be careful"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("Eh, you're really going to help? For once I'm the one being helped instead and can't help out too. I think you really are sent by the goddesses."));
					await dialog.Msg(L("I'll let you deal with the monsters in Zbuka Inner Court while I rest a little. They are more vicious than they look, so be careful."));
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				await dialog.Msg(L("The soldiers took care of most of the monsters heading towards Orsha but... They've collected them and piled them together on those carts."));

				var answer = await dialog.SelectQuestOffer(Sq02, L("It stinks, and even more monsters have come to run amok since they've been left like that. Sorry, but could you possibly burn those carts down while I rest a little?"),
					Option(L("I'll take care of it"), "accept"),
					Option(L("That's not for me; I have a weak stomach"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Carts.GetLength(0); ++i)
						character.Variables.Perm.Set(CartVar + i, false);
					character.Variables.Perm.SetInt(CartCountVar, 0);

					character.Quests.Start(Sq02);
					character.Inventory.Add(ItemId.SIAU15RE_SQ_02_ITEM, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
			{
				await dialog.Msg(L("Did you defeat 1000 monsters? Their numbers seem to have reduced a little, but they're still many."));
				await dialog.Msg(L("I should send a report to Orsha. I'll collect some more information."));

				var answer = await dialog.SelectQuestOffer(Hq1, L("Would you be so kind as to deliver the report to Orsha for me?"),
					Option(L("I'll bring the report to the Lord."), "accept"),
					Option(L("You ought to report to her yourself."), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq1);
					character.Inventory.Add(ItemId.SIAULIAI15_HIDDENQ1_ITEM, 1, InventoryAddType.PickUp);
					character.Quests.CompleteObjective(Hq1, "deliverReport");
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("There's a limit to what one can do alone. At this rate, I wonder if an army might be more suited to dealing with this..."));
				await dialog.Msg(L("As you know... There are barely enough soldiers in Orsha to protect the settlers."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Forget the monsters going wild because of the stench, the fact that they're gathering near Orsha is definitely not natural. I don't know why they are acting like that. Do you think there may be demons somewhere?"));
				return;
			}

			if (character.Quests.IsActive(Hq1))
			{
				await dialog.Msg(L("The monsters keep coming. Even if we defeat a thousand, will it be enough to reduce their numbers?"));
				await dialog.Msg(L("In any case, we don't really have the military power to deal with this in Orsha..."));
				return;
			}

			await dialog.Msg(L("Dispatched officers used to irk us by poking around before the kingdom fell... I miss those days now."));
		});

		// Chaser Germeja
		//-------------------------------------------------------------------------
		AddNpc(147404, L("Chaser Germeja"), "SIAULIAI15RE_GERMEYA", "f_siauliai_15_re", -5.04, -2246.31, -1, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Germeja"));

			if (character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("What do you think? Isn't it really effective?"));
				await dialog.Msg(L("I dreamt of becoming an alchemist before all of this happened. I only learned it by watching over the shoulders of others so this is actually a failure, but I guess it's all good if you can find a use for it."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("Hey there, how's it going? It appears that you've also received requests from that person over there..."));
				await dialog.Msg(L("Do you want something really useful? It's a Monster Stimulant. Monsters sprayed with it will become hostile and attack other nearby monsters."));

				var answer = await dialog.SelectQuestOffer(Sq03, L("Why don't you give it a try? It will let you pass on some of the more irksome work."),
					Option(L("Thank you"), "accept"),
					Option(L("Thank you but I have to decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Variables.Perm.SetInt(StimulantCountVar, 0);
					character.Quests.Start(Sq03);
					character.Inventory.Add(ItemId.SIAU15RE_SQ_03_ITEM, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Medzio Diena is a terrible memory for everyone. My hometown had been destroyed on that fateful day, and I'm living off these odd jobs in Orsha."));
				return;
			}

			await dialog.Msg(L("I feel as if I am experiencing every type of pain I could ever experience in here. At least I'll have plenty of tales to tell my children when I return."));
		});

		// Chaser Raitis
		//-------------------------------------------------------------------------
		AddNpc(147421, L("Chaser Raitis"), "SIAULIAI15RE_RIGITESS", "f_siauliai_15_re", 1318.29, 391.31, 64, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Raitis"));

			if (character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("You burned the nest to the ground? A wise decision!"));
				await dialog.Msg(L("The reason monsters kept pouring out from Bonan Forest Road is probably because of that one. There are even monsters I'm seeing for the first time since the surrounding areas are all forests."));
				await dialog.CompleteQuest(Sq05);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				await dialog.Msg(L("What in the world... I've never seen anything like it. I went up Bonan Forest Road because it felt as if the monsters were pouring out from there."));
				await dialog.Msg(L("There was a monster that I'd never seen before. It even made a nest for itself."));

				var answer = await dialog.SelectQuestOffer(Sq04, L("I was so startled that I ran away... Can't you do something about it? I'm sure that it'll threaten Orsha if we just leave it there."),
					Option(L("I'll get rid of the nest"), "accept"),
					Option(L("I'd rather not have to do anything dangerous"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("If you really don't mind, please deal with the monster on Bonan Forest Road. There's nothing good that could come from leaving monsters there anyways."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq04) || character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("I would have tried to do something myself if it was just a few monsters, but something of that size... Just run if you think it's too much for you."));
				character.Quests.ClearQuestTrack(Sq05);
				return;
			}

			if (character.Quests.HasCompleted(Sq05))
			{
				await dialog.Msg(L("There are so many monsters even near Orsha... Can you imagine what it is like further away?"));
				return;
			}

			await dialog.Msg(L("I remember back home when they were first looking for Chaser volunteers. I joined up because I trusted Germeja, but the job is more demanding than I thought it would be."));
			await dialog.Msg(L("Not to mention extremely dangerous as well."));
		});

		// The camp on Greate Stone Face Hill
		//-------------------------------------------------------------------------
		AddNpc(147375, "UnvisibleName", "f_siauliai_15_re", -3078.28, 744.79, 90);
		AddConditionalNpc(147375, "UnvisibleName", "SIAU15RE_MQ_05_TENT", "f_siauliai_15_re", -3011.20, 913.91, 45, IsTentStanding);
		AddNpc(154060, "UnvisibleName", "f_siauliai_15_re", -2916.66, 812.88, 90);
		AddNpc(154060, "UnvisibleName", "f_siauliai_15_re", 337.68, -126.85, 90);

		AddQuestTrigger("SIAU15RE_MQ_05_NPC", "f_siauliai_15_re", -2893.04, 414.37, 80, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.StartQuestTrack(Mq05);

			await Task.CompletedTask;
		});

		// Unknown Diary
		//-------------------------------------------------------------------------
		AddConditionalNpc(147312, L("Unknown Diary"), "SIAU15RE_MQ_05_ITEM", "f_siauliai_15_re", -3007.42, 905.02, -13, c => c.Quests.IsCompletable(Mq05), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsCompletable(Mq05))
				return;

			var read = await character.TimeActions.StartAsync(L("Checking..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(1.5));
			if (read != TimeActionResult.Completed)
				return;

			dialog.SetTitle(L("Urbonas' Diary"));
			await dialog.Msg(L("Four years ago, Medzio Diena... ...but that was only the beginning."));
			await dialog.Msg(L("It was the beginning of a whole new calamity. We've been preparing ourselves for hundreds of years now, and we will finish our mission."));
			await dialog.Msg(L("I only hope that Orsha will be safe until my priests and I can finish our mission... ...sent by the goddess..."));
			await dialog.Msg(L("(You cannot recognize any more information.)"));
			await dialog.CompleteQuest(Mq05);
			character.LookAround();
		});

		// Smoky Greenery
		//-------------------------------------------------------------------------
		for (var i = 0; i < Greenery.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(154061, L("Smoky Greenery"), "SIAU15RE_MQ_03_NPC_" + number, "f_siauliai_15_re", Greenery[i, 0], Greenery[i, 1], 90,
				character => character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03) && !character.Variables.Perm.GetBool(GreeneryVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Mq03) || character.Quests.IsCompletable(Mq03) || character.Variables.Perm.GetBool(GreeneryVar + number, false))
						return;

					character.Variables.Perm.Set(GreeneryVar + number, true);
					character.Inventory.Add(ItemId.SIAU15RE_MQ_03_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Smelly Carts
		//-------------------------------------------------------------------------
		for (var i = 0; i < Carts.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc((int)Carts[i, 0], L("Smelly Cart"), "SIAU15RE_SQ_02_NPC_" + number, "f_siauliai_15_re", Carts[i, 1], Carts[i, 2], Carts[i, 3],
				character => !character.Quests.HasCompleted(Sq02) && !character.Variables.Perm.GetBool(CartVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					dialog.SetTitle(L("Smelly Cart"));

					if (!character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02) || character.Variables.Perm.GetBool(CartVar + number, false))
					{
						await dialog.Msg(L("(It reeks.)"));
						return;
					}

					character.Variables.Perm.Set(CartVar + number, true);
					var burnt = character.Variables.Perm.GetInt(CartCountVar, 0) + 1;
					character.Variables.Perm.SetInt(CartCountVar, burnt);

					dialog.Npc.PlayEffect("F_burstup001_fire", 1f);
					character.ServerMessage(LF("Smelly carts burned: {0}/{1}", Math.Min(burnt, 5), 5));
					character.LookAround();
				});
		}

		// The nest on Bonan Forest Road
		//-------------------------------------------------------------------------
		AddConditionalNpc(103034, L("Unidentified Nest"), "SIAU15RE_SQ_05_NPC", "f_siauliai_15_re", -1159.16, 1301.43, -32, IsNestStanding, async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.IsActive(Sq05) && !character.Quests.IsCompletable(Sq05))
				character.Quests.StartQuestTrack(Sq05);

			await Task.CompletedTask;
		});

		for (var i = 0; i < NestProps.GetLength(0); ++i)
			AddConditionalNpc((int)NestProps[i, 0], "UnvisibleName", "SIAU15RE_SQ_05_PROP_" + (i + 1), "f_siauliai_15_re", NestProps[i, 1], NestProps[i, 2], NestProps[i, 3], IsNestStanding);
	}

	/// <summary>
	/// Returns whether the tent the Minotaur tears down is still standing.
	/// </summary>
	private static bool IsTentStanding(Character character)
		=> !character.Quests.HasCompleted(Mq05) && !character.Quests.IsCompletable(Mq05);

	/// <summary>
	/// Returns whether the unknown nest on Bonan Forest Road still stands.
	/// </summary>
	private static bool IsNestStanding(Character character)
		=> !character.Quests.HasCompleted(Sq05) && !character.Quests.IsCompletable(Sq05);

	/// <summary>
	/// Sprays the Monster Stimulant on the monster in front of the player.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_SIAU15RE_SQ_03_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_siauliai_15_re" || !character.Quests.IsActive(Sq03) || character.Quests.IsCompletable(Sq03))
		{
			character.ServerMessage(L("There is no need to use the stimulant right now."));
			return ItemUseResult.OkayNotConsumed;
		}

		var target = character.Map.GetAttackableEnemiesInPosition(character, character.Position, 150).FirstOrDefault();
		if (target == null)
		{
			character.ServerMessage(L("There are no suitable targets nearby."));
			return ItemUseResult.OkayNotConsumed;
		}

		target.PlayEffect("F_spread_out004_dark", 1f);

		var used = character.Variables.Perm.GetInt(StimulantCountVar, 0) + 1;
		character.Variables.Perm.SetInt(StimulantCountVar, used);
		character.ServerMessage(LF("Monsters stimulated: {0}/{1}", Math.Min(used, StimulantUses), StimulantUses));

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Fills Moren's investigation while the player fights around him and
	/// counts the monsters defeated in the woods for Pierneef's report.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Attacker is not Character character || args.Target is not Mob mob)
			return;

		if (character.Map.ClassName != "f_siauliai_15_re")
			return;

		if (!character.Quests.Has(Hq1) && !character.Quests.HasCompleted(Hq1))
		{
			var kills = character.Variables.Perm.GetInt(KillCountVar, 0);
			if (kills < KillsForReport)
				character.Variables.Perm.SetInt(KillCountVar, kills + 1);
		}

		if (!character.Quests.IsActive(Mq02) || character.Quests.IsCompletable(Mq02))
			return;

		if (mob.Position.Get2DDistance(MorenPosition) > 300)
			return;

		var gauge = Math.Min(MorenGaugeMax, character.Variables.Perm.GetInt(MorenGaugeVar, 0) + MorenGaugePerKill);
		character.Variables.Perm.SetInt(MorenGaugeVar, gauge);
		character.ServerMessage(LF("Agent Moren's investigation: {0}%", gauge));
	}
}

/// <summary>
/// Met once the character defeated enough monsters in the Woods of the
/// Linked Bridges.
/// </summary>
public class Siauliai15KillCountPrerequisite : QuestPrerequisite
{
	public override bool Met(Character character)
		=> character.Variables.Perm.GetInt(FSiauliai15ReQuestNpcsScript.KillCountVar, 0) >= 1000;
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50270: Monsters after Monsters
//-----------------------------------------------------------------------------
public class Siauliai15Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50270);
		SetName(L("Monsters after Monsters"));
		SetDescription(L("Agent Pierneef wants you to deliver the activity report to the Lord of Orsha."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_15_re", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", L("Talk with Chaser Pierneef"));
		SetPhase(QuestStatus.InProgress, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk to the Lord of Orsha"));
		SetPhase(QuestStatus.Success, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk to the Lord of Orsha"));

		AddPrerequisite(new Siauliai15KillCountPrerequisite());

		AddObjective("deliverReport", L("Talk to the Lord of Orsha"), new ManualObjective());

		AddReward(new ItemReward("misc_silverbar", 1));
		AddReward(new TakeItemReward("SIAULIAI15_HIDDENQ1_ITEM", 1));
	}
}

// 60089: The Missing Bishop (5)
//-----------------------------------------------------------------------------
public class Siau15reMq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60089);
		SetName(L("The Missing Bishop (5)"));
		SetDescription(L("Agent Cherasia seems to have found part of Priest Irma's journal while looking for the missing bishop. Try and recover the rest of the journal from monsters nearby."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_CHERASIA", "f_siauliai_15_re", L("Talk with Agent Cherasia"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_CHERASIA", "f_siauliai_15_re", L("Collect Priest Irma's Journal Pages"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_CHERASIA", "f_siauliai_15_re", L("Deliver to Agent Cherasia"));

		AddPrerequisite(new QuestStatusPrerequisite(60088, QuestStatus.Completed));

		AddObjective("collectPages", L("Collect Priest Irma's Journal Pages from monsters nearby"), new CollectItemObjective("SIAU15RE_MQ_01_ITEM", 4));
		AddPityDrop("SIAU15RE_MQ_01_ITEM", 0.6f, 3, 1, "Sec_Jukopus", "Onion_green", "Sec_Pokubu", "Sec_arburn_pokubu");

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("SIAU15RE_MQ_01_1_ITEM", 1));
		AddReward(new ItemReward("Vis", 25));
		AddReward(new TakeItemReward("SIAU15RE_MQ_01_ITEM", -1));
	}
}

// 60090: The Burnt Whereabouts (1)
//-----------------------------------------------------------------------------
public class Siau15reMq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60090);
		SetName(L("The Burnt Whereabouts (1)"));
		SetDescription(L("Protect Agent Moren while he investigates. The more monsters you defeat in the area, the faster the agent will finish his investigation."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_MOREN", "f_siauliai_15_re", L("Talk with Agent Moren"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_MOREN", "f_siauliai_15_re", L("Help Agent Moren"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_MOREN", "f_siauliai_15_re", L("Talk with Agent Moren"));

		AddPrerequisite(new QuestStatusPrerequisite(60089, QuestStatus.Completed));

		AddObjective("protectMoren", L("Protect Moren while he analyzes the information"), new VariableCheckObjective(FSiauliai15ReQuestNpcsScript.MorenGaugeVar, 100, isPermanent: true));

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Vis", 30));
	}
}

// 60091: The Burnt Whereabouts (2)
//-----------------------------------------------------------------------------
public class Siau15reMq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60091);
		SetName(L("The Burnt Whereabouts (2)"));
		SetDescription(L("Agent Moren is convinced there might be other clues around. Look for leads in piles of grass with smoke coming out."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_MOREN", "f_siauliai_15_re", L("Talk with Agent Moren"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_MOREN", "f_siauliai_15_re", L("Collect clues from the Smoking Piles of Grass"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_MOREN", "f_siauliai_15_re", L("Deliver to Agent Moren"));

		AddPrerequisite(new QuestStatusPrerequisite(60090, QuestStatus.Completed));

		AddObjective("collectNotes", L("Collect clues from the Smoking Piles of Grass"), new CollectItemObjective("SIAU15RE_MQ_03_ITEM", 5));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 30));
		AddReward(new SelectItemReward("SWD02_115", "STF02_110", "TBW02_112", "MAC02_111"));
		AddReward(new TakeItemReward("SIAU15RE_MQ_03_ITEM", -1));
	}
}

// 60092: Imminent Danger (1)
//-----------------------------------------------------------------------------
public class Siau15reMq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60092);
		SetName(L("Imminent Danger (1)"));
		SetDescription(L("Priest Pranas instructed Chaser Ulysses to investigate, but he finds it difficult because of the monsters. Go and investigate the Greate Stone Face Hill for them."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_YEULIS", "f_siauliai_15_re", L("Talk with Chaser Ulysses"));
		SetPhase(QuestStatus.InProgress, "SIAU15RE_MQ_05_NPC", "f_siauliai_15_re", L("Investigate the Greate Stone Face Hill"));
		SetPhase(QuestStatus.Success, "SIAU15RE_MQ_05_ITEM", "f_siauliai_15_re", L("Read the Unknown Diary"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU15RE_MQ_05_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60091, QuestStatus.Completed));

		AddObjective("killMinotaur", L("Defeat Minotaur"), new KillObjective(1, "boss_Minotaurs_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("SIAU15RE_MQ_05_ITEM", 1));
		AddReward(new ItemReward("Vis", 40));
	}
}

// 60093: Imminent Danger (2)
//-----------------------------------------------------------------------------
public class Siau15reMq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60093);
		SetName(L("Imminent Danger (2)"));
		SetDescription(L("Chaser Ulysses is worried about the demons and thinks it's best for you to go and help Priest Pranas instead. Go to Paupys Crossing and talk to Chaser Talbasi."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_15_re", "f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_YEULIS", "f_siauliai_15_re", L("Deliver to Chaser Ulysses"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_TALBASI", "f_siauliai_11_re", L("Talk to Chaser Talbasi at Paupys Crossing"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_TALBASI", "f_siauliai_11_re", L("Talk to Chaser Talbasi at Paupys Crossing"));

		AddPrerequisite(new QuestStatusPrerequisite(60092, QuestStatus.Completed));

		AddObjective("meetTalbasi", L("Talk to Chaser Talbasi at Paupys Crossing"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("Vis", 15));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
		AddReward(new TakeItemReward("SIAU15RE_MQ_05_ITEM", -1));
	}
}

// 60094: Overworked Agent
//-----------------------------------------------------------------------------
public class Siau15reSq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60094);
		SetName(L("Overworked Agent"));
		SetDescription(L("Agent Pierneef is helping other agents investigate the area. Defeat some monsters at the Zbuka Inner Court."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", L("Talk with Chaser Pierneef"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", L("Defeat the monsters at the Zbuka Inner Court"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", L("Report to Agent Pierneef"));

		AddPrerequisite(new LevelPrerequisite(3));

		AddObjective("killKepa", L("Defeat Green Kepa"), new KillObjective(5, "Onion_green"));
		AddObjective("killPokubu", L("Defeat Arburn Pokubu"), new KillObjective(1, "Sec_arburn_pokubu"));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 30));
	}
}

// 60095: Removing the Odor
//-----------------------------------------------------------------------------
public class Siau15reSq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60095);
		SetName(L("Removing the Odor"));
		SetDescription(L("Agent Pierneef says the smell of defeated monsters is making other monsters go wild. Go to the Zbuka Inner Court and eliminate the smelly cart there."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", L("Talk with Chaser Pierneef"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", L("Get rid of the smelly carts"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_FERNIFF", "f_siauliai_15_re", L("Report to Agent Pierneef"));

		AddPrerequisite(new LevelPrerequisite(3));

		AddObjective("burnCarts", L("Get rid of the smelly carts"), new VariableCheckObjective(FSiauliai15ReQuestNpcsScript.CartCountVar, 5, isPermanent: true));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 30));
		AddReward(new TakeItemReward("SIAU15RE_SQ_02_ITEM", -1));
	}
}

// 60096: Turning Away
//-----------------------------------------------------------------------------
public class Siau15reSq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60096);
		SetName(L("Turning Away"));
		SetDescription(L("Chaser Germeja wants to give the monsters a stimulant drug that makes them fight each other. Use the stimulant on monsters."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_GERMEYA", "f_siauliai_15_re", L("Talk with Chaser Germeja"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_GERMEYA", "f_siauliai_15_re", L("Use the stimulant on the monsters"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_GERMEYA", "f_siauliai_15_re", L("Talk with Chaser Germeja"));

		AddPrerequisite(new LevelPrerequisite(3));

		AddObjective("useStimulant", L("Use the stimulant on the monsters"), new VariableCheckObjective(FSiauliai15ReQuestNpcsScript.StimulantCountVar, 8, isPermanent: true));

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Vis", 30));
		AddReward(new TakeItemReward("SIAU15RE_SQ_03_ITEM", -1));
	}
}

// 60097: Monster Colony (1)
//-----------------------------------------------------------------------------
public class Siau15reSq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60097);
		SetName(L("Monster Colony (1)"));
		SetDescription(L("Chaser Raitis says they saw an unknown monster making a nest at Bonan Forest Road. Go destroy the nest and defeat some monsters there."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI15RE_RIGITESS", "f_siauliai_15_re", L("Talk with Chaser Raitis"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_RIGITESS", "f_siauliai_15_re", L("Defeat the monsters at Bonan Forest Road"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_RIGITESS", "f_siauliai_15_re", L("Defeat the monsters at Bonan Forest Road"));

		AddPrerequisite(new LevelPrerequisite(3));

		AddObjective("killMonsters", L("Defeat the monsters at Bonan Forest Road"), new KillObjective(12, "Sec_Jukopus", "Onion_green", "Sec_Pokubu", "Sec_arburn_pokubu"));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 30));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The client names no turn-in NPC; the nest quest follows on its own.
		character.Quests.Complete(this.QuestId);
	}
}

// 60098: Monster Colony (2)
//-----------------------------------------------------------------------------
public class Siau15reSq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60098);
		SetName(L("Monster Colony (2)"));
		SetDescription(L("You have defeated the monsters in Bonan Forest Road. Keep going and destroy the unknown monster nest."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "SIAU15RE_SQ_05_NPC", "f_siauliai_15_re", L("Destroy the Monster Nest in Bonan Forest Road"));
		SetPhase(QuestStatus.InProgress, "SIAU15RE_SQ_05_NPC", "f_siauliai_15_re", L("Destroy the Monster Nest in Bonan Forest Road"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_RIGITESS", "f_siauliai_15_re", L("Report to Chaser Raitis"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU15RE_SQ_05_TRACK", "m_boss_c", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60097, QuestStatus.Completed));

		AddObjective("killChafer", L("Defeat the Chafer that suddenly attacked"), new KillObjective(1, "boss_Chafer_Q4") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Vis", 40));
	}
}
