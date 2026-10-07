using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for the Bullet Marker skill Mozambique Drill, four shots at a
	/// single target, each a double hit for 22.5% less during Outrage.
	/// </summary>
	/// <remarks>
	/// Mozambique Drill: Ricochet bounces each shot into the nearest other
	/// enemy.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Bulletmarker_MozambiqueDrill)]
	public class Bulletmarker_MozambiqueDrillOverride : IForceSkillHandler
	{
		private const float OutrageDamageRate = 0.775f;
		private const int OutrageHitsPerShot = 2;
		private const float DefensePenetrationPerLevel = 0.02f;
		private const float RicochetRange = 50f;
		private static readonly TimeSpan[] ShotAniTimes = [TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(150), TimeSpan.FromMilliseconds(550), TimeSpan.FromMilliseconds(600)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (target == null || !BulletMarkerSkillHelper.CheckDoubleGunStance(caster))
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var outrage = BulletMarkerSkillHelper.TryConsumeOutrage(caster);
			var ricochetTarget = caster.IsAbilityActive(AbilityId.Bulletmarker10)
				? caster.Map.GetAttackableEnemiesInPosition(caster, target.Position, RicochetRange).FirstOrDefault(a => a != target)
				: null;

			var hits = new List<SkillHitInfo>();

			for (var shot = 0; shot < ShotAniTimes.Length; shot++)
			{
				hits.Add(this.Shoot(skill, caster, target, outrage, shot, 0));

				if (ricochetTarget != null)
					hits.Add(this.Shoot(skill, caster, ricochetTarget, outrage, shot, 1));
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, hits);
		}

		/// <summary>
		/// Fires one shot at the target.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="outrage"></param>
		/// <param name="shot"></param>
		/// <param name="targetIndex"></param>
		/// <returns></returns>
		private SkillHitInfo Shoot(Skill skill, ICombatEntity caster, ICombatEntity target, bool outrage, int shot, int targetIndex)
		{
			var modifier = BulletMarkerSkillHelper.CreateModifier(caster);

			if (outrage)
			{
				modifier.HitCount = OutrageHitsPerShot;
				modifier.DamageMultiplier *= OutrageDamageRate;
			}

			if (caster.TryGetActiveAbilityLevel(AbilityId.Bulletmarker9, out var level))
				modifier.DefensePenetrationRate += level * DefensePenetrationPerLevel;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, ShotAniTimes[shot], TimeSpan.Zero);
			skillHit.HitFrameIndex = (byte)shot;
			skillHit.TargetIndex = (byte)targetIndex;

			return skillHit;
		}
	}
}
