using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers.ItemSets
{
	/// <summary>
	/// Handler for the Legwyn Family Set 4-piece pre-buff.
	/// 1% chance when hit by Dark damage to activate item_set_016_buff.
	/// </summary>
	[BuffHandler(BuffId.item_set_016pre_buff)]
	public class item_set_016pre_buff : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		/// <summary>
		/// Chance to proc the buff (1% = 1 out of 100).
		/// </summary>
		private const int ProcChance = 1;

		/// <summary>
		/// Duration of the triggered buff in milliseconds.
		/// </summary>
		private const int BuffDuration = 5000;

		/// <summary>
		/// Called after defense damage is calculated (when taking damage).
		/// Has 1% chance to trigger item_set_016_buff when hit by Dark damage.
		/// </summary>
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Only characters can benefit
			if (target is not Character)
				return;

			// Only trigger on Dark attribute attacks
			var attribute = modifier.AttackAttribute != AttributeType.None
				? modifier.AttackAttribute
				: skill.Data.Attribute;

			if (attribute != AttributeType.Dark)
				return;

			// 1% chance to proc
			var roll = RandomProvider.Get().Next(1, 101);
			if (roll > ProcChance)
				return;

			// Apply the buff
			target.StartBuff(BuffId.item_set_016_buff, 1, 0, TimeSpan.FromMilliseconds(BuffDuration), target);
		}
	}
}
