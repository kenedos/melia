//--- Melia Script ----------------------------------------------------------
// Letas Stream Quest NPCs
//--- Description -----------------------------------------------------------
// The Guide Owl Sculpture and Liaison Officer Mardas, and the demons'
// Soul Starvation drawing the forest's spirits.
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

public class FKatyn12QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(30060);
	private readonly static QuestId Mq02 = new QuestId(30061);
	private readonly static QuestId Mq03 = new QuestId(30062);
	private readonly static QuestId Mq04 = new QuestId(30063);
	private readonly static QuestId Mq05 = new QuestId(30064);
	private readonly static QuestId Mq06 = new QuestId(30065);
	private readonly static QuestId Mq07 = new QuestId(30066);
	private readonly static QuestId Mq08 = new QuestId(30067);
	private readonly static QuestId Mq09 = new QuestId(30068);
	private readonly static QuestId Mq10 = new QuestId(30069);
	private readonly static QuestId Sq01 = new QuestId(30072);
	private readonly static QuestId Sq02 = new QuestId(30073);
	private readonly static QuestId Hq1 = new QuestId(50271);
	private readonly static QuestId Rp1 = new QuestId(60165);

	public const string PurifyCountVar = "Gabija.Quests.Katyn12Mq03.Purified";
	public const string EvilCountVar = "Gabija.Quests.Katyn12Mq05.Evil";
	public const string BlackCountVar = "Gabija.Quests.Katyn12Mq07.Black";
	private const string EvilVar = "Gabija.Quests.Katyn12Mq05.EvilSpot";
	private const string BlackVar = "Gabija.Quests.Katyn12Mq07.BlackSpot";
	private const string TreeTriesVar = "Gabija.Quests.Katyn12Mq02.Tries";
	private const string CircleOrderVar = "Gabija.Quests.Katyn12Mq08.Order";
	private const string CircleStepVar = "Gabija.Quests.Katyn12Mq08.Step";
	private const string IntroVar = "Gabija.Quests.Katyn12Mq06.Intro";
	private const string SpiritVar = "Gabija.Quests.Katyn12Rp1.Spirit";

	private const int PurifyNeeded = 5;
	private const int EvilNeeded = 5;
	private const int TreeTries = 3;
	private const float NamottRange = 100;

	private static readonly TimeSpan SpiritRespawn = TimeSpan.FromSeconds(30);
	private static readonly Position SacredTreeSpot = new Position(-2890f, 505f, 1520f);
	private static readonly Position SphereCheckpoint = new Position(330f, 249.46f, -1070f);

	private static readonly string[] Monsters = { "puragi", "Sec_zombiegirl2_chpel", "jellyfish_blue", "zigri", "chupacabra_green", "operor_blue" };

	private static readonly double[,] EvilEnergy =
	{
		{ -331.65, 279.21 }, { -576.15, 239.95 }, { -317.78, 526.51 }, { -394.26, 753.11 },
		{ -717.41, 511.95 }, { -901.92, 586.22 }, { -617.42, 767.54 }, { -880.60, 302.73 },
	};

	private static readonly double[,] BlackSpirits =
	{
		{ 1350.51, -2265.04 }, { 2582.92, -2215.71 }, { 3029.99, -1076.11 },
	};

	private static readonly double[,] Spheres =
	{
		{ 537.04, -1075.51 }, { 593.05, -1168.77 }, { 666.32, -1134.07 }, { 748.46, -1179.61 }, { 847.45, -1135.11 },
		{ 983.87, -1016.69 }, { 1044.77, -1141.75 }, { 1181.69, -1093.77 }, { 1247.25, -1187.36 }, { 1348.09, -1212.90 },
	};

	private static readonly double[,] Circles =
	{
		{ 1851.64, 263.37 }, { 1839.33, 121.95 }, { 1997.83, 296.05 }, { 1961.72, 63.06 }, { 2069.30, 182.51 },
	};

	private static readonly double[,] FadingSpirits =
	{
		{ 203.88, 1561.39 }, { 205.94, 1632.85 }, { 130.13, 1515.84 }, { -0.50, 1106.51 }, { 128.21, 900.92 },
		{ 345.54, 998.68 }, { 630.50, 1100.81 }, { 812.58, 1448.76 }, { 980.92, 1489.78 }, { 1283.29, 1721.09 },
		{ 1523.44, 1780.84 }, { 1567.12, 1499.29 }, { 1329.94, 1198 }, { 1382.19, 1053.55 }, { 1186.52, 965.56 },
		{ 1026.70, 797.61 }, { 677.79, 535.80 }, { 692.06, 361.86 }, { 1011.45, 193.78 }, { 961.47, 33.24 },
		{ 860.29, 782.42 }, { -92.42, 1021.48 },
	};

	protected override void Load()
	{
		// Guide Owl Sculpture
		//-------------------------------------------------------------------------
		AddNpc(48004, L("Guide Owl"), "KATYN_12_NPC_01", "f_katyn_12", -1937.02, -1180.28, 0, this.GuideOwl);

		// Liaison Officer Mardas
		//-------------------------------------------------------------------------
		AddConditionalNpc(151077, L("Liaison Officer Mardas"), "KATYN_12_NPC_02", "f_katyn_12", -128.54, -936.37, 45, c => c.Quests.Has(Mq04) && !c.Quests.HasCompleted(Mq10), this.Mardas);

		// Evil energy on Svaigulys Hill
		//-------------------------------------------------------------------------
		for (var i = 0; i < EvilEnergy.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(147469, L("Evil Energy"), "KATYN_12_OBJ_02_" + number, "f_katyn_12", EvilEnergy[i, 0], EvilEnergy[i, 1], 90,
				character => IsEvilEnergyActive(character, number),
				async dialog =>
				{
					this.SuppressEvilEnergy(dialog.Player, number, dialog.Npc);
					await Task.CompletedTask;
				});
		}

		// Senyvas Yard, watched by the Surveillance Spheres
		//-------------------------------------------------------------------------
		AddQuestTrigger("KATYN_12_MQ_06_TRIGGER", "f_katyn_12", 406.22, -1070.43, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			if (!character.Variables.Temp.GetBool(IntroVar, false))
			{
				character.Variables.Temp.Set(IntroVar, true);
				character.SetEtcProperty("KATYN_12_MQ_06_TRACK", 0);
				await character.Tracks.Start("KATYN_12_MQ_06_TRACK", TimeSpan.Zero);
			}
		});

		for (var i = 0; i < Spheres.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(151051, "UnvisibleName", "KATYN_12_MQ_06_SPHERE_" + number, "f_katyn_12", Spheres[i, 0], Spheres[i, 1], 90, IsSneakingPastSpheres);

			AddQuestTrigger("KATYN_12_MQ_06_DETECT_" + number, "f_katyn_12", Spheres[i, 0], Spheres[i, 1], 45, async args =>
			{
				if (args.Initiator is not Character character || !IsSneakingPastSpheres(character))
					return;

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("You were spotted by a Surveillance Sphere!"), 3);
				character.Warp("f_katyn_12", SphereCheckpoint);

				await Task.CompletedTask;
			});
		}

		AddConditionalNpc(147469, L("Surveillance Sphere Magic Circle"), "KATYN_12_OBJ_06", "f_katyn_12", 1560.39, -1268.51, 90, IsSneakingPastSpheres, async dialog =>
		{
			var character = dialog.Player;

			if (!IsSneakingPastSpheres(character))
				return;

			dialog.Npc.PlayEffect("F_explosion014", 1.5f);
			character.Quests.CompleteObjective(Mq06, "removeCircle");
			character.LookAround();

			await Task.CompletedTask;
		});

		// Black Spirits at the end of Senyvas Yard's forked roads
		//-------------------------------------------------------------------------
		for (var i = 0; i < BlackSpirits.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(147469, L("Black Spirit"), "KATYN_12_OBJ_03_" + number, "f_katyn_12", BlackSpirits[i, 0], BlackSpirits[i, 1], 90,
				character => IsBlackSpiritActive(character, number),
				async dialog =>
				{
					this.SuppressBlackSpirit(dialog.Player, number, dialog.Npc);
					await Task.CompletedTask;
				});
		}

		// Protection Magic Circles around the Soul Starvation
		//-------------------------------------------------------------------------
		for (var i = 0; i < Circles.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(147469, L("Protection Magic Circle"), "KATYN_12_OBJ_04_" + number, "f_katyn_12", Circles[i, 0], Circles[i, 1], 90,
				character => character.Quests.IsActive(Mq08) && !character.Quests.IsCompletable(Mq08),
				async dialog =>
				{
					await this.DisableCircle(dialog, number);
				});
		}

		// Soul Starvation
		//-------------------------------------------------------------------------
		AddConditionalNpc(151065, L("Soul Starvation"), "KATYN_12_OBJ_05", "f_katyn_12", 1941.51, 183.77, 90, c => !c.Quests.HasCompleted(Mq09), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.IsCompletable(Mq09))
			{
				var placed = await character.TimeActions.StartAsync(L("Setting the Namott of Suppression"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));
				if (placed != TimeActionResult.Completed)
					return;

				dialog.Npc.PlayEffect("F_explosion014", 2f);
				character.ServerMessage(L("The Namott of Suppression shatters the Soul Starvation, and the trapped souls are set free."));
				await dialog.CompleteQuest(Mq09);

				if (character.Quests.HasCompleted(Mq09))
					character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq09))
				character.Quests.ReplayQuestTrack(Mq09);
		});

		// Letter on the Ground
		//-------------------------------------------------------------------------
		AddConditionalNpc(147312, L("Letter on the Ground"), "KATYN_12_SQ_NPC_01", "f_katyn_12", 48.41, 1527.23, 90, c => !c.Quests.Has(Sq01) && !c.Quests.HasCompleted(Sq01), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.Has(Sq01) || !character.Quests.MeetsPrerequisites(Sq01))
				return;

			var picked = await character.TimeActions.StartAsync(L("Picking up the letter"), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(1));
			if (picked != TimeActionResult.Completed)
				return;

			character.Quests.Start(Sq01);
			character.Quests.CompleteObjective(Sq01, "deliverLetter");
			character.Inventory.Add(ItemId.KATYN_12_SQ_01_ITEM, 1, InventoryAddType.PickUp);
			character.PlayEffect("F_pc_making_finish_white", 1f);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("You've found a letter left by someone{nl}Look for the recipient"), 10);
			character.LookAround();
		});

		// Letas Altar
		//-------------------------------------------------------------------------
		AddNpc(46213, L("Letas Altar"), "KATYN_12_SQ_NPC_02", "f_katyn_12", -2857.33, 109.31, 90, async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var looked = await character.TimeActions.StartAsync(L("Looking at the altar"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));
				if (looked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Sq02);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The energy of the Letas Altar is suppressed by the{nl}malicious force of the nearby monsters{nl}Purge them of their sins!"), 10);
				return;
			}

			if (character.Quests.IsActive(Sq02))
				character.ServerMessage(L("The altar's energy is still suppressed by the nearby monsters."));
		});

		// Frail Owl Sculpture
		//-------------------------------------------------------------------------
		AddNpc(48004, L("Frail Owl Sculpture"), "KATYN12_RP_1_NPC", "f_katyn_12", 1196.35, 736.18, 1, this.FrailOwl);

		for (var i = 0; i < FadingSpirits.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(147469, L("Fading Spirit"), "KATYN12_RP_1_OBJ_" + number, "f_katyn_12", FadingSpirits[i, 0], FadingSpirits[i, 1], 90,
				character => character.Quests.IsActive(Rp1) && !character.Quests.IsCompletable(Rp1),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Rp1) || character.Quests.IsCompletable(Rp1))
						return;

					var takenAt = character.Variables.Temp.GetLong(SpiritVar + number, 0);
					if (takenAt != 0 && DateTime.Now - new DateTime(takenAt) < SpiritRespawn)
					{
						character.ServerMessage(L("The spirit here has already been gathered."));
						return;
					}

					character.Variables.Temp.SetLong(SpiritVar + number, DateTime.Now.Ticks);
					character.Inventory.Add(ItemId.KATYN12_RP_1_ITEM, 1, InventoryAddType.PickUp);

					await Task.CompletedTask;
				});
		}

		// Lost Spirit
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Lost Spirit"), "KATYN12_HQ1_NPC", "f_katyn_12", 1837.78, 396.38, 90, c => c.Quests.HasCompleted(Mq10) && !c.Quests.Has(Hq1) && !c.Quests.HasCompleted(Hq1), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Lost Spirit"));

			if (character.Quests.Has(Hq1) || !character.Quests.MeetsPrerequisites(Hq1))
			{
				character.ServerMessage(L("The spirit seems to be eagerly looking for something."));
				return;
			}

			await dialog.Msg(L("My ring... The ring I exchanged with Sarah for our wedding vows..."));
			await dialog.Msg(L("Thank you for finding it. I can go in peace now."));

			var answer = await dialog.SelectQuestOffer(Hq1, L("But, where do I go...?"),
				Option(L("I'll bring any spirits to the Frail Owl Sculpture."), "accept"),
				Option(L("Well, go luck on your own. See ya."), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Hq1);
				character.LookAround();

				await dialog.Msg(L("Oh, if you guide me to the Owl Statue, I'll gladly follow."));
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Bring the lost spirit to the Frail Owl Sculpture."), 7);
			}
		});
	}

	/// <summary>
	/// The Guide Owl Sculpture's dialog.
	/// </summary>
	private async Task GuideOwl(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Guide Owl Sculpture"));

		if (character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("Ugh... What happened... Who are you? Did you save me?"));
			await dialog.Msg(L("You have my utmost gratitude. Right before I lost consciousness, I saw the image of a goddess, it must have been you..."));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (character.Quests.IsCompletable(Mq02))
		{
			await dialog.Msg(L("How did it go? Did you find the tree? I wondered what should we do if the tree does not appear even with the energy of the forest."));
			await dialog.Msg(L("What a relief! Perhaps, it was no luck but rather you are the special one."));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("Is the branch all filled up? Let me make it into the Namott of Suppression."));
			dialog.Npc.PlayEffect("F_light003_blue", 0.4f);
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (character.Quests.IsCompletable(Mq10))
		{
			var talked = await character.TimeActions.StartAsync(L("Talking"), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
			if (talked != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("Perhaps, this place is free of demons after all this time? My words cannot thank you enough!"));
			await dialog.Msg(L("I do not believe it is no coincidence that you came to us. I sense the divine will of the goddess guiding you here. I pray she will continue to guide you."));
			await dialog.Msg(L("On behalf of the forest and all the owls, I pray for the divine guidance and protection to be onto you. Farewell, my friend..."));
			await dialog.CompleteQuest(Mq10);

			if (character.Quests.HasCompleted(Mq10))
				character.LookAround();
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			var delivered = await character.TimeActions.StartAsync(L("Transmitting the energy of Karolis Springs"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(3));
			if (delivered != TimeActionResult.Completed)
				return;

			dialog.Npc.PlayEffect("F_bg_light009_yellow2", 1f);
			dialog.Npc.PlayEffect("F_bg_light010_yellow2", 1.5f);
			character.Quests.CompleteObjective(Mq01, "deliverEnergy");
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			await dialog.Msg(L("The Owl Chief sent you, eh? The Namott of Suppression... Now that's an extreme measure. The situation must be that dire."));
			await dialog.Msg(L("The Namott of Suppression is not something to be made lightly... But for all of us, I will try."));
			await dialog.Msg(L("The first ingredient we need is Letas' Sacred Branch. It's a branch from Letas' Sacred Tree."));

			var answer = await dialog.SelectQuestOffer(Mq02, L("However, the energy of the forest is needed to find Letas' Sacred Tree..."),
				Option(L("Show the crystal formed by the energy from Karolis Springs"), "accept"),
				Option(L("I don't think it can be helped"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(TreeTriesVar, 0);
				character.Quests.Start(Mq02);

				if (character.Inventory.CountItem(ItemId.KATYN_10_MQ_11_ITEM) == 0)
					character.Inventory.Add(ItemId.KATYN_10_MQ_11_ITEM, 1, InventoryAddType.PickUp);

				await dialog.Msg(L("Wha...? You already had it on you? That is great news. Bring it to the upper side of Atmesti Uphill and sprinkle the energy around."));
				await dialog.Msg(L("It might not be easy to spot at the first try. You need to keep spreading the energy around to make the tree appear."));
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("It seems that Letas' Sacred Branch is weakened by the demons. Its power is insufficient. We need to fill it up."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("I believe that utilizing the malicious force of the monsters would be a good idea. Use it on monsters to purify their energies and absorb them."),
				Option(L("Take the branch and go"), "accept"),
				Option(L("Are you absolutely sure?"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.SetInt(PurifyCountVar, 0);
				character.Quests.Start(Mq03);

				await dialog.Msg(L("The weak ones will not be able to stand the awesome might of the branch. They will burn like a piece of paper."));
			}
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			await dialog.Msg(L("The Namott is complete but... I am not confident it will be enough to combat the demons."));

			var answer = await dialog.SelectQuestOffer(Mq04, L("I, myself, am not sure what the Owl Chief was thinking. I too have lost most of my power, I cannot help you."),
				Option(L("Talk about Mardas"), "accept"),
				Option(L("It worries me too"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);
				character.Quests.CompleteObjective(Mq04, "findMardas");
				character.LookAround();

				await dialog.Msg(L("There is someone who can help you on your mission? What are you waiting for? Go and find out the plan."));
				await dialog.Msg(L("Perhaps, you don't have to go after all."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("Letas' Sacred Tree is a physical manifestation of the forest's will. Before the demon invasion, it was not a rare sight to see but now it is."));
			await dialog.Msg(L("However, if the energy of the forest is restored, they will reappear."));
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("The Owl Chief must trust you very much, seeing that it sent you."));
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("It is a blessing that that Mardas person is helping us out. But the story Mardas told is a very depressing one."));
			await dialog.Msg(L("We are in some ways responsible for such an atrocity. I feel guilty."));
			return;
		}

		if (character.Quests.HasCompleted(Mq10))
		{
			await dialog.Msg(L("Please talk to our Owl Chief about it as well. I'm certain it will be a pleasure to meet you."));
			return;
		}

		if (character.Quests.HasCompleted(Mq01))
		{
			await dialog.Msg(L("You should go see a man named Mardas. I'm sure he has the answer."));
			return;
		}

		await dialog.Msg(L("(There is no reaction.)"));
	}

	/// <summary>
	/// Liaison Officer Mardas' dialog at Letas Stream.
	/// </summary>
	private async Task Mardas(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Liaison Officer Mardas"));
		dialog.SetPortrait("Dlg_port_Mardas");

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("You are just in time. Did you get the Namott of Suppression?"));
			await dialog.CompleteQuest(Mq04);
			return;
		}

		if (character.Quests.IsCompletable(Mq05))
		{
			await dialog.Msg(L("The evil energy is continuously spawning? It's more serious than I thought."));
			await dialog.Msg(L("Our priority is stopping the spot where the energy of the forest is getting drained. The spot is at Senyvas Yard."));
			await dialog.CompleteQuest(Mq05);
			return;
		}

		if (character.Quests.IsCompletable(Mq07))
		{
			await dialog.Msg(L("Are you hurt? It must have been a very dangerous journey, I can imagine... I was just standing here feeling powerless."));
			await dialog.CompleteQuest(Mq07);
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("By talking with the Chief Owl Sculpture, I got the feeling we could work this out."));
			await dialog.Msg(L("The demons are using the evil energy to corrupt Letas Stream and to trap souls. A thing called Soul Starvation is keeping the souls locked up. It must be destroyed but.."));

			var answer = await dialog.SelectQuestOffer(Mq05, L("The evil energy is far too thick and it is protected by the magic circles. Removing the evil energy corrupting the forest would be first in order."),
				Option(L("Where is this evil energy?"), "accept"),
				Option(L("That sounds dangerous; I'd rather not."), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= EvilEnergy.GetLength(0); ++i)
					character.Variables.Perm.Set(EvilVar + i, false);
				character.Variables.Perm.SetInt(EvilCountVar, 0);

				character.Quests.Start(Mq05);
				character.LookAround();

				await dialog.Msg(L("You will find the evil energy concentrated on Svaigulys Hill. Set up the Namott of Suppression around it to remove the evil energy."));
			}
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			await dialog.Msg(L("The Surveillance Spheres are guarding Senyvas Yard, but without the Black Spirit, the demons' installation will weaken. Approaching it won't be easy."));
			await dialog.Msg(L("It's rather risky but... I want you to infiltrate the place without getting detected by the Surveillance Spheres spinning around the Surveillance Eyes."));

			var answer = await dialog.SelectQuestOffer(Mq06, L("At the end of the path, there is a magic circle managing those eyes. If the magic circle is disabled, the Surveilance Spheres might be disabled with it as well."),
				Option(L("Trust me"), "accept"),
				Option(L("I need some time to prepare"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Temp.Set(IntroVar, false);
				character.Quests.Start(Mq06);
				character.LookAround();

				await dialog.Msg(L("The Black Spirit can be removed with the Namott of Suppression, if we can deal with the Surveilance Spheres. Be careful."));
				await dialog.Msg(L("This mission is very dangerous."));
			}
			return;
		}

		if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
		{
			await dialog.Msg(L("Now, the Soul Starvation is vulnerable. The magic circles around it are protecting it but they can be easily disabled."));
			await dialog.Msg(L("There is a certain order in disabling the protection magic circles. One false step and you have to start all over again from the beginning."));

			var answer = await dialog.SelectQuestOffer(Mq08, L("If you remain focused even in the face of failure, you will find the order."),
				Option(L("Let's finish this once and for all"), "accept"),
				Option(L("I need to lie down a bit"), "leave")
			);

			if (answer == "accept")
			{
				var order = Enumerable.Range(1, Circles.GetLength(0)).OrderBy(_ => GameRandom.Get().Next()).ToArray();
				character.Variables.Perm.SetString(CircleOrderVar, string.Join("", order));
				character.Variables.Perm.SetInt(CircleStepVar, 0);

				character.Quests.Start(Mq08);
				character.LookAround();

				await dialog.Msg(L("The Soul Starvation is on the lower side of Apsvaiges Path. The evil energy has weakened and the Black Smoke is gone."));
				await dialog.Msg(L("The demons, if they are not completely stupid, will notice these changes and come after us. Please, hurry up."));
			}
			return;
		}

		if (!character.Quests.Has(Mq10) && character.Quests.MeetsPrerequisites(Mq10))
		{
			await dialog.Msg(L("Is that so... The souls are free now. The Owl Chief promised to guide the souls to their rightful place, into the arms of the goddess."));
			await dialog.Msg(L("The Namott of Suppression is best left to the Guide Owl. It will help the owls regain their power."));

			var answer = await dialog.SelectQuestOffer(Mq10, L("Then, I must go back to the Owl Chief. I will deliver this good news and want to hear more about it."),
				Option(L("Alright"), "accept"),
				Option(L("I will think about it"), "leave")
			);

			if (answer == "accept")
			{
				var talked = await character.TimeActions.StartAsync(L("Talking"), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
				if (talked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq10);
				character.Quests.CompleteObjective(Mq10, "reportMardas");
			}
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("When I think about the folks back home, I cannot bear it any longer but.. I am powerless. Helping you is my way of doing 'something at all'."));
			return;
		}

		if (character.Quests.IsActive(Mq06) || character.Quests.IsActive(Mq07))
		{
			await dialog.Msg(L("The Owl Chief sure did know a lot. It is wise and knowledgeable even though it never left this place in its entire existence."));
			return;
		}

		if (character.Quests.IsActive(Mq08) || character.Quests.IsActive(Mq09))
		{
			await dialog.Msg(L("If we manage to set those souls free, will the guilt and sadness I feel can be set free as well? But what about my rage?"));
			await dialog.Msg(L("I do not know at the moment..."));
			return;
		}

		if (character.Quests.IsActive(Mq10))
		{
			await dialog.Msg(L("How can we ever repay you... I apologize for my initial rudeness. I was very confused."));
			return;
		}

		await dialog.Msg(L("I have nothing left to lose. I don't want to run away anymore."));
	}

	/// <summary>
	/// The Frail Owl Sculpture's dialog.
	/// </summary>
	private async Task FrailOwl(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Frail Owl Sculpture"));

		if (character.Quests.IsCompletable(Hq1))
		{
			await dialog.Msg(L("Thanks to you, another wandering spirit is now going home."));
			await dialog.CompleteQuest(Hq1);
			return;
		}

		if (character.Quests.IsCompletable(Rp1))
		{
			await dialog.Msg(L("If I wasn't hidden among the trees like this, who knows if the monsters wouldn't have taken me too."));
			await dialog.Msg(L("But I will guide the spirits for as long as I can. Thank you for helping."));
			await dialog.CompleteQuest(Rp1);
			return;
		}

		if (character.Quests.IsActive(Hq1))
		{
			dialog.Npc.PlayEffect("F_light018_yellow", 1f);
			await dialog.Msg(L("Some spirits are still attached to a lost object and end up staying behind. They'll linger behind until their belongings are found."));
			character.Quests.CompleteObjective(Hq1, "guideSpirit");
			return;
		}

		if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
		{
			var answer = await dialog.SelectQuestOffer(Rp1, L("The spirits I need to collect are all scattered, in fear of the monsters. Would you find these frightened spirits and bring them back to me?"),
				Option(L("I'll help you"), "accept"),
				Option(L("I'm busy"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Rp1);
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Rp1))
		{
			await dialog.Msg(L("I miss the old, peaceful times..."));
			return;
		}

		await dialog.Msg(L("I wish I could do something for the spirits, like you..."));
	}

	/// <summary>
	/// Returns whether the given evil energy on Svaigulys Hill still has
	/// to be suppressed.
	/// </summary>
	private static bool IsEvilEnergyActive(Character character, int number)
		=> character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05) && !character.Variables.Perm.GetBool(EvilVar + number, false);

	/// <summary>
	/// Returns whether the given Black Spirit still has to be removed.
	/// </summary>
	private static bool IsBlackSpiritActive(Character character, int number)
		=> character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07) && !character.Variables.Perm.GetBool(BlackVar + number, false);

	/// <summary>
	/// Returns whether the character is sneaking past the Surveillance
	/// Spheres of Senyvas Yard.
	/// </summary>
	private static bool IsSneakingPastSpheres(Character character)
		=> character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06);

	/// <summary>
	/// Sets up the Namott of Suppression at one of the evil energies.
	/// </summary>
	private void SuppressEvilEnergy(Character character, int number, IActor target)
	{
		if (!IsEvilEnergyActive(character, number))
			return;

		if (character.Inventory.CountItem(ItemId.KATYN_12_MQ_03_ITEM) == 0)
		{
			character.ServerMessage(L("You need the Namott of Suppression."));
			return;
		}

		character.Variables.Perm.Set(EvilVar + number, true);
		var suppressed = character.Variables.Perm.GetInt(EvilCountVar, 0) + 1;
		character.Variables.Perm.SetInt(EvilCountVar, suppressed);

		target?.PlayEffect("F_light003_blue", 1f);
		character.ServerMessage(LF("Evil energy removed: {0}/{1}", Math.Min(suppressed, EvilNeeded), EvilNeeded));
		character.LookAround();
	}

	/// <summary>
	/// Removes one of the Black Spirits with the Namott of Suppression.
	/// </summary>
	private void SuppressBlackSpirit(Character character, int number, IActor target)
	{
		if (!IsBlackSpiritActive(character, number))
			return;

		if (character.Inventory.CountItem(ItemId.KATYN_12_MQ_03_ITEM) == 0)
		{
			character.ServerMessage(L("You need the Namott of Suppression."));
			return;
		}

		character.Variables.Perm.Set(BlackVar + number, true);
		var removed = character.Variables.Perm.GetInt(BlackCountVar, 0) + 1;
		character.Variables.Perm.SetInt(BlackCountVar, removed);

		target?.PlayEffect("F_light003_blue", 1f);
		character.ServerMessage(LF("Black Spirits removed: {0}/{1}", Math.Min(removed, BlackSpirits.GetLength(0)), BlackSpirits.GetLength(0)));
		character.LookAround();
	}

	/// <summary>
	/// Disables one of the magic circles protecting the Soul Starvation,
	/// which only holds when they are disabled in the right order.
	/// </summary>
	private async Task DisableCircle(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Mq08) || character.Quests.IsCompletable(Mq08))
			return;

		var order = character.Variables.Perm.GetString(CircleOrderVar, "12345");
		var step = character.Variables.Perm.GetInt(CircleStepVar, 0);

		if (step >= order.Length || order[step] - '0' != number)
		{
			character.Variables.Perm.SetInt(CircleStepVar, 0);
			dialog.Npc.PlayEffect("F_spread_out004_dark", 1f);
			character.ServerMessage(L("The magic circles have activated again."));
			return;
		}

		step++;
		character.Variables.Perm.SetInt(CircleStepVar, step);
		dialog.Npc.PlayEffect("F_light018_yellow", 1f);
		character.ServerMessage(LF("Magic circles disabled: {0}/{1}", step, order.Length));

		if (step >= order.Length)
			character.Quests.CompleteObjective(Mq08, "disableCircles");

		await Task.CompletedTask;
	}

	/// <summary>
	/// Sprinkles the energy of Karolis Springs on Atmesti Uphill until
	/// Letas' Sacred Tree shows itself.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_KATYN_10_MQ_11_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_katyn_12" || !character.Quests.IsActive(Mq02) || character.Quests.IsCompletable(Mq02))
		{
			character.ServerMessage(L("The crystal's energy does not react here."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Position.Get2DDistance(SacredTreeSpot) > 300)
		{
			character.ServerMessage(L("Sprinkle the energy on the upper side of Atmesti Uphill."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.PlayEffect("F_bg_light009_yellow2", 1f);

		var tries = character.Variables.Perm.GetInt(TreeTriesVar, 0) + 1;
		character.Variables.Perm.SetInt(TreeTriesVar, tries);

		if (tries < TreeTries)
		{
			character.ServerMessage(L("The energy spreads through the forest, but nothing appears yet."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.ServerMessage(L("Letas' Sacred Tree appears for a moment. You take one of its branches."));
		character.Inventory.Add(ItemId.KATYN_12_MQ_02_ITEM, 1, InventoryAddType.PickUp);

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Purifies a monster with Letas' Sacred Branch and absorbs its
	/// energy.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_KATYN_12_MQ_02_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_katyn_12" || !character.Quests.IsActive(Mq03) || character.Quests.IsCompletable(Mq03))
		{
			character.ServerMessage(L("There is no need to use the branch right now."));
			return ItemUseResult.OkayNotConsumed;
		}

		var target = character.Map.GetAttackableEnemiesInPosition(character, character.Position, 150)
			.FirstOrDefault(entity => entity is Mob mob && Monsters.Contains(mob.Data.ClassName));

		if (target == null)
		{
			character.ServerMessage(L("There are no monsters nearby to purify."));
			return ItemUseResult.OkayNotConsumed;
		}

		target.PlayEffect("F_light003_blue", 1f);

		var purified = character.Variables.Perm.GetInt(PurifyCountVar, 0) + 1;
		character.Variables.Perm.SetInt(PurifyCountVar, purified);
		character.ServerMessage(LF("Purified energy absorbed: {0}/{1}", Math.Min(purified, PurifyNeeded), PurifyNeeded));

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Sets up the Namott of Suppression at the closest evil energy or
	/// Black Spirit.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_KATYN_12_MQ_03_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName == "f_katyn_12")
		{
			for (var i = 0; i < EvilEnergy.GetLength(0); ++i)
			{
				if (IsEvilEnergyActive(character, i + 1) && character.Position.Get2DDistance(new Position((float)EvilEnergy[i, 0], character.Position.Y, (float)EvilEnergy[i, 1])) <= NamottRange)
				{
					character.PlayEffect("F_light003_blue", 1f);
					this.SuppressEvilEnergy(character, i + 1, null);
					return ItemUseResult.OkayNotConsumed;
				}
			}

			for (var i = 0; i < BlackSpirits.GetLength(0); ++i)
			{
				if (IsBlackSpiritActive(character, i + 1) && character.Position.Get2DDistance(new Position((float)BlackSpirits[i, 0], character.Position.Y, (float)BlackSpirits[i, 1])) <= NamottRange)
				{
					character.PlayEffect("F_light003_blue", 1f);
					this.SuppressBlackSpirit(character, i + 1, null);
					return ItemUseResult.OkayNotConsumed;
				}
			}
		}

		character.ServerMessage(L("There is nothing here to suppress."));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Reads the letter picked up at Letas Stream.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_KATYN_12_SQ_01_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.ServerMessage(L("Jurus, forgive me for not fulfilling your orders. We've lost the Delmore Castle."));
		character.ServerMessage(L("What we have there is an unbelievably horrifying sight. The demons... They have begun to hunt down the people."));
		character.ServerMessage(L("I fled, but I don't think I will reach Orsha. If someone finds this letter, please take it to Jurus in Orsha..."));
		character.ServerMessage(L("Please, uphold the family. Eras."));

		return ItemUseResult.OkayNotConsumed;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30060: Saving the Guide Owl
//-----------------------------------------------------------------------------
public class Katyn12Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30060);
		SetName(L("Saving the Guide Owl"));
		SetDescription(L("The Owl Chief Sculpture requested you gather the energy of Karolis Springs and deliver it to the Guide Owl Sculptures. Go find the Guide Owl Sculptures."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_01", "f_katyn_12", L("Find the Guide Owl Sculpture at Letas Stream"), L("The Owl Chief Sculpture requested you gather the energy of Karolis Springs and deliver it to the Guide Owl Sculptures. Go find the Guide Owl Sculptures."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_NPC_01", "f_katyn_12", L("Find the Guide Owl Sculpture at Letas Stream"), L("The Owl Chief Sculpture requested you gather the energy of Karolis Springs and deliver it to the Guide Owl Sculptures. Go find the Guide Owl Sculptures."));
		SetPhase(QuestStatus.Success, "KATYN_12_NPC_01", "f_katyn_12", L("Find the Guide Owl Sculpture at Letas Stream"), L("The Owl Chief Sculpture requested you gather the energy of Karolis Springs and deliver it to the Guide Owl Sculptures. Go find the Guide Owl Sculptures."));

		AddPrerequisite(new QuestStatusPrerequisite(30059, QuestStatus.Completed));

		AddObjective("deliverEnergy", L("Find the Guide Owl Sculpture at Letas Stream"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 330));
		AddReward(new SelectItemReward("LEG02_169", "LEG02_170", "LEG02_171"));
	}
}

// 30061: Sacred Tree of the Forest (1)
//-----------------------------------------------------------------------------
public class Katyn12Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30061);
		SetName(L("Sacred Tree of the Forest (1)"));
		SetDescription(L("The Owl Chief Sculpture requested a divine branch of Letas in order to defeat the demons. Use the crystal containing the energy of Karolis Springs to find the holy tree of Letas and obtain a branch."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_01", "f_katyn_12", L("Talk to the Guide Owl Sculpture"), L("The Guide Owl Sculpture regained its consciousness. Tell it what the Owl Chief Sculpture told you."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_NPC_01", "f_katyn_12", L("Find Letas' Sacred Branch"), L("The Owl Chief Sculpture requested a divine branch of Letas in order to defeat the demons. Use the crystal containing the energy of Karolis Springs to find the holy tree of Letas and obtain a branch."));
		SetPhase(QuestStatus.Success, "KATYN_12_NPC_01", "f_katyn_12", L("Report to the Guide Owl Sculpture"), L("Acquired Letas' Sacred Branch. Return to the Guide Owl Sculpture."));

		AddPrerequisite(new QuestStatusPrerequisite(30060, QuestStatus.Completed));

		AddObjective("findBranch", L("Find Letas' Sacred Branch"), new CollectItemObjective("KATYN_12_MQ_02_ITEM", 1));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 330));
	}
}

// 30062: Sacred Tree of the Forest (2)
//-----------------------------------------------------------------------------
public class Katyn12Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30062);
		SetName(L("Sacred Tree of the Forest (2)"));
		SetDescription(L("The Guide Owl Sculpture tells you that the branch can purify the monsters and their energy. Absorb the purified energy and collect it."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_01", "f_katyn_12", L("Talk to the Guide Owl Sculpture"), L("Ask the Guide Owl Sculpture what the branch can do."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_NPC_01", "f_katyn_12", L("Absorb the purified energy"), L("The Guide Owl Sculpture tells you that the branch can purify the monsters and their energy. Absorb the purified energy and collect it."));
		SetPhase(QuestStatus.Success, "KATYN_12_NPC_01", "f_katyn_12", L("Report to the Guide Owl Sculpture"), L("Letas' Sacred Branch purified and absorbed enough energy. Return to the Guide Owl Sculpture."));

		AddPrerequisite(new QuestStatusPrerequisite(30061, QuestStatus.Completed));

		AddObjective("purifyMonsters", L("Absorb the purified energy"), new VariableCheckObjective(FKatyn12QuestNpcsScript.PurifyCountVar, 5, isPermanent: true));

		AddReward(new ItemReward("KATYN_12_MQ_03_ITEM", 1));
		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 330));
		AddReward(new TakeItemReward("KATYN_12_MQ_02_ITEM", -1));
	}
}

// 30063: Reliable Assistant
//-----------------------------------------------------------------------------
public class Katyn12Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30063);
		SetName(L("Reliable Assistant"));
		SetDescription(L("The Guide Owl Sculpture asks you to speak with Mardas."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_01", "f_katyn_12", L("Talk to the Guide Owl Sculpture"), L("Namott of Suppression is completed but the Guide Owl a lot in its mind. Talk with the Guide Owl."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_NPC_02", "f_katyn_12", L("Speak to Mardas"), L("The Guide Owl Sculpture asks you to speak with Mardas."));
		SetPhase(QuestStatus.Success, "KATYN_12_NPC_02", "f_katyn_12", L("Speak to Mardas"), L("The Guide Owl Sculpture asks you to speak with Mardas."));

		AddPrerequisite(new QuestStatusPrerequisite(30062, QuestStatus.Completed));

		AddObjective("findMardas", L("Speak to Mardas"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 330));
	}
}

// 30064: Forest Corrupted by the Demons
//-----------------------------------------------------------------------------
public class Katyn12Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30064);
		SetName(L("Forest Corrupted by the Demons"));
		SetDescription(L("The first thing to do is eliminate the evil energy contaminating the forest. Set up the Namott of Suppression around the evil energy to clear it out."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_02", "f_katyn_12", L("Speak with Mardas"), L("Mardas seems to have finally found a solution after discussing with the Owl Chief Sculpture. Listen to what Mardas has to say."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_OBJ_02_1", "f_katyn_12", L("Clear out the evil energy contaminating the forest"), L("The first thing to do is eliminate the evil energy contaminating the forest. Set up the Namott of Suppression around the evil energy to clear it out."));
		SetPhase(QuestStatus.Success, "KATYN_12_NPC_02", "f_katyn_12", L("Report to Mardas"), L("You have cleared out all of the evil energy. Go back to Mardas and tell him about it."));

		AddPrerequisite(new QuestStatusPrerequisite(30063, QuestStatus.Completed));

		AddObjective("removeEvil", L("Clear out the evil energy contaminating the forest"), new VariableCheckObjective(FKatyn12QuestNpcsScript.EvilCountVar, 5, isPermanent: true));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 330));
	}
}

// 30065: Away From the Watch
//-----------------------------------------------------------------------------
public class Katyn12Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30065);
		SetName(L("Away From the Watch"));
		SetDescription(L("Mardas says you need to remove the magic circle keeping the Surveillance Spheres in order to access the black energy that's absorbing the forest's own energy. Avoid the Surveillance Spheres and destroy its magic circle."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_02", "f_katyn_12", L("Speak with Mardas"), L("You have eliminated all of the evil energy. Talk to Mardas about what to do next."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_MQ_06_TRIGGER", "f_katyn_12", L("Remove the Surveillance Sphere Magic Circle"), L("Mardas says you need to remove the magic circle keeping the Surveillance Spheres in order to access the black energy that's absorbing the forest's own energy. Avoid the Surveillance Spheres and destroy its magic circle."));
		SetPhase(QuestStatus.Success, "KATYN_12_OBJ_06", "f_katyn_12", L("Remove the Surveillance Sphere Magic Circle"), L("Mardas says you need to remove the magic circle keeping the Surveillance Spheres in order to access the black energy that's absorbing the forest's own energy. Avoid the Surveillance Spheres and destroy its magic circle."));

		AddPrerequisite(new QuestStatusPrerequisite(30064, QuestStatus.Completed));

		AddObjective("removeCircle", L("Remove the Surveillance Sphere Magic Circle"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 330));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The magic circle's removal ends the quest and opens the way to the Black Spirits.
		character.Quests.Complete(this.QuestId);

		var blackSpiritQuestId = new QuestId(30066);
		if (!character.Quests.Has(blackSpiritQuestId))
		{
			character.Variables.Perm.SetInt(FKatyn12QuestNpcsScript.BlackCountVar, 0);
			character.Quests.Start(blackSpiritQuestId);
		}

		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The Surveillance Spheres have stopped.{nl}Remove the Black Spirits at the end of Senyvas Yard's forked roads."), 8);
		character.LookAround();
	}
}

// 30066: Eery Black Energy
//-----------------------------------------------------------------------------
public class Katyn12Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30066);
		SetName(L("Eery Black Energy"));
		SetDescription(L("At the end of the forked roads at Senyvas Yard, destroy the black energy that's absorbing the forest's own energy."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_OBJ_03_1", "f_katyn_12", L("Remove the Black Energy"), L("At the end of the forked roads at Senyvas Yard, destroy the black energy that's absorbing the forest's own energy."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_OBJ_03_1", "f_katyn_12", L("Remove the Black Energy"), L("At the end of the forked roads at Senyvas Yard, destroy the black energy that's absorbing the forest's own energy."));
		SetPhase(QuestStatus.Success, "KATYN_12_NPC_02", "f_katyn_12", L("Report to Mardas"), L("The black energy has been completely removed. Go and tell Mardas."));

		AddPrerequisite(new QuestStatusPrerequisite(30065, QuestStatus.Completed));

		AddObjective("removeBlack", L("Remove the Black Energy"), new VariableCheckObjective(FKatyn12QuestNpcsScript.BlackCountVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 330));
	}
}

// 30067: Soul Starvation (1)
//-----------------------------------------------------------------------------
public class Katyn12Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30067);
		SetName(L("Soul Starvation (1)"));
		SetDescription(L("Mardas says the magic circles protecting Soul Starvation must be removed. The circles must be removed in the correct order, or they will become active again. Control each circle to figure out the order and remove them accordingly."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_02", "f_katyn_12", L("Speak with Mardas"), L("The time has come to put an end to the demon's scheme. Speak with Mardas on the final mission."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_OBJ_05", "f_katyn_12", L("Remove all magic circles protecting the Soul Starvation"), L("Mardas says the magic circles protecting Soul Starvation must be removed. The circles must be removed in the correct order, or they will become active again. Control each circle to figure out the order and remove them accordingly."));
		SetPhase(QuestStatus.Success, "KATYN_12_OBJ_05", "f_katyn_12", L("Remove the magic circles protecting the Soul Starvation"), L("Mardas says the magic circles protecting Soul Starvation must be removed. The circles must be removed in the correct order, or they will become active again. Control each circle to figure out the order and remove them accordingly."));

		AddPrerequisite(new QuestStatusPrerequisite(30066, QuestStatus.Completed));

		AddObjective("disableCircles", L("Remove all magic circles protecting the Soul Starvation"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 330));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The last circle ends the quest and lays the Soul Starvation open.
		character.Quests.Complete(this.QuestId);

		var soulStarvationQuestId = new QuestId(30068);
		if (!character.Quests.Has(soulStarvationQuestId))
			character.Quests.Start(soulStarvationQuestId);

		character.LookAround();
	}
}

// 30068: Soul Starvation (2)
//-----------------------------------------------------------------------------
public class Katyn12Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30068);
		SetName(L("Soul Starvation (2)"));
		SetDescription(L("The magic circles protecting Soul Starvation are all disabled. Use the power of Namott of Suppression to destroy Soul Starvation."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_OBJ_05", "f_katyn_12", L("Destroy the Soul Starvation with the Namott of Suppression"), L("The magic circles protecting Soul Starvation are all disabled. Use the power of Namott of Suppression to destroy Soul Starvation."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_OBJ_05", "f_katyn_12", L("Destroy the Soul Starvation with the Namott of Suppression"), L("The magic circles protecting Soul Starvation are all disabled. Use the power of Namott of Suppression to destroy Soul Starvation."));
		SetPhase(QuestStatus.Success, "KATYN_12_OBJ_05", "f_katyn_12", L("Destroy the Soul Starvation with the Namott of Suppression"), L("You have defeated all the demons around. Use the power of the Namott of Suppression to destroy Soul Starvation."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "KATYN_12_MQ_09_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(30067, QuestStatus.Completed));

		AddObjective("killMerge", L("Defeat Merge"), new KillObjective(1, "boss_Merge_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 330));
	}
}

// 30069: Peace in the Forest
//-----------------------------------------------------------------------------
public class Katyn12Mq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30069);
		SetName(L("Peace in the Forest"));
		SetDescription(L("Mardas says he will deliver the message to the Guide Owl after inspecting the situation. Go to the Guide Owl and return the Namott of Suppression and report that everything is solved."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_NPC_02", "f_katyn_12", L("Report to Mardas"), L("The Soul Starvation is now destroyed. Report back to Mardas."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_NPC_01", "f_katyn_12", L("Report to Mardas"), L("The Soul Starvation is now destroyed. Report back to Mardas."));
		SetPhase(QuestStatus.Success, "KATYN_12_NPC_01", "f_katyn_12", L("Report to the Guide Owl Sculpture"), L("Mardas says he will deliver the message to the Guide Owl after inspecting the situation. Go to the Guide Owl and return the Namott of Suppression and report that everything is solved."));

		AddPrerequisite(new QuestStatusPrerequisite(30068, QuestStatus.Completed));

		AddObjective("reportMardas", L("Report to the Guide Owl Sculpture"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 330));
		AddReward(new SelectItemReward("TOP02_169", "TOP02_170", "TOP02_171"));
		AddReward(new TakeItemReward("KATYN_12_MQ_03_ITEM", -1));
	}
}

// 30072: A Story Left Behind by Eras
//-----------------------------------------------------------------------------
public class Katyn12Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30072);
		SetName(L("A Story Left Behind by Eras"));
		SetDescription(L("The sender of the letter is a person called Eras. It is supposed to go to a person called Jurus in Orsha."));
		SetType(QuestType.Sub);
		SetLocation("f_katyn_12", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_SQ_NPC_01", "f_katyn_12", L("Check the letter on the ground"), L("A letter lies on the ground. Check if someone has lost it."));
		SetPhase(QuestStatus.InProgress, "Jurus", "c_orsha", L("Find the right recipient"), L("The sender of the letter is a person called Eras. It is supposed to go to a person called Jurus in Orsha."));
		SetPhase(QuestStatus.Success, "Jurus", "c_orsha", L("Find the right recipient"), L("The sender of the letter is a person called Eras. It is supposed to go to a person called Jurus in Orsha."));

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("deliverLetter", L("Find the right recipient"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 250));
		AddReward(new TakeItemReward("KATYN_12_SQ_01_ITEM", -1));
	}
}

// 30073: Letas Altar
//-----------------------------------------------------------------------------
public class Katyn12Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30073);
		SetName(L("Letas Altar"));
		SetDescription(L("Letas Altar is weakened by the evil force from the nearby monsters. Defeat the nearby monsters."));
		SetType(QuestType.Sub);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_12_SQ_NPC_02", "f_katyn_12", L("Check the Letas Altar"), L("There is a trouble at the Letas Altar, it seems. Check the altar."));
		SetPhase(QuestStatus.InProgress, "KATYN_12_SQ_NPC_02", "f_katyn_12", L("Defeat the nearby monsters disrupting the energy of the Letas Altar"), L("Letas Altar is weakened by the evil force from the nearby monsters. Defeat the nearby monsters."));
		SetPhase(QuestStatus.Success, "KATYN_12_SQ_NPC_02", "f_katyn_12", L("Defeat the nearby monsters disrupting the energy of the Letas Altar"), L("Letas Altar is weakened by the evil force from the nearby monsters. Defeat the nearby monsters."));

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("killMonsters", L("Defeat the nearby monsters disrupting the energy of the Letas Altar"), new KillObjective(10, "puragi", "Sec_zombiegirl2_chpel", "jellyfish_blue", "zigri", "chupacabra_green", "operor_blue"));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 150));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The altar's energy returning ends the quest; there is no turn-in NPC.
		character.Quests.Complete(this.QuestId);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The energy of the Letas Altar has been restored."), 5);
	}
}

// 50271: Wandering Spirit
//-----------------------------------------------------------------------------
public class Katyn12Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50271);
		SetName(L("Wandering Spirit"));
		SetDescription(L("The spirit had been looking for an important object all along. Now that he has it back, help the spirit reach the Frail Owl Sculpture so he can finally move on."));
		SetType(QuestType.Sub);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN12_HQ1_NPC", "f_katyn_12", L("Talk to the Lost Spirit"), L("There's a lost spirit wandering around Letas Stream. Talk to the spirit."));
		SetPhase(QuestStatus.InProgress, "KATYN12_RP_1_NPC", "f_katyn_12", L("Bring the Lost Spirit to the Frail Owl Sculpture"), L("The spirit had been looking for an important object all along. Now that he has it back, help the spirit reach the Frail Owl Sculpture so he can finally move on."));
		SetPhase(QuestStatus.Success, "KATYN12_RP_1_NPC", "f_katyn_12", L("Talk to the Frail Owl Sculpture"), L("The spirit has successfully reached the Frail Owl Sculpture. Talk to the Owl Sculpture."));

		AddPrerequisite(new QuestStatusPrerequisite(30069, QuestStatus.Completed));
		AddPrerequisite(new ItemPrerequisite("KATYN12_HIDDENQ1_ITEM", 1));

		AddObjective("guideSpirit", L("Bring the Lost Spirit to the Frail Owl Sculpture"), new ManualObjective());

		AddReward(new ItemReward("Collection_Base_KATYN12_HQ1", 1));
		AddReward(new TakeItemReward("KATYN12_HIDDENQ1_ITEM", 1));
	}
}

// 60165: Fading Spirit
//-----------------------------------------------------------------------------
public class Katyn12Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60165);
		SetName(L("Fading Spirit"));
		SetDescription(L("The Frail Owl sculpture asked you to bring back the fading spirits that ran away because of the monsters."));
		SetType(QuestType.Repeat);
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN12_RP_1_NPC", "f_katyn_12", L("Talk to the Frail Owl Sculpture"), L("The Frail Owl sculpture at Letas Stream is waiting for help."));
		SetPhase(QuestStatus.InProgress, "KATYN12_RP_1_NPC", "f_katyn_12", L("Retreiving Fading Spirits"), L("The Frail Owl sculpture asked you to bring back the fading spirits that ran away because of the monsters."));
		SetPhase(QuestStatus.Success, "KATYN12_RP_1_NPC", "f_katyn_12", L("Return to the Frail Owl sculpture"), L("Acquired enough fading spirits. Go back to the Frail Owl sculpture."));

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("collectSpirits", L("Retreiving Fading Spirits"), new CollectItemObjective("KATYN12_RP_1_ITEM", 5));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("KATYN12_RP_1_ITEM", -1));
	}
}
