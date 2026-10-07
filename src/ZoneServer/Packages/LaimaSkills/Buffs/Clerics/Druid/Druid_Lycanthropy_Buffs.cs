using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Lycanthropy wolf, which doubles max HP, raises
	/// damage, critical rate, defenses, speed and block penetration, heals
	/// 2% of max HP every 8 seconds and swaps the Druid's skills for the
	/// wolf's.
	/// </summary>
	/// <remarks>
	/// [Arts] Lycanthropy: Wolf's Spirit adds 3.5 + 0.7 per level % final
	/// damage.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Lycanthropy_Buff)]
	public class Druid_Lycanthropy_BuffOverride : BuffHandler, ITransformationBuff
	{
		private const int HealInterval = 8000;
		private const float HealRate = 0.02f;
		private const float MoveSpeedBonus = 10f;
		private const float DefenseRate = 0.5f;
		private const float BlockPenetrationRate = 0.1f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			buff.SetUpdateTime(HealInterval);

			UpdatePropertyModifier(buff, target, PropertyName.MHP_RATE_BM, 1);
			UpdatePropertyModifier(buff, target, PropertyName.MSPD_BM, MoveSpeedBonus);
			UpdatePropertyModifier(buff, target, PropertyName.DEF_RATE_BM, DefenseRate);
			UpdatePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM, DefenseRate);
			UpdatePropertyModifier(buff, target, PropertyName.CRTHR_RATE_BM, GetCaptionRatio(buff, 2) / 100f);
			UpdatePropertyModifier(buff, target, PropertyName.BLK_BREAK_BM, target.Properties.GetFloat(PropertyName.BLK_BREAK) * BlockPenetrationRate);

			if (target is Character character)
				DruidSkillHelper.AddTemporarySkills(buff, character, DruidSkillHelper.WolfSkills, (int)buff.NumArg1);
		}

		public override void WhileActive(Buff buff)
		{
			if (!buff.Target.IsDead)
				buff.Target.Heal(buff.Target.MaxHp * HealRate, 0);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MHP_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
			RemovePropertyModifier(buff, target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.CRTHR_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.BLK_BREAK_BM);

			if (target is Character character)
				DruidSkillHelper.RemoveTemporarySkills(buff, character);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Lycanthropy_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Lycanthropy_Buff, out var buff))
				return;

			modifier.DamageMultiplier += GetCaptionRatio(buff, 1) / 100f;

			if (attacker.IsAbilityActive(AbilityId.Druid27))
				modifier.FinalDamageMultiplier += (3.5f + buff.NumArg1 * 0.7f) / 100f;
		}
	}

	/// <summary>
	/// Handler for Lycanthropy: Human Form, a hybrid whose basic attack is a
	/// scratch that may cause bleeding, with +5 movement speed and 10.5 + 2.1
	/// per level % final damage.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Lycanthropy_Half_Buff)]
	public class Druid_Lycanthropy_Half_BuffOverride : BuffHandler
	{
		private const float MoveSpeedBonus = 5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MoveSpeedBonus);

			if (buff.Target is not Character character)
				return;

			if (!character.Skills.Has(SkillId.Lycan_Half_Attack))
				character.Skills.Add(new Skill(character, SkillId.Lycan_Half_Attack));

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Lycan_Half_Attack);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);

			if (buff.Target is Character character)
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Lycanthropy_Half_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker.TryGetBuff(BuffId.Lycanthropy_Half_Buff, out var buff))
				modifier.FinalDamageMultiplier += (10.5f + buff.NumArg1 * 2.1f) / 100f;
		}
	}
}
