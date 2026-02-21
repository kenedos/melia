using System;
using System.Collections.Generic;
using System.Linq;
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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Monk
{
	/// <summary>
	/// Handler for the Monk skill Hand Knife.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Monk_HandKnife)]
	public class Monk_HandKnifeOverride : IMeleeGroundSkillHandler
	{
		private const float BaseDefPenetration = 0.10f;
		private const float DefPenetrationPerLevel = 0.01f;
		private const float MaxDefPenetration = 0.40f;

		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(250);
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets?.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var modifier = new SkillModifier();
			modifier.DefensePenetrationRate = Math.Min(BaseDefPenetration + DefPenetrationPerLevel * skill.Level, MaxDefPenetration);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 25, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 50;
			var damageDelay = 250;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, modifySkillHitResult: ModifyResult, skillModifier: modifier);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 25, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 150;
			damageDelay = 250;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, modifySkillHitResult: ModifyResult, skillModifier: modifier);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Monk5, out var abilityLevel))
				SkillResultTargetBuff(caster, skill, BuffId.ArmorBreak, abilityLevel, 0f, 8000, 1, 100, -1, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(250));
			caster.RemoveBuff(BuffId.HandKnife_Buff);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
		}

		private static SkillHitResult ModifyResult(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult skillHitResult)
		{
			if (target.IsKnockedDown())
				skillHitResult.Damage *= 2f;

			return skillHitResult;
		}
	}
}
