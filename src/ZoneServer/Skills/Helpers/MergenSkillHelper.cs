using Melia.Shared.Game.Const;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Mergen's attack skills.
	/// </summary>
	public static class MergenSkillHelper
	{
		/// <summary>
		/// Returns true if the skill is one of the Mergen's attack skills.
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static bool IsAttackSkill(Skill skill)
		{
			return skill.Id switch
			{
				SkillId.Mergen_Unload or SkillId.Mergen_TrickShot or SkillId.Mergen_TrickShot_Explode or SkillId.Mergen_FocusFire or SkillId.Mergen_ArrowRain or SkillId.Mergen_DownFall => true,
				_ => false,
			};
		}
	}
}
