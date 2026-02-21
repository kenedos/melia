using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World
{
	/// <summary>
	/// Represents the details of a bounty on a player.
	/// </summary>
	public class BountyInfo
	{
		public long TargetDbId { get; }
		public string TargetName { get; }
		public long TotalBounty { get; private set; }
		public ConcurrentDictionary<long, int> Contributors { get; } = new();
		public DateTime CreationTime { get; }
		public DateTime ExpirationTime { get; private set; }

		public BountyInfo(long targetDbId, string targetName, int initialAmount, long placerDbId)
		{
			this.TargetDbId = targetDbId;
			this.TargetName = targetName;
			this.CreationTime = DateTime.UtcNow;
			this.AddContribution(initialAmount, placerDbId);
		}

		public void AddContribution(int amount, long placerDbId)
		{
			this.TotalBounty += amount;
			this.Contributors.AddOrUpdate(placerDbId, amount, (key, existingVal) => existingVal + amount);
			// Each new contribution resets the expiration timer.
			this.ExpirationTime = DateTime.UtcNow.AddDays(7);
		}
	}

	/// <summary>
	/// Manages the player bounty system.
	/// </summary>
	public class BountyManager : IUpdateable
	{
		private readonly ConcurrentDictionary<long, BountyInfo> _bounties = new(); // Key: Target Character DB ID
		private const int MinimumBounty = 10000; // Minimum silver to place a bounty.

		public void PlaceBounty(Character placer, Character target, int amount)
		{
			if (!Feature.IsEnabled(FeatureId.BountyHunterSystem))
			{
				placer.ServerMessage("The bounty system is currently disabled.");
				return;
			}

			if (placer.DbId == target.DbId)
			{
				placer.ServerMessage("You cannot place a bounty on yourself.");
				return;
			}

			if (amount < MinimumBounty)
			{
				placer.ServerMessage($"The minimum bounty amount is {MinimumBounty:N0} silver.");
				return;
			}

			// --- Faction Integration: Check if player can place a bounty here ---
			var currentMapFaction = placer.GetRegionFaction();
			if (!string.IsNullOrEmpty(currentMapFaction))
			{
				var factions = ZoneServer.Instance.World.Factions;
				if (factions.HasTierOrLower(placer, currentMapFaction, ReputationTier.Disliked))
				{
					placer.ServerMessage($"Your standing with {factions.GetFactionDisplayName(currentMapFaction)} is too low to place bounties in this territory.");
					return;
				}
			}
			// --------------------------------------------------------------------

			if (!placer.HasSilver(amount))
			{
				// HasSilver already sends a "Not enough money" message.
				return;
			}

			// Take the silver from the placer.
			placer.RemoveItem(ItemId.Silver, amount);

			var bountyInfo = _bounties.GetOrAdd(target.DbId, (dbId) => new BountyInfo(target.DbId, target.Name, amount, placer.DbId));
			if (bountyInfo.TargetName != target.Name) // Handle name changes
			{
				bountyInfo = new BountyInfo(target.DbId, target.Name, amount, placer.DbId);
				_bounties[target.DbId] = bountyInfo;
			}
			else if (bountyInfo.Contributors.ContainsKey(placer.DbId))
			{
				// Player is adding to their existing bounty
				bountyInfo.AddContribution(amount, placer.DbId);
			}

			// --- Faction Integration: Reputation change for placing a bounty ---
			if (!string.IsNullOrEmpty(currentMapFaction))
			{
				// Lose lawful rep
				ZoneServer.Instance.World.Factions.ModifyReputation(placer, currentMapFaction, -10);
			}
			// Gain outlaw rep
			ZoneServer.Instance.World.Factions.ModifyReputation(placer, FactionId.ShadowSyndicate, 25);
			// --------------------------------------------------------------------

			placer.ServerMessage($"You have added {amount:N0} silver to the bounty on {target.Name}.");
			target.ServerMessage($"The bounty on your head has increased!");

			Send.ZC_TEXT(NoticeTextType.Gold, $"{placer.Name} has increased the bounty on {target.Name}! The total is now {bountyInfo.TotalBounty:N0} silver.");
			Log.Info($"Bounty Placed: {placer.Name} ({placer.DbId}) placed {amount} on {target.Name} ({target.DbId}). New total: {bountyInfo.TotalBounty}");
		}

		public void ClaimBounty(Character killer, Character victim)
		{
			if (!Feature.IsEnabled(FeatureId.BountyHunterSystem)) return;

			if (_bounties.TryRemove(victim.DbId, out var bountyInfo))
			{
				var factions = ZoneServer.Instance.World.Factions;
				var currentMapFaction = killer.GetRegionFaction();
				var bountyAmount = (int)bountyInfo.TotalBounty;

				// --- Faction Integration: Apply bonus/penalty on claim ---
				if (!string.IsNullOrEmpty(currentMapFaction))
				{
					var killerTier = factions.GetReputationTier(killer, currentMapFaction);

					if (killerTier >= ReputationTier.Liked)
					{
						var bonus = (int)(bountyAmount * 0.10); // 10% bonus for good guys
						bountyAmount += bonus;
						killer.ServerMessage($"Your good standing with {factions.GetFactionDisplayName(currentMapFaction)} has earned you a bonus of {bonus:N0} silver!");
						factions.ModifyReputation(killer, currentMapFaction, 50); // Bonus rep for lawful good action
					}
					else if (killerTier <= ReputationTier.Disliked)
					{
						var penalty = (int)(bountyAmount * 0.25); // 25% "protection fee" for outlaws
						bountyAmount -= penalty;
						killer.ServerMessage($"The local authorities have seized {penalty:N0} silver of your bounty as a fine for your criminal record.");
						factions.ModifyReputation(killer, currentMapFaction, -25); // Further rep loss
					}
				}
				// ---------------------------------------------------------

				if (bountyAmount > 0)
				{
					killer.AddItem(ItemId.Silver, bountyAmount);
				}

				// Regardless of penalties, claiming a bounty is an outlaw action
				factions.ModifyReputation(killer, FactionId.ShadowSyndicate, 15);

				Send.ZC_TEXT(NoticeTextType.Gold, $"{killer.Name} has claimed the {bountyInfo.TotalBounty:N0} silver bounty on {victim.Name}!");
				killer.ServerMessage($"You have collected a bounty of {bountyAmount:N0} silver!");
				Log.Info($"Bounty Claimed: {killer.Name} ({killer.DbId}) claimed {bountyAmount} (original: {bountyInfo.TotalBounty}) from {victim.Name} ({victim.DbId}).");
			}
		}

		public BountyInfo GetBountyInfo(long targetDbId)
		{
			_bounties.TryGetValue(targetDbId, out var bounty);
			return bounty;
		}

		public List<BountyInfo> GetTopBounties(int count = 10)
		{
			return _bounties.Values.OrderByDescending(b => b.TotalBounty).Take(count).ToList();
		}

		public void Update(TimeSpan elapsed)
		{
			if (!Feature.IsEnabled(FeatureId.BountyHunterSystem)) return;

			// Check for expired bounties periodically.
			// In a real server, this might run less frequently.
			foreach (var bounty in _bounties.Values)
			{
				if (DateTime.UtcNow > bounty.ExpirationTime)
				{
					if (_bounties.TryRemove(bounty.TargetDbId, out var removedBounty))
					{
						// Refund the silver to contributors. This is complex as they may be offline.
						// For simplicity, we can just log it. A real implementation would need a mail system or offline transaction queue.
						Log.Warning($"Bounty on {removedBounty.TargetName} ({removedBounty.TargetDbId}) expired. Total: {removedBounty.TotalBounty}. Refunding is a TODO.");
						// TODO: Implement a system to refund contributors.
					}
				}
			}
		}
	}
}
