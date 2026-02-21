using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Monk
{
	/// <summary>
	/// Handler for the God_Finger_Buff. Increases the damage of the
	/// next attack or skill, then the buff is consumed.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.God_Finger_Buff)]
	public class God_Finger_BuffOverride : BuffHandler, IBuffCombatAttackBeforeBonusesHandler
	{
		private const float BaseDamageRate = 0.20f;
		private const float DamageRatePerLevel = 0.02f;

		public void OnAttackBeforeBonuses(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var skillLevel = buff.NumArg1;
			var damageIncrease = BaseDamageRate + DamageRatePerLevel * skillLevel;

			skillHitResult.Damage *= 1f + damageIncrease;

			attacker.StopBuff(BuffId.God_Finger_Buff);
		}
	}
}
