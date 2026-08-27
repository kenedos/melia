using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.World;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Scripting
{
	public static partial class Shortcuts
	{
		private const int StatPointStatueMonsterId = 40110;
		private const string StatPointStatueName = "Statue of Goddess Zemyna";
		private const string StatPointStatueVarPrefix = "Melia.StatPointStatue.";

		private const int WarpStatueMonsterId = 40120;
		private const string WarpStatueName = "Statue of Goddess Vakarine";
		private const string WarpStatueVarPrefix = "Melia.WarpStatue.";

		/// <summary>
		/// Warp statues every character can travel to from the start.
		/// </summary>
		private static readonly HashSet<string> DefaultWarpStatueKeys = new(StringComparer.OrdinalIgnoreCase)
		{
			"WARP_C_KLAIPE",
			"WARP_C_ORSHA",
			"WARP_C_FEDIMIAN",
		};

		private const int StatueGenTypeMin = 100_000;
		private const uint StatueGenTypeRange = 800_000;

		/// <summary>
		/// Adds a statue that grants a character one permanent bonus stat
		/// point the first time they worship it, at a position on the
		/// ground.
		/// </summary>
		/// <remarks>
		/// The key identifies the statue in the characters' variables and
		/// must be unique and stable, as changing it lets everyone use the
		/// statue again.
		/// </remarks>
		/// <param name="key"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="name"></param>
		/// <param name="monsterId"></param>
		public static Npc AddStatPointStatue(string key, string map, double x, double z, double direction = 0, string name = StatPointStatueName, int monsterId = StatPointStatueMonsterId)
		{
			var pos = GetGroundPosition(map, x, z);

			return AddStatPointStatue(GetStatueGenType(map, key), key, map, pos.X, pos.Y, pos.Z, direction, name, monsterId);
		}

		/// <summary>
		/// Adds a statue that grants a character one permanent bonus stat
		/// point the first time they worship it, with an explicit gen type
		/// and height.
		/// </summary>
		/// <param name="genType"></param>
		/// <param name="key"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="name"></param>
		/// <param name="monsterId"></param>
		public static Npc AddStatPointStatue(int genType, string key, string map, double x, double y, double z, double direction = 0, string name = StatPointStatueName, int monsterId = StatPointStatueMonsterId)
		{
			var npc = CreateStatue(genType, key, map, x, y, z, direction, name, monsterId);
			npc.SetClickTrigger(key, StatPointStatueDialog);

			return npc;
		}

		/// <summary>
		/// Adds a statue that unlocks a fast travel destination when a
		/// character walks up to it and offers travel when clicked, at a
		/// position on the ground.
		/// </summary>
		/// <remarks>
		/// The key is the warp's class name, which ties the statue to its
		/// entry in the warp database.
		/// </remarks>
		/// <param name="key"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="name"></param>
		/// <param name="monsterId"></param>
		public static Npc AddWarpStatue(string key, string map, double x, double z, double direction = 0, string name = WarpStatueName, int monsterId = WarpStatueMonsterId)
		{
			var pos = GetGroundPosition(map, x, z);

			return AddWarpStatue(GetStatueGenType(map, key), key, map, pos.X, pos.Y, pos.Z, direction, name, monsterId);
		}

		/// <summary>
		/// Adds a statue that unlocks a fast travel destination when a
		/// character walks up to it and offers travel when clicked, with an
		/// explicit gen type and height.
		/// </summary>
		/// <param name="genType"></param>
		/// <param name="key"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="name"></param>
		/// <param name="monsterId"></param>
		public static Npc AddWarpStatue(int genType, string key, string map, double x, double y, double z, double direction = 0, string name = WarpStatueName, int monsterId = WarpStatueMonsterId)
		{
			var npc = CreateStatue(genType, key, map, x, y, z, direction, name, monsterId);
			npc.SetClickTrigger(key, WarpStatueDialog);
			npc.SetTriggerArea(Spot(npc.Position.X, npc.Position.Z, WarpStatueUnlockRange));
			npc.SetEnterTrigger(key, WarpStatueEnter);

			// The travel destinations are looked up by this name, both by
			// the dialog and by the client's warp command.
			ZoneServer.Instance.World.NPCs.TryAdd($"{key}_{GetMapOrThrow(map).Data.ClassName}", npc);

			return npc;
		}

		private const double WarpStatueUnlockRange = 100;

		/// <summary>
		/// Creates a statue and adds it to its map.
		/// </summary>
		/// <param name="genType"></param>
		/// <param name="key"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="name"></param>
		/// <param name="monsterId"></param>
		/// <exception cref="ArgumentException"></exception>
		private static Npc CreateStatue(int genType, string key, string map, double x, double y, double z, double direction, string name, int monsterId)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("A statue needs a key.");

			var mapObj = GetMapOrThrow(map);
			var location = new Location(mapObj.Id, new Position((float)x, (float)y, (float)z));

			var npc = new Npc(monsterId, name, location, new Direction(direction), genType);
			npc.UniqueName = $"{map}:{key}";
			npc.Properties.SetFloat(PropertyName.Range, 100);

			mapObj.AddMonster(npc);

			return npc;
		}

		/// <summary>
		/// Returns the position on the given map's ground.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		private static Position GetGroundPosition(string map, double x, double z)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((float)x, 0, (float)z);
			if (mapObj.Ground.TryGetHeightAt(pos, out var height))
				pos.Y = height;

			return pos;
		}

		/// <summary>
		/// Returns a gen type for a statue that stays the same across
		/// restarts, so the characters' personal NPC state remains valid.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="key"></param>
		private static int GetStatueGenType(string map, string key)
		{
			unchecked
			{
				var hash = 2166136261u;

				foreach (var ch in $"{map}:{key}")
				{
					hash ^= (uint)ch;
					hash *= 16777619u;
				}

				return StatueGenTypeMin + (int)(hash % StatueGenTypeRange);
			}
		}

		/// <summary>
		/// Handles the worship of a stat point statue, granting the character
		/// a permanent bonus stat point once.
		/// </summary>
		/// <param name="dialog"></param>
		private static async Task StatPointStatueDialog(Dialog dialog)
		{
			var npc = dialog.Npc;
			var character = dialog.Player;

			if (IsStatueUsed(character, npc.DialogName, StatPointStatueVarPrefix))
				return;

			dialog.PlayAnimation("ON");
			dialog.PlayAnimation("HOLD");
			dialog.PlayAnimation(character, "WORSHIP");

			var result = await dialog.TimeAction(ScpArgMsg("Auto_KyeongBae_Jung"), "WORSHIP", TimeSpan.FromSeconds(2));
			if (result != TimeActionResult.Completed)
			{
				dialog.PlayAnimation(character, "STD");
				dialog.DetachEffect(npc, "F_light023_orange");
				dialog.DetachEffect(npc, "F_light024_orange");
				dialog.DetachEffect(npc, "statue_zemina_light1");
				return;
			}

			dialog.AttachEffect(character, "F_pc_statue_wing", 10, EffectLocation.Top);

			var effectSessionObject = character.SessionObjects.GetOrCreate("SSN_ATTACH_EFF");
			if (effectSessionObject == null)
				return;

			character.SetMapNPCState(npc, NpcState.Unknown_20);
			character.ModifyProperty(PropertyName.StatByBonus, 1);
			MarkStatueUsed(character, npc.DialogName, StatPointStatueVarPrefix, npc.DialogName + "_P");
			character.AddonMessage("NOTICE_Dm_Clear", ScpArgMsg("STATUE_STAT_01"), 3);
			dialog.DetachEffect(npc, "F_light024_orange");
			character.RemoveSessionObject(effectSessionObject.Id);

			character.ShowHelp("MINI_E_STATUE");
		}

		/// <summary>
		/// Unlocks the warp statue as a travel destination for the character
		/// that walked up to it.
		/// </summary>
		/// <param name="args"></param>
		private static async Task WarpStatueEnter(TriggerActorArgs args)
		{
			await Task.Yield();

			var npc = args.Npc;
			if (args.Initiator is not Character character || args.Initiator is DummyCharacter)
				return;

			if (!IsStatueUsed(character, npc.DialogName, WarpStatueVarPrefix))
			{
				MarkStatueUsed(character, npc.DialogName, WarpStatueVarPrefix, npc.DialogName);

				if (ZoneServer.Instance.Data.MapDb.TryFind(npc.Map.Id, out var mapData))
				{
					character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, $"{ScpArgMsg("Auto_KaemPeuKan_iDong_:_")}{mapData.Name}{ScpArgMsg("Auto__HwalSeongHwa")}", 5);
					character.PlaySound("quest_event_click");
				}
			}
			else if (DefaultWarpStatueKeys.Contains(npc.DialogName) && PropertyTable.Exists("SessionObject", npc.DialogName))
			{
				// The client tracks unlocks on its own and needs these set.
				character.SetProperty(character.SessionObjects.Main, npc.DialogName, 300);
			}

			character.ShowHelp("TUTO_CAMPWARP");
		}

		/// <summary>
		/// Handles the worship of a warp statue, letting the character travel
		/// to the destinations they unlocked.
		/// </summary>
		/// <param name="dialog"></param>
		private static async Task WarpStatueDialog(Dialog dialog)
		{
			var result = await dialog.TimeAction(ScpArgMsg("Auto_KyeongBae_Jung"), "WORSHIP", TimeSpan.FromSeconds(1));
			if (result != TimeActionResult.Completed)
				return;

			if (ZoneServer.Instance.Conf.World.FastTravelEnabled)
			{
				await dialog.ExecuteScript("SIMPLEMAP_OPEN_WARP_MODE()");
				return;
			}

			var character = dialog.Player;

			// Define warp destinations and their level ranges
			Dictionary<string, (string warpCode, int minLevel, int maxLevel)> warpOptions = new()
			{
				 // Cities
				{ "Klaipeda", ("WARP_C_KLAIPE_c_Klaipe", 0, 0) },
				{ "Fedimian", ("WARP_C_FEDIMIAN_c_fedimian", 0, 0) },
				{ "Orsha", ("WARP_C_ORSHA_c_orsha", 0, 0) },

				// Level 1-10
				{ "Woods of the Linked Bridges", ("WARP_F_SIAULIAI_15RE_f_siauliai_15_re", 7, 9) },
				// { "Ramstis Ridge", ("WARP_F_RAMSTIS_RIDGE", 1, 11) },
				{ "West Siauliai Woods", ("WARP_F_SIAULIAI_WEST_f_siauliai_west", 1, 3) },

				// Level 11-20
				{ "Miner's Village", ("WARP_F_SIAULIAI_OUT_f_siauliai_out", 11, 13) },
				// { "Syla Forest", ("WARP_F_SYLA_FOREST", 11, 13) },
				// { "Gytis Settlement", ("WARP_F_GYTIS_SETTLEMENT", 14, 20) },
				// { "Baron Allerno", ("WARP_F_BARON_ALLERNO", 16, 24) },

				// Level 21-30
				{ "Tenants' Farm", ("WARP_F_FARM_47_1_f_farm_47_1", 24, 30) },
				{ "Srautas Gorge", ("WARP_F_GELE_57_1_f_gele_57_1", 26, 30) },
				// { "Gele Plateau", ("WARP_F_GELE_PLATEAU", 29, 31) },
				// { "Koru Jungle", ("WARP_F_KORU_JUNGLE", 28, 31) },

				// Level 31-40
				// { "Aqueduct Bridge Area", ("WARP_F_AQUEDUCT_BRIDGE_AREA", 31, 36) },
				// { "Myrkiti Farm", ("WARP_F_MYRKITI_FARM", 31, 40) },
				{ "Nefritas Cliff", ("WARP_F_GELE_57_3_f_gele_57_3", 32, 34) },
				{ "Knidos Jungle", ("WARP_F_BRACKEN_63_2_f_bracken_63_2", 32, 34) },
				{ "Tenet Garden", ("WARP_F_GELE_57_4_f_gele_57_4", 35, 36) },
				{ "King's Plateau", ("WARP_F_ROKAS_30_f_rokas_30", 31, 36) },

				// Level 41-50
				// { "Overlong Bridge Valley", ("WARP_F_OVERLONG_BRIDGE_VALLEY", 42, 48) },
				{ "Grynas Trails", ("WARP_F_KATYN_45_1_f_katyn_45_1", 45, 48) },
				// { "Dadan Jungle", ("WARP_F_DADAN_JUNGLE", 46, 51) },
				// { "Grynas Training Camp", ("WARP_F_GRYNAS_TRAINING_CAMP", 49, 51) },
				{ "Vieta Gorge", ("WARP_F_HUEVILLAGE_58_2_f_huevillage_58_2", 49, 51) },

				// Level 51-60
				// { "Shaton Farm", ("WARP_F_SHATON_FARM", 52, 54) },
				{ "Grynas Hills", ("WARP_F_KATYN_45_3_f_katyn_45_3", 53, 56) },
				// { "Cobalt Forest", ("WARP_F_COBALT_FOREST", 53, 62) },
				// { "Laukyme Swamp", ("WARP_F_LAUKYME_SWAMP", 58, 62) },
				// { "Genar Field", ("WARP_F_GENAR_FIELD", 58, 60) },

				// Level 61-70
				{ "Dina Bee Farm", ("WARP_F_SIAULIAI_46_4_f_siauliai_46_4", 60, 70) },
				{ "Gateway of the Great King", ("WARP_F_ROKAS_24_f_rokas_24", 32, 62) },
				// { "Glade Hillroad", ("WARP_F_GLADE_HILLROAD", 64, 68) },
				// { "Sekta Forest", ("WARP_F_SEKTA_FOREST", 66, 67) },
				// { "Alemeth Forest", ("WARP_F_ALEMETH_FOREST", 68, 70) },

				// Level 71-80
				{ "Viltis Forest", ("WARP_D_THORN_39_1_d_thorn_39_1", 71, 78) },
				//{ "Ouaas Memorial", ("WARP_F_OUAAS_MEMORIAL", 71, 73) },
				//{ "Seir Rainforest", ("WARP_F_SEIR_RAINFOREST", 72, 80) },
				// { "Zeraha", ("WARP_F_ZERAHA", 72, 80) },

				// Mixed/Wide Level Range
				// { "Salvia Forest", ("WARP_F_SALVIA_FOREST", 26, 62) },
				{ "Rasvoy Lake", ("WARP_PILGRIMROAD_41_3_f_pilgrimroad_41_3", 42, 70) },
				{ "Septyni Glen", ("WARP_F_HUEVILLAGE_58_4_f_huevillage_58_4", 55, 65) },
				{ "Izoliacjia Plateau", ("WARP_WHITETREES_22_3_f_whitetrees_22_3", 42, 65) },
			};

			// Allow the player to select a level range
			var levelRangeResponse = await dialog.Select(L("Select map by level range:"),
				Option("Cities", "cities"),
				Option("Level 1-10", "range_1_10"),
				Option("Level 11-20", "range_11_20"),
				Option("Level 21-30", "range_21_30"),
				Option("Level 31-40", "range_31_40"),
				Option("Level 41-50", "range_41_50"),
				Option("Level 51-60", "range_51_60"),
				Option("Level 61-70", "range_61_70"),
				Option("Level 71-80", "range_71_80"),
				Option("Mixed/Wide Level Range", "mixed_range"));

			int selectedMinLevel = 0, selectedMaxLevel = 0;
			switch (levelRangeResponse)
			{
				case "range_1_10": selectedMinLevel = 1; selectedMaxLevel = 10; break;
				case "range_11_20": selectedMinLevel = 11; selectedMaxLevel = 20; break;
				case "range_21_30": selectedMinLevel = 21; selectedMaxLevel = 30; break;
				case "range_31_40": selectedMinLevel = 31; selectedMaxLevel = 40; break;
				case "range_41_50": selectedMinLevel = 41; selectedMaxLevel = 50; break;
				case "range_51_60": selectedMinLevel = 51; selectedMaxLevel = 60; break;
				case "range_61_70": selectedMinLevel = 61; selectedMaxLevel = 70; break;
				case "range_71_80": selectedMinLevel = 71; selectedMaxLevel = 80; break;
				case "mixed_range": selectedMinLevel = 1; selectedMaxLevel = 80; break; // Wide range
				case "cities": selectedMinLevel = 0; selectedMaxLevel = 0; break; // No monster levels
				default: return; // Cancel action
			}

			// Filter destinations based on the selected level range
			var filteredDestinations = warpOptions
				.Where(opt =>
				(ZoneServer.Instance.World.NPCs.TryGetValue(opt.Value.warpCode, out var destinationNpc)
				&& IsStatueUsed(character, destinationNpc.DialogName, WarpStatueVarPrefix))
				&& (opt.Value.minLevel <= selectedMaxLevel && opt.Value.maxLevel >= selectedMinLevel))
				.Take(12) // Limit to 12 options
				.ToDictionary(opt => opt.Key, opt => opt.Value.warpCode);

			if (filteredDestinations.Count == 0)
			{
				await dialog.Msg("You haven't unlocked a Vakarine statue in this range.");
				return;
			}

			// Present filtered destinations to the player
			var destinationOptions = filteredDestinations.Select(opt => Option(opt.Key, opt.Value)).ToArray();
			var destinationResponse = await dialog.Select(L("Choose your destination:"), destinationOptions);

			// Warp to the selected location
			if (ZoneServer.Instance.World.NPCs.TryGetValue($"{destinationResponse}", out var warpNpc))
			{
				var mapId = warpNpc.Map.Id;
				var newPosition = warpNpc.Position.GetRelative(warpNpc.Direction, 50);
				var newDirection = -warpNpc.Direction;
				character.SetDirection(newDirection);
				character.Warp(mapId, newPosition);
			}
		}

		/// <summary>
		/// Returns whether the character already used the statue with the
		/// given key.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="key"></param>
		/// <param name="varPrefix"></param>
		private static bool IsStatueUsed(Character character, string key, string varPrefix)
		{
			if (varPrefix == WarpStatueVarPrefix && DefaultWarpStatueKeys.Contains(key))
				return true;

			if (character.Variables.Perm.GetBool(varPrefix + key))
				return true;

			// Statues that exist in the client data used to track this via
			// a session object property, which still needs to be honored.
			return TryGetStatueProperty(character, key + "_P", out var propValue) && propValue != 0
				|| TryGetStatueProperty(character, key, out propValue) && propValue != 0;
		}

		/// <summary>
		/// Marks the statue with the given key as used by the character,
		/// updating the client's session object if it knows the property.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="key"></param>
		/// <param name="varPrefix"></param>
		/// <param name="propertyName"></param>
		private static void MarkStatueUsed(Character character, string key, string varPrefix, string propertyName)
		{
			character.Variables.Perm.SetBool(varPrefix + key, true);

			if (PropertyTable.Exists("SessionObject", propertyName))
				character.SetProperty(character.SessionObjects.Main, propertyName, 300);
		}

		/// <summary>
		/// Returns the value of a statue's session object property, if the
		/// client knows the property and the character has it set.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		private static bool TryGetStatueProperty(Character character, string propertyName, out float value)
		{
			value = 0;

			if (!PropertyTable.Exists("SessionObject", propertyName))
				return false;

			return character.SessionObjects.Main.TryGetProp(propertyName, out value);
		}
	}
}
