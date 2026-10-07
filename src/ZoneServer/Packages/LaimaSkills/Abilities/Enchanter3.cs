using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Enchanter: Enchant Weapon: Lightning ability, which keeps the
	/// Lightning stack buff on its owner.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Enchanter3)]
	public class Enchanter3Override : IAbilityPropertyHandler
	{
		public void OnActivate(Ability ability, Character character)
		{
			character.StartBuff(BuffId.EnchantLightning_Buff, 1, 0, TimeSpan.Zero, character, SkillId.Enchanter_EnchantLightning);
		}

		public void OnDeactivate(Ability ability, Character character)
		{
			character.StopBuff(BuffId.EnchantLightning_Buff);
		}
	}
}
