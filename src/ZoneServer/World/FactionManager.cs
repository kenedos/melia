using System;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.World
{
	/// <summary>
	/// Defines known faction identifiers used for reputation tracking.
	/// </summary>
	public static class FactionId
	{
		public const string Klaipeda = "Klaipeda";
		public const string Orsha = "Orsha";
		public const string Fedimian = "Fedimian";
		public const string ShadowSyndicate = "ShadowSyndicate";
	}

	/// <summary>
	/// Represents the named reputation tiers.
	/// </summary>
	public enum ReputationTier
	{
		Hated,
		Disliked,
		Neutral,
		Liked,
		Honored
	}

	/// <summary>
	/// Manages player reputation with various factions.
	/// </summary>
	public class FactionManager : IUpdateable
	{
		public const int MinReputation = -1000;
		public const int MaxReputation = 1000;
		private const string RepVarPrefix = "Reputation.";

		/// <summary>
		/// Gets the player's current reputation value with a specific faction.
		/// </summary>
		public int GetReputation(Character character, string factionId)
		{
			if (character == null || string.IsNullOrEmpty(factionId)) return 0;
			var varKey = RepVarPrefix + factionId;
			return character.Variables.Perm.GetInt(varKey, 0);
		}

		/// <summary>
		/// Gets the player's current reputation tier with a specific faction.
		/// </summary>
		public ReputationTier GetReputationTier(Character character, string factionId)
		{
			var reputation = this.GetReputation(character, factionId);
			return GetTier(reputation);
		}

		/// <summary>
		/// Sets the player's reputation with a faction to a specific value.
		/// </summary>
		public void SetReputation(Character character, string factionId, int value)
		{
			if (character == null || string.IsNullOrEmpty(factionId)) return;
			var varKey = RepVarPrefix + factionId;
			var clampedValue = Math.Clamp(value, MinReputation, MaxReputation);
			character.Variables.Perm.SetInt(varKey, clampedValue);
			Log.Debug($"Set {character.Name} reputation with {factionId} to {clampedValue}");
			character.SystemMessage($"Your reputation with {factionId} is now {this.GetTierName(clampedValue)} ({clampedValue}).");
		}

		/// <summary>
		/// Modifies the player's reputation with a faction by a specific amount.
		/// </summary>
		public int ModifyReputation(Character character, string factionId, int amount)
		{
			if (character == null || string.IsNullOrEmpty(factionId) || amount == 0)
				return this.GetReputation(character, factionId);

			var currentRep = this.GetReputation(character, factionId);
			var newValue = currentRep + amount;
			var clampedValue = Math.Clamp(newValue, MinReputation, MaxReputation);

			if (clampedValue != currentRep)
			{
				var varKey = RepVarPrefix + factionId;
				character.Variables.Perm.SetInt(varKey, clampedValue);
				Log.Debug($"Modified {character.Name} reputation with {factionId} by {amount} to {clampedValue}");

				var oldTierName = this.GetTierName(currentRep);
				var newTierName = this.GetTierName(clampedValue);
				var changeText = amount > 0 ? $"improved to" : $"decreased to";

				if (newTierName != oldTierName)
					character.ServerMessage($"Your standing with {this.GetFactionDisplayName(factionId)} has {changeText} {newTierName} ({clampedValue}).");

				ZoneServer.Instance.ServerEvents.PlayerReputationChanged.Raise(new ReputationEventArgs(character, factionId, currentRep, clampedValue));

				return clampedValue;
			}

			return currentRep;
		}

		/// <summary>
		/// Gets the named tier corresponding to a reputation value.
		/// </summary>
		public ReputationTier GetTier(int reputationValue)
		{
			var rep = Math.Clamp(reputationValue, MinReputation, MaxReputation);
			if (rep <= -751) return ReputationTier.Hated;
			if (rep <= -251) return ReputationTier.Disliked;
			if (rep <= 249) return ReputationTier.Neutral;
			if (rep <= 749) return ReputationTier.Liked;
			return ReputationTier.Honored;
		}

		/// <summary>
		/// Gets the display name for a reputation tier.
		/// </summary>
		public string GetTierName(ReputationTier tier)
		{
			return tier switch
			{
				ReputationTier.Hated => L("Hated"),
				ReputationTier.Disliked => L("Disliked"),
				ReputationTier.Neutral => L("Neutral"),
				ReputationTier.Liked => L("Liked"),
				ReputationTier.Honored => L("Honored"),
				_ => L("Unknown")
			};
		}

		/// <summary>
		/// Gets the display name for a reputation tier based on value.
		/// </summary>
		public string GetTierName(int reputationValue)
		{
			return this.GetTierName(this.GetTier(reputationValue));
		}

		/// <summary>
		/// Gets the display name for a faction ID.
		/// </summary>
		public string GetFactionDisplayName(string factionId)
		{
			return factionId switch
			{
				FactionId.Klaipeda => L("Klaipeda"),
				FactionId.Orsha => L("Orsha"),
				FactionId.Fedimian => L("Fedimian"),
				FactionId.ShadowSyndicate => L("Shadow Syndicate"),
				_ => L(factionId)
			};
		}

		/// <summary>
		/// Gets the minimum reputation value required for a given tier.
		/// </summary>
		public int GetMinimumValueForTier(ReputationTier tier)
		{
			return tier switch
			{
				ReputationTier.Hated => MinReputation,
				ReputationTier.Disliked => -750,
				ReputationTier.Neutral => -250,
				ReputationTier.Liked => 250,
				ReputationTier.Honored => 750,
				_ => MinReputation
			};
		}

		/// <summary>
		/// Checks if the player's reputation is at or above a certain tier.
		/// </summary>
		public bool HasTierOrHigher(Character character, string factionId, ReputationTier requiredTier)
		{
			var currentTier = this.GetTier(GetReputation(character, factionId));
			return currentTier >= requiredTier;
		}

		/// <summary>
		/// Checks if the player's reputation is at or below a certain tier.
		/// </summary>
		public bool HasTierOrLower(Character character, string factionId, ReputationTier maxTier)
		{
			var currentTier = this.GetTier(GetReputation(character, factionId));
			return currentTier <= maxTier;
		}

		public void Update(TimeSpan elapsed)
		{
		}
	}
}
