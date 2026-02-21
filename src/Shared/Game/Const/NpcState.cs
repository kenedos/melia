namespace Melia.Shared.Game.Const
{
	/// <summary>
	/// Defines a NPC's state.
	/// </summary>
	public enum NpcState
	{
		/// <summary>
		/// As a buffer state between used states listed below
		/// </summary>
		IgnoreState = -2,

		/// <summary>
		/// Used for opened chests, when they disappear.
		/// </summary>
		Invisible = -1,

		/// <summary>
		/// Presumed default state.
		/// </summary>
		Normal = 0,

		/// <summary>
		/// Make's NPC visible on the minimap
		/// </summary>
		Highlighted = 1,

		/// <summary>
		/// Zemina Statues are set to this after interacting with them.
		/// </summary>
		Unknown_20 = 20,
	}
}
