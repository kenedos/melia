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
	public class Bokor_ZombifyOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const string DefaultZombieClass = "summons_zombie";
		private const string WheelchairZombieClass = "Zombie_Overwatcher";
		private const string GiantZombieClass = "Zombie_hoplite";

		private const int LifetimeSeconds = 300;

		// Max summon counts per zombie type
		private const int MaxDefaultZombies = 6;
		private const int MaxWheelchairZombies = 4;
		private const int MaxGiantZombies = 2;

		/// <summary>
		/// Start casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
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

			// Determine zombie type based on abilities
			string zombieClass;
			string zombieName;
			int maxCount;

			if (caster.TryGetActiveAbility(AbilityId.Bokor22, out var wheelchairAbility))
			{
				// Wheelchair Zombie
				zombieClass = WheelchairZombieClass;
				zombieName = "Wheelchair Zombie";
				maxCount = MaxWheelchairZombies;
			}
			else if (caster.TryGetActiveAbility(AbilityId.Bokor21, out var giantAbility))
			{
				// Giant Zombie
				zombieClass = GiantZombieClass;
				zombieName = "Giant Zombie";
				maxCount = MaxGiantZombies;
			}
			else
			{
				// Default Zombie
				zombieClass = DefaultZombieClass;
				zombieName = "Zombie";
				maxCount = MaxDefaultZombies;
			}

			// Get character and validate
			if (!(caster is Character character))
				return;

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(zombieClass, out var monsterData))
			{
				caster.ServerMessage(Localization.Get("Failed to find zombie monster data."));
				return;
			}

			// Kill all zombies of other types when switching zombie type
			this.KillOtherZombieTypes(character, zombieClass, caster);

			// Kill all existing zombies of the current type to refresh them
			var existingZombies = character.Summons.GetSummons(monsterData.Id);
			foreach (var existingZombie in existingZombies)
			{
				existingZombie.Kill(caster);
			}

			// Summon full maxCount of zombies
			var zombiesToSummon = maxCount;

			// Summon all zombies at once
			var summonedCount = 0;
			var random = new Random();
			for (var i = 0; i < zombiesToSummon; i++)
			{
				// Calculate spawn position with random angle around caster (35 unit radius, matching IdleRadius in AiScript)
				var randomAngle = random.Next(0, 360);
				var spawnRadius = 35f;
				var spawnPosition = caster.Position.GetRelative(new Direction(randomAngle), spawnRadius);

				// Create zombie summon using MonsterSkillCreateMob with PC_Summon AI
				var zombie = MonsterSkillCreateMob(skill, caster, zombieClass, spawnPosition, 0, zombieName, "None", 0, LifetimeSeconds, "PC_Summon", "Faction#Summon#!SCR_USE_ZOMBIFY#1");

				if (zombie == null)
					continue;

				// Apply zombie stats based on type
				this.ApplyZombieStats(zombie, zombieClass, skill, caster);

				summonedCount++;
			}

			if (summonedCount > 0)
			{
				// Grant initial Dark Force stacks to caster for summoning (1 per zombie)
				// PowerOfDarkness_BuffOverride.AddDarkForceStacks(caster, summonedCount, caster);

				caster.ServerMessage(Localization.Get($"Summoned {summonedCount} {zombieName}(s)."));
			}
			else
			{
				caster.ServerMessage(Localization.Get("Failed to summon zombies."));
			}
		}

		/// <summary>
		/// Kills all zombies of other types when switching zombie type.
		/// </summary>
		/// <param name="character">The character who owns the zombies</param>
		/// <param name="currentZombieClass">The current zombie type being summoned</param>
		/// <param name="caster">The caster entity</param>
		private void KillOtherZombieTypes(Character character, string currentZombieClass, ICombatEntity caster)
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
		/// Applies stat overrides to the zombie based on its type and caster's stats.
		/// </summary>
		/// <param name="zombie">The zombie to apply stats to</param>
		/// <param name="zombieClass">The zombie type class name</param>
		/// <param name="skill">The Zombify skill</param>
		/// <param name="caster">The caster entity</param>
		private void ApplyZombieStats(Mob zombie, string zombieClass, Skill skill, ICombatEntity caster)
		{
			// Get caster's stats
			var casterMATK = (caster.Properties.GetFloat(PropertyName.MINMATK) + caster.Properties.GetFloat(PropertyName.MAXMATK)) / 2f;
			var casterMNA = caster.Properties.GetFloat(PropertyName.MNA);
			var casterLevel = caster.Level;

			// Calculate zombie stats based on type
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

				// Giant zombie can block
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

			// Apply property overrides
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.MHP, zombieHP);
			propertyOverrides.Add(PropertyName.MINPATK, zombiePATK);
			propertyOverrides.Add(PropertyName.MAXPATK, zombiePATK);
			propertyOverrides.Add(PropertyName.DEF, zombieDEF);
			propertyOverrides.Add(PropertyName.MDEF, zombieDEF);
			propertyOverrides.Add(PropertyName.BLK_BREAK, zombieBLK_BREAK);
			propertyOverrides.Add(PropertyName.HR, zombieHR);
			propertyOverrides.Add(PropertyName.SDR, 1); // AoE Defense Rate
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
}
