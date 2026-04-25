//--- Melia Script ----------------------------------------------------------
// Emmet Forest - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_23_1 map.
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

public class FWhitetrees231QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Lyoni Thinning
		// =====================================================================
		// Scout Rimvyd - Follow-up on Nobreer 1004 (Zeffi push)
		//---------------------------------------------------------------------
		AddNpc(47245, L("[Scout] Rimvyd"), "f_whitetrees_23_1", -1306, 390, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1001);

			dialog.SetTitle(L("Rimvyd"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A weathered scout crouches behind a fallen trunk, eyes fixed on the ridge*{/}"));
				await dialog.Msg(L("Kazys sent me ahead. Told me the Zeffi were pushing east - seems he was right."));

				var response = await dialog.Select(L("The Zeffi drove the Lyoni packs off their old ground in Nobreer. They've crossed into Emmet now, and they're not happy about sharing territory."),
					Option(L("I can thin their numbers"), "help"),
					Option(L("Why does it matter?"), "info"),
					Option(L("Sounds like Kazys' problem"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("Good. I'd do it myself, but my orders are to watch and report."));

						if (await dialog.YesNo(L("The Lyoni have massed along the northern ridge and the central grove. Cut their numbers down before they reach the Pystis road. Can you handle it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Look for them in the packs - they hunt in groups. The biggest cluster is on the ridge north of here."));
							await dialog.Msg(L("Twenty-five should break their cohesion. Good hunting."));
						}
						break;

					case "info":
						await dialog.Msg(L("The Pystis road runs through the south end of Emmet. Every caravan from Orsha to Klaipeda uses it."));
						await dialog.Msg(L("If the Lyoni reach it, we'll be pulling bodies out of the brush for months. Better to stop them up here."));
						break;

					case "leave":
						await dialog.Msg(L("Watch the ridgeline if you pass through. Their hearing is sharp."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLyoni", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He nods slowly as you report*{/}"));
					await dialog.Msg(L("That'll hold them. They'll regroup eventually, but the road's safe for now."));
					await dialog.Msg(L("Take these - standard issue from the Orsha armory. Better than the rags you're wearing."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. The ridge north of here and the central grove - that's where they mass."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The road's quiet again. Kazys sends his thanks from Nobreer."));
			}
		});

		// =====================================================================
		// QUEST 1002: Letter for Eisenis
		// =====================================================================
		// Courier Vidas - Carries Saulene's letter from Nobreer
		//---------------------------------------------------------------------
		AddNpc(20168, L("[Courier] Vidas"), "f_whitetrees_23_1", -1164, 336, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1002);

			dialog.SetTitle(L("Vidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A courier clutches a sealed letter, glancing nervously at the treeline*{/}"));
				await dialog.Msg(L("Pilgrim Saulene from Nobreer paid me good coin to carry a letter to Druid Eisenis. I didn't know Emmet was this bad."));

				var response = await dialog.Select(L("The druid keeps to the central crossroads. I won't make it through the Lyoni packs - but a fighter like you might."),
					Option(L("I'll deliver it for you"), "help"),
					Option(L("What does the letter say?"), "info"),
					Option(L("Find another courier"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*Relief washes over his face*{/}"));

						if (await dialog.YesNo(L("Bless you. Eisenis waits at the central crossroads of Emmet - head east from here. Just hand him the letter and come back for payment. Will you do it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The letter's sealed with pine resin - Saulene's mark. Don't break it."));
							await dialog.Msg(L("Eisenis is the old druid. White beard, green robes. You can't miss him."));
						}
						break;

					case "info":
						await dialog.Msg(L("I don't read other people's letters, friend. That's how a courier stays employed."));
						await dialog.Msg(L("Saulene said something about the ancient stones needing a second watcher. That's all I know."));
						break;

					case "leave":
						await dialog.Msg(L("There aren't other couriers in Emmet. That's rather the problem."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverLetter", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*Vidas brightens at your return*{/}"));
					await dialog.Msg(L("Eisenis agreed? Ha - Saulene always could charm that old tree-talker."));
					await dialog.Msg(L("Here's your pay. More than I'd earn for the trip myself, but you did the dangerous part."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Eisenis is at the central crossroads. Head east, past the main grove."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Safe travels, friend. The Pystis road's waiting for my next run."));
			}
		});

		// =====================================================================
		// Druid Eisenis - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(155018, L("[Druid] Eisenis"), "f_whitetrees_23_1", -147, 368, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1002);

			dialog.SetTitle(L("Eisenis"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*An old druid listens to the rustling leaves, eyes half-closed*{/}"));
				await dialog.Msg(L("The white-leaf trees speak slowly. You must learn to wait."));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_1.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("Return to Vidas. The letter is read, the answer given."));
				return;
			}

			await dialog.Msg(L("{#666666}*He opens one eye, then both*{/}"));
			await dialog.Msg(L("A letter? Pine-resin seal... Saulene's hand."));
			await dialog.Msg(L("{#666666}*He reads slowly, lips moving*{/}"));
			await dialog.Msg(L("The stones need a second watcher. Yes. The forest has been asking me the same thing in its own way."));
			await dialog.Msg(L("Tell Vidas the old druid accepts. I'll walk to Nobreer before the next moon."));

			character.Variables.Perm.Set("Laima.Quests.f_whitetrees_23_1.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Letter delivered. Return to Courier Vidas.{/}"));
		});

		// =====================================================================
		// QUEST 1003: White-Leaf Bark
		// =====================================================================
		// Herbalist Dagna - Bark gathering
		//---------------------------------------------------------------------
		AddNpc(157100, L("[Herbalist] Dagna"), "f_whitetrees_23_1", 155, 278, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1003);

			dialog.SetTitle(L("Dagna"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A herbalist kneels by a pile of leather satchels, sorting cuttings*{/}"));
				await dialog.Msg(L("The white-leaf trees of Emmet hold the finest inner bark in all of Orsha. The trouble is knowing which trees will offer it freely."));

				var response = await dialog.Select(L("My master taught me to mark the willing ones. Five trees wear my ribbon now - but my knee gives me trouble, and the Tot have claimed the grove between us."),
					Option(L("I'll gather the bark for you"), "help"),
					Option(L("Why only certain trees?"), "info"),
					Option(L("Not today"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She presses her hands together in thanks*{/}"));

						if (await dialog.YesNo(L("Each marked tree will give one piece of inner bark - no more. Five bark pieces, five trees. Use your knife gently, ask first. Will you do it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The ribbons are pale green - easy to miss if you rush. Look at the trunks, not the canopy."));
							await dialog.Msg(L("Be wary - the Tot sometimes burst from the undergrowth. Their poison sap lingers on the bark if they touch it."));
						}
						break;

					case "info":
						await dialog.Msg(L("Every tree has a spirit, young or old. The old ones are ready to give. The young ones still need their bark for growing."));
						await dialog.Msg(L("Take from a young tree and the medicine turns bitter. Take from a willing one and it heals twice over."));
						break;

					case "leave":
						await dialog.Msg(L("Come back when the road is quieter. The trees aren't going anywhere."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var barkCount = character.Inventory.CountItem(666051);

				if (barkCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She weighs the bark in her palms*{/}"));
					await dialog.Msg(L("Five pieces - clean, unspoiled, and still faintly green beneath."));
					await dialog.Msg(L("{#666666}*She stacks them carefully in a linen wrap*{/}"));
					await dialog.Msg(L("This will stock my salves for a full season. The trees offered to you - I can tell."));
					await dialog.Msg(L("Take these. You've earned more than I usually pay, but your hands did good work."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Look for the pale green ribbons on the trunks. Five marked trees, spread across Emmet."));
					await dialog.Msg(L("Mind the Tot - their sap burns if it touches the bark."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The salves hold up well through winter. I'll mark new trees next spring."));
			}
		});

		// =====================================================================
		// MARKED WHITE-LEAF TREES
		// =====================================================================
		// For Quest 1003 - White-Leaf Bark
		// =====================================================================

		void AddMarkedTree(int treeNumber, int x, int z, int direction)
		{
			AddNpc(150214, L("Marked White-Leaf Tree"), "f_whitetrees_23_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_23_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pale green ribbon is tied around the trunk, weathered by wind*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_23_1.Quest1003.Tree{treeNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The bark here has already been carefully cut. The tree is resting*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_whitetrees_23_1.Quest1003.Tree{treeNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Kucarry_Tot, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Kugheri Tot bursts from the undergrowth!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Gathering bark..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(666051, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(666051);
					character.ServerMessage(LF("Inner bark gathered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All bark gathered! Return to Herbalist Dagna.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Gathering cancelled."));
				}
			});
		}

		AddMarkedTree(1, -1403, -188, 0);
		AddMarkedTree(2, -176, 385, 90);
		AddMarkedTree(3, 142, 263, 180);
		AddMarkedTree(4, -358, 240, 270);
		AddMarkedTree(5, -74, 707, 0);

		// =====================================================================
		// QUEST 1004: Tracks in the Clearings
		// =====================================================================
		// Scholar Ilara - Continues Virinne's Kugheri research (Nobreer 1002)
		//---------------------------------------------------------------------
		AddNpc(150183, L("[Scholar] Ilara"), "f_whitetrees_23_1", -760, -1152, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1004);

			dialog.SetTitle(L("Ilara"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young scholar flips through a notebook labeled 'Kugheri Tribal Territories'*{/}"));
				await dialog.Msg(L("Sage Virinne is my teacher. She proved the bells are language - now I'm proving the tribes draw borders."));

				var response = await dialog.Select(L("Each Kugheri tribe marks its ground. Little cairns, bone piles, scraps of cloth. Four markers in Emmet are placed where two or three tribes overlap. I need them examined - but my hands shake when I'm near the Kugheri."),
					Option(L("I'll examine the markers"), "help"),
					Option(L("Why does this matter?"), "info"),
					Option(L("I'd rather not get stabbed for research"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She thrusts a charcoal pencil and paper at you*{/}"));

						if (await dialog.YesNo(L("Four markers. Observe each for a moment - the pencil rubbings will tell me which tribe claims it and whether the lines are shifting. Will you do it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The markers are in the clearings - places where the trees thin. One near each main grove."));
							await dialog.Msg(L("You don't need to fight. Just watch, rub, move on. Don't linger once you're done."));
						}
						break;

					case "info":
						await dialog.Msg(L("If the tribes are redrawing their borders, something is pushing them. Pressure from outside. Something Virinne and I haven't identified yet."));
						await dialog.Msg(L("The bells are how they speak. The markers are where they draw lines. Understand both and you understand the Kugheri."));
						break;

					case "leave":
						await dialog.Msg(L("Keep your eyes open if you walk the clearings. The markers tell a story, even if you don't read it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var markersVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_1.Quest1004.MarkersVisited", 0);

				if (markersVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*She spreads the rubbings across her notebook*{/}"));
					await dialog.Msg(L("Four markers, three tribes overlapping... look at this pattern."));
					await dialog.Msg(L("The Sommi are pushing into Tot ground. The Lyoni are holding the ridge but losing the south. Something's forcing them to reshuffle."));
					await dialog.Msg(L("Virinne will want these notes immediately. Thank you - take these supplies as my thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The markers are in the clearings. One by each main grove."));
					await dialog.Msg(L("Observe, rub, move on. Don't linger."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Virinne confirmed my hypothesis. The tribes are being pushed - but from where, we still don't know."));
			}
		});

		// =====================================================================
		// TERRITORY MARKERS
		// =====================================================================
		// For Quest 1004 - Tracks in the Clearings
		// =====================================================================

		void AddTerritoryMarker(int markerNumber, int x, int z, int direction)
		{
			AddNpc(147414, L("Kugheri Territory Marker"), "f_whitetrees_23_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_23_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A crude cairn of river stones, wound with scraps of knotted cloth*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_23_1.Quest1004.Marker{markerNumber}";
				var observed = character.Variables.Perm.GetBool(variableKey, false);

				if (observed)
				{
					await dialog.Msg(L("{#666666}*You've already taken a rubbing from this marker*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Observing..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var markersVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_1.Quest1004.MarkersVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_whitetrees_23_1.Quest1004.MarkersVisited", markersVisited + 1);

					character.ServerMessage(LF("Territory markers observed: {0}/4", markersVisited + 1));

					if (markersVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All markers observed! Return to Scholar Ilara.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Observation interrupted."));
				}
			});
		}

		AddTerritoryMarker(1, 602, 1290, 0);
		AddTerritoryMarker(2, 182, 282, 90);
		AddTerritoryMarker(3, 975, 513, 180);
		AddTerritoryMarker(4, -302, -1228, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Lyoni Thinning
//-----------------------------------------------------------------------------

public class LyoniThinningQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1001);
		SetName("Lyoni Thinning");
		SetType(QuestType.Sub);
		SetDescription("Scout Rimvyd needs help thinning the Kugheri Lyoni that have been displaced into Emmet Forest by the Zeffi push from Nobreer.");
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Scout] Rimvyd", "f_whitetrees_23_1");

		AddObjective("killLyoni", "Defeat Kugheri Lyoni",
			new KillObjective(25, new[] { MonsterId.Kucarry_Lioni }));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Normal Stamina Potion
		AddReward(new ItemReward(501127, 1));  // Steel Chain Gloves
	}
}

// Quest 1002 CLASS: Letter for Eisenis
//-----------------------------------------------------------------------------

public class LetterForEisenisQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1002);
		SetName("Letter for Eisenis");
		SetType(QuestType.Sub);
		SetDescription("Courier Vidas has asked you to deliver Pilgrim Saulene's sealed letter to Druid Eisenis at the central crossroads of Emmet Forest.");
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Courier] Vidas", "f_whitetrees_23_1");

		AddObjective("deliverLetter", "Deliver Saulene's letter to Druid Eisenis",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_23_1.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_1.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_1.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: White-Leaf Bark
//-----------------------------------------------------------------------------

public class WhiteLeafBarkQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1003);
		SetName("White-Leaf Bark");
		SetType(QuestType.Sub);
		SetDescription("Herbalist Dagna has asked you to gather inner bark from five marked white-leaf trees across Emmet Forest.");
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Herbalist] Dagna", "f_whitetrees_23_1");

		AddObjective("collectBark", "Gather inner bark from marked white-leaf trees",
			new CollectItemObjective(666051, 5));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Normal Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666051,
			character.Inventory.CountItem(666051),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1003.Tree{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1003.Tree{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666051,
			character.Inventory.CountItem(666051),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1003.Tree{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1003.Tree{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Tracks in the Clearings
//-----------------------------------------------------------------------------

public class TracksInTheClearingsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1004);
		SetName("Tracks in the Clearings");
		SetType(QuestType.Sub);
		SetDescription("Scholar Ilara has asked you to observe four Kugheri territory markers scattered through Emmet Forest's clearings.");
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Scholar] Ilara", "f_whitetrees_23_1");

		AddObjective("visitMarkers", "Observe Kugheri territory markers",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_23_1.Quest1004.MarkersVisited", 4, true));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Normal Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_1.Quest1004.MarkersVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1004.Marker{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_1.Quest1004.MarkersVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1004.Marker{i}");
		}
	}
}
