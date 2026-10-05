using System;
using System.Runtime.CompilerServices;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	public static class SageRuptureHelper
	{
		public const int MaximumStacks = 20;
		private sealed class Counter { public int Value; }
		private static readonly ConditionalWeakTable<Character, Counter> Counters = new();

		public static int GetStacks(Character character)
		{
			if (character == null) return 0;
			var counter = Counters.GetValue(character, _ => new Counter());
			lock (counter) return counter.Value;
		}

		internal static void AddCompletedCast(Character character)
		{
			if (character == null) return;
			var counter = Counters.GetValue(character, _ => new Counter());
			int stacks;
			lock (counter)
			{
				counter.Value = Math.Min(MaximumStacks, counter.Value + 1);
				stacks = counter.Value;
			}
			SyncVisualBuff(character, stacks);
		}

		public static bool TrySpendAndConsume(Character character, Skill skill, out int consumed)
		{
			consumed = 0;
			if (character == null || skill == null || character.IsDead) return false;

			var counter = Counters.GetValue(character, _ => new Counter());
			int remaining;

			lock (counter)
			{
				if (counter.Value <= 0 || !character.TrySpendSp(skill)) return false;

				consumed = Math.Min(counter.Value, 10);
				counter.Value -= consumed;
				remaining = counter.Value;
			}

			SyncVisualBuff(character, remaining);
			return true;
		}

		public static int Consume(Character character, int maximum = 10)
		{
			if (character == null || maximum <= 0) return 0;

			var counter = Counters.GetValue(character, _ => new Counter());
			int consumed;
			int remaining;

			lock (counter)
			{
				consumed = Math.Min(counter.Value, maximum);
				counter.Value -= consumed;
				remaining = counter.Value;
			}

			SyncVisualBuff(character, remaining);
			return consumed;
		}

		private static void SyncVisualBuff(Character character, int stacks)
		{
			if (character == null)
				return;

			stacks = Math.Clamp(stacks, 0, MaximumStacks);

			if (stacks <= 0)
			{
				character.RemoveBuff(BuffId.Sage_Rupture_Stack_Buff);
				return;
			}

			var currentStacks = character.Buffs.GetOverbuffCount(BuffId.Sage_Rupture_Stack_Buff);

			if (currentStacks > stacks)
			{
				character.RemoveBuff(BuffId.Sage_Rupture_Stack_Buff);
				currentStacks = 0;
			}

			while (currentStacks < stacks)
			{
				character.StartBuff(
					BuffId.Sage_Rupture_Stack_Buff,
					stacks,
					0,
					TimeSpan.Zero,
					character,
					SkillId.Sage_MicroDimension
				);

				currentStacks++;
			}
		}
	}
}
