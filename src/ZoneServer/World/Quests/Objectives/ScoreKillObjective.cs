using System;
using System.Collections.Generic;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Groups;

namespace Melia.Zone.World.Quests.Objectives
{
	/// <summary>
	/// Objective that fills a meter by the score each killed monster is worth.
	/// </summary>
	public class ScoreKillObjective : QuestObjective
	{
		private readonly Func<Mob, Character, int> _score;

		/// <summary>
		/// Creates an objective that completes once the scores of the
		/// monsters the character killed add up to the given target.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="score">Returns what a kill is worth, or 0 if it does not count.</param>
		public ScoreKillObjective(int target, Func<Mob, Character, int> score)
		{
			this.TargetCount = target;
			_score = score ?? throw new ArgumentNullException(nameof(score));
		}

		/// <summary>
		/// Sets up event subscriptions.
		/// </summary>
		public override void Load()
		{
			ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe(this.OnEntityKilled);
		}

		/// <summary>
		/// Cleans up event subscriptions.
		/// </summary>
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

			var group = killer.Tracks?.ActiveTrack?.Group;
			if (group != null)
			{
				foreach (var member in group.Members)
				{
					if (member != killer && member.Map == killer.Map && !result.Contains(member))
						result.Add(member);
				}
			}

			if (!ZoneServer.Instance.Conf.World.PartyQuestSharingEnabled || !ZoneServer.Instance.Conf.World.PartyShareKillObjectives)
				return result;

			var party = killer.Connection?.Party;
			if (party == null || party.QuestSharing != PartyQuestSharing.Enabled)
				return result;

			var sharingRange = ZoneServer.Instance.Conf.World.PartyQuestSharingRange;

			var partyMembers = sharingRange <= 0
				? killer.Map.GetPartyMembers(killer)
				: killer.Map.GetPartyMembersInRange(killer, sharingRange);

			foreach (var member in partyMembers)
			{
				if (member != killer && !result.Contains(member))
					result.Add(member);
			}

			return result;
		}

		private void UpdateProgress(Character character, Mob mob)
		{
			character.Quests.UpdateObjectives<ScoreKillObjective>((quest, objective, progress) =>
			{
				if (progress.Done)
					return;

				var score = objective._score(mob, character);
				if (score <= 0)
					return;

				progress.Count = Math.Min(objective.TargetCount, progress.Count + score);
				progress.Done = progress.Count >= objective.TargetCount;

				character.Quests.UpdateQuestProgress(quest.Data.Id.Value, objective.Id);
				if (progress.Done)
				{
					objective.Completed?.Invoke(character, this);
					character.Quests.CompleteObjective(quest.Data.Id.Value, objective.Ident);
				}
			});
		}
	}
}
