using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.L10N;
using Melia.Shared.Network;
using Melia.Shared.Network.Inter.Messages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Extensions.Keywords;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
// using Melia.Zone.World.Houses; // Removed: Houses namespace deleted
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Spawning;
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Network.Communication;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.Commands
{
	/// <summary>
	/// The chat command manager, holding the commands and executing them.
	/// </summary>
	public partial class ChatCommands : CommandManager<ChatCommand, ChatCommandFunc>
	{
		/// <summary>
		/// Creates new manager and initializes it.
		/// </summary>
		public ChatCommands()
		{
			// The required authority levels for commands can be specified
			// in the configuration file "conf/commands.conf".

			// Client Commands
			this.Add("requpdateequip", "", "", this.HandleReqUpdateEquip);
			this.Add("readcollection", "", "", this.HandleReadCollection);
			this.Add("buyabilpoint", "<amount>", "", this.HandleBuyAbilPoint);
			this.Add("hairgacha", "", "", this.HandleHairGacha);
			this.Add("guildexpup", "", "", this.HandleGuildExpUp);
			this.Add("intewarp", "<warp id> 0", "", this.HandleInteWarp);
			this.Add("intewarpByToken", "<destination>", "", this.HandleTokenWarp);
			this.Add("mic", "<message>", "", this.HandleMic);

			// Client Party Commands
			this.Add("memberinfoForAct", "<team name>", "", this.HandleMemberInfoForAct);
			this.Add("partyleader", "<team name>", "", this.HandlePartyLeader);
			this.Add("partymake", "<party name>", "", this.HandlePartyMake);
			this.Add("partyname", "0 0 <account id> <party name>", "", this.HandlePartyName);
			this.Add("partyDirectInvite", "<team name>", "", this.HandlePartyInvite);
			this.Add("partyban", "0 <team name>", "", this.HandlePartyBan);
			this.Add("pmyp", "0 <team name> <type> <quest id>", "", this.HandlePartyMemberProperty);

			this.Add("guild", "<team name>", "", this.HandleGuildInvite);
			this.Add("outguildcheck", "", "", this.HandleLeaveGuildCheck);
			this.Add("outguildbyweb", "", "", this.HandleLeaveGuildByWeb);

			this.Add("pethire", "", "", this.HandlePetHire);
			this.Add("petstat", "", "", this.HandlePetStat);
			this.Add("readcollection", "id", "", this.HandleReadCollection);
			this.Add("retquest", "<quest id>", "", this.HandleReturnToQuestGiver);

			// Skills
			this.Add("sageSavePos", "", "", this.HandleSageSavePosition);
			this.Add("sageDelPos", "", "", this.HandleSageDeletePosition);
			this.Add("sageOpenPortal", "", "", this.HandleSageOpenPortal);
			this.Add("hmunclusSkl", "", "", this.HandleHomunculusSkill);

			// Custom Client Commands
			this.Add("buyshop", "", "", this.HandleBuyShop);
			this.Add("sellshop", "", "", this.HandleSellShop);
			this.Add("autotrade", "", "", this.HandleAutoTrade);
			this.Add("updatemouse", "", "", this.HandleUpdateMouse);
			this.Add("cardbattle", "", "", this.HandleCardBattle);
			this.Add("memo", "", "", this.HandleMemoPortal);
			this.Add("portal", "", "", this.HandleOpenPortal);

			// Normal
			this.Add("where", "", "Displays current location.", this.HandleWhere);
			this.Add("distance", "", "Calculates distance between two positions.", this.HandleDistance);
			this.Add("time", "", "Displays the current server and game time.", this.HandleTime);
			this.Add("main", "", "Displays invite for global main chat.", this.HandleMainChat);
			this.Add("help", "[command]", "Displays available commands or information about a certain command.", this.HandleHelp);
			this.Add("iteminfo", "<name>", "Displays information about an item.", this.HandleItemInfo);
			this.Add("monsterinfo", "<name>", "Displays information about a monster.", this.HandleMonsterInfo);
			this.Add("whodrops", "<name>", "Finds monsters and cubes that drop a given item.", this.HandleWhoDrops);
			this.Add("whereis", "<name>", "Finds the maps where a monster spawns.", this.HandleWhereIs);
			this.Add("reputation", "<name>", "Displays current reputation.", this.HandleReputation);
			this.Add("who", "", "Displays players online with names.", this.HandlePlayersOnline);
			this.Add("w", "", "Displays player count.", this.HandlePlayerCount);
			this.Add("uptime", "", "Displays the server uptime.", this.HandleUptime);
			this.Add("rates", "", "Displays the current server rates.", this.HandleRates);

			// VIP
			this.Add("autoloot", "", "Toggles autolooting.", this.HandleAutoloot);
			this.Add("rangepreview", "", "Toggles skill range preview.", this.HandleRangePreview);

			// GMs
			this.Add("name", "<new name>", "Changes character name.", this.HandleName);
			this.Add("jump", "<x> <y> <z> | <x> <z>", "Teleports to given location on the same map.", this.HandleJump);
			this.Add("warp", "<map id> <x> <y> <z>", "Warps to another map.", this.HandleWarp);
			this.Add("item", "<item id> [amount] [grade] [unidentified]", "Spawns item. 'true' as grade = random grade unidentified.", this.HandleItem);
			this.Add("identify", "", "Identifies all unidentified items in inventory.", this.HandleIdentify);
			this.Add("appraise", "", "Identifies all unidentified items in inventory.", this.HandleIdentify);
			this.Add("refine", "<slot> <amount>", "Refines equipment. Slot 0 = all equipped items.", this.HandleRefine);
			this.Add("silver", "<modifier>", "Spawns silver.", this.HandleSilver);
			this.Add("droptest", "<item id|name> [count=1] [radius=50]", "Drops items on the ground for pickup testing.", this.HandleDropTest);
			this.Add("spawn", "<monster id|class name> [amount=1] ['ai'=BasicMonster] ['tendency'=peaceful] ['hp'=amount]", "Spawns monster.", this.HandleSpawn);
			this.Add("madhatter", "", "Spawns all headgears.", this.HandleGetAllHats);
			this.Add("heartofcards", "", "Spawns all boss cards at level 10.", this.HandleGetAllBossCards);
			this.Add("heartofgems", "", "Spawns all gems at level 10.", this.HandleGetAllGems);
			this.Add("levelup", "<levels>", "Changes character's level.", this.HandleLevelUp);
			this.Add("joblevelup", "<levels>", "Changes character's job level (use negative values to downgrade).", this.HandleJobLevelUp);
			this.Add("addexp", "<amount>", "Gives base exp to character.", this.HandleAddExp);
			this.Add("addjobexp", "<amount>", "Gives job exp to character.", this.HandleAddJobExp);
			this.Add("speed", "<speed>", "Modifies character's speed.", this.HandleSpeed);
			this.Add("go", "<destination>", "Warps to certain pre-defined destinations.", this.HandleGo);
			this.Add("goto", "<team name>", "Warps to another character.", this.HandleGoTo);
			this.Add("recall", "<team name>", "Warps another character back.", this.HandleRecall);
			this.Add("recallmap", "[map id/name]", "Warps all characters on given map back.", this.HandleRecallMap);
			this.Add("recallall", "", "Warps all characters on the server back.", this.HandleRecallAll);
			this.Add("heal", "[hp] [sp] [stamina]", "Heals the character's HP, SP, and Stamina.", this.HandleHeal);
			this.Add("alive", "", "Revives the character if dead, or fully heals if alive.", this.HandleAlive);
			this.Add("clearinv", "", "Removes all items from inventory.", this.HandleClearInventory);
			this.Add("addjob", "<job id|name> [circle]", "Adds a job to character by ID or name.", this.HandleAddJob);
			this.Add("removejob", "<job id>", "Removes a job from character.", this.HandleRemoveJob);
			this.Add("skillpoints", "[job id] <modifier>", "Modifies character's skill points. If no job id is given, adds skill points to all jobs character has.", this.HandleSkillPoints);
			this.Add("statpoints", "<amount>", "Modifies character's stat points.", this.HandleStatPoints);
			this.Add("resetstats", "", "Resets character's allocated stat points.", this.HandleStatReset);
			this.Add("str", "<+/- amount>", "Modifies character's STR stat.", this.HandleStr);
			this.Add("dex", "<+/- amount>", "Modifies character's DEX stat.", this.HandleDex);
			this.Add("con", "<+/- amount>", "Modifies character's CON stat.", this.HandleCon);
			this.Add("int", "<+/- amount>", "Modifies character's INT stat.", this.HandleInt);
			this.Add("spr", "<+/- amount>", "Modifies character's SPR stat.", this.HandleSpr);
			this.Add("resetskills", "", "Resets character's skills, returning all spent points.", this.HandleSkillReset);
			this.Add("resetabilities", "", "Resets character's learned abilities, refunding spent ability points.", this.HandleAbilityReset);
			this.Add("broadcast", "<message>", "Broadcasts text message to all players.", this.HandleBroadcast);
			this.Add("b", "<message>", "Alias for broadcast.", this.HandleBroadcast);
			this.Add("kick", "<team name>", "Kicks the player with the given team name if they're online.", this.HandleKick);
			this.Add("runscp", "<script> <handle>", "Official GM Command for various purpose.", this.HandleRunScript);
			this.Add("killmon", "<handle>", "Official GM Command for killing a monster.", this.HandleKillMonster);
			this.Add("fixcam", "", "Fixes the character's camera in place.", this.HandleFixCamera);
			this.Add("daytime", "[timeOfDay=day|night|dawn|dusk]", "Sets the current day time.", this.HandleDayTime);
			this.Add("storage", "", "Opens personal storage.", this.HandlePersonalStorage);
			this.Add("teamstorage", "", "Opens team storage.", this.HandleTeamStorage);
			this.Add("medals", "<modifier>", "Modifies the amount of medals/TP.", this.HandleMedals);
			this.Add("killmonsters", "<handle>", "Official GM Command for killing all monster on the map.", this.HandleKillMonsters);
			this.Add("items", "", "Spawns all the items.", this.HandleGetAllItems);
			this.Add("dungeon", "<id>", "", this.HandleDungeonMatchMaking);

			// Dev
			this.Add("test", "", "", this.HandleTest);
			this.Add("reloadscripts", "", "Reloads all scripts.", this.HandleReloadScripts);
			this.Add("reloadconf", "", "Reloads configuration files.", this.HandleReloadConf);
			this.Add("reloaddata", "", "Reloads data.", this.HandleReloadData);
			this.Add("ai", "[ai name]", "Activates AI for character.", this.HandleAi);
			this.Add("updatedata", "", "Updates data.", this.HandleUpdateData);
			this.Add("updatedatacom", "", "Updates data.", this.HandleUpdateDataCom);
			this.Add("feature", "<feature name> <enabled>", "Toggles a feature.", this.HandleFeature);
			this.Add("resetcd", "", "Resets all skill cooldowns.", this.HandleResetSkillCooldown);
			this.Add("nosave", "<enabled>", "Toggles whether the character will be saved on logout.", this.NoSave);
			this.Add("savelocation", "<location memo>", "Saves a location to locations.txt in temp folder.", this.HandleSaveLocation);
			this.Add("marktreasure", "", "Saves current position as treasure chest spawn to treasures.txt in temp folder.", this.HandleMarkTreasure);
			this.Add("markwarpentry", "", "Marks current position as warpable and saves to mark_warp.txt in temp folder", this.HandleMarkWarpEntry);
			this.Add("markwarpexit", "", "Marks current position as warpable and saves to mark_warp.txt in temp folder", this.HandleMarkWarpExit);
			this.Add("getitemsfile", "", "Gets all items listed in itemid_list.txt in temp folder.", this.HandleGetAllItemsFile);
			this.Add("generatemapimages", "", "Creates a folder 'map_images' in temp folder with all map images.", this.HandleGenerateMapImages);
			this.Add("skill", "", "Add or remove a skill.", this.HandleSkillManagement);
			this.Add("resetdungeon", "[daily|weekly|all]", "Resets dungeon entry counters.", this.HandleResetDungeon);
			this.Add("resetteamstorage", "", "Resets team storage properties (expansions) to default.", this.HandleResetTeamStorage);
			this.Add("cubeinfo", "<group|item_class>", "Shows contents of a cube/gacha by group name or item class.", this.HandleCubeInfo);
			this.Add("cubelist", "[filter]", "Lists all available cube/gacha groups.", this.HandleCubeList);

			// Test ZC_NORMAL Packets.
			this.Add("timeactiontarget", "<player> <anim> <secs> [msg]", "Shows a time action bar to another player.", this.HandleTimeActionOnlyTarget);
			this.Add("npcstateanim", "<handle> <state> <anim>", "Plays a state animation on the targeted NPC.", this.HandleNpcStateAnim);
			this.Add("wind", "<power> <dir> <w> <h> <dist> <time>", "Creates a wind area effect.", this.HandleWindArea);
			this.Add("throwui", "<target_handle>", "Throws a UI effect to a target.", this.HandleThrowUIToActor);
			this.Add("fixbillboard", "<angle> [0/1]", "Fixes the target's billboard rotation.", this.HandleFixBillboard);
			this.Add("attachitem", "<item_id> <slot> [0/1]", "Attaches an item model to the target.", this.HandleAttachItem);
			this.Add("testeffect", "<effect_name> [duration]", "Plays an effect at your position and saves its handle.", this.HandleTestEffect);
			this.Add("endeffect", "[handle]", "Ends the last saved effect or a specified one.", this.HandleEndEffect);
			this.Add("attachtonode", "<attach_handle> <target_handle> <node>", "Attaches one actor to another.", this.HandleAttachToNode);
			this.Add("playeffect15", "<effect_name>", "Plays an effect using the old 0x15 opcode.", this.HandlePlayEffect15);
			this.Add("playforce16", "<target_h> <effect>", "Tests the old PlayForceEffect (0x16) packet.", this.HandlePlayForceEffect16);
			this.Add("reflect", "<orig_caster_h> <new_target_h> <skill_id>", "Tests the PlayForceReflect packet.", this.HandlePlayForceReflect);
			this.Add("addeffect", "<effect_name> [scale]", "Adds a persistent effect to the target.", this.HandleAddEffect);
			this.Add("removeeffectbyname", "<effect_name>", "Removes a persistent effect from the target.", this.HandleRemoveEffectByName);

			// Official GM Commands
			this.Add("safe", "<state>", "Toggles whether the character is invincible or not.", this.HandleSafe);
			this.Add("run", "<script name> <options>", "Runs a script on the server for specific behavior.", this.HandleRun);

			// Chronomancer Test Commands
			// Aliases
			this.AddAlias("iteminfo", "ii");
			this.AddAlias("monsterinfo", "mi");
			this.AddAlias("reloadscripts", "rs");
			this.AddAlias("jump", "setpos");
			this.AddAlias("resetstats", "statsreset");
			this.AddAlias("resetabilities", "resetattributes");
			this.AddAlias("savelocation", "sl");
			this.AddAlias("killmonsters", "killmons");
		}

		private CommandResult HandleDungeonMatchMaking(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// --- 1. Argument Parsing and Validation ---
			if (args.Count < 1)
			{
				sender.ServerMessage("Please specify a dungeon ID. Usage: /command <dungeonId>");
				return CommandResult.InvalidArgument;
			}

			if (!int.TryParse(args.Get(0), out var instanceDungeonId))
			{
				sender.ServerMessage($"'{args.Get(0)}' is not a valid dungeon ID.");
				return CommandResult.InvalidArgument;
			}

			// Check if a matchmaking UI is already open for this player
			if (sender.Variables.Perm.GetInt(AutoMatchZoneManager.DungeonIdVarName) != 0)
			{
				sender.ServerMessage("You already have a dungeon matchmaking window open.");
				return CommandResult.Okay;
			}

			if (!ZoneServer.Instance.Data.InstanceDungeonDb.TryFind(instanceDungeonId, out var dungeonInfo))
			{
				sender.ServerMessage($"Dungeon with ID {instanceDungeonId} could not be found.");
				return CommandResult.Okay;
			}

			var uiOptions = new DungeonOptions
			{
				AllowAutoMatch = true, // Default, can be overridden by dungeon properties
				AllowEnterNow = true,  // Default
			};

			// The default for party match is to follow the auto-match option
			uiOptions.AllowPartyMatch = uiOptions.AllowAutoMatch;

			// Check if the player can re-enter an existing auto-matched dungeon
			// This replaces: IsAutoMatchIndunExist(pc, indunType)
			if (uiOptions.AllowAutoMatch)
			{
				//uiOptions.AllowAutoMatchReenter = ZoneServer.Instance.World.AutoMatch.IsDungeonInProgress(sender, dungeonInfo.Id);
			}
			else
			{
				uiOptions.AllowAutoMatchReenter = false;
			}

			// --- 4. Set Player State and Send Packet ---

			// Store the dungeon ID the player is trying to register for, to prevent opening multiple UIs.
			sender.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, dungeonInfo.Id);

			// Use our new, clearer method to send the packet
			Send.ZC_NORMAL.InstanceDungeonMatchMaking(sender, dungeonInfo.Id, uiOptions);

			return CommandResult.Okay;
		}

		private CommandResult HandleResetDungeon(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var resetType = args.Count > 0 ? args.Get(0).ToLower() : "all";

			switch (resetType)
			{
				case "daily":
					ZoneServer.Instance.DungeonReset.ResetDailyForCharacter(target);
					sender.ServerMessage($"Daily dungeon entries reset for {target.Name}.");
					break;
				case "weekly":
					ZoneServer.Instance.DungeonReset.ResetWeeklyForCharacter(target);
					sender.ServerMessage($"Weekly dungeon entries reset for {target.Name}.");
					break;
				case "all":
					ZoneServer.Instance.DungeonReset.ResetDailyForCharacter(target);
					ZoneServer.Instance.DungeonReset.ResetWeeklyForCharacter(target);
					sender.ServerMessage($"All dungeon entries reset for {target.Name}.");
					break;
				default:
					sender.ServerMessage("Usage: /resetdungeon [daily|weekly|all]");
					return CommandResult.InvalidArgument;
			}

			return CommandResult.Okay;
		}

		private CommandResult HandleSaveLocation(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var tmpFilePath = Path.Combine(Path.GetTempPath(), "locations.txt");

			var position = string.Format("{0}, {1}, {2}", sender.Map.ClassName, sender.Position, (int)sender.Direction.DegreeAngle);

			File.AppendAllText(tmpFilePath, message.Substring(message.IndexOf(" savelocation") + " savelocation".Length).Trim() + ", " + position + "\n");

			return CommandResult.Okay;
		}

		private CommandResult HandleMarkTreasure(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var tmpFilePath = Path.Combine(Path.GetTempPath(), "treasures.txt");

			// Parse position into separate components
			var position = sender.Position;
			var formattedLocation = $"{{ map: \"{sender.Map.ClassName}\", " +
								   $"x: {position.X}, " +
								   $"y: {position.Y}, " +
								   $"z: {position.Z}, " +
								   $"direction: {(int)sender.Direction.DegreeAngle} }},";

			// Append the formatted string to the file
			File.AppendAllText(tmpFilePath, formattedLocation + Environment.NewLine);

			sender.ServerMessage(Localization.Get("Treasure location marked."));

			return CommandResult.Okay;
		}

		private CommandResult HandleMarkWarpEntry(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var character = target;
			var filePath = Path.Combine(Path.GetTempPath(), "mark_warp.txt");

			var map = character.Map;
			var mapWidth = map.Ground.SizeX;
			var mapHeight = map.Ground.SizeZ;

			// 258, From("c_orsha", -1422.59, 368.31)
			var logEntry = $"{(int)character.Direction.DegreeAngle}, From(\"{character.Map.ClassName}\", {(int)character.Position.X}, {(int)character.Position.Z})";
			using (var writer = File.AppendText(filePath))
			{
				writer.WriteLine(logEntry);
			}
			sender.ServerMessage(Localization.Get("From Warp marked."));
			return CommandResult.Okay;
		}

		private CommandResult HandleMarkWarpExit(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var character = target;
			var filePath = Path.Combine(Path.GetTempPath(), "mark_warp.txt");

			var map = character.Map;
			var mapWidth = map.Ground.SizeX;
			var mapHeight = map.Ground.SizeZ;

			// To("c_orsha", -1422.59, 368.31)
			var logEntry = $"To(\"{character.Map.ClassName}\", {(int)character.Position.X}, {(int)character.Position.Z})";
			using (var writer = File.AppendText(filePath))
			{
				writer.WriteLine(logEntry);
			}
			sender.ServerMessage(Localization.Get("To Warp marked."));
			return CommandResult.Okay;
		}

		/// <summary>
		/// Official command for running scripts.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleRun(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;

			switch (args.Get(0))
			{
				case "CHANGE_JOB":
				{
					if (!ZoneServer.Instance.Data.JobDb.TryFind(j => j.ClassName.ToLower() == args.Get(1).ToLower(), out var job))
						return CommandResult.InvalidArgument;
					sender.ChangeJob(job.Id);
					break;
				}
				case "REPAIR_ALL_CHEAT":
					foreach (var item in sender.Inventory.GetItems().Values)
					{
						if (item is DummyEquipItem)
							continue;
						if (item.Durability <= 0)
							continue;
						item.ModifyDurability(sender, (int)item.MaxDurability);
					}
					break;
				default:
					sender.MsgBox($"Unsupported run argument {args.Get(0)}.");
					break;
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official command for invincibility.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSafe(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;

			if (args.Get(0) == "on")
				sender.SetSafeState(true);
			else
				sender.SetSafeState(false);

			return CommandResult.Okay;
		}

		private CommandResult HandleCardBattle(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var table = new Npc(57659, $"{sender.Name}'s Card Table", sender.GetLocation(), sender.Direction.Backwards);
			sender.Map.AddMonster(table);
			var chair = new Npc(57657, $"{sender.Name}'s Card Chair", sender.GetLocation(), sender.Direction.Backwards);
			sender.Map.AddMonster(chair);
			var script = $"MY_CARDBATTELE_OPEN({sender.Handle}, {table.Handle}, nil);";
			sender.ExecuteClientScript(script);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Calculates distance between two positions.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleDistance(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var vars = target.Variables.Temp;

			if (!vars.TryGet<Position>("Melia.Commands.DistancePos1", out var pos1))
			{
				vars.Set("Melia.Commands.DistancePos1", target.Position);

				sender.ServerMessage(Localization.Get("Saved first position. Go to second position and use the command again."));
			}
			else
			{
				vars.Remove("Melia.Commands.DistancePos1");

				var pos2 = target.Position;
				var distance2D = pos1.Get2DDistance(pos2);
				var distance3D = pos1.Get3DDistance(pos2);

				sender.ServerMessage(Localization.Get("Distance: {0:0.##} (2D), {1:0.##} (3D)"), distance2D, distance3D);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Test command, modify to quickly test something, but never
		/// commit the changes to it.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleTest(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count > 0 && args.Get(0) == "pathfind")
			{
				return this.HandlePathfindTest(sender, args);
			}
			// Removed: Houses test block - PersonalHouse type deleted during Laima merge
			// else if (args.Get(0).ToLower() == "house") { ... }
			else if (args.Count >= 0 && args.Get(0).ToLower() == "orchard")
			{
				sender.Warp("f_orchard_34_2", new Position(1933.90f, 399.69f, -455.72f));
				sender.Tracks.Start("f_orchard_34_2_Flume", TimeSpan.Zero);
				return CommandResult.Okay;
			}
			else if (args.Count >= 0 && args.Get(0).ToLower() == "orchard1")
			{
				sender.Warp("f_orchard_34_2", new Position(-1682.90f, 346.20f, 56.95f));
				sender.Tracks.Start("f_orchard_34_2_slider", TimeSpan.Zero);
				return CommandResult.Okay;
			}
			else if (args.Count >= 0 && args.Get(0).ToLower() == "bracken")
			{
				sender.Warp("f_bracken_63_1", new Position(427.343994f, 5.266178f, 1800.973022f));
				sender.Tracks.Start("f_bracken_63_1_flume", TimeSpan.Zero);
				return CommandResult.Okay;
			}
			else if (args.Count >= 0 && args.Get(0).ToLower() == "orchard2")
			{
				sender.Warp("f_orchard_34_2", new Position(-646.41f, -151.92f, -290.28f));
				sender.Tracks.Start("f_orchard_34_2_flamingo", TimeSpan.Zero);
				return CommandResult.Okay;
			}
			else if (args.Count >= 0 && args.Get(0).ToLower() == "jump2pos")
			{
				var moveTime = 0.3f;
				var jumpPower = 300f;


				if (args.Count >= 1)
					float.TryParse(args.Get(1), out moveTime);
				if (args.Count >= 2)
					float.TryParse(args.Get(2), out jumpPower);


				Send.ZC_NORMAL.JumpToPosition(sender, moveTime, jumpPower);
				return CommandResult.Okay;
			}
			else if (args.Count >= 0 && args.Get(0).ToLower() == "guild")
			{
				if (sender.GuildId == 0)
					sender.ExecuteClientScript(ClientScripts.OPEN_GUILD_CREATE_UI);
				return CommandResult.Okay;
			}
			else if (args.Count >= 0 && args.Get(0).ToLower() == "market")
			{
				Send.ZC_CUSTOM_DIALOG(sender.Connection, CustomDialog.MARKET);
				return CommandResult.Okay;
			}
			else if (args.Count >= 1 && args.Get(0).ToLower() == "monballoon")
			{
				var skillId = 20609;
				if (args.Count >= 2)
					int.TryParse(args.Get(1), out skillId);

				var castTime = 1000;
				if (args.Count >= 3)
					int.TryParse(args.Get(2), out castTime);

				var changeColor = 0;
				if (args.Count >= 4)
					int.TryParse(args.Get(3), out changeColor);

				var showCastingBar = 1;
				if (args.Count >= 5)
					int.TryParse(args.Get(4), out showCastingBar);

				Send.ZC_NORMAL.MonsterUsePCSkill(sender, (SkillId)skillId, castTime, changeColor, showCastingBar);
				return CommandResult.Okay;
			}


			Log.Debug("test!!");

			if (sender.Map == null || sender.Map == Map.Limbo)
			{
				Log.Error("Cannot spawn bot on null or Limbo map.");
				return CommandResult.Fail;
			}

			Send.ZC_SHOW_EMOTICON(sender, "I_emo_mvp", TimeSpan.FromSeconds(2));
			Task.Delay(5)
				.ContinueWith(_ =>
				{
					Send.ZC_EMOTICON(sender, "I_emo_mvp", TimeSpan.FromSeconds(2));
				});

			//SkillResultHelper.SkillResultTargetBuff(sender,)

			var subOpCode = 0xF0;
			if (args.Count >= 1 && !int.TryParse(args.Get(0), out subOpCode))
				return CommandResult.Okay;

			var factor = 1.0f;
			if (args.Count >= 2 && !float.TryParse(args.Get(1), out factor))
				return CommandResult.Okay;

			var time = 1.0f;
			if (args.Count >= 3 && !float.TryParse(args.Get(2), out time))
				return CommandResult.Okay;

			var packet = new Packet(Op.ZC_NORMAL);

			packet.PutSubOp(NormalOpType.Zone, subOpCode);
			packet.PutInt(sender.Handle);
			packet.PutFloat(factor);
			packet.PutFloat(time);

			sender.Connection.Send(packet);

			if (true)
				return CommandResult.Okay;

			//Send.ZC_NORMAL.MarketItemList(sender.Connection);

			/**
			
			if (!int.TryParse(args.Get(0), out var comboCount))
				return CommandResult.Okay;

			if (!float.TryParse(args.Get(1), out var comboDuration))
				return CommandResult.Okay;

			if (!int.TryParse(args.Get(2), out var i1))
				return CommandResult.Okay;

			Send.ZC_NORMAL.ShowComboEffect(sender.Connection, comboCount, comboDuration, i1);

			if (ZoneServer.Instance.World.Guilds.Create(sender, args.Get(0)) != null)
			{
				sender.SystemMessage("CreateGuildSuccess");
				Send.ZC_DECREASE_SILVER(sender, 1000000);
			}
			**/
			/**
			if (!ZoneServer.Instance.World.Houses.TryGet(sender.DbId, out var house))
			{
				house = ZoneServer.Instance.World.Houses.Create(sender);
			}
			sender.Warp(house.GetEnterLocation());
			**/

			/**
			// Prepare the combat trial map (if it's a dynamic map)
			var indun = new InstanceDungeon(16);
			indun.MapId = 521;
			var indunMap = new InstanceDungeonMap(indun);
			ZoneServer.Instance.World.AddMap(indunMap);
			sender.Warp(indunMap.Id, indunMap.Data.DefaultPosition);
			**/

			return CommandResult.Okay;
		}

		/// <summary>
		/// Test command, modify to quickly test something, but never
		/// commit the changes to it.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSkillManagement(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count > 0 && args.Get(0) == "add")
			{
				if (!int.TryParse(args.Get(1), out var iSkillId))
					return CommandResult.InvalidArgument;
				var skillId = (SkillId)iSkillId;
				target.Skills.Add(new Skill(target, skillId, isEquipSkill: true));
			}
			else
			{
				if (!int.TryParse(args.Get(1), out var iSkillId))
					return CommandResult.InvalidArgument;
				var skillId = (SkillId)iSkillId;
				target.Skills.Remove(skillId);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Tells the sender how much reputation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleReputation(Character sender, Character target, string message, string command, Arguments args)
		{
			var character = sender == target ? sender : target;
			var faction = character.GetRegionFaction();
			var reputation = ZoneServer.Instance.World.Factions.GetReputation(character, faction);
			var newReputation = reputation;
			if (args.Count > 0)
				int.TryParse(args.Get(0), out newReputation);
			if (reputation != newReputation)
			{
				ZoneServer.Instance.World.Factions.ModifyReputation(character, faction, newReputation);
				reputation += newReputation;
			}
			var tier = ZoneServer.Instance.World.Factions.GetTierName(reputation);

			if (sender == target)
				sender.ServerMessage(Localization.Get("Your reputation is {0} ({1})."), tier, reputation);
			else
				sender.ServerMessage(Localization.Get("{2}'s reputation is {0} ({1})."), tier, ZoneServer.Instance.World.Factions.GetReputation(target, faction), target.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays the number of players currently online (simple count).
		/// </summary>
		private CommandResult HandlePlayerCount(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var playerCount = ZoneServer.Instance.World.GetCharacterCount();
			sender.ServerMessage(Localization.Get("{0} players online."), playerCount);
			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays the number of players currently online with their names.
		/// </summary>
		private CommandResult HandlePlayersOnline(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var characters = ZoneServer.Instance.World.GetCharacters();
			var playerCount = characters.Length;

			sender.ServerMessage(Localization.Get("Players online: {0}"), playerCount);

			foreach (var character in characters)
				sender.ServerMessage("  - {0} ({1})", character.Name, character.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays the server's uptime.
		/// </summary>
		private CommandResult HandleUptime(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// This command should always report to the sender.
			var uptime = ZoneServer.Instance.World.WorldTime.Elapsed;
			var uptimeString = "";
			if (uptime.Days > 0) uptimeString += $"{uptime.Days}d ";
			if (uptime.Hours > 0 || uptime.Days > 0) uptimeString += $"{uptime.Hours}h ";
			uptimeString += $"{uptime.Minutes}m";

			sender.ServerMessage(Localization.Get("Server Uptime: {0}"), uptimeString.Trim());
			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays the current server rates.
		/// </summary>
		private CommandResult HandleRates(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// This command should always report to the sender.
			var worldConf = ZoneServer.Instance.Conf.World;
			sender.ServerMessage(Localization.Get("Current Server Rates:"));
			sender.ServerMessage(Localization.Get("- Experience: {0}%"), worldConf.ExpRate);
			sender.ServerMessage(Localization.Get("- Job Experience: {0}%"), worldConf.JobExpRate);
			sender.ServerMessage(Localization.Get("- Silver Drop Amount: {0}%"), worldConf.SilverDropAmount);
			sender.ServerMessage(Localization.Get("- Item Drop Chance: {0}%"), worldConf.GeneralDropRate);
			return CommandResult.Okay;
		}

		/// <summary>
		/// Tells the sender where the target currently is.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleWhere(Character sender, Character target, string message, string command, Arguments args)
		{
			if (sender == target)
				sender.ServerMessage(Localization.Get("You are here: {0} ({1}), {2} (Direction: {3:0.#####}°) (WorldId: {4})"), target.Map.ClassName, target.Map.Id, target.Position, target.Direction.DegreeAngle, target.Map.WorldId);
			else
				sender.ServerMessage(Localization.Get("{3} is here: {0} ({1}), {2} (Direction: {3:0.#####}°) (WorldId: {4})"), target.Map.ClassName, target.Map.Id, target.Position, target.TeamName, target.Direction.DegreeAngle, target.Map.WorldId);

			if (sender.Connection.Account.PermissionLevel == PermissionLevel.Dev)
			{
				target.Map.Ground.TryGetCellIndex(target.Position, out var index);
				target.Map.Ground.TryGetHeightAt(target.Position, out var height);
				sender.ServerMessage(Localization.Get("Cell Index {0}, Height {1}"), index, height);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays the current server and game time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleTime(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var now = GameTime.Now;

			target.ServerMessage(Localization.Get("Server Time: {0:yyyy-MM-dd HH:mm}"), now.DateTime);
			target.ServerMessage(Localization.Get("Game Time: {0:y-M-dd HH:mm} ({1})"), now, now.TimeOfDay);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays a list of usable commands or details about one command.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleHelp(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var targetAuthLevel = target.Connection.Account.Authority;

			// Display info about one command
			if (args.Count != 0)
			{
				var helpCommandName = args.Get(0);
				var command = this.GetCommand(helpCommandName);
				var levels = ZoneServer.Instance.Conf.Commands.GetLevelsOrDefault(command.Name);

				if (levels.Self > targetAuthLevel)
				{
					sender.ServerMessage(Localization.Get("Command not found or not available."));
					return CommandResult.Okay;
				}

				var aliases = _commands.Where(a => a.Value == command && a.Key != helpCommandName).Select(a => a.Key);

				sender.ServerMessage(Localization.Get("Name: {0}"), command.Name);
				if (aliases.Any())
					sender.ServerMessage(Localization.Get("Aliases: {0}"), string.Join(", ", aliases));
				sender.ServerMessage(Localization.Get("Description: {0}"), command.Description);
				sender.ServerMessage(Localization.Get("Arguments: {0}"), command.Usage);
			}
			// Display list of available commands
			else
			{
				var commandNames = new List<string>();

				foreach (var command in _commands.Values)
				{
					var levels = ZoneServer.Instance.Conf.Commands.GetLevelsOrDefault(command.Name);
					if (levels.Self > targetAuthLevel)
						continue;

					commandNames.Add(command.Name);
				}

				if (commandNames.Count == 0)
				{
					sender.ServerMessage(Localization.Get("No commands found."));
					return CommandResult.Okay;
				}

				var sb = new StringBuilder();

				sender.ServerMessage(Localization.Get("Available commands:"));
				foreach (var name in commandNames)
				{
					// Group command names in strings up to 100 characters,
					// as that's the maximum amount some clients will display
					// as one message.
					if (sb.Length + 2 + name.Length >= 100)
					{
						sender.ServerMessage(sb.ToString());
						sb.Clear();
					}

					if (sb.Length != 0)
						sb.Append(", ");

					sb.Append(name);
				}

				if (sb.Length != 0)
					sender.ServerMessage(sb.ToString());
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps target to given position on their current map.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleJump(Character sender, Character target, string message, string command, Arguments args)
		{
			Position newPos;

			if (args.Count == 0)
			{
				if (!sender.Map.Ground.TryGetRandomPosition(out var rndPos))
				{
					sender.ServerMessage(Localization.Get("Jump to random position failed."));
					return CommandResult.Fail;
				}

				newPos = rndPos;
			}
			else if (args.Count == 2)
			{
				if (!float.TryParse(args.Get(0), NumberStyles.Float, CultureInfo.InvariantCulture, out var x) || !float.TryParse(args.Get(1), NumberStyles.Float, CultureInfo.InvariantCulture, out var z))
					return CommandResult.InvalidArgument;

				var tempPos = new Position(x, 0, z);
				if (!sender.Map.Ground.TryGetHeightAt(tempPos, out var height))
				{
					sender.ServerMessage(Localization.Get("Failed to find ground at position ({0}, {1})."), x, z);
					return CommandResult.Fail;
				}

				newPos = new Position(x, height, z);
			}
			else if (args.Count == 3)
			{
				if (!float.TryParse(args.Get(0), NumberStyles.Float, CultureInfo.InvariantCulture, out var x) || !float.TryParse(args.Get(1), NumberStyles.Float, CultureInfo.InvariantCulture, out var y) || !float.TryParse(args.Get(2), NumberStyles.Float, CultureInfo.InvariantCulture, out var z))
					return CommandResult.InvalidArgument;

				newPos = new Position(x, y, z);
			}
			else
			{
				return CommandResult.InvalidArgument;
			}

			target.Position = newPos;
			Send.ZC_SET_POS(target);

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("You were warped to {0}."), target.Position);
			}
			else
			{
				target.ServerMessage(Localization.Get("You were warped to {0} by {1}."), target.Position, sender.TeamName);
				sender.ServerMessage(Localization.Get("Target was warped."));
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps target to the specified map.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleTokenWarp(Character sender, Character target, string message, string command, Arguments args)
		{
			if (!sender.Connection.Account.Premium.CanUseTokenWarp)
			{
				sender.MsgBox(Localization.Get("Only premium token users are allowed to use this feature."));
				return CommandResult.Okay;
			}

			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			// Find map id
			var mapClassName = args.Get(0);

			if (!ZoneServer.Instance.Data.MapDb.TryFind(mapClassName, out var mapData))
			{
				sender.MsgBox(Localization.Get("Error: The destination does not appear to exist."));
				return CommandResult.Okay;
			}

			var mapId = mapData.Id;

			// Get target position
			var targetPos = mapData.DefaultPosition;

			// Check if the map is available
			var availableZones = ZoneServer.Instance.ServerList.GetZoneServers(mapId);
			if (availableZones.Length == 0)
			{
				sender.MsgBox(Localization.Get("Error: The destination does not appear to be available."));
				return CommandResult.Okay;
			}

			// Warp
			target.Warp(mapId, targetPos);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps target to given location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleWarp(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			// Get map id
			if (!int.TryParse(args.Get(0), out var mapId))
			{
				var data = ZoneServer.Instance.Data.MapDb.Find(args.Get(0));
				if (data == null)
				{
					sender.ServerMessage(Localization.Get("Map not found."));
					return CommandResult.Okay;
				}

				mapId = data.Id;
			}

			// Get map
			if (!ZoneServer.Instance.World.TryGetMap(mapId, out var map))
			{
				sender.ServerMessage(Localization.Get("Map not found."));
				return CommandResult.Okay;
			}

			// Get target position
			Position targetPos;
			if (args.Count < 4)
			{
				if (!map.Ground.TryGetRandomPosition(out targetPos))
				{
					sender.ServerMessage(Localization.Get("Random position warp failed."));
					return CommandResult.Okay;
				}
			}
			else
			{
				if (!float.TryParse(args.Get(1), NumberStyles.Float, CultureInfo.InvariantCulture, out var x))
					return CommandResult.InvalidArgument;

				if (!float.TryParse(args.Get(2), NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
					return CommandResult.InvalidArgument;

				if (!float.TryParse(args.Get(3), NumberStyles.Float, CultureInfo.InvariantCulture, out var z))
					return CommandResult.InvalidArgument;

				targetPos = new Position(x, y, z);
			}

			// Warp
			try
			{
				target.Warp(mapId, targetPos);

				if (sender == target)
				{
					sender.ServerMessage(Localization.Get("You were warped to {0}."), target.GetLocation());
				}
				else
				{
					target.ServerMessage(Localization.Get("You were warped to {0} by {1}."), target.GetLocation(), sender.TeamName);
					sender.ServerMessage(Localization.Get("Target was warped."));
				}
			}
			catch (ArgumentException)
			{
				sender.ServerMessage("Map not found.");
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Spawns item in target's inventory.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleItem(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.IndexedCount == 0)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(0), out var itemId))
			{
				var itemName = args.Get(0);

				var classNameMatches = ZoneServer.Instance.Data.ItemDb.FindAll(a => a.ClassName.Contains(itemName, StringComparison.InvariantCultureIgnoreCase));
				if (classNameMatches.Length == 0)
				{
					sender.ServerMessage(Localization.Get("Item '{0}' not found."), itemName);
					return CommandResult.Okay;
				}

				var rankedMatches = classNameMatches.OrderBy(a => a.ClassName.GetLevenshteinDistance(itemName, false));
				itemId = rankedMatches.First().Id;
			}
			else if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
			{
				sender.ServerMessage(Localization.Get("Item not found."));
				return CommandResult.Okay;
			}

			// Parse optional arguments: [amount] [grade] [unidentified]
			var amount = 1;
			var grade = ItemGrade.None;
			var unidentified = false;
			var randomGrade = false;
			var argIndex = 1;

			// Get amount (optional - if not a number, skip to grade)
			if (args.IndexedCount >= 2)
			{
				if (int.TryParse(args.Get(1), out var parsedAmount))
				{
					if (parsedAmount < 1)
						return CommandResult.InvalidArgument;
					amount = parsedAmount;
					argIndex = 2;
				}
			}

			// Get grade (or "true" for random grade + unidentified)
			if (args.IndexedCount > argIndex)
			{
				var arg = args.Get(argIndex);
				if (bool.TryParse(arg, out var boolVal) && boolVal)
				{
					randomGrade = true;
					unidentified = true;
				}
				else if (!Enum.TryParse(arg, true, out grade))
				{
					sender.ServerMessage(Localization.Get("Invalid grade. Use: true, Normal/Magic/Rare/Unique/Legend/Goddess"));
					return CommandResult.Okay;
				}
				argIndex++;
			}

			// Get unidentified flag
			if (args.IndexedCount > argIndex && !bool.TryParse(args.Get(argIndex), out unidentified))
				return CommandResult.InvalidArgument;

			// Roll random grade if requested
			if (randomGrade)
			{
				var grades = new[] { ItemGrade.Magic, ItemGrade.Rare, ItemGrade.Unique, ItemGrade.Legend };
				grade = grades[RandomProvider.Get().Next(grades.Length)];
			}

			// Create and add item(s)
			var testItem = new Item(itemId, 1);
			var isStackable = testItem.IsStackable;

			if (isStackable)
			{
				var item = new Item(itemId, amount);
				if (grade != ItemGrade.None)
					item.Properties.SetFloat(PropertyName.ItemGrade, (int)grade);
				if (unidentified && grade != ItemGrade.None && grade != ItemGrade.Normal)
				{
					item.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
					item.Properties.SetFloat(PropertyName.NeedAppraisal, 1);
				}
				target.Inventory.Add(item, InventoryAddType.PickUp);
			}
			else
			{
				for (var i = 0; i < amount; i++)
				{
					var item = new Item(itemId, 1);
					if (grade != ItemGrade.None)
						item.Properties.SetFloat(PropertyName.ItemGrade, (int)grade);
					if (unidentified && grade != ItemGrade.None && grade != ItemGrade.Normal)
					{
						item.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
						item.Properties.SetFloat(PropertyName.NeedAppraisal, 1);
					}
					target.Inventory.Add(item, InventoryAddType.PickUp);
				}
			}

			sender.ServerMessage(Localization.Get("Item created."));
			if (sender != target)
				target.ServerMessage(Localization.Get("An item was added to your inventory by {0}."), sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds or removes silver from target's inventory.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSilver(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(0), out var modifier) || modifier == 0)
				return CommandResult.InvalidArgument;

			// Create and add silver item
			if (modifier > 0)
			{
				var item = new Item(ItemId.Silver, modifier);
				target.Inventory.Add(item, InventoryAddType.PickUp);

				if (sender == target)
				{
					sender.ServerMessage(Localization.Get("{0:n0} silver were added to your inventory."), modifier);
				}
				else
				{
					sender.ServerMessage(Localization.Get("{0:n0} silver were added to target's inventory."), modifier);
					target.ServerMessage(Localization.Get("{0} added {1:n0} silver to your inventory."), sender.TeamName, modifier);
				}
			}
			// Remove silver items
			else
			{
				modifier = -modifier;

				target.Inventory.Remove(ItemId.Silver, modifier, InventoryItemRemoveMsg.Destroyed);

				if (sender == target)
				{
					sender.ServerMessage(Localization.Get("{0:n0} silver were removed from your inventory."), modifier);
				}
				else
				{
					sender.ServerMessage(Localization.Get("{0:n0} silver were removed from target's inventory."), modifier);
					target.ServerMessage(Localization.Get("{0} removed {1:n0} silver from your inventory."), sender.TeamName, modifier);
				}
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Drops items on the ground for pickup testing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleDropTest(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.IndexedCount == 0)
				return CommandResult.InvalidArgument;

			// Parse item ID or name
			int itemId;
			if (!int.TryParse(args.Get(0), out itemId))
			{
				var itemName = args.Get(0);

				var classNameMatches = ZoneServer.Instance.Data.ItemDb.FindAll(a => a.ClassName.Contains(itemName, StringComparison.InvariantCultureIgnoreCase));
				if (classNameMatches.Length == 0)
				{
					sender.ServerMessage(Localization.Get("Item '{0}' not found."), itemName);
					return CommandResult.Okay;
				}

				var rankedMatches = classNameMatches.OrderBy(a => a.ClassName.GetLevenshteinDistance(itemName, false));
				itemId = rankedMatches.First().Id;
			}
			else if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
			{
				sender.ServerMessage(Localization.Get("Item not found."));
				return CommandResult.Okay;
			}

			// Parse count (default 1)
			var count = 1;
			if (args.IndexedCount >= 2 && int.TryParse(args.Get(1), out var parsedCount))
			{
				if (parsedCount < 1 || parsedCount > 500)
				{
					sender.ServerMessage(Localization.Get("Count must be between 1 and 500."));
					return CommandResult.Okay;
				}
				count = parsedCount;
			}

			// Parse radius (default 50)
			var radius = 50f;
			if (args.IndexedCount >= 3 && float.TryParse(args.Get(2), out var parsedRadius))
			{
				if (parsedRadius < 0 || parsedRadius > 500)
				{
					sender.ServerMessage(Localization.Get("Radius must be between 0 and 500."));
					return CommandResult.Okay;
				}
				radius = parsedRadius;
			}

			var map = target.Map;
			var position = target.Position;
			var rnd = RandomProvider.Get();

			for (var i = 0; i < count; i++)
			{
				var item = new Item(itemId, 1);

				// Calculate random drop position within radius
				var angle = rnd.NextDouble() * Math.PI * 2;
				var distance = (float)(rnd.NextDouble() * radius);
				var direction = new Direction((float)Math.Cos(angle), (float)Math.Sin(angle));

				item.Drop(map, position, direction, distance, target.AccountObjectId, target.Layer);
			}

			sender.ServerMessage(Localization.Get("Dropped {0} items on the ground."), count);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Spawns monsters at target's location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSpawn(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.IndexedCount == 0)
				return CommandResult.InvalidArgument;

			MonsterData monsterData;
			if (int.TryParse(args.Get(0), out var id))
			{
				monsterData = ZoneServer.Instance.Data.MonsterDb.Find(id);
				if (monsterData == null)
				{
					sender.ServerMessage(Localization.Get("Monster not found by id."));
					return CommandResult.Okay;
				}
			}
			else
			{
				var searchName = args.Get(0).ToLower();

				var monstersData = ZoneServer.Instance.Data.MonsterDb.Entries.Values.Where(a => a.ClassName.Contains(searchName, StringComparison.InvariantCultureIgnoreCase)).ToList();
				if (monstersData.Count == 0)
				{
					sender.ServerMessage(Localization.Get("Monster not found by name."));
					return CommandResult.Okay;
				}

				// Sort candidates by how close their name is to the search
				// name, to find the one that's closest to it.
				var sorted = monstersData.OrderBy(a => a.ClassName.ToLower().GetLevenshteinDistance(searchName));
				monsterData = sorted.First();
			}

			var amount = 1;
			if (args.IndexedCount >= 2 && !int.TryParse(args.Get(1), out amount))
				return CommandResult.InvalidArgument;

			amount = Math2.Clamp(1, 100, amount);

			var aiName = "BasicMonster";
			if (args.TryGet("ai", out var aiNameArg))
			{
				if (aiNameArg.ToLower() == "none")
					aiName = null;
				else
					aiName = aiNameArg;
			}

			var tendency = TendencyType.Peaceful;
			if (args.TryGet("tendency", out var tendencyArg) && tendencyArg.ToLower() == "aggressive")
				tendency = TendencyType.Aggressive;

			var rnd = new Random(Environment.TickCount);
			for (var i = 0; i < amount; ++i)
			{
				var monster = new Mob(monsterData.Id, RelationType.Enemy);

				Position pos;
				Direction dir;
				if (amount == 1)
				{
					pos = target.Position.GetRandomInRange2D(30, 50);
					dir = target.Direction;
				}
				else
				{
					pos = target.Position.GetRandomInRange2D(amount * 4, rnd);
					dir = new Direction(rnd.Next(0, 360));
				}

				if (!target.Map.Ground.TryGetNearestValidPosition(pos, out var validPos))
					validPos = target.Position;

				monster.Position = validPos;
				monster.Direction = dir;
				monster.SpawnPosition = monster.Position;
				monster.Tendency = tendency;
				monster.Components.Add(new MovementComponent(monster));

				if (args.TryGet("hp", out var hpStr))
				{
					if (!int.TryParse(hpStr, out var hp))
					{
						sender.ServerMessage(Localization.Get("Invalid HP amount."));
						return CommandResult.Okay;
					}

					monster.Properties.Overrides.SetFloat(PropertyName.MHP, hp);
					monster.Properties.Invalidate(PropertyName.MHP);
					monster.Properties.SetFloat(PropertyName.HP, hp);
				}

				if (!string.IsNullOrWhiteSpace(aiName))
					monster.Components.Add(new AiComponent(monster, aiName));

				target.Map.AddMonster(monster);
			}

			sender.ServerMessage(Localization.Get("Monsters were spawned."));
			if (sender != target)
				target.ServerMessage(Localization.Get("Monsters were spawned at your location by {0}."), sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds all available hats to target's inventory.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGetAllHats(Character sender, Character target, string message, string command, Arguments args)
		{
			var addedCount = 0;
			for (var itemId = 628001; itemId <= 629503; ++itemId)
			{
				if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
					continue;

				if (!target.Inventory.HasItem(itemId))
				{
					target.Inventory.Add(new Item(itemId), InventoryAddType.PickUp);
					addedCount++;
				}
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("Added {0} hats to your inventory."), addedCount);
			}
			else
			{
				target.ServerMessage(Localization.Get("{1} added {0} hats to your inventory."), addedCount, sender.TeamName);
				sender.ServerMessage(Localization.Get("Added {0} hats to target's inventory."), addedCount);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds all boss cards to target's inventory at level 10.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGetAllBossCards(Character sender, Character target, string message, string command, Arguments args)
		{
			var addedCount = 0;
			for (var itemId = 644001; itemId <= 644159; ++itemId)
			{
				if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
					continue;

				if (!target.Inventory.HasItem(itemId))
				{
					var card = new Item(itemId);
					var targetLevel = 10;
					var totalExp = ZoneServer.Instance.Data.ItemExpDb.GetTotalExp(card.Data.EquipExpGroup, targetLevel);
					card.Properties[PropertyName.ItemExp] = totalExp;
					card.GetItemLevelExp(totalExp, out var level, out var curExp, out var maxExp);
					card.Properties[PropertyName.CardLevel] = level;
					target.Inventory.Add(card, InventoryAddType.PickUp);
					addedCount++;
				}
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("Added {0} boss cards (level 10) to your inventory."), addedCount);
			}
			else
			{
				target.ServerMessage(Localization.Get("{1} added {0} boss cards (level 10) to your inventory."), addedCount, sender.TeamName);
				sender.ServerMessage(Localization.Get("Added {0} boss cards (level 10) to target's inventory."), addedCount);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds all gems to target's inventory at level 10.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGetAllGems(Character sender, Character target, string message, string command, Arguments args)
		{
			var addedCount = 0;
			for (var itemId = 643501; itemId <= 745051; ++itemId)
			{
				if (!ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var itemData))
					continue;

				// Skip non-gem items (like cards which are in this ID range)
				if (itemData.Group != ItemGroup.Gem)
					continue;

				if (!target.Inventory.HasItem(itemId))
				{
					var gem = new Item(itemId);
					var targetLevel = 10;
					var totalExp = ZoneServer.Instance.Data.ItemExpDb.GetTotalExp(gem.Data.EquipExpGroup, targetLevel);
					gem.Properties[PropertyName.ItemExp] = totalExp;
					gem.GetItemLevelExp(totalExp, out var level, out var curExp, out var maxExp);
					gem.Properties[PropertyName.GemLevel] = level;
					target.Inventory.Add(gem, InventoryAddType.PickUp);
					addedCount++;
				}
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("Added {0} gems (level 10) to your inventory."), addedCount);
			}
			else
			{
				target.ServerMessage(Localization.Get("{1} added {0} gems (level 10) to your inventory."), addedCount, sender.TeamName);
				sender.ServerMessage(Localization.Get("Added {0} gems (level 10) to target's inventory."), addedCount);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds all available hats to target's inventory.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGetAllItems(Character sender, Character target, string message, string command, Arguments args)
		{
			var addedCount = 0;
			foreach (var item in ZoneServer.Instance.Data.ItemDb.Entries.Values)
			{
				if (!target.Inventory.HasItem(item.Id))
				{
					target.Inventory.Add(new Item(item.Id), InventoryAddType.PickUp);
					addedCount++;
				}
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("Added {0} items to your inventory."), addedCount);
			}
			else
			{
				target.ServerMessage(Localization.Get("{1} added {0} items to your inventory."), addedCount, sender.TeamName);
				sender.ServerMessage(Localization.Get("Added {0} items to target's inventory."), addedCount);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Changes target's name (not team name).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleName(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var newName = args.Get(0);
			if (newName == sender.Name)
				return CommandResult.Okay;

			// TODO: Can you rename any time, without cooldown?

			// TODO: Keep a list of all account characters after all?
			if (ZoneServer.Instance.Database.CharacterExists(target.Connection.Account.Id, newName))
			{
				sender.ServerMessage(Localization.Get("Name already exists."));
				return CommandResult.Okay;
			}

			target.Name = newName;
			Send.ZC_PC(target, PcUpdateType.Name, 0, 0, newName);

			sender.ServerMessage(Localization.Get("Name changed."), target.Position);
			if (sender != target)
				target.ServerMessage(Localization.Get("Your name was changed by {0}."), sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Reloads all scripts.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleReloadScripts(Character sender, Character target, string message, string command, Arguments args)
		{
			sender.ServerMessage(Localization.Get("Reloading scripts..."));

			var companionsToRestore = new System.Collections.Generic.Dictionary<Character, Companion>();
			var characters = ZoneServer.Instance.World.GetCharacters();

			foreach (var character in characters)
			{
				if (character.ActiveCompanion != null)
					companionsToRestore[character] = character.ActiveCompanion;
			}

			KeywordDb.Clear();
			ZoneServer.Instance.World.RemoveScriptedEntities();
			ZoneServer.Instance.ReloadScripts();

			foreach (var kvp in companionsToRestore)
			{
				var character = kvp.Key;
				var companion = kvp.Value;
				if (companion != null)
					companion.SetCompanionState(true);
			}

			sender.ServerMessage(Localization.Get("Done."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Reloads all conf files.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleReloadConf(Character sender, Character target, string message, string command, Arguments args)
		{
			sender.ServerMessage(Localization.Get("Reloading configuration..."));

			ZoneServer.Instance.Conf.Load();

			sender.ServerMessage(Localization.Get("Done."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Reloads all data files.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleReloadData(Character sender, Character target, string message, string command, Arguments args)
		{
			sender.ServerMessage(Localization.Get("Reloading data..."));

			ZoneServer.Instance.LoadData(ServerType.Zone);

			sender.ServerMessage(Localization.Get("Done."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Levels up target.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleLevelUp(Character sender, Character target, string message, string command, Arguments args)
		{
			var levelChange = 1;
			if (args.Count >= 1 && !int.TryParse(args.Get(0), out levelChange))
				return CommandResult.InvalidArgument;

			if (levelChange > 0)
			{
				levelChange = Math.Min(levelChange, ZoneServer.Instance.Conf.World.MaxLevel - target.Level);

				if (levelChange == 0)
				{
					if (sender == target)
						sender.ServerMessage(Localization.Get("Your level can't be increased any further."));
					else
						sender.ServerMessage(Localization.Get("The level of the target can't be increased any further."));
					return CommandResult.Okay;
				}

				target.Exp = 0;
				target.TotalExp = ZoneServer.Instance.Data.ExpDb.GetTotalExp(target.Level + levelChange);
				target.LevelUp(levelChange);
			}
			else if (levelChange < 0)
			{
				var levelsToRemove = Math.Min(-levelChange, target.Level - 1);

				if (levelsToRemove == 0)
				{
					if (sender == target)
						sender.ServerMessage(Localization.Get("Your level can't be decreased any further."));
					else
						sender.ServerMessage(Localization.Get("The level of the target can't be decreased any further."));
					return CommandResult.Okay;
				}

				target.Exp = 0;
				target.TotalExp = ZoneServer.Instance.Data.ExpDb.GetTotalExp(target.Level - levelsToRemove);
				target.LevelDown(levelsToRemove);
			}
			else
			{
				sender.ServerMessage(Localization.Get("Level change amount cannot be zero."));
				return CommandResult.Okay;
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("Your level was changed."));
			}
			else
			{
				target.ServerMessage(Localization.Get("Your level was changed by {0}."), sender.TeamName);
				sender.ServerMessage(Localization.Get("The target's level was changed."));
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Levels up target's job level.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleJobLevelUp(Character sender, Character target, string message, string command, Arguments args)
		{
			var levelChange = 1;
			if (args.Count >= 1 && !int.TryParse(args.Get(0), out levelChange))
				return CommandResult.InvalidArgument;

			if (levelChange > 0)
			{
				var jobLevelsGained = target.JobLevelUp(levelChange);

				if (jobLevelsGained == 0)
				{
					if (sender == target)
						sender.ServerMessage(Localization.Get("Your current job's level can't be increased any further."));
					else
						sender.ServerMessage(Localization.Get("The level of the target's current job can't be increased any further."));
				}
				else
				{
					if (sender == target)
					{
						sender.ServerMessage(Localization.GetPlural("Your job level was increased by {0} level.", "Your job level was increased by {0} levels.", jobLevelsGained), jobLevelsGained);
					}
					else
					{
						target.ServerMessage(Localization.GetPlural("Your job level was increased by {0} by {1} level.", "Your job level was increased by {0} by {1} levels.", jobLevelsGained), sender.TeamName, jobLevelsGained);
						sender.ServerMessage(Localization.GetPlural("The target's job level was increased by {0} level.", "The target's job level was increased by {0} levels.", jobLevelsGained), jobLevelsGained);
					}
				}
			}
			else if (levelChange < 0)
			{
				var jobLevelsLost = target.JobLevelDown(-levelChange);

				if (jobLevelsLost == 0)
				{
					if (sender == target)
						sender.ServerMessage(Localization.Get("Your current job's level can't be decreased any further."));
					else
						sender.ServerMessage(Localization.Get("The level of the target's current job can't be decreased any further."));
				}
				else
				{
					if (sender == target)
					{
						sender.ServerMessage(Localization.GetPlural("Your job level was decreased by {0} level.", "Your job level was decreased by {0} levels.", jobLevelsLost), jobLevelsLost);
					}
					else
					{
						target.ServerMessage(Localization.GetPlural("Your job level was decreased by {0} by {1} level.", "Your job level was decreased by {0} by {1} levels.", jobLevelsLost), sender.TeamName, jobLevelsLost);
						sender.ServerMessage(Localization.GetPlural("The target's job level was decreased by {0} level.", "The target's job level was decreased by {0} levels.", jobLevelsLost), jobLevelsLost);
					}
				}
			}
			else
			{
				sender.ServerMessage(Localization.Get("Job level change amount cannot be zero."));
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Gives base exp to target.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleAddExp(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0 || !long.TryParse(args.Get(0), out var amount))
				return CommandResult.InvalidArgument;

			target.GiveExp(amount, 0, null);

			if (sender == target)
				sender.ServerMessage(Localization.Get("Added {0} base exp."), amount);
			else
			{
				target.ServerMessage(Localization.Get("{0} added {1} base exp to you."), sender.TeamName, amount);
				sender.ServerMessage(Localization.Get("Added {0} base exp to target."), amount);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Gives job exp to target.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleAddJobExp(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0 || !long.TryParse(args.Get(0), out var amount))
				return CommandResult.InvalidArgument;

			target.GiveExp(0, amount, null);

			if (sender == target)
				sender.ServerMessage(Localization.Get("Added {0} job exp."), amount);
			else
			{
				target.ServerMessage(Localization.Get("{0} added {1} job exp to you."), sender.TeamName, amount);
				sender.ServerMessage(Localization.Get("Added {0} job exp to target."), amount);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Changes target's speed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSpeed(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			if (!float.TryParse(args.Get(0), out var speed))
				return CommandResult.InvalidArgument;

			var currentSpeed = target.Properties.GetFloat(PropertyName.MSPD);
			var bonusSpeed = speed - currentSpeed;

			target.Properties.Modify(PropertyName.MSPD_Bonus, bonusSpeed);
			Send.ZC_MOVE_SPEED(target);

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("Your speed was changed."));
			}
			else
			{
				target.ServerMessage(Localization.Get("Your speed was changed by {0}."), sender.TeamName);
				sender.ServerMessage(Localization.Get("Target's speed was changed."));
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Searches item database for given string.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleItemInfo(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var search = string.Join(" ", args.GetAll());
			var items = ZoneServer.Instance.Data.ItemDb.FindAll(search);

			if (items.Count == 0)
			{
				sender.ServerMessage(Localization.Get("No items found for '{0}'."), search);
				return CommandResult.Okay;
			}

			var maxItemCount = 20;

			sender.ServerMessage(Localization.Get("Results: {0} (Max. {1} shown)"), items.Count, maxItemCount);

			var matchingItems = items.OrderBy(a => a.Name.GetLevenshteinDistance(search)).ThenBy(a => a.Id);
			foreach (var item in matchingItems.Take(maxItemCount))
				sender.ServerMessage(Localization.Get("{0}: {1}, Category: {2}"), item.Id, item.Name, item.Category);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Searches monster database for given string.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleMonsterInfo(Character sender, Character target, string message, string command, Arguments args)
		{
			var monsterRaces = new[] { "Unknown", "Insect", "Mutant", "Plant", "Demon", "Beast", "Item" };
			var monsterElements = new[] { "None", "Fire", "Ice", "Poison", "Earth", "Melee", "Psychokinesis", "Lightning", "Holy", "Dark" };
			var monsterArmors = new[] { "None", "Cloth", "Leather", "Iron", "Chain", "Ghost", "Shield", "Aries" };

			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var search = string.Join(" ", args.GetAll());

			var monsters = ZoneServer.Instance.Data.MonsterDb.FindAllPreferExact(search);
			if (monsters.Count == 0)
			{
				sender.ServerMessage(Localization.Get("No monsters found for '{0}'."), search);
				return CommandResult.Okay;
			}

			var maxMonsterCount = 20;

			sender.ServerMessage(Localization.Get("Results: {0} (Max. {1} shown)"), monsters.Count, maxMonsterCount);

			var monsterEntries = monsters.OrderBy(a => a.Name.GetLevenshteinDistance(search)).ThenBy(a => a.Id);
			foreach (var monsterData in monsterEntries.Take(maxMonsterCount))
			{
				var monsterEntry = new StringBuilder();

				monsterEntry.AppendFormat(Localization.Get("{{nl}}----- {0} ({1}, {2}) -----{{nl}}"), monsterData.Name, monsterData.Id, monsterData.ClassName);
				monsterEntry.AppendFormat(Localization.Get("{0} / {1} / {2} / {3}{{nl}}"), monsterRaces[(int)monsterData.Race], monsterElements[(int)monsterData.Attribute], monsterArmors[(int)monsterData.ArmorMaterial], Enum.GetName(monsterData.Size));
				monsterEntry.AppendFormat(Localization.Get("HP: {0}  SP: {1}  EXP: {2}  CEXP: {3}{{nl}}"), monsterData.Hp, monsterData.Sp, (int)(monsterData.Exp * ZoneServer.Instance.Conf.World.ExpRate / 100f), (int)(monsterData.JobExp * ZoneServer.Instance.Conf.World.JobExpRate / 100f));
				monsterEntry.AppendFormat(Localization.Get("Atk: {0}~{1}  MAtk: {2}~{3}  Def: {4}  MDef: {5}{{nl}}"), monsterData.PhysicalAttackMin, monsterData.PhysicalAttackMax, monsterData.MagicalAttackMin, monsterData.MagicalAttackMax, monsterData.PhysicalDefense, monsterData.MagicalDefense);
				monsterEntry.AppendFormat(Localization.Get("Accuracy: {0} Dodge: {1} Block: {2} Block Pen: {3}{{nl}}"), monsterData.HitRate, monsterData.DodgeRate, monsterData.BlockRate, monsterData.BlockBreakRate);
				monsterEntry.AppendFormat(Localization.Get("Crit. Rate: {0} Crit. Resist: {1} Crit. Atk: {2}{{nl}}"), monsterData.CritHitRate, monsterData.CritDodgeRate, monsterData.CritAttack);

				if (monsterData.Drops.Count != 0)
				{
					monsterEntry.Append(Localization.Get("Drops:"));

					foreach (var currentDrop in monsterData.Drops)
					{
						var itemData = ZoneServer.Instance.Data.ItemDb.Find(currentDrop.ItemId);
						if (itemData == null)
							continue;

						var dropChance = Math2.Clamp(0, 100, currentDrop.DropChance);
						var adjustedDropChance = Math2.Clamp(0, 100, Mob.GetAdjustedDropRate(currentDrop));
						var isMoney = (currentDrop.ItemId == ItemId.Silver || currentDrop.ItemId == ItemId.Gold);

						var minAmount = currentDrop.MinAmount;
						var maxAmount = currentDrop.MaxAmount;
						var hasAmount = (minAmount > 1 || maxAmount > 1);

						if (isMoney)
						{
							minAmount = Math.Max(1, (int)(minAmount * (ZoneServer.Instance.Conf.World.SilverDropAmount / 100f)));
							maxAmount = Math.Max(minAmount, (int)(maxAmount * (ZoneServer.Instance.Conf.World.SilverDropAmount / 100f)));
						}

						var displayAmount = isMoney || hasAmount;

						if (displayAmount)
						{
							if (minAmount == maxAmount)
								monsterEntry.AppendFormat(Localization.Get("{{nl}}- {0} {1} ({2:0.####}% -> {3:0.####}%)"), minAmount, itemData.Name, dropChance, adjustedDropChance);
							else
								monsterEntry.AppendFormat(Localization.Get("{{nl}}- {0}~{1} {2} ({3:0.####}% -> {4:0.####}%)"), minAmount, maxAmount, itemData.Name, dropChance, adjustedDropChance);
						}
						else
						{
							monsterEntry.AppendFormat(Localization.Get("{{nl}}- {0} ({1:0.####}% -> {2:0.####}%)"), itemData.Name, dropChance, adjustedDropChance);
						}
					}
				}
				else
				{
					monsterEntry.Append(Localization.Get("This monster has no drops."));
				}

				sender.ServerMessage(monsterEntry.ToString());
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Searches monster database to find out who drops a given item
		/// and returns a list of the best sources of that item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleWhoDrops(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var search = string.Join(" ", args.GetAll());

			var items = ZoneServer.Instance.Data.ItemDb.FindAllPreferExact(search);
			if (items.Count == 0)
			{
				sender.ServerMessage(Localization.Get("No items found for '{0}'."), search);
				return CommandResult.Okay;
			}

			var maxItemResultCount = 5;
			var maxDropperCount = 100;
			var maxDropResultCount = 10;
			var maxCubeResultCount = 5;

			sender.ServerMessage(Localization.Get("Results: {0} (Max. {1} shown)"), items.Count, maxItemResultCount);

			var itemEntries = items.OrderBy(a => a.Name.GetLevenshteinDistance(search)).ThenBy(a => a.Id);
			foreach (var currentItem in itemEntries.Take(maxItemResultCount))
			{
				var whoDropsEntry = new StringBuilder();

				whoDropsEntry.AppendFormat(Localization.Get("{{nl}}----- {0} -----{{nl}}"), currentItem.Name);

				MonsterData[] droppers;

				if (currentItem.Id == ItemId.Silver || (droppers = ZoneServer.Instance.Data.MonsterDb.FindAll(a => a.Drops.Any(b => b.ItemId == currentItem.Id))).Length > maxDropperCount)
				{
					whoDropsEntry.Append(Localization.Get("Too many enemies drop this."));
				}
				else if (droppers.Length == 0)
				{
					whoDropsEntry.Append(Localization.Get("This item is not dropped by any monsters."));
				}
				else
				{
					var bestDroppers = new List<KeyValuePair<MonsterData, float>>();

					foreach (var monsterData in droppers)
					{
						var dropDatas = monsterData.Drops.Where(a => a.ItemId == currentItem.Id);

						foreach (var dropData in dropDatas)
						{
							var dropChance = Math2.Clamp(0, 100, Mob.GetAdjustedDropRate(dropData));
							bestDroppers.Add(new KeyValuePair<MonsterData, float>(monsterData, dropChance));
						}
					}

					whoDropsEntry.AppendFormat(Localization.Get("Listing up to {0} best sources of this item:"), maxDropResultCount);

					var dropEntries = bestDroppers.OrderByDescending(a => a.Value).ThenBy(a => a.Key.Level);
					foreach (var dropDataKV in dropEntries.Take(maxDropResultCount))
					{
						var dropData = dropDataKV.Key;
						var dropChance = dropDataKV.Value;

						whoDropsEntry.AppendFormat(Localization.Get("{{nl}}{0} ({1}, {2}) - {3:0.####}%"), dropData.Name, dropData.Id, dropData.ClassName, dropChance);
					}
				}

				// Search cube gacha rewards
				var cubeGachaEntries = ZoneServer.Instance.Data.CubeGachaDb.Entries.Values
					.Where(g => g.ItemName.Equals(currentItem.ClassName, StringComparison.OrdinalIgnoreCase))
					.ToList();

				if (cubeGachaEntries.Count > 0)
				{
					// Group by gacha group and find the cube items that use each group
					var cubeDrops = new List<(string CubeName, int CubeId, float Ratio)>();

					foreach (var gachaEntry in cubeGachaEntries)
					{
						// Find cube items that use this gacha group (via script.strArg)
						var cubeItems = ZoneServer.Instance.Data.ItemDb.Entries.Values
							.Where(i => i.Script != null && i.Script.StrArg == gachaEntry.Group)
							.ToList();

						foreach (var cubeItem in cubeItems)
						{
							cubeDrops.Add((cubeItem.Name, cubeItem.Id, gachaEntry.Ratio));
						}
					}

					if (cubeDrops.Count > 0)
					{
						whoDropsEntry.AppendFormat(Localization.Get("{{nl}}Cube Rewards (up to {0} shown):"), maxCubeResultCount);

						var sortedCubeDrops = cubeDrops.OrderByDescending(c => c.Ratio).ThenBy(c => c.CubeName);
						foreach (var cubeDrop in sortedCubeDrops.Take(maxCubeResultCount))
						{
							whoDropsEntry.AppendFormat(Localization.Get("{{nl}}- {0} ({1}) - {2:0.####}%"), cubeDrop.CubeName, cubeDrop.CubeId, cubeDrop.Ratio);
						}
					}
				}

				sender.ServerMessage(whoDropsEntry.ToString());
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Finds the maps where a monster respawns
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleWhereIs(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var search = string.Join(" ", args.GetAll()).ToLower();

			// Find the monster data that matches the search term
			var monsters = ZoneServer.Instance.Data.MonsterDb.FindAllPreferExact(search);
			if (monsters.Count == 0)
			{
				sender.ServerMessage(Localization.Get("No monsters found for '{0}'."), search);
				return CommandResult.Okay;
			}

			// Get all spawners in the world
			var allSpawners = ZoneServer.Instance.World.GetSpawners();
			var monsterSpawners = allSpawners.OfType<MonsterSpawner>();
			var eventSpawners = allSpawners.OfType<EventMonsterSpawner>();

			var results = new List<(MonsterData Monster, Map Map, int MinAmount, int MaxAmount, TimeSpan MinRespawnDelay, TimeSpan MaxRespawnDelay)>();

			foreach (var monster in monsters)
			{
				// Find all MonsterSpawners that spawn this monster
				foreach (var spawner in monsterSpawners)
				{
					// Skip if this spawner doesn't spawn our monster
					if (!ZoneServer.Instance.Data.MonsterDb.TryFind(spawner.MonsterData.Id, out var spawnerMonster) ||
						spawnerMonster.Id != monster.Id)
						continue;

					// Get the map from the spawn areas
					if (!ZoneServer.Instance.World.TryGetSpawnAreas(spawner.SpawnPointsIdent, out var spawnAreas))
						continue;

					foreach (var area in spawnAreas.GetAll())
					{
						if (!ZoneServer.Instance.World.TryGetMap(area.Map.Id, out var map))
							continue;

						results.Add((monster, map, spawner.MinAmount, spawner.MaxAmount, spawner.MinRespawnDelay, spawner.MaxRespawnDelay));
					}
				}

				// Find all EventMonsterSpawners (boss spawners) that spawn this monster
				foreach (var spawner in eventSpawners)
				{
					// Skip if this spawner doesn't spawn our monster
					if (spawner.MonsterData.Id != monster.Id)
						continue;

					// Get the map directly from MapId
					if (!ZoneServer.Instance.World.TryGetMap(spawner.MapId, out var map))
						continue;

					results.Add((monster, map, spawner.MinAmount, spawner.MaxAmount, spawner.MinRespawnDelay, spawner.MaxRespawnDelay));
				}
			}

			if (results.Count == 0)
			{
				sender.ServerMessage(Localization.Get("No spawn locations found for '{0}'."), search);
				return CommandResult.Okay;
			}

			// Order results by map name
			results = results.OrderBy(x => x.Map.ClassName).ToList();

			// Display results
			foreach (var result in results)
			{
				var respawnInfo = "";

				// Add respawn time info if it exists
				if (result.MinRespawnDelay != TimeSpan.Zero || result.MaxRespawnDelay != TimeSpan.Zero)
				{
					if (result.MinRespawnDelay == result.MaxRespawnDelay)
						respawnInfo = $" [Delay: {result.MinRespawnDelay.TotalSeconds:0}s]";
					else
						respawnInfo = $" [Delay: {result.MinRespawnDelay.TotalSeconds:0}s~{result.MaxRespawnDelay.TotalSeconds:0}s]";
				}

				var response = string.Format(
					"{0} ({1}) - {2} ({3}) - Quantity: {4}~{5}{6}",
					result.Monster.Name,
					result.Monster.ClassName,
					result.Map?.Data?.Name ?? "Unknown",
					result.Map.ClassName,
					result.MinAmount,
					result.MaxAmount,
					respawnInfo
				);

				sender.ServerMessage(response);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps target to a pre-defined location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGo(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
			{
				sender.ServerMessage(Localization.Get("Destinations: {0}"), "klaipeda, orsha, start");
				return CommandResult.InvalidArgument;
			}

			if (args.Get(0).StartsWith("klaip")) target.Warp("c_Klaipe", new Position(-75, 148, -24));
			else if (args.Get(0).StartsWith("ors")) target.Warp("c_orsha", new Position(271, 176, 292));
			else if (args.Get(0).StartsWith("fedi")) target.Warp("c_fedimian", new Position(-243, 161, -303));
			else if (args.Get(0).StartsWith("high")) target.Warp("c_highlander", new Position(-20, 1, 80));
			else if (args.Get(0).StartsWith("start")) target.Warp("f_siauliai_west", new Position(-628, 260, -1025));
			else
			{
				sender.ServerMessage(Localization.Get("Unknown destination."));
				return CommandResult.Okay;
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("You were warped to {0}."), target.GetLocation());
			}
			else
			{
				target.ServerMessage(Localization.Get("You were warped to {0} by {1}."), target.GetLocation(), sender.TeamName);
				sender.ServerMessage(Localization.Get("Target was warped."));
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps target to a specific character's location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGoTo(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			// TODO: Once we have support for more than one map server,
			//   we have to search for characters across all of them.

			var teamName = args.Get(0);
			var character = ZoneServer.Instance.World.GetCharacterByTeamName(teamName);
			if (character == null)
			{
				sender.ServerMessage(Localization.Get("Character not found."));
				return CommandResult.Okay;
			}

			target.Warp(character.GetLocation());

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("You've been warped to {0}'s location."), teamName);
			}
			else
			{
				sender.ServerMessage(Localization.Get("Target was warped."));
				target.ServerMessage(Localization.Get("You've been warped to {0}'s location by {1}."), teamName, sender.TeamName);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps specific character to target.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleRecall(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			// TODO: Once we have support for more than one map server,
			//   we have to search for characters across all of them.

			var teamName = args.Get(0);
			var character = ZoneServer.Instance.World.GetCharacterByTeamName(teamName);
			if (character == null)
			{
				sender.ServerMessage(Localization.Get("Character not found."));
				return CommandResult.Okay;
			}

			character.Warp(target.GetLocation());

			character.ServerMessage(Localization.Get("You've been warped to {0}'s location."), target.TeamName);
			sender.ServerMessage(Localization.Get("Character was warped."));
			if (sender != target)
				target.ServerMessage(Localization.Get("{0} was warped to your location by {1}."), character.TeamName, sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps all players on the map to target's location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleRecallMap(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count > 2)
				return CommandResult.InvalidArgument;

			var map = target.Map;

			// TODO: Once we have support for channels and map servers,
			//   add warp from other servers and restrict recall to
			//   channel's max player count.
			if (args.Count >= 1)
			{
				// Search for map by name and id
				if (int.TryParse(args.Get(0), out var mapId))
					map = ZoneServer.Instance.World.GetMap(mapId);
				else
					map = ZoneServer.Instance.World.GetMap(args.Get(0));

				// Check map
				if (map == null)
				{
					sender.ServerMessage(Localization.Get("Unknown map."));
					return CommandResult.Okay;
				}
			}

			var characters = map.GetCharacters(a => a != target);

			// Check for characters
			if (!characters.Any())
			{
				sender.ServerMessage(Localization.Get("No players found."));
				return CommandResult.Okay;
			}

			RecallCharacters(sender, target, characters);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps all players on the server to target's location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleRecallAll(Character sender, Character target, string message, string command, Arguments args)
		{
			// TODO: Once we have support for channels and map servers,
			//   add warp from other servers and restrict recall to
			//   channel's max player count.

			// Check for characters
			var characters = ZoneServer.Instance.World.GetCharacters(a => a != target);
			if (!characters.Any())
			{
				sender.ServerMessage(Localization.Get("No players found."));
				return CommandResult.Okay;
			}

			RecallCharacters(sender, target, characters);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Recalls characters to target's location and sends appropriate
		/// server messages.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="characters"></param>
		private static void RecallCharacters(Character sender, Character target, Character[] characters)
		{
			var location = target.GetLocation();
			foreach (var character in characters)
			{
				character.Warp(location);
				character.ServerMessage(Localization.Get("You've been warped to {0}'s location."), target.TeamName);
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("You have called {0} characters to your location."), characters.Length);
			}
			else
			{
				sender.ServerMessage(Localization.Get("You have called {0} characters to target's location."), characters.Length);
				target.ServerMessage(Localization.Get("{1} called {0} characters to your location."), characters.Length, sender.TeamName);
			}
		}

		/// <summary>
		/// Heals the target hp and optionally sp.
		/// If no argument is given, heals fully.
		/// Can also heal negative values.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleHeal(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count > 3)
				return CommandResult.InvalidArgument;

			if (target.IsDead)
				return CommandResult.Okay;

			// TODO: Maybe refactor to take indexed arguments, named
			//   ones, or combinations, so you can, for example, heal
			//   stamina without specifying HP and SP like so:
			//   >heal sp:10

			// Fully heal HP, SP and Stamina if no arguments are given
			if (args.Count == 0)
			{
				target.ModifyHp(target.MaxHp);
				target.ModifySp(target.MaxSp);
				target.ModifyStamina(target.MaxStamina);

				sender.ServerMessage(Localization.Get("Healed HP, SP and Stamina."));
				if (sender != target)
					target.ServerMessage(Localization.Get("Your HP, SP and Stamina were healed by {0}."), sender.TeamName);
			}
			// Modify only HP if one argument is given
			else if (args.Count == 1)
			{
				if (!int.TryParse(args.Get(0), out var hpAmount))
					return CommandResult.InvalidArgument;

				target.ModifyHp(hpAmount);

				sender.ServerMessage(Localization.Get("Healed HP by {0} points."), hpAmount);
				if (sender != target)
					target.ServerMessage(Localization.Get("{0} healed your HP by {1} points."), sender.TeamName, hpAmount);
			}
			// Modify HP and SP if two arguments are given
			else if (args.Count == 2)
			{
				if (!int.TryParse(args.Get(0), out var hpAmount))
					return CommandResult.InvalidArgument;

				if (!int.TryParse(args.Get(1), out var spAmount))
					return CommandResult.InvalidArgument;

				target.ModifyHp(hpAmount);
				target.ModifySp(spAmount);

				sender.ServerMessage(Localization.Get("Healed HP by {0} and SP by {1} points."), hpAmount, spAmount);
				if (sender != target)
					target.ServerMessage(Localization.Get("{0} healed your HP by {1} and your SP by {2} points."), sender.TeamName, hpAmount, spAmount);
			}
			// Modify HP, SP, and Stamina if three arguments are given
			else if (args.Count >= 3)
			{
				if (!int.TryParse(args.Get(0), out var hpAmount))
					return CommandResult.InvalidArgument;

				if (!int.TryParse(args.Get(1), out var spAmount))
					return CommandResult.InvalidArgument;

				if (!int.TryParse(args.Get(2), out var staminaAmount))
					return CommandResult.InvalidArgument;

				// Adjust Stamina to match the game's display value, since
				// most users of this command wouldn't be aware of this
				// property's value being 1000 times larger than displayed.
				staminaAmount *= 1000;

				target.ModifyHp(hpAmount);
				target.ModifySp(spAmount);
				target.ModifyStamina(staminaAmount);

				sender.ServerMessage(Localization.Get("Healed HP by {0}, SP by {1}, and Stamina by {2} points."), hpAmount, spAmount, staminaAmount);
				if (sender != target)
					target.ServerMessage(Localization.Get("{0} healed your HP by {1}, SP by {2}, and Stamina by {3} points."), sender.TeamName, hpAmount, spAmount, staminaAmount);
			}

			if (target.Hp == 0)
			{
				// Ignore buffs like Priest's Revival
				sender.Kill(target);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Revives the character if dead, or fully heals if alive.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleAlive(Character sender, Character target, string message, string command, Arguments
		args)
		{
			if (target.IsDead)
			{
				// Revive in place like soul crystal
				target.Resurrect(ResurrectOptions.TryAgain);
			}
			else
			{
				// Fully heal HP, SP and Stamina
				target.ModifyHp(target.MaxHp);
				target.ModifySp(target.MaxSp);
				target.ModifyStamina(target.MaxStamina);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Removes all items from target's inventory.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleClearInventory(Character sender, Character target, string message, string command, Arguments args)
		{
			target.Inventory.Clear();

			sender.ServerMessage(Localization.Get("Inventory cleared."));
			if (sender != target)
				target.ServerMessage(Localization.Get("Your inventory was cleared by {0}."), sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to get a Member Info For Act?
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleMemberInfoForAct(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				Log.Debug("HandleMemberInfoForAct: No team name given by user '{0}'.", sender.Username);
				return CommandResult.Okay;
			}

			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.

			if (args.Count != 1)
			{
				Log.Debug("HandleMemberInfoForAct: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			// To Do - Handle Party Name Check
			//ZoneServer.Instance.World.GetParty() ?
			var character = ZoneServer.Instance.World.GetCharacterByTeamName(args.Get(0));
			if (character != null)
			{
				if (character.Connection.Party != null) // Guild check removed: Guild type deleted
				{
					Send.ZC_NORMAL.ShowParty(sender.Connection, character);
					Send.ZC_TO_SOMEWHERE_CLIENT(sender);
				}
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to change a party name
		/// </summary>
		/// <example>
		/// /partyname 0 0 1 Fun Party
		/// </example>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandlePartyName(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count < 4)
			{
				Log.Debug("HandlePartyName: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			var party = sender.Connection.Party;

			if (party == null)
			{
				sender.SystemMessage("HadNotMyParty");
				return CommandResult.Okay;
			}

			if (party.IsLeader(sender))
			{
				var partyName = message.Substring(message.IndexOf(args.Get(2)) + args.Get(2).Length + 1);
				// Client has an internal limit, additional safety check
				if (partyName.Length > 2 && partyName.Length < 16)
					sender.Connection.Party.ChangeName(partyName);
			}

			return CommandResult.Okay;
		}


		/// <summary>
		/// Official slash command to invite to a party
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandlePartyMake(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 0)
			{
				Log.Debug("HandlePartyMake: No team name given by user '{0}'.", sender.Username);
				return CommandResult.Okay;
			}

			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.

			if (args.Count != 1)
			{
				Log.Debug("HandlePartyMake: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			// To Do - Handle Party Name Check
			//ZoneServer.Instance.World.GetParty() ?
			//ZoneServer.Instance.World.Parties.Exists()
			if (sender.Connection.Party == null)
			{
				var party = ZoneServer.Instance.World.Parties.Create(sender);
				party.SetProperty(PropertyName.LastMemberAddedTime, party.DateCreated.Ticks.ToString());
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to invite a character to a party
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandlePartyInvite(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				Log.Debug("HandlePartyInvite: No team name given by user '{0}'.", sender.Username);
				return CommandResult.Okay;
			}

			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.

			if (args.Count != 1)
			{
				Log.Debug("HandlePartyInvite: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			var character = ZoneServer.Instance.World.GetCharacterByTeamName(args.Get(0));

			if (character == null)
			{
				sender.SystemMessage("TargetUserNotExist");
				return CommandResult.Okay;
			}

			// Can't invite a player that already have a party
			if (character.Connection.Party != null)
			{
				sender.SystemMessage("{PC}AlreadyBelongsToParty", new MsgParameter("PC", character.TeamName));
				return CommandResult.Okay;
			}

			Send.ZC_NORMAL.PartyInvite(character, sender, GroupType.Party);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to expel a member from a party
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandlePartyBan(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				Log.Debug("HandlePartyBan: No team name given by user '{0}'.", sender.Username);
				return CommandResult.Okay;
			}

			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.

			if (args.Count != 2)
			{
				Log.Debug("HandlePartyBan: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			var teamName = args.Get(1);
			var party = sender.Connection.Party;

			if (party == null)
			{
				sender.SystemMessage("HadNotMyParty");
				return CommandResult.Okay;
			}

			party?.Expel(sender, teamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Change party leader
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <example>/partyleader Shayaan</example>
		/// <returns></returns>
		private CommandResult HandlePartyLeader(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.

			if (args.Count != 1)
			{
				Log.Debug("HandlePartyLeader: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			var teamName = args.Get(0);
			var party = sender.Connection.Party;

			var character = ZoneServer.Instance.World.GetCharacterByTeamName(teamName);

			if (character == null)
			{
				sender.SystemMessage("TargetUserNotExist");
				return CommandResult.Okay;
			}

			if (party == null)
			{
				sender.SystemMessage("HadNotMyParty");
				return CommandResult.Okay;
			}

			if (!party.IsLeader(sender))
			{
				return CommandResult.Okay;
			}

			party.ChangeLeader(character);

			return CommandResult.Okay;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <example>/pmyp 0 Shayaan Shared_Quest 1001</example>
		/// <returns></returns>
		private CommandResult HandlePartyMemberProperty(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var party = sender.Connection.Party;
			if (party == null)
				return CommandResult.Okay;

			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count != 4)
			{
				Log.Debug("HandlePartyMemberProperty: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			byte.TryParse(args.Get(0), out var partyType);
			var memberName = args.Get(1);


			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to run script before leaving a guild
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleLeaveGuildCheck(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count != 0)
			{
				Log.Debug("HandleLeaveGuildCheck: Invalid call by user '{0}': {1}", sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			// TODO: Implement Guilds before 2030!
			// Removed: Guild type deleted during Laima merge
			// if (sender.Connection.Guild != null)
			// {
			// 	Send.ZC_EXEC_CLIENT_SCP(sender.Connection, "OUT_GUILD()");
			// }

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to leave a guild
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleLeaveGuildByWeb(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count != 0)
			{
				Log.Debug("HandleLeaveGuildByWeb: Invalid call by user '{0}': {1}", sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			// Removed: Guild type deleted during Laima merge
			// var guild = sender.Connection.Guild;
			// guild?.RemoveMember(sender);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to use gacha
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleHairGacha(Character sender, Character target, string message, string command, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count < 1)

			{
				Log.Debug("HandleHairGacha: Invalid call by user '{0}': {1}", sender.Username, command);
				return CommandResult.Okay;
			}

			//if (IsPlayingPairAnimation(pc) == 0)
			//	RunScript('SCR_USE_GHACHA_TPCUBE', pc, arg1);
			if (!ScriptableFunctions.Item.TryGet("SCR_USE_GHACHA_TPCUBE", out var script))
			{
				Log.Debug("HandleHairGacha: Invalid call by user '{0}': {1}", sender.Username, command);
				return CommandResult.Okay;
			}

			if (sender.HasItem(args.Get(0), 1))
			{
				var randomItem = ZoneServer.Instance.Data.ItemDb.Entries.ToArray().Random();
				sender.Inventory.Add(new Item(randomItem.Value.Id), InventoryAddType.NotNew, InventoryType.Inventory, 99999);
				Send.ZC_ENABLE_CONTROL(sender.Connection, "ITEM_GACHA_TP", false);
				Send.ZC_LOCK_KEY(sender, "ITEM_GACHA_TP", true);
				sender.TimedEvents.Add(TimeSpan.FromSeconds(5), TimeSpan.Zero, 0, "gacha", caller =>
				{
					Send.ZC_ENABLE_CONTROL(sender.Connection, "ITEM_GACHA_TP", true);
					Send.ZC_LOCK_KEY(sender, "ITEM_GACHA_TP", false);
					Send.ZC_ADDON_MSG(sender, "HAIR_GACHA_POPUP", 1003, randomItem.Value.ClassName);
				});
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to hire a pet
		/// </summary>
		/// <example>/pethire 3 Pet</example>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandlePetHire(Character sender, Character target, string message, string command, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count < 2)
			{
				Log.Debug("HandlePetHire: Invalid call by user '{0}': {1}", sender.Username, command);
				return CommandResult.Okay;
			}

			if (sender.HasCompanions)
			{
				var durationSeconds = 3;
				var msg = "You already have a companion. Please store your companion at the Barracks first.";
				sender?.AddonMessage("NOTICE_Dm_!", msg, durationSeconds);
				return CommandResult.Okay;
			}

			if (!int.TryParse(args.Get(0), out var petShopId))
			{
				return CommandResult.InvalidArgument;
			}

			if (!ZoneServer.Instance.Data.CompanionDb.TryFind(petShopId, out var data))
			{
				return CommandResult.InvalidArgument;
			}

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(data.ClassName, out var monData))
			{
				return CommandResult.InvalidArgument;
			}

			var targetNpcName = "Companion Trader";
			var currentNpc = sender.Connection.CurrentDialog?.Npc;
			var currentNpcName = currentNpc?.Name;

			if (string.IsNullOrEmpty(currentNpcName) || !currentNpcName.Contains(targetNpcName))
				return CommandResult.InvalidArgument;

			// Try to get price from custom companion shop first
			var price = data.Price;

			if (currentNpc != null && !string.IsNullOrEmpty(currentNpc.AssociatedShopName))
			{
				if (ZoneServer.Instance.Data.CompanionShopDb.TryFind(currentNpc.AssociatedShopName, out var shopData))
				{
					// Look up companion by ClassName since Products are keyed by product ID, not companion ID
					var productData = shopData.Products.Values.FirstOrDefault(p => p.CompanionClassName == data.ClassName);
					if (productData != null)
						price = productData.Price;
				}
			}

			// If no price is available, reject the purchase
			if (price == 0)
			{
				Log.Debug("HandlePetHire: Companion '{0}' has no price defined", data.ClassName);
				return CommandResult.Okay;
			}

			if (sender.Inventory.CountItem(ItemId.Silver) < price)
			{
				sender.SystemMessage("OwnerDontHaveSilver");
				return CommandResult.Okay;
			}

			sender.RemoveItem(ItemId.Silver, price);
			var companion = new Companion(sender, monData.Id, RelationType.Friendly);
			if (args.Count > 1)
				companion.Name = args.Get(1);

			companion.Slot = sender.Companions.GetList().Count;
			companion.IsActivated = true;
			companion.InitProperties();

			// Send properties before adding to map (matches login flow)
			Send.ZC_OBJECT_PROPERTY(sender.Connection, companion);

			sender.Companions.CreateCompanion(companion);
			companion.SetCompanionState(true);

			// Close companion shop UI after successful adoption
			Send.ZC_EXEC_CLIENT_SCP(sender.Connection, "PET_ADOPT_SUC()");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to raise pet stats
		/// </summary>
		/// <example>/petstat 528525790635969 MHP 1</example>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandlePetStat(Character sender, Character target, string message, string command, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count < 2)
			{
				Log.Debug("HandlePetStat: Invalid call by user '{0}': {1}", sender.Username, command);
				return CommandResult.Okay;
			}

			if (!sender.HasCompanions)
				return CommandResult.Okay;

			if (long.TryParse(args.Get(0), out var companionObjectId))
			{
				var companion = sender.Companions.GetCompanion(companionObjectId);
				var statName = args.Get(1);
				var propertyName = "Stat_" + statName;

				if (companion != null && PropertyTable.Exists("Monster", propertyName)
					&& int.TryParse(args.Get(2), out var modifierValue))
				{
					var SCR_Get_Companion_STAT_PRICE = ScriptableFunctions.CompanionPrice.Get("SCR_Get_Companion_STAT_PRICE");
					var totalCost = 0;

					for (var i = 0; i < modifierValue; i++)
					{
						totalCost += SCR_Get_Companion_STAT_PRICE(companion, statName);
						companion.Properties.Modify(propertyName, 1);
					}

					companion.Properties.Modify(propertyName, -modifierValue);

					if (sender.Inventory.CountItem(ItemId.Silver) >= totalCost)
					{
						sender.Inventory.RemoveItem(ItemId.Silver, totalCost);
						companion.Properties.Modify(propertyName, modifierValue);
						Send.ZC_OBJECT_PROPERTY(sender.Connection, companion);
						Send.ZC_NORMAL.PetInfo(sender);
					}
				}
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to read collections?
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleReadCollection(Character sender, Character target, string message, string command, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count == 0)
			{
				Log.Debug("HandleReadCollection: Invalid call by user '{0}': {1}", sender.Connection.Account.Name, command);
				return CommandResult.Okay;
			}

			if (!short.TryParse(args.Get(0), out var collectionId))
			{
				Log.Debug("HandleReadCollection: Invalid collection id '{0}' by user '{1}'.", collectionId, sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			if (!ZoneServer.Instance.Data.CollectionDb.TryFindByClassId(collectionId, out var collection))
			{
				Log.Debug("HandleReadCollection: Collection not found '{0}' by user '{1}'.", collectionId, sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			var propertyName = PropertyName.CollectionRead_101.Replace("101", collection.Id.ToString());
			if (PropertyTable.Exists("PCEtc", propertyName) && !sender.Etc.Properties.Has(propertyName))
			{
				sender.SetEtcProperty(propertyName, 1);
				Send.ZC_PC_PROP_UPDATE(sender, PropertyTable.GetId("PCEtc", propertyName), 1);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to return to quest giver.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleReturnToQuestGiver(Character sender, Character target, string message, string command, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count != 1)
			{
				Log.Debug("HandleReturnToQuestGiver: Invalid call by user '{0}': {1}", sender.Username, command);
				return CommandResult.Okay;
			}

			if (int.TryParse(args.Get(0), out var questId) && ZoneServer.Instance.Data.QuestDb.TryFind(questId, out var quest))
			{
				if (!sender.Quests.IsActive(questId) ||
					string.IsNullOrEmpty(quest.EndNPC)
					|| !ZoneServer.Instance.World.NPCs.TryGetValue($"{quest.EndNPC}_{quest.EndMap}", out var npc))
				{
					return CommandResult.Okay;
				}

				var mapId = npc.Map.Id;
				var newPosition = npc.Position.GetRelative(npc.Direction, 50);
				var newDirection = -npc.Direction;

				sender.SetDirection(newDirection);
				sender.Warp(mapId, newPosition);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds job to target.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleAddJob(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			JobId jobId;

			// Try to parse as integer (job ID)
			if (int.TryParse(args.Get(0), out var iJobId))
			{
				jobId = (JobId)iJobId;
				if (!ZoneServer.Instance.Data.JobDb.Contains(jobId))
				{
					sender.ServerMessage(Localization.Get("Job data for '{0}' not found."), jobId);
					return CommandResult.Okay;
				}
			}
			// Otherwise, try to find by name or class name
			else
			{
				var jobName = args.Get(0);
				if (!ZoneServer.Instance.Data.JobDb.TryFind(jobName, out var jobData))
				{
					sender.ServerMessage(Localization.Get("Job '{0}' not found. Use job ID or job name/class name."), jobName);
					return CommandResult.Okay;
				}
				jobId = jobData.Id;
			}

			var circle = JobCircle.First;

			if (args.Count >= 2)
			{
				if (!int.TryParse(args.Get(1), out var iCircle) || iCircle < (int)JobCircle.First || !Enum.IsDefined(typeof(JobCircle), iCircle))
					return CommandResult.InvalidArgument;

				circle = (JobCircle)iCircle;
			}

			var job = target.Jobs.Get(jobId);
			if (job != null && job.Circle >= circle)
			{
				sender.ServerMessage(Localization.Get("The job exists already, at an equal or higher circle."));
				return CommandResult.Okay;
			}

			if (job == null)
			{
				target.ChangeJob(jobId, circle, skillPoints: 1, playEffect: false);
			}
			else
				target.Jobs.ChangeCircle(jobId, circle);

			sender.ServerMessage(Localization.Get("Job '{0}' was added at circle '{1}'."), jobId, (int)circle);
			if (sender != target)
				target.ServerMessage(Localization.Get("Job '{0}' was added to your character at circle '{1}' by {2}."), jobId, (int)circle, sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Removes a given job from target.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleRemoveJob(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0 || !int.TryParse(args.Get(0), out var iJobId))
			{
				sender.ServerMessage(Localization.Get("List of Jobs."));
				foreach (var job in sender.Jobs.GetList().OrderBy(job => job.Id))
					sender.ServerMessage($"{(int)job.Id}. {job.Data.Name}");

				return CommandResult.InvalidArgument;
			}

			var jobId = (JobId)iJobId;

			if (target.Jobs.Count <= 1)
			{
				sender.ServerMessage(Localization.Get("You can't remove the last remaining job."));
				return CommandResult.Okay;
			}

			if (!target.Jobs.Remove(jobId))
			{
				sender.ServerMessage(Localization.Get("The job doesn't exist."));
				return CommandResult.Okay;
			}

			if (sender == target)
			{
				sender.ServerMessage(Localization.Get("Job '{0}' was removed. Login again to see the change."), jobId);
			}
			else
			{
				target.ServerMessage(Localization.Get("Job '{0}' was removed by {1}. Login again to see the change."), jobId, sender.TeamName);
				sender.ServerMessage(Localization.Get("Job '{0}' was removed from target."), jobId);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Modifies target's skill points for the given job.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSkillPoints(Character sender, Character target, string message, string command, Arguments args)
		{
			var jobIdList = new List<JobId>();
			var modifier = 0;

			// Parses Job Ids
			if (args.Count == 1)
			{
				if (!int.TryParse(args.Get(0), out modifier))
					return CommandResult.InvalidArgument;

				foreach (var job in target.Jobs.GetList())
				{
					jobIdList.Add(job.Id);
				}
			}
			else if (args.Count == 2)
			{
				if (!int.TryParse(args.Get(0), out var iJobId))
					return CommandResult.InvalidArgument;

				if (!int.TryParse(args.Get(1), out modifier))
					return CommandResult.InvalidArgument;

				jobIdList.Add((JobId)iJobId);
			}
			else
			{
				return CommandResult.InvalidArgument;
			}

			// Gives skill points
			foreach (var jobId in jobIdList)
			{

				if (!target.Jobs.ModifySkillPoints(jobId, modifier))
				{
					sender.ServerMessage(Localization.Get("The job doesn't exist."));
					return CommandResult.Okay;
				}

				if (sender == target)
				{
					sender.ServerMessage(Localization.Get("Modified {0}'s skill points by {1:+0;-0;0}."), jobId, modifier);
				}
				else
				{
					sender.ServerMessage(Localization.Get("Modified target {0}'s skill points by {1:+0;-0;0}."), jobId, modifier);
					target.ServerMessage(Localization.Get("Your {0}'s skill points were modified by {1}."), jobId, sender.TeamName);
				}
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds stat points to target character.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleStatPoints(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(0), out var amount) || amount < 1)
				return CommandResult.InvalidArgument;

			// Modification for stat points is a little tricky, because
			// the game has 3 stat point properties:
			// - Stat points gained by leveling
			// - Stat points gained in another way
			// - Used stat points
			// When increasing stats, "Used" is increased and the others are
			// left alone. I'll make this adding-only for now, until I feel
			// like untangling modifying them.

			target.AddStatPoints(amount);

			sender.ServerMessage(Localization.Get("Added {0} stat points."), amount);
			if (sender != target)
				sender.ServerMessage(Localization.Get("{1} added {0} stat points to your character."), amount, sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Resets allocated stat points of a character.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleStatReset(Character sender, Character target, string message, string command, Arguments args)
		{
			target.ResetStats();

			sender.ServerMessage(Localization.Get("Stat points were reset."));
			if (sender != target)
				sender.ServerMessage(Localization.Get("{0} has reset the stat points of your character."), sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Modifies target's STR stat.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleStr(Character sender, Character target, string message, string command, Arguments args)
		{
			return this.ModifyStat(sender, target, args, PropertyName.STR_STAT, PropertyName.STR, "STR");
		}

		/// <summary>
		/// Modifies target's DEX stat.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleDex(Character sender, Character target, string message, string command, Arguments args)
		{
			return this.ModifyStat(sender, target, args, PropertyName.DEX_STAT, PropertyName.DEX, "DEX");
		}

		/// <summary>
		/// Modifies target's CON stat.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleCon(Character sender, Character target, string message, string command, Arguments args)
		{
			return this.ModifyStat(sender, target, args, PropertyName.CON_STAT, PropertyName.CON, "CON");
		}

		/// <summary>
		/// Modifies target's INT stat.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleInt(Character sender, Character target, string message, string command, Arguments args)
		{
			return this.ModifyStat(sender, target, args, PropertyName.INT_STAT, PropertyName.INT, "INT");
		}

		/// <summary>
		/// Modifies target's SPR stat.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSpr(Character sender, Character target, string message, string command, Arguments args)
		{
			return this.ModifyStat(sender, target, args, PropertyName.MNA_STAT, PropertyName.MNA, "SPR");
		}

		/// <summary>
		/// Modifies a character's stat by the specified amount.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="args"></param>
		/// <param name="statProperty"></param>
		/// <param name="totalProperty"></param>
		/// <param name="statName"></param>
		/// <returns></returns>
		private CommandResult ModifyStat(Character sender, Character target, Arguments args, string statProperty, string totalProperty, string statName)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var arg = args.Get(0);
			if (!int.TryParse(arg, out var amount))
				return CommandResult.InvalidArgument;

			var currentStat = target.Properties.GetFloat(statProperty);
			var newStat = currentStat + amount;

			if (newStat < 0)
			{
				sender.ServerMessage(Localization.Get("Cannot reduce {0} below 0. Current allocated: {1}"), statName, (int)currentStat);
				return CommandResult.Okay;
			}

			target.Properties.Modify(statProperty, amount);
			Send.ZC_OBJECT_PROPERTY(target);

			var finalTotal = (int)target.Properties.GetFloat(totalProperty);
			var finalAllocated = (int)target.Properties.GetFloat(statProperty);
			if (amount >= 0)
				sender.ServerMessage(Localization.Get("Added {0} {1}. Allocated: {2}, Total: {3}"), amount, statName, finalAllocated, finalTotal);
			else
				sender.ServerMessage(Localization.Get("Removed {0} {1}. Allocated: {2}, Total: {3}"), -amount, statName, finalAllocated, finalTotal);

			if (sender != target)
			{
				if (amount >= 0)
					target.ServerMessage(Localization.Get("{0} added {1} {2} to your character."), sender.TeamName, amount, statName);
				else
					target.ServerMessage(Localization.Get("{0} removed {1} {2} from your character."), sender.TeamName, -amount, statName);
			}

			return CommandResult.Okay;
		}

		private CommandResult HandleSkillReset(Character sender, Character target, string message, string command, Arguments args)
		{
			target.ResetSkills();

			Send.ZC_SKILL_LIST(target);
			Send.ZC_OBJECT_PROPERTY(target);

			sender.ServerMessage(Localization.Get("Skills were reset."));
			if (sender != target)
				target.ServerMessage(Localization.Get("{0} has reset the skills of your character."), sender.TeamName);

			return CommandResult.Okay;
		}

		private CommandResult HandleAbilityReset(Character sender, Character target, string message, string command, Arguments args)
		{
			target.ResetAbilities();

			Send.ZC_ABILITY_LIST(target);
			Send.ZC_OBJECT_PROPERTY(target);

			sender.ServerMessage(Localization.Get("Abilities were reset."));
			if (sender != target)
				target.ServerMessage(Localization.Get("{0} has reset the abilities of your character."), sender.TeamName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Opens personal storage of target character
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandlePersonalStorage(Character sender, Character target, string message, string command, Arguments args)
		{
			if (!target.PersonalStorage.IsBrowsing)
			{
				target.PersonalStorage.Open();
				sender.ServerMessage(Localization.Get("Opened personal storage."));
				if (sender != target)
					target.ServerMessage(Localization.Get("Your personal storage was opened by '{0}'"), sender.TeamName);
			}
			else
			{
				sender.ServerMessage(Localization.Get("Already browsing personal storage."));
			}
			return CommandResult.Okay;
		}

		/// <summary>
		/// Opens team storage of target character
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleTeamStorage(Character sender, Character target, string message, string command, Arguments args)
		{
			if (!target.TeamStorage.IsBrowsing)
			{
				target.TeamStorage.Open();
				sender.ServerMessage(Localization.Get("Opened team storage."));
				if (sender != target)
					target.ServerMessage(Localization.Get("Your team storage was opened by '{0}'"), sender.TeamName);
			}
			else
			{
				sender.ServerMessage(Localization.Get("Already browsing team storage."));
			}
			return CommandResult.Okay;
		}

		/// <summary>
		/// Resets team storage account properties for debugging.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleResetTeamStorage(Character sender, Character target, string message, string command, Arguments args)
		{
			var account = sender.Connection.Account;

			// Remove all team storage related properties
			account.Properties.Remove(PropertyName.MaxAccountWarehouseCount);
			account.Properties.Remove(PropertyName.AccountWareHouseExtend);
			account.Properties.Remove(PropertyName.BasicAccountWarehouseSlotCount);

			// Reset the team storage size
			sender.Connection.Account.TeamStorage.InitSize();

			sender.ServerMessage("Team storage properties reset. Properties removed:");
			sender.ServerMessage("  - MaxAccountWarehouseCount");
			sender.ServerMessage("  - AccountWareHouseExtend");
			sender.ServerMessage("  - BasicAccountWarehouseSlotCount");
			sender.ServerMessage("Relog to see changes take effect.");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Kill all monsters on the map which is on the same layer as the target
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleKillMonsters(Character sender, Character target, string message, string command, Arguments args)
		{
			var monsters = target.Map.GetMonsters();

			if (monsters.Length > 0)
			{
				foreach (var monster in monsters)
				{
					if (monster is not Mob mob || mob.Layer != target.Layer || mob.IsDead || !target.IsEnemy(mob))
						continue;
					mob.Kill(target);
				}
				sender.ServerMessage(Localization.Get("All monsters has been killed on the map."));
			}
			else
			{
				sender.ServerMessage(Localization.Get("No monsters has been found on the map."));
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Toggles autoloot.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleAutoloot(Character sender, Character target, string message, string command, Arguments args)
		{
			var autoloot = sender.Variables.Perm.GetInt("Melia.Autoloot", 0);

			// If we got an argument, use it as the max drop chance of
			// items that are to be autolooted. Without an argument,
			// toggle autolooting completely on or off.
			if (args.Count >= 1)
			{
				if (!int.TryParse(args.Get(0), out autoloot))
					return CommandResult.InvalidArgument;

				autoloot = Math2.Clamp(0, 100, autoloot);
			}
			else if (autoloot == 0)
			{
				autoloot = 100;
			}
			else
			{
				autoloot = 0;
			}

			sender.Variables.Perm.SetInt("Melia.Autoloot", autoloot);

			if (autoloot == 100)
				target.ServerMessage(Localization.Get("Autoloot is now active."));
			else if (autoloot == 0)
				target.ServerMessage(Localization.Get("Autoloot is now inactive."));
			else
				target.ServerMessage(Localization.Get("Autoloot is now active for items up to a drop chance of {0}%."), autoloot);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Toggles range preview.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleRangePreview(Character sender, Character target, string message, string command, Arguments args)
		{
			var rangePreview = sender.Variables.Temp.GetBool("Melia.RangePreview", false);

			if (args.Count >= 1)
			{
				if (!bool.TryParse(args.Get(0), out rangePreview))
					return CommandResult.InvalidArgument;
			}
			else
			{
				rangePreview = !rangePreview;
			}

			sender.Variables.Temp.SetBool("Melia.RangePreview", rangePreview);

			if (rangePreview)
				target.ServerMessage(Localization.Get("Skill range preview is now active."));
			else
				target.ServerMessage(Localization.Get("Skill range preview is now inactive."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Toggles AI for target.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleAi(Character sender, Character target, string message, string command, Arguments args)
		{
			if (target.Components.Has<AiComponent>())
			{
				Send.ZC_NORMAL.SetupCutscene(target, false, false, false);

				target.Components.Remove<MovementComponent>();
				target.Components.Remove<AiComponent>();

				if (args.Count == 0)
				{
					sender.ServerMessage(Localization.Get("Disabled AI."));
					return CommandResult.Okay;
				}
			}
			else if (args.Count == 0)
			{
				sender.ServerMessage(Localization.Get("No AI active."));
				return CommandResult.Okay;
			}

			if (args.Count >= 1)
			{
				var aiName = args.Get(0);

				// Characters need to be in "cutscene mode" for the server
				// to move them, otherwise they'll just ignore the move
				// packets.
				Send.ZC_NORMAL.SetupCutscene(target, true, false, false);

				target.Components.Add(new MovementComponent(target));
				target.Components.Add(new AiComponent(target, aiName));

				sender.ServerMessage(Localization.Get("Enabled '{0}' AI."), aiName);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Initiates data update.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleUpdateData(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Instructs the client to iterate over all its items (including
			// auto-generated ones), retrieve their monster ids, and send
			// them back to the server using >updatedatacom.
			// The max length of chat messages appears to be ~4090 characters,
			// so we need to split the data into multiple messages.

			var prefix = ZoneServer.Instance.Conf.Commands.SelfPrefix[0];

			Send.ZC_EXEC_CLIENT_SCP(sender.Connection, @"
				local result = ''
				
				ui.Chat('" + prefix + @"updatedatacom init')

				local itemClassList, cnt  = GetClassList('Item');
				for i = 0, cnt - 1 do
					local itemClass = GetClassByIndexFromList(itemClassList, i)
					local itemMonsterId = geItemTable.GetItemMonster(itemClass.ClassID)
					local itemClassName = itemClass.ClassName
					
					result = result .. itemClass.ClassID .. '\t' .. itemMonsterId .. '\t' .. itemClass.ClassName .. '\n'

					if string.len(result) > 2000 then
						ui.Chat('" + prefix + @"updatedatacom add ' .. result)
						result = ''
					end
				end

				ui.Chat('" + prefix + @"updatedatacom fin')
			");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Accepts data updates and writes them to file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleUpdateDataCom(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var tmpFilePath = "user/tmp/updatedata/itemmonsters.txt";
			var outFilePath = "system/db/itemmonsters.txt";

			var tmpDirPath = Path.GetDirectoryName(tmpFilePath);
			if (!Directory.Exists(tmpDirPath))
				Directory.CreateDirectory(tmpDirPath);

			var outDirPath = Path.GetDirectoryName(outFilePath);
			if (!Directory.Exists(outDirPath))
				Directory.CreateDirectory(outDirPath);

			switch (args.Get(0))
			{
				// Clear file
				case "init":
				{
					File.WriteAllText(tmpFilePath, "");
					break;
				}
				// Add text to file
				case "add":
				{
					File.AppendAllText(tmpFilePath, message.Substring(message.IndexOf(" add") + " add".Length).Trim() + "\n");
					break;
				}
				// Generate final data
				case "fin":
				{
					var lines = File.ReadAllLines(tmpFilePath);

					var idTable = new Dictionary<int, int>();
					foreach (var line in lines)
					{
						var split = line.Split('\t');
						var itemId = int.Parse(split[0]);
						var itemMonsterId = int.Parse(split[1]);

						idTable[itemId] = itemMonsterId;
					}

					var sb = new StringBuilder();

					sb.AppendLine("// Melia");
					sb.AppendLine("// Database file");
					sb.AppendLine("//---------------------------------------------------------------------------");
					sb.AppendLine();
					sb.AppendLine("[");

					foreach (var entry in idTable.OrderBy(a => a.Key))
					{
						var itemId = entry.Key;
						var itemMonsterId = entry.Value;
						var className = "";
						var name = "";

						if (ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var data))
						{
							className = data.ClassName;
							name = data.Name;
						}

						sb.AppendFormat("{{ itemId: {0}, monsterId: {1}, className: \"{2}\", name: \"{3}\" }},", itemId, itemMonsterId, className, name);
						sb.AppendLine();
					}

					sb.AppendLine("]");

					File.WriteAllText(outFilePath, sb.ToString());
					break;
				}
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Enables or disables features.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleFeature(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count != 2)
				return CommandResult.InvalidArgument;

			var featureName = args.Get(0);
			var enabled = args.Get(1) == "true";

			if (!ZoneServer.Instance.Data.FeatureDb.TryFind(featureName, out var feature))
			{
				sender.ServerMessage(Localization.Get("Feature '{0}' not found."), featureName);
				return CommandResult.Okay;
			}

			feature.Enable(enabled);

			if (enabled)
				sender.ServerMessage(Localization.Get("Enabled feature '{0}'."), featureName);
			else
				sender.ServerMessage(Localization.Get("Disabled feature '{0}'."), featureName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Resets the cooldowns of all skills.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleResetSkillCooldown(Character sender, Character target, string message, string command, Arguments args)
		{
			foreach (var skill in target.Skills.GetList())
			{
				if (skill.IsOnCooldown)
					skill.StartCooldown(TimeSpan.Zero);
			}

			sender.ServerMessage(Localization.Get("Skill cooldowns reset."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Broadcasts a message to all players.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleBroadcast(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var joinedArgs = string.Join(" ", args.GetAll());
			var text = string.Format("{0} : {1}", target.TeamName, string.Join(" ", args.GetAll()));

			var commMessage = new NoticeTextMessage(NoticeTextType.GoldRed, text);
			ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("AllZones"));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Kicks a player if they're online.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleKick(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			// Use the sender name as the origin so you can't fake someone
			// else kicking players.

			var targetName = args.Get(0);
			var originName = sender.TeamName;

			if (ZoneServer.Instance.Data.MapDb.TryFind(targetName, out _))
			{
				var commMessage = new KickMessage(KickTargetType.Map, targetName, originName);
				ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("AllZones"));

				sender.ServerMessage(Localization.Get("Request for kicking players on map '{0}' sent."), targetName);
			}
			else
			{
				var commMessage = new KickMessage(KickTargetType.Player, targetName, originName);
				ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("AllZones"));

				sender.ServerMessage(Localization.Get("Request for kicking player '{0}' sent."), targetName);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Handle Official Run Script command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleRunScript(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;
			switch (args.Get(0))
			{
				case "get_layer":
					sender.MsgBox("Current Layer: {0}", sender.Layer);
					break;
				case "TEST_SERVPOS":
					if (int.TryParse(args.Get(1), out var handle))
					{
						var monster = sender.Map.GetMonster(handle);
						if (monster != null)
						{
							monster.Map.Ground.TryGetCellIndex(monster.Position, out var cellIndex);
							monster.Map.Ground.TryGetHeightAt(monster.Position, out var height);
							sender.MsgBox("X:{0} Y:{1} Z:{2} Cell: {3} Height: {4}", monster.Position.X, monster.Position.Y, monster.Position.Z, cellIndex, height);
							if (monster.Components.TryGet<MovementComponent>(out var movement))
							{
								movement.ShowDebug = !movement.ShowDebug;
								Send.ZC_MOTIONBLUR(monster, movement.ShowDebug);
							}
							if (monster.Components.TryGet<AiComponent>(out var ai))
							{
								//ai.Script.ShowDebug = !ai.Script.ShowDebug;
							}
						}
					}
					break;
				case "run":
					sender.ExecuteClientScript(args.Get(1));
					break;
				default:
					sender.MsgBox($"Unsupported run script argument {args.Get(0)}.");
					break;

			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Handle Official GM Kill Monster command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleKillMonster(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;

			if (int.TryParse(args.Get(0), out var handle))
			{
				var entity = sender.Map.GetCombatEntity(handle);
				if (entity == null)
				{
					var monster = sender.Map.GetMonster(handle);
					if (monster != null)
						sender.MsgBox($"Unable to kill {((IMonsterAppearance)monster).Name}");
				}
				else
					entity.Kill(null);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command, purpose unknown.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleReqUpdateEquip(Character sender, Character target, string message, string command, Arguments args)
		{
			// Command is sent when the inventory is opened, purpose unknown,
			// officials don't seem to send anything back.

			// Comment in the client's Lua files:
			//   내구도 회복 유료템 때문에 정확한 값을 지금 알아야 함.
			//   (Durability recovery Due to the paid system, you need to know the correct value now.)

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command, exchanges silver for ability points.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleBuyAbilPoint(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count < 0)
			{
				Log.Warning("HandleBuyAbilPoint: No amount given by user '{0}'.", sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			if (!int.TryParse(args.Get(0), out var amount))
			{
				Log.Warning("HandleBuyAbilPoint: Invalid amount '{0}' by user '{1}'.", amount, sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			var costPerPoint = ZoneServer.Instance.Conf.World.AbilityPointCost;
			var totalCost = (amount * costPerPoint);
			var silver = sender.Inventory.CountItem(ItemId.Silver);
			if (silver < totalCost)
			{
				Log.Warning("HandleBuyAbilPoint: User '{0}' didn't have enough money.", sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			sender.Inventory.Remove(ItemId.Silver, totalCost, InventoryItemRemoveMsg.Given);
			sender.ModifyAbilityPoints(amount);

			Send.ZC_ADDON_MSG(sender, AddonMessage.SUCCESS_BUY_ABILITY_POINT, 0, "BLANK");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Opens buy-in shop creation window or creates shop based on
		/// arguments.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleBuyShop(Character sender, Character target, string message, string command, Arguments args)
		{
			// Prevent opening a new shop when one is already active
			if (sender.Connection.ShopCreated != null)
			{
				sender.ServerMessage(Localization.Get("You already have a shop open. Close it first."));
				return CommandResult.Okay;
			}

			if (args.Count == 0)
			{
				Send.ZC_EXEC_CLIENT_SCP(sender.Connection, "OPEN_PERSONAL_SHOP_REGISTER()");
				return CommandResult.Okay;
			}

			var personalShopPacketStringId = 8471;
			if (ZoneServer.Instance.Data.PacketStringDb.TryFind("PersonalShop", out var data))
				personalShopPacketStringId = data.Id;

			if (args.Count < 2)
			{
				Log.Debug("HandleBuyShop: Not enough arguments.");
				return CommandResult.Okay;
			}

			// Read arguments
			var title = args.Get(0);
			var items = new List<Tuple<int, int, int>>();
			var totalCost = 0;

			for (var i = 1; i < args.Count; ++i)
			{
				var split = args.Get(i).Split(',');

				if (split.Length != 3 || !int.TryParse(split[0], out var id) || !int.TryParse(split[1], out var amount) || !int.TryParse(split[2], out var price))
				{
					Log.Debug("HandleBuyShop: Invalid argument '{0}'.", args.Get(i));
					return CommandResult.Okay;
				}
				totalCost += amount * price;
				items.Add(new Tuple<int, int, int>(id, amount, price));
			}

			if (!sender.HasSilver(totalCost))
			{
				return CommandResult.Okay;
			}

			// Create auto seller packet from arguments and have the
			// channel handle it as if the client had sent it.
			var packet = new Packet(Op.CZ_REGISTER_AUTOSELLER);
			packet.PutShort(0);
			packet.PutString(title, 64);
			packet.PutInt(items.Count);
			packet.PutInt(personalShopPacketStringId); // PersonalShop

			var j = 0;
			foreach (var item in items)
			{
				packet.PutInt(item.Item1);
				packet.PutInt(j);
				packet.PutInt(item.Item2);
				packet.PutInt(item.Item3);
				packet.PutEmptyBin(264);
				j++;
			}

			ZoneServer.Instance.PacketHandler.Handle(sender.Connection, packet);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Opens sell shop creation window or creates shop based on
		/// arguments.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSellShop(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Prevent opening a new shop when one is already active
			if (sender.Connection.ShopCreated != null)
			{
				sender.ServerMessage(Localization.Get("You already have a shop open. Close it first."));
				return CommandResult.Okay;
			}

			if (args.Count == 0)
			{
				Send.ZC_EXEC_CLIENT_SCP(sender.Connection, "OPEN_PERSONAL_SELL_SHOP_REGISTER()");
				return CommandResult.Okay;
			}

			if (args.Count < 2)
			{
				Log.Debug("HandleSellShop: Not enough arguments.");
				return CommandResult.Okay;
			}

			// Read arguments
			var title = args.Get(0);
			var shopBuilder = new ShopBuilder(title);

			for (var i = 1; i < args.Count; ++i)
			{
				var split = args.Get(i).Split(',');

				// Format: itemId,amount,price,worldId (worldId is optional for backwards compat)
				if (split.Length < 3 || !int.TryParse(split[0], out var itemId) || !int.TryParse(split[1], out var amount) || !int.TryParse(split[2], out var price))
				{
					Log.Debug("HandleSellShop: Invalid argument '{0}'.", args.Get(i));
					return CommandResult.Okay;
				}

				long worldId = 0;
				if (split.Length >= 4 && long.TryParse(split[3], out var parsedWorldId))
					worldId = parsedWorldId;

				// Find the item - use worldId if provided, otherwise fall back to itemId search
				Item foundItem = null;
				if (worldId != 0 && sender.Inventory.TryGetItem(worldId, out var itemByWorldId))
				{
					if (itemByWorldId.Id == itemId)
						foundItem = itemByWorldId;
				}

				if (foundItem == null)
				{
					// Fall back to search by itemId
					var items = sender.Inventory.GetItems(item => item.Id == itemId);
					foreach (var itemEntry in items)
					{
						foundItem = itemEntry.Value;
						break;
					}
				}

				if (foundItem == null || foundItem.Amount < amount)
				{
					Log.Warning("HandleSellShop: Player doesn't have item {0} (worldId={1})", itemId, worldId);
					return CommandResult.Okay;
				}

				shopBuilder.AddSellItem(itemId, amount, price, [foundItem.ObjectId]);
			}

			var shop = shopBuilder.Build();
			var personalShopPacketStringId = 8471;
			if (ZoneServer.Instance.Data.PacketStringDb.TryFind("PersonalShop", out var data))
				personalShopPacketStringId = data.Id;
			shop.Type = PersonalShopType.PersonalSell;
			shop.EffectId = personalShopPacketStringId;
			shop.OwnerHandle = sender.Handle;

			sender.Connection.ShopCreated = shop;
			Send.ZC_AUTOSELLER_LIST(sender.Connection, sender);
			Send.ZC_NORMAL.Shop_Unknown11C(sender.Connection, "Squire", shop.Type);
			Send.ZC_NORMAL.ShopAnimation(sender.Connection, sender, "Squire_Repair", 1, 1);
			Send.ZC_AUTOSELLER_TITLE(sender);

			Log.Debug("HandleSellShop: {0} opened sell shop '{1}' with {2} item(s)", sender.Name, title, shop.Products.Count);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Enables autotrade mode, allowing the character to remain in the
		/// world with their shop open while the player is disconnected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleAutoTrade(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Check if player has a shop open
			if (sender.Connection.ShopCreated == null)
			{
				sender.ServerMessage(Localization.Get("You must have a shop open to use autotrade."));
				return CommandResult.Okay;
			}

			// Check if player is in a city/town map
			if (sender.Map.Data.Type != MapType.City)
			{
				sender.ServerMessage(Localization.Get("Autotrade is only available in towns."));
				return CommandResult.Okay;
			}

			// Clean up companion and summons before entering autotrade
			sender.ActiveCompanion?.Map?.RemoveMonster(sender.ActiveCompanion);
			sender.Summons.RemoveAllSummons();

			// Remove from party before entering autotrade - autotrading characters cannot be in parties
			if (sender.Connection.Party != null)
			{
				sender.Connection.Party.RemoveMember(sender);
			}

			// Set autotrade mode flag BEFORE sending to barracks
			sender.IsAutoTrading = true;

			Log.Info($"Character '{sender.Name}' enabled autotrade mode at {sender.Position} on map '{sender.Map.ClassName}'.");

			// Save character data to ensure database has correct MapId for reconnection
			ZoneServer.Instance.Database.SaveCharacterData(sender);

			// Send player to barracks - OnClosed will keep character in world due to IsAutoTrading
			sender.MsgBox(Localization.Get("Autotrade mode enabled. Your shop will remain open while you are offline."));
			Send.ZC_ADDON_MSG(sender, AddonMessage.GAMEEXIT_TIMER_END, 0, "None");
			Send.ZC_SAVE_INFO(sender.Connection);
			Send.ZC_MOVE_BARRACK(sender.Connection);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to invite a character to a guild
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGuildInvite(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				Log.Debug("HandleGuildInvite: No team name given by user '{0}'.", sender.Username);
				return CommandResult.Okay;
			}

			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.

			if (args.Count != 1)
			{
				Log.Debug("HandleGuildInvite: Invalid call by user '{0}': {1}", sender.Username, commandName);
				return CommandResult.Okay;
			}

			if (!sender.HasGuild)
			{
				sender.SystemMessage("HadNotMyGuild");
				return CommandResult.Okay;
			}

			var character = ZoneServer.Instance.World.GetCharacterByTeamName(args.Get(0));

			if (character == null)
			{
				sender.SystemMessage("TargetUserNotExist");
				return CommandResult.Okay;
			}

			// Removed: Guild type deleted during Laima merge
			// Guild invite check and send removed
			sender.SystemMessage("Guild system is not available.");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command, increase guild exp up.
		/// </summary>
		/// <example>
		/// /guildexpup 527456344001753 9
		/// </example>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGuildExpUp(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Removed: Guild type deleted during Laima merge
			// Guild exp up functionality not available
			Log.Debug("HandleGuildExpUp: Guild system not available.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to for fast travel (warp statues).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleInteWarp(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 2)
			{
				Log.Debug("HandleInteWarp: No warp id '{0}'.", sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.

			if (args.Count != 2)
			{
				Log.Debug("HandleInteWarp: Invalid call by user '{0}': {1}", sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			var warp = "";
			WarpData warpData = null;
			if (!int.TryParse(args.Get(0), out var warpId))
			{
				warp = args.Get(0);
			}

			if (ZoneServer.Instance.Data.MapDb.TryFind(warpId, out var mapData))
			{
				warp = mapData.ClassName;
				warpId = 0;
			}

			if (warpId != 0 && !ZoneServer.Instance.Data.WarpDb.TryFind(warpId, out warpData))
			{
				Log.Debug("HandleInteWarp: Failed to find warp by id {0}, User: '{1}': {2}", args.Get(0), sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			if (!string.IsNullOrEmpty(warp) && !ZoneServer.Instance.Data.WarpDb.TryFind(warp, out warpData))
			{
				Log.Debug("HandleInteWarp: Failed to find warp by name {0}, User: '{1}': {2}", args.Get(0), sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			if (!int.TryParse(args.Get(1), out var unk1))
			{
				Log.Debug("HandleInteWarp: Failed to find parse by second arg '{0}': {1}", sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			if (!ZoneServer.Instance.World.NPCs.TryGetValue($"{warpData.ClassName}_{warpData.Zone}", out var npc))
			{
				Log.Debug("HandleInteWarp: Failed to find npc by class name '{0}': {1} : {2}", sender.Connection.Account.Name, commandName, warpData.ClassName);
				return CommandResult.Okay;
			}

			if (unk1 == 0 || unk1 == 1)
			{
				var mapId = npc.Map.Id;
				var newPosition = npc.Position.GetRelative(npc.Direction, 50);
				var newDirection = -npc.Direction;
				sender.SetDirection(newDirection);
				sender.Warp(mapId, newPosition);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to set Homunculus Skill
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleHomunculusSkill(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (!sender.TryGetBuff(BuffId.Homunculus_Skill_Buff, out var buff))
				return CommandResult.Okay;

			if (!Enum.TryParse<SkillId>(args.Get(0), out var skillId) || !sender.TryGetSkill(skillId, out var skill))
				return CommandResult.Okay;

			var skills = AlchemistSkillHelper.GetHomunculusSkills();
			if (!skills.Contains(skillId))
				return CommandResult.Okay;

			var skillIdF = (float)skill.Id;
			if (buff.NumArg2 == skillIdF || buff.NumArg3 == skillIdF || buff.NumArg4 == skillIdF || buff.NumArg5 == skillIdF)
				return CommandResult.Okay;

			if (buff.NumArg2 == 0)
				buff.NumArg2 = skillIdF;
			else if (buff.NumArg3 == 0)
				buff.NumArg3 = skillIdF;
			else if (buff.NumArg4 == 0)
				buff.NumArg4 = skillIdF;
			else if (buff.NumArg5 == 0)
				buff.NumArg5 = skillIdF;
			else
				return CommandResult.Okay;

			Send.ZC_BUFF_UPDATE(sender, buff);
			var homunculus = sender.Summons.GetSummons(s => s.Id == MonsterId.Homunculus).FirstOrDefault();
			if (homunculus != null)
				AlchemistSkillHelper.HomunculusSkillUpdate(sender, homunculus);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to save Sage Portal Skill Position
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSageSavePosition(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count != 0)
			{
				Log.Debug("HandleSageSavePosition: Invalid call by user '{0}': {1}", sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			// Check if user has Sage Job
			if (!sender.Jobs.Has(JobId.Sage))
				return CommandResult.Okay;

			if (!sender.Etc.Properties.Has(PropertyName.Sage_Portal_1)
				|| sender.Properties.GetString(PropertyName.Sage_Portal_1) == "None")
			{
				sender.SetEtcProperty(PropertyName.Sage_Portal_1, sender.GetLocationToString());
			}
			else if (!sender.Properties.Has(PropertyName.Sage_Portal_2)
				|| sender.Properties.GetString(PropertyName.Sage_Portal_2) == "None")
			{
				sender.SetEtcProperty(PropertyName.Sage_Portal_2, sender.GetLocationToString());
			}
			else if (!sender.Properties.Has(PropertyName.Sage_Portal_3)
				|| sender.Properties.GetString(PropertyName.Sage_Portal_3) == "None")
			{
				sender.SetEtcProperty(PropertyName.Sage_Portal_3, sender.GetLocationToString());
			}
			else
			{
				sender.SystemMessage("SageMaxSaveCnt");
				return CommandResult.Okay;
			}

			Send.ZC_EXEC_CLIENT_SCP(sender.Connection, ClientScripts.SAGE_PORTAL_SAVE_SUCCESS);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to delete Sage Portal Skill Position
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSageDeletePosition(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count == 0)
			{
				Log.Debug("HandleSageDeletePosition: Invalid call by user '{0}': {1}", sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			// Check if user has Sage Job
			if (!sender.Jobs.Has(JobId.Sage))
				return CommandResult.Okay;

			if (!int.TryParse(args.Get(0), out var position))
			{
				Log.Debug("HandleSageDeletePosition: Invalid position" +
					" '{0}' by user '{1}'.", position, sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			if (position < 1 || position > 3)
				return CommandResult.Okay;

			switch (position)
			{
				case 1:
					sender.SetEtcProperty(PropertyName.Sage_Portal_1, "None");
					break;
				case 2:
					sender.SetEtcProperty(PropertyName.Sage_Portal_2, "None");
					break;
				case 3:
					sender.SetEtcProperty(PropertyName.Sage_Portal_3, "None");
					break;
				default:
				{
					sender.SystemMessage("SageMaxSaveCnt");
					return CommandResult.Okay;
				}
			}

			Send.ZC_EXEC_CLIENT_SCP(sender.Connection, ClientScripts.SAGE_PORTAL_SAVE_SUCCESS);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to delete Sage Open Portal
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleSageOpenPortal(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Since this command is sent via UI interactions, we'll not
			// use any automated command result messages, but we'll leave
			// debug messages for now, in case of unexpected values.
			if (args.Count == 0)
			{
				Log.Debug("HandleSageOpenPortal: Invalid call by user '{0}': {1}", sender.Connection.Account.Name, commandName);
				return CommandResult.Okay;
			}

			// Check if user has Sage Job
			if (!sender.Jobs.Has(JobId.Sage))
				return CommandResult.Okay;

			if (!int.TryParse(args.Get(0), out var portalId))
			{
				Log.Debug("HandleSageOpenPortal: Invalid portal id '{0}' by user '{1}'.", portalId, sender.Connection.Account.Name);
				return CommandResult.Okay;
			}

			if (portalId < 1 || portalId > 3)
				return CommandResult.Okay;

			string portalPosition;
			switch (portalId)
			{
				case 1:
				{
					portalPosition = sender.Properties.GetString(PropertyName.Sage_Portal_1);
					sender.SetEtcProperty(PropertyName.Sage_Portal_1, portalPosition + "@" + DateTimeUtils.ToSPropertyDTNow);
				}
				break;
				case 2:
				{
					portalPosition = sender.Properties.GetString(PropertyName.Sage_Portal_2);
					sender.SetEtcProperty(PropertyName.Sage_Portal_2, portalPosition + "@" + DateTimeUtils.ToSPropertyDTNow);
				}
				break;
				case 3:
				{
					portalPosition = sender.Properties.GetString(PropertyName.Sage_Portal_3);
					sender.SetEtcProperty(PropertyName.Sage_Portal_3, portalPosition + "@" + DateTimeUtils.ToSPropertyDTNow);
				}
				break;
				default:
				{
					sender.SystemMessage("SageMaxSaveCnt");
					return CommandResult.Okay;
				}
			}

			Send.ZC_EXEC_CLIENT_SCP(sender.Connection, ClientScripts.SAGE_PORTAL_SAVE_SUCCESS);
			var location = portalPosition.Split('#');
			if (location.Length == 4)
			{
				var toMapData = ZoneServer.Instance.Data.MapDb.Find(location[0]);
				if (!float.TryParse(location[1], out var destinationX)
				|| !float.TryParse(location[2], out var destinationY)
				|| !float.TryParse(location[3], out var destinationZ))
				{
					Log.Debug("HandleSageOpenPortal: Failed to parse portal position '{0}' by user '{1}'.", portalPosition, sender.Connection.Account.Name);
					return CommandResult.Okay;
				}
				var warpPosition = new Position(destinationX, destinationY, destinationZ);
				if (!ZoneServer.Instance.World.TryGetMap(toMapData.Id, out var map)
					|| !map.Ground.IsValidPosition(warpPosition))
				{
					Log.Debug("HandleSageOpenPortal: Invalid portal position '{0}' by user '{1}'.", portalPosition, sender.Connection.Account.Name);
					return CommandResult.Okay;
				}

				var portal = new WarpMonster(MonsterId.MissionGate,
					new Location(sender.MapId, sender.Position),
					new Location(toMapData.Id, destinationX, destinationY, destinationZ),
					new Direction(1, 0));
				portal.AssociatedHandle = sender.Handle;
				portal.DialogName = "SAGE_WARP";
				portal.Properties[PropertyName.Scale] = 1;
				if (sender.Connection.Party != null)
				{
					portal.Visibility = ActorVisibility.Party;
					portal.VisibilityId = sender.Connection.Party.ObjectId;
				}
				else
				{
					portal.Visibility = ActorVisibility.Individual;
					portal.VisibilityId = sender.ObjectId;
				}
				// Remove portal after 15 seconds
				portal.DisappearTime = DateTime.Now.AddSeconds(15);
				//portal.Components.Add(new LifeTimeComponent(portal, TimeSpan.FromSeconds(15)));
				sender.Map.AddMonster(portal);
				// This is what makes the invisible npc look like a portal.
				portal.AttachEffect(AnimationName.Portal, 1, EffectLocation.Top);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Official slash command to Memo Portal
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleMemoPortal(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (sender.IsDead)
			{
				sender.ServerMessage($"Unable to use while dead.");
				return CommandResult.Okay;
			}

			if (sender.Map?.Data?.Type == MapType.Dungeon)
			{
				sender.ServerMessage($"Unable to use in a dungeon.");
				return CommandResult.Okay;
			}

			if (sender.Map?.Data?.Type == MapType.City)
			{
				sender.ServerMessage($"Unable to use in a city.");
				return CommandResult.Okay;
			}

			var location = sender.GetLocation().ToString();
			sender.ServerMessage($"Saved location {location} to memo.");
			sender.Variables.Perm.SetString("Laima.Portal", sender.GetLocationToString());

			return CommandResult.Okay;
		}

		/// <summary>
		/// Opens a portal to saved memo location
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleOpenPortal(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (sender.Map?.Data?.Type != MapType.City)
			{
				sender.ServerMessage($"You can only create a portal in a city.");
				return CommandResult.Okay;
			}

			var portalPosition = sender.Variables.Perm.GetString("Laima.Portal");
			if (string.IsNullOrEmpty(portalPosition))
			{
				sender.ServerMessage($"No portal location saved.");
				return CommandResult.Okay;
			}
			var location = portalPosition.Split('#');
			if (location.Length == 4)
			{
				var toMapData = ZoneServer.Instance.Data.MapDb.Find(location[0]);
				if (!float.TryParse(location[1], out var destinationX)
				|| !float.TryParse(location[2], out var destinationY)
				|| !float.TryParse(location[3], out var destinationZ))
				{
					Log.Debug("HandleOpenPortal: Failed to parse portal position '{0}' by user '{1}'.", portalPosition, sender.Connection.Account.Name);
					return CommandResult.Okay;
				}
				if (!ZoneServer.Instance.World.TryGetMap(toMapData.Id, out var map))
				{
					Log.Debug("HandleOpenPortal: Invalid portal position '{0}' by user '{1}'.", portalPosition, sender.Connection.Account.Name);
					return CommandResult.Okay;
				}

				var portalDestination = new Position(destinationX, destinationY, destinationZ);
				var portal = new WarpMonster(MonsterId.MissionGate,
					new Location(sender.MapId, sender.Position),
					new Location(toMapData.Id, portalDestination.GetRandomInRange2D(20, 30)),
					new Direction(1, 0),
					false);
				portal.AssociatedHandle = sender.Handle;
				portal.Name = $"{sender.TeamName}'s Portal";
				portal.Properties[PropertyName.Scale] = 1;
				if (sender.Connection.Party != null)
				{
					portal.Visibility = ActorVisibility.Party;
					portal.VisibilityId = sender.Connection.Party.ObjectId;
					portal.MaxUseCount = (short)sender.Connection.Party.MaxMemberCount;
				}
				else
				{
					portal.Visibility = ActorVisibility.Individual;
					portal.VisibilityId = sender.ObjectId;
					portal.MaxUseCount = 1;
				}
				// This is what makes the invisible npc look like a portal.
				portal.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
				// Remove portal after 30 seconds
				portal.DisappearTime = DateTime.Now.AddSeconds(30);
				//portal.Components.Add(new LifeTimeComponent(portal, TimeSpan.FromSeconds(15)));
				sender.Map.AddMonster(portal);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Updates the character's mouse position variables.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleUpdateMouse(Character sender, Character target, string message, string command, Arguments args)
		{
			sender.Variables.Temp.SetFloat("MouseX", float.Parse(args.Get(0), CultureInfo.InvariantCulture));
			sender.Variables.Temp.SetFloat("MouseY", float.Parse(args.Get(1), CultureInfo.InvariantCulture));
			sender.Variables.Temp.SetFloat("ScreenWidth", float.Parse(args.Get(2), CultureInfo.InvariantCulture));
			sender.Variables.Temp.SetFloat("ScreenHeight", float.Parse(args.Get(3), CultureInfo.InvariantCulture));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Fixes or unfixes target's camera position.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleFixCamera(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var isFixed = target.Variables.Temp.GetBool("Melia.Commands.FixedCamera", false);

			if (!isFixed)
			{
				Send.ZC_FIXCAMERA(target, target.Position, 0);
				sender.ServerMessage(Localization.Get("The camera was fixed in place."));
			}
			else
			{
				Send.ZC_CANCEL_FIXCAMERA(target);
				sender.ServerMessage(Localization.Get("The camera was unfixed."));
			}

			target.Variables.Temp.SetBool("Melia.Commands.FixedCamera", !isFixed);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Sets current in-game time and updates the day night cycle.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleDayTime(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (!Feature.IsEnabled(FeatureId.DayNightCycle))
			{
				sender.ServerMessage(Localization.Get("Day Night Cycle feature is disabled, please enable to use this command. '{0}'."), GameTime.Now.TimeOfDay);
				return CommandResult.Okay;
			}

			if (args.Count < 1)
			{
				ZoneServer.Instance.World.DayNightCycle.UnfixTimeOfDay();
				sender.ServerMessage(Localization.Get("Unfixed time of day, it's now '{0}'."), GameTime.Now.TimeOfDay);

				return CommandResult.Okay;
			}

			var timeOfDayStr = args.Get(0);
			timeOfDayStr = char.ToUpper(timeOfDayStr[0]) + timeOfDayStr.Substring(1).ToLower();

			if (!Enum.TryParse<TimeOfDay>(timeOfDayStr, out var timeOfDay))
			{
				sender.ServerMessage(Localization.Get("Invalid time of day."));
				return CommandResult.Okay;
			}

			ZoneServer.Instance.World.DayNightCycle.FixTimeOfDay(timeOfDay);
			sender.ServerMessage(Localization.Get("Fixed time of day to '{0}'."), timeOfDay);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Sets whether the target character will be saved on logout.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		private CommandResult NoSave(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				if (target.Variables.Temp.GetBool("Melia.NoSave", false))
					sender.ServerMessage(Localization.Get("The character is currently set to *not* be saved on logout."));
				else
					sender.ServerMessage(Localization.Get("The character is currently set to be saved on logout."));

				return CommandResult.Okay;
			}

			if (!bool.TryParse(args.Get(0), out var enabled))
				return CommandResult.InvalidArgument;

			target.Variables.Temp.SetBool("Melia.NoSave", enabled);

			if (enabled)
				sender.ServerMessage(Localization.Get("The character was set to *not* be saved on logout."));
			else
				sender.ServerMessage(Localization.Get("The character was set to be saved on logout."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Broadcasts message on the entire server.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleMic(Character sender, Character target, string message, string commandName, Arguments args)
		{
			// Check level requirement (level 40 or above)
			if (target.Level < 40)
			{
				sender.ServerMessage(Localization.Get("You must be level 40 or above to shout."));
				return CommandResult.Okay;
			}

			// Check shout cooldown (30 seconds between shouts)
			if (target.IsOnCooldown(CooldownId.Shout))
			{
				var cooldownComponent = target.Components.Get<CooldownComponent>();
				var remaining = cooldownComponent?.GetRemain(CooldownId.Shout) ?? TimeSpan.Zero;
				var secondsLeft = (int)Math.Ceiling(remaining.TotalSeconds);
				sender.ServerMessage(Localization.Get("You must wait {0} seconds before shouting again."), secondsLeft);
				return CommandResult.Okay;
			}

			var shoutText = string.Join(" ", args.GetAll());

			// Broadcast the message to all servers, so they can do whatever
			// with it. React to shouts on zones, put them on a web page, etc.
			ZoneServer.Instance.Communicator.Send("Coordinator", new ShoutMessage(target.TeamName, shoutText).BroadcastTo("AllServers"));

			// Start 30-second cooldown
			target.StartCooldown(CooldownId.Shout, TimeSpan.FromSeconds(30));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays tag invite for main chat.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleMainChat(Character sender, Character target, string message, string commandName, Arguments args)
		{
			target.ServerMessage("Click here to join the main chat: {a SLC 0@@@557516819791873}{#0000FF}{img link_whisper 24 24}Main{/}{/}{/}");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Modifies the target's medals/TP.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleMedals(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				sender.ServerMessage(Localization.Get("Current TP: {0}"), target.Connection.Account.Medals);
				return CommandResult.Okay;
			}

			if (!int.TryParse(args.Get(0), out var modifier))
				return CommandResult.InvalidArgument;

			var oldValue = target.Connection.Account.Medals;
			var newValue = Math.Max(0, oldValue + modifier);
			target.Connection.Account.Medals = newValue;

			sender.ServerMessage(Localization.Get("Modified TP ({1} -> {2})."), modifier, oldValue, newValue);

			if (sender != target)
				target.ServerMessage(Localization.Get("Your TP were modified by {0} ({1} -> {2})."), sender.TeamName, oldValue, newValue);

			Send.ZC_NORMAL.AccountProperties(target);

			return CommandResult.Okay;
		}


		/// <summary>
		/// Gets all items listed in a temp file.
		/// Item Ids should be listed once per line.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGetAllItemsFile(Character sender, Character target, string message, string command, Arguments args)
		{
			var filePath = Path.Combine(Path.GetTempPath(), "itemid_list.txt");

			if (!File.Exists(filePath))
			{
				sender.ServerMessage(Localization.Get("The itemsid_list.txt file was not found in the temp folder."));
				return CommandResult.Fail;
			}

			var addedCount = 0;
			var failedCount = 0;

			try
			{
				var itemIds = File.ReadAllLines(filePath);

				foreach (var line in itemIds)
				{
					if (int.TryParse(line.Trim(), out var itemId))
					{
						if (ZoneServer.Instance.Data.ItemDb.Contains(itemId))
						{
							var item = new Item(itemId);
							target.Inventory.Add(item, InventoryAddType.PickUp);
							addedCount++;
						}
						else
						{
							failedCount++;
							Log.Warning($"Item with ID {itemId} not found in the database.");
						}
					}
					else
					{
						failedCount++;
						Log.Warning($"Invalid item ID format: {line}");
					}
				}

				if (sender == target)
				{
					sender.ServerMessage(Localization.Get("Added {0} items to your inventory. Failed to add {1} items."), addedCount, failedCount);
				}
				else
				{
					target.ServerMessage(Localization.Get("{0} added {1} items to your inventory. Failed to add {2} items."), sender.TeamName, addedCount, failedCount);
					sender.ServerMessage(Localization.Get("Added {0} items to target's inventory. Failed to add {1} items."), addedCount, failedCount);
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error reading or processing the items file: {ex.Message}");
				sender.ServerMessage(Localization.Get("An error occurred while processing the items file."));
				return CommandResult.Fail;
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Generates all map images in temp folder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult HandleGenerateMapImages(Character sender, Character target, string message, string command, Arguments args)
		{
			foreach (var map in ZoneServer.Instance.World.Maps.GetList())
			{
				if ((map.Ground == null) || (!map.Ground.HasData()))
					continue;

				var path = Path.Combine(Path.GetTempPath(), "map_images", map.ClassName + ".png");
				Debug.DrawMap(path, map, 0.1f, 45);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Handles the pathfinding stress test command.
		/// Usage: /test pathfind [count] [mapId] [endX] [endZ]
		/// </summary>
		private CommandResult HandlePathfindTest(Character sender, Arguments args)
		{
			// 1. Define default parameters
			int count = 10;
			int mapId = 1021; // f_siauliai_west
			Position endPos = new Position(1632, 0, -733);

			// 2. Parse arguments
			if (args.Count >= 2 && !int.TryParse(args.Get(1), out count))
			{
				sender.ServerMessage("Invalid syntax. Usage: /test pathfind [count] [mapId] [endX] [endZ]");
				return CommandResult.Fail;
			}
			count = Math.Clamp(count, 1, 1000);

			if (args.Count == 5)
			{
				if (!int.TryParse(args.Get(2), out mapId) ||
					!float.TryParse(args.Get(3), out var endX) ||
					!float.TryParse(args.Get(4), out var endZ))
				{
					sender.ServerMessage("Invalid syntax for custom map/position. Usage: /test pathfind [count] [mapId] [endX] [endZ]");
					return CommandResult.Fail;
				}
				endPos = new Position(endX, 0, endZ);
			}
			else if (args.Count > 2 && args.Count != 5)
			{
				sender.ServerMessage("Invalid number of arguments. Use 0, 1, or 4 parameters after 'pathfind'.");
				return CommandResult.Fail;
			}

			// 3. Validate environment
			if (!ZoneServer.Instance.World.Maps.TryGet(mapId, out var targetMap) || !targetMap.Ground.HasData())
			{
				sender.ServerMessage($"Map with ID {mapId} is not loaded or has no ground data. Cannot run test.");
				return CommandResult.Fail;
			}

			if (!targetMap.Ground.TryGetHeightAt(endPos, out var endY))
			{
				sender.ServerMessage($"Could not find valid height for end position {endPos} on map {mapId}.");
				return CommandResult.Fail;
			}
			endPos.Y = endY;

			// 4. Stagger the spawning of mobs to prevent lag spikes
			sender.ServerMessage($"Spawning {count} Popolions on map {mapId} to pathfind to {endPos}. Please wait...");

			var spawnInterval = TimeSpan.FromMilliseconds(50); // 20 mobs per second
			var totalDelay = TimeSpan.Zero;

			for (int i = 0; i < count; i++)
			{
				// Use a timed event on the player to spawn each mob after a short delay
				sender.TimedEvents.Add(totalDelay, TimeSpan.Zero, 1, $"PathTestSpawn_{i}", _ =>
				{
					if (!targetMap.Ground.TryGetRandomPosition(out var startPos))
					{
						Log.Warning($"PathTest: Could not find a random valid start position on map {mapId}. Skipping one spawn.");
						return;
					}
					this.SpawnPathfindingTestMob(targetMap, startPos, endPos);
				});
				totalDelay += spawnInterval;
			}

			// Add a final message after all spawn events have been scheduled
			sender.TimedEvents.Add(totalDelay, TimeSpan.Zero, 1, "PathTestSpawnComplete", _ =>
			{
				sender.ServerMessage($"Finished queueing spawns for all {count} Popolions on map {mapId}.");
			});

			return CommandResult.Okay;
		}

		/// <summary>
		/// Spawns a single mob, tells it to pathfind to a destination, and schedules its cleanup.
		/// </summary>
		private void SpawnPathfindingTestMob(Map map, Position startPos, Position endPos)
		{
			var mob = new Mob(400981, RelationType.Neutral); // Popolion
			mob.Position = startPos;
			mob.SpawnPosition = startPos;
			mob.Tendency = TendencyType.Peaceful;

			var movement = new MovementComponent(mob);
			movement.ShowDebug = true;
			movement.ShowMinimapMarker = true;
			var timedEvents = new TimedEventComponent(mob);
			mob.Components.Add(timedEvents);
			mob.Components.Add(movement);

			map.AddMonster(mob);

			var moveTime = movement.MoveTo(endPos);

			if (moveTime > TimeSpan.Zero && moveTime < TimeSpan.FromMinutes(5))
			{
				// If path was found, schedule cleanup after the mob arrives.
				var cleanupTime = moveTime + TimeSpan.FromSeconds(3);
				timedEvents.Add(cleanupTime, TimeSpan.Zero, 1, "PathTestCleanup", _ =>
				{
					// Add a small delay for a final visual before despawning
					if (!mob.IsDead && mob.Map != null)
					{
						timedEvents.Add(TimeSpan.FromSeconds(1), TimeSpan.Zero, 1, "PathTestDespawn", __ =>
						{
							if (!mob.IsDead && mob.Map != null)
							{
								mob.Map.RemoveMonster(mob);
							}
						});
					}
				});
			}
			else
			{
				// If pathfinding failed or took an unreasonable amount of time, remove the mob immediately.
				map.RemoveMonster(mob);
			}
		}

		/// <summary>
		/// Identifies all unidentified items in inventory.
		/// </summary>
		private CommandResult HandleIdentify(Character sender, Character target, string message, string command, Arguments args)
		{
			var items = target.Inventory.GetItems();
			var identifiedCount = 0;

			foreach (var item in items.Values)
			{
				if (!item.NeedsAppraisal)
					continue;

				item.Appraisal();

				var itemGrade = (ItemGrade)(int)item.Properties.GetFloat(PropertyName.ItemGrade);
				if (itemGrade != ItemGrade.None && itemGrade != ItemGrade.Normal)
				{
					item.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
					item.GenerateGradeBasedRandomOptions();
				}

				Send.ZC_OBJECT_PROPERTY(target, item);
				identifiedCount++;
			}

			if (identifiedCount > 0)
				sender.ServerMessage(Localization.Get("Identified {0} item(s)."), identifiedCount);
			else
				sender.ServerMessage(Localization.Get("No unidentified items found."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Refines equipment at the specified slot.
		/// </summary>
		private CommandResult HandleRefine(Character sender, Character target, string message, string command, Arguments args)
		{
			if (args.Count < 2)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(0), out var slot) || slot < 0)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(1), out var amount))
				return CommandResult.InvalidArgument;

			var refinedCount = 0;

			if (slot == 0)
			{
				// Refine all equipped items
				var equippedItems = target.Inventory.GetEquip();
				foreach (var kvp in equippedItems)
				{
					var item = kvp.Value;
					if (item == null || item is DummyEquipItem || !item.IsRefinable)
						continue;

					var currentRefine = (int)item.Properties.GetFloat(PropertyName.Reinforce_2);
					var newRefine = Math.Clamp(currentRefine + amount, 0, 40);
					item.Properties.SetFloat(PropertyName.Reinforce_2, newRefine);
					Send.ZC_OBJECT_PROPERTY(target, item);
					refinedCount++;
				}

				if (refinedCount > 0)
					sender.ServerMessage(Localization.Get("Refined {0} item(s) by {1}."), refinedCount, amount);
				else
					sender.ServerMessage(Localization.Get("No refinable items equipped."));
			}
			else
			{
				// Refine specific slot
				var equipSlot = (EquipSlot)slot;
				var item = target.Inventory.GetEquip(equipSlot);

				if (item == null || item is DummyEquipItem)
				{
					sender.ServerMessage(Localization.Get("No item equipped in slot {0}."), slot);
					return CommandResult.Okay;
				}

				if (!item.IsRefinable)
				{
					sender.ServerMessage(Localization.Get("Item in slot {0} cannot be refined."), slot);
					return CommandResult.Okay;
				}

				var currentRefine = (int)item.Properties.GetFloat(PropertyName.Reinforce_2);
				var newRefine = Math.Clamp(currentRefine + amount, 0, 40);
				item.Properties.SetFloat(PropertyName.Reinforce_2, newRefine);
				Send.ZC_OBJECT_PROPERTY(target, item);

				sender.ServerMessage(Localization.Get("Refined {0} to +{1}."), item.Name, newRefine);
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Shows the contents of a cube/gacha item by dungeon name, group name, or item class name.
		/// </summary>
		private CommandResult HandleCubeInfo(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;

			// Combine all arguments into search term (supports "lanko lake" as two args)
			var searchTerm = string.Join(" ", Enumerable.Range(0, args.Count).Select(i => args.Get(i)));
			var groupName = searchTerm;
			string dungeonName = null;

			// First, try to find a dungeon by name (partial match, case-insensitive)
			var matchingDungeon = ZoneServer.Instance.Data.InstanceDungeonDb.Entries
				.Where(e => e.Value.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
				.Select(e => e.Value)
				.FirstOrDefault();

			if (matchingDungeon != null)
			{
				dungeonName = matchingDungeon.Name;

				// Get the reward item(s) from the dungeon
				if (matchingDungeon.RewardsItems.Count > 0)
				{
					var rewardItemData = matchingDungeon.RewardsItems.FirstOrDefault();

					if (rewardItemData != null && rewardItemData.Script != null && !string.IsNullOrEmpty(rewardItemData.Script.StrArg))
					{
						groupName = rewardItemData.Script.StrArg;
					}
					else
					{
						sender.ServerMessage($"Dungeon '{dungeonName}' reward item has no gacha group defined.");
						return CommandResult.Okay;
					}
				}
				else
				{
					sender.ServerMessage($"Dungeon '{dungeonName}' has no cube reward defined.");
					return CommandResult.Okay;
				}
			}
			// If the search term looks like an item class name (Gacha_*), try to find the group from item data
			else if (searchTerm.StartsWith("Gacha_", StringComparison.OrdinalIgnoreCase))
			{
				if (ZoneServer.Instance.Data.ItemDb.TryFind(searchTerm, out var itemData))
				{
					if (itemData.Script != null && !string.IsNullOrEmpty(itemData.Script.StrArg))
					{
						groupName = itemData.Script.StrArg;
					}
					else
					{
						sender.ServerMessage($"Item '{searchTerm}' found but has no gacha group defined.");
						return CommandResult.Okay;
					}
				}
				else
				{
					sender.ServerMessage($"Item '{searchTerm}' not found in item database.");
					return CommandResult.Okay;
				}
			}

			// Find all entries for this group
			var entries = ZoneServer.Instance.Data.CubeGachaDb.Entries
				.Where(e => e.Value.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase))
				.Select(e => e.Value)
				.OrderByDescending(e => e.Ratio)
				.ToList();

			if (entries.Count == 0)
			{
				sender.ServerMessage($"No gacha entries found for '{searchTerm}'.");
				return CommandResult.Okay;
			}

			// Calculate total ratio for percentage
			var totalRatio = entries.Sum(e => e.Ratio);

			var header = dungeonName != null
				? $"=== {dungeonName} Cube ({entries.Count} items) ==="
				: $"=== Cube Contents: {groupName} ({entries.Count} items) ===";
			sender.ServerMessage(header);

			var displayCount = 0;
			foreach (var entry in entries)
			{
				var percentage = (entry.Ratio / totalRatio) * 100;
				var itemName = entry.ItemName;

				// Try to get friendly item name
				if (ZoneServer.Instance.Data.ItemDb.TryFind(entry.ItemName, out var itemData))
					itemName = $"{itemData.Name} ({entry.ItemName})";

				var countStr = entry.Count > 1 ? $" x{entry.Count}" : "";
				sender.ServerMessage($"  {percentage:F2}% - {itemName}{countStr}");

				displayCount++;
				if (displayCount >= 50)
				{
					sender.ServerMessage($"  ... and {entries.Count - 50} more items (showing top 50)");
					break;
				}
			}

			sender.ServerMessage($"=== Total: {totalRatio:F2} ratio points ===");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Lists all available cube/gacha groups.
		/// </summary>
		private CommandResult HandleCubeList(Character sender, Character target, string message, string commandName, Arguments args)
		{
			var groups = ZoneServer.Instance.Data.CubeGachaDb.Entries
				.Select(e => e.Value.Group)
				.Distinct()
				.OrderBy(g => g)
				.ToList();

			sender.ServerMessage($"=== Available Cube Groups ({groups.Count} total) ===");

			// If a filter is provided, only show matching groups
			if (args.Count >= 1)
			{
				var filter = args.Get(0).ToLower();
				groups = groups.Where(g => g.ToLower().Contains(filter)).ToList();
				sender.ServerMessage($"Filtered by '{filter}': {groups.Count} matches");
			}

			var displayCount = 0;
			foreach (var group in groups)
			{
				var itemCount = ZoneServer.Instance.Data.CubeGachaDb.Entries.Count(e => e.Value.Group == group);
				sender.ServerMessage($"  {group} ({itemCount} items)");

				displayCount++;
				if (displayCount >= 30)
				{
					sender.ServerMessage($"  ... and {groups.Count - 30} more groups (showing first 30)");
					sender.ServerMessage($"  Use >cubelist <filter> to narrow results");
					break;
				}
			}

			return CommandResult.Okay;
		}
	}
}
