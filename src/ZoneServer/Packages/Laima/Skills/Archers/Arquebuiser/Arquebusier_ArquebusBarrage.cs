using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Arquebusier
{
	/// <summary>
	/// Handler for the Arquebuiser skill Arquebus Barrage.
	/// </summary>
	/// <remarks>
	/// The skill is not cast by the player, it follows up the other
	/// Arquebusier attack skills, which call TryActivate once their
	/// own attack went out.
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Arquebusier_ArquebusBarrage)]
	public class Arquebusier_ArquebusBarrage : IGroundSkillHandler
	{
		private const int HitCount = 3;
		private const float SplashLength = 150;
		private const float SplashWidth = 30;
		private const float DeactivatedDamageMultiplier = 1.05f;
		private readonly static TimeSpan FollowUpDelay = TimeSpan.FromMilliseconds(200);
		private readonly static TimeSpan HitAniTime = TimeSpan.FromMilliseconds(400);
		private readonly static TimeSpan CooldownReductionPerHit = TimeSpan.FromSeconds(1);

		/// <summary>
		/// Handles a direct use of the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Attack(skill, caster, farPos);
		}

		/// <summary>
		/// Fires the barrage as a follow-up to another Arquebusier attack
		/// skill, if the caster has it and it's not on cooldown.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="farPos">Target position of the skill that triggered the barrage.</param>
		public static void TryActivate(ICombatEntity caster, Position farPos)
		{
			if (!caster.TryGetSkill(SkillId.Arquebusier_ArquebusBarrage, out var skill) || skill.IsOnCooldown)
				return;

			// Arquebus Barrage: Deactivate
			// The barrage is not cast anymore
			if (caster.IsAbilityActive(AbilityId.Arquebusier21))
				return;

			caster.StartCooldown(skill.Data.CooldownGroup, skill.Properties.CoolDown);
			skill.Run(FollowUp(skill, caster, farPos));
		}

		/// <summary>
		/// Lets the triggering skill's hit play out before attacking.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="farPos"></param>
		private static async Task FollowUp(Skill skill, ICombatEntity caster, Position farPos)
		{
			await skill.Wait(FollowUpDelay);

			if (caster.IsDead)
				return;

			Attack(skill, caster, farPos);
		}

		/// <summary>
		/// Damages the enemies in front of the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="farPos"></param>
		private static void Attack(Skill skill, ICombatEntity caster, Position farPos)
		{
			var splashArea = new Square(caster.Position, caster.Direction, SplashLength, SplashWidth);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitCount));
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, HitAniTime, TimeSpan.Zero);

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, skill);
					skillHit.HitInfo.KnockBackType = skillHit.KnockBackInfo.HitType;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_SKILL_READY(caster, skill, 1, caster.Position, caster.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, hits);
		}

		/// <summary>
		/// Reduces the barrage's cooldown whenever one of the Arquebusier
		/// attack skills lands a hit, or boosts the Arquebusier skills
		/// instead if the barrage was deactivated.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, SkillId.Arquebusier_ArquebusBarrage)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Arquebus Barrage: Deactivate
			// Increases the final damage of Arquebusier skills by 5%
			if (attacker.IsAbilityActive(AbilityId.Arquebusier21))
			{
				if (skill.Data.ClassName.StartsWith("Arquebusier_"))
					skillHitResult.Damage *= DeactivatedDamageMultiplier;

				return;
			}

			if (!this.IsTriggerSkill(skill.Id) || skillHitResult.Result == HitResultType.Dodge || skillHitResult.Result == HitResultType.None)
				return;

			if (attacker.TryGetSkill(SkillId.Arquebusier_ArquebusBarrage, out var barrage))
				barrage.ReduceCooldown(CooldownReductionPerHit);
		}

		/// <summary>
		/// Returns true if the skill is one of the attack skills the
		/// barrage follows up on.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		private bool IsTriggerSkill(SkillId skillId)
		{
			return skillId == SkillId.Arquebusier_LuckyStrike
				|| skillId == SkillId.Arquebusier_Salute
				|| skillId == SkillId.Arquebusier_PrecisionFire;
		}
	}
}
