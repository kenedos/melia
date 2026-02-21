using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	public class ReviveEffect : Effect
	{
		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.Revive(conn, actor);
		}
	}
}
