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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handler for the Highlander skill Crown.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Highlander_Moulinet)]
	public class Highlander_MoulinetOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Applies bonus damage if target is bleeding
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private SkillHitResult ModifySkillHitResult(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult result)
		{
			if (target.IsBuffActiveByKeyword(BuffTag.Wound))
				result.Damage *= 1.5f;

			return result;
		}

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 40, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var t in targetList.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, t, skill);

				t.TakeDamage(skillHitResult.Damage, caster);
				var skillHit = new SkillHitInfo(caster, t, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				hits.Add(skillHit);
			}
			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 60;
			var aniTime = 0;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, modifySkillHitResult: this.ModifySkillHitResult);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 200;
			aniTime = 0;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, modifySkillHitResult: this.ModifySkillHitResult);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 300;
			aniTime = 100;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, modifySkillHitResult: this.ModifySkillHitResult);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 440;
			aniTime = 140;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, modifySkillHitResult: this.ModifySkillHitResult);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 700;
			aniTime = 160;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, modifySkillHitResult: this.ModifySkillHitResult);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 950;
			aniTime = 250;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, modifySkillHitResult: this.ModifySkillHitResult);
		}
	}
}
