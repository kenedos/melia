//--- Melia Script ----------------------------------------------------------
// Steel Heights Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Steel Heights plateau (Tiny Mages, Keparis, Spions, Harugals).
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

public class FTableland74QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Tiny Mage Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Plateau-Watch] Mindaugas"), "f_tableland_74", -700, -250, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tiny Brown Mages swarm the lower terraces. Forty killed and the climbing path opens."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Path?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. Mind their staves."));
						break;

					case "info":
						await dialog.Msg(L("Caravans need the terrace path. Mages have it choked."));
						break;

					case "leave":
						await dialog.Msg(L("Path stays choked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Path opens."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First caravan crossed at dawn."));
			}
		});

		// Quest 2: Kepari Flesh
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Alchemist] Ruta"), "f_tableland_74", 150, -1050, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Keparis nest the rock-folds. Kill twenty-five, bring seven flesh-cuts for reagent work."));

				var response = await dialog.Select(L("Cuts?"),
					Option(L("I'll bring"), "help"),
					Option(L("Reagent?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cold-wrap the cuts. They sour fast."));
						break;

					case "info":
						await dialog.Msg(L("Kepari flesh holds anti-petrification factors. Eight cuts, half a batch of salve."));
						break;

					case "leave":
						await dialog.Msg(L("Salve stays unmade."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killKeparis", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFlesh", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Seven cuts. Salve batch tonight."));
					character.Inventory.Remove(650776, character.Inventory.CountItem(650776), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salve works on early petrification. Late stone resists."));
			}
		});

		// Quest 3: White Spion Essence
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Aether-Scholar] Birute-IV"), "f_tableland_74", 1250, 1250, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1003);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("White Spion Mages distill aether on the ridge. Kill eighteen, bring six essence-vials."));

				var response = await dialog.Select(L("Vials?"),
					Option(L("I'll bring"), "help"),
					Option(L("Aether?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't uncork them."));
						break;

					case "info":
						await dialog.Msg(L("Aether-essence drives counter-curse foci. Six vials, one foci-set."));
						break;

					case "leave":
						await dialog.Msg(L("Foci stay theory."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpions", out var killObj)) return;
				if (!quest.TryGetProgress("gatherEssence", out var eObj)) return;

				if (killObj.Done && eObj.Done)
				{
					await dialog.Msg(L("Six vials. Foci-set forged tonight."));
					character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Foci hum clean. Field-test next moon."));
			}
		});

		// Quest 4: Upper Terrace Kill
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Caravan-Master] Jurgita"), "f_tableland_74", -1650, -400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1004);

			dialog.SetTitle(L("Jurgita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A caravan-master inspects an iron wheel-rim by candle-light, finger-tracing a hairline crack she doesn't like the look of*{/}"));
				await dialog.Msg(L("My salt-run goes from the salt-flats to the upper-terrace camp every fortnight. Three wagons, six drovers, twelve oxen. The route's the same one my father drove and his father before him."));
				await dialog.Msg(L("Tiny Brown Mages drift the upper terrace and erase the wheel-rut chalk every time they pass through. The drovers navigate by the ruts - they don't have time to re-survey the route every trip. No ruts, no salt-run. No salt-run, no bread on the upper terrace by week's end."));

				var response = await dialog.Select(L("Kill 18 Tiny Brown Mages to push them off the terrace, then chalk-mark the four faded salt-cart ruts. The drovers can't navigate by guess. Will you take the run?"),
					Option(L("I'll thin the mages and re-mark the ruts"), "help"),
					Option(L("Why is salt so important?"), "info"),
					Option(L("Find a wheelwright"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She presses a stub of bright-yellow caravan-chalk into your palm*{/}"));
						await dialog.Msg(L("Eighteen Mages, no shortcuts. They drift in clusters of three, cast charms that disorient at range. Close the distance fast; charms don't work inside a blade-length."));
						await dialog.Msg(L("Then four ruts. Wagon-tracks faded along the salt-route. Yellow chalk because the upper-terrace wind doesn't strip yellow as fast as white. Press hard - the drovers need to read it from a wagon-bench at speed."));
						break;

					case "info":
						await dialog.Msg(L("Salt cures meat for the winter, preserves cheese, keeps the well-water sweet, ferments the cabbage. The upper-terrace camp goes through a barrel of salt every two days. Without the salt-run, they slaughter their preserved-stores within a week."));
						await dialog.Msg(L("Then they slaughter the herds. Then they slaughter the breeding-stock. Then they go home. The camp dies from the salt-shortage upward, never from the monsters down."));
						break;

					case "leave":
						await dialog.Msg(L("Wagons stay parked, the camp slaughters its preserved-stores, the upper terrace empties for the winter. I will hire a wheelwright to fix this rim and walk the salt-route with my drovers myself if I must. I would rather not."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMagesUpper", out var killObj)) return;
				if (!quest.TryGetProgress("markRuts", out var rObj)) return;

				if (killObj.Done && rObj.Done)
				{
					await dialog.Msg(L("{#666666}*She watches her drovers harness the lead ox-team, the wheels squealing forward against the rut-chalk*{/}"));
					await dialog.Msg(L("Wheels rolling, ruts re-chalked yellow. The drovers can read the line from the wagon-bench again. Salt at the upper-terrace camp by tomorrow noon."));
					await dialog.Msg(L("Caravan-master's purse, plus a salt-brick from the first cart - tradition for whoever clears the route. Boil it down and trade it; salt-brick from the upper-terrace fetches double in Klaipeda."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Eighteen Mages first. Re-marking ruts while Mages still drift the terrace just gives them fresh chalk to disorient over."));
				}
				else
				{
					await dialog.Msg(L("Mages are cleared. Now the four ruts - yellow chalk, press hard, drovers read at speed. The salt-cart leaves at first light."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salt at the camp on time. The cooks salted the winter-meat the same evening; the cheese rounds went into brine the next morning. The upper-terrace winter is provisioned because of you. The cooks don't know your name; the camp will eat well anyway."));
			}
		});

		// Salt-cart rut chalk points for Quest 1004
		//-------------------------------------------------------------------------
		void AddSaltRut(int rutNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Faded Salt-Cart Rut"), "f_tableland_74", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_74", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A faded wheel-rut where the chalk has worn off the stone*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_74.Quest1004.Rut{rutNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already chalked white; the rut reads clear*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Re-marking rut..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_74.Quest1004.RutsMarked", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_74.Quest1004.RutsMarked", count);
					character.ServerMessage(LF("Ruts re-marked: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All ruts re-marked! Return to Caravan-Master Jurgita.{/}"));
				}
				else
				{
					character.ServerMessage(L("Marking interrupted."));
				}
			});
		}

		AddSaltRut(1, -1500, -300, 0);
		AddSaltRut(2, -1800, -500, 90);
		AddSaltRut(3, -1400, -600, 180);
		AddSaltRut(4, -1700, -200, 270);

		// Quest 5: The Blue Harugal Elite
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Bounty-Sergeant] Tomas-II"), "f_tableland_74", 1000, -130, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1005);
			var eliteSpawnedKey = "Laima.Quests.f_tableland_74.Quest1005.EliteSpawned";

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty-sergeant unrolls a faded warrior-codex from a leather case, flattening the page open with two stones*{/}"));
				await dialog.Msg(L("Blue Harugal Elite leads the pack on the steel heights. Old, scarred, raised on the Harugal warrior-code - the one in this codex. He'll answer a formal challenge but won't fight a hunter who hasn't issued one."));
				await dialog.Msg(L("Three iron-shod challenge-stakes, planted outside his den, are the formal challenge. He's bound by his own code to answer. Then we kill ten of his pack to confirm the challenge has teeth, and he comes out for the duel."));

				var response = await dialog.Select(L("Plant the three challenge-stakes outside the Elite's den, then kill 10 Blue Harugals to confirm the challenge. He fights when his code obliges him. Will you take him?"),
					Option(L("I'll plant the stakes and take the duel"), "help"),
					Option(L("Why follow his code?"), "info"),
					Option(L("Find a code-trained hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you three iron-shod stakes wrapped in oilcloth and a folded translation of the challenge-rite*{/}"));
						await dialog.Msg(L("Three stakes, three drives. Hammer each one to half-depth at the den-mouth, in a triangle around the entrance. Recite the challenge-rite from the paper - even broken Harugal carries the meaning."));
						await dialog.Msg(L("Then ten of his pack. The Elite emerges around the eighth, fully armoured because the challenge requires formal dress. The duel is one-on-one; the surviving Harugals will not interfere."));
						break;

					case "info":
						await dialog.Msg(L("Without the challenge, ambushing him is dishonourable - and the pack avenges dishonourable kills with three generations of vendetta. The militia learned that in the demon war; we lost twelve good hands before someone read the codex."));
						await dialog.Msg(L("With the challenge, the duel is sealed. Pack scatters whether you win or lose - though it's better for everyone involved if you win. The Harugals respect lawful kills the way some humans respect a clean inheritance."));
						break;

					case "leave":
						await dialog.Msg(L("Pack stays bold, the heights stay contested, the militia loses three more hands to Harugal vendetta this season. I will read the codex to anyone willing - it just has to be someone willing. I'll keep waiting."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("plantStakes", out var sObj)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killElite", out var eObj)) return;

				if (sObj.Done && pObj.Done && eObj.Done)
				{
					await dialog.Msg(L("{#666666}*He rolls the codex closed, marks the date in its margin, and slides a heavy coin pouch across the rock-table*{/}"));
					await dialog.Msg(L("Stakes planted, challenge sealed, Elite dropped in formal duel. The pack scattered - I watched them go, no signs of vendetta-formation. The codex held."));
					await dialog.Msg(L("Bounty plus a code-stipend. The militia council voted on the stipend after the demon-war losses; you're the first lawful Harugal-kill since they passed it. Drink the first cup for the Elite. He fought like the warrior the codex describes."));
					character.Variables.Perm.Remove(eliteSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !eObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(eliteSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(eliteSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Harugal_Blue, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Blue Harugal Elite emerges from his den!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else if (!sObj.Done)
				{
					await dialog.Msg(L("Three stakes first. Half-depth, triangle around the den-mouth, recite the rite. The Elite won't fight without the formal challenge sealed."));
				}
				else
				{
					await dialog.Msg(L("Stakes are sealed, the challenge stands. Now ten of his pack - confirms the challenge has weight. The Elite emerges around the eighth, formally armoured."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Plateau quiet at last. No vendetta-cycle started; the Harugal pack relocated south of the steel heights without further loss to the militia. Three new patrol-hands signed up this week - they wouldn't have, before. The codex sits open on my rock-table; I read a different page each evening."));
			}
		});

		// Harugal challenge-stake plant points for Quest 1005
		//-------------------------------------------------------------------------
		void AddChallengeStake(int stakeNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Challenge-Stake"), "f_tableland_74", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_74", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An iron-shod challenge-stake waiting at the den-mouth*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_74.Quest1005.Stake{stakeNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already driven into the dirt; the head reads bright*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Driving stake..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_74.Quest1005.StakesPlanted", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_74.Quest1005.StakesPlanted", count);
					character.ServerMessage(LF("Challenge-stakes planted: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All stakes planted! Now bait out the Elite.{/}"));
				}
				else
				{
					character.ServerMessage(L("Driving interrupted."));
				}
			});
		}

		AddChallengeStake(1, 900, -200, 0);
		AddChallengeStake(2, 1100, 0, 90);
		AddChallengeStake(3, 800, -300, 180);

		// Quest 6: Steel Heights Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Saulius"), "f_tableland_74", 200, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1006);

			dialog.SetTitle(L("Saulius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A militia-captain spreads a warrant-ledger across a table-rock, weighting the corners with militia-stamped coins*{/}"));
				await dialog.Msg(L("Steel heights are the highest contested ground on the upper terrace. Three species share the contest - Tiny Brown Mages on the open ridges, Purple Keparis in the brush-thickets, Blue Hohen Gulaks in the cave-mouths."));
				await dialog.Msg(L("My warrant-ledger pays per cleared sweep. The Heights Cairn at the camp edge records each sweep in chalk; the ledger pays only against verified cairn-marks. It's how the militia council keeps the contractor-pay honest."));

				var response = await dialog.Select(L("Kill 12 Tiny Brown Mages, 12 Purple Keparis, and 12 Blue Hohen Gulaks, then chalk a tally on the Heights Cairn at the camp edge. Will you take the contract?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("How does the verification work?"), "info"),
					Option(L("Find a closer hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a stub of bright blue chalk and a warrant-token cast in militia bronze*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. The patrol-walkers count the corpse-piles on Sunday's perimeter."));
						await dialog.Msg(L("Heights Cairn is at the camp edge, beside the supply-tent. Chalk a single bracket-mark - one stroke down, one stroke across. Blue chalk because the Heights wind strips white inside a day."));
						break;

					case "info":
						await dialog.Msg(L("The patrol-walkers verify the kill-count Sunday morning. The cairn-mark records the contractor's claim Saturday evening. If the two match, the warrant pays out at noon. If they don't, the warrant pays the lower number and the contractor gets one warning."));
						await dialog.Msg(L("Two warnings and the contractor's blacklisted from militia work for a year. Three and they're blacklisted from Salvia warrants entirely. Old council rule. We don't play games with the warrant-coin."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau stays wild, the warrant goes unpaid, the steel heights eventually empty out. I'll find a closer hand if I have to recruit from the patrol-walkers, which means thinning my own perimeter for the duration."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var mObj)) return;
				if (!quest.TryGetProgress("killKeparis", out var kObj)) return;
				if (!quest.TryGetProgress("killGulaks", out var gObj)) return;
				if (!quest.TryGetProgress("chalkCairn", out var cObj)) return;

				if (mObj.Done && kObj.Done && gObj.Done && cObj.Done)
				{
					await dialog.Msg(L("{#666666}*He countersigns the warrant in militia ink, then weighs out the coin onto the table-rock*{/}"));
					await dialog.Msg(L("Cairn-mark verified, ledger holds. Warrant pays in full - no warnings, no questions. The patrol-walkers reported a clean perimeter at sun-up."));
					await dialog.Msg(L("Coin in full plus a half-purse for the bracket-mark form. Some contractors chalk a chevron because they think it looks better; the council doesn't pay for chevrons. Brackets only. You knew."));
					character.Quests.Complete(questId);
				}
				else if (mObj.Done && kObj.Done && gObj.Done)
				{
					await dialog.Msg(L("Sweep done. Now the Heights Cairn - bracket-mark, blue chalk, no flourishes. Patrol-walkers verify Sunday morning."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each species, no shortcuts. The Gulaks in the cave-mouths are the hardest count - they retreat into deep dark, you have to go in after them. Bring a torch."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Militia patrols the heights now, paid by your sweep-warrant. Three new patrol-hands signed on after they read the council's notice about contractors who get the bracket-mark right. Some of them think you're a militia-trained hand who slipped the rolls. I don't tell them otherwise."));
			}
		});

		// Heights Cairn for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Heights Cairn"), "f_tableland_74", 250, 1050, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A camp-edge cairn, chalked with old militia-warrant tallies*{/}"));
				return;
			}

			var chalkedKey = "Laima.Quests.f_tableland_74.Quest1006.CairnChalked";
			if (character.Variables.Perm.GetBool(chalkedKey, false))
			{
				await dialog.Msg(L("{#666666}*Your tally chalked already*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killMages", out var mObj)) return;
			if (!quest.TryGetProgress("killKeparis", out var kObj)) return;
			if (!quest.TryGetProgress("killGulaks", out var gObj)) return;

			if (!(mObj.Done && kObj.Done && gObj.Done))
			{
				await dialog.Msg(L("{#666666}*Cairn's ready, but the sweep isn't done*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Chalking cairn..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(chalkedKey, true);
				character.ServerMessage(L("{#FFD700}Heights Cairn chalked. Return to Militia-Captain Saulius.{/}"));
			}
			else
			{
				character.ServerMessage(L("Chalking interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland74Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1001);
		SetName(L("Tiny Mage Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tiny Brown Mages choking the lower terrace path."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Plateau-Watch] Mindaugas"), "f_tableland_74");

		AddObjective("killMages", L("Kill Tiny Brown Mages"),
			new KillObjective(40, new[] { MonsterId.Tiny_Mage_Brown }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland74Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1002);
		SetName(L("Kepari Flesh-Cuts"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Keparis and bring flesh-cuts for the alchemist."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Ruta"), "f_tableland_74");

		AddObjective("killKeparis", L("Kill Purple Keparis"),
			new KillObjective(25, new[] { MonsterId.Kepari_Purple }));

		AddObjective("gatherFlesh", L("Gather flesh-cuts"),
			new CollectItemObjective(650776, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650776, character.Inventory.CountItem(650776), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650776, character.Inventory.CountItem(650776), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland74Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1003);
		SetName(L("White Spion Essence"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill White Spion Mages and bring essence-vials for foci-work."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Aether-Scholar] Birute"), "f_tableland_74");

		AddObjective("killSpions", L("Kill White Spion Mages"),
			new KillObjective(18, new[] { MonsterId.Spion_Mage_White }));

		AddObjective("gatherEssence", L("Gather essence-vials"),
			new CollectItemObjective(663131, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland74Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1004);
		SetName(L("Upper Terrace Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tiny Brown Mages on the upper terrace to clear the caravan route."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan-Master] Jurgita"), "f_tableland_74");

		AddObjective("killMagesUpper", L("Kill Tiny Brown Mages"),
			new KillObjective(18, new[] { MonsterId.Tiny_Mage_Brown }));

		AddObjective("markRuts", L("Re-mark the four faded salt-cart ruts"),
			new VariableCheckObjective("Laima.Quests.f_tableland_74.Quest1004.RutsMarked", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1004.RutsMarked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_74.Quest1004.Rut{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1004.RutsMarked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_74.Quest1004.Rut{i}");
	}
}

public class FTableland74Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1005);
		SetName(L("The Blue Harugal Elite"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Harugals to draw out the Elite leading the pack."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty-Sergeant] Tomas"), "f_tableland_74");

		AddObjective("plantStakes", L("Plant the three challenge-stakes"),
			new VariableCheckObjective("Laima.Quests.f_tableland_74.Quest1005.StakesPlanted", 3, true));

		AddObjective("killPack", L("Kill Blue Harugals"),
			new KillObjective(10, new[] { MonsterId.Harugal_Blue }));

		AddObjective("killElite", L("Defeat the Blue Harugal Elite"),
			new KillObjective(1, new[] { MonsterId.Harugal_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1005.StakesPlanted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_74.Quest1005.Stake{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1005.StakesPlanted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_74.Quest1005.Stake{i}");
	}
}

public class FTableland74Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1006);
		SetName(L("Steel Heights Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Tiny Brown Mages, Purple Keparis, and Blue Hohen Gulaks."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Saulius"), "f_tableland_74");

		AddObjective("killMages", L("Kill Tiny Brown Mages"),
			new KillObjective(12, new[] { MonsterId.Tiny_Mage_Brown }));

		AddObjective("killKeparis", L("Kill Purple Keparis"),
			new KillObjective(12, new[] { MonsterId.Kepari_Purple }));

		AddObjective("killGulaks", L("Kill Blue Hohen Gulaks"),
			new KillObjective(12, new[] { MonsterId.Hohen_Gulak_Blue }));

		AddObjective("chalkCairn", L("Chalk a tally on the Heights Cairn"),
			new VariableCheckObjective("Laima.Quests.f_tableland_74.Quest1006.CairnChalked", 1, true));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1006.CairnChalked");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1006.CairnChalked");
	}
}
