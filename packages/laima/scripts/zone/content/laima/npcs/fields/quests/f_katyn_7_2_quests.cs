//--- Melia Script ----------------------------------------------------------
// Katyn 7-2 Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Katyn 7-2 (coastal graveyard, crypts, reef shore).
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

public class FKatyn72QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Sakmoli Tide
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Shore-Warden] Mindaugas"), "f_katyn_7_2", 2750, 750, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Sakmoli wash up the dunes after every tide. Forty kills and the graves stay covered."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Graves?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. Watch the wet sand."));
						break;

					case "info":
						await dialog.Msg(L("They drag the bones up. We rebury at dusk, they unbury by dawn."));
						break;

					case "leave":
						await dialog.Msg(L("Tide keeps pulling."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Shore quiets."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Reburied two rows yesterday."));
			}
		});

		// Quest 2: Ellomago Shades
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Exorcist] Ruta"), "f_katyn_7_2", 2200, -2800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ellomago shades drift the crypt aisles. Kill twenty-eight, bring seven fading spirits to anchor the ward."));

				var response = await dialog.Select(L("Spirits?"),
					Option(L("I'll bring"), "help"),
					Option(L("Anchor?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Salt the jars before sealing."));
						break;

					case "info":
						await dialog.Msg(L("Each spirit weighs the ward. Seven and the crypt door holds till next moon."));
						break;

					case "leave":
						await dialog.Msg(L("Door creaks open."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killEllomago", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSpirits", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Seven spirits sealed."));
					character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ward holds. Crypt's quiet for once."));
			}
		});

		// Quest 3: Red Jellyfish Reef
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Reef-Harvester] Egle"), "f_katyn_7_2", 2000, 600, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1003);

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Jellyfish strangle the coral. Kill eighteen, bring six blue corals before the bloom suffocates the reef."));

				var response = await dialog.Select(L("Coral?"),
					Option(L("I'll bring"), "help"),
					Option(L("Bloom?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut clean. Their sting holds an hour after they die."));
						break;

					case "info":
						await dialog.Msg(L("Reef breathes through the coral. Coral dies, fishery dies, village starves."));
						break;

					case "leave":
						await dialog.Msg(L("Reef chokes."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJelly", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCoral", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Six corals. Reef can rebuild from these."));
					character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Coral grafts taking. Reef colors back by autumn."));
			}
		});

		// Quest 4: Ridimed Bog
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Bog-Warden] Jurate"), "f_katyn_7_2", 400, -600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1004);

			dialog.SetTitle(L("Jurate"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bog-warden plants a fresh reed-stake into a soft patch of earth, then steps back to watch it sink half a fingerwidth*{/}"));
				await dialog.Msg(L("West bog used to be solid ground, my grandfather's time. Then the Ridimed came and started churning it. Now it's half-marsh, and our salt-cart route runs straight through the worst of it."));
				await dialog.Msg(L("Worse - the Ridimed dig. They've uncovered four old fisher-graves, dragged the bones up into the open. Old Katyn custom: bones don't belong above the line. We rebury or we accept the bad omen."));

				var response = await dialog.Select(L("Kill 18 Ridimed to push them back, then rebury the four disturbed graves - I've reed-staked each one. Will you wade in?"),
					Option(L("I'll wade in and do both"), "help"),
					Option(L("Why does the line matter?"), "info"),
					Option(L("Find a bog-walker"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She hands you a folded cloth bundle and a small clay jar of bog-water*{/}"));
						await dialog.Msg(L("Eighteen Ridimed, no shortcuts. Don't sink past the knee - if you do, throw weight backward, never forward. Forward is where the bog wants you."));
						await dialog.Msg(L("The four reed-stakes mark the open graves. At each one, fold the cloth over the bones, pour bog-water across the cloth, then mound dirt. Old Katyn rite. The dead expect it."));
						break;

					case "info":
						await dialog.Msg(L("Katyn coast was built on the bones of fishers who never came home from a tide. The line - the bog-line, the grave-line - is what separates the living from those bones. When bones come up over the line, the line breaks."));
						await dialog.Msg(L("Some say it's superstition. The fishers' widows say otherwise. Two of them won't walk the bog road since the bones came up - they take the long route around, three hours each way."));
						break;

					case "leave":
						await dialog.Msg(L("Then the salt-cart stops, the bones stay above the line, and the fishers' widows walk the long way until winter. I'll be here, planting reed-stakes for someone else."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRidimed", out var killObj)) return;
				if (!quest.TryGetProgress("reburyGraves", out var rObj)) return;

				if (killObj.Done && rObj.Done)
				{
					await dialog.Msg(L("{#666666}*She presses both hands flat to the bog-earth, breathes out long, then stands*{/}"));
					await dialog.Msg(L("Bog's clear, bones are back under the line. The four graves will hold through the wet season - I checked the mounds myself this morning."));
					await dialog.Msg(L("Take this. Bog-warden's purse, plus a stipend the widows asked me to add. Don't argue - they insisted."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Eighteen Ridimed first. No use reburying a grave the next Ridimed digs back up by sundown."));
				}
				else
				{
					await dialog.Msg(L("Bog's settling. Now the four reed-staked graves - cloth, bog-water, mound. Old rite, exactly as I told you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salt-cart through this morning, first run in a season. The two widows walked the bog road again last week - the short way, the way they used to. Neither said anything. Neither needed to."));
			}
		});

		// Disturbed grave rebury points for Quest 1004
		//-------------------------------------------------------------------------
		void AddDisturbedGrave(int graveNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Disturbed Grave"), "f_katyn_7_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_7_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A reed-staked grave, bones half-uncovered by Ridimed claws*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_7_2.Quest1004.Grave{graveNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already reburied; the bog smooths the soil over it*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reburying grave..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_7_2.Quest1004.GravesReburied", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_katyn_7_2.Quest1004.GravesReburied", count);
					character.ServerMessage(LF("Graves reburied: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All graves reburied! Return to Bog-Warden Jurate.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reburial interrupted."));
				}
			});
		}

		AddDisturbedGrave(1, 300, -700, 0);
		AddDisturbedGrave(2, 500, -500, 90);
		AddDisturbedGrave(3, 200, -800, 180);
		AddDisturbedGrave(4, 600, -400, 270);

		// Quest 5: The Sakmoli Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Algirdas"), "f_katyn_7_2", 3500, -4200, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_katyn_7_2.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Algirdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter sharpens a gutting-knife on a wet stone, the rasp rhythmic with the surf*{/}"));
				await dialog.Msg(L("Sakmoli Alpha runs the tide-pack on the east shore. Old, scarred, smart enough to retreat into the breakers when a hunter comes calling. He won't fight on dry sand."));
				await dialog.Msg(L("Three salt-pools at his roosting bay - that's where his pack rests at low tide. Salt the pools and the water sours; the pack scatters and the Alpha has to surface to drink fresh. Then we have him."));

				var response = await dialog.Select(L("Salt the three Sakmoli tide-pools, then kill 10 of his pack to bait him out. He fights on dry sand only - dodge the first lunge, strike his flank. Will you take him?"),
					Option(L("I'll take the Alpha"), "help"),
					Option(L("Why salt and not poison?"), "info"),
					Option(L("That's tide-work, not blade-work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you three rough cakes of salt-brick wrapped in oilcloth*{/}"));
						await dialog.Msg(L("Three cakes, three pools. Drop one in each, let it dissolve. Don't stay to watch - the pack smells the salt and comes up curious."));
						await dialog.Msg(L("Then ten Sakmoli, no shortcuts. The Alpha surfaces around the eighth or ninth. Stand on the dry rim - he won't follow you off the wet line."));
						break;

					case "info":
						await dialog.Msg(L("Poison kills the pack and the Alpha both. Salt only scatters them - the pack goes back to the deep, the Alpha surfaces alone. We want him alone, not the whole pack dead in the bay."));
						await dialog.Msg(L("Dead Sakmoli rotting in the bay would foul the fishery for two seasons. Katyn villages can't afford that. So: salt, not poison. Old shore-rule."));
						break;

					case "leave":
						await dialog.Msg(L("Tide rolls on, the Alpha keeps pulling fishers under. The next widow will be along to ask where her husband went. I'll point her to you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("saltPools", out var sObj)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;

				if (sObj.Done && pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("{#666666}*He sheaths the gutting-knife and counts coin onto a salt-stained barrel-head*{/}"));
					await dialog.Msg(L("Pools salted, Alpha down, tide-pack scattered to the deep. Fishery stays clean - that's what the villages will thank you for, not the kill itself."));
					await dialog.Msg(L("Bounty plus a fishery-stipend. The fishers' guild voted on it last spring; you're the first to collect."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Sakmoli, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Sakmoli Alpha rises from the breakers!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else if (!sObj.Done)
				{
					await dialog.Msg(L("Three salt-cakes, three pools. The pack won't surface for the bait if the pools still run sweet."));
				}
				else
				{
					await dialog.Msg(L("Pools are sour, the pack's surfaced. Now ten of them - the Alpha shows by the eighth."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pack's leaderless, fishery's clean again. The fishers' guild posted a thank-you on the village board - they wrote 'a swordhand who understood the bay.' That's rare from them."));
			}
		});

		// Sakmoli tide-pool salt points for Quest 1005
		//-------------------------------------------------------------------------
		void AddTidePool(int poolNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Sakmoli Tide-Pool"), "f_katyn_7_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_7_2", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A salt-rimed tide-pool, kelp coiled at the bottom*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_7_2.Quest1005.Pool{poolNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already salted; the water clouds white*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Salting pool..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_7_2.Quest1005.PoolsSalted", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_katyn_7_2.Quest1005.PoolsSalted", count);
					character.ServerMessage(LF("Tide-pools salted: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All tide-pools salted! Now bait out the Alpha.{/}"));
				}
				else
				{
					character.ServerMessage(L("Salting interrupted."));
				}
			});
		}

		AddTidePool(1, 3300, -4000, 0);
		AddTidePool(2, 3700, -4400, 90);
		AddTidePool(3, 3500, -4500, 180);

		// Quest 6: Katyn Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Tomas"), "f_katyn_7_2", 2700, 2400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1006);

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A militia-captain pins a fresh dune-patrol roster to a salt-warped board, fingers gritty with shore-sand*{/}"));
				await dialog.Msg(L("Three monster-types contest the Katyn coast. Sakmoli on the wet sand, Ridimed in the bog-edges, Red Jellyfish in the tide-pools. We sweep weekly or the dune-line collapses."));
				await dialog.Msg(L("The dune-watch is a string of three watchers strung along the cliff-tops. They can't see the kill from up there - they signal-flag down to me when the bell rings. No bell, no signal, no shift change. Old Pelke custom from the war years."));

				var response = await dialog.Select(L("Kill 12 Sakmoli, 12 Ridimed, and 12 Red Jellyfish, then ring the Shore-Bell at the watch-cairn - one long peal carries to the dune-watch. Take the contract?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why a bell instead of a runner?"), "info"),
					Option(L("Find a coastal hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She unhooks a small bronze pull-token from her belt and hands it across*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. The dune-watch counts the corpses on the next morning's tide-walk."));
						await dialog.Msg(L("Bell's at the watch-cairn above the south crook. Hold the token to the rope-ring, pull once, count to four, release. One long peal - any shorter and the watchers think it's wind."));
						break;

					case "info":
						await dialog.Msg(L("A runner from the watch-cairn to me is forty minutes on a clear day. A bell-peal is four heartbeats. In a war, Pelke chose the bell. We never went back."));
						await dialog.Msg(L("The bell also lets the next sweep-hand know whether anyone's been here today. The cliff-watchers count the peals across the week and report to me on Sundays."));
						break;

					case "leave":
						await dialog.Msg(L("Coast stays choked, dune-watch waits in the cold for a signal that never comes. I'll find a coastal hand if I have to swim them in myself."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var sObj)) return;
				if (!quest.TryGetProgress("killRidimed", out var rObj)) return;
				if (!quest.TryGetProgress("killJelly", out var jObj)) return;
				if (!quest.TryGetProgress("ringBell", out var bObj)) return;

				if (sObj.Done && rObj.Done && jObj.Done && bObj.Done)
				{
					await dialog.Msg(L("{#666666}*She glances toward the cliff-watch, where two distant signal-flags have just dipped in answer to your bell*{/}"));
					await dialog.Msg(L("Bell heard. Sweep counted. The dune-watch shifts at sundown, fed and warm. That's because of you."));
					await dialog.Msg(L("Coin in full, plus a half-purse from the cliff-watchers' tin. They never come down to thank you in person - the bell is their thanks."));
					character.Quests.Complete(questId);
				}
				else if (sObj.Done && rObj.Done && jObj.Done)
				{
					await dialog.Msg(L("Sweep counts up. Now the bell - watch-cairn, south crook, one long peal. The cliff-watchers wait."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each. Don't shortchange one - the dune-walk counts every species on Monday."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Dune-patrols held the line all season. The cliff-watchers signal-flagged a thank-you at last week's shift change - I had to ask them to repeat it twice. They're not used to thanking strangers."));
			}
		});

		// Shore-Bell for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Shore-Bell"), "f_katyn_7_2", 2700, 2300, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A bronze shore-bell hung from a watch-cairn, rope dark with sea-spray*{/}"));
				return;
			}

			var rungKey = "Laima.Quests.f_katyn_7_2.Quest1006.BellRung";
			if (character.Variables.Perm.GetBool(rungKey, false))
			{
				await dialog.Msg(L("{#666666}*Already rung; the dune-watch will have heard*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killSakmoli", out var sObj)) return;
			if (!quest.TryGetProgress("killRidimed", out var rObj)) return;
			if (!quest.TryGetProgress("killJelly", out var jObj)) return;

			if (!(sObj.Done && rObj.Done && jObj.Done))
			{
				await dialog.Msg(L("{#666666}*The rope is in your hand, but you haven't finished the sweep*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Ringing bell..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(rungKey, true);
				character.ServerMessage(L("{#FFD700}Shore-Bell rung. Return to Militia-Captain Tomas.{/}"));
			}
			else
			{
				character.ServerMessage(L("Ringing interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FKatyn72Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1001);
		SetName(L("Sakmoli Tide"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Sakmoli washing up the Katyn shore."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shore-Warden] Mindaugas"), "f_katyn_7_2");

		AddObjective("killSakmoli", L("Kill Sakmoli"),
			new KillObjective(40, new[] { MonsterId.Sakmoli }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FKatyn72Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1002);
		SetName(L("Crypt Shades"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ellomago and gather fading spirits to anchor the crypt ward."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Exorcist] Ruta"), "f_katyn_7_2");

		AddObjective("killEllomago", L("Kill Ellomago"),
			new KillObjective(28, new[] { MonsterId.Ellomago }));

		AddObjective("gatherSpirits", L("Gather fading spirits"),
			new CollectItemObjective(664096, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn72Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1003);
		SetName(L("Reef Harvest"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Jellyfish and gather blue corals before the bloom kills the reef."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Reef-Harvester] Egle"), "f_katyn_7_2");

		AddObjective("killJelly", L("Kill Red Jellyfish"),
			new KillObjective(18, new[] { MonsterId.Jellyfish_Red }));

		AddObjective("gatherCoral", L("Gather blue corals"),
			new CollectItemObjective(668044, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn72Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1004);
		SetName(L("Bog Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ridimed crawling the west Katyn bog."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bog-Warden] Jurate"), "f_katyn_7_2");

		AddObjective("killRidimed", L("Kill Ridimed"),
			new KillObjective(18, new[] { MonsterId.Ridimed }));

		AddObjective("reburyGraves", L("Rebury the four disturbed graves"),
			new VariableCheckObjective("Laima.Quests.f_katyn_7_2.Quest1004.GravesReburied", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1004.GravesReburied");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_7_2.Quest1004.Grave{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1004.GravesReburied");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_7_2.Quest1004.Grave{i}");
	}
}

public class FKatyn72Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1005);
		SetName(L("The Sakmoli Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Sakmoli to drag the tide-pack Alpha from the breakers."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Algirdas"), "f_katyn_7_2");

		AddObjective("saltPools", L("Salt the three Sakmoli tide-pools"),
			new VariableCheckObjective("Laima.Quests.f_katyn_7_2.Quest1005.PoolsSalted", 3, true));

		AddObjective("killPack", L("Kill Sakmoli"),
			new KillObjective(10, new[] { MonsterId.Sakmoli }));

		AddObjective("killAlpha", L("Defeat the Sakmoli Alpha"),
			new KillObjective(1, new[] { MonsterId.Sakmoli }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1005.PoolsSalted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_7_2.Quest1005.Pool{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1005.PoolsSalted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_7_2.Quest1005.Pool{i}");
	}
}

public class FKatyn72Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1006);
		SetName(L("Katyn Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Sakmoli, Ridimed, and Red Jellyfish along the Katyn coast."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Tomas"), "f_katyn_7_2");

		AddObjective("killSakmoli", L("Kill Sakmoli"),
			new KillObjective(12, new[] { MonsterId.Sakmoli }));

		AddObjective("killRidimed", L("Kill Ridimed"),
			new KillObjective(12, new[] { MonsterId.Ridimed }));

		AddObjective("killJelly", L("Kill Red Jellyfish"),
			new KillObjective(12, new[] { MonsterId.Jellyfish_Red }));

		AddObjective("ringBell", L("Ring the Shore-Bell at the watch-cairn"),
			new VariableCheckObjective("Laima.Quests.f_katyn_7_2.Quest1006.BellRung", 1, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1006.BellRung");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1006.BellRung");
	}
}
