using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Abilities.Handlers.Archers.Cannoneer;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>

	/// Cannon Blast

	/// Canaliza por 1,5 segundo e causa dois hits a cada 0,7 segundo

	/// em até 15 inimigos dentro de um cone à frente do personagem.

	/// </summary>

	[Package("laima")]
	[SkillHandler(SkillId.Cannoneer_CannonBlast)]
	public class Cannoneer_CannonBlastOverride : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int DamageCycles = 3;
		private const int HitsPerCycle = 2;
		private const int MaximumTargets = 15;
		private const float ConeRange = 160f;
		private const float ConeHalfAngle = 60f;
		private const float KnockbackPower = 50f;
		private const float KnockbackVerticalAngle = 10f;
		private static readonly TimeSpan DelayBetweenCycles = TimeSpan.FromMilliseconds(700);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(80);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, target);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets?.FirstOrDefault());
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var aimPosition = target != null && !target.IsDead ? target.Position : farPos;
			var directionX = aimPosition.X - originPos.X;
			var directionZ = aimPosition.Z - originPos.Z;
			var directionLength = MathF.Sqrt(directionX * directionX + directionZ * directionZ);

			if (directionLength <= 0.001f)
			{
				directionX = caster.Direction.Cos;
				directionZ = caster.Direction.Sin;
			}
			else
			{
				directionX /= directionLength;
				directionZ /= directionLength;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(aimPosition);
			caster.SetAttackState(true);

			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, aimPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, aimPosition, forceId, null);

			skill.Run(this.Channel(skill, character, originPos, directionX, directionZ, forceId));
		}

		private async Task Channel(Skill skill, Character caster, Position originPos, float directionX, float directionZ, int forceId)
		{
			try
			{
				var damageMultiplier = Cannoneer_CannonBlastEnhanceAbility.GetDamageMultiplier(caster);
				var armorBreakTargets = new HashSet<ICombatEntity>();

				for (var cycleIndex = 0; cycleIndex < DamageCycles; cycleIndex++)
				{
					if (caster.IsDead)
						break;

					var targets = this.GetTargets(caster, originPos, directionX, directionZ);
					var applyKnockback = cycleIndex == DamageCycles - 1;

					foreach (var target in targets)
					{
						if (armorBreakTargets.Add(target))
							Cannoneer_CannonBlastArmorBreakAbility.TryApplyArmorBreak(caster, target);
					}

					for (var hitIndex = 0; hitIndex < HitsPerCycle; hitIndex++)
					{
						if (caster.IsDead)
							break;

						var hits = new List<SkillHitInfo>();

						foreach (var target in targets)
						{
							if (target == null || target.IsDead)
								continue;

							var skillHitResult = SCR_SkillHit(caster, target, skill);
							skillHitResult.Damage = Math.Max(1, (int)(skillHitResult.Damage * damageMultiplier));
							target.TakeDamage(skillHitResult.Damage, caster);

							var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
							skillHit.ForceId = forceId;

							if (applyKnockback && hitIndex == HitsPerCycle - 1 && skillHitResult.Damage > 0 && target.IsKnockdownable())
							{
								skillHit.KnockBackInfo = new KnockBackInfo(caster, KnockBackType.KnockBack, (int)KnockbackPower, (int)KnockbackVerticalAngle, caster.Direction);
								skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
								target.ApplyKnockback(caster, skill, skillHit);
							}

							hits.Add(skillHit);
						}

						if (hits.Count > 0)
							Send.ZC_SKILL_HIT_INFO(caster, hits);

						if (hitIndex + 1 < HitsPerCycle)
							await skill.Wait(DelayBetweenHits);
					}

					if (cycleIndex + 1 < DamageCycles)
					{
						var consumedHitTime = TimeSpan.FromMilliseconds(DelayBetweenHits.TotalMilliseconds * (HitsPerCycle - 1));
						var remainingCycleTime = DelayBetweenCycles - consumedHitTime;

						if (remainingCycleTime > TimeSpan.Zero)
							await skill.Wait(remainingCycleTime);
					}
				}
			}
			finally
			{
				caster.SetAttackState(false);
				Send.ZC_NORMAL.Skill_45(caster);
				Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
				Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			}
		}

		private List<ICombatEntity> GetTargets(ICombatEntity caster, Position originPos, float directionX, float directionZ)
		{
			var searchArea = new Circle(originPos, ConeRange);

			return caster.Map.GetAttackableEnemiesIn(caster, searchArea)
				.Where(target => target != null && !target.IsDead)
				.Where(target => this.IsInsideCone(target.Position, originPos, directionX, directionZ))
				.OrderBy(target => target.Position.Get2DDistance(originPos))
				.Take(MaximumTargets)
				.ToList();
		}

		private bool IsInsideCone(Position targetPosition, Position originPosition, float directionX, float directionZ)
		{
			var offsetX = targetPosition.X - originPosition.X;
			var offsetZ = targetPosition.Z - originPosition.Z;
			var distance = MathF.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

			if (distance <= 0.001f)
				return true;

			if (distance > ConeRange)
				return false;

			offsetX /= distance;
			offsetZ /= distance;

			var dot = directionX * offsetX + directionZ * offsetZ;
			var minimumDot = MathF.Cos(ConeHalfAngle * MathF.PI / 180f);

			return dot >= minimumDot;
		}
	}
}
