# Repository Guidelines

## Estructura del proyecto

La solución `ACTIVIDAD ESPECIAL.sln` contiene una aplicación de consola en C# para gestionar competencias de creadores y equipos, con destino .NET 8.

El objetivo académico es reconstruir StreamORT a partir del ejecutable recuperado, reproduciendo su comportamiento mediante POO. Consultar [IA/Contexto.md](IA/Contexto.md) para la consigna completa y las transcripciones de pruebas; los casos sin resultados documentados requieren investigación, no suposiciones.

- `src/ACTIVIDAD ESPECIAL/`: aplicación principal; `Program.cs` contiene el menú y las clases `Competencia`, `Equipo`, `Creador` y `Datos` representan el dominio y su estado.
- `Mis Librerias/Utilidades/`: entrada validada, menús y presentación en consola.
- `Mis Librerias/DataBases/`: persistencia CSV y construcción de modelos.
- `Mis Librerias/Utilidades/README.md` y `Mis Librerias/DataBases/README.md`: documentación de cada biblioteca. `Mis Librerias/README.md` contiene solo introducción, propósito y enlaces; no hay carpeta separada de documentación.
- `Ejecutable/`: distribución existente de StreamORT y archivos de datos en `Datos/`.
- `IA/`: contexto auxiliar del proyecto.

## Compilación y desarrollo

Instalar el SDK de .NET 8 o uno compatible. Ejecutar desde la raíz y entrecomillar las rutas con espacios:

`Mis Librerias` es un submódulo de https://github.com/51215927-spec/Mis-Librerias.git. Clonar el proyecto con `git clone --recurse-submodules` o, en una copia existente, ejecutar `git submodule update --init --recursive` antes de compilar. Publicar primero los cambios del repositorio de bibliotecas y luego el commit que actualiza su referencia en este proyecto. El ZIP de entrega debe incluir los archivos del submódulo.

```powershell
dotnet restore "ACTIVIDAD ESPECIAL.sln"
dotnet build "ACTIVIDAD ESPECIAL.sln"
dotnet run --project "src/ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL.csproj"
```

Estos comandos restauran dependencias, compilan la solución y ejecutan la aplicación, respectivamente. Usar una terminal interactiva con soporte Unicode para los menús y caracteres decorativos.

## Estilo y nombres

Mantener la indentación de cuatro espacios y las llaves en líneas separadas observadas en el código C#. Usar nombres de clases y archivos en PascalCase, como `Equipo.cs`, y conservar las convenciones existentes de cada clase: el dominio utiliza propiedades como `nombre` y `fechaDeCreacion`. Mantener los identificadores y mensajes del dominio en español.

Los proyectos habilitan referencias anulables e importaciones implícitas. No hay configuración dedicada de formato o lint; evitar reformateos ajenos al cambio. No editar archivos generados en `bin/` u `obj/`.

Mantener cortos los README de entrada de los repositorios: introducción, para qué sirven y enlaces a documentación. La guía detallada de cada biblioteca debe estar en su propio directorio como `README.md`, por indicación del usuario.

## Verificación

No hay proyectos de pruebas automatizadas ni umbral de cobertura configurado. Compilar antes de entregar cambios y comprobar manualmente los flujos afectados del menú. Varias opciones todavía tienen bloques vacíos: distinguir funcionalidad pendiente de regresiones. Para cambios de persistencia, verificar guardado y lectura con copias de los datos.

Investigar entradas inválidas, duplicados, ausencia de competencia, equipos o creadores, cupos agotados y empates. Verificar estadísticas: valoración promedio, cantidad de creadores de especialidad Streamer, total de seguidores y creador más popular. Comprobar comparación de promedios, equipo con más seguidores y TOP 3 al salir. No deducir reglas aún no comprobadas, como la condición de «Estrella».

## Diseño e implementación

Inicializar atributos mediante constructores y almacenar objetos en listas. Ubicar búsquedas, cálculos y validaciones en las clases responsables; sus métodos deben devolver resultados para que el programa principal los muestre, sin imprimir desde las clases de dominio.

La interfaz debe ser idéntica a la del ejecutable. Usar `Utilidades` y `DataBases`, adaptando las bibliotecas en la etapa 3.5 de `docs/Plan.md` para reproducir su presentación y navegación. `PedirOpcionMenu` recibirá un `struct ColoresMenu` para agrupar los colores; usar `MostrarTabla` para información tabular. `Mis Librerias` es un submódulo del repositorio de bibliotecas del usuario; conservar sus archivos y las referencias del proyecto. Persistir con `DataBases`, sin reproducir la codificación Base64 del ejecutable, y evitar pérdidas de información al guardar o recuperar.

Antes de implementar, analizar el ejecutable y documentar clases, responsabilidades y métodos en `docs/Arquitectura.md`. Por indicación del usuario, no es necesario UML. Declarar las firmas y luego implementar un método por vez; usar cuerpos provisionales válidos en C# para mantener la compilación. Al finalizar, agregar pruebas unitarias en un proyecto separado.

## Comportamiento esperado

- Competencia: nombre, patrocinador, año entre 2000 y 2027 y máximo de equipos entre 1 y 10000, según las pruebas registradas.
- Equipo: nombre único, fecha estricta `dd/MM/yyyy` no futura y categoría Gaming, Humor, Música o Entretenimiento.
- Creador: nombre, alias único dentro del equipo, edad entre 14 y 30, seguidores en miles entre 0 e `int.MaxValue`, valoración con decimales entre 1 y 10 y especialidad Streamer, Youtuber, Tiktoker o Influencer. Preservar precisión al guardar, comparar y calcular Estrella; las sumas de seguidores pueden superar `int.MaxValue`.
- Confirmar antes de reemplazar la competencia o los datos en memoria: `s` y `S` confirman; `n`, `x` y Enter cancelan en las pruebas registradas. Consultar `docs/Analisis.md` para la evidencia actualizada.
- En el menú principal, navegar circularmente con flechas y Enter, conservar selección y resaltar 55 columnas en gris con marcador `►`. Categoría y especialidad se eligen con entrada numérica. Usar fondo `DarkBlue` para equipos, `DarkMagenta` para creadores, `Black` para operaciones generales y `DarkGreen` para TOP 3 y cierre, según [informe de interfaz](IA/Log.md#archivo-comparacion-interfaz-20261103). Las fichas no tienen bordes ni los mensajes etiquetas automáticas. Manejar entradas inválidas sin cerrar inesperadamente.

## Commits y solicitudes de cambio

El historial contiene mensajes breves en español e inglés, incluido el prefijo `Fix:`; no establece una convención estricta. Escribir mensajes concretos que describan el cambio y agrupar modificaciones relacionadas.

En cada solicitud de cambio, explicar el propósito, indicar los comandos y verificaciones realizadas y vincular incidencias cuando corresponda. Adjuntar capturas si cambia la presentación de consola.

La regla de trabajo de `IA/Contexto.md` exige cambios atómicos: modificar como máximo un método o una clase por vez, explicar el motivo de cada cambio y hacer commit y push después de cada modificación. Mantener fuera del commit los cambios ajenos a la tarea.

Antes de hacer push o ante un error de permisos, revisar mediante la CLI de GitHub (`gh`) la cuenta autenticada y su acceso al repositorio. Usar `gh auth status` para identificar las cuentas y `gh repo view --json nameWithOwner,viewerPermission` para comprobar el repositorio y los permisos de la cuenta activa.

## Instrucciones para agentes

Responder siempre al usuario en español.

Guardar en `IA/` todos los archivos referentes al uso de IA, incluidos contexto, consultas, respuestas y registros de uso. Mantener `AGENTS.md` en la raíz como guía del repositorio.

Mantener un registro cronológico en `IA/Log.md` de todos los pedidos del usuario, todas las respuestas del agente (incluidos los mensajes de progreso) y todos los cambios realizados. Actualizarlo en cada interacción y antes de finalizar la tarea, indicando fecha, archivos afectados, motivo, verificaciones y resultado de los commits y pushes cuando corresponda. Conservar el historial existente y no inventar conversaciones anteriores que no estén disponibles.

Usar conceptos aprendidos en clase y código que el estudiante pueda comprender y explicar. Ofrecer alternativas sencillas cuando una propuesta requiera herramientas todavía no estudiadas. Consultar al usuario si resulta necesario modificar las bibliotecas existentes.

La entrega requiere un ZIP del proyecto completo y un enlace a Google Docs con todas las preguntas y respuestas de IA. Conservar ese registro durante el desarrollo.
