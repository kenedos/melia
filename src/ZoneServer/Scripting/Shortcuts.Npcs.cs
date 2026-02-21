using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotRecast.Recast;
using System.Timers;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Melia.Zone.Network;

namespace Melia.Zone.Scripting
{
	public static partial class Shortcuts
	{
		private static long UniqueNpcNameId = 0;

		/// <summary>
		/// A function that initializes a shop.
		/// </summary>
		/// <param name="shop"></param>
		public delegate void ShopCreationFunc(ShopBuilder shop);

		/// <summary>
		/// Returns an option element, to be used with the Menu function.
		/// </summary>
		/// <param name="text">Text to display in the menu.</param>
		/// <param name="key">Key to return if the option was selected.</param>
		/// <returns></returns>
		public static DialogOption Option(string text, string key)
			=> new(text, key);

		public static DialogOption Option(string text, string key, Func<bool> enabledPredicate)
			=> new(text, key) { Enabled = enabledPredicate };

		/// <summary>
		/// Adds new NPC to the world.
		/// </summary>
		/// <remarks>
		/// Used in generated scripts.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="dialogFuncName"></param>
		/// <param name="enterFuncName"></param>
		/// <param name="leaveFuncName"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddNpc(Character character, int monsterId, string name, string map, double x, double y, double z, double direction, string dialogFuncName = "", string enterFuncName = "", string leaveFuncName = "", int state = -2, double range = 100, double scale = 1)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((float)x, (float)y, (float)z);

			// Wrap name in localization code if applicable
			if (Dialog.IsLocalizationKey(name))
			{
				name = Dialog.WrapLocalizationKey(name);
			}
			// Insert line breaks in tagged NPC names that don't have one
			else if (name.StartsWith('[') && !name.Contains("{nl}"))
			{
				var endIndex = name.LastIndexOf("] ");
				if (endIndex != -1)
				{
					// Remove space and insert new line instead.
					name = name.Remove(endIndex + 1, 1);
					name = name.Insert(endIndex + 1, "{nl}");
				}
			}

			var location = new Location(mapObj.Id, pos);
			var dir = new Direction(direction);

			ZoneServer.Instance.DialogFunctions.TryGet(dialogFuncName, out var dialog);
			ZoneServer.Instance.TriggerFunctions.TryGet(enterFuncName, out var enter);
			ZoneServer.Instance.TriggerFunctions.TryGet(leaveFuncName, out var leave);

			var uniqueId = Interlocked.Increment(ref UniqueNpcNameId);
			var uniqueName = $"__NPC{uniqueId}__";
			var monster = new Npc(monsterId, name, location, dir, 0);
			monster.UniqueName = uniqueName;
			if (dialog != null)
			{
				monster.SetClickTrigger(dialogFuncName, dialog);
				var uniqueDialogName = $"{dialogFuncName}_{mapObj.Data.ClassName}";
				// Account for multiple npcs using the same dialogue.
				ZoneServer.Instance.World.NPCs.TryAdd(uniqueDialogName, monster);
			}
			if (enter != null || leave != null)
				monster.SetTriggerArea(Spot(monster.Position.X, monster.Position.Z, range));
			if (enter != null)
				monster.SetEnterTrigger(enterFuncName, enter);
			if (leave != null)
				monster.SetLeaveTrigger(leaveFuncName, leave);

			if (state != -2)
				monster.State = (NpcState)state;
			if (range != 0)
				monster.Properties.SetFloat(PropertyName.Range, (float)range);
			if (scale != 1)
				monster.Properties.SetFloat(PropertyName.Scale, (float)scale);

			monster.SetVisibilty(ActorVisibility.Track, character.ObjectId);
			monster.AddEffect(new ScriptInvisibleEffect());
			monster.Layer = character.Layer;

			mapObj.AddMonster(monster);

			return monster;
		}

		/// <summary>
		/// Adds new NPC to the world.
		/// </summary>
		/// <remarks>
		/// Used in generated scripts.
		/// </remarks>
		/// <param name="genType"></param>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="dialogFuncName"></param>
		/// <param name="enterFuncName"></param>
		/// <param name="leaveFuncName"></param>
		/// <param name="status"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddNpc(int genType, int monsterId, string name, string map, double x, double y, double z, double direction, string dialogFuncName = "", string enterFuncName = "", string leaveFuncName = "", int state = -2, double range = 100, double scale = 1)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((float)x, (float)y, (float)z);

			// Wrap name in localization code if applicable
			if (Dialog.IsLocalizationKey(name))
			{
				name = Dialog.WrapLocalizationKey(name);
			}
			// Insert line breaks in tagged NPC names that don't have one
			else if (name.StartsWith('[') && !name.Contains("{nl}"))
			{
				var endIndex = name.LastIndexOf("] ");
				if (endIndex != -1)
				{
					// Remove space and insert new line instead.
					name = name.Remove(endIndex + 1, 1);
					name = name.Insert(endIndex + 1, "{nl}");
				}
			}

			var location = new Location(mapObj.Id, pos);
			var dir = new Direction(direction);

			ZoneServer.Instance.DialogFunctions.TryGet(dialogFuncName, out var dialog);
			ZoneServer.Instance.TriggerFunctions.TryGet(enterFuncName, out var enter);
			ZoneServer.Instance.TriggerFunctions.TryGet(leaveFuncName, out var leave);

			var uniqueId = Interlocked.Increment(ref UniqueNpcNameId);
			var uniqueName = $"__NPC{uniqueId}__";
			var monster = new Npc(monsterId, name, location, dir, genType);
			monster.UniqueName = uniqueName;
			if (dialog != null)
			{
				monster.SetClickTrigger(dialogFuncName, dialog);
				var uniqueDialogName = $"{dialogFuncName}_{mapObj.Data.ClassName}";
				// Account for multiple npcs using the same dialogue.
				if (!ZoneServer.Instance.World.NPCs.ContainsKey(uniqueDialogName))
					ZoneServer.Instance.World.NPCs.TryAdd(uniqueDialogName, monster);
			}
			if (enter != null || leave != null)
				monster.SetTriggerArea(Spot(monster.Position.X, monster.Position.Z, range));
			if (enter != null)
				monster.SetEnterTrigger(enterFuncName, enter);
			if (leave != null)
				monster.SetLeaveTrigger(leaveFuncName, leave);

			if (state != -2)
				monster.State = (NpcState)state;
			if (range != 0)
				monster.Properties.SetFloat(PropertyName.Range, (float)range);
			if (scale != 1)
				monster.Properties.SetFloat(PropertyName.Scale, (float)scale);
			// Ausrine Statues to be Highlighted on the minimap
			if (monster.Id == 154039)
				monster.State = NpcState.Highlighted;

			mapObj.AddMonster(monster);

			return monster;
		}

		/// <summary>
		/// Adds new NPC to the world.
		/// </summary>
		/// <remarks>
		/// Used in generated scripts.
		/// </remarks>
		public static Npc AddNpc(int monsterId, string name, string map, double x, double y, double z, double direction, string dialogFuncName = "", string enterFuncName = "", string leaveFuncName = "", int status = -1, double range = 100)
		{
			return AddNpc(0, monsterId, name, map, x, y, z, direction, dialogFuncName, enterFuncName, leaveFuncName, status, range);
		}

		/// <summary>
		/// Adds new NPC to the world.
		/// </summary>
		/// <remarks>
		/// Used in generated scripts.
		/// </remarks>
		public static Npc AddNpc(int monsterId, string map, double x, double y, double z, double direction, string dialogFuncName = "", string enterFuncName = "", string leaveFuncName = "", int status = -1, double range = 100)
		{
			return AddNpc(0, monsterId, "", map, x, y, z, direction, dialogFuncName, enterFuncName, leaveFuncName, status, range);
		}

		/// <summary>
		/// Adds new NPC to the world.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="dialogFuncName"></param>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddNpc(int monsterId, string name, string map, double x, double z, double direction, string dialogFuncName, string enterFuncName = "", string leaveFuncName = "", double range = 100)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((float)x, 0, (float)z);
			if (mapObj.Ground.TryGetHeightAt(pos, out var height))
				pos.Y = height;

			// Wrap name in localization code if applicable
			if (Dialog.IsLocalizationKey(name))
			{
				name = Dialog.WrapLocalizationKey(name);
			}
			// Insert line breaks in tagged NPC names that don't have one
			else if (name.StartsWith("[") && !name.Contains("{nl}"))
			{
				var endIndex = name.LastIndexOf("] ");
				if (endIndex != -1)
				{
					// Remove space and insert new line instead.
					name = name.Remove(endIndex + 1, 1);
					name = name.Insert(endIndex + 1, "{nl}");
				}
			}

			var location = new Location(mapObj.Id, pos);
			var dir = new Direction(direction);
			ZoneServer.Instance.DialogFunctions.TryGet(dialogFuncName, out var dialog);
			ZoneServer.Instance.TriggerFunctions.TryGet(enterFuncName, out var enter);
			ZoneServer.Instance.TriggerFunctions.TryGet(leaveFuncName, out var leave);

			var uniqueId = Interlocked.Increment(ref UniqueNpcNameId);
			var uniqueName = $"__NPC{uniqueId}__";
			var monster = new Npc(monsterId, name, location, dir);
			monster.UniqueName = uniqueName;
			if (dialog != null)
			{
				monster.SetClickTrigger(dialogFuncName, dialog);
				var uniqueDialogName = $"{dialogFuncName}_{mapObj.Data.ClassName}";
				// Account for multiple npcs using the same dialogue.
				if (!ZoneServer.Instance.World.NPCs.ContainsKey(uniqueDialogName))
					ZoneServer.Instance.World.NPCs.TryAdd(uniqueDialogName, monster);
			}
			if (enter != null || leave != null)
				monster.SetTriggerArea(Spot(monster.Position.X, monster.Position.Z, range));
			if (enter != null)
				monster.SetEnterTrigger(enterFuncName, enter);
			if (leave != null)
				monster.SetLeaveTrigger(leaveFuncName, leave);

			mapObj.AddMonster(monster);

			return monster;
		}

		/// <summary>
		/// Adds new NPC to the world.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="dialog"></param>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddNpc(int monsterId, string name, string map, double x, double z, double direction, DialogFunc dialog = null)
		{
			var uniqueId = Interlocked.Increment(ref UniqueNpcNameId);
			var uniqueName = $"__NPC{uniqueId}__";

			return AddNpc(monsterId, name, uniqueName, map, x, z, direction, dialog);
		}

		/// <summary>
		/// Adds new NPC to the world.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="uniqueName"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="dialog"></param>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddNpc(int monsterId, string name, string uniqueName, string map, double x, double z, double direction, DialogFunc dialog = null)
		{
			var mapObj = GetMapOrThrow(map);

			if (ZoneServer.Instance.World.TryGetMonster(a => a.UniqueName == uniqueName, out _))
				throw new ArgumentException($"An NPC with the unique name '{uniqueName}' already exists.");

			var pos = new Position((float)x, 0, (float)z);
			if (mapObj.Ground.TryGetHeightAt(pos, out var height))
				pos.Y = height;

			// Wrap name in localization code if applicable
			if (Dialog.IsLocalizationKey(name))
			{
				name = Dialog.WrapLocalizationKey(name);
			}
			// Insert line breaks in tagged NPC names that don't have one
			else if (name.StartsWith('[') && !name.Contains("{nl}"))
			{
				var endIndex = name.LastIndexOf("] ");
				if (endIndex != -1)
				{
					// Remove space and insert new line instead.
					name = name.Remove(endIndex + 1, 1);
					name = name.Insert(endIndex + 1, "{nl}");
				}
			}

			var location = new Location(mapObj.Id, pos);
			var dir = new Direction(direction);

			var npc = new Npc(monsterId, name, location, dir);
			npc.UniqueName = uniqueName;

			if (dialog != null)
				npc.SetClickTrigger("DYNAMIC_DIALOG", dialog);

			// Log.Debug("Npc {0} ({1}) added to map {2} at {3}.", npc.Name, npc.Id, mapObj.Data.ClassName, pos);

			mapObj.AddMonster(npc);

			return npc;
		}

		/// <summary>
		/// Creates a custom shop.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="creationFunc"></param>
		public static void CreateShop(string name, ShopCreationFunc creationFunc)
		{
			var shopBuilder = new ShopBuilder(name);
			creationFunc(shopBuilder);

			// TODO: Not a big fan of dynamically modifying the data.
			//   Perhaps we should create shop objects based on the
			//   data and add to that, where we could also do more
			//   things without poluting the data classes.

			var shopData = shopBuilder.Build();
			ZoneServer.Instance.Data.ShopDb.AddOrReplace(shopData.Name, shopData);
		}

		/// <summary>
		/// A function that initializes a companion shop.
		/// </summary>
		/// <param name="shop"></param>
		public delegate void CompanionShopCreationFunc(CompanionShopBuilder shop);

		/// <summary>
		/// Creates a custom companion shop.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="creationFunc"></param>
		public static void CreateCompanionShop(string name, CompanionShopCreationFunc creationFunc)
		{
			var shopBuilder = new CompanionShopBuilder(name);
			creationFunc(shopBuilder);

			var shopData = shopBuilder.Build();
			ZoneServer.Instance.Data.CompanionShopDb.AddOrReplace(shopData.Name, shopData);
		}

		public static Npc AddAreaTrigger(string map, double x, double z, double radius, TriggerActorFuncAsync triggerFunc = null)
		{
			var mapObj = GetMapOrThrow(map);

			var originPos = new Position((float)x, 0, (float)z);
			if (mapObj.Ground.TryGetHeightAt(originPos, out var height))
				originPos.Y = height;

			var area = new CircleF(originPos, (float)radius);

			var uniqueId = Interlocked.Increment(ref UniqueNpcNameId);
			var uniqueName = $"__NPC{uniqueId}__";
			var trigger = new Npc(12082, "", new Location(mapObj.Id, originPos), Direction.East);
			trigger.UniqueName = uniqueName;
			trigger.SetTriggerArea(area);
			trigger.SetEnterTrigger($"{uniqueName}_ENTER", triggerFunc);

			mapObj.AddMonster(trigger);

			return trigger;
		}

		/// <summary>
		/// Helper function to create a treasure chest with a specific item and amount.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <param name="name"></param>
		/// <param name="monsterId">
		/// 147392 - "Treasure Box 1"
		/// 147393 - "Treasure Box 3"
		/// 147394 - "Treasure Box 4"
		/// 147395 - "Treasure Box 5"
		/// 151167 - "Treasure Box 5 Event" (Same as Treasure Box 5)
		/// 152019 - "Small Treasure Chest"
		/// 154098 - "Treasure Chest"
		/// 3010089 - "Ocean Treasure Chest"
		/// 40030 - "Treasure Chest"
		/// 40032 - "Treasure Chest Black"
		/// 40033 - "Treasure Chest Blue"
		/// 40034 - "Treasure Chest Green"
		/// 40035 - "Treasure Chest Yellow"
		/// </param>
		/// <returns></returns>
		public static Npc AddTreasureChest(string uniqueName, string map, double x, double z, double direction, int itemId, int amount = 1, string name = "Treasure Chest", int monsterId = 147392)
		{
			var mapObj = GetMapOrThrow(map);

			if (ZoneServer.Instance.World.TryGetMonster(a => a.UniqueName == uniqueName, out _))
				throw new ArgumentException($"An NPC with the unique name '{uniqueName}' already exists.");

			var chest = AddNpc(monsterId, name, uniqueName, map, x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var npc = dialog.Npc;

				if (npc.Vars.ActivateOnce($"Npc.{uniqueName}"))
				{
					await OpenChest(character, npc, true);
					character.Inventory.Add(itemId, amount, InventoryAddType.PickUp);
				}
			});

			return chest;
		}

		/// <summary>
		/// Helper function to create a treasure chest with a specific item
		/// and amount at a specific Y coordinate. Use this when placing chests
		/// at specific heights.
		/// </summary>
		public static Npc AddFloatingTreasureChest(string uniqueName, string map, double x, double y, double z, double direction, int itemId, int amount = 1, string name = "Treasure Chest", int monsterId = 147392)
		{
			var mapObj = GetMapOrThrow(map);

			if (ZoneServer.Instance.World.TryGetMonster(a => a.UniqueName == uniqueName, out _))
				throw new ArgumentException($"An NPC with the unique name '{uniqueName}' already exists.");

			var pos = new Position((float)x, (float)y, (float)z);
			var location = new Location(mapObj.Id, pos);
			var dir = new Direction(direction);

			// Wrap name in localization code if applicable
			if (Dialog.IsLocalizationKey(name))
			{
				name = Dialog.WrapLocalizationKey(name);
			}
			// Insert line breaks in tagged NPC names that don't have one
			else if (name.StartsWith('[') && !name.Contains("{nl}"))
			{
				var endIndex = name.LastIndexOf("] ");
				if (endIndex != -1)
				{
					// Remove space and insert new line instead.
					name = name.Remove(endIndex + 1, 1);
					name = name.Insert(endIndex + 1, "{nl}");
				}
			}

			var chest = new Npc(monsterId, name, location, dir);
			chest.UniqueName = uniqueName;

			chest.SetClickTrigger("DYNAMIC_DIALOG", async dialog =>
			{
				var character = dialog.Player;
				var npc = dialog.Npc;

				if (npc.Vars.ActivateOnce($"Npc.{uniqueName}"))
				{
					await OpenChest(character, npc, true);
					character.Inventory.Add(itemId, amount, InventoryAddType.PickUp);
				}
			});

			mapObj.AddMonster(chest);
			return chest;
		}

		/// <summary>
		/// Creates a treasure chest spawner with random respawn time.
		/// The chest will spawn immediately at ground level, then respawn 
		/// with a random delay between minRespawnHours and maxRespawnHours
		/// after being opened.
		/// </summary>
		/// <remarks>
		/// A same chest can be opened more than once by same player
		/// </remarks>
		/// <param name="uniqueName"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <param name="minRespawnHours"></param>
		/// <param name="maxRespawnHours"></param>
		/// <param name="name"></param>
		/// <param name="monsterId"></param>
		/// <exception cref="ArgumentException"></exception>
		public static void AddTreasureChestSpawner(
			   string uniqueName,
			   string map,
			   double x,
			   double z,
			   double direction,
			   int itemId,
			   int amount = 1,
			   double minRespawnHours = 3.0,
			   double maxRespawnHours = 6.0,
			   string name = "Treasure Chest",
			   int monsterId = 147392)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((float)x, 0, (float)z);
			if (mapObj.Ground.TryGetHeightAt(pos, out var height))
				pos.Y = height;

			var minRespawn = TimeSpan.FromHours(minRespawnHours);
			var maxRespawn = TimeSpan.FromHours(maxRespawnHours);

			var spawner = new World.Spawning.FloatingTreasureChestSpawner(
					uniqueName,
					map,
					pos.X,
					pos.Y,
					pos.Z,
					direction,
					itemId,
					amount,
					minRespawn,
					maxRespawn,
					name,
					monsterId
			);

			ZoneServer.Instance.World.AddSpawner(spawner);
		}

		/// <summary>
		/// Creates a floating treasure chest spawner with random respawn time.
		/// The chest will spawn immediately, then respawn with a random delay between
		/// minRespawnHours and maxRespawnHours after being opened.
		/// </summary>
		/// <remarks>
		/// A same chest can be opened more than once by same player
		/// </remarks>
		/// <param name="uniqueName"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <param name="minRespawnHours"></param>
		/// <param name="maxRespawnHours"></param>
		/// <param name="name"></param>
		/// <param name="monsterId"></param>
		public static void AddFloatingTreasureChestSpawner(
				string uniqueName,
				string map,
				double x,
				double y,
				double z,
				double direction,
				int itemId,
				int amount = 1,
				double minRespawnHours = 3.0,
				double maxRespawnHours = 6.0,
				string name = "Treasure Chest",
				int monsterId = 147392)
		{
			var minRespawn = TimeSpan.FromHours(minRespawnHours);
			var maxRespawn = TimeSpan.FromHours(maxRespawnHours);

			var spawner = new World.Spawning.FloatingTreasureChestSpawner(
					uniqueName,
					map,
					x,
					y,
					z,
					direction,
					itemId,
					amount,
					minRespawn,
					maxRespawn,
					name,
					monsterId
			);

			// Check if spawner with this ID already exists (from previous script load)
			var existingSpawner = ZoneServer.Instance.World.GetSpawners()
				.FirstOrDefault(s => s.Id == spawner.Id);

			if (existingSpawner != null)
			{
				// Remove old spawner before adding new one
				ZoneServer.Instance.World.RemoveSpawner(existingSpawner);
			}

			ZoneServer.Instance.World.AddSpawner(spawner);
		}

		/// <summary>
		/// Creates a temporary NPC owned by and visible to a specific character.
		/// </summary>
		/// <param name="owner">The character who is creating and will own the NPC.</param>
		/// <param name="className">The class name of the monster/NPC to spawn.</param>
		/// <param name="position">The position where the NPC will be spawned.</param>
		/// <param name="direction">The direction the NPC will face.</param>
		/// <param name="dialogFuncName">The name of the dialog function for the NPC.</param>
		/// <param name="tacticsFuncName">The name of the tactics/AI function for the NPC.</param>
		/// <returns>The created Npc instance, or null if creation failed.</returns>
		public static Npc AddNpc(Character owner, string className, Position position, double direction, string dialogFuncName = null, string tacticsFuncName = null, TimeSpan lifeTime = default)
		{
			// 1. Find the Monster/NPC data from the database using its class name.
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(m => m.ClassName == className, out var npcData))
			{
				Log.Warning($"CreateTemporaryNpc: Could not find NPC data for ClassName '{className}'.");
				return null;
			}

			var map = owner.Map;
			var location = new Location(map.Id, position);
			var dir = new Direction(direction);

			var npc = new Npc(npcData.Id, owner.Name, location, dir);

			if (!string.IsNullOrEmpty(dialogFuncName) && ZoneServer.Instance.DialogFunctions.TryGet(dialogFuncName, out var dialogFunc))
			{
				npc.SetClickTrigger(dialogFuncName, dialogFunc);
			}

			if (!string.IsNullOrEmpty(tacticsFuncName))
			{
				// Assuming a 'SetTactics' or similar method exists.
				// If not, this part would need to be implemented.
				// npc.SetTactics(tacticsFuncName);
			}

			npc.SetVisibilty(ActorVisibility.Individual, owner.ObjectId);

			npc.Layer = owner.Layer;
			npc.OwnerHandle = owner.Handle;
			npc.AssociatedHandle = owner.Handle;

			map.AddMonster(npc);

			if (lifeTime != default)
			{
				npc.DisappearTime = DateTime.Now.Add(lifeTime);
			}

			return npc;
		}

		/// <summary>
		/// Adds a Track NPC, these are elevators, cable cars, 
		/// moving platforms, etc. They work with the "Track" system of the
		/// client. The client handles ALL of the track position calculation
		/// and traversing.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="trackString"></param>
		/// <param name="i1"></param>
		/// <param name="i2"></param>
		/// <returns></returns>
		public static Npc AddTrackNPC(int monsterId, string name, string map, double x, double y, double z, double direction, string trackString, int i1 = 2, int i2 = 5)
		{
			if (string.IsNullOrEmpty(map) || map == "None")
			{
				Log.Debug($"Skipped adding Track NPC {monsterId} - {name} at {x},{y},{z} because of invalid map: {map}");
				return null;
			}
			var npc = AddNpc(0, monsterId, name, map, x, y, z, direction);
			npc.Visibility = ActorVisibility.Always;
			npc.AddEffect(new ReviveEffect());
			npc.AddEffect(new SetTrackPosition());
			npc.AddEffect(new DirectionAPC(trackString, i1, i2));
			//if (ZoneServer.Instance.Data.MapDb.TryFind(map, out var mapData))
			//Log.Debug($"Adding Track NPC {monsterId} - {name} at {x},{y},{z} on {mapData.Name}");
			return npc;
		}

		/// <summary>
		/// Adds an NPC that starts a track when talked to and
		/// teleports the interacting player to a position.
		/// </summary>
		/// <param name="monsterId">The monster/NPC ID</param>
		/// <param name="name">The NPC name</param>
		/// <param name="map">The map name</param>
		/// <param name="x">X coordinate</param>
		/// <param name="z">Z coordinate</param>
		/// <param name="direction">Direction the NPC faces</param>
		/// <param name="trackId">The track ID to start</param>
		/// <param name="warpX">X coordinate to warp player to</param>
		/// <param name="warpY">Y coordinate to warp player to</param>
		/// <param name="warpZ">Z coordinate to warp player to</param>
		/// <returns>The created NPC</returns>
		public static Npc AddTrackStartingNPC(int monsterId, string name, string map, double x, double z, double direction, string trackId, float warpX, float warpY, float warpZ)
		{
			var mapObj = GetMapOrThrow(map);

			return AddNpc(monsterId, name, map, x, z, direction, async dialog =>
			{
				var player = dialog.Player;
				var connection = player.Connection;

				connection.CurrentDialog?.DisposeAsync();

				// Small delay then start track
				await Task.Delay(100);
				player.Warp(map, new Position(warpX, warpY, warpZ));
				await player.Tracks.Start(trackId, TimeSpan.Zero);
			});
		}

		/// <summary>
		/// Creates a floating platform with a colored glowing effect.
		/// Uses Hidden_pillar (MonsterId 154004) with an attached ground effect.
		/// Perfect for creating Rainbow Road-style jumping puzzles or floating pathways.
		/// </summary>
		/// <param name="map">The map classname where the platform will be spawned.</param>
		/// <param name="x">X coordinate of the platform.</param>
		/// <param name="y">Y coordinate of the platform (height above ground).</param>
		/// <param name="z">Z coordinate of the platform.</param>
		/// <param name="direction">Direction the platform faces (typically 0).</param>
		/// <param name="color">Color of the platform: "white" (default), "red", "blue", "green", or "yellow".</param>
		/// <param name="moveSpeed">Optional movement speed if this platform will move (0 = stationary).</param>
		/// <param name="movementFunc">Optional async function to control platform movement.</param>
		/// <returns>The created floating platform NPC with attached effect.</returns>
		public static Npc AddPlatformNpc(string map, double x, double y, double z, double direction = 0, string color = "white", float moveSpeed = 0, Func<Npc, Task> movementFunc = null)
		{
			var mapObj = GetMapOrThrow(map);

			var effectName = color.ToLower() switch
			{
				"red" => "F_ground008_red",
				"blue" => "F_ground008_blue",
				"green" => "F_ground008_green",
				"yellow" => "F_ground008_yellow",
				_ => "F_ground008"
			};

			var pos = new Position((float)x, (float)y, (float)z);
			var location = new Location(mapObj.Id, pos);
			var dir = new Direction(direction);

			var uniqueId = Interlocked.Increment(ref UniqueNpcNameId);
			var uniqueName = $"__OBB_PLATFORM{uniqueId}__";

			// Space as name hides the NPC's name in client
			var platform = new Npc(154004, " ", location, dir);
			platform.UniqueName = uniqueName;
			platform.AddEffect(new AttachEffect(effectName, 3.0f, EffectLocation.Bottom));

			// Configure movement if speed is set
			if (moveSpeed > 0)
			{
				platform.AllowMovement = true;

				// Set movement speed properties
				platform.Properties.SetFloat(PropertyName.WlkMSPD, moveSpeed);
				platform.Properties.SetFloat(PropertyName.RunMSPD, moveSpeed);
				platform.Properties.SetFloat(PropertyName.MSPD, moveSpeed);

				// Add MovementComponent to enable movement
				platform.Components.Add(new MovementComponent(platform));
			}

			mapObj.AddMonster(platform);

			// Start movement function if provided
			if (movementFunc != null)
				Task.Run(() => movementFunc(platform));

			return platform;
		}

		/// <summary>
		/// Creates a moving platform that loops between two positions.
		/// </summary>
		/// <param name="map">The map classname.</param>
		/// <param name="startPos">Starting position of the platform.</param>
		/// <param name="destPos">Destination position to move to.</param>
		/// <param name="waitTime">How long to wait at each position.</param>
		/// <param name="moveSpeed">Movement speed in units per second.</param>
		/// <param name="direction">Platform facing direction.</param>
		/// <param name="color">Platform color effect.</param>
		/// <returns>The created moving platform.</returns>
		public static Npc AddMovingPlatformNpc(string map, Position startPos, Position destPos,
			TimeSpan waitTime, float moveSpeed = 50, double direction = 0, string color = "white")
		{
			return AddPlatformNpc(map, startPos.X, startPos.Y, startPos.Z, direction, color, 0, async platform =>
			{
				// Don't use MovementComponent - we'll manually control position
				var isAtStart = true;
				while (true)
				{
					try
					{
						await Task.Delay(waitTime);

						var currentPos = isAtStart ? startPos : destPos;
						var targetPos = isAtStart ? destPos : startPos;

						// Calculate movement duration using 3D distance to account for vertical movement
						var distance = currentPos.Get3DDistance(targetPos);
						var duration = TimeSpan.FromSeconds(distance / moveSpeed);

						// Get ground height at target position and calculate height offset
						platform.Map.Ground.TryGetHeightAt(targetPos, out var groundHeight);
						var heightOffset = targetPos.Y - groundHeight;

						// Set the platform's height above ground
						Send.ZC_FLY_HEIGHT(platform, heightOffset);

						// Send movement packet using cell positions
						var fromCell = platform.Map.Ground.GetCellPosition(currentPos);
						var toCell = platform.Map.Ground.GetCellPosition(targetPos);
						Send.ZC_MOVE_PATH(platform, fromCell, toCell, moveSpeed, autoRotate: false);

						// Wait for movement to complete, then snap to exact position
						await Task.Delay(duration);
						platform.Position = targetPos;

						isAtStart = !isAtStart;
					}
					catch (Exception ex)
					{
						Log.Error($"AddMovingPlatformNpc: {ex.Message}");
						break;
					}
				}
			});
		}
	}
}
