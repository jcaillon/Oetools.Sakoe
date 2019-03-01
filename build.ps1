param ( 
	[string]
	$ProjectOrSolutionPath = "Oetools.Sakoe/Oetools.Sakoe.csproj",
	[bool]
	$TestOnly = $False,
	[bool]
	$UpdateDocumentationOnly = $False,
	[bool]
	$ShouldExit = $False
)

function Main {
    # inspired by ci script from https://github.com/datalust/piggy.
	# inspired by ci script from https://github.com/Azure/azure-functions-core-tools.
    $path = $ProjectOrSolutionPath	
	[string] $ciTag = If ([string]::IsNullOrEmpty($env:CI_COMMIT_TAG)) {$env:CI_COMMIT_TAG} Else {$env:APPVEYOR_REPO_TAG_NAME}
    $isReleaseBuild = [string]::IsNullOrEmpty($ciTag)
    [string] $versionToBuild = $NULL
    if ($isReleaseBuild) {
        $versionToBuild = If ($ciTag.StartsWith('v')) {$ciTag.SubString(1)} Else {$env:ciTag}
    }

	Write-Host "Building $(If ([string]::IsNullOrEmpty($versionToBuild)) { "default version" } Else { " version $versionToBuild" } )"

	Push-Location $PSScriptRoot
	if ($UpdateDocumentationOnly) {
		Update-Documentation
	} elseif ($TestOnly) {
		Start-Tests -AllTests $True
	} else {
		Invoke-Script -ScriptWithParameters ".\Oetools.Builder\build.ps1 -UpdateXsdOnly 1"
		Invoke-Script -ScriptWithParameters ".\GithubUpdater\build.ps1 -BuildSimpleUpdaterOnly 1"
		Start-Tests
		New-ArtifactDir
		Publish-DotNetCore -Path "$path" -Version "$versionToBuild"	
        Publish-DotNetFramework -Path "$path" -Version "$versionToBuild"
	}
    Pop-Location
}

function New-ArtifactDir {
    if (Test-Path "./artifacts") { 
        Remove-Item -Path "./artifacts" -Force -Recurse 
    }
    New-Item -Path "." -Name "artifacts" -ItemType "directory" | Out-Null
}

function Update-Documentation {
	Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"
	Write-Host "Updating markdown documentation"
	Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"

    if (-Not (Test-Exe("msbuild"))) {
        Throw "The executable msbuild was not found in your path."
	}

	$publishDir = $(Join-Path -Path $(Resolve-Path -Path "./artifacts") -ChildPath "sakoe")
	
	iu msbuild "$path" "/verbosity:minimal" "/t:Restore,Publish" "/p:Configuration=Release" "/p:TargetFramework=net461" "/bl:net461.binlog" "/p:PublishDir=$publishDir" $(If ([string]::IsNullOrEmpty($Version)) { "" } Else { "/p:Version=$Version" })
	
	iu "$(Join-Path -Path "$publishDir" -ChildPath "sakoe.exe")" manual markdown-file docs\index.md

	Remove-Item -Path "$publishDir" -Force -Recurse 
}

function Start-Tests {
	param ( 
		[bool]
		$AllTests = $False
	)
	if (-Not (Test-Exe("dotnet"))) {
        Throw "The executable dotnet was not found in your path."
	}

	if ($AllTests) {
		foreach ($file in Get-ChildItem -Path . -Recurse | Where-Object {$_.Name -like "*.Test.csproj"}) {
			Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"
			Write-Host "Testing assembly $($file.Name)"
			Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"
			
			iu dotnet test "$($file.FullName)" --verbosity "minimal"
		}
	} else {
		Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"
		Write-Host "Testing assembly"
		Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"	
		iu dotnet test "Oetools.Sakoe.Test/Oetools.Sakoe.Test.csproj" --verbosity "minimal"
	}

}

function Publish-DotNetFramework {
	param (
		$Path, 
		$Version
	)
    if (-Not (Test-Exe("msbuild"))) {
        Throw "The executable msbuild was not found in your path."
    }

	# msbuild binlog viewer: http://msbuildlog.com/
	
	$artifactDir = Resolve-Path -Path "./artifacts"
	$publishDir = $(Join-Path -Path "$artifactDir" -ChildPath "sakoe")
	
    $platforms = @("x64", "x86") # "Any CPU", "x86", "x64"
    foreach ($platform in $platforms) {
		$frameworks = @("net461")
		foreach ($framework in $frameworks) {
			Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"
			Write-Host "Building $framework $platform"
			Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"

			iu msbuild "$path" "/verbosity:minimal" "/t:Restore,Publish" "/p:Configuration=Release" "/p:TargetFramework=$framework" "/bl:$framework.binlog" "/p:PublishDir=$publishDir" "/p:Platform=$platform" $(If ([string]::IsNullOrEmpty($Version)) { "" } Else { "/p:Version=$Version" })

			$archiveName = "sakoe.win-$platform"

			Push-Location "$artifactDir"
			iu 7z a "$archiveName.zip" "sakoe\sakoe.exe"
			iu 7z a "$archiveName.zip" "sakoe\sakoe.pdb"
			iu 7z a "$archiveName.zip" "sakoe\sakoe.exe.config"
			Move-Item -Path "sakoe" -Destination "$archiveName"
			Pop-Location
		}
	}
}

function Publish-DotNetCore {
	param (
		$Path, 
		$Version
	)

	if (-Not (Test-Exe("dotnet"))) {
        Throw "The executable dotnet was not found in your path."
    }

    if (Test-Exe("nuget")) {
        iu nuget restore $path -Recursive -NonInteractive
    } else {
        iu dotnet restore $path
    }

	$artifactDir = Resolve-Path -Path "./artifacts"
	$publishDir = $(Join-Path -Path "$artifactDir" -ChildPath "sakoe")

	iu dotnet restore $path


	$rids = @("linux-x64", "")
	foreach ($rid in $rids) {
		Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"
		Write-Host "Building $rid $platform"
		Write-Host "@@@@@@@@@@@@@@@@@@@@@@@"

		$selfContainedSuffix = If ([string]::IsNullOrEmpty($rid)) { ".no-runtime" } Else { ".$rid" }

        if ([string]::IsNullOrEmpty($rid)) {
            iu dotnet publish "$path" --framework "netcoreapp2.2" --configuration "Release" --verbosity "minimal" "/p:TrimUnusedDependencies=true" "/p:ShowLinkerSizeComparison=true" "/p:PublishDir=$publishDir" $(If ([string]::IsNullOrEmpty($Version)) { "" } Else { "/p:Version=$Version" })
        } else {
            iu dotnet publish "$path" --framework "netcoreapp2.2" --configuration "Release" --verbosity "minimal" --runtime "$rid" --self-contained "true" "/p:UseAppHost=true" "/p:TrimUnusedDependencies=true" "/p:ShowLinkerSizeComparison=true" "/p:PublishDir=$publishDir" $(If ([string]::IsNullOrEmpty($Version)) { "" } Else { "/p:Version=$Version" })
        }		

		$archiveName = "sakoe.core$selfContainedSuffix"

		Push-Location "$artifactDir"
		iu tar -zcvf "$archiveName.tar.gz" "sakoe/*"
		Move-Item -Path "sakoe" -Destination "$archiveName"
		Pop-Location
	}
}

function Test-Exe($exeName) {
    return [bool](Get-Command $exeName -ErrorAction SilentlyContinue);
}

function Invoke-Script {
    param (
        [Parameter(Mandatory = $true)]
        [string]
        $ScriptWithParameters
    )
    $ErrorActionPreference = 'Stop' # in case script isn't found
	$ScriptBlock = [ScriptBlock]::Create("$ScriptWithParameters")
	try {
		Invoke-Command -ScriptBlock $ScriptBlock
	} catch {
		$exceptionCatched = $_.Exception.ToString()
		Throw "sub script failure (full command: $ScriptWithParameters): $exceptionCatched" 
	}
}

function Invoke-Utility {
    <#
.SYNOPSIS
Invokes an external utility, ensuring successful execution.

.DESCRIPTION
Invokes an external utility (program) and, if the utility indicates failure by 
way of a nonzero exit code, throws a script-terminating error.

* Pass the command the way you would execute the command directly.
* Do NOT use & as the first argument if the executable name is not a literal.

.EXAMPLE
Invoke-Utility git push

Executes `git push` and throws a script-terminating error if the exit code
is nonzero.
#>
    $exe, $argsForExe = $Args
    $ErrorActionPreference = 'Stop' # in case $exe isn't found
    & $exe $argsForExe
    if ($LASTEXITCODE) { 
        Throw "$exe indicated failure (exit code $LASTEXITCODE; full command: $Args)." 
    }
}

[int] $exitcode = 0

try {
	Set-Alias iu Invoke-Utility
    Main
} catch {
	if (-Not $ShouldExit) {
		throw $_.Exception
	}
    $exceptionCatched = $_.Exception.ToString()
	Write-Host "Exception : $exceptionCatched"
	$exitcode = 1	
}

if ($ShouldExit) {
	Write-Host "Exit code $exitcode"
	$host.SetShouldExit($exitcode)
	exit
}