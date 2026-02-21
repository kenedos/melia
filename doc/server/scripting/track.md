# R1Direction XML Format Documentation

## Overview
R1Direction XML files define track/cutscene sequences in Tree of Savior. They control camera movements, character animations, NPC spawning, visual effects, and scripted events.

## Root Element: R1Direction

### Attributes
- **MapName**: The map identifier where the track plays (e.g., "f_orchard_34_2", "ep13_f_siauliai_3")
- **FPS**: Frames per second, typically `5`
- **MaxFrame**: Total duration in frames
- **CenterPos**: Camera center position as "X#Y#Z" (e.g., "1933.90#399.69#-455.72")
- **changeableWorlds**: Comma-separated list of alternate worlds (usually empty)
- **NewLayer** (optional): Layer number for spawning
- **fadeInOut** (optional): Fade in/out flag
- **autoBornAni** (optional): Auto-play birth animation
- **WideAreaMSPD** (optional): Wide area movement speed (e.g., "250")

## Structure

```xml
<R1Direction MapName="..." FPS="5" MaxFrame="..." CenterPos="...">
    <TimeLine>
        <!-- KeyFrames for this timeline -->
    </TimeLine>
    <TimeLine>
        <!-- KeyFrames for another timeline -->
    </TimeLine>
    <!-- More TimeLine elements... -->
</R1Direction>
```

## TimeLine Element
Each TimeLine represents a separate actor or event sequence. TimeLine[0] typically controls camera and global events, while subsequent timelines control individual actors (player, NPCs, monsters, effects).

## KeyFrame Element

### Attributes
- **CMD**: Command type (see Command Types below)
- **Frame**: Frame number when this keyframe executes
- **Arg1**: First argument (usage varies by CMD)
- **Arg2**: Second argument (usage varies by CMD)
- **X, Y, Z**: 3D position coordinates
- **Script**: Additional script parameters (often empty or contains NPC properties)
- **Memo**: Comments (usually empty)

### Command Types (CMD)

#### CMD="0" - Spawn Actor
Spawns an NPC, monster, or sets player position.
- **Arg1**: Entity ID (0 = player character)
- **Arg2**: Initial rotation angle
- **X, Y, Z**: Spawn position
- **Script**: Actor properties in format `'Key' 'Value' 'Key' 'Value'`

**Script Properties:**
- `BTree`: Behavior tree (e.g., "TrackWaitMonster")
- `Faction`: Faction name (e.g., "Qeust_Team1", "Neutral")
- `SimpleAI`: AI script name
- `Name`: Localized name key
- `Lv`: Level
- `Dialog`: Dialog identifier
- `Journal`: Journal flag
- `DropItemList`: Drop table identifier

**Example:**
```xml
<KeyFrame CMD="0" Frame="0" Arg1="150243" Arg2="0" 
          X="-398.41101" Y="84.886703" Z="504.52197" 
          Script="'BTree' 'TrackWaitMonster' " Memo=""/>
```

#### CMD="1" - Move Actor
Moves actor to new position.
- **Arg1**: Movement speed/flags
- **Arg2**: Movement duration or speed multiplier
- **X, Y, Z**: Target position

#### CMD="2" - Set Direction
Sets actor rotation.
- **Arg1**: Angle in degrees
- **Arg2**: Rotation flags (usually 0)

#### CMD="4" - Set Speed/Movement Parameter
Configures movement speed or acceleration.
- **Arg1**: Speed multiplier flags
- **Arg2**: Speed value

#### CMD="6" - Script Command
Executes a script command (most common command type).
Contains a nested `<Scp>` element with the actual command.

## Scp (Script) Element

### Attributes
- **Scp**: Script command name (see Script Commands below)
- **Client**: Execution context ("YES", "NO", "BOTH")
  - "YES": Client-side only
  - "NO": Server-side only
  - "BOTH": Both client and server
- **UseSleep**: Whether to wait for completion ("YES"/"NO")
- **RunCount**: Number of times to execute (0 = once)
- **UseTooltipScp**: Tooltip flag (0 or 1)
- **CondFunc**: Conditional function (usually empty)

### Child Elements
Script commands can contain:
- **Actor**: Reference to another timeline by Line number
- **Str**: String parameter with `Str` attribute
- **Str_Arg**: String + numeric argument with `Str` and `Arg` attributes
- **Num**: Numeric parameter with `Num` attribute
- **Pos**: Position relative to actor with `Angle` and `Dist` attributes
- **AbsPos**: Absolute 3D position with `X`, `Y`, `Z` attributes
- **Angle_Abs**: Absolute angle with `Angle` attribute

## Common Script Commands

### Camera Commands
- **DRT_C_TARGET_CAM**: Set camera target to actor
- **DRT_C_CHANGE_CAM**: Change camera position/angle
- **DRT_C_CAM_ZOOM_NEW**: Zoom camera
- **DRT_C_FADEOUT_TIME**: Fade screen out
- **DRT_C_SHOCKWAVE**: Camera shake effect

### Animation Commands
- **DRT_C_PLAY_ANIM**: Play character animation
  - Parameters: animation name, loop flag, blend flag, priority

### Movement Commands
- **DRT_MOVE_BY_WMOVE**: Move actor along waypoint path
- **DRT_SETPOS**: Teleport actor to absolute position
- **DRT_JUMP_TO_POS**: Jump to position with arc
- **DRT_LOOKAT_C**: Make actor look at target
- **DRT_KNOCKB_RUN_C**: Knockback effect

### Effect Commands
- **DRT_ACTOR_PLAY_EFT**: Play one-time effect
- **DRT_ACTOR_ATTACH_EFFECT**: Attach looping effect
- **DRT_C_REMOVE_EFT**: Remove effect
- **DRT_C_CLEAR_EFT**: Clear all effects
- **DRT_ACTOR_NODE_PLAY_EFT_C**: Play effect on specific bone
- **DRT_PLAY_EFT_C**: Play effect at relative position

### Dialog Commands
- **DRT_OK_DLG**: Show dialog message (waits for OK)
- **DRT_BALL_DLG**: Show balloon/notification message

### Combat Commands
- **InsertHate**: Add threat/aggro to mob
- **DRT_ALPHA_NPC**: Change actor transparency
- **DRT_CHANGE_COLOR_OPT**: Change actor color

### System Commands
- **DRT_KILL**: Remove actor from track
- **TRACK_BASICLAYER_MOVE**: End track and return to normal layer
- **SetLayer**: Change layer (0 = exit track)
- **StopPCDirection**: Stop cutscene camera control
- **DRT_PLAY_MGAME**: Start minigame
- **CREATE_BATTLE_BOX_INLAYER**: Create battle boundary
- **TRACK_SETTENDENCY**: Set track tendency flags
- **DRT_SET_PC_SSN**: Set session object
- **SCR_DIRECTOR_NOTICE**: Show addon message

### Chat Commands
- **DRT_CHAT_RUN_C**: Show chat bubble

## Height Offset Enum
Used in effect and position commands:
- **TOP**: Above actor
- **MID**: At actor center
- **BOT**: Below actor/ground level

## Example Timeline Breakdown

### Timeline 0 (Camera Control)
```xml
<TimeLine>
    <!-- Frame 0: Target camera on player -->
    <KeyFrame CMD="6" Frame="0" ...>
        <Scp Scp="DRT_C_TARGET_CAM" Client="YES" ...>
            <Actor Line="1"/>
        </Scp>
    </KeyFrame>
    
    <!-- Frame 37: Switch to NPC, zoom in -->
    <KeyFrame CMD="6" Frame="37" ...>
        <Scp Scp="DRT_C_TARGET_CAM" Client="YES" ...>
            <Actor Line="3"/>
        </Scp>
    </KeyFrame>
    
    <!-- Frame 54: End track -->
    <KeyFrame CMD="6" Frame="54" ...>
        <Scp Scp="TRACK_BASICLAYER_MOVE" Client="NO" .../>
    </KeyFrame>
</TimeLine>
```

### Timeline 2 (Player Character)
```xml
<TimeLine>
    <!-- Spawn player at position -->
    <KeyFrame CMD="0" Frame="0" Arg1="0" Arg2="0" 
              X="-2.27" Y="122.49" Z="82.61" Script="" Memo=""/>
    
    <!-- Set initial direction -->
    <KeyFrame CMD="2" Frame="0" Arg1="137.67754" Arg2="0" .../>
</TimeLine>
```

### Timeline 3+ (NPCs/Monsters)
```xml
<TimeLine>
    <!-- Spawn NPC -->
    <KeyFrame CMD="0" Frame="0" Arg1="150243" Arg2="0" 
              X="..." Y="..." Z="..." 
              Script="'BTree' 'TrackWaitMonster' " Memo=""/>
    
    <!-- Play animation -->
    <KeyFrame CMD="6" Frame="0" ...>
        <Scp Scp="DRT_C_PLAY_ANIM" Client="YES" ...>
            <Str Str="std3"/>
            <Num Num="1"/>
            <Num Num="1"/>
            <Num Num="0"/>
        </Scp>
    </KeyFrame>
    
    <!-- Remove at end -->
    <KeyFrame CMD="6" Frame="54" ...>
        <Scp Scp="DRT_KILL" Client="BOTH" .../>
    </KeyFrame>
</TimeLine>
```

## Actor Line References
When `<Actor Line="N"/>` is used, it refers to TimeLine[N] (0-indexed):
- Line="1" → TimeLine[1] (usually the player)
- Line="3" → TimeLine[3] (first NPC)
- Line="4" → TimeLine[4] (second NPC)

## Parsing Considerations

1. **XML Encoding Issues**: Files may have malformed attributes (missing spaces). The XMLUtil class handles this with string replacements.

2. **Localization**: String values in `Script` and `Str` attributes often reference localization keys (e.g., "EP13_F_SIAULIAI_3_MQ_07_DLG_T1").

3. **Frame Timing**: At FPS=5, each frame = 0.2 seconds. Frame 50 = 10 seconds.

4. **Coordinate System**: Y-axis is vertical. Negative Y values below -200 are typically adjusted to 0.

5. **Empty Scripts**: Many KeyFrames have empty Script="" attributes, which is normal.

6. **UnvisibleName**: Special name value used for invisible effect actors.

## Translation Notes

When converting to server scripts:
- CMD="0" with Arg1=0 → Player spawn/movement
- CMD="0" with Arg1>0 → NPC/Monster spawn
- CMD="6" → Script command execution
- Track.Actors array stores spawned actors by timeline index
- Dialog commands should await user input
- Effect commands are often client-side only