using Melia.Shared.Data.Database;
using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	public class AttachEffect : Effect
	{
		public string EffectName { get; }
		public float Scale { get; }
		public EffectLocation HeightOffset { get; }
		public float F1 { get; }
		public float F2 { get; }
		public float F3 { get; }
		public byte B1 { get; }
		public byte B2 { get; }
		public byte B3 { get; }
		public byte B4 { get; }

		public AttachEffect(string effectName, float effectScale, EffectLocation heightOffset = EffectLocation.Unknown, float f1 = 0, float f2 = 0, float f3 = 0, byte b1 = 0, byte b2 = 0, byte b3 = 0, byte b4 = 0)
		{
			this.EffectName = effectName;
			this.Scale = effectScale;
			this.HeightOffset = heightOffset;
			this.F1 = f1;
			this.F2 = f2;
			this.F3 = f3;
			this.B1 = b1;
			this.B2 = b2;
			this.B3 = b3;
			this.B4 = b4;
		}

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.AttachEffect(conn, actor, this.EffectName, this.Scale, this.HeightOffset, this.F1, this.F2, this.F3, this.B1, this.B2, this.B3, this.B4);
		}

		public override void OnRemove(IActor actor)
		{
			Send.ZC_NORMAL.RemoveEffectByName(actor, this.EffectName, true);
		}
	}
}
