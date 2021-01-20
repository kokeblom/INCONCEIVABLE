param(
    [string] $nugetApiKey,
    [bool]   $nugetPublish = $false
)

Install-package BuildUtils -Confirm:$false -Scope CurrentUser -Force
Import-Module BuildUtils

$runningDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition

$nugetTempDir = "$runningDirectory/artifacts/NuGet"

if (Test-Path $nugetTempDir) 
{
    Write-host "Cleaning temporary nuget path $nugetTempDir"
    Remove-Item $nugetTempDir -Recurse -Force
}

$version = Invoke-Gitversion
$assemblyVer = $version.assemblyVersion 
$assemblyFileVersion = $version.assemblyFileVersion
$nugetPackageVersion = $version.nugetVersion
$assemblyInformationalVersion = $version.assemblyInformationalVersion

Write-host "assemblyInformationalVersion   = $assemblyInformationalVersion"
Write-host "assemblyVer                    = $assemblyVer"
Write-host "assemblyFileVersion            = $assemblyFileVersion"
Write-host "nugetPackageVersion            = $nugetPackageVersion"

# Now restore packages