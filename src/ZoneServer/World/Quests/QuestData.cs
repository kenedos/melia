using System;
using System.Collections.Generic;
using Melia.Zone.Scripting;
using Yggdrasil.Util;

namespace Melia.Zone.World.Quests
{
	/// <summary>
	/// Represents a quest's data, as defined by a script.
	/// </summary>
	public class QuestData
	{
		/// <summary>
		/// Get or sets the quest's id.
		/// </summary>
		public QuestId Id { get; set; }

		/// <summary>
		/// Gets or sets the quest's name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the quest's description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the quest's location (map class name).
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the map class name where the quest giver is located.
		/// </summary>
		public string QuestGiverLocation { get; set; }

		/// <summary>
		/// Gets or sets the recommended level for the quest.
		/// </summary>
		public int Level { get; set; } = 1;

		/// <summary>
		/// Gets or sets whether the quest can be cancelled.
		/// </summary>
		public bool Cancelable { get; set; } = false;

		/// <summary>
		/// Gets or sets whether the quest is automatically added to the list
		/// of tracked quests.
		/// </summary>
		public bool AutoTrack { get; set; } = false;

		/// <summary>
		/// Gets or sets the start delay between meeting the quests
		/// prerequisites and receiving it automatically. Applies only
		/// to quests that start automatically.
		/// </summary>
		public TimeSpan StartDelay { get; set; } = TimeSpan.Zero;

		/// <summary>
		/// Gets or sets how the the quest unlocks objectives.
		/// </summary>
		public QuestUnlockType UnlockType { get; set; } = QuestUnlockType.Sequential;

		/// <summary>
		/// Gets or sets how the quest is given to players.
		/// </summary>
		public QuestReceiveType ReceiveType { get; set; } = QuestReceiveType.Manual;

		/// <summary>
		/// Returns a list of the quest's objectives.
		/// </summary>
		public List<QuestObjective> Objectives { get; } = new List<QuestObjective>();

		/// <summary>
		/// Returns a list of the quest's rewards.
		/// </summary>
		public List<QuestReward> Rewards { get; } = new List<QuestReward>();

		/// <summary>
		/// Returns a list of the quest's prerequisites, that need to be
		/// met to receive the quest automatically.
		/// </summary>
		public List<QuestPrerequisite> Prerequisites { get; } = new List<QuestPrerequisite>();

		/// <summary>
		/// Returns a list of the quest's modifiers.
		/// </summary>
		public List<QuestModifier> Modifiers { get; } = new List<QuestModifier>();

		/// <summary>
		/// Returns the quest giver npc.
		/// </summary>
		public string StartNpcUniqueName { get; set; } = null;
		/// <summary>
		/// Returns the quest receiver npc.
		/// </summary>
		public string EndNpcUniqueName { get; set; } = null;

		/// <summary>
		/// Returns the Quest's variables.
		/// </summary>
		/// <remarks>
		/// Quest variables are temporary and are not saved across server
		/// restarts.
		/// </remarks>
		public Variables Vars { get; } = new Variables();
	}

	/// <summary>
	/// Specifies how a quest unlocks its objectives.
	/// </summary>
	public enum QuestUnlockType
	{
		/// <summary>
		/// The objectives are unlocked one by one as they're completed.
		/// </summary>
		Sequential,

		/// <summary>
		/// The objectives are unlocked all at once and can be completed
		/// in any order.
		/// </summary>
		AllAtOnce,
	}

	/// <summary>
	/// Specifies how a quest gets to the player.
	/// </summary>
	public enum QuestReceiveType
	{
		/// <summary>
		/// The quest is started manually, by a script, via a dialog,
		/// or other methods.
		/// </summary>
		Manual,

		/// <summary>
		/// The quest is started automatically, as soon as the prerequisites
		/// have been met.
		/// </summary>
		Auto,
	}

	public enum QuestRepeatability
	{
		None,         // Cannot be repeated
		Simple,       // Can be repeated immediately after completion (like many examples)
		Daily,        // Can be repeated once per day (server reset time based)
		Weekly,       // Can be repeated once per week (server reset time based)
		Cooldown      // Can be repeated after a specific cooldown period
	}

	public enum PrerequisiteType
	{
		Level,
		Class,          // Player's base or current class
		QuestCompleted,
		QuestActive,
		QuestNotCompleted, // Useful for mutually exclusive quests
		ItemOwned,
		Reputation,     // Requires a separate Faction/Reputation system
		PlayerVariable, // Check a custom variable on the player
		Gender
	}

	public enum ObjectiveType
	{
		CollectItem,
		KillMob,
		TalkToNPC,
		VisitLocation,
		UseItem,
		InteractObject,
		Custom // For objectives handled entirely by QuestScript logic
	}

	public enum RewardType
	{
		Item,
		Experience,
		Currency,      // e.g., Silver (Vis)
		Reputation,    // Requires Faction system
		TriggerScript, // Execute a specific script function
		Custom         // For rewards handled entirely by QuestScript logic
	}
}
