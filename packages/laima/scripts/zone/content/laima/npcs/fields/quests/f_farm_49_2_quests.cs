//--- Melia Script ----------------------------------------------------------
// Shaton Farm - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_farm_49_2 map.
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

public class FFarm492QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Walking Stumps
		// =====================================================================
		// Farmer Andrius - Orange Stumpy Trees invading tomato fields
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Farmer] Andrius"), "f_farm_49_2", 1601, -1158, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1001);

			dialog.SetTitle(L("Andrius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A broad-shouldered farmer rests on the handle of a splintered axe, staring at fresh furrows in his field*{/}"));
				await dialog.Msg(L("You ever seen a tree walk? I have. Now I've seen eighteen. They come at night, uproot themselves, and stroll into the tomato rows like they own them."));

				var response = await dialog.Select(L("My axe breaks on the third swing. These aren't normal trees - the corruption from the demon prison has put roots in their spine. I need someone stronger-armed than me to kill eighteen before next harvest moon."),
					Option(L("I'll kill the walking stumps"), "help"),
					Option(L("Walking trees? Seriously?"), "info"),
					Option(L("That sounds like nonsense"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods grimly and hoists his broken axe*{/}"));

						if (await dialog.YesNo(L("Eighteen should break the migration. They favor the eastern rows where the pollen settles thickest. Will you handle it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Aim for the trunk. The Earth-magic's in the roots, but the mouth - yes, they've mouths - is on the trunk."));
							await dialog.Msg(L("Good boots help. Pendinmire crossed this field last night. You'll want to move fast if you see those tracks."));
						}
						break;

					case "info":
						await dialog.Msg(L("Seriously. Stumpy Tree, they call it. Normally it sits in a forest. This batch roots in my soil, pulls up, walks east, roots again in my field."));
						await dialog.Msg(L("Demon pollen wakes them up. Same pollen Morta warned Jonas about. Now it's here and the trees aren't sleeping anymore."));
						break;

					case "leave":
						await dialog.Msg(L("Come back tomorrow morning. You'll see the new furrows where they walked in overnight."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStump", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He exhales, shoulders dropping*{/}"));
					await dialog.Msg(L("Eighteen. The field's quiet tonight. My tomatoes stand a chance."));
					await dialog.Msg(L("Take these - skirmisher boots, good leather. A farmer who walks fields all day knows good boots when he holds them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Eastern rows, where the pollen settles. Aim for the trunk-mouth."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The stumps have stayed in their forest for three nights running. Long may it last."));
			}
		});

		// =====================================================================
		// QUEST 1002: Jonas's Reply
		// =====================================================================
		// Farmer Jonas-the-Elder - Tomato season reply for Morta
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Farmer] Jonas-the-Elder"), "f_farm_49_2", 407, 466, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1002);

			dialog.SetTitle(L("Jonas-the-Elder"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*The old farmer you met at the Myrkiti warp taps the glass of a sealed greenhouse with a knuckle, listening for cracks*{/}"));
				await dialog.Msg(L("Panes held through the night. Tomatoes are still red, still round, still mine. Morta's warning was worth its weight in gold."));

				var response = await dialog.Select(L("I've written her a proper reply - season forecast, a thank-you, and a standing offer to trade husks for tomatoes every autumn. Courier Benas heads north tomorrow at dawn. Take the letter to him before he sets off?"),
					Option(L("I'll carry it to Benas"), "help"),
					Option(L("Write her yourself?"), "info"),
					Option(L("Find your own courier"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He hands you a thick envelope sealed with dried tomato vine*{/}"));

						if (await dialog.YesNo(L("Benas camps at the Myrkiti warp. Give him this and tell him Jonas-the-Elder sends his regards. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The envelope's tied with vine - that's my seal these days. Morta will know it on sight."));
							await dialog.Msg(L("Mind the walking stumps on the northern path. Andrius was still killing them last I heard."));
						}
						break;

					case "info":
						await dialog.Msg(L("These old hands write slow. Benas writes a full letter in the time I draft three lines. He adds his own news too - weather from up north, who's sick, who's selling what."));
						await dialog.Msg(L("A courier letter is half a newspaper around here. Faster if you can read it, fancier if you can't."));
						break;

					case "leave":
						await dialog.Msg(L("Then I'll wait for next week's run. Morta'll be patient - she always is."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverReply", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*He smiles, a hand resting on the greenhouse frame*{/}"));
					await dialog.Msg(L("Good. Benas has the letter. Morta will have her season forecast by sundown."));
					await dialog.Msg(L("Here - a crate of fresh tomatoes from the saved crop. Take three for yourself. You've earned them twice over."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Benas is at the Myrkiti warp. Ahead of you on the map, north. Go before he leaves."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Morta's husks arrived yesterday. Half my farmhands wear her masks now. We trade like neighbors ought to."));
			}
		});

		// =====================================================================
		// Courier Benas - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(20125, L("[Courier] Benas"), "f_farm_49_2", 1976, 1472, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1002);

			dialog.SetTitle(L("Benas"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A courier checks the weight of his satchel, glancing north along the road*{/}"));
				await dialog.Msg(L("North run tomorrow at dawn. Bring any mail now - I seal the bag at sundown."));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_49_2.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("Jonas's letter is bagged. Morta'll have it by lunch tomorrow."));
				return;
			}

			await dialog.Msg(L("{#666666}*He weighs the envelope, impressed*{/}"));
			await dialog.Msg(L("Vine-seal. That's Jonas-the-Elder's mark, sure enough. Heavy envelope, too - must be a proper season forecast."));
			await dialog.Msg(L("Tell Jonas I'll deliver it first thing. And tell him I'll add my own news - the Baron Allerno steward's health, the weather from the thorn fields. Morta appreciates the full picture."));

			character.Variables.Perm.Set("Laima.Quests.f_farm_49_2.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Letter delivered. Return to Jonas-the-Elder.{/}"));
		});

		// =====================================================================
		// QUEST 1003: Cyst Sap Vials
		// =====================================================================
		// Farmer Ruta - Apothecary-farmer distilling anti-pollen poultices
		//---------------------------------------------------------------------
		AddNpc(157100, L("[Farmer] Ruta"), "f_farm_49_2", 379, -568, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1003);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer with ink-stained fingertips labels tiny glass vials in careful rows*{/}"));
				await dialog.Msg(L("Eglė stitches masks at Myrkiti. I distill what goes between the stitches. Cyst sap, boiled down with river-oil, makes a poultice that stops pollen burn in its tracks."));

				var response = await dialog.Select(L("The Cysts grow on old trees - big, round, pulsing. They don't move; they can't. But they wake up angry when you tap them. I need five clean sap vials. Tap, fill, step back, next."),
					Option(L("I'll gather five vials"), "help"),
					Option(L("Do Cysts really wake up?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you five empty vials, stoppered with wax*{/}"));

						if (await dialog.YesNo(L("One vial per Cyst. Five Cysts spread across the farm. Don't crush the vial - the sap burns through weak glass. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Quarter turn to uncork, press the tip into the sap-pore, quarter turn to seal. Easy enough if you're not panicking."));
							await dialog.Msg(L("About one in five wakes up when tapped. Back away and it'll settle again. Don't try to fight it with the vial in your hand."));
						}
						break;

					case "info":
						await dialog.Msg(L("Oh yes. They're not really dead - just rooted. Tap them gentle and they sleep through it. Tap them wrong and they remember they used to move."));
						await dialog.Msg(L("My grandmother said the old Cysts used to be goblins who fell asleep during a harvest festival. I don't believe her, but I also don't tap rough."));
						break;

					case "leave":
						await dialog.Msg(L("Then breathe shallow if you cross the south fields. My poultices go to those who ask for them, not those who wouldn't."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sapCount = character.Inventory.CountItem(666185);

				if (sapCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She holds a vial to the lamplight, watching the sap glow faintly*{/}"));
					await dialog.Msg(L("Clean. All five. No crushed glass, no spoiled sap, no panicked-tap discoloration. You've done this before, haven't you?"));
					await dialog.Msg(L("Take this - it's honest payment plus an extra vial for your own kit. Pollen burn on the eyes will thank you someday."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five Cysts. Quarter-turn tap. Don't run from the angry ones - just step back."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The poultices are distilled. Every mask at Myrkiti and Shaton has a dab of sap now. The pollen burns less."));
			}
		});

		// =====================================================================
		// CYST GROWTHS
		// =====================================================================
		// For Quest 1003 - Cyst Sap Vials
		// =====================================================================

		void AddCystGrowth(int cystNumber, int x, int z, int direction)
		{
			AddNpc(152011, L("Cyst Growth"), "f_farm_49_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_49_2", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pulsing cyst bulges from a tree trunk, sap glistening at a pore*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_49_2.Quest1003.Cyst{cystNumber}";
				var tapped = character.Variables.Perm.GetBool(variableKey, false);

				if (tapped)
				{
					await dialog.Msg(L("{#666666}*The cyst here is sealed. A fresh bead of sap is already forming, but it belongs to the next season*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_farm_49_2.Quest1003.Cyst{cystNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 20)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Cyst, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}The Cyst wakes up, furious!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Tapping sap..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(666185, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(666185);
					character.ServerMessage(LF("Sap vials filled: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All vials filled! Return to Farmer Ruta.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Tapping interrupted."));
				}
			});
		}

		AddCystGrowth(1, 604, 1253, 0);
		AddCystGrowth(2, -74, -1294, 90);
		AddCystGrowth(3, -1289, -1260, 180);
		AddCystGrowth(4, 62, -969, 270);
		AddCystGrowth(5, -1541, -1416, 0);

		// =====================================================================
		// QUEST 1004: Pendinmire's Range
		// =====================================================================
		// Farmer Vytis - Mapping the apex creature's territory
		//---------------------------------------------------------------------
		AddNpc(20118, L("[Farmer] Vytis"), "f_farm_49_2", 670, -2144, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1004);

			dialog.SetTitle(L("Vytis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A grey-haired farmer measures a massive paw-print with his belt, the flattened grass around it still fresh*{/}"));
				await dialog.Msg(L("Pendinmire. The old hunters said it'd never cross the thorn ridge. The old hunters were wrong."));

				var response = await dialog.Select(L("The Pendinmire has been stalking Shaton's southern border for a month. I need its range mapped before I decide which fields to abandon. Four sign-sites - flattened grass, broken fences, bone piles, claw-marks on trees. Walk them, note them, come back."),
					Option(L("I'll survey the sign-sites"), "help"),
					Option(L("Is it going to attack someone?"), "info"),
					Option(L("That thing could eat a cow"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He hands you a charcoal stick and a folded parchment*{/}"));

						if (await dialog.YesNo(L("Four sites. Move quietly; if you see the Pendinmire itself, drop to the ground and don't move. It has poor eyes for still things. Will you walk them?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("One near the southern woods, one by the broken fence, one at the bone pile, one on the claw-marked oak."));
							await dialog.Msg(L("If you don't come back by sundown, I'll assume the worst and ring the evacuation bell."));
						}
						break;

					case "info":
						await dialog.Msg(L("It ate two oxen last week. Farmhands boarded up the southern barn. Children stay inside after dusk."));
						await dialog.Msg(L("If its range includes the village well, we pack and leave. Simple as that. I need the map to know."));
						break;

					case "leave":
						await dialog.Msg(L("It could eat a cow, yes. It has. Whether it eats a man is what I'm trying to figure out before it does."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sitesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_49_2.Quest1004.SitesVisited", 0);

				if (sitesVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*He spreads your marked map across his knees and traces the Pendinmire's range*{/}"));
					await dialog.Msg(L("The range stops short of the well by a furlong. Good. We stay."));
					await dialog.Msg(L("The fields south of the bone pile - those we abandon. Better to lose a harvest than lose a farmhand."));
					await dialog.Msg(L("Thank you. Take this. A farmer's savings, quietly offered."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four sign-sites. Quiet, slow, low if you see it. Come back by sundown."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The southern fields are fallow this year. The Pendinmire crosses them at dusk. The village stays."));
			}
		});

		// =====================================================================
		// PENDINMIRE SIGN-SITES
		// =====================================================================
		// For Quest 1004 - Pendinmire's Range
		// =====================================================================

		void AddSignSite(int siteNumber, string siteName, string observation, int x, int z, int direction)
		{
			AddNpc(152080, L(siteName), "f_farm_49_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_49_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Disturbed ground marks the passage of something very large*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_49_2.Quest1004.Site{siteNumber}";
				var noted = character.Variables.Perm.GetBool(variableKey, false);

				if (noted)
				{
					await dialog.Msg(L("{#666666}*You've already noted this site*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Noting site..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var sitesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_49_2.Quest1004.SitesVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_49_2.Quest1004.SitesVisited", sitesVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Sign-sites noted: {0}/4", sitesVisited + 1));

					if (sitesVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}Survey complete! Return to Farmer Vytis.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Noting interrupted."));
				}
			});
		}

		AddSignSite(1, "Flattened Grass", "A thirty-pace circle pressed flat. The Pendinmire slept here.", -778, 434, 0);
		AddSignSite(2, "Broken Fence", "Fence posts snapped at the base. One is bitten clean through.", -1245, 674, 90);
		AddSignSite(3, "Bone Pile", "Ox bones, cleaned. Two skulls, shattered where the jaw closed.", 1626, 1147, 180);
		AddSignSite(4, "Claw-Marked Oak", "Claw-marks four palms wide, chest-height. The Pendinmire stands taller than a horse.", 2083, 1411, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Walking Stumps
//-----------------------------------------------------------------------------

public class WalkingStumpsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1001);
		SetName("Walking Stumps");
		SetType(QuestType.Sub);
		SetDescription("Farmer Andrius needs the Orange Stumpy Trees killed before they walk out of the forest and destroy Jonas-the-Elder's tomato crop.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Andrius", "f_farm_49_2");

		AddObjective("killStump", "Defeat Orange Stumpy Trees",
			new KillObjective(18, new[] { MonsterId.Stub_Tree_Orange }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10)); // Large HP Potion
		AddReward(new ItemReward(640007, 10)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
		AddReward(new ItemReward(511148, 1));  // Superior Skirmisher Boots
	}
}

// Quest 1002 CLASS: Jonas's Reply
//-----------------------------------------------------------------------------

public class JonassReplyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1002);
		SetName("Jonas's Reply");
		SetType(QuestType.Sub);
		SetDescription("Jonas-the-Elder's reply to Morta needs to reach Courier Benas before he leaves for Myrkiti at dawn.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Jonas-the-Elder", "f_farm_49_2");

		AddObjective("deliverReply", "Deliver Jonas's reply to Courier Benas",
			new VariableCheckObjective("Laima.Quests.f_farm_49_2.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10)); // Large HP Potion
		AddReward(new ItemReward(640007, 10)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_2.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_2.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Cyst Sap Vials
//-----------------------------------------------------------------------------

public class CystSapVialsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1003);
		SetName("Cyst Sap Vials");
		SetType(QuestType.Sub);
		SetDescription("Farmer Ruta needs five vials of Cyst sap to distill anti-pollen poultices for the masks at Myrkiti and Shaton.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Ruta", "f_farm_49_2");

		AddObjective("collectSap", "Tap Cyst growths for sap vials",
			new CollectItemObjective(666185, 5));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666185,
			character.Inventory.CountItem(666185),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666185,
			character.Inventory.CountItem(666185),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Pendinmire's Range
//-----------------------------------------------------------------------------

public class PendinmiresRangeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1004);
		SetName("Pendinmire's Range");
		SetType(QuestType.Sub);
		SetDescription("Farmer Vytis needs the Pendinmire's territory mapped so Shaton knows which fields to abandon before the apex beast claims them.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Vytis", "f_farm_49_2");

		AddObjective("surveySites", "Note the Pendinmire's sign-sites",
			new VariableCheckObjective("Laima.Quests.f_farm_49_2.Quest1004.SitesVisited", 4, true));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_2.Quest1004.SitesVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1004.Site{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_2.Quest1004.SitesVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1004.Site{i}");
		}
	}
}
