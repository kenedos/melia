#!/bin/bash
cd "$(dirname "$0")" || exit
DLL=$(find bin -name "ZoneServer.dll" -path "*/net8.0/*" | head -1)
[ -z "$DLL" ] && echo "ERROR: ZoneServer.dll not found." && exit 1
screen -dmS "Zone-2" dotnet "$DLL" 1001 2
echo "Started ZoneServer (Zone-2) in screen session 'Zone-2'."
