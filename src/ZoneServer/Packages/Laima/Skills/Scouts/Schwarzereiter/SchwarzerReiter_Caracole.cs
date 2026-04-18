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
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzereiter skill Caracole.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_Caracole)]
	public class SchwarzerReiter_CaracoleOverride : IGroundSkillHandler, IDynamicCasted
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(200);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 20);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 0;
			var damageDelay = 200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			if (caster.TryGetActiveAbility(AbilityId.Schwarzereiter16, out var ability))
				caster.StartBuff(BuffId.Caracole_Silence_Debuff, 1f, 0f, TimeSpan.FromMilliseconds(1f), caster, skill.Id);
			var value = 500 * skill.Level;
			if (caster.IsAbilityActive(AbilityId.Schwarzereiter16))
				value *= 2;
			SkillResultTargetBuff(caster, skill, BuffId.Caracole_Silence_Debuff, skill.Level, 0f, value, 1, 100, -1, hits);
			value = 1000 * skill.Level;
			SkillResultTargetBuff(caster, skill, BuffId.Caracole_HR_Debuff, skill.Level, 0f, value, 1, 100, -1, hits);
			if (caster.IsAbilityActive(AbilityId.Schwarzereiter17))
			{
				foreach (var hit in hits)
				{
					var hitTarget = hit.Target;
					if (hitTarget == null || !hitTarget.IsDead) continue;
				}
			}
		}
	}
}
