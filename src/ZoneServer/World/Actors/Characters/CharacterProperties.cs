using System;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Network;
using Yggdrasil.Util;
using Melia.Zone.Scripting;
using Melia.Zone.World.Items;
using Melia.Zone.Buffs;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Properties;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.World.Actors.Characters
{
	/// <summary>
	/// A player character's properties.
	/// </summary>
	public class CharacterProperties : Properties
	{
		/// <summary>
		/// Returns the owner of the properties.
		/// </summary>
		public Character Character { get; }

		/// <summary>
		/// Gets or sets stamina, clamped between 0 and MaxStamina.
		/// </summary>
		public int Stamina
		{
			get => _stamina;
			set => _stamina = (int)Math2.Clamp(0, this.MaxStamina, value);
		}
		private int _stamina;

		/// <summary>
		/// Returns the character's maximum stamina (property MaxSta).
		/// </summary>
		public int MaxStamina => (int)this.GetFloat(PropertyName.MaxSta);

		/// <summary>
		/// Returns the character's ability points based on the string
		/// property "AbilityPoint".
		/// </summary>
		public int AbilityPoints => int.Parse(this.GetString(PropertyName.AbilityPoint, "0"));

		/// <summary>
		/// Creates new instance for the character.
		/// </summary>
		/// <param name="character"></param>
		public CharacterProperties(Character character) : base("PC")
		{
			this.Character = character;
			this.AddDefaultProperties();
			this.InitEvents();
		}

		/// <summary>
		/// Sets up properties that every character has by default.
		/// </summary>
		public void AddDefaultProperties()
		{
			// We only need to set up properties that are calculated or
			// have min/max or non-default values. All others will be
			// created with default values as needed on demand.

			this.Create(new FloatProperty(PropertyName.Lv, 1, min: 1));

			this.Create(PropertyName.STR_JOB, "SCR_Get_Character_STR_JOB");
			this.Create(PropertyName.CON_JOB, "SCR_Get_Character_CON_JOB");
			this.Create(PropertyName.INT_JOB, "SCR_Get_Character_INT_JOB");
			this.Create(PropertyName.MNA_JOB, "SCR_Get_Character_MNA_JOB");
			this.Create(PropertyName.DEX_JOB, "SCR_Get_Character_DEX_JOB");

			this.Create(PropertyName.STR_ADD, "SCR_Get_Character_STR_ADD");
			this.Create(PropertyName.CON_ADD, "SCR_Get_Character_CON_ADD");
			this.Create(PropertyName.INT_ADD, "SCR_Get_Character_INT_ADD");
			this.Create(PropertyName.MNA_ADD, "SCR_Get_Character_MNA_ADD");
			this.Create(PropertyName.DEX_ADD, "SCR_Get_Character_DEX_ADD");

			this.Create(PropertyName.ALLSTAT, "SCR_Get_Character_ALLSTAT");
			this.Create(PropertyName.ALLSTAT_ADD, "SCR_Get_Character_ALLSTAT_ADD");

			this.Create(PropertyName.STR, "SCR_Get_Character_STR");
			this.Create(PropertyName.CON, "SCR_Get_Character_CON");
			this.Create(PropertyName.INT, "SCR_Get_Character_INT");
			this.Create(PropertyName.MNA, "SCR_Get_Character_MNA");
			this.Create(PropertyName.DEX, "SCR_Get_Character_DEX");

			this.Create(PropertyName.MaxSta, "SCR_Get_Character_MaxSta");
			this.Create(PropertyName.Sta_RunStart, "SCR_Get_Character_Sta_RunStart");
			this.Create(PropertyName.Sta_Run, "SCR_Get_Character_Sta_Run");
			this.Create(PropertyName.Sta_Recover, "SCR_Get_Character_Sta_Recover");
			this.Create(PropertyName.Sta_R_Delay, "SCR_Get_Character_Sta_R_Delay");
			this.Create(PropertyName.Sta_Runable, "SCR_Get_Character_Sta_Runable");
			this.Create(PropertyName.Sta_Jump, "SCR_Get_Character_Sta_Jump");
			this.Create(PropertyName.Sta_Step, "SCR_Get_Character_Sta_Step");

			// Don't set a max values for HP and SP initially, as that could cap the HP
			// during loading.
			this.Create(PropertyName.MHP, "SCR_Get_Character_MHP");
			this.Create(PropertyName.MSP, "SCR_Get_Character_MSP");
			this.Create(new FloatProperty(PropertyName.HP, min: 0));
			this.Create(new FloatProperty(PropertyName.SP, min: 0));

			this.Create(PropertyName.RHP, "SCR_Get_Character_RHP");
			this.Create(PropertyName.RSP, "SCR_Get_Character_RSP");
			this.Create(PropertyName.RHPTIME, "SCR_Get_Character_RHPTIME");
			this.Create(PropertyName.RSPTIME, "SCR_Get_Character_RSPTIME");

			this.Create(new FloatProperty(PropertyName.StatByLevel, min: 0));
			this.Create(new FloatProperty(PropertyName.StatByBonus, min: 0));
			this.Create(new FloatProperty(PropertyName.UsedStat, min: 0));
			this.Create(PropertyName.StatPoint, "SCR_Get_Character_StatPoint");
			this.Create(new StringProperty(PropertyName.AbilityPoint, "0")); // Why oh why did they make this a string >_>

			this.Create(PropertyName.MAXPATK, "SCR_Get_Character_MAXPATK");
			this.Create(PropertyName.MINPATK, "SCR_Get_Character_MINPATK");
			this.Create(PropertyName.MAXMATK, "SCR_Get_Character_MAXMATK");
			this.Create(PropertyName.MINMATK, "SCR_Get_Character_MINMATK");
			this.Create(PropertyName.MAXPATK_SUB, "SCR_Get_Character_MAXPATK_SUB");
			this.Create(PropertyName.MINPATK_SUB, "SCR_Get_Character_MINPATK_SUB");

			this.Create(PropertyName.DEF, "SCR_Get_Character_DEF");
			this.Create(PropertyName.MDEF, "SCR_Get_Character_MDEF");
			this.Create(PropertyName.CRTATK, "SCR_Get_Character_CRTATK");
			this.Create(PropertyName.CRTHR, "SCR_Get_Character_CRTHR");
			this.Create(PropertyName.CRTDR, "SCR_Get_Character_CRTDR");
			this.Create(PropertyName.HR, "SCR_Get_Character_HR");
			this.Create(PropertyName.DR, "SCR_Get_Character_DR");
			this.Create(PropertyName.BLK, "SCR_Get_Character_BLK");
			this.Create(PropertyName.BLK_BREAK, "SCR_Get_Character_BLK_BREAK");
			this.Create(PropertyName.HEAL_PWR, "SCR_Get_Character_HEAL_PWR");
			this.Create(PropertyName.SR, "SCR_Get_Character_SR");
			this.Create(PropertyName.SDR, "SCR_Get_Character_SDR");

			this.Create(PropertyName.MaxWeight, "SCR_Get_Character_MaxWeight");
			this.Create(PropertyName.NowWeight, "SCR_Get_Character_NowWeight");

			this.Create(PropertyName.MSPD, "SCR_Get_Character_MSPD");
			this.Create(PropertyName.JumpPower, "SCR_Get_Character_JumpPower");
			this.Create(PropertyName.CastingSpeed, "SCR_Get_Character_CastingSpeed");
			this.Create(PropertyName.NormalASPD, "SCR_Get_Character_AttackSpeed");

			this.Create(PropertyName.MovingShotable, "SCR_Get_Character_MovingShotable");
			this.Create(PropertyName.MovingShot, "SCR_Get_Character_MovingShot");

			this.Create(PropertyName.SkillRange, "SCR_Get_Character_SkillRange");
			this.Create(PropertyName.Guardable, "SCR_Get_Character_Guardable");
			this.Create(PropertyName.LootingChance, "SCR_Get_Character_LootingChance");

			// Elemental-Type Attack
			this.Create(PropertyName.Fire_Atk, "SCR_GET_FIRE_ATK");
			this.Create(PropertyName.Ice_Atk, "SCR_GET_ICE_ATK");
			this.Create(PropertyName.Lightning_Atk, "SCR_GET_LIGHTNING_ATK");
			this.Create(PropertyName.Poison_Atk, "SCR_GET_POISON_ATK");
			this.Create(PropertyName.Earth_Atk, "SCR_GET_EARTH_ATK");
			this.Create(PropertyName.Holy_Atk, "SCR_GET_HOLY_ATK");
			this.Create(PropertyName.Dark_Atk, "SCR_GET_DARK_ATK");
			this.Create(PropertyName.Soul_Atk, "SCR_GET_SOUL_ATK");

			// Attack-Type Attack
			this.Create(PropertyName.Slash_Atk, "SCR_GET_ATK_SLASH");
			this.Create(PropertyName.Aries_Atk, "SCR_GET_ATK_ARIES");
			this.Create(PropertyName.Strike_Atk, "SCR_GET_ATK_STRIKE");

			// Attack-Type Defense
			this.Create(PropertyName.DefSlash, "SCR_GET_DEF_SLASH");
			this.Create(PropertyName.DefAries, "SCR_GET_DEF_ARIES");
			this.Create(PropertyName.DefStrike, "SCR_GET_DEF_STRIKE");

			// Size-Type Attack
			this.Create(PropertyName.SmallSize_Atk, "SCR_GET_SmallSize_ATK");
			this.Create(PropertyName.MiddleSize_Atk, "SCR_GET_MiddleSize_ATK");
			this.Create(PropertyName.LargeSize_Atk, "SCR_GET_LargeSize_ATK");
			this.Create(PropertyName.BOSS_ATK, "SCR_GET_BOSS_ATK");

			// Race-Type Attack
			this.Create(PropertyName.Velnias_Atk, "SCR_GET_Velnias_ATK");
			this.Create(PropertyName.Widling_Atk, "SCR_GET_Widling_ATK");
			this.Create(PropertyName.Paramune_Atk, "SCR_GET_Paramune_ATK");
			this.Create(PropertyName.Forester_Atk, "SCR_GET_Forester_ATK");
			this.Create(PropertyName.Klaida_Atk, "SCR_GET_Klaida_ATK");

			// Armor Material-Type Attack
			this.Create(PropertyName.ADD_CLOTH, "SCR_GET_Cloth_ATK");
			this.Create(PropertyName.ADD_LEATHER, "SCR_GET_Leather_ATK");
			this.Create(PropertyName.ADD_IRON, "SCR_GET_Iron_ATK");
			this.Create(PropertyName.ADD_GHOST, "SCR_GET_Ghost_ATK");

			// Generic Damage Bonus
			this.Create(PropertyName.Add_Damage_Atk, "SCR_GET_Add_Damage_Atk");

			// Elemental-Type Resistances
			this.Create(PropertyName.ResFire, "SCR_GET_RES_FIRE");
			this.Create(PropertyName.ResIce, "SCR_GET_RES_ICE");
			this.Create(PropertyName.ResLightning, "SCR_GET_RES_LIGHTNING");
			this.Create(PropertyName.ResEarth, "SCR_GET_RES_EARTH");
			this.Create(PropertyName.ResPoison, "SCR_GET_RES_POISON");
			this.Create(PropertyName.ResHoly, "SCR_GET_RES_HOLY");
			this.Create(PropertyName.ResDark, "SCR_GET_RES_DARK");
			this.Create(PropertyName.ResSoul, "SCR_GET_RES_SOUL");

			// TODO: Update damage bonus properties based on equipment and
			//   other potential factors.
			this.Create(new RFloatProperty(PropertyName.Attribute, () => (int)AttributeType.None));
			this.Create(new RFloatProperty(PropertyName.ArmorMaterial, () => (int)ArmorMaterialType.None));
		}

		/// <summary>
		/// Sets up auto updates for the default properties.
		/// </summary>
		/// <remarks>
		/// Call after all properties were loaded, as to not trigger
		/// auto-updates before all properties are in place.
		/// </remarks>
		public void InitAutoUpdates()
		{
			this.AutoUpdate(PropertyName.STR, [PropertyName.Lv, PropertyName.STR_ADD, PropertyName.STR_STAT, PropertyName.STR_JOB, PropertyName.ALLSTAT]);
			this.AutoUpdate(PropertyName.CON, [PropertyName.Lv, PropertyName.CON_ADD, PropertyName.CON_STAT, PropertyName.CON_JOB, PropertyName.ALLSTAT]);
			this.AutoUpdate(PropertyName.INT, [PropertyName.Lv, PropertyName.INT_ADD, PropertyName.INT_STAT, PropertyName.INT_JOB, PropertyName.ALLSTAT]);
			this.AutoUpdate(PropertyName.MNA, [PropertyName.Lv, PropertyName.MNA_ADD, PropertyName.MNA_STAT, PropertyName.MNA_JOB, PropertyName.ALLSTAT]);
			this.AutoUpdate(PropertyName.DEX, [PropertyName.Lv, PropertyName.DEX_ADD, PropertyName.DEX_STAT, PropertyName.DEX_JOB, PropertyName.ALLSTAT]);
			this.AutoUpdate(PropertyName.STR_JOB, [PropertyName.Lv]);
			this.AutoUpdate(PropertyName.CON_JOB, [PropertyName.Lv]);
			this.AutoUpdate(PropertyName.INT_JOB, [PropertyName.Lv]);
			this.AutoUpdate(PropertyName.MNA_JOB, [PropertyName.Lv]);
			this.AutoUpdate(PropertyName.DEX_JOB, [PropertyName.Lv]);
			this.AutoUpdate(PropertyName.STR_ADD, [PropertyName.STR_BM, PropertyName.STR_Bonus, PropertyName.STR_ITEM_BM]);
			this.AutoUpdate(PropertyName.CON_ADD, [PropertyName.CON_BM, PropertyName.CON_Bonus, PropertyName.CON_ITEM_BM]);
			this.AutoUpdate(PropertyName.INT_ADD, [PropertyName.INT_BM, PropertyName.INT_Bonus, PropertyName.INT_ITEM_BM]);
			this.AutoUpdate(PropertyName.MNA_ADD, [PropertyName.MNA_BM, PropertyName.MNA_Bonus, PropertyName.MNA_ITEM_BM]);
			this.AutoUpdate(PropertyName.DEX_ADD, [PropertyName.DEX_BM, PropertyName.DEX_Bonus, PropertyName.DEX_ITEM_BM]);
			this.AutoUpdate(PropertyName.MHP, [PropertyName.Lv, PropertyName.CON, PropertyName.MHP_BM, PropertyName.MHP_Bonus]);
			this.AutoUpdate(PropertyName.MSP, [PropertyName.Lv, PropertyName.MNA, PropertyName.MSP_BM, PropertyName.MSP_Bonus]);
			this.AutoUpdate(PropertyName.StatPoint, [PropertyName.StatByLevel, PropertyName.StatByBonus, PropertyName.UsedStat]);
			this.AutoUpdate(PropertyName.MSPD, [PropertyName.FIXMSPD_BM, PropertyName.MSPD_BM, PropertyName.MSPD_Bonus]);
			this.AutoUpdate(PropertyName.CastingSpeed, [PropertyName.DEX, PropertyName.CastingSpeed_BM]);
			this.AutoUpdate(PropertyName.NormalASPD, [PropertyName.NormalASPD_BM]);
			this.AutoUpdate(PropertyName.DEF, [PropertyName.Lv, PropertyName.DEF_BM, PropertyName.DEF_RATE_BM]);
			this.AutoUpdate(PropertyName.MDEF, [PropertyName.Lv, PropertyName.MNA, PropertyName.MDEF_BM, PropertyName.MDEF_RATE_BM]);
			this.AutoUpdate(PropertyName.CRTATK, [PropertyName.Lv, PropertyName.DEX, PropertyName.CRTATK_BM]);
			this.AutoUpdate(PropertyName.CRTHR, [PropertyName.Lv, PropertyName.DEX, PropertyName.CRTHR_BM]);
			this.AutoUpdate(PropertyName.CRTDR, [PropertyName.Lv, PropertyName.CON, PropertyName.CRTDR_BM]);
			this.AutoUpdate(PropertyName.HR, [PropertyName.Lv, PropertyName.DEX, PropertyName.HR_BM, PropertyName.HR_RATE_BM]);
			this.AutoUpdate(PropertyName.DR, [PropertyName.Lv, PropertyName.DEX, PropertyName.DR_BM, PropertyName.DR_RATE_BM]);
			this.AutoUpdate(PropertyName.BLK, [PropertyName.Lv, PropertyName.CON, PropertyName.BLK_BM, PropertyName.BLK_RATE_BM]);
			this.AutoUpdate(PropertyName.BLK_BREAK, [PropertyName.Lv, PropertyName.STR, PropertyName.BLK_BREAK_BM, PropertyName.BLK_BREAK_RATE_BM]);
			this.AutoUpdate(PropertyName.HEAL_PWR, [PropertyName.Lv, PropertyName.INT, PropertyName.MNA, PropertyName.HEAL_PWR_BM, PropertyName.HEAL_PWR_RATE_BM]);
			this.AutoUpdate(PropertyName.SR, [PropertyName.SR_BM]);
			this.AutoUpdate(PropertyName.SDR, [PropertyName.FixedMinSDR_BM, PropertyName.SDR_BM]);
			this.AutoUpdate(PropertyName.MaxSta, [PropertyName.CON, PropertyName.MAXSTA_Bonus, PropertyName.MaxSta_BM]);
			this.AutoUpdate(PropertyName.Sta_Run, [PropertyName.DashRun]);
			this.AutoUpdate(PropertyName.Sta_Recover, [PropertyName.REST_BM, PropertyName.RSta_BM]);
			this.AutoUpdate(PropertyName.MINPATK, [PropertyName.Lv, PropertyName.STR, PropertyName.PATK_BM, PropertyName.MINPATK_BM, PropertyName.PATK_MAIN_BM, PropertyName.MINPATK_MAIN_BM, PropertyName.PATK_RATE_BM, PropertyName.MINPATK_RATE_BM, PropertyName.PATK_MAIN_RATE_BM, PropertyName.MINPATK_MAIN_RATE_BM]);
			this.AutoUpdate(PropertyName.MAXPATK, [PropertyName.Lv, PropertyName.STR, PropertyName.PATK_BM, PropertyName.MAXPATK_BM, PropertyName.PATK_MAIN_BM, PropertyName.MAXPATK_MAIN_BM, PropertyName.PATK_RATE_BM, PropertyName.MAXPATK_RATE_BM, PropertyName.PATK_MAIN_RATE_BM, PropertyName.MAXPATK_MAIN_RATE_BM]);
			this.AutoUpdate(PropertyName.MINMATK, [PropertyName.Lv, PropertyName.INT, PropertyName.MATK_BM, PropertyName.MINMATK_BM, PropertyName.MATK_RATE_BM, PropertyName.MINMATK_RATE_BM]);
			this.AutoUpdate(PropertyName.MAXMATK, [PropertyName.Lv, PropertyName.INT, PropertyName.MATK_BM, PropertyName.MAXMATK_BM, PropertyName.MATK_RATE_BM, PropertyName.MAXMATK_RATE_BM]);
			this.AutoUpdate(PropertyName.MaxWeight, [PropertyName.CON, PropertyName.STR, PropertyName.MaxWeight_BM, PropertyName.MaxWeight_Bonus]);
			this.AutoUpdate(PropertyName.MovingShot, [PropertyName.MovingShot_BM, PropertyName.MovingShotable]);
			this.AutoUpdate(PropertyName.MovingShotable, [PropertyName.MovingShot_BM]);
			this.AutoUpdate(PropertyName.LootingChance, [PropertyName.LootingChance_BM]);

			// Elemental-Type Attack
			this.AutoUpdate(PropertyName.Fire_Atk, [PropertyName.Fire_Atk_BM]);
			this.AutoUpdate(PropertyName.Ice_Atk, [PropertyName.Ice_Atk_BM]);
			this.AutoUpdate(PropertyName.Lightning_Atk, [PropertyName.Lightning_Atk_BM]);
			this.AutoUpdate(PropertyName.Poison_Atk, [PropertyName.Poison_Atk_BM]);
			this.AutoUpdate(PropertyName.Earth_Atk, [PropertyName.Earth_Atk_BM]);
			this.AutoUpdate(PropertyName.Holy_Atk, [PropertyName.Holy_Atk_BM]);
			this.AutoUpdate(PropertyName.Dark_Atk, [PropertyName.Dark_Atk_BM]);
			this.AutoUpdate(PropertyName.Soul_Atk, [PropertyName.Soul_Atk_BM]);

			// Size-Type Attack
			this.AutoUpdate(PropertyName.SmallSize_Atk, [PropertyName.SmallSize_Atk_BM]);
			this.AutoUpdate(PropertyName.MiddleSize_Atk, [PropertyName.MiddleSize_Atk_BM]);
			this.AutoUpdate(PropertyName.LargeSize_Atk, [PropertyName.LargeSize_Atk_BM]);
			this.AutoUpdate(PropertyName.BOSS_ATK, [PropertyName.BOSS_ATK_BM]);

			// Race-Type Attack
			this.AutoUpdate(PropertyName.Velnias_Atk, [PropertyName.Velnias_Atk_BM]);
			this.AutoUpdate(PropertyName.Widling_Atk, [PropertyName.Widling_Atk_BM]);
			this.AutoUpdate(PropertyName.Paramune_Atk, [PropertyName.Paramune_Atk_BM]);
			this.AutoUpdate(PropertyName.Forester_Atk, [PropertyName.Forester_Atk_BM]);
			this.AutoUpdate(PropertyName.Klaida_Atk, [PropertyName.Klaida_Atk_BM]);

			// Generic Damage Bonus
			this.AutoUpdate(PropertyName.Add_Damage_Atk, [PropertyName.Add_Damage_Atk_BM]);

			// Elemental-Type Resistances
			this.AutoUpdate(PropertyName.ResFire, [PropertyName.ResFire_BM]);
			this.AutoUpdate(PropertyName.ResIce, [PropertyName.ResIce_BM]);
			this.AutoUpdate(PropertyName.ResLightning, [PropertyName.ResLightning_BM]);
			this.AutoUpdate(PropertyName.ResEarth, [PropertyName.ResEarth_BM]);
			this.AutoUpdate(PropertyName.ResPoison, [PropertyName.ResPoison_BM]);
			this.AutoUpdate(PropertyName.ResHoly, [PropertyName.ResHoly_BM]);
			this.AutoUpdate(PropertyName.ResDark, [PropertyName.ResDark_BM]);
			this.AutoUpdate(PropertyName.ResSoul, [PropertyName.ResSoul_BM]);

			this.AutoUpdateMax(PropertyName.HP, PropertyName.MHP);
			this.AutoUpdateMax(PropertyName.SP, PropertyName.MSP);
		}

		/// <summary>
		/// Sets up event subscriptions, to react to actions of the
		/// character with property updates.
		/// </summary>
		private void InitEvents()
		{
			// Update recovery times when the character sits down,
			// as those properties are affected by the sitting status.
			this.Character.SitStatusChanged += this.SitStatusChanged;

			// Update some special properties when character's stats change
			this.Character.StatChanged += this.OnStatChanged;

			// Subscribe to equipment changes, as any number of properties
			// might make use of equipment stats
			if (this.Character.Inventory != null)
			{
				this.Character.Inventory.Equipped += this.OnEquipmentChanged;
				this.Character.Inventory.Unequipped += this.OnEquipmentChanged;
			}

			// Subscribe to buff changes
			if (this.Character.Buffs != null)
			{
				this.Character.Buffs.BuffStarted += this.OnBuffStarted;
				this.Character.Buffs.BuffUpdated += this.OnBuffsChanged;
				this.Character.Buffs.BuffEnded += this.OnBuffEnded;
			}

			// Subscribe to NormalASPD and DEX property changes to invalidate skill properties
			// since skills depend on these character properties for their speed calculations
			if (this.TryGet<CFloatProperty>(PropertyName.NormalASPD, out var aspdProperty))
				aspdProperty.ValueChanged += this.OnSkillSpeedPropertyChanged;

			if (this.TryGet<CFloatProperty>(PropertyName.DEX, out var dexProperty))
				dexProperty.ValueChanged += this.OnSkillSpeedPropertyChanged;

			// Subscribe to property changes that affect companion stats
			this.SubscribeCompanionPropertyUpdates();
		}

		/// <summary>
		/// Removes event subscriptions.
		/// </summary>
		public void RemoveEvents()
		{
			// Unsubscribe from sit status changes
			this.Character.SitStatusChanged -= this.SitStatusChanged;

			// Unsubscribe from stat changes
			this.Character.StatChanged -= this.OnStatChanged;

			// Unsubscribe from equipment changes
			this.Character.Inventory.Equipped -= this.OnEquipmentChanged;
			this.Character.Inventory.Unequipped -= this.OnEquipmentChanged;

			// Unsubscribe from buff changes
			this.Character.Buffs.BuffStarted -= this.OnBuffStarted;
			this.Character.Buffs.BuffUpdated -= this.OnBuffsChanged;
			this.Character.Buffs.BuffEnded -= this.OnBuffEnded;

			// Unsubscribe from ASPD and DEX property changes
			if (this.TryGet<CFloatProperty>(PropertyName.NormalASPD, out var aspdProperty))
				aspdProperty.ValueChanged -= this.OnSkillSpeedPropertyChanged;

			if (this.TryGet<CFloatProperty>(PropertyName.DEX, out var dexProperty))
				dexProperty.ValueChanged -= this.OnSkillSpeedPropertyChanged;

			// Unsubscribe from companion property updates
			this.UnsubscribeCompanionPropertyUpdates();
		}

		/// <summary>
		/// Unsubscribes from property changes that affect companion stats.
		/// </summary>
		private void UnsubscribeCompanionPropertyUpdates()
		{
			// Unsubscribe from owner properties that companions depend on
			var propertiesToWatch = new[]
			{
				PropertyName.DEF, PropertyName.MDEF, PropertyName.MHP,
				PropertyName.DR, PropertyName.HR, PropertyName.CRTHR,
				PropertyName.MINPATK, PropertyName.MAXPATK,
				PropertyName.MINMATK, PropertyName.MAXMATK,
				PropertyName.STR, PropertyName.DEX, PropertyName.CON,
				PropertyName.INT, PropertyName.MNA
			};

			foreach (var propertyName in propertiesToWatch)
			{
				if (this.TryGet<CFloatProperty>(propertyName, out var property))
					property.ValueChanged -= this.OnCompanionDependentPropertyChanged;
			}
		}

		/// <summary>
		/// Called when an item was equipped or equipped.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		private void OnEquipmentChanged(Character character, Item item)
		{
			// We could possibly limit this to only update equip-related
			// properties, such as MINATK and MAXATK, but we don't want
			// to accidentally miss something, and users might do god
			// knows what with custom property calculations.

			this.Character.InvalidateProperties();
		}

		/// <summary>
		/// Called when a buff was started.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		private void OnBuffStarted(ICombatEntity entity, Buff buff)
		{
			this.OnBuffsChanged(entity, buff);
			this.InvokeSkillBuffHooks(buff, isStart: true);
		}

		/// <summary>
		/// Called when a buff ended.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		private void OnBuffEnded(ICombatEntity entity, Buff buff)
		{
			this.OnBuffsChanged(entity, buff);
			this.InvokeSkillBuffHooks(buff, isStart: false);
		}

		/// <summary>
		/// Called when a buff was started, updated, or ended.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		private void OnBuffsChanged(ICombatEntity entity, Buff buff)
		{
			// Buffs come with a list of properties they affect,
			// but since we have no control over user customization,
			// and because like 80% of them invalidate all properties
			// anyway, we'll just always refresh everything.

			this.InvalidateAll();
			Send.ZC_OBJECT_PROPERTY(this.Character);
			Send.ZC_CASTING_SPEED(this.Character);
			Send.ZC_UPDATE_SKL_SPDRATE_LIST(this.Character, this.Character.Skills.GetList());
			if (buff.AffectsMovementSpeed())
				Send.ZC_MSPD(this.Character);
		}

		/// <summary>
		/// Invokes skill buff hooks for all skills the character has.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="isStart"></param>
		private void InvokeSkillBuffHooks(Buff buff, bool isStart)
		{
			var prefix = isStart ? "SCR_Buff_OnStart_" : "SCR_Buff_OnEnd_";

			foreach (var skill in this.Character.Skills.GetList())
			{
				if (ScriptableFunctions.SkillBuffHook.TryGet(prefix + skill.Id, out var func))
					func(skill, this.Character, buff);
			}
		}

		/// <summary>
		/// Recalculates and updates HP and SP recovery time properties.
		/// </summary>
		/// <param name="character"></param>
		private void SitStatusChanged(Character character)
		{
			this.Invalidate(PropertyName.RHPTIME, PropertyName.RSPTIME);
			Send.ZC_OBJECT_PROPERTY(this.Character, PropertyName.RHPTIME, PropertyName.RSPTIME);

			if (character.IsSitting)
			{
				//character.Buffs.Start(BuffId.Rest, TimeSpan.Zero);
			}
			else
			{
				character.Buffs.Stop(BuffId.Rest);
				character.Buffs.Stop(BuffId.campfire_Buff);
			}
		}

		/// <summary>
		/// Sends necessary updates to a character when their stats change.
		/// </summary>
		/// <param name="character"></param>
		private void OnStatChanged(Character character)
		{
			Send.ZC_CASTING_SPEED(character);
		}

		/// <summary>
		/// Called when ASPD or DEX properties change to invalidate skill
		/// speed calculations that depend on these character properties.
		/// </summary>
		/// <param name="propertyName"></param>
		private void OnSkillSpeedPropertyChanged(string propertyName)
		{
			// Invalidate SklSpdRate for all skills since it depends on
			// character's ASPD and DEX properties
			foreach (var skill in this.Character.Skills.GetList())
			{
				if (skill.Properties.TryGet<CFloatProperty>(PropertyName.SklSpdRate, out var spdRateProperty))
					spdRateProperty.Invalidate();
			}
		}

		/// <summary>
		/// Creates a new calculated float property that uses the given
		/// function.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="calcFuncName"></param>
		private void Create(string propertyName, string calcFuncName)
		{
			if (PropertyTable.Exists(this.Namespace, propertyName))
				this.Create(new CFloatProperty(propertyName, () => this.CalculateProperty(calcFuncName)));
		}

		/// <summary>
		/// Calls the calculation function with the given name and returns
		/// the result.
		/// </summary>
		/// <param name="calcFuncName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the function doesn't exist.
		/// </exception>
		private float CalculateProperty(string calcFuncName)
		{
			if (!ScriptableFunctions.Character.TryGet(calcFuncName, out var func))
				throw new ArgumentException($"Scriptable character function '{calcFuncName}' not found.");

			if (this.Character == null)
				return 0;

			return func(this.Character);
		}

		/// <summary>
		/// Subscribes to property changes that affect companion stats.
		/// </summary>
		private void SubscribeCompanionPropertyUpdates()
		{
			// Subscribe to owner properties that companions depend on
			var propertiesToWatch = new[]
			{
				PropertyName.DEF, PropertyName.MDEF, PropertyName.MHP,
				PropertyName.DR, PropertyName.HR, PropertyName.CRTHR,
				PropertyName.MINPATK, PropertyName.MAXPATK,
				PropertyName.MINMATK, PropertyName.MAXMATK,
				PropertyName.STR, PropertyName.DEX, PropertyName.CON,
				PropertyName.INT, PropertyName.MNA
			};

			foreach (var propertyName in propertiesToWatch)
			{
				if (this.TryGet<CFloatProperty>(propertyName, out var property))
					property.ValueChanged += this.OnCompanionDependentPropertyChanged;
			}
		}

		/// <summary>
		/// Called when a property that affects companion stats changes.
		/// Updates companion properties accordingly.
		/// </summary>
		/// <param name="propertyName"></param>
		private void OnCompanionDependentPropertyChanged(string propertyName)
		{
			// Check if character has an active companion
			if (!this.Character.Components.TryGet<CompanionComponent>(out var companionComponent))
				return;

			var companion = companionComponent.ActiveCompanion;
			if (companion == null || !companion.IsActivated || companion.IsBird)
				return;

			// Invalidate companion properties based on which owner property changed
			switch (propertyName)
			{
				case PropertyName.DEF:
					companion.Properties.Invalidate(PropertyName.DEF, PropertyName.MountDEF);
					break;

				case PropertyName.MDEF:
					companion.Properties.Invalidate(PropertyName.MDEF);
					break;

				case PropertyName.MHP:
					companion.Properties.Invalidate(PropertyName.MHP, PropertyName.MountMHP);
					break;

				case PropertyName.DR:
					companion.Properties.Invalidate(PropertyName.DR, PropertyName.MountDR);
					break;

				case PropertyName.HR:
					companion.Properties.Invalidate(PropertyName.HR);
					break;

				case PropertyName.CRTHR:
					companion.Properties.Invalidate(PropertyName.CRTHR);
					break;

				case PropertyName.MINPATK:
				case PropertyName.MAXPATK:
					companion.Properties.Invalidate(PropertyName.ATK);
					break;

				case PropertyName.MINMATK:
					companion.Properties.Invalidate(PropertyName.MINMATK);
					break;

				case PropertyName.MAXMATK:
					companion.Properties.Invalidate(PropertyName.MAXMATK);
					break;

				case PropertyName.STR:
					companion.Properties.Invalidate(PropertyName.STR);
					break;

				case PropertyName.DEX:
					companion.Properties.Invalidate(PropertyName.DEX, PropertyName.DR, PropertyName.HR, PropertyName.CRTHR);
					break;

				case PropertyName.CON:
					companion.Properties.Invalidate(PropertyName.CON);
					break;

				case PropertyName.INT:
					companion.Properties.Invalidate(PropertyName.INT);
					break;

				case PropertyName.MNA:
					companion.Properties.Invalidate(PropertyName.MNA);
					break;
			}

			// Send updated companion properties to the client
			if (this.Character.Connection != null)
				Send.ZC_OBJECT_PROPERTY(this.Character.Connection, companion);
		}
	}
}
