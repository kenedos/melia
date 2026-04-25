//--- Melia Script ----------------------------------------------------------
// Letas Stream - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_katyn_12. Eerie Katyn stream-country where
// water carries voices, Blue Meduja drift above shallow runs, and Rodelin
// undead wander out of the east-chapel ruins.
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

public class FKatyn12QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Chupacabras on the Stream-Path
		// =====================================================================
		AddNpc(20109, L("[Stream-Warden] Mantas"), "f_katyn_12", 100, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1001);

			dialog.SetTitle(L("Mantas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A grizzled warden crouches beside a shallow run, listening to the water. When he turns to you, his ear stays cocked to the stream*{/}"));
				await dialog.Msg(L("Corrupt Chupacabras drink from the Letas at dusk and leave the water tasting of iron for an hour after. Pilgrims who drink after them bleed from the gums for three days."));

				var response = await dialog.Select(L("Twenty Chupacabras thinned. They den in the bracken north of the bend. Will you?"),
					Option(L("I'll thin twenty Chupacabras"), "help"),
					Option(L("Corrupt how?"), "info"),
					Option(L("Water-safety is warden business"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Twenty. North bracken. Don't drink from the Letas for an hour after you kill one. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Their saliva is the corruption. Wipe your blade after each strike, or the next hit doubles through."));
						}
						break;

					case "info":
						await dialog.Msg(L("Something upstream taints them. I've walked three miles of bank and haven't found the source. The Letas carries it down to us, clean-looking and not clean at all."));
						break;

					case "leave":
						await dialog.Msg(L("Walk the south banks, then. Fewer Chupacabras, more Meduja drifting overhead. Meduja don't drink."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killChupacabra", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He tastes the stream with a fingertip, then nods*{/}"));
					await dialog.Msg(L("Iron-taste gone for now. A week, maybe two, before it returns. Warden's purse - and a strip of blade-cloth. Wipe every strike."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("North bracken. Wipe your blade. Twenty."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The stream ran clean for eleven days. Then iron-taste at dusk again. Katyn never sleeps."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Water-Sample
		// =====================================================================
		AddNpc(20107, L("[Waterclerk] Ausra"), "f_katyn_12", -700, 400, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1002);

			dialog.SetTitle(L("Ausra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A thin clerk in waxed-canvas sleeves stoppers a glass sample-vial and seals it with black wax*{/}"));
				await dialog.Msg(L("The chemist Brone needs this sample by tomorrow. I can't leave the gauging-post - if I step away, the readings skip, and a week's data goes void."));

				var response = await dialog.Select(L("Carry the vial to Brone at the east sample-hut. Bring back his reading-slip. Will you?"),
					Option(L("I'll carry the sample"), "help"),
					Option(L("What are you testing for?"), "info"),
					Option(L("Carry it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("East sample-hut. Don't shake the vial - sediment skews the reading. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Brone's slip is inked in iron-gall. You'll know it by the smell."));
						}
						break;

					case "info":
						await dialog.Msg(L("Whatever's corrupting the Chupacabras. We measure every dawn. The levels are rising - slowly, but rising."));
						break;

					case "leave":
						await dialog.Msg(L("Then the week's data breaks. Brone won't like it. I won't like it. Your call."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverSample", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She reads the iron-gall slip, lips moving over the numbers*{/}"));
					await dialog.Msg(L("Up three tenths from last week. That's the fastest climb in three years. I'll draft a warning-notice tonight."));
					await dialog.Msg(L("Clerk's pay. And a small vial of pure water from the spring-head - useful if you've tasted iron and want your mouth back."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East sample-hut. Brone. Don't shake the vial."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The warning-notice went out. Pilgrims are boiling stream-water before drinking. That helps. Not enough, but it helps."));
			}
		});

		// Quest 1002 recipient
		AddNpc(20114, L("[Chemist] Brone"), "f_katyn_12", 1000, 600, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1002);

			dialog.SetTitle(L("Brone"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_12.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A stocky chemist uncorks the vial, drops two grains of reagent into it, and watches the water turn a slow amber*{/}"));
					await dialog.Msg(L("Up from last week. I expected a climb. Not this fast."));
					await dialog.Msg(L("{#666666}*He writes numbers on a slip in iron-gall ink, folds it twice, and passes it back*{/}"));
					await dialog.Msg(L("Reading-slip. Tell Ausra the climb-rate changes the model. I'll redraft tonight."));

					character.Variables.Perm.Set("Laima.Quests.f_katyn_12.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Brone's reading-slip received. Return to Waterclerk Ausra.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the slip back. She'll read it by the iron-gall smell."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Redraft done. The model predicts iron-taste at the Klaipeda market-well by autumn. We have time. Not much."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*He stirs a flask over a small burner. The flask rings when he taps it*{/}"));
				await dialog.Msg(L("Sample-hut business only. Mind the flask-rack on your way past."));
			}
		});

		// =====================================================================
		// QUEST 1003: Operor Fog-Vials
		// =====================================================================
		AddNpc(20017, L("[Mist-Reader] Laima"), "f_katyn_12", -2000, 400, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1003);

			dialog.SetTitle(L("Laima"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An old mist-reader unstoppers an empty glass vial and holds it against a patch of morning-fog. The fog curls inward without prompting*{/}"));
				await dialog.Msg(L("Operors carry their own fog with them. Trap the fog in glass and you can read it - names, directions, the last thing someone asked out loud near an Operor's cloud."));

				var response = await dialog.Select(L("Eight fog-vials. Approach an Operor while it exhales, never while it inhales. Stopper the vial fast. Will you gather for me?"),
					Option(L("I'll gather eight fog-vials"), "help"),
					Option(L("What do you read them for?"), "info"),
					Option(L("Fog is fog"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Exhale-breaths only. Eight vials. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If an Operor inhales while you stopper - it takes a word out of you. Usually a small one. Sometimes not."));
						}
						break;

					case "info":
						await dialog.Msg(L("Missing persons. Overheard crimes. Last wishes that never got said aloud to the right ear. Honest work, if you can stand what the fog carries."));
						break;

					case "leave":
						await dialog.Msg(L("Then the missing stay missing. The fog keeps its secrets."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var vialCount = character.Inventory.CountItem(650854);
				if (vialCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She holds each vial up to lamplight, listening more than looking. Two she sets aside without comment*{/}"));
					await dialog.Msg(L("Six clear readings. Two carry voices - one a woman calling a name, one a man laughing at something I'd rather not hear again. Honest work either way."));
					await dialog.Msg(L("Reader's share. And a stopper-strip - wax that seals any vial tight, even if the fog wants out."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Exhale-breaths only. Eight vials."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two missing persons traced this week. The woman calling a name reached her daughter. The laughing man stays in the locked drawer."));
			}
		});

		void AddFogSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Operor Fog-Cloud"), "f_katyn_12", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_12", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A patch of drifting fog. Without a reader's order, there's no reason to trap it*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_12.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);
				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The fog here has been trapped already. The air is clear*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_katyn_12.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);
					if (SpawnTempMonsters(character, MonsterId.Operor_Blue, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}An Operor inhales sharply - you feel a word leave you!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Trapping the fog..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(4));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650854, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					var currentCount = character.Inventory.CountItem(650854);
					character.ServerMessage(LF("Fog-vials gathered: {0}/8", currentCount));
					if (currentCount >= 8)
						character.ServerMessage(L("{#FFD700}All vials gathered! Return to Mist-Reader Laima.{/}"));
				}
				else
				{
					character.ServerMessage(L("Stopper interrupted. The fog slips out."));
				}
			});
		}

		AddFogSpot(1, -2897, -210, 0);
		AddFogSpot(2, -3033, 112, 90);
		AddFogSpot(3, -2875, 364, 180);
		AddFogSpot(4, -2194, 915, 270);
		AddFogSpot(5, -1915, 754, 0);
		AddFogSpot(6, -1625, 1094, 90);
		AddFogSpot(7, -1681, 1481, 180);
		AddFogSpot(8, -2890, 1420, 270);

		// =====================================================================
		// QUEST 1004: The Chapel-Stones
		// =====================================================================
		AddNpc(20151, L("[Chapel-Keeper] Elz"), "f_katyn_12", 1050, 540, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1004);

			dialog.SetTitle(L("Elz"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young keeper in grey chapel-cloth kneels among broken flagstones at the edge of a ruined nave. Rodelin drift past her; she does not flinch*{/}"));
				await dialog.Msg(L("Four chapel-stones survived the crypt-break. If any still hold benediction, the Rodelin will keep their distance from that stone. If none do, the chapel is lost."));

				var response = await dialog.Select(L("Walk the four stones. Tell me whether the air above each feels warm, cold, or silent. I'll know what each means. Will you?"),
					Option(L("I'll walk the four stones"), "help"),
					Option(L("Why don't the Rodelin attack you?"), "info"),
					Option(L("Chapel work isn't mine"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Four stones. Hand a hand's-breadth above each. Warm, cold, silent. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Warm means benediction holds. Cold means it's drained. Silent means the stone hears nothing now - and is no longer a stone in any useful sense."));
						}
						break;

					case "info":
						await dialog.Msg(L("I'm kin to one of them. The rest recognize the blood. It's a thin protection, and I don't recommend strangers test it."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself over a month. My aunt has the patience for it; I'll borrow."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var stonesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", 0);
				if (stonesChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*She closes her eyes while you speak, and opens them only after the final entry*{/}"));
					await dialog.Msg(L("Stone One: warm. Stone Two: warm. Stone Three: cold. Stone Four: silent."));
					await dialog.Msg(L("Two hold, one drained, one gone. The chapel survives on two stones - thin, but enough. I'll renew the drained one at first dawn. The silent one we rope-off."));
					await dialog.Msg(L("Keeper's stipend, every coin. And a small benediction-thread - tied to your gear, it keeps Rodelin from noticing you for a night."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four stones. Hand above each. Warm, cold, silent."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The drained stone renewed. The silent one roped off. My aunt drifts further from the chapel now - I'm glad of that, for her sake."));
			}
		});

		void AddChapelStone(int stoneNumber, string stoneName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(stoneName), "f_katyn_12", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_12", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A chapel-stone half-buried in ruin. Without a keeper's order, there's no reason to test the air above it*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_12.Quest1004.Stone{stoneNumber}";
				var checkedStone = character.Variables.Perm.GetBool(variableKey, false);
				if (checkedStone)
				{
					await dialog.Msg(L("{#666666}*You have already read this stone's air*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Holding a hand above the stone..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var stonesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", stonesChecked + 1);
					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Stones read: {0}/4", stonesChecked + 1));
					if (stonesChecked + 1 >= 4)
						character.ServerMessage(L("{#FFD700}All stones read! Return to Chapel-Keeper Elz.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddChapelStone(1, "Nave Stone", "Nave Stone: warm to the palm. Benediction holds.", 1156, 1158, 0);
		AddChapelStone(2, "Altar Stone", "Altar Stone: warm. Strongest of the four.", 1377, 1603, 90);
		AddChapelStone(3, "West-Aisle Stone", "West-Aisle Stone: cold. Benediction drained but recoverable.", 1579, 94, 180);
		AddChapelStone(4, "Crypt-Step Stone", "Crypt-Step Stone: silent. Not warm, not cold. Nothing answers the hand.", 917, 90, 270);
	}
}

// Quest 1001
public class ChupacabrasOnTheStreamPathQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1001);
		SetName("Chupacabras on the Stream-Path");
		SetType(QuestType.Sub);
		SetDescription("Stream-Warden Mantas needs twenty Corrupt Chupacabras thinned before the Letas runs iron-tasting for another week.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Stream-Warden] Mantas", "f_katyn_12");
		AddObjective("killChupacabra", "Defeat Corrupt Chupacabras on the stream-banks",
			new KillObjective(20, new[] { MonsterId.Chupacabra_Green }));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 4));
	}
}

// Quest 1002
public class TheWaterSampleQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1002);
		SetName("The Water-Sample");
		SetType(QuestType.Sub);
		SetDescription("Waterclerk Ausra's stream-sample must reach Chemist Brone at the east sample-hut and return with his reading-slip.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Waterclerk] Ausra", "f_katyn_12");
		AddObjective("deliverSample", "Deliver the sample and return with the slip",
			new VariableCheckObjective("Laima.Quests.f_katyn_12.Quest1002.Delivered", 1, true));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1002.Delivered");
	}
}

// Quest 1003
public class OperorFogVialsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1003);
		SetName("Operor Fog-Vials");
		SetType(QuestType.Sub);
		SetDescription("Mist-Reader Laima needs eight Operor fog-vials trapped from exhale-breaths for her missing-persons readings.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Mist-Reader] Laima", "f_katyn_12");
		AddObjective("collectFog", "Trap Operor fog into vials",
			new CollectItemObjective(650854, 8));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 4));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650854, character.Inventory.CountItem(650854), InventoryItemRemoveMsg.Destroyed);
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650854, character.Inventory.CountItem(650854), InventoryItemRemoveMsg.Destroyed);
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004
public class TheChapelStonesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1004);
		SetName("The Chapel-Stones");
		SetType(QuestType.Sub);
		SetDescription("Chapel-Keeper Elz has asked you to read the four surviving chapel-stones and determine which still hold benediction, which are drained, and which are silent.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Chapel-Keeper] Elz", "f_katyn_12");
		AddObjective("readStones", "Read the four chapel-stones",
			new VariableCheckObjective("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", 4, true));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1004.StonesChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1004.StonesChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1004.Stone{i}");
	}
}
