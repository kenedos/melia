using System.Text;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Network.Helpers
{
	public static class ItemHelper
	{
		/// <summary>
		/// Adds information about the monster to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="item"></param>
		public static void AddSocket(this Packet packet, short index, Item item)
		{
			//var propertyList = item?.Properties.GetAll();
			//var propertiesSize = propertyList?.GetByteCount();
			var itemExp = item?.Properties[PropertyName.ItemExp] ?? 0;

			packet.PutShort(index);
			packet.PutLong(item?.ObjectId ?? 0);
			packet.PutInt((int)itemExp);
			packet.PutInt(0);
			packet.PutInt(item?.Id ?? 0);
			packet.PutInt(0);
			packet.PutShort(item != null ? 4 : 1);
			packet.PutShort(0);
			packet.PutInt(4);

			var i = 4;
			for (var j = 0; j < i; j++)
			{
				packet.PutLpString("None");
				packet.PutLpString("None");
				packet.PutInt(0);
			}

			//packet.AddProperties(propertyList);
		}
	}
}
