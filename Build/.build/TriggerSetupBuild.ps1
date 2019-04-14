param
(
    [string]$TeamProject,
    [string]$Credentials,
    [string]$NextBuild,
    [string]$NextSnapshotBuild
)

#=================================================== FUNCTIONS ===================================================


# Get Version using regular expression from build run name
function Get-ProductVersion
{
    param
    (
        [string]$BuildNumber
    )

    try
    {
        $VersionRegex = "\d+\.\d+\.\d+\.\d+"
        $Version = ([regex]::matches($BuildNumber,$VersionRegex))[0].Value
        Write-Host "Product version found {$Version}"
        return $Version
    }
    catch
    {
        Write-Host "Get-ProductVersion failed, Exception: $_" -ForegroundColor Red
        Exit 401
    }
}


#===================================================== SCRIPT ====================================================


# Get Build Environment Variables

Write-Host ""
Write-Host "======================================================================" -ForegroundColor Gray
Write-Host "Loading Build Environment Variables" -ForegroundColor Gray
$SourceDir = $Env:BUILD_SOURCESDIRECTORY
$BinDir = $Env:BUILD_BINARIESDIRECTORY
$BuildNumber = $Env:BUILD_BUILDNUMBER
$BuildName = $Env:BUILD_DEFINITIONNAME
$BuildStagingDirectory = $Env:BUILD_STAGINGDIRECTORY
$BuildUri = $Env:BUILD_BUILDURI
$BaseChangesetId = $Env:BUILD_SOURCEVERSION
$CollectionUrl = $Env:SYSTEM_TEAMFOUNDATIONCOLLECTIONURI
Write-Host "SourceDir: $SourceDir"
Write-Host "BinDir: $BinDir"
Write-Host "BuildNumber: $BuildNumber"
Write-Host "BuildName: $BuildName"
Write-Host "BuildStagingDirectory: $BuildStagingDirectory"
Write-Host "BuildUri: $BuildUri"
Write-Host "BaseChangesetId: $BaseChangesetId"
Write-Host "CollectionUrl: $CollectionUrl"


# Import Checkmarx PowerShell Module

Write-Host ""
Write-Host "======================================================================" -ForegroundColor Gray
Write-Host "Importing Checkmarx Powershell Module..." -ForegroundColor Gray
Import-Module "$SourceDir\Build\CommonScripts\CheckmarxModule-v1.psm1" -Force
Write-Host "Module successfully imported"


# Get Current Build Info

$BuildDefinitionId = Get-BuildDefinitionId -BuildDefinitionName $BuildName -CollectionUri $CollectionUrl -TeamProject $TeamProject -Credentials $Credentials
$BuildId = Get-BuildIdFromBuildNumber -BuildDefinitionId $BuildDefinitionId -BuildNumber $BuildNumber -CollectionUri $CollectionUrl -TeamProject $TeamProject -Credentials $Credentials
$BuildReason = Get-BuildReason -BuildId $BuildId -CollectionUri $CollectionUrl -TeamProject $TeamProject -Credentials $Credentials


# Trigger Next Build

if($NextBuild)
{
    # Get branch name from BuildName
    $Branch = $BuildName.split('.')[1]
    

    # Define build to trigger (snapshot if it's a shelveset build) 
    Write-Host "Build Reason {$BuildReason}"
    if($BuildReason -eq "manual")
    {
        $BuildToTrigger = $NextSnapshotBuild
		$BuildParameters = @{EngineVersion = Get-ProductVersion -BuildNumber $BuildNumber}
    }
    else
    {
        $BuildToTrigger = $NextBuild
		$BuildParameters = @{}
    }


    # Register and trigger next build 
    $NextBuildDefinitionId = Get-BuildDefinitionId -BuildDefinitionName $BuildToTrigger -CollectionUri $CollectionUrl -TeamProject $TeamProject -Credentials $Credentials
    $TriggerResponse = Trigger-Build -BuildDefinitionId $NextBuildDefinitionId -Parameters $BuildParameters -CollectionUri $CollectionUrl -TeamProject $TeamProject -Credentials $Credentials
    Register-Build -TriggerRequestResponse $TriggerResponse -OriginBuildDefinition $BuildName -OriginBuildNumber $BuildNumber -BuildMappingsFile "\\storage\devops\Build\BuildMappings\BuildMappings.xml" -CollectionUri $CollectionUrl -TeamProject $TeamProject -Credentials $Credentials
    
}

#=================================================================================================================