# Plan de acción: reconstrucción de StreamORT

Actualizado: 02/11/2026, tras ampliar las pruebas pendientes del original. Referencias: `AGENTS.md`, [Contexto.md](Contexto.md), [Analisis.md](Analisis.md), [Pruebas-StreamORT.md](Pruebas-StreamORT.md), [Investigacion/README.md](Investigacion/README.md), código actual y ejecutable recuperado.

**Aclaración del usuario:** no se requiere UML. Basta con `IA/Arquitectura.md`, explicando clases, responsabilidades, atributos, constructores y métodos. Esta indicación reemplaza el requisito anterior de diagramas.

## Objetivo y alcance

Completar la aplicación de consola en C# y .NET 8 reproduciendo el comportamiento comprobado del original mediante POO, listas de objetos y las bibliotecas `Utilidades` y `DataBases`. Este documento planifica el trabajo; no implementa las funcionalidades.

## Estado de avance

- [x] Analizar las transcripciones previas y ejecutar el ensamblado recuperado en terminal interactiva.
- [x] Documentar resultados, evidencia, correcciones al contexto y casos pendientes en `IA/Analisis.md`, con capturas complementarias en `IA/Pruebas-StreamORT.md`.
- [x] Registrar el análisis en el commit `f420b1c`.
- [x] Resolver la publicación: `main` quedó sincronizada con `origin/main`; la cuenta `51215927-spec` tiene permiso `ADMIN` y el último push de la revisión fue correcto (`c507819`).
- [x] Ampliar las pruebas puntuales pendientes: doce grupos con evidencia, once sobre .NET 8 y una regresión sobre .NET 10. Se documentaron los límites; no equivale a una matriz exhaustiva de toda entrada posible.
- [ ] Elaborar `IA/Arquitectura.md` antes de implementar el dominio.

## Estado del código observado antes de esta actualización

- `Program.Main` presenta las 12 opciones, pero los casos 1 a 11 están vacíos y el TOP 3 solamente tiene un comentario.
- `CrearCompetencia`, `CrearEquipo` y `CrearCreador` tienen cuerpos vacíos sin devolver valores: hay que corregirlos para recuperar la compilación.
- La condición de acceso exige competencia incluso para leer archivos; debe permitir la opción 11 sin competencia previa.
- `Competencia` ya valida año y cupo. `Equipo` almacena sus datos, pero no valida la fecha. `Creador` valida edad, pero no inicializa seguidores ni valoración y carece de especialidad.
- `Datos` solamente conserva una competencia. Faltan las listas, las operaciones del dominio, la persistencia y las pruebas unitarias.
- El análisis identifica reglas de Estrella, equipos vacíos y desempates. Las pruebas nuevas confirman valoraciones decimales, totales superiores a `int.MaxValue`, confirmación con `S`, lectura segura en los errores ensayados y posible guardado parcial del original. Los límites de la investigación están en `IA/Analisis.md`.

## Orden de las próximas acciones

1. Investigación ampliada y documentada: completada para los escenarios de la fase 1. Consultar la evidencia y sus límites antes de implementar; agregar ensayos si aparece una duda concreta.
2. Crear `IA/Arquitectura.md`, sin UML, incorporando las reglas comprobadas y diferenciando propuestas de diseño de comportamiento observado.
3. Recuperar la compilación: la restauración ya funciona; el último `dotnet build` falló por tres errores CS0161 en `CrearCompetencia`, `CrearEquipo` y `CrearCreador`, cuyos cuerpos no devuelven valores.
4. Completar el dominio: listas, validaciones, búsquedas, estadísticas, Estrella y rankings.
5. Conectar las opciones del menú y corregir el acceso a lectura sin competencia previa.
6. Implementar guardado y lectura con `DataBases`, conservando datos y relaciones ante errores.
7. Agregar pruebas unitarias, verificar los flujos y preparar ZIP completo y Google Docs con consultas y respuestas de IA.

## 1. Completar la investigación

- [x] Probar sin alterar los datos originales: sesión inicial en memoria y copia temporal para guardado, lectura y archivos inválidos.
- [x] Consolidar los resultados en `IA/Analisis.md` y las capturas complementarias en `IA/Pruebas-StreamORT.md`. Mantener estos archivos como referencia, sin crear otro registro duplicado.
- [x] Comprobar rechazo de equipos y alias duplicados sin distinguir mayúsculas/minúsculas en los casos ensayados, alias iguales en distintos equipos, recorte de espacios exteriores y rechazo de nombre de competencia vacío.
- [x] Comprobar equipos vacíos, comparación del mismo equipo, empate de promedios, empate de popularidad y de seguidores, salida sin competencia, TOP 3 con menos de tres equipos y empate de cuatro equipos.
- [x] Investigar Estrella con pruebas de frontera de seguidores y valoración, incluyendo resultados positivos en las cuatro especialidades.
- [x] Verificar guardado y recuperación con acentos, punto y coma y `int.MaxValue` seguidores; error de Base64 y ausencia de archivos de competencia y equipos, con conservación de memoria en esos errores concretos.
- [x] Ampliar la matriz de opciones y estados: `S` confirma; `n`, `x` y Enter cancelan en ambas operaciones; comparación vacío/lleno rechazada en ambos órdenes; consultas sin competencia/equipos/creadores verificadas.
- [x] Probar campos vacíos y solo con espacios, conservación de espacios interiores, textos de 256 caracteres y búsquedas con caja, tildes y acentos combinantes. No se determinó un máximo absoluto de longitud ni equivalencias para todo Unicode.
- [x] Ampliar límites numéricos y fechas: rechazo de imposibles, formato abreviado/no admitido y mañana; aceptación de bisiesta válida y hoy; límites y cupo agotado verificados. Se descubrió que valoración acepta decimales con coma o punto.
- [x] Comprobar total de 2147483648 mil; documentar el TOP 3 con variantes Unicode y que nombres que solo difieren en caja son duplicados; comparar 8,004 contra 8,001, mostrados ambos como 8,00: gana el primero.
- [x] Probar 22 variantes de lectura, incluidos archivos vacíos/incompletos, creadores ausentes, duplicados, relaciones inexistentes y corrupción tras filas válidas. Memoria conservada en todos los fallos ensayados. Escritura bloqueada mediante directorio y archivo de solo lectura: error informado y memoria conservada, pero archivos previos parcialmente reemplazados.
- [x] Reemplazar tres equipos/dos creadores por una competencia sin equipos y releer: cero equipos, sin filas residuales.
- [x] Ejecutar once grupos sobre .NET 8.0.33 y comparar una regresión de estadísticas/Estrella/ranking con .NET 10.0.12: siete resultados normalizados idénticos.

### Reglas comprobadas que debe incorporar el diseño

| Aspecto | Comportamiento respaldado por las pruebas |
| --- | --- |
| Estrella | Seguidores en miles >= 500 y valoración >= 8. |
| Valoración y precisión | Acepta decimales 1–10 con coma o punto; conserva precisión en archivo. Comparación, Estrella y ranking usan valores previos al redondeo visual. |
| Total de seguidores | Admite al menos 2147483648 mil; proponer `long` para las sumas, manteniendo `int` por creador. |
| Identidad | Nombre de equipo único; alias único dentro del equipo. Los casos ensayados no distinguen mayúsculas/minúsculas y recortan espacios exteriores. |
| Equipo vacío | Promedio 0,00; cero streamers y seguidores; más popular: `Sin creadores`. |
| Streamers | Contar exclusivamente creadores de especialidad Streamer. |
| Creador más popular | Mayor cantidad de seguidores; en el empate probado conserva el primero registrado. |
| Comparación | Rechazar el mismo equipo; ambos deben tener creadores; comparar promedios y anunciar empate cuando corresponde. |
| Equipo con más seguidores | Excluir equipos sin creadores; en los empates probados conservar el primero registrado/cargado. Un equipo con creadores y cero seguidores sí participa. |
| TOP 3 | Incluir solamente equipos con creadores, ordenar por promedio descendente y, en el empate de nombres ensayado, alfabéticamente; mostrar como máximo tres. |
| Inicio y lectura | No cargar automáticamente; permitir leer sin competencia previa y pedir confirmación si existe una activa. |
| Confirmaciones | `s` y `S` confirman; `n`, `x` y Enter cancelan en los ensayos de reemplazo de competencia y lectura. |
| Lectura fallida | Conservar memoria incluso con corrupción después de filas válidas; comprobado en los fallos de la matriz, sin generalizar a todo fallo posible del sistema. |
| Guardado fallido | El original puede dejar archivos parcialmente reemplazados. Proteger la última copia completa en la reconstrucción es una mejora de robustez del diseño. |

**Cierre:** cada regla necesaria tiene evidencia o queda explícitamente pendiente; no implementar reglas inventadas.

## 2. Documentar el diseño antes de programar

- [ ] Crear `IA/Arquitectura.md` con clases, atributos, constructores, firmas de métodos y responsabilidades. No se requiere UML por indicación del usuario. Se guarda en `IA/` por la regla sobre documentación de IA.
- [ ] Asignar a `Competencia` la gestión de equipos, cupo, búsquedas y comparaciones; a `Equipo`, sus creadores y estadísticas; a `Creador`, sus datos y reglas individuales.
- [ ] Definir el papel de `Datos` como estado y coordinación de persistencia, evitando duplicar listas que puedan quedar desincronizadas.
- [ ] Mantener entrada y presentación en `Program`; los métodos de dominio devolverán resultados sin imprimir.
- [ ] Elegir cómo representar especialidades y cómo identificar relaciones al guardar y reconstruir objetos.
- [ ] Revisar la documentación y el código vigente de `DataBases`: el README menciona APIs que pueden diferir de la implementación actual. Elegir la alternativa más sencilla que conserve todos los datos.

**Cierre:** diseño comprensible para el estudiante, con contratos definidos y dudas funcionales resueltas antes de implementar cada operación afectada.

## 3. Recuperar una base compilable y completar el dominio

Ejecutar cada punto como cambios separados, de un método o una clase por vez.

- [x] Restaurar dependencias y registrar los errores reales: tres CS0161 por métodos sin retorno; el problema anterior de carpeta NuGet no se reprodujo en la revisión.
- [ ] Sustituir cada cuerpo vacío por un cuerpo provisional válido; no conectar al menú operaciones provisionales que lancen excepciones.
- [ ] Declarar las firmas acordadas, manteniendo la compilación después de cada cambio.
- [ ] Completar `Creador`: inicialización por constructor, especialidad, edad 14–30, seguidores en miles 0–`int.MaxValue`, valoración con decimales 1–10 y Estrella cuando seguidores >= 500 y valoración >= 8, sin redondeo previo.
- [ ] Completar `Equipo`: validaciones, lista de creadores, búsqueda y registro con alias único dentro del equipo.
- [ ] Completar `Competencia`: lista de equipos, búsqueda, nombre único y respeto del cupo. Mantener año 2000–2027 y máximo de equipos 1–10000.
- [ ] Ajustar `Datos` al diseño y representar explícitamente la ausencia de competencia.
- [ ] Implementar, uno por vez, promedio de valoración, cantidad de Streamers, total de seguidores y creador más popular.
- [ ] Implementar comparación de promedios, equipo con más seguidores y TOP 3 según la tabla de reglas comprobadas. Conservar precisión de valoración y promedio; usar una representación amplia para los totales (propuesta: `long`). Documentar el criterio elegido para ordenar los nombres Unicode y contrastarlo con la muestra observada.

**Cierre:** las operaciones del dominio funcionan sin depender de la consola y contemplan listas vacías, duplicados y empates.

## 4. Conectar los flujos del menú

- [ ] Opción 1: crear competencia y confirmar antes de reemplazarla; `s` o `S` confirma y las otras teclas ensayadas cancelan. El reemplazo elimina coherentemente los datos de la competencia anterior.
- [ ] Opción 2: registrar equipo, validar fecha estricta no futura y seleccionar categoría Gaming, Humor, Música o Entretenimiento.
- [ ] Opción 3: seleccionar equipo y registrar creador con especialidad Streamer, Youtuber, Tiktoker o Influencer.
- [ ] Opciones 4, 5 y 6: mostrar competencia, equipo y creador; informar ausencias y búsquedas fallidas.
- [ ] Opciones 7, 8 y 9: mostrar estadísticas, comparación y equipo con más seguidores.
- [ ] Permitir acceso a crear, leer y salir cuando no exista competencia; proteger las demás operaciones según sus requisitos.
- [ ] Opción 12: mostrar TOP 3 de equipos con creadores, por promedio descendente y desempate por nombre según lo comprobado; contemplar ausencia de datos y menos de tres equipos. No agregar guardado automático sin definir ese cambio de comportamiento.
- [ ] Usar `PedirOpcionMenu` para selecciones y `MostrarTabla` para información tabular; verificar flechas, Enter, resaltado y fondos azul, violeta y negro según operación.
- [ ] Manejar errores de entrada y restaurar la pantalla al volver al menú.

**Cierre:** cada flujo conectado se compara manualmente con los casos documentados. No modificar bibliotecas sin consultar al usuario si resulta necesario.

## 5. Implementar guardado y lectura

- [ ] Definir archivos CSV y campos que conserven competencia, equipos, creadores y relaciones mediante `DataBases`, sin reproducir Base64.
- [ ] Opción 10: guardar todos los datos, informar errores y proteger la última copia completa frente a fallos parciales entre archivos; comprobar qué garantías ofrece la biblioteca.
- [ ] Opción 11: leer a estructuras temporales, validar datos y relaciones, confirmar el reemplazo cuando corresponda y actualizar memoria solamente tras una lectura completa válida.
- [ ] Verificar ida y vuelta con copias: guardar, reiniciar, leer y comparar todos los campos, asociaciones y resultados estadísticos.
- [ ] Preservar valoraciones decimales (8,5; 8,001; 8,004; 7,999) y su efecto en Estrella/comparación; no guardar el valor redondeado de la ficha.
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

**Respuesta inicial:** se preparó este plan a partir de las instrucciones, el contexto y el código actual.

**Nueva consulta:** «Tras ello actualisa el plan de accion y has commit y push».

**Respuesta y actualización:** se incorporaron las pruebas ya realizadas, las reglas comprobadas, los casos pendientes y los problemas conocidos de compilación y acceso a GitHub. Las casillas marcadas corresponden únicamente a investigación y documentación completadas; la implementación, arquitectura, pruebas de la reconstrucción y entrega siguen pendientes. Esta actualización solo modifica el plan. Se hará commit y se reintentará el push; su resultado se informará al usuario.

**Continuación del 02/11/2026:** el usuario indicó que basta `Arquitectura.md`, sin UML, y pidió avanzar con la investigación. Se ejecutaron doce grupos y se conservaron entradas, salidas y resultados en `IA/Investigacion/`. La fase 1 queda ampliada y documentada con sus límites explícitos; el próximo paso de desarrollo es escribir la arquitectura, sin implementar todavía el dominio. Las consultas sobre la carpeta y los avances completos están registrados en `IA/Log.md`.
