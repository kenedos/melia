using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Rune Caster's runes.
	/// </summary>
	public static class RuneCasterSkillHelper
	{
		private const int MaxCastingStacks = 2;
		private static readonly TimeSpan CastingBaseDuration = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan CastingDurationPerLevel = TimeSpan.FromMilliseconds(250);

		/// <summary>
		/// Rune Caster: Skilled Casting, which adds a stack of Rune Caster:
		/// Casting after a cast rune, up to 2. The client shortens the runes'
		/// cast time from the stack count.
		/// </summary>
		/// <param name="caster"></param>
		public static void ApplySkilledCasting(ICombatEntity caster)
		{
			if (!caster.TryGetActiveAbilityLevel(AbilityId.RuneCaster1, out var level))
				return;

			var duration = CastingBaseDuration + CastingDurationPerLevel * level;
			var buff = caster.StartBuff(BuffId.Runcaster_Casting_Buff, level, 0, duration, caster);

			if (buff == null || buff.OverbuffCounter <= MaxCastingStacks)
				return;

			buff.OverbuffCounter = MaxCastingStacks;
			buff.NotifyUpdate();
		}
	}
}
