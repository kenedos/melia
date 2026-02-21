using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Melia.Shared.Data.Database;
using Melia.Shared.Database;
using Melia.Shared.Network;
using Melia.Zone.Database;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Services;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
// using Melia.Zone.World.Houses; // Removed: Houses namespace deleted
using Melia.Zone.World.Trades;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;

namespace Melia.Zone.Network
{
	/// <summary>
	/// A connection from the client to the zone server.
	/// </summary>
	public interface IZoneConnection : IConnection
	{
		/// <summary>
		/// Gets or sets whether the player is ready to receive packets.
		/// </summary>
		bool GameReady { get; set; }

		/// <summary>
		/// Gets or sets whether the player is ready to act.
		/// </summary>
		bool LoadComplete { get; set; }

		/// <summary>
		/// Gets or sets the account associated with the connection.
		/// </summary>
		Account Account { get; set; }

		/// <summary>
		/// Gets or sets a reference to the currently controlled character.
		/// </summary>
		Character SelectedCharacter { get; set; }

		/// <summary>
		/// Gets or sets the current dialog.
		/// </summary>
		Dialog CurrentDialog { get; set; }

		/// <summary>
		/// Gets or sets the current party.
		/// </summary>
		Party Party { get; set; }

		// Removed: Guild type deleted during Laima merge
		// Guild Guild { get; set; }

		/// <summary>
		/// Gets or sets the currently shop browsed.
		/// </summary>
		ShopData ActiveShop { get; set; }

		/// <summary>
		/// Gets or sets the currently shop opened.
		/// </summary>
		ShopData ShopCreated { get; set; }

		// Removed: PersonalHouse type deleted during Laima merge
		// PersonalHouse ActiveHouse { get; set; }

		/// <summary>
		/// Gets or sets the current trade.
		/// </summary>
		Trade ActiveTrade { get; set; }

		/// <summary>
		/// Gets or sets the current duel.
		/// </summary>
		Duel ActiveDuel { get; set; }

		/// <summary>
		/// Gets or sets the last heartbeat.
		/// </summary>
		DateTime LastHeartBeat { get; set; }

		/// <summary>
		/// Generate a session key.
		/// </summary>
		/// <returns></returns>
		string GenerateSessionKey();

		/// <summary>
		/// Saves the account and character associated with this connection.
		/// </summary>
		void SaveAccountAndCharacter();
	}

	public class DummyConnection : IZoneConnection
	{
		public bool GameReady { get; set; } = true;
		public bool LoadComplete { get; set; } = true;
		public Account Account { get; set; }
		public Character SelectedCharacter { get; set; }
		public Dialog CurrentDialog { get; set; }
		public Party Party { get; set; }
		// Removed: Guild type deleted during Laima merge
		// public Guild Guild { get; set; }
		public ShopData ActiveShop { get; set; }
		public ShopData ShopCreated { get; set; }
		// Removed: PersonalHouse type deleted during Laima merge
		// public PersonalHouse ActiveHouse { get; set; }
		public DateTime LastHeartBeat { get; set; }
		public bool LoggedIn { get; set; }
		public string SessionKey { get; set; }
		public Trade ActiveTrade { get; set; }
		public Duel ActiveDuel { get; set; }

		public int IntegritySeed { get; set; }

		public void Close()
		{
			// Method intentionally left empty.
		}

		public void Close(int milliseconds)
		{
			// Method intentionally left empty.
		}

		public string GenerateSessionKey()
		{
			return "";
		}

		public void SaveAccountAndCharacter()
		{
			// Method intentionally left empty.
		}

		public void Send(Packet packet)
		{
			// Method intentionally left empty.
		}
	}

	/// <summary>
	/// A connection from the client to the zone server.
	/// </summary>
	public class ZoneConnection : Connection, IZoneConnection
	{
		/// <summary>
		/// Gets or sets whether the player is ready to receive packets.
		/// </summary>
		public bool GameReady { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the loading process has completed.
		/// </summary>
		public bool LoadComplete { get; set; }

		/// <summary>
		/// Gets or sets the account associated with the connection.
		/// </summary>
		public Account Account { get; set; }

		/// <summary>
		/// Gets or sets a reference to the currently controlled character.
		/// </summary>
		public Character SelectedCharacter { get; set; }

		/// <summary>
		/// Gets or sets the current dialog.
		/// </summary>
		public Dialog CurrentDialog { get; set; }

		/// <summary>
		/// Gets or sets the current party.
		/// </summary>
		public Party Party { get; set; }

		// Removed: Guild type deleted during Laima merge
		// public Guild Guild { get; set; }

		/// <summary>
		/// Gets or sets the currently shop browsed.
		/// </summary>
		public ShopData ActiveShop { get; set; }

		/// <summary>
		/// Gets or sets the currently shop opened.
		/// </summary>
		public ShopData ShopCreated { get; set; }

		// Removed: PersonalHouse type deleted during Laima merge
		// public PersonalHouse ActiveHouse { get; set; }

		/// <summary>
		/// Gets or sets the current trade.
		/// </summary>
		public Trade ActiveTrade { get; set; }

		/// <summary>
		/// Gets or sets the current duel.
		/// </summary>
		public Duel ActiveDuel { get; set; }

		/// <summary>
		/// Gets or sets the last heartbeat.
		/// </summary>
		public DateTime LastHeartBeat { get; set; }

		/// <summary>
		/// Generates a session key
		/// </summary>
		/// <returns></returns>
		public string GenerateSessionKey()
		{
			var character = this.SelectedCharacter;
			var date = DateTime.Now;
			var result = default(byte[]);

			using (var stream = new MemoryStream())
			{
				using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
				{
					writer.Write(date.Ticks);
					writer.Write(character.Name);
				}

				stream.Position = 0;

				using (var hash = SHA256.Create())
				{
					result = hash.ComputeHash(stream);
				}
			}

			var text = new string[20];

			for (var i = 0; i < text.Length; i++)
			{
				text[i] = result[i].ToString("X2");
			}

			this.SessionKey = "*" + string.Concat(text);
			return this.SessionKey;
		}

		/// <summary>
		/// Handles the given packet for this connection.
		/// </summary>
		/// <param name="packet"></param>
		protected override void OnPacketReceived(Packet packet)
		{
			ZoneServer.Instance.PacketHandler.Handle(this, packet);
		}

		/// <summary>
		/// Called when the connection was closed.
		/// </summary>
		/// <param name="type"></param>
		protected override void OnClosed(ConnectionCloseType type)
		{
			base.OnClosed(type);

			// --- IMMEDIATE STATE INVALIDATION ---
			// Capture references before they are nulled.
			this.LoadComplete = false;
			var account = this.Account;
			var character = this.SelectedCharacter;

			// If no character was selected, there's nothing to clean up in the world.
			if (character == null)
			{
				return;
			}

			// If character is autotrading, keep them in the world with their shop.
			// Only nullify the connection references to allow the socket to close cleanly.
			if (character.IsAutoTrading)
			{
				Log.Info($"Character '{character.Name}' entered autotrade mode. Connection closed but character remains in world.");

				// Update login state to allow other characters on same account to log in
				ZoneServer.Instance.Database.UpdateLoginState(account.Id, 0, LoginState.LoggedOut);

				// Clear connection reference on character since we're disconnecting
				character.Connection = new DummyConnection
				{
					Account = account,
					SelectedCharacter = character,
					ShopCreated = this.ShopCreated,
					Party = this.Party,
					// Guild = this.Guild, // Removed: Guild type deleted
				};

				// Nullify connection references
				this.Account = null;
				this.SelectedCharacter = null;
				this.ActiveDuel = null;
				// this.ActiveHouse = null; // Removed: PersonalHouse type deleted
				this.ActiveShop = null;
				this.ActiveTrade = null;
				this.CurrentDialog?.Cancel();
				this.CurrentDialog = null;
				// this.Guild = null; // Removed: Guild type deleted
				this.Party = null;
				this.ShopCreated = null;
				this.GameReady = false;

				return;
			}

			// Cancel all running async skill tasks (e.g. Dievdirbys statue
			// buff loops) to prevent them from keeping the character alive.
			character.Components.Get<BaseSkillComponent>()?.CancelAllRunningSkills();

			// Cancel OOBE before removing from map so the dummy body
			// is cleaned up while the character is still on the map.
			character.CancelOutOfBody();

			// 1. Clean up visibility state so pad observer references are released.
			character.CloseEyes();

			// 2. Immediately remove the character from the map.
			// This is the MOST IMPORTANT step to prevent doppelgangers.
			// By doing this synchronously, any new connection for this player
			// will not see the old character object in the world.
			Log.Info($"Character '{character.Name}' (Handle: {character.Handle}) removed from map '{character.Map?.ClassName ?? "null"}' due to connection close.");
			character.Map?.RemoveCharacter(character);

			// Clean up card registries
			Items.Effects.ItemHookRegistry.Instance.UnregisterCharacter(character);

			// 2. Update group/system statuses to reflect the offline state.
			character.IsOnline = false;
			this.Party?.UpdateMember(character, false);
			// this.Guild?.UpdateMember(character, false); // Removed: Guild type deleted

			// 3. Clean up active interactions.
			if (this.ActiveTrade != null || character.IsTrading)
			{
				ZoneServer.Instance.World.Trades.CancelTrade(character);
			}
			if (this.ActiveDuel != null || character.IsDueling)
			{
				ZoneServer.Instance.World.Duels.EndDuel(this.ActiveDuel);
			}
			character.ActiveCompanion?.Map?.RemoveMonster(character.ActiveCompanion);
			character.Summons.RemoveAllSummons();

			// 3b. Release any pending TimeAction semaphore so dialog
			// tasks waiting on StartAsync don't hang forever.
			character.Components.Get<TimeActionComponent>()?.End(TimeActionResult.Cancelled);
			character.Components.Get<TimeActionComponent>()?.Resume();

			// 4. Remove from auto-match queue if they were queued.
			// This ensures disconnected players don't block re-queuing when they log back in.
			ZoneServer.Instance.World.AutoMatch.RemoveCharacterFromQueue(character);

			// 5. Clean up card metadata registry entries for equipped cards.
			foreach (var card in character.Inventory.GetCards())
				Items.Effects.CardMetadataRegistry.Instance.Remove(card.Value.ObjectId);

			// 6. Character lock removal is deferred to after save completes (see below).

			// 7. Force end any active battle to prevent _battles dictionary from leaking.
			ZoneServer.Instance.World.BattleManager.ForceEndBattle(character);

			// 8. Remove character property event subscriptions to allow clean GC.
			character.Properties.RemoveEvents();

			// --- DEFERRED SAVE LOGIC ---
			// Enqueue the final save on the SaveQueue worker thread.
			var wasSavedForWarp = character?.SavedForWarp ?? false;
			SaveQueue.Enqueue(() =>
			{
				try
				{
					this.SaveAccountAndCharacter(account, character);
				}
				catch (Exception ex)
				{
					Log.Error($"Exception in SaveQueue save task for Account ID {account?.Id ?? 0}: {ex}");
				}
				finally
				{
					// Only remove the lock on a normal disconnect. During a
					// warp/reconnect the save is skipped here but the reconnect's
					// background save (ProcessConnect) may still be holding the
					// lock. It will be cleaned up on eventual normal disconnect.
					if (character != null && !wasSavedForWarp)
						CharacterLockManager.TryRemoveLock(character.DbId);
				}
			});

			// --- NULLIFY CONNECTION REFERENCES ---
			// This happens after the save task is kicked off, since the task needs the references.
			// The references are passed by value to the task lambda, so it's safe.
			this.Account = null;
			this.SelectedCharacter = null;
			this.ActiveDuel = null;
			// this.ActiveHouse = null; // Removed: PersonalHouse type deleted
			this.ActiveShop = null;
			this.ActiveTrade = null;
			this.CurrentDialog?.Cancel();
			this.CurrentDialog = null;
			// this.Guild = null; // Removed: Guild type deleted
			this.Party = null;
			this.ShopCreated = null;
			this.GameReady = false;
		}

		public void SaveAccountAndCharacter()
			=> this.SaveAccountAndCharacter(this.Account, this.SelectedCharacter);

		/// <summary>
		/// Saves the account and character synchronously. Used in background tasks.
		/// </summary>
		private void SaveAccountAndCharacter(Account account, Character character)
		{
			var sessionKey = this.SessionKey;

			if (account == null)
			{
				return;
			}

			// If character was warping, data was already saved. Do nothing.
			if (character != null && character.SavedForWarp)
			{
				Log.Debug($"SaveAccountAndCharacter: Skipping save for '{character.Name}', was saved for warp.");
				character.ResetWarpSaveFlag();
				return;
			}

			// Check session key against DB to prevent saving stale data after a quick reconnect.
			if (!ZoneServer.Instance.Database.CheckSessionKey(account.Id, sessionKey))
			{
				Log.Warning($"SaveAccountAndCharacter: Session key mismatch for account '{account.Name}'. The player has likely reconnected. Skipping save of stale data.");
				return;
			}

			try
			{
				// Perform final save for character if they exist.
				if (character != null)
				{
					Log.Debug($"SaveAccountAndCharacter: Performing final save for character '{character.Name}' (ID: {character.DbId}) on normal disconnect.");
					character.Components.Get<BuffComponent>()?.StopTempBuffs();
					ZoneServer.Instance.Database.SaveCharacterData(character);
				}

				// Always save account data.
				ZoneServer.Instance.Database.SaveAccountData(account);

				// Update login state to LoggedOut.
				ZoneServer.Instance.Database.UpdateLoginState(account.Id, 0, LoginState.LoggedOut);
			}
			catch (Exception ex)
			{
				Log.Error($"SaveAccountAndCharacter: Failed during final save/logout for Account ID {account.Id}, Character ID {character?.DbId ?? 0}: {ex}");
			}
			finally
			{
				character?.ResetWarpSaveFlag();
			}
		}
	}
}
