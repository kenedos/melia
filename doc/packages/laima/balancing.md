# Laima balancing & design intent

Last verified: 2026-06-10 (seed version — extend with per-class rework notes)

Design conventions observed across the Laima rework; per-skill intent is
recorded in each handler's class doc comment ("Per the rework, …") — that is
the primary record, keep writing those.

## Conventions

- **Reworks, not replicas**: Laima handlers deliberately change skill roles
  (e.g. Cleave became a low-damage, wide-area debuff applier that amplifies
  Slash damage). State the new role in the handler doc comment.
- **Scaling shape**: effects commonly scale as `base + per-level * skill
  level`, with the skill level passed to buffs as `NumArg1`
  (e.g. Cleave debuff: +30% +2%/level Slash vulnerability; Embowel buff:
  40% +2%/level damage reduction capped at 80%).
- **Stack-based buffs**: cap stacks from skill level (Frenzy: 6→20 stacks
  across levels 1→10), decay on a timer via `WhileActive`, bonuses updated
  per stack via property modifiers.
- **Ability reinforcement**: damage hooks multiply in
  `SCR_Get_AbilityReinforceRate` where the upstream skill supports it.
- **Numbers tuning order**: prefer `packages/laima/db/skills_overrides.txt` /
  `buffs_overrides.txt` for stat changes; touch handler constants only when
  the mechanic itself changes.
- Monster cards were rebalanced separately: `monster_cards_rebalanced.md`.
- Item grades: see `packages/laima/db/item_grades.txt` (Item Grade Balance,
  commit 5b6e697b1).

## Per-class notes

(To be filled in as classes are revisited — link the relevant handler folders,
e.g. `src/ZoneServer/Packages/Laima/Skills/Swordsmen/Doppelsoeldner/`.)
