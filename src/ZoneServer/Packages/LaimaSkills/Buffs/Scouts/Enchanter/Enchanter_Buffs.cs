using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for Enchant Glove, which raises accuracy by the skill's ratio
	/// in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Enchantglove_Buff)]
	public class Enchanter_Enchantglove_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var efficiency = EnchanterSkillHelper.GetEfficiency(buff.Caster as ICombatEntity, buff.Target);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM, GetCaptionRatio(buff, 1) / 100f * efficiency);
		}

		public override void WhileActive(Buff buff)
		{
			EnchanterSkillHelper.CheckCasterPresence(buff);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM);
		}
	}

	/// <summary>
	/// Handler for Enchant Earth, which raises block penetration by the
	/// skill's ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.EnchantEarth_Buff)]
	public class Enchanter_EnchantEarth_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var efficiency = EnchanterSkillHelper.GetEfficiency(buff.Caster as ICombatEntity, buff.Target);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.BLK_BREAK_RATE_BM, GetCaptionRatio(buff, 1) / 100f * efficiency);
		}

		public override void WhileActive(Buff buff)
		{
			EnchanterSkillHelper.CheckCasterPresence(buff);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BREAK_RATE_BM);
		}
	}

	/// <summary>
	/// Handler for Agility, which lowers the stamina spent running by the
	/// skill's ratio in percent and raises movement speed by its second
	/// ratio.
	/// </summary>
	/// <remarks>
	/// NumArg2: Stamina consumption reduction in percent
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Agility_Buff)]
	public class Enchanter_Agility_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.NumArg2 = GetCaptionRatio(buff, 1);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, (float)Math.Floor(GetCaptionRatio(buff, 2)));

			if (buff.Target is Character character)
				character.Properties.Invalidate(PropertyName.Sta_Run);
		}

		public override void WhileActive(Buff buff)
		{
			EnchanterSkillHelper.CheckCasterPresence(buff);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);

			if (buff.Target is Character character)
				character.Properties.Invalidate(PropertyName.Sta_Run);
		}
	}

	/// <summary>
	/// Handler for Over-Reinforce, which raises physical and magic attack by
	/// a share of the Enchanter's attack, and with [Arts] Over-Reinforce: ALL
	/// raises physical and magic defense by 20%.
	/// </summary>
	/// <remarks>
	/// NumArg2: Attack bonus
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.OverReinforce_Buff)]
	public class Enchanter_OverReinforce_BuffOverride : BuffHandler
	{
		private const float DefenseRate = 0.20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			UpdatePropertyModifier(buff, target, PropertyName.PATK_BM, buff.NumArg2);
			UpdatePropertyModifier(buff, target, PropertyName.MATK_BM, buff.NumArg2);

			if (buff.Caster is ICombatEntity caster && caster.IsAbilityActive(AbilityId.Enchanter15))
			{
				var defenseRate = DefenseRate * EnchanterSkillHelper.GetEfficiency(caster, target);

				UpdatePropertyModifier(buff, target, PropertyName.DEF_RATE_BM, defenseRate);
				UpdatePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM, defenseRate);
			}
		}

		public override void WhileActive(Buff buff)
		{
			EnchanterSkillHelper.CheckCasterPresence(buff);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_BM);
			RemovePropertyModifier(buff, target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM);
		}
	}

	/// <summary>
	/// Handler for Enchant Weapon: Lightning, which gains a stack on every
	/// attack, up to 15, loses one every second, and raises final damage by
	/// 1% per stack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.EnchantLightning_Buff)]
	public class Enchanter_EnchantLightning_BuffOverride : BuffHandler
	{
		private const float DamagePerStack = 0.01f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType == ActivationType.Start)
				buff.OverbuffCounter = 0;
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.OverbuffCounter <= 0)
				return;

			buff.DecreaseOverbuff();
			buff.NotifyUpdate();
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.EnchantLightning_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.EnchantLightning_Buff, out var buff))
				return;

			modifier.FinalDamageMultiplier += buff.OverbuffCounter * DamagePerStack;

			if (buff.IsFullyOverbuffed)
				return;

			buff.IncreaseOverbuff();
			buff.NotifyUpdate();
		}
	}
}
