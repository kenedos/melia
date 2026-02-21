CREATE TABLE IF NOT EXISTS `cards` (
  `characterId` bigint(20) NOT NULL,
  `itemId` bigint(20) NOT NULL,
  `sort` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE `cards`
  ADD PRIMARY KEY (`characterId`,`itemId`),
  ADD KEY `itemId` (`itemId`);

ALTER TABLE `cards`
  ADD CONSTRAINT `cards_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `cards_ibfk_2` FOREIGN KEY (`itemId`) REFERENCES `items` (`itemUniqueId`) ON DELETE CASCADE ON UPDATE CASCADE;