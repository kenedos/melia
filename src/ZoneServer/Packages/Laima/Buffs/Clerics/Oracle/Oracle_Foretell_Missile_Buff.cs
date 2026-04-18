using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Foretell Missile buff, which nullifies all
	/// incoming missile-type damage while active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Foretell_Missile_Buff)]
	public class Oracle_Foretell_Missile_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc_Defense, BuffId.Foretell_Missile_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.Foretell_Missile_Buff))
				return;

			if (skill?.Data?.ClassType == SkillClassType.Missile)
				modifier.DamageMultiplier = 0;
		}
	}
}
