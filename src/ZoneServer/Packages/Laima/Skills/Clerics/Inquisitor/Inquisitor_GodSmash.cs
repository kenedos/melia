using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Inquisitor
{
	[Package("laima")]
	[SkillHandler(SkillId.Inquisitor_GodSmash)]
	public class Inquisitor_GodSmash : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumTargets = 7;
		private const float AttackRadius = 40f;
		private const float DefensePenetrationRate = 0.15f;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(200);
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
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			var attackPosition = target != null && !target.IsDead ? target.Position : farPos;

			character.TurnTowards(attackPosition);
			character.SetAttackState(true);
			skill.IncreaseOverheat();

			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, attackPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(character, target?.Handle ?? 0, originPos, character.Direction, attackPosition);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, attackPosition, forceId, null);

			skill.Run(this.Attack(skill, character, attackPosition, forceId));
		}

		private async Task Attack(Skill skill, Character caster, Position attackPosition, int forceId)
		{
			try
			{
				await skill.Wait(FirstHitDelay);

				if (caster.IsDead || caster.Map == null)
					return;

				var targets = this.GetTargets(caster, attackPosition);

				if (targets.Count == 0)
					return;

				var enhanceMultiplier = Inquisitor_GodSmashEnhanceAbility.GetDamageMultiplier(caster);
				var demonPunisherMultiplier = Inquisitor_GodSmashDemonPunisherAbility.GetDamageMultiplier(caster);
				var hits = new List<SkillHitInfo>();

				foreach (var target in targets)
				{
					if (target == null || target.IsDead)
						continue;

					var modifier = SkillModifier.Default;
					modifier.DefensePenetrationRate += DefensePenetrationRate;
					modifier.DamageMultiplier *= enhanceMultiplier;
					modifier.DamageMultiplier *= demonPunisherMultiplier;

					if (caster.IsAbilityActive(AbilityId.Inquisitor18) && target.IsBuffActive(BuffId.IronMaiden_Debuff))
						modifier.DamageMultiplier *= 2f;

					var result = SCR_SkillHit(caster, target, skill, modifier);

					if (result.Result != HitResultType.Dodge && result.Damage > 0)
						target.TakeDamage(result.Damage, caster);

					var hit = new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero);
					hit.ForceId = forceId;
					hits.Add(hit);
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
			finally
			{
				caster.SetAttackState(false);
				Send.ZC_NORMAL.Skill_45(caster);
				Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
				Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			}
		}

		private IList<ICombatEntity> GetTargets(ICombatEntity caster, Position attackPosition)
		{
			var area = new Circle(attackPosition, AttackRadius);

			return caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.Distinct()
				.OrderBy(target => attackPosition.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();
		}
	}
}
