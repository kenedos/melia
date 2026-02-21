using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Clerics.Cleric
{
	/// <summary>
	/// Handler for the Cleric_New_Heal pad.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Cleric_New_Heal)]
	public class Cleric_New_HealOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		/// <summary>
		/// Called when the pad is created.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.Trigger.MaxUseCount = 1;
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(30);
		}

		/// <summary>
		/// Called when the pad is destroyed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Called when an actor enters the pad area.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (pad.IsDead)
				return;

			if (initiator.IsAlly(creator))
			{
				// Don't heal characters at full HP
				if (initiator.Hp >= initiator.MaxHp)
					return;

				// Don't heal summons, companions, or allied NPCs
				if (initiator is not Character)
					return;

				this.HealTarget(creator, initiator, skill);

				pad.Trigger.IncreaseUseCount();
			}
			else if (initiator.IsEnemy(creator))
			{
				// Damage enemies (but not flying ones)
				if (initiator.MoveType == MoveType.Flying)
					return;

				// Remove damage ability
				if (creator.TryGetActiveAbility(AbilityId.Cleric10, out var removeDamageAbility))
					return;

				this.DamageTarget(creator, initiator, skill);

				pad.Trigger.IncreaseUseCount();
			}
		}

		/// <summary>
		/// Heals the target using Heal_Buff.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		private void HealTarget(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			Send.ZC_NORMAL.PlayEffect(target, "F_cleric_heal_active_ground_new");

			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var healAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			var healDuration = TimeSpan.FromMilliseconds(1);
			target.StartBuff(BuffId.Heal_Buff, skill.Level, healAmount, healDuration, caster);

			// Apply Cleric22 ability bonus (heal over time)
			if (caster is Character character && character.TryGetActiveAbilityLevel(AbilityId.Cleric22, out var abilLv))
			{
				var healOverTimeAmount = abilLv * 0.05f * healAmount / 10;
				target.StartBuff(BuffId.Heal_Dot_Buff, 1, healOverTimeAmount, TimeSpan.FromMilliseconds(10000), caster);
			}
		}

		/// <summary>
		/// Damages the target (undead/demon enemies).
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		private void DamageTarget(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			Send.ZC_NORMAL.PlayEffect(target, "F_cleric_heal_active_ground_new");

			// Check if we actually hit
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			if (skillHitResult.Damage <= 0)
				return;

			// Heal Damage Bonus
			var modifier = SkillModifier.Default;
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var damageBonus = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			modifier.AttackAttribute = AttributeType.Holy;
			modifier.BonusDamage += damageBonus;
			modifier.DamageMultiplier -= 0.5f;

			// Calculate final damage
			skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			Send.ZC_SKILL_HIT_INFO(caster, skillHit);
		}
	}
}
