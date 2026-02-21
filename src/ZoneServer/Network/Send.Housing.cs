using System.Collections.Generic;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
// using Melia.Zone.World.Houses; // Removed: Houses namespace deleted
using Melia.Zone.World.Items;

namespace Melia.Zone.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Show Grid for Personal House Furniture Placement
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_HOUSING_READY_GRID(Character character)
		{
			var packet = new Packet(Op.ZC_HOUSING_READY_GRID);

			packet.PutInt(0);
			packet.PutInt(0);

			character.Connection.Send(packet);
		}


		/// <summary>
		/// Show Furniture for Personal House Furniture Placement
		/// </summary>
		/// <param name="character"></param>
		/// <param name="furnitureId"></param>
		public static void ZC_HOUSING_START_ARRANGEMENT_FURNITURE(Character character, int furnitureId)
		{
			var packet = new Packet(Op.ZC_HOUSING_START_ARRANGEMENT_FURNITURE);

			packet.PutInt(furnitureId);

			character.Connection.Send(packet);
		}

		// Removed: ZC_HOUSING_READY_ARRANGED_FURNITURE referenced PersonalHouse and Prop
		// types from deleted Houses namespace.

		/// <summary>
		/// Remove furniture
		/// </summary>
		/// <param name="furniture"></param>
		public static void ZC_HOUSING_ANSWER_REMOVE_FURNITURE(IMonster furniture, int furnitureId)
		{
			var packet = new Packet(Op.ZC_HOUSING_ANSWER_REMOVE_FURNITURE);

			packet.PutInt(furnitureId);
			packet.PutInt(furniture.Handle);

			furniture.Map.Broadcast(packet, furniture);
		}

		/// <summary>
		/// Housing group list - Count of props by group
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_PERSONAL_HOUSING_ANSWER_GROUP_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_PERSONAL_HOUSING_ANSWER_GROUP_LIST);

			var count = 3;
			packet.PutInt(count); // Count
			for (var i = 0; i < count; i++)
			{
				switch (i)
				{
					case 0:
						packet.PutInt(1004); // Furniture Group Id
						packet.PutInt(1); // Furniture Group Count
						break;
					case 1:
						packet.PutInt(2003); // Furniture Group Id
						packet.PutInt(1); // Furniture Group Count
						break;
					case 2:
						packet.PutInt(3001); // Furniture Group Id
						packet.PutInt(1); // Furniture Group Count
						break;
				}
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Enable movement of a furniture
		/// </summary>
		/// <param name="character"></param>
		/// <param name="furnitureId"></param>
		/// <param name="furnitureHandle"></param>
		public static void ZC_HOUSING_ANSWER_ENABLE_MOVE_FURNITURE(Character character, int furnitureId, int furnitureHandle)
		{
			var packet = new Packet(Op.ZC_HOUSING_ANSWER_ENABLE_MOVE_FURNITURE);

			packet.PutInt(furnitureId);
			packet.PutInt(furnitureHandle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Create furniture
		/// </summary>
		/// <param name="character"></param>
		/// <param name="furniture"></param>
		public static void ZC_HOUSING_CREATE_BG_FURNITURE(Character character, IMonster furniture)
		{
			var packet = new Packet(Op.ZC_HOUSING_CREATE_BG_FURNITURE);

			packet.PutInt(furniture.Handle);
			packet.PutInt(furniture.Id);
			packet.PutPosition(furniture.Position);
			packet.PutByte((byte)furniture.Direction.ToCardinalDirection());

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Create furniture and broadcast it
		/// </summary>
		/// <param name="furniture"></param>
		public static void ZC_HOUSING_CREATE_BG_FURNITURE(IMonster furniture)
		{
			var packet = new Packet(Op.ZC_HOUSING_CREATE_BG_FURNITURE);

			packet.PutInt(furniture.Handle);
			packet.PutInt(furniture.Id);
			packet.PutPosition(furniture.Position);
			packet.PutByte((byte)furniture.Direction.ToCardinalDirection());

			furniture.Map.Broadcast(packet, furniture);
		}

		/// <summary>
		/// Move furniture
		/// </summary>
		/// <param name="furniture"></param>
		public static void ZC_HOUSING_MOVE_BG_FURNITURE(IMonster furniture)
		{
			var packet = new Packet(Op.ZC_HOUSING_MOVE_BG_FURNITURE);

			packet.PutInt(furniture.Handle);
			packet.PutPosition(furniture.Position);
			packet.PutByte((byte)furniture.Direction.ToCardinalDirection());

			furniture.Map.Broadcast(packet, furniture);
		}

		/// <summary>
		/// Remove background furniture
		/// </summary>
		/// <param name="furniture"></param>
		public static void ZC_HOUSING_REMOVE_BG_FURNITURE(IMonster furniture)
		{
			var packet = new Packet(Op.ZC_HOUSING_REMOVE_BG_FURNITURE);

			packet.PutInt(furniture.Handle);

			furniture.Map.Broadcast(packet, furniture);
		}

		/// <summary>
		/// Response to CZ_HOUSING_GUILD_AGIT_INFO
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="guildId"></param>
		/// <param name="guildMapId"></param>
		/// <param name="guildLevel"></param>
		public static void ZC_HOUSING_ANSWER_GUILD_AGIT_INFO(IZoneConnection conn, long guildId, int guildMapId, int guildLevel)
		{
			var packet = new Packet(Op.ZC_HOUSING_ANSWER_GUILD_AGIT_INFO);

			packet.PutLong(guildId);
			packet.PutInt(guildMapId);
			packet.PutInt(guildLevel);
			packet.PutEmptyBin(128);

			conn.Send(packet);
		}

		/// <summary>
		/// Response to CZ_HOUSING_REQUEST_PREVIEW
		/// </summary>
		/// <remarks>Official Server splits it upto 128 count per packet</remarks>
		/// <param name="conn"></param>
		/// <param name="guildId"></param>
		/// <param name="guildMapId"></param>
		public static void ZC_HOUSING_ANSWER_PREVIEW(IZoneConnection conn, long guildId, int guildMapId)
		{
			var packet = new Packet(Op.ZC_HOUSING_ANSWER_GUILD_AGIT_INFO);
			var count = 0;

			packet.PutLong(guildId);
			packet.PutInt(guildMapId);
			packet.PutInt(count);
			if (count > 0)
			{
				for (var i = 0; i < count; i++)
				{
					packet.PutInt(i);
					packet.PutPosition(Position.Zero);
					packet.PutByte(0);
				}
			}

			conn.Send(packet);
		}
	}
}
