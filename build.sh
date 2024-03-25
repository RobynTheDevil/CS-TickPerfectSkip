#!/bin/bash
msbuild='/c/Program Files (x86)/Microsoft Visual Studio/2022/BuildTools/MsBuild/Current/Bin/MSBuild.exe'
"$msbuild" -p:Configuration=Release \
	&& cp -v bin/Release/TickPerfectSkip.dll ./mod/dll/ \
	&& rm -rf ../saves/mods/TickPerfectSkip \
	&& cp -vr ./mod ../saves/mods/TickPerfectSkip

