using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Network.Helpers
{
	/// <summary>
	/// Helper methods for adding movement information to packets.
	/// </summary>
	public static class MovementHelper
	{
		/// <summary>
		/// Adds information about the actor moving between the given cell
		/// positions to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="actor"></param>
		/// <param name="fromCellPos"></param>
		/// <param name="toCellPos"></param>
		/// <param name="speed"></param>
		public static void AddCellMovement(this Packet packet, IActor actor, Position fromCellPos, Position toCellPos, float speed)
			=> AddCellMovement(packet, actor, fromCellPos, toCellPos, speed, CellMoveType.Normal);

		/// <summary>
		/// Adds information about the actor moving between the given cell
		/// positions to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="actor"></param>
		/// <param name="fromCellPos"></param>
		/// <param name="toCellPos"></param>
		/// <param name="speed"></param>
		/// <param name="type"></param>
		public static void AddCellMovement(this Packet packet, IActor actor, Position fromCellPos, Position toCellPos, float speed, CellMoveType type)
		{
			packet.PutInt(actor.Handle);
			packet.PutInt((int)fromCellPos.X);
			packet.PutInt((int)fromCellPos.Z);
			packet.PutInt((int)fromCellPos.Y);
			packet.PutInt((int)toCellPos.X);
			packet.PutInt((int)toCellPos.Z);
			packet.PutInt((int)toCellPos.Y);
			packet.PutFloat(speed);

			// [i354444] Float removed, byte added. Same thing?
			//packet.PutFloat(0);
			if (Versions.Client < 354444)
				packet.PutFloat(0); 
			else
				packet.PutByte((byte)type);
		}

		/// <summary>
		/// Adds information about the actor moving between the given cell
		/// positions to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="actor"></param>
		/// <param name="fromPos"></param>
		/// <param name="toPos"></param>
		/// <param name="speed"></param>
		public static void AddMovement(this Packet packet, IActor actor, Position fromPos, Position toPos, float speed, float time, int i1 = 0)
		{
			packet.PutInt(actor.Handle);
			packet.PutFloat(fromPos.X);
			packet.PutFloat(0);
			packet.PutFloat(fromPos.Z);
			packet.PutFloat(toPos.X);
			packet.PutFloat(0);
			packet.PutFloat(toPos.Z);
			packet.PutFloat(speed);
			packet.PutFloat(time);
			packet.PutInt(i1);
		}
	}
}
