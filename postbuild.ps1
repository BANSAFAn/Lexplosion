param(
    [string]$TargetPath,
    [string]$SolutionDir
)

# Trim trailing backslash if present
$SolutionDir = $SolutionDir.TrimEnd('\')

$zipPath = "$SolutionDir\DLL\Lexplosion.Core.zip"

Write-Output "Running postbuild.ps1" >> "$SolutionDir\DLL\postbuild_log.txt"
Write-Output "Current Directory: $(Get-Location)" >> "$SolutionDir\DLL\postbuild_log.txt"
Write-Output "TargetPath: $TargetPath" >> "$SolutionDir\DLL\postbuild_log.txt"
Write-Output "SolutionDir: $SolutionDir" >> "$SolutionDir\DLL\postbuild_log.txt"
Write-Output "zipPath: $zipPath" >> "$SolutionDir\DLL\postbuild_log.txt"

Compress-Archive -Path $TargetPath -DestinationPath $zipPath -Force 2>> "$SolutionDir\DLL\postbuild_error.txt"

Write-Output "Compress-Archive completed" >> "$SolutionDir\DLL\postbuild_log.txt"

if (Test-Path $zipPath) {
    Write-Output "Zip file created successfully" >> "$SolutionDir\DLL\postbuild_log.txt"
    $fileSize = (Get-Item $zipPath).Length
    Write-Output "Zip file size: $fileSize bytes" >> "$SolutionDir\DLL\postbuild_log.txt"
    Write-Output "Files in DLL:" >> "$SolutionDir\DLL\postbuild_log.txt"
    Get-ChildItem -Path "$SolutionDir\DLL" | ForEach-Object { Write-Output $_.Name } >> "$SolutionDir\DLL\postbuild_log.txt"
} else {
    Write-Output "Zip file not created" >> "$SolutionDir\DLL\postbuild_log.txt"
}