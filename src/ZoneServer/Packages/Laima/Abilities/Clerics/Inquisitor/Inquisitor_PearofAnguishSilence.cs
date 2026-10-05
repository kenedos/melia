using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Inquisitor7)]
	public class Inquisitor_PearofAnguishSilenceAbility : AbilityPropertyHandler
	{
		private static readonly TimeSpan SilenceDuration = TimeSpan.FromSeconds(5);

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool TryApplySilence(Character caster, ICombatEntity target, Skill skill)
		{
			if (caster == null || target == null || skill == null)
				return false;

			if (!caster.IsAbilityActive(AbilityId.Inquisitor7))
				return false;

			target.StartBuff(BuffId.Silence_Debuff, 1, 0, SilenceDuration, caster, skill.Id);
			return true;
		}
	}
}
