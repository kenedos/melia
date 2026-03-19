using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the Zhendu Buff.
	/// Changes default element to Poison, increases poison attack damage,
	/// and extends poison debuff durations.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Zhendu_Buff)]
	public class Zhendu_BuffOverride : BuffHandler
	{
		static Zhendu_BuffOverride()
		{
			Buff.BuffActivated += TryExtendPoisonDuration;
		}

		/// <summary>
		/// On attack: change default element to Poison and boost poison damage.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Zhendu_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Zhendu_Buff, out var buff))
				return;

			if (skill.Data.Attribute == AttributeType.None || skill.Data.Attribute == AttributeType.Melee || skill.Data.Attribute == AttributeType.Magic)
				modifier.AttackAttribute = AttributeType.Poison;

			if (skill.Data.Attribute == AttributeType.Poison || modifier.AttackAttribute == AttributeType.Poison)
				modifier.DamageMultiplier += 0.05f + buff.NumArg1 * 0.005f;
		}

		/// <summary>
		/// Extends a poison debuff's duration if the caster has Zhendu active.
		/// Hooked into Buff.BuffActivated event.
		/// </summary>
		private static void TryExtendPoisonDuration(Buff buff)
		{
			if (!buff.Data.Tags.Contains(BuffTag.Poison))
				return;

			if (buff.Caster is not ICombatEntity caster)
				return;

			if (!caster.TryGetBuff(BuffId.Zhendu_Buff, out var zhenduBuff))
				return;

			var skillLevel = (int)zhenduBuff.NumArg1;
			var extraSeconds = 5 + skillLevel / 2;

			buff.IncreaseDuration(buff.RemainingDuration + TimeSpan.FromSeconds(extraSeconds));
		}
	}
}
