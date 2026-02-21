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
using Melia.Zone.Skills.Handlers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Swordsman
{
	/// <summary>
	/// Handler for the Swordsman skill Thrust.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Swordman_Thrust)]
	public class Swordman_ThrustOverride : IMeleeGroundSkillHandler
	{
		private const float BaseBleedDamage = 1.0f;
		private const float BleedDamagePerLevel = 0.2f;
		private const float BleedDuration = 10000;

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public async void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
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

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 20, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 70;
			var damageDelay = 270;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			foreach (var hit in hits)
			{
				var damage = hit.HitInfo.Damage;
				var bleedDamage = damage * BaseBleedDamage;
				bleedDamage += damage * skill.Level * BleedDamagePerLevel;
				bleedDamage = Math.Max(1, bleedDamage);
				SkillResultTargetBuff(caster, skill, BuffId.HeavyBleeding, skill.Level, bleedDamage, BleedDuration, 1, 100, -1, hit);
			}
		}
	}
}
