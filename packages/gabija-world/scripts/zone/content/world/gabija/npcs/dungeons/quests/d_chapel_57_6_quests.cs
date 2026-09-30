//--- Melia Script ----------------------------------------------------------
// Tenet Church 1F Quest NPCs
//--- Description -----------------------------------------------------------
// Vaidutis and Donatas at the church gate, and the altars and demons their
// quests run on.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
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

public class DChapel576QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8510);
	private readonly static QuestId Mq02 = new QuestId(8511);
	private readonly static QuestId Mq04 = new QuestId(8513);
	private readonly static QuestId Mq041 = new QuestId(8730);
	private readonly static QuestId Mq05 = new QuestId(8514);
	private readonly static QuestId Mq06 = new QuestId(8515);
	private const string PersuadedVar = "Gabija.Chapel576.Persuaded";
	private const string EscortVar = "Gabija.Chapel576.Escort";
	private readonly static QuestId Mq07 = new QuestId(8451);
	private readonly static QuestId Mq08 = new QuestId(8517);
	private readonly static QuestId Mq09 = new QuestId(8518);
	private readonly static QuestId Mq0905 = new QuestId(8527);
	private readonly static QuestId Rp1 = new QuestId(60156);
	public const string GlobejasCountVar = "Gabija.Chaple576.Mq08.Converted";
	private const string GlobejasUntilVar = "Gabija.Chaple576.Mq08.Until";
	private readonly static Position GlobejasAltarPosition = new Position(-523, 0, 1948);

	protected override void Load()
	{
		// Follower Vaidutis
		//-------------------------------------------------------------------------
		AddNpc(147400, L("Follower Vaidutis"), "CHAPEL_VIRGINIJA", "d_chapel_57_6", 961, -114, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Vaidutis"));

			if (character.Quests.IsActive(Mq0905) && character.Quests.IsCompletable(Mq0905))
			{
				await dialog.Msg(L("I came to say that Gesti went up to the 2nd floor, and I let my guard down."));
				await dialog.Msg(L("I would have been in big trouble if not for you."));

				await dialog.CompleteQuest(Mq0905);
				return;
			}

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Well done."));
				await dialog.Msg(L("Making the Light Crystal with this will be enough power to break the barrier."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("The barrier at the gate is gone. You defeated the demon that guarded it!"));
				await dialog.Msg(L("Follower Algis is already going through to investigate the 1st floor."));
				await dialog.CompleteQuest(Mq02);

				if (!character.Quests.Has(Mq041) && character.Quests.MeetsPrerequisites(Mq041))
					character.Quests.Start(Mq041);

				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Good job."));
				await dialog.Msg(L("Their babbling laughter seems to have stopped."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Rp1) && character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("It's not much... But it will have to do."));
				await dialog.Msg(L("Thank you so much!"));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("I studied the barrier at the gate, but the power of the basement altar is insufficient."));
				var answer = await dialog.SelectQuestOffer(Mq01, L("Could you supply me with Power Crystals from the Corylus?"),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq01);

				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("You should get ready before you take the Light Crystal to the entrance. It could attract a powerful monster."),
					Option(L("I'll open the gate"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					character.Inventory.Add(650726, 1, InventoryAddType.PickUp);
					character.LookAround();
					await dialog.Msg(L("I think I'll stay here to stop the demons from entering the basement."));
				}

				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("I would like to ask you to defeat Pawndel and Pawnd."));
				var answer = await dialog.SelectQuestOffer(Mq04, L("They are demon sisters and they are quite a nuisance."),
					Option(L("I will defeat it"), "accept"),
					Option(L("I don't have time"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq04);

				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				await dialog.Msg(L("I've lost all of my Holy Stones..."));
				var answer = await dialog.SelectQuestOffer(Rp1, L("I don't know how much longer I'll be able to stay here."),
					Option(L("I'll try to find them"), "accept"),
					Option(L("I wish you good luck."), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rp1);
					character.LookAround();
				}

				return;
			}

			if (character.Quests.IsActive(Mq0905))
			{
				await dialog.Msg(L("I heard Cyclops fall. The way up should be clear."));
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The Corylus hoard Power Crystals at the Worship Anteroom."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("I think I'll stay here to stop the demons from entering the basement."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Pawndel and Pawnd live around the Worship Anteroom. Careful, they are violent."));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("The orb crystals are near the Worship Anteroom and Nuosirdum Chapel."));
				return;
			}

			await dialog.Msg(L("The way down is sealed behind us. We hold the gate or nothing."));
		});

		// Follower Donatas
		//-------------------------------------------------------------------------
		AddConditionalNpc(147399, L("Follower Donatas"), "CHAPEL576_DONATAS", "d_chapel_57_6", -1674, 374, 110, c => c.Quests.Has(Mq041), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Donatas"));

			if (character.Quests.IsActive(Mq041) && character.Quests.IsCompletable(Mq041))
			{
				await dialog.Msg(L("It is an honor to fight with you, Revelator."));
				await dialog.Msg(L("Could you drive the demons from this area while Follower Algis investigates?"));
				await dialog.CompleteQuest(Mq041);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("This is sufficient to make a transformation scroll."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("Well done."));
				await dialog.Msg(L("It will probably be difficult for them to trust each other from now on."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("Alright."));
				await dialog.Msg(L("I think that should be enough for you to grasp their nature."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("Good job."));
				await dialog.Msg(L("If there are any good demons left, the goddess will look after them."));
				await dialog.CompleteQuest(Mq08);
				return;
			}

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("It was an ambush."));
				await dialog.Msg(L("We would have been in big trouble without your help."));
				await dialog.CompleteQuest(Mq09);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("My plan is to let you disguise as a demon."));
				var answer = await dialog.SelectQuestOffer(Mq05, L("Please collect Pawndel and Pawnd's clothing first."),
					Option(L("That seems fun"), "accept"),
					Option(L("That's blasphemous"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq05);

				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("You can transform into a demon by using this scroll."));
				var answer = await dialog.SelectQuestOffer(Mq06, L("Persuade the demons to go to the altar on their own."),
					Option(L("Sounds fun"), "accept"),
					Option(L("Looks like we'll be caught soon. Let's just stop."), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("If you lack self confidence, the possibility of getting caught is high."));
					await dialog.Msg(L("Before you use this scroll, it is important to gain some confidence by fighting against demons."));
					for (var i = 1; i <= 72; ++i)
						character.Variables.Perm.Set(PersuadedVar + i, false);

					character.Variables.Temp.SetInt(EscortVar, 0);
					character.Quests.Start(Mq06);
					character.Inventory.Add(650725, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("I'm thinking of converting the demons using the power of the altar."));
				var answer = await dialog.SelectQuestOffer(Mq07, L("The converted demons will absorb the divine power and slowly start to die."),
					Option(L("I'll try to do it"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq07);

				return;
			}

			if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
			{
				await dialog.Msg(L("The power of the altar makes conversion simple."));
				var answer = await dialog.SelectQuestOffer(Mq08, L("First, you activate the power of Globejas Altar. Then, stay within its influence and fight the demons."),
					Option(L("It could be difficult but I'll try"), "accept"),
					Option(L("I need to find out more about the demons"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("The demons have stronger minds than you might think. It will take a lot of effort to make them forget about Gesti."));
					character.Variables.Temp.SetInt(GlobejasCountVar, 0);
					character.Quests.Start(Mq08);
				}
				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("The Central Altar that suppresses the power of the demons suddenly stopped."));
				var answer = await dialog.SelectQuestOffer(Mq09, L("Could you take a look at what's going on?"),
					Option(L("I'll check on it"), "accept"),
					Option(L("About the Church's altar"), "explain"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Many structures in the Tenet Church are placed to ward against the demons."));
					await dialog.Msg(L("Each one in itself is a small barrier that contributes to a huge barrier."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Mq09);

				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Collect Pawndel and Pawnd's clothing for the transformation scroll."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Defeat a demon to gain some confidence, then use the scroll and lure the demons to the Apsauga Altar."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Kill the demons and gather their souls for the altar."));
				return;
			}

			if (character.Quests.IsActive(Mq08))
			{
				await dialog.Msg(L("Activate the Globejas Altar and fight the demons within its reach."));
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("The Central Altar is north. Check what has stopped it."));
				character.Quests.ClearQuestTrack(Mq09);
				return;
			}

			await dialog.Msg(L("Algis is already inside. We drive the demons from this floor."));
		});

		// Church Gate
		//-------------------------------------------------------------------------
		AddConditionalNpc(147379, L("Church Gate"), "CHAPLE576_MQ_04", "d_chapel_57_6", -1778, 426, 91, c => !c.Quests.HasCompleted(Mq02) && !(c.Quests.IsActive(Mq02) && c.Quests.IsCompletable(Mq02)), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Church Gate"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You set the Light Crystal against the demonic barrier. Something powerful turns toward it."));
				character.Quests.StartQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("A demonic barrier seals the church gate."));
		});

		// Central Altar
		//-------------------------------------------------------------------------
		AddNpc(147358, L("Central Altar"), "CHAPLE576_MQ_09", "d_chapel_57_6", -523, 446, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Central Altar"));

			if (character.Quests.IsActive(Mq09) && !character.Quests.IsCompletable(Mq09))
			{
				var checkedAltar = await character.TimeActions.StartAsync(L("Checking the altar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (checkedAltar != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("You touch the altar and the pillar of light snaps out. A Mallet Wyvern drops from the rafters."));
				character.Quests.StartQuestTrack(Mq09);
				return;
			}

			await dialog.Msg(L("The Central Altar thrums, holding back the demons' power."));
		});

		// Globejas Altar
		//-------------------------------------------------------------------------
		AddNpc(147357, L("Globejas Altar"), "CHAPEL576_NORTH", "d_chapel_57_6", -523, 1948, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Globejas Altar"));

			if (character.Quests.IsActive(Mq08) && !character.Quests.IsCompletable(Mq08))
			{
				var operated = await character.TimeActions.StartAsync(L("Operating"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(1));

				if (operated != TimeActionResult.Completed)
					return;

				if (character.Variables.Temp.TryGet<DateTime>(GlobejasUntilVar, out var until) && until > DateTime.Now)
				{
					character.ServerMessage(L("The power of Globejas is already in effect!"));
					return;
				}

				character.Variables.Temp.Set(GlobejasUntilVar, DateTime.Now.AddSeconds(45));

				foreach (var enemy in character.Map.GetAttackableEnemiesInPosition(character, GlobejasAltarPosition, 550))
					enemy.InsertHate(character, 1);

				return;
			}

			await dialog.Msg(L("The Globejas Altar waits for someone to wake it."));
		});

		// Apsauga Altar
		//-------------------------------------------------------------------------
		AddNpc(147357, L("Apsauga Altar"), "CHAPEL576_BASIC_2", "d_chapel_57_6", -526, -1092, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Apsauga Altar"));

			await dialog.Msg(L("An altar of protection, standing silent in the dark."));
		});

		// Orb Crystals
		//-------------------------------------------------------------------------
		QuestSpots.Add(new QuestSpotSpec
		{
			Prefix = "CHAPLE576_RP_1_OBJ",
			MonsterId = 153105,
			Name = L("Orb Crystal"),
			Map = "d_chapel_57_6",
			Points = [(988, 262, 90), (1262, 292, 90), (1148, 256, 90), (1034, 297, 90), (820, 253, 90), (916, 291, 90), (824, 563, 90), (926, 554, 90), (978, 607, 90), (1070, 565, 90), (1159, 602, 90), (1237, 564, 90), (852, 303, 90), (1337, 553, 90), (1270, 614, 90), (1095, 285, 90), (120, 536, 90), (248, 597, 90), (169, 312, 90), (382, 537, 90), (365, 279, 90), (170, -193, 90), (343, 13, 90)],
			IsActive = c => c.Quests.IsActive(Rp1) && !c.Quests.IsCompletable(Rp1),
			TimedLabel = L("Collecting"),
			TimedAnim = "SITGROPESET",
			Seconds = 2,
			IdleMessage = L("A cluster of dull orb crystals."),
			OnDone = (character, npc) =>
			{
				npc?.PlayEffect("F_pc_making_finish_white", 2f);
				character.ServerMessage(L("Acquired Orb Crystal."));
				character.Inventory.Add(664092, 1, InventoryAddType.PickUp);
			},
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// Demons that can be persuaded while disguised
		//-------------------------------------------------------------------------
		this.AddPersuadableDemon(1, true, -585, -286);
		this.AddPersuadableDemon(2, true, -516, -975);
		this.AddPersuadableDemon(3, true, -607, 582);
		this.AddPersuadableDemon(4, true, -571, 1168);
		this.AddPersuadableDemon(5, true, -381, 478);
		this.AddPersuadableDemon(6, true, -522, 1955);
		this.AddPersuadableDemon(7, true, 223, 366);
		this.AddPersuadableDemon(8, true, 462, 496);
		this.AddPersuadableDemon(9, true, -614, -455);
		this.AddPersuadableDemon(10, true, -432, -409);
		this.AddPersuadableDemon(11, true, 160, 433);
		this.AddPersuadableDemon(12, true, 359, 577);
		this.AddPersuadableDemon(13, true, -667, 292);
		this.AddPersuadableDemon(14, true, -547, 192);
		this.AddPersuadableDemon(15, true, -382, 309);
		this.AddPersuadableDemon(16, true, -305, 186);
		this.AddPersuadableDemon(17, true, -285, 629);
		this.AddPersuadableDemon(18, true, -659, 706);
		this.AddPersuadableDemon(19, true, -746, 405);
		this.AddPersuadableDemon(20, true, -448, 648);
		this.AddPersuadableDemon(21, true, -456, 1236);
		this.AddPersuadableDemon(22, true, -579, 1384);
		this.AddPersuadableDemon(23, true, -466, 1371);
		this.AddPersuadableDemon(24, true, -637, 1228);
		this.AddPersuadableDemon(25, true, -578, 1849);
		this.AddPersuadableDemon(26, true, -403, 1872);
		this.AddPersuadableDemon(27, true, -490, 2070);
		this.AddPersuadableDemon(28, true, -676, 2061);
		this.AddPersuadableDemon(29, true, -681, 1891);
		this.AddPersuadableDemon(30, true, -365, 1996);
		this.AddPersuadableDemon(31, true, -523, -1283);
		this.AddPersuadableDemon(32, true, -375, -1274);
		this.AddPersuadableDemon(33, true, -369, -999);
		this.AddPersuadableDemon(34, true, -720, -1138);
		this.AddPersuadableDemon(35, true, -325, -1160);
		this.AddPersuadableDemon(36, true, -629, -882);
		this.AddPersuadableDemon(37, false, -509, 1938);
		this.AddPersuadableDemon(38, false, -513, 1657);
		this.AddPersuadableDemon(39, false, -618, 1250);
		this.AddPersuadableDemon(40, false, -475, 314);
		this.AddPersuadableDemon(41, false, -420, 614);
		this.AddPersuadableDemon(42, false, -718, 242);
		this.AddPersuadableDemon(43, false, -532, -368);
		this.AddPersuadableDemon(44, false, -573, -1163);
		this.AddPersuadableDemon(45, false, -451, -989);
		this.AddPersuadableDemon(46, false, 276, 444);
		this.AddPersuadableDemon(47, false, -740, 583);
		this.AddPersuadableDemon(48, false, -427, 1285);
		this.AddPersuadableDemon(49, false, -654, 441);
		this.AddPersuadableDemon(50, false, -278, 479);
		this.AddPersuadableDemon(51, false, 216, 303);
		this.AddPersuadableDemon(52, false, 273, 570);
		this.AddPersuadableDemon(53, false, -633, -320);
		this.AddPersuadableDemon(54, false, -596, -505);
		this.AddPersuadableDemon(55, false, -613, -1004);
		this.AddPersuadableDemon(56, false, -651, -1255);
		this.AddPersuadableDemon(57, false, -308, -1209);
		this.AddPersuadableDemon(58, false, -306, -972);
		this.AddPersuadableDemon(59, false, -381, 1997);
		this.AddPersuadableDemon(60, false, -654, 1932);
		this.AddPersuadableDemon(61, false, -533, 1329);
		this.AddPersuadableDemon(62, false, -532, 1102);
		this.AddPersuadableDemon(63, false, -531, 653);
		this.AddPersuadableDemon(64, true, -429, 1879);
		this.AddPersuadableDemon(65, true, -623, 1357);
		this.AddPersuadableDemon(66, true, -554, 2143);
		this.AddPersuadableDemon(67, true, -573, 2052);
		this.AddPersuadableDemon(68, true, -667, 1286);
		this.AddPersuadableDemon(69, true, -365, 1990);
		this.AddPersuadableDemon(70, true, -473, 1790);
		this.AddPersuadableDemon(71, true, -693, 1896);
		this.AddPersuadableDemon(72, true, -472, 1324);

		// The Apsauga Altar, where the persuaded demons arrive.
		AddQuestTrigger("CHAPEL576_MQ_06_LURE", "d_chapel_57_6", -526, -1092, 350, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			var waiting = character.Variables.Temp.GetInt(EscortVar, 0);
			if (waiting > 0)
			{
				character.Variables.Temp.SetInt(EscortVar, 0);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Approach closer to the the altar's orb"), 2);

				for (var i = 0; i < waiting; ++i)
					character.Quests.AddObjectiveProgress(Mq06, "lureDemons");
			}

			await Task.CompletedTask;
		});

	}

	/// <summary>
	/// Adds one of the demons that follows a disguised Revelator to Apsauga Altar.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="isPawndel"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	private void AddPersuadableDemon(int number, bool isPawndel, double x, double z)
	{
		var monsterId = isPawndel ? 57028 : 57213;
		var name = isPawndel ? L("Pawndel") : L("Pawnd");

		AddConditionalNpc(monsterId, name, "CHAPLE576_MQ_06_DEMON" + number, "d_chapel_57_6", x, z, 90, c => c.Quests.IsActive(Mq06) && !c.Quests.IsCompletable(Mq06) && !c.Variables.Perm.GetBool(PersuadedVar + number, false), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(name);

			if (!character.IsBuffActive(BuffId.CHAPLE576_MQ_06_1))
				return;

			if (character.Variables.Temp.GetInt(EscortVar, 0) >= 4)
			{
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("You talked to too many demons.{nl}The little demon suspects you."), 5);
				return;
			}

			string[] pitches = [L("Gesti is looking for you"), L("I found something interesting"), L("There's a place we need to go to")];
			string[] urgings = [L("Quick, follow me"), L("Hurry or you'll be late")];

			var first = await dialog.Select(pitches[GameRandom.Get().Next(pitches.Length)], Option(L("What is it?"), "talk"), Option(L("It's nothing"), "leave"));
			if (first != "talk")
				return;

			var second = await dialog.Select(urgings[GameRandom.Get().Next(urgings.Length)], Option(L("Then go without me"), "go"), Option(L("If you do not want to, just tell me"), "leave"));
			if (second != "go")
			{
				await dialog.Msg(L("You are a little strange..."));
				return;
			}

			character.Variables.Perm.Set(PersuadedVar + number, true);
			character.Variables.Temp.SetInt(EscortVar, character.Variables.Temp.GetInt(EscortVar, 0) + 1);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, name + L(" has been successfully persuaded!{nl}Bring it to the Apsauga Altar"), 3);
			await dialog.Msg(L("Alright... Let's go"));
		});
	}

	/// <summary>
	/// Gives the character confidence once it defeats a Pawndel or Pawnd.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Mob altarMob)
			return;

		var converter = altarMob.GetKillBeneficiary(args.Attacker);
		if (converter != null && converter.Quests.IsActive(Mq08) && !converter.Quests.IsCompletable(Mq08)
			&& converter.Variables.Temp.TryGet<DateTime>(GlobejasUntilVar, out var globejasUntil) && globejasUntil > DateTime.Now
			&& altarMob.Race == RaceType.Velnias && altarMob.Position.InRange2D(GlobejasAltarPosition, 550))
		{
			converter.Variables.Temp.SetInt(GlobejasCountVar, converter.Variables.Temp.GetInt(GlobejasCountVar, 0) + 1);
		}

		if (args.Target is not Mob mob || (mob.Data.ClassName != "Pawndel" && mob.Data.ClassName != "pawnd"))
			return;

		var character = mob.GetKillBeneficiary(args.Attacker);
		if (character == null || !character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
			return;

		if (character.IsBuffActive(BuffId.CHAPLE576_MQ_06) || character.IsBuffActive(BuffId.CHAPLE576_MQ_06_1))
			return;

		character.StartBuff(BuffId.CHAPLE576_MQ_06, 1, 0, TimeSpan.FromSeconds(600), character);
		character.ServerMessage(L("You defeated a demon and gained confidence. Use the Demon Transform Scroll!"));
	}

	/// <summary>
	/// Transforms the character into a demon with the Demon Transform Scroll.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CHAPLE576_MQ_06_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "d_chapel_57_6" || character.Layer != 0 || !character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
		{
			character.ServerMessage(L("There is no need to transform now."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.IsBuffActive(BuffId.CHAPLE576_MQ_06_1))
		{
			character.ServerMessage(L("You are already transformed."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (!character.IsBuffActive(BuffId.CHAPLE576_MQ_06))
		{
			character.ServerMessage(L("You lack the confidence to pass as a demon. Defeat Pawndel or Pawnd first."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.StopBuff(BuffId.CHAPLE576_MQ_06);
		character.StartBuff(BuffId.CHAPLE576_MQ_06_1, 1, 0, TimeSpan.FromSeconds(100), character);
		character.PlayEffect("F_smoke019_dark", 1f);
		character.ServerMessage(L("Transformed! Persuade Pawndel and Pawnd and lure them to the Apsauga Altar!"));

		return ItemUseResult.OkayNotConsumed;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8510: Church Gate (1)
//-----------------------------------------------------------------------------
public class Chaple576Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8510);
		SetName(L("Church Gate (1)"));
		SetDescription(L("Vaidutis needs Power Crystals from the Corylus to make a Light Crystal."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Talk to Follower Vaidutis"), L("Meet Follower Vaidutis in the Tenet Church 1F."));
		SetPhase(QuestStatus.InProgress, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Collect the Power Crystals"), L("Defeat Corylus at the Worship Anteroom and obtain the Crystals of Power."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Talk to Follower Vaidutis"), L("Return to Vaidutis."));

		AddPrerequisite(new QuestStatusPrerequisite(8527, QuestStatus.Completed));

		AddPityDrop("CHAPLE576_MQ_02_ITEM", 1.0f, 0, 1, "Corylus");

		AddObjective("collectCrystals", L("Obtain Power Crystals by defeating Corylus"), new CollectItemObjective("CHAPLE576_MQ_02_ITEM", 8));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CHAPLE576_MQ_02_ITEM"));
	}
}

// 8511: Church Gate (2)
//-----------------------------------------------------------------------------
public class Chaple576Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8511);
		SetName(L("Church Gate (2)"));
		SetDescription(L("Use the Light Crystal to break the demonic barrier at the church entrance."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Talk to Follower Vaidutis"), L("The Essence of Light is ready. Talk to Follower Vaidutis."));
		SetPhase(QuestStatus.InProgress, "CHAPLE576_MQ_04", "d_chapel_57_6", L("Open the church entrance"), L("Use the Light Crystal at the church entrance."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Talk to Follower Vaidutis"), L("Return to Vaidutis."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE576_MQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8510, QuestStatus.Completed));

		AddObjective("killMummyghast", L("Defeat Mummyghast"), new KillObjective(1, "boss_Mummyghast") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("CHAPLE576_MQ_02_ITEM_1"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);
		character.LookAround();
	}
}

// 8513: Demon Sisters
//-----------------------------------------------------------------------------
public class Chaple576Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8513);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Demon Sisters"));
		SetDescription(L("Defeat the demon sisters Pawndel and Pawnd around the Worship Anteroom."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Talk to Follower Vaidutis"), L("Vaidutis is looking for help in the Tenet Church 1F."));
		SetPhase(QuestStatus.InProgress, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Defeat Pawndel and Pawnd"), L("Defeat Pawndel and Pawnd at the Tenet Church 1F."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Talk to Follower Vaidutis"), L("Return to Vaidutis."));

		AddPrerequisite(new LevelPrerequisite(34));

		AddObjective("killPawndel", L("Defeat Pawndel"), new KillObjective(15, "Pawndel"));
		AddObjective("killPawnd", L("Defeat Pawnd"), new KillObjective(15, "pawnd"));

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 8730: The Entrance of the Cathedral (3)
//-----------------------------------------------------------------------------
public class Chaple576Mq041Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8730);
		SetName(L("The Entrance of the Cathedral (3)"));
		SetDescription(L("The gate is open. Algis steps through to investigate the 1F."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Open the Gate"), L("Open the church gates."));
		SetPhase(QuestStatus.InProgress, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Speak with Follower Algis"), L("Speak with Follower Algis at the gate."));
		SetPhase(QuestStatus.Success, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Talk to Follower Donatas."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE576_MQ_04_AFTER", 500, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8511, QuestStatus.Completed));

		AddObjective("openGate", L("Open the Gate"), new ManualObjective());
	}
}

// 8514: The Legendary Trick (1)
//-----------------------------------------------------------------------------
public class Chaple576Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8514);
		SetName(L("The Legendary Trick (1)"));
		SetDescription(L("Collect the clothing of Pawndel and Pawnd to make a transformation scroll."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Follower Donatas is seeking your help."));
		SetPhase(QuestStatus.InProgress, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Collect the clothes of Pawndel and Pawnd"), L("Defeat the demon sisters and collect their clothes."));
		SetPhase(QuestStatus.Success, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Hand the clothes to Follower Donatas."));

		AddPrerequisite(new QuestStatusPrerequisite(8730, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(34));

		AddPityDrop("CHAPLE576_MQ_05_ITEM", 0.7f, 3, 1, "Pawndel", "pawnd");

		AddObjective("collectClothes", L("Collect the clothes of Pawndel and Pawnd"), new CollectItemObjective("CHAPLE576_MQ_05_ITEM", 10));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CHAPLE576_MQ_05_ITEM"));
	}
}

// 8515: The Legendary Trick (2)
//-----------------------------------------------------------------------------
public class Chaple576Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8515);
		SetName(L("The Legendary Trick (2)"));
		SetDescription(L("Use the transformation scroll to lure the demon sisters to the Apsauga Altar."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Follower Donatas is waiting with the transformation scroll."));
		SetPhase(QuestStatus.InProgress, "CHAPEL576_BASIC_2", "d_chapel_57_6", L("Lure Pawndel and Pawnd to the Apsauga Altar"), L("Transform into a demon and lure the sisters to the altar."));
		SetPhase(QuestStatus.Success, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Return to Follower Donatas."));

		AddPrerequisite(new QuestStatusPrerequisite(8514, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(34));

		AddObjective("lureDemons", L("Lure Pawndel and Pawnd to the Apsauga Altar"), new ManualObjective(8));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new TakeItemReward("CHAPLE576_MQ_06_ITEM"));
	}
}

// 8451: Get a Hold of Yourself! (1)
//-----------------------------------------------------------------------------
public class Chaple576Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8451);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Get a Hold of Yourself! (1)"));
		SetDescription(L("Collect the souls of the demon sisters and the Corylus."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Follower Donatas is seeking help on the first floor."));
		SetPhase(QuestStatus.InProgress, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Collect demon souls"), L("Defeat the demons and collect their souls."));
		SetPhase(QuestStatus.Success, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Return to Follower Donatas."));

		AddPrerequisite(new QuestStatusPrerequisite(8515, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(35));

		AddPityDrop("CHAPLE576_MQ_07_1_ITEM", 0.8f, 3, 1, "pawnd");
		AddPityDrop("CHAPLE576_MQ_07_2_ITEM", 0.8f, 3, 1, "Pawndel");
		AddPityDrop("CHAPLE576_MQ_07_3_ITEM", 0.8f, 3, 1, "Corylus");

		AddObjective("collectPawndSoul", L("Collect Pawnd's Soul"), new CollectItemObjective("CHAPLE576_MQ_07_1_ITEM", 6));
		AddObjective("collectPawndelSoul", L("Collect Pawndel's Soul"), new CollectItemObjective("CHAPLE576_MQ_07_2_ITEM", 6));
		AddObjective("collectCorylusSoul", L("Collect Corylus' Soul"), new CollectItemObjective("CHAPLE576_MQ_07_3_ITEM", 6));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CHAPLE576_MQ_07_1_ITEM"));
		AddReward(new TakeItemReward("CHAPLE576_MQ_07_2_ITEM"));
		AddReward(new TakeItemReward("CHAPLE576_MQ_07_3_ITEM"));
	}
}

// 8517: Get a Hold of Yourself! (2)
//-----------------------------------------------------------------------------
public class Chaple576Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8517);
		SetName(L("Get a Hold of Yourself! (2)"));
		SetDescription(L("Convert the demons within reach of the Globejas Altar."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("You are ready to convert the demons. Talk to Follower Donatas."));
		SetPhase(QuestStatus.InProgress, "CHAPEL576_NORTH", "d_chapel_57_6", L("Convert demons at the Globejas Altar"), L("Activate the Globejas Altar and fight the demons within its influence."));
		SetPhase(QuestStatus.Success, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Return to Donatas."));

		AddPrerequisite(new QuestStatusPrerequisite(8451, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(34));

		AddObjective("convertDemons", L("Convert demons at the Globejas Altar"), new VariableCheckObjective(DChapel576QuestNpcsScript.GlobejasCountVar, 12, isPermanent: false));

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 8518: Activate the Central Altar
//-----------------------------------------------------------------------------
public class Chaple576Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8518);
		SetName(L("Activate the Central Altar"));
		SetDescription(L("Check the Central Altar and deal with what stopped it."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Follower Donatas is seeking help on the first floor."));
		SetPhase(QuestStatus.InProgress, "CHAPLE576_MQ_09", "d_chapel_57_6", L("Check the Central Altar"), L("Check the Central Altar."));
		SetPhase(QuestStatus.Success, "CHAPEL576_DONATAS", "d_chapel_57_6", L("Talk to Follower Donatas"), L("Return to Donatas."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE576_MQ_09_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8730, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(34));

		AddObjective("killMalletWyvern", L("Defeat Mallet Wyvern"), new KillObjective(1, "boss_Malletwyvern") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 60156: Thorough Preparations
//-----------------------------------------------------------------------------
public class Chaple576Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60156);
		SetName(L("Thorough Preparations"));
		SetDescription(L("Vaidutis has lost his Holy Stones. Gather orb crystals for new ones."));
		SetType(QuestType.Repeat);
		SetLocation("d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Talk to Follower Vaidutis"), L("Follower Vaidutis on the Ground Floor of Tenet Church is waiting for help."));
		SetPhase(QuestStatus.InProgress, "CHAPLE576_RP_1_OBJ_0", "d_chapel_57_6", L("Collect Orb Crystals"), L("Collect orb crystals near the Worship Anteroom and Nuosirdum Chapel."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Report back to Follower Vaidutis"), L("Take the orb crystals to Follower Vaidutis."));

		AddPrerequisite(new QuestStatusPrerequisite(8525, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(34));

		AddObjective("collectOrbs", L("Collect Orb Crystals"), new CollectItemObjective("CHAPLE576_RP_1_ITEM", 7));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CHAPLE576_RP_1_ITEM"));
	}
}
