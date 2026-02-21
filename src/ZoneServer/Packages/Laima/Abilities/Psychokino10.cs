using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Psychokino10 ability.
	/// Reduces damage taken by 30% while casting Psychic Pressure.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Psychokino10)]
	public class Psychokino10Override : IAbilityHandler, IAbilityCombatDefenseAfterCalcHandler
	{
		/// <summary>
		/// Reduces damage taken while casting Psychic Pressure.
		/// </summary>
		public void OnDefenseAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target.TryGetSkill(SkillId.Psychokino_PsychicPressure, out var psychicPressureSkill))
			{
				if (target.IsCasting(psychicPressureSkill))
				{
					skillHitResult.Damage *= 0.7f;
				}
			}
		}
	}
}
