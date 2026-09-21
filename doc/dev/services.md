# ZoneServer services

Last verified: 2026-06-10 (thin — deepen as needed)

Background services in `src/ZoneServer/Services/`, constructed at server
startup (see `ZoneServer.cs`), typically timer-driven and `IDisposable`:

| Service | Owns |
|---|---|
| `AutoSaveService` | slot-based periodic character saves (`persistence.md`); `SaveAllNow()` used at shutdown; triggers orphan cleanup each full cycle |
| `OrphanCleanupService` | cleanup of orphaned db rows (items etc.) |
| `DeadConnectionSweepService` | disconnecting clients that crashed without a TCP FIN. Liveness = `ZoneConnection.LastHeartBeat`, stamped (UTC) on *every* received packet in `ZoneConnection.OnPacketReceived` — not just CZ_HEARTBEAT. Conf (`world/misc.conf`): `dead_connection_timeout_seconds` (default 120, 0 = disabled), `dead_connection_sweep_interval_seconds` (default 30). Connections still loading/warping (`!GameReady`, `!LoadComplete`, `IsWarping`) get 5× the timeout |
| `MapContentService` | map-bound content lifecycle |
| `AchievementService` | achievement progress/grants |
| `LogCleanupService` | pruning old logs |

Related but elsewhere: `ServerShutdownManager`
(`src/ZoneServer/Util/ServerShutdownManager.cs`) — countdown/immediate
shutdown, broadcasts, final save.

To add a service: follow `AutoSaveService` (ctor takes dependencies +
interval, `Timer` callback wrapped in try/catch, `Dispose` stops the timer),
construct and store it on `ZoneServer` during startup.
