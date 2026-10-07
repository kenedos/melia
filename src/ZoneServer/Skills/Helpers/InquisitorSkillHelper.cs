using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Inquisitor's devil and torture skills.
	/// </summary>
	public static class InquisitorSkillHelper
	{
		private const int BurnChance = 20;
		private const int MaxFlames = 3;
		private const int ArtsExtraFlames = 3;
		private const float FlameOffset = 20f;
		private const float FlameRange = 25f;
		private static readonly TimeSpan TortureCooldownReduction = TimeSpan.FromSeconds(1);
		private static readonly SkillId[] TortureSkills = [SkillId.Inquisitor_IronMaiden, SkillId.Inquisitor_PearofAnguish, SkillId.Inquisitor_BreakingWheel, SkillId.Inquisitor_BreastRipper];

		/// <summary>
		/// Returns true if the target counts as a devil for the caster's
		/// attacks, being one or marked by Judgment: Summary Decision.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsDevil(ICombatEntity caster, ICombatEntity target)
		{
			if (target.Race == RaceType.Velnias)
				return true;

			return target.TryGetBuff(BuffId.Judgment_Abil_Debuff, out var mark) && mark.Caster == caster;
		}

		/// <summary>
		/// Returns true if the caster's devil bonuses apply against the
		/// target, which they do against devils and while under Judgment.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsPunishing(ICombatEntity caster, ICombatEntity target)
			=> caster.IsBuffActive(BuffId.Judgment_Buff) || IsDevil(caster, target);

		/// <summary>
		/// Returns true if the skill is one of the torture skills Torture
		/// Expert works on.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static bool IsTortureSkill(SkillId skillId)
			=> Array.IndexOf(TortureSkills, skillId) >= 0;

		/// <summary>
		/// Inquisitor: Burn, which ignites a flame where an enemy the
		/// Inquisitor killed fell 20% of the time.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="victim"></param>
		/// <param name="skill"></param>
		public static void TryBurn(Character caster, ICombatEntity victim, Skill skill)
		{
			if (GameRandom.Get().Next(100) < BurnChance)
				TryIgnite(caster, victim.Position, skill);
		}

		/// <summary>
		/// Inquisitor: Torture Expert, which takes a second off the torture
		/// skills' cooldowns when one of them kills an enemy, and with its
		/// Arts ignites a Burn flame nearby.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		public static void TryTortureExpert(Character caster, Skill skill)
		{
			if (!IsTortureSkill(skill.Id))
				return;

			foreach (var skillId in TortureSkills)
			{
				if (caster.TryGetSkill(skillId, out var tortureSkill))
					tortureSkill.ReduceCooldown(TortureCooldownReduction);
			}

			if (caster.IsAbilityActive(AbilityId.Inquisitor21))
				TryIgnite(caster, caster.Position.GetRandomInRange2D((int)FlameOffset), skill);
		}

		/// <summary>
		/// Ignites a Burn flame at the position, unless the caster already
		/// has as many burning as allowed.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="position"></param>
		/// <param name="skill"></param>
		private static void TryIgnite(Character caster, Position position, Skill skill)
		{
			var maxFlames = MaxFlames;
			if (caster.IsAbilityActive(AbilityId.Inquisitor21))
				maxFlames += ArtsExtraFlames;

			var flames = caster.Map.GetPads(pad => pad.Name == PadName.Inquisitor_FireWall && pad.Creator == caster);
			if (flames.Length >= maxFlames)
				return;

			var flame = new Pad(PadName.Inquisitor_FireWall, caster, skill, new Circle(position, FlameRange));
			flame.Position = position;
			caster.Map.AddPad(flame);
		}
	}
}
