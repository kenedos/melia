using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchanter skill Lightning Hands.
	/// Applies a temporary buff that changes basic attacks into Lightning attacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Enchanter_LightningHands)]
	public class Enchanter_LightningHandsOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			caster.StartBuff(BuffId.LightningHands_Buff, skill.Level, 0, BuffDuration, caster, skill.Id);
		}
	}

	/// <summary>
	/// Handler for the special basic attack used while Lightning Hands is active.
	/// The normal attack damage is preserved and the Lightning Hands factor is added.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.LightningHands_Attack)]
	public class LightningHands_AttackOverride : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (target == null || target.IsDead)
				return;

			if (!caster.TryGetBuff(BuffId.LightningHands_Buff, out _))
				return;

			if (!caster.TryGetSkill(SkillId.Enchanter_LightningHands, out var lightningHandsSkill))
				return;

			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			var lightningFactor = (150f + 14f * Math.Max(0, lightningHandsSkill.Level - 1)) / 100f;

			if (caster is Character character)
			{
				if (character.Abilities.TryGet(AbilityId.Enchanter13, out var enhanceAbility) && enhanceAbility.Active)
				{
					var enhanceRate = enhanceAbility.Level * 0.005f;

					if (enhanceAbility.Level >= 100)
						enhanceRate += 0.10f;

					lightningFactor *= 1f + enhanceRate;
				}

				if (character.IsAbilityActive(AbilityId.Enchanter16))
					lightningFactor *= 1.25f;
			}

			skillHitResult.Damage *= 1f + lightningFactor;
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(200), TimeSpan.Zero);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
			caster.SetAttackState(false);
		}
	}
}
