using System;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handler for the ResistElements_Buff, which reduces incoming elemental damage.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ResistElements_Buff)]
	public class ResistElements_BuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		/// <summary>
		/// Reduces incoming elemental damage.
		/// </summary>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var attackAttribute = modifier.AttackAttribute == AttributeType.None ? skill.Data.Attribute : modifier.AttackAttribute;

			// Check if the incoming attack is elemental (Fire, Ice, Lightning, Earth, Poison)
			if (attackAttribute == AttributeType.Fire ||
				attackAttribute == AttributeType.Ice ||
				attackAttribute == AttributeType.Lightning ||
				attackAttribute == AttributeType.Earth ||
				attackAttribute == AttributeType.Poison)
			{
				// Base reduction: 15% + 1.5% * SkillLv
				var baseReduction = 0.15f + (buff.NumArg1 * 0.015f);

				// Paladin37 ability: 0.5% multiplier per ability level (1.5x at level 100)
				var abilityMultiplier = 1f;
				if (target.TryGetActiveAbilityLevel(AbilityId.Paladin37, out var abilityLevel))
					abilityMultiplier += abilityLevel * 0.005f;

				// Calculate final reduction, capped at 90%
				var finalReduction = Math.Min(0.90f, baseReduction * abilityMultiplier);

				// Reduce damage by the calculated percentage
				modifier.DamageMultiplier *= (1f - finalReduction);
			}
		}
	}
}
