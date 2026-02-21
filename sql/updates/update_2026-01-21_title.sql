-- Add equippedTitleId column to characters table for equippable achievement titles
-- Execute this SQL to update your database schema

ALTER TABLE `characters` ADD COLUMN `equippedTitleId` INT NOT NULL DEFAULT -1 AFTER `equipVisibility`;

-- Update existing characters to have no title equipped by default
UPDATE `characters` SET `equippedTitleId` = -1;
