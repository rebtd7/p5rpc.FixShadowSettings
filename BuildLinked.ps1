# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/p5rpc.FixShadowSettings/*" -Force -Recurse
dotnet publish "./p5rpc.FixShadowSettings.csproj" -c Release -o "$env:RELOADEDIIMODS/p5rpc.FixShadowSettings" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location