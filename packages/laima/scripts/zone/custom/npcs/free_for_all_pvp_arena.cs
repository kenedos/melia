using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Maps;
using Melia.Shared.Game.Const;
using Melia.Zone.Events.Arguments;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.World.Actors.Effects;
using Melia.Shared.Data.Database;
using Yggdrasil.Logging;
using static Melia.Zone.Scripting.Shortcuts;

/// <summary>
/// Script that manages a Free-For-All PvP arena where all players fight against each other
/// </summary>
public class FreeForAllPvPArenaScript : GeneralScript
{
	private readonly object _syncLock = new();
	private readonly List<Character> _playersInArena = new();
	private Map _pvpMap;
	private Dictionary<Character, DateTime> _exitingPlayers = new();

	/// <summary>
	/// Initializes the FFA PvP arena and sets up required NPCs
	/// </summary>
	protected override void Load()
	{
		if (!ZoneServer.Instance.World.TryGetMap("guild_mission_3_pvp", out var map))
		{
			Log.Error("FFA PvP Arena Script: Map 'guild_mission_3_pvp' not found.");
			return;
		}

		_pvpMap = map;
		_pvpMap.IsPVP = true;

		// Add entrance NPC in Klaipeda
		AddNpc(155066, L("[PvP Arena] Zack"), "c_Klaipe", 454, 14, 270, EntranceNpcDialog);
		AddNpc(155066, L("[PvP Arena] Zack"), "c_orsha", 138, -195, 270, EntranceNpcDialog);
		AddNpc(155066, L("[PvP Arena] Zack"), "c_fedimian", -582, -434, 90, EntranceNpcDialog);

		// Add exit NPCs in PvP map
		var npc = AddNpc(147384, L("[PvP Arena] Exit Portal"), "guild_mission_3_pvp", -960, -948, 0, ExitNpcDialog);
		npc.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
		npc = AddNpc(147384, L("[PvP Arena] Exit Portal"), "guild_mission_3_pvp", -965, 975, 0, ExitNpcDialog);
		npc.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
		npc = AddNpc(147384, L("[PvP Arena] Exit Portal"), "guild_mission_3_pvp", 959, 971, 0, ExitNpcDialog);
		npc.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
		npc = AddNpc(147384, L("[PvP Arena] Exit Portal"), "guild_mission_3_pvp", 951, -938, 0, ExitNpcDialog);
		npc.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));

		// Start timer to check exiting players
		StartExitTimer();
	}

	/// <summary>
	/// Handles dialog with entrance NPC
	/// </summary>
	private async Task EntranceNpcDialog(Dialog dialog)
	{
		var player = dialog.Player;

		dialog.SetTitle(L("Zack"));
		dialog.SetPortrait("Dlg_port_WielderJuanaut");

		var selection = await dialog.Select(L("Welcome to the Free-For-All Arena! Here, it's every warrior for themselves. Would you like to enter?"),
			Option(L("Enter Arena"), "enter"),
			Option(L("Cancel"), "cancel")
		);

		if (selection == "enter")
		{
			// Warp to center of PvP map
			if (_pvpMap.Ground.TryGetRandomPosition(out var position))
			{
				player.Warp(_pvpMap.Id, position);
			}
			else
			{
				player.Warp(_pvpMap.Id, _pvpMap.Data.DefaultPosition);
			}
		}
	}

	/// <summary>
	/// Handles interaction with exit NPC
	/// </summary>
	private async Task ExitNpcDialog(Dialog dialog)
	{
		var player = dialog.Player;

		dialog.SetTitle(L("Exit Portal"));

		var selection = await dialog.Select(L("Stand here for 10 seconds to exit the arena. Moving will cancel the exit process."),
			Option(L("Start Exit Timer"), "exit"),
			Option(L("Cancel"), "cancel")
		);

		if (selection == "exit")
		{
			lock (_syncLock)
			{
				_exitingPlayers[player] = DateTime.UtcNow;
			}
			await dialog.Msg(L("Don't move for 10 seconds..."));
		}
	}

	/// <summary>
	/// Sets up PvP state for a player entering the arena
	/// </summary>
	private void EnterBattle(Character player)
	{
		lock (_syncLock)
		{
			_playersInArena.Add(player);

			// Make player hostile to everyone already in arena
			foreach (var existingPlayer in _playersInArena.Where(p => p != player))
			{
				// Make players hostile to each other
				Send.ZC_NORMAL.FightState(player.Connection, existingPlayer, true);
				Send.ZC_NORMAL.FightState(existingPlayer.Connection, player, true);

				// Set enemy relation both ways
				Send.ZC_CHANGE_RELATION(player.Connection, existingPlayer.Handle, RelationType.Enemy);
				Send.ZC_CHANGE_RELATION(existingPlayer.Connection, player.Handle, RelationType.Enemy);
			}

			if (!player.IsDead)
				player.FullHeal();

			Send.ZC_FRIENDLY_STATE(player.Connection, true);
		}
	}

	/// <summary>
	/// Starts timer to check for players trying to exit
	/// </summary>
	private void StartExitTimer()
	{
		Task.Run(async () =>
		{
			while (true)
			{
				await Task.Delay(1000); // Check every second

				lock (_syncLock)
				{
					var now = DateTime.UtcNow;
					var playersToExit = new List<Character>();

					// Check each exiting player
					foreach (var kvp in _exitingPlayers.ToList())
					{
						var player = kvp.Key;
						var startTime = kvp.Value;

						// If player moved, cancel exit
						if (player.Movement.IsMoving)
						{
							_exitingPlayers.Remove(player);
							player.WorldMessage(L("Exit cancelled due to movement."));
							continue;
						}

						// If 10 seconds passed, add to exit list
						if ((now - startTime).TotalSeconds >= 10)
						{
							playersToExit.Add(player);
						}
					}

					// Process exits
					foreach (var player in playersToExit)
					{
						_exitingPlayers.Remove(player);
						_playersInArena.Remove(player);
						player.Warp("c_Klaipe", new Position(409, 85, 24));
					}
				}
			}
		});
	}

	/// <summary>
	/// Handles players entering the PvP map
	/// </summary>
	[On("PlayerEnteredMap")]
	private void OnPlayerEnteredMap(object sender, PlayerEventArgs args)
	{
		var player = args.Character;
		if (player.Map == _pvpMap)
		{
			EnterBattle(player);
		}
	}

	/// <summary>
	/// Handles players leaving the PvP map
	/// </summary>
	[On("PlayerLeftMap")]
	private void OnPlayerLeftMap(object sender, PlayerEventArgs args)
	{
		var player = args.Character;
		lock (_syncLock)
		{
			_playersInArena.Remove(player);
			_exitingPlayers.Remove(player);
		}
	}

	/// <summary>
	/// Cleanup when script is disposed
	/// </summary>
	public override void Dispose()
	{
		_playersInArena.Clear();
		_exitingPlayers.Clear();
		base.Dispose();
	}
}
