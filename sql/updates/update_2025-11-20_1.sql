-- Migration: Cards system refactor
-- Cards are now stored in the inventory table like regular equipment
-- instead of having their own separate table.
--
-- Cards use equipSlot values:
--   100-115 for equipped cards (card slots 1-15)
--   127 (0x7F) for unequipped cards in inventory
--
-- This makes cards work consistently with all other equipment types.

DROP TABLE IF EXISTS `cards`;