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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Sage
{
	/// <summary>
	/// Handler for the Sage skill Dimension Compression, which compresses the
	/// space around an enemy, striking it 4 times and pulling the enemies
	/// around it in.
	/// </summary>
	/// <remarks>
	/// [Arts] Dimension Compression: Gravity Sphere fires a slow sphere
	/// forward instead, which pulls enemies in and strikes them once.
	/// Dimension Compression: Aftermath may stun the enemies hit.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Sage_DimensionCompression)]
	public class Sage_DimensionCompressionOverride : ITargetGroundSkillHandler, IDynamicCasted
	{
		public const int StunChancePerLevel = 10;
		public static readonly TimeSpan StunDuration = TimeSpan.FromMilliseconds(1500);
		private const float PullRange = 300f;
		private const int PullVelocity = 150;
		private const float SphereOffset = 30f;
		private const float SphereRange = 15f;
		private const float SphereDistance = 300f;
		private const float SphereSpeed = 60f;
		private static readonly TimeSpan[] HitTimes = [TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150), TimeSpan.FromMilliseconds(200)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var gravitySphere = caster.IsAbilityActive(AbilityId.Sage21);

			if (!gravitySphere && (target == null || target.IsDead || !caster.IsEnemy(target)))
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (gravitySphere)
			{
				Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

				this.FireSphere(skill, caster);
				return;
			}

			caster.TurnTowards(target);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, target.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, ForceId.GetNew(), null);

			skill.Run(this.Compress(skill, caster, target));
		}

		/// <summary>
		/// Strikes the target once per compression, pulling the enemies
		/// around it in on the first.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		private async Task Compress(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var maxPulled = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			var pulled = caster.Map.GetAttackableEnemiesInPosition(caster, target.Position, PullRange).Where(e => e != target).Take(maxPulled).ToList();
			var elapsed = TimeSpan.Zero;

			foreach (var hitTime in HitTimes)
			{
				await skill.Wait(hitTime - elapsed);
				elapsed = hitTime;

				if (caster.IsDead || target.IsDead)
					break;

				var hits = new List<SkillHitInfo>();

				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

				if (hitTime == HitTimes[0])
				{
					foreach (var enemy in pulled)
					{
						if (!enemy.IsKnockdownable())
							continue;

						var pullFrom = enemy.Position.GetRelative(target.Position.GetDirection(enemy.Position), PullRange);
						var pullHit = new SkillHitInfo(caster, enemy, skill, new SkillHitResult(), TimeSpan.Zero, TimeSpan.Zero);
						pullHit.KnockBackInfo = new KnockBackInfo(pullFrom, enemy, KnockBackType.KnockBack, PullVelocity, 10);
						pullHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
						enemy.ApplyKnockback(caster, skill, pullHit);

						hits.Add(pullHit);
					}
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);
			}

			if (caster.IsDead || !caster.TryGetActiveAbilityLevel(AbilityId.Sage18, out var aftermathLevel) || aftermathLevel <= 0)
				return;

			foreach (var enemy in pulled.Append(target))
			{
				if (!enemy.IsDead && GameRandom.Get().Next(100) < aftermathLevel * StunChancePerLevel)
					enemy.StartBuff(BuffId.Stun, 1, 0, StunDuration, caster, skill.Id);
			}
		}

		/// <summary>
		/// Fires the Gravity Sphere forward from in front of the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		private void FireSphere(Skill skill, ICombatEntity caster)
		{
			var start = caster.Position.GetRelative(caster.Direction, SphereOffset);

			var pad = new Pad(PadName.Sage_DimensionCompression_Abil, caster, skill, new Circle(start, SphereRange));
			pad.Position = start;
			pad.Direction = caster.Direction;
			pad.Movement.Speed = SphereSpeed;
			caster.Map.AddPad(pad);

			var destination = caster.Map.Ground.GetLastValidPosition(start, start.GetRelative(caster.Direction, SphereDistance));
			skill.RunFree(pad.Movement.MoveToAndDestroy(destination));
		}
	}
}
