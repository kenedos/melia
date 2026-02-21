using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Zone.World.Maps
{
	public class DynamicMap : Map
	{
		public DynamicMap(int id, string name = null) : base(id, name)
		{
			this.Id = id;
			this.WorldId = ZoneServer.Instance.World.GenerateDynamicMapId();
			this.ClassName = name ?? "DynamicMap" + this.Id;
		}

		internal void Clear()
		{
			this._characters.Clear();
			this._monsters.Clear();
			this._monsterPropertyOverrides.Clear();
			this._pads.Clear();
			this._obstacles.Clear();
			this._updateEntitiesPool.Value.Clear();
			this._updateVisibleCharactersPool.Value.Clear();
		}
	}
}
