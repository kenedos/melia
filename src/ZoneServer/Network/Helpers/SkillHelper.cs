using Melia.Zone.Skills;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;

namespace Melia.Zone.Network.Helpers
{
	public static class SkillHelper
	{
		/// <summary>
		/// Adds skill data to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="skill"></param>
		/// <param name="withObjectId"></param>
		public static void AddSkill(this Packet packet, Skill skill, bool withObjectId = true)
		{
			var propertyList = skill.Properties.GetAll();
			var propertiesSize = propertyList.GetByteCount();

			packet.PutLong(withObjectId ? skill.ObjectId : 0);
			packet.PutInt((int)skill.Id);
			packet.PutShort(propertiesSize);
			packet.PutEmptyBin(2); // alignment
			packet.PutInt(0);
			packet.PutShort(0);
			packet.PutEmptyBin(2); // alignment
			packet.AddProperties(propertyList);
		}
	}
}
