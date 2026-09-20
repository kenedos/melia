//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 2F Quest NPCs
//--- Description -----------------------------------------------------------
// The guardian statues that still hold their reason, the stone lanterns
// Rexipher comes for, and the hidden place the Great King left behind.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel33QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20164);
	private readonly static QuestId Mq02 = new QuestId(20165);
	private readonly static QuestId Mq03 = new QuestId(20184);
	private readonly static QuestId Mq04 = new QuestId(20185);
	private readonly static QuestId Mq05 = new QuestId(20186);
	private readonly static QuestId Sq01 = new QuestId(8433);
	private readonly static QuestId Sq02 = new QuestId(8434);
	private readonly static QuestId Sq03 = new QuestId(8435);
	private readonly static QuestId Sq04 = new QuestId(8436);
	private readonly static QuestId Sq05 = new QuestId(8437);
	private readonly static QuestId Rp1 = new QuestId(60172);

	private const int MagicSourcesNeeded = 8;

	private readonly static double[,] GuardianEnergySpots =
	{
		{ -1671, 173 }, { -562, 111 }, { -858, 296 }, { -1546, 370 },
		{ -1321, 318 }, { -1528, 157 }, { -1103, 206 }, { -878, 134 },
	};

	private readonly static double[,] LanternSpots =
	{
		{ -210, -882 }, { -379, -723 }, { -208, -566 }, { 3, -722 },
	};

	protected override void Load()
	{
		// Guardian Stone Statue, at the stairs up from the first floor
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Guardian Stone Statue"), "ZACHARIEL33_GUARDIAN1", "d_zachariel_33", -139, -2186, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Guardian Stone Statue"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("The rules of the Royal Mausoleum are in place."));
				await dialog.Msg(L("We can prevent the evil beings from getting near the revelation."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				var answer = await dialog.SelectQuestOffer(Mq01, L("The evil presence has caused the Guardians to break the Royal Slate, which contains the rules of the Royal Mausoleum. Defeat the Guardians and bring back the pieces."),
					Option(L("Retrieve the Royal Slate pieces"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("With the Great King's will weakened within them, some Guardians now prioritize the Royal Slate more than the revelation."));
					await dialog.Msg(L("It's tragic."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Five pieces. The guardians of this hall carry them."));
				return;
			}

			await dialog.Msg(L("Everything at the Royal Mausoleum is designed for the revelation."));
		});

		// Guardian Stone Statue, by the stone lanterns
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Guardian Stone Statue"), "ZACHARIEL33_GUARDIAN2", "d_zachariel_33", -188, -1420, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Guardian Stone Statue"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("The lanterns are still burning. That one is beyond calling back."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("Those Guardians are trying to break a stone lantern of the Royal Mausoleum. Stop them. They are out of control."),
					Option(L("I'll guard the stone lantern of the Royal Mausoleum"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					character.Quests.StartQuestTrack(Mq02);
					await dialog.Msg(L("Everything at the Royal Mausoleum is designed for the revelation."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("This evil is very real. It could possibly reach the deepest parts of the Royal Mausoleum."),
					Option(L("I'll follow Rexipher's whereabouts"), "accept"),
					Option(L("Don't chase"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.Quests.StartQuestTrack(Mq03);
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The guardian is still at the lanterns."));
				character.Quests.ReplayQuestTrack(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("He went down the lantern hall. Follow him."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("The flow of magic in the Royal Mausoleum breaks if the stone lanterns do."));
		});

		// Royal Mausoleum Stone Lanterns
		//-------------------------------------------------------------------------
		for (var i = 0; i < LanternSpots.GetLength(0); i++)
		{
			var uniqueName = "ZACHA2F_MQ02_HIDENPC" + (i + 1);
			AddConditionalNpc(47253, L("Royal Mausoleum Stone Lantern"), uniqueName, "d_zachariel_33", LanternSpots[i, 0], LanternSpots[i, 1], 0, c => !c.Quests.HasCompleted(Mq03), async dialog =>
			{
				dialog.SetTitle(L("Royal Mausoleum Stone Lantern"));
				await dialog.Msg(L("A stone lantern of the Royal Mausoleum. The floor's magic runs through it."));
			});
		}

		// Royal Mausoleum Tombstone
		//-------------------------------------------------------------------------
		AddNpc(47252, L("Royal Mausoleum Tombstone"), "ZACHA2F_MQ_04", "d_zachariel_33", -182, -67, 11, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Tombstone"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("The hall is quiet. The Echad that could not tell friend from foe are down."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("The Guardians will cause a disturbance when the magic that flows in the Royal Mausoleum gets disrupted."),
					Option(L("Seems like it'd be better to defeat some guardians"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Quests.StartQuestTrack(Mq04);
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The Echad are still on the gallery floor above."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("The Guardians will cause a disturbance when the magic that flows in the Royal Mausoleum gets disrupted."));
		});

		// Broken Royal Mausoleum Tombstone
		//-------------------------------------------------------------------------
		AddNpc(47252, L("Broken Royal Mausoleum Tombstone"), "ZACHA2F_MQ_05", "d_zachariel_33", 31.75, 1219.78, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Broken Royal Mausoleum Tombstone"));

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Shnayim is down. The hall past the tombstone is open."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Guardian Shnayim. Defeat the intruder who disturbs the Great King Zachariel's rest."),
					Option(L("Defeat Shnayim"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.Quests.StartQuestTrack(Mq05);
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Shnayim still holds the deep hall."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("A tombstone of the Royal Mausoleum, cracked from top to base."));
		});

		// Secret Location Manual
		//-------------------------------------------------------------------------
		AddNpc(47254, L("Secret Location Manual"), "ZACHA2F_SQ", "d_zachariel_33", -1118, 318, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Location Manual"));

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("We hid the secret treasures for the Revelator who would come someday. Find and absorb the Guardians' hidden energy to open the secret door."),
					Option(L("Let's gather the force of the guardians and find the hidden treasure"), "accept"),
					Option(L("Not interested"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq01);

				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("The guardians' energy hangs in the west gallery. Absorb it."));
				return;
			}

			await dialog.Msg(L("We hid the secret treasures for the Revelator who would come someday."));
		});

		// Guardian's Energy
		//-------------------------------------------------------------------------
		for (var i = 0; i < GuardianEnergySpots.GetLength(0); i++)
		{
			var uniqueName = "ZACHA2F_SQ_01_ENERGY" + (i + 1);
			AddNpc(147469, L("Guardian's Energy"), uniqueName, "d_zachariel_33", GuardianEnergySpots[i, 0], GuardianEnergySpots[i, 1], 0, this.AbsorbGuardianEnergy);
		}

		// Guardian of the Royal Family's Secret Treasure
		//-------------------------------------------------------------------------
		AddConditionalNpc(47260, L("Guardian of the Royal Family's Secret Treasure"), "WS_ZACHA2F_01_TO_02", "d_zachariel_33", -1589.43, -106.24, 86, c => c.Quests.HasCompleted(Sq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Guardian of the Royal Family's Secret Treasure"));

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("You are qualified to receive the secret treasure."),
					Option(L("Find the treasure"), "accept"),
					Option(L("Cancel"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("The door behind me is open. What is on the other side of it was left there to keep the treasure."));
					character.Warp("d_zachariel_33", -1532, 684, -480);
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Tomb Lord is still standing over the treasure."));
				character.Quests.ClearQuestTrack(Sq02);
				character.Warp("d_zachariel_33", -1532, 684, -480);
				return;
			}

			await dialog.Msg(L("The secret place of the Royal Family is behind me."));
		});

		// The Royal Family's Secret Treasure
		//-------------------------------------------------------------------------
		AddConditionalNpc(45320, L("Royal Family's Secret Treasure"), "ZACHA2F_SQ02_TREASURE", "d_zachariel_33", -2087.16, -823.13, 91, c => c.Quests.IsActive(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Family's Secret Treasure"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("The ark opens. What the Royal Family left for the Revelator is inside it."));
				await dialog.CompleteQuest(Sq02);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("An ark of the Royal Family, sealed while Tomb Lord still stands."));
		});

		// Guardian Stone Statue, at the magic source gallery
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Guardian Stone Statue"), "ZACHA2F_SQ_03", "d_zachariel_33", 372.91, 250.47, 353, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Guardian Stone Statue"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Good. The provision to the magical power shortage will now proceed."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("The demons have cut off the regulatory magic source. Please gather magical power from the nearby Guardians."),
					Option(L("I'll gather the magic"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("If the control magic is dispelled, the divine messages won't be protected."));
				}
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("A path has been opened. Restore magical power by reigniting the stone lanterns of the Royal Mausoleum."),
					Option(L("Let's light the stone lanterns of the Royal Mausoleum"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("When the regulatory magic flows yet again, the Guardians will regain their reason."));
					await dialog.Msg(L("But that will take a long time."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Eight blazing magic sources. Vikaras and Vekarabe carry them."));
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("The side path leads to the lantern. Light it with what you gathered."));
				return;
			}

			await dialog.Msg(L("If the control magic is dispelled, the divine messages won't be protected."));
		});

		// Royal Mausoleum Stone Lantern, past the side path
		//-------------------------------------------------------------------------
		AddNpc(47253, L("Royal Mausoleum Stone Lantern"), "ZACHA2F_SQ_04", "d_zachariel_33", 2108, 470, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Stone Lantern"));

			if (character.Quests.IsActive(Sq04) && !character.Quests.IsCompletable(Sq04))
			{
				if (character.Inventory.CountItem(ItemId.ZACHA2F_SQ_03_ITEM) < MagicSourcesNeeded)
				{
					await dialog.Msg(L("The lantern is cold. It wants eight blazing magic sources to catch."));
					return;
				}

				var lit = await character.TimeActions.StartAsync(L("Lighting it..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (lit != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.ZACHA2F_SQ_03_ITEM, MagicSourcesNeeded);
				character.Quests.CompleteObjective(Sq04, "lightLantern");

				await dialog.Msg(L("The sources go in one by one and the lantern takes the fire."));
				return;
			}

			await dialog.Msg(L("A stone lantern of the Royal Mausoleum, burning steadily."));
		});

		// Guardian Stone Statue, at the Magic Vessels
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Guardian Stone Statue"), "ZACHA2F_SQ_05", "d_zachariel_33", 2284.16, 111.49, 19, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Guardian Stone Statue"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("Order will surely return to this place."));
				await dialog.Msg(L("But it will take a long time."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("Abnormal Guardians are attacking the Magic Vessels. Protect the Magic Vessels until the regulatory magic flows again."),
					Option(L("I'll guard the Magic Vessel"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("The Magic Vessels are like the heart of the Royal Mausoleum."));
					await dialog.Msg(L("It must never be allowed to be destroyed."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("The vessels are north of here. They are still being struck."));
				character.Quests.ClearQuestTrack(Sq05);
				return;
			}

			await dialog.Msg(L("The Magic Vessels are like the heart of the Royal Mausoleum."));
		});

		// Royal Mausoleum Guardian
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Royal Mausoleum Guardian"), "ZACHA33_RP_1_NPC", "d_zachariel_33", 932.23, -637.05, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Guardian"));

			if (character.Quests.IsActive(Rp1) && character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("Deal with all of the Tainted Guardians."));
				await dialog.Msg(L("That is the fate of the Royal Mausoleum Guardians."));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				var answer = await dialog.SelectQuestOffer(Rp1, L("You must retrieve the essence from the Tainted Guardians. You will be able to interrupt the destruction of the Royal Mausoleum by doing so."),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Rp1);

				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("Nine essences. The tainted ones walk the galleries either side of this hall."));
				return;
			}

			await dialog.Msg(L("I have kept this floor since the Great King was laid down."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The hidden place behind the treasure guardian.
		AddQuestTrigger("ZACHA2F_SQ_02", "d_zachariel_33", -1543, -545, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
				character.Quests.StartQuestTrack(Sq02);

			await Task.CompletedTask;
		});

		// The approach to the Magic Vessels.
		AddQuestTrigger("ZACHA2F_SQ_05_TRIGGER", "d_zachariel_33", 2291.61, -68.01, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq05) && !character.Quests.IsCompletable(Sq05))
				character.Quests.StartQuestTrack(Sq05);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Takes in the energy one of the guardians of the west gallery left behind.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task AbsorbGuardianEnergy(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Guardian's Energy"));

		if (character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
		{
			var absorbed = await character.TimeActions.StartAsync(L("Taking in the energy..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

			if (absorbed != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Sq01, "absorbEnergy");

			await dialog.Msg(L("The energy goes into you, and something heavy turns over in the west wall."));
			character.LookAround();
			return;
		}

		await dialog.Msg(L("What is left of a guardian, hanging in the air where it stood."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20164: Words of the King
//-----------------------------------------------------------------------------
public class Zacha2fMq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20164);
		SetName(L("Words of the King"));
		SetDescription(L("The guardians broke the Royal Slate and are hoarding the pieces."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHARIEL33_GUARDIAN1", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("You have to stop the demons attacking the Royal Mausoleum, while chasing Rexipher. Talk to the Guardian Stone Statue."));
		SetPhase(QuestStatus.InProgress, "ZACHARIEL33_GUARDIAN1", "d_zachariel_33", L("Get the pieces of the Royal Slate"), L("Mausoleum Guardians are hoarding the pieces of the Royal Slate that Rexipher destroyed. Collect the pieces of the slate from the Royal Mausoleum Guardians."));
		SetPhase(QuestStatus.Success, "ZACHARIEL33_GUARDIAN1", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("Collected all pieces of the slate. Talk to the Guardian Stone Statue."));

		AddPrerequisite(new QuestStatusPrerequisite(8600, QuestStatus.Completed));

		AddPityDrop("ZACHA2F_MQ_01_ITEM", 0.65f, 3, 1, "Beetle", "Wolf_statue");

		AddObjective("collectSlate", L("Defeat the Guardians and get the Broken Slate of the Royal Mausoleum"), new CollectItemObjective("ZACHA2F_MQ_01_ITEM", 5));

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new TakeItemReward("ZACHA2F_MQ_01_ITEM"));
	}
}

// 20165: Trick of the Demon (1)
//-----------------------------------------------------------------------------
public class Zacha2fMq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20165);
		SetName(L("Trick of the Demon (1)"));
		SetDescription(L("A guardian out of its mind is trying to break the floor's stone lanterns."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHARIEL33_GUARDIAN2", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("You have to stop the demons attacking the Royal Mausoleum, while chasing Rexipher. Talk to the Guardian Stone Statue."));
		SetPhase(QuestStatus.InProgress, "ZACHARIEL33_GUARDIAN2", "d_zachariel_33", L("Protect the Royal Mausoleum Stone Lantern"), L("The Guardian Stone Statue says the flow of magic in the Royal Mausoleum will be disrupted if the stone lanterns are destroyed. Protect the stone lanterns from demons."));
		SetPhase(QuestStatus.Success, "ZACHARIEL33_GUARDIAN2", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("The lanterns are still standing. Report to the Guardian Stone Statue."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA2F_MQ_02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20164, QuestStatus.Completed));

		// The client runs this phase as a protect-the-lantern minigame; the port
		// makes it the guardian that attacks them.
		AddObjective("killGuardian", L("Put down the guardian attacking the lanterns"), new KillObjective(1, "zachariel_guardian") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
	}
}

// 20184: Trick of the Demon (2)
//-----------------------------------------------------------------------------
public class Zacha2fMq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20184);
		SetName(L("Trick of the Demon (2)"));
		SetDescription(L("Rexipher breaks the lanterns himself and goes down to the third floor."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHARIEL33_GUARDIAN2", "d_zachariel_33", L("Protect the Royal Mausoleum Stone Lantern"), L("The Guardian Stone Statue says the flow of magic in the Royal Mausoleum will be disrupted if the stone lanterns are destroyed. Protect the stone lanterns from demons."));
		SetPhase(QuestStatus.InProgress, "ZACHARIEL33_GUARDIAN2", "d_zachariel_33", L("Protect the Royal Mausoleum Stone Lantern"), L("The Guardian Stone Statue says the flow of magic in the Royal Mausoleum will be disrupted if the stone lanterns are destroyed. Protect the stone lanterns from demons."));
		SetPhase(QuestStatus.Success, "ZACHARIEL33_GUARDIAN2", "d_zachariel_33", L("Follow Rexipher to Royal Mausoleum 3F"), L("Rexipher destroyed the stone lantern and disappeared to a deeper area. Follow him to the 3rd Floor."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA2F_MQ_03_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(20165, QuestStatus.Completed));

		AddObjective("seeRexipher", L("See what breaks the stone lanterns"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The cutscene is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Follow Rexipher to Royal Mausoleum 3F!"));
		character.Quests.Complete(this.QuestId);
		character.LookAround();
	}
}

// 20185: Friend or Foe Error
//-----------------------------------------------------------------------------
public class Zacha2fMq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20185);
		SetName(L("Friend or Foe Error"));
		SetDescription(L("The Echad of the upper gallery are cutting down anything that moves, their own included."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA2F_MQ_04", "d_zachariel_33", L("Read the Royal Mausoleum gravestone"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA2F_MQ_04", "d_zachariel_33", L("Defeat the corrupted Guardians"), L("A twisted spell is causing the Guardians to attack both enemies and allies. Defeat the corrupted Guardians."));
		SetPhase(QuestStatus.Success, "ZACHA2F_MQ_04", "d_zachariel_33", L("Read the gravestone again"), L("The Echad are down. Read the gravestone again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA2F_MQ_04_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(75));

		AddObjective("killEchad", L("Defeat the Echad that can't identify friend or foe"), new KillObjective(7, "Echad") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 20186: Deviated Guardian
//-----------------------------------------------------------------------------
public class Zacha2fMq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20186);
		SetName(L("Deviated Guardian"));
		SetDescription(L("Shnayim reads the Revelator as the intruder and holds the deep hall against them."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA2F_MQ_05", "d_zachariel_33", L("Search the deeper part of the Royal Mausoleum"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA2F_MQ_05", "d_zachariel_33", L("Defeat the Shnayim that thinks you are a foe"), L("Much of the Royal Mausoleum is not functioning well because of the corrupt magic. Defeat the Shnayim that identifies you as an enemy."));
		SetPhase(QuestStatus.Success, "ZACHA2F_MQ_05", "d_zachariel_33", L("Read the tombstone again"), L("Shnayim is down. Read the tombstone again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA2F_MQ_05_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(76));

		AddObjective("killShnayim", L("Defeat Shnayim"), new KillObjective(1, "boss_Shnayim_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("FOOT02_122", 1));
	}
}

// 8433: Hidden Place (1)
//-----------------------------------------------------------------------------
public class Zacha2fSq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8433);
		SetName(L("Hidden Place (1)"));
		SetDescription(L("The guardians' hidden energy opens the secret door of the west gallery."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA2F_SQ", "d_zachariel_33", L("Read the Secret Location Manual"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA2F_SQ_01_ENERGY1", "d_zachariel_33", L("Obtain the Guardian Energy"), L("Find and absorb the Guardians' hidden energy to open the secret door."));
		SetPhase(QuestStatus.Success, "ZACHA2F_SQ_01_ENERGY1", "d_zachariel_33", L("Obtain the Guardian Energy"), L("Find and absorb the Guardians' hidden energy to open the secret door."));

		AddPrerequisite(new LevelPrerequisite(75));

		AddObjective("absorbEnergy", L("Obtain the Guardian Energy"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The absorbing is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Guardian of the Royal Family's Secret Treasure has opened its mouth."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8434: Hidden Place (2)
//-----------------------------------------------------------------------------
public class Zacha2fSq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8434);
		SetName(L("Hidden Place (2)"));
		SetDescription(L("Tomb Lord stands between the secret door and what the Royal Family hid behind it."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "WS_ZACHA2F_01_TO_02", "d_zachariel_33", L("Move to the secret location"), L("The Guardian of the Royal Family's Secret Treasure has opened the way. Move to the secret location."));
		SetPhase(QuestStatus.InProgress, "ZACHA2F_SQ_02", "d_zachariel_33", L("Defeat Tomb Lord"), L("Tomb Lord was left to keep the secret place. Defeat it."));
		SetPhase(QuestStatus.Success, "ZACHA2F_SQ02_TREASURE", "d_zachariel_33", L("Find the secret treasure of the Royal Family"), L("Tomb Lord is down. Open the ark of the Royal Family."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA2F_SQ_02_TRACK", 7000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8433, QuestStatus.Completed));

		AddObjective("killTombLord", L("Defeat Tomb Lord"), new KillObjective(1, "boss_TombLord_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("misc_BRC03_105_1", 1));
		AddReward(new SelectItemReward("HAND02_145", "HAND02_146", "HAND02_147"));
	}
}

// 8435: Emergency (1)
//-----------------------------------------------------------------------------
public class Zacha2fSq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8435);
		SetName(L("Emergency (1)"));
		SetDescription(L("The demons cut the regulatory magic source. The guardians of the gallery still carry some."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA2F_SQ_03", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("The demons have cut off the regulatory magic source. Talk to the Guardian Stone Statue."));
		SetPhase(QuestStatus.InProgress, "ZACHA2F_SQ_03", "d_zachariel_33", L("Collect the Blazing Magic Sources"), L("Please gather magical power from the nearby Guardians."));
		SetPhase(QuestStatus.Success, "ZACHA2F_SQ_03", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("You have gathered enough magic. Talk to the Guardian Stone Statue."));

		AddPrerequisite(new LevelPrerequisite(76));

		AddPityDrop("ZACHA2F_SQ_03_ITEM", 0.65f, 3, 1, "Wolf_statue", "Beetle");

		AddObjective("collectSources", L("Obtain Blazing Magic Sources by defeating Vikaras and Vekarabe"), new CollectItemObjective("ZACHA2F_SQ_03_ITEM", 8));

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 8436: Emergency (2)
//-----------------------------------------------------------------------------
public class Zacha2fSq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8436);
		SetName(L("Emergency (2)"));
		SetDescription(L("The gathered magic goes back into the mausoleum through its stone lantern."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA2F_SQ_03", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("A path has been opened. Restore magical power by reigniting the stone lanterns of the Royal Mausoleum."));
		SetPhase(QuestStatus.InProgress, "ZACHA2F_SQ_04", "d_zachariel_33", L("Light up the stone lantern"), L("Restore magical power by reigniting the stone lantern of the Royal Mausoleum."));
		SetPhase(QuestStatus.Success, "ZACHA2F_SQ_04", "d_zachariel_33", L("Light up the stone lantern"), L("Restore magical power by reigniting the stone lantern of the Royal Mausoleum."));

		AddPrerequisite(new QuestStatusPrerequisite(8435, QuestStatus.Completed));

		AddObjective("lightLantern", L("Light up the stone lantern"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The lighting is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The lantern is burning again and the regulatory magic has somewhere to run."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8437: Emergency (3)
//-----------------------------------------------------------------------------
public class Zacha2fSq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8437);
		SetName(L("Emergency (3)"));
		SetDescription(L("The Magic Vessels have to hold until the regulatory magic flows again."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA2F_SQ_05", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("Abnormal Guardians are attacking the Magic Vessels. Talk to the Guardian Stone Statue."));
		SetPhase(QuestStatus.InProgress, "ZACHA2F_SQ_05_TRIGGER", "d_zachariel_33", L("Defeat the Guardians near the Magic Vessels"), L("Protect the Magic Vessels until the regulatory magic flows again."));
		SetPhase(QuestStatus.Success, "ZACHA2F_SQ_05", "d_zachariel_33", L("Talk to the Guardian Stone Statue"), L("The vessels held. Talk to the Guardian Stone Statue."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA2F_SQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8436, QuestStatus.Completed));

		AddObjective("clearVessels", L("Defeat the Guardians near the Magic Vessels"), new KillObjective(7, "Tombsinker", "Karas") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
	}
}

// 60172: Protect the Royal Mausoleum
//-----------------------------------------------------------------------------
public class Zacha33Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60172);
		SetName(L("Protect the Royal Mausoleum"));
		SetDescription(L("The essence out of the tainted guardians slows what is eating the mausoleum."));
		SetType(QuestType.Repeat);
		SetLocation("d_zachariel_33");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA33_RP_1_NPC", "d_zachariel_33", L("Talk with the Royal Mausoleum Guardian"), L("The Royal Mausoleum Guardian is waiting for the Revelator."));
		SetPhase(QuestStatus.InProgress, "ZACHA33_RP_1_NPC", "d_zachariel_33", L("Retrieve the Guardian's Essence"), L("You must retrieve the essence from the Tainted Guardians."));
		SetPhase(QuestStatus.Success, "ZACHA33_RP_1_NPC", "d_zachariel_33", L("Report back to the Royal Mausoleum Guardian"), L("You have collected enough essence. Go back to the Royal Mausoleum Guardian."));

		AddPrerequisite(new LevelPrerequisite(74));

		AddPityDrop("ZACHA33_RP_1_ITEM", 0.75f, 2, 1, "Beetle", "Vesper", "Wolf_statue", "Tombsinker", "Beetle_Elite");

		AddObjective("collectEssence", L("Collect the Guardian's Essence"), new CollectItemObjective("ZACHA33_RP_1_ITEM", 9));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new TakeItemReward("ZACHA33_RP_1_ITEM"));
	}
}
