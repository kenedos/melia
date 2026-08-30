using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Preparation buff, which blocks every incoming attack
	/// while the skill is being held.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Preparation_Buff)]
	public class Fencer_Preparation_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Preparation_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.Preparation_Buff))
				return;

			skillHitResult.Damage = 0;
			skillHitResult.Result = HitResultType.Block;
		}
	}
}
