//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 1F Quest NPCs
//--- Description -----------------------------------------------------------
// Priest Pranas' party, the cursed idol and Bishop Urbonas' hideout.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
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

public class DPrison621QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(60115);
	private readonly static QuestId Mq02 = new QuestId(60116);
	private readonly static QuestId Mq03 = new QuestId(60117);
	private readonly static QuestId Mq04 = new QuestId(60118);
	private readonly static QuestId Mq05 = new QuestId(60119);
	private readonly static QuestId Mq06 = new QuestId(60120);
	private readonly static QuestId Mq07 = new QuestId(60121);
	private readonly static QuestId Sq01 = new QuestId(60122);
	private readonly static QuestId Sq02 = new QuestId(60123);
	private readonly static QuestId Sq03 = new QuestId(60124);
	private readonly static QuestId Sq04 = new QuestId(60125);
	private readonly static QuestId Prison622Mq01 = new QuestId(60126);
	private readonly static QuestId Prison623Mq06 = new QuestId(60140);
	private readonly static QuestId Prison623Mq07 = new QuestId(60141);
	private readonly static QuestId OrshaMq3_01 = new QuestId(60145);

	public const string PackageCountVar = "Gabija.Quests.Prison621Sq04.Used";
	private const int PackageUses = 7;

	private static readonly Position HiddenRoomEntry = new Position(500f, 430.99f, 660f);
	private static readonly Position HiddenRoomExit = new Position(1947.65f, 199.73f, 590f);

	private static readonly string[] PackageTargets = { "Dumaro_blue", "wendigo_blue", "Sec_Yekubite", "Goblin_Miners_Blue" };

	protected override void Load()
	{
		// Priest Pranas
		//-------------------------------------------------------------------------
		AddConditionalNpc(155044, L("Priest Pranas"), "PRISON621_PRANAS", "d_prison_62_1", -600.84, -177.69, 90, c => c.Quests.Has(Mq01) && !c.Quests.Has(Prison622Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Pranas"));

			if (character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("It feels as if something I can't see is bound to me tightly... I'm finding it hard... to even breathe."));
				await dialog.Msg(L("How are you so unaffected by all this? Maybe the goddesses are looking after you seeing how indifferent you seem to be to all this..."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("What a strange idol. I've never seen anything like it even in any of the books before."));
				await dialog.Msg(L("I am certain that the demons are behind all of this... But there does not seem to be a way for us to become free of this curse."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				character.Quests.Start(Mq04);
				character.Quests.CompleteObjective(Mq04, "returnToPranas");
			}

			if (character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Has the curse been lifted? I suddenly feel a bit lighter now... How did you do that?"));
				await dialog.Msg(L("What? The bishop... is alive? Let us not further delay ourselves and save him!"));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("I've come here a few times when I was an apprentice a very long time ago, but I've never felt so weighed down like this before."));
				await dialog.Msg(L("Let's see... I sense a very strong evil presence towards the Torture Material Room. I haven't seen that ominous looking fog here before either."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("You seem to be in a good condition... Could you go and check what's happening over at the Torture Material Room for us?"),
					Option(L("I'll check"), "accept"),
					Option(L("I'm afraid"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					character.Quests.CompleteObjective(Mq02, "checkRoom");
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("My body... seems to become heavier and heavier. It's the same with the Chasers as well."));
				await dialog.Msg(L("There's something I want you to do since you seem to be the most uneffected out of all of us. Do you remember the map of Ashaq Underground Prison 1F that was in the chest along with his journal?"));
				await dialog.Msg(L("All we have to rely on now are the points marked on the map. I don't know what you'll find there, but this is the best we can do."));

				var answer = await dialog.SelectQuestOffer(Mq05, L("Take the map. The bishop has definitely left some clues... at the marked locations."),
					Option(L("I will check it"), "accept"),
					Option(L("Is there no other way?"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.Inventory.Add(ItemId.PRISON611_MAP_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await dialog.Msg(L("Oh yes, in order to reach those locations, you'll have to take the rail system meant to transport prisoners. There's one at the bottom of the stairs just right of here, but I don't know if it is still functioning."));
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Check the Ashaq Underground Prison 1F map in your inventory!"), 5);
				}
				return;
			}

			if (!character.Quests.Has(Prison622Mq01) && character.Quests.MeetsPrerequisites(Prison622Mq01))
			{
				await dialog.Msg(L("I cannot believe that you are the Revelator who will save this world... I knew it was strange that you were the only one unaffected by the curse."));
				await dialog.Msg(L("Unfortunately I don't have the time to revel in the fact that I'm in the presence of the Revelator. Like the bishop said: Time is of the utmost importance."));
				await dialog.Msg(L("Fortunately, the bishop seems to be safe for now... We don't know what's happened to the rest of the priests and we might be caught by Marnox at any moment."));
				await dialog.Msg(L("Those Chasers are holding us back. I can move immediately since I am a priest, but they look like they could use a bit more time."));

				var answer = await dialog.SelectQuestOffer(Prison622Mq01, L("Oh well. I shall go first and see what's going on. Don't be too far behind!"),
					Option(L("I will follow soon"), "accept"),
					Option(L("I still have other things to do"), "leave")
				);

				if (answer != "accept")
					return;

				var relayed = await character.TimeActions.StartAsync(L("Relaying the bishop's message..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
				if (relayed != TimeActionResult.Completed)
					return;

				character.Quests.Start(Prison622Mq01);
				character.Quests.CompleteObjective(Prison622Mq01, "followPranas");
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("This feels... exactly like a curse. I've never felt anything quite as vicious before."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Oh yes, in order to reach those locations, you'll have to take the rail system meant to transport prisoners. There's one at the bottom of the stairs just right of here, but I don't know if it is still functioning."));
				return;
			}

			await dialog.Msg(L("Don't worry about me. I'm second to none when it comes to stamina in the church."));
		});

		// Chaser Torvana
		//-------------------------------------------------------------------------
		AddConditionalNpc(147403, L("Chaser Torvana"), "PRISON621_TORNAVA", "d_prison_62_1", -370.74, -142.47, 2, IsPartyInThePrison, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Torvana"));

			if (character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("There was no curse when I came here before on other business. There were also only petty demons back then."));
				await dialog.Msg(L("I'm only sticking around since I've been paid and bound to a contract. At least I'll be able to scare the few demons trying to get out with this."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("What is this? Why did it suddenly turn into a normal piece of wood? Oh... Could it be the energy from the Goddess Statue?"));
				await dialog.Msg(L("At least I got to know that I'll be returning to the goddesses the instant I step away from this Goddess Statue."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("I can barely move in this chilly aura... You, are you really okay?"));
				await dialog.Msg(L("You seem to be the only person able to move, so I hope you don't mind me asking a favor. We'd be in trouble if the demons suddenly attack us, so I want to threaten them a bit."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("Gather some red crystals from Blue Dumaro and Wendigo near the Torture Chamber. It's similar to war trophies so they will be hesitant to attack us if we have a lot of them."),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("I don't think I can be of help"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq01);

				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				await dialog.Msg(L("I'm having a hard time even talking. Priest Pranas seems better off, maybe it's the priest powers?"));
				await dialog.Msg(L("They say that it's an eye for an eye, and a tooth for a tooth. Maybe we can destroy the cursed idols with the evil energy from demons?"));

				var answer = await dialog.SelectQuestOffer(Sq02, L("Mind if I ask you to gather some Black Wooden Pieces from demons in the Punishment Room? I know it sounds strange, but it's better than doing nothing."),
					Option(L("I'm not sure, but I'll gather some"), "accept"),
					Option(L("That's absurd"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq02);

				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("You seem to be fine unlike the rest of us... What did you do?"));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("The petty demons are here just like they were last time. I would have been leading you if I felt okay but..."));
				return;
			}

			if (character.Quests.HasCompleted(Mq07))
			{
				await dialog.Msg(L("Priest Pranas seems to have recovered quickly... But the air still seems to be heavy despite the cursed idol being destroyed."));
				return;
			}

			await dialog.Msg(L("Being so confident was a mistake. My body feels as if it is very heavy... What's happening?"));
		});

		// Chaser Daramaus
		//-------------------------------------------------------------------------
		AddConditionalNpc(147406, L("Chaser Daramaus"), "PRISON621_DARAMAUS", "d_prison_62_1", -373.31, -196.53, 173, IsPartyInThePrison, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chaser Daramaus"));

			if (character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("It's not the effect I was hoping for, but this is quite handy. I'll have to show it to my colleagues later."));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("Torvana says that the Blue Dumaro and Wendigo are carrying around strange packages."));
				await dialog.Msg(L("The reason that they're able to move around in such an environment... Perhaps they have something in those packages?"));

				var answer = await dialog.SelectQuestOffer(Sq03, L("If you ever find a reason to go to the prison, why don't you try gathering a few and use them? Maybe they'll be useful to you as well."),
					Option(L("I will collect them"), "accept"),
					Option(L("I won't do anything that's not necessary"), "leave")
				);

				if (answer == "accept")
				{
					character.Variables.Perm.SetInt(PackageCountVar, 0);
					character.Quests.Start(Sq03);
				}
				return;
			}

			if (character.Quests.IsActive(Sq03) || character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("I came because they said they'll pay me ten times the normal rate, and I'm paying dearly for it. I should stop trusting those priests outside of the church."));
				return;
			}

			if (character.Quests.HasCompleted(Mq04))
			{
				await dialog.Msg(L("Pranas seems to be heading straight down to the 2nd floor after suffering from the cursed idol. I guess not anyone can become a priest..."));
				return;
			}

			if (character.Quests.HasCompleted(Mq03))
			{
				await dialog.Msg(L("The cursed idol or whatever it is is really unsettling. It's going to take a while for me to recuperate..."));
				return;
			}

			await dialog.Msg(L("Torvana was confident a few minutes ago... We were chosen because we were supposed to be the best, but look at us now."));
		});

		// Cursed Idol
		//-------------------------------------------------------------------------
		AddConditionalNpc(47150, L("Cursed Idol"), "PRISON621_MQ_02_NPC", "d_prison_62_1", -1377.05, 92.34, 90, c => c.Quests.Has(Mq02) && !c.Quests.HasCompleted(Mq07), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.IsCompletable(Mq02))
			{
				var checkedIdol = await character.TimeActions.StartAsync(L("Checking..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));
				if (checkedIdol != TimeActionResult.Completed)
					return;

				await dialog.CompleteQuest(Mq02);

				if (character.Quests.HasCompleted(Mq02) && !character.Quests.Has(Mq03))
					character.Quests.Start(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				character.Quests.Start(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			if (character.Quests.IsCompletable(Mq07))
			{
				var used = await character.TimeActions.StartAsync(L("Using the Orb of Return..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));
				if (used != TimeActionResult.Completed)
					return;

				await dialog.CompleteQuest(Mq07);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("The curse on Ashaq Underground Prison 1F is lifting!"), 5);

				if (character.Quests.HasCompleted(Mq07) && !character.Quests.Has(Mq04))
				{
					character.Quests.Start(Mq04);
					character.Quests.CompleteObjective(Mq04, "returnToPranas");
				}

				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				character.ServerMessage(L("The Orb of Return needs more of the demons' evil energy."));
				return;
			}
		});

		// Bishop Urbonas
		//-------------------------------------------------------------------------
		AddConditionalNpc(154057, L("Bishop Urbonas"), "PRISON621_URBONAS", "d_prison_62_1", 522.08, 683.13, 90, IsUrbonasHiding, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bishop Urbonas"));

			if (character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("You... Oh, thank the goddesses! Does this mean that I may finally accomplish my mission...?"));
				await dialog.Msg(L("Yes. I am the bishop of Orsha, Urbonas. Who are you... may I perhaps hear your name?"));
				await dialog.Msg(L("Of course. Priest Pranas and the lord... This old man seems to have caused concern for a lot of people. Could I perhaps interest you into listening to me for a little bit?"));
				await dialog.Msg(L("Not long ago, Irma discovered strange orders on a demon while up north. The orders contained terrible plans that would destroy the entire region of Orsha."));
				await dialog.Msg(L("But that was not all. It also contained orders from the Demon Goddess Giltine to get rid of all Revelators."));
				await dialog.Msg(L("A Revelator is... ...someone chosen by the goddesses to save this world from the catastrophes to come."));
				await dialog.Msg(L("I wanted to give this information to the lord, but the demons had already started chasing me. I had no choice but to hide in this prison, a hideout that I had been preparing for a while."));
				await dialog.Msg(L("I could have ran to Orsha, but if I did that, the demons would have followed me there. Unfortunately, the demon that was after us was not an ordinary demon, but a Demon Lord."));
				await dialog.Msg(L("Demon Lord Marnox cursed the entire prison with his idols so that we could not escape. He also let loose his servants to search for us."));
				await dialog.Msg(L("We had to make a decision. First, I hid the Demon Orders in the Penitence Room on the 3rd floor, so that it would be safe even if I fall."));
				await dialog.Msg(L("Then I faced off against Marnox to the best of my abilities, but it was simply not enough. In the end, Irma was captured by Marnox... While the other priests were all scattered."));
				await dialog.Msg(L("This is the story of what happened up to now. Until you showed up, I was thinking that I would return to the arms of the goddess without being able to complete my mission."));
				await dialog.Msg(L("But my faith in hope has restored since you've arrived here."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("My goddess, your Revelator is finally here... The efforts of our church are not in vain..."));
				await dialog.Msg(L("Only you can stand up to the Demon Lord Marnox now. According to the goddesses, only you... the Revelator, can save this world."));
				await dialog.Msg(L("Please save the priests and Orsha."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (character.Quests.IsCompletable(Prison623Mq06))
			{
				var told = await character.TimeActions.StartAsync(L("Telling him what has happened..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (told != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Is it true? You've... defeated Marnox?!"));
				await dialog.Msg(L("All we could do was run even when all of us attacked him together... I see that a Revelator has remarkable powers."));
				await dialog.Msg(L("To see all the priests alive and well... it's a relief. I assume that we shall meet in Orsha soon."));
				await dialog.CompleteQuest(Prison623Mq06);
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("You've kept dreaming of the Demon Goddess Giltine? Hmm... Perhaps..."));
				await dialog.Msg(L("Goddess Laima is responsible for fate and foresight. You finding me may seem like a sequence of coincidences, but I am certain that the goddess has led you here."));

				var answer = await dialog.SelectQuestOffer(Mq06, L("There were orders from Giltine to get rid of 'the Revelator' as well. I wish to see if perhaps you... since you have been having those dreams... may be 'the Revelator'."),
					Option(L("What should I do?"), "accept"),
					Option(L("That's absurd"), "leave")
				);

				if (answer != "accept")
					return;

				var talked = await character.TimeActions.StartAsync(L("Describing the nightmare..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (talked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("The most important part is the invasion plans for Orsha. I didn't have enough time to fully analyze the orders while on the run from Marnox..."));
				await dialog.Msg(L("I hid the orders in the Penitence Room on the 3rd floor... I sealed it as well, but there is no guarantee that Marnox will not be able to break the seal."));
				await dialog.Msg(L("Not only that, but even though you are free of the idol's curse, you will not be able to reach the room if the priests are unable to move."));
				await dialog.Msg(L("I wish I could show you the way myself but... I think that would be a strech due to the wounds I've suffered from Marnox..."));
				await dialog.Msg(L("This Orb of Return may be able to help us. We can destroy the idols with it and free the priests from the curse."));

				var answer = await dialog.SelectQuestOffer(Mq07, L("Deal with nearby demons to collect evil energy in the orb and destroy the cursed idols!"),
					Option(L("Leave it to me"), "accept"),
					Option(L("That's a little too much for me"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq07);
					character.Inventory.Add(ItemId.PRISON621_MQ_07_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("I will be of no help to you in my current state... If you destroy all of the idols, please pass on my story to Pranas."));
				}
				return;
			}

			if (!character.Quests.Has(Prison623Mq07) && character.Quests.MeetsPrerequisites(Prison623Mq07))
			{
				await dialog.Msg(L("Now that we've recovered the Demon Orders... Let's head back to Orsha."));
				await dialog.Msg(L("The lord tends to attempt to act tough. She is strong, but gentle. I'm sure she's more worried than anyone else."));

				var answer = await dialog.SelectQuestOffer(Prison623Mq07, L("I'll head back to Orsha and meet with the lord before continuing with the interpretation of the Demon Orders. Why don't you meet with her again?"),
					Option(L("I'll go back to Orsha"), "accept"),
					Option(L("There are still some things to do"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Prison623Mq07);
					character.Quests.CompleteObjective(Prison623Mq07, "meetInesa");
					await dialog.Msg(L("I shall see you in Orsha then. I'll make my leave since the interpretation of the Demon Orders is a pressing matter."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("I'm certain that the goddess led you here. All these coincidences are pointing towards you."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Don't worry about me and look for the Demon Orders first. We must interpret the rest and find out the exact plans for the invasion of Orsha."));
				return;
			}

			if (character.Quests.HasCompleted(Mq06))
			{
				await dialog.Msg(L("Don't mind me, the orders... We must find it before Marnox gets his hands on it."));
				return;
			}

			await dialog.Msg(L("Only you can stand up to Demon Lord Marnox now. According to the goddesses, only you... the Revelator, can save this world."));
		});

		// The hidden chamber the map leads to
		//-------------------------------------------------------------------------
		AddConditionalNpc(154069, L("Hidden Chamber Entrance"), "PRISON621_TO_PRISON621_1", "d_prison_62_1", 1947.65, 625.84, 90, c => c.Quests.Has(Mq05), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
			{
				var opened = await character.TimeActions.StartAsync(L("Opening..."), L("Cancel"), "SITGROPESET", TimeSpan.FromSeconds(3));
				if (opened != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq05);
				return;
			}

			character.Warp("d_prison_62_1", HiddenRoomEntry.X, HiddenRoomEntry.Y, HiddenRoomEntry.Z);
		});

		AddConditionalNpc(154069, L("Ashaq Underground Prison 1F"), "PRISON621_1_TO_PRISON621", "d_prison_62_1", 463.65, 659.84, 90, c => c.Quests.Has(Mq05), async dialog =>
		{
			dialog.Player.Warp("d_prison_62_1", HiddenRoomExit.X, HiddenRoomExit.Y, HiddenRoomExit.Z);
			await Task.CompletedTask;
		});

		// The candlesticks around the Revelator's magic circle
		//-------------------------------------------------------------------------
		AddNpc(147358, "UnvisibleName", "d_prison_62_1", 805, 578, 45);
		AddNpc(147358, "UnvisibleName", "d_prison_62_1", 822, 761, 45);
		AddNpc(147358, "UnvisibleName", "d_prison_62_1", 987, 752, 45);
		AddNpc(147358, "UnvisibleName", "d_prison_62_1", 965, 575, 45);

		AddQuestTrigger("PRISON621_MQ_01_NPC", "d_prison_62_1", -453.46, -818.52, 180, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
				character.Quests.StartQuestTrack(Mq01);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether the Chasers are resting inside the prison.
	/// </summary>
	private static bool IsPartyInThePrison(Character character)
		=> character.Quests.Has(Mq01) && !character.Quests.HasCompleted(OrshaMq3_01);

	/// <summary>
	/// Returns whether Bishop Urbonas is hiding in his chamber.
	/// </summary>
	private static bool IsUrbonasHiding(Character character)
		=> character.Quests.Has(Mq05) && !character.Quests.Has(Prison623Mq07);

	/// <summary>
	/// Shows where the map left by Bishop Urbonas leads.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PRISON611_MAP_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Take the track under the stairs to the right side of Pranas to get to the location marked on the map."), 5);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Opens one of the demons' Unidentified Packages on the monster in
	/// front of the player.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PRISON621_SQ_04_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "d_prison_62_1" || !character.Quests.IsActive(Sq04) || character.Quests.IsCompletable(Sq04))
		{
			character.ServerMessage(L("There is no need to use the package right now."));
			return ItemUseResult.OkayNotConsumed;
		}

		var target = character.Map.GetAttackableEnemiesInPosition(character, character.Position, 150)
			.FirstOrDefault(entity => entity is Mob mob && PackageTargets.Contains(mob.Data.ClassName));

		if (target == null)
		{
			character.ServerMessage(L("There are no suitable targets nearby."));
			return ItemUseResult.OkayNotConsumed;
		}

		target.PlayEffect("F_buff_basic009_blue", 1f);

		var used = character.Variables.Perm.GetInt(PackageCountVar, 0) + 1;
		character.Variables.Perm.SetInt(PackageCountVar, used);
		character.ServerMessage(LF("Packages tested on monsters: {0}/{1}", Math.Min(used, PackageUses), PackageUses));

		return ItemUseResult.OkayNotConsumed;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60116: Bishop Urbonas' Whereabouts (2)
//-----------------------------------------------------------------------------
public class Prison621Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60116);
		SetName(L("Bishop Urbonas' Whereabouts (2)"));
		SetDescription(L("Priest Pranas says he feels as if his body is being constricted. He did say the sensation felt stronger inside the Torture Material Room; go there and investigate."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_PRANAS", "d_prison_62_1", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_MQ_02_NPC", "d_prison_62_1", L("Check the Torture Material Room"));
		SetPhase(QuestStatus.Success, "PRISON621_MQ_02_NPC", "d_prison_62_1", L("Check the Torture Material Room"));

		AddPrerequisite(new QuestStatusPrerequisite(60115, QuestStatus.Completed));

		AddObjective("checkRoom", L("Check the Torture Material Room"), new ManualObjective());
	}
}

// 60117: Bishop Urbonas' Whereabouts (3)
//-----------------------------------------------------------------------------
public class Prison621Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60117);
		SetName(L("Bishop Urbonas' Whereabouts (3)"));
		SetDescription(L("The idol inside the Torture Material Room is making the demons attack!"));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_MQ_02_NPC", "d_prison_62_1", L("Defeat the incoming demons"));
		SetPhase(QuestStatus.InProgress, "PRISON621_MQ_02_NPC", "d_prison_62_1", L("Defeat the incoming demons"));
		SetPhase(QuestStatus.Success, "PRISON621_PRANAS", "d_prison_62_1", L("Report to Priest Pranas on the Cursed Idol"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON621_MQ_03_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60116, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the attacking demons"), new KillObjective(9, "Dumaro_blue", "wendigo_blue") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
	}
}

// 60118: Bishop Urbonas' Whereabouts (7)
//-----------------------------------------------------------------------------
public class Prison621Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60118);
		SetName(L("Bishop Urbonas' Whereabouts (7)"));
		SetDescription(L("With the cursed idol destroyed, the curse on Ashaq Underground Prison 1F seems to have been lifted. Return to Priest Pranas."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_PRANAS", "d_prison_62_1", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_PRANAS", "d_prison_62_1", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.Success, "PRISON621_PRANAS", "d_prison_62_1", L("Talk with Priest Pranas"));

		AddPrerequisite(new QuestStatusPrerequisite(60121, QuestStatus.Completed));

		AddObjective("returnToPranas", L("Talk with Priest Pranas"), new ManualObjective());
	}
}

// 60119: Bishop Urbonas' Whereabouts (4)
//-----------------------------------------------------------------------------
public class Prison621Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60119);
		SetName(L("Bishop Urbonas' Whereabouts (4)"));
		SetDescription(L("Priest Pranas believes the map left behind by Bishop Urbonas is the only hope you have. Take the track under the stairs to the right side of Pranas to get to the location marked on the map."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_PRANAS", "d_prison_62_1", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_TO_PRISON621_1", "d_prison_62_1", L("Go to the location marked on the map left by Bishop Urbonas"));
		SetPhase(QuestStatus.Success, "PRISON621_URBONAS", "d_prison_62_1", L("Talk to Bishop Urbonas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON621_MQ_05_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60117, QuestStatus.Completed));

		AddObjective("killClymen", L("Defeat Clymen"), new KillObjective(1, "boss_Clymen_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new TakeItemReward("PRISON611_MAP_ITEM", -1));
	}
}

// 60120: Bishop Urbonas' Whereabouts (5)
//-----------------------------------------------------------------------------
public class Prison621Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60120);
		SetName(L("Bishop Urbonas' Whereabouts (5)"));
		SetDescription(L("Bishop Urbonas wonders if you're not a Revelator as the ones told in the legends yourself. Step on the Revelator magic circle to prove your identity as a Revelator."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_URBONAS", "d_prison_62_1", L("Talk to Bishop Urbonas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_URBONAS", "d_prison_62_1", L("Check the magic circle"));
		SetPhase(QuestStatus.Success, "PRISON621_URBONAS", "d_prison_62_1", L("Talk to Bishop Urbonas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON621_MQ_06_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60119, QuestStatus.Completed));

		AddObjective("checkCircle", L("Check the magic circle"), new ManualObjective());
	}
}

// 60121: Bishop Urbonas' Whereabouts (6)
//-----------------------------------------------------------------------------
public class Prison621Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60121);
		SetName(L("Bishop Urbonas' Whereabouts (6)"));
		SetDescription(L("According to Urbonas, to destroy the cursed idol you need to defeat demons in Ashaq Underground Prison 1F and recharge the Orb of Return, then use it on the cursed idol."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_URBONAS", "d_prison_62_1", L("Talk to Bishop Urbonas"));
		SetPhase(QuestStatus.InProgress, "PRISON621_URBONAS", "d_prison_62_1", L("Defeat demons to recharge the Orb of Return"));
		SetPhase(QuestStatus.Success, "PRISON621_MQ_02_NPC", "d_prison_62_1", L("Destroy the Cursed Idol in the Torture Material Room"));

		AddPrerequisite(new QuestStatusPrerequisite(60120, QuestStatus.Completed));

		AddObjective("rechargeOrb", L("Defeat Blue Dumaros and Blue Wendigos to recharge the Orb of Return"), new KillObjective(8, "Dumaro_blue", "wendigo_blue"));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 20));
		AddReward(new TakeItemReward("PRISON621_MQ_07_ITEM", -1));
	}
}

// 60122: Blackmail
//-----------------------------------------------------------------------------
public class Prison621Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60122);
		SetName(L("Blackmail"));
		SetDescription(L("Chaser Torvana has asked you to collect red crystals from demons. Go to the Torture Chamber and defeat Blue Dumaros and Blue Wendigos to collect red crystals."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_TORNAVA", "d_prison_62_1", L("Talk with Chaser Torvana"));
		SetPhase(QuestStatus.InProgress, "PRISON621_TORNAVA", "d_prison_62_1", L("Collect Red Essences"));
		SetPhase(QuestStatus.Success, "PRISON621_TORNAVA", "d_prison_62_1", L("Deliver to Chaser Torvana"));

		AddPrerequisite(new LevelPrerequisite(10));

		AddObjective("collectEssences", L("Collect Red Essences by defeating Blue Dumaros and Blue Wendigos"), new CollectItemObjective("PRISON621_SQ_01_ITEM", 7));
		AddPityDrop("PRISON621_SQ_01_ITEM", 0.6f, 3, 1, "Dumaro_blue", "wendigo_blue");

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 25));
		AddReward(new TakeItemReward("PRISON621_SQ_01_ITEM", -1));
	}
}

// 60123: Fight Poison With Poison
//-----------------------------------------------------------------------------
public class Prison621Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60123);
		SetName(L("Fight Poison With Poison"));
		SetDescription(L("Chaser Torvana wants to beat the curse using the demons' evil energy. Obtain black wooden pieces from the demons in the Punishment Room."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_TORNAVA", "d_prison_62_1", L("Talk with Chaser Torvana"));
		SetPhase(QuestStatus.InProgress, "PRISON621_TORNAVA", "d_prison_62_1", L("Collect Black Wooden Pieces"));
		SetPhase(QuestStatus.Success, "PRISON621_TORNAVA", "d_prison_62_1", L("Deliver to Chaser Torvana"));

		AddPrerequisite(new LevelPrerequisite(10));

		AddObjective("collectWood", L("Collect Black Wooden Pieces"), new CollectItemObjective("PRISON621_SQ_02_ITEM", 7));
		AddPityDrop("PRISON621_SQ_02_ITEM", 0.7f, 3, 1, "Dumaro_blue", "wendigo_blue", "Sec_Yekubite", "Goblin_Miners_Blue");

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("PRISON621_SQ_02_ITEM", -1));
	}
}

// 60124: Unidentified Package (1)
//-----------------------------------------------------------------------------
public class Prison621Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60124);
		SetName(L("Unidentified Package (1)"));
		SetDescription(L("Chaser Daramaus believes the solution to lifting the curse might be in the unidentified packages carried around by the demons. Defeat Blue Dumaros and obtain their packages."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_DARAMAUS", "d_prison_62_1", L("Talk with Chaser Daramaus"));
		SetPhase(QuestStatus.InProgress, "PRISON621_DARAMAUS", "d_prison_62_1", L("Obtain Unidentified Packages"));
		SetPhase(QuestStatus.Success, "PRISON621_DARAMAUS", "d_prison_62_1", L("Obtain Unidentified Packages"));

		AddPrerequisite(new LevelPrerequisite(10));

		AddObjective("collectPackages", L("Collect Unidentified Packages by defeating Blue Dumaros and Blue Wendigos"), new CollectItemObjective("PRISON621_SQ_04_ITEM", 7));
		AddPityDrop("PRISON621_SQ_04_ITEM", 1.0f, 0, 1, "Dumaro_blue", "wendigo_blue");

		AddReward(new ItemReward("expCard2", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The client names no turn-in NPC; testing the packages follows on its own.
		character.Quests.Complete(this.QuestId);
	}
}

// 60125: Unidentified Package (2)
//-----------------------------------------------------------------------------
public class Prison621Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60125);
		SetName(L("Unidentified Package (2)"));
		SetDescription(L("You have collected enough unidentified packages. Use them on monsters to see what effect they possess."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "PRISON621_DARAMAUS", "d_prison_62_1", L("Use the Unidentified Packages to see their effect"));
		SetPhase(QuestStatus.InProgress, "PRISON621_DARAMAUS", "d_prison_62_1", L("Use the Unidentified Packages to see their effect"));
		SetPhase(QuestStatus.Success, "PRISON621_DARAMAUS", "d_prison_62_1", L("Talk with Chaser Daramaus"));

		AddPrerequisite(new QuestStatusPrerequisite(60124, QuestStatus.Completed));

		AddObjective("usePackages", L("Use the Unidentified Packages on monsters and check their effect"), new VariableCheckObjective(DPrison621QuestNpcsScript.PackageCountVar, 7, isPermanent: true));

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new TakeItemReward("PRISON621_SQ_04_ITEM", -1));
	}
}

// 60126: Into the Grip (1)
//-----------------------------------------------------------------------------
public class Prison622Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60126);
		SetName(L("Into the Grip (1)"));
		SetDescription(L("The Chasers have not yet recovered from the effects of the curse. Follow Priest Pranas into Ashaq Underground Prison 2F."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1", "d_prison_62_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_PRANAS", "d_prison_62_1", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "PRISON622_PRANAS", "d_prison_62_2", L("Follow Priest Pranas into Ashaq Underground Prison 2F"));
		SetPhase(QuestStatus.Success, "PRISON622_PRANAS", "d_prison_62_2", L("Follow Priest Pranas into Ashaq Underground Prison 2F"));

		AddPrerequisite(new QuestStatusPrerequisite(60118, QuestStatus.Completed));

		AddObjective("followPranas", L("Follow Priest Pranas into Ashaq Underground Prison 2F"), new ManualObjective());
	}
}

// 60141: Everything Intact (2)
//-----------------------------------------------------------------------------
public class Prison623Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60141);
		SetName(L("Everything Intact (2)"));
		SetDescription(L("Bishop Urbonas believes the Lord of Orsha, Inesa Hamondale, will be worried about him. Return to Orsha and talk to Inesa Hamondale."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_1", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON621_URBONAS", "d_prison_62_1", L("Talk to Bishop Urbonas"));
		SetPhase(QuestStatus.InProgress, "C_ORSHA_HAMONDAIL", "c_orsha", L("Return to Orsha and talk to Inesa Hamondale"));
		SetPhase(QuestStatus.Success, "C_ORSHA_HAMONDAIL", "c_orsha", L("Return to Orsha and talk to Inesa Hamondale"));

		AddPrerequisite(new QuestStatusPrerequisite(60140, QuestStatus.Completed));

		AddObjective("meetInesa", L("Return to Orsha and talk to Inesa Hamondale"), new ManualObjective());
	}
}
