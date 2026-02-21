using System;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Monstrance, Damage taken increased..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Monstrance_Debuff)]
	public class Monstrance_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var damageBonus = buff.NumArg2;

			modifier.DamageMultiplier += damageBonus;
		}
	}
}
