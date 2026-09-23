//--- Melia Script ----------------------------------------------------------
// Lemprasa Pond Quest NPCs
//--- Description -----------------------------------------------------------
// The settlers waiting to enter Orsha and the officer processing them.
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

public class FSiauliai16QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(60070);
	private readonly static QuestId Mq02 = new QuestId(60071);
	private readonly static QuestId Mq03 = new QuestId(60072);
	private readonly static QuestId Mq04 = new QuestId(60073);
	private readonly static QuestId Mq05 = new QuestId(60074);
	private readonly static QuestId Mq06 = new QuestId(60075);
	private readonly static QuestId Mq07 = new QuestId(60076);
	private readonly static QuestId Sq01 = new QuestId(60077);
	private readonly static QuestId Sq02 = new QuestId(60078);
	private readonly static QuestId Sq03 = new QuestId(60079);
	private readonly static QuestId Sq04 = new QuestId(60080);
	private readonly static QuestId Sq05 = new QuestId(60081);
	private readonly static QuestId Sq06 = new QuestId(60082);
	private readonly static QuestId Sq07 = new QuestId(60083);
	private readonly static QuestId Sq08 = new QuestId(60084);

	public const string BonfireCountVar = "Gabija.Quests.Siau16Sq03.Lit";
	private const string BonfireVar = "Gabija.Quests.Siau16Sq03.Bonfire";
	private const string GoodsVar = "Gabija.Quests.Siau16Sq05.Goods";
	public const string SettlerCountVar = "Gabija.Quests.Siau16Sq08.Found";
	private const string SettlerVar = "Gabija.Quests.Siau16Sq08.Settler";

	private static readonly double[,] Bonfires =
	{
		{ -1204.29, 1365.78 }, { -1203.49, 1192.55 }, { -1059.28, 1024.21 }, { -859.52, 889.17 }, { -1133.68, 1853.05 },
		{ -929.16, 1907.17 }, { -834.44, 1569.94 }, { -1137.77, 1655.69 }, { -485.77, 1805.56 }, { -619.23, 1553.02 },
		{ -694.37, 1287.05 }, { -299.29, 1027.97 }, { -866.85, 1209.56 }, { -343.01, 1183.32 }, { -676.33, 898 },
	};

	private static readonly double[,] Goods =
	{
		{ 47161, 1603.95, -601.48 }, { 47161, 1506.28, -787.91 }, { 47161, 1308.95, -741.81 }, { 47161, 1641.76, -771.63 },
		{ 47161, 1266.90, -892.61 }, { 153041, 1312.93, -1056.44 }, { 153041, 1368.17, -1159.32 }, { 153041, 1472.75, -934.28 },
		{ 153041, 1378.48, -824.03 }, { 153041, 1737.61, -560.96 }, { 153041, 1726.48, -679.95 }, { 153041, 1517.05, -872.69 },
	};

	private static readonly double[,] Settlers =
	{
		{ 151087, 36.12, 989.14, 90 }, { 151087, 905.57, 1464.53, 265 }, { 151087, 126.50, 1763.65, 90 },
		{ 151094, 262.29, 857.93, -73 }, { 151094, 432.98, 1719.86, -5 }, { 151094, 997.82, 1813.57, -18 },
		{ 151097, 777.67, 1946.41, 11 }, { 151097, 43.10, 1274.85, 204 }, { 151097, 897, 1032.21, 237 },
	};

	protected override void Load()
	{
		// Settler Bowein
		//-------------------------------------------------------------------------
		AddNpc(151082, L("Settler Bowein"), "SIAULIAI16_BOWEIN", "f_siauliai_16", -2334.04, -1400.90, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Settler Bowein"));

			if (character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("I don't really know where he has gone either. I think Brophen followed the village elder to the Malkos Felled Area, maybe he might know?"));
				await dialog.Msg(L("Anyways, I want to thank you in the meantime. I hope that the grace of the goddesses will bless you on your travels."));
				await dialog.CompleteQuest(Mq01);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Press 'M' to open the map and check your destination"), 7);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Oh, good morning. It seems you've finally woken up. I've heard that they're making preparations to process people to enter Orsha."));
				await dialog.Msg(L("I've received a lot of help from you on the way here. You've even saved me when I was surrounded by monsters..."));
				await dialog.Msg(L("In times like these with all the goddesses gone, the villagers are all grateful for your help. I guess today will be the last of that though."));
				await dialog.Msg(L("Anyway, how are your dreams? You've been suffering from nightmares for a while..."));
				await dialog.Msg(L("You've had them last night as well? Oh that's... not good. How about getting some consultation in Orsha since we'll be there soon?"));
				await dialog.Msg(L("I'm a bit worried since there are so many people from other villages coming to Orsha. I don't know how many migration orders they have issued..."));
				await dialog.Msg(L("If you're bored of just waiting around, try asking Mayor Romanas about whats going on. He said that he's going to meet the lord of Orsha soon."));

				character.Quests.Start(Mq01);
				character.Quests.CompleteObjective(Mq01, "talk");
				return;
			}

			if (character.Quests.HasCompleted(Mq01))
			{
				await dialog.Msg(L("I wonder how many migration orders Orsha issued... I don't know how much longer we will have to just wait here."));
				return;
			}

			await dialog.Msg(L("Thank you so much for protecting the townfolk on the journey here. They are all grateful for your kindness."));
		});

		// Settler Brophen
		//-------------------------------------------------------------------------
		AddNpc(151083, L("Settler Brophen"), "SIAULIAI16_BROPHEN", "f_siauliai_16", -1540.71, -1274.20, 7, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Settler Brophen"));

			if (character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Thank you so much for saving me! Forget about Orsha, I almost... Whew..."));
				await dialog.Msg(L("You helped us from the monsters on the way here as well. Even though the goddesses are nowhere to be seen, I can't help but think that they are still somewhere when I look at you."));
				await dialog.Msg(L("I don't know what the lord is thinking, making us wait in such a dangerous place. I'm going to move somewhere else as soon as my family gets here."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("That is a superb choice. You will be able to choose what Status you want to enhance by allocating points every time you level up from now on."));
				await dialog.Msg(L("It is difficult to revert decisions after allocating a Status Point... So I imagine that choosing wisely is important."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("They said that the lord would send soldiers and officials to help if we managed to get here... I don't see any sign of them."));
				await dialog.Msg(L("There were orders issued by the lord. Refugees from the west of Orsha are to wait here at Lemprasa Pond."));
				await dialog.Msg(L("Mayor Romanas? I'm not sure. I did see Layla accompanying him a while ago."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("Hmm? Do you hear something strange? I think something is coming this way..."),
					Option(L("Monsters might come out"), "accept"),
					Option(L("There's no need to hurry"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq02);

				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("By the way, didn't you just level up while fighting those Kepas?"));
				await dialog.Msg(L("I've heard something from a friend that enlisted a while ago... He said that you can choose one Status Point and grow stronger when you level up."));

				var answer = await dialog.SelectQuestOffer(Mq03, L("Why don't you try it out?"),
					Option(L("I'll check"), "accept"),
					Option(L("I will ask later"), "leave")
				);

				if (answer != "accept")
					return;

				character.Variables.Perm.SetInt(NormalTxFunctionsScript.StatPointsSpentVarName, (int)character.Properties.GetFloat(PropertyName.UsedStat));
				character.Quests.Start(Mq03);

				if (character.Quests.IsCompletable(Mq03))
				{
					await dialog.Msg(L("That is a superb choice. You will be able to choose what Status you want to enhance by allocating points every time you level up from now on."));
					await dialog.CompleteQuest(Mq03);
					return;
				}

				dialog.ShowHelp("TUTO_STATPOINT");
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Press 'F1' and try using a Status Point"), 8);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("Oh yes, you mentioned that you were looking for Mayor Romanas didn't you? The mayor headed towards the Temporary Settler Camp with Layla a while ago."));

				var answer = await dialog.SelectQuestOffer(Mq04, L("Could you possibly give this Grass Leaf Ointment to Layla? My legs are still shaking after that ordeal..."),
					Option(L("I will pass it"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Inventory.Add(ItemId.SIAU16_MQ_04_ITEM, 1, InventoryAddType.PickUp);
					character.Quests.CompleteObjective(Mq04, "deliver");
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Follow the road to the right to deliver the Grass Leaf Ointment to Settler Layla"), 8);
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Hmm? Do you hear something strange? I think something is coming this way..."));
				character.Quests.ReplayQuestTrack(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("You should give some serious thought on where you want to focus on from now on. It is no easy choice."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("These are just some rumors that I heard on the way here. They say that the towns on the far side of the river are also lying in ruins because of monsters."));
				await dialog.Msg(L("We would have been in a lot of trouble as well if the lord hadn't issued the migration orders."));
				return;
			}

			if (character.Quests.HasCompleted(Mq02))
			{
				await dialog.Msg(L("I don't know how many times you've protected our townsfolk on the way here... Yet we receive your help again. Thank you so much."));
				await dialog.Msg(L("By the way, how are the nightmares now? How about consulting the bishop in Orsha when you get there?"));
				return;
			}

			await dialog.Msg(L("I caught a glimpse of Orsha as we were traveling up the slope a while ago... There is no sign of the enormous castle walls I remember from when I was young, and it is covered by trees the size of houses."));
			await dialog.Msg(L("I thought it would be safe with the migration orders and all, but I don't know. Do you think it will be okay?"));
		});

		// Settler Layla
		//-------------------------------------------------------------------------
		AddNpc(20061, L("Settler Layla"), "SIAULIAI16_RHEILAR", "f_siauliai_16", -911.11, -393.23, 14, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Settler Layla"));

			if (character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("This is... Did Brophen ask you to give this to me? Thank you very much."));
				await dialog.Msg(L("Mayor Romanas went ahead towards the Temporary Settler Camp. I was here waiting for Brophen."));
				await dialog.Msg(L("Oh, by the way, I've overheard Brophen going on about this. Have you heard about Skill Points?"));
				await dialog.Msg(L("I've heard that you can use them to learn new skills or enhance skills you've already learned. I'm sure it will help you in combat if you carefully choose what to invest in."));
				await dialog.CompleteQuest(Mq04);

				dialog.ShowHelp("TUTO_SKILL");
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Press 'F3' to check your skills"), 8);
				return;
			}

			if (character.Quests.HasCompleted(Mq04))
			{
				await dialog.Msg(L("On Medzio Diena, it was a nightmare because of the earthquake. It was still easier to get by back then than it is now..."));
				return;
			}

			await dialog.Msg(L("Sometimes I wonder if it would have been better if the migration orders had been issued on Medzio Diena. It's being delayed because they're haphazardly trying to solve problems four years too late."));
		});

		// Mayor Romanas
		//-------------------------------------------------------------------------
		AddNpc(151096, L("Mayor Romanas"), "SIAULIAI16_ROMANAS", "f_siauliai_16", 271.52, 319.19, 1, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Mayor Romanas"));

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("Oh good, you're here. We're thankful for the fair bit of trouble you've gone through to get us here."));
				await dialog.Msg(L("So, are the nightmares still giving you trouble? That's a shame. I wish there was something I could do for the person I owe so much..."));
				await dialog.Msg(L("Of course, how could I have forgotten. How about meeting Bishop Urbonas of Orsha?"));
				await dialog.Msg(L("I've heard that he is the most respected cleric from around these parts. I'm sure he'll be able to resolve your nightmares."));
				await dialog.Msg(L("From what I've heard... They are letting skilled individuals like yourself get into Orsha faster."));
				await dialog.Msg(L("As it so happens, Officer Lutas wants to meet you anyway, so why don't you meet him at the Orsha Migration Office? I've been singing your praise to him for a while now."));

				while (true)
				{
					var answer = await dialog.SelectQuestOffer(Mq05, L("By the way, I would appreciate it if you could help a few refugees if it isn't too much of a bother. There are many who are just too kind-hearted to ask anyone for help. I'm sure they would all greatly appreciate it."),
						Option(L("I will go"), "accept"),
						Option(L("I have other things to do"), "leave"),
						Option(L("Tell me about Medzio Diena"), "explain")
					);

					if (answer == "explain")
					{
						await dialog.Msg(L("Medzio Diena... It's already been four years. I did not see it in person, but they say that a giant tree suddenly rose out of the ground in the capital."));
						await dialog.Msg(L("The tree was so enormous that many cities in the kingdom, not to mention the capital, were destroyed by the tree's roots. Many people were injured or killed."));
						await dialog.Msg(L("But that was far from the end... Docile monsters became violent, and some swelled in numbers at a ridiculous rate."));
						await dialog.Msg(L("The scariest part is that the demons began to appear. Demons that have lain dormant thanks to the goddesses began to appear all over the kingdom."));
						await dialog.Msg(L("The goddesses... They didn't answer either. Even before Medzio Diena, they had started to be unresponsive one by one... But still..."));
						await dialog.Msg(L("Allow me to stop here as these are painful memories for me. Things are pretty grim these days as well, but I really do not wish to relive that fateful day."));
						continue;
					}

					if (answer == "accept")
					{
						character.Quests.Start(Mq05);
						character.Quests.CompleteObjective(Mq05, "meetLutas");
						await dialog.Msg(L("What a gruesome sight. I've heard that there is not a place untouched by the roots and monsters after Medzio Diena."));
						await dialog.Msg(L("Orsha and Klaipeda are the only cities around that you could say still stand. Times like these make people look around for things to depend on, but the goddesses are nowhere to be seen..."));
						await dialog.Msg(L("Oh my, look at how time flies. It's about time I went off to see the soldiers. Why don't you go see Officer Lutas if you wish to meet the bishop of Orsha."));
					}
					return;
				}
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Rumors that the villages on the other side of mountains and rivers had been destroyed by monsters spread like wildfire. I tried calming the people thinking that the soldiers of Orsha would come to help but..."));
				await dialog.Msg(L("The neighboring village had been ransacked. I could not help but think that the even the goddesses had forsaken us."));
				await dialog.Msg(L("Thankfully, migration orders were issued to the surviving villages, but it was too late for many. They should have been issued sooner..."));
				return;
			}

			if (character.Quests.HasCompleted(Mq05))
			{
				await dialog.Msg(L("Orsha's lord should have issued the migration orders sooner. There were too many casualties. Way too many..."));
				await dialog.Msg(L("It is in times like this when you need something to have faith in... Where have the goddesses gone to..."));
				return;
			}

			await dialog.Msg(L("All I can see in the eyes of people coming here is wariness and fear. To make matters worse, the goddesses are not responding either."));
			await dialog.Msg(L("What would you think if you were the mayor?"));
			await dialog.Msg(L("Whew... Never mind. It was just the complaints of an old man. I am more worried about our young lord..."));
		});

		// Officer Lutas
		//-------------------------------------------------------------------------
		AddNpc(151086, L("Officer Lutas"), "SIAULIAI16_LUTAS", "f_siauliai_16", 2025.06, 331.47, 16, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Officer Lutas"));

			if (character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("I've heard that there is someone helping the refugees, it must have been you! I thank you on behalf of our lord."));
				await dialog.Msg(L("So... You're planning on stopping by at Orsha? As you can see, there are too many refugees... I have my hands completely full."));
				await dialog.Msg(L("I understand that the refugees have complaints about having to abandon their hometowns because of the migration orders... I'm sure the lord wouldn't have issued such orders unless she had no choice."));
				await dialog.Msg(L("It's not like the military and adminsitration of Orsha faired any better than the other cities on Medzio Diena. We've worked hard to get back to this state."));
				await dialog.Msg(L("It won't be easy to send officials or troops to other regions for at least a few years. Especially since the demons keep heading south..."));
				await dialog.Msg(L("That's why the lord decided that the best course of action was to gather everyone at Orsha. Even if it means causing all this chaos."));
				await dialog.Msg(L("The reconstruction of Orsha is still ongoing. It's not possible to let all the refugees who arrive enter Orsha due to some complications."));
				await dialog.Msg(L("I'm sorry, but I will get to you once these people have been processed."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("That was a close call, since the soldiers were all in the middle of changing shifts. If it wasn't for you, that monster would have charged straight towards Orsha."));
				await dialog.Msg(L("I take back what I said earlier about fighting monsters like the soldiers. I wouldn't be able to survive fights with those monsters even if the goddesses blessed me with a few to spare."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("Not enough manpower, too many refugees... Sometimes I think it might be easier fighting monsters like the soldiers."));

				while (true)
				{
					var answer = await dialog.SelectQuestOffer(Mq06, L("I wish I could accommodate you sooner... But look at it from my perspective. If I let you in before the people that have been waiting over there, there'll be an uproar."),
						Option(L("I'll wait"), "accept"),
						Option(L("I have some complaints"), "leave"),
						Option(L("Tell me about the procedure"), "explain")
					);

					if (answer == "explain")
					{
						await dialog.Msg(L("You can enter Orsha without being processed. But the reason they are all waiting over here is because of the refugee qualification."));
						await dialog.Msg(L("You can't recieve any support from Orsha without having a refugee status. They know that they will all starve without support so that's why they're all waiting."));
						continue;
					}

					if (answer == "accept")
					{
						character.Quests.Start(Mq06);
						character.LookAround();
					}
					return;
				}
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("I guess it'll be slow around here for a while since everyone ran off... Besides, a person with your skills is always welcome in Orsha."));
				await dialog.Msg(L("By the way, the lord of Orsha is gathering talented people. I think she's offering jobs, so why don't you stop by if you aren't in a hurry?"));

				var answer = await dialog.SelectQuestOffer(Mq07, L("The lord is a strong willed and level-headed person. Go to the castle next to the Central Plaza if you wish to meet her."),
					Option(L("Tell me about the bishop of Orsha"), "accept"),
					Option(L("There's still things I need to take care of here"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq07);
					character.Quests.CompleteObjective(Mq07, "visitOrsha");
					await dialog.Msg(L("Bishop Urbonas? Why are you looking for him?"));
					await dialog.Msg(L("I don't know... I haven't seen him for a couple of days. Why don't you try meeting the lord first since they are close?"));
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Follow the green arrow to move to Orsha"), 8);
				}
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Not enough manpower, too many refugees... Sometimes I think it might be easier fighting monsters like the soldiers."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Most refugees here haven't been to Orsha before in their life. That's why most of them would have thought that they would be protected by the massive castle they've heard rumors about..."));
				await dialog.Msg(L("I'm sure you'll notice when you enter, but much has changed since Medzio Diena... It's a pity sight for all the refugees that make it here just on the basis of the migration orders."));
				return;
			}

			if (character.Quests.HasCompleted(Mq07))
			{
				await dialog.Msg(L("A person with your skills is welcome to enter Orsha at once. Go to the castle next to the Central Plaza if you wish to meet the lord."));
				return;
			}

			if (character.Quests.HasCompleted(Mq05))
			{
				await dialog.Msg(L("Four years have passed since Medzio Diena, but the reconstruction of Orsha is still underway. There are some refugees that are disappointed when they see Orsha... But it should still be better than other places in the world."));
				return;
			}

			await dialog.Msg(L("The discontent of the refugees is mounting due to the sudden nature of the migration orders. Due to lack of facilities, long processing times... Even I would be angry."));
			await dialog.Msg(L("What can we do though. It may have been too late, but this is the best we can do..."));
		});

		// The settlers queuing at the Migration Office
		//-------------------------------------------------------------------------
		AddConditionalNpc(151081, L("Settler"), "SIAU16_MQ_06_NPC_1", "f_siauliai_16", 2012.56, 298.37, 157, IsQueueWaiting, async dialog =>
		{
			dialog.SetTitle(L("Settler"));
			await dialog.Msg(L("Could you ask that person how much longer he's going to make us stand here?"));
		});

		AddConditionalNpc(151092, L("Settler"), "SIAU16_MQ_06_NPC_2", "f_siauliai_16", 2002.46, 273.60, 156, IsQueueWaiting, async dialog =>
		{
			dialog.SetTitle(L("Settler"));
			await dialog.Msg(L("It's been days since we've arrived, and yet we still haven't set foot in Orsha. Why did they make us travel all the way here just to make us stand around like this?"));
		});

		AddConditionalNpc(20062, L("Settler"), "SIAU16_MQ_06_NPC_3", "f_siauliai_16", 1990.85, 246.97, 137, IsQueueWaiting, async dialog =>
		{
			dialog.SetTitle(L("Settler"));
			await dialog.Msg(L("I'm tired and hungry... Missing a meal would've been unthinkable back home..."));
			await dialog.Msg(L("They call us settlers, but we're basically refugees..."));
		});

		// Settler Izna
		//-------------------------------------------------------------------------
		AddNpc(151080, L("Settler Izna"), "SIAULIAI16_IZNA", "f_siauliai_16", 156.84, 341.31, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Settler Izna"));

			if (character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Here you go, take this potion. It works wonders on wounds."));
				await dialog.Msg(L("Thank you so much for helping the refugees. I feel like we can depend on you more than the missing goddesses..."));
				await dialog.CompleteQuest(Sq01);
				dialog.ShowHelp("TUTO_RECOVERY");
				return;
			}

			if (character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("This should be enough Rampar Mucus. Here, you should take some for yourself."));
				await dialog.Msg(L("You can recover Stamina from Tree Root Crystals or by resting. But you won't have time for that when you are being chased by monsters, will you?"));
				await dialog.Msg(L("These Stamina pills will be worth their weight in gold at times like that. I'll pray for the goddesses' blessings upon you!"));
				await dialog.CompleteQuest(Sq02);
				dialog.ShowHelp("TUTO_RECOVERY");
				return;
			}

			if (character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thank you so much for helping out. I just hope that they'll notice the smoke..."));
				await dialog.CompleteQuest(Sq03);
				character.LookAround();
				return;
			}

			if (character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("I heard something loud... What do you think happened?"));
				await dialog.Msg(L("Oh my, a big monster? I'm so sorry that I dragged you into something like this..."));
				await dialog.Msg(L("The smoke is noticeable thanks to your efforts, but this is terrible. The fact that all we can do is wait here when such dangerous monsters are appearing... It makes no sense!"));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("It was a mess four years ago on Medzio Diena. A gigantic tree sprouting up from beneath the capital... Monsters attacking villages..."));
				await dialog.Msg(L("Even the king and his army did not help us with the world in such disarray. But I've heard that someone still had it in them to help us refugees, it's you isn't it?"));
				await dialog.Msg(L("I know it's not much... But I wanted to thank you somehow."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("Please gather some Red Leaves by catching Leaf Bugs at Ziedo Pond. I'll make you some potions that work well on wounds."),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("I don't need it"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq01);

				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				await dialog.Msg(L("Survivors from other villages are trickling towards this place. Everybody is worn out from fleeing day and night without a proper rest."));
				await dialog.Msg(L("They are all friendly faces... Yet all I can do is to craft stamina pills for them."));

				var answer = await dialog.SelectQuestOffer(Sq02, L("Could you gather some Rampar Mucus from Chinency near Ziedo Pond? That's the main material for crafting the pills."),
					Option(L("I'll help you"), "accept"),
					Option(L("I don't have time for that"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq02);

				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("Did you by any chance see some people wondering nearby? The villagers from my town all got separated because we ran into some monsters..."));
				await dialog.Msg(L("They should still be on their way here since the only safe place is Orsha... I don't know why they're not here already. I'm worried that they are perhaps lost in the forest."));

				var answer = await dialog.SelectQuestOffer(Sq03, L("I'm sorry, but could you by any chance light a campfire on Adata Highway? It's for people to see the smoke and come towards it."),
					Option(L("Sure, I'll help"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Bonfires.GetLength(0); ++i)
						character.Variables.Perm.Set(BonfireVar + i, false);
					character.Variables.Perm.SetInt(BonfireCountVar, 0);

					character.Quests.Start(Sq03);
					character.LookAround();
					dialog.ShowHelp("SIAU16_SQ_03");
				}
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				await dialog.Msg(L("I think the smoke is too faint... Those campfires may be too small to be noticed."));
				await dialog.Msg(L("We need a larger fire. A very large fire. Maybe the firewood that Official Lutas prepared on Adata Highway may be good enough."));

				var answer = await dialog.SelectQuestOffer(Sq04, L("I've already received permission to use it as well. Well, what are you waiting for?"),
					Option(L("I will come back soon"), "accept"),
					Option(L("I don't think it'll be much use"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq04);

				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("I'm not from Mayor Romanas' village. I lived in a nearby village that was destroyed not long ago."));
				await dialog.Msg(L("The hunters warned us that monsters were heading our way... But the lord never sent any soldiers."));
				await dialog.Msg(L("I barely made it away from my hometown when I heard that the lord finally issued the Orsha migration orders."));
				await dialog.Msg(L("It would have made a lot of difference if the orders had been issued sooner... Not only are the goddesses missing, now I've lost my home and fields as well."));
				await dialog.Msg(L("How are we supposed to live now..."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("I'm sure the lord had her reasons, but I wonder why she didn't issue the migration orders sooner. Maybe the messenger was being lazy?"));
				await dialog.Msg(L("There is so much I want to ask if I ever get to meet the lord."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("I've met only a few people here. I don't know where all the others may have gone..."));
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("Official Lutas says that there are too many refugees to simply allow them entrance into Orsha."));
				await dialog.Msg(L("That's why they've created smaller refugee camps near Orsha and are supplying them there. The firewood on Adata Highway was also supposed to be used in that fashion."));
				character.Quests.ClearQuestTrack(Sq04);
				return;
			}

			if (character.Quests.HasCompleted(Sq04))
			{
				await dialog.Msg(L("Officer Lutas is saying that there are too many refugees to let them all into Orsha. That's why they've made and are offering aid to several smaller refugee camps near Orsha."));
				await dialog.Msg(L("Aid... I don't know if you can call this proper aid."));
				return;
			}

			await dialog.Msg(L("It was a mess four years ago on Medzio Diena. A gigantic tree sprouting up from beneath the capital... Monsters attacking villages..."));
			await dialog.Msg(L("It was okay back then because my town was far from the capital. Now... I don't think I will be able to go back."));
		});

		// Settler Ivanayus
		//-------------------------------------------------------------------------
		AddNpc(151091, L("Settler Ivanayus"), "SIAULIAI16_IHBANAYUS", "f_siauliai_16", 199.40, 147.89, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Settler Ivanayus"));

			if (character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("Wow, these will come in handy. You've earned my thanks in many more ways than those officials ever will."));
				await dialog.CompleteQuest(Sq05);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				await dialog.Msg(L("I haven't seen you around before, are you another one of those useless officials from Orsha?"));
				await dialog.Msg(L("You're not? Oh... My apologies."));
				await dialog.Msg(L("I came from a place near the southern seashore. My house was unscathed, but I left because they said that they would protect us if we came to Orsha..."));
				await dialog.Msg(L("But as you can see, all that's prepared for us are houses unfit for people and a few campfires. There aren't even enough troops or officials here to help us either."));
				await dialog.Msg(L("We've started to use what the previous inhabitants left behind since there is a shortage of pretty much everything. There are monsters everywhere else you see."));

				var answer = await dialog.SelectQuestOffer(Sq05, L("Don't you think it's miserable? There seem to be a lot of useful things in the Lumberjack Cabin, so it's a shame we can't go and get them."),
					Option(L("I'll see what I can find"), "accept"),
					Option(L("Some things can't be helped"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Goods.GetLength(0); ++i)
						character.Variables.Perm.Set(GoodsVar + i, false);

					character.Quests.Start(Sq05);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("It's not like the Southern Seashore is damage free. The massive fruits that came from upstream have totally ruined the fishing nets."));
				await dialog.Msg(L("I gazed vacantly at the shore covered in the colorful juice from all those fruits.. They all began rotting not too long afterwards. Ugh.."));
				await dialog.Msg(L("It was still managable since there weren't any monsters though. I don't know if that's still the case."));
				return;
			}

			if (character.Quests.HasCompleted(Sq05))
			{
				await dialog.Msg(L("I would have stayed in my hometown if I knew that this is what was going happen, whether it be migration orders or not. It couldn't have been worse than the situation we're in now."));
				return;
			}

			await dialog.Msg(L("To live holed up inside Orsha's castle walls that have been destroyed by roots... Or wait out here where there are swarms of monsters..."));
			await dialog.Msg(L("It's a hard life either way. I'd rather be back home..."));
		});

		// Settler Dallanas
		//-------------------------------------------------------------------------
		AddNpc(151090, L("Settler Dallanas"), "SIAULIAI16_DALLANAS", "f_siauliai_16", 387, 323, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Settler Dallanas"));

			if (character.Quests.IsCompletable(Sq07))
			{
				await dialog.Msg(L("I can't believe that they told us to gather in a place like that with all those monsters... The lord clearly didn't think it through."));
				await dialog.CompleteQuest(Sq07);
				return;
			}

			if (character.Quests.IsCompletable(Sq08))
			{
				await dialog.Msg(L("Thank you. I'll have to ask Mayor Romanas if our villagers can stay as well."));
				await dialog.CompleteQuest(Sq08);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq06) && character.Quests.MeetsPrerequisites(Sq06))
			{
				await dialog.Msg(L("You must not be from around these parts. People from around here don't wear clothes like that."));
				await dialog.Msg(L("If you're a traveler... How about worshiping the Statue of Goddess Zemyna at Randoluma Rest Place?"));
				await dialog.Msg(L("Goddess Zemyna administers the Earth. She also looks after travelers on their journeys."));

				while (true)
				{
					var answer = await dialog.SelectQuestOffer(Sq06, L("Of course, the goddesses haven't been answering our prayers for a while... But the statues still sparkle brightly when we pray to them."),
						Option(L("I will take a look around"), "accept"),
						Option(L("I don't need that"), "leave"),
						Option(L("Tell me about the missing goddesses"), "explain")
					);

					if (answer == "explain")
					{
						await dialog.Msg(L("I can't recall the exact date but... Yes. It must have been shortly before Medzio Diena."));
						await dialog.Msg(L("There used to always be an answer when we prayed to the goddesses, but the prayers began to be unanswered. There aren't any people that claim to have seen the goddesses after that either."));
						await dialog.Msg(L("The first goddess to stop answering was Goddess Gabija. This was probably around the time of my grandfather's grandfather."));
						await dialog.Msg(L("Then Goddess Zemyna, Ausrine, and Vakarine fell silent... Now there's nobody left."));
						await dialog.Msg(L("But I still believe that the goddesses have not forsaken us. There must be some reason they aren't answering."));
						await dialog.Msg(L("I still have faith because the Goddess Statues shine as if they are acknowledging your prayers. That is what is upholding my faith."));
						continue;
					}

					if (answer == "accept")
					{
						character.Quests.Start(Sq06);
						dialog.ShowHelp("MINI_E_STATUE");
					}
					return;
				}
			}

			if (!character.Quests.Has(Sq07) && character.Quests.MeetsPrerequisites(Sq07))
			{
				await dialog.Msg(L("Have you been to Randoluma Rest Place by any chance? A soldier from Orsha said that he'd be back for us if we wait there."));
				await dialog.Msg(L("I don't know what the Orsha soldiers are thinking. We can't get there because of the monsters."));

				var answer = await dialog.SelectQuestOffer(Sq07, L("All we know is how to plough fields and cast nets, so what can we do? So would you mind clearing the monsters from Randoluma Rest Place for us?"),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("I'm not talented enough for that"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq07);

				return;
			}

			if (!character.Quests.Has(Sq08) && character.Quests.MeetsPrerequisites(Sq08))
			{
				await dialog.Msg(L("They're saying that people from my village have arrived at Randoluma Rest Place. But they've scattered due to the monsters."));
				await dialog.Msg(L("I'm sure they're trembling in fear out there... Could you look for them and bring them here after you find them?"));

				var answer = await dialog.SelectQuestOffer(Sq08, L("Even if Mayor Romanas and the villagers run into trouble, at least it seems safer here than at Randoluma Rest Place."),
					Option(L("I will bring it"), "accept"),
					Option(L("It will come if you wait"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Settlers.GetLength(0); ++i)
						character.Variables.Perm.Set(SettlerVar + i, false);
					character.Variables.Perm.SetInt(SettlerCountVar, 0);

					character.Quests.Start(Sq08);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq06))
			{
				await dialog.Msg(L("I don't know who sculpted all those goddess statues either. From what I've heard, they've been around for a long time..."));
				await dialog.Msg(L("With the world in the state that it is there are some that are in ruin or have become habitats for monsters... In some cases, monsters are even indwelling in the statues."));
				await dialog.Msg(L("Of course, it's up to you if you believe it or not."));
				return;
			}

			if (character.Quests.IsActive(Sq07))
			{
				await dialog.Msg(L("I've felt a lot of resentment while abandoning my hometown. But coming here and seeing the northern villages changed my mind about that."));
				await dialog.Msg(L("I just hope that the village will be safe until the migration orders are lifted. You see, the elders are still there because they couldn't bear leaving their hometown."));
				return;
			}

			if (character.Quests.IsActive(Sq08))
			{
				await dialog.Msg(L("I don't know if they have actually thought it through. It shouldn't be that dangerous if they prepared it for us."));
				return;
			}

			if (character.Quests.HasCompleted(Sq06))
			{
				await dialog.Msg(L("Some say that the goddesses simply abandoned us... I am sure that there must be a reason."));
				await dialog.Msg(L("I still have faith because the Goddess Statues shine as if they are acknowledging your prayers. That is what is upholding my faith."));
				return;
			}

			await dialog.Msg(L("I'm getting nervous since nobody is in sight when they should be here by now. You don't think they've all entered Orsha without me, do you?"));
		});

		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(40110, L("Statue of Goddess Zemyna"), "SIAU16_SQ_06_EV_NPC", "f_siauliai_16", 13.89, 1277.25, 60, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Statue of Goddess Zemyna"));

			var worshipResult = await WorshipStatPointStatue(dialog, "SIAU16_SQ_06_EV_NPC");
			if (worshipResult == null)
				return;

			if (character.Quests.IsActive(Sq06))
			{
				if (worshipResult == false)
				{
					var worshipped = await dialog.TimeAction(ScpArgMsg("Auto_KyeongBae_Jung"), "WORSHIP", TimeSpan.FromSeconds(2));
					if (worshipped != TimeActionResult.Completed)
						return;
				}

				character.Quests.CompleteObjective(Sq06, "worship");
				await dialog.Msg(L("The statue shines softly as you finish your prayer. The goddess may be silent, but the stone still answers."));
				await dialog.CompleteQuest(Sq06);
			}
		});

		// The bonfire wood on Adata Highway
		//-------------------------------------------------------------------------
		for (var i = 0; i < Bonfires.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(154060, L("Firewood"), "SIAU16_SQ_03_NPC_" + number, "f_siauliai_16", Bonfires[i, 0], Bonfires[i, 1], 90,
				character => character.Quests.IsActive(Sq03) && !character.Quests.IsCompletable(Sq03) && !character.Variables.Perm.GetBool(BonfireVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq03) || character.Quests.IsCompletable(Sq03) || character.Variables.Perm.GetBool(BonfireVar + number, false))
						return;

					character.Variables.Perm.Set(BonfireVar + number, true);
					var lit = character.Variables.Perm.GetInt(BonfireCountVar, 0) + 1;
					character.Variables.Perm.SetInt(BonfireCountVar, lit);

					dialog.Npc.PlayEffect("F_burstup001_fire", 1f);
					character.ServerMessage(LF("Bonfires lit: {0}/{1}", Math.Min(lit, 6), 6));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Firewood on Adata Highway
		//-------------------------------------------------------------------------
		AddNpc(47223, L("Firewood"), "SIAU16_SQ_04_NPC", "f_siauliai_16", -482, 1531, -58, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Firewood"));

			if (character.Quests.IsActive(Sq04) && !character.Quests.IsCompletable(Sq04))
			{
				var lit = await character.TimeActions.StartAsync(L("Lighting the fire..."), L("Cancel"), "FIRE", TimeSpan.FromSeconds(2));
				if (lit != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Sq04);
				return;
			}

			await dialog.Msg(L("A stack of firewood Officer Lutas prepared for the refugee camps."));
		});

		// Leftover goods at the Lumberjack Cabin
		//-------------------------------------------------------------------------
		for (var i = 0; i < Goods.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc((int)Goods[i, 0], L("Leftover Goods"), "SIAU16_SQ_05_NPC_" + number, "f_siauliai_16", Goods[i, 1], Goods[i, 2], 90,
				character => character.Quests.IsActive(Sq05) && !character.Quests.IsCompletable(Sq05) && !character.Variables.Perm.GetBool(GoodsVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq05) || character.Quests.IsCompletable(Sq05) || character.Variables.Perm.GetBool(GoodsVar + number, false))
						return;

					character.Variables.Perm.Set(GoodsVar + number, true);
					character.Inventory.Add(ItemId.SIAU16_SQ_05_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The settlers hiding around Randoluma Rest Place
		//-------------------------------------------------------------------------
		for (var i = 0; i < Settlers.GetLength(0); ++i)
		{
			var number = i + 1;
			var type = 1 + i / 3;

			AddConditionalNpc((int)Settlers[i, 0], L("Scared Settler"), "SIAU16_SQ_08_NPC_" + type + "_" + number, "f_siauliai_16", Settlers[i, 1], Settlers[i, 2], Settlers[i, 3],
				character => character.Quests.IsActive(Sq08) && !character.Quests.IsCompletable(Sq08) && !character.Variables.Perm.GetBool(SettlerVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					dialog.SetTitle(L("Scared Settler"));

					if (!character.Quests.IsActive(Sq08) || character.Quests.IsCompletable(Sq08) || character.Variables.Perm.GetBool(SettlerVar + number, false))
						return;

					await dialog.Msg(L("Dallanas sent you? Thank the goddesses... I'll make my way to the camp right away."));

					character.Variables.Perm.Set(SettlerVar + number, true);
					var found = character.Variables.Perm.GetInt(SettlerCountVar, 0) + 1;
					character.Variables.Perm.SetInt(SettlerCountVar, found);

					character.ServerMessage(LF("Settlers found: {0}/{1}", Math.Min(found, 3), 3));
					character.LookAround();
				});
		}
	}

	/// <summary>
	/// Returns whether the settlers are still queuing at the Migration Office.
	/// </summary>
	private static bool IsQueueWaiting(Character character)
		=> !character.Quests.Has(Mq06) && !character.Quests.HasCompleted(Mq06);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60070: The Journey Begins (1)
//-----------------------------------------------------------------------------
public class Siau16Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60070);
		SetName(L("The Journey Begins (1)"));
		SetDescription(L("What a horrific dream. But it's time to head to Orsha. Talk to Settler Bowein by the Lemprasa Pond."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_BOWEIN", "f_siauliai_16", L("Talk with Settler Browein"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_BOWEIN", "f_siauliai_16", L("Talk with Settler Browein"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_BOWEIN", "f_siauliai_16", L("Talk with Settler Browein"));

		AddObjective("talk", L("Talk with Settler Browein"), new ManualObjective());
	}
}

// 60071: The Journey Begins (2)
//-----------------------------------------------------------------------------
public class Siau16Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60071);
		SetName(L("The Journey Begins (2)"));
		SetDescription(L("Press the M key to open the map. Confirm Settler Brophen's location, then go and talk to him."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_BROPHEN", "f_siauliai_16", L("Talk with Settler Brophen"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_BROPHEN", "f_siauliai_16", L("Defeat the swarm of Kepas"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_BROPHEN", "f_siauliai_16", L("Talk with Settler Brophen"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU16_MQ_02_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(60070, QuestStatus.Completed));

		AddObjective("killKepa", L("Defeat the swarm of Kepas"), new KillObjective(5, "Sec_Onion") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 10));
	}
}

// 60072: The Journey Begins (3)
//-----------------------------------------------------------------------------
public class Siau16Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60072);
		SetName(L("The Journey Begins (3)"));
		SetDescription(L("Brophen says you can use Status Points by leveling up to become even stronger. Press the F1 key and distribute your new Status Point."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_BROPHEN", "f_siauliai_16", L("Talk with Settler Brophen"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_BROPHEN", "f_siauliai_16", L("Use a Status Point"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_BROPHEN", "f_siauliai_16", L("Talk with Settler Brophen"));

		AddPrerequisite(new QuestStatusPrerequisite(60071, QuestStatus.Completed));

		AddObjective("spendStat", L("Use a Status Point"), new VariableCheckObjective(NormalTxFunctionsScript.StatPointsSpentVarName, 1, isPermanent: true));
	}
}

// 60073: The Journey Begins (4)
//-----------------------------------------------------------------------------
public class Siau16Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60073);
		SetName(L("The Journey Begins (4)"));
		SetDescription(L("Settler Brophen says Settler Layla might know where the mayor is. He also asked you to deliver some grass leaf ointment to her."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_BROPHEN", "f_siauliai_16", L("Talk with Settler Brophen"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_RHEILAR", "f_siauliai_16", L("Deliver Grass Leaf Ointment to Settler Layla"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_RHEILAR", "f_siauliai_16", L("Deliver Grass Leaf Ointment to Settler Layla"));

		AddPrerequisite(new QuestStatusPrerequisite(60072, QuestStatus.Completed));

		AddObjective("deliver", L("Deliver Grass Leaf Ointment to Settler Layla"), new ManualObjective());

		AddReward(new TakeItemReward("SIAU16_MQ_04_ITEM", -1));
	}
}

// 60074: Orsha (1)
//-----------------------------------------------------------------------------
public class Siau16Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60074);
		SetName(L("Orsha (1)"));
		SetDescription(L("Mayor Romanas believes in your talent and will make sure you're admitted into Orsha immediately. Officer Lutas is looking for you at the Orsha Migration Office."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_ROMANAS", "f_siauliai_16", L("Talk with Mayor Romanas"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_LUTAS", "f_siauliai_16", L("Talk with Officer Lutas"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_LUTAS", "f_siauliai_16", L("Talk with Officer Lutas"));

		AddPrerequisite(new QuestStatusPrerequisite(60073, QuestStatus.Completed));

		AddObjective("meetLutas", L("Talk with Officer Lutas"), new ManualObjective());
	}
}

// 60075: Orsha (2)
//-----------------------------------------------------------------------------
public class Siau16Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60075);
		SetName(L("Orsha (2)"));
		SetDescription(L("A Poata suddenly appeared at the entrance to Orsha and it's threatening the settlers! Defeat the Poata and make the entrance to Orsha safe again."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_LUTAS", "f_siauliai_16", L("Talk with Officer Lutas"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_LUTAS", "f_siauliai_16", L("Defeat Poata"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_LUTAS", "f_siauliai_16", L("Talk with Officer Lutas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU16_MQ_06_TRACK", "m_boss_a", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60074, QuestStatus.Completed));

		AddObjective("killPoata", L("Defeat the Poata attacking Orsha's Migration Office"), new KillObjective(1, "boss_poata_Q4") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Vis", 25));
		AddReward(new SelectItemReward("SWD01_113", "STF01_113", "TBW01_113", "MAC01_113"));
	}
}

// 60076: Orsha (3)
//-----------------------------------------------------------------------------
public class Siau16Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60076);
		SetName(L("Orsha (3)"));
		SetDescription(L("Inesa Hamondale, the lord of Orsha, is looking for someone with skills. To find Urbonas, the bishop of Orsha, talk to Inesa Hamondale at the Central Plaza first."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_16", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_LUTAS", "f_siauliai_16", L("Talk with Officer Lutas"));
		SetPhase(QuestStatus.InProgress, "C_ORSHA_HAMONDAIL", "c_orsha", L("Visit Orsha"));
		SetPhase(QuestStatus.Success, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk to Inesa Hamondale, the lord of Orsha"));

		AddPrerequisite(new QuestStatusPrerequisite(60075, QuestStatus.Completed));

		AddObjective("visitOrsha", L("Visit Orsha"), new ManualObjective());
	}
}

// 60077: A Thankful Heart
//-----------------------------------------------------------------------------
public class Siau16Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60077);
		SetName(L("A Thankful Heart"));
		SetDescription(L("Settler Izna wants to make some medicine for you as a token of appreciation. Defeat some Leaf Bugs around Ziedo Pond to obtain Red Leaves for the medicine."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_IZNA", "f_siauliai_16", L("Talk with Settler Izna"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_IZNA", "f_siauliai_16", L("Defeat Leaf Bugs to obtain Red Leaves"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_IZNA", "f_siauliai_16", L("Deliver the Red Leaves to Settler Izna"));

		AddObjective("collectLeaves", L("Defeat Leaf Bugs to obtain Red Leaves"), new CollectItemObjective("SIAU16_SQ_01_ITEM", 4));
		AddPityDrop("SIAU16_SQ_01_ITEM", 1.0f, 0, 1, "Sec_Leaf_diving");

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 20));
		AddReward(new ItemReward("Drug_HP1_Q", 3));
		AddReward(new TakeItemReward("SIAU16_SQ_01_ITEM", -1));
	}
}

// 60078: Something to Give
//-----------------------------------------------------------------------------
public class Siau16Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60078);
		SetName(L("Something to Give"));
		SetDescription(L("Settler Izna wants to make Stamina Pills for the new settlers arriving to Orsha. Defeat Chinencys around Ziedo Pond and collect the Rampar Mucus used to make Stamina pills."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_IZNA", "f_siauliai_16", L("Talk with Settler Izna"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_IZNA", "f_siauliai_16", L("Defeat Chinencys to obtain Rampar Mucus"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_IZNA", "f_siauliai_16", L("Deliver the Rampar Mucus to Settler Izna"));

		AddObjective("collectMucus", L("Defeat Chinencys to obtain Rampar Mucus"), new CollectItemObjective("SIAU16_SQ_02_ITEM", 5));
		AddPityDrop("SIAU16_SQ_02_ITEM", 1.0f, 0, 1, "Sec_Bokchoy");

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 20));
		AddReward(new ItemReward("Drug_STA1_Q", 3));
		AddReward(new TakeItemReward("SIAU16_SQ_02_ITEM", -1));
	}
}

// 60079: Lost in the Forest (1)
//-----------------------------------------------------------------------------
public class Siau16Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60079);
		SetName(L("Lost in the Forest (1)"));
		SetDescription(L("Settler Izna believes some village residents have gotten lost on their way to Orsha. Go to Adata Highway and make a bonfire so they can spot the smoke and find their way."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_IZNA", "f_siauliai_16", L("Talk with Settler Izna"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_IZNA", "f_siauliai_16", L("Make a bonfire on Adata Highway"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_IZNA", "f_siauliai_16", L("Report to Settler Izna"));

		AddObjective("lightBonfires", L("Make a bonfire on Adata Highway"), new VariableCheckObjective(FSiauliai16QuestNpcsScript.BonfireCountVar, 6, isPermanent: true));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 20));
	}
}

// 60080: Lost in the Forest (2)
//-----------------------------------------------------------------------------
public class Siau16Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60080);
		SetName(L("Lost in the Forest (2)"));
		SetDescription(L("Settler Izna thinks the bonfire may be hard to spot by the residents. Go to Adata Highway and set fire to the firewood there."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_IZNA", "f_siauliai_16", L("Talk with Settler Izna"));
		SetPhase(QuestStatus.InProgress, "SIAU16_SQ_04_NPC", "f_siauliai_16", L("Set fire to the firewood on Adata Highway"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_IZNA", "f_siauliai_16", L("Talk with Settler Izna"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU16_SQ_04_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60079, QuestStatus.Completed));

		AddObjective("killWoodspirit", L("Defeat Woodspirit"), new KillObjective(1, "boss_woodspirit_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Vis", 25));
	}
}

// 60081: An Unowned Object
//-----------------------------------------------------------------------------
public class Siau16Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60081);
		SetName(L("An Unowned Object"));
		SetDescription(L("Settler Ivanayus is in desperate need of supplies and willing to settle for used ones. Go to the Lumberjack Cabin and collect useful objects."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_IHBANAYUS", "f_siauliai_16", L("Talk with Settler Ivanayus"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_IHBANAYUS", "f_siauliai_16", L("Collect Useful Object"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_IHBANAYUS", "f_siauliai_16", L("Deliver the useful objects to Settler Ivanayus"));

		AddObjective("collectObjects", L("Collect Useful Object"), new CollectItemObjective("SIAU16_SQ_05_ITEM", 5));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 20));
		AddReward(new TakeItemReward("SIAU16_SQ_05_ITEM", -1));
	}
}

// 60082: Statue of Goddess Zemyna
//-----------------------------------------------------------------------------
public class Siau16Sq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60082);
		SetName(L("Statue of Goddess Zemyna"));
		SetDescription(L("Settler Dallanas says the goddesses bless those who worship their statues. Worship the Statue of Goddess Zemyna at the Randoluma Rest Place and receive Her blessings."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_DALLANAS", "f_siauliai_16", L("Talk with Settler Dallanas"));
		SetPhase(QuestStatus.InProgress, "SIAU16_SQ_06_EV_NPC", "f_siauliai_16", L("Worship the Statue of Goddess Zemyna at the Randoluma Rest Place"));
		SetPhase(QuestStatus.Success, "SIAU16_SQ_06_EV_NPC", "f_siauliai_16", L("Worship the Statue of Goddess Zemyna at the Randoluma Rest Place"));

		AddObjective("worship", L("Worship the Statue of Goddess Zemyna at the Randoluma Rest Place"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 20));
	}
}

// 60083: Unsafe Safety Zone
//-----------------------------------------------------------------------------
public class Siau16Sq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60083);
		SetName(L("Unsafe Safety Zone"));
		SetDescription(L("Settler Dallanas was told to wait at the Randoluma Rest Place, but staying there proved impossible due to the amount of monsters. Go to the Randoluma Rest Place and defeat the monsters there."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_DALLANAS", "f_siauliai_16", L("Talk with Settler Dallanas"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_DALLANAS", "f_siauliai_16", L("Defeat Monsters at the Randoluma Rest Place"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_DALLANAS", "f_siauliai_16", L("Talk with Settler Dallanas"));

		AddObjective("killMonsters", L("Defeat Monsters at the Randoluma Rest Place"), new KillObjective(15, "Sec_Onion", "Sec_Leaf_diving", "Sec_chupaluka", "Sec_Bokchoy", "Sec_Weaver", "pappus_kepa_beige"));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 20));
	}
}

// 60084: The Settler Without Rest
//-----------------------------------------------------------------------------
public class Siau16Sq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60084);
		SetName(L("The Settler Without Rest"));
		SetDescription(L("Settler Dallanas says her people arrived at the Randoluma Rest Place but had to scatter because of the monsters. Go to Randoluma Rest Place and find the villagers hiding there."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_16");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI16_DALLANAS", "f_siauliai_16", L("Talk with Settler Dallanas"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI16_DALLANAS", "f_siauliai_16", L("Randoluma Rest Place"));
		SetPhase(QuestStatus.Success, "SIAULIAI16_DALLANAS", "f_siauliai_16", L("Talk with Settler Dallanas"));

		AddObjective("findSettlers", L("Bring back the settlers hiding at the Randoluma Rest Place"), new VariableCheckObjective(FSiauliai16QuestNpcsScript.SettlerCountVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Vis", 20));
	}
}
