#!/bin/bash
#
# Unified server startup script
# Launches all .NET servers in their own screen sessions.
# -------------------------------------------------------------------------

# Change to the script's directory so paths resolve correctly.
cd "$(dirname "$0")" || exit

# --- Function to find and launch a server ---
# Usage: launch_server "ScreenName" "DllFileName" ["GroupID" "ServerID"]
launch_server() {
  local SCREEN_NAME="$1"
  local FILENAME="$2"
  local GROUP_ID="$3"
  local SERVER_ID="$4"
  local SUBFOLDER="net8.0"
  local BINPATH=""

  # Find the correct binary path (Release is preferred over Debug)
  if [ -f "bin/Release/$SUBFOLDER/$FILENAME" ]; then
    BINPATH="bin/Release/$SUBFOLDER"
  elif [ -f "bin/Debug/$SUBFOLDER/$FILENAME" ]; then
    BINPATH="bin/Debug/$SUBFOLDER"
  elif [ -f "bin/$SUBFOLDER/$FILENAME" ]; then
    BINPATH="bin/$SUBFOLDER"
  else
    echo "ERROR: Could not find $FILENAME in bin/Release, bin/Debug, or bin/."
    return 1
  fi

  local DLL_PATH="$BINPATH/$FILENAME"
  echo "Found '$FILENAME' at '$DLL_PATH'. Starting in screen session '$SCREEN_NAME'..."

  screen -dmS "$SCREEN_NAME" dotnet "$DLL_PATH" $GROUP_ID $SERVER_ID
}

# --- Main Execution ---
screen -wipe > /dev/null 2>&1

echo "Starting all servers..."

# BarracksServer acts as the coordinator in Melia, so start it first.
launch_server "Barracks" "BarracksServer.dll" "1001" "1"
sleep 2

launch_server "Zone-1" "ZoneServer.dll" "1001" "1"
sleep 2

launch_server "Social" "SocialServer.dll" "1001" "1"
sleep 2

launch_server "Web" "WebServer.dll"

echo "----------------------------------------------------"
echo "All server launch commands have been sent."
echo "Use 'screen -ls' to see running sessions."
echo "Use 'screen -r <SessionName>' to attach to a server."
echo "----------------------------------------------------"

exit 0
