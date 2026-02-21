using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.Network
{
	public partial class PacketHandler : PacketHandler<IZoneConnection>
	{
		/// <summary>
		/// Accepting a party invite
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PARTY_INVITE_ACCEPT)]
		public void CZ_PARTY_INVITE_ACCEPT(IZoneConnection conn, Packet packet)
		{
			var groupType = (GroupType)packet.GetByte();
			var teamName = packet.GetString();
			var character = conn.SelectedCharacter;
			var sender = ZoneServer.Instance.World.GetCharacterByTeamName(teamName);

			if (sender == null)
				return;
			switch (groupType)
			{
				case GroupType.Party:
					var party = sender.Connection.Party;
					party ??= ZoneServer.Instance.World.Parties.Create(sender);
					party.AddMember(character);
					break;
				// Removed: Guild type deleted during Laima merge
				// case GroupType.Guild:
				// 	var guild = sender.Connection.Guild;
				// 	guild?.AddMember(character);
				// 	break;
			}
		}

		/// <summary>
		/// Rejecting a party invite
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PARTY_INVITE_CANCEL)]
		public void CZ_PARTY_INVITE_CANCEL(IZoneConnection conn, Packet packet)
		{
			var groupType = (GroupType)packet.GetByte();
			var teamName = packet.GetString();

			var character = conn.SelectedCharacter;

			var partyInviter = ZoneServer.Instance.World.GetCharacterByTeamName(teamName);

			if (partyInviter != null)
			{
				Send.ZC_ADDON_MSG(partyInviter, AddonMessage.PARTY_INVITE_CANCEL, (int)groupType, character.TeamName);
			}
		}

		/// <summary>
		/// Leaving a party
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PARTY_OUT)]
		public void CZ_PARTY_OUT(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			var party = character.Connection.Party;

			if (party != null)
			{
				// RemoveMember will block if party is in auto-match queue
				if (!party.RemoveMember(character))
					return;

				// Only delete if party is empty and still exists (it may have been
				// deleted by dungeon cleanup via the PlayerLeftParty event)
				if (party.MemberCount == 0 && ZoneServer.Instance.World.Parties.Exists(party.ObjectId))
					ZoneServer.Instance.World.Parties.Delete(party);
			}
		}

		/// <summary>
		/// Changing Party Settings
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PARTY_PROP_CHANGE)]
		public void CZ_PARTY_PROP_CHANGE(IZoneConnection conn, Packet packet)
		{
			var groupType = (GroupType)packet.GetByte();
			var type = packet.GetInt();
			var b2 = packet.GetByte();
			var b3 = packet.GetByte();
			var s1 = packet.GetShort();
			var value = packet.GetString();

			var character = conn.SelectedCharacter;

			switch (groupType)
			{
				case GroupType.Party:
					var party = character.Connection.Party;

					if (party != null && party.LeaderDbId == character.DbId)
					{
						party.UpdateSetting(type, value);
					}
					break;
				// Removed: Guild type deleted during Laima merge
				// case GroupType.Guild: { ... }
				//	break;
			}
		}

		// TODO:
		//CZ_PARTY_JOIN_BY_LINK
		//CZ_PARTY_INVENTORY_LOAD
		//CZ_PARTY_SHARED_QUEST
		//CZ_PARTY_MEMBER_SKILL_USE
		//CZ_PARTY_MEMBER_SKILL_ACCEPT
	}
}
