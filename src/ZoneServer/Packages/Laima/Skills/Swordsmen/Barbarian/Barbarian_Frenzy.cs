using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Handlers.Swordsman.Barbarian;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the passive Barbarian skill Frenzy.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_Frenzy)]
	public class Barbarian_FrenzyOverride : ISkillHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, SkillId.Barbarian_Frenzy)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetSkill(SkillId.Barbarian_Frenzy, out var frenzySkill))
				return;

			var buffInstanceDuration = TimeSpan.FromSeconds(10);

			// Gain a stack on every hit
			target.StartBuff(BuffId.Frenzy_Buff, frenzySkill.Level, 1, buffInstanceDuration, target, SkillId.Barbarian_Frenzy);
		}
	}
}
