# Plan de acción: reconstrucción de StreamORT

Actualizado: 03/11/2026, tras completar las dos últimas tareas de la etapa 3. **Etapa 3 completa; siguiente etapa: 4, conectar los flujos del menú**. La lógica de dominio de `Creador`, `Equipo` y `Competencia` está implementada. Los métodos `Configurar` se implementarán junto con la persistencia del proyecto completo, por indicación del usuario. Referencias: `AGENTS.md`, [Arquitectura.md](Arquitectura.md), [Contexto.md](Contexto.md), [Analisis.md](Analisis.md), [Pruebas-StreamORT.md](Pruebas-StreamORT.md), [Investigacion/README.md](Investigacion/README.md), código actual y ejecutable recuperado.

**Aclaración del usuario:** no se requiere UML. Basta con `IA/Arquitectura.md`, explicando clases, responsabilidades, atributos, constructores y métodos. Esta indicación reemplaza el requisito anterior de diagramas.

## Objetivo y alcance

Completar la aplicación de consola en C# y .NET 8 reproduciendo el comportamiento comprobado del original mediante POO, listas de objetos y las bibliotecas `Utilidades` y `DataBases`. Este documento planifica el trabajo; no implementa las funcionalidades.

## Estado de avance

- [x] Analizar las transcripciones previas y ejecutar el ensamblado recuperado en terminal interactiva.
- [x] Documentar resultados, evidencia, correcciones al contexto y casos pendientes en `IA/Analisis.md`, con capturas complementarias en `IA/Pruebas-StreamORT.md`.
- [x] Registrar el análisis en el commit `f420b1c`.
- [x] Resolver la publicación: arquitectura y actualización del plan publicadas en `acfac2d`; cierre del registro publicado en `00f3f7c`. Cuenta de `gh`: `51215927-spec`, con permiso `ADMIN`. Usar el proveedor de credenciales de `gh` por comando para evitar que Git Credential Manager seleccione otra cuenta.
- [x] Ampliar las pruebas puntuales pendientes: doce grupos con evidencia, once sobre .NET 8 y una regresión sobre .NET 10. Se documentaron los límites; no equivale a una matriz exhaustiva de toda entrada posible.
- [x] Elaborar `IA/Arquitectura.md` antes de implementar el dominio.
- [x] Recuperar la compilación: los tres métodos de creación tienen cuerpos provisionales con `NotImplementedException`, sin llamadas desde el menú. Verificación final: cero errores y cinco advertencias (tres NU1900, CA1416 y CS0219).
- [x] Declarar los contratos del diseño en las cinco clases; agregar `IEntidad<T>` y conservar todos los enums en `ENUMs.cs`. Tras las indicaciones del usuario, dos de las 39 firmas iniciales se sustituyen por las propiedades `equipos` y `creadores` con get y se elimina el método de reemplazo de competencia: quedan 36 firmas de constructores y métodos. Métodos y getters nuevos son provisionales y no están conectados al menú. No equivale a implementar las operaciones.
- [x] Conservar solo get para datos fijados por constructor y private set donde se requiere reemplazo interno. Por indicación expresa del usuario, `Datos.competencia` tiene get y set públicos sin lógica personalizada. Las consultas de colecciones son propiedades; las búsquedas, altas y cálculos siguen como métodos.

Revisión actual del 03/11/2026: etapas 1, 2 y 3 completas para el alcance documentado. `Creador`, `Equipo` y `Competencia` tienen completa su lógica de dominio; `Datos` representa la ausencia de competencia con su estado nullable. La etapa 4 está pendiente de iniciar. Se mantiene un commit y push por método, incluyendo solo los archivos propios de cada cambio. `Configurar` queda para la etapa 5 y el proyecto separado de pruebas unitarias para la etapa 6. La comprobación auxiliar final de `Competencia` pasó 56 casos, incluida la muestra Unicode del TOP 3; no equivale a probar todo Unicode ni todos los sistemas.

## Estado actual del código

- `Program.Main` presenta las 12 opciones, pero los casos 1 a 11 están vacíos y el TOP 3 solamente tiene un comentario.
- `CrearCompetencia`, `CrearEquipo` y `CrearCreador` tienen cuerpos provisionales válidos que lanzan `NotImplementedException`. La solución compila; las operaciones todavía no están implementadas ni conectadas al menú.
- La condición de acceso exige competencia incluso para leer archivos; debe permitir la opción 11 sin competencia previa.
- `Competencia` valida textos, año y máximo de equipos; inicializa la lista privada y devuelve copias. Implementa búsqueda, cupo y registro con `RegistroException`, comparación por promedio decimal, equipo con más seguidores con totales `long` y TOP 3 con desempate cultural explícito `es-AR`. Solo queda configuración CSV.
- `Equipo` valida nombre, fecha no futura y categoría, gestiona sus creadores con copia pública de la lista, busca alias y registra con `RegistroException`. Implementa promedio decimal, conteo de Streamers, total de seguidores con `long` y creador más popular con desempate por orden. Solo queda configuración CSV.
- `Creador` inicializa y valida sus siete datos y calcula Estrella sin redondeo. Solo queda configuración CSV.
- `Datos` expone `Competencia?` con get y set públicos para asignación directa y ausencia con `null`, sin método separado de reemplazo. Guardado, lectura y construcción del modelo siguen provisionales y se implementarán en la etapa 5. Todavía no existe el proyecto separado de pruebas unitarias.
- El análisis identifica reglas de Estrella, equipos vacíos y desempates. Las pruebas nuevas confirman valoraciones decimales, totales superiores a `int.MaxValue`, confirmación con `S`, lectura segura en los errores ensayados y posible guardado parcial del original. Los límites de la investigación están en `IA/Analisis.md`.

## Orden de las próximas acciones

1. Investigación ampliada y documentada: completada para los escenarios de la fase 1. Consultar la evidencia y sus límites antes de implementar; agregar ensayos si aparece una duda concreta.
2. Arquitectura documentada en `IA/Arquitectura.md`, sin UML, con reglas comprobadas y propuestas de diseño diferenciadas. Dominio de `Creador`, `Equipo` y `Competencia` implementado.
3. Compilación recuperada. Verificación actual: `dotnet build "ACTIVIDAD ESPECIAL.sln" --artifacts-path "IA/obj/Compilacion" --no-restore`, cero errores y dos advertencias previas (CA1416 y CS0219). Los artefactos aislados quedan fuera de Git. Las comprobaciones auxiliares se ejecutaron sobre .NET 10 con roll-forward explícito por ausencia del runtime .NET 8, sin cambiar net8.0.
4. Dominio completo: `Competencia` implementa validación de textos, lista de equipos, búsquedas, altas, cupos, comparación de promedios y rankings. La persistencia de `Datos` y los métodos `Configurar` se reservan para la etapa 5.
5. Próximo paso: iniciar la etapa 4, conectar las opciones del menú y corregir el acceso a lectura sin competencia previa. No invocar métodos provisionales de persistencia hasta implementarlos en la etapa 5.
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

- [x] Crear `IA/Arquitectura.md` con clases, atributos, constructores, firmas de métodos y responsabilidades. No se requiere UML por indicación del usuario. Se guarda en `IA/` por la regla sobre documentación de IA.
- [x] Asignar a `Competencia` la gestión de equipos, cupo, búsquedas y comparaciones; a `Equipo`, sus creadores y estadísticas; a `Creador`, sus datos y reglas individuales.
- [x] Definir el papel de `Datos` como estado y coordinación de persistencia, evitando duplicar listas que puedan quedar desincronizadas.
- [x] Mantener entrada y presentación en `Program`; los métodos de dominio devolverán resultados sin imprimir.
- [x] Elegir cómo representar especialidades y cómo identificar relaciones al guardar y reconstruir objetos.
- [x] Revisar el código vigente de `DataBases`: configuración mediante `IEntidad<T>`, constructores compatibles, soporte de `decimal` y `DateOnly`, carga explícita y ausencia de transacción en `GuardarTodo`. Definida una propuesta de preparación y respaldo sin modificar la biblioteca.

Estas casillas corresponden al diseño documentado, no a funcionalidades implementadas. Quedan por contrastar los comparadores Unicode, los detalles de navegación disponibles y la recuperación ante fallos durante la publicación de archivos, según `Arquitectura.md`.

**Cierre:** diseño comprensible para el estudiante, con contratos definidos y dudas funcionales resueltas antes de implementar cada operación afectada.

## 3. Recuperar una base compilable y completar el dominio

Ejecutar cada punto como cambios separados, de un método o una clase por vez.

- [x] Restaurar dependencias y registrar los errores reales: tres CS0161 por métodos sin retorno; el problema anterior de carpeta NuGet no se reprodujo en la revisión.
- [x] Sustituir cada cuerpo vacío por un cuerpo provisional válido; no conectar al menú operaciones provisionales que lancen excepciones. Corregidos los tres métodos, un commit y push por método.
- [x] Declarar las firmas y propiedades acordadas, manteniendo la compilación después de cada cambio. Las consultas de colecciones son propiedades con get (`equipos`, `creadores`), sin setter; los cálculos se mantienen como métodos. Todos los enums están reunidos en `ENUMs.cs` por pedido del usuario. Los cuerpos provisionales lanzan `NotImplementedException`.
- [x] Completar `Creador`: inicialización por constructor, especialidad, edad 14–30, seguidores en miles 0–`int.MaxValue`, valoración con decimales 1–10 y Estrella cuando seguidores >= 500 y valoración >= 8, sin redondeo previo. `Configurar` se deja para la persistencia del proyecto completo por indicación del usuario.
- [x] Completar `Equipo`: validaciones, lista de creadores, búsqueda y registro con alias único dentro del equipo. Consulta mediante copia; rechazos con `RegistroException` sin alterar registros. `Configurar` queda para la persistencia integral.
- [x] Completar `Competencia`: lista de equipos, búsqueda, nombre único y respeto del cupo. Mantener año 2000–2027 y máximo de equipos 1–10000. Constructor, getter, cupo, búsquedas y altas implementados y verificados; rechazos con `RegistroException` sin modificar registros. `Configurar` queda para persistencia.
- [x] Ajustar el estado de `Datos` al diseño y representar explícitamente la ausencia de competencia: propiedad `Competencia?` con get/set públicos y `null` inicial. Los métodos de persistencia siguen pendientes en la etapa 5.
- [x] Implementar, uno por vez, promedio de valoración, cantidad de Streamers, total de seguidores y creador más popular. Verificados vacíos, precisión decimal, total mayor que `int.MaxValue` y empates por orden de registro.
- [x] Implementar comparación de promedios, equipo con más seguidores y TOP 3 según la tabla de reglas comprobadas. Se conserva precisión decimal y se comparan totales `long`. TOP 3 por promedio descendente y nombre con comparador `es-AR` sin distinguir mayúsculas e inserción estable para equivalentes culturales; coincide con la muestra observada `Aguila`, `águila`, `Águila`. Verificados vacíos, empates y menos de tres candidatos. El contraste de esa muestra no implica equivalencia universal con el original.

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
- [ ] Generar el README.md
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
