using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Bullet Marker skill Napalm Bullet.
	/// SkillId: 51103
	/// Factor: 118 + 20 per level
	/// Multi Hit: 4
	/// Overheat: 2
	/// Hits all enemies inside a cone in front of the caster.
	/// Requires Double Gun Stance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_NapalmBullet)]
	public class BulletMarker_NapalmBullet : IForceSkillHandler
	{
		private const int HitCount = 4;
		private const float ConeRange = 160f;
		private const float ConeHalfAngle = 40f;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(10);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				character.ServerMessage(Localization.Get("Double Gun Stance must be active."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var aimPosition = target != null && !target.IsDead ? target.Position : farPos;

			caster.TurnTowards(aimPosition);
			caster.SetAttackState(true);
			skill.IncreaseOverheat();

			var consumedOutrageStack = BulletMarkerOverheatingHelper.TryConsumeOutrageStack(character);
			BulletMarkerOverheatingHelper.AddSkillStacks(character, skill);

			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, forceId, null);

			skill.Run(this.Attack(skill, caster, originPos, aimPosition, forceId, consumedOutrageStack));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position aimPosition, int forceId, bool consumedOutrageStack)
		{
			await skill.Wait(FirstHitDelay);

			var directionX = aimPosition.X - originPos.X;
			var directionZ = aimPosition.Z - originPos.Z;
			var directionLength = MathF.Sqrt(directionX * directionX + directionZ * directionZ);

			if (directionLength <= 0.001f)
			{
				caster.SetAttackState(false);
				return;
			}

			directionX /= directionLength;
			directionZ /= directionLength;

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				var searchArea = new Circle(originPos, ConeRange);

				var targets = caster.Map
					.GetAttackableEnemiesIn(caster, searchArea)
					.Where(hitTarget => hitTarget != null && !hitTarget.IsDead)
					.Where(hitTarget => this.IsInsideCone(hitTarget.Position, originPos, directionX, directionZ))
					.ToList();

				foreach (var hitTarget in targets)
				{
					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);

					this.ApplyEnhanceAbility(caster, skillHitResult);

					if (consumedOutrageStack)
						skillHitResult.Damage *= 1.25f;

					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, hitTarget, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
					skillHit.ForceId = forceId;

					Send.ZC_SKILL_HIT_INFO(caster, skillHit);
				}

				if (hitIndex < HitCount - 1)
					await skill.Wait(DelayBetweenHits);
			}

			caster.SetAttackState(false);
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

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker1, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
