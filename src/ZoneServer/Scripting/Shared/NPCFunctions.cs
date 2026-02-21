using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Util;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Yggdrasil.Util.Commands;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.Scripting.Shared
{
	public static partial class NPCFunctions
	{
		public static async Task COMMON_QUEST_HANDLER(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task NPC_CASTLE_WARP(Dialog dialog)
		{
			dialog.Player.Warp("id_castle2", 600, 106, 180);
		}

		[DialogFunction]
		public static async Task NPC_3CMLAKE_WARP(Dialog dialog)
		{
			dialog.Player.Warp("id_3cmlake_26_1", 920, -80, 1270);
		}

		[DialogFunction]
		public static async Task HOMUNCULUS_UI(Dialog dialog)
		{
			var character = dialog.Player;
			var trigger = dialog.Trigger;

			if (trigger is not ISubActor actor)
				return;
			if (actor.OwnerHandle != character.Handle)
				return;

			dialog.OpenAddon("OPEN_HOMUNCLUS_INFO", character.Name, character.Handle);
		}

		[DialogFunction]
		public static async Task LITTLE_GLACIER(Dialog dialog)
		{
			switch (await dialog.Select(@"EVENT_2312_8TH_ANI_LITTLE", @"!@#$EVENT_2312_8TH_ANNIVERSARY_01#@!",
				@"!@#$EVENT_2312_8TH_ANNIVERSARY_02#@!", @"!@#$EVENT_2312_8TH_ANNIVERSARY#@!",
				@"!@#$EVENT_2312_8TH_ANNIVERSARY_04#@!", @"!@#$EVENT_2312_8TH_ANNIVERSARY_05#@!",
				@"!@#$Auto_JongLyo#@!"))
			{
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				case 4:
					break;
				case 5:
					break;
				case 6:
					break;
				case 7:
					break;
			}

		}

		//AddNpc(0, 150217, "Crusader Sentry", "0", -729.7, 285.29, -733.48, 353, "ETC_20200710_049102");
		[DialogFunction]
		public static async Task ETC_20200710_049102(Dialog dialog)
		{
			switch (await dialog.Select(@"W_SNIGO_START_NPC_MSG", @"!@#$Start#@!", @"!@#$Cancel#@!"))
			{
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
			}

		}

		[DialogFunction]
		public static async Task GUILD_HOUSE_OBJECT(Dialog dialog)
		{
			// Removed: Guild type deleted during Laima merge
			// IGuildMember.Guild property no longer available
			await Task.CompletedTask;
		}

		/// <summary>
		/// Guild Tower
		/// </summary>
		/// <param name="dialog"></param>
		/// <returns></returns>
		[DialogFunction]
		public static async Task GUILD_TOWER(Dialog dialog)
		{
			// Removed: Guild type deleted during Laima merge
			// IGuildMember.Guild property no longer available
			await Task.CompletedTask;
		}

		/// <summary>
		/// Treasure Box
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="className"></param>
		/// <param name="amount"></param>
		/// <param name="itemRequiredToUnlock"></param>
		/// <returns></returns>
		public static async Task TREASUREBOX_LV(Dialog dialog, string className, int amount, string itemRequiredToUnlock = "")
		{
			if (!string.IsNullOrEmpty(itemRequiredToUnlock) && dialog.Player.RemoveItem(itemRequiredToUnlock) > 0)
				return;
			await OpenChest(dialog.Player, dialog.Npc);
			dialog.Player.AddItem(className, amount);
			await Task.Yield();
		}

		/// <summary>
		/// Warp Statues
		/// </summary>
		/// <param name="dialog"></param>
		/// <returns></returns>
		public static async Task STATUE_WARP(Dialog dialog)
		{
			var result = await dialog.TimeAction(ScpArgMsg("Auto_KyeongBae_Jung"), "WORSHIP", TimeSpan.FromSeconds(1));
			if (result == TimeActionResult.Completed)
			{
				if (ZoneServer.Instance.Conf.World.FastTravelEnabled)
				{
					await dialog.ExecuteScript("SIMPLEMAP_OPEN_WARP_MODE()");
				}
				else
				{
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
						(ZoneServer.Instance.World.NPCs.TryGetValue(opt.Value.warpCode, out var warpNpc)
						&& character.SessionObjects.Main.Has(warpNpc.DialogName))
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
			}
		}

		/// <summary>
		/// Bonus Stat Statues
		/// </summary>
		/// <param name="dialog"></param>
		/// <returns></returns>
		public static async Task ZEMINA_STATUE(Dialog dialog)
		{
			var npc = dialog.Npc;
			var character = dialog.Player;
			if (npc.GenType == 0)
				return;

			var npcState = character.GetMapNPCState(npc);
			var sessionObject = character.SessionObjects.Main;
			var sessionObjectPropertyName = npc.DialogName + "_P";
			sessionObject.TryGetProp(sessionObjectPropertyName, out float propValue);
			if (propValue == 0)
			{
				dialog.PlayAnimation("ON");
				dialog.PlayAnimation("HOLD");
				dialog.PlayAnimation(character, "WORSHIP");
				var result = await dialog.TimeAction(ScpArgMsg("Auto_KyeongBae_Jung"), "WORSHIP", TimeSpan.FromSeconds(2));
				if (result == TimeActionResult.Completed)
				{
					dialog.AttachEffect(character, "F_pc_statue_wing", 10, EffectLocation.Top);
					var result1 = character.SessionObjects.GetOrCreate("SSN_ATTACH_EFF");
					if (result1 != null)
					{
						var beforeValue = character.Properties[PropertyName.StatByBonus];
						character.SetMapNPCState(npc, NpcState.Unknown_20);
						character.ModifyProperty(PropertyName.StatByBonus, 1);
						character.SetProperty(sessionObject, sessionObjectPropertyName, 300);
						var afterValue = character.Properties[PropertyName.StatByBonus];
						character.AddonMessage("NOTICE_Dm_Clear", "STATUE_STAT_01", 3);
						dialog.DetachEffect(npc, "F_light024_orange");
						character.RemoveSessionObject(result1.Id);
					}
				}
				else
				{
					dialog.PlayAnimation(character, "STD");
					dialog.DetachEffect(npc, "F_light023_orange");
					dialog.DetachEffect(npc, "F_light024_orange");
					dialog.DetachEffect(npc, "statue_zemina_light1");
				}
			}
		}

		/// <summary>
		/// Handles the dialog for the temporary Goddess Statues created by players.
		/// </summary>
		/// <param name="dialog">The dialog context.</param>
		[DialogFunction]
		public static async Task GODDESS_STATUE_EV(Dialog dialog)
		{
			var npc = dialog.Npc;
			var character = dialog.Player;

			var result = await dialog.TimeAction(ScpArgMsg("Auto_KyeongBae_Jung"), "WORSHIP", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				if (character.IsBuffActive(BuffId.Event_Goddess))
					character.RemoveBuff(BuffId.Event_Goddess);

				dialog.AttachEffect(npc, "F_buff_basic025_white_line", 1.0f, EffectLocation.Top);

				character.StartBuff(BuffId.Event_Goddess, 1, 0, TimeSpan.FromMilliseconds(1800000), npc);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, ScpArgMsg("Goddess_Statue1"), 5);
			}
		}

		[DialogFunction]
		public static async Task TEST_TP_SHOP(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task TEST_RANK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task SKILLPOINTUP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task IMC_GUARD(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_GRITA(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_GRITA_BASIC01");
			await dialog.Msg("FEDIMIAN_GRITA_BASIC02");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task JOB_SQUIRE3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_SQUIRE3_1_NPC_BASIC01");
			await dialog.Msg("JOB_SQUIRE3_1_NPC_BASIC03");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task WARP_C_FEDIMIAN(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task FEDIMIAN_DETECTIVE_GUARD(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_DETECTIVE_GUARD_basic01");
			await dialog.Msg("FEDIMIAN_DETECTIVE_GUARD_basic02");
			await dialog.Msg("FEDIMIAN_DETECTIVE_GUARD_basic03");
			await dialog.Msg("FEDIMIAN_DETECTIVE_GUARD_basic04");
			await dialog.Msg("FEDIMIAN_DETECTIVE_GUARD_basic05");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task CRIMINAL(Dialog dialog)
		{
			await dialog.Msg("CRIMINAL_basic01");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task REMAIN39_SQ03_GIRL(Dialog dialog)
		{
			await dialog.Msg("REMAIN39_SQ03_GIRL_basic01");
			await dialog.Msg("REMAIN39_SQ03_GIRL_basic02");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task MASTER_ROGUE(Dialog dialog)
		{
			await dialog.Msg("MASTER_ROGUE_BASIC01");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FED_TOOL(Dialog dialog)
		{
			await dialog.Msg("FED_TOOL_BASIC01");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FED_EQUIP(Dialog dialog)
		{
			await dialog.Msg("FED_EQUIP_BASIC01");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_SIGN1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_OLDMAN1(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_OLDMAN1_BASIC01");
			await dialog.Msg("FEDIMIAN_OLDMAN1_3");

			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_SIGN2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_SIGN3(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_SIGN4(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task MASTER_HACKAPELL(Dialog dialog)
		{
			await dialog.Msg("MASTER_HACKAPELL_basic");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task JOB_DRUID3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_DRUID3_1_NPC_basic01");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_NPC_11(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_NPC_11_basic01");
			await dialog.Msg("FEDIMIAN_NPC_11_basic02");
			await dialog.Msg("FEDIMIAN_NPC_11_basic03");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_NPC_12(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_NPC_12_basic01");
			await dialog.Msg("FEDIMIAN_NPC_12_basic02");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task PILGRIM_PRE_BOARD(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task MASTER_DOPPELSOELDNER(Dialog dialog)
		{
			await dialog.Msg("MASTER_DOPPELSOELDNER_normal");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}



		[DialogFunction]
		public static async Task MARKET_FEDIMIAN(Dialog dialog)
		{
			switch (await dialog.Select("FEDIMIAN_MARKET_SEL", "!@#$KLAPEDA_MARKET_OPEN#@!", "!@#$Auto_DaeHwa_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenCustomDialog(CustomDialog.MARKET);
					break;
			}
		}

		[DialogFunction]
		public static async Task WAREHOUSE_FEDIMIAN(Dialog dialog)
		{
			switch (await dialog.Select("WAREHOUSE_FEDIMIAN_DLG", "!@#$WareHouse#@!", "!@#$AccountWareHouse#@!", "!@#$Close#@!"))
			{
				case 1:
					await dialog.OpenCustomDialog(CustomDialog.WAREHOUSE);
					break;
				case 2:
					await dialog.OpenCustomDialog(CustomDialog.ACCOUNT_WAREHOUSE);
					break;
			}
		}

		[DialogFunction]
		public static async Task FED_ACCESSORY(Dialog dialog)
		{
			await dialog.Msg("FED_ACCESSORY_Select_1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task SHINOBI_POTTER(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task TP_NPC(Dialog dialog)
		{
			await dialog.Msg("TP_NPC_basic01");
			await dialog.Msg("TP_NPC_basic02");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
			dialog.OpenAddon("TP_SHOP_UI_OPEN");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_NPC_TEMPLE04(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_NPC_TEMPLE04_basic01");
			await dialog.Msg("FEDIMIAN_NPC_TEMPLE04_basic02");
			await dialog.Msg("HT_FEDIMIAN_NPC_TEMPLE04_BASIC03");
			await dialog.Msg("HT_FEDIMIAN_NPC_TEMPLE04_BASIC04");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_APPRAISER(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_APPRAISER_DLG1");
		}

		[DialogFunction]
		public static async Task AUTO_CAHLLENGE_ENTER(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task FEDIMIAN_APPRAISER_NPC(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_APPRAISER_NPC_DLG1");
		}

		[DialogFunction]
		public static async Task MATADOR_MASTER(Dialog dialog)
		{
			await dialog.Msg("MATADOR_MASTER_basic1");
			await dialog.Msg("MATADOR_MASTER_basic2");
		}

		[DialogFunction]
		public static async Task EXPLORER_ANASTAZIJA(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task NPC_JUNK_SHOP_FEDIMIAN_ORSHA(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_OFFICIAL_BOARD(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EXORCIST_MASTER(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task CHAR420_STEP321_NPC(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task CHAR420_STEP323_NPC1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_TERIAVELIS(Dialog dialog)
		{
			await dialog.Msg("NPC_TERIAVELIS_DLG1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task OUTLAW_MASTER_W_NPC(Dialog dialog)
		{
			await dialog.Msg("OUTLAW_MASTER_W_NPC_basic_1");
			await dialog.Msg("OUTLAW_MASTER_W_NPC_basic_2");
			await dialog.Msg("OUTLAW_MASTER_W_NPC_basic_3");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task OUTLAW_MASTER_M_NPC(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task PAYAUTA_EP11_NPC_CITY_1(Dialog dialog)
		{
			await dialog.Msg("PAYAUTA_EP11_NPC_CITY_1_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task INSTANCE_DUNGEON(Dialog dialog)
		{
			var player = dialog.Player;

			switch (await dialog.Select("ID_ENDTER_DIALOG", "!@#$IndunSelectDialog$*$ID_NAME$*$@dicID_^*$ETC_20200129_044061$*^$*$ID_LV$*$400$*$ID_LIMIT_LV$*${img infinity_text 20 10}#@!", "!@#$IndunSelectDialog$*$ID_NAME$*$@dicID_^*$ETC_20151224_017833$*^$*$ID_LV$*$50$*$ID_LIMIT_LV$*$299#@!", "!@#$IndunSelectDialog$*$ID_NAME$*$@dicID_^*$ETC_20180827_035054$*^$*$ID_LV$*$300$*$ID_LIMIT_LV$*$399#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 2:
					Send.ZC_NORMAL.InstanceDungeonMatchMaking(player, 16, 0, 1, 1, 1);
					break;
			}
		}

		[DialogFunction]
		public static async Task NPC_PERSONAL_HOUSING_MANAGER(Dialog dialog)
		{
			var player = dialog.Player;
			var account = player.Connection.Account;

			switch (await dialog.Select("NPC_PERSONAL_HOUSING_MANAGER_DLG_2", "!@#$PH_SEL_DLG_6#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					switch (await dialog.Select(
						"NPC_PERSONAL_HOUSING_POINT_INFOMATION\\!@#$PERSONAL_HOUSING_POINT_CHECK_MSG_1$*$POINT$*$1000$*$REMAIN_POINT$*$0#@!",
						"!@#$PH_POINT_SHOP_DLG_1#@!", "!@#$Auto_JongLyo#@!"))
					{
						case 1:
							switch (await dialog.Select(
								"NPC_PERSONAL_HOUSING_POINT_INFOMATION\\!@#$PH_POINT_SHOP_DLG_SEL_1$*$TEAM_NAME$*$Shayaan$*$LV$*$1$*$PLACE_LV$*$0#@!"
								.Replace("Shayaan", account.TeamName),
								"!@#$PH_POINT_SHOP_POINT_TRADE_1#@!", "!@#$Auto_JongLyo#@!"))
							{
								case 1:
									switch (await dialog.Select(
										"NPC_PERSONAL_HOUSING_POINT_INFOMATION\\!@#$PH_POINT_SHOP_DLG_SEL_1$*$TEAM_NAME$*$Shayaan$*$LV$*$1$*$PLACE_LV$*$0#@!"
										.Replace("Shayaan", account.TeamName),
										"!@#$PH_POINT_SHOP_POINT_TRADE_One#@!",
										"!@#$PH_POINT_SHOP_POINT_TRADE_ALL$*$COUNT$*$20#@!",
										"!@#$Auto_JongLyo#@!"))
									{
										// TODO: Calculate how many points are needed for each? How ther personal housing point system works?
										case 1:
										{
											account.Properties.SetFloat(PropertyName.PersonalHousing_Point1, 0);
											var item = ZoneServer.Instance.Data.ItemDb.FindByClass("PersonalHousing_PointGacha_1");
											player.AddItem(item.ClassName);
											dialog.OpenAddon(AddonMessage.EVENT_REWARD_NOTIFY_ITEM_GET,
												//string.Format("!@#$PH_POINT_SHOP_POINT_TRADE_SUCCESS_1#@!;@dicID_^*{0}*^", item.LocalKey));
												string.Format("!@#$PH_POINT_SHOP_POINT_TRADE_SUCCESS_1#@!;{0}", item.Name));
										}
										break;
										case 2:
										{
											account.Properties.SetFloat(PropertyName.PersonalHousing_Point1, 0);
											var item = ZoneServer.Instance.Data.ItemDb.FindByClass("PersonalHousing_PointGacha_1");
											player.AddItem(item.ClassName);
											dialog.OpenAddon(AddonMessage.EVENT_REWARD_NOTIFY_ITEM_GET,
												//string.Format("!@#$PH_POINT_SHOP_POINT_TRADE_SUCCESS_1#@!;@dicID_^*{0}*^", item.LocalKey));
												string.Format("!@#$PH_POINT_SHOP_POINT_TRADE_SUCCESS_1#@!;{0}", item.Name));
										}
										break;
									}
									break;
							}
							break;
					}
					break;
			}
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task EVENT_GODDESS_ROULETTE_NPC(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_POPO_ROULETTE_NPC(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task FEDIMIAN_NAUJOVES(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task SHOP_NPC_RAGANA_CITY_1(Dialog dialog)
		{
			await dialog.Msg(RandomElement("SHOP_NPC_RAGANA_CITY_1_basic1",
				"SHOP_NPC_RAGANA_CITY_1_basic2",
				"SHOP_NPC_RAGANA_CITY_1_basic3"));
			await dialog.ExecuteScript(ClientScripts.REQ_SILVER_GACHA_SHOP_OPEN);
		}

		[DialogFunction]
		public static async Task KLAPEDA_FISHING_BOARD(Dialog dialog)
		{
			await dialog.ExecuteScript(ClientScripts.FISHING_RANK_OPEN);
		}

		[DialogFunction]
		public static async Task FISHING_SUB_NPC(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task REPUTATION_QUEST_BOARD_01(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task ALCHEIST_EXPERT_NPC_FEDIMIAN(Dialog dialog)
		{
			await dialog.Msg("ALCHEIST_EXPERT_NPC_FEDIMIAN_basic");
		}

		[DialogFunction]
		public static async Task UNKNOWN_SANTUARY_GATE_GUILD(Dialog dialog)
		{
			// TODO: Check if requirements are met, currently my character only sees this option.
			switch (await dialog.Select("UNKNOWN_SANTUARY_GATE_DLG1", "!@#$UNKNOWN_SANTUARY_GATE_CLMSG_INFO#@!", "!@#$UNKNOWN_SANTUARY_GATE_CLMSG_PAY_CANT_2#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.Msg("UNKNOWN_SANTUARY_GATE_INFO_1");
					break;
				case 2:
					await dialog.Msg("UNKNOWN_SANTUARY_GATE_INFO_CANT_1");
					break;
			}
		}

		[DialogFunction]
		public static async Task BOUNTY_HUNT_START(Dialog dialog)
		{
			dialog.ShowHelp("TUTO_BOUNTYHUNT_1");
			dialog.OpenAddon(AddonMessage.BOUNTYHUNT_OPEN);
		}

		[DialogFunction]
		public static async Task EP13CARE_TRADE(Dialog dialog)
		{
			switch (await dialog.Select("EP13CARE_TRADE_DLG1", "!@#$EP13CARE_TRADE1#@!", "!@#$EP13CARE_TRADE2#@!", "!@#$EP13CARE_TRADE3#@!", "!@#$EP13CARE_TRADE4#@!", "!@#$EP13CARE_TRADE5#@!", "!@#$EP13CARE_TRADE6#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "GoddessCare_Moru");
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "GoddessCare_ExtractKit");
					break;
				case 3:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "GoddessCare_EnchantJewel");
					break;
				case 4:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "GoddessCare_UniqueMaterial");
					break;
				case 5:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "GoddessCare_HiddenAbility");
					break;
				case 6:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "GoddessCare_Vibora");
					break;
			}
		}

		[DialogFunction]
		public static async Task BLESSED_CUBE(Dialog dialog)
		{
			await dialog.ExecuteScript(ClientScripts.BLESSED_CUBE_OPEN);
		}

		[DialogFunction]
		public static async Task DUCTILITY_NPC(Dialog dialog)
		{
			switch (await dialog.Select("COMMON_SKILL_ENCHANT_DLG1", "!@#$COMMON_SKILL_ENCHANT_MSG1#@!", "!@#$COMMON_ACC_UPGRADE_MSG#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.ExecuteScript(ClientScripts.REQ_COMMON_SKILL_ENCHANT_UI_OPEN);
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.REQ_COMMON_ACC_UPGRADE_UI_OPEN);
					break;
			}
		}

		[DialogFunction]
		public static async Task EVENT_TOS_WHOLE_NPC(Dialog dialog)
		{
			switch (await dialog.Select("EVENT_TOS_WHOLE_DLG1", "!@#$EVENT_COMMON_MSG1#@!", "!@#$EVENT_TOS_WHOLE_SHOP#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.Msg("EVENT_TOS_WHOLE_DLG2");
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "EVENT_TOS_WHOLE_SHOP");
					break;
			}
		}

		[DialogFunction]
		public static async Task TREEDAY_NPC(Dialog dialog)
		{
			switch (await dialog.Select("EVENT_TREEDAY_SHOP_DLG1", "!@#$EVENT_2304_ARBOR_DAY_SHOP#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "EVENT_2304_ARBOR_DAY_SHOP");
					break;
			}
		}

		[DialogFunction]
		public static async Task PREVIOUS_COIN_SHOP(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task BLACK_MARKET(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EVENT_2006_SUMMER_NPC(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_2006_SUMMER_CAT(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_2101_SUPPLY_NPC5(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_2109_MOON_RABBIT_NPC(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_NEWYEAR_ASSIST(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task CITY_EVENT_2110_NPC(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_2110_POT_NPC(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task CITY_EVENT_2110_NPC2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_2212_LAST_CHRISTMAS_NPC(Dialog dialog)
		{
			switch (await dialog.Select("EVENT_2212_LAST_CHRISTMAS_DLG1", "!@#$EVENT_COMMON_MSG1#@!", "!@#$EVENT_COMMON_MSG2#@!", "!@#$EVENT_COMMON_MSG3#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.Msg("EVENT_2212_LAST_CHRISTMAS_DLG2");
					break;
				case 2:
					await dialog.Msg("EVENT_2212_LAST_CHRISTMAS_DLG3");
					break;
				case 3:
					await dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "EVENT_2212_LAST_CHRISTMAS_SHOP");
					break;
			}
		}

		[DialogFunction]
		public static async Task EVENT_2207_UNITY(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EVENT_2301_HAPPY_NEW_YEAR_NPC(Dialog dialog)
		{
			switch (await dialog.Select("EVENT_2301_HAPPY_NEW_YEAR_DLG1", "!@#$EVENT_COMMON_MSG1#@!", "!@#$EVENT_COMMON_MSG2#@!", "!@#$EVENT_COMMON_MSG3#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.Msg("EVENT_2301_HAPPY_NEW_YEAR_DLG2");
					await Task.Yield();
					break;
				case 2:
					await dialog.Msg("EVENT_2301_HAPPY_NEW_YEAR_DLG3");
					await Task.Yield();
					break;
				case 3:
					dialog.ExecuteScript(ClientScripts.REQ_EVENT_SHOP_OPEN_COMMON, "EVENT_2301_HAPPY_NEW_YEAR_SHOP");
					break;
			}
		}

		[DialogFunction]
		public static async Task STEAM_TREASURE_EVENT(Dialog dialog)
		{
			await dialog.Msg("GLOBAL_EVENT_BOARD_NO_EVENT");
		}

		[DialogFunction]
		public static async Task EMILIA(Dialog dialog)
		{
			switch (await dialog.Select("Emilia_Select_1", "@dicID_^*$ETC_20150317_007303$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenShop("Klapeda_Misc");
					break;
			}
		}

		[DialogFunction]
		public static async Task AKALABETH(Dialog dialog)
		{
			switch (await dialog.Select("KLAPEDA_Akalabeth_basic28", "@dicID_^*$ETC_20150317_004443$*^", "@dicID_^*$ETC_20150317_004444$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenShop("Klapeda_Weapon");
					break;
				case 2:
					await dialog.OpenShop("Klapeda_Armor");
					break;
			}
		}

		[DialogFunction]
		public static async Task ALFONSO(Dialog dialog)
		{
			switch (await dialog.Select("ALFONSO_DLG1", "!@#$ALFONSO_1#@!", "!@#$ALFONSO_2#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenShop("Klapeda_Accessory");
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.REQ_BRIQUETTING_HAIR_ACC_UI_OPEN);
					break;
			}
		}

		[DialogFunction]
		public static async Task KLAPEDA_USKA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
			//await dialog.Msg("ACT_KNIGHT_basic1");
			switch (await dialog.Select("ACT_KNIGHT_basic1", "{img minimap_1_SUB 16 16}@dicID_^*$QUEST_LV_0200_20150317_001589$*^", "@dicID_^*$ETC_20151224_018539$*^", "@dicID_^*$ETC_20151224_018540$*^", "@dicID_^*$ETC_20180418_032110$*^", "@dicID_^*$ITEM_20190104_021361$*^", "@dicID_^*$ETC_20210809_060225$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					switch (await dialog.Select("KATYN_14_HQ_01_ST", "KATYN_14_HQ_01", "@dicID_^*$QUEST_LV_0200_20150317_001590$*^", "@dicID_^*$QUEST_LV_0100_20150317_002369$*^"))
					{
						case 1:
							//player.Quests.Start(19091);
							break;
					}
					break;
				case 2:
					if (dialog.Player.GuildId == 0)
						await dialog.ExecuteScript(ClientScripts.OPEN_GUILD_CREATE_UI);
					break;
			}
		}

		[DialogFunction]
		public static async Task MASTER_CLERIC(Dialog dialog)
		{
			await dialog.Msg("MASTER_CLERIC_basic01");
			switch (await dialog.Select("MASTER_CLERIC_basic02", "@dicID_^*$ETC_20190104_037025$*^", "@dicID_^*$ETC_20191024_043341$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenShop("Master_Cleric");
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.HIDDENABILITY_DECOMPOSE_UI_OPEN);
					break;
				default:
					await Task.Yield();
					break;
			}
		}

		[DialogFunction]
		public static async Task ACT_VILLAGERS(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_Villagers_basic1");
			await dialog.Msg("KLAPEDA_Villagers_basic2");
			await dialog.Msg("KLAPEDA_Villagers_basic3");
			await dialog.Msg("KLAPEDA_Villagers_basic4");
			await dialog.Msg("KLAPEDA_Villagers_basic5");
			await dialog.Msg("KLAPEDA_Villagers_basic6");
			await dialog.Msg("KLAPEDA_Villagers_basic7");
			await dialog.Msg("KLAPEDA_Villagers_basic8");
			await dialog.Msg("KLAPEDA_Villagers_basic9");
			await dialog.Msg("KLAPEDA_Villagers_basic10");
			await dialog.Msg("KLAPEDA_Villagers_basic11");
			await dialog.Msg("KLAPEDA_Villagers_basic12");
			await dialog.Msg("KLAPEDA_Villagers_basic13");
			await dialog.Msg("KLAPEDA_Villagers_basic14");
			await dialog.Msg("KLAPEDA_Villagers_basic15");
			await dialog.Msg("KLAPEDA_Villagers_basic16");
			await dialog.Msg("KLAPEDA_Villagers_basic17");
			await dialog.Msg("KLAPEDA_Villagers_basic18");
			await dialog.Msg("KLAPEDA_Villagers_basic19");
			await dialog.Msg("KLAPEDA_Villagers_basic20");
			await dialog.Msg("KLAPEDA_Villagers_basic21");
			await dialog.Msg("KLAPEDA_Villagers_basic22");
		}

		[DialogFunction]
		public static async Task ACT_SMOM(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_Smom_basic1");
			await dialog.Msg("KLAPEDA_Smom_basic2");
			await dialog.Msg("KLAPEDA_Smom_basic3");
			await dialog.Msg("KLAPEDA_Smom_basic4");
			await dialog.Msg("KLAPEDA_Smom_basic5");
			await dialog.Msg("KLAPEDA_Smom_basic6");
			await dialog.Msg("KLAPEDA_Smom_basic7");
		}

		[DialogFunction]
		public static async Task MASTER_PRIEST(Dialog dialog)
		{
			await dialog.Msg("MASTER_PRIEST_basic1");
			await dialog.Msg("MASTER_PRIEST_basic2");
		}

		[DialogFunction]
		public static async Task KLAPEDA_SIGN9(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_SIGN9");
		}

		[DialogFunction]
		public static async Task KLAPEDA_BLACKSMITH(Dialog dialog)
		{
			dialog.SetTitle("Zaras");
			dialog.SetPortrait("Dlg_port_vettel");

			await BLACKSMITH_OPTIONS("Zaras, best Blacksmith in town! How may I help you?", dialog);
		}

		[DialogFunction]
		public static async Task ORSHA_BLACKSMITH(Dialog dialog)
		{
			dialog.SetTitle("Ilanai");
			dialog.SetPortrait("Dlg_port_Illanai");

			await BLACKSMITH_OPTIONS("Welcome to my humble shop~! How can I help you?", dialog);
		}

		[DialogFunction]
		public static async Task FEDIMIAN_BLACKSMITH(Dialog dialog)
		{
			dialog.SetTitle("Anna");
			dialog.SetPortrait("Dlg_port_Anna");

			await BLACKSMITH_OPTIONS("Welcome, welcome! My services will surprise you!", dialog);
		}

		private static async Task BLACKSMITH_OPTIONS(string greeting_message, Dialog dialog)
		{
			switch (await dialog.Select(greeting_message,
				"Repair",
				"Socket Management",
				"Identify",
				"Item Dismantle",
				// "Transcend Item",
				// "Item Dismantle",
				// "Item Re-Identification",
				// "Change Weapon Appearance",
				"Cancel"))
			{
				case 1:
					dialog.OpenAddon(AddonMessage.OPEN_DLG_REPAIR);
					break;
				// case 2:
				//  await dialog.OpenShop("Klapeda_Recipe");
				//  break;
				case 2:
					dialog.OpenAddon(AddonMessage.DO_OPEN_MANAGE_GEM_UI);
					break;
				case 3:
					await dialog.OpenCustomDialog(CustomDialog.APPRAISAL);
					break;
				case 4:
					dialog.OpenUI("itemdecompose");
					break;
					// case 5:
					// 	await dialog.CustomDialog(CustomDialog.ITEM_TRANSCEND);
					// 	break;
					// case 6:
					// 	await dialog.OpenUI("itemdecompose");
					// 	break;
					// case 7:
					// 	await dialog.OpenUI("itemrandomreset");
					// 	break;
					// case 8:
					// 	await dialog.OpenUI("briquetting");
					// 	break;
			}
		}

		[DialogFunction]
		public static async Task MASTER_KRIWI(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
			await dialog.Msg("MASTER_KRIWI_basic1");
			await dialog.OpenShop("Master_Kriwi");
		}

		[DialogFunction]
		public static async Task TUTO_GIRL(Dialog dialog)
		{
			await dialog.Msg("TUTO_GIRL_basic_01");
			await dialog.Msg("TUTO_GIRL_basic_02");
		}

		[DialogFunction]
		public static async Task KLAPEDA_NPC_14(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_NPC_14_basic01");
			await dialog.Msg("KLAPEDA_NPC_14_basic02");
			await dialog.Msg("KLAPEDA_NPC_14_basic03");
		}

		[DialogFunction]
		public static async Task KLAPEDA_MARKET(Dialog dialog)
		{
			switch (await dialog.Select("KLAPEDA_MARKET_SEL", "!@#$KLAPEDA_MARKET_OPEN#@!", "!@#$Auto_DaeHwa_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenCustomDialog(CustomDialog.MARKET);
					break;
			}
		}

		[DialogFunction]
		public static async Task MASTER_PELTASTA(Dialog dialog)
		{
			await dialog.Msg("MASTER_PELTASTA_basic1");
			await dialog.Msg("MASTER_PELTASTA_basic2");
		}

		[DialogFunction]
		public static async Task MASTER_ICEMAGE(Dialog dialog)
		{
			await dialog.Msg("MASTER_ICEMAGE_basic1");
		}

		[DialogFunction]
		public static async Task MASTER_QU(Dialog dialog)
		{
			await dialog.Msg("MASTER_QU_basic1");
			await dialog.Msg("MASTER_QU_basic2");
		}

		[DialogFunction]
		public static async Task MASTER_RANGER(Dialog dialog)
		{
			await dialog.Msg("MASTER_RANGER_basic1");
			await dialog.Msg("MASTER_RANGER_basic2");
		}

		[DialogFunction]
		public static async Task MASTER_SWORDMAN(Dialog dialog)
		{
			switch (await dialog.Select(RandomElement("MASTER_SWORDMAN_basic1", "MASTER_SWORDMAN_basic2"), "@dicID_^*$ETC_20190104_037026$*^", "@dicID_^*$ETC_20191024_043341$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenShop("Master_Swordman");
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.HIDDENABILITY_DECOMPOSE_UI_OPEN);
					break;
			}
		}

		[DialogFunction]
		public static async Task MASTER_WIZARD(Dialog dialog)
		{
			await dialog.Msg("MASTER_WIZARD_basic1");
			await dialog.Msg("MASTER_WIZARD_basic2");
		}

		[DialogFunction]
		public static async Task MASTER_ARCHER(Dialog dialog)
		{
			await dialog.Msg("MASTER_ARCHER_basic1");
			switch (await dialog.Select("MASTER_ARCHER_basic2", "@dicID_^*$ETC_20190104_037027$*^", "@dicID_^*$ETC_20191024_043341$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenShop("Master_Archer");
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.HIDDENABILITY_DECOMPOSE_UI_OPEN);
					break;
				default:
					await Task.Yield();
					break;
			}
		}

		[DialogFunction]
		public static async Task KLAPEDA_SIGN30(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_SIGN30");
		}

		[DialogFunction]
		public static async Task KLAPEDA_SIGN31(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_SIGN31");
		}

		[DialogFunction]
		public static async Task KLAPEDA_SIGN32(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_SIGN32");
		}

		[DialogFunction]
		public static async Task KLAPEDA_SIGN33(Dialog dialog)
		{
			dialog.Chat("!@#$KlaipedaCentralPlaza#@!");
		}

		[DialogFunction("KLAPEDA_SIGN_WELCOME")]
		public static async Task KLAPEDA_SIGN_WELCOME(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_SIGN_WELCOME");
		}

		[DialogFunction("KLAPEDA_SIGN35")]
		public static async Task KLAPEDA_SIGN35(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_SIGN35");
		}

		[DialogFunction("KLAPEDA_SIGN36")]
		public static async Task KLAPEDA_SIGN36(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_SIGN36");
		}

		[DialogFunction("KLAPEDA_BOARD_01")]
		public static async Task KLAPEDA_BOARD_01(Dialog dialog)
		{
			await dialog.Msg("KLAPEDA_BOARD_01");
		}

		[DialogFunction("HUEVILLAGE_58_3_KLAIPEDA_NPC")]
		public static async Task HUEVILLAGE_58_3_KLAIPEDA_NPC(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE_58_3_KLAIPEDA_NPC_basic01");
			await dialog.Msg("HUEVILLAGE_58_3_KLAIPEDA_NPC_basic02");
		}


		[DialogFunction]
		public static async Task JOURNEY_SHOP(Dialog dialog)
		{
			var character = dialog.Player;

			switch (await dialog.Select("RENA_BASIC01", "@dicID_^*$ETC_20150317_002514$*^", "@dicID_^*$ETC_20160310_021033$*^", "@dicID_^*$ETC_20211217_065424$*^", "@dicID_^*$ETC_20190802_042398$*^", "@dicID_^*$ETC_20180418_032113$*^", "@dicID_^*$ETC_20190104_037030$*^", "@dicID_^*$ETC_20190223_040846$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.ExecuteScript(ClientScripts.OPEN_DO_JOURNAL);
					break;
				case 2:
					var hasMapsToCheck = false;
					if (hasMapsToCheck)
					{
						// If stories to share (different map names)
						switch (await dialog.Select("RENA_NORMAL3_SELECT1", "@dicID_^*$ETC_20150317_001153$*^", "@dicID_^*$ETC_20200128_043943$*^", "!@#$Auto_JongLyo#@!"))
						{
							case 1:
								// Depending on which maps have been visited add reward item and reward property.
								// Reference property: PropertyName.Reward_{mapname}
								character.Etc.Properties.SetFloat(PropertyName.Reward_f_siauliai_out, 1);
								Send.ZC_OBJECT_PROPERTY(character, PropertyName.Reward_f_siauliai_out);
								Send.ZC_PC_PROP_UPDATE(character, (short)PropertyTable.GetId("PCEtc", PropertyName.Reward_f_siauliai_out), 1);
								break;
						}
					}
					else
					{
						// No stories to share
						await dialog.Msg("RENA_NORMAL3_NULL1");
					}
					break;
				case 3:
				{
					switch (await dialog.Select("RENA_GET_KEDORA_SUPPORT_BOX_DLG_2", "!@#$GET_KEDORA_SUPPORT_BOX_MSG_3#@!", "Lv40!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv75!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv120!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv170!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv220!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv270!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv315!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv350!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "Lv380!@#$GET_KEDORA_SUPPORT_BOX_MSG_2#@!", "NEXT PAGE"))
					{
						case 11:
							switch (await dialog.Select("RENA_GET_KEDORA_SUPPORT_BOX_DLG_2", "!@#$GET_KEDORA_SUPPORT_BOX_MSG_4#@!", "!@#$Auto_JongLyo#@!"))
							{
								case 1:
									character.AddItem(11030377);
									character.AddonMessage(AddonMessage.NOTICE_Dm_GetItem, "!@#$GET_KEDORA_SUPPORT_BOX_MSG_1#@!");
									break;
							}
							break;
					}
				}
				break;
			}
		}

		[DialogFunction]
		public static async Task COLLECTION_SHOP(Dialog dialog)
		{
			await dialog.Msg("HENRIKA_BASIC01");
			switch (await dialog.Select("HENRIKA_BASIC01", "@dicID_^*$ETC_20190104_037034$*^", "@dicID_^*$ETC_20190104_037035$*^", "@dicID_^*$ETC_20180320_031806$*^", "@dicID_^*$ETC_20200129_044673$*^", "@dicID_^*$ETC_20210115_054932$*^", "@dicID_^*$ETC_20210809_061344$*^", "@dicID_^*$ETC_20200129_044674$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 2:
					switch (await dialog.Select("COLLECT_REWARD_ITEM_FLOANA", "!@#$Yes#@!", "!@#$No#@!"))
					{
						default:
							await Task.Yield();
							break;
					}
					break;
				case 3:
					await dialog.OpenShop("Magic_Society");
					break;
				case 4:
					await dialog.ExecuteScript(ClientScripts.CONTENTS_TOTAL_SHOP_OPEN);
					break;
				case 5:
					await dialog.ExecuteScript(ClientScripts.SEASONOFF_CONTENTS_TOTAL_SHOP_OPEN);
					break;
				case 6:
					await dialog.ExecuteScript(ClientScripts.CLASS_COSTUME_TOTAL_SHOP_OPEN);
					break;
				case 7:
					switch (await dialog.Select("COLLECTION_SHOP_NORMAL_7_ORSHA", "!@#$JOURNEY_SHOP_LvRewards_Msg1#@!", "!@#$JOURNEY_SHOP_LvRewards_Msg2$*$LV$*$440#@!", "!@#$Auto_JongLyo#@!"))
					{
						default:
							await Task.Yield();
							break;
					}
					break;
			}
		}

		[DialogFunction]
		public static async Task WAREHOUSE(Dialog dialog)
		{
			switch (await dialog.Select("WAREHOUSE_DLG", "!@#$WareHouse#@!", "!@#$AccountWareHouse#@!", "!@#$Close#@!"))
			{
				case 1:
					await dialog.OpenCustomDialog(CustomDialog.WAREHOUSE);
					break;
				case 2:
					await dialog.OpenCustomDialog(CustomDialog.ACCOUNT_WAREHOUSE);
					break;
			}
		}

		[DialogFunction]
		public static async Task MASTER_ORACLE(Dialog dialog)
		{
			var player = dialog.Player;

			switch (await dialog.Select("MASTER_ORACLE_basic01", "@dicID_^*$ETC_20180629_034269$*^", "@dicID_^*$ETC_20150414_010982$*^", "@dicID_^*$ETC_20200514_047922$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.Msg("SOLO_DUNGEON_SELECT_DLG4");
					break;
				case 2:
					// Subtracts 1 TP per option.
					switch (await dialog.Select("MASTER_ORACLE_NORMAL_2_SELECT", "!@#$MASTER_ORACLE_NORMAL_2_AGREE5#@!!@#$OraclePreMsg1#@!1)", "!@#$MASTER_ORACLE_NORMAL_2_AGREE6#@!!@#$OraclePreMsg1#@!1)", "!@#$MASTER_ORACLE_NORMAL_2_AGREE7#@!!@#$OraclePreMsg1#@!1)", "!@#$MASTER_ORACLE_NORMAL_2_AGREE8#@!!@#$OraclePreMsg1#@!1)", "!@#$Auto_DaeHwa_JongLyo#@!"))
					{
						case 1:
							var account = player.Connection.Account;
							if (account.Charge(1))
								await dialog.Msg("JOB_ORACLE5_1_01\\!@#$MASTER_ORACLE_NORMAL_2_ANSWER1$*$day1$*$2022-12-06 13:00:00$*$day2$*$2022-12-07 13:00:00#@!!@#$MASTER_ORACLE_NORMAL_2_ANSWER3_2$*$rank$*$1$*$zone$*$@dicID_^*$ETC_20220823_069167$*^$*$count$*$947#@!!@#$MASTER_ORACLE_NORMAL_2_ANSWER3_2$*$rank$*$2$*$zone$*$@dicID_^*$ETC_20220823_069168$*^$*$count$*$745#@!!@#$MASTER_ORACLE_NORMAL_2_ANSWER3_2$*$rank$*$3$*$zone$*$@dicID_^*$QUEST_LV_0500_20220204_001588$*^$*$count$*$153#@!");
							break;
					}
					break;
				case 3:
					await dialog.Msg("ANCIENT_DUNGEON_DLG4");
					break;
			}
		}

		[DialogFunction]
		public static async Task WARP_C_KLAIPE(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task FEDIMIAN_ROTA_02(Dialog dialog)
		{
			await dialog.Msg("FEDIMIAN_ROTA_02_basic01");
			switch (await dialog.Select("CHAR318_MSETP1_QDLG1", "!@#$CHAR318_MSETP1_TXT1#@!", "!@#$CHAR318_MSETP1_TXT2#@!"))
			{
				case 1:
					await dialog.Msg("CHAR318_MSETP1_DLG1");
					break;
				default:
					await Task.Yield();
					break;
			}
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task MISSIONSHOP_NPC_01(Dialog dialog)
		{
			switch (await dialog.Select("MISSIONSHOP_NPC_01_DLG_1", "!@#$MISSIONSHOP_NPC_01_SELECT1_MSG7#@!", "!@#$MISSIONSHOP_NPC_01_SELECT1_MSG1#@!", "!@#$MISSIONSHOP_NPC_01_SELECT1_MSG2#@!", "!@#$MISSIONSHOP_NPC_01_SELECT1_MSG3#@!", "!@#$MISSIONSHOP_NPC_01_SELECT1_MSG4#@!", "!@#$MISSIONSHOP_NPC_01_SELECT1_MSG5#@!", "!@#$MISSIONSHOP_NPC_01_SELECT1_MSG6#@!", "!@#$Auto_JongLyo#@!"))
			{
				default:
					break;
			}
		}

		[DialogFunction]
		public static async Task PARTYQUEST_NPC_01(Dialog dialog)
		{
			var party = dialog.Player.Connection.Party;
			if (party == null)
			{
				await dialog.Msg("PARTYQUEST_NPC_01_CANCLE1");
				await Task.Yield();
			}
			else
			{
				switch (await dialog.Select("PARTYQUEST_NPC_01_SELECT1", "!@#$PARTYQUEST_NPC_01_SEL1#@!", "!@#$Auto_DaeHwa_JongLyo#@!"))
				{
					case 1:
						dialog.OpenAddon(AddonMessage.NOTICE_Dm_Scroll, "!@#$PartyQuestRandomCheckStartMsg$*$count$*$0$*$max$*$5$*$quest$*$@dicID_^*$QUEST_LV_0100_20150714_007355$*^#@!");
						party.SetProperty(PropertyName.P_PARTY_Q_030, "1");
						break;
					default:
						await Task.Yield();
						break;
				}
			}
		}

		[DialogFunction]
		public static async Task DROPITEM_REQUEST1_NPC(Dialog dialog)
		{
			await dialog.Msg("DROPITEM_REQUEST1_NPC_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task KLAPEDA_FISHING_CAT(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task LOWLV_GREEN_SELPHUI(Dialog dialog)
		{
			await dialog.Msg("LOWLV_GREEN_SELPHUI_BASIC");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task LOWLV_BOASTER_MUENCHHAUSEN(Dialog dialog)
		{
			await dialog.Msg(RandomElement<string>("MUENCHHAUSEN_BASIC1", "MUENCHHAUSEN_BASIC2"));
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task KLAPEDA_FISHING_MANAGER(Dialog dialog)
		{
			if (await dialog.HooksByDialogName("BeforeStart"))
				await Task.Yield();
			else if (await dialog.HooksByDialogName("BeforeEnd"))
				await Task.Yield();
			else
			{
				switch (await dialog.Select("Fish_Shop_Klapead", "{img minimap_1_MAIN 16 16}@dicID_^*$QUEST_LV_0100_20170912_016977$*^", "@dicID_^*$QUEST_LV_0200_20150714_008439$*^", "@dicID_^*$ETC_20170726_028404$*^", "!@#$Auto_JongLyo#@!"))
				{
					case 2:
						switch (await dialog.Select("Fish_Shop_Klapead_Dlg5", "!@#$FISH_RUBBING_YES#@!", "!@#$FISH_RUBBING_NO#@!"))
						{
							case 1:
								await dialog.Msg("Fish_Shop_Klapead_Dlg10");
								break;
						}
						break;
					case 3:
						await dialog.OpenShop("Fish_Shop_Manager");
						break;
				}
			}
		}

		[DialogFunction]
		public static async Task WORLDPVP_START(Dialog dialog)
		{
			await dialog.Msg("WORLDPVP_SELECT_MSG1");
			await dialog.Msg("WORLDPVP_SELECT_MSG2");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task CHAR119_MSTEP3_4_NPC(Dialog dialog)
		{
			await dialog.Msg("CHAR119_MSTEP3_4_NPC_basic1");
			await dialog.Msg("CHAR119_MSTEP3_4_NPC_basic2");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task ONMYOJI_MASTER(Dialog dialog)
		{
			await dialog.Msg("ONMYOJI_MASTER_basic1");
			await dialog.Msg("ONMYOJI_MASTER_basic2");
			await dialog.Msg("ONMYOJI_MASTER_basic3");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task NPC_JUNK_SHOP_KLAPEDA(Dialog dialog)
		{
			var character = dialog.Player;
			var account = character.Connection.Account;
			var isFree = account.Properties.GetFloat(PropertyName.JUNK_SHOP_BUY_COUNT) == 0;

			switch (await dialog.Select("NPC_JUNK_SHOP_KLAPEDA_DLG_01", "!@#$ASSISTOR_QUEST_MSG_02#@!", "!@#$ARCHEOLOGY_MSG_01#@!", "!@#$Cancel#@!"))
			{
				case 1:
					switch (await dialog.Select("NPC_JUNK_SHOP_KLAIPE_TAX", isFree ? "!@#$GEM_GACHA_FREE$*$GEMPAY$*$0#@!" : "!@#$GEM_GACHA_START$*$GEMPAY$*$30000#@!"))
					{
						case 1:
							var item = new Item(645639);
							if (isFree)
							{
								account.Properties.SetString(PropertyName.JUNK_BUY_COUNT_RESET_DAY, DateTimeUtils.ToSPropertyDTNow);
								Send.ZC_NORMAL.AccountProperties(character, PropertyName.JUNK_BUY_COUNT_RESET_DAY);
							}
							else
							{
								var fixedCost = 30000;
								if (character.Inventory.CountItem(ItemId.Silver) < fixedCost)
								{
									character.SystemMessage("NotEnoughMoney");
									return;
								}
								character.RemoveItem(ItemId.Silver, fixedCost);
							}
							account.Properties.Modify(PropertyName.JUNK_SHOP_BUY_COUNT, 1);
							Send.ZC_NORMAL.AccountProperties(character, PropertyName.JUNK_SHOP_BUY_COUNT);
							character.Inventory.Add(item);
							Send.ZC_NORMAL.ShowItemBalloon(character);
							Send.ZC_NORMAL.ShowItemBalloon(character, item, "junksilvergacha_itembox_low", "{@st43}", "JunkSilverGachaResultInRaidRewardSmall", 5);
							break;
					}
					break;
				case 2:
					await dialog.Msg("NPC_JUNK_SHOP_CHECK_LV_2");
					break;
			}
		}

		[DialogFunction]
		public static async Task BEAUTY_IN_MOVE(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task CTRLTYPE_RESET_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_BALLENCE_REWARD_NPC_DLG_11_GLOBAL\\!@#$JOB_BALLENCE_REWARD_NPC_CM_11_GLOBAL#@!");
			await dialog.Msg("JOB_BALLENCE_REWARD_NPC_DLG_11_GLOBAL\\!@#$JOB_BALLENCE_REWARD_NPC_CM_11_GLOBAL1$*$Char1$*$@dicID_^*$ETC_20190802_042303$*^ @dicID_^*$ETC_20190802_042304$*^ @dicID_^*$ETC_20190802_042305$*^ @dicID_^*$ETC_20190802_042306$*^ @dicID_^*$ETC_20190802_042308$*^ @dicID_^*$ETC_20190802_042309$*^ @dicID_^*$ETC_20190802_042310$*^ @dicID_^*$ETC_20190802_042311$*^ @dicID_^*$ETC_20190802_042312$*^ @dicID_^*$ETC_20190802_042313$*^ @dicID_^*$ETC_20190802_042314$*^ @dicID_^*$ETC_20190802_042315$*^ @dicID_^*$ETC_20190802_042316$*^ @dicID_^*$ETC_20190802_042317$*^ @dicID_^*$ETC_20190802_042318$*^ @dicID_^*$ETC_20190802_042319$*^ @dicID_^*$ETC_20190802_042320$*^ @dicID_^*$ETC_20200129_044605$*^ @dicID_^*$ETC_20211104_064667$*^ #@!");
			await dialog.Msg("JOB_BALLENCE_REWARD_NPC_DLG_11_GLOBAL\\!@#$JOB_BALLENCE_REWARD_NPC_CM_11_GLOBAL2$*$Char2$*$@dicID_^*$ETC_20190802_042321$*^ @dicID_^*$ETC_20190802_042322$*^ @dicID_^*$ETC_20190802_042323$*^ @dicID_^*$ETC_20190802_042324$*^ @dicID_^*$ETC_20190802_042325$*^ @dicID_^*$ETC_20190802_042326$*^ @dicID_^*$ETC_20190802_042327$*^ @dicID_^*$ETC_20190802_042328$*^ @dicID_^*$ETC_20190802_042329$*^ @dicID_^*$ETC_20190802_042331$*^ @dicID_^*$ETC_20190802_042332$*^ @dicID_^*$ETC_20190802_042333$*^ @dicID_^*$ETC_20190802_042334$*^ @dicID_^*$ETC_20190802_042335$*^ @dicID_^*$ETC_20190802_042336$*^ @dicID_^*$ETC_20190802_042337$*^ @dicID_^*$ETC_20190802_042338$*^ @dicID_^*$ETC_20200129_044606$*^ @dicID_^*$ETC_20220426_068015$*^ #@!");
			await dialog.Msg("JOB_BALLENCE_REWARD_NPC_DLG_11_GLOBAL\\!@#$JOB_BALLENCE_REWARD_NPC_CM_11_GLOBAL3$*$Char3$*$@dicID_^*$ETC_20190802_042339$*^ @dicID_^*$ETC_20190802_042340$*^ @dicID_^*$ETC_20190802_042341$*^ @dicID_^*$ETC_20190802_042342$*^ @dicID_^*$ETC_20190802_042343$*^ @dicID_^*$ETC_20190802_042344$*^ @dicID_^*$ETC_20190802_042345$*^ @dicID_^*$ETC_20190802_042346$*^ @dicID_^*$ETC_20190802_042347$*^ @dicID_^*$ETC_20190802_042348$*^ @dicID_^*$ETC_20190802_042349$*^ @dicID_^*$ETC_20190802_042350$*^ @dicID_^*$ETC_20190802_042351$*^ @dicID_^*$ETC_20190802_042352$*^ @dicID_^*$ETC_20190802_042353$*^ @dicID_^*$ETC_20200129_044607$*^ @dicID_^*$ETC_20210115_054773$*^ @dicID_^*$ETC_20220204_066623$*^ #@!");
			await dialog.Msg("JOB_BALLENCE_REWARD_NPC_DLG_11_GLOBAL\\!@#$JOB_BALLENCE_REWARD_NPC_CM_11_GLOBAL4$*$Char4$*$@dicID_^*$ETC_20190802_042354$*^ @dicID_^*$ETC_20190802_042355$*^ @dicID_^*$ETC_20190802_042356$*^ @dicID_^*$ETC_20190802_042357$*^ @dicID_^*$ETC_20190802_042358$*^ @dicID_^*$ETC_20190802_042359$*^ @dicID_^*$ETC_20190802_042360$*^ @dicID_^*$ETC_20190802_042361$*^ @dicID_^*$ETC_20190802_042362$*^ @dicID_^*$ETC_20190802_042363$*^ @dicID_^*$ETC_20190802_042364$*^ @dicID_^*$ETC_20190802_042366$*^ @dicID_^*$ETC_20190802_042367$*^ @dicID_^*$ETC_20190802_042368$*^ @dicID_^*$ETC_20190802_042369$*^ @dicID_^*$ETC_20190802_042370$*^ @dicID_^*$ETC_20190802_042371$*^ @dicID_^*$ETC_20200129_044608$*^ @dicID_^*$ETC_20220614_068824$*^ #@!");
			await dialog.Msg("JOB_BALLENCE_REWARD_NPC_DLG_11_GLOBAL\\!@#$JOB_BALLENCE_REWARD_NPC_CM_11_GLOBAL5$*$Char5$*$@dicID_^*$ETC_20190802_042372$*^ @dicID_^*$ETC_20190802_042373$*^ @dicID_^*$ETC_20190802_042374$*^ @dicID_^*$ETC_20190802_042375$*^ @dicID_^*$ETC_20190802_042376$*^ @dicID_^*$ETC_20190802_042377$*^ @dicID_^*$ETC_20190802_042378$*^ @dicID_^*$ETC_20190802_042379$*^ @dicID_^*$ETC_20190802_042380$*^ @dicID_^*$ETC_20190802_042381$*^ @dicID_^*$ETC_20190802_042382$*^ @dicID_^*$ETC_20190802_042383$*^ @dicID_^*$ETC_20190802_042384$*^ @dicID_^*$ETC_20190802_042385$*^ @dicID_^*$ETC_20200129_044609$*^ @dicID_^*$ETC_20210115_054774$*^ @dicID_^*$ETC_20220204_066624$*^ @dicID_^*$ETC_20221125_071531$*^ #@!");
			switch (await dialog.Select("JOB_BALLENCE_REWARD_NPC_DLG_11_GLOBAL\\!@#$JOB_BALLENCE_REWARD_NPC_CHECK_CLASS#@!", "!@#$EVENT_1901_RANKRESET_NPC_1#@!", "!@#$Auto_JongLyo#@!"))
			{
				default:
					await Task.Yield();
					break;
			}
		}

		[DialogFunction]
		public static async Task JOB_SCOUT3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_SCOUT3_1_NPC_BASIC01");
			await dialog.Msg("JOB_SCOUT3_1_NPC_BASIC02");
			switch (await dialog.Select(RandomElement("JOB_SCOUT3_1_NPC_BASIC01", "JOB_SCOUT3_1_NPC_BASIC02"), "@dicID_^*$ETC_20190104_037024$*^", "@dicID_^*$ETC_20191024_043341$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.OpenShop("Master_Scout");
					break;
				case 2:
					await dialog.ExecuteScript(ClientScripts.HIDDENABILITY_DECOMPOSE_UI_OPEN);
					break;
			}
		}

		[DialogFunction]
		public static async Task ALCHEIST_EXPERT_NPC_KLAPEDA(Dialog dialog)
		{
			await dialog.Msg("ALCHEIST_EXPERT_NPC_KLAPEDA_basic");
		}

		[DialogFunction]
		public static async Task TOSHERO_TUTO_NPC_01(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task CARE_POPOBOOST_STATUS(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task MASTER_HIGHLANDER(Dialog dialog)
		{
			await dialog.Msg("MASTER_HIGHLANDER_basic1");
			await dialog.Msg("MASTER_HIGHLANDER_basic2");
		}

		[DialogFunction]
		public static async Task MASTER_BOCORS(Dialog dialog)
		{
			await dialog.Msg("MASTER_BOCORS_basic2");
			await dialog.Msg("MASTER_BOCORS_basic1");
		}

		[DialogFunction]
		public static async Task MASTER_FIREMAGE(Dialog dialog)
		{
			await dialog.Msg("MASTER_FIREMAGE_basic1");
			await dialog.Msg("MASTER_FIREMAGE_basic2");
		}

		[DialogFunction]
		public static async Task MASTER_CHAR2_24_CITY(Dialog dialog)
		{
			await dialog.Msg("MASTER_CHAR2_24_1_basic1");
		}

		[DialogFunction]
		public static async Task FEDIMIAN_BOARD_01(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task C_REQUEST_1_NPC_01(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task C_REQUEST_1_NPC_02(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task ORSHA_KEDORLA_BOARD01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task ORSHA_EQUIPMENT_DEALER(Dialog dialog)
		{
			await dialog.Msg("ORSHA_EQUIPMENT_DEALER_basic01");
			await dialog.Msg("ORSHA_EQUIPMENT_DEALER_basic02");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task ORSHA_KEDORLA_MEMBER01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task C_ORSHA_HAMONDAIL(Dialog dialog)
		{
			await dialog.Msg("C_ORSHA_HAMONDAIL_basic_01");
		}

		[DialogFunction]
		public static async Task C_ORSHA_SOLDIER_02(Dialog dialog)
		{
			await dialog.Msg("C_ORSHA_SOLDIER_02_basic_01");
		}

		[DialogFunction]
		public static async Task C_ORSHA_URBONAS(Dialog dialog)
		{
			await dialog.Msg("C_ORSHA_URBONAS_basic_01");
			await dialog.Msg("C_ORSHA_URBONAS_basic_02");
		}

		[DialogFunction]
		public static async Task ORSHA_TOOL_NPC(Dialog dialog)
		{
			await dialog.Msg("ORSHA_TOOL_NPC_basic01");
			await dialog.Msg("ORSHA_TOOL_NPC_basic02");
		}

		[DialogFunction]
		public static async Task ORSHA_ACCESSARY_NPC(Dialog dialog)
		{
			await dialog.Msg("ORSHA_ACCESSARY_NPC_basic01");
			await dialog.Msg("ORSHA_ACCESSARY_NPC_basic02");
		}

		[DialogFunction]
		public static async Task DAOSHI_MASTER(Dialog dialog)
		{
			await dialog.Msg("DAOSHI_MASTER_basic1");
			await dialog.Msg("DAOSHI_MASTER_basic2");
		}

		[DialogFunction]
		public static async Task JOB_2_PELTASTA_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_PELTASTA_basic01");
			await dialog.Msg("JOB_2_PELTASTA_basic02");
		}

		[DialogFunction]
		public static async Task JOB_2_HOPLITE_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_HOPLITE_basic01");
			await dialog.Msg("JOB_2_HOPLITE_basic02");
		}

		[DialogFunction]
		public static async Task JOB_2_CLERIC_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_CLERIC_basic01");
			await dialog.Msg("JOB_2_CLERIC_basic02");
		}

		[DialogFunction]
		public static async Task JOB_2_KRIVIS_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_KRIVIS_basic01");
			await dialog.Msg("JOB_2_KRIVIS_basic02");
		}

		[DialogFunction]
		public static async Task JOB_2_PRIEST_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_PRIEST_basic01");
			await dialog.Msg("JOB_2_PRIEST_basic02");
		}

		[DialogFunction]
		public static async Task JOB_2_BOKOR_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_BOKOR_basic01");
			await dialog.Msg("JOB_2_BOKOR_basic02");
		}

		[DialogFunction]
		public static async Task JOB_2_RODELERO_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_RODELERO_basic01");
			await dialog.Msg("JOB_2_RODELERO_basic02");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_CATAPHRACT_NPC")]
		public static async Task JOB_2_CATAPHRACT_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_CATAPHRACT_basic01");
			await dialog.Msg("JOB_2_CATAPHRACT_basic02");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_PALADIN_NPC")]
		public static async Task JOB_2_PALADIN_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_PALADIN_basic01");
			await dialog.Msg("JOB_2_PALADIN_basic02");
			await Task.Yield();
		}

		[DialogFunction("C_ORSHA_PRANAS")]
		public static async Task C_ORSHA_PRANAS(Dialog dialog)
		{
			await dialog.Msg("ORSHA_MQ2_01_02");
			await Task.Yield();
		}

		[DialogFunction("C_ORSHA_SOLDIER_01")]
		public static async Task C_ORSHA_SOLDIER_01(Dialog dialog)
		{
			await dialog.Msg("C_ORSHA_SOLDIER_01_basic_01");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_SADHU_NPC")]
		public static async Task JOB_2_SADHU_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_SADHU_basic01");
			await dialog.Msg("JOB_2_SADHU_basic02");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_SWORDMAN_NPC")]
		public static async Task JOB_2_SWORDMAN_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_2_SWORDMAN_basic01");
			await dialog.Msg("JOB_2_SWORDMAN_basic02");
			await Task.Yield();
		}

		[DialogFunction("WARP_C_ORSHA")]
		public static async Task WARP_C_ORSHA(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("JOB_2_WIZARD_MASTER")]
		public static async Task JOB_2_WIZARD_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_WIZARD_MASTER_basic1");
			await dialog.Msg("JOB_2_WIZARD_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_PYROMANCER_MASTER")]
		public static async Task JOB_2_PYROMANCER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_PYROMANCER_MASTER_basic1");
			await dialog.Msg("JOB_2_PYROMANCER_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_CRYOMANCER_MASTER")]
		public static async Task JOB_2_CRYOMANCER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_CRYOMANCER_MASTER_basic1");
			await dialog.Msg("JOB_2_CRYOMANCER_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_PSYCHOKINO_MASTER")]
		public static async Task JOB_2_PSYCHOKINO_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_PSYCHOKINO_MASTER_basic1");
			await dialog.Msg("JOB_2_PSYCHOKINO_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_LINKER_MASTER")]
		public static async Task JOB_2_LINKER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_LINKER_MASTER_basic1");
			await dialog.Msg("JOB_2_LINKER_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_THAUMATURGE_MASTER")]
		public static async Task JOB_2_THAUMATURGE_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_THAUMATURGE_MASTER_basic1");
			await dialog.Msg("JOB_2_THAUMATURGE_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_ELEMENTALIST_MASTER")]
		public static async Task JOB_2_ELEMENTALIST_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_ELEMENTALIST_MASTER_basic1");
			await dialog.Msg("JOB_2_ELEMENTALIST_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_ARCHER_MASTER")]
		public static async Task JOB_2_ARCHER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_ARCHER_MASTER_basic1");
			await dialog.Msg("JOB_2_ARCHER_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_RANGER_MASTER")]
		public static async Task JOB_2_RANGER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_RANGER_MASTER_basic1");
			await dialog.Msg("JOB_2_RANGER_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_QUARRELSHOOTER_MASTER")]
		public static async Task JOB_2_QUARRELSHOOTER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_QUARRELSHOOTER_MASTER_basic1");
			await dialog.Msg("JOB_2_QUARRELSHOOTER_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_2_SAPPER_MASTER")]
		public static async Task JOB_2_SAPPER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_SAPPER_MASTER_basic1");
			await dialog.Msg("JOB_2_SAPPER_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_HUNTER_MASTER")]
		public static async Task JOB_2_HUNTER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_HUNTER_MASTER_basic1");
			await dialog.Msg("JOB_2_HUNTER_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_WUGUSHI_MASTER")]
		public static async Task JOB_2_WUGUSHI_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_WUGUSHI_MASTER_basic1");
			await dialog.Msg("JOB_2_WUGUSHI_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("JOB_2_SCOUT_MASTER")]
		public static async Task JOB_2_SCOUT_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_2_SCOUT_MASTER_basic1");
			await dialog.Msg("JOB_2_SCOUT_MASTER_basic2");
			await Task.Yield();
		}


		[DialogFunction("ORSHA_MARKET")]
		public static async Task ORSHA_MARKET(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("ORSHA_WAREHOUSE")]
		public static async Task ORSHA_WAREHOUSE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("ORSHA_COLLECTION_SHOP")]
		public static async Task ORSHA_COLLECTION_SHOP(Dialog dialog)
		{
			switch (await dialog.Select("ORSHA_COLLECTION_SHOP_basic01", "@dicID_^*$ETC_20190104_037034$*^", "@dicID_^*$ETC_20190104_037035$*^", "@dicID_^*$ETC_20180320_031806$*^", "@dicID_^*$ETC_20200129_044673$*^", "@dicID_^*$ETC_20210115_054932$*^", "@dicID_^*$ETC_20210809_061344$*^", "@dicID_^*$ETC_20200129_044674$*^", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await Task.Yield();
					dialog.OpenAddon(AddonMessage.COLLECTION_UI_OPEN);
					break;
				case 2:
					switch (await dialog.Select("COLLECT_REWARD_ITEM_FLOANA", "!@#$Yes#@!", "!@#$No#@!"))
					{
						default:
							await Task.Yield();
							break;
					}
					break;
				case 3:
					await dialog.OpenShop("Magic_Society");
					break;
				case 4:
					await dialog.ExecuteScript(ClientScripts.CONTENTS_TOTAL_SHOP_OPEN);
					break;
				case 5:
					await dialog.ExecuteScript(ClientScripts.SEASONOFF_CONTENTS_TOTAL_SHOP_OPEN);
					break;
				case 6:
					await dialog.ExecuteScript(ClientScripts.CLASS_COSTUME_TOTAL_SHOP_OPEN);
					break;
				case 7:
					await Task.Yield();
					switch (await dialog.Select("COLLECTION_SHOP_NORMAL_7_ORSHA", "!@#$JOURNEY_SHOP_LvRewards_Msg1#@!", "!@#$JOURNEY_SHOP_LvRewards_Msg2$*$LV$*$440#@!", "!@#$Auto_JongLyo#@!"))
					{
						default:
							break;
					}
					break;
			}
		}

		[DialogFunction("ORSHA_JOURNEY_SHOP")]
		public static async Task ORSHA_JOURNEY_SHOP(Dialog dialog)
		{
			await dialog.Msg("ORSHA_JOURNEY_SHOP_basic01");
			await Task.Yield();
		}

		[DialogFunction("ORSHA_NPC01")]
		public static async Task ORSHA_NPC01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("ORSHA_NPC02")]
		public static async Task ORSHA_NPC02(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("ORSHA_BOOK01")]
		public static async Task ORSHA_BOOK01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("ORSHA_BOOK02")]
		public static async Task ORSHA_BOOK02(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("TREASUREBOX_LV_C_ORSHA150")]
		public static async Task TREASUREBOX_LV_C_ORSHA150(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_257", 1);

		[DialogFunction("HIDDEN_MIKO_JUROTA")]
		public static async Task HIDDEN_MIKO_JUROTA(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("ORSHA_CHAR119_MSTEP1")]
		public static async Task ORSHA_CHAR119_MSTEP1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("EP12_PRELUDE_NERINGA_ORSHA1")]
		public static async Task EP12_PRELUDE_NERINGA_ORSHA1(Dialog dialog)
		{
			await dialog.Msg("EP12_NERINGA_NORMAL_02");
			await Task.Yield();
		}

		[DialogFunction("FISHING_SUB_ORSHA_NPC")]
		public static async Task FISHING_SUB_ORSHA_NPC(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("EP13_WEEK_REPUTATION_01")]
		public static async Task EP13_WEEK_REPUTATION_01(Dialog dialog)
		{
			await dialog.Msg("REPUTATION_NPC_BASIC6");
			await Task.Yield();
		}

		[DialogFunction("CLO_MASTER")]
		public static async Task CLO_MASTER(Dialog dialog)
		{
			await dialog.Msg("MASTER_CLO_NPC_basic");
			await Task.Yield();
		}

		[DialogFunction("ARQ_MASTER")]
		public static async Task ARQ_MASTER(Dialog dialog)
		{
			await dialog.Msg("MASTER_ARQ_NPC_basic");
			await Task.Yield();
		}

		[DialogFunction("SAGE_MASTER")]
		public static async Task SAGE_MASTER(Dialog dialog)
		{
			await dialog.Msg("SAGE_MASTER_basic1");
			await dialog.Msg("SAGE_MASTER_basic2");
			await Task.Yield();
		}

		[DialogFunction("CHAR119_MSTEP3_3_1_NPC")]
		public static async Task CHAR119_MSTEP3_3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("CHAR119_MSTEP3_3_1_NPC_basic1");
			await Task.Yield();
		}

		[DialogFunction("ALCHEIST_EXPERT_NPC_ORSHA")]
		public static async Task ALCHEIST_EXPERT_NPC_ORSHA(Dialog dialog)
		{
			await dialog.Msg("ALCHEIST_EXPERT_NPC_ORSHA_basic");
			await Task.Yield();
		}

		[DialogFunction("THREECMLAKE_84_HIDDEN_MADAM")]
		public static async Task THREECMLAKE_84_HIDDEN_MADAM(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("MASTER_HWARANG_NPC")]
		public static async Task MASTER_HWARANG_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_HWARANG_NPC_basic01");
			await dialog.Msg("MASTER_HWARANG_NPC_basic02");
			await Task.Yield();
		}

		[DialogFunction("EP14_MULIA_NPC_3")]
		public static async Task EP14_MULIA_NPC_3(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("EP14_F_CORAL_RAID_1_NPC_1")]
		public static async Task EP14_F_CORAL_RAID_1_NPC_1(Dialog dialog)
		{
			await dialog.Msg("EP14_F_CORAL_RAID_1_NPC_1_basic_1");
			await Task.Yield();
		}

		[DialogFunction("WARP_C_NUNNERY")]
		public static async Task WARP_C_NUNNERY(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("REQUEST_MISSION_ABBEY")]
		public static async Task REQUEST_MISSION_ABBEY(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("C_NUNNERY_NPC1")]
		public static async Task C_NUNNERY_NPC1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("ENCHANTER_MASTER")]
		public static async Task ENCHANTER_MASTER(Dialog dialog)
		{
			await dialog.Msg("ENCHANTER_MASTER_basic1");
			await dialog.Msg("ENCHANTER_MASTER_basic2");
			await dialog.Msg("ENCHANTER_MASTER_basic3");
			await Task.Yield();
		}

		[DialogFunction("HT_ESCAPE_MERCHANT")]
		public static async Task HT_ESCAPE_MERCHANT(Dialog dialog)
		{
			await dialog.Msg("HT_ESCAPE_MERCHANT_BASIC01");
			await dialog.Msg("HT_ESCAPE_MERCHANT_BASIC02");
			await Task.Yield();
		}

		[DialogFunction("KEY_QUEST_NPC_NUNNERY")]
		public static async Task KEY_QUEST_NPC_NUNNERY(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("MASTER_MATROSS_NPC")]
		public static async Task MASTER_MATROSS_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_MATROSS_NPC_basic1");
			await dialog.Msg("MASTER_MATROSS_NPC_basic2");
			await Task.Yield();
		}

		[DialogFunction("MASTER_ARDITI_NPC")]
		public static async Task MASTER_ARDITI_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_ARDITI_NPC_basic1");
			await dialog.Msg("MASTER_ARDITI_NPC_basic2");
			await Task.Yield();
		}

		[DialogFunction("MASTER_SHERIFF_NPC")]
		public static async Task MASTER_SHERIFF_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_SHERIFF_NPC_basic1");
			await dialog.Msg("MASTER_SHERIFF_NPC_basic2");
			await Task.Yield();
		}

		[DialogFunction("MASTER_TERRAMANCER_NPC")]
		public static async Task MASTER_TERRAMANCER_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_TERRAMANCER_NPC_basic1");
			await dialog.Msg("MASTER_TERRAMANCER_NPC_basic2");
			await dialog.Msg("MASTER_TERRAMANCER_NPC_basic_move");
			await Task.Yield();
		}

		[DialogFunction("MASTER_BLOSSOMBLADER_NPC")]
		public static async Task MASTER_BLOSSOMBLADER_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_BLOSSOMBLADER_NPC_basic1");
			await dialog.Msg("MASTER_BLOSSOMBLADER_NPC_basic_move");
			await Task.Yield();
		}

		[DialogFunction("MASTER_RANGDA_NPC")]
		public static async Task MASTER_RANGDA_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_RANGDA_NPC_basic1");
			await dialog.Msg("MASTER_RANGDA_NPC_basic2");
			await dialog.Msg("MASTER_RANGDA_NPC_basic_move");
			await Task.Yield();
		}

		[DialogFunction("NPC_MIELAS_01")]
		public static async Task NPC_MIELAS_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction("NPC_RANGDAGIRL_01")]
		public static async Task NPC_RANGDAGIRL_01(Dialog dialog)
		{
			await dialog.Msg("NPC_RANGDAGIRL_01_basic1");
			await Task.Yield();
		}

		[DialogFunction("MASTER_LUCHADOR_NPC")]
		public static async Task MASTER_LUCHADOR_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_LUCHADOR_NPC_basic01");
			await Task.Yield();
		}

		[DialogFunction("MASTER_LAMA_NPC")]
		public static async Task MASTER_LAMA_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_CHAR4_22_1_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task MASTER_SPE_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_Char1_24_1_basic1");
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BEAUTY_HAIRSHOP_MOVE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BEAUTY_BOUTIQUE_MOVE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BEAUTY_OUT_MOVE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BEAUTY_SHOP_HAIR_F(Dialog dialog)
		{
			switch (await dialog.Select("BEAUTY_SHOP_HAIR_F1", "!@#$BEAUTY_SHOP_HAIR_F2#@!", "!@#$BEAUTY_SHOP_HAIR_F3#@!", "!@#$BEAUTY_SHOP_HAIR_F4#@!", "!@#$BEAUTY_SHOP_SKIN#@!", "!@#$BEAUTY_SHOP_HAIR_ETC#@!", "!@#$BEAUTY_SHOP_HAIR_COUPON#@!", "!@#$Close#@!"))
			{
				case 1:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "HAIR", 2);
					break;
				case 2:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "WIG", 2);
					break;
				case 3:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "DESIGNCUT", 2);
					break;
				case 4:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "SKIN");
					break;
				case 5:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "ETC");
					break;
				case 6:
					dialog.OpenAddon(AddonMessage.UPDATE_BEAUTY_COUPON_STAMP, "None");
					await dialog.ExecuteScript(ClientScripts.BEAUTY_COUPON_OPEN);
					break;
			}
		}

		[DialogFunction]
		public static async Task BEAUTY_SHOP_HAIR_M(Dialog dialog)
		{
			switch (await dialog.Select("BEAUTY_SHOP_HAIR_M1", ScpArgMsg("BEAUTY_SHOP_HAIR_M2"),
				ScpArgMsg("BEAUTY_SHOP_HAIR_M3"), ScpArgMsg("BEAUTY_SHOP_HAIR_M4"), ScpArgMsg("BEAUTY_SHOP_SKIN"),
				ScpArgMsg("BEAUTY_SHOP_HAIR_ETC"), ScpArgMsg("BEAUTY_SHOP_HAIR_COUPON"), ScpArgMsg("Close")))
			{
				case 1:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "HAIR", 1);
					break;
				case 2:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "WIG", 1);
					break;
				case 3:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "DESIGNCUT", 1);
					break;
				case 4:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "SKIN");
					break;
				case 5:
					dialog.OpenAddon(AddonMessage.BEAUTYSHOP_UI_OPEN, "ETC");
					break;
				case 6:
					dialog.OpenAddon(AddonMessage.UPDATE_BEAUTY_COUPON_STAMP, "None");
					await dialog.ExecuteScript(ClientScripts.BEAUTY_COUPON_OPEN);
					break;
			}
		}

		[DialogFunction]
		public static async Task BEAUTY_SHOP_FASHION(Dialog dialog)
		{
			switch (await dialog.Select("BEAUTY_SHOP_FASHION",
				ScpArgMsg("BEAUTY_SHOP_FASHION_1"), ScpArgMsg("BEAUTY_SHOP_FASHION_2"), ScpArgMsg("BEAUTY_SHOP_FASHION_4"),
				ScpArgMsg("PREVIEW_SILVERGACHA"), ScpArgMsg("Close")))
			{
				case 1:
					dialog.OpenAddon("BEAUTYSHOP_UI_OPEN", "COSTUME", 1);
					break;
				case 2:
					dialog.OpenAddon("BEAUTYSHOP_UI_OPEN", "COSTUME", 2);
					break;
				case 3:
					dialog.OpenAddon("BEAUTYSHOP_UI_OPEN", "PREVIEW");
					break;
				case 4:
					dialog.OpenAddon("BEAUTYSHOP_UI_OPEN", "PREVIEW_SILVERGACHA");
					break;
			}
		}

		[DialogFunction("SIALUL_WEST_DRASIUS")]
		public static async Task SIALUL_WEST_DRASIUS(Dialog dialog)
		{
			await dialog.Msg("SIALUL_WEST_DRASIUS_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction("SIAUL_WEST_NAGLIS2")]
		public static async Task SIAUL_WEST_NAGLIS2(Dialog dialog)
		{
			await dialog.Msg("SIAUL_WEST_NAGLIS2_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction("WARP_F_SIAULIAI_WEST")]
		public static async Task WARP_F_SIAULIAI_WEST(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("SIAUL_WEST_CAMP_MANAGER")]
		public static async Task SIAUL_WEST_CAMP_MANAGER(Dialog dialog)
		{
			await dialog.Msg("SIAUL_WEST_CAMP_MANAGER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_WEST_LAIMONAS")]
		public static async Task SIAUL_WEST_LAIMONAS(Dialog dialog)
		{
			await dialog.Msg("SIAUL_WEST_LAIMONAS_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAU_FRON_NPC_01")]
		public static async Task SIAU_FRON_NPC_01(Dialog dialog)
		{
			await dialog.Msg("SIAU_FRON_NPC_01_basic01", "SIAU_FRON_NPC_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_WEST_RESIDENT1")]
		public static async Task SIAUL_WEST_RESIDENT1(Dialog dialog)
		{
			await dialog.Msg("SIAUL_WEST_RESIDENT1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
			switch (await dialog.Select("SIAUL_WEST_RESIDENT1_basic2", ScpArgMsg("SIAUL_WEST_RESIDENT1_select01"), ScpArgMsg("Auto_JongLyo")))
			{
				case 1:
					switch (await dialog.Select("SIAUL_WEST_RESIDENT1_basic3", ScpArgMsg("SIAUL_WEST_RESIDENT1_select03"), ScpArgMsg("Auto_JongLyo")))
					{
						case 1:
							await dialog.Msg("SIAUL_WEST_RESIDENT1_basic5");
							break;
					}
					break;
			}
		}

		[DialogFunction("F_SIAUL_GAURD")]
		public static async Task F_SIAUL_GAURD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_ST1_ST2_GAURD")]
		public static async Task SIAUL_ST1_ST2_GAURD(Dialog dialog)
		{
			await dialog.Msg("SIAUL_ST1_ST2_GAURD_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_ST1_ST2")]
		public static async Task SIAUL_ST1_ST2(Dialog dialog)
		{
			await dialog.Msg("SIAUL_ST1_ST2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_WEST_SOLDIER3")]
		public static async Task SIAUL_WEST_SOLDIER3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAU_FRON_NPC_04")]
		public static async Task SIAU_FRON_NPC_04(Dialog dialog)
		{
			await dialog.Msg("SIAU_FRON_NPC_04_basic01");
			await dialog.Msg("SIAU_FRON_NPC_04_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAU_FRON_NPC_05")]
		public static async Task SIAU_FRON_NPC_05(Dialog dialog)
		{
			await dialog.Msg("SIAU_FRON_NPC_05_basic01");
			await dialog.Msg("SIAU_FRON_NPC_05_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAU_FRON_NPC_03")]
		public static async Task SIAU_FRON_NPC_03(Dialog dialog)
		{
			await dialog.Msg("SIAU_FRON_NPC_03_basic01");
			await dialog.Msg("SIAU_FRON_NPC_03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAU_FRON_NPC_02")]
		public static async Task SIAU_FRON_NPC_02(Dialog dialog)
		{
			await dialog.Msg("SIAU_FRON_NPC_02_basic01");
			await dialog.Msg("SIAU_FRON_NPC_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_WEST_SOL3")]
		public static async Task SIAUL_WEST_SOL3(Dialog dialog)
		{
			await dialog.Msg("SIAUL_WEST_SOL3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL1_BOARD2")]
		public static async Task SIAUL1_BOARD2(Dialog dialog)
		{
			await dialog.Msg("SIAUL1_BOARD2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL1_BOARD3")]
		public static async Task SIAUL1_BOARD3(Dialog dialog)
		{
			await dialog.Msg("SIAUL1_BOARD3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL1_BOARD4")]
		public static async Task SIAUL1_BOARD4(Dialog dialog)
		{
			await dialog.Msg("SIAUL1_BOARD4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL1_BOARD5")]
		public static async Task SIAUL1_BOARD5(Dialog dialog)
		{
			await dialog.Msg("SIAUL1_BOARD5");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL1_BOARD6")]
		public static async Task SIAUL1_BOARD6(Dialog dialog)
		{
			await dialog.Msg("SIAUL1_BOARD6");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL1_BOARD7")]
		public static async Task SIAUL1_BOARD7(Dialog dialog)
		{
			await dialog.Msg("SIAUL1_BOARD7");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL1_BOARDCRYSTAL")]
		public static async Task SIAUL1_BOARDCRYSTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_HOPLITE2_NPC")]
		public static async Task JOB_HOPLITE2_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_HOPLITE2_NPC_BASIC01");
			await dialog.Msg("JOB_HOPLITE2_NPC_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_WEST_HQ01_INFO01")]
		public static async Task SIAUL_WEST_HQ01_INFO01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_WEST_HQ01_INFO02")]
		public static async Task SIAUL_WEST_HQ01_INFO02(Dialog dialog)
		{
			var character = dialog.Player;
			if (!character.SessionObjects.Main.Properties.Has(PropertyName.SIAUL_WEST_HQ01_INFO2))
			{
				character.SessionObjects.Main.Properties.SetFloat(PropertyName.SIAUL_WEST_HQ01_INFO2, 1);
			}
			await dialog.Msg("SIAUL1_BOARD9");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_WEST_HQ01_INFO03")]
		public static async Task SIAUL_WEST_HQ01_INFO03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TUTO_SKIP_NPC")]
		public static async Task TUTO_SKIP_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_WEST2027")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_WEST2027(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "NECK01_109", 1);

		[DialogFunction("F_SIAULIAI_WEST_EV_55_001")]
		public static async Task F_SIAULIAI_WEST_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_WEST2032")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_WEST2032(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "BRC02_114", 1);

		[DialogFunction("JOB_DIEVDIRBYS2_NPC")]
		public static async Task JOB_DIEVDIRBYS2_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_DIEVDIRBYS2_NPC_basisc01");
			await dialog.Msg("JOB_DIEVDIRBYS2_NPC_basisc02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_DIEVDIRBYS3_3_NPC")]
		public static async Task JOB_DIEVDIRBYS3_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_WEST2034")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_WEST2034(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "Drug_HP1_Q", 4);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_WEST2035")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_WEST2035(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "Hat_628037", 1, "TreasureboxKey3");

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_WEST2036")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_WEST2036(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_118", 1);

		[DialogFunction("RUNECASTER_SIAULIAI_WEST_STONE")]
		public static async Task RUNECASTER_SIAULIAI_WEST_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_MANAGER")]
		public static async Task SIAUL_EAST_MANAGER(Dialog dialog)
		{
			await dialog.Msg("SIAUL_EAST_MANAGER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SOLDIER6")]
		public static async Task SIAUL_EAST_SOLDIER6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SOLDIER7")]
		public static async Task SIAUL_EAST_SOLDIER7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SOLDIER4")]
		public static async Task SIAUL_EAST_SOLDIER4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SOLDIER5")]
		public static async Task SIAUL_EAST_SOLDIER5(Dialog dialog)
		{
			await dialog.Msg("SIAUL_EAST_SOLDIER5_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SUPPLY_MANAGER")]
		public static async Task SIAUL_EAST_SUPPLY_MANAGER(Dialog dialog)
		{
			await dialog.Msg("SIAUL_EAST_SUPPLY_MANAGER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SOLDIER8")]
		public static async Task SIAUL_EAST_SOLDIER8(Dialog dialog)
		{
			await dialog.Msg("SIAUL_EAST_SOLDIER8_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SOLDIER10")]
		public static async Task SIAUL_EAST_SOLDIER10(Dialog dialog)
		{
			await dialog.Msg("SIAUL_EAST_SOLDIER10_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAUL_EAST_SOLDIER9")]
		public static async Task SIAUL_EAST_SOLDIER9(Dialog dialog)
		{
			await dialog.Msg("SIAUL_EAST_SOLDIER9_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ACT2_DISS1_BOX")]
		public static async Task ACT2_DISS1_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_SIAULIAI_EST")]
		public static async Task WARP_F_SIAULIAI_EST(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("SIAUL_EAST_SUPPLY_MANAGER2")]
		public static async Task SIAUL_EAST_SUPPLY_MANAGER2(Dialog dialog)
		{
			await dialog.Msg("SIAUL_EAST_SUPPLY_MANAGER2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_SAPPER2_1_NPC")]
		public static async Task JOB_SAPPER2_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_SAPPER2_1_NPC_basic1");
			await dialog.Msg("JOB_SAPPER2_1_NPC_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAULIA_2_BOARD01")]
		public static async Task F_SIAULIA_2_BOARD01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_HUNTER2_1_NPC")]
		public static async Task JOB_HUNTER2_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_HUNTER2_1_NPC_basic1");
			await dialog.Msg("JOB_HUNTER2_1_NPC_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_BARBARIAN2_NPC")]
		public static async Task JOB_BARBARIAN2_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_BARBARIAN2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_210023")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_210023(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "Hat_628032", 1, "TreasureboxKey2");

		[DialogFunction("SAUI_EAST_GUARD_01")]
		public static async Task SAUI_EAST_GUARD_01(Dialog dialog)
		{
			await dialog.Msg("SAUI_EAST_GUARD_01_BASIC02");
			await dialog.Msg("SAUI_EAST_GUARD_01_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAUI_EAST_GUARD_02")]
		public static async Task SAUI_EAST_GUARD_02(Dialog dialog)
		{
			await dialog.Msg("SAUI_EAST_GUARD_02_BASIC");
			await dialog.Msg("SAUI_EAST_GUARD_02_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_210039")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_210039(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_119", 1);

		[DialogFunction("LOWLV_BOASTER_SQ_10_BOX")]
		public static async Task LOWLV_BOASTER_SQ_10_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task WARP_F_SIAULIAI_OUT(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("JOB_KRIWI1_OUT")]
		public static async Task JOB_KRIWI1_OUT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_2")]
		public static async Task SIAULIAIOUT_119_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_3")]
		public static async Task SIAULIAIOUT_119_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_4")]
		public static async Task SIAULIAIOUT_119_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_5")]
		public static async Task SIAULIAIOUT_119_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_6")]
		public static async Task SIAULIAIOUT_119_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_7")]
		public static async Task SIAULIAIOUT_119_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_8")]
		public static async Task SIAULIAIOUT_119_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_119_9")]
		public static async Task SIAULIAIOUT_119_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_HEALER_B")]
		public static async Task SIAULIAIOUT_HEALER_B(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_HEALER_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_HEALER_C")]
		public static async Task SIAULIAIOUT_HEALER_C(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_MINER_B")]
		public static async Task SIAULIAIOUT_MINER_B(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_MINER_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_MINER_A")]
		public static async Task SIAULIAIOUT_MINER_A(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_CHIEF_A")]
		public static async Task SIAULIAIOUT_CHIEF_A(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_CHIEF_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_ALCHE")]
		public static async Task SIAULIAIOUT_ALCHE(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_ALCHE_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_HUNTER")]
		public static async Task SIAULIAIOUT_HUNTER(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_HUNTER_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_STONE")]
		public static async Task SIAULIAIOUT_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_ALCHE_A")]
		public static async Task SIAULIAIOUT_ALCHE_A(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_ALCHE_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_BLOCK")]
		public static async Task SIAULIAIOUT_BLOCK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAULIA_OUT_BOARD01")]
		public static async Task F_SIAULIA_OUT_BOARD01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAU_OUT_NPC01")]
		public static async Task F_SIAU_OUT_NPC01(Dialog dialog)
		{
			await dialog.Msg("F_SIAU_OUT_NPC01_basic01");
			await dialog.Msg("F_SIAU_OUT_NPC01_basic02");
			await dialog.Msg("F_SIAU_OUT_NPC01_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAU_OUT_NPC02")]
		public static async Task F_SIAU_OUT_NPC02(Dialog dialog)
		{
			await dialog.Msg("F_SIAU_OUT_NPC02_basic01");
			await dialog.Msg("F_SIAU_OUT_NPC02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAU_OUT_NPC03")]
		public static async Task F_SIAU_OUT_NPC03(Dialog dialog)
		{
			await dialog.Msg("F_SIAU_OUT_NPC03_basic01");
			await dialog.Msg("F_SIAU_OUT_NPC03_basic02");
			await dialog.Msg("F_SIAU_OUT_NPC03_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAULIAI_OUT_BOARD02")]
		public static async Task F_SIAULIAI_OUT_BOARD02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PSYCHOKINESIST2_1_NPC")]
		public static async Task JOB_PSYCHOKINESIST2_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_PSYCHOKINESIST2_1_NPC_basic1");
			await dialog.Msg("JOB_PSYCHOKINESIST2_1_NPC_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_LINKER2_1_NPC")]
		public static async Task JOB_LINKER2_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_LINKER2_1_NPC_basic1");
			await dialog.Msg("JOB_LINKER2_1_NPC_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_BUBE")]
		public static async Task TREASUREBOX_BUBE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAULIAI_OUT_EV_55_001")]
		public static async Task F_SIAULIAI_OUT_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("SOUT_REFUGEE01")]
		public static async Task SOUT_REFUGEE01(Dialog dialog)
		{
			await dialog.Msg("SOUT_REFUGEE01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SOUT_REFUGEE01_1")]
		public static async Task SOUT_REFUGEE01_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SOUT_REFUGEE02_1")]
		public static async Task SOUT_REFUGEE02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SOUT_REFUGEE02")]
		public static async Task SOUT_REFUGEE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_SIAULIAI_OUT_BOARD03")]
		public static async Task F_SIAULIAI_OUT_BOARD03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_MINER_C")]
		public static async Task SIAULIAIOUT_MINER_C(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SOUT_PHARMACY")]
		public static async Task SOUT_PHARMACY(Dialog dialog)
		{
			await dialog.Msg("SOUT_PHARMACY_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_SOLDIRE_SQ31")]
		public static async Task SIAULIAIOUT_SOLDIRE_SQ31(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_SOLDIRE_SQ31_basic01");
			await dialog.Msg("SIAULIAIOUT_SOLDIRE_SQ31_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAIOUT_SOLDIRE_SQ32")]
		public static async Task SIAULIAIOUT_SOLDIRE_SQ32(Dialog dialog)
		{
			await dialog.Msg("SIAULIAIOUT_SOLDIRE_SQ32_basic01");
			await dialog.Msg("SIAULIAIOUT_SOLDIRE_SQ32_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAU_HUAN_MNT")]
		public static async Task SIAU_HUAN_MNT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_OUT10054")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_OUT10054(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_120", 1);

		[DialogFunction("MINE_1_ALCHEMIST")]
		public static async Task MINE_1_ALCHEMIST(Dialog dialog)
		{
			await dialog.Msg("NoneMINE_1_ALCHEMIST_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_1_PURIFY_1")]
		public static async Task MINE_1_PURIFY_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_1_PURIFY_7")]
		public static async Task MINE_1_PURIFY_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_1_CRYSTAL_10")]
		public static async Task MINE_1_CRYSTAL_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_1_PURIFY_5")]
		public static async Task MINE_1_PURIFY_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_CMINE_01")]
		public static async Task WARP_D_CMINE_01(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_01533")]
		public static async Task TREASUREBOX_LV_D_CMINE_01533(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "NECK02_114", 1, "TreasureboxKey2");

		[DialogFunction("MINE_1_CRYSTAL_4")]
		public static async Task MINE_1_CRYSTAL_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_1_CRYSTAL_9_DEVICE")]
		public static async Task MINE_1_CRYSTAL_9_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CMINE_01541")]
		public static async Task TREASUREBOX_LV_D_CMINE_01541(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "gem_circle_1", 1);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_01542")]
		public static async Task TREASUREBOX_LV_D_CMINE_01542(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "gem_square_1", 1);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_01543")]
		public static async Task TREASUREBOX_LV_D_CMINE_01543(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_121", 1);

		[DialogFunction("MINE_2_PURIFY_1")]
		public static async Task MINE_2_PURIFY_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_PURIFY_3")]
		public static async Task MINE_2_PURIFY_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_PURIFY_7")]
		public static async Task MINE_2_PURIFY_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_CMINE_02")]
		public static async Task WARP_D_CMINE_02(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("BOOKSHELF_02")]
		public static async Task BOOKSHELF_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CMINE_02_HIDEMAN")]
		public static async Task CMINE_02_HIDEMAN(Dialog dialog)
		{
			await dialog.Msg("CMINE_02_HIDEMAN_basic01");
			await dialog.Msg("CMINE_02_HIDEMAN_basic02");
			await dialog.Msg("CMINE_02_HIDEMAN_basic03");
			await dialog.Msg("CMINE_02_HIDEMAN_basic04");
			await dialog.Msg("CMINE_02_HIDEMAN_basic05");
			await dialog.Msg("CMINE_02_HIDEMAN_basic06");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CMINE_FOOD_01")]
		public static async Task CMINE_FOOD_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CMINE_FOOD_04")]
		public static async Task CMINE_FOOD_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CMINE_FOOD_03")]
		public static async Task CMINE_FOOD_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_02_BOARD01")]
		public static async Task D_CMINE_02_BOARD01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_CRYSTAL_2_PIPE")]
		public static async Task MINE_2_CRYSTAL_2_PIPE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_CRYSTAL_3_PIPE")]
		public static async Task MINE_2_CRYSTAL_3_PIPE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_CRYSTAL_7_ENERGY")]
		public static async Task MINE_2_CRYSTAL_7_ENERGY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_CRYSTAL_10_OIL")]
		public static async Task MINE_2_CRYSTAL_10_OIL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_CRYSTAL_16_PART")]
		public static async Task MINE_2_CRYSTAL_16_PART(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_2_CRYSTAL_20_PART")]
		public static async Task MINE_2_CRYSTAL_20_PART(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CMINE_02550")]
		public static async Task TREASUREBOX_LV_D_CMINE_02550(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "gem_star_1", 1);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_02551")]
		public static async Task TREASUREBOX_LV_D_CMINE_02551(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "gem_diamond_1", 1);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_02553")]
		public static async Task TREASUREBOX_LV_D_CMINE_02553(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_122", 1);

		[DialogFunction("CMINE3_BOSSROOM_OPEN")]
		public static async Task CMINE3_BOSSROOM_OPEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_3_ALCHEMIST")]
		public static async Task MINE_3_ALCHEMIST(Dialog dialog)
		{
			await dialog.Msg("MINE_3_ALCHEMIST_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_3_RESQUE3")]
		public static async Task MINE_3_RESQUE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_3_RESIENT1_BIND")]
		public static async Task MINE_3_RESIENT1_BIND(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_3_GIRL_BIND")]
		public static async Task MINE_3_GIRL_BIND(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CMINE6_TO_KATYN7_1_START")]
		public static async Task CMINE6_TO_KATYN7_1_START(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_NPC01_BIND")]
		public static async Task D_CMINE_NPC01_BIND(Dialog dialog)
		{
			await dialog.Msg("D_CMINE_NPC01_BIND_basic01");
			await dialog.Msg("D_CMINE_NPC01_BIND_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_NPC02_BIND")]
		public static async Task D_CMINE_NPC02_BIND(Dialog dialog)
		{
			await dialog.Msg("D_CMINE_NPC02_BIND_basic01");
			await dialog.Msg("D_CMINE_NPC02_BIND_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_CMINE_6")]
		public static async Task WARP_D_CMINE_6(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_6530")]
		public static async Task TREASUREBOX_LV_D_CMINE_6530(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "emoticonItem_1", 1);

		[DialogFunction("MINE_3_RESIENT1")]
		public static async Task MINE_3_RESIENT1(Dialog dialog)
		{
			await dialog.Msg("MINE_3_RESIENT1_BASIC01");
			await dialog.Msg("MINE_3_RESIENT1_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MINE_3_GIRL")]
		public static async Task MINE_3_GIRL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_NPC01")]
		public static async Task D_CMINE_NPC01(Dialog dialog)
		{
			await dialog.Msg("D_CMINE_NPC01_basic01");
			await dialog.Msg("D_CMINE_NPC01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_NPC02")]
		public static async Task D_CMINE_NPC02(Dialog dialog)
		{
			await dialog.Msg("D_CMINE_NPC02_basic01");
			await dialog.Msg("D_CMINE_NPC02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CMINE_6540")]
		public static async Task TREASUREBOX_LV_D_CMINE_6540(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_123", 1);

		[DialogFunction("CMINE6_RP_1_OBJ")]
		public static async Task CMINE6_RP_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CMINE6_RP_1_NPC")]
		public static async Task CMINE6_RP_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_CMINE_6_1")]
		public static async Task HIDDEN_MIKO_CMINE_6_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_CMINE_6_2")]
		public static async Task HIDDEN_MIKO_CMINE_6_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_CMINE_6_3")]
		public static async Task HIDDEN_MIKO_CMINE_6_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_CMINE_6_4")]
		public static async Task HIDDEN_MIKO_CMINE_6_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_CMINE_6_5")]
		public static async Task HIDDEN_MIKO_CMINE_6_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_CMINE_6_6")]
		public static async Task HIDDEN_MIKO_CMINE_6_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_CMINE_6_7")]
		public static async Task HIDDEN_MIKO_CMINE_6_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE571_NPC_GILBERT")]
		public static async Task GELE571_NPC_GILBERT(Dialog dialog)
		{
			await dialog.Msg("GELE571_NPC_GILBERT_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE571_MQ_03")]
		public static async Task GELE571_MQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_GELE_57_1")]
		public static async Task WARP_F_GELE_57_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("GELE571_NPC_MATTHEW")]
		public static async Task GELE571_NPC_MATTHEW(Dialog dialog)
		{
			await dialog.Msg("GELE571_NPC_MATTHEW_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE571_NPC_MARLEY")]
		public static async Task GELE571_NPC_MARLEY(Dialog dialog)
		{
			await dialog.Msg("GELE571_NPC_MARLEY_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE571_NPC_PANTO")]
		public static async Task GELE571_NPC_PANTO(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE1_BOARD01")]
		public static async Task GELE1_BOARD01(Dialog dialog)
		{
			await dialog.Msg("GELE1_BOARD01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE571_MQ_05")]
		public static async Task GELE571_MQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE571_MQ_07")]
		public static async Task GELE571_MQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE_57_1_CABLETEMP_01")]
		public static async Task GELE_57_1_CABLETEMP_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE_57_1_CABLETEMP_02")]
		public static async Task GELE_57_1_CABLETEMP_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q3_GELE01")]
		public static async Task PARTY_Q3_GELE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q3_GELE02")]
		public static async Task PARTY_Q3_GELE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_GELE_57_1158")]
		public static async Task TREASUREBOX_LV_F_GELE_57_1158(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_124", 1);

		[DialogFunction("GELE571_RP_1_OBJ")]
		public static async Task GELE571_RP_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE572_NPC_BASIL")]
		public static async Task GELE572_NPC_BASIL(Dialog dialog)
		{
			await dialog.Msg("GELE572_NPC_BASIL_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE572_MQ_07")]
		public static async Task GELE572_MQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE572_NPC_MORI")]
		public static async Task GELE572_NPC_MORI(Dialog dialog)
		{
			await dialog.Msg("GELE572_NPC_MORI_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE572_MQ_05")]
		public static async Task GELE572_MQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE572_MQ_09")]
		public static async Task GELE572_MQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE_57_2_CABLETEMP_01")]
		public static async Task GELE_57_2_CABLETEMP_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE_57_2_CABLETEMP_02")]
		public static async Task GELE_57_2_CABLETEMP_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_GELE_57_2150")]
		public static async Task TREASUREBOX_LV_F_GELE_57_2150(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "Hat_628099", 1, "TreasureboxKey4");

		[DialogFunction("TREASUREBOX_LV_F_GELE_57_2153")]
		public static async Task TREASUREBOX_LV_F_GELE_57_2153(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_125", 1);

		[DialogFunction("GELE573_MQ_04")]
		public static async Task GELE573_MQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE573_MQ_01")]
		public static async Task GELE573_MQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE573_ALLEN")]
		public static async Task GELE573_ALLEN(Dialog dialog)
		{
			await dialog.Msg("GELE573_ALLEN_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE573_KAROLINA")]
		public static async Task GELE573_KAROLINA(Dialog dialog)
		{
			await dialog.Msg("GELE573_KAROLINA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE573_MASTER")]
		public static async Task GELE573_MASTER(Dialog dialog)
		{
			await dialog.Msg("GELE573_MASTER_BASIC04");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE573_MQ_03_AI_KILL")]
		public static async Task GELE573_MQ_03_AI_KILL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE_57_3_HQ01_NPC01")]
		public static async Task GELE_57_3_HQ01_NPC01(Dialog dialog)
		{
			await dialog.Msg("GELE_57_3_HQ01_NPC01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_GELE_57_3")]
		public static async Task WARP_F_GELE_57_3(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("GELE573_KENNETH")]
		public static async Task GELE573_KENNETH(Dialog dialog)
		{
			await dialog.Msg("GELE573_KENNETH_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE573_MQ_07_F")]
		public static async Task GELE573_MQ_07_F(Dialog dialog)
		{
			await dialog.Msg("GELE573_MQ_07_F_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q3_GELE03")]
		public static async Task PARTY_Q3_GELE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q3_GELE04")]
		public static async Task PARTY_Q3_GELE04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_GELE_57_340")]
		public static async Task TREASUREBOX_LV_F_GELE_57_340(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_126", 1);

		[DialogFunction("GELE574_ADRIJA")]
		public static async Task GELE574_ADRIJA(Dialog dialog)
		{
			await dialog.Msg("GELE574_ADRIJA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_MQ_04")]
		public static async Task GELE574_MQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_MQ_07_SHRINE")]
		public static async Task GELE574_MQ_07_SHRINE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_ARUNE_1")]
		public static async Task GELE574_ARUNE_1(Dialog dialog)
		{
			await dialog.Msg("GELE574_ARUNE_1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_MQ_07_PLACE")]
		public static async Task GELE574_MQ_07_PLACE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_MQ_08")]
		public static async Task GELE574_MQ_08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_REIKE")]
		public static async Task GELE574_REIKE(Dialog dialog)
		{
			await dialog.Msg("GELE574_REIKE_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_ERRA")]
		public static async Task GELE574_ERRA(Dialog dialog)
		{
			await dialog.Msg("GELE574_ERRA_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_ALLGES")]
		public static async Task GELE574_ALLGES(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_MQ_01")]
		public static async Task GELE574_MQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GELE574_MQ_06")]
		public static async Task GELE574_MQ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_GELE_57_4")]
		public static async Task WARP_F_GELE_57_4(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_GELE_57_451")]
		public static async Task TREASUREBOX_LV_F_GELE_57_451(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "R_Hat_628043", 1, "TreasureboxKey2");

		[DialogFunction("UPPER_WARNING_F_GELE_57_4")]
		public static async Task UPPER_WARNING_F_GELE_57_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_GELE_57_460")]
		public static async Task TREASUREBOX_LV_F_GELE_57_460(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_127", 1);

		[DialogFunction("CHAR120_MASTER")]
		public static async Task CHAR120_MASTER(Dialog dialog)
		{
			await dialog.Msg("CHAR120_MASTER_basic1");
			await dialog.Msg("CHAR120_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPEL_TOMAS")]
		public static async Task CHAPEL_TOMAS(Dialog dialog)
		{
			await dialog.Msg("CHAPEL_TOMAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPEL_TABERIJUS")]
		public static async Task CHAPEL_TABERIJUS(Dialog dialog)
		{
			await dialog.Msg("CHAPEL_TABERIJUS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPEL_VIDAS")]
		public static async Task CHAPEL_VIDAS(Dialog dialog)
		{
			await dialog.Msg("CHAPEL_VIDAS_basic_01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE575_MQ_09")]
		public static async Task CHAPLE575_MQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_CHAPEL_57_5")]
		public static async Task WARP_D_CHAPEL_57_5(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("CHAPLE575_MQ_05")]
		public static async Task CHAPLE575_MQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CHAPEL_57_533")]
		public static async Task TREASUREBOX_LV_D_CHAPEL_57_533(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_132", 1);

		[DialogFunction("CHAPEL_VIRGINIJA")]
		public static async Task CHAPEL_VIRGINIJA(Dialog dialog)
		{
			await dialog.Msg("CHAPEL_VIRGINIJA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPEL576_DONATAS")]
		public static async Task CHAPEL576_DONATAS(Dialog dialog)
		{
			await dialog.Msg("CHAPEL576_DONATAS_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE576_MQ_09")]
		public static async Task CHAPLE576_MQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPEL576_NORTH")]
		public static async Task CHAPEL576_NORTH(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE576_MQ_04")]
		public static async Task CHAPLE576_MQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_CHAPEL_57_6")]
		public static async Task WARP_D_CHAPEL_57_6(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_CHAPEL_57_630")]
		public static async Task TREASUREBOX_LV_D_CHAPEL_57_630(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_133", 1);

		[DialogFunction("CHAPLE576_RP_1_OBJ")]
		public static async Task CHAPLE576_RP_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_ARUNE_02")]
		public static async Task CHAPLE577_ARUNE_02(Dialog dialog)
		{
			await dialog.Msg("CHAPLE577_ARUNE_02_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_HOLY_1")]
		public static async Task CHAPLE577_HOLY_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_HOLY_2")]
		public static async Task CHAPLE577_HOLY_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_HOLY_3")]
		public static async Task CHAPLE577_HOLY_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_ARUNE_01")]
		public static async Task CHAPLE577_ARUNE_01(Dialog dialog)
		{
			await dialog.Msg("CHAPLE577_ARUNE_01_basic01");
			await dialog.Msg("CHAPLE577_ARUNE_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_03")]
		public static async Task CHAPLE577_MQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_10")]
		public static async Task CHAPLE577_MQ_10(Dialog dialog)
		{
			await dialog.Msg("d_chapel_57_7_dlg_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_1")]
		public static async Task CHAPLE577_MQ_04_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_2")]
		public static async Task CHAPLE577_MQ_04_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_3")]
		public static async Task CHAPLE577_MQ_04_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_4")]
		public static async Task CHAPLE577_MQ_04_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_5")]
		public static async Task CHAPLE577_MQ_04_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_6")]
		public static async Task CHAPLE577_MQ_04_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_7")]
		public static async Task CHAPLE577_MQ_04_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAPLE577_MQ_04_8")]
		public static async Task CHAPLE577_MQ_04_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SANCTUM_BOOK_NO")]
		public static async Task SANCTUM_BOOK_NO(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SANCTUM_BOOK_OK")]
		public static async Task SANCTUM_BOOK_OK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CHAPEL_57_760")]
		public static async Task TREASUREBOX_LV_D_CHAPEL_57_760(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_134", 1);

		[DialogFunction("HUEVILLAGE_58_1_MQ01_NPC")]
		public static async Task HUEVILLAGE_58_1_MQ01_NPC(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE_58_1_MQ01_NPC_basic01");
			await dialog.Msg("HUEVILLAGE_58_1_MQ01_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_1_MQ02_NPC")]
		public static async Task HUEVILLAGE_58_1_MQ02_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_1_MQ03_NPC")]
		public static async Task HUEVILLAGE_58_1_MQ03_NPC(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE_58_1_MQ03_NPC_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_1_PORTAL")]
		public static async Task HUEVILLAGE_58_1_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_HUEVILLAGE_58_1")]
		public static async Task WARP_F_HUEVILLAGE_58_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("HUEVILLAGE_58_1_SQ02_NPC")]
		public static async Task HUEVILLAGE_58_1_SQ02_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_1_CABLETEMP_01")]
		public static async Task HUEVILLAGE_58_1_CABLETEMP_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_1_CABLETEMP_02")]
		public static async Task HUEVILLAGE_58_1_CABLETEMP_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KLAIPEDA_MAYER")]
		public static async Task KLAIPEDA_MAYER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q4_TRIGGER")]
		public static async Task PARTY_Q4_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q41_ORB")]
		public static async Task PARTY_Q41_ORB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_HUEVILLAGE_58_150")]
		public static async Task TREASUREBOX_LV_F_HUEVILLAGE_58_150(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_128", 1);

		[DialogFunction("HUEVILLAGE_58_2_MQ01_NPC")]
		public static async Task HUEVILLAGE_58_2_MQ01_NPC(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE58_2_CHIEF_basic01");
			await dialog.Msg("HUEVILLAGE58_2_CHIEF_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_2_MQ02_NPC")]
		public static async Task HUEVILLAGE_58_2_MQ02_NPC(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE58_2_PEAPLE_basic01");
			await dialog.Msg("HUEVILLAGE58_2_PEAPLE_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_2_SQ01_NPC")]
		public static async Task HUEVILLAGE_58_2_SQ01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_2_MQ02_BUCKET01")]
		public static async Task HUEVILLAGE_58_2_MQ02_BUCKET01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_2_MQ02_BUCKET02")]
		public static async Task HUEVILLAGE_58_2_MQ02_BUCKET02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_2_MQ03_NPC")]
		public static async Task HUEVILLAGE_58_2_MQ03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_2_OBELISK_BEFORE")]
		public static async Task HUEVILLAGE_58_2_OBELISK_BEFORE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_2_STORN01")]
		public static async Task HUEVILLAGE_58_2_STORN01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_HUEVILLAGE_58_2")]
		public static async Task WARP_F_HUEVILLAGE_58_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_HUEVILLAGE_58_248")]
		public static async Task TREASUREBOX_LV_F_HUEVILLAGE_58_248(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "JOB_SHINOBI_HIDDEN_ITEM_1", 1);

		[DialogFunction("TREASUREBOX_LV_F_HUEVILLAGE_58_249")]
		public static async Task TREASUREBOX_LV_F_HUEVILLAGE_58_249(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_129", 1);

		[DialogFunction("HUEVILLAGE_58_3_MQ01_NPC")]
		public static async Task HUEVILLAGE_58_3_MQ01_NPC(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE_58_3_MQ02_basic01");
			await dialog.Msg("HUEVILLAGE_58_3_MQ02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_MQ03_NPC")]
		public static async Task HUEVILLAGE_58_3_MQ03_NPC(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE_58_3_MQ04_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUVILLAGE_58_3_WELL_WARING")]
		public static async Task HUVILLAGE_58_3_WELL_WARING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_MQ04_NPC01")]
		public static async Task HUEVILLAGE_58_3_MQ04_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_MQ01_GRASS")]
		public static async Task HUEVILLAGE_58_3_MQ01_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_MQ02_FLOWER")]
		public static async Task HUEVILLAGE_58_3_MQ02_FLOWER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_MQ03_DRUM_FAKE")]
		public static async Task HUEVILLAGE_58_3_MQ03_DRUM_FAKE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_MQ03_DRUM")]
		public static async Task HUEVILLAGE_58_3_MQ03_DRUM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_SQ01_NPC01")]
		public static async Task HUEVILLAGE_58_3_SQ01_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_SQ01_NPC02")]
		public static async Task HUEVILLAGE_58_3_SQ01_NPC02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_SQ01_NPC03")]
		public static async Task HUEVILLAGE_58_3_SQ01_NPC03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_HUEVILLAGE_58_351")]
		public static async Task TREASUREBOX_LV_F_HUEVILLAGE_58_351(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_130", 1);

		[DialogFunction("HUEVILLAGE_58_4_MQ01_NPC01")]
		public static async Task HUEVILLAGE_58_4_MQ01_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_SAULE_BEFORE")]
		public static async Task HUEVILLAGE_58_4_SAULE_BEFORE(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE_58_4_SAULE_BEFORE_basic01");
			await dialog.Msg("HUEVILLAGE_58_4_SAULE_BEFORE_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_SAULE_AFTER")]
		public static async Task HUEVILLAGE_58_4_SAULE_AFTER(Dialog dialog)
		{
			await dialog.Msg("HUEVILLAGE_58_4_SAULE_AFTER_basic01");
			await dialog.Msg("HUEVILLAGE_58_4_SAULE_AFTER_basic02");
			await dialog.Msg("HUEVILLAGE_58_4_SAULE_AFTER_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ03_NPC01")]
		public static async Task HUEVILLAGE_58_4_MQ03_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ04_NPC01")]
		public static async Task HUEVILLAGE_58_4_MQ04_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ05_NPC01")]
		public static async Task HUEVILLAGE_58_4_MQ05_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ05_NPC02")]
		public static async Task HUEVILLAGE_58_4_MQ05_NPC02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ05_NPC03")]
		public static async Task HUEVILLAGE_58_4_MQ05_NPC03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ05_NPC04")]
		public static async Task HUEVILLAGE_58_4_MQ05_NPC04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ05_NPC05")]
		public static async Task HUEVILLAGE_58_4_MQ05_NPC05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_MQ07_NPC01")]
		public static async Task HUEVILLAGE_58_4_MQ07_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_4_PORTAL_AFTER")]
		public static async Task HUEVILLAGE_58_4_PORTAL_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q06_THORN")]
		public static async Task PARTY_Q06_THORN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_HUEVILLAGE_58_4")]
		public static async Task WARP_F_HUEVILLAGE_58_4(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_HUEVILLAGE_58_445")]
		public static async Task TREASUREBOX_LV_F_HUEVILLAGE_58_445(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_131", 1);

		[DialogFunction("LIMESTONE_52_4_MQ_9_ORB")]
		public static async Task LIMESTONE_52_4_MQ_9_ORB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN7_KEYNPC_1")]
		public static async Task KATYN7_KEYNPC_1(Dialog dialog)
		{
			await dialog.Msg("KATYN7_KEYNPC_1_basic1");
			await dialog.Msg("KATYN7_KEYNPC_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN7_KEYNPC_3")]
		public static async Task KATYN7_KEYNPC_3(Dialog dialog)
		{
			await dialog.Msg("KATYN7_KEYNPC_3_basic1");
			await dialog.Msg("KATYN7_KEYNPC_3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN7_KEYNPC_4")]
		public static async Task KATYN7_KEYNPC_4(Dialog dialog)
		{
			await dialog.Msg("KATYN7_KEYNPC_4_basic1");
			await dialog.Msg("KATYN7_KEYNPC_4_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_SUCH_POINT_01")]
		public static async Task KATYN_SUCH_POINT_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_SUCH_POINT_02")]
		public static async Task KATYN_SUCH_POINT_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_SUCH_POINT_03")]
		public static async Task KATYN_SUCH_POINT_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_SUCH_POINT_04")]
		public static async Task KATYN_SUCH_POINT_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_SUCH_POINT_05")]
		public static async Task KATYN_SUCH_POINT_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_SUCH_POINT_01_SOLDIER")]
		public static async Task KATYN_SUCH_POINT_01_SOLDIER(Dialog dialog)
		{
			await dialog.Msg("KATYN_SUCH_POINT_01_SOLDIER_basic01");
			await dialog.Msg("KATYN_SUCH_POINT_01_SOLDIER_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN71_OWL_01")]
		public static async Task KATYN71_OWL_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN71_OFFICER")]
		public static async Task KATYN71_OFFICER(Dialog dialog)
		{
			await dialog.Msg("KATYN71_OFFICER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN71_OFFICER_AFTER")]
		public static async Task KATYN71_OFFICER_AFTER(Dialog dialog)
		{
			await dialog.Msg("KATYN71_OFFICER_AFTER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_KATYN_7_BOARD02")]
		public static async Task F_KATYN_7_BOARD02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN7_MQ06_TRACK")]
		public static async Task KATYN7_MQ06_TRACK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN71_SOLDIER01_1")]
		public static async Task KATYN71_SOLDIER01_1(Dialog dialog)
		{
			await dialog.Msg("KATYN71_SOLDIER01_2_basic01");
			await dialog.Msg("KATYN71_SOLDIER01_2_basic02");
			await dialog.Msg("KATYN71_SOLDIER01_2_basic03");
			await dialog.Msg("KATYN71_SOLDIER01_2_basic04");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN71_BOSS")]
		public static async Task KATYN71_BOSS(Dialog dialog)
		{
			await dialog.Msg("KATYN71_BOSS_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN71_SOLDIER01_2")]
		public static async Task KATYN71_SOLDIER01_2(Dialog dialog)
		{
			await dialog.Msg("KATYN71_SOLDIER01_2_basic01");
			await dialog.Msg("KATYN71_SOLDIER01_2_basic02");
			await dialog.Msg("KATYN71_SOLDIER01_2_basic03");
			await dialog.Msg("KATYN71_SOLDIER01_2_basic04");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN7_KEYNPC_5")]
		public static async Task KATYN7_KEYNPC_5(Dialog dialog)
		{
			await dialog.Msg("KATYN7_KEYNPC_5_basic01");
			await dialog.Msg("KATYN7_KEYNPC_5_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_KATYN_710025")]
		public static async Task TREASUREBOX_LV_F_KATYN_710025(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_166", 1);

		[DialogFunction("KATYN72_SECTOR_01")]
		public static async Task KATYN72_SECTOR_01(Dialog dialog)
		{

			await dialog.Msg("KATYN72_SECTOR_01_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_SECTOR_02")]
		public static async Task KATYN72_SECTOR_02(Dialog dialog)
		{

			await dialog.Msg("KATYN72_SECTOR_02_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_SECTOR_03")]
		public static async Task KATYN72_SECTOR_03(Dialog dialog)
		{

			await dialog.Msg("KATYN72_SECTOR_03_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_SECTOR_04")]
		public static async Task KATYN72_SECTOR_04(Dialog dialog)
		{

			await dialog.Msg("KATYN72_SECTOR_04_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_BOSS")]
		public static async Task KATYN72_BOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_CORPSE")]
		public static async Task KATYN72_CORPSE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_RASSFLY")]
		public static async Task KATYN72_RASSFLY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_TIED")]
		public static async Task KATYN72_TIED(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_GHOST")]
		public static async Task KATYN72_GHOST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_KATYN_7_2_HIDDENNPC_BOARD")]
		public static async Task F_KATYN_7_2_HIDDENNPC_BOARD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_7_2_HQ01_NPC02")]
		public static async Task KATYN_7_2_HQ01_NPC02(Dialog dialog)
		{
			await dialog.Msg("KATYN_7_2_HQ01_NPC02_basic01");
			await dialog.Msg("KATYN_7_2_HQ01_NPC02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_KATYN_7_2")]
		public static async Task WARP_F_KATYN_7_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("KATYN7_2_FALL")]
		public static async Task KATYN7_2_FALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN72_GOLEMLET")]
		public static async Task KATYN72_GOLEMLET(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN7_2_HIDDEN_EVENT")]
		public static async Task KATYN7_2_HIDDEN_EVENT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_KATYN_7_210029")]
		public static async Task TREASUREBOX_LV_F_KATYN_7_210029(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_167", 1);

		[DialogFunction("KLAIPE_HQ_02_NPC")]
		public static async Task KLAIPE_HQ_02_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KLAIPE_HQ_01_NPC_D")]
		public static async Task KLAIPE_HQ_01_NPC_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CMINE_81078")]
		public static async Task TREASUREBOX_LV_D_CMINE_81078(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "R_Hat_628060", 1, "TreasureboxKey3");

		[DialogFunction("CRYSTAL_MACHINE")]
		public static async Task CRYSTAL_MACHINE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_8_BAG")]
		public static async Task D_CMINE_8_BAG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_8_JUKEBOX")]
		public static async Task D_CMINE_8_JUKEBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q_011_CHAMICAL01")]
		public static async Task PARTY_Q_011_CHAMICAL01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q_011_MACHIN01")]
		public static async Task PARTY_Q_011_MACHIN01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CMINE_81109")]
		public static async Task TREASUREBOX_LV_D_CMINE_81109(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "R_SPR02_105", 1, "TreasureboxKey2");

		[DialogFunction("TREASUREBOX_LV_D_CMINE_8700")]
		public static async Task TREASUREBOX_LV_D_CMINE_8700(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_164", 1);

		[DialogFunction("D_CMINE_9_RED")]
		public static async Task D_CMINE_9_RED(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_9_BLUE")]
		public static async Task D_CMINE_9_BLUE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_9_YELLOW")]
		public static async Task D_CMINE_9_YELLOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CMINE_9_WARNING")]
		public static async Task D_CMINE_9_WARNING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CMINE_91054")]
		public static async Task TREASUREBOX_LV_D_CMINE_91054(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_165", 1);

		[DialogFunction("KATYN13_1_OWLBOSS")]
		public static async Task KATYN13_1_OWLBOSS(Dialog dialog)
		{
			await dialog.Msg("KATYN13_1_OWLBOSS_basic2");
			await dialog.Msg("KATYN13_1_OWLBOSS_basic4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_1_KEYNPC")]
		public static async Task KATYN13_1_KEYNPC(Dialog dialog)
		{
			await dialog.Msg("KATYN13_1_KEYNPC_basic1");
			await dialog.Msg("KATYN13_1_KEYNPC_basic2");
			await dialog.Msg("KATYN13_1_KEYNPC_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_1_OWLJUNIOR1")]
		public static async Task KATYN13_1_OWLJUNIOR1(Dialog dialog)
		{
			await dialog.Msg("KATYN13_1_OWLJUNIOR1_basic1");
			await dialog.Msg("KATYN13_1_OWLJUNIOR1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_1_OWLJUNIOR2")]
		public static async Task KATYN13_1_OWLJUNIOR2(Dialog dialog)
		{
			await dialog.Msg("KATYN13_1_OWLJUNIOR2_basic1");
			await dialog.Msg("KATYN13_1_OWLJUNIOR2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_1_OWLJUNIOR3")]
		public static async Task KATYN13_1_OWLJUNIOR3(Dialog dialog)
		{
			await dialog.Msg("KATYN13_1_OWLJUNIOR3_basic1");
			await dialog.Msg("KATYN13_1_OWLJUNIOR3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_ADDQUEST1_NPC")]
		public static async Task KATYN13_ADDQUEST1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_ADDQUEST2_NPC")]
		public static async Task KATYN13_ADDQUEST2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_ADDQUEST3_NPC")]
		public static async Task KATYN13_ADDQUEST3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_ADDQUEST4_NPC")]
		public static async Task KATYN13_ADDQUEST4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_ADDQUEST6_NPC")]
		public static async Task KATYN13_ADDQUEST6_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_ADDQUEST7_NPC")]
		public static async Task KATYN13_ADDQUEST7_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_ADDQUEST8_NPC")]
		public static async Task KATYN13_ADDQUEST8_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_1_OWLJUNIOR3_AFTER")]
		public static async Task KATYN13_1_OWLJUNIOR3_AFTER(Dialog dialog)
		{
			await dialog.Msg("KATYN13_1_OWLJUNIOR3_AFTER_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_1_TO_OWLJUNIOR3_S1_BOX")]
		public static async Task KATYN13_1_TO_OWLJUNIOR3_S1_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN13_1_KEY_SUB1_TRUFFLE")]
		public static async Task KATYN13_1_KEY_SUB1_TRUFFLE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q7_DEVICE")]
		public static async Task PARTY_Q7_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_KATYN_13773")]
		public static async Task TREASUREBOX_LV_F_KATYN_13773(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_168", 1);

		[DialogFunction("CHAR120_MSTEP6_1_NPC1")]
		public static async Task CHAR120_MSTEP6_1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP6_1_NPC2")]
		public static async Task CHAR120_MSTEP6_1_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP6_1_NPC3")]
		public static async Task CHAR120_MSTEP6_1_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP5_2_NPC3")]
		public static async Task CHAR120_MSTEP5_2_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP5_NPC2")]
		public static async Task CHAR120_MSTEP5_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP5_NPC1")]
		public static async Task CHAR120_MSTEP5_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_LAIMUNAS")]
		public static async Task KATYN14_LAIMUNAS(Dialog dialog)
		{
			await dialog.Msg("KATYN14_LAIMUNAS_basic1");
			await dialog.Msg("KATYN14_LAIMUNAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_LAIMUNAS_SOL")]
		public static async Task KATYN14_LAIMUNAS_SOL(Dialog dialog)
		{
			await dialog.Msg("KATYN14_LAIMUNAS_SOL_basic01");
			await dialog.Msg("KATYN14_LAIMUNAS_SOL_basic02");
			await dialog.Msg("KATYN14_LAIMUNAS_SOL_basic03");
			await dialog.Msg("KATYN14_LAIMUNAS_SOL_basic04");
			await dialog.Msg("KATYN14_LAIMUNAS_SOL_basic05");
			await dialog.Msg("KATYN14_LAIMUNAS_SOL_basic07");
			await dialog.Msg("KATYN14_LAIMUNAS_SOL_basic10");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_VACENIN")]
		public static async Task KATYN14_VACENIN(Dialog dialog)
		{
			await dialog.Msg("KATYN14_VACENIN_basic01");
			await dialog.Msg("KATYN14_VACENIN_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_SUPP")]
		public static async Task KATYN14_SUPP(Dialog dialog)
		{
			await dialog.Msg("KATYN14_SUPP_BASIC01");
			await dialog.Msg("KATYN14_SUPP_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_SUPP_SOL")]
		public static async Task KATYN14_SUPP_SOL(Dialog dialog)
		{
			await dialog.Msg("KATYN14_SUPP_SOL_basic01");
			await dialog.Msg("KATYN14_SUPP_SOL_basic03");
			await dialog.Msg("KATYN14_SUPP_SOL_basic04");
			await dialog.Msg("KATYN14_SUPP_SOL_basic05");
			await dialog.Msg("KATYN14_SUPP_SOL_basic06");
			await dialog.Msg("KATYN14_SUPP_SOL_basic10");
			await dialog.Msg("KATYN14_SUPP_SOL_basic11");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_ROY")]
		public static async Task KATYN14_ROY(Dialog dialog)
		{
			await dialog.Msg("KATYN14_ROY_basic1");
			await dialog.Msg("KATYN14_ROY_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_JOHN")]
		public static async Task KATYN14_JOHN(Dialog dialog)
		{

			await dialog.Msg("KATYN14_JOHN_basic1");
			await dialog.Msg("KATYN14_JOHN_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_JOHN_SOL")]
		public static async Task KATYN14_JOHN_SOL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_VACENIN_AFTER")]
		public static async Task KATYN14_VACENIN_AFTER(Dialog dialog)
		{
			await dialog.Msg("KATYN14_VACENIN_AFTER_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_VACENIN_CHASE")]
		public static async Task KATYN14_VACENIN_CHASE(Dialog dialog)
		{

			await dialog.Msg("KATYN14_VACENIN_CHASE_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_MQ_05_ITEM")]
		public static async Task KATYN14_MQ_05_ITEM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_PIL_SOL")]
		public static async Task KATYN14_PIL_SOL(Dialog dialog)
		{
			await dialog.Msg("KATYN14_VACENIN_SOL_basic15");
			await dialog.Msg("KATYN14_VACENIN_SOL_basic08");
			await dialog.Msg("KATYN14_VACENIN_SOL_basic01");
			await dialog.Msg("KATYN14_VACENIN_SOL_basic02");
			await dialog.Msg("KATYN14_VACENIN_SOL_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_SUB_WOLF")]
		public static async Task KATYN14_SUB_WOLF(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_SUB_08_STONE")]
		public static async Task KATYN14_SUB_08_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN14_SUB_02_MUSH")]
		public static async Task KATYN14_SUB_02_MUSH(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_KATYN_14_EV_55_001")]
		public static async Task F_KATYN_14_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("TREASUREBOX_LV_F_KATYN_14601")]
		public static async Task TREASUREBOX_LV_F_KATYN_14601(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_169", 1);

		[DialogFunction("HTA_KATYN_14_OWL")]
		public static async Task HTA_KATYN_14_OWL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_NPC_1")]
		public static async Task KATYN_18_RE_SQ_NPC_1(Dialog dialog)
		{
			await dialog.Msg("KATYN_18_RE_SQ_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_NPC_2")]
		public static async Task KATYN_18_RE_SQ_NPC_2(Dialog dialog)
		{
			await dialog.Msg("KATYN_18_RE_SQ_NPC_2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_NPC_3")]
		public static async Task KATYN_18_RE_SQ_NPC_3(Dialog dialog)
		{
			await dialog.Msg("KATYN_18_RE_SQ_NPC_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_1")]
		public static async Task KATYN_18_RE_SQ_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_2")]
		public static async Task KATYN_18_RE_SQ_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_3_DUMMY")]
		public static async Task KATYN_18_RE_SQ_OBJ_3_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_3_1")]
		public static async Task KATYN_18_RE_SQ_OBJ_3_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_3_2")]
		public static async Task KATYN_18_RE_SQ_OBJ_3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_3_3")]
		public static async Task KATYN_18_RE_SQ_OBJ_3_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_3")]
		public static async Task KATYN_18_RE_SQ_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_4")]
		public static async Task KATYN_18_RE_SQ_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_18_RE_SQ_OBJ_5")]
		public static async Task KATYN_18_RE_SQ_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_KATYN_181000")]
		public static async Task TREASUREBOX_LV_F_KATYN_181000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_295", 1);

		[DialogFunction("THORN19_HOLY01")]
		public static async Task THORN19_HOLY01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_BELIEVER")]
		public static async Task THORN19_BELIEVER(Dialog dialog)
		{
			await dialog.Msg("THORN19_BELIEVER_basic01");
			await dialog.Msg("THORN19_BELIEVER_basic02");
			await dialog.Msg("THORN19_BELIEVER_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_CHAFER_LURE")]
		public static async Task THORN19_CHAFER_LURE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_REFINE")]
		public static async Task THORN19_REFINE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_RECHARGE1")]
		public static async Task THORN19_RECHARGE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_RECHARGE2")]
		public static async Task THORN19_RECHARGE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_RECHARGE3")]
		public static async Task THORN19_RECHARGE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_RECHARGE4")]
		public static async Task THORN19_RECHARGE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_THORN_19")]
		public static async Task WARP_D_THORN_19(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("THORN19_BELIEVER02")]
		public static async Task THORN19_BELIEVER02(Dialog dialog)
		{
			await dialog.Msg("THORN19_BELIEVER02_basic01");
			await dialog.Msg("THORN19_BELIEVER02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN19_BELIEVER03")]
		public static async Task THORN19_BELIEVER03(Dialog dialog)
		{
			await dialog.Msg("THORN19_BELIEVER03_basic01");
			await dialog.Msg("THORN19_BELIEVER03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN_BLACKMAN_TRIGGER2")]
		public static async Task THORN_BLACKMAN_TRIGGER2(Dialog dialog)
		{
			await dialog.Msg("THORN_BLACKMAN_TRIGGER2_basic");
			await dialog.Msg("THORN_BLACKMAN_TRIGGER2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_19682")]
		public static async Task TREASUREBOX_LV_D_THORN_19682(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "emoticonItem_2", 1);

		[DialogFunction("THORN19_GATE01_OPEN")]
		public static async Task THORN19_GATE01_OPEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_19687")]
		public static async Task TREASUREBOX_LV_D_THORN_19687(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_135", 1);

		[DialogFunction("THORN20_MQ01")]
		public static async Task THORN20_MQ01(Dialog dialog)
		{
			await dialog.Msg("THORN20_MQ01_basic01");
			await dialog.Msg("THORN20_MQ01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN20_MQ02_BOSS")]
		public static async Task THORN20_MQ02_BOSS(Dialog dialog)
		{
			await dialog.Msg("THORN20_MQ02_BOSS_basic01");
			await dialog.Msg("THORN20_MQ02_BOSS_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN20_MQ03_TRACK")]
		public static async Task THORN20_MQ03_TRACK(Dialog dialog)
		{
			await dialog.Msg("THORN20_MQ03_TRACK_basic01");
			await dialog.Msg("THORN20_MQ03_TRACK_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN20_MQ04_TRACK")]
		public static async Task THORN20_MQ04_TRACK(Dialog dialog)
		{
			await dialog.Msg("THORN20_MQ04_TRACK_basic01");
			await dialog.Msg("THORN20_MQ04_TRACK_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN20_MAGIC3STAGE")]
		public static async Task THORN20_MAGIC3STAGE(Dialog dialog)
		{
			await dialog.Msg("THORN20_MAGIC3STAGE_basic01");
			await dialog.Msg("THORN20_MAGIC3STAGE_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN20_MQ07_TRACK")]
		public static async Task THORN20_MQ07_TRACK(Dialog dialog)
		{
			await dialog.Msg("THORN20_MQ07_TRACK_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UPPER_WARNING_D_THORN_20")]
		public static async Task UPPER_WARNING_D_THORN_20(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_20865")]
		public static async Task TREASUREBOX_LV_D_THORN_20865(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "Hat_628091", 1, "TreasureboxKey3");

		[DialogFunction("SAULE_PURIFICATION")]
		public static async Task SAULE_PURIFICATION(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_20700")]
		public static async Task TREASUREBOX_LV_D_THORN_20700(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_136", 1);

		[DialogFunction("THORN21_BELIEVER")]
		public static async Task THORN21_BELIEVER(Dialog dialog)
		{
			await dialog.Msg("THORN21_BELIEVER_basic01");
			await dialog.Msg("THORN21_BELIEVER_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_BELIEVER02")]
		public static async Task THORN21_BELIEVER02(Dialog dialog)
		{
			await dialog.Msg("THORN21_BELIEVER02_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_BELIEVER03")]
		public static async Task THORN21_BELIEVER03(Dialog dialog)
		{
			await dialog.Msg("THORN21_BELIEVER03_basic01");
			await dialog.Msg("THORN21_BELIEVER03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_BELIEVER04")]
		public static async Task THORN21_BELIEVER04(Dialog dialog)
		{
			await dialog.Msg("THORN21_BELIEVER04_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_BELIEVER04_AFTER")]
		public static async Task THORN21_BELIEVER04_AFTER(Dialog dialog)
		{
			await dialog.Msg("THORN21_BELIEVER04_after_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_BRAMBLE01_ROOT")]
		public static async Task THORN21_BRAMBLE01_ROOT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_BRAMBLE02_ROOT")]
		public static async Task THORN21_BRAMBLE02_ROOT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_MQ02_TRACK")]
		public static async Task THORN21_MQ02_TRACK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN21_MQ06_TRACK")]
		public static async Task THORN21_MQ06_TRACK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UPPER_WARNING_D_THORN_21")]
		public static async Task UPPER_WARNING_D_THORN_21(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_21142")]
		public static async Task TREASUREBOX_LV_D_THORN_21142(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_137", 1);

		[DialogFunction("THORN21_RP_2_OBJ")]
		public static async Task THORN21_RP_2_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_ADD_SUB_02")]
		public static async Task THORN22_ADD_SUB_02(Dialog dialog)
		{
			await dialog.Msg("THORN22_ADD_SUB_02_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_ADD_SUB_05")]
		public static async Task THORN22_ADD_SUB_05(Dialog dialog)
		{
			await dialog.Msg("THORN22_ADD_SUB_05_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_DOMINIC")]
		public static async Task THORN22_DOMINIC(Dialog dialog)
		{
			await dialog.Msg("THORN22_DOMINIC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_SAMSON")]
		public static async Task THORN22_SAMSON(Dialog dialog)
		{
			await dialog.Msg("THORN22_SAMSON_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_FIREWOOD_1")]
		public static async Task THORN22_FIREWOOD_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_FIREWOOD_2")]
		public static async Task THORN22_FIREWOOD_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_Q_6_TRIGGER")]
		public static async Task THORN22_Q_6_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_OROURKE")]
		public static async Task THORN22_OROURKE(Dialog dialog)
		{
			await dialog.Msg("THORN22_OROURKE_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_JULIAN")]
		public static async Task THORN22_JULIAN(Dialog dialog)
		{
			await dialog.Msg("THORN22_JULIAN_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_POULLTER")]
		public static async Task THORN22_POULLTER(Dialog dialog)
		{
			await dialog.Msg("THORN22_POULLTER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_BOSSKILL_1_TRIGGER")]
		public static async Task THORN22_BOSSKILL_1_TRIGGER(Dialog dialog)
		{
			await dialog.Msg("THORN22_BOSSKILL_1_TRIGGER_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_OWL1")]
		public static async Task THORN22_OWL1(Dialog dialog)
		{
			await dialog.Msg("THORN22_OWL1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_OWL2")]
		public static async Task THORN22_OWL2(Dialog dialog)
		{
			await dialog.Msg("THORN22_OWL2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_Q_9_TRIGGER")]
		public static async Task THORN22_Q_9_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_Q_18")]
		public static async Task THORN22_Q_18(Dialog dialog)
		{
			await dialog.Msg("THORN22_Q_18_basic01");
			await dialog.Msg("THORN22_Q_18_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN22_AREA_WARING01")]
		public static async Task THORN22_AREA_WARING01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_THORN_22")]
		public static async Task WARP_D_THORN_22(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_THORN_22832")]
		public static async Task TREASUREBOX_LV_D_THORN_22832(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_175", 1);

		[DialogFunction("CHAPLAIN_MASTER")]
		public static async Task CHAPLAIN_MASTER(Dialog dialog)
		{
			await dialog.Msg("CHAPLAIN_MASTER_basic_1");
			await dialog.Msg("CHAPLAIN_MASTER_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_ALAN")]
		public static async Task THORN23_ALAN(Dialog dialog)
		{
			await dialog.Msg("THORN23_ALAN_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_WALLACE")]
		public static async Task THORN23_WALLACE(Dialog dialog)
		{
			await dialog.Msg("THORN23_WALLACE_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_WISE")]
		public static async Task THORN23_WISE(Dialog dialog)
		{
			await dialog.Msg("THORN23_WISE_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_OWL2")]
		public static async Task THORN23_OWL2(Dialog dialog)
		{
			await dialog.Msg("THORN23_OWL2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_OWL3")]
		public static async Task THORN23_OWL3(Dialog dialog)
		{
			await dialog.Msg("THORN23_OWL3_basic1");
			await dialog.Msg("THORN23_OWL3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_OWL4")]
		public static async Task THORN23_OWL4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_OWL5")]
		public static async Task THORN23_OWL5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_OWL6")]
		public static async Task THORN23_OWL6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_Q_7_TRIGGER")]
		public static async Task THORN23_Q_7_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN23_FIRETRIGGER")]
		public static async Task THORN23_FIRETRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_23631")]
		public static async Task TREASUREBOX_LV_D_THORN_23631(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_176", 1);

		[DialogFunction("WARP_F_ROKAS_24")]
		public static async Task WARP_F_ROKAS_24(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("ROKAS_24_NEALS")]
		public static async Task ROKAS_24_NEALS(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_NEALS_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_BEARD")]
		public static async Task ROKAS_24_BEARD(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_BEARD_basic1");
			await dialog.Msg("ROKAS_24_BEARD_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_KEFEK")]
		public static async Task ROKAS_24_KEFEK(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_KEFEK_BASIC01");
			await dialog.Msg("ROKAS_24_KEFEK_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_BEACON_TRIGGER")]
		public static async Task ROKAS_24_BEACON_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_RELIC_TRIGGERGO")]
		public static async Task ROKAS_24_RELIC_TRIGGERGO(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_MIRTA")]
		public static async Task ROKAS24_MIRTA(Dialog dialog)
		{
			await dialog.Msg("ROKAS24_MIRTA_basic1");
			await dialog.Msg("ROKAS24_MIRTA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_FLORIJONAS")]
		public static async Task ROKAS_24_FLORIJONAS(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_FLORIJONAS_basic1");
			await dialog.Msg("ROKAS_24_FLORIJONAS_basic2");
			await dialog.Msg("ROKAS_24_FLORIJONAS_basic4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_FLORIJONAS2")]
		public static async Task ROKAS_24_FLORIJONAS2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_FLORIJONAS3")]
		public static async Task ROKAS_24_FLORIJONAS3(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_FLORIJONAS_basic1");
			await dialog.Msg("ROKAS_24_FLORIJONAS_basic2");
			await dialog.Msg("ROKAS_24_FLORIJONAS_basic4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_DEVICE_1")]
		public static async Task ROKAS24_DEVICE_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_DEVICE_2")]
		public static async Task ROKAS24_DEVICE_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_SOLDIER")]
		public static async Task ROKAS24_SOLDIER(Dialog dialog)
		{
			await dialog.Msg("ROKAS24_SOLDIER_basic01");
			await dialog.Msg("ROKAS24_SOLDIER_basic02");
			await dialog.Msg("ROKAS24_SOLDIER_basic03");
			await dialog.Msg("ROKAS24_SOLDIER_basic04");
			await dialog.Msg("ROKAS24_SOLDIER_BASIC01");
			await dialog.Msg("ROKAS24_SOLDIER_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_DABIO")]
		public static async Task ROKAS24_DABIO(Dialog dialog)
		{
			await dialog.Msg("ROKAS24_DABIO_basic01");
			await dialog.Msg("ROKAS24_DABIO_basic02");
			await dialog.Msg("ROKAS24_DABIO_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_FLORIJONAS4")]
		public static async Task ROKAS_24_FLORIJONAS4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_FORSYTHIA")]
		public static async Task ROKAS24_FORSYTHIA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_SUPP_01")]
		public static async Task ROKAS24_SUPP_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_PHARMACIST")]
		public static async Task ROKAS_24_PHARMACIST(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_PHARMACIST_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_NEWCOMER")]
		public static async Task ROKAS_24_NEWCOMER(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_NEWCOMER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_GORATH")]
		public static async Task ROKAS_24_GORATH(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_GORATH_basic01");
			await dialog.Msg("ROKAS_24_GORATH_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_ARCHAEOLOGIST")]
		public static async Task ROKAS_24_ARCHAEOLOGIST(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_ARCHAEOLOGIST_BASIC01");
			await dialog.Msg("ROKAS_24_ARCHAEOLOGIST_basic_01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_PIPOTI")]
		public static async Task ROKAS24_PIPOTI(Dialog dialog)
		{
			await dialog.Msg("ROKAS24_PIPOTI_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_ALCHEMIST")]
		public static async Task ROKAS24_ALCHEMIST(Dialog dialog)
		{
			await dialog.Msg("ROKAS24_ALCHEMIST_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_BOX1")]
		public static async Task ROKAS24_BOX1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_PORTAL")]
		public static async Task ROKAS_24_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_BOARD")]
		public static async Task ROKAS24_BOARD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_MQFLOWER")]
		public static async Task ROKAS24_MQFLOWER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_24_BEARD_AFTER")]
		public static async Task ROKAS_24_BEARD_AFTER(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_BEARD_basic1");
			await dialog.Msg("ROKAS_24_BEARD_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UPPER_WARNING_F_ROKAS_24")]
		public static async Task UPPER_WARNING_F_ROKAS_24(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_RODELERO3_1_NPC")]
		public static async Task JOB_RODELERO3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_RODELERO3_1_NPC_BASIC01");
			await dialog.Msg("JOB_RODELERO3_1_NPC_BASIC03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_CATAPHRACT")]
		public static async Task MASTER_CATAPHRACT(Dialog dialog)
		{
			await dialog.Msg("MASTER_CATAPHRACT_normal");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_THAUMATURGE3_1_NPC")]
		public static async Task JOB_THAUMATURGE3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_THAUMATURGE3_1_NPC_BASIC01");
			await dialog.Msg("JOB_THAUMATURGE3_1_NPC_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_WUGU3_1_NPC")]
		public static async Task JOB_WUGU3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_WUGU3_1_NPC_BASIC01");
			await dialog.Msg("JOB_WUGU3_1_NPC_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_24727")]
		public static async Task TREASUREBOX_LV_F_ROKAS_24727(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_139", 1);

		[DialogFunction("ROKAS24_RP_2_NPC")]
		public static async Task ROKAS24_RP_2_NPC(Dialog dialog)
		{
			await dialog.Msg("ROKAS24_RP_2_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_RP_2_OBJ")]
		public static async Task ROKAS24_RP_2_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_MOVING_DEVICES01")]
		public static async Task HT2_MOVING_DEVICES01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_MOVING_DEVICES02")]
		public static async Task HT2_MOVING_DEVICES02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_MOVING_DEVICES03")]
		public static async Task HT2_MOVING_DEVICES03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_MOVING_DEVICES04")]
		public static async Task HT2_MOVING_DEVICES04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_MOVING_DEVICES05")]
		public static async Task HT2_MOVING_DEVICES05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_MOVING_DEVICES06")]
		public static async Task HT2_MOVING_DEVICES06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_MOVING_DEVICES07")]
		public static async Task HT2_MOVING_DEVICES07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ASSASSIN_MASTER")]
		public static async Task ASSASSIN_MASTER(Dialog dialog)
		{
			await dialog.Msg("ASSASSIN_MASTER_basic1");
			await dialog.Msg("ASSASSIN_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS24_ALCHEMIST_OTHER")]
		public static async Task ROKAS24_ALCHEMIST_OTHER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_F_CORAL_RAID_3_NPC_1")]
		public static async Task EP14_F_CORAL_RAID_3_NPC_1(Dialog dialog)
		{
			await dialog.Msg("EP14_F_CORAL_RAID_3_NPC_1_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_SWITCH1")]
		public static async Task ROKAS25_SWITCH1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_REXIPHER2")]
		public static async Task ROKAS25_REXIPHER2(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_REXIPHER_basic1");
			await dialog.Msg("ROKAS_24_REXIPHER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_REXIPHER3")]
		public static async Task ROKAS25_REXIPHER3(Dialog dialog)
		{
			await dialog.Msg("ROKAS_24_REXIPHER3_basic1");
			await dialog.Msg("ROKAS_24_REXIPHER3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_SWITCH3")]
		public static async Task ROKAS25_SWITCH3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_SUB1")]
		public static async Task ROKAS25_SUB1(Dialog dialog)
		{
			await dialog.Msg("ROKAS25_SUB1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_SWITCH4")]
		public static async Task ROKAS25_SWITCH4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_SWITCH5")]
		public static async Task ROKAS25_SWITCH5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_SWITCH6")]
		public static async Task ROKAS25_SWITCH6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_CALDRON1")]
		public static async Task ROKAS25_CALDRON1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_KEBIN")]
		public static async Task ROKAS25_KEBIN(Dialog dialog)
		{
			await dialog.Msg("ROKAS25_KEBIN_BASIC01");
			await dialog.Msg("ROKAS25_KEBIN_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_BINSENT")]
		public static async Task ROKAS25_BINSENT(Dialog dialog)
		{
			await dialog.Msg("ROKAS25_BINSENT_BASIC_01");
			await dialog.Msg("ROKAS25_BINSENT_BASIC_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_HILDA1")]
		public static async Task ROKAS25_HILDA1(Dialog dialog)
		{
			await dialog.Msg("ROKAS25_HILDA1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_HILDA2")]
		public static async Task ROKAS25_HILDA2(Dialog dialog)
		{
			await dialog.Msg("ROKAS25_HILDA2_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_HILDA3")]
		public static async Task ROKAS25_HILDA3(Dialog dialog)
		{
			await dialog.Msg("ROKAS25_HILDA3_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_FIREWOOD")]
		public static async Task ROKAS25_FIREWOOD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_CALDRON2")]
		public static async Task ROKAS25_CALDRON2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_EX2_STRUCTURE")]
		public static async Task ROKAS25_EX2_STRUCTURE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_HILDA_STRUCTURE")]
		public static async Task ROKAS25_HILDA_STRUCTURE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_REXIPHER5")]
		public static async Task ROKAS25_REXIPHER5(Dialog dialog)
		{
			await dialog.Msg("ROKAS25_REXIPHER5_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_25_HQ01_NPC01")]
		public static async Task ROKAS_25_HQ01_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_25_HQ01_NPC02")]
		public static async Task ROKAS_25_HQ01_NPC02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_25_HQ01_NPC03")]
		public static async Task ROKAS_25_HQ01_NPC03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_25_HQ01_NPC04")]
		public static async Task ROKAS_25_HQ01_NPC04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_REXIPHER2_BOSS_STONE")]
		public static async Task ROKAS25_REXIPHER2_BOSS_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_HILDA_STRUCTURE_TRUE1")]
		public static async Task ROKAS25_HILDA_STRUCTURE_TRUE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_HILDA_STRUCTURE_TRUE2")]
		public static async Task ROKAS25_HILDA_STRUCTURE_TRUE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS25_HILDA_STRUCTURE_TRUE3")]
		public static async Task ROKAS25_HILDA_STRUCTURE_TRUE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_THAUMATURGE3_1_PAPER1")]
		public static async Task JOB_THAUMATURGE3_1_PAPER1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_THAUMATURGE3_1_PAPER2")]
		public static async Task JOB_THAUMATURGE3_1_PAPER2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_THAUMATURGE3_1_PAPER3")]
		public static async Task JOB_THAUMATURGE3_1_PAPER3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_25679")]
		public static async Task TREASUREBOX_LV_F_ROKAS_25679(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_140", 1);

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_25682")]
		public static async Task TREASUREBOX_LV_F_ROKAS_25682(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "JOB_SHINOBI_HIDDEN_ITEM_2", 1);

		[DialogFunction("ROKAS26_BIO")]
		public static async Task ROKAS26_BIO(Dialog dialog)
		{
			await dialog.Msg("ROKAS26_BIO_BASIC02");
			await dialog.Msg("ROKAS26_BIO_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_EHTA")]
		public static async Task ROKAS26_EHTA(Dialog dialog)
		{
			await dialog.Msg("ROKAS26_EHTA_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_LINT")]
		public static async Task ROKAS26_LINT(Dialog dialog)
		{
			await dialog.Msg("ROKAS26_LINT_basic1");
			await dialog.Msg("ROKAS26_LINT_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_REXIPHER1")]
		public static async Task ROKAS26_REXIPHER1(Dialog dialog)
		{
			await dialog.Msg("ROKAS26_REXIPHER1_basic1");
			await dialog.Msg("ROKAS26_REXIPHER1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_POPPY")]
		public static async Task ROKAS26_POPPY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_STONE")]
		public static async Task ROKAS26_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_SUB_Q12")]
		public static async Task ROKAS26_SUB_Q12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_SUB_Q10_TRIGGER")]
		public static async Task ROKAS26_SUB_Q10_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_MQ1_NPC")]
		public static async Task ROKAS26_MQ1_NPC(Dialog dialog)
		{
			await dialog.Msg("ROKAS26_MQ1_NPC_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_MQ1_STONE")]
		public static async Task ROKAS26_MQ1_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_QUEST01_STONE_1")]
		public static async Task ROKAS26_QUEST01_STONE_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_QUEST03_STONE")]
		public static async Task ROKAS26_QUEST03_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_QUEST04_STONE")]
		public static async Task ROKAS26_QUEST04_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS26_QUEST05_STONE")]
		public static async Task ROKAS26_QUEST05_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_26531")]
		public static async Task TREASUREBOX_LV_F_ROKAS_26531(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_141", 1);

		[DialogFunction("ROKAS24_RP_1_NPC")]
		public static async Task ROKAS24_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("ROKAS24_RP_1_NPC_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_DESIG_01")]
		public static async Task ROKAS27_DESIG_01(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_DESIG_01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_GLEN")]
		public static async Task ROKAS27_GLEN(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_GLEN_basic1");
			await dialog.Msg("ROKAS27_GLEN_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_HEINEN")]
		public static async Task ROKAS27_HEINEN(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_HEINEN_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_AIRINE")]
		public static async Task ROKAS27_AIRINE(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_AIRINE_after1");
			await dialog.Msg("ROKAS27_AIRINE_after2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_REXITHER_1")]
		public static async Task ROKAS27_REXITHER_1(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_REXITHER_1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_DESIG_02")]
		public static async Task ROKAS27_DESIG_02(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_DESIG_02_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_QM_2")]
		public static async Task ROKAS27_QM_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_QB_14_1")]
		public static async Task ROKAS27_QB_14_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_QB_14_2")]
		public static async Task ROKAS27_QB_14_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_QB_14_3")]
		public static async Task ROKAS27_QB_14_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_CABLE_LEFT")]
		public static async Task ROKAS27_CABLE_LEFT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_CABLE_RIGHT")]
		public static async Task ROKAS27_CABLE_RIGHT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_MQ1BOX")]
		public static async Task ROKAS27_MQ1BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_MQ2BOX")]
		public static async Task ROKAS27_MQ2BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_MQ3BOX")]
		public static async Task ROKAS27_MQ3BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_MQ4BOX")]
		public static async Task ROKAS27_MQ4BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_MQ5BOX")]
		public static async Task ROKAS27_MQ5BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_DESIG_03")]
		public static async Task ROKAS27_DESIG_03(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_DESIG_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS27_AIRINE2")]
		public static async Task ROKAS27_AIRINE2(Dialog dialog)
		{
			await dialog.Msg("ROKAS27_AIRINE2_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_ROKAS_27")]
		public static async Task WARP_F_ROKAS_27(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("JOB_CORSAIR4_NPC")]
		public static async Task JOB_CORSAIR4_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_CORSAIR4_NPC_basic01");
			await dialog.Msg("JOB_CORSAIR4_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_SADU3_1_NPC")]
		public static async Task JOB_SADU3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_SADU3_1_NPC_BASIC01");
			await dialog.Msg("JOB_SADU3_1_NPC_BASIC03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_WARLOCK3_1_NPC")]
		public static async Task JOB_WARLOCK3_1_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_WARLOCK3_1_NPC_BASIC01");
			await dialog.Msg("JOB_WARLOCK3_1_NPC_BASIC03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_27_CABLETEMP_01")]
		public static async Task ROKAS_27_CABLETEMP_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_27_CABLETEMP_02")]
		public static async Task ROKAS_27_CABLETEMP_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PARDONER4_1")]
		public static async Task JOB_PARDONER4_1(Dialog dialog)
		{
			await dialog.Msg("JOB_PARDONER4_1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_27722")]
		public static async Task TREASUREBOX_LV_F_ROKAS_27722(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_142", 1);

		[DialogFunction("ROKAS28_TRAP")]
		public static async Task ROKAS28_TRAP(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_ENTICE")]
		public static async Task ROKAS28_ENTICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_MQ1_1")]
		public static async Task ROKAS28_MQ1_1(Dialog dialog)
		{
			await dialog.Msg("ROKAS28_MQ1_1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_ODEL_MQ4_DLG")]
		public static async Task ROKAS28_ODEL_MQ4_DLG(Dialog dialog)
		{
			await dialog.Msg("ROKAS28_ODEL_MQ4_DLG_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_MQ5_NPC")]
		public static async Task ROKAS28_MQ5_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_ODEL_MQ6")]
		public static async Task ROKAS28_ODEL_MQ6(Dialog dialog)
		{
			await dialog.Msg("ROKAS28_ODEL_MQ6_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_FAKE1")]
		public static async Task ROKAS28_SEAL_FAKE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_FAKE2")]
		public static async Task ROKAS28_SEAL_FAKE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_FAKE3")]
		public static async Task ROKAS28_SEAL_FAKE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_FAKE4")]
		public static async Task ROKAS28_SEAL_FAKE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_FAKE5")]
		public static async Task ROKAS28_SEAL_FAKE5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_FAKE6")]
		public static async Task ROKAS28_SEAL_FAKE6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_TRUE2")]
		public static async Task ROKAS28_SEAL_TRUE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_TRUE3")]
		public static async Task ROKAS28_SEAL_TRUE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SEAL_TRUE1")]
		public static async Task ROKAS28_SEAL_TRUE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_FRIDKA")]
		public static async Task ROKAS28_FRIDKA(Dialog dialog)
		{
			await dialog.Msg("ROKAS28_FRIDKA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_ORION")]
		public static async Task ROKAS28_ORION(Dialog dialog)
		{
			await dialog.Msg("ROKAS28_ORION_basic01");
			await dialog.Msg("ROKAS28_ORION_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_ODEL")]
		public static async Task ROKAS28_ODEL(Dialog dialog)
		{
			await dialog.Msg("ROKAS28_ODEL_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS28_SECRET_PORTAL")]
		public static async Task ROKAS28_SECRET_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_281040")]
		public static async Task TREASUREBOX_LV_F_ROKAS_281040(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_143", 1);

		[DialogFunction("ROKAS29_HALF_SUCCESS1")]
		public static async Task ROKAS29_HALF_SUCCESS1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VACYS_LIVE")]
		public static async Task VACYS_LIVE(Dialog dialog)
		{
			await dialog.Msg("VACYS_LIVE_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_BAG")]
		public static async Task ROKAS29_BAG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_FIRE")]
		public static async Task ROKAS29_FIRE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VACYS_SOUL")]
		public static async Task VACYS_SOUL(Dialog dialog)
		{
			await dialog.Msg("VACYS_SOUL_basic1");
			await dialog.Msg("VACYS_SOUL_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_SLATE1")]
		public static async Task ROKAS29_SLATE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_SLATE2")]
		public static async Task ROKAS29_SLATE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_SLATE3")]
		public static async Task ROKAS29_SLATE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_REXITHER1")]
		public static async Task ROKAS29_MQ_REXITHER1(Dialog dialog)
		{
			await dialog.Msg("ROKAS29_MQ_REXITHER1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_REXITHER6")]
		public static async Task ROKAS29_MQ_REXITHER6(Dialog dialog)
		{
			await dialog.Msg("ROKAS29_MQ_REXITHER6_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_REXITHER2")]
		public static async Task ROKAS29_MQ_REXITHER2(Dialog dialog)
		{
			await dialog.Msg("ROKAS29_MQ_REXITHER2_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_REXITHER3")]
		public static async Task ROKAS29_MQ_REXITHER3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_REXITHER4")]
		public static async Task ROKAS29_MQ_REXITHER4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_REXITHER5")]
		public static async Task ROKAS29_MQ_REXITHER5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_DEVICE1")]
		public static async Task ROKAS29_MQ_DEVICE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_DEVICE2")]
		public static async Task ROKAS29_MQ_DEVICE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_DEVICE4")]
		public static async Task ROKAS29_MQ_DEVICE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS29_MQ_DEVICE5")]
		public static async Task ROKAS29_MQ_DEVICE5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_29654")]
		public static async Task TREASUREBOX_LV_F_ROKAS_29654(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_148", 1);

		[DialogFunction("ROKAS30_PIPOTI")]
		public static async Task ROKAS30_PIPOTI(Dialog dialog)
		{
			await dialog.Msg("ROKAS30_PIPOTI_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_PIPOTI02_TRIGGER")]
		public static async Task ROKAS30_PIPOTI02_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_PIPOTI03_TRIGGER")]
		public static async Task ROKAS30_PIPOTI03_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_PIPOTI04_TRIGGER")]
		public static async Task ROKAS30_PIPOTI04_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_BAIL")]
		public static async Task ROKAS30_BAIL(Dialog dialog)
		{
			await dialog.Msg("ROKAS30_BAIL_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_COLLIN")]
		public static async Task ROKAS30_COLLIN(Dialog dialog)
		{
			await dialog.Msg("ROKAS30_COLLIN_basic01");
			await dialog.Msg("ROKAS30_COLLIN_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_ODEL")]
		public static async Task ROKAS30_ODEL(Dialog dialog)
		{
			await dialog.Msg("ROKAS30_ODEL_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_MQ2_1_SCAR")]
		public static async Task ROKAS30_MQ2_1_SCAR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_SAELDEVICE2")]
		public static async Task ROKAS30_SAELDEVICE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_SEALDESTROY1")]
		public static async Task ROKAS30_SEALDESTROY1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_SAELDEVICE1")]
		public static async Task ROKAS30_SAELDEVICE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_SEALDESTROY2")]
		public static async Task ROKAS30_SEALDESTROY2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_ZACARIEL_SERVANT1")]
		public static async Task ROKAS30_ZACARIEL_SERVANT1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_ZACARIEL_SERVANT2")]
		public static async Task ROKAS30_ZACARIEL_SERVANT2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_HURT")]
		public static async Task ROKAS30_HURT(Dialog dialog)
		{
			await dialog.Msg("ROKAS30_HURT_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_ODEL2")]
		public static async Task ROKAS_ODEL2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_ROKAS_30")]
		public static async Task WARP_F_ROKAS_30(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("ROKAS30_PIPOTI02_TREASUREBOX")]
		public static async Task ROKAS30_PIPOTI02_TREASUREBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_PIPOTI05_TRIGGER")]
		public static async Task ROKAS30_PIPOTI05_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_PIPOTI03_TREASUREBOX")]
		public static async Task ROKAS30_PIPOTI03_TREASUREBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_PIPOTI04_TREASUREBOX")]
		public static async Task ROKAS30_PIPOTI04_TREASUREBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS30_PIPOTI05_TREASUREBOX")]
		public static async Task ROKAS30_PIPOTI05_TREASUREBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_30668")]
		public static async Task TREASUREBOX_LV_F_ROKAS_30668(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_149", 1);

		[DialogFunction("JOB_PLAGUEDOCTOR71_NPC1")]
		public static async Task JOB_PLAGUEDOCTOR71_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS31_PACT_END")]
		public static async Task ROKAS31_PACT_END(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS31_REXITHER2")]
		public static async Task ROKAS31_REXITHER2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS31_RECORD")]
		public static async Task ROKAS31_RECORD(Dialog dialog)
		{
			await dialog.Msg("ROKAS31_RECORD_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS31_ODEL2")]
		public static async Task ROKAS31_ODEL2(Dialog dialog)
		{
			await dialog.Msg("ROKAS31_ODEL2_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS31_REXITHER1")]
		public static async Task ROKAS31_REXITHER1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS31_SUB_01_BOX")]
		public static async Task ROKAS31_SUB_01_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS31_SUB")]
		public static async Task ROKAS31_SUB(Dialog dialog)
		{
			await dialog.Msg("ROKAS31_SUB_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_ROKAS_31_EV_55_001")]
		public static async Task F_ROKAS_31_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_31629")]
		public static async Task TREASUREBOX_LV_F_ROKAS_31629(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_150", 1);

		[DialogFunction("ZACHA32_MQ_04_D")]
		public static async Task ZACHA32_MQ_04_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_MQ_01")]
		public static async Task ZACHA1F_MQ_01(Dialog dialog)
		{
			await dialog.Msg("ZACHA1F_MQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_MQ_01_MON")]
		public static async Task ZACHA1F_MQ_01_MON(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_MQ_02")]
		public static async Task ZACHA1F_MQ_02(Dialog dialog)
		{
			await dialog.Msg("ZACHA1F_MQ_02_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA32_DEVICE_1_2")]
		public static async Task ZACHA32_DEVICE_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA32_DEVICE02")]
		public static async Task ZACHA32_DEVICE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_MQ_03")]
		public static async Task ZACHA1F_MQ_03(Dialog dialog)
		{
			await dialog.Msg("ZACHA1F_MQ_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_MQ_04")]
		public static async Task ZACHA1F_MQ_04(Dialog dialog)
		{
			await dialog.Msg("ZACHA1F_MQ_04_01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_SQ_01")]
		public static async Task ZACHA1F_SQ_01(Dialog dialog)
		{
			await dialog.Msg("ZACHA1F_SQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_SQ_02")]
		public static async Task ZACHA1F_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_SQ_03")]
		public static async Task ZACHA1F_SQ_03(Dialog dialog)
		{
			await dialog.Msg("ZACHA1F_SQ_03_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_SQ_04")]
		public static async Task ZACHA1F_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA1F_SQ_05")]
		public static async Task ZACHA1F_SQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_ZACHARIEL_32")]
		public static async Task WARP_D_ZACHARIEL_32(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("ZACHA1F_MQ_05_NPC")]
		public static async Task ZACHA1F_MQ_05_NPC(Dialog dialog)
		{
			await dialog.Msg("ZACHA1F_MQ_05_prog_startnpc");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_ZACHARIEL_323023")]
		public static async Task TREASUREBOX_LV_D_ZACHARIEL_323023(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_151", 1);

		[DialogFunction("ZACHA32_RP_1_NPC")]
		public static async Task ZACHA32_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("ZACHA32_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_MQ_05")]
		public static async Task ZACHA2F_MQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_SQ_01")]
		public static async Task ZACHA2F_SQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHARIEL33_GUARDIAN1")]
		public static async Task ZACHARIEL33_GUARDIAN1(Dialog dialog)
		{
			await dialog.Msg("ZACHARIEL33_GUARDIAN1_basic01");
			await dialog.Msg("ZACHARIEL33_GUARDIAN1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHARIEL33_GUARDIAN2")]
		public static async Task ZACHARIEL33_GUARDIAN2(Dialog dialog)
		{
			await dialog.Msg("ZACHARIEL33_GUARDIAN2_basic01");
			await dialog.Msg("ZACHARIEL33_GUARDIAN2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_SQ_03")]
		public static async Task ZACHA2F_SQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_MQ_04")]
		public static async Task ZACHA2F_MQ_04(Dialog dialog)
		{
			await dialog.Msg("ZACHA2F_MQ_04_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_SQ")]
		public static async Task ZACHA2F_SQ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_SQ_04")]
		public static async Task ZACHA2F_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_SQ02_TREASURE")]
		public static async Task ZACHA2F_SQ02_TREASURE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA2F_SQ_05")]
		public static async Task ZACHA2F_SQ_05(Dialog dialog)
		{
			await dialog.Msg("ZACHA2F_SQ_05_basic01");
			await dialog.Msg("ZACHA2F_SQ_05_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WS_ZACHA2F_01_TO_02")]
		public static async Task WS_ZACHA2F_01_TO_02(Dialog dialog)
		{
			await dialog.Msg("WS_ZACHA2F_01_TO_02_basic01");
			await dialog.Msg("WS_ZACHA2F_01_TO_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_ZACHARIEL_33")]
		public static async Task WARP_D_ZACHARIEL_33(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_ZACHARIEL_333024")]
		public static async Task TREASUREBOX_LV_D_ZACHARIEL_333024(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_152", 1);

		[DialogFunction("ZACHA33_RP_1_NPC")]
		public static async Task ZACHA33_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("ZACHA33_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_POT")]
		public static async Task ZACHA3F_POT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_MQ")]
		public static async Task ZACHA3F_MQ(Dialog dialog)
		{
			await dialog.Msg("ZACHA3F_MQ_basic01");
			await dialog.Msg("ZACHA3F_MQ_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_SQ_01")]
		public static async Task ZACHA3F_SQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_SQ_02")]
		public static async Task ZACHA3F_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_SQ_03")]
		public static async Task ZACHA3F_SQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_SQ_04")]
		public static async Task ZACHA3F_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_MQ03_MQ04")]
		public static async Task ZACHA3F_MQ03_MQ04(Dialog dialog)
		{
			await dialog.Msg("ZACHA3F_MQ03_MQ04_basic01");
			await dialog.Msg("ZACHA3F_MQ03_MQ04_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_SQ02_GUARD")]
		public static async Task ZACHA3F_SQ02_GUARD(Dialog dialog)
		{
			await dialog.Msg("ZACHA3F_SQ02_GUARD_basic01");
			await dialog.Msg("ZACHA3F_SQ02_GUARD_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_SQ04_GUARD")]
		public static async Task ZACHA3F_SQ04_GUARD(Dialog dialog)
		{
			await dialog.Msg("ZACHA3F_SQ04_GUARD_basic01");
			await dialog.Msg("ZACHA3F_SQ04_GUARD_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_MQ04_GUARD")]
		public static async Task ZACHA3F_MQ04_GUARD(Dialog dialog)
		{
			await dialog.Msg("ZACHA3F_MQ04_GUARD_basic01");
			await dialog.Msg("ZACHA3F_MQ04_GUARD_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_ZACHARIEL_343027")]
		public static async Task TREASUREBOX_LV_D_ZACHARIEL_343027(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_153", 1);

		[DialogFunction("ZACHA3F_MQ_01_BLACKMAN_01")]
		public static async Task ZACHA3F_MQ_01_BLACKMAN_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA3F_MQ_01_BLACKMAN_04")]
		public static async Task ZACHA3F_MQ_01_BLACKMAN_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ")]
		public static async Task ZACHA4F_MQ(Dialog dialog)
		{
			await dialog.Msg("ZACHA4F_MQ_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ_02")]
		public static async Task ZACHA4F_MQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ_04")]
		public static async Task ZACHA4F_MQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_SQ_01")]
		public static async Task ZACHA4F_SQ_01(Dialog dialog)
		{
			await dialog.Msg("ZACHA4F_SQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_SQ_03")]
		public static async Task ZACHA4F_SQ_03(Dialog dialog)
		{
			await dialog.Msg("ZACHA4F_SQ_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_SQ_05")]
		public static async Task ZACHA4F_SQ_05(Dialog dialog)
		{
			await dialog.Msg("ZACHA4F_SQ_05_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_GUARDIAN01")]
		public static async Task ZACHA4F_GUARDIAN01(Dialog dialog)
		{
			await dialog.Msg("ZACHA3F_SQ02_GUARD_basic01");
			await dialog.Msg("ZACHA3F_SQ02_GUARD_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_GUARDIAN02")]
		public static async Task ZACHA4F_GUARDIAN02(Dialog dialog)
		{
			await dialog.Msg("ZACHARIEL33_GUARDIAN2_basic02");
			await dialog.Msg("ZACHA3F_MQ04_GUARD_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ05")]
		public static async Task ZACHA4F_MQ05(Dialog dialog)
		{
			await dialog.Msg("ZACHA4F_MQ05_basic01");
			await dialog.Msg("ZACHA4F_MQ05_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ_03")]
		public static async Task ZACHA4F_MQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ_02_HIDE")]
		public static async Task ZACHA4F_MQ_02_HIDE(Dialog dialog)
		{
			await dialog.Msg("ZACHARIEL33_GUARDIAN2_basic02");
			await dialog.Msg("ZACHA3F_MQ04_GUARD_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_ZACHARIEL_353026")]
		public static async Task TREASUREBOX_LV_D_ZACHARIEL_353026(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_154", 1);

		[DialogFunction("HT_ZACHA_CUBE01")]
		public static async Task HT_ZACHA_CUBE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT_ZACHA_DESK01")]
		public static async Task HT_ZACHA_DESK01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ_01_BLACKMAN")]
		public static async Task ZACHA4F_MQ_01_BLACKMAN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ_04_BLACKMAN")]
		public static async Task ZACHA4F_MQ_04_BLACKMAN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA4F_MQ_05_BLACKMAN")]
		public static async Task ZACHA4F_MQ_05_BLACKMAN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA36_POT1_NPC")]
		public static async Task ZACHA36_POT1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_MQ_02")]
		public static async Task ZACHA5F_MQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_MQ_03")]
		public static async Task ZACHA5F_MQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_MQ_04")]
		public static async Task ZACHA5F_MQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHARIEL_GUARDIAN")]
		public static async Task ZACHARIEL_GUARDIAN(Dialog dialog)
		{
			await dialog.Msg("ZACHARIEL_GUARDIAN_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_ZACHARIEL_36")]
		public static async Task WARP_D_ZACHARIEL_36(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("ZACHA5F_EQ_04")]
		public static async Task ZACHA5F_EQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_EQ_03")]
		public static async Task ZACHA5F_EQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_EQ_01")]
		public static async Task ZACHA5F_EQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_EQ_02")]
		public static async Task ZACHA5F_EQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_EQ_05")]
		public static async Task ZACHA5F_EQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHARIEL36_ROKAS31")]
		public static async Task ZACHARIEL36_ROKAS31(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZACHA5F_MQ_05")]
		public static async Task ZACHA5F_MQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_ZACHARIEL_363025")]
		public static async Task TREASUREBOX_LV_D_ZACHARIEL_363025(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_155", 1);

		[DialogFunction("CATACOMB_01_SPIRIT_01")]
		public static async Task CATACOMB_01_SPIRIT_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_01_SPIRIT_03")]
		public static async Task CATACOMB_01_SPIRIT_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_ID_CATACOMB_01")]
		public static async Task WARP_ID_CATACOMB_01(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("CATACOMB_01_TOMBSTONE_01")]
		public static async Task CATACOMB_01_TOMBSTONE_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_01_TOMBSTONE_02")]
		public static async Task CATACOMB_01_TOMBSTONE_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_01_TOMBSTONE_03")]
		public static async Task CATACOMB_01_TOMBSTONE_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_01_BOARD_01")]
		public static async Task CATACOMB_01_BOARD_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_01_TOMBSTONE_04")]
		public static async Task CATACOMB_01_TOMBSTONE_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_01700")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_01700(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_138", 1);

		[DialogFunction("LOWLV_MASTER_ENCY_SQ_30_BOOK")]
		public static async Task LOWLV_MASTER_ENCY_SQ_30_BOOK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EXORCIST_JOB_QUEST_OBJ_START")]
		public static async Task EXORCIST_JOB_QUEST_OBJ_START(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_NPC_01")]
		public static async Task CATACOMB_02_NPC_01(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_02_NPC_01_basic01");
			await dialog.Msg("CATACOMB_02_NPC_01_basic02");
			await dialog.Msg("CATACOMB_02_NPC_01_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_NPC_02")]
		public static async Task CATACOMB_02_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_01")]
		public static async Task CATACOMB_02_OBJ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_RECORD_01")]
		public static async Task CATACOMB_02_RECORD_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_RECORD_02")]
		public static async Task CATACOMB_02_RECORD_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_RECORD_03")]
		public static async Task CATACOMB_02_RECORD_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_BOOK_01")]
		public static async Task CATACOMB_02_BOOK_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_BOOK_02")]
		public static async Task CATACOMB_02_BOOK_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_BOOK_03")]
		public static async Task CATACOMB_02_BOOK_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_02")]
		public static async Task CATACOMB_02_OBJ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_03")]
		public static async Task CATACOMB_02_OBJ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_04")]
		public static async Task CATACOMB_02_OBJ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_05_1")]
		public static async Task CATACOMB_02_OBJ_05_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_05_2")]
		public static async Task CATACOMB_02_OBJ_05_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_05_3")]
		public static async Task CATACOMB_02_OBJ_05_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_06")]
		public static async Task CATACOMB_02_OBJ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_07")]
		public static async Task CATACOMB_02_OBJ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_08")]
		public static async Task CATACOMB_02_OBJ_08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_09")]
		public static async Task CATACOMB_02_OBJ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_10")]
		public static async Task CATACOMB_02_OBJ_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_11")]
		public static async Task CATACOMB_02_OBJ_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_05_1_AFTER")]
		public static async Task CATACOMB_02_OBJ_05_1_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_05_2_AFTER")]
		public static async Task CATACOMB_02_OBJ_05_2_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_02_OBJ_05_3_AFTER")]
		public static async Task CATACOMB_02_OBJ_05_3_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_021000")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_021000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_217", 1);

		[DialogFunction("CATACOMB_04_SECRETOUT")]
		public static async Task CATACOMB_04_SECRETOUT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_04_OBJ_01")]
		public static async Task CATACOMB_04_OBJ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_04_OBJ_02")]
		public static async Task CATACOMB_04_OBJ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_04_OBJ_03")]
		public static async Task CATACOMB_04_OBJ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_04_OBJ_04")]
		public static async Task CATACOMB_04_OBJ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_04_OBJ_05")]
		public static async Task CATACOMB_04_OBJ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_04_OBJ_06")]
		public static async Task CATACOMB_04_OBJ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_04_OBJ_07")]
		public static async Task CATACOMB_04_OBJ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_041000")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_041000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_215", 1);

		[DialogFunction("HT_CATACOMB_04_SKULL")]
		public static async Task HT_CATACOMB_04_SKULL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_STONE1")]
		public static async Task REMAINS37_STONE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN37_SQ07")]
		public static async Task REMAIN37_SQ07(Dialog dialog)
		{
			await dialog.Msg("REMAIN37_SQ07_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN37_RAYMOND")]
		public static async Task REMAIN37_RAYMOND(Dialog dialog)
		{
			await dialog.Msg("REMAIN37_RAYMOND_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN37_SQ6_UNCLE1")]
		public static async Task REMAIN37_SQ6_UNCLE1(Dialog dialog)
		{
			await dialog.Msg("REMAIN37_SQ6_UNCLE1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN37_SMEADE")]
		public static async Task REMAIN37_SMEADE(Dialog dialog)
		{
			await dialog.Msg("REMAIN37_SMEADE_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_CHRONO")]
		public static async Task MASTER_CHRONO(Dialog dialog)
		{
			await dialog.Msg("MASTER_CHRONO_BASIC01");
			await dialog.Msg("MASTER_CHRONO_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_REMAINS_37")]
		public static async Task WARP_F_REMAINS_37(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("REMAIN37_MQ05")]
		public static async Task REMAIN37_MQ05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN37_MQ03")]
		public static async Task REMAIN37_MQ03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN37_MQ04")]
		public static async Task REMAIN37_MQ04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN37_SQ04")]
		public static async Task REMAIN37_SQ04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_FALCONER")]
		public static async Task MASTER_FALCONER(Dialog dialog)
		{
			await dialog.Msg("MASTER_FALCONER_basic01");
			await dialog.Msg("MASTER_FALCONER_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_3764")]
		public static async Task TREASUREBOX_LV_F_REMAINS_3764(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_160", 1);

		[DialogFunction("JOB_MATADOR1_NPC")]
		public static async Task JOB_MATADOR1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LEGEND_RAID_MORINGPONIA_PORTAL")]
		public static async Task LEGEND_RAID_MORINGPONIA_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_MQ_MONUMENT1")]
		public static async Task REMAIN38_MQ_MONUMENT1(Dialog dialog)
		{
			await dialog.Msg("REMAIN38_MQ_MONUMENT1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_MQ_MONUMENT2")]
		public static async Task REMAIN38_MQ_MONUMENT2(Dialog dialog)
		{
			await dialog.Msg("REMAIN38_MQ_MONUMENT2_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_MQ_MONUMENT3")]
		public static async Task REMAIN38_MQ_MONUMENT3(Dialog dialog)
		{
			await dialog.Msg("REMAIN38_MQ_MONUMENT3_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_MQ_MONUMENT4")]
		public static async Task REMAIN38_MQ_MONUMENT4(Dialog dialog)
		{
			await dialog.Msg("REMAIN38_MQ_MONUMENT4_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_MQ_MONUMENT5")]
		public static async Task REMAIN38_MQ_MONUMENT5(Dialog dialog)
		{
			await dialog.Msg("REMAIN38_MQ_MONUMENT5_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_SQ01")]
		public static async Task REMAIN38_SQ01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_SQ_DRASIUS")]
		public static async Task REMAIN38_SQ_DRASIUS(Dialog dialog)
		{
			await dialog.Msg("REMAIN38_SQ_DRASIUS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_REMAINS_38")]
		public static async Task WARP_F_REMAINS_38(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("REMAIN38_HUNTER")]
		public static async Task REMAIN38_HUNTER(Dialog dialog)
		{
			await dialog.Msg("REMAIN38_HUNTER_basic_01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_SQ06")]
		public static async Task REMAIN38_SQ06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN38_SQ02")]
		public static async Task REMAIN38_SQ02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_3886")]
		public static async Task TREASUREBOX_LV_F_REMAINS_3886(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "expCard6", 3, "TreasureboxKey3");

		[DialogFunction("MASTER_FLETCHER")]
		public static async Task MASTER_FLETCHER(Dialog dialog)
		{
			await dialog.Msg("MASTER_FLETCHER_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_MONK4_1")]
		public static async Task JOB_MONK4_1(Dialog dialog)
		{
			await dialog.Msg("JOB_MONK4_1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_3888")]
		public static async Task TREASUREBOX_LV_F_REMAINS_3888(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_161", 1);

		[DialogFunction("RUNECASTER_MASTER")]
		public static async Task RUNECASTER_MASTER(Dialog dialog)
		{
			await dialog.Msg("RUNECASTER_MASTER_basic1");
			await dialog.Msg("RUNECASTER_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS38_RP_1_OBJ")]
		public static async Task REMAINS38_RP_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_WATER_REMAINS")]
		public static async Task HIDDEN_WATER_REMAINS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PLAGUEDOCTOR71_NPC3")]
		public static async Task JOB_PLAGUEDOCTOR71_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_MQ_MONUMENT1")]
		public static async Task REMAINS39_MQ_MONUMENT1(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_MQ_MONUMENT1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_MQ_MONUMENT2")]
		public static async Task REMAINS39_MQ_MONUMENT2(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_MQ_MONUMENT2_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_MQ_MONUMENT3")]
		public static async Task REMAINS39_MQ_MONUMENT3(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_MQ_MONUMENT3_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_MQ_MONUMENT4")]
		public static async Task REMAINS39_MQ_MONUMENT4(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_MQ_MONUMENT4_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_MQ_MONUMENT5")]
		public static async Task REMAINS39_MQ_MONUMENT5(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_MQ_MONUMENT5_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_MQ_MONUMENT6")]
		public static async Task REMAINS39_MQ_MONUMENT6(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_MQ_MONUMENT6_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_GHOST_BAG_1")]
		public static async Task REMAINS39_GHOST_BAG_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_PEAPLE")]
		public static async Task REMAINS39_PEAPLE(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_PEAPLE_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_GHOST")]
		public static async Task REMAINS39_GHOST(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_GHOST_basic_01");
			await dialog.Msg("REMAINS39_GHOST_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN39_SQ_MOJE")]
		public static async Task REMAIN39_SQ_MOJE(Dialog dialog)
		{
			await dialog.Msg("REMAIN39_SQ_MOJE_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN39_SQ_MAN")]
		public static async Task REMAIN39_SQ_MAN(Dialog dialog)
		{
			await dialog.Msg("REMAIN39_SQ_MAN_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAIN39_SQ_WOOD")]
		public static async Task REMAIN39_SQ_WOOD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_GHOST_BAG_2")]
		public static async Task REMAINS39_GHOST_BAG_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_3955")]
		public static async Task TREASUREBOX_LV_F_REMAINS_3955(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_162", 1);

		[DialogFunction("REMAINS39_RP_1_NPC")]
		public static async Task REMAINS39_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("REMAINS39_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_CHAR119_MSTEP2_1")]
		public static async Task REMAINS39_CHAR119_MSTEP2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_CHAR119_MSTEP2_2")]
		public static async Task REMAINS39_CHAR119_MSTEP2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS39_CHAR119_MSTEP2_3")]
		public static async Task REMAINS39_CHAR119_MSTEP2_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_DRUNK_01")]
		public static async Task REMAINS_40_DRUNK_01(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_DRUNK_01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_TARA_01")]
		public static async Task REMAINS_40_TARA_01(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_TARA_01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_CANOLIN_01")]
		public static async Task REMAINS_40_CANOLIN_01(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_CANOLIN_01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_MQ_02")]
		public static async Task REMAINS_40_MQ_02(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_MQ_02_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_MQ_03")]
		public static async Task REMAINS_40_MQ_03(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_MQ_03_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_MQ_04")]
		public static async Task REMAINS_40_MQ_04(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_MQ_04_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_MQ_05")]
		public static async Task REMAINS_40_MQ_05(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_MQ_05_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_MQ_06")]
		public static async Task REMAINS_40_MQ_06(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_MQ_06_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_MQ_07")]
		public static async Task REMAINS_40_MQ_07(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_MQ_07_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_TARA_02")]
		public static async Task REMAINS_40_TARA_02(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_TARA_02_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_DRUNK_02")]
		public static async Task REMAINS_40_DRUNK_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_DRUNK_03")]
		public static async Task REMAINS_40_DRUNK_03(Dialog dialog)
		{
			await dialog.Msg("REMAINS_40_DRUNK_03_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_MQ_01")]
		public static async Task REMAINS_40_MQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS40_GRITA")]
		public static async Task REMAINS40_GRITA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS_40_HQ_01_TB")]
		public static async Task REMAINS_40_HQ_01_TB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS40_BOARD01")]
		public static async Task REMAINS40_BOARD01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_TRESURE01")]
		public static async Task HIDDEN_TRESURE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_TRESURE02")]
		public static async Task HIDDEN_TRESURE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_TRESURE03")]
		public static async Task HIDDEN_TRESURE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_TRESURE04")]
		public static async Task HIDDEN_TRESURE04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TRESURE01_BOX")]
		public static async Task TRESURE01_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS40_HIDDEN_EVENT")]
		public static async Task REMAINS40_HIDDEN_EVENT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_40427")]
		public static async Task TREASUREBOX_LV_F_REMAINS_40427(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_163", 1);

		[DialogFunction("CATACOMB_25_4_LAIMIS")]
		public static async Task CATACOMB_25_4_LAIMIS(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_25_4_LAIMIS_BG");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HTA_REMAINS_40_WATERPOT")]
		public static async Task HTA_REMAINS_40_WATERPOT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HTA_REMAINS_40_SMOKE")]
		public static async Task HTA_REMAINS_40_SMOKE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EXORCIST_MASTER_STEP33_NPC1")]
		public static async Task EXORCIST_MASTER_STEP33_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EXORCIST_MASTER_STEP33_NPC2")]
		public static async Task EXORCIST_MASTER_STEP33_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER41_GRITA_01")]
		public static async Task FTOWER41_GRITA_01(Dialog dialog)
		{
			await dialog.Msg("FTOWER41_GRITA_01_basic01");
			await dialog.Msg("FTOWER41_GRITA_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER41_MQ_05")]
		public static async Task FTOWER41_MQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER41_MQ_03")]
		public static async Task FTOWER41_MQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER41_SQ_01")]
		public static async Task FTOWER41_SQ_01(Dialog dialog)
		{
			await dialog.Msg("FTOWER41_SQ_01_IDLE");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER41_SQ_03")]
		public static async Task FTOWER41_SQ_03(Dialog dialog)
		{
			await dialog.Msg("FTOWER41_SQ_IDLE");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER41_SQ_05_NPC")]
		public static async Task FTOWER41_SQ_05_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER41_MQ_02_NPC")]
		public static async Task FTOWER41_MQ_02_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_FIRETOWER_41")]
		public static async Task WARP_D_FIRETOWER_41(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("FTOWER41_MQ_01_NPC")]
		public static async Task FTOWER41_MQ_01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_2_PSYCHOKINO_5_1_BOOK_1")]
		public static async Task JOB_2_PSYCHOKINO_5_1_BOOK_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_41126")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_41126(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_170", 1);

		[DialogFunction("FTOWER42_GRITA_01")]
		public static async Task FTOWER42_GRITA_01(Dialog dialog)
		{
			await dialog.Msg("FTOWER42_GRITA_01_basic01");
			await dialog.Msg("FTOWER42_GRITA_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER42_MQ_05")]
		public static async Task FTOWER42_MQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER42_SQ_01")]
		public static async Task FTOWER42_SQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER42_SQ_02")]
		public static async Task FTOWER42_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER42_SQ_04")]
		public static async Task FTOWER42_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER42_SQ_05")]
		public static async Task FTOWER42_SQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_2_PSYCHOKINO_5_1_BOOK_2")]
		public static async Task JOB_2_PSYCHOKINO_5_1_BOOK_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_42223")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_42223(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_171", 1);

		[DialogFunction("SAGE_JOBQ_IN_FTOWER1")]
		public static async Task SAGE_JOBQ_IN_FTOWER1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_FTOWER2")]
		public static async Task SAGE_JOBQ_IN_FTOWER2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_FTOWER4")]
		public static async Task SAGE_JOBQ_IN_FTOWER4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_FTOWER3")]
		public static async Task SAGE_JOBQ_IN_FTOWER3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER43_GRITA_01")]
		public static async Task FTOWER43_GRITA_01(Dialog dialog)
		{
			await dialog.Msg("FTOWER43_GRITA_01_basic11");
			await dialog.Msg("FTOWER43_GRITA_01_basic12");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER43_SQ_01")]
		public static async Task FTOWER43_SQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER43_SQ_03")]
		public static async Task FTOWER43_SQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER43_SQ_05")]
		public static async Task FTOWER43_SQ_05(Dialog dialog)
		{
			await dialog.Msg("FTOWER43_SQ_05_IDLE");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER43_MQ_03_BOOK")]
		public static async Task FTOWER43_MQ_03_BOOK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER43_MQ_02_VALVE")]
		public static async Task FTOWER43_MQ_02_VALVE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER43_MQ_05_MINENPC")]
		public static async Task FTOWER43_MQ_05_MINENPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_2_PSYCHOKINO_5_1_BOOK_3")]
		public static async Task JOB_2_PSYCHOKINO_5_1_BOOK_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_43241")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_43241(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_172", 1);

		[DialogFunction("FTOWER44_GRITA_01")]
		public static async Task FTOWER44_GRITA_01(Dialog dialog)
		{
			await dialog.Msg("FTOWER44_GRITA_01_basic01");
			await dialog.Msg("FTOWER44_GRITA_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_SQ_01")]
		public static async Task FTOWER44_SQ_01(Dialog dialog)
		{
			await dialog.Msg("FTOWER44_SQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_SQ_02")]
		public static async Task FTOWER44_SQ_02(Dialog dialog)
		{
			await dialog.Msg("FTOWER44_SQ_02_IDLE");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_SQ_04")]
		public static async Task FTOWER44_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_SQ_05")]
		public static async Task FTOWER44_SQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_MQ_02")]
		public static async Task FTOWER44_MQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_MQ_03")]
		public static async Task FTOWER44_MQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_44_WARING")]
		public static async Task FTOWER_44_WARING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_MQ_04_NPC")]
		public static async Task FTOWER44_MQ_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER44_GRITA_REMAIN")]
		public static async Task FTOWER44_GRITA_REMAIN(Dialog dialog)
		{
			await dialog.Msg("FTOWER44_GRITA_REMAIN_basic01");
			await dialog.Msg("FTOWER44_GRITA_REMAIN_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_2_PSYCHOKINO_5_1_BOOK_4")]
		public static async Task JOB_2_PSYCHOKINO_5_1_BOOK_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_44230")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_44230(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_173", 1);

		[DialogFunction("HT2_FTOWER_44_TSCOPE")]
		public static async Task HT2_FTOWER_44_TSCOPE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_SQ_01")]
		public static async Task FTOWER45_SQ_01(Dialog dialog)
		{
			await dialog.Msg("FTOWER45_SQ_01_basic01");
			await dialog.Msg("FTOWER45_SQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_SQ_03")]
		public static async Task FTOWER45_SQ_03(Dialog dialog)
		{
			await dialog.Msg("FTOWER45_SQ_03_IDLE");
			await dialog.Msg("FTOWER45_SQ_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_SQ_04")]
		public static async Task FTOWER45_SQ_04(Dialog dialog)
		{
			await dialog.Msg("FTOWER45_SQ_04_IDLE");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_MQ_01_D")]
		public static async Task FTOWER45_MQ_01_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_MQ_02_D")]
		public static async Task FTOWER45_MQ_02_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_MQ_03_D")]
		public static async Task FTOWER45_MQ_03_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_MQ_04_D")]
		public static async Task FTOWER45_MQ_04_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER45_MQ_06_D")]
		public static async Task FTOWER45_MQ_06_D(Dialog dialog)
		{
			await dialog.Msg("FTOWER45_MQ_06_D_basic09");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_FIRETOWER_45")]
		public static async Task WARP_D_FIRETOWER_45(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("FTOWER45_MQ_05_D")]
		public static async Task FTOWER45_MQ_05_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_2_PSYCHOKINO_5_1_BOOK_5")]
		public static async Task JOB_2_PSYCHOKINO_5_1_BOOK_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_45219")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_45219(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_174", 1);

		[DialogFunction("JOB_CLERIC_GRASS")]
		public static async Task JOB_CLERIC_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PRIEST4_1")]
		public static async Task JOB_PRIEST4_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_NPC01")]
		public static async Task PILGRIM46_NPC01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM46_NPC01_basic01");
			await dialog.Msg("PILGRIM46_NPC01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_NPC02")]
		public static async Task PILGRIM46_NPC02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM46_NPC02_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_NPC04")]
		public static async Task PILGRIM46_NPC04(Dialog dialog)
		{
			await dialog.Msg("PILGRIM46_NPC04_BASIC01");
			await dialog.Msg("PILGRIM46_NPC04_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_GRASS")]
		public static async Task PILGRIM46_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_BOSS")]
		public static async Task PILGRIM46_BOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_FOOD02")]
		public static async Task PILGRIM46_FOOD02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_REED")]
		public static async Task PILGRIM46_REED(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_TOMB")]
		public static async Task PILGRIM46_TOMB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM46_ACELLU")]
		public static async Task PILGRIM46_ACELLU(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_4652")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_4652(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "R_TSW03_101", 1, "TreasureboxKey3");

		[DialogFunction("JOB_2_PALADIN5_TRIGGER")]
		public static async Task JOB_2_PALADIN5_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_46700")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_46700(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_177", 1);

		[DialogFunction("PILGRIM47_DMSG")]
		public static async Task PILGRIM47_DMSG(Dialog dialog)
		{
			//SCR_BOOK_GET(self, pc, 'PILGRIM47_DMSG_BOOK', 1);
			//SendAddonMessage(pc, "NOTICE_Dm_Clear", ScpArgMsg("read01"), 5);
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST01")]
		public static async Task PILGRIM47_CRYST01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST02")]
		public static async Task PILGRIM47_CRYST02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST03")]
		public static async Task PILGRIM47_CRYST03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST04")]
		public static async Task PILGRIM47_CRYST04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST05")]
		public static async Task PILGRIM47_CRYST05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST06")]
		public static async Task PILGRIM47_CRYST06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST07")]
		public static async Task PILGRIM47_CRYST07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_SQ_060_SUM")]
		public static async Task PILGRIM47_SQ_060_SUM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_SQ_080_BOX")]
		public static async Task PILGRIM47_SQ_080_BOX(Dialog dialog)
		{
			var character = dialog.Player;
			var npc = dialog.Npc;

			if (!character.SessionObjects.Main.Has(PropertyName.PILGRIM47_SQ_080))
			{
				character.AddSessionObject(PropertyName.PILGRIM47_SQ_080);
				await OpenChest(character, npc);
				dialog.OpenAddon(AddonMessage.NOTICE_Dm_Exclaimation, "@dicID_^*$QUEST_LV_0200_20150918_009109$*^", 7);
			}
			else
				dialog.OpenAddon(AddonMessage.NOTICE_Dm_Exclaimation, "!@#$ALREADY_OPEN#@!", 3);
		}

		[DialogFunction("PILGRIM47_DMSG02")]
		public static async Task PILGRIM47_DMSG02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_DMSG04")]
		public static async Task PILGRIM47_DMSG04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_DMSG03")]
		public static async Task PILGRIM47_DMSG03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_CRYST08")]
		public static async Task PILGRIM47_CRYST08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_DMSG05")]
		public static async Task PILGRIM47_DMSG05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM47_DMSG06")]
		public static async Task PILGRIM47_DMSG06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_4740")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_4740(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_178", 1);

		[DialogFunction("REQ_SEMPLE_04")]
		public static async Task REQ_SEMPLE_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_NPC01")]
		public static async Task PILGRIM50_NPC01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM50_NPC01_basic01");
			await dialog.Msg("PILGRIM50_NPC01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_ALTAR01")]
		public static async Task PILGRIM50_ALTAR01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_ALTAR02")]
		public static async Task PILGRIM50_ALTAR02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_ANGRY01")]
		public static async Task PILGRIM50_ANGRY01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_ANGRY02")]
		public static async Task PILGRIM50_ANGRY02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_ANGRY03")]
		public static async Task PILGRIM50_ANGRY03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_FLOWER")]
		public static async Task PILGRIM50_FLOWER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_ALTAR")]
		public static async Task PILGRIM50_ALTAR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_GHOST")]
		public static async Task PILGRIM50_GHOST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_GHOST2")]
		public static async Task PILGRIM50_GHOST2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_GHOST4")]
		public static async Task PILGRIM50_GHOST4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_ALTAR03")]
		public static async Task PILGRIM50_ALTAR03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_DIARY")]
		public static async Task PILGRIM50_DIARY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM50_LOSTPAPER")]
		public static async Task PILGRIM50_LOSTPAPER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_5041")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_5041(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_179", 1);

		[DialogFunction("REQ_SEMPLE_05")]
		public static async Task REQ_SEMPLE_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_PILGRIMROAD_51")]
		public static async Task WARP_F_PILGRIMROAD_51(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("PILGRIM51_INFO_BOARD")]
		public static async Task PILGRIM51_INFO_BOARD(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_INFO_BOARD_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_MAP_GIVE")]
		public static async Task PILGRIM51_MAP_GIVE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SR01")]
		public static async Task PILGRIM51_SR01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SR02")]
		public static async Task PILGRIM51_SR02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SR03")]
		public static async Task PILGRIM51_SR03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SR04")]
		public static async Task PILGRIM51_SR04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SR05")]
		public static async Task PILGRIM51_SR05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SR06")]
		public static async Task PILGRIM51_SR06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SR07")]
		public static async Task PILGRIM51_SR07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_FGOD01")]
		public static async Task PILGRIM51_FGOD01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_FGOD02")]
		public static async Task PILGRIM51_FGOD02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BRG")]
		public static async Task PILGRIM51_BRG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BOARD01")]
		public static async Task PILGRIM51_BOARD01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_BOARD01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BOARD02")]
		public static async Task PILGRIM51_BOARD02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_BOARD02_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BOARD03")]
		public static async Task PILGRIM51_BOARD03(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_BOARD03_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BOARD04")]
		public static async Task PILGRIM51_BOARD04(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_BOARD04_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BOARD05")]
		public static async Task PILGRIM51_BOARD05(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_BOARD05_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BOARD06")]
		public static async Task PILGRIM51_BOARD06(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_BOARD06_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_BOARD07")]
		public static async Task PILGRIM51_BOARD07(Dialog dialog)
		{
			await dialog.Msg("PILGRIM51_BOARD07_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_REED")]
		public static async Task PILGRIM51_REED(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_FLOWER")]
		public static async Task PILGRIM51_FLOWER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_MAP_TAKE")]
		public static async Task PILGRIM51_MAP_TAKE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_MAGIC")]
		public static async Task PILGRIM51_MAGIC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_SHOP")]
		public static async Task PILGRIM51_SHOP(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM51_LOSTPAPER")]
		public static async Task PILGRIM51_LOSTPAPER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_SORCERER4_1")]
		public static async Task JOB_SORCERER4_1(Dialog dialog)
		{
			await dialog.Msg("JOB_SORCERER4_1_BASIC01");
			await dialog.Msg("JOB_SORCERER4_1_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_SCHWARZEREITER")]
		public static async Task MASTER_SCHWARZEREITER(Dialog dialog)
		{
			await dialog.Msg("MASTER_SCHWARZEREITER_basic");
			await dialog.Msg("MASTER_SCHWARZEREITER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_FENCER")]
		public static async Task MASTER_FENCER(Dialog dialog)
		{
			await dialog.Msg("MASTER_FENCER_basic01");
			await dialog.Msg("MASTER_FENCER_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_FENCER_7_1_WOOD_CARVING")]
		public static async Task JOB_FENCER_7_1_WOOD_CARVING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_5157")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_5157(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_180", 1);

		[DialogFunction("PILGRIM51_HIDDEN_OBJ3")]
		public static async Task PILGRIM51_HIDDEN_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PIED_PIPER_MASTER")]
		public static async Task PIED_PIPER_MASTER(Dialog dialog)
		{
			await dialog.Msg("PIED_PIPER_MASTER_basic1");
			await dialog.Msg("PIED_PIPER_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REQ_SEMPLE_06")]
		public static async Task REQ_SEMPLE_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_NPC01")]
		public static async Task PILGRIM52_NPC01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM52_NPC01_BASIC01");
			await dialog.Msg("PILGRIM52_NPC01_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_NPC02")]
		public static async Task PILGRIM52_NPC02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM52_NPC02_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_TOMB01")]
		public static async Task PILGRIM52_TOMB01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_TOMB02")]
		public static async Task PILGRIM52_TOMB02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_COLUMN01")]
		public static async Task PILGRIM52_COLUMN01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_COLUMN02")]
		public static async Task PILGRIM52_COLUMN02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_COLUMN03")]
		public static async Task PILGRIM52_COLUMN03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_SPRING")]
		public static async Task PILGRIM52_SPRING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_TOMBSTONE")]
		public static async Task PILGRIM52_TOMBSTONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_BIGREED")]
		public static async Task PILGRIM52_BIGREED(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_BAG")]
		public static async Task PILGRIM52_BAG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_POT_GET")]
		public static async Task PILGRIM52_POT_GET(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_WATERPOT01")]
		public static async Task PILGRIM52_WATERPOT01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_WATERPOT02")]
		public static async Task PILGRIM52_WATERPOT02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_WATERPOT03")]
		public static async Task PILGRIM52_WATERPOT03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_PLACE01")]
		public static async Task PILGRIM52_PLACE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_PLACE02")]
		public static async Task PILGRIM52_PLACE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_PLACE03")]
		public static async Task PILGRIM52_PLACE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UPPER_WARNING_F_PILGRIMROAD_52")]
		public static async Task UPPER_WARNING_F_PILGRIMROAD_52(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_BOARD01")]
		public static async Task PILGRIM52_BOARD01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_BOARD02")]
		public static async Task PILGRIM52_BOARD02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_BOARD03")]
		public static async Task PILGRIM52_BOARD03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_BOARD04")]
		public static async Task PILGRIM52_BOARD04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM52_BOX")]
		public static async Task PILGRIM52_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_2_HOPLITE_ARMOR")]
		public static async Task JOB_2_HOPLITE_ARMOR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_5264")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_5264(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_181", 1);

		[DialogFunction("CHATHEDRAL53_MQ_BISHOP")]
		public static async Task CHATHEDRAL53_MQ_BISHOP(Dialog dialog)
		{
			await dialog.Msg("CHATHEDRAL53_MQ_BISHOP_basic01");
			await dialog.Msg("CHATHEDRAL53_MQ_BISHOP_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ01")]
		public static async Task CHATHEDRAL53_MQ01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ02_BOOK1")]
		public static async Task CHATHEDRAL53_MQ02_BOOK1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ02_BOOK2")]
		public static async Task CHATHEDRAL53_MQ02_BOOK2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ02_BOOK3")]
		public static async Task CHATHEDRAL53_MQ02_BOOK3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ02_BOOK4")]
		public static async Task CHATHEDRAL53_MQ02_BOOK4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ02_BOOK5")]
		public static async Task CHATHEDRAL53_MQ02_BOOK5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ02_BOOK6")]
		public static async Task CHATHEDRAL53_MQ02_BOOK6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ03")]
		public static async Task CHATHEDRAL53_MQ03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ04_HINT")]
		public static async Task CHATHEDRAL53_MQ04_HINT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01")]
		public static async Task CHATHEDRAL53_SQ01(Dialog dialog)
		{
			await dialog.Msg("CATHEDRAL53_SQ01_basic01");
			await dialog.Msg("CATHEDRAL53_SQ01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ02")]
		public static async Task CHATHEDRAL53_SQ02(Dialog dialog)
		{
			await dialog.Msg("CHATHEDRAL53_SQ02_basic01");
			await dialog.Msg("CHATHEDRAL53_SQ02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ03")]
		public static async Task CHATHEDRAL53_SQ03(Dialog dialog)
		{
			await dialog.Msg("CATHEDRAL53_SQ03_basic01");
			await dialog.Msg("CATHEDRAL53_SQ03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ06_HINT01")]
		public static async Task CHATHEDRAL53_MQ06_HINT01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ06_HINT02")]
		public static async Task CHATHEDRAL53_MQ06_HINT02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ03_AFTER")]
		public static async Task CHATHEDRAL53_SQ03_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ02_AFTER")]
		public static async Task CHATHEDRAL53_SQ02_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT01")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT02")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT03")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT04")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT05")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT06")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT07")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_SQ01_OBJECT08")]
		public static async Task CHATHEDRAL53_SQ01_OBJECT08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_BOOK")]
		public static async Task CHATHEDRAL54_BOOK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_BOOK02")]
		public static async Task CHATHEDRAL54_BOOK02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_BOOK03")]
		public static async Task CHATHEDRAL54_BOOK03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL53_MQ06_PUZZLE")]
		public static async Task CHATHEDRAL53_MQ06_PUZZLE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CATHEDRAL_5363")]
		public static async Task TREASUREBOX_LV_D_CATHEDRAL_5363(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_183", 1);

		[DialogFunction("CHATHEDRAL53_SQ07_OBJ1")]
		public static async Task CHATHEDRAL53_SQ07_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_SQ01_PART1")]
		public static async Task CHATHEDRAL54_SQ01_PART1(Dialog dialog)
		{
			await dialog.Msg("CATHEDRAL54_SQ01_PART1_basic01");
			await dialog.Msg("CHATHEDRAL54_SQ01_PART1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_SQ03_PART1")]
		public static async Task CHATHEDRAL54_SQ03_PART1(Dialog dialog)
		{
			await dialog.Msg("CATHEDRAL54_SQ03_PART1_basic01");
			await dialog.Msg("CATHEDRAL54_SQ03_PART1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_SQ04_PART2")]
		public static async Task CHATHEDRAL54_SQ04_PART2(Dialog dialog)
		{
			await dialog.Msg("CATHEDRAL54_SQ04_PART2_basic01");
			await dialog.Msg("CATHEDRAL54_SQ04_PART2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_MQ04_PART2")]
		public static async Task CHATHEDRAL54_MQ04_PART2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_PART1_BISHOP")]
		public static async Task CHATHEDRAL54_PART1_BISHOP(Dialog dialog)
		{
			await dialog.Msg("CHATHEDRAL54_PART1_BISHOP_basic01");
			await dialog.Msg("CHATHEDRAL54_PART1_BISHOP_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_SQ01_PART1_BOOK1")]
		public static async Task CHATHEDRAL54_SQ01_PART1_BOOK1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_SQ01_PART1_BOOK2")]
		public static async Task CHATHEDRAL54_SQ01_PART1_BOOK2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_SQ01_PART1_BOOK3")]
		public static async Task CHATHEDRAL54_SQ01_PART1_BOOK3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_BISHOP_AFTER")]
		public static async Task CHATHEDRAL54_BISHOP_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_MQ01_PUZZLE")]
		public static async Task CHATHEDRAL54_MQ01_PUZZLE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_MQ06_BOOK")]
		public static async Task CHATHEDRAL54_MQ06_BOOK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_POTAL")]
		public static async Task CHATHEDRAL54_POTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL54_SQ01_PART1_BOOK4")]
		public static async Task CHATHEDRAL54_SQ01_PART1_BOOK4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATHEDRAL54MQ02_OBJECT")]
		public static async Task CATHEDRAL54MQ02_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MQ05_PROOF_PRIST")]
		public static async Task MQ05_PROOF_PRIST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CATHEDRAL_5459")]
		public static async Task TREASUREBOX_LV_D_CATHEDRAL_5459(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_184", 1);

		[DialogFunction("JOB_ARCHER4_2")]
		public static async Task JOB_ARCHER4_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_PILGRIMROAD_55")]
		public static async Task WARP_F_PILGRIMROAD_55(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("PILGRIMROAD55_SQ02")]
		public static async Task PILGRIMROAD55_SQ02(Dialog dialog)
		{
			await dialog.Msg("PILGRIMROAD55_SQ02_basic01");
			await dialog.Msg("PILGRIMROAD55_SQ02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_SQ01")]
		public static async Task PILGRIMROAD55_SQ01(Dialog dialog)
		{
			await dialog.Msg("PILGRIMROAD55_SQ01_basic01");
			await dialog.Msg("PILGRIMROAD55_SQ01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_SQ09")]
		public static async Task PILGRIMROAD55_SQ09(Dialog dialog)
		{
			await dialog.Msg("PILGRIMROAD55_SQ09_basic01");
			await dialog.Msg("PILGRIMROAD55_SQ09_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_SQ05")]
		public static async Task PILGRIMROAD55_SQ05(Dialog dialog)
		{
			await dialog.Msg("PILGRIMROAD55_SQ05_basic01");
			await dialog.Msg("PILGRIMROAD55_SQ05_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BISHOP_TOMB")]
		public static async Task BISHOP_TOMB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_SQ11")]
		public static async Task PILGRIMROAD55_SQ11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_SQ02_HERB")]
		public static async Task PILGRIMROAD55_SQ02_HERB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_ALTAR01")]
		public static async Task PILGRIMROAD55_ALTAR01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_ALTAR02")]
		public static async Task PILGRIMROAD55_ALTAR02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_ALTAR03")]
		public static async Task PILGRIMROAD55_ALTAR03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_ALTAR04")]
		public static async Task PILGRIMROAD55_ALTAR04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SQ09_DEAD_PRIST")]
		public static async Task SQ09_DEAD_PRIST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD55_SQ02_AFTER")]
		public static async Task PILGRIMROAD55_SQ02_AFTER(Dialog dialog)
		{
			await dialog.Msg("PILGRIMROAD55_SQ02_AFTER_basic01");
			await dialog.Msg("PILGRIMROAD55_SQ02_AFTER_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_5539")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_5539(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "Hat_628053", 1, "TreasureboxKey4");

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_55900")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_55900(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_185", 1);

		[DialogFunction("CHATHEDRAL54_BLACKMAN")]
		public static async Task CHATHEDRAL54_BLACKMAN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_MQ_BISHOP")]
		public static async Task CHATHEDRAL56_MQ_BISHOP(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_MQ01_PAPER")]
		public static async Task CHATHEDRAL56_MQ01_PAPER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_MQ01_BOOK")]
		public static async Task CHATHEDRAL56_MQ01_BOOK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_MQ02")]
		public static async Task CHATHEDRAL56_MQ02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_MQ05_PUZZLE")]
		public static async Task CHATHEDRAL56_MQ05_PUZZLE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_SQ03")]
		public static async Task CHATHEDRAL56_SQ03(Dialog dialog)
		{
			await dialog.Msg("CHATHEDRAL56_SQ03_basic01");
			await dialog.Msg("CHATHEDRAL56_SQ03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_NPC_HIDE")]
		public static async Task CHATHEDRAL56_NPC_HIDE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_MQ08_POTAL")]
		public static async Task CHATHEDRAL56_MQ08_POTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_MQ_BISHOP_AFTER")]
		public static async Task CHATHEDRAL56_MQ_BISHOP_AFTER(Dialog dialog)
		{
			await dialog.Msg("CHATHEDRAL56_MQ_BISHOP_AFTER_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56SQ04_NPC")]
		public static async Task CHATHEDRAL56SQ04_NPC(Dialog dialog)
		{
			await dialog.Msg("CHATHEDRAL56SQ04_NPC_basic01");
			await dialog.Msg("CHATHEDRAL56SQ04_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_SQ01")]
		public static async Task CHATHEDRAL56_SQ01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHATHEDRAL56_SQ02_KILL")]
		public static async Task CHATHEDRAL56_SQ02_KILL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_CATHEDRAL_5648")]
		public static async Task TREASUREBOX_LV_D_CATHEDRAL_5648(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_186", 1);

		[DialogFunction("CHATHEDRAL56_SQ05_OBJ1")]
		public static async Task CHATHEDRAL56_SQ05_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_MQ01_NPC")]
		public static async Task SIAULIAI_46_1_MQ01_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_1_MQ01_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_1_MQ01_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_ALTAR")]
		public static async Task SIAULIAI_46_1_ALTAR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_DEADTREE01")]
		public static async Task SIAULIAI_46_1_DEADTREE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_DEADTREE02")]
		public static async Task SIAULIAI_46_1_DEADTREE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_SQ_03_NPC")]
		public static async Task SIAULIAI_46_1_SQ_03_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_1_SQ_03_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_1_SQ_03_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_SQ_04_NPC")]
		public static async Task SIAULIAI_46_1_SQ_04_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_1_SQ_04_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_1_SQ_04_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_SQ_05_NPC")]
		public static async Task SIAULIAI_46_1_SQ_05_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_1_SQ_05_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_1_SQ_05_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_SQ_03_BAG01")]
		public static async Task SIAULIAI_46_1_SQ_03_BAG01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_SQ_03_BAG02")]
		public static async Task SIAULIAI_46_1_SQ_03_BAG02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_1_SQ_03_BAG03")]
		public static async Task SIAULIAI_46_1_SQ_03_BAG03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI46_1_HIDDEN_EVENT")]
		public static async Task SIAULIAI46_1_HIDDEN_EVENT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_46_127")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_46_127(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_198", 1);

		[DialogFunction("CHAR420_STEP322_NPC1")]
		public static async Task CHAR420_STEP322_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR420_STEP322_NPC2")]
		public static async Task CHAR420_STEP322_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR420_STEP322_NPC3")]
		public static async Task CHAR420_STEP322_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR420_STEP322_NPC4")]
		public static async Task CHAR420_STEP322_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR420_STEP322_NPC5")]
		public static async Task CHAR420_STEP322_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_MQ01_NPC")]
		public static async Task SIAULIAI_46_2_MQ01_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_2_MQ01_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_2_MQ01_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_AUSTEJA")]
		public static async Task SIAULIAI_46_2_AUSTEJA(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_2_AUSTEJA_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_SEAL")]
		public static async Task SIAULIAI_46_2_SEAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_SQ_03_NPC")]
		public static async Task SIAULIAI_46_2_SQ_03_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_2_SQ_03_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_2_SQ_03_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_SQ_05_NPC")]
		public static async Task SIAULIAI_46_2_SQ_05_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_2_SQ_05_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_SQ_01_NPC")]
		public static async Task SIAULIAI_46_2_SQ_01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_BEETREE")]
		public static async Task SIAULIAI_46_2_BEETREE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_SQ_04_01")]
		public static async Task SIAULIAI_46_2_SQ_04_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_GUARDIAN")]
		public static async Task SIAULIAI_46_2_GUARDIAN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_2_WOODPIECE")]
		public static async Task SIAULIAI_46_2_WOODPIECE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_46_227")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_46_227(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_197", 1);

		[DialogFunction("SIAULIAI462_HIDDENQ1_PLANT1")]
		public static async Task SIAULIAI462_HIDDENQ1_PLANT1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI462_HIDDENQ1_PLANT2")]
		public static async Task SIAULIAI462_HIDDENQ1_PLANT2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_MQ01_NPC")]
		public static async Task SIAULIAI_46_3_MQ01_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_3_MQ01_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_3_MQ01_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_SQ_03_NPC")]
		public static async Task SIAULIAI_46_3_SQ_03_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_3_SQ_03_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_3_SQ_03_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_AUSTEJA_ALTAR_01")]
		public static async Task SIAULIAI_46_3_AUSTEJA_ALTAR_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_AUSTEJA_ALTAR_02")]
		public static async Task SIAULIAI_46_3_AUSTEJA_ALTAR_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_MQ05_NPC")]
		public static async Task SIAULIAI_46_3_MQ05_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_3_MQ05_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_3_MQ05_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_SQ_02_NPC")]
		public static async Task SIAULIAI_46_3_SQ_02_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_3_SQ_02_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_3_SQ_02_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_SQ_04_FARM")]
		public static async Task SIAULIAI_46_3_SQ_04_FARM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_BEEHIVE")]
		public static async Task SIAULIAI_46_3_BEEHIVE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_SQ_02_KIT")]
		public static async Task SIAULIAI_46_3_SQ_02_KIT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_INV_NPC_01")]
		public static async Task SIAULIAI_46_3_INV_NPC_01(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_3_INV_NPC_01_basic01");
			await dialog.Msg("SIAULIAI_46_3_INV_NPC_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_3_INV_NPC_02")]
		public static async Task SIAULIAI_46_3_INV_NPC_02(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_3_INV_NPC_02_basic01");
			await dialog.Msg("SIAULIAI_46_3_INV_NPC_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_46_323")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_46_323(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_196", 1);

		[DialogFunction("WARP_F_SIAULIAI_46_4")]
		public static async Task WARP_F_SIAULIAI_46_4(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("SIAULIAI_46_4_MQ01_NPC")]
		public static async Task SIAULIAI_46_4_MQ01_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_4_MQ01_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_4_MQ01_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_4_SQ03_NPC")]
		public static async Task SIAULIAI_46_4_SQ03_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_4_SQ03_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_4_SQ03_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_4_BEEHIVE01")]
		public static async Task SIAULIAI_46_4_BEEHIVE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_4_MEADBARREL")]
		public static async Task SIAULIAI_46_4_MEADBARREL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_4_MEADBOX")]
		public static async Task SIAULIAI_46_4_MEADBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_4_MQ04_NPC")]
		public static async Task SIAULIAI_46_4_MQ04_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_4_MQ04_NPC_basic01");
			await dialog.Msg("SIAULIAI_46_4_MQ04_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_4_SQ04_NPC01")]
		public static async Task SIAULIAI_46_4_SQ04_NPC01(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_4_SQ04_NPC01_basic01");
			await dialog.Msg("SIAULIAI_46_4_SQ04_NPC01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_46_4_SQ04_NPC02")]
		public static async Task SIAULIAI_46_4_SQ04_NPC02(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_46_4_SQ04_NPC02_basic01");
			await dialog.Msg("SIAULIAI_46_4_SQ04_NPC02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_46_434")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_46_434(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_195", 1);

		[DialogFunction("SIAULIAI_46_4_TO_STARTOWER_76_1")]
		public static async Task SIAULIAI_46_4_TO_STARTOWER_76_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI462_HIDDENQ2_CANDLE1")]
		public static async Task SIAULIAI462_HIDDENQ2_CANDLE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI462_HIDDENQ2_CANDLE2")]
		public static async Task SIAULIAI462_HIDDENQ2_CANDLE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI462_HIDDENQ2_CANDLE3")]
		public static async Task SIAULIAI462_HIDDENQ2_CANDLE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT3_SIAULIAI_46_4_DRUM")]
		public static async Task HT3_SIAULIAI_46_4_DRUM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FREE_DUNGEON_SIGN2")]
		public static async Task FREE_DUNGEON_SIGN2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP5_5_NPC1")]
		public static async Task CHAR120_MSTEP5_5_NPC1(Dialog dialog)
		{
			await dialog.Msg("CHAR120_MSTEP5_5_NPC1_basic1");
			await dialog.Msg("CHAR120_MSTEP5_5_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_TADAS")]
		public static async Task FARM47_TADAS(Dialog dialog)
		{
			await dialog.Msg("FARM47_TADAS_BASIC01");
			await dialog.Msg("FARM47_TADAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_STEPAS")]
		public static async Task FARM47_STEPAS(Dialog dialog)
		{
			await dialog.Msg("FARM47_STEPAS_BASIC01");
			await dialog.Msg("FARM47_STEPAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_JAUNIUS")]
		public static async Task FARM47_JAUNIUS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MARIUS")]
		public static async Task FARM47_MARIUS(Dialog dialog)
		{
			await dialog.Msg("FARM47_MARIUS_BASIC01");
			await dialog.Msg("FARM47_MARIUS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DALIUS")]
		public static async Task FARM47_DALIUS(Dialog dialog)
		{
			await dialog.Msg("FARM47_DALIUS_basic01");
			await dialog.Msg("FARM47_DALIUS_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DOWN_SACK_D")]
		public static async Task FARM47_DOWN_SACK_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_SCORPIO")]
		public static async Task FARM47_SCORPIO(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_GRASS01")]
		public static async Task FARM47_GRASS01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_GRASS02")]
		public static async Task FARM47_GRASS02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DEVINEMARK01")]
		public static async Task FARM47_DEVINEMARK01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DEVINEMARK02")]
		public static async Task FARM47_DEVINEMARK02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DEVINEMARK03")]
		public static async Task FARM47_DEVINEMARK03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_47_434")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_47_434(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_156", 1);

		[DialogFunction("SIAU474_RP_1_NPC")]
		public static async Task SIAU474_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAU474_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_47_436")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_47_436(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_255", 1);

		[DialogFunction("PILGRIM41_1_NUMBER_MIXING")]
		public static async Task PILGRIM41_1_NUMBER_MIXING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM41_1_CURLING")]
		public static async Task PILGRIM41_1_CURLING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DELIVERY_CHARGES_GAME")]
		public static async Task DELIVERY_CHARGES_GAME(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_01")]
		public static async Task PILGRIM411_SQ_01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM411_SQ_01_basic1");
			await dialog.Msg("PILGRIM411_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_02")]
		public static async Task PILGRIM411_SQ_02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM411_SQ_02_basic1");
			await dialog.Msg("PILGRIM411_SQ_02_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_03")]
		public static async Task PILGRIM411_SQ_03(Dialog dialog)
		{
			await dialog.Msg("PILGRIM411_SQ_03_basic1");
			await dialog.Msg("PILGRIM411_SQ_03_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_06")]
		public static async Task PILGRIM411_SQ_06(Dialog dialog)
		{
			await dialog.Msg("PILGRIM411_SQ_06_basic1");
			await dialog.Msg("PILGRIM411_SQ_06_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_07")]
		public static async Task PILGRIM411_SQ_07(Dialog dialog)
		{
			await dialog.Msg("PILGRIM411_SQ_07_basic1");
			await dialog.Msg("PILGRIM411_SQ_07_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_09")]
		public static async Task PILGRIM411_SQ_09(Dialog dialog)
		{
			await dialog.Msg("PILGRIM411_SQ_09_basic1");
			await dialog.Msg("PILGRIM411_SQ_09_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_08")]
		public static async Task PILGRIM411_SQ_08(Dialog dialog)
		{
			await dialog.Msg("PILGRIM411_SQ_08_basic1");
			await dialog.Msg("PILGRIM411_SQ_08_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT3_PILGRIM41_1_WELLROPE")]
		public static async Task HT3_PILGRIM41_1_WELLROPE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_02_1")]
		public static async Task PILGRIM411_SQ_02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_04")]
		public static async Task PILGRIM411_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM411_SQ_08_1")]
		public static async Task PILGRIM411_SQ_08_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_01")]
		public static async Task PILGRIM412_SQ_01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_01_basic1");
			await dialog.Msg("PILGRIM412_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_02")]
		public static async Task PILGRIM412_SQ_02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_02_basic1");
			await dialog.Msg("PILGRIM412_SQ_02_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_03")]
		public static async Task PILGRIM412_SQ_03(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_03_basic1");
			await dialog.Msg("PILGRIM412_SQ_03_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_05")]
		public static async Task PILGRIM412_SQ_05(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_05_basic2");
			await dialog.Msg("PILGRIM412_SQ_05_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_04")]
		public static async Task PILGRIM412_SQ_04(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_04_basic1");
			await dialog.Msg("PILGRIM412_SQ_04_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_07")]
		public static async Task PILGRIM412_SQ_07(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_07_basic1");
			await dialog.Msg("PILGRIM412_SQ_07_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_02_1")]
		public static async Task PILGRIM412_SQ_02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_09")]
		public static async Task PILGRIM412_SQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_04_2")]
		public static async Task PILGRIM412_SQ_04_2(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_01_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_04_3")]
		public static async Task PILGRIM412_SQ_04_3(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_02_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_04_4")]
		public static async Task PILGRIM412_SQ_04_4(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_03_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_04_5")]
		public static async Task PILGRIM412_SQ_04_5(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_04_patient01");
			await dialog.Msg("PILGRIM412_SQ_04_patient02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_SQ_04_6")]
		public static async Task PILGRIM412_SQ_04_6(Dialog dialog)
		{
			await dialog.Msg("PILGRIM412_SQ_04_patient01");
			await dialog.Msg("PILGRIM412_SQ_04_patient02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM412_RMG_NPC_1")]
		public static async Task PILGRIM412_RMG_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_PILGRIMROAD_41_3")]
		public static async Task WARP_PILGRIMROAD_41_3(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("PILGRIM41_3_PAIR")]
		public static async Task PILGRIM41_3_PAIR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM41_3_CURLING")]
		public static async Task PILGRIM41_3_CURLING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_02")]
		public static async Task PILGRIM413_SQ_02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_02_basic1");
			await dialog.Msg("PILGRIM413_SQ_02_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_06")]
		public static async Task PILGRIM413_SQ_06(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_06_basic1");
			await dialog.Msg("PILGRIM413_SQ_06_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_08")]
		public static async Task PILGRIM413_SQ_08(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_08_basic1");
			await dialog.Msg("PILGRIM413_SQ_08_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_08_1")]
		public static async Task PILGRIM413_SQ_08_1(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_08_basic1");
			await dialog.Msg("PILGRIM413_SQ_08_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_07")]
		public static async Task PILGRIM413_SQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_06_1")]
		public static async Task PILGRIM413_SQ_06_1(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_06_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_06_2")]
		public static async Task PILGRIM413_SQ_06_2(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_06_2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_06_3")]
		public static async Task PILGRIM413_SQ_06_3(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_06_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_06_4")]
		public static async Task PILGRIM413_SQ_06_4(Dialog dialog)
		{
			await dialog.Msg("PILGRIM413_SQ_06_4_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_03_1")]
		public static async Task PILGRIM413_SQ_03_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_03_2")]
		public static async Task PILGRIM413_SQ_03_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_03_3")]
		public static async Task PILGRIM413_SQ_03_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_SQ_03_4")]
		public static async Task PILGRIM413_SQ_03_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM413_KQ_1_BOX")]
		public static async Task PILGRIM413_KQ_1_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_SHADOWMANCER_MASTER")]
		public static async Task JOB_SHADOWMANCER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_SHADOWMANCER_NML_DLG");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_SHADOWMASTER_Q1_OBJECT")]
		public static async Task JOB_SHADOWMASTER_Q1_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR220_MSETP2_4_NPC")]
		public static async Task CHAR220_MSETP2_4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD_RAID_LEGEND_PORTAL")]
		public static async Task PILGRIMROAD_RAID_LEGEND_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM41_4_HIGH_LOW")]
		public static async Task PILGRIM41_4_HIGH_LOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_01")]
		public static async Task PILGRIM414_SQ_01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM414_SQ_01_basic1");
			await dialog.Msg("PILGRIM414_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_02")]
		public static async Task PILGRIM414_SQ_02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM414_SQ_02_basic1");
			await dialog.Msg("PILGRIM414_SQ_02_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_02_1")]
		public static async Task PILGRIM414_SQ_02_1(Dialog dialog)
		{
			await dialog.Msg("PILGRIM414_SQ_02_basic3");
			await dialog.Msg("PILGRIM414_SQ_02_basic4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_03")]
		public static async Task PILGRIM414_SQ_03(Dialog dialog)
		{
			await dialog.Msg("PILGRIM414_SQ_03_basic1");
			await dialog.Msg("PILGRIM414_SQ_03_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_04")]
		public static async Task PILGRIM414_SQ_04(Dialog dialog)
		{
			await dialog.Msg("PILGRIM414_SQ_04_basic1");
			await dialog.Msg("PILGRIM414_SQ_04_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_08")]
		public static async Task PILGRIM414_SQ_08(Dialog dialog)
		{
			await dialog.Msg("PILGRIM414_SQ_08_basic1");
			await dialog.Msg("PILGRIM414_SQ_08_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_05")]
		public static async Task PILGRIM414_SQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_03_1")]
		public static async Task PILGRIM414_SQ_03_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM414_SQ_09")]
		public static async Task PILGRIM414_SQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_01")]
		public static async Task PILGRIM415_SQ_01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM415_SQ_01_basic1");
			await dialog.Msg("PILGRIM415_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_09")]
		public static async Task PILGRIM415_SQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_10")]
		public static async Task PILGRIM415_SQ_10(Dialog dialog)
		{
			await dialog.Msg("PILGRIM415_SQ_10_basic1");
			await dialog.Msg("PILGRIM415_SQ_10_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_03")]
		public static async Task PILGRIM415_SQ_03(Dialog dialog)
		{
			await dialog.Msg("PILGRIM415_SQ_03_basic1");
			await dialog.Msg("PILGRIM415_SQ_03_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_06")]
		public static async Task PILGRIM415_SQ_06(Dialog dialog)
		{
			await dialog.Msg("PILGRIM415_SQ_06_basic1");
			await dialog.Msg("PILGRIM415_SQ_06_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_06_2")]
		public static async Task PILGRIM415_SQ_06_2(Dialog dialog)
		{
			await dialog.Msg("PILGRIM415_SQ_06_basic5");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_08")]
		public static async Task PILGRIM415_SQ_08(Dialog dialog)
		{
			await dialog.Msg("PILGRIM415_SQ_08_basic1");
			await dialog.Msg("PILGRIM415_SQ_08_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_08_1")]
		public static async Task PILGRIM415_SQ_08_1(Dialog dialog)
		{
			await dialog.Msg("PILGRIM415_SQ_08_basic3");
			await dialog.Msg("PILGRIM415_SQ_08_basic4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_02")]
		public static async Task PILGRIM415_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_05")]
		public static async Task PILGRIM415_SQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_07")]
		public static async Task PILGRIM415_SQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_10_1")]
		public static async Task PILGRIM415_SQ_10_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_10_2")]
		public static async Task PILGRIM415_SQ_10_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_10_3")]
		public static async Task PILGRIM415_SQ_10_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_SQ_11")]
		public static async Task PILGRIM415_SQ_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM415_TREE")]
		public static async Task PILGRIM415_TREE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DICE_MATCHING")]
		public static async Task DICE_MATCHING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_02")]
		public static async Task ABBEY416_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_03")]
		public static async Task ABBEY416_SQ_03(Dialog dialog)
		{
			await dialog.Msg("ABBEY416_SQ_03_basic1");
			await dialog.Msg("ABBEY416_SQ_03_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_08")]
		public static async Task ABBEY416_SQ_08(Dialog dialog)
		{
			await dialog.Msg("ABBEY416_SQ_08_basic1");
			await dialog.Msg("ABBEY416_SQ_08_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_09_1")]
		public static async Task ABBEY416_SQ_09_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_09_2")]
		public static async Task ABBEY416_SQ_09_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_09_3")]
		public static async Task ABBEY416_SQ_09_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_01")]
		public static async Task ABBEY416_SQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_06")]
		public static async Task ABBEY416_SQ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_09")]
		public static async Task ABBEY416_SQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY416_SQ_10")]
		public static async Task ABBEY416_SQ_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_INQUGITOR_ITEM")]
		public static async Task JOB_INQUGITOR_ITEM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_BENJAMINAS")]
		public static async Task FLASH59_BENJAMINAS(Dialog dialog)
		{
			await dialog.Msg("FLASH59_SOLDIER_basic01");
			await dialog.Msg("FLASH59_SOLDIER_basic02");
			await dialog.Msg("FLASH59_SOLDIER_basic03");
			await dialog.Msg("FLASH59_SOLDIER_basic04");
			await dialog.Msg("FLASH59_SOLDIER_basic05");
			await dialog.Msg("FLASH59_SOLDIER_basic06");
			await dialog.Msg("FLASH59_SOLDIER_basic07");
			await dialog.Msg("FLASH59_SOLDIER_basic08");
			await dialog.Msg("FLASH59_SOLDIER_basic09");
			await dialog.Msg("FLASH59_SOLDIER_basic10");
			await dialog.Msg("FLASH59_SOLDIER_basic11");
			await dialog.Msg("FLASH59_SOLDIER_basic12");
			await dialog.Msg("FLASH59_SOLDIER_basic13");
			await dialog.Msg("FLASH59_SOLDIER_basic14");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_RETIA")]
		public static async Task FLASH59_RETIA(Dialog dialog)
		{
			await dialog.Msg("FLASH59_RETIA_basic_01");
			await dialog.Msg("FLASH59_RETIA_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_HUBERTAS")]
		public static async Task FLASH59_HUBERTAS(Dialog dialog)
		{
			await dialog.Msg("FLASH59_HUBERTAS_basic_01");
			await dialog.Msg("FLASH59_HUBERTAS_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_SOLDIER")]
		public static async Task FLASH59_SOLDIER(Dialog dialog)
		{
			await dialog.Msg("FLASH59_SOLDIER_basic01");
			await dialog.Msg("FLASH59_SOLDIER_basic02");
			await dialog.Msg("FLASH59_SOLDIER_basic03");
			await dialog.Msg("FLASH59_SOLDIER_basic04");
			await dialog.Msg("FLASH59_SOLDIER_basic05");
			await dialog.Msg("FLASH59_SOLDIER_basic06");
			await dialog.Msg("FLASH59_SOLDIER_basic07");
			await dialog.Msg("FLASH59_SOLDIER_basic08");
			await dialog.Msg("FLASH59_SOLDIER_basic09");
			await dialog.Msg("FLASH59_SOLDIER_basic10");
			await dialog.Msg("FLASH59_SOLDIER_basic11");
			await dialog.Msg("FLASH59_SOLDIER_basic12");
			await dialog.Msg("FLASH59_SOLDIER_basic13");
			await dialog.Msg("FLASH59_SOLDIER_basic14");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_SQ_03_1")]
		public static async Task FLASH59_SQ_03_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_SQ_03_2")]
		public static async Task FLASH59_SQ_03_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_SQ_03_5")]
		public static async Task FLASH59_SQ_03_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_SQ_03_3")]
		public static async Task FLASH59_SQ_03_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_SQ_03_4")]
		public static async Task FLASH59_SQ_03_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH59_SQ_08_NPC")]
		public static async Task FLASH59_SQ_08_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FLASH_591000")]
		public static async Task TREASUREBOX_LV_F_FLASH_591000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_209", 1);

		[DialogFunction("JOB_CLERIC5_1_TRACK")]
		public static async Task JOB_CLERIC5_1_TRACK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_GOFDEN")]
		public static async Task FLASH60_GOFDEN(Dialog dialog)
		{
			await dialog.Msg("FLASH60_GOFDEN_basic_01");
			await dialog.Msg("FLASH60_GOFDEN_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_KOSTAS")]
		public static async Task FLASH60_KOSTAS(Dialog dialog)
		{
			await dialog.Msg("FLASH60_KOSTAS_basic_01");
			await dialog.Msg("FLASH60_KOSTAS_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_RUDOLFAS")]
		public static async Task FLASH60_RUDOLFAS(Dialog dialog)
		{
			await dialog.Msg("FLASH60_RUDOLFAS_basic_01");
			await dialog.Msg("FLASH60_RUDOLFAS_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_SQ_02")]
		public static async Task FLASH60_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_SQ_03")]
		public static async Task FLASH60_SQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_SQ_06_NPC")]
		public static async Task FLASH60_SQ_06_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_SQ_08_NPC")]
		public static async Task FLASH60_SQ_08_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH60_SQ_09_NPC")]
		public static async Task FLASH60_SQ_09_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FLASH_601000")]
		public static async Task TREASUREBOX_LV_F_FLASH_601000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_210", 1);

		[DialogFunction("WARP_F_FLASH_61")]
		public static async Task WARP_F_FLASH_61(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("FLASH61_SABINA")]
		public static async Task FLASH61_SABINA(Dialog dialog)
		{
			await dialog.Msg("FLASH61_SABINA_basic_01");
			await dialog.Msg("FLASH61_SABINA_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_NOBAVIS")]
		public static async Task FLASH61_NOBAVIS(Dialog dialog)
		{
			await dialog.Msg("FLASH61_NOBAVIS_basic_01");
			await dialog.Msg("FLASH61_NOBAVIS_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_LAIL")]
		public static async Task FLASH61_LAIL(Dialog dialog)
		{
			await dialog.Msg("FLASH61_LAIL_basic_01");
			await dialog.Msg("FLASH61_LAIL_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_SQ_09_1")]
		public static async Task FLASH61_SQ_09_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_SQ_09_2")]
		public static async Task FLASH61_SQ_09_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_SQ_09_3")]
		public static async Task FLASH61_SQ_09_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_SQ_09_4")]
		public static async Task FLASH61_SQ_09_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_SQ_07_NPC")]
		public static async Task FLASH61_SQ_07_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH61_SQ_04_NPC")]
		public static async Task FLASH61_SQ_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FLASH_611000")]
		public static async Task TREASUREBOX_LV_F_FLASH_611000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_211", 1);

		[DialogFunction("FLASH_61_TO_VELNIASPRISON_77_1")]
		public static async Task FLASH_61_TO_VELNIASPRISON_77_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_ROFHDEL")]
		public static async Task FLASH63_ROFHDEL(Dialog dialog)
		{
			await dialog.Msg("FLASH63_ROFHDEL_basic_01");
			await dialog.Msg("FLASH63_ROFHDEL_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_HANS")]
		public static async Task FLASH63_HANS(Dialog dialog)
		{
			await dialog.Msg("FLASH63_HANS_basic_01");
			await dialog.Msg("FLASH63_HANS_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_STEPONAS")]
		public static async Task FLASH63_STEPONAS(Dialog dialog)
		{
			await dialog.Msg("FLASH63_STEPONAS_basic_01");
			await dialog.Msg("FLASH63_STEPONAS_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SOLDIER03")]
		public static async Task FLASH63_SOLDIER03(Dialog dialog)
		{
			await dialog.Msg("FLASH63_SOLDIER03_basic01");
			await dialog.Msg("FLASH63_SOLDIER03_basic02");
			await dialog.Msg("FLASH63_SOLDIER03_basic03");
			await dialog.Msg("FLASH63_SOLDIER03_basic04");
			await dialog.Msg("FLASH63_SOLDIER03_basic05");
			await dialog.Msg("FLASH63_SOLDIER03_basic06");
			await dialog.Msg("FLASH63_SOLDIER03_basic07");
			await dialog.Msg("FLASH63_SOLDIER03_basic08");
			await dialog.Msg("FLASH63_SOLDIER03_basic09");
			await dialog.Msg("FLASH63_SOLDIER03_basic10");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_08_NPC")]
		public static async Task FLASH63_SQ_08_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_10_NPC")]
		public static async Task FLASH63_SQ_10_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SOLDIER04")]
		public static async Task FLASH63_SOLDIER04(Dialog dialog)
		{
			await dialog.Msg("FLASH63_SOLDIER04_basic01");
			await dialog.Msg("FLASH63_SOLDIER04_basic02");
			await dialog.Msg("FLASH63_SOLDIER04_basic03");
			await dialog.Msg("FLASH63_SOLDIER04_basic04");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_04_NPC")]
		public static async Task FLASH63_SQ_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_03_NPC")]
		public static async Task FLASH63_SQ_03_NPC(Dialog dialog)
		{
			await dialog.Msg("FLASH63_SOLDIER02_basic01");
			await dialog.Msg("FLASH63_SOLDIER02_basic02");
			await dialog.Msg("FLASH63_SOLDIER04_basic01");
			await dialog.Msg("FLASH63_SOLDIER04_basic02");
			await dialog.Msg("FLASH63_SOLDIER04_basic03");
			await dialog.Msg("FLASH63_SOLDIER04_basic04");
			await dialog.Msg("FLASH63_SOLDIER04_basic05");
			await dialog.Msg("FLASH63_SOLDIER04_basic06");
			await dialog.Msg("FLASH63_SOLDIER04_basic07");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_12_01_NPC")]
		public static async Task FLASH63_SQ_12_01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_12_02_NPC")]
		public static async Task FLASH63_SQ_12_02_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_12_03_NPC")]
		public static async Task FLASH63_SQ_12_03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ_12_04_NPC")]
		public static async Task FLASH63_SQ_12_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FLASH_631000")]
		public static async Task TREASUREBOX_LV_F_FLASH_631000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_212", 1);

		[DialogFunction("FLASH63_SQ13")]
		public static async Task FLASH63_SQ13(Dialog dialog)
		{
			await dialog.Msg("FLASH63_SQ13_basic01");
			await dialog.Msg("FLASH63_SQ13_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH29_1_SQ_090")]
		public static async Task FLASH29_1_SQ_090(Dialog dialog)
		{
			await dialog.Msg("FLASH29_1_SQ_090_basic01");
			await dialog.Msg("FLASH29_1_SQ_090_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ13_OBJ1")]
		public static async Task FLASH63_SQ13_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH63_SQ14_OBJ")]
		public static async Task FLASH63_SQ14_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_FLASH_64_EV_55_001")]
		public static async Task F_FLASH_64_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("FLASH64_KARRIAT")]
		public static async Task FLASH64_KARRIAT(Dialog dialog)
		{
			await dialog.Msg("FLASH64_KARRIAT_basic_01");
			await dialog.Msg("FLASH64_KARRIAT_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CANNONEER_MASTER")]
		public static async Task CANNONEER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_CANNONEER_MASTER_basic1");
			await dialog.Msg("JOB_CANNONEER_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MUSKETEER_MASTER")]
		public static async Task MUSKETEER_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_MUSKETEER_MASTER_basic1");
			await dialog.Msg("JOB_MUSKETEER_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_EDITA")]
		public static async Task FLASH64_EDITA(Dialog dialog)
		{
			await dialog.Msg("FLASH64_EDITA_basic_1");
			await dialog.Msg("FLASH64_EDITA_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_SALIAMONS")]
		public static async Task FLASH64_SALIAMONS(Dialog dialog)
		{
			await dialog.Msg("FLASH64_SALIAMONS_basic_1");
			await dialog.Msg("FLASH64_SALIAMONS_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_AMANDA")]
		public static async Task FLASH64_AMANDA(Dialog dialog)
		{
			await dialog.Msg("FLASH64_AMANDA_basic_1");
			await dialog.Msg("FLASH64_AMANDA_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_SQ_06_NPC")]
		public static async Task FLASH64_SQ_06_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_SQ_03_NPC")]
		public static async Task FLASH64_SQ_03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_SQ_07_NPC")]
		public static async Task FLASH64_SQ_07_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_MQ_01_NPC")]
		public static async Task FLASH64_MQ_01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH64_MQ_03_NPC")]
		public static async Task FLASH64_MQ_03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_FLASH_64")]
		public static async Task WARP_F_FLASH_64(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("UNDER_67_SQ030_NPC")]
		public static async Task UNDER_67_SQ030_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FLASH_641000")]
		public static async Task TREASUREBOX_LV_F_FLASH_641000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_213", 1);

		[DialogFunction("MURMILO_MASTER")]
		public static async Task MURMILO_MASTER(Dialog dialog)
		{
			await dialog.Msg("MURMILO_MASTER_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LANCER_MASTER")]
		public static async Task LANCER_MASTER(Dialog dialog)
		{
			await dialog.Msg("LANCER_MASTER_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ZEALOT_MASTER")]
		public static async Task ZEALOT_MASTER(Dialog dialog)
		{
			await dialog.Msg("ZEALOT_MASTER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_65_1")]
		public static async Task AMANDA_65_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_65_2")]
		public static async Task AMANDA_65_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_65_3")]
		public static async Task AMANDA_65_3(Dialog dialog)
		{
			await dialog.Msg("AMANDA_65_3_basic01");
			await dialog.Msg("AMANDA_65_3_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SETUP_BOOM01")]
		public static async Task SETUP_BOOM01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SETUP_BOOM02")]
		public static async Task SETUP_BOOM02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GLASS_MATERIAL")]
		public static async Task GLASS_MATERIAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER65_SQ02_BOMB_RANGE01")]
		public static async Task UNDER65_SQ02_BOMB_RANGE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER65_SQ02_BOMB_RANGE02")]
		public static async Task UNDER65_SQ02_BOMB_RANGE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER65_SQ02_BOMB_RANGE03")]
		public static async Task UNDER65_SQ02_BOMB_RANGE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER65_SQ02_BOMB_RANGE04")]
		public static async Task UNDER65_SQ02_BOMB_RANGE04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_65_4")]
		public static async Task AMANDA_65_4(Dialog dialog)
		{
			await dialog.Msg("AMANDA_65_4_basic01");
			await dialog.Msg("AMANDA_65_4_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER65_TO_UNDER66_WARP")]
		public static async Task UNDER65_TO_UNDER66_WARP(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_UNDERFORTRESS_65")]
		public static async Task WARP_D_UNDERFORTRESS_65(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("UNDER66_DELLOOS01")]
		public static async Task UNDER66_DELLOOS01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN01")]
		public static async Task UNDER66_KINGDOM_GUADIAN01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_DELLOOS")]
		public static async Task UNDER66_DELLOOS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_DELLOOS03")]
		public static async Task UNDER66_DELLOOS03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BOMB_BOX")]
		public static async Task BOMB_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_66_1")]
		public static async Task AMANDA_66_1(Dialog dialog)
		{
			await dialog.Msg("AMANDA_66_1_basic01");
			await dialog.Msg("AMANDA_66_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_66_2")]
		public static async Task AMANDA_66_2(Dialog dialog)
		{
			await dialog.Msg("AMANDA_66_2_basic01");
			await dialog.Msg("AMANDA_66_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP01")]
		public static async Task UNDER66_BONB_SETUP01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP02")]
		public static async Task UNDER66_BONB_SETUP02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP03")]
		public static async Task UNDER66_BONB_SETUP03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP04")]
		public static async Task UNDER66_BONB_SETUP04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP05")]
		public static async Task UNDER66_BONB_SETUP05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN02")]
		public static async Task UNDER66_KINGDOM_GUADIAN02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN03")]
		public static async Task UNDER66_KINGDOM_GUADIAN03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN04")]
		public static async Task UNDER66_KINGDOM_GUADIAN04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN01_1")]
		public static async Task UNDER66_KINGDOM_GUADIAN01_1(Dialog dialog)
		{
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN01_1_basic01");
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN01_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN02_1")]
		public static async Task UNDER66_KINGDOM_GUADIAN02_1(Dialog dialog)
		{
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN02_1_basic01");
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN02_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN03_1")]
		public static async Task UNDER66_KINGDOM_GUADIAN03_1(Dialog dialog)
		{
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN03_1_basic01");
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN03_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN04_1")]
		public static async Task UNDER66_KINGDOM_GUADIAN04_1(Dialog dialog)
		{
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN04_1_basic01");
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN04_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_KINGDOM_GUADIAN")]
		public static async Task UNDER66_KINGDOM_GUADIAN(Dialog dialog)
		{
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN_basic01");
			await dialog.Msg("UNDER66_KINGDOM_GUADIAN_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_DEAD_KINGDOM_GUADIAN01")]
		public static async Task UNDER66_DEAD_KINGDOM_GUADIAN01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_DEAD_KINGDOM_GUADIAN02")]
		public static async Task UNDER66_DEAD_KINGDOM_GUADIAN02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_DEAD_KINGDOM_GUADIAN03")]
		public static async Task UNDER66_DEAD_KINGDOM_GUADIAN03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_DEAD_KINGDOM_GUADIAN04")]
		public static async Task UNDER66_DEAD_KINGDOM_GUADIAN04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_MQ04")]
		public static async Task UNDER66_MQ04(Dialog dialog)
		{
			await dialog.Msg("UNDER66_MQ04_basic01");
			await dialog.Msg("UNDER66_MQ04_basic02");
			await dialog.Msg("UNDER66_MQ04_basic03");
			await dialog.Msg("UNDER66_MQ04_basic04");
			await dialog.Msg("UNDER66_MQ04_basic05");
			await dialog.Msg("UNDER66_MQ04_basic06");
			await dialog.Msg("UNDER66_MQ04_basic07");
			await dialog.Msg("UNDER66_MQ04_basic08");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_MQ05")]
		public static async Task UNDER66_MQ05(Dialog dialog)
		{
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic01");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic02");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic03");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic04");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic05");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic06");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic07");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic08");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic09");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic10");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_TO_UNDER65_SECRET_ROOM")]
		public static async Task UNDER66_TO_UNDER65_SECRET_ROOM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_MQ07_GHOST")]
		public static async Task UNDER66_MQ07_GHOST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_GUADIAN_WARDEN")]
		public static async Task UNDER66_GUADIAN_WARDEN(Dialog dialog)
		{
			await dialog.Msg("UNDER66_GUADIAN_WARDEN_basic01");
			await dialog.Msg("UNDER66_GUADIAN_WARDEN_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_MQ05_WARDEN")]
		public static async Task UNDER66_MQ05_WARDEN(Dialog dialog)
		{
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic01");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic02");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic03");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic04");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic05");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic06");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic07");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic08");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic09");
			await dialog.Msg("UNDER66_MQ05_WARDEN_basic10");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_66_3")]
		public static async Task AMANDA_66_3(Dialog dialog)
		{
			await dialog.Msg("AMANDA_66_3_basic01");
			await dialog.Msg("AMANDA_66_3_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP06")]
		public static async Task UNDER66_BONB_SETUP06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP07")]
		public static async Task UNDER66_BONB_SETUP07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP08")]
		public static async Task UNDER66_BONB_SETUP08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER66_BONB_SETUP09")]
		public static async Task UNDER66_BONB_SETUP09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_67_1")]
		public static async Task AMANDA_67_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_67_2")]
		public static async Task AMANDA_67_2(Dialog dialog)
		{
			await dialog.Msg("AMANDA_67_2_basic01");
			await dialog.Msg("AMANDA_67_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EMINENT_67_1")]
		public static async Task EMINENT_67_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EMINENT_67_2")]
		public static async Task EMINENT_67_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER67_BOOK1")]
		public static async Task UNDER67_BOOK1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER67_BOOK2")]
		public static async Task UNDER67_BOOK2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER67_SQ020")]
		public static async Task UNDER67_SQ020(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER67_GRASS")]
		public static async Task UNDER67_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER67_SQ030")]
		public static async Task UNDER67_SQ030(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER67_MQ060")]
		public static async Task UNDER67_MQ060(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EMINENT_68_1")]
		public static async Task EMINENT_68_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIAS_PLANT")]
		public static async Task VELNIAS_PLANT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER68_GHOST")]
		public static async Task UNDER68_GHOST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER68_SQ2")]
		public static async Task UNDER68_SQ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER68_SQ2_OWL")]
		public static async Task UNDER68_SQ2_OWL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_68_1")]
		public static async Task AMANDA_68_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_68_2")]
		public static async Task AMANDA_68_2(Dialog dialog)
		{
			await dialog.Msg("AMANDA_68_2_basic01");
			await dialog.Msg("AMANDA_68_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EMINENT_69_1")]
		public static async Task EMINENT_69_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EMINENT_69_2")]
		public static async Task EMINENT_69_2(Dialog dialog)
		{
			await dialog.Msg("UNDER_69_MQ020_startnpc_prog01");
			await dialog.Msg("EMINENT_69_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_69_1")]
		public static async Task AMANDA_69_1(Dialog dialog)
		{
			await dialog.Msg("AMANDA_69_1_basic01");
			await dialog.Msg("AMANDA_69_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AMANDA_69_2")]
		public static async Task AMANDA_69_2(Dialog dialog)
		{
			await dialog.Msg("AMANDA_69_2_basic01");
			await dialog.Msg("AMANDA_69_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ3_DEVICE_REPAIR")]
		public static async Task UNDER69_MQ3_DEVICE_REPAIR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ4_DEVICE")]
		public static async Task UNDER69_MQ4_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ5")]
		public static async Task UNDER69_MQ5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_SQ030_GHOST")]
		public static async Task UNDER69_SQ030_GHOST(Dialog dialog)
		{
			await dialog.Msg("UNDER69_SQ030_GHOST_basic01");
			await dialog.Msg("UNDER69_SQ030_GHOST_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ020_DEVICE01")]
		public static async Task UNDER69_MQ020_DEVICE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_SQ030_STONE01")]
		public static async Task UNDER69_SQ030_STONE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_SQ030_STONE02")]
		public static async Task UNDER69_SQ030_STONE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_PAPER01")]
		public static async Task UNDER69_PAPER01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_PAPER02")]
		public static async Task UNDER69_PAPER02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_PAPER03")]
		public static async Task UNDER69_PAPER03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_PAPER04")]
		public static async Task UNDER69_PAPER04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ020_DEVICE02")]
		public static async Task UNDER69_MQ020_DEVICE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ020_DEVICE03")]
		public static async Task UNDER69_MQ020_DEVICE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ020_DEVICE04")]
		public static async Task UNDER69_MQ020_DEVICE04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ020_DEVICE05")]
		public static async Task UNDER69_MQ020_DEVICE05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ4_FLAG01")]
		public static async Task UNDER69_MQ4_FLAG01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ4_FLAG02")]
		public static async Task UNDER69_MQ4_FLAG02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ4_FLAG03")]
		public static async Task UNDER69_MQ4_FLAG03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER69_MQ4_FLAG04")]
		public static async Task UNDER69_MQ4_FLAG04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_UNDERFORTRESS_69")]
		public static async Task WARP_D_UNDERFORTRESS_69(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TABLE70_SOLDIER1_1")]
		public static async Task TABLE70_SOLDIER1_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER1_1_basic1");
			await dialog.Msg("TABLE70_SOLDIER1_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_CAPTIN1_1")]
		public static async Task TABLE70_CAPTIN1_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_CAPTIN1_1_basic1");
			await dialog.Msg("TABLE70_CAPTIN1_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER2_1")]
		public static async Task TABLE70_SOLDIER2_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER2_1_basic1");
			await dialog.Msg("TABLE70_SOLDIER2_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER2_2")]
		public static async Task TABLE70_SOLDIER2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_CAPTIN1_2")]
		public static async Task TABLE70_CAPTIN1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER2_3")]
		public static async Task TABLE70_SOLDIER2_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER2_3_basic1");
			await dialog.Msg("TABLE70_SOLDIER2_3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER1_2")]
		public static async Task TABLE70_SOLDIER1_2(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER1_2_basic1");
			await dialog.Msg("TABLE70_SOLDIER1_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER3_1")]
		public static async Task TABLE70_SOLDIER3_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER3_1_basic1");
			await dialog.Msg("TABLE70_SOLDIER3_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER3_2")]
		public static async Task TABLE70_SOLDIER3_2(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER3_2_basic1");
			await dialog.Msg("TABLE70_SOLDIER3_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER4_1")]
		public static async Task TABLE70_SOLDIER4_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER4_1_basic1");
			await dialog.Msg("TABLE70_SOLDIER4_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER5_1")]
		public static async Task TABLE70_SOLDIER5_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER5_1_basic1");
			await dialog.Msg("TABLE70_SOLDIER5_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER6_1")]
		public static async Task TABLE70_SOLDIER6_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER6_1_basic1");
			await dialog.Msg("TABLE70_SOLDIER6_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER5_2")]
		public static async Task TABLE70_SOLDIER5_2(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER5_2_basic1");
			await dialog.Msg("TABLE70_SOLDIER5_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER6_2")]
		public static async Task TABLE70_SOLDIER6_2(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER6_2_basic1");
			await dialog.Msg("TABLE70_SOLDIER6_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_BOX")]
		public static async Task TABLE70_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_FENCE1")]
		public static async Task TABLE70_FENCE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_BOMB1")]
		public static async Task TABLE70_BOMB1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_BOMB2")]
		public static async Task TABLE70_BOMB2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_BOMB3")]
		public static async Task TABLE70_BOMB3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER4_2")]
		public static async Task TABLE70_SOLDIER4_2(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER4_2_basic1");
			await dialog.Msg("TABLE70_SOLDIER4_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER2_4")]
		public static async Task TABLE70_SOLDIER2_4(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER2_4_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER3_3")]
		public static async Task TABLE70_SOLDIER3_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER3_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER1_3")]
		public static async Task TABLE70_SOLDIER1_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER1_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER4_3")]
		public static async Task TABLE70_SOLDIER4_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER4_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER5_3")]
		public static async Task TABLE70_SOLDIER5_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER5_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER6_3")]
		public static async Task TABLE70_SOLDIER6_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER6_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_FENCE4")]
		public static async Task TABLE70_FENCE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_FENCE3")]
		public static async Task TABLE70_FENCE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_FENCE2")]
		public static async Task TABLE70_FENCE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER7_1")]
		public static async Task TABLE70_SOLDIER7_1(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER7_1_basic1");
			await dialog.Msg("TABLE70_SOLDIER7_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER7_2")]
		public static async Task TABLE70_SOLDIER7_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SOLDIER7_3")]
		public static async Task TABLE70_SOLDIER7_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_SOLDIER7_3_basic1");
			await dialog.Msg("TABLE70_SOLDIER7_3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SUBQ3_BOX")]
		public static async Task TABLE70_SUBQ3_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_CAPTIN1_3")]
		public static async Task TABLE70_CAPTIN1_3(Dialog dialog)
		{
			await dialog.Msg("TABLE70_CAPTIN1_3_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE70_SUBQ7_STONE")]
		public static async Task TABLE70_SUBQ7_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_701000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_701000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_274", 1);

		[DialogFunction("HT2_TABLELAND_70_BOOK01")]
		public static async Task HT2_TABLELAND_70_BOOK01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_TABLELAND_70_BOOK02")]
		public static async Task HT2_TABLELAND_70_BOOK02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_TABLELAND_70_BOOK03")]
		public static async Task HT2_TABLELAND_70_BOOK03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND71_ITEM_CHECK")]
		public static async Task TABLELAND71_ITEM_CHECK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_PEAPLE1")]
		public static async Task TABLE71_PEAPLE1(Dialog dialog)
		{
			await dialog.Msg("TABLE71_PEAPLE1_basic1");
			await dialog.Msg("TABLE71_PEAPLE1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_PEAPLE2")]
		public static async Task TABLE71_PEAPLE2(Dialog dialog)
		{
			await dialog.Msg("TABLE71_PEAPLE2_basic1");
			await dialog.Msg("TABLE71_PEAPLE2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_PEAPLE3")]
		public static async Task TABLE71_PEAPLE3(Dialog dialog)
		{
			await dialog.Msg("TABLE71_PEAPLE3_basic1");
			await dialog.Msg("TABLE71_PEAPLE3_basic2");
			await dialog.Msg("TABLE71_PEAPLE3_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_PEAPLE4")]
		public static async Task TABLE71_PEAPLE4(Dialog dialog)
		{
			await dialog.Msg("TABLE71_PEAPLE3_basic1");
			await dialog.Msg("TABLE71_PEAPLE3_basic2");
			await dialog.Msg("TABLE71_PEAPLE3_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_PEAPLE5")]
		public static async Task TABLE71_PEAPLE5(Dialog dialog)
		{
			await dialog.Msg("TABLE71_PEAPLE3_basic1");
			await dialog.Msg("TABLE71_PEAPLE3_basic2");
			await dialog.Msg("TABLE71_PEAPLE3_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_PEAPLE6")]
		public static async Task TABLE71_PEAPLE6(Dialog dialog)
		{
			await dialog.Msg("TABLE71_PEAPLE3_basic1");
			await dialog.Msg("TABLE71_PEAPLE3_basic2");
			await dialog.Msg("TABLE71_PEAPLE3_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_POINT1")]
		public static async Task TABLE71_POINT1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_POINT2")]
		public static async Task TABLE71_POINT2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_POINT3")]
		public static async Task TABLE71_POINT3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_STONE")]
		public static async Task TABLE71_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_PEAPLE7")]
		public static async Task TABLE71_PEAPLE7(Dialog dialog)
		{
			await dialog.Msg("TABLE71_PEAPLE3_basic1");
			await dialog.Msg("TABLE71_PEAPLE3_basic2");
			await dialog.Msg("TABLE71_PEAPLE3_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE71_GLASS")]
		public static async Task TABLE71_GLASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_711000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_711000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_275", 1);

		[DialogFunction("WARP_F_TABLELAND_72")]
		public static async Task WARP_F_TABLELAND_72(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TABLELAND72_BASE_BALL")]
		public static async Task TABLELAND72_BASE_BALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_ARTIFACT")]
		public static async Task TABLE72_ARTIFACT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE1_1")]
		public static async Task TABLE72_PEAPLE1_1(Dialog dialog)
		{
			await dialog.Msg("TABLE72_PEAPLE1_1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE2")]
		public static async Task TABLE72_PEAPLE2(Dialog dialog)
		{
			await dialog.Msg("TABLE72_PEAPLE2_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE3")]
		public static async Task TABLE72_PEAPLE3(Dialog dialog)
		{
			await dialog.Msg("TABLE72_PEAPLE3_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE4")]
		public static async Task TABLE72_PEAPLE4(Dialog dialog)
		{
			await dialog.Msg("TABLE72_PEAPLE4_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_MONITORING")]
		public static async Task TABLE72_MONITORING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_SUBQ6_ARTIFACT")]
		public static async Task TABLE72_SUBQ6_ARTIFACT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_GLASS1")]
		public static async Task TABLE72_GLASS1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE1_2")]
		public static async Task TABLE72_PEAPLE1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE5")]
		public static async Task TABLE72_PEAPLE5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE1_3")]
		public static async Task TABLE72_PEAPLE1_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_PEAPLE5_1")]
		public static async Task TABLE72_PEAPLE5_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE72_SUBQ3_STONE")]
		public static async Task TABLE72_SUBQ3_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_721000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_721000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_276", 1);

		[DialogFunction("TABLELAND73_HIGH_LOW")]
		public static async Task TABLELAND73_HIGH_LOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND73_CURLING")]
		public static async Task TABLELAND73_CURLING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUB_ARTIFACT1")]
		public static async Task TABLE73_SUB_ARTIFACT1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUB_ARTIFACT2")]
		public static async Task TABLE73_SUB_ARTIFACT2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUB_ARTIFACT3")]
		public static async Task TABLE73_SUB_ARTIFACT3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ_ALTAR1")]
		public static async Task TABLE73_SUBQ_ALTAR1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ_ALTAR2")]
		public static async Task TABLE73_SUBQ_ALTAR2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ8_ARTIFACT1")]
		public static async Task TABLE73_SUBQ8_ARTIFACT1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ8_LUMP1")]
		public static async Task TABLE73_SUBQ8_LUMP1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ8_LUMP2")]
		public static async Task TABLE73_SUBQ8_LUMP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ8_LUMP3")]
		public static async Task TABLE73_SUBQ8_LUMP3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ8_LUMP4")]
		public static async Task TABLE73_SUBQ8_LUMP4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ1_ARTIFACT")]
		public static async Task TABLE73_SUBQ1_ARTIFACT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE73_SUBQ6_WOOD")]
		public static async Task TABLE73_SUBQ6_WOOD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_731000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_731000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_277", 1);

		[DialogFunction("TABLELAND74_ITEM_CHECK")]
		public static async Task TABLELAND74_ITEM_CHECK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND74_NUMBER_MIXING")]
		public static async Task TABLELAND74_NUMBER_MIXING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER5")]
		public static async Task TABLE74_SUBQ_SOLDIER5(Dialog dialog)
		{
			await dialog.Msg("TABLE74_SUBQ_SOLDIER5_basic1");
			await dialog.Msg("TABLE74_SUBQ_SOLDIER5_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER1")]
		public static async Task TABLE74_SUBQ_SOLDIER1(Dialog dialog)
		{
			await dialog.Msg("TABLE74_SUBQ_SOLDIER1_basic1");
			await dialog.Msg("TABLE74_SUBQ_SOLDIER1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER2")]
		public static async Task TABLE74_SUBQ_SOLDIER2(Dialog dialog)
		{
			await dialog.Msg("TABLE74_SUBQ_SOLDIER2_basic1");
			await dialog.Msg("TABLE74_SUBQ_SOLDIER2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER4")]
		public static async Task TABLE74_SUBQ_SOLDIER4(Dialog dialog)
		{
			await dialog.Msg("TABLE74_SUBQ_SOLDIER4_basic1");
			await dialog.Msg("TABLE74_SUBQ_SOLDIER4_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_BOX1")]
		public static async Task TABLE74_SUBQ_BOX1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER3_1")]
		public static async Task TABLE74_SUBQ_SOLDIER3_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER3_2")]
		public static async Task TABLE74_SUBQ_SOLDIER3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_CRYSTAL")]
		public static async Task TABLE74_SUBQ_CRYSTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_ARTIFACT")]
		public static async Task TABLE74_SUBQ_ARTIFACT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ4_STONE")]
		public static async Task TABLE74_SUBQ4_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER2_2")]
		public static async Task TABLE74_SUBQ_SOLDIER2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLE74_SUBQ_SOLDIER3_3")]
		public static async Task TABLE74_SUBQ_SOLDIER3_3(Dialog dialog)
		{
			await dialog.Msg("TABLE74_SUBQ_SOLDIER3_3_basic1");
			await dialog.Msg("TABLE74_SUBQ_SOLDIER3_3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_741000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_741000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_278", 1);

		[DialogFunction("WARP_D_PRISON_78")]
		public static async Task WARP_D_PRISON_78(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("PRISON_78_NUMBER_BASEBALL")]
		public static async Task PRISON_78_NUMBER_BASEBALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_1")]
		public static async Task PRISON_78_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_NPC_1")]
		public static async Task PRISON_78_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_3_1")]
		public static async Task PRISON_78_OBJ_3_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_3_2")]
		public static async Task PRISON_78_OBJ_3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_3_3")]
		public static async Task PRISON_78_OBJ_3_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_3_4")]
		public static async Task PRISON_78_OBJ_3_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_3_5")]
		public static async Task PRISON_78_OBJ_3_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_4")]
		public static async Task PRISON_78_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_OBJ_5")]
		public static async Task PRISON_78_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_SQ_OBJ_1")]
		public static async Task PRISON_78_SQ_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_SQ_OBJ_2")]
		public static async Task PRISON_78_SQ_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_SQ_OBJ_3")]
		public static async Task PRISON_78_SQ_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_SQ_OBJ_4_1")]
		public static async Task PRISON_78_SQ_OBJ_4_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_SQ_OBJ_4_2")]
		public static async Task PRISON_78_SQ_OBJ_4_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_78_SQ_OBJ_4_3")]
		public static async Task PRISON_78_SQ_OBJ_4_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT_KALEJIMO_PAPER01")]
		public static async Task HT_KALEJIMO_PAPER01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT_KALEJIMO_PAPER02")]
		public static async Task HT_KALEJIMO_PAPER02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT_KALEJIMO_PAPER03")]
		public static async Task HT_KALEJIMO_PAPER03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT_KALEJIMO_PAPER04")]
		public static async Task HT_KALEJIMO_PAPER04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_NUMBER_HI_LOW")]
		public static async Task PRISON_79_NUMBER_HI_LOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_NPC_1")]
		public static async Task PRISON_79_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_1")]
		public static async Task PRISON_79_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_NPC_2")]
		public static async Task PRISON_79_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_2_1")]
		public static async Task PRISON_79_OBJ_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_2_2")]
		public static async Task PRISON_79_OBJ_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_2_3")]
		public static async Task PRISON_79_OBJ_2_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_2_4")]
		public static async Task PRISON_79_OBJ_2_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_2_5")]
		public static async Task PRISON_79_OBJ_2_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_3")]
		public static async Task PRISON_79_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_4")]
		public static async Task PRISON_79_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_5")]
		public static async Task PRISON_79_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_6")]
		public static async Task PRISON_79_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_7_1")]
		public static async Task PRISON_79_OBJ_7_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_7_2")]
		public static async Task PRISON_79_OBJ_7_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_7_3")]
		public static async Task PRISON_79_OBJ_7_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_7_4")]
		public static async Task PRISON_79_OBJ_7_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_7_5")]
		public static async Task PRISON_79_OBJ_7_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_OBJ_8")]
		public static async Task PRISON_79_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_MQ_10_TRIGGER")]
		public static async Task PRISON_79_MQ_10_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_79_SQ_OBJ_1")]
		public static async Task PRISON_79_SQ_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DTACURLING")]
		public static async Task DTACURLING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_MIXING")]
		public static async Task PRISON_80_MIXING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_NPC_1")]
		public static async Task PRISON_80_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_1")]
		public static async Task PRISON_80_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_2")]
		public static async Task PRISON_80_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_3")]
		public static async Task PRISON_80_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_4")]
		public static async Task PRISON_80_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_NPC_2")]
		public static async Task PRISON_80_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_5")]
		public static async Task PRISON_80_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_6_1")]
		public static async Task PRISON_80_OBJ_6_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_6_2")]
		public static async Task PRISON_80_OBJ_6_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_6_3")]
		public static async Task PRISON_80_OBJ_6_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_6_4")]
		public static async Task PRISON_80_OBJ_6_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_6_5")]
		public static async Task PRISON_80_OBJ_6_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_OBJ_6_6")]
		public static async Task PRISON_80_OBJ_6_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_MQ_10_TRIGGER")]
		public static async Task PRISON_80_MQ_10_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_SQ_OBJ_1")]
		public static async Task PRISON_80_SQ_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_80_SQ_OBJ_2")]
		public static async Task PRISON_80_SQ_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_PRISON_81")]
		public static async Task WARP_D_PRISON_81(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("PRISON_81_NUMBER_BASEBALL")]
		public static async Task PRISON_81_NUMBER_BASEBALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_NPC_1")]
		public static async Task PRISON_81_NPC_1(Dialog dialog)
		{
			await dialog.Msg("PRISON_81_NPC_INFO1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_OBJ_1_1")]
		public static async Task PRISON_81_OBJ_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_OBJ_1_2")]
		public static async Task PRISON_81_OBJ_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_OBJ_1_3")]
		public static async Task PRISON_81_OBJ_1_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_NPC_2")]
		public static async Task PRISON_81_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_OBJ_4")]
		public static async Task PRISON_81_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_OBJ_5")]
		public static async Task PRISON_81_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_NPC_3")]
		public static async Task PRISON_81_NPC_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_OBJ_6")]
		public static async Task PRISON_81_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_MQ_10_TRIGGER")]
		public static async Task PRISON_81_MQ_10_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_SQ_OBJ_1")]
		public static async Task PRISON_81_SQ_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_81_SQ_OBJ_2")]
		public static async Task PRISON_81_SQ_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MONSTER_PAIR_GIMMICK")]
		public static async Task MONSTER_PAIR_GIMMICK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_NPC_1")]
		public static async Task PRISON_82_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_NPC_2")]
		public static async Task PRISON_82_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_1")]
		public static async Task PRISON_82_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_2")]
		public static async Task PRISON_82_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_3")]
		public static async Task PRISON_82_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_5")]
		public static async Task PRISON_82_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_5_1")]
		public static async Task PRISON_82_OBJ_5_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_5_2")]
		public static async Task PRISON_82_OBJ_5_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_5_3")]
		public static async Task PRISON_82_OBJ_5_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_5_4")]
		public static async Task PRISON_82_OBJ_5_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_6")]
		public static async Task PRISON_82_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_OBJ_8")]
		public static async Task PRISON_82_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_SQ_OBJ_1")]
		public static async Task PRISON_82_SQ_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_SQ_OBJ_2")]
		public static async Task PRISON_82_SQ_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_82_MQ_11_BOOK")]
		public static async Task PRISON_82_MQ_11_BOOK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_01_NPC_1")]
		public static async Task F_3CMLAKE_85_MQ_01_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_02_NPC")]
		public static async Task F_3CMLAKE_85_MQ_02_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_85_MQ_02_NPC_basic01");
			await dialog.Msg("F_3CMLAKE_85_MQ_02_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_03_NPC")]
		public static async Task F_3CMLAKE_85_MQ_03_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_85_MQ_03_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_02_2_NPC")]
		public static async Task F_3CMLAKE_85_MQ_02_2_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_85_MQ_02_2_NPC_basic01");
			await dialog.Msg("F_3CMLAKE_85_MQ_02_2_NPC_basic03");
			await dialog.Msg("F_3CMLAKE_85_MQ_02_2_NPC_basic04");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_01_NPC_2")]
		public static async Task F_3CMLAKE_85_MQ_01_NPC_2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_85_MQ_01_NPC_2_basic01");
			await dialog.Msg("F_3CMLAKE_85_MQ_01_NPC_2_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_06_NPC")]
		public static async Task F_3CMLAKE_85_MQ_06_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_10_NPC")]
		public static async Task F_3CMLAKE_85_MQ_10_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_02_1_OBJ")]
		public static async Task F_3CMLAKE_85_MQ_02_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_03_OBJ")]
		public static async Task F_3CMLAKE_85_MQ_03_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_04_OBJ")]
		public static async Task F_3CMLAKE_85_MQ_04_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_06_OBJ_1")]
		public static async Task F_3CMLAKE_85_MQ_06_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_06_OBJ_2")]
		public static async Task F_3CMLAKE_85_MQ_06_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_06_OBJ_3")]
		public static async Task F_3CMLAKE_85_MQ_06_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_SQ_01_OBJ")]
		public static async Task F_3CMLAKE_85_SQ_01_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_07_NPC")]
		public static async Task F_3CMLAKE_85_MQ_07_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_10_NPC1")]
		public static async Task F_3CMLAKE_85_MQ_10_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_85_MQ_10_NPC1_basic01");
			await dialog.Msg("F_3CMLAKE_85_MQ_10_NPC1_basic02");
			await dialog.Msg("F_3CMLAKE_85_MQ_10_NPC1_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_10_NPC2")]
		public static async Task F_3CMLAKE_85_MQ_10_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_85_MQ_10_NPC2_basic01");
			await dialog.Msg("F_3CMLAKE_85_MQ_10_NPC2_basic02");
			await dialog.Msg("F_3CMLAKE_85_MQ_10_NPC2_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_08_CHECK1")]
		public static async Task F_3CMLAKE_86_MQ_08_CHECK1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_85_MQ_11_NPC")]
		public static async Task F_3CMLAKE_85_MQ_11_NPC(Dialog dialog)
		{
			await dialog.Msg("CASTLE93_MAIN07_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_01_NPC_1")]
		public static async Task F_3CMLAKE_86_MQ_01_NPC_1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_01_NPC_1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_01_NPC_2")]
		public static async Task F_3CMLAKE_86_MQ_01_NPC_2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_01_NPC_2_basic01");
			await dialog.Msg("F_3CMLAKE_86_MQ_01_NPC_2_basic02");
			await dialog.Msg("F_3CMLAKE_86_MQ_01_NPC_2_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_02_NPC")]
		public static async Task F_3CMLAKE_86_MQ_02_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_02_NPC_basic01");
			await dialog.Msg("F_3CMLAKE_86_MQ_02_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_03_NPC")]
		public static async Task F_3CMLAKE_86_MQ_03_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_03_NPC_basic01");
			await dialog.Msg("F_3CMLAKE_86_MQ_03_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_16_NPC")]
		public static async Task F_3CMLAKE_86_MQ_16_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_16_NPC_basic01");
			await dialog.Msg("F_3CMLAKE_86_MQ_16_NPC_basic02");
			await dialog.Msg("F_3CMLAKE_86_MQ_16_NPC_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_18_NPC")]
		public static async Task F_3CMLAKE_86_MQ_18_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_18_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_15_NPC")]
		public static async Task F_3CMLAKE_86_MQ_15_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_15_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_SQ_04_NPC")]
		public static async Task F_3CMLAKE_86_SQ_04_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_SQ_04_NPC_basic01");
			await dialog.Msg("F_3CMLAKE_86_MQ_02_NPC_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_03_NEUTRAL_NPC")]
		public static async Task F_3CMLAKE_86_MQ_03_NEUTRAL_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_04_OBJ1")]
		public static async Task F_3CMLAKE_86_MQ_04_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_04_OBJ2")]
		public static async Task F_3CMLAKE_86_MQ_04_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_04_OBJ3")]
		public static async Task F_3CMLAKE_86_MQ_04_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_04_OBJ4")]
		public static async Task F_3CMLAKE_86_MQ_04_OBJ4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_04_OBJ5")]
		public static async Task F_3CMLAKE_86_MQ_04_OBJ5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_04_SCOUT_NPC")]
		public static async Task F_3CMLAKE_86_MQ_04_SCOUT_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_13_OBJ")]
		public static async Task F_3CMLAKE_86_MQ_13_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_SQ_01_OBJ")]
		public static async Task F_3CMLAKE_86_SQ_01_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_SQ_03_OBJ")]
		public static async Task F_3CMLAKE_86_SQ_03_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_SQ_02_OBJ_1")]
		public static async Task F_3CMLAKE_86_SQ_02_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_SQ_04_CONFUSE_NPC")]
		public static async Task F_3CMLAKE_86_SQ_04_CONFUSE_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_SQ_02_OBJ_2")]
		public static async Task F_3CMLAKE_86_SQ_02_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_SQ_02_OBJ_3")]
		public static async Task F_3CMLAKE_86_SQ_02_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_07_NPC")]
		public static async Task F_3CMLAKE_86_MQ_07_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_10_NPC")]
		public static async Task F_3CMLAKE_86_MQ_10_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_11_NPC")]
		public static async Task F_3CMLAKE_86_MQ_11_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_12_NPC")]
		public static async Task F_3CMLAKE_86_MQ_12_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_18_NPC_2")]
		public static async Task F_3CMLAKE_86_MQ_18_NPC_2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_18_NPC_2_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_MQ_18_NPC_3")]
		public static async Task F_3CMLAKE_86_MQ_18_NPC_3(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_86_MQ_18_NPC_3_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_86_EV_55_001")]
		public static async Task F_3CMLAKE_86_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("F_3CMLAKE_87_MQ_01_NPC_2")]
		public static async Task F_3CMLAKE_87_MQ_01_NPC_2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_MQ_01_NPC_2_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_01_NPC_1")]
		public static async Task F_3CMLAKE_87_MQ_01_NPC_1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_MQ_01_NPC_1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_03_NPC")]
		public static async Task F_3CMLAKE_87_MQ_03_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_MQ_03_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_01_NPC_3")]
		public static async Task F_3CMLAKE_87_MQ_01_NPC_3(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_MQ_01_NPC_3_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_07_NPC")]
		public static async Task F_3CMLAKE_87_MQ_07_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_MQ_07_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_05_NPC")]
		public static async Task F_3CMLAKE_87_MQ_05_NPC(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_MQ_05_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_02_OBJ_1")]
		public static async Task F_3CMLAKE_87_MQ_02_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_02_OBJ_2")]
		public static async Task F_3CMLAKE_87_MQ_02_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_02_OBJ_3")]
		public static async Task F_3CMLAKE_87_MQ_02_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_02_OBJ_4")]
		public static async Task F_3CMLAKE_87_MQ_02_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_02_OBJ_5")]
		public static async Task F_3CMLAKE_87_MQ_02_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_02_OBJ_6")]
		public static async Task F_3CMLAKE_87_MQ_02_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_05_OBJ")]
		public static async Task F_3CMLAKE_87_MQ_05_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_07_OBJ")]
		public static async Task F_3CMLAKE_87_MQ_07_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_09_NPC_1")]
		public static async Task F_3CMLAKE_87_MQ_09_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_09_NPC_2")]
		public static async Task F_3CMLAKE_87_MQ_09_NPC_2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_FOLLOWER2_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_10_NPC_2")]
		public static async Task F_3CMLAKE_87_MQ_10_NPC_2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_FOLLOWER1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_10_NPC_1")]
		public static async Task F_3CMLAKE_87_MQ_10_NPC_1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_87_FOLLOWER2_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_11_OBJ_1")]
		public static async Task F_3CMLAKE_87_MQ_11_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_11_OBJ_2")]
		public static async Task F_3CMLAKE_87_MQ_11_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_11_OBJ_3")]
		public static async Task F_3CMLAKE_87_MQ_11_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_11_OBJ_4")]
		public static async Task F_3CMLAKE_87_MQ_11_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_11_OBJ_5")]
		public static async Task F_3CMLAKE_87_MQ_11_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_12_NPC")]
		public static async Task F_3CMLAKE_87_MQ_12_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_MQ_13_NPC")]
		public static async Task F_3CMLAKE_87_MQ_13_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_87_SQ_02_OBJ")]
		public static async Task F_3CMLAKE_87_SQ_02_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_3CMLAKE_87")]
		public static async Task WARP_F_3CMLAKE_87(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("PAYAUTA_EP11_NPC_87")]
		public static async Task PAYAUTA_EP11_NPC_87(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PAJAUTA_EP11_SIGN_1_87")]
		public static async Task PAJAUTA_EP11_SIGN_1_87(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_MEMBER_01")]
		public static async Task D_STARTOWER_88_RESISTANCE_MEMBER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_MEMBER_02")]
		public static async Task D_STARTOWER_88_RESISTANCE_MEMBER_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_MEMBER_03")]
		public static async Task D_STARTOWER_88_RESISTANCE_MEMBER_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_SENIOR_01")]
		public static async Task D_STARTOWER_88_RESISTANCE_SENIOR_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_MEMBER_04")]
		public static async Task D_STARTOWER_88_RESISTANCE_MEMBER_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_MEMBER_05")]
		public static async Task D_STARTOWER_88_RESISTANCE_MEMBER_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_MEMBER_06")]
		public static async Task D_STARTOWER_88_RESISTANCE_MEMBER_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_MEMBER_07")]
		public static async Task D_STARTOWER_88_RESISTANCE_MEMBER_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_SUB_LEADER_KRON_01")]
		public static async Task D_STARTOWER_88_RESISTANCE_SUB_LEADER_KRON_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_RESISTANCE_SUB_LEADER_KRON_02")]
		public static async Task D_STARTOWER_88_RESISTANCE_SUB_LEADER_KRON_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_1ST_DEFENCE_DEVICE")]
		public static async Task D_STARTOWER_88_1ST_DEFENCE_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_3RD_SUMMON_PORTAL")]
		public static async Task D_STARTOWER_88_VELNIAS_3RD_SUMMON_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_1ST_SUMMON_PORTAL")]
		public static async Task D_STARTOWER_88_VELNIAS_1ST_SUMMON_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_2ND_SUMMON_PORTAL")]
		public static async Task D_STARTOWER_88_VELNIAS_2ND_SUMMON_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_4TH_SUMMON_PORTAL")]
		public static async Task D_STARTOWER_88_VELNIAS_4TH_SUMMON_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_1ST_CONTROL_DEVICE")]
		public static async Task D_STARTOWER_88_VELNIAS_1ST_CONTROL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_2ND_CONTROL_DEVICE")]
		public static async Task D_STARTOWER_88_VELNIAS_2ND_CONTROL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_3RD_CONTROL_DEVICE")]
		public static async Task D_STARTOWER_88_VELNIAS_3RD_CONTROL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_VELNIAS_4TH_CONTROL_DEVICE")]
		public static async Task D_STARTOWER_88_VELNIAS_4TH_CONTROL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_88_1ST_DEFENCE_AFTER_DEVICE")]
		public static async Task D_STARTOWER_88_1ST_DEFENCE_AFTER_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_STARTOWER_88")]
		public static async Task WARP_D_STARTOWER_88(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("STARTOWER_88_MQ_20_PORTAL_NPC")]
		public static async Task STARTOWER_88_MQ_20_PORTAL_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_LEADER_BAYL_01")]
		public static async Task D_STARTOWER_89_RESISTANCE_LEADER_BAYL_01(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_89_MQ_20_DLG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_LEADER_BAYL_02")]
		public static async Task D_STARTOWER_89_RESISTANCE_LEADER_BAYL_02(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_89_MQ_50_DLG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_01")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_02")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_03")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_LEADER_BAYL_03")]
		public static async Task D_STARTOWER_89_RESISTANCE_LEADER_BAYL_03(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_89_MQ_60_DLG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_LEADER_BAYL_04")]
		public static async Task D_STARTOWER_89_RESISTANCE_LEADER_BAYL_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_04")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_05")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_06")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_07")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_08")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_1ST_DEFENCE_DEVICE")]
		public static async Task D_STARTOWER_89_1ST_DEFENCE_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_2ND_DEFENCE_DEVICE")]
		public static async Task D_STARTOWER_89_2ND_DEFENCE_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_SCHAFFENSTAR_ELDER_HENIKA")]
		public static async Task D_STARTOWER_89_SCHAFFENSTAR_ELDER_HENIKA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_89_RESISTANCE_MEMBER_09")]
		public static async Task D_STARTOWER_89_RESISTANCE_MEMBER_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_LEADER_BAYL_01")]
		public static async Task D_STARTOWER_90_RESISTANCE_LEADER_BAYL_01(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_90_MQ_50_DLG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_MEMBER_01_1")]
		public static async Task D_STARTOWER_90_RESISTANCE_MEMBER_01_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_MEMBER_02_1")]
		public static async Task D_STARTOWER_90_RESISTANCE_MEMBER_02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_MEMBER_03_1")]
		public static async Task D_STARTOWER_90_RESISTANCE_MEMBER_03_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_LEADER_BAYL_02")]
		public static async Task D_STARTOWER_90_RESISTANCE_LEADER_BAYL_02(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_90_MQ_80_DLG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_SCHAFFENSTAR_ELDER_ADAUX")]
		public static async Task D_STARTOWER_90_SCHAFFENSTAR_ELDER_ADAUX(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_90_MQ_60_DLG3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_SCHAFFENSTAR_MEMBER_01")]
		public static async Task D_STARTOWER_90_SCHAFFENSTAR_MEMBER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_SCHAFFENSTAR_MEMBER_02")]
		public static async Task D_STARTOWER_90_SCHAFFENSTAR_MEMBER_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_SCHAFFENSTAR_MEMBER_03")]
		public static async Task D_STARTOWER_90_SCHAFFENSTAR_MEMBER_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_SCHAFFENSTAR_MEMBER_04")]
		public static async Task D_STARTOWER_90_SCHAFFENSTAR_MEMBER_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_SCHAFFENSTAR_MEMBER_05")]
		public static async Task D_STARTOWER_90_SCHAFFENSTAR_MEMBER_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_MEMBER_04")]
		public static async Task D_STARTOWER_90_RESISTANCE_MEMBER_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_MEMBER_05")]
		public static async Task D_STARTOWER_90_RESISTANCE_MEMBER_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_RESISTANCE_MEMBER_06")]
		public static async Task D_STARTOWER_90_RESISTANCE_MEMBER_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_2ND_DEFENCE_DEVICE")]
		public static async Task D_STARTOWER_90_2ND_DEFENCE_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_1ST_DEFENCE_DEVICE")]
		public static async Task D_STARTOWER_90_1ST_DEFENCE_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_1ST_HIDE_CONTROL_DEVICE")]
		public static async Task D_STARTOWER_90_1ST_HIDE_CONTROL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_2ND_HIDE_CONTROL_DEVICE")]
		public static async Task D_STARTOWER_90_2ND_HIDE_CONTROL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_3RD_HIDE_CONTROL_DEVICE")]
		public static async Task D_STARTOWER_90_3RD_HIDE_CONTROL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_STATUE")]
		public static async Task D_STARTOWER_90_STATUE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_MQ_HIDDENWALL")]
		public static async Task D_STARTOWER_90_MQ_HIDDENWALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_90_MQ_HIDDENWALL_TRIGGER")]
		public static async Task D_STARTOWER_90_MQ_HIDDENWALL_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_LEADER_BAYL_01")]
		public static async Task D_STARTOWER_91_RESISTANCE_LEADER_BAYL_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_01")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_02")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_03")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_04")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_05")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_06")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_07")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_1ST_SUB_DEVICE")]
		public static async Task D_STARTOWER_91_1ST_SUB_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_2ND_SUB_DEVICE")]
		public static async Task D_STARTOWER_91_2ND_SUB_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_3RD_SUB_DEVICE")]
		public static async Task D_STARTOWER_91_3RD_SUB_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_4TH_SUB_DEVICE_01")]
		public static async Task D_STARTOWER_91_4TH_SUB_DEVICE_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_1ST_DEFENCE_DEVICE")]
		public static async Task D_STARTOWER_91_1ST_DEFENCE_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_ELEVATER_CONFIG_DEVICE")]
		public static async Task D_STARTOWER_91_ELEVATER_CONFIG_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_LEADER_BAYL_02")]
		public static async Task D_STARTOWER_91_RESISTANCE_LEADER_BAYL_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_LEADER_BAYL_03")]
		public static async Task D_STARTOWER_91_RESISTANCE_LEADER_BAYL_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_SENIOR_ARKADIJUS")]
		public static async Task D_STARTOWER_91_RESISTANCE_SENIOR_ARKADIJUS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_08")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_09")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_10")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_11")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_RESISTANCE_MEMBER_12")]
		public static async Task D_STARTOWER_91_RESISTANCE_MEMBER_12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_4TH_SUB_DEVICE_02")]
		public static async Task D_STARTOWER_91_4TH_SUB_DEVICE_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_BOOKSHELF_04")]
		public static async Task D_STARTOWER_91_BOOKSHELF_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_BOOKSHELF_01")]
		public static async Task D_STARTOWER_91_BOOKSHELF_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_BOOKSHELF_05")]
		public static async Task D_STARTOWER_91_BOOKSHELF_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_BOOKSHELF_03")]
		public static async Task D_STARTOWER_91_BOOKSHELF_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_91_BOOKSHELF_02")]
		public static async Task D_STARTOWER_91_BOOKSHELF_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_STARTOWER_91")]
		public static async Task WARP_D_STARTOWER_91(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("D_STARTOWER_92_RESISTANCE_LEADER_BAYL_01")]
		public static async Task D_STARTOWER_92_RESISTANCE_LEADER_BAYL_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_LEADER_BAYL_02")]
		public static async Task D_STARTOWER_92_RESISTANCE_LEADER_BAYL_02(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_92_BAYL_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_LEADER_BAYL_03")]
		public static async Task D_STARTOWER_92_RESISTANCE_LEADER_BAYL_03(Dialog dialog)
		{
			await dialog.Msg("STARTOWER_92_MQ_60_DLG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_01")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_03")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_02")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_04")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_05")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_06")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_07")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_RESISTANCE_MEMBER_08")]
		public static async Task D_STARTOWER_92_RESISTANCE_MEMBER_08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_VALNIAS_DEVICE")]
		public static async Task D_STARTOWER_92_VALNIAS_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_92_MQ_60_TRIGGER")]
		public static async Task D_STARTOWER_92_MQ_60_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_92_MQ_20_HIDDEN_NPC")]
		public static async Task STARTOWER_92_MQ_20_HIDDEN_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_MAIN01")]
		public static async Task CASTLE93_NPC_MAIN01(Dialog dialog)
		{
			await dialog.Msg("CASTLE93_NPC_MAIN01_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_01")]
		public static async Task CASTLE93_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_02")]
		public static async Task CASTLE93_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_03")]
		public static async Task CASTLE93_NPC_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_04")]
		public static async Task CASTLE93_NPC_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_05")]
		public static async Task CASTLE93_NPC_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_06")]
		public static async Task CASTLE93_NPC_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_07")]
		public static async Task CASTLE93_NPC_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_08")]
		public static async Task CASTLE93_NPC_08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_09")]
		public static async Task CASTLE93_NPC_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_10")]
		public static async Task CASTLE93_NPC_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_24")]
		public static async Task CASTLE93_NPC_24(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_11")]
		public static async Task CASTLE93_NPC_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_12")]
		public static async Task CASTLE93_NPC_12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_13")]
		public static async Task CASTLE93_NPC_13(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_14")]
		public static async Task CASTLE93_NPC_14(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_15")]
		public static async Task CASTLE93_NPC_15(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_17")]
		public static async Task CASTLE93_NPC_17(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_16")]
		public static async Task CASTLE93_NPC_16(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_25")]
		public static async Task CASTLE93_NPC_25(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_18")]
		public static async Task CASTLE93_NPC_18(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_19")]
		public static async Task CASTLE93_NPC_19(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_20")]
		public static async Task CASTLE93_NPC_20(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_21")]
		public static async Task CASTLE93_NPC_21(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_22")]
		public static async Task CASTLE93_NPC_22(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE93_NPC_23")]
		public static async Task CASTLE93_NPC_23(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_MAIN01")]
		public static async Task CASTLE94_NPC_MAIN01(Dialog dialog)
		{
			await dialog.Msg("CASTLE94_NPC_MAIN01_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_01")]
		public static async Task CASTLE94_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_03")]
		public static async Task CASTLE94_NPC_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_02")]
		public static async Task CASTLE94_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_MAIN02")]
		public static async Task CASTLE94_NPC_MAIN02(Dialog dialog)
		{
			await dialog.Msg("CASTLE94_MAIN05_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_MONUMENT")]
		public static async Task CASTLE94_NPC_MONUMENT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_04")]
		public static async Task CASTLE94_NPC_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_NPC_05")]
		public static async Task CASTLE94_NPC_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE94_MAIN05_SHIELD")]
		public static async Task CASTLE94_MAIN05_SHIELD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_CASTLE_94")]
		public static async Task WARP_F_CASTLE_94(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("CASTLE95_NPC_MAIN01")]
		public static async Task CASTLE95_NPC_MAIN01(Dialog dialog)
		{
			await dialog.Msg("CASTLE95_NPC_MAIN01_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_NPC_01")]
		public static async Task CASTLE95_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_NPC_02")]
		public static async Task CASTLE95_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_NPC_03")]
		public static async Task CASTLE95_NPC_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_01")]
		public static async Task CASTLE95_OBELISK_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_01_BROKEN")]
		public static async Task CASTLE95_OBELISK_01_BROKEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_02")]
		public static async Task CASTLE95_OBELISK_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_02_BROKEN")]
		public static async Task CASTLE95_OBELISK_02_BROKEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_PORTAL")]
		public static async Task CASTLE95_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_NPC_MAIN02")]
		public static async Task CASTLE95_NPC_MAIN02(Dialog dialog)
		{
			await dialog.Msg("CASTLE95_NPC_MAIN02_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_NPC_04")]
		public static async Task CASTLE95_NPC_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_NPC_05")]
		public static async Task CASTLE95_NPC_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_NPC_06")]
		public static async Task CASTLE95_NPC_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_01")]
		public static async Task CASTLE95_SETTING_POINT_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_02")]
		public static async Task CASTLE95_SETTING_POINT_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_03")]
		public static async Task CASTLE95_SETTING_POINT_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_04")]
		public static async Task CASTLE95_SETTING_POINT_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_BOX_01")]
		public static async Task CASTLE95_SETTING_POINT_BOX_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_BOX_02")]
		public static async Task CASTLE95_SETTING_POINT_BOX_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_BOX_03")]
		public static async Task CASTLE95_SETTING_POINT_BOX_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_SETTING_POINT_BOX_04")]
		public static async Task CASTLE95_SETTING_POINT_BOX_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_04")]
		public static async Task CASTLE95_OBELISK_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_03")]
		public static async Task CASTLE95_OBELISK_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_04_BROKEN")]
		public static async Task CASTLE95_OBELISK_04_BROKEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBELISK_03_BROKEN")]
		public static async Task CASTLE95_OBELISK_03_BROKEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_MAIN07_DEVICE")]
		public static async Task CASTLE95_MAIN07_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE95_OBJECT")]
		public static async Task CASTLE95_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE96_MQ_TADAS_NPC_1")]
		public static async Task CASTLE96_MQ_TADAS_NPC_1(Dialog dialog)
		{
			await dialog.Msg("CASTLE96_MQ_TADAS_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE96_MQ_TADAS_NPC_2")]
		public static async Task CASTLE96_MQ_TADAS_NPC_2(Dialog dialog)
		{
			await dialog.Msg("CASTLE96_MQ_TADAS_NPC_2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE96_MQ_WOLKE_NPC")]
		public static async Task CASTLE96_MQ_WOLKE_NPC(Dialog dialog)
		{
			await dialog.Msg("CASTLE96_MQ_WOLKE_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE96_MQ_NERGUI_NPC")]
		public static async Task CASTLE96_MQ_NERGUI_NPC(Dialog dialog)
		{
			await dialog.Msg("CASTLE96_MQ_NERGUI_NPC_baisc1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE96_MQ_10_NPC")]
		public static async Task CASTLE96_MQ_10_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_96_TO_D_UNDERAQUEDUCT")]
		public static async Task F_CASTLE_96_TO_D_UNDERAQUEDUCT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE98_MQ_INETA_NPC")]
		public static async Task CASTLE98_MQ_INETA_NPC(Dialog dialog)
		{
			await dialog.Msg("CASTLE98_MQ_INETA_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE98_MQ_2_NPC")]
		public static async Task CASTLE98_MQ_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE98_MQ_3_NPC")]
		public static async Task CASTLE98_MQ_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE98_MQ_4_NPC")]
		public static async Task CASTLE98_MQ_4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_97_MQ_01_NPC2")]
		public static async Task F_CASTLE_97_MQ_01_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_CASTLE_97_MQ_01_NPC2_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_97_MQ_01_NPC")]
		public static async Task F_CASTLE_97_MQ_01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_97_MQ_03_NPC")]
		public static async Task F_CASTLE_97_MQ_03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_97_MQ_04_NPC")]
		public static async Task F_CASTLE_97_MQ_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_97_MQ_05_NPC")]
		public static async Task F_CASTLE_97_MQ_05_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_97_MQ_05_NPC2")]
		public static async Task F_CASTLE_97_MQ_05_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_CASTLE_97_MQ_05_NPC2_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_CASTLE_99")]
		public static async Task WARP_F_CASTLE_99(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("F_CASTLE_99_MQ_01_NPC")]
		public static async Task F_CASTLE_99_MQ_01_NPC(Dialog dialog)
		{
			switch (await dialog.Select("F_CASTLE_99_MQ_01_ST", "F_CASTLE_99_MQ_01", "@dicID_^*$QUEST_LV_0400_20200129_006039$*^", "@dicID_^*$QUEST_LV_0400_20200129_006040$*^"))
			{
				case 1:
					//dialog.Player.Quests.Start(80454);
					break;
			}
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_NPC")]
		public static async Task F_CASTLE_99_MQ_05_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_06_NPC")]
		public static async Task F_CASTLE_99_MQ_06_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_07_NPC")]
		public static async Task F_CASTLE_99_MQ_07_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_07_NPC2")]
		public static async Task F_CASTLE_99_MQ_07_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_CASTLE_99_MQ_07_NPC2_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_07_NPC3")]
		public static async Task F_CASTLE_99_MQ_07_NPC3(Dialog dialog)
		{
			await dialog.Msg("F_CASTLE_99_MQ_07_NPC3_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_04_NPC")]
		public static async Task F_CASTLE_99_MQ_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_07_NPC4")]
		public static async Task F_CASTLE_99_MQ_07_NPC4(Dialog dialog)
		{
			await dialog.Msg("F_CASTLE_99_MQ_07_NPC4_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_OBJ_1")]
		public static async Task F_CASTLE_99_MQ_05_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_OBJ_2")]
		public static async Task F_CASTLE_99_MQ_05_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_OBJ_3")]
		public static async Task F_CASTLE_99_MQ_05_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_OBJ_4")]
		public static async Task F_CASTLE_99_MQ_05_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_OBJ_5")]
		public static async Task F_CASTLE_99_MQ_05_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_OBJ_6")]
		public static async Task F_CASTLE_99_MQ_05_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_CASTLE_99_MQ_05_OBJ_7")]
		public static async Task F_CASTLE_99_MQ_05_OBJ_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_CASTLE_101")]
		public static async Task WARP_F_CASTLE_101(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("WARP_F_DCAPITAL_101_TO_D_DCAPITAL_108")]
		public static async Task WARP_F_DCAPITAL_101_TO_D_DCAPITAL_108(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_MASTER_01")]
		public static async Task EP12_2_F_CASTLE_101_MQ_MASTER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ02_BOWER01")]
		public static async Task EP12_2_F_CASTLE_101_MQ02_BOWER01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ02_BOWER02")]
		public static async Task EP12_2_F_CASTLE_101_MQ02_BOWER02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ02_BOWER03")]
		public static async Task EP12_2_F_CASTLE_101_MQ02_BOWER03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ02_BOWER04")]
		public static async Task EP12_2_F_CASTLE_101_MQ02_BOWER04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ02_BOWER05")]
		public static async Task EP12_2_F_CASTLE_101_MQ02_BOWER05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ03_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ03_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_GIRL_01")]
		public static async Task EP12_2_F_CASTLE_101_MQ_GIRL_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_MASTER_02")]
		public static async Task EP12_2_F_CASTLE_101_MQ_MASTER_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ03_1_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ03_1_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ03_2_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ03_2_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ03_3_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ03_3_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ03_02_REMAINS")]
		public static async Task EP12_2_F_CASTLE_101_MQ03_02_REMAINS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ03_3_BRANCH")]
		public static async Task EP12_2_F_CASTLE_101_MQ03_3_BRANCH(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ04_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ04_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ04_1_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ04_1_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ04_2_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ04_2_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ05_STONE")]
		public static async Task EP12_2_F_CASTLE_101_MQ05_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_GIRL_02")]
		public static async Task EP12_2_F_CASTLE_101_MQ_GIRL_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_MASTER_03")]
		public static async Task EP12_2_F_CASTLE_101_MQ_MASTER_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_GIRL_03")]
		public static async Task EP12_2_F_CASTLE_101_MQ_GIRL_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_GIRL_04")]
		public static async Task EP12_2_F_CASTLE_101_MQ_GIRL_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_F_CASTLE_101_MQ_MASTER_04")]
		public static async Task EP12_2_F_CASTLE_101_MQ_MASTER_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE102_AUSTEJA_01")]
		public static async Task CASTLE102_AUSTEJA_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE102_RAIMA_WHEEL")]
		public static async Task CASTLE102_RAIMA_WHEEL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_01")]
		public static async Task DCAPITAL103_SQ_01(Dialog dialog)
		{
			await dialog.Msg("DCAPITAL103_SQ_01_basic1");
			await dialog.Msg("DCAPITAL103_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_07")]
		public static async Task DCAPITAL103_SQ_07(Dialog dialog)
		{
			await dialog.Msg("DCAPITAL103_SQ_07_basic1");
			await dialog.Msg("DCAPITAL103_SQ_07_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_11")]
		public static async Task DCAPITAL103_SQ_11(Dialog dialog)
		{
			await dialog.Msg("DCAPITAL103_SQ_11_basic1");
			await dialog.Msg("DCAPITAL103_SQ_11_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_01_1")]
		public static async Task DCAPITAL103_SQ_01_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_07_1")]
		public static async Task DCAPITAL103_SQ_07_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_09")]
		public static async Task DCAPITAL103_SQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_11_1")]
		public static async Task DCAPITAL103_SQ_11_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_03")]
		public static async Task DCAPITAL103_SQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_04_1")]
		public static async Task DCAPITAL103_SQ_04_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_04_2")]
		public static async Task DCAPITAL103_SQ_04_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_04_3")]
		public static async Task DCAPITAL103_SQ_04_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_04_4")]
		public static async Task DCAPITAL103_SQ_04_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL103_SQ_04_5")]
		public static async Task DCAPITAL103_SQ_04_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_DCAPITAL_1031000")]
		public static async Task TREASUREBOX_LV_F_DCAPITAL_1031000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_294", 1);

		[DialogFunction("HT2_DCAPITAL_103_DOC")]
		public static async Task HT2_DCAPITAL_103_DOC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_103_SHADOW")]
		public static async Task DCAPITAL_103_SHADOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_103_SHADOW_DEVICE")]
		public static async Task DCAPITAL_103_SHADOW_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_FINALE_RAIMA02")]
		public static async Task EP12_FINALE_RAIMA02(Dialog dialog)
		{
			await dialog.Msg("EP12_RAIMA_NORMAL_01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_FINALE_RAIMA01")]
		public static async Task EP12_FINALE_RAIMA01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL105_GRISIA_NPC_1")]
		public static async Task DCAPITAL105_GRISIA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL105_GRISIA_NPC_2")]
		public static async Task DCAPITAL105_GRISIA_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL105_KUPOLE_NPC")]
		public static async Task DCAPITAL105_KUPOLE_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL105_SQ_2_NPC")]
		public static async Task DCAPITAL105_SQ_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL105_SQ_3_NPC")]
		public static async Task DCAPITAL105_SQ_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL105_SQ_5_NPC")]
		public static async Task DCAPITAL105_SQ_5_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_105_EV_55_001")]
		public static async Task DCAPITAL_105_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("EP12_PRELUDE_NERINGA_DCAPITAL_105A")]
		public static async Task EP12_PRELUDE_NERINGA_DCAPITAL_105A(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_NERINGA_DCAPITAL_105B")]
		public static async Task EP12_PRELUDE_NERINGA_DCAPITAL_105B(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_Namot01")]
		public static async Task EP12_PRELUDE_Namot01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_Namot02")]
		public static async Task EP12_PRELUDE_Namot02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_Namot03")]
		public static async Task EP12_PRELUDE_Namot03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_Namot04")]
		public static async Task EP12_PRELUDE_Namot04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_HIDDEN_OBJECT01")]
		public static async Task EP12_PRELUDE_HIDDEN_OBJECT01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_HIDDEN_OBJECT02")]
		public static async Task EP12_PRELUDE_HIDDEN_OBJECT02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_HIDDEN_OBJECT03")]
		public static async Task EP12_PRELUDE_HIDDEN_OBJECT03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_HIDDEN_OBJECT04")]
		public static async Task EP12_PRELUDE_HIDDEN_OBJECT04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL106_GRISIA_NPC_1")]
		public static async Task DCAPITAL106_GRISIA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL106_GRISIA_NPC_2")]
		public static async Task DCAPITAL106_GRISIA_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL106_ZUSAIA_NPC_2")]
		public static async Task DCAPITAL106_ZUSAIA_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL106_ZUSAIA_NPC_1")]
		public static async Task DCAPITAL106_ZUSAIA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_DCAPITAL_106")]
		public static async Task WARP_DCAPITAL_106(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("EP12_PRELUDE_NERINGA_DCAPITAL_106A")]
		public static async Task EP12_PRELUDE_NERINGA_DCAPITAL_106A(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_NERINGA_DCAPITAL_106B")]
		public static async Task EP12_PRELUDE_NERINGA_DCAPITAL_106B(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_NERINGA_DCAPITAL_106C")]
		public static async Task EP12_PRELUDE_NERINGA_DCAPITAL_106C(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_PRELUDE_WATER")]
		public static async Task EP12_PRELUDE_WATER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL107_GRISIA_NPC_1")]
		public static async Task DCAPITAL107_GRISIA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL107_BLAD_NPC_4")]
		public static async Task DCAPITAL107_BLAD_NPC_4(Dialog dialog)
		{
			await dialog.Msg("DCAPITAL107_BLAD_NPC_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL107_BLAD_NPC_1")]
		public static async Task DCAPITAL107_BLAD_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL107_BLAD_NPC_2")]
		public static async Task DCAPITAL107_BLAD_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL107_BLAD_NPC_3")]
		public static async Task DCAPITAL107_BLAD_NPC_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL107_SQ_2_NPC")]
		public static async Task DCAPITAL107_SQ_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL107_SQ_4_NPC")]
		public static async Task DCAPITAL107_SQ_4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ASIOMAGE_ENTER_01")]
		public static async Task ASIOMAGE_ENTER_01(Dialog dialog)
		{
			await dialog.Msg("INSTANCE_DUNGEON_select19");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CASTLE_19_1_MQ_PRE_NPC")]
		public static async Task D_CASTLE_19_1_MQ_PRE_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_DCAPITAL_108_TO_F_CASTLE_101")]
		public static async Task WARP_D_DCAPITAL_108_TO_F_CASTLE_101(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ01_MASTER")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ01_MASTER(Dialog dialog)
		{
			await dialog.Msg("EP12_2_D_DCAPITAL_108_MQ01_MASTER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ02_GIRL")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ02_GIRL(Dialog dialog)
		{
			await dialog.Msg("EP12_2_D_DCAPITAL_108_MQ02_GIRL_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ03_JANGCHI")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ03_JANGCHI(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ05_01")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ05_02")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ05_03")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ05_04")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ05_05")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ05_06")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ05_07")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ08_02")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ08_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ11_01")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ11_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ15_GIRL")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ15_GIRL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_DCAPITAL_108_TO_DEVINE_SANCTUARY")]
		public static async Task WARP_D_DCAPITAL_108_TO_DEVINE_SANCTUARY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_DEVINE_SANCTUARY_TO_D_DCAPITAL_108")]
		public static async Task WARP_DEVINE_SANCTUARY_TO_D_DCAPITAL_108(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ17_RAIMA_01")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ17_RAIMA_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LEGEND_RAID_GILTINE_PORTAL")]
		public static async Task LEGEND_RAID_GILTINE_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ17_GILTINE_01")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ17_GILTINE_01(Dialog dialog)
		{
			await dialog.Msg("EP12_2_D_DCAPITAL_108_MQ17_GILTINE_01_basic_01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ17_GILTINE_02")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ17_GILTINE_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ17_RANGDAGIRL_01")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ17_RANGDAGIRL_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ17_RANGDAMASTER_01")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ17_RANGDAMASTER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ18_GIRL_01")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ18_GIRL_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_108_MQ18_GIRL_02")]
		public static async Task EP12_2_D_DCAPITAL_108_MQ18_GIRL_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_DCAPITAL_OUT_OF_SECRET_TO_DEVINE_SANCTUARY")]
		public static async Task WARP_D_DCAPITAL_OUT_OF_SECRET_TO_DEVINE_SANCTUARY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_DCAPITAL_OUT_OF_SECRET_TO_DEVINE_SANCTUARY2")]
		public static async Task WARP_D_DCAPITAL_OUT_OF_SECRET_TO_DEVINE_SANCTUARY2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP12_2_D_DCAPITAL_SECRET_1")]
		public static async Task EP12_2_D_DCAPITAL_SECRET_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_DCAPITAL_108_ZEMINA")]
		public static async Task D_DCAPITAL_108_ZEMINA(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("GILTINE_GO_GUILDMISSION")]
		public static async Task GILTINE_GO_GUILDMISSION(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_SQ_01")]
		public static async Task WHITETREES231_SQ_01(Dialog dialog)
		{
			await dialog.Msg("WHITETREES231_SQ_01_basic1");
			await dialog.Msg("WHITETREES231_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_SQ_05")]
		public static async Task WHITETREES231_SQ_05(Dialog dialog)
		{
			await dialog.Msg("WHITETREES231_SQ_05_basic1");
			await dialog.Msg("WHITETREES231_SQ_05_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_SQ_11_1")]
		public static async Task WHITETREES231_SQ_11_1(Dialog dialog)
		{
			await dialog.Msg("WHITETREES231_SQ_11_1_basic1");
			await dialog.Msg("WHITETREES231_SQ_11_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_SQ_01_1")]
		public static async Task WHITETREES231_SQ_01_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_WARNING")]
		public static async Task WHITETREES231_WARNING(Dialog dialog)
		{
			await dialog.Msg("WHITETREES231_SQ_warn1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_SQ_09_1")]
		public static async Task WHITETREES231_SQ_09_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_SQ_09_2")]
		public static async Task WHITETREES231_SQ_09_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_SQ_09_3")]
		public static async Task WHITETREES231_SQ_09_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES231_WARP_BOARD3")]
		public static async Task WHITETREES231_WARP_BOARD3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_WHITETREES_23_11000")]
		public static async Task TREASUREBOX_LV_F_WHITETREES_23_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_301", 1);

		[DialogFunction("WARP_C_MAPLE_23_2")]
		public static async Task WARP_C_MAPLE_23_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("MAPLE232_SQ_01")]
		public static async Task MAPLE232_SQ_01(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_01_basic1");
			await dialog.Msg("MAPLE232_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_02")]
		public static async Task MAPLE232_SQ_02(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_02_basic1");
			await dialog.Msg("MAPLE232_SQ_02_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_04")]
		public static async Task MAPLE232_SQ_04(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_04_basic1");
			await dialog.Msg("MAPLE232_SQ_04_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_07_1")]
		public static async Task MAPLE232_SQ_07_1(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_07_1_basic1");
			await dialog.Msg("MAPLE232_SQ_07_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_07_2")]
		public static async Task MAPLE232_SQ_07_2(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_07_2_basic1");
			await dialog.Msg("MAPLE232_SQ_07_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_13_1")]
		public static async Task MAPLE232_SQ_13_1(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_13_1_basic1");
			await dialog.Msg("MAPLE232_SQ_13_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_13_2")]
		public static async Task MAPLE232_SQ_13_2(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_13_2_basic1");
			await dialog.Msg("MAPLE232_SQ_13_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_04_1")]
		public static async Task MAPLE232_SQ_04_1(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_04_1_basic1");
			await dialog.Msg("MAPLE232_SQ_04_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_07_3")]
		public static async Task MAPLE232_SQ_07_3(Dialog dialog)
		{
			await dialog.Msg("MAPLE232_SQ_07_3_basic1");
			await dialog.Msg("MAPLE232_SQ_07_3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_02_1")]
		public static async Task MAPLE232_SQ_02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_04_2")]
		public static async Task MAPLE232_SQ_04_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_06")]
		public static async Task MAPLE232_SQ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_09")]
		public static async Task MAPLE232_SQ_09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE232_SQ_13_3")]
		public static async Task MAPLE232_SQ_13_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_MAPLE_23_21000")]
		public static async Task TREASUREBOX_LV_F_MAPLE_23_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_302", 1);

		[DialogFunction("MAPLE232_KQ_1_NPC")]
		public static async Task MAPLE232_KQ_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PAYAUTA_EP11_NPC_232")]
		public static async Task PAYAUTA_EP11_NPC_232(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PAYAUTA_EP11_SIGN_232")]
		public static async Task PAYAUTA_EP11_SIGN_232(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PAYAUTA_EP11_8_OBJ_1")]
		public static async Task PAYAUTA_EP11_8_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PAYAUTA_EP11_NPC_232_2")]
		public static async Task PAYAUTA_EP11_NPC_232_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_01")]
		public static async Task WHITETREES233_SQ_01(Dialog dialog)
		{
			await dialog.Msg("WHITETREES233_SQ_01_basic1");
			await dialog.Msg("WHITETREES233_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_01_1")]
		public static async Task WHITETREES233_SQ_01_1(Dialog dialog)
		{
			await dialog.Msg("WHITETREES233_SQ_01_1_basic1");
			await dialog.Msg("WHITETREES233_SQ_01_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_08")]
		public static async Task WHITETREES233_SQ_08(Dialog dialog)
		{
			await dialog.Msg("WHITETREES233_SQ_08_basic1");
			await dialog.Msg("WHITETREES233_SQ_08_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_07")]
		public static async Task WHITETREES233_SQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_01_2")]
		public static async Task WHITETREES233_SQ_01_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_06_1")]
		public static async Task WHITETREES233_SQ_06_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_06_2")]
		public static async Task WHITETREES233_SQ_06_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_06_3")]
		public static async Task WHITETREES233_SQ_06_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_06_4")]
		public static async Task WHITETREES233_SQ_06_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES233_SQ_06_5")]
		public static async Task WHITETREES233_SQ_06_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_WHITETREES_23_31000")]
		public static async Task TREASUREBOX_LV_F_WHITETREES_23_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_303", 1);

		[DialogFunction("WHITETREES_23_3_TO_GUILDMISSION")]
		public static async Task WHITETREES_23_3_TO_GUILDMISSION(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND281_SQ_01")]
		public static async Task TABLELAND281_SQ_01(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_1_SQ_01_basic01");
			await dialog.Msg("TABLELAND28_1_SQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND281_SQ_02")]
		public static async Task TABLELAND281_SQ_02(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_1_SQ_02_basic01");
			await dialog.Msg("TABLELAND28_1_SQ_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND281_SQ_06")]
		public static async Task TABLELAND281_SQ_06(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_1_SQ_06_basic01");
			await dialog.Msg("TABLELAND28_1_SQ_06_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND281_SQ_08")]
		public static async Task TABLELAND281_SQ_08(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_1_SQ_08_basic01");
			await dialog.Msg("TABLELAND28_1_SQ_08_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND281_SQ_04")]
		public static async Task TABLELAND281_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND281_SQ_08_STONE")]
		public static async Task TABLELAND281_SQ_08_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SHINOBI_MASTER")]
		public static async Task SHINOBI_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_SHINOBI_MASTER_basic1");
			await dialog.Msg("JOB_SHINOBI_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_28_11000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_28_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_264", 1);

		[DialogFunction("TABLELAND282_SQ_01")]
		public static async Task TABLELAND282_SQ_01(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_2_SQ_01_basic01");
			await dialog.Msg("TABLELAND28_2_SQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_02")]
		public static async Task TABLELAND282_SQ_02(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_2_SQ_02_basic01");
			await dialog.Msg("TABLELAND28_2_SQ_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_05")]
		public static async Task TABLELAND282_SQ_05(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_2_SQ_05_basic01");
			await dialog.Msg("TABLELAND28_2_SQ_05_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_06")]
		public static async Task TABLELAND282_SQ_06(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_2_SQ_06_basic01");
			await dialog.Msg("TABLELAND28_2_SQ_06_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_07")]
		public static async Task TABLELAND282_SQ_07(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_2_SQ_07_basic01");
			await dialog.Msg("TABLELAND28_2_SQ_07_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_05_P")]
		public static async Task TABLELAND282_SQ_05_P(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_2_SQ_05_P_basic01");
			await dialog.Msg("TABLELAND28_2_SQ_05_P_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_03_ROOT")]
		public static async Task TABLELAND282_SQ_03_ROOT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_04_SPEAR")]
		public static async Task TABLELAND282_SQ_04_SPEAR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_07_BOSS")]
		public static async Task TABLELAND282_SQ_07_BOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND282_SQ_01_R")]
		public static async Task TABLELAND282_SQ_01_R(Dialog dialog)
		{
			await dialog.Msg("TABLELAND28_2_SQ_01_R_basic01");
			await dialog.Msg("TABLELAND28_2_SQ_01_R_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_TABLELAND_28_2")]
		public static async Task WARP_F_TABLELAND_28_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_28_21000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_28_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_265", 1);

		[DialogFunction("TABLELAND_28_2_TO_CATHEDRAL_78_1")]
		public static async Task TABLELAND_28_2_TO_CATHEDRAL_78_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FREE_DUNGEON_SIGN4")]
		public static async Task FREE_DUNGEON_SIGN4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_CRUSADER_NPC")]
		public static async Task MASTER_CRUSADER_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_CRUSADER_NPC_basic1");
			await dialog.Msg("MASTER_CRUSADER_NPC_basic2");
			await dialog.Msg("MASTER_CRUSADER_NPC_basic_move");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NPC_CRUSADER_01")]
		public static async Task NPC_CRUSADER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NPC_CRUSADER_02")]
		public static async Task NPC_CRUSADER_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LEGEND_RAID_GLACIER_PORTAL")]
		public static async Task LEGEND_RAID_GLACIER_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_TABLELAND_28_2_RAID_05_NPC_01")]
		public static async Task F_TABLELAND_28_2_RAID_05_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_TABLELAND_28_2_RAID_05_NPC_03")]
		public static async Task F_TABLELAND_28_2_RAID_05_NPC_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_TABLELAND_28_2_RAID_05_NPC_02")]
		public static async Task F_TABLELAND_28_2_RAID_05_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_TABLELAND_28_2_RAID_05_NPC_04")]
		public static async Task F_TABLELAND_28_2_RAID_05_NPC_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_TABLELAND_28_2_RAID_10_NPC_06")]
		public static async Task F_TABLELAND_28_2_RAID_10_NPC_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_KATYN_45_1")]
		public static async Task WARP_F_KATYN_45_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("KATYN_45_1_AJEL1")]
		public static async Task KATYN_45_1_AJEL1(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_1_SQ_1_BG");
			await dialog.Msg("KATYN_45_1_SQ_1_BG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_OWL1")]
		public static async Task KATYN_45_1_OWL1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_AJEL2")]
		public static async Task KATYN_45_1_AJEL2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_OWL2")]
		public static async Task KATYN_45_1_OWL2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_OWL3")]
		public static async Task KATYN_45_1_OWL3(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_1_SQ_9_BS");
			await dialog.Msg("KATYN_45_1_SQ_8_SU1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_AJEL3")]
		public static async Task KATYN_45_1_AJEL3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_6_HERB")]
		public static async Task KATYN_45_1_SQ_6_HERB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE4")]
		public static async Task KATYN_45_1_SQ_10_STONE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE5")]
		public static async Task KATYN_45_1_SQ_10_STONE5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE6")]
		public static async Task KATYN_45_1_SQ_10_STONE6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE3")]
		public static async Task KATYN_45_1_SQ_10_STONE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE2")]
		public static async Task KATYN_45_1_SQ_10_STONE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE1")]
		public static async Task KATYN_45_1_SQ_10_STONE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_9_STONE")]
		public static async Task KATYN_45_1_SQ_9_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_EFF1")]
		public static async Task KATYN_45_1_SQ_10_EFF1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_EFF2")]
		public static async Task KATYN_45_1_SQ_10_EFF2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE10")]
		public static async Task KATYN_45_1_SQ_10_STONE10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE11")]
		public static async Task KATYN_45_1_SQ_10_STONE11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE12")]
		public static async Task KATYN_45_1_SQ_10_STONE12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE7")]
		public static async Task KATYN_45_1_SQ_10_STONE7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE8")]
		public static async Task KATYN_45_1_SQ_10_STONE8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_1_SQ_10_STONE9")]
		public static async Task KATYN_45_1_SQ_10_STONE9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_KATYN_45_11000")]
		public static async Task TREASUREBOX_LV_F_KATYN_45_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_279", 1);

		[DialogFunction("KATYN_45_2_NUMBER_BASEBALL")]
		public static async Task KATYN_45_2_NUMBER_BASEBALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_AJEL1")]
		public static async Task KATYN_45_2_AJEL1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_ESOL")]
		public static async Task KATYN_45_2_ESOL(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_2_ESOL_BG");
			await dialog.Msg("KATYN_45_2_ESOL_BG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_SCULPTOR1")]
		public static async Task KATYN_45_2_SCULPTOR1(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_2_SQ_8_BS2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_SCULPTOR2")]
		public static async Task KATYN_45_2_SCULPTOR2(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_2_SCULPTOR2_BG");
			await dialog.Msg("KATYN_45_2_SCULPTOR2_BG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_OWL1")]
		public static async Task KATYN_45_2_OWL1(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_2_OWL1_BG");
			await dialog.Msg("KATYN_45_2_OWL1_BG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_OWL2")]
		public static async Task KATYN_45_2_OWL2(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_2_OWL2_BG");
			await dialog.Msg("KATYN_45_2_OWL2_BG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_OWL3")]
		public static async Task KATYN_45_2_OWL3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_OWL4")]
		public static async Task KATYN_45_2_OWL4(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_2_OWL4_BG");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_SQ_5_DARK")]
		public static async Task KATYN_45_2_SQ_5_DARK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_AJEL2")]
		public static async Task KATYN_45_2_AJEL2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_SQ_9_HERB")]
		public static async Task KATYN_45_2_SQ_9_HERB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_2_AJEL3")]
		public static async Task KATYN_45_2_AJEL3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_KATYN_45_21000")]
		public static async Task TREASUREBOX_LV_F_KATYN_45_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_280", 1);

		[DialogFunction("WARP_F_KATYN_45_3")]
		public static async Task WARP_F_KATYN_45_3(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("KATYN_45_3_NUMBER_MIXING")]
		public static async Task KATYN_45_3_NUMBER_MIXING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL1")]
		public static async Task KATYN_45_3_OWL1(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_3_SQ_13_BS2");
			await dialog.Msg("KATYN_45_3_SQ_13_BS");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_SQ_14_SOUL")]
		public static async Task KATYN_45_3_SQ_14_SOUL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_AJEL1")]
		public static async Task KATYN_45_3_AJEL1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_AJEL2")]
		public static async Task KATYN_45_3_AJEL2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL2")]
		public static async Task KATYN_45_3_OWL2(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_3_OWL2_BG");
			await dialog.Msg("KATYN_45_3_OWL2_BG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL4")]
		public static async Task KATYN_45_3_OWL4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_AJEL3")]
		public static async Task KATYN_45_3_AJEL3(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_3_SQ_15_ST");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL3")]
		public static async Task KATYN_45_3_OWL3(Dialog dialog)
		{
			await dialog.Msg("KATYN_45_3_SQ_9_BS2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_GRASS")]
		public static async Task KATYN_45_3_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_AJEL4")]
		public static async Task KATYN_45_3_AJEL4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_DARKSMOKE")]
		public static async Task KATYN_45_3_DARKSMOKE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL5_3")]
		public static async Task KATYN_45_3_OWL5_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL5_2")]
		public static async Task KATYN_45_3_OWL5_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL5_1")]
		public static async Task KATYN_45_3_OWL5_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_45_3_OWL4PURI")]
		public static async Task KATYN_45_3_OWL4PURI(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_HACKAPELL_8_1_OBJ")]
		public static async Task JOB_HACKAPELL_8_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_KATYN_45_31000")]
		public static async Task TREASUREBOX_LV_F_KATYN_45_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_281", 1);

		[DialogFunction("KATYN_45_3_TO_CATACOMB_80_1")]
		public static async Task KATYN_45_3_TO_CATACOMB_80_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FREE_DUNGEON_SIGN7")]
		public static async Task FREE_DUNGEON_SIGN7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_MQ_01")]
		public static async Task FARM491_MQ_01(Dialog dialog)
		{
			await dialog.Msg("FARM49_1_MQ_01");
			await dialog.Msg("FARM49_1_MQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_SQ_01")]
		public static async Task FARM491_SQ_01(Dialog dialog)
		{
			await dialog.Msg("FARM49_1_FARMER_01");
			await dialog.Msg("FARM491_SQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_SQ_03")]
		public static async Task FARM491_SQ_03(Dialog dialog)
		{
			await dialog.Msg("FARM49_1_SQ_03");
			await dialog.Msg("FARM491_SQ_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_SQ_05")]
		public static async Task FARM491_SQ_05(Dialog dialog)
		{
			await dialog.Msg("FARM49_1_SQ_05");
			await dialog.Msg("FARM491_SQ_05_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_MQ_02_1")]
		public static async Task FARM491_MQ_02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_MQ_02_2")]
		public static async Task FARM491_MQ_02_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_DANDELION")]
		public static async Task FARM491_DANDELION(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_SQ_04_BUNDLE")]
		public static async Task FARM491_SQ_04_BUNDLE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM491_SQ_06")]
		public static async Task FARM491_SQ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_FARM_49_1")]
		public static async Task WARP_F_FARM_49_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_FARM_49_142")]
		public static async Task TREASUREBOX_LV_F_FARM_49_142(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_187", 1);

		[DialogFunction("FARM491_SQ_07")]
		public static async Task FARM491_SQ_07(Dialog dialog)
		{
			await dialog.Msg("FARM49_1_SQ_07_basic01");
			await dialog.Msg("FARM49_1_SQ_07_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM49_1_SQ08_OBJ1")]
		public static async Task FARM49_1_SQ08_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BULLETMARKER_MASTER")]
		public static async Task BULLETMARKER_MASTER(Dialog dialog)
		{
			await dialog.Msg("BULLETMARKER_MASTER_basic1");
			await dialog.Msg("BULLETMARKER_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_01")]
		public static async Task FARM492_MQ_01(Dialog dialog)
		{
			await dialog.Msg("FARM49_2_MQ_01");
			await dialog.Msg("FARM49_2_MQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_02")]
		public static async Task FARM492_SQ_02(Dialog dialog)
		{
			await dialog.Msg("FARM492_SQ_02_basic01");
			await dialog.Msg("FARM492_SQ_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_03")]
		public static async Task FARM492_SQ_03(Dialog dialog)
		{
			await dialog.Msg("FARM49_2_SQ_03");
			await dialog.Msg("FARM49_2_SQ_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_04")]
		public static async Task FARM492_MQ_04(Dialog dialog)
		{
			await dialog.Msg("FARM49_2_MQ_04");
			await dialog.Msg("FARM49_2_MQ_04_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_05")]
		public static async Task FARM492_SQ_05(Dialog dialog)
		{
			await dialog.Msg("FARM49_2_SQ_05");
			await dialog.Msg("FARM492_SQ_05_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_02_ROOF")]
		public static async Task FARM492_MQ_02_ROOF(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_02_WOODBOX")]
		public static async Task FARM492_MQ_02_WOODBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_04_FARMER")]
		public static async Task FARM492_MQ_04_FARMER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_03_GRAPE")]
		public static async Task FARM492_SQ_03_GRAPE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_04_BUSH")]
		public static async Task FARM492_SQ_04_BUSH(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_06_DUMMY")]
		public static async Task FARM492_SQ_06_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_06")]
		public static async Task FARM492_SQ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_04_CURED01")]
		public static async Task FARM492_MQ_04_CURED01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_04_CURED02")]
		public static async Task FARM492_MQ_04_CURED02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_04_CURED03")]
		public static async Task FARM492_MQ_04_CURED03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FARM_49_251")]
		public static async Task TREASUREBOX_LV_F_FARM_49_251(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_188", 1);

		[DialogFunction("FARM492_MQ_06")]
		public static async Task FARM492_MQ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_MQ_06_2")]
		public static async Task FARM492_MQ_06_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_07")]
		public static async Task FARM492_SQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM492_SQ_08")]
		public static async Task FARM492_SQ_08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_MQ_01")]
		public static async Task FARM493_MQ_01(Dialog dialog)
		{
			await dialog.Msg("FARM49_3_MQ_01_basic04");
			await dialog.Msg("FARM493_MQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_MQ_03")]
		public static async Task FARM493_MQ_03(Dialog dialog)
		{
			await dialog.Msg("FARM49_3_MQ_03");
			await dialog.Msg("FARM49_3_MQ_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_MQ_02")]
		public static async Task FARM493_MQ_02(Dialog dialog)
		{
			await dialog.Msg("FARM49_3_MQ_02");
			await dialog.Msg("FARM49_3_MQ_02_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_SQ_03")]
		public static async Task FARM493_SQ_03(Dialog dialog)
		{
			await dialog.Msg("FARM49_3_SQ_03");
			await dialog.Msg("FARM49_3_SQ_03_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_SQ_01")]
		public static async Task FARM493_SQ_01(Dialog dialog)
		{
			await dialog.Msg("FARM49_3_SQ_01");
			await dialog.Msg("FARM49_3_SQ_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_SQ_06")]
		public static async Task FARM493_SQ_06(Dialog dialog)
		{
			await dialog.Msg("FARM493_SQ_06_basic01");
			await dialog.Msg("FARM493_SQ_06_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP01")]
		public static async Task FARM493_CAVEWARP01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP02")]
		public static async Task FARM493_CAVEWARP02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP03")]
		public static async Task FARM493_CAVEWARP03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP04")]
		public static async Task FARM493_CAVEWARP04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP05")]
		public static async Task FARM493_CAVEWARP05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP06")]
		public static async Task FARM493_CAVEWARP06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP07")]
		public static async Task FARM493_CAVEWARP07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP08")]
		public static async Task FARM493_CAVEWARP08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP09")]
		public static async Task FARM493_CAVEWARP09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP10")]
		public static async Task FARM493_CAVEWARP10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP11")]
		public static async Task FARM493_CAVEWARP11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEEXIT")]
		public static async Task FARM493_CAVEEXIT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_SQ_02")]
		public static async Task FARM493_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM49_3_TO_VELNIASP54_1")]
		public static async Task FARM49_3_TO_VELNIASP54_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_FARM_49_3")]
		public static async Task WARP_F_FARM_49_3(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_FARM_49_360")]
		public static async Task TREASUREBOX_LV_F_FARM_49_360(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "R_TSF02_107", 1, "TreasureboxKey2");

		[DialogFunction("TREASUREBOX_LV_F_FARM_49_363")]
		public static async Task TREASUREBOX_LV_F_FARM_49_363(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_189", 1);

		[DialogFunction("FARM493_SQ_07")]
		public static async Task FARM493_SQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_SQ_07_OBJ")]
		public static async Task FARM493_SQ_07_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_SQ_08_OBJ1")]
		public static async Task FARM493_SQ_08_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM493_CAVEWARP12")]
		public static async Task FARM493_CAVEWARP12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_P_CATHEDRAL_1")]
		public static async Task WARP_P_CATHEDRAL_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("UPPER_WARNING_P_CATHEDRAL_1")]
		public static async Task UPPER_WARNING_P_CATHEDRAL_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MIKO_MASTER")]
		public static async Task MIKO_MASTER(Dialog dialog)
		{
			await dialog.Msg("MIKO_MASTER_basic1");
			await dialog.Msg("MIKO_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MIKO_SOUL_SPIRIT")]
		public static async Task MIKO_SOUL_SPIRIT(Dialog dialog)
		{
			await dialog.Msg("MIKO_SPIRIT_dlg1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_P_CATACOMB_1")]
		public static async Task WARP_P_CATACOMB_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("UPPER_WARNING_P_CATACOMB_1")]
		public static async Task UPPER_WARNING_P_CATACOMB_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_NECRO4_NPC")]
		public static async Task JOB_NECRO4_NPC(Dialog dialog)
		{
			await dialog.Msg("JOB_NECRO4_NPC_basic01");
			await dialog.Msg("JOB_NECRO4_NPCbasic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_MQ_01")]
		public static async Task THORN391_MQ_01(Dialog dialog)
		{
			await dialog.Msg("THORN39_1_MQ_01_basic01");
			await dialog.Msg("THORN39_1_MQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_MQ_04")]
		public static async Task THORN391_MQ_04(Dialog dialog)
		{
			await dialog.Msg("THORN39_1_MQ_04_basic01");
			await dialog.Msg("THORN39_1_MQ_04_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_SQ_01")]
		public static async Task THORN391_SQ_01(Dialog dialog)
		{
			await dialog.Msg("THORN391_SQ_01_basic01");
			await dialog.Msg("THORN391_SQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_SQ_02")]
		public static async Task THORN391_SQ_02(Dialog dialog)
		{
			await dialog.Msg("THORN39_1_SQ_02_basic01");
			await dialog.Msg("THORN39_1_SQ_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_MQ_02")]
		public static async Task THORN391_MQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_MQ_05")]
		public static async Task THORN391_MQ_05(Dialog dialog)
		{
			await dialog.Msg("THORN39_1_MQ_04_basic05");
			await dialog.Msg("THORN39_1_MQ_04_basic06");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_MQ_04_T")]
		public static async Task THORN391_MQ_04_T(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN391_SQ_02_H")]
		public static async Task THORN391_SQ_02_H(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_THORN_39_1")]
		public static async Task WARP_D_THORN_39_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_THORN_39_11000")]
		public static async Task TREASUREBOX_LV_D_THORN_39_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_204", 1);

		[DialogFunction("JOB_PLAGUEDOCTOR71_NPC2")]
		public static async Task JOB_PLAGUEDOCTOR71_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN_39_1_TO_GUILD_FIELD_BOSS")]
		public static async Task THORN_39_1_TO_GUILD_FIELD_BOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_MQ_02")]
		public static async Task THORN392_MQ_02(Dialog dialog)
		{
			await dialog.Msg("THORN392_MQ_02_basic01");
			await dialog.Msg("THORN392_MQ_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_MQ_03")]
		public static async Task THORN392_MQ_03(Dialog dialog)
		{
			await dialog.Msg("THORN39_2_MQ_03_basic01");
			await dialog.Msg("THORN39_2_MQ_03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_SQ_01")]
		public static async Task THORN392_SQ_01(Dialog dialog)
		{
			await dialog.Msg("THORN39_2_SQ_01_basic01");
			await dialog.Msg("THORN39_2_SQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_MQ_05")]
		public static async Task THORN392_MQ_05(Dialog dialog)
		{
			await dialog.Msg("THORN39_2_MQ_05_basic01");
			await dialog.Msg("THORN39_2_MQ_05_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_MQ_01")]
		public static async Task THORN392_MQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_MQ_03_P1")]
		public static async Task THORN392_MQ_03_P1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_MQ_03_P2")]
		public static async Task THORN392_MQ_03_P2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_MQ_03_P3")]
		public static async Task THORN392_MQ_03_P3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN392_SQ_01_P")]
		public static async Task THORN392_SQ_01_P(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_39_21000")]
		public static async Task TREASUREBOX_LV_D_THORN_39_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_203", 1);

		[DialogFunction("THORN392_SQ07_OBJ1")]
		public static async Task THORN392_SQ07_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN_39_2_TO_GUILD_FIELD_BOSS")]
		public static async Task THORN_39_2_TO_GUILD_FIELD_BOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_MQ_01")]
		public static async Task THORN393_MQ_01(Dialog dialog)
		{
			await dialog.Msg("THORN393_MQ_01_basic01");
			await dialog.Msg("THORN393_MQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_MQ_03")]
		public static async Task THORN393_MQ_03(Dialog dialog)
		{
			await dialog.Msg("THORN393_MQ_03_basic01");
			await dialog.Msg("THORN393_MQ_03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_SQ_01")]
		public static async Task THORN393_SQ_01(Dialog dialog)
		{
			await dialog.Msg("THORN393_SQ_01_basic01");
			await dialog.Msg("THORN393_SQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_SQ_03")]
		public static async Task THORN393_SQ_03(Dialog dialog)
		{
			await dialog.Msg("THORN39_3_SQ_03_basic01");
			await dialog.Msg("THORN39_3_SQ_03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_MQ_05_2")]
		public static async Task THORN393_MQ_05_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_MQ_05_3")]
		public static async Task THORN393_MQ_05_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_MQ_05_4")]
		public static async Task THORN393_MQ_05_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_MQ_05_5")]
		public static async Task THORN393_MQ_05_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_MQ_05_1")]
		public static async Task THORN393_MQ_05_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THORN393_SQ_02")]
		public static async Task THORN393_SQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_THORN_39_31000")]
		public static async Task TREASUREBOX_LV_D_THORN_39_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_205", 1);

		[DialogFunction("THORN39_3_SQ06_OBJ")]
		public static async Task THORN39_3_SQ06_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_01")]
		public static async Task ABBEY394_MQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_01_1")]
		public static async Task ABBEY394_MQ_01_1(Dialog dialog)
		{
			await dialog.Msg("ABBEY394_MQ_01_1_basic01");
			await dialog.Msg("ABBEY394_MQ_01_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_01_2")]
		public static async Task ABBEY394_MQ_01_2(Dialog dialog)
		{
			await dialog.Msg("ABBEY394_MQ_01_2_basic01");
			await dialog.Msg("ABBEY394_MQ_01_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_06")]
		public static async Task ABBEY394_MQ_06(Dialog dialog)
		{
			await dialog.Msg("ABBEY394_MQ_06_basic01");
			await dialog.Msg("ABBEY394_MQ_06_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_09_1")]
		public static async Task ABBEY394_MQ_09_1(Dialog dialog)
		{
			await dialog.Msg("ABBEY39_4_MQ_09");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_08")]
		public static async Task ABBEY394_MQ_08(Dialog dialog)
		{
			await dialog.Msg("ABBEY39_4_MQ_08");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_10_1")]
		public static async Task ABBEY394_MQ_10_1(Dialog dialog)
		{
			await dialog.Msg("ABBEY39_4_MQ_10_basic01");
			await dialog.Msg("ABBEY39_4_MQ_10_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_06_2")]
		public static async Task ABBEY394_MQ_06_2(Dialog dialog)
		{
			await dialog.Msg("ABBEY394_MQ_06_2_basic01");
			await dialog.Msg("ABBEY394_MQ_06_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07")]
		public static async Task ABBEY394_MQ_07(Dialog dialog)
		{
			await dialog.Msg("ABBEY39_4_MQ_07_basic01");
			await dialog.Msg("ABBEY39_4_MQ_07_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_01_3")]
		public static async Task ABBEY394_MQ_01_3(Dialog dialog)
		{
			await dialog.Msg("ABBEY394_MQ_01_3_basic01");
			await dialog.Msg("ABBEY394_MQ_01_3_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_02_1")]
		public static async Task ABBEY394_MQ_02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_02")]
		public static async Task ABBEY394_MQ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_03")]
		public static async Task ABBEY394_MQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_04")]
		public static async Task ABBEY394_MQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX1")]
		public static async Task ABBEY394_MQ_07_BOX1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX2")]
		public static async Task ABBEY394_MQ_07_BOX2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX3")]
		public static async Task ABBEY394_MQ_07_BOX3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX4")]
		public static async Task ABBEY394_MQ_07_BOX4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX5")]
		public static async Task ABBEY394_MQ_07_BOX5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX6")]
		public static async Task ABBEY394_MQ_07_BOX6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX7")]
		public static async Task ABBEY394_MQ_07_BOX7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_07_BOX8")]
		public static async Task ABBEY394_MQ_07_BOX8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_SQ_01_1")]
		public static async Task ABBEY394_SQ_01_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_09_3")]
		public static async Task ABBEY394_MQ_09_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_08_1")]
		public static async Task ABBEY394_MQ_08_1(Dialog dialog)
		{
			await dialog.Msg("ABBEY394_MQ_08_1_basic01");
			await dialog.Msg("ABBEY394_MQ_08_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_SQ_01")]
		public static async Task ABBEY394_SQ_01(Dialog dialog)
		{
			await dialog.Msg("ABBEY39_4_SQ_01_basic01");
			await dialog.Msg("ABBEY39_4_SQ_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_MQ_10_3")]
		public static async Task ABBEY394_MQ_10_3(Dialog dialog)
		{
			await dialog.Msg("ABBEY394_MQ_10_3_basic01");
			await dialog.Msg("ABBEY394_MQ_10_3_basic02");
			await dialog.Msg("ABBEY394_MQ_10_3_basic03");
			await dialog.Msg("ABBEY394_MQ_10_3_basic04");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY394_SQ_03")]
		public static async Task ABBEY394_SQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_ABBEY_39_41000")]
		public static async Task TREASUREBOX_LV_D_ABBEY_39_41000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_206", 1);

		[DialogFunction("JOB_ORACLE_6_1_TRIGGER1")]
		public static async Task JOB_ORACLE_6_1_TRIGGER1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_ORACLE_6_1_TRIGGER2")]
		public static async Task JOB_ORACLE_6_1_TRIGGER2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_ORACLE_6_1_TRIGGER3")]
		public static async Task JOB_ORACLE_6_1_TRIGGER3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_PILLA01")]
		public static async Task ROKAS_36_1_PILLA01(Dialog dialog)
		{
			await dialog.Msg("ROKAS_36_1_PILLA01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_PILLA02")]
		public static async Task ROKAS_36_1_PILLA02(Dialog dialog)
		{
			await dialog.Msg("ROKAS_36_1_PILLA02_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_030_CON01")]
		public static async Task ROKAS_36_1_SQ_030_CON01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_030_CON02")]
		public static async Task ROKAS_36_1_SQ_030_CON02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_030_CON03")]
		public static async Task ROKAS_36_1_SQ_030_CON03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_PILLA03")]
		public static async Task ROKAS_36_1_PILLA03(Dialog dialog)
		{
			await dialog.Msg("ROKAS_36_1_PILLA03_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_PILLA05")]
		public static async Task ROKAS_36_1_PILLA05(Dialog dialog)
		{
			await dialog.Msg("ROKAS_36_1_PILLA05_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_PILLA06")]
		public static async Task ROKAS_36_1_PILLA06(Dialog dialog)
		{
			await dialog.Msg("ROKAS_36_1_PILLA06_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_060_CON_A")]
		public static async Task ROKAS_36_1_SQ_060_CON_A(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_060_CON_B")]
		public static async Task ROKAS_36_1_SQ_060_CON_B(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_060_CON_C")]
		public static async Task ROKAS_36_1_SQ_060_CON_C(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_060_CON_D")]
		public static async Task ROKAS_36_1_SQ_060_CON_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_SQ_040_STONE")]
		public static async Task ROKAS_36_1_SQ_040_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_PILLA_END")]
		public static async Task ROKAS_36_1_PILLA_END(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_36_1_PILLA05_RUN")]
		public static async Task ROKAS_36_1_PILLA05_RUN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ROKAS_36_11000")]
		public static async Task TREASUREBOX_LV_F_ROKAS_36_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_249", 1);

		[DialogFunction("REMAINS38_HIDDEN_RUNE")]
		public static async Task REMAINS38_HIDDEN_RUNE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_DOPPELSOELDNER_6_1_OBJ01")]
		public static async Task JOB_DOPPELSOELDNER_6_1_OBJ01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_ALBINAS")]
		public static async Task PILGRIM_36_2_ALBINAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_36_2_ALBINAS_BASIC01");
			await dialog.Msg("PILGRIM_36_2_ALBINAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_DAMIJONAS")]
		public static async Task PILGRIM_36_2_DAMIJONAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_36_2_DAMIJONAS_BASIC01");
			await dialog.Msg("PILGRIM_36_2_DAMIJONAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_EDVINAS")]
		public static async Task PILGRIM_36_2_EDVINAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_36_2_EDVINAS_BASIC01");
			await dialog.Msg("PILGRIM_36_2_EDVINAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_GEDAS")]
		public static async Task PILGRIM_36_2_GEDAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_36_2_GEDAS_BASIC01");
			await dialog.Msg("PILGRIM_36_2_GEDAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_BARTE")]
		public static async Task PILGRIM_36_2_BARTE(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_36_2_BARTE_BASIC01");
			await dialog.Msg("PILGRIM_36_2_BARTE_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_NIJOLE")]
		public static async Task PILGRIM_36_2_NIJOLE(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_36_2_NIJOLE_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_FABIJUS")]
		public static async Task PILGRIM_36_2_FABIJUS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_36_2_FABIJUS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_SHRINE_REAL")]
		public static async Task PILGRIM_36_2_SHRINE_REAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_RUIN")]
		public static async Task PILGRIM_36_2_RUIN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_36_2_MATERIALS")]
		public static async Task PILGRIM_36_2_MATERIALS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_36_21000")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_36_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_250", 1);

		[DialogFunction("PILGRIM362_RP_1_NPC")]
		public static async Task PILGRIM362_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("PILGRIM362_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD362_HIDDEN_TABLE")]
		public static async Task PILGRIMROAD362_HIDDEN_TABLE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD362_HIDDEN_OBJ1")]
		public static async Task PILGRIMROAD362_HIDDEN_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD362_HIDDEN_OBJ2")]
		public static async Task PILGRIMROAD362_HIDDEN_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD362_HIDDEN_OBJ3")]
		public static async Task PILGRIMROAD362_HIDDEN_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD362_HIDDEN_OBJ4")]
		public static async Task PILGRIMROAD362_HIDDEN_OBJ4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD362_HIDDEN_OBJ5")]
		public static async Task PILGRIMROAD362_HIDDEN_OBJ5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIMROAD362_HIDDEN_OBJ6")]
		public static async Task PILGRIMROAD362_HIDDEN_OBJ6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_ADRIJUS")]
		public static async Task REMAINS37_1_ADRIJUS(Dialog dialog)
		{
			await dialog.Msg("REMAINS37_1_ADRIJUS_BASIC01");
			await dialog.Msg("REMAINS37_1_ADRIJUS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_BOARD")]
		public static async Task REMAINS37_1_BOARD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_FAKEMT01")]
		public static async Task REMAINS37_1_FAKEMT01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_FAKEMT02")]
		public static async Task REMAINS37_1_FAKEMT02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_MT01")]
		public static async Task REMAINS37_1_MT01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_MT02")]
		public static async Task REMAINS37_1_MT02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_MT05")]
		public static async Task REMAINS37_1_MT05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_KRAUT")]
		public static async Task REMAINS37_1_KRAUT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_1_MT04")]
		public static async Task REMAINS37_1_MT04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_37_11000")]
		public static async Task TREASUREBOX_LV_F_REMAINS_37_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_200", 1);

		[DialogFunction("REMAINS37_2_MT01")]
		public static async Task REMAINS37_2_MT01(Dialog dialog)
		{
			await dialog.Msg("REMAIN37_SQ07_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_MT02")]
		public static async Task REMAINS37_2_MT02(Dialog dialog)
		{
			await dialog.Msg("REMAIN37_SQ07_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_MT03")]
		public static async Task REMAINS37_2_MT03(Dialog dialog)
		{
			await dialog.Msg("AITVARAS_RECORD_3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_HEAL")]
		public static async Task REMAINS37_2_HEAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_STONES")]
		public static async Task REMAINS37_2_STONES(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_BRANCHES")]
		public static async Task REMAINS37_2_BRANCHES(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_MT02_BROKEN")]
		public static async Task REMAINS37_2_MT02_BROKEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_VINE")]
		public static async Task REMAINS37_2_VINE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_BUSH")]
		public static async Task REMAINS37_2_BUSH(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_LEVER1")]
		public static async Task REMAINS37_2_LEVER1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_LEVER2")]
		public static async Task REMAINS37_2_LEVER2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_LEVER3")]
		public static async Task REMAINS37_2_LEVER3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_TREE1")]
		public static async Task REMAINS37_2_TREE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_TREE2")]
		public static async Task REMAINS37_2_TREE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_2_TREE3")]
		public static async Task REMAINS37_2_TREE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_37_21000")]
		public static async Task TREASUREBOX_LV_F_REMAINS_37_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_201", 1);

		[DialogFunction("REMAINS_37_2_JOB_SHADOW_OBJECT")]
		public static async Task REMAINS_37_2_JOB_SHADOW_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_JUSTAS")]
		public static async Task REMAINS37_3_JUSTAS(Dialog dialog)
		{
			await dialog.Msg("REMAINS37_3_JUSTAS_BASIC01");
			await dialog.Msg("REMAINS37_3_JUSTAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_ALVYDA")]
		public static async Task REMAINS37_3_ALVYDA(Dialog dialog)
		{
			await dialog.Msg("REMAINS37_3_ALVYDA_BASIC01");
			await dialog.Msg("REMAINS37_3_ALVYDA_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_PLATE01")]
		public static async Task REMAINS37_3_PLATE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_PLATEPIECES")]
		public static async Task REMAINS37_3_PLATEPIECES(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_BIGVINES")]
		public static async Task REMAINS37_3_BIGVINES(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_PLATE03")]
		public static async Task REMAINS37_3_PLATE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_WELL")]
		public static async Task REMAINS37_3_WELL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_MAGNET")]
		public static async Task REMAINS37_3_MAGNET(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("REMAINS37_3_JUSTAS_AFTER")]
		public static async Task REMAINS37_3_JUSTAS_AFTER(Dialog dialog)
		{
			await dialog.Msg("REMAINS37_3_JUSTAS_A_BASIC01");
			await dialog.Msg("REMAINS37_3_JUSTAS_A_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_REMAINS_37_3")]
		public static async Task WARP_F_REMAINS_37_3(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("DRAGOON_MASTER")]
		public static async Task DRAGOON_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_DRAGOON_MASTER_basic1");
			await dialog.Msg("JOB_DRAGOON_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KABBALIST_MASTER")]
		public static async Task KABBALIST_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_KABBALIST_MASTER_basic1");
			await dialog.Msg("JOB_KABBALIST_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PLAGUEDOCTOR_MASTER")]
		public static async Task PLAGUEDOCTOR_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_PLAGUEDOCTOR_MASTER_basic1");
			await dialog.Msg("JOB_PLAGUEDOCTOR_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GT_RELICSHOP_NPC")]
		public static async Task GT_RELICSHOP_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("INSTANCE_GT_GROUNDTOWER_1")]
		public static async Task INSTANCE_GT_GROUNDTOWER_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_REMAINS_37_31000")]
		public static async Task TREASUREBOX_LV_F_REMAINS_37_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_202", 1);

		[DialogFunction("HACKAPELL_MASTER")]
		public static async Task HACKAPELL_MASTER(Dialog dialog)
		{
			await dialog.Msg("HACKAPELL_MASTER_BASICK");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MERGEN_MASTER")]
		public static async Task MERGEN_MASTER(Dialog dialog)
		{
			await dialog.Msg("MERGEN_MASTER_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GT_RELICSHOP_NPC2")]
		public static async Task GT_RELICSHOP_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR220_MSETP2_5_OBJ2_1")]
		public static async Task CHAR220_MSETP2_5_OBJ2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR220_MSETP2_5_OBJ2_2")]
		public static async Task CHAR220_MSETP2_5_OBJ2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR220_MSETP2_5_OBJ2_3")]
		public static async Task CHAR220_MSETP2_5_OBJ2_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR220_MSETP2_5_OBJ2_4")]
		public static async Task CHAR220_MSETP2_5_OBJ2_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_TABLELAND_28_2_RAID_02_NPC_01")]
		public static async Task F_TABLELAND_28_2_RAID_02_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_LEOPOLDAS")]
		public static async Task PILGRIM_48_LEOPOLDAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_LEOPOLDAS_BASIC01");
			await dialog.Msg("PILGRIM_48_LEOPOLDAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_JURATE")]
		public static async Task PILGRIM_48_JURATE(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_JURATE_BASIC01");
			await dialog.Msg("PILGRIM_48_SQ_040_ST");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_MARCELIJUS")]
		public static async Task PILGRIM_48_MARCELIJUS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_MARCELIJUS_.BASIC01");
			await dialog.Msg("PILGRIM_48_MARCELIJUS_.BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_GERDA")]
		public static async Task PILGRIM_48_GERDA(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_GERDA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_SERAPINAS")]
		public static async Task PILGRIM_48_SERAPINAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_SERAPINAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_NPC02")]
		public static async Task PILGRIM_48_NPC02(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_NPC02_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_NPC01")]
		public static async Task PILGRIM_48_NPC01(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_NPC01_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_NPC03")]
		public static async Task PILGRIM_48_NPC03(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_48_NPC03_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_BOARD")]
		public static async Task PILGRIM_48_BOARD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_BOARDPLACE")]
		public static async Task PILGRIM_48_BOARDPLACE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_EXCAVATION01")]
		public static async Task PILGRIM_48_EXCAVATION01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_48_EXCAVATION03")]
		public static async Task PILGRIM_48_EXCAVATION03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_PILGRIMROAD_48")]
		public static async Task WARP_F_PILGRIMROAD_48(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_4826")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_4826(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_251", 1);

		[DialogFunction("PILGRIM48_RP_1_OBJ")]
		public static async Task PILGRIM48_RP_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("INQUISITOR_MASTER")]
		public static async Task INQUISITOR_MASTER(Dialog dialog)
		{
			await dialog.Msg("INQUISITOR_MASTER_basic1");
			await dialog.Msg("INQUISITOR_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_49_ANTANAS")]
		public static async Task PILGRIM_49_ANTANAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_49_ANTANAS_BASIC01");
			await dialog.Msg("PILGRIM_49_ANTANAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_49_GIEDRA")]
		public static async Task PILGRIM_49_GIEDRA(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_49_GIEDRA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_49_KESTAS")]
		public static async Task PILGRIM_49_KESTAS(Dialog dialog)
		{
			await dialog.Msg("PILGRIM_49_KESTAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_49_SHRINE01")]
		public static async Task PILGRIM_49_SHRINE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM_49_STONE01")]
		public static async Task PILGRIM_49_STONE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_491000")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_491000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_252", 1);

		[DialogFunction("PILGRIM49_RP_1_NPC")]
		public static async Task PILGRIM49_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("PILGRIM49_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM48_HIDDEN_OBJ1")]
		public static async Task PILGRIM48_HIDDEN_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM48_HIDDEN_OBJ2")]
		public static async Task PILGRIM48_HIDDEN_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM48_HIDDEN_OBJ3")]
		public static async Task PILGRIM48_HIDDEN_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM48_HIDDENQ1_OBJ1")]
		public static async Task PILGRIM48_HIDDENQ1_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM48_HIDDENQ1_OBJ2")]
		public static async Task PILGRIM48_HIDDENQ1_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM48_HIDDENQ1_OBJ3")]
		public static async Task PILGRIM48_HIDDENQ1_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PILGRIM48_HIDDENQ1_OBJ4")]
		public static async Task PILGRIM48_HIDDENQ1_OBJ4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_GYTIS")]
		public static async Task SIAULIAI_50_1_SQ_GYTIS(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_GYTIS_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_GYTIS_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_RESEARCHER")]
		public static async Task SIAULIAI_50_1_SQ_RESEARCHER(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_GRASS_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_GRASS_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_GRASS")]
		public static async Task SIAULIAI_50_1_SQ_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER01")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER01(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER01_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER02")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER02(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER02_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER03")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER03(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER03_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_MAN01")]
		public static async Task SIAULIAI_50_1_SQ_MAN01(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN01_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER03_AFTER")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER03_AFTER(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER03_AFTER_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER03_AFTER_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER04")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER04(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER04_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER04_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER05")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER05(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER05_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER05_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER06")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER06(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER06_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER06_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_SOLDIER07")]
		public static async Task SIAULIAI_50_1_SQ_SOLDIER07(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER07_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_SOLDIER07_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_WOOD_PIECE")]
		public static async Task SIAULIAI_50_WOOD_PIECE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD01")]
		public static async Task SIAULIAI50_FENCE_BUILD01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD03")]
		public static async Task SIAULIAI50_FENCE_BUILD03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD02")]
		public static async Task SIAULIAI50_FENCE_BUILD02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD05")]
		public static async Task SIAULIAI50_FENCE_BUILD05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD07")]
		public static async Task SIAULIAI50_FENCE_BUILD07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD06")]
		public static async Task SIAULIAI50_FENCE_BUILD06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD04")]
		public static async Task SIAULIAI50_FENCE_BUILD04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI50_FENCE_BUILD08")]
		public static async Task SIAULIAI50_FENCE_BUILD08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_MAN02")]
		public static async Task SIAULIAI_50_1_SQ_MAN02(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN02_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_MAN03")]
		public static async Task SIAULIAI_50_1_SQ_MAN03(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN03_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN03_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_50_1_SQ_MAN04")]
		public static async Task SIAULIAI_50_1_SQ_MAN04(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN04_basic01");
			await dialog.Msg("SIAULIAI_50_1_SQ_MAN04_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_50_190")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_50_190(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_144", 1);

		[DialogFunction("SIAU501_RP_1_NPC")]
		public static async Task SIAU501_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("SIAU501_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_50_194")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_50_194(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_256", 1);

		[DialogFunction("SIAULIAI_50_1_TO_PRISON_75_1")]
		public static async Task SIAULIAI_50_1_TO_PRISON_75_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT3_SIAULIAI_50_1_SEED")]
		public static async Task HT3_SIAULIAI_50_1_SEED(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FREE_DUNGEON_SIGN1")]
		public static async Task FREE_DUNGEON_SIGN1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("KATYN_13_2_HQ_01")]
		public static async Task KATYN_13_2_HQ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_58_SVTRIGAILA")]
		public static async Task FLASH_58_SVTRIGAILA(Dialog dialog)
		{
			await dialog.Msg("FLASH_58_SVTRIGAILA_basic01");
			await dialog.Msg("FLASH_58_SVTRIGAILA_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_SOUL_COLLECTOR_S1_D")]
		public static async Task FLASH_SOUL_COLLECTOR_S1_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_SOUL_COLLECTOR_S2_D")]
		public static async Task FLASH_SOUL_COLLECTOR_S2_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_SOUL_COLLECTOR_S3_D")]
		public static async Task FLASH_SOUL_COLLECTOR_S3_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_SOUL_COLLECTOR")]
		public static async Task FLASH_SOUL_COLLECTOR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_58_PETRIFACTION")]
		public static async Task FLASH_58_PETRIFACTION(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_58_PETRIFICATION_MON")]
		public static async Task FLASH_58_PETRIFICATION_MON(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_58_SQ_060_D")]
		public static async Task FLASH_58_SQ_060_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_58_GRASS")]
		public static async Task FLASH_58_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_SOUL_COLLECTOR_S3_1_D")]
		public static async Task FLASH_SOUL_COLLECTOR_S3_1_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_SOUL_COLLECTOR_S3_2_D")]
		public static async Task FLASH_SOUL_COLLECTOR_S3_2_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_FLASH_58")]
		public static async Task WARP_F_FLASH_58(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_FLASH_581000")]
		public static async Task TREASUREBOX_LV_F_FLASH_581000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_262", 1);

		[DialogFunction("FLASH58_HIDDEN_OBJ4")]
		public static async Task FLASH58_HIDDEN_OBJ4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH29_1_HIDDENQ1_OBJ1")]
		public static async Task FLASH29_1_HIDDENQ1_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH29_1_HIDDENQ1_OBJ2")]
		public static async Task FLASH29_1_HIDDENQ1_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH29_1_HIDDENQ1_OBJ3")]
		public static async Task FLASH29_1_HIDDENQ1_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH29_1_HIDDENQ1_OBJ4")]
		public static async Task FLASH29_1_HIDDENQ1_OBJ4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN631_RP_1_NPC")]
		public static async Task BRACKEN631_RP_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELLCOFFER_ENTER_01")]
		public static async Task VELLCOFFER_ENTER_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WEAPON_AFFILIATION_NPC")]
		public static async Task WEAPON_AFFILIATION_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NPC_LITTLEGIRL_01")]
		public static async Task NPC_LITTLEGIRL_01(Dialog dialog)
		{
			await dialog.Msg("NPC_LITTLEGIRL_01_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NPC_RANGDAGIRL_02")]
		public static async Task NPC_RANGDAGIRL_02(Dialog dialog)
		{
			await dialog.Msg("NPC_RANGDAGIRL_02_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_C_KLAIPE_CATHEDRAL_MEDIUM")]
		public static async Task WARP_C_KLAIPE_CATHEDRAL_MEDIUM(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("EP13_GODDESS_LADA4TO5")]
		public static async Task EP13_GODDESS_LADA4TO5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GODDESS_LADA_1")]
		public static async Task GODDESS_LADA_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HUEVILLAGE_58_3_HQ01_NPC03")]
		public static async Task HUEVILLAGE_58_3_HQ01_NPC03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GT_LUTHA_NPC")]
		public static async Task GT_LUTHA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GT2_LUTHA_NPC")]
		public static async Task GT2_LUTHA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_KLAIPE_WARP1")]
		public static async Task EVENT_VIVID_KLAIPE_WARP1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_KLAIPE_BOX")]
		public static async Task EVENT_VIVID_KLAIPE_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_KLAIPE_BOX2")]
		public static async Task EVENT_VIVID_KLAIPE_BOX2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_KLAIPE_BOX3")]
		public static async Task EVENT_VIVID_KLAIPE_BOX3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_KLAIPE_WARP2")]
		public static async Task EVENT_VIVID_KLAIPE_WARP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_KLAIPE_BOX4")]
		public static async Task EVENT_VIVID_KLAIPE_BOX4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_KLAIPE_WARP3")]
		public static async Task EVENT_VIVID_KLAIPE_WARP3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_WARP1")]
		public static async Task EVENT_VIVID_ORSHA_WARP1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_BOX")]
		public static async Task EVENT_VIVID_ORSHA_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_WARP2")]
		public static async Task EVENT_VIVID_ORSHA_WARP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_WARP3")]
		public static async Task EVENT_VIVID_ORSHA_WARP3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_BOX2")]
		public static async Task EVENT_VIVID_ORSHA_BOX2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_POTAL")]
		public static async Task EVENT_VIVID_ORSHA_POTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_BOX4")]
		public static async Task EVENT_VIVID_ORSHA_BOX4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_BOX3")]
		public static async Task EVENT_VIVID_ORSHA_BOX3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_ORSHA_WARP4")]
		public static async Task EVENT_VIVID_ORSHA_WARP4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_BOX")]
		public static async Task EVENT_VIVID_FEDIMIAN_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_BOX2")]
		public static async Task EVENT_VIVID_FEDIMIAN_BOX2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_WARP1")]
		public static async Task EVENT_VIVID_FEDIMIAN_WARP1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_WARP2")]
		public static async Task EVENT_VIVID_FEDIMIAN_WARP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_POTAL")]
		public static async Task EVENT_VIVID_FEDIMIAN_POTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_WARP3")]
		public static async Task EVENT_VIVID_FEDIMIAN_WARP3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_BOX3")]
		public static async Task EVENT_VIVID_FEDIMIAN_BOX3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_BOX4")]
		public static async Task EVENT_VIVID_FEDIMIAN_BOX4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_VIVID_FEDIMIAN_WARP4")]
		public static async Task EVENT_VIVID_FEDIMIAN_WARP4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_CURSED_COAST")]
		public static async Task EVENT_CURSED_COAST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_CURSED_COAST2")]
		public static async Task EVENT_CURSED_COAST2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CREATE_CURSED_WHALE")]
		public static async Task CREATE_CURSED_WHALE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_CURSED_COAST_WARP2")]
		public static async Task EVENT_CURSED_COAST_WARP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CASTLE_19_1_MQ_04_NPC")]
		public static async Task D_CASTLE_19_1_MQ_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CASTLE_19_1_MQ_07_NPC")]
		public static async Task D_CASTLE_19_1_MQ_07_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_CASTLE_19_1")]
		public static async Task WARP_D_CASTLE_19_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("D_CASTLE_19_1_MQ_03_OBJ_1")]
		public static async Task D_CASTLE_19_1_MQ_03_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CASTLE_19_1_MQ_03_OBJ_2")]
		public static async Task D_CASTLE_19_1_MQ_03_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CASTLE_19_1_MQ_03_OBJ_3")]
		public static async Task D_CASTLE_19_1_MQ_03_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CASTLE_19_1_MQ_03_OBJ_4")]
		public static async Task D_CASTLE_19_1_MQ_03_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_CASTLE_19_1_MQ_03_OBJ_5")]
		public static async Task D_CASTLE_19_1_MQ_03_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_NPC_1")]
		public static async Task CASTLE_20_1_NPC_1(Dialog dialog)
		{
			await dialog.Msg("CASTLE_20_1_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_1")]
		public static async Task CASTLE_20_1_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_2_1")]
		public static async Task CASTLE_20_1_OBJ_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_2_2")]
		public static async Task CASTLE_20_1_OBJ_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_2_3")]
		public static async Task CASTLE_20_1_OBJ_2_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_3")]
		public static async Task CASTLE_20_1_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_4")]
		public static async Task CASTLE_20_1_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_5_DUMMY")]
		public static async Task CASTLE_20_1_OBJ_5_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_5")]
		public static async Task CASTLE_20_1_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_6")]
		public static async Task CASTLE_20_1_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_6_1")]
		public static async Task CASTLE_20_1_OBJ_6_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_6_2")]
		public static async Task CASTLE_20_1_OBJ_6_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_6_3")]
		public static async Task CASTLE_20_1_OBJ_6_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_6_4")]
		public static async Task CASTLE_20_1_OBJ_6_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_7")]
		public static async Task CASTLE_20_1_OBJ_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_8")]
		public static async Task CASTLE_20_1_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_1_OBJ_9")]
		public static async Task CASTLE_20_1_OBJ_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PAYAUTA_EP11_NPC_201")]
		public static async Task PAYAUTA_EP11_NPC_201(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PAJAUTA_EP11_SIGN_1")]
		public static async Task PAJAUTA_EP11_SIGN_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_NPC_1")]
		public static async Task CASTLE_20_2_NPC_1(Dialog dialog)
		{
			await dialog.Msg("CASTLE_20_2_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_1")]
		public static async Task CASTLE_20_2_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_2")]
		public static async Task CASTLE_20_2_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_3_1")]
		public static async Task CASTLE_20_2_OBJ_3_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_3_2")]
		public static async Task CASTLE_20_2_OBJ_3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_1")]
		public static async Task CASTLE_20_2_OBJ_4_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_2")]
		public static async Task CASTLE_20_2_OBJ_4_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_3")]
		public static async Task CASTLE_20_2_OBJ_4_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_4")]
		public static async Task CASTLE_20_2_OBJ_4_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_5")]
		public static async Task CASTLE_20_2_OBJ_4_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_6")]
		public static async Task CASTLE_20_2_OBJ_4_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_7")]
		public static async Task CASTLE_20_2_OBJ_4_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_4_8")]
		public static async Task CASTLE_20_2_OBJ_4_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_5_1")]
		public static async Task CASTLE_20_2_OBJ_5_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_5_2")]
		public static async Task CASTLE_20_2_OBJ_5_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_5_3")]
		public static async Task CASTLE_20_2_OBJ_5_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_5_4")]
		public static async Task CASTLE_20_2_OBJ_5_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_6")]
		public static async Task CASTLE_20_2_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_7_1")]
		public static async Task CASTLE_20_2_OBJ_7_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_7_2")]
		public static async Task CASTLE_20_2_OBJ_7_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_7_3")]
		public static async Task CASTLE_20_2_OBJ_7_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_7_4")]
		public static async Task CASTLE_20_2_OBJ_7_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_7_DUMMY")]
		public static async Task CASTLE_20_2_OBJ_7_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_OBJ_8")]
		public static async Task CASTLE_20_2_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_2_NPC_1_AFTER")]
		public static async Task CASTLE_20_2_NPC_1_AFTER(Dialog dialog)
		{
			await dialog.Msg("CASTLE_20_2_NPC_1_AFTER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_CASTLE202_1")]
		public static async Task SAGE_JOBQ_IN_CASTLE202_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_CASTLE202_2")]
		public static async Task SAGE_JOBQ_IN_CASTLE202_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_CASTLE202_3")]
		public static async Task SAGE_JOBQ_IN_CASTLE202_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_CASTLE202_4")]
		public static async Task SAGE_JOBQ_IN_CASTLE202_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_SQ_NPC_1")]
		public static async Task CASTLE_20_3_SQ_NPC_1(Dialog dialog)
		{
			await dialog.Msg("CASTLE_20_3_SQ_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_1")]
		public static async Task CASTLE_20_3_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_2")]
		public static async Task CASTLE_20_3_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_3")]
		public static async Task CASTLE_20_3_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_4")]
		public static async Task CASTLE_20_3_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_5")]
		public static async Task CASTLE_20_3_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_6")]
		public static async Task CASTLE_20_3_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_7")]
		public static async Task CASTLE_20_3_OBJ_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_8")]
		public static async Task CASTLE_20_3_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_11")]
		public static async Task CASTLE_20_3_OBJ_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_9")]
		public static async Task CASTLE_20_3_OBJ_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_10")]
		public static async Task CASTLE_20_3_OBJ_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_8_AFTER")]
		public static async Task CASTLE_20_3_OBJ_8_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_12")]
		public static async Task CASTLE_20_3_OBJ_12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_13")]
		public static async Task CASTLE_20_3_OBJ_13(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_14")]
		public static async Task CASTLE_20_3_OBJ_14(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_15")]
		public static async Task CASTLE_20_3_OBJ_15(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_16")]
		public static async Task CASTLE_20_3_OBJ_16(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_17")]
		public static async Task CASTLE_20_3_OBJ_17(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_18")]
		public static async Task CASTLE_20_3_OBJ_18(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_20_1")]
		public static async Task CASTLE_20_3_OBJ_20_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_20_2")]
		public static async Task CASTLE_20_3_OBJ_20_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_3_OBJ_19")]
		public static async Task CASTLE_20_3_OBJ_19(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_CASTLE_20_3")]
		public static async Task WARP_CASTLE_20_3(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("CASTLE203_KQ_1_NPC")]
		public static async Task CASTLE203_KQ_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_NPC_1")]
		public static async Task CASTLE_20_4_NPC_1(Dialog dialog)
		{
			await dialog.Msg("CASTLE_20_4_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_1")]
		public static async Task CASTLE_20_4_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_2_1")]
		public static async Task CASTLE_20_4_OBJ_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_2_2")]
		public static async Task CASTLE_20_4_OBJ_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_2_3")]
		public static async Task CASTLE_20_4_OBJ_2_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_3")]
		public static async Task CASTLE_20_4_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_4")]
		public static async Task CASTLE_20_4_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_6_1")]
		public static async Task CASTLE_20_4_OBJ_6_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_6_2")]
		public static async Task CASTLE_20_4_OBJ_6_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_OBJ_6_3")]
		public static async Task CASTLE_20_4_OBJ_6_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE_20_4_NPC_1_AFTER")]
		public static async Task CASTLE_20_4_NPC_1_AFTER(Dialog dialog)
		{
			await dialog.Msg("CASTLE_20_4_NPC_1_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_CASTLE_20_41000")]
		public static async Task TREASUREBOX_LV_F_CASTLE_20_41000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_291", 1);

		[DialogFunction("DCAPITAL_20_5_REDA")]
		public static async Task DCAPITAL_20_5_REDA(Dialog dialog)
		{
			await dialog.Msg("DCAPITAL_20_5_REDA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_5_SQ_60_CLUE")]
		public static async Task DCAPITAL_20_5_SQ_60_CLUE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_5_SQ_30")]
		public static async Task DCAPITAL_20_5_SQ_30(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_5_SQ_90_CLUE")]
		public static async Task DCAPITAL_20_5_SQ_90_CLUE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_110_3")]
		public static async Task DCAPITAL_20_6_SQ_110_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_110_4")]
		public static async Task DCAPITAL_20_6_SQ_110_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_110_5")]
		public static async Task DCAPITAL_20_6_SQ_110_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_110_RUN_5")]
		public static async Task DCAPITAL_20_6_SQ_110_RUN_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_5_SQ_100")]
		public static async Task DCAPITAL_20_5_SQ_100(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_DCAPITAL_20_51000")]
		public static async Task TREASUREBOX_LV_F_DCAPITAL_20_51000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_292", 1);

		[DialogFunction("DCAPITAL_20_6_REDA")]
		public static async Task DCAPITAL_20_6_REDA(Dialog dialog)
		{
			await dialog.Msg("DCAPITAL_20_6_REDA_STD");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_80")]
		public static async Task DCAPITAL_20_6_SQ_80(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_110_1")]
		public static async Task DCAPITAL_20_6_SQ_110_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_110_2")]
		public static async Task DCAPITAL_20_6_SQ_110_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_40")]
		public static async Task DCAPITAL_20_6_SQ_40(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_25")]
		public static async Task DCAPITAL_20_6_SQ_25(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL_20_6_SQ_55")]
		public static async Task DCAPITAL_20_6_SQ_55(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_DCAPITAL_20_61000")]
		public static async Task TREASUREBOX_LV_F_DCAPITAL_20_61000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_293", 1);

		[DialogFunction("HT2_DCAPITAL_20_6_GODDESS")]
		public static async Task HT2_DCAPITAL_20_6_GODDESS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HT2_DCAPITAL_20_6_SLIDE")]
		public static async Task HT2_DCAPITAL_20_6_SLIDE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_NPC_1")]
		public static async Task WTREES_21_1_NPC_1(Dialog dialog)
		{
			await dialog.Msg("WTREES_21_1_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_1_1")]
		public static async Task WTREES_21_1_OBJ_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_1_2")]
		public static async Task WTREES_21_1_OBJ_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_1_3")]
		public static async Task WTREES_21_1_OBJ_1_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_SQ_2_CORE")]
		public static async Task WTREES_21_1_SQ_2_CORE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_2")]
		public static async Task WTREES_21_1_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_4")]
		public static async Task WTREES_21_1_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_5")]
		public static async Task WTREES_21_1_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_6_1")]
		public static async Task WTREES_21_1_OBJ_6_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_6_2")]
		public static async Task WTREES_21_1_OBJ_6_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_6_3")]
		public static async Task WTREES_21_1_OBJ_6_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_7")]
		public static async Task WTREES_21_1_OBJ_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_8")]
		public static async Task WTREES_21_1_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_8_DUMMY")]
		public static async Task WTREES_21_1_OBJ_8_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_NPC_1_AFTER")]
		public static async Task WTREES_21_1_NPC_1_AFTER(Dialog dialog)
		{
			await dialog.Msg("WTREES_21_1_NPC_1_AFTER_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_1_OBJ_9")]
		public static async Task WTREES_21_1_OBJ_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_WHITETREES_21_11000")]
		public static async Task TREASUREBOX_LV_F_WHITETREES_21_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_306", 1);

		[DialogFunction("WARP_WHITETREES_21_2")]
		public static async Task WARP_WHITETREES_21_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("WTREES_21_2_NPC_1")]
		public static async Task WTREES_21_2_NPC_1(Dialog dialog)
		{
			await dialog.Msg("WTREES_21_2_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_NPC_2")]
		public static async Task WTREES_21_2_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_1")]
		public static async Task WTREES_21_2_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_2_1_DUMMY")]
		public static async Task WTREES_21_2_OBJ_2_1_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_2_2_DUMMY")]
		public static async Task WTREES_21_2_OBJ_2_2_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_2_3_DUMMY")]
		public static async Task WTREES_21_2_OBJ_2_3_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_2_1")]
		public static async Task WTREES_21_2_OBJ_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_2_2")]
		public static async Task WTREES_21_2_OBJ_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_2_3")]
		public static async Task WTREES_21_2_OBJ_2_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_3")]
		public static async Task WTREES_21_2_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_3_DUMMY")]
		public static async Task WTREES_21_2_OBJ_3_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_4")]
		public static async Task WTREES_21_2_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_5_1")]
		public static async Task WTREES_21_2_OBJ_5_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_5_1_DUMMY")]
		public static async Task WTREES_21_2_OBJ_5_1_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_5_2")]
		public static async Task WTREES_21_2_OBJ_5_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_OBJ_5_2_DUMMY")]
		public static async Task WTREES_21_2_OBJ_5_2_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_NPC_3")]
		public static async Task WTREES_21_2_NPC_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_CAUTION01")]
		public static async Task WTREES_21_2_CAUTION01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES_21_2_CAUTION02")]
		public static async Task WTREES_21_2_CAUTION02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_WHITETREES_21_21000")]
		public static async Task TREASUREBOX_LV_F_WHITETREES_21_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_305", 1);

		[DialogFunction("WTREE212_KQ_1_BOX")]
		public static async Task WTREE212_KQ_1_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES212_TO_FLIBRARY481")]
		public static async Task WTREES212_TO_FLIBRARY481(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREE212_NERINGA_NPC")]
		public static async Task WTREE212_NERINGA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_NPC1")]
		public static async Task WTREES221_SUBQ_NPC1(Dialog dialog)
		{
			await dialog.Msg("WTREES221_SUBQ_NPC1_basic1");
			await dialog.Msg("WTREES221_SUBQ_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_NPC2")]
		public static async Task WTREES221_SUBQ_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_NPC3")]
		public static async Task WTREES221_SUBQ_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_NPC4")]
		public static async Task WTREES221_SUBQ_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_NPC5")]
		public static async Task WTREES221_SUBQ_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_NPC6")]
		public static async Task WTREES221_SUBQ_NPC6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_PILLAR1")]
		public static async Task WTREES221_SUBQ_PILLAR1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_PILLAR2")]
		public static async Task WTREES221_SUBQ_PILLAR2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ_PILLAR3")]
		public static async Task WTREES221_SUBQ_PILLAR3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ3_OBJ1")]
		public static async Task WTREES221_SUBQ3_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ5_OBJ1")]
		public static async Task WTREES221_SUBQ5_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ8_OBJ1")]
		public static async Task WTREES221_SUBQ8_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES221_SUBQ8_OBJ2")]
		public static async Task WTREES221_SUBQ8_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ1_NPC1")]
		public static async Task WTREES22_2_SUBQ1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ4_NPC1")]
		public static async Task WTREES22_2_SUBQ4_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ1_PRE_NPC1")]
		public static async Task WTREES22_2_SUBQ1_PRE_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ1_PRE_NPC2")]
		public static async Task WTREES22_2_SUBQ1_PRE_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC1")]
		public static async Task WTREES22_2_SUBQ5_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC2")]
		public static async Task WTREES22_2_SUBQ5_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC3")]
		public static async Task WTREES22_2_SUBQ5_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC4")]
		public static async Task WTREES22_2_SUBQ5_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC5")]
		public static async Task WTREES22_2_SUBQ5_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC6")]
		public static async Task WTREES22_2_SUBQ5_NPC6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC7")]
		public static async Task WTREES22_2_SUBQ5_NPC7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_2_SUBQ5_NPC8")]
		public static async Task WTREES22_2_SUBQ5_NPC8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES22_2_EV_55_001")]
		public static async Task WHITETREES22_2_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("WTREES22_3_SUBQ1_NPC1")]
		public static async Task WTREES22_3_SUBQ1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ3_NPC1")]
		public static async Task WTREES22_3_SUBQ3_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ3_NPC2")]
		public static async Task WTREES22_3_SUBQ3_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ3_NPC3")]
		public static async Task WTREES22_3_SUBQ3_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ3_NPC4")]
		public static async Task WTREES22_3_SUBQ3_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ3_NPC5")]
		public static async Task WTREES22_3_SUBQ3_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ1_NPC2")]
		public static async Task WTREES22_3_SUBQ1_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ1_NPC3")]
		public static async Task WTREES22_3_SUBQ1_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ1_NPC4")]
		public static async Task WTREES22_3_SUBQ1_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ1_NPC5")]
		public static async Task WTREES22_3_SUBQ1_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_SUBQ7_DEVICENPC")]
		public static async Task WTREES22_3_SUBQ7_DEVICENPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_WHITETREES_22_3")]
		public static async Task WARP_WHITETREES_22_3(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("WTREES22_3_BOARD1")]
		public static async Task WTREES22_3_BOARD1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WTREES22_3_BOARD2")]
		public static async Task WTREES22_3_BOARD2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_WHITETREES_22_337")]
		public static async Task TREASUREBOX_LV_F_WHITETREES_22_337(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_311", 1);

		[DialogFunction("ABBEY22_4_SUBQ1_NPC1")]
		public static async Task ABBEY22_4_SUBQ1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ1_NPC2")]
		public static async Task ABBEY22_4_SUBQ1_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ2_NPC1")]
		public static async Task ABBEY22_4_SUBQ2_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ3_NPC1")]
		public static async Task ABBEY22_4_SUBQ3_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ4_POT")]
		public static async Task ABBEY22_4_SUBQ4_POT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ5_NPC1")]
		public static async Task ABBEY22_4_SUBQ5_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ6_DEVICE")]
		public static async Task ABBEY22_4_SUBQ6_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ7_NPC1")]
		public static async Task ABBEY22_4_SUBQ7_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ7_UNKNOWN_OBJ")]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ7_NPC2")]
		public static async Task ABBEY22_4_SUBQ7_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ8_DEVICE1")]
		public static async Task ABBEY22_4_SUBQ8_DEVICE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ8_TABLE2")]
		public static async Task ABBEY22_4_SUBQ8_TABLE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ8_TABLE3")]
		public static async Task ABBEY22_4_SUBQ8_TABLE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ8_TABLE4")]
		public static async Task ABBEY22_4_SUBQ8_TABLE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ8_TABLE5")]
		public static async Task ABBEY22_4_SUBQ8_TABLE5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ8_FLURRY")]
		public static async Task ABBEY22_4_SUBQ8_FLURRY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_AGAFLARY_DLG")]
		public static async Task ABBEY22_AGAFLARY_DLG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ11_FLURRY")]
		public static async Task ABBEY22_4_SUBQ11_FLURRY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ10_NPC1")]
		public static async Task ABBEY22_4_SUBQ10_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_4_SUBQ8_TABLE1")]
		public static async Task ABBEY22_4_SUBQ8_TABLE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_FLURRY1")]
		public static async Task ABBEY225_FLURRY1(Dialog dialog)
		{
			await dialog.Msg("ABBEY225_FLURRY1_basic1");
			await dialog.Msg("ABBEY225_FLURRY1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_SUBQ2_NPC1")]
		public static async Task ABBEY225_SUBQ2_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_SUBQ1_NPC1")]
		public static async Task ABBEY225_SUBQ1_NPC1(Dialog dialog)
		{
			await dialog.Msg("ABBEY225_SUBQ1_NPC1_basic1");
			await dialog.Msg("ABBEY225_SUBQ1_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_DECEPTION_HAUBERK")]
		public static async Task ABBEY225_DECEPTION_HAUBERK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_FLURRY2")]
		public static async Task ABBEY225_FLURRY2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_SUBQ5_NPC2")]
		public static async Task ABBEY225_SUBQ5_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_FLURRY3")]
		public static async Task ABBEY225_FLURRY3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_SUBQ7_OBJ1")]
		public static async Task ABBEY225_SUBQ7_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_5_SUBQ9_NPC1")]
		public static async Task ABBEY22_5_SUBQ9_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_5_SUBQ14_PORTAL")]
		public static async Task ABBEY22_5_SUBQ14_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY22_5_SUBQ13_DEVICE")]
		public static async Task ABBEY22_5_SUBQ13_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_FLURRY4")]
		public static async Task ABBEY225_FLURRY4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_SUBQ15_NPC1")]
		public static async Task ABBEY225_SUBQ15_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY225_FLURRY5")]
		public static async Task ABBEY225_FLURRY5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_TO_WTREES212")]
		public static async Task FLIBRARY481_TO_WTREES212(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_TO_FLIBRARY482")]
		public static async Task FLIBRARY481_TO_FLIBRARY482(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_NERINGA_NPC")]
		public static async Task FLIBRARY481_NERINGA_NPC(Dialog dialog)
		{
			await dialog.Msg("FLIBRARY481_NERINGA_NPC_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_VIVORA_NPC")]
		public static async Task FLIBRARY481_VIVORA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_RUGILE_NPC")]
		public static async Task FLIBRARY481_RUGILE_NPC(Dialog dialog)
		{
			await dialog.Msg("FLIBRARY481_RUGILE_NPC_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB481_MQ_2_NPC")]
		public static async Task FANTASYLIB481_MQ_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB481_MQ_3_NPC")]
		public static async Task FANTASYLIB481_MQ_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_BOOKLIST_1_NPC")]
		public static async Task FLIBRARY481_BOOKLIST_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_BOOKLIST_2_NPC")]
		public static async Task FLIBRARY481_BOOKLIST_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY481_BOOKLIST_3_NPC")]
		public static async Task FLIBRARY481_BOOKLIST_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_FANTASYLIB_48_1_COLLECTION_OBJ")]
		public static async Task D_FANTASYLIB_48_1_COLLECTION_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_TO_FLIBRARY481")]
		public static async Task FLIBRARY482_TO_FLIBRARY481(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_TO_FLIBRARY483")]
		public static async Task FLIBRARY482_TO_FLIBRARY483(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_NERINGA_NPC_1")]
		public static async Task FLIBRARY482_NERINGA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_SVAJA_NPC")]
		public static async Task FLIBRARY482_SVAJA_NPC(Dialog dialog)
		{
			await dialog.Msg("FLIBRARY482_SVAJA_NPC_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_VIDA_NPC")]
		public static async Task FLIBRARY482_VIDA_NPC(Dialog dialog)
		{
			await dialog.Msg("FLIBRARY482_VIDA_NPC_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB482_MQ_1_NPC")]
		public static async Task FANTASYLIB482_MQ_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_NERINGA_NPC_2")]
		public static async Task FLIBRARY482_NERINGA_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_NERINGA_NPC_3")]
		public static async Task FLIBRARY482_NERINGA_NPC_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB482_MQ_3_NPC")]
		public static async Task FANTASYLIB482_MQ_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB482_MQ_5_NPC")]
		public static async Task FANTASYLIB482_MQ_5_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_BOOKLIST_1_NPC")]
		public static async Task FLIBRARY482_BOOKLIST_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_BOOKLIST_2_NPC")]
		public static async Task FLIBRARY482_BOOKLIST_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY482_BOOKLIST_3_NPC")]
		public static async Task FLIBRARY482_BOOKLIST_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_TO_FLIBRARY482")]
		public static async Task FLIBRARY483_TO_FLIBRARY482(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_TO_FLIBRARY484")]
		public static async Task FLIBRARY483_TO_FLIBRARY484(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_NERINGA_NPC_1")]
		public static async Task FLIBRARY483_NERINGA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_VIVORA_2_NPC")]
		public static async Task FLIBRARY483_VIVORA_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_RYTE_NPC")]
		public static async Task FLIBRARY483_RYTE_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_DANUTE_NPC")]
		public static async Task FLIBRARY483_DANUTE_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB483_MQ_3_NPC")]
		public static async Task FANTASYLIB483_MQ_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_NERINGA_NPC_2")]
		public static async Task FLIBRARY483_NERINGA_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB483_MQ_4_NPC")]
		public static async Task FANTASYLIB483_MQ_4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB483_MQ_6_NPC")]
		public static async Task FANTASYLIB483_MQ_6_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_NERINGA_NPC_3")]
		public static async Task FLIBRARY483_NERINGA_NPC_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_BOOKLIST_1_NPC")]
		public static async Task FLIBRARY483_BOOKLIST_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_BOOKLIST_2_NPC")]
		public static async Task FLIBRARY483_BOOKLIST_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY483_BOOKLIST_3_NPC")]
		public static async Task FLIBRARY483_BOOKLIST_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_FANTASYLIB_48_3_COLLECTION_OBJ")]
		public static async Task D_FANTASYLIB_48_3_COLLECTION_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_TO_FLIBRARY483")]
		public static async Task FLIBRARY484_TO_FLIBRARY483(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_TO_FLIBRARY485")]
		public static async Task FLIBRARY484_TO_FLIBRARY485(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_NERINGA_NPC_1")]
		public static async Task FLIBRARY484_NERINGA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_VIVORA_NPC")]
		public static async Task FLIBRARY484_VIVORA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_SJAIVA_NPC")]
		public static async Task FLIBRARY484_SJAIVA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB484_MQ_7_NPC")]
		public static async Task FANTASYLIB484_MQ_7_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB484_MQ_8_NPC")]
		public static async Task FANTASYLIB484_MQ_8_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB484_MQ_3_NPC")]
		public static async Task FANTASYLIB484_MQ_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FANTASYLIB484_MQ_4_NPC")]
		public static async Task FANTASYLIB484_MQ_4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_BOOKLIST_1_NPC")]
		public static async Task FLIBRARY484_BOOKLIST_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_BOOKLIST_2_NPC")]
		public static async Task FLIBRARY484_BOOKLIST_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY484_BOOKLIST_3_NPC")]
		public static async Task FLIBRARY484_BOOKLIST_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_FANTASYLIB_48_4_COLLECTION_OBJ")]
		public static async Task D_FANTASYLIB_48_4_COLLECTION_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY485_TO_FLIBRARY484")]
		public static async Task FLIBRARY485_TO_FLIBRARY484(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY485_NERINGA_FLLW_NPC")]
		public static async Task FLIBRARY485_NERINGA_FLLW_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY485_NERINGA_NPC_1")]
		public static async Task FLIBRARY485_NERINGA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY485_NERINGA_NPC_2")]
		public static async Task FLIBRARY485_NERINGA_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY485_BOOKLIST_1_NPC")]
		public static async Task FLIBRARY485_BOOKLIST_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY485_BOOKLIST_2_NPC")]
		public static async Task FLIBRARY485_BOOKLIST_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLIBRARY485_BOOKLIST_3_NPC")]
		public static async Task FLIBRARY485_BOOKLIST_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SECRET_ROOM_TO_FL485")]
		public static async Task SECRET_ROOM_TO_FL485(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC1")]
		public static async Task F_3CMLAKE_27_1_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_1_FDLG3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ2_HNPC")]
		public static async Task F_3CMLAKE_27_1_SQ2_HNPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC2_2")]
		public static async Task F_3CMLAKE_27_1_NPC2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC2_3")]
		public static async Task F_3CMLAKE_27_1_NPC2_3(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_1_FREE_DLG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_5_NPC1")]
		public static async Task F_3CMLAKE_27_1_SQ_5_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_5_NPC2")]
		public static async Task F_3CMLAKE_27_1_SQ_5_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_5_NPC3")]
		public static async Task F_3CMLAKE_27_1_SQ_5_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_6_NPC1")]
		public static async Task F_3CMLAKE_27_1_SQ_6_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_6_NPC2")]
		public static async Task F_3CMLAKE_27_1_SQ_6_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_6_NPC3")]
		public static async Task F_3CMLAKE_27_1_SQ_6_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_6_NPC4")]
		public static async Task F_3CMLAKE_27_1_SQ_6_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_6_NPC5")]
		public static async Task F_3CMLAKE_27_1_SQ_6_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_6_NPC6")]
		public static async Task F_3CMLAKE_27_1_SQ_6_NPC6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_SQ_8_PNPC")]
		public static async Task F_3CMLAKE_27_1_SQ_8_PNPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC4")]
		public static async Task F_3CMLAKE_27_1_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC3")]
		public static async Task F_3CMLAKE_27_1_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC5")]
		public static async Task F_3CMLAKE_27_1_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC6")]
		public static async Task F_3CMLAKE_27_1_NPC6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_1_NPC7")]
		public static async Task F_3CMLAKE_27_1_NPC7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_NPC1")]
		public static async Task F_3CMLAKE_27_2_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_2_FDLG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC1")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC2")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC3")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC4")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC5")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC6")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC7")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_1_NPC8")]
		public static async Task F_3CMLAKE_27_2_SQ_1_NPC8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_2_HNPC1")]
		public static async Task F_3CMLAKE_27_2_SQ_2_HNPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_4_NPC1")]
		public static async Task F_3CMLAKE_27_2_SQ_4_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_2_FDLG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_4_NPC2")]
		public static async Task F_3CMLAKE_27_2_SQ_4_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_2_FDLG3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_4_NPC3")]
		public static async Task F_3CMLAKE_27_2_SQ_4_NPC3(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_2_FDLG4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_4_NPC4")]
		public static async Task F_3CMLAKE_27_2_SQ_4_NPC4(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_2_FDLG5");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_NPC2")]
		public static async Task F_3CMLAKE_27_2_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_NPC3")]
		public static async Task F_3CMLAKE_27_2_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_NPC4")]
		public static async Task F_3CMLAKE_27_2_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_7_HNPC1")]
		public static async Task F_3CMLAKE_27_2_SQ_7_HNPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_7_HNPC2")]
		public static async Task F_3CMLAKE_27_2_SQ_7_HNPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_7_HNPC3")]
		public static async Task F_3CMLAKE_27_2_SQ_7_HNPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_7_HNPC4")]
		public static async Task F_3CMLAKE_27_2_SQ_7_HNPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_7_HNPC5")]
		public static async Task F_3CMLAKE_27_2_SQ_7_HNPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_2_SQ_7_HNPC6")]
		public static async Task F_3CMLAKE_27_2_SQ_7_HNPC6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_NPC1")]
		public static async Task F_3CMLAKE_27_3_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_3_FDLG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_NPC2")]
		public static async Task F_3CMLAKE_27_3_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_3_FREE_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_SQ_1_HNPC")]
		public static async Task F_3CMLAKE_27_3_SQ_1_HNPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_SQ_2_NPC1")]
		public static async Task F_3CMLAKE_27_3_SQ_2_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_SQ_2_NPC3")]
		public static async Task F_3CMLAKE_27_3_SQ_2_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_SQ_3_NPC1")]
		public static async Task F_3CMLAKE_27_3_SQ_3_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_NPC3")]
		public static async Task F_3CMLAKE_27_3_NPC3(Dialog dialog)
		{
			await dialog.Msg("F_3CMLAKE_27_3_FREE_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_NPC4")]
		public static async Task F_3CMLAKE_27_3_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_27_3_SQ_5_HNPC")]
		public static async Task F_3CMLAKE_27_3_SQ_5_HNPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_MAPLE_24_1")]
		public static async Task WARP_F_MAPLE_24_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("F_MAPLE_241_MQ_01_NPC")]
		public static async Task F_MAPLE_241_MQ_01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_02_NPC")]
		public static async Task F_MAPLE_241_MQ_02_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_MEDEINA_NPC")]
		public static async Task F_MAPLE_241_MQ_MEDEINA_NPC(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_241_MQ_MEDEINA_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_NPC1")]
		public static async Task F_MAPLE_241_MQ_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_241_MQ_NPC1_basic01");
			await dialog.Msg("F_MAPLE_241_MQ_NPC1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_03_OBJ")]
		public static async Task F_MAPLE_241_MQ_03_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_04_1_OBJ")]
		public static async Task F_MAPLE_241_MQ_04_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_04_2_OBJ_TRUE")]
		public static async Task F_MAPLE_241_MQ_04_2_OBJ_TRUE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_04_2_OBJ_FAKE3")]
		public static async Task F_MAPLE_241_MQ_04_2_OBJ_FAKE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_04_2_OBJ_FAKE2")]
		public static async Task F_MAPLE_241_MQ_04_2_OBJ_FAKE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_04_2_OBJ_FAKE1")]
		public static async Task F_MAPLE_241_MQ_04_2_OBJ_FAKE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_06_NPC1")]
		public static async Task F_MAPLE_241_MQ_06_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_241_MQ_06_NPC2")]
		public static async Task F_MAPLE_241_MQ_06_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_01_NPC")]
		public static async Task F_MAPLE_242_MQ_01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_02_NPC2")]
		public static async Task F_MAPLE_242_MQ_02_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_02_NPC1")]
		public static async Task F_MAPLE_242_MQ_02_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_02_NPC1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_03_NPC2")]
		public static async Task F_MAPLE_242_MQ_03_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_03_NPC1")]
		public static async Task F_MAPLE_242_MQ_03_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_03_NPC1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_MEDEINA_NPC1")]
		public static async Task F_MAPLE_242_MQ_MEDEINA_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_MEDEINA_NPC1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_06_NPC2")]
		public static async Task F_MAPLE_242_MQ_06_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_06_NPC2_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_06_NPC1")]
		public static async Task F_MAPLE_242_MQ_06_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_06_NPC1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_MEDEINA_NPC2")]
		public static async Task F_MAPLE_242_MQ_MEDEINA_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_NERINGA_NPC")]
		public static async Task F_MAPLE_242_MQ_NERINGA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_11_NPC3")]
		public static async Task F_MAPLE_242_MQ_11_NPC3(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_11_NPC3_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_11_NPC2")]
		public static async Task F_MAPLE_242_MQ_11_NPC2(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_11_NPC2_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_11_NPC1")]
		public static async Task F_MAPLE_242_MQ_11_NPC1(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_242_MQ_11_NPC1_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_242_MQ_MEDEINA_NPC1_AFTER")]
		public static async Task F_MAPLE_242_MQ_MEDEINA_NPC1_AFTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_01_NPC")]
		public static async Task F_MAPLE_243_MQ_01_NPC(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_243_MQ_01_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_03_OBJ1")]
		public static async Task F_MAPLE_243_MQ_03_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_03_OBJ2")]
		public static async Task F_MAPLE_243_MQ_03_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_03_OBJ3")]
		public static async Task F_MAPLE_243_MQ_03_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_05_NPC")]
		public static async Task F_MAPLE_243_MQ_05_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_08_NPC")]
		public static async Task F_MAPLE_243_MQ_08_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_05_FLOWER1")]
		public static async Task F_MAPLE_243_MQ_05_FLOWER1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_05_FLOWER2")]
		public static async Task F_MAPLE_243_MQ_05_FLOWER2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_05_FLOWER3")]
		public static async Task F_MAPLE_243_MQ_05_FLOWER3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_10_NPC")]
		public static async Task F_MAPLE_243_MQ_10_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_MAPLE_243_MQ_11_NPC")]
		public static async Task F_MAPLE_243_MQ_11_NPC(Dialog dialog)
		{
			await dialog.Msg("F_MAPLE_243_MQ_11_NPC_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_30")]
		public static async Task MAPLE_25_1_SQ_30(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_20")]
		public static async Task MAPLE_25_1_SQ_20(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_CEZARIS")]
		public static async Task MAPLE_25_1_CEZARIS(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_1_CEZARIS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_DOVYDAS")]
		public static async Task MAPLE_25_1_DOVYDAS(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_1_DOVYDAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_BRONIUS")]
		public static async Task MAPLE_25_1_BRONIUS(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_1_BRONIUS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_50_1")]
		public static async Task MAPLE_25_1_SQ_50_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_50_2")]
		public static async Task MAPLE_25_1_SQ_50_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_50_3")]
		public static async Task MAPLE_25_1_SQ_50_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_50_4")]
		public static async Task MAPLE_25_1_SQ_50_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_60_1")]
		public static async Task MAPLE_25_1_SQ_60_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_60_3")]
		public static async Task MAPLE_25_1_SQ_60_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_SQ_60_2")]
		public static async Task MAPLE_25_1_SQ_60_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_AIDAS")]
		public static async Task MAPLE_25_1_AIDAS(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_1_AIDAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_1_ROCK")]
		public static async Task MAPLE_25_1_ROCK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_LANCER_8_1")]
		public static async Task JOB_LANCER_8_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_MAPLE_25_11000")]
		public static async Task TREASUREBOX_LV_F_MAPLE_25_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_282", 1);

		[DialogFunction("LOWLV_BOASTER_SQ_30")]
		public static async Task LOWLV_BOASTER_SQ_30(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR312_PRE_MSTEP1_SOLDIER1")]
		public static async Task CHAR312_PRE_MSTEP1_SOLDIER1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_MORKUS")]
		public static async Task MAPLE_25_2_MORKUS(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_2_MORKUS_ST1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_1")]
		public static async Task MAPLE_25_2_SQ_20_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_2")]
		public static async Task MAPLE_25_2_SQ_20_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_3")]
		public static async Task MAPLE_25_2_SQ_20_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_4")]
		public static async Task MAPLE_25_2_SQ_20_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_5")]
		public static async Task MAPLE_25_2_SQ_20_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_30_1")]
		public static async Task MAPLE_25_2_SQ_30_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_30_2")]
		public static async Task MAPLE_25_2_SQ_30_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_30_3")]
		public static async Task MAPLE_25_2_SQ_30_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_30_4")]
		public static async Task MAPLE_25_2_SQ_30_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_30_5")]
		public static async Task MAPLE_25_2_SQ_30_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_EIVYDAS")]
		public static async Task MAPLE_25_2_EIVYDAS(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_2_EIVYDAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_VILITA")]
		public static async Task MAPLE_25_2_VILITA(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_2_VILITA_ST1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_MAPLE_25_2")]
		public static async Task WARP_F_MAPLE_25_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("MAPLE_25_2_SQ_20_CLEAR_1")]
		public static async Task MAPLE_25_2_SQ_20_CLEAR_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_CLEAR_2")]
		public static async Task MAPLE_25_2_SQ_20_CLEAR_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_CLEAR_3")]
		public static async Task MAPLE_25_2_SQ_20_CLEAR_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_CLEAR_4")]
		public static async Task MAPLE_25_2_SQ_20_CLEAR_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_2_SQ_20_CLEAR_5")]
		public static async Task MAPLE_25_2_SQ_20_CLEAR_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_MAPLE_25_21000")]
		public static async Task TREASUREBOX_LV_F_MAPLE_25_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_283", 1);

		[DialogFunction("MAPLE252_KQ_1_NPC")]
		public static async Task MAPLE252_KQ_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_HILDA")]
		public static async Task MAPLE_25_3_HILDA(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_3_HILDA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_LINA")]
		public static async Task MAPLE_25_3_LINA(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_3_LINA_ST1");
			await dialog.Msg("MAPLE_25_3_EGLE_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_EGLE")]
		public static async Task MAPLE_25_3_EGLE(Dialog dialog)
		{
			await dialog.Msg("MAPLE_25_3_EGLE_ST");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_30")]
		public static async Task MAPLE_25_3_SQ_30(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_90")]
		public static async Task MAPLE_25_3_SQ_90(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_40_1")]
		public static async Task MAPLE_25_3_SQ_40_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_40_2")]
		public static async Task MAPLE_25_3_SQ_40_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_40_3")]
		public static async Task MAPLE_25_3_SQ_40_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_40_4")]
		public static async Task MAPLE_25_3_SQ_40_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_40_5")]
		public static async Task MAPLE_25_3_SQ_40_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MAPLE_25_3_SQ_40_6")]
		public static async Task MAPLE_25_3_SQ_40_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_MAPLE_25_31000")]
		public static async Task TREASUREBOX_LV_F_MAPLE_25_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_284", 1);

		[DialogFunction("MAPLE_25_3_JOB_SHADOW_OBJECT")]
		public static async Task MAPLE_25_3_JOB_SHADOW_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_JAUNIUS1")]
		public static async Task CATACOMB_25_4_SQ_JAUNIUS1(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_25_4_SQ_JAUNIUS1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_KOSTAS")]
		public static async Task CATACOMB_25_4_SQ_KOSTAS(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_25_4_SQ_KOSTAS_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_JAUNIUS2")]
		public static async Task CATACOMB_25_4_SQ_JAUNIUS2(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_25_4_SQ_JAUNIUS2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_120_IN")]
		public static async Task CATACOMB_25_4_SQ_120_IN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_30")]
		public static async Task CATACOMB_25_4_SQ_30(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_60_1")]
		public static async Task CATACOMB_25_4_SQ_60_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_60_2")]
		public static async Task CATACOMB_25_4_SQ_60_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_60_3")]
		public static async Task CATACOMB_25_4_SQ_60_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_60_4")]
		public static async Task CATACOMB_25_4_SQ_60_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_70_1")]
		public static async Task CATACOMB_25_4_SQ_70_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_70_2")]
		public static async Task CATACOMB_25_4_SQ_70_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_70_3")]
		public static async Task CATACOMB_25_4_SQ_70_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_70_4")]
		public static async Task CATACOMB_25_4_SQ_70_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_80")]
		public static async Task CATACOMB_25_4_SQ_80(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_25_4_SQ_120_OUT")]
		public static async Task CATACOMB_25_4_SQ_120_OUT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_25_41000")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_25_41000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_285", 1);

		[DialogFunction("ZEALOT_QUEST_COSTUME")]
		public static async Task ZEALOT_QUEST_COSTUME(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_NPC1_1")]
		public static async Task WHITETREES561_SUBQ_NPC1_1(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_SUBQ_NPC1_1_basic1");
			await dialog.Msg("WHITETREES561_SUBQ_NPC1_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_NPC2_1")]
		public static async Task WHITETREES561_SUBQ_NPC2_1(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_SUBQ_NPC2_1_basic1");
			await dialog.Msg("WHITETREES561_SUBQ_NPC2_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_NPC1_2")]
		public static async Task WHITETREES561_SUBQ_NPC1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_NPC2_2")]
		public static async Task WHITETREES561_SUBQ_NPC2_2(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_SUBQ_NPC2_2_basic1");
			await dialog.Msg("WHITETREES561_SUBQ_NPC2_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_NPC1_3")]
		public static async Task WHITETREES561_SUBQ_NPC1_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_NPC3")]
		public static async Task WHITETREES561_SUBQ_NPC3(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_SUBQ_NPC3_basic1");
			await dialog.Msg("WHITETREES561_SUBQ_NPC3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_GRASS")]
		public static async Task WHITETREES561_SUBQ_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_FENCE1")]
		public static async Task WHITETREES561_SUBQ_FENCE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_FENCE2")]
		public static async Task WHITETREES561_SUBQ_FENCE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_FENCE3")]
		public static async Task WHITETREES561_SUBQ_FENCE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_FENCE4")]
		public static async Task WHITETREES561_SUBQ_FENCE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_FENCE5")]
		public static async Task WHITETREES561_SUBQ_FENCE5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_FENCE6")]
		public static async Task WHITETREES561_SUBQ_FENCE6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_SUBQ_WOOD1")]
		public static async Task WHITETREES561_SUBQ_WOOD1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC1")]
		public static async Task WHITETREES561_NORMAL_NPC1(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC1_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC2")]
		public static async Task WHITETREES561_NORMAL_NPC2(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC2_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC3")]
		public static async Task WHITETREES561_NORMAL_NPC3(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC3_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC4")]
		public static async Task WHITETREES561_NORMAL_NPC4(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC4_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC4_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC5")]
		public static async Task WHITETREES561_NORMAL_NPC5(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC5_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC5_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC6")]
		public static async Task WHITETREES561_NORMAL_NPC6(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC6_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC6_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC7")]
		public static async Task WHITETREES561_NORMAL_NPC7(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC7_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC7_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC8")]
		public static async Task WHITETREES561_NORMAL_NPC8(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC8_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC8_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC9")]
		public static async Task WHITETREES561_NORMAL_NPC9(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC9_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC9_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC10")]
		public static async Task WHITETREES561_NORMAL_NPC10(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC10_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC10_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC11")]
		public static async Task WHITETREES561_NORMAL_NPC11(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC11_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC11_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC12")]
		public static async Task WHITETREES561_NORMAL_NPC12(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC12_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC12_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC13")]
		public static async Task WHITETREES561_NORMAL_NPC13(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC13_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC13_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC14")]
		public static async Task WHITETREES561_NORMAL_NPC14(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC14_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC14_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WHITETREES561_NORMAL_NPC15")]
		public static async Task WHITETREES561_NORMAL_NPC15(Dialog dialog)
		{
			await dialog.Msg("WHITETREES561_NORMAL_NPC15_basic1");
			await dialog.Msg("WHITETREES561_NORMAL_NPC15_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER301_HI_LOW")]
		public static async Task UNDER301_HI_LOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER301_MONSTER_PAIR_GIMMICK")]
		public static async Task UNDER301_MONSTER_PAIR_GIMMICK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER30_1_BASE_BALL")]
		public static async Task UNDER30_1_BASE_BALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER30_2_EVENT2_OBJ1")]
		public static async Task UNDER30_2_EVENT2_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER30_3_EVENT1_OBJ1")]
		public static async Task UNDER30_3_EVENT1_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER30_3_EVENT1_OBJ2")]
		public static async Task UNDER30_3_EVENT1_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER30_3_NUMBER_MIXING")]
		public static async Task UNDER30_3_NUMBER_MIXING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDER30_3_EVENT2_BOX")]
		public static async Task UNDER30_3_EVENT2_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_29_1_KUPOLE_MEILE")]
		public static async Task FLASH_29_1_KUPOLE_MEILE(Dialog dialog)
		{
			await dialog.Msg("FLASH_29_1_KUPOLE_MEILE_BASIC01");
			await dialog.Msg("FLASH_29_1_KUPOLE_MEILE_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_29_1_DETECTOR01")]
		public static async Task FLASH_29_1_DETECTOR01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_29_1_DETECTOR02")]
		public static async Task FLASH_29_1_DETECTOR02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_29_1_DETECTOR03")]
		public static async Task FLASH_29_1_DETECTOR03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_29_1_DETECTOR04")]
		public static async Task FLASH_29_1_DETECTOR04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_29_1_BOX")]
		public static async Task FLASH_29_1_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FLASH_29_1_SOL_SCAR")]
		public static async Task FLASH_29_1_SOL_SCAR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FLASH_29_11000")]
		public static async Task TREASUREBOX_LV_F_FLASH_29_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_261", 1);

		[DialogFunction("JOB_CORSAIR_6_1_BONE")]
		public static async Task JOB_CORSAIR_6_1_BONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HATR_BOOK02")]
		public static async Task HATR_BOOK02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_PEOPLE2")]
		public static async Task CORAL_32_1_PEOPLE2(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_PEOPLE2_BASIC1");
			await dialog.Msg("CORAL_32_1_PEOPLE2_BASIC3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_PEOPLE3")]
		public static async Task CORAL_32_1_PEOPLE3(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_PEOPLE3_BASIC1");
			await dialog.Msg("CORAL_32_1_PEOPLE3_BASIC3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_PEOPLE1")]
		public static async Task CORAL_32_1_PEOPLE1(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_PEOPLE1_BASIC2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_PEOPLE4")]
		public static async Task CORAL_32_1_PEOPLE4(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_PEOPLE4_BASIC");
			await dialog.Msg("CORAL_32_1_PEOPLE4_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_MERCHANT1")]
		public static async Task CORAL_32_1_MERCHANT1(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_MERCHANT1_BASIC1");
			await dialog.Msg("CORAL_32_1_MERCHANT1_BASIC3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_MERCHANT2")]
		public static async Task CORAL_32_1_MERCHANT2(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_MERCHANT2_BASIC1");
			await dialog.Msg("CORAL_32_1_MERCHANT2_BASIC2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_GUARD3")]
		public static async Task CORAL_32_1_GUARD3(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_GUARD3_BASIC1");
			await dialog.Msg("CORAL_32_1_GUARD3_BASIC3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_GUARD1")]
		public static async Task CORAL_32_1_GUARD1(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_GUARD1_BASIC1");
			await dialog.Msg("CORAL_32_1_GUARD1_BASIC3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_GUARD2")]
		public static async Task CORAL_32_1_GUARD2(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_GUARD2_BASIC");
			await dialog.Msg("CORAL_32_1_GUARD2_BASIC1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_SERVANT")]
		public static async Task CORAL_32_1_SERVANT(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_SERVANT_BASIC");
			await dialog.Msg("CORAL_32_1_SERVANT_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_KEY")]
		public static async Task CORAL_32_1_KEY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_CORALPOINT1")]
		public static async Task CORAL_32_1_CORALPOINT1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_CORALPOINT2")]
		public static async Task CORAL_32_1_CORALPOINT2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_CORALPOINT3")]
		public static async Task CORAL_32_1_CORALPOINT3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_HIDDEN_TRAP1")]
		public static async Task CORAL_32_1_HIDDEN_TRAP1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_HIDDEN_TRAP2")]
		public static async Task CORAL_32_1_HIDDEN_TRAP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_SQ_8")]
		public static async Task CORAL_32_1_SQ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_GUARD4")]
		public static async Task CORAL_32_1_GUARD4(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_GUARD4_BASIC1");
			await dialog.Msg("CORAL_32_1_GUARD4_BASIC2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_GUARD5")]
		public static async Task CORAL_32_1_GUARD5(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_GUARD5_BASIC1");
			await dialog.Msg("CORAL_32_1_GUARD5_BASIC2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_WORKER1")]
		public static async Task CORAL_32_1_WORKER1(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_WORKER1_basic01");
			await dialog.Msg("CORAL_32_1_WORKER1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_1_WORKER2")]
		public static async Task CORAL_32_1_WORKER2(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_1_WORKER2_basic01");
			await dialog.Msg("CORAL_32_1_WORKER2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_CORAL_32_11000")]
		public static async Task TREASUREBOX_LV_F_CORAL_32_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_272", 1);

		[DialogFunction("CORAL_32_2_DARUL1")]
		public static async Task CORAL_32_2_DARUL1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_DARUL2")]
		public static async Task CORAL_32_2_DARUL2(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_2_DARUL2_BG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_BERTA2")]
		public static async Task CORAL_32_2_BERTA2(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_2_BERTA2_BG2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_JURATEALTAR")]
		public static async Task CORAL_32_2_JURATEALTAR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_BERTA3")]
		public static async Task CORAL_32_2_BERTA3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_DARUL3")]
		public static async Task CORAL_32_2_DARUL3(Dialog dialog)
		{
			await dialog.Msg("CORAL_32_2_DARUL3_STD");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_3_CORAL")]
		public static async Task CORAL_32_2_SQ_3_CORAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_12_STONE")]
		public static async Task CORAL_32_2_SQ_12_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_12_STONE2")]
		public static async Task CORAL_32_2_SQ_12_STONE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_13_1")]
		public static async Task CORAL_32_2_SQ_13_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_13_2")]
		public static async Task CORAL_32_2_SQ_13_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_13_3")]
		public static async Task CORAL_32_2_SQ_13_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_13_4")]
		public static async Task CORAL_32_2_SQ_13_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_32_2_SQ_7_CORAL")]
		public static async Task CORAL_32_2_SQ_7_CORAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_CORAL_32_21000")]
		public static async Task TREASUREBOX_LV_F_CORAL_32_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_273", 1);

		[DialogFunction("EP14_F_CORAL_RAID_4_NPC_1")]
		public static async Task EP14_F_CORAL_RAID_4_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GODDESS_RAID_CORAL_PORTAL")]
		public static async Task GODDESS_RAID_CORAL_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_NUMBER_HI_LOW")]
		public static async Task ORCHARD_34_1_NUMBER_HI_LOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_NPC_1")]
		public static async Task ORCHARD_34_1_SQ_NPC_1(Dialog dialog)
		{
			await dialog.Msg("ORCHARD_34_1_SQ_NPC_1_basic01");
			await dialog.Msg("ORCHARD_34_1_SQ_NPC_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_NPC_2_1")]
		public static async Task ORCHARD_34_1_SQ_NPC_2_1(Dialog dialog)
		{
			await dialog.Msg("ORCHARD_34_1_SQ_NPC_2_1_basic01");
			await dialog.Msg("ORCHARD_34_1_SQ_NPC_2_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_NPC_2_2")]
		public static async Task ORCHARD_34_1_SQ_NPC_2_2(Dialog dialog)
		{
			await dialog.Msg("ORCHARD_34_1_SQ_NPC_2_2_basic01");
			await dialog.Msg("ORCHARD_34_1_SQ_NPC_2_2_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_1")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_2_1")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_2_2")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_2_3")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_2_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_2_4")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_2_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_2_5")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_2_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_3")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_4")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_5")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_6")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_7_1")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_7_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_7_2")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_7_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_7_3")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_7_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_8")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_9")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_10")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_11")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_12")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_13")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_13(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_14")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_14(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_1_SQ_2_OBJ_15")]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_15(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ORCHARD_34_11000")]
		public static async Task TREASUREBOX_LV_F_ORCHARD_34_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_270", 1);

		[DialogFunction("ORCHARD_34_3_SQ_NPC_1")]
		public static async Task ORCHARD_34_3_SQ_NPC_1(Dialog dialog)
		{
			await dialog.Msg("ORCHARD_34_3_SQ_NPC_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_NPC_2")]
		public static async Task ORCHARD_34_3_SQ_NPC_2(Dialog dialog)
		{
			await dialog.Msg("ORCHARD_34_3_SQ_NPC_2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_NPC_3")]
		public static async Task ORCHARD_34_3_SQ_NPC_3(Dialog dialog)
		{
			await dialog.Msg("ORCHARD_34_3_SQ_NPC_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_1")]
		public static async Task ORCHARD_34_3_SQ_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_2_1")]
		public static async Task ORCHARD_34_3_SQ_OBJ_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_2_2")]
		public static async Task ORCHARD_34_3_SQ_OBJ_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_3")]
		public static async Task ORCHARD_34_3_SQ_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_4")]
		public static async Task ORCHARD_34_3_SQ_OBJ_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_4_DUMMY")]
		public static async Task ORCHARD_34_3_SQ_OBJ_4_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_5")]
		public static async Task ORCHARD_34_3_SQ_OBJ_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_6")]
		public static async Task ORCHARD_34_3_SQ_OBJ_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_7")]
		public static async Task ORCHARD_34_3_SQ_OBJ_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_8_DUMMY")]
		public static async Task ORCHARD_34_3_SQ_OBJ_8_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_8")]
		public static async Task ORCHARD_34_3_SQ_OBJ_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_OBJ_9")]
		public static async Task ORCHARD_34_3_SQ_OBJ_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ORCHARD_34_3_SQ_NPC_4")]
		public static async Task ORCHARD_34_3_SQ_NPC_4(Dialog dialog)
		{
			await dialog.Msg("ORCHARD_34_3_SQ_NPC_4_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_ORCHARD_34_31000")]
		public static async Task TREASUREBOX_LV_F_ORCHARD_34_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_271", 1);

		[DialogFunction("HT2_ORCHARD_34_3_SPRING")]
		public static async Task HT2_ORCHARD_34_3_SPRING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_SIAULIAI_35_1")]
		public static async Task WARP_F_SIAULIAI_35_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("SIAULIAI_35_1_MONSTER_PAIR")]
		public static async Task SIAULIAI_35_1_MONSTER_PAIR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_NUMBER_BASEBALL")]
		public static async Task SIAULIAI_35_1_NUMBER_BASEBALL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_ALCHEMY_1")]
		public static async Task SIAULIAI_35_1_ALCHEMY_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_ALCHEMY_2")]
		public static async Task SIAULIAI_35_1_ALCHEMY_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_ALCHEMY_BAG")]
		public static async Task SIAULIAI_35_1_ALCHEMY_BAG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_ALCHEMY_3")]
		public static async Task SIAULIAI_35_1_ALCHEMY_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_ALCHEMY_4")]
		public static async Task SIAULIAI_35_1_ALCHEMY_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_ALCHEMY_5")]
		public static async Task SIAULIAI_35_1_ALCHEMY_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_LUCIEN")]
		public static async Task SIAULIAI_35_1_LUCIEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_LUCIEN_2")]
		public static async Task SIAULIAI_35_1_LUCIEN_2(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_35_1_LUCIEN_2_basic_1");
			await dialog.Msg("SIAULIAI_35_1_LUCIEN_2_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_LUCIEN_3")]
		public static async Task SIAULIAI_35_1_LUCIEN_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_SQ_12_HERB")]
		public static async Task SIAULIAI_35_1_SQ_12_HERB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_VILLAGE_A")]
		public static async Task SIAULIAI_35_1_VILLAGE_A(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_35_1_VILLAGE_A_basic_01");
			await dialog.Msg("SIAULIAI_35_1_VILLAGE_A_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_VILLAGE_B")]
		public static async Task SIAULIAI_35_1_VILLAGE_B(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_35_1_VILLAGE_B_basic_01");
			await dialog.Msg("SIAULIAI_35_1_VILLAGE_B_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_VILLAGE_C")]
		public static async Task SIAULIAI_35_1_VILLAGE_C(Dialog dialog)
		{
			await dialog.Msg("SIAULIAI_35_1_VILLAGE_C_basic_01");
			await dialog.Msg("SIAULIAI_35_1_VILLAGE_C_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_35_1_SQ_8_NPC_END")]
		public static async Task SIAULIAI_35_1_SQ_8_NPC_END(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_35_11000")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_35_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_266", 1);

		[DialogFunction("SIAULIAI351_HIDDENQ1_PREITEM")]
		public static async Task SIAULIAI351_HIDDENQ1_PREITEM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_351_HIDDENQ1_ITEM1")]
		public static async Task SIAULIAI_351_HIDDENQ1_ITEM1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAULIAI_351_HIDDENQ1_ITEM2")]
		public static async Task SIAULIAI_351_HIDDENQ1_ITEM2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_LUTAS")]
		public static async Task CORAL_35_2_LUTAS(Dialog dialog)
		{
			await dialog.Msg("CORAL_35_2_LUTAS_basic_1");
			await dialog.Msg("CORAL_35_2_LUTAS_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_LUTAS_2")]
		public static async Task CORAL_35_2_LUTAS_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_LUTAS_3")]
		public static async Task CORAL_35_2_LUTAS_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_LUTAS_4")]
		public static async Task CORAL_35_2_LUTAS_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_2_STONE")]
		public static async Task CORAL_35_2_SQ_2_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_6_TERRA")]
		public static async Task CORAL_35_2_SQ_6_TERRA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_MARINE_1")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_MARINE_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_MARINE_2")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_MARINE_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_MARINE_3")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_MARINE_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_TERRA_1")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_TERRA_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_TERRA_2")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_TERRA_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_TERRA_3")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_TERRA_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_HARMONY_1")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_HARMONY_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_HARMONY_2")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_HARMONY_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_35_2_SQ_13_MAGIC_HARMONY_3")]
		public static async Task CORAL_35_2_SQ_13_MAGIC_HARMONY_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_CORAL_35_21000")]
		public static async Task TREASUREBOX_LV_F_CORAL_35_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_267", 1);

		[DialogFunction("ZEALOT_GIMMICK_STONE1")]
		public static async Task ZEALOT_GIMMICK_STONE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PARDONER_6_1_NOTE01")]
		public static async Task JOB_PARDONER_6_1_NOTE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PARDONER_6_1_NOTE02")]
		public static async Task JOB_PARDONER_6_1_NOTE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PARDONER_6_1_NOTE03")]
		public static async Task JOB_PARDONER_6_1_NOTE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PARDONER_6_1_NOTE04")]
		public static async Task JOB_PARDONER_6_1_NOTE04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PARDONER_6_1_NOTE05")]
		public static async Task JOB_PARDONER_6_1_NOTE05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_PARDONER_6_1_NOTE06")]
		public static async Task JOB_PARDONER_6_1_NOTE06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_HI_LOW")]
		public static async Task ABBEY_35_3_HI_LOW(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_CURLING")]
		public static async Task ABBEY_35_3_CURLING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_DOMINIKAS")]
		public static async Task ABBEY_35_3_DOMINIKAS(Dialog dialog)
		{
			await dialog.Msg("ABBEY_35_3_SQ_1_basic01");
			await dialog.Msg("ABBEY_35_3_SQ_1_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_VILLAGE_A_2")]
		public static async Task ABBEY_35_3_VILLAGE_A_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_VILLAGE_B_2")]
		public static async Task ABBEY_35_3_VILLAGE_B_2(Dialog dialog)
		{
			await dialog.Msg("ABBEY_35_3_VILLAGE_B_2_basic_1");
			await dialog.Msg("ABBEY_35_3_VILLAGE_B_2_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_VILLAGE_C_2")]
		public static async Task ABBEY_35_3_VILLAGE_C_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_VINE")]
		public static async Task ABBEY_35_3_VINE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_PAPER")]
		public static async Task ABBEY_35_3_PAPER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_DOMINIKAS_2")]
		public static async Task ABBEY_35_3_DOMINIKAS_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_MAGIC")]
		public static async Task ABBEY_35_3_MAGIC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_MONK")]
		public static async Task ABBEY_35_3_MONK(Dialog dialog)
		{
			await dialog.Msg("ABBEY_35_3_MONK_basic_01");
			await dialog.Msg("ABBEY_35_3_MONK_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_SQ_11_EVILBILE")]
		public static async Task ABBEY_35_3_SQ_11_EVILBILE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_3_VILLAGE_B_3")]
		public static async Task ABBEY_35_3_VILLAGE_B_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_ABBEY_35_31000")]
		public static async Task TREASUREBOX_LV_D_ABBEY_35_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_268", 1);

		[DialogFunction("ABBEY353_HIDDENQ1_PREOBJ")]
		public static async Task ABBEY353_HIDDENQ1_PREOBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_4_MIXING")]
		public static async Task ABBEY_35_4_MIXING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_4_MONK")]
		public static async Task ABBEY_35_4_MONK(Dialog dialog)
		{
			await dialog.Msg("ABBEY_35_4_MONK_basic01");
			await dialog.Msg("ABBEY_35_4_MONK_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_4_ELDER")]
		public static async Task ABBEY_35_4_ELDER(Dialog dialog)
		{
			await dialog.Msg("ABBEY_35_4_ELDER_basic_01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_ABBEY_35_4")]
		public static async Task WARP_D_ABBEY_35_4(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("ABBEY_35_4_ELDER_2")]
		public static async Task ABBEY_35_4_ELDER_2(Dialog dialog)
		{
			await dialog.Msg("ABBEY_35_4_ELDER_2_basic_01");
			await dialog.Msg("ABBEY_35_4_ELDER_2_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ABBEY_35_4_SQ_2_FOOD")]
		public static async Task ABBEY_35_4_SQ_2_FOOD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_ABBEY354_1")]
		public static async Task SAGE_JOBQ_IN_ABBEY354_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_ABBEY354_2")]
		public static async Task SAGE_JOBQ_IN_ABBEY354_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_ABBEY354_3")]
		public static async Task SAGE_JOBQ_IN_ABBEY354_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SAGE_JOBQ_IN_ABBEY354_4")]
		public static async Task SAGE_JOBQ_IN_ABBEY354_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_ABBEY_35_41000")]
		public static async Task TREASUREBOX_LV_D_ABBEY_35_41000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_269", 1);

		[DialogFunction("CATACOMB_38_1_NPC_01")]
		public static async Task CATACOMB_38_1_NPC_01(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_38_1_NPC_01_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_NPC_02")]
		public static async Task CATACOMB_38_1_NPC_02(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_38_1_NPC_02_basic01");
			await dialog.Msg("CATACOMB_38_1_NPC_02_basic02");
			await dialog.Msg("CATACOMB_38_1_NPC_02_basic03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_GHOST_01")]
		public static async Task CATACOMB_38_1_GHOST_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_GHOST_02")]
		public static async Task CATACOMB_38_1_GHOST_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_GHOST_03")]
		public static async Task CATACOMB_38_1_GHOST_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_GHOST_04")]
		public static async Task CATACOMB_38_1_GHOST_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_OBJ_01")]
		public static async Task CATACOMB_38_1_OBJ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_NPC_03")]
		public static async Task CATACOMB_38_1_NPC_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_OBJ_02")]
		public static async Task CATACOMB_38_1_OBJ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_1_OBJ_03")]
		public static async Task CATACOMB_38_1_OBJ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_38_11000")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_38_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_216", 1);

		[DialogFunction("CATACOMB_38_2_NPC_01")]
		public static async Task CATACOMB_38_2_NPC_01(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_38_2_NPC_01_basic01");
			await dialog.Msg("CATACOMB_38_2_NPC_01_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_NPC_02")]
		public static async Task CATACOMB_38_2_NPC_02(Dialog dialog)
		{
			await dialog.Msg("CATACOMB_38_2_NPC_02_basic01");
			await dialog.Msg("CATACOMB_38_2_NPC_02_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_OBJ_01")]
		public static async Task CATACOMB_38_2_OBJ_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_DIARY_01")]
		public static async Task CATACOMB_38_2_DIARY_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_DIARY_02")]
		public static async Task CATACOMB_38_2_DIARY_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_DIARY_03")]
		public static async Task CATACOMB_38_2_DIARY_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_DIARY_04")]
		public static async Task CATACOMB_38_2_DIARY_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_OBJ_02")]
		public static async Task CATACOMB_38_2_OBJ_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_OBJ_02_EMPTY")]
		public static async Task CATACOMB_38_2_OBJ_02_EMPTY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_ID_CATACOMB_38_2")]
		public static async Task WARP_ID_CATACOMB_38_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("WARLOCK_MASTER")]
		public static async Task WARLOCK_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_WARLOCK_MASTER_basic1");
			await dialog.Msg("JOB_WARLOCK_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FEATHERFOOT_MASTER")]
		public static async Task FEATHERFOOT_MASTER(Dialog dialog)
		{
			await dialog.Msg("JOB_FEATHERFOOT_MASTER_basic1");
			await dialog.Msg("JOB_FEATHERFOOT_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_38_2_HAUBERK_BOOK_1")]
		public static async Task CATACOMB_38_2_HAUBERK_BOOK_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_38_21000")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_38_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_214", 1);

		[DialogFunction("CATACOMB382_HIDDEN_GHOST")]
		public static async Task CATACOMB382_HIDDEN_GHOST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB382_HIDDENQ1_SPIRIT")]
		public static async Task CATACOMB382_HIDDENQ1_SPIRIT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LOWLV_MASTER_ENCY_SQ_50")]
		public static async Task LOWLV_MASTER_ENCY_SQ_50(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LOWLV_MASTER_ENCY_SQ_50_DOLL")]
		public static async Task LOWLV_MASTER_ENCY_SQ_50_DOLL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0101_TO_WARP_0102")]
		public static async Task WARP_0101_TO_WARP_0102(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0102_TO_WARP_0101")]
		public static async Task WARP_0102_TO_WARP_0101(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0201_TO_WARP_0202")]
		public static async Task WARP_0201_TO_WARP_0202(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0202_TO_WARP_0201")]
		public static async Task WARP_0202_TO_WARP_0201(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0301_TO_WARP_0302")]
		public static async Task WARP_0301_TO_WARP_0302(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0401_TO_WARP_0402")]
		public static async Task WARP_0401_TO_WARP_0402(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0302_TO_WARP_0301")]
		public static async Task WARP_0302_TO_WARP_0301(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1001_TO_WARP_1002")]
		public static async Task WARP_1001_TO_WARP_1002(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1002_TO_WARP_1001")]
		public static async Task WARP_1002_TO_WARP_1001(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0402_TO_WARP_0401")]
		public static async Task WARP_0402_TO_WARP_0401(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0103_TO_WARP_0102")]
		public static async Task WARP_0103_TO_WARP_0102(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0702_TO_WARP_0701")]
		public static async Task WARP_0702_TO_WARP_0701(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0502_TO_WARP_0501")]
		public static async Task WARP_0502_TO_WARP_0501(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0601_TO_WARP_0602")]
		public static async Task WARP_0601_TO_WARP_0602(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1301_TO_WARP_1302")]
		public static async Task WARP_1301_TO_WARP_1302(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0802_TO_WARP_0801")]
		public static async Task WARP_0802_TO_WARP_0801(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0902_TO_WARP_0901")]
		public static async Task WARP_0902_TO_WARP_0901(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1302_TO_WARP_1301")]
		public static async Task WARP_1302_TO_WARP_1301(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0602_TO_WARP_0601")]
		public static async Task WARP_0602_TO_WARP_0601(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0501_TO_WARP_0502")]
		public static async Task WARP_0501_TO_WARP_0502(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0801_TO_WARP_0802")]
		public static async Task WARP_0801_TO_WARP_0802(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0701_TO_WARP_0702")]
		public static async Task WARP_0701_TO_WARP_0702(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_0901_TO_WARP_0902")]
		public static async Task WARP_0901_TO_WARP_0902(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1101_TO_WARP_1102")]
		public static async Task WARP_1101_TO_WARP_1102(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1201_TO_WARP_1202")]
		public static async Task WARP_1201_TO_WARP_1202(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1202_TO_WARP_0101")]
		public static async Task WARP_1202_TO_WARP_0101(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_1101_TO_WARP_0101")]
		public static async Task WARP_1101_TO_WARP_0101(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIAS54_1_ITEM_BOX")]
		public static async Task VELNIAS54_1_ITEM_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP54_1_EVENT01_START")]
		public static async Task VELNIASP54_1_EVENT01_START(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP54_1_TO_FARM49_3")]
		public static async Task VELNIASP54_1_TO_FARM49_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP54_MEMO1")]
		public static async Task VELNIASP54_MEMO1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP54_MEMO2")]
		public static async Task VELNIASP54_MEMO2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP54_MEMO3")]
		public static async Task VELNIASP54_MEMO3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIAS_TREASURE01")]
		public static async Task VELNIAS_TREASURE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIAS_TREASURE02")]
		public static async Task VELNIAS_TREASURE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_VELNIASPRISON_54_152")]
		public static async Task TREASUREBOX_LV_D_VELNIASPRISON_54_152(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_199", 1);

		[DialogFunction("JOB_MIKO_6_1_VPRISON_54_1")]
		public static async Task JOB_MIKO_6_1_VPRISON_54_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_MAN1")]
		public static async Task CORAL_44_1_MAN1(Dialog dialog)
		{
			await dialog.Msg("CORAL_44_1_MAN1_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_OLDMAN1")]
		public static async Task CORAL_44_1_OLDMAN1(Dialog dialog)
		{
			await dialog.Msg("CORAL_44_1_OLDMAN1_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_OLDMAN2")]
		public static async Task CORAL_44_1_OLDMAN2(Dialog dialog)
		{
			await dialog.Msg("CORAL_44_1_OLDMAN2_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_MAN2")]
		public static async Task CORAL_44_1_MAN2(Dialog dialog)
		{
			await dialog.Msg("CORAL_44_1_MAN2_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_SQ_10_BAG")]
		public static async Task CORAL_44_1_SQ_10_BAG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_SQ_80_CORAL")]
		public static async Task CORAL_44_1_SQ_80_CORAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_SQ_110_REDSTOEN")]
		public static async Task CORAL_44_1_SQ_110_REDSTOEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_WARP1")]
		public static async Task CORAL_44_1_WARP1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_1_WARP2")]
		public static async Task CORAL_44_1_WARP2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_OLDMAN")]
		public static async Task CORAL_44_2_OLDMAN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_MAN2")]
		public static async Task CORAL_44_2_MAN2(Dialog dialog)
		{
			await dialog.Msg("CORAL_44_2_MAN2_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_LOCK")]
		public static async Task CORAL_44_2_LOCK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_SQ_30_OBJ")]
		public static async Task CORAL_44_2_SQ_30_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_SQ_40_EFF")]
		public static async Task CORAL_44_2_SQ_40_EFF(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_SQ_60_STONE")]
		public static async Task CORAL_44_2_SQ_60_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_OLDMAN2")]
		public static async Task CORAL_44_2_OLDMAN2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_SQ_70_EFF")]
		public static async Task CORAL_44_2_SQ_70_EFF(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_SQ_90_DARK")]
		public static async Task CORAL_44_2_SQ_90_DARK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_2_MAN3")]
		public static async Task CORAL_44_2_MAN3(Dialog dialog)
		{
			await dialog.Msg("CORAL_44_2_MAN3_BASIC");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_OLDMAN1")]
		public static async Task CORAL_44_3_OLDMAN1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_MAN1")]
		public static async Task CORAL_44_3_MAN1(Dialog dialog)
		{
			await dialog.Msg("CORAL_44_3_MAN1_NQ");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_KRUVINA1")]
		public static async Task CORAL_44_3_KRUVINA1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_OLDMAN2")]
		public static async Task CORAL_44_3_OLDMAN2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_KRUVINA2")]
		public static async Task CORAL_44_3_KRUVINA2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_KRUVINA3")]
		public static async Task CORAL_44_3_KRUVINA3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_OLDMAN3")]
		public static async Task CORAL_44_3_OLDMAN3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_KRUVINA4")]
		public static async Task CORAL_44_3_KRUVINA4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_OLDMAN4")]
		public static async Task CORAL_44_3_OLDMAN4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_SQ_80_RUINS")]
		public static async Task CORAL_44_3_SQ_80_RUINS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CORAL_44_3_SQ_90_DARK")]
		public static async Task CORAL_44_3_SQ_90_DARK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_CORAL_44_333")]
		public static async Task TREASUREBOX_LV_F_CORAL_44_333(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_309", 1);

		[DialogFunction("LIMESTONECAVE_52_1_MEDENA")]
		public static async Task LIMESTONECAVE_52_1_MEDENA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_1_TRIA")]
		public static async Task LIMESTONECAVE_52_1_TRIA(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_1_TRIA_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMSTONE_52_1_CRYSTAL_PIECE")]
		public static async Task LIMSTONE_52_1_CRYSTAL_PIECE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_1_ANTIEVIL")]
		public static async Task LIMESTONE_52_1_ANTIEVIL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_1_MQ_6_CLEAR")]
		public static async Task LIMESTONE_52_1_MQ_6_CLEAR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_1_MEDENA_2")]
		public static async Task LIMESTONECAVE_52_1_MEDENA_2(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_1_MEDENA_2_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_1_BOAT")]
		public static async Task LIMESTONE_52_1_BOAT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_LIMESTONE_52_1")]
		public static async Task WARP_D_LIMESTONE_52_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_LIMESTONECAVE_52_11000")]
		public static async Task TREASUREBOX_LV_D_LIMESTONECAVE_52_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_286", 1);

		[DialogFunction("LSCAVE521_RMG_NPC_1")]
		public static async Task LSCAVE521_RMG_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_2_ALLENA")]
		public static async Task LIMESTONE_52_2_ALLENA(Dialog dialog)
		{
			await dialog.Msg("LIMESTONE_52_2_ALLENA_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_2_SERIJA")]
		public static async Task LIMESTONE_52_2_SERIJA(Dialog dialog)
		{
			await dialog.Msg("LIMESTONE_52_2_SERIJA_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_2_MEDENA")]
		public static async Task LIMESTONECAVE_52_2_MEDENA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_2_EVIL_1")]
		public static async Task LIMESTONE_52_2_EVIL_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_2_EVIL_2")]
		public static async Task LIMESTONE_52_2_EVIL_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_2_EVIL_3")]
		public static async Task LIMESTONE_52_2_EVIL_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_2_HEALING")]
		public static async Task LIMESTONE_52_2_HEALING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_2_ANTIEVIL")]
		public static async Task LIMESTONE_52_2_ANTIEVIL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_LIMESTONECAVE_52_21000")]
		public static async Task TREASUREBOX_LV_D_LIMESTONECAVE_52_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_287", 1);

		[DialogFunction("LIMESTONECAVE_52_3_SERIJA")]
		public static async Task LIMESTONECAVE_52_3_SERIJA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_3_DALIA")]
		public static async Task LIMESTONECAVE_52_3_DALIA(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_3_DALIA_basic_1");
			await dialog.Msg("LIMESTONECAVE_52_3_DALIA_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_3_SIUTE")]
		public static async Task LIMESTONECAVE_52_3_SIUTE(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_3_SIUTE_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_3_SERIJA_2")]
		public static async Task LIMESTONECAVE_52_3_SERIJA_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_3_ANTI_EVIL")]
		public static async Task LIMESTONE_52_3_ANTI_EVIL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_3_MQ_4_OBJ_1")]
		public static async Task LIMESTONE_52_3_MQ_4_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_3_MQ_4_OBJ_2")]
		public static async Task LIMESTONE_52_3_MQ_4_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_3_MQ_4_OBJ_3")]
		public static async Task LIMESTONE_52_3_MQ_4_OBJ_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_3_SERIJA_3")]
		public static async Task LIMESTONECAVE_52_3_SERIJA_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_3_SERIJA_4")]
		public static async Task LIMESTONECAVE_52_3_SERIJA_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_3_SIUTE_2")]
		public static async Task LIMESTONECAVE_52_3_SIUTE_2(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_3_SIUTE_2_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_3_ANTI_EVIL_ON")]
		public static async Task LIMESTONE_52_3_ANTI_EVIL_ON(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_3_MQ_8_MOSS")]
		public static async Task LIMESTONE_52_3_MQ_8_MOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_3_SIUTE_3")]
		public static async Task LIMESTONECAVE_52_3_SIUTE_3(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_3_SIUTE_3_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_LIMESTONECAVE_52_31000")]
		public static async Task TREASUREBOX_LV_D_LIMESTONECAVE_52_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_288", 1);

		[DialogFunction("LIMESTONECAVE_52_4_SERIJA")]
		public static async Task LIMESTONECAVE_52_4_SERIJA(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_4_SERIJA_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_4_MEDENA")]
		public static async Task LIMESTONECAVE_52_4_MEDENA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_4_PORTAL")]
		public static async Task LIMESTONE_52_4_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_4_MEDENA_2")]
		public static async Task LIMESTONECAVE_52_4_MEDENA_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_LIMESTONECAVE_52_41000")]
		public static async Task TREASUREBOX_LV_D_LIMESTONECAVE_52_41000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_289", 1);

		[DialogFunction("LIMESTONECAVE_52_5_MEDENA")]
		public static async Task LIMESTONECAVE_52_5_MEDENA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_5_DALIA")]
		public static async Task LIMESTONECAVE_52_5_DALIA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_5_MQ_6_OBJ_1")]
		public static async Task LIMESTONE_52_5_MQ_6_OBJ_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_5_MQ_6_OBJ_2")]
		public static async Task LIMESTONE_52_5_MQ_6_OBJ_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_5_MQ_5_EVIL_DEVICE")]
		public static async Task LIMESTONE_52_5_MQ_5_EVIL_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_5_MQ_6_ANTIEVIL_1")]
		public static async Task LIMESTONE_52_5_MQ_6_ANTIEVIL_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_5_MQ_6_ANTIEVIL_2")]
		public static async Task LIMESTONE_52_5_MQ_6_ANTIEVIL_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONE_52_5_GESTI")]
		public static async Task LIMESTONE_52_5_GESTI(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_5_MEDENA_2")]
		public static async Task LIMESTONECAVE_52_5_MEDENA_2(Dialog dialog)
		{
			await dialog.Msg("LIMESTONECAVE_52_5_MEDENA_2_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LIMESTONECAVE_52_5_DALIA_SOUL")]
		public static async Task LIMESTONECAVE_52_5_DALIA_SOUL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_LIMESTONE_52_5")]
		public static async Task WARP_D_LIMESTONE_52_5(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_LIMESTONECAVE_52_51000")]
		public static async Task TREASUREBOX_LV_D_LIMESTONECAVE_52_51000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_290", 1);

		[DialogFunction("VELNIASP_511_GROUP_2_1")]
		public static async Task VELNIASP_511_GROUP_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_511_GROUP_2_2")]
		public static async Task VELNIASP_511_GROUP_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_511_GROUP_1_1")]
		public static async Task VELNIASP_511_GROUP_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_511_GROUP_1_2")]
		public static async Task VELNIASP_511_GROUP_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_511_GROUP_3_1")]
		public static async Task VELNIASP_511_GROUP_3_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_511_GROUP_3_2")]
		public static async Task VELNIASP_511_GROUP_3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_511_GROUP_4_2")]
		public static async Task VELNIASP_511_GROUP_4_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP511_TO_FARM472")]
		public static async Task VELNIASP511_TO_FARM472(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP511_TO_VELNIASP512")]
		public static async Task VELNIASP511_TO_VELNIASP512(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON511_MQ_AUDRA")]
		public static async Task VPRISON511_MQ_AUDRA(Dialog dialog)
		{
			await dialog.Msg("VPRISON511_MQ_AUDRA_basic_3");
			await dialog.Msg("VPRISON511_MQ_AUDRA_basic_4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON511_MQ_ZYDRONE")]
		public static async Task VPRISON511_MQ_ZYDRONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON511_MQ_HAUBERK")]
		public static async Task VPRISON511_MQ_HAUBERK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON511_MQ_04_NPC")]
		public static async Task VPRISON511_MQ_04_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON_PAPER01")]
		public static async Task VPRISON_PAPER01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON_PAPER03")]
		public static async Task VPRISON_PAPER03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_VELNIASPRISON_51_133")]
		public static async Task TREASUREBOX_LV_D_VELNIASPRISON_51_133(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_190", 1);

		[DialogFunction("HIDDEN_MIKO_VPRISON_SUN")]
		public static async Task HIDDEN_MIKO_VPRISON_SUN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_MON")]
		public static async Task HIDDEN_MIKO_VPRISON_MON(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_TUES")]
		public static async Task HIDDEN_MIKO_VPRISON_TUES(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_WEDNES")]
		public static async Task HIDDEN_MIKO_VPRISON_WEDNES(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_THURS")]
		public static async Task HIDDEN_MIKO_VPRISON_THURS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_FRI")]
		public static async Task HIDDEN_MIKO_VPRISON_FRI(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_SATUR")]
		public static async Task HIDDEN_MIKO_VPRISON_SATUR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_12")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_1")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_2")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_3")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_4")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_5")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_6")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_7")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_8")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_9")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_10")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("HIDDEN_MIKO_VPRISON_BEAD_11")]
		public static async Task HIDDEN_MIKO_VPRISON_BEAD_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LOWLV_EYEOFBAIGA_SQ_20")]
		public static async Task LOWLV_EYEOFBAIGA_SQ_20(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON512_MQ_NORGAILE")]
		public static async Task VPRISON512_MQ_NORGAILE(Dialog dialog)
		{
			await dialog.Msg("VPRISON512_MQ_NORGAILE_basic_1");
			await dialog.Msg("VPRISON512_MQ_NORGAILE_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON512_MQ_ALDONA")]
		public static async Task VPRISON512_MQ_ALDONA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_512_GROUP_3_2")]
		public static async Task VELNIASP_512_GROUP_3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_512_GROUP_3_1")]
		public static async Task VELNIASP_512_GROUP_3_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_512_GROUP_2_2")]
		public static async Task VELNIASP_512_GROUP_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_512_GROUP_2_1")]
		public static async Task VELNIASP_512_GROUP_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_512_GROUP_1_1")]
		public static async Task VELNIASP_512_GROUP_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_512_GROUP_1_2")]
		public static async Task VELNIASP_512_GROUP_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP512_TO_VELNIASP511")]
		public static async Task VELNIASP512_TO_VELNIASP511(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP512_TO_VELNIASP513")]
		public static async Task VELNIASP512_TO_VELNIASP513(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_VPRISON_51_2")]
		public static async Task WARP_D_VPRISON_51_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("VPRISON512_STONE_01")]
		public static async Task VPRISON512_STONE_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_VELNIASPRISON_51_225")]
		public static async Task TREASUREBOX_LV_D_VELNIASPRISON_51_225(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_191", 1);

		[DialogFunction("VPRISON513_MQ_DAIVA_01")]
		public static async Task VPRISON513_MQ_DAIVA_01(Dialog dialog)
		{
			await dialog.Msg("VPRISON513_MQ_DAIVA_01_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON513_MQ_DAIVA_02")]
		public static async Task VPRISON513_MQ_DAIVA_02(Dialog dialog)
		{
			await dialog.Msg("VPRISON513_MQ_DAIVA_02_basic_1");
			await dialog.Msg("VPRISON513_MQ_DAIVA_02_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON513_MQ_SIGITA")]
		public static async Task VPRISON513_MQ_SIGITA(Dialog dialog)
		{
			await dialog.Msg("VPRISON513_MQ_SIGITA_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP513_TO_VELNIASP514")]
		public static async Task VELNIASP513_TO_VELNIASP514(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP513_TO_VELNIASP515")]
		public static async Task VELNIASP513_TO_VELNIASP515(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON513_MQ_DAIVA_03")]
		public static async Task VPRISON513_MQ_DAIVA_03(Dialog dialog)
		{
			await dialog.Msg("VPRISON513_MQ_DAIVA_02_basic_1");
			await dialog.Msg("VPRISON513_MQ_DAIVA_02_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_VELNIASPRISON_51_317")]
		public static async Task TREASUREBOX_LV_D_VELNIASPRISON_51_317(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_193", 1);

		[DialogFunction("VPRISON514_MQ_VAKARINE")]
		public static async Task VPRISON514_MQ_VAKARINE(Dialog dialog)
		{
			await dialog.Msg("VPRISON514_MQ_VAKARINE_basic01");
			await dialog.Msg("VPRISON514_MQ_VAKARINE_basic02");
			await dialog.Msg("VPRISON514_MQ_VAKARINE_basic05");
			await dialog.Msg("VPRISON514_MQ_VAKARINE_basic06");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_ALDONA")]
		public static async Task VPRISON514_MQ_ALDONA(Dialog dialog)
		{
			await dialog.Msg("VPRISON514_MQ_ALDONA_basic_3");
			await dialog.Msg("VPRISON514_MQ_ALDONA_basic_4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP514_TO_VELNIASP512")]
		public static async Task VELNIASP514_TO_VELNIASP512(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_ZYDRONE")]
		public static async Task VPRISON514_MQ_ZYDRONE(Dialog dialog)
		{
			await dialog.Msg("VPRISON514_MQ_ZYDRONE_basic01");
			await dialog.Msg("VPRISON514_MQ_ZYDRONE_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP514_TO_VELNIASP513")]
		public static async Task VELNIASP514_TO_VELNIASP513(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_514_GROUP_1_1")]
		public static async Task VELNIASP_514_GROUP_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_514_GROUP_1_2")]
		public static async Task VELNIASP_514_GROUP_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_514_GROUP_2_1")]
		public static async Task VELNIASP_514_GROUP_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_514_GROUP_2_2")]
		public static async Task VELNIASP_514_GROUP_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_514_GROUP_3_2")]
		public static async Task VELNIASP_514_GROUP_3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_02_NPC_01")]
		public static async Task VPRISON514_MQ_02_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_02_NPC_02")]
		public static async Task VPRISON514_MQ_02_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_02_NPC_03")]
		public static async Task VPRISON514_MQ_02_NPC_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_04_NPC_01")]
		public static async Task VPRISON514_MQ_04_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_04_NPC_02")]
		public static async Task VPRISON514_MQ_04_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_MQ_04_NPC_03")]
		public static async Task VPRISON514_MQ_04_NPC_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON514_SQ_03_NPC")]
		public static async Task VPRISON514_SQ_03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP514_TO_GUILDMISSION")]
		public static async Task VELNIASP514_TO_GUILDMISSION(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_VELNIASPRISON_51_432")]
		public static async Task TREASUREBOX_LV_D_VELNIASPRISON_51_432(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_192", 1);

		[DialogFunction("VELNIASP_515_GROUP_2_2")]
		public static async Task VELNIASP_515_GROUP_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_515_GROUP_2_1")]
		public static async Task VELNIASP_515_GROUP_2_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_515_GROUP_1_1")]
		public static async Task VELNIASP_515_GROUP_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_515_GROUP_1_2")]
		public static async Task VELNIASP_515_GROUP_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP515_TO_VELNIASP513")]
		public static async Task VELNIASP515_TO_VELNIASP513(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON515_MQ_VAKARINE")]
		public static async Task VPRISON515_MQ_VAKARINE(Dialog dialog)
		{
			await dialog.Msg("VPRISON515_MQ_VAKARINE_basic_01");
			await dialog.Msg("VPRISON515_MQ_VAKARINE_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP_515_GROUP_3_2")]
		public static async Task VELNIASP_515_GROUP_3_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON515_MQ_SIGITA")]
		public static async Task VPRISON515_MQ_SIGITA(Dialog dialog)
		{
			await dialog.Msg("VPRISON515_MQ_SIGITA_basic_1");
			await dialog.Msg("VPRISON515_MQ_SIGITA_basic_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON515_SQ_03_NPC")]
		public static async Task VPRISON515_SQ_03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_VELNIASPRISON_51_523")]
		public static async Task TREASUREBOX_LV_D_VELNIASPRISON_51_523(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_194", 1);

		[DialogFunction("LSCAVE551_ALTAR_NPC")]
		public static async Task LSCAVE551_ALTAR_NPC(Dialog dialog)
		{
			await dialog.Msg("LSCAVE551_ALTAR_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LSCAVE551_HELANDA_NPC")]
		public static async Task LSCAVE551_HELANDA_NPC(Dialog dialog)
		{
			await dialog.Msg("LSCAVE551_HELANDA_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LSCAVE551_CELVERKA_NPC")]
		public static async Task LSCAVE551_CELVERKA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LSCAVE551_GIGO_NPC")]
		public static async Task LSCAVE551_GIGO_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LSCAVE551_SQ_4_OBJ")]
		public static async Task LSCAVE551_SQ_4_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LSCAVE551_SQ_7_NPC")]
		public static async Task LSCAVE551_SQ_7_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LSCAVE551_SQB_1_OBJ")]
		public static async Task LSCAVE551_SQB_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("LSCAVE551_SQB_3_OBJ")]
		public static async Task LSCAVE551_SQB_3_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_LIMESTONECAVE_55_11000")]
		public static async Task TREASUREBOX_LV_D_LIMESTONECAVE_55_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_304", 1);

		[DialogFunction("BRACKEN421_SQ_01")]
		public static async Task BRACKEN421_SQ_01(Dialog dialog)
		{
			await dialog.Msg("BRACKEN421_SQ_01_basic1");
			await dialog.Msg("BRACKEN421_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN421_SQ_03")]
		public static async Task BRACKEN421_SQ_03(Dialog dialog)
		{
			await dialog.Msg("BRACKEN421_SQ_03_basic1");
			await dialog.Msg("BRACKEN421_SQ_03_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN421_SQ_04_2")]
		public static async Task BRACKEN421_SQ_04_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN421_SQ_06")]
		public static async Task BRACKEN421_SQ_06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN421_SQ_04_1")]
		public static async Task BRACKEN421_SQ_04_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN421_SQ_05")]
		public static async Task BRACKEN421_SQ_05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRAKEN_42_1_MEDENA")]
		public static async Task BRAKEN_42_1_MEDENA(Dialog dialog)
		{
			await dialog.Msg("BRAKEN_42_1_MEDENA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN421_SQ_08_2")]
		public static async Task BRACKEN421_SQ_08_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FEATHERFOOT_ORB_JOB_ITEM")]
		public static async Task FEATHERFOOT_ORB_JOB_ITEM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN_42_1_JOB_SHADOW_OBJECT")]
		public static async Task BRACKEN_42_1_JOB_SHADOW_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN_42_1_TO_GUILD_FIELD_BOSS")]
		public static async Task BRACKEN_42_1_TO_GUILD_FIELD_BOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_01")]
		public static async Task BRACKEN422_SQ_01(Dialog dialog)
		{
			await dialog.Msg("BRACKEN422_SQ_01_basic1");
			await dialog.Msg("BRACKEN422_SQ_01_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_03")]
		public static async Task BRACKEN422_SQ_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_04")]
		public static async Task BRACKEN422_SQ_04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_06_1")]
		public static async Task BRACKEN422_SQ_06_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_06_2")]
		public static async Task BRACKEN422_SQ_06_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_06_3")]
		public static async Task BRACKEN422_SQ_06_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_06_4")]
		public static async Task BRACKEN422_SQ_06_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_06_5")]
		public static async Task BRACKEN422_SQ_06_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_07")]
		public static async Task BRACKEN422_SQ_07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_12")]
		public static async Task BRACKEN422_SQ_12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_08_1")]
		public static async Task BRACKEN422_SQ_08_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_08_2")]
		public static async Task BRACKEN422_SQ_08_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_08_3")]
		public static async Task BRACKEN422_SQ_08_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_08_4")]
		public static async Task BRACKEN422_SQ_08_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_08_5")]
		public static async Task BRACKEN422_SQ_08_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN422_SQ_11")]
		public static async Task BRACKEN422_SQ_11(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN_42_2_TO_GUILD_FIELD_BOSS")]
		public static async Task BRACKEN_42_2_TO_GUILD_FIELD_BOSS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_JAGUAR_NPC")]
		public static async Task MASTER_JAGUAR_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_Char5_18_1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ_NPC1")]
		public static async Task BRACKEN431_SUBQ_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN431_SUBQ_NPC1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ_START_NPC")]
		public static async Task BRACKEN431_SUBQ_START_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ_NPC2")]
		public static async Task BRACKEN431_SUBQ_NPC2(Dialog dialog)
		{
			await dialog.Msg("BRACKEN431_SUBQ_NPC2_basic1");
			await dialog.Msg("BRACKEN431_SUBQ_NPC_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ_ALTAR_NPC")]
		public static async Task BRACKEN431_SUBQ_ALTAR_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ1_NPC1")]
		public static async Task BRACKEN431_SUBQ1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ4_NPC")]
		public static async Task BRACKEN431_SUBQ4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ6_NPC1")]
		public static async Task BRACKEN431_SUBQ6_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ6_NPC2")]
		public static async Task BRACKEN431_SUBQ6_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ6_NPC3")]
		public static async Task BRACKEN431_SUBQ6_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ6_NPC4")]
		public static async Task BRACKEN431_SUBQ6_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ6_SEED")]
		public static async Task BRACKEN431_SUBQ6_SEED(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN431_SUBQ7_NPC")]
		public static async Task BRACKEN431_SUBQ7_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_BRACKEN_43_11000")]
		public static async Task TREASUREBOX_LV_F_BRACKEN_43_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_296", 1);

		[DialogFunction("BRACKEN432_SUBQ_NPC1")]
		public static async Task BRACKEN432_SUBQ_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN432_SUBQ_NPC1_basic1");
			await dialog.Msg("BRACKEN432_SUBQ_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ_NPC2")]
		public static async Task BRACKEN432_SUBQ_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ8_NPC")]
		public static async Task BRACKEN432_SUBQ8_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ1_BRACKEN_NPC")]
		public static async Task BRACKEN432_SUBQ1_BRACKEN_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ1_WATER_NPC")]
		public static async Task BRACKEN432_SUBQ1_WATER_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ1_FLOWER_NPC")]
		public static async Task BRACKEN432_SUBQ1_FLOWER_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ3_MON_A")]
		public static async Task BRACKEN432_SUBQ3_MON_A(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ3_MON_B")]
		public static async Task BRACKEN432_SUBQ3_MON_B(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN432_SUBQ4_BRACKEN_NPC")]
		public static async Task BRACKEN432_SUBQ4_BRACKEN_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_BRACKEN_43_2")]
		public static async Task WARP_F_BRACKEN_43_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_F_BRACKEN_43_21000")]
		public static async Task TREASUREBOX_LV_F_BRACKEN_43_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_297", 1);

		[DialogFunction("BRACKEN432_KQ_1_BOX")]
		public static async Task BRACKEN432_KQ_1_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_TIGERHUNTER_NPC")]
		public static async Task MASTER_TIGERHUNTER_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_TIGERHUNTER_NPC_basic1");
			await dialog.Msg("MASTER_TIGERHUNTER_NPC_basic2");
			await dialog.Msg("MASTER_TIGERHUNTER_NPC_basic3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ1_NPC1")]
		public static async Task BRACKEN433_SUBQ1_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN433_SUBQ1_NPC1_basic1");
			await dialog.Msg("BRACKEN433_SUBQ1_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ3_NPC1")]
		public static async Task BRACKEN433_SUBQ3_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN433_SUBQ3_NPC1_basic1");
			await dialog.Msg("BRACKEN433_SUBQ3_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ2_NPC1")]
		public static async Task BRACKEN433_SUBQ2_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN433_SUBQ2_NPC1_basic1");
			await dialog.Msg("BRACKEN433_SUBQ2_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ5_NPC1")]
		public static async Task BRACKEN433_SUBQ5_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN433_SUBQ5_NPC1_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ1_FLOWER")]
		public static async Task BRACKEN433_SUBQ1_FLOWER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ1_BRACKEN")]
		public static async Task BRACKEN433_SUBQ1_BRACKEN(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ1_GRASS")]
		public static async Task BRACKEN433_SUBQ1_GRASS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ4_NPC1")]
		public static async Task BRACKEN433_SUBQ4_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ5_NPC2")]
		public static async Task BRACKEN433_SUBQ5_NPC2(Dialog dialog)
		{
			await dialog.Msg("BRACKEN433_SUBQ5_NPC2_basic1");
			await dialog.Msg("BRACKEN433_SUBQ5_NPC2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ4_NPC2")]
		public static async Task BRACKEN433_SUBQ4_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ4_NPC3")]
		public static async Task BRACKEN433_SUBQ4_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ4_NPC4")]
		public static async Task BRACKEN433_SUBQ4_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ4_NPC5")]
		public static async Task BRACKEN433_SUBQ4_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ4_NPC6")]
		public static async Task BRACKEN433_SUBQ4_NPC6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ4_NPC7")]
		public static async Task BRACKEN433_SUBQ4_NPC7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ1_NPC2")]
		public static async Task BRACKEN433_SUBQ1_NPC2(Dialog dialog)
		{
			await dialog.Msg("BRACKEN433_SUBQ1_NPC2_basic1");
			await dialog.Msg("BRACKEN433_SUBQ1_NPC2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_SUBQ3_NPC2")]
		public static async Task BRACKEN433_SUBQ3_NPC2(Dialog dialog)
		{
			await dialog.Msg("BRACKEN433_SUBQ3_NPC2_basic1");
			await dialog.Msg("BRACKEN433_SUBQ3_NPC2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN433_ARROW_NPC")]
		public static async Task BRACKEN433_ARROW_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_BRACKEN_43_31000")]
		public static async Task TREASUREBOX_LV_F_BRACKEN_43_31000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_298", 1);

		[DialogFunction("BRACKEN_43_3_JOB_SHADOW_OBJECT")]
		public static async Task BRACKEN_43_3_JOB_SHADOW_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ1_NPC1")]
		public static async Task BRACKEN434_SUBQ1_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN434_SUBQ1_NPC1_basic1");
			await dialog.Msg("BRACKEN434_SUBQ1_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ9_NPC1")]
		public static async Task BRACKEN434_SUBQ9_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN434_SUBQ9_NPC1_basic1");
			await dialog.Msg("BRACKEN434_SUBQ9_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ10_NPC1")]
		public static async Task BRACKEN434_SUBQ10_NPC1(Dialog dialog)
		{
			await dialog.Msg("BRACKEN434_SUBQ10_NPC1_basic1");
			await dialog.Msg("BRACKEN434_SUBQ10_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ1_NPC2")]
		public static async Task BRACKEN434_SUBQ1_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ1_NPC3")]
		public static async Task BRACKEN434_SUBQ1_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ1_NPC4")]
		public static async Task BRACKEN434_SUBQ1_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ9_NPC2")]
		public static async Task BRACKEN434_SUBQ9_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ10_NPC2")]
		public static async Task BRACKEN434_SUBQ10_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ1_PAPER_NPC")]
		public static async Task BRACKEN434_SUBQ1_PAPER_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ_STELE_NPC1")]
		public static async Task BRACKEN434_SUBQ_STELE_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ_STELE_NPC2")]
		public static async Task BRACKEN434_SUBQ_STELE_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ_STELE_NPC3")]
		public static async Task BRACKEN434_SUBQ_STELE_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ_STELE_NPC4")]
		public static async Task BRACKEN434_SUBQ_STELE_NPC4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ8_STONE_NPC1")]
		public static async Task BRACKEN434_SUBQ8_STONE_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ_STELE_NPC5")]
		public static async Task BRACKEN434_SUBQ_STELE_NPC5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUB6_NPC1")]
		public static async Task BRACKEN434_SUB6_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUB6_PLANT_NPC")]
		public static async Task BRACKEN434_SUB6_PLANT_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BRACKEN434_SUBQ10_NPC3")]
		public static async Task BRACKEN434_SUBQ10_NPC3(Dialog dialog)
		{
			await dialog.Msg("BRACKEN434_SUBQ10_NPC3_basic1");
			await dialog.Msg("BRACKEN434_SUBQ10_NPC3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_BRACKEN_43_41000")]
		public static async Task TREASUREBOX_LV_F_BRACKEN_43_41000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_299", 1);

		[DialogFunction("FARM47_LEADER")]
		public static async Task FARM47_LEADER(Dialog dialog)
		{
			await dialog.Msg("FARM47_LEADER_BASIC01");
			await dialog.Msg("FARM47_LEADER_BASIC03");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_TAURAS")]
		public static async Task FARM47_TAURAS(Dialog dialog)
		{
			await dialog.Msg("FARM47_TAURAS_BASIC01");
			await dialog.Msg("FARM47_TAURAS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_ALBINA")]
		public static async Task FARM47_ALBINA(Dialog dialog)
		{
			await dialog.Msg("FARM47_ALBINA_BASIC01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DRYER")]
		public static async Task FARM47_DRYER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_FENCE")]
		public static async Task FARM47_FENCE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC31_D")]
		public static async Task FARM47_MAGIC31_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC32")]
		public static async Task FARM47_MAGIC32(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_FARMER01")]
		public static async Task FARM47_FARMER01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_FARMER02")]
		public static async Task FARM47_FARMER02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_FARMER03")]
		public static async Task FARM47_FARMER03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_FARMER04")]
		public static async Task FARM47_FARMER04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_WOODPICK")]
		public static async Task FARM47_WOODPICK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_HERBS")]
		public static async Task FARM47_HERBS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_FARM_47_1")]
		public static async Task WARP_F_FARM_47_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("FARM47_1_HIDDEN_EVENT")]
		public static async Task FARM47_1_HIDDEN_EVENT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FARM_47_164")]
		public static async Task TREASUREBOX_LV_F_FARM_47_164(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_159", 1);

		[DialogFunction("FARM471_RP_1_NPC")]
		public static async Task FARM471_RP_1_NPC(Dialog dialog)
		{
			await dialog.Msg("FARM471_RP_1_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM471_RP_1_OBJ")]
		public static async Task FARM471_RP_1_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("RETIARII_MASTER")]
		public static async Task RETIARII_MASTER(Dialog dialog)
		{
			await dialog.Msg("RETIARII_MASTER_basic1");
			await dialog.Msg("RETIARII_MASTER_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP5_2_NPC2")]
		public static async Task CHAR120_MSTEP5_2_NPC2(Dialog dialog)
		{
			await dialog.Msg("CHAR120_MSTEP5_2_NPC2_basic1");
			await dialog.Msg("CHAR120_MSTEP5_2_NPC2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CHAR120_MSTEP5_2_NPC1")]
		public static async Task CHAR120_MSTEP5_2_NPC1(Dialog dialog)
		{
			await dialog.Msg("CHAR120_MSTEP5_2_NPC1_basic1");
			await dialog.Msg("CHAR120_MSTEP5_2_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("RETIARII_AGILITY_TRAINING")]
		public static async Task RETIARII_AGILITY_TRAINING(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_JOANA")]
		public static async Task FARM47_JOANA(Dialog dialog)
		{
			await dialog.Msg("FARM47_JOANA_BASIC01");
			await dialog.Msg("FARM47_JOANA_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_JONARIS")]
		public static async Task FARM47_JONARIS(Dialog dialog)
		{
			await dialog.Msg("FARM47_JONARIS_BASIC01");
			await dialog.Msg("FARM47_JONARIS_BASIC02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DZIUGAS")]
		public static async Task FARM47_DZIUGAS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_HEAD_D")]
		public static async Task FARM47_HEAD_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DRUM01_D")]
		public static async Task FARM47_DRUM01_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_DRUM02_D")]
		public static async Task FARM47_DRUM02_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_WING_D")]
		public static async Task FARM47_WING_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC12")]
		public static async Task FARM47_MAGIC12(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM_47_2_TO_VELNIASP511")]
		public static async Task FARM_47_2_TO_VELNIASP511(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP511_PORTAL_MAGE")]
		public static async Task VELNIASP511_PORTAL_MAGE(Dialog dialog)
		{
			await dialog.Msg("VELNIASP511_PORTAL_MAGE_basic_1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VELNIASP511_PORTAL_HAUBERK")]
		public static async Task VELNIASP511_PORTAL_HAUBERK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_OLD_WOOD")]
		public static async Task FARM47_OLD_WOOD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC_FAKE")]
		public static async Task FARM47_MAGIC_FAKE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_FIRE_ON")]
		public static async Task FARM47_FIRE_ON(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_2_DIARY")]
		public static async Task FARM47_2_DIARY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("VPRISON_PAPER02")]
		public static async Task VPRISON_PAPER02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PARTY_Q9_TRIGGER")]
		public static async Task PARTY_Q9_TRIGGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FARM_47_255")]
		public static async Task TREASUREBOX_LV_F_FARM_47_255(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_158", 1);

		[DialogFunction("FARM47_ARUNAS")]
		public static async Task FARM47_ARUNAS(Dialog dialog)
		{
			await dialog.Msg("FARM47_ARUNAS_basic01");
			await dialog.Msg("FARM47_ARUNAS_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_RIMAS")]
		public static async Task FARM47_RIMAS(Dialog dialog)
		{
			await dialog.Msg("FARM47_RIMAS_BASIC03");
			await dialog.Msg("FARM47_RIMAS_BASIC04");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_BARON")]
		public static async Task FARM47_BARON(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_BENIUS")]
		public static async Task FARM47_BENIUS(Dialog dialog)
		{
			await dialog.Msg("FARM47_BENIUS_basic01");
			await dialog.Msg("FARM47_BENIUS_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC01")]
		public static async Task FARM47_MAGIC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC02")]
		public static async Task FARM47_MAGIC02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC03")]
		public static async Task FARM47_MAGIC03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC04_D")]
		public static async Task FARM47_MAGIC04_D(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC_PRE")]
		public static async Task FARM47_MAGIC_PRE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_HUT")]
		public static async Task FARM47_HUT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_CROPS01")]
		public static async Task FARM47_CROPS01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_PROTECT_GEM_1")]
		public static async Task FARM47_PROTECT_GEM_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_BURY01")]
		public static async Task FARM47_BURY01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FARM47_MAGIC_PLACE")]
		public static async Task FARM47_MAGIC_PLACE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_FARM_47_339")]
		public static async Task TREASUREBOX_LV_F_FARM_47_339(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "TSW02_104", 1, "TreasureboxKey3");

		[DialogFunction("TREASUREBOX_LV_F_FARM_47_3700")]
		public static async Task TREASUREBOX_LV_F_FARM_47_3700(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_157", 1);

		[DialogFunction("TABLELAND_11_1_CART_MANAGER")]
		public static async Task TABLELAND_11_1_CART_MANAGER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_PAIR")]
		public static async Task TABLELAND_11_1_PAIR(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_ADOMAS")]
		public static async Task TABLELAND_11_1_ADOMAS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_ADOMAS_BAG")]
		public static async Task TABLELAND_11_1_ADOMAS_BAG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_LEMIJA")]
		public static async Task TABLELAND_11_1_LEMIJA(Dialog dialog)
		{
			await dialog.Msg("TABLELAND_11_1_LEMIJA_basic01");
			await dialog.Msg("TABLELAND_11_1_LEMIJA_basic02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_SQ_03_LAST")]
		public static async Task TABLELAND_11_1_SQ_03_LAST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_SQ_03_LOCK")]
		public static async Task TABLELAND_11_1_SQ_03_LOCK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_SQ_03_FOOD")]
		public static async Task TABLELAND_11_1_SQ_03_FOOD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_SQ_03_LETTER")]
		public static async Task TABLELAND_11_1_SQ_03_LETTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_SQ_03_BLOOD")]
		public static async Task TABLELAND_11_1_SQ_03_BLOOD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_SQ_03_TRACE")]
		public static async Task TABLELAND_11_1_SQ_03_TRACE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_LEMIJA2")]
		public static async Task TABLELAND_11_1_LEMIJA2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_FAUSTAS")]
		public static async Task TABLELAND_11_1_FAUSTAS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_RIP")]
		public static async Task TABLELAND_11_1_RIP(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_LEMIJA3")]
		public static async Task TABLELAND_11_1_LEMIJA3(Dialog dialog)
		{
			await dialog.Msg("TABLELAND_11_1_LEMIJA3_basic_01");
			await dialog.Msg("TABLELAND_11_1_LEMIJA3_basic_02");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TABLELAND_11_1_SQ_07_ENDNPC")]
		public static async Task TABLELAND_11_1_SQ_07_ENDNPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("JOB_DRAGOON_8_1_KEY")]
		public static async Task JOB_DRAGOON_8_1_KEY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_F_TABLELAND_11_11000")]
		public static async Task TREASUREBOX_LV_F_TABLELAND_11_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_263", 1);

		[DialogFunction("RVR_BK_KAZE_NPC_1")]
		public static async Task RVR_BK_KAZE_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("BORUTOSKAPAS_GO_GUILDMISSION")]
		public static async Task BORUTOSKAPAS_GO_GUILDMISSION(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_SKIRGAILA")]
		public static async Task THREECMLAKE_SKIRGAILA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_ABRAOMAS")]
		public static async Task THREECMLAKE_ABRAOMAS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_BLACKMAN01")]
		public static async Task THREECMLAKE_BLACKMAN01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_BLACKMAN02")]
		public static async Task THREECMLAKE_BLACKMAN02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_BLACKMAN03")]
		public static async Task THREECMLAKE_BLACKMAN03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LINK_STONE01")]
		public static async Task THREECMLAKE261_SQ02_LINK_STONE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LINK_STONE02")]
		public static async Task THREECMLAKE261_SQ02_LINK_STONE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ03_LINK_STONE01")]
		public static async Task THREECMLAKE261_SQ03_LINK_STONE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ03_LINK_STONE02")]
		public static async Task THREECMLAKE261_SQ03_LINK_STONE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ04_LINK_STONE01")]
		public static async Task THREECMLAKE261_SQ04_LINK_STONE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ04_LINK_STONE02")]
		public static async Task THREECMLAKE261_SQ04_LINK_STONE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN01_1")]
		public static async Task THREECMLAKE261_SQ02_LANTERN01_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN01_2")]
		public static async Task THREECMLAKE261_SQ02_LANTERN01_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN01_3")]
		public static async Task THREECMLAKE261_SQ02_LANTERN01_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN01_4")]
		public static async Task THREECMLAKE261_SQ02_LANTERN01_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_SACK")]
		public static async Task THREECMLAKE_SACK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_SKETCH_TRIG")]
		public static async Task THREECMLAKE_SKETCH_TRIG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_CART")]
		public static async Task THREECMLAKE_CART(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN02_1")]
		public static async Task THREECMLAKE261_SQ02_LANTERN02_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN02_2")]
		public static async Task THREECMLAKE261_SQ02_LANTERN02_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN02_3")]
		public static async Task THREECMLAKE261_SQ02_LANTERN02_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE261_SQ02_LANTERN02_4")]
		public static async Task THREECMLAKE261_SQ02_LANTERN02_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_F_3CMLAKE_26_1")]
		public static async Task WARP_F_3CMLAKE_26_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("F_3CMLAKE_26_1_COLLECTION_OBJ")]
		public static async Task F_3CMLAKE_26_1_COLLECTION_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_SOLCOMM")]
		public static async Task THREECMLAKE_SOLCOMM(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ03_LINK_STONE")]
		public static async Task THREECMLAKE262_SQ03_LINK_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ05_LINK_STONE")]
		public static async Task THREECMLAKE262_SQ05_LINK_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_LEONARDAS")]
		public static async Task THREECMLAKE_LEONARDAS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ09_BOX")]
		public static async Task THREECMLAKE262_SQ09_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ03_SECURITY_DEVICE")]
		public static async Task THREECMLAKE262_SQ03_SECURITY_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ06_FALSE_STONE01")]
		public static async Task THREECMLAKE262_SQ06_FALSE_STONE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ06_FALSE_STONE02")]
		public static async Task THREECMLAKE262_SQ06_FALSE_STONE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ06_FALSE_STONE03")]
		public static async Task THREECMLAKE262_SQ06_FALSE_STONE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE262_SQ06_TRUE_STONE")]
		public static async Task THREECMLAKE262_SQ06_TRUE_STONE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_BOMB_DEVICE")]
		public static async Task THREECMLAKE_BOMB_DEVICE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("THREECMLAKE_BLACKMAN04")]
		public static async Task THREECMLAKE_BLACKMAN04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_26_2_ZEMINA")]
		public static async Task F_3CMLAKE_26_2_ZEMINA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("F_3CMLAKE_26_2_COLLECTION_OBJ")]
		public static async Task F_3CMLAKE_26_2_COLLECTION_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF591_TYPEB_OBJ")]
		public static async Task UNDERF591_TYPEB_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_MEMO01_NPC")]
		public static async Task ROKAS_MEMO01_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_UNDERF591_TYPED_LAMP_01")]
		public static async Task FD_UNDERF591_TYPED_LAMP_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_UNDERFORTRESS_59_138")]
		public static async Task TREASUREBOX_LV_D_UNDERFORTRESS_59_138(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_145", 1);

		[DialogFunction("JOB_MIKO_6_1_UNDER_59_1")]
		public static async Task JOB_MIKO_6_1_UNDER_59_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_TYPEB_ALTER_1")]
		public static async Task UNDERF592_TYPEB_ALTER_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_TYPEB_ALTER_2")]
		public static async Task UNDERF592_TYPEB_ALTER_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_TYPEB_ALTER_3")]
		public static async Task UNDERF592_TYPEB_ALTER_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_TYPEB_ALTER_4")]
		public static async Task UNDERF592_TYPEB_ALTER_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_TYPEB_ALTER_5")]
		public static async Task UNDERF592_TYPEB_ALTER_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_TYPEB_ALTER_6")]
		public static async Task UNDERF592_TYPEB_ALTER_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_UNDERF592_TYPEA_ALTER")]
		public static async Task FD_UNDERF592_TYPEA_ALTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_ZEMINA_STATUE")]
		public static async Task UNDERF592_ZEMINA_STATUE(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("WARP_D_UNDERFORTRESS_59_2")]
		public static async Task WARP_D_UNDERFORTRESS_59_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("ROKAS_MEMO02_NPC")]
		public static async Task ROKAS_MEMO02_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF592_TYPEC_DEFENCE_1")]
		public static async Task UNDERF592_TYPEC_DEFENCE_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_UNDERFORTRESS_59_245")]
		public static async Task TREASUREBOX_LV_D_UNDERFORTRESS_59_245(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_146", 1);

		[DialogFunction("UNDERF593_TYPED_BOMBCART_1")]
		public static async Task UNDERF593_TYPED_BOMBCART_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF593_TYPED_BOMBCART_2")]
		public static async Task UNDERF593_TYPED_BOMBCART_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF593_TYPED_BOMB")]
		public static async Task UNDERF593_TYPED_BOMB(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNDERF593_TYPED_SEAL")]
		public static async Task UNDERF593_TYPED_SEAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ROKAS_MEMO03_NPC")]
		public static async Task ROKAS_MEMO03_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_UNDERFORTRESS_59_334")]
		public static async Task TREASUREBOX_LV_D_UNDERFORTRESS_59_334(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_147", 1);

		[DialogFunction("STARTOWER_60_1_SYSTEM_01")]
		public static async Task STARTOWER_60_1_SYSTEM_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_SYSTEM_02")]
		public static async Task STARTOWER_60_1_SYSTEM_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_SYSTEM_03")]
		public static async Task STARTOWER_60_1_SYSTEM_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_STORNSTATUE")]
		public static async Task STARTOWER_60_1_STORNSTATUE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_CANDLESTICK")]
		public static async Task STARTOWER_60_1_CANDLESTICK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_DAILY_BOX")]
		public static async Task STARTOWER_60_1_DAILY_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_STARSTORN_01")]
		public static async Task STARTOWER_60_1_STARSTORN_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_STARTOWER_60_1")]
		public static async Task WARP_D_STARTOWER_60_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("STARTOWER_60_1_MEMO_01")]
		public static async Task STARTOWER_60_1_MEMO_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_MEMO_02")]
		public static async Task STARTOWER_60_1_MEMO_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_60_1_MEMO_03")]
		public static async Task STARTOWER_60_1_MEMO_03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_STARTOWER_60_127")]
		public static async Task TREASUREBOX_LV_D_STARTOWER_60_127(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_182", 1);

		[DialogFunction("JOB_MIKO_6_1_STOWER_60_1")]
		public static async Task JOB_MIKO_6_1_STOWER_60_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_NPC_02")]
		public static async Task FD_FTOWER611_TYPE_A_NPC_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_NPC_01")]
		public static async Task FD_FTOWER611_TYPE_A_NPC_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_BG_AI_011")]
		public static async Task FD_FTOWER611_TYPE_A_BG_AI_011(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_BG_AI_012")]
		public static async Task FD_FTOWER611_TYPE_A_BG_AI_012(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_BG_AI_013")]
		public static async Task FD_FTOWER611_TYPE_A_BG_AI_013(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_BG_AI_014")]
		public static async Task FD_FTOWER611_TYPE_A_BG_AI_014(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER611_GROUP_1_1")]
		public static async Task FIRETOWER611_GROUP_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_BOARD_01")]
		public static async Task FD_FTOWER611_TYPE_A_BOARD_01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_A_BOARD_02")]
		public static async Task FD_FTOWER611_TYPE_A_BOARD_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER611_GROUP_1_2")]
		public static async Task FIRETOWER611_GROUP_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FTOWER611_TYPE_B_ROAMER_NPC")]
		public static async Task FD_FTOWER611_TYPE_B_ROAMER_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_FIRETOWER_61_1")]
		public static async Task WARP_D_FIRETOWER_61_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_61_11000")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_61_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_207", 1);

		[DialogFunction("JOB_MIKO_6_1_FTOWER_61_1")]
		public static async Task JOB_MIKO_6_1_FTOWER_61_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FIRETOWER612_T01_GATE_NOUTH_RUN_2")]
		public static async Task FD_FIRETOWER612_T01_GATE_NOUTH_RUN_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_FIRETOWER612_T01_GATE_SOUTH_RUN_2")]
		public static async Task FD_FIRETOWER612_T01_GATE_SOUTH_RUN_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_61_21000")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_61_21000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_208", 1);

		[DialogFunction("FIRETOWER691_TO_FIRETOWER692")]
		public static async Task FIRETOWER691_TO_FIRETOWER692(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_MQ_5_NPC")]
		public static async Task FIRETOWER691_MQ_5_NPC(Dialog dialog)
		{
			await dialog.Msg("FIRETOWER691_MQ_5_NPC_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_MQ_1_NPC")]
		public static async Task FIRETOWER691_MQ_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_SQ_1_NPC")]
		public static async Task FIRETOWER691_SQ_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_1_TO_FIRETOWER691_2")]
		public static async Task FIRETOWER691_1_TO_FIRETOWER691_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_3_TO_FIRETOWER691_4")]
		public static async Task FIRETOWER691_3_TO_FIRETOWER691_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_SQ_2_NPC")]
		public static async Task FIRETOWER691_SQ_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_SQ_3_NPC")]
		public static async Task FIRETOWER691_SQ_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_2_TO_FIRETOWER691_1")]
		public static async Task FIRETOWER691_2_TO_FIRETOWER691_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_4_TO_FIRETOWER691_3")]
		public static async Task FIRETOWER691_4_TO_FIRETOWER691_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FIRETOWER691_SQ_2_OBJ")]
		public static async Task FIRETOWER691_SQ_2_OBJ(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task FIRETOWER691_SQ_3_OBJ(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction("TREASUREBOX_LV_D_FIRETOWER_69_11000")]
		public static async Task TREASUREBOX_LV_D_FIRETOWER_69_11000(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_300", 1);

		[DialogFunction("TREASUREBOX_LV_F_KATYN_10649")]
		public static async Task TREASUREBOX_LV_F_KATYN_10649(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_232", 1);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_11_RE633")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_11_RE633(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_220", 1);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_11_RE634")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_11_RE634(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_259", 1);

		[DialogFunction("TREASUREBOX_LV_F_KATYN_1232")]
		public static async Task TREASUREBOX_LV_F_KATYN_1232(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "JOB_SHINOBI_HIDDEN_ITEM_2", 1);

		[DialogFunction("TREASUREBOX_LV_F_KATYN_12335")]
		public static async Task TREASUREBOX_LV_F_KATYN_12335(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_233", 1);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_15_RE643")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_15_RE643(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_219", 1);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_15_RE644")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_15_RE644(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_258", 1);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_16581")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_16581(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "BRC02_114", 1);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_16582")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_16582(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "NECK01_109", 1);

		[DialogFunction("TREASUREBOX_LV_F_SIAULIAI_16583")]
		public static async Task TREASUREBOX_LV_F_SIAULIAI_16583(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_218", 1);

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_31_1159")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_31_1159(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_241", 1);

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_31_2108")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_31_2108(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_243", 1);

		[DialogFunction("TREASUREBOX_LV_F_PILGRIMROAD_31_3123")]
		public static async Task TREASUREBOX_LV_F_PILGRIMROAD_31_3123(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_242", 1);

		[DialogFunction("TREASUREBOX_LV_F_3CMLAKE_83172")]
		public static async Task TREASUREBOX_LV_F_3CMLAKE_83172(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_234", 1);

		[DialogFunction("TREASUREBOX_LV_F_3CMLAKE_84183")]
		public static async Task TREASUREBOX_LV_F_3CMLAKE_84183(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_235", 1);

		[DialogFunction("TREASUREBOX_LV_F_3CMLAKE_8440")]
		public static async Task TREASUREBOX_LV_F_3CMLAKE_8440(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "Hat_628001", 1);

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_33_1248")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_33_1248(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_236", 1);

		[DialogFunction("TREASUREBOX_LV_ID_CATACOMB_33_2259")]
		public static async Task TREASUREBOX_LV_ID_CATACOMB_33_2259(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_237", 1);

		[DialogFunction("TREASUREBOX_LV_F_ORCHARD_32_3228")]
		public static async Task TREASUREBOX_LV_F_ORCHARD_32_3228(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_246", 1);

		[DialogFunction("TREASUREBOX_LV_F_ORCHARD_32_4319")]
		public static async Task TREASUREBOX_LV_F_ORCHARD_32_4319(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_248", 1);

		[DialogFunction("TREASUREBOX_LV_F_ORCHARD_34_2314")]
		public static async Task TREASUREBOX_LV_F_ORCHARD_34_2314(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_247", 1);

		[DialogFunction("TREASUREBOX_LV_D_PRISON_62_1270")]
		public static async Task TREASUREBOX_LV_D_PRISON_62_1270(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_221", 1);

		[DialogFunction("TREASUREBOX_LV_D_PRISON_62_2322")]
		public static async Task TREASUREBOX_LV_D_PRISON_62_2322(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_222", 1);

		[DialogFunction("TREASUREBOX_LV_D_PRISON_62_3245")]
		public static async Task TREASUREBOX_LV_D_PRISON_62_3245(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_223", 1);

		[DialogFunction("TREASUREBOX_LV_F_BRACKEN_63_1503")]
		public static async Task TREASUREBOX_LV_F_BRACKEN_63_1503(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_224", 1);

		[DialogFunction("TREASUREBOX_LV_F_BRACKEN_63_2493")]
		public static async Task TREASUREBOX_LV_F_BRACKEN_63_2493(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_225", 1);

		[DialogFunction("TREASUREBOX_LV_F_BRACKEN_63_3509")]
		public static async Task TREASUREBOX_LV_F_BRACKEN_63_3509(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_226", 1);

		[DialogFunction("TREASUREBOX_LV_D_ABBEY_64_1262")]
		public static async Task TREASUREBOX_LV_D_ABBEY_64_1262(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_229", 1);

		[DialogFunction("TREASUREBOX_LV_D_ABBEY_64_2660")]
		public static async Task TREASUREBOX_LV_D_ABBEY_64_2660(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_230", 1);

		[DialogFunction("TREASUREBOX_LV_D_ABBEY_64_3242")]
		public static async Task TREASUREBOX_LV_D_ABBEY_64_3242(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_231", 1);

		[DialogFunction("TREASUREBOX_LV_F_CASTLE_65_1202")]
		public static async Task TREASUREBOX_LV_F_CASTLE_65_1202(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_238", 1);

		[DialogFunction("TREASUREBOX_LV_F_CASTLE_65_2261")]
		public static async Task TREASUREBOX_LV_F_CASTLE_65_2261(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_239", 1);

		[DialogFunction("TREASUREBOX_LV_F_CASTLE_65_3441")]
		public static async Task TREASUREBOX_LV_F_CASTLE_65_3441(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_240", 1);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_66_1223")]
		public static async Task TREASUREBOX_LV_D_CMINE_66_1223(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_227", 1);

		[DialogFunction("TREASUREBOX_LV_D_CMINE_66_2292")]
		public static async Task TREASUREBOX_LV_D_CMINE_66_2292(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_228", 1);

		[DialogFunction("TREASUREBOX_LV_D_CASTLE_67_1231")]
		public static async Task TREASUREBOX_LV_D_CASTLE_67_1231(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_244", 1);

		[DialogFunction("TREASUREBOX_LV_D_CASTLE_67_2219")]
		public static async Task TREASUREBOX_LV_D_CASTLE_67_2219(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_245", 1);

		[DialogFunction("TREASUREBOX_LV_D_UNDERFORTRESS_68_1147")]
		public static async Task TREASUREBOX_LV_D_UNDERFORTRESS_68_1147(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_253", 1);

		[DialogFunction("TREASUREBOX_LV_D_UNDERFORTRESS_68_2161")]
		public static async Task TREASUREBOX_LV_D_UNDERFORTRESS_68_2161(Dialog dialog)
			=> await TREASUREBOX_LV(dialog, "COLLECT_254", 1);

		[DialogFunction("FTOWER691_KQ_1_NPC")]
		public static async Task FTOWER691_KQ_1_NPC(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction("FIRETOWER_69_2_FIRETOWER_69_1")]
		public static async Task FIRETOWER_69_2_FIRETOWER_69_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_FIRETOWER_69_2")]
		public static async Task WARP_D_FIRETOWER_69_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("FTOWER_69_2_G2_1_BOX")]
		public static async Task FTOWER_69_2_G2_1_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_G3_3_BOX")]
		public static async Task FTOWER_69_2_G3_3_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_G4_3_GAP")]
		public static async Task FTOWER_69_2_G4_3_GAP(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_WARP_IN_9")]
		public static async Task FTOWER_69_2_WARP_IN_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_WARP_OUT_9")]
		public static async Task FTOWER_69_2_WARP_OUT_9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_WARP_IN_6")]
		public static async Task FTOWER_69_2_WARP_IN_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_WARP_OUT_6")]
		public static async Task FTOWER_69_2_WARP_OUT_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_WARP_IN_3")]
		public static async Task FTOWER_69_2_WARP_IN_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FTOWER_69_2_WARP_OUT_3")]
		public static async Task FTOWER_69_2_WARP_OUT_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_SETPOS_1")]
		public static async Task D_UNDERAQUEDUCT_SETPOS_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_SETPOS_2")]
		public static async Task D_UNDERAQUEDUCT_SETPOS_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_SETPOS_3")]
		public static async Task D_UNDERAQUEDUCT_SETPOS_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_SETPOS_4")]
		public static async Task D_UNDERAQUEDUCT_SETPOS_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_EVENT_1")]
		public static async Task D_UNDERAQUEDUCT_EVENT_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_EVENT_2")]
		public static async Task D_UNDERAQUEDUCT_EVENT_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_BOSS_SETPOS")]
		public static async Task D_UNDERAQUEDUCT_BOSS_SETPOS(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_BOSS_SETPOS2")]
		public static async Task D_UNDERAQUEDUCT_BOSS_SETPOS2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_BOOK_1_NPC")]
		public static async Task D_UNDERAQUEDUCT_BOOK_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_BOOK_2_NPC")]
		public static async Task D_UNDERAQUEDUCT_BOOK_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_BOOK_3_NPC")]
		public static async Task D_UNDERAQUEDUCT_BOOK_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_BOOK_4_NPC")]
		public static async Task D_UNDERAQUEDUCT_BOOK_4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_BOOK_5_NPC")]
		public static async Task D_UNDERAQUEDUCT_BOOK_5_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_UNDERAQUEDUCT_TO_F_CASTLE_96")]
		public static async Task D_UNDERAQUEDUCT_TO_F_CASTLE_96(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_DCAPITAL53_1")]
		public static async Task WARP_DCAPITAL53_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("DCAPITAL53_1_SUB_OWL_STATUE01")]
		public static async Task DCAPITAL53_1_SUB_OWL_STATUE01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_OWL_STATUE02")]
		public static async Task DCAPITAL53_1_SUB_OWL_STATUE02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_OWL_STATUE03")]
		public static async Task DCAPITAL53_1_SUB_OWL_STATUE03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_OWL_STATUE04")]
		public static async Task DCAPITAL53_1_SUB_OWL_STATUE04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_OWL_STATUE05")]
		public static async Task DCAPITAL53_1_SUB_OWL_STATUE05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_MAIN_NPC01")]
		public static async Task DCAPITAL53_1_MAIN_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_MAIN_NPC02")]
		public static async Task DCAPITAL53_1_MAIN_NPC02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_MAIN_NPC03")]
		public static async Task DCAPITAL53_1_MAIN_NPC03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC01")]
		public static async Task DCAPITAL53_1_SUB_NPC01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC02")]
		public static async Task DCAPITAL53_1_SUB_NPC02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC03")]
		public static async Task DCAPITAL53_1_SUB_NPC03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC04")]
		public static async Task DCAPITAL53_1_SUB_NPC04(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC05")]
		public static async Task DCAPITAL53_1_SUB_NPC05(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC06")]
		public static async Task DCAPITAL53_1_SUB_NPC06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC07")]
		public static async Task DCAPITAL53_1_SUB_NPC07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC08")]
		public static async Task DCAPITAL53_1_SUB_NPC08(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC09")]
		public static async Task DCAPITAL53_1_SUB_NPC09(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_NPC10")]
		public static async Task DCAPITAL53_1_SUB_NPC10(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_MQ_04_OBJ")]
		public static async Task DCAPITAL53_1_MQ_04_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_OWL_STATUE06")]
		public static async Task DCAPITAL53_1_SUB_OWL_STATUE06(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("DCAPITAL53_1_SUB_OWL_STATUE07")]
		public static async Task DCAPITAL53_1_SUB_OWL_STATUE07(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("PRISON_75_1_TO_SIAULIAI_50_1")]
		public static async Task PRISON_75_1_TO_SIAULIAI_50_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER_76_1_TO_SIAULIAI_46_4")]
		public static async Task STARTOWER_76_1_TO_SIAULIAI_46_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_76_1_TO_D_STARTOWER_76_2")]
		public static async Task D_STARTOWER_76_1_TO_D_STARTOWER_76_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("D_STARTOWER_76_2_TO_D_STARTOWER_76_1")]
		public static async Task D_STARTOWER_76_2_TO_D_STARTOWER_76_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ1")]
		public static async Task STARTOWER762_EVENT1_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ2")]
		public static async Task STARTOWER762_EVENT1_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ3")]
		public static async Task STARTOWER762_EVENT1_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ4")]
		public static async Task STARTOWER762_EVENT1_OBJ4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ5")]
		public static async Task STARTOWER762_EVENT1_OBJ5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ6")]
		public static async Task STARTOWER762_EVENT1_OBJ6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ7")]
		public static async Task STARTOWER762_EVENT1_OBJ7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT1_OBJ8")]
		public static async Task STARTOWER762_EVENT1_OBJ8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT2_CLOUDYBEAD")]
		public static async Task STARTOWER762_EVENT2_CLOUDYBEAD(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT2_OBJ")]
		public static async Task STARTOWER762_EVENT2_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT2_REWARDBOX")]
		public static async Task STARTOWER762_EVENT2_REWARDBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT3_OBJECT")]
		public static async Task STARTOWER762_EVENT3_OBJECT(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("STARTOWER762_EVENT4_OBJ")]
		public static async Task STARTOWER762_EVENT4_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("FD_STARTOWER762_EV_55_001")]
		public static async Task FD_STARTOWER762_EV_55_001(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("VELNIASPRISON_77_1_TO_FLASH_61")]
		public static async Task VELNIASPRISON_77_1_TO_FLASH_61(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATHEDRAL_78_1_TO_TABLELAND_28_2")]
		public static async Task CATHEDRAL_78_1_TO_TABLELAND_28_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_80_1_CRYSTAL")]
		public static async Task CATACOMB_80_1_CRYSTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_80_1_TO_CATACOMB_80_2")]
		public static async Task CATACOMB_80_1_TO_CATACOMB_80_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_80_1_TO_KATYN_45_3")]
		public static async Task CATACOMB_80_1_TO_KATYN_45_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_80_2_TO_CATACOMB_80_1")]
		public static async Task CATACOMB_80_2_TO_CATACOMB_80_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_80_2_TO_CATACOMB_80_3")]
		public static async Task CATACOMB_80_2_TO_CATACOMB_80_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CATACOMB_80_3_TO_CATACOMB_80_2")]
		public static async Task CATACOMB_80_3_TO_CATACOMB_80_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("RVR_BK_REST1_TO_REST2")]
		public static async Task RVR_BK_REST1_TO_REST2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("RVR_BK_REST2_TO_REST1")]
		public static async Task RVR_BK_REST2_TO_REST1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AGARIO_DUNGEON_NPC")]
		public static async Task AGARIO_DUNGEON_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("AGARIO_MINE_DMG")]
		public static async Task AGARIO_MINE_DMG(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE102_AUSTEJA_02")]
		public static async Task CASTLE102_AUSTEJA_02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("CASTLE102_MQ_06_VIVORA")]
		public static async Task CASTLE102_MQ_06_VIVORA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("ANOTHER_SPACE_PORTAL")]
		public static async Task ANOTHER_SPACE_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_TO_NICO813")]
		public static async Task IRREDIANS1131_TO_NICO813(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_D_IRREDIANS113_1")]
		public static async Task WARP_D_IRREDIANS113_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("IRREDIANS1131_ENTRANCE")]
		public static async Task IRREDIANS1131_ENTRANCE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_ESCAPE_EAST")]
		public static async Task IRREDIANS1131_ESCAPE_EAST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_ESCAPE_WEST")]
		public static async Task IRREDIANS1131_ESCAPE_WEST(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_BOOK_1_NPC")]
		public static async Task IRREDIANS1131_BOOK_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_BOOK_2_NPC")]
		public static async Task IRREDIANS1131_BOOK_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_BOOK_3_NPC")]
		public static async Task IRREDIANS1131_BOOK_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_SCROLL_1_NPC")]
		public static async Task IRREDIANS1131_SCROLL_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_SCROLL_2_NPC")]
		public static async Task IRREDIANS1131_SCROLL_2_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_SCROLL_3_NPC")]
		public static async Task IRREDIANS1131_SCROLL_3_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIANS1131_SCROLL_4_NPC")]
		public static async Task IRREDIANS1131_SCROLL_4_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIAN1131_CLP_NPC")]
		public static async Task IRREDIAN1131_CLP_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("IRREDIAN1131_CRP_NPC")]
		public static async Task IRREDIAN1131_CRP_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("RAID_TELHARSHA_ENTER")]
		public static async Task RAID_TELHARSHA_ENTER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_3LINE_TUTO_MQ_2_F_PEOPLE_1")]
		public static async Task EP14_3LINE_TUTO_MQ_2_F_PEOPLE_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_3LINE_TUTO_MQ_2_M_PEOPLE_1")]
		public static async Task EP14_3LINE_TUTO_MQ_2_M_PEOPLE_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_3LINE_TUTO_MQ_2_M_PEOPLE_2")]
		public static async Task EP14_3LINE_TUTO_MQ_2_M_PEOPLE_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_HEOSHAA_NPC")]
		public static async Task EP14_HEOSHAA_NPC(Dialog dialog)
		{
			await dialog.Msg("EP14_HEOSHAA_NPC_Basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_RAMIN_NPC")]
		public static async Task EP14_RAMIN_NPC(Dialog dialog)
		{
			await dialog.Msg("EP14_RAMIN_NPC_Basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_KAONEELA_NPC")]
		public static async Task EP14_KAONEELA_NPC(Dialog dialog)
		{
			await dialog.Msg("EP14_KAONEELA_NPC_Basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_MULIA_NPC_1")]
		public static async Task EP14_MULIA_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_MULIA_NPC_2")]
		public static async Task EP14_MULIA_NPC_2(Dialog dialog)
		{
			await dialog.Msg("EP14_MULIA_NPC_2_basic01");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_OWYNIA_NPC")]
		public static async Task EP14_OWYNIA_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("TEST_GQ_GM_NPC")]
		public static async Task TEST_GQ_GM_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_TUTO_SKIP_NPC")]
		public static async Task EP14_TUTO_SKIP_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP14_3LINE_TUTO_MQ_9_1_NPC")]
		public static async Task EP14_3LINE_TUTO_MQ_9_1_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_NPC1")]
		public static async Task NICO811_NPC1(Dialog dialog)
		{
			await dialog.Msg("NICO811_NPC1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE1")]
		public static async Task NICO811_DEVICE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE2")]
		public static async Task NICO811_DEVICE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE3")]
		public static async Task NICO811_DEVICE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE4")]
		public static async Task NICO811_DEVICE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE5")]
		public static async Task NICO811_DEVICE5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE6")]
		public static async Task NICO811_DEVICE6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE7")]
		public static async Task NICO811_DEVICE7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE8")]
		public static async Task NICO811_DEVICE8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_DEVICE9")]
		public static async Task NICO811_DEVICE9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_NPC2")]
		public static async Task NICO811_NPC2(Dialog dialog)
		{
			await dialog.Msg("NICO811_NPC2_basic1");
			await dialog.Msg("NICO811_NPC2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_NPC3")]
		public static async Task NICO811_NPC3(Dialog dialog)
		{
			await dialog.Msg("NICO811_NPC3_basic1");
			await dialog.Msg("NICO811_NPC3_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO811_NPC1_2")]
		public static async Task NICO811_NPC1_2(Dialog dialog)
		{
			await dialog.Msg("NICO811_NPC1_2_basic1");
			await dialog.Msg("NICO811_NPC1_2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO_811_SUBQ022")]
		public static async Task NICO_811_SUBQ022(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO_811_SUBQ022_OBJ")]
		public static async Task NICO_811_SUBQ022_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICOPOLIS_811_SUBQ_NPC1")]
		public static async Task NICOPOLIS_811_SUBQ_NPC1(Dialog dialog)
		{
			await dialog.Msg("NICOPOLIS_811_SUBQ_NPC1_basic1");
			await dialog.Msg("NICOPOLIS_811_SUBQ_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICOPOLIS_811_SUBQ_NPC2")]
		public static async Task NICOPOLIS_811_SUBQ_NPC2(Dialog dialog)
		{
			await dialog.Msg("NICOPOLIS_811_SUBQ_NPC2_basic1");
			await dialog.Msg("NICOPOLIS_811_SUBQ_NPC2_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO_811_SUBQ0241_OBJ")]
		public static async Task NICO_811_SUBQ0241_OBJ(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("INSTANCE_DUNGEON_NICO811")]
		public static async Task INSTANCE_DUNGEON_NICO811(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC1")]
		public static async Task NICO812_SUBQ_NPC1(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC1_basic1");
			await dialog.Msg("NICO812_SUBQ_NPC1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC2")]
		public static async Task NICO812_SUBQ_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC3")]
		public static async Task NICO812_SUBQ_NPC3(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC4")]
		public static async Task NICO812_SUBQ_NPC4(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC4_basic1");
			await dialog.Msg("NICO812_SUBQ_NPC4_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC5")]
		public static async Task NICO812_SUBQ_NPC5(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC5_basic1");
			await dialog.Msg("NICO812_SUBQ_NPC5_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC6")]
		public static async Task NICO812_SUBQ_NPC6(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC6_basic1");
			await dialog.Msg("NICO812_SUBQ_NPC6_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC7")]
		public static async Task NICO812_SUBQ_NPC7(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC7_basic1");
			await dialog.Msg("NICO812_SUBQ_NPC7_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC8")]
		public static async Task NICO812_SUBQ_NPC8(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC8_basic1");
			await dialog.Msg("NICO812_SUBQ_NPC8_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC9")]
		public static async Task NICO812_SUBQ_NPC9(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUB5_NPC")]
		public static async Task NICO812_SUB5_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUB10_NPC1")]
		public static async Task NICO812_SUB10_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUB10_NPC2")]
		public static async Task NICO812_SUB10_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUB10_NPC3")]
		public static async Task NICO812_SUB10_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUBQ_NPC10")]
		public static async Task NICO812_SUBQ_NPC10(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUBQ_NPC10_basic1");
			await dialog.Msg("NICO812_SUBQ_NPC10_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO812_SUB12_NPC1")]
		public static async Task NICO812_SUB12_NPC1(Dialog dialog)
		{
			await dialog.Msg("NICO812_SUB12_NPC1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC1_1")]
		public static async Task NICO813_SUBQ_NPC1_1(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC1_1_basic1");
			await dialog.Msg("NICO813_SUBQ_NPC1_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC3_1")]
		public static async Task NICO813_SUBQ_NPC3_1(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC3_1_basic1");
			await dialog.Msg("NICO813_SUBQ_NPC3_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC2_1")]
		public static async Task NICO813_SUBQ_NPC2_1(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC2_1_basic1");
			await dialog.Msg("NICO813_SUBQ_NPC2_1_basic2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC1_2")]
		public static async Task NICO813_SUBQ_NPC1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC2_2")]
		public static async Task NICO813_SUBQ_NPC2_2(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC2_2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC1_3")]
		public static async Task NICO813_SUBQ_NPC1_3(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC1_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC3_2")]
		public static async Task NICO813_SUBQ_NPC3_2(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC3_2_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_SEAL1")]
		public static async Task NICO813_SUBQ_SEAL1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_SEAL2")]
		public static async Task NICO813_SUBQ_SEAL2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_SEAL3")]
		public static async Task NICO813_SUBQ_SEAL3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_SEAL4")]
		public static async Task NICO813_SUBQ_SEAL4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_SEAL5")]
		public static async Task NICO813_SUBQ_SEAL5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_SEAL6")]
		public static async Task NICO813_SUBQ_SEAL6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_SEAL7")]
		public static async Task NICO813_SUBQ_SEAL7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ043")]
		public static async Task NICO813_SUBQ043(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ042_OBJ1")]
		public static async Task NICO813_SUBQ042_OBJ1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ042_OBJ2")]
		public static async Task NICO813_SUBQ042_OBJ2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ042_OBJ3")]
		public static async Task NICO813_SUBQ042_OBJ3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ042_OBJ4")]
		public static async Task NICO813_SUBQ042_OBJ4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC2_3")]
		public static async Task NICO813_SUBQ_NPC2_3(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC2_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC1_4")]
		public static async Task NICO813_SUBQ_NPC1_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_SUBQ_NPC3_3")]
		public static async Task NICO813_SUBQ_NPC3_3(Dialog dialog)
		{
			await dialog.Msg("NICO813_SUBQ_NPC3_3_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("NICO813_TO_IRREDIANS1131")]
		public static async Task NICO813_TO_IRREDIANS1131(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_1_MQ_LADA_1")]
		public static async Task EP13_F_SIAULIAI_1_MQ_LADA_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("SIAU16_SQ_06_EV_NPC")]
		public static async Task SIAU16_SQ_06_EV_NPC(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction("EP13_F_SIAULIAI_1_MQ_LADA_2")]
		public static async Task EP13_F_SIAULIAI_1_MQ_LADA_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_1_MQ_LADA_3")]
		public static async Task EP13_F_SIAULIAI_1_MQ_LADA_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_1_MQ_06_CRACK")]
		public static async Task EP13_F_SIAULIAI_1_MQ_06_CRACK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_1_REPUTATION_01")]
		public static async Task EP13_F_SIAULIAI_1_REPUTATION_01(Dialog dialog)
		{
			await dialog.Msg("REPUTATION_NPC_BASIC1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_SUB_RECONSTRUCTION_NPC_SIAU_1_1")]
		public static async Task EP13_SUB_RECONSTRUCTION_NPC_SIAU_1_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_SUB_RECONSTRUCTION_NPC_SIAU_1_2")]
		public static async Task EP13_SUB_RECONSTRUCTION_NPC_SIAU_1_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_2_MQ_LADA_1")]
		public static async Task EP13_F_SIAULIAI_2_MQ_LADA_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_2_MQ_03_CRACK")]
		public static async Task EP13_F_SIAULIAI_2_MQ_03_CRACK(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_2_MQ_LADA_2")]
		public static async Task EP13_F_SIAULIAI_2_MQ_LADA_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_2_MQ_04_BLACKCRYSTAL")]
		public static async Task EP13_F_SIAULIAI_2_MQ_04_BLACKCRYSTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_2_MQ_06_BLACKCRYSTAL")]
		public static async Task EP13_F_SIAULIAI_2_MQ_06_BLACKCRYSTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_2_MQ_LADA_3")]
		public static async Task EP13_F_SIAULIAI_2_MQ_LADA_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_EP13_F_SIAULIAI_2")]
		public static async Task WARP_EP13_F_SIAULIAI_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("EP13_F_SIAULIAI_2_REPUTATION_01")]
		public static async Task EP13_F_SIAULIAI_2_REPUTATION_01(Dialog dialog)
		{
			await dialog.Msg("REPUTATION_NPC_BASIC2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_SUB_RECONSTRUCTION_NPC_SIAU_2_1")]
		public static async Task EP13_SUB_RECONSTRUCTION_NPC_SIAU_2_1(Dialog dialog)
		{
			await dialog.Msg("EP13_F_SIAULIAI_2_SQ_01_1_2");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_SUB_RECONSTRUCTION_NPC_SIAU_2_2")]
		public static async Task EP13_SUB_RECONSTRUCTION_NPC_SIAU_2_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("GODDESS_RAID_VASILISSA_PORTAL")]
		public static async Task GODDESS_RAID_VASILISSA_PORTAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("MASTER_ARBALESTER_NPC")]
		public static async Task MASTER_ARBALESTER_NPC(Dialog dialog)
		{
			await dialog.Msg("MASTER_ARBALESTER_NPC_basic1");
			await dialog.Msg("MASTER_ARBALESTER_NPC_basic_move");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_LADA_1")]
		public static async Task EP13_F_SIAULIAI_3_MQ_LADA_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_LADA_2")]
		public static async Task EP13_F_SIAULIAI_3_MQ_LADA_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_BAIGA_1")]
		public static async Task EP13_F_SIAULIAI_3_MQ_BAIGA_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_LADA_3")]
		public static async Task EP13_F_SIAULIAI_3_MQ_LADA_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_06_CRACK_1")]
		public static async Task EP13_F_SIAULIAI_3_MQ_06_CRACK_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_06_CRACK_2")]
		public static async Task EP13_F_SIAULIAI_3_MQ_06_CRACK_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_06_CRACK_3")]
		public static async Task EP13_F_SIAULIAI_3_MQ_06_CRACK_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_06_CRACK_4")]
		public static async Task EP13_F_SIAULIAI_3_MQ_06_CRACK_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_06_CRACK_5")]
		public static async Task EP13_F_SIAULIAI_3_MQ_06_CRACK_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_07_CRACK_1")]
		public static async Task EP13_F_SIAULIAI_3_MQ_07_CRACK_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_MQ_LADA_4")]
		public static async Task EP13_F_SIAULIAI_3_MQ_LADA_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_REPUTATION_01")]
		public static async Task EP13_F_SIAULIAI_3_REPUTATION_01(Dialog dialog)
		{
			await dialog.Msg("REPUTATION_NPC_BASIC3");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_SUB_RECONSTRUCTION_NPC_SIAU_3_1")]
		public static async Task EP13_SUB_RECONSTRUCTION_NPC_SIAU_3_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_SQ_02_CITIZEN_1")]
		public static async Task EP13_F_SIAULIAI_3_SQ_02_CITIZEN_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_SQ_02_CITIZEN_2")]
		public static async Task EP13_F_SIAULIAI_3_SQ_02_CITIZEN_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_SQ_02_CITIZEN_3")]
		public static async Task EP13_F_SIAULIAI_3_SQ_02_CITIZEN_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_SQ_02_CITIZEN_4")]
		public static async Task EP13_F_SIAULIAI_3_SQ_02_CITIZEN_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_3_SQ_02_CITIZEN_5")]
		public static async Task EP13_F_SIAULIAI_3_SQ_02_CITIZEN_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_1")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_4_MQ04_WAY01")]
		public static async Task EP13_F_SIAULIAI_4_MQ04_WAY01(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_4_MQ04_WAY02")]
		public static async Task EP13_F_SIAULIAI_4_MQ04_WAY02(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_4_MQ04_WAY03")]
		public static async Task EP13_F_SIAULIAI_4_MQ04_WAY03(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_GODDESS_LADA_ISA")]
		public static async Task EP13_GODDESS_LADA_ISA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_4_MQ_05_MAIL")]
		public static async Task EP13_F_SIAULIAI_4_MQ_05_MAIL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_4_MQ_06_MAIL")]
		public static async Task EP13_F_SIAULIAI_4_MQ_06_MAIL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("WARP_EP13_F_SIAULIAI_4")]
		public static async Task WARP_EP13_F_SIAULIAI_4(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction("EP13_F_SIAULIAI_4_ZEMINA")]
		public static async Task EP13_F_SIAULIAI_4_ZEMINA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_MQ08_BOX")]
		public static async Task EP13_F_SIAULIAI_MQ08_BOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_4_REPUTATION_01")]
		public static async Task EP13_F_SIAULIAI_4_REPUTATION_01(Dialog dialog)
		{
			await dialog.Msg("REPUTATION_NPC_BASIC4");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP15_F_ABBEY2_6_NPC")]
		public static async Task EP15_F_ABBEY2_6_NPC(Dialog dialog)
		{
			await dialog.Msg("EP15_1_Pamaster_basic");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_5_RAGANA")]
		public static async Task EP13_F_SIAULIAI_5_RAGANA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_5_BAIGA")]
		public static async Task EP13_F_SIAULIAI_5_BAIGA(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_F_SIAULIAI_5_REPUTATION_01")]
		public static async Task EP13_F_SIAULIAI_5_REPUTATION_01(Dialog dialog)
		{
			await dialog.Msg("REPUTATION_NPC_BASIC5");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task UNKNOWN_SANTUARY_GATE_NORMAL(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_10(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task WARP_EP13_2_D_PRISON_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP13_2_DPRISON2_MQ_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON2_MQ_NPC_BOOKSHELF(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON2_MQ_NPC_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON2_MQ_NPC_LOSTARTICLE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON2_MQ_NPC_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON2_MQ_STORAGEBOX(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_1")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_2")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_3")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_4")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_5")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_6")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_7")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_8")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_SOULFRAGMENT_8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_5")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON2_MQ_NPC_6")]
		public static async Task EP13_2_DPRISON2_MQ_NPC_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("UNKNOWN_SANTUARY_GATE_DUNGEON")]
		public static async Task UNKNOWN_SANTUARY_GATE_DUNGEON(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_2204_CAMPING_JUMP_CHECK_NPC")]
		public static async Task EVENT_2204_CAMPING_JUMP_CHECK_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_2204_CAMPING_JUMP_BOARD_NPC")]
		public static async Task EVENT_2204_CAMPING_JUMP_BOARD_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_2204_CAMPING_JUMP_RETURN_NPC")]
		public static async Task EVENT_2204_CAMPING_JUMP_RETURN_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EVENT_2204_CAMPING_JUMP_HIDDEN_NPC")]
		public static async Task EVENT_2204_CAMPING_JUMP_HIDDEN_NPC(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON3_MQ_NPC_1")]
		public static async Task EP13_2_DPRISON3_MQ_NPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction("EP13_2_DPRISON3_MQ_NPC_2")]
		public static async Task EP13_2_DPRISON3_MQ_NPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_DUMMY(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R7(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_R8(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L7(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_SPOT_L8(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_6(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_5(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_F_CASTLE_1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE1_MQ_2_SOLDIER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE1_MQ_3_SOLDIER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_F_CASTLE_1_NPC3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task D_CASTLE_19_1_PORTAL_NPC_01(Dialog dialog)
		{
			await dialog.Msg("D_CASTLE_19_1_PORTAL_NPC_basic1");
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_1_NPC1(Dialog dialog)
		{
			await dialog.Msg("EP14_1_FCASTLE2_MQ_1_NPC1_IDLE_DLG1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_2_DUMMYNPC_1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_2_DUMMYNPC_2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_BEFORE1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_BEFORE2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_BEFORE3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_BEFORE4(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_4_SOLDIER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_5_SOLDIER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_6_SOLDIER(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE2_MQ_7_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_F_CASTLE_2_06_EV_NPC(Dialog dialog)
			=> await ZEMINA_STATUE(dialog);

		[DialogFunction]
		public static async Task WARP_EP14_1_F_CASTLE_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_1_NPC1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_3_TRACE(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_4_TENT1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_4_TENT2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_4_TENT3(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_4_TENT4(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_7_TENT1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_7_TENT2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_7_TENT3(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_7_TENT4(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_7_TENT5(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE3_MQ_7_TENT6(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_2_NPC1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_3_NPC1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_3_NPC2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_PAJAUTA_1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_6_NPC1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_7_BLACKCRYSTAL1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_7_BLACKCRACK(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_8_NPC1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE4_MQ_8_NPC2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task WARP_EP14_1_F_CASTLE_4(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP14_1_FCASTLE5_MQ_1_NPC1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE5_MQ_1_NPC2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE5_MQ_1_CART1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE5_MQ_1_CART2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE5_MQ_1_FLAG1(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE5_MQ_8_NPC1(Dialog dialog)
		{
			await dialog.Msg("EP14_1_FCASTLE5_MQ_8_NPC1_IDLE_DLG1");
		}

		[DialogFunction]
		public static async Task EP14_1_FCASTLE5_MQ_8_NPC2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_DELMORE_PORTAL(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_2_PORTAL2(Dialog dialog)
		{
		}

		[DialogFunction]
		public static async Task EP14_2_PORTAL1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_1_Lamin1(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_Lamin2_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_1_PAJAUTA1(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_PAJAUTA1_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_1_Lamin2(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_Lamin2_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_1_PAJAUTA2(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_PAJAUTA1_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task WARP_EP14_2_DCASTLE_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP14_2_RED_CRYSTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE1_WALL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE_EXIT(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE_EXIT2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task WARP_EP14_2_DCASTLE_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP14_2_DCASLTE2_ZEMINA(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASLTE2_PORTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_2_LAMIN1(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_Lamin2_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_2_PAJAUTA1(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_PAJAUTA1_basic1");
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE2_MANA1(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE2_MANA2(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE2_MANA3(Dialog dialog)
		{
			await COMMON_QUEST_HANDLER(dialog);
		}

		[DialogFunction]
		public static async Task EP14_2_2_PAJAUTA2(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_PAJAUTA1_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_2_LAMIN2(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_Lamin2_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE2_MANA4(Dialog dialog)
		{
			await dialog.Msg("EP14_2_DCASTLE2_MANA4_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE2_EXIT2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE2_EXIT(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task BAUBAS_GO_GUILDMISSION(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASLTE3_PORTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE3_PAJAUTA(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_PAJAUTA1_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE3_RAMIN(Dialog dialog)
		{
			await dialog.Msg("EP14_2_1_Lamin2_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_TRPLEHELIX1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_TRPLEHELIX2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_TRPLEHELIX3(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE3_WALL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task RAID_CASTLE_EP14_2_PORTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task RAID_CASTLE_EP14_2_SOLD(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE3_RAMIN2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE3_EXIT2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP14_2_DCASTLE3_EXIT(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task RAID_CASTLE_EP14_2_FSOLD(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_AD1(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_AD_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_ROZE1(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_Roze_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task WARP_EP15_1_F_ABBEY_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP15_1_RED_PLANET(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_ROZE2(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_Roze_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_AD2(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_AD_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_AD3(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_AD_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_ROZE3(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_Roze_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_ROZE4(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_Roze_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY1_AD4(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_AD_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_ABBEY1_SQ1_NPC(Dialog dialog)
		{
			await dialog.Msg("EP15_1_ABBEY1_SQ1_basic");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_GLOVECUPSULE(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task WARP_EP15_1_F_ABBEY_2(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_ZEMINA(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_AD1(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_AD_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_ROZE1(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_Roze_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY2_2_CART(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_MQ4_BOWER01(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_MQ4_BOWER02(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_MQ4_BOWER03(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_MQ4_BOWER04(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_ROZE2(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_Roze_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY2_AD2(Dialog dialog)
		{
			await dialog.Msg("EP15_1_F_ABBEY_AD_basic1");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_ABBEY2_SQ3_NPC(Dialog dialog)
		{
			await dialog.Msg("EP15_1_ABBEY2_SQ3_basic");
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_ABBEY2_SQ1_NPC(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_2_PIP(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_ROZE_PORTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY3_AD1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_FABBEY3_BLOCK(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY3_3_STONE(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY3_4_STONE(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY3_AD2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task RAID_EP15_1_NPC(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY_3_SQ1_BOOK1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY_3_SQ1_BOOK2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_1_F_ABBEY_3_SQ2_BOOK1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_DNICOPOLIS_PORTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICO_AUSIRINE_1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task WARP_EP15_2_D_NICOPOLIS_1(Dialog dialog)
			=> await STATUE_WARP(dialog);

		[DialogFunction]
		public static async Task EP15_2_D_NICO_AUSIRINE_2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_MQ_4_CRYSTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICO_AUSIRINE_3(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_MQ_6_CRYSTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task GODDESS_EP15_2_RAID_PORTAL2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_DNICOPOLIS_2_PORTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICO_2_AUSIRINE_1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICO_2_PORTAL_1(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICO_2_PORTAL_2(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task GODDESS_EP15_2_RAID_PORTAL(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_6_END(Dialog dialog)
		{
			await dialog.HooksByDialogName("BeforeStart");
			await dialog.HooksByDialogName("BeforeEnd");
		}

		[DialogFunction]
		public static async Task MON_DUMMY(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task NPC_HIDEMONBOX(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task NPC_FLAMEGOD1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task DIBSKILL_ZONEMOVE(Dialog dialog)
		{
			var npc = dialog.Npc;

			if (npc == null)
				return;

			var warpCount = npc.Vars.GetInt("WarpCount");
			if (warpCount > 0)
			{
				warpCount -= 1;
				npc.Vars.SetInt("WarpCount", warpCount);
				await dialog.ExecuteScript("INTE_WARP_OPEN_DIB()");
			}

			if (warpCount <= 0)
				npc.DisappearTime = DateTime.Now;
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task MANDRAGORA(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task UDUMBARA(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task APPRAISER_FORGERY(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task Well_Of_Life(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BLANK_NPC(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task RED_CRYSTAL(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task VELCOFFER_ROOM_WARP_POTAL(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task UNIQ_STARTTOWER_STAGE_3_2_SETPOS(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task UNIQ_STARTTOWER_STAGE_5_2_SETPOS(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GLACIER_TUTO_RP_NPC_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task TREEDAY_TREE1(Dialog dialog)
		{
			switch (await dialog.Select("EVENT_TREEDAY_TALK_DLG", "!@#$EVENT_COMMON_MSG1#@!", "!@#$EVENT_TREEDAY_GROW#@!", "!@#$EVENT_TREEDAY_CHECK_EXP#@!", "!@#$EVENT_TREEDAY_GUILD_RANK#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					await dialog.Msg("EVENT_TREEDAY_DLG1");
					break;
				case 2:
					dialog.OpenAddon(AddonMessage.NOTICE_Dm_Exclaimation, "!@#${item}NotExistMaterial$*$item$*$@dicID_^*$QUEST_20230410_014651$*^#@!");
					break;
				case 3:
					dialog.OpenAddon(AddonMessage.NOTICE_Dm_Exclaimation, "!@#${ratio}event_treeday_exp$*$ratio$*$56#@!", 3);
					break;
			}
			await COMMON_QUEST_HANDLER(dialog);
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task TREEDAY_TREE2(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task TREEDAY_TREE3(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task TREEDAY_TREE4(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task TREEDAY_TREE5(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task TREEDAY_TREE6(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task TREEDAY_TREE7(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_PRODUCTION(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PVP_MINE_SHOP(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GOLD_FISH_STATUE(Dialog dialog)
		{
			var character = dialog.Player;
			var npc = dialog.Npc;
			var requiredSilver = 56000;
			switch (await dialog.Select("FISHING_STATUE_DLG1", "!@#$FISHING_STATUE_WORSHIP$*$needSilver$*$56,000#@!", "!@#$Close#@!"))
			{
				case 1:
					if (!character.HasItem(ItemId.Silver, requiredSilver))
					{
						character.SystemMessage("Auto_SilBeoKa_BuJogHapNiDa.");
						return;
					}
					character.StartBuff(BuffId.STATUE_LOOTINGCHANCE, 50, 0, TimeSpan.FromMinutes(30), npc);
					break;
			}
		}

		[DialogFunction]
		public static async Task F_TABLELAND_28_2_RAID_07_NPC_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task DELMORECASTLEGATE_KRUVINABULLET(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_FARMER_NORMAL(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task EVENT_2001_NEWYEAR_POT(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task NPC_CONTENTS_MULTIPLE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task AUTO_CHALLENGE_END(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task MULTIPLE_REWARD_BY_MGAME(Dialog dialog)
		{
			await Task.Yield();
		}

		//AddNpc(0, 160065, "", "0", 1269.11, 602.46, 1503.88, 90, "MGAME_END_PORTAL");
		[DialogFunction]
		public static async Task MGAME_END_PORTAL(Dialog dialog)
		{
			switch (await dialog.Select(@"MGame_EndPortal_Msg", @"!@#$Auto_JongLyo#@!", @"!@#$Event_Steam_YC_2#@!", @"!@#$Event_Steam_YC_3#@!", @"!@#$Event_Steam_YC_4#@!"))
			{
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				case 4:
					break;
				case 5:
					break;
			}
		}

		[DialogFunction]
		public static async Task SOLO_DUNGEON_EXIT(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task VASILISSA_SPEAR_GET_ENTER(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task AETHER_GEM_REINFORCE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task EARRING_RAID_SET_READY(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_JELLYZELE_TWINKLE_OBJ(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task SPREADER_TRANSITION_CRYSTAL_FRAGMENT(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_SPREADER_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_FALOUROS_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_BAUBAS_RAID(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_ROZE_KUPOLE_LIGHT(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_ROZE_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BRIDGE_WAILING_FIRST_SECTION(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BRIDGE_WAILING_SECOND_SECTION(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BRIDGE_WAILING_THIRD_SECTION(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_UPINIS_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_SLOGUTIS_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task EVENT_MOON_RABBIT_START_NPC(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BRIDGE_WAILING_DIMENSION_FIRST_STEP(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BRIDGE_WAILING_DIMENSION_SECOND_STEP(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BRIDGE_WAILING_DIMENSION_THIRD_STEP(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_MERREGINA_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_MERREGINA_MAP_PATTERN(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task SEASON_CHALLENGE_START_NPC(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task SEASON_CHALLENGE_END(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task SEASON_EARRING_RAID_START_NPC(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_NERINGA_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GODDESS_RAID_CRYSTALGOLEM_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task SEASON_COLLAPSING_MINE_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task DEMONLAIR_ASHARK_START(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task DEMONLAIR_ASHARK_AREA_NPC(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task UNIQUE_RAID_NICOPOLIS_STAGE_STAGE_03_OPEN_ANIM(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task UNIQUE_RAID_NICOPOLIS_STAGE_04_MANABALL(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task UNIQUE_RAID_NICOPOLIS_STAGE_05_OBJ(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task UNIQUE_RAID_NICOPOLIS_BOSS_STAGE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task DesperateDefense(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task OBJ_TEST(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task Interaction_orsha_catapult(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task Interaction_ballista(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task Interaction_ballista_extreme(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task OBJ_RIDE_TEST(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task INTERACTION_RIDE_GRESMERAVEN(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task MYTHIC_PORTAL(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task EXCHANGE_PVP_MINE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task EVENT_1910_FIREBALL(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task EVENT_1910_SCARECROW(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_BENCH(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_LAMP(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_LANTERN(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_BIG_TREE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_GRAMOPHONE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_PARASOL(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_FLOWER_TABLE_SET(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_ARMOR_LAB(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_ATTRIBUTE_LAB(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_WEAPON_LAB(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_BAKARINE_STATUS(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_FOREST_POND_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_FOREST_BEAR_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_FOREST_BENCH_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_FOREST_BENCH_02(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_FOREST_HAMMOCK_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_FOREST_TABLE_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_HOUSING_OBJ_DUNGEON(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_06_CHAIR_1(Dialog dialog)
		{
			var character = dialog.Player;
			if (!character.IsPaired)
			{
				character.IsPaired = true;
				Send.ZC_ENABLE_CONTROL(character.Connection, "MOVE_PC", false);
				character.Position = dialog.Npc.Position;
				character.Direction = dialog.Npc.Direction;
				Send.ZC_SET_POS(character);
				Send.ZC_FLY(character);
				Send.ZC_PLAY_PAIR_ANIMATION(character, AnimationName.BarrackSit);
			}
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_06_DESK_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_06_FISHTANK_01(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_06_FISHTANK_01_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_04_CHAIR_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_03_BEARTOY_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERONAL_HOUSING_OBJ_CANDY_GACHA(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_VILLAGE_CHAIR_01_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_VILLAGE_SOFA_01_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_02_CHAIR_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_08_SOFA_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_07_ALLIGATOR_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_09_CHAIR_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_02_CHAIR_2(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_02_CHAIR_3(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_02_CHAIR_1_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_03_DESK_BOOK_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_03_BEARTOYDESK_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_02_CHAIR_4(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_11_CHAIR_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_15_CHAIR_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_12_LAMP_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_12_PHONE_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BARRACK_12_CLOCK_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_12_CHAIR_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_12_RECORD_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_12_SANDGLASS_1(Dialog dialog)
		{
			var player = dialog.Player;
			var npc = dialog.Npc;

			switch (await dialog.Select("PERSONAL_hp_barrack_12_sandglass_1\\!@#$Personal_Housing_hp_barrack_12_sandglass_MSG_1$*$TGTNAME$*$@dicID_^*$ETC_20210809_060159$*^$*$STATE$*$!@#$Personal_Housing_Active_Off_2#@!#@!", "!@#$Personal_Housing_Active_On_1#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					//Send.ZC_PLAY_SOUND(player, 1531);
					//Send.ZC_PLAY_ANI(player, 12720, false, 253);
					break;
				default:
					await COMMON_QUEST_HANDLER(dialog);
					break;
			}
			switch (await dialog.Select("PERSONAL_hp_barrack_12_sandglass_1\\!@#$Personal_Housing_hp_barrack_12_sandglass_MSG_1$*$TGTNAME$*$@dicID_^*$ETC_20210809_060161$*^$*$STATE$*$!@#$Personal_Housing_Active_On_2#@!#@!", "!@#$Personal_Housing_Active_Off_1#@!", "!@#$Auto_JongLyo#@!"))
			{
				case 1:
					//Send.ZC_PLAY_SOUND(player, 1532);
					//Send.ZC_PLAY_ANI(player, 12720, false, 253);
					Send.ZC_NORMAL.ClearEffects(dialog.Npc);
					break;
			}
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_12_DESK_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BARRACK_12_TABLELAMP_1(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_EXHIBITION_STAND(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task PERSONAL_HOUSING_OBJ_BOSS_FIGURE(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task BOUNTYHUNT_NPC(Dialog dialog)
		{
			await Task.Yield();
		}

		[DialogFunction]
		public static async Task GUILD_COLONY_TOWER_DIRECTION(Dialog dialog)
		{
			var npc = dialog.Npc;
			var character = dialog.Player;

			var sessionObject = character.SessionObjects.GetOrCreate("SSN_EV_STOP");
			Send.ZC_SESSION_OBJ_ADD(character, sessionObject);
			Send.ZC_CANCEL_MOUSE_MOVE(character);
			var result = await dialog.TimeAction(ScpArgMsg("ColonyOccupied"), "ABSORB", TimeSpan.FromSeconds(5));
			if (result == TimeActionResult.Completed)
			{
				if (sessionObject != null)
				{
					character.RemoveSessionObject(sessionObject.Id);
				}
				}
			await Task.Yield();
		}

		[DialogFunction("WARP_F_SIAULIAI_11RE")]
		[DialogFunction("WARP_F_SIAULIAI_15RE")]
		[DialogFunction("WARP_F_SIAULIAI_16")]
		[DialogFunction("WARP_F_ORCHARD_32_3")]
		[DialogFunction("WARP_F_KATYN_12")]
		[DialogFunction("WARP_F_PILGRIMROAD_31_3")]
		[DialogFunction("WARP_ID_CATACOMB_33_1")]
		[DialogFunction("WARP_D_PRISON_62_1")]
		[DialogFunction("WARP_D_PRISON_62_2")]
		[DialogFunction("WARP_D_PRISON_62_3")]
		[DialogFunction("WARP_F_BRACKEN_63_2")]
		[DialogFunction("WARP_D_ABBEY_64_1")]
		[DialogFunction("WARP_D_ABBEY_64_3")]
		[DialogFunction("WARP_D_CASTLE_67_1")]
		[DialogFunction("WARP_D_UNDERFORTRESS_68_1")]
		public static async Task WARP(Dialog dialog)
			=> await STATUE_WARP(dialog);
	}
}
