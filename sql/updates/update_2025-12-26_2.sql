-- Fix for deadlock issue caused by DELETE + INSERT pattern
-- Adding UNIQUE constraints to enable UPSERT pattern and prevent gap lock deadlocks

-- Skills table: Add unique constraint on (characterId, id)
-- This allows INSERT ON DUPLICATE KEY UPDATE instead of DELETE ALL + INSERT
ALTER TABLE `skills` ADD UNIQUE KEY `uk_character_skill` (`characterId`, `id`);

-- Abilities table: Add unique constraint on (characterId, id)
ALTER TABLE `abilities` ADD UNIQUE KEY `uk_character_ability` (`characterId`, `id`);

-- Cooldowns table: Add unique constraint on (characterId, classId)
ALTER TABLE `cooldowns` ADD UNIQUE KEY `uk_character_cooldown` (`characterId`, `classId`);

-- Achievements table: Add unique constraint on (characterId, achievementId)
ALTER TABLE `achievements` ADD UNIQUE KEY `uk_character_achievement` (`characterId`, `achievementId`);

-- Achievement points table: Add unique constraint on (characterId, pointId)
ALTER TABLE `achievement_points` ADD UNIQUE KEY `uk_character_point` (`characterId`, `pointId`);

-- Quests table: Add unique constraint on (characterId, classId)
ALTER TABLE `quests` ADD UNIQUE KEY `uk_character_quest` (`characterId`, `classId`);

-- Variables table: Add unique constraint on (characterId, name)
ALTER TABLE `vars_characters` ADD UNIQUE KEY `uk_character_var` (`characterId`, `name`);

-- Character properties table: Add unique constraint on (characterId, name)
ALTER TABLE `character_properties` ADD UNIQUE KEY `uk_character_property` (`characterId`, `name`);

-- Character etc properties table: Add unique constraint on (characterId, name)
ALTER TABLE `character_etc_properties` ADD UNIQUE KEY `uk_character_etc_property` (`characterId`, `name`);
