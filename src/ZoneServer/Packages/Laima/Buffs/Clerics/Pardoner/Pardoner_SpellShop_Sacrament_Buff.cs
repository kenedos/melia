using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Grace: Additional Holy Hits buff sold by a Spell
	/// Shop. Turns the target's attacks into holy property attacks.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.SpellShop_Sacrament_Buff)]
	public class Pardoner_SpellShop_Sacrament_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.SpellShop_Sacrament_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.IsBuffActive(BuffId.SpellShop_Sacrament_Buff))
				return;

			if (skill.Data.Attribute is AttributeType.None or AttributeType.Melee or AttributeType.Magic)
				modifier.AttackAttribute = AttributeType.Holy;
		}
	}
}
