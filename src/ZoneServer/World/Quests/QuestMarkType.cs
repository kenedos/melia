namespace Melia.Zone.World.Quests
{
	/// <summary>
	/// Defines the marker displayed above a quest NPC's head.
	/// </summary>
	public enum QuestMarkType
	{
		/// <summary>
		/// No marker.
		/// </summary>
		None = 0,

		/// <summary>
		/// The character is on one of the NPC's quests, but isn't done with
		/// it yet.
		/// </summary>
		InProgress = 1,

		/// <summary>
		/// The NPC has a quest the character can start.
		/// </summary>
		Available = 2,

		/// <summary>
		/// The character finished a quest they can hand in to the NPC.
		/// </summary>
		Complete = 3,
	}
}
