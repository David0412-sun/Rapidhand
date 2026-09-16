param(
    [Parameter(Mandatory = $true)]
    [string]$Workspace
)

$ErrorActionPreference = 'Stop'
trap {
    Write-Output $_.InvocationInfo.PositionMessage
    Write-Output $_.ScriptStackTrace
    Write-Output $_.Exception.ToString()
    break
}

$outDir = Join-Path $Workspace 'tmp\solidworks'
New-Item -ItemType Directory -Force -Path $outDir | Out-Null

function Get-ComponentRows {
    param($Assembly)
    $rows = @()
    $components = @($Assembly.GetComponents($false))
    foreach ($component in $components) {
        if ($null -eq $component) { continue }
        $path = [string]$component.GetPathName()
        $rows += [pscustomobject]@{
            instance = [string]$component.Name2
            file = if ($path) { [System.IO.Path]::GetFileName($path) } else { '' }
            path = $path
            suppressed = [bool]$component.IsSuppressed()
            configuration = [string]$component.ReferencedConfiguration
        }
    }
    return $rows
}

function Save-ModelView {
    param($SwApp, $Doc, [string]$ViewName, [int]$ViewId, [string]$OutputPath)
    $Doc.ShowNamedView2($ViewName, $ViewId) | Out-Null
    $Doc.ViewZoomtofit2() | Out-Null
    Start-Sleep -Milliseconds 800
    $errors = 0
    $warnings = 0
    $ok = $Doc.Extension.SaveAs($OutputPath, 0, 2, $null, [ref]$errors, [ref]$warnings)
    return [pscustomobject]@{ path = $OutputPath; ok = [bool]$ok; errors = $errors; warnings = $warnings }
}

$assemblies = @(
    [pscustomobject]@{ key='finger'; path=(Join-Path $Workspace 'RAPID-Hand-main\RapidHandHardware\mechanical_structure\rss_design\finger_with_cam\手指带摄像头.SLDASM') },
    [pscustomobject]@{ key='thumb'; path=(Join-Path $Workspace 'RAPID-Hand-main\RapidHandHardware\mechanical_structure\rss_design\thumb_with_cam\拇指带摄像头.SLDASM') }
)

Write-Output 'Creating SolidWorks COM object'
$sw = New-Object -ComObject SldWorks.Application.31
Write-Output 'SolidWorks COM object created'
$sw.Visible = $true
$results = @()
try {
    foreach ($item in $assemblies) {
        $openErrors = 0
        $openWarnings = 0
        $doc = $sw.OpenDoc6($item.path, 2, 1, '', [ref]$openErrors, [ref]$openWarnings)
        if ($null -eq $doc) { throw "SolidWorks failed to open $($item.path); error=$openErrors warning=$openWarnings" }
        $sw.ActivateDoc3($doc.GetTitle(), $true, 0, [ref]$openErrors) | Out-Null
        $assembly = $doc
        $components = @(Get-ComponentRows -Assembly $assembly)
        $views = @()
        $views += Save-ModelView -SwApp $sw -Doc $doc -ViewName '*Isometric' -ViewId 7 -OutputPath (Join-Path $outDir "$($item.key)_isometric.png")
        $views += Save-ModelView -SwApp $sw -Doc $doc -ViewName '*Front' -ViewId 1 -OutputPath (Join-Path $outDir "$($item.key)_front.png")
        $views += Save-ModelView -SwApp $sw -Doc $doc -ViewName '*Right' -ViewId 3 -OutputPath (Join-Path $outDir "$($item.key)_right.png")
        $explodeNames = @()
        try { $explodeNames = @($assembly.GetExplodedViewNames2($doc.ConfigurationManager.ActiveConfiguration.Name)) } catch {}
        $results += [pscustomobject]@{
            key = $item.key
            title = [string]$doc.GetTitle()
            open_errors = $openErrors
            open_warnings = $openWarnings
            configuration = [string]$doc.ConfigurationManager.ActiveConfiguration.Name
            exploded_views = $explodeNames
            components = $components
            views = $views
        }
        $sw.CloseDoc($doc.GetTitle())
    }
}
finally {
    $sw.ExitApp()
    [System.Runtime.InteropServices.Marshal]::FinalReleaseComObject($sw) | Out-Null
}

$jsonPath = Join-Path $outDir 'inspection.json'
$results | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $jsonPath -Encoding UTF8
Write-Output $jsonPath
