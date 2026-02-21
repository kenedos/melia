using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizard
{
	/// <summary>
	/// Handler for the Wizard skill Energy Bolt.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wizard_EnergyBolt)]
	public class Wizard_EnergyBoltOverride : IForceSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_wiz_energybolt_cast", "voice_wiz_m_energybolt_cast");
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_wiz_energybolt_cast", "voice_wiz_m_energybolt_cast");
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}


		/// <summary>
		/// Handles the skill, attacking the targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
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
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var damageDelay = TimeSpan.FromMilliseconds(550);
			var skillHitDelay = TimeSpan.FromMilliseconds(100);

			var splashArea = new Circle(target.Position, skill.Properties.GetFloat(PropertyName.SplRange));
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			var skillHits = new List<SkillHitInfo>();

			var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(2));
			target.TakeDamage(skillHitResult.Damage, caster);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			if (skillHitResult.Damage > 0 && target.IsKnockdownable())
			{
				skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, skillHit.Target, HitType.KnockBack, 45, 10);
				skillHit.HitInfo.Type = HitType.KnockBack;
				target.ApplyKnockback(caster, skill, skillHit);
			}

			skillHits.Add(skillHit);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHits);
		}
	}
}
