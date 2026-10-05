using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Skills.Handlers;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Bullet Marker skill Outrage.
	/// SkillId: 51112
	/// Factor: 46 + 47 per level
	/// Requires Double Gun Stance.
	/// Requires at least 4 Overheating stacks.
	/// Converts Overheating stacks into Outrage stacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_Outrage)]
	public class BulletMarker_Outrage : IGroundSkillHandler
	{
		private const int MinimumOverheatingStacks = 4;
		private const int MaximumOverheatingStacks = 40;
		private static readonly TimeSpan OutrageDuration = TimeSpan.FromSeconds(35);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				character.ServerMessage(Localization.Get("Double Gun Stance must be active."));
				return;
			}

			var overheatingStacks = character.Buffs.GetOverbuffCount(BuffId.Overheating_Buff);

			if (overheatingStacks < MinimumOverheatingStacks)
			{
				character.ServerMessage(Localization.Get("At least 4 Overheating stacks are required."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			if (target != null && !target.IsDead && caster.InSkillUseRange(skill, target))
				this.Attack(skill, caster, target);

			this.ActivateOutrage(skill, character, overheatingStacks);

			caster.SetAttackState(false);
		}

		private void Attack(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill);

			this.ApplyEnhanceAbility(caster, skillHitResult);

			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(
				caster,
				target,
				skill,
				skillHitResult,
				TimeSpan.FromMilliseconds(50),
				TimeSpan.Zero);

			Send.ZC_SKILL_HIT_INFO(caster, skillHit);
		}

		private void ActivateOutrage(Skill skill, Character character, int overheatingStacks)
		{
			var outrageStacks = overheatingStacks / 2;

			if (overheatingStacks >= MaximumOverheatingStacks)
				outrageStacks = 30;

			character.StopBuff(BuffId.Overheating_Buff);
			character.StopBuff(BuffId.Outrage_Buff);
			character.StopBuff(BuffId.Overheating_outrage_Buff);

			for (var i = 0; i < outrageStacks; i++)
			{
				character.StartBuff(
					BuffId.Outrage_Buff,
					skill.Level,
					0f,
					OutrageDuration,
					character,
					skill.Id);
			}

			character.StartBuff(
				BuffId.Overheating_outrage_Buff,
				skill.Level,
				0f,
				OutrageDuration,
				character,
				skill.Id);
		}

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker11, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
