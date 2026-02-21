using System;
using System.Collections.Generic;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Groups;

namespace Melia.Zone.World.Quests.Objectives
{
	/// <summary>
	/// Objective to kill certain monsters.
	/// </summary>
	public class KillObjective : QuestObjective
	{
		/// <summary>
		/// Returns the tags which monsters must match to qualify for this
		/// objective.
		/// </summary>
		public HashSet<int> MonsterIds { get; }

		/// <summary>
		/// Creates an objective to kill a certain amount of one of the
		/// given types of monsters.
		/// </summary>
		/// <param name="monsterIds"></param>
		/// <param name="amount"></param>
		public KillObjective(int amount, params int[] monsterIds)
		{
			if (monsterIds == null || monsterIds.Length == 0)
				throw new ArgumentException("Must specify at least one monster id.");

			this.TargetCount = amount;
			this.MonsterIds = [.. monsterIds];
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

		/// <summary>
		/// Called when a character dies.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnEntityKilled(object sender, CombatEventArgs args)
		{
			if (args.Target is not IMonster monster)
				return;

			if (args.Attacker is not Character character)
				return;

			// Get all characters that should receive quest progress
			var eligibleCharacters = this.GetEligibleCharacters(character);

			foreach (var eligibleCharacter in eligibleCharacters)
			{
				this.UpdateKillProgress(eligibleCharacter, monster);
			}
		}

		/// <summary>
		/// Returns all characters eligible for quest progress from this kill.
		/// </summary>
		/// <param name="killer"></param>
		/// <returns></returns>
		private List<Character> GetEligibleCharacters(Character killer)
		{
			var result = new List<Character> { killer };

			// Check if party quest sharing is enabled
			if (!ZoneServer.Instance.Conf.World.PartyQuestSharingEnabled)
				return result;

			if (!ZoneServer.Instance.Conf.World.PartyShareKillObjectives)
				return result;

			var party = killer.Connection?.Party;
			if (party == null)
				return result;

			if (party.QuestSharing != PartyQuestSharing.Enabled)
				return result;

			// Get party members on same map
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

		/// <summary>
		/// Updates kill progress for a specific character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monster"></param>
		private void UpdateKillProgress(Character character, IMonster monster)
		{
			character.Quests.UpdateObjectives<KillObjective>((quest, objective, progress) =>
			{
				if (progress.Done)
					return;

				if (objective.IsTarget(monster))
				{
					progress.Count = Math.Min(objective.TargetCount, progress.Count + 1);
					progress.Done = progress.Count >= objective.TargetCount;

					character.Quests.UpdateQuestProgress(quest.Data.Id.Value, objective.Id);
					if (progress.Done)
					{
						objective.Completed?.Invoke(character, this);
						character.Quests.CompleteObjective(quest.Data.Id.Value, objective.Ident);
					}
				}
			});
		}

		/// <summary>
		/// Returns true if the given monster is a target for this objective.
		/// </summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		private bool IsTarget(IMonster monster)
		{
			return this.MonsterIds.Contains(monster.Id);
		}
	}
}
