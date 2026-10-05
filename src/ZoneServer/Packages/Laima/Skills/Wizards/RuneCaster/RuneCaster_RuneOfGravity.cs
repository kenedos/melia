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

	/// Handler for Rune Caster skill Rune of Gravity.

	/// SkillId: 21307

	/// ClassName: RuneCaster_Ehwaz

	///

	/// Behavior:

	/// - MELEE_GROUND magic attack.

	/// - Creates a fast spinning Psychokinesis sphere around the caster.

	/// - Deals 20 hits over 5 seconds.

	/// - Supports RuneCaster20: Rune of Gravity Enhance.

	/// </summary>

	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Ehwaz)]
	public class RuneCaster_EhwazOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int HitCount = 20;
		private const int MaxTargets = 30;
		private const int AreaRadius = 100;
		private static readonly TimeSpan HitInterval = TimeSpan.FromMilliseconds(250);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(250));

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				var area = new Circle(caster.Position, AreaRadius);
				var targets = caster.Map
					.GetAttackableEnemiesIn(caster, area)
					.Where(target => target != null && !target.IsDead)
					.Take(MaxTargets)
					.ToList();

				foreach (var target in targets)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					RuneCasterFriendlyHelper.Apply(caster, skill, skillHitResult);
					this.ApplyRuneOfGravityEnhance(caster, skillHitResult);
					target.TakeDamage(skillHitResult.Damage, caster);
					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(50), TimeSpan.Zero);
					Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
				}

				if (hitIndex < HitCount - 1)
					await skill.Wait(HitInterval);
			}

			if (caster is Character character)
				RuneCasterSkilledCastingHelper.Apply(character, skill);

			caster.SetAttackState(false);
		}

		private void ApplyRuneOfGravityEnhance(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster20, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
