param(
[Parameter()]
[string]$GitHubDestinationPAT,


[Parameter()]
[string]$ADOSourcePAT

# Write your PowerShell commands here.
Write-Host '- - - - - - - - - - - - - - - - - - - - - - - - - - - - '
Write-Host ' reflect Azure Devops repo changes to GitHub repo'
Write-Host '- - - - - - - - - - - - - - - - - - - - - - - - - - - - '
$AzureRepoName = "eon-soft"
$ADOCloneURL = "dev.azure.com/Eon-Software/EonSoft-Enterprise/_git/eon-soft"
$GitHubCloneURL = "https://github.com/EnterTheBlackDragon36/eon-soft.git"
$stageDir = pwd | Split-Path
Write-Host "stage Dir is : $stageDir"
$githubDir = $stageDir +"\"+"gitHub"
Write-Host "gitHub Dir : $githubDir"
$destination = $githubDir+"\"+ $AzureRepoName+".git"
Write-Host "destination : $destination"
#Please make sure, you remove https from azure-repo-clone-url
$sourceURL = "https://$($ADOSourcePAT)" + "@" + "$($ADOCloneURL)"
Write-Host "source URL : $sourceURL"
$destURL = "https://" + $($GitHubDestinationPAT) +"@" + "$($GitHubCloneURL)"
Write-Host " dest URL : $destURL"
#Check if the parent directory exists and delete
if((Test-Path -path $githubDir))
{
	Remove-Item -Path $githubDir -Recurse -force
}
if(!(Test-Path -path $gitHubDir))
{
	New-Item -ItemType directory -Path $githubDir
	Set-Location $githubDir
	git clone --mirror $sourceURL
}
else
{
	Write-Host "The given folder path $githubDir already exists";
}
Set-Location $destination
Write-Output '******Git removing remote secondary******'
git remote rm secondary
Write-Output '******Git remote add******'
git remote add --mirror=fetch secondary $destUrl
Write-Output '******Git fetch origin******'
git fetch $sourceURL
Write-Output '******Git push secondary******'
#git remote set-url origin $destURLSetURL
git push secondary --all -f
Write-Output 'Azure DevOps is synced with GitHub Repo'
Set-Location $stageDir
if((Test-Path -path $githubDir))
{
	Remove-Item -Path $githubDir -Recurse -force
}
Write-Host "Job Completed"