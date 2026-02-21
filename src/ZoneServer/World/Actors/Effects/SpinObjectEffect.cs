using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	internal class SpinObjectEffect : Effect
	{
		public float SpinDelay { get; }
		public int SpinCount { get; }
		public float RotationPerSecond { get; }
		public float VelocityChangeTerm { get; }

		public SpinObjectEffect(float spinDelay = 0, int spinCount = -1, float rotationPerSecond = 0.2f, float velocityChangeTerm = 0)
		{
			this.SpinDelay = spinDelay;
			this.SpinCount = spinCount;
			this.RotationPerSecond = rotationPerSecond;
			this.VelocityChangeTerm = velocityChangeTerm;
		}

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.SpinObject(conn, actor, this.SpinDelay, this.SpinCount, this.RotationPerSecond, this.VelocityChangeTerm);
		}
	}
}
