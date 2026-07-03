using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune Caster skill Rune of Beginning.
	/// SkillId: 21308
	/// ClassName: RuneCaster_Berkana
	///
	/// Passive effect:
	/// - Increases final damage of Rune Caster skills.
	/// - Uses the passive skill level as bonus percentage.
	/// - Lv1 = +1%, Lv5 = +5%, etc.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Berkana)]
	public class RuneCaster_BerkanaOverride : IPassiveSkillHandler, ISkillCombatAttackAfterCalcHandler
	{
		public void Handle(Skill skill, ICombatEntity caster)
		{
			// Passive marker only.
		}

		public void OnAttackAfterCalc(
			Skill skill,
			ICombatEntity attacker,
			ICombatEntity target,
			Skill attackerSkill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			if (attacker is not Character character)
				return;

			if (!this.IsRuneCasterSkill(attackerSkill.Id))
				return;

			var bonusRate = skill.Level * 0.01f;

			this.ApplyRuneOfBeginningEnhance(character, ref bonusRate);

			skillHitResult.Damage *= 1f + bonusRate;
		}

		private bool IsRuneCasterSkill(SkillId skillId)
		{
			return
				skillId == SkillId.RuneCaster_Hagalaz ||
				skillId == SkillId.RuneCaster_Isa ||
				skillId == SkillId.RuneCaster_Thurisaz ||
				skillId == SkillId.RuneCaster_Tiwaz ||
				skillId == SkillId.RuneCaster_Stan ||
				skillId == SkillId.RuneCaster_Ehwaz;
		}

		private void ApplyRuneOfBeginningEnhance(Character character, ref float bonusRate)
		{
			if (!character.Abilities.TryGet(AbilityId.RuneCaster25, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			bonusRate *= 1f + enhanceRate;
		}
	}
}
