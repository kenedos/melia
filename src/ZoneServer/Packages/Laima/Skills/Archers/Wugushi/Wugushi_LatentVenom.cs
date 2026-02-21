using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Wugushi skill Latent Venom.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wugushi_LatentVenom)]
	public class Wugushi_LatentVenomOverride : IForceSkillHandler
	{
		private const int DamageDelayMs = 300;
		private const int BaseLatentDurationMs = 40000;
		private const int StacksPerUse = 2;
		private const float SplashLength = 30f;
		private const float SplashWidth = 30f;
		private const float SplashAngle = 10f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			skillHit.ForceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos, target, skillHitResult));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos, ICombatEntity primaryTarget, SkillHitResult primaryHitResult)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: SplashLength, width: SplashWidth, angle: SplashAngle);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var hits = new List<SkillHitInfo>();

			var primaryHit = new SkillHitInfo(caster, primaryTarget, skill, primaryHitResult, TimeSpan.FromMilliseconds(DamageDelayMs), TimeSpan.Zero);
			primaryHit.ForceId = ForceId.GetNew();
			hits.Add(primaryHit);

			await skill.Wait(TimeSpan.FromMilliseconds(DamageDelayMs));

			var stacks = StacksPerUse;

			SkillResultTargetBuff(caster, skill, BuffId.LatentVenom_Debuff, stacks, 0f, BaseLatentDurationMs, 1, 100, -1, hits);
		}
	}
}
