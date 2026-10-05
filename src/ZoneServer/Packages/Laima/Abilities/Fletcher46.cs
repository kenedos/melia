using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Archers.Fletcher;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Abilities.Handlers
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Fletcher46)]
	public class Fletcher46Override : IAbilityPropertyHandler
	{
		public void OnActivate(Ability ability, Character character)
		{
		}

		public void OnDeactivate(Ability ability, Character character)
		{
			if (!character.TryGetSkill(SkillId.Fletcher_Singijeon, out var skill) || skill.IsOnCooldown)
				return;

			if (!Fletcher_FletcherArrowShotOverride.HasQuiverSpace(character, Fletcher_FletcherArrowShotOverride.SingijeonCost))
				return;

			var cooldown = character.StartCooldown(skill.Data.CooldownGroup, skill.Properties.CoolDown);
			if (cooldown != null)
				cooldown.OnCooldownChanged = () => skill.OnCooldownChanged?.Invoke();

			character.StartBuff(BuffId.Fletcher_Singijeon_Buff, TimeSpan.Zero);
		}
	}
}
