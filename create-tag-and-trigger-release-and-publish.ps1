param(
    [switch]$NonInteractive,
    [string]$TagAsVersion,
    $TagMessage = $null
)

Add-Type -AssemblyName System.Web

## Constants of project
[string]$githubProjectUrl = 'https://github.com/CompuMasterGmbH/cammUtils-vTigerNetApi/' #must contain trailing slash!
[string]$masterBranchName = "master" #ususally 'master' or 'main'
[string]$assemblyVersionSourceFilePath = ".\VTigerApi\VTigerApi.csproj"
[string]$assemblyVersionSourceVersionPattern = '^.*\<Version\>(.*)\<\/Version\>.*$'

## Check for master branch
$branch = git branch
if ($branch -ne ('* ' + $masterBranchName))
{
    Throw "Branch must be set to master before"
}
""

## Check for missing commits
[string]$changes = git status -s
if($LASTEXITCODE -ne 0)
{
    Throw "Some git commits are missing: git status failed"
}
if($changes -ne "" -and $changes -ne $null)
{
    "Missing commits for:"
    $changes
    Throw "Some git commits are missing: there are file modifications present"
}
""

## Refresh local tags list
[string]$changes = git fetch --tags --all --prune --prune-tags --force
if($LASTEXITCODE -ne 0)
{
    Throw "git pull failed or incomplete"
}
if($changes -ne "" -and $changes -ne $null)
{
    "Local tags refreshing:"
    $changes
}
""

## Check for missing pulls
[string]$changes = git pull
if($LASTEXITCODE -ne 0)
{
    Throw "git pull failed or incomplete"
}
if($changes -ne "" -and $changes -ne $null)
{
    "git pull actions:"
    $changes
}
""

## Show existing list of tags
"Existing tags:"
git tag -n5 #max 5 lines per message
""

## Read version default from source file
[string]$versionInput = $TagAsVersion
if ($versionInput -eq "" -and $assemblyVersionSourceFilePath -ne $null -and $assemblyVersionSourceFilePath -ne "" -and (Test-Path -Path $assemblyVersionSourceFilePath))
{
    Write-Host "Probing $assemblyVersionSourceFilePath for version no. with $assemblyVersionSourceVersionPattern"
    [string]$assemblyVersionSource = Get-Content -Path $assemblyVersionSourceFilePath
    $found = $assemblyVersionSource -match $assemblyVersionSourceVersionPattern
    if ($found) 
    {
        $versionFromSourceFile = $matches[1]
    }
}

## Read version from user input
if ($NonInteractive -eq $false)
{
    if ($versionInput -eq $null -or $versionInput -eq "")
    {
        if ($versionFromSourceFile -ne "")
        {
            $versionInput = Read-Host "Please enter a version no. for the new git tag [$versionFromSourceFile]"
            if ($versionInput -eq '')
            {
                $versionInput = $versionFromSourceFile
            }
        }
        else
        {
            $versionInput = Read-Host "Please enter a version no. for the new git tag"
        }
    }
}
if ($versionInput -eq '')
{
    Throw "Version no. for tag must be defined"
}
elseif ([Version]::TryParse($versionInput, [ref]$null) -eq $false)
{
    Throw "Invald version no."
}
[string]$tagName = "v" + $versionInput

## Read tag message from user input
[string]$tagBody = $TagMessage
if ($NonInteractive -eq $false)
{
    if ($null -eq $TagMessage)
    {
        $tagBody = Read-Host "Please enter a description/message/body for the new git tag (empty = auto from commits, line breaks with <Shift>+<Enter>)"
    }
}

## Start creation of tag
"Creating tag ""$tagName"" . . ."
$tagBody
""
if ($tagBody -eq "")
{
    #$lastCommit = git log -1 --format=%H
    #if($LASTEXITCODE -ne 0)
    #{
    #    Throw "Git last commit detection failed"
    #}
    #git tag -f -a "$tagName" $lastCommit
    $lastCommitMessage = git log -1 --format=%s
    if($LASTEXITCODE -ne 0)
    {
        Throw "Git last commit detection failed"
    }
    git tag -f -a "$tagName" -m "$lastCommitMessage"
}
else
{
    git tag -f -a "$tagName" -m "$tagBody"
}
if($LASTEXITCODE -ne 0)
{
    Throw "Git tagging failed"
}
""

## Pushing commits to remote
[string]$changes = git push origin
if($LASTEXITCODE -ne 0)
{
    Throw "Git push (commits) failed"
}
""

## Pushing tag to remote
"Pushing tag ""$tagName"" to remote . . ."
[string]$changes = git push origin "$tagName"
if($LASTEXITCODE -ne 0)
{
    Throw "Git push (tag) failed"
}
""

## Open web browser for drafting release
[Diagnostics.Process]::Start($githubProjectUrl + 'releases/new?tag=' + [System.Web.HttpUtility]::UrlEncode($tagName) + '&body=' + [System.Web.HttpUtility]::UrlEncode($tagBody)) | Out-Null

## Final note
"GitHub release created successfully (usually even if previous git push actions showed up with errors)"