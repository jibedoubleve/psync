
pushd

cd "\Projects\psync\src\PhotoSync.Cli"

Write-Host Release on Windows -ForegroundColor Yellow
dotnet publish -c Release -r win-x64   -o ../../Publish/Windows

#Write-Host Release on MacOS -ForegroundColor Yellow
#dotnet pulish  -c Release  -r osx-arm64 -o ../../Publish/MacOs

popd
