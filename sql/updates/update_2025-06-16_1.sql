CREATE TABLE IF NOT EXISTS `item_properties_log` (
  `logId` bigint(20) NOT NULL AUTO_INCREMENT,
  `itemId` bigint(20) NOT NULL,
  `name` varchar(128) NOT NULL,
  `type` char(1) NOT NULL,
  `value` mediumtext DEFAULT NULL,
  `backupTimestamp` datetime NOT NULL DEFAULT current_timestamp(),
  `backupReason` varchar(50) DEFAULT 'pre_save',
  PRIMARY KEY (`logId`),
  KEY `idx_itemId_timestamp` (`itemId`,`backupTimestamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
