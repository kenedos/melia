using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Shinobi
{
	/// <summary>

	/// Applies the status effects of Bunshin no Jutsu: Gen to clone attacks.

	/// </summary>

	public static class ShinobiBunshinGenHelper
	{
		private const int BaseChance = 10;
		private const int IncreasedChance = 25;
		private static readonly TimeSpan StandardDuration = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(2);

		public static void TryApply(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			if (caster is not DummyCharacter clone || target == null || target.IsDead || clone.Owner == null || !clone.Owner.IsAbilityActive(AbilityId.Shinobi18))
				return;

			var roll = RandomProvider.Get().Next(100);
			var slowChance = skill.Id == SkillId.Shinobi_Kunai ? IncreasedChance : BaseChance;
			var blindChance = skill.Id == SkillId.Shinobi_Katon_no_jutsu ? IncreasedChance : BaseChance;
			var silenceChance = skill.Id == SkillId.Shinobi_Raiton_no_Jutsu ? IncreasedChance : BaseChance;
			var stunChance = skill.Id == SkillId.Shinobi_Mijin_no_jutsu ? IncreasedChance : BaseChance;

			if (roll < slowChance)
			{
				ApplyEffect(target, BuffId.Common_Slow, StandardDuration, clone, skill);
				return;
			}

			roll -= slowChance;

			if (roll < blindChance)
			{
				ApplyEffect(target, BuffId.Blind, StandardDuration, clone, skill);
				return;
			}

			roll -= blindChance;

			if (roll < silenceChance)
			{
				ApplyEffect(target, BuffId.Common_Silence, StandardDuration, clone, skill);
				return;
			}

			roll -= silenceChance;

			if (roll < stunChance)
				ApplyEffect(target, BuffId.Stun, StunDuration, clone, skill);
		}

		private static void ApplyEffect(ICombatEntity target, BuffId buffId, TimeSpan duration, DummyCharacter clone, Skill skill)
		{
			target.StartBuff(buffId, 1, 0f, duration, clone, skill.Id);
		}
	}
}
