using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Trades;

namespace Melia.Zone.World
{
	public class DuelManager
	{
		/// <summary>
		/// Sends relation change packets for a character's owned entities (companions and summons)
		/// to another character by re-sending the monster enter packet with updated MonsterType.
		/// </summary>
		/// <param name="owner">The owner of the companions/summons</param>
		/// <param name="viewer">The character who should see the relation change</param>
		/// <param name="relation">The relation type to set</param>
		public static void SendOwnedEntitiesRelation(Character owner, Character viewer, RelationType relation)
		{
			if (viewer.Connection == null)
				return;

			// Send relation for active companion
			if (owner.ActiveCompanion != null && owner.ActiveCompanion.IsActivated)
			{
				RefreshMonsterRelationForViewer(owner.ActiveCompanion, viewer, relation);
			}

			// Send relation for all summons
			var summons = owner.Summons.GetSummons();
			foreach (var summon in summons)
			{
				RefreshMonsterRelationForViewer(summon, viewer, relation);
			}
		}

		/// <summary>
		/// Refreshes a monster's appearance for a specific viewer with the given relation type.
		/// This works by temporarily changing the monster's MonsterType and re-sending
		/// the ZC_ENTER_MONSTER packet to the viewer, plus updating the faction.
		/// </summary>
		/// <param name="monster">The monster to refresh</param>
		/// <param name="viewer">The character who should see the updated relation</param>
		/// <param name="relation">The relation type to display</param>
		private static void RefreshMonsterRelationForViewer(IMonster monster, Character viewer, RelationType relation)
		{
			if (viewer.Connection == null || monster.Map == null)
				return;

			if (monster is Mob mob)
			{
				var originalType = mob.MonsterType;
				mob.MonsterType = relation;

				Send.ZC_LEAVE(viewer.Connection, monster);
				Send.ZC_ENTER_MONSTER(viewer.Connection, monster);

				mob.MonsterType = originalType;
			}
		}

		private readonly Dictionary<long, Duel> _duels = new Dictionary<long, Duel>();

		public bool IsInDuel(Character character)
		{
			lock (_duels)
				return _duels.Values.Any(duel => duel.GetParticipants().Contains(character));
		}

		public bool AreDueling(Character dueler1, Character dueler2)
		{
			lock (_duels)
				return _duels.Values.Any(duel => duel.AreDueling(dueler1, dueler2));
		}

		public Duel GetDuel(Character character)
		{
			lock (_duels)
				return _duels.Values.FirstOrDefault(duel => duel.GetParticipants().Contains(character));
		}

		public Duel CreateDuel(Character requester, Character receiver)
		{
			var duel = new Duel(requester, receiver);
			_duels[duel.Id] = duel;
			return duel;
		}

		public Duel CreateTeamDuel(List<Character> team1, List<Character> team2)
		{
			var duel = new Duel(team1, team2);
			_duels[duel.Id] = duel;
			return duel;
		}

		/// <summary>
		/// Sends a duel request between two characters, the sender and recipient.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipient"></param>
		public void RequestDuel(Character sender, Character recipient)
		{
			if (sender.IsDueling || recipient.IsDueling)
				return;

			if (sender.IsDead || recipient.IsDead)
				return;

			if (sender.IsTrading || recipient.IsTrading)
				return;

			Send.ZC_NORMAL.RequestDuel(recipient.Connection, sender);
		}

		public void StartDuel(Duel duel)
		{
			foreach (var participant in duel.GetParticipants())
			{
				participant.Connection.ActiveDuel = duel;

				Send.ZC_FRIENDLY_STATE(participant.Connection, true);
				foreach (var otherParticipant in duel.GetParticipants().Where(p => p != participant))
				{
					Send.ZC_NORMAL.FightState(participant.Connection, otherParticipant, true);
					Send.ZC_CHANGE_RELATION(participant.Connection, otherParticipant.Handle, RelationType.Enemy);

					// Send relation change for opponent's companions and summons
					SendOwnedEntitiesRelation(otherParticipant, participant, RelationType.Enemy);

					if (duel.IsTeamDuel)
					{
						Send.ZC_TEAMID(participant.Connection, otherParticipant, 1);
						Send.ZC_TEAMID(otherParticipant.Connection, participant, 2);
					}
				}
			}
		}

		public void EndDuel(Duel duel, ICombatEntity winnerEntity = null, ICombatEntity loserEntity = null)
		{
			if (duel == null)
				return;
			if (winnerEntity is Character winner && loserEntity is Character loser)
			{
				ZoneServer.Instance.Database.SaveDuelResult(winner.DbId, loser.DbId);
			}
			foreach (var participant in duel.GetParticipants())
			{
				participant.Connection.ActiveDuel = null;

				Send.ZC_FRIENDLY_STATE(participant.Connection, false);
				foreach (var otherParticipant in duel.GetParticipants().Where(p => p != participant))
				{
					Send.ZC_NORMAL.FightState(participant.Connection, otherParticipant, false);
					Send.ZC_CHANGE_RELATION(participant.Connection, otherParticipant.Handle, RelationType.Neutral);

					// Reset relation for opponent's companions and summons
					SendOwnedEntitiesRelation(otherParticipant, participant, RelationType.Neutral);

					if (duel.IsTeamDuel)
					{
						Send.ZC_TEAMID(participant.Connection, otherParticipant, 0);
						Send.ZC_TEAMID(otherParticipant.Connection, participant, 0);
					}
				}
			}

			_duels.Remove(duel.Id);
		}
	}

	public class Duel
	{
		private static long NextId = 1;

		public long Id { get; }
		public List<Character> Team1 { get; }
		public List<Character> Team2 { get; }
		public bool IsTeamDuel { get; }

		public IReadOnlyList<Character> GetParticipants() => this.Team1.Concat(this.Team2).ToList();

		public Duel(Character requester, Character receiver)
		{
			this.Id = NextId++;
			this.Team1 = new List<Character>();
			this.Team1.Add(requester);
			this.Team2 = new List<Character>();
			this.Team2.Add(receiver);
			this.IsTeamDuel = false;
		}

		public Duel(List<Character> team1, List<Character> team2)
		{
			this.Id = NextId++;
			this.Team1 = team1;
			this.Team2 = team2;
			this.IsTeamDuel = true;
		}

		public bool AreDueling(Character character1, Character character2)
		{
			return this.Team1.Contains(character1) && this.Team2.Contains(character2) ||
				   this.Team2.Contains(character1) && this.Team1.Contains(character2);
		}

		public bool AreOnSameTeam(Character character1, Character character2)
		{
			return this.Team1.Contains(character1) && this.Team1.Contains(character2) ||
				   this.Team2.Contains(character1) && this.Team2.Contains(character2);
		}
	}
}
