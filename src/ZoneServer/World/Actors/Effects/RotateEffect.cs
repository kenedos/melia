using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	internal class RotateEffect : Effect
	{
		public float X { get; }
		public float Y { get; }
		public float Z { get; }
		public RotateEffect(float angleX = 0, float angleY = 0, float angleZ = 0)
		{
			this.X = angleX;
			this.Y = angleY;
			this.Z = angleZ;
		}

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.ActorRotate(conn, actor, this.X, this.Y, this.Z);
		}
	}
}
