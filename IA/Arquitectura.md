# Arquitectura de la reconstrucción de StreamORT

Fecha: 03/11/2026. Estado: contratos declarados con cuerpos provisionales; implementación funcional pendiente, sin UML.

## Alcance y referencias

La aplicación será una consola C# para .NET 8, con una sola competencia activa. Este diseño parte de [Analisis.md](Analisis.md), [Pruebas-StreamORT.md](Pruebas-StreamORT.md), [Contexto.md](Contexto.md) y del código actual de la aplicación y sus bibliotecas. No se volvió a ejecutar el original en esta tarea: se utiliza la investigación ya documentada.

Las reglas funcionales descritas como comprobadas proceden de esos ensayos. Los tipos, las firmas, el reparto de responsabilidades y la protección del guardado son propuestas para la reconstrucción; no describen clases internas del original. No se agregan edición, eliminación, partidas ni administración simultánea de competencias.

## Estado actual

- `Competencia` tiene datos generales y valida año y máximo de equipos, pero no contiene equipos.
- `Equipo` tiene nombre, fecha y categoría, sin lista de creadores ni validaciones.
- `Creador` valida edad y su constructor recibe los siete datos del diseño; valoración es `decimal` y tiene especialidad. Faltan las validaciones adicionales y Estrella.
- `Datos` expone una propiedad `Competencia?` con escritura privada y declara las operaciones de reemplazo y persistencia, aún provisionales.
- `Program` muestra el menú, pero los casos 1 a 11 están vacíos y falta el TOP 3. Su control de acceso bloquea incorrectamente la lectura sin competencia.
- Los tres métodos de creación y los nuevos métodos del diseño tienen cuerpos provisionales con `NotImplementedException`, sin llamadas desde el menú. Las 39 firmas documentadas de constructores y métodos están declaradas; las clases persistidas implementan `IEntidad<T>` mediante `Configurar` provisional. La solución compila sin errores y mantiene advertencias NU1900, CA1416 y CS0219.

## Organización y propiedad de los objetos

`Program` recibe entradas, invoca operaciones y muestra resultados. `Datos` conserva la competencia activa y coordina los archivos. `Competencia` es dueña de una lista de equipos; cada `Equipo` es dueño de una lista de creadores. `Creador` conserva sus datos y calcula si es estrella.

Se usarán campos privados `List<Equipo> equipos` y `List<Creador> creadores`, inicializados en los constructores. Para consultar las colecciones, se devolverán copias con `ObtenerEquipos()` y `ObtenerCreadores()`: agregar o quitar elementos de esas copias no alterará las listas del dominio. Las altas pasarán por los métodos responsables de validar duplicados y cupos.

No habrá listas globales de equipos o creadores en `Datos`. Las tablas de `DataBases` serán estructuras temporales para guardar o reconstruir el mismo conjunto de objetos, nunca un segundo estado editable de la aplicación. Se conserva el orden de registro y carga.

Las clases de dominio no leen teclas ni imprimen mensajes. Devuelven objetos, números, listas o resultados de validación; `Program` decide su presentación.

## Tipos y criterios comunes

| Dato | Tipo propuesto | Motivo |
| --- | --- | --- |
| Nombres, patrocinador y alias | `string` | Recortar espacios exteriores y rechazar vacío o solo espacios. Conservar espacios interiores y tildes. |
| Año, cupo y edad | `int` | Rangos comprobados: 2000–2027, 1–10000 y 14–30. |
| Fecha de creación | `DateOnly` | No tiene hora y no puede ser posterior a hoy. |
| Seguidores de un creador | `int` | Entre 0 e `int.MaxValue`, en miles. |
| Total de seguidores | `long` | Se comprobó un total de 2147483648 mil. Convertir a `long` antes de sumar. |
| Valoración y promedio | `decimal` | Admitir decimales y conservar el valor antes del formato visual. Es una elección del diseño, no un tipo identificado en el original. |
| Competencia o búsqueda ausente | `Competencia?`, `Equipo?`, `Creador?` | Representar explícitamente la ausencia con `null`. |

No redondear antes de calcular Estrella, comparar o ordenar. Por ejemplo, 7,999 no cumple valoración >= 8, y 8,004 supera a 8,001 aunque ambos promedios se muestren como 8,00. `decimal` tiene precisión finita: no se afirma que represente cualquier longitud de entrada; se verificará al menos la precisión de las muestras investigadas.

Para búsquedas y duplicados se propone `StringComparison.OrdinalIgnoreCase` después de `Trim()`. No quitar tildes, normalizar acentos combinantes ni comprimir espacios interiores. Esta elección debe contrastarse con las muestras Unicode: los ensayos no identifican el comparador universal del original.

Para ordenar nombres empatados en el TOP 3 se propone un comparador cultural explícito `es-AR`, sin distinguir mayúsculas. Se comprobará el orden de la muestra `Aguila`, `águila`, `Águila`; si difiere, investigar antes de cambiar la regla. El criterio cultural es una propuesta, no un resultado demostrado por caja negra.

## Clases y contratos propuestos

Las firmas siguientes están declaradas en el proyecto. Los métodos nuevos tienen cuerpos provisionales y la lógica descrita en sus contratos sigue pendiente. Los miembros indicados como públicos pertenecen a las clases `internal` existentes.

### Creador

Responsabilidad: representar un creador válido y resolver sus reglas individuales.

Propiedades de solo lectura: `string nombre`, `string alias`, `string nombreEquipo`, `int edad`, `int seguidores`, `decimal valoracion` y `Especialidad especialidad`. Se conserva `nombreEquipo` para persistir la asociación existente; `Equipo` verificará que coincida con su nombre y no habrá una operación para trasladarlo.

```csharp
public Creador(string nombre, string alias, string nombreEquipo, int edad,
    int seguidores, decimal valoracion, Especialidad especialidad);
public bool EsEstrella();
public static void Configurar(ConstructorEntidad<Creador> entidad);
```

El constructor inicializa todos los atributos, valida textos, edad 14–30, seguidores >= 0, valoración 1–10 y especialidad definida. Los límites de `int` forman parte del tipo de seguidores. La unicidad del alias corresponde al equipo, no al constructor del creador.

`EsEstrella()` devuelve `seguidores >= 500 && valoracion >= 8`, sin redondear ni condicionar por especialidad. `Configurar` registra únicamente los siete datos persistidos mediante `IEntidad<Creador>`; no guarda Estrella.

### Equipo

Responsabilidad: representar un equipo, gestionar sus creadores y calcular estadísticas.

Propiedades de solo lectura: `string nombre`, `DateOnly fechaDeCreacion`, `Categoria categoria`. Campo privado: `List<Creador> creadores`.

```csharp
public Equipo(string nombre, DateOnly fechaDeCreacion, Categoria categoria);
public List<Creador> ObtenerCreadores();
public Creador? BuscarCreador(string alias);
public bool RegistrarCreador(Creador creador, out string error);
public decimal CalcularValoracionPromedio();
public int ContarStreamers();
public long CalcularTotalSeguidores();
public Creador? ObtenerCreadorMasPopular();
public static void Configurar(ConstructorEntidad<Equipo> entidad);
```

| Operación | Contrato |
| --- | --- |
| Constructor | Validar nombre requerido, fecha no futura y categoría definida; inicializar lista vacía. |
| `ObtenerCreadores` | Devolver copia de la lista, respetando orden de registro. |
| `BuscarCreador` | Buscar alias dentro del equipo con los criterios comunes; devolver `null` si no existe. |
| `RegistrarCreador` | Rechazar alias repetido, objeto nulo o asociación con otro equipo, sin modificar la lista. Devolver `false` y motivo; con éxito agregar, devolver `true` y error vacío. No inventar cupo de creadores. |
| `CalcularValoracionPromedio` | Sumar valoraciones y dividir por cantidad; devolver 0 si está vacío. |
| `ContarStreamers` | Contar exclusivamente especialidad `Streamer`. |
| `CalcularTotalSeguidores` | Sumar en miles con acumulador `long`. |
| `ObtenerCreadorMasPopular` | Mayor número de seguidores; en empate conservar el primero. Devolver `null` si está vacío. |
| `Configurar` | Mapear nombre, fecha y categoría; excluir la lista de creadores. |

### Competencia

Responsabilidad: datos generales, cupos, altas y búsquedas de equipos, comparación y rankings.

Propiedades de solo lectura existentes: `string nombre`, `string patrocinador`, `int año`, `int cantMaxEquipos`. Campo nuevo privado: `List<Equipo> equipos`.

```csharp
public Competencia(string nombre, string patrocinador, int año, int cantMaxEquipos);
public List<Equipo> ObtenerEquipos();
public bool HayCupo();
public Equipo? BuscarEquipo(string nombre);
public bool RegistrarEquipo(Equipo equipo, out string error);
public ResultadoComparacion CompararEquipos(string nombrePrimero, string nombreSegundo);
public Equipo? ObtenerEquipoConMasSeguidores();
public List<Equipo> ObtenerTop3();
public static void Configurar(ConstructorEntidad<Competencia> entidad);
```

El constructor mantiene los límites actuales, valida textos e inicializa la lista vacía. `ObtenerEquipos` devuelve una copia. `HayCupo` compara cantidad con máximo. `BuscarEquipo` devuelve `null` si no encuentra el nombre. `RegistrarEquipo` rechaza nulos, duplicados o cupo agotado devolviendo `false` y motivo, sin alterar el estado; con éxito agrega y devuelve `true` con error vacío.

`CompararEquipos` valida primero el primer equipo, después el segundo, que sean diferentes y que ambos tengan creadores. Devuelve un valor de `ResultadoComparacion`: `PrimerEquipoInexistente`, `SegundoEquipoInexistente`, `MismoEquipo`, `EquipoSinCreadores`, `Empate`, `GanaPrimero` o `GanaSegundo`. Se propone este enum para que `Program` no tenga que comparar textos ni recalcular ganadores. Los promedios para mostrar se consultan en los equipos encontrados.

`ObtenerEquipoConMasSeguidores` excluye equipos vacíos y conserva el primero entre los empatados. Participan los equipos con creadores y cero seguidores. Devuelve `null` cuando ninguno tiene creadores.

`ObtenerTop3` devuelve una lista nueva de hasta tres equipos con creadores, ordenada por promedio descendente y nombre en los empates. Una competencia sin candidatos devuelve una lista vacía. No altera el orden de registro de la lista original. Puede resolverse con recorridos y una lista auxiliar, sin exigir técnicas todavía no estudiadas.

`Configurar` mapea solo los cuatro datos generales. Los cupos ocupados se calculan contando equipos y no se persisten.

### Enumeraciones

Se conservan todas las enumeraciones en `ENUMs.cs` por indicación del usuario: `Categoria` con `Gaming`, `Humor`, `Musica`, `Entretenimiento`; `Especialidad` con `Streamer`, `Youtuber`, `Tiktoker`, `Influencer`; y `ResultadoComparacion` con los siete estados anteriores. Mostrar «Música» en la interfaz. No asignar significado a valores fuera de las enumeraciones; rechazarlos también al cargar archivos.

### Datos

Responsabilidad: conservar la única competencia activa y coordinar la persistencia, sin imprimir ni pedir confirmaciones.

```csharp
public static Competencia? competencia { get; private set; }
public static void ReemplazarCompetencia(Competencia nuevaCompetencia);
public static void Guardar(string directorio);
public static Competencia Leer(string directorio);
private static Modelo CrearModelo(string directorio);
```

`ReemplazarCompetencia` rechaza nulos y sustituye una sola referencia. `Program` confirma antes de llamarlo; el nuevo objeto ya debe estar completamente construido.

`Guardar` exige competencia activa y obtiene de ella los equipos y creadores. No cambia la memoria. `Leer` devuelve una competencia nueva completa y validada, sin cambiar `competencia`: solo después de una lectura correcta `Program` llama a `ReemplazarCompetencia`.

Los errores de archivos o formato se propagan para que `Program` los informe. No convertir un error en una competencia vacía ni ignorar filas inválidas. Los constructores lanzan `ArgumentException` para datos individuales inválidos; los rechazos habituales de altas usan `bool` y `out string`. Este reparto mantiene contratos sencillos y comprensibles.

### Program

Responsabilidad: navegación, entradas, confirmaciones y presentación usando `Utilidades`.

Se propone completar las firmas existentes y agregar auxiliares conforme se conecte cada flujo:

```csharp
static void Main(string[] args);
static Competencia CrearCompetencia();
static Equipo CrearEquipo();
static Creador CrearCreador(string nombreEquipo);
static string PedirTextoRequerido(string mensaje);
static DateOnly PedirFechaEquipo();
static decimal PedirValoracion();
static bool ConfirmarReemplazo(string mensaje);
static void MostrarCompetencia(Competencia competencia);
static void MostrarEquipo(Equipo equipo);
static void MostrarCreador(Creador creador);
static void MostrarEstadisticas(Equipo equipo);
static void MostrarComparacion(Competencia competencia);
static void MostrarTop3(Competencia? competencia);
```

`CrearCreador` recibe el nombre del equipo ya encontrado, evitando pedirlo dos veces. Los métodos de creación devuelven objetos válidos; los casos del menú registran esos objetos en su dueño. Cupos y duplicados deben consultarse antes de solicitar los campos restantes, según el orden observado, y comprobarse nuevamente en los métodos de alta.

`ConfirmarReemplazo` lee una tecla: `s` o `S` confirma; las otras teclas ensayadas cancelan. No usar directamente `PedirBool`, que actualmente exige `y/n` y repregunta.

`PedirFechaEquipo` usa `PedirString` y `DateOnly.TryParseExact` con `dd/MM/yyyy`, además de rechazar fechas futuras. `PedirDateOnly` usa un análisis flexible y no garantiza ese contrato.

`PedirValoracion` usa `PedirString` y `decimal.TryParse`, admitiendo coma o punto decimal sin separadores de miles y validando 1–10. No pasar por `PedirDouble` para después convertir a `decimal`, porque introduciría una conversión innecesaria antes de guardar. Los enteros usan `PedirInt`; las categorías y especialidades usan `PedirOpcionMenu` o `PedirEnum`, que ya lo utiliza. Validar textos en el constructor y también al solicitarlos permite repreguntar inmediatamente.

Las tablas se arman como `string[,]` para `MostrarTabla`. Promedios, unidades, «Sin creadores» y «Estrella: Sí/No» son presentación. Aplicar fondo negro a operaciones generales, azul a equipos y violeta a creadores, restaurando la pantalla al volver.

## Correspondencia con el menú

| Opción | Flujo propuesto |
| --- | --- |
| 1 | Confirmar si hay competencia, crear objeto y reemplazar referencia; los equipos anteriores dejan de formar parte del estado activo. |
| 2 | Consultar cupo y duplicado; crear equipo y registrarlo en la competencia. |
| 3 | Buscar equipo, consultar alias, crear creador y registrarlo en ese equipo. |
| 4 | Consultar competencia y recorrer equipos y creadores para mostrar tablas. |
| 5 | Buscar y mostrar equipo, incluidos sus creadores. |
| 6 | Buscar equipo y luego creador por alias; mostrar ficha. |
| 7 | Buscar equipo y mostrar los cuatro cálculos definidos en `Equipo`. |
| 8 | Pedir y validar el primer equipo antes de pedir el segundo; usar `CompararEquipos` y mostrar sus promedios. |
| 9 | Consultar `ObtenerEquipoConMasSeguidores` o informar ausencia de candidatos. |
| 10 | Invocar `Datos.Guardar`, informar éxito o error sin cambiar la memoria. |
| 11 | Confirmar si hay competencia; ejecutar `Datos.Leer` y reemplazar solo tras éxito completo. |
| 12 | Mostrar TOP 3 y esperar una tecla. No guardar automáticamente. |

Crear, leer y salir funcionan sin competencia previa. Las opciones 2 a 10 requieren competencia. Al iniciar no se cargan archivos automáticamente. Informar búsquedas fallidas y listas vacías con los mensajes respaldados por el análisis.

El original conserva selección y navegación circular. La firma pública actual de `PedirOpcionMenu` no recibe selección inicial; verificar qué comportamiento ofrece antes de conectar el menú. Si reproducirlo exige cambiar `Utilidades`, consultar al usuario. La prioridad sigue siendo usar la biblioteca aunque haya diferencias visuales.

## Persistencia con DataBases

### Formato y configuración

Se proponen `competencia.csv`, `equipos.csv` y `creadores.csv`, independientes de los `.txt` del original. No escribir en `Ejecutable/Datos` ni implementar importación Base64 como parte de esta tarea.

| Archivo | Columnas propuestas |
| --- | --- |
| `competencia.csv` | `nombre`, `patrocinador`, `año`, `cantMaxEquipos` |
| `equipos.csv` | `nombre`, `fechaDeCreacion`, `categoria` |
| `creadores.csv` | `nombre`, `alias`, `nombreEquipo`, `edad`, `seguidores`, `valoracion`, `especialidad` |

La biblioteca escribe encabezados, separador coma, escape de comillas y valores con cultura invariable. Soporta `DateOnly`, `decimal` y enums. No crear un parser CSV propio ni concatenar campos manualmente. No persistir listas ni resultados calculados.

Cada clase persistida implementará `IEntidad<T>` y su `Configurar`. El constructor de cada clase debe tener exactamente los parámetros correspondientes a las columnas, con nombres y tipos compatibles: `CsvManager` los exige al construir el modelo. Las firmas propuestas cumplen esa condición.

`CrearModelo` usa `new ConstructorModelo().Agregar<Competencia>().Agregar<Equipo>().Agregar<Creador>().Construir()`. Como `Configurar` es estático y no recibe directorio, se propone un campo privado temporal de configuración en `Datos`, accesible mediante una propiedad interna para formar las rutas absolutas. Solo se modifica mientras se construye el modelo y se restaura con `finally`; las rutas quedan resueltas en `Archivo`. Es una solución para esta consola secuencial, sin cambiar el directorio de trabajo global ni modificar la biblioteca; no se diseña para persistencia concurrente.

No utilizar `Modelo.Muchos` como almacenamiento del dominio: devuelve una copia y su resolución de claves usa `Equals`, que por sí sola no reproduce las búsquedas acordadas sin distinguir mayúsculas. Reconstruir las relaciones explícitamente con los métodos de alta.

### Lectura completa antes de reemplazar memoria

1. Crear un modelo local y cargar cada tabla con `Cargar<T>(exigirArchivo: true)`. `CargarTodo` permite archivos inexistentes y no ofrece este contrato.
2. Exigir exactamente una fila de competencia. Los archivos de equipos y creadores pueden tener solo encabezados cuando no hay registros; rechazar un archivo completamente vacío como política de integridad propuesta, aunque la biblioteca lo interpreta como tabla vacía.
3. Registrar los equipos en la competencia temporal, validando cupo y nombres únicos.
4. Asociar cada creador por `nombreEquipo` y registrarlo con validación de alias. Rechazar creadores sin equipo, filas inválidas y duplicados; conservar el orden de carga.
5. Devolver la competencia temporal solo cuando todo sea válido. Ante error, descartar esa construcción y mantener el estado activo anterior.

No cargar directamente sobre un modelo global activo: aunque una tabla conserva sus datos si falla su propia lectura, eso no convierte la carga de varias tablas en una operación indivisible.

### Guardado protegido

El código vigente escribe cada archivo mediante un temporal, pero el comentario y la implementación de `Modelo.GuardarTodo()` aclaran que **no es una transacción entre archivos**. No alcanza con llamar a ese método sobre la única copia válida.

Propuesta sin modificar `DataBases`:

1. Crear una carpeta de preparación única dentro del directorio de datos; construir allí un modelo y cargar tablas inicialmente vacías.
2. Agregar una fila de competencia, todos los equipos y todos los creadores a sus `Tabla<T>`, y llamar a `GuardarTodo` en esa carpeta nueva.
3. Releer ese conjunto y validar todos sus datos y relaciones antes de publicarlo.
4. Mantener el conjunto anterior en una carpeta de respaldo y promover la carpeta preparada como `actual`. Si la promoción falla, intentar restaurar el respaldo e informar el error; conservar el respaldo aunque la restauración falle.
5. Al leer, si falta `actual` por una promoción interrumpida, permitir recuperar el respaldo completo y validarlo. Un conjunto actual corrupto debe generar un error explícito, sin sustituirlo silenciosamente.

Usar operaciones de carpetas únicamente sobre rutas resueltas dentro del directorio de datos, con nombres internos definidos por la aplicación. No borrar la única copia válida. La secuencia de renombrados no equivale a una transacción del sistema; exige ensayos de recuperación y no permite prometer resistencia universal a cortes de energía. La garantía propuesta es conservar una copia completa recuperable ante los fallos de escritura controlados.

## Implementación pendiente y orden recomendado

1. Compilación recuperada y contratos declarados. Los cuerpos provisionales permanecen desconectados del menú hasta implementarse.
2. Implementar `Creador`, `Equipo` y `Competencia`, sus listas, validaciones, estadísticas, comparación y TOP 3. Enumeraciones ya declaradas en `ENUMs.cs`.
3. Adaptar `Datos` para representar la ausencia de competencia y reemplazar el estado mediante una sola referencia.
4. Conectar las 12 opciones, corregir acceso a lectura, entradas, confirmaciones, tablas y colores. Comprobar navegación con la API disponible.
5. Implementar configuración CSV, guardado protegido, reconstrucción temporal y recuperación con copias de datos.
6. Crear un proyecto separado de pruebas unitarias. Cubrir límites, duplicados por equipo, cupos, vacíos, Estrella, precisión, totales amplios y desempates. Probar persistencia con directorios temporales, incluidos fallos durante la publicación y recuperación del respaldo.
7. Compilar, ejecutar pruebas y recorrer manualmente los flujos del menú contra la evidencia. Contrastar especialmente el comparador Unicode y el formato de redondeo, sin asumir equivalencia universal.
8. Actualizar documentos y registro de IA; preparar y comprobar el ZIP completo y reunir las consultas y respuestas en el Google Docs requerido.

No hace falta repetir toda la investigación inicial: agregar ensayos concretos cuando surja una duda que afecte una decisión. Mantener cambios atómicos, explicación, commit y push según las reglas del repositorio. Este documento describe los contratos declarados y la lógica pendiente; no acredita que las operaciones ya estén implementadas.
