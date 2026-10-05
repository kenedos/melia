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

	/// Handler for Rune Caster skill Rune of Earth.

	/// SkillId: 21302

	/// ClassName: RuneCaster_Isa

	///

	/// Behavior:

	/// - MELEE_GROUND magic attack.

	/// - Deals 8 hits to enemies in the targeted area.

	/// - Has 2 overheats from skill.ies.

	/// - Supports RuneCaster9: Rune of Earth Enhance.

	/// </summary>

	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Isa)]
	public class RuneCaster_IsaOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int HitCount = 8;
		private const int AreaRadius = 50;

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
			skill.Run(this.HandleSkill(caster, skill, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPosition)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var aniTime = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;
			var hasSlowAbility = caster.TryGetAbility(AbilityId.RuneCaster2, out var slowAbility);
			var area = new Circle(targetPosition, AreaRadius);
			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.ToList();

			foreach (var target in targets)
			{
				for (var i = 0; i < HitCount; i++)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					RuneCasterFriendlyHelper.Apply(caster, skill, skillHitResult);
					this.ApplyRuneOfEarthEnhance(caster, skillHitResult);

					if (caster is Character artsCharacter && artsCharacter.IsAbilityActive(AbilityId.RuneCaster15))
						skillHitResult.Damage *= 1.25f;

					target.TakeDamage(skillHitResult.Damage, caster);

					if (hasSlowAbility)
					{
						target.StartBuff(
							BuffId.RuneOfEarth_Slow_Debuff,
							skill.Level,
							slowAbility.Level,
							TimeSpan.FromSeconds(5),
							caster);
					}
					var skillHit = new SkillHitInfo(
						caster,
						target,
						skill,
						skillHitResult,
						aniTime,
						skillHitDelay);

					Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
				}
			}

			if (caster is Character character)
				RuneCasterSkilledCastingHelper.Apply(character, skill);

			caster.SetAttackState(false);
		}

		private void ApplyRuneOfEarthEnhance(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster9, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
