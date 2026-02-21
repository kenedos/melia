using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers.ItemSets
{
	/// <summary>
	/// Handler for the Watcher/Sentinel Set 4-piece pre-buff.
	/// 1% chance on normal attack to activate item_set_011_buff.
	/// </summary>
	[BuffHandler(BuffId.item_set_011pre_buff)]
	public class item_set_011pre_buff : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		/// <summary>
		/// Chance to proc the buff (1% = 1 out of 100).
		/// </summary>
		private const int ProcChance = 1;

		/// <summary>
		/// Duration of the triggered buff in milliseconds.
		/// </summary>
		private const int BuffDuration = 30000;

		/// <summary>
		/// Called after attack damage is calculated.
		/// Has 1% chance to trigger item_set_011_buff on normal attacks.
		/// </summary>
		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Only apply on normal attacks
			if (!skill.IsNormalAttack)
				return;

			// Don't apply if no damage was dealt
			if (skillHitResult.Damage <= 0)
				return;

			// Don't apply to item-type targets (destructible objects, etc.)
			if (target is Mob mob && mob.Data.Race == RaceType.Item)
				return;

			// 1% chance to proc
			var roll = RandomProvider.Get().Next(1, 101);
			if (roll > ProcChance)
				return;

			// Apply the buff
			attacker.StartBuff(BuffId.item_set_011_buff, 0, 0, TimeSpan.FromMilliseconds(BuffDuration), attacker);
		}
	}
}
