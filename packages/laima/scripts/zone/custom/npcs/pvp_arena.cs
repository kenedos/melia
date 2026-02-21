//--- Melia Script ----------------------------------------------------------
// PVP Arena Custom NPC
//--- Description -----------------------------------------------------------
// Allows players to enter the PVP Arena
//--- Details ---------------------------------------------------------------
// PVP Arena is a Team vs Team arena.
// Players are assigned to a Party and that is their team in the arena.
// Limit of players in the arena depends on party limit size.
//---------------------------------------------------------------------------

using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Maps;
using Melia.Zone.Scripting;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Events.Arguments;
using Yggdrasil.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.World;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using Melia.Zone;
using System;
using Yggdrasil.Logging;

public class CustomNpcPvpArena : GeneralScript
{
	private readonly object _syncLock = new();
	private List<Character> _playersInQueue = new();
	private bool battleStarted = false;
	private Party _teamA = null;
	private Party _teamB = null;
	private Map _pvpMap;
	private Timer _battleManagerTimer;

	protected override void Load()
	{
		if (!ZoneServer.Instance.World.TryGetMap("pvp_tournament", out var map))
		{
			Log.Error($"Pvp Arena Script: Map 'pvp_tournament' not found.");
			return;
		}

		_pvpMap = map;
		_pvpMap.IsPVP = true;
		_battleManagerTimer = new Timer(ManageBattles, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

		// AddNpc(155066, L("[PVP Arena] Zack"), "c_Klaipe", 454, 14, 270, NpcDialog);
		// AddNpc(155066, L("[PVP Arena] Zack"), "pvp_tournament", 322, 45, -243, NpcExitArenaDialog);
	}

	private async Task NpcExitArenaDialog(Dialog dialog)
	{
		var player = dialog.Player;

		dialog.SetTitle(L("Zack"));
		dialog.SetPortrait("WielderJuanaut");

		if (this.GetPlayersInBattle().Contains(player))
			return;

		var selection = await dialog.Select(L("Would you like to exit the PVP Arena?"),
			Option(L("Exit"), "yes"),
			Option(L("Cancel"), "end")
		);

		if (selection == "end")
			return;

		player.Warp("c_Klaipe", new Position(409, 85, 24));
	}

	private async Task NpcDialog(Dialog dialog)
	{
		var player = dialog.Player;

		dialog.SetTitle(L("Zack"));
		dialog.SetPortrait("Dlg_port_WielderJuanaut");

		var selection = await dialog.Select(L("Are you ready to battle for glory in the PVP Arena?"),
			Option(L("Battle!"), "battle"),
			Option(L("Spectate"), "spectate"),
			Option(L("Cancel"), "end")
		);

		if (selection == "end")
		{
			return;
		}

		if (selection == "spectate")
		{
			var spectatePositions = new List<Position>();
			spectatePositions.Add(new Position(509, 110, -44));
			spectatePositions.Add(new Position(309, 110, -364));
			spectatePositions.Add(new Position(-23, 110, -542));

			player.Warp(_pvpMap.Id, spectatePositions.Random());
			return;
		}

		if (battleStarted)
		{
			await dialog.Msg(L("There is a battle currently going on, please wait a moment!"));
			return;
		}

		lock (_syncLock)
		{
			var maxPartyMemberCount = Party.GetDefaultMaxMemberCount();

			if (_playersInQueue.Count < maxPartyMemberCount * 2)
				_playersInQueue.Add(player);
		}

		await dialog.Msg(L("You've been added to the queue. Please wait for the battle to start."));
	}

	private void ManageBattles(object state)
	{
		lock (_syncLock)
		{
			if (!battleStarted && _playersInQueue.Count >= 2)
			{
				StartBattle();
			}

			if (battleStarted)
			{
				CheckBattle();
			}
		}
	}

	private void StartBattle()
	{
		lock (_syncLock)
		{
			// Warp players to pvp map
			var playerTeam = 0;
			var partyManager = ZoneServer.Instance.World.Parties;
			foreach (var player in _playersInQueue)
			{
				// Team A
				if (playerTeam % 2 == 0)
				{
					// Leave previous party
					var party = player.Connection.Party;
					if (party != null)
					{
						party.RemoveMember(player);
						if (party.MemberCount == 0)
							partyManager.Delete(party);
					}

					// Enter new party
					if (_teamA == null)
						_teamA = partyManager.Create(player);
					else
						_teamA.AddMember(player);

					player.Warp(_pvpMap.Id, new Position(157, -31, 138));
				}
				// Team B
				else
				{
					// Leave previous party
					var party = player.Connection.Party;
					if (party != null)
					{
						party.RemoveMember(player);
						if (party.MemberCount == 0)
							partyManager.Delete(party);
					}

					// Enter new party
					if (_teamB == null)
						_teamB = partyManager.Create(player);
					else
						_teamB.AddMember(player);

					player.Warp(_pvpMap.Id, new Position(-156, -31, -221));
				}

				playerTeam++;
			}

			// Wait for all players to load the map
			var maxWaitTime = 30000;
			var sleepInterval = 1000;
			var stopwatch = Stopwatch.StartNew();
			while (stopwatch.ElapsedMilliseconds < maxWaitTime)
			{
				if (GetPlayersInBattle().All(player => player.Map == _pvpMap))
					break;

				Thread.Sleep(sleepInterval);
			}

			// Battle Start
			battleStarted = true;
			_playersInQueue.Clear();
			foreach (var player in _pvpMap.GetCharacters())
			{
				this.EnterBattle(player);
			}
		}
	}

	private void EnterBattle(Character player)
	{
		player.FullHeal();
		Send.ZC_FRIENDLY_STATE(player.Connection, true);

		foreach (var enemy in _pvpMap.GetCharacters())
		{
			if (enemy.Connection.Party == player.Connection.Party)
				continue;

			if (enemy == player)
				continue;

			Send.ZC_NORMAL.FightState(player.Connection, enemy, true);
			Send.ZC_CHANGE_RELATION(player.Connection, enemy.Handle, RelationType.Enemy);
			Send.ZC_TEAMID(player.Connection, enemy, 1);
			Send.ZC_TEAMID(enemy.Connection, player, 2);
		}
	}

	private void CheckBattle()
	{
		if (!battleStarted)
			return;

		var playersInMap = _pvpMap.GetCharacters();

		// No players in map, what happened?!
		if (!playersInMap.Any())
		{
			battleStarted = false;
			return;
		}

		// Counts how many players in map for each team
		// Note: Players can be in party but not in pvp map anymore
		var battlingPlayersInTeamA = playersInMap.Count(player => player.Connection.Party == _teamA && !player.IsDead);
		var battlingPlayersInTeamB = playersInMap.Count(player => player.Connection.Party == _teamB && !player.IsDead);

		if (battlingPlayersInTeamA == 0)
		{
			// Team B Wins
			WinBattle(_teamB);
		}
		else if (battlingPlayersInTeamB == 0)
		{
			// Team A Wins
			WinBattle(_teamA);
		}
	}

	private void WinBattle(Party team)
	{
		lock (_syncLock)
		{
			if (!battleStarted)
				return;

			foreach (var player in this.GetPlayersInBattle())
			{
				if (player.Connection.Party == team)
				{
					// TODO: Send packet 'You win!'
				}
				else
				{
					// TODO: Send packet 'You lose!'
				}
			}

			Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(_ => { FinishBattle(); });
		}
	}

	private void FinishBattle()
	{
		foreach (var player in _pvpMap.GetCharacters())
		{
			player.FullHeal();
			player.Warp("c_Klaipe", new Position(409, 85, 24));
		}

		// Do not break parties, but lose the reference to them
		_teamA = null;
		_teamB = null;
		battleStarted = false;
	}

	private List<Character> GetPlayersInBattle()
	{
		var list = new List<Character>();

		if (!battleStarted)
			return list;

		list.AddRange(_teamA.GetPartyMembers());
		list.AddRange(_teamB.GetPartyMembers());
		return list;
	}

	public override void Dispose()
	{
		_battleManagerTimer?.Dispose();
		base.Dispose();
	}


	[On("PlayerLeftParty")]
	private void CheckForfeit(object sender, PlayerEventArgs args)
	{
		if (!battleStarted)
			return;

		lock (_syncLock)
		{
			var player = args.Character;

			if (this.GetPlayersInBattle().Contains(player))
			{
				player.Warp("c_Klaipe", new Position(409, 85, 24));
			}
		}
	}

	[On("EntityKilled")]
	private void CheckKill(object sender, CombatEventArgs args)
	{
		if (!battleStarted)
			return;

		if ((args.Target is not Character) || (args.Attacker is not Character))
			return;

		lock (_syncLock)
		{
			var target = args.Target as Character;
			var attacker = args.Attacker as Character;

			var targetParty = ZoneServer.Instance.World.Parties.GetParty(target.PartyId);
			var attackerParty = ZoneServer.Instance.World.Parties.GetParty(attacker.PartyId);

			if ((targetParty == _teamA) && (attackerParty == _teamB))
			{
				// TODO: Announce "Player has been killed"
				this.CheckBattle();
			}
			else if ((targetParty == _teamB) && (attackerParty == _teamA))
			{
				// TODO: Announce "Player has been killed"
				this.CheckBattle();
			}
		}
	}

	[On("PlayerLeftMap")]
	private void CheckPlayerLeftQueue(object sender, PlayerEventArgs args)
	{
		var player = args.Character;
		_playersInQueue.Remove(player);
	}
}
