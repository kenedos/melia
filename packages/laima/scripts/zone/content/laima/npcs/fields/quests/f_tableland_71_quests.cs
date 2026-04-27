//--- Melia Script ----------------------------------------------------------
// Grand Yard Mesa Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Grand Yard Mesa (plateau extension beyond Ibre).
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

public class FTableland71QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Purple Ritter Push
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Mesa-Warden] Aistis"), "f_tableland_71", 200, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1001);

			dialog.SetTitle(L("Aistis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Hohen Ritter hold the mesa flats past Ibre. Kill forty and the survey teams can stake rope."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Survey?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. The flats are wide."));
						break;

					case "info":
						await dialog.Msg(L("Mesa extension maps past the last Ibre marker. No stakes means no road."));
						break;

					case "leave":
						await dialog.Msg(L("Mesa stays unmapped."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRitter", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Surveyors riding out at dawn."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First rope-line staked yesterday."));
			}
		});

		// Quest 2: Cronewt Arrow-Fletch
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fletcher] Rasa"), "f_tableland_71", 600, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1002);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Cronewt Bows shoot clean fletchings. Kill twenty-five, bring eight intact fletches for mesa patrols."));

				var response = await dialog.Select(L("Fletches?"),
					Option(L("I'll bring"), "help"),
					Option(L("Patrols?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Keep the vanes flat."));
						break;

					case "info":
						await dialog.Msg(L("Patrols need reach against the Tinys. Cronewt fletches carry."));
						break;

					case "leave":
						await dialog.Msg(L("Patrols stay short-range."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCronewts", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFletches", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight fletches. Patrols get reach."));
					character.Inventory.Remove(650271, character.Inventory.CountItem(650271), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Patrols cleared two ridges this week."));
			}
		});

		// Quest 3: Barkle Bark-Salves
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Herbalist] Milda"), "f_tableland_71", -400, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1003);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Hohen Barkle bark holds salve-resin. Kill twenty, bring six resin-pulls for the field kit."));

				var response = await dialog.Select(L("Resin?"),
					Option(L("I'll bring"), "help"),
					Option(L("Field kit?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Pull slow. The bark tears."));
						break;

					case "info":
						await dialog.Msg(L("Surveyors sleep rough. Salve closes mesa-wind cuts overnight."));
						break;

					case "leave":
						await dialog.Msg(L("Cuts stay open."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBarkles", out var killObj)) return;
				if (!quest.TryGetProgress("gatherResin", out var rObj)) return;

				if (killObj.Done && rObj.Done)
				{
					await dialog.Msg(L("Six pulls. Kit's full."));
					character.Inventory.Remove(650272, character.Inventory.CountItem(650272), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salve holds three days on a mesa-cut."));
			}
		});

		// Quest 4: Tiny Tide
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Ridgeman] Jogaila"), "f_tableland_71", -1000, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1004);

			dialog.SetTitle(L("Jogaila"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A weather-bitten ridgeman folds a length of climbing-rope into a loop, his hands knowing the work without his eyes*{/}"));
				await dialog.Msg(L("I walk the mesa ridges six nights a week, signaling for the survey-camp below. My grandfather walked them before me. A ridgeman knows his cairns the way a sailor knows his stars - they are how we don't fall off the edge in the dark."));
				await dialog.Msg(L("Blue Tinys come in waves through the ridges. Each wave knocks the cairns apart - they roost on the stacked stones, then panic-flock when something startles them and the cairn collapses. Three of my cairns are down; one more wave and I lose my whole night-route."));

				var response = await dialog.Select(L("Kill 60 Blue Tinys to break the wave, then stack the four ridge-cairns they've knocked apart. The cairns are how I navigate the ridge at night. Will you take it?"),
					Option(L("I'll thin the tide and stack the cairns"), "help"),
					Option(L("Why sixty?"), "info"),
					Option(L("Find a ridge-walker"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a small leather pouch of cairn-chinking pebbles*{/}"));
						await dialog.Msg(L("Sixty Tinys, no shortcuts. Watch the footing - the ridges are knife-narrow in places and a flock-panic can throw you off-edge. If you feel a Tiny brush you, drop flat."));
						await dialog.Msg(L("Four cairns to stack. Use the chinking-pebbles between the larger stones - the wind on the mesa shakes loose-stacked stones. Three layers wide at the base, one stone at the cap. Old ridgeman's pattern."));
						break;

					case "info":
						await dialog.Msg(L("Sixty is one Tiny for each year my grandfather walked the ridge. Old ridgeman's count - we measure work in years, not in numbers. He died on the ridge at sixty-one; I plan to walk it longer, but only if the cairns stand."));
						await dialog.Msg(L("Sixty also breaks a wave. The Tinys flock-cycle ends when a third of the wave dies; sixty is roughly a third. Conveniently, my grandfather knew that too."));
						break;

					case "leave":
						await dialog.Msg(L("Ridges stay crowded, cairns stay flat, the night-route closes for the season. I will walk it anyway, by feel, and I will fall off the edge eventually. I would rather not."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTinys", out var killObj)) return;
				if (!quest.TryGetProgress("stackCairns", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("{#666666}*He runs his palm across the chinking on each freshly-stacked cairn, then nods once*{/}"));
					await dialog.Msg(L("Ridges breathe again. All four cairns chinked tight - I tested the cap-stones with a knee and not one moved. I can walk the night-route from here to the south crag and back."));
					await dialog.Msg(L("Take this. Ridgeman's purse, plus my grandfather's old climbing-rope. He'd want it on someone who understood what a stacked cairn is for. So do I."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Sixty Tinys first. The wave hasn't crested - stack a cairn now and the next Tiny-flock will roost on it inside the hour and bring it down again."));
				}
				else
				{
					await dialog.Msg(L("Tide's broken. Now the four cairns - chinking-pebbles between the layers, three-wide base, one-stone cap. The mesa-wind tests every join."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Next wave's a week out and the cairns are holding through three storms already. The survey-camp below has had clean signal-fires every night since you stacked them. The youngest surveyor asked who the ridgeman was now - I said I had a friend who helped. She thinks I made you up."));
			}
		});

		// Ridge-cairn stack points for Quest 1004
		//-------------------------------------------------------------------------
		void AddRidgeCairn(int cairnNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Ridge-Cairn"), "f_tableland_71", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_71", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Loose stones scattered around a half-toppled ridge-cairn*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_71.Quest1004.Cairn{cairnNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already stacked; the cairn stands tall*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Stacking cairn..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_71.Quest1004.CairnsStacked", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_71.Quest1004.CairnsStacked", count);
					character.ServerMessage(LF("Ridge-cairns stacked: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All cairns stacked! Return to Ridgeman Jogaila.{/}"));
				}
				else
				{
					character.ServerMessage(L("Stacking interrupted."));
				}
			});
		}

		AddRidgeCairn(1, -900, -100, 0);
		AddRidgeCairn(2, -1200, -300, 90);
		AddRidgeCairn(3, -800, -400, 180);
		AddRidgeCairn(4, -1100, 100, 270);

		// Quest 5: The Crystal-Warden Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Gediminas"), "f_tableland_71", 1400, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_tableland_71.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Gediminas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter chalks her palms with grip-dust, then flexes both hands until the joints crack*{/}"));
				await dialog.Msg(L("Ritter Alpha wards the rootcrystal vein under the south crag. He's bonded to the vein - sleeps with his back against the crystal, draws strength from it, won't leave it for a single hunter unless the bond's broken first."));
				await dialog.Msg(L("Three crystalline anchor-studs link him to the vein. Chip the studs and the bond snaps; he comes off the stone, fights like a normal Hohen Ritter instead of the warded thing he becomes when the crystal's feeding him."));

				var response = await dialog.Select(L("Chip the three vein-anchors to break his ward, then kill 10 Purple Hohen Ritters to draw him off the vein. He's a normal fight once unbonded. Will you take him?"),
					Option(L("I'll take the Alpha"), "help"),
					Option(L("Why is he bonded to the vein?"), "info"),
					Option(L("That's miner-work, not blade-work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She unhooks a small chisel and hammer from her belt and presses them into your pack*{/}"));
						await dialog.Msg(L("Three studs, three chisel-strikes - one per anchor, struck dead-centre. The studs spider-crack and the bond severs. Don't strike twice; the second strike sends a vein-shock that knocks you flat."));
						await dialog.Msg(L("Then ten Ritters. He comes off the stone around the eighth, slow and stiff because he hasn't moved in months. Strike the joints first - they're rusted from disuse."));
						break;

					case "info":
						await dialog.Msg(L("Some Ritters bond to a vein for life - it's older instinct than their warband culture. The crystal calls and a strong-enough Ritter answers. They become wards rather than warriors; the vein protects them, they protect the vein."));
						await dialog.Msg(L("This vein feeds half the mesa's crystal-pull. With the warden gone, the miners can finally work it - they've been waiting half a year for the bond to break naturally. It won't, not while he's alive."));
						break;

					case "leave":
						await dialog.Msg(L("Vein stays warded, miners stay idle, the survey-camp loses its main contract by autumn. I will keep my chalk-dust and my chisel and wait for the next hand to come through. There usually is one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("chipAnchors", out var nObj)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;

				if (nObj.Done && pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("{#666666}*She watches the miners' wagon-train begin to roll toward the south crag in the distance, and almost grins*{/}"));
					await dialog.Msg(L("Anchors chipped, Alpha down, vein's open. The miners are already moving in - they've been waiting half a year for this moment, and you gave it to them in an afternoon."));
					await dialog.Msg(L("Bounty plus a vein-stipend the survey-camp posted for whoever broke the ward. Drink the first cup for the Ritter; he was a magnificent thing in his way, even if he had to go."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Hohen_Ritter_Purple, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Ritter Alpha emerges from the vein!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else if (!nObj.Done)
				{
					await dialog.Msg(L("Three anchor-studs first. Single chisel-strike each, dead-centre. Don't strike twice or the vein-shock takes you."));
				}
				else
				{
					await dialog.Msg(L("Anchors are chipped, the bond's broken. Now ten Ritters - the Alpha comes off the stone around the eighth, slow and stiff. Strike the joints."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Crystal pulls are cleaner, miners are moving in. The survey-camp posted a banquet in your honour but you weren't there to attend. They drank to you anyway, twice. The lead miner mounted one of the chipped anchor-studs over his bunk."));
			}
		});

		// Vein-anchor chip points for Quest 1005
		//-------------------------------------------------------------------------
		void AddVeinAnchor(int anchorNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Vein-Anchor"), "f_tableland_71", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_71", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A crystalline anchor-stud bonded to the rootcrystal vein*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_71.Quest1005.Anchor{anchorNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already chipped; the bond is broken*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Chipping anchor..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_71.Quest1005.AnchorsChipped", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_71.Quest1005.AnchorsChipped", count);
					character.ServerMessage(LF("Vein-anchors chipped: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All anchors chipped! Now bait out the Alpha.{/}"));
				}
				else
				{
					character.ServerMessage(L("Chipping interrupted."));
				}
			});
		}

		AddVeinAnchor(1, 1300, 400, 0);
		AddVeinAnchor(2, 1500, 600, 90);
		AddVeinAnchor(3, 1200, 700, 180);

		// Quest 6: Mesa Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Survey-Captain] Vilma"), "f_tableland_71", 1300, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1006);

			dialog.SetTitle(L("Vilma"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A survey-captain unrolls a hide-map, points at three crossed-out sections, then weighs the corners with three smooth survey-stones*{/}"));
				await dialog.Msg(L("Survey-camp's expanding the mesa-grid this season. Three sections cleared, three more to clear, twelve more after that. Each section opens only when its sweep is logged - that's how Salvia's survey-office insists we account for the work."));
				await dialog.Msg(L("Three species contest the mesa-grid. Ritters in the open ground, Barkles in the brush-lines, Cronewt Bows on the ridge-rims. Sweep them, plant a survey-flag on the Mesa Survey Stone, and the next section unlocks."));

				var response = await dialog.Select(L("Kill 12 Purple Ritters, 12 Blue Barkles, and 12 Blue Cronewt Bows, then plant a survey-flag in the Mesa Survey Stone to unlock the next section. Take the contract?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why a flag instead of a chalk-line?"), "info"),
					Option(L("Find a survey-hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She unhooks a small canvas survey-flag on a cedar pole and hands it across*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. Survey-camp inspects the kill-grounds on Sunday's grid-walk."));
						await dialog.Msg(L("Flag goes into the socket on the Survey Stone - cedar pole, jam it deep, the canvas catches the wind. Survey-office reads the flag from the camp-tower a half-mile out. No flag, no unlock."));
						break;

					case "info":
						await dialog.Msg(L("Survey-office insists on flag-signaling because chalk fades in the mesa-rain and they don't trust runners. The flag stands until the next sweep takes it down. Bureaucratic, yes; but the system has logged every section the survey-camp ever opened, going back forty years."));
						await dialog.Msg(L("Each unlocked section means more work for the camp, more wages for the surveyors, more taxes for Salvia. The flag-tradition is what keeps the gold flowing."));
						break;

					case "leave":
						await dialog.Msg(L("Then the next section stays unsurveyed and the camp budget contracts. I will find a survey-hand if I have to recruit one from Salvia. Wastes a week of survey-time."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRitter", out var rObj)) return;
				if (!quest.TryGetProgress("killBarkles", out var bObj)) return;
				if (!quest.TryGetProgress("killCronewts", out var cObj)) return;
				if (!quest.TryGetProgress("plantFlag", out var fObj)) return;

				if (rObj.Done && bObj.Done && cObj.Done && fObj.Done)
				{
					await dialog.Msg(L("{#666666}*She turns to the camp-tower behind her, where a watcher has just lowered a small responding-flag in confirmation*{/}"));
					await dialog.Msg(L("Flag spotted. Section unlocked. The surveyors are already drafting the new grid-line - they'll be staking it by tomorrow noon."));
					await dialog.Msg(L("Survey-rate plus a half-purse for the clean flag-form. Some contractors plant the flag at half-mast or back-to-the-wind; you didn't. Survey-office notices."));
					character.Quests.Complete(questId);
				}
				else if (rObj.Done && bObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Sweep done. Now the Mesa Survey Stone - cedar pole, jammed deep, canvas to the wind. The camp-tower watches."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each species, no skipping the Cronewts because they're high. They're high for a reason - that's where the next survey-grid extends."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Survey stakes running past the next ridge already - your unlock paid for the new grid in three days. The lead surveyor named the new section after a swordhand they couldn't quite describe. The map calls it 'Section Twelve, by stranger.' That's you."));
			}
		});

		// Mesa Survey Stone for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Mesa Survey Stone"), "f_tableland_71", 1350, 850, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A flat survey stone, faded survey-marks across its face*{/}"));
				return;
			}

			var plantedKey = "Laima.Quests.f_tableland_71.Quest1006.FlagPlanted";
			if (character.Variables.Perm.GetBool(plantedKey, false))
			{
				await dialog.Msg(L("{#666666}*Your flag stands in the stone-socket*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killRitter", out var rObj)) return;
			if (!quest.TryGetProgress("killBarkles", out var bObj)) return;
			if (!quest.TryGetProgress("killCronewts", out var cObj)) return;

			if (!(rObj.Done && bObj.Done && cObj.Done))
			{
				await dialog.Msg(L("{#666666}*Stone's ready, but the sweep isn't done*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Planting survey-flag..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(plantedKey, true);
				character.ServerMessage(L("{#FFD700}Survey-flag planted. Return to Survey-Captain Vilma.{/}"));
			}
			else
			{
				character.ServerMessage(L("Planting interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland71Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1001);
		SetName(L("Purple Ritter Push"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Hohen Ritter holding the mesa flats past Ibre."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mesa-Warden] Aistis"), "f_tableland_71");

		AddObjective("killRitter", L("Kill Purple Hohen Ritter"),
			new KillObjective(40, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland71Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1002);
		SetName(L("Cronewt Arrow-Fletch"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Cronewt Bows and bring intact fletches for mesa patrols."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Rasa"), "f_tableland_71");

		AddObjective("killCronewts", L("Kill Blue Cronewt Bows"),
			new KillObjective(25, new[] { MonsterId.Cronewt_Bow_Blue }));

		AddObjective("gatherFletches", L("Gather intact fletches"),
			new CollectItemObjective(650271, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650271, character.Inventory.CountItem(650271), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650271, character.Inventory.CountItem(650271), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland71Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1003);
		SetName(L("Barkle Bark-Salves"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Hohen Barkle and bring resin-pulls for the field kit."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Milda"), "f_tableland_71");

		AddObjective("killBarkles", L("Kill Blue Hohen Barkle"),
			new KillObjective(20, new[] { MonsterId.Hohen_Barkle_Blue }));

		AddObjective("gatherResin", L("Gather resin-pulls"),
			new CollectItemObjective(650272, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650272, character.Inventory.CountItem(650272), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650272, character.Inventory.CountItem(650272), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland71Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1004);
		SetName(L("Tiny Tide"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Blue Tiny swarm along the mesa ridges."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ridgeman] Jogaila"), "f_tableland_71");

		AddObjective("killTinys", L("Kill Blue Tinys"),
			new KillObjective(60, new[] { MonsterId.Tiny_Blue }));

		AddObjective("stackCairns", L("Stack the four ridge-cairns"),
			new VariableCheckObjective("Laima.Quests.f_tableland_71.Quest1004.CairnsStacked", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1004.CairnsStacked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_71.Quest1004.Cairn{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1004.CairnsStacked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_71.Quest1004.Cairn{i}");
	}
}

public class FTableland71Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1005);
		SetName(L("The Ritter Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Hohen Ritter to draw out the Alpha warding the rootcrystal vein."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Gediminas"), "f_tableland_71");

		AddObjective("chipAnchors", L("Chip the three vein-anchors"),
			new VariableCheckObjective("Laima.Quests.f_tableland_71.Quest1005.AnchorsChipped", 3, true));

		AddObjective("killPack", L("Kill Purple Hohen Ritter"),
			new KillObjective(10, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddObjective("killAlpha", L("Defeat the Ritter Alpha"),
			new KillObjective(1, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1005.AnchorsChipped");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_71.Quest1005.Anchor{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1005.AnchorsChipped");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_71.Quest1005.Anchor{i}");
	}
}

public class FTableland71Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1006);
		SetName(L("Mesa Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Purple Ritter, Blue Barkle, and Blue Cronewt Bows."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Survey-Captain] Vilma"), "f_tableland_71");

		AddObjective("killRitter", L("Kill Purple Hohen Ritter"),
			new KillObjective(12, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddObjective("killBarkles", L("Kill Blue Hohen Barkle"),
			new KillObjective(12, new[] { MonsterId.Hohen_Barkle_Blue }));

		AddObjective("killCronewts", L("Kill Blue Cronewt Bows"),
			new KillObjective(12, new[] { MonsterId.Cronewt_Bow_Blue }));

		AddObjective("plantFlag", L("Plant a survey-flag on the Mesa Survey Stone"),
			new VariableCheckObjective("Laima.Quests.f_tableland_71.Quest1006.FlagPlanted", 1, true));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1006.FlagPlanted");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1006.FlagPlanted");
	}
}
