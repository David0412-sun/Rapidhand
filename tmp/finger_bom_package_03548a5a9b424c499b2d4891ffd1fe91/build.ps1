param(
    [string]$Tectonic = 'tectonic'
)

$ErrorActionPreference = 'Stop'
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
Push-Location $here
try {
    & $Tectonic 'main.tex' '--keep-logs'
    if ($LASTEXITCODE -ne 0) { throw "Tectonic failed with exit code $LASTEXITCODE" }
    Move-Item -LiteralPath 'main.pdf' -Destination 'RapidHand_camera_BOM_assembly_guide.pdf' -Force
}
finally {
    Pop-Location
}
