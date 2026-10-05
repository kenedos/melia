using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Grants HP-scaled final damage and Accuracy while the user has
	/// a two-handed sword equipped.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DeedsOfValor)]
	public class DeedsOfValor_BuffOverride :
		BuffHandler,
		IBuffCombatAttackBeforeCalcHandler,
		IBuffCombatAttackAfterCalcHandler
	{
		/// <summary>
		/// Maximum HP required for each 1% final-damage bonus.
		/// </summary>
		private const float HpPerOnePercentFinalDamage = 2_000f;

		/// <summary>
		/// Maximum final-damage bonus granted by Deeds of Valor.
		/// </summary>
		private const float MaximumFinalDamageBonus = 0.25f;

		/// <summary>
		/// Accuracy granted equals maximum HP divided by this value.
		/// </summary>
		private const float MaximumHpPerAccuracy = 200f;

		public override void OnActivate(
			Buff buff,
			ActivationType activationType
		)
		{
			var accuracyBonus = GetAccuracyBonus(buff.Target);

			AddPropertyModifier(
				buff,
				buff.Target,
				PropertyName.HR_BM,
				accuracyBonus
			);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(
				buff,
				buff.Target,
				PropertyName.HR_BM
			);
		}

		/// <summary>
		/// Recalculates the Accuracy bonus immediately before every attack.
		/// This accounts for changes to maximum HP or the equipped weapon.
		/// </summary>
		public void OnAttackBeforeCalc(
			Buff buff,
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult
		)
		{
			if (attacker != buff.Target)
				return;

			var accuracyBonus = GetAccuracyBonus(attacker);

			UpdatePropertyModifier(
				buff,
				attacker,
				PropertyName.HR_BM,
				accuracyBonus
			);
		}

		/// <summary>
		/// Applies the HP-scaled final-damage bonus to physical attacks.
		/// </summary>
		public void OnAttackAfterCalc(
			Buff buff,
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult
		)
		{
			if (
				attacker != buff.Target ||
				target == null ||
				target.IsDead ||
				skill == null ||
				skillHitResult.Damage <= 0f
			)
			{
				return;
			}

			if (
				skill.Data.ClassType == SkillClassType.Magic ||
				skill.Data.ClassType == SkillClassType.TrueDamage
			)
			{
				return;
			}

			if (!IsUsingTwoHandedSword(attacker))
				return;

			var maximumHp = Math.Max(
				0f,
				attacker.Properties.GetFloat(
					PropertyName.MHP
				)
			);

			var finalDamageBonus = Math.Min(
				MaximumFinalDamageBonus,
				maximumHp /
					HpPerOnePercentFinalDamage /
					100f
			);

			if (finalDamageBonus > 0f)
			{
				skillHitResult.Damage *=
					1f + finalDamageBonus;
			}
		}

		/// <summary>
		/// Returns the Accuracy bonus based on maximum HP.
		/// Returns zero if a two-handed sword is not equipped.
		/// </summary>
		private static float GetAccuracyBonus(
			ICombatEntity attacker
		)
		{
			if (!IsUsingTwoHandedSword(attacker))
				return 0f;

			var maximumHp = Math.Max(
				0f,
				attacker.Properties.GetFloat(
					PropertyName.MHP
				)
			);

			return maximumHp / MaximumHpPerAccuracy;
		}

		/// <summary>
		/// Checks whether the character has a two-handed sword equipped.
		/// </summary>
		private static bool IsUsingTwoHandedSword(
			ICombatEntity attacker
		)
		{
			return
				attacker is Character character &&
				character.TryGetEquipItem(
					EquipSlot.RightHand,
					out var weapon
				) &&
				weapon.Data.EquipType1 == EquipType.THSword;
		}
	}
}
