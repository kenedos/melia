-- Fix for deadlock issue caused by DELETE + INSERT pattern on item_properties and account_properties
-- Adding UNIQUE constraints to enable UPSERT pattern and prevent gap lock deadlocks
-- Follows pattern from update_2025-12-26_2.sql

-- item_properties: Deduplicate existing rows (keep latest propertyId per itemId+name)
DELETE ip1 FROM `item_properties` ip1
INNER JOIN `item_properties` ip2
  ON ip1.`itemId` = ip2.`itemId`
  AND ip1.`name` = ip2.`name`
  AND ip1.`propertyId` < ip2.`propertyId`;

-- item_properties: Add unique constraint on (itemId, name)
ALTER TABLE `item_properties` ADD UNIQUE KEY `uk_item_property` (`itemId`, `name`);

-- account_properties: Deduplicate existing rows (keep latest propertyId per accountId+name)
DELETE ap1 FROM `account_properties` ap1
INNER JOIN `account_properties` ap2
  ON ap1.`accountId` = ap2.`accountId`
  AND ap1.`name` = ap2.`name`
  AND ap1.`propertyId` < ap2.`propertyId`;

-- account_properties: Add unique constraint on (accountId, name)
ALTER TABLE `account_properties` ADD UNIQUE KEY `uk_account_property` (`accountId`, `name`);
