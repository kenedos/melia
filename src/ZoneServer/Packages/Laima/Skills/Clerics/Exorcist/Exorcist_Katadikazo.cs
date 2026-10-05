using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Exorcist
{
	[Package("laima")]
	[SkillHandler(SkillId.Exorcist_Katadikazo)]
	public class Exorcist_Katadikazo : IGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const float AttackRange = 100f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MinimumTargets = 5;
		private const int MaximumTargets = 10;
		private const int CastTimeMilliseconds = 500;
		private const float AquaBenedictaDamageBonus = 0.50f;
		private const int HitCount = 3;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetAttackState(true);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			if (caster is Character character)
			{
				character.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(character);
			}
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("No target location specified."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, farPos);
			skill.Run(this.ExecuteKatadikazo(skill, character, targetPos));
		}

		private async Task ExecuteKatadikazo(Skill skill, Character caster, Position targetPosition)
		{
			try
			{
				await skill.Wait(TimeSpan.FromMilliseconds(CastTimeMilliseconds));

				if (caster.IsDead || caster.Map == null)
					return;

				Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

				var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
				var maximumTargets = Math.Min(MaximumTargets, MinimumTargets + skillLevel / 2);
				var area = new Melia.Zone.Skills.SplashAreas.Circle(targetPosition, AttackRange);
				var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
						.Where(target => target != null && !target.IsDead)
						.OrderBy(target => targetPosition.Get2DDistance(target.Position))
						.Take(maximumTargets)
						.ToList();

				var hits = new List<SkillHitInfo>();

				foreach (var target in targets)
				{
					var modifier = SkillModifier.Default;
					modifier.HitCount = HitCount;
					modifier.DamageMultiplier *= Exorcist_KatadikazoEnhanceAbility.GetDamageMultiplier(caster);

					if (target.IsBuffActive(BuffId.AquaBenedicta_DeBuff))
						modifier.DamageMultiplier *= 1f + AquaBenedictaDamageBonus;

					var result = SCR_SkillHit(caster, target, skill, modifier);

					if (result.Result != HitResultType.Dodge && result.Damage > 0)
						target.TakeDamage(result.Damage, caster);

					hits.Add(new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
			finally
			{
				caster.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(caster);
			}
		}
	}
}
