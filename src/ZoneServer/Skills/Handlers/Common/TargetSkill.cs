using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Shared.World;
using Melia.Zone.Skills.SplashAreas;
using System.Threading.Tasks;

namespace Melia.Zone.Skills.Handlers.Common
{
	/// <summary>
	/// Handles ranged skills that target a single entity.
	/// </summary>
	[SkillHandler(SkillId.Bow_Attack, SkillId.Magic_Attack, SkillId.Magic_Attack_TH,
		SkillId.Bow_Hanging_Attack, SkillId.Pistol_Attack, SkillId.Cannon_Normal_Attack,
		SkillId.CrossBow_Attack, SkillId.CrossBow_Attack2,
		SkillId.Bow_Attack2, SkillId.Pistol_Attack2, SkillId.Cannon_Attack,
		SkillId.DoubleGun_Attack, SkillId.DoubleBullet_Attack)]
	public class TargetSkill : ITargetSkillHandler, IForceSkillHandler
	{
		/// <summary>
		/// Handles usage of the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			//Send.ZC_SKILL_READY(caster, skill, target.Position, Position.Zero);
			//Send.ZC_NORMAL.Unkown_1c(caster, target.Handle, target.Position, caster.Position.GetDirection(target.Position), Position.Zero);

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

			var damageDelay = TimeSpan.FromMilliseconds(skill.Id != SkillId.Common_DaggerAries ? 330 : 250);
			var skillHitDelay = skill.Properties.HitDelay;

			damageDelay = TimeSpan.FromMilliseconds(damageDelay.TotalMilliseconds / skill.Properties.GetFloat(PropertyName.SklSpdRate));
			skillHitDelay = TimeSpan.FromMilliseconds(skillHitDelay.TotalMilliseconds / skill.Properties.GetFloat(PropertyName.SklSpdRate));

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}

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
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill, null);
				return;
			}

			// Based on Normal_Attack posessing a hit delay of 100ms,
			// and Common_DaggerAries one of 50ms, and these two values
			// matching the logs almost 1:1, we can assume this to be
			// the correct value for the skill hit delay. Not a clue
			// about damage delay though. Though there are potentially
			// related values in older skill_bytool files.
			var damageDelay = TimeSpan.FromMilliseconds(skill.Id != SkillId.Common_DaggerAries ? 330 : 250);
			var skillHitDelay = skill.Properties.HitDelay;

			// This part is somewhat guessed. The damage delay does seem to
			// decrease with the speed rate, but the hit delay doesn't. If
			// we don't decrease the hit delay though, the client can't
			// handle very high attack speeds. Granted, they need to be
			// higher than the devs might have ever intended for this to
			// happen, but I still kinda want them to work.
			damageDelay = TimeSpan.FromMilliseconds(damageDelay.TotalMilliseconds / skill.Properties.GetFloatSafe(PropertyName.SklSpdRate));
			skillHitDelay = TimeSpan.FromMilliseconds(skillHitDelay.TotalMilliseconds / skill.Properties.GetFloatSafe(PropertyName.SklSpdRate));

			var modifier = SkillModifier.Default;

			// Random chance to trigger double hit while buff is active
			if (caster.TryGetBuff(BuffId.DoubleAttack_Buff, out var doubleAttackBuff)
				&& RandomProvider.Get().Next(100) < doubleAttackBuff.NumArg2)
			{
				modifier.HitCount += 1;
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}
	}
}
