# Plan de acción: reconstrucción de StreamORT

Fecha: 02/11/2026. Referencias: `AGENTS.md`, `IA/Contexto.md`, código actual y ejecutable recuperado.

## Objetivo y alcance

Completar la aplicación de consola en C# y .NET 8 reproduciendo el comportamiento comprobado del original mediante POO, listas de objetos y las bibliotecas `Utilidades` y `DataBases`. Este documento planifica el trabajo; no implementa las funcionalidades.

## Estado inicial observado

- `Program.Main` presenta las 12 opciones, pero los casos 1 a 11 están vacíos y el TOP 3 solamente tiene un comentario.
- `CrearCompetencia`, `CrearEquipo` y `CrearCreador` tienen cuerpos vacíos sin devolver valores: hay que corregirlos para recuperar la compilación.
- La condición de acceso exige competencia incluso para leer archivos; debe permitir la opción 11 sin competencia previa.
- `Competencia` ya valida año y cupo. `Equipo` almacena sus datos, pero no valida la fecha. `Creador` valida edad, pero no inicializa seguidores ni valoración y carece de especialidad.
- `Datos` solamente conserva una competencia. Faltan las listas, las operaciones del dominio, la persistencia y las pruebas unitarias.
- Parte de los casos del original está documentada; las reglas sin evidencia deben investigarse.

## 1. Completar la investigación

- [ ] Preparar una copia del ejecutable y sus datos para probar sin alterar los originales.
- [ ] Crear `IA/PruebasEjecutable.md` con una matriz: opción, estado inicial, entradas, resultado observado y dudas pendientes. Aprovechar las transcripciones existentes.
- [ ] Probar las 12 opciones con y sin competencia, equipos y creadores; verificar cancelaciones y reemplazos.
- [ ] Investigar nombres y alias duplicados, mayúsculas, espacios, textos vacíos y alias iguales en equipos diferentes.
- [ ] Comprobar límites numéricos, fechas estrictas `dd/MM/yyyy`, fechas imposibles y futuras, y cupo agotado.
- [ ] Determinar la condición de «Estrella», los resultados para equipos vacíos, empates en estadísticas y comparaciones, y criterio y desempate del TOP 3.
- [ ] Probar lectura con archivos ausentes, incompletos o dañados y salida con menos de tres equipos.

**Cierre:** cada regla necesaria tiene evidencia o queda explícitamente pendiente; no implementar reglas inventadas.

## 2. Documentar el diseño antes de programar

- [ ] Crear `IA/Arquitectura.md` con clases, atributos, constructores, firmas de métodos, responsabilidades y UML. Se guarda en `IA/` por la regla sobre documentación de IA.
- [ ] Asignar a `Competencia` la gestión de equipos, cupo, búsquedas y comparaciones; a `Equipo`, sus creadores y estadísticas; a `Creador`, sus datos y reglas individuales.
- [ ] Definir el papel de `Datos` como estado y coordinación de persistencia, evitando duplicar listas que puedan quedar desincronizadas.
- [ ] Mantener entrada y presentación en `Program`; los métodos de dominio devolverán resultados sin imprimir.
- [ ] Elegir cómo representar especialidades y cómo identificar relaciones al guardar y reconstruir objetos.
- [ ] Revisar la documentación y el código vigente de `DataBases`: el README menciona APIs que pueden diferir de la implementación actual. Elegir la alternativa más sencilla que conserve todos los datos.

**Cierre:** diseño comprensible para el estudiante, con contratos definidos y dudas funcionales resueltas antes de implementar cada operación afectada.

## 3. Recuperar una base compilable y completar el dominio

Ejecutar cada punto como cambios separados, de un método o una clase por vez.

- [ ] Registrar los errores iniciales de compilación y sustituir cada cuerpo vacío por un cuerpo provisional válido; no conectar al menú operaciones provisionales que lancen excepciones.
- [ ] Declarar las firmas acordadas, manteniendo la compilación después de cada cambio.
- [ ] Completar `Creador`: inicialización por constructor, especialidad, edad 14–30, seguidores en miles 0–`int.MaxValue`, valoración 1–10 y condición de estrella comprobada.
- [ ] Completar `Equipo`: validaciones, lista de creadores, búsqueda y registro con alias único dentro del equipo.
- [ ] Completar `Competencia`: lista de equipos, búsqueda, nombre único y respeto del cupo. Mantener año 2000–2027 y máximo de equipos 1–10000.
- [ ] Ajustar `Datos` al diseño y representar explícitamente la ausencia de competencia.
- [ ] Implementar, uno por vez, promedio de valoración, cantidad de Streamers, total de seguidores y creador más popular.
- [ ] Implementar comparación de promedios, equipo con más seguidores y TOP 3 según las reglas verificadas. Elegir un tipo que evite desbordamientos al sumar seguidores.

**Cierre:** las operaciones del dominio funcionan sin depender de la consola y contemplan listas vacías, duplicados y empates.

## 4. Conectar los flujos del menú

- [ ] Opción 1: crear competencia y confirmar antes de reemplazarla; cualquier tecla distinta de `s` cancela. El reemplazo elimina coherentemente los datos de la competencia anterior.
- [ ] Opción 2: registrar equipo, validar fecha estricta no futura y seleccionar categoría Gaming, Humor, Música o Entretenimiento.
- [ ] Opción 3: seleccionar equipo y registrar creador con especialidad Streamer, Youtuber, Tiktoker o Influencer.
- [ ] Opciones 4, 5 y 6: mostrar competencia, equipo y creador; informar ausencias y búsquedas fallidas.
- [ ] Opciones 7, 8 y 9: mostrar estadísticas, comparación y equipo con más seguidores.
- [ ] Permitir acceso a crear, leer y salir cuando no exista competencia; proteger las demás operaciones según sus requisitos.
- [ ] Opción 12: mostrar TOP 3 y cerrar, incluyendo los casos sin datos y con menos de tres equipos.
- [ ] Usar `PedirOpcionMenu` para selecciones y `MostrarTabla` para información tabular; verificar flechas, Enter, resaltado y fondos azul, violeta y negro según operación.
- [ ] Manejar errores de entrada y restaurar la pantalla al volver al menú.

**Cierre:** cada flujo conectado se compara manualmente con los casos documentados. No modificar bibliotecas sin consultar al usuario si resulta necesario.

## 5. Implementar guardado y lectura

- [ ] Definir archivos CSV y campos que conserven competencia, equipos, creadores y relaciones mediante `DataBases`, sin reproducir Base64.
- [ ] Opción 10: guardar todos los datos, informar errores y proteger la última copia completa frente a fallos parciales entre archivos; comprobar qué garantías ofrece la biblioteca.
- [ ] Opción 11: leer a estructuras temporales, validar datos y relaciones, confirmar el reemplazo cuando corresponda y actualizar memoria solamente tras una lectura completa válida.
- [ ] Verificar ida y vuelta con copias: guardar, reiniciar, leer y comparar todos los campos, asociaciones y resultados estadísticos.
- [ ] Probar archivos vacíos, ausentes, dañados, duplicados, relaciones inválidas, caracteres especiales y errores de escritura sin perder los datos previos.

**Cierre:** guardar y recuperar conserva la información; un error o una cancelación no reemplaza parcialmente el estado válido.

## 6. Verificación final y entrega

- [ ] Agregar un proyecto separado de pruebas unitarias y sumarlo a la solución, usando herramientas que el estudiante pueda explicar.
- [ ] Probar límites de constructores, duplicados, cupos, búsquedas, estadísticas, listas vacías, empates y ranking según las reglas descubiertas.
- [ ] Agregar pruebas de persistencia sobre directorios temporales, sin tocar los datos originales.
- [ ] Ejecutar los comandos desde la raíz:

```powershell
dotnet restore "ACTIVIDAD ESPECIAL.sln"
dotnet build "ACTIVIDAD ESPECIAL.sln"
dotnet test "ACTIVIDAD ESPECIAL.sln"
dotnet run --project "ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL.csproj"
```

- [ ] Recorrer manualmente las 12 opciones, incluyendo entradas inválidas, cancelaciones y navegación. Registrar resultados y capturas de la presentación.
- [ ] Actualizar arquitectura y registro de IA; reunir todas las consultas y respuestas en el Google Docs requerido.
- [ ] Preparar un ZIP del proyecto completo con solución, fuentes, bibliotecas y archivos necesarios; verificar que se pueda extraer, compilar y ejecutar.

**Cierre:** compilación y pruebas satisfactorias, flujos contrastados, ZIP comprobado y enlace al registro de IA disponible.

## Forma de trabajo para cada cambio

1. Elegir un método o una clase y explicar el problema y el motivo del cambio.
2. Modificar únicamente esa unidad, conservando el estilo y los identificadores en español.
3. Compilar y verificar el comportamiento afectado; registrar resultado y limitaciones en `IA/`.
4. Revisar el diff, incluir solamente archivos propios del cambio y hacer un commit con mensaje concreto en español.
5. Hacer push antes de continuar con la siguiente modificación. Si falta acceso o aprobación del entorno, informar el impedimento.
6. Conservar las preguntas y respuestas completas para el documento de entrega. No incluir cambios ajenos que ya existían en el árbol de trabajo.

## Registro de esta consulta

**Usuario:** «Genera el plan de accion. Guardalo en ia\plan.md».

**Respuesta:** se preparó este plan a partir de las instrucciones, el contexto y el código actual. Las fases describen trabajo futuro; las casillas no indican tareas ya ejecutadas.
