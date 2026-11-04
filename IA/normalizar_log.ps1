# Normalización de codificación: conserva UTF-8 válido y convierte bytes CP1252 aislados.
$registroRuta = Join-Path $PSScriptRoot 'Log.md'
$registroBytes = [IO.File]::ReadAllBytes($registroRuta)
$utf8Estricto = [Text.UTF8Encoding]::new($false, $true)
$cp1252 = [Text.Encoding]::GetEncoding(1252)
$ErrorActionPreference = 'Stop'
$texto = [Text.StringBuilder]::new()
$convertidos = 0
$inicio = 0
$indice = 0
while ($indice -lt $registroBytes.Length)
{
    $byte = [int]$registroBytes[$indice]
    $longitud = 0
    if ($byte -lt 128) { $longitud = 1 }
    elseif ($byte -ge 194 -and $byte -le 223) { $longitud = 2 }
    elseif ($byte -ge 224 -and $byte -le 239) { $longitud = 3 }
    elseif ($byte -ge 240 -and $byte -le 244) { $longitud = 4 }
    $valido = $longitud -gt 0 -and $indice + $longitud -le $registroBytes.Length
    if ($valido)
    {
        try { $null = $utf8Estricto.GetString($registroBytes, $indice, $longitud) }
        catch [Text.DecoderFallbackException] { $valido = $false }
    }
    if ($valido)
    {
        $indice += $longitud
        continue
    }
    $null = $texto.Append($utf8Estricto.GetString($registroBytes, $inicio, $indice - $inicio))
    $null = $texto.Append($cp1252.GetString($registroBytes, $indice, 1))
    $convertidos++
    $indice++
    $inicio = $indice
}
$null = $texto.Append($utf8Estricto.GetString($registroBytes, $inicio, $registroBytes.Length - $inicio))
if ($convertidos -gt 0)
{
    $respaldoCarpeta = Join-Path $PSScriptRoot 'Verificacion'
    [IO.Directory]::CreateDirectory($respaldoCarpeta) | Out-Null
    $respaldoRuta = Join-Path $respaldoCarpeta ('Log-original-' + [Guid]::NewGuid().ToString('N') + '.bin')
    [IO.File]::WriteAllBytes($respaldoRuta, $registroBytes)
    [IO.File]::WriteAllText($registroRuta, $texto.ToString(), [Text.UTF8Encoding]::new($false))
    Write-Output "Bytes CP1252 normalizados: $convertidos. Respaldo: $respaldoRuta"
}
else
{
    Write-Output 'El registro ya es UTF-8 válido; no se modifica.'
}
