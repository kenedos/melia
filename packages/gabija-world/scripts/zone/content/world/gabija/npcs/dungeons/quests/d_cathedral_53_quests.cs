//--- Melia Script ----------------------------------------------------------
// Great Cathedral Main Chamber Quest NPCs
//--- Description -----------------------------------------------------------
// The bishop who has waited hundreds of years for a Revelator, the vessel
// his spirit is poured into, and the first of Maven's five keys.
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

public class DCathedral53QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20300);
	private readonly static QuestId Mq02 = new QuestId(20301);
	private readonly static QuestId Mq03 = new QuestId(20302);
	private readonly static QuestId Mq04 = new QuestId(20303);
	private readonly static QuestId Mq05 = new QuestId(20304);
	private readonly static QuestId Mq06 = new QuestId(20305);
	private readonly static QuestId Sq01 = new QuestId(20306);
	private readonly static QuestId Sq02 = new QuestId(20307);
	private readonly static QuestId Sq03 = new QuestId(20308);
	private readonly static QuestId Sq05 = new QuestId(50000);
	private readonly static QuestId Sq06 = new QuestId(50001);

	private const int SpiritEssencesNeeded = 4;
	private const int ScripturalRelicsNeeded = 8;
	private const int GracefulRelicsNeeded = 5;

	private const string EssenceVar = "Gabija.Cathedral53.Essence";
	private const string RelicVar = "Gabija.Cathedral53.Relic";
	private const string HiddenRelicVar = "Gabija.Cathedral53.HiddenRelic";
	private const string AltarVar = "Gabija.Cathedral53.Altar";

	/// <summary>
	/// Dialog of the Meile Oratorium platform, on the map and inside its puzzle track.
	/// </summary>
	/// <param name="dialog"></param>
	/// <returns></returns>
	public static async Task MeilePlatformDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Writing on the Platform"));

		if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("A drop-shaped jewel has come out of the device."));
			await dialog.CompleteQuest(Mq04);
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("Remember the holy number with your two eyes."));
			await dialog.Msg(L("The light of candles drives away the evil dark."));

			var lit = await character.TimeActions.StartAsync(L("Lighting the candles..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

			if (lit != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Mq04, "solveSecret");
			character.ServerMessage(L("The candles of the Meile Oratorium burn in the right number."));
			return;
		}

		await dialog.Msg(L("Remember the holy number with your two eyes."));
		await dialog.Msg(L("The light of candles drives away the evil dark."));
	}

	protected override void Load()
	{
		// Bishop Aurelius' Spirit, waiting at the chancel
		//-------------------------------------------------------------------------
		AddConditionalNpc(151033, L("Bishop Aurelius' Spirit"), "CHATHEDRAL53_MQ_BISHOP", "d_cathedral_53", 82.87, -1004.60, 335, c => !c.Quests.HasCompleted(Mq03), this.Bishop);

		// Bishop Aurelius, called up out of the Spirit's Scripture
		//-------------------------------------------------------------------------
		// The client summons him from the item; the port stands him by the
		// Altar of Stability, where the scripture was completed.
		AddConditionalNpc(151033, L("Bishop Aurelius' Spirit"), "CHATHEDRAL_BISHOP", "d_cathedral_53", -546.36, -180, 89, c => c.Quests.HasCompleted(Mq02) && !c.Quests.HasCompleted(Mq06), this.SummonedBishop);

		// Spirit Essence
		//-------------------------------------------------------------------------
		this.AddSpiritEssence(1, 61.88, -740.86);
		this.AddSpiritEssence(2, 351.24, -791.75);
		this.AddSpiritEssence(3, -69.02, -493.37);
		this.AddSpiritEssence(4, -306.24, -836.98);
		this.AddSpiritEssence(5, 294.53, -534.66);
		this.AddSpiritEssence(6, -270.25, -625.88);

		// The scriptures of Pamaldu Groom
		//-------------------------------------------------------------------------
		this.AddScripture(1, L("Scripture of Prophecy"), -769.20, 223, 44);
		this.AddScripture(2, L("Scripture of the Earth"), -662.35, -53.83, 150);
		this.AddScripture(3, L("Scripture of Rest"), -995.45, 21.42, 91);
		this.AddScripture(4, L("Scripture of Eternity"), -572.85, -326.47, 44);
		this.AddScripture(5, L("Scripture of Purification"), -913.91, -359.43, 7);
		this.AddScripture(6, L("Scripture of Praise"), -456.20, 174.36, 350);

		// Altar of Stability
		//-------------------------------------------------------------------------
		AddNpc(147417, L("Altar of Stability"), "CHATHEDRAL53_MQ03", "d_cathedral_53", -546.36, -49.55, 89, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Altar of Stability"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				var poured = await character.TimeActions.StartAsync(L("Pouring the Spirit Essence into the scripture..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (poured != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The essence settles into the pages and the scripture closes itself."));
				await dialog.CompleteQuest(Mq02);
				character.ServerMessage(L("The Spirit's Scripture is complete."));
				character.LookAround();
				return;
			}

			await dialog.Msg(L("The Altar of Stability. Whatever is set here is held together."));
		});

		// Meile Oratorium Platform
		//-------------------------------------------------------------------------
		AddNpc(153023, L("Meile Oratorium Platform"), "CHATHEDRAL53_MQ04_HINT", "d_cathedral_53", -2363.94, -51.86, 47, MeilePlatformDialog);

		// The candle puzzle is staged when the platform is reached.
		AddQuestTrigger("CHATHEDRAL53_MQ04_ARRIVE", "d_cathedral_53", -2200, -52, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
				character.Quests.StartQuestTrack(Mq04);

			await Task.CompletedTask;
		});

		// The two old altars of the Small Hall
		//-------------------------------------------------------------------------
		this.AddOldAltar(1, 862.74, -1444.78, L("Everlasting goddess."), L("May we praise your infinite mercy."));
		this.AddOldAltar(2, 863.64, -1351.71, L("Life withers and the soul has nowhere to go."), L("Only the goddess can save us."));

		// Maven's Secret
		//-------------------------------------------------------------------------
		AddNpc(153018, L("Maven's Secret"), "CHATHEDRAL53_MQ06_PUZZLE", "d_cathedral_53", 830.83, -1398.16, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Maven's Secret"));

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				var checkedDevice = await character.TimeActions.StartAsync(L("Checking..."), L("Cancel"), "SITGROPESET2", TimeSpan.FromSeconds(2));

				if (checkedDevice != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The device opens and the first of Maven's keys lies inside."));
				await dialog.CompleteQuest(Mq06);
				character.ServerMessage(L("Acquired Maven's key!"));
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The device is shut. The altars of the Small Hall have not been read."));
				return;
			}

			await dialog.Msg(L("A machine of Maven's, sealed and silent."));
		});

		// Priest Inea
		//-------------------------------------------------------------------------
		AddNpc(147386, L("Priest Inea"), "CHATHEDRAL53_SQ01", "d_cathedral_53", 212.48, -128.79, 146, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Inea"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Fortunately, most of the holy relics seem okay."));
				await dialog.Msg(L("If I had a little more power, I would have purified the demons in the name of the goddesses right away."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Yes, good work."));
				await dialog.Msg(L("We've found the important things so we can ask the sisters for the other things."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("There was a sect to move all the relics to somewhere safe. But look at how horrible things have turned out. The Scriptural Relics contained the words of the great bishops, and I am so infuriated that they are placed now in the wake of filthy demons."),
					Option(L("I will retrieve the Relics"), "accept"),
					Option(L("I'm not interested"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= ScripturalRelicsNeeded; ++i)
						character.Variables.Perm.Set(RelicVar + i, false);

					character.Quests.Start(Sq01);
					await dialog.Msg(L("They are strewn over the floor of the Great Cathedral. Bring back every one you find."));
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Some relics hide themselves when they feel an evil force. When that happens, you just can't find it no matter how hard you try. To prepare for times like this, I've brought an orb that can detect holy energy."),
					Option(L("Tell her not to worry and trust you"), "accept"),
					Option(L("Better give up as it's dangerous"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= GracefulRelicsNeeded; ++i)
						character.Variables.Perm.Set(HiddenRelicVar + i, false);

					character.Quests.Start(Sq02);
					character.Inventory.Add(ItemId.CATHEDRAL53_SQ02_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Keep an eye on the demons though. You may be in for some trouble, since they will run towards the relics as soon as they see them."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("It's so brutal."));
				await dialog.Msg(L("Our Great Cathedral, to those demons..."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("How come the goddesses leave those filthy demons like that..."));
				await dialog.Msg(L("Is this also what the goddesses want?"));
				return;
			}

			await dialog.Msg(L("I knew that the Great Cathedral would still be standing, even on Medzio Diena."));
			await dialog.Msg(L("But, Naktis' attacks were not something that could be endured."));
		});

		// The Scriptural Relics on the cathedral floor
		//-------------------------------------------------------------------------
		this.AddScripturalRelic(1, 100.45, 632.29);
		this.AddScripturalRelic(2, -192.57, 788.02);
		this.AddScripturalRelic(3, -221.08, 403.59);
		this.AddScripturalRelic(4, -376.21, 907.56);
		this.AddScripturalRelic(5, 444.77, 890.65);
		this.AddScripturalRelic(6, 150.18, 1039.97);
		this.AddScripturalRelic(7, 126.76, 338.18);
		this.AddScripturalRelic(8, 381.40, 464.02);

		// The Graceful Relics the orb uncovers
		//-------------------------------------------------------------------------
		// The client keeps these as hidden triggers the Orb of Divine Detection
		// reveals; the port places the relics themselves on those spots.
		this.AddHiddenRelic(1, 753.93, -88.17);
		this.AddHiddenRelic(2, 786.18, -181.80);
		this.AddHiddenRelic(3, 705.11, 210.50);
		this.AddHiddenRelic(4, 1027.64, -165.88);
		this.AddHiddenRelic(5, 990.39, 181.33);

		// Priest Aden
		//-------------------------------------------------------------------------
		AddNpc(147398, L("Priest Aden"), "CHATHEDRAL53_SQ03", "d_cathedral_53", 106.28, -109.52, 128, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Aden"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("If we've got the reports, even Naktis' curse will just be as if it were a mere spell."));
				await dialog.Msg(L("Yup, that's right. Great work."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("This Great Cathedral is like a repository of knowledge. Of course, that should mean that we can find how to release Naktis' curses. I'm roughly done studying this area, but we need the reports of clerics from other areas."),
					Option(L("I will retrieve the research reports"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("The other clerics are at the Grand Corridor, Penitence Route, and the Sanctuary."));
					await dialog.Msg(L("Research is important but I hope they are safe. If you see the other clerics, please send them my regards."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Yosana at the Grand Corridor, Gadan at the Penitence Route, Prosit at the Sanctuary."));
				return;
			}

			await dialog.Msg(L("The Great Cathedral is like a repository of knowledge."));
			await dialog.Msg(L("If you find those documents, we will easily find a way how to face against Naktis."));
		});

		// Priest Benedict
		//-------------------------------------------------------------------------
		AddNpc(147389, L("Priest Benedict"), "CHATHEDRAL53_SQ02", "d_cathedral_53", 813.66, -487.38, 148, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Benedict"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("It would have been nice if the Church sent us enough people."));
				await dialog.Msg(L("Oh, I better test the samples you brought."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (character.Quests.IsActive(Sq06) && character.Quests.IsCompletable(Sq06))
			{
				await dialog.Msg(L("Ah. It definitely reacted this time."));
				await dialog.Msg(L("I would have spent a long time without your help. Thank you so much."));
				await dialog.CompleteQuest(Sq06);
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("I am investigating on how to undo Naktis' curse here. For the sake of those who are suffering from the curse, can you get me samples of the demons?"),
					Option(L("I can get them"), "accept"),
					Option(L("I'm not interested"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("The Coliflies will do. Twelve samples should be enough to work with."));
				}
				return;
			}

			if (!character.Quests.Has(Sq06) && character.Quests.MeetsPrerequisites(Sq06))
			{
				var answer = await dialog.SelectQuestOffer(Sq06, L("Oh no, that's a bland reaction. I'm sure each curse has it's own characteristic, but it seems like there's a key to it. Well darn. We should be experimenting, not imagining things. I am sorry, but could you get me samples of other demons?"),
					Option(L("I can get them for sure"), "accept"),
					Option(L("I should go back"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq06);
					await dialog.Msg(L("The Loftlems this time. Six of them will tell me what I need."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("It's not easy to get samples."));
				await dialog.Msg(L("I don't have enough time to research them either."));
				return;
			}

			if (character.Quests.IsActive(Sq06))
			{
				await dialog.Msg(L("I thought I just had to match the properties of the monster but it seems like the opposite."));
				await dialog.Msg(L("Naktis.. What could be moving that demon?"));
				return;
			}

			await dialog.Msg(L("We don't have much time left to stay in the Great Cathedral."));
			await dialog.Msg(L("In the meantime, we should find clues on how to release the curse."));
		});
	}

	/// <summary>
	/// The bishop's spirit at the chancel, who hands out the chain until his
	/// spirit has a scripture to live in.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task Bishop(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Bishop Aurelius' Spirit"));
		dialog.SetPortrait("Dlg_port_aurelius");

		if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("This will be enough."));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
		{
			var answer = await dialog.SelectQuestOffer(Mq01, L("Revelator. It has been long. I have waited hundreds of years to guide you. This Great Cathedral is an important heritage of the first bishop Maven, as well as being the sacred location of the revelations."),
				Option(L("Ask him how you can get guidance"), "accept"),
				Option(L("About the Great Cathedral"), "explain"),
				Option(L("I will find it alone"), "leave")
			);

			if (answer == "explain")
			{
				await dialog.Msg(L("My body is rotten and has gone back to Zemyna. But, my spirit remained at the Great Cathedral and saw many things."));
				await dialog.Msg(L("The Great Cathedral during its time of prosperity, many bishops, waves of pilgrimage. It continued until the day that the Great Cathedral turned out like this, due to Medzio Diena, four years ago."));
				return;
			}

			if (answer == "accept")
			{
				for (var i = 1; i <= 6; ++i)
					character.Variables.Perm.Set(EssenceVar + i, false);

				character.Quests.Start(Mq01);
				await dialog.Msg(L("Don't worry. We just have to complete a vessel for my Spirit."));
				await dialog.Msg(L("To start, please collect some Spirit Essence here."));
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			var answer = await dialog.SelectQuestOffer(Mq02, L("I believe a scripture will hold the Spirit Essences well enough. Unfortunately, they are all at Pamaldu Groom."),
				Option(L("I will find the scripture"), "accept"),
				Option(L("I need some time to prepare"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq02);
				await dialog.Msg(L("Oh, the vessel should be completed at the Altar of Stability."));
				await dialog.Msg(L("I have spent centuries without a body, so I hope you get a decent scripture."));
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			var answer = await dialog.SelectQuestOffer(Mq03, L("Are you done with the ritual? Check if this scripture can call me correctly."),
				Option(L("Ask him how to call him"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq03);
				character.Quests.CompleteObjective(Mq03, "callTheBishop");
				await dialog.Msg(L("It's simple. Just open the Spirit's Scripture in your inventory."));
				character.ServerMessage(L("The Spirit's Scripture opens and the bishop stands beside the Altar of Stability."));
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("It has been worth the long wait."));
			await dialog.Msg(L("Although my body is like this, I am overflowing with emotion right now."));
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("Although I'm just a spirit, I have offered myself to the goddess."));
			await dialog.Msg(L("I want to be placed in a scripture."));
			return;
		}

		await dialog.Msg(L("I've been waiting for you for hundreds of years."));
		await dialog.Msg(L("However, you are still weak. You must improve..."));
	}

	/// <summary>
	/// The bishop as he stands once the Spirit's Scripture can call him, who
	/// owns the rest of the Main Chamber's chain.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SummonedBishop(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Bishop Aurelius' Spirit"));
		dialog.SetPortrait("Dlg_port_aurelius");

		if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("Splendid. Now you must solve the secret of the Great Cathedral and get the revelation."));
			await dialog.Msg(L("Of course, there will be hardship. The burden of the holy mission will surely weigh down on your shoulders."));
			await dialog.Msg(L("But only you and the goddess can save this world. Even knowing my body would turn out like this, I desired to guide you."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
		{
			await dialog.Msg(L("You have taken a round jewel back from Naktis' servant."));
			await dialog.CompleteQuest(Mq05);
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			var answer = await dialog.SelectQuestOffer(Mq04, L("The revelation is being protected by the five keys of Maven. To get those keys, you must solve all the secrets. Unfortunately, it is Maven's will that you solve every secret by yourself."),
				Option(L("I will look for the divine artifacts of the mercy"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);
				await dialog.Msg(L("First, get on the platform at the Meile Oratorium to solve the first secret and get the Holy Relic of Mercy."));
			}
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			var answer = await dialog.SelectQuestOffer(Mq05, L("Well done. Now to continue hereafter at this pace. I mean the second secret. Unfortunately, it seems that Naktis' servants have taken the lead."),
				Option(L("Ask him what to do"), "accept"),
				Option(L("Tell him that there is nothing that can be done for the ones missed"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq05);
				await dialog.Msg(L("We have to take care of it before it falls into Naktis' hands."));
				await dialog.Msg(L("Please get back the Holy Relic of Salvation from the demons."));
			}
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			var answer = await dialog.SelectQuestOffer(Mq06, L("I'm relieved we got it back. I finally get to see the holy relic, even after death. Bring that holy relic to the Small Hall and place it on top of the altar. Only then will you be able to get the first key of Maven."),
				Option(L("I will go to the Small Hall"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.Set(AltarVar + 1, false);
				character.Variables.Perm.Set(AltarVar + 2, false);

				character.Quests.Start(Mq06);
				await dialog.Msg(L("You must thoroughly check the writings on the altar."));
				await dialog.Msg(L("Well then, I will.. wait in the Grand Hall."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("Unfortunately, I cannot solve it for you. It is the will of Maven. However, I am able to give some words of advice."));
			await dialog.Msg(L("I think this is what the writings on the platform said. Remember the sacred numbers with your own two eyes. The candle light will chase away the evil darkness."));
			await dialog.Msg(L("Remember the number written on the candle holder, and light that many candles to solve it."));
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("The relics have fallen into the demons' hands because of my weakness."));
			await dialog.Msg(L("But they shouldn't have reached Naktis yet. I cannot guarantee the safety of other Holy Relics or the keys so you best hurry."));
			return;
		}

		if (character.Quests.IsActive(Mq06))
		{
			await dialog.Msg(L("Insert the Relics that match each altar."));
			await dialog.Msg(L("Then you will be able to get Maven's first key."));
			return;
		}

		await dialog.Msg(L("There are still more keys left to find."));
		await dialog.Msg(L("It's a relief that they are all secretly guarded by Maven's plan."));
	}

	/// <summary>
	/// Adds one of the Spirit Essence crystals of the chancel.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	private void AddSpiritEssence(int number, double x, double z)
	{
		AddConditionalNpc(46221, L("Spirit Essence"), "CHATHEDRAL53_MQ01_" + number, "d_cathedral_53", x, z, 90, c => c.Quests.IsActive(Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Spirit Essence"));

			if (character.Variables.Perm.GetBool(EssenceVar + number, false))
			{
				await dialog.Msg(L("{#666666}*This crystal has already given up its essence*{/}"));
				return;
			}

			var gathered = await character.TimeActions.StartAsync(L("Gathering the Spirit Essence..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

			if (gathered != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(EssenceVar + number, true);
			character.Inventory.Add(ItemId.CHATHEDRAL53_MQ01_ITEM, 1, InventoryAddType.PickUp);
			character.ServerMessage(LF("Spirit Essence: {0}/{1}", character.Inventory.CountItem(ItemId.CHATHEDRAL53_MQ01_ITEM), SpiritEssencesNeeded));
		});
	}

	/// <summary>
	/// Adds one of the scriptures of Pamaldu Groom, any of which can hold the
	/// bishop's spirit.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="direction"></param>
	private void AddScripture(int number, string name, double x, double z, double direction)
	{
		AddConditionalNpc(147311, name, "CHATHEDRAL53_MQ02_BOOK" + number, "d_cathedral_53", x, z, direction, c => c.Quests.IsActive(Mq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(name);

			if (character.Inventory.CountItem(ItemId.CHATHEDRAL53_MQ02_ITEM) > 0)
			{
				await dialog.Msg(L("{#666666}*You are already carrying a scripture whole enough to hold a spirit*{/}"));
				return;
			}

			var searched = await character.TimeActions.StartAsync(L("Looking the scripture over..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

			if (searched != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.CHATHEDRAL53_MQ02_ITEM, 1, InventoryAddType.PickUp);
			character.ServerMessage(L("The binding is whole and the pages are unburnt. This one will do."));
		});
	}

	/// <summary>
	/// Adds one of the two old altars whose writings open Maven's Secret.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="firstLine"></param>
	/// <param name="secondLine"></param>
	private void AddOldAltar(int number, double x, double z, string firstLine, string secondLine)
	{
		AddNpc(147417, L("Old Altar"), "CHATHEDRAL53_MQ06_HINT0" + number, "d_cathedral_53", x, z, 88, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Writings on the Altar"));

			await dialog.Msg(firstLine);
			await dialog.Msg(secondLine);

			if (!character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			if (character.Variables.Perm.GetBool(AltarVar + number, false))
				return;

			var placed = await character.TimeActions.StartAsync(L("Setting the relic into the altar..."), L("Cancel"), "SITGROPESET2", TimeSpan.FromSeconds(2));

			if (placed != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(AltarVar + number, true);

			if (!character.Variables.Perm.GetBool(AltarVar + 1, false) || !character.Variables.Perm.GetBool(AltarVar + 2, false))
			{
				character.ServerMessage(L("One altar answers. The other is still silent."));
				return;
			}

			character.Quests.CompleteObjective(Mq06, "insertRelic");
			character.ServerMessage(L("Both altars answer, and Maven's Secret stirs."));
		});
	}

	/// <summary>
	/// Adds one of the Scriptural Relics strewn over the cathedral floor.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	private void AddScripturalRelic(int number, double x, double z)
	{
		AddConditionalNpc(151022, L("Scriptural Relic"), "CHATHEDRAL53_SQ01_OBJECT0" + number, "d_cathedral_53", x, z, 90, c => c.Quests.IsActive(Sq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Scriptural Relic"));

			if (character.Variables.Perm.GetBool(RelicVar + number, false))
			{
				await dialog.Msg(L("{#666666}*You have already recovered this relic*{/}"));
				return;
			}

			var recovered = await character.TimeActions.StartAsync(L("Recovering the Scriptural Relic..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

			if (recovered != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(RelicVar + number, true);
			character.Inventory.Add(ItemId.CATHEDRAL53_SQ01_ITEM, 1, InventoryAddType.PickUp);
			character.ServerMessage(LF("Scriptural Relics recovered: {0}/{1}", character.Inventory.CountItem(ItemId.CATHEDRAL53_SQ01_ITEM), ScripturalRelicsNeeded));
		});
	}

	/// <summary>
	/// Adds one of the Graceful Relics that only show themselves to the Orb of
	/// Divine Detection.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	private void AddHiddenRelic(int number, double x, double z)
	{
		AddConditionalNpc(151022, L("Graceful Relic"), "CATHEDRAL_SQ_OBJECT0" + number, "d_cathedral_53", x, z, 90, c => c.Quests.IsActive(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Graceful Relic"));

			if (character.Variables.Perm.GetBool(HiddenRelicVar + number, false))
			{
				await dialog.Msg(L("{#666666}*You have already recovered this relic*{/}"));
				return;
			}

			var recovered = await character.TimeActions.StartAsync(L("Drawing the relic out with the orb..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

			if (recovered != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(HiddenRelicVar + number, true);
			character.Inventory.Add(ItemId.CATHEDRAL53_SQ021_ITEM, 1, InventoryAddType.PickUp);
			character.ServerMessage(LF("Graceful Relics recovered: {0}/{1}", character.Inventory.CountItem(ItemId.CATHEDRAL53_SQ021_ITEM), GracefulRelicsNeeded));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20300: A Vessel for a Spirit (1)
//-----------------------------------------------------------------------------
public class Cathedral53Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20300);
		SetName(L("A Vessel for a Spirit (1)"));
		SetDescription(L("Something pale is waiting in the Main Chamber, and it has been waiting a very long time."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_MQ_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("Someone is welcoming you at the Great Cathedral. Talk with Bishop Aurelius."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_MQ_BISHOP", "d_cathedral_53", L("Collect Spirit Essences"), L("Bishop Aurelius has been waiting for you for hundreds of years. In order for Bishop Aurelius to guide you, you need to collect Spirit Essences."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_MQ_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("You've collected enough Spirit Essences. Please hand them over to Bishop Aurelius."));

		// The chain link from the Mage Tower; the client gates this on the
		// level band alone.
		AddPrerequisite(new LevelPrerequisite(127));
		AddPrerequisite(new QuestStatusPrerequisite(8512, QuestStatus.Completed));

		AddObjective("collectEssences", L("Collect Spirit Essences"), new CollectItemObjective("CHATHEDRAL53_MQ01_ITEM", 4));

		AddReward(new ItemReward("expCard8", 1));
	}
}

// 20301: A Vessel for a Spirit (2)
//-----------------------------------------------------------------------------
public class Cathedral53Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20301);
		SetName(L("A Vessel for a Spirit (2)"));
		SetDescription(L("A scripture whole enough to hold a spirit is somewhere in Pamaldu Groom."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_MQ_BISHOP", "d_cathedral_53", L("Talk with Bishop Aurelius"), L("You should make a vessel for Bishop Aurelius' Spirit. Talk with Bishop Aurelius."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_MQ_BISHOP", "d_cathedral_53", L("Look for a Complete Scripture at Pamaldu Groom"), L("Bishop Aurelius told you that a scripture would be a good vessel for his spirit. Look for a Complete Scripture at Pamaldu Groom."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_MQ03", "d_cathedral_53", L("Complete the vessel for the spirit at the Altar of Stability"), L("You found a Complete Scripture. Put the Spirit Essence into the Scripture at the Altar of Stability and complete the vessel for Aurelius' Spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(20300, QuestStatus.Completed));

		AddObjective("findScripture", L("Look for a Complete Scripture at Pamaldu Groom"), new CollectItemObjective("CHATHEDRAL53_MQ02_ITEM", 1));

		AddReward(new ItemReward("CHATHEDRAL53_MQ03_ITEM", 1));
		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CHATHEDRAL53_MQ01_ITEM", 4));
		AddReward(new TakeItemReward("CHATHEDRAL53_MQ02_ITEM", 1));
	}
}

// 20302: A Vessel for a Spirit (3)
//-----------------------------------------------------------------------------
public class Cathedral53Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20302);
		SetName(L("A Vessel for a Spirit (3)"));
		SetDescription(L("The Spirit's Scripture is finished, and it should be tried once before it is trusted."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_MQ_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("You've completed the vessel for the spirit. Talk with Bishop Aurelius."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL_BISHOP", "d_cathedral_53", L("Call Bishop Aurelius with the Spirit's Scripture"), L("You've captured the Bishop's Spirit in the completed vessel. Try calling out Bishop Aurelius using the Spirit's Scripture."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("You were able to call Bishop Aurelius using the Scripture without any problem. Talk with Bishop Aurelius again."));

		AddPrerequisite(new QuestStatusPrerequisite(20301, QuestStatus.Completed));

		AddObjective("callTheBishop", L("Call Bishop Aurelius with the Spirit's Scripture"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 1));
	}
}

// 20303: Mercy and Salvation (1)
//-----------------------------------------------------------------------------
public class Cathedral53Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20303);
		SetName(L("Mercy and Salvation (1)"));
		SetDescription(L("The first of Maven's five secrets is a count of candles in the Meile Oratorium."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("Bishop Aurelius' Spirit has been completely saved at the altar. To find out the next step, talk to Bishop Aurelius by using the Spirit's Scripture in your inventory."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_MQ04_HINT", "d_cathedral_53", L("Solve the secret of Meile Oratorium"), L("Get the secret hint from the platform in Meile Oratorium and obtain the Holy Relic of Mercy. If you have difficulty solving the secret, call on Aurelius' Spirit and ask him for help."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_MQ04_HINT", "d_cathedral_53", L("Solve the secret of Meile Oratorium"), L("The secret of Meile Oratorium has been released. Look at the Meile Oratorium."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL53_MQ04_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20302, QuestStatus.Completed));

		AddObjective("solveSecret", L("Solve the secret of Meile Oratorium"), new ManualObjective());

		AddReward(new ItemReward("CHATHEDRAL53_MQ04_ITEM", 1));
		AddReward(new ItemReward("expCard8", 2));
	}
}

// 20304: Mercy and Salvation (2)
//-----------------------------------------------------------------------------
public class Cathedral53Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20304);
		SetName(L("Mercy and Salvation (2)"));
		SetDescription(L("The second relic is already in the hands of Naktis' servants."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("Found the Relic of Mercy. In order to proceed, talk to Bishop Aurelius by using the Spirit's Scripture in your inventory."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL_BISHOP", "d_cathedral_53", L("Look for the Holy Relic of Salvation"), L("Retrieve the Holy Relic of Salvation by defeating the demons. If you have trouble retrieving the relic, call out Bishop Aurelius and ask him."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("You retrieved the Holy Relic of Salvation. Talk with Bishop Aurelius."));

		AddPrerequisite(new QuestStatusPrerequisite(20303, QuestStatus.Completed));

		AddPityDrop("CHATHEDRAL53_MQ05_ITEM", 0.1f, 20, 1, "loftlem_blue");

		AddObjective("retrieveRelic", L("Retrieve the Holy Relic of Salvation by defeating Loftlems"), new CollectItemObjective("CHATHEDRAL53_MQ05_ITEM", 1));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CHATHEDRAL53_MQ05_ITEM"));
	}
}

// 20305: Mercy and Salvation (3)
//-----------------------------------------------------------------------------
public class Cathedral53Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20305);
		SetName(L("Mercy and Salvation (3)"));
		SetDescription(L("Both relics go into the old altars of the Small Hall, and the first key comes out."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL_BISHOP", "d_cathedral_53", L("Talk to Bishop Aurelius"), L("Found the Relic of Mercy. In order to find Maven's first key, talk to Bishop Aurelius by using the Spirit's Scripture in your inventory."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_MQ06_HINT01", "d_cathedral_53", L("Insert the Relic of Mercy into the altar at the Small Hall"), L("Find Maven's first key by inserting the appropriate relic into the altars at the Small Hall. If you're having trouble, then call Bishop Aurelius for help."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_MQ06_PUZZLE", "d_cathedral_53", L("Look at Maven's secret"), L("Maven's secret has been revealed. Look at Maven's secret."));

		AddPrerequisite(new QuestStatusPrerequisite(20304, QuestStatus.Completed));

		AddObjective("insertRelic", L("Insert the Relic of Mercy into the altar at the Small Hall"), new ManualObjective());

		AddReward(new ItemReward("CHATHEDRAL53_MQ06_ITEM", 1));
		AddReward(new ItemReward("expCard8", 2));
	}
}

// 20306: Endlessly Blasphemous
//-----------------------------------------------------------------------------
public class Cathedral53Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20306);
		SetName(L("Endlessly Blasphemous"));
		SetDescription(L("The words of the great bishops are lying on the floor where the demons walk."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_SQ01", "d_cathedral_53", L("Talk to Priest Inea"), L("Priest Inea seems to be irritated. Talk with Priest Inea."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_SQ01", "d_cathedral_53", L("Look for the Scriptural Relics"), L("Priest Inea told you that the congregation ordered to retrieve all the relics in the Great Cathedral. Retrieve all the Scriptural Relics that are scattered on the floor of the Great Cathedral."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_SQ01", "d_cathedral_53", L("Hand it over to Priest Inea"), L("You found all the Scriptural Relics. Hand them over to Priest Inea."));

		AddPrerequisite(new LevelPrerequisite(127));

		AddObjective("collectRelics", L("Look for the Scriptural Relics"), new CollectItemObjective("CATHEDRAL53_SQ01_ITEM", 8));

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("CATHEDRAL53_SQ01_ITEM", 8));
	}
}

// 20307: Well Hidden Holy Relics
//-----------------------------------------------------------------------------
public class Cathedral53Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20307);
		SetName(L("Well Hidden Holy Relics"));
		SetDescription(L("Some relics hide from evil, and only the Orb of Divine Detection draws them out."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_SQ01", "d_cathedral_53", L("Talk with Priest Inea"), L("Priest Inea seems to have some trouble looking for some relics. Talk with Priest Inea."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_SQ01", "d_cathedral_53", L("Find the relics using the Orb of Divine Detection"), L("Some relics hide themselves when they detect evil energy. Use the Orb of Divine Detection to find the hidden relics."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_SQ01", "d_cathedral_53", L("Hand it over to Priest Inea"), L("Acquired all the Graceful Relics. Hand them over to Priest Inea."));

		AddPrerequisite(new LevelPrerequisite(127));

		AddObjective("collectHiddenRelics", L("Find the relics using the Orb of Divine Detection"), new CollectItemObjective("CATHEDRAL53_SQ021_ITEM", 5));

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("CATHEDRAL53_SQ021_ITEM", 5));
		AddReward(new TakeItemReward("CATHEDRAL53_SQ02_ITEM", 1));
	}
}

// 20308: Research Reports of the Priests
//-----------------------------------------------------------------------------
public class Cathedral53Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20308);
		SetName(L("Research Reports of the Priests"));
		SetDescription(L("Three priests are studying Naktis' curse in three separate places, and none of them can leave."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_53", "d_cathedral_54", "d_cathedral_56", "f_pilgrimroad_55");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_SQ03", "d_cathedral_53", L("Talk with Priest Aden"), L("Priest Aden seems to be troubled. Talk with Priest Aden."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_SQ03_PART1", "d_cathedral_54", L("Bring the Priests' Research Reports"), L("Priest Aden is investigating how to release Naktis' curse so he wants to look at the reports of other priests. Obtain the reports from Yosana at Grand Corridor, Gadan at Penitence Route of Great Cathedral, and from Prosit at Sanctuary."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_SQ03", "d_cathedral_53", L("Hand it over to Priest Aden"), L("You've received every Priests' Research Reports. Hand them over to Priest Aden."));

		AddPrerequisite(new LevelPrerequisite(127));

		AddObjective("yosanaReport", L("Obtain Priest Yosana's report"), new CollectItemObjective("PRIST_REPORT01", 1));
		AddObjective("gadanReport", L("Obtain Priest Gadan's report"), new CollectItemObjective("PRIST_REPORT02", 1));
		AddObjective("prositReport", L("Obtain Priest Prosit's report"), new CollectItemObjective("PRIST_REPORT03", 1));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("PRIST_REPORT01", 1));
		AddReward(new TakeItemReward("PRIST_REPORT02", 1));
		AddReward(new TakeItemReward("PRIST_REPORT03", 1));
	}
}

// 50000: Disintegration (1)
//-----------------------------------------------------------------------------
public class Cathedral53Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50000);
		SetName(L("Disintegration (1)"));
		SetDescription(L("Priest Benedict wants the curse taken apart, and that starts with samples."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_SQ02", "d_cathedral_53", L("Talk to Priest Benedict"), L("Priest Benedict needs your help. Talk to Priest Benedict."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_SQ02", "d_cathedral_53", L("Collect demon samples that are needed for the research"), L("Priest Benedict is researching a way to remove Naktis' curse. Obtain demon samples for Benedict."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_SQ02", "d_cathedral_53", L("Deliver them to Priest Benedict"), L("You've collected enough demon samples. Hand them over to Priest Benedict."));

		AddPrerequisite(new LevelPrerequisite(128));

		AddPityDrop("CATHEDRAL53_SQ_SAMPLE01", 0.6f, 3, 1, "Colifly");

		AddObjective("collectSamples", L("Obtain demon samples by defeating Colifly"), new CollectItemObjective("CATHEDRAL53_SQ_SAMPLE01", 12));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CATHEDRAL53_SQ_SAMPLE01"));
	}
}

// 50001: Disintegration (2)
//-----------------------------------------------------------------------------
public class Cathedral53Sq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50001);
		SetName(L("Disintegration (2)"));
		SetDescription(L("The first samples read backwards, so Benedict wants a different demon."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_53");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL53_SQ02", "d_cathedral_53", L("Talk to Priest Benedict"), L("Priest Benedict looks dissatisfied with the results. Talk to him again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL53_SQ02", "d_cathedral_53", L("Collect demon samples"), L("Priest Benedict thinks that his theory might be wrong so he asked you to get the samples of other demons. Collect samples of the other demons."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL53_SQ02", "d_cathedral_53", L("Deliver them to Priest Benedict"), L("You've collected enough demon samples. Hand them over to Priest Benedict."));

		AddPrerequisite(new LevelPrerequisite(128));
		AddPrerequisite(new QuestStatusPrerequisite(50000, QuestStatus.Completed));

		AddPityDrop("CATHEDRAL53_SQ_SAMPLE02", 0.6f, 3, 1, "loftlem_blue");

		AddObjective("collectSamples", L("Obtain demon samples by defeating Loftlem"), new CollectItemObjective("CATHEDRAL53_SQ_SAMPLE02", 6));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CATHEDRAL53_SQ_SAMPLE02"));
	}
}
