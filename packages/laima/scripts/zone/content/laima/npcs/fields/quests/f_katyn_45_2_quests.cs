//--- Melia Script ----------------------------------------------------------
// Grynas Training Camp - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_katyn_45_2. An eerie dark-forest tract where
// trees twist into watching faces, knot-eyes follow travelers, and distant
// noises carry from nowhere in particular.
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

public class FKatyn452QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: The Circling Ridimed
		// =====================================================================
		// Woodwarden Marius - Ridimed spiraling strangely around lone travelers
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Woodwarden] Marius"), "f_katyn_45_2", -20, 540, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1001);

			dialog.SetTitle(L("Marius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A hollow-eyed warden leans against a trunk whose bark has puckered into something like a face. He does not lean close to it*{/}"));
				await dialog.Msg(L("Ridimed don't pack-hunt. Never have. This month they're circling lone walkers in slow spirals - inward, always inward - and the trees... hear them. You can tell. The knots turn."));

				var response = await dialog.Select(L("Twenty Ridimed put down. Break the spirals before whatever they're rehearsing is finished. Will you?"),
					Option(L("I'll put down twenty Ridimed"), "help"),
					Option(L("Rehearsing for what?"), "info"),
					Option(L("The forest isn't my problem"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He does not smile. His relief is only in the shoulders easing*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("If you hear breathing behind you and nothing's there - walk, don't run. Running is what the trees seem to want."));
						await dialog.Msg(L("And don't answer if something calls your name in a voice you know. Nothing in this wood should know your name."));
						break;

					case "info":
						await dialog.Msg(L("Old foresters say the Ridimed copy what the wood is teaching them. When they spiral, something is winding something else up. I don't know what. I don't want to be here when we find out."));
						break;

					case "leave":
						await dialog.Msg(L("Walk the edge-paths, then. Middle of the wood is where the spirals form. Edge is safer by a margin."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRidimed", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens, tilting his head toward the forest, and only then exhales*{/}"));
					await dialog.Msg(L("The wood's quieter. Not quiet - quieter. I'll take what I can get."));
					await dialog.Msg(L("Warden's coin. Keep the salt-pouch too - sprinkle it on your boot-soles before you sleep in a Katyn wood. Old trick, still works."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Twenty. Don't stop in the center of a spiral."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The knots stopped turning for three nights. Then started again, slower. We count that a win in this wood."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Warding Charm
		// =====================================================================
		// Shrine-Keeper Nijole - Ward delivery to the Deep Shrine
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Shrine-Keeper] Nijole"), "f_katyn_45_2", 170, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1002);

			dialog.SetTitle(L("Nijole"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A quiet-voiced keeper braids red thread through a small bone-and-wax charm. Her fingers do not shake, but they do not stop, either*{/}"));
				await dialog.Msg(L("The Deep Shrine's ward-thread snapped on the new moon. My sister keeps that shrine - eight days' silence since. The charm I'm finishing replaces the thread."));

				var response = await dialog.Select(L("Carry this charm to the Deep Shrine, south of the watching-oak. My sister Dalia will answer if she hears the charm's bells. Bring back her ward-token so I know the replacement took."),
					Option(L("I'll carry the charm"), "help"),
					Option(L("Why hasn't she sent word?"), "info"),
					Option(L("Shrine business stays with shrines"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She ties the last knot, tests the weight, and presses the charm into your palm. The bells chime once - small, high, clean*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("If a tree seems to lean toward you, keep your eyes on the path. Leaning trees test for eye-contact. Deny it."));
						await dialog.Msg(L("Dalia will be at the shrine-stone. If she isn't, leave the charm on the stone and come back to me. Do not search."));
						break;

					case "info":
						await dialog.Msg(L("Because the ward-thread snapped. Without it, the shrine-stone absorbs messages instead of passing them on. She's probably speaking into nothing and wondering why no one hears her."));
						break;

					case "leave":
						await dialog.Msg(L("Then my sister sits in silence another night. I'll finish a second charm. But the wood moves slower than my fingers."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverCharm", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She receives the ward-token, turns it in her fingers, and lets out a breath she'd been holding for eight days*{/}"));
					await dialog.Msg(L("Her token. Warm. She's alive, and the ward took. That's all I needed to know."));
					await dialog.Msg(L("Keeper's coin. And a strand of charm-thread - doesn't make you a shrine, but it keeps small things from speaking to you at night."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("South, past the watching-oak. Deep Shrine. Don't ring the bells on the path."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Dalia sends daily word again. Eight days silent, eight days speaking. The wood knows the ward is back; the spirals slowed around her shrine."));
			}
		});

		// =====================================================================
		// DEEP-SHRINE DALIA (Quest 1002 recipient)
		// =====================================================================
		AddNpc(20114, L("[Deep-Shrine] Dalia"), "f_katyn_45_2", 1500, -1000, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1002);

			dialog.SetTitle(L("Dalia"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_2.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A thin woman in shrine-grey stands very still beside a waist-high stone. The stone is warm to the eye, as if it breathes. She does not turn when you approach - she listens first*{/}"));
					await dialog.Msg(L("The bells. Nijole's work. You came."));
					await dialog.Msg(L("{#666666}*She takes the charm, binds it to the shrine-stone with the red thread, and the stone... settles. A pressure you hadn't noticed lifts*{/}"));
					await dialog.Msg(L("Eight days I spoke into stone that swallowed the words. Now the stone carries them again."));
					await dialog.Msg(L("{#666666}*She presses a ward-token into your hand - small, bone, warm*{/}"));
					await dialog.Msg(L("Carry this to Nijole. She'll know by the warmth the ward took. And thank you, traveler. The wood noticed when I went silent. It will notice that I'm back."));

					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_2.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Dalia's ward-token received. Return to Shrine-Keeper Nijole.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the token. Nijole waits. The wood listens, but it is not urgent."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The shrine-stone speaks clean again. I hear Nijole at morning and midnight. The spirals thin where I stand."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*She does not turn. Her voice is low, measured, careful*{/}"));
				await dialog.Msg(L("The shrine-stone accepts no visitors without a reason. Walk softly where you walk."));
			}
		});

		// =====================================================================
		// QUEST 1003: Bark from the Watching Trees
		// =====================================================================
		// Dream-Herbalist Egle - Twisted bark-strips for dream-sight draughts
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Dream-Herbalist] Egle"), "f_katyn_45_2", -50, -310, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1003);

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A wiry herbalist sifts bark-curls through her fingers, holding each one up to a weak shaft of forest-light. Old Pappus-Kepas shuffle at the treeline behind her, watching*{/}"));
				await dialog.Msg(L("Twisted bark from the watching-trees. The ones whose knots look back at you. Boiled with moonwater, it gives dream-sight - useful work, if you can stand the dreams."));

				var response = await dialog.Select(L("Eight strips. Only from trees whose eye-knots are closed - never peel an open knot. The tree remembers who strips it when it's looking. Will you gather for me?"),
					Option(L("I'll gather eight bark-strips"), "help"),
					Option(L("What do the dreams show?"), "info"),
					Option(L("Dream-work is dangerous"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She pulls a small curved bark-knife from her apron and offers it hilt-first*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("If a knot opens while you cut, walk away. Return to that tree never. The wood keeps a grudge with very slow teeth."));
						await dialog.Msg(L("And don't hum while you work. The bark remembers melodies."));
						break;

					case "info":
						await dialog.Msg(L("The dreamer sees what the wood saw while the bark grew. Pleasant, some nights. Unpleasant, most. Once a year, something stares back through the dream. I don't drink the draught myself anymore."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. Safer not to know what the wood sees. I'll find another gatherer, or do without."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var barkCount = character.Inventory.CountItem(650851);

				if (barkCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She inspects each strip, turning them to catch the light, and sets four aside without comment*{/}"));
					await dialog.Msg(L("Eight clean strips. Four will steep cleanly; four carried a watcher's weight, but they'll cure if I salt them a week. Honest work."));
					await dialog.Msg(L("Herbalist's share. And a vial of salt-water - useful when you notice something following that shouldn't be."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Closed knots only. Shallow cuts. Eight strips."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The draught steeped well. I sold seven measures and kept one back for dreams I never plan to drink. Good month."));
			}
		});

		// =====================================================================
		// BARK-STRIP SPOTS
		// =====================================================================
		// For Quest 1003 - Bark from the Watching Trees
		// =====================================================================

		void AddBarkSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Watching Tree"), "f_katyn_45_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_2", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A twisted trunk with knots that suggest closed eyes. Without a herbalist's order, there's no reason to cut*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_2.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*This tree has been cut shallow - a pale scar where the strip came away. The knots remain closed, for now*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_katyn_45_2.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Pappus_Kepa_Purple, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Black Old Kepa steps out from behind the trunk - as if it had always been there!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Cutting a bark-strip, shallow..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(4));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650851, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650851);
					character.ServerMessage(LF("Bark-strips gathered: {0}/8", currentCount));

					if (currentCount >= 8)
					{
						character.ServerMessage(L("{#FFD700}All strips gathered! Return to Dream-Herbalist Egle.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Cutting interrupted."));
				}
			});
		}

		AddBarkSpot(1, -190, -489, 0);
		AddBarkSpot(2, -513, -998, 90);
		AddBarkSpot(3, 199, -1052, 180);
		AddBarkSpot(4, 742, -519, 270);
		AddBarkSpot(5, 878, -277, 0);
		AddBarkSpot(6, 461, 126, 90);
		AddBarkSpot(7, -149, 318, 180);
		AddBarkSpot(8, 1286, -135, 270);

		// =====================================================================
		// QUEST 1004: Which Knot-Eyes Are Open
		// =====================================================================
		// Forest-Scholar Audrius - Surveying which watching-trees are actively looking
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Forest-Scholar] Audrius"), "f_katyn_45_2", 840, -10, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1004);

			dialog.SetTitle(L("Audrius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young scholar in shrine-dyed grey kneels beside a stump, sketching a knot that looks disquietingly like a half-closed eye. He sketches without looking down at the page*{/}"));
				await dialog.Msg(L("Old foresters talk about 'watching-trees' as folklore. My work says it isn't. Some knots open at night and close by morning - and four specific trees in this stretch have begun keeping their knots open through the day."));

				var response = await dialog.Select(L("Walk the four marked trees. Tell me whether each knot is closed, weeping sap, or - the worst case - actively tracking movement. Don't stare back. Will you?"),
					Option(L("I'll walk the four trees"), "help"),
					Option(L("What does 'tracking' mean?"), "info"),
					Option(L("I'm not here for trees"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He tears the sketch-page loose, marks four points with a charcoal nub, and does not quite meet your eyes when handing it over*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Closed knot: grain still, tight ring. Weeping sap: grain still, clear bead of sap. Tracking: grain ripples toward your hand, slow, like a pupil focusing."));
						await dialog.Msg(L("If one tracks you, leave immediately. Do not return alone. Do not return even with company."));
						break;

					case "info":
						await dialog.Msg(L("Tracking means the tree has enough of something - awareness, memory, I don't know - to follow you across a gesture. Dormant wood doesn't do that. Active wood does. We don't want this wood to be active."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself over a month, one per week. Slower, but my eyes still work. For now."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var treesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", 0);

				if (treesChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*He writes each observation without looking up, and his pen slows at the last entry*{/}"));
					await dialog.Msg(L("Tree One: closed, grain still. Tree Two: weeping sap. Tree Three: weeping sap, grain shifted faintly. Tree Four: tracked your hand across a full arc. Full tracking, by daylight."));
					await dialog.Msg(L("{#666666}*He sets the pen down and does not pick it up for a long moment*{/}"));
					await dialog.Msg(L("One tree out of four is fully active. That's above the threshold my paper called 'folklore.' I'll forward this to Fedimian tonight and ask for a wardmage visit before the count goes to two."));
					await dialog.Msg(L("Scholar's stipend, every coin I can spare. Thank you. I'll mark the fourth tree for a keep-clear circle."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four trees. Wave past each knot. Closed, weeping, tracking. Don't stare back."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The wardmage set a circle around the active tree. It closed its knot for a week and re-opened. We watch. We do not stare."));
			}
		});

		// =====================================================================
		// WATCHING-TREE LANDMARKS
		// =====================================================================
		// For Quest 1004 - Which Knot-Eyes Are Open
		// =====================================================================

		void AddKnotTree(int treeNumber, string treeName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(treeName), "f_katyn_45_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A marked tree. Its knots suggest eyes, but without a scholar's order, there's no reason to wave past them*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_2.Quest1004.Tree{treeNumber}";
				var checkedTree = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedTree)
				{
					await dialog.Msg(L("{#666666}*You have already read this tree's knot. You do not wish to read it twice*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Waving past the knot..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var treesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", treesChecked + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Trees read: {0}/4", treesChecked + 1));

					if (treesChecked + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All trees read! Return to Forest-Scholar Audrius.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Reading interrupted. You step back a pace."));
				}
			});
		}

		AddKnotTree(1, "West Watching-Tree", "West Tree: knot closed tight, grain still. Dormant.", -299, 1111, 90);
		AddKnotTree(2, "North Watching-Tree", "North Tree: knot ringed by a fresh bead of sap. Grain still, but the sap is new.", 502, 790, 180);
		AddKnotTree(3, "East Watching-Tree", "East Tree: sap beading and grain shifted half an inch sideways as your hand passed. Partial track.", 1120, 10, 270);
		AddKnotTree(4, "Deep Watching-Tree", "Deep Tree: grain followed your hand across a full arc. The knot remained open after you lowered your arm.", 595, -1781, 0);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Circling Ridimed
//-----------------------------------------------------------------------------

public class TheCirclingRidimedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1001);
		SetName("The Circling Ridimed");
		SetType(QuestType.Sub);
		SetDescription("Woodwarden Marius needs twenty Blue Ridimed put down before their strange inward spirals complete whatever the wood is rehearsing.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Woodwarden] Marius", "f_katyn_45_2");

		AddObjective("killRidimed", "Defeat Blue Ridimed breaking the spirals",
			new KillObjective(20, new[] { MonsterId.Ridimed_Blue }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1));  // Recovery Potion
	}
}

// Quest 1002 CLASS: The Warding Charm
//-----------------------------------------------------------------------------

public class TheWardingCharmQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1002);
		SetName("The Warding Charm");
		SetType(QuestType.Sub);
		SetDescription("Shrine-Keeper Nijole's warding charm must reach her sister Dalia at the Deep Shrine, and Dalia's ward-token must return to confirm the replacement took.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Shrine-Keeper] Nijole", "f_katyn_45_2");

		AddObjective("deliverCharm", "Deliver the charm and return with the ward-token",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_2.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2));  // Large HP Potion
		AddReward(new ItemReward(640007, 2));  // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Bark from the Watching Trees
//-----------------------------------------------------------------------------

public class BarkFromTheWatchingTreesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1003);
		SetName("Bark from the Watching Trees");
		SetType(QuestType.Sub);
		SetDescription("Dream-Herbalist Egle needs eight twisted bark-strips cut shallow from closed-knot watching-trees for her dream-sight draughts.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Dream-Herbalist] Egle", "f_katyn_45_2");

		AddObjective("collectBark", "Gather twisted bark-strips from closed-knot trees",
			new CollectItemObjective(650851, 8));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2));  // Large HP Potion
		AddReward(new ItemReward(640007, 2));  // Large SP Potion
		AddReward(new ItemReward(640012, 1));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650851,
			character.Inventory.CountItem(650851),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650851,
			character.Inventory.CountItem(650851),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Which Knot-Eyes Are Open
//-----------------------------------------------------------------------------

public class WhichKnotEyesAreOpenQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1004);
		SetName("Which Knot-Eyes Are Open");
		SetType(QuestType.Sub);
		SetDescription("Forest-Scholar Audrius has asked you to survey four marked watching-trees and determine which knot-eyes are dormant, weeping, or actively tracking movement.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Forest-Scholar] Audrius", "f_katyn_45_2");

		AddObjective("readTrees", "Read the four watching-tree knot-eyes",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", 4, true));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1004.Tree{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1004.Tree{i}");
		}
	}
}
