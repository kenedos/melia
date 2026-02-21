using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Network;
using Melia.Shared.Network.Inter.Messages;
using Yggdrasil.Logging;
using Yggdrasil.Util.Commands;

namespace Melia.Barracks.Util
{
	public class BarracksConsoleCommands : ConsoleCommands
	{
		public BarracksConsoleCommands()
		{
			this.Add("auth", "<account> <level>", "Changes authority level of account", this.HandleAuth);
			this.Add("passwd", "<account> <password>", "Changes password of account", this.HandlePasswd);
			this.Add("shutdown", "[minutes|now] [reason]", "Schedules a shutdown on all servers (default: 15 minutes).", this.HandleShutdown);
			this.Add("cancelshutdown", "", "Cancels a pending shutdown on all servers.", this.HandleCancelShutdown);
			this.Add("status", "", "Shows the status of all connected servers.", this.HandleStatus);
		}

		private CommandResult HandleAuth(string command, Arguments args)
		{
			if (args.Count < 3)
				return CommandResult.InvalidArgument;

			var accountName = args.Get(1);

			if (!int.TryParse(args.Get(2), out var level))
				return CommandResult.InvalidArgument;

			if (!BarracksServer.Instance.Database.AccountExists(accountName))
			{
				Log.Error("Please specify an existing account.");
				return CommandResult.Okay;
			}

			if (!BarracksServer.Instance.Database.ChangeAuth(accountName, level))
			{
				Log.Error("Failed to change auth.");
				return CommandResult.Okay;
			}

			Log.Info("Changed auth successfully.");

			return CommandResult.Okay;
		}

		private CommandResult HandlePasswd(string command, Arguments args)
		{
			if (args.Count < 3)
			{
				return CommandResult.InvalidArgument;
			}

			var accountName = args.Get(1);
			var password = args.Get(2);

			if (!BarracksServer.Instance.Database.AccountExists(accountName))
			{
				Log.Error("Please specify an existing account.");
				return CommandResult.Okay;
			}

			BarracksServer.Instance.Database.SetAccountPassword(accountName, password);

			Log.Info("Password change for {0} complete.", accountName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Handles the shutdown command.
		/// Usage: shutdown [minutes|now] [reason]
		/// </summary>
		private CommandResult HandleShutdown(string command, Arguments args)
		{
			var delayMinutes = 15;
			var reasonStartIndex = 1;
			var immediate = false;

			if (args.Count >= 2)
			{
				var firstArg = args.Get(1).ToLowerInvariant();

				if (firstArg == "now" || firstArg == "immediate")
				{
					immediate = true;
					reasonStartIndex = 2;
				}
				else if (int.TryParse(firstArg, out var parsedMinutes))
				{
					if (parsedMinutes < 0)
					{
						Log.Warning("Shutdown delay cannot be negative.");
						return CommandResult.InvalidArgument;
					}

					if (parsedMinutes == 0)
						immediate = true;
					else
						delayMinutes = parsedMinutes;

					reasonStartIndex = 2;
				}
			}

			var reasonParts = args.GetAll().Skip(reasonStartIndex).ToList();
			var reason = reasonParts.Count > 0
				? string.Join(" ", reasonParts)
				: "maintenance";

			ShutdownMessage message;

			if (immediate)
			{
				Log.Info("Broadcasting IMMEDIATE shutdown to all servers. Reason: {0}", reason);
				message = ShutdownMessage.Immediate(reason, "Barracks Console");
			}
			else
			{
				Log.Info("Broadcasting GRACEFUL shutdown to all servers. Delay: {0} minute(s), Reason: {1}", delayMinutes, reason);
				message = ShutdownMessage.Graceful(reason, delayMinutes, "Barracks Console");
			}

			BarracksServer.Instance.Communicator.Broadcast("AllServers", message);

			if (!immediate)
			{
				Log.Info("Use 'cancelshutdown' to abort the shutdown on all servers.");
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Handles cancelling a pending shutdown on all servers.
		/// </summary>
		private CommandResult HandleCancelShutdown(string command, Arguments args)
		{
			Log.Info("Broadcasting shutdown CANCELLATION to all servers.");

			var message = ShutdownMessage.Cancel("Barracks Console");
			BarracksServer.Instance.Communicator.Broadcast("AllServers", message);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Shows the status of all connected servers.
		/// </summary>
		private new CommandResult HandleStatus(string command, Arguments args)
		{
			var servers = BarracksServer.Instance.ServerList.GetAll();

			Log.Info("=== Server Status ===");

			var grouped = servers.GroupBy(s => s.Type).OrderBy(g => g.Key);

			foreach (var group in grouped)
			{
				Log.Info("{0} Servers:", group.Key);
				foreach (var server in group.OrderBy(s => s.Id))
				{
					var statusColor = server.Status == ServerStatus.Online ? "Online" : "Offline";
					var playerInfo = server.Type == ServerType.Zone
						? $", Players: {server.CurrentPlayers}"
						: "";

					Log.Info("  [{0}] {1} - {2}{3}",
						server.Id,
						$"{server.Type}:{server.Id}",
						statusColor,
						playerInfo);
				}
			}

			var totalPlayers = servers
				.Where(s => s.Type == ServerType.Zone)
				.Sum(s => s.CurrentPlayers);

			Log.Info("Total players online: {0}", totalPlayers);
			Log.Info("=====================");

			return CommandResult.Okay;
		}
	}
}
