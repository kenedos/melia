# Network & packets

Last verified: 2026-06-10 (thin — deepen as needed)

## Authoritative source files

- `src/ZoneServer/Network/PacketHandler*.cs` — inbound client packets:
  methods marked `[PacketHandler(Op.CZ_*)]`, discovered by reflection
- `src/ZoneServer/Network/Send*.cs` — outbound packets: static `Send.ZC_*`
  methods (partials: `Send.cs`, `Send.Normal.cs` for sub-channel "normal"
  packets, `Send.Group.cs`, `Send.Housing.cs`, `Send.Normal.Market.cs`, legacy)
- `src/ZoneServer/Network/Helpers/` — packet-building helpers shared by senders
- `src/Shared/Network/` — `Op` opcode table, packet primitives, inter-server
- `doc/bt/` — binary template notes for wire formats

## Conventions

- Inbound: add a `[PacketHandler(Op.CZ_X)]` method to the right
  `PacketHandler.*.cs` partial; read fields in wire order; validate before
  touching world state.
- Outbound: add a `Send.ZC_X(...)` static method next to related senders;
  many "ZC_NORMAL" sub-packets live in `Send.Normal.cs`.
- Opcodes must match the supported client (Laima branch: 390044, versioned
  data under `system/versions/` — see `data_system.md`).
- Barracks/Social/Web have parallel Network folders with the same pattern.

When adding a packet: find an existing handler/sender for a similar feature
and mirror it; verify opcode and field layout against `doc/bt/` or client data.
