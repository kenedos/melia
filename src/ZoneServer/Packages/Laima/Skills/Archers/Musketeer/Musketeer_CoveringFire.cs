using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Covering Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_CoveringFire)]
	public class Musketeer_CoveringFireOverride : IGroundSkillHandler, IDynamicCasted
	{
		TimeSpan DamageDelay = TimeSpan.FromMilliseconds(100);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(5));

			var splashParam = skill.GetSplashParameters(caster, targetPos, targetPos, length: 50, width: 50, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var results = new List<SkillHitResult>();
			var hitTargets = new List<ICombatEntity>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(skill.Data.MultiHitCount);

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);
				results.Add(skillHitResult);

				var hit = new HitInfo(caster, target, skill, skillHitResult);
				hit.ResultType = skillHitResult.Result;
				hit.AniTime = DamageDelay;

				if (skillHitResult.Damage > 0)
				{
					hitTargets.Add(target);
				}

				Send.ZC_HIT_INFO(hit);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}
	}
}
