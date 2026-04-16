using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Zombify.
	/// Summons zombies that attack enemies and grant Dark Force to the caster.
	/// Zombies prioritize cursed targets and have stats based on caster's MATK/MNA.
	/// - Default Zombie: Max 6, balanced stats
	/// - Wheelchair Zombie (Bokor22 ability): Max 4, higher attack, lower HP
	/// - Giant Zombie (Bokor21 ability): Max 2, highest attack and HP
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Zombify)]
	public class Bokor_ZombifyOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int LifetimeSeconds = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			// Wait for client effect timing (matches Time="250" in bytool)
			await skill.Wait(TimeSpan.FromMilliseconds(250));

			if (caster is not Character character)
				return;

			var zombieInfo = ZombifyHelper.GetZombieInfo(caster);

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(zombieInfo.ClassName, out var monsterData))
			{
				caster.ServerMessage(Localization.Get("Failed to find zombie monster data."));
				return;
			}

			// Kill all zombies of other types when switching zombie type
			ZombifyHelper.KillOtherZombieTypes(character, zombieInfo.ClassName, caster);

			// Kill all existing zombies of the current type to refresh them
			var existingZombies = character.Summons.GetSummons(monsterData.Id);
			foreach (var existingZombie in existingZombies)
			{
				existingZombie.Kill(caster);
			}

			// Summon full maxCount of zombies
			var summonedCount = ZombifyHelper.SummonZombies(skill, caster, zombieInfo, zombieInfo.MaxCount);

			if (summonedCount > 0)
			{
				caster.ServerMessage(Localization.Get($"Summoned {summonedCount} {zombieInfo.Name}(s)."));
			}
			else
			{
				caster.ServerMessage(Localization.Get("Failed to summon zombies."));
			}
		}
	}

	/// <summary>
	/// Shared helper for zombie summoning logic used by Zombify and Damballa.
	/// </summary>
	public static class ZombifyHelper
	{
		private const string DefaultZombieClass = "summons_zombie";
		private const string WheelchairZombieClass = "Zombie_Overwatcher";
		private const string GiantZombieClass = "Zombie_hoplite";

		private const int MaxDefaultZombies = 6;
		private const int MaxWheelchairZombies = 4;
		private const int MaxGiantZombies = 2;

		private const int LifetimeSeconds = 300;

		/// <summary>
		/// Determines the zombie type based on the caster's active abilities.
		/// </summary>
		public static ZombieInfo GetZombieInfo(ICombatEntity caster)
		{
			if (caster.TryGetActiveAbility(AbilityId.Bokor22, out _))
				return new ZombieInfo(WheelchairZombieClass, "Wheelchair Zombie", MaxWheelchairZombies);
			else if (caster.TryGetActiveAbility(AbilityId.Bokor21, out _))
				return new ZombieInfo(GiantZombieClass, "Giant Zombie", MaxGiantZombies);
			else
				return new ZombieInfo(DefaultZombieClass, "Zombie", MaxDefaultZombies);
		}

		/// <summary>
		/// Summons the specified number of zombies at the caster's position.
		/// Returns the number successfully summoned.
		/// </summary>
		public static int SummonZombies(Skill skill, ICombatEntity caster, ZombieInfo info, int count)
		{
			var summonedCount = 0;
			var random = new Random();

			for (var i = 0; i < count; i++)
			{
				var randomAngle = random.Next(0, 360);
				var spawnRadius = 35f;
				var spawnPosition = caster.Position.GetRelative(new Direction(randomAngle), spawnRadius);

				var zombie = MonsterSkillCreateMob(skill, caster, info.ClassName, spawnPosition, 0, info.Name, "None", 0, LifetimeSeconds, "PC_Summon", "Faction#Summon#!SCR_USE_ZOMBIFY#1");

				if (zombie == null)
					continue;

				ApplyZombieStats(zombie, info.ClassName, skill, caster);
				summonedCount++;
			}

			return summonedCount;
		}

		/// <summary>
		/// Summons a single zombie at the specified position.
		/// Returns true if successful.
		/// </summary>
		public static bool SummonZombieAt(Skill skill, ICombatEntity caster, ZombieInfo info, Position position)
		{
			var zombie = MonsterSkillCreateMob(skill, caster, info.ClassName, position, 0, info.Name, "None", 0, LifetimeSeconds, "PC_Summon", "Faction#Summon#!SCR_USE_ZOMBIFY#1");

			if (zombie == null)
				return false;

			ApplyZombieStats(zombie, info.ClassName, skill, caster);
			return true;
		}

		/// <summary>
		/// Kills all zombies of types other than the specified class.
		/// </summary>
		public static void KillOtherZombieTypes(Character character, string currentZombieClass, ICombatEntity caster)
		{
			var otherZombieTypes = new List<string>();
			if (currentZombieClass != DefaultZombieClass)
				otherZombieTypes.Add(DefaultZombieClass);
			if (currentZombieClass != WheelchairZombieClass)
				otherZombieTypes.Add(WheelchairZombieClass);
			if (currentZombieClass != GiantZombieClass)
				otherZombieTypes.Add(GiantZombieClass);

			foreach (var otherZombieClass in otherZombieTypes)
			{
				if (ZoneServer.Instance.Data.MonsterDb.TryFind(otherZombieClass, out var otherMonsterData))
				{
					var otherZombies = character.Summons.GetSummons(otherMonsterData.Id);
					foreach (var otherZombie in otherZombies)
					{
						otherZombie.Kill(caster);
					}
				}
			}
		}

		/// <summary>
		/// Applies stat overrides to a zombie based on its type and caster's stats.
		/// </summary>
		public static void ApplyZombieStats(Mob zombie, string zombieClass, Skill skill, ICombatEntity caster)
		{
			var casterMATK = (caster.Properties.GetFloat(PropertyName.MINMATK) + caster.Properties.GetFloat(PropertyName.MAXMATK)) / 2f;
			var casterMNA = caster.Properties.GetFloat(PropertyName.MNA);
			var casterLevel = caster.Level;

			float zombiePATK;
			float zombieHP;
			float zombieDEF;
			float zombieBlockable = 0;
			float zombieBLK = 0;
			float zombieBLK_BREAK;
			float zombieHR;

			if (zombieClass == GiantZombieClass)
			{
				zombiePATK = casterMATK * (1.20f + (0.12f * skill.Level));
				zombieHP = 2000f + (200f * casterMNA) + (casterLevel * 10f);
				zombieDEF = 20f + (5f * casterMNA) + casterLevel;

				zombieBLK_BREAK = 30f + (8f * casterMNA) + casterLevel / 2;
				zombieHR = 20f + (5f * casterMNA) + casterLevel / 2;

				zombieBlockable = 1;
				zombieBLK = 20f + (5f * casterMNA) + casterLevel / 2;
			}
			else if (zombieClass == WheelchairZombieClass)
			{
				zombiePATK = casterMATK * (0.70f + (0.07f * skill.Level));
				zombieHP = 1000f + (100f * casterMNA) + (casterLevel * 10f);
				zombieDEF = 20f + (2f * casterMNA + casterLevel);

				zombieBLK_BREAK = 20f + (5f * casterMNA) + casterLevel / 2;
				zombieHR = 30f + (8f * casterMNA) + casterLevel / 2;
			}
			else
			{
				zombiePATK = casterMATK * (0.90f + (0.09f * skill.Level));
				zombieHP = 1500f + (150f * casterMNA) + (casterLevel * 10f);
				zombieDEF = 20f + (3f * casterMNA) + casterLevel;

				zombieBLK_BREAK = 20f + (5f * casterMNA) + casterLevel / 2;
				zombieHR = 20f + (5f * casterMNA) + casterLevel / 2;
			}

			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.MHP, zombieHP);
			propertyOverrides.Add(PropertyName.MINPATK, zombiePATK);
			propertyOverrides.Add(PropertyName.MAXPATK, zombiePATK);
			propertyOverrides.Add(PropertyName.DEF, zombieDEF);
			propertyOverrides.Add(PropertyName.MDEF, zombieDEF);
			propertyOverrides.Add(PropertyName.BLK_BREAK, zombieBLK_BREAK);
			propertyOverrides.Add(PropertyName.HR, zombieHR);
			if (zombieBlockable != 0)
			{
				propertyOverrides.Add(PropertyName.Blockable, zombieBlockable);
				propertyOverrides.Add(PropertyName.BLK, zombieBLK);
			}

			zombie.ApplyOverrides(propertyOverrides);
			zombie.Properties.InvalidateAll();
			zombie.HealToFull();
		}
	}

	/// <summary>
	/// Contains zombie type information.
	/// </summary>
	public class ZombieInfo
	{
		public string ClassName { get; }
		public string Name { get; }
		public int MaxCount { get; }

		public ZombieInfo(string className, string name, int maxCount)
		{
			this.ClassName = className;
			this.Name = name;
			this.MaxCount = maxCount;
		}
	}
}
