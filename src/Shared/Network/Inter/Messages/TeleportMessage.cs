using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Message to teleport a player to a specific location.
	/// </summary>
	[Serializable]
	public class TeleportMessage : ICommMessage
	{
		/// <summary>
		/// Returns the target player's team name.
		/// </summary>
		public string TargetTeamName { get; }

		/// <summary>
		/// Returns the destination map class name.
		/// </summary>
		public string MapName { get; }

		/// <summary>
		/// Returns the X coordinate.
		/// </summary>
		public float X { get; }

		/// <summary>
		/// Returns the Y coordinate.
		/// </summary>
		public float Y { get; }

		/// <summary>
		/// Returns the Z coordinate.
		/// </summary>
		public float Z { get; }

		/// <summary>
		/// Creates a new teleport message.
		/// </summary>
		/// <param name="targetTeamName">The target player's team name.</param>
		/// <param name="mapName">The destination map class name.</param>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <param name="z">The Z coordinate.</param>
		public TeleportMessage(string targetTeamName, string mapName, float x, float y, float z)
		{
			this.TargetTeamName = targetTeamName;
			this.MapName = mapName;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
	}
}
