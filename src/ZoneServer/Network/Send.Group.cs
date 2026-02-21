using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.World;
using Melia.Zone.Network.Helpers;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Groups;
// using Melia.Zone.World.Houses; // Removed: Houses namespace deleted
using Melia.Zone.World.Items;

namespace Melia.Zone.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Party Info usually sent when party is created
		/// </summary>
		/// <param name="character"></param>
		/// <param name="group"></param>
		public static void ZC_PARTY_INFO(Character character, IGroup group)
		{
			var propertyList = group.Properties.GetAll();
			var propertiesSize = propertyList.GetByteCount();

			var packet = new Packet(Op.ZC_PARTY_INFO);
			packet.PutByte((byte)group.Type);
			packet.PutByte(0);
			packet.PutDate(group.DateCreated);
			packet.PutLong(group.ObjectId);
			packet.PutLpString(group.Name);
			// TODO: Troubleshoot crash.
			packet.PutLong(group.Owner?.AccountObjectId ?? 0);
			packet.PutLpString(group.Owner?.TeamName ?? "");
			packet.PutInt(0);
			packet.PutInt(1);
			packet.PutShort(1);
			if (group.Type == GroupType.Party)
			{
				packet.PutShort(256); // Probably two bytes 0 and 1
				packet.PutShort(0);
				packet.PutShort(propertiesSize);
				packet.AddProperties(propertyList);
			}
			else
			{
				packet.PutByte(0);
				packet.PutLpString(group.Note);
				packet.PutLong(0);
				packet.PutByte(0);
				packet.PutLong(0);
				packet.PutEmptyBin(20004);
				packet.PutInt(0);
				packet.PutShort(2000);
				packet.PutShort(1);
				packet.PutShort(1);
				packet.PutShort(1);
				packet.PutShort(1);
				packet.PutShort(1);
				packet.PutEmptyBin(68);
				packet.PutFloat(0);
				packet.PutShort(0);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// List of party members also sent when party is created and members join
		/// </summary>
		/// <param name="group"></param>
		public static void ZC_PARTY_LIST(IGroup group)
		{
			var members = group.GetMembers();

			var packet = new Packet(Op.ZC_PARTY_LIST);
			packet.PutLong(0);
			packet.PutByte((byte)group.Type);
			packet.PutLong(group.ObjectId);
			packet.PutByte((byte)members.Count);
			foreach (var member in members)
				packet.AddMember(member);

			group.Broadcast(packet);
		}

		/// <summary>
		/// When a new character joins the party
		/// </summary>
		/// <param name="character"></param>
		/// <param name="group"></param>
		public static void ZC_PARTY_ENTER(Character character, IGroup group)
		{
			var packet = new Packet(Op.ZC_PARTY_ENTER);

			packet.PutByte((byte)group.Type);
			packet.PutLong(group.ObjectId);
			packet.AddMember(group.ToMember(character));
			packet.PutShort(0);

			group.Broadcast(packet);
		}

		/// <summary>
		/// Party member left/expelled from party
		/// </summary>
		/// <param name="group"></param>
		public static void ZC_PARTY_OUT(Character character, IGroup group)
		{
			var packet = new Packet(Op.ZC_PARTY_OUT);

			packet.PutByte((byte)group.Type);
			packet.PutLong(group.ObjectId);
			packet.PutLong(character.AccountDbId);
			packet.PutByte(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Party member left/expelled from party
		/// with broadcast.
		/// </summary>
		/// <param name="group"></param>
		public static void ZC_PARTY_OUT(IGroup group, IMember member)
		{
			var packet = new Packet(Op.ZC_PARTY_OUT);

			packet.PutByte((byte)group.Type);
			packet.PutLong(group.ObjectId);
			packet.PutLong(member.AccountObjectId);
			packet.PutByte(0);

			group.Broadcast(packet);
		}

		/// <summary>
		/// Party info updates
		/// </summary>
		/// <param name="group"></param>
		public static void ZC_PARTY_INST_INFO(IGroup group)
		{
			var members = group.GetMembers();

			var packet = new Packet(Op.ZC_PARTY_INST_INFO);

			packet.PutByte((byte)group.Type);
			packet.PutInt(members.Count);
			foreach (var member in members)
				packet.AddPartyInstantMemberInfo(member);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutByte(0);

			group.Broadcast(packet);
		}
	}
}
