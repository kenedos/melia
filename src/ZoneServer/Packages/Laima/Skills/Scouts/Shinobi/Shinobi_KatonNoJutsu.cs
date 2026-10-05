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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Shinobi
{
	[Package("laima")]
	[SkillHandler(SkillId.Shinobi_Katon_no_jutsu)]
	public class Shinobi_KatonNoJutsu : IGroundSkillHandler
	{
		private const float SkillRange = 150f;
		private const float ConeHalfAngle = 50f;
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(50);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (originPos.Get2DDistance(farPos) > SkillRange)
			{
				if (caster is not DummyCharacter)
					character.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (caster is not DummyCharacter && !caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if (caster is not DummyCharacter)
				skill.IncreaseOverheat();

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, originPos, farPos));

			if (character is not DummyCharacter)
				ShinobiBunshinHelper.ReplicateSkill(character, skill, originPos, farPos, target);
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			await skill.Wait(HitDelay);

			var directionX = farPos.X - originPos.X;
			var directionZ = farPos.Z - originPos.Z;
			var length = MathF.Sqrt(directionX * directionX + directionZ * directionZ);

			if (length <= 0f)
			{
				caster.SetAttackState(false);
				return;
			}

			directionX /= length;
			directionZ /= length;

			var searchArea = new Circle(originPos, SkillRange);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, searchArea).Where(target => target != null && !target.IsDead).Where(target => this.IsInsideCone(target.Position, originPos, directionX, directionZ)).ToList();

			foreach (var target in targets)
			{
				if (target == null || target.IsDead)
					continue;

				var skillHitResult = SCR_SkillHit(caster, target, skill);

				this.ApplyEnhanceAbility(caster, skillHitResult);
				this.ApplyBunshinDamageModifier(caster, skillHitResult);

				target.TakeDamage(skillHitResult.Damage, caster);
				ShinobiBunshinGenHelper.TryApply(caster, target, skill);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
				Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
			}

			caster.SetAttackState(false);
		}

		private bool IsInsideCone(Position targetPos, Position originPos, float directionX, float directionZ)
		{
			var targetX = targetPos.X - originPos.X;
			var targetZ = targetPos.Z - originPos.Z;
			var targetDistance = MathF.Sqrt(targetX * targetX + targetZ * targetZ);

			if (targetDistance <= 0f || targetDistance > SkillRange)
				return false;

			targetX /= targetDistance;
			targetZ /= targetDistance;

			var dot = directionX * targetX + directionZ * targetZ;
			var minimumDot = MathF.Cos(ConeHalfAngle * MathF.PI / 180f);

			return dot >= minimumDot;
		}

		private void ApplyBunshinDamageModifier(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is DummyCharacter)
				skillHitResult.Damage *= 0.25f;
		}

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			Character character;

			if (caster is DummyCharacter dummy)
				character = dummy.Owner;
			else
				character = caster as Character;

			if (character == null)
				return;

			if (!character.Abilities.TryGet(AbilityId.Shinobi2, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
