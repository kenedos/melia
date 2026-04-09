namespace Melia.Shared.Game.Const
{
	/// <summary>
	/// Defines the relation type between actors.
	/// </summary>
	public enum RelationType : byte
	{
		/// <summary>
		/// Friendly actors, such as party members.
		/// </summary>
		Friendly = 0,

		/// <summary>
		/// Enemy actors, such as mobs.
		/// </summary>
		Enemy = 1,

		/// <summary>
		/// Neutral actors, such as NPCs.
		/// </summary>
		Neutral = 2,

		/// <summary>
		/// A party member
		/// </summary>
		Party = 3,

		/// <summary>
		/// A guild member
		/// </summary>
		Guild = 4,

		/// <summary>
		/// All relations
		/// </summary>
		All = 127,
	}
}
