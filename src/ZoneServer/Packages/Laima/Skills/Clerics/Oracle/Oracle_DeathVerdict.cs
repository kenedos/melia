using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Death Verdict.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_DeathVerdict)]
	public class Oracle_DeathVerdictOverride : IGroundSkillHandler
	{
		private const float CenterDistance = 35f;
		private const float Radius = 70f;
		private const int MinDurationSeconds = 5;
		private const int MaxDurationSeconds = 30;
		private const int MaxHitsToKill = 50;
		private const float ResetRatePerLevel = 0.1f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var centerPos = caster.Position.GetRelative(caster.Direction, CenterDistance);
			var maxTargets = OracleSkillHelper.GetTargetCount(skill);

			// Already sentenced enemies are passed over entirely, rather than
			// taking up one of the skill's targets
			var skillTargets = SkillSelectEnemiesInCircle(caster, centerPos, Radius)
				.Where(a => !a.IsBuffActive(BuffId.DeathVerdict_Buff))
				.Take(maxTargets)
				.ToList();

			await skill.Wait(TimeSpan.FromMilliseconds(700));

			foreach (var skillTarget in skillTargets)
			{
				if (skillTarget.IsDead || skillTarget.IsBuffActive(BuffId.DeathVerdict_Buff))
					continue;

				if (!this.TryGetSentenceDuration(caster, skill, skillTarget, out var duration))
					continue;

				skillTarget.StartBuff(BuffId.DeathVerdict_Buff, skill.Level, 0f, duration, caster, skill.Id);
			}

			SkillTargetBuffAbility(caster, skill, AbilityId.Oracle8, BuffId.DeathVerdict_Slow_Debuff, 1, -1, MaxDurationSeconds * 1000, 0, 1, 100);
		}

		/// <summary>
		/// Returns how long the target's sentence runs via out, derived from
		/// how many plain hits of this skill would be needed to kill it.
		/// Returns false if it endures more than the skill can sentence.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="target"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		private bool TryGetSentenceDuration(ICombatEntity caster, Skill skill, ICombatEntity target, out TimeSpan duration)
		{
			duration = TimeSpan.Zero;

			// The countdown has to be the same every cast, and the skill has
			// no attack type to suppress these rolls on its own
			var modifier = SkillModifier.Default;
			modifier.ForcedHit = true;
			modifier.Unblockable = true;
			modifier.Uncrittable = true;

			var hitResult = SCR_SkillHit(caster, target, skill, modifier);
			if (hitResult.Damage <= 0)
				return false;

			var hitsToKill = (int)Math.Ceiling(target.Properties.GetFloat(PropertyName.HP) / hitResult.Damage);
			if (hitsToKill > MaxHitsToKill)
				return false;

			var seconds = Math.Clamp(hitsToKill, MinDurationSeconds, MaxDurationSeconds);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Oracle18, out var abilityLevel))
				seconds = Math.Max(MinDurationSeconds, (int)(seconds * (1f - abilityLevel * ResetRatePerLevel)));

			duration = TimeSpan.FromSeconds(seconds);
			return true;
		}
	}
}
