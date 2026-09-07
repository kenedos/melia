using System;
using System.Collections.Generic;
using System.Text;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Storages;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Pardoner's Oblation offering box.
	/// </summary>
	public static class PardonerSkillHelper
	{
		/// <summary>
		/// How long the church takes to accept a new set of offerings.
		/// </summary>
		public static readonly TimeSpan ChurchDonationCooldown = TimeSpan.FromSeconds(24);

		/// <summary>
		/// How long a full offering box is left standing before its shop
		/// closes itself.
		/// </summary>
		public static readonly TimeSpan FullBoxCloseDelay = TimeSpan.FromMinutes(5);

		private const string DonationTimeVar = "Melia.Pardoner.LastChurchDonation";
		private const string SelectionVar = "Melia.Pardoner.OblationSelection";

		// What a batch may hold, leaving room for the call that wraps it.
		private const int MaxBatchLength = ClientScript.ScriptMaxLength - 64;

		/// <summary>
		/// Returns how many items the given Pardoner's offering box holds,
		/// from the skill's first caption ratio.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <returns></returns>
		public static int GetOblationCapacity(Character pardoner)
		{
			if (!pardoner.TryGetSkill(SkillId.Pardoner_Oblation, out var skill))
				return 0;

			var capacity = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			return Math.Min(capacity, OblationStorage.MaxSize);
		}

		/// <summary>
		/// Returns the percentage of an item's shop value the given
		/// Pardoner's offering box pays for it.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <returns></returns>
		public static float GetOblationRate(Character pardoner)
		{
			if (!pardoner.TryGetSkill(SkillId.Pardoner_Oblation, out var skill))
				return 0;

			return skill.Properties.GetFloat(PropertyName.CaptionRatio2);
		}

		/// <summary>
		/// Returns the silver the given Pardoner pays for one unit of the
		/// item, or 0 if the item can't be offered.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static int GetOblationPrice(Character pardoner, Item item)
		{
			if (item.IsLocked || item.Id == ItemId.Silver || item.Data.SellPrice <= 0)
				return 0;

			var rate = GetOblationRate(pardoner) / 100f;
			if (rate <= 0)
				return 0;

			return Math.Max(1, (int)(item.Data.SellPrice * rate));
		}

		/// <summary>
		/// Returns the silver the church gives the given Pardoner for the
		/// item in their offering box with the given object id.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <param name="objectId"></param>
		/// <returns></returns>
		public static int GetChurchDonationPrice(Character pardoner, long objectId)
		{
			var pricePaid = pardoner.OblationBox.GetPricePaid(objectId);
			if (pricePaid <= 0)
				return 0;

			if (!pardoner.TryGetSkill(SkillId.Pardoner_Oblation, out var skill))
				return 0;

			var rate = skill.Properties.GetFloat(PropertyName.CaptionRatio3) / 100f;

			return Math.Max(pricePaid, (int)(pricePaid * rate));
		}

		/// <summary>
		/// Streams the given Pardoner's offering box to their client and
		/// opens the box window on it.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <param name="atChurch"></param>
		public static void SendOblationBox(Character pardoner, bool atChurch)
		{
			StreamOblationBox(pardoner, pardoner);

			var cooldown = (int)GetChurchDonationCooldown(pardoner).TotalSeconds;

			Send.ZC_EXEC_CLIENT_SCP(pardoner.Connection, $"M_OPEN_OBLATION_BOX({GetOblationCapacity(pardoner)}, {(atChurch ? 1 : 0)}, {cooldown})");
		}

		/// <summary>
		/// Streams the given Pardoner's offering box to their client and
		/// redraws the box window if they have it open.
		/// </summary>
		/// <param name="pardoner"></param>
		public static void RefreshOblationBox(Character pardoner)
		{
			StreamOblationBox(pardoner, pardoner);

			Send.ZC_EXEC_CLIENT_SCP(pardoner.Connection, $"M_REFRESH_OBLATION_BOX({GetOblationCapacity(pardoner)})");

			NotifyOblationBoxViewers(pardoner);
		}

		/// <summary>
		/// Sends the given Pardoner's offering box to everyone who has it
		/// open, so a box that changed under them doesn't stay on screen.
		/// </summary>
		/// <param name="pardoner"></param>
		private static void NotifyOblationBoxViewers(Character pardoner)
		{
			var viewers = pardoner.Map.GetCharacters(a => a != pardoner && a.Connection != null && a.Connection.ActiveShopOwnerHandle == pardoner.Handle);

			foreach (var viewer in viewers)
				SendOblationShop(viewer, pardoner);
		}

		/// <summary>
		/// Streams the given Pardoner's offering box to a donor looking at
		/// it, so their window can show what it already holds and how much
		/// room is left.
		/// </summary>
		/// <param name="donor"></param>
		/// <param name="pardoner"></param>
		public static void SendOblationShop(Character donor, Character pardoner)
		{
			StreamOblationBox(pardoner, donor);

			var rate = (int)GetOblationRate(pardoner);

			Send.ZC_EXEC_CLIENT_SCP(donor.Connection, $"M_SET_OBLATION_SHOP({GetOblationCapacity(pardoner)}, {rate})");
		}

		/// <summary>
		/// Sends the contents of the given Pardoner's offering box to the
		/// given viewer.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <param name="viewer"></param>
		private static void StreamOblationBox(Character pardoner, Character viewer)
		{
			var conn = viewer.Connection;
			var box = pardoner.OblationBox;

			ItemPreview.Show(viewer, box.GetItems().Values);

			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.BeginRecv('OblationBox')");

			var sb = new StringBuilder();
			foreach (var itemKv in box.GetItems())
			{
				var item = itemKv.Value;
				var pricePaid = box.GetPricePaid(item.ObjectId);
				var entry = string.Format("{{{0},{1},{2},{3},{4},'{5}'}},", itemKv.Key, item.Id, item.Amount, pricePaid, GetChurchDonationPrice(pardoner, item.ObjectId), item.ObjectId);

				// Flushed before the entry rather than after it, so a batch
				// can never be built past the length the client accepts.
				if (sb.Length > 0 && sb.Length + entry.Length > MaxBatchLength)
				{
					Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('OblationBox', {{ {sb} }})");
					sb.Clear();
				}

				sb.Append(entry);
			}

			if (sb.Length > 0)
				Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('OblationBox', {{ {sb} }})");

			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.ExecData('OblationBox', M_SET_OBLATION_BOX)");
			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.EndRecv('OblationBox')");
		}

		/// <summary>
		/// Closes the given Pardoner's offering box shop once the box has
		/// been full long enough to be doing nothing but taking up room.
		/// </summary>
		/// <remarks>
		/// A Pardoner who is there to empty it gets the grace period; one
		/// who is autotrading is not coming back to empty anything, so
		/// their shop closes as soon as the box fills.
		/// </remarks>
		/// <param name="pardoner"></param>
		public static void UpdateFullBoxTimer(Character pardoner)
		{
			var shop = pardoner.Connection?.ShopCreated;
			if (shop == null || shop.Type != PersonalShopType.Oblation || shop.IsClosed)
				return;

			var box = pardoner.OblationBox;
			var capacity = GetOblationCapacity(pardoner);

			if (capacity <= 0 || box.GetItemCount() < capacity)
			{
				box.FullSince = null;
				return;
			}

			if (pardoner.IsAutoTrading)
			{
				CloseOblationShop(pardoner, shop);
				return;
			}

			box.FullSince ??= DateTime.Now;

			if (DateTime.Now - box.FullSince.Value >= FullBoxCloseDelay)
				CloseOblationShop(pardoner, shop);
		}

		/// <summary>
		/// Closes the given Pardoner's offering box shop.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <param name="shop"></param>
		private static void CloseOblationShop(Character pardoner, ShopData shop)
		{
			shop.IsClosed = true;
			pardoner.OblationBox.FullSince = null;

			Send.ZC_AUTOSELLER_LIST(pardoner.Connection, pardoner);
			Send.ZC_AUTOSELLER_TITLE(pardoner);
			Send.ZC_NORMAL.ShopAnimation(pardoner, "Squire_Repair", 1, 0);

			pardoner.Connection.ShopCreated = null;

			pardoner.ServerMessage(Localization.Get("Your Offering Box is full. The shop has closed."));
		}

		/// <summary>
		/// Returns the box positions the given Pardoner has selected in
		/// the box window.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <returns></returns>
		public static List<int> GetSelection(Character pardoner)
		{
			var selection = pardoner.Variables.Temp.Get<List<int>>(SelectionVar);
			if (selection == null)
			{
				selection = new List<int>();
				pardoner.Variables.Temp.Set(SelectionVar, selection);
			}

			return selection;
		}

		/// <summary>
		/// Moves every selected item out of the given Pardoner's offering
		/// box and into their inventory, and clears the selection.
		/// </summary>
		/// <param name="pardoner"></param>
		public static void RetrieveSelection(Character pardoner)
		{
			var box = pardoner.OblationBox;
			var selection = GetSelection(pardoner);

			foreach (var position in selection)
			{
				var item = box.GetItemAtPosition(position);
				if (item == null)
					continue;

				box.TakeBack(item.ObjectId);
			}

			selection.Clear();
		}

		/// <summary>
		/// Gives every selected item in the given Pardoner's offering box
		/// to the church, pays them for it and starts the wait before the
		/// church takes another set.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <returns></returns>
		public static int DonateSelection(Character pardoner)
		{
			var box = pardoner.OblationBox;
			var selection = GetSelection(pardoner);
			var total = 0;

			foreach (var position in selection)
			{
				var item = box.GetItemAtPosition(position);
				if (item == null)
					continue;

				var price = GetChurchDonationPrice(pardoner, item.ObjectId) * item.Amount;

				if (box.Consume(item.ObjectId) != StorageResult.Success)
					continue;

				total += price;
			}

			selection.Clear();

			if (total > 0)
			{
				pardoner.AddItem(ItemId.Silver, total);
				StartChurchDonationCooldown(pardoner);
			}

			return total;
		}

		/// <summary>
		/// Returns how long the given Pardoner still has to wait before
		/// the church accepts offerings again, or zero if it does now.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <returns></returns>
		public static TimeSpan GetChurchDonationCooldown(Character pardoner)
		{
			var lastDonation = pardoner.Variables.Perm.GetLong(DonationTimeVar, -1);
			if (lastDonation == -1)
				return TimeSpan.Zero;

			var remaining = new DateTime(lastDonation) + ChurchDonationCooldown - DateTime.Now;

			return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
		}

		/// <summary>
		/// Starts the wait before the church accepts offerings from the
		/// given Pardoner again.
		/// </summary>
		/// <param name="pardoner"></param>
		public static void StartChurchDonationCooldown(Character pardoner)
			=> pardoner.Variables.Perm.SetLong(DonationTimeVar, DateTime.Now.Ticks);
	}
}
