-- Add equippedTitleId column to snapshot_character_data table for rollback feature
-- Execute this SQL to update your database schema

ALTER TABLE `snapshot_character_data` ADD COLUMN `equippedTitleId` INT NOT NULL DEFAULT -1 AFTER `equipVisibility`;
