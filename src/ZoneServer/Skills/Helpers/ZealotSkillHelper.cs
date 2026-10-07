using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Zealot's Fanaticism: Martyr.
	/// </summary>
	public static class ZealotSkillHelper
	{
		private static readonly TimeSpan MartyrTimePerLevel = TimeSpan.FromSeconds(2);

		/// <summary>
		/// Returns true if the character survives a lethal hit, either
		/// because they are already a martyr or because Fanaticism: Martyr
		/// turns them into one.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static bool TrySurviveAsMartyr(Character character)
		{
			if (character.IsBuffActive(BuffId.Fanaticism_Martyrdom_Buff))
				return true;

			if (!character.IsBuffActive(BuffId.Fanaticism_Buff) || !character.TryGetActiveAbilityLevel(AbilityId.Zealot10, out var level))
				return false;

			var duration = MartyrTimePerLevel * level;
			if (character.Map.IsPVP)
				duration /= 2;

			character.StopBuff(BuffId.Fanaticism_Buff);
			character.Components.Get<CooldownComponent>()?.RemoveAll();
			character.StartBuff(BuffId.Fanaticism_Martyrdom_Buff, level, 0, duration, character, SkillId.Zealot_Fanaticism);

			return true;
		}
	}
}
