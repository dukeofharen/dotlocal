$BuildNumber = (Get-Childitem env:APPVEYOR_BUILD_NUMBER).Value
Write-Host $BuildNumber