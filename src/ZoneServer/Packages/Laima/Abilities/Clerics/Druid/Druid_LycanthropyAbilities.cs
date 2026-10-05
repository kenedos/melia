using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Druid
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Druid14)]
	public class Druid_LycanthropyHumanFormAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character.IsAbilityActive(AbilityId.Druid14);
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid18)]
	public class Druid_WolfSpiritAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character.IsAbilityActive(AbilityId.Druid18);
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid20)]
	public class Druid_TransformationSpecialityAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character.IsAbilityActive(AbilityId.Druid20);
		}
	}
}
