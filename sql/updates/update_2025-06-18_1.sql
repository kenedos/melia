-- For account properties
CREATE TABLE `account_properties_log` LIKE `item_properties_log`;
ALTER TABLE `account_properties_log` CHANGE `itemId` `accountId` BIGINT(20) NOT NULL;

-- For character properties
CREATE TABLE `character_properties_log` LIKE `item_properties_log`;
ALTER TABLE `character_properties_log` CHANGE `itemId` `characterId` BIGINT(20) NOT NULL;

-- For etc properties
CREATE TABLE `character_etc_properties_log` LIKE `item_properties_log`;
ALTER TABLE `character_etc_properties_log` CHANGE `itemId` `characterId` BIGINT(20) NOT NULL;