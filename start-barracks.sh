#!/bin/bash
cd "$(dirname "$0")" || exit
DLL=$(find bin -name "BarracksServer.dll" -path "*/net8.0/*" | head -1)
[ -z "$DLL" ] && echo "ERROR: BarracksServer.dll not found." && exit 1
screen -dmS "Barracks" dotnet "$DLL" 1001 1
echo "Started BarracksServer in screen session 'Barracks'."
