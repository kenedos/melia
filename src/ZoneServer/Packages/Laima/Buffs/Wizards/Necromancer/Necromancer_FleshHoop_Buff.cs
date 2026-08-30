using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Flesh Hoop buff, which doubles Flesh Cannon's damage
	/// and is consumed by it.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.FleshHoop_Buff)]
	public class Necromancer_FleshHoop_BuffOverride : BuffHandler
	{
		private const float FleshCannonBonus = 1f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc_Attack, BuffId.FleshHoop_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id != SkillId.Necromancer_FleshCannon)
				return;

			if (!attacker.IsBuffActive(BuffId.FleshHoop_Buff))
				return;

			skillHitResult.Damage *= 1f + FleshCannonBonus;
		}
	}
}
