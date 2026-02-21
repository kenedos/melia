using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Swordsman.Barbarian
{
	/// <summary>
	/// Handler for the Frenzy Buff. As per the rework, this buff now:
	/// - Is gained when the Barbarian takes damage.
	/// - Increases damage dealt by the Barbarian (handled in an offensive combat handler).
	/// - Increases attack speed (affects skills with speedRateAffectedByBuff: true).
	/// - Causes the Barbarian to take slightly more damage.
	/// - Loses one stack every 5 seconds of not taking damage.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Frenzy_Buff)]
	public class Frenzy_BuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		public const string NextDecayTimeKey = "FrenzyNextDecayTime";
		public const string LastStackGainTimeKey = "FrenzyLastStackGainTime";
		private static readonly TimeSpan StackDecayInterval = TimeSpan.FromSeconds(5);

		private const float ASpdBonusBase = 150;
		private const float ASpdBonusPerStack = 10;

		/// <summary>
		/// Called when the buff is first applied or when a stack is added.
		/// We use this to cap the stacks and reset the decay timer.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Ensure the stack count doesn't exceed the new maximum.
			this.CapStacks(buff);

			// Update attack speed bonus based on current stack count.
			this.UpdateBonus(buff);

			// Whenever a stack is gained (triggering OnActivate), reset the 5-second decay timer.
			this.SetNextDecayTime(buff);

			// Update last stack gain time (for the 1-second cooldown check in skill handler).
			buff.Vars.Set(LastStackGainTimeKey, DateTime.UtcNow);
		}

		/// <summary>
		/// This method is called periodically. We use it to check if a stack should decay.
		/// </summary>
		public override void WhileActive(Buff buff)
		{
			// If the current time has passed the scheduled decay time, remove a stack.
			if (DateTime.UtcNow >= GetNextDecayTime(buff))
			{
				// Decay one stack.
				buff.OverbuffCounter--;

				// If stacks have run out, stop the buff completely.
				if (buff.OverbuffCounter <= 0)
				{
					buff.Target.StopBuff(BuffId.Frenzy_Buff);
					return; // Stop further processing as the buff is ending.
				}

				// Update attack speed bonus after stack decay.
				this.UpdateBonus(buff);

				// Refresh the buff's remaining duration to prevent premature expiration.
				// Set it to slightly longer than the decay interval to ensure the next decay can occur.
				buff.IncreaseDuration(StackDecayInterval + TimeSpan.FromSeconds(5));

				// Send an update packet to the client to show the new stack count.
				buff.NotifyUpdate();

				// Schedule the next decay time.
				this.SetNextDecayTime(buff);
			}
		}

		/// <summary>
		/// Called when the buff is removed.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			// Remove attack speed property modifier
			if (buff.Target is Character)
			{
				RemovePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM);
			}

			// Clean up any state data stored on the buff.
			buff.Vars.Remove(NextDecayTimeKey);
			buff.Vars.Remove(LastStackGainTimeKey);
		}

		/// <summary>
		/// Handles the defensive portion of Frenzy.
		/// While not explicitly in the rework, a "frenzy" state making the user
		/// more vulnerable is thematically appropriate.
		/// </summary>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// The Barbarian takes 0.5% more damage per stack of Frenzy.
			// This is a small penalty for the high-risk/high-reward playstyle.
			modifier.DamageMultiplier += 0.005f * buff.OverbuffCounter;

			// DEVELOPER NOTE: The primary offensive bonus (+5% damage dealt per stack) is NOT
			// handled here. It must be implemented in an ISkillCombatAttackBeforeCalcHandler
			// that checks if the ATTACKER has the Frenzy buff.
		}

		/// <summary>
		/// Checks stacks, capping the overbuff counter at the new allowed maximum.
		/// </summary>
		private void CapStacks(Buff buff)
		{
			// Calculate max stacks: 6 + (SkillLevel - 1) * (14.0 / 9.0)
			// Lv 1: 6 stacks, Lv 10: 20 stacks
			var maxStacks = 6;
			if (buff.Target.TryGetSkill(SkillId.Barbarian_Frenzy, out var frenzySkill))
			{

				maxStacks = (int)Math.Round(6 + (frenzySkill.Level - 1) * (14.0 / 9.0));
				maxStacks = Math.Max(6, Math.Min(20, maxStacks));
			}

			if (buff.OverbuffCounter > maxStacks)
				buff.OverbuffCounter = maxStacks;
		}

		// --- Bonus Update Helper ---

		/// <summary>
		/// Updates the attack speed bonus based on current stack count.
		/// Formula: 150 + (stacks * 10)
		/// </summary>
		private void UpdateBonus(Buff buff)
		{
			// Attack speed bonus only applies to characters, not monsters.
			if (buff.Target is Character)
			{
				var aspdBonus = ASpdBonusBase + buff.OverbuffCounter * ASpdBonusPerStack;
				UpdatePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM, aspdBonus);
			}
		}

		// --- Timer Helper Methods ---

		private void SetNextDecayTime(Buff buff)
		{
			buff.Vars.Set(NextDecayTimeKey, DateTime.UtcNow + StackDecayInterval);
		}

		private DateTime GetNextDecayTime(Buff buff)
		{
			if (buff.Vars.TryGet<DateTime>(NextDecayTimeKey, out var time))
				return time;

			// If for some reason the time isn't set, set it now to prevent rapid decay.
			this.SetNextDecayTime(buff);
			return buff.Vars.Get<DateTime>(NextDecayTimeKey);
		}
	}
}
