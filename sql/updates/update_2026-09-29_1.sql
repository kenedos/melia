CREATE TABLE IF NOT EXISTS `hunting_tasks` (
    `accountId` BIGINT NOT NULL,
    `points` INT NOT NULL DEFAULT 0,
    `tasksCompleted` INT NOT NULL DEFAULT 0,
    `status` TINYINT UNSIGNED NOT NULL DEFAULT 0,
    `option1MonsterId` INT NOT NULL DEFAULT 0,
    `option2MonsterId` INT NOT NULL DEFAULT 0,
    `option3MonsterId` INT NOT NULL DEFAULT 0,
    `selectedMonsterId` INT NOT NULL DEFAULT 0,
    `requiredKills` INT NOT NULL DEFAULT 0,
    `currentKills` INT NOT NULL DEFAULT 0,
    `rewardPoints` INT NOT NULL DEFAULT 0,
    `rerollCount` INT NOT NULL DEFAULT 0,
    PRIMARY KEY (`accountId`),
    CONSTRAINT `hunting_tasks_ibfk_1`
        FOREIGN KEY (`accountId`)
        REFERENCES `accounts` (`accountId`)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
