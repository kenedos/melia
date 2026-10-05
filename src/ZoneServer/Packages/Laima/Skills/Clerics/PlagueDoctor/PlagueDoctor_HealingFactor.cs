using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor
{
	/// <summary>
	/// Healing Factor.
	/// Applies regenerative healing to a designated ally or nearby party members.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.PlagueDoctor_HealingFactor)]
	public class PlagueDoctor_HealingFactor : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const float PartyEffectRange = 170f;
		private static readonly TimeSpan NormalDuration = TimeSpan.FromSeconds(45);
		private static readonly TimeSpan HealPartyDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan HealPartyCooldown = TimeSpan.FromMilliseconds(2500);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, target);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			var target = targets?.FirstOrDefault(candidate => this.IsValidTarget(caster, candidate));
			this.Cast(skill, caster, originPos, farPos, target);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			var healPartyActive = character.IsAbilityActive(AbilityId.PlagueDoctor18);

			if (!healPartyActive && !this.IsValidTarget(character, selectedTarget))
				selectedTarget = character;

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			if (healPartyActive)
				skill.StartCooldown(HealPartyCooldown);

			character.TurnTowards(farPos);
			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(character, selectedTarget?.Handle ?? character.Handle, originPos, character.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

			if (healPartyActive)
				this.ApplyToParty(character, skill);
			else
				this.ApplyHealingFactor(selectedTarget, character, skill, NormalDuration);

			character.SetAttackState(false);
		}

		private void ApplyToParty(Character caster, Skill skill)
		{
			this.ApplyHealingFactor(caster, caster, skill, HealPartyDuration);

			if (caster.Connection?.Party == null)
				return;

			var partyMembers = caster.Map.GetPartyMembersInRange(caster, PartyEffectRange, true).Where(member => member != null && !member.IsDead && member != caster).ToList();

			foreach (var partyMember in partyMembers)
				this.ApplyHealingFactor(partyMember, caster, skill, HealPartyDuration);
		}

		private void ApplyHealingFactor(ICombatEntity target, Character caster, Skill skill, TimeSpan duration)
		{
			if (!this.IsValidTarget(caster, target))
				return;

			var referenceHp = target.Properties.GetFloat(PropertyName.HP);

			target.StartBuff(BuffId.HealingFactor_Buff, skill.Level, referenceHp, duration, caster, skill.Id);
		}

		private bool IsValidTarget(ICombatEntity caster, ICombatEntity target)
		{
			return target is Character && !target.IsDead && !caster.IsEnemy(target);
		}
	}
}
