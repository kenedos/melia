using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.World.Actors.Effects
{
	internal class TranslationEffect : Effect
	{
		public int PadHandle { get; }
		public float Height { get; }
		public TranslationEffect(int padHandle, float height = 0)
		{
			this.PadHandle = padHandle;
			this.Height = height;
		}

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			if (actor.Map.TryGetPad(this.PadHandle, out var pad))
				Send.ZC_NORMAL.PadSetMonsterAltitude(conn, pad, actor, this.Height);
		}
	}
}
