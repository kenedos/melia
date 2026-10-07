using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for Engkrateia, which reduces the damage taken, more so from
	/// devils and Dark attacks, and blocks knockback and knockdown.
	/// </summary>
	/// <remarks>
	/// Engkrateia: The Goddess' Reply keeps the Exorcist at 1 HP, which
	/// Character.TakeDamage checks.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Engkrateia_Buff)]
	public class Exorcist_Engkrateia_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const float EvilDamageReduction = 0.10f;
		private const float MaxDamageReduction = 0.9f;

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Engkrateia_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Engkrateia_Buff, out var buff))
				return;

			var reduction = GetCaptionRatio(buff, 1) / 100f;

			var attribute = modifier.AttackAttribute != AttributeType.None ? modifier.AttackAttribute : skill.Data.Attribute;
			if (attacker.Race == RaceType.Velnias || attribute == AttributeType.Dark)
				reduction += EvilDamageReduction;

			skillHitResult.Damage *= Math.Max(1 - MaxDamageReduction, 1 - reduction);
		}
	}
}
