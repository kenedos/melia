//--- Melia Script ----------------------------------------------------------
// Ouaas Memorial Quest NPCs
//--- Description -----------------------------------------------------------
// The memorial ground at the head of the pilgrim road, where three suppression
// devices were sunk generations ago and one of them has been broken open.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad415QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Trees the Ground Is Blessed With
		//---------------------------------------------------------------------
		AddNpc(155046, L("[Monk] Matas"), "f_pilgrimroad_41_5", -743, 361, 280, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1001);

			dialog.SetTitle(L("Matas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's leaning on his broom at the edge of the terraces, watching a bare-stripped tree the way a man watches a wound*{/}"));
				await dialog.Msg(L("You'll forgive an old keeper staring. 41 years I have swept this ground. The memorial is consecrated with sap off the Ouaas trees and nothing else, and it is redressed every spring, and I have never once missed a spring."));
				await dialog.Msg(L("The Brown Nuka have been stripping the trees down to the white. Kill 25 of them and bring me 8 measures of sap off them, because that sap is the only sap there is."));

				var response = await dialog.Select(L("Will you go to the trees?"),
					Option(L("I'll bring you 8 measures"), "help"),
					Option(L("What happens if the ground isn't dressed?"), "info"),
					Option(L("Use sap from the lake woods"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The sap dries on their backs in sheets. You want the ones that look wet-shouldered, and you want them before the sun gets at it."));
						break;

					case "info":
						await dialog.Msg(L("For 400 years, nothing, because it was always dressed. This is the first spring anyone has been able to ask."));
						await dialog.Msg(L("I would rather not learn the answer at 66 with a broom in my hand."));
						break;

					case "leave":
						await dialog.Msg(L("It was tried in my grandfather's time. Whoever set this ground set it with Ouaas sap and the ground knows the difference, which is a stupid sentence that happens to be true."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNuka", out var killObj)) return;
				if (!quest.TryGetProgress("collectSap", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He rubs a measure between finger and thumb and holds it to the light for a long time*{/}"));
					await dialog.Msg(L("Good sap. Late, but good. The terraces get dressed tomorrow and I will have missed nothing in 41 years."));
					await dialog.Msg(L("Take the memorial's keeping money. There is nobody left up here to keep and it has been sitting in a box since the road shut."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Enough of them down. Now get the sap - it's on the ones you've killed, not on the trees any more."));
				}
				else
				{
					await dialog.Msg(L("Still stripping. Work the middle ground where the stands are thickest, that's where they've done the worst of it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Terraces dressed, every stone of them. The ground took it the way it always does and I slept through the night for the first time since the sap ran short."));
			}
		});

		// Quest 1002: Forty Years of Flasks
		//---------------------------------------------------------------------
		AddNpc(155126, L("[Monk] Stella"), "f_pilgrimroad_41_5", -1518, 682, 279, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1002);

			dialog.SetTitle(L("Stella"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's crouched at an empty flask rack, running a finger along a row of bare pegs like she's counting a wound*{/}"));
				await dialog.Msg(L("Oh - you'll do, actually, better than a letter would have. I walked up from the lake. I said I would write and then I decided a letter was not going to be enough, and now I am standing on the ground I have been sending people to for 8 years."));
				await dialog.Msg(L("The flask racks here are stripped. 40 years of abbey flasks and the Brown Lapasape Shamans are carrying them off the terraces empty. Bring me 10 back so I can count what's actually missing."));

				var response = await dialog.Select(L("Will you get the flasks?"),
					Option(L("I'll bring you 10 flasks"), "help"),
					Option(L("Why count empty flasks?"), "info"),
					Option(L("Ask the keeper first"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They hold them by the neck and won't set them down even to fight, so you will not have to look far once one has seen you."));
						break;

					case "info":
						await dialog.Msg(L("Because each flask is stamped with the year it was filled. If they were taken at random the years will be scattered, and if they were not, they will not be."));
						await dialog.Msg(L("I have counted the road for 8 years off things nobody thought were worth counting. It is the only skill I have that has ever been useful."));
						break;

					case "leave":
						await dialog.Msg(L("Matas has swept this ground for 41 years and has not looked at a flask rack in 20. He is the last person who would notice."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectFlasks", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She sets the flasks out by year stamp, then moves 3 of them and sits back*{/}"));
					await dialog.Msg(L("Not scattered. 10 flasks and 9 of them are stamped inside the same 4 years, and those 4 years are the ones the third device was last opened for."));
					await dialog.Msg(L("Take the relay purse - it came up the road with me and there is no boatman to spend it on here. And do not tell Matas yet. I want to be sure."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough to say anything with. 10 is the smallest number I would be willing to draw a conclusion from."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I told Matas. He went and looked at the rack himself, which he has not done in 20 years, and he came back and did not say anything at all."));
			}
		});

		// Quest 1003: On the Memorial Itself
		//---------------------------------------------------------------------
		AddNpc(156006, L("[Abbey Officer] Medeya"), "f_pilgrimroad_41_5", 1170, -861, 291, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1003);

			dialog.SetTitle(L("Medeya"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's rereading a folded list of orders, lips moving slightly, standing stiff as if reporting to someone who isn't there*{/}"));
				await dialog.Msg(L("You're not on my roster, which at this point almost recommends you. The abbey sent me down with 6 people and a list of instructions written for a road that still worked. I have 4 people left and I have used none of the instructions."));
				await dialog.Msg(L("There are Deadborn Scap Archers standing on the memorial terraces. Not near them - on them. Kill 30 and I will have done one thing here I can put in a report."));

				var response = await dialog.Select(L("Will you clear the terraces?"),
					Option(L("I'll kill 30 of the archers"), "help"),
					Option(L("What did the instructions say?"), "info"),
					Option(L("You have 4 people, use them"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They shoot from the upper terrace down the stair. Do not take the stair. Go round and come up onto their level and the whole advantage stops existing."));
						break;

					case "info":
						await dialog.Msg(L("Reassure the keeper. Inspect the ring. Report by the lamp relay. The lamps were packed with clay, the ring is not something I am cleared to inspect, and the keeper is 66 and reassuring me."));
						await dialog.Msg(L("I have been an officer 9 years and this is the first posting where the paper and the ground had nothing to do with each other."));
						break;

					case "leave":
						await dialog.Msg(L("2 of the 4 are carrying the other 2. I am not spending them on a terrace to make a report read better."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The terraces are clear and Matas has been up there with the broom since noon. He has not stopped and I have not suggested he should."));
					await dialog.Msg(L("Take the detachment's pay. There are 4 of us drawing it and 2 of those 4 cannot walk, so it has been going into a bag."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still up there. Come at the upper terrace on the level - the stair is theirs and it will stay theirs."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My report says the terraces were cleared by a passing traveller. It is the truest line in it and it will be the one they query."));
			}
		});

		// Quest 1004: Walk the Ring
		//---------------------------------------------------------------------
		AddNpc(155046, L("[Monk] Matas"), "f_pilgrimroad_41_5", 1060, 250, 289, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1004);

			dialog.SetTitle(L("Matas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's back at the terraces, but his broom is idle against his shoulder this time instead of moving*{/}"));
				await dialog.Msg(L("Good, you're still about. I've had a question sitting in me since Stella left and no one to put it to but you. There are 3 devices sunk into this ground and they are older than the memorial standing on top of them. The abbey calls them suppression devices and will not tell a keeper what they suppress."));
				await dialog.Msg(L("The lake monk walked up here to ask me about them and I could not answer, which I did not enjoy. Walk all 3 and look at them properly, since I am plainly not going to."));

				var response = await dialog.Select(L("Will you walk the ring?"),
					Option(L("I'll look at all 3 devices"), "help"),
					Option(L("You've never looked at them?"), "info"),
					Option(L("The abbey should send someone"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They sit at the 3 corners of the ground, well out past the terraces. Look at the housing, not the stone - the stone is only what was put over the top."));
						break;

					case "info":
						await dialog.Msg(L("I have swept round them 41 years and never once knelt down. It was not forbidden. It simply was not a thing a keeper did, and I have started to wonder who arranged for that."));
						await dialog.Msg(L("The monk buried under the unnamed stone set all 3. He has no name on him because he asked for none, and I have always assumed that was humility."));
						break;

					case "leave":
						await dialog.Msg(L("The abbey sent an officer with 6 people and a list. She is not cleared to inspect them. I am not cleared to inspect them. You are not on anyone's list at all."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("walkRing", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He takes the fragment, turns it over, and sets his broom down for the first time all day*{/}"));
					await dialog.Msg(L("Two failing and one broken open, and this came out of the third. That housing was cut, not worn. Somebody knelt where I never did and took it apart."));
					await dialog.Msg(L("Take the keeper's own purse, not the memorial's. I have 41 years of stipend in it and nothing whatever I want to do with it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 3. They are at the corners of the ground - west below the old path, east past the terraces, and one south beyond the grave."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have knelt at all 3 since. 41 years and it took a stranger walking past to make me do it, and I intend to be honest about that in the letter."));
			}
		});

		// Quest 1004 collection points - the suppression devices
		//---------------------------------------------------------------------
		void AddSuppressionDevice(int deviceNumber, string observation, bool yieldsFragment, int x, int z, int direction)
		{
			AddNpc(47107, L("Suppression Device"), "f_pilgrimroad_41_5", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_5", 1004);
				var variableKey = $"Laima.Quests.f_pilgrimroad_41_5.Quest1004.Device{deviceNumber}";
				var counterKey = "Laima.Quests.f_pilgrimroad_41_5.Quest1004.DevicesWalked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A squat housing sunk into the ground with a memorial stone set over the top of it*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already looked this one over*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Looking over the housing..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					if (yieldsFragment)
						character.Inventory.Add(666113, 1, InventoryAddType.PickUp);

					var walked = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, walked);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Devices walked: {0}/3", walked));

					if (walked >= 3)
						character.ServerMessage(L("{#FFD700}All 3 devices walked. Return to Matas.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the housing alone."));
				}
			});
		}

		AddSuppressionDevice(1,
			L("West Device: the housing is sound and the seam is still true, but it is running warm enough to feel through a glove."), false, -1204, -1109, 0);
		AddSuppressionDevice(2,
			L("East Device: warm as well, and one of the 4 anchor pins has lifted a finger's width out of the stone."), false, 1415, 37, 0);
		AddSuppressionDevice(3,
			L("South Device: cold. The housing has been cut open along the seam with a tool and a fragment of it is lying in the grass."), true, 1395, -1146, 0);

		// The unnamed monk's grave
		//---------------------------------------------------------------------
		AddNpc(47252, L("Grave of the Unnamed Monk"), "f_pilgrimroad_41_5", 71, -794, 264, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A memorial stone with no name cut into it, only a date and the outline of 3 circles*{/}"));
			await dialog.Msg(L("{#666666}*The 3 circles are set at the corners of a triangle, and the southern one has been chiselled out*{/}"));
		});

		// Quest 1005: What the Third Device Held
		//---------------------------------------------------------------------
		AddNpc(155126, L("[Monk] Stella"), "f_pilgrimroad_41_5", -127, -924, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1005);

			dialog.SetTitle(L("Stella"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_pilgrimroad_41_5", 1004)))
				{
					await dialog.Msg(L("Walk the ring for Matas first. I will not stand here guessing at 3 devices when one of us could simply go and look at them."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's spread three sets of notes across a memorial stone, weighing the corners down with pebbles against the wind*{/}"));
				await dialog.Msg(L("You walked the ring - good, I've been waiting on that more than I let on. Flask years, a cut housing, and a chiselled circle on a grave nobody put a name on. Salvia lost an altar, my lake got a warband, Sekta got a forest packed into it - and all 3 of those were pushed away from this ground, not toward it."));
				await dialog.Msg(L("Whatever the third device held has been standing in the open south of the grave since the housing was cut. Kill 20 Brown Nuka to clear the approach, then break what is holding the ground open. Carry the abbey's scroll while you do."));

				var response = await dialog.Select(L("Will you go south of the grave?"),
					Option(L("I'll close the third device"), "help"),
					Option(L("Who cut the housing?"), "info"),
					Option(L("This is the abbey's to do"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(666112, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("The scroll only holds while you are standing on the cut seam. Fight it back onto the housing rather than chasing it off the ground."));
						break;

					case "info":
						await dialog.Msg(L("Somebody with a tool, 4 years of abbey flasks to work through, and enough time on this ground that a keeper of 41 years never thought to ask what they were doing."));
						await dialog.Msg(L("I have a name I am not going to say until I have written to the abbey and had it say the name back to me first."));
						break;

					case "leave":
						await dialog.Msg(L("The abbey has an officer here with 4 people, 2 of whom cannot walk, and a list that does not mention any of this. It is nobody's to do. That is how it got to 3 maps."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearApproach", out var approachObj)) return;
				if (!quest.TryGetProgress("closeDevice", out var deviceObj)) return;

				if (approachObj.Done && deviceObj.Done)
				{
					await dialog.Msg(L("{#666666}*The housing takes the scroll and pulls the cut seam shut with a sound like the lake in winter*{/}"));
					await dialog.Msg(L("Closed. The west and east devices went cool inside a minute, which means all 3 were carrying what one of them was supposed to."));
					await dialog.Msg(L("Take the mace off the seam - it is abbey armoury, 40 years old, and it was left where somebody knelt to cut the housing. I am carrying that to Mavern myself, and I am not sending it by lamp."));

					character.Quests.Complete(questId);
				}
				else if (approachObj.Done)
				{
					await dialog.Msg(L("The approach is clear. Get it back onto the seam - off the housing the scroll is a piece of paper."));
				}
				else
				{
					await dialog.Msg(L("Too many Nuka between the grave and the housing. Clear them or you will be holding a scroll with both hands and nothing else."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Matas dressed the south terrace this morning and the ground took it. He is 66, he has swept here 41 years, and he asked me to write his name into the letter as a witness."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Trees the Ground Is Blessed With
//-----------------------------------------------------------------------------

public class TheTreesTheGroundIsBlessedWithQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1001);
		SetName(L("The Trees the Ground Is Blessed With"));
		SetType(QuestType.Sub);
		SetDescription(L("Ouaas Memorial is consecrated with sap from the Ouaas trees and nothing else, redressed every spring for 400 years. The Brown Nuka have stripped the stands down to the white."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Monk] Matas"), "f_pilgrimroad_41_5");

		AddObjective("killNuka", L("Kill Brown Nuka in the Ouaas stands"),
			new KillObjective(25, new[] { MonsterId.Nuka_Brown }));

		AddObjective("collectSap", L("Recover Ouaas Tree Sap"),
			new CollectItemObjective(666111, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(666111, 0.40f, MonsterId.Nuka_Brown);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666111, character.Inventory.CountItem(666111), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666111, character.Inventory.CountItem(666111), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Forty Years of Flasks
//-----------------------------------------------------------------------------

public class FortyYearsOfFlasksQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1002);
		SetName(L("Forty Years of Flasks"));
		SetType(QuestType.Sub);
		SetDescription(L("The memorial's flask racks have been stripped and the Brown Lapasape Shamans are carrying 40 years of abbey flasks off the terraces. Each flask is stamped with the year it was filled, and the lake monk wants to know whether the years are scattered."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Monk] Stella"), "f_pilgrimroad_41_5");

		AddObjective("collectFlasks", L("Recover Empty Holy Water Flasks"),
			new CollectItemObjective(666110, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(666110, 0.45f, MonsterId.Lapasape_Bow_Brown);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666110, character.Inventory.CountItem(666110), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666110, character.Inventory.CountItem(666110), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: On the Memorial Itself
//-----------------------------------------------------------------------------

public class OnTheMemorialItselfQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1003);
		SetName(L("On the Memorial Itself"));
		SetType(QuestType.Sub);
		SetDescription(L("Deadborn Scap Archers are standing on the memorial terraces themselves, shooting down the stair. The abbey officer sent to inspect the ground has 4 people left and no instruction that covers it."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Abbey Officer] Medeya"), "f_pilgrimroad_41_5");

		AddObjective("killArchers", L("Kill Deadborn Scap Archers on the memorial terraces"),
			new KillObjective(30, new[] { MonsterId.Deadbornscab_Bow }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Walk the Ring
//-----------------------------------------------------------------------------

public class WalkTheRingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1004);
		SetName(L("Walk the Ring"));
		SetType(QuestType.Sub);
		SetDescription(L("3 suppression devices are sunk at the corners of the memorial ground, older than the memorial standing on them, and the keeper of 41 years has never once knelt at one. Walk all 3 and look at the housings."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Monk] Matas"), "f_pilgrimroad_41_5");

		AddObjective("walkRing", L("Look over all 3 suppression devices"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_5.Quest1004.DevicesWalked", 3, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666113, character.Inventory.CountItem(666113), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1004.DevicesWalked");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_5.Quest1004.Device{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666113, character.Inventory.CountItem(666113), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1004.DevicesWalked");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_5.Quest1004.Device{i}");
	}
}

// Quest 1005 CLASS: What the Third Device Held
//-----------------------------------------------------------------------------

public class WhatTheThirdDeviceHeldQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1005);
		SetName(L("What the Third Device Held"));
		SetType(QuestType.Sub);
		SetDescription(L("The southern suppression device was cut open with a tool, and everything that has gone wrong down the pilgrim road was pushed away from this ground rather than toward it. Clear the approach south of the grave and close the housing with the abbey's scroll."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Monk] Stella"), "f_pilgrimroad_41_5");

		AddPrerequisite(new CompletedPrerequisite("f_pilgrimroad_41_5", 1004));

		AddObjective("clearApproach", L("Kill Brown Nuka between the grave and the housing"),
			new KillObjective(20, new[] { MonsterId.Nuka_Brown }));

		AddObjective("closeDevice", L("Close the cut housing"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Nuka_Brown, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Elma_Red, 3),
				},
				resetIdent: "clearApproach",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(203204, 1)); // Vienarazis Mace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666112, character.Inventory.CountItem(666112), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666112, character.Inventory.CountItem(666112), InventoryItemRemoveMsg.Destroyed);
	}
}
