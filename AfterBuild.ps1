$configuration = (Get-Childitem env:CONFIGURATION).Value
Rename-Item "Ducode.Local\bin\$configuration\Ducode.Local.exe" dotlocal.exe