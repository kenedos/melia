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
	/// Handler for Rune Caster skill Rune of Repulsion.
	/// Channels for two seconds and deals ten damage pulses in the aimed area.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Thurisaz)]
	public class RuneCaster_ThurisazOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int HitCount = 10;
		private const int MaxTargets = 4;
		private const int AreaRadius = 30;
		private static readonly TimeSpan HitInterval = TimeSpan.FromMilliseconds(200);

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
			skill.Run(this.ChannelSkill(caster, skill, farPos));
		}

		private async Task ChannelSkill(ICombatEntity caster, Skill skill, Position targetPosition)
		{
			var damageArea = new Circle(targetPosition, AreaRadius);

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				await skill.Wait(HitInterval);

				var targets = caster.Map
					.GetAttackableEnemiesIn(caster, damageArea)
					.Where(target => target != null && !target.IsDead)
					.Take(MaxTargets)
					.ToList();

				foreach (var target in targets)
				{
					var result = SCR_SkillHit(caster, target, skill);
					RuneCasterFriendlyHelper.Apply(caster, skill, result);
					this.ApplyRuneOfRepulsionEnhance(caster, result);

					if (caster is Character character && character.IsAbilityActive(AbilityId.RuneCaster24))
						result.Damage *= 1.25f;

					target.TakeDamage(result.Damage, caster);
					var hit = new SkillHitInfo(caster, target, skill, result, TimeSpan.FromMilliseconds(50), TimeSpan.Zero);
					Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, hit);
				}
			}

			caster.SetAttackState(false);
		}

		private void ApplyRuneOfRepulsionEnhance(ICombatEntity caster, SkillHitResult result)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster23, out var ability) || !ability.Active)
				return;

			var bonus = ability.Level * 0.005f;

			if (ability.Level >= 100)
				bonus += 0.10f;

			result.Damage *= 1f + bonus;
		}
	}
}
