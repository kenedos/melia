//--- Melia Script ----------------------------------------------------------
// Popup Book Items
//--- Description -----------------------------------------------------------
// Item scripts that spawn popup book/photo wall NPCs.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

public class PopupBookItemScripts : GeneralScript
{
	/// <summary>
	/// Spawns a popup book/photo wall NPC near the player.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="npcClassName">The NPC/monster class name for the popup book</param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PopUpBook(Character character, Item item, string npcClassName, float numArg1, float numArg2)
	{
		// Verify the NPC exists in the monster database
		if (!ZoneServer.Instance.Data.MonsterDb.TryFind(npcClassName, out var monsterData))
		{
			character.SystemMessage("Invalid popup book type.");
			return ItemUseResult.Fail;
		}

		// Check if player already has a popup book active
		if (character.Variables.Temp.Has("Melia.PopupBook.Active"))
		{
			// Remove existing popup book first
			var existingMonster = character.Map.GetMonster(m =>
				m is Mob mob &&
				mob.Vars.Get<Character>("PopupBook.Owner") == character);

			if (existingMonster != null)
				character.Map.RemoveMonster(existingMonster);

			character.Variables.Temp.Remove("Melia.PopupBook.Active");
		}

		// Calculate spawn position (offset from player position)
		var spawnPos = new Position(
			character.Position.X - 45,
			character.Position.Y,
			character.Position.Z + 45
		);

		// Try to get ground height at spawn position
		if (character.Map.Ground.TryGetHeightAt(spawnPos, out var height))
			spawnPos.Y = height;

		// Create the popup book NPC
		var popupBook = new Mob(monsterData.Id, RelationType.Neutral);
		popupBook.SpawnPosition = spawnPos;
		popupBook.Position = spawnPos;
		popupBook.Direction = new Direction(315); // Face 315 degrees

		// Track ownership
		popupBook.Vars.Set("PopupBook.Owner", character);
		character.Variables.Temp.Set("Melia.PopupBook.Active", true);

		// Add movement component (required for proper spawning)
		// popupBook.Components.Add(new MovementComponent(popupBook));

		// Spawn the popup book
		character.Map.AddMonster(popupBook);
		popupBook.Components.Add(new LifeTimeComponent(popupBook, TimeSpan.FromSeconds(10)));

		return ItemUseResult.Okay;
	}
}
