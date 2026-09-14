//--- Melia Script ----------------------------------------------------------
// World Map
//--- Description -----------------------------------------------------------
// Makes changes to the world and mini maps, such as removing the level
// ranges, to accomodate customization better. Also handles sending of
// default icons, such as for warps.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Melia.Shared.L10N;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;

public class WorldMapClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadAllScripts();
		this.LoadIesMods();

		ZoneServer.Instance.ServerEvents.PlayerCompletedQuest.Subscribe(this.OnPlayerCompletedQuest);
		ZoneServer.Instance.ServerEvents.PlayerStartedQuest.Subscribe(this.OnPlayerStartedQuest);
		ZoneServer.Instance.ServerEvents.PlayerAbandonedQuest.Subscribe(this.OnPlayerAbandonedQuest);
		ZoneServer.Instance.ServerEvents.PlayerQuestObjectivesCompleted.Subscribe(this.OnPlayerQuestObjectivesCompleted);
	}

	protected override void Ready(Character character)
	{
		this.SendAllScripts(character);
		this.SendIcons(character);
	}

	private void OnPlayerCompletedQuest(object sender, PlayerCompletedQuestEventArgs args)
	{
		this.SendIcons(args.Character);
		this.SendRawLuaScript(args.Character, "imcSound.PlaySoundEvent(\"quest_success_3\")");
	}

	private void OnPlayerStartedQuest(object sender, PlayerStartedQuestEventArgs args)
	{
		this.SendIcons(args.Character);
		this.SendRawLuaScript(args.Character, "imcSound.PlaySoundEvent(\"quest_event_click\")");
	}

	private void OnPlayerAbandonedQuest(object sender, PlayerAbandonedQuestEventArgs args)
	{
		this.SendIcons(args.Character);
	}

	private void OnPlayerQuestObjectivesCompleted(object sender, PlayerQuestObjectivesCompletedEventArgs args)
	{
		this.SendIcons(args.Character);
		this.SendRawLuaScript(args.Character, "imcSound.PlaySoundEvent(\"quest_ui_alarm_2\")");
	}

	private void LoadIesMods()
	{
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 1, "Name", "Klaipeda Area"); // Episode1
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 2, "Name", "Fedimian Area"); // Episode5
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 3, "Name", "Orsha Area"); // Episode13~15
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 101, "Name", "Area 4"); // Episode2
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 102, "Name", "Area 5"); // Episode3
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 103, "Name", "Area 6"); // Episode4
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 104, "Name", "Area 7"); // Episode6
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 105, "Name", "Area 8"); // Episode7-1
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 106, "Name", "Area 9"); // Episode7-2
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 107, "Name", "Area 10"); // Episode7-3
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 108, "Name", "Area 11"); // Episode8-1
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 109, "Name", "Area 12"); // Episode8-2
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 110, "Name", "Area 13"); // Episode9-1
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 111, "Name", "Area 14"); // Episode9-2
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 112, "Name", "Area 15"); // Episode10
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 113, "Name", "Waters"); // Episode11 1
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 114, "Name", "Layered Castle Wall"); // Episode11 2
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 115, "Name", "Maple Forest"); // Episode11 3
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 116, "Name", "Eternal Resting Place"); // Episode12
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 201, "Name", "Area 16"); // sub_episode1
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 202, "Name", "Coast"); // sub_episode2
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 203, "Name", "Suburb of Fallen City"); // sub_episode3
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 204, "Name", "Shore"); // sub_episode4
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 205, "Name", "White Tree Forest"); // sub_episode5
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 206, "Name", "Nicopolis"); // sub_episode6
		ZoneServer.Instance.IesMods.Add("worldmap2_data", 207, "Name", "Memorial"); // sub_episode7
	}

	private void SendIcons(Character character)
	{
		var mapClassName = character.Map.ClassName;
		var icons = new List<LuaTable>();

		var warps = character.Map.GetMonsters(static a => a is WarpMonster);
		foreach (WarpMonster warp in warps)
		{
			var tooltip = "";
			if (ZoneServer.Instance.Data.MapDb.TryFind(warp.WarpLocation.MapId, out var targetMapData))
				tooltip = string.Format(Localization.Get("To {0}"), Localization.Get(targetMapData.Name));

			icons.Add(CreateIconTable("minimap_portal", mapClassName, warp.Position, tooltip));
		}

		this.AppendQuestIcons(character, mapClassName, icons);
		this.SendIconBatches(character, icons);
	}

	private void SendIconBatches(Character character, List<LuaTable> icons)
	{
		var isFirstBatch = true;
		var funcName = "Melia.World.Icons.Load";
		var pending = new List<LuaTable>();

		void Flush()
		{
			var batch = new LuaTable();
			foreach (var entry in pending)
				batch.Insert(entry);

			this.SendRawLuaScript(character, funcName + "(" + batch.Serialize() + ")");
			pending.Clear();

			if (isFirstBatch)
			{
				funcName = "Melia.World.Icons.LoadMore";
				isFirstBatch = false;
			}
		}

		foreach (var iconTable in icons)
		{
			pending.Add(iconTable);

			var trial = new LuaTable();
			foreach (var entry in pending)
				trial.Insert(entry);

			var serializedLength = funcName.Length + 2 + trial.SerializedSize;
			if (serializedLength > ScriptMaxLength)
			{
				if (pending.Count == 1)
				{
					Flush();
					continue;
				}

				pending.RemoveAt(pending.Count - 1);
				Flush();
				pending.Add(iconTable);
			}
		}

		if (isFirstBatch || pending.Count > 0)
			Flush();
	}

	private void AppendQuestIcons(Character character, string mapClassName, List<LuaTable> icons)
	{
		var turnInNpcs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		foreach (var quest in character.Quests.GetInProgress())
		{
			if (!quest.ObjectivesCompleted)
				continue;

			var data = quest.Data;
			var turnInNpcName = !string.IsNullOrEmpty(data.EndNpcUniqueName) ? data.EndNpcUniqueName : data.StartNpcUniqueName;
			if (string.IsNullOrEmpty(turnInNpcName))
				continue;

			var preferredMap = !string.IsNullOrEmpty(data.EndNpcUniqueName) ? null : data.QuestGiverLocation;
			if (!TryFindNpcAcrossMaps(turnInNpcName, out var turnInNpc, out var turnInMapClassName, preferredMap))
				continue;

			var imageName = GetQuestCompleteIconImage(data.Type);
			var tooltip = string.IsNullOrEmpty(data.Name) ? "" : Localization.Get(data.Name);
			icons.Add(CreateIconTable(imageName, turnInMapClassName, turnInNpc.Position, tooltip));
			turnInNpcs.Add(turnInNpcName);
		}

		foreach (var script in QuestScript.GetAll())
		{
			var data = script.Data;
			if (data.StartNpcUniqueName == null)
				continue;
			var isRepeat = data.Type == QuestType.Repeat;

			if (!isRepeat && character.Quests.IsActive(data.Id))
				continue;

			if (!isRepeat && character.Quests.HasCompleted(data.Id))
				continue;

			if (!character.Quests.MeetsPrerequisites(data.Id))
				continue;

			if (turnInNpcs.Contains(data.StartNpcUniqueName))
				continue;

			if (!TryFindNpcAcrossMaps(data.StartNpcUniqueName, out var startNpc, out var giverMapClassName, data.QuestGiverLocation))
				continue;

			var imageName = GetQuestIconImage(data.Type);
			var tooltip = string.IsNullOrEmpty(data.Name) ? "" : Localization.Get(data.Name);
			icons.Add(CreateIconTable(imageName, giverMapClassName, startNpc.Position, tooltip));
		}
	}

	private static bool TryFindNpcAcrossMaps(string name, out MonsterInName npc, out string mapClassName, string preferredMapClassName = null)
	{
		npc = null;
		mapClassName = null;

		if (!string.IsNullOrEmpty(preferredMapClassName) && ZoneServer.Instance.World.TryGetMap(preferredMapClassName, out var preferredMap))
		{
			npc = FindNpcOnMap(preferredMap, name);
			if (npc != null)
			{
				mapClassName = preferredMap.ClassName;
				return true;
			}
		}

		foreach (var map in ZoneServer.Instance.World.Maps.GetList())
		{
			var found = FindNpcOnMap(map, name);
			if (found != null)
			{
				npc = found;
				mapClassName = map.ClassName;
				return true;
			}
		}

		return false;
	}

	private static MonsterInName FindNpcOnMap(Melia.Zone.World.Maps.Map map, string name)
	{
		var normalized = NormalizeNpcName(name);
		var npcs = map.GetNpcs(a =>
			string.Equals(a.UniqueName, name, StringComparison.OrdinalIgnoreCase) ||
			string.Equals(NormalizeNpcName(a.Name), normalized, StringComparison.OrdinalIgnoreCase));
		return npcs.Count > 0 ? npcs[0] : null;
	}

	private static string NormalizeNpcName(string name)
	{
		if (string.IsNullOrEmpty(name))
			return name;
		return name.Replace("{nl}", " ").Trim();
	}

	private static string GetQuestIconImage(QuestType type)
	{
		switch (type)
		{
			case QuestType.Main: return "minimap_1_MAIN";
			case QuestType.Repeat: return "minimap_1_REPEAT";
			case QuestType.Party: return "minimap_1_PARTY";
			case QuestType.KeyItem: return "minimap_1_KEYQUEST";
			default: return "minimap_1_SUB";
		}
	}

	private static string GetQuestCompleteIconImage(QuestType type)
	{
		switch (type)
		{
			case QuestType.Main: return "minimap_3_MAIN";
			case QuestType.Repeat: return "minimap_3_REPEAT";
			case QuestType.Party: return "minimap_3_PARTY";
			case QuestType.KeyItem: return "minimap_3_KEYQUEST";
			default: return "minimap_3_SUB";
		}
	}

	private LuaTable CreateIconTable(string imageName, string mapClassName, Position pos, string tooltip)
	{
		var posTable = new LuaTable();
		posTable.Insert("X", pos.X);
		posTable.Insert("Y", pos.Y);
		posTable.Insert("Z", pos.Z);

		var iconTable = new LuaTable();
		iconTable.Insert("Image", imageName);
		iconTable.Insert("Tooltip", tooltip);
		iconTable.Insert("Map", mapClassName);
		iconTable.Insert("WorldPos", posTable);

		return iconTable;
	}
}
