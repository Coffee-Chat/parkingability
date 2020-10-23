# Require Admin

function Test-IsRunningAsAdmin 
{
    $currentPrincipal = New-Object Security.Principal.WindowsPrincipal( [Security.Principal.WindowsIdentity]::GetCurrent() )

    if ($currentPrincipal.IsInRole( [Security.Principal.WindowsBuiltInRole]::Administrator ))
    {
        return $true
    }
    return $false
}

function New-RandomPassword {
    Param(
        [ValidateRange(4,79)]
        [int]    $Length = 16,
        [switch] $ExcludeSpecialCharacters
    )
    $SpecialCharacters = @((33,35) + (36..38) + (42..44) + (60..64) + (91..94))
    try {
        if (-not $ExcludeSpecialCharacters) {
                $Password = -join ((48..57) + (65..90) + (97..122) + $SpecialCharacters | Get-Random -Count $Length | foreach {[char]$_})
            } else {
                $Password = -join ((48..57) + (65..90) + (97..122) | Get-Random -Count $Length | foreach {[char]$_})  
        }

    } catch {
        throw $_.Exception.Message
    }

    return $Password;
}

$IsAdmin = Test-IsRunningAsAdmin;
if ($IsAdmin -eq $false) 
{ 
    throw "Setup requires PowerShell to be running as Administrator." 
}

# Install DotNetCli https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script
# Run a separate PowerShell process because the script calls exit, so it will end the current PowerShell session.

&powershell -NoProfile -ExecutionPolicy unrestricted -Command `
"&([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -Architecture x64 -Channel Current -Runtime aspnetcore"

# Install Azure CLI (or update) https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli

&powershell -NoProfile -ExecutionPolicy unrestricted -Command `
"Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi"

# Install VSCode https://github.com/PowerShell/vscode-powershell

&powershell -NoProfile -ExecutionPolicy unrestricted -Command `
"&([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://raw.githubusercontent.com/PowerShell/vscode-powershell/master/scripts/Install-VSCode.ps1'))) -AdditionalExtensions ms-dotnettools.csharp,docsmsft.docs-authoring-pack,dbaeumer.vscode-eslint,eamodio.gitlens,ms-vscode.vscode-typescript-tslint-plugin,visualstudioexptteam.vscodeintellicode,redhat.vscode-yaml,ms-vsliveshare.vsliveshare-audio,ms-vsliveshare.vsliveshare,ms-vscode.powershell,ms-azure-devops.azure-pipelines,azurite.azurite,github.vscode-pull-request-github"

dotnet dev-certs https --trust

# import certificate to localmachine store
$NewPassword = New-RandomPassword -ExcludeSpecialCharacters
$PfxFilePath = ".\localhost.pfx"
dotnet dev-certs https -ep $PfxFilePath -p $NewPassword
$PasswordSecure = ConvertTo-SecureString -String $NewPassword -Force -AsPlainText
Import-PfxCertificate -Exportable -FilePath $PfxFilePath -CertStoreLocation 'Cert:\LocalMachine\My' -Password $PasswordSecure -Verbose
Remove-Item $PfxFilePath