using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Composition;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// LifeTime component, an entity lives for a fixed duration.
	/// </summary>
	public class LifeTimeComponent : CombatEntityComponent, IUpdateable
	{
		private TimeSpan _lifeTime;

		/// <summary>
		/// Creates new component.
		/// </summary>
		/// <param name="entity"></param>
		public LifeTimeComponent(ICombatEntity entity, TimeSpan lifeTime) : base(entity)
		{
			this._lifeTime = lifeTime;
		}

		/// <summary>
		/// Updates the LifeCycle of an entity.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			_lifeTime -= elapsed;

			if (!this.Entity.IsDead && _lifeTime <= TimeSpan.Zero)
			{
				this.Entity.Kill(null);
			}
		}
	}
}
