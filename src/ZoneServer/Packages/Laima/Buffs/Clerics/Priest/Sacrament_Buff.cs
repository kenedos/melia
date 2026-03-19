using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Shared.Data.Database;

namespace Melia.Zone.Buffs.Handlers.Wizard
{
	/// <summary>
	/// Handler for the Sacrament buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Sacrament_Buff)]
	public class Sacrament_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Sacrament_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Sacrament_Buff, out var buff))
				return;

			if ((skill.Data.Attribute == AttributeType.None) || (skill.Data.Attribute == AttributeType.Melee) || (skill.Data.Attribute == AttributeType.Magic))
				modifier.AttackAttribute = AttributeType.Holy;
		}
	}
}
