using System;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Spawning;
using static Melia.Zone.Scripting.Shortcuts;

/// <summary>
/// Initializes the minigame spawner system.
/// </summary>
public class MinigameSpawnerInitScript : GeneralScript
{
	protected override void Load()
	{
		// Get minigame spawn points from database
		var spawnPoints = ZoneServer.Instance.Data.MinigameSpawnPointDb.Entries.ToArray();

		if (spawnPoints.Length == 0)
		{
			Yggdrasil.Logging.Log.Warning("No minigame spawn points found in database. Minigames will not spawn.");
			return;
		}

		// Get configuration values
		var conf = ZoneServer.Instance.Conf.World;
		var maxPopulation = conf.MinigameMaxPopulation;
		var cooldownSeconds = conf.MinigameRespawnCooldownSeconds;
		var rotationMinutes = conf.MinigameRotationTimeoutMinutes;

		// Create minigame spawner using configuration values
		var spawner = new MinigameSpawner(
			maxPopulation: maxPopulation,
			respawnDelay: Seconds(cooldownSeconds),
			rotationTimeout: Minutes(rotationMinutes),
			spawnPoints
		);

		// Register available minigames
		spawner.RegisterMinigame(new DefendTheTorchScript());
		spawner.RegisterMinigame(new MarbleShooterScript());
		spawner.RegisterMinigame(new WhackAMoleScript());

		// Add spawner to world
		ZoneServer.Instance.World.AddSpawner(spawner);

		Yggdrasil.Logging.Log.Info("Minigame spawner initialized with {0} spawn points (max: {1}, cooldown: {2}s).",
			spawnPoints.Length, maxPopulation, cooldownSeconds);
	}
}
