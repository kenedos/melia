//--- Melia Script ----------------------------------------------------------
// Thaumas Trail - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_pilgrimroad_49 map. Pilgrim road leading
// toward Fedimian, overrun by Tini demon-folk spilling from the prison-cracks.
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

public class FPilgrimroad49QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Trail Pest Kill
		// =====================================================================
		// Trail-Warden Balys - Brown Tinis picking off lone pilgrims
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Trail-Warden] Balys"), "f_pilgrimroad_49", -1050, -2450, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1001);

			dialog.SetTitle(L("Balys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A broad-shouldered warden in a patched Fedimian tabard tightens the strap on a dented half-helm*{/}"));
				await dialog.Msg(L("Four pilgrims this week. Four. Brown Tinis ambush singles on the beetle-path, then scatter before the rest of the caravan catches up."));

				var response = await dialog.Select(L("I need them thinned. Twenty Brown Tinis. Hit the main cluster south of here - that's where they den. The rest will scatter once their numbers break."),
					Option(L("I'll thin the Brown Tinis"), "help"),
					Option(L("Why so many Tinis now?"), "info"),
					Option(L("Pilgrims travel at their own risk"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods once, relief cracking his weathered face*{/}"));

						if (await dialog.YesNo(L("Twenty Brown Tinis. The south beetle-cluster is thickest. Can you do it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Watch your flanks. Brown Tinis strike in pairs - one draws, the other comes from behind."));
							await dialog.Msg(L("And if you see their Archers - Green Tinis with bows - those are harder. Don't engage unless you're close already."));
						}
						break;

					case "info":
						await dialog.Msg(L("Used to be a Tini every fortnight, maybe. Now they come in packs. Something stirred them out of wherever they nested."));
						await dialog.Msg(L("A refugee from Aqueduct told me the prison-cracks opened wider down south. Seed-pollen first, then the Tinis started boiling up the trail."));
						break;

					case "leave":
						await dialog.Msg(L("Then the road stays closed, and the pilgrims stay stranded. Your call."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTini", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to the count, then allows himself a long exhale*{/}"));
					await dialog.Msg(L("Twenty. The south cluster won't mount a full ambush for a week, maybe two. I can run a caravan through by tomorrow."));
					await dialog.Msg(L("Take this - warden's pay, from my own purse. Fedimian will cover it once the dispatch reports are in."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("South beetle-cluster. Watch your flanks. Brown Tinis strike in pairs."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First caravan made it through clean. Second one had one graze-wound. That's as good as this road's been in months."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Unfinished Pilgrimage
		// =====================================================================
		// Pilgrim-Priest Petras - Rites scroll for Fedimian's Katyn waystation
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Pilgrim-Priest] Petras"), "f_pilgrimroad_49", 1370, -100, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1002);

			dialog.SetTitle(L("Petras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A road-stained priest clutches a bundle of wax-sealed scrolls, his robe singed at the hem*{/}"));
				await dialog.Msg(L("I left Klaipeda with three apprentices. Green Tini Archers caught us at the crossing. I alone made it this far. I dare not take another step."));

				var response = await dialog.Select(L("The rites-scroll must reach Waystation-Keeper Urte at the Grynas Trails crossing - she holds the Katyn wardings. Without it, her waystation falls silent and the next caravan through has no sanctuary. Will you carry it?"),
					Option(L("I'll carry the scroll to Urte"), "help"),
					Option(L("Why not retreat to Klaipeda?"), "info"),
					Option(L("I'm not a messenger"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He presses the scroll-bundle into your hands with both of his, slow and careful*{/}"));

						if (await dialog.YesNo(L("The Grynas Trails warp is northwest of here. Urte keeps a stone shrine beside the gate. Bring back whatever reply she offers. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The scroll-wax bears the Fedimian seal. Do not break it. Urte will know the mark."));
							await dialog.Msg(L("And if you see the apprentices' bodies on the path - do not touch them. The Tinis lay traps over the fallen."));
						}
						break;

					case "info":
						await dialog.Msg(L("Because the rites must reach Fedimian. Three apprentices died for this bundle. Retreat turns their deaths into wasted breath."));
						await dialog.Msg(L("Our order teaches: the scroll travels, or the scroll-bearer dies trying. I have failed at the second. Someone must not fail the first."));
						break;

					case "leave":
						await dialog.Msg(L("Then I pray the next traveler is braver. Walk on, friend."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverScroll", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*He receives Urte's reply with both hands, reads it standing, then presses it to his forehead*{/}"));
					await dialog.Msg(L("She accepted the rites. The Katyn waystation stays lit for another season. My apprentices' deaths are not wasted."));
					await dialog.Msg(L("{#666666}*He unclasps a small silver icon from his robe and offers it*{/}"));
					await dialog.Msg(L("Take this. It is all I carry that is mine, and not the order's. Walk blessed, pilgrim."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Urte's waystation is northwest, at the Grynas Trails warp. The Fedimian seal - do not break it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I begin the return pilgrimage to Klaipeda at first light. The order will send new apprentices. The rites continue."));
			}
		});

		// =====================================================================
		// WAYSTATION-KEEPER URTE (Quest 1002 recipient)
		// =====================================================================
		AddNpc(20114, L("[Waystation-Keeper] Urte"), "f_pilgrimroad_49", 1500, -1620, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1002);

			dialog.SetTitle(L("Urte"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_49.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A sturdy keeper in a grey wool cloak trims the wick of a waystation lamp, one eye always on the trail*{/}"));
					await dialog.Msg(L("{#666666}*You present the Fedimian-sealed scroll bundle. Her shoulders drop a fraction*{/}"));
					await dialog.Msg(L("From Petras. So he made it that far, at least. The seal is unbroken - good."));
					await dialog.Msg(L("{#666666}*She writes a short reply on the back of a spare waystation slip and rolls it tight*{/}"));
					await dialog.Msg(L("Tell him the waystation accepts the rites. Tell him the Katyn lamp will burn through winter. And tell him - the apprentices' names will be on the next Fedimian remembrance slate."));

					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_49.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Urte's reply received. Return to Pilgrim-Priest Petras.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry my reply back to Petras. He waits at the dandel crossing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Katyn waystation lamp is lit. Pilgrims can rest here again. It is a small thing, but the road is built of small things."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*The keeper trims a lamp wick, watching the trail*{/}"));
				await dialog.Msg(L("Travelers pass; I keep lamps. That is the waystation's simple contract."));
			}
		});

		// =====================================================================
		// QUEST 1003: Rootcrystal Shards
		// =====================================================================
		// Crystal-Gatherer Morta - Shards for pilgrim-blessing trades
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Crystal-Gatherer] Morta"), "f_pilgrimroad_49", -1200, 480, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1003);

			dialog.SetTitle(L("Morta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A thin-framed woman kneels beside a split rootcrystal, fingers wrapped in linen rags stained with crystal-dust*{/}"));
				await dialog.Msg(L("I trade shards at Fedimian's waystation for pilgrim-blessings. One shard, one blessing - that's the fair rate. But the Tinis took my last haul."));

				var response = await dialog.Select(L("Eight Rootcrystal shards. The clusters here are rich, but the crystals crack wrong if struck in haste. Take your time, and the shards come clean. Will you gather for me?"),
					Option(L("I'll gather eight shards"), "help"),
					Option(L("Why not gather yourself?"), "info"),
					Option(L("Trade crystals somewhere safer"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She smiles, crystal-dust cracking at the corners of her eyes*{/}"));

						if (await dialog.YesNo(L("Strike the rootcrystals at their base, not the crown. The shards come away cleaner that way. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The big clusters are northwest, along the old trail. Smaller ones scatter toward the southern bend."));
							await dialog.Msg(L("If a Tini appears while you're gathering - run first, fight second. The shards aren't worth a wound."));
						}
						break;

					case "info":
						await dialog.Msg(L("My eyes are going. The crystal-dust eats sight, a little each year. I can still strike clean, but I can't see a Tini coming at thirty paces."));
						await dialog.Msg(L("One more season, maybe two. Then I retire to the waystation and trade from a chair."));
						break;

					case "leave":
						await dialog.Msg(L("Crystals only grow here. The road is what the road is. I'll manage."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var shardCount = character.Inventory.CountItem(650555);

				if (shardCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She weighs each shard between thumb and forefinger, nodding slowly*{/}"));
					await dialog.Msg(L("Clean breaks. Base-struck, not crown-struck. You listened."));
					await dialog.Msg(L("{#666666}*She draws a small pouch and a waystation-token from her gathering-basket*{/}"));
					await dialog.Msg(L("Here - a gatherer's share. And a waystation-token; Urte honors these for a hot meal."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Strike at the base. Eight shards. Take your time."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The shards traded clean for pilgrim-blessings. I sleep under the waystation roof tonight."));
			}
		});

		// =====================================================================
		// ROOTCRYSTAL SHARD SPOTS
		// =====================================================================
		// For Quest 1003 - Rootcrystal Shards
		// =====================================================================

		void AddCrystalSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Rootcrystal Cluster"), "f_pilgrimroad_49", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_49", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A veined rootcrystal hums quietly in the soil. Striking now would shatter it to waste*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_49.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*This cluster is already worked. The base is cleanly broken, shards removed*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_pilgrimroad_49.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 18)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Tiny_Brown, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Brown Tini lunges from the crystal shadow!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Striking the crystal base..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(4));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650555, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650555);
					character.ServerMessage(LF("Rootcrystal shards gathered: {0}/8", currentCount));

					if (currentCount >= 8)
					{
						character.ServerMessage(L("{#FFD700}All shards gathered! Return to Crystal-Gatherer Morta.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Gathering interrupted."));
				}
			});
		}

		AddCrystalSpot(1, -1009, -2132, 0);
		AddCrystalSpot(2, -894, -136, 90);
		AddCrystalSpot(3, -1218, 480, 180);
		AddCrystalSpot(4, -1236, 901, 270);
		AddCrystalSpot(5, -25, 990, 0);
		AddCrystalSpot(6, 1193, 31, 90);
		AddCrystalSpot(7, 265, -364, 180);
		AddCrystalSpot(8, 983, 1409, 270);

		// =====================================================================
		// QUEST 1004: The Silent Seals
		// =====================================================================
		// Pilgrim-Archivist Darija - Checking mile-shrine wardmage seals
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Pilgrim-Archivist] Darija"), "f_pilgrimroad_49", 2350, 160, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1004);

			dialog.SetTitle(L("Darija"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A thin archivist in Fedimian grey unrolls a hand-drawn trail map, pinning the corners with pebbles*{/}"));
				await dialog.Msg(L("The Fedimian wardmage went silent three weeks ago. The last thing his desk-clerk recorded was a set of rubbings - from Aqueduct Bridge, by a Gintaras."));

				var response = await dialog.Select(L("There are four mile-shrines along the Thaumas Trail. Each holds a wardmage seal tied to the Fedimian working. If any seal is broken or corrupted, we'll know the silence is not absence - it is consumption. Walk the four. Tell me what you find."),
					Option(L("I'll walk the four mile-shrines"), "help"),
					Option(L("What does 'consumed' mean here?"), "info"),
					Option(L("Seals are archivist business"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She weights the map with a fifth pebble and circles four points*{/}"));

						if (await dialog.YesNo(L("West shrine, north shrine, east shrine, south shrine. Don't touch the seals - just read them. Describe each to me. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The shrines are stone pillars marked with a wardmage's sigil. The seal is a wax disc set into the pillar's face."));
							await dialog.Msg(L("Clean wax means the seal holds. Cracked wax means it weakened. Black wax means - something drank it through the working."));
						}
						break;

					case "info":
						await dialog.Msg(L("A wardmage's silence can mean three things. One: he's dead. Two: he's fled. Three: something came through the working and took him. Three is the worst."));
						await dialog.Msg(L("If the seals are black, it's three. And if it's three, Fedimian has a problem that pilgrim-archivists cannot solve alone."));
						break;

					case "leave":
						await dialog.Msg(L("Then I walk them myself, in time. But I have the knees of an archivist, not a trail-warden. It will take me a season."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var shrinesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_49.Quest1004.ShrinesChecked", 0);

				if (shrinesChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*She listens without writing, then slowly picks up her pen*{/}"));
					await dialog.Msg(L("West: cracked. North: cracked. East: black. South: black."));
					await dialog.Msg(L("{#666666}*Her jaw sets*{/}"));
					await dialog.Msg(L("Two drained. The working is half-consumed. The wardmage was taken through the south seal - it's closest to the Aqueduct side."));
					await dialog.Msg(L("I send a report to Fedimian tonight. Take this - an archivist's stipend, paid in full for field-work. You earned every coin."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four mile-shrines. West, north, east, south. Read each, tell me the wax."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Fedimian dispatched a war-mage to replace the silent one. The Thaumas Trail will have a new set of seals by the new moon. This time we guard them."));
			}
		});

		// =====================================================================
		// MILE-SHRINES
		// =====================================================================
		// For Quest 1004 - The Silent Seals
		// =====================================================================

		void AddMileShrine(int shrineNumber, string shrineName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(shrineName), "f_pilgrimroad_49", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_49", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered stone mile-shrine, a wardmage's sigil faintly visible beneath trail-dust*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_49.Quest1004.Shrine{shrineNumber}";
				var checkedShrine = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedShrine)
				{
					await dialog.Msg(L("{#666666}*You have already read this seal*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the wardmage seal..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var shrinesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_49.Quest1004.ShrinesChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_49.Quest1004.ShrinesChecked", shrinesChecked + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Mile-shrines read: {0}/4", shrinesChecked + 1));

					if (shrinesChecked + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All mile-shrines read! Return to Pilgrim-Archivist Darija.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddMileShrine(1, "West Mile-Shrine", "West seal: wax cracked through the sigil's heart. Not drained - weakened.", -1780, -310, 90);
		AddMileShrine(2, "North Mile-Shrine", "North seal: wax cracked along the sigil's rim. Warding still holds, barely.", -20, 1000, 180);
		AddMileShrine(3, "East Mile-Shrine", "East seal: wax blackened. Something drank through this working.", 1700, -40, 270);
		AddMileShrine(4, "South Mile-Shrine", "South seal: wax blackened and pitted. A single fingerprint impressed into the center, pressed from the inside.", 2050, -1300, 0);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Trail Pest Kill
//-----------------------------------------------------------------------------

public class TrailPestCullQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1001);
		SetName("Trail Pest Kill");
		SetType(QuestType.Sub);
		SetDescription("Trail-Warden Balys needs twenty Brown Tinis thinned before pilgrim caravans can move along the Thaumas Trail again.");
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Trail-Warden] Balys", "f_pilgrimroad_49");

		AddObjective("killTini", "Defeat Brown Tinis",
			new KillObjective(20, new[] { MonsterId.Tiny_Brown }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10)); // Large HP Potion
		AddReward(new ItemReward(640007, 10)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
	}
}

// Quest 1002 CLASS: The Unfinished Pilgrimage
//-----------------------------------------------------------------------------

public class TheUnfinishedPilgrimageQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1002);
		SetName("The Unfinished Pilgrimage");
		SetType(QuestType.Sub);
		SetDescription("Pilgrim-Priest Petras's Fedimian rites-scroll must reach Waystation-Keeper Urte at the Grynas Trails crossing.");
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Pilgrim-Priest] Petras", "f_pilgrimroad_49");

		AddObjective("deliverScroll", "Deliver the rites-scroll to Urte and return",
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_49.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10));  // Large HP Potion
		AddReward(new ItemReward(640007, 10));  // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_49.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_49.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Rootcrystal Shards
//-----------------------------------------------------------------------------

public class RootcrystalShardsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1003);
		SetName("Rootcrystal Shards");
		SetType(QuestType.Sub);
		SetDescription("Crystal-Gatherer Morta needs eight Rootcrystal shards cleanly broken from the Thaumas Trail clusters.");
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Crystal-Gatherer] Morta", "f_pilgrimroad_49");

		AddObjective("collectShards", "Gather Rootcrystal shards from the clusters",
			new CollectItemObjective(650555, 8));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650555,
			character.Inventory.CountItem(650555),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650555,
			character.Inventory.CountItem(650555),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Silent Seals
//-----------------------------------------------------------------------------

public class TheSilentSealsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1004);
		SetName("The Silent Seals");
		SetType(QuestType.Sub);
		SetDescription("Pilgrim-Archivist Darija has asked you to read the wardmage seals at the four mile-shrines along the Thaumas Trail and determine whether the silent Fedimian wardmage was consumed through his own working.");
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Pilgrim-Archivist] Darija", "f_pilgrimroad_49");

		AddObjective("readShrines", "Read the four mile-shrine seals",
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_49.Quest1004.ShrinesChecked", 4, true));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_49.Quest1004.ShrinesChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1004.Shrine{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_49.Quest1004.ShrinesChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1004.Shrine{i}");
		}
	}
}
