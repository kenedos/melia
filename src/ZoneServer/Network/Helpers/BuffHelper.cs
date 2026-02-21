using System.Text;
using Melia.Shared.Network;
using Melia.Shared.Versioning;
using Melia.Zone.Buffs;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Network.Helpers
{
	public static class BuffHelper
	{
		/// <summary>
		/// Adds buff data to packet.
		/// </summary>
		/// <remarks>
		/// This helper is used in ZC_BUFF_ADD and ZC_BUFF_UPDATE.
		/// </remarks>
		/// <param name="packet"></param>
		/// <param name="buff"></param>
		public static void AddTargetedBuff(this Packet packet, Buff buff)
		{
			packet.PutInt(buff.Target.Handle);
			if (Versions.Protocol > 500)
				packet.PutInt(buff.Handle);
			packet.AddBuff(buff);
		}

		/// <summary>
		/// Adds buff data to packet.
		/// </summary>
		/// <remark>
		/// This helper is used in ZC_BUFF_LIST.
		/// </remarks>
		/// <param name="packet"></param>
		/// <param name="buff"></param>
		public static void AddBuff(this Packet packet, Buff buff)
		{
			packet.PutInt((int)buff.Id);
			packet.PutInt((int)buff.NumArg1);
			packet.PutInt((int)buff.NumArg2);
			packet.PutInt((int)buff.NumArg3); // NumArg3?
			packet.PutInt((int)buff.NumArg4); // NumArg4?
			packet.PutInt((int)buff.NumArg5); // NumArg5?
			packet.PutInt(buff.OverbuffCounter);
			packet.PutInt((int)buff.RemainingDuration.TotalMilliseconds);

			if (Versions.Protocol > 500)
			{
				packet.PutInt(0);
				packet.PutInt(0);

				// Instead of just the length of the string + null terminator
				// byte (LpString), the short integer in this packet is always
				// 4 higher for unknown reasons.
				var bytes = Encoding.UTF8.GetBytes(buff.Caster.Name + '\0');
				packet.PutShort(bytes.Length + 4);
				packet.PutBin(bytes);

				packet.PutInt(buff.Caster.Handle);
				packet.PutInt(buff.Handle);
				if (buff.Target is Character character && character.HasParty)
					packet.PutLong(character.Connection?.Party.ObjectId ?? 0);
				else
					packet.PutLong(0);
				packet.PutByte(1);
				packet.PutByte(0); // if 1, DashRun is sent again while running, which seems wrong
								   // if 1, It cancels the slithering animation early instead of full duration
			}
			else
			{
				packet.PutLpString(buff.Caster.Name);
				packet.PutInt(buff.Handle);
			}
		}
	}
}
