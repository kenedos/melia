using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.Network;
using Melia.Shared.World;
using Yggdrasil.Logging;

namespace Melia.Zone.Network
{
	public partial class PacketHandler : PacketHandler<IZoneConnection>
	{
		/// <summary>
		/// Stub: Houses system was removed during Laima merge.
		/// All housing packet handlers return immediately.
		/// </summary>
		[PacketHandler(Op.CZ_HOUSING_REQUEST_POST_HOUSE_WARP)]
		public void CZ_HOUSING_REQUEST_POST_HOUSE_WARP(IZoneConnection conn, Packet packet)
		{
			var houseOwnerAccountId = packet.GetLong();
			conn.SelectedCharacter?.SystemMessage("CanNotVisitHousing");
		}

		[PacketHandler(Op.CZ_PERSONAL_HOUSING_REQUEST_LEAVE)]
		public void CZ_PERSONAL_HOUSING_REQUEST_LEAVE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_OPEN_EDIT_MODE)]
		public void CZ_HOUSING_OPEN_EDIT_MODE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_GRID_ARRANGED_FURNITURE)]
		public void CZ_HOUSING_REQUEST_GRID_ARRANGED_FURNITURE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_ARRANGED_FURNITURE)]
		public void CZ_HOUSING_REQUEST_ARRANGED_FURNITURE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_CLOSE_EDIT_MODE)]
		public void CZ_HOUSING_CLOSE_EDIT_MODE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_PERSONAL_HOUSING_REQUEST_GROUP_LIST)]
		public void CZ_PERSONAL_HOUSING_REQUEST_GROUP_LIST(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			Send.ZC_PERSONAL_HOUSING_ANSWER_GROUP_LIST(character);
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_GUILD_AGIT_INFO)]
		public void CZ_HOUSING_REQUEST_GUILD_AGIT_INFO(IZoneConnection conn, Packet packet)
		{
			var guildId = packet.GetLong();
			var addonMessage = packet.GetString(128);
			var guildMapId = 6000;
			var guildLevel = 1;

			Send.ZC_HOUSING_ANSWER_GUILD_AGIT_INFO(conn, guildId, guildMapId, guildLevel);
			Send.ZC_ADDON_MSG(conn.SelectedCharacter, AddonMessage.RECEIVE_OTHER_GUILD_AGIT_INFO, 0, guildId.ToString());
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_PREVIEW)]
		public void CZ_HOUSING_REQUEST_PREVIEW(IZoneConnection conn, Packet packet)
		{
			var guildId = packet.GetLong();
			var l1 = packet.GetLong();
			var guildMapId = 6000;

			Send.ZC_HOUSING_ANSWER_PREVIEW(conn, guildId, guildMapId);
		}

		[PacketHandler(Op.CZ_HOUSING_CANCEL_ARRANGEMENT_FURNITURE)]
		public void CZ_HOUSING_CANCEL_ARRANGEMENT_FURNITURE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_MOVE_FURNITURE)]
		public void CZ_HOUSING_REQUEST_MOVE_FURNITURE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_REMOVE_FURNITURE_ALOT)]
		public void CZ_HOUSING_REQUEST_REMOVE_FURNITURE_ALOT(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_REMOVE_ALL_FURNITURE)]
		public void CZ_HOUSING_REQUEST_REMOVE_ALL_FURNITURE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_REQUEST_ENABLE_MOVE_FURNITURE)]
		public void CZ_HOUSING_REQUEST_ENABLE_MOVE_FURNITURE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_PAGE_SAVE)]
		public void CZ_HOUSING_PAGE_SAVE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}

		[PacketHandler(Op.CZ_HOUSING_PAGE_LOAD)]
		public void CZ_HOUSING_PAGE_LOAD(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available
		}
	}
}
