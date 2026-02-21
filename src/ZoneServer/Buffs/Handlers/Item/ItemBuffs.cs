using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
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
			var target = buff.Target;
			target.Properties.Modify(PropertyName.PATK_BM, AttackBonus);
			target.Properties.Modify(PropertyName.MATK_BM, AttackBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.Properties.Modify(PropertyName.PATK_BM, -AttackBonus);
			target.Properties.Modify(PropertyName.MATK_BM, -AttackBonus);
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
			var target = buff.Target;
			target.Properties.Modify(PropertyName.DEF_BM, DefenseBonus);
			target.Properties.Modify(PropertyName.MDEF_BM, DefenseBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.Properties.Modify(PropertyName.DEF_BM, -DefenseBonus);
			target.Properties.Modify(PropertyName.MDEF_BM, -DefenseBonus);
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
				character.Properties.Modify(PropertyName.Stamina, StaminaRegen);
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
				buff.Target.StopBuff(buff.Id);
			}
		}

		public override void OnEnd(Buff buff)
		{
			// The debuff explodes if it was fully stacked (i.e., it had 9 stacks and was about to get a 10th).
			if (buff.OverbuffCounter < 9) return;

			if (buff.Caster is not ICombatEntity caster || buff.Target.Map == null) return;

			buff.Target.PlayEffect("F_archer_explosiontrap_hit_explosion");

			var minAtk = caster.Properties.GetFloat(PropertyName.MINPATK);
			var maxAtk = caster.Properties.GetFloat(PropertyName.MAXPATK);
			var randomDivisor = 2.0 + (RandomProvider.Get().NextDouble() * (2.5 - 2.0));
			var damage = (float)Math.Floor((minAtk + maxAtk) / randomDivisor) * 5;

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
			HealTick(buff);
		}

		public override void WhileActive(Buff buff)
		{
			HealTick(buff);
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
		private const string VarMdefBonus = "Melia.Item.WizardSlayer.MdefBonus";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_wizardSlayer");

			var mdefBonus = (float)Math.Floor(target.Properties.GetFloat(PropertyName.Lv));
			buff.Vars.SetFloat(VarMdefBonus, mdefBonus);
			target.Properties.Modify(PropertyName.MDEF_BM, mdefBonus);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Vars.TryGetFloat(VarMdefBonus, out var mdefBonus))
			{
				buff.Target.Properties.Modify(PropertyName.MDEF_BM, -mdefBonus);
			}
		}
	}

	/// <summary>
	/// Handle for item_temere, which converts half of MDEF into DEF.
	/// </summary>
	[BuffHandler(BuffId.item_temere)]
	public class item_temere : BuffHandler
	{
		private const string VarStatSwap = "Melia.Item.Temere.StatSwap";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_temere");

			var mdef = target.Properties.GetFloat(PropertyName.MDEF);
			var value = (float)Math.Floor(mdef / 2);

			buff.Vars.SetFloat(VarStatSwap, value);
			target.Properties.Modify(PropertyName.MDEF_BM, -value);
			target.Properties.Modify(PropertyName.DEF_BM, value);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Vars.TryGetFloat(VarStatSwap, out var value))
			{
				buff.Target.Properties.Modify(PropertyName.MDEF_BM, value);
				buff.Target.Properties.Modify(PropertyName.DEF_BM, -value);
			}
		}
	}

	/// <summary>
	/// Base class for poison item buffs that deal damage over time.
	/// </summary>
	public abstract class item_poison_base : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_poison");
		}

		public override void WhileActive(Buff buff)
		{
			var damage = buff.NumArg1;
			// The script uses self as the attacker for this damage tick.
			buff.Target.TakeSimpleHit(damage, buff.Target, SkillId.None, HitType.Poison);
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
		public override void OnActivate(Buff buff, ActivationType activationType) => buff.Target.Properties.Modify(PropertyName.MSPD_BM, -MspdPenalty);
		public override void OnEnd(Buff buff) => buff.Target.Properties.Modify(PropertyName.MSPD_BM, MspdPenalty);
	}

	/// <summary>
	/// Handle for item_electricShock, which deals lightning damage over time.
	/// </summary>
	[BuffHandler(BuffId.item_electricShock)]
	public class item_electricShock : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_electricShock");
		}

		public override void WhileActive(Buff buff)
		{
			var damage = buff.NumArg1;
			buff.Target.TakeSimpleHit(damage, buff.Target, SkillId.None, HitType.Lightning);
		}
	}

	/// <summary>
	/// Handle for item_magicAmulet_1, which reduces DEF and MDEF by 50%.
	/// </summary>
	[BuffHandler(BuffId.item_magicAmulet_1)]
	public class item_magicAmulet_1 : BuffHandler
	{
		private const string VarDefPenalty = "Melia.Item.MagicAmulet.DefPenalty";
		private const string VarMdefPenalty = "Melia.Item.MagicAmulet.MdefPenalty";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_magicAmulet_1");

			var defPenalty = (float)Math.Floor(target.Properties.GetFloat(PropertyName.DEF) / 2);
			var mdefPenalty = (float)Math.Floor(target.Properties.GetFloat(PropertyName.MDEF) / 2);

			buff.Vars.SetFloat(VarDefPenalty, defPenalty);
			buff.Vars.SetFloat(VarMdefPenalty, mdefPenalty);

			target.Properties.Modify(PropertyName.DEF_BM, -defPenalty);
			target.Properties.Modify(PropertyName.MDEF_BM, -mdefPenalty);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			if (buff.Vars.TryGetFloat(VarDefPenalty, out var defPenalty))
				target.Properties.Modify(PropertyName.DEF_BM, defPenalty);
			if (buff.Vars.TryGetFloat(VarMdefPenalty, out var mdefPenalty))
				target.Properties.Modify(PropertyName.MDEF_BM, mdefPenalty);
		}
	}

	/// <summary>
	/// Handle for item_magicAmulet_4, which heals HP over time.
	/// </summary>
	[BuffHandler(BuffId.item_magicAmulet_4)]
	public class item_magicAmulet_4 : BuffHandler
	{
		private void HealTick(Buff buff) => buff.Target.Heal(buff.NumArg1, 0);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_magicAmulet_4");
			HealTick(buff);
		}

		public override void WhileActive(Buff buff) => HealTick(buff);
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
				character.Heal(0, -buff.NumArg1);
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
				Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_SKILL_EFFECT", 0, null, "item_cicel2");
				target.StartBuff(BuffId.item_cicel2, Cicel2Duration, target);
				target.StopBuff(buff.Id);
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
			var target = buff.Target;
			target.Properties.Modify(PropertyName.PATK_BM, 235);
			target.Properties.Modify(PropertyName.MSPD_BM, 10);
			target.Properties.Modify(PropertyName.DEF_BM, -88);
		}

		public override void WhileActive(Buff buff)
		{
			// The original script removes the stacking buff `item_cicel` during the update.
			// This acts as a safeguard in case the removal in `item_cicel`'s OnActivate failed.
			buff.Target.StopBuff(BuffId.item_cicel);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.Properties.Modify(PropertyName.PATK_BM, -235);
			target.Properties.Modify(PropertyName.MSPD_BM, -10);
			target.Properties.Modify(PropertyName.DEF_BM, 88);
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
			buff.Target.SetTempVar(VarEffigyBonus, buff.NumArg1);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveTempVar(VarEffigyBonus);
		}
	}

	/// <summary>
	/// Handle for item_armorBreak, which decreases Defense. The penalty amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.item_armorBreak)]
	public class item_armorBreak : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var penalty = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.DEF_BM, -penalty);
		}

		public override void OnEnd(Buff buff)
		{
			var penalty = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.DEF_BM, penalty);
		}
	}
}
