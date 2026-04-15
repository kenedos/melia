using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Archers.Falconer;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for FirstStrike_Buff (Pre-Emptive Strike toggle).
	/// Auto-triggers Sonic Strike (Blistering Thrash) when the caster
	/// attacks an enemy while this buff is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.FirstStrike_Buff)]
	public class Falconer_FirstStrike_BuffOverride : BuffHandler
	{
		/// <summary>
		/// After attack calculations are complete, attempt to trigger
		/// Sonic Strike on the target if the skill is off cooldown.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc_Attack, BuffId.FirstStrike_Buff)]
		public static float OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.FirstStrike_Buff, out var buff))
				return 0;

			// Don't trigger on Sonic Strike itself to avoid recursion
			if (skill.Id == SkillId.Falconer_BlisteringThrash)
				return 0;

			Falconer_BlisteringThrashOverride.TryActivateSonicStrike(attacker, target);

			return 0;
		}
	}
}
