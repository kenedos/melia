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

namespace Melia.Zone.Skills.Handlers.Swordsman.Barbarian
{
	/// <summary>
	/// Handler for the Barbarian skill Cleave.
	/// Per the rework, Cleave is a low-damage, wide-area utility skill
	/// that applies a debuff, making enemies more vulnerable to Slash attacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_Cleave)]
	public class Barbarian_CleaveOverride : IMeleeGroundSkillHandler
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

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 40, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes the attack, dealing minimal damage and applying the Slash vulnerability debuff.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			// Apply the effect to all targets within the skill's AoE Attack Ratio limit.
			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				if (skillHitResult.Damage > 0)
				{
					var debuffDuration = TimeSpan.FromSeconds(5);
					target.StartBuff(BuffId.Cleave_Debuff, skill.Level, 0, debuffDuration, caster);
				}

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(50), TimeSpan.Zero);
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
