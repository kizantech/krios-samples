### UploadDataToKrios.ps1
### This script will upload HR Data to Krios using the Krios API
### it expects the data to be exported to a Storage Account in JSON format that matches the Krios API schema

#Organization Specific Variable Section. Please edit the below to meet your organization's information

$storageAccountName = "myhrexportstorage"
$storageAccountKey = "{storage account key goes here}"
$storageShareName = "systemsexports"
$fileNameToBeUploaded = "PTOSummarized.json"

$clientId = "{enter your app's client id guid}"  #the client ID for your application
$secret = '{client secret}' #the secret for your application registration
$tenant = "{azure tenant id guid}" #your Entra tenant ID

$scope = 'api://167d39bc-b848-4212-a93d-c441201e3b30/.default' #api scope, this should be static, it references KiZAN's Krios API app registration


#End of Organization Specific Variables

<#
.DESCRIPTION
Downloads a file from Azure Storage, places in temporary storage for the runbook, and returns the content of the file
.PARAMETER fileName
The name of the file to be retrieved from Azure Storage
.PARAMETER shareName
The name of the share within Azure Storage (e.g. FilesForKriosExport)
.PARAMETER deleteContentAfterDownloading
If true, we will remove the file from temporary storage manually, otherwise it will remain until otherwise cleaned up
#>
function Get-FileFromAzureAndReturnJson([string] $fileName, [string] $shareName, [bool] $deleteContentAfterDownloading) {
    $tempPath = $env:TEMP
    $destinationPath = "$tempPath\$filename"

    $ctx = New-AzStorageContext -StorageAccountName $storageAccountName -Protocol Https -StorageAccountKey $storageAccountKey

    Get-AzStorageFileContent -ShareName $shareName -Path $fileName -Context $ctx -Destination $destinationPath -Force

    $jsonBody = Get-Content $destinationPath
    return $jsonBody
    if($deleteContentAfterDownloading)
    {
        Remove-Item $destinationPath
    }
}

$kriosDataFileContent = Get-FileFromAzureAndReturnJson -fileName $fileNameToBeUploaded -shareName $storageShareName -deleteContentAfterDownloading $false

$secretString = ConvertTo-SecureString -String $secret -Force -AsPlainText

$accessToken = Get-MsalToken -TenantId $tenant -ClientId $clientId -Scopes $Scope -ClientSecret $secretString -AzureCloudInstance AzurePublic

$headers = @{
    "Authorization" = "Bearer " + $accessToken.AccessToken
    "Api-Version"   = "v1"
}

$uri = "https://kz-krios-apim-prod.azure-api.net/api/Dataload/$tenant/json"

$results = Invoke-RestMethod -Method Post -Headers $headers -Uri $uri -ContentType "application/json" -Body $kriosDataFileContent
Write-Output $results.Body
