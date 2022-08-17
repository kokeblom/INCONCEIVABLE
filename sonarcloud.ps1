
param(
    [string] $sonarSecret
)


Install-package BuildUtils -Confirm:$false -Scope CurrentUser -Force
Import-Module BuildUtils
