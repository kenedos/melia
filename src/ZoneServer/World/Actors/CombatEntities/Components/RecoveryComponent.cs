using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// Recovery component, restores an entity's HP, SP, and Stamina
	/// over time.
	/// </summary>
	public class RecoveryComponent : CombatEntityComponent, IUpdateable
	{
		private TimeSpan _rhpTime;
		private TimeSpan _rspTime;
		private TimeSpan _staminaTime;
		private TimeSpan _shieldTime;

		/// <summary>
		/// Creates new component.
		/// </summary>
		/// <param name="entity"></param>
		public RecoveryComponent(ICombatEntity entity) : base(entity)
		{
		}

		/// <summary>
		/// Updates the entity's stats.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			if (this.Entity.IsDead)
				return;

			this.UpdateHp(elapsed);
			if (this.Entity is Character)
			{
				this.UpdateSp(elapsed);
				this.UpdateStamina(elapsed);
			}
			if (this.Entity is Mob mob && mob.Rank == MonsterRank.Boss)
			{
				this.UpdateShield(elapsed);
			}
		}

		/// <summary>
		/// Updates the entity's HP.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateHp(TimeSpan elapsed)
		{
			_rhpTime -= elapsed;

			if (_rhpTime <= TimeSpan.Zero)
			{
				this.RecoverHp();
				_rhpTime = TimeSpan.FromMilliseconds(this.Entity.Properties.GetFloat(PropertyName.RHPTIME));
			}
		}

		/// <summary>
		/// Updates the entity's SP.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateSp(TimeSpan elapsed)
		{
			_rspTime -= elapsed;

			if (_rspTime <= TimeSpan.Zero)
			{
				this.RecoverSp();
				_rspTime = TimeSpan.FromMilliseconds(this.Entity.Properties.GetFloat(PropertyName.RSPTIME));
			}
		}

		/// <summary>
		/// Updates the entity's stamina.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateStamina(TimeSpan elapsed)
		{
			_staminaTime -= elapsed;

			if (_staminaTime <= TimeSpan.Zero)
			{
				this.RecoverStamina();
				_staminaTime = TimeSpan.FromMilliseconds(this.Entity.Properties.GetFloat(PropertyName.Sta_R_Delay));
			}
		}

		/// <summary>
		/// Updates entity's shields.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateShield(TimeSpan elapsed)
		{
			_shieldTime -= elapsed;

			if (_shieldTime <= TimeSpan.Zero)
			{
				if (this.Entity is Mob mob && !mob.CombatState.AttackState)
					this.RecoverShield();

				// Using HP Regen time
				_shieldTime = TimeSpan.FromMilliseconds(this.Entity.Properties.GetFloat(PropertyName.RHPTIME));
			}
		}

		/// <summary>
		/// Recovers some HP.
		/// </summary>
		private void RecoverHp()
		{
			var cur = this.Entity.Properties.GetFloat(PropertyName.HP);
			var max = this.Entity.Properties.GetFloat(PropertyName.MHP);
			var rec = this.Entity.Properties.GetFloat(PropertyName.RHP);

			if (rec > 0 && cur < max)
				this.Entity.Heal(rec, 0);
		}

		/// <summary>
		/// Recovers some SP.
		/// </summary>
		private void RecoverSp()
		{
			var cur = this.Entity.Properties.GetFloat(PropertyName.SP);
			var max = this.Entity.Properties.GetFloat(PropertyName.MSP);
			var rec = this.Entity.Properties.GetFloat(PropertyName.RSP);

			if (rec > 0 && cur < max)
				this.Entity.Heal(0, rec);
		}

		/// <summary>
		/// Recovers some Shield.
		/// </summary>
		private void RecoverShield()
		{
			if (this.Entity is not Mob mob)
				return;

			var cur = mob.Shield;
			var max = mob.MaxShield;
			var rec = (int)this.Entity.Properties.GetFloat(PropertyName.RHP);

			if (rec > 0 && cur < max)
				mob.HealShield(rec);
		}

		/// <summary>
		/// Recovers or drains stamina.
		/// </summary>
		private void RecoverStamina()
		{
			// Stamina is weird. Do monsters have stamina? Why is there
			// no property for the current stamina? Should we make it a
			// separate component if only characters have it? Meh.
			if (!(this.Entity is Character character))
				return;

			var stamina = character.Properties.Stamina;
			var maxStamina = (int)character.Properties.GetFloat(PropertyName.MaxSta);

			var prev = stamina;

			// Drain stamina during movement, recover otherwise
			if (character.Movement.IsMoving)
			{
				var runDrain = (int)character.Properties.GetFloat(PropertyName.Sta_Run, 0);
				stamina = Math2.Clamp(0, maxStamina, stamina - runDrain);
			}
			else
			{
				var recovery = (int)character.Properties.GetFloat(PropertyName.Sta_Recover, 0);
				stamina = Math2.Clamp(0, maxStamina, stamina + recovery);
			}

			if (prev == stamina)
				return;

			character.Properties.Stamina = stamina;
			Send.ZC_STAMINA(character, stamina);
		}

		internal void ResetSpRecoveryTime()
		{
			_rspTime = TimeSpan.FromMilliseconds(this.Entity.Properties.GetFloat(PropertyName.RSPTIME));
		}
	}
}
