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
		/// Gets or sets the client's selected language name (e.g. "pt-BR").
		/// </summary>
		string SelectedLanguage { get; set; }

		/// <summary>
		/// Generate a session key.
		/// </summary>
		/// <returns></returns>
		string GenerateSessionKey();
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
		public string SelectedLanguage { get; set; }

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
		/// Gets or sets the client's selected language name (e.g. "pt-BR").
		/// </summary>
		public string SelectedLanguage { get; set; }

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

			var character = this.SelectedCharacter;

			if (character == null)
			{
				this.LoggedIn = false;
				this.LoadComplete = false;
				return;
			}

			// Autotrading characters stay in the world.
			if (character.IsAutoTrading)
			{
				var account = this.Account;

				ZoneServer.Instance.Database.UpdateLoginState(account.Id, 0, LoginState.LoggedOut);

				character.Connection = new DummyConnection
				{
					Account = account,
					SelectedCharacter = character,
					ShopCreated = this.ShopCreated,
					Party = this.Party,
				};

				this.LoggedIn = false;
				this.LoadComplete = false;
				this.NullifyConnectionReferences();
				return;
			}

			// If SavedForWarp is set, FinalizeWarp or CZ_MOVE_BARRACK
			// already enqueued a save. Skip the redundant save but
			// still run full cleanup — CZ_MOVE_BARRACK doesn't do
			// any cleanup itself, and these operations are safe to
			// call even if FinalizeWarp already handled some of them.
			var skipSave = character.SavedForWarp;

			this.CleanupCharacter(save: !skipSave);
		}

		/// <summary>
		/// Performs full character cleanup. Used by both normal disconnect
		/// and the dead connection sweep service to ensure consistent
		/// teardown of character state.
		/// </summary>
		/// <param name="save">Whether to save account and character data.</param>
		internal void CleanupCharacter(bool save)
		{
			var account = this.Account;
			var character = this.SelectedCharacter;

			if (character == null)
				return;

			this.LoggedIn = false;
			this.LoadComplete = false;
			character.IsWarping = false;

			character.Components.Get<BaseSkillComponent>()?.CancelAllRunningSkills();
			character.CancelOutOfBody();

			// Strip temp buffs while the character is still on the map
			// so that OnEnd handlers have full map context available.
			character.Components.Get<BuffComponent>()?.StopTempBuffs(silently: true);

			if (character.Map != null)
			{
				character.CloseEyes();
				character.Map.RemoveCharacter(character);
			}

			Items.Effects.ItemHookRegistry.Instance.UnregisterCharacter(character);

			character.IsOnline = false;
			this.Party?.UpdateMember(character, false);

			if (this.ActiveTrade != null || character.IsTrading)
				ZoneServer.Instance.World.Trades.CancelTrade(character);
			if (this.ActiveDuel != null || character.IsDueling)
				ZoneServer.Instance.World.Duels.EndDuel(this.ActiveDuel);

			foreach (var companion in character.Companions.GetList())
				companion.Map?.RemoveMonster(companion);
			character.Summons.RemoveAllSummons();

			character.Components.Get<TimeActionComponent>()?.End(TimeActionResult.Cancelled);
			character.Components.Get<TimeActionComponent>()?.Resume();

			ZoneServer.Instance.World.AutoMatch.RemoveCharacterFromQueue(character);

			foreach (var card in character.Inventory.GetCards())
				Items.Effects.CardMetadataRegistry.Instance.Remove(card.Value.ObjectId);

			ZoneServer.Instance.World.BattleManager.ForceEndBattle(character);
			character.Properties.RemoveEvents();

			if (save && account != null)
			{
				if (ZoneServer.Instance.Database.CheckSessionKey(account.Id, this.SessionKey))
				{
					ZoneServer.Instance.Database.SaveCharacterData(character);
					ZoneServer.Instance.Database.SaveAccountData(account);
					ZoneServer.Instance.Database.UpdateLoginState(account.Id, 0, LoginState.LoggedOut);
				}
				else
				{
					Log.Warning("ZoneConnection.CleanupCharacter: Skipping save for account '{0}' and character '{1}' because session key does not match.", account.Name, character?.Name ?? "NULL");
				}
			}

			CharacterLockManager.TryRemoveLock(character.DbId);

			this.NullifyConnectionReferences();
		}

		/// <summary>
		/// Nullifies all connection-level references after disconnect.
		/// </summary>
		private void NullifyConnectionReferences()
		{
			this.Account = null;
			this.SelectedCharacter = null;
			this.ActiveDuel = null;
			this.ActiveShop = null;
			this.ActiveTrade = null;
			this.CurrentDialog?.Cancel();
			this.CurrentDialog = null;
			this.Party = null;
			this.ShopCreated = null;
			this.GameReady = false;
		}

	}
}
