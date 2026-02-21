using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Network.Helpers
{
	public static class CompanionHelper
	{
		/// <summary>
		/// Writes companion data to the packet in the PetInfo format.
		/// Handles both spawned companions (with handle) and unspawned companions (in barracks).
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="companion"></param>
		public static void AddCompanion(this Packet packet, Companion companion)
		{
			if (companion == null)
				return;

			var properties = companion.Properties.GetAll();
			var propertiesSize = properties.GetByteCount();

			packet.PutInt(companion.Id);
			packet.PutLong(companion.ObjectId);
			packet.PutLong(companion.Owner?.ObjectId ?? 0);
			packet.PutLong(companion.TotalExp);
			packet.PutLpString(companion.Name);
			packet.PutByte(0);
			packet.PutPosition(companion.Position);
			packet.PutDirection(companion.Direction);
			packet.PutInt(companion.Handle);
			packet.PutShort(propertiesSize);
			packet.AddProperties(properties);

			// Conditional section based on whether companion is spawned
			if (companion.Handle == 0)
			{
				// Companion is in barracks (not spawned on current character)
				packet.PutShort(1);
				packet.PutLong(companion.Owner?.ObjectId ?? 0);
				packet.PutShort(0); // String property count
			}
			else
			{
				// Companion is spawned
				packet.PutShort(0);
			}

			// Common footer
			packet.PutByte(1);
			packet.PutByte(1);
			packet.PutByte(0); // bin[0]
			packet.PutByte(0); // bin[1]
			packet.PutByte(0); // bin[2]
			packet.PutByte(0); // b4
			packet.PutInt(1);
			packet.PutByte(0);
			packet.PutByte(1);
			packet.PutByte(1);
			packet.PutInt(0);
			packet.PutLong((long)companion.Properties.GetFloat(PropertyName.Stamina));
		}
	}
}
