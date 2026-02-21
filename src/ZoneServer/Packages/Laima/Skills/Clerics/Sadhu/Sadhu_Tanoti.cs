using System;
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
using System.Linq;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Tanoti.
	/// Circle AoE + pull chance on enemies.
	/// Requires spirit form.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_Tanoti)]
	public class Sadhu_TanotiOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int MaxTargets = 9;

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

			Send.ZC_GROUND_EFFECT(caster, caster.Position, "F_pose_magical2_light01_mint", 1.5f, 1f, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(caster, caster.Position, "E_cleric_tanoti001", 1f, 1f, 0, 0, 0);

			var circle = new Circle(caster.Position, 70);
			var aoeTargets = caster.Map.GetAttackableEnemiesIn(caster, circle);

			foreach (var target in aoeTargets.Take(MaxTargets))
			{
				var chance = this.GetPullChance(skill);

				if (chance >= RandomProvider.Get().Next(100) && target.IsKnockdownable())
					this.PullEntity(caster, target);

				var modifier = SkillModifier.MultiHit(4);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(200));
				Send.ZC_HIT_INFO(caster, target, hit);
			}
		}

		private void PullEntity(ICombatEntity caster, ICombatEntity target)
		{
			target.Position = caster.Position;
			Send.ZC_SET_POS(target, caster.Position);
		}

		private float GetPullChance(Skill skill)
		{
			return Math.Min(80, 35 + (4.5f * skill.Level));
		}
	}
}
