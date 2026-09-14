using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Transmit Prana buff.
	/// Enchants attacks with Psychokinesis element, increases damage,
	/// and grants flat Soul_Atk bonus.
	/// NumArg1: skill level
	/// NumArg2: damage multiplier increase
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.TransmitPrana_Buff)]
	public class TransmitPrana_BuffOverride : BuffHandler
	{

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var flatSoulAtk = GetCaptionRatio(buff, 2);
			AddPropertyModifier(buff, buff.Target, PropertyName.Soul_Atk_BM, flatSoulAtk);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.Soul_Atk_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.TransmitPrana_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.TransmitPrana_Buff, out var buff))
				return;

			var damageMultiplierIncrease = buff.NumArg2;

			if (skill.Data.Attribute == AttributeType.None || skill.Data.Attribute == AttributeType.Melee || skill.Data.Attribute == AttributeType.Magic)
				modifier.AttackAttribute = AttributeType.Soul;

			skillHitResult.Damage *= 1f + damageMultiplierIncrease;
		}
	}
}
