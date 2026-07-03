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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune Caster skill Rune of Rock.
	/// SkillId: 21306
	/// ClassName: RuneCaster_Stan
	///
	/// Original ToS behavior:
	/// - Magic ground-targeted AoE.
	/// - Deals 5 consecutive hits.
	/// - Uses the standard Melia magic damage formula.
	/// - Supports Rune of Rock: Enhance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Stan)]
	public class RuneCaster_StanOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int HitCount = 5;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var targets = new List<ICombatEntity>();

			if (target != null)
				targets.Add(target);

			this.Cast(skill, caster, originPos, farPos, targets);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var validTargets = targets
				.Where(a => a != null && !a.IsDead)
				.ToList();

			if (validTargets.Count == 0)
				return;

			skill.IncreaseOverheat();

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(
				caster,
				validTargets[0].Handle,
				originPos,
				originPos.GetDirection(farPos),
				Position.Zero);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.ExecuteSkill(caster, skill, validTargets));
		}

		private async Task ExecuteSkill(ICombatEntity caster, Skill skill, IList<ICombatEntity> targets)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(400));

			var hitAnimation = TimeSpan.FromMilliseconds(50);

			foreach (var target in targets)
			{
				for (var i = 0; i < HitCount; i++)
				{
					var result = SCR_SkillHit(caster, target, skill);

					this.ApplyRuneOfRockEnhance(caster, result);

					target.TakeDamage(result.Damage, caster);

					var hit = new SkillHitInfo(
						caster,
						target,
						skill,
						result,
						hitAnimation,
						TimeSpan.Zero);

					Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, hit);
				}
			}

			if (caster is Character character)
				RuneCasterSkilledCastingHelper.Apply(character, skill);

			caster.SetAttackState(false);
		}

		private void ApplyRuneOfRockEnhance(ICombatEntity caster, SkillHitResult result)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster6, out var ability) || !ability.Active)
				return;

			var bonus = ability.Level * 0.005f;

			if (ability.Level >= 100)
				bonus += 0.10f;

			result.Damage *= 1f + bonus;
		}
	}
}
