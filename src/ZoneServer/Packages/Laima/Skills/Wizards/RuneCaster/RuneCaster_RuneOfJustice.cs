using System;
using System.Collections.Generic;
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
using Melia.Zone.Skills.Helpers.Wizards.RuneCaster;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune Caster skill Rune of Justice.
	/// Creates a narrow linear area in the aimed direction and hits every enemy inside it.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Tiwaz)]
	public class RuneCaster_TiwazOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int HitCount = 5;
		private const int MaxTargets = 10;
		private const int LineLength = 250;
		private const int LineAngle = 20;
		private static readonly TimeSpan HitInterval = TimeSpan.FromMilliseconds(100);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, target?.Handle ?? 0);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets?.FirstOrDefault(target => target != null)?.Handle ?? 0);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, int targetHandle)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
			skill.Run(this.ExecuteSkill(caster, skill, originPos, farPos));
		}

		private async Task ExecuteSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(400));

			var direction = originPos.GetDirection(farPos);
			var lineArea = new Fan(originPos, direction, LineLength, LineAngle);
			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, lineArea)
				.Where(target => target != null && !target.IsDead)
				.Take(MaxTargets)
				.ToList();

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				foreach (var target in targets.Where(target => !target.IsDead))
				{
					var result = SCR_SkillHit(caster, target, skill);
					RuneCasterFriendlyHelper.Apply(caster, skill, result);
					this.ApplyRuneOfJusticeEnhance(caster, result);
					target.TakeDamage(result.Damage, caster);
					var hit = new SkillHitInfo(caster, target, skill, result, TimeSpan.FromMilliseconds(50), TimeSpan.Zero);
					Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, hit);
				}

				if (hitIndex < HitCount - 1)
					await skill.Wait(HitInterval);
			}

			if (caster is Character character)
				RuneCasterSkilledCastingHelper.Apply(character, skill);

			caster.SetAttackState(false);
		}

		private void ApplyRuneOfJusticeEnhance(ICombatEntity caster, SkillHitResult result)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster5, out var ability) || !ability.Active)
				return;

			var bonus = ability.Level * 0.005f;

			if (ability.Level >= 100)
				bonus += 0.10f;

			result.Damage *= 1f + bonus;
		}
	}
}
