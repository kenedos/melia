using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Helpers
{
	public static class NecromancerCorpsePartsHelper
	{
		private const int BaseCapacity = 300;
		private const int CapacityPerLevel = 100;
		private const int MaximumAbilityLevel = 6;

		public static int GetMaximumCapacity(Character character)
		{
			var abilityLevel = 0;
			if (character?.Abilities.TryGet(AbilityId.Necromancer21, out var ability) == true)
				abilityLevel = Math.Clamp(ability.Level, 0, MaximumAbilityLevel);

			return BaseCapacity + abilityLevel * CapacityPerLevel;
		}

		public static bool TryAdd(Character character, int amount)
		{
			if (character == null || amount <= 0)
				return false;

			var currentParts = Math.Max(0, (int)character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt));
			var maximumParts = GetMaximumCapacity(character);
			if (currentParts >= maximumParts)
				return false;

			var newParts = currentParts + Math.Min(amount, maximumParts - currentParts);
			character.SetEtcProperty(PropertyName.Necro_DeadPartsCnt, newParts);
			RefreshHud(character);
			return true;
		}

		public static void RefreshHud(Character character)
		{
			if (character == null)
				return;

			Send.ZC_OBJECT_PROPERTY(character, character.Etc, PropertyName.Necro_DeadPartsCnt);
			character.AddonMessage(AddonMessage.UPDATE_NECRONOMICON_UI);
		}
	}
}
