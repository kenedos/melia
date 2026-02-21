using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scout
{
	/// <summary>
	/// Handler for Oblique Fire buff, which affects the target's movement
	/// speed.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ObliqueFire_Buff)]
	public class ObliqueFire_BuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		/// <summary>
		/// Applies the buff's effect before the damage calculation.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			modifier.DamageMultiplier += buff.OverbuffCounter * 0.04f;
		}
	}
}
