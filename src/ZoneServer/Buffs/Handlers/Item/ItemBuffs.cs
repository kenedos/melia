using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for item_cmine_red_buff, which provides attack bonuses.
	/// </summary>
	[BuffHandler(BuffId.item_cmine_red_buff)]
	public class item_cmine_red_buff : BuffHandler
	{
		private const int AttackBonus = 20;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_BM, AttackBonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.MATK_BM, AttackBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_BM);
		}
	}

	/// <summary>
	/// Handle for item_cmine_blue_buff, which provides defense bonuses.
	/// </summary>
	[BuffHandler(BuffId.item_cmine_blue_buff)]
	public class item_cmine_blue_buff : BuffHandler
	{
		private const int DefenseBonus = 8;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, DefenseBonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, DefenseBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}

	/// <summary>
	/// Handle for item_cmine_yellow_buff, which rapidly regenerates Stamina.
	/// </summary>
	[BuffHandler(BuffId.item_cmine_yellow_buff)]
	public class item_cmine_yellow_buff : BuffHandler
	{
		private const float StaminaRegen = 99999f;

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
			{
				character.ModifyStamina((int)StaminaRegen);
			}
		}
	}

	/// <summary>
	/// Handle for item_toyhammer_debuff, which explodes when it reaches 10 stacks.
	/// </summary>
	[BuffHandler(BuffId.item_toyhammer_debuff)]
	public class item_toyhammer_debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Buff has no effect on PCs.
			if (buff.Target is Character)
			{
				buff.Target.RemoveBuff(buff.Id);
			}
			if (buff.OverbuffCounter >= 10)
				buff.Target.RemoveBuff(buff.Id);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.OverbuffCounter >= 10)
				buff.Target.RemoveBuff(buff.Id);
		}

		public override void OnEnd(Buff buff)
		{
			// The debuff explodes if it was fully stacked (i.e., it had 9 stacks and was about to get a 10th).
			if (buff.OverbuffCounter < 9) return;

			if (buff.Caster is not ICombatEntity caster || buff.Target.Map == null) return;

			buff.Target.PlayEffect("F_archer_explosiontrap_hit_explosion");

			var damage = caster.Properties.GetFloat(PropertyName.MINPATK) * 5f;

			var nearbyEnemies = buff.Target.Map.GetAttackableEnemiesInPosition(caster, buff.Target.Position, 30);
			foreach (var enemy in nearbyEnemies)
			{
				enemy.TakeSimpleHit(damage, caster, SkillId.None);
				enemy.StopBuff(buff.Id);
				// The original script breaks after hitting the first enemy.
				break;
			}
		}
	}

	/// <summary>
	/// Handle for item_set_013_buff, which stores its overbuff count.
	/// </summary>
	[BuffHandler(BuffId.item_set_013_buff)]
	public class item_set_013_buff : BuffHandler
	{
		private const string VarOverbuff = "Melia.ItemSet.Set013.Overbuff";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Store the current overbuff count. This might be read by another system.
			buff.Vars.SetFloat(VarOverbuff, buff.OverbuffCounter);
		}
	}

	/// <summary>
	/// Handle for item_shovel_buff, which creates a dropped item.
	/// </summary>
	[BuffHandler(BuffId.item_shovel_buff)]
	public class item_shovel_buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			if (target is not Character character) return;

			// Assuming GetMaterialItem and CREATE_DROP_ITEM can be translated.
			// This might involve looking up a drop table or a specific item associated with the map/area.
			var item = new Item(645620);
			//if (Item.TryCreateFromMaterial(character, out var item))
			{
				item.Drop(character.Map, character.Position);
				character.AddAchievementPoint("Shovel", 1);
			}

			// This is a one-shot effect, so remove the buff immediately.
			character.StopBuff(buff.Id);
		}
	}

	/// <summary>
	/// Handle for item_set_016_buff, which restores SP over time.
	/// </summary>
	[BuffHandler(BuffId.item_set_016_buff)]
	public class item_set_016_buff : BuffHandler
	{
		private const float SpHeal = 40f;

		private void HealTick(Buff buff)
		{
			if (buff.Target is Character character)
			{
				character.Heal(0, SpHeal);
			}
		}

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_set_016_buff");
			this.HealTick(buff);
		}

		public override void WhileActive(Buff buff)
		{
			this.HealTick(buff);
		}
	}

	// Handlers for empty buffs that might be used as markers or prerequisites.
	[BuffHandler(BuffId.item_set_013pre_buff)]
	public class item_set_013pre_buff : BuffHandler { }

	[BuffHandler(BuffId.item_set_016pre_buff)]
	public class item_set_016pre_buff : BuffHandler { }

	/// <summary>
	/// Handle for item_wizardSlayer, which increases MDEF based on character level.
	/// </summary>
	[BuffHandler(BuffId.item_wizardSlayer)]
	public class item_wizardSlayer : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_wizardSlayer");

			var mdefBonus = (float)Math.Floor(target.Properties.GetFloat(PropertyName.Lv));
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, mdefBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}

	/// <summary>
	/// Handle for item_temere, which converts half of MDEF into DEF.
	/// </summary>
	[BuffHandler(BuffId.item_temere)]
	public class item_temere : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_temere");

			var mdef = target.Properties.GetFloat(PropertyName.MDEF);
			var value = (float)Math.Floor(mdef / 2);

			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, -value);
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, value);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}
	}

	/// <summary>
	/// Base class for poison item buffs that deal damage over time.
	/// </summary>
	/// <remarks>
	/// NumArg2: Snapshotted damage per tick (pre-calculated by equipment modifier)
	/// </remarks>
	public abstract class item_poison_base : DamageOverTimeBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_poison");
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}
	}

	[BuffHandler(BuffId.item_poison)]
	public class item_poison : item_poison_base { }

	[BuffHandler(BuffId.item_poison_fast)]
	public class item_poison_fast : item_poison_base { }

	/// <summary>
	/// Handle for item_laideka, which reduces movement speed.
	/// </summary>
	[BuffHandler(BuffId.item_laideka)]
	public class item_laideka : BuffHandler
	{
		private const int MspdPenalty = 15;
		public override void OnActivate(Buff buff, ActivationType activationType) => AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -MspdPenalty);
		public override void OnEnd(Buff buff) => RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
	}

	/// <summary>
	/// Handle for item_electricShock, which deals lightning damage over time.
	/// </summary>
	/// <remarks>
	/// NumArg2: Snapshotted damage per tick (pre-calculated by equipment modifier)
	/// </remarks>
	[BuffHandler(BuffId.item_electricShock)]
	public class item_electricShock : DamageOverTimeBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_electricShock");
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Lightning;
		}
	}

	/// <summary>
	/// Handle for item_magicAmulet_1, which reduces DEF and MDEF by 50%.
	/// </summary>
	[BuffHandler(BuffId.item_magicAmulet_1)]
	public class item_magicAmulet_1 : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_magicAmulet_1");

			var defPenalty = (float)Math.Floor(target.Properties.GetFloat(PropertyName.DEF) / 2);
			var mdefPenalty = (float)Math.Floor(target.Properties.GetFloat(PropertyName.MDEF) / 2);

			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -defPenalty);
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, -mdefPenalty);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}

	/// <summary>
	/// Handle for item_magicAmulet_4, which heals HP over time.
	/// </summary>
	[BuffHandler(BuffId.item_magicAmulet_4)]
	public class item_magicAmulet_4 : BuffHandler
	{
		private void HealTick(Buff buff) => buff.Target.Heal(buff.NumArg2, 0);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_magicAmulet_4");
			this.HealTick(buff);
		}

		public override void WhileActive(Buff buff) => this.HealTick(buff);
	}

	/// <summary>
	/// Handle for item_magicAmulet_3, which reduces current SP by a flat amount.
	/// </summary>
	[BuffHandler(BuffId.item_magicAmulet_3)]
	public class item_magicAmulet_3 : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_magicAmulet_3");
			if (buff.Target is Character character)
			{
				character.Heal(0, -buff.NumArg2);
			}
			// One-shot effect, remove immediately.
			buff.Target.StopBuff(buff.Id);
		}
	}

	[BuffHandler(BuffId.item_magicAmulet_2)]
	public class item_magicAmulet_2 : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_magicAmulet_2");
		}
	}

	/// <summary>
	/// Handle for item_linne, which grants a reward after 15 stacks.
	/// </summary>
	[BuffHandler(BuffId.item_linne)]
	public class item_linne : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.OverbuffCounter >= 15)
			{
				var target = buff.Target;
				Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_linne");
				// Assuming a script function exists to grant a reward.
				// ScriptableFunctions.Run("GIVE_REWARD", target, "ITEM_LINNE", "Drop");
				target.StopBuff(buff.Id);
			}
		}
	}

	/// <summary>
	/// Handle for item_cicel, which transforms into item_cicel2 after 8 stacks.
	/// </summary>
	[BuffHandler(BuffId.item_cicel)]
	public class item_cicel : BuffHandler
	{
		private static readonly TimeSpan Cicel2Duration = TimeSpan.FromSeconds(30);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.OverbuffCounter >= 8)
			{
				var target = buff.Target;
				target.StartBuff(BuffId.item_cicel2, Cicel2Duration, target);
			}
		}
	}

	/// <summary>
	/// Handle for item_cicel2, a temporary powerful buff.
	/// </summary>
	[BuffHandler(BuffId.item_cicel2)]
	public class item_cicel2 : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_BM, 235);
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, 10);
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -88);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}
	}

	/// <summary>
	/// Handle for item_effigyCount, which stores a value on the character.
	/// </summary>
	[BuffHandler(BuffId.item_effigyCount)]
	public class item_effigyCount : BuffHandler
	{
		private const string VarEffigyBonus = "Melia.Item.EffigyCount.Bonus";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.SetTempVar(VarEffigyBonus, buff.NumArg2);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveTempVar(VarEffigyBonus);
		}
	}

	/// <summary>
	/// Handle for item_armorBreak, which decreases Defense. The penalty amount is passed as NumArg2.
	/// </summary>
	[BuffHandler(BuffId.item_armorBreak)]
	public class item_armorBreak : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -buff.NumArg2);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}
	}

	/// <summary>
	/// Handle for item_NECK03_106. Trades DEF for MDEF.
	/// DEF_BM -= 6*over, MDEF_BM += 6*over
	/// </summary>
	[BuffHandler(BuffId.item_NECK03_106)]
	public class item_NECK03_106 : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var mod = 6f * buff.OverbuffCounter;
			UpdatePropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -mod);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, mod);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}

	/// <summary>
	/// Handle for TBW03_109_Debuff. Reduces crit dodge rate.
	/// CRTDR_BM -= 8*over
	/// </summary>
	[BuffHandler(BuffId.TBW03_109_Debuff)]
	public class TBW03_109_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var mod = 8f * buff.OverbuffCounter;
			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -mod);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}

	/// <summary>
	/// Handle for Item_BLOCK_Debuff. Reduces block rate.
	/// BLK_BM -= arg2
	/// </summary>
	[BuffHandler(BuffId.Item_BLOCK_Debuff)]
	public class Item_BLOCK_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, -buff.NumArg2);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
		}
	}

	/// <summary>
	/// Handle for CRIDR_Debuff. Reduces crit dodge rate based on arg2 and stacks.
	/// CRTDR_BM -= arg2*over
	/// </summary>
	[BuffHandler(BuffId.CRIDR_Debuff)]
	public class CRIDR_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var mod = buff.NumArg2 * buff.OverbuffCounter;
			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -mod);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}

	/// <summary>
	/// Handle for MAC03_110_Debuff. Reduces defense by 84.
	/// DEF_BM -= 84
	/// </summary>
	[BuffHandler(BuffId.MAC03_110_Debuff)]
	public class MAC03_110_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -84);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}
	}

	/// <summary>
	/// Handle for SWD03_110_Buff. Stacks up; at 5 stacks triggers the active buff.
	/// </summary>
	[BuffHandler(BuffId.SWD03_110_Buff)]
	public class SWD03_110_Buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.OverbuffCounter > 4)
				buff.Target.StartBuff(BuffId.SWD03_110_active_Buff, 1, 0, TimeSpan.FromSeconds(30), buff.Target);
		}
	}

	/// <summary>
	/// Handle for SWD03_110_active_Buff. Marker buff; removes the stacking buff on enter.
	/// </summary>
	[BuffHandler(BuffId.SWD03_110_active_Buff)]
	public class SWD03_110_active_Buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.StopBuff(BuffId.SWD03_110_Buff);
		}
	}

	/// <summary>
	/// Handle for TSP03_106_Debuff. Drains 1000 stamina per stack.
	/// At 10+ stacks, triggers movement speed debuff.
	/// </summary>
	[BuffHandler(BuffId.TSP03_106_Debuff)]
	public class TSP03_106_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.OverbuffCounter > 9)
			{
				buff.Target.StartBuff(BuffId.TSP03_106_Active_Debuff, 1, 0, TimeSpan.FromSeconds(10), buff.Caster);
				buff.Target.StopBuff(BuffId.TSP03_106_Debuff);
				return;
			}

			if (buff.Target is Character character)
				character.ModifyStamina(-1000);
		}
	}

	/// <summary>
	/// Handle for TSP03_106_Active_Debuff. Reduces movement speed by 20.
	/// </summary>
	[BuffHandler(BuffId.TSP03_106_Active_Debuff)]
	public class TSP03_106_Active_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -20);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}

	/// <summary>
	/// Handle for TSW03_111_Buff. Grants a damage-absorbing shield (2380 value).
	/// </summary>
	[BuffHandler(BuffId.TSW03_111_Buff)]
	public class TSW03_111_Buff : BuffHandler
	{
		private const string ShieldValueKey = "Melia.Item.TSW03_111.Shield";
		private const float ShieldAmount = 2380f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Vars.SetFloat(ShieldValueKey, ShieldAmount);
			Send.ZC_UPDATE_SHIELD(buff.Target, (long)ShieldAmount, 1);
		}

		public override void OnExtend(Buff buff)
		{
			buff.Vars.SetFloat(ShieldValueKey, ShieldAmount);
			Send.ZC_UPDATE_SHIELD(buff.Target, (long)ShieldAmount, 1);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Vars.Remove(ShieldValueKey);
			Send.ZC_UPDATE_SHIELD(buff.Target, 0, 1);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.TSW03_111_Buff)]
		public static float OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.TSW03_111_Buff, out var buff))
				return 0;

			var remaining = buff.Vars.GetFloat(ShieldValueKey);
			if (remaining <= 0)
			{
				target.RemoveBuff(BuffId.TSW03_111_Buff);
				return 0;
			}

			var absorbed = Math.Min(remaining, skillHitResult.Damage);
			skillHitResult.Damage -= absorbed;
			remaining -= absorbed;

			buff.Vars.SetFloat(ShieldValueKey, remaining);
			Send.ZC_UPDATE_SHIELD(target, (long)remaining, 0);

			if (remaining <= 0)
				target.RemoveBuff(BuffId.TSW03_111_Buff);

			return 0;
		}
	}

	/// <summary>
	/// Handle for SHD03_104_Buff. Heals 40 HP on enter and each tick.
	/// </summary>
	[BuffHandler(BuffId.SHD03_104_Buff)]
	public class SHD03_104_Buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Heal(40, 0);
		}

		public override void WhileActive(Buff buff)
		{
			buff.Target.Heal(40, 0);
		}
	}

	/// <summary>
	/// Handle for Common_Rotten. Reduces max HP by 1% per tick (monsters only).
	/// </summary>
	[BuffHandler(BuffId.Common_Rotten)]
	public class Common_Rotten : BuffHandler
	{
		private const string VarMaxHp = "Melia.Item.Common_Rotten.MaxHP";
		private const string VarRemoved = "Melia.Item.Common_Rotten.Removed";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Vars.SetFloat(VarMaxHp, buff.Target.Properties.GetFloat(PropertyName.MHP));
			buff.Vars.SetFloat(VarRemoved, 0);
		}

		public override void WhileActive(Buff buff)
		{
			// Does not affect PCs
			if (buff.Target is Character)
				return;

			var maxHp = buff.Vars.GetFloat(VarMaxHp);
			var reduction = Math.Max(1f, maxHp * 0.01f);

			if (buff.Target.Properties.GetFloat(PropertyName.MHP) > 10)
			{
				var totalRemoved = buff.Vars.GetFloat(VarRemoved) + reduction;
				buff.Vars.SetFloat(VarRemoved, totalRemoved);
				UpdatePropertyModifier(buff, buff.Target, PropertyName.MHP_BM, -totalRemoved);
			}
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MHP_BM);
		}
	}

	/// <summary>
	/// Handle for ITEM_SKIACLIPS_Dagger. "Marking" debuff — marker tag
	/// with no direct property effect. Used as a condition for other effects.
	/// </summary>
	[BuffHandler(BuffId.ITEM_SKIACLIPS_Dagger)]
	public class ITEM_SKIACLIPS_Dagger : BuffHandler
	{
	}

	/// <summary>
	/// Handle for ITEM_SKIACLIPS_THMACE. "Broken Leg" debuff —
	/// reduces movement speed.
	/// </summary>
	[BuffHandler(BuffId.ITEM_SKIACLIPS_THMACE)]
	public class ITEM_SKIACLIPS_THMACE : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -10);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}

	/// <summary>
	/// Handle for ITEM_MISRUS_SWORD_DEBUFF. "Cower" debuff —
	/// reduces physical attack, stacks up to 5.
	/// </summary>
	[BuffHandler(BuffId.ITEM_MISRUS_SWORD_DEBUFF)]
	public class ITEM_MISRUS_SWORD_DEBUFF : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var mod = 1000f * buff.OverbuffCounter;
			UpdatePropertyModifier(buff, buff.Target, PropertyName.PATK_BM, -mod);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
		}
	}
}
