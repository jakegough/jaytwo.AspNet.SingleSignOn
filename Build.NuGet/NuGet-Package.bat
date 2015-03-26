call NuGet-Settings.cmd

"%MSBUILD_EXE%" %NUGET_PROJECT% /t:Clean
"%NUGET_EXE%" pack "%NUGET_PROJECT%" -Verbosity detailed -Build -Properties Configuration="Release"

pause