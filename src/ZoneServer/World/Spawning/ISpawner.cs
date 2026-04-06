using System;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Spawning
{
	public interface ISpawner : IUpdateable
	{
		int Id { get; }

		/// <summary>
		/// Initializes the population by setting current monster amount to zero
		/// </summary>
		void InitializePopulation();

		/// <summary>
		/// Spawns the given amount of monsters.
		/// </summary>
		/// <param name="amount"></param>
		void Spawn(int amount);

		/// <summary>
		/// Notifies the spawner that monsters were removed due to map
		/// dormancy.
		/// </summary>
		/// <param name="removedCount"></param>
		void NotifyDormancy(int removedCount);

		/// <summary>
		/// Raised for every monster the spawner creates, just before it's
		/// added to the world.
		/// </summary>
		public event EventHandler<SpawnEventArgs> Spawning;

		/// <summary>
		/// Raised for every monster the spawner creates, just after it was
		/// added to the world.
		/// </summary>
		public event EventHandler<SpawnEventArgs> Spawned;
	}
}
