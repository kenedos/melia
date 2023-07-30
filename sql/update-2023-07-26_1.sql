CREATE TABLE `storage_team` (
  `accountId` bigint(20) NOT NULL,
  `itemId` bigint(20) NOT NULL,
  PRIMARY KEY (`accountId`,`itemId`),
  KEY `itemId` (`itemId`),
  CONSTRAINT `storage_team_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `storage_team_ibfk_2` FOREIGN KEY (`itemId`) REFERENCES `items` (`itemUniqueId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `storage_team_silver` (
  `entryId` bigint(20) NOT NULL AUTO_INCREMENT,
  `accountId` bigint(20) NOT NULL,
  `interaction` tinyint(4) NOT NULL,
  `silverTransacted` bigint(20) NOT NULL,
  `silverTotal` varchar(64) NOT NULL,
  `entryDateTime` datetime NOT NULL,
  PRIMARY KEY (`entryId`),
  CONSTRAINT `storage_team_silver_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
