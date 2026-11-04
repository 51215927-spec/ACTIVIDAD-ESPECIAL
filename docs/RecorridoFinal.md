# Recorrido final de StreamORT

Fecha: 04/11/2026. Etapa 6, punto 5. Recorrido interactivo supervisado de las 12 opciones mediante pseudoconsola Windows, con entradas reproducibles y revisión de capturas.

Resultado: **204 comprobaciones correctas**, 23 capturas de entradas inválidas revisadas y **119 pruebas xUnit correctas**, sin fallos ni casos omitidos. Se conservan **110 capturas PNG**: 89 de la aplicación, dos después de reiniciar y 19 del original. Compilación sin errores; advertencias previas CA1416 y CS0219.

## Escenarios recorridos

| Opción | Comprobaciones principales |
| --- | --- |
| 1. Crear competencia | Nombre vacío, año no numérico y fuera de límites, cupos inválidos; n/x/Enter cancelan; s/S reemplazan y dejan cero equipos. |
| 2. Registrar equipo | Sin competencia, fecha abreviada/imposible/futura, bisiesta válida, categoría numérica inválida, las cuatro categorías, duplicado sin distinguir caja y cupo agotado. |
| 3. Registrar creador | Equipo inexistente, edades inválidas, seguidores negativos y superiores a int.MaxValue, valoración inválida, coma/punto, especialidad numérica, las cuatro especialidades y alias duplicado. Un mismo alias se acepta en equipos distintos. |
| 4. Mostrar competencia | Todos los equipos, creadores y asociaciones; ausencia de filas antiguas al reemplazar; memoria conservada tras lectura fallida. |
| 5. Mostrar equipo | Equipo inexistente y búsqueda con espacios/caja diferentes; fecha, categoría y creadores. |
| 6. Mostrar creador | Alias inexistente y búsqueda con espacios/caja diferentes; formato visual, especialidad y Estrella. |
| 7. Estadísticas | Equipo vacío con ceros y Sin creadores; promedio decimal, un streamer, 2147483648 mil seguidores y creador más popular. |
| 8. Comparar | Empate, ganador en ambos órdenes, mismo equipo y comparación vacío/lleno en ambos órdenes. |
| 9. Más seguidores | Ausencia de candidatos y selección del equipo con total superior a int.MaxValue. |
| 10. Guardar | Tres CSV en carpeta aislada; error al ocupar actual con un archivo y conservación del obstáculo. |
| 11. Leer | Directorio inexistente, n/x/Enter cancelan, s/S confirman, lectura corrupta sin reemplazo parcial, lectura tras reinicio sin competencia ni confirmación. |
| 12. Salir | TOP 3 ordenado, exclusión de equipo vacío, pausa, cierre y mismos hashes CSV antes/después: no hay guardado automático. |

Además: flechas arriba/abajo circularmente, Enter, conservación de selección al volver, resaltado gris de 55 columnas y fondos Black, DarkBlue, DarkMagenta y DarkGreen. Los equipos de prueba son Alfa, Beta, Vacio y Delta; todos los guardados se hicieron bajo `test/resultados/local/Recorrido-*`, nunca en los datos originales. El original se ejecutó desde una copia de `Ejecutable/` dentro de esa carpeta aislada.

## Errores encontrados y corregidos

- `MostrarEstadisticas` confundía equipo vacío con inexistente. Ahora muestra estadísticas en cero y Sin creadores.
- `MostrarComparacion` anunciaba un ganador al comparar el mismo equipo. Ahora lo rechaza; se igualaron también los textos de empate y equipos vacíos.
- Las fichas mostraban todos los decimales. Ahora presentan hasta dos, sin ceros decimales innecesarios, manteniendo el valor completo en memoria, cálculos y CSV.
- Se agregaron la leyenda (Sin creadores), las separaciones del listado de competencia y TOP 3, y los textos del cupo, fecha inválida y ausencia de candidatos observados en el original.
- Con autorización del usuario, `Utilidades.PedirInt` muestra Ingresá un número entero válido, sin etiqueta ERROR. Publicado primero en Mis-Librerias (`9b92817`); su guía se actualizó después (`7d4299c`). Este repositorio referencia esos cambios publicados.

Cada corrección de código se compiló y publicó antes de modificar otro método. El estado del dominio y la persistencia no cambió.

## Contraste de presentación

Se contrastaron diez pantallas con una ejecución nueva del original: menú inicial, estadísticas vacías, ficha de competencia, equipo y creador, estadísticas con creadores, comparación del mismo equipo, empate, más seguidores y TOP 3. Coinciden en texto y estilos visibles a **120 columnas por 40 filas**.

En las dos fichas consultadas se normalizó solo el eco de la búsqueda: la aplicación recibió variantes deliberadas de caja y espacios; el original recibió Alfa/ana. El fondo de esas filas se compara, pero no su tinta ni su texto de entrada. Se ignora la tinta de espacios vacíos sin vídeo inverso, porque no cambia lo visible. El menú inicial es además idéntico píxel por píxel en los PNG reconstruidos; ambos tienen SHA-256 `578ac297b4103c854e30138e8b1967ad52e18aa65a9ec5f30b3bfb1af00be7d2`.

La primera comparación de estilos contó la tinta invisible como diferencia: se conserva [el resultado anterior a la normalización](../test/resultados/RecorridoFinal-20261104/resultados-antes-normalizacion.json). El [resultado final](../test/resultados/RecorridoFinal-20261104/resultados.json) recalcula únicamente el contraste sobre las mismas capturas, sin alterar la aplicación ni las imágenes.

Las imágenes reconstruyen las celdas ANSI con pyte, paleta de consola de 16 colores y Consolas de 16 píxeles. **No son fotografías del escritorio.** La comprobación actual no demuestra identidad universal en otras dimensiones, culturas, terminales o toda entrada posible. Las pruebas previas del menú a otros tamaños se conservan por separado. El requisito interno de usar MostrarTabla para las fichas sigue pendiente en etapa 4: las correcciones actuales conservan su presentación con MyConsole.

## Evidencia y reproducción

- [Entradas, pantalla y estilos de la aplicación](../test/resultados/RecorridoFinal-20261104/actual.json).
- [Entradas y pantallas tras reinicio](../test/resultados/RecorridoFinal-20261104/reinicio.json).
- [Entradas y pantallas del original](../test/resultados/RecorridoFinal-20261104/original.json).
- [Revisión de entradas inválidas](../test/resultados/RecorridoFinal-20261104/revision-invalidos.json).
- [Manifiesto SHA-256](../test/resultados/RecorridoFinal-20261104/manifest.json).
- Capturas: [menú](../test/resultados/RecorridoFinal-20261104/actual-001-menu-inicial.png), [estadísticas vacías](../test/resultados/RecorridoFinal-20261104/actual-035-estadisticas-equipo-vacio.png), [ficha de competencia](../test/resultados/RecorridoFinal-20261104/actual-057-ficha-competencia.png), [mismo equipo](../test/resultados/RecorridoFinal-20261104/actual-065-comparacion-mismo-equipo.png), [lectura fallida](../test/resultados/RecorridoFinal-20261104/actual-086-lectura-corrupta.png) y [TOP 3](../test/resultados/RecorridoFinal-20261104/actual-089-top3.png).

Requiere las dependencias Python descritas en [Pruebas.md](Pruebas.md), Windows y runtime .NET 8. Ejecutar desde la raíz con una carpeta de salida nueva:

```powershell
dotnet build "ACTIVIDAD ESPECIAL.sln"
python test/verificar_recorrido_final.py test/resultados/local/Recorrido-NuevaEjecucion
dotnet test "ACTIVIDAD ESPECIAL.sln"
```

Consultar `resultados.json`: fallos debe ser cero. No compilar mientras la aplicación de la sesión siga abierta: Windows impide reemplazar la DLL en uso. Durante este trabajo se encontró ese bloqueo al intentar pruebas con compilación en paralelo; al cerrar las sesiones y repetir, las 119 pruebas pasaron.

Este recorrido cubre la tarea 5. La actualización integral de arquitectura, Google Docs, revisión final del README y ZIP siguen en los puntos siguientes del plan.
