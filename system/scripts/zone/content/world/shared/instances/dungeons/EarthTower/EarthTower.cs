//--- Melia Script ----------------------------------------------------------
// Earth Tower
//--- Description -----------------------------------------------------------
// The Earth Tower: eight sections of five floors each, entered at Istora
// Ruins. The floors run on the client's mgame data, interpreted here.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Scripting.Shortcuts;

/// <summary>
/// Shared constants and lookups of the Earth Tower.
/// </summary>
public static class EarthTower
{
	public const string FeatureName = "EarthTower";
	public const string ClearedFloorVar = "Laima.EarthTower.ClearedFloor";
	public const string StartFloorVar = "Laima.EarthTower.StartFloor";
	public const int FloorsPerSection = 5;
	public const int RewardAmount = 10;

	private static Dictionary<string, MGameData> _games;

	/// <summary>
	/// Returns the floor mgames, built on first use.
	/// </summary>
	public static Dictionary<string, MGameData> Games
	{
		get
		{
			if (_games == null)
			{
				var games = new Dictionary<string, MGameData>();
				EarthTowerFloors.Load(games);
				_games = games;
			}
			return _games;
		}
	}

	/// <summary>
	/// Returns the name of the given floor's mgame.
	/// </summary>
	public static string GetGameName(int floor) => floor <= 20 ? "M_GTOWER_STAGE_" + floor : "M_GTOWER2_STAGE_" + floor;

	/// <summary>
	/// Returns the instance dungeon class name of the tower the floor is in.
	/// </summary>
	public static string GetDungeonClassName(int floor) => floor <= 20 ? "M_GTOWER_1" : "M_GTOWER_2";

	/// <summary>
	/// Returns the highest floor the character has cleared.
	/// </summary>
	public static int GetClearedFloor(Character character) => character.Variables.Perm.GetInt(ClearedFloorVar, 0);
}

/// <summary>
/// Runs the mgames of one Earth Tower instance.
/// </summary>
public class EarthTowerRun
{
	private const int TickMs = 250;

	private readonly object _syncLock = new();
	private readonly InstanceDungeon _instance;
	private readonly EarthTowerDungeon _script;
	private readonly Dictionary<string, int> _values = new();
	private readonly Dictionary<string, StageState> _stages = new();
	private readonly List<IMonster> _gameMonsters = new();
	private MGameData _game;
	private int _gameVersion;
	private bool _cleared;
	private bool _ended;

	public int FirstFloor { get; }
	public int LastFloor => this.FirstFloor + EarthTower.FloorsPerSection - 1;

	private class StageState
	{
		public MStageData Data;
		public bool Running;
		public DateTime StartTime;
		public int TimeoutSeconds;
		public bool TimeoutWarned;
		public Dictionary<MEventData, int> ExecCounts = new();
		public Dictionary<MEventData, DateTime> LastExec = new();
		public Dictionary<int, List<IMonster>> Spawned = new();
	}

	public EarthTowerRun(InstanceDungeon instance, EarthTowerDungeon script, int firstFloor)
	{
		_instance = instance;
		_script = script;
		this.FirstFloor = firstFloor;
	}

	/// <summary>
	/// Starts the section's first floor and runs the mgames until the
	/// section ends or the instance is destroyed.
	/// </summary>
	public async Task Run()
	{
		lock (_syncLock)
			this.LoadGame(EarthTower.GetGameName(this.FirstFloor));

		var token = _instance.StageCancellationToken;
		while (!_ended && !token.IsCancellationRequested && _instance.State != InstanceState.Destroyed)
		{
			try
			{
				await Task.Delay(TickMs, token);
			}
			catch (OperationCanceledException)
			{
				break;
			}

			lock (_syncLock)
			{
				try
				{
					this.Tick();
				}
				catch (Exception ex)
				{
					Log.Error("EarthTowerRun: Error in '{0}': {1}", _game?.Name, ex);
					_ended = true;
				}
			}
		}

		lock (_syncLock)
			this.RemoveGameMonsters();
	}

	/// <summary>
	/// Moves the character to the arrival point of the given floor.
	/// </summary>
	public void WarpToFloor(Character character, int floor)
	{
		if (!EarthTowerFloors.Arrivals.TryGetValue(floor, out var position))
			return;

		character.SetPosition(position);
		Send.ZC_SET_POS(character);

		if (character.Connection != null)
			Send.ZC_EXEC_CLIENT_SCP(character.Connection, $"OPEN_EARTH_TOWER_OPEN('{floor}', 3, 1)");
	}

	private void LoadGame(string name)
	{
		if (!EarthTower.Games.TryGetValue(name, out var game))
			return;

		foreach (var state in _stages.Values)
			state.Running = false;

		this.RemoveGameMonsters();

		_game = game;
		_gameVersion++;
		_stages.Clear();

		foreach (var stage in game.Stages)
			_stages[stage.Name] = new StageState { Data = stage };

		foreach (var stage in game.Stages.Where(a => a.AutoStart))
			this.StartStage(stage.Name);
	}

	private void Tick()
	{
		var version = _gameVersion;
		var now = DateTime.UtcNow;

		foreach (var state in _stages.Values.ToList())
		{
			if (!state.Running)
				continue;

			if (state.TimeoutSeconds >= 120 && !state.TimeoutWarned && (now - state.StartTime).TotalSeconds >= state.TimeoutSeconds - 60)
			{
				state.TimeoutWarned = true;
				this.Notice("scroll", L("One minute remains!"), 5);
			}

			foreach (var ev in state.Data.Events)
			{
				if (!state.Running || version != _gameVersion || _ended)
					return;

				state.ExecCounts.TryGetValue(ev, out var count);
				if (ev.ExecCount > 0 && count >= ev.ExecCount)
					continue;

				if (ev.IntervalMs > 0 && state.LastExec.TryGetValue(ev, out var last) && (now - last).TotalMilliseconds < ev.IntervalMs)
					continue;

				if (!ev.Conditions.All(a => this.CheckCondition(state, a)))
					continue;

				state.ExecCounts[ev] = count + 1;
				state.LastExec[ev] = now;

				foreach (var call in ev.Calls)
				{
					this.Execute(call);
					if (version != _gameVersion || _ended)
						return;
				}
			}
		}
	}

	private bool CheckCondition(StageState state, MCall call)
	{
		switch (call.Name)
		{
			case "GAME_ST_EVT_COND_TIMECHECK":
				return (DateTime.UtcNow - state.StartTime).TotalSeconds >= call.Int(0);

			case "GAME_ST_EVT_COND_VALUE":
			{
				_values.TryGetValue(call.Str(0), out var value);
				var target = call.Int(2);
				return call.Str(1) switch
				{
					"OVER" or ">=" => value >= target,
					"UNDER" or "<=" => value <= target,
					">" => value > target,
					"<" => value < target,
					"!=" => value != target,
					_ => value == target,
				};
			}

			case "MGAME_EVT_COND_MONCNT":
				return this.CountAlive(call.Str(0)) <= call.Int(1);

			case "MGAME_EVT_COND_MONCNT_OVER":
				return this.CountAlive(call.Str(0)) >= call.Int(1);

			case "MGAME_EVT_COND_PCCNT":
				return this.GetCharacters().Count(a => !a.IsDead) <= call.Int(0);

			case "MGAME_EVT_COND_PCCNT_OVER":
				return this.GetCharacters().Count(a => !a.IsDead) >= call.Int(0);

			case "MGAME_EVT_NO_CONNECTED_PC":
				return !this.GetCharacters().Any();
		}

		return false;
	}

	private void Execute(MCall call)
	{
		switch (call.Name)
		{
			case "GAME_ST_EVT_EXEC_STAGE_START":
				this.StartStage(call.Str(0));
				break;

			case "GAME_ST_EVT_EXEC_STAGE_CLEAR":
			case "GAME_ST_EVT_EXEC_STAGE_DISABLE":
				if (_stages.TryGetValue(call.Str(0), out var stopped))
					stopped.Running = false;
				break;

			case "GAME_ST_EVT_EXEC_STAGE_DESTROY":
				this.DestroyStage(call.Str(0));
				break;

			case "GAME_ST_EVT_EXEC_VALUE":
				_values[call.Str(0)] = call.Int(1);
				break;

			case "MGAME_EVT_EXEC_CREMON":
				foreach (var (state, obj) in this.ResolveObjects(call.Str(0)))
					this.Spawn(state, obj);
				break;

			case "MGAME_EVT_EXEC_DELMON":
				foreach (var (state, obj) in this.ResolveObjects(call.Str(0)))
					this.Remove(state, obj.Key);
				break;

			case "MGAME_EXEC_RUNMGAME":
				this.LoadGame(call.Str(0));
				break;

			case "MGAME_EXEC_ACTORSCP_MAIN":
				if (call.Str(0).StartsWith("TX_GT_REWARD"))
					this.ClearSection();
				break;

			case "MGAME_RETURN":
				this.Return();
				break;

			case "MGAME_END":
				_ended = true;
				break;

			case "MGAME_SET_RAID_ICON":
				this.Notice(call.Str(1), call.Str(0), call.Int(2));
				break;

			case "MGAME_EVT_EXEC_CHANGE_BGM":
				_script.MGameChangeBgm(_instance, call.Str(0));
				break;
		}
	}

	private void StartStage(string name)
	{
		if (!_stages.TryGetValue(name, out var state))
			return;

		state.Running = true;
		state.StartTime = DateTime.UtcNow;
		state.TimeoutSeconds = 0;
		state.TimeoutWarned = false;
		state.ExecCounts.Clear();
		state.LastExec.Clear();

		foreach (var call in state.Data.StartCalls)
		{
			if (call.Name == "MGAME_SET_TIMEOUT")
				state.TimeoutSeconds = call.Int(0);
			else
				this.Execute(call);
		}

		foreach (var obj in state.Data.Objects.Where(a => !a.IsManual))
			this.Spawn(state, obj);
	}

	private void DestroyStage(string name)
	{
		if (!_stages.TryGetValue(name, out var state))
			return;

		state.Running = false;
		foreach (var key in state.Spawned.Keys.ToList())
			this.Remove(state, key);
	}

	private IEnumerable<(StageState, MObjData)> ResolveObjects(string list)
	{
		var parts = list.Split('/', StringSplitOptions.RemoveEmptyEntries);
		for (var i = 0; i + 1 < parts.Length; i += 2)
		{
			if (!_stages.TryGetValue(parts[i], out var state) || !int.TryParse(parts[i + 1], out var key))
				continue;

			var obj = state.Data.Objects.FirstOrDefault(a => a.Key == key);
			if (obj != null)
				yield return (state, obj);
		}
	}

	private int CountAlive(string list)
	{
		var count = 0;
		foreach (var (state, obj) in this.ResolveObjects(list))
		{
			if (state.Spawned.TryGetValue(obj.Key, out var monsters))
				count += monsters.Count(IsAlive);
		}
		return count;
	}

	private static bool IsAlive(IMonster monster)
	{
		if (monster.Map == null)
			return false;

		return monster is not Mob mob || !mob.IsDead;
	}

	private void Spawn(StageState state, MObjData obj)
	{
		if (!ZoneServer.Instance.Data.MonsterDb.TryFind(obj.MonsterId, out var monsterData))
			return;

		if (!state.Spawned.TryGetValue(obj.Key, out var monsters))
			state.Spawned[obj.Key] = monsters = new List<IMonster>();

		monsters.RemoveAll(a => !IsAlive(a));

		var isNpc = obj.IsNeutral || monsterData.Rank == MonsterRank.NPC || monsterData.Rank == MonsterRank.MISC || monsterData.Faction == FactionType.Neutral;
		var rnd = RandomProvider.Get();

		while (monsters.Count < obj.GenCount)
		{
			var position = obj.Range > 0 ? obj.Position.GetRandomInRange2D(obj.Range, rnd) : obj.Position;
			if (!_instance.Map.Ground.IsValidPosition(position))
				position = obj.Position;

			var monster = isNpc ? (IMonster)this.SpawnNpc(obj, position) : this.SpawnMob(state, obj, position);
			if (monster == null)
				return;

			monsters.Add(monster);
		}
	}

	private Npc SpawnNpc(MObjData obj, Position position)
	{
		var name = obj.IsHidden ? "UnvisibleName" : obj.Name ?? "";
		var npc = new Npc(obj.MonsterId, name, position, new Direction(obj.Angle))
		{
			Layer = _instance.Layer,
		};

		if (obj.EnterFunc != null && obj.EnterFunc.StartsWith("G_TOWER_WARP_TO_") && int.TryParse(obj.EnterFunc.Substring("G_TOWER_WARP_TO_".Length), out var floor))
		{
			npc.SetTriggerArea(Spot(position.X, position.Z, obj.EnterRange));
			npc.SetEnterTrigger(obj.EnterFunc, args =>
			{
				if (args.Initiator is Character character && character.Layer == _instance.Layer)
					this.WarpToFloor(character, floor);
			});
		}

		_instance.Map.AddMonster(npc);
		_instance.RegisterPersistentMonster(npc);
		_gameMonsters.Add(npc);

		return npc;
	}

	private Mob SpawnMob(StageState state, MObjData obj, Position position)
	{
		var mob = _script.SpawnMonster(_instance, obj.MonsterId, position);
		mob.Direction = new Direction(obj.Angle);
		if (obj.IsPassive)
			mob.Tendency = TendencyType.Peaceful;

		if (obj.DeadCalls.Length > 0)
		{
			mob.Died += (dead, killer) =>
			{
				lock (_syncLock)
				{
					foreach (var call in obj.DeadCalls)
						this.ExecuteDead(dead, call);
				}
			};
		}

		_gameMonsters.Add(mob);
		return mob;
	}

	private void ExecuteDead(Mob mob, MCall call)
	{
		switch (call.Name)
		{
			case "SAI_DEAD_ADD_MGAME_V_NAME":
				this.AddValue(call.Str(1), call.Int(2));
				break;

			case "S_AI_DEAD_GTOWER_DM_NAME":
				this.NoticeRemaining(call.Str(1), call.Int(2), call.Str(3), call.Str(4), call.Int(5));
				break;

			case "SAI_DEAD_ADD_VALUE_NOTICE":
				this.AddValue(call.Str(1), call.Int(2));
				this.NoticeRemaining(call.Str(1), call.Int(3), call.Str(4), call.Str(5), call.Int(6));
				break;

			case "S_AI_DEAD_ADD_BUFF":
			{
				if (!ZoneServer.Instance.Data.BuffDb.TryFind(call.Str(0), out var buffData))
					break;

				var range = call.Int(5);
				var duration = TimeSpan.FromMilliseconds(call.Int(3));
				foreach (var character in this.GetCharacters().Where(a => !a.IsDead && a.Position.Get2DDistance(mob.Position) <= range))
					character.StartBuff(buffData.Id, call.Int(1), 0, duration, character);
				break;
			}
		}
	}

	private void AddValue(string name, int amount)
	{
		_values.TryGetValue(name, out var value);
		_values[name] = value + amount;
	}

	private void NoticeRemaining(string name, int total, string text, string icon, int seconds)
	{
		_values.TryGetValue(name, out var value);
		var remaining = total - value;
		if (remaining >= 0)
			this.Notice(icon, $"{text}: {remaining}", seconds);
	}

	private void Notice(string icon, string text, int seconds)
	{
		if (string.IsNullOrEmpty(icon) || icon == "!")
			icon = "scroll";

		_script.MGameMessage(_instance, "NOTICE_Dm_" + icon, text, seconds);
	}

	private void Remove(StageState state, int key)
	{
		if (!state.Spawned.TryGetValue(key, out var monsters))
			return;

		foreach (var monster in monsters)
		{
			monster.Map?.RemoveMonster(monster);
			_gameMonsters.Remove(monster);
		}
		monsters.Clear();
	}

	private void RemoveGameMonsters()
	{
		foreach (var monster in _gameMonsters.OfType<Mob>().ToList())
		{
			monster.Map?.RemoveMonster(monster);
			_gameMonsters.Remove(monster);
		}
	}

	private List<Character> GetCharacters()
	{
		return _instance.Characters.Where(a => a != null && a.IsOnline && a.Map?.Id == _instance.MapId && a.Layer == _instance.Layer).ToList();
	}

	private void ClearSection()
	{
		if (_cleared)
			return;

		_cleared = true;

		var tower = this.FirstFloor <= 20 ? 1 : 2;
		var fragment = tower == 1 ? "misc_earthTower" : "misc_earthTower_2";
		var essence = $"misc_earthTower{this.LastFloor}_boss";

		foreach (var character in this.GetCharacters())
		{
			character.AddItem(fragment, EarthTower.RewardAmount);
			character.AddItem(essence, EarthTower.RewardAmount);

			if (EarthTower.GetClearedFloor(character) < this.LastFloor)
				character.Variables.Perm.SetInt(EarthTower.ClearedFloorVar, this.LastFloor);
		}

		_script.DungeonComplete(_instance);
	}

	private void Return()
	{
		_ended = true;
		if (_cleared)
			return;

		var characters = this.GetCharacters();
		_script.DungeonEnded(_instance, true);

		foreach (var character in characters)
			character.Warp(character.GetCityReturnLocation());
	}
}

/// <summary>
/// The single stage of an Earth Tower instance, which hosts its mgame run.
/// </summary>
public class EarthTowerStage : DungeonStage
{
	public EarthTowerRun Run { get; private set; }

	public EarthTowerStage(DungeonScript script) : base(script, "earth_tower")
	{
	}

	public override async Task Initialize(InstanceDungeon instance)
	{
		await base.Initialize(instance);

		var firstFloor = instance.Owner?.Variables.Temp.GetInt(EarthTower.StartFloorVar, 0) ?? 0;
		if (firstFloor <= 0)
			firstFloor = ((EarthTowerDungeon)this.DungeonScript).FirstFloor;

		this.Run = new EarthTowerRun(instance, (EarthTowerDungeon)this.DungeonScript, firstFloor);
		CallSafe(this.Run.Run());
	}

	public override bool IsObjectiveComplete() => false;
}

/// <summary>
/// Base script of the two Earth Tower areas.
/// </summary>
public abstract class EarthTowerDungeon : DungeonScript
{
	public abstract int FirstFloor { get; }

	protected override bool UseDefaultTimer => false;
	public override bool UseRewardHud => false;

	protected override List<DungeonStage> GetDungeonStages()
	{
		return new List<DungeonStage> { new EarthTowerStage(this) };
	}
}

[DungeonScript("M_GTOWER_INIT")]
public class EarthTowerLolopantherDungeon : EarthTowerDungeon
{
	public override int FirstFloor => 1;

	protected override void Load()
	{
		this.SetId("M_GTOWER_INIT");
		this.SetName("Earth Tower Lolopanther Area");
		this.SetMapName("mission_groundtower_1");
		this.SetStartPosition(new Position(3175, 270, -5998));
	}
}

[DungeonScript("M_GT2_INIT")]
public class EarthTowerSolmikiDungeon : EarthTowerDungeon
{
	public override int FirstFloor => 21;

	protected override void Load()
	{
		this.SetId("M_GT2_INIT");
		this.SetName("Earth Tower Solmiki Area");
		this.SetMapName("mission_groundtower_2");
		this.SetStartPosition(new Position(5803, 147, -6425));
	}
}

/// <summary>
/// The Earth Tower entrance at Istora Ruins.
/// </summary>
public class EarthTowerEntranceScript : GeneralScript
{
	protected override void Load()
	{
		if (!Feature.IsEnabled(EarthTower.FeatureName))
			return;

		AddNpc(154013, L("[Earth Tower] Kupole Lutha"), "f_remains_37_3", -2700, 2640, 45, this.LuthaDialog);
	}

	private async Task LuthaDialog(Dialog dialog)
	{
		var character = dialog.Player;
		dialog.SetTitle(L("Kupole Lutha"));

		var cleared = EarthTower.GetClearedFloor(character);
		var options = new List<DialogOption>();
		for (var floor = 1; floor <= 40; floor += EarthTower.FloorsPerSection)
		{
			if (floor - 1 > cleared)
				break;

			var area = floor <= 20 ? L("Lolopanther") : L("Solmiki");
			options.Add(Option(LF("{0}: Floors {1}-{2}", area, floor, floor + EarthTower.FloorsPerSection - 1), floor.ToString()));
		}
		options.Add(Option(L("Leave"), "leave"));

		var selection = await dialog.Select(LF("The Earth Tower tests those who climb it, five floors at a time. Each section you clear opens the next.{nl}{nl}Highest floor cleared: {0}", cleared), options);
		if (!int.TryParse(selection, out var startFloor))
			return;

		this.Enter(character, startFloor);
	}

	private void Enter(Character character, int startFloor)
	{
		if (!ZoneServer.Instance.Data.InstanceDungeonDb.TryGet(EarthTower.GetDungeonClassName(startFloor), out var dungeonData))
			return;

		if (character.Level < dungeonData.Level)
		{
			character.ServerMessage(LF("You need to be at least level {0} to enter.", dungeonData.Level));
			return;
		}

		if (!character.HasParty)
			ZoneServer.Instance.World.Parties.Create(character);

		var party = character.Connection.Party;
		if (party == null || !party.IsLeader(character))
		{
			character.ServerMessage(L("Only the party leader can lead the party into the Earth Tower."));
			return;
		}

		if (DungeonScript.GetInstance(character.DbId) != null)
		{
			character.ServerMessage(L("Your party is already inside a dungeon."));
			return;
		}

		var members = party.GetPartyMembers().Where(a => a != null && a.Map?.Id == character.Map?.Id).ToList();
		foreach (var member in members)
		{
			if (member.Dungeon.GetCurrentEntryCount(dungeonData.Id) >= member.Dungeon.GetMaxEntryCount(dungeonData.Id))
			{
				member.SystemMessage("CannotJoinIndunYet");
				if (member != character)
					character.ServerMessage(L("A party member has exceeded their dungeon entry limit."));
				return;
			}
		}

		character.Variables.Temp.SetInt(EarthTower.StartFloorVar, startFloor);
		DungeonScript.WarpPartyToDungeon(members, dungeonData.Id);
	}

	[ScriptableFunction("TX_EARTH_TOWER_SHOP_TREAD")]
	public DialogTxResult EARTH_TOWER_SHOP_TREAD(Character character, DialogTxArgs args)
		=> Trade(character, args, "EarthTower");

	[ScriptableFunction("TX_EARTH_TOWER_SHOP_TREAD2")]
	public DialogTxResult EARTH_TOWER_SHOP_TREAD2(Character character, DialogTxArgs args)
		=> Trade(character, args, "EarthTower2");

	/// <summary>
	/// Trades the given materials for an item of the given trade shop.
	/// </summary>
	private static DialogTxResult Trade(Character character, DialogTxArgs args, string shopType)
	{
		if (args.NumArgs.Length < 2)
			return DialogTxResult.Fail;

		var amount = args.NumArgs[1];
		if (amount < 1 || !ZoneServer.Instance.Data.TradeShopDb.TryFind(args.NumArgs[0], out var shopItem) || shopItem.ShopType != shopType)
			return DialogTxResult.Fail;

		foreach (var required in shopItem.RequiredItems)
		{
			var given = args.TxItems.Where(a => a.Item.Id == required.Key).Sum(a => a.Amount);
			if (given < required.Value * amount)
				return DialogTxResult.Fail;
		}

		foreach (var required in shopItem.RequiredItems)
		{
			var remaining = required.Value * amount;
			foreach (var txItem in args.TxItems.Where(a => a.Item.Id == required.Key))
			{
				var take = Math.Min(remaining, txItem.Amount);
				character.Inventory.Remove(txItem.Item.ObjectId, take, InventoryItemRemoveMsg.Given);
				remaining -= take;
				if (remaining <= 0)
					break;
			}
		}

		character.AddItem(shopItem.ItemCraftedClassName, shopItem.ItemCraftedCount * amount);
		character.AddonMessage(AddonMessage.EARTHTOWERSHOP_BUY_ITEM, shopItem.ItemCraftedClassName);
		character.AddonMessage(AddonMessage.EARTHTOWERSHOP_BUY_ITEM_RESULT, shopType + "/0/0");

		return DialogTxResult.Okay;
	}
}
