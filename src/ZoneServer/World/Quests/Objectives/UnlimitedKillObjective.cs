using System;
using System.Collections.Generic;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Groups;

namespace Melia.Zone.World.Quests.Objectives
{
	/// <summary>
	/// A kill-tracking objective that never marks itself complete. Used for
	/// repeatable bounty-style quests where the NPC consumes the accumulated
	/// kill count for rewards and resets the remainder.
	/// </summary>
	public class UnlimitedKillObjective : QuestObjective
	{
		private readonly Func<Mob, Character, bool> _matches;

		/// <summary>
		/// Creates a new objective that increments its progress whenever the
		/// player kills a monster matching the given predicate.
		/// </summary>
		public UnlimitedKillObjective(Func<Mob, Character, bool> matches)
		{
			this.TargetCount = int.MaxValue;
			_matches = matches ?? throw new ArgumentNullException(nameof(matches));
		}

		public override void Load()
		{
			ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe(this.OnEntityKilled);
		}

		public override void Unload()
		{
			ZoneServer.Instance.ServerEvents.EntityKilled.Unsubscribe(this.OnEntityKilled);
		}

		private void OnEntityKilled(object sender, CombatEventArgs args)
		{
			if (args.Target is not Mob mob)
				return;

			var character = mob.GetKillBeneficiary(args.Attacker);
			if (character == null)
				return;

			foreach (var eligible in this.GetEligibleCharacters(character))
				this.UpdateProgress(eligible, mob);
		}

		private List<Character> GetEligibleCharacters(Character killer)
		{
			var result = new List<Character> { killer };

			if (!ZoneServer.Instance.Conf.World.PartyQuestSharingEnabled)
				return result;

			if (!ZoneServer.Instance.Conf.World.PartyShareKillObjectives)
				return result;

			var party = killer.Connection?.Party;
			if (party == null)
				return result;

			if (party.QuestSharing != PartyQuestSharing.Enabled)
				return result;

			var sharingRange = ZoneServer.Instance.Conf.World.PartyQuestSharingRange;

			List<Character> partyMembers = sharingRange <= 0
				? killer.Map.GetPartyMembers(killer)
				: killer.Map.GetPartyMembersInRange(killer, sharingRange);

			foreach (var member in partyMembers)
			{
				if (member != killer)
					result.Add(member);
			}

			return result;
		}

		private void UpdateProgress(Character character, Mob mob)
		{
			character.Quests.UpdateObjectives<UnlimitedKillObjective>((quest, objective, progress) =>
			{
				if (objective != this)
					return;

				if (!_matches(mob, character))
					return;

				if (progress.Count < int.MaxValue)
					progress.Count++;

				character.Quests.UpdateQuestProgress(quest.Data.Id.Value, objective.Id);
			});
		}

	}
}
