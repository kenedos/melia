using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Scouts.Rogue
{
	/// <summary>
	/// Handler for the SneakHit buff. Increases back attack damage.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.SneakHit_Buff)]
	public class SneakHit_BuffOverride : BuffHandler, IBuffCombatAttackBeforeBonusesHandler
	{
		private const float BaseDamageRate = 0.50f;
		private const float DamageRatePerLevel = 0.05f;

		public void OnAttackBeforeBonuses(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.IsBehind(target) && !modifier.ForcedBackAttack)
				return;

			var skillLevel = buff.NumArg1;
			var damageIncrease = BaseDamageRate + DamageRatePerLevel * skillLevel;

			skillHitResult.Damage *= 1f + damageIncrease;
		}
	}
}
