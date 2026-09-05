CREATE TABLE IF NOT EXISTS `storage_oblation` (
  `characterId` bigint(20) NOT NULL,
  `itemId` bigint(20) NOT NULL,
  `position` int(11) NOT NULL,
  `pricePaid` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`characterId`,`itemId`),
  KEY `itemId` (`itemId`),
  CONSTRAINT `storage_oblation_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `storage_oblation_ibfk_2` FOREIGN KEY (`itemId`) REFERENCES `items` (`itemUniqueId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
