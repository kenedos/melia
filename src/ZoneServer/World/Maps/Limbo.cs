using Melia.Shared.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.World.Maps
{
	/// <summary>
	/// Dummy map actors receive by default.
	/// </summary>
	/// <remarks>
	/// Limbo acts as a dummy, so the Map property on actors is never null
	/// and can't cause exceptions.
	/// </remarks>
	public class Limbo : Map
	{
		/// <summary>
		/// Initializes dummy map.
		/// </summary>
		public Limbo() : base(0, "__limbo__")
		{
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="packet"></param>
		public override void Broadcast(Packet packet)
		{
			//Log.Warning("Broadcast in Limbo.");
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="source"></param>
		public override void BroadcastToViewers(Packet packet, IActor source)
		{
		}
	}
}
