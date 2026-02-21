using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Possession.
	/// Circle AoE burst + Hold debuff on enemies.
	/// Requires spirit form. Has a 1.5 second cast time.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_Possession)]
	public class Sadhu_PossessionOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int MaxTargets = 7;

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
			if (!caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
				return;

			if (caster is not Character casterCharacter)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, ForceId.GetNew(), null);

			Send.ZC_GROUND_EFFECT(caster, caster.Position, "F_spread_out026_mint", 3f, 3f, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(caster, caster.Position, "F_explosion086_mint", 1.2f, 3f, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(caster, caster.Position, "F_burstup047_mint", 0.7f, 3f, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(caster, caster.Position, "F_pattern025_loop", 1.5f, 3f, 0, 0, 0);

			var circle = new Circle(caster.Position, 120);
			var aoeTargets = caster.Map.GetAttackableEnemiesIn(caster, circle);

			foreach (var target in aoeTargets.Take(MaxTargets))
			{
				var holdTime = this.GetHoldTime(skill);
				target.StartBuff(BuffId.Common_Hold, TimeSpan.FromMilliseconds(holdTime));

				var modifier = SkillModifier.MultiHit(8);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(200));
				Send.ZC_HIT_INFO(caster, target, hit);
			}
		}

		private int GetHoldTime(Skill skill)
		{
			return 3000 + (300 * skill.Level);
		}
	}
}
