using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Sage
{
	/// <summary>
	/// Handler for the Sage skill Micro Dimension, which distorts the space
	/// around the Sage, striking up to the skill's ratio of enemies in it.
	/// </summary>
	/// <remarks>
	/// Micro Dimension: Confusion may confuse the enemies hit, and Micro
	/// Dimension: After Effects strikes them once more 1.5 seconds later.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Sage_MicroDimension)]
	public class Sage_MicroDimensionOverride : IGroundSkillHandler
	{
		private const float Range = 30f;
		private const int ConfusionChancePerLevel = 5;
		private static readonly TimeSpan ConfusionDuration = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan AfterEffectDelay = TimeSpan.FromMilliseconds(1500);
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(500);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var pad = new Pad(PadName.Sage_MicroDimension, caster, skill, new Circle(caster.Position, Range));
			pad.Position = caster.Position;
			caster.Map.AddPad(pad);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			caster.TryGetActiveAbilityLevel(AbilityId.Sage4, out var confusionLevel);
			var hasAfterEffects = caster.IsAbilityActive(AbilityId.Sage11);
			var hits = new List<SkillHitInfo>();

			foreach (var hitTarget in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, Range).Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, HitDelay, TimeSpan.Zero));

				if (hitTarget.IsDead || skillHitResult.Damage <= 0)
					continue;

				if (GameRandom.Get().Next(100) < confusionLevel * ConfusionChancePerLevel)
					hitTarget.StartBuff(BuffId.Confuse, 1, 0, ConfusionDuration, caster, skill.Id);

				if (hasAfterEffects)
					hitTarget.StartBuff(BuffId.MicroDimension_Debuff, skill.Level, 0, AfterEffectDelay, caster, skill.Id);
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
