# Compilación y pruebas

Ejecutar desde la raíz en PowerShell con SDK .NET 8 o compatible:

```powershell
git submodule update --init --recursive
dotnet build "ACTIVIDAD ESPECIAL.sln"
dotnet run --project "src/ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL.csproj"
```

## Persistencia en C#

El ejecutable de comprobación verifica ida y vuelta en procesos separados, precisión decimal, respaldos y archivos inválidos. Es previo a las pruebas unitarias de la etapa 6; una excepción indica fallo.

```powershell
New-Item -ItemType Directory -Force test/resultados/local | Out-Null
dotnet run --project test/VerificacionGuardado/VerificacionGuardado.csproj *> test/resultados/local/persistencia.txt
$LASTEXITCODE
Get-Content test/resultados/local/persistencia.txt
```

El informe queda en `test/resultados/local/persistencia.txt`. Las copias CSV se conservan en las rutas temporales que imprime el informe.

## Scripts Python de consola (Windows)

Requieren Python 3.13 y dependencias locales:

```powershell
python -m pip install --target test/herramientas pywinpty==3.1.2 pyte==0.8.2 Pillow==12.0.0
Invoke-WebRequest https://dot.net/v1/dotnet-install.ps1 -OutFile test/herramientas/dotnet-install.ps1
& test/herramientas/dotnet-install.ps1 -Version 8.0.33 -Runtime dotnet -InstallDir test/herramientas/dotnet8 -NoPath
```

El [recorrido final de las 12 opciones](RecorridoFinal.md) conserva 110 capturas, 204 comprobaciones correctas y la revisión de 23 entradas inválidas. Para repetirlo sin sobrescribir la evidencia, usar una carpeta nueva:

```powershell
dotnet build "ACTIVIDAD ESPECIAL.sln"
python test/verificar_recorrido_final.py test/resultados/local/Recorrido-NuevaEjecucion
dotnet test "ACTIVIDAD ESPECIAL.sln"
```

Revisar `resultados.json` (fallos debe ser cero). Las PNG reconstruyen las celdas ANSI, no el escritorio. Esperar a que termine la sesión antes de compilar otra vez.

Para verificar lectura sin competencia, confirmación s/S y cancelación n/x/Enter:

```powershell
dotnet build "ACTIVIDAD ESPECIAL.sln" --artifacts-path test/resultados/local/Confirmacion
dotnet build test/VerificacionGuardado/VerificacionGuardado.csproj
python test/verificar_confirmacion_lectura.py
```

Los resultados JSON, entradas y capturas se generan en `test/resultados/local/Confirmacion-*`.

Para investigar el ejecutable original, preparar una copia una sola vez:

```powershell
if (-not (Test-Path test/StreamORT)) {
    Copy-Item -LiteralPath Ejecutable -Destination test/StreamORT -Recurse
}
python test/probar_original.py
python test/matrices.py entradas
```

`matrices.py` también acepta persistencia, totales, ranking, redondeo, decimales, vacios, escritura, solo-lectura, regresion8 y regresion10. Ejecutar secuencialmente: comparten los datos de `test/StreamORT/`. El último grupo requiere un runtime mayor instalado globalmente.

La evidencia se guarda en `test/resultados/`: archivos `NN-*.txt`, entradas JSON y `resultados.json`. Repetir un escenario puede sobrescribir sus resultados.

Los otros scripts de interfaz son herramientas históricas: requieren las compilaciones de sondas y versiones indicadas en los informes conservados en [IA/Log.md](../IA/Log.md). Ahora las sondas están en `test/sondas/`, las carpetas de trabajo en `test/resultados/local/` y las capturas en subcarpetas fechadas de `test/resultados/`. No equivalen a pruebas unitarias de todos los flujos actuales.

## Organización

- `src/`: aplicación C#.
- `docs/`: guías del proyecto.
- `test/`: pruebas C#, scripts Python y sondas auxiliares.
- `test/resultados/`: evidencia histórica versionada.
- `test/resultados/local/`: nuevos resultados, copias y compilaciones excluidos de Git.
- `test/resultados/local/historico/`: resultados, capturas, clones y compilaciones antiguas trasladados desde `IA/obj/` e `IA/Verificacion/`. El archivo `manifest-traslado.json` registra sus hashes SHA-256. Este material es local y no se incluye en GitHub.
- `IA/`: consigna, arquitectura, plan e historial de IA.

El submódulo `Mis Librerias/` conserva su ubicación y documentación propia.
