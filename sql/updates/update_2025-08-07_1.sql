-- For logging all item additions and removals
CREATE TABLE `log_item_transactions` (
  `id` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `characterId` BIGINT UNSIGNED NOT NULL,
  `characterName` VARCHAR(50) NOT NULL,
  `itemObjectId` BIGINT UNSIGNED NOT NULL COMMENT 'In-memory unique ID of the item instance',
  `itemDbId` BIGINT UNSIGNED DEFAULT NULL COMMENT 'Database unique ID of the item instance',
  `itemId` INT UNSIGNED NOT NULL COMMENT 'The class ID of the item',
  `itemName` VARCHAR(100) DEFAULT NULL,
  `amount` INT NOT NULL,
  `transactionType` VARCHAR(20) NOT NULL COMMENT 'e.g., Add, Remove',
  `reason` VARCHAR(255) DEFAULT NULL COMMENT 'e.g., Pickup, Quest Reward, Used, Sold',
  `timestamp` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `idx_characterId` (`characterId`),
  INDEX `idx_timestamp` (`timestamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- For logging player chat messages
CREATE TABLE `log_chat` (
  `id` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `characterId` BIGINT UNSIGNED NOT NULL,
  `characterName` VARCHAR(50) NOT NULL,
  `chatType` INT NOT NULL COMMENT 'e.g., Normal, Whisper, Party, Guild',
  `message` TEXT NOT NULL,
  `targetName` VARCHAR(50) DEFAULT NULL COMMENT 'For whispers or private channels',
  `timestamp` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `idx_characterId` (`characterId`),
  INDEX `idx_timestamp` (`timestamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- For logging bot-related activities
CREATE TABLE `log_bot` (
  `id` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `characterId` BIGINT UNSIGNED NOT NULL,
  `characterName` VARCHAR(50) NOT NULL,
  `activity` VARCHAR(100) NOT NULL COMMENT 'e.g., Suspicious Movement, High-speed Looting',
  `details` TEXT,
  `timestamp` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `idx_characterId` (`characterId`),
  INDEX `idx_timestamp` (`timestamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;