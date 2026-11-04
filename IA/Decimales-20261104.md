# Verificación de decimales — 04/11/2026

Usuario: «Sigue con la siguente tarea.».

Respuesta inicial: «Voy a verificar la conservación de 8,5, 8,001, 8,004 y 7,999 al guardar y leer, y su efecto en Estrella, la comparación de equipos y el ranking. Después actualizaré el plan con los resultados».

## Cambios

Nueva clase auxiliar `VerificacionGuardado/PruebaDecimales.cs`, integrada en `VerificacionGuardado/Program.cs`. Construye cuatro equipos con valoraciones 8.5, 8.001, 8.004 y 7.999; cada equipo tiene ocho creadores, cruzando las cuatro especialidades con seguidores 499 y 500. Ejecuta dos recorridos: guardar con es-AR y leer con en-US, y viceversa. Restaura cultura y estado global al terminar.

Las comprobaciones exigen los valores esperados, no solo igualdad entre resultados calculados antes y después. Confirman promedio exacto, Estrella para valoración mayor o igual que 8 y seguidores 500 en las cuatro especialidades, rechazo de Estrella con 499 seguidores o valoración 7.999, comparación en ambos órdenes y ranking Mayor/Zeta/Alfa. Alfa tiene 8.001 y Zeta 8.004: ambos formatean como 8.00, pero gana Zeta y se ordena antes de Alfa pese a su nombre.

No se modificó el código productivo ni las bibliotecas: las pruebas confirmaron que la persistencia decimal existente cumple esta tarea.

## Verificaciones y publicación

`dotnet build IA/VerificacionGuardado/VerificacionGuardado.csproj`: cero errores y cero advertencias.

`dotnet run --project IA/VerificacionGuardado/VerificacionGuardado.csproj --roll-forward Major`: código cero; matriz decimal y regresiones previas correctas (ida/vuelta entre procesos, respaldo, destino bloqueado, lectura temporal y corrupción).

Copias locales conservadas: `C:/Users/julian/AppData/Local/Temp/StreamORT-Decimales-85278e24867844bfbcbb274d2784a02d`. No se suben CSV ni otros datos generados, únicamente el código y la documentación. No se eliminaron archivos.

Cuenta gh 51215927-spec, permiso ADMIN comprobado. Clase publicada en `875e4c0` antes de integrar la llamada. La integración y esta actualización del plan se publican en commits separados. Pendientes: prueba interactiva de confirmación de lectura y matriz de archivos inválidos de la etapa 5.

Progreso comunicado: «Las pruebas pasaron: las valoraciones se recuperan exactas al guardar con cultura argentina y leer con estadounidense, y viceversa. 7,999 no cumple la valoración de Estrella; 8,004 supera a 8,001, aunque ambos se muestran como 8,00. También se conservan los promedios y el orden del TOP 3».

Registro complementario a `Log.md`, que contiene codificación mixta previa y no se reescribe para conservar su historial.

Respuesta final prevista: «Completé la verificación de decimales: precisión, Estrella, comparaciones y TOP 3 correctos tras guardar y leer. Plan actualizado y cambios publicados. Los datos de prueba permanecen fuera de GitHub».
