using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	public class TransmuteEffect : Effect
	{
		private readonly int _id;
		private readonly BuffId _buffId;

		public TransmuteEffect(int id, BuffId buffId = 0)
		{
			this._id = id;
			this._buffId = buffId;
		}

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			//Send.ZC_NORMAL.Transmute
			Send.ZC_NORMAL.Transmutation(conn, actor, _id, _buffId);
		}

		public override void OnRemove(IActor actor)
		{
			Send.ZC_NORMAL.Transmutation(actor, 0);
		}
	}
}
