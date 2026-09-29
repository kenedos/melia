//--- Melia Script ----------------------------------------------------------
// Zeraha Quest NPCs
//--- Description -----------------------------------------------------------
// Following the mysterious girl through Zeraha with Druid Leja, Demon Lord
// Zaura's ambush, and Druid Benes' study of the ferrets.
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
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard342QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Bellai323Mq03 = new QuestId(80020);
	private readonly static QuestId Mq01 = new QuestId(80029);
	private readonly static QuestId Mq02 = new QuestId(80030);
	private readonly static QuestId Mq03 = new QuestId(80031);
	private readonly static QuestId Mq04 = new QuestId(80032);
	private readonly static QuestId Mq05 = new QuestId(80033);
	private readonly static QuestId Mq06 = new QuestId(80034);
	private readonly static QuestId Mq07 = new QuestId(80035);
	private readonly static QuestId Mq08 = new QuestId(80036);
	private readonly static QuestId Sq01 = new QuestId(80037);
	private readonly static QuestId Sq02 = new QuestId(80038);
	private readonly static QuestId Sq03 = new QuestId(80039);
	private readonly static QuestId Sq04 = new QuestId(80040);
	private readonly static QuestId Seir324Mq01 = new QuestId(80041);
	private readonly static QuestId Seir324Mq07 = new QuestId(80047);

	public const string NotesVar = "Gabija.Quests.Orchard342Sq01.Notes";
	public const string TalksVar = "Gabija.Quests.Orchard342Sq02.Talks";
	private const string IncenseVar = "Gabija.Quests.Orchard342Mq05.Incense";
	private const string IncenseUsesVar = "Gabija.Quests.Orchard342Mq05.Uses";
	private const string HerbVar = "Gabija.Quests.Orchard342Mq04.Herb";
	private const string ScrollUsedVar = "Gabija.Quests.Orchard342.ScrollUsed";

	private const int HerbsNeeded = 6;
	private const int NotesNeeded = 3;
	private const int TalksNeeded = 3;
	private const int IncenseUses = 3;
	private const int FerretRange = 200;
	private const int GirlRange = 200;
	private const int WoodRange = 300;
	private const int WorkshopRange = 300;

	private static readonly TimeSpan GatherRespawn = TimeSpan.FromSeconds(30);
	private static readonly TimeSpan ScrollCooldown = TimeSpan.FromSeconds(5);

	private static readonly Position TiedGirlSpot = new Position(-623.66f, -81.89f, 1655.53f);
	private static readonly Position WoodSpot = new Position(-589f, -69.99f, -777f);
	private static readonly Position WorkshopEntrance = new Position(-343f, -6f, 432f);

	private static readonly string[] Ferrets = { "ferret_folk", "ferret_loader" };

	private static readonly double[,] DrowsyHerbs =
	{
		{ 1738.09, -662.35 }, { 1797.89, -479.65 }, { 1867.28, -585.40 }, { 1894.20, -496.00 }, { 1975.53, -599.70 },
		{ 2006.79, -511.46 }, { 2061.80, -712.00 }, { 2098.04, -620.80 }, { 1825.42, -711.45 }, { 1984.54, -821.36 },
		{ 1714.15, -731.71 }, { 1819.63, -640.91 }, { 2072.05, -546.44 }, { 2039.73, -775.71 }, { 1997.47, -663.65 },
	};

	private static readonly double[,] Sacks =
	{
		{ 409.59, 1776.27 }, { 71.93, 1596.35 }, { 237.95, 1654.70 }, { -120.93, 1451.03 },
	};

	protected override void Load()
	{
		// Druid Leja and Druid Benes
		//-------------------------------------------------------------------------
		AddConditionalNpc(156003, L("Druid Leja"), "ORCHARD342_LEJA", "f_orchard_34_2", 1194.33, 268.31, 0, c => c.Quests.Has(Bellai323Mq03), this.Leja);
		AddNpc(156004, L("Druid Benes"), "ORCHARD342_BENES", "f_orchard_34_2", -1180.39, -358.17, 90, this.Benes);

		// The trail the girl leaves through Zeraha
		//-------------------------------------------------------------------------
		AddQuestTrigger("ORCHARD342_MQ_01_TRIG", "f_orchard_34_2", 403.41, 900.94, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
				character.Quests.Start(Mq01);

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
			{
				character.Quests.CompleteObjective(Mq01, "followTrace");
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The girl appeared and asked me to follow her."), 5);
			}

			await Task.CompletedTask;
		});

		AddQuestTrigger("ORCHARD_342_MQ_03_MINI", "f_orchard_34_2", -597.15, -809.85, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
				character.Quests.Start(Mq03);

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
				character.Quests.StartQuestTrack(Mq03);

			await Task.CompletedTask;
		});

		AddQuestTrigger("ORCHARD342_MQ_06_TRIG", "f_orchard_34_2", -789.29, 605.04, 100, async args =>
		{
			if (args.Initiator is not Character character || !character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			character.Quests.CompleteObjective(Mq06, "findGirl");
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The girl stared at me as if she wanted me to follow her."), 5);
			character.LookAround();

			await Task.CompletedTask;
		});

		// Drowsy Herbs on the cliff above the Balais Highway
		//-------------------------------------------------------------------------
		for (var i = 0; i < DrowsyHerbs.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "ORCHARD342_SLEEP" : "ORCHARD342_SLEEP_" + number;

			AddConditionalNpc(47201, L("Drowsy Herb"), uniqueName, "f_orchard_34_2", DrowsyHerbs[i, 0], DrowsyHerbs[i, 1], 90,
				c => c.Quests.IsActive(Mq04) && !c.Quests.IsCompletable(Mq04),
				async dialog =>
				{
					var character = dialog.Player;
					if (!character.Quests.IsActive(Mq04) || character.Inventory.CountItem(ItemId.ORCHARD_342_MQ_04_ITEM) >= HerbsNeeded)
						return;

					if (Gather(character, HerbVar + number, L("The Drowsy Herbs here have already been picked.")))
						character.Inventory.Add(ItemId.ORCHARD_342_MQ_04_ITEM, 1, InventoryAddType.PickUp);

					await Task.CompletedTask;
				});
		}

		// The ferret hideout at the Zelbe Shelter
		//-------------------------------------------------------------------------
		AddConditionalNpc(156042, L("Mysterious Girl"), "ORCHARD42_BINDIG_GIRL", "f_orchard_34_2", TiedGirlSpot.X, TiedGirlSpot.Z, 0, c => c.Quests.IsActive(Mq05) && !c.Quests.IsCompletable(Mq05), this.TiedGirl);

		for (var i = 0; i < Sacks.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "ORCHARD342_FAKE" : "ORCHARD342_FAKE_" + number;

			AddConditionalNpc(153041, L("Suspicious Sack"), uniqueName, "f_orchard_34_2", Sacks[i, 0], Sacks[i, 1], 90,
				c => c.Quests.IsActive(Mq05) && !c.Quests.IsCompletable(Mq05),
				async dialog =>
				{
					dialog.Player.ServerMessage(L("There is nothing in the sack but crops."));
					await Task.CompletedTask;
				});
		}

		// The great statue of Goddess Laima
		//-------------------------------------------------------------------------
		AddConditionalNpc(47236, L("Mysterious Girl"), "ORCHARD342_GIRL", "f_orchard_34_2", -1753.73, 695.96, 190, c => c.Quests.Has(Mq06) && !c.Quests.Has(Mq07), this.Girl);
		AddConditionalNpc(156036, L("Shiny Sapling"), "ORCHARD342_TREE", "f_orchard_34_2", -1784.86, 745.74, 90, c => c.Quests.Has(Mq06) && !c.Quests.IsActive(Mq08), this.Sapling);
		AddConditionalNpc(156041, L("Goddess' Orb"), "ORCHARD342_HOLY_TALK", "f_orchard_34_2", -1784.86, 745.74, 90, c => c.Quests.IsActive(Mq08), this.GoddessOrb);

		AddConditionalNpc(147413, "UnvisibleName", "ORCHARD342_CRYSTAL", "f_orchard_34_2", -1385.86, 1161.03, 280, IsCrystalCalling, async dialog =>
		{
			if (IsCrystalCalling(dialog.Player))
				dialog.Player.Quests.StartQuestTrack(Seir324Mq07);

			await Task.CompletedTask;
		});

		AddQuestTrigger("ORCHARD342_CRYSTAL_TRIGGER", "f_orchard_34_2", -1385.86, 1161.03, 150, async args =>
		{
			if (args.Initiator is Character character && IsCrystalCalling(character))
				character.Quests.StartQuestTrack(Seir324Mq07);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Druid Leja's dialog in Zeraha.
	/// </summary>
	private async Task Leja(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Druid Leja"));

		if (character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("Revelator, you're late!"));
			await dialog.Msg(L("Oh... I didn't know. I'm so sorry. I've talked to our village chief about it countless times but..."));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (character.Quests.IsCompletable(Mq03))
		{
			var told = await character.TimeActions.StartAsync(L("Telling her the girl was kidnapped"), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
			if (told != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("Demon Lord Zaura took the girl? Oh no... Zaura is the one who had Goddess Lada taken away by demons in the past."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("This should be enough to make the scent. Hold on a minute."));

			dialog.Npc.PlayAnimation("fire");
			await Task.Delay(TimeSpan.FromSeconds(2));
			dialog.Npc.PlayEffect("F_pc_making_finish_white", 2f, 1, EffectLocation.Bottom);

			await dialog.CompleteQuest(Mq04);
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			await dialog.Msg(L("What? You mean the girl could have been sent by Goddess Laima? I saw her running away from the ferrets to the Broken Bridge!"));
			await dialog.Msg(L("I wanted to chase after them but... The ferrets were so many, there was nothing I could do."));

			var answer = await dialog.SelectQuestOffer(Mq02, L("We have no time to waste. We can still find the girl if we go after the ferrets now!"),
				Option(L("I'll go immediately"), "accept"),
				Option(L("I need to prepare"), "leave")
			);

			if (answer != "accept")
				return;

			var talked = await character.TimeActions.StartAsync(L("Talking about the mysterious girl"), L("Cancel"), "TALK", TimeSpan.FromSeconds(1));
			if (talked != TimeActionResult.Completed)
				return;

			character.Quests.Start(Mq02);
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			await dialog.Msg(L("Oh, right. When Zaura took the girl, did you see any ferrets around? That means the ferrets are keeping the girl."));
			await dialog.Msg(L("They know the forest well, you see. I mean, I was born and raised here so I'm not clueless either. I think... I can guess where they're hiding her."));
			await dialog.Msg(L("It's too dangerous to just go out to save her without a plan, though. Things could get ugly if they start attacking."));

			var answer = await dialog.SelectQuestOffer(Mq04, L("Right. I know a scent that makes ferrets lose their minds. Will you bring me some Drowsy Herbs so I can make some?"),
				Option(L("I'll get it"), "accept"),
				Option(L("I don't think that'll be necessary"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("Here's the scent. Let's see now... About the ferrets..."));
			await dialog.Msg(L("There are a few ferret hideouts around the Balais Highway and the Zelbe Shelter. That's probably where they're keeping the girl."));

			var answer = await dialog.SelectQuestOffer(Mq05, L("When you find the girl, throw the scent pouch I gave you. When the ferrets react to it, take the girl and come back!"),
				Option(L("I'll do it right away"), "accept"),
				Option(L("I need some time to prepare"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.Set(IncenseVar, false);
				character.Variables.Perm.SetInt(IncenseUsesVar, 0);
				character.Quests.Start(Mq05);
				GiveIncense(character);
				character.LookAround();

				await dialog.Msg(L("The amount of scent I made is only enough for about three uses. Make sure you only use it when it's absolutely necessary."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("I should have gone after them... I'm so worried now. I hope she's okay..."));
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			character.Quests.ClearQuestTrack(Mq03);
			await dialog.Msg(L("If Zaura didn't want you and the girl to meet... Then she might just be the key to rescuing Goddess Lada."));
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("Be careful when you collect Drowsy Herbs. There's a reason why they call them that."));
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			if (character.Inventory.CountItem(ItemId.ORCHARD_342_MQ_05_ITEM) == 0)
			{
				character.Variables.Perm.SetInt(IncenseUsesVar, 0);
				GiveIncense(character);
			}

			await dialog.Msg(L("Don't worry if you accidentally get a whiff of the herbs. I made sure it only affects ferrets."));
			await dialog.Msg(L("Quick, save the girl! We can't let Zaura have his way with this!"));
			return;
		}

		if (character.Quests.HasCompleted(Seir324Mq01))
		{
			await dialog.Msg(L("Do you remember? What happened when you first came to help me. If you hadn't saved me then... We wouldn't be able to save Goddess Lada either."));
			await dialog.Msg(L("As you know, Laima is the goddess of fate and foresight. I believe she's the one who put you in my path. Thank you very much."));
			await dialog.Msg(L("I expect this red water will have cleared out by the time you visit our village again."));
		}
		else if (character.Quests.HasCompleted(Mq03))
			await dialog.Msg(L("If Zaura didn't want you and the girl to meet... Then she might just be the key to rescuing Goddess Lada."));
		else
			await dialog.Msg(L("Hm? Revelator, did you speak with our village chief? Let me take a look here first, you go and help the villagers."));
	}

	/// <summary>
	/// Druid Benes' dialog at the Menacing Lowlands.
	/// </summary>
	private async Task Benes(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Druid Benes"));

		if (character.Quests.IsCompletable(Sq01))
		{
			await dialog.Msg(L("Your observations are pretty detailed. You should be ready to transform now. Hold on."));

			dialog.Npc.PlayAnimation("scroll");
			await Task.Delay(TimeSpan.FromSeconds(2));
			dialog.Npc.PlayEffect("F_pc_making_finish_white", 2f, 1, EffectLocation.Bottom);

			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (character.Quests.IsCompletable(Sq02))
		{
			var told = await character.TimeActions.StartAsync(L("Sharing what you found out"), L("Cancel"), "TALK", TimeSpan.FromSeconds(1));
			if (told != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("Hm? So what you're saying is... The ferrets are helping the demons on purpose?"));
			await dialog.Msg(L("How does... I can't believe it!"));
			await dialog.CompleteQuest(Sq02);
			return;
		}

		if (character.Quests.IsCompletable(Sq03))
		{
			var told = await character.TimeActions.StartAsync(L("Telling him the results"), L("Cancel"), "TALK", TimeSpan.FromSeconds(1));
			if (told != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("They really attacked the piece of wood? That's a bad sign. It means they're attacking anyone and everyone..."));
			await dialog.CompleteQuest(Sq03);
			return;
		}

		if (character.Quests.IsCompletable(Sq04))
		{
			await dialog.Msg(L("Thanks. I don't think the ferrets are going to go near the workshop now."));
			await dialog.Msg(L("It makes me mad to think the ferrets are using the scent thanks to the demons' dirty tricks. I should talk to Widas when I go back."));
			await dialog.Msg(L("But I still think there has to be a way to make the ferrets go back to normal. R-right...?"));
			await dialog.CompleteQuest(Sq04);
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("An outsider...? Hm, you'll have to do. Hey, um... will you give me a hand?"));
			await dialog.Msg(L("I'm trying to figure out why the ferrets are siding with the demons. So I found a way to turn into one and talk to them."));
			await dialog.Msg(L("Thing is, they've seen me too many times already. Even when I turn into a ferret, they recognize my smell and start attacking me."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("If that's okay with you, will you turn into a ferret and snoop around for me?"),
				Option(L("I'll help you"), "accept"),
				Option(L("I'm afraid I can't help you now"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(NotesVar, 0);
				character.Quests.Start(Sq01);
				GiveItem(character, ItemId.ORCHARD_342_SQ_01_SCROLL);

				await dialog.Msg(L("Before transforming into a ferret, you need to study their behavior. They're going to know something's up if you act weird."));
				await dialog.Msg(L("I'm going to give you an empty scroll so you can take notes on the ferrets. After that you can transform."));
			}
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("Here you go. The transformation scroll is ready."));
			await dialog.Msg(L("From your observations I don't think you'll be awkward at all. I trust that you'll be comfortable talking to the ferrets."));

			var answer = await dialog.SelectQuestOffer(Sq02, L("See if you can find out why the ferrets are siding with the demons. Don't forget to turn into a ferret first with the scroll."),
				Option(L("I will find out more about it"), "accept"),
				Option(L("I'll quit here"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(TalksVar, 0);
				character.Quests.Start(Sq02);
				GiveItem(character, ItemId.ORCHARD_342_SQ_02_TRANSFORM);
			}
			return;
		}

		if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
		{
			await dialog.Msg(L("Did the ferrets really say that? I know you have no reason to lie, but still... I really can't believe it, though."));
			await dialog.Msg(L("The ferrets have always been nothing but friendly. I need to see if they at least still feel some compassion to humans."));
			await dialog.Msg(L("I'll try and enchant a piece of wood to look like someone injured. The wood is going to look like someone who's hurt to whoever looks at it."));

			var answer = await dialog.SelectQuestOffer(Sq03, L("Place it close to the ferrets and see if they attack."),
				Option(L("I'll go now"), "accept"),
				Option(L("I can only help so much"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq03);
				GiveItem(character, ItemId.ORCHARD_342_SQ_03_WOOD);
			}
			return;
		}

		if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
		{
			await dialog.Msg(L("There's no doubt after this experiment. The ferrets are very, very dangerous now."));
			await dialog.Msg(L("Still, there's no way I can fight all those ferrets. The most I can do is chase away the ones closest to the village."));
			await dialog.Msg(L("There's a type of strong scent that ferrets hate. If we make a fire and burn this scent, that should keep the ferrets away."));

			var answer = await dialog.SelectQuestOffer(Sq04, L("Would you spread this scent around the Bellai Forest Workshop? It smells a bit... foul, but please do me this favor."),
				Option(L("I will certainly help"), "accept"),
				Option(L("I can only help so much"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq04);
				GiveItem(character, ItemId.ORCHARD_342_SQ_04_ITEM);
			}
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			GiveItem(character, ItemId.ORCHARD_342_SQ_01_SCROLL);
			await dialog.Msg(L("I'm going to make a transformation scroll from the information we collect. Write everything down. You can't act too awkward as a ferret."));
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			GiveItem(character, ItemId.ORCHARD_342_SQ_02_TRANSFORM);
			await dialog.Msg(L("If the scroll wears out in front of the ferrets they'll come at you, so be careful."));
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			GiveItem(character, ItemId.ORCHARD_342_SQ_03_WOOD);
			await dialog.Msg(L("Will they attack someone if they're injured? Nah... If they do, it's even more serious than I thought..."));
			return;
		}

		if (character.Quests.IsActive(Sq04))
		{
			GiveItem(character, ItemId.ORCHARD_342_SQ_04_ITEM);
			await dialog.Msg(L("I should make more of the scent. In case the effect wears out."));
			return;
		}

		if (character.Quests.HasCompleted(Seir324Mq01))
		{
			await dialog.Msg(L("Finally that Demon Lord was defeated... And Goddess Lada was rescued... Thank you."));
			await dialog.Msg(L("Thanks to you now we can focus on the ferrets. I still don't know about the red water, but we'll find an answer eventually."));
			return;
		}

		switch (GameRandom.Get().Next(3))
		{
			case 0: await dialog.Msg(L("Huh, an outsider? You must be new here... Those ferrets may look cute, but they'll shred your face to pieces if you're not careful.")); break;
			case 1: await dialog.Msg(L("I don't know what the demons did to those ferrets but... Seeing how they attack, avoiding a fight must be impossible, right?")); break;
			default: await dialog.Msg(L("Those ferrets, they weren't like this... If we're being honest, this was all our fault though.")); break;
		}
	}

	/// <summary>
	/// The girl the ferrets tied up at their hideout.
	/// </summary>
	private async Task TiedGirl(Dialog dialog)
	{
		var character = dialog.Player;
		if (!character.Quests.IsActive(Mq05) || character.Quests.IsCompletable(Mq05))
			return;

		if (!character.Variables.Perm.GetBool(IncenseVar, false))
		{
			character.ServerMessage(L("The ferrets guarding the girl are too alert. Throw the bag of incense first."));
			return;
		}

		dialog.Npc.PlayEffect("F_buff_basic025_white_line", 1f, 1, EffectLocation.Bottom);
		character.Quests.CompleteObjective(Mq05, "rescueGirl");

		await Task.CompletedTask;
	}

	/// <summary>
	/// The mysterious girl by the great statue of Goddess Laima.
	/// </summary>
	private async Task Girl(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mysterious Girl"));

		if (character.Quests.IsCompletable(Mq06))
		{
			await dialog.Msg(L("The girl is looking at the tree that is shining mysteriously."));
			await dialog.CompleteQuest(Mq06);
			return;
		}

		if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
		{
			var answer = await dialog.SelectQuestOffer(Mq07, L("Here..."),
				Option(L("Let's have a look at the girl's sapling"), "accept"),
				Option(L("Let's wait and see what happens"), "leave")
			);

			if (answer == "accept")
			{
				dialog.Npc.PlayEffect("F_buff_basic025_white_line", 1f, 1, EffectLocation.Bottom);
				character.Quests.Start(Mq07);
				character.LookAround();
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The girl disappeared with the light."), 5);
			}
			return;
		}

		if (character.Quests.IsActive(Mq06))
		{
			character.ServerMessage(L("The girl stared at me as if she wanted me to follow her."));
			return;
		}

		await dialog.Msg(L("Here..."));
	}

	/// <summary>
	/// The shiny sapling beneath the great statue of Goddess Laima.
	/// </summary>
	private async Task Sapling(Dialog dialog)
	{
		var character = dialog.Player;
		if (!character.Quests.IsActive(Mq07) || character.Quests.IsCompletable(Mq07))
			return;

		dialog.Npc.PlayEffect("F_buff_basic025_white_line", 1f, 1, EffectLocation.Bottom);
		character.Quests.CompleteObjective(Mq07, "investigateSapling");

		await Task.CompletedTask;
	}

	/// <summary>
	/// Goddess Laima's message, left in her orb under the sapling.
	/// </summary>
	private async Task GoddessOrb(Dialog dialog)
	{
		var character = dialog.Player;
		if (!character.Quests.IsCompletable(Mq08))
			return;

		dialog.SetTitle(L("Goddess Laima"));
		dialog.SetPortrait("Dlg_port_Raima");

		await dialog.Msg(L("I hope my words can reach you... I am Laima, goddess of destiny and premonition."));
		await dialog.Msg(L("Before Medzio Diena, the goddesses lost much of their power. Because of this, Goddess Lada, unable to resist, was taken away by Demon Lord Zaura."));
		await dialog.Msg(L("Zaura is making something evil called Kruvina using Goddess Lada's vital force. The same Kruvina the people of Delmore Castle were sacrificed to make."));
		await dialog.Msg(L("The evil energy of Kruvina can make objects grow. Depending on how it is used, it can enhance the power of the demons, too."));
		await dialog.Msg(L("By the time I had a vision of this horrible future, it was too late to do anything. That's why... I sent a young girl to meet you."));
		await dialog.Msg(L("Only one person can free this forest from the calamity that fell upon it... A savior among the Revelators. You."));
		await dialog.Msg(L("Take this orb... Goddess Lada is being held captive at the Seir Rainforest. Please, rescue her and stop the demons' plans..."));
		await dialog.CompleteQuest(Mq08);

		if (!character.Quests.HasCompleted(Mq08))
			return;

		character.PlayEffect("F_buff_basic025_white_line", 1f);

		if (!character.Quests.Has(Seir324Mq01) && character.Quests.MeetsPrerequisites(Seir324Mq01))
			character.Quests.Start(Seir324Mq01);

		character.LookAround();
	}

	/// <summary>
	/// Hands the character one of Benes' quest items, unless they still
	/// carry it.
	/// </summary>
	private static void GiveItem(Character character, int itemId)
	{
		if (character.Inventory.CountItem(itemId) == 0)
			character.Inventory.Add(itemId, 1, InventoryAddType.PickUp);
	}

	/// <summary>
	/// Hands the character a fresh bag of Leja's Drowsy Herb incense.
	/// </summary>
	private static void GiveIncense(Character character)
		=> GiveItem(character, ItemId.ORCHARD_342_MQ_05_ITEM);

	/// <summary>
	/// Returns the closest ferret the character can talk to or observe.
	/// </summary>
	private static Mob GetNearbyFerret(Character character)
	{
		return character.Map.GetAttackableEnemiesInPosition(character, character.Position, FerretRange)
			.OfType<Mob>()
			.FirstOrDefault(mob => Ferrets.Contains(mob.Data.ClassName));
	}

	/// <summary>
	/// Returns false while one of Benes' scrolls is still recovering from
	/// its last use.
	/// </summary>
	private static bool TryUseScroll(Character character)
	{
		var usedAt = character.Variables.Temp.GetLong(ScrollUsedVar, 0);
		if (usedAt != 0 && DateTime.Now - new DateTime(usedAt) < ScrollCooldown)
		{
			character.ServerMessage(L("You need to wait a moment before trying again."));
			return false;
		}

		character.Variables.Temp.SetLong(ScrollUsedVar, DateTime.Now.Ticks);
		return true;
	}

	/// <summary>
	/// Bursts a bag of Leja's Drowsy Herb incense, dazing the ferrets
	/// around.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_342_MQ_05_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_34_2" || !character.Quests.IsActive(Mq05) || character.Quests.IsCompletable(Mq05))
		{
			character.ServerMessage(L("There is no reason to use the incense here."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.PlayEffect("F_smoke017_red", 1f);

		var uses = character.Variables.Perm.GetInt(IncenseUsesVar, 0) + 1;
		character.Variables.Perm.SetInt(IncenseUsesVar, uses);

		if (character.Position.Get2DDistance(TiedGirlSpot) <= GirlRange)
		{
			character.Variables.Perm.Set(IncenseVar, true);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The ferrets have lost their minds to the scent! Rescue the girl while you can."), 5);
		}
		else
		{
			character.ServerMessage(L("The scent spreads, but the girl is nowhere near."));
		}

		if (uses >= IncenseUses)
			character.Inventory.Remove(ItemId.ORCHARD_342_MQ_05_ITEM, 1, InventoryItemRemoveMsg.Given);

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Takes notes on the behavior of a nearby ferret.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_342_SQ_01_SCROLL(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_34_2" || !character.Quests.IsActive(Sq01) || character.Quests.IsCompletable(Sq01))
		{
			character.ServerMessage(L("There is nothing to write down."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (GetNearbyFerret(character) == null)
		{
			character.ServerMessage(L("There are no ferrets nearby to observe."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (!TryUseScroll(character))
			return ItemUseResult.OkayNotConsumed;

		var notes = character.Variables.Perm.GetInt(NotesVar, 0);

		switch (notes)
		{
			case 0: character.ServerMessage(L("The ferret plays dead in critical situations.")); break;
			case 1: character.ServerMessage(L("The ferrets are curious animals and eats whatever they have yet to see.")); break;
			default: character.ServerMessage(L("The ferrets are nocturnal and seems to sleep a lot during the day.")); break;
		}

		character.Variables.Perm.SetInt(NotesVar, Math.Min(NotesNeeded, notes + 1));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Turns into a ferret and talks to a nearby one.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_342_SQ_02_TRANSFORM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_34_2" || !character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02))
		{
			character.ServerMessage(L("There is no one to talk to here."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (GetNearbyFerret(character) == null)
		{
			character.ServerMessage(L("This ferret doesn't hold a single interest to converse with me."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (!TryUseScroll(character))
			return ItemUseResult.OkayNotConsumed;

		character.PlayEffect("F_smoke017_red", 1f);

		var talks = character.Variables.Perm.GetInt(TalksVar, 0);

		switch (talks)
		{
			case 0: character.ServerMessage(L("The ferret became stronger by touching the totem. Ferrets! Win against humans!")); break;
			case 1: character.ServerMessage(L("The fruit has become bigger! Run away coward humans! No human qualifications! Ferrets will scare them away!")); break;
			default: character.ServerMessage(L("Demons will help the ferrets. Make us stronger! Demons said to protect. Humans! Cannot come closer! Then demons. Continue to help the ferrets!")); break;
		}

		character.Variables.Perm.SetInt(TalksVar, Math.Min(TalksNeeded, talks + 1));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Sets down the voodooed piece of wood near the ferrets.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_342_SQ_03_WOOD(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_34_2" || !character.Quests.IsActive(Sq03) || character.Quests.IsCompletable(Sq03))
		{
			character.ServerMessage(L("There is no reason to set the wooden piece down here."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Position.Get2DDistance(WoodSpot) > WoodRange)
		{
			character.ServerMessage(L("There are no ferrets around to watch."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.PlayEffect("F_light018_yellow", 1f);
		character.Quests.CompleteObjective(Sq03, "placeWood");
		character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The ferrets attacked the wooden piece shaped like an injured villager!"), 3);

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Burns Benes' terrible scent at the entrance to the Bellai Forest
	/// Workshop.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ORCHARD_342_SQ_04_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_orchard_32_3" || !character.Quests.IsActive(Sq04) || character.Quests.IsCompletable(Sq04))
		{
			character.ServerMessage(L("The scent should be burnt around the Bellai Forest Workshop."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Position.Get2DDistance(WorkshopEntrance) > WorkshopRange)
		{
			character.ServerMessage(L("This is too far from the entrance of the Bellai Forest Workshop."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.PlayEffect("F_smoke017_red", 1f);
		character.Quests.CompleteObjective(Sq04, "burnScent");
		character.ServerMessage(L("The ferrets run off from the terrible scent."));

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Returns whether the statue of Goddess Laima is calling out to the
	/// character.
	/// </summary>
	private static bool IsCrystalCalling(Character character)
		=> character.Quests.IsActive(Seir324Mq07) && !character.Quests.IsCompletable(Seir324Mq07);

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

// 80029: The Guiding Girl (1)
//-----------------------------------------------------------------------------
public class FOrchard342Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80029);
		SetName(L("The Guiding Girl (1)"));
		SetDescription(L("The girl left visible traces. Follow her trace to find her."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_32_3", "f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_MQ_01_TRIG", "f_orchard_34_2", L("Look for the traces of the mysterious girl"), L("Follow the girl in the direction she's headed."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_MQ_01_TRIG", "f_orchard_34_2", L("Look for the traces of the mysterious girl"), L("The girl left visible traces. Follow her trace to find her."));
		SetPhase(QuestStatus.Success, "ORCHARD342_LEJA", "f_orchard_34_2", L("Follow the Girl"), L("The girl seems to be telling you to follow her downwards. Keep following her."));

		AddPrerequisite(new QuestStatusPrerequisite(80023, QuestStatus.Completed));

		AddObjective("followTrace", L("Look for the traces of the mysterious girl"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("Vis", 1140));
	}
}

// 80030: The Guiding Girl (2)
//-----------------------------------------------------------------------------
public class FOrchard342Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80030);
		SetName(L("The Guiding Girl (2)"));
		SetDescription(L("Druid Leja says the girl was being chased by ferrets. Defeat the ferrets and rescue the girl."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_LEJA", "f_orchard_34_2", L("Talk to Druid Leja"), L("While chasing the girl, you met Druid Leja. Ask Leja about the girl's whereabouts."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_LEJA", "f_orchard_34_2", L("Defeat the ferrets and find a trace of the girl"), L("Druid Leja says the girl was being chased by ferrets. Defeat the ferrets and rescue the girl."));
		SetPhase(QuestStatus.Success, "ORCHARD342_LEJA", "f_orchard_34_2", L("Defeat the ferrets and find a trace of the girl"), L("Druid Leja says the girl was being chased by ferrets. Defeat the ferrets and rescue the girl."));

		AddPrerequisite(new QuestStatusPrerequisite(80029, QuestStatus.Completed));

		AddObjective("killFerrets", L("Defeat the ferrets and find a trace of the girl"), new KillObjective(6, "ferret_folk", "ferret_loader"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself; the strange sensation leads on to the girl.
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(80031));
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("While defeating ferrets, you got that strange sensation again. Follow that sensation, it will lead you to the girl."), 5);
	}
}

// 80031: The Missing Girl (1)
//-----------------------------------------------------------------------------
public class FOrchard342Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80031);
		SetName(L("The Missing Girl (1)"));
		SetDescription(L("While defeating ferrets, you got that strange sensation again. Follow that sensation, it will lead you to the girl."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD_342_MQ_03_MINI", "f_orchard_34_2", L("Find the traces of the Mysterious Girl"), L("While defeating ferrets, you got that strange sensation again. Follow that sensation, it will lead you to the girl."));
		SetPhase(QuestStatus.InProgress, "ORCHARD_342_MQ_03_MINI", "f_orchard_34_2", L("Find the traces of the Mysterious Girl"), L("While defeating ferrets, you got that strange sensation again. Follow that sensation, it will lead you to the girl."));
		SetPhase(QuestStatus.Success, "ORCHARD342_LEJA", "f_orchard_34_2", L("Talk to Druid Leja"), L("The mysterious girl was taken by Demon Lord Zaura. Discuss this with Druid Leja."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ORCHARD_342_MQ_03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(80030, QuestStatus.Completed));

		AddObjective("killFerrets", L("Defeat the ferrets"), new KillObjective(20, "ferret_folk", "ferret_loader") { LayerOnly = true });

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1710));
	}
}

// 80032: The Missing Girl (2)
//-----------------------------------------------------------------------------
public class FOrchard342Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80032);
		SetName(L("The Missing Girl (2)"));
		SetDescription(L("Druid Leja believes the ferrets are hiding the girl. She asks you to bring some Drowsy Herbs for her to make an incense to use on the ferrets."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_LEJA", "f_orchard_34_2", L("Talk to Druid Leja"), L("The Demon Lord took the girl and disappeared. Discuss with Druid Leja about what to do next."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_SLEEP", "f_orchard_34_2", L("Collect the herbs"), L("Druid Leja believes the ferrets are hiding the girl. She asks you to bring some Drowsy Herbs for her to make an incense to use on the ferrets."));
		SetPhase(QuestStatus.Success, "ORCHARD342_LEJA", "f_orchard_34_2", L("Deliver the Drowsy Herbs to Druid Leja"), L("Acquired all of the herbs Druid Leja requested from you. Pass them to Druid Leja."));

		AddPrerequisite(new QuestStatusPrerequisite(80031, QuestStatus.Completed));

		AddObjective("collectHerbs", L("Gather Drowsy Herbs"), new CollectItemObjective("ORCHARD_342_MQ_04_ITEM", 6));

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new ItemReward("Vis", 1140));
		AddReward(new TakeItemReward("ORCHARD_342_MQ_04_ITEM", -1));
	}
}

// 80033: The Missing Girl (3)
//-----------------------------------------------------------------------------
public class FOrchard342Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80033);
		SetName(L("The Missing Girl (3)"));
		SetDescription(L("The girl kidnapped by the ferrets needs to be rescued. Throw the bag of incense to leave the ferrets disoriented, then take your chance and rescue the girl."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_LEJA", "f_orchard_34_2", L("Talk to Druid Leja"), L("Druid Leja has created an incense able to put the ferrets in a trance. Ask Druid Leja about the possible location of the girl and the ferrets."));
		SetPhase(QuestStatus.InProgress, "ORCHARD42_BINDIG_GIRL", "f_orchard_34_2", L("Rescue the Kidnapped Girl from the Ferret Hideout"), L("The girl kidnapped by the ferrets needs to be rescued. Throw the bag of incense to leave the ferrets disoriented, then take your chance and rescue the girl."));
		SetPhase(QuestStatus.Success, "ORCHARD42_BINDIG_GIRL", "f_orchard_34_2", L("Rescue the Kidnapped Girl from the Ferret Hideout"), L("The girl kidnapped by the ferrets needs to be rescued. Burst the bag of incense to leave the ferrets disoriented, then take your chance and rescue the girl."));

		AddPrerequisite(new QuestStatusPrerequisite(80032, QuestStatus.Completed));

		AddObjective("rescueGirl", L("Rescue the Kidnapped Girl from the Ferret Hideout"), new ManualObjective());

		AddReward(new TakeItemReward("ORCHARD_342_MQ_05_ITEM", -1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself; the freed girl runs off towards the statue.
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(80034));
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The freed girl has ran away somewhere with urgency."), 5);
		character.LookAround();
	}
}

// 80034: The Goddess' Assignment (1)
//-----------------------------------------------------------------------------
public class FOrchard342Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80034);
		SetName(L("The Goddess' Assignment (1)"));
		SetDescription(L("The mysterious girl started running as soon as you freed her. Follow her trace to where she is."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_MQ_06_TRIG", "f_orchard_34_2", L("Find the Mysterious Girl"), L("The mysterious girl started running as soon as you freed her. Follow her trace to where she is."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_MQ_06_TRIG", "f_orchard_34_2", L("Find the Mysterious Girl"), L("The mysterious girl started running as soon as you freed her. Follow her trace to where she is."));
		SetPhase(QuestStatus.Success, "ORCHARD342_GIRL", "f_orchard_34_2", L("Talk to the Mysterious Girl"), L("You have found the girl by the great Goddess Statue. Talk to the mysterious girl."));

		AddPrerequisite(new QuestStatusPrerequisite(80033, QuestStatus.Completed));

		AddObjective("findGirl", L("Find the Mysterious Girl"), new ManualObjective());
	}
}

// 80035: The Goddess' Assignment (2)
//-----------------------------------------------------------------------------
public class FOrchard342Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80035);
		SetName(L("The Goddess' Assignment (2)"));
		SetDescription(L("The mysterious girl is pointing to a shiny sapling. Have a look at the sapling."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_GIRL", "f_orchard_34_2", L("Talk to the Mysterious Girl"), L("The mysterious girl is pointing to a shiny sapling. Talk to the mysterious girl."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_TREE", "f_orchard_34_2", L("Investigate the Shiny Sapling"), L("The mysterious girl is pointing to a shiny sapling. Have a look at the sapling."));
		SetPhase(QuestStatus.Success, "ORCHARD342_TREE", "f_orchard_34_2", L("Investigate the Shiny Sapling"), L("The mysterious girl is pointing to a shiny sapling. Have a look at the sapling."));

		AddPrerequisite(new QuestStatusPrerequisite(80034, QuestStatus.Completed));

		AddObjective("investigateSapling", L("Investigate the Shiny Sapling"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself; the goddess' orb lies under the sapling.
		var orb = new QuestId(80036);

		character.Quests.Complete(this.QuestId);
		character.Quests.Start(orb);
		character.Quests.CompleteObjective(orb, "examineOrb");
		character.LookAround();
	}
}

// 80036: The Goddess' Assignment (3)
//-----------------------------------------------------------------------------
public class FOrchard342Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80036);
		SetName(L("The Goddess' Assignment (3)"));
		SetDescription(L("Found the Goddess' Orb under the mysterious seedling. Examine the Goddess' Orb."));
		SetType(QuestType.Main);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_HOLY_TALK", "f_orchard_34_2", L("Examine the Goddess' Orb"), L("Found the Goddess' Orb under the mysterious seedling. Examine the Goddess' Orb."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_HOLY_TALK", "f_orchard_34_2", L("Examine the Goddess' Orb"), L("Found the Goddess' Orb under the mysterious seedling. Examine the Goddess' Orb."));
		SetPhase(QuestStatus.Success, "ORCHARD342_HOLY_TALK", "f_orchard_34_2", L("Examine the Goddess' Orb"), L("Found the Goddess' Orb under the mysterious seedling. Examine the Goddess' Orb."));

		AddPrerequisite(new QuestStatusPrerequisite(80035, QuestStatus.Completed));

		AddObjective("examineOrb", L("Examine the Goddess' Orb"), new ManualObjective());

		AddReward(new ItemReward("ORCHARD_342_MQ_HOLLY_SPHERE", 1));
	}
}

// 80037: Recording the Behavior
//-----------------------------------------------------------------------------
public class FOrchard342Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80037);
		SetName(L("Recording the Behavior"));
		SetDescription(L("Druid Benes has asked you to observe the behavior patterns of the ferrets for him. Use an empty scroll arround the ferrets to take notes."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_BENES", "f_orchard_34_2", L("Speak with Druid Benes"), L("Druid Benes is looking for someone to help with his research. Talk to Druid Benes."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_BENES", "f_orchard_34_2", L("Record the behaviors of the ferrets"), L("Druid Benes has asked you to observe the behavior patterns of the ferrets for him. Use an empty scroll arround the ferrets to take notes."));
		SetPhase(QuestStatus.Success, "ORCHARD342_BENES", "f_orchard_34_2", L("Deliver the Scroll to Druid Benes"), L("You have written down the behavior patterns of the ferrets on the blank scroll. Bring it to Benes."));

		AddPrerequisite(new LevelPrerequisite(86));

		AddObjective("recordBehavior", L("Record the behaviors of the ferrets"), new VariableCheckObjective(FOrchard342QuestNpcsScript.NotesVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1720));
		AddReward(new TakeItemReward("ORCHARD_342_SQ_01_SCROLL", -1));
	}
}

// 80038: The Blurred Barrier Between The Thing and I
//-----------------------------------------------------------------------------
public class FOrchard342Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80038);
		SetName(L("The Blurred Barrier Between The Thing and I"));
		SetDescription(L("The ferret transformation scroll given by Druid Benes allows you to turn into a ferret. Transform into one and find out why they have sided with the demons and are attacking humans."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_BENES", "f_orchard_34_2", L("Speak with Druid Benes"), L("You've written the behaviors of the ferrets on the empty scroll. Speak with Benes."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_BENES", "f_orchard_34_2", L("Understanding the ferrets"), L("The ferret transformation scroll given by Druid Benes allows you to turn into a ferret. Transform into one and find out why they have sided with the demons and are attacking humans."));
		SetPhase(QuestStatus.Success, "ORCHARD342_BENES", "f_orchard_34_2", L("Speak with Druid Benes"), L("The reason ferrets have started to attack humans is because the demons have made them stronger. Tell Druid Benes about this."));

		AddPrerequisite(new QuestStatusPrerequisite(80037, QuestStatus.Completed));

		AddObjective("talkToFerrets", L("Understanding the ferrets"), new VariableCheckObjective(FOrchard342QuestNpcsScript.TalksVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1720));
		AddReward(new TakeItemReward("ORCHARD_342_SQ_02_TRANSFORM", -1));
	}
}

// 80039: Unbelievable Reality (1)
//-----------------------------------------------------------------------------
public class FOrchard342Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80039);
		SetName(L("Unbelievable Reality (1)"));
		SetDescription(L("Druid Benes has asked you to set up the voodooed piece of wood shaped like an injured villager and watch the ferrets' reaction to it."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_BENES", "f_orchard_34_2", L("Speak with Druid Benes"), L("You've transformed into a ferret and talked with them as one of them. Report their true reason to Druid Benes."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_BENES", "f_orchard_34_2", L("Place the Voodooed Wooden Piece and observe the ferrets' reaction"), L("Druid Benes has asked you to set up the voodooed piece of wood shaped like an injured villager and watch the ferrets' reaction to it."));
		SetPhase(QuestStatus.Success, "ORCHARD342_BENES", "f_orchard_34_2", L("Speak with Druid Benes"), L("The ferrets attacked the wooden block shaped like an injured townfolk. Report back to Druid Benes."));

		AddPrerequisite(new QuestStatusPrerequisite(80038, QuestStatus.Completed));

		AddObjective("placeWood", L("Place the Voodooed Wooden Piece and observe the ferrets' reaction"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1720));
		AddReward(new TakeItemReward("ORCHARD_342_SQ_03_WOOD", -1));
	}
}

// 80040: Unbelievable Reality (2)
//-----------------------------------------------------------------------------
public class FOrchard342Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(80040);
		SetName(L("Unbelievable Reality (2)"));
		SetDescription(L("Benes has asked you to spread a ferret-repelling scent around the entrance to the Bellai Forest Workshop."));
		SetType(QuestType.Sub);
		SetLocation("f_orchard_34_2", "f_orchard_32_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ORCHARD342_BENES", "f_orchard_34_2", L("Speak with Druid Benes"), L("The ferrets seem to attack all humans indiscriminately. Ask Druid Benes about what to do next."));
		SetPhase(QuestStatus.InProgress, "ORCHARD342_BENES", "f_orchard_34_2", L("Chase away the ferrets around the Bellai Forest Workshop with the repellent"), L("Benes has asked you to spread a ferret-repelling scent around the entrance to the Bellai Forest Workshop."));
		SetPhase(QuestStatus.Success, "ORCHARD342_BENES", "f_orchard_34_2", L("Report to Druid Benes"), L("You have chased away the ferrets with a repellent. Return to Druid Benes."));

		AddPrerequisite(new QuestStatusPrerequisite(80039, QuestStatus.Completed));

		AddObjective("burnScent", L("Chase away the ferrets around the Bellai Forest Workshop with the repellent"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 3));
		AddReward(new ItemReward("Vis", 1725));
		AddReward(new TakeItemReward("ORCHARD_342_SQ_04_ITEM", -1));
	}
}
