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
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper
{
	[Package("laima")]
	[SkillHandler(SkillId.PiedPiper_Improvisation)]
	public class PiedPiper_Improvisation : ISelfSkillHandler
	{
		private const float SkillRange = 160f;
		private const int BaseMarschierendesliedBlockCount = 10;
		private const int BaseLiedDerWeltbaumBlockCount = 3;
		private static readonly TimeSpan CooldownReduction = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster is not Character character || character.IsDead)
				return;

			var availableSkills = this.GetAvailableSkills(character);

			if (availableSkills.Count == 0)
			{
				character.ServerMessage("No valid Pied Piper skill has been learned.");
				caster.SetAttackState(false);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.SetAttackState(false);
				return;
			}

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, dir, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			var selectedSkill = availableSkills[RandomProvider.Get().Next(availableSkills.Count)];
			var failed = selectedSkill.IsOnCooldown;

			this.ApplySelectedSkill(character, selectedSkill, failed);

			skill.IncreaseOverheat();

			if (character.IsAbilityActive(AbilityId.PiedPiper16))
				skill.ReduceCooldown(CooldownReduction);

			caster.SetAttackState(false);
		}

		private List<Skill> GetAvailableSkills(Character character)
		{
			var skills = new List<Skill>();

			this.AddLearnedSkill(character, SkillId.PiedPiper_Dissonanz, skills);
			this.AddLearnedSkill(character, SkillId.PiedPiper_Wiegenlied, skills);
			this.AddLearnedSkill(character, SkillId.PiedPiper_Marschierendeslied, skills);
			this.AddLearnedSkill(character, SkillId.PiedPiper_LiedDerWeltbaum, skills);

			return skills;
		}

		private void AddLearnedSkill(Character character, SkillId skillId, List<Skill> skills)
		{
			if (character.TryGetSkill(skillId, out var learnedSkill) && learnedSkill.Level > 0)
				skills.Add(learnedSkill);
		}

		private void ApplySelectedSkill(Character caster, Skill selectedSkill, bool failed)
		{
			switch (selectedSkill.Id)
			{
				case SkillId.PiedPiper_Dissonanz:
					this.ApplyDissonanz(caster, selectedSkill, failed);
					break;
				case SkillId.PiedPiper_Wiegenlied:
					this.ApplyWiegenlied(caster, selectedSkill, failed);
					break;
				case SkillId.PiedPiper_Marschierendeslied:
					this.ApplyMarschierendeslied(caster, selectedSkill, failed);
					break;
				case SkillId.PiedPiper_LiedDerWeltbaum:
					this.ApplyLiedDerWeltbaum(caster, selectedSkill, failed);
					break;
			}
		}

		private void ApplyDissonanz(Character caster, Skill selectedSkill, bool failed)
		{
			var hasSeaker = caster.IsAbilityActive(AbilityId.PiedPiper1);
			var hasSoundWaveAttack = caster.IsAbilityActive(AbilityId.PiedPiper2);
			var durationMultiplier = failed ? 0.5f : 1f;
			var stunDurationSeconds = (hasSeaker ? 7f : 5f) * durationMultiplier;
			var soundWaveDurationSeconds = Math.Max(selectedSkill.Level, 1) * 2f * durationMultiplier;
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SkillRange).Where(target => target != null && !target.IsDead).ToList();

			foreach (var target in targets)
			{
				target.StartBuff(BuffId.Dissonanz_Stun_Debuff, selectedSkill.Level, 0, TimeSpan.FromSeconds(stunDurationSeconds), caster, selectedSkill.Id);

				if (hasSoundWaveAttack)
					target.StartBuff(BuffId.Dissonanz_Debuff, selectedSkill.Level, 0, TimeSpan.FromSeconds(soundWaveDurationSeconds), caster, selectedSkill.Id);
			}
		}

		private void ApplyWiegenlied(Character caster, Skill selectedSkill, bool failed)
		{
			var drowsyEffectMultiplier = failed ? 0.5f : 1f;
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SkillRange).Where(target => target != null && !target.IsDead).ToList();

			foreach (var target in targets)
				target.StartBuff(BuffId.Lullaby_Debuff, selectedSkill.Level, drowsyEffectMultiplier, TimeSpan.FromSeconds(10), caster, selectedSkill.Id);
		}

		private void ApplyMarschierendeslied(Character caster, Skill selectedSkill, bool failed)
		{
			var recipients = this.GetRecipients(caster);
			var swordsmanCount = recipients.Count(member => member.JobClass == JobClass.Swordsman);
			var hasAllegro = caster.IsAbilityActive(AbilityId.PiedPiper7);
			var hasMoraleBoost = caster.IsAbilityActive(AbilityId.PiedPiper8);
			var blockCount = BaseMarschierendesliedBlockCount + Math.Clamp(selectedSkill.Level, 1, 10) + swordsmanCount;
			var durationSeconds = 60f;

			if (hasMoraleBoost)
				durationSeconds += swordsmanCount * 5f;

			if (failed)
				durationSeconds *= 0.5f;

			var moveSpeedDurationSeconds = hasAllegro ? 60f : 5f;

			if (failed)
				moveSpeedDurationSeconds *= 0.5f;

			foreach (var recipient in recipients)
			{
				recipient.StartBuff(BuffId.Marschierendeslied_Buff, blockCount, selectedSkill.Level, TimeSpan.FromSeconds(durationSeconds), caster, selectedSkill.Id);
				recipient.StartBuff(BuffId.Allegro_Buff, 15, 0, TimeSpan.FromSeconds(moveSpeedDurationSeconds), caster, selectedSkill.Id);
			}
		}

		private void ApplyLiedDerWeltbaum(Character caster, Skill selectedSkill, bool failed)
		{
			var recipients = this.GetRecipients(caster);
			var additionalBlockCount = this.GetActiveAbilityLevel(caster, AbilityId.PiedPiper14);
			var durationAbilityLevel = this.GetActiveAbilityLevel(caster, AbilityId.PiedPiper15);
			var blockCount = BaseLiedDerWeltbaumBlockCount + additionalBlockCount;
			var durationSeconds = 900f + durationAbilityLevel * 12f;

			if (failed)
				durationSeconds *= 0.5f;

			foreach (var recipient in recipients)
			{
				recipient.StartBuff(BuffId.LiedDerWeltbaum_Buff, selectedSkill.Level, 0, TimeSpan.FromSeconds(durationSeconds), caster, selectedSkill.Id);
				recipient.StartBuff(BuffId.LiedDerWeltbaum_NoDamage_Buff, blockCount, 0, TimeSpan.FromSeconds(durationSeconds), caster, selectedSkill.Id);
			}
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
