//--- Melia Script ----------------------------------------------------------
// Central Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Central Parias Forest.
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

public class FMaple241QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Rudas Bloom
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Florist] Morta"), "f_maple_24_1", 0, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1001);

			dialog.SetTitle(L("Morta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rudas Elavines bloom in Parias. The petals fetch silver at every Klaipeda flower stall."));
				await dialog.Msg(L("Kill twenty-five Rudas Elavines to drop enough petals for one season's stall."));

				var response = await dialog.Select(L("Petal run?"),
					Option(L("I'll harvest"), "help"),
					Option(L("Just petals?"), "info"),
					Option(L("Buy roses"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-five. They shed only under stress."));
						break;

					case "info":
						await dialog.Msg(L("The petals preserve - that's the magic. Cut flowers wilt in a day; Rudas petals keep a month."));
						break;

					case "leave":
						await dialog.Msg(L("Roses die. Rudas endure."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRudas", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Stall will be bright for weeks."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep harvesting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Petals sold out in three days."));
			}
		});

		// Quest 2: Atti Pollen
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Beekeeper] Kovas"), "f_maple_24_1", -1100, -600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1002);

			dialog.SetTitle(L("Kovas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Atti creatures carry rare pollen on their legs. Bees can't reach those blooms - but Attis walk right through them."));
				await dialog.Msg(L("Kill fifteen Attis and bring five pollen-clusters."));

				var response = await dialog.Select(L("Pollen run?"),
					Option(L("I'll kill and scrape"), "help"),
					Option(L("Why Attis?"), "info"),
					Option(L("Use honey"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Scrape the leg-joints. That's where the pollen packs."));
						break;

					case "info":
						await dialog.Msg(L("They walk the inner grove. Bees stay perimeter."));
						break;

					case "leave":
						await dialog.Msg(L("Honey's only half of beekeeping."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killAttis", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPollen", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Five clusters. The hives will triple."));
					character.Inventory.Remove(650041, character.Inventory.CountItem(650041), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Hives are booming. Queens laying double."));
			}
		});

		// Quest 3: Delione Thinning
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Groundskeeper] Dovydas"), "f_maple_24_1", -900, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1003);

			dialog.SetTitle(L("Dovydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A groundskeeper leans on a long-handled rake, the head clogged with Delione bloom-petals*{/}"));
				await dialog.Msg(L("Forty years tending Parias's inner paths. I know which root holds, which one rots from the heart. Deliones I can rake. Deliones-and-saplings, I cannot."));
				await dialog.Msg(L("They've found the four sapling-rings I planted last autumn. Uproot them faster than they grow. If they get the saplings before the saplings get their second year, the inner path goes treeless."));

				var response = await dialog.Select(L("Kill 35 Deliones to push them off the inner paths, then tend the four reed-staked sapling-rings I planted. Will you take a rake to the work?"),
					Option(L("I'll thin them and tend the saplings"), "help"),
					Option(L("Why thirty-five?"), "info"),
					Option(L("Find a younger back"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a folded apron of waxed linen and a bone trowel*{/}"));
						await dialog.Msg(L("Thirty-five Deliones, no shortcuts. They nest in clusters of seven - take the cluster-mother first, the rest scatter slow."));
						await dialog.Msg(L("Sapling-rings are reed-staked along the inner path. At each one: brush the loose dirt back over the roots, press the soil firm with the trowel, sing the Parias short-verse if you know it. The trees do better when sung to."));
						break;

					case "info":
						await dialog.Msg(L("Thirty-five is one Delione for every year I've tended these paths. Old groundskeeper's count - you measure the work in years, not in numbers."));
						await dialog.Msg(L("Visitors won't see the math. They'll just see the inner path open in spring instead of choked. That's enough for me."));
						break;

					case "leave":
						await dialog.Msg(L("Then the inner path goes treeless and the visitors stop coming and Parias forgets how to be Parias. I am too old to do it alone."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDeliones", out var killObj)) return;
				if (!quest.TryGetProgress("tendSaplings", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("{#666666}*He inspects each sapling-ring in turn, knee in the soil, breath held*{/}"));
					await dialog.Msg(L("Paths are walkable. Saplings firm-rooted, dirt brushed clean. They'll hold through the autumn rains."));
					await dialog.Msg(L("Take this. Groundskeeper's purse - thin pay, I know, but Parias has no rich treasury. Come back in five years and see the trees you saved."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Thirty-five Deliones first. Tending saplings while the Deliones still uproot them is just letting them have a head start tomorrow."));
				}
				else
				{
					await dialog.Msg(L("Paths are clear. Now the four sapling-rings - dirt back, trowel firm, short-verse if you know it. Take your time."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Visitors back on the inner paths every weekend. The four saplings have put on a finger of growth since you tended them - I checked this morning. Forty-one years of tending and I still measure growth in fingers."));
			}
		});

		// Sapling-ring tend points for Quest 1003
		//-------------------------------------------------------------------------
		void AddSaplingRing(int ringNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Sapling-Ring"), "f_maple_24_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_maple_24_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A reed-staked ring of young saplings, half uprooted*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_maple_24_1.Quest1003.Ring{ringNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already tended; the saplings stand straight*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Tending sapling-ring..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_maple_24_1.Quest1003.RingsTended", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_maple_24_1.Quest1003.RingsTended", count);
					character.ServerMessage(LF("Sapling-rings tended: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All sapling-rings tended! Return to Groundskeeper Dovydas.{/}"));
				}
				else
				{
					character.ServerMessage(L("Tending interrupted."));
				}
			});
		}

		AddSaplingRing(1, -800, 300, 0);
		AddSaplingRing(2, -1100, 500, 90);
		AddSaplingRing(3, -700, 600, 180);
		AddSaplingRing(4, -1000, 250, 270);

		// Quest 4: Cloverin Clovers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Alchemist] Rasa"), "f_maple_24_1", 50, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1004);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Cloverins carry a four-lobed clover in their leaf-patterns. My luck potion needs eight of them."));
				await dialog.Msg(L("Kill twenty Cloverins for the lucky clovers."));

				var response = await dialog.Select(L("Lucky clovers?"),
					Option(L("I'll harvest"), "help"),
					Option(L("Does luck work?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Eight. Don't hold out for nine - that's superstition."));
						break;

					case "info":
						await dialog.Msg(L("It works if you believe it works. Same as prayer."));
						break;

					case "leave":
						await dialog.Msg(L("Your loss."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCloverins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherClovers", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight clovers. Potion brews tonight."));
					character.Inventory.Remove(650051, character.Inventory.CountItem(650051), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep going."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Potion sold to a gambler. He tripled his purse."));
			}
		});

		// Quest 5: Elavine Matriarch
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Druid] Vaiva"), "f_maple_24_1", 1300, -100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1005);
			var matriarchSpawnedKey = "Laima.Quests.f_maple_24_1.Quest1005.MatriarchSpawned";

			dialog.SetTitle(L("Vaiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A druid runs her fingertips over a leaf turned grey-veined and brittle, then crushes it slowly*{/}"));
				await dialog.Msg(L("There is a Matriarch gestating at Parias's heart-grove. Not a normal Elavine - older, hungrier, her bloom-rot spreads through the soil and curdles every leaf within a half-league."));
				await dialog.Msg(L("Three rot-blooms feed her, planted at the cardinal points around her grove. Burn them and her gestation collapses - she'll emerge in fury, and that's when we kill her. Not before."));

				var response = await dialog.Select(L("Burn the three rot-blooms around the heart-grove, then kill 10 of her Rudas Elavine daughters to draw her out. The Matriarch fights when her brood thins. Will you draw her?"),
					Option(L("I'll burn the blooms and draw her"), "help"),
					Option(L("Why kill the daughters?"), "info"),
					Option(L("Leave the grove to its rot"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She hands you a small clay torch-pot, the wick already pitch-soaked*{/}"));
						await dialog.Msg(L("Three rot-blooms first - they sit at north, south, east of the heart-grove. Touch the torch to the bloom-stem, step back, watch it shrivel. Don't breathe the smoke."));
						await dialog.Msg(L("Then ten daughters. The Matriarch only emerges when she feels the brood thin. When she comes, fight on the heart-grove's stone ring - the rot can't cross stone."));
						break;

					case "info":
						await dialog.Msg(L("Daughters are her grafts. Each one she loses is a tendril of her self torn loose. Ten lost, and the loss is real enough that her instinct overrides her gestation. She emerges to defend what's left."));
						await dialog.Msg(L("It is cruel work. The grove is crueler. The rot has already taken the eastern half - if we don't end her now, the western half goes by autumn."));
						break;

					case "leave":
						await dialog.Msg(L("Then Parias dies and a thousand pilgrims stop coming and the cantors mourn a forest, not a saint. I have seen the like before. I would prefer not to see it again."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("burnBlooms", out var bObj)) return;
				if (!quest.TryGetProgress("killElavines", out var eObj)) return;
				if (!quest.TryGetProgress("killMatriarch", out var mObj)) return;

				if (bObj.Done && eObj.Done && mObj.Done)
				{
					await dialog.Msg(L("{#666666}*She kneels at the heart-grove and presses her forehead to the stone ring, eyes closed*{/}"));
					await dialog.Msg(L("Blooms burned, Matriarch dead. Grove will scar - the eastern half is past saving - but the western half breathes again."));
					await dialog.Msg(L("Take this. Druid's purse and a sapling cutting from the heart-grove itself. Plant it where it can hear running water; it will remember Parias forever."));
					character.Variables.Perm.Remove(matriarchSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (eObj.Done && !mObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(matriarchSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(matriarchSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Rudas_Elavine, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She bursts from the heart-grove!"));
							character.ServerMessage(L("{#FF9966}The Elavine Matriarch erupts in bloom!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find her before she retreats."));
					}
				}
				else if (!bObj.Done)
				{
					await dialog.Msg(L("Three rot-blooms first. Burn each at the cardinal points - the Matriarch can't gestate without them feeding her."));
				}
				else
				{
					await dialog.Msg(L("Blooms are ash. Now ten daughters - she emerges when the brood-loss outweighs the gestation. Ten is the number we know."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Grove's healing. The western half put out new shoots last week - small, but green. The eastern half remains bare; we don't pretend it will recover. Some things you save by accepting what's already lost."));
			}
		});

		// Rot-bloom burn points for Quest 1005
		//-------------------------------------------------------------------------
		void AddRotBloom(int bloomNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Rot-Bloom"), "f_maple_24_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_maple_24_1", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weeping bloom of grey rot, sap dripping into the heart-grove soil*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_maple_24_1.Quest1005.Bloom{bloomNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already burned to ash; the soil smokes white*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Burning bloom..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_maple_24_1.Quest1005.BloomsBurned", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_maple_24_1.Quest1005.BloomsBurned", count);
					character.ServerMessage(LF("Rot-blooms burned: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All rot-blooms burned! Now bait out the Matriarch.{/}"));
				}
				else
				{
					character.ServerMessage(L("Burning interrupted."));
				}
			});
		}

		AddRotBloom(1, 1200, -100, 0);
		AddRotBloom(2, 1400, 100, 90);
		AddRotBloom(3, 1100, 200, 180);

		// Quest 6: Parias Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Ranger] Justinas"), "f_maple_24_1", -300, 900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1006);

			dialog.SetTitle(L("Justinas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A ranger checks his bowstring against a callused thumb, listening to the note it makes*{/}"));
				await dialog.Msg(L("Parias has three species we sweep weekly - Rudas Elavines, Deliones, Cloverins. The triad. Each one alone is manageable; together they crowd the paths and crowd each other into the visitors."));
				await dialog.Msg(L("The grove's druid keeps a Druid's Cairn at the grove-edge to record the sweeps. Cantor-rangers cycle through Parias on a roster; the cairn-tally tells them whether to expect a clear path or bring extra arrows."));

				var response = await dialog.Select(L("Kill 12 Rudas Elavines, 12 Deliones, and 12 Cloverins, then chalk a tally line on the Druid's Cairn at the grove-edge. Will you take the contract?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why a cairn-tally instead of a roster-book?"), "info"),
					Option(L("Find a cantor-ranger"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a stub of white chalk wrapped in oilcloth*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. The cantor-rangers count the corpses on Sunday's foot-rotation."));
						await dialog.Msg(L("Cairn's at the grove-edge, west face. Chalk a single horizontal line under the last one - one line per sweep, no flourishes. The druid reads them like a calendar."));
						break;

					case "info":
						await dialog.Msg(L("Roster-books travel with the cantor-ranger; cairn-tallies stay where the next ranger needs them. The druid set the custom up in her grandmother's day - it's outlived three roster-systems already."));
						await dialog.Msg(L("The chalk lines run back forty years up the cairn's west face. Some weeks have no line - that's how we know which weeks the sweep failed."));
						break;

					case "leave":
						await dialog.Msg(L("Next cantor-ranger will ask the same. The paths don't sweep themselves and neither does the cairn."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRudas", out var rObj)) return;
				if (!quest.TryGetProgress("killDeliones", out var dObj)) return;
				if (!quest.TryGetProgress("killCloverins", out var cObj)) return;
				if (!quest.TryGetProgress("logCairn", out var lObj)) return;

				if (rObj.Done && dObj.Done && cObj.Done && lObj.Done)
				{
					await dialog.Msg(L("{#666666}*He nods at the cairn in the distance, where your chalk line sits clean under the last one*{/}"));
					await dialog.Msg(L("Tally logged, sweep math holds. The druid will read the cairn at sundown, the next ranger reads it at dawn."));
					await dialog.Msg(L("Coin in full, plus a shoulder-flask of grove-tea. The druid sends it for the swordhand - she does that for ones who chalk a clean line."));
					character.Quests.Complete(questId);
				}
				else if (rObj.Done && dObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Sweep done. Now the cairn - west face, single horizontal line, no flourishes. The druid notices flourishes."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each. The triad crowds together; if you get pinned by Rudas and Cloverin both, back to the path-stones - they won't follow you off the path."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Parias walks clean. Your chalk line on the cairn has weathered three rains and is still readable - the druid says that's how you know a swordhand pressed the chalk in like they meant it."));
			}
		});

		// Druid's Cairn for Quest 1006 tally
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Druid's Cairn"), "f_maple_24_1", -200, 950, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A moss-grown cairn at the grove edge, chalked with old ranger-tallies*{/}"));
				return;
			}

			var loggedKey = "Laima.Quests.f_maple_24_1.Quest1006.CairnLogged";
			if (character.Variables.Perm.GetBool(loggedKey, false))
			{
				await dialog.Msg(L("{#666666}*Your tally chalked already*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killRudas", out var rObj)) return;
			if (!quest.TryGetProgress("killDeliones", out var dObj)) return;
			if (!quest.TryGetProgress("killCloverins", out var cObj)) return;

			if (!(rObj.Done && dObj.Done && cObj.Done))
			{
				await dialog.Msg(L("{#666666}*The cairn is ready, but you haven't finished the sweep*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Logging cairn..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(loggedKey, true);
				character.ServerMessage(L("{#FFD700}Cairn logged. Return to Ranger Justinas.{/}"));
			}
			else
			{
				character.ServerMessage(L("Logging interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FMaple241Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1001);
		SetName(L("Rudas Bloom"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Rudas Elavines for preserving petals."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Florist] Morta"), "f_maple_24_1");

		AddObjective("killRudas", L("Kill Rudas Elavines"),
			new KillObjective(25, new[] { MonsterId.Rudas_Elavine }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640003, 2));
		AddReward(new ItemReward(640006, 2));
	}
}

public class FMaple241Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1002);
		SetName(L("Atti Pollen"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Attis and scrape inner-grove pollen for the beekeeper."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Beekeeper] Kovas"), "f_maple_24_1");

		AddObjective("killAttis", L("Kill Attis"),
			new KillObjective(15, new[] { MonsterId.Atti }));

		AddObjective("gatherPollen", L("Gather pollen-clusters"),
			new CollectItemObjective(650041, 5));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 2));
		AddReward(new ItemReward(640006, 2));
		AddReward(new ItemReward(640009, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650041, character.Inventory.CountItem(650041), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650041, character.Inventory.CountItem(650041), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple241Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1003);
		SetName(L("Delione Thinning"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Deliones choking the inner Parias paths."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Groundskeeper] Dovydas"), "f_maple_24_1");

		AddObjective("killDeliones", L("Kill Deliones"),
			new KillObjective(35, new[] { MonsterId.Delione }));

		AddObjective("tendSaplings", L("Tend the four sapling-rings"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_1.Quest1003.RingsTended", 4, true));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640003, 2));
		AddReward(new ItemReward(640006, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1003.RingsTended");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_1.Quest1003.Ring{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1003.RingsTended");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_1.Quest1003.Ring{i}");
	}
}

public class FMaple241Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1004);
		SetName(L("Lucky Clovers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Cloverins and gather lucky four-lobed clovers for luck potions."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Rasa"), "f_maple_24_1");

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(20, new[] { MonsterId.Cloverin }));

		AddObjective("gatherClovers", L("Gather lucky clovers"),
			new CollectItemObjective(650051, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 2));
		AddReward(new ItemReward(640006, 2));
		AddReward(new ItemReward(640009, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650051, character.Inventory.CountItem(650051), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650051, character.Inventory.CountItem(650051), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple241Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1005);
		SetName(L("The Elavine Matriarch"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Rudas Elavines to draw out the Matriarch rotting the heart-grove."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Druid] Vaiva"), "f_maple_24_1");

		AddObjective("burnBlooms", L("Burn the three rot-blooms"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_1.Quest1005.BloomsBurned", 3, true));

		AddObjective("killElavines", L("Kill Rudas Elavines"),
			new KillObjective(10, new[] { MonsterId.Rudas_Elavine }));

		AddObjective("killMatriarch", L("Defeat the Matriarch"),
			new KillObjective(1, new[] { MonsterId.Rudas_Elavine }));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(3800));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 2));
		AddReward(new ItemReward(640006, 2));
		AddReward(new ItemReward(640009, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1005.BloomsBurned");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_1.Quest1005.Bloom{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1005.BloomsBurned");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_1.Quest1005.Bloom{i}");
	}
}

public class FMaple241Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1006);
		SetName(L("Parias Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill the Parias triad: Rudas, Deliones, Cloverins."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger] Justinas"), "f_maple_24_1");

		AddObjective("killRudas", L("Kill Rudas Elavines"),
			new KillObjective(12, new[] { MonsterId.Rudas_Elavine }));

		AddObjective("killDeliones", L("Kill Deliones"),
			new KillObjective(12, new[] { MonsterId.Delione }));

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(12, new[] { MonsterId.Cloverin }));

		AddObjective("logCairn", L("Log the sweep on the Druid's Cairn"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_1.Quest1006.CairnLogged", 1, true));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(3800));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 2));
		AddReward(new ItemReward(640006, 2));
		AddReward(new ItemReward(640009, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1006.CairnLogged");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1006.CairnLogged");
	}
}
