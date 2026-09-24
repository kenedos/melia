//--- Melia Script ----------------------------------------------------------
// Bellai Rainforest Quest NPCs
//--- Description -----------------------------------------------------------
// Druid Leja and the wary villagers of the Bellai Forest Workshop, the
// ferrets turned by a demon totem, and a mysterious girl wrapped in light.
//---------------------------------------------------------------------------

using System;
using System.Linq;
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

public class FOrchard323QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Hq1 = new QuestId(50275);
	private readonly static QuestId Rp1 = new QuestId(60184);
	private readonly static QuestId Mq01 = new QuestId(80018);
	private readonly static QuestId Mq02 = new QuestId(80019);
	private readonly static QuestId Mq03 = new QuestId(80020);
	private readonly static QuestId Mq04 = new QuestId(80021);
	private readonly static QuestId Mq05 = new QuestId(80022);
	private readonly static QuestId Mq06 = new QuestId(80023);
	private readonly static QuestId Sq01 = new QuestId(80024);
	private readonly static QuestId Sq02 = new QuestId(80025);
	private readonly static QuestId Sq03 = new QuestId(80026);
	private readonly static QuestId Sq04 = new QuestId(80027);
	private readonly static QuestId Sq05 = new QuestId(80028);
	private readonly static QuestId Seir324Mq01 = new QuestId(80041);
	private readonly static QuestId Seir324Mq07 = new QuestId(80047);

	private const string HerbVar = "Gabija.Quests.Orchard323Mq01.Herb";
	private const int HerbsNeeded = 6;
	private const int StatueRange = 600;
	private const int JerkyRange = 100;

	private static readonly TimeSpan GatherRespawn = TimeSpan.FromSeconds(30);

	private static readonly Position JerkySpot = new Position(630.08f, 0.87f, -511.33f);
	private static readonly Position FerretDen = new Position(1018f, 0f, -834f);

	private static readonly double[,] Herbs =
	{
		{ -1137.75, -298.95 }, { -1072.01, -192.18 }, { -1110.70, -123.32 }, { -1149.60, -34.72 }, { -1277.51, -295.71 }, { -1382.70, -375.50 },
		{ -1155.27, -231.75 }, { -1411.44, -115.57 }, { -1458.64, -185.81 }, { -1408.37, -228.31 }, { -1343.94, -282.64 }, { -1454.33, -315.41 },
	};

	private static readonly double[,] DecoyBaskets =
	{
		{ 250.63, -96.02 }, { 366.75, -138.39 }, { 581.47, -211.32 }, { 615.23, -593.37 },
	};

	protected override void Load()
	{
		// Druid Leja
		//-------------------------------------------------------------------------
		AddConditionalNpc(156003, L("Druid Leja"), "ORCHARD323_LEJA", "f_orchard_32_3", -1282.72, -977.03, 45, c => !c.Quests.Has(Mq03), this.Leja);

		// The villagers of the Bellai Forest Workshop
		//-------------------------------------------------------------------------
		AddNpc(152002, L("Village Headman"), "ORCHARD323_MAYOR", "f_orchard_32_3", -268.78, 853.90, 90, this.Headman);
		AddNpc(147484, L("Mayor's Grandson"), "ORCHARD323_GRANDSON", "f_orchard_32_3", -220.03, 876.36, 0, this.Grandson);
		AddNpc(147473, L("Village Resident"), "ORCHARD323_PEOPLE", "f_orchard_32_3", -777.23, 1067.73, 90, this.Resident);
		AddNpc(156005, L("Dievdirbys Widas"), "ORCHARD323_VYDAS", "f_orchard_32_3", -780.48, 931.22, 0, this.Widas);
		AddConditionalNpc(147407, L("Village Priest"), "ORCHARD323_PRIEST", "f_orchard_32_3", -536.68, 919.12, 90, c => !c.Quests.Has(Seir324Mq07), this.Priest);
		AddConditionalNpc(147408, L("Great Priest of the Village"), "ORCHARD323_FUZE", "f_orchard_32_3", -563.56, 1116.29, 180, c => !c.Quests.Has(Seir324Mq07), this.GreatPriest);
		AddNpc(147409, L("Priest Rovli"), "ORCHARD323_RP_1_NPC", "f_orchard_32_3", 569.12, 239.52, 90, this.Rovli);

		// Hemostasis Herbs at the Luvda Cliff
		//-------------------------------------------------------------------------
		for (var i = 0; i < Herbs.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "ORCHARD323_HERB" : "ORCHARD323_HERB_" + number;

			AddConditionalNpc(153054, L("Hemostasis Herb"), uniqueName, "f_orchard_32_3", Herbs[i, 0], Herbs[i, 1], 90,
				c => c.Quests.IsActive(Mq01) && !c.Quests.IsCompletable(Mq01),
				async dialog =>
				{
					var character = dialog.Player;
					if (!character.Quests.IsActive(Mq01) || character.Inventory.CountItem(ItemId.ORCHARD_323_MQ_01_ITEM) >= HerbsNeeded)
						return;

					if (Gather(character, HerbVar + number, L("The herbs here have already been picked.")))
						character.Inventory.Add(ItemId.ORCHARD_323_MQ_01_ITEM, 1, InventoryAddType.PickUp);

					await Task.CompletedTask;
				});
		}

		// Where the girl crosses the Revelator's path
		//-------------------------------------------------------------------------
		AddQuestTrigger("ORCHARD323_MQ_02_TRIG", "f_orchard_32_3", -964.15, -310.01, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
				character.Quests.Start(Mq02);

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
				character.Quests.StartQuestTrack(Mq02);

			await Task.CompletedTask;
		});

		AddQuestTrigger("ORCHARD323_MQ_04_TRIG", "f_orchard_32_3", -512.16, 162.56, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
				character.Quests.Start(Mq04);

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
				character.Quests.StartQuestTrack(Mq04);

			await Task.CompletedTask;
		});

		AddQuestTrigger("ORCHARD323_MQ_06_TRIG", "f_orchard_32_3", 104.65, 1467.81, 80, async args =>
		{
			if (args.Initiator is not Character character || !character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			character.Quests.CompleteObjective(Mq06, "followGirl");

			await Task.CompletedTask;
		});

		// The demon totem on the Banaga Forest Trail
		//-------------------------------------------------------------------------
		AddConditionalNpc(47150, L("Demon Totem"), "ORCHARD323_EVIL", "f_orchard_32_3", 391.81, -251.69, 90, c => !c.Quests.HasCompleted(Mq05) && !c.Quests.IsCompletable(Mq05), async dialog =>
		{
			var character = dialog.Player;
			if (!character.Quests.IsActive(Mq05) || character.Quests.IsCompletable(Mq05))
				return;

			var looked = await character.TimeActions.StartAsync(L("Looking around"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));
			if (looked != TimeActionResult.Completed)
				return;

			character.Quests.ReplayQuestTrack(Mq05);
		});

		// The old well south of the workshop
		//-------------------------------------------------------------------------
		AddNpc(155009, L("Old Well"), "ORCHARD323_WELL", "f_orchard_32_3", 179.17, 620.71, 90, async dialog =>
		{
			var character = dialog.Player;
			if (!character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02))
				return;

			var scooped = await character.TimeActions.StartAsync(L("Scooping up the red water"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));
			if (scooped != TimeActionResult.Completed)
				return;

			if (character.Inventory.CountItem(ItemId.ORCHARD_323_SQ_WATER) == 0)
				character.Inventory.Add(ItemId.ORCHARD_323_SQ_WATER, 1, InventoryAddType.PickUp);
		});

		// The baskets of stolen food on the Banaga Forest Trail
		//-------------------------------------------------------------------------
		AddConditionalNpc(153041, L("Suspicious Basket"), "ORCHARD323_FOOD", "f_orchard_32_3", JerkySpot.X, JerkySpot.Z, 90, c => c.Quests.IsActive(Sq03) && !c.Quests.IsCompletable(Sq03), async dialog =>
		{
			ContaminateJerky(dialog.Player);
			await Task.CompletedTask;
		});

		for (var i = 0; i < DecoyBaskets.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "ORCHARD323_FOOD_F" : "ORCHARD323_FOOD_F_" + number;

			AddConditionalNpc(153041, L("Suspicious Basket"), uniqueName, "f_orchard_32_3", DecoyBaskets[i, 0], DecoyBaskets[i, 1], 90,
				c => c.Quests.IsActive(Sq03) && !c.Quests.IsCompletable(Sq03),
				async dialog =>
				{
					switch (number % 3)
					{
						case 1: dialog.Player.ServerMessage(L("Beef Jerky cannot be found here.")); break;
						case 2: dialog.Player.ServerMessage(L("Only fruits can be seen from this basket.")); break;
						default: dialog.Player.ServerMessage(L("This must be the wrong basket.")); break;
					}

					await Task.CompletedTask;
				});
		}

		// The offering tool the ferrets dropped
		//-------------------------------------------------------------------------
		AddConditionalNpc(20025, L("Ritual Device"), "ORCHARD323_HIDDEN_OBJ2", "f_orchard_32_3", -1552.11, 915.30, 90, IsRitualDeviceLying, async dialog =>
		{
			var character = dialog.Player;
			if (!IsRitualDeviceLying(character))
				return;

			character.Inventory.Add(ItemId.ORCHARD323_HIDDENQ1_ITEM, 1, InventoryAddType.PickUp);
			character.LookAround();

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Druid Leja's dialog at the foot of the Luvda Cliff.
	/// </summary>
	private async Task Leja(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Druid Leja"));

		if (character.Quests.IsCompletable(Mq02))
		{
			await dialog.Msg(L("Thank you so much for this favor."));
			await dialog.Msg(L("I'm going to get some rest and then go back to the workshop where the other people are."));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
		{
			await dialog.Msg(L("I don't think... I've seen you before. Hm... That's good."));
			await dialog.Msg(L("If you can, will you please help me? I came running after the ferrets who stole our food and ended up here."));
			await dialog.Msg(L("But the ferrets attacked me and I couldn't get the food. I need some first aid for my wounds..."));

			var answer = await dialog.SelectQuestOffer(Mq01, L("There's a herb at the Luvda Cliff that helps stop bleeding. Will you get some for me?"),
				Option(L("I'll go; what do the herbs look like?"), "accept"),
				Option(L("I'm busy"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq01);
				character.LookAround();

				await dialog.Msg(L("It's darker than the grass around it and has wider leaves. If you go to Luvda Cliff you'll recognize it right away."));
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("You're telling me you saw a girl being chased by a demon with an axe? That demon is probably Zaura; the girl I don't know."));
			await dialog.Msg(L("And he wants to stop the girl from meeting the Revelator...? That sounds suspicious."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("I need to tell the village about this."),
				Option(L("I am the Revelator"), "accept"),
				Option(L("Stay quiet"), "leave")
			);

			if (answer != "accept")
				return;

			var talked = await character.TimeActions.StartAsync(L("Talking about the mysterious girl"), L("Cancel"), "TALK", TimeSpan.FromSeconds(1));
			if (talked != TimeActionResult.Completed)
				return;

			character.Quests.Start(Mq03);

			await dialog.Msg(L("What? You're a Revelator? Did you come here after dreaming about a calamity?"));
			await dialog.Msg(L("I wonder if...? I think you might just be the Revelator told in the story of our village."));
			await dialog.Msg(L("The story says... that when chaos takes over our village, a Revelator will dream of a calamity and come."));
			await dialog.Msg(L("They and their divine light will chase away evil and save the village. Those are the words of Goddess Laima that have been passed down."));
			await dialog.Msg(L("But... um... This isn't the right time. First I'm going to ask our village chief about this story."));
			await dialog.Msg(L("Meanwhile, please take back our food from the ferrets who stole it. You can bring it to our village chief at the Bellai Forest Workshop."));
			await dialog.Msg(L("Our people don't really trust outsiders but... We can't turn our back on help."));

			character.LookAround();
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("The ferrets might attack again. These days it's like they think everything in the forest belongs to them."));
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			character.Quests.ClearQuestTrack(Mq02);
			await dialog.Msg(L("Ow, ouch... Those darned ferrets have some sharp nails..."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("Ow, ouch... Those darned ferrets have some sharp nails..."));
		else
			await dialog.Msg(L("Our village doesn't usually welcome outsiders... It's best if you don't expect much kindness from people here."));
	}

	/// <summary>
	/// The Village Headman's dialog at the Bellai Forest Workshop.
	/// </summary>
	private async Task Headman(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Village Headman"));

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("Are you the Revelator Leja talked about? And that's... our food that got stolen."));
			await dialog.Msg(L("Thank you for taking back our food but... I don't think I can trust you just yet."));
			await dialog.Msg(L("Please understand. We were tricked by the demons once before, you see."));
			await dialog.CompleteQuest(Mq04);
			return;
		}

		if (character.Quests.IsCompletable(Mq05))
		{
			await dialog.Msg(L("You... you're a true Revelator, there's no doubt about it. I feel so sorry for how I treated you earlier... Please forgive this old man."));
			await dialog.Msg(L("Leja went to Zeraha to try and find something to help you. She said Goddess Laima will have left something prepared for the Revelator."));
			await dialog.Msg(L("There is a big statue of Goddess Laima there. It was sculpted a long time ago when She was still at our village."));
			await dialog.Msg(L("Oh... that girl came. Can I tell you about what happened in our village?"));
			await dialog.Msg(L("Our village has always worshiped Goddess Laima. One day, she had a vision of a calamity that would strike the kingdom in a future far away."));
			await dialog.Msg(L("So, to stop this calamity, She decided to leave the village. Maybe She was worried about leaving us behind, so she had Goddess Lada look after us instead."));
			await dialog.Msg(L("But unlike what Goddess Laima predicted, our village remained peaceful. Then, unexpectedly, Medzio Diena happened."));
			await dialog.Msg(L("It was the calamity in Goddess Laima's vision. Medzio Diena didn't cause much damage here, but that was only the beginning."));
			await dialog.Msg(L("You see, not long ago our river turned red. The river water caused terrible pain to everyone who drank it, and it made the fruit grow to an abnormal size."));
			await dialog.Msg(L("The fruit, just like the red water, was inedible. As if that weren't enough, the demons then came and took Goddess Lada."));
			await dialog.Msg(L("Just as Laima predicted... chaos took over the village. Leja probably told you... what Goddess Laima said to us."));
			await dialog.Msg(L("She said a Revelator would dream of the calamity and come with the light to chase away evil."));
			await dialog.CompleteQuest(Mq05);
			return;
		}

		if (character.Quests.IsCompletable(Hq1))
		{
			await dialog.Msg(L("They'll steal anything, those lousy ferrets. Thanks for bringing back our offering tools."));
			await dialog.Msg(L("We can finally do a proper offering ceremony."));
			await dialog.CompleteQuest(Hq1);
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("Leja said she had somewhere else to be and left. I can't tell you where she is going, though."));
			await dialog.Msg(L("Like I said earlier, we can't trust you just yet. We once had someone come to our village calling himself a Revelator."));
			await dialog.Msg(L("He was a man with a pale face and long hair. We thought our Revelator had finally arrived and did as the man said."));
			await dialog.Msg(L("Truth is, in reality... he was a demon. We believed the demon and because of that Goddess Lada was taken away..."));
			await dialog.Msg(L("Even the ferrets turned violent. We think it was because of the totem the demon made us set up."));
			await dialog.Msg(L("We all want to join forces and rescue Goddess Lada, but we're worried about the ferret attacks."));
			await dialog.Msg(L("There is a totem that we don't even dare get close to because of its evil energy. If you really are a Revelator, then you should be able to destroy it with your divine power."));

			var answer = await dialog.SelectQuestOffer(Mq05, L("Show us. If you really are a Revelator, that is..."),
				Option(L("I'll destroy the totems"), "accept"),
				Option(L("I'll refuse"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq05);

				await dialog.Msg(L("The totem is at the Banaga Forest Trail."));
				await dialog.Msg(L("If you really are a Revelator that came here with the light to save our town... Then you should be able to take back the ferrets' hideout."));
				await dialog.Msg(L("If you can prove that you are in fact a Revelator, I will tell you where to find Leja."));
			}
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			await dialog.Msg(L("We aren't sure what She meant by the light, but... Our elders used to say that a warm energy filled the village when Goddess Laima was here."));
			await dialog.Msg(L("It was all very quick. The moment that girl came to me, I felt the same warm energy our elders talked about."));
			await dialog.Msg(L("The girl looked me straight in the eyes and, without a word, she nodded. Then she disappeared towards Zeraha."));
			await dialog.Msg(L("Hurry and follow the light. This can only be a sign sent by Goddess Laima to guide you in the right direction."));

			var answer = await dialog.SelectQuestOffer(Mq06, L("Please, for Goddess Lada... and our village."),
				Option(L("I will follow it"), "accept"),
				Option(L("Why can't you ask for help around here?"), "explain"),
				Option(L("Tell him that you need some time to think"), "leave")
			);

			if (answer == "explain")
			{
				await dialog.Msg(L("Help...? It's not something I haven't thought about. The city closest to here is Fedimian."));
				await dialog.Msg(L("I heard Fedimian was devastated after Medzio Diena, though. Even after four years, it's still being reconstructed."));
				await dialog.Msg(L("And Goddess Lada, She said that no matter what happens to Her... She would never make Her presence known to anyone but the Revelators."));
				await dialog.Msg(L("As you know... People now think that the goddesses have all disappeared."));
				await dialog.Msg(L("But if they find out that the goddesses are still around... You can imagine what a shock that would be."));
				await dialog.Msg(L("That's why only you, a Revelator, can save Goddess Lada. The goddess said so Herself, too..."));
				return;
			}

			if (answer == "accept")
			{
				character.Quests.Start(Mq06);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The mysterious girl has run off to Zeraha without a word. Let's follow her."), 5);
			}
			return;
		}

		if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
		{
			await dialog.Msg(L("Those are our offering tools. Are you saying the ferrets had them? First the ferrets steal our food, now our offering tools too...?"));
			await dialog.Msg(L("You're saying the ferrets were keeping these tools? We were looking for them all over the place."));
			await dialog.Msg(L("Um, I dunno if it's too much to ask, but would you find our other offering tools? We need them to pray for peace in our village."));

			var answer = await dialog.SelectQuestOffer(Hq1, L("You could say those tools are what's keeping our village safe all these years, so you know."),
				Option(L("I'll get back the offering tools."), "accept"),
				Option(L("Do it yourself."), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Hq1);
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			character.Quests.ClearQuestTrack(Mq04);
			await dialog.Msg(L("Your face doesn't look familiar... Please don't cause a commotion and just move along."));
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("Please understand. I know Leja is glad the Revelator is finally here but..."));
			await dialog.Msg(L("We've been fooled before, we can't help but be suspicious."));
			return;
		}

		if (character.Quests.IsActive(Mq06))
		{
			await dialog.Msg(L("That warm energy... Until the day I return to the goddess I will never be able to forget it."));
			await dialog.Msg(L("Go now. Follow the light and you will see the girl once again."));
			return;
		}

		if (character.Quests.IsActive(Hq1))
		{
			await dialog.Msg(L("The ferrets already had their hands in our food, and now the village's offering tools..."));
			return;
		}

		if (!character.Quests.HasCompleted(Mq05))
		{
			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("Your face doesn't look familiar... Please don't cause a commotion and just move along."));
			else
				await dialog.Msg(L("That red water is no one's business but our village's. Outsiders would do well to leave us alone."));
			return;
		}

		if (!character.Quests.HasCompleted(Seir324Mq01))
		{
			await dialog.Msg(L("I'm sorry I doubted you. It's just that our town has been hurt by fake Revelators before..."));
			await dialog.Msg(L("Goddess Lada being taken by the demons... None of that would have happened if we hadn't believed the fake Revelators."));
			await dialog.Msg(L("And with our water turning red we felt more and more powerless... We should have realized our mistake and gone to the real Revelators, but... I'm so sorry."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("Thank you. I'm so grateful to you and to Goddess Lada for having sent you. You are a blessing to our village."));
		else
			await dialog.Msg(L("May the Revelator who dreamt of chaos defeat the evil forces and bring light back into our world... If the legends are true, this red water will soon be no more."));
	}

	/// <summary>
	/// The Mayor's Grandson's dialog.
	/// </summary>
	private async Task Grandson(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mayor's Grandson"));

		if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
		{
			await dialog.Msg(L("Are you the Revelator who destroyed the totem? I wanted to thank you. It made me feel terrible every time I looked at it..."));
			await dialog.Msg(L("Anyway, I went to check in on the ferrets and they're still vicious. It looks like the effects of the totem haven't faded yet."));
			await dialog.Msg(L("Widas, a man from our village, said he was planning to do something... He is a talented Dievdirbys after all."));

			var answer = await dialog.SelectQuestOffer(Sq04, L("Will you ask Widas if the statue is finished?"),
				Option(L("I will find out more about it"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq04);
				character.Quests.CompleteObjective(Sq04, "askWidas");
			}
			return;
		}

		if (character.Quests.IsActive(Sq04))
		{
			await dialog.Msg(L("When I was little the ferrets would help us get back to the village whenever we got lost."));
			await dialog.Msg(L("But now we've been fooled by the demons... It's a shame."));
			return;
		}

		if (character.Quests.HasCompleted(Seir324Mq01))
			await dialog.Msg(L("Thank you so much for rescuing Goddess Lada! And our village!"));
		else if (character.Quests.HasCompleted(Sq05))
			await dialog.Msg(L("Uh... you say the statue only made the ferrets even more violent? Maybe it's because the statue is not completed yet... I feel so sorry for bothering Widas now..."));
		else if (character.Quests.HasCompleted(Mq05))
			await dialog.Msg(L("A real Revelator... I heard about you from the village priest. He says he was in danger and you came to save him with a light? That light... I want to see it too!"));
		else
			await dialog.Msg(L("Who are you? Is this your first time here? Did you get permission from my grandfather?"));
	}

	/// <summary>
	/// The Village Resident's dialog.
	/// </summary>
	private async Task Resident(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Village Resident"));

		if (character.Quests.IsCompletable(Sq01))
		{
			await dialog.Msg(L("Wow, thank you. With this much juice I won't have to worry for quite some time."));
			await dialog.Msg(L("But, um... Sigh, those dogged ferrets..."));
			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (character.Quests.IsCompletable(Sq03))
		{
			await dialog.Msg(L("I sure hope the ferrets won't come to steal our food again after this."));
			await dialog.Msg(L("It's a bit of a waste but... I hope they'll just eat it and leave us alone."));
			await dialog.CompleteQuest(Sq03);
			character.LookAround();
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("Ah... And we can't let them know in Fedimian... We're in a really tough spot."));
			await dialog.Msg(L("Revelator, you didn't drink the red water, did you? I mean... If you had you probably wouldn't be here talking to me."));
			await dialog.Msg(L("Just don't drink the red water no matter how thirsty you are. The pain will kill you."));
			await dialog.Msg(L("The water is so toxic we can't even eat our fruit. It's not even the taste. When they first started to grow we were so glad but..."));
			await dialog.Msg(L("But then, one day I got lost around the Menacing Lowlands and I was really thirsty. Then I noticed some fruit juice that belonged to the ferrets."));
			await dialog.Msg(L("I figured whether I died of thirst or at the hands of the ferrets I was doomed either way. So I drank the ferrets' juice and... I don't know, nothing happened."));
			await dialog.Msg(L("I don't know what they did but it looked like a good substitute for water. The water in the wells is starting to turn red too, you see."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("Would you get some of the ferrets' fruit juice? Please. We can't live with no water."),
				Option(L("I'll get it"), "accept"),
				Option(L("Why don't you ask for help around here?"), "explain"),
				Option(L("I don't have time for that"), "leave")
			);

			if (answer == "explain")
			{
				await dialog.Msg(L("We want to ask for help, too. We want it more than anyone. Be it Orsha... or Fedimian, which is closer... But that's just something we can't do."));
				await dialog.Msg(L("Orsha is too far away, and Fedimian is still recovering from Medzio Diena. Besides... that's not what Goddess Lada wants."));
				await dialog.Msg(L("She thinks that people will be too confused if they find out about her. I don't know why that would be so confusing, but if she says so, we must follow her will."));
				return;
			}

			if (answer == "accept")
				character.Quests.Start(Sq01);
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("While you were getting the juice, the ferrets came and stole our food again. We get it back, they steal it... I'm fed up with this!"));
			await dialog.Msg(L("This has to stop. This time we need to put an end to the ferrets' bad habits."));
			await dialog.Msg(L("We need to find the food and give it some red water \"seasoning\". They're all going to be rolling on the floor in pain after they eat it."));
			await dialog.Msg(L("The water in the old well south from here is particularly red. Maybe you can spray the food with the water from there."));

			var answer = await dialog.SelectQuestOffer(Sq02, L("Oh, make sure you spray it on the meat jerky hidden in the Banaga Forest Trail. Ferrets love meat jerky."),
				Option(L("I will come back after polluting it"), "accept"),
				Option(L("That's a bit too much"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Sq02);
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			await dialog.Msg(L("I drank it expecting to die but surprisingly I didn't. It even tasted good."));
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			await dialog.Msg(L("We can't let them get away with this. We've put up with it long enough."));
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("I know it doesn't feel good to pollute the food, but... That is the best way in the long term."));
			return;
		}

		if (character.Quests.HasCompleted(Seir324Mq01))
			await dialog.Msg(L("I can't believe the Demon Lord was defeated! And thanks to you the ferrets aren't stealing out food anymore. Thank you so much!"));
		else if (!character.Quests.HasCompleted(Mq05))
			await dialog.Msg(L("Who are you? I've never seen you before... Our men are too tired. If you have no business here, would you mind not bothering us?"));
		else if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("So you're a Revelator, like the ones in the legends? Wow... I really didn't know!"));
		else
			await dialog.Msg(L("Want to go see the ferrets? They love meat jerky. It must be dangerous, but I'm curious..."));
	}

	/// <summary>
	/// Dievdirbys Widas' dialog.
	/// </summary>
	private async Task Widas(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Dievdirbys Widas"));

		if (character.Quests.IsCompletable(Sq04))
		{
			await dialog.Msg(L("Oh, that? I'm so sorry about that. It's not the first time it happened, now it seems they're finding other people to bother."));
			await dialog.Msg(L("Just a moment. I'll give it to you right away."));
			await dialog.CompleteQuest(Sq04);
			return;
		}

		if (character.Quests.IsCompletable(Sq05))
		{
			await dialog.Msg(L("The ferrets are more violent? So I failed...? I'm so sorry. Are you sure you're not hurt?"));
			await dialog.Msg(L("Ah... But I'm not going to quit. They're probably still brainwashed by the demons."));
			await dialog.Msg(L("We should ask Benes when he comes back from studying the ferrets in Zeraha."));
			await dialog.CompleteQuest(Sq05);
			return;
		}

		if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
		{
			await dialog.Msg(L("I made something based on the totem I built for that filthy demon. I made it so that it radiates good instead of evil energy."));

			var answer = await dialog.SelectQuestOffer(Sq05, L("It's still only in testing phase, but it'll be worth something. Place this near the ferrets and watch their reaction."),
				Option(L("I'll go now"), "accept"),
				Option(L("I don't have time for that"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq05);

				if (character.Inventory.CountItem(ItemId.ORCHARD_323_SQ_CARVE) == 0)
					character.Inventory.Add(ItemId.ORCHARD_323_SQ_CARVE, 1, InventoryAddType.PickUp);
			}
			return;
		}

		if (character.Quests.IsActive(Sq05))
		{
			if (character.Inventory.CountItem(ItemId.ORCHARD_323_SQ_CARVE) == 0)
				character.Inventory.Add(ItemId.ORCHARD_323_SQ_CARVE, 1, InventoryAddType.PickUp);

			await dialog.Msg(L("This is all our fault. We should have been more suspicious, but the name of Revelator got us hopeful."));
			await dialog.Msg(L("I wish this statue could bring peace between us and the ferrets."));
			return;
		}

		if (character.Quests.HasCompleted(Seir324Mq01))
			await dialog.Msg(L("Finding out Goddess Lada is safe is the best news I've heard since Medzio Diena. Seeing all the commotion with the ferrets, however, I better finish the statue soon."));
		else if (character.Quests.HasCompleted(Sq05))
			await dialog.Msg(L("This thing about the ferrets... It feels like when we were tricked into making that demon totem... I'll try and start over again."));
		else if (character.Quests.HasCompleted(Mq05))
		{
			await dialog.Msg(L("Our village has had a really bad experience with a demon before. It came to us pretending to be a legendary Revelator."));
			await dialog.Msg(L("Of course believing it was our mistake but... We never thought we'd see an actual Revelator after that."));
		}
		else
			await dialog.Msg(L("No... This isn't it... Hm? Stop interrupting and go away. You're distracting me."));
	}

	/// <summary>
	/// The Village Priest's dialog.
	/// </summary>
	private async Task Priest(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Village Priest"));

		if (!character.Quests.HasCompleted(Mq04))
		{
			await dialog.Msg(L("I'm sorry but... if you have no business here, you better leave now. The atmosphere in the village isn't good..."));
			return;
		}

		if (!character.Quests.HasCompleted(Mq05))
		{
			await dialog.Msg(L("I'm sorry. I can't really give you any details... Just go back and don't ask questions..."));
			return;
		}

		await dialog.Msg(L("I saw it. The bright glowing light that came with you. If it wasn't for that I would have been attacked by the ferrets."));
		await dialog.Msg(L("But who was that girl, I wonder...?"));
	}

	/// <summary>
	/// The Great Priest of the Village's dialog.
	/// </summary>
	private async Task GreatPriest(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Great Priest of the Village"));

		if (!character.Quests.HasCompleted(Mq05))
		{
			await dialog.Msg(L("If you're done here I think you should go. This village is done anyways..."));
			return;
		}

		await dialog.Msg(L("I almost made another big mistake, didn't I? I'm sorry. All of these tragedies happening in our village have really ruined our mood."));
		await dialog.Msg(L("We were worried about the river turning red... Then a demon disguised as a Revelator... It was my fault for not realizing..."));
		await dialog.Msg(L("Because of the demon Goddess Lada was taken away, and our people didn't have the power to do anything... I'm so sorry... I hope you will understand..."));
		await dialog.Msg(L("If you ever meet that demon, be careful. It knows how to act and look like a person, you can't even tell it's not human."));
	}

	/// <summary>
	/// Priest Rovli's dialog on the road to the shrines.
	/// </summary>
	private async Task Rovli(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Priest Rovli"));

		if (character.Quests.IsCompletable(Rp1))
		{
			await dialog.Msg(L("Do you think that they've learned their lesson? Thank you so much for helping."));
			await dialog.CompleteQuest(Rp1);
			return;
		}

		if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
		{
			await dialog.Msg(L("The villagers and other priests believe that the ferrets can be converted."));
			await dialog.Msg(L("It's all useless! What use is converting the ferrets after the shrine and village are destroyed."));

			var answer = await dialog.SelectQuestOffer(Rp1, L("Please help me. Teach the ferrets a lesson by dealing with them to show that they can't come here!"),
				Option(L("Alright, I'll help you"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Rp1);
			return;
		}

		if (character.Quests.IsActive(Rp1))
		{
			await dialog.Msg(L("I'm begging passing adventurers since I'm a priest but.. I don't know how it came to this..."));
			return;
		}

		await dialog.Msg(L("They're all afraid of the ferrets... I'll defend this place by myself if I have to."));
	}

	/// <summary>
	/// Sprinkles the red water over the meat jerky the ferrets hid.
	/// </summary>
	private static void ContaminateJerky(Character character)
	{
		if (!character.Quests.IsActive(Sq03) || character.Quests.IsCompletable(Sq03))
			return;

		if (character.Inventory.CountItem(ItemId.ORCHARD_323_SQ_WATER) == 0)
		{
			character.ServerMessage(L("You need the red water from the old well."));
			return;
		}

		character.PlayEffect("F_light018_yellow", 1f);
		character.Quests.CompleteObjective(Sq03, "contaminateFood");
		character.ServerMessage(L("You sprinkled the red water over the meat jerky."));
		character.LookAround();
	}

	/// <summary>
	/// Sprinkles the red water from the old well on the meat jerky.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_323_SQ_WATER(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_32_3" || !character.Quests.IsActive(Sq03) || character.Position.Get2DDistance(JerkySpot) > JerkyRange)
		{
			character.ServerMessage(L("There is no meat jerky here."));
			return ItemUseResult.OkayNotConsumed;
		}

		ContaminateJerky(character);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Sets up Widas' statue near the ferrets.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_323_SQ_CARVE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_32_3" || !character.Quests.IsActive(Sq05) || character.Quests.IsCompletable(Sq05))
		{
			character.ServerMessage(L("There is no reason to set up the statue here."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Position.Get2DDistance(FerretDen) > StatueRange)
		{
			character.ServerMessage(L("There are no ferrets around to watch."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.PlayEffect("F_light018_yellow", 1f);
		character.Quests.CompleteObjective(Sq05, "placeStatue");
		character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The ferrets around the statue became even more violent!"), 3);

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Returns whether the ferrets' dropped offering tool is still lying
	/// where the character can pick it up.
	/// </summary>
	private static bool IsRitualDeviceLying(Character character)
		=> character.Quests.HasCompleted(Mq05) && !character.Quests.Has(Hq1) && character.Inventory.CountItem(ItemId.ORCHARD323_HIDDENQ1_ITEM) == 0;

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

// 80018: Helping Hand
//-----------------------------------------------------------------------------
public class FOrchard323Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80018);
		SetName(L("Helping Hand"));
		SetDescription(L("Druid Leja got hurt trying to catch the ferrets that stole food from the village. Go to the place she instructed and collect herbs to help her regain her energy."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_LEJA", "f_orchard_32_3", L("Talk to Druid Leja"), L("Leja looks hurt and in need of help. Talk to her."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_HERB", "f_orchard_32_3", L("Collect the Herbs requested by Druid Leja"), L("Druid Leja got hurt trying to catch the ferrets that stole food from the village. Go to the place she instructed and collect herbs to help her regain her energy."));
		SetPhase(QuestStatus.Success, "ORCHARD323_LEJA", "f_orchard_32_3", L("Deliver the herbs to Druid Leja"), L("Acquired all the herbs as requested by Druid Leja. Return to Leja."));

		AddPrerequisite(new QuestStatusPrerequisite(70448, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(82));

		AddObjective("collectHerbs", L("Collect Hemostasis Herbs"), new CollectItemObjective("ORCHARD_323_MQ_01_ITEM", 6));

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("Vis", 1140));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself and chains straight into the girl's appearance.
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(80019));
		character.LookAround();
	}
}

// 80019: The Mysterious Girl (1)
//-----------------------------------------------------------------------------
public class FOrchard323Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80019);
		SetName(L("The Mysterious Girl (1)"));
		SetDescription(L("On your way to deliver the herbs requested by Druid Leja, you came across a mysterious girl. For now, deliver the herbs to Leja."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "ORCHARD323_MQ_02_TRIG", "f_orchard_32_3", L("Meeting the Mysterious Girl"), L("A sacred energy is felt in the air."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_MQ_02_TRIG", "f_orchard_32_3", L("Meeting the Mysterious Girl"), L("A sacred energy is felt in the air."));
		SetPhase(QuestStatus.Success, "ORCHARD323_LEJA", "f_orchard_32_3", L("Talk to Druid Leja"), L("On your way to deliver the herbs requested by Druid Leja, you came across a mysterious girl. For now, deliver the herbs to Leja."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ORCHARD_323_MQ_02_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(80018, QuestStatus.Completed));

		AddObjective("meetGirl", L("Meeting the Mysterious Girl"), new ManualObjective());

		AddReward(new TakeItemReward("ORCHARD_323_MQ_01_ITEM", -1));
	}
}

// 80020: Reclaiming Food
//-----------------------------------------------------------------------------
public class FOrchard323Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80020);
		SetName(L("Reclaiming Food"));
		SetDescription(L("Retrieve the food which the ferrets had stolen for the villagers. You can get them back when you defeat ferrets."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_LEJA", "f_orchard_32_3", L("Talk to Druid Leja"), L("On your way to deliver the herbs requested by Druid Leja, you came across a mysterious girl. Talk to Leja about the girl."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Bring back the stolen food from the ferrets"), L("Retrieve the food which the ferrets had stolen for the villagers. You can get them back when you defeat ferrets."));
		SetPhase(QuestStatus.Success, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Deliver the Recovered Food to the Village Chief"), L("You have recovered all the food items stolen by the ferrets. As Leja instructed, go find the village chief by the Ephlysti Crossing."));

		AddPrerequisite(new QuestStatusPrerequisite(80019, QuestStatus.Completed));

		AddObjective("collectFood", L("Take the Stolen Food back from the ferrets"), new CollectItemObjective("ORCHARD_323_MQ_03_ITEM2", 8));
		AddPityDrop("ORCHARD_323_MQ_03_ITEM2", 0.6f, 3, 1, "ferret_patter", "ferret_searcher", "ferret_slinger");

		AddReward(new ItemReward("expCard6", 4));
		AddReward(new ItemReward("Vis", 2280));
		AddReward(new ItemReward("Drug_SP2_Q", 20));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself; the food is delivered in The Mysterious Girl (2).
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(80021));
		character.LookAround();
	}
}

// 80021: The Mysterious Girl (2)
//-----------------------------------------------------------------------------
public class FOrchard323Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80021);
		SetName(L("The Mysterious Girl (2)"));
		SetDescription(L("As you were coming back after retrieving the food from the ferrets, you encountered the mysterious girl again. The girl saved the villagers and disappeared with the ray. Go meet the village elder with the retrieved food."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_MQ_04_TRIG", "f_orchard_32_3", L("Meeting with the mysterious girl"), L("A sacred energy is felt in the air."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_MQ_04_TRIG", "f_orchard_32_3", L("Meeting with the mysterious girl"), L("A sacred energy is felt in the air."));
		SetPhase(QuestStatus.Success, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Talk to the Village Headman"), L("As you were coming back after retrieving the food from the ferrets, you encountered the mysterious girl again. The girl saved the villagers and disappeared with the ray. Go meet the village elder with the retrieved food."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ORCHARD_323_MQ_04_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(80020, QuestStatus.Completed));

		AddObjective("meetGirl", L("Meeting with the mysterious girl"), new ManualObjective());

		AddReward(new TakeItemReward("ORCHARD_323_MQ_03_ITEM2", -1));
	}
}

// 80022: Ferret-Controlling Totem
//-----------------------------------------------------------------------------
public class FOrchard323Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80022);
		SetName(L("Ferret-Controlling Totem"));
		SetDescription(L("The village chief has been tricked by a false Revelator before and is wary of trusting you. Destroy the demon totems in the Banaga Forest Trail to prove you're a real Revelator."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Talk to the Village Headman"), L("You need to obtain information regarding the mysterious girl and the folklore that has been passed down for generations in the village. Talk to the Village Headman."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_EVIL", "f_orchard_32_3", L("Destroy the Demon Totems brainwashing the ferrets"), L("The village chief has been tricked by a false Revelator before and is wary of trusting you. Destroy the demon totems in the Banaga Forest Trail to prove you're a real Revelator."));
		SetPhase(QuestStatus.Success, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Talk to the Village Headman"), L("You have destroyed all demon totems. Return to the village chief and report this."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ORCHARD_323_MQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(80021, QuestStatus.Completed));

		AddObjective("destroyTotem", L("Destroy the demonic totem"), new KillObjective(1, "bube_flag1") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 5));
		AddReward(new ItemReward("Vis", 2895));
	}
}

// 80023: The Mysterious Girl (3)
//-----------------------------------------------------------------------------
public class FOrchard323Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80023);
		SetName(L("The Mysterious Girl (3)"));
		SetDescription(L("The village chief told you of an old village legend and believes that the girl was sent by Laima. Follow the girl to Zeraha."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Talk to the Village Headman"), L("The village chief is grateful to you for having destroyed the totems. Ask the village chief about what to do next."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_MQ_06_TRIG", "f_orchard_32_3", L("Follow the Mysterious Girl"), L("The village chief told you of an old village legend and believes that the girl was sent by Laima. Follow the girl to Zeraha."));
		SetPhase(QuestStatus.Success, "ORCHARD323_MQ_06_TRIG", "f_orchard_32_3", L("Meeting with the mysterious girl"), L("The village chief told you of an old village legend and believes that the girl was sent by Laima. Follow the girl to Zeraha."));

		AddPrerequisite(new QuestStatusPrerequisite(80022, QuestStatus.Completed));

		AddObjective("followGirl", L("Follow the Mysterious Girl"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself; the girl's trail picks up in Zeraha.
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(80029));
		character.LookAround();
	}
}

// 80024: Thirst for Drinks
//-----------------------------------------------------------------------------
public class FOrchard323Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80024);
		SetName(L("Thirst for Drinks"));
		SetDescription(L("The villager says that the red water from the river has contaminated their fruit, but that the ferrets might know a way of making them safe to eat. Collect fruit juice from the ferrets."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_PEOPLE", "f_orchard_32_3", L("Talk to the villager"), L("Ask the villager if there's anything you can help with"));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_PEOPLE", "f_orchard_32_3", L("Collect Gigantic Fruit Juice from the ferrets"), L("The villager says that the red water from the river has contaminated their fruit, but that the ferrets might know a way of making them safe to eat. Collect fruit juice from the ferrets."));
		SetPhase(QuestStatus.Success, "ORCHARD323_PEOPLE", "f_orchard_32_3", L("Deliver to the Villager"), L("Got enough Gigantic Fruit from the ferrets. Bring it to the Village Resident."));

		AddPrerequisite(new QuestStatusPrerequisite(80023, QuestStatus.Completed));

		AddObjective("collectJuice", L("Collect Gigantic Fruit Juice from the ferrets"), new CollectItemObjective("ORCHARD_323_SQ_01_ITEM", 10));
		AddPityDrop("ORCHARD_323_SQ_01_ITEM", 0.6f, 3, 1, "ferret_patter", "ferret_searcher", "ferret_slinger");

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("Vis", 1140));
		AddReward(new TakeItemReward("ORCHARD_323_SQ_01_ITEM", -1));
	}
}

// 80025: Putting Spices (1)
//-----------------------------------------------------------------------------
public class FOrchard323Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80025);
		SetName(L("Putting Spices (1)"));
		SetDescription(L("The villager is planning to contaminate the food stolen by the ferrets with red water. First, get some red water from the old well."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_PEOPLE", "f_orchard_32_3", L("Talk to the villager"), L("While you were away getting the Gigantic Fruit Juice, the ferrets raided the town again and stole foods from the town. Speak with the townfolk."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_WELL", "f_orchard_32_3", L("Take the red water from the Old Well"), L("The villager is planning to contaminate the food stolen by the ferrets with red water. First, get some red water from the old well."));
		SetPhase(QuestStatus.Success, "ORCHARD323_WELL", "f_orchard_32_3", L("Take the red water from the Old Well"), L("The villager is planning to contaminate the food stolen by the ferrets with red water. First, get some red water from the old well."));

		AddPrerequisite(new QuestStatusPrerequisite(80024, QuestStatus.Completed));

		AddObjective("scoopWater", L("Obtain Dirty Red Water from the old well"), new CollectItemObjective("ORCHARD_323_SQ_WATER", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself and chains straight into seasoning the jerky.
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(80026));
		character.LookAround();
	}
}

// 80026: Putting Spices (2)
//-----------------------------------------------------------------------------
public class FOrchard323Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80026);
		SetName(L("Putting Spices (2)"));
		SetDescription(L("The village resident asked you to contaminate the ferrets' meat jerky with the red water."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_PEOPLE", "f_orchard_32_3", L("Contaminate the Stolen Food With Red Water"), L("The village resident asked you to contaminate the ferrets' meat jerky with the red water."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_FOOD", "f_orchard_32_3", L("Contaminate the Stolen Food With Red Water"), L("The village resident asked you to contaminate the ferrets' meat jerky with the red water."));
		SetPhase(QuestStatus.Success, "ORCHARD323_PEOPLE", "f_orchard_32_3", L("Talk to the villager"), L("You found the meat jerky and used the red water to contaminate it. Return to the villager."));

		AddPrerequisite(new QuestStatusPrerequisite(80025, QuestStatus.Completed));

		AddObjective("contaminateFood", L("Contaminate the Stolen Food With Red Water"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1710));
		AddReward(new TakeItemReward("ORCHARD_323_SQ_WATER", -1));
	}
}

// 80027: Statue of Peace (1)
//-----------------------------------------------------------------------------
public class FOrchard323Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80027);
		SetName(L("Statue of Peace (1)"));
		SetDescription(L("The elder's grandson explains that even though the demon totems are gone, the ferrets are still ferocious, Consult with Dievdirbys Widas about the way to revert the ferrets back to normal."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_GRANDSON", "f_orchard_32_3", L("Talk to the Elder's grandchild"), L("The village chief's grandson is back from checking up on the ferrets after the totem was destroyed. He seems disapponted. Talk to him."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_VYDAS", "f_orchard_32_3", L("Talk to Dievdirbys Widas"), L("The elder's grandson explains that even though the demon totems are gone, the ferrets are still ferocious, Consult with Dievdirbys Widas about the way to revert the ferrets back to normal."));
		SetPhase(QuestStatus.Success, "ORCHARD323_VYDAS", "f_orchard_32_3", L("Talk to Dievdirbys Widas"), L("The elder's grandson explains that even though the demon totems are gone, the ferrets are still ferocious, Consult with Dievdirbys Widas about the way to revert the ferrets back to normal."));

		AddPrerequisite(new QuestStatusPrerequisite(80023, QuestStatus.Completed));

		AddObjective("askWidas", L("Talk to Dievdirbys Widas"), new ManualObjective());
	}
}

// 80028: Statue of Peace (2)
//-----------------------------------------------------------------------------
public class FOrchard323Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80028);
		SetName(L("Statue of Peace (2)"));
		SetDescription(L("Dievdirbys Widas says he carved a statue that radiates good energy based on the totem he made for the demons. Set up Widas' statue close to the ferrets and watch their reaction."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_VYDAS", "f_orchard_32_3", L("Talk to Dievdirbys Widas"), L("Dievdirbys Widas says he carved a statue that radiates good energy based on the totem he made for the demons. Talk to Widas again."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_VYDAS", "f_orchard_32_3", L("Set up Widas' Statue close to the ferrets"), L("Dievdirbys Widas says he carved a statue that radiates good energy based on the totem he made for the demons. Set up Widas' statue close to the ferrets and watch their reaction."));
		SetPhase(QuestStatus.Success, "ORCHARD323_VYDAS", "f_orchard_32_3", L("Talk to Dievdirbys Widas"), L("After raising Widas' statue arround the ferrets, they became unexpectedly more violent. Tell Widas about this."));

		AddPrerequisite(new QuestStatusPrerequisite(80027, QuestStatus.Completed));

		AddObjective("placeStatue", L("Set up Widas' Statue close to the ferrets"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1710));
		AddReward(new ItemReward("Drug_SP2_Q", 25));
		AddReward(new TakeItemReward("ORCHARD_323_SQ_CARVE", -1));
	}
}

// 60184: Violent Rampage
//-----------------------------------------------------------------------------
public class FOrchard323Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60184);
		SetName(L("Violent Rampage"));
		SetDescription(L("Priest Rovli wishes to teach the Ferrets that are wrecking havoc on roads that lead to the major shrines a lesson. Teach the Ferrets in the surrounding area a lesson."));
		SetType(QuestType.Repeat);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_RP_1_NPC", "f_orchard_32_3", L("Talk to Priest Rovli"), L("Priest Rovli is waiting for help at Bellai Rainforest."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_RP_1_NPC", "f_orchard_32_3", L("Teach the Ferrets a lesson"), L("Priest Rovli wishes to teach the Ferrets that are wrecking havoc on roads that lead to the major shrines a lesson. Teach the Ferrets in the surrounding area a lesson."));
		SetPhase(QuestStatus.Success, "ORCHARD323_RP_1_NPC", "f_orchard_32_3", L("Report back to Priest Rovli"), L("You have scared the Ferrets enough. Go back to Priest Rovli."));

		AddPrerequisite(new LevelPrerequisite(82));

		AddObjective("killFerrets", L("Defeat the ferrets"), new KillObjective(9, "ferret_patter", "ferret_slinger", "ferret_searcher"));

		AddReward(new ItemReward("expCard6", 2));
	}
}

// 50275: Ferrets and Their Grabby Hands
//-----------------------------------------------------------------------------
public class FOrchard323Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50275);
		SetName(L("Ferrets and Their Grabby Hands"));
		SetDescription(L("The head of the village wants you to take back the rest of the offering tools from the ferrets. Defeat the ferrets and take back the tools."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Talk to the Village Headman"), L("The head of the village is curious to know how the missing offering tools showed up. Talk to him."));
		SetPhase(QuestStatus.InProgress, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Retrieve the Offering Tools"), L("The head of the village wants you to take back the rest of the offering tools from the ferrets. Defeat the ferrets and take back the tools."));
		SetPhase(QuestStatus.Success, "ORCHARD323_MAYOR", "f_orchard_32_3", L("Talk to the Village Headman"), L("You have collected all of the offering tools. Return to the head of the village and give him the tools."));

		AddPrerequisite(new ItemPrerequisite("ORCHARD323_HIDDENQ1_ITEM"));
		AddPrerequisite(new LevelPrerequisite(90));

		AddObjective("collectTools", L("Retrieve the Offering Tools"), new CollectItemObjective("ORCHARD323_HIDDENQ1_ITEM2", 10));
		AddPityDrop("ORCHARD323_HIDDENQ1_ITEM2", 1.0f, 0, 1, "ferret_patter", "ferret_slinger");

		AddReward(new ItemReward("misc_scrollskulp", 1));
		AddReward(new TakeItemReward("ORCHARD323_HIDDENQ1_ITEM2", -1));
		AddReward(new TakeItemReward("ORCHARD323_HIDDENQ1_ITEM", -1));
	}
}
