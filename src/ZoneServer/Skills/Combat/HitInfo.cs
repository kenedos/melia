using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Combat
{
	/// <summary>
	/// Contains information about a hit and the damage it caused.
	/// </summary>
	public class HitInfo
	{
		/// <summary>
		/// Returns a reference to the attacker.
		/// </summary>
		public ICombatEntity Attacker { get; }

		/// <summary>
		/// Returns a reference to the target.
		/// </summary>
		public ICombatEntity Target { get; }

		/// <summary>
		/// Returns the id of the skill used in the attack.
		/// </summary>
		public SkillId SkillId { get; }

		/// <summary>
		/// Returns the damage dealt.
		/// </summary>
		public float Damage { get; set; }

		/// <summary>
		/// Returns the target's current HP after the hit.
		/// </summary>
		public float Hp { get; }

		/// <summary>
		/// Returns the HP priority of the current HP value after the hit.
		/// </summary>
		public int HpPriority { get; }

		/// <summary>
		/// Returns the result type of the hit, affecting the hit effect.
		/// </summary>
		public HitResultType ResultType { get; set; }

		/// <summary>
		/// Returns the visual type of the hit, affecting the hit effect.
		/// </summary>
		public HitType Type { get; set; }

		/// <summary>
		/// Returns the knock back type of the hit.
		/// </summary>
		public KnockBackType KnockBackType { get; set; }

		/// <summary>
		/// Gets or sets the hit's force id.
		/// </summary>
		public int ForceId { get; set; }

		/// <summary>
		/// Gets or sets the number of times an attack hits.
		/// </summary>
		public int HitCount { get; set; } = 1;

		public HitAttackType AttackType { get; set; } = HitAttackType.None;

		/// <summary>
		/// Gets or sets the hit's unknown float 1.
		/// </summary>
		public float UnkFloat1 { get; set; } = 0f;

		/// <summary>
		/// Gets or sets the hit's unknown float 2.
		/// </summary>
		public float UnkFloat2 { get; set; } = 0f;

		/// <summary>
		/// Gets or sets the delay before the damage is shown.
		/// </summary>
		public TimeSpan AniTime { get; set; }

		/// <summary>
		/// Creates new hit.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="skillHitResult"></param>
		/// <param name="aniTime"></param>
		public HitInfo(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillHitResult skillHitResult, TimeSpan aniTime = default)
			: this(attacker, target, skill.Id, skillHitResult.Damage, skill.Data.HitType, skillHitResult.Result, skillHitResult.HitCount, aniTime)
		{
		}

		/// <summary>
		/// Creates new hit.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="skillHitResult"></param>
		/// <param name="resultType"></param>
		/// <param name="aniTime"></param>
		public HitInfo(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillHitResult skillHitResult, HitResultType resultType, TimeSpan aniTime = default)
			: this(attacker, target, skill.Id, skillHitResult.Damage, skill.Data.HitType, resultType, skillHitResult.HitCount, aniTime)
		{
		}

		/// <summary>
		/// Creates new hit.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="damage"></param>
		/// <param name="resultType"></param>
		/// <param name="hitCount"></param>
		/// <param name="aniTime"></param>
		public HitInfo(ICombatEntity attacker, ICombatEntity target, Skill skill, float damage, HitResultType resultType, int hitCount = 0, TimeSpan aniTime = default)
			: this(attacker, target, skill.Id, damage, skill.Data.HitType, resultType, hitCount, aniTime)
		{
		}

		/// <summary>
		/// Creates new hit.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skillId"></param>
		/// <param name="damage"></param>
		/// <param name="resultType"></param>
		/// <param name="hitType"></param>
		///	<param name="hitCount"></param>
		///	<param name="aniTime"></param>
		public HitInfo(ICombatEntity attacker, ICombatEntity target, SkillId skillId, float damage, SkillHitType hitType, HitResultType resultType, int hitCount = 0, TimeSpan aniTime = default)
		{
			this.Attacker = attacker;
			this.Target = target;
			this.SkillId = skillId;

			this.Damage = damage;
			this.ResultType = resultType;
			this.HitCount = hitCount;
			this.AniTime = aniTime == default ? TimeSpan.Zero : aniTime;
			this.Type = HitType.Normal;
			this.AttackType = HitAttackType.None;

			if (ZoneServer.Instance.Data.SkillDb.TryFind(skillId, out var skillData))
			{
				switch (resultType)
				{
					case HitResultType.Block:
						this.Type = HitType.Block;
						break;
					case HitResultType.Dodge:
						this.Type = HitType.NoHit;
						break;
					default:
						this.Type = skillData.Attribute switch
						{
							AttributeType.Fire => HitType.Fire,
							AttributeType.Ice => HitType.Ice,
							AttributeType.Poison => HitType.Poison,
							AttributeType.Lightning => HitType.Lightning,
							AttributeType.Earth => HitType.Earth,
							AttributeType.Soul => HitType.Soul,
							AttributeType.Holy => HitType.Holy,
							AttributeType.Dark => HitType.Dark,
							_ => HitType.Normal,
						};
						break;
				}
				this.AttackType = skillData.HitAttackType;
			}


			this.Hp = target.Hp;
			this.HpPriority = target.HpChangeCounter;
		}

		/// <summary>
		/// Creates new hit.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		/// <param name="resultType"></param>
		public HitInfo(ICombatEntity attacker, ICombatEntity target, float damage, HitResultType resultType)
		{
			this.Attacker = attacker;
			this.Target = target;
			this.SkillId = SkillId.Normal_Attack;

			this.Damage = damage;
			this.ResultType = resultType;
			this.Type = HitType.Normal;

			this.Hp = target.Hp;
			this.HpPriority = target.HpChangeCounter;

			// Disabled for now as it caused issues with some skills.
			// Multishot for example is type Force, but the hits get
			// delayed if a ForceId is sent.
			//if (hitType == SkillHitType.Force)
			//	this.ForceId = Combat.ForceId.GetNew();
		}
	}
}
