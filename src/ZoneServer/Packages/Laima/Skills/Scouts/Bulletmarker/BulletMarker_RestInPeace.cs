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
	/// Handler for Bullet Marker skill R.I.P.
	///
	/// SkillId: 51106
	/// Factor: 41 + 68 per level
	/// Multi Hit: 7
	/// Overheat: 3
	///
	/// Hits all enemies inside a rectangular area
	/// directly in front of the caster.
	/// Requires Double Gun Stance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_RestInPeace)]
	public class BulletMarker_RestInPeace : IGroundSkillHandler
	{
		private const int HitCount = 7;
		private const float RectangleLength = 160f;
		private const float RectangleHalfWidth = 55f;
		private const float SearchRadius = 155f;
		private const int BaseAttackRatio = 10;
		private const int AoEAbilityBonus = 5;

		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(10);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(70);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				character.ServerMessage(Localization.Get("Double Gun Stance must be active."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var consumedOutrageStack = BulletMarkerOverheatingHelper.TryConsumeOutrageStack(character);
			BulletMarkerOverheatingHelper.AddSkillStacks(character, skill);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, originPos, farPos, consumedOutrageStack));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos, bool consumedOutrageStack)
		{
			await skill.Wait(FirstHitDelay);

			var directionX = farPos.X - originPos.X;
			var directionZ = farPos.Z - originPos.Z;

			var directionLength = MathF.Sqrt(directionX * directionX + directionZ * directionZ);

			if (directionLength <= 0.001f)
			{
				caster.SetAttackState(false);
				return;
			}

			directionX /= directionLength;
			directionZ /= directionLength;

			var rightX = -directionZ;
			var rightZ = directionX;

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				var searchArea = new Circle(originPos, SearchRadius);

				var maxTargets = this.GetAttackRatio(caster);

				var targets = caster.Map
					.GetAttackableEnemiesIn(caster, searchArea)
					.Where(target => target != null && !target.IsDead)
					.Where(target => this.IsInsideRectangle(target.Position, originPos, directionX, directionZ, rightX, rightZ))
					.Take(maxTargets)
					.ToList();

				foreach (var target in targets)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);

					this.ApplyEnhanceAbility(caster, skillHitResult);

					if (consumedOutrageStack)
						skillHitResult.Damage *= 1.25f;

					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(
						caster,
						target,
						skill,
						skillHitResult,
						HitAnimationTime,
						TimeSpan.Zero);

					Send.ZC_SKILL_HIT_INFO(caster, skillHit);
				}

				if (hitIndex < HitCount - 1)
					await skill.Wait(DelayBetweenHits);
			}

			caster.SetAttackState(false);
		}

		private bool IsInsideRectangle(
			Position targetPosition,
			Position originPos,
			float directionX,
			float directionZ,
			float rightX,
			float rightZ)
		{
			var offsetX = targetPosition.X - originPos.X;
			var offsetZ = targetPosition.Z - originPos.Z;

			var forwardDistance =
				offsetX * directionX +
				offsetZ * directionZ;

			if (forwardDistance < 0f || forwardDistance > RectangleLength)
				return false;

			var sideDistance =
				offsetX * rightX +
				offsetZ * rightZ;

			return MathF.Abs(sideDistance) <= RectangleHalfWidth;
		}

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker3, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}

		private int GetAttackRatio(ICombatEntity caster)
		{
			var attackRatio = BaseAttackRatio;

			if (caster is Character character && character.Abilities.TryGet(AbilityId.Bulletmarker13, out var ability) && ability.Active)
				attackRatio += AoEAbilityBonus;

			return attackRatio;
		}
	}
}
