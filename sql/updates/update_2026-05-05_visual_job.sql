ALTER TABLE `characters` ADD COLUMN `visualJob` smallint(6) NOT NULL DEFAULT 0;
UPDATE `characters` SET `visualJob` = `job` WHERE `visualJob` = 0;
