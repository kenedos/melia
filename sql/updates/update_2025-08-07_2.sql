-- Character Rollback System Database Schema (Idempotent Version)
-- This version can be run multiple times without causing "already exists" errors.

-- Main snapshots table
CREATE TABLE IF NOT EXISTS `character_snapshots` (
    `snapshotId` bigint(20) NOT NULL AUTO_INCREMENT,
    `characterId` bigint(20) NOT NULL,
    `accountId` bigint(20) NOT NULL,
    `snapshotReason` varchar(255) NOT NULL,
    `operationDetails` text DEFAULT NULL,
    `createdAt` datetime NOT NULL,
    `gameVersion` varchar(50) DEFAULT NULL,
    `characterLevel` int(11) DEFAULT 0,
    `mapId` int(11) DEFAULT 0,
    PRIMARY KEY (`snapshotId`),
    INDEX `idx_character_snapshots_character` (`characterId`),
    INDEX `idx_character_snapshots_account` (`accountId`),
    INDEX `idx_character_snapshots_created` (`createdAt`),
    INDEX `idx_character_snapshots_reason` (`snapshotReason`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Other tables...
CREATE TABLE IF NOT EXISTS `snapshot_character_data` (
    `snapshotId` bigint(20) NOT NULL,
    `characterId` bigint(20) NOT NULL,
    `name` varchar(64) NOT NULL,
    `teamName` varchar(64) NOT NULL,
    `jobId` smallint(6) NOT NULL,
    `gender` tinyint(4) NOT NULL,
    `hair` int(11) NOT NULL,
    `skinColor` int(10) unsigned NOT NULL,
    `mapId` int(11) NOT NULL,
    `x` float NOT NULL,
    `y` float NOT NULL,
    `z` float NOT NULL,
    `dir` float NOT NULL,
    `exp` bigint(20) NOT NULL,
    `maxExp` bigint(20) NOT NULL,
    `totalExp` bigint(20) NOT NULL,
    `equipVisibility` int(11) NOT NULL,
    `stamina` int(11) NOT NULL,
    `level` int(11) NOT NULL,
    PRIMARY KEY (`snapshotId`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_items` (
    `snapshotItemId` bigint(20) NOT NULL AUTO_INCREMENT,
    `snapshotId` bigint(20) NOT NULL,
    `originalItemId` bigint(20) DEFAULT NULL,
    `itemId` int(11) NOT NULL,
    `amount` int(11) NOT NULL,
    `isEquipped` tinyint(1) NOT NULL DEFAULT 0,
    `equipSlot` tinyint(3) unsigned DEFAULT NULL,
    `inventoryIndex` int(11) DEFAULT NULL,
    PRIMARY KEY (`snapshotItemId`),
    INDEX `idx_snapshot_items_snapshot` (`snapshotId`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_item_properties` (
    `snapshotItemId` bigint(20) NOT NULL,
    `name` varchar(255) NOT NULL,
    `type` varchar(10) NOT NULL,
    `value` mediumtext NOT NULL,
    PRIMARY KEY (`snapshotItemId`, `name`),
    FOREIGN KEY (`snapshotItemId`) REFERENCES `snapshot_items`(`snapshotItemId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_cards` (
    `snapshotId` bigint(20) NOT NULL,
    `cardIndex` int(11) NOT NULL,
    `itemId` int(11) NOT NULL,
    `amount` int(11) NOT NULL,
    PRIMARY KEY (`snapshotId`, `cardIndex`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_character_properties` (
    `snapshotId` bigint(20) NOT NULL,
    `name` varchar(255) NOT NULL,
    `type` varchar(10) NOT NULL,
    `value` mediumtext NOT NULL,
    PRIMARY KEY (`snapshotId`, `name`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_character_etc_properties` (
    `snapshotId` bigint(20) NOT NULL,
    `name` varchar(255) NOT NULL,
    `type` varchar(10) NOT NULL,
    `value` mediumtext NOT NULL,
    PRIMARY KEY (`snapshotId`, `name`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_variables` (
    `snapshotId` bigint(20) NOT NULL,
    `variableName` varchar(255) NOT NULL,
    `variableType` varchar(10) NOT NULL,
    `variableValue` mediumtext NOT NULL,
    PRIMARY KEY (`snapshotId`, `variableName`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_jobs` (
    `snapshotId` bigint(20) NOT NULL,
    `jobId` int(11) NOT NULL,
    `circle` int(11) NOT NULL,
    `skillPoints` int(11) NOT NULL,
    `totalExp` bigint(20) NOT NULL,
    `selectionDate` datetime NOT NULL,
    PRIMARY KEY (`snapshotId`, `jobId`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_skills` (
    `snapshotId` bigint(20) NOT NULL,
    `skillId` int(11) NOT NULL,
    `level` int(11) NOT NULL,
    PRIMARY KEY (`snapshotId`, `skillId`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_abilities` (
    `snapshotId` bigint(20) NOT NULL,
    `abilityId` int(11) NOT NULL,
    `level` int(11) NOT NULL,
    `active` tinyint(1) NOT NULL,
    PRIMARY KEY (`snapshotId`, `abilityId`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_quests` (
    `snapshotQuestId` bigint(20) NOT NULL AUTO_INCREMENT,
    `snapshotId` bigint(20) NOT NULL,
    `questClassId` bigint(20) NOT NULL,
    `status` int(11) NOT NULL,
    `startTime` datetime NOT NULL,
    `completeTime` datetime NOT NULL,
    `tracked` tinyint(1) NOT NULL,
    PRIMARY KEY (`snapshotQuestId`),
    INDEX `idx_snapshot_quests_snapshot` (`snapshotId`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_quest_progress` (
    `snapshotQuestId` bigint(20) NOT NULL,
    `ident` varchar(255) NOT NULL,
    `count` int(11) NOT NULL,
    `done` tinyint(1) NOT NULL,
    `unlocked` tinyint(1) NOT NULL,
    PRIMARY KEY (`snapshotQuestId`, `ident`),
    FOREIGN KEY (`snapshotQuestId`) REFERENCES `snapshot_quests`(`snapshotQuestId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_social_data` (
    `snapshotId` bigint(20) NOT NULL,
    `partyId` bigint(20) NOT NULL DEFAULT 0,
    `guildId` bigint(20) NOT NULL DEFAULT 0,
    PRIMARY KEY (`snapshotId`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_storage_personal` (
    `snapshotId` bigint(20) NOT NULL,
    `position` int(11) NOT NULL,
    `itemId` int(11) NOT NULL,
    `amount` int(11) NOT NULL,
    `originalItemDbId` bigint(20) DEFAULT NULL,
    PRIMARY KEY (`snapshotId`, `position`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `snapshot_storage_team` (
    `snapshotId` bigint(20) NOT NULL,
    `position` int(11) NOT NULL,
    `itemId` int(11) NOT NULL,
    `amount` int(11) NOT NULL,
    `originalItemDbId` bigint(20) DEFAULT NULL,
    PRIMARY KEY (`snapshotId`, `position`),
    FOREIGN KEY (`snapshotId`) REFERENCES `character_snapshots`(`snapshotId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `rollback_log` (
    `logId` bigint(20) NOT NULL AUTO_INCREMENT,
    `characterId` bigint(20) NOT NULL,
    `snapshotId` bigint(20) NOT NULL,
    `rollbackReason` varchar(500) NOT NULL,
    `rolledBackAt` datetime NOT NULL,
    `originalSnapshotReason` varchar(255) NOT NULL,
    `originalSnapshotDate` datetime NOT NULL,
    PRIMARY KEY (`logId`),
    INDEX `idx_rollback_log_character` (`characterId`),
    INDEX `idx_rollback_log_snapshot` (`snapshotId`),
    INDEX `idx_rollback_log_date` (`rolledBackAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `mass_rollback_log` (
    `massRollbackId` bigint(20) NOT NULL AUTO_INCREMENT,
    `bugDescription` text NOT NULL,
    `fromTime` datetime NOT NULL,
    `toTime` datetime NOT NULL,
    `totalAttempted` int(11) NOT NULL,
    `successfulCount` int(11) NOT NULL,
    `failedCount` int(11) NOT NULL,
    `executedAt` datetime NOT NULL,
    `resultDetails` longtext DEFAULT NULL,
    PRIMARY KEY (`massRollbackId`),
    INDEX `idx_mass_rollback_executed` (`executedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Views for easier querying
CREATE OR REPLACE VIEW `v_recent_snapshots` AS
SELECT
    cs.snapshotId, cs.characterId, cs.accountId, cs.snapshotReason, cs.operationDetails, cs.createdAt,
    cs.gameVersion, cs.characterLevel, cs.mapId, scd.name as characterName, scd.teamName,
    CASE
        WHEN cs.createdAt >= DATE_SUB(NOW(), INTERVAL 24 HOUR) THEN 'Last 24 Hours'
        WHEN cs.createdAt >= DATE_SUB(NOW(), INTERVAL 7 DAY) THEN 'Last Week'
        WHEN cs.createdAt >= DATE_SUB(NOW(), INTERVAL 30 DAY) THEN 'Last Month'
        ELSE 'Older'
    END as ageCategory
FROM character_snapshots cs
LEFT JOIN snapshot_character_data scd ON cs.snapshotId = scd.snapshotId
WHERE cs.createdAt >= DATE_SUB(NOW(), INTERVAL 90 DAY)
ORDER BY cs.createdAt DESC;

CREATE OR REPLACE VIEW `v_rollback_summary` AS
SELECT
    rl.characterId, scd.name as characterName, scd.teamName, COUNT(*) as totalRollbacks,
    MAX(rl.rolledBackAt) as lastRollback,
    GROUP_CONCAT(DISTINCT rl.rollbackReason ORDER BY rl.rolledBackAt DESC SEPARATOR '; ') as reasons
FROM rollback_log rl
LEFT JOIN character_snapshots cs ON rl.snapshotId = cs.snapshotId
LEFT JOIN snapshot_character_data scd ON cs.snapshotId = scd.snapshotId
GROUP BY rl.characterId, scd.name, scd.teamName
ORDER BY totalRollbacks DESC, lastRollback DESC;

CREATE OR REPLACE VIEW `v_snapshot_statistics` AS
SELECT
    DATE(createdAt) as snapshotDate, snapshotReason, COUNT(*) as snapshotCount,
    COUNT(DISTINCT characterId) as uniqueCharacters,
    COUNT(DISTINCT accountId) as uniqueAccounts
FROM character_snapshots
WHERE createdAt >= DATE_SUB(NOW(), INTERVAL 30 DAY)
GROUP BY DATE(createdAt), snapshotReason
ORDER BY snapshotDate DESC, snapshotCount DESC;

-- Cleanup and maintenance procedures
DROP PROCEDURE IF EXISTS `sp_cleanup_old_snapshots`;
CREATE PROCEDURE `sp_cleanup_old_snapshots`(
    IN retention_days INT,
    IN preserve_critical BOOLEAN,
    IN batch_size INT
)
BEGIN
    -- (Procedure body as in previous answer)
    DECLARE done INT DEFAULT FALSE;
    DECLARE snapshot_id BIGINT;
    DECLARE total_deleted INT DEFAULT 0;
    DECLARE batch_deleted INT DEFAULT 0;
    DECLARE effective_retention INT;
    DECLARE effective_batch_size INT;

    SET effective_retention = IFNULL(retention_days, 90);
    SET effective_batch_size = IFNULL(batch_size, 1000);

    delete_loop: LOOP
        SET batch_deleted = 0;
        CREATE TEMPORARY TABLE IF NOT EXISTS snapshots_to_delete (id BIGINT PRIMARY KEY);
        TRUNCATE TABLE snapshots_to_delete;

        INSERT INTO snapshots_to_delete (id)
        SELECT snapshotId
        FROM character_snapshots
        WHERE createdAt < DATE_SUB(NOW(), INTERVAL effective_retention DAY)
        AND (preserve_critical = FALSE OR snapshotReason != 'CRITICAL_PRESERVE')
        LIMIT effective_batch_size;

        SET batch_deleted = (SELECT COUNT(*) FROM snapshots_to_delete);
        IF batch_deleted = 0 THEN
            LEAVE delete_loop;
        END IF;

        DELETE FROM snapshot_item_properties WHERE snapshotItemId IN (SELECT si.snapshotItemId FROM snapshot_items si JOIN snapshots_to_delete d ON si.snapshotId = d.id);
        DELETE FROM snapshot_items WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_cards WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_character_properties WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_character_etc_properties WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_variables WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_jobs WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_skills WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_abilities WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_quest_progress WHERE snapshotQuestId IN (SELECT sq.snapshotQuestId FROM snapshot_quests sq JOIN snapshots_to_delete d ON sq.snapshotId = d.id);
        DELETE FROM snapshot_quests WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_social_data WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_storage_personal WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_storage_team WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM snapshot_character_data WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);
        DELETE FROM character_snapshots WHERE snapshotId IN (SELECT id FROM snapshots_to_delete);

        SET total_deleted = total_deleted + batch_deleted;
    END LOOP;

    DROP TEMPORARY TABLE IF EXISTS snapshots_to_delete;
    SELECT CONCAT('Deleted ', total_deleted, ' snapshots') as result;
END;

DROP PROCEDURE IF EXISTS `sp_snapshot_size_report`;
CREATE PROCEDURE `sp_snapshot_size_report`()
BEGIN
    -- (Procedure body as in previous answer)
    SELECT
        table_schema AS database_name,
        table_name,
        ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS `size_in_mb`
    FROM
        information_schema.tables
    WHERE
        table_schema = DATABASE() AND
        (table_name LIKE 'snapshot_%' OR table_name = 'character_snapshots')
    GROUP BY
        table_name, table_schema
    ORDER BY
        `size_in_mb` DESC;
END;