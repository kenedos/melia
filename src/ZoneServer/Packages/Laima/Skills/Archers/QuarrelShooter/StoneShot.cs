using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Reflection.Metadata;

namespace Melia.Zone.Skills.Handlers.Archers.QuarrelShooter
{
	/// <summary>
	/// Handler for the QuarrelShooter skill Stone Shot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.QuarrelShooter_StoneShot)]
	public class QuarrelShooterStoneShot : IForceSkillHandler
	{
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
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill, null);
				return;
			}

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var skillHitResult = SCR_SkillHit(caster, target, skill);

			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			if (skillHitResult.Damage > 0 && target.IsKnockdownable())
			{
				skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockBack, 65, 10);
				skillHit.HitInfo.Type = HitType.KnockBack;
				target.ApplyKnockback(caster, skill, skillHit);
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			if (skillHitResult.Damage > 0)
			{
				var stunChance = 0.3f + (skill.Level * 0.05f);
				if (RandomProvider.Get().NextDouble() < stunChance)
				{
					target.StartBuff(BuffId.Stun, TimeSpan.FromSeconds(1.5), caster);
				}
			}
		}
	}
}
