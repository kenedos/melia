
// --- Melia Script ----------------------------------------------------------
// Adventurer's Guild NPCs
// --- Description -----------------------------------------------------------
// Provides various services related to the Adventurer's Guild.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Extensions.LivelyDialog;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class AdventurersGuildScript : GeneralScript
{
	private readonly Random _random = RandomProvider.Get();

	internal static class ItemId
	{
		public const int JexpCard_UpRank2 = 640142;
		public const int JexpCard_UpRank3 = 640143;
		public const int JexpCard_UpRank4 = 640144;
		public const int JexpCard_UpRank5 = 640145;
		public const int JexpCard_UpRank6 = 640146;
		public const int Drug_CRTHR_GIMMICK1 = 640159;
		public const int Drug_CRTHR_GIMMICK2 = 640160;
		public const int Drug_PATK_GIMMICK1 = 640161;
		public const int Drug_PATK_GIMMICK2 = 640162;
		public const int Drug_ResFire_GIMMICK1 = 640164;
		public const int Vis = 900011;
	}

	// Define adventurer ranks and their corresponding mob lists
	private static readonly Dictionary<AdventurerRank, List<MonsterData>> _mobLists = new()
	{
		{ AdventurerRank.Wood, new List<MonsterData>() },
		{ AdventurerRank.Copper, new List<MonsterData>() },
		{ AdventurerRank.Bronze, new List<MonsterData>() },
		{ AdventurerRank.Iron, new List<MonsterData>() },
		{ AdventurerRank.Silver, new List<MonsterData>() }
	};

	// Define the daily quest rewards
	private readonly Dictionary<AdventurerRank, int> _expCardRewards = new()
	{
		{ AdventurerRank.Wood, ItemId.JexpCard_UpRank2 },
		{ AdventurerRank.Copper, ItemId.JexpCard_UpRank3 },
		{ AdventurerRank.Bronze, ItemId.JexpCard_UpRank4 },
		{ AdventurerRank.Iron, ItemId.JexpCard_UpRank5 },
		{ AdventurerRank.Silver, ItemId.JexpCard_UpRank6 }
	};

	// Store daily mob information persistently
	private readonly Dictionary<AdventurerRank, DailyQuestInfo> _dailyQuests = new();
	private DateTime _resetDateTime = DateTime.MinValue;
	private int _currentDailyMobId;
	private int _currentDailyMaterialItemId;
	private string _currentDailyMobName;
	private readonly int _requiredItemCount = 25;

	// Rank-up boss data
	private readonly Dictionary<AdventurerRank, Tuple<int, string>> _rankUpBosses = new()
	{
		{ AdventurerRank.Copper, Tuple.Create(58069, "c_firemage_event") },
		{ AdventurerRank.Bronze, Tuple.Create(58069, "c_firemage_event") },
		{ AdventurerRank.Iron, Tuple.Create(58069, "c_firemage_event") },
		{ AdventurerRank.Silver, Tuple.Create(58069, "c_firemage_event") },
        // Add more bosses for higher ranks
    };


	private readonly Dictionary<long, RankUpQuestInfo> _rankUpQuestData = new();
	private readonly Dictionary<AdventurerRank, List<ProceduralQuest>> _proceduralQuests = new();

	[On("PlayerReady")]
	protected void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		var character = args.Character;

		var currentRank = GetAdventurerRank(character);
		var nextRank = (AdventurerRank)((int)currentRank + 1);

		var mapVar = $"RankUpQuest_{nextRank}_Map";
		ZoneServer.Instance.Data.MapDb.TryFind("c_request_1", out var adventurerGuildMap);

		if (character.Map.Id == adventurerGuildMap.Id)
		{
			if (character.IsDead)
				character.Resurrect(ResurrectOptions.NearestRevivalPoint);
		}
		// Check if the player has an ongoing rank-up quest
		else if (character.Variables.Perm.TryGetInt(mapVar, out var mapId)
				&& character.MapId == mapId
				&& ZoneServer.Instance.World.TryGetMap(mapId, out var map))
		{
			// If the boss is dead and quest is completed, rank up the player
			if (character.Variables.Perm.GetBool($"RankUpQuest_{nextRank}_BossDead"))
			{
				SetAdventurerRank(character, nextRank);
				character.Variables.Perm.Remove($"RankUpQuest_{nextRank}_BossDead");
				character.Variables.Perm.Remove($"RankUpQuest_{nextRank}");
				character.Variables.Perm.Remove($"RankUpQuest_{nextRank}_Map");
				character.WorldMessage(LF("Congratulations! You have been promoted to {0} rank!.", nextRank));

				// No need to warp here, the player can stay on the same map
			}
			// If the boss is not found, respawn
			else if (HasRankUpQuest(character, out var targetRank))
			{
				StartRankUpQuest(character, targetRank);
			}
			else
			{
				if (adventurerGuildMap != null)
					character.Warp(new Location(adventurerGuildMap.Id, adventurerGuildMap.DefaultPosition));
			}
		}
	}

	protected override async void Load()
	{
		if (!ZoneServer.Instance.Conf.World.EnableProceduralQuests)
			return;
		await Task.Delay(TimeSpan.FromSeconds(10));
		// Categorize monsters into rank lists based on their level
		var monsterDb = ZoneServer.Instance.Data.MonsterDb;
		var maps = ZoneServer.Instance.World.Maps.GetList();
		var monsters = monsterDb.Entries.Values.Where(monsterData => monsterData.Rank != MonsterRank.Boss
				&& monsterData.Rank != MonsterRank.NPC && monsterData.Rank != MonsterRank.Material
				&& monsterData.Rank != MonsterRank.Neutral && monsterData.Rank != MonsterRank.Pet
				&& monsterData.Faction != FactionType.RootCrystal && monsterData.Level != 0).ToList();
		foreach (var monsterData in monsters)
		{
			var map = maps.Where(map => map.Data.SpawnedMonsterIds.Contains(monsterData.Id)).FirstOrDefault();
			if (map == null || map.Data.Level == 0)
				continue;
			if (monsterData.Level <= 30)
				_mobLists[AdventurerRank.Wood].Add(monsterData);
			else if (monsterData.Level <= 60)
				_mobLists[AdventurerRank.Copper].Add(monsterData);
			else if (monsterData.Level <= 90)
				_mobLists[AdventurerRank.Bronze].Add(monsterData);
			else if (monsterData.Level <= 120)
				_mobLists[AdventurerRank.Iron].Add(monsterData);
			else
				_mobLists[AdventurerRank.Silver].Add(monsterData);
		}

		// Load daily quest information
		LoadDailyQuestInfo();
		InitializeProceduralQuests();

		// Add NPCs
		// AddNpc(155067, L("[Adventurer's Guild] Receptionist"), "c_request_1", -113, 38.5, 45, ReceptionistDialog);
		// AddNpc(155068, L("[Adventurer's Guild] Quartermaster"), "c_request_1", -102, -137, 45, QuartermasterDialog);
		// AddNpc(155069, L("[Adventurer's Guild] Bartender"), "c_request_1", -20, 38, 0, BartenderDialog);
	}

	private void LoadDailyQuestInfo()
	{
		var ranks = Enum.GetValues(typeof(AdventurerRank)).Cast<AdventurerRank>().ToList();
		foreach (var rank in ranks)
		{
			if (rank == AdventurerRank.Unregistered)
				continue;

			_dailyQuests[rank] = new DailyQuestInfo
			{
				MobId = GlobalVariables.Perm.GetInt($"{rank}DailyQuestMonster", 0),
				MaterialItemId = GlobalVariables.Perm.GetInt($"{rank}DailyQuestItem", 0)
			};
		}

		// Load daily quest reset time
		_resetDateTime = GlobalVariables.Perm.Get<DateTime>($"DailyQuestResetTime");
	}

	private void SetDailyMob(AdventurerRank rank)
	{
		// Check if the daily mob needs to be reset (new day)
		if (_resetDateTime.Date < DateTime.Now.Date)
		{
			ResetDailyQuests();
		}

		// If the current rank doesn't have a valid quest, generate one
		if (_dailyQuests[rank].MobId == 0 || _dailyQuests[rank].MaterialItemId == 0)
		{
			GenerateDailyQuest(rank);
		}

		// Set the current mob information from the daily quest for this rank
		_currentDailyMobId = _dailyQuests[rank].MobId;
		_currentDailyMobName = L(GetMonsterName(_currentDailyMobId));
		_currentDailyMaterialItemId = _dailyQuests[rank].MaterialItemId;
	}

	private void ResetDailyQuests()
	{
		var ranks = Enum.GetValues(typeof(AdventurerRank)).Cast<AdventurerRank>().ToList();
		foreach (var rank in ranks)
		{
			if (rank == AdventurerRank.Unregistered)
				continue;

			GenerateDailyQuest(rank);
		}

		_resetDateTime = DateTime.Now.AddDays(1);
		GlobalVariables.Perm.Set($"DailyQuestResetTime", _resetDateTime);

		Log.Debug("Daily quests reset for all ranks.");
	}

	private void GenerateDailyQuest(AdventurerRank rank)
	{
		if (!_mobLists.ContainsKey(rank))
			return;

		var mobList = _mobLists[rank];
		var monster = mobList[_random.Next(mobList.Count)];
		var materialItemId = GetMonsterDropItemId(monster);

		_dailyQuests[rank] = new DailyQuestInfo
		{
			MobId = monster.Id,
			MaterialItemId = materialItemId
		};

		SaveDailyMobInformation(rank);

		Log.Debug($"Daily quest generated for rank {rank}: Mob {L(monster.Name)} (ID {monster.Id})");
	}

	private void LoadRankUpQuestData(Character character)
	{
		foreach (var rank in _rankUpBosses.Keys)
		{
			if (character.Variables.Perm.GetBool($"RankUpQuest_{rank}"))
			{
				// Load the boss ID, name, and map ID from player variables
				var bossId = character.Variables.Perm.GetInt($"RankUpQuest_{rank}_Boss");
				if (!ZoneServer.Instance.Data.MonsterDb.TryFind(bossId, out var boss))
				{
					character.Variables.Perm.Remove($"RankUpQuest_{rank}");
					continue;
				}
				var bossMapId = character.Variables.Perm.GetInt($"RankUpQuest_{rank}_Map");

				_rankUpQuestData[character.ObjectId] = new RankUpQuestInfo
				{
					BossId = bossId,
					BossName = boss.Name,
					BossMapId = bossMapId
				};

				break; // Only one rank-up quest is active at a time
			}
		}
	}

	private void SaveDailyMobInformation(AdventurerRank rank)
	{
		GlobalVariables.Perm.SetInt($"{rank}DailyQuestMonster", _dailyQuests[rank].MobId);
		GlobalVariables.Perm.SetInt($"{rank}DailyQuestItem", _dailyQuests[rank].MaterialItemId);
	}

	private async Task ReceptionistDialog(Dialog dialog)
	{
		var character = dialog.Player;
		var adventurerRank = GetAdventurerRank(character);

		dialog.SetTitle(L("Receptionist"));
		dialog.SetPortrait("Dlg_port_WielderJuanaut");

		await dialog.Intro(L("Welcome to the Adventurer's Guild!"));

		if (adventurerRank == AdventurerRank.Unregistered)
		{
			var selection = await dialog.Select(L("Would you like to register?"),
				Option(L("Yes"), "yes"),
				Option(L("No"), "no")
			);

			if (selection == "yes")
			{
				SetAdventurerRank(character, AdventurerRank.Wood);
				await dialog.Msg(L("Excellent! You are now a Wood Rank adventurer. I will have quests for you soon!"));
				adventurerRank = AdventurerRank.Wood;
				dialog.ModifyMemory(1);
			}
			else
			{
				await dialog.Msg(L("No problem. Let me know if you change your mind."));
				return;
			}
		}
		else if (dialog.GetMemory() > 0)
		{
			await dialog.Msg(LF("Welcome back to the Adventurer's Guild, {0}! Your current rank is {1}.", character.Name, adventurerRank));
		}

		if (adventurerRank == AdventurerRank.Unregistered)
			return;

		LoadRankUpQuestData(character);

		// Combine all quest options into one dialog
		await HandleAllQuestsDialog(dialog, character, adventurerRank);
	}

	private async Task HandleAllQuestsDialog(Dialog dialog, Character character, AdventurerRank adventurerRank)
	{
		var options = new DialogOptionList();

		// Daily Quest Option
		if (!HasCompletedDailyQuest(character))
		{
			SetDailyMob(adventurerRank);
			options.Add(Option(LF("Daily Quest."), "dailyQuest"));
		}

		// Procedural Quest Option
		var availableProceduralQuests = _proceduralQuests[adventurerRank]
			.Where(quest => !character.Variables.Perm.GetBool($"ProceduralQuest_{quest.Id}_Completed"));
		if (availableProceduralQuests.Any())
		{
			options.Add(Option(L("I'd like to hear about other available tasks."), "proceduralQuest"));
		}

		// Rank-up Quest Option
		if (HasRankUpQuest(character, out var targetRank))
		{
			if (!_rankUpQuestData.TryGetValue(character.ObjectId, out var questInfo))
				questInfo = new RankUpQuestInfo();
			options.Add(Option(
				LF("I'm here for the {0} rank-up quest (defeat the {1}).", targetRank, questInfo.BossName),
				"rankUpQuest"
			));
		}
		else if (MeetsRankRequirements(character, (AdventurerRank)((int)adventurerRank + 1)))
		{
			options.Add(Option(L("I'm ready for a promotion!"), "rankUpEligibility"));
		}

		// Nevermind Option
		options.Add(Option(L("Nevermind"), "nevermind"));

		var selection = await dialog.Select(L("What can I help you with today?"), options);

		switch (selection)
		{
			case "dailyQuest":
				await HandleDailyQuest(dialog, character, adventurerRank);
				break;
			case "proceduralQuest":
				await HandleProceduralQuests(dialog, character, adventurerRank);
				break;
			case "rankUpQuest":
				await HandleRankUpQuest(dialog, character, targetRank);
				break;
			case "rankUpEligibility":
				await HandleRankUpEligibility(dialog, character, adventurerRank);
				break;
			case "nevermind":
				await dialog.Msg(L("Alright, feel free to come back when you're ready."));
				break;
		}
	}

	private async Task HandleDailyQuest(Dialog dialog, Character player, AdventurerRank adventurerRank)
	{
		if (adventurerRank == AdventurerRank.Unregistered)
			return;

		var mapDb = ZoneServer.Instance.Data.MapDb;
		var mapData = mapDb.Entries.Values.FirstOrDefault(map => map.SpawnedMonsterIds.Contains(_currentDailyMobId));
		if (mapData == null)
		{
			Log.Debug($"Map Data is null for {_currentDailyMobId}.");
			await dialog.Msg(L("Hmm, something seems to be wrong with my records for today's quest."));
			return;
		}

		var selection = await dialog.Select(LF("Today's quest is to collect {1} {2} drops. These are found on map '{3}' (level {4}).",
			adventurerRank, _requiredItemCount, _currentDailyMobName, L(mapData.Name), mapData.Level),
			Option(L("Turn in items."), "accept"),
			Option(L("Decline"), "decline")
		);

		if (selection == "decline")
		{
			await dialog.Msg(L("Alright, let me know if you change your mind."));
			return;
		}

		// Check if the player has enough items
		var itemCount = player.Inventory.CountItem(_currentDailyMaterialItemId);
		if (itemCount >= _requiredItemCount)
		{
			// Reward the player
			player.Inventory.Remove(_currentDailyMaterialItemId, _requiredItemCount, InventoryItemRemoveMsg.Given);
			player.Inventory.Add(_expCardRewards[adventurerRank], 1, InventoryAddType.New);
			SetDailyQuestCompleted(player);

			await dialog.Msg(LF("Excellent work! Here's your reward: {0}.", GetItemName(_expCardRewards[adventurerRank])));
		}
		else
		{
			await dialog.Msg(LF("You don't have enough {0} drops yet. Keep hunting!", _currentDailyMobName));
		}
	}

	private async Task HandleRankUpQuest(Dialog dialog, Character character, AdventurerRank targetRank)
	{
		if (!_rankUpQuestData.TryGetValue(character.ObjectId, out var questInfo))
		{
			// No rank-up quest data found for the player
			await dialog.Msg(L("You don't have a pending rank-up quest."));
			return;
		}

		if (IsRankUpBossDead(character, targetRank))
		{
			// Rank up the player
			SetAdventurerRank(character, targetRank);
			await dialog.Msg(LF("Congratulations! You have defeated the {0} and have been promoted to {1} rank!",
				L(questInfo.BossName), targetRank));

			// Remove the rank-up quest flag and data
			character.Variables.Perm.Remove($"RankUpQuest_{targetRank}");
			_rankUpQuestData.Remove(character.ObjectId);
		}
		else
		{
			await dialog.Msg(LF("You have a pending rank-up quest to defeat the {0}! Go forth and prove your strength!",
				L(questInfo.BossName)));

			// Optionally, you can re-warp the player to the boss's map
			if (character.Variables.Perm.TryGetInt($"RankUpQuest_{targetRank}_Map", out var mapId) && ZoneServer.Instance.World.TryGetMap(mapId, out var map))
			{
				WarpPlayerToMap(character, map);
			}
		}
	}

	private async Task HandleRankUpEligibility(Dialog dialog, Character character, AdventurerRank currentRank)
	{
		var nextRank = (AdventurerRank)((int)currentRank + 1);
		if (MeetsRankRequirements(character, nextRank))
		{
			await dialog.Msg(L("You are now eligible for a promotion! Are you ready for a challenging task?"));

			var selection = await dialog.Select(LF("Take on the {0} rank-up quest?", nextRank),
				Option(L("Accept"), "acceptRankUp"),
				Option(L("Decline"), "declineRankUp")
			);

			if (selection == "acceptRankUp")
			{
				StartRankUpQuest(character, nextRank);
				var questInfo = _rankUpQuestData[character.ObjectId];
				await dialog.Msg(LF("Excellent! I have summoned a powerful foe for you to defeat. Seek out the {0} on the {1} map! Good luck!",
					L(questInfo.BossName), L(GetMapName(questInfo.BossMapId))));
			}
			else
			{
				await dialog.Msg(L("Very well. Come back when you are ready to face the challenge!"));
			}
		}
		else
		{
			await dialog.Msg(L("Come back tomorrow for another quest!"));
		}
	}

	// Start the rank-up quest for the player
	private void StartRankUpQuest(Character character, AdventurerRank targetRank)
	{
		// Get boss data
		var bossData = _rankUpBosses[targetRank];

		// Spawn the boss on the designated map
		var mapData = ZoneServer.Instance.Data.MapDb.Find(bossData.Item2);
		var map = ZoneServer.Instance.World.TryGetMap(bossData.Item2, out var targetMap) ? targetMap : new DynamicMap(mapData.Id);
		if (map is DynamicMap)
			ZoneServer.Instance.World.AddMap(map);
		map.IsRaid = true;

		var boss = new Mob(bossData.Item1, RelationType.Enemy);
		boss.VisibilityId = character.ObjectId;
		boss.Visibility = ActorVisibility.Individual;
		boss.Level = character.Level + _random.Next(5, 10);
		boss.Vars.SetString("SpawnMap", map.ClassName);

		boss.Components.Add(new MovementComponent(boss));
		boss.Components.Add(new AiComponent(boss, "BasicMonster"));
		boss.Visibility = ActorVisibility.Individual;
		boss.VisibilityId = character.ObjectId;

		if (Versions.Client > KnownVersions.ClosedBeta2)
			boss.StartBuff(BuffId.EliteMonsterBuff);
		boss.InvalidateProperties();

		map.AddMonster(boss);
		boss.Died += (mob, killer) =>
		{
			if (killer is not Character character)
				return;
			if (character.IsDead)
				return;
			// Set marker on player that rank up quest boss is dead.
			character.Variables.Perm.Set($"RankUpQuest_{targetRank}_BossDead", true);

			if (ZoneServer.Instance.Data.MapDb.TryFind("c_request_1", out var toMap))
				CreateWarpPortal(new Location(map.Id, map.Data.DefaultPosition), new Location(toMap.Id, toMap.DefaultPosition), direction: killer.Direction.Backwards.DegreeAngle, durationSeconds: 60);
		};

		// Handle player death
		character.Died += async (character, killer) =>
		{
			// Fail the quest on player death
			character.Variables.Perm.Remove($"RankUpQuest_{targetRank}");
			character.Variables.Perm.Remove($"RankUpQuest_{targetRank}_BossDead");
			character.Variables.Perm.Remove($"RankUpQuest_{targetRank}_Map");
			_rankUpQuestData.Remove(character.ObjectId);

			// Optionally, you can send a message to the player
			character.WorldMessage(L("Your rank-up quest has failed. Moving back to Adventurer Guild."));
			boss.Kill(null);

			await Task.Delay(TimeSpan.FromSeconds(1));
			if (ZoneServer.Instance.Data.MapDb.TryFind("c_request_1", out var toMap))
				character.Warp(new Location(toMap.Id, toMap.DefaultPosition));
		};

		// Store the boss instance
		_rankUpQuestData[character.ObjectId] = new RankUpQuestInfo
		{
			BossId = boss.Data.Id,
			BossName = L(boss.Data.Name),
			BossMapId = map.Id
		};

		// Set the rank-up quest flag
		character.Variables.Perm.Set($"RankUpQuest_{targetRank}", true);

		// Set the rank-up map flag
		character.Variables.Perm.Set($"RankUpQuest_{targetRank}_Map", map.Id);

		// Set the rank-up boss flag
		character.Variables.Perm.Set($"RankUpQuest_{targetRank}_Boss", boss.Data.Id);

		// Warp the player to the boss's map
		if (character.Map.Id != map.Id)
			WarpPlayerToMap(character, map);
	}

	// Warp the player to the specified map
	private void WarpPlayerToMap(Character character, Map map)
	{
		character.Warp(map.Id, map.Data.DefaultPosition);
	}


	// Check if the player has a pending rank-up quest
	private bool HasRankUpQuest(Character character, out AdventurerRank targetRank)
	{
		var currentRank = GetAdventurerRank(character);
		var nextRank = (AdventurerRank)((int)currentRank + 1);
		targetRank = nextRank;

		if (character.Variables.Perm.TryGetBool($"RankUpQuest_{nextRank}", out var hasQuest))
			return hasQuest;

		return false;
	}

	// Check if the rank-up boss for the player is dead
	private bool IsRankUpBossDead(Character character, AdventurerRank targetRank)
	{
		return character.Variables.Perm.GetBool($"RankUpQuest_{targetRank}_BossDead");
	}

	private async Task HandleProceduralQuests(Dialog dialog, Character character, AdventurerRank adventurerRank)
	{
		var availableQuests = _proceduralQuests[adventurerRank]
			.Where(quest => !character.Variables.Perm.GetBool($"ProceduralQuest_{quest.Id}_Completed"));

		if (!availableQuests.Any())
		{
			await dialog.Msg(L("I don't have any other quests for you right now."));
			return;
		}

		var selection = await dialog.Select(L("I have some other tasks available. Would you like to hear about them?"),
			Option(L("Yes"), "yes"),
			Option(L("No"), "no")
		);

		if (selection == "no")
		{
			await dialog.Msg(L("Alright, let me know if you change your mind."));
			return;
		}

		var questOptions = availableQuests.Select(quest =>
			Option(L(quest.Title), quest.Id.ToString())
		).ToArray();

		var chosenQuestId = await dialog.Select(L("Which task are you interested in?"), questOptions);
		if (chosenQuestId == null) return; // Player closed the dialog

		var chosenQuest = availableQuests.FirstOrDefault(quest => quest.Id.ToString() == chosenQuestId);
		if (chosenQuest == null)
		{
			await dialog.Msg(L("Hmm, something seems to be wrong with my records. Please try again later."));
			return;
		}

		// Explain the quest and ask if they want to accept
		await dialog.Msg(L(chosenQuest.Description));

		selection = await dialog.Select(L("Will you accept this task?"),
			Option(L("Yes"), "yes"),
			Option(L("No"), "no")
		);

		if (selection == "yes")
		{
			character.Variables.Perm.Set($"ProceduralQuest_{chosenQuest.Id}_Accepted", true);
			await dialog.Msg(L("Excellent! Good luck."));
		}
		else
		{
			await dialog.Msg(L("Alright, no problem."));
		}
	}

	[On("EntityKilled")]
	protected void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (!ZoneServer.Instance.Conf.World.EnableProceduralQuests)
			return;

		if (args.Attacker is not Character character
			|| args.Target is not Mob mob)
			return;

		var adventurerRank = GetAdventurerRank(character);
		if (adventurerRank == AdventurerRank.Unregistered) return;
		if (!_proceduralQuests.TryGetValue(adventurerRank, out var quests))
			return;
		var acceptedQuests = quests.Where(quest => character.Variables.Perm.GetBool($"ProceduralQuest_{quest.Id}_Accepted"));

		foreach (var quest in acceptedQuests)
		{
			if (quest.TargetMobId == mob.Data.Id)
			{
				var currentCount = character.Variables.Temp.GetInt($"ProceduralQuest_{quest.Id}_Count", 0);
				currentCount++;
				character.Variables.Temp.SetInt($"ProceduralQuest_{quest.Id}_Count", currentCount);

				if (currentCount >= quest.TargetCount)
				{
					// Quest completed!
					character.Variables.Temp.Remove($"ProceduralQuest_{quest.Id}_Count");
					character.Variables.Perm.Set($"ProceduralQuest_{quest.Id}_Completed", true);
					character.Variables.Perm.Remove($"ProceduralQuest_{quest.Id}_Accepted");

					character.GiveExp(quest.RewardExp, quest.RewardExp, mob);
					character.AddItem(ItemId.Vis, quest.RewardSilver);

					character.WorldMessage(LF("You have completed the quest '{0}'!", L(quest.Title)));
				}
				else
				{
					character.WorldMessage(LF("Quest '{0}': {1}/{2} {3} slain.",
						L(quest.Title), currentCount, quest.TargetCount, L(mob.Data.Name)));
				}
			}
		}
	}

	private async Task QuartermasterDialog(Dialog dialog)
	{
		var player = dialog.Player;
		dialog.SetTitle(L("Quartermaster"));
		dialog.SetPortrait("Dlg_port_WielderJuanaut");

		await dialog.Msg(L("Welcome! I offer powerful buffs to aid you on your adventures."));

		var selection = await dialog.Select(L("Which buff would you like to purchase?"),
			Option(L("Increased Attack (1000 Silver)"), "attackBuff", () => player.HasItem(ItemId.Vis, 1000)),
			Option(L("Increased Defense (1500 Silver)"), "defenseBuff", () => player.HasItem(ItemId.Vis, 1500)),
			Option(L("Increased Movement Speed (800 Silver)"), "speedBuff", () => player.HasItem(ItemId.Vis, 800)),
			Option(L("Nevermind"), "nevermind")
		);

		switch (selection)
		{
			case "attackBuff":
				await PurchaseBuff(dialog, player, 1000, BuffId.Rage_Rockto_atk);
				break;
			case "defenseBuff":
				await PurchaseBuff(dialog, player, 1500, BuffId.Rage_Rockto_def);
				break;
			case "speedBuff":
				await PurchaseBuff(dialog, player, 800, BuffId.Rage_Rockto_spd);
				break;
			case "nevermind":
				await dialog.Msg(L("Alright, let me know if you change your mind."));
				break;
		}
	}

	private async Task BartenderDialog(Dialog dialog)
	{
		var player = dialog.Player;
		dialog.SetTitle(L("Bartender"));
		dialog.SetPortrait("Dlg_port_WielderJuanaut");

		await dialog.Msg(L("Welcome! I have a selection of potent potions to enhance your abilities."));

		var selection = await dialog.Select(L("Which potion would you like to purchase?"),
			Option(L("Might Potion (500 Silver)"), "mightPotion", () => player.HasItem(ItemId.Vis, 500)),
			Option(L("Condensed Might Potion (750 Silver)"), "condensedMightPotion", () => player.HasItem(ItemId.Vis, 750)),
			Option(L("Strength Potion (500 Silver)"), "strengthPotion", () => player.HasItem(ItemId.Vis, 500)),
			Option(L("Condensed Strength Potion (750 Silver)"), "condensedStrengthPotion", () => player.HasItem(ItemId.Vis, 750)),
			Option(L("Fire Resistance Potion (600 Silver)"), "fireResistPotion", () => player.HasItem(ItemId.Vis, 600)),
			Option(L("Nevermind"), "nevermind")
		);

		switch (selection)
		{
			case "mightPotion":
				await PurchasePotion(dialog, player, 500, ItemId.Drug_CRTHR_GIMMICK1);
				break;
			case "condensedMightPotion":
				await PurchasePotion(dialog, player, 750, ItemId.Drug_CRTHR_GIMMICK2);
				break;
			case "strengthPotion":
				await PurchasePotion(dialog, player, 500, ItemId.Drug_PATK_GIMMICK1);
				break;
			case "condensedStrengthPotion":
				await PurchasePotion(dialog, player, 750, ItemId.Drug_PATK_GIMMICK2);
				break;
			case "fireResistPotion":
				await PurchasePotion(dialog, player, 600, ItemId.Drug_ResFire_GIMMICK1);
				break;
			case "nevermind":
				await dialog.Msg(L("Alright, come back if you need something."));
				break;
		}
	}

	private async Task PurchaseBuff(Dialog dialog, Character character, int price, BuffId buffId)
	{
		if (!character.HasSilver(price))
		{
			await dialog.Msg(L("You don't have enough silver."));
			return;
		}

		character.RemoveItem(ItemId.Vis, price);
		character.StartBuff(buffId, TimeSpan.Zero, dialog.Npc);
		await dialog.Msg(L("Buff applied!"));
	}

	private async Task PurchasePotion(Dialog dialog, Character character, int price, int itemId)
	{
		if (!character.HasSilver(price))
		{
			await dialog.Msg(L("You don't have enough silver."));
			return;
		}

		character.RemoveItem(ItemId.Vis, price);
		character.AddItem(itemId);
		await dialog.Msg(LF("Here's your {0}!", GetItemName(itemId)));
	}

	private AdventurerRank GetAdventurerRank(Character character)
	{
		var rank = AdventurerRank.Unregistered;
		if (character.Variables.Perm.TryGetInt("AdventurerRank", out var rankInt))
			rank = (AdventurerRank)rankInt;
		return rank;
	}

	private void SetAdventurerRank(Character character, AdventurerRank rank)
	{
		character.Variables.Perm.SetInt("AdventurerRank", (int)rank);
	}

	private bool HasCompletedDailyQuest(Character character)
	{
		return character.Variables.Perm.GetBool($"DailyQuest{DateTime.Now:yyyyMMDD}");
	}

	private void SetDailyQuestCompleted(Character player)
	{
		player.Variables.Perm.Set($"DailyQuest{DateTime.Now:yyyyMMDD}", true);
	}

	private int GetMonsterDropItemId(MonsterData monsterData)
	{
		// Get the first drop item ID from the monster's drop list
		var materialItems = ZoneServer.Instance.Data.ItemDb.Entries.Values
			.Where(a => a.Type == ItemType.Etc && a.Group == ItemGroup.Material)
			.Select(item => item.Id)
			.ToList();

		if (monsterData.Drops.Count > 0)
		{
			return monsterData.Drops.Find(dropItemId => materialItems.Contains(dropItemId.ItemId))?.ItemId ?? 0;
		}

		return 0;
	}

	// Define adventurer ranks
	public enum AdventurerRank
	{
		Unregistered,
		Wood,
		Copper,
		Bronze,
		Iron,
		Silver,
		Gold,
		Platinum,
		Mithril,
		Oricalcum,
		Adamantite
	}

	// Rank requirements
	private readonly Dictionary<AdventurerRank, RankRequirement> _rankRequirements = new Dictionary<AdventurerRank, RankRequirement>()
	{
		{ AdventurerRank.Copper, new RankRequirement(AdventurerRank.Wood, 10, 0, new List<long>()) },
		{ AdventurerRank.Bronze, new RankRequirement(AdventurerRank.Copper, 20, 4, new List<long>()) },
		{ AdventurerRank.Iron, new RankRequirement(AdventurerRank.Bronze, 30, 8, new List<long>()) },
		{ AdventurerRank.Silver, new RankRequirement(AdventurerRank.Iron, 40, 12, new List<long>()) },
		{ AdventurerRank.Gold, new RankRequirement(AdventurerRank.Silver, 50, 20, new List<long> { 1000002 }) }, // Requires completion of quest 1000002
		{ AdventurerRank.Platinum, new RankRequirement(AdventurerRank.Gold, 60, 30, new List<long>()) },
		{ AdventurerRank.Mithril, new RankRequirement(AdventurerRank.Platinum, 70, 40, new List<long>()) },
        // Oricalcum and Adamantite will have special requirements handled separately
    };

	// Check if a character meets the requirements for a specific rank
	private bool MeetsRankRequirements(Character character, AdventurerRank rank)
	{
		// TODO: Remove for production server.
		if (character.PermissionLevel == PermissionLevel.Dev)
			return true;

		if (!_rankRequirements.TryGetValue(rank, out var requirements))
			return false;

		// Check previous rank
		if (GetAdventurerRank(character) < requirements.PreviousRank)
			return false;

		// Check quest completion count
		var completedQuests = character.Quests.GetCompletedQuests();
		if (completedQuests.Count < requirements.QuestCount)
			return false;

		// Check high-rank quest completion count
		var highRankQuestCount = completedQuests.Count(quest => quest.Rank >= (int)rank);
		if (highRankQuestCount < requirements.HighRankQuestCount)
			return false;

		// Check special quest requirements
		foreach (var questId in requirements.RequiredQuestIds)
		{
			if (!character.Quests.HasCompleted(questId))
				return false;
		}

		return true;
	}

	// Check for special requirements for Oricalcum rank
	private bool MeetsOricalcumRequirements(Character character)
	{
		// Example: Check for a specific title or achievement
		return character.Achievements.HasAchievement(101); // Replace with actual achievement ID
	}

	// Check for special requirements for Adamantite rank
	private bool MeetsAdamantiteRequirements(Character character)
	{
		// Example: Check for recommendations from specific NPCs
		return character.Variables.Perm.GetBool("Recommendation_King") && character.Variables.Perm.GetBool("Recommendation_GuildMaster");
	}

	// Define a struct to hold rank requirements
	private struct RankRequirement
	{
		public AdventurerRank PreviousRank;
		public int QuestCount;
		public int HighRankQuestCount;
		public List<long> RequiredQuestIds;

		public RankRequirement(AdventurerRank previousRank, int questCount, int highRankQuestCount, List<long> requiredQuestIds)
		{
			PreviousRank = previousRank;
			QuestCount = questCount;
			HighRankQuestCount = highRankQuestCount;
			RequiredQuestIds = requiredQuestIds;
		}
	}

	// Helper class to store daily quest information
	private class DailyQuestInfo
	{
		public int MobId { get; set; }
		public int MaterialItemId { get; set; }
	}

	// Helper class to store rank-up quest information
	private class RankUpQuestInfo
	{
		public int BossId { get; set; }
		public string BossName { get; set; }
		public int BossMapId { get; set; }
	}

	private class ProceduralQuest
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int TargetMobId { get; set; } // For kill quests
		public int TargetItemId { get; set; } // For collect quests
		public int TargetCount { get; set; }
		public long RewardExp { get; set; }
		public int RewardSilver { get; set; }
	}

	private void InitializeProceduralQuests()
	{
		foreach (var rank in _mobLists.Keys)
		{
			_proceduralQuests[rank] = new List<ProceduralQuest>();

			// Create 3 procedural quests for each rank
			for (int i = 0; i < 5; i++)
			{
				var monsterData = _mobLists[rank][_random.Next(_mobLists[rank].Count)]; // Random monster from the rank's list

				_proceduralQuests[rank].Add(new ProceduralQuest
				{
					Id = 1000000 + (int)rank * 100 + i + 1, // Unique ID based on rank and quest number
					Title = $"Eliminate {L(monsterData.Name)}s",
					Description = $"Hunt down and slay {GetTargetCount(rank)} {L(monsterData.Name)}s.",
					TargetMobId = monsterData.Id,
					TargetCount = GetTargetCount(rank),
					RewardExp = GetRewardExp(rank),
					RewardSilver = GetRewardSilver(rank)
				});
			}
		}
	}

	private int GetTargetCount(AdventurerRank rank)
	{
		// Adjust target count based on rank
		switch (rank)
		{
			case AdventurerRank.Wood: return 5;
			case AdventurerRank.Copper: return 8;
			case AdventurerRank.Bronze: return 12;
			case AdventurerRank.Iron: return 15;
			case AdventurerRank.Silver: return 20;
			default: return 10; // Default count
		}
	}

	private int GetRewardExp(AdventurerRank rank)
	{
		// Adjust EXP reward based on rank
		switch (rank)
		{
			case AdventurerRank.Wood: return 250;
			case AdventurerRank.Copper: return 500;
			case AdventurerRank.Bronze: return 750;
			case AdventurerRank.Iron: return 1000;
			case AdventurerRank.Silver: return 1500;
			default: return 500; // Default EXP
		}
	}

	private int GetRewardSilver(AdventurerRank rank)
	{
		// Adjust silver reward based on rank
		switch (rank)
		{
			case AdventurerRank.Wood: return 1000;
			case AdventurerRank.Copper: return 10000;
			case AdventurerRank.Bronze: return 50000;
			case AdventurerRank.Iron: return 100000;
			case AdventurerRank.Silver: return 200000;
			default: return 200; // Default silver
		}
	}
}
