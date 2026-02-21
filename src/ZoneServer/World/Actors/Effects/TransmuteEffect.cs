using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	public class TransmuteEffect : Effect
	{
		private readonly int _id;

		public TransmuteEffect(int id)
		{
			this._id = id;
		}

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			//Send.ZC_NORMAL.Transmute
			Send.ZC_NORMAL.Transmutation(conn, actor, _id);
		}

		public override void OnRemove(IActor actor)
		{
			Send.ZC_NORMAL.Transmutation(actor, 0);
		}
	}
}
