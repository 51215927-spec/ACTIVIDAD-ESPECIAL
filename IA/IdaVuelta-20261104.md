# Verificación de ida y vuelta — 04/11/2026

Usuario: «sigue con la siguente tarea».

Respuesta inicial: «Voy a avanzar con la verificación de ida y vuelta: guardar, iniciar otro proceso, leer y comparar todos los campos, las asociaciones y las estadísticas. También cerraré la comprobación pendiente de las confirmaciones de lectura».

## Cambios y motivo

Se agregó `VerificacionGuardado/PruebaIdaVuelta.cs` y se integró su ejecución en `VerificacionGuardado/Program.cs`. Cada escenario usa dos procesos nuevos de .NET: el primero guarda CSV y una descripción JSON de referencia; el segundo comienza sin competencia, lee y compara su descripción completa con la referencia. También comprueba que Leer no asigna el estado global antes de la confirmación. No se cambia código de producción ni bibliotecas.

Casos: competencia completa con cuatro equipos, competencia sin equipos y cuatro equipos vacíos. Incluye todas las categorías y especialidades, alias compartido entre equipos, fecha bisiesta, edades límite, seguidores cero y dos creadores con int.MaxValue, valoraciones 8.004, 8.001, 7.999 y 8.5, tildes, comas, comillas, punto y coma y barra inversa.

La comparación abarca todos los campos persistidos, pertenencia de equipos y creadores, orden de registro, promedios, Streamers, seguidores, creador más popular, Estrella, todas las comparaciones entre equipos, equipo con más seguidores y TOP 3. Es una comprobación de conservación entre procesos; no sustituye las pruebas independientes de reglas ni la matriz exhaustiva de errores.

## Resultados

`dotnet run --project IA/VerificacionGuardado/VerificacionGuardado.csproj --roll-forward Major`: correcto para tres escenarios y para la regresión de guardado/lectura previa. Los procesos hijo se exigen con código de salida cero y tiempo máximo de 30 segundos. La sonda compila sin errores ni advertencias.

`dotnet build "ACTIVIDAD ESPECIAL.sln" --artifacts-path IA/obj/IdaVuelta`: cero errores y las dos advertencias previas CA1416 y CS0219.

Copias locales: `C:/Users/julian/AppData/Local/Temp/StreamORT-IdaVuelta-0f7d7007a1e44791a503641979a1d238`. No se publican CSV ni JSON generados; solo código de verificación y documentación. No se eliminaron archivos.

La confirmación interactiva anunciada al iniciar no se verificó en esta ejecución: el proceso de lectura de la sonda no recorre el menú. Se conserva pendiente la casilla de opción 11; la prueba entre procesos permite cerrar únicamente la tarea de ida y vuelta. La matriz específica de decimales/Estrella y la de errores siguen pendientes.

Progreso comunicado: «La prueba de ida y vuelta pasó en tres escenarios: competencia completa, sin equipos y con equipos vacíos. Comparó todos los campos, el orden, las asociaciones, las estadísticas, los resultados de Estrella, las comparaciones y el TOP 3 entre procesos separados. Los CSV quedaron en carpetas temporales locales. Voy a marcar esta tarea en el plan; la confirmación de lectura seguirá pendiente de prueba interactiva».

Commits publicados, uno antes del siguiente cambio: `b4b9734` agrega la clase de prueba; `124a211` integra los procesos. Se prepara un commit documental para este registro y el plan. Cuenta gh activa 51215927-spec, permiso ADMIN comprobado antes de publicar.

Este registro complementa `Log.md`, cuyo historial contiene codificación mixta que impide editarlo mediante apply_patch; se preserva el archivo existente.

Respuesta final prevista: «Completé la verificación de ida y vuelta en procesos separados: tres escenarios correctos, todos los campos y estadísticas conservados. Plan actualizado; compilación sin errores y cambios publicados. La confirmación interactiva de lectura sigue pendiente».
