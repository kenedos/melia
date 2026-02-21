# MiniGame XML Format Documentation

## Overview
MiniGame XML files define the logic for Dungeons, Raids, Missions, and Minigames in Tree of Savior. Unlike `Track` files (which are linear cinematic sequences), MiniGame files function as state machines that manage stages, monster spawning, objective tracking, and failure conditions.

## Root Element: GameList

The root container is `<GameList>`, which typically contains a single `<Game>` element.

### Game Element Attributes
- **Name**: Internal identifier for the mission (e.g., "UNIQUE_RAID_PAST_FANTASY_LIBRARY_SOLO").
- **mapName**: The map identifier where the mission takes place.
- **minLv / maxLv**: Level range requirements.
- **playerCountCheckDestroy**: Flag (1 or 0) to destroy the instance if player count drops to zero.
- **startWaitSec**: Delay in seconds before the mission logic begins.
- **rankScript**: (Optional) Script used for ranking logic.

## Structure

```xml
<GameList>
    <Game Name="..." mapName="...">
        <StageList>
            <!-- Stages define the state machine flow -->
            <Stage Name="Intro"> ... </Stage>
            <Stage Name="1ST"> ... </Stage>
            <Stage Name="Success"> ... </Stage>
            <Stage Name="Fail"> ... </Stage>
        </StageList>
        <EnterList>
            <!-- Scripts to run upon player entry -->
        </EnterList>
    </Game>
</GameList>
```

## Stage Element
A **Stage** represents a specific phase of the dungeon (e.g., "Defend the crystal", "Kill the Boss", "Fail State"). Stages can run concurrently if triggered.

### Attributes
- **Name**: Unique identifier for the stage (referenced in scripts).
- **AutoStart**: `1` to start immediately when the instance loads, `0` to wait for a script command.

### Child Elements
- **ObjList**: Definitions of monsters, NPCs, and objects to spawn in this stage.
- **StageEvents**: Logic triggers (Conditions -> Executions) active during this stage.
- **StartScpList**: Scripts that execute exactly once when the stage activates.

## Obj (Object) Element
Defines entities (Monsters, NPCs, Traps) belonging to a specific stage.

### Attributes
- **Type**: Usually "Monster".
- **MonType**: The ID of the monster/NPC (references `monsters.xml`).
- **Pos**: Spawn coordinates in "X#Y#Z" format.
- **angle**: Initial rotation (degrees).
- **genCount**: How many to spawn.
- **genTime**: Time (ms) between respawns (if logic allows).
- **objectKey**: **Crucial**. A unique numeric ID within this Stage used to reference this object in scripts (e.g., `List="StageName/ObjectKey"`).
- **autoGen**: `0` usually means the monster waits for a specific `MGAME_EVT_EXEC_CREMON` command. `1` (or omitted) spawns immediately with stage start.
- **propList**: A string containing key-value pairs for properties.
  - Example: `'HpCount' '4' 'Name' 'UnvisibleName' 'Faction' 'Monster'`

### SelfAI Element
Objects can have embedded AI logic without needing external scripts.
- **StartList**: Scripts run on spawn.
- **DeadList**: Scripts run on death (often used to update counters or variables).
- **List**: Periodic update scripts.

## StageEvents (Logic Engine)
This is the core logic. An event consists of **Conditions** (`condList`) and **Executions** (`execList`).

```xml
<Event eventName="CLEAR" execCount="1">
    <condList>
        <!-- All conditions must be true -->
    </condList>
    <execList>
        <!-- Execute these scripts -->
    </execList>
</Event>
```

### Attributes
- **eventName**: Name for debugging/logging.
- **execCount**: How many times this event can trigger. `1` = once per stage activation. `0` = infinite (loop).
- **execWithStart**: If `1`, checks conditions immediately when stage starts.
- **checkSec**: Interval in seconds to check conditions.

## ToolScp (Script) Element
Used in `condList`, `execList`, and `SelfAI`.

### Attributes
- **Scp**: The function name (Lua script).
- **Client**: Execution context (`YES` = Client, `NO` = Server).
- **UseSleep**: `YES` to yield execution until finished.

### Arguments
Scripts take arguments via child elements:
- **Str**: String argument.
- **Num**: Numeric argument.
- **MGameObj / MGameObjIndex**: References objects.
    - **Syntax**: `List="StageName/ObjectKey"`
    - Example: `List="5TH/9"` refers to the object with `objectKey="9"` inside `Stage Name="5TH"`.
    - Multiple objects: `List="1ST/1/1ST/2"`

## Common Script Commands

### Conditions (`condList`)
| Command | Description |
| :--- | :--- |
| `GAME_ST_EVT_COND_TIMECHECK` | Checks if `Num` seconds have passed since stage start. |
| `MGAME_EVT_COND_MONCNT` | Checks if count of specific objects equals `Num`. |
| `MGAME_EVT_COND_MONCNT_OVER` | Checks if count of specific objects is > `Num`. |
| `GAME_ST_EVT_COND_VALUE` | Checks internal variable (Str) against a value (Num). <br>Arg 2 is operator: `==`, `OVER`, `UNDER`. |
| `MGAME_EVT_COND_MONHP` | Checks HP percentage of target object. |

### Executions (`execList`)
| Command | Description |
| :--- | :--- |
| `GAME_ST_EVT_EXEC_STAGE_START` | Activates a new stage by Name. |
| `GAME_ST_EVT_EXEC_STAGE_CLEAR` | Marks a stage as "Cleared" (logic flag). |
| `GAME_ST_EVT_EXEC_STAGE_DESTROY` | Removes all objects associated with the stage. |
| `GAME_ST_EVT_EXEC_STAGE_DISABLE` | Stops checking events for this stage. |
| `MGAME_EVT_EXEC_CREMON` | Spawns objects defined in `ObjList`. <br>Arg 1: `MGameObjIndex` list. <br>Arg 2: Count. |
| `MGAME_EVT_EXEC_DELMON` | Despawns/Removes specific objects. |
| `MGAME_MSG_ICON` | Shows an on-screen notification/warning with an icon. |
| `GAME_ST_EVT_EXEC_VALUE` | Sets an internal variable (`Str` key, `Num` value). |
| `MGAME_SET_QUEST_NAME` | Updates the UI objective text. |
| `MGAME_RETURN` | Returns players to their previous location (end of raid). |

## Example: Boss Kill Event
This snippet defines a stage that waits for a boss to die, then triggers the "Success" stage.

```xml
<Stage Name="BossFight">
    <ObjList>
        <!-- Boss Object, Key 0 -->
        <Obj Type="Monster" MonType="58523" objectKey="0" ... />
    </ObjList>
    <StageEvents>
        <Event eventName="CheckBossDead" execCount="1">
            <condList>
                <!-- Condition: Monster Count of Boss is 0 -->
                <ToolScp Scp="MGAME_EVT_COND_MONCNT" Client="NO">
                    <MGameObj List="BossFight/0"/>
                    <Num Num="0"/>
                </ToolScp>
            </condList>
            <execList>
                <!-- Start Success Stage -->
                <ToolScp Scp="GAME_ST_EVT_EXEC_STAGE_START" Client="NO">
                    <Str Str="Success"/>
                </ToolScp>
                <!-- Destroy current stage -->
                <ToolScp Scp="GAME_ST_EVT_EXEC_STAGE_DESTROY" Client="NO">
                    <Str Str="BossFight"/>
                </ToolScp>
            </execList>
        </Event>
    </StageEvents>
</Stage>
```

## Parsing Notes
1.  **PropList Parsing**: The `propList` attribute in `Obj` uses a specific format: `'Key' 'Value' 'Key' 'Value'`. This requires a custom parser to separate keys from values (often mapped to properties like Faction, Name, Dialog, SimpleAI).
2.  **References**: Unlike Track files which use timeline indexes, MiniGames use the `StageName/ObjectKey` string path to link scripts to specific entity definitions.
3.  **Coordinates**: `Pos` attribute uses `#` as a separator (`X#Y#Z`), while `AbsPos` elements use individual attributes.
4.  **Wait Logic**: `startWaitSec="-1"` in the root element usually implies waiting for a client packet or interaction to begin the session.