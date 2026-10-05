$ErrorActionPreference = "Stop"

$Root = Split-Path -Parent $PSScriptRoot

$DailyQuestPool = Join-Path $Root "src\ZoneServer\World\Quests\Daily\DailyQuestPools.cs"

$SpawnDirs = @(
    (Join-Path $Root "packages\laima\scripts\zone\content\laima\mobs\fields"),
    (Join-Path $Root "packages\laima\scripts\zone\content\laima\mobs\dungeons")
)

$MonsterIdFile = Join-Path $Root "packages\laima\scripts\zone\const\MonsterId.cs"

$OutputFile = Join-Path $Root "src\ZoneServer\Services\HuntingTaskMonsters.cs"
$ReportFile = Join-Path $Root "tools\hunting-task-monster-map.txt"

Write-Host "============================================"
Write-Host " Hunting Task Monster Generator"
Write-Host "============================================"
Write-Host ""

if (-not (Test-Path $DailyQuestPool)) {
    throw "DailyQuestPools.cs not found: $DailyQuestPool"
}

if (-not (Test-Path $MonsterIdFile)) {
    throw "MonsterId.cs not found: $MonsterIdFile"
}

foreach ($dir in $SpawnDirs) {
    if (-not (Test-Path $dir)) {
        throw "Spawn directory not found: $dir"
    }
}

# ------------------------------------------------------------
# Read Daily Quest map list
# ------------------------------------------------------------

$dailyContent = Get-Content $DailyQuestPool -Raw

$mapMatches = [regex]::Matches(
    $dailyContent,
    '\["([^"]+)"\]\s*='
)

$maps = @(
    $mapMatches |
        ForEach-Object {
            $_.Groups[1].Value
        } |
        Sort-Object -Unique
)

Write-Host "Daily Quest maps: $($maps.Count)"

# ------------------------------------------------------------
# Read MonsterId constants
# ------------------------------------------------------------

$monsterIdContent = Get-Content $MonsterIdFile -Raw

$monsterConstants = @{}

[regex]::Matches(
    $monsterIdContent,
    'public\s+const\s+int\s+([A-Za-z0-9_]+)\s*=\s*(\d+)\s*;'
) | ForEach-Object {
    $name = $_.Groups[1].Value
    $id = [int]$_.Groups[2].Value

    $monsterConstants[$name] = $id
}

Write-Host "MonsterId constants: $($monsterConstants.Count)"

# ------------------------------------------------------------
# Index all spawn files
# ------------------------------------------------------------

$spawnFiles = @(
    Get-ChildItem $SpawnDirs -Recurse -File -Filter "*.cs"
)

Write-Host "Spawn files: $($spawnFiles.Count)"
Write-Host ""

$spawnIndex = @{}

foreach ($file in $spawnFiles) {
    $baseName = $file.BaseName.ToLowerInvariant()

    if (-not $spawnIndex.ContainsKey($baseName)) {
        $spawnIndex[$baseName] = @()
    }

    $spawnIndex[$baseName] += $file
}

# ------------------------------------------------------------
# Process each Daily Quest map
# ------------------------------------------------------------

$spawnEntries = New-Object System.Collections.Generic.List[object]
$report = New-Object System.Collections.Generic.List[string]

$mapsFound = 0
$mapsMissing = 0
$mapsWithoutMonsters = 0
$unresolved = 0

foreach ($map in $maps) {
    $key = $map.ToLowerInvariant()

    $report.Add("============================================================")
    $report.Add("MAP: $map")

    if (-not $spawnIndex.ContainsKey($key)) {
        $report.Add("STATUS: SPAWN FILE NOT FOUND")
        $report.Add("")

        Write-Host "[MISSING] $map"

        $mapsMissing++
        continue
    }

    $files = @($spawnIndex[$key])

    $mapMonsterNames = New-Object System.Collections.Generic.HashSet[string]

    foreach ($file in $files) {
        $report.Add("FILE: $($file.FullName)")

        $content = Get-Content $file.FullName -Raw

        # Only normal AddSpawner calls.
        # AddBossSpawner is intentionally ignored.
        $matches = [regex]::Matches(
            $content,
            'AddSpawner\s*\([^;]*?MonsterId\.([A-Za-z0-9_]+)'
        )

        foreach ($match in $matches) {
            $monsterName = $match.Groups[1].Value

            [void]$mapMonsterNames.Add($monsterName)
        }
    }

    if ($mapMonsterNames.Count -eq 0) {
        $report.Add("STATUS: NO NORMAL MONSTERS FOUND")
        $report.Add("")

        Write-Host "[EMPTY]   $map"

        $mapsWithoutMonsters++
        continue
    }

    $mapsFound++

    foreach ($monsterName in ($mapMonsterNames | Sort-Object)) {
        if (-not $monsterConstants.ContainsKey($monsterName)) {
            $report.Add("UNRESOLVED: MonsterId.$monsterName")

            Write-Warning "MonsterId constant not found: $monsterName ($map)"

            $unresolved++
            continue
        }

        $monsterId = $monsterConstants[$monsterName]

        $spawnEntries.Add(
            [PSCustomObject]@{
                MonsterId    = $monsterId
                MonsterConst = $monsterName
                MapClassName = $map
            }
        )

        $report.Add("MONSTER: $monsterName = $monsterId")
    }

    $report.Add("")
}

# ------------------------------------------------------------
# Remove only duplicate MonsterId + MapClassName combinations
# ------------------------------------------------------------

$uniqueEntries = @(
    $spawnEntries |
        Sort-Object MapClassName, MonsterId |
        Group-Object {
            "$($_.MapClassName)|$($_.MonsterId)"
        } |
        ForEach-Object {
            $_.Group[0]
        }
)

$uniqueMonsterIds = @(
    $uniqueEntries |
        Select-Object -ExpandProperty MonsterId -Unique
)

# ------------------------------------------------------------
# Generate C#
# ------------------------------------------------------------

$lines = New-Object System.Collections.Generic.List[string]

$lines.Add("namespace Melia.Zone.Services")
$lines.Add("{")
$lines.Add("    public sealed class HuntingTaskMonsterSpawn")
$lines.Add("    {")
$lines.Add("        public int MonsterId { get; }")
$lines.Add("        public string MapClassName { get; }")
$lines.Add("")
$lines.Add("        public HuntingTaskMonsterSpawn(int monsterId, string mapClassName)")
$lines.Add("        {")
$lines.Add("            this.MonsterId = monsterId;")
$lines.Add("            this.MapClassName = mapClassName;")
$lines.Add("        }")
$lines.Add("    }")
$lines.Add("")
$lines.Add("    public static class HuntingTaskMonsters")
$lines.Add("    {")
$lines.Add("        public static readonly HuntingTaskMonsterSpawn[] Spawns =")
$lines.Add("        {")

foreach ($entry in $uniqueEntries) {
    $lines.Add(
        "            new($($entry.MonsterId), `"$($entry.MapClassName)`"), // $($entry.MonsterConst)"
    )
}

$lines.Add("        };")
$lines.Add("    }")
$lines.Add("}")

Set-Content `
    -Path $OutputFile `
    -Value $lines `
    -Encoding UTF8

Set-Content `
    -Path $ReportFile `
    -Value $report `
    -Encoding UTF8

Write-Host ""
Write-Host "============================================"
Write-Host " Generation complete"
Write-Host "============================================"
Write-Host ""
Write-Host "Daily Quest maps : $($maps.Count)"
Write-Host "Maps found       : $mapsFound"
Write-Host "Maps missing     : $mapsMissing"
Write-Host "Maps empty       : $mapsWithoutMonsters"
Write-Host "Spawn entries    : $($uniqueEntries.Count)"
Write-Host "Unique monsters  : $($uniqueMonsterIds.Count)"
Write-Host "Unresolved       : $unresolved"
Write-Host ""
Write-Host "Generated:"
Write-Host "  $OutputFile"
Write-Host ""
Write-Host "Report:"
Write-Host "  $ReportFile"