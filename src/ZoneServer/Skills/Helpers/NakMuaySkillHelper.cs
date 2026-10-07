using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Nak Muay's Ram Muay stance and Muay
	/// Thai.
	/// </summary>
	public static class NakMuaySkillHelper
	{
		private const string BoranIndexVar = "Melia.NakMuay.BoranIndex";
		private static readonly TimeSpan MuayThaiReduction = TimeSpan.FromSeconds(3);
		private static readonly SkillId[] BoranSkills = [SkillId.NakMuay_TeKha, SkillId.NakMuay_TeTrong, SkillId.NakMuay_SokChiang];

		/// <summary>
		/// Returns true if the caster is in the Ram Muay stance, telling them
		/// otherwise.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static bool CheckRamMuay(ICombatEntity caster)
		{
			if (caster.IsBuffActive(BuffId.RamMuay_Buff))
				return true;

			caster.ServerMessage(Localization.Get("Ram Muay must be active."));
			return false;
		}

		/// <summary>
		/// Takes 3 seconds off Muay Thai's cooldown, which every Nak Muay
		/// attack skill does when used.
		/// </summary>
		/// <param name="caster"></param>
		public static void ReduceMuayThaiCooldown(ICombatEntity caster)
		{
			if (caster.TryGetSkill(SkillId.NakMuay_MuayThai, out var muayThai))
				muayThai.ReduceCooldown(MuayThaiReduction);
		}

		/// <summary>
		/// With Muay Thai: Muay Boran active, follows a basic attack with
		/// the next learned Te Kha, Te Trong or Sok Chiang.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		public static void TryMuayBoran(ICombatEntity caster, ICombatEntity target)
		{
			if (target.IsDead || !caster.TryGetBuff(BuffId.MuayThai_Buff, out var boran))
				return;

			var index = boran.Vars.GetInt(BoranIndexVar);

			for (var i = 0; i < BoranSkills.Length; i++)
			{
				var skillId = BoranSkills[(index + i) % BoranSkills.Length];
				if (!caster.TryGetSkill(skillId, out var skill))
					continue;

				boran.Vars.SetInt(BoranIndexVar, index + i + 1);
				Strike(skill, caster, target);
				return;
			}
		}

		/// <summary>
		/// Strikes the target with a skill cast by Muay Boran.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		private static void Strike(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var modifier = SkillModifier.MultiHit(skill.Id == SkillId.NakMuay_TeTrong ? 2 : 4);

			if (caster.TryGetSkill(SkillId.NakMuay_MuayThai, out var muayThai))
				modifier.FinalDamageMultiplier += muayThai.Properties.GetFloat(PropertyName.CaptionRatio2) / 100f;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(300), TimeSpan.Zero));

			if (skillHitResult.Damage <= 0)
				return;

			if (skill.Id == SkillId.NakMuay_TeKha)
				target.StartBuff(BuffId.TeKha_Debuff, skill.Level, 0, TimeSpan.FromMilliseconds(1500), caster, skill.Id);
			else if (skill.Id == SkillId.NakMuay_SokChiang)
				ApplySokChiang(skill, caster, target, skillHitResult.Damage);
		}

		/// <summary>
		/// Starts Sok Chiang's bleeding on the target, which bleeds a tenth
		/// of the hit every second, for no more than 3 seconds on bosses.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		public static void ApplySokChiang(Skill skill, ICombatEntity caster, ICombatEntity target, float damage)
		{
			var duration = skill.Properties.CaptionTime;
			if (target.Rank == MonsterRank.Boss)
				duration = TimeSpan.FromSeconds(Math.Min(3, duration.TotalSeconds));

			target.StartBuff(BuffId.SokChiang_Debuff, skill.Level, damage * 0.1f, duration, caster, skill.Id);
		}
	}
}
