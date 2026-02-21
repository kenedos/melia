using System;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	/// <summary>
	/// An effect that shows an emoticon above an actor's head.
	/// </summary>
	public class EmoticonEffect : Effect
	{
		/// <summary>
		/// Gets the name of the emoticon to display.
		/// </summary>
		public string EmoticonName { get; }

		/// <summary>
		/// Gets the duration to show the emoticon for.
		/// </summary>
		public TimeSpan Duration { get; }

		/// <summary>
		/// Creates a new emoticon effect.
		/// </summary>
		/// <param name="emoticonName">Name of the emoticon (e.g., "I_emo_mvp").</param>
		/// <param name="duration">Duration to display the emoticon.</param>
		public EmoticonEffect(string emoticonName, TimeSpan duration)
		{
			this.EmoticonName = emoticonName;
			this.Duration = duration;
		}

		/// <summary>
		/// Shows the emoticon effect to a specific connection.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_SHOW_EMOTICON(conn, actor, this.EmoticonName, this.Duration);
		}
	}
}
