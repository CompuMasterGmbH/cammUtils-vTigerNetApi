::REM -UpdateNuGetExecutable not required since it's updated by VS.NET mechanisms
PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& 'VTigerApi\_CreateNewNuGetPackage\DoNotModify\New-NuGetPackage.ps1' -ProjectFilePath '.\VTigerApi\VTigerApi.csproj' -verbose -NoPrompt -DoNotUpdateNuSpecFile -PushPackageToNuGetGallery"
pause