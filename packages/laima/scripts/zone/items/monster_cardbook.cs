using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;

/// <summary>
/// Monster Card Album summoning scripts.
/// Handles the SCR_SUMMON_MONSTER_FROM_CARDBOOK function for various card books.
/// </summary>
public class MonsterCardBookScripts : GeneralScript
{
	/// <summary>
	/// Summons a random boss monster from the card book based on the card type(s).
	/// </summary>
	/// <param name="character">The character using the item.</param>
	/// <param name="item">The card book item being used.</param>
	/// <param name="cardTypes">Card type groups separated by "/" (e.g., "Red", "Blue", "Red/Blue/Green/Purple", or "All").</param>
	/// <param name="numArg1">Not used.</param>
	/// <param name="numArg2">Not used.</param>
	/// <returns>ItemUseResult indicating success or failure.</returns>
	[ScriptableFunction]
	public ItemUseResult SCR_SUMMON_MONSTER_FROM_CARDBOOK(Character character, Item item, string cardTypes, float numArg1, float numArg2)
	{
		// Cannot use in town
		if (character.Map.Data.Type == MapType.City)
		{
			character.ServerMessage(Localization.Get("Cannot summon monsters in town."));
			return ItemUseResult.Fail;
		}

		// Split the card types (e.g., "Red/Blue/Green/Purple" -> ["Red", "Blue", "Green", "Purple"])
		var groups = cardTypes.Split('/');

		// Get random boss from the weighted selection
		var bossData = ZoneServer.Instance.Data.ItemSummonBossDb.GetRandomFromGroups(groups);
		if (bossData == null)
		{
			Log.Warning("SCR_SUMMON_MONSTER_FROM_CARDBOOK: No valid boss found for groups '{0}'.", cardTypes);
			character.ServerMessage(Localization.Get("Failed to summon a monster from the card book."));
			return ItemUseResult.Fail;
		}

		// Find the monster data
		if (!ZoneServer.Instance.Data.MonsterDb.TryFind(bossData.MonsterClassName, out var monsterData))
		{
			Log.Warning("SCR_SUMMON_MONSTER_FROM_CARDBOOK: Monster '{0}' not found in database.", bossData.MonsterClassName);
			character.ServerMessage(Localization.Get("Failed to summon a monster from the card book."));
			return ItemUseResult.Fail;
		}

		// Spawn the monster(s)
		for (var i = 0; i < bossData.Count; i++)
		{
			var monster = CreateCardBookMonster(monsterData.Id, bossData.SummonLevel, bossData.LifeTime, bossData.Group, character);
			if (monster == null)
				continue;

			character.Map.AddMonster(monster);

			// Play effect based on the group color
			PlayGroupEffect(monster, bossData.Group);
		}

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Creates a monster for the card book summon.
	/// </summary>
	/// <param name="monsterClassId">The monster class ID to spawn.</param>
	/// <param name="summonLevel">The level to set for the monster.</param>
	/// <param name="lifeTime">The lifetime in seconds before the monster despawns.</param>
	/// <param name="group">The card book group for the monster.</param>
	/// <param name="summoner">The character who summoned the monster.</param>
	/// <returns>The created monster, or null if creation failed.</returns>
	private static Mob CreateCardBookMonster(int monsterClassId, int summonLevel, int lifeTime, string group, Character summoner)
	{
		var pos = GetRandomSpawnPosition(summoner);

		var monster = new Mob(monsterClassId, RelationType.Enemy);
		monster.Position = pos;
		monster.Level = summonLevel;

		// Set the disappear time
		monster.DisappearTime = DateTime.Now + TimeSpan.FromSeconds(lifeTime);

		// Apply map-specific property overrides if any
		if (summoner.Map.TryGetPropertyOverrides(monsterClassId, out var propertyOverrides))
			monster.ApplyOverrides(propertyOverrides);

		// Add components
		monster.Components.Add(new MovementComponent(monster));
		monster.Components.Add(new AiComponent(monster, "BasicBoss"));
		//monster.Components.Add(new AiComponent(monster, "BasicCardSummonBoss"));

		// Mark as card book summon (for death handling and special rewards)
		monster.Vars.SetInt("CARDSUMMON_BOSS", 1);

		// Skip normal drops - cards are handled by CardAlbumBossRewardsScript
		monster.HasDrops = false;

		return monster;
	}

	/// <summary>
	/// Returns a random spawn position near the character.
	/// </summary>
	/// <param name="character">The character to spawn near.</param>
	/// <returns>A valid spawn position.</returns>
	private static Position GetRandomSpawnPosition(Character character)
	{
		var rnd = RandomProvider.Get();
		var pos = character.Position;

		// Try to find a valid ground position nearby
		for (var i = 0; i < 10; ++i)
		{
			var rndPos = pos.GetRandomInRange2D(30, rnd);
			if (character.Map.Ground.TryGetHeightAt(rndPos, out var height))
			{
				pos = rndPos;
				pos.Y = height;
				break;
			}
		}

		return pos;
	}

	/// <summary>
	/// Plays the spawn effect based on the card book group.
	/// </summary>
	/// <param name="monster">The monster to play the effect on.</param>
	/// <param name="group">The card book group name.</param>
	private static void PlayGroupEffect(Mob monster, string group)
	{
		var effectName = group switch
		{
			"Red" => "F_pc_CardBook_ground_red",
			"Blue" => "F_pc_CardBook_ground_blue",
			"Green" => "F_pc_CardBook_ground_green",
			"Purple" => "F_pc_CardBook_ground_violet",
			"Field" => "F_pc_CardBook_ground_dark",
			"HighField" => "F_pc_CardBook_ground_dark",
			_ => null
		};

		if (effectName != null)
			Send.ZC_NORMAL.PlayEffect(monster, effectName, 2.0f);
	}
}
