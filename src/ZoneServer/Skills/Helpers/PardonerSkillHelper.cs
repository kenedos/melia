using System;
using System.Collections.Generic;
using System.Text;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Storages;
using Yggdrasil.Logging;

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
		public static readonly TimeSpan ChurchDonationCooldown = TimeSpan.FromHours(24);

		private const string DonationTimeVar = "Melia.Pardoner.LastChurchDonation";
		private const string SelectionVar = "Melia.Pardoner.OblationSelection";

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
			StreamOblationBox(pardoner, pardoner.Connection);

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
			StreamOblationBox(pardoner, pardoner.Connection);

			Send.ZC_EXEC_CLIENT_SCP(pardoner.Connection, $"M_REFRESH_OBLATION_BOX({GetOblationCapacity(pardoner)})");
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
			StreamOblationBox(pardoner, donor.Connection);

			var rate = (int)GetOblationRate(pardoner);

			Send.ZC_EXEC_CLIENT_SCP(donor.Connection, $"M_SET_OBLATION_SHOP({GetOblationCapacity(pardoner)}, {rate})");
		}

		/// <summary>
		/// Sends the contents of the given Pardoner's offering box to the
		/// given connection.
		/// </summary>
		/// <param name="pardoner"></param>
		/// <param name="conn"></param>
		private static void StreamOblationBox(Character pardoner, IZoneConnection conn)
		{
			var box = pardoner.OblationBox;

			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.BeginRecv('OblationBox')");

			var sb = new StringBuilder();
			foreach (var itemKv in box.GetItems())
			{
				var position = itemKv.Key;
				var item = itemKv.Value;
				var props = SerializeItemProperties(item);
				var pricePaid = box.GetPricePaid(item.ObjectId);

				sb.AppendFormat("{{{0},{1},{2},{3},{4},{5}}},", position, item.Id, item.Amount, pricePaid, GetChurchDonationPrice(pardoner, item.ObjectId), props);

				if (sb.Length > ClientScript.ScriptMaxLength * 0.8)
				{
					Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('OblationBox', {{ {sb} }})");
					sb.Clear();
				}
			}

			if (sb.Length > 0)
				Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('OblationBox', {{ {sb} }})");

			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.ExecData('OblationBox', M_SET_OBLATION_BOX)");
			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.EndRecv('OblationBox')");
		}

		/// <summary>
		/// Returns the item's properties as a lua table, so the box window
		/// can show the same tooltip the inventory would.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private static string SerializeItemProperties(Item item)
		{
			try
			{
				return item.SerializePropertiesToLua();
			}
			catch (Exception ex)
			{
				Log.Warning("PardonerSkillHelper.SerializeItemProperties: Failed to serialize item '{0}'. {1}", item.Id, ex.Message);
				return "nil";
			}
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
