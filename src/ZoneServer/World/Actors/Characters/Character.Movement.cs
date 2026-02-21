// ===================================================================
// CharacterMovement.cs - Movement, positioning and visibility
// ===================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		#region Movement Properties
		/// <summary>
		/// Specifies whether the character currently updates the visible entities around the character.
		/// </summary>
		public bool EyesOpen { get; private set; }
		#endregion

		#region Position Methods
		/// <summary>
		/// Returns character's jump type.
		/// </summary>
		public int GetJumpType() => 1;

		/// <summary>
		/// Sets character's position.
		/// </summary>
		public void SetPosition(float x, float y, float z)
			=> this.SetPosition(new Position(x, y, z));

		/// <summary>
		/// Sets character's position.
		/// </summary>
		public void SetPosition(Position pos)
		{
			this.Position = pos;
		}

		/// <summary>
		/// Sets character's direction.
		/// </summary>
		public void SetDirection(float cos, float sin)
			=> this.SetDirection(new Direction(cos, sin));

		/// <summary>
		/// Sets character's direction.
		/// </summary>
		public void SetDirection(Direction dir)
		{
			this.Direction = dir;
		}

		/// <summary>
		/// Sets character's head direction.
		/// </summary>
		public void SetHeadDirection(float cos, float sin)
			=> this.SetHeadDirection(new Direction(cos, sin));

		/// <summary>
		/// Sets character's head direction.
		/// </summary>
		public void SetHeadDirection(Direction dir)
		{
			this.HeadDirection = dir;
		}

		/// <summary>
		/// Sets direction and updates clients.
		/// </summary>
		public void Rotate(Direction dir)
		{
			if (this.Direction != dir)
				this.SetDirection(dir);
			Send.ZC_ROTATE(this);
		}

		/// <summary>
		/// Sets head direction and updates clients.
		/// </summary>
		public void RotateHead(Direction dir)
		{
			if (this.HeadDirection != dir)
				this.SetHeadDirection(dir);
			Send.ZC_HEAD_ROTATE(this);
		}

		/// <summary>
		/// Returns true if the character can move.
		/// </summary>
		public bool CanMove()
		{
			if (this.IsDead || this.IsSitting || this.IsWarping)
				return false;
			if (this.IsCasting() && !this.IsMoveableCasting())
				return false;
			if (this.IsLocked(LockType.Movement))
				return false;
			if (this.Components.TryGet<AttachmentComponent>(out var attachment)
				&& attachment.IsAttached
				&& !attachment.IsController)
				return false;
			return true;
		}

		/// <summary>
		/// Returns the character's current location.
		/// </summary>
		public Location GetLocation()
		{
			return new Location(this.MapId, this.Position);
		}

		/// <summary>
		/// Returns the character's current location as a formatted string.
		/// </summary>
		public string GetLocationToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}#{1}#{2}#{3}",
				this.Map.ClassName, (int)this.Position.X, (int)this.Position.Y, (int)this.Position.Z);
		}

		/// <summary>
		/// Returns the city return location for the actor.
		/// </summary>
		public Location GetCityReturnLocation()
		{
			MapData mapData;

			if (this.Variables.Perm.Has("Melia.CityReturnLocation.Map"))
			{
				var mapName = this.Variables.Perm.GetString("Melia.CityReturnLocation.Map");

				if (!string.IsNullOrEmpty(mapName) && ZoneServer.Instance.Data.MapDb.TryFind(mapName, out mapData))
				{
					var x = this.Variables.Perm.GetFloat("Melia.CityReturnLocation.X", 0);
					var y = this.Variables.Perm.GetFloat("Melia.CityReturnLocation.Y", 0);
					var z = this.Variables.Perm.GetFloat("Melia.CityReturnLocation.Z", 0);

					if (x == 0 && y == 0 && z == 0)
					{
						x = mapData.DefaultPosition.X;
						y = mapData.DefaultPosition.Y;
						z = mapData.DefaultPosition.Z;
					}

					return new Location(mapData.Id, x, y, z);
				}
			}

			var nearbyCity = this.Map?.Data?.NearbyCity;
			if (string.IsNullOrEmpty(nearbyCity) || !ZoneServer.Instance.Data.MapDb.TryFind(nearbyCity, out mapData))
			{
				if (!ZoneServer.Instance.Data.MapDb.TryFind("c_Klaipe", out mapData))
					throw new InvalidOperationException($"No nearby city found for map '{this.Map.ClassName}' and no fallback city found either.");
			}

			return new Location(mapData.Id, mapData.DefaultPosition);
		}

		/// <summary>
		/// Sets the character's city return location.
		/// </summary>
		public void SetCityReturnLocation(Location location)
		{
			if (!ZoneServer.Instance.Data.MapDb.TryFind(location.MapId, out var mapData))
				throw new ArgumentException($"Map '{location.MapId}' not found in data.");

			this.Variables.Perm.SetString("Melia.CityReturnLocation.Map", mapData.ClassName);
			this.Variables.Perm.SetFloat("Melia.CityReturnLocation.X", location.X);
			this.Variables.Perm.SetFloat("Melia.CityReturnLocation.Y", location.Y);
			this.Variables.Perm.SetFloat("Melia.CityReturnLocation.Z", location.Z);
		}
		#endregion

		#region Warping Methods
		/// <summary>
		/// Warps character to given location.
		/// </summary>
		public void Warp(Location location)
			=> this.Warp(location.MapId, location.Position);

		/// <summary>
		/// Warps character to given map.
		/// </summary>
		public void Warp(string mapName)
		{
			if (!ZoneServer.Instance.Data.MapDb.TryFind(mapName, out var map))
				throw new ArgumentException("Map '" + mapName + "' not found in data.");
			this.Warp(map.Id, map.DefaultPosition);
		}

		public void Warp(string mapName, double x, double y, double z)
			=> Warp(mapName, new Position((float)x, (float)y, (float)z));

		/// <summary>
		/// Warps character to given location.
		/// </summary>
		public void Warp(string mapName, Position pos)
		{
			if (!ZoneServer.Instance.Data.MapDb.TryFind(mapName, out var map))
				throw new ArgumentException("Map '" + mapName + "' not found in data.");
			this.Warp(map.Id, pos);
		}

		/// <summary>
		/// Warps character to given location.
		/// </summary>
		public void Warp(int mapId, Position pos)
		{
			lock (_warpLock)
			{
				if (this.IsWarping)
					return;
				this.IsWarping = true;
			}

			try
			{
				if (!ZoneServer.Instance.World.Maps.TryGet(mapId, out var map))
					throw new ArgumentException("Map '" + mapId + "' not found in data.");

				if (this.MapId == mapId)
				{
					this.Position = pos;
					Send.ZC_SET_POS(this);
					lock (_warpLock)
					{
						this.IsWarping = false;
					}
				}
				else
				{
					this.CancelOutOfBody();

					if (map is DynamicMap)
					{
						this.Etc.Properties.SetFloat(PropertyName.LastWarpMapID, this.Map.Id);
						mapId = map.Id;
					}
					this.MapId = mapId;
					this.Position = pos;

					Send.ZC_MOVE_ZONE(this.Connection);
				}
			}
			catch
			{
				lock (_warpLock)
				{
					this.IsWarping = false;
				}
				throw;
			}
		}

		/// <summary>
		/// Makes character warp to the same map on another channel.
		/// </summary>
		internal void WarpChannel(int channelId)
		{
			lock (_warpLock)
			{
				if (this.IsWarping)
					return;
				this.IsWarping = true;
			}

			try
			{
				_destinationChannelId = channelId;

				Send.ZC_SAVE_INFO(this.Connection);
				Send.ZC_MOVE_ZONE(this.Connection);
			}
			catch
			{
				lock (_warpLock)
				{
					this.IsWarping = false;
				}
				throw;
			}
		}


		/// <summary>
		/// Resets the flag indicating whether the character was recently saved specifically for a warp operation.
		/// </summary>
		internal void ResetWarpSaveFlag()
		{
			this.SavedForWarp = false;
		}

		/// <summary>
		/// Finalizes warp after client announced readiness.
		/// </summary>
		internal void FinalizeWarp()
		{
			if (!this.IsWarping)
			{
				Log.Warning($"Character.FinalizeWarp: Player '{this.Name}' (ID: {this.DbId}, AccountID: {this.AccountDbId}) tried to warp without permission.");
				return;
			}

			var currentConnection = this.Connection;
			if (currentConnection?.Account == null)
			{
				Log.Warning($"Character.FinalizeWarp: Player '{this.Name}' (ID: {this.DbId}) connection or account is null. Aborting warp.");
				this.IsWarping = false;
				return;
			}

			var destinationMapId = this.MapId;
			var availableZones = ZoneServer.Instance.ServerList.GetZoneServers(destinationMapId);
			if (availableZones.Length == 0)
			{
				Log.Error($"Character.FinalizeWarp: No suitable zone server found for map '{destinationMapId}' for character '{this.Name}'.");
				this.IsWarping = false;
				return;
			}

			var channelId = Math2.Clamp(0, availableZones.Length, _destinationChannelId);
			var serverInfo = availableZones[channelId];

			this.Components.Get<BuffComponent>()?.StopTempBuffs();

			Log.Info($"Character '{this.Name}' (ID: {this.DbId}) finalizing warp to Map {destinationMapId}. Saving...");

			var saveSuccess = false;

			try
			{
				if (currentConnection != null && currentConnection.Account != null &&
					ZoneServer.Instance.Database.CheckSessionKey(currentConnection.Account.Id, currentConnection.SessionKey))
				{
					ZoneServer.Instance.Database.SaveCharacterData(this);
					ZoneServer.Instance.Database.SaveAccountData(currentConnection.Account);
					this.SavedForWarp = true;
					Log.Debug($"Character '{this.Name}' (ID: {this.DbId}) save successful for warp.");
					saveSuccess = true;
				}
				else
				{
					Log.Warning($"Character.FinalizeWarp Save: Skipping save for {this.Name} (ID: {this.DbId}). Connection/Account null or session key mismatch.");
					this.SavedForWarp = false;
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Character.FinalizeWarp Save: Error saving character {this.Name} (ID: {this.DbId}): {ex}");
				this.SavedForWarp = false;
			}

			try
			{
				if (saveSuccess)
				{
					Log.Info($"Instructing client for '{this.Name}' (ID: {this.DbId}) to move to Zone Server {serverInfo.Ip}:{serverInfo.Port}, Map {destinationMapId}, Channel {channelId}.");
					Send.ZC_MOVE_ZONE_OK(this, channelId, serverInfo.Ip, serverInfo.Port, destinationMapId);
				}
				else
				{
					Log.Error($"Character.FinalizeWarp: Warp aborted for {this.Name} (ID: {this.DbId}) due to save failure.");
					this.IsWarping = false;
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Character.FinalizeWarp: Unhandled exception during warp for {this.Name} (ID: {this.DbId}): {ex}");
				this.IsWarping = false;
				this.SavedForWarp = false;
			}
		}
		#endregion

		#region Visibility Methods
		/// <summary>
		/// Returns if a character can see an actor.
		/// </summary>
		public override bool CanSee(IActor actor)
		{
			if (actor.Visibility == ActorVisibility.Always)
				return true;

			if (actor is Npc npc && this.GetMapNPCState(npc) == NpcState.Invisible)
				return false;

			if (actor is Npc npc1 && this.IsHidden(npc1))
				return false;

			return base.CanSee(actor);
		}

		/// <summary>
		/// Updates visible entities around character.
		/// </summary>
		public void LookAround()
		{
			if (!this.EyesOpen)
				return;

			lock (_lookAroundLock)
			{
				var currentlyVisibleMonsters = this.Map.GetVisibleMonsters(this);
				var currentlyVisibleCharacters = this.Map.GetVisibleCharacters(this);
				var currentlyVisiblePads = this.Map.GetVisiblePads(this);

				var appearMonsters = currentlyVisibleMonsters.Except(_visibleMonsters);
				var appearCharacters = currentlyVisibleCharacters.Except(_visibleCharacters);
				var appearPads = currentlyVisiblePads.Except(_visiblePads);

				var disappearMonsters = _visibleMonsters.Except(currentlyVisibleMonsters);
				var disappearCharacters = _visibleCharacters.Except(currentlyVisibleCharacters);
				var disappearPads = _visiblePads.Except(currentlyVisiblePads);

				this.HandleAppearingCharacters(appearCharacters);
				this.HandleDisappearingCharacters(disappearCharacters);

				// Throttle monster appearances to prevent packet storms.
				// Only send up to MaxMonsterAppearPerTick new monsters per tick.
				// Unsent monsters will reappear in the next tick's delta since
				// _visibleMonsters only tracks what was actually sent to the client.
				var appearMonsterList = appearMonsters.ToArray();
				var sentCount = 0;
				foreach (var monster in appearMonsterList)
				{
					if (sentCount >= MaxMonsterAppearPerTick)
						break;

					this.HandleAppearingSingleMonster(monster);
					sentCount++;
				}

				// Disappearances remain unlimited (ZC_LEAVE is cheap)
				this.HandleDisappearingMonsters(disappearMonsters);

				this.HandleAppearingPads(appearPads);
				this.HandleDisappearingPads(disappearPads);

				// Update _visibleMonsters to reflect only what the client
				// actually knows about: previously visible (minus disappeared)
				// plus only the monsters we actually sent this tick.
				var newVisibleMonsters = new HashSet<IMonster>(_visibleMonsters);
				foreach (var monster in disappearMonsters)
					newVisibleMonsters.Remove(monster);
				for (var i = 0; i < sentCount && i < appearMonsterList.Length; i++)
					newVisibleMonsters.Add(appearMonsterList[i]);
				_visibleMonsters = newVisibleMonsters.ToArray();

				_visibleCharacters = currentlyVisibleCharacters;
				_visiblePads = currentlyVisiblePads;
			}
		}

		private void HandleAppearingCharacters(IEnumerable<Character> appearCharacters)
		{
			foreach (var character in appearCharacters)
			{
				Send.ZC_ENTER_PC(this.Connection, character);
				Send.ZC_NORMAL.HeadgearVisibilityUpdate(this.Connection, character);

				// Send wig/hair costume look update if character has one equipped and it's visible
				var hairItem = character.Inventory.GetEquip(EquipSlot.Hair);
				var wigVisible = (character.VisibleEquip & VisibleEquip.Wig) != 0;
				if (hairItem != null && hairItem.Id != 12101 && wigVisible) // 12101 is default "no hair costume"
				{
					var strArg = hairItem.Data?.Script?.StrArg ?? "";
					if (!string.IsNullOrEmpty(strArg))
					{
						if (ZoneServer.Instance.Data.HairTypeDb.TryFind(character.Gender, strArg, out var hairData))
							Send.ZC_NORMAL.UpdateCharacterLook(this.Connection, character, hairItem.Id, EquipSlot.Hair, hairData.Index);
						else if (ZoneServer.Instance.Data.HeadTypeDb.TryFind(character.Gender, strArg, out var headData))
							Send.ZC_NORMAL.UpdateCharacterLook(this.Connection, character, hairItem.Id, EquipSlot.Hair, headData.Index);
					}
				}

				if (character.HasParty || character.HasGuild)
					Send.ZC_NORMAL.ShowParty(this.Connection, character);

				if (character.Connection.ShopCreated != null)
				{
					var shop = character.Connection.ShopCreated;
					Send.ZC_NORMAL.ShopAnimation(this.Connection, character, shop.ShopAnimation, 0, 1);
					Send.ZC_AUTOSELLER_TITLE(this.Connection, character);
				}

				if (Versions.Protocol > 500)
				{
					Send.ZC_RES_DAMAGEFONT_SKIN(this.Connection, character);
					Send.ZC_RES_DAMAGEEFFECT_SKIN(this.Connection, character);
					Send.ZC_SEND_APPLY_HUD_SKIN_OTHER(this.Connection, character);
				}

				character.ShowEffects(this.Connection);
				Send.ZC_BUFF_LIST(this.Connection, character);
			}
		}

		private void HandleDisappearingCharacters(IEnumerable<Character> disappearCharacters)
		{
			foreach (var character in disappearCharacters)
				Send.ZC_LEAVE(this.Connection, character);
		}

		private void HandleAppearingMonsters(IEnumerable<IMonster> appearMonsters)
		{
			foreach (var monster in appearMonsters)
				this.HandleAppearingSingleMonster(monster);
		}

		/// <summary>
		/// Sends all appearance packets for a single monster to this character.
		/// </summary>
		/// <param name="monster"></param>
		private void HandleAppearingSingleMonster(IMonster monster)
		{
			// Check if this monster should appear as enemy due to PvP/duel
			var shouldAppearAsEnemy = false;
			RelationType originalMonsterType = RelationType.Friendly;

			if (monster is Mob mob && monster.OwnerHandle != 0)
			{
				originalMonsterType = mob.MonsterType;

				// Check if the monster's owner is an enemy (duel or PvP)
				if (this.Map.TryGetCharacter(monster.OwnerHandle, out var ownerCharacter) && ownerCharacter != this)
				{
					// Check duel
					var duel = this.Connection?.ActiveDuel;
					if (duel != null && duel.AreDueling(this, ownerCharacter))
					{
						shouldAppearAsEnemy = true;
					}
					// Check PvP map
					else if (this.Map.IsPVP)
					{
						shouldAppearAsEnemy = true;
					}
				}

				if (shouldAppearAsEnemy)
					mob.MonsterType = RelationType.Enemy;
			}

			Send.ZC_ENTER_MONSTER(this.Connection, monster);

			// Restore original MonsterType
			if (shouldAppearAsEnemy && monster is Mob mob2)
				mob2.MonsterType = originalMonsterType;

			if (monster.Properties.TryGetFloat(PropertyName.Scale, out var scale) && scale != 1)
				monster.ChangeScale(scale, 0);

			if (monster is Mob mobBoss && mobBoss.Rank == MonsterRank.Boss)
				Send.ZC_UPDATE_SHIELD(this.Connection, monster, mobBoss.Shield);

			monster.ShowEffects(this.Connection);

			if (monster.OwnerHandle != 0)
				Send.ZC_OWNER(this, monster);

			if (monster is Summon summon)
				Send.ZC_IS_SUMMON_SORCERER_MONSTER(this, summon);

			if (monster is Companion companion)
			{
				Send.ZC_NORMAL.PetOwner(this.Connection, companion);
				if (companion.IsBird)
				{
					Send.ZC_FLY_HEIGHT(this.Connection, companion, 80);
				}
				Send.ZC_NORMAL.Pet_AssociateHandleWorldId(this.Connection, companion);
			}

			if (monster is ICombatEntity entity)
			{
				if (entity.CheckBoolTempVar("BroadcastOwner"))
					Send.ZC_OWNER(this, monster);

				Send.ZC_FACTION(this.Connection, monster, entity.Faction);

				if (entity.HasBuffs())
					Send.ZC_BUFF_LIST(this.Connection, entity);

				if (entity.Components.TryGet<MovementComponent>(out var movement) && movement.IsMoving && movement.MoveTarget == MoveTargetType.Position)
				{
					var fromCellPos = entity.Map.Ground.GetCellPosition(entity.Position);
					var toCellPos = entity.Map.Ground.GetCellPosition(movement.Destination);
					var speed = entity.Properties.GetFloat(PropertyName.MSPD);
					Send.ZC_MOVE_PATH(this, entity, fromCellPos, toCellPos, speed);
				}
			}

			if (monster is MonsterInName minMon && this.GetMapNPCState(minMon) != NpcState.IgnoreState)
				Send.ZC_SET_NPC_STATE(minMon);
		}

		private void HandleDisappearingMonsters(IEnumerable<IMonster> disappearMonsters)
		{
			foreach (var monster in disappearMonsters)
			{
				Send.ZC_LEAVE(this.Connection, monster);
				if (monster is ICombatEntity entity)
					Send.ZC_BUFF_CLEAR(this.Connection, entity);
			}
		}

		private void HandleAppearingPads(IEnumerable<Pad> appearPads)
		{
			foreach (var pad in appearPads)
			{
				if (pad.Creator is ICombatEntity creator)
				{
					// Send pad create packet to this character
					Send.ZC_NORMAL.PadUpdateToCharacter(this, creator, pad, true);
					// Track this character as an observer
					pad.Observers.AddObserver(this);
					_observedPads.Add(pad);
				}
			}
		}

		private void HandleDisappearingPads(IEnumerable<Pad> disappearPads)
		{
			// Don't remove from observers or send destroy - the player stays tracked
			// so they receive the destroy packet when the pad actually expires.
			// The client continues to render the pad even though we're out of range,
			// which is fine since it will be cleaned up when the pad is destroyed.
		}

		/// <summary>
		/// Starts auto-updates of visible entities.
		/// Performs an unthrottled initial scan so all entities
		/// appear immediately on map entry.
		/// </summary>
		public void OpenEyes()
		{
			this.EyesOpen = true;

			// Perform unthrottled initial visibility scan. Unlike
			// LookAround(), this sends all monsters at once since
			// the player expects to see everything on map entry.
			lock (_lookAroundLock)
			{
				var currentlyVisibleMonsters = this.Map.GetVisibleMonsters(this);
				var currentlyVisibleCharacters = this.Map.GetVisibleCharacters(this);
				var currentlyVisiblePads = this.Map.GetVisiblePads(this);

				this.HandleAppearingCharacters(currentlyVisibleCharacters.Except(_visibleCharacters));
				this.HandleAppearingMonsters(currentlyVisibleMonsters.Except(_visibleMonsters));
				this.HandleAppearingPads(currentlyVisiblePads.Except(_visiblePads));

				_visibleMonsters = currentlyVisibleMonsters;
				_visibleCharacters = currentlyVisibleCharacters;
				_visiblePads = currentlyVisiblePads;
			}
		}

		/// <summary>
		/// Stops auto-updates of visible entities.
		/// </summary>
		public void CloseEyes()
		{
			this.EyesOpen = false;

			lock (_lookAroundLock)
			{
				foreach (var monster in _visibleMonsters)
					Send.ZC_LEAVE(this.Connection, monster);

				foreach (var character in _visibleCharacters)
					Send.ZC_LEAVE(this.Connection, character);

				// Clean up all observed pads (includes those we walked out of range from)
				foreach (var pad in _observedPads)
					pad.Observers.RemoveObserver(this);

				_visibleMonsters = [];
				_visibleCharacters = [];
				_visiblePads = [];
				_observedPads.Clear();
			}
		}
		#endregion
	}
}
