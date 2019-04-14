param
(
    [string]$ArtifactoryServer,
    [string]$ArtifactoryUser,
    [string]$ArtifactoryPassword,
    [string]$ArtifactoryRepository,
    [string]$DropFolder
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


# Get Branch Name from the Build Name
function Get-BranchName
{
    param
    (
        [string]$BuildName
    )

    try
    {
        $BranchName = $BuildName.split('.')[1]

        # If it's a shelveset build, add "-snapshot"
        if($BuildReason -eq "ValidateShelveset")
        {
            $BranchName+="-snapshot"
        }

        Write-Host "BranchName found {$BranchName}"
        return $BranchName
    }
    catch
    {
        Write-Host "Get-BranchName failed, Exception: $_" -ForegroundColor Red
        Exit 402
    }
}


# Upload artifact to artifactory server
function Push-Artifact
{
    param
    (
        [string]$ArtifactoryServer,
        [string]$ArtifactoryRepository,
        [string]$ArtifactPath,
        [string]$ArtifactName,
        [string]$ArtifactFile,
        $ArtifactoryCredentials
    )

    try
    {
        # Upload file to artifactory
        Write-Host "Uploading {$ArtifactFile} to {$ArtifactoryServer/$ArtifactoryRepository/$ArtifactPath}..."
        $response = Invoke-WebRequest -Uri "$ArtifactoryServer/$ArtifactoryRepository/$ArtifactPath" -Method PUT -Credential $ArtifactoryCredentials -InFile $ArtifactFile -UseBasicParsing
        return $response.Headers.Location.Replace("$ArtifactoryServer/","")
    }
    catch
    {
        Write-Host -Message "Error Uploading {$ArtifactFile} to {$ArtifactoryServer}, Exception: $_" -ForegroundColor Red
        #Exit 403
    }
}


# Upload artifact to artifactory server
function Delete-Artifact
{
    param
    (
        [string]$ArtifactoryServer,
        [string]$ArtifactPath,
        [string]$ArtifactoryApiKey
    )

    try
    {
        # Delete file in artifactory
        Write-Host "Deleting {$ArtifactoryServer/$ArtifactPath} from {$ArtifactoryServer}..."
        
        $base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}" -f $ArtifactoryApiKey)))
        $response = Invoke-WebRequest -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo)} -Uri "$ArtifactoryServer/$ArtifactPath" -Method DELETE -UseBasicParsing
        Write-Host "{$ArtifactoryServer/$ArtifactPath} Succesfully Deleted..."
        return $response
    }
    catch
    {
        Write-Host -Message "Error Uploading {$ArtifactFile} to {$ArtifactoryServer}, Exception: $_" -ForegroundColor Red
        #Exit 404
    }
}


# Upload Engine artifact to artifactory (for Setup)
function Upload-Artifacts
{
    param
    (
        [string]$Branch,
        [string]$Version,
        [string]$ArtifactoryServer,
        [string]$ArtifactoryRepository,
        [string]$ArtifactoryApiKey,
        [string]$DropFolder,
        $ArtifactoryCredentials,
        $ArtifactsPackages
    )

    
    $ArtifactsPaths = @{}

    try
    {
        $ArtifactsPackages | %{
            $Package = $_.BaseName
            $response = Push-Artifact -ArtifactoryServer $ArtifactoryServer -ArtifactoryRepository $ArtifactoryRepository -ArtifactPath "$Branch/$Package/$Package-$Version.zip" -ArtifactFile "$DropFolder/$Package.zip" -ArtifactoryCredentials $ArtifactoryCredentials
            $ArtifactsPaths.add($_,$response)
        }
    }
    catch
    {
        Write-Host -Message "Failed upload to artifactory, Exception: $_" -ForegroundColor Red
        Write-Host -Message "Rolling back uploaded files..." -ForegroundColor Yellow
        $ArtifactsPaths.Values | %{Delete-Artifact -ArtifactoryServer $ArtifactoryServer -ArtifactPath $_ -ArtifactoryApiKey $ArtifactoryApiKey}
        Exit 405
    }
}


# Get artifacts packages names from drop folder
function Get-ArtifactsPackages
{
    param
    (
        [string]$DropFolder
    )

    try
    {
        # Get artifacts packages
        $ArtifactsPackages = Get-ChildItem $DropFolder | Select BaseName

        # Show packages found
        foreach($package in $ArtifactsPackages)
        {
            $artifact = $package.BaseName
            Write-Host "Artifact package found {$artifact}"
        }

        # Return list
        return $ArtifactsPackages
    }
    catch
    {
        Write-Host -Message "Unexpected error searching for artifact packages, Exception: $_" -ForegroundColor Red
        #Exit 406
    }
}


#===================================================== SCRIPT ====================================================


# Convert credentials

Write-Host ""
Write-Host "======================================================================" -ForegroundColor Gray
Write-Host "Converting Artifactory Credentials" -ForegroundColor Gray
$ArtifactorySecurePassword = ConvertTo-SecureString $ArtifactoryPassword -AsPlainText -Force
$ArtifactoryCredentials = New-Object System.Management.Automation.PSCredential ($ArtifactoryUser, $ArtifactorySecurePassword)
Write-Host "Credentials Successfully Converted to PSCredential"


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
Write-Host "SourceDir: $SourceDir"
Write-Host "BinDir: $BinDir"
Write-Host "BuildNumber: $BuildNumber"
Write-Host "BuildName: $BuildName"
Write-Host "BuildStagingDirectory: $BuildStagingDirectory"
Write-Host "BuildUri: $BuildUri"
Write-Host "BaseChangesetId: $BaseChangesetId"


# Initialize Variables

Write-Host ""
Write-Host "======================================================================" -ForegroundColor Gray
Write-Host "Initialize Variables" -ForegroundColor Gray
$ProductVersion = Get-ProductVersion -BuildNumber $BuildNumber
$Branch = Get-BranchName -BuildName $BuildName
$ArtifactoryApiKey = "$ArtifactoryUser`:$ArtifactoryPassword"


# Get ArtifactPackages

Write-Host ""
Write-Host "======================================================================" -ForegroundColor Gray
Write-Host "Get ArtifactPackages" -ForegroundColor Gray
$ArtifactPackages = Get-ArtifactsPackages -DropFolder $DropFolder



# Upload Artifacts

Write-Host ""
Write-Host "======================================================================" -ForegroundColor Gray
Write-Host "Upload Artifacts" -ForegroundColor Gray
Upload-Artifacts -Version $ProductVersion -Branch $Branch -ArtifactoryServer $ArtifactoryServer -ArtifactoryRepository $ArtifactoryRepository -ArtifactoryApiKey $ArtifactoryApiKey -ArtifactoryCredentials $ArtifactoryCredentials -ArtifactsPackages $ArtifactPackages -DropFolder $DropFolder


#=================================================================================================================
