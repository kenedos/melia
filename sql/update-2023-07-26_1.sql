CREATE TABLE `storage_team` (
  `accountId` bigint(20) NOT NULL,
  `itemId` bigint(20) NOT NULL,
  PRIMARY KEY (`accountId`,`itemId`),
  KEY `itemId` (`itemId`),
  CONSTRAINT `storage_team_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `storage_team_ibfk_2` FOREIGN KEY (`itemId`) REFERENCES `items` (`itemUniqueId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;