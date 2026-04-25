//--- Melia Script ----------------------------------------------------------
// Grynas Trails - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_katyn_45_1. Trail-country through the dark
// Katyn wood where cairns drift overnight, cloaked Sockets lean between
// trunks, and Stoulet archers pick off travelers who walk one bend too far.
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

public class FKatyn451QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: The Archers on the Bends
		// =====================================================================
		// Trail-Courier Rokas - Stoulet Archers picking off solo travelers
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Trail-Courier] Rokas"), "f_katyn_45_1", -80, -740, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1001);

			dialog.SetTitle(L("Rokas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A broad-shouldered courier leans on a trail-staff and listens to the wood more than he looks at it. A half-healed arrow-scar runs along one forearm*{/}"));
				await dialog.Msg(L("Stoulet Archers are picking off lone walkers at the blind bends. They shoot and fade - you find the body two bends later, and the forest has already started to cover it."));

				var response = await dialog.Select(L("Twenty Brown Stoulet Archers thinned. Take the trails clockwise - they nest east of the central fork. Will you?"),
					Option(L("I'll thin twenty archers"), "help"),
					Option(L("Why fade back into the woods?"), "info"),
					Option(L("Trails aren't my problem"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods once, and relief shows only at the corners of his mouth*{/}"));

						if (await dialog.YesNo(L("Twenty. Hit the east nests first. If an arrow-shaft goes past you from behind - drop prone, don't turn. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Their arrows whistle low. If you hear the whistle twice from the same direction, you're being ranged. Move."));
						}
						break;

					case "info":
						await dialog.Msg(L("The wood hides them. They don't hide themselves. There's a difference, and in Katyn, it matters. The trees here step aside when a Stoulet runs. They never do that for us."));
						break;

					case "leave":
						await dialog.Msg(L("Walk the center-stone route, then. Fewer blind bends. More Socket-cloaks leaning between trunks, but Sockets won't shoot unless you speak first."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArcher", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens again - a longer pause this time - then exhales*{/}"));
					await dialog.Msg(L("Twenty. The bends are quieter for a week. I can run couriers in pairs instead of singles."));
					await dialog.Msg(L("Courier's purse. And a whistle-knot - three sharp tugs means 'archer sighted.' Pass it on or keep it. Either way it earns its weight."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East nests. Drop prone on the second whistle. Twenty."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The bends held clean for a fortnight. Then an arrow whistled at dawn and we were back at it. Katyn is Katyn."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Trail-Book
		// =====================================================================
		// Tracker-Elder Giedre - Trail-book to her apprentice Tautvydas
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Tracker-Elder] Giedre"), "f_katyn_45_1", -400, 180, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1002);

			dialog.SetTitle(L("Giedre"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An elder in patched tracker-leathers ties a leather cord around a small wax-sealed trail-book. Her fingers are steady; her eyes are tired*{/}"));
				await dialog.Msg(L("Forty years I've kept this trail-book. It lists the real paths and the looping paths - which fork leads out, which fork leads you back to yourself three hours later in a different coat."));

				var response = await dialog.Select(L("Carry the book to my apprentice Tautvydas at the south-bend. He's taking the trails over next season. Without the book, he'll learn by walking the looped paths - and not everyone who walks them comes out."),
					Option(L("I'll carry the trail-book"), "help"),
					Option(L("Looped paths? Different coat?"), "info"),
					Option(L("Pass the book yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She presses the book into your hands, then wraps your fingers around it for a moment*{/}"));

						if (await dialog.YesNo(L("South-bend. Tautvydas wears a grey tracker-cord. Don't open the book on the way - the forest reads over your shoulder. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If you reach a fork that isn't on any path you've walked, go back. Don't reason it out. The forest wins arguments."));
						}
						break;

					case "info":
						await dialog.Msg(L("Some trail-forks in Katyn loop back to a point you already passed - but the point has changed. Same stone, different moss. Same tree, different bark. You in the same coat, but the coat isn't quite yours anymore."));
						await dialog.Msg(L("The book marks these forks. Take the other branch, always. Even if the other branch looks wrong, the wrong branch is the right one."));
						break;

					case "leave":
						await dialog.Msg(L("My knees are done. I'd be eight days on a two-hour walk. Someone young carries, or Tautvydas learns the hard way. Your call."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverBook", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She receives Tautvydas's reply-token, reads the short note, and her eyes close for a long moment*{/}"));
					await dialog.Msg(L("He's read the first chapter. Already marked the west-fork as the wrong-branch, which means he trusts the book. That's enough - the trails will have a tracker when I don't."));
					await dialog.Msg(L("Elder's coin. And take this - a tracker-cord of my own. Thin, grey, nothing special. But it frays the way real forest-cord frays, and that helps the wood recognize you as someone who belongs on the path."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("South-bend. Grey cord. Don't open the book on the way."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Tautvydas sent word he marked three new looped forks this week. Already finding routes I missed in forty years. The trails are in good hands."));
			}
		});

		// =====================================================================
		// APPRENTICE-TRACKER TAUTVYDAS (Quest 1002 recipient)
		// =====================================================================
		AddNpc(47245, L("[Apprentice-Tracker] Tautvydas"), "f_katyn_45_1", 710, -580, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1002);

			dialog.SetTitle(L("Tautvydas"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_1.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A young tracker in a grey cord leans against a center-stone, listening to the wood without looking at it. He turns only when you're close enough to speak quietly*{/}"));
					await dialog.Msg(L("{#666666}*He takes the trail-book with both hands, breaks the wax, and reads the first page standing. His eyes sharpen*{/}"));
					await dialog.Msg(L("She finally sent it. Forty years and she sends it in a plain cord."));
					await dialog.Msg(L("{#666666}*He tucks the book into an inner pocket, pulls a small carved token from his tracker-pouch, and inks a single rune on it*{/}"));
					await dialog.Msg(L("Reply-token. West-fork is the looped one - I marked it before I read the chapter. Tell her I guessed right, and I trust the book to tell me what I haven't guessed yet."));

					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_1.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Tautvydas's reply-token received. Return to Tracker-Elder Giedre.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the token back. She'll read the west-fork note by the rune's ink-pattern."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three new loops marked this week. The wood doesn't like losing its ambushes. It'll try a fourth soon. I'm ready."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*The apprentice listens to the wood. He does not offer conversation*{/}"));
				await dialog.Msg(L("Trail-business only. I don't answer questions that don't come with a tracker's token."));
			}
		});

		// =====================================================================
		// QUEST 1003: Socket Hood-Patches
		// =====================================================================
		// Wards-Stitcher Danute - Hood-patches for stitched-silence cloaks
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Wards-Stitcher] Danute"), "f_katyn_45_1", -930, 420, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1003);

			dialog.SetTitle(L("Danute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A spry older woman stitches a grey cloak-lining by lamp-light, her needle moving in a pattern that does not quite repeat. Every third stitch seems to vanish*{/}"));
				await dialog.Msg(L("Socket cloaks dampen sound. You pass a Socket in full stride and hear nothing - not a footstep, not a breath. Strip the hood-patch from a fallen cloak and you get the same quiet, sewn flat into the cloth."));

				var response = await dialog.Select(L("Eight hood-patches. Cut the patch at the seam, not the fabric - fabric cut wrong lets the silence out. Sockets fall only to hand-weapons; arrows slide through them. Will you gather?"),
					Option(L("I'll gather eight hood-patches"), "help"),
					Option(L("Why does silence stitch?"), "info"),
					Option(L("Stitching is stitchers' work"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She pulls a seam-ripper from her apron pocket and passes it to you. The handle is warm*{/}"));

						if (await dialog.YesNo(L("Seam, not fabric. Eight patches. Don't hum while you cut - the stitches remember melodies. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If a Socket steps out while you're cutting, raise your blade and keep cutting. They're slow to decide. Be faster."));
						}
						break;

					case "info":
						await dialog.Msg(L("Old craft. The stitches form a pattern that absorbs sound - don't ask me the theory, I only know the pattern. Wardens who need quiet feet pay for a lined cloak. I sew what keeps them alive."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. My needle is slow and my eyes aren't young. I'll find another gatherer. Maybe. Maybe not."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var patchCount = character.Inventory.CountItem(650853);

				if (patchCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She holds each patch near her cheek, listening, and sets two aside without explanation*{/}"));
					await dialog.Msg(L("Six stitch clean. Two carry a voice - a muttering, nothing you'd catch unless you sewed them. Those I'll boil a week; the voice washes out with hot lye."));
					await dialog.Msg(L("Stitcher's coin. And a small scrap of clean silence-cloth - sewn into a boot-sole, it muffles footfalls enough to slip past a sleeping wolf. Useful trick."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Seam cuts. Eight patches. No humming."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Five warden-cloaks lined and delivered this week. One ranger sent word the silence saved his life at a blind bend. The stitches work."));
			}
		});

		// =====================================================================
		// SOCKET HOOD-PATCH SPOTS
		// =====================================================================
		// For Quest 1003 - Socket Hood-Patches
		// =====================================================================

		void AddPatchSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Fallen Socket Cloak"), "f_katyn_45_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A grey Socket cloak lies crumpled in the leaf-litter. Without a stitcher's order, there's no reason to rip seams*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_1.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The hood-patch is already cut. The rest of the cloak lies undisturbed*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_katyn_45_1.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Socket_Mage_Green, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Socket Mage steps out from between two trunks - you never heard it approach!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Ripping the seam..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(4));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650853, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650853);
					character.ServerMessage(LF("Hood-patches gathered: {0}/8", currentCount));

					if (currentCount >= 8)
					{
						character.ServerMessage(L("{#FFD700}All patches gathered! Return to Wards-Stitcher Danute.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Cutting interrupted. The seam re-knits, slightly."));
				}
			});
		}

		AddPatchSpot(1, -969, -357, 0);
		AddPatchSpot(2, -1331, -474, 90);
		AddPatchSpot(3, -810, -715, 180);
		AddPatchSpot(4, -205, 221, 270);
		AddPatchSpot(5, 334, 245, 0);
		AddPatchSpot(6, -884, -1113, 90);
		AddPatchSpot(7, 599, -577, 180);
		AddPatchSpot(8, 755, 380, 270);

		// =====================================================================
		// QUEST 1004: Which Cairns Walked
		// =====================================================================
		// Path-Warden Lukas - Cairns drifting overnight on the trail-forks
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Path-Warden] Lukas"), "f_katyn_45_1", 270, 430, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1004);

			dialog.SetTitle(L("Lukas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young warden in trail-dust kneels beside a small cairn and sketches its stacked stones. He counts them twice - once looking, once by touch*{/}"));
				await dialog.Msg(L("Four cairns mark the main trail-forks. All four are meant to point the safe way. Three moved overnight last week - pointed walkers into the looped branches. We had two people lost three days each before we caught it."));

				var response = await dialog.Select(L("Walk the four cairns. Tell me which still point where they should point, and which have been turned. Don't re-arrange them - just describe. Will you?"),
					Option(L("I'll walk the four cairns"), "help"),
					Option(L("Who turns cairns at night?"), "info"),
					Option(L("Trail-cairns are trail-warden work"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He tears a sketch from his book, marks four points, and passes it over with a compass-disc no bigger than a coin*{/}"));

						if (await dialog.YesNo(L("Four cairns. Compass at each - note the tip-stone's direction. Safe-way is north-east. Any other direction is turned. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If a cairn points back the way you came - that's not a cairn anymore. That's a trap. Step away from it, don't touch it, and walk out along the opposite heading."));
						}
						break;

					case "info":
						await dialog.Msg(L("I don't know. Could be Sockets - they walk without sound, and cairns don't take long to rebuild. Could be the trees, rearranging their own shortcut-game. Could be something else. I measure. I don't guess."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself over a week. But a week is two more walkers lost. Your call."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var cairnsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", 0);

				if (cairnsChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*He writes each compass-reading, and his pen slows at the last entry*{/}"));
					await dialog.Msg(L("Cairn One: tip-stone north-east. Safe. Cairn Two: tip-stone north-east, correct. Cairn Three: tip-stone due south - turned ninety degrees clockwise. Cairn Four: tip-stone pointing back at me."));
					await dialog.Msg(L("{#666666}*He closes the book and sets it down very carefully*{/}"));
					await dialog.Msg(L("One turned. One trapped. I'll rebuild Three tonight and rope-off Four until a wardmage can walk the spot. Thank you for not touching them."));
					await dialog.Msg(L("Warden's stipend, every coin. And if a cairn ever points at you - walk opposite, don't turn. Same rule for mirrors, in certain woods."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four cairns. Compass on the tip-stone. Note which point where. Don't touch."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cairn Three rebuilt and holding. Cairn Four roped off - the wardmage's visit is scheduled for the new moon. Nobody's walked back-pointed since."));
			}
		});

		// =====================================================================
		// TRAIL CAIRNS
		// =====================================================================
		// For Quest 1004 - Which Cairns Walked
		// =====================================================================

		void AddTrailCairn(int cairnNumber, string cairnName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(cairnName), "f_katyn_45_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A small trail-cairn beside the fork. Stones stacked hand-height. Without a warden's order, there's no reason to measure it*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_1.Quest1004.Cairn{cairnNumber}";
				var checkedCairn = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedCairn)
				{
					await dialog.Msg(L("{#666666}*You have already read this cairn's tip-stone. The reading is logged*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the tip-stone direction..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var cairnsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", cairnsChecked + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Cairns read: {0}/4", cairnsChecked + 1));

					if (cairnsChecked + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All cairns read! Return to Path-Warden Lukas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Reading interrupted. The compass-needle settles."));
				}
			});
		}

		AddTrailCairn(1, "North-Fork Cairn", "North-Fork: tip-stone oriented north-east. Correct. Stones set firm.", -66, 413, 0);
		AddTrailCairn(2, "East-Fork Cairn", "East-Fork: tip-stone oriented north-east. Correct. Moss on the base undisturbed.", 647, 300, 90);
		AddTrailCairn(3, "South-Fork Cairn", "South-Fork: tip-stone oriented due south. Turned ninety degrees clockwise - this cairn sends walkers into the loop.", 330, -595, 180);
		AddTrailCairn(4, "West-Fork Cairn", "West-Fork: tip-stone points back toward the reader. A trap, not a cairn. You step away without touching it.", -435, 437, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Archers on the Bends
//-----------------------------------------------------------------------------

public class TheArchersOnTheBendsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1001);
		SetName("The Archers on the Bends");
		SetType(QuestType.Sub);
		SetDescription("Trail-Courier Rokas needs twenty Brown Stoulet Archers thinned before couriers can run the Katyn trails alone again.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Trail-Courier] Rokas", "f_katyn_45_1");

		AddObjective("killArcher", "Defeat Brown Stoulet Archers on the trail-bends",
			new KillObjective(20, new[] { MonsterId.Stoulet_Bow_Blue }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(21000));
		AddReward(new ItemReward(640084, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10)); // Large HP Potion
		AddReward(new ItemReward(640007, 10)); // Large SP Potion
	}
}

// Quest 1002 CLASS: The Trail-Book
//-----------------------------------------------------------------------------

public class TheTrailBookQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1002);
		SetName("The Trail-Book");
		SetType(QuestType.Sub);
		SetDescription("Tracker-Elder Giedre's forty-year trail-book must reach her apprentice Tautvydas at the south-bend, and his rune-marked reply-token must return.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Tracker-Elder] Giedre", "f_katyn_45_1");

		AddObjective("deliverBook", "Deliver the trail-book and return with the reply-token",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_1.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11));  // Large HP Potion
		AddReward(new ItemReward(640007, 11));  // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Socket Hood-Patches
//-----------------------------------------------------------------------------

public class SocketHoodPatchesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1003);
		SetName("Socket Hood-Patches");
		SetType(QuestType.Sub);
		SetDescription("Wards-Stitcher Danute needs eight hood-patches cut at the seam from fallen Socket cloaks for her stitched-silence cloak-linings.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wards-Stitcher] Danute", "f_katyn_45_1");

		AddObjective("collectPatches", "Cut hood-patches from fallen Socket cloaks",
			new CollectItemObjective(650853, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11));  // Large HP Potion
		AddReward(new ItemReward(640007, 11));  // Large SP Potion
		AddReward(new ItemReward(640012, 3));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650853,
			character.Inventory.CountItem(650853),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650853,
			character.Inventory.CountItem(650853),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Which Cairns Walked
//-----------------------------------------------------------------------------

public class WhichCairnsWalkedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1004);
		SetName("Which Cairns Walked");
		SetType(QuestType.Sub);
		SetDescription("Path-Warden Lukas has asked you to read the four trail-fork cairns and determine which still point north-east and which have been turned overnight.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Path-Warden] Lukas", "f_katyn_45_1");

		AddObjective("readCairns", "Read the four trail-fork cairns",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", 4, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1004.Cairn{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1004.Cairn{i}");
		}
	}
}
