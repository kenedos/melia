using System;
using System.Threading.Tasks;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.World.Actors.Pads.Components
{
	public class PadMovementComponent : ActorMovementComponent
	{
		/// <summary>
		/// Returns the pad that owns this component.
		/// </summary>
		public Pad Pad { get; }

		/// <summary>
		/// Creates new instance for pad.
		/// </summary>
		/// <param name="pad"></param>
		public PadMovementComponent(Pad pad) : base(pad)
		{
			this.Pad = pad;
		}

		/// <summary>
		/// Moves the pad to the given destination and returns once it
		/// arrived there.
		/// </summary>
		/// <remarks>
		/// The wait is deliberately not tied to the skill's cancellation
		/// token, so a pad in flight still reaches its destination once
		/// the skill that spawned it ends.
		/// </remarks>
		/// <param name="destination"></param>
		public async Task MoveToAsync(Position destination)
		{
			var moveTime = this.MoveTo(destination);
			await GameClock.Delay(moveTime);
		}

		/// <summary>
		/// Moves the pad to the given destination and destroys it once it
		/// arrived there.
		/// </summary>
		/// <remarks>
		/// The canonical way to fly a projectile pad. Awaiting the move
		/// and destroying the pad by hand risks leaking it onto the map
		/// forever if the wait is cancellable.
		/// </remarks>
		/// <param name="destination"></param>
		/// <param name="earlyDestroy">Time to cut off the end of the move.</param>
		public async Task MoveToAndDestroy(Position destination, TimeSpan earlyDestroy = default)
		{
			var moveTime = this.MoveTo(destination) - earlyDestroy;
			await GameClock.Delay(moveTime);

			this.Pad.Destroy();
		}

		/// <summary>
		/// Updates the pad's movement on nearby clients.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dest"></param>
		/// <param name="speed"></param>
		protected override void UpdateMoveTo(Position pos, Position dest, float speed)
		{
			if (this.Pad.Map == null)
				return;

			Send.ZC_NORMAL.PadMoveTo(this.Pad, dest, speed);
		}

		/// <summary>
		/// Stops the pad at the given position on nearby clients.
		/// </summary>
		/// <param name="pos"></param>
		protected override void UpdateStop(Position pos)
		{
			if (this.Pad.Map == null)
				return;

			// It's possible there's a dedicated packet for stopping pad movement,
			// but for now we'll just send a move with a very high speed, so it
			// snaps there instantly and stops moving. Alternatively, we could
			// just not send anything and hope it's in the right place.
			Send.ZC_NORMAL.PadMoveTo(this.Pad, pos, 10000);
		}
	}
}
