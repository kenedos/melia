using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for Motion, which raises the minimum critical rate of
	/// Schwarzer Reiter skills by 1% per stack, and loses a stack every
	/// 5 seconds its owner stands still.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Specialmove_Buff)]
	public class SchwarzerReiter_Specialmove_BuffOverride : BuffHandler
	{
		private const float MinCritPerStack = 1f;
		private const int MaxCritStacks = 20;

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character && character.Movement.IsMoving)
				return;

			buff.OverbuffCounter--;

			if (buff.OverbuffCounter <= 0)
			{
				buff.Target.StopBuff(BuffId.Specialmove_Buff);
				return;
			}

			buff.NotifyUpdate();
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Specialmove_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!skill.Data.ClassName.StartsWith("Schwarzereiter_"))
				return;

			if (!attacker.TryGetBuff(BuffId.Specialmove_Buff, out var buff))
				return;

			modifier.MinCritChance += MinCritPerStack * Math.Min(MaxCritStacks, buff.OverbuffCounter);
		}
	}
}
