using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Conviction_Debuff debuff, which increases damage taken
	/// from elemental attacks.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Conviction_Debuff)]
	public class Conviction_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		/// <summary>
		/// Applies the debuff's effect during the combat calculations.
		/// </summary>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Get the attack attribute (override or skill's default)
			var attackAttribute = modifier.AttackAttribute == AttributeType.None ? skill.Data.Attribute : modifier.AttackAttribute;

			// Check if the incoming attack is of elemental type (Fire, Ice, Lightning, Earth, Poison)
			if (attackAttribute == AttributeType.Fire ||
			attackAttribute == AttributeType.Ice ||
			attackAttribute == AttributeType.Lightning ||
			attackAttribute == AttributeType.Earth ||
			attackAttribute == AttributeType.Poison)
			{
				// Increase damage taken from elemental type damage by 20% + 3% * SkillLv
				// buff.NumArg1 contains the skill level
				modifier.DamageMultiplier += 0.20f + (buff.NumArg1 * 0.03f);
			}
		}
	}
}
