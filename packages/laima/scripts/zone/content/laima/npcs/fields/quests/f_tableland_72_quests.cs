//--- Melia Script ----------------------------------------------------------
// Tableland 72 Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the high tableland beyond Ibre.
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

public class FTableland72QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: White Spion Sweep
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Plateau-Ward] Mindaugas"), "f_tableland_72", 400, -1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("White Spions cluster the upper terraces. Forty kills and the herd-trails open again."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Trails?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. Mind the wind on the ledges."));
						break;

					case "info":
						await dialog.Msg(L("Cattle drovers can't reach the high pasture while Spions own it."));
						break;

					case "leave":
						await dialog.Msg(L("Trails stay closed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpions", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Pasture's clear."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First herd up the ledges yesterday."));
			}
		});

		// Quest 2: Lapasape Moss
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Herbalist] Ruta"), "f_tableland_72", -450, -950, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Lapasapes hoard blue moss in their fur. Kill twenty-five and bring six tufts."));

				var response = await dialog.Select(L("Moss?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why blue?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Tufts only. The roots wilt off-stone."));
						break;

					case "info":
						await dialog.Msg(L("Blue moss steeps a salve that mends wind-burn. Plateau winters demand it."));
						break;

					case "leave":
						await dialog.Msg(L("Salve runs out by next moon."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLapasapes", out var killObj)) return;
				if (!quest.TryGetProgress("gatherMoss", out var mObj)) return;

				if (killObj.Done && mObj.Done)
				{
					await dialog.Msg(L("Six tufts. Salve sets tonight."));
					character.Inventory.Remove(668020, character.Inventory.CountItem(668020), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salve jars on the way to Ibre already."));
			}
		});

		// Quest 3: Cronewt Needler Crystals
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Crystal-Cutter] Birute-IV"), "f_tableland_72", 1250, -550, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1003);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Cronewt Mages grow needler crystals along their spines. Kill eighteen, bring five crystals."));

				var response = await dialog.Select(L("Crystals?"),
					Option(L("I'll bring"), "help"),
					Option(L("Use?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Pinch the base. They snap clean."));
						break;

					case "info":
						await dialog.Msg(L("Cut right, they replace lens-glass for far-sight scopes. Plateau watchtowers want a dozen."));
						break;

					case "leave":
						await dialog.Msg(L("Towers stay near-blind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCrystals", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Five crystals. Cutting begins at dusk."));
					character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First scope tested. Watchtower sees the river bend now."));
			}
		});

		// Quest 4: Red Hohen Orben
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Hunter] Jurgita"), "f_tableland_72", 500, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1004);

			dialog.SetTitle(L("Jurgita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A hunter strokes the breast of a hooded falcon perched on her glove, the bird trembling with grounded restlessness*{/}"));
				await dialog.Msg(L("Red Hohen Orben drift through the wind-eddies above the upper terrace. They look harmless - bobbing balls of red-tinted air - until a hawk flies through one. Then they constrict, and the hawk doesn't come back."));
				await dialog.Msg(L("Three of my hawks died last week. The four falconry-perches on the cliff-edge are torn down too - the Orben pop the perch-cords for sport. Without perches, even the surviving hawks can't rest between flights."));

				var response = await dialog.Select(L("Burst 15 Red Hohen Orben to clear the eddies, then re-string the four falconry-perches the Orben tore down. The hawks need both - sky cleared and perches standing. Will you?"),
					Option(L("I'll burst the Orben and re-string the perches"), "help"),
					Option(L("Why did three of your hawks die?"), "info"),
					Option(L("Find a falconer"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She passes you a long thin needle and a coil of waxed perch-cord*{/}"));
						await dialog.Msg(L("Fifteen Orben, no shortcuts. Strike from below - the underside is membrane, the top is hardened crystal. A blade goes through the underside; a hammer wouldn't crack the top."));
						await dialog.Msg(L("Then the four perches. Knot the perch-cord back to the cliff-post with a falconer's hitch - one loop over, one through, two ends taut. Don't tie a regular knot or the perch swings; a swinging perch terrifies a hawk."));
						break;

					case "info":
						await dialog.Msg(L("They flew through Orben without seeing them. Red-tinted air against red-tinted sunset; no hawk's eye distinguishes that. The Orben constrict on contact - it's defensive, not predatory, but the result is a falcon caught and crushed before it understands what happened."));
						await dialog.Msg(L("Three was bad enough. If I lose another, I won't have a falconry left. The trade dies with my generation; I taught no apprentices."));
						break;

					case "leave":
						await dialog.Msg(L("Hawks stay grounded, the falconry contracts close out, the trade ends one falconer earlier than it had to. I will not be the last; there are others on the south coast. But it stings to be one of the last."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("burstOrben", out var killObj)) return;
				if (!quest.TryGetProgress("restringPerches", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("{#666666}*She unhoods the falcon and lets it bate, the bird flapping a long stretching arc before settling clean back on the glove*{/}"));
					await dialog.Msg(L("Eddies are clear. Perches knotted with falconer's hitches - I tested each one. The hawks fly the upper terrace tomorrow at dawn for the first time in a fortnight."));
					await dialog.Msg(L("Take this. Hunter's purse, and a moulted feather from this falcon's last shed. Carry it; falconers will know you walk with the trade. So will the hawks."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Fifteen Orben first. Re-stringing perches while the eddies still hold Orben is just preparing dinner for the next contraction. Burst them all before you knot."));
				}
				else
				{
					await dialog.Msg(L("Eddies are clear, the air reads true. Now the four perches - falconer's hitch, two ends taut, no swing in the rope. Take your time on the knots."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Hawks back on the wing this morning - all five of them, including the youngest, who had never flown the upper terrace before. She caught a stooped Tipio at first hour. The trade lives another generation. Yours, if you ever wanted it."));
			}
		});

		// Falconry-perch re-string points for Quest 1004
		//-------------------------------------------------------------------------
		void AddFalconryPerch(int perchNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Falconry-Perch"), "f_tableland_72", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_72", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A wooden falconry-perch, jess-cord trailing in the dust*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_72.Quest1004.Perch{perchNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already re-strung; the cord runs taut*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Knotting cord..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_72.Quest1004.PerchesStrung", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_72.Quest1004.PerchesStrung", count);
					character.ServerMessage(LF("Falconry-perches re-strung: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All perches re-strung! Return to Hunter Jurgita.{/}"));
				}
				else
				{
					character.ServerMessage(L("Knotting interrupted."));
				}
			});
		}

		AddFalconryPerch(1, 400, -100, 0);
		AddFalconryPerch(2, 600, -300, 90);
		AddFalconryPerch(3, 700, -100, 180);
		AddFalconryPerch(4, 500, -400, 270);

		// Quest 5: The White Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Tomas-II"), "f_tableland_72", 700, 1200, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_tableland_72.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter scrapes Spion-musk off the heel of his boot with a knife-edge, grimacing*{/}"));
				await dialog.Msg(L("White Spion alpha leads the upper-terrace pack. Big, mean, smart enough to keep to his den unless directly insulted. He's killed three drovers this season - two gored, one trampled."));
				await dialog.Msg(L("Three scent-mounds outside his den-mouth mark his territory. Boot them flat - that's the deepest insult to a Spion alpha. He'll come out fighting instead of waiting in the den for the pack to flush you."));

				var response = await dialog.Select(L("Crush the three Spion scent-mounds outside his den, then kill 10 White Spions to flush him out. He fights when his territory's defaced. Will you take him?"),
					Option(L("I'll take the alpha"), "help"),
					Option(L("Why not just smoke the den?"), "info"),
					Option(L("Find a Spion specialist"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a stiff-bristled scrub-brush and a pair of leather over-boots*{/}"));
						await dialog.Msg(L("Three mounds, three boot-flattenings. Wear the over-boots - the musk soaks through normal leather and the smell stays for a week. Stomp the mounds to powder; don't just kick them aside."));
						await dialog.Msg(L("Then ten Spions. The alpha emerges around the eighth. He charges straight, like the bull-Nuka but smaller and faster. Sidestep at the last second; his momentum throws him past you."));
						break;

					case "info":
						await dialog.Msg(L("Smoke would flush him, but it would also kill his pack inside the den - cubs included. Drovers won't take that contract; the cubs are protected under the old herd-codes. Mound-flattening sends the same message and lets the cubs choose to flee or stay."));
						await dialog.Msg(L("Old hunter's compromise. The alpha dies, the territory empties, but the bloodline survives in whichever cub strikes off on her own. Cleaner conscience for the whole drove-trade."));
						break;

					case "leave":
						await dialog.Msg(L("Pack grows. Drovers stop using the upper road. The drover-trade contracts and prices climb in Klaipeda. I will sit here, scraping musk off my boot, until you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("crushMounds", out var mObj)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;

				if (mObj.Done && pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("{#666666}*He flips you a coin pouch and grins through the dust on his face*{/}"));
					await dialog.Msg(L("Mounds crushed flat, pack broken, alpha dropped clean. The cubs scattered south - one survivor at least, if the drover-codes still mean anything. The territory's empty for the next pack."));
					await dialog.Msg(L("Bounty plus a drover-stipend. The drovers' guild voted on it after the third gored herder; they'll pay anyone who clears the upper-terrace alpha. You're the first to actually do it."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Spion_White, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The White Alpha emerges from the ridge cave!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else if (!mObj.Done)
				{
					await dialog.Msg(L("Three scent-mounds first. Stomp them to powder; over-boots only, the musk soaks through."));
				}
				else
				{
					await dialog.Msg(L("Mounds are flat. Now ten Spions - the alpha emerges around the eighth. Sidestep his charge."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Terrace's quiet. Drovers are picking the upper-road again - first time this season. The drovers' guild posted your description on every staging-post; nobody knows your name. The description ends 'asks for no thanks.' True or not, that's how the trade tells the story now."));
			}
		});

		// Spion scent-mound crush points for Quest 1005
		//-------------------------------------------------------------------------
		void AddScentMound(int moundNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Spion Scent-Mound"), "f_tableland_72", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_72", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pungent mound of Spion-musk and dirt outside the den*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_72.Quest1005.Mound{moundNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already flat; the scent disperses*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Crushing mound..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_72.Quest1005.MoundsCrushed", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_72.Quest1005.MoundsCrushed", count);
					character.ServerMessage(LF("Scent-mounds crushed: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All mounds crushed! Now bait out the Alpha.{/}"));
				}
				else
				{
					character.ServerMessage(L("Crushing interrupted."));
				}
			});
		}

		AddScentMound(1, 600, 1100, 0);
		AddScentMound(2, 800, 1300, 90);
		AddScentMound(3, 500, 1400, 180);

		// Quest 6: Tableland Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Kazimieras"), "f_tableland_72", -400, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1006);

			dialog.SetTitle(L("Kazimieras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A militia-captain ties a leather knot in a tally-cord, the cord already heavy with last week's tallies*{/}"));
				await dialog.Msg(L("Upper road serves the survey-camps and the falconry contracts both. We sweep it weekly or those contracts collapse and the whole upper terrace empties out. Drovers, surveyors, falconers - all of them depend on this road."));
				await dialog.Msg(L("Three species contest the upper road. White Spions in the open, Cronewt Mages in the brush-pockets, Brown Lapasapes on the verges. The Upper Road Bell at the road's head signals the militia when the sweep's done - they hold off the perimeter walk until they hear it."));

				var response = await dialog.Select(L("Kill 12 White Spions, 12 Blue Cronewt Mages, and 12 Brown Lapasapes, then ring the Upper Road Bell at the road's head. The militia waits on the bell. Take it?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why a tally-cord on top of the bell?"), "info"),
					Option(L("Find a road-walker"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She passes you a small wooden token carved with the militia's road-mark*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. The militia perimeter-walk counts the corpses on the morning sweep."));
						await dialog.Msg(L("Bell at the road's head, just past the survey-camp turnoff. Slot the token into the bell-ring, pull the rope once, count to three, release. Single long peal carries up the terrace and back."));
						break;

					case "info":
						await dialog.Msg(L("Bell tells the militia. Tally-cord tells me. The bell-peal is loud and public; the cord is private and exact. I knot one knot per twenty-four kills. At month's end I count the knots and pay the contractors who matched."));
						await dialog.Msg(L("Trust but verify. Old militia saying. The bell starts the trust; the cord finishes the verification."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau stays unwalked, the falconers stay grounded, the survey-camps wait. I will find a road-walker if I have to recruit from the militia rolls. That eats into our patrol hours."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpions", out var sObj)) return;
				if (!quest.TryGetProgress("killMages", out var mObj)) return;
				if (!quest.TryGetProgress("killLapasapes", out var lObj)) return;
				if (!quest.TryGetProgress("ringBell", out var bObj)) return;

				if (sObj.Done && mObj.Done && lObj.Done && bObj.Done)
				{
					await dialog.Msg(L("{#666666}*She knots a fresh leather knot into the tally-cord and counts coin from a militia-stamped pouch*{/}"));
					await dialog.Msg(L("Bell heard. Sweep counted. Tally-cord adds another knot. Falconers and surveyors both can work the upper terrace tomorrow without flinching at every shadow."));
					await dialog.Msg(L("Coin in full plus a quarter-purse for the clean bell-form. Some contractors panic-pull the rope and we get two short peals - that's the danger-call and the militia turns out armed. You didn't. Worth the extra."));
					character.Quests.Complete(questId);
				}
				else if (sObj.Done && mObj.Done && lObj.Done)
				{
					await dialog.Msg(L("Sweep done. Now the Upper Road Bell - token in the ring, single rope-pull, count to three. The militia listens."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each. The Cronewt Mages cluster in the brush-pockets - work them in pairs if you can. Don't let them open distance; their ranged work is brutal."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Militia walks the upper road now, paid by the contract-cycle. Three falconers reported their hawks flying again; two survey-camps requested expansion. Your bell-peal echoed up the terrace - they say one drover heard it from the staging-post and started walking."));
			}
		});

		// Upper Road Bell for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Upper Road Bell"), "f_tableland_72", -350, 750, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A bronze road-bell hung at the upper road's head*{/}"));
				return;
			}

			var rungKey = "Laima.Quests.f_tableland_72.Quest1006.BellRung";
			if (character.Variables.Perm.GetBool(rungKey, false))
			{
				await dialog.Msg(L("{#666666}*Already rung*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killSpions", out var sObj)) return;
			if (!quest.TryGetProgress("killMages", out var mObj)) return;
			if (!quest.TryGetProgress("killLapasapes", out var lObj)) return;

			if (!(sObj.Done && mObj.Done && lObj.Done))
			{
				await dialog.Msg(L("{#666666}*The rope is in your hand, but the sweep isn't done*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Ringing bell..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(rungKey, true);
				character.ServerMessage(L("{#FFD700}Upper Road Bell rung. Return to Militia-Captain Kazimieras.{/}"));
			}
			else
			{
				character.ServerMessage(L("Ringing interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland72Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1001);
		SetName(L("White Spion Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill White Spions overrunning the plateau terraces."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Plateau-Ward] Mindaugas"), "f_tableland_72");

		AddObjective("killSpions", L("Kill White Spions"),
			new KillObjective(40, new[] { MonsterId.Spion_White }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland72Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1002);
		SetName(L("Blue Lapasape Moss"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Lapasapes and bring blue moss tufts for wind-burn salve."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Ruta"), "f_tableland_72");

		AddObjective("killLapasapes", L("Kill Brown Lapasapes"),
			new KillObjective(25, new[] { MonsterId.Lapasape_Brown }));

		AddObjective("gatherMoss", L("Gather blue moss tufts"),
			new CollectItemObjective(668020, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668020, character.Inventory.CountItem(668020), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668020, character.Inventory.CountItem(668020), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland72Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1003);
		SetName(L("Needler Crystals"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Cronewt Mages and bring needler crystals for far-sight scopes."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Cutter] Birute"), "f_tableland_72");

		AddObjective("killMages", L("Kill Blue Cronewt Mages"),
			new KillObjective(18, new[] { MonsterId.Cronewt_Mage_Blue }));

		AddObjective("gatherCrystals", L("Gather needler crystals"),
			new CollectItemObjective(663123, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland72Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1004);
		SetName(L("Wind-Eddy Burst"));
		SetType(QuestType.Sub);
		SetDescription(L("Burst Red Hohen Orben to clear the upper-pass eddies for falconers."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hunter] Jurgita"), "f_tableland_72");

		AddObjective("burstOrben", L("Burst Red Hohen Orben"),
			new KillObjective(15, new[] { MonsterId.Hohen_Orben_Red }));

		AddObjective("restringPerches", L("Re-string the four falconry-perches"),
			new VariableCheckObjective("Laima.Quests.f_tableland_72.Quest1004.PerchesStrung", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1004.PerchesStrung");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_72.Quest1004.Perch{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1004.PerchesStrung");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_72.Quest1004.Perch{i}");
	}
}

public class FTableland72Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1005);
		SetName(L("The White Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill White Spions to draw out the alpha leading the upper-terrace pack."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Tomas"), "f_tableland_72");

		AddObjective("crushMounds", L("Crush the three Spion scent-mounds"),
			new VariableCheckObjective("Laima.Quests.f_tableland_72.Quest1005.MoundsCrushed", 3, true));

		AddObjective("killPack", L("Kill White Spions"),
			new KillObjective(10, new[] { MonsterId.Spion_White }));

		AddObjective("killAlpha", L("Defeat the White Alpha"),
			new KillObjective(1, new[] { MonsterId.Spion_White }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1005.MoundsCrushed");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_72.Quest1005.Mound{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1005.MoundsCrushed");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_72.Quest1005.Mound{i}");
	}
}

public class FTableland72Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1006);
		SetName(L("Tableland Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of White Spions, Blue Cronewt Mages, and Brown Lapasapes."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Kazimieras"), "f_tableland_72");

		AddObjective("killSpions", L("Kill White Spions"),
			new KillObjective(12, new[] { MonsterId.Spion_White }));

		AddObjective("killMages", L("Kill Blue Cronewt Mages"),
			new KillObjective(12, new[] { MonsterId.Cronewt_Mage_Blue }));

		AddObjective("killLapasapes", L("Kill Brown Lapasapes"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Brown }));

		AddObjective("ringBell", L("Ring the Upper Road Bell"),
			new VariableCheckObjective("Laima.Quests.f_tableland_72.Quest1006.BellRung", 1, true));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1006.BellRung");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1006.BellRung");
	}
}
