In this game, you must find four different cubes and place them in their correct spots. Your time is recorded, so you can challenge yourself to speedrun it.

Stack:
Monogame C#

Build:

Linux:
```dotnet publish -c Release -r linux-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained```

Windows:
```dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained```

MacOS:
```dotnet publish -c Release -r osx-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained```