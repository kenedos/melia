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
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Cannon Shot
	/// Causa dois hits no alvo principal e nos inimigos próximos.
	/// Chain Explosion adiciona três hits e reduz o dano.
	/// Bazooka aumenta alcance, quantidade de alvos, dano e cooldown.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cannoneer_CannonShot)]
	public class Cannoneer_CannonShotOverride : IForceSkillHandler, IDynamicCasted
	{
		private const int BaseHitCount = 2;
		private const int BaseMaximumTargets = 5;
		private const float NormalMaximumRange = 160f;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(80);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(20);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float castTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity designatedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (designatedTarget == null || designatedTarget.IsDead)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!CannoneerBazookaHelper.IsInsideAllowedDistance(character, originPos, designatedTarget.Position, NormalMaximumRange))
			{
				character.ServerMessage(Localization.Get("The target is outside the valid attack distance."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			Log.Info("Cannon Shot before overheat: Counter={0}, Max={1}, Data={2}, Cooldown={3}", skill.OverheatCounter, skill.OverheatMaxCount, skill.Data.OverheatCount, skill.IsOnCooldown);

			skill.IncreaseOverheat(2, skill.OverheatCooldown);

			Log.Info("Cannon Shot after overheat: Counter={0}, Max={1}, Data={2}, Cooldown={3}", skill.OverheatCounter, skill.OverheatMaxCount, skill.Data.OverheatCount, skill.IsOnCooldown);

			CannoneerBazookaHelper.ApplyCooldownMultiplier(character, skill);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var forceId = ForceId.GetNew();

			caster.SetAttackState(true);
			caster.TurnTowards(designatedTarget);

			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, designatedTarget.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, designatedTarget.Position);
			Send.ZC_SKILL_FORCE_TARGET(caster, designatedTarget, skill);

			skill.Run(this.Attack(skill, character, designatedTarget, forceId));
			skill.Run(this.ReleaseAttackState(skill, character));
		}

		private async Task Attack(Skill skill, Character character, ICombatEntity designatedTarget, int forceId)
		{
			await skill.Wait(FirstHitDelay);

			var hitCount = BaseHitCount + Cannoneer_CannonShotChainExplosionAbility.GetAdditionalHitCount(character);
			var chainExplosionMultiplier = Cannoneer_CannonShotChainExplosionAbility.GetDamageMultiplier(character);
			var enhanceMultiplier = Cannoneer_CannonShotEnhanceAbility.GetDamageMultiplier(character);
			var bazookaMultiplier = CannoneerBazookaHelper.GetFinalDamageMultiplier(character);
			var damageMultiplier = chainExplosionMultiplier * enhanceMultiplier * bazookaMultiplier;

			for (var hitIndex = 0; hitIndex < hitCount; hitIndex++)
			{
				if (character.IsDead || character.Map == null || designatedTarget == null || designatedTarget.IsDead || designatedTarget.Map != character.Map)
					break;

				var targets = this.GetTargets(skill, character, designatedTarget);

				foreach (var target in targets)
					this.ApplyHit(skill, character, target, damageMultiplier, forceId);

				if (hitIndex + 1 < hitCount)
					await skill.Wait(DelayBetweenHits);
			}
		}

		private async Task ReleaseAttackState(Skill skill, Character character)
		{
			try
			{
				await skill.Wait(TimeSpan.FromMilliseconds(450));
			}
			finally
			{
				if (character != null)
				{
					character.SetAttackState(false);
					Send.ZC_SKILL_DISABLE(character);
				}
			}
		}

		private IList<ICombatEntity> GetTargets(Skill skill, Character character, ICombatEntity designatedTarget)
		{
			var splashRange = Math.Max(1f, skill.Properties.GetFloat(PropertyName.SplRange));
			var maximumTargets = BaseMaximumTargets + CannoneerBazookaHelper.GetAdditionalAoeAttackRatio(character);
			var splashArea = new CircleF(designatedTarget.Position, splashRange);

			var targets = character.Map.GetAttackableEnemiesIn(character, splashArea)
				.Where(target => target != null && !target.IsDead)
				.Distinct()
				.OrderBy(target => target == designatedTarget ? 0 : 1)
				.ThenBy(target => target.Position.Get2DDistance(designatedTarget.Position))
				.Take(maximumTargets)
				.ToList();

			if (!targets.Contains(designatedTarget))
			{
				if (targets.Count >= maximumTargets)
					targets.RemoveAt(targets.Count - 1);

				targets.Insert(0, designatedTarget);
			}

			return targets;
		}

		private void ApplyHit(Skill skill, Character character, ICombatEntity target, float damageMultiplier, int forceId)
		{
			if (target == null || target.IsDead)
				return;

			var skillHitResult = SCR_SkillHit(character, target, skill);
			skillHitResult.Damage = Math.Max(1, (int)(skillHitResult.Damage * damageMultiplier));
			target.TakeDamage(skillHitResult.Damage, character);

			var skillHit = new SkillHitInfo(character, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
			skillHit.ForceId = forceId;

			Send.ZC_SKILL_HIT_INFO(character, skillHit);
		}
	}
}
