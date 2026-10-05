using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handles the application of Flowering by Control Blade.
	/// </summary>
	public static class BlossomBladerFloweringHelper
	{
		private const int MaximumEnhanceLevel = 100;
		private static readonly TimeSpan FloweringDuration = TimeSpan.FromSeconds(30);

		public static void Apply(ICombatEntity attacker, ICombatEntity target, Skill attackingSkill)
		{
			if (attacker == null || attacker.IsDead || target == null || target.IsDead || attackingSkill == null)
				return;

			if (attackingSkill.Id != SkillId.BlossomBlader_ControlBlade)
				return;

			if (!attacker.TryGetSkill(SkillId.BlossomBlader_Flowering, out var floweringSkill) || floweringSkill.Level <= 0)
				return;

			var enhanceLevel = GetEnhanceLevel(attacker);
			target.StartBuff(BuffId.Flowering_Debuff, floweringSkill.Level, enhanceLevel, FloweringDuration, attacker, SkillId.BlossomBlader_Flowering);
		}

		private static int GetEnhanceLevel(ICombatEntity attacker)
		{
			if (attacker is not Character character)
				return 0;

			return Math.Clamp(character.Abilities.GetLevel(AbilityId.Blossomblader1), 0, MaximumEnhanceLevel);
		}
	}
}
