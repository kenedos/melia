using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Zone.Network;
using Yggdrasil.Network.Communication;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// A character's collection manager.
	/// </summary>
	/// <remarks>
	/// While we appear to manage collections on a per-character basis,
	/// they're actually account-wide.
	/// </remarks>
	public class CollectionComponent : CharacterComponent
	{
		private readonly object _syncLock = new();
		private readonly Dictionary<int, Collection> _collections = new();

		/// <summary>
		/// Clears all collections to release references for GC.
		/// </summary>
		public void Clear()
		{
			lock (_syncLock)
				_collections.Clear();
		}

		/// <summary>
		/// Returns the total number of registered collections.
		/// </summary>
		public int Count { get { lock (_syncLock) return _collections.Count; } }

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="character"></param>
		public CollectionComponent(Character character)
			: base(character)
		{
		}

		/// <summary>
		/// Returns the collection with the given id via out. Returns false if
		/// the collection doesn't exist.
		/// </summary>
		/// <param name="collectionId"></param>
		/// <param name="collection"></param>
		/// <returns></returns>
		public bool TryGet(int collectionId, out Collection collection)
		{
			lock (_syncLock)
				return _collections.TryGetValue(collectionId, out collection);
		}

		/// <summary>
		/// Returns the list of all collections the user has registered.
		/// </summary>
		/// <returns></returns>
		public List<Collection> GetList()
		{
			lock (_syncLock)
				return _collections.Values.ToList();
		}

		/// <summary>
		/// Adds a new collection. Returns false if the collection already existed.
		/// </summary>
		/// <param name="collectionId"></param>
		/// <returns></returns>
		public bool Add(int collectionId)
		{
			lock (_syncLock)
			{
				if (_collections.ContainsKey(collectionId))
					return false;

				_collections.Add(collectionId, new Collection(collectionId));

				return true;
			}
		}

		/// <summary>
		/// Check if a collection already exists.
		/// </summary>
		/// <param name="collectionId"></param>
		/// <returns></returns>
		public bool Has(int collectionId)
		{
			lock (_syncLock)
				return _collections.ContainsKey(collectionId);
		}

		/// <summary>
		/// Adds the given collection or overrides it if it already exists.
		/// </summary>
		/// <remarks>
		/// The method is primarily intended for loading collections from the
		/// database.
		/// </remarks>
		/// <param name="collectionId"></param>
		/// <param name="redeemCount"></param>
		internal Collection InitAdd(int collectionId, int redeemCount)
		{
			lock (_syncLock)
			{
				var collection = new Collection(collectionId);
				collection.RedeemCount = redeemCount;

				return _collections[collectionId] = collection;
			}
		}

		/// <summary>
		/// Registers the item to the collection without triggering checks or
		/// events.
		/// </summary>
		/// <remarks>
		/// The method is primarily intended for loading collections from the
		/// database.
		/// </remarks>
		/// <param name="collectionId"></param>
		/// <param name="itemId"></param>
		internal void InitRegisterItem(int collectionId, int itemId)
		{
			if (this.TryGet(collectionId, out var collection))
				collection.RegisterItem(itemId);
		}

		/// <summary>
		/// Returns whether the given collection was completed.
		/// </summary>
		/// <param name="collectionId"></param>
		/// <returns></returns>
		public bool IsComplete(int collectionId)
		{
			if (this.TryGet(collectionId, out var collection))
				return collection.IsComplete;

			return false;
		}

		/// <summary>
		/// Registers an item to this collection. Returns false if the collection
		/// didn't exist yet or the item is not needed.
		/// </summary>
		/// <param name="collectionId"></param>
		/// <param name="itemId"></param>
		/// <param name="silent">If true, the client is not updated.</param>
		/// <returns></returns>
		public bool RegisterItem(int collectionId, int itemId, bool silent = false)
		{
			if (!this.TryGet(collectionId, out var collection))
				return false;

			if (!collection.RegisterItem(itemId))
				return false;

			if (collection.IsComplete)
				this.OnCompleted(collection);

			return true;
		}

		/// <summary>
		/// Called when a collection was completed, grants rewards to current
		/// character.
		/// </summary>
		/// <param name="collection"></param>
		private void OnCompleted(Collection collection)
		{
			var characterPropertiesChanged = false;
			var accountPropertiesChanged = false;
			this.GrantRewards(collection, this.Character, ref characterPropertiesChanged, ref accountPropertiesChanged);
			this.SendChangedProperties(this.Character, characterPropertiesChanged, accountPropertiesChanged);
		}

		/// <summary>
		/// Grants all rewards of all collections they are eligible for to the
		/// current character.
		/// </summary>
		/// <param name="character"></param>
		public void GrantEligibleRewards()
		{
			var characterPropertiesChanged = false;
			var accountPropertiesChanged = false;

			foreach (var collection in this.GetList().Where(a => a.IsComplete))
				this.GrantRewards(collection, this.Character, ref characterPropertiesChanged, ref accountPropertiesChanged);

			this.SendChangedProperties(this.Character, characterPropertiesChanged, accountPropertiesChanged);
		}

		/// <summary>
		/// Grants the collection's rewards to the given character.
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="character"></param>
		private void GrantRewards(Collection collection, Character character, ref bool characterPropertiesChanged, ref bool accountPropertiesChanged)
		{
			if (!collection.GotPropertyBonuses(character))
				characterPropertiesChanged |= collection.GrantPropertyBonusesSilent(character);

			if (!collection.GotAccountPropertyBonuses(character))
				accountPropertiesChanged |= collection.GrantAccountPropertyBonusesSilent(character);
			else
				accountPropertiesChanged |= collection.MigrateAccountPropertyBonuses(character);
		}

		private void SendChangedProperties(Character character, bool characterPropertiesChanged, bool accountPropertiesChanged)
		{
			if (characterPropertiesChanged)
			{
				character.Properties.InvalidateAll();
				Send.ZC_OBJECT_PROPERTY(character);
			}

			if (accountPropertiesChanged && character.Connection?.Account != null)
			{
				character.Connection.Account.Properties.InvalidateAll();
				character.Connection.Account.TeamStorage?.RefreshCapacity();
				Send.ZC_NORMAL.AccountProperties(character);
			}
		}
	}

	/// <summary>
	/// Represents a collection of a player.
	/// </summary>
	public class Collection
	{
		private readonly object _syncLock = new();
		private readonly List<int> _registeredItems = new();

		/// <summary>
		/// Returns the collection's id.
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Returns a reference to the collection data.
		/// </summary>
		public CollectionData Data { get; }

		/// <summary>
		/// Returns the number of times the collection's item rewards have
		/// been redeemed.
		/// </summary>
		public int RedeemCount { get; internal set; }

		/// <summary>
		/// Returns the maximum number of times the collection's item rewards
		/// can be redeemed.
		/// </summary>
		public int RedeemMax => this.Data.RedeemMax;

		/// <summary>
		/// Returns true if the collection's item rewards have been redeemed
		/// the maximum number of times.
		/// </summary>
		public bool RedeemMaxReached => this.RedeemCount >= this.RedeemMax;

		/// <summary>
		/// Returns true if the collection is complete.
		/// </summary>
		public bool IsComplete { get { lock (_syncLock) return _registeredItems.Count >= this.Data.RequiredItems.Count; } }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="collectionId"></param>
		public Collection(int collectionId)
		{
			if (!ZoneServer.Instance.Data.CollectionDb.TryFindByClassId(collectionId, out var data))
				throw new ArgumentException($"Collection with id {collectionId} not found in data.");

			this.Id = data.Id;
			this.Data = data;
		}

		/// <summary>
		/// Returns a list of ids for the items registered to this collection.
		/// </summary>
		/// <returns></returns>
		public List<int> GetRegisteredItems()
		{
			lock (_syncLock)
				return _registeredItems.ToList();
		}

		/// <summary>
		/// Registers an item to this collection. Returns false if the item is
		/// not needed.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public bool RegisterItem(int itemId)
		{
			if (!this.Data.RequiredItems.TryGetValue(itemId, out var neededCount))
				return false;

			lock (_syncLock)
			{
				var registeredCount = _registeredItems.Count(a => a == itemId);
				var gotAll = registeredCount >= neededCount;

				if (gotAll)
					return false;

				_registeredItems.Add(itemId);
			}

			return true;
		}

		/// <summary>
		/// Grants the collection's property bonuses to the given character.
		/// </summary>
		/// <param name="character"></param>
		public void GrantPropertyBonuses(Character character)
		{
			if (!this.GrantPropertyBonusesSilent(character))
				return;

			character.Properties.InvalidateAll();
			Send.ZC_OBJECT_PROPERTY(character);
		}

		internal bool GrantPropertyBonusesSilent(Character character)
		{
			if (character == null)
				return false;

			foreach (var bonus in this.Data.RewardProperties)
				character.Properties.Modify(bonus.Key, bonus.Value);

			character.Variables.Temp.SetBool(this.GetSessionPropertyFlag(), true);
			character.Variables.Perm.SetBool(this.GetPropertyGrantedFlag(), true);
			return this.Data.RewardProperties.Count > 0;
		}

		/// <summary>
		/// Grants the collection's account property bonuses to the given
		/// character's account.
		/// </summary>
		/// <param name="character"></param>
		public void GrantAccountPropertyBonuses(Character character)
		{
			if (!this.GrantAccountPropertyBonusesSilent(character))
				return;

			character.Connection.Account.Properties.InvalidateAll();
			character.Connection.Account.TeamStorage?.RefreshCapacity();
			Send.ZC_NORMAL.AccountProperties(character);
		}

		internal bool GrantAccountPropertyBonusesSilent(Character character)
		{
			var account = character?.Connection?.Account;
			if (account == null)
				return false;

			foreach (var bonus in this.Data.RewardAccountProperties)
				account.Properties.Modify(bonus.Key, bonus.Value);

			account.Variables.Perm.SetBool(this.GetPropertyGrantedFlag(), true);
			account.Variables.Perm.SetBool(this.GetAccountPropertyMigrationFlag(), true);
			return this.Data.RewardAccountProperties.Count > 0;
		}

		internal bool MigrateAccountPropertyBonuses(Character character)
		{
			var account = character?.Connection?.Account;
			if (account == null || this.Data.RewardMigrationVersion < 1 || account.Variables.Perm.GetBool(this.GetAccountPropertyMigrationFlag()))
				return false;

			foreach (var bonus in this.Data.LegacyRewardAccountProperties)
				account.Properties.Modify(bonus.Key, -bonus.Value);

			foreach (var bonus in this.Data.RewardAccountProperties)
				account.Properties.Modify(bonus.Key, bonus.Value);

			account.Variables.Perm.SetBool(this.GetAccountPropertyMigrationFlag(), true);
			return this.Data.LegacyRewardAccountProperties.Count > 0 || this.Data.RewardAccountProperties.Count > 0;
		}

		/// <summary>
		/// Grants the collection's item rewards to the given character.
		/// </summary>
		/// <param name="character"></param>
		public void GrantItemRewards(Character character)
		{
			var rewardItems = this.Data.RewardItems;

			foreach (var rewardItem in rewardItems)
			{
				var itemId = rewardItem.Key;
				var amount = rewardItem.Value;

				character.Inventory.Add(itemId, amount);
			}

			this.RedeemCount++;
		}

		/// <summary>
		/// Returns whether the character was already granted the collection's
		/// property bonuses.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool GotPropertyBonuses(Character character)
		{
			return character != null && character.Variables.Temp.GetBool(this.GetSessionPropertyFlag());
		}

		/// <summary>
		/// Returns whether the character was already granted the collection's
		/// account property bonuses.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool GotAccountPropertyBonuses(Character character)
		{
			return character?.Connection?.Account != null && character.Connection.Account.Variables.Perm.GetBool(this.GetPropertyGrantedFlag());
		}

		private string GetPropertyGrantedFlag() => "Melia.Collections.GotProperties_" + this.Id;
		private string GetSessionPropertyFlag() => "Melia.Collections.AppliedProperties_" + this.Id;
		private string GetAccountPropertyMigrationFlag() => "Melia.Collections.AccountRewardMigrationV1_" + this.Id;
	}
}
