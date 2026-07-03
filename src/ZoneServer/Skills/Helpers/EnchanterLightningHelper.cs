using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	public static class EnchanterLightningHelper
	{
		private const int MaxStacks = 15;

		private class LightningState
		{
			public int Stacks;
			public DateTime LastAttackTime;
		}

		private static readonly Dictionary<long, LightningState> States = new();

		public static void Reset(Character character)
		{
			States.Remove(character.Handle);
		}

		public static int AddStack(Character character)
		{
			var state = GetOrCreateState(character);

			state.Stacks = Math.Min(MaxStacks, state.Stacks + 1);
			state.LastAttackTime = DateTime.Now;

			return state.Stacks;
		}

		public static void DecayStack(Character character)
		{
			if (!States.TryGetValue(character.Handle, out var state))
				return;

			if ((DateTime.Now - state.LastAttackTime).TotalSeconds < 1)
				return;

			state.Stacks = Math.Max(0, state.Stacks - 1);
		}

		public static float GetFinalDamageMultiplier(Character character)
		{
			if (!States.TryGetValue(character.Handle, out var state))
				return 1f;

			return 1f + (state.Stacks * 0.01f);
		}

		public static bool CanUseLightningStacks(Character character)
		{
			return
				character.TryGetBuff(BuffId.EnchantLightning_Buff, out _) &&
				character.IsAbilityActive(AbilityId.Enchanter3);
		}

		private static LightningState GetOrCreateState(Character character)
		{
			if (!States.TryGetValue(character.Handle, out var state))
			{
				state = new LightningState
				{
					Stacks = 0,
					LastAttackTime = DateTime.Now,
				};

				States[character.Handle] = state;
			}

			return state;
		}
	}
}
