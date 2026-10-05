using System;
using System.Collections.Generic;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Groups;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World.Quests.Weekly
{
	public class WeeklyKillObjective : QuestObjective
	{
		private static readonly object SubscriptionLock = new();
		private static bool isSubscribed;
		private readonly Func<Mob, Character, bool> matches;

		public string Key { get; }

		public WeeklyKillObjective(string key, int targetCount, Func<Mob, Character, bool> matches)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Weekly quest key cannot be empty.", nameof(key));
			if (targetCount <= 0)
				throw new ArgumentOutOfRangeException(nameof(targetCount));

			this.Key = key;
			this.matches = matches ?? throw new ArgumentNullException(nameof(matches));
			this.TargetCount = targetCount;
			EnsureSubscribed();
		}

		public override void Load() { }
		public override void Unload() { }

		private static void EnsureSubscribed()
		{
			lock (SubscriptionLock)
			{
				if (isSubscribed)
					return;

				ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe(OnEntityKilled);
				isSubscribed = true;
			}
		}

		private static void OnEntityKilled(object sender, CombatEventArgs args)
		{
			if (args.Target is not Mob mob)
				return;

			var killer = mob.GetKillBeneficiary(args.Attacker);
			if (killer == null)
				return;

			foreach (var character in GetEligibleCharacters(killer))
				UpdateProgress(character, mob);
		}

		private static IEnumerable<Character> GetEligibleCharacters(Character killer)
		{
			var characters = new HashSet<Character> { killer };
			if (!ZoneServer.Instance.Conf.World.PartyQuestSharingEnabled || !ZoneServer.Instance.Conf.World.PartyShareKillObjectives)
				return characters;

			var party = killer.Connection?.Party;
			if (party == null || party.QuestSharing != PartyQuestSharing.Enabled)
				return characters;

			var range = ZoneServer.Instance.Conf.World.PartyQuestSharingRange;
			var members = range <= 0 ? killer.Map.GetPartyMembers(killer) : killer.Map.GetPartyMembersInRange(killer, range);
			foreach (var member in members)
				characters.Add(member);

			return characters;
		}

		private static void UpdateProgress(Character character, Mob mob)
		{
			character.Quests.UpdateObjectives<WeeklyKillObjective>((quest, objective, progress) =>
			{
				if (!progress.Unlocked || progress.Done || !objective.matches(mob, character))
					return;

				var manager = new WeeklyQuestManager();
				if (!manager.IncreaseProgress(character, objective.Key))
					return;

				var saved = manager.GetQuest(character, objective.Key);
				if (saved == null)
					return;

				progress.Count = Math.Min(saved.Progress, objective.TargetCount);
				if (progress.Count >= objective.TargetCount)
					progress.SetDone();
			});
		}
	}
}
