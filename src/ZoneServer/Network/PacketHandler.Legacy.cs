using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Network;

namespace Melia.Zone.Network
{
	public partial class PacketHandler : PacketHandler<IZoneConnection>
	{
		// Legacy Packets
		[PacketHandler(Op.CZ_CHECK_PING)]
		public void CZ_CHECK_PING(IZoneConnection conn, Packet packet)
		{

		}

		[PacketHandler(Op.CZ_WIKI_RECIPE_UPDATE)]
		public void CZ_WIKI_RECIPE_UPDATE(IZoneConnection conn, Packet packet)
		{

		}

		[PacketHandler(Op.CZ_I_NEED_PARTY)]
		public void CZ_I_NEED_PARTY(IZoneConnection conn, Packet packet)
		{
			Send.ZC_NEAR_PARTY_LIST(conn);
		}

		[PacketHandler(Op.CZ_REQUEST_SOME_PARTY)]
		public void CZ_REQUEST_SOME_PARTY(IZoneConnection conn, Packet packet)
		{

		}

		/// <summary>
		/// Sent after attacking.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_CANCEL)]
		public void CZ_SKILL_CANCEL(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			Send.ZC_SKILL_CAST_CANCEL(character);
		}

		[PacketHandler(Op.CZ_REQ_WIKI_CATEGORY_RANK_PAGE_INFO)]
		public void CZ_REQ_WIKI_CATEGORY_RANK_PAGE_INFO(IZoneConnection conn, Packet packet)
		{

		}

		/// <summary>
		/// Sent after completing the castbar of a dynamic skill cast.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_COMPLETE_PRELOAD)]
		public void CZ_COMPLETE_PRELOAD(IZoneConnection conn, Packet packet)
		{

		}
	}
}
