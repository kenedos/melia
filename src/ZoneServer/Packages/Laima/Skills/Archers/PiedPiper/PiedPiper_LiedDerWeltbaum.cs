using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper
{
	[Package("laima")]
	[SkillHandler(SkillId.PiedPiper_LiedDerWeltbaum)]
	public class PiedPiper_LiedDerWeltbaum : IGroundSkillHandler
	{
		private const float SkillRange = 160f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int BaseBlockCount = 1;
		private const int BaseDurationSeconds = 900;
		private const int DurationPerAbilityLevelSeconds = 12;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.SetAttackState(false);
				return;
			}

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position);

			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			var additionalBlockCount = this.GetActiveAbilityLevel(character, AbilityId.PiedPiper14);
			var durationAbilityLevel = this.GetActiveAbilityLevel(character, AbilityId.PiedPiper15);
			var blockCount = BaseBlockCount + additionalBlockCount;
			var durationSeconds = BaseDurationSeconds + durationAbilityLevel * DurationPerAbilityLevelSeconds;
			var duration = TimeSpan.FromSeconds(durationSeconds);
			var recipients = this.GetRecipients(character);

			foreach (var recipient in recipients)
			{
				recipient.StartBuff(BuffId.LiedDerWeltbaum_Buff, skillLevel, 0, duration, character, skill.Id);
				recipient.StartBuff(BuffId.LiedDerWeltbaum_NoDamage_Buff, blockCount, 0, duration, character, skill.Id);
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(false);
		}

		private int GetActiveAbilityLevel(Character character, AbilityId abilityId)
		{
			if (!character.TryGetAbility(abilityId, out var ability) || !ability.Active)
				return 0;

			return Math.Max(ability.Level, 0);
		}

		private List<Character> GetRecipients(Character caster)
		{
			var recipients = new List<Character> { caster };

			if (caster.Connection?.Party == null)
				return recipients;

			var partyMembers = caster.Map.GetPartyMembersInRange(caster, SkillRange, true).Where(member => member != null && !member.IsDead && member.Layer == caster.Layer);

			foreach (var member in partyMembers)
				if (recipients.All(existing => existing.Handle != member.Handle))
					recipients.Add(member);

			return recipients;
		}
	}
}
