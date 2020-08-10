@echo off
ECHO Enter a the file name, including extension. Valid extensions are: zip/7z
SET /P param=
@echo on
7z a %param% * -r -x!Library -x!obj -x!Temp -x!.vs -x!.git -x!Unused -x!unitytozip.bat -x!Assets\3rdParty\GameSparks\Resources\GameSparksSettings.asset
PAUSE