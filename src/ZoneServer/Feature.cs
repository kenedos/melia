using System;

namespace Melia.Zone
{
	/// <summary>
	/// Easy access method for feature options checks and modification.
	/// </summary>
	public static class Feature
	{
		/// <summary>
		/// Returns true if the given feature is enabled.
		/// </summary>
		/// <param name="feature"></param>
		/// <returns></returns>
		public static bool IsEnabled(string feature)
			=> ZoneServer.Instance.Data.FeatureDb.IsEnabled(feature);

		/// <summary>
		/// Enables the given feature.
		/// </summary>
		/// <param name="featureName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the given feature doesn't exist.
		/// </exception>
		public static void Enable(string featureName)
		{
			if (!ZoneServer.Instance.Data.FeatureDb.TryFind(featureName, out var feature))
				throw new ArgumentException($"Feature '{featureName}' not found.");

			feature.Enable(true);
		}

		/// <summary>
		/// Enables the given feature.
		/// </summary>
		/// <param name="featureName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the given feature doesn't exist.
		/// </exception>
		public static void Disable(string featureName)
		{
			if (!ZoneServer.Instance.Data.FeatureDb.TryFind(featureName, out var feature))
				throw new ArgumentException($"Feature '{featureName}' not found.");

			feature.Enable(false);
		}
	}

	public static class FeatureId
	{
		public static readonly string AbilityCostRevamp = "AbilityCostRevamp";
		public static readonly string AttackTypeBonusRevamp1 = "AttackTypeBonusRevamp1";
		public static readonly string AttendanceRewardSystem = "AttendanceRewardSystem";
		public static readonly string AttributeBonusRevamp = "AttributeBonusRevamp";
		public static readonly string BattleManager = "BattleManager";
		public static readonly string BountyHunterSystem = "BountyHunterSystem";
		public static readonly string CenturionRemoved = "CenturionRemoved";
		public static readonly string ClericHealPartySelect = "ClericHealPartySelect";
		public static readonly string CraftingTable = "CraftingTable";
		public static readonly string DashingForAll = "DashingForAll";
		public static readonly string DayNightCycle = "DayNightCycle";
		public static readonly string GrowthEquipOnStart = "GrowthEquipOnStart";
		public static readonly string IncreasedStatRatio = "IncreasedStatRatio";
		public static readonly string TerritoryWarSystem = "TerritoryWarSystem";
	}
}
