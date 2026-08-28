//--- Melia Script ----------------------------------------------------------
// Grynas Forest Road Quest NPCs
//--- Description -----------------------------------------------------------
// The approach to the Dievdirbys training ground, and the ring of carved road
// statues the order has kept standing here for two hundred years.
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

public class FKatyn451QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: What the Stoulets Prised Out
		//---------------------------------------------------------------------
		AddNpc(157004, L("[Dievdirbys] Ajel"), "f_katyn_45_1", -2072, -267, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1001);

			dialog.SetTitle(L("Ajel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's kneeling at the base of a road statue, thumb pressed against a hairline crack in the wood*{/}"));
				await dialog.Msg(L("Didn't hear you on the gravel. Fine — spares us both the noise of an introduction. My order carves these statues. Not decoration. Each one's got a crystal in its chest, and that crystal is the only thing keeping this stretch of forest polite."));
				await dialog.Msg(L("The Gray Stoulets figured out the chest opens. Wasn't supposed to. Kill 20 of them, bring me back 6 crystals, before the whole ring goes dark on my watch."));

				var response = await dialog.Select(L("Well? Are you getting my crystals back, or admiring the scenery?"),
					Option(L("I'll hunt the Stoulets and recover the crystals"), "help"),
					Option(L("Why do they want the crystals?"), "info"),
					Option(L("Carve new ones"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They carry what they take, always have. Walking slower than the others — that's your thief. Crystal weighs more than they expect."));
						break;

					case "info":
						await dialog.Msg(L("They don't want them, mind. They want them out. Something's telling them to open the statues — and in the recorded history of this forest, a Gray Stoulet has never once been told anything. Worth sitting with that a moment."));
						await dialog.Msg(L("That's the part I'd like an answer to. I expect it's up in the hills, past the training ground. Isn't my hill to climb, though."));
						break;

					case "leave":
						await dialog.Msg(L("A crystal takes 11 years to set right. I've carved 3 in my whole life and I'm 64 — so no, I will not simply 'carve new ones,' and I'd thank you not to suggest it twice."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStoulets", out var killObj)) return;
				if (!quest.TryGetProgress("collectCrystals", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He turns each crystal to the light before setting it in his satchel*{/}"));
					await dialog.Msg(L("6, and 4 of them still hold. I can reseat those tonight and the western half of the ring lights again."));
					await dialog.Msg(L("Take the order's road purse. We carry it for exactly this, and gods know we don't spend it on anything else out here."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The road's quieter, but I'm still short of crystals. Check what the dead ones were carrying."));
				}
				else
				{
					await dialog.Msg(L("Still Stoulets working the ring. They go statue to statue, so follow the ring and you'll find them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("4 statues lit again by morning. The birds came back to that stretch of road within the hour, which tells you what the crystals are actually for. Didn't expect to feel anything about a bird."));
			}
		});

		// Quest 1002: Yellow Sapous
		//---------------------------------------------------------------------
		AddNpc(157005, L("[Trainee Carver] Rusne"), "f_katyn_45_1", -1977, -476, 286, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1002);

			dialog.SetTitle(L("Rusne"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's perched on the rock with her knees drawn up, a coil of empty twine in her lap*{/}"));
				await dialog.Msg(L("Oh — oh, thank the carvers, someone, finally! Please, please don't tell Ajel how long I've been sitting on this rock. He sent me out for Yellow Sapous — I got exactly this far, saw a Stoulet, and my knees just... stopped. Four months! I've only been carving four months, nobody trains you for the scrub part!"));
				await dialog.Msg(L("They grow right out there in the open ground — I can see them from here, that's honestly the worst part. Bring me 3 and I'll boil the fixing oil tonight, and nobody, nobody has to know how long I stood here."));

				var response = await dialog.Select(L("Would you — I mean, only if you don't mind — would you gather them for me? Please?"),
					Option(L("I'll bring you 3 Yellow Sapous"), "help"),
					Option(L("What's the oil for?"), "info"),
					Option(L("Go back and tell him"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They're bright yellow, you really can't miss them — cut low, right at the root, or the sap runs out before you're even back. I learned that the hard way. Obviously."));
						break;

					case "info":
						await dialog.Msg(L("It's what sets a crystal into carved wood, if you must know. Without it, the crystal sits loose, and any animal with a claw can just pop it right out — which is, apparently, exactly what's been happening."));
						await dialog.Msg(L("So the one plant I'm too frightened to go and pick is the reason the entire road is failing. Yes. I'm aware of how that sounds, thank you."));
						break;

					case "leave":
						await dialog.Msg(L("I will, eventually — once I've worked out how to say it without the word 'frightened' anywhere in the sentence."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectSapous", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She smells the cut ends and looks visibly relieved*{/}"));
					await dialog.Msg(L("Fresh! Oh, good, good — that's a full pot of oil, that's 30 statues' worth, that's the whole western ring twice over!"));
					await dialog.Msg(L("Here — take my stipend, please, I've nothing else to spend it on out here except exactly this."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short! They're all out in the western open ground, past where the scrub thins — I promise I checked, from a very safe distance."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I told Ajel, finally. He just said 'good' and went right back to carving. I think that was the whole lesson, honestly, and I still don't feel like I passed it."));
			}
		});

		// Quest 1002 collection points - the Sapous stands
		//---------------------------------------------------------------------
		void AddSapousStand(int standNumber, int x, int z, int direction)
		{
			AddNpc(157006, L("Yellow Sapous"), "f_katyn_45_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_1", 1002);
				var variableKey = $"Laima.Quests.f_katyn_45_1.Quest1002.Sapous{standNumber}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A stand of bright yellow herbs in the open grass*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already cut this stand*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 400, 300);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The cut sap carries - {0} drawn in!{{/}}", luredCount));

				var result = await character.TimeActions.StartAsync(
					L("Cutting the Sapous..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(668025, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					character.ServerMessage(L("Cut a Yellow Sapous, root and all."));
				}
				else
				{
					character.ServerMessage(L("You leave the stand uncut."));
				}
			});
		}

		AddSapousStand(1, -1875, 607, 0);
		AddSapousStand(2, -1686, 341, 0);
		AddSapousStand(3, -1604, 663, 0);
		AddSapousStand(4, -1422, 595, 0);

		// Quest 1003: Two Whistles
		//---------------------------------------------------------------------
		AddNpc(147410, L("[Road-Warden] Algis"), "f_katyn_45_1", 900, 690, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1003);

			dialog.SetTitle(L("Algis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He doesn't turn from the bend until he's placed your footsteps as two legs, not four*{/}"));
				await dialog.Msg(L("Traveler. Not Stoulet. Good — small victories. Brown Stoulet Archers took that blind bend behind me 5 weeks back. Killed 2 couriers and a carving trainee since. Two whistles, then arrows. That's the entire warning system out here."));
				await dialog.Msg(L("I can hold the road, or I can clear the bend. Not both — there's one of me. Kill 25 of the archers and the road stays mine."));

				var response = await dialog.Select(L("So. Taking the bend, or adding yourself to the count?"),
					Option(L("I'll kill the archers"), "help"),
					Option(L("Two whistles?"), "info"),
					Option(L("Ask for more wardens"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Go in off the road and come at the bend from above. If you walk it straight they'll have 3 volleys into you before you see the first one."));
						break;

					case "info":
						await dialog.Msg(L("One whistle marks you. Second one means everybody's got you now. Between that second whistle and the arrows, you've got about as long as it takes to say 'two whistles.' Try it. Not long, is it."));
						await dialog.Msg(L("The trainee they got was 16. Heard both, froze solid. That's what everyone does, the first time. Wasn't her fault."));
						break;

					case "leave":
						await dialog.Msg(L("I've asked, believe me. The reply came back saying the post was reviewed and found adequately manned. I am the post."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Walked the bend at noon, hands empty, nothing whistled. First time since spring. Felt strange, not being shot at."));
					await dialog.Msg(L("Take the warden's fee. Paid to me for holding a road I couldn't actually hold — so by rights, it's yours."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still archers on the bend. Listen for the first whistle and move on it - don't wait to be sure."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two carts and a carving cart came through this week without an escort. I stood here and watched them go past and did nothing, and it was the best day I've had."));
			}
		});

		// Quest 1004: The Sticky Road
		//---------------------------------------------------------------------
		AddNpc(152001, L("[Sap-Boiler] Milda"), "f_katyn_45_1", 179, -528, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1004);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's stirring a black pot with a stick as long as she is tall, not looking up*{/}"));
				await dialog.Msg(L("Stand upwind, unless you fancy losing your nose. Forty years I've boiled glue for this order — forty years off the same pine stand, right up until those Green Socket Mages waltzed in and killed every last tree in it. Forty years, gone, just like that."));
				await dialog.Msg(L("Turns out the muck they throw boils down better than pine ever did. Go figure. Bring me 8 of it, and I'll have the whole order laughing at me right up until they're using it anyway."));

				var response = await dialog.Select(L("Well? Are you fetching it, or just admiring my pot?"),
					Option(L("I'll bring you 8 of the sticky liquid"), "help"),
					Option(L("You're using monster sap?"), "info"),
					Option(L("That sounds dangerous"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Scrape it off the ground where they've thrown it, not off the Socket. It goes hard on the creature and stays soft on stone."));
						break;

					case "info":
						await dialog.Msg(L("I use what's available, that's all. Sixty dead trees out there and a glue-boiler with no glue — only one of those two problems has a fix."));
						await dialog.Msg(L("Ajel will hate it, mark me. Ajel's hated every improvement I've made in forty years, and used every single one anyway."));
						break;

					case "leave":
						await dialog.Msg(L("Boiling anything's dangerous, dear. Look at these arms — every scar's a lesson I only needed once."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectLiquid", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She works a blob of it between two fingers and pulls them apart slowly*{/}"));
					await dialog.Msg(L("Better draw than pine, doesn't yellow either. I'll have a full barrel by week's end, and no — I will not be telling anyone where it came from."));
					await dialog.Msg(L("Take your cut. And take a pot of the old pine glue, it's the last of it and it's good for nothing but nostalgia."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough yet. The Sockets throw it constantly, so the pine stand floor should be covered in the stuff."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ajel used a full pot this morning, said it was the cleanest joint he'd seen in a decade. I let the old goat finish his sentence before I told him what it was made of."));
			}
		});

		// Quest 1005: The Silva Griffin
		//---------------------------------------------------------------------
		AddNpc(157004, L("[Dievdirbys] Ajel"), "f_katyn_45_1", 1160, 820, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1005);

			dialog.SetTitle(L("Ajel"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_katyn_45_1", 1001)))
				{
					await dialog.Msg(L("Bring me the crystals off the western ring first. Until that half of the road is lit I've nothing to stand behind out here."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's standing further up the road than before, staring at a gap in the tree line where four statues used to stand*{/}"));
				await dialog.Msg(L("You again. Good timing, if there is such a thing today — I've just seen something I wish to every carved saint I hadn't. The western ring is lit, the eastern ring is not, and now I can see exactly what's been sitting in the gap. A Silva Griffin, nesting where the last 4 statues used to stand."));
				await dialog.Msg(L("It's what has been driving the Stoulets onto the statues - not orders, just a bigger thing pushing them along the road. Kill 20 of the Brown Stoulet Archers to clear its nest approach, then take the Griffin itself."));

				var response = await dialog.Select(L("Will you go up to the nest?"),
					Option(L("I'll kill the Silva Griffin"), "help"),
					Option(L("A griffin drove all this?"), "info"),
					Option(L("That's more than I can take"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It stoops from height and only stoops once per pass. Break its line under the canopy edge and it has to land — and landed, it's just a large, furious animal. I can work with large and furious. I've carved statues of large and furious for a living."));
						break;

					case "info":
						await dialog.Msg(L("A griffin drove the Stoulets. Something else drove the griffin down out of the hills, and that is a question for the training ground, not for this road."));
						await dialog.Msg(L("I've been carving 46 years. Every time the ring fails, it fails from the hills down. Never once from the road up."));
						break;

					case "leave":
						await dialog.Msg(L("Suit yourself. Then it stays in the gap, the eastern ring stays dark, and inside a season this road shuts the same way the Letas road did. Your name's not on that one, at least."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearApproach", out var approachObj)) return;
				if (!quest.TryGetProgress("killGriffin", out var bossObj)) return;

				if (approachObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("Dead in the gap, and 4 statue bases underneath it that nobody's reached in 2 years. I'll be busy a while."));
					await dialog.Msg(L("Take the shield off the nest. There was a warden under all that, once, and this is the only piece of him worth carrying out."));

					character.Quests.Complete(questId);
				}
				else if (approachObj.Done)
				{
					await dialog.Msg(L("The approach is clear. It's up there alone now, which is the only condition anyone should fight a griffin in."));
				}
				else
				{
					await dialog.Msg(L("Too many archers still under the nest. Take them first - a griffin fight is not one you want arrows in."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Both halves of the ring lit, first time since I took the road. I'm going up to the training ground to ask Esol what came down out of the hills. I don't expect to like the answer. I never do."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: What the Stoulets Prised Out
//-----------------------------------------------------------------------------

public class WhatTheStouletsPrisedOutQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1001);
		SetName(L("What the Stoulets Prised Out"));
		SetType(QuestType.Sub);
		SetDescription(L("The Dievdirbys keep a ring of carved statues along the Grynas road, each holding a protective crystal. Gray Stoulets have learned to open the chest cavities. Kill them and recover the crystals."));
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dievdirbys] Ajel"), "f_katyn_45_1");

		AddObjective("killStoulets", L("Kill Gray Stoulet along the statue ring"),
			new KillObjective(20, new[] { MonsterId.Stoulet_Gray }));

		AddObjective("collectCrystals", L("Recover Protective Crystals"),
			new CollectItemObjective(668028, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668028, 0.40f, MonsterId.Stoulet_Gray);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668028, character.Inventory.CountItem(668028), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668028, character.Inventory.CountItem(668028), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Yellow Sapous
//-----------------------------------------------------------------------------

public class YellowSapousQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1002);
		SetName(L("Yellow Sapous"));
		SetType(QuestType.Sub);
		SetDescription(L("A trainee carver was sent for Yellow Sapous and got as far as a rock. Without the fixing oil the plant makes, the road statues cannot hold their crystals at all. Cut the herb in the western open ground for her."));
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trainee Carver] Rusne"), "f_katyn_45_1");

		AddObjective("collectSapous", L("Cut Yellow Sapous in the western open ground"),
			new CollectItemObjective(668025, 3));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668025, character.Inventory.CountItem(668025), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1002.Sapous{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668025, character.Inventory.CountItem(668025), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1002.Sapous{i}");
	}
}

// Quest 1003 CLASS: Two Whistles
//-----------------------------------------------------------------------------

public class TwoWhistlesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1003);
		SetName(L("Two Whistles"));
		SetType(QuestType.Sub);
		SetDescription(L("Brown Stoulet Archers hold the blind bend on the Grynas road and have killed 2 couriers and a carving trainee. The road-warden can hold the road or clear the bend, not both."));
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Warden] Algis"), "f_katyn_45_1");

		AddObjective("killArchers", L("Kill Brown Stoulet Archers at the blind bend"),
			new KillObjective(25, new[] { MonsterId.Stoulet_Bow_Blue }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1004 CLASS: The Sticky Road
//-----------------------------------------------------------------------------

public class TheStickyRoadQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1004);
		SetName(L("The Sticky Road"));
		SetType(QuestType.Sub);
		SetDescription(L("Green Socket Mages killed the pine stand the order's glue-boiler has drawn from for 40 years. She intends to boil the Sockets' own sticky liquid instead. Scrape 8 of it off the pine-stand floor."));
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sap-Boiler] Milda"), "f_katyn_45_1");

		AddObjective("collectLiquid", L("Collect Sticky Liquid from Green Socket Mages"),
			new CollectItemObjective(668029, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668029, 0.50f, MonsterId.Socket_Mage_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668029, character.Inventory.CountItem(668029), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668029, character.Inventory.CountItem(668029), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Silva Griffin
//-----------------------------------------------------------------------------

public class TheSilvaGriffinQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1005);
		SetName(L("The Silva Griffin"));
		SetType(QuestType.Sub);
		SetDescription(L("A Silva Griffin has nested in the gap where the last 4 road statues used to stand, and it is what has been pushing the Stoulets along the ring. Clear its nest approach and kill it."));
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Dievdirbys] Ajel"), "f_katyn_45_1");

		AddPrerequisite(new CompletedPrerequisite("f_katyn_45_1", 1001));

		AddObjective("clearApproach", L("Kill Brown Stoulet Archers under the nest"),
			new KillObjective(20, new[] { MonsterId.Stoulet_Bow_Blue }));

		AddObjective("killGriffin", L("Defeat the Silva Griffin"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Silva_Griffin, 1) },
				resetIdent: "clearApproach",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(16000, 11000));
		AddReward(new SilverReward(20000));
		AddReward(new ItemReward(223106, 1)); // Ledas Shield
		AddReward(new ItemReward(640084, 3)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}
}
