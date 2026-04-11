//--- Melia Script ----------------------------------------------------------
// Equipment Combat Modifiers
//--- Description -----------------------------------------------------------
// Combat calculation modifiers triggered by equipped items (weapons, armor,
// accessories). These implement special item effects like SFR bonuses,
// conditional damage modifiers, etc., mapped from client SpcItem triggers
// (scptrigger_Item*.xml).
//
// Each modifier is keyed by the item's numeric ID and uses the
// CombatCalcModifier attribute system. The "new system" dispatcher in
// combat_modifiers.cs looks up by item ID without Attack/Defense suffix.
//
// Phase mapping:
//   BeforeAttack      -> BeforeCalc
//   Attack            -> BeforeBonuses
//   BeforeAttacked    -> BeforeCalc
//   Attacked          -> BeforeBonuses
//   AfterHitScript_*  -> AfterCalc
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Extensions;
using Yggdrasil.Util;

public class EquipmentCombatModifiersScript : GeneralScript
{
	//=======================================================================
	// Helper: Check if attacker has item equipped
	//=======================================================================
	private static bool AttackerHasItem(ICombatEntity attacker, int itemId)
		=> attacker is Character ch && ch.Inventory.IsEquipped(itemId);

	private static bool TargetHasItem(ICombatEntity target, int itemId)
		=> target is Character ch && ch.Inventory.IsEquipped(itemId);

	//=======================================================================
	// BeforeAttack -> BeforeCalc
	//=======================================================================

	/// <summary>
	/// DAG03_102 / Flamberg Dagger (113002): +100% SFR to Rogue_Backstab
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 113002)]
	public static void DAG03_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 113002))
			return;

		if (skill.Id == SkillId.Rogue_Backstab)
			modifier.SkillFactorBonus += 1.0f;
	}

	/// <summary>
	/// DAG04_113 / Masinios Dagger (114013): +150% SFR to Rogue_Backstab
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 114013)]
	public static void DAG04_113(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 114013))
			return;

		if (skill.Id == SkillId.Rogue_Backstab)
			modifier.SkillFactorBonus += 1.5f;
	}

	/// <summary>
	/// SPR04_118 / Velcoffer Spear (244118): 5% chance to add lightning ATK * 10 bonus damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 244118)]
	public static void SPR04_118(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 244118))
			return;

		if (RandomProvider.Get().Next(100) < 5)
			modifier.BonusDamage += attacker.Properties.GetFloat(PropertyName.Lightning_Atk) * 10f;
	}

	/// <summary>
	/// HAND01_202 / Cafrisun Gloves (501202): +12% damage for Slash attacks vs Plate armor
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 501202)]
	public static void HAND01_202(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 501202))
			return;

		if (skill.Data.AttackType == SkillAttackType.Slash && target.ArmorMaterial == ArmorMaterialType.Iron)
			modifier.FinalDamageMultiplier += 0.12f;
	}

	/// <summary>
	/// HAND01_201 / Cafrisun Gloves (501201): +12% damage for Strike attacks vs Leather armor
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 501201)]
	public static void HAND01_201(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 501201))
			return;

		if (skill.Data.AttackType == SkillAttackType.Strike && target.ArmorMaterial == ArmorMaterialType.Leather)
			modifier.FinalDamageMultiplier += 0.12f;
	}

	/// <summary>
	/// HAND03_202 (503202): +25% damage for Slash attacks vs Plate armor
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 503202)]
	public static void HAND03_202(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 503202))
			return;

		if (skill.Data.AttackType == SkillAttackType.Slash && target.ArmorMaterial == ArmorMaterialType.Iron)
			modifier.FinalDamageMultiplier += 0.25f;
	}

	/// <summary>
	/// HAND02_204 (502204): +20% damage for Slash attacks vs Plate armor
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 502204)]
	public static void HAND02_204(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 502204))
			return;

		if (skill.Data.AttackType == SkillAttackType.Slash && target.ArmorMaterial == ArmorMaterialType.Iron)
			modifier.FinalDamageMultiplier += 0.20f;
	}

	/// <summary>
	/// HAND03_201 (503201): +25% damage for Strike attacks vs Leather armor
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 503201)]
	public static void HAND03_201(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 503201))
			return;

		if (skill.Data.AttackType == SkillAttackType.Strike && target.ArmorMaterial == ArmorMaterialType.Leather)
			modifier.FinalDamageMultiplier += 0.25f;
	}

	/// <summary>
	/// HAND02_203 (502203): +20% damage for Strike attacks vs Leather armor
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 502203)]
	public static void HAND02_203(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 502203))
			return;

		if (skill.Data.AttackType == SkillAttackType.Strike && target.ArmorMaterial == ArmorMaterialType.Leather)
			modifier.FinalDamageMultiplier += 0.20f;
	}

	/// <summary>
	/// RAP04_101 / Velcoffer Rapier (314101): 3% chance to add Holy ATK as bonus damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 314101)]
	public static void RAP04_101(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 314101))
			return;

		if (RandomProvider.Get().Next(100) < 3)
			modifier.BonusDamage += attacker.Properties.GetFloat(PropertyName.Holy_Atk);
	}

	/// <summary>
	/// SPR03_102 (243102): +50% SFR to Hoplite_ThrouwingSpear
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 243102)]
	public static void SPR03_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 243102))
			return;

		if (skill.Id == SkillId.Hoplite_ThrouwingSpear)
			modifier.SkillFactorBonus += 0.5f;
	}

	/// <summary>
	/// MAC03_102 (203102): +1000 flat damage to Sadhu_EctoplasmAttack
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 203102)]
	public static void MAC03_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 203102))
			return;

		if (skill.Id == SkillId.Sadhu_EctoplasmAttack)
			modifier.BonusDamage += attacker.Properties.GetFloat(PropertyName.MNA);
	}

	/// <summary>
	/// TSP04_107 (254107): +235 flat damage to Cataphract_Impaler
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 254107)]
	public static void TSP04_107(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 254107))
			return;

		if (skill.Id == SkillId.Cataphract_Impaler)
			modifier.BonusDamage += 235;
	}

	/// <summary>
	/// TSF03_111 (273111): +186 flat damage to Wizard_MagicMissile
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 273111)]
	public static void TSF03_111(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 273111))
			return;

		if (skill.Id == SkillId.Wizard_MagicMissile)
			modifier.BonusDamage += 186;
	}

	/// <summary>
	/// SWD04_106 (104106): Normal attack bonus damage = target's Block * 50%
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 104106)]
	public static void SWD04_106(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 104106))
			return;

		if (skill.IsNormalAttack)
			modifier.BonusDamage += target.Properties.GetFloat(PropertyName.BLK) * 0.5f;
	}

	/// <summary>
	/// SWD03_110 (103110): -50% damage vs Ghost armor targets (drawback),
	/// and 5-hit multi-hit on normal attack when buff active.
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 103110)]
	public static void SWD03_110(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 103110))
			return;

		// -50% damage vs Ghost armor targets
		if (target.ArmorMaterial == ArmorMaterialType.Ghost)
			modifier.FinalDamageMultiplier -= 0.5f;

		// Multi-hit on normal attack when active buff is up
		if (skill.IsNormalAttack && attacker.IsBuffActive(BuffId.SWD03_110_active_Buff))
		{
			attacker.StopBuff(BuffId.SWD03_110_active_Buff);
			attacker.StopBuff(BuffId.SWD03_110_Buff);
			modifier.HitCount = 5;
		}
	}

	/// <summary>
	/// TBW03_101 (163101): 100% crit vs Beast, -25% damage vs non-Beast
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 163101)]
	public static void TBW03_101(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 163101))
			return;

		if (target.Race == RaceType.Widling)
			modifier.ForcedCritical = true;
		else
			modifier.FinalDamageMultiplier -= 0.25f;
	}

	// --- 2% chance for 7x damage weapons (Primus weapons) ---

	/// <summary>
	/// SWD02_116 (102116): 2% chance for 7x damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 102116)]
	public static void SWD02_116(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 102116))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.DamageMultiplier += 6.0f;
	}

	/// <summary>
	/// TSW02_112 (122112): 2% chance for 7x damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 122112)]
	public static void TSW02_112(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 122112))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.DamageMultiplier += 6.0f;
	}

	/// <summary>
	/// STF02_111 (142111): 2% chance for 7x damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 142111)]
	public static void STF02_111(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 142111))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.DamageMultiplier += 6.0f;
	}

	/// <summary>
	/// TSF02_111 (272111): 2% chance for 7x damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 272111)]
	public static void TSF02_111(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 272111))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.DamageMultiplier += 6.0f;
	}

	/// <summary>
	/// TBW02_114 (162114): 2% chance for 7x damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 162114)]
	public static void TBW02_114(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 162114))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.DamageMultiplier += 6.0f;
	}

	/// <summary>
	/// BOW02_110 (182110): 2% chance for 7x damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 182110)]
	public static void BOW02_110(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 182110))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.DamageMultiplier += 6.0f;
	}

	/// <summary>
	/// MAC02_112 (202112): 2% chance for 7x damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 202112)]
	public static void MAC02_112(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 202112))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.DamageMultiplier += 6.0f;
	}

	/// <summary>
	/// TSW03_112 (123112): 2% chance to add SP recovery as bonus damage
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 123112)]
	public static void TSW03_112(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 123112))
			return;

		if (RandomProvider.Get().Next(100) < 2)
			modifier.BonusDamage += attacker.Properties.GetFloat(PropertyName.RSP);
	}

	// --- Elemental Necklaces: +50% SFR to matching attribute skills ---

	/// <summary>
	/// NECK02_127 / Agny Necklace (582127): +50% SFR to Fire skills
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 582127)]
	public static void NECK02_127(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 582127))
			return;

		switch (skill.Id)
		{
			case SkillId.Pyromancer_FireBall:
			case SkillId.Pyromancer_FireWall:
			case SkillId.Pyromancer_FirePillar:
			case SkillId.Pyromancer_HellBreath:
			case SkillId.Pyromancer_FlameGround:
			case SkillId.Elementalist_Meteor:
			case SkillId.PlagueDoctor_Incineration:
			case SkillId.Elementalist_Prominence:
			case SkillId.Alchemist_Combustion:
			case SkillId.Zealot_Immolation:
				modifier.SkillFactorBonus += 50;
				break;
		}
	}

	/// <summary>
	/// NECK02_128 / Lightning Necklace (582128): +50% SFR to Lightning skills
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 582128)]
	public static void NECK02_128(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 582128))
			return;

		switch (skill.Id)
		{
			case SkillId.Elementalist_Electrocute:
			case SkillId.Kriwi_Zaibas:
			case SkillId.Daoshi_DivinePunishment:
			case SkillId.Zealot_FanaticIllusion:
				modifier.SkillFactorBonus += 50;
				break;
		}
	}

	/// <summary>
	/// NECK02_129 / Ice Necklace (582129): +50% SFR to Ice skills
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 582129)]
	public static void NECK02_129(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 582129))
			return;

		switch (skill.Id)
		{
			case SkillId.Cryomancer_IceBolt:
			case SkillId.Cryomancer_IciclePike:
			case SkillId.Cryomancer_IceBlast:
			case SkillId.Cryomancer_SnowRolling:
			case SkillId.Cryomancer_FrostPillar:
			case SkillId.Elementalist_Hail:
			case SkillId.RuneCaster_Isa:
			case SkillId.Onmyoji_WaterShikigami:
				modifier.SkillFactorBonus += 50;
				break;
		}
	}

	/// <summary>
	/// NECK02_132 / Earth Necklace (582132): +50% SFR to Earth skills
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 582132)]
	public static void NECK02_132(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 582132))
			return;

		switch (skill.Id)
		{
			case SkillId.Elementalist_StormDust:
			case SkillId.Wizard_EarthQuake:
			case SkillId.Onmyoji_GreenwoodShikigami:
			case SkillId.Onmyoji_Toyou:
				modifier.SkillFactorBonus += 50;
				break;
		}
	}

	//=======================================================================
	// Attack -> BeforeBonuses
	//=======================================================================

	/// <summary>
	/// MAC04_104 / Toy Hammer (204104): Apply debuff to target on Slash/Strike melee skills
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 204104)]
	public static void MAC04_104(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 204104))
			return;

		//if ((skill.Data.AttackType == SkillAttackType.Slash || skill.Data.AttackType == SkillAttackType.Strike) && skill.Data.Attribute == AttributeType.Melee)
		{
			target.StartBuff(BuffId.item_toyhammer_debuff, 1, 0, TimeSpan.FromSeconds(10), attacker);
		}
	}

	/// <summary>
	/// MAC03_105 (203105): 3% chance to stun target on attack
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 203105)]
	public static void MAC03_105(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 203105))
			return;

		if (RandomProvider.Get().Next(100) < 3)
			target.StartBuff(BuffId.Stun, 1, 0, TimeSpan.FromSeconds(3), attacker);
	}

	/// <summary>
	/// MAC04_101 (204101): 3% chance to stun target on attack
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 204101)]
	public static void MAC04_101(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 204101))
			return;

		if (RandomProvider.Get().Next(100) < 3)
			target.StartBuff(BuffId.Stun, 1, 0, TimeSpan.FromSeconds(3), attacker);
	}

	/// <summary>
	/// BRC04_101 (604101): Apply Frenzy self buff when attack misses (target dodges)
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.OnDodge, 604101)]
	public static void BRC04_101(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 604101))
			return;

		if (skillHitResult.Result == HitResultType.Dodge && !attacker.IsBuffActive(BuffId.item_cicel2))
		{
			attacker.StartBuff(BuffId.item_cicel, 1, 0, TimeSpan.FromSeconds(5), attacker);
		}
			
	}

	/// <summary>
	/// TMAC04_113 (210413): 5% chance to apply debuff to target
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 210413)]
	public static void TMAC04_113(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 210413))
			return;

		if (RandomProvider.Get().Next(100) < 5)
			target.StartBuff(BuffId.ITEM_SKIACLIPS_THMACE, 1, 0, TimeSpan.FromSeconds(5), attacker);
	}

	//=======================================================================
	// BeforeAttacked -> BeforeCalc (defender-side)
	//=======================================================================

	/// <summary>
	/// SHD04_102 (224102): 8% chance to reduce incoming damage by 50%
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 224102)]
	public static void SHD04_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!TargetHasItem(target, 224102))
			return;

		if (RandomProvider.Get().Next(100) < 8)
			modifier.FinalDamageMultiplier -= 0.5f;
	}

	//=======================================================================
	// AfterHitScript_Attack -> AfterCalc (post-damage effects)
	//=======================================================================

	/// <summary>
	/// DAG04_118 / Skiaclips Dagger (114018): Apply debuff on critical hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 114018)]
	public static void DAG04_118(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 114018))
			return;

		if (skillHitResult.Result == HitResultType.Crit)
			target.StartBuff(BuffId.ITEM_SKIACLIPS_Dagger, 1, 0, TimeSpan.FromSeconds(15), attacker);
	}

	/// <summary>
	/// MUS04_108 (334108): 10% chance to apply block debuff on critical hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 334108)]
	public static void MUS04_108(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 334108))
			return;

		if (skillHitResult.Result == HitResultType.Crit && RandomProvider.Get().Next(100) < 10)
			target.StartBuff(BuffId.Item_BLOCK_Debuff, 1, 400, TimeSpan.FromSeconds(10), attacker);
	}

	/// <summary>
	/// HAND03_117 (504117): 15% lifesteal on Featherfoot_BloodSucking vs Mutant/Plant
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 504117)]
	public static void HAND03_117(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 504117))
			return;

		if (skill.Id == SkillId.Featherfoot_BloodSucking && (target.Race == RaceType.Paramune || target.Race == RaceType.Forester))
		{
			var healAmount = (int)(skillHitResult.Damage * 0.15f);
			if (healAmount > 0)
				attacker.Heal(healAmount, 0);
		}
	}

	/// <summary>
	/// DAG02_101 (112001): 1% chance to apply CriticalWound on hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 112001)]
	public static void DAG02_101(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 112001))
			return;

		if (RandomProvider.Get().Next(100) < 1)
		{
			var damage = attacker.Properties.GetFloat(PropertyName.MINPATK) * 0.50f;
			target.StartBuff(BuffId.CriticalWound, 1, damage, TimeSpan.FromSeconds(15), attacker);
		}
	}

	/// <summary>
	/// DAG04_101 (114001): 2% chance to poison on specific dagger skills
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 114001)]
	public static void DAG04_101(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 114001))
			return;

		if (skill.Id != SkillId.War_JustFrameDagger
			&& skill.Id != SkillId.Common_DaggerAries
			&& skill.Id != SkillId.Corsair_HexenDropper
			&& skill.Id != SkillId.Corsair_DustDevil)
			return;

		if (RandomProvider.Get().Next(100) < 2)
			target.StartBuff(BuffId.item_poison_fast, 1, 685, TimeSpan.FromSeconds(10), attacker);
	}

	/// <summary>
	/// TBW04_117 (164117): 10% chance to apply crit dodge debuff
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 164117)]
	public static void TBW04_117(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 164117))
			return;

		if (RandomProvider.Get().Next(100) < 10)
			target.StartBuff(BuffId.CRIDR_Debuff, 1, 10, TimeSpan.FromSeconds(5), attacker);
	}

	/// <summary>
	/// TSP04_109 (254109): 3% chance to apply Rotten debuff
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 254109)]
	public static void TSP04_109(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 254109))
			return;

		if (RandomProvider.Get().Next(100) < 3)
			target.StartBuff(BuffId.Common_Rotten, 1, 0, TimeSpan.FromSeconds(20), attacker);
	}

	/// <summary>
	/// TBW03_109 (163109): Apply debuff on hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 163109)]
	public static void TBW03_109(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 163109))
			return;

		target.StartBuff(BuffId.TBW03_109_Debuff, 1, 0, TimeSpan.FromSeconds(20), attacker);
	}

	/// <summary>
	/// TSP03_106 (253106): 33% chance to apply debuff
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 253106)]
	public static void TSP03_106(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 253106))
			return;

		if (RandomProvider.Get().Next(100) < 33)
			target.StartBuff(BuffId.TSP03_106_Debuff, 1, 0, TimeSpan.FromSeconds(30), attacker);
	}

	/// <summary>
	/// SWD03_110 AfterHit (103110): 5% chance to apply CriticalWound +
	/// self buff on Shark Cutter skill attacks
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 103110)]
	public static void SWD03_110_AfterHit(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 103110))
			return;

		if (skill.Id != SkillId.Common_shark)
			return;

		if (RandomProvider.Get().Next(100) < 5)
		{
			var damage = attacker.Properties.GetFloat(PropertyName.MINPATK) * 0.20f;
			target.StartBuff(BuffId.CriticalWound, 1, damage, TimeSpan.FromSeconds(30), attacker);
			attacker.StartBuff(BuffId.SWD03_110_Buff, TimeSpan.Zero);
		}
	}

	/// <summary>
	/// TSF03_102 (273102): 15% chance to freeze target on Ice skills
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 273102)]
	public static void TSF03_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 273102))
			return;

		if (skill.Data.Attribute != AttributeType.Ice)
			return;

		if (RandomProvider.Get().Next(100) < 15)
			target.StartBuff(BuffId.Freeze, 1, 0, TimeSpan.FromSeconds(3), attacker);
	}

	/// <summary>
	/// TSP03_103 (253103): 1% chance to apply Laideka debuff
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 253103)]
	public static void TSP03_103(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 253103))
			return;

		if (RandomProvider.Get().Next(100) < 1)
			target.StartBuff(BuffId.item_laideka, 1, 0, TimeSpan.FromSeconds(10), attacker);
	}

	/// <summary>
	/// TBW02_108 (162108): 2% chance to poison on normal attack
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 162108)]
	public static void TBW02_108(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 162108))
			return;

		if (skill.IsNormalAttack && RandomProvider.Get().Next(100) < 2)
			target.StartBuff(BuffId.item_poison, 1, 75, TimeSpan.FromSeconds(20), attacker);
	}

	/// <summary>
	/// TSF02_106 (272106): 20% chance to apply Fire debuff on HellBreath
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 272106)]
	public static void TSF02_106(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 272106))
			return;

		if (skill.Id == SkillId.Pyromancer_HellBreath && RandomProvider.Get().Next(100) < 20)
		{
			var damage = attacker.Level * 0.33f;
			target.StartBuff(BuffId.Fire, 1, damage, TimeSpan.FromSeconds(10), attacker);
		}
	}

	/// <summary>
	/// STF03_102 (143102): 5% chance to apply Fire debuff on hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 143102)]
	public static void STF03_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 143102))
			return;

		if (RandomProvider.Get().Next(100) < 5)
		{
			var damage = attacker.Properties.GetFloat(PropertyName.MINMATK) * 0.16f;
			target.StartBuff(BuffId.Fire, 1, damage, TimeSpan.FromSeconds(10), attacker);
		}
	}

	/// <summary>
	/// STF03_101 (143101): 1% chance to apply Temere debuff on normal attack
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 143101)]
	public static void STF03_101(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 143101))
			return;

		if (skill.IsNormalAttack && target.Rank != MonsterRank.Boss && RandomProvider.Get().Next(100) < 1)
			target.StartBuff(BuffId.item_temere, 1, 0, TimeSpan.FromSeconds(30), attacker);
	}

	/// <summary>
	/// TSP04_102 (254102): 3-hit multi-hit on L/XL monsters
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 254102)]
	public static void TSP04_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 254102))
			return;

		if (skill.IsNormalAttack && target is Mob mob && (mob.EffectiveSize == SizeType.L || mob.EffectiveSize == SizeType.XL))
			modifier.HitCount = 3;
	}

	/// <summary>
	/// TSP01_103 (251103): 3-hit multi-hit on L/XL monsters
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 251103)]
	public static void TSP01_103(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 251103))
			return;

		if (skill.IsNormalAttack && target is Mob mob && (mob.EffectiveSize == SizeType.L || mob.EffectiveSize == SizeType.XL))
			modifier.HitCount = 3;
	}

	/// <summary>
	/// TSP02_105 (252105): 3-hit multi-hit on L/XL monsters
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 252105)]
	public static void TSP02_105(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 252105))
			return;

		if (skill.IsNormalAttack && target is Mob mob && (mob.EffectiveSize == SizeType.L || mob.EffectiveSize == SizeType.XL))
			modifier.HitCount = 3;
	}

	/// <summary>
	/// STF04_124 (144124): +50% SFR and heal 30 SP on RuneCaster_Tiwaz hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 144124)]
	public static void STF04_124_SFR(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 144124))
			return;

		if (skill.Id == SkillId.RuneCaster_Tiwaz)
			modifier.SkillFactorBonus += 0.5f;
	}

	/// <summary>
	/// STF04_124 (144124): Heal 30 SP on RuneCaster_Tiwaz hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 144124)]
	public static void STF04_124(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 144124))
			return;

		if (skill.Id == SkillId.RuneCaster_Tiwaz)
			attacker.Heal(0, 30);
	}

	/// <summary>
	/// TSF04_126 (274126): Apply Slow debuff on Onmyoji_WaterShikigami hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 274126)]
	public static void TSF04_126(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 274126))
			return;

		if (skill.Id == SkillId.Onmyoji_WaterShikigami)
			target.StartBuff(BuffId.Slow_Debuff, 8, 0, TimeSpan.FromSeconds(5), attacker);
	}

	/// <summary>
	/// SWD04_124 (104124): Apply debuff on Swordman_Bash hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 104124)]
	public static void SWD04_124(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 104124))
			return;

		if (skill.Id == SkillId.Swordman_Bash)
			target.StartBuff(BuffId.ITEM_MISRUS_SWORD_DEBUFF, 1, 0, TimeSpan.FromSeconds(5), attacker);
	}

	//=======================================================================
	// AfterHitScript_Attacked -> AfterCalc (defender-side post-damage)
	//=======================================================================

	/// <summary>
	/// NECK03_106 (583106): Apply self-buff when hit by Magic skill
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 583106)]
	public static void NECK03_106(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!TargetHasItem(target, 583106))
			return;

		if (skill.Data.ClassType == SkillClassType.Magic)
			target.StartBuff(BuffId.item_NECK03_106, 1, 0, TimeSpan.FromSeconds(30), target);
	}

	/// <summary>
	/// NECK03_103 (583103): 15% chance to shock attacker when hit by Lightning
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 583103)]
	public static void NECK03_103(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!TargetHasItem(target, 583103))
			return;

		if (skill.Data.Attribute == AttributeType.Lightning && RandomProvider.Get().Next(100) < 15)
			attacker.StartBuff(BuffId.item_electricShock, 1, 405, TimeSpan.FromSeconds(1), target);
	}

	/// <summary>
	/// TSW03_102 (123102): 2% chance to debuff attacker when hit by Magic
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 123102)]
	public static void TSW03_102(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!TargetHasItem(target, 123102))
			return;

		if (skill.Data.ClassType == SkillClassType.Magic && RandomProvider.Get().Next(100) < 2)
			attacker.StartBuff(BuffId.item_wizardSlayer, 1, 0, TimeSpan.FromSeconds(15), target);
	}

	/// <summary>
	/// TSW03_111 (123111): 1.5% chance to debuff attacker when hit by Magic
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, 123111)]
	public static void TSW03_111(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!TargetHasItem(target, 123111))
			return;

		if (skill.Data.ClassType == SkillClassType.Magic && RandomProvider.Get().NextDouble() * 100 < 1.5)
			attacker.StartBuff(BuffId.TSW03_111_Buff, 1, 0, TimeSpan.FromSeconds(30), target);
	}

	//=======================================================================
	// Attacked -> BeforeBonuses (defender-side)
	//=======================================================================

	/// <summary>
	/// SHD03_104 (223104): 1% chance to apply HP regen buff when hit
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, 223104)]
	public static void SHD03_104(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!TargetHasItem(target, 223104))
			return;

		if (RandomProvider.Get().Next(100) < 1)
			target.StartBuff(BuffId.SHD03_104_Buff, 1, 0, TimeSpan.FromSeconds(5), target);
	}

	//=======================================================================
	// Staff attack bonus (TSF03_109)
	//=======================================================================

	/// <summary>
	/// TSF03_109 (273109): Staff attack (C key) deals bonus damage equal to MATK
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 273109)]
	public static void TSF03_109(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 273109))
			return;

		if (skill.Id == SkillId.Common_StaffAttack)
			modifier.BonusDamage += attacker.Properties.GetFloat(PropertyName.MATK);
	}

	//=======================================================================
	// Trinket combat modifiers
	//=======================================================================

	/// <summary>
	/// TRK04_106 (694006): +10% damage to bleeding targets
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 694006)]
	public static void TRK04_106(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 694006))
			return;

		if (target.IsBuffActive(BuffId.HeavyBleeding))
			modifier.FinalDamageMultiplier += 0.10f;
	}

	/// <summary>
	/// TRK04_110 (694010): +15% damage to shield-equipped characters
	/// </summary>
	[CombatCalcModifier(CombatCalcPhase.BeforeCalc, 694010)]
	public static void TRK04_110(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!AttackerHasItem(attacker, 694010))
			return;

		if (target is Character ch)
		{
			var leftHand = ch.Inventory.GetEquip(EquipSlot.LeftHand);
			if (leftHand != null && leftHand.Data.EquipType1 == EquipType.Shield)
				modifier.FinalDamageMultiplier += 0.15f;
		}
	}

	//=======================================================================
	// TODO: Effects requiring additional infrastructure
	//=======================================================================
	// TBW04_999 (164999): SPCI_TGT_EXPLOSION on Missile skills - needs AoE spawn system
	// MAC03_110 (203110): SPCI_BUFF2 on DamType=2 - needs DamType check
	// TBW04_123 (164123): SPCI_REDUCE_ITEM_COOLTIME for MultiShot - needs cooldown reduction API
	// RAP04_112 (314112): SPCI_REMOVE_BUFF on normal attack with buff check - needs buff interaction
	// TSW01_101 (121101): SPCI_ADD_SKILL_RANGE for skill 222 - needs skill range modifier
	// SSword_knife_D: SPCI_DAGGER_MULTIPLE_HIT - item not found in DB
	// SWD01_932: SPCI_TGT_KB knockback - item not found in DB
	// BRC03_102 (603102): Plant kill herb gathering effect - needs kill event hook
	// TRK04_109 (694009): +10% damage from magic circles - needs pad source tracking
}
