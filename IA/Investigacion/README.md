# Investigación del ejecutable original

> Estructura actual: scripts en `test/`, sondas en `test/sondas/` y evidencia en `test/resultados/`. Los comandos de los informes siguientes son históricos. Consultar [la guía actual](../../docs/Pruebas.md).

Esta carpeta reúne herramientas y evidencia de las pruebas de caja negra de StreamORT. Se creó para completar el paso 1 de [plan.md](../plan.md). No forma parte de la aplicación C# ni de sus futuras pruebas unitarias.

## Contenido

- `probar_original.py`: conduce una pseudoterminal, envía teclas y texto al menú real y captura sus respuestas.
- `matrices.py`: prepara escenarios de entrada, persistencia, estadísticas y ranking.
- `NN-*.entradas.json`: teclas y líneas enviadas en cada grupo; `\r` representa Enter y `\u001b[B`, flecha abajo.
- `NN-*.txt`: salidas de consola con secuencias ANSI, espacios de relleno y redibujados del menú eliminados. No son capturas visuales exactas; pueden quedar fragmentos de redibujado. El orden de las entradas está en el JSON correspondiente.
- `resultados.json`: resultados observados por grupo, incluidos mensajes, consultas posteriores a errores y muestras de archivos. Sus 131 campos de resultado no equivalen a 131 pruebas independientes.
- [Comparacion-Interfaz-20261103.md](Comparacion-Interfaz-20261103.md): contraste de presentación y navegación de la etapa 3.5, con 43 pantallas en `Interfaz-20261103/`, entradas y colores por celda. `comparar_interfaz.py` conduce las sesiones y reconstruye las capturas; `SondaInterfaz/` llama las utilidades reales sin cambiar la aplicación. Las dependencias auxiliares locales adicionales son pyte 0.8.2 y Pillow 12.0.0. El informe contiene los comandos de repetición y las limitaciones.
- [ColoresMenu-20261103.md](ColoresMenu-20261103.md): diseño, publicación y comparación interactiva de las firmas anterior/nueva de `PedirOpcionMenu`. `SondaColoresMenu/` y `verificar_colores_menu.py` verifican equivalencia de las pantallas, selección y restauración con dos paletas; evidencia en `ColoresMenu-20261103/`. No sustituye las pruebas unitarias finales.
- [MenuCompatible-20261103.md](MenuCompatible-20261103.md): implementación del menú con ColoresMenu/FormatoMenu e integración en Main. `SondaMenuCompatible/` y `verificar_menu_compatible.py` contrastan menús por celdas/colores, circularidad, regreso, listas numéricas, inválidos y entrada redirigida. Evidencia en `MenuCompatible-20261103/` y límites detallados en el informe.
- [VerificacionEtapa35-20261103.md](VerificacionEtapa35-20261103.md): repetición del contraste sobre compilaciones nuevas de bibliotecas, aplicación y sonda; 28 capturas nuevas y resultado por escenario. `verificar_etapa35.py` conserva la evidencia previa. Alcance limitado a lo implementado; no incluye flujos pendientes de la etapa 4.
- [DistribucionEtapa35-20261103.md](DistribucionEtapa35-20261103.md): clonación recursiva desde GitHub, ZIP de prueba con los archivos reales del submódulo, manifiesto SHA-256 y compilación de la copia extraída sin Git. El ZIP final sigue pendiente en la etapa 6.
- [DocumentacionEtapa35-20261103.md](DocumentacionEtapa35-20261103.md): README de entrada cortos, guías junto a cada biblioteca, consolidación de versiones, enlaces y ejemplos verificados. Publicación documental y cierre del alcance vigente de etapa 3.5.
- `StreamORT/`: copia de trabajo del ejecutable y sus datos, excluida de Git.
- `herramientas/`: pywinpty 3.1.2 y runtime .NET 8.0.33 x64 locales, excluidos de Git. No se instaló otro runtime global.
- `*.ansi.txt` y `ejecucion*.txt`: capturas crudas y diagnósticos locales, también excluidos. Se conserva en Git la versión legible y las entradas.

Los resultados funcionales se explican en [Analisis.md](../Analisis.md), con extractos en [Pruebas-StreamORT.md](../Pruebas-StreamORT.md). Los scripts Python son auxiliares del agente; la entrega académica sigue siendo C# y .NET 8.

## Repetir las pruebas en Windows

Requiere Python 3.13, PowerShell, conexión para obtener las herramientas y la distribución `Ejecutable/` del repositorio. Desde la raíz, preparar la copia **solo si todavía no existe**:

```powershell
if (-not (Test-Path -LiteralPath 'IA/Investigacion/StreamORT')) {
    Copy-Item -LiteralPath 'Ejecutable' -Destination 'IA/Investigacion/StreamORT' -Recurse
}
python -m pip install --target IA/Investigacion/herramientas pywinpty==3.1.2
Invoke-WebRequest https://dot.net/v1/dotnet-install.ps1 -OutFile IA/Investigacion/herramientas/dotnet-install.ps1
& IA/Investigacion/herramientas/dotnet-install.ps1 -Version 8.0.33 -Runtime dotnet -InstallDir IA/Investigacion/herramientas/dotnet8 -NoPath
```

Los siguientes comandos sobrescriben los datos **de la copia de trabajo** y regeneran la evidencia del grupo correspondiente. Ejecutarlos secuencialmente, porque comparten esa copia:

```powershell
python IA/Investigacion/probar_original.py
python IA/Investigacion/matrices.py entradas
python IA/Investigacion/matrices.py persistencia
python IA/Investigacion/matrices.py totales
python IA/Investigacion/matrices.py ranking
python IA/Investigacion/matrices.py redondeo
python IA/Investigacion/matrices.py decimales
python IA/Investigacion/matrices.py vacios
python IA/Investigacion/matrices.py escritura
python IA/Investigacion/matrices.py solo-lectura
python IA/Investigacion/matrices.py regresion8
python IA/Investigacion/matrices.py regresion10
```

El último grupo usa `C:/Program Files/dotnet/dotnet.exe` con `--roll-forward Major`: en este equipo ejecutó .NET 10.0.12. Verificar los runtimes instalados antes de repetir esa comparación en otro equipo. Las pruebas de hoy/mañana usan la fecha local al ejecutarse; la sesión registrada fue el 02/11/2026.

El ensayo `solo-lectura` restaura el atributo de escritura del archivo de prueba en un bloque `finally`. Si se interrumpe el proceso desde fuera, revisar ese atributo en la copia antes de repetirlo. Los scripts no escriben en `Ejecutable/` ni modifican el ensamblado recuperado.

## Alcance

Son ensayos automatizados sobre el programa real, con inspección posterior de sus resultados. La finalización de un script significa que recorrió los pasos y obtuvo las respuestas esperadas para continuar; no reemplaza una batería de aserciones unitarias. No se descompiló el ejecutable.

Los ensayos abarcan casos que afectan el diseño y la persistencia; no prueban toda combinación posible de caracteres, longitudes, fechas, sistemas operativos o interrupciones de energía.
