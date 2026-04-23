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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
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
		/// Returns the layer on which this companion exists.
		/// When activated, always matches the owner's layer so the
		/// companion stays visible after layer changes (e.g. dungeons).
		/// </summary>
		public override int Layer
		{
			get => this.IsActivated && this.Owner != null ? this.Owner.Layer : base.Layer;
			set => base.Layer = value;
		}

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
		/// Whether the companion should mount its owner when it becomes
		/// visible on the client. Set during map change when the owner
		/// was riding before warping. The mount buff is applied once
		/// the companion appears to the owner via HandleAppearingSingleMonster.
		/// </summary>
		public bool PendingMount { get; set; } = false;

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
				this.Map?.RemoveMonster(this);
				this.Map = this.Owner.Map;
				this.Layer = this.Owner.Layer;
				this.OwnerHandle = this.Owner.Handle;
				if (!this.Map.Ground.TryGetNearestValidPosition(this.Owner.Position.GetRandomInRange2D(15), out var validPos))
					validPos = this.Owner.Position;
				this.Position = validPos;
				this.SpawnPosition = this.Position;
				this.Components.Add(new RecoveryComponent(this));
				this.Components.Add(new MovementComponent(this));
				if (this.IsBird)
					this.Components.Add(new AiComponent(this, "PC_Pet_Hawk", this.Owner));
				else
					this.Components.Add(new AiComponent(this, "PC_Pet", this.Owner));
				Send.ZC_NORMAL.PetIsInactive(this.Owner.Connection, this);
				this.Map.AddMonster(this);
				var hpValue = this.Properties.GetFloat(PropertyName.HP);
				Send.ZC_OBJECT_PROPERTY(this.Owner.Connection, this);
				Send.ZC_NORMAL.PetAssociateWorldId(this.Owner.Connection, this, (int)this.DbId);
				Send.ZC_NORMAL.PetPlayAnimation(this.Owner.Connection, this);
				Send.ZC_MOVE_SPEED(this);
				Send.ZC_PET_AUTO_ATK(this.Owner, this);
				Send.ZC_NORMAL.PetInfo(this.Owner);
				// Note: PvP/duel relation handling is done in HandleAppearingMonsters

				if (this.Owner.Variables.Perm.GetBool("Melia.WasRidingOnWarp"))
				{
					this.Owner.Variables.Perm.Remove("Melia.WasRidingOnWarp");
					if (!this.IsDead && !this.IsBird)
						this.PendingMount = true;
				}
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
			var maxLevel = ZoneServer.Instance.Conf.World.MaxCompanionLevel;

			// Consume EXP as many times as possible to reach new levels
			while (maxExp > 0 && this.Exp >= maxExp && level < maxLevel)
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

			// Use the Level setter so both PropertyName.Lv and the Mob's
			// cached level field stay in sync. Calling Properties.Modify
			// directly bypasses the setter and leaves _cachedLevel stale,
			// which corrupts subsequent GiveExp calls and DB saves.
			var newLevel = this.Level + amount;
			this.Level = newLevel;

			this.MaxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(ExpType.Pet, newLevel);
			this.Heal(this.MaxHp, 0);

			Send.ZC_OBJECT_PROPERTY(this.Owner.Connection, this);
			Send.ZC_NORMAL.PetInfo(this.Owner);

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

			if (this.IsBird)
				Melia.Zone.Skills.Helpers.FalconerHawkHelper.ResetHawkState(this);
		}

		/// <summary>
		/// Recalculates and updates HP recovery time properties when combat state changes.
		/// </summary>
		/// <param name="combatEntity"></param>
		/// <param name="attackState"></param>
		private void OnCombatStateChanged(ICombatEntity combatEntity, bool attackState)
		{
			if (this.Map == null)
				return;

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

			if (this.IsBird)
				this.UpdateBirdBehavior(elapsed);
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

		#region Bird Companion Methods

		private const float ShoulderLandDelay = 30f;
		private const float DefaultBirdFlyHeight = 80f;
		private const int PerchIdleLoopIntervalMs = 2500;

		/// <summary>
		/// Whether the bird companion is currently landed on its owner's shoulder.
		/// </summary>
		public bool IsLandedOnShoulder { get; private set; }

		/// <summary>
		/// Whether the bird companion is currently perched on a roost.
		/// </summary>
		public bool IsOnRoost { get; private set; }

		/// <summary>
		/// Whether the bird is currently landed (shoulder or roost).
		/// </summary>
		public bool IsPerched => this.IsLandedOnShoulder || this.IsOnRoost;

		/// <summary>
		/// The roost mob the bird should fly to. Set by the skill handler,
		/// consumed by the AI script which moves the bird and calls LandOnRoost().
		/// </summary>
		public Mob ActiveRoost { get; private set; }

		/// <summary>
		/// Set to true when the still timer fires. The AI script should
		/// move the bird to the owner and call LandOnOwnerShoulder() once close.
		/// </summary>
		public bool WantsToLand { get; private set; }

		/// <summary>
		/// Time the owner has been standing still. Reset on movement.
		/// </summary>
		private TimeSpan _ownerStillTimer;

		/// <summary>
		/// Cached owner moving state to detect transitions.
		/// </summary>
		private bool _wasOwnerMoving;

		/// <summary>
		/// Sets the bird companion's flight height via the movement component.
		/// </summary>
		/// <param name="height">Height above ground. Use 0 to land.</param>
		/// <param name="raiseTime">Time to reach the target height.</param>
		/// <param name="easing">Easing factor for the height transition.</param>
		public void SetFlyHeight(float height, float raiseTime = 1f, float easing = 1.5f)
		{
			if (this.Components.TryGet<MovementComponent>(out var movement))
				movement.NotifyFlying(true, height, raiseTime, easing);
		}

		/// <summary>
		/// Configures the fly option flags for this bird companion.
		/// Should be called after takeoff or spawn to ensure proper
		/// client-side flight rendering.
		/// </summary>
		public void SetFlyOption()
		{
			Send.ZC_FLY_OPTION(this, true, true, true, false);
		}

		/// <summary>
		/// Updates the bird companion's shoulder landing timer.
		/// Called from Update() every tick.
		/// </summary>
		private void UpdateBirdBehavior(TimeSpan elapsed)
		{
			if (this.Owner == null)
				return;

			// If roost was destroyed, leave it and clear reference
			if (this.IsOnRoost && (this.ActiveRoost == null || this.ActiveRoost.IsDead))
			{
				this.LeaveRoost();
				this.ActiveRoost = null;
			}
			else if (this.ActiveRoost != null && this.ActiveRoost.IsDead)
			{
				this.ActiveRoost = null;
			}

			// Don't run shoulder landing logic while roost is active
			if (this.ActiveRoost != null && !this.ActiveRoost.IsDead)
				return;

			var isOwnerMoving = this.Owner.Components.TryGet<MovementComponent>(out var movement) && movement.IsMoving;

			// Reset shoulder timer when owner is moving or hawk is
			// busy with a skill (e.g. FirstStrike auto-attacks)
			var hawkBusy = this.Vars.Get<bool>("Hawk.UsingSkill", false);

			if (isOwnerMoving || hawkBusy)
			{
				_ownerStillTimer = TimeSpan.Zero;
				this.WantsToLand = false;

				if (this.IsLandedOnShoulder)
					this.TakeOff();
			}
			else
			{
				_ownerStillTimer += elapsed;

				if (!this.IsPerched && !this.WantsToLand && _ownerStillTimer.TotalSeconds >= ShoulderLandDelay)
					this.WantsToLand = true;
			}

			_wasOwnerMoving = isOwnerMoving;
		}

		/// <summary>
		/// Lands the bird companion on its owner's shoulder.
		/// Attaches to the owner's hawk node and enables auto-detach
		/// so the bird lifts off when the owner moves.
		/// </summary>
		public async void LandOnOwnerShoulder()
		{
			if (this.Owner == null || this.IsLandedOnShoulder)
				return;

			this.Direction = this.Owner.Direction;
			Send.ZC_ROTATE(this);

			this.IsLandedOnShoulder = true;
			this.WantsToLand = false;

			Send.ZC_PLAY_ANI(this, "ASTD_TO_SIT", stopOnLastFrame: true, b1: 1);
			Send.ZC_ATTACH_TO_OBJ(this, this.Owner, "Dummy_pet_hawk_R", "None",
				attachSeconds: 1f, attachAnimation: "SIT", preserveCurrentAnim: 1);
			Send.ZC_NORMAL.AddAttachAnimList(this, "SIT_IDLE", "SIT_IDLE2");

			await Task.Delay(1000);

			if (this.Owner == null || !this.IsLandedOnShoulder)
				return;

			if (this.Components.TryGet<MovementComponent>(out var shoulderMovement))
				shoulderMovement.NotifyFlying(false, 0);

			Send.ZC_NORMAL.AutoDetachWhenTargetMove(this, true, "SIT");
			this.Owner.PlayEffectNode("F_smoke109_2", 1, "Dummy_pet_hawk_R");
			this.Owner.PlayEffectNode("F_archer_hawk_fether_sit", 1, "Dummy_pet_hawk_R");

			this.StartPerchIdleLoop();
		}

		/// <summary>
		/// Lifts the bird companion off the owner's shoulder and resumes flying.
		/// </summary>
		/// <param name="flyHeight">Height to fly at after takeoff.</param>
		public void TakeOff(float flyHeight = DefaultBirdFlyHeight)
		{
			if (!this.IsLandedOnShoulder)
				return;

			Send.ZC_NORMAL.AutoDetachWhenTargetMove(this, false, "SIT");
			this.AttachToObject(null, "None", "None", attachSec: 1);
			this.SetFlyHeight(flyHeight);
			this.PlayAnimation("SIT_TO_ASTD");
			this.AttachToObject(null, "None", "None", attachSec: 1);
			this.SetFlyOption();

			this.IsLandedOnShoulder = false;
			_ownerStillTimer = TimeSpan.Zero;
		}

		/// <summary>
		/// Sets the active roost for this bird companion. The AI script
		/// will fly the bird to the roost and call LandOnRoost().
		/// </summary>
		/// <param name="roost">The roost mob, or null to clear.</param>
		public void SetRoost(Mob roost)
		{
			if (this.IsLandedOnShoulder)
				this.TakeOff();
			else if (this.IsOnRoost)
				this.LeaveRoost();

			this.ActiveRoost = roost;
		}

		/// <summary>
		/// Clears the active roost reference.
		/// </summary>
		public void ClearRoost()
		{
			this.ActiveRoost = null;
		}

		/// <summary>
		/// Lands the bird companion on its roost.
		/// Attaches to the roost's hawk node.
		/// </summary>
		public async void LandOnRoost(Mob roost)
		{
			if (roost == null || roost.IsDead || this.IsOnRoost)
				return;

			this.IsOnRoost = true;

			Send.ZC_PLAY_ANI(this, "ASTD_TO_SIT", stopOnLastFrame: true, b1: 1);
			Send.ZC_ATTACH_TO_OBJ(this, roost, "Dummy_hawk", "None",
				attachSeconds: 1f, attachAnimation: "SIT", preserveCurrentAnim: 1);
			Send.ZC_NORMAL.AddAttachAnimList(this, "SIT_IDLE", "SIT_IDLE2");

			await Task.Delay(1000);

			if (roost == null || roost.IsDead || !this.IsOnRoost)
				return;

			if (this.Components.TryGet<MovementComponent>(out var roostMovement))
				roostMovement.NotifyFlying(false, 0);

			Send.ZC_NORMAL.AutoDetachWhenTargetMove(this, true, "SIT");
			roost.PlayEffectNode("F_smoke109_2", 1, "Dummy_hawk");
			roost.PlayEffectNode("F_archer_hawk_fether_sit", 1, "Dummy_hawk");

			this.StartPerchIdleLoop();
		}

		/// <summary>
		/// Periodically re-sends the attach idle animation list so the bird
		/// keeps cycling SIT_IDLE animations while perched instead of freezing.
		/// </summary>
		private async void StartPerchIdleLoop()
		{
			while (this.IsPerched)
			{
				await Task.Delay(PerchIdleLoopIntervalMs);

				if (!this.IsPerched)
					break;

				Send.ZC_NORMAL.AddAttachAnimList(this, "SIT_IDLE");
			}
		}

		/// <summary>
		/// Lifts the bird companion off the roost and resumes flying.
		/// </summary>
		public void LeaveRoost(float flyHeight = DefaultBirdFlyHeight)
		{
			if (!this.IsOnRoost)
				return;

			Send.ZC_NORMAL.AutoDetachWhenTargetMove(this, false, "SIT");
			this.AttachToObject(null, "None", "None", attachSec: 1);
			this.SetFlyHeight(flyHeight);
			this.PlayAnimation("SIT_TO_ASTD");
			this.AttachToObject(null, "None", "None", attachSec: 1);
			this.SetFlyOption();

			this.IsOnRoost = false;
		}

		#endregion
	}
}
