using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Database;
using Melia.Shared.L10N;
using Melia.Shared.Network;
using Melia.Shared.Network.Inter.Messages;
using Melia.Zone.Network;
using Yggdrasil.Logging;
using Yggdrasil.Util.Commands;

namespace Melia.Zone.Util
{
	public class ZoneConsoleCommands : ConsoleCommands
	{
		public ZoneConsoleCommands()
		{
			this.Add("banip", "<ip>", "Bans an IP Address", this.HandleBanIP);
			this.Add("shutdown", "[minutes] [reason]", "Schedules a graceful server shutdown (default: 15 minutes). Use 'now' for immediate shutdown.", this.HandleShutdown);
			this.Add("cancelshutdown", "", "Cancels a pending server shutdown.", this.HandleCancelShutdown);
			this.Add("shutdownstatus", "", "Shows the status of any pending shutdown.", this.HandleShutdownStatus);
			this.Add("profiling", "[on|off]", "Enables, disables, or toggles performance profiling logs.", this.HandleToggleProfiling);
			this.Add("who", "", "Shows the current number of online players.", this.HandlePlayersOnline);
			this.Add("playersonline", "", "Shows the current number of online players (alias for 'who').", this.HandlePlayersOnline);
			this.Add("uptime", "", "Shows how long the server has been running.", this.HandleUptime);
			this.Add("features", "[all]", "Shows enabled features. Use 'all' to show the full tree.", this.HandleShowFeatures);
		}

		/// <summary>
		/// Handles the shutdown command with graceful countdown.
		/// Usage: shutdown [minutes|now] [reason]
		/// </summary>
		private CommandResult HandleShutdown(string command, Arguments args)
		{
			var shutdownManager = ServerShutdownManager.Instance;

			if (shutdownManager.IsShutdownPending)
			{
				Log.Warning("A shutdown is already pending. Use 'cancelshutdown' to cancel it first, or 'shutdownstatus' to check status.");
				return CommandResult.Okay;
			}

			var delayMinutes = 15; // Default delay
			var reasonStartIndex = 1; // Start after command name (args.Get(0) is "shutdown")

			// Parse delay argument (args.Get(1) is first actual argument)
			if (args.Count >= 2)
			{
				var firstArg = args.Get(1).ToLowerInvariant();

				if (firstArg == "now" || firstArg == "immediate")
				{
					delayMinutes = 0;
					reasonStartIndex = 2;
				}
				else if (int.TryParse(firstArg, out var parsedMinutes))
				{
					if (parsedMinutes < 0)
					{
						Log.Warning("Shutdown delay cannot be negative.");
						return CommandResult.InvalidArgument;
					}

					delayMinutes = parsedMinutes;
					reasonStartIndex = 2;
				}
			}

			// Build reason message (skip command name and optional time argument)
			var reasonParts = args.GetAll().Skip(reasonStartIndex).ToList();
			var reason = reasonParts.Count > 0
				? string.Join(" ", reasonParts)
				: "maintenance";

			if (!shutdownManager.ScheduleShutdown(delayMinutes, reason, "Console"))
			{
				Log.Warning("Failed to schedule shutdown.");
				return CommandResult.Fail;
			}

			if (delayMinutes > 0)
			{
				Log.Info("Use 'cancelshutdown' to abort, or 'shutdownstatus' to check remaining time.");
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Handles cancelling a pending shutdown.
		/// </summary>
		private CommandResult HandleCancelShutdown(string command, Arguments args)
		{
			var shutdownManager = ServerShutdownManager.Instance;

			if (!shutdownManager.IsShutdownPending)
			{
				Log.Info("No shutdown is currently pending.");
				return CommandResult.Okay;
			}

			if (shutdownManager.CancelShutdown("Console"))
			{
				Log.Info("Shutdown has been cancelled.");
				return CommandResult.Okay;
			}
			else
			{
				Log.Error("Failed to cancel shutdown.");
				return CommandResult.Fail;
			}
		}

		/// <summary>
		/// Shows the status of any pending shutdown.
		/// </summary>
		private CommandResult HandleShutdownStatus(string command, Arguments args)
		{
			var shutdownManager = ServerShutdownManager.Instance;

			if (!shutdownManager.IsShutdownPending)
			{
				Log.Info("No shutdown is currently pending.");
				return CommandResult.Okay;
			}

			var remaining = shutdownManager.GetRemainingTime();
			if (!remaining.HasValue || remaining.Value.TotalSeconds <= 0)
			{
				Log.Info("Shutdown is imminent (executing now).");
			}
			else
			{
				Log.Info($"Shutdown scheduled in: {shutdownManager.GetRemainingTimeFormatted()}");
				Log.Info($"Scheduled time: {shutdownManager.ScheduledShutdownTime:yyyy-MM-dd HH:mm:ss}");
				Log.Info($"Reason: {shutdownManager.ShutdownReason}");
			}

			return CommandResult.Okay;
		}

		private CommandResult HandleBanIP(string command, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;

			var ip = args.Get(0);

			ZoneServer.Instance.Database.BanIp(ip, DateTime.Now, DateTime.MaxValue, "Manual Ban");
			Log.Info($"Banned IP mask {ip} successfully.");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Handles the 'toggleprofiling' console command to enable or disable
		/// performance logging at runtime.
		/// </summary>
		private CommandResult HandleToggleProfiling(string command, Arguments args)
		{
			if (args.Count < 1)
			{
				// If no argument is provided, just toggle the current state.
				Debug.IsProfilingEnabled = !Debug.IsProfilingEnabled;
			}
			else
			{
				var state = args.Get(0).ToLowerInvariant();
				if (state == "on" || state == "true" || state == "1")
				{
					Debug.IsProfilingEnabled = true;
				}
				else if (state == "off" || state == "false" || state == "0")
				{
					Debug.IsProfilingEnabled = false;
				}
				else
				{
					Log.Info("Invalid argument. Usage: profiling [on|off]");
					return CommandResult.InvalidArgument;
				}
			}

			Log.Info($"Performance profiling is now {(Debug.IsProfilingEnabled ? "ENABLED" : "DISABLED")}.");
			return CommandResult.Okay;
		}

		private CommandResult HandleUptime(string command, Arguments args)
		{
			var uptime = ZoneServer.Instance.World.WorldTime.Elapsed;
			Log.Info($"Server uptime: {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Handles the 'who' and 'playersonline' console command.
		/// </summary>
		private CommandResult HandlePlayersOnline(string command, Arguments args)
		{
			var playerCount = ZoneServer.Instance.World.GetCharacterCount();
			Log.Info($"There are currently {playerCount} players online.");
			var players = ZoneServer.Instance.World.GetCharacters();
			foreach (var player in players)
			{
				if (player.Connection == null || player.Connection.Account == null || player.Connection is DummyConnection)
					continue;
				var mapName = player.Map?.Data?.Name ?? "Unknown";
				var pos = player.Position;
				Log.Info($"Account: {player.Connection.Account.Name} - Character: {player.Name} (Level {player.Level} {player.Job?.Data.Name ?? "Unknown"}) - Map: {mapName} ({pos.X:0}, {pos.Y:0}, {pos.Z:0})");
			}
			return CommandResult.Okay;
		}

		/// <summary>
		/// Handles the 'features' console command to display the status of configured game features.
		/// </summary>
		private CommandResult HandleShowFeatures(string command, Arguments args)
		{
			var showAll = args.Count > 0 && args.Get(0).ToLowerInvariant() == "all";

			var featureDb = ZoneServer.Instance.Data.FeatureDb;
			if (featureDb == null || featureDb.Entries.Count == 0)
			{
				Log.Info("Feature database is not loaded or is empty.");
				return CommandResult.Okay;
			}

			Log.Info($"--- Feature Status ({(showAll ? "All" : "Enabled Only")}) ---");

			var rootNodes = featureDb.Entries.Values.Where(n => n.Parent == null).OrderBy(n => n.Name);

			if (!rootNodes.Any())
			{
				Log.Info("No root feature nodes found.");
			}

			foreach (var rootNode in rootNodes)
			{
				this.PrintFeatureNode(rootNode, "", true, showAll);
			}

			Log.Info("--- End of Feature Status ---");

			return CommandResult.Okay;
		}

		private void PrintFeatureNode(FeatureNode node, string indent, bool isRoot, bool showAll)
		{
			// If we only show enabled features, and this node is disabled, we don't process it or its children.
			// The FeatureDb logic ensures all children of a disabled node are also disabled.
			if (!showAll && !node.Enabled)
			{
				return;
			}

			Console.Write(indent);
			if (!isRoot)
			{
				Console.Write("- ");
			}

			Console.ForegroundColor = node.Enabled ? ConsoleColor.Green : ConsoleColor.Red;
			Console.WriteLine(node.Name);
			Console.ResetColor();

			foreach (var child in node.Children.OrderBy(c => c.Name))
			{
				this.PrintFeatureNode(child, indent + "  ", false, showAll);
			}
		}
	}
}
