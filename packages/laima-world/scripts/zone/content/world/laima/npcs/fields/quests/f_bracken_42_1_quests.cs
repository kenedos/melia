//--- Melia Script ----------------------------------------------------------
// Khonot Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Khonot Forest - the trade-trail forest east of Orsha where
// the Blue Gosaru troupe has overrun the canopy and disrupted caravan routes.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors;

public class FBracken421QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Gosaru Ape Troupe
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Forest-Ward] Bronius"), "f_bracken_42_1", 3, 46, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1001);

			dialog.SetTitle(L("Bronius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Caravans used to come through Khonot Forest without much trouble. Not anymore."));
				await dialog.Msg(L("There's a troupe of Blue Gosaru living in the trees along the western trail. They drop down on travelers without warning. The merchant guild stopped sending escorts because too many of them weren't coming back."));
				await dialog.Msg(L("If we don't cut their numbers down soon, the inland villages are going to be cut off from Orsha completely."));

				var response = await dialog.Select(L("Will you help thin the troupe?"),
					Option(L("I'll kill them"), "help"),
					Option(L("Tell me about these Gosaru"), "info"),
					Option(L("Maybe later"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thirty-five should be enough to break the pack for a season. Stay away from the thickest trees — that's where they nest and where the ambushes come from."));
						await dialog.Msg(L("And keep looking up while you walk. They're patient."));
						break;

					case "info":
						await dialog.Msg(L("They're smart. Way smarter than apes should be. They post lookouts and signal each other across the canopy, and they'll lure people into the deeper brush before jumping them."));
						await dialog.Msg(L("I've watched them. It's organized. That's what makes them dangerous."));
						break;

					case "leave":
						await dialog.Msg(L("Don't think about it too long. Every week the trail's closed, another inland village runs lower on supplies."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGosarus", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The canopy's quiet for the first time in months. The trail's open again."));
					await dialog.Msg(L("Take your pay. You did in a few days what we couldn't manage in a season. Thank you."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Branches are still moving with them up there. Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravans are coming through Khonot again. First time in months. The merchant guild sends thanks every time one makes it through."));
			}
		});

		// Quest 2: Gosaru Stone Shards
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Lapidary] Alfreds"), "f_bracken_42_1", -1000, -400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1002);

			dialog.SetTitle(L("Alfreds"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Gosarus carry a stone in their gut. Some kind of crystal — the apes use it to communicate with each other, near as anyone can tell."));
				await dialog.Msg(L("If you crack one open, you get shards that hum when you touch them. The Orsha mage-houses pay good silver for clean ones. Trouble is, regular trappers won't go in deep enough to find the troupe."));

				var response = await dialog.Select(L("Interested in the work?"),
					Option(L("I'll harvest the shards"), "help"),
					Option(L("What makes a shard worth buying?"), "info"),
					Option(L("Not for me"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut the stone out clean and split it across the grain. If you find the right line, the shards come apart on their own."));
						await dialog.Msg(L("I need seven clean ones. You'll have to crack a lot more than seven to get that — the mage-houses are picky."));
						break;

					case "info":
						await dialog.Msg(L("Has to be see-through, no cloudy bits, no cracks. A clean one hums when you hold it. A bad one just sits there."));
						await dialog.Msg(L("You'll feel the difference. The good ones almost ring."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. Another hunter'll come by before the week's out."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGosarus", out var killObj)) return;
				if (!quest.TryGetProgress("gatherShards", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Seven clean ones, all humming. That'll keep the mage-houses stocked through next month, easy."));
					await dialog.Msg(L("Here's your pay. Little extra for the quality. We could do business again."));
					character.Inventory.Remove(666120, character.Inventory.CountItem(666120), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep cracking stones. Seven clean ones is what I need. The duds you can sell to apprentices."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sold out of Gosaru shards by the end of the week. The Orsha mage-houses are already asking about the next batch."));
			}
		});

		// Quest 3: Doyor Fangs
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Charm-Carver] Alda"), "f_bracken_42_1", -471, -476, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1003);

			dialog.SetTitle(L("Alda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Caravan drivers won't take the Khonot trail without a tooth-charm hanging off the lead horse's bridle. Old superstition. And I'm clean out of them right now."));
				await dialog.Msg(L("I carve them from Blue Doyor fangs. Drilled, threaded, marked up. If you can bring me six clean fangs, I can have charms ready for the next caravan."));

				var response = await dialog.Select(L("Will you help me?"),
					Option(L("I'll bring the fangs"), "help"),
					Option(L("Tooth-charms?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Six fangs — the long canines, mind, not the molars. And make sure they're not cracked. Cracked ones split under the drill and the carving's ruined."));
						break;

					case "info":
						await dialog.Msg(L("Drivers swear a Doyor fang on the harness keeps the canopy beasts off the lead horse. Maybe it's true, maybe it's just something to settle their nerves — I couldn't tell you."));
						await dialog.Msg(L("All I know is, every caravan that goes out with one of my charms comes back fine. The ones without lose people. Take that for what it's worth."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan leaves in three days. If I haven't got charms to sell, the drivers will turn around at the trailhead, and the inland villages go another month without grain."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDoyors", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFangs", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Six clean fangs, all sound. I'll have the charms threaded and marked by the time the caravan rolls out."));
					await dialog.Msg(L("Take this for your trouble. And if you're heading out with the next caravan yourself, come back for one of the charms."));
					character.Inventory.Remove(650692, character.Inventory.CountItem(650692), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still need those fangs. Keep hunting. And remember — long canines, no cracks."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan rolled out yesterday with a fang on every lead bridle. Drivers paid double for them, and one was already back asking for a second string."));
			}
		});

		// Quest 4: Folibu Wings
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Tinkerer] Kaspars"), "f_bracken_42_1", 776, 18, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1004);

			dialog.SetTitle(L("Kaspars"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I'm building a glider. A real one — not a toy, not a model. Big enough for a grown man to ride."));
				await dialog.Msg(L("Yellow Folibu wings are the only thing in three days' ride that's both light enough and strong enough. They're see-through, tight as a drum, and they hold up under weight."));
				await dialog.Msg(L("I just need five intact wings to finish the prototype. Three years of calculations, and it all comes down to this."));

				var response = await dialog.Select(L("Want to be part of history?"),
					Option(L("I'll harvest the wings"), "help"),
					Option(L("Does this thing actually fly?"), "info"),
					Option(L("Sounds like nonsense"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Tear them at the joint, never through the membrane. One puncture and the wing's useless — the air leaks right through."));
						await dialog.Msg(L("You'll have to kill more than five Folibus to get five clean wings. That's just how it goes."));
						break;

					case "info":
						await dialog.Msg(L("It will fly. It has to. I've been over the numbers a thousand times — the lift, the weight, the way the wing trims out."));
						await dialog.Msg(L("Five wings and I'll know one way or the other. Either it works, or I've wasted my whole life. Either way, I need to find out."));
						break;

					case "leave":
						await dialog.Msg(L("Then flight stays on paper. For another year, anyway. Every furrier in Orsha thinks I'm a fool, but the numbers don't lie."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFolibus", out var killObj)) return;
				if (!quest.TryGetProgress("gatherWings", out var wObj)) return;

				if (killObj.Done && wObj.Done)
				{
					await dialog.Msg(L("Five wings, all whole! Look at the membrane — perfect, every one. I'll test it on the ridge first thing this weekend."));
					await dialog.Msg(L("Take this. If the glider works, your name goes in my notebook next to mine."));
					character.Inventory.Remove(666172, character.Inventory.CountItem(666172), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. Remember — tear at the joint, not through the membrane. Five intact wings."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The glider went six paces before it crashed into a tree. But it flew! It actually flew! Next prototype's already on the workbench."));
			}
		});

		// Quest 5: The Gosaru King
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Jekabs"), "f_bracken_42_1", -2, 833, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1005);

			dialog.SetTitle(L("Jekabs"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bronius'll tell you to kill thirty-five Gosarus. That's fine work, but the troupe won't stay broken for long."));
				await dialog.Msg(L("There's a king in that pack. Three times the size of the rest, and a lot smarter. He won't come out while his apes are at full strength."));
				await dialog.Msg(L("Kill ten of his troupe and he'll drop down from the canopy himself. That's our chance."));

				var response = await dialog.Select(L("Will you take the bounty?"),
					Option(L("I'll face the King"), "help"),
					Option(L("How big is this thing, really?"), "info"),
					Option(L("Find another hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Kill exactly ten Blue Gosarus. The King will come down as soon as the tenth one falls."));
						await dialog.Msg(L("Stay away from the big branches. He doesn't climb down — he drops, and he takes half a tree with him."));
						break;

					case "info":
						await dialog.Msg(L("Three times the size of a regular Blue Gosaru. Smart as a man, and mean."));
						await dialog.Msg(L("He tore the last hunter clean in half. All I found was a belt buckle and a coin purse. Go in respectful, or don't go in at all."));
						break;

					case "leave":
						await dialog.Msg(L("His troupe gets bigger every week. Eventually he comes down whether anyone hunts him or not — and on his terms, that's worse for everyone."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTroupe", out var gObj)) return;
				if (!quest.TryGetProgress("killKing", out var kObj)) return;

				if (kObj.Done)
				{
					await dialog.Msg(L("The King's dead? Show me the hide. Yeah — that's him. That's the bastard."));
					await dialog.Msg(L("Khonot's safe again. Caravans will be running without escort soon. Pay's yours, with a hunter's bonus on top."));
					character.Quests.Complete(questId);
				}
				else if (gObj.Done)
				{
					await dialog.Msg(L("The canopy's shaking — he's down! Find him and finish it before he climbs back up."));
				}
				else
				{
					await dialog.Msg(L("Ten of his troupe first. He won't show until they thin out — that's how it works with the canopy kings."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Khonot's safe. Caravans run without escorts now. That was real work — the kind people remember."));
			}
		});

		// Quest 6: Khonot Sweep
		//-------------------------------------------------------------------------
		AddNpc(20064, L("[Ranger] Peteris"), "f_bracken_42_1", -744, 945, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1006);

			dialog.SetTitle(L("Peteris"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Standard caravan-protection contract. Khonot has three problem species — Gosarus, Doyors, and Tanus. Unless somebody kills them on a regular schedule, they take over the trails within a month."));
				await dialog.Msg(L("Twelve of each. Doesn't matter what order, just give me the count when you're done. The merchant guild pays per sweep, and they pay fair."));

				var response = await dialog.Select(L("Take the contract?"),
					Option(L("I'll do the sweep"), "help"),
					Option(L("What's the pay?"), "info"),
					Option(L("Not interested"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thirty-six total. Keep moving and you'll find them in any direction — they're all over the forest."));
						break;

					case "info":
						await dialog.Msg(L("Standard ranger pay, plus a guild bonus for every successful sweep. Fair work for fair coin."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan comes through in three days either way. Hope the trails are clear, or that the guild finds someone less choosy."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGosarus", out var gObj)) return;
				if (!quest.TryGetProgress("killDoyors", out var dObj)) return;
				if (!quest.TryGetProgress("killTanus", out var tObj)) return;

				if (gObj.Done && dObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Sweep done, full count, all three species. Caravan will get through Khonot fine now."));
					await dialog.Msg(L("Take your pay. Thanks. I'll mark you down for the next contract."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at the sweep. Twelve of each — Gosarus, Doyors, Tanus."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan made it through without a scratch. You'll be the first I call when the next contract opens up."));
			}
		});

		// Gosaru Troupe Ambush Triggers (Quest 1001)
		//-------------------------------------------------------------------------
		AddGosaruAmbush(1, -83, -175);
		AddGosaruAmbush(2, 56, -383);
		AddGosaruAmbush(3, 444, -135);
		AddGosaruAmbush(4, 320, 84);
		AddGosaruAmbush(5, 289, 357);
		AddGosaruAmbush(6, 122, -642);
		AddGosaruAmbush(7, 87, -816);
	}

	private void AddGosaruAmbush(int index, int x, int z)
	{
		var questId = new QuestId("f_bracken_42_1", 1001);
		var ambushKey = $"Laima.Quests.f_bracken_42_1.Quest1001.Ambush{index}";

		AddAreaTrigger("f_bracken_42_1", x, z, 50, async (args) =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.IsDead)
				return;

			if (!character.Quests.IsActive(questId))
				return;

			if (character.Variables.Perm.GetBool(ambushKey, false))
				return;

			character.Variables.Perm.SetBool(ambushKey, true);

			if (SpawnTempMonsters(character, MonsterId.Gosaru_Blue, 3, 70, TimeSpan.FromMinutes(1)))
			{
				character.ServerMessage(L("{#FF6666}Blue Gosarus drop from the canopy!{/}"));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FBracken421Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1001);
		SetName(L("Gosaru Troupe"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Gosarus dominating the Khonot canopy."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forest-Ward] Bronius"), "f_bracken_42_1");

		AddObjective("killGosarus", L("Kill Blue Gosarus"),
			new KillObjective(35, new[] { MonsterId.Gosaru_Blue }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FBracken421Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1002);
		SetName(L("Gosaru Stone Shards"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Gosarus and gather magic stone shards for the Orsha mage-houses."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Lapidary] Alfreds"), "f_bracken_42_1");

		AddDrop(666120, 0.40f, MonsterId.Gosaru_Blue);

		AddObjective("killGosarus", L("Kill Blue Gosarus"),
			new KillObjective(30, new[] { MonsterId.Gosaru_Blue }));

		AddObjective("gatherShards", L("Gather magic stone shards from Blue Gosarus"),
			new CollectItemObjective(666120, 7));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666120, character.Inventory.CountItem(666120), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666120, character.Inventory.CountItem(666120), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FBracken421Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1003);
		SetName(L("Doyor Fangs"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Doyors and bring fangs for caravan tooth-charms."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Charm-Carver] Alda"), "f_bracken_42_1");

		AddDrop(650692, 0.45f, MonsterId.Doyor_Blue);

		AddObjective("killDoyors", L("Kill Blue Doyors"),
			new KillObjective(15, new[] { MonsterId.Doyor_Blue }));

		AddObjective("gatherFangs", L("Gather fangs from Blue Doyors"),
			new CollectItemObjective(650692, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650692, character.Inventory.CountItem(650692), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650692, character.Inventory.CountItem(650692), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FBracken421Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1004);
		SetName(L("Glider Wings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Yellow Folibus and bring wings for a tinkerer's glider."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tinkerer] Kaspars"), "f_bracken_42_1");

		AddDrop(666172, 0.50f, MonsterId.Folibu_Yellow);

		AddObjective("killFolibus", L("Kill Yellow Folibus"),
			new KillObjective(12, new[] { MonsterId.Folibu_Yellow }));

		AddObjective("gatherWings", L("Gather translucent wings from Yellow Folibus"),
			new CollectItemObjective(666172, 5));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666172, character.Inventory.CountItem(666172), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666172, character.Inventory.CountItem(666172), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FBracken421Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1005);
		SetName(L("The Gosaru King"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Gosaru troupe to bring down the canopy King."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bounty Hunter] Jekabs"), "f_bracken_42_1");

		AddObjective("killTroupe", L("Thin Blue Gosarus"),
			new KillObjective(10, new[] { MonsterId.Gosaru_Blue }));

		AddObjective("killKing", L("Defeat the Gosaru King"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Velniamonkey, 1) },
				resetIdent: "killTroupe",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
		AddReward(new ItemReward(103309, 1));
	}
}

public class FBracken421Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1006);
		SetName(L("Khonot Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Khonot - Gosarus, Doyors, Tanus."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger] Peteris"), "f_bracken_42_1");

		AddObjective("killGosarus", L("Kill Blue Gosarus"),
			new KillObjective(12, new[] { MonsterId.Gosaru_Blue }));

		AddObjective("killDoyors", L("Kill Blue Doyors"),
			new KillObjective(12, new[] { MonsterId.Doyor_Blue }));

		AddObjective("killTanus", L("Kill Blue Tanus"),
			new KillObjective(12, new[] { MonsterId.Tanu_Blue }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
	}
}
