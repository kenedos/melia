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
	[AbilityHandler(AbilityId.Fletcher45)]
	public class Fletcher45Override : IAbilityPropertyHandler
	{
		public void OnActivate(Ability ability, Character character)
		{
		}

		public void OnDeactivate(Ability ability, Character character)
		{
			if (!character.TryGetSkill(SkillId.Fletcher_CrossFire, out var skill) || skill.IsOnCooldown)
				return;

			if (!Fletcher_FletcherArrowShotOverride.HasQuiverSpace(character, Fletcher_FletcherArrowShotOverride.CrossFireCost))
				return;

			var cooldown = character.StartCooldown(skill.Data.CooldownGroup, skill.Properties.CoolDown);
			if (cooldown != null)
				cooldown.OnCooldownChanged = () => skill.OnCooldownChanged?.Invoke();

			character.StartBuff(BuffId.Fletcher_CrossFire_Buff, TimeSpan.Zero);
		}
	}
}
