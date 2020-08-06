param(
    [string] $nugetApiKey,
    [bool]   $nugetPublish = $false
)

Install-package BuildUtils -Confirm:$false -Scope CurrentU