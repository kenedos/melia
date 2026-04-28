using System;
using Melia.Zone.Network;
using Melia.Zone.Util;

namespace Melia.Zone.World.Actors.Effects
{
	/// <summary>
	/// Persistent actor color tint sent via ZC_NORMAL.SetActorColor.
	/// Property/param names Yellow/Magenta/Cyan are legacy/misleading — the bytes are actually sent and interpreted by the client as Blue, Green, Red, Alpha.
	/// </summary>
	public class ColorEffect : Effect
	{
		public byte Yellow { get; }
		public byte Magenta { get; }
		public byte Cyan { get; }
		public byte Alpha { get; }
		public float TransitionDuration { get; }
		public byte UnkByte { get; }

		/// <summary>
		/// Creates a color tint. Despite the param names, bytes are interpreted as B, G, R, A by the client.
		/// </summary>
		/// <param name="yellow">Blue byte (0-255).</param>
		/// <param name="magenta">Green byte (0-255).</param>
		/// <param name="cyan">Red byte (0-255).</param>
		/// <param name="alpha">Alpha byte (0-255).</param>
		/// <param name="transitionDuration"></param>
		/// <param name="unkByte"></param>
		public ColorEffect(byte yellow, byte magenta, byte cyan, byte alpha, float transitionDuration = 0, byte unkByte = 1)
		{
			this.Yellow = yellow;
			this.Magenta = magenta;
			this.Cyan = cyan;
			this.Alpha = alpha;
			this.TransitionDuration = transitionDuration;
			this.UnkByte = unkByte;
		}

		/// <summary>
		/// Builds a ColorEffect from RGBA floats (0..1) by mapping directly to the BGRA bytes the client expects.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		/// <param name="transition"></param>
		/// <param name="unkByte"></param>
		public static ColorEffect FromRgba(float r, float g, float b, float a, float transition = 0, byte unkByte = 1)
		{
			return new ColorEffect(
				yellow: GameMathUtil.ToByte(b),
				magenta: GameMathUtil.ToByte(g),
				cyan: GameMathUtil.ToByte(r),
				alpha: GameMathUtil.ToByte(a),
				transitionDuration: transition,
				unkByte: unkByte
			);
		}


		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.SetActorColor(conn, actor, this.Yellow, this.Magenta, this.Cyan, this.Alpha, this.TransitionDuration, this.UnkByte);
		}
	}
}
