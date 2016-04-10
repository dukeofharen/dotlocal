$configuration = (Get-Childitem env:path).Value
Rename-Item "Ducode.Local\bin\$configuration\Ducode.Local.exe" dotlocal.exe