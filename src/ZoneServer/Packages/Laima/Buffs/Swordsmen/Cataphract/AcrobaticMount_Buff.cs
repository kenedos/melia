using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Cataphract
{
	/// <summary>
	/// Handler for the Acrobatic Mount buff.
	/// Grants damage and block penetration bonuses after mounted dash.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.AcrobaticMount_Buff)]
	public class AcrobaticMount_BuffOverride : BuffHandler, IBuffCombatAttackBeforeBonusesHandler
	{
		private const float BaseFinalDamageRate = 0.10f;
		private const float FinalDamageRatePerLevel = 0.01f;
		private const float BaseBlockPenRate = 0.10f;
		private const float BlockPenRatePerLevel = 0.01f;
		private const float BaseBlockPenFixed = 10f;
		private const float BlockPenFixedPerLevel = 4f;
		private const float MovementSpeedBonus = 5f;

		/// <summary>
		/// Applies property modifiers when buff activates.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			var blockPenFixed = BaseBlockPenFixed + BlockPenFixedPerLevel * skillLevel;
			var blockPenRate = BaseBlockPenRate + BlockPenRatePerLevel * skillLevel;

			AddPropertyModifier(buff, target, PropertyName.BLK_BREAK_BM, blockPenFixed);
			AddPropertyModifier(buff, target, PropertyName.BLK_BREAK_RATE_BM, blockPenRate);
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, MovementSpeedBonus);
		}

		/// <summary>
		/// Removes property modifiers when buff ends.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.BLK_BREAK_BM);
			RemovePropertyModifier(buff, target, PropertyName.BLK_BREAK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
		}

		/// <summary>
		/// Applies final damage bonus before attribute/race bonuses.
		/// </summary>
		public void OnAttackBeforeBonuses(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var skillLevel = buff.NumArg1;
			var damageIncrease = BaseFinalDamageRate + FinalDamageRatePerLevel * skillLevel;

			// Cataphract33: Acrobatic Mount: Enhance - increases damage bonus by 0.5% per level
			if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Cataphract33, out var abilityLevel))
				damageIncrease *= 1f + (abilityLevel * 0.005f);

			skillHitResult.Damage *= 1f + damageIncrease;
		}
	}
}
