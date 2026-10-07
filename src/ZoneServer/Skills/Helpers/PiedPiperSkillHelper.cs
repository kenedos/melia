using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Buffs;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Pied Piper's songs and mice.
	/// </summary>
	public static class PiedPiperSkillHelper
	{
		/// <summary>
		/// Range the Pied Piper's songs reach.
		/// </summary>
		public const float SongRange = 160f;

		private const int MaxMice = 5;
		private const int RareMouseChancePerLevel = 2;
		private const int SwordsmanExtraBlocks = 1;
		private static readonly TimeSpan MarchDuration = TimeSpan.FromMinutes(1);
		private static readonly TimeSpan AllegroDuration = TimeSpan.FromSeconds(5);
		private static readonly int[] MouseIds = [MonsterId.PiedPiperMouse, MonsterId.PiedPiperMouseWhite];

		/// <summary>
		/// Returns the caster's summoned mice.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static List<Summon> GetMice(ICombatEntity caster)
		{
			if (caster is not Character character)
				return [];

			return character.Summons.GetSummons(s => !s.IsDead && MouseIds.Contains(s.Id));
		}

		/// <summary>
		/// Summons a mouse for a caster who knows Hameln Nagetier, rarely a
		/// white one with Hameln Nagetier: Rare Species.
		/// </summary>
		/// <param name="caster"></param>
		public static void SummonMouse(ICombatEntity caster)
		{
			if (caster is not Character character || !character.TryGetSkill(SkillId.PiedPiper_HamelnNagetier, out var hamelnSkill))
				return;

			if (GetMice(character).Count >= MaxMice)
				return;

			var isRare = character.TryGetActiveAbilityLevel(AbilityId.PiedPiper13, out var rareLevel) && GameRandom.Get().Next(100) < rareLevel * RareMouseChancePerLevel;

			var mouse = new Summon(character, isRare ? MonsterId.PiedPiperMouseWhite : MonsterId.PiedPiperMouse, RelationType.Friendly);
			mouse.Position = character.Position.GetRandomInRange2D(10, GameRandom.Get());
			mouse.Direction = character.Direction;
			mouse.OwnerHandle = character.Handle;
			mouse.Faction = FactionType.Law;
			mouse.Properties.SetFloat(PropertyName.Lv, character.Level);

			character.Summons.AddSummon(mouse);
			mouse.SetState(true);
			mouse.StartBuff(isRare ? BuffId.Ability_buff_PC_PiedPiper_WHITE_Summon : BuffId.Ability_buff_PC_PiedPiper_Summon, 0, 0, TimeSpan.Zero, mouse);

			character.StartBuff(BuffId.HamelnNagetier_Buff, hamelnSkill.Level, 0, Buff.DefaultDuration, character, hamelnSkill.Id);
		}

		/// <summary>
		/// Removes the caster's mice.
		/// </summary>
		/// <param name="caster"></param>
		public static void RemoveMice(ICombatEntity caster)
		{
			foreach (var mouse in GetMice(caster))
				mouse.Kill(null);
		}

		/// <summary>
		/// Stuns the enemies around the caster with Dissonanz, wounding them
		/// every second while they're stunned.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		public static void PlayDissonanz(ICombatEntity caster, Skill skill)
		{
			var duration = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio));

			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SongRange))
			{
				enemy.StopBuff(BuffId.Cloaking_Buff);
				enemy.StartBuff(BuffId.Dissonanz_Stun_Debuff, skill.Level, 0, duration, caster, skill.Id);
				enemy.StartBuff(BuffId.Dissonanz_Debuff, skill.Level, 0, duration, caster, skill.Id);
			}
		}

		/// <summary>
		/// Lulls the enemies around the caster to sleep with Wiegenlied.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		public static void PlayWiegenlied(ICombatEntity caster, Skill skill)
		{
			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SongRange))
				enemy.StartBuff(BuffId.Lullaby_Debuff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
		}

		/// <summary>
		/// Gives the caster's party Marschierendeslied, with an extra block
		/// for every Swordsman among them.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		public static void PlayMarschierendeslied(ICombatEntity caster, Skill skill)
		{
			var allies = PartySkillHelper.GetAlliesInRange(caster, caster.Position, SongRange);
			var swordsmen = allies.OfType<Character>().Count(c => c.JobClass == JobClass.Swordsman);
			var blocks = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio) + swordsmen * SwordsmanExtraBlocks;

			foreach (var ally in allies)
			{
				ally.StartBuff(BuffId.Marschierendeslied_Buff, skill.Level, blocks, MarchDuration, caster, skill.Id);
				ally.StartBuff(BuffId.Allegro_Buff, skill.Level, 0, AllegroDuration, caster, skill.Id);
			}
		}

		/// <summary>
		/// Gives the caster's party Lied des Weltbaum.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		public static void PlayLiedDerWeltbaum(ICombatEntity caster, Skill skill)
		{
			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, SongRange))
			{
				ally.StartBuff(BuffId.LiedDerWeltbaum_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
				ally.StartBuff(BuffId.LiedDerWeltbaum_NoDamage_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
			}
		}
	}
}
