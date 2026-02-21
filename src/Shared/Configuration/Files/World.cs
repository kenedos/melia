using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melia.Shared.Game.Const.Web;
using Yggdrasil.Configuration;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents world.conf
	/// </summary>
	public class WorldConfFile : ConfFile
	{
		// ai.conf
		public bool MonstersUsePathfinding { get; protected set; }

		// drops.conf
		public float SilverDropAmount { get; protected set; }
		public float SilverDropRate { get; protected set; }
		public float EquipmentDropRate { get; protected set; }
		public float BlueOrbDropRate { get; protected set; }
		public float RedOrbDropRate { get; protected set; }
		public float RecipeDropRate { get; protected set; }
		public float GemDropRate { get; protected set; }
		public float CubeDropRate { get; protected set; }
		public float GeneralDropRate { get; protected set; }
		public int DropRadius { get; protected set; }
		public int PickUpRadius { get; protected set; }
		public int LootPrectionSeconds { get; protected set; }
		public int DropDisappearSeconds { get; protected set; }
		public bool Littering { get; protected set; }
		public bool TargetedLittering { get; protected set; }

		// exp.conf
		public float ExpRate { get; protected set; }
		public float JobExpRate { get; protected set; }
		public bool LevelExpPenalty { get; protected set; }

		// instance_dungeon.conf
		public float InstancedDungeonExpRate { get; protected set; }
		public int InstancedDungeonDurationMinutes { get; protected set; }
		public int InstancedDungeonMinStageKillPercentage { get; protected set; }
		public int InstancedDungeonEmptyTimeoutSeconds { get; protected set; }
		public int InstancedDungeonResetHour { get; protected set; }
		public int InstancedDungeonResetMinute { get; protected set; }
		public DayOfWeek InstancedDungeonWeeklyResetDay { get; protected set; }
		public bool InstancedDungeonAllowRejoin { get; protected set; }
		public bool InstancedDungeonIncrementEntryOnComplete { get; protected set; }
		public float InstancedDungeonRewardExpRate { get; protected set; }

		// game_time.conf
		public int TicksPerMinute { get; protected set; }
		public int MinutesPerHour { get; protected set; }
		public int HoursPerDay { get; protected set; }
		public int DaysPerMonth { get; protected set; }
		public int MonthsPerYear { get; protected set; }
		public bool EnableDayNightCycle { get; protected set; }

		// items.conf
		public float ItemCooldownRate { get; protected set; }

		// jobs.conf
		public int JobMaxRank { get; protected set; }
		public bool NoAdvancement { get; protected set; }
		public bool NoRankReset { get; protected set; }
		public int MaxLevel { get; protected set; }
		public int MaxCompanionLevel { get; protected set; }
		public int MaxBaseJobLevel { get; protected set; }
		public int MaxAdvanceJobLevel { get; protected set; }
		public float StatsPerLevel { get; protected set; }
		public List<int> ExtraStatsLevels { get; protected set; }
		public bool UseJobQuests { get; protected set; }

		// misc.conf
		public bool ResurrectCityOption { get; protected set; }
		public bool ResurrectRevivalPointOption { get; protected set; }
		public bool ResurrectSoulCrystalOption { get; protected set; }

		// quests.conf
		public bool DisplayQuestObjectives { get; protected set; }
		public bool EnableProceduralQuests { get; protected set; }

		// skills.conf
		public bool DisableSDR { get; protected set; }
		public int AbilityPointCost { get; protected set; }
		public float NormalAttackSkillFactorRate { get; protected set; }
		public float PlayerSkillFactorRate { get; protected set; }
		public float MonsterSkillFactorRate { get; protected set; }
		public float MonsterSkillSpeedRate { get; protected set; }
		public float MonsterSkillDelayRate { get; protected set; }
		public bool FreezeAffectsElement { get; protected set; }

		// storage.conf
		public int StorageFee { get; protected set; }
		public int StorageExtCost { get; protected set; }
		public int StorageExtCount { get; protected set; }
		public int StorageDefaultSize { get; protected set; }
		public int StorageMaxSize { get; protected set; }
		public int StorageMaxExtensions { get; protected set; }
		public bool StorageMultiStack { get; protected set; }
		public int TeamStorageDefaultSize { get; protected set; }
		public int TeamStorageExtCost { get; protected set; }
		public int TeamStorageMaxSilverExpands { get; protected set; }
		public int TeamStorageMaxSize { get; protected set; }
		public int TeamStorageMinimumLevelRequired { get; protected set; }

		// summons.conf
		public bool BlueOrbFollowWarp { get; protected set; }
		public bool BlueOrbPetSystem { get; protected set; }

		// rare_monsters.conf
		public float BlueJackpotSpawnChance { get; protected set; }
		public float BlueJackpotExpRate { get; protected set; }
		public float RedJackpotSpawnChance { get; protected set; }
		public int RedJackpotWaveMin { get; protected set; }
		public int RedJackpotWaveMax { get; protected set; }
		public int RedJackpotWaveMonsterCount { get; protected set; }
		public int RedJackpotWaveDelayMin { get; protected set; }
		public int RedJackpotWaveDelayMax { get; protected set; }
		public float SilverJackpotSpawnChance { get; protected set; }
		public int SilverJackpotRolls { get; protected set; }
		public float SilverJackpotGuaranteedItemThreshold { get; protected set; }
		public float GoldJackpotSpawnChance { get; protected set; }
		public int GoldJackpotRolls { get; protected set; }
		public float GoldJackpotGuaranteedItemThreshold { get; protected set; }
		public float EliteSpawnChance { get; protected set; }
		public float EliteHPSPRate { get; protected set; }
		public float EliteStatRate { get; protected set; }
		public float EliteExpRate { get; protected set; }
		public int EliteRolls { get; protected set; }
		public float EliteGuaranteedItemThreshold { get; protected set; }
		public int EliteMinLevel { get; protected set; }
		public bool EliteAlwaysAggressive { get; protected set; }
		public float RedOrbJackpotRate { get; protected set; }
		public float RedOrbEliteRate { get; protected set; }

		// boss_monsters.conf
		public float BossHPSPRate { get; protected set; }
		public float BossStatRate { get; protected set; }
		public float BossExpRate { get; protected set; }
		public int BossRolls { get; protected set; }
		public float BossGuaranteedItemThreshold { get; protected set; }
		public int BossMinLevel { get; protected set; }
		public bool BossAlwaysAggressive { get; protected set; }

		// global_drops.conf
		public bool GlobalDropSuperMobItemThreshold { get; protected set; }
		public bool GlobalDropSuperMobItemReroll { get; protected set; }
		public float GlobalDropRate { get; protected set; }
		public int GlobalDropRolls { get; protected set; }

		// minigames.conf
		public int MinigameMaxPopulation { get; protected set; }
		public int MinigameRespawnCooldownSeconds { get; protected set; }
		public int MinigameRotationTimeoutMinutes { get; protected set; }
		public int DefendTorchWaveMonsterCount { get; protected set; }
		public int DefendTorchDurationMinutes { get; protected set; }
		public int DefendTorchWaveIntervalSeconds { get; protected set; }
		public float DefendTorchActivationRange { get; protected set; }
		public float DefendTorchRewardRange { get; protected set; }
		public int DefendTorchHp { get; protected set; }

		// Marble Shooter minigame settings
		public int MarbleShooterDurationMinutes { get; protected set; }
		public float MarbleShooterActivationRange { get; protected set; }
		public float MarbleShooterRewardRange { get; protected set; }
		public int MarbleShooterRequiredHits { get; protected set; }
		public int MarbleShooterMarbleCount { get; protected set; }
		public float MarbleShooterMarbleMinRange { get; protected set; }
		public float MarbleShooterMarbleMaxRange { get; protected set; }
		public float MarbleShooterFireballMinRange { get; protected set; }
		public float MarbleShooterFireballMaxRange { get; protected set; }
		public float MarbleShooterFireballSpeedMultiplier { get; protected set; }
		public float MarbleShooterMarbleSpeedMultiplier { get; protected set; }
		public float MarbleShooterInteractionDistance { get; protected set; }
		public float MarbleShooterMarbleBaseSpeed { get; protected set; }
		public int MarbleShooterMonsterWaveIntervalSeconds { get; protected set; }

		// Whack-a-Mole minigame settings
		public int WhackAMoleDurationMinutes { get; protected set; }
		public float WhackAMoleActivationRange { get; protected set; }
		public float WhackAMoleRewardRange { get; protected set; }
		public int WhackAMoleRequiredKills { get; protected set; }
		public float WhackAMoleBurrowRange { get; protected set; }
		public int WhackAMoleBurrowShiftIntervalSeconds { get; protected set; }
		public int WhackAMoleMoleSpawnIntervalSeconds { get; protected set; }
		public int WhackAMoleMonsterWaveIntervalSeconds { get; protected set; }
		public float WhackAMoleMonsterSpawnDistance { get; protected set; }

		/// <summary>
		/// Custom Things
		/// </summary>
		public bool EnableMonsterLevelItemBonus { get; protected set; }
		public bool FastTravelEnabled { get; protected set; }
		public bool IsPBE { get; protected set; }
		public int AutoSaveSlots { get; set; }
		public int AutoSaveIntervalMinutes { get; set; }

		// Orphan cleanup settings
		public bool OrphanCleanupEnabled { get; protected set; }
		public int OrphanCleanupCycles { get; protected set; }
		public int OrphanCleanupBatchSize { get; protected set; }

		// party.conf - Quest Sharing
		public bool PartyQuestSharingEnabled { get; protected set; }
		public float PartyQuestSharingRange { get; protected set; }
		public bool PartyShareKillObjectives { get; protected set; }

		// party.conf - Exp Level Penalties
		public int PartyExpLevelPenaltyFull { get; protected set; }
		public int PartyExpLevelPenaltyHigh { get; protected set; }
		public int PartyExpLevelPenaltyMedium { get; protected set; }

		// party.conf - Default Party Settings
		public int PartyDefaultExpDistribution { get; protected set; }
		public int PartyDefaultItemDistribution { get; protected set; }
		public int PartyDefaultQuestSharing { get; protected set; }

		// connection.conf
		public int ConnectionCloseDelay { get; protected set; }

		/// <summary>
		/// Loads conf file and its options from the given path.
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath, params string[] extraIncludes)
		{
			this.Include(filePath);

			foreach (var path in extraIncludes)
			{
				if (File.Exists(path))
					this.Include(path);
			}

			this.MonstersUsePathfinding = this.GetBool("monsters_use_pathfinding", true);

			this.SilverDropAmount = this.GetFloat("silver_drop_amount", 100);
			this.SilverDropRate = this.GetFloat("silver_drop_rate", 100);
			this.EquipmentDropRate = this.GetFloat("equipment_drop_rate", 100);
			this.BlueOrbDropRate = this.GetFloat("blue_orb_drop_rate", 100);
			this.RedOrbDropRate = this.GetFloat("red_orb_drop_rate", 100);
			this.RecipeDropRate = this.GetFloat("recipe_drop_rate", 100);
			this.GemDropRate = this.GetFloat("gem_drop_rate", 100);
			this.GeneralDropRate = this.GetFloat("general_drop_rate", 100);
			this.CubeDropRate = this.GetFloat("cube_drop_rate", 100);
			this.DropRadius = this.GetInt("drop_radius", 25);
			this.PickUpRadius = this.GetInt("pick_up_radius", 100);
			this.LootPrectionSeconds = this.GetInt("loot_protection", 100);
			this.DropDisappearSeconds = this.GetInt("drop_disappear_time", 100);
			this.Littering = this.GetBool("littering", false);
			this.TargetedLittering = this.GetBool("targeted_littering", false);

			this.StorageFee = this.GetInt("storage_fee", 20);
			this.StorageExtCost = this.GetInt("storage_ext_cost", 20);
			this.StorageDefaultSize = this.GetInt("storage_default_size", 60);
			this.StorageMaxSize = this.GetInt("storage_max_size", 110);
			this.StorageMultiStack = this.GetBool("storage_multi_stack", true);
			this.TeamStorageDefaultSize = this.GetInt("team_storage_default_size", 5);
			this.TeamStorageExtCost = this.GetInt("team_storage_ext_cost", 200000);
			this.TeamStorageMaxSilverExpands = this.GetInt("team_storage_max_silver_expands", 9);
			this.TeamStorageMaxSize = this.GetInt("team_storage_max_size", 70);
			this.TeamStorageMinimumLevelRequired = this.GetInt("team_storage_min_level_req", 15);

			this.ExpRate = this.GetFloat("exp_rate", 100);
			this.JobExpRate = this.GetFloat("job_exp_rate", 100);
			this.LevelExpPenalty = this.GetBool("level_exp_penalty", false);

			this.InstancedDungeonExpRate = this.GetFloat("instanced_dungeon_exp_rate", 300);
			this.InstancedDungeonDurationMinutes = this.GetInt("instanced_dungeon_duration_minutes", 60);
			this.InstancedDungeonMinStageKillPercentage = this.GetInt("instanced_dungeon_min_stage_kill_percentage", 70);
			this.InstancedDungeonEmptyTimeoutSeconds = this.GetInt("instanced_dungeon_empty_timeout_seconds", 300);
			this.InstancedDungeonResetHour = this.GetInt("instanced_dungeon_reset_hour", 0);
			this.InstancedDungeonResetMinute = this.GetInt("instanced_dungeon_reset_minute", 0);
			this.InstancedDungeonWeeklyResetDay = (DayOfWeek)this.GetInt("instanced_dungeon_weekly_reset_day", (int)DayOfWeek.Monday);
			this.InstancedDungeonAllowRejoin = this.GetBool("instanced_dungeon_allow_rejoin", true);
			this.InstancedDungeonIncrementEntryOnComplete = this.GetBool("instanced_dungeon_increment_entry_on_complete", false);
			this.InstancedDungeonRewardExpRate = this.GetFloat("instanced_dungeon_reward_exp_rate", 100);

			this.TicksPerMinute = this.GetInt("rt2gt_ms_per_minute", 1500) * 10000;
			this.MinutesPerHour = this.GetInt("gt_minutes_per_hour", 60);
			this.HoursPerDay = this.GetInt("gt_hours_per_day", 24);
			this.DaysPerMonth = this.GetInt("gt_days_per_month", 40);
			this.MonthsPerYear = this.GetInt("gt_months_per_year", 7);
			this.EnableDayNightCycle = this.GetBool("enable_day_night_cycle", true);

			this.ItemCooldownRate = this.GetFloat("item_cooldown_rate", 1);

			this.JobMaxRank = this.GetInt("job_max_rank", 4);
			this.NoAdvancement = this.GetBool("no_advancement", false);
			this.NoRankReset = this.GetBool("no_rank_reset", true);
			this.MaxLevel = this.GetInt("max_level", 520);
			this.MaxCompanionLevel = this.GetInt("max_companion_level", 520);
			this.MaxBaseJobLevel = this.GetInt("max_base_job_level", 15);
			this.MaxAdvanceJobLevel = this.GetInt("max_advanced_job_level", 45);
			this.StatsPerLevel = this.GetFloat("stats_per_level", 1);
			this.ExtraStatsLevels = this.GetString("extra_stats_levels", "0").Replace(" ", "").Split(',').Select(int.Parse).ToList();
			this.UseJobQuests = this.GetBool("use_job_quests", false);

			this.ResurrectRevivalPointOption = this.GetBool("resurrect_revival_point_option", true);
			this.ResurrectCityOption = this.GetBool("resurrect_city_option", true);
			this.ResurrectSoulCrystalOption = this.GetBool("resurrect_soul_crystal_option", true);

			this.DisplayQuestObjectives = this.GetBool("display_quest_objectives", true);
			this.EnableProceduralQuests = this.GetBool("enable_procedural_quests", false);

			this.DisableSDR = this.GetBool("disable_sdr", false);
			this.AbilityPointCost = this.GetInt("ability_point_cost", 1000);
			this.NormalAttackSkillFactorRate = this.GetFloat("normal_attack_skill_factor_rate", 1);
			this.PlayerSkillFactorRate = this.GetFloat("player_skill_factor_rate", 1);
			this.MonsterSkillFactorRate = this.GetFloat("monster_skill_factor_rate", 1);
			this.MonsterSkillSpeedRate = this.GetFloat("monster_skill_speed_rate", 1);
			this.MonsterSkillDelayRate = this.GetFloat("monster_skill_delay_rate", 1);
			this.FreezeAffectsElement = this.GetBool("freeze_affects_element", false);

			this.BlueOrbFollowWarp = this.GetBool("blue_orb_follow_warp", false);
			this.BlueOrbPetSystem = this.GetBool("blue_orb_pet_system", false);

			this.BlueJackpotSpawnChance = this.GetFloat("blue_jackpot_spawn_chance", 0.05f);
			this.BlueJackpotExpRate = this.GetFloat("blue_jackpot_exp_rate", 10000);
			this.RedJackpotSpawnChance = this.GetFloat("red_jackpot_spawn_chance", 0.05f);
			this.RedJackpotWaveMin = this.GetInt("red_wave_min", 4);
			this.RedJackpotWaveMax = this.GetInt("red_wave_max", 10);
			this.RedJackpotWaveMonsterCount = this.GetInt("red_wave_monster_count", 15);
			this.RedJackpotWaveDelayMin = this.GetInt("red_wave_delay_min", 5);
			this.RedJackpotWaveDelayMax = this.GetInt("red_wave_delay_max", 15);
			this.SilverJackpotSpawnChance = this.GetFloat("silver_jackpot_spawn_chance", 0.05f);
			this.SilverJackpotRolls = this.GetInt("silver_jackpot_rolls", 100);
			this.SilverJackpotGuaranteedItemThreshold = this.GetFloat("silver_guaranteed_item_threshold", 0.5f);
			this.GoldJackpotSpawnChance = this.GetFloat("gold_jackpot_spawn_chance", 0.01f);
			this.GoldJackpotRolls = this.GetInt("gold_jackpot_rolls", 1000);
			this.GoldJackpotGuaranteedItemThreshold = this.GetFloat("gold_guaranteed_item_threshold", 0.5f);
			this.EliteSpawnChance = this.GetFloat("elite_spawn_chance", 2);
			this.EliteHPSPRate = this.GetFloat("elite_hpsp_rate", 150);
			this.EliteStatRate = this.GetFloat("elite_stat_rate", 150);
			this.EliteExpRate = this.GetFloat("elite_exp_rate", 2);
			this.EliteRolls = this.GetInt("elite_rolls", 2);
			this.EliteGuaranteedItemThreshold = this.GetFloat("elite_guaranteed_item_threshold", 0.5f);
			this.EliteMinLevel = this.GetInt("elite_min_level", 100);
			this.EliteAlwaysAggressive = this.GetBool("elite_always_aggressive", true);
			this.RedOrbJackpotRate = this.GetFloat("red_orb_jackpot_rate", 10000);
			this.RedOrbEliteRate = this.GetFloat("red_orb_elite_rate", 1000);

			this.BossHPSPRate = this.GetFloat("boss_hpsp_rate", 100);
			this.BossStatRate = this.GetFloat("boss_stat_rate", 100);
			this.BossExpRate = this.GetFloat("boss_exp_rate", 100);
			this.BossRolls = this.GetInt("boss_rolls", 1);
			this.BossGuaranteedItemThreshold = this.GetFloat("boss_guaranteed_item_threshold", 0.5f);
			this.BossMinLevel = this.GetInt("boss_min_level", 1);
			this.BossAlwaysAggressive = this.GetBool("boss_always_aggressive", true);

			this.FastTravelEnabled = this.GetBool("fast_travel_enabled", true);
			this.IsPBE = this.GetBool("is_pbe", true);
			this.AutoSaveSlots = this.GetInt("auto_save_slots", 10);
			this.AutoSaveIntervalMinutes = this.GetInt("auto_save_interval", 1);

			this.OrphanCleanupEnabled = this.GetBool("orphan_cleanup_enabled", true);
			this.OrphanCleanupCycles = this.GetInt("orphan_cleanup_cycles", 144);
			this.OrphanCleanupBatchSize = this.GetInt("orphan_cleanup_batch_size", 10000);

			this.GlobalDropSuperMobItemThreshold = this.GetBool("global_drop_super_mob_item_threshold", false);
			this.GlobalDropSuperMobItemReroll = this.GetBool("global_drop_super_mob_item_reroll", false);
			this.GlobalDropRate = this.GetFloat("global_drop_rate", 100);
			this.GlobalDropRolls = this.GetInt("global_drop_rolls", 0);

			this.MinigameMaxPopulation = this.GetInt("minigame_max_population", 3);
			this.MinigameRespawnCooldownSeconds = this.GetInt("minigame_respawn_cooldown_seconds", 600);
			this.MinigameRotationTimeoutMinutes = this.GetInt("minigame_rotation_timeout_minutes", 30);
			this.DefendTorchWaveMonsterCount = this.GetInt("defend_torch_wave_monster_count", 15);
			this.DefendTorchDurationMinutes = this.GetInt("defend_torch_duration_minutes", 5);
			this.DefendTorchWaveIntervalSeconds = this.GetInt("defend_torch_wave_interval_seconds", 15);
			this.DefendTorchActivationRange = this.GetFloat("defend_torch_activation_range", 200.0f);
			this.DefendTorchRewardRange = this.GetFloat("defend_torch_reward_range", 400.0f);
			this.DefendTorchHp = this.GetInt("defend_torch_hp", 100);

			// Marble Shooter minigame settings
			this.MarbleShooterDurationMinutes = this.GetInt("marble_shooter_duration_minutes", 3);
			this.MarbleShooterActivationRange = this.GetFloat("marble_shooter_activation_range", 300.0f);
			this.MarbleShooterRewardRange = this.GetFloat("marble_shooter_reward_range", 400.0f);
			this.MarbleShooterRequiredHits = this.GetInt("marble_shooter_required_hits", 5);
			this.MarbleShooterMarbleCount = this.GetInt("marble_shooter_marble_count", 10);
			this.MarbleShooterMarbleMinRange = this.GetFloat("marble_shooter_marble_min_range", 100.0f);
			this.MarbleShooterMarbleMaxRange = this.GetFloat("marble_shooter_marble_max_range", 150.0f);
			this.MarbleShooterFireballMinRange = this.GetFloat("marble_shooter_fireball_min_range", 30.0f);
			this.MarbleShooterFireballMaxRange = this.GetFloat("marble_shooter_fireball_max_range", 80.0f);
			this.MarbleShooterFireballSpeedMultiplier = this.GetFloat("marble_shooter_fireball_speed_multiplier", 2.0f);
			this.MarbleShooterMarbleSpeedMultiplier = this.GetFloat("marble_shooter_marble_speed_multiplier", 2.0f);
			this.MarbleShooterInteractionDistance = this.GetFloat("marble_shooter_interaction_distance", 30.0f);
			this.MarbleShooterMarbleBaseSpeed = this.GetFloat("marble_shooter_marble_base_speed", 50.0f);
			this.MarbleShooterMonsterWaveIntervalSeconds = this.GetInt("marble_shooter_monster_wave_interval_seconds", 20);

			// Whack-a-Mole minigame settings
			this.WhackAMoleDurationMinutes = this.GetInt("whack_a_mole_duration_minutes", 3);
			this.WhackAMoleActivationRange = this.GetFloat("whack_a_mole_activation_range", 300.0f);
			this.WhackAMoleRewardRange = this.GetFloat("whack_a_mole_reward_range", 400.0f);
			this.WhackAMoleRequiredKills = this.GetInt("whack_a_mole_required_kills", 5);
			this.WhackAMoleBurrowRange = this.GetFloat("whack_a_mole_burrow_range", 300.0f);
			this.WhackAMoleBurrowShiftIntervalSeconds = this.GetInt("whack_a_mole_burrow_shift_interval_seconds", 10);
			this.WhackAMoleMoleSpawnIntervalSeconds = this.GetInt("whack_a_mole_mole_spawn_interval_seconds", 5);
			this.WhackAMoleMonsterWaveIntervalSeconds = this.GetInt("whack_a_mole_monster_wave_interval_seconds", 15);
			this.WhackAMoleMonsterSpawnDistance = this.GetFloat("whack_a_mole_monster_spawn_distance", 70.0f);

			// party.conf - Quest Sharing
			this.PartyQuestSharingEnabled = this.GetBool("party_quest_sharing_enabled", true);
			this.PartyQuestSharingRange = this.GetFloat("party_quest_sharing_range", 0);
			this.PartyShareKillObjectives = this.GetBool("party_share_kill_objectives", true);

			// party.conf - Exp Level Penalties
			this.PartyExpLevelPenaltyFull = this.GetInt("party_exp_level_penalty_full", 20);
			this.PartyExpLevelPenaltyHigh = this.GetInt("party_exp_level_penalty_high", 15);
			this.PartyExpLevelPenaltyMedium = this.GetInt("party_exp_level_penalty_medium", 10);

			// party.conf - Default Party Settings
			this.PartyDefaultExpDistribution = this.GetInt("party_default_exp_distribution", 0);
			this.PartyDefaultItemDistribution = this.GetInt("party_default_item_distribution", 1);
			this.PartyDefaultQuestSharing = this.GetInt("party_default_quest_sharing", 1);

			// connection.conf
			this.ConnectionCloseDelay = this.GetInt("connection_close_delay", 100);

			this.ManualAdjustments();
		}


		private void ManualAdjustments()
		{
			// Round storage size to next full 10, since the client only extends
			// in multiples of 10.
			this.StorageDefaultSize = (int)Math.Ceiling(this.StorageDefaultSize / 10f) * 10;
			this.StorageMaxSize = (int)Math.Ceiling(this.StorageMaxSize / 10f) * 10;

			// Get the max number of storage extensions relative to the client's
			// default (60), as that's the value the client works with.
			this.StorageMaxExtensions = Math.Max(0, this.StorageMaxSize - 60) / 10;
		}
	}
}
