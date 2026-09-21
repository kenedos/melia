-- Fix missing unique keys required by owner-scoped variable UPSERT saves.

-- vars_accounts: Deduplicate existing rows (keep latest varId per accountId+name)
DELETE va1 FROM `vars_accounts` va1
INNER JOIN `vars_accounts` va2
  ON va1.`accountId` = va2.`accountId`
  AND va1.`name` = va2.`name`
  AND va1.`varId` < va2.`varId`;

-- vars_accounts: Add unique constraint on (accountId, name)
ALTER TABLE `vars_accounts` ADD UNIQUE KEY `uk_account_var` (`accountId`, `name`);

-- vars_buffs: Deduplicate existing rows (keep latest varId per buffId+name)
DELETE vb1 FROM `vars_buffs` vb1
INNER JOIN `vars_buffs` vb2
  ON vb1.`buffId` = vb2.`buffId`
  AND vb1.`name` = vb2.`name`
  AND vb1.`varId` < vb2.`varId`;

-- vars_buffs: Add unique constraint on (buffId, name)
ALTER TABLE `vars_buffs` ADD UNIQUE KEY `uk_buff_var` (`buffId`, `name`);
