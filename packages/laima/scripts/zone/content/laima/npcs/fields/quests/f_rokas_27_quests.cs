//--- Melia Script ----------------------------------------------------------
// Akmens Ridge - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_rokas_27. Dry ridge country bridging Tenet
// Garden and Overlong Bridge Valley, scattered with ancient tomb-stones
// and patrolled by rider-captains tracking lost reliquaries.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas27QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Ambushers in the Pass
		// =====================================================================
		// Pass-Scout Arnas - Tucen ambushing lone riders along the ridge pass
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Pass-Scout] Arnas"), "f_rokas_27", 840, -700, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1001);

			dialog.SetTitle(L("Arnas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A broad-chested scout in dust-caked leathers shades his eyes against the ridge-glare, one hand resting on a crossbow stock*{/}"));
				await dialog.Msg(L("Two riders down this week. Tucen don't usually work this high up the ridge - something's pushed them out of the lower washes."));

				var response = await dialog.Select(L("Twenty Tucen put down. Hit the center pass first - that's where they bottleneck a rider. The rest scatter back to the washes once their cluster breaks."),
					Option(L("I'll thin twenty Tucen"), "help"),
					Option(L("What pushed them up here?"), "info"),
					Option(L("Riders chose this work"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He pulls a short whistle from his collar*{/}"));

						if (await dialog.YesNo(L("Twenty Tucen. The center pass - look for the rockfall markers. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If you hear two sharp whistles - that's me, rider incoming. Clear the pass-center so they don't ride into your fight."));
							await dialog.Msg(L("Tucen pincers don't break armor, but they pop grease-seals. Oil your gear before you engage."));
						}
						break;

					case "info":
						await dialog.Msg(L("Sauga scorpions pushed them. And the Sauga only push when something else pushes them - Tomb Lord may be stirring in the deep-ridge. Or worse."));
						await dialog.Msg(L("I send reports to Fedimian through Rider-Captain Indre. Whether they get read - that's above my pay."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. Ride-couriers know the risk. Doesn't make me sleep better when two of them don't come back."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTucen", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lowers the crossbow and lets out a long breath*{/}"));
					await dialog.Msg(L("Twenty. The center pass runs clean for a day or three. I can wave the next rider-string through without holding my breath."));
					await dialog.Msg(L("Take this - scout's pay, and a grease-vial from my own kit. Seal your gear before you ride down-ridge."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Center pass. Rockfall markers. Oil your gear first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The pass stayed clear long enough for three rider-strings to cross. That's three more dispatches Fedimian actually gets."));
			}
		});

		// =====================================================================
		// QUEST 1002: Rider-Captain's Relay
		// =====================================================================
		// Rider-Captain Indre - Patrol orders to Bridge-Watcher Kaspar
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Rider-Captain] Indre"), "f_rokas_27", -150, -3600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1002);

			dialog.SetTitle(L("Indre"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A lean captain in a leather patrol-coat holds a folded dispatch against the wind, one boot propped on a tether-post*{/}"));
				await dialog.Msg(L("My last relay-rider came back with a crossbow bolt through her saddle and no dispatch delivered. The Overlong Bridge patrol needs these orders by tonight or they'll hold position when they should advance."));

				var response = await dialog.Select(L("Carry this dispatch to Bridge-Watcher Kaspar at the Overlong Bridge warp. Don't break the seal. Bring his countersigned reply back - the patrol's marching orders depend on it."),
					Option(L("I'll carry the dispatch"), "help"),
					Option(L("Who shot your rider?"), "info"),
					Option(L("Find another courier"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She presses the dispatch into your hand with both of hers, then tightens the tether on her own mount*{/}"));

						if (await dialog.YesNo(L("The Overlong Bridge warp is northeast of the pass. Kaspar keeps a stone bridgehead shelter. Will you go?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The rider-seal is horse-and-hawk. Don't press it - the wax is soft. Kaspar will countersign with bridge-pitch."));
							await dialog.Msg(L("If you hear a whistle-pattern from the pass-scout - three long, one short - that's 'rider-down nearby.' Ride around it, don't toward it."));
						}
						break;

					case "info":
						await dialog.Msg(L("Bandit-types with Overlong Bridge bolt-patterns. I've been asking Kaspar for two weeks if his side has a security problem. Haven't gotten a straight answer."));
						await dialog.Msg(L("That's part of why I'm sending you and not another rider. If Kaspar's compromised, I'd rather lose a dispatch than a second rider."));
						break;

					case "leave":
						await dialog.Msg(L("Then I hold the dispatch until morning and hope the patrol figures it out without orders. Which they won't."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverDispatch", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She cracks the bridge-pitch seal with her thumbnail, scans Kaspar's reply, and lets out a short breath*{/}"));
					await dialog.Msg(L("Pitch seal, clean countersign. Bridge-Watcher is who he says he is. Good - that's one thing off my list."));
					await dialog.Msg(L("{#666666}*She tucks the reply into her coat and hands you a rider's purse*{/}"));
					await dialog.Msg(L("Rider's pay. And an honest warning: the bolt-pattern that killed Irma is someone working both sides of the warp. I haven't caught them yet."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Overlong Bridge warp, northeast. Kaspar. Bridge-pitch seal on the countersign."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The patrol marched on schedule. Whoever shot Irma hasn't shown themselves again. Yet."));
			}
		});

		// =====================================================================
		// BRIDGE-WATCHER KASPAR (Quest 1002 recipient)
		// =====================================================================
		AddNpc(20117, L("[Bridge-Watcher] Kaspar"), "f_rokas_27", 340, -620, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1002);

			dialog.SetTitle(L("Kaspar"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_27.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A stone-faced watcher turns the sealed dispatch over in his hands, examining the horse-and-hawk mark without breaking it*{/}"));
					await dialog.Msg(L("Indre's seal. Unbroken. Good."));
					await dialog.Msg(L("{#666666}*He cracks the wax, reads the dispatch twice, then draws a short stick of bridge-pitch from a belt-pouch*{/}"));
					await dialog.Msg(L("She's asking if my patrol can cover the south arch through next week. Answer is yes, if her riders can hold the pass. Tell her the bolt-pattern that got Irma matches a crate of stolen stock from the Overlong armory."));
					await dialog.Msg(L("{#666666}*He warms the pitch-stick against a bridgehead lamp and presses it into the reply, countersigning with a thumbprint*{/}"));
					await dialog.Msg(L("Pitch seal, my print. That tells her the countersign came from me, not from whoever took my stock."));

					character.Variables.Perm.Set("Laima.Quests.f_rokas_27.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Kaspar's countersigned reply received. Return to Rider-Captain Indre.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the pitch-sealed reply to Indre. She'll know the thumbprint."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Indre's patrol's covering the south arch. I'm chasing the stolen bolt-stock before it kills another rider."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*The watcher adjusts a sighting-lens on a bridgehead stand, eyes on the pass*{/}"));
				await dialog.Msg(L("Bridge is held. Patrols rotate at sundown. If you've no dispatch, mind the pass-markers on your way through."));
			}
		});

		// =====================================================================
		// QUEST 1003: The Scattered Reliquaries
		// =====================================================================
		// Relic-Sweeper Aldona - Recovering reliquaries lost in the ridge crossing
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Relic-Sweeper] Aldona"), "f_rokas_27", 950, -700, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1003);

			dialog.SetTitle(L("Aldona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A grey-haired sweeper kneels beside a ridge-stone, sifting dust through a horsehair sieve with practiced fingers*{/}"));
				await dialog.Msg(L("Fedimian's reliquary-list came through last week. Seventeen missing; four matched at the bridgehead. The other ten? Scattered across this ridge when the pilgrim-caravan ran from a Tucen pack."));

				var response = await dialog.Select(L("Eight reliquary fragments. The ridge-stones catch them - dust blows, stones stay. Sweep at the base of any standing stone with a ridge-shadow. Will you help gather?"),
					Option(L("I'll gather eight fragments"), "help"),
					Option(L("Why at the stone-bases?"), "info"),
					Option(L("Sweeping is your trade"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She pulls a second sieve from her gather-pack and passes it up*{/}"));

						if (await dialog.YesNo(L("Sieve the dust at each stone-base. Fragments look like chapel silver, palm-sized or smaller. Eight is enough to make a recovery report worth sending. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wind blows pilgrim-dust east in this ridge. Stones on the east face catch more. Start there."));
							await dialog.Msg(L("And if a Sauga scorpion wakes while you're sweeping - step back, don't strike. They only lunge at movement toward them."));
						}
						break;

					case "info":
						await dialog.Msg(L("Wind catches soft things. Reliquary-silver is soft. Stones cast shadow-pockets where the wind-current slows. So: fragments fall behind stones, not in the open pass."));
						await dialog.Msg(L("I've swept this ridge for eleven years. I know where the shadows are."));
						break;

					case "leave":
						await dialog.Msg(L("True enough. I'll sweep alone tomorrow. Eleven years, I'm slow but thorough."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var fragmentCount = character.Inventory.CountItem(650735);

				if (fragmentCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She weighs each fragment in her palm, cross-checking against a worn reliquary-list*{/}"));
					await dialog.Msg(L("All chapel-silver. No forgeries. Two match entries on the Tenet list - I'll route those through Emilis. The other six go to Fedimian on the next rider."));
					await dialog.Msg(L("{#666666}*She hands you a small leather pouch and a polished ridge-stone*{/}"));
					await dialog.Msg(L("Sweeper's share. And the stone - old tradition; a gatherer's thanks for a gatherer's work. Keep it on your gear-strap."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East stone-faces. Sieve at the base. Eight fragments."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Six fragments reached Fedimian. Two went back to Mindaugas at the chapel. Ten years I've been chasing that list - closest it's been to cleared."));
			}
		});

		// =====================================================================
		// RELIQUARY FRAGMENT SPOTS
		// =====================================================================
		// For Quest 1003 - The Scattered Reliquaries
		// =====================================================================

		void AddFragmentSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Ridge-Stone Base"), "f_rokas_27", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_rokas_27", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered ridge-stone casts a shadow-pocket at its base. Wind-blown dust has caught here*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_rokas_27.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*This stone-base is already swept clean*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_rokas_27.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 18)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Sauga_S, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Sauga scorpion stirs from the stone's shadow!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Sieving the stone-base..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(4));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650735, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650735);
					character.ServerMessage(LF("Reliquary fragments recovered: {0}/8", currentCount));

					if (currentCount >= 8)
					{
						character.ServerMessage(L("{#FFD700}All fragments recovered! Return to Relic-Sweeper Aldona.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Sweep interrupted."));
				}
			});
		}

		AddFragmentSpot(1, 328, -2523, 0);
		AddFragmentSpot(2, 782, -2293, 90);
		AddFragmentSpot(3, 1110, -1049, 180);
		AddFragmentSpot(4, 1364, 108, 270);
		AddFragmentSpot(5, 1948, -134, 0);
		AddFragmentSpot(6, 1725, -298, 90);
		AddFragmentSpot(7, 1165, 706, 180);
		AddFragmentSpot(8, -550, -1778, 270);

		// =====================================================================
		// QUEST 1004: The Ridge Tombs Are Stirring
		// =====================================================================
		// Tomb-Scholar Vytenis - Checking which tomb-seals have failed
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Tomb-Scholar] Vytenis"), "f_rokas_27", 1440, 400, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1004);

			dialog.SetTitle(L("Vytenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young scholar in road-worn academic grey unrolls a ridge-survey across his knee, weighting the corners with tomb-dust-stained stones*{/}"));
				await dialog.Msg(L("The Tomb Lord's dormancy was supposed to hold another two centuries. It isn't. Something woke him, and the ridge's lesser tombs may already be emptying in sympathy."));

				var response = await dialog.Select(L("Four lesser tomb-stones cluster on the upper ridge. Check each seal. Intact, cracked, or broken - each tells me how far the waking has spread. Will you walk them for me?"),
					Option(L("I'll walk the four tombs"), "help"),
					Option(L("What woke the Tomb Lord?"), "info"),
					Option(L("Tomb-scholarship is dangerous"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He tears a page from his survey-book and marks four points in ridge-chalk*{/}"));

						if (await dialog.YesNo(L("The tombs are scattered across the upper ridge. Read each seal - describe the stone-state. Do not disturb the seal-lines. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The seal-lines are chalk-and-pitch, renewed every half-century. Fresh chalk is pale; old chalk yellows. Pitch-break means the line was pulled apart, not worn down."));
							await dialog.Msg(L("Pulled-apart pitch is the one that means something climbed out. Not something got in - something got out."));
						}
						break;

					case "info":
						await dialog.Msg(L("I don't know. The Fedimian wardmage went silent before he could answer. A pilgrim-archivist from the Thaumas Trail sent word that his seals were drained from the inside - that's the same pattern Tomb Lord tradition describes for an awakening consumed by an outside working."));
						await dialog.Msg(L("Translation: something far away is eating the ridge-wards by reaching through other seals. The Tomb Lord is downstream of that."));
						break;

					case "leave":
						await dialog.Msg(L("True. I'd walk them myself, but the Loftlem cluster guards the upper ridge more aggressively than my travel-budget accounts for."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var tombsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_27.Quest1004.TombsChecked", 0);

				if (tombsChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*He listens, writing rapidly, then sets the pen down at the last entry*{/}"));
					await dialog.Msg(L("Warrior-King: seal intact, chalk fresh. Priestess: cracked chalk, pitch still bridging. Keeper: pulled-apart pitch, chalk scattered. Unnamed: pulled-apart pitch, chalk scattered, and a chalk-handprint inside the seal-line."));
					await dialog.Msg(L("{#666666}*His jaw works*{/}"));
					await dialog.Msg(L("Two pulled apart. And a handprint inside the line - which means whatever climbed out paused long enough to leave a signature. That's not mindless waking. That's intent."));
					await dialog.Msg(L("Take this - a scholar's stipend, every coin I can spare. I ride to Fedimian tonight. Someone has to carry this finding past the silent wardmage."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four tombs. Chalk state, pitch state. Handprints or other marks. Walk, read, report."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My report reached the Fedimian magistrate-archivist. A replacement wardmage was dispatched yesterday. Whether he arrives before the third tomb opens - that's the question."));
			}
		});

		// =====================================================================
		// RIDGE TOMB-STONES
		// =====================================================================
		// For Quest 1004 - The Ridge Tombs Are Stirring
		// =====================================================================

		void AddRidgeTomb(int tombNumber, string tombName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(tombName), "f_rokas_27", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_rokas_27", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered tomb-stone on the upper ridge. The chalk-and-pitch seal-line is faded but present*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_rokas_27.Quest1004.Tomb{tombNumber}";
				var checkedTomb = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedTomb)
				{
					await dialog.Msg(L("{#666666}*You have already read this seal*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the seal-line..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var tombsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_27.Quest1004.TombsChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_rokas_27.Quest1004.TombsChecked", tombsChecked + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Tombs read: {0}/4", tombsChecked + 1));

					if (tombsChecked + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All tombs read! Return to Tomb-Scholar Vytenis.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddRidgeTomb(1, "Warrior-King Tomb", "Warrior-King: chalk fresh, pitch intact. Seal holds.", 1434, 403, 0);
		AddRidgeTomb(2, "Priestess Tomb", "Priestess: chalk cracked but continuous. Pitch bridges the fractures. Seal weakened, not broken.", 1683, 588, 90);
		AddRidgeTomb(3, "Keeper Tomb", "Keeper: pitch pulled apart at the western arc. Chalk scattered downhill. Something climbed out.", 1270, 270, 180);
		AddRidgeTomb(4, "Unnamed Ridge-Tomb", "Unnamed: pitch pulled apart, chalk scattered. A chalk-handprint pressed flat inside the seal-line - deliberate, not accidental.", 1551, 227, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Ambushers in the Pass
//-----------------------------------------------------------------------------

public class AmbushersInThePassQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1001);
		SetName("Ambushers in the Pass");
		SetType(QuestType.Sub);
		SetDescription("Pass-Scout Arnas needs twenty Tucen thinned before the ridge pass can stay open for rider-strings.");
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Pass-Scout] Arnas", "f_rokas_27");

		AddObjective("killTucen", "Defeat Tucen in the ridge pass",
			new KillObjective(20, new[] { MonsterId.Tucen }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));  // Lv6 EXP Card
		AddReward(new ItemReward(640004, 15)); // Large HP Potion
		AddReward(new ItemReward(640007, 15)); // Large SP Potion
		AddReward(new ItemReward(640013, 13));  // Recovery Potion
	}
}

// Quest 1002 CLASS: Rider-Captain's Relay
//-----------------------------------------------------------------------------

public class RiderCaptainsRelayQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1002);
		SetName("Rider-Captain's Relay");
		SetType(QuestType.Sub);
		SetDescription("Rider-Captain Indre's patrol dispatch must reach Bridge-Watcher Kaspar at the Overlong Bridge warp and return countersigned.");
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Rider-Captain] Indre", "f_rokas_27");

		AddObjective("deliverDispatch", "Deliver the dispatch and return countersigned",
			new VariableCheckObjective("Laima.Quests.f_rokas_27.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 15)); // Large HP Potion
		AddReward(new ItemReward(640007, 15)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_27.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_27.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: The Scattered Reliquaries
//-----------------------------------------------------------------------------

public class TheScatteredReliquariesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1003);
		SetName("The Scattered Reliquaries");
		SetType(QuestType.Sub);
		SetDescription("Relic-Sweeper Aldona needs eight reliquary fragments recovered from the ridge stone-bases.");
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Relic-Sweeper] Aldona", "f_rokas_27");

		AddObjective("collectFragments", "Recover reliquary fragments from ridge-stones",
			new CollectItemObjective(650735, 8));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 14)); // Large HP Potion
		AddReward(new ItemReward(640007, 14)); // Large SP Potion
		AddReward(new ItemReward(640013, 13));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650735,
			character.Inventory.CountItem(650735),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650735,
			character.Inventory.CountItem(650735),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Ridge Tombs Are Stirring
//-----------------------------------------------------------------------------

public class TheRidgeTombsAreStirringQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1004);
		SetName("The Ridge Tombs Are Stirring");
		SetType(QuestType.Sub);
		SetDescription("Tomb-Scholar Vytenis has asked you to read the four lesser tomb-seals on the upper ridge and determine how far the Tomb Lord's awakening has spread.");
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Tomb-Scholar] Vytenis", "f_rokas_27");

		AddObjective("readTombs", "Read the four ridge-tomb seal-lines",
			new VariableCheckObjective("Laima.Quests.f_rokas_27.Quest1004.TombsChecked", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 15)); // Large HP Potion
		AddReward(new ItemReward(640007, 15)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_27.Quest1004.TombsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1004.Tomb{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_27.Quest1004.TombsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1004.Tomb{i}");
		}
	}
}
