//--- Melia Script ----------------------------------------------------------
// Fedimian Suburbs Quest NPCs
//--- Description -----------------------------------------------------------
// The drunk who follows his brother's monuments, the stonemasons and the
// doctor on the Fedimian road, and the magician waiting at the Karsta Hall
// Site for a Revelator.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains40QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8452);
	private readonly static QuestId Mq02 = new QuestId(8453);
	private readonly static QuestId Mq03 = new QuestId(8454);
	private readonly static QuestId Mq04 = new QuestId(8455);
	private readonly static QuestId Mq05 = new QuestId(8456);
	private readonly static QuestId Mq06 = new QuestId(8457);
	private readonly static QuestId Mq07 = new QuestId(8458);
	private readonly static QuestId Sq01 = new QuestId(8459);
	private readonly static QuestId Sq02 = new QuestId(8460);
	private readonly static QuestId Sq03 = new QuestId(8461);
	private readonly static QuestId Sq04 = new QuestId(8462);
	private readonly static QuestId Sq05 = new QuestId(8463);
	private readonly static QuestId Rp1 = new QuestId(60183);
	private readonly static QuestId Hq01 = new QuestId(19041);
	private readonly static QuestId ToTheTower1 = new QuestId(8471);
	private readonly static QuestId ToTheTower2 = new QuestId(8472);

	private readonly static double[,] OldBoxSpots =
	{
		{ -1716, -800 }, { -1723, -590 }, { -1719, -482 }, { -1414, -517 }, { -1415, -1087 },
		{ -1288, -924 }, { -1495, -577 }, { -1042, -768 }, { -2036, -55 }, { -1847, -164 },
	};

	protected override void Load()
	{
		// Coben, on the Fedimian road
		//-------------------------------------------------------------------------
		AddConditionalNpc(20118, L("Coben"), "REMAINS_40_DRUNK_01", "f_remains_40", -502, -2059, 90, c => !c.Quests.HasCompleted(Mq03), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Coben"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Something is between the pages of this diary. A blueprint... and oh, my brother had recorded a few things here."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("So he challenged the great Mage Tower? That's my brother! He could certainly do that."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Yes! He would not stop at stealing from the Mage Tower."));
				await dialog.Msg(L("I will go to Negyvas Field first, so follow me!"));
				await dialog.CompleteQuest(Mq03);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				var answer = await dialog.SelectQuestOffer(Mq01, L("I am looking for my brother. If you can help me, I may give you a reward."),
					Option(L("Help find his brother"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("The hideout of my brother, whom I used to serve, is located at the Abandoned Farm up north. Please get my brother's diary there."));
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("I came here to look for my brother, but I am not as brave as he is. I want you to take a look at his monument's writing on my behalf."),
					Option(L("I'll go and read the tombstone"), "accept"),
					Option(L("I'd like to stop"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("Yes. My brother is a thief. He used to come to our shop once in a few years and tell us stories about his adventures."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("We should look for the next monument. If the writing can't be seen clearly, rub it with Cockatrice Fat."),
					Option(L("I'll go and read the tombstone"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("Mage Tower... Is there anything to steal there?"));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("He was a really fantastic person. I can't imagine myself being like him."));
				return;
			}

			if (character.Quests.IsActive(Mq02) || character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The monuments stand along the old road. Read them for me."));
				return;
			}

			await dialog.Msg(L("My brother was a thief, and a famous one. I only ever kept the shop."));
		});

		// Coben, at Negyvas Field
		//-------------------------------------------------------------------------
		AddConditionalNpc(20118, L("Coben"), "REMAINS_40_DRUNK_02", "f_remains_40", 1791, 1195, 90, c => c.Quests.HasCompleted(Mq03) && !c.Quests.HasCompleted(Mq06), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Coben"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("How could he even think of using magic against the Archmage? That's my brother."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("You have to concentrate magic power? Ah! My brother left me something before."));
				await dialog.Msg(L("I brought it to give it back to him, but I don't know if it's right..."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("Yes. The monument, the monument..."));
				await dialog.Msg(L("There is only one place left which I haven't been to. Follow me to the Karsta Hall Site."));
				await dialog.CompleteQuest(Mq06);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("You came. The next monument is in front of me, but I am just a drunken coward. I will stay at the road on the left side of Negyvas Field, so you go and check it."),
					Option(L("Read the tombstone and come back"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("It is a shame to look back once a man starts his journey. My brother will rather choose to die than look back."));
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("For the next one, we will need to apply Hallowventer Charcoal in order to read it. The next monument is at the right side of Negyvas Field."),
					Option(L("I'll read what's written on the tombstone"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					await dialog.Msg(L("He will do anything he sets his mind to. How could we have imagined that he could climb into the Mage Tower which even the demons failed to climb?"));
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				var answer = await dialog.SelectQuestOffer(Mq06, L("My brother told me that it is a device that will charge magic power when it is installed at the right place. But he also told me that it could summon a strong monster, so he said to be careful."),
					Option(L("I'll charge the Spell Device"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					character.Inventory.Add(ItemId.REMAINS40_MQ_06_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("The Mage Tower should let us enter it without any complaints. If I could find my brother with this... It will be so good."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq04) || character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The monument is out there. I will wait here."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Set the jar down at the Camp of Apiarists and let it drink the magic in the air."));
				return;
			}

			await dialog.Msg(L("Negyvas Field. He walked this road too, I am sure of it."));
		});

		// Coben, at the Karsta Hall Site
		//-------------------------------------------------------------------------
		AddConditionalNpc(20118, L("Coben"), "REMAINS_40_DRUNK_03", "f_remains_40", 3016, 2916, 90, c => c.Quests.HasCompleted(Mq06), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Coben"));

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("I'm sure my brother stood firm and accepted his final moments."));
				await dialog.Msg(L("However, I still think he could've lived more meaningfully if he had just turned away from his ambition..."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				var answer = await dialog.SelectQuestOffer(Mq07, L("Soon I can meet my brother. Please check the last monument."),
					Option(L("Go and check what's written on the last tombstone"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq07);
					await dialog.Msg(L("His mind must have been like mine here. A famous name can be a burden sometimes."));
				}
				return;
			}

			if (!character.Quests.Has(Hq01) && character.Quests.MeetsPrerequisites(Hq01))
			{
				var answer = await dialog.SelectQuestOffer(Hq01, L("Since you told me about my brother's death, I'd like to reward you. It's something I saved for when my brother and I could journey again, but I don't need it anymore."),
					Option(L("I appreciate it"), "accept"),
					Option(L("I don't need it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq01);
					character.Inventory.Add(ItemId.REMAINS_40_HQ_01_ITEM2, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("With this key, we can open the treasure chest. But you will need to look for the chest yourself."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("The last monument is over there, past the hall. I cannot bring myself to read it."));
				return;
			}

			if (character.Quests.IsActive(Hq01))
			{
				await dialog.Msg(L("If it weren't for you, I would have continued believing that my brother was alive and well somewhere. You can't say you are not responsible for this."));
				return;
			}

			await dialog.Msg(L("Zubeck. That was his name. Nobody here remembers it now."));
		});

		// The Forgotten Thief's Monuments
		//-------------------------------------------------------------------------
		AddNpc(47191, L("Forgotten Thief's Monument"), "REMAINS_40_MQ_02", "f_remains_40", -121, -17, 43, dialog => this.ReadMonument(dialog, Mq02, "readSecondMonument", L("He swore here that he would take on the greatest of all targets, and he named the Mage Tower.")));
		AddNpc(47191, L("Forgotten Thief's Monument"), "REMAINS_40_MQ_03", "f_remains_40", 867, -273, 92, dialog => this.ReadMonument(dialog, Mq03, "readThirdMonument", L("The stone is weathered, but rubbed over it reads that a thief who cannot rob the Mage Tower is no thief at all.")));
		AddNpc(47191, L("Forgotten Thief's Monument"), "REMAINS_40_MQ_04", "f_remains_40", 884, 784, 77, dialog => this.ReadMonument(dialog, Mq04, "readFourthMonument", L("So what should I do? The answer he wrote under it is to meet magic with magic.")));
		AddNpc(47191, L("Forgotten Thief's Monument"), "REMAINS_40_MQ_05", "f_remains_40", 2168, 1372, 176, dialog => this.ReadMonument(dialog, Mq05, "readFifthMonument", L("The way into the tower, written out step by step. It ends: concentrate the magic power, and the door opens itself.")));

		// The Forgotten Thief's Monument, at the Camp of Apiarists
		//-------------------------------------------------------------------------
		AddNpc(47191, L("Forgotten Thief's Monument"), "REMAINS_40_MQ_06", "f_remains_40", 2419, 3792, 47, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Forgotten Thief's Monument"));

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
			{
				var charged = await character.TimeActions.StartAsync(L("Charging the jar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (charged != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq06, "chargeJar");
				character.ServerMessage(L("The jar draws in the magic that hangs around the monument and goes still."));
				return;
			}

			await dialog.Msg(L("A monument to a thief nobody remembers, standing where the magic runs thickest."));
		});

		// The Commanding Monument
		//-------------------------------------------------------------------------
		AddNpc(47191, L("Commanding Monument"), "REMAINS_40_MQ_07", "f_remains_40", 3660, 2624, 102, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Commanding Monument"));

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
			{
				var read = await character.TimeActions.StartAsync(L("Reading the monument..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The last line names no theft at all. It is an epitaph, written by whoever buried him here."));
				character.Quests.StartQuestTrack(Mq07);
				return;
			}

			await dialog.Msg(L("The name at the head of the stone has been struck out and written over."));
		});

		// The old boxes at the Abandoned Farm
		//-------------------------------------------------------------------------
		for (var i = 0; i < OldBoxSpots.GetLength(0); i++)
		{
			var uniqueName = "REMAINS_40_MQ_01_" + (i + 1);
			AddNpc(46212, L("Old Box"), uniqueName, "f_remains_40", OldBoxSpots[i, 0], OldBoxSpots[i, 1], 179, this.SearchOldBox);
		}

		// Stonemason Canolyn
		//-------------------------------------------------------------------------
		AddNpc(20156, L("Stonemason Canolyn"), "REMAINS_40_CANOLIN_01", "f_remains_40", -645, -1898, 96, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Stonemason Canolyn"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Hey, why did you take so long? While you were gone, the monsters came and stole everything from us!"));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("The toolbox wasn't there? Well. We can't do anything about it then. At least I can say we got our revenge on it."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Rp1) && character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("It seems like you've done quite a job even if it's only a small step towards clearing it all up. Thank you!"));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("Are you going to Fedimian as well? We are stonemasons. We were called to rebuild Fedimian and were hurrying over there, but..."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("The Cockatrices along the road will not let a loaded cart through."),
					Option(L("I'll defeat the Cockatrices"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("There is no reason for those monsters to be walking around. They are just there."));
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				await dialog.Msg(L("I can charge more to get everything else back in order, but that toolbox... That Moa took the heart and soul of a stonemason."));

				var answer = await dialog.SelectQuestOffer(Sq02, L("Will you get it back for me?"),
					Option(L("Which monster's act is it?"), "accept"),
					Option(L("I better get going"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("It's a monster called Moa. It ran away to Leipsna Chapel Site."));
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				await dialog.Msg(L("Fedimian is one of the major stopping points for the traders heading inland. There will be many traders coming through here once this is all settled."));

				var answer = await dialog.SelectQuestOffer(Rp1, L("That's why I want to create a trading post at Leipsna Chapel Site. I'd appreciate it if you'd clear the place up a bit from the monsters."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("Not really my problem"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rp1);
					await dialog.Msg(L("The only alternative to success is for mankind to fail, so I am investing and hoping for the best."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq01) || character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("There is no reason for those monsters to be walking around. They are just there."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("I guess it's true, all things with wings like shiny things."));
				return;
			}

			await dialog.Msg(L("Stone is honest work. It is the road to it that is the trouble."));
		});

		// Tara Miles, on the Fedimian road
		//-------------------------------------------------------------------------
		AddConditionalNpc(147473, L("Tara Miles"), "REMAINS_40_TARA_01", "f_remains_40", -242, -1931, 8, c => !c.Quests.HasCompleted(Sq03), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tara Miles"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Okay. Let's talk again at Negyvas Field."));
				await dialog.CompleteQuest(Sq03);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("I'm a doctor who came to help patients in Fedimian. I have some problems, can you help me?"),
					Option(L("I can help you"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("I thank you in the name of the goddess! We should start by eliminating the filthy Cockatrices that are spreading disease around."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("There's a sign of plague within those monsters' trap. Fortunately, I think it's not too serious."));
				return;
			}

			await dialog.Msg(L("Prevention is worth more than any cure I can carry in a bag."));
		});

		// Tara Miles, at Negyvas Field
		//-------------------------------------------------------------------------
		AddConditionalNpc(147473, L("Tara Miles"), "REMAINS_40_TARA_02", "f_remains_40", 1723, 1564, 12, c => c.Quests.HasCompleted(Sq03), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tara Miles"));

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("Thank you. We just need one more sample."));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("You got them. Thank you so much. You've saved many lives in Fedimian."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("You came. Please get some dust samples from the Hallowventers. There are many reasons that a plague could spread."),
					Option(L("I'll gather the sample first"), "accept"),
					Option(L("I'd like to stop"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("It is important to cure diseases, but it is more important to prevent it in the first place."));
				}
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("This is an opportunity for us. The Cockats are suffering from a disease. Please get enough tails from them to use for our research."),
					Option(L("I'll get it"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("Even if we fail at preventing the disease, we should be able to quickly make a medicine with our research results."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq04) || character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("The samples first. Then we will know what we are fighting."));
				return;
			}

			await dialog.Msg(L("Fedimian will take the sick whether we are ready or not."));
		});

		// Grita, at the Karsta Hall Site
		//-------------------------------------------------------------------------
		AddConditionalNpc(147449, L("Grita"), "REMAINS40_GRITA", "f_remains_40", 3510, 2864, 20, c => c.Quests.Has(ToTheTower1) && !c.Quests.HasCompleted(ToTheTower2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(ToTheTower1))
			{
				await dialog.Msg(L("You came. This is the entrance to the tower."));
				await dialog.CompleteQuest(ToTheTower1);
				return;
			}

			if (!character.Quests.Has(ToTheTower2) && character.Quests.MeetsPrerequisites(ToTheTower2))
			{
				var answer = await dialog.SelectQuestOffer(ToTheTower2, L("This is the entrance to the tower. I won't be able to use magic, but I will definitely take you to Goddess Gabija."),
					Option(L("Follow Grita to the Mage Tower"), "accept"),
					Option(L("About Goddess Gabija"), "explain"),
					Option(L("Reject"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Gabija did not disappear on purpose. She had to protect the tower from the constant demon attacks. There was no choice for her other than to go."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(ToTheTower2);
					character.Quests.CompleteObjective(ToTheTower2, "enterTheTower");
					await dialog.Msg(L("The other magicians have either died or ran away. You are our last hope. Let's go."));
				}
				return;
			}

			if (character.Quests.IsActive(ToTheTower2))
			{
				await dialog.Msg(L("Go on in. I will be waiting on the first floor."));
				return;
			}

			await dialog.Msg(L("The tower is just past this hall."));
		});

		// The secret chest in the Crystal Stream
		//-------------------------------------------------------------------------
		AddNpc(40030, L("Secret Chest"), "REMAINS_40_HQ_01_TB", "f_remains_40", -466, 139, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Chest"));

			if (character.Quests.IsActive(Hq01) && !character.Quests.IsCompletable(Hq01))
			{
				if (character.Inventory.CountItem(ItemId.REMAINS_40_HQ_01_ITEM2) < 1)
				{
					await dialog.Msg(L("The lid is held by a lock with no keyhole you can find without the key."));
					return;
				}

				var opened = await character.TimeActions.StartAsync(L("Opening the chest with the key..."), L("Cancel"), "SITGROPESET2", TimeSpan.FromSeconds(3));

				if (opened != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Hq01, "openTheChest");

				await dialog.Msg(L("The key turns. Whatever Zubeck saved for the road he never took is in here."));
				await dialog.CompleteQuest(Hq01);
				return;
			}

			await dialog.Msg(L("A chest wedged under the bank of the Crystal Stream, locked tight."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The Leipsna Chapel Site, where the Moa took the toolbox.
		AddQuestTrigger("REMAINS40_SQ_02", "f_remains_40", -1744, -3502, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
				character.Quests.StartQuestTrack(Sq02);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Reads one of the monuments Coben sends the player to, completing the
	/// quest phase it belongs to.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="questId"></param>
	/// <param name="objectiveIdent"></param>
	/// <param name="inscription"></param>
	private async Task ReadMonument(Dialog dialog, QuestId questId, string objectiveIdent, string inscription)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Forgotten Thief's Monument"));

		if (character.Quests.IsActive(questId) && !character.Quests.IsCompletable(questId))
		{
			var read = await character.TimeActions.StartAsync(L("Reading the monument..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

			if (read != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(questId, objectiveIdent);

			await dialog.Msg(inscription);
			return;
		}

		await dialog.Msg(L("A thief's monument, one of a line of them along the old road."));
	}

	/// <summary>
	/// Searches one of the old boxes at the Abandoned Farm for the diary
	/// Coben is after.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SearchOldBox(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Old Box"));

		if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
		{
			var searched = await character.TimeActions.StartAsync(L("Searching the box..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

			if (searched != TimeActionResult.Completed)
				return;

			if (character.Inventory.CountItem(ItemId.REMAINS40_MQ_01_ITEM) < 1)
			{
				character.Inventory.Add(ItemId.REMAINS40_MQ_01_ITEM, 1, InventoryAddType.PickUp);
				await dialog.Msg(L("A diary, wrapped in oilcloth against the damp. This is what Coben asked for."));
				return;
			}

			await dialog.Msg(L("Nothing else in here worth carrying."));
			return;
		}

		await dialog.Msg(L("An old box left behind when the farm was abandoned."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8452: Old Story (1)
//-----------------------------------------------------------------------------
public class Remains40Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8452);
		SetName(L("Old Story (1)"));
		SetDescription(L("Coben is looking for a brother he has not seen in years, and for the diary he left at the Abandoned Farm."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_01", "f_remains_40", L("Talk to Coben"), L("You can see Coben pointing at you, hinting something. Talk to Coben."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_DRUNK_01", "f_remains_40", L("Find the diary Coben is looking for"), L("Coben needs the diary of his relative. Find the diary at the Abandoned Farm."));
		SetPhase(QuestStatus.Success, "REMAINS_40_DRUNK_01", "f_remains_40", L("Hand the diary over to Coben"), L("You found the diary at the Abandoned Farm. Hand it over to Coben."));

		AddPrerequisite(new LevelPrerequisite(97));

		AddObjective("findDiary", L("Find the diary Coben is looking for"), new CollectItemObjective("REMAINS40_MQ_01_ITEM", 1));

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new SelectItemReward("R_TOP02_150", "R_TOP02_151", "R_TOP02_152"));
		AddReward(new TakeItemReward("REMAINS40_MQ_01_ITEM"));
	}
}

// 8453: Old Story (2)
//-----------------------------------------------------------------------------
public class Remains40Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8453);
		SetName(L("Old Story (2)"));
		SetDescription(L("The diary points at a monument at the Crumbled Chapel."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_01", "f_remains_40", L("Talk to Coben"), L("Coben found something inside the diary. Talk to Coben again."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_MQ_02", "f_remains_40", L("Check the monument at the Crumbled Chapel"), L("Coben wants to check the monument that is written about his relative who was a thief, but he can't go due to the monsters. Check what is written on the epitaph for Coben."));
		SetPhase(QuestStatus.Success, "REMAINS_40_DRUNK_01", "f_remains_40", L("Tell Coben what was written on the monument"), L("It was the monument with the person's promise to himself written on it. Tell Coben about it."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8452, QuestStatus.Completed));

		AddObjective("readSecondMonument", L("Check the monument at the Crumbled Chapel"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 1));
	}
}

// 8454: Old Story (3)
//-----------------------------------------------------------------------------
public class Remains40Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8454);
		SetName(L("Old Story (3)"));
		SetDescription(L("The second monument is worn down, and Coben says Cockatrice Fat will bring the letters back."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_01", "f_remains_40", L("Talk to Coben"), L("Coben is excited to hear that the person once challenged himself to enter the Mage Tower. Talk to Coben again."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_MQ_03", "f_remains_40", L("Check the writings on the second monument"), L("Coben told you to look for the next monument, and that if the monument is unreadable due to its deterioration, you can read the writings again when you rub them with Cockatrice Fat."));
		SetPhase(QuestStatus.Success, "REMAINS_40_DRUNK_01", "f_remains_40", L("Tell Coben what was written on the monument"), L("The second monument was written about the person's promise to himself to do something at the Mage Tower in order to become the greatest thief of all time. Tell Coben about it."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8453, QuestStatus.Completed));

		AddObjective("readThirdMonument", L("Check the writings on the second monument"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new TakeItemReward("REMAINS40_MQ_03_ITEM"));
	}
}

// 8455: Old Story (4)
//-----------------------------------------------------------------------------
public class Remains40Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8455);
		SetName(L("Old Story (4)"));
		SetDescription(L("Coben has moved on to Negyvas Field, and the third monument stands in front of him."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_02", "f_remains_40", L("Talk to Coben"), L("Coben disappeared after telling you that he will follow his brother's footsteps. Find Coben and talk to him."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_MQ_04", "f_remains_40", L("Check what is written on the third monument"), L("Coben told you that the third monument is located at the left side of Negyvas Field. Check what is written on the third monument on behalf of Coben."));
		SetPhase(QuestStatus.Success, "REMAINS_40_DRUNK_02", "f_remains_40", L("Tell Coben what was written on the monument"), L("As a thief, Coben's brother seemed to have found a way to achieve complete victory at the Mage Tower. Tell Coben about it."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8454, QuestStatus.Completed));

		AddObjective("readFourthMonument", L("Check what is written on the third monument"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 1));
	}
}

// 8456: Old Story (5)
//-----------------------------------------------------------------------------
public class Remains40Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8456);
		SetName(L("Old Story (5)"));
		SetDescription(L("The fourth monument stands at the right side of Negyvas Field."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_02", "f_remains_40", L("Talk to Coben"), L("Coben seems to be excited to hear that. Talk to Coben again."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_MQ_05", "f_remains_40", L("Check what is written on the fourth monument"), L("Coben told you that you can't use fat anymore to read what's on the next monument. Check what is written on the fourth monument at the right side of Negyvas Field."));
		SetPhase(QuestStatus.Success, "REMAINS_40_DRUNK_02", "f_remains_40", L("Tell Coben what was written on the monument"), L("A method of entering the Mage Tower was written on the monument. Tell Coben about it."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8455, QuestStatus.Completed));

		AddObjective("readFifthMonument", L("Check what is written on the fourth monument"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 1));
	}
}

// 8457: Old Story (6)
//-----------------------------------------------------------------------------
public class Remains40Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8457);
		SetName(L("Old Story (6)"));
		SetDescription(L("Coben's brother left behind a jar that drinks magic, and it has to be charged before it is of any use."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_02", "f_remains_40", L("Talk to Coben"), L("It seems that there is a way for one to enter the Mage Tower. Talk to Coben again."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_MQ_06", "f_remains_40", L("Recharge the Strong Magical Power Absorption Jar"), L("Coben showed you a device his brother left him. Could this device help one gain access to the Mage Tower? Charge the device first."));
		SetPhase(QuestStatus.Success, "REMAINS_40_DRUNK_02", "f_remains_40", L("Talk to Coben"), L("The device is now fully charged. Talk to Coben."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8456, QuestStatus.Completed));

		AddObjective("chargeJar", L("Recharge the Strong Magical Power Absorption Jar"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new TakeItemReward("REMAINS40_MQ_06_ITEM"));
	}
}

// 8458: Old Story (7)
//-----------------------------------------------------------------------------
public class Remains40Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8458);
		SetName(L("Old Story (7)"));
		SetDescription(L("The last monument stands at the Karsta Hall Site, and something has been left to guard it."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_03", "f_remains_40", L("Talk to Coben"), L("You may meet Coben's brother soon. Talk to Coben."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_MQ_07", "f_remains_40", L("Check the writings on the Commanding Monument"), L("The name of the monument is strange which should be the last monument. Check what is written on it."));
		SetPhase(QuestStatus.Success, "REMAINS_40_DRUNK_03", "f_remains_40", L("Talk to Coben"), L("Sadly, the last monument reads that Coben's brother died after failing to invade the Mage Tower. Tell Coben about it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "REMAINS40_MQ_07_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8457, QuestStatus.Completed));

		AddObjective("killDevilglove", L("Defeat Devilglove"), new KillObjective(1, "boss_Devilglove") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("REMAINS40_MQ_07_ITEM", 1));
		AddReward(new ItemReward("misc_liena_top_1", 1));
	}
}

// 8459: New Market District (1)
//-----------------------------------------------------------------------------
public class Remains40Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8459);
		SetName(L("New Market District (1)"));
		SetDescription(L("The stonemasons called to rebuild Fedimian cannot get their carts past the Cockatrices."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Talk to Stonemason Canolyn"), L("You have met a very perplexed looking Stonemason Canolyn. Talk to Stonemason Canolyn."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Defeat Cockatrice"), L("Stonemason Canolyn can't complete his trip to Fedimian due to the Cockatrices around. Defeat the Cockatrices along the road to Fedimian."));
		SetPhase(QuestStatus.Success, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Report back to Stonemason Canolyn"), L("It seems as if you have reduced a fair amount of Cockatrices. Report back to Stonemason Canolyn."));

		AddPrerequisite(new LevelPrerequisite(97));

		AddObjective("killCockatrices", L("Defeat Cockatrice"), new KillObjective(9, "Cockatries"));

		AddReward(new ItemReward("expCard6", 1));
	}
}

// 8460: New Market District (2)
//-----------------------------------------------------------------------------
public class Remains40Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8460);
		SetName(L("New Market District (2)"));
		SetDescription(L("A Moa took the stonemason's toolbox and fled to the Leipsna Chapel Site."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Talk to Stonemason Canolyn"), L("Stonemason Canolyn says that her equipment has been stolen by monsters. Talk to her again."));
		SetPhase(QuestStatus.InProgress, "REMAINS40_SQ_02", "f_remains_40", L("Defeat Moa"), L("Stonemason Canolyn told you that a monster called Moa stole his toolbox. Defeat the Moa that fled to the Leipsna Chapel Site and look for the toolbox for him."));
		SetPhase(QuestStatus.Success, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Report back to Stonemason Canolyn"), L("Unfortunately, there is no equipment box. Go back to Stonemason Canolyn."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "REMAINS40_SQ_02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8459, QuestStatus.Completed));

		AddObjective("killMoa", L("Defeat Moa"), new KillObjective(1, "boss_moa") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("misc_liena_top_2", 1));
	}
}

// 8461: Preventive Measures (1)
//-----------------------------------------------------------------------------
public class Remains40Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8461);
		SetName(L("Preventive Measures (1)"));
		SetDescription(L("A doctor on her way to Fedimian wants the disease-carrying Cockatrices thinned out first."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_TARA_01", "f_remains_40", L("Talk to Tara Miles"), L("Tara Miles wants your help."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_TARA_01", "f_remains_40", L("Defeat the monsters nearby"), L("Tara Miles is looking for someone who can defeat the monsters that are spreading a dangerous disease. Defeat Cockatrices nearby."));
		SetPhase(QuestStatus.Success, "REMAINS_40_TARA_01", "f_remains_40", L("Talk to Tara Miles"), L("You have defeated many of the monsters that are spreading disease. Return to Tara Miles."));

		AddPrerequisite(new LevelPrerequisite(97));

		AddObjective("killCockatrices", L("Defeat Cockatrice"), new KillObjective(10, "Cockatries"));

		AddReward(new ItemReward("expCard6", 1));
	}
}

// 8462: Preventive Measures (2)
//-----------------------------------------------------------------------------
public class Remains40Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8462);
		SetName(L("Preventive Measures (2)"));
		SetDescription(L("The doctor wants dust samples off the Hallowventers before she names the cause."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_TARA_02", "f_remains_40", L("Talk to Tara Miles"), L("Tara Miles told you that she will be waiting at Negyvas Field. Find Tara Miles and talk to her."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_TARA_02", "f_remains_40", L("Collect dust samples from Hallowventers"), L("Tara Miles told you that there are many reasons for the spread of the disease. First, obtain the dust samples from Hallowventers."));
		SetPhase(QuestStatus.Success, "REMAINS_40_TARA_02", "f_remains_40", L("Hand over the dust samples to Tara Miles"), L("You obtained many dust samples from Hallowventers. Hand them over to Tara Miles."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8461, QuestStatus.Completed));

		AddPityDrop("REMAINS40_SQ_04_ITEM", 1.0f, 0, 1, "Hallowventor");

		AddObjective("collectDust", L("Collect dust samples from Hallowventers"), new CollectItemObjective("REMAINS40_SQ_04_ITEM", 7));

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new TakeItemReward("REMAINS40_SQ_04_ITEM"));
	}
}

// 8463: Preventive Measures (3)
//-----------------------------------------------------------------------------
public class Remains40Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8463);
		SetName(L("Preventive Measures (3)"));
		SetDescription(L("The Cockats past the Camp of Apiarists are already sick, and their tails are what the research needs."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_TARA_02", "f_remains_40", L("Talk to Tara Miles"), L("It seems that Tara Miles' investigation is not done yet. Talk to Tara Miles again."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_TARA_02", "f_remains_40", L("Collect Cockat tails"), L("Tara Miles says that the Cockat is already dying and wants to have its tail. Obtain the Cockat tail from the other side of the Camp of Apiarists."));
		SetPhase(QuestStatus.Success, "REMAINS_40_TARA_02", "f_remains_40", L("Hand the Cockat tail to Tara Miles"), L("You seem to have obtained enough Cockat tails. Go back to Tara Miles."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8462, QuestStatus.Completed));

		AddPityDrop("REMAINS40_SQ_05_ITEM", 1.0f, 0, 1, "Big_Cockatries");

		AddObjective("collectTails", L("Collect Cockat tails"), new CollectItemObjective("REMAINS40_SQ_05_ITEM", 2));

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new TakeItemReward("REMAINS40_SQ_05_ITEM"));
	}
}

// 8472: Goddess Gabija (2)
//-----------------------------------------------------------------------------
public class ToTheTower02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8472);
		SetName(L("Goddess Gabija (2)"));
		SetDescription(L("Grita will take you up the Mage Tower to the goddess who has been holding it alone."));
		SetType(QuestType.Main);
		SetLocation("f_remains_40", "d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS40_GRITA", "f_remains_40", L("Talk to Grita at Fedimian Suburbs"), L("Grita is waiting at the Karsta Hall Site of the Fedimian Suburbs. Talk to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_GRITA_01", "d_firetower_41", L("Go to the Mage Tower"), L("Grita told you that Goddess Gabija is resisting the attacks of the demons at the Mage Tower. Go to the Mage Tower to help the goddess."));
		SetPhase(QuestStatus.Success, "FTOWER41_GRITA_01", "d_firetower_41", L("Go to the Mage Tower"), L("Grita told you that Goddess Gabija is resisting the attacks of the demons at the Mage Tower. Go to the Mage Tower to help the goddess."));

		AddPrerequisite(new QuestStatusPrerequisite(8471, QuestStatus.Completed));

		AddObjective("enterTheTower", L("Go to the Mage Tower"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 19041: Brother, Then
//-----------------------------------------------------------------------------
public class Remains40Hq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19041);
		SetName(L("Brother, Then"));
		SetDescription(L("Coben hands over the key to what he was saving for a journey he will not take now."));
		SetType(QuestType.Sub);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_DRUNK_03", "f_remains_40", L("Talk to Coben"), L("Coben wants to give you something. Talk to him."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_HQ_01_TB", "f_remains_40", L("Find the Treasure Chest"), L("Find the treasure chest hidden somewhere in the Crystal Stream. It can be opened with the key Coben gave you."));
		SetPhase(QuestStatus.Success, "REMAINS_40_HQ_01_TB", "f_remains_40", L("Find the Treasure Chest"), L("Find the treasure chest hidden somewhere in the Crystal Stream. It can be opened with the key Coben gave you."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8458, QuestStatus.Completed));

		AddObjective("openTheChest", L("Find the Treasure Chest"), new ManualObjective());

		AddReward(new TakeItemReward("REMAINS_40_HQ_01_ITEM2"));
	}
}

// 60183: Thinking Ahead
//-----------------------------------------------------------------------------
public class Remains40Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60183);
		SetName(L("Thinking Ahead"));
		SetDescription(L("The stonemason wants the Leipsna Chapel Site cleared so a trading post can stand there."));
		SetType(QuestType.Repeat);
		SetLocation("f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Talk to Stonemason Canolyn"), L("Stonemason Canolyn is waiting for help at the Fedimian Suburbs."));
		SetPhase(QuestStatus.InProgress, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Deal with the monsters at Leipsna Chapel Site"), L("Stonemason Canolyn wishes to establish a trade hub that leads to Fedimian at Leipsna Chapel Site. Deal with the monsters there and return to Canolyn."));
		SetPhase(QuestStatus.Success, "REMAINS_40_CANOLIN_01", "f_remains_40", L("Report back to Stonemason Canolyn"), L("You have dealt with enough monsters at Leipsna Chapel Site. Go back to Stonemason Canolyn."));

		AddPrerequisite(new LevelPrerequisite(97));
		AddPrerequisite(new QuestStatusPrerequisite(8460, QuestStatus.Completed));

		AddObjective("clearChapelSite", L("Deal with the monsters at Leipsna Chapel Site"), new KillObjective(13, "Hallowventor", "Cockatries", "Big_Cockatries"));

		AddReward(new ItemReward("expCard6", 2));
	}
}
