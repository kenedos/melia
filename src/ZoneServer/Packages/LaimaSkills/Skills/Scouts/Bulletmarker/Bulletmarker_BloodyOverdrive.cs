using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for the Bullet Marker skill Bloody Overdrive, which fires
	/// five shots into each quarter around the Bullet Marker in turn while
	/// they can't be knocked back.
	/// </summary>
	/// <remarks>
	/// During Outrage it fires three rounds into the whole circle instead.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Bulletmarker_BloodyOverdrive)]
	public class Bulletmarker_BloodyOverdriveOverride : IGroundSkillHandler
	{
		private const int HitsPerRound = 5;
		private const float Range = 80f;
		private const float QuarterAngle = 90f;
		private const float RicochetRange = 50f;
		private const int RicochetChancePerLevel = 5;
		private static readonly int[] QuarterRoundDelays = [200, 400, 400, 400];
		private static readonly int[] OutrageRoundDelays = [0, 100, 150];
		private static readonly int[] BroadsideRoundDelays = [50, 100, 100, 100];
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan SuperArmorDuration = TimeSpan.FromMilliseconds(1600);
		private static readonly TimeSpan InvincibleDuration = TimeSpan.FromSeconds(1);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!BulletMarkerSkillHelper.CheckDoubleGunStance(caster))
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var broadside = caster.IsAbilityActive(AbilityId.Bulletmarker25);

			caster.StartBuff(BuffId.Skill_SuperArmor_Buff, SuperArmorDuration);
			if (!broadside && caster.IsAbilityActive(AbilityId.Bulletmarker12))
				caster.StartBuff(BuffId.Skill_NoDamage_Buff, InvincibleDuration);

			var outrage = BulletMarkerSkillHelper.TryConsumeOutrage(caster);
			var roundDelays = outrage ? OutrageRoundDelays : broadside ? BroadsideRoundDelays : QuarterRoundDelays;

			skill.Run(this.Fire(skill, caster, roundDelays, !outrage));
		}

		/// <summary>
		/// Fires the rounds, into one quarter at a time or into the whole
		/// circle.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="roundDelays"></param>
		/// <param name="byQuarter"></param>
		/// <returns></returns>
		private async Task Fire(Skill skill, ICombatEntity caster, int[] roundDelays, bool byQuarter)
		{
			var direction = caster.Direction;

			for (var round = 0; round < roundDelays.Length; round++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(roundDelays[round]));

				if (caster.IsDead)
					return;

				IShapeF area = byQuarter
					? new Fan(caster.Position, direction.AddDegreeAngle(QuarterAngle * round), Range, QuarterAngle)
					: new Circle(caster.Position, Range);

				var hits = new List<SkillHitInfo>();
				var targets = caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill).ToList();

				foreach (var target in targets)
				{
					hits.Add(this.Shoot(skill, caster, target));

					var ricochet = this.TryRicochet(skill, caster, target, targets);
					if (ricochet != null)
						hits.Add(ricochet);
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}

		/// <summary>
		/// Shoots the target with one round of five shots.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		private SkillHitInfo Shoot(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var modifier = BulletMarkerSkillHelper.CreateModifier(caster);
			modifier.HitCount = HitsPerRound;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			return new SkillHitInfo(caster, target, skill, skillHitResult, HitDelay, TimeSpan.Zero);
		}

		/// <summary>
		/// With Bloody Overdrive: Ricochet, may bounce a round off the target
		/// into an enemy next to it that the round didn't hit.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="roundTargets"></param>
		/// <returns></returns>
		private SkillHitInfo TryRicochet(Skill skill, ICombatEntity caster, ICombatEntity target, List<ICombatEntity> roundTargets)
		{
			if (!caster.TryGetActiveAbilityLevel(AbilityId.Bulletmarker8, out var level))
				return null;

			if (GameRandom.Get().Next(100) >= level * RicochetChancePerLevel)
				return null;

			var victim = caster.Map.GetAttackableEnemiesInPosition(caster, target.Position, RicochetRange).FirstOrDefault(a => !roundTargets.Contains(a));
			if (victim == null)
				return null;

			return this.Shoot(skill, caster, victim);
		}
	}
}
