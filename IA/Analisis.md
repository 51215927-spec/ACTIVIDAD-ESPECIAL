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

Esta adaptación se usó en la exploración inicial sobre .NET 10. En la ampliación del 02/11/2026 se instaló .NET 8.0.33 x64 localmente en `IA/Investigacion/herramientas/dotnet8`, sin alterar el runtime global, y se ejecutaron once grupos sobre .NET 8 y un grupo de regresión sobre .NET 10.0.12. La regresión de estadísticas, Estrella y ranking produjo salidas iguales en ambos runtimes; esto no demuestra equivalencia para toda entrada posible. Procedimiento y evidencia: [Investigacion/README.md](Investigacion/README.md).

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
| Valoración | Número con decimales entre 1 y 10 | Ampliación: `8,5`, `8.5`, `8,001`, `8.004` y `7,999` aceptados. La caracterización anterior como entero era incorrecta. |
| Categoría/especialidad | Selección numérica entre 1 y 4 | Contexto y pruebas nuevas. |

El contexto muestra rechazo de letras y números fuera de rango con “Ingresá un número entero válido.”, seguido de una nueva solicitud del mismo campo. Para valoración, la ampliación comprobó “Ingresá un decimal válido (por ejemplo 8,5).”. Para fechas inválidas muestra “Fecha inválida o futura.”.

Las categorías son Gaming, Humor, Música y Entretenimiento. Las especialidades son Streamer, Youtuber, Tiktoker e Influencer.

Los seguidores se almacenan y muestran en miles: 500 equivale a 500 mil seguidores. No se debe multiplicar por 1000 al calcular los totales que se muestran con esa unidad.

### Búsquedas y duplicados

- Registrar `alpha` cuando ya existe `Alpha` devuelve “Ese equipo ya existe.”.
- Buscar `alpha` o ` alpha ` permite encontrar `Alpha`.
- Registrar `alias` en un equipo que ya contiene `Alias` devuelve “Ya existe ese alias en el equipo.”.
- El alias `Alias` se aceptó en otro equipo.
- La duplicación de equipo se detecta antes de pedir fecha y categoría.
- La duplicación de alias se detecta después de nombre y alias, antes de edad, seguidores y valoración.

La ampliación confirmó búsqueda individual por equipo y alias sin distinguir mayúsculas en los ejemplos `ÁGUILA  AZUL` y `áLIAS  DOBLE`. Conserva los espacios interiores: `Águila Azul` no encuentra `Águila  Azul`. `Aguila` no equivale a `Águila`, y una `A` seguida del acento combinante no encontró el nombre almacenado con `Á` precompuesta; ocurrió lo mismo con el alias. No se deduce de estos casos una regla universal para todos los caracteres Unicode.

## Comportamiento de las operaciones

### 1. Crear competencia

Solicita nombre, patrocinador, año y máximo de equipos. Finaliza con “Competencia creada.”.

Si existe una competencia, pregunta “¿Reemplazar la competencia actual y sus datos? (S/N):”. Confirman `s` y `S`, sin necesitar Enter. En la ampliación, `n`, `x` y Enter cancelaron y conservaron la competencia y sus datos. La afirmación anterior de que toda tecla distinta de `s` cancela omitía la `S` mayúscula.

Se comprobó que recrear una competencia deja la nueva competencia activa sin los equipos anteriores y que posteriormente la lectura puede recuperar la anterior guardada.

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

Sin competencia muestra el aviso general. La ampliación comprobó también la búsqueda individual de alias cambiando mayúsculas, con los límites Unicode descritos arriba.

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

El contexto documenta que, con promedios distintos, anuncia “Ganador:” seguido del equipo con mayor promedio. La comparación no utiliza el total de seguidores. La ampliación comprobó el mensaje “Ambos equipos deben tener creadores.” con un equipo vacío frente a otro con creadores, en ambos órdenes.

Con Zeta en 8,004 y Alfa en 8,001 mostró `Zeta: 8,00 | Alfa: 8,00`, pero anunció `Ganador: Zeta`. Compara los valores antes de redondearlos para presentar; el TOP 3 también colocó Zeta primero.

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

La ampliación comprobó reemplazo de tres equipos y dos creadores por una competencia sin equipos: los archivos de equipos y creadores quedaron sin filas, con BOM UTF-8, y la lectura posterior recuperó cero equipos. No quedaron filas residuales.

**Fallo de escritura comprobado:** con `creadores.txt` de solo lectura, guardar una competencia distinta falló con acceso denegado. `competencia.txt` y `equipos.txt` ya habían sido reemplazados y `creadores.txt` conservaba los creadores anteriores. La memoria siguió con la competencia nueva; el guardado en disco quedó inconsistente. También se ensayó un directorio ocupando el nombre `creadores.txt`, con error de acceso. Por tanto, no debe atribuirse al original un guardado atómico. Proteger la copia completa en la reconstrucción es una decisión de robustez requerida por el plan.

### 11. Leer datos

Sin competencia activa se intentó leer directamente, sin confirmación. Si ya hay competencia pregunta “¿Reemplazar los datos en memoria por los archivos? (S/N):”. Se comprobaron `s` y `S` para confirmar; `n`, `x` y Enter cancelan y conservan memoria.

Una lectura correcta muestra “Datos recuperados: N equipos.” y reemplaza el estado activo.

| Prueba sobre copia temporal | Resultado |
| --- | --- |
| Carpeta sin archivos de datos | “Error: Falta el archivo competencia.txt.” |
| `competencia.txt` con Base64 inválido | Mensaje “Error:” con detalle de Base64; vuelve al menú. |
| `equipos.txt` ausente | “Error: Falta el archivo equipos.txt.” |
| Consultar después de los dos errores anteriores | Se conservó la competencia, el equipo y el creador previamente cargados. |

La ampliación añadió 22 variantes de archivos; la tabla de resultados está más abajo. En particular, una fila válida seguida de otra con Base64 corrupto, tanto en equipos como en creadores, dejó intacta la competencia previa en memoria. La garantía se limita a los fallos ensayados; no demuestra seguridad frente a cualquier fallo del sistema.

### 12. Salir

Muestra “TOP 3 DE STREAMORT” y espera una tecla antes de cerrar.

- Sin competencia muestra “No hay equipos con creadores para armar el ranking.”.
- Con Alpha y Beta con creadores, más Gamma y Delta vacíos, mostró solamente Alpha y Beta.
- Se ordena por promedio de valoración descendente: Alpha 8,13 precedió a Beta 8,00.
- Con cuatro equipos empatados en 8, cargados en orden Zeta, Alfa, Medio, Ultimo, mostró Alfa, Medio y Ultimo. El resultado respalda un desempate alfabético para esos nombres, y un máximo de tres puestos.
- No se observó una solicitud de guardado al salir. La primera sesión terminó sin guardar y no alteró los archivos de datos originales.

En la ampliación, con seis equipos empatados cargados `Zeta`, `Alfa`, `águila`, `Aguila`, `beta`, `Águila` (el último con acento combinante), el TOP 3 fue `Aguila`, `águila`, `Águila`. Se registra el orden concreto observado, sin identificar por caja negra un comparador cultural universal. No puede ensayarse un empate entre nombres que solo difieren en mayúsculas dentro de la misma competencia: el alta/lectura los rechaza como duplicados.

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

La valoración no se redondea antes de decidir Estrella: con 500 mil seguidores, `7,999` se muestra como `8` en la ficha, pero es `Estrella: No`; `8,001` se muestra como `8` y es `Estrella: Sí`.

## Persistencia original

Los archivos son texto delimitado por punto y coma, sin encabezados en las muestras. Los campos textuales se codifican en Base64 de UTF-8; esto no constituye cifrado.

| Archivo | Campos en orden |
| --- | --- |
| `competencia.txt` | Nombre en Base64; patrocinador en Base64; año; máximo de equipos |
| `equipos.txt` | Nombre en Base64; fecha `yyyy-MM-dd`; categoría en Base64 |
| `creadores.txt` | Equipo en Base64; nombre en Base64; alias en Base64; edad; seguidores; valoración; especialidad en Base64 |

El archivo de creadores relaciona cada creador con un equipo mediante el nombre de ese equipo. El indicador Estrella no figura como campo: debe derivarse de seguidores y valoración.

Las valoraciones se guardan con punto decimal (`8.5`, `8.001`, `8.004`, `7.999`) y se recuperan sin perder la distinción. La ficha puede mostrarlas redondeadas; no debe reconstruirse el valor persistido a partir de ese texto de presentación.

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

El siguiente paso de diseño es desarrollar `IA/Arquitectura.md` con atributos, firmas y responsabilidades. Por indicación expresa del usuario, no se requiere UML. Este análisis no implementa métodos ni modifica las bibliotecas.

## Ampliación de pruebas del 02/11/2026

Evidencia reproducible: [Investigacion/README.md](Investigacion/README.md), entradas y salidas de sus doce grupos y [resultados.json](Investigacion/resultados.json). Once grupos usan .NET 8.0.33 y uno repite una regresión sobre .NET 10.0.12. Son pruebas de caja negra; los archivos preparados siguen el formato del original y se cargan por su menú.

### Entradas, fechas y estados

| Caso | Resultado observado |
| --- | --- |
| Opciones 2 a 10 sin competencia | Todas muestran el aviso de crear o leer antes de operar. La lectura inicial sí funciona. |
| Campos de nombre de competencia, patrocinador, equipo, creador y alias vacíos o solo con espacios | Rechazados; vuelve a pedir el campo. |
| Espacios exteriores e interiores | Recorta los exteriores; conserva los dobles espacios interiores en datos y búsquedas. |
| Textos de 256 caracteres | Aceptados en nombre de competencia, nombre de equipo, nombre de creador y alias. No se determinó un máximo universal; no agregar un límite arbitrario a partir de esta muestra. |
| Año 1999, 2028; cupo 0, -1, 10001; letras y entero desbordado | Rechazados. Se aceptaron 2000, 2027 y cupo 10000; una competencia con cupo 1 rechazó el segundo equipo antes de solicitar sus datos. |
| 31/02/2020, 29/02/2023, 1/1/2000, 2000-01-01 | Rechazados por fecha inválida o formato no admitido. |
| 29/02/2024 y hoy 02/11/2026 | Aceptados. Mañana 03/11/2026 fue rechazado. |
| Edad 13/31; seguidores -1/2147483648/1,5; valoración 0/11/letras | Rechazados; edades 14/30 y seguidores 0/2147483647 aceptados. |
| Categoría/especialidad fuera de 1–4 o letras | Rechazadas según los ensayos de cada selección. |
| Sin equipos | Las consultas/búsquedas ensayadas solicitan equipo y responden que no existe. Sin seguidores, informa que no hay equipos con creadores. |
| Equipo existente sin creadores | Se muestra su información; buscar un alias falla como creador inexistente; estadísticas en cero y ranking sin participantes. |
| Total 2147483647 + 1 | `2147483648 mil`, tanto en estadísticas como al elegir el equipo con más seguidores. El total requiere una representación mayor que `int`; usar `long` es una propuesta de implementación, no identificación del tipo interno original. |

### Matriz de lectura y conservación de memoria

Antes de cada variante se restableció una competencia válida con Alpha, Vacio y el creador Uno en Alpha. Tras la lectura se consultó la información activa.

| Variante | Resultado |
| --- | --- |
| competencia.txt vacío | Error de archivo vacío; memoria anterior conservada. |
| equipos.txt vacío con creadores que lo referencian | Error por equipo inexistente; memoria anterior conservada. |
| equipos.txt y creadores.txt sin filas | Recupera cero equipos correctamente (grupo de estados vacíos). |
| creadores.txt vacío | Recupera dos equipos, ambos sin creadores. |
| creadores.txt ausente | Error por archivo faltante; memoria anterior conservada. |
| Columnas faltantes en competencia/equipo/creador | Errores de datos/fila inválida; memoria anterior conservada. |
| Año no numérico, año 1999 o cupo menor que los equipos | Error de formato, año inválido o cupos excedidos; memoria anterior conservada. |
| Equipos Alpha/alpha o alias Uno/uno en Alpha | Error de duplicado; memoria anterior conservada. |
| Creador de Fantasma | Error de equipo inexistente; memoria anterior conservada. |
| Fecha imposible, categoría u especialidad desconocida | Error de validación; memoria anterior conservada. |
| Seguidores negativos, valoración 11 o edad no numérica | Error de validación/formato; memoria anterior conservada. |
| Fila válida seguida de Base64 corrupto en equipos o creadores | Error; memoria anterior conservada íntegra, sin incorporar el equipo Nuevo o creador Nuevo. |
| Líneas en blanco antes/después del creador | Se ignoran; carga correcta. |

### Comparación entre runtimes

La misma competencia con tres equipos y nueve creadores produjo resultados idénticos en .NET 8.0.33 y .NET 10.0.12: Alpha con promedio 8,13, cuatro Streamers, 4498 mil seguidores y el primer creador entre los empatados como más popular; Beta con 4498 mil y promedio 8,00; Gamma vacío con ceros. Ganó Alpha en seguidores por orden de carga y el TOP incluyó Alpha y Beta. La igualdad se comprobó sobre los siete resultados normalizados de cada sesión, incluida la información de Estrella.

### Límites que no deben convertirse en reglas inventadas

- No se determinó el máximo absoluto de longitud ni se recorrió todo Unicode. Se conserva la evidencia de los casos concretos.
- El orden Unicode registrado no identifica el comparador interno ni garantiza idéntico orden en todas las culturas o sistemas.
- Se forzaron fallos de escritura reproducibles, incluido un archivo de solo lectura; no se simuló un corte de energía o cierre arbitrario del sistema.
- No se probó toda combinación de las doce opciones y todos los datos posibles. Los escenarios pendientes que afectaban el diseño se ampliaron y sus límites quedaron explícitos; pueden planificarse más ensayos si surge una duda concreta al implementar.

Las pruebas unitarias de la futura reconstrucción deberían cubrir especialmente duplicados por equipo, cupos, promedio vacío, conteo por especialidad, frontera de Estrella, empates y guardado/lectura sin pérdida de información.

## Registro de uso de IA y verificación de esta entrega

**Consulta del usuario:** “Analiza el ejecutable, pruebalo. Tienes en contexto.md una gran cantidad de resultado de las pruebas. En base a la informacion ya recopilada y a la que consiguieras probando el ejecutable, genera un archivo Analisis.md”. Durante el trabajo agregó: “Sigue”.

**Respuesta y trabajo realizado:** se revisó el contexto, se ejecutó el ensamblado recuperado en terminal interactiva, se investigaron casos faltantes y se elaboró este documento junto con el registro complementario de capturas. Estos archivos pueden incorporarse al Google Docs exigido por la consigna; no se creó ni publicó un documento externo.

Se intentó `dotnet build "ACTIVIDAD ESPECIAL.sln" --no-restore`. Falló por una referencia de NuGet a una carpeta de reserva inexistente: `C:\Program Files (x86)\Microsoft Visual Studio\Shared\NuGetPackages`. El cambio entregado es documental y no modifica código C#; no se cambió la configuración del proyecto para corregir ese problema previo del entorno.
