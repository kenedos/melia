//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 2F Quest NPCs
//--- Description -----------------------------------------------------------
// The priests hiding from Marnox, Priest Irma's rescue and the second
// cursed idol.
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

public class DPrison622QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Prison622Mq01 = new QuestId(60126);
	private readonly static QuestId Mq02 = new QuestId(60127);
	private readonly static QuestId Mq03 = new QuestId(60128);
	private readonly static QuestId Mq04 = new QuestId(60129);
	private readonly static QuestId Mq05 = new QuestId(60130);
	private readonly static QuestId Mq06 = new QuestId(60131);
	private readonly static QuestId Sq01 = new QuestId(60132);
	private readonly static QuestId Sq02 = new QuestId(60133);
	private readonly static QuestId Sq03 = new QuestId(60134);
	private readonly static QuestId Sq04 = new QuestId(60135);
	private readonly static QuestId Rp1 = new QuestId(60153);
	private readonly static QuestId OrshaMq3_01 = new QuestId(60145);

	public const string IdolEnergyVar = "Gabija.Quests.Prison622Mq02.Energy";
	public const string CrystalPowerVar = "Gabija.Quests.Prison622Mq05.Power";
	private const string CrystalVar = "Gabija.Quests.Prison622Mq05.Crystal";
	private const string PowderVar = "Gabija.Quests.Prison622Sq01.Powder";
	public const string TrapKillsVar = "Gabija.Quests.Prison622Sq04.Lured";
	private const string TrapVar = "Gabija.Quests.Prison622Sq04.Trap";
	private const string FaultyTrapVar = "Gabija.Quests.Prison622Rp1.Trap";

	private const int IdolEnergyNeeded = 8;
	private const int CrystalPowerNeeded = 15;
	private const int CrystalPowerPerCrystal = 3;
	private const int TrapKillsNeeded = 3;

	private static readonly Position IdolPosition = new Position(-1182.26f, 546.48f, -49.25f);
	private static readonly string[] IdolDemons = { "Sec_Bat", "Sec_goblin_archer_blue", "Goblin_Spear_blue", "Sec_escape_wendigo" };

	private static readonly double[,] Crystals =
	{
		{ -739.34, 1452.03 }, { -623.95, 1452.46 }, { -510.37, 1454.15 }, { -391.84, 1455.39 }, { -737.25, 1654.78 },
		{ -613.46, 1652.77 }, { -496.48, 1653.60 }, { -386.66, 1653.41 }, { -1783.97, 1485.76 }, { -1779.82, 1644.67 },
		{ -1773.10, 1801.82 }, { 307.82, 1241.85 }, { 446.90, 1243.90 }, { 192.54, 1500.05 },
	};

	private static readonly double[,] Powder =
	{
		{ 1381.13, 639.66 }, { 1527.68, 683.64 }, { 1227.37, 719.05 }, { 1283.84, 891.14 }, { 1256.37, 1064.56 }, { 1460.99, 1073.76 },
		{ 1399.11, 855.73 }, { 1509.47, 881.64 }, { 1371.42, 1204.19 }, { 1371.06, 1405.20 }, { 1250.56, 1378.57 }, { 1113.36, 1357.81 },
	};

	private static readonly double[,] Traps =
	{
		{ -1917.39, -804.13, 90 }, { -1915.80, -709.60, 90 }, { -1815.84, -617.33, 1 }, { -1714.19, -620.82, 9 },
		{ -1617.70, -618.23, 3 }, { -1518.55, -612.22, -5 }, { -1912.22, -912.80, 90 }, { -1911.35, -1011.56, 90 },
	};

	private static readonly double[,] FaultyTraps =
	{
		{ 1540.40, -224.82 }, { 1734.19, -254.11 }, { 1851.11, -215.81 }, { 1856.78, 15.41 }, { 1802.97, 130.80 },
		{ 1555.45, 118.65 }, { 1473.03, 34 }, { 1476.19, -190.49 }, { 654.10, -174.35 }, { 661.11, 72.16 },
		{ 943.39, 89.63 }, { 953.04, -195.36 }, { 157.04, 1270.98 }, { 348.04, 1233.71 }, { 430, 1266.78 },
		{ 104.20, 1479.83 }, { 171.54, 1763.05 }, { 420.14, 1738.68 }, { 463.42, 1553.36 },
	};

	private static readonly double[,] Fences =
	{
		{ -446.24, 68.89, 90 }, { -448.86, -185.84, 90 }, { -1284.52, -295.71, 180 }, { -1289.92, 286.47, 180 },
		{ -900.49, 67.14, 90 }, { -903.01, -194.19, 90 }, { -1785.23, 1396.29, 180 }, { 69.03, 1731.82, 90 },
		{ -1398.62, 1693.99, 90 }, { 509.76, 1697.81, 90 }, { -132.78, -365.96, 180 },
	};

	protected override void Load()
	{
		// Priest Pranas
		//-------------------------------------------------------------------------
		AddConditionalNpc(155044, L("Priest Pranas"), "PRISON622_PRANAS", "d_prison_62_2", -358.16, 283.26, -9, c => c.Quests.Has(Prison622Mq01) && !c.Quests.HasCompleted(OrshaMq3_01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Pranas"));

			if (character.Quests.IsCompletable(Prison622Mq01))
			{
				await dialog.Msg(L("I'm sorry. I know we don't have time to waste... I think there's a cursed idol here as well..."));
				await dialog.Msg(L("Fortunately, Draznie and Auranas seem to be fine. Let's just hope that the rest of the priests are okay as well."));
				await dialog.Msg(L("This curse... I really can't get used to it at all. I think we'll need more Orbs of Return."));
				await dialog.CompleteQuest(Prison622Mq01);
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("The best part is... According to Auranas it seems that Irma, who was captured by Marnox, is still alive."));
				await dialog.Msg(L("I also heard that Irma has a Orb of Return as well. We should hurry and rescue her before something bad happens to her."));

				while (true)
				{
					var answer = await dialog.SelectQuestOffer(Mq03, L("Irma is currently being held in the Solitary Confinement District... Marnox is sure to be nearby, so make sure you're prepared."),
						Option(L("I will go there right away"), "accept"),
						Option(L("I need to prepare"), "leave"),
						Option(L("Tell me about the legends"), "explain")
					);

					if (answer == "explain")
					{
						await dialog.Msg(L("Are you talking about the legends? Oh, you may not know since you're not from Orsha."));
						await dialog.Msg(L("A long time ago... It is said that a young shepherd boy met a girl that he had never seen before while tending his sheep."));
						await dialog.Msg(L("The girl barely talked, but everything she said happened. The shepherd boy was fascinated and took the girl to his uncle."));
						await dialog.Msg(L("The uncle is said to have shown his respect as soon as he saw the girl. The girl prophesied of numerous catastrophes to the uncle before disappearing."));
						await dialog.Msg(L("Medzio Diena and the catastrophe involving Orsha have already occurred... There is one more prophecy that has only been passed on by the bishops of Orsha."));
						await dialog.Msg(L("You can hear the story in more detail from any of the residents of Orsha, so why don't you ask around?"));
						continue;
					}

					if (answer == "accept")
					{
						character.Quests.Start(Mq03);
						character.LookAround();
					}
					return;
				}
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("I can't imagine what Irma is going through right now. I just hope that she'll be okay until you can get to her."));
				character.Quests.ClearQuestTrack(Mq03);
				return;
			}

			if (character.Quests.HasCompleted(Mq03))
			{
				await dialog.Msg(L("It's a relief knowing that Irma is safe. I think... I need to rest a little..."));
				return;
			}

			await dialog.Msg(L("Even if Irma is still alive, she might suffer a gruesome fate at any moment. I hope nothing happens to her..."));
		});

		// Priest Auranas
		//-------------------------------------------------------------------------
		AddConditionalNpc(155045, L("Priest Auranas"), "PRISON621_ARUNARAS", "d_prison_62_2", -420.58, 220.92, -7, c => !c.Quests.HasCompleted(OrshaMq3_01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Auranas"));

			if (character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("The pearl powder was scattered all over the floor? It seems as if the demons had torn into it."));
				await dialog.Msg(L("Thankfully, this is more than enough to use. Good job on getting it here."));
				await dialog.CompleteQuest(Sq01);
				character.LookAround();
				return;
			}

			if (character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Good job. The demons will be less aggressive for a while since their chain of command is ruined."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("We've prepared to greet the Revelator on the bishop's orders... But it looks like we'll be done for if we don't use it now."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("We've hidden a sack of Pearl Powder in the watchtower. Do you think you could gather it?"),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("I don't think that'll be needed"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Powder.GetLength(0); ++i)
						character.Variables.Perm.Set(PowderVar + i, false);

					character.Quests.Start(Sq01);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				await dialog.Msg(L("The demons also have a chain of command like we do, so I think that the other demons will fall if we take out their leaders."));
				await dialog.Msg(L("I've crafted an orb from the pearl powder you brought me. Try using it when you find a demon far away."));

				var answer = await dialog.SelectQuestOffer(Sq02, L("I've empowered it with holy power so it will tell you which demons you need to deal with."),
					Option(L("I will try"), "accept"),
					Option(L("I need some time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					character.Inventory.Add(ItemId.PRISON622_SQ_02_ITEM, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Once again, the bishop has impressed me with his wisdom. If such preparations hadn't been in place, we wouldn't have been able to deal with the demons at all."));
				await dialog.Msg(L("I'd say that my brothers are also in similar hideouts."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("You mustn't... think of the demons as monsters. I've heard that some Demon Lords are as powerful as Goddess Gabija."));
				return;
			}

			if (character.Quests.HasCompleted(Mq03))
			{
				await dialog.Msg(L("Thank the goddesses, Irma is safe! Now all we have to do is push the demons out of the Ashaq Underground Prison."));
				return;
			}

			if (character.Quests.HasCompleted(Prison622Mq01))
			{
				await dialog.Msg(L("It seems that I still have much to learn... I can't do anything because of the curse..."));
				return;
			}

			await dialog.Msg(L("I will become a more devout priest when we leave this place... I pray that we will make it out alive..."));
		});

		// Priest Draznie
		//-------------------------------------------------------------------------
		AddConditionalNpc(155046, L("Priest Draznie"), "PRISON622_DRAZUNIE", "d_prison_62_2", -418.49, 162.68, 125, c => !c.Quests.HasCompleted(OrshaMq3_01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Draznie"));

			if (character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thank you. I'll have to wait for a chance to take them to the Special Prison District."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("Did the demons fall for the traps? I'd never used them on demons before, so I was a little worried."));
				await dialog.Msg(L("I'm so glad that they worked as intended."));
				await dialog.CompleteQuest(Sq04);
				character.LookAround();
				return;
			}

			if (character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("I built a few dozens of them, so we're still far from collecting them all. If I get out of here, I need to go to Klaipeda and find that Sapper."));
				await dialog.CompleteQuest(Rp1);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("I feel awful because of the cursed idols. I think I need to rest a little at the safer Special Prison District for a while."));
				await dialog.Msg(L("We can also react to threats more easily thanks to its height. The only problem is that the demons are in control of it."));

				var answer = await dialog.SelectQuestOffer(Sq03, L("I feel guilty for asking for more help... But could you clear out the Special Prison District for us?"),
					Option(L("I'll take care of it"), "accept"),
					Option(L("That looks dangerous too"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("Auranas is in a really bad state. Pretending to be okay in order so we can worry less isn't doing any good..."));
					await dialog.Msg(L("Since safety isn't guaranteed here despite the Goddess Statue, I must beg you to hurry."));
				}
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				await dialog.Msg(L("I'm so relieved that Irma is safe and sound. I'm sure she'll dust it off since she's so easygoing."));
				await dialog.Msg(L("Oh, yes. We've made a bunch of traps in the Solitary Confinement District on the orders of the bishop quite some time ago. I learned this back when I was an apprentice... It's a trap to lure monsters to a apparition that looks like me."));

				var answer = await dialog.SelectQuestOffer(Sq04, L("I didn't say anything about them because I feared Irma might have gotten caught, but I think it's safe now. If you happen to go there, please be my guest and use the traps."),
					Option(L("Thank you"), "accept"),
					Option(L("I don't think that'll happen"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Traps.GetLength(0); ++i)
						character.Variables.Perm.Set(TrapVar + i, false);
					character.Variables.Perm.SetInt(TrapKillsVar, 0);

					character.Quests.Start(Sq04);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				await dialog.Msg(L("Running from the demons I found some traps around this area that weren't working like they should. They're supposed to react to demons only, but they reacted to me, too."));

				var answer = await dialog.SelectQuestOffer(Rp1, L("Then some Blue Vubbe Archers saw it and took a few of my traps for themselves. If you're going over there, would you please collect some of my traps?"),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("We should let that go"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= FaultyTraps.GetLength(0); ++i)
						character.Variables.Perm.Set(FaultyTrapVar + i, false);

					character.Quests.Start(Rp1);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Auranas is in a really bad state. Pretending to be okay in order so we can worry less isn't doing any good..."));
				await dialog.Msg(L("Since safety isn't guaranteed here despite the Goddess Statue, I must beg you to hurry."));
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("There are other places where we've prepared hideouts out of the prison as well. To exaggerate a little, we made so many that we could evacuate the population of Orsha."));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("I bought them from a Sapper I met in Klaipeda, and they weren't cheap... Guess they aren't worth the money."));
				return;
			}

			if (character.Quests.HasCompleted(Mq03))
			{
				await dialog.Msg(L("I can finally be at ease thanks to you. It seems as if the Revelator sent by the goddesses is different after all..."));
				return;
			}

			await dialog.Msg(L("I wonder what happened to the other priests that ran away from Marnox... It weighs heavily on my mind."));
		});

		// Priest Irma
		//-------------------------------------------------------------------------
		AddConditionalNpc(156006, L("Priest Irma"), "PRISON621_IRMA", "d_prison_62_2", -1904.77, -1204.17, 90, c => c.Quests.Has(Mq03) && !c.Quests.Has(Mq06), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Irma"));

			if (character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("I thought it was all over when I was captured by the Demon Lord... Thank you so much for rescuing me!"));
				await dialog.Msg(L("But... I think I heard Marnox call you the Revelator..."));
				await dialog.Msg(L("Ah... the bishop... So the bishop is safe as well. Oh... that's... a relief..."));
				await dialog.Msg(L("The Orb of Return? Of course I have one. Here you... Huh?"));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You've done well. Just a little divine power and it will be almost as good as new."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("I think this will be enough to destroy the idol. I'm grateful that we could get enough energy."));
				await dialog.Msg(L("It's our only orb and it barely has enough power... We should try to think of a way to make sure we destroy the idol."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				var freed = await character.TimeActions.StartAsync(L("Freeing..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (freed != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("Oh dear... It seems as if I've used the orb too much. It seems like it will break at any moment."));
				await dialog.Msg(L("I've used it numerous times against that uncouth and arrogant Marnox."));
				await dialog.Msg(L("It's too early to give up. There's a way to restore it. This orb is an instrument that turns powerful evil energy and turns it into divine power..."));
				await dialog.Msg(L("There's more than enough evil energy around. The Wendigo Escapees."));

				var answer = await dialog.SelectQuestOffer(Mq04, L("You can find the Wendigo Escapees near the Central Passage. You'll be able to restore the orb by absorbing their life and evil energy."),
					Option(L("I'll go there"), "accept"),
					Option(L("I'm going to rest for a while"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Inventory.Add(ItemId.PRISON622_MQ_04_ITEM, 1, InventoryAddType.PickUp);

					await dialog.Msg(L("Please hurry. I think Marnox found out where we've hid the Demon Orders."));
					await dialog.Msg(L("The seal that protects the orders can't be broken by a demon but... There is no telling what dirty tricks Marnox may play to break it."));
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("There's a crystal that Draznie prepared for such an occasion. I think... it should be in the Bird Room and the Supply Room."));

				while (true)
				{
					var answer = await dialog.SelectQuestOffer(Mq05, L("The power of light should transfer to the orb from the crystal if you take it there. Return to me after the orb has absorbed enough of the power."),
						Option(L("I will try"), "accept"),
						Option(L("Please wait a while"), "leave"),
						Option(L("Tell me about what the priests prepared"), "explain")
					);

					if (answer == "explain")
					{
						await dialog.Msg(L("I don't know whether to call the bishop meticulous... Or whether to call him worrisome..."));
						await dialog.Msg(L("He ordered us priests to prepare things near Orsha. Of course we never knew why we were preparing things back then."));
						await dialog.Msg(L("Then this all happened. If I could, I want to go back in time and kneel in front of the bishop in gratitude."));
						continue;
					}

					if (answer == "accept")
					{
						for (var i = 1; i <= Crystals.GetLength(0); ++i)
							character.Variables.Perm.Set(CrystalVar + i, false);
						character.Variables.Perm.SetInt(CrystalPowerVar, 0);

						character.Quests.Start(Mq05);
						character.Inventory.Add(ItemId.PRISON622_MQ_05_ITEM, 1, InventoryAddType.PickUp);
						character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Absorb the crystals' power in the Bird Room"), 8);
					}
					return;
				}
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("No matter how powerful the cursed idol is, the idol itself is merely a piece of wood. We'll have to aim for the instant it is full of evil energy."));
				await dialog.Msg(L("You saw the cursed idol on the way here right? How about filling it with evil energy by dealing with demons near it."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("Use the orb when the idol is full to the limit with evil energy. It will be destroyed as the divine and evil energies collide."),
					Option(L("I will try"), "accept"),
					Option(L("That's a very dangerous idea"), "leave")
				);

				if (answer == "accept")
				{
					character.Variables.Perm.SetInt(IdolEnergyVar, 0);
					character.Quests.Start(Mq02);
					character.Inventory.Add(ItemId.PRISON622_MQ_06_ITEM, 1, InventoryAddType.PickUp);
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Defeat the demons around the Cursed Idol in the Examination Room"), 8);
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("You've succeeded! The force that was weighing down my body is gone."));
				await dialog.Msg(L("Now let's go find the orders we've hidden away. I know the way there, of course. That's why Marnox captured me..."));

				var answer = await dialog.SelectQuestOffer(Mq06, L("I'll head down to the 3rd floor first. I think Gelija is there somewhere... I do hope that she is okay."),
					Option(L("I will go right away"), "accept"),
					Option(L("I have some things to do first"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					character.Quests.CompleteObjective(Mq06, "meetIrma");
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("I don't know how many more cursed idols are in this Underground Prison... But the only orb we have left is the one I just gave you."));
				return;
			}

			await dialog.Msg(L("I thought it was all over when I was locked in the Solitary Confinement District. But seeing you... I feel as if the efforts of the church were not in vain."));
		});

		// Cursed Idol
		//-------------------------------------------------------------------------
		AddConditionalNpc(47150, L("Cursed Idol"), "PRISON622_MQ_02_NPC", "d_prison_62_2", -1182.26, -49.25, 90, c => !c.Quests.HasCompleted(Mq02), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.IsCompletable(Mq02))
			{
				var used = await character.TimeActions.StartAsync(L("Using the Orb of Return..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));
				if (used != TimeActionResult.Completed)
					return;

				await dialog.CompleteQuest(Mq02);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("The Cursed Idol could not withstand the power and burst!{nl}Return to Priest Irma and tell her about it"), 5);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq02))
				character.ServerMessage(LF("The idol's evil energy: {0}/{1}", character.Variables.Perm.GetInt(IdolEnergyVar, 0), IdolEnergyNeeded));
		});

		// Draznie's crystals
		//-------------------------------------------------------------------------
		for (var i = 0; i < Crystals.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(46216, "UnvisibleName", "d_prison_62_2", Crystals[i, 0], Crystals[i, 1], 90);
			AddQuestTrigger("PRISON622_MQ_05_CRYSTAL_" + number, "d_prison_62_2", Crystals[i, 0], Crystals[i, 1], 80, async args =>
			{
				if (args.Initiator is not Character character)
					return;

				if (!character.Quests.IsActive(Mq05) || character.Quests.IsCompletable(Mq05) || character.Variables.Perm.GetBool(CrystalVar + number, false))
					return;

				character.Variables.Perm.Set(CrystalVar + number, true);
				var power = Math.Min(CrystalPowerNeeded, character.Variables.Perm.GetInt(CrystalPowerVar, 0) + CrystalPowerPerCrystal);
				character.Variables.Perm.SetInt(CrystalPowerVar, power);

				character.PlayEffect("F_light018_yellow", 1f, 1, EffectLocation.Middle);
				character.ServerMessage(LF("Crystal power absorbed: {0}/{1}", power, CrystalPowerNeeded));

				await Task.CompletedTask;
			});
		}

		// Twinkling Pearl Powder
		//-------------------------------------------------------------------------
		for (var i = 0; i < Powder.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(154064, L("Twinkling Pearl Powder"), "PRISON622_SQ_01_NPC_" + number, "d_prison_62_2", Powder[i, 0], Powder[i, 1], 90,
				character => character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01) && !character.Variables.Perm.GetBool(PowderVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq01) || character.Quests.IsCompletable(Sq01) || character.Variables.Perm.GetBool(PowderVar + number, false))
						return;

					character.Variables.Perm.Set(PowderVar + number, true);
					character.Inventory.Add(ItemId.PRISON622_SQ_01_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Draznie's Traps
		//-------------------------------------------------------------------------
		for (var i = 0; i < Traps.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(154067, L("Draznie's Trap"), "PRISON622_SQ_04_NPC_" + number, "d_prison_62_2", Traps[i, 0], Traps[i, 1], Traps[i, 2],
				character => character.Quests.IsActive(Sq04) && !character.Quests.IsCompletable(Sq04) && !character.Variables.Perm.GetBool(TrapVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq04) || character.Quests.IsCompletable(Sq04) || character.Variables.Perm.GetBool(TrapVar + number, false))
						return;

					character.Variables.Perm.Set(TrapVar + number, true);
					character.Variables.Temp.Set(TrapVar + "Armed", number);
					dialog.Npc.PlayEffect("F_light018_yellow", 1f);
					character.ServerMessage(L("The trap is set. Lure the demons into it and defeat them!"));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Faulty Traps
		//-------------------------------------------------------------------------
		for (var i = 0; i < FaultyTraps.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(46212, L("Faulty Trap"), "PRISON622_RP_1_OBJ_" + number, "d_prison_62_2", FaultyTraps[i, 0], FaultyTraps[i, 1], 180,
				character => character.Quests.IsActive(Rp1) && !character.Quests.IsCompletable(Rp1) && !character.Variables.Perm.GetBool(FaultyTrapVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Rp1) || character.Quests.IsCompletable(Rp1) || character.Variables.Perm.GetBool(FaultyTrapVar + number, false))
						return;

					character.Variables.Perm.Set(FaultyTrapVar + number, true);
					character.Inventory.Add(ItemId.PRISON622_RP_1_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The fences closing off the prison until the idol is destroyed
		//-------------------------------------------------------------------------
		for (var i = 0; i < Fences.GetLength(0); ++i)
			AddConditionalNpc(MonsterId.Block_Fence_2, "", "PRISON622_MQ_02_WALL_" + (i + 1), "d_prison_62_2", Fences[i, 0], Fences[i, 1], Fences[i, 2], c => !c.Quests.HasCompleted(Mq02));
	}

	/// <summary>
	/// Holds up the Pearly Orb, which marks the demon officers nearby.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PRISON622_SQ_02_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "d_prison_62_2" || !character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02))
		{
			character.ServerMessage(L("The orb doesn't react."));
			return ItemUseResult.OkayNotConsumed;
		}

		var targets = character.Map.GetAttackableEnemiesInPosition(character, character.Position, 300)
			.Where(entity => entity is Mob mob && IdolDemons.Contains(mob.Data.ClassName))
			.Take(3)
			.ToList();

		if (targets.Count == 0)
		{
			character.ServerMessage(L("There are no suitable targets nearby."));
			return ItemUseResult.OkayNotConsumed;
		}

		foreach (var target in targets)
			target.PlayEffect("F_light018_yellow", 1f);

		character.ServerMessage(L("The orb shines on the demons you need to deal with."));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Feeds the cursed idol with the demons defeated around it, and counts
	/// the demons lured into Draznie's traps.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Attacker is not Character character || args.Target is not Mob mob)
			return;

		if (character.Map.ClassName != "d_prison_62_2")
			return;

		if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02) && IdolDemons.Contains(mob.Data.ClassName) && mob.Position.Get2DDistance(IdolPosition) <= 400)
		{
			var energy = Math.Min(IdolEnergyNeeded, character.Variables.Perm.GetInt(IdolEnergyVar, 0) + 1);
			character.Variables.Perm.SetInt(IdolEnergyVar, energy);
			character.ServerMessage(LF("The idol absorbs the demon's magic: {0}/{1}", energy, IdolEnergyNeeded));
		}

		if (character.Quests.IsActive(Sq04) && !character.Quests.IsCompletable(Sq04))
		{
			var armed = character.Variables.Temp.GetInt(TrapVar + "Armed", 0);
			if (armed <= 0 || armed > Traps.GetLength(0))
				return;

			var trap = new Position((float)Traps[armed - 1, 0], mob.Position.Y, (float)Traps[armed - 1, 1]);
			if (mob.Position.Get2DDistance(trap) > 250)
				return;

			character.Variables.Temp.SetInt(TrapVar + "Armed", 0);

			var lured = Math.Min(TrapKillsNeeded, character.Variables.Perm.GetInt(TrapKillsVar, 0) + 1);
			character.Variables.Perm.SetInt(TrapKillsVar, lured);
			character.ServerMessage(LF("Demons lured into the traps: {0}/{1}", lured, TrapKillsNeeded));
		}
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60127: Into the Hands (5)
//-----------------------------------------------------------------------------
public class Prison622Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60127);
		SetName(L("Into the Hands (5)"));
		SetDescription(L("For the cursed idol to be safely destroyed, its energy needs to clash with the orb's power. First, defeat demons around the idol in the Examination Room and have it absorb their power."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_IRMA", "d_prison_62_2", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON622_MQ_02_NPC", "d_prison_62_2", L("Absorb demon magic into the Cursed Idol"));
		SetPhase(QuestStatus.Success, "PRISON622_MQ_02_NPC", "d_prison_62_2", L("Destroy the Cursed Idol with the Orb of Return"));

		AddPrerequisite(new QuestStatusPrerequisite(60130, QuestStatus.Completed));

		AddObjective("feedIdol", L("Have the Cursed Idol absorb the demons' magic to its limit"), new VariableCheckObjective(DPrison622QuestNpcsScript.IdolEnergyVar, 8, isPermanent: true));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("PRISON622_MQ_06_ITEM", -1));
	}
}

// 60128: Into the Grip (2)
//-----------------------------------------------------------------------------
public class Prison622Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60128);
		SetName(L("Into the Grip (2)"));
		SetDescription(L("Priest Pranas heard about what happened to Priest Irma from the other priests. Rescue Priest Irma from the Solitary Confinement District."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON622_PRANAS", "d_prison_62_2", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_IRMA", "d_prison_62_2", L("Rescue Priest Irma from the Solitary Confinement District"));
		SetPhase(QuestStatus.Success, "PRISON621_IRMA", "d_prison_62_2", L("Talk with Priest Irma"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON622_MQ_03_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60126, QuestStatus.Completed));

		AddObjective("killFireLord", L("Defeat Fire Lord"), new KillObjective(1, "boss_Fireload_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 3));
	}
}

// 60129: Into the Grip (3)
//-----------------------------------------------------------------------------
public class Prison622Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60129);
		SetName(L("Into the Grip (3)"));
		SetDescription(L("Priest Irma says the Orb of Return is heavily damaged from having used it on Demon Lord Marnox. Defeat Wendigo Escapees nearby the Central Passage and use their magic energy to restore the Orb of Return."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_IRMA", "d_prison_62_2", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON621_IRMA", "d_prison_62_2", L("Defeat Wendigo Escapees and charge the Orb of Return"));
		SetPhase(QuestStatus.Success, "PRISON621_IRMA", "d_prison_62_2", L("Deliver to Priest Irma"));

		AddPrerequisite(new QuestStatusPrerequisite(60128, QuestStatus.Completed));

		AddObjective("killWendigo", L("Defeat Wendigo Escapees"), new KillObjective(13, "Sec_escape_wendigo"));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("PRISON622_MQ_04_ITEM", -1));
	}
}

// 60130: Into the Grip (4)
//-----------------------------------------------------------------------------
public class Prison622Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60130);
		SetName(L("Into the Grip (4)"));
		SetDescription(L("Priest Irma wants you to charge the orb with power from the crystals in the Bird Room and the Supply Room. Walk near the crystals to have it absorb their power."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_IRMA", "d_prison_62_2", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON621_IRMA", "d_prison_62_2", L("Strengthen the Orb with the Crystal's Power"));
		SetPhase(QuestStatus.Success, "PRISON621_IRMA", "d_prison_62_2", L("Talk with Priest Irma"));

		AddPrerequisite(new QuestStatusPrerequisite(60129, QuestStatus.Completed));

		AddObjective("absorbPower", L("Absorb the crystals' power"), new VariableCheckObjective(DPrison622QuestNpcsScript.CrystalPowerVar, 15, isPermanent: true));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("PRISON622_MQ_05_ITEM", -1));
	}
}

// 60131: Into the Hands (6)
//-----------------------------------------------------------------------------
public class Prison622Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60131);
		SetName(L("Into the Hands (6)"));
		SetDescription(L("Priest Irma believes it's time to take back the Demon Orders and has asked you to meet her at Ashaq Underground Prison 3F."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_2", "d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_IRMA", "d_prison_62_2", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk to Priest Irma at Ashaq Underground Prison 3F"));
		SetPhase(QuestStatus.Success, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk to Priest Irma at Ashaq Underground Prison 3F"));

		AddPrerequisite(new QuestStatusPrerequisite(60127, QuestStatus.Completed));

		AddObjective("meetIrma", L("Talk to Priest Irma at Ashaq Underground Prison 3F"), new ManualObjective());
	}
}

// 60132: What If Again (1)
//-----------------------------------------------------------------------------
public class Prison622Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60132);
		SetName(L("What If Again (1)"));
		SetDescription(L("Under the instruction of Bishop Urbonas, Priest Auranas has several emergency items prepared all around Ashaq Underground Prison. Go to the Watchtower and retrieve the shiny pearl powder left there by Auranas."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_ARUNARAS", "d_prison_62_2", L("Talk with Priest Auranas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_ARUNARAS", "d_prison_62_2", L("Collect Twinkling Pearl Powder"));
		SetPhase(QuestStatus.Success, "PRISON621_ARUNARAS", "d_prison_62_2", L("Deliver to Priest Auranas"));

		AddPrerequisite(new LevelPrerequisite(12));

		AddObjective("collectPowder", L("Collect Twinkling Pearl Powder"), new CollectItemObjective("PRISON622_SQ_01_ITEM", 5));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("PRISON622_SQ_01_ITEM", -1));
	}
}

// 60133: What If Again (2)
//-----------------------------------------------------------------------------
public class Prison622Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60133);
		SetName(L("What If Again (2)"));
		SetDescription(L("According to Priest Auranas, using this orb will point out high-rank demons. Use the orb and defeat the demons signaled by it."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_ARUNARAS", "d_prison_62_2", L("Talk with Priest Auranas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_ARUNARAS", "d_prison_62_2", L("Defeat the demons marked by the Orb"));
		SetPhase(QuestStatus.Success, "PRISON621_ARUNARAS", "d_prison_62_2", L("Talk with Priest Auranas"));

		AddPrerequisite(new QuestStatusPrerequisite(60132, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the demons marked by the Orb"), new KillObjective(9, "Sec_Bat", "Sec_goblin_archer_blue", "Goblin_Spear_blue", "Sec_escape_wendigo"));

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new TakeItemReward("PRISON622_SQ_02_ITEM", -1));
	}
}

// 60134: To Expel
//-----------------------------------------------------------------------------
public class Prison622Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60134);
		SetName(L("To Expel"));
		SetDescription(L("Priest Draznie is looking for a safe place for the priests to take refuge. Defeat the monsters in the Special Prison District to make it a safe shelter for the priests."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Talk with Priest Draznie"));
		SetPhase(QuestStatus.InProgress, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Defeat the monsters in the Special Prison District"));
		SetPhase(QuestStatus.Success, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Report to Priest Draznie"));

		AddPrerequisite(new LevelPrerequisite(12));

		AddObjective("killMonsters", L("Defeat the monsters in the Special Prison District"), new KillObjective(7, "Sec_Bat", "Sec_goblin_archer_blue", "Goblin_Spear_blue", "Sec_escape_wendigo"));

		AddReward(new ItemReward("expCard2", 2));
	}
}

// 60135: Recruiting Prisoners
//-----------------------------------------------------------------------------
public class Prison622Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60135);
		SetName(L("Recruiting Prisoners"));
		SetDescription(L("Priest Draznie thinks it's time to use the trap prepared by Priest Auranas. Lure the demons into the trap set up in the Solitary Cells area, where they should be easily defeated."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Talk with Priest Draznie"));
		SetPhase(QuestStatus.InProgress, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Lure the demons into the traps"));
		SetPhase(QuestStatus.Success, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Talk with Priest Draznie"));

		AddPrerequisite(new QuestStatusPrerequisite(60130, QuestStatus.Completed));

		AddObjective("lureDemons", L("Lure demons into the traps in the Solitary Confinement District"), new VariableCheckObjective(DPrison622QuestNpcsScript.TrapKillsVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard2", 2));
	}
}

// 60153: Collect the Faulty Goods
//-----------------------------------------------------------------------------
public class Prison622Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60153);
		SetName(L("Collect the Faulty Goods"));
		SetDescription(L("Priest Draznie has asked you to collect some traps she set up a long time ago. According to her, most traps are set up in the Waiting Room and the Supply Room."));
		SetType(QuestType.Repeat);
		SetLocation("d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Talk with Priest Draznie"));
		SetPhase(QuestStatus.InProgress, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Collect Faulty Traps"));
		SetPhase(QuestStatus.Success, "PRISON622_DRAZUNIE", "d_prison_62_2", L("Return the Faulty Traps to Priest Draznie"));

		AddPrerequisite(new LevelPrerequisite(12));

		AddObjective("collectTraps", L("Collect Faulty Traps"), new CollectItemObjective("PRISON622_RP_1_ITEM", 8));
		AddPityDrop("PRISON622_RP_1_ITEM", 0.3f, 7, 1, "Sec_goblin_archer_blue");

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("PRISON622_RP_1_ITEM", -1));
	}
}
