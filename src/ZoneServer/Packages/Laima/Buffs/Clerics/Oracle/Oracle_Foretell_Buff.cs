using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Foretell buff, which reduces the damage taken by
	/// friendly targets inside the Foretell pad area.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Foretell_Buff)]
	public class Oracle_Foretell_BuffOverride : BuffHandler
	{
		private const float MaxReduction = 0.9f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc_Defense, BuffId.Foretell_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Foretell_Buff, out var buff))
				return;

			var reduction = MathF.Min(MaxReduction, GetCaptionRatio(buff, 1) / 100f);

			skillHitResult.Damage *= 1f - reduction;
		}
	}
}
