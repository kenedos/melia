using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Spawning
{
	/// <summary>
	/// Defines a buff to be applied to monsters when they spawn on a map.
	/// </summary>
	public class SpawnBuffEntry
	{
		/// <summary>
		/// Returns the buff to apply.
		/// </summary>
		public BuffId BuffId { get; }

		/// <summary>
		/// Returns the first numeric argument for the buff.
		/// </summary>
		public float NumArg1 { get; }

		/// <summary>
		/// Returns the second numeric argument for the buff.
		/// </summary>
		public float NumArg2 { get; }

		/// <summary>
		/// Returns the chance (0-100) that this buff is applied to a
		/// spawning monster.
		/// </summary>
		public float Chance { get; }

		/// <summary>
		/// Returns the monster class id this entry applies to, or 0
		/// if it applies to all monsters.
		/// </summary>
		public int MonsterClassId { get; }

		/// <summary>
		/// Creates a new spawn buff entry.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="chance"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="monsterClassId"></param>
		public SpawnBuffEntry(BuffId buffId, float chance, float numArg1 = 0, float numArg2 = 0, int monsterClassId = 0)
		{
			this.BuffId = buffId;
			this.Chance = chance;
			this.NumArg1 = numArg1;
			this.NumArg2 = numArg2;
			this.MonsterClassId = monsterClassId;
		}
	}
}
