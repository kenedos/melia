//--- Melia Script ----------------------------------------------------------
// Pilgrim Road West Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the west section of Pilgrim Road.
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

public class FPilgrimroad414QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Purple Repusbunny Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Tollwarden] Mindaugas"), "f_pilgrimroad_41_4", -400, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Repusbunnies swarm the west road. Forty-five kills and pilgrims walk again."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Pilgrims?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty-five. Mind the burrows."));
						break;

					case "info":
						await dialog.Msg(L("The caravan's been camped behind the waystones three days. Road needs clearing."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays blocked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Caravan's moving."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First pilgrims passed at dawn."));
			}
		});

		// Quest 2: Bowbunny Fletchings
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Caravan-Guard] Ruta"), "f_pilgrimroad_41_4", 600, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bow Repusbunnies pick pilgrims off the ridge. Kill thirty, bring eight fletchings so we can mark the range."));

				var response = await dialog.Select(L("Fletchings?"),
					Option(L("I'll bring"), "help"),
					Option(L("Mark?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fletchings, not arrows. Arrows snap."));
						break;

					case "info":
						await dialog.Msg(L("Fletchings tell us the draw. Draw tells us the range. Range tells us where to station shields."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims stay pincushioned."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBowBunnies", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFletchings", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight fletchings. Range mapped."));
					character.Inventory.Remove(650254, character.Inventory.CountItem(650254), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Shield line holds. Ridge is quieter."));
			}
		});

		// Quest 3: Mage-Stub Bark
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Hedge-Witch] Vaiva"), "f_pilgrimroad_41_4", -500, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1003);

			dialog.SetTitle(L("Vaiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tree-Mage Stubs carry rootspell in their bark. Kill fifteen, bring five strips."));

				var response = await dialog.Select(L("Bark?"),
					Option(L("I'll bring"), "help"),
					Option(L("Rootspell?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Strip clean. Bark with moss won't read."));
						break;

					case "info":
						await dialog.Msg(L("Older than the road. Hedge-charms still pull from it when nothing else answers."));
						break;

					case "leave":
						await dialog.Msg(L("Hedge goes silent, then."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStubs", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBark", out var bObj)) return;

				if (killObj.Done && bObj.Done)
				{
					await dialog.Msg(L("Five strips. Hedge wards will hold the month."));
					character.Inventory.Remove(650256, character.Inventory.CountItem(650256), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Wards burning green. Good sign."));
			}
		});

		// Quest 4: Waystone Rootcrystals
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Waystone-Keeper] Darius"), "f_pilgrimroad_41_4", -1100, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1004);

			dialog.SetTitle(L("Darius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A waystone-keeper traces a finger along a rune-line that no longer hums under his touch*{/}"));
				await dialog.Msg(L("My grandmother kept these waystones. So did her mother. Eight generations, and not one of them ever saw the ley-lines bend the way they're bending now."));
				await dialog.Msg(L("Rootcrystals push up between the markers. Pilgrims walking by eye end up a league off-route, sometimes two. Last week a wool-cart drove three days west when it should have gone north - the driver swore the road kept turning under his wheels."));

				var response = await dialog.Select(L("Break 12 Rootcrystals, then re-true the four ley-marker waystones along the road. Lay your hand flat on each marker - the ley reads through the bone of the palm. Will you walk the line?"),
					Option(L("I'll break the crystals and re-true the markers"), "help"),
					Option(L("How does a waystone bend?"), "info"),
					Option(L("That sounds like priest-work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He blesses your palm with iron-water from a clay cup, then dries it on his sleeve*{/}"));
						await dialog.Msg(L("The crystals first - 12 of them between the markers. Strike clean, don't let the shards touch the rune-lines or the ley sours."));
						await dialog.Msg(L("Then the four marker-stones. Lay your right hand flat on each carved face, count to ten. The runes will hum if the line is true; if they buzz, the marker's still tangled and you need to step back to the last one."));
						break;

					case "info":
						await dialog.Msg(L("A ley-line is the path the world remembers. The waystones don't cause the line - they hold the line where pilgrims can read it by eye."));
						await dialog.Msg(L("Crystals push the bedrock and the bedrock carries the line. Move the bedrock, the line drifts. Pilgrims drift with it. Some never come back."));
						break;

					case "leave":
						await dialog.Msg(L("It is priest-work, in a sense. But the priests are at Salvia and the wool-cart's still out there somewhere. I'll wait here for someone less particular."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("retrueMarkers", out var rObj)) return;

				if (killObj.Done && rObj.Done)
				{
					await dialog.Msg(L("{#666666}*He pulls a copper pendulum from his collar and lets it swing - it stops dead-true on the road's axis*{/}"));
					await dialog.Msg(L("Ley reads straight from here to the next caravan-stop. Markers humming clean, runes warm to the palm again."));
					await dialog.Msg(L("Take this. Old waystone-keeper's coin, minted in my grandmother's time. Spends the same as new."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("The crystals are still pushing the bedrock. No use re-truing markers while the line keeps bending under your feet."));
				}
				else
				{
					await dialog.Msg(L("Crystals down, ley settling. Now the four markers. Right palm flat, count to ten, listen for the hum."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three caravans through this week, none drifted. The wool-cart found its way back too - driver swears he heard the markers humming as he passed each one. He's not wrong."));
			}
		});

		// Ley-marker waystones for Quest 1004
		//-------------------------------------------------------------------------
		void AddLeyMarker(int markerNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Ley-Marker Waystone"), "f_pilgrimroad_41_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_4", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered ley-marker waystone, ley-runes faintly glowing*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_41_4.Quest1004.Marker{markerNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already re-trued; the runes hum steady*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Re-truing waystone..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_41_4.Quest1004.MarkersTrued", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_41_4.Quest1004.MarkersTrued", count);
					character.ServerMessage(LF("Marker-stones re-trued: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All marker-stones re-trued! Return to Waystone-Keeper Darius.{/}"));
				}
				else
				{
					character.ServerMessage(L("Re-truing interrupted."));
				}
			});
		}

		AddLeyMarker(1, -900, -300, 0);
		AddLeyMarker(2, -300, 200, 90);
		AddLeyMarker(3, 700, 700, 180);
		AddLeyMarker(4, 1500, -700, 270);

		// Quest 5: The Warren-King
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Bounty Hunter] Saule"), "f_pilgrimroad_41_4", 1800, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_pilgrimroad_41_4.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Saule"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter sits on an upturned crate, sharpening a curved blade with long, patient strokes*{/}"));
				await dialog.Msg(L("Warren-King in the west tunnels. Twice my size, scarred from forty fights, smart enough to bolt before any single hunter pins him."));
				await dialog.Msg(L("That's the trick - he never fights. He runs. Three burrow-holes I've staked are his escape routes. Plug them, and he has to stand."));

				var response = await dialog.Select(L("Plug the three staked burrow-holes, then kill 10 Repusbunnies to draw him up. He won't show his face for less than ten of his own pack dead. Are you in?"),
					Option(L("I'm in"), "help"),
					Option(L("Why doesn't he just fight?"), "info"),
					Option(L("That's a hunter's job, not mine"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She points west with the blade-tip, then taps three spots on a charcoal-sketched map*{/}"));
						await dialog.Msg(L("North slope, south crook, east scree. Each hole has a wedge of pine I cut to size - jam it deep, pack with dirt and stones."));
						await dialog.Msg(L("Then ten kills. He'll come up shooting - he uses a Repusbunny bow, full draw. Take cover behind the warren-mounds."));
						break;

					case "info":
						await dialog.Msg(L("Survival, mostly. Bunnies that fight die quick; bunnies that bolt and re-form a new warren keep breeding. Warren-Kings are the ones smart enough to know that. Hard to hunt. Easy to wait out, if we had time."));
						await dialog.Msg(L("We don't. He's been killing the caravan-guards on the west road every fortnight for a season. The widows have stopped asking when their husbands are coming home."));
						break;

					case "leave":
						await dialog.Msg(L("Warren swells, widows multiply, the road's caravan-guards stop signing up. I'll be sharpening this blade when you walk back through."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("plugBurrows", out var bObj)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;

				if (bObj.Done && pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("{#666666}*She slides the blade into its sheath with a satisfied click and flips you a leather purse*{/}"));
					await dialog.Msg(L("Burrows plugged, king's down, warren collapses on itself by week's end. The caravan-guards drink at my expense tonight - I'm telling them why."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Repusbunny_Purple, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Warren-King emerges from the west warren!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else if (!bObj.Done)
				{
					await dialog.Msg(L("Three burrow-holes still open. North slope, south crook, east scree. Plug them or he runs the moment you draw blade."));
				}
				else
				{
					await dialog.Msg(L("Burrows are sealed. Now ten of his pack - he won't surface for a single one less."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Warren's collapsed in on itself. The widows came around to ask whose blade did it - I told them mine, but they didn't believe me. They were right not to."));
			}
		});

		// Burrow-hole plug points for Quest 1005
		//-------------------------------------------------------------------------
		void AddBurrowHole(int burrowNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Warren Burrow-Hole"), "f_pilgrimroad_41_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_4", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A burrow-hole on the warren ridge, freshly dug*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_41_4.Quest1005.Burrow{burrowNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already plugged with stone and turf*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Plugging burrow..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_41_4.Quest1005.BurrowsPlugged", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_41_4.Quest1005.BurrowsPlugged", count);
					character.ServerMessage(LF("Burrows plugged: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All burrows plugged! Now bait out the Warren-King.{/}"));
				}
				else
				{
					character.ServerMessage(L("Plugging interrupted."));
				}
			});
		}

		AddBurrowHole(1, 1500, -800, 0);
		AddBurrowHole(2, 1900, -1100, 90);
		AddBurrowHole(3, 1600, -1200, 180);

		// Quest 6: West Road Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Road-Marshal] Aldona"), "f_pilgrimroad_41_4", 1700, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1006);

			dialog.SetTitle(L("Aldona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A road-marshal pins a fresh ledger-page to a board, the older pages fluttering in the road-wind*{/}"));
				await dialog.Msg(L("West stretch is the worst on the whole pilgrim road. Three monster-types, none of them light, all of them territorial. We sweep it weekly or it doesn't stay swept."));
				await dialog.Msg(L("And the Marshal's office in Salvia doesn't pay on a hunter's word. They pay on tally-stones. One stone per kill, dropped on the Toll-Stone Cairn at the bend, counted by the cantor on Sundays."));

				var response = await dialog.Select(L("Kill 12 Repusbunnies, 12 Bow Repusbunnies, and 12 Tree-Mage Stubs, then drop one tally-stone on the Toll-Stone Cairn for the marshal-rolls. Take the contract?"),
					Option(L("I'll take the contract"), "help"),
					Option(L("Why tally-stones, not coin-receipts?"), "info"),
					Option(L("Find a closer hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She presses a smooth river-stone into your palm, weight of a small egg*{/}"));
						await dialog.Msg(L("Thirty-six kills. Don't pad the count - the cantor weighs the cairn-stones against my ledger and the math has to match."));
						await dialog.Msg(L("Cairn's at the toll bend, where the road kinks south. Drop the stone, let it lie. I'll countersign after."));
						break;

					case "info":
						await dialog.Msg(L("Coin-receipts can be forged, copied, lost in the post. A river-stone in a sealed cairn cannot. Pelke's old custom from the demon war - simple is honest."));
						await dialog.Msg(L("Pay scales with the count, so don't lie about the kills. If I undercount you, complain to me. If you overcount me, the cantor finds out by Sunday and the marshal's office bans you for a year."));
						break;

					case "leave":
						await dialog.Msg(L("Then the west stretch stays thick and the next pilgrim-cart pays the price. There is always a closer hunter; there is rarely a willing one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var bObj)) return;
				if (!quest.TryGetProgress("killBowBunnies", out var wObj)) return;
				if (!quest.TryGetProgress("killStubs", out var sObj)) return;
				if (!quest.TryGetProgress("dropTally", out var tObj)) return;

				if (bObj.Done && wObj.Done && sObj.Done && tObj.Done)
				{
					await dialog.Msg(L("{#666666}*She marks your ledger-row with a tar-ink stripe and counts coin into a cloth*{/}"));
					await dialog.Msg(L("Sweep done, cairn-stone dropped, math holds. Marshal's coin, paid honest."));
					await dialog.Msg(L("Stop by next fortnight if you want the contract again. The road never stops needing it."));
					character.Quests.Complete(questId);
				}
				else if (bObj.Done && wObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Sweep complete. Now drop the tally-stone on the cairn at the toll bend - I can't pay until the cantor counts it."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each, river-stone in pocket. Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Marshal's patrols cover the west stretch hourly now, wages paid by your levy. The cantor still weighs your stone every Sunday - says it's heavier than it should be. He means it as a compliment."));
			}
		});

		// Toll-Stone Cairn for Quest 1006 tally
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Toll-Stone Cairn"), "f_pilgrimroad_41_4", 1700, 1100, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A weather-worn cairn beside the toll bend, ringed with old tally-stones*{/}"));
				return;
			}

			var droppedKey = "Laima.Quests.f_pilgrimroad_41_4.Quest1006.TallyDropped";
			if (character.Variables.Perm.GetBool(droppedKey, false))
			{
				await dialog.Msg(L("{#666666}*Your tally-stone sits on the cairn already*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killBunnies", out var bObj)) return;
			if (!quest.TryGetProgress("killBowBunnies", out var wObj)) return;
			if (!quest.TryGetProgress("killStubs", out var sObj)) return;

			if (!(bObj.Done && wObj.Done && sObj.Done))
			{
				await dialog.Msg(L("{#666666}*The cairn is ready to receive a tally-stone, but you haven't finished the sweep*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Dropping tally-stone..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(droppedKey, true);
				character.ServerMessage(L("{#FFD700}Tally-stone laid on the cairn. Return to Road-Marshal Aldona.{/}"));
			}
			else
			{
				character.ServerMessage(L("Drop interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FPilgrimroad414Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1001);
		SetName(L("Purple Repusbunny Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Repusbunnies blocking the west pilgrim road."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tollwarden] Mindaugas"), "f_pilgrimroad_41_4");

		AddObjective("killBunnies", L("Kill Purple Repusbunnies"),
			new KillObjective(45, new[] { MonsterId.Repusbunny_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FPilgrimroad414Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1002);
		SetName(L("Bowbunny Fletchings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bow Repusbunnies and bring fletchings to map the ridge shots."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan-Guard] Ruta"), "f_pilgrimroad_41_4");

		AddObjective("killBowBunnies", L("Kill Bow Repusbunnies"),
			new KillObjective(30, new[] { MonsterId.Repusbunny_Bow_Purple }));

		AddObjective("gatherFletchings", L("Gather fletchings"),
			new CollectItemObjective(650254, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650254, character.Inventory.CountItem(650254), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650254, character.Inventory.CountItem(650254), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad414Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1003);
		SetName(L("Mage-Stub Bark"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tree-Mage Stubs and bring rootspell bark for hedge wards."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hedge-Witch] Vaiva"), "f_pilgrimroad_41_4");

		AddObjective("killStubs", L("Kill Tree-Mage Stubs"),
			new KillObjective(15, new[] { MonsterId.Stub_Tree_Mage }));

		AddObjective("gatherBark", L("Gather bark strips"),
			new CollectItemObjective(650256, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650256, character.Inventory.CountItem(650256), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650256, character.Inventory.CountItem(650256), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad414Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1004);
		SetName(L("Waystone Tangles"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals bending the waystone ley off the west road."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Waystone-Keeper] Darius"), "f_pilgrimroad_41_4");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(12, new[] { MonsterId.Rootcrystal_05 }));

		AddObjective("retrueMarkers", L("Re-true the four ley-marker waystones"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_4.Quest1004.MarkersTrued", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1004.MarkersTrued");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_4.Quest1004.Marker{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1004.MarkersTrued");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_4.Quest1004.Marker{i}");
	}
}

public class FPilgrimroad414Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1005);
		SetName(L("The Warren-King"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Repusbunnies to draw out the Warren-King from the west warren."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Saule"), "f_pilgrimroad_41_4");

		AddObjective("plugBurrows", L("Plug the three Warren burrow-holes"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_4.Quest1005.BurrowsPlugged", 3, true));

		AddObjective("killPack", L("Kill Purple Repusbunnies"),
			new KillObjective(10, new[] { MonsterId.Repusbunny_Purple }));

		AddObjective("killAlpha", L("Defeat the Warren-King"),
			new KillObjective(1, new[] { MonsterId.Repusbunny_Purple }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1005.BurrowsPlugged");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_4.Quest1005.Burrow{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1005.BurrowsPlugged");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_4.Quest1005.Burrow{i}");
	}
}

public class FPilgrimroad414Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1006);
		SetName(L("West Road Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Purple Repusbunnies, Bow Repusbunnies, and Tree-Mage Stubs."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Marshal] Aldona"), "f_pilgrimroad_41_4");

		AddObjective("killBunnies", L("Kill Purple Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Purple }));

		AddObjective("killBowBunnies", L("Kill Bow Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Bow_Purple }));

		AddObjective("killStubs", L("Kill Tree-Mage Stubs"),
			new KillObjective(12, new[] { MonsterId.Stub_Tree_Mage }));

		AddObjective("dropTally", L("Lay a tally-stone on the Toll-Stone Cairn"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_4.Quest1006.TallyDropped", 1, true));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1006.TallyDropped");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1006.TallyDropped");
	}
}
