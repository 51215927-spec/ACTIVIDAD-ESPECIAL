# Matriz de errores de persistencia — 04/11/2026

Usuario: «siguente».

Respuesta inicial: «Voy a probar archivos vacíos, ausentes, dañados, duplicados, relaciones inválidas, caracteres especiales y errores de escritura, comprobando que se conserven los datos anteriores. Después actualizaré el plan y publicaré los cambios».

Se agregó `VerificacionGuardado/PruebaArchivosInvalidos.cs` y su invocación en `Program.cs` de la sonda. No se modificaron las clases productivas ni las bibliotecas. Cada variante altera una copia temporal nueva de un guardado válido. No se tocan los datos del usuario.

## Casos y garantías comprobadas

- 18 variantes: para cada uno de los tres CSV se prueba ausencia, contenido vacío, solo espacios, cabecera inválida, fila con comillas sin cerrar después de una fila válida y fila duplicada. Se rechazan los 18 casos manteniendo la misma competencia en memoria, su equipo, su creador y valoración exacta.
- Dos asociaciones inválidas: equipo de otra competencia y creador de equipo inexistente. Ambas se rechazan sin modificar memoria.
- Directorio de datos inexistente: rechazo y memoria conservada.
- Guardado con competencia.csv de actual abierto sin compartir: Windows impide publicar el nuevo conjunto. Se recibe IOException y, tras liberar el bloqueo, los tres CSV anteriores se comparan completos y son idénticos. La memoria también permanece intacta.

Total: 22 casos correctos. Los caracteres especiales se comprueban en la regresión de ida/vuelta existente: tildes, comas, comillas, punto y coma y barra inversa se recuperan exactamente. Las competencias sin equipos y los equipos vacíos siguen siendo válidos con archivos que contienen encabezados.

## Verificaciones

`dotnet build IA/VerificacionGuardado/VerificacionGuardado.csproj`: cero errores y cero advertencias.

`dotnet run --project IA/VerificacionGuardado/VerificacionGuardado.csproj --roll-forward Major`: salida cero, matriz de 22 casos y regresiones previas correctas (tres escenarios entre procesos, culturas/decimales, respaldo y lectura temporal).

Copias conservadas en `C:/Users/julian/AppData/Local/Temp/StreamORT-Errores-4647f648efeb48c5bec735629e56d84a`. No se suben los CSV generados ni se eliminan datos. El comportamiento del bloqueo se ensaya en Windows; no demuestra resistencia a cortes de energía, conflictos simultáneos entre aplicaciones ni todos los errores posibles de sistema.

## Progreso, publicación y pendientes

Progreso comunicado: «Pasaron los 22 casos de archivos inválidos y escritura bloqueada. Los errores de lectura conservaron la competencia en memoria; al bloquear un CSV durante el guardado, también se conservaron intactos los tres archivos anteriores. Las pruebas de caracteres especiales, decimales y lectura tras reiniciar volvieron a pasar».

Clase publicada en `9b035bc`, antes de integrar la llamada. Se publican por separado integración, plan y registro. Cuenta gh 51215927-spec, permiso ADMIN comprobado antes del push. Se marca la última casilla de pruebas de persistencia, pero sigue pendiente la confirmación interactiva de la opción 11; no se declara cerrada toda la etapa 5.

Registro complementario a `Log.md`, cuya codificación mixta previa impide su edición mediante apply_patch sin convertir el historial; se conserva íntegro.

Respuesta final prevista: «Completé la matriz: 22 casos correctos, sin perder la competencia en memoria ni los CSV anteriores ante el bloqueo ensayado. Plan actualizado y cambios publicados. Para cerrar la etapa 5 queda verificar la confirmación interactiva de lectura».
