# Contexto

## Consigna

### Trabajo Práctico: StreamORT – ¡Código perdido

Orientación: Informática

Materia: Taller de Programación / TI

¡URGENTE! Se perdió todo el código
El equipo de desarrollo de **CampusORT** estuvo trabajando durante meses en una nueva aplicación llamada **StreamORT**, un sistema que permite administrar competencias entre equipos de creadores de contenido, streamers, youtubers e influencers.

Después de mucho esfuerzo, lograron terminar el proyecto y entregarle al cliente una versión ejecutable de la aplicación.

¡El cliente quedó encantado con el resultado! Y decidió recomendarlo a un colega para que lo compre.

Todo parecía marchar perfecto, hasta que ocurrió una catástrofe...**¡SE ROMPIÓ EL SERVIDOR!**

En él estaban guardados todos los proyectos. Dejó de funcionar y, para empeorar las cosas, descubrieron que nadie había realizado una copia de seguridad.

**¡PERDIERON TODO EL CÓDIGO FUENTE!** Las clases, los métodos, las validaciones, los archivos...

El problema es que necesitan realizar algunas modificaciones y, sin el código fuente, resulta imposible.

Pero entonces, alguien del equipo recordó algo... **¡EL MAIL!** Antes del desastre, habían enviado por correo electrónico una copia del ejecutable al cliente inicial.

¡La aplicación todavía existe y funciona!

**El equipo de CampusORT los contrató a ustedes para una misión muy importante: RECONSTRUIR STREAMORT**

Deberán desarrollar nuevamente la aplicación, **pero no tendrán acceso al código fuente original**. Su única referencia será el ejecutable que logró recuperarse.

Para completar el desafío deberán ejecutar el programa, explorar sus funcionalidades, analizar su comportamiento y descubrir cómo está organizado.

**El objetivo es desarrollar una nueva versión de StreamORT que reproduzca el funcionamiento de la aplicación original.**

### Objetivos del trabajo práctico

Este desafío tiene como objetivo poner en práctica los siguientes conceptos:

* Programación Orientada a Objetos (POO).
* Creación de diagramas UML
* Clases, objetos, atributos y métodos. Interacción entre clases y objetos.
* Manejo de listas de objetos.
* Manejo de archivos de texto mediante StreamReader y StreamWriter.
* Analizar una aplicación existente y reconstruir su funcionamiento sin su implementación original.

### Primera etapa: investigar el programa

Antes de comenzar a programar, deberán ejecutar la aplicación proporcionada por el docente. Exploren todas las opciones disponibles e intenten comprender cómo funciona.

Durante esta etapa deberán prestar especial atención a:

* ¿Qué información solicita el programa? ¿Qué datos necesita almacenar?
* ¿Qué operaciones permite realizar?
* ¿Qué sucede cuando se ingresan datos incorrectos?
* ¿Qué ocurre cuando se intenta registrar información duplicada?
* ¿Qué cálculos y estadísticas realiza?
* ¿Cómo se relacionan los distintos elementos del sistema?
* ¿Qué información se guarda en los archivos y cómo se recupera?
* ¿Cómo funciona la navegación del menú y qué cambios visuales presenta?
* ¿Qué ocurre al salir de la aplicación?

**Importante: No alcanza con probar una sola vez cada opción.** Deberán investigar diferentes situaciones, incluyendo casos en los que el usuario comete errores.

### Segunda etapa: diseñar la solución

Una vez que comprendan el funcionamiento de la aplicación, deberán pensar cómo reconstruirla utilizando Programación Orientada a Objetos.

Para ello deberán identificar:

* Las clases necesarias para representar la información. Los atributos que debería tener cada clase. Los constructores necesarios. Los métodos que permitirán realizar las distintas operaciones.
* Las listas que deberán utilizarse para almacenar objetos.
El menú a realizar.
* Guardar y ver cómo se almacenan los datos en archivos.
Recuerden que no todo debe resolverse desde el programa principal. Cada clase deberá tener sus propias responsabilidades.

### Tercera etapa: reconstruir StreamORT

* El proyecto deberá desarrollarse en C# como una aplicación de consola.
* Se deberán utilizar clases para la resolución del ejercicio.
* Los atributos deberán inicializarse mediante constructores.
* Las listas deberán almacenar objetos de las clases correspondientes.
* Las búsquedas, cálculos y validaciones deberán realizarse mediante métodos de las clases cuando corresponda.
* No se permitirá registrar información duplicada.
* Los métodos deberán devolver valores adecuados para que el programa principal pueda mostrar los resultados. No se permiten impresiones en pantalla dentro de métodos de clases,
* Se deberá evitar la pérdida de información al guardar y recuperar los archivos.
* El programa deberá contemplar situaciones en las que todavía no existan datos registrados.
* La aplicación deberá funcionar correctamente ante entradas inválidas, sin finalizar inesperadamente.
* Navegación por las opciones del menú mediante las flechas del teclado.
* Selección de opciones utilizando la tecla Enter.
* Identificación visual de la opción seleccionada.
* Cambios de color de fondo según el tipo de operación que se esté realizando.

### Uso de Inteligencia Artificial

Durante el desarrollo del trabajo práctico **está permitido utilizar herramientas de Inteligencia Artificial.**

Sin embargo, existe una condición fundamental:

La IA debe ayudarlos a resolver el problema utilizando los conceptos y herramientas aprendidos en clase.

Podrán utilizarla para comprender errores, consultar dudas, repasar conceptos, analizar posibles soluciones o recibir explicaciones sobre el funcionamiento de determinadas instrucciones.

No deberán incorporar código que no puedan comprender o explicar.

Si la IA propone utilizar herramientas, estructuras o conceptos que todavía no fueron trabajados en clase, deberán solicitarle una solución alternativa utilizando los conocimientos adquiridos.

Cada grupo deberá crear un documento de Google Docs en el que registre todas las consultas realizadas a herramientas de Inteligencia Artificial durante el desarrollo del proyecto.
El documento deberá incluir las preguntas realizadas y las respuestas obtenidas, de manera que pueda observarse cómo utilizaron la IA para avanzar en el trabajo.

Este registro formará parte de la entrega.

Recuerden: El objetivo no es que la IA programe por ustedes, sino que los ayude a aprender a resolver los problemas que se presenten.

### Entrega del proyecto

Cada grupo deberá entregar:

1. Proyecto de C#

    Un archivo .zip que contenga el proyecto completo, incluyendo todos los archivos necesarios para abrirlo, compilarlo y ejecutarlo.

1. Registro de utilización de IA

    Un enlace al documento de Google Docs con todas las consultas realizadas a herramientas de IA durante el desarrollo.

No se trata solamente de conseguir que el programa funcione: también deberán poder explicar cómo lo desarrollaron.

## Contexto previo

Por ahora he hecho una parte del codigo. He creado un par de clases y una parte del menu principal. Para todo esto he usado dos libreias: `DataBases` y `Utilidades`. Estas librerias se encuentran en `Mis librerias\`. En es directorio encontraras el codigo y la documentacion.

## Reglas de codificacion

* Hacer modificaciones atomicas, es decir, no modificar mas de un metodo o clase a la ves.
* Usar git. Cada ves que se haga una modificacion, un commit y un push.
* Debes explcar la razon de cada cambio y de porque se hiso.
* Ser flexible a la hora de la implementacion. Es preferente usar las librerias y que la interfaz no quede identica a crear nuevos metodos. Puedes preguntarme si quieres hacer un cambio en las libreias si lo ves necesario.

## Pruebas

### Menu Principal

```Text
╔════════════════════════════════════════════════════╗
║        STREAMORT - BATALLA DE CREADORES            ║
╚════════════════════════════════════════════════════╝
Usá ↑ / ↓ para navegar y ENTER para seleccionar.

 ►  1. Crear competencia
    2. Registrar equipo
    3. Registrar creador en un equipo
    4. Mostrar información de la competencia
    5. Mostrar información de un equipo
    6. Mostrar información de un creador
    7. Mostrar estadísticas de un equipo
    8. Comparar valoración de dos equipos
    9. Mostrar el equipo con más seguidores
   10. Guardar datos de la competencia
   11. Leer datos de la competencia
   12. Salir

Azul: equipos | Violeta: creadores | Negro: general
Competencia: Sin crear
```

> Fondo negro y letras blancas pero para la parte seleccionada tiene subrayado blanco y letra negra.

#### Despues de crear la competencia

```text
╔════════════════════════════════════════════════════╗
║        STREAMORT - BATALLA DE CREADORES            ║
╚════════════════════════════════════════════════════╝
Usá ↑ / ↓ para navegar y ENTER para seleccionar.

 ►  1. Crear competencia
    2. Registrar equipo
    3. Registrar creador en un equipo
    4. Mostrar información de la competencia
    5. Mostrar información de un equipo
    6. Mostrar información de un creador
    7. Mostrar estadísticas de un equipo
    8. Comparar valoración de dos equipos
    9. Mostrar el equipo con más seguidores
   10. Guardar datos de la competencia
   11. Leer datos de la competencia
   12. Salir

Azul: equipos | Violeta: creadores | Negro: general
Competencia: AAA
```

### Opcion 1

#### Crear una competecia por primera ves

```text
============================================
  STREAMORT | CREAR COMPETENCIA
============================================

Nombre de la competencia: AAA
Patrocinador: AAA
Año (2000 a 2027): 100
Ingresá un número entero válido.
Año (2000 a 2027): sfsdf
Ingresá un número entero válido.
Año (2000 a 2027): 3000
Ingresá un número entero válido.
Año (2000 a 2027): 2000
Máximo de equipos (1 a 10000): -1
Ingresá un número entero válido.
Máximo de equipos (1 a 10000): a
Ingresá un número entero válido.
Máximo de equipos (1 a 10000): 20000000
Ingresá un número entero válido.
Máximo de equipos (1 a 10000): 3
Competencia creada.

Presioná cualquier tecla para volver al menú...
```

#### Seleccionar la opcion 1 otra ves

```text
============================================
  STREAMORT | CREAR COMPETENCIA
============================================

¿Reemplazar la competencia actual y sus datos? (S/N):
```

#### Decir que no

```text
============================================
  STREAMORT | CREAR COMPETENCIA
============================================

¿Reemplazar la competencia actual y sus datos? (S/N):
Presioná cualquier tecla para volver al menú...
```

> Toma como un no cualquier tecla que no sea una s.

#### Decir que si

```text
============================================
  STREAMORT | CREAR COMPETENCIA
============================================

¿Reemplazar la competencia actual y sus datos? (S/N): 
Nombre de la competencia: wwdwd
Patrocinador: adsas
Año (2000 a 2027): 2000
Máximo de equipos (1 a 10000): 1
Competencia creada.

Presioná cualquier tecla para volver al menú...
```

### Opcion 2

> Todo el fondo de la consola es azul

* Si no hay competencia

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

#### Si existe la competencia

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Nombre del equipo: aaa
Fecha de creación (dd/MM/yyyy): a
Fecha inválida o futura.
Fecha de creación (dd/MM/yyyy): 2000-12-12
Fecha inválida o futura.
Fecha de creación (dd/MM/yyyy): 12/5/2000
Fecha inválida o futura.
Fecha de creación (dd/MM/yyyy): 12/05/2000
Categoría:
1. Gaming
2. Humor
3. Música
4. Entretenimiento
Opción (1 a 4): 1
Equipo registrado.

Presioná cualquier tecla para volver al menú...
```

> Cuando alla que inplementar esto usar PedirOpcionMenu, en ves de esta interfaz.

#### Si se intenta registar un equipo con el mismo nombre

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

Nombre del equipo: aaa
Ese equipo ya existe.

Presioná cualquier tecla para volver al menú...
```

#### Si se intenta crear un equpo cuanod se ha alcansado el cupo

```text
============================================
  STREAMORT | REGISTRAR EQUIPO
============================================

No quedan cupos.

Presioná cualquier tecla para volver al menú...
```

### Opcion 3

> El fondo es violeta

#### Crear un creador

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: asd
Nombre del creador: pepe
Alias: pepe67
Edad (14 a 30): 40
Ingresá un número entero válido.
Edad (14 a 30): a
Ingresá un número entero válido.
Edad (14 a 30): 10
Ingresá un número entero válido.
Edad (14 a 30): 14
Seguidores (en miles) (0 a 2147483647): -1
Ingresá un número entero válido.
Seguidores (en miles) (0 a 2147483647): 1
Valoración (1 a 10): 5
Especialidad:
1. Streamer
2. Youtuber
3. Tiktoker
4. Influencer
Opción (1 a 4): 5
Ingresá un número entero válido.
Opción (1 a 4): 3
Creador registrado.

Presioná cualquier tecla para volver al menú...
```

#### Si no hay Competencia

```text
```

#### Si no hay equipos

```text
```

#### Si el equipo no existe

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: bbb
No se encontró el equipo.

Presioná cualquier tecla para volver al menú...
```

#### Si el creador ya existe en el equipo

```text
============================================
  STREAMORT | REGISTRAR CREADOR EN UN EQUIPO
============================================

Nombre del equipo: asd
Nombre del creador: pepe
Alias: pepe67
Ya existe ese alias en el equipo.

Presioná cualquier tecla para volver al menú...
```

### Opcion 4

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA
============================================

adsad | Sponsor: asasd | Año: 2000 | Cupos: 2/2
Equipo: aaa | Creación: 12/05/2000 | Categoría: Gaming
Creadores (1):
  - qqq (@qqq) | Edad: 14 | Seguidores: 0 mil | Valoración: 1 | Especialidad: Streamer | Estrella: No

Equipo: asd | Creación: 12/12/2000 | Categoría: Gaming
Creadores (1):
  - pepe (@pepe67) | Edad: 14 | Seguidores: 1 mil | Valoración: 5 | Especialidad: Tiktoker | Estrella: No

Presioná cualquier tecla para volver al menú...
```

> Cuando alla que implementar usa MostrarTabla antes de esto

### Opcion 5

#### Si no se ha creado la competencia

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN CREADOR
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

#### Si el equipo no existe

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN EQUIPO
============================================

Nombre del equipo: bbb
No se encontró el equipo.

Presioná cualquier tecla para volver al menú...
```

#### Si el equipo si existe

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN EQUIPO
============================================

Nombre del equipo: asd
Equipo: asd | Creación: 12/12/2000 | Categoría: Gaming
Creadores (1):
  - pepe (@pepe67) | Edad: 14 | Seguidores: 1 mil | Valoración: 5 | Especialidad: Tiktoker | Estrella: No

Presioná cualquier tecla para volver al menú...
```

### Opcion 6

> El fondo es violeta

#### Si no se ha creado la competencia

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN CREADOR
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

#### Si no hay equipos

Dise que no lo encuentra

#### Si no hay creadores en un equipo

Dise que no lo encuentra

#### Si el creador no se encuentra

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN CREADOR
============================================

Nombre del equipo: aaa
Alias del creador: bbb
No se encontró el creador.

Presioná cualquier tecla para volver al menú...
```

#### Si el creador existe

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN CREADOR
============================================

Nombre del equipo: aaa
Alias del creador: qqq
qqq (@qqq) | Edad: 14 | Seguidores: 0 mil | Valoración: 1 | Especialidad: Streamer | Estrella: No

Presioná cualquier tecla para volver al menú...
```

### Opcion 7

#### Si el equipo existe

```text
============================================
  STREAMORT | MOSTRAR ESTADÍSTICAS DE UN EQUIPO
============================================

Nombre del equipo: asd
Equipo: asd
Valoración promedio: 5,00
Cantidad de streamers: 0
Total seguidores: 1 mil
Creador más popular: pepe (@pepe67) | Edad: 14 | Seguidores: 1 mil | Valoración: 5 | Especialidad: Tiktoker | Estrella: No

Presioná cualquier tecla para volver al menú...
```

> Observación: la cantidad de streamers es 0, aunque el equipo tiene un creador cuya especialidad es Tiktoker.

#### Si el equipo no existe

```text
============================================
  STREAMORT | MOSTRAR ESTADÍSTICAS DE UN EQUIPO
============================================

Nombre del equipo: bbb
No se encontró el equipo.

Presioná cualquier tecla para volver al menú...
```

#### Si no se ha creado la competencia

```text
============================================
  STREAMORT | MOSTRAR INFORMACIÓN DE UN CREADOR
============================================

Primero tenés que crear o leer una competencia.

Presioná cualquier tecla para volver al menú...
```

#### Si no hay equipos

Dise que no lo encuentra

#### Si no hay creadores en un equipo

Dise que no lo encuentra

### Opcion 8

#### Si ambos equipos existen

```text
============================================
  STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS
============================================

Primer equipo: aaa
Segundo equipo: asd
aaa: 1,00 | asd: 5,00
Ganador: asd

Presioná cualquier tecla para volver al menú...
```

#### Si el segundo equipo no existe

```text
============================================
  STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS
============================================

Primer equipo: aaa
Segundo equipo: bbb
No se encontró el equipo.

Presioná cualquier tecla para volver al menú...
```

#### Si el primer equipo no existe

```text
============================================
  STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS
============================================

Primer equipo: bbb
No se encontró el equipo.

Presioná cualquier tecla para volver al menú...
```

* Si no se ha creado la competencia
* Si no hay equipos
* Si hay un solo equipo
* Si ambos equipos tienen la misma valoración promedio

### Opcion 9

#### Si hay equipos registrados

```text
============================================
  STREAMORT | MOSTRAR EL EQUIPO CON MÁS SEGUIDORES
============================================

asd: 1 mil seguidores.

Presioná cualquier tecla para volver al menú...
```

* Si no se ha creado la competencia
* Si no hay equipos
* Si hay un solo equipo
* Si hay varios equipos y uno tiene más seguidores que los demás
* Si dos o más equipos tienen la misma cantidad de seguidores
* Si los equipos no tienen creadores
* Si todos los equipos tienen cero seguidores

### Opcion 10

#### Si se guardan los datos de la competencia

```text
============================================
  STREAMORT | GUARDAR DATOS DE LA COMPETENCIA
============================================

Guardados competencia.txt, equipos.txt y creadores.txt en:
Z:\programacion 3\ACTIVIDAD ESPECIAL\Ejecutable\Datos

Presioná cualquier tecla para volver al menú...
```

#### Contenido de los archivos generados

**competencia.txt**

```text
YWRzYWQ=;YXNhc2Q=;2000;2
```

**creadores.txt**

```text
YWFh;cXFx;cXFx;14;0;1;U3RyZWFtZXI=
YXNk;cGVwZQ==;cGVwZTY3;14;1;5;VGlrdG9rZXI=
```

**equipos.txt**

```text
YWFh;2000-05-12;R2FtaW5n
YXNk;2000-12-12;R2FtaW5n
```

> Observación: No se deben guardar los datos de esta forma. Usar DataBases.

### Opción 11: Leer datos de la competencia

#### Caso 1: Confirmación para reemplazar los datos en memoria

```text
============================================
  STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================

¿Reemplazar los datos en memoria por los archivos? (S/N):
Presioná cualquier tecla para volver al menú...
```

#### Caso 2: Lectura correcta de los archivos

```text
============================================
  STREAMORT | LEER DATOS DE LA COMPETENCIA
============================================

¿Reemplazar los datos en memoria por los archivos? (S/N): 
Datos recuperados: 2 equipos.

Presioná cualquier tecla para volver al menú...
```

### Opción 12: Salir

#### Caso 1: Mostrar el TOP 3 y cerrar el programa

```text
=== TOP 3 DE STREAMORT ===

1. asd - Promedio: 5,00
2. aaa - Promedio: 1,00

¡Hasta la próxima! Presioná una tecla para cerrar.
```

## Pasos para la implementacion

1. Analizar la consisgna y el resultado del ejecutable y deducir cuales son las clases.
1. Generar un archivo `Arquitectura.md` donde estara toda la explicacion de las clases y sus metodos.
1. Declarar las clases y los metodos pero no implementarlos.

    ```csharp
    static Competencia CrearCompetencia() { }
    static Equipo CrearEquipo() { }
    static Creador CrearCreador() { }
    ```

1. Escribir el codigo de un metodo, commit y push. Asi hasta terminar.
1. Generar unos test unitarios en un proyecto aparte.
