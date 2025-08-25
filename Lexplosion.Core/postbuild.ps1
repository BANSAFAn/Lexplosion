param(
    [string]$TargetPath,
    [string]$SolutionDir
)
$SolutionDir = $SolutionDir.TrimEnd('\')
Compress-Archive -Path $TargetPath -DestinationPath "$SolutionDir\DLL\Lexplosion.Core.zip" -Force