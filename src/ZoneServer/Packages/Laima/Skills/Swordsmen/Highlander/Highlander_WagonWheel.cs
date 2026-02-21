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
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handler for the Highlander skill Wagon Wheel
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Highlander_WagonWheel)]
	public class Highlander_WagonWheelOverride : IMeleeGroundSkillHandler
	{
		private const float BleedDamage = 0.2f;
		private const float BleedDuration = 5000;

		/// <summary>
		/// Applies bonus damage if caster has crossguard buff
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private SkillHitResult ModifySkillHitResult(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult result)
		{
			if (caster.IsBuffActive(BuffId.CrossGuard_Damage_Buff))
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
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();

			var hitDelay = 0;
			var damageDelay = 175;
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, modifySkillHitResult: this.ModifySkillHitResult);
			foreach (var hit in hits)
			{
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.KnockDown, KnockDirection.TowardsTarget, 250, 85f, 1, 4, 2);
				var bleedDamage = hit.HitInfo.Damage * BleedDamage;
				bleedDamage = Math.Max(1, bleedDamage);
				SkillResultTargetBuff(caster, skill, BuffId.HeavyBleeding, skill.Level, bleedDamage, BleedDuration, 1, 100, -1, hit);
			}
		}
	}
}
