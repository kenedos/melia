//--- Melia Script ----------------------------------------------------------
// Coastal Fortress Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the petrified fortress-city ruins.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
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

public class FFlash291QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Minos Charge
		//-------------------------------------------------------------------------
		AddNpc(20120, L("[Fortress Watch] Havel"), "f_flash_29_1", -1421, -204, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1001);

			dialog.SetTitle(L("Havel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("This fortress was the first wall north of the inland villages. The curse came up through the foundations and turned the whole garrison to stone where they stood - mid-watch, mid-step, mid-sentence."));
				await dialog.Msg(L("Now Minos herds graze through the petrified ranks. They charge anything that moves, and our patrol can't even walk the streets with this many around."));
				await dialog.Msg(L("Kill twenty-two of them. We need the perimeter walked - the stone-wards depend on it."));

				var response = await dialog.Select(L("Will you walk the streets for us?"),
					Option(L("I'll thin the Minos"), "help"),
					Option(L("Stone-wards?"), "info"),
					Option(L("Abandon the fortress"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. Don't try to dodge when they charge - just hold your ground. Otherwise they'll swarm you."));
						await dialog.Msg(L("Keep your ward-charm on you. They leave stone-dust behind when they fall."));
						break;

					case "info":
						await dialog.Msg(L("The curse leaks out of every cracked stone in this place. Our wards keep it from reaching the inland villages."));
						await dialog.Msg(L("If the patrol can't walk, the wards fail. And if the wards fail, the whole region turns to stone in a month."));
						break;

					case "leave":
						await dialog.Msg(L("This fortress is what holds the stone-wards together. If we fall, the region falls with us. I'm not going anywhere."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Patrol walked the whole round this morning, no trouble. The stone-wards hold for another week."));
					await dialog.Msg(L("Here's your pay. And a ward-charm - this curse hits hard, even glancing."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The streets are still full of hooves out there. Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Stone-wards are holding. The villages get to stay human another month."));
			}
		});

		// Quest 2: Naming the Stone
		//-------------------------------------------------------------------------
		AddNpc(147416, L("[Garrison Scribe] Vidas"), "f_flash_29_1", -570, -323, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1002);

			dialog.SetTitle(L("Vidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Every man and woman of the garrison is still standing where the curse caught them. Stone, every one - and most of their families never even got a body to mourn."));
				await dialog.Msg(L("I'm trying to put a name to each one before the weather wears the rank-plates smooth. There are eight scattered across the ruins I can't reach safely - too many Minos between them and my desk."));
				await dialog.Msg(L("If you can find five of them and read me back the names from their rank-plates, I'll write each one into the memorial book. The families finally get to grieve properly."));

				var response = await dialog.Select(L("Will you find five of them for me?"),
					Option(L("I'll find them"), "help"),
					Option(L("Memorial book?"), "info"),
					Option(L("Some other time"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Read each rank-plate carefully. The carvings are at the chest, not the brow, and the curse has made the stone brittle - don't touch them with bare hands."));
						await dialog.Msg(L("Five names will do. Take your time. They've waited a decade already - another hour won't hurt them."));
						break;

					case "info":
						await dialog.Msg(L("It's the only register left. The official roster burned in the fortress vault when the curse hit - paper doesn't survive that kind of heat-wave."));
						await dialog.Msg(L("Each name I confirm gets sent back to the inland villages. There are mothers who've waited a decade just to know their child's last post."));
						break;

					case "leave":
						await dialog.Msg(L("The plates won't be readable forever. If you change your mind, come back before the season turns."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var namesFound = character.Variables.Perm.GetInt("Laima.Quests.f_flash_29_1.Quest1002.RubbingsTaken", 0);

				if (namesFound >= 5)
				{
					await dialog.Msg(L("Five names, clear as the day they were carved. I can match every one to a family on my list."));
					await dialog.Msg(L("Here's your pay. The names go into the memorial book tonight, and a courier carries them south at first light."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep going. {0} of five names so far. Read carefully - one mistake and the wrong family gets the news.", namesFound));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The names reached the villages last week. Two families came back already - just to stand in the street where their kin was posted."));
			}
		});

		AddPetrifiedSoldier(1, 154023, -834, -324, 45);
		AddPetrifiedSoldier(2, 154026, -1931, 1, 45);
		AddPetrifiedSoldier(3, 154027, -987, -865, 135);
		AddPetrifiedSoldier(4, 154028, -219, -224, 0);
		AddPetrifiedSoldier(5, 154029, 399, -708, 270);
		AddPetrifiedSoldier(6, 154023, 182, 313, 315);
		AddPetrifiedSoldier(7, 154026, -49, 1034, 45);
		AddPetrifiedSoldier(8, 154027, 1499, 530, 135);

		AddFortressArmament(1, -1162, 353, 160);
		AddFortressArmament(2, -919, 121, 160);
		AddFortressArmament(3, -655, 37, 160);
		AddFortressArmament(4, -214, 228, 160);
		AddFortressArmament(5, -110, 71, 160);

		// Quest 3: The Fortress Armament
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Quartermaster] Osk"), "f_flash_29_1", -1493, 298, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1003);

			dialog.SetTitle(L("Osk"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The garrison was right in the middle of a watch-change when the curse took them. They left four weapon caches sealed up in the armament niches, never opened."));
				await dialog.Msg(L("Infroholder Bowmen are roosting on top of them now - the niches are high and sheltered, perfect for nesting. Kill fifteen and bring back the four caches, and the current watch finally gets proper gear."));

				var response = await dialog.Select(L("Will you bring the caches back?"),
					Option(L("I'll recover the caches"), "help"),
					Option(L("Proper equipment?"), "info"),
					Option(L("Use current gear"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fifteen. The bowmen shoot first, so close the gap fast. The caches are in the high niches, sealed with iron clasps."));
						await dialog.Msg(L("Use a knife to pry the clasps open. Don't force them - the wood's gone brittle from the curse."));
						break;

					case "info":
						await dialog.Msg(L("We've been patrolling with mismatched spears and old swords. The caches have proper standard-issue gear in them - uniform, heavy, built for the job."));
						await dialog.Msg(L("Better gear means faster rounds, and faster rounds mean tighter wards."));
						break;

					case "leave":
						await dialog.Msg(L("Our current gear is half-broken and mismatched. That's how we lost three patrol-hands last month - their equipment failed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBowmen", out var killObj)) return;

				var cachesRecovered = character.Variables.Perm.GetInt("Laima.Quests.f_flash_29_1.Quest1003.CachesRecovered", 0);

				if (killObj.Done && cachesRecovered >= 4)
				{
					await dialog.Msg(L("All four caches. Every patrol-hand gets fresh gear tomorrow. First proper re-equip in a decade."));
					await dialog.Msg(L("Here's your pay. The watch'll be drinking to you tonight."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Infroholder Bowmen. ");
					if (cachesRecovered < 4)
						status += LF("Recover more armament caches ({0}/4). ", cachesRecovered);

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Re-equip's done. The patrol walks faster and holds tighter. Haven't lost anyone in three months."));
			}
		});

		// Quest 5: The Herd-Master
		//-------------------------------------------------------------------------
		AddNpc(20141, L("[Bounty Hunter] Drus"), "f_flash_29_1", 1292, 141, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1005);

			dialog.SetTitle(L("Drus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Orange Minos herd has a Herd-Master - a curse-warped bull, twice the size of the others, with a stone plate across his brow."));
				await dialog.Msg(L("Kill ten of his pack and his pride'll force him out to put things in order. The bounty's huge - that brow-plate alone is worth more than a year of patrol pay."));

				var response = await dialog.Select(L("Want the contract?"),
					Option(L("I'll take the Herd-Master"), "help"),
					Option(L("Brow-plate?"), "info"),
					Option(L("Leave him to his herd"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten of the pack first. Once they're down, the Herd-Master will come out with six of his guard at his back - keep moving when he charges, that brow-plate's basically a battering ram."));
						await dialog.Msg(L("Good hunting."));
						break;

					case "info":
						await dialog.Msg(L("It's cursed bone-plate. The ward-smiths can carve it into charge-wards strong enough to stop a curse-wave at the wall."));
						await dialog.Msg(L("One plate makes about a dozen wards. That's enough to pay the whole patrol for a year."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next month. The bounty just keeps going up."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHerd", out var herdObj)) return;
				if (!quest.TryGetProgress("killHerdMaster", out var masterObj)) return;

				if (herdObj.Done && masterObj.Done)
				{
					await dialog.Msg(L("The brow-plate's intact. We'll get a dozen charge-wards out of just that one piece."));
					await dialog.Msg(L("Bounty paid, plus my cut. The fortress patrol's drinking on us tonight."));

					character.Quests.Complete(questId);
				}
				else if (herdObj.Done && !masterObj.Done)
				{
					await dialog.Msg(L("The Herd-Master's already drawn out. Hunt him down before the trail goes cold."));
				}
				else
				{
					await dialog.Msg(L("Herd's still too thick. Thin them out first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The brow-plate's been carved into charge-wards. The wall held through a curse-surge last week - first time in years."));
			}
		});

		// Quest 6: Wall Perimeter
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Wall Captain] Mara"), "f_flash_29_1", -276, -533, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1006);

			dialog.SetTitle(L("Mara"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The wall patrol runs the whole fortress perimeter. Right now we're getting bled on both flanks - Minos on the inner ground, Infroholders pinning us from the rampart-niches above."));
				await dialog.Msg(L("My patrol-hands won't commit until both flanks are thinned out. I need them walking again, and I need it done this week."));

				var response = await dialog.Select(L("Will you clear both flanks for us?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Pull the patrol back"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve of each. The Minos charge straight at you, the Infroholders pick at you from range. Watch out for both."));
						await dialog.Msg(L("Clear them out, and we're walking the full perimeter by Monday."));
						break;

					case "info":
						await dialog.Msg(L("The Minos charge. The Infroholders pin you down. Whichever one just hit you is the worse one."));
						await dialog.Msg(L("Together they're impossible to get through. Apart, we can walk the patrol just fine."));
						break;

					case "leave":
						await dialog.Msg(L("If we pull back, the stone-wards break. And if those break, every village turns to stone. Not on my watch."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var minObj)) return;
				if (!quest.TryGetProgress("killInfros", out var infObj)) return;

				if (minObj.Done && infObj.Done)
				{
					await dialog.Msg(L("Both flanks are thinned out. We walk the full perimeter Monday."));
					await dialog.Msg(L("Here's your pay. Stone-wards stay lit thanks to you."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!minObj.Done)
						status += L("Kill more Orange Minos. ");
					if (!infObj.Done)
						status += L("Kill more Infroholder Bowmen. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The patrol's walking clean now. Stone-wards held all the way through the equinox."));
			}
		});

		// Petrified soldier name-plates for Quest 1002
		//-------------------------------------------------------------------------
		void AddFortressArmament(int armamentNumber, int x, int z, int direction)
		{
			AddNpc(46212, L("Fortress Armament Cache"), "f_flash_29_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_29_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A sealed garrison weapon cache wedged into an armament niche. Iron clasps hold the lid shut.*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_flash_29_1.Quest1003.Armament{armamentNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This cache is already empty.*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Prying the clasps open..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_flash_29_1.Quest1003.CachesRecovered", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_flash_29_1.Quest1003.CachesRecovered", count);
					character.ServerMessage(LF("Armament caches recovered: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All four caches recovered. Return to Osk.{/}"));
				}
				else
				{
					character.ServerMessage(L("Recovery cancelled."));
				}
			});
		}

		void AddPetrifiedSoldier(int soldierNumber, int modelId, int x, int z, int direction)
		{
			AddNpc(modelId, L("Petrified Soldier"), "f_flash_29_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_29_1", 1002);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A garrison soldier turned to stone mid-step. The rank-plate at the chest is worn but legible.*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_flash_29_1.Quest1002.Soldier{soldierNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already noted this soldier's name.*{/}"));
					return;
				}

				var petrifyRolledKey = $"Laima.Quests.f_flash_29_1.Quest1002.Soldier{soldierNumber}.PetrifyRolled";
				if (!character.Variables.Perm.GetBool(petrifyRolledKey, false))
				{
					character.Variables.Perm.Set(petrifyRolledKey, true);
					if (GameRandom.Get().Next(100) < 30)
					{
						character.StartBuff(BuffId.GM_Petrification_PC_Debuff, TimeSpan.FromSeconds(3));
						character.ServerMessage(L("{#AAAAAA}The curse leaks out of the rank-plate - your limbs lock in stone!{/}"));
						return;
					}
				}

				var result = await character.TimeActions.StartAsync(L("Reading the rank-plate..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_flash_29_1.Quest1002.RubbingsTaken", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_flash_29_1.Quest1002.RubbingsTaken", count);
					character.ServerMessage(LF("Names noted: {0}/5", count));

					if (count >= 5)
						character.ServerMessage(L("{#FFD700}Five names noted. Return to Vidas.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class MinosChargeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1001);
		SetName(L("Minos Charge"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Orange Minos grazing through the petrified garrison so the fortress patrol can walk its rounds."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fortress Watch] Havel"), "f_flash_29_1");

		AddObjective("killMinos", L("Kill curse-grazing Orange Minos"),
			new KillObjective(22, new[] { MonsterId.Minos_Orange }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class NamingTheStoneQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1002);
		SetName(L("Naming the Stone"));
		SetType(QuestType.Sub);
		SetDescription(L("Find five petrified soldiers in the ruins and read their rank-plates back to the garrison scribe for the memorial book."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Garrison Scribe] Vidas"), "f_flash_29_1");

		AddObjective("noteNames", L("Read the names of petrified soldiers"),
			new VariableCheckObjective("Laima.Quests.f_flash_29_1.Quest1002.RubbingsTaken", 5, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_flash_29_1.Quest1002.RubbingsTaken");
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1002.Soldier{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1002.Soldier{i}.PetrifyRolled");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_flash_29_1.Quest1002.RubbingsTaken");
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1002.Soldier{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1002.Soldier{i}.PetrifyRolled");
		}
	}
}

public class TheFortressArmamentQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1003);
		SetName(L("The Fortress Armament"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Infroholder Bowmen roosting in the armament niches and recover four sealed fortress caches for the current watch."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Quartermaster] Osk"), "f_flash_29_1");

		AddObjective("killBowmen", L("Kill Infroholder Bowmen"),
			new KillObjective(15, new[] { MonsterId.Infroholder_Bow_Red }));

		AddObjective("recoverCaches", L("Recover fortress armament caches"),
			new VariableCheckObjective("Laima.Quests.f_flash_29_1.Quest1003.CachesRecovered", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_flash_29_1.Quest1003.CachesRecovered");
		for (int i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1003.Armament{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_flash_29_1.Quest1003.CachesRecovered");
		for (int i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1003.Armament{i}");
	}
}

public class TheHerdMasterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1005);
		SetName(L("The Herd-Master"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Minos herd to draw out the stone-plated Herd-Master, then bring him down for the brow-plate bounty."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bounty Hunter] Drus"), "f_flash_29_1");

		AddObjective("killHerd", L("Thin the Minos herd"),
			new KillObjective(10, new[] { MonsterId.Minos_Orange }));

		AddObjective("killHerdMaster", L("Defeat the Herd-Master and his guard"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Minos_Orange, 1, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Minos_Orange, 6),
				},
				resetIdent: "killHerd"));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

public class WallPerimeterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1006);
		SetName(L("Wall Perimeter"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Orange Minos on the inner flank and Infroholder Bowmen on the rampart flank to reopen the wall patrol."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Wall Captain] Mara"), "f_flash_29_1");

		AddObjective("killMinos", L("Kill Orange Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Orange }));

		AddObjective("killInfros", L("Kill Infroholder Bowmen"),
			new KillObjective(12, new[] { MonsterId.Infroholder_Bow_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
