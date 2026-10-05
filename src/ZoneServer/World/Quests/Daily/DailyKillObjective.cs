using System;
using System.Collections.Generic;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Groups;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World.Quests.Daily
{
	public class DailyKillObjective : QuestObjective
	{
		private static readonly object SubscriptionLock = new();
		private static bool isSubscribed;

		private readonly DailyQuestType dailyQuestType;
		private readonly Func<Mob, Character, bool> matches;

		public DailyKillObjective(
			DailyQuestType dailyQuestType,
			int targetCount,
			Func<Mob, Character, bool> matches)
		{
			if (targetCount <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(targetCount),
					"Daily quest target count must be greater than zero.");
			}

			this.dailyQuestType = dailyQuestType;

			this.matches = matches
				?? throw new ArgumentNullException(nameof(matches));

			this.TargetCount = targetCount;

			EnsureSubscribed();
		}

		/// <summary>
		/// As objectives são criadas dinamicamente por personagem.
		/// A inscrição global já é realizada no construtor.
		/// </summary>
		public override void Load()
		{
		}

		/// <summary>
		/// A inscrição permanece ativa durante toda a execução do ZoneServer.
		/// </summary>
		public override void Unload()
		{
		}

		private static void EnsureSubscribed()
		{
			lock (SubscriptionLock)
			{
				if (isSubscribed)
					return;

				ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe(
					OnEntityKilled);

				isSubscribed = true;
			}
		}

		private static void OnEntityKilled(
			object sender,
			CombatEventArgs args)
		{
			if (args.Target is not Mob mob)
				return;

			var killer = mob.GetKillBeneficiary(args.Attacker);

			if (killer == null)
				return;

			foreach (var character in GetEligibleCharacters(killer))
			{
				UpdateProgress(
					character,
					mob);
			}
		}

		private static IEnumerable<Character> GetEligibleCharacters(
			Character killer)
		{
			var characters = new HashSet<Character>
			{
				killer
			};

			if (!ZoneServer.Instance.Conf.World.PartyQuestSharingEnabled)
				return characters;

			if (!ZoneServer.Instance.Conf.World.PartyShareKillObjectives)
				return characters;

			var party = killer.Connection?.Party;

			if (party == null)
				return characters;

			if (party.QuestSharing != PartyQuestSharing.Enabled)
				return characters;

			var sharingRange =
				ZoneServer.Instance.Conf.World.PartyQuestSharingRange;

			var partyMembers = sharingRange <= 0
				? killer.Map.GetPartyMembers(killer)
				: killer.Map.GetPartyMembersInRange(
					killer,
					sharingRange);

			foreach (var member in partyMembers)
				characters.Add(member);

			return characters;
		}

		private static void UpdateProgress(
			Character character,
			Mob mob)
		{
			character.Quests.UpdateObjectives<DailyKillObjective>(
				(quest, objective, progress) =>
				{
					if (!progress.Unlocked)
						return;

					if (progress.Done)
						return;

					if (!objective.matches(
						mob,
						character))
					{
						return;
					}

					var manager =
						new DailyQuestManager();

					var changed =
						manager.IncreaseProgress(
							character,
							objective.dailyQuestType);

					if (!changed)
						return;

					var savedQuest =
						manager.GetQuest(
							character,
							objective.dailyQuestType);

					if (savedQuest == null)
						return;

					progress.Count = Math.Min(
						savedQuest.Progress,
						objective.TargetCount);

					if (progress.Count >= objective.TargetCount)
						progress.SetDone();
				});
		}
	}
}
