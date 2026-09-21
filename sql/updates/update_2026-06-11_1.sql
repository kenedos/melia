-- Add index on accounts.name to prevent full table scans
-- during GetAccount lookup in CZ_CONNECT auth phase.
ALTER TABLE `accounts` ADD INDEX `name` (`name`);
