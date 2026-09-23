//--- Melia Script ----------------------------------------------------------
// Tenet Church 1F Quest NPCs
//--- Description -----------------------------------------------------------
// Vaidutis and Donatas at the church gate, and the altars and demons their
// quests run on.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
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
	private readonly static QuestId Mq07 = new QuestId(8451);
	private readonly static QuestId Mq08 = new QuestId(8517);
	private readonly static QuestId Mq09 = new QuestId(8518);
	private readonly static QuestId Mq0905 = new QuestId(8527);
	private readonly static QuestId Rp1 = new QuestId(60156);

	protected override void Load()
	{
		// The Chapparition, on its gentype anchor in the church
		//-------------------------------------------------------------------------
		AddSpawner("d_chapel_57_6.Chapparition", MonsterId.F_Boss_Chapparition, min: 1, max: 1, respawn: Minutes(5));
		AddSpawnPoint("d_chapel_57_6.Chapparition", "d_chapel_57_6", Rectangle(217.25, 460.63, 40));

		// Follower Vaidutis
		//-------------------------------------------------------------------------
		AddNpc(147400, L("Follower Vaidutis"), "CHAPEL_VIRGINIJA", "d_chapel_57_6", 961, -114, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Vaidutis"));

			if (character.Quests.IsActive(Mq0905) && character.Quests.IsCompletable(Mq0905))
			{
				await dialog.Msg(L("Is Brother Vaidutis alright? I came running when I heard the barrier break."));

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
					character.Quests.Start(Rp1);

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
		AddNpc(147399, L("Follower Donatas"), "CHAPEL576_DONATAS", "d_chapel_57_6", -1674, 374, 110, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Donatas"));

			if (character.Quests.IsActive(Mq041) && character.Quests.IsCompletable(Mq041))
			{
				await dialog.Msg(L("It is an honor to fight with you, Revelator."));
				await dialog.Msg(L("Could you drive the demons from this area while Follower Algis investigates?"));
				await dialog.CompleteQuest(Mq041);
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
				await dialog.Msg(L("Use the scroll and lure the demons to the Apsauga Altar."));
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
		AddNpc(147379, L("Church Gate"), "CHAPLE576_MQ_04", "d_chapel_57_6", -1778, 426, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Church Gate"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("The gate groans open under the Light Crystal's glow."));
				await dialog.CompleteQuest(Mq02);

				if (!character.Quests.Has(Mq041) && character.Quests.MeetsPrerequisites(Mq041))
					character.Quests.Start(Mq041);

				return;
			}

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You set the Light Crystal against the demonic barrier. Something powerful turns toward it."));
				character.Quests.StartQuestTrack(Mq02);
				return;
			}

			if (!character.Quests.Has(Mq041) && character.Quests.MeetsPrerequisites(Mq041))
			{
				await dialog.Msg(L("The barrier is broken. Algis steps through the gate."));
				character.Quests.Start(Mq041);
				return;
			}

			await dialog.Msg(L("The gate stands open. The demons have not come this far yet."));
		});

		// Central Altar
		//-------------------------------------------------------------------------
		AddNpc(147358, L("Central Altar"), "CHAPLE576_MQ_09", "d_chapel_57_6", -523, 446, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Central Altar"));

			if (character.Quests.IsActive(Mq09) && !character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("You touch the altar and the pillar of light snaps out. A Mallet Wyvern drops from the rafters."));
				var checkedAltar = await character.TimeActions.StartAsync(L("Checking the altar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (checkedAltar != TimeActionResult.Completed)
					return;

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

		// Orb Crystal
		//-------------------------------------------------------------------------
		AddNpc(153105, L("Orb Crystal"), "CHAPLE576_RP_1_OBJ", "d_chapel_57_6", 343, 13, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Orb Crystal"));

			if (character.Quests.IsActive(Rp1) && !character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("You prise an orb crystal loose from the cluster."));
				character.Inventory.Add(664092, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Rp1, "collectOrbs");
				return;
			}

			await dialog.Msg(L("A cluster of dull orb crystals."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The Apsauga Altar, where the disguised demons are lured.
		AddQuestTrigger("CHAPEL576_MQ_06_LURE", "d_chapel_57_6", -526, -1092, 350, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
				character.Quests.CompleteObjective(Mq06, "lureDemons");

			await Task.CompletedTask;
		});

		// The Globejas Altar, where the demons are converted.
		AddQuestTrigger("CHAPEL576_MQ_08_TRIGGER", "d_chapel_57_6", -523, 1948, 300, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq08) && !character.Quests.IsCompletable(Mq08))
				character.Quests.CompleteObjective(Mq08, "convertDemons");

			await Task.CompletedTask;
		});
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
		SetPhase(QuestStatus.Success, "CHAPLE576_MQ_04", "d_chapel_57_6", L("Open the church entrance"), L("Open the church entrance."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE576_MQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8510, QuestStatus.Completed));

		AddObjective("killMummyghast", L("Defeat Mummyghast"), new KillObjective(1, "boss_Mummyghast") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("CHAPLE576_MQ_02_ITEM_1"));
	}
}

// 8513: Demon Sisters
//-----------------------------------------------------------------------------
public class Chaple576Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8513);
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

		SetPhase(QuestStatus.Possible, "CHAPLE576_MQ_04", "d_chapel_57_6", L("Open the Gate"), L("Open the church gates."));
		SetPhase(QuestStatus.InProgress, "CHAPLE576_MQ_04", "d_chapel_57_6", L("Speak with Follower Algis"), L("Speak with Follower Algis at the gate."));
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

		AddObjective("lureDemons", L("Lure Pawndel and Pawnd to the Apsauga Altar"), new ManualObjective());

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

		AddObjective("convertDemons", L("Convert demons at the Globejas Altar"), new ManualObjective());

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
		SetPhase(QuestStatus.InProgress, "CHAPLE576_RP_1_OBJ", "d_chapel_57_6", L("Collect Orb Crystals"), L("Collect orb crystals near the Worship Anteroom and Nuosirdum Chapel."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Report back to Follower Vaidutis"), L("Take the orb crystals to Follower Vaidutis."));

		AddPrerequisite(new QuestStatusPrerequisite(8525, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(34));

		AddObjective("collectOrbs", L("Collect Orb Crystals"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CHAPLE576_RP_1_ITEM"));
	}
}
