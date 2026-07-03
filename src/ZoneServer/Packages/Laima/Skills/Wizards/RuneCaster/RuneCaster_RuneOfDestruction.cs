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
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.Helpers.Wizards.RuneCaster;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune Caster skill Rune of Destruction.
	/// SkillId: 21301
	/// ClassName: RuneCaster_Hagalaz
	///
	/// Behavior:
	/// - MELEE_GROUND magic attack.
	/// - Deals 5 hits to enemies in the targeted area.
	/// - Uses the default Melia combat formula.
	/// - Supports RuneCaster3: Rune of Destruction Enhance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Hagalaz)]
	public class RuneCaster_HagalazOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
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
				.Where(target => target != null && !target.IsDead)
				.ToList();

			if (validTargets.Count == 0)
				return;

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, validTargets[0].Handle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, validTargets));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, IList<ICombatEntity> targets)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(150));

			var aniTime = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			var hasMDefAbility = caster.TryGetAbility(AbilityId.RuneCaster8, out var mdefAbility);

			foreach (var target in targets.Where(target => target != null && !target.IsDead))
			{
				for (var i = 0; i < HitCount; i++)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);

					RuneCasterFriendlyHelper.Apply(caster, skill, skillHitResult);

					this.ApplyRuneOfDestructionEnhance(caster, skillHitResult);

					target.TakeDamage(skillHitResult.Damage, caster);

					if (hasMDefAbility)
					{
						target.StartBuff(
							BuffId.RuneOfDestruction_MDef_Debuff,
							skill.Level,
							mdefAbility.Level,
							TimeSpan.FromSeconds(10),
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

		private void ApplyRuneOfDestructionEnhance(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster3, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
