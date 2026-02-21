using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Scheduling;
using Melia.Zone.Events.Arguments;
using Yggdrasil.Logging;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Buffs.Handlers.Common;

namespace Melia.Zone.World.Actors.Monsters
{
	public class Companion : Mob, IPropertyObject
	{
		/// <summary>
		/// Companion's unique id.
		/// </summary>
		public long DbId { get; set; }

		/// <summary>
		/// Companion's globally unique id.
		/// </summary>
		public long ObjectId => ObjectIdRanges.Companions + this.DbId;

		/// <summary>
		/// Returns whether the monster is dead.
		/// </summary>
		public override bool IsDead => (this.Hp == 0) || this.Buffs.TryGet(BuffId.Pet_Dead, out var _);

		/// <summary>
		/// A reference to the character which owns this companion.
		/// </summary>
		public Character Owner { get; private set; }

		/// <summary>
		/// Companion's slot in the companion list.
		/// </summary>
		public int Slot { get; set; }

		/// <summary>
		/// Companion is activated (Visible)
		/// </summary>
		public bool IsActivated
		{
			get
			{
				return this.Properties.GetFloat(PropertyName.IsActivated) == 1;
			}
			set
			{
				this.Properties.SetFloat(PropertyName.IsActivated, value ? 1 : 0);
			}
		}

		/// <summary>
		/// Current experience points.
		/// </summary>
		public long Exp
		{
			get => (long)this.Properties.GetFloat("EXP", 0);
			set => this.Properties.SetFloat("EXP", value);
		}

		/// <summary>
		/// Current maximum experience points.
		/// </summary>
		public long MaxExp { get; set; }

		/// <summary>
		/// Total number of accumulated experience points.
		/// </summary>
		public long TotalExp { get; set; }
		public DateTime AdoptTime { get; set; }
		public bool IsRiding { get; set; } = false;

		/// <summary>
		/// Whether the companion is in aggressive mode (auto-attack).
		/// </summary>
		public bool IsAggressiveMode { get; set; } = false;

		public CompanionData CompanionData { get; private set; }

		public bool IsBird => (JobId)this.CompanionData.JobId == JobId.Falconer;

		public Companion(Character owner, int id, RelationType type) : base(id, type)
		{
			this.Owner = owner;

			if (!ZoneServer.Instance.Data.CompanionDb.TryFindByClassName(this.Data.ClassName, out var companionData))
				throw new NullReferenceException("No companion data found for '" + this.Data.ClassName + "'.");
			this.CompanionData = companionData;

			_staminaDecayTime = TimeSpan.FromMilliseconds(60000);
			this.Properties = new CompanionProperties(this);
			this.Properties.AddDefaultProperties();
			this.InitEvents();
		}

		/// <summary>
		/// Initializes properties for the first time a companion is obtained
		/// </summary>
		public void InitProperties()
		{
			// Only initialize once (safe check)
			// If companion has DbId or existing exp, it's already been initialized
			if (this.DbId > 0 || this.Exp > 0 || this.TotalExp > 0)
			{
				var maxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(ExpType.Pet, this.Level);
				if (this.MaxExp != maxExp)
					this.MaxExp = maxExp;

				this.Properties.InvalidateAll();
				this.Properties.InitAutoUpdates();
				return;
			}

			this.Properties.Invalidate(PropertyName.MHP, PropertyName.MaxStamina);
			this.InitAutoUpdates();

			this.Level = 1;
			this.Exp = 0;
			this.TotalExp = 0;
			this.MaxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(ExpType.Pet, 1);

			this.Properties.SetFloat(PropertyName.HP, this.Properties.GetFloat(PropertyName.MHP));
			this.Properties.SetFloat(PropertyName.Stamina, this.Properties.GetFloat(PropertyName.MaxStamina));

			this.Properties.InvalidateAll();
		}

		/// <summary>
		/// Sets up auto updates for companion properties.
		/// </summary>
		/// <remarks>
		/// Call after all properties were loaded, as to not trigger
		/// auto-updates before all properties are in place.
		/// </remarks>
		public void InitAutoUpdates()
		{
			this.Properties.InitAutoUpdates();
			this.Properties.AutoUpdate(PropertyName.MHP, [PropertyName.Lv, PropertyName.Stat_MHP]);
			this.Properties.AutoUpdate(PropertyName.MINPATK, [PropertyName.Lv, PropertyName.Stat_ATK]);
			this.Properties.AutoUpdate(PropertyName.MAXPATK, [PropertyName.Lv, PropertyName.Stat_ATK]);
			this.Properties.AutoUpdate(PropertyName.MINMATK, [PropertyName.Lv, PropertyName.Stat_ATK]);
			this.Properties.AutoUpdate(PropertyName.MAXMATK, [PropertyName.Lv, PropertyName.Stat_ATK]);
			this.Properties.AutoUpdate(PropertyName.ATK, [PropertyName.Lv, PropertyName.Stat_ATK]);
			this.Properties.AutoUpdate(PropertyName.DEF, [PropertyName.Lv, PropertyName.Stat_DEF]);
			this.Properties.AutoUpdate(PropertyName.MDEF, [PropertyName.Lv, PropertyName.Stat_MDEF]);
			this.Properties.AutoUpdate(PropertyName.HR, [PropertyName.Lv, PropertyName.DEX, PropertyName.Stat_HR]);
			this.Properties.AutoUpdate(PropertyName.CRTHR, [PropertyName.Lv, PropertyName.DEX, PropertyName.Stat_CRTHR]);
			this.Properties.AutoUpdate(PropertyName.DR, [PropertyName.Lv, PropertyName.DEX, PropertyName.Stat_DR]);
			this.Properties.AutoUpdate(PropertyName.RHP, [PropertyName.MHP]);
		}

		/// <summary>
		/// Sets up event subscriptions.
		/// </summary>
		private void InitEvents()
		{
			this.CombatState.CombatStateChanged += this.OnCombatStateChanged;

			if (this.Components.TryGet<BuffComponent>(out var buffComponent))
			{
				buffComponent.BuffStarted += this.OnBuffsChanged;
				buffComponent.BuffUpdated += this.OnBuffsChanged;
				buffComponent.BuffEnded += this.OnBuffsChanged;
			}
		}

		public void SetCompanionState(bool isActive)
		{
			this.IsActivated = isActive;
			Send.ZC_OBJECT_PROPERTY(this.Owner.Connection, this, PropertyName.IsActivated);
			if (isActive)
			{
				this.Map = this.Owner.Map;
				this.Layer = this.Owner.Layer;
				this.OwnerHandle = this.Owner.Handle;
				this.Position = this.Owner.Position.GetRandomInRange2D(15);
				this.SpawnPosition = this.Position;
				this.Components.Add(new RecoveryComponent(this));
				this.Components.Add(new MovementComponent(this));
				if (this.IsBird)
					this.Components.Add(new AiComponent(this, "PC_Pet_Hawk_Debug", this.Owner));
				else
					this.Components.Add(new AiComponent(this, "PC_Pet", this.Owner));
				Send.ZC_NORMAL.PetIsInactive(this.Owner.Connection, this);
				this.Map.AddMonster(this);
				var hpValue = this.Properties.GetFloat(PropertyName.HP);
				Send.ZC_OBJECT_PROPERTY(this.Owner.Connection, this);
				Send.ZC_NORMAL.PetPlayAnimation(this.Owner.Connection, this);
				// Probably speed is not a fixed 90f value
				Send.ZC_MSPD(this.Owner, this, this.ObjectId, 90f);
				Send.ZC_PET_AUTO_ATK(this.Owner, this);
				Send.ZC_NORMAL.PetInfo(this.Owner);
				// Note: PvP/duel relation handling is done in HandleAppearingMonsters
			}
			else
			{
				this.Components.Remove<RecoveryComponent>();
				this.Components.Remove<AiComponent>();
				this.Components.Remove<MovementComponent>();
				this.Position = Position.Zero;
				this.OwnerHandle = 0;
				// Clear Buffs
				Send.ZC_LEAVE(this, LeaveType.Companion);
				this.Map.RemoveMonster(this);
			}
		}

		/// <summary>
		/// Grants exp to companion.
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="monster"></param>
		public void GiveExp(long exp, IMonster monster)
		{
			// Don't give exp while dead
			if (this.IsDead)
				return;

			// Base EXP
			this.Exp += exp;
			this.TotalExp += exp;

			Send.ZC_NORMAL.PetExpUpdate(this.Owner, this);

			var level = this.Level;
			var levelUps = 0;
			var maxExp = this.MaxExp;
			var maxLevel = ZoneServer.Instance.Data.ExpDb.GetMaxLevel();

			// Consume EXP as many times as possible to reach new levels
			while (this.Exp >= maxExp && level < maxLevel)
			{
				this.Exp -= maxExp;

				level++;
				levelUps++;
				maxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(ExpType.Pet, level);
			}

			// Execute level up only once to avoid client lag on multiple
			// level ups. Leveling up a thousand times in a loop is not
			// fun for the client =D"
			if (levelUps > 0)
				this.LevelUp(levelUps);
		}

		/// <summary>
		/// Increases companion's level by the given amount.
		/// </summary>
		/// <param name="amount"></param>
		public void LevelUp(int amount = 1)
		{
			if (amount < 1)
				throw new ArgumentException("Amount can't be lower than 1.");

			var newLevel = this.Properties.Modify(PropertyName.Lv, amount);

			this.MaxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(ExpType.Pet, (int)newLevel);
			this.Heal(this.MaxHp, 0);

			this.PlayEffect("F_companion_level_up", 3);
		}

		public override void Kill(ICombatEntity killer)
		{
			Send.ZC_SKILL_CAST_CANCEL(this);
			Send.ZC_SKILL_DISABLE(this);
			//Send.ZC_DEAD(this);

			this.Properties.SetFloat(PropertyName.HP, 1);
			ZoneServer.Instance.ServerEvents.EntityKilled.Raise(new CombatEventArgs(this, killer));
			this.StartBuff(BuffId.Pet_Dead, TimeSpan.FromSeconds(8));
		}

		/// <summary>
		/// Recalculates and updates HP recovery time properties when combat state changes.
		/// </summary>
		/// <param name="combatEntity"></param>
		/// <param name="attackState"></param>
		private void OnCombatStateChanged(ICombatEntity combatEntity, bool attackState)
		{
			this.Properties.Invalidate(PropertyName.RHPTIME);
		}

		/// <summary>
		/// Called when a buff was started, updated, or ended.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		private void OnBuffsChanged(ICombatEntity entity, Buff buff)
		{
			this.Properties.InvalidateAll();

			if (this.Owner?.Connection != null)
				Send.ZC_OBJECT_PROPERTY(this.Owner.Connection, this);
		}

		private TimeSpan _staminaDecayTime;

		/// <summary>
		/// Updates companion's components and handles stamina decay.
		/// </summary>
		/// <param name="elapsed"></param>
		public override void Update(TimeSpan elapsed)
		{
			base.Update(elapsed);

			if (!this.IsActivated || this.Owner == null)
				return;

			_staminaDecayTime -= elapsed;

			if (_staminaDecayTime <= TimeSpan.Zero)
			{
				this.DecayStamina();
				_staminaDecayTime = TimeSpan.FromMilliseconds(60000);
			}
		}

		/// <summary>
		/// Decreases companion stamina over time (hunger system).
		/// </summary>
		private void DecayStamina()
		{
			var decayAmount = 1;
			var maxStamina = (int)this.Properties.GetFloat(PropertyName.MaxStamina, 60);
			var currentStamina = (int)this.Properties.GetFloat(PropertyName.Stamina, 0);

			var newStamina = Math.Max(0, currentStamina - decayAmount);
			this.Properties.SetFloat(PropertyName.Stamina, newStamina);

			Send.ZC_OBJECT_PROPERTY(this.Owner.Connection, this, PropertyName.Stamina);
		}

		/// <summary>
		/// Heals the companion's HP and SP by the given amounts. Applies potential
		/// (de)buffs that affect healing. Uses ZC_HEAL_INFO for companions instead
		/// of ZC_ADD_HP.
		/// </summary>
		/// <param name="hpAmount"></param>
		/// <param name="spAmount"></param>
		public override void Heal(float hpAmount, float spAmount)
		{
			if (this.IsDead)
				return;

			if (hpAmount == 0 && spAmount == 0)
				return;

			DecreaseHeal_Debuff.TryApply(this, ref hpAmount);

			this.Properties.Modify(PropertyName.HP, hpAmount);
			this.Properties.Modify(PropertyName.SP, spAmount);

			this.HpChangeCounter++;

			if (hpAmount > 0)
			{
				Send.ZC_HEAL_INFO(this, hpAmount, this.Hp, HealType.Hp);
				Send.ZC_ADD_HP(this, hpAmount, this.Hp, this.HpChangeCounter);
			}

		}

		/// <summary>
		/// Feeds the companion, restoring stamina and optionally playing animation.
		/// If the food type matches the companion's food group, also fully restores HP.
		/// </summary>
		/// <param name="staminaAmount">Amount of stamina to restore.</param>
		/// <param name="foodType">Type of food being used.</param>
		/// <param name="animationName">Optional animation name to play.</param>
		public void Feed(int staminaAmount, CompanionFoodType foodType, string animationName = null)
		{
			var maxStamina = (int)this.Properties.GetFloat(PropertyName.MaxStamina, 60);
			var currentStamina = (int)this.Properties.GetFloat(PropertyName.Stamina, 0);

			var newStamina = Math.Min(maxStamina, currentStamina + staminaAmount);
			this.Properties.SetFloat(PropertyName.Stamina, newStamina);

			Send.ZC_OBJECT_PROPERTY(this.Owner.Connection, this, PropertyName.Stamina);

			if (this.CompanionData.FoodGroup == (int)foodType)
			{
				this.Heal(this.MaxHp, 0);
			}

			if (!string.IsNullOrEmpty(animationName))
			{
				this.DetachEffect("F_sys_heart");
				this.AttachEffect("F_sys_heart", 2, EffectLocation.Top);

				Task.Delay(2000).ContinueWith(_ =>
				{
					this.DetachEffect("F_sys_heart");
				});
			}
		}

		internal void SetHide(bool isHidden)
		{
			if (isHidden)
			{
				this.Visibility = ActorVisibility.NoOne;
			}
			else
			{
				this.Visibility = ActorVisibility.Individual;
			}
		}

		/// <summary>
		/// Updates the companion's position without triggering movement broadcasts.
		/// Used when syncing position with attached entities.
		/// </summary>
		internal void SetPositionDirect(Position pos)
		{
			this.Position = pos;
		}
	}
}
