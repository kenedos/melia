using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sage
{
	/// <summary>
	/// Handler for Missile Hole, which reduces missile and magic bullet hits
	/// to 1 damage.
	/// </summary>
	/// <remarks>
	/// With [Arts] Missile Hole: Master of Dimensions, half of the damage it
	/// stopped is dealt to the enemies around the target when it ends.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.MissileHole_Buff)]
	public class Sage_MissileHole_BuffOverride : BuffHandler
	{
		private const string StoredDamageVar = "Melia.Sage.MissileHoleDamage";
		private const float ReleaseRate = 0.5f;
		private const float ReleaseRange = 100f;

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Caster is not ICombatEntity caster || !caster.IsAbilityActive(AbilityId.Sage22) || target.IsDead)
				return;

			var damage = buff.Vars.GetFloat(StoredDamageVar) * ReleaseRate;
			if (damage <= 0 || !caster.TryGetSkill(SkillId.Sage_MissileHole, out var skill))
				return;

			foreach (var enemy in target.Map.GetAttackableEnemiesInPosition(caster, target.Position, ReleaseRange))
			{
				var skillHitResult = new SkillHitResult { Damage = damage, Result = HitResultType.Hit };
				enemy.TakeDamage(damage, caster);

				Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, enemy, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.MissileHole_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.MissileHole_Buff, out var buff) || skillHitResult.Damage <= 1)
				return;

			if (skill.Data.ClassType != SkillClassType.Missile && skill.Data.UseType != SkillUseType.Force)
				return;

			buff.Vars.SetFloat(StoredDamageVar, buff.Vars.GetFloat(StoredDamageVar) + skillHitResult.Damage);
			skillHitResult.Damage = 1;
		}
	}

	/// <summary>
	/// Handler for Missile Hole: Escape, which sets movement speed to 60.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.MissileHole_MSPD_Buff)]
	public class Sage_MissileHole_MSPD_BuffOverride : BuffHandler
	{
		private const float FixedSpeed = 60f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.FIXMSPD_BM, FixedSpeed);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.FIXMSPD_BM);
		}
	}

	/// <summary>
	/// Handler for Micro Dimension: After Effects, which strikes the target
	/// with Micro Dimension once more when it ends.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.MicroDimension_Debuff)]
	public class Sage_MicroDimension_DebuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead || target.IsDead || caster.Map != target.Map)
				return;

			if (!caster.TryGetSkill(SkillId.Sage_MicroDimension, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
		}
	}

	/// <summary>
	/// Handler for Ultimate Dimension: After Effects' slow, which halves
	/// movement speed.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.UltimateDimension_Debuff)]
	public class Sage_UltimateDimension_DebuffOverride : BuffHandler
	{
		private const float SpeedReduction = 0.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -target.Properties.GetFloat(PropertyName.MSPD) * SpeedReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}

	/// <summary>
	/// Handler for Ultimate Dimension: After Effects' damage, which strikes
	/// the target with Ultimate Dimension every 0.5 seconds.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.UltimateDimension_Damage_Debuff)]
	public class Sage_UltimateDimension_Damage_DebuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead || target.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.Sage_UltimateDimension, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
		}
	}

	/// <summary>
	/// Handler for Blink's apparition, which takes the apparition out of the
	/// world when it ends.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Blink_ColorBlned)]
	public class Sage_Blink_ColorBlnedOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not DummyCharacter apparition || apparition.Map == null)
				return;

			Send.ZC_LEAVE(apparition);
			apparition.Map.RemoveCharacter(apparition);
		}
	}
}
