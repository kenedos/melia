CREATE TABLE IF NOT EXISTS `achievements` (
  `characterId` bigint(20) NOT NULL,
  `achievementId` bigint(20)  NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE `achievements`
  ADD PRIMARY KEY (`achievementId`, `characterId`);

ALTER TABLE `achievements`
  ADD KEY `achievements_ibfk_1` (`characterId`);
  
ALTER TABLE `achievements`
  ADD CONSTRAINT `achievements_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;
  
CREATE TABLE IF NOT EXISTS `achievement_points` (
  `characterId` bigint(20) NOT NULL,
  `pointId` bigint(20) NOT NULL,
  `pointValue` mediumint NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE `achievement_points`
  ADD PRIMARY KEY (`characterId`, `pointId`);

ALTER TABLE `achievement_points`
  ADD KEY `achievement_points_ibfk_1` (`characterId`);
  
ALTER TABLE `achievement_points`
  ADD CONSTRAINT `achievement_points_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;