# Registro de trabajo con IA

Registro cronológico de pedidos, respuestas y cambios. Se incluyen las interacciones anteriores disponibles en esta conversación; no se reconstruyen conversaciones no disponibles. Las instrucciones generales del proyecto están en `AGENTS.md` y `IA/Contexto.md`.

## 2026-11-02 — Push solicitado

### Pedido del usuario

> has el push

### Respuestas del agente

> Voy a revisar los cambios pendientes, la cuenta de GitHub y los permisos del repositorio antes de hacer el push.

> Git intentó usar las credenciales de `julianweitzman` y GitHub rechazó el push. La cuenta activa en `gh`, `51215927-spec`, tiene permisos de administrador; voy a reintentar usando esas credenciales.

> Push realizado correctamente a `origin/main`. Se subió el commit `57bded4`.

### Acciones y resultado

- Se revisaron el estado de Git, las ramas y el remoto: `main` tenía un commit pendiente y no había cambios locales.
- Se verificaron la cuenta y los permisos con `gh auth status` y `gh repo view --json nameWithOwner,viewerPermission`. La consulta requirió acceso fuera del entorno restringido; se confirmó permiso `ADMIN` para `51215927-spec`.
- El primer push falló con HTTP 403 porque Git utilizó la cuenta `julianweitzman`.
- Tras volver a verificar los permisos, se ejecutó `git -c credential.helper= -c "credential.helper=!gh auth git-credential" push origin main` correctamente.
- Se publicó el commit existente `57bded4` en `origin/main`. No se modificaron archivos ni se crearon commits durante esa tarea.

## 2026-11-02 — Creación del registro y regla permanente

### Pedido del usuario

> Crea un Log.md. En ese archivo debes guardar todos los cambioa que hases, que te pido y que respondes. Agrega esta intruccion a AGENTS.md

### Respuestas del agente

> Voy a crear `IA/Log.md` para registrar tus pedidos, mis respuestas y los cambios realizados, y agregaré esa instrucción a `AGENTS.md`.

Respuesta final prevista tras verificar la publicación:

> Creé `IA/Log.md` con los pedidos, mis respuestas y los cambios realizados. También agregué la instrucción a `AGENTS.md`. Ambos archivos quedaron publicados en GitHub.

### Cambios y motivo

- `IA/Log.md`: creado para conservar los pedidos, las respuestas y las acciones de esta conversación. Se ubica en `IA/` siguiendo la regla del repositorio sobre registros de uso de IA.
- `AGENTS.md`: agregada la obligación de actualizar este registro en cada interacción, conservar el historial y documentar cambios, verificaciones, commits y pushes.

### Verificaciones y publicación

- `git diff --check`: correcto. Se revisaron los archivos afectados; el cambio es documental y no modifica la aplicación.
- Se verificaron la cuenta activa `51215927-spec` y sus permisos `ADMIN` mediante GitHub CLI antes de publicar.
- El entorno restringido impidió escribir `.git/index.lock`; se repitieron el agregado y el commit con permisos ampliados.
- Commit `94f408d` creado y publicado correctamente en `origin/main` con los dos archivos.
- Se agrega esta actualización documental para registrar el resultado de la publicación y la respuesta final; su commit queda identificado en el historial de Git.

## 2026-11-02 — Revisión de tareas pendientes

### Pedido del usuario

> Que mas queda por hacer?

### Respuesta de progreso

> Voy a revisar la consigna, el código y el estado del repositorio para decirte qué falta y qué ya está resuelto.

### Revisión y verificaciones

- Se revisaron las instrucciones, IA/plan.md, IA/Analisis.md, IA/Contexto.md y las clases de la aplicación. El árbol de trabajo estaba limpio y main sincronizada con origin/main.
- El análisis del ejecutable y el plan ya existen; faltan pruebas puntuales y IA/Arquitectura.md.
- Se ejecutó dotnet build "ACTIVIDAD ESPECIAL.sln": restauración correcta; compilación fallida con tres errores CS0161 en CrearCompetencia, CrearEquipo y CrearCreador, y dos advertencias. El problema de carpeta NuGet mencionado en el plan no se reprodujo.
- Se confirmó que los casos 1 a 11 del menú están vacíos, el TOP 3 no está implementado y la lectura está bloqueada sin competencia. Faltan listas, operaciones, persistencia y proyecto de pruebas.
- Se verificaron gh auth status y gh repo view: cuenta activa 51215927-spec con permiso ADMIN. El bloqueo de publicación mencionado en el plan no refleja el estado actual.
- Único archivo modificado: IA/Log.md, para registrar esta consulta. No se modificó código ni se implementaron funcionalidades. El commit y el push de este registro se verifican a continuación; sus identificadores quedan en el historial Git.

### Respuesta final

Ya están hechos el **análisis del ejecutable y el plan**. Falta principalmente la implementación:

1. **Completar las pruebas pendientes del original:** fechas límite, confirmaciones, archivos dañados y algunos empates.
2. **Crear `IA/Arquitectura.md`** con clases, responsabilidades, métodos y UML antes de implementar.
3. **Recuperar la compilación:** comprobé que falla porque `CrearCompetencia`, `CrearEquipo` y `CrearCreador` no devuelven valores. La restauración de dependencias ya funciona.
4. **Completar el dominio:** listas de equipos y creadores, validaciones, búsquedas, estadísticas, regla Estrella y rankings.
5. **Conectar el menú:** las opciones 1 a 11 están vacías; también falta el TOP 3 al salir y permitir leer sin competencia previa.
6. **Implementar guardado y lectura con `DataBases`**, conservando datos y relaciones ante errores.
7. **Agregar pruebas unitarias, verificar los flujos y preparar la entrega:** ZIP completo y Google Docs con las consultas y respuestas de IA.

El próximo paso es **resolver las dudas que afecten al diseño y escribir la arquitectura**. El detalle está en `IA/plan.md`.

## 2026-11-02 — Actualizar el plan y continuar la investigación

### Pedido del usuario

> Anota esto en el plan, la parte del uml no es necesaria. Con Arquitectura.md basta. Tras ello avansa con el paso 1

### Respuestas del agente

> Voy a actualizar el plan para quitar el requisito de UML y luego continuar con las pruebas pendientes del ejecutable original, registrando los resultados comprobados.

### Cambios iniciales

- IA/plan.md: incorporada la lista actual de pendientes, eliminada la obligación de UML por indicación expresa del usuario y actualizado el estado de compilación y publicación a partir de la revisión anterior.
- Se continuará con el paso 1: investigación de casos pendientes del ejecutable, sin implementar todavía el dominio.
- Entorno comprobado: solo runtime .NET 10.0.12 instalado; Python 3.13 disponible. Se conservarán los archivos originales y se probará persistencia sobre una copia.
- Este cambio documental se verifica con git diff --check y se publica antes de continuar; resultado en las siguientes entradas y en el historial Git.

### Avances y respuestas posteriores de la investigación (02/11/2026)

El cambio inicial del plan y su registro se publicaron correctamente en el commit `718482f`.

> El plan ya quedó actualizado y publicado: alcanza con `Arquitectura.md`, sin UML. Voy a probar las validaciones, las confirmaciones y los errores de lectura sobre una copia del ejecutable y sus datos, para conservar los originales.

> Ya pude preparar un runtime .NET 8 local para contrastar las pruebas anteriores, que se habían hecho con .NET 10. También estoy guardando las entradas y salidas de cada ensayo para distinguir los resultados observados de las reglas todavía pendientes.

> Encontré una diferencia importante para el diseño: el original acepta valoraciones decimales, como `8,5`; el código actual usa `int`. También confirmé que `S` mayúscula acepta el reemplazo y que un equipo vacío no puede compararse con uno que tiene creadores. Ahora sigo con archivos incompletos, duplicados y fallos después de filas válidas.

> Las pruebas de lectura conservaron los datos en memoria incluso cuando el archivo tenía una fila válida seguida de otra corrupta. El original también suma correctamente más de `int.MaxValue` seguidores: mostró `2147483648` mil. Estoy cerrando los casos de decimales, orden del TOP 3 y fallos de escritura para documentar las reglas que necesitará la arquitectura.

### Archivos y herramientas creados

- Se creó IA/Investigacion/ para las pruebas de caja negra: copia local del ejecutable, scripts Python de automatización, entradas enviadas, salidas capturadas y resultados JSON.
- IA/Investigacion/probar_original.py: controla una pseudoterminal e interactúa con el menú real. IA/Investigacion/matrices.py: prepara datos y recorre escenarios sobre esa copia. Son herramientas auxiliares de investigación, no código de la aplicación académica.
- Se instaló pywinpty 3.1.2 en IA/Investigacion/herramientas y .NET Runtime 8.0.33 x64 en su subcarpeta dotnet8 mediante el instalador oficial; no se modificó el runtime global.
- La copia StreamORT/, las herramientas y __pycache__/ están excluidas mediante un .gitignore local. Se conservaron capturas y entradas de los ensayos.
- Se ajustó la automatización al comprobar que el menú conserva la opción seleccionada; un primer intento navegaba a opciones distintas. Otro ensayo se corrigió porque el mensaje de validación de valoración pide un decimal, no un entero. Un primer conjunto de ranking tenía Beta/beta y la lectura lo rechazó correctamente como duplicado; se sustituyó por nombres admitidos antes de medir el orden. Los ensayos corregidos se repitieron; no se presentan los intentos fallidos de automatización como reglas funcionales.
- Quedaron registrados 12 grupos de pruebas: confirmaciones/ausencias; entradas/fechas/Unicode; matriz de persistencia; totales; ranking Unicode; redondeo; decimales; estados vacíos/cupos; escritura bloqueada por directorio; archivo de solo lectura; regresión .NET 8; regresión .NET 10. Los resultados se consolidarán en IA/Analisis.md y el plan, sin declarar una matriz exhaustiva de toda entrada posible.

## 2026-11-02 — Consulta sobre el registro y la carpeta de investigación

### Pedido del usuario

> Guardaeste estas cosas en el log.md? Si no, haslo
> Que es Investigacion/

### Respuesta de progreso

> El pedido y la primera actualización ya estaban en `IA/Log.md`, pero faltaba registrar los avances y resultados posteriores. Voy a agregarlos ahora y comprobar qué contiene `IA/Investigacion/` para explicártelo.

El usuario interrumpió antes de que se escribiera esta actualización; se conserva aquí el contenido disponible y se registra ahora.

### Nueva consulta del usuario

> Que es investigacion y para que lo creaste?

### Respuesta del agente

`IA/Investigacion/` es una carpeta que creé para **probar el ejecutable original sin modificarlo ni tocar sus datos**.

Contiene:

- Una copia de StreamORT para ensayar archivos dañados y errores de guardado.
- Scripts de Python que automatizan las entradas del menú.
- Las entradas, salidas y resultados de las pruebas.
- Herramientas locales, incluido .NET 8 para ejecutarlo con la versión correspondiente.

La creé para avanzar con el paso 1 del plan y comprobar las reglas que todavía faltaban investigar. **No forma parte de la aplicación C# ni es una funcionalidad de la entrega**; sirve como apoyo y evidencia del análisis.

## 2026-11-02 — Continuación del cierre de investigación

### Pedido del usuario

> sigue

### Respuesta de progreso

> Voy a completar el registro en `IA/Log.md`, documentar los resultados de las pruebas y actualizar el plan. También dejaré una explicación dentro de `IA/Investigacion/` sobre su contenido y cómo repetir los ensayos.

### Acciones iniciales

- Se comprobó que la ejecución pendiente finalizó y dejó registrado el grupo de regresión en .NET 10.
- Se revisaron el estado de Git, el registro, el plan y las capturas para incorporar los avances que no se habían anotado antes de las interrupciones.
- Se conserva el historial existente y se registran solamente los pedidos y respuestas disponibles.

### Consolidación de resultados y verificaciones

> El registro ya incluye los avances y las consultas sobre `Investigacion/`. Confirmé además que la misma prueba de estadísticas y ranking dio resultados idénticos en .NET 8 y .NET 10. Hay otro hallazgo importante: si falla la escritura del último archivo, el original puede dejar un guardado parcial; la reconstrucción deberá proteger el guardado anterior.

- IA/Analisis.md: incorporados los doce grupos de investigación, valoraciones decimales y precisión, confirmaciones s/S, cancelaciones, fechas, búsquedas Unicode, totales amplios, matriz de lectura y errores de escritura. Se corrigieron afirmaciones anteriores incompletas, conservando la distinción entre pruebas originales y ampliación.
- IA/Pruebas-StreamORT.md: agregados ocho extractos de evidencia y sus referencias a los grupos completos.
- IA/plan.md: marcada la ampliación del paso 1 con el alcance realmente ensayado; incorporadas reglas nuevas y límites. Arquitectura.md queda como siguiente paso, sin UML. La implementación sigue pendiente.
- AGENTS.md: alineada la guía con la indicación del usuario de no requerir UML y con los resultados comprobados sobre valoración decimal y confirmación con S.
- IA/Investigacion/README.md: explicado qué contiene la carpeta, su propósito auxiliar, instalación local, ejecución secuencial, formato de entradas y límites de las capturas. Se conservan dos scripts, doce capturas legibles, doce registros de entradas y resultados.json. Los binarios, copia de trabajo, cachés y capturas crudas/diagnósticos quedan excluidos de Git.
- Se redujeron los redibujados del menú en las capturas legibles sin alterar las entradas originales. Se corrigió un problema de codificación ASCII al generar los títulos de los extractos; los documentos finales conservan los acentos. Una primera comprobación de evidencia falló por ese mismo problema en el texto de la aserción; al ejecutar la comprobación en UTF-8 pasó.
- Verificación: Python compiló los dos scripts; los doce registros de entradas y resultados JSON son legibles. Hay 22 variantes de lectura: dos correctas y veinte con error, todas estas últimas con la competencia y el creador anteriores conservados.
- Se comprobó igualdad de los siete resultados normalizados de la regresión .NET 8/.NET 10, total de 2147483648 y ganador correcto entre promedios redondeados igual. En el fallo de escritura por solo lectura se comprobó que creadores.txt mantuvo sus bytes mientras los otros archivos ya se habían reemplazado.
- SHA-256 de original y copia de StreamORT.dll idéntico: 20ccccafdfcc2644de58a5bea8fc1328ebdb84f58bbfcf7cfe8d4a64716f9603. git diff confirmó que Ejecutable/, la aplicación C# y las bibliotecas no tienen cambios. No se repitió la compilación C# porque no se modificó código ni configuración; permanece el fallo previo por los tres métodos sin retorno.
- git diff --check correcto antes de preparar la publicación. No se afirma cobertura exhaustiva, longitud máxima universal, comparador Unicode interno ni tolerancia a un corte de energía.

### Respuesta final prevista tras verificar publicación

Terminé el paso 1 y actualicé el plan, el análisis y el registro completo en IA/Log.md.

- Quedaron documentados 12 grupos de pruebas.
- Se confirmó que la valoración admite decimales y que los totales pueden superar int.MaxValue.
- Se comprobaron errores de lectura, cancelaciones y guardado parcial del original.
- IA/Investigacion/ ahora tiene un README que explica su contenido y cómo repetir los ensayos.

Los cambios quedaron publicados en GitHub. El siguiente paso es crear IA/Arquitectura.md, sin UML.

### Resultado de publicación

- Cuenta verificada antes del push: 51215927-spec, permiso ADMIN.
- Commit d643edd, «Documentar pruebas pendientes del ejecutable original»: creado correctamente con 34 archivos de documentación, scripts auxiliares y evidencia; git diff --cached --check correcto.
- Push del commit d643edd a origin/main: correcto. No se incluyeron binarios, dependencias, copia de trabajo ni capturas crudas.
- Esta entrada registra el resultado comprobado y se incorpora mediante un commit documental de cierre; su identificador y publicación quedan verificables en el historial de Git.
