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
				await dialog.Msg(L("{#666666}*A farmer in patched armor turns a scorched stone chip in his fingers, knuckles white on a short spear*{/}"));
				await dialog.Msg(L("They're casting at the bridges now. Kepari Shamans - Velnias-race demon-flesh, right out of the prison gate. If one stone of that aqueduct cracks, four farms lose water before sundown."));

				var response = await dialog.Select(L("I was a farmhand two months ago. Now I'm holding a spear and watching the water for sparks. The magistrates won't send soldiers because 'the situation is being monitored.' I need them thinned. Fifteen shamans - that's enough to stop the next spell."),
					Option(L("I'll kill the Shamans"), "help"),
					Option(L("Why are they attacking the water?"), "info"),
					Option(L("Sounds suicidal"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He exhales, the spear shaft loosening in his grip*{/}"));

						character.Quests.Start(questId);

						if (!character.Variables.Perm.GetBool("Laima.Quests.f_farm_47_2.Quest1001.DaggerGiven", false))
						{
							await dialog.Msg(L("Hold on - take this knife before you go. It's nothing fancy, kitchen-grade steel my brother left behind, but it'll bite a Kepari deeper than a wooden spear-shaft will. Better than nothing."));
							await dialog.Msg(L("{#666666}*He works a plain knife from his belt and presses it into your hand.*{/}"));
							character.Inventory.Add(111001, 1, InventoryAddType.PickUp);
							character.Variables.Perm.Set("Laima.Quests.f_farm_47_2.Quest1001.DaggerGiven", true);
						}

						await dialog.Msg(L("Stay low when they cast. The lightning arcs straight - duck and it passes over."));
						break;

					case "info":
						await dialog.Msg(L("Ask them. They don't answer. The best guess Lina has is that they want the farms evacuated before something bigger comes through the portal."));
						await dialog.Msg(L("If the wells dry, we pack. If we pack, the demon prison has a clear road to Klaipeda. So we stay. And we fight."));
						break;

					case "leave":
						await dialog.Msg(L("Yeah, probably is. Come back if you change your mind. Or don't. I'll be here either way."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killShaman", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to the aqueduct, the water running steady*{/}"));
					await dialog.Msg(L("Fifteen down. The bridges held. That's a week of water, maybe two, before they regroup."));
					await dialog.Msg(L("Take this - farmhand's purse, honest earnings. And keep the Arde dagger. You'll need it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Fifteen Shamans. Near the portal warp. Stay low when the lightning flies."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The aqueduct still flows. Four farms still have water. Every day we hold is a day the villages don't pack."));
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
				await dialog.Msg(L("{#666666}*A farmer annotates a map of the farm cluster, marking each homestead with a colored pin*{/}"));
				await dialog.Msg(L("If the portal widens tomorrow, I need to know which farm takes whose children. Red pins for Myrkiti, blue for Shaton, green for here. Tenants' Farm has the most barns - they shelter Myrkiti's families first."));

				var response = await dialog.Select(L("I drafted a refugee protocol - who goes where, what supplies each host farm needs, who rings the evacuation bell. Mykolas at Tenants' Farm needs a copy before he can plan the barn conversions. Can you carry it for me towards Tenants' Farm?"),
					Option(L("I'll deliver it to Mykolas"), "help"),
					Option(L("Is it really that bad?"), "info"),
					Option(L("That's the magistrate's job"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She folds the pin-marked map into a waterproof oilcloth and presses it into your hands*{/}"));

						character.Quests.Start(questId);
						character.Inventory.Add(650580, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("If Mykolas argues, tell him it's not negotiable - the plan has to be ready before the bell rings."));
						await dialog.Msg(L("And if you pass Grimaras near the portal - tell him the ward-survey is still waiting."));
						break;

					case "info":
						await dialog.Msg(L("The magistrates sent one Pyromancer last week. She sealed two cracks and went back to Klaipeda. The portal is still open."));
						await dialog.Msg(L("We plan for evacuation because the alternative is panic. Panic gets children trampled in a barn doorway. A protocol doesn't."));
						break;

					case "leave":
						await dialog.Msg(L("The magistrate's already had the letter six weeks. They 'review procedural recommendations.' Meanwhile we farm and pray."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverProtocol", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She nods grimly as you report Mykolas's reply*{/}"));
					await dialog.Msg(L("Seven barns, thirty pallets each. That's more capacity than I calculated. Good - we might fit everyone."));
					await dialog.Msg(L("Take these. A farmer's thanks. If the bell rings tomorrow, your coin is the least we owe."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Through the north warp. Mykolas - older, broad man. Give him the protocol."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The protocol is posted at every farm house now. Children know which barn to run to. That's something."));
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
				await dialog.Msg(L("{#666666}*A farmer winds copper wire around a knotty round pod, the husk crackling faintly under her fingers*{/}"));
				await dialog.Msg(L("Three generations of wardmakers in my family. My grandmother warded against frost, my mother against weevils. I ward against demon-pollen. One charged Dandel pod holds enough lightning to keep a threshold clean for a month."));

				var response = await dialog.Select(L("I need six charged pods for a new ward-circle at the evacuation barn. The Dandels drift over the fields, snapping at gnats with those big toothed mouths - and every so often one drops a pod. The fresh ones still hum from the parent's lightning. Pluck the ripe ones, the ones that prickle when you touch them. Left ones too long, they go inert; took ones too early, no charge at all."),
					Option(L("I'll gather six pods"), "help"),
					Option(L("How do the wards work?"), "info"),
					Option(L("Another farmer task"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you a copper-lined pouch*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Pluck them by the husk, not the spine. The husk's tough; the spine's where the charge lives, and it bites. If a Dandel comes circling overhead, don't swing at it - they hate the crack of a blade and they spit lightning when they're angry. Just stand still until it loses interest."));
						break;

					case "info":
						await dialog.Msg(L("Copper coil, pod-charge, salt circle, threshold stone. Close the loop, bind the corners, seal with the charge at sunrise."));
						await dialog.Msg(L("Keeps the pollen out for a lunar month. Keeps most small demons out for three nights. Keeps a Kepari Shaman out for about one angry spell. Not forever. Never forever."));
						break;

					case "leave":
						await dialog.Msg(L("Then the barn has no ward tonight. Children sleep under unwarded roofs. Small decisions stack."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var podCount = character.Inventory.CountItem(663355);

				if (podCount >= 6)
				{
					await dialog.Msg(L("{#666666}*She tests each pod with a copper pick; sparks jump neatly*{/}"));
					await dialog.Msg(L("All six charged. Husk-plucked, spines unbroken. You've a careful hand."));
					await dialog.Msg(L("Here - a farmer's wage with my own thanks folded in. The evacuation barn has its ward by sundown."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Six pods. Husk-pluck only, never the spine. The copper pouch holds the charge - drop them straight in."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The evacuation barn is warded. If the bell rings, the children sleep safe. That's what the wards are for."));
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
				await dialog.Msg(L("{#666666}*A soldier in field colors leans on his halberd, the warp-circle's glow flickering across his breastplate*{/}"));
				await dialog.Msg(L("Years ago the ground tore open in the middle of this field. No warning, no quake - one morning a hole into the demon prison was just there, and it has been there since. The Ancients rode out from Klaipeda within the week and laid four seals around it. Stone wards, each carved with a binding rune."));
				await dialog.Msg(L("The seals don't close the portal. They never could. What they do is keep what's inside the prison from spilling out. A man can walk through that gate if he's stupid enough - the seals don't stop anyone going in. Nothing comes back the other way. That's the whole point."));

				var response = await dialog.Select(L("The seals are still holding, but stone weathers. Wind, frost, demon-pollen eating the carving year by year. The wardmage in Fedimian wants to know how much time we have before any of them give. Walk the circle, inspect each seal up close, note the wear - hairline cracks, runes worn shallow, edges crumbling. My orders keep me on the line; I can't break perimeter to do it myself."),
					Option(L("I'll inspect the seals"), "help"),
					Option(L("What happens if I go in?"), "info"),
					Option(L("The portal's too close"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He hands you a brass caliper and a soldier's notebook from his pack, then catches your wrist before you turn away*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Listen. Look at me. Stay on this side of the portal. The seals around it are an outbound binding - whatever's on the inside trying to get out gets pinned, and that includes you. Cross that threshold and you don't come back. Not by sword, not by spell, not by prayer."));
						await dialog.Msg(L("I have watched four people walk through that gate thinking they'd just take a quick look. None of them came back. So do me the favor: keep both feet outside the warp circle while you work. The seals you're surveying are at arm's reach from the edge - you don't need to step inside to read them."));
						await dialog.Msg(L("Four seals. Ring the warp point. Walk the circle counter-clockwise - old soldier's habit, sweep your blind side first. Look close at each rune. Pitting in the strokes. Edges going soft. If you smell metal in your mouth, the pollen is getting to you. Back off for a count of ten."));
						await dialog.Msg(L("Finish the survey and the wardmage in Fedimian sends a Seal Release Crystal back with my courier - one for the inspector. {#FF6666}Don't go in until you have it.{/} The crystal is the only way out, and she only cuts them for surveyors who already brought her the notes."));
						break;

					case "info":
						await dialog.Msg(L("{#666666}*He grips the haft of his halberd a little tighter*{/}"));
						await dialog.Msg(L("You don't come back out. That's what happens. The seals are a one-way binding - they don't care whether you're a demon or a man, only that you crossed the threshold from the inside. Anything with a soul-tether trying to leave the prison gets pinned by the wards."));
						await dialog.Msg(L("Three militiamen and a Pyromancer have walked through that gate in my time on this line. None of them came back. We hear them sometimes, on the wind. It carries from the other side."));
						await dialog.Msg(L("The wardmage in Fedimian can attune a Seal Release Crystal to a single bearer - the crystal slips you past the binding on the way out. She makes them sparingly, and only for surveyors who give her something useful. So if you ever plan on going in there - finish the seal survey first, and don't lose the crystal."));
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
					await dialog.Msg(L("{#666666}*He spreads your notes on his shield, running through each seal's measurements with a gloved finger*{/}"));
					await dialog.Msg(L("Three of the seals show steady wear. Centuries of weather, even decay, nothing surprising. The fourth is eroding faster than the others. Pitted strokes. Edges going soft. The pollen is eating at it harder than wind ever could."));
					await dialog.Msg(L("{#666666}*He folds the notes into his courier-pouch and hands you a small pale crystal in return*{/}"));
					await dialog.Msg(L("Wardmage's standing offer. A Seal Release Crystal, attuned to your survey - she keeps a few cut and ready for whoever does her the favor. Keep it on you. If you ever set foot inside that prison, the crystal is what gets you back out."));
					await dialog.Msg(L("The wardmage in Klaipeda needs to see this before that fourth seal fails. Soldier's purse for your trouble."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four seals. Counter-clockwise. Inspect each one closely. Don't linger at the portal."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The wardmage rode out two days ago. She's drafting a re-carving plan for the fourth seal. The other three should hold for another generation, she thinks."));

				if (character.Inventory.CountItem(650530) <= 0)
				{
					var response = await dialog.Select(L("You're not carrying the Seal Release Crystal she cut for you. Used it, lost it, or never picked it up - doesn't matter. The wardmage left a small stock with my courier for exactly this. Want a replacement?"),
						Option(L("Yes, I'll take one"), "give"),
						Option(L("No, I'm fine"), "decline")
					);

					if (response == "give")
					{
						await dialog.Msg(L("{#666666}*He digs a fresh pale crystal out of his courier-pouch and presses it into your hand*{/}"));
						await dialog.Msg(L("Last one in the pouch. The courier rides for Fedimian tomorrow; she'll send more by week's end. Keep it on you this time."));
						character.Inventory.Add(650530, 1, InventoryAddType.PickUp);
					}
					else
					{
						await dialog.Msg(L("Suit yourself. Come back if you change your mind - the pouch isn't going anywhere."));
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
