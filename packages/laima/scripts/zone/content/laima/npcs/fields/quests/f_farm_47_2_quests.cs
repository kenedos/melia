//--- Melia Script ----------------------------------------------------------
// Aqueduct Bridge Area - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_farm_47_2 map. Front-line farm next to the
// Demon Prison District 1 warp portal.
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

public class FFarm472QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Aqueduct Siege
		// =====================================================================
		// Farmer Tadas - Kepari Shamans assaulting the aqueduct
		//---------------------------------------------------------------------
		AddNpc(20128, L("[Farmer] Tadas"), "f_farm_47_2", 1383, 1005, 89, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1001);

			dialog.SetTitle(L("Tadas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer in patched armor is gripping a short spear, turning a scorched stone chip in his other hand*{/}"));
				await dialog.Msg(L("They're casting at the bridges now. Kepari Shamans — straight out of the demon prison. If one stone of that aqueduct cracks, four farms lose water before sundown."));

				var response = await dialog.Select(L("Two months ago I was a farmhand. Now I'm holding a spear and watching for sparks on the water. The magistrates won't send soldiers — they say 'the situation's being monitored.' I need them thinned. Fifteen shamans should be enough to stop the next spell."),
					Option(L("I'll kill the Shamans"), "help"),
					Option(L("Why are they attacking the water?"), "info"),
					Option(L("Sounds suicidal"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He exhales and lowers the spear*{/}"));

						character.Quests.Start(questId);

						if (!character.Variables.Perm.GetBool("Laima.Quests.f_farm_47_2.Quest1001.DaggerGiven", false))
						{
							await dialog.Msg(L("Hold on — take this knife before you go. Nothing fancy, just a kitchen blade my brother left behind, but it'll cut a Kepari better than a wooden spear-shaft. Better than nothing."));
							await dialog.Msg(L("{#666666}*He pulls a plain knife from his belt and hands it to you*{/}"));
							character.Inventory.Add(111001, 1, InventoryAddType.PickUp);
							character.Variables.Perm.Set("Laima.Quests.f_farm_47_2.Quest1001.DaggerGiven", true);
						}

						await dialog.Msg(L("Stay low when they cast. The lightning arcs straight — duck and it goes right over you."));
						break;

					case "info":
						await dialog.Msg(L("Ask them yourself, they don't answer. Lina figures they want the farms cleared out before something bigger comes through the portal."));
						await dialog.Msg(L("If the wells dry, we pack. If we pack, the demon prison has a clear road to Klaipeda. So we stay, and we fight."));
						break;

					case "leave":
						await dialog.Msg(L("Yeah, probably. Come back if you change your mind. Or don't — I'll be here either way."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killShaman", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to the aqueduct — water still running steady*{/}"));
					await dialog.Msg(L("Fifteen down. The bridges held. That buys us a week of water, maybe two, before they regroup."));
					await dialog.Msg(L("Take this — farmhand's purse, honest earnings. And keep the dagger. You'll need it more than I will."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Fifteen Shamans, near the portal warp. Stay low when the lightning flies."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Aqueduct still flows. Four farms still have water. Every day we hold is a day the villages don't have to pack up."));
			}
		});

		// =====================================================================
		// QUEST 1002: Refugee Protocol
		// =====================================================================
		// Farmer Audrone - Evacuation planning with Tenants' Farm
		//---------------------------------------------------------------------
		AddNpc(20161, L("[Farmer] Audrone"), "f_farm_47_2", -172, 1790, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1002);

			dialog.SetTitle(L("Audrone"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer is marking a map of the farm cluster with colored pins*{/}"));
				await dialog.Msg(L("If the portal widens tomorrow, I need to know which farm takes whose kids. Red pins for Myrkiti, blue for Shaton, green for here. Tenants' Farm has the most barns, so Myrkiti's families go there first."));

				var response = await dialog.Select(L("I drafted a refugee protocol — who goes where, what supplies each host farm needs, who rings the evacuation bell. Mykolas at Tenants' Farm needs a copy before he can plan the barn conversions. Can you carry it over to him?"),
					Option(L("I'll deliver it to Mykolas"), "help"),
					Option(L("Is it really that bad?"), "info"),
					Option(L("That's the magistrate's job"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She wraps the pin-marked map in oilcloth and hands it over*{/}"));

						character.Quests.Start(questId);
						character.Inventory.Add(650580, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("If Mykolas argues, tell him it's not negotiable. The plan has to be ready before the bell rings."));
						await dialog.Msg(L("Oh — and if you pass Grimaras near the portal, tell him the ward-survey is still waiting."));
						break;

					case "info":
						await dialog.Msg(L("The magistrates sent one Pyromancer last week. She sealed two cracks and went back to Klaipeda. The portal's still open."));
						await dialog.Msg(L("We plan for evacuation because the alternative is a panic, and a panic gets kids trampled in a barn doorway. A protocol doesn't."));
						break;

					case "leave":
						await dialog.Msg(L("The magistrate's already had the letter six weeks. They're 'reviewing procedural recommendations.' Meanwhile we farm and pray."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverProtocol", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She nods as you report Mykolas's reply*{/}"));
					await dialog.Msg(L("Seven barns, thirty pallets each. That's more space than I figured. Good — we might fit everyone."));
					await dialog.Msg(L("Here, take these. A farmer's thanks. If the bell rings tomorrow, the coin is the least we owe you."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Through the north warp. Mykolas — older, broad-shouldered. Give him the protocol."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The protocol's posted at every farmhouse now. The kids know which barn to run to. That's something."));
			}
		});

		// =====================================================================
		// Mykolas (recipient for Quest 1002) lives at f_farm_47_1

		// =====================================================================
		// QUEST 1003: Charged Dandel Pods
		// =====================================================================
		// Wardmaker-Farmer Lina - Ward-circles against demon-pollen
		//---------------------------------------------------------------------
		AddNpc(147419, L("[Wardmaker-Farmer] Lina"), "f_farm_47_2", 491, -56, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1003);

			dialog.SetTitle(L("Lina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer is winding copper wire around a knotty round pod, the husk crackling faintly*{/}"));
				await dialog.Msg(L("Three generations of wardmakers in my family. Grandma warded against frost, my mother against weevils, and I ward against demon-pollen. One charged Dandel pod is enough to keep a threshold clean for a whole month."));

				var response = await dialog.Select(L("I need six charged pods for a new ward-circle at the evacuation barn. The Dandels drift over the fields snapping at gnats, and every so often one drops a pod. The fresh ones still buzz a little — that's the ones I want. If they prickle when you touch them, they're ready. Wait too long and they go flat."),
					Option(L("I'll gather six pods"), "help"),
					Option(L("How do the wards work?"), "info"),
					Option(L("Another farmer task"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you a copper-lined pouch*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Pluck them by the husk, not the spine. The husk's tough, but the spine's where the charge lives and it bites. And if a Dandel comes circling overhead, don't swing at it — they hate the sound of a blade and they'll spit lightning at you. Just stand still until it loses interest."));
						break;

					case "info":
						await dialog.Msg(L("Copper coil, pod-charge, salt circle, threshold stone. You close the loop, bind the corners, then seal it with the charge at sunrise. That's the whole trick."));
						await dialog.Msg(L("Keeps the pollen out for about a month. Keeps most small demons out for three nights. Keeps a Kepari Shaman out for one angry spell, give or take. Nothing lasts forever."));
						break;

					case "leave":
						await dialog.Msg(L("Then the barn has no ward tonight, and the kids sleep under an unwarded roof. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var podCount = character.Inventory.CountItem(663355);

				if (podCount >= 6)
				{
					await dialog.Msg(L("{#666666}*She tests each pod with a copper pick — sparks jump neatly*{/}"));
					await dialog.Msg(L("All six charged. Plucked clean, no broken spines. You've got a careful hand."));
					await dialog.Msg(L("Here — a farmer's wage, plus my thanks. The evacuation barn'll have its ward by sundown."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Six pods, husk-pluck only. Drop them straight into the copper pouch — that's what holds the charge."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Evacuation barn's warded. If the bell rings, the kids sleep safe. That's what wards are for."));
			}
		});

		// =====================================================================
		// CHARGED DANDEL PODS
		// =====================================================================
		// For Quest 1003 - Charged Dandel Pods
		// =====================================================================

		void AddDandelPod(int podNumber, int x, int z, int direction)
		{
			AddNpc(153046, L("Charged Dandel Pod"), "f_farm_47_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_2", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A knotty pod sits low in the grass, husk crackling faintly with static. A Dandel must have shed it nearby.*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_2.Quest1003.Pod{podNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The husk has already been plucked. Another Dandel will shed a fresh pod here in a day or so.*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character,
					new[] { MonsterId.Dandel_Orange, MonsterId.Dandel_White, MonsterId.Dandel },
					150, 400);
				if (luredCount > 0)
					character.ServerMessage(L("{#FF6666}As you start plucking, the nearby Dandels turn on you.{/}"));

				var result = await character.TimeActions.StartAsync(L("Plucking pod..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(663355, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(663355);
					character.ServerMessage(LF("Charged pods gathered: {0}/6", currentCount));

					if (currentCount >= 6)
					{
						character.ServerMessage(L("{#FFD700}All pods gathered! Return to Lina.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Plucking interrupted."));
				}
			});
		}

		AddDandelPod(1, 332, -7, 45);
		AddDandelPod(2, 3, 378, 45);
		AddDandelPod(3, 384, 557, 45);
		AddDandelPod(4, 698, 460, 45);
		AddDandelPod(5, 202, 984, 45);
		AddDandelPod(6, 1382, 825, 45);
		AddDandelPod(7, 1468, 1294, 45);
		AddDandelPod(8, 249, 1220, 45);

		// =====================================================================
		// QUEST 1004: Broken Ward Stones
		// =====================================================================
		// Soldier Grimaras - Surveying the ancient warding circle at the portal
		//---------------------------------------------------------------------
		AddNpc(20125, L("[Soldier] Grimaras"), "f_farm_47_2", -1421, -1123, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1004);

			dialog.SetTitle(L("Grimaras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A soldier in field colors leans on his halberd, the warp-circle glowing behind him*{/}"));
				await dialog.Msg(L("Years ago the ground tore open in the middle of this field. No warning, no quake — one morning a hole into the demon prison was just there, and it's been there ever since. The Ancients rode out from Klaipeda within the week and laid four seals around it. Stone wards, each carved with a binding rune."));
				await dialog.Msg(L("The seals don't close the portal — they never could. What they do is keep what's inside the prison from spilling out. A man can walk through that gate if he's stupid enough; the seals don't stop anyone going in. Nothing comes back the other way. That's the whole point."));

				var response = await dialog.Select(L("The seals are holding, but stone weathers. Wind, frost, demon-pollen eating away at the carvings year by year. The wardmage in Fedimian wants to know how long before any of them give. Walk the circle, inspect each seal up close, note the wear — hairline cracks, runes worn shallow, edges crumbling. My orders keep me on the line, so I can't do it myself."),
					Option(L("I'll inspect the seals"), "help"),
					Option(L("What happens if I go in?"), "info"),
					Option(L("The portal's too close"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He hands you a brass caliper and a soldier's notebook from his pack, then grabs your wrist before you turn away*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Listen — stay on this side of the portal. The seals are a one-way binding. Anything trying to come out gets pinned, and that includes you if you cross. Step through that gate and you don't come back. Not by sword, not by spell, not by prayer."));
						await dialog.Msg(L("I've watched four people walk through that gate thinking they'd just take a quick look. None of them came back. So do me a favor — keep both feet outside the warp circle while you work. The seals are at arm's reach from the edge. You don't need to step inside to read them."));
						await dialog.Msg(L("Four seals around the warp point. Walk it counter-clockwise — old soldier's habit. Look close at each rune for pitting in the strokes or soft edges. If you start tasting metal, that's the pollen getting to you. Back off and count to ten."));
						await dialog.Msg(L("Finish the survey and the wardmage in Fedimian sends a Seal Release Crystal back with my courier — one for the inspector. {#FF6666}Don't go in until you have it.{/} The crystal is the only way back out, and she only cuts them for surveyors who actually brought her the notes."));
						break;

					case "info":
						await dialog.Msg(L("{#666666}*He grips the haft of his halberd a little tighter*{/}"));
						await dialog.Msg(L("You don't come back out, that's what happens. The seals are a one-way binding — they don't care if you're a demon or a man, only that you crossed from the inside. Anything trying to leave the prison gets pinned."));
						await dialog.Msg(L("Three militiamen and a Pyromancer have walked through that gate while I've been posted here. None of them came back. We hear them sometimes, when the wind's right."));
						await dialog.Msg(L("The wardmage in Fedimian can tune a Seal Release Crystal to a single person — that's what slips you past the binding on the way out. She doesn't make many, and only for surveyors who bring her useful notes. So if you ever plan on going in there, finish the survey first, and don't lose the crystal."));
						break;

					case "leave":
						await dialog.Msg(L("Understood. Somebody has to walk it. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var stonesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", 0);

				if (stonesVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*He spreads your notes on his shield, going through each seal's measurements*{/}"));
					await dialog.Msg(L("Three of the seals show even wear. Centuries of weather, nothing unusual. The fourth one is eroding faster than the others — pitted strokes, soft edges. The pollen's chewing at it harder than wind ever could."));
					await dialog.Msg(L("{#666666}*He folds the notes into his courier-pouch and hands you a small pale crystal*{/}"));
					await dialog.Msg(L("Wardmage's standing offer — a Seal Release Crystal, attuned to your survey. She keeps a few cut and ready for anyone who does her the favor. Hang on to it. If you ever set foot inside that prison, that crystal is how you get back out."));
					await dialog.Msg(L("The wardmage in Klaipeda needs to see this before the fourth seal fails. Soldier's purse for your trouble."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four seals, counter-clockwise. Inspect each one up close, and don't linger at the portal."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The wardmage rode out two days ago. She's drafting a re-carving plan for the fourth seal. The other three should hold another generation, she thinks."));

				if (character.Inventory.CountItem(650530) <= 0)
				{
					var response = await dialog.Select(L("You're not carrying the Seal Release Crystal she cut for you. Used it, lost it, never picked it up — doesn't matter. The wardmage left a small stock with my courier for exactly this. Want a replacement?"),
						Option(L("Yes, I'll take one"), "give"),
						Option(L("No, I'm fine"), "decline")
					);

					if (response == "give")
					{
						await dialog.Msg(L("{#666666}*He digs a fresh pale crystal out of his courier-pouch and hands it to you*{/}"));
						await dialog.Msg(L("Last one in the pouch. Courier rides for Fedimian tomorrow — she'll send more by week's end. Keep it on you this time."));
						character.Inventory.Add(650530, 1, InventoryAddType.PickUp);
					}
					else
					{
						await dialog.Msg(L("Suit yourself. Come back if you change your mind — the pouch isn't going anywhere."));
					}
				}
			}
		});

		// =====================================================================
		// BROKEN WARD STONES
		// =====================================================================
		// For Quest 1004 - Broken Ward Stones
		// =====================================================================

		void AddWardStone(int stoneNumber, string stoneName, string observation, int x, int z, int direction)
		{
			AddNpc(150187, L(stoneName), "f_farm_47_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An ancient warding stone, its binding rune still legible beneath centuries of weather*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_2.Quest1004.Stone{stoneNumber}";
				var inspected = character.Variables.Perm.GetBool(variableKey, false);

				if (inspected)
				{
					await dialog.Msg(L("{#666666}*You've already inspected this seal*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Inspecting seal..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var stonesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", stonesVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Seals inspected: {0}/4", stonesVisited + 1));

					if (stonesVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}Survey complete! Return to Grimaras.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Inspection interrupted."));
				}
			});
		}

		AddWardStone(1, "East Ward Stone", "Edges worn smooth by wind. Rune still legible, strokes shallow but whole. Even decay.", -1561, -1138, 135);
		AddWardStone(2, "South Ward Stone", "Lower half sound. The top has weathered down to faint grooves. Slow, even erosion.", -1555, -1244, 45);
		AddWardStone(3, "West Ward Stone", "The oldest of the four - moss in every furrow, but the binding rune holds at the core.", -1667, -1239, 315);
		AddWardStone(4, "North Ward Stone", "Erosion is sharp here, not gradual. The rune's strokes are pitted, the edges going soft. Something is eating at this seal faster than weather alone.", -1659, -1141, 225);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Aqueduct Siege
//-----------------------------------------------------------------------------

public class AqueductSiegeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_2", 1001);
		SetName("Aqueduct Siege");
		SetType(QuestType.Sub);
		SetDescription("Farmer Tadas needs the Kepari Shamans thinned before their spells crack the aqueduct supports.");
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Tadas", "f_farm_47_2");

		AddObjective("killShaman", "Defeat Kepari Shamans",
			new KillObjective(15, new[] { MonsterId.Kepari_Mage }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
	}
}

// Quest 1002 CLASS: Refugee Protocol
//-----------------------------------------------------------------------------

public class RefugeeProtocolQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_2", 1002);
		SetName("Refugee Protocol");
		SetType(QuestType.Sub);
		SetDescription("Audrone needs her refugee-protocol delivered to Mykolas at Tenants' Farm so the evacuation plan is ready before the portal widens.");
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Audrone", "f_farm_47_2");

		AddObjective("deliverProtocol", "Deliver the refugee protocol to Mykolas",
			new VariableCheckObjective("Laima.Quests.f_farm_47_2.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_2.Quest1002.Delivered");

		var leftover = character.Inventory.CountItem(650580);
		if (leftover > 0)
			character.Inventory.Remove(650580, leftover, InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_2.Quest1002.Delivered");

		var leftover = character.Inventory.CountItem(650580);
		if (leftover > 0)
			character.Inventory.Remove(650580, leftover, InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Charged Dandel Pods
//-----------------------------------------------------------------------------

public class ChargedDandelPodsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_2", 1003);
		SetName("Charged Dandel Pods");
		SetType(QuestType.Sub);
		SetDescription("Wardmaker Lina needs five charged Dandel pods to complete a ward-circle at the evacuation barn before sundown.");
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wardmaker-Farmer] Lina", "f_farm_47_2");

		AddObjective("collectPods", "Gather charged Dandel pods",
			new CollectItemObjective(663355, 6));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663355,
			character.Inventory.CountItem(663355),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_2.Quest1003.Pod{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_2.Quest1003.Pod{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663355,
			character.Inventory.CountItem(663355),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_2.Quest1003.Pod{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_2.Quest1003.Pod{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Broken Ward Stones
//-----------------------------------------------------------------------------

public class BrokenWardStonesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_2", 1004);
		SetName("Eroding Seals");
		SetType(QuestType.Sub);
		SetDescription("Soldier Grimaras needs the four ancient ward stones ringing the demon-prison portal inspected for erosion, so a Fedimian wardmage can judge how long each seal will hold.");
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Soldier] Grimaras", "f_farm_47_2");

		AddObjective("surveyStones", "Inspect the four ward stones for erosion",
			new VariableCheckObjective("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", 4, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1));  // Recovery Potion
		AddReward(new ItemReward(650530, 1));  // Seal Release Crystal
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_2.Quest1004.Stone{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_2.Quest1004.Stone{i}");
		}
	}
}
