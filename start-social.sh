#!/bin/bash
cd "$(dirname "$0")" || exit
DLL=$(find bin -name "SocialServer.dll" -path "*/net8.0/*" | head -1)
[ -z "$DLL" ] && echo "ERROR: SocialServer.dll not found." && exit 1
screen -dmS "Social" dotnet "$DLL" 1001 1
echo "Started SocialServer in screen session 'Social'."
