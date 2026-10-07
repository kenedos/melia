using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Plague Doctor skill Healing Factor, which lets an
	/// ally regenerate back up to the HP they had when they received it.
	/// </summary>
	/// <remarks>
	/// Healing Factor: Heal Party applies it to the whole party for 10
	/// seconds, and [Arts] Healing Factor: Close Healing makes it last 120
	/// seconds on anyone but the caster.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PlagueDoctor_HealingFactor)]
	public class PlagueDoctor_HealingFactorOverride : IGroundSkillHandler
	{
		private const float PartyRange = 170f;
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(45);
		private static readonly TimeSpan PartyDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan CloseHealingDuration = TimeSpan.FromSeconds(120);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var healParty = caster.IsAbilityActive(AbilityId.PlagueDoctor18);
			var closeHealing = caster.IsAbilityActive(AbilityId.PlagueDoctor19);

			if (target == null || target.IsDead || caster.IsEnemy(target))
				target = caster;

			if (closeHealing && !healParty && target == caster)
			{
				caster.ServerMessage(Localization.Get("Healing Factor can't be applied to yourself."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var duration = closeHealing ? CloseHealingDuration : healParty ? PartyDuration : Duration;

			if (!healParty)
			{
				this.ApplyHealingFactor(skill, caster, target, duration);
				return;
			}

			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, PartyRange))
			{
				if (closeHealing && ally == caster)
					continue;

				this.ApplyHealingFactor(skill, caster, ally, duration);
			}
		}

		/// <summary>
		/// Starts Healing Factor on the target, remembering their current HP
		/// as the amount it heals them back up to.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="duration"></param>
		private void ApplyHealingFactor(Skill skill, ICombatEntity caster, ICombatEntity target, TimeSpan duration)
		{
			target.StartBuff(BuffId.HealingFactor_Buff, skill.Level, target.Hp, duration, caster, skill.Id);
		}
	}
}
