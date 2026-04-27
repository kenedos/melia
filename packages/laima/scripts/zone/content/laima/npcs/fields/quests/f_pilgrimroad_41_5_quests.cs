//--- Melia Script ----------------------------------------------------------
// Ouaas Memorial Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Ouaas Memorial pilgrim road.
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

public class FPilgrimroad415QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Brown Nuka Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Pilgrim-Warden] Mindaugas"), "f_pilgrimroad_41_5", -250, -50, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Nukas swarm Ouaas's road. Forty kills and the pilgrims pass clean."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Pilgrims?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. Watch their charge."));
						break;

					case "info":
						await dialog.Msg(L("Salvia faithful walk this road to the memorial. Nukas trample them."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays unsafe."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNukas", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Pilgrims cross the road tonight."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First caravan reached the memorial."));
			}
		});

		// Quest 2: Fire Hardened Arrowheads
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Fletcher] Algirdas"), "f_pilgrimroad_41_5", 1100, 200, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1002);

			dialog.SetTitle(L("Algirdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Lapasape archers carry fire-hardened heads. Kill twenty-eight, bring seven heads."));

				var response = await dialog.Select(L("Heads?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Heads only. Shafts splinter."));
						break;

					case "info":
						await dialog.Msg(L("Refit them for the memorial guard. Cheaper than forging."));
						break;

					case "leave":
						await dialog.Msg(L("Guard runs short."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLapasape", out var killObj)) return;
				if (!quest.TryGetProgress("gatherHeads", out var hObj)) return;

				if (killObj.Done && hObj.Done)
				{
					await dialog.Msg(L("Seven heads. Quivers refilled."));
					character.Inventory.Remove(650756, character.Inventory.CountItem(650756), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Memorial guard fully shafted."));
			}
		});

		// Quest 3: Yellow-Eyed Petals
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Memorial-Tender] Birute"), "f_pilgrimroad_41_5", -300, 870, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1003);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Elma bloom yellow-eyed petals along Ouaas's verge. Kill eighteen, gather six petals."));

				var response = await dialog.Select(L("Petals?"),
					Option(L("I'll gather"), "help"),
					Option(L("Use?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Press them flat. Don't crush."));
						break;

					case "info":
						await dialog.Msg(L("Memorial offering. Ouaas favored yellow."));
						break;

					case "leave":
						await dialog.Msg(L("Altar stays bare."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killElma", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPetals", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Six petals. Altar dressed at dusk."));
					character.Inventory.Remove(661076, character.Inventory.CountItem(661076), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep gathering."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pilgrims wept at the offering."));
			}
		});

		// Quest 4: Deadbornscab Archers
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Road-Marshal] Kestutis"), "f_pilgrimroad_41_5", 700, 380, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1004);

			dialog.SetTitle(L("Kestutis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A road-marshal pulls a black-fletched arrow from a sandbag-target and tosses it onto a growing pile*{/}"));
				await dialog.Msg(L("Three pilgrims this week. Two foot-pilgrims, one barefoot widow walking the memorial circuit. All three shot from above before they ever saw the archer."));
				await dialog.Msg(L("Deadbornscab nest in the cliff-roosts above the bend. They wait for shadow under the rim, then volley-shoot when a pilgrim walks the open stretch. We've never had an archer-problem this bad on Ouaas's road."));

				var response = await dialog.Select(L("Kill 18 Deadbornscab archers, then spike the four cliff-roosts shut with the iron tacks at the cliff-base. They can't re-nest in spiked wood. Will you climb?"),
					Option(L("I'll kill them and spike the roosts"), "help"),
					Option(L("Why now? Why this season?"), "info"),
					Option(L("Cliffs aren't my work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you an iron-tack pouch heavy enough to anchor a small cart*{/}"));
						await dialog.Msg(L("Eighteen archers, no shortcuts. Kill them in the open if you can - on the cliff, the wind throws every shot."));
						await dialog.Msg(L("Then the four roosts. Drive a tack through the perch-wood at four points, then a fifth at the entrance. Spiked wood's poison to nesting Deadbornscab; they won't re-roost for a year."));
						break;

					case "info":
						await dialog.Msg(L("Memorial-week. Salvia's pilgrims come thicker than usual to lay flowers for Ouaas, and the Deadbornscab know it. They feed on the panic as much as the kill."));
						await dialog.Msg(L("If we can clear the bend before the second-week processions, the rest of the memorial passes safe. If not, the widow-count doubles."));
						break;

					case "leave":
						await dialog.Msg(L("Then the cliff-bend stays a death-stretch and the widows multiply. I'll be here, pulling arrows out of straw and pretending it's enough."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("spikeRoosts", out var rObj)) return;

				if (killObj.Done && rObj.Done)
				{
					await dialog.Msg(L("{#666666}*He weighs the empty tack-pouch in his hand and almost smiles*{/}"));
					await dialog.Msg(L("Cliffs silent. The four roosts spiked clean through. The bend will hold for at least a season - maybe two."));
					await dialog.Msg(L("This is marshal's coin, plus the memorial-week bonus from Salvia. The widow whose husband was the third kill - she sent word for me to thank whoever did this. So: thank you."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Eighteen archers first. Roosts can wait until they stop shooting - no point spiking wood while a Deadbornscab still draws on you from above."));
				}
				else
				{
					await dialog.Msg(L("Cliffs are quiet. Now the four roosts - five tacks each, perch and entry. The poison in the iron does the rest."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The second-week procession passed under the bend with no losses. First time in years. The widow you avenged - she walked at the front of the line, barefoot, like she meant to be seen."));
			}
		});

		// Archer roost spike points for Quest 1004
		//-------------------------------------------------------------------------
		void AddArcherRoost(int roostNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Archer Roost"), "f_pilgrimroad_41_5", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_5", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An empty archer roost cut into the cliff wood*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_41_5.Quest1004.Roost{roostNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already spiked through with iron tacks*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Spiking roost..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_41_5.Quest1004.RoostsSpiked", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_41_5.Quest1004.RoostsSpiked", count);
					character.ServerMessage(LF("Roosts spiked: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All roosts spiked! Return to Road-Marshal Kestutis.{/}"));
				}
				else
				{
					character.ServerMessage(L("Spiking interrupted."));
				}
			});
		}

		AddArcherRoost(1, 600, 500, 0);
		AddArcherRoost(2, 800, 200, 90);
		AddArcherRoost(3, 900, 600, 180);
		AddArcherRoost(4, 500, 700, 270);

		// Quest 5: The Trampler
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Hunter] Vytenis"), "f_pilgrimroad_41_5", -850, -400, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1005);
			var bossSpawnedKey = "Laima.Quests.f_pilgrimroad_41_5.Quest1005.BossSpawned";

			dialog.SetTitle(L("Vytenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A hunter kneels in the road-dust, sketching hoof-shapes onto a slab of slate with a charcoal stick*{/}"));
				await dialog.Msg(L("Bull Nuka. Twice the size of his herd, scarred along the flank from a wagon-pole he charged through last spring. Pilgrims call him the Trampler - the carriages he breaks aren't always empty when he hits them."));
				await dialog.Msg(L("I've chalked three of his tracks along the road. Read each one - the splay tells me which way his herd's drifting next. After that we kill enough of his cows to draw him out."));

				var response = await dialog.Select(L("Read the three trampling-tracks I chalked, then kill 10 Brown Nukas to bait the bull. He charges straight - dodge low, strike the flank. Will you hunt him?"),
					Option(L("I'll read the tracks and bait him"), "help"),
					Option(L("Why read tracks before the kill?"), "info"),
					Option(L("That's not my work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He passes you the slate and a stub of charcoal*{/}"));
						await dialog.Msg(L("Three tracks: north verge, south crook, east scree. Kneel by each, sketch the splay-angle on the slate. The deeper hoof tells you which way he's leaning when he runs."));
						await dialog.Msg(L("Then ten Nukas. He'll charge from upwind once the herd thins. Don't try to outrun him on flat ground - cut sideways, let him commit, then strike the flank."));
						break;

					case "info":
						await dialog.Msg(L("Salvia's hunter-guild keeps a herd-drift log going back ninety years. Knowing where the Trampler's herd is heading next is half of how we keep the road open after we kill him."));
						await dialog.Msg(L("The other half is making sure the cows don't elect a worse bull from his bloodline. The tracks tell us which yearling's been following close. That's the next bull - we watch him, not hunt him."));
						break;

					case "leave":
						await dialog.Msg(L("Then the Trampler keeps trampling and the slate stays half-sketched. I'll be here, drawing tracks I can't reach."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readTracks", out var tObj)) return;
				if (!quest.TryGetProgress("killHerd", out var hObj)) return;
				if (!quest.TryGetProgress("killBull", out var bObj)) return;

				if (tObj.Done && hObj.Done && bObj.Done)
				{
					await dialog.Msg(L("{#666666}*He copies your sketches into a leather log-book, marks the date, then closes it with a snap*{/}"));
					await dialog.Msg(L("Bull's down, herd-drift logged. Salvia's hunter-guild has the next ninety years of road-data, courtesy of you."));
					await dialog.Msg(L("Hunter's coin and a flask of memorial-mead. The mead's traditional - first cup goes to the bull. The second is yours."));
					character.Variables.Perm.Remove(bossSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (hObj.Done && !bObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(bossSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(bossSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Nuka_Brown, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He charges!"));
							character.ServerMessage(L("{#FF9966}The bull Nuka thunders down the road!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else if (!tObj.Done)
				{
					await dialog.Msg(L("Three chalked tracks first. Splay-angle on each, sketched on the slate. The bull moves through that drift-line; we miss it, we miss the next one."));
				}
				else
				{
					await dialog.Msg(L("Tracks logged. Ten Nukas now - he won't bait for less, and the cows reform a herd if you stop short."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Herd's broken into three small drifts, each one led by a younger cow. The yearling I had my eye on - the next bull - he stayed with the south drift. We watch him from here on, but we don't hunt him. That's the contract with the herd."));
			}
		});

		// Trampling track read points for Quest 1005
		//-------------------------------------------------------------------------
		void AddTramplingTrack(int trackNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Trampling Track"), "f_pilgrimroad_41_5", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_5", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A churned-up patch of road, hooves the size of saucepans*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_41_5.Quest1005.Track{trackNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already read and noted in your tracking-tally*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading track..."), "Cancel", "SITREAD", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_41_5.Quest1005.TracksRead", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_41_5.Quest1005.TracksRead", count);
					character.ServerMessage(LF("Tracks read: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All tracks read! Now bait out the bull.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddTramplingTrack(1, -750, -500, 0);
		AddTramplingTrack(2, -900, -300, 90);
		AddTramplingTrack(3, -650, -200, 180);

		// Quest 6: Pilgrim Road Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Gediminas"), "f_pilgrimroad_41_5", 450, -300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1006);

			dialog.SetTitle(L("Gediminas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A militia-captain marks tomorrow's date on a calendar of memorial days, then folds it small*{/}"));
				await dialog.Msg(L("Memorial-week is the busiest week we have. More pilgrims, more monsters, more carriages, more widows if we don't run a sweep before the second-week processions."));
				await dialog.Msg(L("Salvia priests don't pay bounty on bare hunter's word during memorial-week. They want it offered - lantern lit at Ouaas's altar, count given to the dead before the living. Old custom."));

				var response = await dialog.Select(L("Kill 12 Brown Nukas, 12 Brown Lapasape, and 12 Deadbornscab archers, then light the Memorial-Lantern at Ouaas's altar to consecrate the count. Will you take the sweep?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why offer the count to the dead?"), "info"),
					Option(L("That's priest-work mixed with hunter-work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She unfastens a wax-sealed wick from her belt and presses it into your hand*{/}"));
						await dialog.Msg(L("Thirty-six kills, then the lantern. The altar's on the verge above the cliff-bend - bronze fixture, wick-rest already cut. Use this wax-wick, not your own."));
						await dialog.Msg(L("Recite Ouaas's short verse if you know it. If you don't, just say the count out loud. The priests count the dead with you that way."));
						break;

					case "info":
						await dialog.Msg(L("Memorial-week is for the pilgrims who didn't make it to Ouaas's altar in past years. The count of monsters killed is offered as a token - we couldn't save the past pilgrims, but we cleared the road for the next ones."));
						await dialog.Msg(L("Bounty without the offering during memorial-week is just blood-money. Salvia priests will pay it; the dead won't be quieted by it."));
						break;

					case "leave":
						await dialog.Msg(L("It is mixed work. So is being a militia-captain in memorial-week. I'll find someone less squeamish about wax-wicks."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNukas", out var nObj)) return;
				if (!quest.TryGetProgress("killLapasape", out var lObj)) return;
				if (!quest.TryGetProgress("killArchers", out var aObj)) return;
				if (!quest.TryGetProgress("lightLantern", out var lanObj)) return;

				if (nObj.Done && lObj.Done && aObj.Done && lanObj.Done)
				{
					await dialog.Msg(L("{#666666}*She presses both palms together briefly, then counts coin into a memorial-stamped pouch*{/}"));
					await dialog.Msg(L("Lantern's lit, sweep's done, count is offered. Salvia coin in full, plus the memorial-week stipend."));
					await dialog.Msg(L("The priests will speak your contribution at the dawn invocation tomorrow. The dead will hear it. So will the next pilgrims."));
					character.Quests.Complete(questId);
				}
				else if (nObj.Done && lObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Sweep complete. Now the lantern - bronze fixture above the cliff-bend, wax-wick I gave you, count spoken aloud."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each, no shortcuts. Memorial-week priests check the count against the cantor's roster."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The militia patrols the road hourly through memorial-week. Three of the past pilgrims' names were spoken at the lantern - one was my brother. The lantern still burns."));
			}
		});

		// Memorial-Lantern for Quest 1006 offering
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Memorial-Lantern of Ouaas"), "f_pilgrimroad_41_5", -200, 900, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A bronze memorial-lantern on Ouaas's altar verge, wick trimmed and ready*{/}"));
				return;
			}

			var litKey = "Laima.Quests.f_pilgrimroad_41_5.Quest1006.LanternLit";
			if (character.Variables.Perm.GetBool(litKey, false))
			{
				await dialog.Msg(L("{#666666}*Your lantern burns steady on the altar*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killNukas", out var nObj)) return;
			if (!quest.TryGetProgress("killLapasape", out var lObj)) return;
			if (!quest.TryGetProgress("killArchers", out var aObj)) return;

			if (!(nObj.Done && lObj.Done && aObj.Done))
			{
				await dialog.Msg(L("{#666666}*The lantern is ready, but you haven't finished the sweep*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Lighting lantern..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(litKey, true);
				character.ServerMessage(L("{#FFD700}Memorial-lantern lit. Return to Militia-Captain Gediminas.{/}"));
			}
			else
			{
				character.ServerMessage(L("Lighting interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FPilgrimroad415Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1001);
		SetName(L("Brown Nuka Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Nukas trampling the pilgrim road."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim-Warden] Mindaugas"), "f_pilgrimroad_41_5");

		AddObjective("killNukas", L("Kill Brown Nukas"),
			new KillObjective(40, new[] { MonsterId.Nuka_Brown }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FPilgrimroad415Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1002);
		SetName(L("Fire Hardened Arrowheads"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Lapasape archers and bring fire hardened arrowheads."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Algirdas"), "f_pilgrimroad_41_5");

		AddObjective("killLapasape", L("Kill Brown Lapasape"),
			new KillObjective(28, new[] { MonsterId.Lapasape_Bow_Brown }));

		AddObjective("gatherHeads", L("Gather fire hardened arrowheads"),
			new CollectItemObjective(650756, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650756, character.Inventory.CountItem(650756), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650756, character.Inventory.CountItem(650756), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad415Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1003);
		SetName(L("Yellow-Eyed Petals"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Elma and gather yellow-eyed petals for Ouaas's altar."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Memorial-Tender] Birute"), "f_pilgrimroad_41_5");

		AddObjective("killElma", L("Kill Red Elma"),
			new KillObjective(18, new[] { MonsterId.Elma_Red }));

		AddObjective("gatherPetals", L("Gather yellow-eyed petals"),
			new CollectItemObjective(661076, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(18000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661076, character.Inventory.CountItem(661076), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661076, character.Inventory.CountItem(661076), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad415Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1004);
		SetName(L("Cliffside Archers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Deadbornscab archers ambushing the road from the cliffs."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Marshal] Kestutis"), "f_pilgrimroad_41_5");

		AddObjective("killArchers", L("Kill Deadbornscab archers"),
			new KillObjective(18, new[] { MonsterId.Deadbornscab_Bow }));

		AddObjective("spikeRoosts", L("Spike the four cliff archer-roosts"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_5.Quest1004.RoostsSpiked", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17500));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1004.RoostsSpiked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_5.Quest1004.Roost{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1004.RoostsSpiked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_5.Quest1004.Roost{i}");
	}
}

public class FPilgrimroad415Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1005);
		SetName(L("The Trampler"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Nukas to draw out the bull leading the herd."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hunter] Vytenis"), "f_pilgrimroad_41_5");

		AddObjective("readTracks", L("Read the three trampling-tracks"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_5.Quest1005.TracksRead", 3, true));

		AddObjective("killHerd", L("Kill Brown Nukas"),
			new KillObjective(10, new[] { MonsterId.Nuka_Brown }));

		AddObjective("killBull", L("Defeat the bull Nuka"),
			new KillObjective(1, new[] { MonsterId.Nuka_Brown }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1005.TracksRead");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_5.Quest1005.Track{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1005.TracksRead");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_5.Quest1005.Track{i}");
	}
}

public class FPilgrimroad415Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1006);
		SetName(L("Pilgrim Road Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Brown Nukas, Brown Lapasape, and Deadbornscab archers."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Gediminas"), "f_pilgrimroad_41_5");

		AddObjective("killNukas", L("Kill Brown Nukas"),
			new KillObjective(12, new[] { MonsterId.Nuka_Brown }));

		AddObjective("killLapasape", L("Kill Brown Lapasape"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Bow_Brown }));

		AddObjective("killArchers", L("Kill Deadbornscab archers"),
			new KillObjective(12, new[] { MonsterId.Deadbornscab_Bow }));

		AddObjective("lightLantern", L("Light the Memorial-Lantern of Ouaas"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_5.Quest1006.LanternLit", 1, true));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1006.LanternLit");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_5.Quest1006.LanternLit");
	}
}
