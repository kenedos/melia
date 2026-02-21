Melia
=============================================================================

Melia is an open-source MMORPG server, developed as a collaborative effort
of programmers from all around the world. We're aiming to create a server
software that provides a stable and extensible platform for users to build
their own projects on top of, while giving developers the opportunity to
learn and hone their skills in a familiar environment.

This project is very explicitly not about playing a game or competing
with any services provided by game developers or publishers, and we don't
endorse such actions. We're here to learn and create, not to steal or
destroy.

This branch (`laima-merge`) integrates the Laima expansion into Melia
via a package system. Laima adds hundreds of implemented skills, buffs,
instanced dungeons, a party system, player trading, and much more. All
Laima content is toggled by a single config option, so you can run
vanilla Melia or the full Laima experience.

Client
-----------------------------------------------------------------------------

Melia does not have a client of its own at this time. Instead, it's designed
to be network compatible with the latest client of the international
version of ToS, which is freely available on Steam.
The Laima package additionally supports client version 390044 via
the versioning system.

Features
-----------------------------------------------------------------------------

### Core Systems

- **Characters**: Creation, deletion, class advancement, stat distribution
- **Inventory**: Item management, equipping, unequipping
- **Chat**: In-game chat with command system
- **Stats & Properties**: Full property system with calculations
- **NPC Dialogues & Shops**: NPC interactions, shop buying/selling
- **Monster Spawns**: Monster spawning with configurable AI behaviors
- **Quests**: Quest system with objectives, rewards, and tracking
- **Skills**: Skill point distribution and combat
- **Team Storage**: Shared storage across characters on the same account
- **Goddess Bills**: Goddess bill system

### Laima Expansion Features (Package-Gated)

When enabled via `packages.conf`, the Laima package adds:

#### Combat & Skills

Laima provides a massive expansion of combat functionality with hundreds
of skill, buff, pad, and ability handlers.

**Swordsman tree** — Swordsman, Barbarian, Cataphract, Highlander,
Hoplite, Peltasta, Rodelero

**Archer tree** — Archer, Fletcher, Hunter, Quarrel Shooter, Ranger,
Sapper, Wugushi

**Cleric tree** — Cleric, Dievdirbys, Krivis, Monk, Paladin, Priest,
Sadhu

**Scout tree** — Scout, Assassin, Corsair, Linker, Outlaw,
Rogue, Thaumaturge

**Wizard tree** — Wizard, Bokor, Chronomancer, Cryomancer, Elementalist,
Psychokino, Pyromancer

**Monsters** — Extensive monster skill handlers covering boss and field
monster abilities

#### Instance Dungeons

A full stage-based dungeon framework with multiple stage types:

- **Boss Stage** — Boss encounter sequences
- **Wave Survival** — Defend against waves of enemies
- **Uphill Wave Survival** — Specialized wave defense variant
- **Timed Survival** — Survive for a set duration
- **Kill Monsters** — Clear all monsters to progress
- **Conditional Stage** — Progression gated by conditions
- **Timeline Stage** — Scripted sequential events
- **Action Stage** — Mechanics-driven encounters
- **Unlock Stage** — Progression gates
- **Timer Stage** — Timed challenge mechanics
- **Initial Setup / Background** — Stage initialization

#### Party System

Full party implementation with invites, member management, experience
sharing, and leader tracking.

#### Player Trading

Direct item exchange between players with a multi-step confirmation
state machine (Started → Confirmed → FinalConfirmed) and multi-item
support.

#### Personal Shops (Buy/Sell)

Player-run personal shops allowing characters to set up buy and sell
stalls for other players.

#### Tracks & Cinematics

Cinematic/dialogue track system supporting cable cars, elevators,
and scripted NPC sequences with frames, dialogs, and actors.

#### Minigames

- Defend the Torch
- Marble Shooter
- Whack-a-Mole

#### Monster AI

Multiple AI behavior scripts:

- Basic monster, boss, attacker, and magical caster behaviors
- Player pet AI (general, hawk variants)
- Summon AI (minions, player summons)
- Specialized dungeon AI (Uphill healer, kamikaze, ranged gimmicks)
- Track/wait and dummy behaviors

#### Content

- NPC scripts across cities and dungeons
- Monster spawn scripts for fields, dungeons, and bosses
- Warp/portal scripts connecting the world
- Item scripts for usable items and consumables
- Job advancement scripts
- Treasure spawns and map bonus drops
- Cube gacha and equipment card data

#### Services

- **Achievement Service** — Tracks kills, items, quests and awards points
- **AutoSave Service** — Distributes character saves across time slots
- **Orphan Cleanup** — Removes orphaned items from the database
- **Map Content Service** — Queries map-specific spawnable content
- **Server Shutdown Manager** — Graceful shutdown handling
- **Game Event Manager** — Infrastructure for server-wide events

### Versioning System

Supports multiple client versions through versioned op code files and
data directories. The system loads version-specific network opcodes
at runtime, enabling compatibility with different client builds.

### Package System

A 3-tier content loading architecture:

1. **system/** — Base Melia data and scripts (always loaded)
2. **packages/** — Optional content packages (loaded when enabled)
3. **user/** — Server operator customizations (highest priority)

Toggle the Laima package by editing `packages.conf`:
```
enabled_packages: laima
```

Remove the line (or leave it empty) to run vanilla Melia. The package
controls handler registration, database overlays, script loading, and
all Laima-specific content.

Architecture
-----------------------------------------------------------------------------

Melia uses a distributed multi-server architecture:

- **BarracksServer** — Authentication, character management, lobby.
  Also acts as the coordinator for inter-server communication.
- **ZoneServer** — Main gameplay server handling combat, NPCs, skills,
  dungeons, and world simulation. Multiple instances supported.
- **SocialServer** — Parties, chat, and social features.
- **WebServer** — Web-based interfaces and APIs.

Requirements
-----------------------------------------------------------------------------

- The .NET SDK (8+)
- A MySQL-compatible database server (MariaDB 10+ recommended)

On an up-to-date Windows system, the SDK will already be included,
so you only need to install a MySQL-compatible server. On Linux and
macOS, you will need to install the SDK as well.

Installation
-----------------------------------------------------------------------------

* Compile Melia
* Run `sql/main.sql` to setup the database
* Copy `system/conf/database.conf` to `user/conf/`,
  adjust the necessary values and remove the rest.

To enable the Laima expansion, set `enabled_packages: laima` in
`packages.conf`.

Afterwards, you should be able to start Melia via the provided scripts or
directly from the bin directories.

Further Reading
-----------------------------------------------------------------------------

* Check the FAQ.md for frequently asked questions about Melia.
* Check the file CONTRIBUTING.md for detailed information on how you may
  contribute.

Links
-----------------------------------------------------------------------------

* GitHub: https://github.com/NoCode-NoLife/melia
* Wiki: https://github.com/NoCode-NoLife/melia/wiki
* Forum: https://nocodenolife.org/forum/65-melia/
* Chat: https://discord.gg/5sszEgw
