using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers.Wizards.RuneCaster
{
	public static class RuneCasterSkilledCastingHelper
	{
		private const int MaxStacks = 2;

		public static void Apply(Character character, Skill skill)
		{
			if (!IsRuneCasterCastingSkill(skill.Id))
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster1, out var ability) || !ability.Active)
				return;

			var duration = TimeSpan.FromSeconds(5 + ability.Level * 0.25f);
			var stacks = 1;

			if (character.TryGetBuff(BuffId.Runcaster_Casting_Buff, out var buff))
			{
				var currentStacks = (int)buff.NumArg1;
				stacks = Math.Min(MaxStacks, currentStacks + 1);
				character.StopBuff(BuffId.Runcaster_Casting_Buff);
			}

			character.StartBuff(
				BuffId.Runcaster_Casting_Buff,
				stacks,
				0f,
				duration,
				character,
				skill.Id);
		}

		public static TimeSpan? GetCastTime(Character character)
		{
			if (!character.TryGetBuff(BuffId.Runcaster_Casting_Buff, out var buff))
				return null;

			return buff.NumArg1 switch
			{
				1 => TimeSpan.FromSeconds(1),
				2 => TimeSpan.FromMilliseconds(500),
				_ => null,
			};
		}

		public static bool IsRuneCasterCastingSkill(SkillId skillId)
		{
			return
				skillId == SkillId.RuneCaster_Hagalaz ||
				skillId == SkillId.RuneCaster_Isa ||
				skillId == SkillId.RuneCaster_Tiwaz ||
				skillId == SkillId.RuneCaster_Stan ||
				skillId == SkillId.RuneCaster_Ehwaz;
		}
	}
}
