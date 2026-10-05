using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities.Handlers.Archers.Cannoneer;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Mantém os efeitos ativos da Bazooka.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Bazooka_Buff)]
	public class Cannoneer_Bazooka_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			if (Cannoneer_BazookaVeteranMercenaryAbility.IsActive(character))
				AddPropertyModifier(buff, character, PropertyName.NormalASPD_BM, Cannoneer_BazookaVeteranMercenaryAbility.AttackSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			RemovePropertyModifier(buff, character, PropertyName.NormalASPD_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Bazooka_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || !character.IsBuffActive(BuffId.Bazooka_Buff))
				return;

			if (Cannoneer_BazookaSteadyFireAbility.IsActive(character))
				modifier.DamageMultiplier *= Cannoneer_BazookaSteadyFireAbility.AdditionalDamageMultiplier;
		}
	}
}
