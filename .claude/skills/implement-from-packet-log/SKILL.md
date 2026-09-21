---
name: implement-from-packet-log
description: Implement a Melia server feature (skill, buff, pad, ability, character/gameplay behavior) by reconstructing it from a client packet log. Use when the user points to a .txt log of client<->server traffic and asks to build, replicate or fix a feature based on it, e.g. "implement this skill from the log", "here are the packets for X".
argument-hint: <path-to-log.txt> <feature to implement>
---

# Implement a feature from a client packet log

The user gives you a `.txt` log captured from the game client (requests it sent, responses the
official server gave) and names a feature. Your job is to make this server produce the same
conversation: same packets, same order, same timing, with the gameplay logic behind them written
the way this project already writes it.

The log is the source of truth for *what goes over the wire and when*. The codebase is the source
of truth for *how to express it*. Never invent packet layouts or helper APIs; find them.

If the log path or the target feature is missing, ask for it. Everything else you work out yourself.

## 1. Read the log

Logs can be large. Don't read them top to bottom. Open the first ~100 lines to learn the format
(timestamp style, direction marker, op name or numeric op, hex dump vs. parsed fields), then Grep
for what matters:

- the skill/buff/pad class name or numeric id (`SkillId`, `BuffId` in `src/Shared/Game/Const/`)
- the client request that starts the interaction (`CZ_SKILL_GROUND`, `CZ_SKILL_TARGET`,
  `CZ_SKILL_SELF`, `CZ_DYNAMIC_CASTING_START/END`, `CZ_SKILL_CANCEL`, ...)
- the caster's handle, once you know it, to separate their traffic from the noise

Isolate one clean use of the feature, from the client request to the last server packet it caused.
If the log has several uses, compare at least two: what stays constant is the feature, what varies
is input (positions, handles, damage, targets hit or not).

Ignore ambient traffic (movement, `ZC_MOVE_*`, chat, heartbeat, other actors' updates) unless the
feature causes it.

Build a timeline table before writing any code:

| t (ms from request) | dir | packet | key fields |
|---|---|---|---|

`t` is relative to the client request that triggered the sequence. This table drives step 4.

### Decoding packets

- Op names and values: `src/Shared/Network/Op.cs`.
- `ZC_NORMAL` carries a sub-op in its first int: `src/Shared/Network/NormalOp.cs`
  (`NormalOp.Zone`). Sub-op values shift between client versions (see the comment at the top of
  `Zone` and `SubOpcodeMapper`); if a value from the log doesn't match by number, match by payload
  shape against the senders in `Send.Normal.cs`.
- Field layouts: `doc/bt/<OP_NAME>.bt` (010 Editor templates, one per packet) and the matching
  method in `src/ZoneServer/Network/Send*.cs`. Incoming layouts are in
  `src/ZoneServer/Network/PacketHandler*.cs` under `[PacketHandler(Op.X)]`.
- Hex dumps are little-endian. Floats that look like garbage as ints are usually positions or
  directions (cos/sin pairs). Strings are often effect names (`F_...`, `I_...`) and tell you which
  `Send.ZC_NORMAL.*` effect helper was used.

## 2. Find how the project already does it

Before writing, read two or three existing implementations closest to the feature. Locations:

| What | Where |
|---|---|
| Skill handlers (custom, preferred) | `src/ZoneServer/Packages/Laima/Skills/<Class>/<Job>/` |
| Skill handlers (base) | `src/ZoneServer/Skills/Handlers/<Class>/<Job>/` |
| Handler interfaces | `src/ZoneServer/Skills/Handlers/Base/Interfaces.cs` |
| Buff handlers | `src/ZoneServer/Packages/Laima/Buffs/<Class>/<Job>/` |
| Pad handlers | `src/ZoneServer/Packages/Laima/Pads/<Class>/<Job>/` or next to the skill that owns them |
| Abilities | `src/ZoneServer/Packages/Laima/Abilities/` |
| Hit calculation | `SkillUseFunctions` (`SCR_SkillHit`), `HitInfo`, `SkillHitInfo` |
| Splash areas | `src/ZoneServer/Skills/SplashAreas/` (`Circle`, `Square`, `Fan`) |
| Outgoing packets | `src/ZoneServer/Network/Send.cs`, `Send.Normal.cs` |
| Incoming packets | `src/ZoneServer/Network/PacketHandler.cs` |

The repo is big and a recursive Grep over `src` can time out. Scope searches to one of the
directories above.

Pick the handler interface from the *client request* in the log, not from the skill's description:
`CZ_SKILL_GROUND` -> `IGroundSkillHandler` (or `IMeleeGroundSkillHandler`), `CZ_SKILL_TARGET` ->
`ITargetSkillHandler`, `CZ_SKILL_SELF` -> `ISelfSkillHandler`, dynamic cast packets ->
additionally `IDynamicCasted`, cancel -> `ICancelSkillHandler`. Check `PacketHandler.cs` for how
the request is dispatched if unsure.

Map every server packet in your timeline to an existing `Send.*` method. Only add a new sender
when nothing fits; then add it next to its siblings in `Send.cs` / `Send.Normal.cs` in the same
style (`using var packet = Packet.Rent(Op.X)`, XML summary, broadcast via `Map.Broadcast` or
`conn.Send`), and add the op / sub-op constant if it's missing. Fields whose meaning you can't
determine keep the log's constant value and a neutral name (`i1`, `f1`), as the project does.

If the feature already has a handler, modify it instead of creating a second one. Laima package
handlers (`[Package("laima")]`) override base handlers for the same id.

## 3. Write the code

Match the surrounding files exactly: tabs, `this.` on member access, `var`, constants as
`private const` / `private readonly static TimeSpan` at the top of the class, XML `<summary>` on
the class and its methods, file named `<Job>_<SkillName>.cs` matching the `SkillId` enum name.

Standard skill shape, in this order:

1. `caster.TrySpendSp(skill)` guard with the localized "Not enough SP." message
2. `skill.IncreaseOverheat()`, `caster.TurnTowards(...)` if the log shows a rotation,
   `caster.SetAttackState(true)`
3. the immediate packets, in the order the log shows them (typically effect sub-ops, then
   `ZC_SKILL_READY`, then `ZC_SKILL_MELEE_GROUND` / `ZC_SKILL_FORCE_TARGET` / ...)
4. `skill.Run(this.Attack(...))` for anything that happens later

Damage goes through `SCR_SkillHit` -> `target.TakeDamage` -> `new HitInfo(...)` ->
`Send.ZC_HIT_INFO`, with targets from `caster.Map.GetAttackableEnemiesIn(caster, splashArea)` and
`LimitBySDR` where the skill is AoE-ratio limited. Splash dimensions come from the log's positions
and the skill data, not guesses; if you must estimate, say so in your report.

Persistent or moving ground effects are pads; stat changes with a duration are buffs with
`AddPropertyModifier` / `RemovePropertyModifier`. Don't emulate either with a timed task inside
the skill when a pad or buff appears in the log.

Comments: only what the code can't say. The existing pattern is a short note for an ability/arts
branch (`// [Arts] Name` + one line on the effect), for a deliberate deviation from the client
formula, or for a packet whose purpose is a visual trick. No narration of obvious steps, no
references to the log or to this task.

## 4. Packet timing (skills)

The client animates on its own clock. A hit packet that arrives too early plays damage numbers
before the projectile lands; too late and the skill feels laggy. Reproduce the log's spacing.

From the timeline table, group server packets into bursts (packets within ~30 ms of each other are
one burst: network jitter, not design). The first burst is sent synchronously from `Handle`. Each
later burst is preceded by a wait equal to the gap from the previous burst:

```csharp
private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
{
	await skill.Wait(HitDelay);

	// second burst
}
```

Rules:

- Use `skill.Wait(...)` inside a task started with `skill.Run(...)`. It ties the delay to the
  skill's cancellation token, so a cancelled or interrupted cast stops sending packets. Use raw
  `Task.Delay` only for work that must outlive the skill, and start that with `skill.RunFree`.
- Waits are gaps between bursts, not offsets from the start. Three hits at 300/500/700 ms is
  `Wait(300)`, then `Wait(200)` inside the loop, not three absolute timers.
- Round to the design value the capture is obviously approximating (487 ms -> 500 ms). If two
  captures disagree by more than jitter, the delay probably scales with something (distance,
  attack speed, cast time); find what before hardcoding.
- Name every delay as a constant (`HitDelay`, `DelayBetweenHits`). No inline magic numbers for
  anything used more than once.
- `HitInfo`'s delay argument is the *client-side* display delay carried inside the packet. It is
  not a substitute for waiting: read its value from the logged `ZC_HIT_INFO` and still send the
  packet at the logged time.
- Re-query targets after the wait, not before it. Enemies move during the delay and the official
  server evaluates the area at hit time.
- If the log shows no gap, add no wait. Don't pad timing "to be safe".
- Dynamic casts: `StartDynamicCast` / `EndDynamicCast` only mirror the cast packets; the time the
  player spent casting is already elapsed when `Handle` runs, so don't wait for it again.

Awaits are for skill handlers only. Pads use `PadOptions.LifeTime` / `UpdateInterval`, buffs use
their duration and `OnUpdate` tick, and non-skill features (NPC dialogs, item use, UI responses)
answer immediately; don't add delays there even if the capture shows latency.

## 5. Verify

Build the zone server and fix every error and any warning you introduced:

```
dotnet build src/ZoneServer/ZoneServer.csproj
```

Then walk your timeline table against the code once: every server packet in the capture is either
sent, in order, at the right burst, or deliberately left out with a reason. You can't run the game
client, so don't claim in-game behavior is confirmed; say what was checked (build, packet order,
timing) and what needs an in-game test.

Report back briefly: files touched, the timeline you reconstructed, any field or value you had to
estimate, and anything in the log you couldn't attribute to the feature.
