$configuration = (Get-Childitem env:CONFIGURATION).Value
Rename-Item "Ducode.Local\bin\$configuration\Ducode.Local.exe" dotlocal.exe
Rename-Item "Ducode.Local\bin\$configuration\Ducode.Local.exe.config" App.config

Get-Childitem . -recurse -include *.xml -force | Remove-Item
Get-Childitem . -recurse -include *.pdb -force | Remove-Item