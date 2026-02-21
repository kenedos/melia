using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Items.Effects
{
	/// <summary>
	/// Helper methods for registering common item hook patterns
	/// </summary>
	public static class ItemHookHelper
	{
		/// <summary>
		/// Registers a final damage multiplier hook that triggers on condition.
		/// Uses FinalDamageMultiplier which is applied after all calculations (skill factor, defense, bonuses)
		/// to provide accurate percentage-based damage increases for equipment/card bonuses.
		/// </summary>
		public static void RegisterConditionalDamageHook(Character character, Item item, ItemHookType hookType, Func<ICombatEntity, ICombatEntity, Skill, bool> condition, float damageMultiplier)
		{
			ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
				(itm, attacker, target, skill, modifier, skillHitResult) =>
				{
					if (condition(attacker, target, skill))
					{
						modifier.FinalDamageMultiplier += damageMultiplier;
					}
				});
		}

		/// <summary>
		/// Registers a damage reduction hook
		/// </summary>
		public static void RegisterDamageReductionHook(Character character, Item item, ItemHookType hookType, Func<ICombatEntity, ICombatEntity, Skill, bool> condition, float reductionRate)
		{
			ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
				(itm, attacker, target, skill, modifier, skillHitResult) =>
				{
					if (condition(attacker, target, skill))
						skillHitResult.Damage *= (1f - reductionRate);
				});
		}

		/// <summary>
		/// Registers a physical attack bonus hook
		/// </summary>
		public static void RegisterPAtkBonusHook(Character character, Item item, ItemHookType hookType, float bonusPatk)
		{
			ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
				(itm, attacker, target, skill, modifier, skillHitResult) =>
				{
					modifier.BonusPAtk += bonusPatk;
				});
		}

		/// <summary>
		/// Registers a magical attack bonus hook
		/// </summary>
		public static void RegisterMAtkBonusHook(Character character, Item item, ItemHookType hookType, float bonusMatk)
		{
			ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
				(itm, attacker, target, skill, modifier, skillHitResult) =>
				{
					modifier.BonusMAtk += bonusMatk;
				});
		}

		/// <summary>
		/// Registers an attack speed bonus hook
		/// </summary>
		public static void RegisterAttackSpeedHook(Character character, Item item, float aspdBonus)
		{
			character.Properties.Modify(PropertyName.ASPD, aspdBonus);

			if (CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
			{
				metadata.AspdModifier = aspdBonus;
			}
		}

		/// <summary>
		/// Creates a condition checker for monster race
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckMonsterRace(string raceStr)
		{
			if (!Enum.TryParse<RaceType>(raceStr, true, out var race))
			{
				if (!Enum.TryParse<MoveType>(raceStr, true, out var moveType))
				{
					return (attacker, target, skill) => false;
				}
				else
				{
					return (attacker, target, skill) => target.MoveType == moveType;
				}
			}
			else
			{
				return (attacker, target, skill) => target.Race == race;
			}
		}

		/// <summary>
		/// Creates a condition checker for monster size
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckMonsterSize(string sizeStr)
		{
			var sizes = sizeStr.Split('/');

			return (attacker, target, skill) =>
			{
				foreach (var size in sizes)
				{
					if (size == "PC" && target is Character)
						return true;

					if (target is Mob monster && Enum.TryParse<SizeType>(size, true, out var sizeType) && monster.EffectiveSize == sizeType)
						return true;
				}
				return false;
			};
		}

		/// <summary>
		/// Creates a condition checker for skill attribute
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckSkillAttribute(string attributeStr)
		{
			if (!Enum.TryParse<AttributeType>(attributeStr, true, out var attribute))
				return (attacker, target, skill) => false;

			return (attacker, target, skill) =>
			{
				var skillAttribute = skill.Data.Attribute;
				return skillAttribute == attribute;
			};
		}

		/// <summary>
		/// Creates a condition checker for monster/target attribute
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckMonsterAttribute(string attributeStr)
		{
			if (!Enum.TryParse<AttributeType>(attributeStr, true, out var attribute))
				return (attacker, target, skill) => false;

			return (attacker, target, skill) =>
			{
				var targetAttr = target.Attribute;
				return targetAttr == attribute;
			};
		}

		/// <summary>
		/// Creates a condition checker for monster rank (Boss, Elite, etc.)
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckMonsterRank(string rankStr)
		{
			if (rankStr == "Boss")
				return (attacker, target, skill) => target is Mob monster && monster.Rank == MonsterRank.Boss;

			if (rankStr == "Elite")
				return (attacker, target, skill) => target is Mob monster && monster.IsBuffActive(BuffId.EliteMonsterBuff);

			return (attacker, target, skill) => false;
		}

		/// <summary>
		/// Creates a condition checker for target having a specific buff
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckTargetHasBuff(string buffKeyword)
		{
			return (attacker, target, skill) =>
			{
				var buffs = target.Components.Get<World.Actors.CombatEntities.Components.BuffComponent>();
				if (buffs == null)
					return false;

				var matchingBuffs = buffs.GetAll(b => b.Id.ToString().Contains(buffKeyword, StringComparison.OrdinalIgnoreCase));
				return matchingBuffs.Count > 0;
			};
		}

		/// <summary>
		/// Creates a condition checker for equipment attack type
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckEquipmentAttackType(Character character, string attackTypeStr)
		{
			if (!Enum.TryParse<SkillAttackType>(attackTypeStr, true, out var attackType))
				return (attacker, target, skill) => false;

			return (attacker, target, skill) =>
			{
				if (character.TryGetEquipItem(EquipSlot.RightHand, out var weapon))
					return weapon.Data.AttackType == attackType;

				return false;
			};
		}

		/// <summary>
		/// Creates a condition checker for missile type (Arrow/Cannon/Gun)
		/// </summary>
		public static Func<ICombatEntity, ICombatEntity, Skill, bool> CheckMissileType(Character character, string missileTypesStr)
		{
			var types = missileTypesStr.Split('/');

			return (attacker, target, skill) =>
			{
				if (!character.TryGetEquipItem(EquipSlot.RightHand, out var weapon))
					return false;

				var weaponType = weapon.Data.ClassName;

				foreach (var type in types)
				{
					if (type == "Arrow" && weaponType.Contains("Bow", StringComparison.OrdinalIgnoreCase))
						return true;
					if (type == "Cannon" && weaponType.Contains("Cannon", StringComparison.OrdinalIgnoreCase))
						return true;
					if (type == "Gun" && (weaponType.Contains("Pistol", StringComparison.OrdinalIgnoreCase) || weaponType.Contains("Musket", StringComparison.OrdinalIgnoreCase)))
						return true;
				}
				return false;
			};
		}

		/// <summary>
		/// Registers a damage multiplier hook using the raw DamageMultiplier property.
		/// WARNING: This applies before defense and skill factor calculations, which can result in
		/// inflated damage values. Use RegisterConditionalDamageHook with FinalDamageMultiplier instead
		/// for accurate percentage-based damage increases. This method is kept only for compatibility
		/// with specific buff/debuff scenarios that need early-stage multipliers.
		/// </summary>
		public static void RegisterConditionalEarlyDamageHook(Character character, Item item, ItemHookType hookType, Func<ICombatEntity, ICombatEntity, Skill, bool> condition, float damageMultiplier)
		{
			ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
				(itm, attacker, target, skill, modifier, skillHitResult) =>
				{
					if (condition(attacker, target, skill))
					{
						modifier.DamageMultiplier += damageMultiplier;
					}
				});
		}

		/// <summary>
		/// Gets the appropriate hook type based on card use type.
		/// Maps card trigger events to combat calculation stages.
		/// </summary>
		public static ItemHookType GetHookTypeForCardUseType(CardUseType useType)
		{
			return useType switch
			{
				CardUseType.BeforeAttack => ItemHookType.AttackBeforeBonuses,
				CardUseType.Attack => ItemHookType.AttackAfterBonuses,
				CardUseType.Attacked => ItemHookType.DefenseBeforeCalc,
				CardUseType.UseItem => ItemHookType.ItemUse,
				CardUseType.Kill => ItemHookType.Kill,
				CardUseType.Dead => ItemHookType.Dead,
				_ => throw new ArgumentException($"UseType {useType} does not map to a combat hook type")
			};
		}
	}
}
