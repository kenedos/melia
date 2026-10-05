using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Inquisitor11)]
	public class Inquisitor_MalleusMaleficarumManaBurnAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 4;
		private const int SilenceChancePerLevel = 5;
		private static readonly TimeSpan SilenceDuration = TimeSpan.FromSeconds(5);

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool TryApplySilence(Character caster, ICombatEntity target, Skill skill)
		{
			if (caster == null || caster.IsDead || target == null || target.IsDead || skill == null)
				return false;

			if (!caster.TryGetActiveAbilityLevel(AbilityId.Inquisitor11, out var abilityLevel))
				return false;

			abilityLevel = Math.Clamp(abilityLevel, 1, MaximumLevel);
			var silenceChance = abilityLevel * SilenceChancePerLevel;

			if (RandomProvider.Get().Next(100) >= silenceChance)
				return false;

			target.StartBuff(BuffId.Silence_Debuff, 1, 0, SilenceDuration, caster, skill.Id);
			return true;
		}
	}
}
