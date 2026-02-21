using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class SkillData : ITagged
	{
		public SkillId Id { get; set; }
		public string ClassName { get; set; }
		public string Name { get; set; }

		public SkillActivationType ActivationType { get; set; }
		public SkillUseType UseType { get; set; }
		public SkillAttackType AttackType { get; set; }
		public AttributeType Attribute { get; set; }
		public SkillClassType ClassType { get; set; }
		public SkillType Type { get; set; }
		public SkillTargetType TargetType { get; set; }
		public Tags Tags { get; set; }
		public SkillCastingType CastingType { get; set; } = SkillCastingType.Normal;

		public float BasicSp { get; set; }
		public float BasicCast { get; set; }
		public float BasicStamina { get; set; }

		public float EnableAngle { get; set; }
		public float MaxRange { get; set; }
		public float WaveLength { get; set; }

		public SplashType SplashType { get; set; }
		public float SplashRange { get; set; }
		public float SplashHeight { get; set; }
		public float SplashAngle { get; set; }
		public float SplashRate { get; set; }

		public float Factor { get; set; }
		public float FactorByLevel { get; set; }
		public float AtkAdd { get; set; }
		public float AtkAddByLevel { get; set; }
		public int HitCount { get; set; }
		public int MultiHitCount { get; set; }

		public TimeSpan DefaultHitDelay { get; set; }
		public TimeSpan DeadHitDelay { get; set; }
		public TimeSpan ShootTime { get; set; }
		public TimeSpan DelayTime { get; set; }
		public TimeSpan CancelTime { get; set; }
		public List<TimeSpan> HitTime { get; set; }
		public List<TimeSpan> HoldTime { get; set; }

		public float SpeedRate { get; set; }
		public bool SpeedRateAffectedByDex { get; set; }
		public bool SpeedRateAffectedByBuff { get; set; }

		public bool EnableCastMove { get; set; }
		public bool CastInterruptible { get; set; }

		public HitType KnockDownHitType { get; set; }
		public int KnockDownVelocity { get; set; }
		public int KnockDownHAngle { get; set; }
		public int KnockDownVAngle { get; set; }
		public int KnockDownBound { get; set; }

		public AbilityId ReinforceAbility { get; set; }
		public AbilityId HiddenReinforceAbility { get; set; }
		public float HiddenReinforceAbilityFactorByLevel { get; set; }

		public CooldownId CooldownGroup { get; set; }
		public TimeSpan CooldownTime { get; set; }

		public CooldownId OverheatGroup { get; set; }
		public int OverheatCount { get; set; }
		public TimeSpan OverHeatDelay { get; set; }
		// Ask exec what's the custom SkillAttackType he's using.
		// This Attack Type is the client defined values.
		public HitAttackType HitAttackType { get; set; }
		public List<int> RequiredStance { get; internal set; }
		public int IdValue { get; internal set; }
	}

	public enum SkillType
	{
		Attack,
		Buff,
		Magic,
	}

	public enum SkillCastingType
	{
		/// <summary>
		/// Normal casting, no special effects.
		/// </summary>
		Normal,
		/// <summary>
		/// Casting with a delay, where the skill is prepared before use.
		/// </summary>
		Casting,
		/// <summary>
		/// Dynamic casting, where the skill can be used while moving or changing direction.
		/// </summary>
		DynamicCasting,
		/// <summary>
		/// Channeling casting, where the skill is channeled over time.
		/// </summary>
		Channeling,
		/// <summary>
		/// Instant casting, where the skill is used immediately without delay.
		/// </summary>
		Instant,
	}

	public enum SkillTargetType
	{
		None,
		Actor,
		Front,
		Ground,
		Self
	}

	public enum SplashType
	{
		Square,
		Circle,
		Fan,
		Area,
		Wall,

		// The Vanquisher skills have an empty string as splash type.
		// TODO: Figure out what it does.
		Empty,
	}

	public enum SkillUseType
	{
		Melee,
		Force,
		Self,
		Script,
		MeleeGround,
		ForceGround,
		TargetGround,
	}

	public enum SkillAttackType
	{
		None,
		Slash,
		Aries,
		Strike,
		Magic,
		Arrow,
		Gun,
		Cannon,

		// These values don't seem to be attack types, but they are found
		// on skills in the client's data.
		Melee,
		Holy,
		Pad,
	}

	// We used to have this dedicated skill attribute enum for the skill data
	// in addition to the AttributeType enum in Shared, which effectively does
	// the same thing. We switched over to using AttributeType to unify the
	// attributes, though we'll leave this here as a reminder for the moment,
	// and in case we might actually want or need separate enums.
	// The only difference of note is that the skill data uses a "Magic"
	// attribute, which doesn't appear to exist as a monster attribute.
	//public enum AttributeType
	//{
	//	None,
	//	Fire,
	//	Ice,
	//	Lightning,
	//	Earth,
	//	Poison,
	//	Holy,
	//	Dark,
	//	Soul,

	//	// Any attributes using Melee or Magic could be bugs, since those
	//	// should be attack types, not attributes. But they are found on
	//	// skills in the the client's data.
	//	Melee,
	//	Magic,
	//}

	public enum SkillClassType
	{
		None,
		Melee,
		Missile,
		Magic,
		Responsive,
		TrueDamage,
		AbsoluteDamage,
	}

	public enum HitAttackType
	{
		None = 0,
		Magic = 0,
		Pad = 0,
		Fire = 1,
		Ice = 2,
		Lightning = 3,
		Earth = 4,
		Poison = 5,
		Dark = 6,
		Holy = 7,
		Soul = 8,
		Melee = 9,
		Slash = 101,
		Aries = 102,
		Strike = 103,
		Arrow = 104,
		Gun = 105,
		Cannon = 106,
		Missile = 301,
		Cleric_Cure = 501,
	}

	/// <summary>
	/// Defines how a skill is activated.
	/// </summary>
	public enum SkillActivationType
	{
		/// <summary>
		/// Skill is used actively by an actor.
		/// </summary>
		ActiveSkill,

		/// <summary>
		/// Skill is not used and its effects are passive.
		/// </summary>
		PassiveSkill,
	}

	/// <summary>
	/// Skill database, indexed by skill id.
	/// </summary>
	public class SkillDb : DatabaseJsonIndexed<SkillId, SkillData>
	{
		/// <summary>
		/// Returns first skill data entry with given class name, or null
		/// if it wasn't found.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public SkillData Find(string className)
			=> this.Find(a => a.ClassName == className);

		/// <summary>
		/// Returns the skill data entry if found
		/// otherwise if null returns false.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="skillData"></param>
		/// <returns></returns>
		public bool TryFind(string className, out SkillData skillData)
		{
			skillData = this.Find(className);
			return skillData != null;
		}

		/// <summary>
		/// Returns first skill data entry with given overheat group, or null
		/// if it wasn't found.
		/// </summary>
		/// <param name="overheatGroup"></param>
		/// <returns></returns>
		public SkillData FindByOverheatGroup(CooldownId overheatGroup)
			=> this.Find(a => a.OverheatGroup == overheatGroup);

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("skillId", "className", "name", "useType", "attackType", "attribute", "classType", "enableAngle", "maxRange", "waveLength", "splashType", "splashRange", "splashHeight", "splashAngle", "splashRate", "factor", "factorByLevel", "atkAdd", "atkAddByLevel", "hitCount", "multiHitCount", "defaultHitDelay", "deadHitDelay", "shootTime", "delayTime", "cancelTime", "hitTime", "holdTime", "speedRate", "speedRateAffectedByDex", "speedRateAffectedByBuff", "enableCastMove");

			var data = new SkillData();

			data.IdValue = entry.ReadInt("skillId");
			data.Id = (SkillId)entry.ReadInt("skillId");
			data.ClassName = entry.ReadString("className");
			data.Name = entry.ReadString("name");

			data.ActivationType = entry.ReadEnum("activationType", SkillActivationType.ActiveSkill);
			data.UseType = entry.ReadEnum<SkillUseType>("useType");
			data.AttackType = entry.ReadEnum<SkillAttackType>("attackType");
			data.HitAttackType = entry.ReadEnum<HitAttackType>("attackType");
			data.Attribute = entry.ReadEnum<AttributeType>("attribute");
			data.ClassType = entry.ReadEnum<SkillClassType>("classType");
			data.Type = entry.ReadEnum<SkillType>("type");
			data.TargetType = entry.ReadEnum<SkillTargetType>("target");

			data.BasicSp = entry.ReadFloat("basicSp", 0);
			data.BasicCast = entry.ReadFloat("basicCast", 0);
			data.BasicStamina = entry.ReadFloat("basicStamina", 0);

			data.EnableAngle = entry.ReadFloat("enableAngle");
			data.MaxRange = entry.ReadFloat("maxRange");
			data.WaveLength = entry.ReadFloat("waveLength");

			data.SplashType = entry.ReadEnum<SplashType>("splashType");
			data.SplashRange = entry.ReadFloat("splashRange");
			data.SplashHeight = entry.ReadFloat("splashHeight");
			data.SplashAngle = entry.ReadFloat("splashAngle");
			data.SplashRate = entry.ReadFloat("splashRate");

			data.Factor = entry.ReadFloat("factor");
			data.FactorByLevel = entry.ReadFloat("factorByLevel");
			data.AtkAdd = entry.ReadFloat("atkAdd");
			data.AtkAddByLevel = entry.ReadFloat("atkAddByLevel");
			data.HitCount = entry.ReadInt("hitCount");
			data.MultiHitCount = entry.ReadInt("multiHitCount");

			data.DefaultHitDelay = entry.ReadTimeSpan("defaultHitDelay");
			data.DeadHitDelay = entry.ReadTimeSpan("deadHitDelay");
			data.ShootTime = entry.ReadTimeSpan("shootTime");
			data.DelayTime = entry.ReadTimeSpan("delayTime");
			data.CancelTime = entry.ReadTimeSpan("cancelTime");
			data.HitTime = entry.ReadList<int>("hitTime").Select(a => TimeSpan.FromMilliseconds(a)).ToList();
			data.HoldTime = entry.ReadList<int>("holdTime").Select(a => TimeSpan.FromMilliseconds(a)).ToList();

			data.SpeedRate = entry.ReadFloat("speedRate");
			data.SpeedRateAffectedByDex = entry.ReadBool("speedRateAffectedByDex");
			data.SpeedRateAffectedByBuff = entry.ReadBool("speedRateAffectedByBuff");

			data.EnableCastMove = entry.ReadBool("enableCastMove");
			data.CastInterruptible = entry.ReadBool("castInterruptible");

			data.KnockDownHitType = entry.ReadEnum("knockDownType", HitType.Normal);
			data.KnockDownVelocity = entry.ReadInt("knockDownVelocity", 0);
			data.KnockDownHAngle = entry.ReadInt("knockDownHAngle", 0);
			data.KnockDownVAngle = entry.ReadInt("knockDownVAngle", 0);

			var reinforceStr = entry.ReadString("reinforceAbility", "");
			data.ReinforceAbility = string.IsNullOrEmpty(reinforceStr) ? 0 : entry.ReadEnum<AbilityId>("reinforceAbility", 0);
			var hiddenReinforceStr = entry.ReadString("hiddenReinforceAbility", "");
			data.HiddenReinforceAbility = string.IsNullOrEmpty(hiddenReinforceStr) ? 0 : entry.ReadEnum<AbilityId>("hiddenReinforceAbility", 0);
			data.HiddenReinforceAbilityFactorByLevel = entry.ReadFloat("hiddenReinforceAbilityFactorByLevel", 0);

			var cooldownGroup = entry.ReadString("cooldownGroup");
			if (!string.IsNullOrEmpty(cooldownGroup))
				data.CooldownGroup = entry.ReadEnum("cooldownGroup", CooldownId.Default);
			else
				data.CooldownGroup = CooldownId.Global;
			data.CooldownTime = entry.ReadTimeSpan("cooldownTime", TimeSpan.Zero);

			if (Versions.Protocol > 500)
				data.OverheatGroup = entry.ReadEnum("overheatGroup", CooldownId.Default);
			else
				data.OverheatGroup = entry.ReadEnum("overheatGroup", CooldownId.Global);
			data.OverheatCount = entry.ReadInt("overheatCount", 0);
			data.OverHeatDelay = entry.ReadTimeSpan("overheatDelay", TimeSpan.Zero);

			data.RequiredStance = entry.ReadList<int>("requiredStance");

			data.Tags = new Tags(entry.ReadList<string>("tags", []));

			this.AddOrReplace(data.Id, data);
		}
	}
}
