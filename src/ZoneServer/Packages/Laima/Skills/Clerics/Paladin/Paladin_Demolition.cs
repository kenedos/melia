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
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handler for the Paladin skill Demolition.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Paladin_Demolition)]
	public class Paladin_DemolitionOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(300);
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

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
			var length = 65;
			var width = 50;
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 100;
			var damageDelay = 300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ModifySkillHitResult);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, skill.Level, 0f, 4000, 1, 5 + skill.Level, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ModifySkillHitResult);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, skill.Level, 0f, 4000, 1, 5 + skill.Level, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 500;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ModifySkillHitResult);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, skill.Level, 0f, 4000, 1, 5 + skill.Level, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 300;
			damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ModifySkillHitResult);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, skill.Level, 0f, 4000, 1, 5 + skill.Level, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ModifySkillHitResult);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, skill.Level, 0f, 4000, 1, 5 + skill.Level, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ModifySkillHitResult);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, skill.Level, 0f, 4000, 1, 5 + skill.Level, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: width, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 400;
			damageDelay = 1400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ModifySkillHitResult);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, skill.Level, 0f, 4000, 1, 5 + skill.Level, -1, hits);
			var targetPos = originPos.GetRelative(farPos, distance: 0);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 50f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			SkillTargetBuffAbility(caster, skill, AbilityId.Paladin39, BuffId.Demolition_Buff, skill.Level, -1, 10000, 0, 1, 100);
		}

		/// <summary>
		/// Applies bonus damage if target has conviction debuff
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private SkillHitResult ModifySkillHitResult(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult result)
		{
			if (target.IsBuffActive(BuffId.Conviction_Debuff))
				result.Damage *= 1.20f;

			return result;
		}
	}
}
