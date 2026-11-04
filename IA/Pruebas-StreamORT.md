# Capturas complementarias de pruebas de StreamORT

Fecha: 02/11/2026. Complemento de [Analisis.md](Analisis.md).

Se eliminaron secuencias ANSI y se redujeron líneas vacías. Las capturas de una terminal que redibuja la pantalla pueden juntar mensajes o alterar su distribución; no representan una transcripción visual exacta. No incluyen las primeras pruebas exploratorias, resumidas en el análisis. Los archivos de persistencia se manipularon solamente en la copia temporal. El caso de cuatro equipos empatados se preparó por archivos y se cargó desde el menú.

## comparar mismo vacio

```text
============================================
  STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS
============================================

Primer equipo: Alpha
Segundo equipo: Alpha
Elegí dos equipos diferentes.

Presioná cualquier tecla para volver al menú...
```

## mas seguidores unico vacio

```text
============================================
  STREAMORT | MOSTRAR EL EQUIPO CON MÁS SEGUIDORES
============================================

Todavía no hay equipos con creadores.

Presioná cualquier tecla para volver al menú...
```

## duplicado mayusculas

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Nombre del equipo: alpha
Ese equipo ya existe.

Presioná cualquier tecla para volver al menú...
```

## crear Beta

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Nombre del equipo: Beta
Fecha de creación (dd/MM/yyyy): 01/01/2000
Categoría:
1. Gaming
2. Humor
3. Música
4. Entretenimiento
Opción (1 a 4): 2
Equipo registrado.

Presioná cualquier tecla para volver al menú...
```

## creador Alpha

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo:  alpha
Nombre del creador: Uno
Alias: Alias
Edad (14 a 30): 14
Seguidores (en miles) (0 a 2147483647): 0
Valoración (1 a 10): 8
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 1

Creador registrado.Presioná cualquier tecla para volver al menú...
```

## estadisticas Alpha

```text
============================================
  STREAMORT | MOSTRAR ESTADÍSTICAS DE UN EQUIPO
============================================

Nombre del equipo: alpha
Equipo: Alpha
Valoración promedio: 8,00
Cantidad de streamers: 1

Total seguidores: 0 mil
Creador más popular: Uno (@Alias) | Edad: 14 | Seguidores: 0 mil | Valoración: 8 | Especialidad: Streamer | Estrella: NoPresioná cualquier tecla para volver al menú...
```

## duplicado alias

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Otro
Alias: alias
Ya existe ese alias en el equipo.

Presioná cualquier tecla para volver al menú...
```

## alias otro equipo

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Beta
Nombre del creador: Dos
Alias: Alias
Edad (14 a 30): 30
Seguidores (en miles) (0 a 2147483647): 0
Valoración (1 a 10): 8
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 2
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## comparacion empate

```text
============================================
  STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS
============================================

Primer equipo: Alpha
Segundo equipo: Beta
Alpha: 8,00 | Beta: 8,00
¡Empate!

Presioná cualquier tecla para volver al menú...
```

## ganador cero

```text
============================================
  STREAMORT | MOSTRAR EL EQUIPO CON MÁS SEGUIDORES
============================================

Alpha: 0 mil seguidores.

Presioná cualquier tecla para volver al menú...
```

## altas estrella

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Tres
Alias: a500v8
Edad (14 a 30): 20
Seguidores (en miles) (0 a 2147483647): 500
Valoración (1 a 10): 8
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 3
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## alta 1000v7

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Cuatro
Alias: a1000v7
Edad (14 a 30): 20
Seguidores (en miles) (0 a 2147483647): 1000
Valoración (1 a 10): 7
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 4
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## alta 1000v8

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Cinco
Alias: a1000v8
Edad (14 a 30): 20
Seguidores (en miles) (0 a 2147483647): 1000
Valoración (1 a 10): 8
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 1
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## alta 500v9

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Seis
Alias: a500v9
Edad (14 a 30): 20
Seguidores (en miles) (0 a 2147483647): 500
Valoración (1 a 10): 9
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 2
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## info

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================

2000 | Sponsor: 10 | Año: 2000 | Cupos: 2/10
Equipo: Alpha | Creación: 01/01/2000 | Categoría: Gaming
Creadores (5):
  - Uno (@Alias) | Edad: 14 | Seguidores: 0 mil | Valoración: 8 | Especialidad: Streamer | Estrella: No
  - Tres (@a500v8) | Edad: 20 | Seguidores: 500 mil | Valoración: 8 | Especialidad: Tiktoker | Estrella: Sí
  - Cuatro (@a1000v7) | Edad: 20 | Seguidores: 1000 mil | Valoración: 7 | Especialidad: Influencer | Estrella: No
  - Cinco (@a1000v8) | Edad: 20 | Seguidores: 1000 mil | Valoración: 8 | Especialidad: Streamer | Estrella: Sí
  - Seis (@a500v9) | Edad: 20 | Seguidores: 500 mil | Valoración: 9 | Especialidad: Youtuber | Estrella: SíEquipo: Beta | Creación: 01/01/2000 | Categoría: Humor
Creadores (1):
  - Dos (@Alias) | Edad: 30 | Seguidores: 0 mil | Valoración: 8 | Especialidad: Youtuber | Estrella: NoPresioná cualquier tecla para volver al menú...
```

## 499v8

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Siete
Alias: a499v8
Edad (14 a 30): 20
Seguidores (en miles) (0 a 2147483647): 499
Valoración (1 a 10): 8
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 1
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## 500v7

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Ocho
Alias: a500v7
Edad (14 a 30): 20
Seguidores (en miles) (0 a 2147483647): 500
Valoración (1 a 10): 7
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 1
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## 499v10

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Alpha
Nombre del creador: Nueve
Alias: a499v10
Edad (14 a 30): 20
Seguidores (en miles) (0 a 2147483647): 499
Valoración (1 a 10): 10
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 4
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## detalle frontera

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================

2000 | Sponsor: 10 | Año: 2000 | Cupos: 2/10
Equipo: Alpha | Creación: 01/01/2000 | Categoría: Gaming
Creadores (8):
  - Uno (@Alias) | Edad: 14 | Seguidores: 0 mil | Valoración: 8 | Especialidad: Streamer | Estrella: No
  - Tres (@a500v8) | Edad: 20 | Seguidores: 500 mil | Valoración: 8 | Especialidad: Tiktoker | Estrella: Sí
  - Cuatro (@a1000v7) | Edad: 20 | Seguidores: 1000 mil | Valoración: 7 | Especialidad: Influencer | Estrella: No
  - Cinco (@a1000v8) | Edad: 20 | Seguidores: 1000 mil | Valoración: 8 | Especialidad: Streamer | Estrella: Sí
  - Seis (@a500v9) | Edad: 20 | Seguidores: 500 mil | Valoración: 9 | Especialidad: Youtuber | Estrella: Sí
  - Siete (@a499v8) | Edad: 20 | Seguidores: 499 mil | Valoración: 8 | Especialidad: Streamer | Estrella: No
  - Ocho (@a500v7) | Edad: 20 | Seguidores: 500 mil | Valoración: 7 | Especialidad: Streamer | Estrella: No
  - Nueve (@a499v10) | Edad: 20 | Seguidores: 499 mil | Valoración: 10 | Especialidad: Influencer | Estrella: No

Equipo: Beta | Creación: 01/01/2000 | Categoría: Humor
Creadores (1):
  - Dos (@Alias) | Edad: 30 | Seguidores: 0 mil | Valoración: 8 | Especialidad:
 Youtuber | Estrella: No

Presioná cualquier tecla para volver al menú...
```

## estadisticas empate popular

```text
============================================
  STREAMORT | MOSTRAR ESTADÍSTICAS DE UN EQUIPO
============================================

Nombre del equipo: Alpha
Equipo: Alpha
Valoración promedio: 8,13
Cantidad de streamers: 4
Total seguidores: 4498 mil
Creador más popular: Cuatro (@a1000v7) | Edad: 20 | Seguidores: 1000 mil | Valoración: 7 | Especialidad: Influencer | Estrella: No

Presioná cualquier tecla para volver al menú...
```

## Gamma vacio

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Nombre del equipo: Gamma
Fecha de creación (dd/MM/yyyy): 01/01/2000
Categoría:
1. Gaming
2. Humor
3. Música
4. Entretenimiento
Opción (1 a 4): 3
Equipo registrado.

Presioná cualquier tecla para volver al menú...
```

## Delta vacio

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Nombre del equipo: Delta
Fecha de creación (dd/MM/yyyy): 01/01/2000
Categoría:
1. Gaming
2. Humor
3. Música
4. Entretenimiento
Opción (1 a 4): 4
Equipo registrado.

Presioná cualquier tecla para volver al menú...
```

## comparar vacios

```text
============================================
  STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS
============================================

Primer equipo: Gamma
Segundo equipo: Delta
Ambos equipos deben tener creadores.

Presioná cualquier tecla para volver al menú...
```

## salir top3

```text
=== TOP 3 DE STREAMORT ===

1. Alpha - Promedio: 8,13
2. Beta - Promedio: 8,00¡Hasta la próxima! Presioná una tecla para cerrar.
```

## leer ausentes

```text
============================================
  STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================

Error: Falta el archivo competencia.txt.Presioná cualquier tecla para volver al menú...
```

## crear textos espacios

```text
============================================
  STREAMORT | CREAR COMPETENCIA
============================================

Nombre de la competencia:   Copa ñ;ORT
Patrocinador:   Sponsor

Año (2000 a 2027): 2027
Máximo de equipos (1 a 10000): 1
Competencia creada.Presioná cualquier tecla para volver al menú...
```

## equipo

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Nombre del equipo:   Equipo Á
Fecha de creación (dd/MM/yyyy): 01/01/2000
Categoría:
1. Gaming
2. Humor
3. Música
4. Entretenimiento
Opción (1 a 4): 1
Equipo registrado.

Presioná cualquier tecla para volver al menú...
```

## creador

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: Equipo Á
Nombre del creador:   Nombre Ñ
Alias:   alias
Edad (14 a 30): 30
Seguidores (en miles) (0 a 2147483647): 2147483647
Valoración (1 a 10): 10
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 4
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

## guardar

```text
============================================
  STREAMORT | GUARDAR DATOS DE LA COMPETENCIA
============================================

Guardados competencia.txt, equipos.txt y creadores.txt en:
C:\Users\julian\AppData\Local\Temp\StreamORT-Analisis-20261102\DatosPresioná cualquier tecla para volver al menú...
```

## recrear

```text
============================================
  STREAMORT | CREAR COMPETENCIA
============================================

¿Reemplazar la competencia actual y sus datos? (S/N): Nombre de la competencia: Otra
Patrocinador: Otro
Año (2000 a 2027): 2000
Máximo de equipos (1 a 10000): 1
Competencia creada.

Presioná cualquier tecla para volver al menú...
```

## leer con confirmacion

```text
============================================
  STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================

¿Reemplazar los datos en memoria por los archivos? (S/N):

Datos recuperados: 1 equipos.

Presioná cualquier tecla para volver al menú...
```

## mostrar recuperados

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================

Copa ñ;ORT | Sponsor: Sponsor | Año: 2027 | Cupos: 1/1
Equipo: Equipo Á | Creación: 01/01/2000 | Categoría: Gaming
Creadores (1):
  - Nombre Ñ (@alias) | Edad: 30 | Seguidores: 2147483647 mil | Valoración: 10 | Especialidad: Influencer | Estrella: SíPresioná cualquier tecla para volver al menú...
```

## leer corrupto

```text
============================================
  STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================

¿Reemplazar los datos en memoria por los archivos? (S/N):
Error: The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.

Presioná cualquier tecla para volver al menú...
```

## memoria tras error

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================

Copa ñ;ORT | Sponsor: Sponsor | Año: 2027 | Cupos: 1/1
Equipo: Equipo Á | Creación: 01/01/2000 | Categoría: Gaming
Creadores (1):
  - Nombre Ñ (@alias) | Edad: 30 | Seguidores: 2147483647 mil | Valoración: 10 | Especialidad: Influencer | Estrella: Sí

Presioná cualquier tecla para volver al menú...
```

## leer sin equipos txt

```text
============================================
  STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================

¿Reemplazar los datos en memoria por los archivos? (S/N):
Error: Falta el archivo equipos.txt.

Presioná cualquier tecla para volver al menú...
```

## estado sin equipos txt

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================

Copa ñ;ORT | Sponsor: Sponsor | Año: 2027 | Cupos: 1/1
Equipo: Equipo Á | Creación: 01/01/2000 | Categoría: Gaming
Creadores (1):
  - Nombre Ñ (@alias) | Edad: 30 | Seguidores: 2147483647 mil | Valoración: 10 | Especialidad: Influencer | Estrella: Sí

Presioná cualquier tecla para volver al menú...
```

## cargar fixture empate 4

```text
============================================
  STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================

¿Reemplazar los datos en memoria por los archivos? (S/N): Datos recuperados: 4 equipos.

Presioná cualquier tecla para volver al menú...
```

## ganador empate positivo

```text
============================================
  STREAMORT | MOSTRAR EL EQUIPO CON MÁS SEGUIDORES
============================================

Zeta: 500 mil seguidores.

Presioná cualquier tecla para volver al menú...
```

## salida top empatados

```text
=== TOP 3 DE STREAMORT ===

1. Alfa - Promedio: 8,00
2. Medio - Promedio: 8,00
3. Ultimo - Promedio: 8,00¡Hasta la próxima! Presioná una tecla para cerrar.
```

## opcion4 sin competencia

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

## opcion5 sin competencia

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN EQUIPO
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

## opcion6 sin competencia

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN CREADOR
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

## opcion7 sin competencia

```text
============================================
  STREAMORT | MOSTRAR ESTADÍSTICAS DE UN EQUIPO
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

## opcion8 sin competencia

```text
============================================
  STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

## salir sin competencia

```text
=== TOP 3 DE STREAMORT ===

No hay equipos con creadores para armar el ranking.

¡Hasta la próxima! Presioná una tecla para cerrar.
```


## Ampliación del 02/11/2026: pruebas sobre .NET 8

Doce grupos registrados en [Pruebas](../test/README.md): once sobre .NET 8.0.33 y una regresión sobre .NET 10.0.12. Entradas y salidas completas por grupo en esa carpeta; resultados estructurados en `Investigacion/resultados.json`. Los extractos siguientes corresponden a salidas observadas, normalizadas para eliminar secuencias de terminal.

### Confirmación con S mayúscula y memoria recuperada

Grupo: `01-basicas-net8`.

```text
============================================
STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================
Guardada | Sponsor: Sponsor | Año: 2027 | Cupos: 2/10000
Equipo: Alpha | Creación: 01/01/2000 | Categoría: Gaming
Creadores (1):
- Nombre Uno (@Uno) | Edad: 14 | Seguidores: 500 mil | Valoración: 8 | Especialidad: Streamer | Estrella: Sí
Equipo: Vacio | Creación: 01/01/2000 | Categoría: Gaming
Creadores (0):
(Sin creadores)
Presioná cualquier tecla para volver al menú...
```

### Valoración decimal y conservación de espacios interiores

Grupo: `02-entradas-net8`.

```text
============================================
STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================
Copa  con  espacios | Sponsor: Sponsor  doble | Año: 2000 | Cupos: 2/10000
Equipo: Águila  Azul | Creación: 29/02/2024 | Categoría: Música
Creadores (1):
- Nombre  doble (@Álias  doble) | Edad: 30 | Seguidores: 2147483647 mil | Valoración: 8,5 | Especialidad: Influencer | Estrella: Sí
Equipo: Hoy | Creación: 02/11/2026 | Categoría: Gaming
Creadores (0):
(Sin creadores)
Presioná cualquier tecla para volver al menú...
```

### Error después de un creador válido

Grupo: `03-persistencia-net8`.

**Lectura:**

```text
============================================
STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================
¿Reemplazar los datos en memoria por los archivos? (S/N):
Error: The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.
Presioná cualquier tecla para volver al menú...
```

**Memoria consultada después del error:**

```text
============================================
STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================
Guardada | Sponsor: Sponsor | Año: 2027 | Cupos: 2/10000
Equipo: Alpha | Creación: 01/01/2000 | Categoría: Gaming
Creadores (1):
- Nombre Uno (@Uno) | Edad: 14 | Seguidores: 500 mil | Valoración: 8 | Especialidad: Streamer | Estrella: Sí
Equipo: Vacio | Creación: 01/01/2000 | Categoría: Gaming
Creadores (0):
(Sin creadores)
Presioná cualquier tecla para volver al menú...
```

### Total superior a int.MaxValue

Grupo: `04-totales-net8`.

```text
Alpha
Equipo: Alpha
Valoración promedio: 8,00
Cantidad de streamers: 2
Total seguidores: 2147483648 mil
Creador más popular: Nombre Max (@Max) | Edad: 14 | Seguidores: 2147483647 mil | Valoración: 8 | Especialidad: Streamer | Estrella: Sí
Presioná cualquier tecla para volver al menú...
```

### TOP 3 con nombres Unicode empatados

Grupo: `05-ranking-net8`.

```text
=== TOP 3 DE STREAMORT ===
1. Aguila - Promedio: 8,00
2. águila - Promedio: 8,00
3. Águila - Promedio: 8,00
¡Hasta la próxima! Presioná una tecla para cerrar.
```

### Promedios visualmente iguales con distinto ganador

Grupo: `06-redondeo-net8`.

```text
Alfa
Zeta: 8,00 | Alfa: 8,00
Ganador: Zeta
Presioná cualquier tecla para volver al menú...
```

### Valoración real frente a redondeo visual y Estrella

Grupo: `07-decimales-net8`.

```text
============================================
STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================
Guardada | Sponsor: Sponsor | Año: 2027 | Cupos: 1/10000
Equipo: Zeta | Creación: 01/01/2000 | Categoría: Gaming
Creadores (4):
- Prueba (@8.5) | Edad: 14 | Seguidores: 500 mil | Valoración: 8,5 | Especialidad: Streamer | Estrella: Sí
- Prueba (@8,001) | Edad: 14 | Seguidores: 500 mil | Valoración: 8 | Especialidad: Streamer | Estrella: Sí
- Prueba (@8.004) | Edad: 14 | Seguidores: 500 mil | Valoración: 8 | Especialidad: Streamer | Estrella: Sí
- Prueba (@7,999) | Edad: 14 | Seguidores: 500 mil | Valoración: 8 | Especialidad: Streamer | Estrella: No
Presioná cualquier tecla para volver al menú...
```

### Archivo de solo lectura y guardado parcial

Grupo: `10-solo-lectura-net8`.

```text
============================================
STREAMORT | GUARDAR DATOS DE LA COMPETENCIA
============================================
Error: Access to the path 'C:\Users\julian\programacion\ACTIVIDAD-ESPECIAL\IA\Investigacion\StreamORT\Datos\creadores.txt' is denied.
Presioná cualquier tecla para volver al menú...
```

En el caso de solo lectura, `competencia.txt` quedó con `SoloMemoria`, `equipos.txt` sin filas y `creadores.txt` conservó los creadores anteriores. El error no cerró la aplicación; dejó el conjunto guardado inconsistente. La memoria conservó `SoloMemoria`.

La regresión de .NET 8 y .NET 10 produjo siete resultados normalizados idénticos; se conservaron ambos grupos de evidencia. El alcance y los límites de estas pruebas están en `Analisis.md`.
