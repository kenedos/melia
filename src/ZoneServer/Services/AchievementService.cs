using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Services
{
	/// <summary>
	/// Service that listens to global server events and awards achievement points.
	/// </summary>
	public class AchievementService
	{
		/// <summary>
		/// Initializes the achievement service and sets up event subscriptions.
		/// </summary>
		public void Initialize()
		{
			var events = ZoneServer.Instance.ServerEvents;

			events.EntityKilled.Subscribe(this.OnEntityKilled);
			events.PlayerUsedItem.Subscribe(this.OnPlayerUsedItem);
			events.PlayerCompletedQuest.Subscribe(this.OnPlayerCompletedQuest);
		}

		/// <summary>
		/// Called when any entity is killed.
		/// </summary>
		private void OnEntityKilled(object sender, CombatEventArgs args)
		{
			if (args.Target is Character victim)
			{
				// Character died
				if (args.Attacker is Character killer)
				{
					// Award PcKill points
					killer.Achievements?.AddPlayerKillPoints(1);
				}
			}
			else if (args.Target is Mob mob)
			{
				// Monster died
				var killerCharacter = this.GetKillBeneficiary(args.Attacker, mob);
				if (killerCharacter == null)
					return;

				// Check if this is a boss monster
				if (mob.Data.Rank == MonsterRank.Boss)
				{
					killerCharacter.Achievements?.AddBossMonsterKillPoints(1);
				}
				else
				{
					killerCharacter.Achievements?.AddMonsterKillPoints(1);
					if (mob.Data.Name == "Hanaming")
						killerCharacter.Achievements?.AddHanamingKillPoints(1);
				}
			}
		}

		/// <summary>
		/// Called when a player uses an item.
		/// </summary>
		private void OnPlayerUsedItem(object sender, PlayerUsedItemEventArgs args)
		{
			var character = args.Character;
			var item = args.Item;

			if (character.Achievements == null)
				return;

			// Check if the item is a potion (category 2 is typically consumables/potions)
			var itemDb = ZoneServer.Instance.Data.ItemDb;
			if (itemDb.TryFind(item.Id, out var itemData))
			{
				if (itemData.Category == InventoryCategory.Consume_Potion)
				{
					character.Achievements.AddPotionUsePoints(1);
				}
			}
		}

		/// <summary>
		/// Called when a player completes a quest.
		/// </summary>
		private void OnPlayerCompletedQuest(object sender, PlayerCompletedQuestEventArgs args)
		{
			args.Character.Achievements?.AddQuestCompletionPoints(1);
		}

		/// <summary>
		/// Returns the character that benefits from the kill.
		/// </summary>
		private Character GetKillBeneficiary(ICombatEntity killer, Mob mob)
		{
			if (killer == null)
				return null;

			var beneficiary = killer;

			// Handle pets/summons - master gets the credit
			if (beneficiary.Components.TryGet<Melia.Zone.World.Actors.CombatEntities.Components.AiComponent>(out var aiComponent))
			{
				if (aiComponent.Script.GetMaster() is Character master)
					beneficiary = master;
			}

			return beneficiary as Character;
		}
	}
}
