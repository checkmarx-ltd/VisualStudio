##-----------------------------------------------------------------------
## <copyright file="ApplyVersionToAssemblies.ps1">(c) http://TfsBuildExtensions.codeplex.com/. This source is subject to the Microsoft Permissive License. See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx. All other rights reserved.</copyright>
##-----------------------------------------------------------------------
# Look for a 0.0.0.0 pattern in the build number. 
# If found use it to version the assemblies.
#
# For example, if the 'Build number format' build process parameter 
# $(BuildDefinitionName)_$(Year:yyyy).$(Month).$(DayOfMonth)$(Rev:.r)
# then your build numbers come out like this:
# "Build HelloWorld_2013.07.19.1"
# This script would then apply version 2013.07.19.1 to your assemblies.
	
# Enable -Verbose option
[CmdletBinding()]	

# Disable parameter
# Convenience option so you can debug this script or disable it in 
# your build definition without having to remove it from
# the 'Post-build script path' build process parameter.
param([switch]$Disable)
if ($PSBoundParameters.ContainsKey('Disable'))
{
	Write-Verbose "Script disabled; no actions will be taken on the files."
}

 Write-Host Running PowerShell version $PSVersionTable.PSVersion.ToString()	

# Regular expression pattern to find the version in the build number 
# and then apply it to the assemblies
$VersionRegex = "\d+\.\d+\.\d+\.\d+"
	
# If this script is not running on a build server, remind user to 
# set environment variables so that this script can be debugged
if(-not $Env:TF_BUILD -and -not ($Env:TF_BUILD_SOURCESDIRECTORY -and $Env:TF_BUILD_BUILDNUMBER))
{
	Write-Error "You must set the following environment variables"
	Write-Error "to test this script interactively."
	Write-Host '$Env:TF_BUILD_SOURCESDIRECTORY - For example, enter something like:'
	Write-Host '$Env:TF_BUILD_SOURCESDIRECTORY = "C:\code\FabrikamTFVC\HelloWorld"'
	Write-Host '$Env:TF_BUILD_BUILDNUMBER - For example, enter something like:'
	Write-Host '$Env:TF_BUILD_BUILDNUMBER = "Build HelloWorld_0000.00.00.0"'
	exit 1
}
	
# Make sure path to source code directory is available
if (-not $Env:TF_BUILD_SOURCESDIRECTORY)
{
	Write-Error ("TF_BUILD_SOURCESDIRECTORY environment variable is missing.")
	exit 1
}
elseif (-not (Test-Path $Env:TF_BUILD_SOURCESDIRECTORY))
{
	Write-Error "TF_BUILD_SOURCESDIRECTORY does not exist: $Env:TF_BUILD_SOURCESDIRECTORY"
	exit 1
}
Write-Verbose "TF_BUILD_SOURCESDIRECTORY: $Env:TF_BUILD_SOURCESDIRECTORY"
	
# Make sure there is a build number
if (-not $Env:TF_BUILD_BUILDNUMBER)
{
	Write-Error ("TF_BUILD_BUILDNUMBER environment variable is missing.")
	exit 1
}
Write-Verbose "TF_BUILD_BUILDNUMBER: $Env:TF_BUILD_BUILDNUMBER"
	
# Get and validate the version data
$VersionData = [regex]::matches($Env:TF_BUILD_BUILDNUMBER,$VersionRegex)
switch($VersionData.Count)
{
   0		
	  { 
		 Write-Error "Could not find version number data in TF_BUILD_BUILDNUMBER."
		 exit 1
	  }
   1 {}
   default 
	  { 
		 Write-Warning "Found more than instance of version data in TF_BUILD_BUILDNUMBER." 
		 Write-Warning "Will assume first instance is version."
	  }
}
$NewVersion = $VersionData[0]
Write-Verbose "Version: $NewVersion"
	
# Remove read-only from the sources
Write-Output "$(Get-Date -format 'u') Removing read-only from '$Env:TF_BUILD_SOURCESDIRECTORY'"
attrib -r $Env:TF_BUILD_SOURCESDIRECTORY\*.* /s

# Update source.extension.vsixmanifest version number
Write-Output "|   Updating source.extension.vsixmanifest version  |"
Write-Output " --------------------------------------------------- "
$FilePath1 = $Env:TF_BUILD_SOURCESDIRECTORY + '\CxViewerVSIX\source.extension.vsixmanifest'
if($FilePath1)
{
	[xml]$XmlDocument = Get-Content -Path $FilePath1
	# Get the version number from <Version> node
	$CurrentVersionNumber = $XmlDocument.Vsix.Identifier.Version
	if($CurrentVersionNumber) 
	{
		# Change the <Version> node value and save the file
		$XmlDocument.Vsix.Identifier.Version = $NewVersion.ToString()
		$XmlDocument.Save($FilePath1)
		if(!$?) 
		{
			Write-Error "***********************************************"
            Write-Error "*                                             *"
            Write-Error "*       Release Version Was Not Updated       *"
            Write-Error "*   in source.extension.vsixmanifest version  *"
			Write-Error "*                                             *"
            Write-Error "***********************************************"
			Exit 1
		}
		Write-Host "|     File changed: source.extension.vsixmanifest      |"
	}
	else 
	{
		Write-Error "|    Cannot retrieve <Version> node from source.extension.vsixmanifest   |"
		exit 1
	}
}	
else 
{
	Write-Error "|    File not found: source.extension.vsixmanifest     |"
	exit 1
}

# Update CxViewerPackage.cs - Replace old version with new build version
Write-Output "|   Updating CxViewerPackage.cs version  |"
Write-Output " ---------------------------------------- "
$FilePath2 = $Env:TF_BUILD_SOURCESDIRECTORY + '\CxViewerVSIX\CxViewerPackage.cs'
if($FilePath2) 
{
	$FileContent = Get-Content -Path $FilePath2
	
	# Get full line content 
	$LineContentWithLineNumber = Select-String -Path $FilePath2 -Pattern "InstalledProductRegistration"
	if($LineContentWithLineNumber) 
	{
		# Remove new lines fron string
		$LineWithoutNewLines = $LineContentWithLineNumber | ForEach-Object { $_ -replace "`n", " "}
		$StringLineNumber = $LineWithoutNewLines.split(':')[2]
		# Convert string number to int 
		$IntLineNumber = [int]$StringLineNumber 
		# Change line content - replace old version with new
		$FileContent[$IntLineNumber - 1] = $FileContent | Select-String -Pattern "InstalledProductRegistration" | %{$_.line.replace($_.line.split(',')[2], '"' + $NewVersion + '"')}
		# Save changes
		$FileContent | Set-Content $FilePath2
		if(!$?) 
		{
			Write-Error "***********************************************"
            Write-Error "*                                             *"
            Write-Error "*       Release Version Was Not Updated       *"
            Write-Error "*           in CxViewerPackage.cs             *"
			Write-Error "*                                             *"
            Write-Error "***********************************************"
			Exit 1
		}
		Write-Output "|    File changed: CxViewerPackage.cs    |"
	}
	else 
	{
		Write-Error "|    Cannot retrieve [InstalledProductRegistration] element from CxViewerPackage.cs    |"
		Exit 1
	}
}
else 
{
	Write-Error "|    File not found: CxViewerPackage.cs     |"
	exit 1
}


Write-Output "$(Get-Date -format 'u') Applying version $NewVersion to assembly files."

# Apply the version to the assembly property files
$files = gci $Env:TF_BUILD_SOURCESDIRECTORY -recurse -include "*Properties*","My Project" | 
	?{ $_.PSIsContainer } | 
	foreach { gci -Path $_.FullName -Recurse -include AssemblyInfo.* }
if($files)
{
	Write-Verbose "Will apply $NewVersion to $($files.count) files."
	
	foreach ($file in $files) {
			
		if(-not $Disable)
		{
			$filecontent = Get-Content($file)
			$filecontent -replace $VersionRegex, $NewVersion | Out-File $file
			Write-Verbose "$file - version applied"
		}
	}
}
else
{
	Write-Warning "Found no files."
}




		



