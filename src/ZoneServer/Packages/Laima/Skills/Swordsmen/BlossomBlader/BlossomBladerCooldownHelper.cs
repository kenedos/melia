using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;

public static class BlossomBladerCooldownHelper
{
	private static readonly SkillId[] BlossomBladerComboSkills =
	{
		SkillId.BlossomBlader_ControlBlade,
		SkillId.BlossomBlader_BlossomSlash,
		SkillId.BlossomBlader_Flash,
		SkillId.BlossomBlader_FallenBlossom,
	};

	public static void ResetComboCooldowns(ICombatEntity caster)
	{
		if (caster == null)
			return;

		foreach (var skillId in BlossomBladerComboSkills)
		{
			if (caster.TryGetSkill(skillId, out var affectedSkill))
			{
				affectedSkill.ResetCooldown(); // Reseta tempo de recarga e overheat
			}
		}
	}
}
