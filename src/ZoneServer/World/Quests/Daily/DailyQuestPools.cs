using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Quests.Daily
{
	public static class DailyQuestPools
	{
		private static readonly IReadOnlyDictionary<string, string> DailyMapNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			["f_tableland_74"] = "Steel Heights",
			["f_remains_39"] = "Escanciu Village",
			["f_tableland_28_2"] = "Stogas Plateau",
			["f_tableland_28_1"] = "Mesafasla",
			["f_tableland_72"] = "Sventimas Exile",
			["d_thorn_39_3"] = "Laukyme Swamp",
			["f_remains_38"] = "Goddess' Ancient Garden",
			["f_flash_60"] = "Roxona Market",
			["f_tableland_11_1"] = "Vedas Plateau",
			["f_tableland_71"] = "Grand Yard Mesa",
			["d_thorn_39_1"] = "Viltis Forest",
			["d_thorn_39_2"] = "Glade Hillroad",
			["f_remains_37"] = "Stele Road",
			["f_flash_59"] = "Verkti Square",
			["f_flash_61"] = "Ruklys Street",
			["f_tableland_70"] = "Ibre Plateau",
			["f_farm_49_1"] = "Greene Manor",
			["f_siauliai_46_4"] = "Dina Bee Farm",
			["f_rokas_26"] = "Overlong Bridge Valley",
			["f_rokas_24"] = "Gateway of the Great King",
			["f_orchard_32_4"] = "Seir Rainforest",
			["f_flash_63"] = "Downtown",
			["f_flash_64"] = "Inner Enceinte District",
			["f_farm_47_2"] = "Aqueduct Bridge Area",
			["f_huevillage_58_4"] = "Septyni Glen",
			["f_rokas_27"] = "Akmens Ridge",
			["f_rokas_30"] = "King's Plateau",
			["f_orchard_34_2"] = "Zeraha",
			["f_flash_29_1"] = "Coastal Fortress",
			["f_flash_58"] = "Dingofasil District",
			["f_siauliai_46_1"] = "Spring Light Woods",
			["f_farm_49_2"] = "Shaton Farm",
			["f_farm_47_3"] = "Myrkiti Farm",
			["f_farm_47_1"] = "Tenants' Farm",
			["f_huevillage_58_3"] = "Cobalt Forest",
			["f_gele_57_4"] = "Tenet Garden",
			["f_rokas_28"] = "Tiltas Valley",
			["f_orchard_34_1"] = "Alemeth Forest",
			["f_orchard_34_3"] = "Barha Forest",
			["d_thorn_19"] = "Gate Route",
			["d_thorn_21"] = "Kvailas Forest",
			["f_siauliai_47_4"] = "Baron Allerno",
			["f_huevillage_58_2"] = "Vieta Gorge",
			["f_gele_57_3"] = "Nefritas Cliff",
			["f_rokas_31"] = "Zachariel Crossroads",
			["f_bracken_63_3"] = "Dadan Jungle",
			["d_thorn_23"] = "Sunset Flag Forest",
			["d_thorn_22"] = "Dvasia Peak",
			["f_siauliai_50_1"] = "Gytis Settlement Area",
			["f_gele_57_2"] = "Gele Plateau",
			["f_pilgrimroad_49"] = "Genar Field",
			["f_rokas_25"] = "Ramstis Ridge",
			["f_bracken_63_2"] = "Knidos Jungle",
			["f_bracken_63_1"] = "Koru Jungle",
			["f_siauliai_out"] = "Miners' Village",
			["f_gele_57_1"] = "Srautas Gorge",
			["f_katyn_45_3"] = "Grynas Hills",
			["f_katyn_45_1"] = "Grynas Trails",
			["f_pilgrimroad_41_2"] = "Salvia Forest",
			["f_bracken_42_1"] = "Khonot Forest",
			["f_siauliai_11_re"] = "(Ex)Paupys Crossing",
			["f_siauliai_west"] = "West Siauliai Woods",
			["f_3cmlake_84"] = "Absenta Reservoir",
			["f_3cmlake_83"] = "Pelke Shrine Ruins",
			["f_katyn_45_2"] = "Grynas Training Camp",
			["f_pilgrimroad_41_4"] = "Sekta Forest",
			["f_pilgrimroad_41_3"] = "Rasvoy Lake",
			["f_whitetrees_56_1"] = "Mishekan Forest",
			["f_siauliai_15_re"] = "(Ex)Woods of the Linked Bridges",
			["f_katyn_13"] = "Poslinkis Forest",
			["f_katyn_12"] = "Letas Stream",
			["f_pilgrimroad_41_5"] = "Ouaas Memorial",
			["f_maple_24_3"] = "Northern Parias Forest",
			["f_whitetrees_22_3"] = "Izoliacjia Plateau",
			["f_katyn_7_2"] = "Owl Burial Ground",
			["f_katyn_13_3"] = "Arrow Path",
			["f_maple_24_1"] = "Central Parias Forest",
			["f_whitetrees_23_3"] = "Syla Forest",
			["f_whitetrees_21_2"] = "Nobreer Forest",
			["f_maple_24_2"] = "Southern Parias Forest",
			["f_maple_23_2"] = "Pystis Forest",
			["f_whitetrees_23_1"] = "Emmet Forest",
			["d_prison_78"] = "Kalejimas Visiting Room",
			["d_prison_79"] = "Storage",
			["d_prison_80"] = "Solitary Cells",
			["d_prison_81"] = "Workshop",
			["d_prison_82"] = "Investigation Room",
			["d_catacomb_80_1"] = "Rancid Labyrinth",
			["d_catacomb_80_2"] = "Balaam Camp Site",
			["d_catacomb_80_3"] = "Michmas Temple",
			["d_abbey_39_4"] = "Tyla Monastery",
			["d_underfortress_65"] = "Sentry Bailey",
			["d_underfortress_66"] = "Drill Ground of Confliction",
			["d_underfortress_67"] = "Resident Quarter",
			["d_underfortress_68"] = "Storage Quarter",
			["d_underfortress_30_1"] = "Ruklys Hall of Fame",
			["d_underfortress_30_2"] = "Extension",
			["d_underfortress_30_3"] = "Evacuation Residential District",
			["d_underfortress_69"] = "Fortress Battlegrounds",
			["d_velniasprison_51_1"] = "Demon Prison District 1",
			["d_velniasprison_51_2"] = "Demon Prison District 2",
			["d_velniasprison_51_3"] = "Demon Prison District 4",
			["d_velniasprison_51_4"] = "Demon Prison District 3",
			["d_velniasprison_51_5"] = "Demon Prison District 5",
			["d_firetower_41"] = "Mage Tower 1F",
			["d_firetower_42"] = "Mage Tower 2F",
			["d_firetower_43"] = "Mage Tower 3F",
			["d_firetower_44"] = "Mage Tower 4F",
			["d_firetower_45"] = "Mage Tower 5F",
			["d_chapel_57_5"] = "Tenet Church B1",
			["d_chapel_57_6"] = "Tenet Church 1F",
			["d_chapel_57_7"] = "Tenet Church 2F",
			["id_catacomb_01"] = "Guards Graveyard",
			["d_zachariel_32"] = "Royal Mausoleum 1F",
			["d_zachariel_33"] = "Royal Mausoleum 2F",
			["d_zachariel_34"] = "Royal Mausoleum 3F",
			["d_zachariel_35"] = "Royal Mausoleum 4F",
			["d_zachariel_36"] = "Royal Mausoleum 5F",
			["d_abbey_64_1"] = "(Closed) Novaha Assembly Hall",
			["d_abbey_64_2"] = "(Closed) Novaha Annex",
			["d_abbey_64_3"] = "(Closed) Novaha Institute",
			["d_cmine_01"] = "Crystal Mine 1F",
			["d_cmine_02"] = "Crystal Mine 2F",
			["d_cmine_6"] = "Crystal Mine 3F",
			["d_cmine_8"] = "Crystal Mine Lot 2 - 1F",
			["d_cmine_9"] = "Crystal Mine Lot 2 - 2F",
			["d_limestonecave_52_1"] = "Tevhrin Stalactite Cave Section 1",
			["d_limestonecave_52_2"] = "Tevhrin Stalactite Cave Section 2",
			["d_limestonecave_52_3"] = "Tevhrin Stalactite Cave Section 3",
			["d_limestonecave_52_4"] = "Tevhrin Stalactite Cave Section 4",
			["d_limestonecave_52_5"] = "Tevhrin Stalactite Cave Section 5",
			["d_prison_62_1"] = "(Closed) Ashaq Underground Prison 1F",
			["d_prison_62_2"] = "(Closed) Ashaq Underground Prison 2F",
			["d_prison_62_3"] = "(Closed) Ashaq Underground Prison 3F",
			["id_catacomb_33_1"] = "(Closed) Sienakal Graveyard",
			["id_catacomb_33_2"] = "(Closed) Carlyle's Mausoleum",
			["d_abbey_41_6"] = "Maven Abbey",
			["d_abbey_22_4"] = "Narvas Temple",
			["d_abbey_22_5"] = "Narvas Temple Annex",
			["d_fantasylibrary_48_1"] = "Sausis Room 9",
			["d_fantasylibrary_48_2"] = "Sausis Room 10",
			["d_fantasylibrary_48_3"] = "Valandis Room 2",
			["d_fantasylibrary_48_4"] = "Valandis Room 3",
			["d_fantasylibrary_48_5"] = "Valandis Room 91",
			["d_limestonecave_73_1"] = "(Closed) Tavorh Cave",
			["d_prison_75_1"] = "Narcon Prison",
			["d_startower_76_1"] = "Natarh Watchtower",
			["d_startower_76_2"] = "Nazarene Tower",
			["d_velniasprison_77_1"] = "Tatenye Prison",
			["d_cathedral_78_1"] = "Neighport Church East Building",
			["d_zachariel_79_1"] = "(Closed) Sjarejo Chamber",
			["d_zachariel_79_2"] = "(Closed) Netanmalek Mausoleum"
		};

		public static bool IsHuntingTaskMap(string className)
		{
			if (string.IsNullOrWhiteSpace(className))
				return false;

			return DailyMapNames.ContainsKey(className.Trim());
		}

		public static bool TryGetHuntingTaskMapName(string className, out string displayName)
		{
			if (string.IsNullOrWhiteSpace(className))
			{
				displayName = null;
				return false;
			}

			return DailyMapNames.TryGetValue(className.Trim(), out displayName);
		}

		private static readonly object SyncRoot = new();

		private static MapTarget[] mapTargets;

		private static RaceType[] raceTargets;

		private static AttributeType[] attributeTargets;

		private static SizeType[] sizeTargets;

		public static IReadOnlyList<MapTarget> MapTargets
		{
			get
			{
				EnsureBuilt();

				return mapTargets;
			}
		}

		public static IReadOnlyList<RaceType> RaceTargets
		{
			get
			{
				EnsureBuilt();

				return raceTargets;
			}
		}

		public static IReadOnlyList<AttributeType> AttributeTargets
		{
			get
			{
				EnsureBuilt();

				return attributeTargets;
			}
		}

		public static IReadOnlyList<SizeType> SizeTargets
		{
			get
			{
				EnsureBuilt();

				return sizeTargets;
			}
		}

		public static void Rebuild()
		{
			lock (SyncRoot)
			{
				BuildPools();
			}
		}

		private static void EnsureBuilt()
		{
			if (mapTargets != null &&
			raceTargets != null &&
			attributeTargets != null &&
			sizeTargets != null)
			{
				return;
			}

			lock (SyncRoot)
			{
				if (mapTargets != null &&
				raceTargets != null &&
				attributeTargets != null &&
				sizeTargets != null)
				{
					return;
				}

				BuildPools();
			}
		}

		private static void BuildPools()
		{
			var data = ZoneServer.Instance.Data;

			var allMaps = data.MapDb.Entries.Values
				.Where(IsValidMap)
				.OrderBy(map => map.Level)
				.ThenBy(map => GetMapName(map))
				.ToArray();

			mapTargets = allMaps.Select(map => new MapTarget(map.ClassName, GetMapName(map))).ToArray();

			var spawnedMonsterIds = allMaps.SelectMany(map => map.SpawnedMonsterIds).Distinct().ToArray();
			var monsters = new List<MonsterData>();

			foreach (var monsterId in spawnedMonsterIds)
			{
				var monster = data.MonsterDb.Find(monsterId);

				if (monster == null)
					continue;

				monsters.Add(monster);
			}

			raceTargets = monsters.Select(monster => monster.Race).Where(IsValidRace).Distinct().OrderBy(race => (int)race).ToArray();
			attributeTargets = monsters.Select(monster => monster.Attribute).Where(IsValidAttribute).Distinct().OrderBy(attribute => (int)attribute).ToArray();
			sizeTargets = monsters.Select(monster => monster.Size).Where(IsValidSize).Distinct().OrderBy(size => (int)size).ToArray();

			ApplyFallbacks();
		}

		private static bool IsValidMap(MapData map)
		{
			if (map == null || string.IsNullOrWhiteSpace(map.ClassName))
				return false;

			return DailyMapNames.ContainsKey(map.ClassName.Trim());
		}

		private static bool IsValidRace(RaceType race)
		{
			return race switch
			{
				RaceType.None => false,
				RaceType.Item => false,
				RaceType.Human => false,
				_ => true,
			};
		}

		private static bool IsValidAttribute(
		AttributeType attribute)
		{
			return attribute switch
			{
				AttributeType.None => false,
				AttributeType.Melee => false,
				// Valor customizado usado principalmente em outros
				// sistemas, não como atributo normal de monstros.
				AttributeType.Magic => false,

				_ => true,
			};
		}

		private static bool IsValidSize(
		SizeType size)
		{
			return size switch
			{
				SizeType.S => true,
				SizeType.M => true,
				SizeType.L => true,
				_ => false,
			};
		}

		private static string GetMapName(MapData map)
		{
			if (map != null && !string.IsNullOrWhiteSpace(map.ClassName) && DailyMapNames.TryGetValue(map.ClassName.Trim(), out var displayName))
				return displayName;

			return DailyQuestDisplayNames.GetMapDisplayName(map?.ClassName);
		}

		private static void ApplyFallbacks()
		{
			if (mapTargets == null || mapTargets.Length == 0)
			{
				throw new InvalidOperationException(
					"No valid playable maps were found in system/db/maps.txt " +
					"for the Daily Quest map pool.");
			}

			if (raceTargets == null || raceTargets.Length == 0)
			{
				raceTargets =
					new[]
					{
						RaceType.Klaida,
						RaceType.Paramune,
						RaceType.Forester,
						RaceType.Velnias,
						RaceType.Widling,
					};
			}

			if (attributeTargets == null || attributeTargets.Length == 0)
			{
				attributeTargets =
					new[]
					{
						AttributeType.Fire,
						AttributeType.Ice,
						AttributeType.Poison,
						AttributeType.Earth,
						AttributeType.Soul,
						AttributeType.Lightning,
						AttributeType.Holy,
						AttributeType.Dark,
					};
			}

			if (sizeTargets == null || sizeTargets.Length == 0)
			{
				sizeTargets =
					new[]
					{
						SizeType.S,
						SizeType.M,
						SizeType.L,
					};
			}
		}

		public readonly struct MapTarget
		{
			public string ClassName { get; }

			public string DisplayName { get; }

			public MapTarget(
			string className,
			string displayName)
			{
				this.ClassName = className;
				this.DisplayName = displayName;
			}
		}
	}
}
