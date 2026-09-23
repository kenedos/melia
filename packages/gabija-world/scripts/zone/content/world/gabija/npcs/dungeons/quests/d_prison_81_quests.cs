//--- Melia Script ----------------------------------------------------------
// Kalejimas Workshop Quest NPCs
//--- Description -----------------------------------------------------------
// The restrained piece of Zanas' soul, the King's Yellow Jewel, and the
// fourth demon barrier.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
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

public class DPrison81QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Prison80Mq10 = new QuestId(30173);
	private readonly static QuestId Mq1 = new QuestId(30174);
	private readonly static QuestId Mq2 = new QuestId(30175);
	private readonly static QuestId Mq3 = new QuestId(30176);
	private readonly static QuestId Mq4 = new QuestId(30177);
	private readonly static QuestId Mq5 = new QuestId(30178);
	private readonly static QuestId Mq6 = new QuestId(30179);
	private readonly static QuestId Mq7 = new QuestId(30180);
	private readonly static QuestId Mq8 = new QuestId(30181);
	private readonly static QuestId Mq9 = new QuestId(30182);
	private readonly static QuestId Mq10 = new QuestId(30183);
	private readonly static QuestId Sq1 = new QuestId(30200);
	private readonly static QuestId Sq3 = new QuestId(30202);

	private const string ZanasPortrait = "Dlg_port_zanas_prison";
	private const string LightCrystalVar = "Gabija.Prison81.LightCrystal";
	private const string ShardMaskVar = "Gabija.Prison81.ShardMask";
	private const string NumberVar = "Gabija.Prison81.Number";
	private const string NumberKillsVar = "Gabija.Prison81.NumberKills";
	private const string PassSentence = "trusts";
	private const int SoulStonesNeeded = 10;
	private const int AllShards = 0b1111;

	// The cell whose teleportation magic still answers the pass-sentence.
	private const int TeleportCell = 3;

	private readonly static Position OtherSide = new Position(-883, 170, 601);

	private readonly static double[,] CellWallSpots =
	{
		{ -1156.94, -211.58 }, { -949.42, -207.51 }, { -783.26, -212.56 },
	};

	private readonly static double[,] LightCrystalSpots =
	{
		{ 447.65, 965.17 }, { 383.61, 736.49 }, { 567.38, 959.94 },
		{ 567.43, 729.10 }, { 694.72, 954.87 }, { 701.00, 758.19 },
	};

	private readonly static int[] ShardItems =
	{
		ItemId.PRISON_81_MQ_5_ITEM_1, ItemId.PRISON_81_MQ_5_ITEM_2, ItemId.PRISON_81_MQ_5_ITEM_3, ItemId.PRISON_81_MQ_5_ITEM_4,
	};

	private readonly static string[] WorkshopMonsters = { "Nuka_blue", "Elma_blue", "TerraNymph_bow_brown" };

	protected override void Load()
	{
		// Zanas' Soul, at the Reintegration Workshop
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_81_NPC_1", "d_prison_81", -1171, -1044, 225, this.IsZanasAtTheWorkshop, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq1) && character.Quests.IsCompletable(Mq1))
			{
				await dialog.Msg(L("Darn, a dead end."));
				await dialog.Msg(L("They must have noticed us."));
				await dialog.Msg(L("But then again, it is pretty hard to miss the work of art we've left in the solitary confinement area."));
				await dialog.Msg(L("Looking at how things are at the moment, we should obtain a different soul piece."));
				await dialog.CompleteQuest(Mq1);
				return;
			}

			if (!character.Quests.Has(Mq1) && character.Quests.MeetsPrerequisites(Mq1))
			{
				var talked = await character.TimeActions.StartAsync(L("Talking to Zanas' Spirit"), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));

				if (talked != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The demons surely are showing suspicious signs."));
				await dialog.Msg(L("I don't have a good feeling about that..."));

				character.Quests.Start(Mq1);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
			{
				await dialog.Msg(L("I cannot move further due to demon magic. You are on your own for now."));
				await dialog.Msg(L("But fortunately, I can tell you the way to get to the other side."));
				await dialog.Msg(L("A cell in Individual Workshop has teleportation magic."));
				await dialog.Msg(L("It is something made by King Kadumel as a contingency plan."));
				await dialog.Msg(L("The magic is activated upon uttering a special sentence."));
				await dialog.Msg(L("I only remember one pass-sentence..."));

				var answer = await dialog.SelectQuestOffer(Mq2, L("So, you would have to find the cell with the right code."),
					Option(L("Ask what the special Crest is"), "accept"),
					Option(L("Say that you should wait until the Demon Magic is undone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2);
					await dialog.Msg(L("The sentence that works the magic is 'The king trusts no one'."));
					await dialog.Msg(L("That is King Kadumel all over, isn't it?"));
					await dialog.Msg(L("Once you are across, find the Soul Stones of Restrainment on the monsters first."));
					await dialog.Msg(L("You will need them to free my other soul, held at the watchtower."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq1))
			{
				await dialog.Msg(L("The demons surely are showing suspicious signs."));
				character.Quests.ReplayQuestTrack(Mq1);
				return;
			}

			if (character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("When you say the correct phrase in the right cell, you will be able to get to the other side."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("We need the memories of my spirit that's in the workshop."));
			await dialog.Msg(L("It'll have important clues about the revelation."));
		});

		// The Individual Workshop cell walls
		//-------------------------------------------------------------------------
		for (var i = 0; i < CellWallSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(147469, L("Wall"), "PRISON_81_OBJ_1_" + number, "d_prison_81", CellWallSpots[i, 0], CellWallSpots[i, 1], 90, dialog => this.SpeakToTheWall(dialog, number));
		}

		// The demon magic across the Workshop
		//-------------------------------------------------------------------------
		AddConditionalNpc(147455, "UnvisibleName", "PRISON_81_OBJ_2", "d_prison_81", -607, -734, 90, this.IsTheWayBlocked);

		// The Magic Circle of Restrainment
		//-------------------------------------------------------------------------
		AddConditionalNpc(151115, L("Magic Circle of Restrainment"), "PRISON_81_OBJ_3", "d_prison_81", -1052.10, 1150.18, 90, this.IsZanasRestrained, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Magic Circle of Restrainment"));

			if (!character.Quests.Has(Mq4) && character.Quests.MeetsPrerequisites(Mq4))
			{
				if (character.Inventory.CountItem(ItemId.PRISON_81_MQ_3_ITEM) < SoulStonesNeeded)
				{
					await dialog.Msg(L("The circle will not give way without the Soul Stones of Restrainment."));
					return;
				}

				var released = await character.TimeActions.StartAsync(L("Disarming the Restrainment Magic Circle"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (released != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.PRISON_81_MQ_3_ITEM, character.Inventory.CountItem(ItemId.PRISON_81_MQ_3_ITEM));
				character.Quests.Start(Mq4);
				character.Quests.CompleteObjective(Mq4, "releaseTheCircle");
				character.LookAround();
				character.ServerMessage(L("Restrainment Magic Circle has been disarmed. Talk to Zanas' Spirit."));
				return;
			}

			await dialog.Msg(L("A magic circle holding a soul in place."));
		});

		// Zanas' Soul, at the watchtower
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_81_NPC_2", "d_prison_81", -890, 1230, 0, this.IsZanasAtTheWatchtower, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq4) && character.Quests.IsCompletable(Mq4))
			{
				await dialog.Msg(L("You have saved me."));
				await dialog.Msg(L("Are you perhaps the Revelator?"));
				await dialog.CompleteQuest(Mq4);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq5) && character.Quests.IsCompletable(Mq5))
			{
				await dialog.Msg(L("All the pieces have been brought together."));
				await dialog.Msg(L("Blimey, there is a slight problem."));
				await dialog.Msg(L("The pieces have been contaminated by the evil energy from the monsters."));
				await dialog.CompleteQuest(Mq5);
				return;
			}

			if (!character.Quests.Has(Mq4))
			{
				dialog.SetPortrait(null);
				await dialog.Msg(L("{#666666}*The soul is held fast by the Magic Circle of Restrainment and cannot answer*{/}"));
				return;
			}

			if (!character.Quests.Has(Mq5) && character.Quests.MeetsPrerequisites(Mq5))
			{
				await dialog.Msg(L("Just as I have expected."));
				await dialog.Msg(L("It is indeed marvelous to see that my other spirits have not failed."));
				await dialog.Msg(L("You have come this far, so perhaps, it is quite safe to assume that you are in fact seeking the King's Jewels. Am I correct in assuming so?"));
				await dialog.Msg(L("If my memory serves me rightfully, there are two King's Jewels and one of them is hidden inside the Storage."));
				await dialog.Msg(L("Alas, the other one has fallen into the hands of Nebulas..."));

				var answer = await dialog.SelectQuestOffer(Mq5, L("No all is lost, let us take comfort in that and find the yellow one first in the Workshop."),
					Option(L("Say that you think it is a good idea"), "accept"),
					Option(L("Say that you think it is better to head directly to Nebulas"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq5);
					await dialog.Msg(L("The King's Yellow Jewel is stored in the secret device located in the Supply Room."));
					await dialog.Msg(L("There are four fragments each with unique properties such as Fire, Ice, Electricity and Poison needed to open the secret device."));
					await dialog.Msg(L("It originally belonged to me but, I am terribly ashamed to say this, got taken away by the monsters."));
					await dialog.Msg(L("It should be around here. We must retrieve it from the monsters."));
				}
				return;
			}

			if (!character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6))
			{
				await dialog.Msg(L("We ought to purify them."));
				await dialog.Msg(L("Providentially, there is a way to perform such task."));
				await dialog.Msg(L("The top of the Supply Room, the part where lights are in, there will be Light Crystals."));
				await dialog.Msg(L("Shine the pieces with its light. That would purify them."));
				await dialog.Msg(L("Doing it once or twice will not be sufficient enough."));

				var answer = await dialog.SelectQuestOffer(Mq6, L("You must keep shining the light at them until all the evil energy is casted out."),
					Option(L("Say that you will purify the Shards"), "accept"),
					Option(L("I don't think that's necessary"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= LightCrystalSpots.GetLength(0); ++i)
						character.Variables.Perm.Remove(LightCrystalVar + i);

					character.Quests.Start(Mq6);
					await dialog.Msg(L("After the purification process is complete, head down to Supply Room."));
					await dialog.Msg(L("Open the secret device there and obtain the yellow gem."));
					await dialog.Msg(L("Place the pieces into the spots around the secret device to open it."));
					await dialog.Msg(L("I am still under Dominance Magic and it's hindering my movement..."));
					await dialog.Msg(L("As soon as such limitation is removed, I will quickly join you in the Supply Room."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq5))
			{
				await dialog.Msg(L("Curse me for a novie..."));
				await dialog.Msg(L("I should not have never let Nebulas lay his hand on the King's Blue Jewel..."));
				return;
			}

			if (character.Quests.IsActive(Mq6))
			{
				await dialog.Msg(L("I do not profess to know how Light Crystal is formed."));
				await dialog.Msg(L("One could only conjecture that it is a result of collected good aspects of souls fell in this prison..."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("I'm glad you came before it was too late."));
			await dialog.Msg(L("I bet Goddess Laima is the one who guided you here."));
		});

		// Light Crystals
		//-------------------------------------------------------------------------
		for (var i = 0; i < LightCrystalSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(147469, L("Light Crystal"), "PRISON_81_OBJ_4_" + number, "d_prison_81", LightCrystalSpots[i, 0], LightCrystalSpots[i, 1], 90, dialog => this.PurifyTheShards(dialog, number));
		}

		// The Supply Room's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_81_OBJ_5", "d_prison_81", 990, 890, 315, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq7) && character.Quests.IsCompletable(Mq7))
			{
				await dialog.Msg(L("With a shard on every side, the secret device opens. The King's Yellow Jewel lies inside."));
				await dialog.CompleteQuest(Mq7);
				character.Variables.Temp.Remove(ShardMaskVar);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq7) && character.Quests.MeetsPrerequisites(Mq7))
			{
				var looked = await character.TimeActions.StartAsync(L("Examining"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (looked != TimeActionResult.Completed)
					return;

				character.Variables.Temp.Remove(ShardMaskVar);
				character.Quests.Start(Mq7);
				character.ServerMessage(L("Use the Shards on all four sides of the Device"));
				return;
			}

			if (character.Quests.IsActive(Mq7))
			{
				await this.PlaceAShard(dialog);
				return;
			}

			await dialog.Msg(L("A secret device with a socket on each of its four sides."));
		});

		// Zanas' Soul, at the Supply Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_81_NPC_3", "d_prison_81", 897.66, 939.52, 45, this.IsZanasAtTheSupplyRoom, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (!character.Quests.Has(Mq8) && character.Quests.MeetsPrerequisites(Mq8))
			{
				await dialog.Msg(L("You've acquired the Yellow Jewel?"));
				await dialog.Msg(L("What good news."));
				await dialog.Msg(L("I am now able to move again."));
				await dialog.Msg(L("There is a secret device in the Punishment Room."));
				await dialog.Msg(L("It can be activated by a very strong magic attack."));
				await dialog.Msg(L("Once it is activated, we can jolly well blast through the demons and their putrid magic that's standing in our way..."));

				var answer = await dialog.SelectQuestOffer(Mq8, L("All good things are not that easy to earn as we know by now. I do not have the necessary memory in regards to how to operate it."),
					Option(L("Say that there are instructions"), "accept"),
					Option(L("Ponder on how to proceed together"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq8);
					await dialog.Msg(L("My word, you have the manual!"));
					await dialog.Msg(L("Goddesses be praised! It makes the entire thing much easier."));
					await dialog.Msg(L("Activating the secret device in the Punishment Room will surely deal an overwhelming blow to the demons."));
				}
				return;
			}

			if (!character.Quests.Has(Mq9) && character.Quests.MeetsPrerequisites(Mq9))
			{
				await dialog.Msg(L("Righty-O!"));
				await dialog.Msg(L("There might be some monsters left in Segregated Ward."));
				await dialog.Msg(L("The radius of the blast was not that wide."));
				await dialog.Msg(L("We are in this to disable demon barriers..."));
				await dialog.Msg(L("So, eliminating the remaining monsters is in the job description."));
				await dialog.Msg(L("I will reunite with my other spirit."));

				var answer = await dialog.SelectQuestOffer(Mq9, L("I will join you right away."),
					Option(L("Say that you will deal with it all"), "accept"),
					Option(L("Ask for some time to recover before carrying on"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq9);
					await dialog.Msg(L("If I can recollect all my separated spirits, I might be able to find a way to..."));
					await dialog.Msg(L("Defeat Nebulas once and for all."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq8))
			{
				await dialog.Msg(L("This strike would definitely leave a mark on their side."));
				await dialog.Msg(L("And the magic blocking the way is now go as well."));
				return;
			}

			if (character.Quests.IsActive(Mq9))
			{
				await dialog.Msg(L("I can sense something strange."));
				await dialog.Msg(L("It is not a pleasant one, I can surely tell you that."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("We're almost done gathering up my spirits."));
			await dialog.Msg(L("We'll find the revelation in no time."));
		});

		// The Punishment Room's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151110, L("Secret Device"), "PRISON_81_OBJ_6", "d_prison_81", 1396, 820, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq8) && character.Quests.IsCompletable(Mq8))
			{
				var fired = await character.TimeActions.StartAsync(L("Activating Secret Device"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (fired != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The device unleashes its magic across the whole Workshop."));
				await dialog.CompleteQuest(Mq8);
				character.LookAround();
				character.ServerMessage(L("The Demons in the workshop have suffered greatly. Return to Zanas' Spirit."));
				return;
			}

			if (character.Quests.IsActive(Mq8))
			{
				var turned = await character.TimeActions.StartAsync(L("Activating secret device"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (turned != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq8, "turnTheCrystals");
				character.ServerMessage(L("All the Magic Crystals are feeding the device. Activate it."));
				return;
			}

			await dialog.Msg(L("A secret device that answers only to a very strong magic."));
		});

		// The Letter of a Prisoner
		//-------------------------------------------------------------------------
		AddConditionalNpc(147312, L("Letter"), "PRISON_81_SQ_OBJ_1", "d_prison_81", 19.64, 326.97, 90, character => !character.Quests.Has(Sq1), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Letter"));

			if (!character.Quests.MeetsPrerequisites(Sq1))
			{
				await dialog.Msg(L("A letter, dropped on the floor of the Inmate's Lounge."));
				return;
			}

			var read = await character.TimeActions.StartAsync(L("Reading the Letter"), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(2));

			if (read != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.PRISON_81_SQ_1_ITEM, 1, InventoryAddType.PickUp);
			character.Quests.Start(Sq1);
			character.Quests.CompleteObjective(Sq1, "findTheOwner");
			character.LookAround();
			character.ServerMessage(L("A prisoner's letter to his parents in Fedimian. Someone there might know him."));
		});

		// The Interrogation Room Entrance's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151110, L("Secret Device"), "PRISON_81_SQ_OBJ_2", "d_prison_81", 1031.58, -963.26, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			var canStart = !character.Quests.Has(Sq3) && character.Quests.MeetsPrerequisites(Sq3);
			var canReroll = character.Quests.IsActive(Sq3) && !character.Quests.IsCompletable(Sq3);

			if (!canStart && !canReroll)
			{
				await dialog.Msg(L("A mysterious secret device with a number plate on it."));
				return;
			}

			var worked = await character.TimeActions.StartAsync(L("Activating Secret Device"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

			if (worked != TimeActionResult.Completed)
				return;

			var number = System.Random.Shared.Next(5, 10);
			character.Variables.Perm.SetInt(NumberVar, number);
			character.Variables.Perm.SetInt(NumberKillsVar, 0);

			if (canStart)
				character.Quests.Start(Sq3);

			character.ServerMessage(LF("A number appears as soon as you activate the Secret Device: {0}. Think about what the number might mean before acting.", number));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("PRISON_81_MQ_10_TRIGGER", "d_prison_81", 584, -168, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq10) && character.Quests.MeetsPrerequisites(Mq10))
			{
				character.Quests.Start(Mq10);
				character.Quests.CompleteObjective(Mq10, "releaseTheBarrier");
			}
			else if (character.Quests.IsActive(Mq10) && character.Quests.IsCompletable(Mq10))
			{
				character.Quests.ReplayQuestTrack(Mq10);
			}

			await Task.CompletedTask;
		});

		// The Old Man in Fedimian
		//-------------------------------------------------------------------------
		AddNpc(20165, L("Old Man"), "FEDIMIAN_OLDMAN1", "c_fedimian", -899.01, 333.52, 34, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Man"));

			if (character.Quests.IsActive(Sq1) && character.Quests.IsCompletable(Sq1))
			{
				var asked = await character.TimeActions.StartAsync(L("Asking about the Prisoner's Letter"), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));

				if (asked != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Let me have a look."));
				await dialog.Msg(L("Hmm... This was written by Gav. He used to live around here ten years ago."));
				await dialog.Msg(L("I'm too old to remember why he was in prison, but..."));
				await dialog.Msg(L("I know his parents moved to Delmore a few years ago."));
				await dialog.Msg(L("They did return to Fedimian a couple of times after that... but we lost touch lately."));
				await dialog.Msg(L("You can leave the letter to me."));
				await dialog.Msg(L("I'll make sure to deliver it to them when they return."));
				await dialog.Msg(L("I'm sure they will love to have this letter from their son."));
				await dialog.Msg(L("They're his parents, after all..."));
				await dialog.CompleteQuest(Sq1);
				return;
			}

			await dialog.Msg(L("You must have seen many artifacts since coming to this city."));
			await dialog.Msg(L("Yet in our long history, heroes are but a fraction of the dusts."));
			await dialog.Msg(L("Especially Ruklys, one of Maven's disciple... I pity him."));
		});
	}

	/// <summary>
	/// Counts the Workshop monsters defeated toward the number on the
	/// Interrogation Room Entrance's device.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Attacker is not Character character || args.Target is not Mob mob)
			return;

		if (!character.Quests.IsActive(Sq3) || character.Quests.IsCompletable(Sq3))
			return;

		if (!WorkshopMonsters.Contains(mob.Data.ClassName))
			return;

		var number = character.Variables.Perm.GetInt(NumberVar, 0);
		if (number == 0)
			return;

		var kills = character.Variables.Perm.GetInt(NumberKillsVar, 0) + 1;
		character.Variables.Perm.SetInt(NumberKillsVar, kills);

		if (kills < number)
		{
			character.ServerMessage(LF("The number on the device: {0}/{1}", kills, number));
			return;
		}

		character.Variables.Perm.Remove(NumberVar);
		character.Variables.Perm.Remove(NumberKillsVar);
		character.Quests.CompleteObjective(Sq3, "matchTheNumber");
	}

	/// <summary>
	/// Speaks the pass-sentence in one of the Individual Workshop cells.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="number"></param>
	private async Task SpeakToTheWall(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Wall"));

		if (!character.Quests.IsActive(Mq2) || character.Quests.IsCompletable(Mq2))
		{
			await dialog.Msg(L("There's something written here."));
			return;
		}

		await dialog.Msg(L("There's something written here."));

		var answer = await dialog.Select(L("Shall we read some of it?"),
			Option(L("The King believes everyone"), "believes"),
			Option(L("Everyone believes in the King"), "everyone"),
			Option(L("The King trusts noone"), PassSentence),
			Option(L("Noone trusts the King"), "noone"),
			Option(L("Leave it"), "leave")
		);

		if (answer == "leave")
			return;

		if (answer != PassSentence || number != TeleportCell)
		{
			character.ServerMessage(L("Nothing happened. Say the correct phrase or try looking in another room."));
			return;
		}

		character.ServerMessage(L("The Teleportation Magic Circle has been activated and will teleport you to the Prisoner Rest Area"));

		var teleported = await character.TimeActions.StartAsync(L("Teleporting"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

		if (teleported != TimeActionResult.Completed)
			return;

		character.Warp("d_prison_81", OtherSide);
		character.Quests.CompleteObjective(Mq2, "crossOver");
	}

	/// <summary>
	/// Shines a Light Crystal on the defiled shards.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="number"></param>
	private async Task PurifyTheShards(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Light Crystal"));

		if (!character.Quests.IsActive(Mq6) || character.Quests.IsCompletable(Mq6))
		{
			await dialog.Msg(L("A crystal full of warm light, in a sunny spot of the Supply Room."));
			return;
		}

		if (character.Variables.Perm.GetBool(LightCrystalVar + number, false))
		{
			await dialog.Msg(L("This crystal's light has already passed over the shards. Try another."));
			return;
		}

		var purified = await character.TimeActions.StartAsync(L("Cleansing Fragment"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

		if (purified != TimeActionResult.Completed)
			return;

		character.Variables.Perm.SetBool(LightCrystalVar + number, true);

		var count = 0;
		for (var i = 1; i <= LightCrystalSpots.GetLength(0); ++i)
		{
			if (character.Variables.Perm.GetBool(LightCrystalVar + i, false))
				count++;
		}

		if (count < LightCrystalSpots.GetLength(0))
		{
			character.ServerMessage(LF("The evil energy thins out of the shards ({0}/{1}).", count, LightCrystalSpots.GetLength(0)));
			return;
		}

		character.Quests.CompleteObjective(Mq6, "purifyTheShards");
	}

	/// <summary>
	/// Places one of the four shards into a side of the Supply Room's device.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PlaceAShard(Dialog dialog)
	{
		var character = dialog.Player;
		var mask = character.Variables.Temp.GetInt(ShardMaskVar, 0);

		var names = new[] { L("Fire Fragment"), L("Ice Fragment"), L("Lightning Fragment"), L("Poison Fragment") };
		var options = Enumerable.Range(0, ShardItems.Length)
			.Where(i => (mask & (1 << i)) == 0 && character.Inventory.CountItem(ShardItems[i]) > 0)
			.Select(i => Option(names[i], i.ToString()))
			.ToList();

		if (options.Count == 0)
		{
			await dialog.Msg(L("You have no shard left to place."));
			return;
		}

		options.Add(Option(L("Leave it"), "leave"));

		var answer = await dialog.Select(L("What will you use?"), options);

		if (!int.TryParse(answer, out var index))
			return;

		mask |= 1 << index;
		character.Variables.Temp.SetInt(ShardMaskVar, mask);

		if (mask != AllShards)
		{
			character.ServerMessage(LF("{0} fits into the device.", names[index]));
			return;
		}

		character.Quests.CompleteObjective(Mq7, "placeTheShards");
		character.ServerMessage(L("The secret device has been disarmed. Take the King's Yellow Jewel."));
	}

	/// <summary>
	/// Returns whether Zanas' Soul waits at the Reintegration Workshop.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheWorkshop(Character character)
		=> character.Quests.HasCompleted(Prison80Mq10) && !character.Quests.HasCompleted(Mq4);

	/// <summary>
	/// Returns whether the demon magic still blocks the way through the
	/// Workshop.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheWayBlocked(Character character)
		=> character.Quests.Has(Mq1) && !character.Quests.HasCompleted(Mq8);

	/// <summary>
	/// Returns whether the Magic Circle of Restrainment still holds Zanas.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasRestrained(Character character)
		=> character.Quests.HasCompleted(Prison80Mq10) && !character.Quests.Has(Mq4);

	/// <summary>
	/// Returns whether Zanas' Soul waits at the watchtower.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheWatchtower(Character character)
		=> character.Quests.HasCompleted(Prison80Mq10) && !character.Quests.HasCompleted(Mq7);

	/// <summary>
	/// Returns whether Zanas' Soul waits at the Supply Room.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheSupplyRoom(Character character)
		=> character.Quests.HasCompleted(Mq7) && !character.Quests.HasCompleted(Mq10);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30174: The Road Back(1)
//-----------------------------------------------------------------------------
public class Prison81Mq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30174);
		SetName(L("The Road Back(1)"));
		SetDescription(L("Demon magic closes the way through the Workshop."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_NPC_1", "d_prison_81", L("Talk to Zanas' Spirit at the Reintegration Workshop"), L("You have disarmed the Demon Barrier at the Solitary Cells. Go and talk to Zanas' Spirit at the Workshop."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_NPC_1", "d_prison_81", L("Talk to Zanas' Spirit at the Reintegration Workshop"), L("You have disarmed the Demon Barrier at the Solitary Cells. Go and talk to Zanas' Spirit at the Workshop."));
		SetPhase(QuestStatus.Success, "PRISON_81_NPC_1", "d_prison_81", L("Talk to Zanas' Soul"), L("You cannot proceed because of Demon Magic. Talk to Zanas' Spirit about what to do."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON_81_MQ_1_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(30173, QuestStatus.Completed));

		AddObjective("watchTheWay", L("Talk to Zanas' Spirit at the Reintegration Workshop"), new ManualObjective());
	}
}

// 30175: The Road Back(2)
//-----------------------------------------------------------------------------
public class Prison81Mq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30175);
		SetName(L("The Road Back(2)"));
		SetDescription(L("King Kadumel left teleportation magic in a cell, keyed to a sentence."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_NPC_1", "d_prison_81", L("Talk to Zanas' Soul"), L("You can go no further because of Demon Magic. Ask Zanas' Spirit if there is a solution."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_OBJ_1_1", "d_prison_81", L("Search for Teleporation Magic in the Individual Workshop"), L("It is said that King Kadumel installed Teleporation Magic in each cell in case he was imprisoned. Look for that magic that is activated by a special crest that Zanas told you about."));
		SetPhase(QuestStatus.Success, "PRISON_81_OBJ_1_1", "d_prison_81", L("Search for Teleporation Magic in the Individual Workshop"), L("It is said that King Kadumel installed Teleporation Magic in each cell in case he was imprisoned. Look for that magic that is activated by a special crest that Zanas told you about."));

		AddPrerequisite(new QuestStatusPrerequisite(30174, QuestStatus.Completed));

		AddObjective("crossOver", L("Search for Teleporation Magic in the Individual Workshop"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8333));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);

		var next = new QuestId(30176);
		if (!character.Quests.Has(next) && character.Quests.MeetsPrerequisites(next))
			character.Quests.Start(next);
	}
}

// 30176: The Restrained Spirit of Zanas(1)
//-----------------------------------------------------------------------------
public class Prison81Mq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30176);
		SetName(L("The Restrained Spirit of Zanas(1)"));
		SetDescription(L("Soul Stones of Restrainment will break the circle holding Zanas."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "PRISON_81_OBJ_3", "d_prison_81", L("Obtain the Spirit Stone of Restrainment by defeating monsters"), L("You require the Spirit Stone of Restrainment in order to free Zanas' Spirit that is captured by the Demons. Defeat the Monsters to obtain them."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_OBJ_3", "d_prison_81", L("Obtain the Spirit Stone of Restrainment by defeating monsters"), L("You require the Spirit Stone of Restrainment in order to free Zanas' Spirit that is captured by the Demons. Defeat the Monsters to obtain them."));
		SetPhase(QuestStatus.Success, "PRISON_81_OBJ_3", "d_prison_81", L("Obtain the Spirit Stone of Restrainment by defeating monsters"), L("You require the Spirit Stone of Restrainment in order to free Zanas' Spirit that is captured by the Demons. Defeat the Monsters to obtain them."));

		AddPrerequisite(new QuestStatusPrerequisite(30175, QuestStatus.Completed));

		AddObjective("collectStones", L("Obtain the Soul Stone of Restrainment by defeating monsters"), new CollectItemObjective("PRISON_81_MQ_3_ITEM", 10));

		AddPityDrop("PRISON_81_MQ_3_ITEM", 1.0f, 0, 1, "Nuka_blue", "Elma_blue", "TerraNymph_bow_brown");

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8333));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("You have enough Soul Stones. Disarm the Magic Circle of Restrainment to free Zanas' Soul."));
	}
}

// 30177: The Restrained Spirit of Zanas(2)
//-----------------------------------------------------------------------------
public class Prison81Mq4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30177);
		SetName(L("The Restrained Spirit of Zanas(2)"));
		SetDescription(L("The Soul Stones break the Magic Circle of Restrainment."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_OBJ_3", "d_prison_81", L("Rescue Zanas' Spirit by disarming the Magic Circle of Restrainment"), L("You have obtained enough Spirit Stones. Find and disarm the Magic Circle of Retainment to free Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_NPC_2", "d_prison_81", L("Rescue Zanas' Spirit by disarming the Magic Circle of Restrainment"), L("You have obtained enough Spirit Stones. Find and disarm the Magic Circle of Retainment to free Zanas' Spirit."));
		SetPhase(QuestStatus.Success, "PRISON_81_NPC_2", "d_prison_81", L("Talk to the now free Zanas' Spirit"), L("Zanas' Spirit is now free as the magic has been disarmed. Talk to the now free Zanas' Spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(30176, QuestStatus.Completed));

		AddObjective("releaseTheCircle", L("Rescue Zanas' Spirit by disarming the Magic Circle of Restrainment"), new ManualObjective());
	}
}

// 30178: Shard Collection(1)
//-----------------------------------------------------------------------------
public class Prison81Mq5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30178);
		SetName(L("Shard Collection(1)"));
		SetDescription(L("The four shards that open the Supply Room's device are in the monsters' hands."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_NPC_2", "d_prison_81", L("Talk to Zanas' Soul"), L("You require information from Zanas' Spirit about when he was captured by the Demons. Listen to the new information Zanas' Spirit has to offer."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_NPC_2", "d_prison_81", L("Defeat the monsters and obatin for types of Shards"), L("Zanas' Spirit says that he has lost the four Shards of Fire, Ice, Lightning and Poison because they were taken by the Demons. All four are required to disarm the Secret Device in which the King's Yellow Jewel is hidden, so you must recover all of them to obtain the Jewel."));
		SetPhase(QuestStatus.Success, "PRISON_81_NPC_2", "d_prison_81", L("Talk to Zanas' Soul"), L("You have recovered all four of the Jewels. Return to Zanas' Spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(30177, QuestStatus.Completed));

		AddObjective("fireShard", L("Retrieve the Fire Fragment by defeating the monsters"), new CollectItemObjective("PRISON_81_MQ_5_ITEM_1", 1));
		AddObjective("iceShard", L("Retrieve the Ice Fragment by defeating the monsters"), new CollectItemObjective("PRISON_81_MQ_5_ITEM_2", 1));
		AddObjective("lightningShard", L("Retrieve the Lightning Fragment by defeating the monsters"), new CollectItemObjective("PRISON_81_MQ_5_ITEM_3", 1));
		AddObjective("poisonShard", L("Retrieve the Poison Fragment by defeating the monsters"), new CollectItemObjective("PRISON_81_MQ_5_ITEM_4", 1));

		AddPityDrop("PRISON_81_MQ_5_ITEM_1", 0.5f, 3, 1, "Nuka_blue", "Elma_blue", "TerraNymph_bow_brown");
		AddPityDrop("PRISON_81_MQ_5_ITEM_2", 0.3f, 5, 1, "Nuka_blue", "Elma_blue", "TerraNymph_bow_brown");
		AddPityDrop("PRISON_81_MQ_5_ITEM_3", 0.1f, 10, 1, "Nuka_blue", "Elma_blue", "TerraNymph_bow_brown");
		AddPityDrop("PRISON_81_MQ_5_ITEM_4", 0.05f, 15, 1, "Nuka_blue", "Elma_blue", "TerraNymph_bow_brown");

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8333));
	}
}

// 30179: Shard Collection(2)
//-----------------------------------------------------------------------------
public class Prison81Mq6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30179);
		SetName(L("Shard Collection(2)"));
		SetDescription(L("The Light Crystals of the Supply Room drive the evil energy out of the shards."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_NPC_2", "d_prison_81", L("Talk to Zanas' Soul"), L("You have retrieved all four types of Shard but you feel an evil aura from them. Talk to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_OBJ_4_1", "d_prison_81", L("Purify the Shards by using Light Crystal"), L("You must purify the Shards as they have been defiled by the Demons. Use the shining Light Crystal that are in a sunny spot of the Supply room to purify them."));
		SetPhase(QuestStatus.Success, "PRISON_81_OBJ_4_1", "d_prison_81", L("Purify the Shards by using Light Crystal"), L("You must purify the Shards as they have been defiled by the Demons. Use the shining Light Crystal that are in a sunny spot of the Supply room to purify them."));

		AddPrerequisite(new QuestStatusPrerequisite(30178, QuestStatus.Completed));

		AddObjective("purifyTheShards", L("Purify the Shards by using Light Crystal"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8333));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("The shards are purified. Open the Supply Room's secret device."));
	}
}

// 30180: Shard Collection(3)
//-----------------------------------------------------------------------------
public class Prison81Mq7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30180);
		SetName(L("Shard Collection(3)"));
		SetDescription(L("A shard on every side opens the device holding the King's Yellow Jewel."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_OBJ_5", "d_prison_81", L("Check the Secret Device in the Supply Room"), L("All the Shards have been purified. Go to the Supply Room's Secret Device and take the King's Yellow Jewel."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_OBJ_5", "d_prison_81", L("Use the Shards on all four directions of the Secret Device"), L("Supply Room's Secret Device : The Device will be disarmed if you use the shards on all four directions. The type of Shard doesn't matter but you may not use the same one more than once."));
		SetPhase(QuestStatus.Success, "PRISON_81_OBJ_5", "d_prison_81", L("Obtain the King's Yellow Jewel from the Secret Device"), L("The Secret Device has been disarmed. Take the King's Yellow Jewel."));

		AddPrerequisite(new QuestStatusPrerequisite(30179, QuestStatus.Completed));

		AddObjective("placeTheShards", L("Use the Shards on all four directions of the Secret Device"), new ManualObjective());

		AddReward(new ItemReward("PRISON_81_MQ_7_ITEM", 1));
		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8333));
		AddReward(new TakeItemReward("PRISON_81_MQ_5_ITEM_1"));
		AddReward(new TakeItemReward("PRISON_81_MQ_5_ITEM_2"));
		AddReward(new TakeItemReward("PRISON_81_MQ_5_ITEM_3"));
		AddReward(new TakeItemReward("PRISON_81_MQ_5_ITEM_4"));
	}
}

// 30181: Destruction of the Workshop
//-----------------------------------------------------------------------------
public class Prison81Mq8Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30181);
		SetName(L("Destruction of the Workshop"));
		SetDescription(L("The Punishment Room's device strikes the whole Workshop at once."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_NPC_3", "d_prison_81", L("Talk to Zanas' Spirit in the Supply Room"), L("You have obtained the King's Yellow Jewel. Talk to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_OBJ_6", "d_prison_81", L("Activate the Secret Device in the Punishment Room"), L("Punishment Room's Secret Device : Four Magic Crystal will be created when you activate it. Magic Crystals will provide magic to the Device if you turn them so the Crests are facing the Device. When all the Crystals are providing magic to the Device and this will in turn activate a strong offensive skill across all of the Workshop area."));
		SetPhase(QuestStatus.Success, "PRISON_81_OBJ_6", "d_prison_81", L("Activate the Secret Device in the Punishment Room"), L("All Magic Crystals are providing magic to the Secret Device. Activate the magic by using the Secret Device."));

		AddPrerequisite(new QuestStatusPrerequisite(30180, QuestStatus.Completed));

		AddObjective("turnTheCrystals", L("Activate the Secret Device in the Punishment Room"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8333));
	}
}

// 30182: Workshop Barrier(1)
//-----------------------------------------------------------------------------
public class Prison81Mq9Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30182);
		SetName(L("Workshop Barrier(1)"));
		SetDescription(L("The monsters the blast missed stand between the two Zanas and the barrier."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_NPC_3", "d_prison_81", L("Talk to Zanas' Soul"), L("You have dealt massive damage to the Demons by activating the Secret Device. Return to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_MQ_10_TRIGGER", "d_prison_81", L("Defeat the monsters on the way to the Demon Barrier"), L("Zanas' Spirit says that he will go to the Demon Barrier by meeting with the Zanas' Spirit that had been blocked by Demon Magic. Defeat the monsters on the way to the Demon Barrier to help the two Zanas' Spirits meet."));
		SetPhase(QuestStatus.Success, "PRISON_81_MQ_10_TRIGGER", "d_prison_81", L("Defeat the monsters on the way to the Demon Barrier"), L("Zanas' Spirit says that he will go to the Demon Barrier by meeting with the Zanas' Spirit that had been blocked by Demon Magic. Defeat the monsters on the way to the Demon Barrier to help the two Zanas' Spirits meet."));

		AddPrerequisite(new QuestStatusPrerequisite(30181, QuestStatus.Completed));

		AddObjective("clearTheWay", L("Defeat the monsters on the way to the Demon Barrier"), new KillObjective(20, "Nuka_blue", "Elma_blue", "TerraNymph_bow_brown"));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8333));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("The way is clear. Go to the Demon Barrier on the way to the Interrogation Room Entrance."));
	}
}

// 30183: Workshop Barrier(2)
//-----------------------------------------------------------------------------
public class Prison81Mq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30183);
		SetName(L("Workshop Barrier(2)"));
		SetDescription(L("The fourth demon barrier falls, and only one is left."));
		SetType(QuestType.Main);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_MQ_10_TRIGGER", "d_prison_81", L("Release the Demon Barrier"), L("Zanas' Spirit should be able to follow you without issue since you have defeated many monsters. Go to the Demon Barrier on the way to the Interrogation Room Entrance."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_MQ_10_TRIGGER", "d_prison_81", L("Release the Demon Barrier"), L("Zanas' Spirit should be able to follow you without issue since you have defeated many monsters. Go to the Demon Barrier on the way to the Interrogation Room Entrance."));
		SetPhase(QuestStatus.Success, "PRISON_81_MQ_10_TRIGGER", "d_prison_81", L("Release the Demon Barrier"), L("Zanas' Spirit should be able to follow you without issue since you have defeated many monsters. Go to the Demon Barrier on the way to the Interrogation Room Entrance."));

		SetTrack(QuestStatus.Success, QuestStatus.Completed, "PRISON_81_MQ_10_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(30182, QuestStatus.Completed));

		AddObjective("releaseTheBarrier", L("Release the Demon Barrier"), new ManualObjective());
	}
}

// 30200: Letters of a Prisoner
//-----------------------------------------------------------------------------
public class Prison81Sq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30200);
		SetName(L("Letters of a Prisoner"));
		SetDescription(L("A prisoner's letter to his parents never left the Inmate's Lounge."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_81", "c_fedimian");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_SQ_OBJ_1", "d_prison_81", L("Check the letter on the ground at the Inmate's Lounge"), L("There is a letter on the ground of the Inmate's Lounge. Pick it up and read it."));
		SetPhase(QuestStatus.InProgress, "FEDIMIAN_OLDMAN1", "c_fedimian", L("Search for a person that knows the letter's owner"), L("The letter is from a prisoner to his parents. Try to find someone at Fedimian to find that might know the letter's owner."));
		SetPhase(QuestStatus.Success, "FEDIMIAN_OLDMAN1", "c_fedimian", L("Talk to the old man at Fedimian"), L("You have found a person that knows the letter's owner. Try talking to the old man in Fedimian."));

		AddPrerequisite(new LevelPrerequisite(259));

		AddObjective("findTheOwner", L("Search for a person that knows the letter's owner"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8333));
		AddReward(new TakeItemReward("PRISON_81_SQ_1_ITEM"));
	}
}

// 30202: Workshop's Secret Device
//-----------------------------------------------------------------------------
public class Prison81Sq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30202);
		SetName(L("Workshop's Secret Device"));
		SetDescription(L("A number rises on the device, and the monsters of the Workshop are how it is answered."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_81");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_81_SQ_OBJ_2", "d_prison_81", L("Activate the Secret Device in the Interrogation Room Entrance"), L("There is a mysterious Secret Device in the Interrogation Room Entrance. Check it to see what it's function is."));
		SetPhase(QuestStatus.InProgress, "PRISON_81_SQ_OBJ_2", "d_prison_81", L("Defeat as many monsters as the Secret Device is indicating"), L("A number appears as soon as you activate the Secret Device. Think about what the number might mean before acting. You may re-activate the Device to change the number."));
		SetPhase(QuestStatus.Success, "PRISON_81_SQ_OBJ_2", "d_prison_81", L("Defeat as many monsters as the Secret Device is indicating"), L("A number appears as soon as you activate the Secret Device. Think about what the number might mean before acting. You may re-activate the Device to change the number."));

		AddPrerequisite(new LevelPrerequisite(259));

		AddObjective("matchTheNumber", L("Defeat as many monsters as the Secret Device is indicating"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8333));
		AddReward(new ItemReward("Drug_Premium_SP1", 20));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("The number on the device goes dark, and something inside it unlocks."));
	}
}
