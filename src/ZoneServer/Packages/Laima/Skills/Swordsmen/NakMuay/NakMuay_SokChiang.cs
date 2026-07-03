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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for Nak Muay skill Sok Chiang.
	/// Requires Ram Muay stance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.NakMuay_SokChiang)]
	public class NakMuay_SokChiangOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			this.Cast(skill, caster, caster.Position, target?.Position ?? caster.Position, new[] { target });
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, new[] { target });
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IEnumerable<ICombatEntity> targets)
		{
			if (!caster.TryGetBuff(BuffId.RamMuay_Buff, out _))
			{
				caster.ServerMessage(Localization.Get("Ram Muay is required."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var validTargets = targets
				.Where(target => target != null && !target.IsDead)
				.Take(1)
				.ToList();

			if (validTargets.Count == 0)
				return;

			skill.IncreaseOverheat();
			caster.TurnTowards(validTargets[0]);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, validTargets[0].Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, validTargets[0]);

			skill.Run(this.HandleSkill(caster, skill, validTargets));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, IList<ICombatEntity> targets)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(400));

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);

				if (caster is Character character && character.Abilities.TryGet(AbilityId.NakMuay2, out var ability) &&	ability.Active)
				{
					var enhanceRate = ability.Level * 0.005f;

					if (ability.Level >= 100)
						enhanceRate += 0.10f;

					skillHitResult.Damage *= 1f + enhanceRate;
				}

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(
					caster,
					target,
					skill,
					skillHitResult,
					TimeSpan.FromMilliseconds(20),
					TimeSpan.Zero);

				Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
			}

			caster.SetAttackState(false);
		}
	}
}
