using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper
{
	[Package("laima")]
	public static class PiedPiperHamelnNagetierHelper
	{
		private const int MaximumMice = 5;
		private const int MaximumRareSpeciesLevel = 10;
		private const int RegularMouseMonsterId = 300004;
		private const int RareMouseMonsterId = 300005;
		private static readonly TimeSpan BaseDuration = TimeSpan.FromSeconds(20);

		public static void TrySummonMouse(Character character)
		{
			if (character == null || character.IsDead || character.Map == null)
				return;

			if (!character.Skills.TryGet(SkillId.PiedPiper_HamelnNagetier, out var skill))
				return;

			var mice = GetMice(character);
			if (mice.Count >= MaximumMice)
				return;

			var monsterId = RollMonsterId(character);
			var summonBuffId = monsterId == RareMouseMonsterId ? BuffId.Ability_buff_PC_PiedPiper_WHITE_Summon : BuffId.Ability_buff_PC_PiedPiper_Summon;
			var summon = new Summon(character, monsterId, RelationType.Friendly);

			character.Summons.AddSummon(summon);

			summon.Position = character.Position.GetRandomInRange2D(10, RandomProvider.Get());
			summon.Direction = character.Direction;
			summon.Map = character.Map;
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;
			summon.Properties.SetFloat(PropertyName.Level, character.Level);
			summon.Properties.SetFloat(PropertyName.Lv, character.Level);
			summon.Properties.SetFloat(PropertyName.FIXMSPD_BM, character.Properties.GetFloat(PropertyName.MSPD));
			summon.SetState(true);
			summon.StartBuff(summonBuffId, TimeSpan.Zero, character);

			var duration = GetDuration(character, skill.Level);
			character.StartBuff(BuffId.HamelnNagetier_Buff, skill.Level, 0, duration, character, skill.Id);
		}

		public static List<Summon> GetMice(Character character)
		{
			return character.Summons.GetSummons(summon => summon.Id == RegularMouseMonsterId || summon.Id == RareMouseMonsterId).ToList();
		}

		public static void RemoveMice(Character character)
		{
			if (character == null)
				return;

			var mice = GetMice(character).ToList();

			foreach (var mouse in mice)
			{
				if (!mouse.IsDead)
					mouse.Kill(null);

				character.Summons.RemoveSummon(mouse);
			}
		}

		public static float GetEnhanceMultiplier(Character character)
		{
			if (!character.TryGetAbility(AbilityId.PiedPiper11, out var ability))
				return 1f;

			var level = Math.Clamp(ability.Level, 0, 100);
			var multiplier = 1f + level * 0.005f;

			if (level >= 100)
				multiplier += 0.10f;

			return multiplier;
		}

		private static TimeSpan GetDuration(Character character, int skillLevel)
		{
			if (!character.IsAbilityActive(AbilityId.PiedPiper12))
				return BaseDuration;

			return BaseDuration + TimeSpan.FromSeconds(Math.Max(1, skillLevel));
		}

		private static int RollMonsterId(Character character)
		{
			if (!character.TryGetAbility(AbilityId.PiedPiper13, out var ability))
				return RegularMouseMonsterId;

			var level = Math.Clamp(ability.Level, 0, MaximumRareSpeciesLevel);
			var rareChance = level * 2;

			return RandomProvider.Get().Next(100) < rareChance ? RareMouseMonsterId : RegularMouseMonsterId;
		}
	}
}
