using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Cryomancer: Freeze ability (Modificada para aumento de dano de Gelo).
	/// Increases Ice element damage by 4% per active ability level.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cryomancer9)]
	public class Cryomancer9Override : IAbilityHandler
	{
		/// <summary>
		/// Increases Ice damage by 4% per level on hit.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Cryomancer9)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker == null || skill == null || skillHitResult.Damage <= 0f)
				return;

			if (!attacker.TryGetActiveAbility(AbilityId.Cryomancer9, out var ability))
				return;

			// Verifica se a habilidade ou o ataque possui o elemento Gelo
			if (skill.Data.Attribute != AttributeType.Ice && modifier.AttackAttribute != AttributeType.Ice)
				return;

			// Aplica o multiplicador de +4% por nível da habilidade
			skillHitResult.Damage *= 1f + Math.Max(0, ability.Level) * 0.05f;
		}
	}
}
