// ===================================================================
// CharacterCombat.cs - Combat and health management
// ===================================================================
using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Buffs.Handlers.Common;
using Melia.Zone.Buffs.Handlers.Scout.Assassin;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Items.Effects;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		#region Healing Methods
		/// <summary>
		/// Heals character's HP, SP, and Stamina fully and updates the client.
		/// </summary>
		public void FullHeal()
		{
			this.ModifyHp(this.MaxHp);
			this.ModifySp(this.MaxSp);
		}

		/// <summary>
		/// Heals character's HP and SP by the given amounts and updates the client.
		/// </summary>
		public void Heal(float hpAmount, float spAmount)
		{
			if (!this.IsResurrecting && this.IsDead)
				return;

			if (hpAmount == 0 && spAmount == 0)
				return;

			DecreaseHeal_Debuff.TryApply(this, ref hpAmount);
			PiercingHeart_Debuff.TryApply(this, ref hpAmount);

			this.ModifyHpSafe(hpAmount, out var hp, out var priority);
			if (hpAmount > 0)
			{
				Send.ZC_HEAL_INFO(this, hpAmount, this.Hp, HealType.Hp);
			}
			this.Properties.Modify(PropertyName.SP, spAmount);
			if (spAmount > 0)
				Send.ZC_HEAL_INFO(this, spAmount, this.Sp, HealType.Sp);

			Send.ZC_UPDATE_ALL_STATUS(this, priority);
		}

		/// <summary>
		/// Modifies character's HP by the given amount without updating the client.
		/// </summary>
		public void ModifyHpSafe(float amount, out float newHp, out int priority)
		{
			lock (_hpLock)
			{
				newHp = (int)this.Properties.Modify(PropertyName.HP, amount);
				priority = (this.HpChangeCounter += 1);
			}
			this.Connection.Party?.UpdateMemberInfo(this);
			// this.Connection.Guild?.UpdateMemberInfo(this); // Removed: Guild type deleted
		}

		/// <summary>
		/// Modifies character's HP by the given amount and updates the client.
		/// </summary>
		public void ModifyHp(float amount)
		{
			this.ModifyHpSafe(amount, out var hp, out var priority);
			Send.ZC_ADD_HP(this, amount, hp, priority);
		}

		/// <summary>
		/// Modifies character's SP by the given amount and updates the client.
		/// </summary>
		public void ModifySp(float amount)
		{
			var sp = this.Properties.Modify(PropertyName.SP, amount);
			Send.ZC_UPDATE_SP(this, sp, true);
			this.Connection.Party?.UpdateMemberInfo(this);
			// this.Connection.Guild?.UpdateMemberInfo(this); // Removed: Guild type deleted
		}

		/// <summary>
		/// Modifies character's current stamina and updates the client.
		/// </summary>
		public void ModifyStamina(int amount)
		{
			this.Properties.Stamina += amount;
			Send.ZC_STAMINA(this, this.Properties.Stamina);
		}

		/// <summary>
		/// Reduces character's stamina and updates the client.
		/// </summary>
		private void UseStamina(int staminaUsage)
		{
			var stamina = (this.Properties.Stamina -= staminaUsage);
			Send.ZC_STAMINA(this, stamina);
		}
		#endregion

		#region Combat Methods
		/// <summary>
		/// Makes character take damage and kills them if their HP reached 0.
		/// </summary>
		public virtual bool TakeDamage(float damage, ICombatEntity attacker)
		{
			if (this.IsDead)
				return true;

			if (this.IsSafe())
				return false;

			if (this.IsAnyBuffActive(BuffId.Skill_NoDamage_Buff,
				BuffId.LiedDerWeltbaum_NoDamage_Buff, BuffId.EarringRaid_PartyLeaderBuff_NoDamage,
				BuffId.InfernalShadow_CasterNoDamage_Buff))
				return false;

			if (damage > 0 && this.IsBuffActive(BuffId.SitRest))
				this.RemoveBuff(BuffId.SitRest);

			if (damage > 0)
			{
				this.Components.Get<CombatComponent>().TryInterruptCasting(out _);
				this.Components.Get<TimeActionComponent>().End(TimeActionResult.CancelledByHit);
			}

			this.Components.Get<CombatComponent>().SetAttackState(true);
			this.ModifyHpSafe(-damage, out _, out _);

			this.Components.Get<CombatComponent>()?.RegisterHit(attacker, damage);

			if (this.Hp < this.MaxHp / 2)
				this.ShowHelp("TUTO_RECOVERY");
			if (this.Hp == 0)
			{
				if (this.TryGetBuff(BuffId.Cleric_Revival_Buff, out var reviveBuff))
				{
					this.ModifyHpSafe(1, out _, out _);
					reviveBuff.Activate(Zone.Buffs.Base.ActivationType.Start);
				}
				else
					this.Kill(attacker);
			}

			this.Map.AlertNearbyAis(this, new HitEventAlert(this, attacker, damage));

			this.Damaged?.Invoke(this, damage, attacker);

			return this.IsDead;
		}

		/// <summary>
		/// Kills character.
		/// </summary>
		public virtual void Kill(ICombatEntity killer)
		{
			this.Properties.SetFloat(PropertyName.HP, 0);
			this.Buffs.RemoveAll(b => b.Data.RemoveOnDeath);

			if (killer.Components.TryGet<AiComponent>(out var aiComponent))
				aiComponent.Script.QueueEventAlert(new CancelSkillAlert());

			if (this.ActiveCompanion != null)
			{
				_companionToReactivate = this.ActiveCompanion;
				this.Companions.ActiveCompanion.SetCompanionState(false);
			}

			if (this.Summons.Count != 0)
			{
				var summons = this.Summons.GetSummons();
				foreach (var summon in summons)
					summon.Kill(null);
			}

			this.Died?.Invoke(this, killer);
			ZoneServer.Instance.ServerEvents.EntityKilled.Raise(new CombatEventArgs(this, killer));

			// Invoke card Dead hooks (e.g., auto-revival cards like Durahan)
			ItemHookRegistry.Instance.InvokeDeadHooks(this, killer);

			if (Feature.IsEnabled(FeatureId.BountyHunterSystem) && killer is Character killerCharacter && killerCharacter != this)
			{
				ZoneServer.Instance.World.BountyManager.ClaimBounty(killerCharacter, this);
			}

			Send.ZC_DEAD(this);

			if (this.IsDueling)
				ZoneServer.Instance.World.Duels.EndDuel(this.Connection.ActiveDuel, killer);
			this.Tracks.Cancel();

			// Durability damage on death
			if (!this.Map.IsGTW && !this.Map.IsCity)
			{
				foreach (var equip in this.Inventory.GetEquip().Values)
				{
					if (equip is DummyEquipItem || equip.Durability <= 0)
						continue;
					equip.ModifyDurability(this, (int)Math.Floor(equip.MaxDurability * -0.2f));
				}
			}

			_resurrectDialogTimer = ResurrectDialogDelay;
		}

		/// <summary>
		/// Resurrects the character if its dead.
		/// </summary>
		public void Resurrect(ResurrectOptions option)
		{
			if (option == ResurrectOptions.SoulCrystal)
			{
				// Cancel ress if no soul crystals were removed
				if (this.RemoveItem("RestartCristal", 1) == 0)
				{
					return;
				}
			}

			this.IsResurrecting = true;

			switch (option)
			{
				case ResurrectOptions.NearestRevivalPoint:
				{
					var startHp = this.Properties.GetFloat(PropertyName.MHP) * 0.25f;
					this.Heal(startHp, 0);

					var safePos = this.Map.GetSafePositionNear(this.Position, true);
					this.Warp(this.MapId, safePos);
					break;
				}
				case ResurrectOptions.NearestCity:
				{
					var startHp = this.Properties.GetFloat(PropertyName.MHP) * 0.25f;
					this.Heal(startHp, 0);

					var location = this.GetCityReturnLocation();
					this.Warp(location);
					break;
				}
				case ResurrectOptions.TryAgain:
				case ResurrectOptions.SoulCrystal:
				default:
				{
					this.Heal(this.MaxHp, 0);
					break;
				}
			}

			Send.ZC_RESURRECT_SAVE_POINT_ACK(this);
			Send.ZC_RESURRECT(this);
			this.IsResurrecting = false;

			if (_companionToReactivate != null)
			{
				_companionToReactivate.SetCompanionState(true);
				_companionToReactivate = null;
			}
		}

		/// <summary>
		/// Returns true if the character can attack the entity.
		/// </summary>
		public virtual bool CanAttack(ICombatEntity entity)
		{
			if (entity == this || this.Handle == entity.Handle || entity.IsDead)
				return false;

			if (this.IsLocked(LockType.Attack))
				return false;

			if (!this.CanSee(entity))
				return false;

			if (!this.IsEnemy(entity))
				return false;

			if (entity.IsSafe())
				return false;

			return true;
		}

		/// <summary>
		/// Returns true if the character can attack others.
		/// </summary>
		public bool CanFight()
		{
			return !this.IsDead && !this.IsCasting() && !this.IsLocked(LockType.Attack);
		}

		/// <summary>
		/// Returns when the character can guard.
		/// </summary>
		public bool CanGuard()
		{
			if (this.Properties.GetFloat(PropertyName.Guardable) != 1)
				return false;
			if (this.IsKnockedDown() || this.IsCasting())
				return false;
			return true;
		}

		/// <summary>
		/// Returns if the character can be staggered.
		/// </summary>
		public bool CanStagger() => false;

		/// <summary>
		/// Returns true if the character can be knocked down.
		/// </summary>
		public bool IsKnockdownable() => true;
		#endregion
	}
}
