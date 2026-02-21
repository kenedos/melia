using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Feint debuff.
	/// Increases critical chance and minimum critical rate of attackers
	/// against the debuffed target.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Feint_Debuff)]
	public class Feint_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var skillLevel = (int)buff.NumArg1;

			var byAbility = 1f;
			if (buff.Caster is ICombatEntity caster && caster.TryGetActiveAbilityLevel(AbilityId.Rogue28, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;

			modifier.BonusCritChance += (150 + (skillLevel * 15)) * byAbility;
			modifier.MinCritChance += (0.30f + (skillLevel * 0.03f)) * byAbility;
		}
	}
}
