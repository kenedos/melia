ALTER TABLE `achievements`
ADD COLUMN `unlockDate` BIGINT NOT NULL DEFAULT 0 AFTER `achievementId`;