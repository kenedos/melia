using System;
using Melia.Zone.Network;
using Melia.Zone.Util;

namespace Melia.Zone.World.Actors.Effects
{
	public class ColorEffect : Effect
	{
		public byte Yellow { get; }
		public byte Magenta { get; }
		public byte Cyan { get; }
		public byte Alpha { get; }
		public float TransitionDuration { get; }
		public byte UnkByte { get; }
		public ColorEffect(byte yellow, byte magenta, byte cyan, byte alpha, float transitionDuration = 0, byte unkByte = 1)
		{
			this.Yellow = yellow;
			this.Magenta = magenta;
			this.Cyan = cyan;
			this.Alpha = alpha;
			this.TransitionDuration = transitionDuration;
			this.UnkByte = unkByte;
		}

		public static ColorEffect FromRgba(float r, float g, float b, float a, float transition = 0, byte unkByte = 1)
		{
			return new ColorEffect(
				yellow: GameMathUtil.ToByte(1f - b),
				magenta: GameMathUtil.ToByte(1f - g),
				cyan: GameMathUtil.ToByte(1f - r),
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
