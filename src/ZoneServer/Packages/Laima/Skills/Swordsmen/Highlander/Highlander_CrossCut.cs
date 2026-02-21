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
using Melia.Zone.World.Actors.Characters.Components;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;

namespace Melia.Zone.Skills.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handler for the Highlander skill Cross Cut.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Highlander_CrossCut)]
	public class Highlander_CrossCutOverride : IMeleeGroundSkillHandler
	{
		private const float BaseBleedDamage = 0.2f;
		private const float BleedDamagePerLevel = 0.01f;
		private const float BaseBleedDuration = 5000;
		private const float BleedDurationPerLevel = 1000;

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
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, originPos, farPos));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var bleedDuration = BaseBleedDuration + skill.Level * BleedDurationPerLevel;

			if (caster.TryGetActiveAbilityLevel(AbilityId.Highlander34, out var abilityLevel))
				bleedDuration += abilityLevel * 1000;

			var hits = new List<SkillHitInfo>();

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 0;
			var damageDelay = 175;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			foreach (var hit in hits)
			{
				var damage = hit.HitInfo.Damage;
				var bleedDamage = damage * BaseBleedDamage;
				bleedDamage += damage * skill.Level * BleedDamagePerLevel;
				bleedDamage = Math.Max(1, bleedDamage);
				SkillResultTargetBuff(caster, skill, BuffId.HeavyBleeding, skill.Level, bleedDamage, bleedDuration, 1, 100, -1, hit);
			}

			hits.Clear();

			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 0);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 360;
			damageDelay = 560;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			foreach (var hit in hits)
			{
				var damage = hit.HitInfo.Damage;
				var bleedDamage = damage * BaseBleedDamage;
				bleedDamage += damage * skill.Level * BleedDamagePerLevel;
				bleedDamage = Math.Max(1, bleedDamage);
				SkillResultTargetBuff(caster, skill, BuffId.HeavyBleeding, skill.Level, bleedDamage, bleedDuration, 1, 100, -1, hit);
			}
		}
	}
}
