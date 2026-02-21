using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Extensions;

namespace Melia.Shared.Data.Database
{
	public enum InstanceDungeonResetType
	{
		None,
		PC,
		ACCOUNT
	}

	public enum InstanceDungeonRaidType
	{
		None,
		PartyNormal,
		PartyHard,
		Solo,
		SoloHard,
		AutoNormal,
		AutoHard
	}

	public enum InstanceDungeonType
	{
		None,
		KlaipeMission,
		Indun,
		GTower,
		AbbeyMission,
		DefenceMission,
		Raid,
		MissionIndun,
		Event,
		Solo_dungeon,
		WeeklyRaid,
		FreeDungeon,
		FieldBossRaid,
		MythicDungeon_Auto,
		MythicDungeon_Auto_Hard,
		TOSHero,
		EarringRaid,
		BridgeWailing,
		Challenge_Solo,
		Challenge_Auto,
		SeasonEarringRaid
	}

	public enum InstanceDungeonTicketingType
	{
		None,
		Entrance_Ticket
	}

	[Serializable]
	public class InstanceDungeonData
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ClassName { get; set; }
		public int GearScore { get; set; }
		public int AbilityScore { get; set; }
		public int Level { get; set; }
		public int MaxLevel { get; set; }
		public int MaxPlayers { get; set; }
		public List<ItemData> RewardsItems { get; set; } = new List<ItemData>();
		public bool EnableInfiniteEnter { get; set; }
		public int PlayPerReset { get; set; }
		public int PlayPerResetType { get; set; }
		public bool AutoMatchEnable { get; set; }
		public int RewardPerReset { get; set; }
		public List<MonsterData> BossList { get; set; } = new List<MonsterData>();
		public int WeeklyEnterableCount { get; set; }
		public int RewardContribution { get; set; }
		public int RewardExp { get; set; }
		public int RewardSilver { get; set; }
		public InstanceDungeonResetType ResetType { get; set; }
		public MapData Map { get; set; }
		public MapData StartMap { get; set; }
		public string StartNPCDialog { get; set; }
		public ItemData ClearDungeonItem { get; set; }
		public bool EnableUnderStaffEntry { get; set; }
		public InstanceDungeonRaidType RaidType { get; set; }
		public string MiniGame { get; set; }
		public ItemData AdmissionItem { get; set; }
		public int AdmissionItemCount { get; set; }
		public List<ItemData> ItemList { get; set; } = new List<ItemData>();
		public bool AutoSweepEnable { get; set; }
		public InstanceDungeonType DungeonType { get; set; }
		public int PartyDeadCountLimit { get; set; }
		public InstanceDungeonTicketingType TicketingType { get; set; }
		public string CheckCountName { get; set; }
		public string MapName { get; set; }

		/// <summary>
		/// Determinate if a given mapId is part of a dungeon.
		/// </summary>
		/// <returns></returns>
		public bool IsDungeonMap()
		{
			if (this.Map == null)
				return false;

			return this.DungeonType == InstanceDungeonType.Indun;
		}
	}

	/// <summary>
	/// Instance dungeon database.
	/// </summary>
	public class InstanceDungeonDb : DatabaseJsonIndexed<int, InstanceDungeonData>
	{
		private readonly MapDb _mapDb;
		private readonly ItemDb _itemDb;
		private readonly MonsterDb _monsterDb;

		/// <summary>
		/// Creates new instanced dungeon db.
		/// </summary>
		/// <param name="mapDb"></param>
		public InstanceDungeonDb(MapDb mapDb, ItemDb itemDb, MonsterDb monsterDb)
		{
			_mapDb = mapDb;
			_itemDb = itemDb;
			_monsterDb = monsterDb;
		}

		/// <summary>
		/// Find a InstanceDungeon or null with a given class name.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public bool TryGet(string className, out InstanceDungeonData data)
		{
			data = this.Entries.Values.FirstOrDefault(a => a.ClassName.ToLowerInvariant() == className.ToLowerInvariant());
			return data != null;
		}

		/// <summary>
		/// Find a InstanceDungeon or null with a given map class name.
		/// </summary>
		/// <param name="mapClassName"></param>
		/// <returns></returns>
		public bool TryGetByMapClassName(string mapClassName, out InstanceDungeonData data)
		{
			data = this.Entries.Values.FirstOrDefault(a => a.Map?.ClassName.ToLowerInvariant() == mapClassName.ToLowerInvariant());
			return data != null;
		}

		/// <summary>
		/// Returns true if the given mapId is a dungeon.
		/// </summary>
		/// <returns></returns>
		public bool IsDungeonMap(int mapId)
		{
			var instanceDungeonData = this.Entries.Values.First(a => a.Map?.Id == mapId);
			if (instanceDungeonData == null)
				return false;

			return instanceDungeonData.DungeonType == InstanceDungeonType.Indun;
		}

		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "className", "name", "level", "maxLevel", "maxPlayers");

			var data = new InstanceDungeonData();

			data.Id = entry.ReadInt("id");
			data.ClassName = entry.ReadString("className");
			data.Name = entry.ReadString("name");
			data.GearScore = entry.ReadInt("gearScore");
			data.AbilityScore = entry.ReadInt("abilityScore");
			data.Level = entry.ReadInt("level");
			data.MaxLevel = entry.ReadInt("maxLevel");
			data.MaxPlayers = entry.ReadInt("maxPlayers");
			data.MapName = entry.ReadString("mapName");

			var rewardItemsString = entry.ReadString("rewardItem");

			if (!rewardItemsString.IsNullOrWhiteSpace())
			{
				foreach (var itemString in rewardItemsString.Split('/').ToList())
				{
					var itemData = _itemDb.FindByClass(itemString);
					if (itemData != null)
					{
						data.RewardsItems.Add(itemData);
					}
				}
			}

			data.EnableInfiniteEnter = entry.ReadBool("enableInfiniteEnter");
			data.PlayPerReset = entry.ReadInt("playPerReset");
			data.PlayPerResetType = entry.ReadInt("playPerResetType");
			data.AutoMatchEnable = entry.ReadBool("autoMatch");
			data.RewardPerReset = entry.ReadInt("rewardPerReset");

			var bossListString = entry.ReadString("bossList");

			if (!bossListString.IsNullOrWhiteSpace())
			{
				foreach (var bossString in bossListString.Split('/').ToList())
				{
					if (_monsterDb.TryFind(bossString, out MonsterData monsterData))
					{
						data.BossList.Add(monsterData);
					}
				}
			}

			data.WeeklyEnterableCount = entry.ReadInt("weeklyEnterableCount");
			data.RewardContribution = entry.ReadInt("rewardContribution");
			data.RewardExp = entry.ReadInt("rewardExperience");
			data.RewardSilver = entry.ReadInt("rewardSilver");

			if (Enum.TryParse<InstanceDungeonResetType>(entry.ReadString("resetType"), out var result))
				data.ResetType = result;
			else
				data.ResetType = InstanceDungeonResetType.None;

			if (_mapDb.TryFind(data.MapName, out var mapData))
			{
				data.Map = mapData;
			}

			if (_mapDb.TryFind(entry.ReadString("startMap"), out var startMapData))
			{
				data.StartMap = startMapData;
			}

			data.StartNPCDialog = entry.ReadString("startNPCDialog");
			var clearDungeonItemClsStr = entry.ReadString("clearDungeonItem");

			if (!string.IsNullOrEmpty(clearDungeonItemClsStr))
			{
				var clearDungeoItemCls = clearDungeonItemClsStr.Substring(0, clearDungeonItemClsStr.Length - 1);

				if (_itemDb.TryFind(clearDungeoItemCls, out var clearDungeonItemData))
				{
					data.ClearDungeonItem = clearDungeonItemData;
				}
				else
				{
					if (_itemDb.TryFind("Premium_" + clearDungeoItemCls, out var clearDungeonItemData2))
						data.ClearDungeonItem = clearDungeonItemData2;
				}
			}

			data.EnableUnderStaffEntry = entry.ReadBool("EnableUnderStaffEntry");

			if (Enum.TryParse<InstanceDungeonRaidType>(entry.ReadString("raidType"), out var instanceDungeonRaidType))
				data.RaidType = instanceDungeonRaidType;
			else
				data.RaidType = InstanceDungeonRaidType.None;

			data.MiniGame = entry.ReadString("miniGame");

			if (_itemDb.TryFind(entry.ReadString("admissionItemName"), out var admissionItem))
			{
				data.AdmissionItem = admissionItem;
			}

			data.AdmissionItemCount = entry.ReadInt("admissionItemCount");

			var itemListString = entry.ReadString("itemList");

			if (!itemListString.IsNullOrWhiteSpace())
			{
				foreach (var itemClassName in itemListString.Split('/').ToList())
				{
					if (_itemDb.TryFind(itemClassName, out var itemData))
					{
						data.ItemList.Add(itemData);
					}
				}
			}

			data.AutoSweepEnable = entry.ReadBool("autoSweepEnable");

			if (Enum.TryParse<InstanceDungeonType>(entry.ReadString("type"), out var dungeonType))
				data.DungeonType = dungeonType;
			else
				data.DungeonType = InstanceDungeonType.None;


			data.PartyDeadCountLimit = entry.ReadInt("partyDeadCountLimit");

			if (Enum.TryParse<InstanceDungeonTicketingType>(entry.ReadString("ticketingType"), out var instanceDungeonTicketingType))
				data.TicketingType = instanceDungeonTicketingType;
			else
				data.TicketingType = InstanceDungeonTicketingType.None;

			data.CheckCountName = entry.ReadString("checkCountName");

			this.Entries[data.Id] = data;
		}
	}
}
