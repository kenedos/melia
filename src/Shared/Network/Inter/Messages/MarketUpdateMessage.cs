using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Market update types.
	/// </summary>
	public enum MarketUpdateType
	{
		/// <summary>
		/// A new item was listed on the market.
		/// </summary>
		ItemListed,

		/// <summary>
		/// An item was purchased from the market.
		/// </summary>
		ItemPurchased,

		/// <summary>
		/// A market listing was cancelled.
		/// </summary>
		ItemCancelled,

		/// <summary>
		/// A general update that requires reloading all market data.
		/// </summary>
		Reload,
	}

	/// <summary>
	/// Contains information about a market update event.
	/// </summary>
	[Serializable]
	public class MarketUpdateMessage : ICommMessage
	{
		/// <summary>
		/// Returns the type of update.
		/// </summary>
		public MarketUpdateType UpdateType { get; set; }

		/// <summary>
		/// Returns the market item's unique id.
		/// </summary>
		public long MarketItemId { get; set; }

		/// <summary>
		/// Creates a new market update message.
		/// </summary>
		/// <param name="updateType">The type of update.</param>
		/// <param name="marketItemId">The market item's unique id (optional).</param>
		public MarketUpdateMessage(MarketUpdateType updateType, long marketItemId = 0)
		{
			this.UpdateType = updateType;
			this.MarketItemId = marketItemId;
		}
	}
}
