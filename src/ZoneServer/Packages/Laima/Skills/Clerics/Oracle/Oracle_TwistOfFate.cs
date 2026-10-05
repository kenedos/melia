using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Twist Of Fate.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_TwistOfFate)]
	public class Oracle_TwistOfFateOverride : IForceSkillHandler
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

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var dmgRate = RandomProvider.Get().Next(8 * skill.Level - 7, 8 * skill.Level + 1);

			if (target is not Character)
			{
				if (target is Mob mob && mob.Rank == MonsterRank.Boss)
				{
					if (mob.Data.Rank == MonsterRank.Boss)
					{
						dmgRate /= 2;
					}
				}
			}

			var damage = (float)Math.Floor(target.Properties.GetFloat(PropertyName.MHP) * dmgRate / 100f);

			target.SetTempVar("TwistOfFate_DamageRate", damage);

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			skillHitResult.Damage = damage;
			target.TakeDamage(skillHitResult.Damage, caster);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			if (target is Character && !target.IsBuffActive(BuffId.TwistOfFate_Debuff))
				SkillResultTargetBuff(caster, skill, BuffId.TwistOfFate_Debuff, skill.Level, 0f, 30000f, 1, 100, -1, [skillHit]);
		}
	}
}
