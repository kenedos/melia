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
	/// Handler for the Sacrament Debuff
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Sacrament_Debuff)]
	public class Sacrament_DebuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Sacrament_Debuff)]
		public void OnDefenseBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Sacrament_Debuff, out var buff))
				return;

			if (modifier.AttackAttribute != AttributeType.Holy)
				return;

			modifier.DamageMultiplier += buff.NumArg2;
		}
	}
}
