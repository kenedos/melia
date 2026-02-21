using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Const.Web;
using Melia.Shared.Network;
using Melia.Shared.Util;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Network
{
	public partial class Send
	{
		public static partial class ZC_NORMAL
		{
			/// <summary>
			/// Send list of items that are retrievable from the Market
			/// </summary>
			/// <param name="character"></param>
			public static void MarketRetrievalItems(Character character, List<MarketItem> items)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MarketRetrievalItems);
				packet.Zlib(true, zpacket =>
				{
					// TODO: Market Items
					zpacket.PutInt(items.Count);
					foreach (var item in items)
					{
						var itemId = item.GetItemId(character.DbId);
						var itemCount = itemId == ItemId.Silver ? (int)item.SellPrice : item.Count;
						zpacket.PutLong(item.ItemGuid);
						zpacket.PutInt(itemId);
						zpacket.PutInt(0);
						zpacket.PutInt(itemCount);
						zpacket.PutEmptyBin(33);
						zpacket.PutLong(item.RegTime.ToUnixTimeSeconds() * 1000);

						zpacket.PutLpString(item.GetMarketStatus(character.DbId));
						zpacket.PutShort(0); // item properties size
											 // Add properties here
						zpacket.PutInt(0);
					}
				});

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Register an item on the market
			/// </summary>
			/// <param name="character"></param>
			/// <param name="itemWorldId"></param>
			public static void MarketRetrieveItem(Character character, long itemWorldId)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MarketRegisterItem);
				packet.PutLong(itemWorldId);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Register an item on the market
			/// </summary>
			/// <param name="character"></param>
			/// <param name="marketWorldId"></param>
			public static void MarketBuyItem(Character character, long marketWorldId, int itemRemainingAmount, short s1 = 1)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MarketBuyItem);
				packet.PutLong(marketWorldId);
				packet.PutInt(itemRemainingAmount);
				packet.PutShort(s1);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Cancel an item market registration
			/// </summary>
			/// <param name="character"></param>
			/// <param name="marketWorldId"></param>
			public static void MarketCancelItem(Character character, long marketWorldId)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MarketCancelItem);
				packet.PutLpString(marketWorldId.ToString());
				packet.PutInt(0);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Market Item Min/Max Prices
			/// </summary>
			/// <param name="character"></param>
			/// <param name="isTradable">Boolean flag (1 = Success/Tradable)</param>
			/// <param name="avgPrice">The Base/Average price calculated by the server</param>
			/// <param name="minPrice">Minimum allowed price (50% of Avg)</param>
			/// <param name="softCapPrice">Price warning threshold (400% of Avg)</param>
			/// <param name="maxPrice">Maximum allowed price (2000% of Avg)</param>
			/// <param name="unitPrice">The default or current unit price</param>
			public static void MarketMinMaxInfo(Character character, bool isTradable, long avgPrice, long minPrice, long softCapPrice, long maxPrice, long unitPrice)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MarketMinMaxInfo);

				packet.PutByte(isTradable);
				packet.PutLong(avgPrice);           // l1 (22,492,000)
				packet.PutLong(minPrice);           // minPrice (11,246,000)
				packet.PutLong(softCapPrice);       // l2 (89,968,000)
				packet.PutLong(maxPrice);           // maxPrice (449,840,000)
				packet.PutLong(unitPrice);          // unitPrice / totalPrice

				character.Connection.Send(packet);
			}
		}
	}
}
