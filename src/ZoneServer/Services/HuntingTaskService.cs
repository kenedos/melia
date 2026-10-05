using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Database;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Yggdrasil.Logging;

namespace Melia.Zone.Services
{
	public class HuntingTaskService
	{
		private const byte StatusAvailable = 0;
		private const byte StatusActive = 1;
		private const byte StatusCompleted = 2;

		private const int DefaultRequiredKills = 500;
		private const int DefaultRewardPoints = 5;
		public static QuestId TrackerQuestId => new("hunting_tasks", 1001);

		public void Initialize()
		{
			HuntingTaskPool.LogPool();

			var events = ZoneServer.Instance.ServerEvents;
			events.EntityKilled.Subscribe(this.OnEntityKilled);
		}

		public HuntingTaskState GetOrCreateTaskOptions(Character character)
		{
			var accountId = character.AccountDbId;
			var state = ZoneServer.Instance.Database.GetHuntingTask(accountId);

			if (state == null)
			{
				state = new HuntingTaskState
				{
					AccountId = accountId,
					Status = StatusAvailable,
				};
			}

			if (state.Status == StatusActive)
				return state;

			if (state.Status == StatusAvailable &&
					state.Option1MonsterId != 0 &&
					state.Option2MonsterId != 0 &&
					state.Option3MonsterId != 0)
				return state;

			this.GenerateOptions(character, state);

			return state;
		}

		private void OnEntityKilled(object sender, CombatEventArgs args)
		{
			if (args.Target is not Mob mob)
				return;

			var character = this.GetKillBeneficiary(args.Attacker);

			if (character == null)
			{
				return;
			}

			var accountId = character.AccountDbId;

			var task = ZoneServer.Instance.Database.GetHuntingTask(accountId);

			if (task == null)
			{
				return;
			}

			if (task.Status != StatusActive)
			{
				return;
			}

			if (task.SelectedMonsterId <= 0)
			{
				return;
			}

			if (mob.Id != task.SelectedMonsterId)
			{
				return;
			}

			if (task.RequiredKills <= 0)
			{
				return;
			}

			if (task.CurrentKills >= task.RequiredKills)
			{
				return;
			}

			task.CurrentKills++;

			if (task.CurrentKills > task.RequiredKills)
				task.CurrentKills = task.RequiredKills;

			if (task.CurrentKills >= task.RequiredKills)
			{
				task.Points += task.RewardPoints;
				task.TasksCompleted++;
				task.Status = StatusCompleted;
			}

			ZoneServer.Instance.Database.SaveHuntingTask(task);

			this.SyncTracker(character);
		}

		private Character GetKillBeneficiary(ICombatEntity killer)
		{
			if (killer == null)
				return null;

			var beneficiary = killer;

			if (beneficiary.Components.TryGet<Melia.Zone.World.Actors.CombatEntities.Components.AiComponent>(out var aiComponent))
			{
				if (aiComponent.Script.GetMaster() is Character master)
					beneficiary = master;
			}

			return beneficiary as Character;
		}

		private void GenerateOptions(Character character, HuntingTaskState state)
		{
			var level = character.Level;

			var options = HuntingTaskPool.GetOptions(level, 3);

			if (options.Length < 3)
			{
				Log.Warning(
						"[Hunting Task] Could not generate 3 options for Character={0}, Level={1}. Found={2}",
						character.Name,
						level,
						options.Length
				);

				return;
			}

			state.Status = StatusAvailable;

			state.Option1MonsterId = options[0].MonsterId;
			state.Option2MonsterId = options[1].MonsterId;
			state.Option3MonsterId = options[2].MonsterId;

			state.SelectedMonsterId = 0;
			state.RequiredKills = 0;
			state.CurrentKills = 0;
			state.RewardPoints = 0;

			ZoneServer.Instance.Database.SaveHuntingTask(state);
		}

		public bool SelectTask(Character character, int option)
		{
			var accountId = character.AccountDbId;
			var state = ZoneServer.Instance.Database.GetHuntingTask(accountId);

			if (state == null)
				return false;

			if (state.Status != StatusAvailable)
				return false;

			var monsterId = option switch
			{
				1 => state.Option1MonsterId,
				2 => state.Option2MonsterId,
				3 => state.Option3MonsterId,
				_ => 0,
			};

			if (monsterId == 0)
				return false;

			var monster = HuntingTaskPool.GetMonster(monsterId);

			if (monster == null)
				return false;

			state.SelectedMonsterId = monsterId;
			state.RequiredKills = DefaultRequiredKills;
			state.CurrentKills = 0;
			state.RewardPoints = DefaultRewardPoints;
			state.Status = StatusActive;

			ZoneServer.Instance.Database.SaveHuntingTask(state);

			return true;
		}

		public bool Reroll(Character character)
		{
			var accountId = character.AccountDbId;
			var state = ZoneServer.Instance.Database.GetHuntingTask(accountId);

			if (state == null)
				return false;

			if (state.Status != StatusAvailable)
				return false;

			var old1 = state.Option1MonsterId;
			var old2 = state.Option2MonsterId;
			var old3 = state.Option3MonsterId;

			for (var attempt = 0; attempt < 10; attempt++)
			{
				var level = character.Level;
				var options = HuntingTaskPool.GetOptions(level, 3);

				if (options.Length < 3)
					return false;

				var new1 = options[0].MonsterId;
				var new2 = options[1].MonsterId;
				var new3 = options[2].MonsterId;

				var identical =
						new1 == old1 &&
						new2 == old2 &&
						new3 == old3;

				if (identical)
					continue;

				state.Option1MonsterId = new1;
				state.Option2MonsterId = new2;
				state.Option3MonsterId = new3;

				state.SelectedMonsterId = 0;
				state.RequiredKills = 0;
				state.CurrentKills = 0;
				state.RewardPoints = 0;

				state.RerollCount++;

				ZoneServer.Instance.Database.SaveHuntingTask(state);

				return true;
			}

			return false;
		}

		public bool GenerateNextTask(Character character)
		{
			var accountId = character.AccountDbId;
			var state = ZoneServer.Instance.Database.GetHuntingTask(accountId);

			if (state == null)
				return false;

			if (state.Status != StatusCompleted)
				return false;

			state.Status = StatusAvailable;

			state.Option1MonsterId = 0;
			state.Option2MonsterId = 0;
			state.Option3MonsterId = 0;

			state.SelectedMonsterId = 0;
			state.RequiredKills = 0;
			state.CurrentKills = 0;
			state.RewardPoints = 0;

			ZoneServer.Instance.Database.SaveHuntingTask(state);

			this.GenerateOptions(character, state);

			return true;
		}

		public HuntingTaskState GetState(Character character)
		{
			return ZoneServer.Instance.Database.GetHuntingTask(character.AccountDbId);
		}

		public int GetPoints(Character character)
		{
			if (character == null)
				return 0;

			var state = ZoneServer.Instance.Database.GetHuntingTask(character.AccountDbId);

			if (state == null)
				return 0;

			return state.Points;
		}

		public bool TrySpendPoints(Character character, int amount)
		{
			if (character == null || amount <= 0)
				return false;

			var state = ZoneServer.Instance.Database.GetHuntingTask(character.AccountDbId);

			if (state == null)
				return false;

			if (state.Points < amount)
				return false;

			state.Points -= amount;

			ZoneServer.Instance.Database.SaveHuntingTask(state);

			Log.Info(
				"[Hunting Points] Spent: AccountId={0}, Character={1}, Amount={2}, Remaining={3}",
				character.AccountDbId,
				character.Name,
				amount,
				state.Points
			);

			return true;
		}

		public async Task StartTracker(Character character)
		{
			if (character == null)
				return;

			var task = ZoneServer.Instance.Database.GetHuntingTask(character.AccountDbId);

			if (task == null || task.Status != StatusActive)
				return;

			var questId = TrackerQuestId;

			if (!character.Quests.IsActive(questId))
				await character.Quests.Start(questId);

			HuntingTaskObjective.Sync(character);
		}

		public void RestoreTracker(Character character)
		{
			if (character == null)
				return;

			var task = ZoneServer.Instance.Database.GetHuntingTask(character.AccountDbId);

			if (task == null || task.Status != StatusActive)
				return;

			if (!character.Quests.IsActive(TrackerQuestId))
				return;

			HuntingTaskObjective.Sync(character);
		}

		public void SyncTracker(Character character)
		{
			if (character == null)
				return;

			var task = ZoneServer.Instance.Database.GetHuntingTask(character.AccountDbId);

			if (task == null)
				return;

			if (!character.Quests.IsActive(TrackerQuestId))
				return;

			HuntingTaskObjective.Sync(character);
		}
	}
}
