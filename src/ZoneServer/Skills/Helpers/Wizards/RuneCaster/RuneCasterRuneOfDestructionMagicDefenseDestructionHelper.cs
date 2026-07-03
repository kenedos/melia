using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	public static class RuneCasterRuneOfDestructionMagicDefenseDestructionHelper
	{
		private const float MDefReduction = -0.30f;
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(10);

		public static void Apply(ICombatEntity caster, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster8, out var ability) || !ability.Active)
				return;

			target.StartBuff(
				BuffId.RuneOfDestruction_MDef_Debuff,
				1,
				MDefReduction,
				Duration,
				character,
				SkillId.RuneCaster_Hagalaz);
		}
	}
}
