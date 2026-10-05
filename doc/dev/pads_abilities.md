# Pads & abilities

Last verified: 2026-06-10 (thin — deepen as needed)

## Pads (AoE trigger zones)

Pads are persistent area effects (fire patches, auras, traps) created by
skills or monsters. **Full creation guide with handler examples:**
`doc/server/skills/pads.md` (user-facing but authoritative).

- Source: `src/ZoneServer/Pads/` — `Base/` (handler interfaces:
  `ICreatePadHandler`, `IUpdatePadHandler`, `IEnterPadHandler`,
  `ILeavePadHandler`, …), `Handlers/` (incl. `Monster/`, `Boss/`,
  `Generated/`), `Helpers/`
- Registration mirrors skills/buffs: `[PadHandler(PadName.X)]` classes,
  reflection-discovered, `[Package("laima")]` overrides win
  (`src/ZoneServer/Pads/PadHandlers.cs`)
- Pad names: `src/Shared/Game/Const/PadName.cs`
- Laima pads: `src/ZoneServer/Packages/Laima/Pads/`

## Abilities (passives)

Abilities are job passives toggled/leveled by the player.

- Handlers: `[AbilityHandler(AbilityId.X)]` classes implementing
  `IAbilityHandler`; registration in `src/ZoneServer/Abilities/AbilityHandlers.cs`
  (same package-override ordering)
- Golden example: `src/ZoneServer/Packages/Laima/Abilities/Falconer21.cs` —
  an ability whose entire effect is a `[CombatCalcModifier(phase, AbilityId.X)]`
  hook checking `attacker.TryGetActiveAbilityLevel(AbilityId.X, out var level)`
- Data: `packages/laima/db/abilities.txt` + `abilitytree.txt`; enum in
  `src/Shared/Game/Const/AbilityId.cs`
- Combat-hook phases and signature: see `buff_handlers.md` — identical
  mechanism, identified by AbilityId instead of BuffId.
