//--- Melia Script ----------------------------------------------------------
// Delmore Outskirts Quest NPCs
//--- Description -----------------------------------------------------------
// The Revelators chasing Delmore Rephaim through the outskirts to rescue
// Mage Melchioras.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.Util;
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

public class FCastle653QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Manor652Mq05 = new QuestId(70424);
	private readonly static QuestId Hq1 = new QuestId(50268);
	private readonly static QuestId Rp1 = new QuestId(60177);
	private readonly static QuestId Rp2 = new QuestId(60178);
	private readonly static QuestId Mq01 = new QuestId(70440);
	private readonly static QuestId Mq02 = new QuestId(70441);
	private readonly static QuestId Mq03 = new QuestId(70442);
	private readonly static QuestId Mq04 = new QuestId(70443);
	private readonly static QuestId Mq05 = new QuestId(70444);
	private readonly static QuestId Mq06 = new QuestId(70445);
	private readonly static QuestId Mq07 = new QuestId(70446);
	private readonly static QuestId Mq08 = new QuestId(70447);
	private readonly static QuestId Mq09 = new QuestId(70448);
	private readonly static QuestId Sq01 = new QuestId(70449);
	private readonly static QuestId Sq02 = new QuestId(70450);
	private readonly static QuestId Sq03 = new QuestId(70451);
	private readonly static QuestId Sq04 = new QuestId(70452);

	private const string Mq05TrackId = "CASTLE65_3_MQ05_TRACK";

	public const string DollSoulsVar = "Gabija.Quests.Castle653Mq05.Souls";
	public const string ReserveDevicesVar = "Gabija.Quests.Castle653Sq01.Destroyed";
	public const string ManorPillarsVar = "Gabija.Quests.Castle653Sq02.Destroyed";
	private const string ReserveDeviceVar = "Gabija.Quests.Castle653Sq01.Device";
	private const string ManorPillarVar = "Gabija.Quests.Castle653Sq02.Pillar";
	private const string CrystalChargeVar = "Gabija.Quests.Castle653Sq01.Charge";
	private const string GrassVar = "Gabija.Quests.Castle653Rp2.Grass";

	private const int SoulsPerKill = 10;
	private const int ChargePerKill = 20;
	private const int CrystalRange = 150;
	private const int GrassNeeded = 6;

	private static readonly TimeSpan GrassRespawn = TimeSpan.FromSeconds(30);

	private static readonly string[] OutskirtsDemons = { "PagNurse", "PagEmitter", "PagDoper" };
	private static readonly string[] ManorDemons = { "PagNanny", "PagWheeler", "Paggnat" };

	private static readonly double[,] ReserveDevices =
	{
		{ -1854.69, -1036.02, 90 }, { -1963.86, 911.72, 95 }, { 453.17, -1645.25, 90 },
	};

	private static readonly double[,] ManorPillars =
	{
		{ 509.23, -1839.06 }, { 433.11, 1283.32 }, { 1987.75, 1009.20 },
	};

	private static readonly double[,] EnchantmentDevices =
	{
		{ -1035.45, 1239.05 }, { -894.03, 1380.47 }, { -1035.45, 1039.05 }, { -894.03, 897.63 },
		{ -694.03, 897.63 }, { -552.61, 1039.05 }, { -552.61, 1239.05 }, { -694.03, 1380.47 },
		{ -848.72, -291.36 }, { -756.79, -199.82 }, { -848.72, -421.75 }, { -756.79, -513.67 },
		{ -626.79, -513.67 }, { -534.87, -421.75 }, { -534.87, -291.36 }, { -626.79, -199.82 },
	};

	private static readonly double[,] Furniture =
	{
		{ 550.59, -1049.98, 112 }, { 587.10, -1283.40, 51 }, { 583.46, -1219.16, 175 }, { 586.42, -1150.58, 19 },
	};

	private static readonly double[,] MaroulGrass =
	{
		{ -14.55, 470.97 }, { 388.80, 334.94 }, { 328.61, 584.09 }, { 346.04, 753.82 }, { 368.60, 919.06 }, { 312.39, 972.65 },
		{ 138.71, 967.78 }, { 50.59, 880.21 }, { -10.88, 810.57 }, { -9.38, 568.96 }, { -246.80, 699.70 }, { -232.60, 1315.84 },
		{ 1147.65, 172.26 }, { 949.25, 173.12 }, { 721.22, 25.01 }, { 596.72, 203.09 }, { -3.01, 685.74 }, { 201.91, 405.81 },
	};

	protected override void Load()
	{
		// The Revelators at the crossroads entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(155095, L("Revelator Yane"), "CASTLE653_MQ_01_1", "f_castle_65_3", -1690.17, -452.16, 43, c => c.Quests.Has(Manor652Mq05) && !c.Quests.HasCompleted(Mq01), this.YaneAtEntrance);

		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE653_MQ_01_2", "f_castle_65_3", -1713.32, -502.79, 75, c => c.Quests.Has(Manor652Mq05) && !c.Quests.IsCompletable(Mq01) && !c.Quests.HasCompleted(Mq01), this.MihailAtEntrance);

		AddConditionalNpc(155096, L("Revelator Connor"), "CASTLE653_MQ_01_3", "f_castle_65_3", -1633.75, -429.88, 353, c => c.Quests.Has(Manor652Mq05) && !c.Quests.HasCompleted(Mq01), async dialog =>
		{
			dialog.SetTitle(L("Revelator Connor"));
			await dialog.Msg(L("Delmore Rephaim... Seeing how he made the Kruvina, he's not someone we should underestimate."));
		});

		// The Outskirts Central Plaza, where Delmore Rephaim set his trap
		//-------------------------------------------------------------------------
		AddQuestTrigger("CASTLE653_MQ_02", "f_castle_65_3", -1103.01, 1151.13, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsCompletable(Mq01))
			{
				character.Quests.Complete(Mq01);
				character.LookAround();
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
				character.Quests.Start(Mq02);
			else if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
				character.Quests.ReplayQuestTrack(Mq02);

			await Task.CompletedTask;
		});

		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE653_MQ_03", "f_castle_65_3", -691.02, 447.46, 14, c => c.Quests.Has(Mq02) && !c.Quests.Has(Mq03), this.MihailAtPlaza);
		AddNpc(47107, "UnvisibleName", "CASTLE653_ENCHANT_CORE", "f_castle_65_3", -752.97, 387.70, 20);

		for (var i = 0; i < EnchantmentDevices.GetLength(0); ++i)
			AddNpc(47106, "UnvisibleName", "CASTLE653_ENCHANT_DEVICE_" + (i + 1), "f_castle_65_3", EnchantmentDevices[i, 0], EnchantmentDevices[i, 1], 90);

		// The Tagika Crossroads, where Yane's group got trapped
		//-------------------------------------------------------------------------
		AddQuestTrigger("CASTLE653_MQ_03_1", "f_castle_65_3", -689.14, 43.90, 200, async args =>
		{
			if (args.Initiator is Character character && character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				character.Quests.ClearQuestTrack(Mq03);
				character.Quests.StartQuestTrack(Mq03);
			}

			await Task.CompletedTask;
		});

		AddConditionalNpc(155095, L("Revelator Yane"), "CASTLE653_MQ_04_1", "f_castle_65_3", -718.17, -372.99, 90, IsGroupAtCrossroads, this.YaneAtCrossroads);
		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE653_MQ_04_2", "f_castle_65_3", -706.21, -429.81, 90, IsGroupAtCrossroads, this.MihailAtCrossroads);
		AddConditionalNpc(155096, L("Revelator Connor"), "CASTLE653_MQ_04_3", "f_castle_65_3", -650.08, -348.71, 12, c => c.Quests.Has(Mq03), this.Connor);
		AddConditionalNpc(155117, L("Delmore Barrier"), "CASTLE653_MQ_04_4", "f_castle_65_3", -409.77, -293.15, 90, c => !c.Quests.Has(Mq04));

		// The Government Ruins and the Life Absorbing Altar
		//-------------------------------------------------------------------------
		AddNpc(155106, "UnvisibleName", "CASTLE653_ALTAR", "f_castle_65_3", 99.75, -142.96, 80);
		AddConditionalNpc(155113, L("Mage Melchioras"), "CASTLE653_MQ_04_5", "f_castle_65_3", 102.15, -148.15, 0, c => c.Quests.Has(Mq04) && !IsMelchiorasFreed(c));
		AddConditionalNpc(57411, "UnvisibleName", "CASTLE653_MQ_05_3", "f_castle_65_3", 103.09, -152.36, 357, IsMelchiorasFreed);

		AddConditionalNpc(155095, L("Revelator Yane"), "CASTLE653_MQ_05_1", "f_castle_65_3", 57.50, -176.52, 65, c => c.Quests.Has(Mq04) && !c.Quests.Has(Mq09), this.YaneAtRuins);
		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE653_MQ_05_2", "f_castle_65_3", 157.74, -162.11, 1, c => c.Quests.Has(Mq04) && !c.Quests.Has(Mq06), async dialog =>
		{
			dialog.SetTitle(L("Revelator Mihail"));

			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("Fortunately Melchioras is still breathing. It could have been worse had we arrived later."));
			else
				await dialog.Msg(L("To think he resorted to something so low... It's getting late. We should hurry now."));
		});

		AddConditionalNpc(155113, L("Mage Melchioras"), "CASTLE653_MQ_06", "f_castle_65_3", 104.43, -192.52, 20, c => IsMelchiorasFreed(c) && !c.Quests.Has(Mq09), this.MelchiorasAtRuins);

		// The Odaginkas Vacant Lot, where Delmore Rephaim made his last stand
		//-------------------------------------------------------------------------
		AddQuestTrigger("CASTLE653_MQ_07_1", "f_castle_65_3", 897.96, -1490.94, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsCompletable(Mq06))
				character.Quests.Complete(Mq06);

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				character.Quests.Start(Mq07);
				character.LookAround();
			}
			else if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
			{
				character.Quests.ReplayQuestTrack(Mq07);
			}

			await Task.CompletedTask;
		});

		AddConditionalNpc(155094, L("Revelator Mihail"), "CASTLE653_MQ_07_2", "f_castle_65_3", 1163.10, -1572.72, 71, c => c.Quests.Has(Mq07) && !c.Quests.Has(Mq09), this.MihailAtVacantLot);

		AddConditionalNpc(147379, "UnvisibleName", "CASTLE653_MQ_08", "f_castle_65_3", 1197.16, -1555.22, 103, c => !c.Quests.Has(Mq08));
		for (var i = 0; i < Furniture.GetLength(0); ++i)
			AddConditionalNpc(47241, "UnvisibleName", "CASTLE653_MQ_08_" + (i + 2), "f_castle_65_3", Furniture[i, 0], Furniture[i, 1], Furniture[i, 2], c => !c.Quests.Has(Mq08));

		// Mage Melchioras, back at the Tagika Crossroads
		//-------------------------------------------------------------------------
		AddConditionalNpc(155113, L("Mage Melchioras"), "CASTLE653_MQ_09", "f_castle_65_3", -704.52, -343.85, 41, c => c.Quests.Has(Mq09), this.MelchiorasAtCrossroads);

		// The reserve Kruvina devices
		//-------------------------------------------------------------------------
		for (var i = 0; i < ReserveDevices.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(155104, L("Kruvina Central Device"), "CASTLE653_SQ_01_" + number, "f_castle_65_3", ReserveDevices[i, 0], ReserveDevices[i, 1], ReserveDevices[i, 2],
				c => !IsReserveDeviceDestroyed(c, number),
				async dialog =>
				{
					UseCrystal(dialog.Player);
					await Task.CompletedTask;
				});
		}

		// Maroulu Grass on the fortress walls
		//-------------------------------------------------------------------------
		for (var i = 0; i < MaroulGrass.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "CASTLE653_RP_2_OBJ" : "CASTLE653_RP_2_OBJ_" + number;

			AddConditionalNpc(47200, L("Maroulu Grass"), uniqueName, "f_castle_65_3", MaroulGrass[i, 0], MaroulGrass[i, 1], 90,
				c => c.Quests.IsActive(Rp2) && !c.Quests.IsCompletable(Rp2),
				async dialog =>
				{
					GatherGrass(dialog.Player, number);
					await Task.CompletedTask;
				});
		}
	}

	/// <summary>
	/// Revelator Yane's dialog at the crossroads entrance.
	/// </summary>
	private async Task YaneAtEntrance(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Yane"));

		if (character.Quests.IsCompletable(Manor652Mq05))
		{
			await dialog.Msg(L("You're... the one who helped Melchioras. I really don't know what to say to him..."));
			await dialog.Msg(L("I should have believed him right away... But I guess I was too greedy..."));
			await dialog.Msg(L("I don't know if he'll ever accept my apologies... But rescuing him is the least I can do to try and redeem myself."));
			await dialog.CompleteQuest(Manor652Mq05);
			character.LookAround();
			return;
		}

		if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
		{
			await dialog.Msg(L("Everyone is... in their worst condition... But we're willing to do anything to rescue Melchioras and stop Delmore Rephaim."));
			await dialog.Msg(L("The crossroads start here. I think it's best if we split into groups and surround him. He'll be more intimidated once he realizes he has nowhere to escape."));

			var answer = await dialog.SelectQuestOffer(Mq01, L("Connor and I will head to Tagika Crossroads. You and Mihail take the Outskirts Central Plaza."),
				Option(L("Let's follow after"), "accept"),
				Option(L("Tell her to wait a bit"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Mq01);
			return;
		}

		if (character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("I pray for the chance to redeem myself... May the goddesses protect Melchioras..."));
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("We're getting closer to him on both sides of the crossroads. He'll be more intimidated once he realizes he has nowhere to escape."));
			return;
		}

		await dialog.Msg(L("I think Delmore Rephaim is planning to use Melchioras as a shield. We should hurry."));
	}

	/// <summary>
	/// Revelator Mihail's dialog at the crossroads entrance.
	/// </summary>
	private async Task MihailAtEntrance(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Mihail"));

		if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("He may be on the run, but don't forget Delmore Rephaim is a talented alchemist. Not to mention he can be very persuasive."));
			await dialog.Msg(L("We don't know what kind of trap he has prepared, so be very careful."));

			character.Quests.CompleteObjective(Mq01, "askMihail");
			character.LookAround();
			return;
		}

		await dialog.Msg(L("I'm mostly worried about Melchioras. Delmore Rephaim... if we leave him be he's just going to try and make another Kruvina."));
	}

	/// <summary>
	/// Revelator Mihail's dialog after breaking out of the trap at the
	/// Outskirts Central Plaza.
	/// </summary>
	private async Task MihailAtPlaza(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Mihail"));

		if (character.Quests.IsCompletable(Mq02))
		{
			await dialog.Msg(L("That was a really tough enchantment. I wonder how Yane did in Tagika Crossroads..."));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("I... I feel completely sick to my stomach. My whole body feels terrible after being around that Kruvina device."));
			await dialog.Msg(L("And Yane... She's too proud to admit she was affected... But she was close to the device, she probably has it way worse than me."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("I'm worried she may have already got to Delmore Rephaim. Let's go down and find her."),
				Option(L("Let's hurry"), "accept"),
				Option(L("Let's catch our breath for a while"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq03);
				character.LookAround();
			}
			return;
		}

		await dialog.Msg(L("To have an enchantment prepared in such a short time is definitely extraordinary. So, enchantment and demons... I think I got it."));
	}

	/// <summary>
	/// Revelator Yane's dialog at the Tagika Crossroads.
	/// </summary>
	private async Task YaneAtCrossroads(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Yane"));

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("Thank you for your help. I could've done more if only my condition was better..."));
			await dialog.Msg(L("We saw a strange light on the way to a street near the ruins. We ran to it but we got caught up in the enchantment. Connor's injuries seem worse than mine, he should rest here."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (character.Quests.IsCompletable(Sq03))
		{
			await dialog.Msg(L("I, too, did Melchioras wrong. I want to stay and help him for a little longer."));
			await dialog.Msg(L("I have nothing but gratitude for you. I will pray to the goddesses, that they may protect your every step."));
			await dialog.CompleteQuest(Sq03);
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			var answer = await dialog.SelectQuestOffer(Mq04, L("Everyone else should move now. It's just over the obstacle. Are you ready?"),
				Option(L("Let's go now"), "accept"),
				Option(L("Let's get some rest first"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
		{
			await dialog.Msg(L("Thank you so much for rescuing Melchioras. I would not be able to handle the guilt if he hadn't made it..."));
			await dialog.Msg(L("I know this was all the work of Delmore Rephaim but... Without help from the demons it would have been impossible."));
			await dialog.Msg(L("The only way is to nip the evil in the bud and destroy them all... But how? I can barely stand up right now."));

			var answer = await dialog.SelectQuestOffer(Sq03, L("Would you please help me and clear out a few Pags?"),
				Option(L("Just tell me what you need"), "accept"),
				Option(L("I'll take it from here"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Sq03);
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("Monsters are a threat too, but not as much as demons. Still, I believe the day will come when all demons are eradicated from this land."));
			return;
		}

		if (character.Quests.Has(Mq09))
		{
			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("Nothing guarantees there won't be another person like Delmore Rephaim in the future. Still, it's a pity what happened to Delmore Castle..."));
			else
				await dialog.Msg(L("Thank you so much for rescuing Melchioras. I don't know what would be the consequence of this guilt if he hadn't made it..."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("I could've done more if only my condition was better... Clearly I need to train more. Let's go. I saw a strange light by a street near the ruins."));
		else
			await dialog.Msg(L("I'm already exhausted, but I can't stop. Rescuing Melchioras is more important than my condition right now."));
	}

	/// <summary>
	/// Revelator Mihail's dialog at the Tagika Crossroads.
	/// </summary>
	private async Task MihailAtCrossroads(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Mihail"));

		if (character.Quests.IsCompletable(Sq04))
		{
			await dialog.Msg(L("Oh, thank you. I should be able to tweak this and make a more powerful bomb."));
			await dialog.CompleteQuest(Sq04);
			return;
		}

		if (character.Quests.IsCompletable(Rp1))
		{
			await dialog.Msg(L("That's a lot of monsters! Now all that is left is to set up the traps without them noticing."));
			await dialog.CompleteQuest(Rp1);
			return;
		}

		if (character.Quests.IsCompletable(Hq1))
		{
			await dialog.Msg(L("You've had an audience with Goddess Gabija in Mage Tower and Goddess Vakarine in the Demon Prison? My word..."));
			await dialog.Msg(L("I'm glad to hear the goddesses are safe."));
			await dialog.CompleteQuest(Hq1);
			return;
		}

		if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
		{
			await dialog.Msg(L("Melchioras is safe now... And thanks to you, Delmore Rephaim was defeated, too. I'm glad it's all over."));
			await dialog.Msg(L("The problem now are the demons... They never seem to go away. Black Maizes are even carrying around magic amplifiers."));

			var answer = await dialog.SelectQuestOffer(Sq04, L("Those amplifiers have given us a lot of trouble, but as a Sapper I can't help but take an interest in them. Would you collect some for me?"),
				Option(L("I'll bring it to you as soon as I have it"), "accept"),
				Option(L("It doesn't seem like we need to take action"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq04);

				await dialog.Msg(L("It's hard to tell which ones have amplifiers. Just try and fight any Black Maizes that stand out to you."));
			}
			return;
		}

		if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
		{
			await dialog.Msg(L("I am thinking of setting up traps around the Delmore Outskirts but... I wasn't able to do anything due to the Pag Emitters coming from all over."));

			var answer = await dialog.SelectQuestOffer(Rp1, L("Could you deal with some of them if they come near? I'm sure that setting up any traps will be much easier if you help."),
				Option(L("I will defeat it"), "accept"),
				Option(L("Ignore"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Rp1);
			return;
		}

		if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
		{
			var answer = await dialog.SelectQuestOffer(Hq1, L("Revelator, so nice to see you again!"),
				Option(L("How are you doing?"), "accept"),
				Option(L("Just go"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Hq1);

				await dialog.Msg(L("What have you been doing? I can't wait to know... Come on, tell me all about your new adventures!"));
				character.ServerMessage(L("Mihail looks filled with expectation."));
			}
			return;
		}

		if (character.Quests.IsActive(Hq1))
		{
			var told = await character.TimeActions.StartAsync(L("Telling him about your travels"), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
			if (told != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Hq1, "tellStories");
			return;
		}

		if (character.Quests.IsActive(Sq04))
		{
			await dialog.Msg(L("The demons here seem more... should I say human? I think they might have shared some techniques with Delmore Rephaim."));
			return;
		}

		if (character.Quests.IsActive(Rp1))
		{
			await dialog.Msg(L("I'll be discussing where the best places would be to set up the traps with my colleagues here."));
			return;
		}

		if (character.Quests.Has(Mq09))
		{
			if (GameRandom.Get().NextDouble() >= 0.5)
			{
				await dialog.Msg(L("I met quite a lot of Revelators in Klaipeda. I don't know how to explain this feeling but... you're different, that's for sure."));
				await dialog.Msg(L("Say what you will, but it was thanks to you that this all came to an end. I wonder if you're not on a different, more important mission than the rest of us... It's just what I think."));
			}
			else
			{
				await dialog.Msg(L("Yane may have commited a serious mistake, but she's still my ally. I'm going to stay here with her for a while and help Melchioras."));
			}
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("To think he set up all these enchantments in the short time he ran away... Do we even have a chance to catch up to Delmore Rephaim?"));
		else
			await dialog.Msg(L("A light in Government Ruins... It feels odd somehow. What if something happened..."));
	}

	/// <summary>
	/// Revelator Connor's dialog at the Tagika Crossroads.
	/// </summary>
	private async Task Connor(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Connor"));

		if (character.Quests.IsCompletable(Rp2))
		{
			await dialog.Msg(L("I think that this will be enough for one person. Thank you."));
			await dialog.CompleteQuest(Rp2);
			return;
		}

		if (!character.Quests.Has(Rp2) && character.Quests.MeetsPrerequisites(Rp2))
		{
			await dialog.Msg(L("I'd like to stay and help Melchioras. I'm sure you feel the same, correct?"));

			var answer = await dialog.SelectQuestOffer(Rp2, L("We should hand out the Maroulu Herbs that we found while searching the area to the people. Please look for some more from the Fortress Walls since we need much more. I'm sure we'll be able to help people recover."),
				Option(L("I'll try to find them"), "accept"),
				Option(L("I'm busy"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Rp2);
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Rp2))
		{
			await dialog.Msg(L("An herb that helps people become enthusiastic... Isn't it incredible?"));
			return;
		}

		if (character.Quests.Has(Mq09))
		{
			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("I hoped I could be the one to catch Delmore Rephaim... But I'm glad it turned out well. If only I wasn't hurt..."));
			else
				await dialog.Msg(L("Chasing away the demons, rebuilding the walls... I wonder how long it will take to make Delmore Castle a place worth living again."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("That lousy enchantment... We shouldn't be wasting time here..."));
		else
			await dialog.Msg(L("I... I think my injuries are too serious. You'll have to fight Delmore Rephaim on my behalf..."));
	}

	/// <summary>
	/// Revelator Yane's dialog at the Government Ruins.
	/// </summary>
	private async Task YaneAtRuins(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Yane"));

		if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			character.Quests.CompleteObjective(Mq04, "goToRuins");

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("This altar... It's just another one of Delmore Rephaim's dirty tricks."));
			await dialog.Msg(L("The altar is absorbing Melchioras' spirit. He did this because he knew we weren't just going to let it happen."));
			await dialog.CompleteQuest(Mq04);
			return;
		}

		if (character.Quests.IsCompletable(Mq05))
		{
			if (character.Etc.Properties.GetFloat(Mq05TrackId) != 1)
			{
				await dialog.Msg(L("When you become a Bokor you start by making your own shaman doll. I just never thought I'd be using mine to save a life..."));
				character.Quests.StartQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("I can't be stubborn anymore. Everything that happened was because of me..."));
			await dialog.CompleteQuest(Mq05);
			character.LookAround();
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("I'm a Sadhu now, but I used to train with the Bokor Master. I made a shaman doll back then, if only I could put that in Melchioras' place right now..."));

			var answer = await dialog.SelectQuestOffer(Mq05, L("It's not impossible, though. Let me give you my shaman doll; defeat some demons and collect their spirits here."),
				Option(L("I will come back soon"), "accept"),
				Option(L("That sounds dangerous"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(DollSoulsVar, 0);
				character.Quests.Start(Mq05);

				if (character.Inventory.CountItem(ItemId.CASTLE65_3_MQ05_ITEM) == 0)
					character.Inventory.Add(ItemId.CASTLE65_3_MQ05_ITEM, 1, InventoryAddType.PickUp);
			}
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("When you become a Bokor you start by making your own shaman doll. I just never thought I'd be using mine to save a life..."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
			await dialog.Msg(L("I can't be stubborn anymore. Everything that happened was because of me..."));
		else
			await dialog.Msg(L("I'll take care of Melchioras, you go and chase Delmore Rephaim. And please... come back safely."));
	}

	/// <summary>
	/// Mage Melchioras' dialog after being freed from the altar.
	/// </summary>
	private async Task MelchiorasAtRuins(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mage Melchioras"));

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			await dialog.Msg(L("Thank you for saving me but... Right now you need to focus on... Delmore Rephaim..."));

			var answer = await dialog.SelectQuestOffer(Mq06, L("Hurry and find him... He's at the Odaginkas Vacant Lot... You have to stop him..."),
				Option(L("Don't worry, just get some rest"), "accept"),
				Option(L("I need to prepare"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq06);
				character.Quests.CompleteObjective(Mq06, "chaseRephaim");

				dialog.SetTitle(L("Revelator Mihail"));
				await dialog.Msg(L("Yane, stop being stubborn and take care of Melchioras. You're in worse condition than all of us, you should stay."));
				await dialog.Msg(L("We're lagging behind. Let's hurry and head to the Odaginkas Vacant Lot!"));

				character.LookAround();
			}
			return;
		}

		await dialog.Msg(L("I'm... losing my senses now... I'm sorry... for keeping you behind."));
	}

	/// <summary>
	/// Revelator Mihail's dialog at the Odaginkas Vacant Lot.
	/// </summary>
	private async Task MihailAtVacantLot(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Revelator Mihail"));

		if (character.Quests.IsCompletable(Mq07))
		{
			await dialog.Msg(L("It's over!"));
			await dialog.CompleteQuest(Mq07);
			return;
		}

		if (character.Quests.IsCompletable(Mq08))
		{
			await dialog.Msg(L("What happened to Delmore Rephaim? What was all that light and the ground shaking...?"));
			await dialog.Msg(L("Is that so...? So he had the end he deserved. I just don't know if this can appease all the Delmore Castle residents that are now with the goddess."));
			await dialog.CompleteQuest(Mq08);
			return;
		}

		if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
		{
			character.Quests.Start(Mq08);
			character.LookAround();
			return;
		}

		if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
		{
			var answer = await dialog.SelectQuestOffer(Mq09, L("Delmore Rephaim is gone but... I'm still worried about Melchioras. Shall we go back now?"),
				Option(L("Let's go back to Melchioras"), "accept"),
				Option(L("I'm going to rest for a while"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq09);
				character.Quests.CompleteObjective(Mq09, "returnToMelchioras");
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Mq07))
		{
			await dialog.Msg(L("I think I can break it down with a bomb. Two minutes should be enough. Please hold back the demons."));
			character.Quests.ReplayQuestTrack(Mq07);
			return;
		}

		if (character.Quests.IsActive(Mq08))
		{
			await dialog.Msg(L("The explosion is sure to make demons come running here. I'll try and stop them as much as I can; you take care of Delmore Rephaim."));
			character.Quests.ReplayQuestTrack(Mq08);
			return;
		}

		await dialog.Msg(L("It's over!"));
	}

	/// <summary>
	/// Mage Melchioras' dialog back at the Tagika Crossroads.
	/// </summary>
	private async Task MelchiorasAtCrossroads(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mage Melchioras"));

		if (character.Quests.IsCompletable(Mq09))
		{
			await dialog.Msg(L("What... what happened to Delmore Rephaim?"));
			await dialog.Msg(L("I see... I guess it went as expected from him then..."));
			await dialog.Msg(L("Right before he escaped he seemed almost addicted to forbidden knowledge, you see. As if there was nothing that could stop him..."));
			await dialog.Msg(L("And I followed him, knowing full well he was an ally of the demons. But there was no match for his character, his thirst for knowledge."));
			await dialog.Msg(L("When I found out he was going to use the people of Delmore Castle to make the Kruvina... Yes, had I been able to stop him then, everyone would have been saved..."));
			await dialog.Msg(L("The reason I gathered the Revelators and came back here... Wasn't just to clear my conscience for not saving all those people."));
			await dialog.Msg(L("The Kruvina spore plan was going to happen at the Pelke Shrine Ruins... The rest of the spirits must have been sent to Letas Stream to be offered to the Divine Tree."));
			await dialog.Msg(L("The Kruvina is capable of much more horrendous deeds... Now the demons are going to try and create an even stronger one."));
			await dialog.Msg(L("The new Kruvina won't use humans, but a source of sacred power... The goddesses."));
			await dialog.Msg(L("That's why they have to be stopped. I didn't realize it at first but... I feel in you a power that is different from other Revelators."));
			await dialog.Msg(L("The only hope I have left is on you. Please, go to Seir Rainforest... put a stop to the demons' plans."));
			await dialog.CompleteQuest(Mq09);
			return;
		}

		if (character.Quests.IsCompletable(Sq01))
		{
			await dialog.Msg(L("Kruvina... I can never forgive myself for what happened..."));
			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (character.Quests.IsCompletable(Sq02))
		{
			await dialog.Msg(L("Thank you... so much. It should be impossible to create another Kruvina now."));
			await dialog.Msg(L("I can't rid myself of my sins, nor can I take them back. All I can do is stay here and continue to repent."));
			await dialog.CompleteQuest(Sq02);
			character.LookAround();
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("I think I might stay here for a while and repent. I just don't think I'll be able to move around much..."));
			await dialog.Msg(L("Something is worrying me... You see, I didn't just make one Kruvina device. There were more."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("They may be unfinished, but those devices are still out there. I was wondering if you could help me destroy them."),
				Option(L("I'll help out as much as I can"), "accept"),
				Option(L("I think it's time to leave"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(ReserveDevicesVar, 0);
				character.Variables.Perm.SetInt(CrystalChargeVar, 0);
				for (var i = 1; i <= ReserveDevices.GetLength(0); ++i)
					character.Variables.Perm.Set(ReserveDeviceVar + i, false);

				character.Quests.Start(Sq01);
				GiveCrystal(character);
				character.LookAround();

				await dialog.Msg(L("Defeat some demons and collect their demonic power in this crystal. Once the crystal is full, insert it in the device. Its magic will collide with that of the device and destroy it."));
				await dialog.Msg(L("Using the crystal depletes its magic, however, so you will have to fill it up again..."));
			}
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("What worries me, though... Do you remember?"));

			var answer = await dialog.SelectQuestOffer(Sq02, L("We didn't completely destroy the Magic Power Supply Device at Delmore Manor. I'm not trying to blame Yane, but we need to do something."),
				Option(L("Leave it to me"), "accept"),
				Option(L("Tell her that there is a more emergent issue"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(ManorPillarsVar, 0);
				character.Variables.Perm.SetInt(CrystalChargeVar, 0);
				for (var i = 1; i <= ManorPillars.GetLength(0); ++i)
					character.Variables.Perm.Set(ManorPillarVar + i, false);

				character.Quests.Start(Sq02);
				GiveCrystal(character);

				await dialog.Msg(L("We'll do it the same way as before. Defeat the demons and collect their power in the crystal."));
				await dialog.Msg(L("Use the crystal on the Magic Power Supply Device to destroy it."));
			}
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			GiveCrystal(character);
			await dialog.Msg(L("When we were designing the Kruvina device... I had no idea it would be consuming the lives of the residents of Delmore Castle."));
			await dialog.Msg(L("This was all due to my negligence."));
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			GiveCrystal(character);
			await dialog.Msg(L("I'm really not trying to blame Yane but... I just want to make sure we do things right."));
			return;
		}

		if (GameRandom.Get().NextDouble() >= 0.5)
		{
			await dialog.Msg(L("The demons are going to try and make an even stronger Kruvina. The new Kruvina won't run on humans, but on a source of divine power... the goddesses."));
			await dialog.Msg(L("The only hope I have left is on you. Please, go to Seir Rainforest... put a stop to the demons' plans."));
		}
		else
		{
			await dialog.Msg(L("I... I feel in you a power that's different from other Revelators. It makes me wonder if meeting you wasn't in fact the destiny granted to me by the goddesses..."));
		}
	}

	/// <summary>
	/// Inserts Melchioras' crystal into the closest reserve Kruvina device
	/// or Magic Power Supply Device.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CASTLE65_3_SQ01_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		UseCrystal(character);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Destroys the device the character stands at, if the crystal is
	/// full of demonic power.
	/// </summary>
	private static void UseCrystal(Character character)
	{
		double[,] targets;
		string destroyedVar, countVar;
		QuestId questId;

		if (character.Map.ClassName == "f_castle_65_3" && character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
		{
			targets = ReserveDevices;
			destroyedVar = ReserveDeviceVar;
			countVar = ReserveDevicesVar;
			questId = Sq01;
		}
		else if (character.Map.ClassName == "f_castle_65_2" && character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
		{
			targets = ManorPillars;
			destroyedVar = ManorPillarVar;
			countVar = ManorPillarsVar;
			questId = Sq02;
		}
		else
		{
			character.ServerMessage(L("The crystal does not react here."));
			return;
		}

		var number = 0;
		for (var i = 0; i < targets.GetLength(0); ++i)
		{
			if (character.Variables.Perm.GetBool(destroyedVar + (i + 1), false))
				continue;

			if (character.Position.Get2DDistance(new Position((float)targets[i, 0], character.Position.Y, (float)targets[i, 1])) <= CrystalRange)
			{
				number = i + 1;
				break;
			}
		}

		if (number == 0)
		{
			character.ServerMessage(L("There is no device nearby to insert the crystal into."));
			return;
		}

		if (character.Variables.Perm.GetInt(CrystalChargeVar, 0) < 100)
		{
			character.ServerMessage(L("The crystal needs more demonic power. Defeat demons to fill it."));
			return;
		}

		character.PlayEffect("F_explosion069_blue", 1f);
		character.Variables.Perm.SetInt(CrystalChargeVar, 0);
		character.Variables.Perm.Set(destroyedVar + number, true);

		var destroyed = character.Variables.Perm.GetInt(countVar, 0) + 1;
		character.Variables.Perm.SetInt(countVar, destroyed);
		character.ServerMessage(LF("Devices destroyed: {0}/{1}", Math.Min(destroyed, targets.GetLength(0)), targets.GetLength(0)));
		character.LookAround();
	}

	/// <summary>
	/// Picks Maroulu Grass for Connor, which grows back after a while.
	/// </summary>
	private static void GatherGrass(Character character, int number)
	{
		if (!character.Quests.IsActive(Rp2) || character.Quests.IsCompletable(Rp2))
			return;

		if (character.Inventory.CountItem(ItemId.CASTLE653_RP_2_ITEM) >= GrassNeeded)
			return;

		var pickedAt = character.Variables.Temp.GetLong(GrassVar + number, 0);
		if (pickedAt != 0 && DateTime.Now - new DateTime(pickedAt) < GrassRespawn)
		{
			character.ServerMessage(L("The Maroulu Grass here has already been picked."));
			return;
		}

		character.Variables.Temp.SetLong(GrassVar + number, DateTime.Now.Ticks);
		character.Inventory.Add(ItemId.CASTLE653_RP_2_ITEM, 1, InventoryAddType.PickUp);
	}

	/// <summary>
	/// Collects demon souls in Yane's shaman doll and charges Melchioras'
	/// crystal while the player fights.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Attacker is not Character character || args.Target is not Mob mob)
			return;

		var mapName = character.Map.ClassName;
		var className = mob.Data.ClassName;

		if (mapName == "f_castle_65_3" && OutskirtsDemons.Contains(className))
		{
			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Variables.Perm.SetInt(DollSoulsVar, Math.Min(100, character.Variables.Perm.GetInt(DollSoulsVar, 0) + SoulsPerKill));

			if (character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
				ChargeCrystal(character);
		}
		else if (mapName == "f_castle_65_2" && ManorDemons.Contains(className))
		{
			if (character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
				ChargeCrystal(character);
		}
	}

	/// <summary>
	/// Fills Melchioras' crystal with a defeated demon's power.
	/// </summary>
	private static void ChargeCrystal(Character character)
	{
		var charge = character.Variables.Perm.GetInt(CrystalChargeVar, 0);
		if (charge >= 100)
			return;

		charge = Math.Min(100, charge + ChargePerKill);
		character.Variables.Perm.SetInt(CrystalChargeVar, charge);

		if (charge >= 100)
			character.ServerMessage(L("The crystal is full of demonic power. Insert it into a device."));
		else
			character.ServerMessage(LF("Demonic power in the crystal: {0}%", charge));
	}

	/// <summary>
	/// Hands Melchioras' crystal over again, unless the character still
	/// carries it.
	/// </summary>
	private static void GiveCrystal(Character character)
	{
		if (character.Inventory.CountItem(ItemId.CASTLE65_3_SQ01_ITEM) == 0)
			character.Inventory.Add(ItemId.CASTLE65_3_SQ01_ITEM, 1, InventoryAddType.PickUp);
	}

	/// <summary>
	/// Returns whether Yane and Mihail are at the Tagika Crossroads.
	/// </summary>
	private static bool IsGroupAtCrossroads(Character character)
		=> (character.Quests.Has(Mq03) && !character.Quests.Has(Mq04)) || character.Quests.Has(Mq09);

	/// <summary>
	/// Returns whether the shaman doll has taken Melchioras' place on the
	/// altar.
	/// </summary>
	private static bool IsMelchiorasFreed(Character character)
		=> character.Quests.HasCompleted(Mq05) || (character.Quests.IsActive(Mq05) && character.Etc.Properties.GetFloat(Mq05TrackId) == 1);

	/// <summary>
	/// Returns whether the given reserve Kruvina device has been destroyed.
	/// </summary>
	private static bool IsReserveDeviceDestroyed(Character character, int number)
		=> character.Quests.HasCompleted(Sq01) || (character.Quests.IsActive(Sq01) && character.Variables.Perm.GetBool(ReserveDeviceVar + number, false));

	/// <summary>
	/// Returns whether the given Magic Power Supply Device at Delmore Manor
	/// has been destroyed with Melchioras' crystal.
	/// </summary>
	public static bool IsManorPillarDestroyed(Character character, int number)
		=> character.Quests.HasCompleted(Sq02) || (character.Quests.IsActive(Sq02) && character.Variables.Perm.GetBool(ManorPillarVar + number, false));

	/// <summary>
	/// Returns whether the given Magic Power Supply Device at Delmore Manor
	/// is still waiting to be destroyed.
	/// </summary>
	public static bool IsManorPillarStanding(Character character, int number)
		=> character.Quests.IsActive(Sq02) && !character.Variables.Perm.GetBool(ManorPillarVar + number, false);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 70440: Chasing Lord Delmore (1)
//-----------------------------------------------------------------------------
public class FCastle653Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70440);
		SetName(L("Chasing Lord Delmore (1)"));
		SetDescription(L("Revelator Yane wants to split the group to chase after Delmore Rephaim. Ask Revelator Mihail to accompany you."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_01_1", "f_castle_65_3", L("Talk to Revelator Yane"), L("Revelator Yane is in a hurry to chase after Lord Delmore accompanied by a party. Talk to her."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_01_2", "f_castle_65_3", L("Ask Revelator Mihail to accompany you"), L("Revelator Yane wants to split the group to chase after Delmore Rephaim. Ask Revelator Mihail to accompany you."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_02", "f_castle_65_3", L("Chase Delmore Rephaim to the Outskirts Central Plaza"), L("Follow Revelator Mihail and chase Delmore Rephaim up to the Outskirts Central Plaza."));

		AddPrerequisite(new QuestStatusPrerequisite(70424, QuestStatus.Completed));

		AddObjective("askMihail", L("Ask Revelator Mihail to accompany you"), new ManualObjective());
	}
}

// 70441: Chasing Lord Delmore (2)
//-----------------------------------------------------------------------------
public class FCastle653Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70441);
		SetName(L("Chasing Lord Delmore (2)"));
		SetDescription(L("You are caught in Delmore Rephaim's trap. Fight demons to find the hidden magic core or defeat all demons to escape."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_02", "f_castle_65_3", L("Chase Delmore Rephaim to the Outskirts Central Plaza"), L("Follow Revelator Mihail and chase Delmore Rephaim up to the Outskirts Central Plaza."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_02", "f_castle_65_3", L("Escape from Delmore Rephaim's trap"), L("You are caught in Delmore Rephaim's trap. Fight demons to find the hidden magic core or defeat all demons to escape."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_03", "f_castle_65_3", L("Talk to Revelator Mihail"), L("You have disabled the trap. Talk to Revelator Mihail."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CASTLE65_3_MQ02_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(70440, QuestStatus.Completed));

		AddObjective("breakTrap", L("Destroy the barrier"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 1050));
	}
}

// 70442: Unbreakable Barrier
//-----------------------------------------------------------------------------
public class FCastle653Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70442);
		SetName(L("Unbreakable Barrier"));
		SetDescription(L("Find Revelator Yane's group at the Tagika Crossroads and help them."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_03", "f_castle_65_3", L("Talk to Revelator Mihail"), L("Revelator Mihail is worried about Revelator Yane's party headed towards the Tagika Crossroads. Talk to him."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_03_1", "f_castle_65_3", L("Join Yane's group at the Tagika Crossroads"), L("Find Revelator Yane's group at the Tagika Crossroads and help them."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_04_1", "f_castle_65_3", L("Talk to Revelator Yane"), L("Revelator Mihail has disabled the barrier that was trapping Yane and her group. Ask Revelator Yane if she is all right."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CASTLE65_3_MQ03_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(70441, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the incoming demons"), new KillObjective(10, "PagEmitter") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 790));
	}
}

// 70443: To the Government Ruins
//-----------------------------------------------------------------------------
public class FCastle653Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70443);
		SetName(L("To the Government Ruins"));
		SetDescription(L("Go to the Government Ruins with Revelators Yane and Mihail."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_04_1", "f_castle_65_3", L("Talk to Revelator Yane"), L("Revelator Yane says she saw a suspicious light by the Government Ruins. Keep talking to her."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_05_1", "f_castle_65_3", L("Go to the Government Ruins"), L("Go to the Government Ruins with Revelators Yane and Mihail."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_05_1", "f_castle_65_3", L("Talk to Revelator Yane"), L("Mage Melchioras is trapped in the altar. Try and find a solution with Revelator Yane."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CASTLE65_3_MQ04_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(70442, QuestStatus.Completed));

		AddObjective("goToRuins", L("Go to the Government Ruins"), new ManualObjective());
	}
}

// 70444: The Shaman Doll and the Savior
//-----------------------------------------------------------------------------
public class FCastle653Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70444);
		SetName(L("The Shaman Doll and the Savior"));
		SetDescription(L("Yane wants to impart demons souls onto a shaman doll and replace Melchioras with it. Hunt demons and collect their souls on the doll."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_05_1", "f_castle_65_3", L("Talk to Revelator Yane"), L("Revelator Yane seems to have figured out a solution. Talk to her."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_05_1", "f_castle_65_3", L("Collect Demons Souls in the Shaman Doll"), L("Yane wants to impart demons souls onto a shaman doll and replace Melchioras with it. Hunt demons and collect their souls on the doll."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_05_1", "f_castle_65_3", L("Deliver to Revelator Yane"), L("The shaman doll is now fully charged with souls. Bring it to Revelator Yane."));

		SetTrack(QuestStatus.Success, QuestStatus.Success, "CASTLE65_3_MQ05_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(70443, QuestStatus.Completed));

		AddObjective("collectSouls", L("Collect Demons Souls in the Shaman Doll"), new VariableCheckObjective(FCastle653QuestNpcsScript.DollSoulsVar, 100, isPermanent: true));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 520));
		AddReward(new TakeItemReward("CASTLE65_3_MQ05_ITEM", -1));
	}
}

// 70445: Lead to a Dead-end (1)
//-----------------------------------------------------------------------------
public class FCastle653Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70445);
		SetName(L("Lead to a Dead-end (1)"));
		SetDescription(L("Mage Melchioras says Delmore Rephaim has escaped to the Odaginkas Vacant Lot. Go and chase Delmore Rephaim with Revelator Mihail."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_06", "f_castle_65_3", L("Talk to Mage Melchioras"), L("Mage Melchioras is still struggling but he is trying to tell you something. Talk to him."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_07_1", "f_castle_65_3", L("Chase Delmore Rephaim to the Odaginkas Vacant Lot"), L("Mage Melchioras says Delmore Rephaim has escaped to the Odaginkas Vacant Lot. Go and chase Delmore Rephaim with Revelator Mihail."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_07_1", "f_castle_65_3", L("Chase Delmore Rephaim to the Odaginkas Vacant Lot"), L("Mage Melchioras says Delmore Rephaim has escaped to the Odaginkas Vacant Lot. Go and chase Delmore Rephaim with Revelator Mihail."));

		AddPrerequisite(new QuestStatusPrerequisite(70444, QuestStatus.Completed));

		AddObjective("chaseRephaim", L("Chase Delmore Rephaim to the Odaginkas Vacant Lot"), new ManualObjective());
	}
}

// 70446: Lead to a Dead-end (2)
//-----------------------------------------------------------------------------
public class FCastle653Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70446);
		SetName(L("Lead to a Dead-end (2)"));
		SetDescription(L("Revelator Mihail wants to blow up the obstacle that's blocking the way with a bomb. Protect him from the incoming demon attacks while he finished setting up the bomb."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_07_1", "f_castle_65_3", L("Chase Delmore Rephaim to the Odaginkas Vacant Lot"), L("Go and chase Delmore Rephaim to the Odaginkas Vacant Lot with Revelator Mihail."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_07_2", "f_castle_65_3", L("Protect Mihail while he sets up the bomb"), L("Revelator Mihail wants to blow up the obstacle that's blocking the way with a bomb. Protect him from the incoming demon attacks while he finished setting up the bomb."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_07_2", "f_castle_65_3", L("Talk to Revelator Mihail"), L("Revelator Mihail has finished setting up the bomb. Talk to him."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CASTLE65_3_MQ07_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(70445, QuestStatus.Completed));

		AddObjective("protectMihail", L("Protect Mihail while he sets up the bomb"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 790));
	}
}

// 70447: Comforting Them
//-----------------------------------------------------------------------------
public class FCastle653Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70447);
		SetName(L("Comforting Them"));
		SetDescription(L("You've almost caught up with Delmore Rephaim now. Keep going and stop his plans!"));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_07_2", "f_castle_65_3", L("Talk to Revelator Mihail"), L("Revelator Mihail has finished setting up the bomb. Talk to him."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_07_2", "f_castle_65_3", L("Stop Delmore Rephaim"), L("You've almost caught up with Delmore Rephaim now. Keep going and stop his plans!"));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_07_2", "f_castle_65_3", L("Report to Revelator Mihail"), L("Delmore Rephaim tried to use the power of the Kruvina but was ultimately defeated. Return to Revelator Mihail."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CASTLE65_3_MQ08_TRACK", "m_boss_c", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(70446, QuestStatus.Completed));

		AddObjective("stopRephaim", L("Stop Delmore Rephaim"), new KillObjective(1, "boss_Rambandgad_red") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 1050));
	}
}

// 70448: This is Only the Beginning
//-----------------------------------------------------------------------------
public class FCastle653Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70448);
		SetName(L("This is Only the Beginning"));
		SetDescription(L("It's time to go back to where Melchioras and the Revelators are. Talk to Mage Melchioras at the Tagika Crossroads."));
		SetType(QuestType.Main);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_07_2", "f_castle_65_3", L("Talk to Revelator Mihail"), L("It's time to go back to where Melchioras and the Revelators are. Talk to Revelator Mihail."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_09", "f_castle_65_3", L("Talk to Mage Melchioras"), L("It's time to go back to where Melchioras and the Revelators are. Talk to Mage Melchioras at the Tagika Crossroads."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_09", "f_castle_65_3", L("Talk to Mage Melchioras"), L("It's time to go back to where Melchioras and the Revelators are. Talk to Mage Melchioras at the Tagika Crossroads."));

		AddPrerequisite(new QuestStatusPrerequisite(70447, QuestStatus.Completed));

		AddObjective("returnToMelchioras", L("Talk to Mage Melchioras"), new ManualObjective());
	}
}

// 70449: For It to Never Happen Again
//-----------------------------------------------------------------------------
public class FCastle653Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70449);
		SetName(L("For It to Never Happen Again"));
		SetDescription(L("Melchioras is worried about the backup Kruvina devices that are still around. Fill the crystal given to you by Melchioras with evil energy by defeating demons and place it on the devices."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_09", "f_castle_65_3", L("Talk to Mage Melchioras"), L("Mage Melchioras is apparently still having difficulties moving. Talk to him."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_SQ_01_1", "f_castle_65_3", L("Destroy all reserve Kruvina devices"), L("Melchioras is worried about the backup Kruvina devices that are still around. Fill the crystal given to you by Melchioras with evil energy by defeating demons and place it on the devices."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_09", "f_castle_65_3", L("Report to Mage Melchioras"), L("You have cleared out all the reserve devices. Go and tell Mage Melchioras."));

		AddPrerequisite(new QuestStatusPrerequisite(70448, QuestStatus.Completed));

		AddObjective("destroyDevices", L("Destroy the reserve Kruvina devices"), new VariableCheckObjective(FCastle653QuestNpcsScript.ReserveDevicesVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 790));
		AddReward(new TakeItemReward("CASTLE65_3_SQ01_ITEM", -1));
	}
}

// 70450: Corners Well Polished
//-----------------------------------------------------------------------------
public class FCastle653Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70450);
		SetName(L("Corners Well Polished"));
		SetDescription(L("Mage Melchioras has asked you to destroy the remaining Magic Power Supply Device in Delmore Manor. Go back there and defeat demons to charge the crystal with demonic power, then place it on the supply device."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_3", "f_castle_65_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_09", "f_castle_65_3", L("Talk to Mage Melchioras"), L("There still seems to be something concerning Mage Melchioras. Talk to Melchioras."));
		SetPhase(QuestStatus.InProgress, "CASTLE652_MQ_02_PILLAR", "f_castle_65_2", L("Inject demonic power into the Supply Device"), L("Mage Melchioras has asked you to destroy the remaining Magic Power Supply Device in Delmore Manor. Go back there and defeat demons to charge the crystal with demonic power, then place it on the supply device."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_09", "f_castle_65_3", L("Report to Mage Melchioras"), L("The Delmore Manor is now free of the Magic Power Supply Device. Go back and tell Mage Melchioras."));

		AddPrerequisite(new QuestStatusPrerequisite(70449, QuestStatus.Completed));

		AddObjective("destroyPillars", L("Destroy the remaining Magic Power Supply Devices at Delmore Manor"), new VariableCheckObjective(FCastle653QuestNpcsScript.ManorPillarsVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 1050));
		AddReward(new TakeItemReward("CASTLE65_3_SQ01_ITEM", -1));
	}
}

// 70451: Finding Clues to the Plot
//-----------------------------------------------------------------------------
public class FCastle653Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70451);
		SetName(L("Finding Clues to the Plot"));
		SetDescription(L("Revelator Yane wants you to help clear out some demons. Defeat demons nearby."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_04_1", "f_castle_65_3", L("Talk to Revelator Yane"), L("It seems Revelator Yane has not yet fully recovered. Talk to her."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_04_1", "f_castle_65_3", L("Defeat nearby demons"), L("Revelator Yane wants you to help clear out some demons. Defeat demons nearby."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_04_1", "f_castle_65_3", L("Report to Revelator Yane"), L("You have cleared out some of the demons nearby. Go and tell Revelator Yane."));

		AddPrerequisite(new QuestStatusPrerequisite(70448, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat nearby demons"), new KillObjective(24, "PagNurse", "PagEmitter", "PagDoper"));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 520));
	}
}

// 70452: Thorough Procedure
//-----------------------------------------------------------------------------
public class FCastle653Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(70452);
		SetName(L("Thorough Procedure"));
		SetDescription(L("Mage Melchioras seems interested in the magic amplifiers. Obtain some magic amplifiers from Black Maizes."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Talk to Revelator Mihail"), L("Revelator Mihail seems to have something on his mind. Talk to Revelator Mihail."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Collect Magic Amplifiers from Black Maizes"), L("Mage Melchioras seems interested in the magic amplifiers. Obtain some magic amplifiers from Black Maizes."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Deliver to Revelator Mihail"), L("You have collected enough magic amplifiers. Bring them to Mage Melchioras."));

		AddPrerequisite(new QuestStatusPrerequisite(70448, QuestStatus.Completed));

		AddObjective("collectAmplifiers", L("Collect Magic Amplifiers from Black Maizes"), new CollectItemObjective("CASTLE65_3_SQ04_ITEM", 16));
		AddPityDrop("CASTLE65_3_SQ04_ITEM", 0.75f, 3, 1, "Sec_Zibu_Maize");

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 520));
		AddReward(new TakeItemReward("CASTLE65_3_SQ04_ITEM", -1));
	}
}

// 60177: Dangerous Distraction
//-----------------------------------------------------------------------------
public class FCastle653Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60177);
		SetName(L("Dangerous Distraction"));
		SetDescription(L("Revelator Mihail has asked you to defeat the Pag Emitters that are attacking people at Delmore Outskirts."));
		SetType(QuestType.Repeat);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Talk to Revelator Mihail"), L("Revelator Mihail is waiting for help at Delmore Outskirts."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Defeat the Pag Emitter"), L("Revelator Mihail has asked you to defeat the Pag Emitters that are attacking people at Delmore Outskirts."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Report to Revelator Mihail"), L("You have defeated enough Pag Emitters. Report back to Revelator Mihail."));

		AddPrerequisite(new QuestStatusPrerequisite(70448, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(74));

		AddObjective("killEmitters", L("Defeat Pag Emitter"), new KillObjective(12, "PagEmitter"));

		AddReward(new ItemReward("expCard5", 2));
	}
}

// 60178: Good Day to Recover
//-----------------------------------------------------------------------------
public class FCastle653Rp2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60178);
		SetName(L("Good Day to Recover"));
		SetDescription(L("Revelator Connor asked you to collect Maroulu Herbs Grass from the Fortress walls in order for the people with Melchioras to recover."));
		SetType(QuestType.Repeat);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_04_3", "f_castle_65_3", L("Talk to Revelator Connor"), L("Revelator Connor is waiting for help at Delmore Outskirts."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_RP_2_OBJ", "f_castle_65_3", L("Collect Maroulu Herbs"), L("Revelator Connor asked you to collect Maroulu Herbs Grass from the Fortress walls in order for the people with Melchioras to recover."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_04_3", "f_castle_65_3", L("Report back to Revelator Connor"), L("You have gathered enough Maroulu Herbs. Go back and hand them to Revelator Connor."));

		AddPrerequisite(new QuestStatusPrerequisite(70448, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(74));

		AddObjective("collectGrass", L("Collect Maroulu Grass"), new CollectItemObjective("CASTLE653_RP_2_ITEM", 6));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new TakeItemReward("CASTLE653_RP_2_ITEM", -1));
	}
}

// 50268: Best of the Best
//-----------------------------------------------------------------------------
public class FCastle653Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50268);
		SetName(L("Best of the Best"));
		SetDescription(L("Revelator Mihail wants to know about your recent adventures. Tell him stories about your travels."));
		SetType(QuestType.Sub);
		SetLocation("f_castle_65_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Talk to Revelator Mihail"), L("Revelator Mihail seems eager to greet you. Talk to Revelator Mihail."));
		SetPhase(QuestStatus.InProgress, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Tell Revelator Mihail About Your Adventures"), L("Revelator Mihail wants to know about your recent adventures. Tell him stories about your travels."));
		SetPhase(QuestStatus.Success, "CASTLE653_MQ_04_2", "f_castle_65_3", L("Talk to Revelator Mihail"), L("Talk to Revelator Mihail."));

		AddPrerequisite(new QuestStatusPrerequisite(70448, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(8498, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(60042, QuestStatus.Completed));

		AddObjective("tellStories", L("Tell Revelator Mihail About Your Adventures"), new ManualObjective());

		AddReward(new ItemReward("Collection_Base_CASTLE65_3_HQ1", 1));
	}
}
