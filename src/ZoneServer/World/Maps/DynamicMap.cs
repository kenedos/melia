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
			lock (_characters)
				_characters.Clear();
			lock (_monsters)
				_monsters.Clear();
			lock (_combatEntities)
				_combatEntities.Clear();
			lock (_triggerableAreas)
				_triggerableAreas.Clear();
			lock (_monsterPropertyOverrides)
				_monsterPropertyOverrides.Clear();
			lock (_spawnBuffs)
				_spawnBuffs.Clear();
			lock (_pads)
				_pads.Clear();
			lock (_obstaclesLock)
				_obstacles.Clear();
		}
	}
}
