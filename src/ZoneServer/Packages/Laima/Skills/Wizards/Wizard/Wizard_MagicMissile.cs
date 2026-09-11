using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Extensions;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Wizard
{
	/// <summary>
	/// Handles the Wizard skill Magic Missile.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wizard_MagicMissile)]
	public class Wizard_MagicMissileOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int MaxTargets = 5;
		private const int MissileCount = 3;
		private const int RicochetTargets = 3;
		private const float SubSplashAreaSize = 200;
		private const float RicochetSpeed = 150;
		private const int MinTravelTimeMs = 50;
		private const int MaxTravelTimeMs = 400;

		/// <summary>
		/// Handles the skill, shooting missiles at enemies.
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

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 130, width: 60, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).Take(MaxTargets).ToList();
			var aniTime = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = skill.Properties.HitDelay;

			var skillHits = new List<SkillHitInfo>();

			foreach (var (missileTarget, hitFrameIndex, targetIndex) in GetMissileHits(targets))
			{
				var skillHitResult = SCR_SkillHit(caster, missileTarget, skill);
				missileTarget.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, missileTarget, skill, skillHitResult, aniTime, skillHitDelay);
				skillHit.ForceId = ForceId.GetNew();
				skillHit.HitFrameIndex = hitFrameIndex;
				skillHit.TargetIndex = targetIndex;

				skillHits.Add(skillHit);
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, skillHits);

			skill.Run(this.Ricochet(skill, caster, skillHits));
		}

		/// <summary>
		/// Returns the hits the missiles cause, spread across the skill's
		/// hit frames, together with the index of the frame they belong
		/// to and their index within it.
		/// </summary>
		/// <param name="targets"></param>
		private static IEnumerable<(ICombatEntity Target, byte HitFrameIndex, byte TargetIndex)> GetMissileHits(List<ICombatEntity> targets)
		{
			if (targets.Count == 0)
				yield break;

			// Each missile flies at the hits of one frame, so every frame
			// needs at least one hit for its missile to home in on
			var slotCount = Math.Max(MissileCount, targets.Count);

			for (var hitFrameIndex = 0; hitFrameIndex < MissileCount; ++hitFrameIndex)
			{
				for (var i = hitFrameIndex; i < slotCount; i += MissileCount)
					yield return (targets[i % targets.Count], (byte)hitFrameIndex, (byte)(i / MissileCount));
			}
		}

		/// <summary>
		/// Shoots the ricochet bullets, dealing their damage once they arrive.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="skillHits"></param>
		private async Task Ricochet(Skill skill, ICombatEntity caster, List<SkillHitInfo> skillHits)
		{
			var bullets = new List<(ICombatEntity Target, SkillHitResult Result, int ForceId, TimeSpan Delay)>();

			foreach (var skillHit in skillHits.DistinctBy(a => a.Target))
			{
				var sourceTarget = skillHit.Target;

				var subSplashArea = Square.Centered(sourceTarget.Position, caster.Direction, SubSplashAreaSize, SubSplashAreaSize / 2);
				var subTargets = caster.Map.GetAttackableEnemiesIn(caster, subSplashArea).Where(a => a != sourceTarget).Take(RicochetTargets);

				foreach (var subTarget in subTargets)
				{
					var skillHitResult = SCR_SkillHit(caster, subTarget, skill);
					var forceId = ForceId.GetNew();
					var travelTime = GetTravelTime(sourceTarget.Position.Get2DDistance(subTarget.Position));

					bullets.Add((subTarget, skillHitResult, forceId, travelTime));

					Send.ZC_NORMAL.PlayForceEffect(forceId, caster, sourceTarget, subTarget, "I_force001_yellow", 1, "arrow_cast", "I_explosion004_yellow", 1, "arrow_blow", "SLOW", RicochetSpeed);
				}
			}

			var elapsed = TimeSpan.Zero;

			foreach (var (target, result, forceId, delay) in bullets.OrderBy(a => a.Delay))
			{
				if (delay > elapsed)
				{
					await skill.Wait(delay - elapsed);
					elapsed = delay;
				}

				target.TakeDamage(result.Damage, caster);

				var hit = new HitInfo(caster, target, skill, result.Damage, result.Result);
				hit.ForceId = forceId;

				Send.ZC_HIT_INFO(caster, target, hit);
			}
		}

		/// <summary>
		/// Returns how long a ricochet bullet takes to reach a target
		/// the given distance away.
		/// </summary>
		/// <param name="distance"></param>
		private static TimeSpan GetTravelTime(double distance)
		{
			var progress = Math.Clamp(distance / SubSplashAreaSize, 0, 1);
			var travelTime = MinTravelTimeMs + progress * (MaxTravelTimeMs - MinTravelTimeMs);

			return TimeSpan.FromMilliseconds(travelTime);
		}
	}
}
