using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Increases the damage of Fencer skills while using a Rapier.
	/// Grants additional damage when directly confronting the target and scales with DEX.
	/// Includes Epee Garde: Enhance ability calculation.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EpeeGarde_Buff)]
	public class EpeeGarde_BuffOverride : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float DamageBonusPerLevel = 0.05f;
		private const float FrontalDamageBonus = 0.20f;
		private const float DexRequiredPerBonusStep = 50f;
		private const float DexBonusPerStep = 0.01f;
		private const float MaximumDexDamageBonus = 0.20f;
		private const float FrontalConeDotThreshold = 0.50f;

		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker != buff.Target || target == null || skill == null || skillHitResult.Damage <= 0f)
				return;

			if (!skill.Id.ToString().StartsWith("Fencer_", StringComparison.Ordinal))
				return;

			if (!attacker.TryGetEquipItem(EquipSlot.RightHand, out var weapon) || weapon.Data.EquipType1 != EquipType.Rapier)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var skillDamageBonus = skillLevel * DamageBonusPerLevel;

			// Aplicação da Enhance (Fencer12: +0.5% por nível + 10% bônus no lvl 100)
			if (attacker is Character character && character.TryGetActiveAbilityLevel(AbilityId.Fencer12, out var enhanceLevel) && enhanceLevel > 0)
			{
				var enhanceBonusRate = 1f + (enhanceLevel * 0.005f) + (enhanceLevel >= 100 ? 0.10f : 0f);
				skillDamageBonus *= enhanceBonusRate;
			}

			var dexDamageBonus = this.GetDexDamageBonus(attacker);
			var frontalDamageBonus = this.IsInFrontOfTarget(attacker, target) ? FrontalDamageBonus : 0f;
			var totalDamageBonus = skillDamageBonus + dexDamageBonus + frontalDamageBonus;

			skillHitResult.Damage *= 1f + totalDamageBonus;
		}

		private float GetDexDamageBonus(ICombatEntity attacker)
		{
			var dex = Math.Max(0f, attacker.Properties.GetFloat(PropertyName.DEX));
			var bonusSteps = MathF.Floor(dex / DexRequiredPerBonusStep);
			var damageBonus = bonusSteps * DexBonusPerStep;

			return Math.Min(damageBonus, MaximumDexDamageBonus);
		}

		private bool IsInFrontOfTarget(ICombatEntity attacker, ICombatEntity target)
		{
			var targetToAttacker = target.Position.GetDirection(attacker.Position);
			var dotProduct = target.Direction.Cos * targetToAttacker.Cos + target.Direction.Sin * targetToAttacker.Sin;

			return dotProduct >= FrontalConeDotThreshold;
		}
	}
}
