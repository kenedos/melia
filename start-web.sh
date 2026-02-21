#!/bin/bash
cd "$(dirname "$0")" || exit
DLL=$(find bin -name "WebServer.dll" -path "*/net8.0/*" | head -1)
[ -z "$DLL" ] && echo "ERROR: WebServer.dll not found." && exit 1
screen -dmS "Web" dotnet "$DLL"
echo "Started WebServer in screen session 'Web'."
