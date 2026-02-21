using System;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Monsters
{
	public class Summon : Mob
	{
		/// <summary>
		/// A reference to the character which owns this summon.
		/// </summary>
		public ICombatEntity Owner { get; private set; }
		public long Experience { get; set; }
		public DateTime CreateTime { get; set; }

		public Summon(ICombatEntity owner, int id, RelationType type) : base(id, type)
		{
			this.Owner = owner;
		}

		public void SetState(bool isActive, bool canMove = true, bool hasAi = true)
		{
			if (isActive)
			{
				this.Map = this.Owner.Map;
				this.OwnerHandle = this.Owner.Handle;
				this.AssociatedHandle = this.Owner.Handle;
				this.Position = this.Owner.Position.GetRandomInRange2D(15, RandomProvider.Get());
				if (canMove)
					this.Components.Add(new MovementComponent(this));
				if (hasAi)
					this.Components.Add(new AiComponent(this, "BasicMonster", this.Owner));
				Send.ZC_SET_POS(this);
				this.Map.AddMonster(this);
				Send.ZC_HARDCODED_SKILL(this, 1);
				// Note: PvP/duel relation handling is done in HandleAppearingMonsters
			}
			else
			{
				Send.ZC_HARDCODED_SKILL(this, 0);
				this.Position = Position.Zero;
				this.OwnerHandle = 0;
				this.AssociatedHandle = 0;
				// Clear Buffs
				this.Map.RemoveMonster(this);
				this.Components.Get<BuffComponent>()?.RemoveAll();
				this.Components.Remove<BuffComponent>();
				this.Components.Remove<AiComponent>();
				this.Components.Remove<MovementComponent>();
			}
		}
	}
}
