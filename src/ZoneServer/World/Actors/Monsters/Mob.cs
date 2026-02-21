using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.Network;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Buffs.Handlers.Common;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Items.Effects;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Monsters
{
	/// <summary>
	/// An actual monster.
	/// </summary>
	public partial class Mob : Actor, IMonster, ICombatEntity, IUpdateable
	{
		private readonly object _hpLock = new();
		private Position _position;

		/// <summary>
		/// Returns the monster's position on its current map.
		/// Setting this property automatically updates the spatial index.
		/// </summary>
		public override Position Position
		{
			get => _position;
			set
			{
				var oldPosition = _position;
				_position = value;
				this.Map?.UpdateEntitySpatialPosition(this, oldPosition, value);
			}
		}

		/// <summary>
		/// Returns the monster's race.
		/// </summary>
		public RaceType Race => this.Data.Race;

		/// <summary>
		/// Returns the monster's element/attribute.
		/// </summary>
		public AttributeType Attribute => (AttributeType)(int)this.Properties.GetFloat(PropertyName.Attribute, (int)AttributeType.None);

		/// <summary>
		/// Returns the monster's armor material.
		/// </summary>
		public ArmorMaterialType ArmorMaterial => (ArmorMaterialType)(int)this.Properties.GetFloat(PropertyName.ArmorMaterial, (int)ArmorMaterialType.None);

		/// <summary>
		/// Returns the monster's mode of movement.
		/// </summary>
		public MoveType MoveType { get; set; }

		/// <summary>
		/// Gets or sets the monster's tendency
		/// </summary>
		public TendencyType Tendency { get; set; } = TendencyType.Peaceful;

		/// <summary>
		/// Monster ID in database.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Monster Class Name in database.
		/// </summary>
		public string ClassName => this.Data.ClassName ?? null;

		/// <summary>
		/// ?
		/// </summary>
		/// <remarks>
		/// Used by the anchors in the client files, with multiple anchors
		/// being able to use the same "gen type". This is also used to
		/// identify NPCs however, like in ZC_SET_NPC_STATE.
		/// </remarks>
		public int GenType { get; set; }
			// The client files set the gen type manually, but that seems
			// bothersome. For now, we'll generate them automatically and
			// see for what purpose we would need them to be the same for
			// multiple monsters.
			= ZoneServer.Instance.World.CreateGenType();

		/// <summary>
		/// Gets or sets what kind of "monster" the mob is.
		/// </summary>
		public RelationType MonsterType { get; set; } = RelationType.Enemy;

		/// <summary>
		/// Gets or sets monster's name, leave empty for default.
		/// </summary>
		public override string Name { get; set; }

		/// <summary>
		/// Gets or sets monster's unique name.
		/// </summary>
		/// <remarks>
		/// Purpose currently unknown.
		/// </remarks>
		public string UniqueName { get; set; }

		/// <summary>
		/// Gets or sets the function to call when the monster is clicked.
		/// </summary>
		/// <remarks>
		/// Not actively used in Melia, but important to the client,
		/// as it's used to determine whether the monster is clickable
		/// and should display an HP bar.
		/// </remarks>
		public string DialogName { get; set; }

		/// <summary>
		/// Gets or sets the name of the function to call when the monster's
		/// trigger area is entered.
		/// </summary>
		/// <remarks>
		/// Not actively used in Melia, but potentially importat to the
		/// client.
		/// </remarks>
		public string EnterName { get; set; }

		/// <summary>
		/// Gets or sets the name of the function to call when the monster's
		/// trigger area is left.
		/// </summary>
		/// <remarks>
		/// Not actively used in Melia, but potentially importat to the
		/// client.
		/// </remarks>
		public string LeaveName { get; set; }

		/// <summary>
		/// Gets or sets the mob's level.
		/// </summary>
		public int Level
		{
			get { return (int)this.Properties.GetFloat(PropertyName.Lv); }
			set { this.Properties.SetFloat(PropertyName.Lv, value); }
		}

		/// <summary>
		/// Gets or sets the mob's AoE Defense Ratio.
		/// </summary>
		public float SDR => this.Properties.GetFloat(PropertyName.SDR);

		/// <summary>
		/// Returns the mob's current HP.
		/// </summary>
		public int Hp => (int)this.Properties.GetFloat(PropertyName.HP);

		/// <summary>
		/// Returns the mob's maximum HP.
		/// </summary>
		public int MaxHp => (int)this.Properties.GetFloat(PropertyName.MHP);

		/// <summary>
		/// Raised when the monster died.
		/// </summary>
		public event Action<Mob, ICombatEntity> Died;

		/// <summary>
		/// Data entry for this monster.
		/// </summary>
		public MonsterData Data { get; private set; }

		/// <summary>
		/// List of items that are dropped in addition to defined monster drops.
		/// </summary>
		public List<DropData> FixedDrops { get; } = new();

		/// <summary>
		/// Returns a list of fixed items the monster drops as is when it dies.
		/// </summary>
		public ConcurrentBag<Item> StaticDrops { get; } = new ConcurrentBag<Item>();

		/// <summary>
		/// Returns whether the monster is dead.
		/// </summary>
		public virtual bool IsDead => this.Hp == 0;

		/// <summary>
		/// Gets or sets whether the monster appears from inside the ground.
		/// </summary>
		/// <remarks>
		/// If this property is true, the monster will dig itself out of the
		/// ground when it appears. 
		/// </remarks>
		public bool FromGround { get; set; }

		/// <summary>
		/// Holds the order of successive changes in mob HP.
		/// A higher value indicates the latest HP amount.
		/// </summary>
		public int HpChangeCounter { get; protected set; }

		/// <summary>
		/// Gets or sets whether the monster is a prop.
		/// </summary>
		public bool IsProp { get; set; }

		/// <summary>
		/// Show adventure book entry notification.
		/// </summary>
		public bool Journal { get; set; }

		/// <summary>
		/// Returns if the monster can give exp or not.
		/// </summary>
		public bool HasExp { get; set; } = true;

		/// <summary>
		/// Returns if the monster can drop items or not.
		/// </summary>
		public bool HasDrops { get; set; } = true;

		/// <summary>
		/// Custom drop list
		/// </summary>
		public string DropList { get; set; } = null;

		/// <summary>
		/// If set, a spawn location is associated with this monster.
		/// </summary>
		public Position SpawnPosition { get; set; } = Position.Zero;

		/// <summary>
		/// Returns the monster's property collection.
		/// </summary>
		public MonsterProperties Properties { get; protected set; }

		/// <summary>
		/// Returns the monster's property collection.
		/// </summary>
		Properties IPropertyHolder.Properties => this.Properties;

		/// <summary>
		/// Returns the monster's property collection.
		/// </summary>
		Properties IMonster.Properties => this.Properties;

		/// <summary>
		/// Monster's buffs.
		/// </summary>
		public BuffComponent Buffs { get; }

		/// <summary>
		/// Monster's effects.
		/// </summary>
		public EffectsComponent Effects { get; }

		/// <summary>
		/// Monster's combat state.
		/// </summary>
		public CombatComponent CombatState { get; }

		/// <summary>
		/// Return the monster's temporary variables.
		/// </summary>
		public Variables Vars { get; } = new Variables();

		/// <summary>
		/// Returns the monster's effective size type, read from its
		/// properties or falling back to the data definition.
		/// </summary>
		public SizeType EffectiveSize => Enum.Parse<SizeType>(this.Properties.GetString(PropertyName.Size, this.Data.Size));

		/// <summary>
		/// Gets or sets the monster's rank (e.g. Normal, Elite, Boss).
		/// </summary>
		public MonsterRank Rank
		{
			get
			{
				return Enum.Parse<MonsterRank>(this.Properties.GetString(PropertyName.MonRank, this.Data.Rank));
			}
			set
			{
				this.Properties.SetString(PropertyName.MonRank, value);
			}
		}

		/// <summary>
		/// Gets or sets the monster's current shield value.
		/// </summary>
		public int Shield
		{
			get
			{
				return (int)this.Properties.GetFloat(PropertyName.Shield);
			}
			set
			{
				this.Properties.SetFloat(PropertyName.Shield, value);
			}
		}

		/// <summary>
		/// Returns the monster's maximum shield value.
		/// </summary>
		public int MaxShield
		{
			get
			{
				return (int)this.Properties.GetFloat(PropertyName.MShield);
			}
		}

		/// <summary>
		/// Gets or sets if this mob is a special GTW objective (Amplifier, Boss).
		/// </summary>
		public bool IsGtwObjective { get; set; } = false;

		/// <summary>
		/// Creates a new monster instance as a clone of an existing monster.
		/// </summary>
		/// <param name="originalMonster">The monster to clone.</param>
		/// <param name="position">The position where the clone should be created.</param>
		/// <returns>A new Mob instance, or null if the original was invalid.</returns>
		public static Mob CopyFrom(Mob originalMonster, Position position)
		{
			if (originalMonster == null || originalMonster.Data == null)
			{
				Log.Warning("Mob.CreateFrom: Attempted to clone a null or invalid monster.");
				return null;
			}

			// Create a new mob using the same ID and type. This handles loading
			// the base data and initializing default properties.
			var clone = new Mob(originalMonster.Id, originalMonster.MonsterType);

			// Copy properties from the original monster. This will transfer the values
			// of simple properties (FloatProperty, StringProperty), but not buffs or
			// complex calculated states.
			clone.Properties.CopyFrom(originalMonster.Properties);

			// Set the clone's position and level to match the original.
			clone.Position = position;
			clone.Level = originalMonster.Level;

			// Invalidate all calculated properties to ensure they are recalculated
			// based on the newly copied base values and the updated level.
			clone.Properties.InvalidateAll();

			// The clone should start with full HP and SP, as CopyFrom would have copied
			// the original's current (possibly zero) HP.
			clone.Properties.SetFloat(PropertyName.HP, clone.Properties.GetFloat(PropertyName.MHP));
			clone.Properties.SetFloat(PropertyName.SP, clone.Properties.GetFloat(PropertyName.MSP));

			return clone;
		}

		/// <summary>
		/// Creates new NPC.
		/// </summary>
		public Mob(int id, RelationType type) : base()
		{
			this.Id = id;
			this.MonsterType = type;

			this.Components.Add(this.Buffs = new BuffComponent(this));
			this.Components.Add(this.CombatState = new CombatComponent(this));
			this.Components.Add(new StateLockComponent(this));
			this.Components.Add(new CooldownComponent(this));
			this.Components.Add(new EffectsComponent(this));
			this.Components.Add(new RecoveryComponent(this));
			this.Components.Add(new BaseSkillComponent(this));

			this.LoadData();

			this.Name = this.Data.Name;
			this.MoveType = this.Data.MoveType;

			if (this.Rank == MonsterRank.Boss)
			{
				this.Properties.SetFloat(PropertyName.ShieldRate, 100);
				this.Shield = this.MaxShield;
				this.Properties.AutoUpdateMax(PropertyName.Shield, PropertyName.MShield);
			}
			else if ((this.Rank == MonsterRank.MISC) || (this.Rank == MonsterRank.Material) || (this.Rank == MonsterRank.NPC))
			{
				this.HasDrops = false;
				this.HasExp = false;
			}
		}

		/// <summary>
		/// Loads data from data files.
		/// </summary>
		protected void LoadData()
		{
			if (this.Id == 0)
				throw new InvalidOperationException("Id wasn't set before calling LoadData.");

			this.Data = ZoneServer.Instance.Data.MonsterDb.Find(this.Id);
			if (this.Data == null)
				throw new NullReferenceException("No data found for '" + this.Id + "'.");

			this.Faction = this.Data.Faction;
			this.DialogName = this.Data.Dialog;
			this.Journal = !string.IsNullOrEmpty(this.Data.Journal);

			this.InitProperties();
		}

		/// <summary>
		/// Initializes monster's properties.
		/// </summary>
		private void InitProperties()
		{
			this.Properties = new MonsterProperties(this);

			this.Properties.AddDefaultProperties();
			this.Properties.InitAutoUpdates();
			this.Properties.InvalidateAll();

			this.Properties.SetFloat(PropertyName.HP, this.Properties.GetFloat(PropertyName.MHP));
			this.Properties.SetFloat(PropertyName.SP, this.Properties.GetFloat(PropertyName.MSP));
		}

		/// <summary>
		/// Makes monster take damage and kills it if the HP reach 0.
		/// Returns true if the monster is dead.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="attacker"></param>
		/// <returns></returns>
		public bool TakeDamage(float damage, ICombatEntity attacker)
		{
			// Don't hit an already dead monster
			if (this.IsDead)
				return true;

			if (this.IsSafe())
				return false;

			if (this.IsBuffActive(BuffId.Skill_NoDamage_Buff))
				return false;

			// Interrupt casting when taking damage (force interrupt for monsters)
			if (damage > 0 && this.IsCasting())
			{
				this.Components.Get<CombatComponent>().InterruptCasting();
				Send.MonsterSkillBalloonCancel(this);
			}

			// Apply damage to shield, then handle stagger and HP damage.
			damage = this.ApplyToShield(damage);

			// Increase damage if the mob is staggered.
			if (this.IsStaggered())
			{
				// Apply a damage multiplier while staggered (e.g., 1.5x).
				damage *= 1.5f;
			}

			var currentHp = this.Hp;

			this.Components.Get<CombatComponent>().SetAttackState(true);
			this.ModifyHpSafe(-damage, out _, out _);

			// Register hits before potentially killing the monster,
			// so the damage can be factored into finding the top
			// attacker.
			if (attacker != null)
				this.Components.Get<CombatComponent>()?.RegisterHit(attacker, damage);

			if (this.Hp == 0)
				this.Kill(attacker);

			if (attacker != null)
				this.Map.AlertNearbyAis(this, new HitEventAlert(this, attacker, damage));

			return this.IsDead;
		}

		/// <summary>
		/// Applies damage to the monster's shield at 5x rate and returns
		/// any remaining damage that passes through to HP.
		/// </summary>
		private float ApplyToShield(float damage)
		{
			if (this.Shield > 0)
			{
				// Shield takes 5x damage
				var shieldDamage = damage * 5;
				var shieldBreak = (this.Shield - shieldDamage) < 0;

				if (shieldBreak)
				{
					var remainingShieldHealth = this.Shield;
					this.Shield = 0;

					if (!this.CanStagger())
						damage -= remainingShieldHealth / 5;
					else
						this.ApplyStagger();

					Send.ZC_UPDATE_SHIELD(this, this.Shield, 1);
				}
				else
				{
					this.Shield -= (int)shieldDamage;
					Send.ZC_UPDATE_SHIELD(this, this.Shield, 0);

					if (!this.CanStagger())
						return 0;
				}
			}

			return damage;
		}

		/// <summary>
		/// Kills monster.
		/// </summary>
		/// <param name="killer"></param>
		public virtual void Kill(ICombatEntity killer)
		{
			Send.ZC_SKILL_CAST_CANCEL(this);
			Send.ZC_SKILL_DISABLE(this);
			Send.ZC_DEAD(this);

			this.Components.Get<BaseSkillComponent>()?.CancelCurrentSkill();

			this.Properties.SetFloat(PropertyName.HP, 0);
			this.Components.Get<MovementComponent>()?.Stop();
			this.DisappearTime = DateTime.Now.AddSeconds(2);
			if (this.Effects?.Count != 0)
				Send.ZC_NORMAL.ClearEffects(this);

			var beneficiary = this.GetKillBeneficiary(killer);

			if (this.MonsterType == RelationType.Enemy && beneficiary != null)
			{
				this.GetExpToGive(out var exp, out var jobExp);

				this.DropItems(beneficiary);

				var SCR_Get_MON_ExpPenalty = ScriptableFunctions.MonsterCharacter.Get("GET_EXP_RATIO");
				var SCR_Get_MON_ClassExpPenalty = ScriptableFunctions.MonsterCharacter.Get("GET_EXP_RATIO");

				var expMultiplier = SCR_Get_MON_ExpPenalty(this, beneficiary);
				var jobExpMultiplier = SCR_Get_MON_ClassExpPenalty(this, beneficiary);
				exp = (long)(exp * expMultiplier);
				jobExp = (long)(jobExp * jobExpMultiplier);

				if (beneficiary.Connection.Party != null)
					beneficiary.Connection.Party.GiveExp(beneficiary, exp, jobExp, this);
				else
					beneficiary?.GiveExp(exp, jobExp, this);
			}

			this.Died?.Invoke(this, killer);
			ZoneServer.Instance.ServerEvents.EntityKilled.Raise(new CombatEventArgs(this, killer));
			this.Buffs?.RemoveAll();

			// Trigger Kill card effects
			if (beneficiary != null)
				ItemHookRegistry.Instance.InvokeKillHooks(beneficiary, this);

			if (beneficiary != null && this.Data != null && this.Journal)
			{
				if (ZoneServer.Instance.Data.MonsterDb.TryFind(this.Data.Journal, out var journalMonster))
				{
					if (beneficiary.AdventureBook.IsNewEntry(AdventureBookType.MonsterKilled, journalMonster.Id))
						beneficiary.AddonMessage(AddonMessage.ADVENTURE_BOOK_NEW, journalMonster.Name);
					beneficiary.AdventureBook.AddMonsterKill(journalMonster.Id, 1);
				}
			}

			if (beneficiary is Character character && Feature.IsEnabled("BattleManager"))
				ZoneServer.Instance.World.BattleManager.EndBattle(character);
		}

		/// <summary>
		/// Returns the character that benefits from the kill of the mob
		/// in form of EXP and drops.
		/// </summary>
		/// <param name="killer"></param>
		/// <returns></returns>
		private Character GetKillBeneficiary(ICombatEntity killer)
		{
			if (killer == null)
				return null;
			var beneficiary = killer;

			var topAttacker = this.Components.Get<CombatComponent>()?.GetTopAttackerByDamage();
			if (topAttacker != null)
				beneficiary = topAttacker;

			if (beneficiary.Components.Get<AiComponent>()?.Script.GetMaster() is Character master)
				beneficiary = master;

			return beneficiary as Character;
		}

		/// <summary>
		/// Returns the EXP to give to the beneficiary of killing the mob
		/// via out.
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="jobExp"></param>
		private void GetExpToGive(out long exp, out long jobExp)
		{
			if (!this.HasExp)
			{
				exp = 0;
				jobExp = 0;
				return;
			}

			var worldConf = ZoneServer.Instance.Conf.World;

			var expRate = worldConf.ExpRate / 100.0;
			var jobExpRate = worldConf.JobExpRate / 100.0;

			if (this.IsBuffActive(BuffId.SuperExp))
			{
				expRate *= worldConf.BlueJackpotExpRate / 100.0;
				jobExpRate *= worldConf.BlueJackpotExpRate / 100.0;
			}
			if (this.IsBuffActive(BuffId.EliteMonsterBuff))
			{
				expRate *= worldConf.EliteExpRate / 100.0;
				jobExpRate *= worldConf.EliteExpRate / 100.0;
			}
			if (this.Rank == MonsterRank.Boss)
			{
				expRate *= worldConf.BossExpRate / 100.0;
				jobExpRate *= worldConf.BossExpRate / 100.0;
			}
			if (this.Map != null && ZoneServer.Instance.Data.InstanceDungeonDb.TryGetByMapClassName(this.Map.ClassName, out _))
			{
				expRate *= worldConf.InstancedDungeonExpRate / 100.0;
				jobExpRate *= worldConf.InstancedDungeonExpRate / 100.0;
			}

			exp = 0L;
			jobExp = 0L;

			var propExpRate = this.Properties.GetFloat(PropertyName.EXP_Rate, 1f);
			var propClassExpRate = this.Properties.GetFloat(PropertyName.JEXP_Rate, 1f);

			if (this.Data.Exp > 0)
				exp = (long)Math.Max(1, this.Data.Exp * expRate * propExpRate);
			if (this.Data.JobExp > 0)
				jobExp = (long)Math.Max(1, this.Data.JobExp * jobExpRate * propClassExpRate);
		}

		/// <summary>
		/// Calculates the drop chance rate for the given item based
		/// on its own property, the server's configuration, as well
		/// as other factors.
		/// </summary>
		/// <param name="dropEntry">The drop data to get the adjusted drop rate for.</param>
		/// <returns></returns>
		public static float GetAdjustedDropRate(DropData dropEntry)
		{
			var itemData = ZoneServer.Instance.Data.ItemDb.Find(dropEntry.ItemId);

			// Don't drop items that don't exist in the database
			if (itemData == null)
				return -1;

			var worldConf = ZoneServer.Instance.Conf.World;
			var dropChance = dropEntry.DropChance;

			if (itemData.Id == ItemId.Silver || itemData.Id == ItemId.Gold)
			{
				dropChance *= worldConf.SilverDropRate / 100f;
			}
			else if ((itemData.Type == ItemType.Equip || itemData.Type == ItemType.PetArmor || itemData.Type == ItemType.PetWeapon) && itemData.EquipType1 != EquipType.Hat)
			{
				dropChance *= worldConf.EquipmentDropRate / 100f;
			}
			else if (itemData.ClassName.StartsWith("BlueOrb_"))
			{
				dropChance *= worldConf.BlueOrbDropRate / 100f;
			}
			else if (itemData.ClassName.StartsWith("RedOrb_"))
			{
				dropChance *= worldConf.RedOrbDropRate / 100f;
			}
			else if (itemData.Group == ItemGroup.Gem)
			{
				dropChance *= worldConf.GemDropRate / 100f;
			}
			else if (itemData.Group == ItemGroup.Recipe)
			{
				dropChance *= worldConf.RecipeDropRate / 100f;
			}
			else
			{
				dropChance *= worldConf.GeneralDropRate / 100f;
			}

			return dropChance;
		}

		/// <summary>
		/// Drops random items from the monster's drop table.
		/// </summary>
		/// <param name="killer"></param>
		private void DropItems(Character killer)
		{
			if (!this.HasDrops)
				return;

			if (this.Data.Drops == null)
				return;

			// Normal
			var drops = this.GenerateDropStacks(killer);
			this.DropStacks(killer, drops);

			// Event - Removed: GlobalBonusManager was in deleted GameEvents namespace
			// var eventDrops = this.GenerateDropStacks(killer, ZoneServer.Instance.GameEvents.GlobalBonuses.GetDrops(this, killer));
			// if (eventDrops.Count != 0)
			// {
			// 	this.DropStacks(killer, eventDrops);
			// }

			// Fixed drops
			if (this.FixedDrops != null && this.FixedDrops.Count > 0)
			{
				var fixedDrops = this.GenerateDropStacks(killer, this.FixedDrops);
				this.DropStacks(killer, fixedDrops);
			}

			// Global drops
			if (killer != null)
			{
				var globalDrops = this.GetGlobalDropStacks(killer);
				this.DropStacks(killer, globalDrops);
			}
			this.DropStatic(killer);

			// Map bonus drops
			var mapBonusRerolls = 1;
			if (this.TryGetSuperMob(out var superMobType))
			{
				var worldConf = ZoneServer.Instance.Conf.World;
				mapBonusRerolls = superMobType switch
				{
					SuperMobType.Silver => worldConf.SilverJackpotRolls,
					SuperMobType.Gold => worldConf.GoldJackpotRolls,
					SuperMobType.Elite => worldConf.EliteRolls,
					_ => 1
				};
			}
			this.DropMapBonusItems(killer, mapBonusRerolls);
		}

		/// <summary>
		/// Generates a list of random items to drop from the monster's
		/// drop table.
		/// </summary>
		/// <param name="killer"></param>
		/// <returns></returns>
		private List<DropStack> GenerateDropStacks(Character killer)
		{
			return GenerateDropStacks(killer, this.Data.Drops);
		}

		/// <summary>
		/// Generates a list of random items to drop from the monster's
		/// drop table.
		/// </summary>
		/// <param name="killer"></param>
		/// <returns></returns>
		private List<DropStack> GenerateDropStacks(Character killer, List<DropData> drops)
		{
			var result = new List<DropStack>();
			var rnd = RandomProvider.Get();

			foreach (var dropItemData in drops)
			{
				if (!ZoneServer.Instance.Data.ItemDb.TryFind(dropItemData.ItemId, out var itemData))
				{
					Log.Warning("Monster.DropItems: Drop item '{0}' not found.", dropItemData.ItemId);
					continue;
				}

				var originalDropChance = dropItemData.DropChance;
				var adjustedDropChance = GetAdjustedDropRate(dropItemData);

				// Each point of looting chance increases drop rate by 0.1%,
				// so 500 looting chance = 1.5x drop rate.
				var lootingChance = killer.Properties.GetFloat(PropertyName.LootingChance);
				var lootingRate = 1f + lootingChance * 0.001f;
				adjustedDropChance *= lootingRate;

				// Calculate Enhanced drops for super mobs
				var isSuperMob = this.TryGetSuperMob(out var superMobType);
				var superMobRerolls = 0;
				var superMobGuaranteedItemDrop = false;
				var superMobMoneyMultiplier = 0f;
				var superMobMoneyStacks = 0;
				if (isSuperMob)
				{
					var worldConf = ZoneServer.Instance.Conf.World;
					switch (superMobType)
					{
						case SuperMobType.Silver:
							superMobRerolls = worldConf.SilverJackpotRolls;
							superMobGuaranteedItemDrop = originalDropChance > worldConf.SilverJackpotGuaranteedItemThreshold;
							superMobMoneyMultiplier = worldConf.SilverJackpotRolls / 20f;
							superMobMoneyStacks = rnd.Next(40, 50);
							break;

						case SuperMobType.Gold:
							superMobRerolls = worldConf.GoldJackpotRolls;
							superMobGuaranteedItemDrop = originalDropChance > worldConf.GoldJackpotGuaranteedItemThreshold;
							superMobMoneyMultiplier = worldConf.GoldJackpotRolls / 20f;
							superMobMoneyStacks = rnd.Next(40, 50);
							break;

						case SuperMobType.Elite:
							superMobRerolls = worldConf.EliteRolls;
							superMobGuaranteedItemDrop = originalDropChance > worldConf.EliteGuaranteedItemThreshold;
							superMobMoneyMultiplier = worldConf.EliteRolls / 20f;
							superMobMoneyStacks = rnd.Next(40, 50);
							break;
					}
				}

				if (this.Rank == MonsterRank.Boss)
				{
					var worldConf = ZoneServer.Instance.Conf.World;
					superMobRerolls = worldConf.BossRolls;
					superMobGuaranteedItemDrop = originalDropChance > worldConf.BossGuaranteedItemThreshold;
					superMobMoneyMultiplier = worldConf.BossRolls / 20f;
					superMobMoneyStacks = rnd.Next(40, 50);
				}

				var isMoney = itemData.Id == ItemId.Silver || itemData.Id == ItemId.Gold;
				var minAmount = dropItemData.MinAmount;
				var maxAmount = dropItemData.MaxAmount;
				var stackCount = 1;

				if (isMoney)
				{
					minAmount = Math.Max(1, (int)(minAmount * (ZoneServer.Instance.Conf.World.SilverDropAmount / 100f)));
					maxAmount = Math.Max(minAmount, (int)(maxAmount * (ZoneServer.Instance.Conf.World.SilverDropAmount / 100f)));

					// Increased number of stacks and items per stack for
					// super mobs or bosses
					if ((isSuperMob) || this.Rank == MonsterRank.Boss)
					{
						minAmount = (int)Math.Round(minAmount * superMobMoneyMultiplier);
						maxAmount = (int)Math.Round(maxAmount * superMobMoneyMultiplier);
						stackCount += superMobMoneyStacks;
					}
				}

				var itemId = dropItemData.ItemId;
				var amount = rnd.Next(minAmount, maxAmount + 1);
				var rerolls = superMobRerolls;
				var guaranteed = superMobGuaranteedItemDrop;
				do
				{
					rerolls--;

					// Items above the given threshold will
					// always drop at least once for super mobs.
					var dropSuccess = rnd.NextDouble() < adjustedDropChance / 100f;
					if (!dropSuccess && !guaranteed)
						continue;

					for (var i = 0; i < stackCount; ++i)
					{
						var dropStack = new DropStack(itemId, amount, originalDropChance, adjustedDropChance);
						result.Add(dropStack);
					}

					guaranteed = false;

					// Some items cannot be rerolled
					if (itemData.Group == ItemGroup.Card)
						break;
				}
				while (rerolls > 0 && !isMoney);
			}

			return result;
		}

		/// <summary>
		/// Checks if the given entity is a super mob
		/// </summary>
		/// <remarks>
		/// The super drop level affects the drop chance and drop rate
		/// of items. Level 1 increases the chance and even elevates
		/// some items to 100%, while level 2 increases the chance
		/// even more.
		/// </remarks>
		/// <returns></returns>
		private bool TryGetSuperMob(out SuperMobType superMobType)
		{
			superMobType = (SuperMobType)(-1);

			// Note: The client cannot handle SuperDrop and EliteMonsterBuff
			// together.
			if (this.Buffs.Has(BuffId.EliteMonsterBuff))
			{
				superMobType = SuperMobType.Elite;
				return true;
			}
			if (this.Buffs.TryGet(BuffId.SuperDrop, out var buff))
			{
				if (buff.NumArg2 == 0)
				{
					superMobType = SuperMobType.Silver;
					return true;
				}
				else if (buff.NumArg2 == 1)
				{
					superMobType = SuperMobType.Gold;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Drops a previously generated item stack.
		/// </summary>
		/// <param name="killer"></param>
		/// <param name="dropStacks"></param>
		private void DropStacks(Character killer, List<DropStack> dropStacks)
		{
			foreach (var stack in dropStacks)
			{
				this.DropItem(killer, stack.ItemId, stack.Amount, stack.DropChance);
			}
		}

		/// <summary>
		/// Drops a single item.
		/// The dropped item may go to killer, killer's party, or
		/// simply stay on ground.
		/// </summary>
		/// <remarks>
		/// Drop chance is used only to calculate if item goes directly
		/// to owner during autoloot. This method does NOT calculate
		/// if an item should drop or not.
		/// </remarks>
		/// <param name="killer"></param>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <param name="dropChance"></param>
		public void DropItem(Character killer, int itemId, int amount, float dropChance)
		{
			var rnd = RandomProvider.Get();

			var dropItem = new Item(itemId, amount);

			if (dropItem.Data.Type == ItemType.Equip)
			{
				var lootingChance = killer?.Properties.GetFloat(PropertyName.LootingChance, 1) ?? 1;
				if (lootingChance <= 0)
					lootingChance = 1;
				var grade = this.DetermineItemGrade(lootingChance, dropItem);
				this.ApplyItemGrade(dropItem, grade);
			}

			var autolootThreshold = killer?.Variables.Perm.Get("Melia.Autoloot", 0);
			var autoloot = dropChance <= autolootThreshold;

			var direction = new Direction(rnd.Next(0, 360));
			var dropRadius = ZoneServer.Instance.Conf.World.DropRadius;
			var distance = rnd.Next(dropRadius / 2, dropRadius + 1);

			// Can't find killer, drop item on ground without owner
			if (killer == null)
			{
				dropItem.SetLootProtection(killer, TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.LootPrectionSeconds));
				dropItem.Drop(this.Map, this.Position, direction, distance, this.Level, killer?.Layer ?? 0);
				return;
			}

			// Check if killer has party
			var killersParty = killer.Connection?.Party;
			if (killersParty != null)
			{
				if (killersParty.TryGetItemRecipient(killer, out var recipient))
				{
					if (autoloot || Versions.Protocol < 200)
					{
						recipient.Inventory.Add(dropItem, InventoryAddType.PickUp);

						// TODO:
						// Items on autoloot are not adding to adventure book.
					}
					else
					{
						dropItem.SetLootProtection(recipient, TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.LootPrectionSeconds));
						var itemMonster = dropItem.Drop(this.Map, this.Position, direction, distance, recipient?.AccountObjectId ?? 0, recipient?.Layer ?? 0);

						// Prevent global drop items to be added to adventure
						// book
						if (itemMonster.Item.Data.Type == ItemType.Equip || itemMonster.Item.Data.Type == ItemType.Recipe || itemMonster.Item.Data.Type == ItemType.Consume)
							return;

						// Add to adventure book on item pickup
						if (this.Journal && ZoneServer.Instance.Data.MonsterDb.TryFind(this.Data.Journal, out var journalMonster))
							itemMonster.MonsterId = journalMonster.Id;
					}
				}
			}
			// Killer doesn't have party
			else
			{
				if (autoloot || Versions.Protocol < 200)
				{
					killer.Inventory.Add(dropItem, InventoryAddType.PickUp);

					// TODO:
					// Items on autoloot are not adding to adventure book.
				}
				else
				{
					dropItem.SetLootProtection(killer, TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.LootPrectionSeconds));
					var itemMonster = dropItem.Drop(this.Map, this.Position, direction, distance, killer?.AccountObjectId ?? 0, killer?.Layer ?? 0);

					// Prevent global drop items to be added to adventure
					// book
					if (itemMonster.Item.Data.Type == ItemType.Equip || itemMonster.Item.Data.Type == ItemType.Recipe || itemMonster.Item.Data.Type == ItemType.Consume)
						return;

					// Add to adventure book on item pickup
					if (this.Journal && ZoneServer.Instance.Data.MonsterDb.TryFind(this.Data.Journal, out var journalMonster))
						itemMonster.MonsterId = journalMonster.Id;
				}
			}
		}

		/// <summary>
		/// Drops the monster's static drops if any were added.
		/// </summary>
		/// <param name="killer"></param>
		private void DropStatic(Character killer)
		{
			var rnd = RandomProvider.Get();

			if (this.StaticDrops.IsEmpty)
				return;

			while (this.StaticDrops.TryTake(out var dropItem))
			{
				this.DropItem(killer, dropItem.Id, dropItem.Amount, 100f);
			}
		}

		/// <summary>
		/// Drops bonus items configured for the current map.
		/// Only applies to non-Boss and non-MISC rank monsters.
		/// Each item in the map's bonus drop list rolls independently.
		/// </summary>
		/// <param name="killer"></param>
		/// <param name="rerolls">Number of times to roll for drops (jackpot/elite mobs get more rolls)</param>
		private void DropMapBonusItems(Character killer, int rerolls = 1)
		{
			// Skip Boss and MISC rank monsters
			if (this.Rank == MonsterRank.Boss ||
				this.Rank == MonsterRank.MISC ||
				this.Rank == MonsterRank.Material ||
				this.Rank == MonsterRank.NPC)
				return;

			// Get the map class name
			var mapClassName = this.Map?.Data?.ClassName;
			if (string.IsNullOrEmpty(mapClassName))
				return;

			// Check if this map has bonus drops configured
			if (!ZoneServer.Instance.Data.MapBonusDropsDb.TryFind(mapClassName, out var mapBonusData))
				return;

			var rnd = RandomProvider.Get();
			var lootingChance = killer?.Properties.GetFloat(PropertyName.LootingChance) ?? 0;
			var lootingRate = 1f + lootingChance * 0.001f;

			// Roll for each item independently
			foreach (var dropEntry in mapBonusData.Items)
			{
				// Get item data to determine type-specific drop rate modifier
				if (!ZoneServer.Instance.Data.ItemDb.TryFind(dropEntry.ItemId, out var itemData))
					continue;

				// Base chance from config (percentage)
				var dropChance = dropEntry.Chance;

				// Apply world config modifiers based on item type
				var worldConf = ZoneServer.Instance.Conf.World;
				if (itemData.Id == ItemId.Silver || itemData.Id == ItemId.Gold)
				{
					dropChance *= worldConf.SilverDropRate / 100f;
				}
				else if ((itemData.Type == ItemType.Equip || itemData.Type == ItemType.PetArmor || itemData.Type == ItemType.PetWeapon) && itemData.EquipType1 != EquipType.Hat)
				{
					dropChance *= worldConf.EquipmentDropRate / 100f;
				}
				else if (itemData.ClassName.StartsWith("BlueOrb_"))
				{
					dropChance *= worldConf.BlueOrbDropRate / 100f;
				}
				else if (itemData.ClassName.StartsWith("RedOrb_"))
				{
					dropChance *= worldConf.RedOrbDropRate / 100f;
				}
				else if (itemData.Group == ItemGroup.Gem)
				{
					dropChance *= worldConf.GemDropRate / 100f;
				}
				else if (itemData.Group == ItemGroup.Recipe)
				{
					dropChance *= worldConf.RecipeDropRate / 100f;
				}
				else
				{
					dropChance *= worldConf.GeneralDropRate / 100f;
				}

				// Apply looting chance bonus
				dropChance *= lootingRate;

				// Roll for drop (reroll for jackpot/elite mobs)
				var isMoney = itemData.Id == ItemId.Silver || itemData.Id == ItemId.Gold;
				var rollsRemaining = rerolls;
				do
				{
					rollsRemaining--;

					if (rnd.NextDouble() * 100 < dropChance)
					{
						// Determine amount
						var amount = dropEntry.MinAmount;
						if (dropEntry.MaxAmount > dropEntry.MinAmount)
							amount = rnd.Next(dropEntry.MinAmount, dropEntry.MaxAmount + 1);

						// Use DropItem which handles party distribution and equipment grading
						this.DropItem(killer, dropEntry.ItemId, amount, dropChance);
					}
				}
				while (rollsRemaining > 0 && !isMoney);
			}
		}

		/// <summary>
		/// Randomly assigns rare monster buffs based on given rates.
		/// </summary>
		/// <param name="jackpotRate">
		/// Rate modifier for chance to receive a jackpot buff. The default,
		/// 100%, represents the default chance as per the configuration.
		/// </param>
		/// <param name="eliteRate">
		/// Rate modifier for chance to receive an elite buff. The default,
		/// 100%, represents the default chance as per the configuration.
		/// </param>
		/// <returns></returns>
		public void PossiblyBecomeRare(float jackpotRate = 100, float eliteRate = 100)
		{
			if (Versions.Client < KnownVersions.PreReBuild)
				return;

			if (this.Rank == MonsterRank.Boss ||
				this.Rank == MonsterRank.MISC ||
				this.Rank == MonsterRank.Material ||
				this.Rank == MonsterRank.NPC)
				return;

			var rnd = RandomProvider.Get();

			var worldConf = ZoneServer.Instance.Conf.World;

			var silverChance = worldConf.SilverJackpotSpawnChance * jackpotRate / 100f;
			if (rnd.NextDouble() * 100 < silverChance)
			{
				this.StartBuff(BuffId.SuperDrop, 100, 0, TimeSpan.Zero, this);
				return;
			}

			var goldChance = worldConf.GoldJackpotSpawnChance * jackpotRate / 100f;
			if (rnd.NextDouble() * 100 < goldChance)
			{
				this.StartBuff(BuffId.SuperDrop, 1000, 1, TimeSpan.Zero, this);
				return;
			}

			// The default chance for SuperExp is 1:12000, based on the
			// monster property "SuperExpRegenRatio".
			var blueChance = worldConf.BlueJackpotSpawnChance * jackpotRate / 100f;
			if (rnd.NextDouble() * 100 < blueChance)
			{
				this.StartBuff(BuffId.SuperExp, 1, 0, TimeSpan.Zero, this);
				return;
			}

			var redChance = worldConf.RedJackpotSpawnChance * jackpotRate / 100f;
			if ((rnd.NextDouble() * 100) < redChance)
			{
				this.StartBuff(BuffId.SuperMonGen, 1, 0, TimeSpan.Zero, this);
				this.Died += this.SuperMonGenMob_Died;
				return;
			}

			var canBecomeElite = this.Level < worldConf.EliteMinLevel;
			if (canBecomeElite)
				return;

			var eliteChance = worldConf.EliteSpawnChance * eliteRate / 100f;
			if (rnd.NextDouble() * 100 < eliteChance)
			{
				this.StartBuff(BuffId.EliteMonsterBuff, 1, 0, TimeSpan.Zero, this);

				if (worldConf.EliteAlwaysAggressive)
					this.Tendency = TendencyType.Aggressive;

				// TODO: Add summoning and special attacks.
			}
		}

		private async void SuperMonGenMob_Died(Mob mob, ICombatEntity killer)
		{
			mob.Died -= mob.SuperMonGenMob_Died;

			if (killer == null)
				return;

			var map = killer.Map;
			if (map == null)
				return;

			var worldConf = ZoneServer.Instance.Conf.World;

			var waves = RandomProvider.Next(worldConf.RedJackpotWaveMin, worldConf.RedJackpotWaveMax);
			var waveMinDelay = worldConf.RedJackpotWaveDelayMin;
			var waveMaxDelay = worldConf.RedJackpotWaveDelayMax;
			var mobPerWave = worldConf.RedJackpotWaveMonsterCount;

			var levelModifier = Math.Min(1f, mob.Level / 100f);
			mobPerWave = (int)Math.Ceiling(mobPerWave * levelModifier);
			mobPerWave = Math.Max(3, mobPerWave);

			var monsterId = mob.Id;
			var deathPos = mob.Position;
			var aiName = mob.Data?.AiName;

			await Task.Delay(500);
			for (var i = 0; i < waves; i++)
			{
				if (map == null)
					break;

				var fromGround = false;
				if (RandomProvider.Get().Next(2) == 1)
					fromGround = true;

				var waveDelay = TimeSpan.FromSeconds(RandomProvider.Get().Next(waveMinDelay, waveMaxDelay));

				for (var j = 0; j < mobPerWave; j++)
				{
					var spawnMob = new Mob(monsterId, RelationType.Enemy);
					var spawnPos = Position.Zero;
					for (var k = 1; k <= 100; k++)
					{
						var randPos = deathPos.GetRandomInRange2D(0, 150);
						if (map.Ground.IsValidPosition(randPos))
							spawnPos = randPos;
						else
							spawnPos = deathPos;
					}
					spawnMob.Position = spawnPos;
					spawnMob.SpawnPosition = spawnMob.Position;
					spawnMob.Components.Add(new MovementComponent(spawnMob));
					spawnMob.Components.Add(new LifeTimeComponent(spawnMob, TimeSpan.FromMinutes(1)));
					if (!string.IsNullOrEmpty(aiName) && AiScript.Exists(aiName))
						spawnMob.Components.Add(new AiComponent(spawnMob, aiName));
					else
						spawnMob.Components.Add(new AiComponent(spawnMob, "BasicMonster"));
					spawnMob.InsertHate(killer);
					spawnMob.Tendency = TendencyType.Aggressive;
					spawnMob.FromGround = fromGround;
					if (map.TryGetPropertyOverrides(monsterId, out var propertyOverrides))
						spawnMob.ApplyOverrides(propertyOverrides);
					map.AddMonster(spawnMob);
				}
				await Task.Delay(waveDelay);
			}
		}

		/// <summary>
		/// Returns true if the monster can attack the entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool CanAttack(ICombatEntity entity)
		{
			if (entity == this)
				return false;

			if (entity.IsDead)
				return false;

			if (this.IsLocked(LockType.Attack))
				return false;

			if (!this.CanSee(entity))
				return false;

			if (entity.Properties.GetString(PropertyName.HitProof, "NO") == "YES" || entity.IsSafe())
				return false;

			if (entity is Companion companion && companion.IsRiding)
				return false;

			if (!this.IsEnemy(entity))
				return false;

			if (entity is Character character
				&& character.Connection != null
				&& !character.Connection.LoadComplete)
				return false;

			// For now, let's specify that mobs can attack any combat
			// entities, since we want them them to be able to attack
			// both characters and other mobs.
			//return (entity is ICombatEntity);

			// New plan. Let's say that mobs can attack those entities
			// they're hostile towards. That allows AoEs to ignore
			// friendly entities. If the mob doesn't have an AI,
			// it shouldn't need to be able to attack anything,
			// so we return false in that case.
			if (!this.Components.TryGet<AiComponent>(out var ai))
				return this.IsEnemy(entity);


			return ai.Script.IsHostileTowards(entity);
		}

		/// <summary>
		/// Returns true if the monster can attack others.
		/// </summary>
		/// <returns></returns>
		public virtual bool CanFight()
		{
			if (this.IsDead || this.IsLocked(LockType.Attack) || this.IsKnockedDown() || this.IsKnockedBack())
				return false;

			if (this is Companion companion && companion.IsRiding)
				return false;

			// Cannot attack while casting
			if (this.IsCasting())
				return false;

			return true;
		}

		/// <summary>
		/// Returns true if the monster can move.
		/// </summary>
		/// <returns></returns>
		public bool CanMove()
		{
			if (this.IsDead
				|| this.MoveType == MoveType.Holding
				|| !this.Components.Has<MovementComponent>()
				|| this.IsLocked(LockType.Movement))
				return false;

			// Cannot move while casting (unless skill allows it)
			if (this.IsCasting() && !this.IsMoveableCasting())
				return false;

			return true;
		}

		/// <summary>
		/// Updates monster and its components.
		/// </summary>
		/// <param name="elapsed"></param>
		public virtual void Update(TimeSpan elapsed)
		{
			this.Components.Update(elapsed);
		}

		/// <summary>
		/// Heals the monster to its maximum HP and SP.
		/// </summary>
		public void HealToFull()
		{
			if (this.IsDead)
				return;
			this.Heal(this.Properties.GetFloat(PropertyName.MHP), this.Properties.GetFloat(PropertyName.MSP));
		}

		/// <summary>
		/// Heals the monster's HP and SP by the given amounts. Applies potential
		/// (de)buffs that affect healing.
		/// </summary>
		/// <param name="hpAmount"></param>
		/// <param name="spAmount"></param>
		public virtual void Heal(float hpAmount, float spAmount)
		{
			if (this.IsDead)
				return;

			if (hpAmount == 0 && spAmount == 0)
				return;

			// TODO: Move this somewhere else, perhaps with a hook/event?
			DecreaseHeal_Debuff.TryApply(this, ref hpAmount);

			this.Properties.Modify(PropertyName.HP, hpAmount);
			this.Properties.Modify(PropertyName.SP, spAmount);

			this.HpChangeCounter++;

			if (hpAmount > 0)
				Send.ZC_ADD_HP(this, hpAmount, this.Hp, this.HpChangeCounter);
		}

		/// <summary>
		/// Restore the monster's shield. Only works for boss monsters.
		/// </summary>
		public void HealShield(int amount)
		{
			if (this.IsDead)
				return;

			if (amount == 0)
				return;

			this.Shield = this.Shield + amount;

			if (amount > 0)
				Send.ZC_UPDATE_SHIELD(this, this.Shield, 1);
		}

		/// <summary>
		/// Modifies character's HP by the given amount without updating
		/// the client. Returns the new HP value and the priority number
		/// of this modification.
		/// </summary>
		/// <remarks>
		/// There are several packets in this game that require the HP
		/// to be set synchronized, to ensure that it's only set from
		/// one source and to identify the latest amount based on the
		/// "priority".
		/// </remarks>
		/// <param name="amount"></param>
		public void ModifyHpSafe(float amount, out float newHp, out int priority)
		{
			// Make sure it's not possible for two calls to interfere
			// with each other, so that the correct amount makes it to
			// the client, with the correct priority.
			lock (_hpLock)
			{
				newHp = (int)this.Properties.Modify(PropertyName.HP, amount);
				priority = (this.HpChangeCounter += 1);
			}
		}

		/// <summary>
		/// Overrides the monster's properties with the given values.
		/// </summary>
		/// <param name="overrides"></param>
		public void ApplyOverrides(PropertyOverrides overrides)
		{
			foreach (var propertyOverride in overrides)
			{
				var propertyName = propertyOverride.Key;

				// Since calculated properties can't be overridden directly,
				// we swap to the override properties that the calculation
				// functions use for each property as necessary.
				var properties = this.Properties as Properties;

				var canSet = !properties.TryGet(propertyName, out var property) || (property is not CFloatProperty && property is not RFloatProperty);
				if (!canSet)
					properties = this.Properties.Overrides;

				switch (propertyOverride.Value)
				{
					case int intValue:
						properties.SetFloat(propertyName, intValue);
						break;

					case float floatValue:
						properties.SetFloat(propertyName, floatValue);
						break;

					case string stringValue:
						properties.SetString(propertyName, stringValue);
						break;
				}
			}

			this.Properties.InvalidateAll();

			this.Properties.SetFloat(PropertyName.HP, this.Properties.GetFloat(PropertyName.MHP));
			this.Properties.SetFloat(PropertyName.SP, this.Properties.GetFloat(PropertyName.MSP));
		}

		/// <summary>
		/// Applies a "#"-delimited list of property name/value pairs to
		/// the monster's property collection.
		/// </summary>
		public void ApplyPropList(string propertyList, ICombatEntity owner, Skill skill)
		{
			if (string.IsNullOrEmpty(propertyList))
				return;
			var props = propertyList.Split("#");

			for (var i = 0; i < props.Length; i += 2)
			{
				var prop = props[i];
				var propValue = props[i + 1];

				if (!PropertyTable.Exists(this.Properties.Namespace, prop))
					continue;
				if (float.TryParse(propValue, out var floatPropValue))
					this.Properties.SetFloat(prop, floatPropValue);
				else
					this.Properties.SetString(prop, propValue);
			}
		}

		/// <summary>
		/// Can guard
		/// </summary>
		/// <returns></returns>
		public bool CanGuard()
		{
			return true;
		}

		/// <summary>
		/// Can stagger
		/// </summary>
		/// <returns></returns>
		public bool CanStagger()
		{
			return this.Rank == MonsterRank.Boss;
		}

		/// <summary>
		/// If this mob can be knocked down
		/// </summary>
		/// <returns></returns>
		public bool IsKnockdownable()
		{
			if (this.Data.MoveType == MoveType.Holding)
				return false;

			if (this.EffectiveSize > SizeType.L)
				return false;

			if (this.Rank == MonsterRank.Boss || this.Rank == MonsterRank.NPC)
				return false;

			return true;
		}

		/// <summary>
		/// Sets whether the monster can be hit. When set to false the
		/// monster is immune to all damage.
		/// </summary>
		public void SetHittable(bool isHittable)
		{
			this.Properties.SetString(PropertyName.HitProof, isHittable ? "NO" : "YES");
		}

		/// <summary>
		/// Starts casting a skill, showing the skill balloon UI and setting
		/// the casting state. The mob cannot move or attack while casting.
		/// </summary>
		/// <param name="skill">The skill being cast.</param>
		/// <param name="skillName">Display name for the skill balloon.</param>
		/// <param name="castTimeMs">Cast time in milliseconds.</param>
		/// <param name="showCastingBar">If true, shows a casting bar with gauge.</param>
		/// <param name="changeColor">If true, displays the text in yellow.</param>
		public void StartCasting(Skill skill, string skillName, int castTimeMs, bool showCastingBar = true, bool changeColor = false)
		{
			this.SetCastingState(true, skill);
			Send.MonsterSkillBalloon(this, skillName, castTimeMs, showCastingBar, changeColor);
		}

		/// <summary>
		/// Ends casting, hiding the skill balloon UI and clearing the casting state.
		/// </summary>
		/// <param name="skill">The skill that was being cast.</param>
		public void EndCasting(Skill skill)
		{
			this.SetCastingState(false, skill);
			Send.MonsterSkillBalloonCancel(this);
		}

		/// <summary>
		/// Interrupts the current casting, cancelling the skill and hiding
		/// the balloon. Returns true if casting was interrupted.
		/// </summary>
		/// <returns>True if casting was interrupted, false if not casting.</returns>
		public bool TryInterruptCasting()
		{
			if (this.Components.Get<CombatComponent>().TryInterruptCasting(out _))
			{
				Send.MonsterSkillBalloonCancel(this);
				return true;
			}
			return false;
		}
	}
}
