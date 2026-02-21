-- Migration: Clean up snapshot_cards table
-- Cards are now included in the snapshot_items table with equipSlot values 100-115
-- The separate snapshot_cards table is no longer needed

DROP TABLE IF EXISTS `snapshot_cards`;