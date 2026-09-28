using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	/// <summary>
	/// An effect that plays an animation on an actor for every client that sees it.
	/// </summary>
	public class PlayAnimationEffect : Effect
	{
		/// <summary>
		/// Gets the name of the animation to play.
		/// </summary>
		public string AnimationName { get; }

		/// <summary>
		/// Gets whether the animation stops on its last frame instead of looping.
		/// </summary>
		public bool StopOnLastFrame { get; }

		/// <summary>
		/// Creates a new animation effect.
		/// </summary>
		/// <param name="animationName">Name of the animation (e.g., "event_loop").</param>
		/// <param name="stopOnLastFrame">If true, the animation plays once and holds its last frame.</param>
		public PlayAnimationEffect(string animationName, bool stopOnLastFrame = false)
		{
			this.AnimationName = animationName;
			this.StopOnLastFrame = stopOnLastFrame;
		}

		/// <summary>
		/// Plays the animation for a specific connection.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_PLAY_ANI(conn, actor, this.AnimationName, this.StopOnLastFrame);
		}
	}
}
