CREATE TABLE IF NOT EXISTS `adventure_book` (
  `id` bigint(20) NOT NULL,
  `accountId` bigint(20) NOT NULL,
  `type` tinyint(1) NOT NULL,
  `classId` int(11)  NOT NULL,
  `count` int(11)  NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE `adventure_book`
  ADD PRIMARY KEY (`id`),
  ADD KEY `accountId` (`accountId`);
  
ALTER TABLE `adventure_book`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

ALTER TABLE `adventure_book`
  ADD KEY `adventure_book_ibfk_1` (`accountId`);
  
ALTER TABLE `adventure_book`
  ADD CONSTRAINT `adventure_book_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

CREATE TABLE IF NOT EXISTS `adventure_book_monster_drops` (
  `id` bigint(20) NOT NULL,
  `accountId` bigint(20) NOT NULL,
  `monsterId` int(11)  NOT NULL,
  `itemId` int(11)  NOT NULL,
  `count` int(11)  NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE `adventure_book_monster_drops`
  ADD PRIMARY KEY (`id`),
  ADD KEY `accountId` (`accountId`);
  
ALTER TABLE `adventure_book_monster_drops`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

ALTER TABLE `adventure_book_monster_drops`
  ADD KEY `adventure_book_monster_drops_ibfk_1` (`accountId`);
  
ALTER TABLE `adventure_book_monster_drops`
  ADD CONSTRAINT `adventure_book_monster_drops_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;
  
CREATE TABLE IF NOT EXISTS `adventure_book_items` (
  `id` bigint(20) NOT NULL,
  `accountId` bigint(20) NOT NULL,
  `itemId` int(11)  NOT NULL,
  `craftCount` int(11)  NOT NULL DEFAULT 0,
  `obtainCount` int(11)  NOT NULL DEFAULT 0,
  `useCount` int(11)  NOT NULL DEFAULT 0,
  `count` int(11)  NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE `adventure_book_items`
  ADD PRIMARY KEY (`id`),
  ADD KEY `accountId` (`accountId`);
  
ALTER TABLE `adventure_book_items`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

ALTER TABLE `adventure_book_items`
  ADD KEY `adventure_book_items_ibfk_1` (`accountId`);
  
ALTER TABLE `adventure_book_items`
  ADD CONSTRAINT `adventure_book_items_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;
  