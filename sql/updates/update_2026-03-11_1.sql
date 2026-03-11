ALTER TABLE `help` DROP FOREIGN KEY `help_ibfk_1`;
ALTER TABLE `help` DROP PRIMARY KEY;

TRUNCATE TABLE `help`;

ALTER TABLE `help` DROP COLUMN `characterId`;
ALTER TABLE `help` ADD COLUMN `accountId` bigint(20) NOT NULL FIRST;

ALTER TABLE `help` ADD PRIMARY KEY (`accountId`, `helpId`);
ALTER TABLE `help` ADD CONSTRAINT `help_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;
