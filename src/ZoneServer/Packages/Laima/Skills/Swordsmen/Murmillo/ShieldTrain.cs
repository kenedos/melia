using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Murmillo
{
	/// <summary>
	/// Handler for the Murmillo skill Shield Train.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Murmillo_ShieldTrain)]
	public class Murmillo_ShieldTrainOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 4.4258108f);
			var value = skill.GetPVPValue(10);
			var skillTargets = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 100f, 35f, value);
			if (skillTargets == null || skillTargets.Count == 0)
				return;

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			await skill.Wait(TimeSpan.FromMilliseconds(60));

			// First knockback phase - push forward
			var hits1 = new List<SkillHitInfo>();
			foreach (var target in caster.GetTargets())
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);

				if (target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockBack, 200, 10);
					skillHit.HitInfo.Type = HitType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits1.Add(skillHit);
			}
			if (hits1.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits1);

			await skill.Wait(TimeSpan.FromMilliseconds(540));

			// Second knockback phase - pull to center
			var centerPos = caster.Position.GetRelative(caster.Direction, 40f);
			var hits2 = new List<SkillHitInfo>();
			foreach (var target in caster.GetTargets())
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);

				if (target.IsKnockdownable())
				{
					var pullDirection = target.Position.GetDirection(centerPos);
					var pullFromPos = target.Position.GetRelative(pullDirection.Backwards, 50);

					skillHit.KnockBackInfo = new KnockBackInfo(pullFromPos, target, HitType.KnockBack, 200, 10);
					skillHit.HitInfo.Type = HitType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				target.StartBuff(BuffId.ShieldTrain_Debuff, TimeSpan.FromMilliseconds(500), caster);
				hits2.Add(skillHit);
			}
			if (hits2.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits2);

			SkillTargetBuff(skill, caster, skillTargets, BuffId.Common_Hold, 1f, 0f, TimeSpan.FromMilliseconds(3000f));
		}
	}
}
