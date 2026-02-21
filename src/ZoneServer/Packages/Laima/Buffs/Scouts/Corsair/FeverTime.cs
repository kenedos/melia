using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Fever Time buff.
	/// Increases final damage based on Jolly Roger skill level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.FeverTime)]
	public class FeverTimeOverride : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const float BaseDamageBonus = 1.0f; // +100% Damage
		private const float DamageBonusPerLevel = 0.1f; // +10% Damage per SkillLv
		private const float AbilityBonusPerLevel = 0.005f; // +0.5% Damage per Corsair20 ability level

		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (buff.Caster is ICombatEntity caster && caster.TryGetSkillLevel(SkillId.Corsair_JollyRoger, out var level))
			{
				var damageBonus = BaseDamageBonus + (level * DamageBonusPerLevel);

				if (caster.TryGetActiveAbilityLevel(AbilityId.Corsair20, out var abilityLevel))
					damageBonus += abilityLevel * AbilityBonusPerLevel;

				skillHitResult.Damage += (int)(skillHitResult.Damage * damageBonus);
			}
		}
	}
}
