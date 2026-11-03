# Análisis funcional de StreamORT

Fecha de investigación: 02/11/2026.

## Objetivo y alcance

Este documento describe el comportamiento observable del ejecutable recuperado para orientar su reconstrucción en C# mediante POO. Reúne las transcripciones de [Contexto.md](Contexto.md) y pruebas nuevas realizadas en una terminal interactiva. No se descompiló el programa ni se usó el código de la reconstrucción como prueba del funcionamiento original.

Se distinguen tres niveles de información:

- **Contexto:** comportamiento registrado previamente en `Contexto.md`.
- **Prueba nueva:** observado ejecutando la distribución recuperada durante esta investigación.
- **Propuesta o pendiente:** recomendación de diseño o caso todavía sin comprobación suficiente.

Las capturas de una parte de las pruebas nuevas están en [Pruebas-StreamORT.md](Pruebas-StreamORT.md). Los primeros ensayos se resumen aquí; ese registro no contiene todas las interacciones iniciales.

## Entorno y procedimiento

La distribución contiene `StreamORT.exe`, `StreamORT.dll`, archivos de configuración, símbolos y la carpeta `Datos`. Su configuración solicita `Microsoft.NETCore.App 8.0.0`, arquitectura x64.

El inicio directo de `StreamORT.exe` falló porque el entorno dispone de .NET 10.0.12 y no de .NET 8. Para probar la aplicación se ejecutó el mismo ensamblado, sin modificarlo:

```powershell
dotnet --roll-forward Major .\StreamORT.dll
```

Esta adaptación debe considerarse al reproducir las pruebas: los resultados corresponden al ensamblado original ejecutado sobre .NET 10. No se verificaron diferencias frente al runtime .NET 8.

SHA-256 de `Ejecutable/StreamORT.dll`:

```text
20CCCCAFDFCC2644DE58A5BEA8FC1328EBDB84F58BBFCF7CFE8D4A64716F9603
```

La primera sesión utilizó la distribución original exclusivamente para operaciones en memoria. Las pruebas de escritura, recuperación y archivos inválidos se hicieron en una copia en `%TEMP%/StreamORT-Analisis-20261102`. No se guardaron datos sobre los archivos originales.

Se usaron flechas y Enter para navegar, texto para completar campos y una tecla para continuar tras cada operación. El caso de cuatro equipos empatados se preparó mediante archivos de prueba con el formato observado y se cargó con la opción 11; los demás escenarios principales se crearon desde el menú.

## Información y relaciones observables

| Elemento | Datos observables | Relación |
| --- | --- | --- |
| Competencia | Nombre, patrocinador, año y máximo de equipos | Una competencia activa contiene equipos. |
| Equipo | Nombre, fecha de creación y categoría | Pertenece a la competencia y contiene creadores. |
| Creador | Nombre, alias, edad, seguidores, valoración y especialidad | Se registra dentro de un equipo. |
| Datos calculados | Estrella, promedios, seguidores totales, cantidad de streamers y rankings | Se obtienen a partir de los datos anteriores. |

Se trabaja con una sola competencia activa. Crear otra, tras confirmar, reemplaza el estado de trabajo. No se observan opciones para editar, eliminar, trasladar creadores, administrar varias competencias simultáneas o disputar partidas: la comparación se basa en valoraciones.

El nombre identifica al equipo. El alias identifica al creador **dentro de su equipo**: el mismo alias se pudo registrar en Alpha y Beta. No debe imponerse una unicidad global del alias.

## Menú y navegación

Al arrancar se muestra `Competencia: Sin crear`, incluso cuando existen archivos de datos. La recuperación se solicita expresamente con la opción 11.

| Opción | Operación | Fondo |
| --- | --- | --- |
| 1 | Crear competencia | Negro |
| 2 | Registrar equipo | Azul |
| 3 | Registrar creador en un equipo | Violeta |
| 4 | Mostrar información de la competencia | Negro |
| 5 | Mostrar información de un equipo | Azul |
| 6 | Mostrar información de un creador | Violeta |
| 7 | Mostrar estadísticas de un equipo | Azul |
| 8 | Comparar valoración de dos equipos | Azul |
| 9 | Mostrar el equipo con más seguidores | Azul |
| 10 | Guardar datos de la competencia | Negro |
| 11 | Leer datos de la competencia | Negro |
| 12 | Salir | Negro |

La selección se resalta con fondo claro y texto negro. Se conserva la opción seleccionada al regresar al menú. La navegación es circular: desde la primera opción, dos flechas hacia arriba permitieron llegar a la opción 11.

Las selecciones de categoría y especialidad del original se realizan ingresando un número de 1 a 4. Para la reconstrucción, el contexto pide usar `PedirOpcionMenu` y mostrar datos con `MostrarTabla`; no es necesario copiar literalmente esa presentación.

## Validaciones de entrada

| Campo | Regla documentada u observada | Evidencia |
| --- | --- | --- |
| Textos requeridos | Un campo vacío se rechaza con “El dato no puede estar vacío.” | Prueba nueva sobre nombre de competencia. |
| Espacios exteriores | Se eliminan en los campos textuales ensayados | Prueba nueva sobre competencia, patrocinador, equipo, nombre y alias de creador. |
| Año | Entero entre 2000 y 2027 | Contexto; extremos 2000 y 2027 aceptados en pruebas nuevas. |
| Máximo de equipos | Entero entre 1 y 10000 | Contexto; 1 y 10 aceptados en pruebas nuevas. |
| Fecha de equipo | Formato estricto `dd/MM/yyyy`, válida y no futura | Contexto. |
| Edad | Entero entre 14 y 30 | Contexto; extremos aceptados en pruebas nuevas. |
| Seguidores | Entero entre 0 y 2147483647, expresado en miles | Contexto y extremos aceptados en pruebas nuevas. |
| Valoración | Entero entre 1 y 10 | Contexto y pruebas nuevas. |
| Categoría/especialidad | Selección numérica entre 1 y 4 | Contexto y pruebas nuevas. |

El contexto muestra rechazo de letras y números fuera de rango con “Ingresá un número entero válido.”, seguido de una nueva solicitud del mismo campo. Para fechas inválidas muestra “Fecha inválida o futura.”.

Las categorías son Gaming, Humor, Música y Entretenimiento. Las especialidades son Streamer, Youtuber, Tiktoker e Influencer.

Los seguidores se almacenan y muestran en miles: 500 equivale a 500 mil seguidores. No se debe multiplicar por 1000 al calcular los totales que se muestran con esa unidad.

### Búsquedas y duplicados

- Registrar `alpha` cuando ya existe `Alpha` devuelve “Ese equipo ya existe.”.
- Buscar `alpha` o ` alpha ` permite encontrar `Alpha`.
- Registrar `alias` en un equipo que ya contiene `Alias` devuelve “Ya existe ese alias en el equipo.”.
- El alias `Alias` se aceptó en otro equipo.
- La duplicación de equipo se detecta antes de pedir fecha y categoría.
- La duplicación de alias se detecta después de nombre y alias, antes de edad, seguidores y valoración.

Estas pruebas respaldan comparaciones sin distinción de mayúsculas/minúsculas para los casos ensayados. No determinan reglas para tildes, normalización Unicode ni equivalencia cultural de todos los caracteres.

## Comportamiento de las operaciones

### 1. Crear competencia

Solicita nombre, patrocinador, año y máximo de equipos. Finaliza con “Competencia creada.”.

Si existe una competencia, pregunta “¿Reemplazar la competencia actual y sus datos? (S/N):”. El contexto registra que cualquier tecla distinta de `s` cancela; en la prueba nueva se confirmó con `s`. La confirmación consume una tecla, sin necesitar Enter.

Se comprobó que recrear una competencia deja la nueva competencia activa y que posteriormente la lectura puede recuperar la anterior guardada. No se ha comprobado por separado la tecla `S` mayúscula.

### 2. Registrar equipo

Requiere competencia activa. Si no existe, muestra “Primero tenés que crear o leer una competencia.”.

Solicita nombre, fecha y categoría. El contexto confirma rechazo de nombres repetidos y, al alcanzar el máximo, “No quedan cupos.”. Un alta correcta muestra “Equipo registrado.”.

### 3. Registrar creador en un equipo

Sin competencia, presenta el mismo aviso general. Con competencia pero sin equipos, igualmente solicita nombre del equipo; ante un nombre ausente muestra “No se encontró el equipo.”.

Con equipo válido solicita nombre, alias, edad, seguidores, valoración y especialidad. Finaliza con “Creador registrado.”. No se observó un límite de creadores por equipo; no debe inventarse uno sin evidencia.

### 4. Mostrar información de la competencia

Presenta nombre, patrocinador, año y ocupación, por ejemplo `Cupos: 2/10`: equipos registrados sobre máximo permitido.

Muestra los equipos y sus creadores. En las altas manuales se conservó el orden de registro. Cada creador incluye sus datos y el resultado “Estrella: Sí/No”.

### 5. Mostrar información de un equipo

Busca por nombre. Un nombre inexistente devuelve “No se encontró el equipo.”. Un equipo encontrado muestra fecha, categoría y listado de creadores, según las transcripciones previas.

La prueba nueva confirmó que el encabezado correcto sin competencia es “MOSTRAR INFORMACIÓN DE UN EQUIPO”; el encabezado de creador copiado en ese apartado del contexto no corresponde a la pantalla observada.

### 6. Mostrar información de un creador

Solicita equipo y luego alias. Según el contexto, un alias ausente muestra “No se encontró el creador.” y un alias encontrado muestra su ficha completa.

Sin competencia muestra el aviso general. No se comprobó específicamente en esta sesión la búsqueda individual de alias cambiando mayúsculas: la prueba de alias sin distinción de caja se realizó sobre el alta duplicada.

### 7. Mostrar estadísticas de un equipo

Calcula valoración promedio, cantidad de streamers, total de seguidores y creador más popular.

**Corrección del contexto:** un equipo existente sin creadores no se trata como inexistente. Para Alpha vacío se obtuvo:

```text
Equipo: Alpha
Valoración promedio: 0,00
Cantidad de streamers: 0
Total seguidores: 0 mil
Creador más popular: Sin creadores
```

Para Alpha con ocho creadores, las valoraciones fueron 8, 8, 7, 8, 9, 8, 7 y 10. Los resultados fueron:

- Promedio: `65 / 8 = 8,125`, presentado como `8,13`.
- Seguidores: `0 + 500 + 1000 + 1000 + 500 + 499 + 500 + 499 = 4498` mil.
- Cantidad de streamers: 4, contando solamente la especialidad Streamer.
- Más popular: Cuatro, con 1000 mil seguidores, registrado antes que Cinco, también con 1000.

La popularidad se decide por seguidores; en ese empate conservó al primer creador registrado, aunque el segundo tenía mayor valoración. El ejemplo de redondeo no permite deducir todos los casos posibles de formato numérico.

### 8. Comparar valoración de dos equipos

Valida el primer equipo antes de solicitar el segundo. Si alguno no existe, muestra “No se encontró el equipo.”.

Casos nuevos comprobados:

| Situación | Resultado |
| --- | --- |
| Alpha contra Alpha | “Elegí dos equipos diferentes.” |
| Gamma y Delta, ambos sin creadores | “Ambos equipos deben tener creadores.” |
| Alpha y Beta con promedio 8 | Muestra ambos promedios y “¡Empate!”. |
| Sin competencia | Aviso general de crear o leer competencia. |

El contexto documenta que, con promedios distintos, anuncia “Ganador:” seguido del equipo con mayor promedio. La comparación no utiliza el total de seguidores. Queda pendiente probar específicamente un solo equipo vacío frente a otro con creadores.

### 9. Mostrar el equipo con más seguidores

Selecciona por la suma de seguidores de los creadores de cada equipo.

| Situación comprobada | Resultado |
| --- | --- |
| Sin competencia | Aviso general. |
| Competencia sin equipos | “Todavía no hay equipos con creadores.” |
| Un único equipo vacío | El mismo mensaje. |
| Alpha y Beta, ambos con creadores y cero seguidores | “Alpha: 0 mil seguidores.” |
| Cuatro equipos con 500 mil cada uno, cargados Zeta, Alfa, Medio, Ultimo | “Zeta: 500 mil seguidores.” |

Los ensayos de empate devolvieron el primer equipo en el orden de registro/carga, tanto con cero como con 500 mil seguidores. Este criterio difiere del desempate observado para el TOP 3.

### 10. Guardar datos

Sin competencia muestra el aviso general. Con competencia crea la carpeta `Datos` y escribe `competencia.txt`, `equipos.txt` y `creadores.txt`, indicando su ubicación.

En la copia temporal se guardó una competencia con acentos y un punto y coma en su nombre. Luego se reemplazó la competencia en memoria y se recuperaron correctamente los datos guardados, incluyendo un creador con 2147483647 mil seguidores.

### 11. Leer datos

Sin competencia activa se intentó leer directamente, sin confirmación. Si ya hay competencia pregunta “¿Reemplazar los datos en memoria por los archivos? (S/N):”. El contexto documenta cancelación; las nuevas lecturas se confirmaron con `s`.

Una lectura correcta muestra “Datos recuperados: N equipos.” y reemplaza el estado activo.

| Prueba sobre copia temporal | Resultado |
| --- | --- |
| Carpeta sin archivos de datos | “Error: Falta el archivo competencia.txt.” |
| `competencia.txt` con Base64 inválido | Mensaje “Error:” con detalle de Base64; vuelve al menú. |
| `equipos.txt` ausente | “Error: Falta el archivo equipos.txt.” |
| Consultar después de los dos errores anteriores | Se conservó la competencia, el equipo y el creador previamente cargados. |

La conservación de memoria está comprobada para esos errores concretos; no demuestra que cualquier fallo a mitad de lectura sea seguro.

### 12. Salir

Muestra “TOP 3 DE STREAMORT” y espera una tecla antes de cerrar.

- Sin competencia muestra “No hay equipos con creadores para armar el ranking.”.
- Con Alpha y Beta con creadores, más Gamma y Delta vacíos, mostró solamente Alpha y Beta.
- Se ordena por promedio de valoración descendente: Alpha 8,13 precedió a Beta 8,00.
- Con cuatro equipos empatados en 8, cargados en orden Zeta, Alfa, Medio, Ultimo, mostró Alfa, Medio y Ultimo. El resultado respalda un desempate alfabético para esos nombres, y un máximo de tres puestos.
- No se observó una solicitud de guardado al salir. La primera sesión terminó sin guardar y no alteró los archivos de datos originales.

Queda pendiente determinar el orden de desempate para mayúsculas, tildes y otros caracteres.

## Condición «Estrella»

Se variaron seguidores, valoración y especialidad. Estos resultados amplían los ejemplos del contexto:

| Seguidores en miles | Valoración | Especialidad ensayada | Estrella |
| --- | --- | --- | --- |
| 0 | 8 | Streamer | No |
| 499 | 8 | Streamer | No |
| 499 | 10 | Influencer | No |
| 500 | 7 | Streamer | No |
| 500 | 8 | Tiktoker | Sí |
| 500 | 9 | Youtuber | Sí |
| 1000 | 7 | Influencer | No |
| 1000 | 8 | Streamer | Sí |
| 2147483647 | 10 | Influencer | Sí |

**Regla respaldada por las pruebas de frontera:** un creador es estrella cuando tiene **seguidores >= 500 y valoración >= 8**. Se observaron resultados positivos en las cuatro especialidades. Es una regla funcional deducida de pruebas de caja negra; no se inspeccionó su implementación.

## Persistencia original

Los archivos son texto delimitado por punto y coma, sin encabezados en las muestras. Los campos textuales se codifican en Base64 de UTF-8; esto no constituye cifrado.

| Archivo | Campos en orden |
| --- | --- |
| `competencia.txt` | Nombre en Base64; patrocinador en Base64; año; máximo de equipos |
| `equipos.txt` | Nombre en Base64; fecha `yyyy-MM-dd`; categoría en Base64 |
| `creadores.txt` | Equipo en Base64; nombre en Base64; alias en Base64; edad; seguidores; valoración; especialidad en Base64 |

El archivo de creadores relaciona cada creador con un equipo mediante el nombre de ese equipo. El indicador Estrella no figura como campo: debe derivarse de seguidores y valoración.

Ejemplo nuevo guardado:

```text
Q29wYSDDsTtPUlQ=;U3BvbnNvcg==;2027;1
```

Representa `Copa ñ;ORT`, patrocinador `Sponsor`, año 2027 y cupo 1. El punto y coma pertenece al nombre decodificado, no es un separador adicional.

Para la reconstrucción se debe usar **DataBases**, como exige el contexto, conservando campos y relaciones sin reproducir obligatoriamente Base64. La lectura de archivos del original sería una función adicional de importación, no un requisito demostrado de la reconstrucción.

## Orientación de diseño, sin implementar

A partir de las operaciones observadas resulta razonable distribuir responsabilidades así:

| Clase propuesta | Responsabilidad |
| --- | --- |
| `Competencia` | Datos generales, lista de equipos, cupos, búsqueda y alta de equipos, comparación, equipo con más seguidores y TOP 3. |
| `Equipo` | Datos del equipo, lista de creadores, búsqueda por alias, alta y estadísticas. |
| `Creador` | Datos personales, especialidad y cálculo de Estrella. |
| `Datos` | Guardar y recuperar el conjunto completo mediante DataBases. |
| `Program` | Navegación, solicitudes de datos, confirmaciones y presentación de resultados. |

Esta es una **propuesta de reconstrucción**, no una identificación de las clases internas del ejecutable. Los constructores deben inicializar atributos; las listas deben contener objetos y las clases de dominio devolver resultados sin imprimir.

El siguiente paso de diseño es desarrollar `IA/Arquitectura.md` con atributos, firmas y UML. Este análisis no implementa métodos ni modifica las bibliotecas.

## Casos pendientes y criterios para futuras pruebas

No se deben convertir estos puntos en reglas supuestas:

1. Textos formados solo por espacios en cada campo; límites de longitud y espacios interiores.
2. Fechas inexistentes, año bisiesto, fecha de hoy y mañana según el reloj del sistema.
3. Confirmaciones con `S` mayúscula y otras teclas en ambas operaciones.
4. Comparación de un equipo vacío con otro no vacío.
5. Suma de seguidores superior a `int.MaxValue`: se aceptó ese máximo en un creador, pero no se comprobó el tipo o el desbordamiento del total.
6. Archivos vacíos, columnas faltantes, números inválidos, filas duplicadas y creadores con equipos inexistentes.
7. Ausencia de `creadores.txt`, permisos de escritura denegados y guardado interrumpido.
8. Reemplazo de un guardado grande por otro menor para descartar filas residuales.
9. Lectura corrupta después de procesar filas válidas y conservación del estado ante ese fallo.
10. Desempates con caracteres acentuados, nombres que difieren solo en caja y promedios distintos que se muestran redondeados igual.
11. Ejecución sobre .NET 8 para confirmar que el cambio de runtime no afecta los resultados.

Las pruebas unitarias de la futura reconstrucción deberían cubrir especialmente duplicados por equipo, cupos, promedio vacío, conteo por especialidad, frontera de Estrella, empates y guardado/lectura sin pérdida de información.

## Registro de uso de IA y verificación de esta entrega

**Consulta del usuario:** “Analiza el ejecutable, pruebalo. Tienes en contexto.md una gran cantidad de resultado de las pruebas. En base a la informacion ya recopilada y a la que consiguieras probando el ejecutable, genera un archivo Analisis.md”. Durante el trabajo agregó: “Sigue”.

**Respuesta y trabajo realizado:** se revisó el contexto, se ejecutó el ensamblado recuperado en terminal interactiva, se investigaron casos faltantes y se elaboró este documento junto con el registro complementario de capturas. Estos archivos pueden incorporarse al Google Docs exigido por la consigna; no se creó ni publicó un documento externo.

Se intentó `dotnet build "ACTIVIDAD ESPECIAL.sln" --no-restore`. Falló por una referencia de NuGet a una carpeta de reserva inexistente: `C:\Program Files (x86)\Microsoft Visual Studio\Shared\NuGetPackages`. El cambio entregado es documental y no modifica código C#; no se cambió la configuración del proyecto para corregir ese problema previo del entorno.
