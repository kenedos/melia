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
		// Bridge-Farmer Mindaugas - Kepari Shamans assaulting the aqueduct
		//---------------------------------------------------------------------
		AddNpc(20128, L("[Bridge-Farmer] Mindaugas"), "f_farm_47_2", 2559, -1202, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer in patched armor inspects a scorch-mark on the aqueduct's stone footing, knuckles white on a short spear*{/}"));
				await dialog.Msg(L("They're casting at the bridges now. Kepari Shamans - Velnias-race demon-flesh, right out of the prison gate. If one stone of that aqueduct cracks, four farms lose water before sundown."));

				var response = await dialog.Select(L("I was a farmhand two months ago. Now I'm a bridge-warden with a farmer's spear. The magistrates won't send soldiers because 'the situation is being monitored.' I need them thinned. Fifteen shamans - that's enough to stop the next spell."),
					Option(L("I'll kill the Shamans"), "help"),
					Option(L("Why are they attacking the water?"), "info"),
					Option(L("Sounds suicidal"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He exhales, the spear shaft loosening in his grip*{/}"));

						if (await dialog.YesNo(L("Fifteen Shamans. They cluster near the portal warp - south edge of the map - and hit the bridge supports from there. Will you do it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Take this dagger - Arde-forged. The fire bites demon-flesh harder than anything else I've got."));
							character.Inventory.Add(113001, 1, InventoryAddType.PickUp);
							await dialog.Msg(L("Stay low when they cast. The lightning arcs straight - duck and it passes over."));
						}
						break;

					case "info":
						await dialog.Msg(L("Ask them. They don't answer. The best guess Saule has is that they want the farms evacuated before something bigger comes through the portal."));
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
				await dialog.Msg(L("The aqueduct still flows. Four farms still have water. Every day we hold the bridge is a day the villages don't evacuate."));
			}
		});

		// =====================================================================
		// QUEST 1002: Refugee Protocol
		// =====================================================================
		// Farmer Audrone - Evacuation planning with Tenants' Farm
		//---------------------------------------------------------------------
		AddNpc(20161, L("[Farmer] Audrone"), "f_farm_47_2", -493, 960, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1002);

			dialog.SetTitle(L("Audrone"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer annotates a map of the farm cluster, marking each homestead with a colored pin*{/}"));
				await dialog.Msg(L("If the portal widens tomorrow, I need to know which farm takes whose children. Red pins for Myrkiti, blue for Shaton, green for here. Tenants' Farm has the most barns - they shelter Myrkiti's families first."));

				var response = await dialog.Select(L("I drafted a refugee protocol - who goes where, what supplies each host farm needs, who rings the evacuation bell. Mykolas at Tenants' Farm needs a copy before he can plan the barn conversions. Carry it through the north warp?"),
					Option(L("I'll deliver it to Mykolas"), "help"),
					Option(L("Is it really that bad?"), "info"),
					Option(L("That's the magistrate's job"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She folds the protocol carefully, tucking it into a water-proof oilcloth*{/}"));

						if (await dialog.YesNo(L("Mykolas is at Tenants' Farm through the north warp. Tell him red pins come first. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If Mykolas argues, tell him it's not negotiable - the plan has to be ready before the bell rings."));
							await dialog.Msg(L("And if you pass Gintaras near the portal - tell him the ward-survey is still waiting."));
						}
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
		// Tenant-Farmer Mykolas - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Tenant-Farmer] Mykolas"), "f_farm_47_2", 1888, 1061, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1002);

			dialog.SetTitle(L("Mykolas"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A broad-shouldered tenant farmer chews a stalk of straw, eyes on a barn ridge in the distance*{/}"));
				await dialog.Msg(L("Tenants' Farm is through the warp behind me. Quieter over there. For now."));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_2.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("Tell Audrone seven barns, thirty pallets. I'll have them cleared by week's end."));
				return;
			}

			await dialog.Msg(L("{#666666}*He reads the protocol, jaw tightening*{/}"));
			await dialog.Msg(L("Red pins first. Myrkiti's children. Right."));
			await dialog.Msg(L("{#666666}*He folds the paper and tucks it inside his coat*{/}"));
			await dialog.Msg(L("Tell Audrone I've got seven barns, thirty pallets each - more than she calculated. The big hay-barn has a coal stove; I'll prioritize that one for infants."));
			await dialog.Msg(L("And tell her I'll have the barns ready in five days, not seven. No sense cutting it close when the portal's widening."));

			character.Variables.Perm.Set("Laima.Quests.f_farm_47_2.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Protocol delivered. Return to Audrone.{/}"));
		});

		// =====================================================================
		// QUEST 1003: Charged Dandel Pods
		// =====================================================================
		// Wardmaker-Farmer Saule - Ward-circles against demon-pollen
		//---------------------------------------------------------------------
		AddNpc(147419, L("[Wardmaker-Farmer] Saule"), "f_farm_47_2", 460, 4, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1003);

			dialog.SetTitle(L("Saule"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer winds copper wire around a tight bundle of dandelion fluff, crackling faintly with static*{/}"));
				await dialog.Msg(L("Three generations of wardmakers in my family. My grandmother warded against frost, my mother against weevils. I ward against demon-pollen. A Dandel pod holds enough charge to keep a threshold clean for a month."));

				var response = await dialog.Select(L("I need five charged pods for a new ward-circle at the evacuation barn. The Dandels drift over the fields - pluck the ripe ones, the ones that prickle when you touch them. Left ones too long, they lose their spark; took ones too early, no charge at all."),
					Option(L("I'll gather five pods"), "help"),
					Option(L("How do the wards work?"), "info"),
					Option(L("Another farmer task"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you a copper-lined pouch*{/}"));

						if (await dialog.YesNo(L("The pouch holds the charge. Drop each pod straight in, don't squeeze. Five pods and the circle is done. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Touch only the stem. The fluff itself is soft but the static hurts. Startled Dandels buzz past - if one lands in your hair, shake it off, don't swat it."));
						}
						break;

					case "info":
						await dialog.Msg(L("Copper coil, dandelion charge, salt circle, threshold stone. Close the loop, bind the corners, seal with the charge at sunrise."));
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

				if (podCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She tests each pod with a copper pick; sparks jump neatly*{/}"));
					await dialog.Msg(L("All five charged. Stem-plucked. No crushed fluff. You've a careful hand."));
					await dialog.Msg(L("Here - a farmer's wage with my own thanks folded in. The evacuation barn has its ward by sundown."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five pods. Stem-pluck only. The copper pouch holds the charge - drop them straight in."));
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
			AddNpc(47246, L("Charged Dandel Pod"), "f_farm_47_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_2", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A dandelion head drifts at waist height, fluff crackling faintly with static*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_2.Quest1003.Pod{podNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The stem has already been cut. A new pod will drift by tomorrow*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_farm_47_2.Quest1003.Pod{podNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Dandel_Orange, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A startled Orange Dandel buzzes away from the pod!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Plucking stem..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(663355, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(663355);
					character.ServerMessage(LF("Charged pods gathered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All pods gathered! Return to Saule.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Plucking interrupted."));
				}
			});
		}

		AddDandelPod(1, -708, 1023, 0);
		AddDandelPod(2, -432, 1196, 90);
		AddDandelPod(3, 357, 1938, 180);
		AddDandelPod(4, 529, 287, 270);
		AddDandelPod(5, -615, 1724, 0);

		// =====================================================================
		// QUEST 1004: Broken Ward Stones
		// =====================================================================
		// Farmer Gintaras - Surveying the ancient warding circle at the portal
		//---------------------------------------------------------------------
		AddNpc(20125, L("[Farmer] Gintaras"), "f_farm_47_2", -1616, -1188, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_2", 1004);

			dialog.SetTitle(L("Gintaras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A quiet farmer sharpens a knife he will not use, the warp-circle's glow flickering across his hands*{/}"));
				await dialog.Msg(L("The Ancients built a ring of warding stones around this portal. Four stones. Each one held a rune that kept the prison sealed."));

				var response = await dialog.Select(L("The stones are cracked now. All four. A Fedimian mage could read the breaks - tell us when each seal failed, tell us why. I can't approach the portal myself; the pollen close to the warp knocks a man down. But you might. Survey each stone and bring the notes back."),
					Option(L("I'll survey the stones"), "help"),
					Option(L("Why did the seals fail?"), "info"),
					Option(L("The portal's too close"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He hands you a piece of charcoal and a square of vellum*{/}"));

						if (await dialog.YesNo(L("Rub the charcoal over the crack on each stone. The fracture pattern will transfer. Don't stand still near the portal - demons see movement less than they smell blood. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Four stones. They ring the warp point. Walk the circle counter-clockwise - superstition, but mine."));
							await dialog.Msg(L("If you smell metal in your mouth, the pollen is getting to you. Back off for a count of ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("Saule says the seals were worn thin for centuries and the demon war finished them off. Mindaugas says someone broke them from outside. I don't know."));
						await dialog.Msg(L("The ward-mage will know. That's why we need the rubbings."));
						break;

					case "leave":
						await dialog.Msg(L("It is. But somebody has to walk it. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var stonesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", 0);

				if (stonesVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*He spreads the rubbings, tracing each fracture line with the tip of his knife*{/}"));
					await dialog.Msg(L("Three broken outward - as if pushed from the inside. One broken inward - as if struck from outside."));
					await dialog.Msg(L("{#666666}*He sets the knife down carefully*{/}"));
					await dialog.Msg(L("Someone broke the fourth seal. The demons did the rest. The wardmage in Klaipeda needs to see this. Take these for your walking."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four stones. Counter-clockwise. Don't linger at the portal."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The wardmage rode out two days ago. She's studying the rubbings. She hasn't spoken a word since she read them."));
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
					await dialog.Msg(L("{#666666}*An ancient warding stone, its runes split by a deep fracture*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_2.Quest1004.Stone{stoneNumber}";
				var rubbed = character.Variables.Perm.GetBool(variableKey, false);

				if (rubbed)
				{
					await dialog.Msg(L("{#666666}*You've already taken a rubbing from this stone*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Taking rubbing..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var stonesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", stonesVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Ward stones surveyed: {0}/4", stonesVisited + 1));

					if (stonesVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}Survey complete! Return to Gintaras.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Rubbing interrupted."));
				}
			});
		}

		AddWardStone(1, "North Ward Stone", "Fracture pushed outward from the core. The rune is shattered at the top.", -1560, -1100, 0);
		AddWardStone(2, "East Ward Stone", "Fracture pushed outward. The rune's lower half is intact, the upper crumbled.", -1500, -1240, 90);
		AddWardStone(3, "South Ward Stone", "Fracture pushed outward. The oldest of the four - weathered deep.", -1680, -1290, 180);
		AddWardStone(4, "West Ward Stone", "Fracture pushed inward. The edges are clean, sharp. Recent. Deliberate.", -1720, -1130, 270);
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
		SetDescription("Bridge-Farmer Mindaugas needs the Kepari Shamans thinned before their spells crack the aqueduct supports.");
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Bridge-Farmer] Mindaugas", "f_farm_47_2");

		AddObjective("killShaman", "Defeat Kepari Shamans",
			new KillObjective(15, new[] { MonsterId.Kepari_Mage }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
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
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_2.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_2.Quest1002.Delivered");
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
		SetDescription("Wardmaker Saule needs five charged Dandel pods to complete a ward-circle at the evacuation barn before sundown.");
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wardmaker-Farmer] Saule", "f_farm_47_2");

		AddObjective("collectPods", "Gather charged Dandel pods",
			new CollectItemObjective(663355, 5));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640011, 3));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663355,
			character.Inventory.CountItem(663355),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
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

		for (int i = 1; i <= 5; i++)
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
		SetName("Broken Ward Stones");
		SetType(QuestType.Sub);
		SetDescription("Gintaras needs the four ancient ward stones around the demon-prison portal surveyed, so a Fedimian wardmage can read when and how each seal failed.");
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Gintaras", "f_farm_47_2");

		AddObjective("surveyStones", "Take rubbings from the broken ward stones",
			new VariableCheckObjective("Laima.Quests.f_farm_47_2.Quest1004.StonesVisited", 4, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 4));  // Recovery Potion
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
