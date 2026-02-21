-- Add advDate column to snapshot_jobs table for rollback system
ALTER TABLE `snapshot_jobs` ADD COLUMN `advDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP AFTER `selectionDate`;

-- Backfill NULL advDate values in jobs table using selectionDate
UPDATE `jobs` SET `advDate` = `selectionDate` WHERE `advDate` IS NULL;
